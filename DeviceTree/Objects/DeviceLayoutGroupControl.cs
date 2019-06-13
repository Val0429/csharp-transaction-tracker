using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
    public sealed class DeviceLayoutGroupControl : Panel
    {
        public event MouseEventHandler OnDeviceLayoutGroupMouseDrag;
        public event MouseEventHandler OnDeviceLayoutGroupMouseDown;
        public event MouseEventHandler OnDeviceLayoutGroupMouseDoubleClick;

        public event MouseEventHandler OnDeviceLayoutMouseDrag;
        public event MouseEventHandler OnDeviceLayoutMouseDown;
        public event MouseEventHandler OnDeviceLayoutMouseDoubleClick;

        public event MouseEventHandler OnSubLayoutMouseDrag;
        public event MouseEventHandler OnSubLayoutMouseDown;
        public event MouseEventHandler OnSubLayoutMouseDoubleClick;

        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel DeviceLayoutControlContainer = new DoubleBufferPanel();

        public INVR NVR;
        public IDeviceGroup DeviceGroup { get; set; }
        public Dictionary<String, String> Localization;

        private static readonly Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        private static readonly Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.folderPanelBG, Properties.Resources.IMGFolderPanelBG);
        private System.Timers.Timer _timer;
        public DeviceLayoutGroupControl()
        {
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(59, 62, 67);//drag label will use it
            ForeColor = Color.White; //drag label will use it

            _timer = new System.Timers.Timer(200);
            _timer.Elapsed += HideOrShowContainer;
            _timer.SynchronizingObject = this;

            TitlePanel.Size = new Size(220, 32);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.BackgroundImage = _titlePanelBg;
            TitlePanel.BackgroundImageLayout = ImageLayout.Center;
            TitlePanel.Cursor = Cursors.Hand;

            DeviceLayoutControlContainer.BackColor = Color.FromArgb(55, 59, 66);
            DeviceLayoutControlContainer.Dock = DockStyle.Fill;
            DeviceLayoutControlContainer.AutoSize = true;

            Controls.Add(DeviceLayoutControlContainer);
            Controls.Add(TitlePanel);

            TitlePanel.MouseDoubleClick += TitlePanelDoubleClick;
            TitlePanel.MouseUp += TitlePanelMouseUp;
            TitlePanel.MouseDown += TitlePanelMouseDown;

            TitlePanel.Paint += TitlePanelPaint;
        }

        private void HideOrShowContainer(Object sender, EventArgs e)
        {
            _timer.Enabled = false;

            DeviceLayoutControlContainer.Visible = !DeviceLayoutControlContainer.Visible;
            TitlePanel.Invalidate();
        }

        private Rectangle _switchRectangle = new Rectangle(188, 0, 32, 32);
        private readonly Font _font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private static RectangleF _nameRectangleF = new RectangleF(22, 8, 165, 17);
        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (DeviceGroup == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceLayoutControlContainer.Visible ? _arrowUp : _arrowDown), 192, 13);

            g.DrawString(DeviceGroup.ToString(), _font, Brushes.White, _nameRectangleF);
        }

        private Point _position;

        private void TitlePanelMouseDown(Object sender, MouseEventArgs e)
        {
            _position = e.Location;

            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y))
                return;

            MouseMove -= TitlePanelMouseMove;
            MouseMove += TitlePanelMouseMove;
        }

        private void TitlePanelMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= TitlePanelMouseMove;

            // click on rectangle
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                DeviceLayoutControlContainer.Visible = !DeviceLayoutControlContainer.Visible;
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

            if (OnDeviceLayoutGroupMouseDown != null)
                OnDeviceLayoutGroupMouseDown(this, e);
        }

        private void TitlePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnDeviceLayoutGroupMouseDrag != null)
            {
                MouseMove -= TitlePanelMouseMove;
                OnDeviceLayoutGroupMouseDrag(this, e);
            }
        }

        private Boolean _isDblClick;
        private void TitlePanelDoubleClick(Object sender, MouseEventArgs e)
        {
            //dont drag if click on expand icon
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            _isDblClick = true;
            _timer.Enabled = false;

            if (OnDeviceLayoutGroupMouseDoubleClick != null)
                OnDeviceLayoutGroupMouseDoubleClick(this, e);
        }

        public void CreateTree()
        {
            var layoutSortResult = new List<IDeviceLayout>(NVR.Device.DeviceLayouts.Values);

            //reverse
            layoutSortResult.Sort((x, y) => (y.Id - x.Id));

            var layoutControls = new List<DeviceLayoutControlUI2>();
            foreach (IDeviceLayout deviceLayout in layoutSortResult)
            {
                //if (deviceLayout.SubLayouts.Count(subLayout => subLayout.Value != null) == 0) continue;
                if (deviceLayout.Items.Count(device => device != null) == 0) continue;

                var layoutControl = GetDeviceLayoutControl();

                layoutControl.DeviceLayout = deviceLayout;
                layoutControls.Add(layoutControl);

                layoutControl.CreateTree();
            }

            if (layoutControls.Count > 0)
            {
                DeviceLayoutControlContainer.Controls.AddRange(layoutControls.ToArray());
                layoutControls.Clear();
            }
        }

        protected Queue<DeviceLayoutControlUI2> RecycleLayout = new Queue<DeviceLayoutControlUI2>();
        public DeviceLayoutControlUI2 GetDeviceLayoutControl()
        {
            if (RecycleLayout.Count > 0)
            {
                return RecycleLayout.Dequeue();
            }

            var layoutControl = new DeviceLayoutControlUI2();

            layoutControl.OnDeviceLayoutMouseDrag += OnDeviceLayoutMouseDrag;
            layoutControl.OnDeviceLayoutMouseDown += OnDeviceLayoutMouseDown;
            layoutControl.OnDeviceLayoutMouseDoubleClick += OnDeviceLayoutMouseDoubleClick;

            layoutControl.OnSubLayoutMouseDrag += OnSubLayoutMouseDrag;
            layoutControl.OnSubLayoutMouseDown += OnSubLayoutMouseDown;
            layoutControl.OnSubLayoutMouseDoubleClick += OnSubLayoutMouseDoubleClick;

            return layoutControl;
        }

        public void Clear()
        {
            foreach (DeviceLayoutControlUI2 layoutControl in DeviceLayoutControlContainer.Controls)
            {
                if (!RecycleLayout.Contains(layoutControl))
                    RecycleLayout.Enqueue(layoutControl);

                layoutControl.Clear();
            }
        }

        public void UpdateToolTips()
        {
            foreach (DeviceLayoutControlUI2 layoutControl in DeviceLayoutControlContainer.Controls)
            {
                layoutControl.UpdateToolTips();
            }
        }

        public void UpdateRecordingStatus()
        {
            foreach (DeviceLayoutControlUI2 layoutControl in DeviceLayoutControlContainer.Controls)
            {
                layoutControl.UpdateRecordingStatus();
            }
        }
    }
}