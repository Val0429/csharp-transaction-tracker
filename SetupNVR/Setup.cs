using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;
using SetupBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Manager = SetupBase.Manager;

namespace SetupNVR
{
    public partial class Setup : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public String TitleName { get; set; }
        public IApp App { get; set; }
        public ICMS CMS;
        public IVAS VAS;
        public IFOS FOS;
        public IPTS PTS;

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is ICMS)
                    CMS = value as ICMS;
                else if (value is IVAS)
                    VAS = value as IVAS;
                else if (value is IFOS)
                {
                    FOS = value as IFOS;
                    FOS.NVR.OnNVRStatusUpdate += ServerOnNVRStatusUpdate;
                }
                else if (value is IPTS)
                    PTS = value as IPTS;
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

        protected ListPanel _listPanel;
        private EditPanel _editPanel;
        private SearchPanel _searchPanel;

        private EditDevicePanel _editDevicePanel;
        private Control _focusControl;
        private BackgroundWorker _syncBackgroundWorker;
        private Boolean _isSearching;
        //private BackgroundWorker _setupBackgroundWorker;
        public Setup()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_NVR", "NVR"},

                                   {"MessageBox_Information", "Information"},
                                   {"MessageBox_Error", "Error"},
                                   {"MessageBox_Confirm", "Confirm"},
                                   {"SetupNVR_NVRDevice", "Device"},
                                   {"SetupNVR_SearchNVR", "Search NVR"},
                                   {"SetupNVR_NewNVR", "New NVR"},
                                   {"SetupTitle_Delete", "Delete"},
                                   {"SetupNVR_MaximumLicense", "Reached maximum license limit \"%1\""},
                                   {"SetupNVR_MaximumAmount", "Reached maximum number of NVR limit \"%1\""},
                                   {"SetupNVR_SetupNVRInProgress", "Setup nvr already in progress.\r\nPlease complete previous operation."},
                                   {"SetupNVR_SyncConfirm", "Synchronization nvr setting will lost unsaved changes. Are you sure you want to sync settings?"},

                                   {"Application_SomeNVRUnavailable", "The following NVR can't connect:"},
                                   {"Application_ServerIsNotNVR", "\"%1\" is not NVR."},

                                    {"LoginForm_SignInFailedConnectFailure", "Can not connect to server. Please confirm host and port is correct."},
                                    {"LoginForm_SignInFailedAuthFailure", "Login failure. Please confirm account and password is correct."},

                                   {"LoginForm_SignInTimeout", "Login timeout. Please check firewall setting."},
                               };
            Localizations.Update(Localization);

            Name = "NVR";
            TitleName = Localization["Control_NVR"];

            InitializeComponent();

            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_NVR"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        protected virtual EditPanel CreateEditPanel()
        {
            var editPanel = new EditPanel
            {
                Server = Server,
                CMS = CMS,
                VAS = VAS,
                FOS = FOS,
                PTS = PTS
            };
            return editPanel;
        }

        public void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            _listPanel = CreateListPanel();
            _listPanel.Initialize();

            _editPanel = CreateEditPanel();
            _editPanel.Initialize();

            _listPanel.OnNVREdit += ListPanelOnNVREdit;
            _listPanel.OnNVRAdd += ListPanelOnNVRAdd;
            _listPanel.OnNVRSearch += ListPanelOnNVRSearch;

            _searchPanel = CreateSearchPanel();
            _searchPanel.Initialize();

            if (_editDevicePanel == null)
            {
                _editDevicePanel = new EditDevicePanel
                {
                    Server = Server,
                };
                _editDevicePanel.Initialize();
            }

            contentPanel.Controls.Contains(_listPanel);

            _syncBackgroundWorker = new BackgroundWorker();
            _syncBackgroundWorker.DoWork += SyncNVRSetting;
            _syncBackgroundWorker.RunWorkerCompleted += SyncNVRSettingCompleted;

            //_setupBackgroundWorker = new BackgroundWorker();
            //_setupBackgroundWorker.DoWork += RunSetupNVRSetting;
            //_setupBackgroundWorker.RunWorkerCompleted += SetupNVRSettingCompleted;
            Server.OnLoadComplete += ServerOnLoadComplete;
            Server.OnSaveFailure += ServerOnSaveComplete;
            Server.OnSaveComplete += ServerOnSaveComplete;

            _editPanel.OnDeviceEdit += ListPanelOnDeviceEdit;
            _editPanel.OnDeviceSelect += EditPanelOnDeviceSelect;
        }

        protected virtual SearchPanel CreateSearchPanel()
        {
            var search = new SearchPanel()
            {
                Server = Server,
                CMS = CMS
            };
            return search;
        }

        protected virtual ListPanel CreateListPanel()
        {
            var panel = new ListPanel
            {
                App = App,
                Server = Server,
                CMS = CMS,
                VAS = VAS,
                FOS = FOS,
                PTS = PTS
            };

            return panel;
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
                catch (Exception)
                {
                }

                if (_focusControl == null) _focusControl = _listPanel;
                Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowNVRList);
            }

        }

        private void ServerOnSaveComplete(object sender, EventArgs<string> e)
        {
            if (Parent != null && Parent.Visible && _focusControl == _searchPanel)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new RefreshContentDelegate(ServerOnSaveComplete), sender, e);
                        return;
                    }
                }
                catch (Exception)
                {
                }

                if (_focusControl == null) _focusControl = _listPanel;
                Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowNVRList);
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

            ShowNVRList();
        }

        private void ListPanelOnDeviceEdit(Object sender, EventArgs<IDevice> e)
        {
            EditDevice(e.Value);
        }

        private void EditDevice(IDevice device)
        {
            if (!(device is ICamera)) return;
            var camera = (ICamera)device;

            _focusControl = _editDevicePanel;
            _editDevicePanel.Camera = camera;

            Manager.ReplaceControl(_editPanel, _editDevicePanel, contentPanel, ManagerMoveToDeviceEditComplete);
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "Confirm":
                    RemoveNVR();
                    break;

                case "Delete":
                    DeleteNVR();
                    break;

                case "EditSetup":
                    SetupNVRSetting();
                    break;

                case "SearchNVR":
                    if (_isSearching)
                        _searchPanel.SearchNVR();
                    else
                        ListPanelOnNVRSearch(this, null);
                    break;

                case "Sync":
                case "GetDeviceList":
                    if (!_editPanel.CheckHost())
                    {
                        TopMostMessageBox.Show(Localization["LoginForm_SignInFailedConnectFailure"], Localization["MessageBox_Information"],
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (!_editPanel.CheckUser())
                    {
                        TopMostMessageBox.Show(Localization["LoginForm_SignInFailedAuthFailure"], Localization["MessageBox_Information"],
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    SyncNVRSetting();
                    break;

                default:
                    if (item == TitleName || item == "Back")
                    {
                        if (_focusControl is EditDevicePanel)
                        {
                            Manager.ReplaceControl(_focusControl, _editPanel, contentPanel, ManagerMoveToEditComplete);
                            _focusControl = _editPanel;
                        }
                        else
                        {
                            Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowNVRList);
                        }

                    }
                    //EditDevice(_presetPointPanel.Camera);
                    break;
            }
        }

        private void RemoveNVR()
        {
            //_listPanel.RemoveSelectedNVRs();

            //ShowNVRList();
            //ApplicationForms.ShowLoadingIcon(Server.Form);

            Application.Idle -= RemoveNVR;
            Application.Idle += RemoveNVR;
        }

        private void RemoveNVR(Object sender, EventArgs e)
        {
            Application.Idle -= RemoveNVR;
            _listPanel.RemoveSelectedNVRs();

            ShowNVRList();
            //ApplicationForms.HideLoadingIcon();
        }

        private void DeleteNVR()
        {
            _listPanel.SelectedColor = Manager.DeleteTextColor;
            _listPanel.ShowCheckBox();

            var text = TitleName + "  /  " + Localization["SetupTitle_Delete"];
            _listPanel.searchDoubleBufferPanel.Visible = false;
            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
        }

        //private Process _process;
        private void InitSetupForm()
        {
            //if (_nvrSetupForm != null)
            //{
            //    _nvrSetupForm.NVR = _editPanel.NVR;
            //    _nvrSetupForm.Location = new Point(Server.Form.Location.X + 20, Server.Form.Location.Y + 20);
            //    _nvrSetupForm.Size = new Size(Math.Max(Server.Form.Width - 40, 800), Math.Max(Server.Form.Height - 40, 600));
            //    _nvrSetupForm.UpdateServer();
            //    return;
            //}

            try
            {
                var credential = _editPanel.NVR.Credential;

                var process = new Process
                {
                    StartInfo =
                                       {
                                           FileName = "NVR.exe",
                                           Arguments = credential.Domain + " " +
                                           credential.Port + " " + credential.UserName + " " +
                                           credential.Password + " " + App.Language + " Setup",
                                           //"172.16.1.177 80 Admin 123456 zh-tw.resx Setup"
                                       },
                    //EnableRaisingEvents = true
                };
                //process.Exited += ProcessExited;
                process.Start();

                //_process = Process.Start("NVR.exe");
                //if (_process != null)
                //{
                //    _process.EnableRaisingEvents = true;
                //    _process.Exited += ProcessExited;
                //}
            }
            catch (Exception)
            {
            }

            //_nvrSetupForm = new SetupForm
            //{
            //    App = App,
            //    NVR = _editPanel.NVR,
            //    Location = new Point(Server.Form.Location.X + 20, Server.Form.Location.Y + 20),
            //    Size = new Size(Math.Max(Server.Form.Width - 40, 800), Math.Max(Server.Form.Height - 40, 600)),
            //};
            //_nvrSetupForm.Closing += NVRSetupFormClosing;

            //_nvrSetupForm.Initialize();
        }

        //private void ProcessExited(Object sender, EventArgs e)
        //{
        //    process = null;
        //}

        //private SetupForm _nvrSetupForm;
        //private Boolean _isEditing;
        private void SetupNVRSetting()
        {
            //if (_setupBackgroundWorker.IsBusy) return;
            //if (_nvrSetupForm != null && _nvrSetupForm.Visible)
            //{
            //    TopMostMessageBox.Show(Localization["SetupNVR_SetupNVRInProgress"], Localization["MessageBox_Information"],
            //                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    _nvrSetupForm.Show();
            //    _nvrSetupForm.BringToFront();

            //    return;
            //}

            //if (_process != null)
            //{
            //    TopMostMessageBox.Show(Localization["SetupNVR_SetupNVRInProgress"], Localization["MessageBox_Information"],
            //                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    //_nvrSetupForm.Show();
            //    //_nvrSetupForm.BringToFront();

            //    return;
            //}

            ApplicationForms.ShowLoadingIcon(Server.Form);
            Application.RaiseIdle(null);
            RunSetupNVRSetting();
            //_setupBackgroundWorker.RunWorkerAsync();
        }

        private void RunSetupNVRSetting()//Object sender, DoWorkEventArgs e)
        {
            _loging = false;
            if (!_editPanel.NVR.ValidateCredentialWithMessage())
            {
                ApplicationForms.HideLoadingIcon();
                Application.RaiseIdle(null);
                return;
            }

            //_isEditing = true;
            if (_editPanel.NVR.ReadyState == ReadyState.Ready || _editPanel.NVR.ReadyState == ReadyState.Modify)
            {
                SetupNVRSettingCompleted();
                return;
            }

            _loging = true;
            LoginNVR();
            SetupNVRSettingCompleted();
        }

        private void SetupNVRSettingCompleted()//Object sender, RunWorkerCompletedEventArgs e)
        {
            while (_loging)
            {
                Thread.Sleep(500);
            }

            ApplicationForms.HideLoadingIcon();
            Application.RaiseIdle(null);

            if (_editPanel.NVR.ReadyState == ReadyState.Ready || _editPanel.NVR.ReadyState == ReadyState.Modify)
            {
                InitSetupForm();

                if (CMS != null)
                    CMS.NVRModify(_editPanel.NVR);
                else if (PTS != null)
                    PTS.NVRModify(_editPanel.NVR);
                else if (VAS != null)
                    VAS.NVRModify(_editPanel.NVR);
            }
            else
            {
                if (_editPanel.NVR.ReadyState != ReadyState.New)
                    _editPanel.NVR.ReadyState = ReadyState.Unavailable;
            }
        }

        private Boolean _isSync;
        private void SyncNVRSetting()
        {
            if (_isSync) return;
            if (_syncBackgroundWorker.IsBusy) return;

            if (PTS != null)
            {
                DialogResult result = MessageBox.Show(Localization["SetupNVR_SyncConfirm"], Localization["MessageBox_Confirm"],
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result != DialogResult.OK) return;
            }
            else if (CMS != null)
            {

            }
            else if (VAS == null && FOS == null)
                return;

            _isSync = true;
            ApplicationForms.ShowLoadingIcon(Server.Form);
            _syncBackgroundWorker.RunWorkerAsync();
        }

        private void SyncNVRSetting(Object sender, DoWorkEventArgs e)
        {
            if (CMS != null)
            {
                _editPanel.NVR.ReadNVRDeviceWithoutSaving();
            }
            else
            {
                if (_editPanel.NVR.ValidateCredentialWithMessage())
                {
                    LoginNVR();
                }
                else
                {
                    if (_editPanel.NVR.ReadyState != ReadyState.New)
                        _editPanel.NVR.ReadyState = ReadyState.Unavailable;

                    _isSync = false;
                    ApplicationForms.HideLoadingIcon();
                }
            }
            Application.RaiseIdle(null);

            if (CMS != null)
            {
                _isSync = false;
                ApplicationForms.HideLoadingIcon();
            }

        }

        private void SyncNVRSettingCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (CMS != null)
            {
                _editPanel.NVR.ReadyState = ReadyState.Modify;
                CMS.NVRModify(_editPanel.NVR);
                _editPanel.GenerateViewModel();
                GC.Collect();
            }

            _syncBackgroundWorker.Dispose();
        }

        private void SyncNVRSettingCompleted()
        {
            if (CMS != null)
            {
                CMS.NVRModify(_editPanel.NVR);

                if (_editPanel.NVR.Utility != null)
                {
                    _editPanel.NVR.Utility.Server = _editPanel.NVR;
                    CMS.ListenNVREvent(_editPanel.NVR);
                }
            }
            else if (PTS != null)
            {
                PTS.NVRModify(_editPanel.NVR);

                if (_editPanel.NVR.Utility != null)
                {
                    _editPanel.NVR.Utility.Server = _editPanel.NVR;
                    PTS.ListenNVREvent(_editPanel.NVR);
                }
            }
            else if (VAS != null)
            {
                VAS.NVRModify(_editPanel.NVR);
            }

            _isSync = false;
            ApplicationForms.HideLoadingIcon();
        }

        private void LoginNVR()
        {
            _editPanel.NVR.OnLoadComplete -= NVROnSilentLoadComplete;
            _editPanel.NVR.OnLoadFailure -= NVROnSilentLoadFailure;

            _editPanel.NVR.OnLoadComplete += NVROnSilentLoadComplete;
            _editPanel.NVR.OnLoadFailure += NVROnSilentLoadFailure;
            _editPanel.NVR.SilentLoad();
        }

        private void NVROnSilentLoadComplete(Object sender, EventArgs<String> e)
        {
            _editPanel.NVR.OnLoadComplete -= NVROnSilentLoadComplete;
            _editPanel.NVR.OnLoadFailure -= NVROnSilentLoadFailure;

            if (CMS != null || VAS != null)
                UpdateDeviceGroupAfterSyncNVR(_editPanel.NVR);

            _loging = false;

            var isNVR = false;

            if (CMS != null || VAS != null)
            {
                isNVR = (_editPanel.NVR.Server.ProductNo == "00001" || _editPanel.NVR.Server.ProductNo == "00004" || _editPanel.NVR.Server.ProductNo == "00005" || _editPanel.NVR.Server.ProductNo == "00121");
            }
            else if (PTS != null)
            {
                switch (_editPanel.NVR.Manufacture)
                {
                    case "Salient":
                        isNVR = true;
                        break;

                    //case "iSap":
                    default:
                        isNVR = (_editPanel.NVR.Server.ProductNo == "00001" || _editPanel.NVR.Server.ProductNo == "00004" || _editPanel.NVR.Server.ProductNo == "00005");
                        break;
                }
            }
            else if (FOS != null)
            {
                //failover dont support linux nvr server
                // || _editPanel.NVR.Server.ProductNo != "00005"
                isNVR = (_editPanel.NVR.Server.ProductNo == "00001" || _editPanel.NVR.Server.ProductNo == "00004");
            }

            SyncNVRSettingCompleted();

            if (!isNVR)
            {
                ApplicationForms.HideLoadingIcon();

                TopMostMessageBox.Show(Localization["Application_ServerIsNotNVR"].Replace("%1", _editPanel.NVR.Credential.Domain), Localization["MessageBox_Error"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (_editPanel.NVR.ReadyState == ReadyState.Unavailable || _editPanel.NVR.ReadyState == ReadyState.Ready)
                _editPanel.NVR.ReadyState = ReadyState.Modify;

            ApplicationForms.HideLoadingIcon();
        }

        private void NVROnSilentLoadFailure(Object sender, EventArgs<String> e)
        {
            _editPanel.NVR.OnLoadComplete -= NVROnSilentLoadComplete;
            _editPanel.NVR.OnLoadFailure -= NVROnSilentLoadFailure;
            _editPanel.NVR.ReadyState = ReadyState.Unavailable;

            _loging = false;

            SyncNVRSettingCompleted();

            ApplicationForms.HideLoadingIcon();

            TopMostMessageBox.Show(Localization["LoginForm_SignInTimeout"], Localization["MessageBox_Error"],
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateDeviceGroupAfterSyncNVR(INVR nvr)
        {
            if (Server.Device == null || Server.Device.Groups.Count == 0) return;
            foreach (var obj in Server.Device.Groups)
            {
                var devices = new List<IDevice>(obj.Value.Items);
                obj.Value.Items.Clear();

                foreach (IDevice device in devices)
                {
                    if (device.Server == nvr)
                    {
                        if (nvr.Device.Devices.ContainsKey(device.Id))
                            obj.Value.Items.Add(nvr.Device.Devices[device.Id]);
                    }
                    else
                    {
                        obj.Value.Items.Add(device);
                    }
                }

                devices = new List<IDevice>(obj.Value.View);
                obj.Value.View.Clear();

                foreach (IDevice device in devices)
                {
                    if (device == null)
                    {
                        obj.Value.View.Add(null);
                        continue;
                    }

                    if (device.Server == nvr)
                    {
                        if (nvr.Device.Devices.ContainsKey(device.Id))
                            obj.Value.View.Add(nvr.Device.Devices[device.Id]);
                    }
                    else
                    {
                        obj.Value.View.Add(device);
                    }
                }

                if (obj.Value.Items.Count > 0)
                    obj.Value.Items.Sort((x, y) => (x.Id - y.Id));
            }
        }

        private Boolean _loging = true;

        //private void NVRSetupFormClosing(Object sender, CancelEventArgs e)
        //{
        //    //_nvrSetupForm = null;
        //    _isEditing = false;
        //    try
        //    {
        //        if (CMS != null)
        //            CMS.NVRModify(_editPanel.NVR);
        //        else if (VAS != null)
        //            VAS.NVRModify(_editPanel.NVR);
        //        else if (PTS != null)
        //            PTS.NVRModify(_editPanel.NVR);

        //        ApplicationForms.HideLoadingIcon();

        //        _editPanel.NVR.Form = Server.Form;

        //        _editPanel.CompareNVRSettingAfterCloseSetupForm();
        //        _editPanel.ParseNVR();
        //        Server.Form.Show();
        //        Server.Form.BringToFront();
        //    }
        //    catch(Exception)
        //    {
        //    }
        //}

        private void ServerOnNVRStatusUpdate(Object sender, EventArgs e)
        {
            if (_focusControl == _listPanel && Parent.Visible)
                _listPanel.Invalidate();
        }

        private void EditPanelOnDeviceSelect(object sender, EventArgs e)
        {
            String count = "";
            String buttons = "";
            if (CMS != null && CMS.NVRManager.NVRs.Count > 0)
            {
                count = " (" + Localization["SetupNVR_NVRDevice"] + "   " + CMS.NVRManager.DeviceChannelTable.Count + "/" + Server.License.Amount + ")";

                buttons = "GetDeviceList";

                var text = TitleName + "  /  " + _editPanel.NVR;

                if (OnSelectionChange != null)
                    OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text + count, "Back", buttons)));
            }
        }

        protected virtual void ShowNVRList()
        {
            _isSearching = false;
            _focusControl = _listPanel;

            _listPanel.Enabled = true;
            if (!contentPanel.Controls.Contains(_listPanel))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(_listPanel);
            }

            _listPanel.GenerateViewModel();

            if (OnSelectionChange == null) return;
            String buttons = "";

            _listPanel.searchDoubleBufferPanel.Visible = false;

            if (CMS != null)
            {
                _listPanel.searchDoubleBufferPanel.Visible = true;
                if (CMS.NVRManager.NVRs.Count > 0)
                {
                    buttons = "Delete";
                }
            }
            else if (FOS != null && FOS.NVR.NVRs.Count > 0)
                buttons = "Delete";
            else if (PTS != null && PTS.NVR.NVRs.Count > 0)
                buttons = "Delete";
            else if (VAS != null && VAS.NVR.NVRs.Count > 0)
                buttons = "Delete";

            String count = "";
            if (CMS != null && CMS.NVRManager.NVRs.Count > 0)
            {
                count = " (" + Localization["SetupNVR_NVRDevice"] + "   " + CMS.NVRManager.DeviceChannelTable.Count + "/" + Server.License.Amount + ")";
            }
            else if (FOS != null && FOS.NVR.NVRs.Count > 0)
            {
                count = " (" + FOS.NVR.NVRs.Count + "/" + Server.License.Amount + ")";
            }
            else if (VAS != null && VAS.NVR.NVRs.Count > 0)
            {
                count = " (" + VAS.NVR.NVRs.Count + ")";
            }
            //else if (PTS != null && PTS.NVR.NVRs.Count > 0)
            //{
            //    count = " (" + PTS.NVR.NVRs.Count + ")";
            //}

            OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName + count,
                                                                                      "", buttons)));
        }

        private void ListPanelOnNVREdit(Object sender, EventArgs<INVR> e)
        {
            EditNVR(e.Value);
        }

        protected virtual INVR CreateNvr()
        {
            INVR nvr = null;
            if (CMS != null)
            {
                nvr = new NVR
                {
                    Id = CMS.NVRManager.GetNewNVRId(),
                    ServerManager = Server,
                    Driver = "iSap",
                    Manufacture = "iSap"
                };

            }
            else if (FOS != null)
            {
                nvr = new NVR
                {
                    Id = FOS.NVR.GetNewNVRId(),
                    FailoverSetting = new FailoverSetting(),
                };
            }
            else if (PTS != null)
            {
                nvr = PTS.CreateNewNVR();
            }
            else if (VAS != null)
            {
                nvr = new NVR
                {
                    Id = VAS.NVR.GetNewNVRId(),
                };
            }

            return nvr;
        }

        private void ListPanelOnNVRAdd(Object sender, EventArgs e)
        {
            INVR nvr = CreateNvr();

            if (nvr == null) return;

            if (nvr.Id == 0)
            {
                if (CMS != null)
                {
                    TopMostMessageBox.Show(Localization["SetupNVR_MaximumAmount"].Replace("%1", CMS.NVRManager.MaximunNVRAmount.ToString()),
                        Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TopMostMessageBox.Show(Localization["SetupNVR_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            nvr.Form = Server.Form;
            nvr.ReadyState = ReadyState.JustAdd;
            nvr.Name = Localization["SetupNVR_NewNVR"] + @" " + nvr.Id;
            nvr.Initialize();

            if (CMS != null && !CMS.NVRManager.NVRs.ContainsKey(nvr.Id))
            {
                nvr.Server.TimeZone = Server.Server.TimeZone;
                CMS.NVRManager.NVRs.Add(nvr.Id, nvr);
                CMS.NVRModify(nvr);

                nvr.Configure.BandwidthControlBitrate = Bitrate.NA;
                nvr.Configure.BandwidthControlStream = 1;
                nvr.Configure.CustomStreamSetting.Enable = true;
            }
            else if (FOS != null && !FOS.NVR.NVRs.ContainsKey(nvr.Id))
            {
                FOS.NVR.NVRs.Add(nvr.Id, nvr);
            }
            else if (PTS != null && !PTS.NVR.NVRs.ContainsKey(nvr.Id))
            {
                PTS.NVR.NVRs.Add(nvr.Id, nvr);
            }
            else if (VAS != null && !VAS.NVR.NVRs.ContainsKey(nvr.Id))
            {
                VAS.NVR.NVRs.Add(nvr.Id, nvr);
            }

            nvr.ReadyState = ReadyState.New;
            Server.WriteOperationLog("Add New NVR %1".Replace("%1", nvr.Id.ToString()));

            EditNVR(nvr);
        }

        private void ListPanelOnNVRSearch(object sender, EventArgs e)
        {
            _isSearching = true;
            _focusControl = _searchPanel;
            _searchPanel.ApplyManufactures(_listPanel.SelectedManufacturer);
            _searchPanel.ClearViewModel();
            Manager.ReplaceControl(_listPanel, _searchPanel, contentPanel, ManagerMoveToSearchComplete);
        }

        private void EditNVR(INVR nvr)
        {
            _focusControl = _editPanel;
            _editPanel.NVR = nvr;

            Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
        }

        private void ManagerMoveToSearchComplete()
        {
            _searchPanel.SearchNVR();

            var text = TitleName + "  /  " + Localization["SetupNVR_SearchNVR"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
                         text, "Back", "SearchNVR")));
        }

        private void ManagerMoveToEditComplete()
        {
            _editPanel.ParseNVR();

            String buttons = "";
            String count = "";
            if (CMS != null)
            {
                buttons = "GetDeviceList";
                count = " (" + Localization["SetupNVR_NVRDevice"] + "   " + CMS.NVRManager.DeviceChannelTable.Count + "/" + Server.License.Amount + ")";
            }
            if (VAS != null || FOS != null || PTS != null)
                buttons = "Sync";

            var text = TitleName + "  /  " + _editPanel.NVR;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text + count, "Back", buttons)));
        }

        private void ManagerMoveToDeviceEditComplete()
        {
            _editDevicePanel.Parse();

            const string buttons = "";

            var text = TitleName + "  /  " + _editDevicePanel.Camera.Server + "  /  " + _editDevicePanel.Camera;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", buttons)));
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowNVRList();
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

        public void SISSetupView()
        {
            if (_editPanel != null)
            {
                _editPanel.SISSetupView();
            }
        }
    }
}
