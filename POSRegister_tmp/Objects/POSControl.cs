using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;
using PanelBase;

namespace POSRegister.Objects
{
    public sealed class POSControl : Panel
    {
        public event MouseEventHandler OnPOSMouseDrag;
        public event MouseEventHandler OnPOSMouseDown;
        public event MouseEventHandler OnPOSMouseDoubleClick;

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

        protected static readonly Image _hideDevice = Resources.GetResources(Properties.Resources.group_state, Properties.Resources.IMGGroupState);
        protected static readonly Image _showDevice = Resources.GetResources(Properties.Resources.group_state2, Properties.Resources.IMGGroupState2);

        public POSControl()
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
            BackColor = Color.FromArgb(154, 154, 154);
            ForeColor = Color.Black;
            Padding = new Padding(0, 25, 0, 0);
            Cursor = Cursors.Hand;

            DeviceControlContainer.BackColor = Color.FromArgb(120, 120, 120);
            DeviceControlContainer.Dock = DockStyle.Fill;
            DeviceControlContainer.AutoSize = true;

            Controls.Add(DeviceControlContainer);

            MouseDoubleClick += POSControlDoubleClick;
            MouseUp += POSControlMouseUp;
            MouseDown += POSControlMouseDown;

            Paint += POSControlPaint;
        }

        private Rectangle _switchRectangle = new Rectangle(0, 0, 20, 25);
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private void POSControlPaint(Object sender, PaintEventArgs e)
        {
            if (_pos == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((DeviceControlContainer.Visible ? _showDevice : _hideDevice), 4, 6);

            String str = (_pos.Items.Count <= 1)
                ? "GroupPanel_NumDevice"
                : "GroupPanel_NumDevices";

            g.DrawString(_pos + "   " + Localization[str].Replace("%1", _pos.Items.Count.ToString()), _font, Brushes.Black, 25, 5);
        }

        private Int32 _positionX;
        private void POSControlMouseDown(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                DeviceControlContainer.Visible = !DeviceControlContainer.Visible;
                Invalidate();
                return;
            }

            _positionX = e.X;
            MouseMove -= POSControlMouseMove;
            MouseMove += POSControlMouseMove;
        }

        private void POSControlMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= POSControlMouseMove;
            //Focus();
            if (OnPOSMouseDown != null)
                OnPOSMouseDown(this, e);
        }

        private void POSControlDoubleClick(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            if (OnPOSMouseDoubleClick != null)
                OnPOSMouseDoubleClick(this, e);
        }

        private void POSControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _positionX) return;
            if (OnPOSMouseDrag != null)
            {
                MouseMove -= POSControlMouseMove;
                OnPOSMouseDrag(this, e);
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