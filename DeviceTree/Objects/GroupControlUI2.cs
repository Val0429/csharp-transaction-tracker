using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
    public class GroupControlUI2 : Panel
    {
        public event MouseEventHandler OnDeviceMouseDrag;
        protected void RaiseOnDeviceMouseDrag(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDrag;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDown;
        protected void RaiseOnDeviceMouseDown(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDoubleClick;
        protected void RaiseOnDeviceMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDoubleClick;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public event MouseEventHandler OnGroupMouseDrag;
        public event MouseEventHandler OnGroupMouseDown;
        public event MouseEventHandler OnGroupMouseDoubleClick;

        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel DeviceControlContainer = new DoubleBufferPanel();

        protected IDeviceGroup _deviceGroup;
        public virtual IDeviceGroup DeviceGroup
        {
            get { return _deviceGroup; }
            set
            {
                _deviceGroup = value;

                var tooltip = string.Empty;

                if (_deviceGroup != null)
                {
                    tooltip = string.Format("{0}{1}{2}", _deviceGroup, Environment.NewLine,
                        _deviceGroup.Items.Count <= 1
                            ? Localization["GroupPanel_NumDevice"].Replace("%1", _deviceGroup.Items.Count.ToString(CultureInfo.InvariantCulture))
                            : Localization["GroupPanel_NumDevices"].Replace("%1", _deviceGroup.Items.Count.ToString(CultureInfo.InvariantCulture)));
                }

                SharedToolTips.SharedToolTip.SetToolTip(TitlePanel, tooltip);
            }
        }
        public Dictionary<String, String> Localization;

        protected static Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        protected static Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
        private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.groupPanelBG, Properties.Resources.IMGGroupPanelBG);

        private readonly System.Timers.Timer _timer;


        // Constructor
        public GroupControlUI2()
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

        private void HideOrShowContainer(Object sender, EventArgs e)
        {
            _timer.Enabled = false;

            DeviceControlContainer.Visible = !DeviceControlContainer.Visible;

            if (DeviceControlContainer.Visible && !_isCreateTree)
                CreateTree();

            TitlePanel.Invalidate();
        }

        private Rectangle _switchRectangle = new Rectangle(188, 0, 32, 39);
        protected readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private static readonly RectangleF _nameRectangleF = new RectangleF(22, 11, 165, 17);
        protected virtual void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_deviceGroup == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _arrowUp : _arrowDown), 192, 17);

            if (_deviceGroup.ReadyState == ReadyState.New)
                g.DrawImage(_new, 4, 11);
            else
            {
                if (_deviceGroup.ReadyState == ReadyState.Modify)
                    g.DrawImage(_modify, 4, 11);
            }

            Int32 count = _deviceGroup.Items.Count;

            var name = _deviceGroup + "   " +
                       ((count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                           Replace("%1", count.ToString());

            g.DrawString(name, _font, Brushes.White, _nameRectangleF);
        }

        private Point _position;

        private void TitlePanelMouseDown(Object sender, MouseEventArgs e)
        {
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

            if (OnGroupMouseDown != null)
                OnGroupMouseDown(this, e);
        }

        private void TitlePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnGroupMouseDrag != null)
            {
                TitlePanel.MouseMove -= TitlePanelMouseMove;
                OnGroupMouseDrag(this, e);
            }
        }

        private Boolean _isDblClick;
        private void TitlePanelDoubleClick(Object sender, MouseEventArgs e)
        {
            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            _isDblClick = true;
            _timer.Enabled = false;

            if (OnGroupMouseDoubleClick != null)
                OnGroupMouseDoubleClick(this, e);
        }

        public void CreateTree()
        {
            DeviceControlContainer.Visible = true;
            _isCreateTree = true;
            if (_deviceGroup.Items.Count == 0) return;

            var list = GetSortedDevices();
            var deviceControls = new List<Control>();

            foreach (IDevice device in list)
            {
                if (device == null) continue;

                if (device is IDeviceLayout)
                {
                    var layoutControl = GetDeviceControl(device);
                    deviceControls.Add(layoutControl as Control);

                }
                else if (device is ISubLayout)
                {
                    var sublayoutControl = GetDeviceControl(device);
                    deviceControls.Add(sublayoutControl as Control);
                }
                else
                {
                    var deviceControl = GetDeviceControl(device);
                    deviceControls.Add(deviceControl as Control);
                }
            }

            if (deviceControls.Any())
            {
                DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                deviceControls.Clear();
            }
        }

        protected virtual IEnumerable<IDevice> GetSortedDevices()
        {
            var list = new List<IDevice>(_deviceGroup.Items);
            list.Sort((x, y) => (y.Id - x.Id));

            return list;
        }

        protected Queue<DeviceControlUI2> RecycleDevice = new Queue<DeviceControlUI2>();

        public virtual IDeviceControl GetDeviceControl(IDevice device)
        {
            if (RecycleDevice.Count > 0)
            {
                var control = RecycleDevice.Dequeue();
                control.Device = device;
                control.OnDeviceMouseDoubleClick -= RaiseOnDeviceMouseDoubleClick;
                control.OnDeviceMouseDoubleClick += RaiseOnDeviceMouseDoubleClick;
                return control;
            }

            var deviceControl = CreateDeviceControl(device);

            return deviceControl;
        }

        protected virtual IDeviceControl CreateDeviceControl(IDevice device)
        {
            var deviceControl = new DeviceControlUI2 { Device = device };
            deviceControl.OnDeviceMouseDown += RaiseOnDeviceMouseDown;
            deviceControl.OnDeviceMouseDrag += RaiseOnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick -= RaiseOnDeviceMouseDoubleClick;
            deviceControl.OnDeviceMouseDoubleClick += RaiseOnDeviceMouseDoubleClick;

            return deviceControl;
        }

        public void ClearViewModel()
        {
            _isCreateTree = false;
            foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
            {
                if (!RecycleDevice.Contains(deviceControl))
                {
                    deviceControl.Device = null;
                    deviceControl.OnDeviceMouseDoubleClick -= RaiseOnDeviceMouseDoubleClick;
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

        public void UpdateRecordingStatus()
        {
            foreach (DeviceControlUI2 deviceControl in DeviceControlContainer.Controls)
            {
                if (!((deviceControl).Device is ICamera)) continue;

                (deviceControl).Invalidate();
            }
        }
    }
}