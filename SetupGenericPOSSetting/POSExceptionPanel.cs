using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupGenericPOSSetting
{
    public sealed class POSExceptionPanel : Panel
    {
        public event EventHandler OnPOSExceptionEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        public POS_Exception POSException;

        public POSExceptionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"POSException_ID", "ID"},
                                   {"POSException_Name", "Name"},
                                   {"POSException_Manufacture", "Manufacture"},
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

            MouseClick += POSExceptionPanelMouseClick;
            Paint += POSExceptionPanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(44, 13, 156, 17);
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;
            Manager.PaintTitleText(g, Localization["POSException_ID"]);

            g.DrawString(Localization["POSException_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

            if (Width <= 320) return;
            g.DrawString(Localization["POSException_Manufacture"], Manager.Font, Manager.TitleTextColor, 200, 13);
        }

        private void POSExceptionPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;
           
            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, (Control)sender);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            Manager.PaintStatus(g, POSException.ReadyState);
            Brush fontBrush = Brushes.Black;

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }
            if (Width <= 200) return;
            g.DrawString(POSException.ToString(), Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 320) return;
            g.DrawString(POS_Exception.ToDisplay(POSException.Manufacture), Manager.Font, fontBrush, 200, 13);
        }

        private void POSExceptionPanelMouseClick(Object sender, MouseEventArgs e)
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
                if (OnPOSExceptionEditClick != null)
                    OnPOSExceptionEditClick(this, e);
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if (IsTitle)
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

        public Brush SelectedColor = Manager.SelectedTextColor;

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
            set { _checkBox.Visible = value; }
        }

        private Boolean _editVisible;
        public Boolean EditVisible
        {
            set
            {
                _editVisible = value;
                Invalidate();
            }
        }
    }
}
