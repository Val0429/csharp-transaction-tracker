using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Device;
using DeviceConstant;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using PanelBase;

namespace DeviceTree
{
    public partial class DeviceList : UserControl, IControl, IServerUse, IAppUse, IDrag, IDrop, IMinimize, IMouseHandler, IBlockPanelUse
    {
        public event EventHandler OnMinimizeChange;

        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;

        public event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;
        public event EventHandler<EventArgs<IDeviceLayout>> OnImmerVisionDeviceLayoutDoubleClick;

        public event EventHandler<EventArgs<INVR>> OnNVRDoubleClick;

        public event EventHandler<EventArgs<Object>> OnPatrolStart;
        public event EventHandler<EventArgs<UInt16>> OnPageChange;
        public event EventHandler<EventArgs<Object>> OnDragStart;
        public event EventHandler OnSaveView;

        public event EventHandler<EventArgs<String>> OnMultiScreenPatrolStart;
        public event EventHandler<EventArgs<String>> OnMultiScreenPatrolStop;

        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();

        protected ToolStripMenuItemUI2 PatrolMenuItem { get; set; }
        protected ToolStripMenuItemUI2 MultiScreenPatrolMenuItem { get; set; }
        protected ToolStripMenuItemUI2 SaveViewMenuItem { get; set; }

        public Label DragDropLabel { get; private set; }
        public Panel DragDropProxy { get; private set; }

        public Dictionary<String, String> Localization;

        public String TitleName { get; set; }

        public Button Icon { get; private set; }

        private static readonly Image _icon = Resources.GetResources(Properties.Resources.deviceListIcon, Properties.Resources.IMGDeviceListIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.deviceListIcon_activate, Properties.Resources.IMGDeviceListIconActivate);

        public IApp App { get; set; }

