using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;
using PanelBase;

namespace POSRegister.Objects
{
    public sealed class POSControlUI2 : Panel
    {
        public event MouseEventHandler OnPOSMouseDrag;
        public event MouseEventHandler OnPOSMouseDown;
        public event MouseEventHandler OnPOSMouseDoubleClick;

        public event MouseEventHandler OnDeviceMouseDrag;
        public event MouseEventHandler OnDeviceMouseDown;
        public event MouseEventHandler OnDeviceMouseDoubleClick;

        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel DeviceControlContainer = new DoubleBufferPanel();
        private IPOS _pos;
        public IPOS POS
        {
            get { return _pos; }
            set
            {
                _pos = value;

                if (_pos != null)
                {
                    String str = (_pos.Items.Count <= 1)
                        ? "GroupPanel_NumDevice"
                        : "GroupPanel_NumDevices";

                    SharedToolTips.SharedToolTip.SetToolTip(this, _pos + Environment.NewLine +
                        Localization[str].Replace("%1", _pos.Items.Count.ToString()));
                }
                else
                    SharedToolTips.SharedToolTip.SetToolTip(this, "");
            }
        }
        public Dictionary<String, String> Localization;

        protected static Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        protected static Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);

        private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.groupPanelBG, Properties.Resources.IMGGroupPanelBG);

        private System.Timers.Timer _timer;
        public POSControlUI2()
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

            TitlePanel.Size = new Size(350, 39);
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

        private Rectangle _switchRectangle = new Rectangle(265, 0, 32, 39);
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        
        private static RectangleF _nameRectangleF = new RectangleF(25, 11, 265, 17);
        protected void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_pos == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _arrowUp : _arrowDown), 269, 17);

            if (_pos.ReadyState == ReadyState.New)
                g.DrawImage(_new, 4, 11);
            else
            {
                if (_pos.ReadyState == ReadyState.Modify)
                    g.DrawImage(_modify, 4, 11);

            }

            Int32 count = _pos.Items.Count;

            var name = _pos + "   " +
                       ((count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                           Replace("%1", count.ToString());

            g.DrawString(name, _font, Brushes.White, _nameRectangleF);
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

            if (OnPOSMouseDoubleClick != null)
                OnPOSMouseDoubleClick(this, e);
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

            if (OnPOSMouseDown != null)
                OnPOSMouseDown(this, e);
        }

        private void TitlePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnPOSMouseDrag != null)
            {
                TitlePanel.MouseMove -= TitlePanelMouseMove;
                OnPOSMouseDrag(this, e);
            }
        }
        
        public void CreateTree()
        {
            ClearViewModel();
            DeviceControlContainer.Visible = true;
            _isCreateTree = true;
            var deviceControls = new List<DeviceControlUI2>();

            var sortResult = GetSortedDevice(_pos);
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
            DeviceControlContainer.Visible = false;
            _isCreateTree = false;
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

        protected List<IDevice> GetSortedDevice(IPOS server)
        {
            var sortResult = server.Items.OrderByDescending(d => d.Id).ToList();

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