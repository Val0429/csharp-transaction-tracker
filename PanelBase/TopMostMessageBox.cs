using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PanelBase
{
    public static class TopMostMessageBox
    {
        public static Form MainForm;
        public static DialogResult Show(String message)
        {
            return Show(message, String.Empty, MessageBoxButtons.OK);
        }

        public static DialogResult Show(String message, String title)
        {
            return Show(message, title, MessageBoxButtons.OK);
        }

        public static DialogResult Show(String message, String title, MessageBoxButtons buttons)
        {
            return Show(message, title, buttons, MessageBoxIcon.None);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public delegate DialogResult ShowDelegate(String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon);
        public static DialogResult Show(String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Screen screen = Screen.FromHandle(GetForegroundWindow());

            //Rectangle rect = SystemInformation.VirtualScreen;

            var topmostForm = new Form
            {
                Opacity = 0,
                ControlBox = false,
                Size = new Size(1, 1),
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,//FormStartPosition.CenterScreen
                //Location = new System.Drawing.Point(rect.Bottom + 10, rect.Right + 10),
                Location = new Point(screen.Bounds.X, screen.Bounds.Y),
            };

            topmostForm.Show();
            topmostForm.Focus();
            topmostForm.BringToFront();
            topmostForm.TopMost = true;
            var result = MessageBox.Show(topmostForm, message, title, buttons, icon);
            topmostForm.Dispose();

            try
            {
                return result;
            }
            finally
            {
                FocusMainForm();
            }
        }

        public delegate void FocusMainFormDelegate();
        public static void FocusMainForm()
        {
            if (MainForm == null) return;
            if (MainForm.InvokeRequired)
            {
                try
                {
                    MainForm.Invoke(new FocusMainFormDelegate(FocusMainForm));
                }
                catch (Exception)
                {
                }
                return;
            }
            MainForm.TopMost = true;
            //MainForm.Show();
            //MainForm.Focus();
            MainForm.BringToFront();
            MainForm.Activate();
            MainForm.TopMost = false;
        }
    }
}
