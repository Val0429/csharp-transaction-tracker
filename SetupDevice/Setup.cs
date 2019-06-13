using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App;
using Constant;
using Device;
using DeviceCab;
using DeviceConstant;
using Interface;
using SetupBase;

namespace SetupDevice
{
    public partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMouseHandler, IAppUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public String TitleName { get; set; }
        public IApp App { get; set; }
        public IServer Server { get; set; }
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

        protected ListPanel ListPanel;
        protected EditPanel EditPanel;
        protected SearchPanel SearchPanel;
        protected CopySettingPanel CopySettingPanel;
        protected PresetPointPanel PresetPointPanel;
        protected PresetTourPanel PresetTourPanel;
        protected PtzPatrolPanel PtzPatrolPanel;
        protected MotionSettingPanel MotionSettingPanel;
        protected Control FocusControl;

        public Setup()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Device", "Device"},
								   
								   {"MessageBox_Information", "Information"},
								   
								   {"SetupDevice_MaximumLicense", "Reached maximum license limit \"%1\""},
								   {"SetupDevice_AddDevice", "Add Device"},
								   {"SetupDevice_CopySetting", "Copy Setting"},
								   {"SetupDevice_CloneDevice", "Clone Device"},
								   {"SetupDevice_DeleteDevice", "Delete Device"},
								   {"SetupDevice_MotionSetting", "Motion Area Setting"},
								   {"SetupDevice_PresetPointSetting", "Preset Point Setting"},
								   {"SetupDevice_PresetTourSetting", "Preset Tour Setting"},
                                   {"SetupDevice_PTZPatrol", "Digital PTZ Patrol Setting"},
							   };
            Localizations.Update(Localization);

            BackgroundImage = Manager.Background;

            Name = "Device";
            TitleName = Localization["Control_Device"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Device"] };
            Icon.Click += DockIconClick;

            //SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            
            var toolTip = new ToolTip { ShowAlways = true };
            toolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            ListPanel = new ListPanel
            {
                Server = Server,
            };
            ListPanel.Initialize();

            if (EditPanel == null)
            {
                EditPanel = new EditPanel
                {
                    Server = Server,
                };
                EditPanel.Initialize();
            }
            SearchPanel = new SearchPanel
            {
                Server = Server,
            };
            SearchPanel.Initialize();

            CopySettingPanel = new CopySettingPanel
            {
                Server = Server,
            };
            CopySettingPanel.Initialize();

            PresetPointPanel = new PresetPointPanel
            {
                App = App,
                Server = Server,
            };
            PresetPointPanel.Initialize();

            PresetTourPanel = new PresetTourPanel
            {
                Server = Server,
            };
            PresetTourPanel.Initialize();

            MotionSettingPanel = new MotionSettingPanel
            {
                Server = Server,
            };
            MotionSettingPanel.Initialize();

            PtzPatrolPanel = new PtzPatrolPanel()
            {
                App = App,
                Server = Server,
            };
            PtzPatrolPanel.Initialize();

            EditPanel.OnPtzPatrol += EditPanelOnPtzPatrol;
            EditPanel.OnMotionSetting += EditPanelOnMotionSetting;
            EditPanel.OnPresetPointSetting += EditPanelOnPresetPointSetting;
            EditPanel.OnPresetTourSetting += EditPanelOnPresetTourSetting;

            ListPanel.OnDeviceEdit += ListPanelOnDeviceEdit;
            ListPanel.OnDeviceAdd += ListPanelOnDeviceAdd;
            ListPanel.OnDeviceSearch += ListPanelOnDeviceSearch;

            SearchPanel.OnSearchStart += SearchPanelOnSearchStart;
            SearchPanel.OnSearchComplete += SearchPanelOnSearchComplete;
            SearchPanel.OnDeviceModify += SearchPanelOnDeviceModify;

            contentPanel.Controls.Contains(ListPanel);

            Server.OnSaveFailure += RefreshDeviceList;
            Server.OnSaveComplete += RefreshDeviceList;

            Server.OnLoadComplete += RefreshDeviceList;
        }

        private void RefreshDeviceList(Object sender, EventArgs<String> e)
        {
            //if ((FocusControl == ListPanel || FocusControl == SearchPanel) && Parent.Visible)
            if (Parent != null && Parent.Visible)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action<object, EventArgs<String>>(RefreshDeviceList), sender, e);
                        return;
                    }
                }
                catch (Exception)
                {
                }
                ShowDeviceList();
            }
        }

        private void ShowDeviceList()
        {
            if (FocusControl == PresetPointPanel)
                PresetPointPanel.Deactivate();

            if (FocusControl == SearchPanel)
            {
                foreach (var keyValuePair in Server.Device.Devices.Where(keyValuePair => keyValuePair.Value.ReadyState == ReadyState.New))
                {
                    Server.WriteOperationLog("Add Device [%1]".Replace("%1", keyValuePair.Value.Id.ToString()));
                }
            }

            _isSearching = false;
            FocusControl = ListPanel;

            ListPanel.Enabled = true;
            if (!contentPanel.Controls.Contains(ListPanel))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(ListPanel);
            }

            ListPanel.GenerateViewModel();

            if (OnSelectionChange != null)
            {
                String buttons = "";

                if (Server.Device.Devices.Count == 1)
                    buttons = "Delete,Clone";
                else if (Server.Device.Devices.Count > 1)
                {
                    buttons = (Server.Device.Devices.Count != Server.License.Amount)
                        ? "Delete,Clone,CopySetting"
                        : "Delete,CopySetting";
                }

                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         TitleName + " (" + Server.Device.Devices.Count + "/" + Server.License.Amount + ")", "", buttons)));
            }
        }

        private void EditPanelOnPtzPatrol(object sender, EventArgs e)
        {
            EditPtzPatrol();
        }

        private void EditPanelOnMotionSetting(Object sender, EventArgs e)
        {
            EditDeviceMotionSetting();
        }

        private void EditPanelOnPresetPointSetting(Object sender, EventArgs e)
        {
            EditPresetPoint();
        }

        private void EditPanelOnPresetTourSetting(Object sender, EventArgs e)
        {
            EditPresetTour();
        }


        private void ListPanelOnDeviceEdit(Object sender, EventArgs<IDevice> e)
        {
            EditDevice(e.Value);
        }

        private void ListPanelOnDeviceSearch(Object sender, EventArgs e)
        {
            SearchDevice();
        }

        private void ListPanelOnDeviceAdd(Object sender, EventArgs e)
        {
            var camera = new Camera
            {
                Server = Server,
                Id = Server.Device.GetNewDeviceId(),
                ReadyState = ReadyState.JustAdd,
            };

            if (camera.Id == 0)
            {
                //TopMostMessageBox.Show(Localization["SetupDevice_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
                //    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Server.WriteOperationLog("Add Device [%1]".Replace("%1", camera.Id.ToString()));

            camera.Profile = new CameraProfile();

            camera.Profile.StreamConfigs.Add(1, new StreamConfig());

            camera.Model = Server.Device.Manufacture.Values.First().First();
            camera.Mode = camera.Model.CameraMode[0];
            camera.Name = camera.Model.Model;

            ProfileChecker.SetDefaultMode(camera, camera.Model);
            switch (camera.Mode)
            {
                case CameraMode.Single:
                    break;

                case CameraMode.Dual:
                    camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;

                case CameraMode.Triple:
                    camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;

                case CameraMode.Multi:
                case CameraMode.FourVga:
                    camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;

                case CameraMode.Five:
                    camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;

                case CameraMode.SixVga:
                    camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    camera.Profile.StreamConfigs.Add(6, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;
            }
            ProfileChecker.SetDefaultAccountPassword(camera, camera.Model);
            ProfileChecker.SetDefaultProtocol(camera, camera.Model);
            ProfileChecker.SetDefaultPort(camera, camera.Model);
            ProfileChecker.SetDefaultMulticastNetworkAddress(camera, camera.Model);
            ProfileChecker.SetDefaultAudioOutPort(camera, camera.Model);
            ProfileChecker.SetDefaultTvStandard(camera, camera.Model);
            ProfileChecker.SetDefaultSensorMode(camera, camera.Model);
            ProfileChecker.SetDefaultPowerFrequency(camera, camera.Model);
            ProfileChecker.SetDefaultAspectRatio(camera, camera.Model);

            foreach (var config in camera.Profile.StreamConfigs)
            {
                ProfileChecker.SetDefaultCompression(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultResolution(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultFramerate(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultBitrate(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.SetDefaultBitrateControl(camera.Model, config.Value);
            }

            camera.RecordSchedule = new Schedule();
            camera.RecordSchedule.SetDefaultSchedule(ScheduleType.Continuous);
            camera.RecordSchedule.Description = ScheduleMode.FullTimeRecording;

            camera.EventSchedule = new Schedule();
            camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
            camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;

            camera.EventHandling = new EventHandling();
            camera.EventHandling.SetDefaultEventHandling(camera.Model);

            if (!Server.Device.Devices.ContainsKey(camera.Id))
                Server.Device.Devices.Add(camera.Id, camera);

            var allDeviceGroup = Server.Device.Groups.Values.First();

            if (allDeviceGroup != null)
            {
                if (!allDeviceGroup.Items.Contains(camera))
                {
                    allDeviceGroup.Items.Add(camera);
                    allDeviceGroup.Items.Sort((x, y) => (x.Id - y.Id));
                }

                if (!allDeviceGroup.View.Contains(camera))
                {
                    allDeviceGroup.View.Add(camera);
                    allDeviceGroup.View.Sort((x, y) => (x.Id - y.Id));
                }

                Server.GroupModify(allDeviceGroup);
            }

            EditDevice(camera);
        }

        public void Activate()
        {
            if (FocusControl == PresetPointPanel)
                PresetPointPanel.Activate();
        }

        public void Deactivate()
        {
            if (FocusControl == PresetPointPanel)
                PresetPointPanel.Deactivate();
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            ShowDeviceList();
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!SetupBase.Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "Confirm":
                    ListPanel.RemoveSelectedDevices();

                    ShowDeviceList();
                    break;

                case "Duplicate":
                    ListPanel.CloneSelectedDevices();

                    ShowDeviceList();
                    break;

                case "Delete":
                    DeleteDevice();
                    break;

                case "Clone":
                    CloneDevice();
                    break;

                case "CopySetting":
                    CopySetting();
                    break;

                case "Search":
                    if (_isSearching)
                        SearchPanel.SearchDevice();
                    else
                        SearchDevice();
                    break;

                case "OpenWebPage":
                    EditPanel.OpenWebPage();
                    break;

                default:
                    if (item == TitleName || item == "Back")
                    {
                        if (FocusControl == MotionSettingPanel)
                        {
                            EditDevice(MotionSettingPanel.Camera);
                            return;
                        }

                        if (FocusControl == PresetPointPanel)
                        {
                            EditDevice(PresetPointPanel.Camera);
                            return;
                        }

                        if (FocusControl == PresetTourPanel)
                        {
                            EditDevice(PresetTourPanel.Camera);
                            return;
                        }

                        if (FocusControl == PtzPatrolPanel)
                        {
                            EditDevice(PtzPatrolPanel.Camera);
                            return;
                        }

                        SetupBase.Manager.ReplaceControl(FocusControl, ListPanel, contentPanel, ShowDeviceList);

                    }
                    break;
            }
        }

        private void EditPtzPatrol()
        {
            if (EditPanel.Camera == null) return;

            FocusControl = PtzPatrolPanel;
            PtzPatrolPanel.Camera = EditPanel.Camera;

            SetupBase.Manager.ReplaceControl(ListPanel, PtzPatrolPanel, contentPanel, ManagerMoveToPtzPatrolComplete);
        }

        private void EditPresetPoint()
        {
            if (EditPanel.Camera == null) return;

            FocusControl = PresetPointPanel;
            PresetPointPanel.Camera = EditPanel.Camera;

            SetupBase.Manager.ReplaceControl(ListPanel, PresetPointPanel, contentPanel, ManagerMoveToPresetPointComplete);
        }

        private void EditPresetTour()
        {
            if (EditPanel.Camera == null) return;

            FocusControl = PresetTourPanel;
            PresetTourPanel.Camera = EditPanel.Camera;

            SetupBase.Manager.ReplaceControl(ListPanel, PresetTourPanel, contentPanel, ManagerMoveToPresetTourComplete);
        }

        private void EditDeviceMotionSetting()
        {
            if (FocusControl == MotionSettingPanel)
                MotionSettingPanel.Deactivate();

            if (EditPanel.Camera == null) return;

            FocusControl = MotionSettingPanel;

            ApplicationForms.ShowLoadingIcon(Server.Form);

            MotionSettingPanel.Camera = EditPanel.Camera;

            MotionSettingPanel.ParseDevice();

            ApplicationForms.HideLoadingIcon();

            SetupBase.Manager.ReplaceControl(EditPanel, MotionSettingPanel, contentPanel, ManagerMoveToMotionSettingComplete);
        }

        private void CopySetting()
        {
            FocusControl = CopySettingPanel;

            CopySettingPanel.GenerateViewModel();

            SetupBase.Manager.ReplaceControl(ListPanel, CopySettingPanel, contentPanel, ManagerMoveToCopyComplete);
        }

        protected virtual void EditDevice(IDevice device)
        {
            if (FocusControl == PresetPointPanel)
                PresetPointPanel.Deactivate();

            if (!(device is ICamera)) return;
            var camera = (ICamera)device;

            //select first model in manufacture
            if (String.Equals(camera.Model.Model, "UNKNOWN"))
            {
                if (!Server.Device.Manufacture.ContainsKey(camera.Model.Manufacture)) return;

                camera.Model = Server.Device.Manufacture[camera.Model.Manufacture][0];
                camera.Name = camera.Model.Model;

                ProfileChecker.SetDefaultProtocol(camera, camera.Model);
                ProfileChecker.SetDefaultMode(camera, camera.Model);
                ProfileChecker.SetDefaultTvStandard(camera, camera.Model);
                ProfileChecker.SetDefaultSensorMode(camera, camera.Model);
                ProfileChecker.SetDefaultPowerFrequency(camera, camera.Model);
                ProfileChecker.SetDefaultAspectRatio(camera, camera.Model);
                ProfileChecker.SetDefaultCompression(camera, camera.Model);

                foreach (var config in camera.Profile.StreamConfigs)
                {
                    ProfileChecker.CheckAvailableSetDefaultResolution(camera, camera.Model, config.Value, config.Key);
                    ProfileChecker.CheckAvailableSetDefaultFramerate(camera, camera.Model, config.Value, config.Key);
                    ProfileChecker.CheckAvailableSetDefaultBitrate(camera, camera.Model, config.Value, config.Key);
                    ProfileChecker.SetDefaultBitrateControl(camera.Model, config.Value);
                }
            }

            FocusControl = EditPanel;
            EditPanel.Camera = camera;

            SetupBase.Manager.ReplaceControl(ListPanel, EditPanel, contentPanel, ManagerMoveToEditComplete);
        }

        private Boolean _isSearching;
        private void SearchDevice()
        {
            _isSearching = true;
            FocusControl = SearchPanel;

            SearchPanel.ApplyManufactures(ListPanel.SelectedManufacturer);

            SearchPanel.ClearViewModel();

            SetupBase.Manager.ReplaceControl(ListPanel, SearchPanel, contentPanel, ManagerMoveToSearchComplete);
        }

        private void DeleteDevice()
        {
            ListPanel.SelectedColor = SetupBase.Manager.DeleteTextColor;
            ListPanel.ShowCheckBox();

            var text = TitleName + "  /  " + Localization["SetupDevice_DeleteDevice"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "Confirm")));
        }

        private void CloneDevice()
        {
            ListPanel.SelectedColor = SetupBase.Manager.SelectedTextColor;
            ListPanel.ShowCheckBox();

            var text = TitleName + "  /  " + Localization["SetupDevice_CloneDevice"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "Duplicate")));
        }

        private void ManagerMoveToCopyComplete()
        {
            var text = TitleName + "  /  " + Localization["SetupDevice_CopySetting"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "")));
        }

        private void ManagerMoveToSearchComplete()
        {
            if (!String.Equals(ListPanel.SelectedManufacturer, "ALL"))
                SearchPanel.SearchDevice();

            var text = TitleName + "  /  " + Localization["SetupDevice_AddDevice"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "Search")));
        }

        protected virtual void ManagerMoveToEditComplete()
        {
            String text = TitleName + "  /  " + Localization["SetupDevice_AddDevice"];

            if (EditPanel.Camera.ReadyState == ReadyState.JustAdd)
            {
                Server.DeviceModify(EditPanel.Camera);
            }
            else
            {
                text = TitleName + "  /  " + EditPanel.Camera;
            }

            EditPanel.ParseDevice();

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                        text, "Back", "OpenWebPage")));
        }

        private void ManagerMoveToPtzPatrolComplete()
        {
            PtzPatrolPanel.ParseDevice();
            PtzPatrolPanel.Activate();

            String text = TitleName + "  /  " + PtzPatrolPanel.Camera + "  /  " + Localization["SetupDevice_PTZPatrol"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "")));
        }

        private void ManagerMoveToPresetPointComplete()
        {
            PresetPointPanel.ParseDevice();
            PresetPointPanel.Activate();

            String text = TitleName + "  /  " + PresetPointPanel.Camera + "  /  " + Localization["SetupDevice_PresetPointSetting"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "")));
        }

        private void ManagerMoveToPresetTourComplete()
        {
            PresetTourPanel.ParsePresetTour();

            String text = TitleName + "  /  " + PresetTourPanel.Camera + "  /  " + Localization["SetupDevice_PresetTourSetting"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                    text, "Back", "")));
        }

        private void ManagerMoveToMotionSettingComplete()
        {
            String text = TitleName + "  /  " + MotionSettingPanel.Camera + "  /  " + Localization["SetupDevice_MotionSetting"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "")));
        }

        public void GlobalMouseHandler()
        {
            if (FocusControl == PresetPointPanel)
                PresetPointPanel.GlobalMouseHandler();
        }

        private void SearchPanelOnSearchStart(Object sender, EventArgs e)
        {
            if (!_isSearching) return;

            String text = TitleName + "  /  " + Localization["SetupDevice_AddDevice"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "")));
        }

        private void SearchPanelOnSearchComplete(Object sender, EventArgs e)
        {
            if (!_isSearching) return;

            String text = TitleName + "  /  " + Localization["SetupDevice_AddDevice"] + " (" + Server.Device.Devices.Count + "/" + Server.License.Amount + ")";

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "Search")));
        }

        private void SearchPanelOnDeviceModify(Object sender, EventArgs e)
        {
            String text = TitleName + "  /  " + Localization["SetupDevice_AddDevice"] + " (" + Server.Device.Devices.Count + "/" + Server.License.Amount + ")";

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(SetupBase.Manager.SelectionChangedXml(TitleName,
                         text, "Back", "Search")));
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowDeviceList();
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
    }
}
