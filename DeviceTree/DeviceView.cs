using Constant;
using DeviceConstant;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;

namespace DeviceTree
{
    public partial class DeviceView : UserControl, IControl, IServerUse, IAppUse, IDrag, IDrop, IMinimize, IMouseHandler, IBlockPanelUse
    {
        public event EventHandler OnMinimizeChange;

        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;
        public event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;

        public event EventHandler<EventArgs<Object>> OnPatrolStart;
        public event EventHandler<EventArgs<UInt16>> OnPageChange;
        public event EventHandler<EventArgs<Object>> OnDragStart;

        public event EventHandler<EventArgs<String>> OnMultiScreenPatrolStart;
        public event EventHandler<EventArgs<String>> OnMultiScreenPatrolStop;


        public event EventHandler OnDeviceViewDelete;

        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();

        protected ToolStripMenuItemUI2 PatrolMenuItem;
        protected ToolStripMenuItemUI2 MultiScreenPatrolMenuItem;
        protected ToolStripMenuItemUI2 DeleteViewMenuItem;

        public Label DragDropLabel { get; private set; }
        public Panel DragDropProxy { get; private set; }

        public Dictionary<String, String> Localization;

        public String TitleName { get; set; }

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.deviceViewIcon, Properties.Resources.IMGDeviceViewIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.deviceViewIcon_activate, Properties.Resources.IMGDeviceViewIconActivate);

        private readonly System.Timers.Timer _hideLoadingTimer = new System.Timers.Timer();

