using Constant;
using Device;
using DeviceConstant;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using PanelBase;
using ServerProfile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ListView = DeviceTree.View.ListView;

namespace DeviceTree
{
    public partial class DeviceTree : UserControl, IControl, IServerUse, IAppUse, IDrag, IDrop, IMinimize, IMouseHandler
    {
        public event EventHandler OnMinimizeChange;

        protected void RaiseOnMinimizeChange()
        {
            if (OnMinimizeChange != null)
            {
                OnMinimizeChange(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs<INVR>> OnNVRDoubleClick;
        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;

        protected void RaiseOnGroupDoubleClick(EventArgs<IDeviceGroup> e)
        {
            if (OnGroupDoubleClick != null)
            {
                OnGroupDoubleClick(this, e);
            }
        }

        public event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;

        protected void RaiseOnDeviceDoubleClick(EventArgs<IDevice> e)
        {
            if (OnDeviceDoubleClick != null)
            {
                OnDeviceDoubleClick(this, e);
            }
        }

        public event EventHandler<EventArgs<UInt16>> OnPageChange;
        public event EventHandler<EventArgs<Object>> OnDragStart;

        protected void RaiseOnDragStart(EventArgs<Object> e)
        {
            if (OnDragStart != null)
            {
                OnDragStart(this, e);
            }
        }

        protected readonly PanelTitleBar PanelTitleBar = new PanelTitleBar();
        protected PictureBox PatrolButton;

        public Label DragDropLabel { get; private set; }
        public Panel DragDropProxy { get; private set; }

        public Dictionary<String, String> Localization;

        public String TitleName { get; set; }

        public Button Icon { get; private set; }

        public IApp App { get; set; }

        protected INVR NVR { get; set; }
        protected ICMS CMS { get; set; }
        protected IPTS PTS { get; set; }
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    NVR = value as INVR;
                if (value is ICMS)
                    CMS = value as ICMS;
                if (value is IPTS)
                    PTS = value as IPTS;
            }
        }

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }

        private bool _isMinimize;
        public Boolean IsMinimize
        {
            get { return _isMinimize; }
            private set
            {
                if (_isMinimize != value)
                {
                    _isMinimize = value;
                    RaiseOnMinimizeChange();
                }
            }
        }
        private readonly System.Timers.Timer _patrolGroupTimer = new System.Timers.Timer();

        private static readonly Image _patrol = Resources.GetResources(Properties.Resources.patrol, Properties.Resources.IMGPatrol);
        private static readonly Image _patrolactivate = Resources.GetResources(Properties.Resources.patrol_activate, Properties.Resources.IMGPatrolActivate);

        private Boolean _askPatrol = true;

        public DeviceTree()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Device", "Device"},

								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"GroupPanel_NumDevice", "(%1 Device)"},
								   {"GroupPanel_NumDevices", "(%1 Devices)"},
								   {"DeviceTree_EnablePatrol", "Enable group patrol (Dewell Time %1 sec)"},
								   {"DeviceTree_DisablePatrol", "Disable group patrol (Dewell Time %1 sec)"},
								   {"DeviceTree_PatrolConfirm", "Are you sure you want to patrol device?"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            Dock = DockStyle.Fill;
            //---------------------------
            Image iconActivate = Resources.GetResources(Properties.Resources.deviceListIcon_activate, Properties.Resources.IMGDeviceListIconActivate);
            Icon = new ControlIconButton { Image = iconActivate };
            Icon.Click += DockIconClick;
            //---------------------------
        }

