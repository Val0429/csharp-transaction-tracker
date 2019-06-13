using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace GPSTree.Objects
{
    public sealed class GroupControl : Panel
    {
        public event MouseEventHandler OnGroupMouseDrag;
        public event MouseEventHandler OnGroupMouseDoubleClick;

        public DoubleBufferPanel DeviceControlContainer = new DoubleBufferPanel();
        private IDeviceGroup _deviceGroup;
        public IDeviceGroup DeviceGroup{
            get { return _deviceGroup; }
            set
            {
                _deviceGroup = value;

                if (_deviceGroup != null)
                {
                    if (_deviceGroup.Items.Count <= 1)
                        _toolTip.SetToolTip(this, _deviceGroup.Name + Environment.NewLine +
                        Localization["GroupPanel_NumDevice"].Replace("{NUM}", _deviceGroup.Items.Count.ToString()));
                    else
                        _toolTip.SetToolTip(this, _deviceGroup.Name + Environment.NewLine +
                        Localization["GroupPanel_NumDevices"].Replace("{NUM}", _deviceGroup.Items.Count.ToString()));
                }
                else
                    _toolTip.SetToolTip(this, "");
            }
        }
        public Dictionary<String, String> Localization;

        private readonly ToolTip _toolTip = new ToolTip();

        public GroupControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"GroupPanel_NumDevice", "({NUM} Device)"},
                                   {"GroupPanel_NumDevices", "({NUM} Devices)"},
                               };
            Localizations.Update(Localization);

            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;
            BackColor = ColorTranslator.FromHtml("#787878"); //Color.DimGray;
            ForeColor = Color.Black;
            Padding = new Padding(0, 25, 0, 0);
            Cursor = Cursors.Hand;

            DeviceControlContainer.BackColor = ColorTranslator.FromHtml("#626262");
            DeviceControlContainer.Dock = DockStyle.Fill;
            DeviceControlContainer.AutoSize = true;

            Controls.Add(DeviceControlContainer);

            MouseDoubleClick += GroupControlDoubleClick;
            MouseUp += GroupControlMouseUp;
            MouseDown += GroupControlMouseDown;

            Paint += GroupControlPaint;
        }

        private Rectangle _switchRectangle = new Rectangle(0, 0, 20, 25);
        private readonly Image _hideDevice = Properties.Resources.group_state;
        private readonly Image _showDevice = Properties.Resources.group_state2;
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private void GroupControlPaint(Object sender, PaintEventArgs e)
        {
            if (_deviceGroup == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _showDevice : _hideDevice), 4, 9);

            Int32 count = _deviceGroup.Items.Count;

            if (count <= 1)
                g.DrawString(_deviceGroup.Name + "   " + Localization["GroupPanel_NumDevice"].Replace("{NUM}", count.ToString()), _font, Brushes.Black, 25, 5);
            else
                g.DrawString(_deviceGroup.Name + "   " + Localization["GroupPanel_NumDevices"].Replace("{NUM}", count.ToString()), _font, Brushes.Black, 25, 5);
        }

        private Int32 _positionX;
        private void GroupControlMouseDown(Object sender, MouseEventArgs e)
        {
            if(_switchRectangle.Contains(e.X, e.Y))
            {
                DeviceControlContainer.Visible = !DeviceControlContainer.Visible;
                Invalidate();
                return;
            }

            _positionX = e.X;
            MouseMove -= GroupControlMouseMove;
            MouseMove += GroupControlMouseMove;
        }

        private void GroupControlMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= GroupControlMouseMove;
        }

        private void GroupControlDoubleClick(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            if (OnGroupMouseDoubleClick != null)
                OnGroupMouseDoubleClick(this, e);
        }

        private void GroupControlMouseMove(Object sender, MouseEventArgs e)
        {
            if(e.X == _positionX) return;
            if (OnGroupMouseDrag != null)
            {
                MouseMove -= GroupControlMouseMove;
                OnGroupMouseDrag(this, e);
            }
        }

        public void UpdateRecordingStatus()
        {
            foreach (GPSControl deviceControl in DeviceControlContainer.Controls)
            {
                if (!((deviceControl).Device is ICamera)) continue;

                (deviceControl).Invalidate();
            }
        }
    }
}