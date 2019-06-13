using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using Layout;
using PanelBase;

namespace App_VideoAnalyticsServer
{
	public partial class VideoAnalyticsServer : AppClient
	{
		private Form _form;
		public override Form Form
		{
			get { return _form; }
			set
			{
				_form = _vas.Form = value;
				value.Icon = Properties.Resources.icon;
			}
		}

		private ServerCredential _credential;
		public override ServerCredential Credential
		{
			get { return _credential; }
			set { _credential = _vas.Credential = value; }
		}
		public override String Language { get; set; }

		private readonly Stopwatch _watch = new Stopwatch();
		private readonly VAS _vas;
		public VideoAnalyticsServer()
		{
			var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			Version = version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');

			AppProperties = new AppClientProperties();
			_vas = new VAS();

			_watch.Reset();
			_watch.Start();

			IsInitialize = false;

			Pages = new Dictionary<String, IPage>();

			Localization = new Dictionary<String, String>
							   {
								   {"Menu_Application", "Application"},
								   {"Menu_HidePanel", "Hide Panel"},
								   {"Menu_ShowPanel", "Show Panel"},
								   {"Menu_LockApplication", "Lock Application"},
								   {"Menu_SignOut", "Sign Out"},

								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   {"MessageBox_Information", "Information"},
								   
								   {"Application_64Bit", " (64 Bit)"},
								   {"Application_VideoAnalyticsServer", "Video Analytics Server"},

								   {"Application_ConnectionTimeout", "Connection Timeout"},
								   {"Application_Ver", "Ver. %1"},
								   {"Application_PageNotFound", "Page not found."},
								   {"Application_UserInformationNotFound", "User information not found."},
								   {"Application_GroupPermissionNotFound", "User group \"%1\" permission not found."},
								   {"Application_CannotConnect", "Can't connect to the server. Please login again."},
								   {"Application_SaveCompleted", "Save Completed"},
								   {"Application_PortChange", "The server port has been changed. Please sign in again using port %1."},
								   {"Application_UserChange", "Current login acount has been modified. Please login again."},
								   {"Application_SaveTimeout", "Saving timeout. Some settings can't be saved to server."},
								   {"Application_ConfirmLockApp", "Are you sure you want to lock application?"},
								   {"Application_ConfirmSignOut", "Are you sure you want to sign out?"},
								   {"Application_SomeNVRUnavailable", "The following NVR can't connect:"},
								   {"Application_ServerIsNotVAS", "\"%1\" is not VAS."},
							   };
			Localizations.Update(Localization);

			_vas.Name = Localization["Application_VideoAnalyticsServer"];
		}

		public override void Activate()
		{
			_vas.OnLoadFailure -= NvrOnLoadFailure;

			ResetTitleBarText();

			//Check Apps Numbers
			if (_vas.Server.PageList.Count <= 0)
			{
				TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (_vas.User.Current == null || _vas.User.Current.Group == null)
			{
				TopMostMessageBox.Show(Localization["Application_UserInformationNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (_vas.User.Current.Group.Permissions.Count == 0)
			{
				TopMostMessageBox.Show(Localization["Application_GroupPermissionNotFound"].Replace("%1", _vas.User.Current.Group.Name), Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			CreatePage();

			if (SwitchPagePanel.Controls.Count <= 1)
				SwitchPagePanel.Visible = false;

			//Check Apps Numbers
			if (Pages.Count <= 0)
			{
				TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			foreach (KeyValuePair<String, IPage> obj in Pages)
			{
				obj.Value.LoadConfig();
			}
			_timePanel.Invalidate();
			_tickDateTimeTimer.Enabled = true;

			Activate(Pages.First().Value);

			_watch.Stop();
			Console.WriteLine(Environment.NewLine + _vas.Status + Environment.NewLine + @"Total App Startup: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));
		}

		private void CreatePage()
		{
			foreach (KeyValuePair<String, XmlElement> obj in _vas.Server.PageList)
			{
				if (!_vas.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
					continue;

				if (Pages.ContainsKey(obj.Key)) continue;

				var page = new Page
							   {
								   App = this,
								   Server = _vas,
								   Name = obj.Key,
								   PageNode = obj.Value,
							   };

				if (!page.IsExists) continue;

				Pages.Add(page.Name, page);

				SwitchPagePanel.Controls.Add(page.Icon);
				page.Icon.BringToFront();
			}
		}

		public override void ResetTitleBarText()
		{
			_form.Text = _credential.Domain + @":" + _credential.Port + @" - " + Localization["Application_VideoAnalyticsServer"];

			if (IntPtr.Size == 8)
				_form.Text += Localization["Application_64Bit"];
		}

		protected override void FormShown(Object sender, EventArgs e)
		{
			//_form.Shown -= FormShown;

			base.FormShown(sender, e);

			ShowUnavailableNVRMessage();
		}

		private delegate void QuitDelegate();
		public override void Quit()
		{
			if (_form.InvokeRequired)
			{
				try
				{
					_form.Invoke(new QuitDelegate(Quit));
				}
				catch (Exception)
				{
				}
				return;
			}

			_vas.Logout();

			AppProperties.SaveProperties();

			Deactivate();
		}
	}
}
