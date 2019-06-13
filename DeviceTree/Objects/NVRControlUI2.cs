using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
    public class NVRControlUI2 : Panel
    {
        public event MouseEventHandler OnNVRMouseDrag;
        public event MouseEventHandler OnNVRMouseDown;
        public event MouseEventHandler OnNVRMouseDoubleClick;

        public event MouseEventHandler OnDeviceMouseDrag;
        public event MouseEventHandler OnDeviceMouseDown;
        public event MouseEventHandler OnDeviceMouseDoubleClick;

        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel DeviceControlContainer = new DoubleBufferPanel();
        private readonly System.Timers.Timer _refreshTimer = new System.Timers.Timer();

        private Boolean _isPTS;
        public Boolean IsPTS{
            set
            {
                _isPTS = value;
                if(value)
                {
                    TitlePanel.Size = new Size(295, 39);
                    _nameRectangleF = new RectangleF(45, 11, 207, 17);
                }
                    
            }
        }
        private IServer _nvr;
        public IServer NVR
        {
            get { return _nvr; }
            set
            {
                _nvr = value;
                
                if (_nvr != null)
                {
                    if(_nvr is ICMS)
                    {
                        SharedToolTips.SharedToolTip.SetToolTip(this, "");
                    }
                    else
                    {
                        CreateTree();
                        DeviceControlContainer.Visible = false;

                        if (_nvr.Device.Devices.Count <= 1)
                            SharedToolTips.SharedToolTip.SetToolTip(TitlePanel, _nvr + Environment.NewLine +
                            Localization["GroupPanel_NumDevice"].Replace("%1", _nvr.Device.Devices.Count.ToString()));
                        else
                            SharedToolTips.SharedToolTip.SetToolTip(TitlePanel, _nvr + Environment.NewLine +
                            Localization["GroupPanel_NumDevices"].Replace("%1", _nvr.Device.Devices.Count.ToString()));

                        if (_nvr.Manufacture == "iSAP Failover Server")
                        {
                            _refreshTimer.Elapsed += RefreshTimerElapsed;
                            _refreshTimer.Interval = _nvr.ServerStatusCheckInterval * 1000;//1 min
                            _refreshTimer.SynchronizingObject = _nvr.Form;
                            _refreshTimer.Enabled = true;
                        }
                    }
                }
                else
                    SharedToolTips.SharedToolTip.SetToolTip(this, "");
            }
        }

        private Boolean _isLoadFailoverDeviceList;
        private delegate List<IDevice> ReadDeviceListDelegate();
        private delegate void LoadDeviceListCallbackDelegate(IAsyncResult result);

        private void RefreshTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReflashFailoverDeviceList();
        }

        private void ReflashFailoverDeviceList()
        {
            if (_nvr.Manufacture != "iSAP Failover Server") return;
            if (_isLoadFailoverDeviceList) return;
            //foreach (KeyValuePair<UInt16, IDevice> device in _nvr.Device.Devices)
            //{
            //    var camera = device.Value as ICamera;
            //    if (camera == null) continue;
            //    device.Value.Name = "Failover Device " + device.Key;
            //    camera.Profile.NetworkAddress = String.Empty;
            //    //camera.StreamConfig.Compression = Compression.Off;
            //    //camera.StreamConfig.Resolution = Resolution.NA;
            //}

            if (_nvr == null) return;
            _isLoadFailoverDeviceList = true;
            ReadDeviceListDelegate loadDeviceListDelegate = _nvr.ReadDeviceList;
            loadDeviceListDelegate.BeginInvoke(LoadDeviceListCallback, loadDeviceListDelegate);
        }

        private UInt16 _deviceListcount = 0;
        private void LoadDeviceListCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new LoadDeviceListCallbackDelegate(LoadDeviceListCallback), result);
                return;
            }

            var list = ((ReadDeviceListDelegate)result.AsyncState).EndInvoke(result);

            var needUpdateTooltips = false;
            foreach (IDevice device in list)
            {
                var deviceCamera = device as ICamera;
                if (deviceCamera == null) return;
                if (_nvr.Device.Devices.ContainsKey(deviceCamera.Id))
                {
                    var camera = _nvr.Device.Devices[deviceCamera.Id] as ICamera;
                    if (camera == null) continue;
                    camera.Name = deviceCamera.Name;
                    camera.Profile.NetworkAddress = deviceCamera.Profile.NetworkAddress;
                    camera.Model.Manufacture = deviceCamera.Model.Manufacture;
                    if (deviceCamera.StreamConfig == null)
                    {
                        camera.StreamConfig.Compression = Compression.Off;
                        camera.StreamConfig.Resolution = Resolution.NA;
                        camera.StreamConfig.Bitrate = Bitrate.NA;
                    }
                    else
                    {
                        camera.StreamConfig.Compression = deviceCamera.StreamConfig.Compression;
                        camera.StreamConfig.Resolution = deviceCamera.StreamConfig.Resolution;
                        camera.StreamConfig.Bitrate = deviceCamera.StreamConfig.Bitrate;
                    }
                    

                    needUpdateTooltips = true;
                    continue;
                }
            }

            if (needUpdateTooltips)
                UpdateToolTips();

            if (DeviceControlContainer.Controls.Count != list.Count)
            {

                if (_nvr.Manufacture == "iSAP Failover Server")
                {
                    if (_nvr.FailoverDeviceList == null)
                        _nvr.FailoverDeviceList = new List<IDevice>();
                    else
                        _nvr.FailoverDeviceList.Clear();
                }

                foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
                {
                    if (!RecycleDevice.Contains(deviceControl))
                    {
                        deviceControl.Device = null;
                        RecycleDevice.Enqueue(deviceControl);
                    }
                }

                DeviceControlContainer.Controls.Clear();

                var deviceControls = new List<Control>();

                var sortResult = GetSortedDevice(_nvr);
                foreach (IDevice device in sortResult)
                {
                    if (device == null) continue;
                    if (list.Where(s => s.Id == device.Id).Count() > 0)
                    {
                        var deviceControl = GetDeviceControl();

                        deviceControl.Device = device;
                        deviceControls.Add(deviceControl);
                        _nvr.FailoverDeviceList.Add(device);
                    }
                }

                if (deviceControls.Count > 0)
                {
                    DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                    deviceControls.Clear();
                }

                _deviceListcount = (ushort)list.Count;
            }

            _isLoadFailoverDeviceList = false;
        }

        public void UpdateFailoverSyncTimer(Boolean enable)
        {
            if (_nvr.Manufacture != "iSAP Failover Server") return;
            _refreshTimer.Enabled = enable;
        }

        public Dictionary<String, String> Localization;

        protected static Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        protected static Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
        private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
        private static readonly Image _normal = Resources.GetResources(Properties.Resources.normal, Properties.Resources.IMGNormal);
        private static readonly Image _online = Resources.GetResources(Properties.Resources.online, Properties.Resources.IMGOnLine);
        private static readonly Image _yellow = Resources.GetResources(Properties.Resources.yellow, Properties.Resources.IMGYellow);

        private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.groupPanelBG, Properties.Resources.IMGGroupPanelBG);
        private static readonly Image _titleFailoverPanelBg = Resources.GetResources(Properties.Resources.failoverPanelBG, Properties.Resources.IMGFailoverPanelBG);

        private System.Timers.Timer _timer;
        public NVRControlUI2()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"GroupPanel_NumDevice", "(%1 Device)"},
                                   {"GroupPanel_NumDevices", "(%1 Devices)"},
                          
                               };
            Localizations.Update(Localization);

            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(55, 58, 66);//drag label will use it
            ForeColor = Color.White; //drag label will use it

            _timer = new System.Timers.Timer(200);
            _timer.Elapsed += HideOrShowContainer;
            _timer.SynchronizingObject = this;

            TitlePanel.Size = new Size(220, 39);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.BackgroundImage = _titlePanelBg;
            TitlePanel.BackgroundImageLayout = ImageLayout.Center;
            TitlePanel.Cursor = Cursors.Hand;

            DeviceControlContainer.BackColor = Color.FromArgb(46, 49, 55);
            DeviceControlContainer.Dock = DockStyle.Fill;
            DeviceControlContainer.AutoSize = true;
            DeviceControlContainer.Padding = new Padding(0, 0, 0, 0);
            DeviceControlContainer.Visible = false;

            Controls.Add(DeviceControlContainer);
            Controls.Add(TitlePanel);

            TitlePanel.Paint += TitlePanelPaint;

            TitlePanel.MouseDoubleClick += TitlePanelDoubleClick;
            TitlePanel.MouseUp += TitlePanelMouseUp;
            TitlePanel.MouseDown += TitlePanelMouseDown;
        }

        private Boolean _isFailover;
        public virtual Boolean IsFailover
        {
            set
            {
                if(value)
                {
                    TitlePanel.BackgroundImage = _titleFailoverPanelBg;
                }
                else
                {
                    TitlePanel.BackgroundImage = _titlePanelBg;
                }
                _isFailover = value;
            }    
        }

        private Rectangle _switchRectangle = new Rectangle(188, 0, 32, 39);
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        
        private static RectangleF _nameRectangleF = new RectangleF(45, 11, 142, 17);
        protected void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_nvr == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _arrowUp : _arrowDown), _isPTS ? 267 : 192, 17);

            if (_nvr.ReadyState == ReadyState.New)
                g.DrawImage(_new, 4, 11);
            else
            {
                if (_nvr.ReadyState == ReadyState.Modify)
                    g.DrawImage(_modify, 4, 11);

                if ( _nvr.ReadyState != ReadyState.New)
                {
                    var statusIcon = GetStatusIcon(_nvr.NVRStatus);
                    if (statusIcon != null)
                    {
                        g.DrawImage(statusIcon, 24, 11);
                    }
                }
            }

            Int32 count = GetSortedDevice(_nvr).Count;

            var name = _nvr + "   " +
                       ((count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                           Replace("%1", count.ToString());

            g.DrawString(name, _font, Brushes.White, _nameRectangleF);
        }

        protected virtual Image GetStatusIcon(NVRStatus status)
        {
            switch (status)
            {
                case NVRStatus.WrongAccountPassowrd:
                    return _yellow;

                case NVRStatus.Health:
                    return _online;

                default:
                    return _normal;
            }
        }

        public void UpdateRecordingStatus()
        {
            TitlePanel.Invalidate();
            foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
            {
                if (!((deviceControl).Device is ICamera)) continue;

                (deviceControl).Invalidate();
            }
        }

        private Boolean _isDblClick;
        private void TitlePanelDoubleClick(Object sender, MouseEventArgs e)
        {
            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            _isDblClick = true;
            _timer.Enabled = false;

            if (OnNVRMouseDoubleClick != null)
                OnNVRMouseDoubleClick(this, e);
        }

        private Point _position;
        private void TitlePanelMouseDown(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                //GroupControlContainer.Visible = !GroupControlContainer.Visible;
                Invalidate();
                return;
            }

            _position = e.Location;
            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            TitlePanel.MouseMove -= TitlePanelMouseMove;
            TitlePanel.MouseMove += TitlePanelMouseMove;
        }

        private Boolean _isCreateTree;
        private void TitlePanelMouseUp(Object sender, MouseEventArgs e)
        {
            TitlePanel.MouseMove -= TitlePanelMouseMove;

            // click on rectangle
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                DeviceControlContainer.Visible = !DeviceControlContainer.Visible;

                if (DeviceControlContainer.Visible && !_isCreateTree)
                    CreateTree();

                TitlePanel.Invalidate();
            }
            //click on same point
            else if ((e.X == _position.X && e.Y == _position.Y))
            {
                if (!_isDblClick)
                {
                    _timer.Enabled = false;
                    _timer.Enabled = true;
                }
                _isDblClick = false;
            }

            if (OnNVRMouseDown != null)
                OnNVRMouseDown(this, e);
        }

        private void TitlePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnNVRMouseDrag != null)
            {
                TitlePanel.MouseMove -= TitlePanelMouseMove;
                OnNVRMouseDrag(this, e);
            }
        }
        
        public void CreateTree()
        {
            DeviceControlContainer.Visible = true;
            _isCreateTree = true;
            if (_nvr.Manufacture == "iSAP Failover Server")
            {
                ReflashFailoverDeviceList();
                return;
            }
            var deviceControls = new List<Control>();

            var sortResult = GetSortedDevice(_nvr);
            foreach (IDevice device in sortResult)
            {
                if (device == null) continue;
                var deviceControl = GetDeviceControl();

                deviceControl.Device = device;
                deviceControls.Add(deviceControl);
            }
            if (deviceControls.Count > 0)
            {
                DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                deviceControls.Clear();
            }
        }

        public void ClearViewModel()
        {
            //TitlePanel.BackgroundImage = _titlePanelBg;
            TitlePanel.BackgroundImage = _isFailover ? _titleFailoverPanelBg : _titlePanelBg;

            DeviceControlContainer.Visible = false;
            _isCreateTree = false;
            _refreshTimer.Enabled = false;
            foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
            {
                if (!RecycleDevice.Contains(deviceControl))
                {
                    deviceControl.Device = null;
                    RecycleDevice.Enqueue(deviceControl);
                }
            }

            DeviceControlContainer.Controls.Clear();
        }

        public void UpdateToolTips()
        {
            foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
            {
                deviceControl.UpdateToolTips();
            }
        }

        protected Queue<DeviceControlUI2> RecycleDevice = new Queue<DeviceControlUI2>();

        protected virtual List<IDevice> GetSortedDevice(IServer server)
        {
            var sortResult = server.Device.Devices.Values.OrderByDescending(d => d.Id).ToList();

            return sortResult;
        }

        protected DeviceControlUI2 GetDeviceControl()
        {
            if (RecycleDevice.Count > 0)
            {
                return RecycleDevice.Dequeue();
            }

            var deviceControl = new DeviceControlUI2 {isShowNVRName = false};
            deviceControl.OnDeviceMouseDown += OnDeviceMouseDown;
            deviceControl.OnDeviceMouseDrag += OnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick += OnDeviceMouseDoubleClick;

            return deviceControl;
        }

        private void HideOrShowContainer(Object sender, EventArgs e)
        {
            _timer.Enabled = false;

            DeviceControlContainer.Visible = !DeviceControlContainer.Visible;

            if (DeviceControlContainer.Visible && !_isCreateTree)
                CreateTree();

            TitlePanel.Invalidate();
        }
    }
}