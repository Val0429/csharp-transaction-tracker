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
    public sealed class NVRControl : Panel
    {
        public event MouseEventHandler OnNVRMouseDrag;
        public event MouseEventHandler OnNVRMouseDown;
        public event MouseEventHandler OnNVRMouseDoubleClick;

        public DoubleBufferPanel GroupControlContainer = new DoubleBufferPanel();
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
                        IDeviceGroup group = _nvr.Device.Groups.Values.First();
                        if (group != null)
                        {
                            String str = (group.Items.Count <= 1)
                                ? "GroupPanel_NumDevice"
                                : "GroupPanel_NumDevices";

                            SharedToolTips.SharedToolTip.SetToolTip(this, _nvr + Environment.NewLine + _nvr.Credential.Domain + Environment.NewLine +
                                Localization[str].Replace("%1", group.Items.Count.ToString()));
                        }
                        else
                            SharedToolTips.SharedToolTip.SetToolTip(this, "");
                    }
                }
                else
                    SharedToolTips.SharedToolTip.SetToolTip(this, "");
            }
        }
        private static readonly Dictionary<String, String> Localization;

        private static readonly Image _hideGroup = Resources.GetResources(Properties.Resources.nvr_state, Properties.Resources.IMGNvrState);
        private static readonly Image _showGroup = Resources.GetResources(Properties.Resources.nvr_state2, Properties.Resources.IMGNvrState2);


        static NVRControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"GroupPanel_NumDevice", "(%1 Device)"},
                                   {"GroupPanel_NumDevices", "(%1 Devices)"},
                                   {"GroupPanel_NumGroup", "(%1 Group)"},
                                   {"GroupPanel_NumGroups", "(%1 Groups)"},

                                   {"LoginForm_CMS", "CMS"},
                               };
            Localizations.Update(Localization);
        }

        public NVRControl()
        {
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(154, 154, 154);
            ForeColor = Color.Black;

            Padding = new Padding(0, 25, 0, 0);
            Cursor = Cursors.Hand;

            GroupControlContainer.BackColor = Color.FromArgb(120, 120, 120);
            GroupControlContainer.Dock = DockStyle.Fill;
            GroupControlContainer.AutoSize = true;

            Controls.Add(GroupControlContainer);

            MouseDoubleClick += NVRControlDoubleClick;
            MouseUp += NVRControlMouseUp;
            MouseDown += NVRControlMouseDown;

            Paint += NVRControlPaint;
        }

        private Rectangle _switchRectangle = new Rectangle(0, 0, 20, 25);
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private void NVRControlPaint(Object sender, PaintEventArgs e)
        {
            if (_nvr == null) return;

            Graphics g = e.Graphics;

            g.DrawImage((GroupControlContainer.Visible ? _showGroup : _hideGroup), 4, 6);

            if (_nvr is ICMS)
            {
                UInt16 count = 0;
                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in _nvr.Device.Groups)
                {
                    if (obj.Value.Items.Count > 0)
                        count++;
                }
                if(count > 0)
                {
                    String str = (count == 1)
                        ? "GroupPanel_NumGroup"
                        : "GroupPanel_NumGroups";

                    g.DrawString(Localization["LoginForm_CMS"] + "   " + Localization[str].Replace("%1", count.ToString()), _font, Brushes.Black, 25, 5);
                }
                else
                {
                    g.DrawString(Localization["LoginForm_CMS"], _font, Brushes.Black, 25, 5);
                }
            }
            else
            {
                String str = (_nvr.Device.Groups.Values.First().Items.Count <= 1)
                    ? "GroupPanel_NumDevice"
                    : "GroupPanel_NumDevices";

                g.DrawString(_nvr + "   " + Localization[str].Replace("%1", _nvr.Device.Groups.Values.First().Items.Count.ToString()), _font, Brushes.Black, 25, 5);
            }
        }

        private Point _position;

        private void NVRControlMouseDown(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y))
            {
                GroupControlContainer.Visible = !GroupControlContainer.Visible;
                Invalidate();
                return;
            }

            _position = e.Location;
            MouseMove -= NVRControlMouseMove;
            MouseMove += NVRControlMouseMove;
        }

        private void NVRControlMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= NVRControlMouseMove;
            //Focus();
            if (OnNVRMouseDown != null)
                OnNVRMouseDown(this, e);
        }

        private void NVRControlDoubleClick(Object sender, MouseEventArgs e)
        {
            if (_switchRectangle.Contains(e.X, e.Y)) return;

            if (OnNVRMouseDoubleClick != null)
                OnNVRMouseDoubleClick(this, e);
        }

        private void NVRControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnNVRMouseDrag != null)
            {
                MouseMove -= NVRControlMouseMove;
                OnNVRMouseDrag(this, e);
            }
        }

        public void UpdateRecordingStatus()
        {
            Invalidate();
            foreach (GroupControl groupControl in GroupControlContainer.Controls)
            {
                groupControl.UpdateRecordingStatus();
            }
        }
    }
}