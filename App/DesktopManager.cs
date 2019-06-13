using System;
using System.Runtime.InteropServices;

namespace App
{
	public partial class AppClient
	{
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll")]
		static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

		[DllImport("user32.dll")]
		static extern IntPtr FindWindow(string className, string windowText);

		[DllImport("User32.dll")]
		static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

		//private static readonly IntPtr HDesktop = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
		private static IntPtr _toolbarWnd;
		// try an alternate way, as mentioned on CodeProject by Earl Waylon Flinn
		IntPtr _startWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");

		private static IntPtr _desktopWnd;

		public void HideToolbar()
		{
			if (!IsAdministrator) return;

			try
			{
				_toolbarWnd = FindWindow("Shell_TrayWnd", null);
				_startWnd = FindWindow("Button", null);
				_desktopWnd = FindWindow("Progman", "Program Manager");

				ShowWindow(_toolbarWnd, 0);
				ShowWindow(_startWnd, 0);
				ShowWindow(_desktopWnd, 0);

				//Process taskMgr = new Process();
				//taskMgr.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
				//taskMgr.StartInfo.FileName = "taskmgr.exe";
				//taskMgr.StartInfo.CreateNoWindow = true;
				//taskMgr.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				//taskMgr.Start();
			}
			catch (Exception)
			{
			}
		}

		public void ShowToolbar()
		{
			if (!IsAdministrator) return;

			try
			{
				_toolbarWnd = FindWindow("Shell_TrayWnd", null);
				_startWnd = FindWindow("Button", null);
				_desktopWnd = FindWindow("Progman", "Program Manager");

				ShowWindow(_toolbarWnd, 1);
				ShowWindow(_startWnd, 1);
				ShowWindow(_desktopWnd, 1);

				//IntPtr taskManager = FindWindow("#32770", "Windows Task Manager");
				//SendMessage((int)taskManager, 0x0010, 0, 0);
			}
			catch (Exception)
			{
			}
		}
	}
}
