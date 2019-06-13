using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace App
{
	public partial class AppClient
	{
		public struct KBDLLHOOKSTRUCT { public int vkCode; int scanCode; public int flags; int time; int dwExtraInfo; }

		public delegate int HookProc(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// 設置掛鉤.
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		// 將之前設置的掛鉤移除。記得在應用程式結束前呼叫此函式.
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		// 呼叫下一個掛鉤處理常式（若不這麼做，會令其他掛鉤處理常式失效）.
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		const int WH_KEYBOARD_LL = 13;
		private static int m_HookHandle = 0;    // Hook handle
		private static HookProc m_KbdHookProc;            // 鍵盤掛鉤函式指標

		public void KeyboardHook()
		{
			if (!IsAdministrator) return;

			try
			{
				using (Process curProcess = Process.GetCurrentProcess())
				using (ProcessModule curModule = curProcess.MainModule)
				{

					m_KbdHookProc = new HookProc(KeyboardHookProc);
					m_HookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, m_KbdHookProc, GetModuleHandle(curModule.ModuleName), 0);
				}
			}
			catch (Exception)
			{
			}
		}

		public void ReleaseKeyboardHook()
		{
			if (!IsAdministrator) return;

			try
			{
				bool ret = UnhookWindowsHookEx(m_HookHandle);
				m_KbdHookProc = null;
				m_HookHandle = 0;
			}
			catch (Exception)
			{
			}
		}

		public static int KeyboardHookProc(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
		{
			if (nCode < 0)
				return CallNextHookEx(m_HookHandle, nCode, wParam, ref lParam);

            Console.WriteLine(@"lParam.vkCode:" + lParam.vkCode + ", lParam.flags: " + lParam.flags);
			switch (wParam.ToInt32())  
			{  
				case 256: // WM_KEYDOWN  
				case 257: // WM_KEYUP  
				case 260: // WM_SYSKEYDOWN  
				case 261: // WM_SYSKEYUP  
					// http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
					 if (
                        (lParam.vkCode == 32 && lParam.flags == 0) ||		// Space
                        (lParam.vkCode == 37 && lParam.flags == 1) ||		// Left
                        (lParam.vkCode == 39 && lParam.flags == 1) ||		// Right
						(lParam.vkCode == 0x1b && lParam.flags == 0) ||		// Ctrl+Esc
						(lParam.vkCode == 0x5b && lParam.flags == 1) ||		// LWin Key		VK_LWIN		0x5B   
						(lParam.vkCode == 0x5c && lParam.flags == 1) ||		// RWin Key		VK_RWIN		0x5C  		
						(lParam.vkCode == 0x5d && lParam.flags == 1) ||		// Apps Key		VK_APPS		0x5D   
						(lParam.vkCode == 0x09 && lParam.flags == 32) ||	// Alt+Tab		VK_TAB		0x09 
						(lParam.vkCode == 0x1b && lParam.flags == 32) ||	// Alt+Esc		VK_ESCAPE	0x1B  
						(lParam.vkCode == 0x73 && lParam.flags == 32) ||	// Alt+F4		VK_F4		0x73  
						(lParam.vkCode == 0x20 && lParam.flags == 32)		// Alt+Space	VK_SPACE	0x20 
					   )
						{
                            if (lParam.vkCode == 27 && lParam.flags == 0)
                            {
                                //just ESC, let it go
                            }
                            else
                            {
                                return 1;  
                            }
						}
						break;  
			}  
			return CallNextHookEx(0, nCode, wParam, ref lParam);  
		}
	}
}
