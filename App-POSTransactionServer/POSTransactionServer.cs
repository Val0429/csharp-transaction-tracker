using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Device;
using Interface;
using Layout;
using PanelBase;

namespace App_POSTransactionServer
{
	public partial class POSTransactionServer : AppClient
	{
		public UInt16 MaximumInstantPlaybackWindow = 4;// = 0;

		private Form _form;
		public override Form Form
		{
			get { return _form; }
			set
			{
				_form = _pts.Form = value;
                var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                value.Icon = name == "TransactionTracker" ?   Properties.Resources.icon : Properties.Resources.icon2;
			}
		}

		private ServerCredential _credential;
		public override ServerCredential Credential
		{
			get { return _credential; }
			set { _credential = _pts.Credential = value; }
		}
		public override String Language { get; set; }

		private readonly Stopwatch _watch = new Stopwatch();
		private readonly PTS _pts;
		public POSTransactionServer()
		{
			AppProperties = new AppClientProperties();
			_pts = new PTS();

			_watch.Reset();
			_watch.Start();

			IsInitialize = false;

			Pages = new Dictionary<String, IPage>();

			Localization = new Dictionary<String, String>
							   {
                                    {"Menu_Application", "Application"},
								   {"Menu_Bandwidth", "Bandwidth"},
								   {"Menu_Fullscreen", "Full Screen"},
								   {"Menu_About", "About"},
								   {"Menu_Setup", "Setup"},

								   {"Menu_Resolution", "Resolution"},
								   {"Menu_HidePanel", "Hide Panel"},
								   {"Menu_ShowPanel", "Show Panel"},
								   {"Menu_LockApplication", "Lock Application"},
								   {"Menu_SignOut", "Sign Out"},
								   {"Menu_OriginalStreaming", "Original Streaming"},
								   {"Menu_1M", "1Mbps"},
								   {"Menu_512K", "512Kbps"},
								   {"Menu_256K", "256Kbps"},
								   {"Menu_56K", "56Kbps"},
								   
                                   {"Menu_512KVGA", "512Kbps VGA"},
								   {"Menu_256KCIF", "256Kbps CIF"},
								   {"Menu_128KQCIF", "128Kbps QCIF"},
                                   {"Menu_Original", "Original"},

								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   {"MessageBox_Information", "Information"},
								   
								   {"Application_64Bit", " (64 Bit)"},
								   {"Application_POSTransactionServer", "POS Transaction Server"},

								   {"Application_ConnectionTimeout", "Connection Timeout"},
								   {"Application_Ver", "Ver. %1"},
								   {"Application_PageNotFound", "Page not found."},
								   {"Application_UserInformationNotFound", "User information not found."},
								   {"Application_GroupPermissionNotFound", "User group \"%1\" permission not found."},
								   {"Application_StorageUsage", "Usage %1%    %2 GB used    %3 GB free"},
								   {"Application_CannotConnect", "Can't connect to the server. Please login again."},
								   {"Application_SaveCompleted", "Save Completed"},
								   {"Application_PortChange", "The server port has been changed. Please sign in again using port %1."},
								   {"Application_SSLPortChange", "The server's SSL port has been changed. Please sign in again using port %1."},
								   {"Application_UserChange", "Current login acount has been modified. Please login again."},
								   {"Application_SaveTimeout", "Saving timeout. Some settings can't be saved to server."},
								   
								   {"Application_ConfirmLockApp", "Are you sure you want to lock application?"},
								   {"Application_ConfirmSignOut", "Are you sure you want to sign out?"},

								   {"Application_TimezoneChange", "Server's time zone has been modified.\r\nFrom \"%1\" to \"%2\"\r\nDo you want to login again to apply new time zone?"},
								   {"Application_DaylightChange", "Server's daylight saving has been modified.\r\nDo you want to login again to apply new daylight saving?"},
								   {"Application_SomeNVRUnavailable", "The following NVR can't connect:"},
								   {"Application_ServerIsNotPTS", "\"%1\" is not PTS."},

								   {"Application_UtilityInitError", "Utility Initialize Error"},

                                   {"Application_CPUUsage", "CPU %1%"},
							   };
			Localizations.Update(Localization);

			var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			Version = version.Major + "." + version.Minor;//.ToString().PadLeft(2, '0');// +"." + version.Build.ToString().PadLeft(2, '0');

			switch (_pts.ReleaseBrand)
			{
				case "Salient":
					Version = "4.5.7";
					Localization["Application_POSTransactionServer"] = "Salient TransactionTracker";
					break;

				default:
                    Version = version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');
					Localization["Application_POSTransactionServer"] = "POS Transaction Server";
					break;
			}

			_pts.Name = Localization["Application_POSTransactionServer"];
		}