        public virtual void Initialize()
        {
            InitialzeViewList();

            PanelTitleBar.Text = TitleName = Localization["Control_Device"];
            PanelTitleBar.Panel = this;
            titlePanel.Controls.Add(PanelTitleBar);

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            DragDropProxy = new DoubleBufferPanel
            {
                Width = Width,
                Height = 25,
                BackColor = Color.Orange,
                Padding = new Padding(1),
            };

            DragDropLabel = new DoubleBufferLabel
            {
                AutoSize = true,//avoid text too oong and wrap lines
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                MinimumSize = new Size(DragDropProxy.Width - 2, DragDropProxy.Height - 2) //border
            };
            DragDropProxy.Controls.Add(DragDropLabel);

            _patrolGroupTimer.Elapsed += GroupPatrol;
            _patrolGroupTimer.SynchronizingObject = Server.Form;

            if (NVR != null)
            {
                NVR.OnCameraStatusReceive -= EventReceive;
                NVR.OnCameraStatusReceive += EventReceive;

                NVR.OnDeviceModify += DeviceModify;
                NVR.OnGroupModify += GroupModify;

                App.OnUserDefineDeviceGroupModify += AppOnUserDefineDeviceGroupModify;
            }

            if (CMS != null)
            {
                CMS.OnNVRModify += NVRModify;

                CMS.OnNVRStatusReceive += CMSOnNVRStatusReceive;
            }

            if (PTS != null)
            {
                PTS.OnCameraStatusReceive -= EventReceive;
                PTS.OnCameraStatusReceive += EventReceive;

                PTS.OnNVRModify += NVRModify;
            }
        }

        private void CMSOnNVRStatusReceive(object sender, EventArgs<INVR> e)
        {
            _view.UpdateView();
        }

        private void AppOnAppStarted(Object sender, EventArgs e)
        {
            if (App.StartupOption.GroupPatrol)
            {
                var id = Convert.ToUInt16(NVR.Configure.StartupOptions.DeviceGroup);
                IDeviceGroup group = NVR.Device.Groups[id];
                _patrolGroup = group;

                _askPatrol = false;
                PatrolButtonMouseClick(this, null);
                _askPatrol = true;

                return;
            }

            if (App.StartupOption.Items != "")
            {
                var innerText = App.StartupOption.Items;
                var items = innerText.Split(',');
                List<String> viewId = items.ToList();

                var list = new List<IDevice>();
                var view = new List<IDevice>();

                if (viewId.Count == 1 && viewId[0] == "0")
                    viewId.Clear();

                if (viewId.Count > 0)
                {
                    foreach (string s in viewId)
                    {
                        if (s.IndexOf("-0", StringComparison.Ordinal) >= 0)
                        {
                            var layout = s.Replace("-0", "");
                            var id = Convert.ToUInt16(layout);
                            if (Server.Device.DeviceLayouts.ContainsKey(id))
                            {
                                view.Add(Server.Device.DeviceLayouts[id]);
                                list.Add(Server.Device.DeviceLayouts[id]);
                            }

                        }
                        else if (s.IndexOf("-", StringComparison.Ordinal) >= 0)
                        {
                            var temp = s.Split('-');
                            var id = Convert.ToUInt16(temp[0]);
                            var sub = Convert.ToUInt16(temp[1]);

                            if (Server.Device.DeviceLayouts.ContainsKey(id))
                            {
                                var layout = Server.Device.DeviceLayouts[id];
                                if (layout.SubLayouts.ContainsKey(sub))
                                {
                                    view.Add(layout.SubLayouts[sub]);
                                    list.Add(layout.SubLayouts[sub]);
                                }
                            }
                        }
                        else
                        {
                            if (s == "")
                                view.Add(null);
                            else
                            {
                                var id = Convert.ToUInt16(s);
                                if (Server.Device.Devices.ContainsKey(id))
                                {
                                    view.Add(Server.Device.Devices[id]);
                                    list.Add(Server.Device.Devices[id]);
                                }
                                else
                                    view.Add(null);
                            }
                        }
                    }
                }

                if (OnGroupDoubleClick != null)
                {
                    var windowLayout = DeviceConverter.StringToLayout(App.StartupOption.Layout);

                    var tmpGroup = new DeviceGroup() { Items = list, View = view, Layout = windowLayout, Server = Server };

                    RaiseOnGroupDoubleClick(new EventArgs<IDeviceGroup>(tmpGroup));
                }
                return;
            }

            if (NVR.Configure.StartupOptions.GroupPatrol)
            {
                _askPatrol = false;
                PatrolButtonMouseClick(this, null);
                _askPatrol = true;
                return;
            }

            if (NVR.Configure.StartupOptions.DeviceGroup != "")
            {
                if (OnGroupDoubleClick != null)
                {
                    var id = Convert.ToUInt16(NVR.Configure.StartupOptions.DeviceGroup);
                    IDeviceGroup group = NVR.Device.Groups[id];

                    RaiseOnGroupDoubleClick(new EventArgs<IDeviceGroup>(group));
                }
            }
        }