        protected ICMS CMS { get; set; }
        protected INVR NVR { get; set; }
        protected IPTS PTS { get; set; }
        private IServer _server;
        public IBlockPanel BlockPanel { get; set; }
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is ICMS)
                    CMS = value as ICMS;
                else if (value is IPTS)
                    PTS = value as IPTS;
                else if (value is INVR)
                    NVR = value as INVR;
            }
        }

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }
        public Boolean IsMinimize { get; private set; }
        private readonly System.Timers.Timer _patrolDeviceTimer = new System.Timers.Timer();

        private Boolean _askPatrol = true;

        public DeviceList()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Device", "Device"},

								   {"MessageBox_Confirm", "Confirm"},

								   {"SetupDeviceGroup_NewView", "New View"},
								   {"DeviceView_SaveView", "Save View"},

								   {"DeviceTree_ImageStitching", "Image Stitching"},

								   {"DeviceTree_EnableDevicePatrol", "Enable device patrol (Dewell Time %1 sec)"},
								   {"DeviceTree_DisableDevicePatrol", "Disable device patrol (Dewell Time %1 sec)"},
								   
								   {"DeviceTree_EnableMultiScreenDevicePatrol", "Enable Multi-screen device patrol"},
								   {"DeviceTree_DisableMultiScreenDevicePatrol", "Disable Multi-screen device patrol"},

								   {"DeviceTree_PatrolConfirm", "Please confirm again to proceed."},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            Dock = DockStyle.Fill;
            //---------------------------
            Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
            Icon.Click += DockIconClick;
            //---------------------------
        }

        public void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            InitialzeViewList();

            PanelTitleBarUI2.Text = TitleName = Localization["Control_Device"];
            titlePanel.Controls.Add(PanelTitleBarUI2);

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            DragDropProxy = new DoubleBufferPanel
            {
                Width = Width,
                Height = 31,
                BackColor = Color.FromArgb(92, 204, 250),
                Padding = new Padding(1),
            };

            DragDropLabel = new DoubleBufferLabel
            {
                AutoSize = true,//avoid text too oong and wrap lines
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                //ForeColor = Color.DarkGray, //no need set here, forecolor and backcolor will auto match to drag item
                MinimumSize = new Size(DragDropProxy.Width - 2, DragDropProxy.Height - 2) //border
            };
            DragDropProxy.Controls.Add(DragDropLabel);

            _patrolDeviceTimer.Elapsed += DevicePatrol;
            _patrolDeviceTimer.SynchronizingObject = Server.Form;

            if (NVR != null)
            {
                NVR.OnCameraStatusReceive -= EventReceive;
                NVR.OnCameraStatusReceive += EventReceive;

                NVR.OnGroupModify += GroupModify;
                NVR.OnDeviceModify += DeviceModify;
                NVR.OnDeviceLayoutModify += DeviceLayoutModify;
                NVR.OnSubLayoutModify += SubLayoutModify;
            }

            if (CMS != null)
            {
                CMS.OnCameraStatusReceive -= EventReceive;
                CMS.OnCameraStatusReceive += EventReceive;
                CMS.OnDeviceModify += DeviceModify;
                CMS.OnNVRModify += NVRModify;
                CMS.OnNVRStatusReceive += CMSOnNVRStatusReceive;

            }

            if (PTS != null)
            {
                PTS.OnCameraStatusReceive -= EventReceive;
                PTS.OnCameraStatusReceive += EventReceive;
                //PTS.OnDeviceModify += DeviceModify;
                PTS.OnNVRModify += NVRModify;
                //PTS.OnNVRStatusReceive += CMSOnNVRStatusReceive;
            }

            Server.OnSaveComplete += Server_OnSaveComplete;
        }

        public void RegisterLoadCompletedEvent()
        {
            if (CMS != null)
            {
                CMS.OnLoadComplete += OnCMSLoadCompleted;
            }
        }

        private void OnCMSLoadCompleted(object sender, EventArgs e)
        {
            _reloadTree = true;
        }

        private Boolean _reloadTree = true;

        private void CMSOnNVRStatusReceive(object sender, EventArgs<INVR> e)
        {
            _view.UpdateRecordingStatus();
        }

        private void Server_OnSaveComplete(object sender, EventArgs<string> e)
        {
            _reloadTree = true;
        }

        public void NVRModify(Object sender, EventArgs<INVR> e)
        {
            _reloadTree = true;
        }

        public void GroupModify(Object sender, EventArgs<IDeviceGroup> e)
        {
            _reloadTree = true;
        }

        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            _reloadTree = true;
        }

        public void DeviceLayoutModify(Object sender, EventArgs<IDeviceLayout> e)
        {
            _reloadTree = true;
        }

        public void SubLayoutModify(Object sender, EventArgs<ISubLayout> e)
        {
            _reloadTree = true;
        }

        public void AddPatrolIcon()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            PatrolMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceTree_EnableDevicePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString())
            };

            PatrolMenuItem.Click += PatrolMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(PatrolMenuItem);
        }

        public void AddMultiScreenPatrolIcon()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            MultiScreenPatrolMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceTree_EnableMultiScreenDevicePatrol"]
            };
            MultiScreenPatrolMenuItem.Click += MultiScreenPatrolMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(MultiScreenPatrolMenuItem);
        }

        public void SetLiveProperty()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            AddPatrolIcon();
            AddMultiScreenPatrolIcon();

            SaveViewMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceView_SaveView"]
            };

            SaveViewMenuItem.Click += SaveViewMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(SaveViewMenuItem);
        }

        public void SetSISLiveProperty()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            SaveViewMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceView_SaveView"]
            };

            SaveViewMenuItem.Click += SaveViewMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(SaveViewMenuItem);
        }

        public virtual void EventReceive(Object sender, EventArgs<List<ICamera>> e)
        {
            if (_view == null) return;

            _view.UpdateRecordingStatus();
        }

        private SortOrder _sortOrder = SortOrder.Descending;
        public SortOrder SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
                if (_view != null)
                {
                    _view.UpdateView();
                }
            }
        }

        private String _sortMode = "GROUP";
        public virtual String SortMode
        {
            get
            {
                return _sortMode;
            }
            set
            {
                _sortMode = value;
                if (_view != null)
                {
                    _view.UpdateView(_sortMode);
                }
            }
        }

        public Dictionary<String, IViewBase> ViewList = new Dictionary<String, IViewBase>();

        private IViewBase _view;
        public virtual IViewBase View
        {
            get
            {
                return _view;
            }
            set
            {
                if (ViewList.ContainsKey(value.Name))
                {
                    _view = value as ViewBase;
                    if (_view == null) return;

                    _view.UpdateView();
                }
            }
        }

        protected virtual void InitialzeViewList()
        {
            var view = CreateView();

            view.Name = "list";
            view.ViewModelPanel = viewModelPanel;

            ViewList.Add(view.Name, view);

            if (_view == null)
                _view = view;
        }

        protected virtual IViewBase CreateView()
        {
            if (CMS != null)
            {
                var view = new NVRDeviceListView { CMS = CMS };

                view.OnNVRMouseDown += ViewModelPanelMouseDown;
                view.OnNVRMouseDrag += ViewModelPanelNVRMouseDrag;
                view.OnNVRMouseDoubleClick += ViewModelPanelNVRMouseDoubleClick;

                view.OnDeviceMouseDown += ViewModelPanelMouseDown;
                view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                return view;
            }

            if (PTS != null)
            {
                var view = new NVRDeviceListView { PTS = PTS };

                view.OnNVRMouseDown += ViewModelPanelMouseDown;
                view.OnNVRMouseDrag += ViewModelPanelNVRMouseDrag;
                view.OnNVRMouseDoubleClick += ViewModelPanelNVRMouseDoubleClick;

                view.OnDeviceMouseDown += ViewModelPanelMouseDown;
                view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                return view;
            }
            else
            {
                var view = new DeviceListView
                               {
                                   App = App,
                                   NVR = NVR,
                                   Localization = Localization
                               };

                view.OnGroupMouseDown += ViewModelPanelMouseDown;
                view.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
                view.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

                view.OnDeviceMouseDown += ViewModelPanelMouseDown;
                view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                view.OnDeviceLayoutMouseDown += ViewModelPanelMouseDown;
                view.OnDeviceLayoutMouseDrag += ViewModelPanelDeviceLayoutMouseDrag;
                view.OnDeviceLayoutMouseDoubleClick += ViewModelPanelDeviceLayoutMouseDoubleClick;

                view.OnSubLayoutMouseDown += ViewModelPanelMouseDown;
                view.OnSubLayoutMouseDrag += ViewModelPanelSubLayoutMouseDrag;
                view.OnSubLayoutMouseDoubleClick += ViewModelPanelSubLayoutMouseDoubleClick;

                return view;
            }
        }

        public void ViewModelPanelNVRMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as NVRControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;
                DragDropLabel.Text = ((NVRControlUI2)sender).NVR.Name;

                //_patrolGroup = null;
                //_patrolNVR = ((NVRControl)sender).NVR;
                //GetNVRNextGroup();
                OnDragStart(this, new EventArgs<Object>(control.NVR));
            }
        }

        public void ViewModelPanelNVRMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as NVRControlUI2;
            if (control != null && OnNVRDoubleClick != null)
            {
                //_patrolGroup = null;
                //_patrolNVR = control.NVR;
                //GetNVRNextGroup();
                OnNVRDoubleClick(this, new EventArgs<INVR>((INVR)control.NVR));
            }
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is INVR || dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public void DragStop(Point point, EventArgs<Object> e)
        {
            if (DragDropProxy != null)
                DragDropProxy.Visible = false;
        }

        public void DragMove(MouseEventArgs e)
        {
            if (DragDropProxy == null) return;

            Point location = DragDropProxy.Location;
            location.X = e.X - 10;// -(DragDropProxy.Size.Width / 2);
            location.Y = e.Y - (DragDropProxy.Size.Height / 2);
            DragDropProxy.Location = location;
        }

        public void ReloadTreePanel(Object sender, EventArgs e)
        {
            _view.UpdateView();
            _view.UpdateToolTips();
        }

        private Boolean CheckHasView()
        {
            foreach (var deviceGroup in Server.Device.Groups)
            {
                if (deviceGroup.Key == 0) continue; //all device group
                if (deviceGroup.Value.Items.Count == 0) continue; //no device

                return true;

            }

            foreach (var deviceGroup in Server.User.Current.DeviceGroups)
            {
                if (deviceGroup.Value.Items.Count == 0) continue; //no device

                return true;
            }

            return false;
        }

        private Boolean _checkIfHideAtFirstAppear;
        public virtual void Activate()
        {
            if (!_checkIfHideAtFirstAppear)
            {
                _checkIfHideAtFirstAppear = true;

                // have group, show panel!
                if (!CheckHasView())
                    Maximize();
            }

            if (PatrolMenuItem != null)
                PatrolMenuItem.Text = Localization["DeviceTree_EnableDevicePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString());

            if (_reloadTree)
            {
                _view.UpdateView();
            }

            _view.UpdateToolTips();
            _view.UpdateFailoverSyncTimer(true);

            App.OnAppStarted -= AppOnAppStarted;
            App.OnAppStarted += AppOnAppStarted;

            _reloadTree = false;
        }

        public void Deactivate()
        {
            App.OnAppStarted -= AppOnAppStarted;
            _view.UpdateFailoverSyncTimer(false);
            PatrolStop();
        }

        private void AppOnAppStarted(Object sender, EventArgs e)
        {
            App.OnAppStarted -= AppOnAppStarted;

            if ((App.StartupOption.DevicePatrol) && (!App.StartupOption.GroupPatrol))
            {
                _nextPage = App.StartupOption.TourItem;

                _askPatrol = false;
                PatrolMenuItemClick(this, null);
                _askPatrol = true;
            }
        }

        public void PatrolStop(Object sender, EventArgs<Object> e)
        {
            if (e.Value == null)
                PatrolStop();
        }

        private void PatrolStop()
        {
            if (MultiScreenPatrolMenuItem != null && MultiScreenPatrolMenuItem.IsSelected)
            {
                DisableMultiScreenPatrol();
            }

            if (!_patrolDeviceTimer.Enabled) return;
            if (PatrolMenuItem != null)
            {
                PatrolMenuItem.IsSelected = false;
            }

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = SaveViewMenuItem.Enabled = true;

            _patrolDeviceTimer.Enabled = false;

            App.StartupOption.DevicePatrol = false;
            App.StartupOption.TourItem = 0;
            App.StartupOption.SaveSetting();

            _isPatrolInitial = false;
        }

        private void DevicePatrol(Object sender, EventArgs e)
        {
            DevicePatrol();
        }

        private Boolean _isPatrolInitial = false;
        private UInt16 _nextPage;
        private void DevicePatrol()
        {
            //patrol next page
            if (_isPatrolInitial && (_nextPage != 0))
            {
                if (OnPageChange != null)
                    OnPageChange(this, new EventArgs<UInt16>(_nextPage));
                return;
            }

            var list = new List<IDevice>();
            var layoutSortResult = new List<IDeviceLayout>();

            if (CMS != null)
            {
                foreach (KeyValuePair<UInt16, INVR> nvr in CMS.NVRManager.NVRs)
                {
                    if (!nvr.Value.IsPatrolInclude) continue;
                    list.AddRange(nvr.Value.Device.Devices.Values.OrderBy(d => d.Id));
                }
            }
            else if (PTS != null)
            {
                foreach (KeyValuePair<UInt16, INVR> nvr in PTS.NVR.NVRs)
                {
                    if (!nvr.Value.IsPatrolInclude) continue;
                    list.AddRange(nvr.Value.Device.Devices.Values.OrderBy(d => d.Id));
                }
            }
            else
            {
                list = NVR.Device.Devices.Values.OrderBy(d => d.Id).ToList();

                layoutSortResult = NVR.Device.DeviceLayouts.Values.OrderBy(d => d.Id).ToList();
            }

            foreach (var deviceLayout in layoutSortResult)
            {
                if (deviceLayout.Items.Count(device => device != null) == 0) continue;

                list.Add(deviceLayout);

                IEnumerable<IDevice> sublist = deviceLayout.SubLayouts.Values.OfType<IDevice>().OrderBy(d => d.Id);

                list.AddRange(sublist);
            }

            if (OnGroupDoubleClick != null)
            {
                App.StartupOption.DevicePatrol = true;
                App.StartupOption.TourItem = _nextPage;
                App.StartupOption.SaveSetting();

                _isPatrolInitial = true;
                var tmpGroup = new DeviceGroup { Items = list, View = list, Layout = _layout, Server = Server };
                OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(tmpGroup));
            }
        }

        private List<WindowLayout> _layout = WindowLayouts.LayoutGenerate(4);
        public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
        {
            if (e.Value == null) return;

            _layout = e.Value;
        }

        protected List<IDevice> UsingDevices = new List<IDevice>();
        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            UsingDevices.Clear();
            if (e.Value == null) return;
            if (e.Value is IDevice)
            {
                UsingDevices.Add(e.Value as IDevice);
            }
            else if (e.Value is IDevice[])
            {
                UsingDevices.AddRange(e.Value as IDevice[]);
            }
        }

        public void PageChange(Object sender, EventArgs<UInt16, UInt16> e)
        {
            //no need switch page
            if (e.Value1 == e.Value2)
                _nextPage = 0;
            else
                _nextPage = (UInt16)(e.Value1 + 1);

            if (!_patrolDeviceTimer.Enabled)
            {
                return;
            }

            _patrolDeviceTimer.Interval = Server.Configure.PatrolInterval * 1000;
            _patrolDeviceTimer.Enabled = true;
        }

        private void PatrolMenuItemClick(Object sender, EventArgs e)
        {
            //禁用其他patrol選單與刪除 VIew選單
            if (MultiScreenPatrolMenuItem != null)
            {
                MultiScreenPatrolMenuItem.Enabled =
                SaveViewMenuItem.Enabled = _patrolDeviceTimer.Enabled;
            }
            

            if (!_patrolDeviceTimer.Enabled)
            {
                if (CMS != null)
                {
                    //count all nvr's devices count
                    var deviceCount = CMS.NVRManager.NVRs.Sum(nvr => nvr.Value.Device.Devices.Count);
                    if (deviceCount == 0)
                        return;
                }
                else if (PTS != null)
                {
                    //count all nvr's devices count
                    var deviceCount = PTS.NVR.NVRs.Sum(nvr => nvr.Value.Device.Devices.Count);
                    if (deviceCount == 0)
                        return;
                }
                else if(NVR != null){
                    if (NVR.Device.Devices.Count == 0)
                        return;
                }

                if (_askPatrol)
                {
                    DialogResult result = TopMostMessageBox.Show(Localization["DeviceTree_PatrolConfirm"], Localization["MessageBox_Confirm"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        MultiScreenPatrolMenuItem.Enabled =
                            SaveViewMenuItem.Enabled = true;
                        _patrolDeviceTimer.Enabled = false;
                        return;
                    }
                }

                PatrolMenuItem.IsSelected = true;

                _patrolDeviceTimer.Interval = Server.Configure.PatrolInterval * 1000;
                _patrolDeviceTimer.Enabled = false;
                _patrolDeviceTimer.Enabled = true;

                if (OnPatrolStart != null)
                    OnPatrolStart(this, new EventArgs<Object>(null));

                //inst patrol
                DevicePatrol();

                Server.WriteOperationLog("Enable device patrol (Dewell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString()));
            }
            else
            {
                _patrolDeviceTimer.Enabled = false;

                PatrolMenuItem.IsSelected = false;

                Server.WriteOperationLog("Disable device patrol (Dewell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString()));
            }
        }

        private void MultiScreenPatrolMenuItemClick(Object sender, EventArgs e)
        {
            //是啟用的 -> 停用
            if (MultiScreenPatrolMenuItem.IsSelected)
            {
                DisableMultiScreenPatrol();
            }
            else //是停用的 -> 啟用
            {
                EnableMultiScreenPatrol();
            }
        }

        private void EnableMultiScreenPatrol()
        {
            if (CMS != null)
            {
                //count all nvr's devices count
                var deviceCount = CMS.NVRManager.NVRs.Sum(nvr => nvr.Value.Device.Devices.Count);
                if (deviceCount == 0)
                    return;
            }
            else
            {
                if (NVR.Device.Devices.Count == 0)
                    return;
            }

            if (_askPatrol)
            {
                DialogResult result = TopMostMessageBox.Show(Localization["DeviceTree_PatrolConfirm"], Localization["MessageBox_Confirm"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    PatrolMenuItem.Enabled =
                        SaveViewMenuItem.Enabled = true;
                    return;
                }
            }

            //先停用原本的patrol (如果有啟用的話)
            PatrolStop();

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.IsSelected = true;

            //禁用其他patrol選單與Save VIew選單
            PatrolMenuItem.Enabled =
            SaveViewMenuItem.Enabled = false;
            //讓VideoMonitor開始Patrol
            if (OnMultiScreenPatrolStart != null)
                OnMultiScreenPatrolStart(this, new EventArgs<String>(@"DeviceList"));
        }

        private void DisableMultiScreenPatrol()
        {
            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.IsSelected = false;

            //解除禁用其他patrol選單與Save VIew選單
            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = SaveViewMenuItem.Enabled = true;

            //讓VideoMonitor停止Patrol
            if (OnMultiScreenPatrolStop != null)
                OnMultiScreenPatrolStop(this, new EventArgs<String>(@"DeviceList"));
        }

        public void DisablePatrolMenu(Object sender, EventArgs<String> e)
        {
            //自己觸發的事件不要理會
            if (String.Equals(e.Value, @"DeviceList"))
                return;

            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = PatrolMenuItem.IsSelected = false;


            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = MultiScreenPatrolMenuItem.IsSelected = false;
        }

        public void ResumePatrolMenu(Object sender, EventArgs<String> e)
        {
            //自己觸發的事件不要理會
            if (String.Equals(e.Value, @"DeviceList"))
                return;

            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = true;

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = true;
        }

        private System.Timers.Timer _timer;
        private UInt16 _refreshCounter;
        public virtual void ViewModelPanelMouseDown(Object sender, MouseEventArgs e)
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(500);
                _timer.Elapsed += RefreshPanel;
                _timer.SynchronizingObject = Server.Form;
            }
            viewModelPanel.Focus();

            _refreshCounter = 0;
            _timer.Enabled = true;
        }

        //force refresh to hide scroll bar ghost
        private void RefreshPanel(Object sender, EventArgs e)
        {
            _refreshCounter++;
            _view.Refresh();
            viewModelPanel.Invalidate();
            if (_refreshCounter > 5)
                _timer.Enabled = false;
        }

        public void ViewModelPanelGroupMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                DragDropLabel.Text = control.DeviceGroup.ToString();

                OnDragStart(this, new EventArgs<Object>(control.DeviceGroup));
            }
        }

        public virtual void ViewModelPanelGroupMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControlUI2;
            if (control != null && OnGroupDoubleClick != null)
            {
                OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(control.DeviceGroup));
            }
        }

        public void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                control.Device.DeviceType = DeviceType.Device;
                DragDropLabel.Text = control.Device.ToString();

                OnDragStart(this, new EventArgs<Object>(control.Device));
            }
        }

        public void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceControlUI2;
            if (control != null && OnDeviceDoubleClick != null)
            {
                OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.Device));
            }
        }

        public void ViewModelPanelDeviceLayoutMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceLayoutControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;
                DragDropLabel.Text = control.DeviceLayout.ToString();

                OnDragStart(this, new EventArgs<Object>(control.DeviceLayout));
            }
        }

        public void ViewModelPanelDeviceLayoutMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceLayoutControlUI2;

            var layout = control.DeviceLayout;

            if (layout.isImmerVision)
            {
                if (control != null && OnImmerVisionDeviceLayoutDoubleClick != null)
                {
                    OnImmerVisionDeviceLayoutDoubleClick(this, new EventArgs<IDeviceLayout>(control.DeviceLayout));
                }
            }
            else
            {
                if (control != null && OnDeviceDoubleClick != null)
                {
                    OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.DeviceLayout));
                }
            }
        }

        public void ViewModelPanelSubLayoutMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as SubLayoutControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                DragDropLabel.Text = control.SubLayout.ToString();

                OnDragStart(this, new EventArgs<Object>(control.SubLayout));
            }
        }

        public void ViewModelPanelSubLayoutMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as SubLayoutControlUI2;
            if (control != null && OnDeviceDoubleClick != null)
            {
                OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.SubLayout));
            }
        }

        public void GlobalMouseHandler()
        {
            if (Drag.IsDrop(viewModelPanel))
            {
                if (!viewModelPanel.AutoScroll)
                {
                    viewModelPanel.AutoScroll = true;
                }

                return;
            }
            if (viewModelPanel.AutoScroll)
                HideScrollBar();
        }

        private Point _previousScrollPosition;
        private void HideScrollBar()
        {
            _previousScrollPosition = viewModelPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            viewModelPanel.AutoScroll = false;

            //force refresh to hide scroll bar
            viewModelPanel.Height++;
            viewModelPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public void SaveViewWithDeviceRegion(Object sender, EventArgs<List<WindowMountLayout>> e)
        {
            IDeviceGroup deviceGroup = new DeviceGroup();

            if (NVR == null && CMS == null) return;

            //default is private view
            deviceGroup.Id = CMS != null ? CMS.User.Current.GetNewGroupId() : NVR.User.Current.GetNewGroupId();
            deviceGroup.Name = Localization["SetupDeviceGroup_NewView"] + @" " + deviceGroup.Id;

            var inputForm = new GroupNameInputForm
            {
                DeviceGroup = deviceGroup,
                Server = NVR,
                Icon = Server.Form.Icon,
                CMS = CMS
            };

            var result = inputForm.ShowDialog();

            if (result != DialogResult.OK) return;
            if (String.IsNullOrEmpty(deviceGroup.Name) || deviceGroup.Id == 0) return;

            deviceGroup.View.AddRange(UsingDevices.ToArray());
            foreach (var device in deviceGroup.View)
            {
                if (device == null) continue;
                if (deviceGroup.Items.Contains(device)) continue;

                deviceGroup.Items.Add(device);
            }
            deviceGroup.Items.Sort((x, y) => (x.Id - y.Id));

            deviceGroup.Layout.Clear();
            deviceGroup.Layout.AddRange(_layout);

            deviceGroup.Regions.Clear();
            deviceGroup.MountType.Clear();
            deviceGroup.DewarpEnable.Clear();
            var regions = e.Value;
            if (regions != null)
            {
                foreach (var region in regions)
                {
                    if (region == null)
                    {
                        deviceGroup.Regions.Add(null);
                        deviceGroup.MountType.Add(-1);
                        deviceGroup.DewarpEnable.Add(false);
                        continue;
                    }
                    deviceGroup.Regions.Add(region.RegionXML);
                    deviceGroup.MountType.Add(region.MountType);
                    deviceGroup.DewarpEnable.Add(region.DewarpEnable);
                }
            }

            if (CMS != null)
            {
                if (inputForm.IsPublish)
                {
                    CMS.Device.Groups.Add(deviceGroup.Id, deviceGroup);
                }
                else
                {
                    CMS.User.Current.DeviceGroups.Add(deviceGroup.Id, deviceGroup);
                }
            }
            else
            {
                if (inputForm.IsPublish)
                {
                    NVR.Device.Groups.Add(deviceGroup.Id, deviceGroup);
                }
                else
                {
                    NVR.User.Current.DeviceGroups.Add(deviceGroup.Id, deviceGroup);
                }
            }

            App.SaveUserDefineDeviceGroup(deviceGroup);
        }

        private void SaveViewMenuItemClick(Object sender, EventArgs e)
        {
            if (OnSaveView != null)
                OnSaveView(this, EventArgs.Empty);
            else
            {
                SaveViewWithDeviceRegion(this, new EventArgs<List<WindowMountLayout>>(null));
            }
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
        }

        public void Minimize()
        {
            //dont check auto appear, if call minize at initialize
            _checkIfHideAtFirstAppear = true;

            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
            {
                BlockPanel.HideThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = true;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _icon;
            Icon.BackgroundImage = null;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0")
            {
                BlockPanel.ShowThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = false;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            //DeviceTree.Objects.GroupControlUI2
            foreach (Control ctrl1 in viewModelPanel.Controls)
            {
                Boolean bShowChild = false;
                if ((ctrl1 is GroupControlUI2) || (ctrl1 is NVRControlUI2))
                {
                    if (ctrl1 is NVRControlUI2)
                    {
                        var nvr = (ctrl1 as NVRControlUI2).NVR;
                        if (nvr.Name.ToLower().IndexOf(keywordTextBox.Text.ToLower()) >= 0)
                        {
                            ctrl1.Visible = bShowChild = true;
                        }
                        else
                        {
                            ctrl1.Visible = bShowChild = false;
                        }
                            
                    }
                    else
                        ctrl1.Visible = bShowChild = keywordTextBox.Text == "" ? true : false;

                    //PanelBase.DoubleBufferPanel
                    foreach (Control ctrl2 in ctrl1.Controls)
                    {
                        //DeviceTree.Objects.DeviceControlUI2
                        foreach (Control ctrl3 in ctrl2.Controls)
                        {
                            if (!(ctrl3 is DeviceControlUI2)) continue;

                            var device = (ctrl3 as DeviceControlUI2).Device;

                            if (device == null)
                                ctrl3.Visible = false;
                            else
                            {
                                if (bShowChild)
                                {
                                    ctrl1.Visible = ctrl3.Visible = true;
                                }
                                else
                                {
                                    if ((device.Name.Trim().ToLower().IndexOf(keywordTextBox.Text.ToLower()) >= 0))
                                        ctrl1.Visible = ctrl3.Visible = true;
                                        //ctrl1.Visible = ctrl2.Visible = ctrl3.Visible = true;
                                    else
                                        ctrl3.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            keywordTextBox.Text = "";
            SearchButtonClick(this, EventArgs.Empty);
        }

        private void KeywordTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode != Keys.Enter) return;

            if (!String.IsNullOrEmpty(keywordTextBox.Text))
                SearchButtonClick(this, EventArgs.Empty);
        }
    }
}
