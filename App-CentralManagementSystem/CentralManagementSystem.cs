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
using ServerProfile;
using ApplicationForms = App.ApplicationForms;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem : AppClient
    {
        public override event EventHandler OnAppStarted;
        public UInt16 MaximumInstantPlaybackWindow = 4;//3;
        public UInt16 MaximumInstantLiveWindow = 4;

        public override Form Form
        {
            get { return CMS.Form; }
            set
            {
                CMS.Form = value;
                value.Icon = Properties.Resources.icon;
            }
        }

        public override ServerCredential Credential
        {
            get { return CMS.Credential; }
            set { CMS.Credential = value; }
        }
        public override String Language { get; set; }

        private readonly Stopwatch _watch = new Stopwatch();

        private ICMS _cms;

        protected ICMS CMS
        {
            get { return _cms ?? (_cms = CreateCMS()); }
        }

        protected virtual ICMS CreateCMS()
        {
            var cms = new CMS()
            {
                Name = Localization["Application_CentralManagementSystem"]
            };

            return cms;
        }

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

        public override void RemoveProperties()
        {
            AppProperties.RemoveProperties(Credential);
        }


        // Constructor
        public CentralManagementSystem()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Version = version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');
            if (version.Revision.ToString() != "0")
                Version += ("." + version.Revision.ToString().PadLeft(2, '0'));

            AppProperties = new AppClientProperties();

            _watch.Reset();
            _watch.Start();

            IsInitialize = false;
            _checkAllNVRTimer.Enabled = true;

            Pages = new Dictionary<String, IPage>();

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
								   {"Application_CentralManagementSystem", "Central Management System"},

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
								   {"Application_TotalBitrate", "Bitrate %1"},

								   {"Application_ConfirmLockApp", "Please confirm to lock application."},
								   {"Application_ConfirmSignOut", "Are you sure you want to sign out?"},

								   {"Application_TimezoneChange", "Server's time zone has been modified.\r\nFrom \"%1\" to \"%2\"\r\nDo you want to login again to apply new time zone?"},
								   {"Application_NVRChange", "NVR setting changed. Please login again to apply new settings."},
								   {"Application_DaylightChange", "Server's daylight saving has been modified.\r\nDo you want to login again to apply new daylight saving?"},
								   {"Application_SomeNVRUnavailable", "The following NVR can't connect:"},
								   {"Application_ServerIsNotCMS", "\"%1 :%2\" is not CMS."},

								   {"Application_UtilityInitError", "Utility Initialize Error"},

                                   {"Application_CPUUsage", "CPU %1%"},
							   };
            Localizations.Update(Localization);
        }

        public override void Activate()
        {
            BasicDevice.DisplayDeviceId = CMS.Configure.DisplayDeviceId;
            DeviceGroup.DisplayGroupId = CMS.Configure.DisplayGroupId;
            NVR.DisplayNVRId = CMS.Configure.DisplayNVRId;

            CMS.OnLoadFailure -= CmsOnLoadFailure;

            ResetTitleBarText();

            //Check Apps Numbers
            if (CMS.Server.PageList.Count <= 0)
            {
                TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            if (CMS.User.Current == null || CMS.User.Current.Group == null)
            {
                TopMostMessageBox.Show(Localization["Application_UserInformationNotFound"], Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            if (CMS.User.Current.Group.Permissions.Count == 0)
            {
                TopMostMessageBox.Show(Localization["Application_GroupPermissionNotFound"].Replace("%1", CMS.User.Current.Group.Name), Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            //RemoveNoPermissionDevice();

            CreatePage();

            _hideLoadingTimer.Elapsed += HideLoading;
            _hideLoadingTimer.Interval = 500;
            _hideLoadingTimer.SynchronizingObject = CMS.Form;

            //Check Apps Numbers
            if (Pages.Count <= 0)
            {
                TopMostMessageBox.Show(Localization["Application_PageNotFound"], Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //hide setup button from header panel
            if (!Pages.ContainsKey("Setup"))
            {
                SetupMenu.Visible = false;
            }

            //create unlock nvr form
            _unlockAppForm = new UnlockAppForm
            {
                App = this,
                User = CMS.User.Current,
                Icon = Form.Icon,
            };

            _unlockAppForm.OnCancel += UnlockAppFormOnCancel;
            _unlockAppForm.OnConfirm += UnlockAppFormOnConfirm;
            //-----------------------------------------

            RegisterViewer();

            //---- Add By Leo for Restore Client
            //is this is null , mean it's not load from client.ini , means it should load from general config
            if (StartupOption == null)
            {
                var temp = CMS.Configure.StartupOptions.Clone();
                if (temp.Enabled)
                    StartupOption = temp;
                else
                    StartupOption = new StartupOptions();
            }
            //---- End By Leo

            foreach (KeyValuePair<String, IPage> obj in Pages)
            {
                obj.Value.LoadConfig();
            }

            _timePanel.Invalidate();
            _tickDateTimeTimer.Enabled = true;
            _tickCpuTimer.Enabled = true;
            RestoreLastStartupOption();

            //Activate(Pages.First().Value);

            CMS.ListenNVREvent(null);

            CMS.Utility.StartEventReceive();

            //if (CMS.Configure.EnableJoystick && IntPtr.Size == 4) //only 32bit support joystick
            //CMS.Utility.InitializeJoystick();

            if (CMS.Configure.EnableAxisJoystick)
                CMS.Utility.InitializeAxisJoystick();
            else if (CMS.Configure.EnableJoystick)
                CMS.Utility.InitializeJoystick();


            CMS.OnEventReceive -= CMSOnEventReceive;
            CMS.OnEventReceive += CMSOnEventReceive;
            _watch.Stop();

            CMS.Utility.GetAllNVRStatus();
            CMS.Utility.GetAllChannelStatus();

            CMS.Server.OnServerTimeZoneChange += NetworkVideoRecorderOnServerTimeZoneChange;

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
            Form.Text = Credential.Domain + @":" + Credential.Port + @" - " + Localization["Application_CentralManagementSystem"];

            if (IntPtr.Size == 8)
                Form.Text += Localization["Application_64Bit"];
        }

        protected virtual void RegisterViewer()
        {
            RegistViewer(32);
        }

        protected virtual void CreatePage()
        {
            var noPermissionPage = new List<String>();
            foreach (var obj in CMS.Server.PageList)
            {
                if (!CMS.User.Current.Group.CheckPermission(obj.Key, Permission.Access))
                {
                    noPermissionPage.Add(obj.Key);
                    continue;
                }

                if (Pages.ContainsKey(obj.Key)) continue;

                var page = CreatePage(obj.Key, CMS, this, obj.Value);

                if (!page.IsExists)
                {
                    noPermissionPage.Add(obj.Key);
                    continue;
                }

                Pages.Add(page.Name, page);

                //setup dont add to page switch panel.
                if (page.Name == "Setup") continue;

                SwitchPagePanel.Controls.Add(page.Icon);
                page.Icon.BringToFront();
            }

            foreach (var pageName in noPermissionPage)
                CMS.Server.PageList.Remove(pageName);

            //get plug-ins page
            String plusinsPath = Environment.CurrentDirectory + @"\plug-ins\";
            if (Directory.Exists(plusinsPath))
            {
                String[] files = Directory.GetFiles(plusinsPath);

                foreach (var file in files)
                {
                    var xmldoc = Xml.LoadXmlFromFile(file);

                    var name = Xml.GetFirstElementValueByTagName(xmldoc, "PageName");
                    //dupcated!)
                    if (Pages.ContainsKey(name)) continue;

                    var root = xmldoc.CreateElement("Root");
                    root.AppendChild(xmldoc.CreateXmlElementWithText("Config", Path.GetFileName(file)));

                    AddPage(name, root);
                }
            }

            if (SwitchPagePanel.Controls.Count > 1)
                SwitchPagePanel.Width = SwitchPagePanel.Controls.Count * 170;
            else
                SwitchPagePanel.Visible = false;
        }

        protected virtual IPage CreatePage(string name, IServer server, IApp app, XmlElement node)
        {
            var page = new Page
            {
                App = app,
                Server = server,
                Name = name,
                PageNode = node,
            };

            return page;
        }

        private void AddPage(String name, XmlElement node)
        {
            var page = new Page
            {
                App = this,
                Server = CMS,
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

        protected override void FormShown(Object sender, EventArgs e)
        {
            base.FormShown(sender, e);

            if (!SwitchPagePanel.Visible || SwitchPagePanel.Controls.Count == 0) return;

            var width = SwitchPagePanel.Controls.Count * 170;
            var leftWidth = (HeaderPanel.Width - LogoPictureBox.Width - MenuStrip.Width - width) / 2;
            SwitchPagePanel.Location = new Point(LogoPictureBox.Location.X + LogoPictureBox.Width + leftWidth, 10);

            //ShowUnavailableNVRMessage();
        }

        private Boolean _isAskingLogin;
        private void NetworkVideoRecorderOnServerTimeZoneChange(Object sender, EventArgs<String> location)
        {
            if (_isAskingLogin) return;
            _isAskingLogin = true;

            var result = TopMostMessageBox.Show(
                (location.Value != CMS.Server.Location)
                    ? Localization["Application_TimezoneChange"].Replace("%1", CMS.Server.Location).Replace("%2", location.Value)
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
                CMS.Server.OnServerTimeZoneChange -= NetworkVideoRecorderOnServerTimeZoneChange;
            }
            _isAskingLogin = false;
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

        public override void PopupInstantPlayback(IDevice device, UInt64 timecode)
        {
            if (!Pages.ContainsKey("Playback")) return;

            if (device == null || timecode == 0 || device.Server == null) return;
            if (!CMS.NVRManager.NVRs.ContainsKey(device.Server.Id) || !device.Server.Device.Devices.ContainsKey(device.Id)) return;

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
            //timecode -= 10000;
            //if (InstantPlayback != null) return;

            //var instantPlayback = new InstantPlayback
            //{
            //    App = this,
            //    Server = CMS,
            //    Camera = (ICamera)device,
            //    DateTime = DateTimes.ToDateTime(Convert.ToUInt64(timecode), CMS.Server.TimeZone),
            //    Icon = Form.Icon,
            //};

            //instantPlayback.Show();
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
            if (Form.InvokeRequired)
            {
                try
                {
                    Form.Invoke(new QuitDelegate(Quit));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return;
            }

            if (_exportVideoForm != null && _exportVideoForm.IsExporting)
                _exportVideoForm.StopExport();

            if (CMS.Utility != null)
                CMS.Utility.Quit();

            CMS.Logout();
            _tickDateTimeTimer.Enabled = false;
            _tickCpuTimer.Enabled = false;
            AppProperties.SaveProperties();

            Deactivate();
        }

        private PrintImageForm _printImageForm;
        public override void PrintImage(List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime)
        {
            if (_printImageForm == null)
                _printImageForm = new PrintImageForm { App = this };

            _printImageForm.PrintImage(CMS, printDevices, printImages, dateTime);
        }

        private ExportVideoForm _exportVideoForm;
        public override void ExportVideo(IDevice[] usingDevices, DateTime start, DateTime end)
        {
            if (_exportVideoForm == null)
                _exportVideoForm = new ExportVideoForm { App = this };

            _exportVideoForm.ExportVideo(CMS, usingDevices, start, end);
        }

        public void ExitFullScreen(Object sender, EventArgs e)
        {
            ExitFullScreen();
        }

        private readonly System.Timers.Timer _hideLoadingTimer = new System.Timers.Timer();
        public override void SaveUserDefineDeviceGroup(IDeviceGroup group)
        {
            ApplicationForms.ShowLoadingIcon(Form);
            Application.RaiseIdle(null);

            if (CMS.Device.Groups.Values.ToArray().Contains(group))
            {
                CMS.Device.Save(SaveDeviceGroupXml());
            }
            else
            {
                CMS.User.Save();
            }
            group.ReadyState = ReadyState.Ready;

            RaiseOnUserDefineDeviceGroupModify(EventArgs.Empty);

            _hideLoadingTimer.Enabled = true;
        }

        public override void DeleteUserDefineDeviceGroup(IDeviceGroup group)
        {
            ApplicationForms.ShowLoadingIcon(Form);
            Application.RaiseIdle(null);

            if (CMS.Device.Groups.Values.ToArray().Contains(group))
            {
                CMS.Device.Groups.Remove(group.Id);
                CMS.Device.Save(SaveDeviceGroupXml());
            }
            else
            {
                CMS.User.Current.DeviceGroups.Remove(group.Id);
                CMS.User.Save();
            }

            group.ReadyState = ReadyState.Delete;
            CMS.GroupModify(group);

            RaiseOnUserDefineDeviceGroupModify(EventArgs.Empty);

            _hideLoadingTimer.Enabled = true;
        }

        private void HideLoading(Object sender, EventArgs e)
        {
            _hideLoadingTimer.Enabled = false;

            ApplicationForms.HideLoadingIcon();
        }

        protected static String SaveDeviceGroupXml()
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Type", "DeviceGroup"));

            return xmlDoc.InnerXml;
        }
    }
}