        private void AppOnUserDefineDeviceGroupModify(object sender, EventArgs e)
        {
            ReloadTreePanel(this, EventArgs.Empty);
        }

        protected Boolean _reloadTree = true;

        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            _reloadTree = true;
        }

        public void GroupModify(Object sender, EventArgs<IDeviceGroup> e)
        {
            _reloadTree = true;
        }

        public void NVRModify(Object sender, EventArgs<INVR> e)
        {
            _reloadTree = true;
        }

        public void AddPatrolIcon()
        {
            PatrolButton = new PictureBox
            {
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Size = new Size(25, 25),
                BackColor = Color.Transparent,
                BackgroundImage = _patrol,
                BackgroundImageLayout = ImageLayout.Center
            };

            SharedToolTips.SharedToolTip.SetToolTip(PatrolButton, Localization["DeviceTree_EnablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString()));
            PatrolButton.MouseClick += PatrolButtonMouseClick;
            PanelTitleBar.Controls.Add(PatrolButton);
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

        public void InitialzeViewList()
        {
            IViewBase view = CreateListView();
            view.Name = "list";
            view.ViewModelPanel = viewModelPanel;

            ViewList.Add(view.Name, view);

            if (_view == null)
                _view = view;
        }

        protected virtual IViewBase CreateListView()
        {
            IViewBase view;
            if (CMS != null)
            {
                var nvrListView = new NVRListView
                {
                    CMS = CMS,
                };

                nvrListView.OnNVRMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnNVRMouseDrag += ViewModelPanelNVRMouseDrag;
                nvrListView.OnNVRMouseDoubleClick += ViewModelPanelNVRMouseDoubleClick;

                nvrListView.OnGroupMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
                nvrListView.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

                nvrListView.OnDeviceMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                nvrListView.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                view = nvrListView;
            }
            else if (PTS != null)
            {
                var nvrListView = new NVRListView
                {
                    PTS = PTS
                };

                nvrListView.OnNVRMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnNVRMouseDrag += ViewModelPanelNVRMouseDrag;
                nvrListView.OnNVRMouseDoubleClick += ViewModelPanelNVRMouseDoubleClick;

                nvrListView.OnGroupMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
                nvrListView.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

                nvrListView.OnDeviceMouseDown += ViewModelPanelMouseDown;
                nvrListView.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                nvrListView.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                view = nvrListView;
            }
            else
            {
                var listView = new ListView
                {
                    NVR = NVR,
                };

                listView.OnGroupMouseDown += ViewModelPanelMouseDown;
                listView.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
                listView.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

                listView.OnDeviceMouseDown += ViewModelPanelMouseDown;
                listView.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
                listView.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

                view = listView;
            }

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
            _view.UpdateView();
            //_view.UpdateToolTips();
        }

        public virtual void Activate()
        {
            if (PatrolButton != null)
            {
                SharedToolTips.SharedToolTip.SetToolTip(PatrolButton, Localization["DeviceTree_EnablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString()));
            }
            if (_reloadTree)
            {
                _view.UpdateView();
            }

            App.OnAppStarted -= AppOnAppStarted;
            App.OnAppStarted += AppOnAppStarted;

            _reloadTree = false;
        }

        public void Deactivate()
        {
            App.OnAppStarted -= AppOnAppStarted;

            PatrolStop();
        }

        public void PatrolStop(Object sender, EventArgs<Object> e)
        {
            if (e.Value == null)
                PatrolStop();
        }

        private void PatrolStop()
        {
            if (!_patrolGroupTimer.Enabled) return;
            if (PatrolButton != null)
                PatrolButton.BackgroundImage = _patrol;
            _patrolGroupTimer.Enabled = false;
            _patrolGroup = null;
            _patrolNVR = null;

            App.StartupOption.GroupPatrol = false;
            App.StartupOption.DeviceGroup = "";
            App.StartupOption.SaveSetting();
        }

        private void GroupPatrol(Object sender, EventArgs e)
        {
            GroupPatrol();
        }

        private IServer _patrolNVR;
        private IDeviceGroup _patrolGroup;
        private UInt16 _nextPage;
        private void GroupPatrol()
        {
            //patrol next page
            if (_nextPage != 0)
            {
                if (OnPageChange != null)
                    OnPageChange(this, new EventArgs<UInt16>(_nextPage));
                return;
            }

            if (CMS != null || PTS != null)
            {
                GetCMSNextGroup();
            }
            else
            {
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
            }

            //patrol next group
            if (OnGroupDoubleClick != null)
            {
                App.StartupOption.GroupPatrol = true;
                App.StartupOption.DeviceGroup = _patrolGroup.Id.ToString();
                App.StartupOption.SaveSetting();

                RaiseOnGroupDoubleClick(new EventArgs<IDeviceGroup>(_patrolGroup));
            }

        }

        private void GetCMSNextGroup()
        {
            if (_patrolNVR != null)
            {
                if (_patrolNVR is ICMS)
                {
                    if (_patrolNVR.Device.Groups.Count == 0 || !_patrolNVR.IsPatrolInclude)
                    {
                        _patrolGroup = null;
                        _patrolNVR = null;
                    }
                }
                else
                {
                    if (_patrolNVR.Device.Devices.Count == 0 || !_patrolNVR.IsPatrolInclude)
                    {
                        _patrolGroup = null;
                        _patrolNVR = null;
                    }
                }
            }

            if (_patrolNVR == null)
                GetNextNVR();

            GetNVRNextGroup();

            if (_patrolGroup == null)
            {
                GetNextNVR();
                GetNVRNextGroup();
            }
        }

        private void GetNextNVR()
        {
            var list = new List<IServer>();

            if (CMS != null)
            {
                foreach (INVR nvr in CMS.NVRManager.NVRs.Values)
                {
                    if (!nvr.IsPatrolInclude) continue;
                    list.Add(nvr);
                }

                if (CMS.Device.Groups.Count > 0)
                    list.Insert(0, CMS);
            }
            else if (PTS != null)
            {
                foreach (INVR nvr in PTS.NVR.NVRs.Values)
                {
                    if (!nvr.IsPatrolInclude) continue;
                    list.Add(nvr);
                }
            }

            foreach (IServer nvr in list)
            {
                if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
                //if (nvr.ReadyState != ReadyState.Ready) continue;
                if (!(nvr is INVR)) continue;

                if (_patrolNVR == null)
                {
                    if (nvr is ICMS)
                    {
                        foreach (var deviceGroup in nvr.Device.Groups)
                        {
                            if (deviceGroup.Value.Items.Count > 0)
                            {
                                _patrolNVR = nvr;
                                break;
                            }
                        }
                        if (_patrolNVR != null) break;
                    }
                    else if (nvr.Device.Devices.Count > 0)
                    {
                        _patrolNVR = nvr;
                    }
                }

                if (nvr == _patrolNVR)
                    _patrolNVR = null;
            }

            if (_patrolNVR == null)
            {
                foreach (INVR nvr in list.OfType<INVR>())
                {
                    if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
                    //if (nvr.ReadyState != ReadyState.Ready) continue;
                    if (!nvr.IsPatrolInclude) continue;

                    if (nvr.Device.Devices.Count > 0 || nvr is ICMS)
                    {
                        _patrolNVR = nvr;
                        break;
                    }
                }
            }
        }

        private void GetNVRNextGroup()
        {
            if (_patrolNVR == null)
            {
                _patrolGroup = null;
                return;
            }

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in _patrolNVR.Device.Groups)
            {
                if (_patrolGroup == null && obj.Value.Items.Count > 0)
                {
                    _patrolGroup = obj.Value;
                    break;
                }

                if (obj.Value == _patrolGroup)
                    _patrolGroup = null;
            }
        }

        private void GetNextGroup()
        {
            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Server.Device.Groups)
            {
                if (_patrolGroup == null && obj.Value.Items.Count > 0)
                {
                    _patrolGroup = obj.Value;
                    return;
                }

                if (obj.Value == _patrolGroup)
                    _patrolGroup = null;
            }

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Server.User.Current.DeviceGroups)
            {
                if (_patrolGroup == null && obj.Value.Items.Count > 0)
                {
                    _patrolGroup = obj.Value;
                    return;
                }

                if (obj.Value == _patrolGroup)
                    _patrolGroup = null;
            }

            if (_patrolGroup != null) return;

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Server.Device.Groups)
            {
                if (obj.Value.Items.Count > 0)
                {
                    _patrolGroup = obj.Value;
                    return;
                }
            }

            //if STILL no group, ingore PatrolAllDeviceGroup setting, get the first one to patrol
            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Server.Device.Groups)
            {
                if (obj.Value.Items.Count > 0)
                {
                    _patrolGroup = obj.Value;
                    return;
                }
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

        private void PatrolButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (!_patrolGroupTimer.Enabled)
            {
                if (CMS != null || PTS != null)
                {
                    var hasDevice = false;
                    if (CMS != null)
                    {
                        if (CMS.NVRManager.NVRs.Values.Where(nvr => nvr.IsPatrolInclude).Any(nvr => nvr.Device.Devices.Count != 0))
                        {
                            hasDevice = true;
                        }
                    }
                    if (PTS != null)
                    {
                        if (PTS.NVR.NVRs.Values.Where(nvr => nvr.IsPatrolInclude).Any(nvr => nvr.Device.Devices.Count != 0))
                        {
                            hasDevice = true;
                        }
                    }
                    if (!hasDevice) return;
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
                        _patrolGroupTimer.Enabled = false;
                        return;
                    }
                }

                SharedToolTips.SharedToolTip.SetToolTip(PatrolButton, Localization["DeviceTree_DisablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString()));

                PatrolButton.BackgroundImage = _patrolactivate;
                _patrolGroupTimer.Interval = Server.Configure.PatrolInterval * 1000;
                _patrolGroupTimer.Enabled = false;
                _patrolGroupTimer.Enabled = true;
                //inst patrol
                GroupPatrol();

                Server.WriteOperationLog("Enable group patrol (Dewell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString()));
            }
            else
            {
                _patrolGroupTimer.Enabled = false;
                SharedToolTips.SharedToolTip.SetToolTip(PatrolButton, Localization["DeviceTree_EnablePatrol"].Replace("%1", Server.Configure.PatrolInterval.ToString()));
                PatrolButton.BackgroundImage = _patrol;

                Server.WriteOperationLog("Disable group patrol (Dewell Time %1 sec)".Replace("%1", Server.Configure.PatrolInterval.ToString()));
            }
        }

        public void ViewModelPanelNVRMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as NVRControl;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;
                DragDropLabel.Text = ((NVRControl)sender).NVR.Name;

                _patrolGroup = null;
                _patrolNVR = ((NVRControl)sender).NVR;
                GetNVRNextGroup();
                OnDragStart(this, new EventArgs<Object>(_patrolNVR));
            }
        }

        public void ViewModelPanelGroupMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControl;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                var num = ((control.DeviceGroup.Items.Count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                    Replace("%1", control.DeviceGroup.Items.Count.ToString());

                DragDropLabel.Text = control.DeviceGroup + " " + num;

                _patrolGroup = control.DeviceGroup;
                _patrolNVR = _patrolGroup.Server;
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

            var control = sender as DeviceControl;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                control.Device.DeviceType = DeviceType.Device;
                DragDropLabel.Text = control.Device.ToString();

                OnDragStart(this, new EventArgs<Object>(control.Device));
            }
        }

        public void ViewModelPanelNVRMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as NVRControl;
            if (control != null && OnNVRDoubleClick != null)
            {
                _patrolGroup = null;
                _patrolNVR = control.NVR;
                GetNVRNextGroup();
                OnNVRDoubleClick(this, new EventArgs<INVR>(_patrolNVR as INVR));
            }
        }

        public virtual void ViewModelPanelGroupMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as GroupControl;
            if (control != null && OnGroupDoubleClick != null)
            {
                _patrolGroup = control.DeviceGroup;
                _patrolNVR = _patrolGroup.Server;
                RaiseOnGroupDoubleClick(new EventArgs<IDeviceGroup>(_patrolGroup));
            }
        }

        public virtual void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceControl;
            if (control != null && OnDeviceDoubleClick != null)
            {
                RaiseOnDeviceDoubleClick(new EventArgs<IDevice>(control.Device));
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

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
        }

        public void Minimize()
        {
            IsMinimize = true;
        }

        public void Maximize()
        {
            IsMinimize = false;
        }
    }
}
