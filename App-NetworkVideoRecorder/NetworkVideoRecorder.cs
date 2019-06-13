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
using DeviceConstant;
using Interface;
using Layout;
using PanelBase;
using ServerProfile;

using ApplicationForms = App.ApplicationForms;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder : AppClient
    {
        public override event EventHandler OnAppStarted;

        public UInt16 MaximumInstantPlaybackWindow = 4;
        public UInt16 MaximumInstantLiveWindow = 4;

        private ulong _playbackTimeCode = 0;
        public override ulong PlaybackTimeCode
        {
            get { return _playbackTimeCode; }
            set
            {
                if (StartupOption == null)
                    return;

                StartupOption.TimeCode = _playbackTimeCode = value;
                StartupOption.SaveSetting();
            }
        }

        public override Form Form
        {
            get { return Nvr.Form; }
            set
            {
                Nvr.Form = value;
                value.Icon = Properties.Resources.icon;
            }
        }

        private ServerCredential _credential;
        public override ServerCredential Credential
        {
            get { return _credential; }
            set { _credential = Nvr.Credential = value; }
        }

        public override String Language { get; set; }

        private readonly Stopwatch _watch = new Stopwatch();
        public NVR _nvr;

        protected NVR Nvr
        {
            get { return _nvr ?? (_nvr = CreateNvr()); }
            set { _nvr = value; }
        }

        public override void RemoveProperties()
        {
            AppProperties.RemoveProperties(Credential);
        }


        // Constructor
        public NetworkVideoRecorder()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Menu_Application", "Application"},
								   {"Menu_Bandwidth", "Bandwidth"},
								   {"Menu_Fullscreen", "Full Screen"},
								   {"Menu_About", "About"},
								   {"Menu_Setup", "Setup"},
								   {"Menu_HidePanel", "Hide Panel"},
								   {"Menu_ShowPanel", "Show Panel"},
								   {"Menu_LockApplication", "Lock Application"},
								   {"Menu_SignOut", "Sign Out"},
								   {"Menu_OriginalStreaming", "Original Streaming"},
								   {"Menu_1M", "1Mbps"},
								   {"Menu_512K", "512Kbps"},
								   {"Menu_256K", "256Kbps"},
								   {"Menu_56K", "56Kbps"},

								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   {"MessageBox_Information", "Information"},
								   
								   {"Application_64Bit", " (64 Bit)"},
								   {"Application_NetworkVideoRecorder", "Network Video Recorder"},

								   {"Application_ConnectionTimeout", "Connection Timeout"},
								   {"Application_Ver", "Version %1"},
								   {"Application_DevicePackVer", "Device Pack Version %1"},
								   {"Application_PageNotFound", "Page not found."},
								   {"Application_UserInformationNotFound", "User information not found."},
								   {"Application_GroupPermissionNotFound", "User group \"%1\" permission not found."},
								   {"Application_StorageUsage", "Usage %1%    %2 GB used    %3 GB free"},
								   {"Application_TotalBitrate", "Bitrate %1"},
								   {"Application_CannotConnect", "Can't connect to the server. Please login again."},
								   {"Application_SaveCompleted", "Save Completed"},
								   {"Application_PortChange", "The server port has been changed. Please sign in again using port %1."},
								   {"Application_SSLPortChange", "The server's SSL port has been changed. Please sign in again using port %1."},
								   {"Application_UserChange", "Current login acount has been modified. Please login again."},
								   {"Application_SaveTimeout", "Saving timeout. Some settings can't be saved to server."},

								   {"Application_ConfirmLockApp", "Please confirm to lock application."},
								   {"Application_ConfirmSignOut", "Are you sure you want to sign out?"},

								   {"Application_TimezoneChange", "Server's time zone has been modified.\r\nFrom \"%1\" to \"%2\"\r\nDo you want to login again to apply new time zone?"},
								   {"Application_DaylightChange", "Server's daylight saving has been modified.\r\nDo you want to login again to apply new daylight saving?"},
								   {"Application_ServerIsNotNVR", "\"%1\" is not NVR."},
								   
								   {"Application_ServerVersionRequire", "\"%1\" server version is %2, require server version %3."},

								   {"Application_UtilityInitError", "Utility Initialize Error"},

								   {"Application_CPUUsage", "CPU %1%"},
							   };
            Localizations.Update(Localization);

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Version = version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');
            if (version.Revision.ToString() != "0")
                Version += ("." + version.Revision.ToString().PadLeft(2, '0'));


            AppProperties = new AppClientProperties();

            _watch.Reset();
            _watch.Start();

            IsInitialize = false;

            Pages = new Dictionary<String, IPage>();
        }


        protected virtual NVR CreateNvr()
        {
            return new NVR { Name = Localization["Application_NetworkVideoRecorder"] };
        }

        public override void Activate()
        {
            BasicDevice.DisplayDeviceId = Nvr.Configure.DisplayDeviceId;
            DeviceGroup.DisplayGroupId = Nvr.Configure.DisplayGroupId;
            Nvr.OnLoadFailure -= NvrOnLoadFailure;

            ResetTitleBarText();

            if (Nvr.Server.PageList.Count == 0)
            {
                TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Nvr.User.Current == null || Nvr.User.Current.Group == null)
            {
                TopMostMessageBox.Show(Localization["Application_UserInformationNotFound"], Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Nvr.User.Current.Group.Permissions.Count == 0)
            {
                TopMostMessageBox.Show(Localization["Application_GroupPermissionNotFound"].Replace("%1", Nvr.User.Current.Group.Name), Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //version check
            //var devicePackVer = Convert.ToInt32(_nvr.Server.ServerVersion.Replace(".", ""));
            var devicePackVer = Nvr.Server.ServerVersion.Split('.')[0];
            if (devicePackVer != "2" && Nvr.Server.Platform == Platform.Windows)
            {
                TopMostMessageBox.Show(Localization["Application_ServerVersionRequire"].Replace("%1", Nvr.Credential.Domain).Replace("%2", Nvr.Server.ServerVersion).Replace("%3", "2.00.00"),
                    Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                AppProperties.DefaultAutoSignIn = false;
                AppProperties.SaveProperties();

                MenuStrip.Visible = false;
                CancelAutoLogin();
                Application.Exit();
                return;
            }

            if (Nvr.Server.Platform == Platform.Linux)
            {
                Version = "v1.00.07";
            }

            DevicePackVersion = Nvr.Server.DevicePackVersion;
            RemoveNoPermissionDevice();

            CreatePages();

            _hideLoadingTimer.Elapsed += HideLoading;
            _hideLoadingTimer.Interval = 500;
            _hideLoadingTimer.SynchronizingObject = Nvr.Form;

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
                User = Nvr.User.Current,
                Icon = Form.Icon,
            };

            _unlockAppForm.OnCancel += UnlockAppFormOnCancel;
            _unlockAppForm.OnConfirm += UnlockAppFormOnConfirm;
            //-----------------------------------------

            //hide setup button from header panel
            if (!Pages.ContainsKey("Setup"))
            {
                SetupMenu.Visible = false;
            }

            RegisterViewer();

            //---- Add By Tulip for Restore Client
            //is this is null , mean it's not load from client.ini , means it should load from general config
            if (StartupOption == null)
            {
                var temp = Nvr.Configure.StartupOptions.Clone();
                if (temp.Enabled)
                    StartupOption = temp;
                else
                    StartupOption = new StartupOptions();
            }

            //---- End By Tulip

            foreach (KeyValuePair<String, IPage> obj in Pages)
            {
                obj.Value.LoadConfig();
            }

            _storagePanel.Invalidate();

            _timePanel.Invalidate();

            _tickDateTimeTimer.Enabled = true;
            _tickCpuTimer.Enabled = true;

            RestoreLastStartupOption();

            Nvr.Utility.StartEventReceive();

            Nvr.StorageStatusUpdate();

            //if (_nvr.Configure.EnableJoystick && IntPtr.Size == 4) //only 32bit support joystick
            //Nvr.Utility.InitializeJoystick();

            if (_nvr.Configure.EnableAxisJoystick)
                _nvr.Utility.InitializeAxisJoystick();
            else if (_nvr.Configure.EnableJoystick)
                _nvr.Utility.InitializeJoystick();


            Nvr.OnEventReceive -= NVROnEventReceive;
            Nvr.OnEventReceive += NVROnEventReceive;

            Nvr.Server.OnServerTimeZoneChange += NetworkVideoRecorderOnServerTimeZoneChange;
            Nvr.Server.OnStorageStatusUpdate += NetworkVideoRecorderOnStorageStatusUpdate;

            _watch.Stop();
            Console.WriteLine(Environment.NewLine + Nvr.Status + Environment.NewLine + @"Total App Startup: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (StartupOption.Enabled && OnAppStarted != null)
            {
                var timer = new Timer { Interval = 500 };
                timer.Tick += (ss, ee) =>
                {
                    timer.Enabled = false;

                    OnAppStarted(this, null);

                    StartupOption.Loading = false;
                };

                timer.Enabled = false;
                timer.Enabled = true;
            }
            else
            {
                StartupOption.Loading = false;
            }
        }

        public override void Logout()
        {
            if (_isLock)
                UnlockApplication();

            base.Logout();
        }

        public override void ResetTitleBarText()
        {
            //_credential.UserName + @"@" + 
            Form.Text = _credential.Domain + @":" + _credential.Port + @" - " + Localization["Application_NetworkVideoRecorder"];

            if (IntPtr.Size == 8)
                Form.Text += Localization["Application_64Bit"];

            if (Nvr.Configure.CustomStreamSetting.Enable)
            {
                Form.Text += @"  " + Localization["Menu_Bandwidth"] + @" : " + Bitrates.ToDisplayString(_totalBitrate);
            }
        }

        protected virtual void RegisterViewer()
        {
            RegistViewer(32);
        }

        protected virtual void CreatePages()
        {
            foreach (var obj in Nvr.Server.PageList)
            {
                if (!Nvr.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
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

                    foreach (KeyValuePair<ushort, IUserGroup> pair in Nvr.User.Groups)
                    {
                        pair.Value.Permissions.Add(name, new List<Permission> { Permission.Access });
                    }
                }
            }

            if (SwitchPagePanel.Controls.Count > 1)
                SwitchPagePanel.Width = SwitchPagePanel.Controls.Count * 170;
            else
                SwitchPagePanel.Visible = false;
        }

        protected virtual IPage CreatePage(String name, XmlElement node)
        {
            var page = new Page
            {
                App = this,
                Server = Nvr,
                Name = name,
                PageNode = node,
            };

            return page;
        }

        private void AddPage(String name, XmlElement node)
        {
            var page = CreatePage(name, node);

            if (!page.IsExists) return;

            Pages.Add(page.Name, page);

            //setup dont add to page switch panel.
            if (page.Name == "Setup") return;

            SwitchPagePanel.Controls.Add(page.Icon);
            page.Icon.BringToFront();
        }

        protected override void FormShown(Object sender, EventArgs e)
        {
            base.FormShown(sender, e);

            if (!SwitchPagePanel.Visible || SwitchPagePanel.Controls.Count == 0) return;

            var width = SwitchPagePanel.Controls.Count * 170;
            var leftWidth = (HeaderPanel.Width - LogoPictureBox.Width - MenuStrip.Width - width) / 2;
            SwitchPagePanel.Location = new Point(LogoPictureBox.Location.X + LogoPictureBox.Width + leftWidth, 10);
        }

        private Boolean _isAskingLogin;
        private void NetworkVideoRecorderOnServerTimeZoneChange(Object sender, EventArgs<String> location)
        {
            //popup 1 dialog window at once.
            if (_isAskingLogin) return;
            _isAskingLogin = true;

            var result = TopMostMessageBox.Show(
                (location.Value != Nvr.Server.Location)
                    ? Localization["Application_TimezoneChange"].Replace("%1", Nvr.Server.Location).Replace("%2", location.Value)
                    : Localization["Application_DaylightChange"],
                    Localization["MessageBox_Information"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (result == DialogResult.No)
            {
                Nvr.Server.OnServerTimeZoneChange -= NetworkVideoRecorderOnServerTimeZoneChange;
            }
            _isAskingLogin = false;
        }

        public void NetworkVideoRecorderOnStorageStatusUpdate(Object sender, EventArgs e)
        {
            //re-check panel's width.
            _firstCheckStorageWidth = true;
            _storagePanel.Invalidate();
        }

        public override void PopupInstantPlayback(IDevice device, UInt64 timecode)
        {
            if (!Pages.ContainsKey("Playback")) return;

            if (device == null || timecode == 0 || device.Server == null) return;
            if (!device.Server.Device.Devices.ContainsKey(device.Id)) return;

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
        }

        public override void PopupInstantPlayback(IDevice device, ulong timecode, object info)
        {
            if (!Pages.ContainsKey("Playback")) return;

            if (device == null || timecode == 0 || device.Server == null) return;
            if (!device.Server.Device.Devices.ContainsKey(device.Id)) return;

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
                    var popupStream = controlPanel.Control as IPopupStream;
                    if (popupStream != null)
                    {
                        popupStream.PopupInstantPlayback(device, timecode, info);
                    }
                }
            }
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

        public override void PopupLiveStream(IDevice device)
        {
            if (!Pages.ContainsKey("Live")) return;

            if (device == null) return;

            if (MaximumInstantLiveWindow != 0)
            {
                if (GetLiveStreamCount() >= MaximumInstantLiveWindow)
                    return;
            }

            foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
            {
                foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
                {
                    if (controlPanel.Control is IPopupStream)
                    {
                        ((IPopupStream)controlPanel.Control).PopupLiveStream(device);
                    }
                }
            }
        }

        protected virtual UInt16 GetLiveStreamCount()
        {
            UInt16 count = 0;
            foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
            {
                foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
                {
                    if (controlPanel.Control is IPopupStream)
                    {
                        count += ((IPopupStream)controlPanel.Control).GetLiveStreamCount;
                    }
                }
            }

            return count;
            //return Convert.ToUInt16(Application.OpenForms.OfType<PopupLiveStream>().Count());
        }

        private delegate void QuitDelegate();
        public override void Quit()
        {
            if (Form.InvokeRequired)
            {
                try
                {
                    Form.Invoke(new QuitDelegate(Quit));
                }
                catch (Exception)
                {
                }
                return;
            }

            if (_exportVideoForm != null && _exportVideoForm.IsExporting)
                _exportVideoForm.StopExport();

            if (Nvr.Utility != null)
                Nvr.Utility.Quit();

            Nvr.Logout();
            _tickDateTimeTimer.Enabled = false;
            _tickCpuTimer.Enabled = false;

            AppProperties.SaveProperties();

            Nvr.WriteOperationLog(String.Format("{0} Logout from {1}:{2} at {3}",
                        Nvr.Credential.UserName, Nvr.Credential.Domain, Nvr.Credential.Port, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));

            Deactivate();
        }

        private PrintImageForm _printImageForm;
        public override void PrintImage(List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime)
        {
            if (_printImageForm == null)
                _printImageForm = new PrintImageForm { App = this };

            _printImageForm.PrintImage(Nvr, printDevices, printImages, dateTime);
        }

        protected ExportVideoForm _exportVideoForm;
        public override void ExportVideo(IDevice[] usingDevices, DateTime start, DateTime end)
        {
            if (_exportVideoForm == null)
                _exportVideoForm = new ExportVideoForm { App = this };

            _exportVideoForm.ExportVideo(Nvr, usingDevices, start, end);
        }

        public void ExitFullScreen(Object sender, EventArgs e)
        {
            ExitFullScreen();
        }

        public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
        {
            LogCurrentViewLayout();
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            LogCurrentViewLayout();
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

        protected static String SaveDeviceGroupXml()
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Type", "DeviceGroup"));

            return xmlDoc.InnerXml;
        }

        private readonly System.Timers.Timer _hideLoadingTimer = new System.Timers.Timer();

        public override void SaveUserDefineDeviceGroup(IDeviceGroup group)
        {
            ApplicationForms.ShowLoadingIcon(Form);
            Application.RaiseIdle(null);

            if (Nvr.Device.Groups.Values.ToArray().Contains(group))
            {
                Nvr.Device.Save(SaveDeviceGroupXml());
            }
            else
            {
                Nvr.User.Save();
            }
            group.ReadyState = ReadyState.Ready;

            RaiseOnUserDefineDeviceGroupModify(EventArgs.Empty);

            _hideLoadingTimer.Enabled = true;
        }

        public override void DeleteUserDefineDeviceGroup(IDeviceGroup group)
        {
            ApplicationForms.ShowLoadingIcon(Form);
            Application.RaiseIdle(null);

            if (Nvr.Device.Groups.Values.ToArray().Contains(group))
            {
                Nvr.Device.Groups.Remove(group.Id);
                Nvr.Device.Save(SaveDeviceGroupXml());
            }
            else
            {
                Nvr.User.Current.DeviceGroups.Remove(group.Id);
                Nvr.User.Save();
            }

            group.ReadyState = ReadyState.Delete;
            Nvr.GroupModify(group);

            RaiseOnUserDefineDeviceGroupModify(EventArgs.Empty);

            _hideLoadingTimer.Enabled = true;
        }

        private void HideLoading(Object sender, EventArgs e)
        {
            _hideLoadingTimer.Enabled = false;

            ApplicationForms.HideLoadingIcon();
        }
    }
}