		public override void Activate()
		{
            BasicDevice.DisplayDeviceId = _pts.Configure.DisplayDeviceId;
            DeviceGroup.DisplayGroupId = _pts.Configure.DisplayGroupId;

			_pts.OnLoadFailure -= PTSOnLoadFailure;

			ResetTitleBarText();

			//Check Apps Numbers
			if (_pts.Server.PageList.Count <= 0)
			{
				TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				Application.Exit();
				return;
			}

			if (_pts.User.Current == null || _pts.User.Current.Group == null)
			{
				TopMostMessageBox.Show(Localization["Application_UserInformationNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				Application.Exit();
				return;
			}

			if (_pts.User.Current.Group.Permissions.Count == 0)
			{
				TopMostMessageBox.Show(Localization["Application_GroupPermissionNotFound"].Replace("%1", _pts.User.Current.Group.Name), Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				Application.Exit();
				return;
			}

			CreatePage();

			//Check Apps Numbers
			if (Pages.Count <= 0)
			{
				TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//create unlock nvr form
			_unlockAppForm = new UnlockAppForm
			{
				App = this,
				User = _pts.User.Current,
				Icon = Form.Icon,
			};

			_unlockAppForm.OnCancel += UnlockAppFormOnCancel;
			_unlockAppForm.OnConfirm += UnlockAppFormOnConfirm;
			//-----------------------------------------

			RegistViewer(32);

            //is this is null , mean it's not load from client.ini , means it should load from general config
            if (StartupOption == null)
            {
                var temp = _pts.Configure.StartupOptions.Clone();
                if (temp.Enabled)
                    StartupOption = temp;
                else
                    StartupOption = new StartupOptions();
            }

			foreach (KeyValuePair<String, IPage> obj in Pages)
			{
				obj.Value.LoadConfig();
			}

			_timePanel.Invalidate();
			_tickDateTimeTimer.Enabled = true;

			Activate(Pages.First().Value);
			
			_pts.ListenNVREvent();
			_pts.ListenPOSEvent();

			//if (_pts.Configure.EnableJoystick && IntPtr.Size == 4) //only 32bit support joystick
            _pts.Utility.InitializeJoystick();

			_watch.Stop();
			Console.WriteLine(Environment.NewLine + _pts.Status + Environment.NewLine + @"Total App Startup: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

			_pts.Server.OnServerTimeZoneChange += NetworkVideoRecorderOnServerTimeZoneChange;
		}

		public override void Logout()
		{
			if (_isLock)
				UnlockApplication();

			base.Logout();
		}

        private void AddPage(String name, XmlElement node)
        {
            var page = new Page
            {
                App = this,
                Server = _pts,
                Name = name,
                PageNode = node,
            };

            if (!page.IsExists) return;

            Pages.Add(page.Name, page);

            //setup dont add to page switch panel.
            if (page.Name == "Setup") return;

            SwitchPagePanel.Controls.Add(page.Icon);
            page.Icon.BringToFront();
        }

		private void CreatePage()
		{
            //foreach (KeyValuePair<String, XmlElement> obj in _pts.Server.PageList)
            //{
            //    if (!_pts.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
            //        continue;

            //    if (Pages.ContainsKey(obj.Key)) continue;

            //    var page = new Page
            //    {
            //        App = this,
            //        Server = _pts,
            //        Name = obj.Key,
            //        PageNode = obj.Value,
            //    };

            //    if (!page.IsExists) continue;

            //    Pages.Add(page.Name, page);

            //    SwitchPagePanel.Controls.Add(page.Icon);
            //    page.Icon.BringToFront();
            //}

            foreach (var obj in _pts.Server.PageList)
            {
                if (!_pts.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
                    continue;

                //dupcated!
                if (Pages.ContainsKey(obj.Key)) continue;

                AddPage(obj.Key, obj.Value);
            }

            //get plug-ins page
            String plusinsPath = Environment.CurrentDirectory + "\\plug-ins\\";
            if (Directory.Exists(plusinsPath))
            {
                String[] files = Directory.GetFiles(plusinsPath);

                foreach (var file in files)
                {

                    XmlDocument xmldoc = Xml.LoadXmlFromFile(file);

                    var name = Xml.GetFirstElementValueByTagName(xmldoc, "PageName");
                    //dupcated!)
                    if (Pages.ContainsKey(name)) continue;

                    var root = xmldoc.CreateElement("Root");
                    root.AppendChild(Xml.CreateXmlElementWithText(xmldoc, "Config", Path.GetFileName(file)));

                    AddPage(name, root);
                }
            }

            if (SwitchPagePanel.Controls.Count > 1)
                SwitchPagePanel.Width = SwitchPagePanel.Controls.Count * 170;
            else
                SwitchPagePanel.Visible = false;
		}

		public override void ResetTitleBarText()
		{
			_form.Text = _credential.Domain + @":" + _credential.Port + @" - " + Localization["Application_POSTransactionServer"];

			if (IntPtr.Size == 8)
				_form.Text += Localization["Application_64Bit"];
		}

		protected override void FormShown(Object sender, EventArgs e)
		{
			//_form.Shown -= FormShown;
            base.FormShown(sender, e);

            if (!SwitchPagePanel.Visible || SwitchPagePanel.Controls.Count == 0) return;

            var width = SwitchPagePanel.Controls.Count * 170;
            var leftWidth = (HeaderPanel.Width - LogoPictureBox.Width - MenuStrip.Width - width) / 2;
            SwitchPagePanel.Location = new Point(LogoPictureBox.Location.X + LogoPictureBox.Width + leftWidth, 10);

			ShowUnavailableNVRMessage();
		}

		private Boolean _isAskingLogin;
		private void NetworkVideoRecorderOnServerTimeZoneChange(Object sender, EventArgs<String> location)
		{
			if (_isAskingLogin) return;
			_isAskingLogin = true;

			var result = TopMostMessageBox.Show(
				(location.Value != _pts.Server.Location)
					? Localization["Application_TimezoneChange"].Replace("%1", _pts.Server.Location).Replace("%2", location.Value)
					: Localization["Application_DaylightChange"], Localization["MessageBox_Information"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if(result == DialogResult.Yes)
			{
				OpenAnotherProcessAfterLogout = true;
				Logout();
				return;
			}

			if(result == DialogResult.No)
			{
				_pts.Server.OnServerTimeZoneChange -= NetworkVideoRecorderOnServerTimeZoneChange;
			}
			_isAskingLogin = false;
			//if (OnServerTimeZoneChange != null)
			//    OnServerTimeZoneChange(this, null);
		}

		public override void PopupInstantPlayback(IDevice device, UInt64 timecode)
		{
			if (!Pages.ContainsKey("Playback")) return;

			if (device == null || timecode == 0 || device.Server == null) return;
			if (!_pts.NVR.NVRs.ContainsKey(device.Server.Id) || !device.Server.Device.Devices.ContainsKey(device.Id)) return;

			if (MaximumInstantPlaybackWindow != 0)
			{
				if (GetInstantPlaybackCount() >= MaximumInstantPlaybackWindow)
					return;
			}

			//backward 10 secs
			timecode -= 10000;

			foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
			{
				foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
				{
					if (controlPanel.Control is IPopupStream)
					{
						((IPopupStream)controlPanel.Control).PopupInstantPlayback(device, timecode);
					}
				}
			}
			//Popup Window
			//var instantPlayback = new InstantPlayback
			//{
			//    App = this,
			//    Server = _pts,
			//    Camera = (ICamera)device,
			//    DateTime = DateTimes.ToDateTime(Convert.ToUInt64(timecode), _pts.Server.TimeZone),
			//    Icon = Form.Icon,
			//};

			//instantPlayback.Show();
		}

		protected virtual UInt16 GetInstantPlaybackCount()
		{
			UInt16 count = 0;
			foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
			{
				foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
				{
					if (controlPanel.Control is IPopupStream)
					{
						count += ((IPopupStream)controlPanel.Control).GetInstantPlaybackCount;
					}
				}
			}

			return count;
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

			if (_downloadCaseForm != null && _downloadCaseForm.IsDownloading)
				_downloadCaseForm.StopDownload();

			if (_pts.Utility != null)
				_pts.Utility.Quit();

			_pts.Logout();
			_tickDateTimeTimer.Enabled = false;

			AppProperties.SaveProperties();

			Deactivate();
		}

		private PrintImageForm _printImageForm;
		public override void PrintImage(List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime)
		{
			if (_printImageForm == null)
				_printImageForm = new PrintImageForm { App = this };

			_printImageForm.PrintImage(_pts, printDevices, printImages, dateTime);
		}

		private DownloadCaseForm _downloadCaseForm;
		public override void DownloadCase(IDevice[] usingDevices, DateTime start, DateTime end, XmlDocument xmlDoc)
		{
			if (_downloadCaseForm == null)
				_downloadCaseForm = new DownloadCaseForm { App = this, Server = _pts };

			if (xmlDoc == null)
			{
				_downloadCaseForm.DownloadCase(usingDevices, start, end);
				return;
			}

			_downloadCaseForm.AttachXmlDoc = xmlDoc;
		}

		public void ShowDownloadCaseQueue(Object sender, EventArgs e)
		{
			if (_downloadCaseForm == null)
				_downloadCaseForm = new DownloadCaseForm { App = this, Server = _pts };

			_downloadCaseForm.ShowDownloadCaseQueue();
		}

        public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
        {
            LogCurrentViewLayout();
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            LogCurrentViewLayout();
        }

        public void ExitFullScreen(Object sender, EventArgs e)
        {
            ExitFullScreen();
        }

        private void LogCurrentViewLayout()
        {
            foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
            {
                foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
                {
                    if (controlPanel.Control is IUpdateClientSetting)
                    {
                        ((IUpdateClientSetting)controlPanel.Control).UpdateClientSetting();
                    }
                }
            }
        }
	}
}
