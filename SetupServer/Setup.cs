using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public partial class Setup : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public String TitleName { get; set; }
        public IApp App { get; set; }
        private INVR _nvr;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    _nvr = value as INVR;
            }
        }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        private OverallControl _overall;
        private PortControl _port;
        private SSLPortControl _sslport;
        private TimeControl _dateTime;
        private EthernetControl _ethernet1;
        private EthernetControl _ethernet2;
        private RAID _raid;
        private StorageControl _storage;
        private DeviceKeepDaysRecording _deviceKeepDaysRecording;
        private RestoreControl _restore;
        private UpgradeControl _upgrade;
        private PowerControl _power;
        private StoreControl _store;
        private DatabaseControl _database;
        private DevicePackControl _devicePack;
        private ArchiveServerControl _archiveServer;

        private const UInt16 SecondsWaiting = 900;

        public Setup()
        {
            Localization = new Dictionary<String, String>
			{
				{"Control_Server", "Server"},

				{"MessageBox_Error", "Error"},
				{"MessageBox_Confirm", "Confirm"},
				{"MessageBox_Information", "Information"},

				{"Application_ConfigChange", "Configuration settings has been restored. Please sign in again."},
				{"Application_DevicePackChange", "Device Pack has been updated. Please sign in again."},
								   
				{"SetupServer_Port", "Port"},
				{"SetupServer_SSLPort", "SSL Port"},
				{"SetupServer_Storage", "Storage"},
				{"SetupServer_Restore", "Restore"},
				{"SetupServer_Ethernet", "Ethernet"},
				{"SetupServer_RAID", "RAID"},
				{"SetupServer_DateTime", "Date Time"},
				{"SetupServer_Power", "Power"},
				{"SetupServer_Upgrade", "Upgrade"},
				{"SetupServer_Store", "Store"},
				{"SetupServer_Database", "Database"},
				{"SetupServer_DeviceKeepDaysRecording", "Device keep days recording"},
				{"SetupServer_DevicePackVersion", "Device Pack Version"},
                {"SetupServer_ArchiveServer", "Archive Server"},

				{"SetupServer_NoSelectedFile", "No selected file"},
				{"SetupServer_NoSelectedContent", "Didn't select restore content"},
				{"SetupServer_ConfirmRestore", "Please confirm again to restore settings."},
				{"SetupServer_ConfirmUpdateEthernet", "Are you sure you want to update ethernet?"},
				{"SetupServer_ConfirmDevicePack", "Please confirm again to update device pack."},

				{"SetupServer_WrongContent", "Please restore with the \"%1\" file. "},
				{"SetupServer_ConfirmUpgrade", "Are you sure you want to upgrade firmware?"},
				{"SetupServer_ConfirmUpgradeRestart", "Please login again after %1 minutes. "},
				{"SetupServer_UpgradeFail", "Upgrade failed. Please check your upgrade file."}
			};
            Localizations.Update(Localization);

            Name = "Server";
            TitleName = Localization["Control_Server"];

            InitializeComponent();

            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Server"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        public void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            _overall = CreateOverallControl();
            _overall.Initialize();

            _port = new PortControl
            {
                Server = Server
            };
            _port.Initialize();

            _sslport = new SSLPortControl
            {
                Server = Server
            };
            _sslport.Initialize();

            _storage = new StorageControl
            {
                Server = Server
            };
            _storage.OnDeviceKeepDaysRecording += StorageOnDeviceKeepDaysRecording;

            _deviceKeepDaysRecording = new DeviceKeepDaysRecording
            {
                Server = Server
            };
            _deviceKeepDaysRecording.Initialize();

            _restore = new RestoreControl
            {
                Server = Server
            };

            _dateTime = new TimeControl
            {
                Server = Server
            };
            _dateTime.InitialTimeSetting();

            _raid = new RAID
            {
                Server = Server
            };
            _raid.InitialRAIDSetting();
            _raid.OnUpdateButtons += RaidOnUpdateButtons;

            _ethernet1 = new EthernetControl
            {
                Id = 1,
                Server = Server
            };
            _ethernet1.InitailEthernetSetting();

            _ethernet2 = new EthernetControl
            {
                Id = 2,
                Server = Server
            };
            _ethernet2.InitailEthernetSetting();

            _upgrade = new UpgradeControl
            {
                Server = Server
            };

            _power = new PowerControl
            {
                Server = Server,
                App = App
            };

            _store = new StoreControl
            {
                Server = Server
            };
            _database = new DatabaseControl
            {
                Server = Server
            };
            _database.Initialize();

            _devicePack = new DevicePackControl
            {
                Server = Server
            };

            _archiveServer = new ArchiveServerControl()
            {
                Server = Server,
                CMS = Server is ICMS ? Server as ICMS : null
            };
            _archiveServer.Initialize();

            if (Server is IFOS)
                ((IFOS)Server).NVR.OnNVRStatusUpdate += ServerOnNVRStatusUpdate;

            _overall.OnPortEdit += OverallOnPortEdit;
            _overall.OnSSLPortEdit += OverallOnSSLPortEdit;
            _overall.OnStorageEdit += OverallOnStorageEdit;
            _overall.OnRestore += OverallOnRestore;
            _overall.OnDateTimeEdit += OverallOnDateTime;
            _overall.OnEthernetEdit += OverallOnEthernetEdit;
            _overall.OnPowerEdit += OverallOnPower;
            _overall.OnUpgradeEdit += OverallOnUpgradeEdit;
            _overall.OnRAIDEdit += OverallOnRAIDEdit;
            _overall.OnStoreEdit += OverallOnStoreEdit;
            _overall.OnDBEdit += OverallOnDBEdit;
            _overall.OnDevicePackEdit += OverallOnDevicePackEdit;
            _overall.OnArchiveServerEdit += OverallOnArchiveEdit;

            contentPanel.Controls.Add(_overall);

            Server.Server.OnCompleteUpdateEthernetLogout += ServerOnCompleteUpdateEthernetLogout;

            Server.OnLoadComplete -= ServerOnLoadComplete;
            Server.OnLoadComplete += ServerOnLoadComplete;
        }

        protected virtual OverallControl CreateOverallControl()
        {
            return new OverallControl
            {
                App = App,
                Server = Server
            };
        }

        protected Control _focusControl;

        private void ServerOnNVRStatusUpdate(Object sender, EventArgs e)
        {
            if (_focusControl == null && Parent.Visible)
            {
                _overall.Invalidate();
                _overall.DisplayProgress();
            }
        }

        private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);

        private void ServerOnLoadComplete(object sender, EventArgs<string> e)
        {
            if (Parent != null && Parent.Visible)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new RefreshContentDelegate(ServerOnLoadComplete), sender, e);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                if (_focusControl == null) return;
                if (_focusControl == _port) _port.ParseSetting();
                if (_focusControl == _database) _database.ParseSetting();
                if (_focusControl == _storage) _storage.GeneratorStorageList();
                if (_focusControl == _raid) _raid.ParseRAIDInfo();
                if (_focusControl == _sslport) _sslport.ParseSetting();
                if (_focusControl == _dateTime) _dateTime.ParseTimeConfig();
                if (_focusControl == _deviceKeepDaysRecording) _deviceKeepDaysRecording.GenerateViewModel();
            }
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            ShowOverAll();
        }

        private void ShowOverAll()
        {
            _focusControl = null;
            _overall.Enabled = true;
            if (_restore.Stream != null)
            {
                _restore.Stream.Close();
                _restore.Stream = null;
            }

            if (_upgrade.Stream != null)
            {
                _upgrade.Stream.Close();
                _upgrade.Stream = null;
            }

            if (!contentPanel.Controls.Contains(_overall))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(_overall);
            }

            if (Server is IFOS)
            {
                _overall.DisplayProgress();
            }

            _overall.ParserSetting();

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "Refresh":
                    if (_focusControl == _storage)
                    {
                        ApplicationForms.ShowLoadingIcon(Server.Form);

                        RefreshStorageDelegate refreshStorageDelegate = RefreshStorage;
                        refreshStorageDelegate.BeginInvoke(RefreshStorageCallback, refreshStorageDelegate);
                    }
                    break;

                case "UpdateDevicePack":
                    if (_nvr == null) return;

                    if (String.IsNullOrEmpty(_devicePack.FullFileName))
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_NoSelectedFile"],
                                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    ApplicationForms.ShowProgressBar(Server.Form);
                    Application.RaiseIdle(null);

                    var updateResult = TopMostMessageBox.Show(Localization["SetupServer_ConfirmDevicePack"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (updateResult != DialogResult.Yes)
                    {
                        ApplicationForms.HideProgressBar();
                        return;
                    }


                    Server.Utility.UploadPack(_devicePack.FullFileName);

                    _nvr.OnDevicePackUpdateCompleted -= NVROnDevicePackUpdateCompleted;
                    _nvr.OnDevicePackUpdateCompleted += NVROnDevicePackUpdateCompleted;
                    break;

                case "RestoreSetting":
                    if (_restore.Stream == null)
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_NoSelectedFile"],
                                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    if (_restore.Contents.Count == 0)
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_NoSelectedContent"],
                                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    ApplicationForms.ShowProgressBar(Server.Form);
                    Application.RaiseIdle(null);

                    var result = TopMostMessageBox.Show(Localization["SetupServer_ConfirmRestore"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes)
                    {
                        ApplicationForms.HideProgressBar();
                        return;
                    }

                    Server.Server.Restore(_restore.Stream, _restore.Contents);

                    TopMostMessageBox.Show(Localization["Application_ConfigChange"], Localization["MessageBox_Information"],
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    App.OpenAnotherProcessAfterLogout = true;
                    App.Logout();
                    break;

                case "UpgradeFirmware":
                    if (_upgrade.Stream == null)
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_NoSelectedFile"],
                                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (!_upgrade.Stream.CanRead)
                        _upgrade.Stream = _upgrade.OpenSettingDialog.OpenFile();

                    var ext = _upgrade.FileName.Split('.');
                    if (ext[ext.Length - 1].ToUpper() != "IMG")
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_WrongContent"].Replace("%1", ".img"),
                                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    ApplicationForms.ShowProgressBar(Server.Form);
                    Application.RaiseIdle(null);

                    var upgradeResult = TopMostMessageBox.Show(Localization["SetupServer_ConfirmUpgrade"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (upgradeResult != DialogResult.Yes)
                    {
                        ApplicationForms.HideProgressBar();
                        return;
                    }

                    ApplicationForms.ShowLoadingIcon(Server.Form);
                    var check = Server.Server.Upgrade(_upgrade.Stream);
                    if (check.IndexOf("Success") > -1)
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_ConfirmUpgradeRestart"].Replace("%1", (SecondsWaiting / 60).ToString()), Localization["MessageBox_Information"],
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        App.OpenAnotherProcessAfterLogout = true;
                        App.Logout();
                    }
                    else
                    {
                        TopMostMessageBox.Show(Localization["SetupServer_UpgradeFail"], Localization["MessageBox_Error"],
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ApplicationForms.HideLoadingIcon();
                    }
                    break;

                case "Format":
                    _raid.CheckFormat();
                    break;

                case "UpdateEthernet":
                    var ethernetResult = TopMostMessageBox.Show(Localization["SetupServer_ConfirmUpdateEthernet"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ethernetResult != DialogResult.Yes) return;

                    if (_focusControl == _ethernet1)
                        Server.Server.SaveEthernet(1);
                    else if (_focusControl == _ethernet2)
                        Server.Server.SaveEthernet(2);
                    break;

                case "SaveSetting":
                    if (_focusControl == _dateTime)
                        Server.Server.Save("DateTime");
                    break;

                default:
                    if (_focusControl == _deviceKeepDaysRecording && item == "Back")
                    {
                        _focusControl = _storage;
                        Manager.ReplaceControl(_deviceKeepDaysRecording, _focusControl, contentPanel, ManagerMoveToSettingComplete);
                    }
                    else if (item == TitleName || item == "Back")
                    {
                        Manager.ReplaceControl(_focusControl, _overall, contentPanel, ManagerMoveToOverallComplete);
                    }
                    break;
            }
        }

        private void NVROnDevicePackUpdateCompleted(Object sender, EventArgs e)
        {
            _nvr.OnDevicePackUpdateCompleted -= NVROnDevicePackUpdateCompleted;

            TopMostMessageBox.Show(Localization["Application_DevicePackChange"], Localization["MessageBox_Information"],
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            App.OpenAnotherProcessAfterLogout = true;
            App.Logout();
        }

        private void ServerOnCompleteUpdateEthernetLogout(Object sender, EventArgs e)
        {
            App.OpenAnotherProcessAfterLogout = true;
            App.Logout();
        }

        private void StorageOnDeviceKeepDaysRecording(Object sender, EventArgs e)
        {
            _focusControl = _deviceKeepDaysRecording;

            _deviceKeepDaysRecording.GenerateViewModel();

            Manager.ReplaceControl(_storage, _deviceKeepDaysRecording, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnPortEdit(Object sender, EventArgs e)
        {
            _focusControl = _port;
            _port.ParseSetting();

            Manager.ReplaceControl(_overall, _port, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnSSLPortEdit(Object sender, EventArgs e)
        {
            _focusControl = _sslport;
            _sslport.ParseSetting();

            Manager.ReplaceControl(_overall, _sslport, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnStorageEdit(Object sender, EventArgs e)
        {
            _focusControl = _storage;
            _storage.GeneratorStorageList();

            Manager.ReplaceControl(_overall, _storage, contentPanel, ManagerMoveToSettingComplete);
        }

        protected virtual void OverallOnRestore(Object sender, EventArgs e)
        {
            _focusControl = _restore;
            _restore.Reset();

            Manager.ReplaceControl(_overall, _restore, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnDateTime(Object sender, EventArgs e)
        {
            _focusControl = _dateTime;

            Manager.ReplaceControl(_overall, _dateTime, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnRAIDEdit(Object sender, EventArgs e)
        {
            _focusControl = _raid;

            Manager.ReplaceControl(_overall, _raid, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnUpgradeEdit(Object sender, EventArgs e)
        {
            _focusControl = _upgrade;

            Manager.ReplaceControl(_overall, _upgrade, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnEthernetEdit(Object sender, EventArgs<UInt16> e)
        {
            if (e.Value == 1)
            {
                _focusControl = _ethernet1;

                Manager.ReplaceControl(_overall, _ethernet1, contentPanel, ManagerMoveToSettingComplete);
            }
            else if (e.Value == 2)
            {
                _focusControl = _ethernet2;

                Manager.ReplaceControl(_overall, _ethernet2, contentPanel, ManagerMoveToSettingComplete);
            }
        }

        private void OverallOnPower(Object sender, EventArgs e)
        {
            _focusControl = _power;

            Manager.ReplaceControl(_overall, _power, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnStoreEdit(Object sender, EventArgs e)
        {
            _focusControl = _store;
            _store.ParseSetting();

            Manager.ReplaceControl(_overall, _store, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnDBEdit(Object sender, EventArgs e)
        {
            _focusControl = _database;

            Manager.ReplaceControl(_overall, _database, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnDevicePackEdit(Object sender, EventArgs e)
        {
            _focusControl = _devicePack;

            Manager.ReplaceControl(_overall, _devicePack, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnArchiveEdit(Object sender, EventArgs e)
        {
            _focusControl = _archiveServer;
            _archiveServer.ParseArchiveServer();
            Manager.ReplaceControl(_overall, _archiveServer, contentPanel, ManagerMoveToSettingComplete);
        }

        private delegate void RefreshStorageCallbackDelegate(IAsyncResult result);
        private delegate void RefreshStorageDelegate();
        private void RefreshStorageCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new RefreshStorageCallbackDelegate(RefreshStorageCallback), result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return;
            }

            ((RefreshStorageDelegate)result.AsyncState).EndInvoke(result);

            _storage.GeneratorStorageList();

            ApplicationForms.HideLoadingIcon();
        }

        private void RefreshStorage()
        {
            Server.Server.LoadStorageInfo();
        }

        private void ManagerMoveToOverallComplete()
        {
            if (_restore.Stream != null)
            {
                _restore.Stream.Close();
                _restore.Stream = null;
            }

            if (_upgrade.Stream != null)
            {
                _upgrade.Stream.Close();
                _upgrade.Stream = null;
            }

            _focusControl = null;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
        }

        protected void ManagerMoveToSettingComplete()
        {
            String buttons = "";
            if (_focusControl == _storage)
                buttons = "Refresh";

            if (_focusControl == _restore)
                buttons = "RestoreSetting";

            if (_focusControl == _upgrade)
                buttons = "UpgradeFirmware";

            if (_focusControl == _ethernet1)
            {
                buttons = "UpdateEthernet";
                _ethernet1.ParseEthernetInfo();
            }

            if (_focusControl == _ethernet2)
            {
                buttons = "UpdateEthernet";
                _ethernet2.ParseEthernetInfo();
            }

            if (_focusControl == _dateTime)
            {
                if (Server.Server.Platform == Platform.Linux)
                    buttons = "SaveSetting";
                _dateTime.ParseTimeConfig();
            }

            if (_focusControl == _devicePack)
            {
                buttons = "UpdateDevicePack";
            }

            if (_focusControl == _database)
                _database.ParseSetting();

            if (_focusControl == _raid)
            {
                _raid.ParseRAIDInfo();
                return;
            }

            var text = TitleName + "  /  ";
            text += (Localization.ContainsKey("SetupServer_" + _focusControl.Name))
                        ? Localization["SetupServer_" + _focusControl.Name]
                        : _focusControl.Name;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
                    text, "Back", buttons)));
        }

        private void RaidOnUpdateButtons(object sender, EventArgs<String> e)
        {
            var text = TitleName + "  /  ";
            text += (Localization.ContainsKey("SetupServer_" + _focusControl.Name))
                        ? Localization["SetupServer_" + _focusControl.Name]
                        : _focusControl.Name;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
                    text, "Back", e.Value)));
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowOverAll();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        /*public void GlobalMouseHandler()
        {
            if (_focusControl == _timeZone)
                _timeZone.GlobalMouseHandler();
        }*/
    }
}
