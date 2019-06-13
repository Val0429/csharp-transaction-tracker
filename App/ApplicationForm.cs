using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PanelBase;

namespace App
{
	public partial class ApplicationForm : Form
	{
		private AppClient _app;

		public AppClient App
		{
			get { return _app; }
			set { _app = value; }
		}


		protected readonly GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();

		public ApplicationForm()
		{
			InitializeComponent();

			Activated += FormActivated;
			Deactivate += FormDeactivate;
		}

		private readonly Stopwatch _watch = new Stopwatch();
		public virtual void Login()
		{
			_watch.Reset();
			_watch.Start();
			if (App.Login())
			{
				Trace.WriteLine(@"Login Completed: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

				_watch.Reset();
				_watch.Start();
				App.OnLogout += AppOnLogout;

				App.CheckDisplayLocation(this);

				//---- Added by Tulip for Restore Client
				if (App.StartupOption != null)
					App.StartupOption.LoadSetting();

				App.Activate();
				Trace.WriteLine(@"Activate Completed: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

				Application.AddMessageFilter(_globalMouseHandler);
				_globalMouseHandler.TheMouseMoved += App.GlobatMouseMoveHandler;

				Show();

				BringToFront();
				Focus();

				return;
			}

			App.CancelAutoLogin();
		}

		private void FormActivated(Object sender, EventArgs e)
		{
			if (App == null) return;

			Application.RemoveMessageFilter(_globalMouseHandler);
			Application.AddMessageFilter(_globalMouseHandler);
			_globalMouseHandler.TheMouseMoved -= App.GlobatMouseMoveHandler;
			_globalMouseHandler.TheMouseMoved += App.GlobatMouseMoveHandler;

			App.WindowFocusGet();
		}

		private void FormDeactivate(Object sender, EventArgs e)
		{
			Application.RemoveMessageFilter(_globalMouseHandler);
			_globalMouseHandler.TheMouseMoved -= App.GlobatMouseMoveHandler;

			App.WindowFocusLost();
		}

		public delegate void AppOnLogoutDelegate(Object sender, EventArgs e);
		public virtual void AppOnLogout(Object sender, EventArgs e)
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke(new AppOnLogoutDelegate(AppOnLogout), sender, e);
					return;
				}

				//ApplicationForms.HideLoadingIcon();
				//ApplicationForms.HideProgressBar();
				//Application.RaiseIdle(null);

				if (App != null && App.StartupOption != null)
				{
					App.StartupOption.LogoutProcess = true;
					App.StartupOption.ClearSetting();
				}

				FormClosing -= StandaloneNetworkVideoRecorderFormFormClosing;

				if (App != null && WindowState != FormWindowState.Minimized)
					App.SetDisplayLocation(this);

				Hide();
			}
			catch (Exception)
			{
			}

			//wake another ap after logout
			if (App != null && App.OpenAnotherProcessAfterLogout)
			{
				try
				{
					Process.Start(Process.GetCurrentProcess().MainModule.ModuleName);
				}
				catch (Exception)
				{
				}
			}
            else
			{
                try
                {
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception)
                {

                }
			}

            if (Application.MessageLoop)
            {
              // Use this since we are a WinForms app
                Application.Exit();
            }
            else
            {
              // Use this since we are a console app
                Environment.Exit(Environment.ExitCode);
            }

            //Environment.Exit(Environment.ExitCode);
		}

		public FormWindowState PreviousWindowState;
		public Size PreviousWindowSize { get; set; }

		[DllImport("user32.dll")]
		private static extern IntPtr GetFocus();

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
            if (App != null)
                App.IdleTimer = 0;

			try
			{
				var wndHandle = GetFocus();
				var focusedControl = FromChildHandle(wndHandle);
				if ((focusedControl is DateTimePicker) || (focusedControl is TextBox) || (focusedControl is ComboBox))
					return base.ProcessCmdKey(ref msg, keyData);
			}
			catch (Exception)
			{
			}

			if (App != null)
				App.KeyPress(keyData);

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void StandaloneNetworkVideoRecorderFormFormClosing(Object sender, FormClosingEventArgs e)
		{
			if (App != null && App.IsLock)
			{
				e.Cancel = true;
				return;
			}

			try
			{
				if (App != null && App.StartupOption != null)
				{
					App.StartupOption.LogoutProcess = true;
					App.StartupOption.ClearSetting();
				}
			}
			catch (Exception)
			{
			}

			if (App != null && WindowState != FormWindowState.Minimized)
				App.SetDisplayLocation(this);

			FormClosing -= StandaloneNetworkVideoRecorderFormFormClosing;

			Hide();

			try
			{
				if (App != null && App.IsInitialize)
				{
					App.Quit();
				}
			}
			catch (Exception)
			{
			}

			Environment.Exit(Environment.ExitCode);
		}
	}
}