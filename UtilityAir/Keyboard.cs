using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UtilityAir
{
    partial class UtilityAir
    {
        #region Public Events
        private static event KeyPressEventHandler _keyPress;
        public static event KeyPressEventHandler GlobalKeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                _keyPress += value;
            }
            remove
            {
                _keyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler _keyUp;
        public static event KeyEventHandler GlobalKeyUp
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                _keyUp += value;
            }
            remove
            {
                _keyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler _keyDown;
        public static event KeyEventHandler GlobalKeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                _keyDown += value;
            }
            remove
            {
                _keyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }
        #endregion Public Events


        #region Utility Functions
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        private static int s_KeyboardHookHandle;
        private static HookProc s_KeyboardDelegate;

        // Keyboard Event Handler
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            bool handled = false;

            if (nCode >= 0)
            {
                KeyboardHookStruct s_keyboardHook =
                    (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                // KeyDown
                if (_keyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)s_keyboardHook.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    _keyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                // KeyPress
                if (_keyPress != null && wParam == WM_KEYDOWN)
                {
                    bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);

                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (
                        ToAscii(s_keyboardHook.VirtualKeyCode, s_keyboardHook.ScanCode, keyState, inBuffer,
                            s_keyboardHook.Flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        _keyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
                }

                // KeyUp
                if (_keyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)s_keyboardHook.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    _keyUp.Invoke(null, e);
                    handled = handled || e.Handled;
                }
            }

            // If event handled in application, don't handoff to other listeners
            if (handled) return -1;

            // forward to other application / form
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it's not installed
            if (s_KeyboardHookHandle == 0)
            {
                s_KeyboardDelegate = KeyboardHookProc;
                // install hook
                s_KeyboardHookHandle = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    s_KeyboardDelegate,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0])
                    , 0);

                if (s_KeyboardHookHandle != 0) return;

                // If SetWindowsHookEx failed
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            if (_keyDown == null && _keyUp == null && _keyPress == null)
            {
                ForceUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsubscribeFromGlobalKeyboardEvents()
        {
            if (s_KeyboardHookHandle != 0)
            {
                // uninstall hook
                int result = UnhookWindowsHookEx(s_KeyboardHookHandle);
                // reset handle
                s_KeyboardHookHandle = 0;
                // Free up for GC
                s_KeyboardDelegate = null;

                if (result != 0) return;

                // If failed unhook, throw
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }
        }
        #endregion Utility Functions


        #region Windows Dll Import
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32")]
        private static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        #endregion Windows Dll Import


        #region Helper Structure
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            public int VirtualKeyCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        #endregion Helper Structure
    }
}
