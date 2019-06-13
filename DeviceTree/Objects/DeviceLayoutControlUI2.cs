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
    public sealed class DeviceLayoutControlUI2 : Panel
    {
        public event MouseEventHandler OnDeviceLayoutMouseDrag;
        public event MouseEventHandler OnDeviceLayoutMouseDown;
        public event MouseEventHandler OnDeviceLayoutMouseDoubleClick;

        public event MouseEventHandler OnSubLayoutMouseDrag;
        public event MouseEventHandler OnSubLayoutMouseDown;
        public event MouseEventHandler OnSubLayoutMouseDoubleClick;

        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel SubLayoutControlContainer = new DoubleBufferPanel();
        private IDeviceLayout _deviceLayout;
        public IDeviceLayout DeviceLayout
        {
            get { return _deviceLayout; }
            set
            {
                _deviceLayout = value;

                UpdateToolTips();
            }
        }

        private static readonly Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        private static readonly Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);

        private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
        private static readonly Image _normal = Resources.GetResources(Properties.Resources.normal, Properties.Resources.IMGNormal);
        private static readonly Image _online = Resources.GetResources(Properties.Resources.online, Properties.Resources.IMGOnLine);
        private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.groupPanelBG, Properties.Resources.IMGGroupPanelBG);

        private readonly System.Timers.Timer _timer;

        public DeviceLayoutControlUI2()
        {
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

            SubLayoutControlContainer.BackColor = Color.FromArgb(46, 49, 55);
            SubLayoutControlContainer.Dock = DockStyle.Fill;
            SubLayoutControlContainer.AutoSize = true;
            SubLayoutControlContainer.Padding = new Padding(0, 0, 0, 0);

            Controls.Add(SubLayoutControlContainer);
            Controls.Add(TitlePanel);

            TitlePanel.MouseDoubleClick += TitlePanelMouseDoubleClick;
            TitlePanel.MouseUp += TitlePanelMouseUp;
            TitlePanel.MouseDown += TitlePanelMouseDown;

            TitlePanel.Paint += TitlePanelPaint;
        }

        private void HideOrShowContainer(Object sender, EventArgs e)
        {
            _timer.Enabled = false;

            SubLayoutControlContainer.Visible = !SubLayoutControlContainer.Visible;
            TitlePanel.Invalidate();
        }

        private Rectangle _switchRectangle = new Rectangle(188, 0, 32, 39);
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private static RectangleF _nameRectangleF = new RectangleF(45, 11, 142, 17);
        private Boolean _displayIcon = true;

        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_deviceLayout == null) return;

            Graphics g = e.Graphics;

            if (_displayIcon)
                g.DrawImage((SubLayoutControlContainer.Visible ? _arrowUp : _arrowDown), 192, 17);

            if (_deviceLayout.ReadyState == ReadyState.New)
            {
                g.DrawImage(_new, 4, 11);
            }
            else
            {
                if (_deviceLayout.ReadyState == ReadyState.Modify)
                    g.DrawImage(_modify, 4, 11);

                var camera = _deviceLayout.Items.FirstOrDefault(device => device != null) as ICamera;
                //----------------------------------------------------------------------

                if (camera != null && camera.ReadyState != ReadyState.New)
                {
                    switch (camera.Status)
                    {
                        case CameraStatus.Recording:
                            g.DrawImage(_record, 24, 11);
                            break;

                        case CameraStatus.Streaming:
                            g.DrawImage(_online, 24, 11);
                            break;

                        default:
                            g.DrawImage(_normal, 24, 11);
                            break;
                    }
                }
            }

            g.DrawString(_deviceLayout.ToString(), _font, Brushes.White, _nameRectangleF);
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

        private void TitlePanelMouseUp(Object sender, MouseEventArgs e)
        {
            TitlePanel.MouseMove -= TitlePanelMouseMove;

            // click on rectangle
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                SubLayoutControlContainer.Visible = !SubLayoutControlContainer.Visible;
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

            if (OnDeviceLayoutMouseDown != null)
                OnDeviceLayoutMouseDown(this, e);
        }

        private void TitlePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnDeviceLayoutMouseDrag != null)
            {
                TitlePanel.MouseMove -= TitlePanelMouseMove;
                OnDeviceLayoutMouseDrag(this, e);
            }
        }

        private Boolean _isDblClick;
        private void TitlePanelMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            _isDblClick = true;
            _timer.Enabled = false;

            if (OnDeviceLayoutMouseDoubleClick != null)
                OnDeviceLayoutMouseDoubleClick(this, e);
        }

        public void CreateTree()
        {
            var list = _deviceLayout.SubLayouts.Values.Where(obj => obj != null).ToList();
            list.Sort((x, y) => (y.Id - x.Id));
            var subLayoutControls = new List<SubLayoutControlUI2>();

            foreach (ISubLayout layout in list)
            {
                if (layout == null) continue;
                if (layout.Id == 99) continue;
                var sublayoutControl = GetSubLayoutControl();

                sublayoutControl.SubLayout = layout;
                subLayoutControls.Add(sublayoutControl);
            }
            if (subLayoutControls.Count > 0)
            {
                SubLayoutControlContainer.Controls.AddRange(subLayoutControls.ToArray());
                subLayoutControls.Clear();
            }
        }


        protected Queue<SubLayoutControlUI2> RecycleSubLayout = new Queue<SubLayoutControlUI2>();
        public SubLayoutControlUI2 GetSubLayoutControl()
        {
            if (RecycleSubLayout.Count > 0)
            {
                return RecycleSubLayout.Dequeue();
            }

            var deviceControl = new SubLayoutControlUI2();
            deviceControl.OnSubLayoutMouseDown += OnSubLayoutMouseDown;
            deviceControl.OnSubLayoutMouseDrag += OnSubLayoutMouseDrag;
            deviceControl.OnSubLayoutMouseDoubleClick += OnSubLayoutMouseDoubleClick;

            return deviceControl;
        }

        public void Clear()
        {
            foreach (SubLayoutControlUI2 deviceControl in SubLayoutControlContainer.Controls)
            {
                if (!RecycleSubLayout.Contains(deviceControl))
                {
                    deviceControl.SubLayout = null;
                    RecycleSubLayout.Enqueue(deviceControl);
                }
            }

            SubLayoutControlContainer.Controls.Clear();
        }

        public void UpdateRecordingStatus()
        {
            TitlePanel.Invalidate();
            foreach (SubLayoutControlUI2 subControl in SubLayoutControlContainer.Controls)
            {
                subControl.Invalidate();
            }
        }

        public void UpdateToolTips()
        {
            if (_deviceLayout != null)
            {
                var tooltips = _deviceLayout + " " + _deviceLayout.Width + "x" + _deviceLayout.Height;

                SharedToolTips.SharedToolTip.SetToolTip(TitlePanel, tooltips);
            }
            else
                SharedToolTips.SharedToolTip.SetToolTip(TitlePanel, "");

            foreach (SubLayoutControlUI2 deviceControl in SubLayoutControlContainer.Controls)
            {
                deviceControl.UpdateToolTips();
            }
        }
    }
}
