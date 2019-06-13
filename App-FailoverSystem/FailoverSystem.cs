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

namespace App_FailoverSystem
{
	public partial class FailoverSystem : AppClient
	{
		private Form _form;
		public override Form Form
		{
			get { return _form; }
			set
			{
				_form = _fos.Form = value;
				value.Icon = Properties.Resources.icon;
			}
		}

		private ServerCredential _credential;
		public override ServerCredential Credential
		{
			get { return _credential; }
			set { _credential = _fos.Credential = value; }
		}
		public override String Language { get; set; }

		private readonly Stopwatch _watch = new Stopwatch();
		private readonly FOS _fos;
		public FailoverSystem()
		{
			var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			Version = version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');

			AppProperties = new AppClientProperties();
			_fos = new FOS();

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
                                   {"Menu_Bandwidth", "Bandwidth"},
                                   {"Menu_Fullscreen", "Full Screen"},
                                   {"Menu_About", "About"},
								   {"Menu_Setup", "Setup"},
								   {"Menu_SignOut", "Sign Out"},

								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   {"MessageBox_Information", "Information"},
								   
								   {"Application_64Bit", " (64 Bit)"},
								   {"Application_FailoverSystem", "Failover System"},

								   {"Application_ConnectionTimeout", "Connection Timeout"},
								   {"Application_Ver", "Ver. %1"},
								   {"Application_PageNotFound", "Page not found."},
								   {"Application_UserInformationNotFound", "User information not found."},
								   {"Application_GroupPermissionNotFound", "User group \"%1\" permission not found."},
								   {"Application_StorageUsage", "Usage %1%    %2 GB used    %3 GB free"},
								   {"Application_CannotConnect", "Can't connect to the server. Please login again."},
								   {"Application_SaveCompleted", "Save Completed"},
								   {"Application_PortChange", "The server port has been changed. Please sign in again using port %1."},
								   {"Application_UserChange", "Current login acount has been modified. Please login again."},
								   {"Application_SaveTimeout", "Saving timeout. Some settings can't be saved to server."},
								   {"Application_ConfirmLockApp", "Are you sure you want to lock application?"},
								   {"Application_ConfirmSignOut", "Are you sure you want to sign out?"},
								   {"Application_SomeNVRUnavailable", "The following NVR can't connect:"},
								   {"Application_ServerIsNotFOS", "\"%1\" is not Failover System."},
							   };
			Localizations.Update(Localization);

			_fos.Name = Localization["Application_FailoverSystem"];
		}

		public override void Activate()
		{
			_fos.OnLoadFailure -= NvrOnLoadFailure;

			ResetTitleBarText();

			//Check Apps Numbers
			if (_fos.Server.PageList.Count <= 0)
			{
				TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (_fos.User.Current == null || _fos.User.Current.Group == null)
			{
				TopMostMessageBox.Show(Localization["Application_UserInformationNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (_fos.User.Current.Group.Permissions.Count == 0)
			{
				TopMostMessageBox.Show(Localization["Application_GroupPermissionNotFound"].Replace("%1", _fos.User.Current.Group.Name), Localization["MessageBox_Error"],
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
			_refreshFailoverStatusTimer.Enabled = true;

			Activate(Pages.First().Value);

			_watch.Stop();
			Console.WriteLine(Environment.NewLine + _fos.Status + Environment.NewLine + @"Total App Startup: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            //create unlock nvr form
            _unlockAppForm = new UnlockAppForm
            {
                App = this,
                User = _fos.User.Current,
                Icon = Form.Icon,
            };

            _unlockAppForm.OnCancel += UnlockAppFormOnCancel;
            _unlockAppForm.OnConfirm += UnlockAppFormOnConfirm;

			_fos.Server.OnStorageStatusUpdate += FailoverSystemOnStorageStatusUpdate;
		}

		private void CreatePage()
		{
			foreach (KeyValuePair<String, XmlElement> obj in _fos.Server.PageList)
			{
				if (!_fos.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
					continue;

				if (Pages.ContainsKey(obj.Key)) continue;

				var page = new Page
							   {
								   App = this,
								   Server = _fos,
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
			_form.Text = _credential.Domain + @":" + _credential.Port + @" - " + Localization["Application_FailoverSystem"];

			if (IntPtr.Size == 8)
				_form.Text += Localization["Application_64Bit"];
		}

		protected override void FormShown(Object sender, EventArgs e)
		{
			//_form.Shown -= FormShown;

			base.FormShown(sender, e);

			ShowUnavailableNVRMessage();
		}

		public void FailoverSystemOnStorageStatusUpdate(Object sender, EventArgs e)
		{
			_storagePanel.Invalidate();
		}

		//Quit is almost the same thing with logout, but don't clear login-info, and don't return back to login panel

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

			_fos.Logout();

			AppProperties.SaveProperties();

			Deactivate();
		}
	}
}