        public IApp App { get; set; }
        protected ICMS CMS;
        protected INVR NVR;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is ICMS)
                    CMS = value as ICMS;
                else if (value is INVR)
                    NVR = value as INVR;
            }
        }

        public IBlockPanel BlockPanel { get; set; }

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }
        public Boolean IsMinimize { get; private set; }
        private readonly System.Timers.Timer _patrolGroupTimer = new System.Timers.Timer();

        private Boolean _askPatrol = true;


        // Constructor
        public DeviceView()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_DeviceView", "View"},

								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"GroupPanel_NumDevice", "(%1 Device)"},
								   {"GroupPanel_NumDevices", "(%1 Devices)"},

								   {"FolderControl_Shared", "Shared"},
								   {"FolderControl_Private", "Private"},
								   
								   {"DeviceView_DeleteView", "Delete View"},

								   {"DeviceTree_EnablePatrol", "Enable view patrol (Dewell Time %1 sec)"},
								   {"DeviceTree_DisablePatrol", "Disable view patrol (Dewell Time %1 sec)"},
								   
								   {"DeviceTree_EnableMultiScreenPatrol", "Enable Multi-screen view patrol"},
								   {"DeviceTree_DisableMultiScreenPatrol", "Disable Multi-screen view patrol"},
								   
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

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            InitialzeViewList();

            PanelTitleBarUI2.Text = TitleName = Localization["Control_DeviceView"];
            titlePanel.Controls.Add(PanelTitleBarUI2);

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            _hideLoadingTimer.Elapsed += HideLoading;
            _hideLoadingTimer.Interval = 500;
            _hideLoadingTimer.SynchronizingObject = Server.Form;

            DragDropProxy = new DoubleBufferPanel
            {
                Width = Width,
                Height = 31,
                BackColor = Color.FromArgb(92, 204, 250),
                Padding = new Padding(1),
            };

            DragDropLabel = new DoubleBufferLabel
            {
                AutoSize = true,//avoid text too long and wrap lines
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                //ForeColor = Color.DarkGray, //no need set here, forecolor and backcolor will auto match to drag item
                MinimumSize = new Size(DragDropProxy.Width - 2, DragDropProxy.Height - 2) //border
            };
            DragDropProxy.Controls.Add(DragDropLabel);

            _patrolGroupTimer.Elapsed += GroupPatrol;
            _patrolGroupTimer.SynchronizingObject = Server.Form;

            if (NVR != null)
            {
                NVR.OnCameraStatusReceive -= EventReceive;
                NVR.OnCameraStatusReceive += EventReceive;

                NVR.OnGroupModify += GroupModify;
                NVR.OnDeviceModify += DeviceModify;
                NVR.OnDeviceLayoutModify += DeviceLayoutModify;
                NVR.OnSubLayoutModify += SubLayoutModify;

                App.OnUserDefineDeviceGroupModify += AppOnUserDefineDeviceGroupModify;
            }

            if (CMS != null)
            {
                CMS.OnCameraStatusReceive -= EventReceive;
                CMS.OnCameraStatusReceive += EventReceive;
                CMS.OnDeviceModify += DeviceModify;
                CMS.OnGroupModify += GroupModify;
                CMS.OnDeviceLayoutModify += DeviceLayoutModify;
                CMS.OnNVRModify += NVRModify;

                CMS.OnNVRStatusReceive += CMSOnNVRStatusReceive;
                App.OnUserDefineDeviceGroupModify += AppOnUserDefineDeviceGroupModify;
            }
        }

        private void AppOnUserDefineDeviceGroupModify(object sender, EventArgs e)
        {
            ReloadTreePanel(this, EventArgs.Empty);
        }

        private Boolean _reloadTree = true;

        public void GroupModify(Object sender, EventArgs<IDeviceGroup> e)
        {
            ReloadTreePanel(this, EventArgs.Empty);
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

        public void NVRModify(Object sender, EventArgs<INVR> e)
        {
            _reloadTree = true;
        }

        private void CMSOnNVRStatusReceive(object sender, EventArgs<INVR> e)
        {
            _view.UpdateRecordingStatus();
        }

        public void AddPatrolIcon()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            PatrolMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceTree_EnablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString(CultureInfo.InvariantCulture))
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
                Text = Localization["DeviceTree_EnableMultiScreenPatrol"]
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

            DeleteViewMenuItem = new ToolStripMenuItemUI2
                                     {
                                         Text = Localization["DeviceView_DeleteView"]
                                     };
            DeleteViewMenuItem.Click += DeleteViewMenuItemItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(DeleteViewMenuItem);
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

        public void SetSISLiveProperty()
        {
            if (PanelTitleBarUI2.MenuStrip == null)
                PanelTitleBarUI2.InitializeToolStripMenuItem();

            DeleteViewMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["DeviceView_DeleteView"]
            };
            DeleteViewMenuItem.Click += DeleteViewMenuItemItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(DeleteViewMenuItem);
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

        private void InitialzeViewList()
        {
            var view = CreateViewBase();

            view.OnGroupMouseDown += ViewModelPanelMouseDown;
            view.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
            view.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

            view.OnDeviceMouseDown += ViewModelPanelMouseDown;
            view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
            view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

            view.Name = "list";
            view.ViewModelPanel = viewModelPanel;

            ViewList.Add(view.Name, view);

            if (_view == null)
                _view = view;
        }

        protected virtual DeviceViewView CreateViewBase()
        {
            var view = new DeviceViewView();
            if (CMS != null)
            {
                view.CMS = CMS;
            }
            else
            {
                view.NVR = NVR;
            }
            view.Localization = Localization;

            return view;
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
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs>(ReloadTreePanel), sender, e);

                return;
            }

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

                // no group, hide panel!
                if (CheckHasView())
                    Maximize();
            }

            if (PatrolMenuItem != null)
                PatrolMenuItem.Text = Localization["DeviceTree_EnablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString(CultureInfo.InvariantCulture));

            if (_reloadTree)
            {
                _view.UpdateView();
            }

            _view.UpdateToolTips();

            App.OnAppStarted -= AppOnAppStarted;
            App.OnAppStarted += AppOnAppStarted;

            _reloadTree = false;
        }

        public void Deactivate()
        {
            App.OnAppStarted -= AppOnAppStarted;

            PatrolStop();
        }

        private void AppOnAppStarted(Object sender, EventArgs e)
        {
            App.OnAppStarted -= AppOnAppStarted;

            if (!App.StartupOption.GroupPatrol) return;

            _askPatrol = false;
            PatrolMenuItemClick(this, null);
            _askPatrol = true;
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

            //clear _patrolGroup setting. when delete view, a not exist view there still be there
            _patrolGroup = null;

            if (!_patrolGroupTimer.Enabled) return;
            if (PatrolMenuItem != null)
            {
                PatrolMenuItem.IsSelected = false;
            }

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = DeleteViewMenuItem.Enabled = true;

            _patrolGroupTimer.Enabled = false;

            App.StartupOption.GroupPatrol = false;
            App.StartupOption.DeviceGroup = "";
            App.StartupOption.SaveSetting();
        }

        private void GroupPatrol(Object sender, EventArgs e)
        {
            GroupPatrol();
        }

        private IDeviceGroup _patrolGroup;
        private UInt16 _nextPage;
        private void GroupPatrol()
        {
            //patrol next page if have _patrolGroup
            if (_patrolGroup != null && _nextPage != 0)
            {
                if (OnPageChange != null)
                    OnPageChange(this, new EventArgs<UInt16>(_nextPage));
                return;
            }

            //check if this view still exist
            if (_patrolGroup != null)
            {
                if (!Server.Device.Groups.Values.Contains(_patrolGroup) && !Server.User.Current.DeviceGroups.Values.Contains(_patrolGroup))
                    _patrolGroup = null;
            }

            var group = _patrolGroup;
            GetNextGroup();
            //patrol page 1
            if (group == _patrolGroup)
            {
                _nextPage = 1;
                if (OnPageChange != null)
                    OnPageChange(this, new EventArgs<UInt16>(_nextPage));
                return;
            }

            //patrol next group
            if (OnGroupDoubleClick != null && _patrolGroup != null)
            {
                App.StartupOption.GroupPatrol = true;
                App.StartupOption.DeviceGroup = _patrolGroup.Id.ToString(CultureInfo.InvariantCulture);
                App.StartupOption.SaveSetting();

                OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(_patrolGroup));
            }
        }

        private void GetNextGroup()
        {
            var groups = Server.Device.Groups.Values.OrderBy(g => g.Id);

            foreach (var group in groups)
            {
                //dont show all device group
                if (group.Id == 0) continue;

                if (_patrolGroup == null && group.Items.Count > 0)
                {
                    _patrolGroup = group;
                    return;
                }

                if (group == _patrolGroup)
                    _patrolGroup = null;
            }
            //---------------------------------
            groups = Server.User.Current.DeviceGroups.Values.OrderBy(g => g.Id);

            foreach (var group in groups)
            {
                if (_patrolGroup == null && group.Items.Count > 0)
                {
                    _patrolGroup = group;
                    return;
                }

                if (group == _patrolGroup)
                    _patrolGroup = null;
            }

            if (_patrolGroup != null) return;
            //---------------------------------
            groups = Server.Device.Groups.Values.OrderBy(g => g.Id);

            foreach (var group in groups)
            {
                //dont show all device group
                if (group.Id == 0) continue;

                if (_patrolGroup == null && group.Items.Count > 0)
                {
                    _patrolGroup = group;
                    return;
                }
            }
            //---------------------------------
            groups = Server.User.Current.DeviceGroups.Values.OrderBy(g => g.Id);

            foreach (var group in groups)
            {
                if (_patrolGroup == null && group.Items.Count > 0)
                {
                    _patrolGroup = group;
                    return;
                }

                if (group == _patrolGroup)
                    _patrolGroup = null;
            }
        }

        public void PageChange(Object sender, EventArgs<UInt16, UInt16> e)
        {
            //no need switch page
            if (e.Value1 == e.Value2)
                _nextPage = 0;
            else
                _nextPage = (UInt16)(e.Value1 + 1);

            if (!_patrolGroupTimer.Enabled)
            {
                return;
            }

            _patrolGroupTimer.Interval = Server.Configure.PatrolInterval * 1000;
            _patrolGroupTimer.Enabled = true;
        }

        private void PatrolMenuItemClick(Object sender, EventArgs e)
        {
            //禁用其他patrol選單與刪除 VIew選單
            if (MultiScreenPatrolMenuItem != null)
            {
                MultiScreenPatrolMenuItem.Enabled =
                DeleteViewMenuItem.Enabled = _patrolGroupTimer.Enabled;
            }

            if (!_patrolGroupTimer.Enabled)
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

                //no viewer dont patrol
                if (!CheckHasView())
                    return;

                if (_askPatrol)
                {
                    DialogResult result = TopMostMessageBox.Show(Localization["DeviceTree_PatrolConfirm"], Localization["MessageBox_Confirm"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        MultiScreenPatrolMenuItem.Enabled =
                            DeleteViewMenuItem.Enabled = true;
                        _patrolGroupTimer.Enabled = false;
                        return;
                    }
                }

                PatrolMenuItem.IsSelected = true;

                _patrolGroupTimer.Interval = Server.Configure.PatrolInterval * 1000;
                _patrolGroupTimer.Enabled = false;
                _patrolGroupTimer.Enabled = true;

                if (OnPatrolStart != null)
                    OnPatrolStart(this, new EventArgs<Object>(null));

                //inst patrol
                GroupPatrol();

                Server.WriteOperationLog("Enable view patrol (Dwell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                _patrolGroupTimer.Enabled = false;

                PatrolMenuItem.IsSelected = false;

                App.StartupOption.GroupPatrol = false;
                App.StartupOption.DeviceGroup = "";
                App.StartupOption.SaveSetting();

                Server.WriteOperationLog("Disable view patrol (Dwell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString(CultureInfo.InvariantCulture)));
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

            //no viewer dont patrol
            if (!CheckHasView())
                return;

            if (_askPatrol)
            {
                DialogResult result = TopMostMessageBox.Show(Localization["DeviceTree_PatrolConfirm"], Localization["MessageBox_Confirm"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    PatrolMenuItem.Enabled =
                    DeleteViewMenuItem.Enabled = true;
                    return;
                }
                   
            }

            //先停用原本的patrol (如果有啟用的話)
            PatrolStop();

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.IsSelected = true;

            //禁用其他patrol選單與刪除 VIew選單
            if (DeleteViewMenuItem != null)
            {
                PatrolMenuItem.Enabled =
                DeleteViewMenuItem.Enabled = false;
            }
            
            //讓VideoMonitor開始Patrol
            if (OnMultiScreenPatrolStart != null)
                OnMultiScreenPatrolStart(this, new EventArgs<String>(@"DeviceView"));
        }

        private void DisableMultiScreenPatrol()
        {
            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.IsSelected = false;

            //解除禁用其他patrol選單與刪除 VIew選單
            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = DeleteViewMenuItem.Enabled = true;

            //讓VideoMonitor停止Patrol
            if (OnMultiScreenPatrolStop != null)
                OnMultiScreenPatrolStop(this, new EventArgs<String>(@"DeviceView"));
        }

        public void DisablePatrolMenu(Object sender, EventArgs<String> e)
        {
            //自己觸發的事件不要理會
            if (String.Equals(e.Value, @"DeviceView"))
                return;

            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = PatrolMenuItem.IsSelected = false;
                

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = MultiScreenPatrolMenuItem.IsSelected = false;
        }


        public void ResumePatrolMenu(Object sender, EventArgs<String> e)
        {
            //自己觸發的事件不要理會
            if (String.Equals(e.Value, @"DeviceView"))
                return;

            if (PatrolMenuItem != null)
                PatrolMenuItem.Enabled = true;

            if (MultiScreenPatrolMenuItem != null)
                MultiScreenPatrolMenuItem.Enabled = true;
        }

        public void ViewModelPanelGroupMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControlUI2;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                var num = ((control.DeviceGroup.Items.Count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                    Replace("%1", control.DeviceGroup.Items.Count.ToString(CultureInfo.InvariantCulture));

                DragDropLabel.Text = control.DeviceGroup + @" " + num;

                _patrolGroup = control.DeviceGroup;
                OnDragStart(this, new EventArgs<Object>(_patrolGroup));
            }
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

        private void RefreshPanel(Object sender, EventArgs e)
        {
            _refreshCounter++;
            _view.Refresh();
            viewModelPanel.Invalidate();
            if (_refreshCounter > 5)
                _timer.Enabled = false;
        }

        public virtual void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
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

        public virtual void ViewModelPanelGroupMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControlUI2;
            if (control != null && OnGroupDoubleClick != null)
            {
                _patrolGroup = control.DeviceGroup;

                OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(_patrolGroup));
            }
        }

        public virtual void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceControlUI2;
            if (control != null && OnDeviceDoubleClick != null)
            {
                OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.Device));
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

        private void DeleteViewMenuItemItemClick(Object sender, EventArgs e)
        {
            if (OnDeviceViewDelete != null)
                OnDeviceViewDelete(this, null);
        }

        private void HideLoading(Object sender, EventArgs e)
        {
            _hideLoadingTimer.Enabled = false;

            ApplicationForms.HideLoadingIcon();
            Focus();
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
            _checkIfHideAtFirstAppear = false;

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
    }
}
