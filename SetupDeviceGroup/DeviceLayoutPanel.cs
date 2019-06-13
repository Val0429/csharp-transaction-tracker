using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDeviceGroup
{
    public sealed class DeviceLayoutPanel : Panel
    {
        public event EventHandler OnDeviceLayoutEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;
        //public event EventHandler<EventArgs<String>> OnSortChange;

        private readonly  CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        public IDeviceLayout DeviceLayout;

        public DeviceLayoutPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_ID", "ID"},
                                   {"DeviceLayoutPanel_Name", "Name"},
                                   {"DeviceLayoutPanel_Width", "Width"},
                                   {"DeviceLayoutPanel_Height", "Height"},
                                   {"DeviceLayoutPanel_Layout", "Layout"},
                                   {"DeviceLayoutPanel_Device", "Device"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;
            
            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += DeviceLayoutPanelMouseClick;
            Paint += DevicePanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);

        private void DevicePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            
            Graphics g = e.Graphics;

            if(IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, this);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            if (DeviceLayout == null) return;

            Manager.PaintStatus(g, DeviceLayout.ReadyState);
            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;

            Manager.PaintText(g, DeviceLayout.Id.ToString().PadLeft(2, '0'));
            g.DrawString(DeviceLayout.Name, Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 300) return;
            g.DrawString(DeviceLayout.Width.ToString(), Manager.Font, Brushes.Black, 200, 13);

            if (Width <= 400) return;
            g.DrawString(DeviceLayout.Height.ToString(), Manager.Font, Brushes.Black, 300, 13);

            if (Width <= 500) return;
            g.DrawString(DeviceLayout.LayoutX + " x " + DeviceLayout.LayoutY, Manager.Font, Brushes.Black, 400, 13);

            if (Width <= 600) return;
            var deviceRectangleF = new RectangleF(500, 13, Width - 530, 15);
            g.DrawString(String.Join(",", DeviceLayout.Items.Where(device => device != null).Select(device => device.ToString()).ToArray()), Manager.Font, Brushes.Black, deviceRectangleF);
        }


        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;

            Manager.PaintTitleText(g, Localization["DeviceLayoutPanel_ID"]);

            g.DrawString(Localization["DeviceLayoutPanel_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

            if (Width <= 300) return;
            g.DrawString(Localization["DeviceLayoutPanel_Width"], Manager.Font, Manager.TitleTextColor, 200, 13);

            if (Width <= 400) return;
            g.DrawString(Localization["DeviceLayoutPanel_Height"], Manager.Font, Manager.TitleTextColor, 300, 13);

            if (Width <= 500) return;
            g.DrawString(Localization["DeviceLayoutPanel_Layout"], Manager.Font, Manager.TitleTextColor, 400, 13);

            g.DrawString(Localization["DeviceLayoutPanel_Device"], Manager.Font, Manager.TitleTextColor, 500, 13);
        }

        private void DeviceLayoutPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle)
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
            }
            else
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
                if (OnDeviceLayoutEditClick != null)
                    OnDeviceLayoutEditClick(this, e);
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if(IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }
            
            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        public Brush SelectedColor = Manager.DeleteTextColor;

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            set{ _checkBox.Visible = value; }
        }

        private Boolean _editVisible;
        public Boolean EditVisible { 
            set
            {
                _editVisible = value;
                Invalidate();
            }
        }
    }
}
