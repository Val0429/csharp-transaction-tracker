using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupPOS
{
    public sealed class POSPanel : Panel
    {
        public event EventHandler OnPOSEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();

        public String DataType = "Information";
        public Boolean IsTitle;
        public Boolean IsMiddleTitle;
        public IPOS POS;

        public POSPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"POS_ID", "ID"},
                                   {"POS_Name", "Name"},
                                   {"POS_Manufacture", "Manufacture"},
                                   {"POS_Model", "Model"},
                                   {"POS_RegisterId", "Register Id"},
                                   {"POS_Exception", "Exception"},
                                   {"POS_Keyword", "Keyword"},
                                   {"POS_ExceptionReport", "Exception Report"},
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

            MouseClick += POSPanelMouseClick;
            Paint += POSPanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(264, 13, 276, 17);
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 250) return;
            Manager.PaintTitleText(g, Localization["POS_RegisterId"]);

            g.DrawString(Localization["POS_Name"], Manager.Font, Manager.TitleTextColor, 264, 13);

            switch (DataType)
            {
                case "Information":
                    if (Width <= 510) return;
                    g.DrawString(Localization["POS_Manufacture"], Manager.Font, Manager.TitleTextColor, 390, 13);

                    //if (Width <= 400) return;
                    //g.DrawString(Localization["POS_Model"], Manager.Font, Manager.TitleTextColor, 330, 13);

                    //if (Width <= 400) return;
                    //g.DrawString(Localization["POS_RegisterId"], Manager.Font, Manager.TitleTextColor, 310, 13);

                    if (Width <= 590) return;
                    g.DrawString(Localization["POS_Exception"], Manager.Font, Manager.TitleTextColor, 500, 13);

                    //if (Width <= 650) return;
                    //g.DrawString(Localization["POS_Keyword"], Manager.Font, Manager.TitleTextColor,550, 13);
                    break;

                case "ExceptionReport":
                    if (Width <= 510) return;
                    g.DrawString(Localization["POS_ExceptionReport"], Manager.Font, Manager.TitleTextColor, 390, 13);
                    break;
            }
        }

        private void POSPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;
           
            if (IsTitle)
            {
                if (IsMiddleTitle)
                {
                    Manager.PaintTitleMiddleInput(g, this);
                }
                else
                {
                    Manager.PaintTitleTopInput(g, this);
                }
                
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, (Control)sender);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            Brush fontBrush = Brushes.Black;

            Manager.PaintStatus(g, POS.ReadyState);

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 250) return;
            Manager.PaintText(g, POS.Id.ToString());

            g.DrawString(POS.Name, Manager.Font, fontBrush, _nameRectangleF);

            switch (DataType)
            {
                case "Information":
                    PaintInformation(g, fontBrush);
                    break;

                case "ExceptionReport":
                    PaintExceptionReport(g, fontBrush);
                    break;
            }
        }

        private void PaintInformation(Graphics g, Brush fontBrush)
        {
            if (Width <= 510) return;
            g.DrawString(POS_Exception.ToDisplay(POS.Manufacture), Manager.Font, fontBrush, 390, 13);

            //if (Width <= 400) return;
            //g.DrawString(POS.Model, Manager.Font, Brushes.Black, 330, 13);

            //if (Width <= 400) return;
            //g.DrawString(POS.RegisterId.ToString(), Manager.Font, fontBrush, 330, 13);

            if (Width <= 590) return;
            if (POS.Exception == 0)
                g.DrawString("", Manager.Font, fontBrush, 500, 13);
            else
                g.DrawString(POS.Exception.ToString().PadLeft(2, '0'), Manager.Font, fontBrush, 500, 13);

            //if (Width <= 650) return;
            //g.DrawString(POS.Keyword.ToString(), Manager.Font, Brushes.Black, 550, 13);
        }

        private void PaintExceptionReport(Graphics g, Brush fontBrush)
        {
            var exceptions = new List<String>();

            var list = POS.ExceptionReports.Select(exceptionReport => exceptionReport.Exception).ToList();
            list.Sort();

            foreach (var exception in list)
            {
                if (exceptions.Count > 3)
                {
                    exceptions[2] += "...";
                    break;
                }
                if (!exceptions.Contains(POS_Exception.FindExceptionValueByKey(exception)))
                    exceptions.Add(POS_Exception.FindExceptionValueByKey(exception));
            }

            g.DrawString(String.Join(",", exceptions.ToArray()), Manager.Font, fontBrush, 390, 13);
        }

        private void POSPanelMouseClick(Object sender, MouseEventArgs e)
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
                if (OnPOSEditClick != null)
                    OnPOSEditClick(this, e);
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
                if (!Enabled && value) return;
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
