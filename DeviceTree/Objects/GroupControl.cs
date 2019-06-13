using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
    public class GroupControl : Panel
    {
        public event MouseEventHandler OnGroupMouseDrag;
        public event MouseEventHandler OnGroupMouseDown;
        public event MouseEventHandler OnGroupMouseDoubleClick;

        public DoubleBufferPanel DeviceControlContainer = new DoubleBufferPanel();
        protected IDeviceGroup _deviceGroup;
        public virtual IDeviceGroup DeviceGroup
        {
            get { return _deviceGroup; }
            set
            {
                _deviceGroup = value;

                if (_deviceGroup != null)
                {
                    if (_deviceGroup.Items.Count <= 1)
                        SharedToolTips.SharedToolTip.SetToolTip(this, _deviceGroup + Environment.NewLine +
                        Localization["GroupPanel_NumDevice"].Replace("%1", _deviceGroup.Items.Count.ToString()));
                    else
                        SharedToolTips.SharedToolTip.SetToolTip(this, _deviceGroup + Environment.NewLine +
                        Localization["GroupPanel_NumDevices"].Replace("%1", _deviceGroup.Items.Count.ToString()));
                }
                else
                    SharedToolTips.SharedToolTip.SetToolTip(this, "");
            }
        }


        private static readonly Dictionary<String, String> Localization;

        protected Image _plusIcon;
        protected Image _minusIcon;

        static GroupControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"GroupPanel_NumDevice", "(%1 Device)"},
                                   {"GroupPanel_NumDevices", "(%1 Devices)"},
                               };
            Localizations.Update(Localization);
        }

        public GroupControl()
        {
            _plusIcon = Resources.GetResources(Properties.Resources.plus, Properties.Resources.IMGPlus);
            _minusIcon = Resources.GetResources(Properties.Resources.minus, Properties.Resources.IMGMinus);

            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(120, 120, 120);
            ForeColor = Color.Black;

            Padding = new Padding(0, 25, 0, 0);
            Cursor = Cursors.Hand;

            DeviceControlContainer.BackColor = Color.FromArgb(98, 98, 98);
            DeviceControlContainer.Dock = DockStyle.Fill;
            DeviceControlContainer.AutoSize = true;
            DeviceControlContainer.Padding = new Padding(0, 0, 0, 10);
            Controls.Add(DeviceControlContainer);

            MouseDoubleClick += GroupControlDoubleClick;
            MouseUp += GroupControlMouseUp;
            MouseDown += GroupControlMouseDown;

            Paint += GroupControlPaint;
        }

        public void ChangeStatusIcon(Image plus, Image minus)
        {
            _plusIcon = plus;
            _minusIcon = minus;
        }

        private Rectangle _switchRectangle = new Rectangle(0, 0, 20, 25);
        protected readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private Int32 _paintShift = 0;
        public Int32 PaintShift
        {
            get { return _paintShift; }
            set
            {
                _paintShift = value;
                _switchRectangle = new Rectangle(0 + _paintShift, 0, 20, 25);
            }
        }
        protected virtual void GroupControlPaint(Object sender, PaintEventArgs e)
        {
            if (_deviceGroup == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _minusIcon : _plusIcon), 4 + PaintShift, 3);

            Int32 count = _deviceGroup.Items.Count;

            var name = _deviceGroup + "   " +
                       ((count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
                           Replace("%1", count.ToString());

            g.DrawString(name, _font, Brushes.Black, 25 + PaintShift, 5);
        }

        private Point _position;

        private void GroupControlMouseDown(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                DeviceControlContainer.Visible = !DeviceControlContainer.Visible;
                Invalidate();
                return;
            }

            _position = e.Location;
            MouseMove -= GroupControlMouseMove;
            MouseMove += GroupControlMouseMove;
        }

        private void GroupControlMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= GroupControlMouseMove;
            //Focus();
            if (OnGroupMouseDown != null)
                OnGroupMouseDown(this, e);
        }

        private void GroupControlDoubleClick(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            if (OnGroupMouseDoubleClick != null)
                OnGroupMouseDoubleClick(this, e);
        }

        private void GroupControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnGroupMouseDrag != null)
            {
                MouseMove -= GroupControlMouseMove;
                OnGroupMouseDrag(this, e);
            }
        }

        public void UpdateRecordingStatus()
        {
            foreach (DeviceControl deviceControl in DeviceControlContainer.Controls)
            {
                if (!((deviceControl).Device is ICamera)) continue;

                (deviceControl).Invalidate();
            }
        }
    }
}