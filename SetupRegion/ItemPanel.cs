using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupRegion
{
    public sealed class ItemPanel : Panel
    {
        public event EventHandler OnItemEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        public Boolean IsMiddleTitle;

        public IRegion Region;

        public ItemPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"City_ID", "ID"},
                                   {"City_Name", "Name"},
                                   {"City_Stores", "Store(s)"}
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

            MouseClick += ItemPanelMouseClick;
            Paint += ItemPanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(164, 13, 276, 17);
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 250) return;
            Manager.PaintTitleText(g, Localization["City_ID"]);

            g.DrawString(Localization["City_Name"], Manager.Font, Manager.TitleTextColor, 164, 13);

            if (Width <= 410) return;
            g.DrawString(Localization["City_Stores"], Manager.Font, Manager.TitleTextColor, 350, 13);
        }

        private void ItemPanelPaint(Object sender, PaintEventArgs e)
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

            Manager.PaintStatus(g, Region.ReadyState);

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 250) return;
            Manager.PaintText(g, Region.Id.ToString());

            g.DrawString(Region.Name, Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 410) return;

            List<String> names = new List<string>();
            foreach (IStore store in Region.Stores.Values)
            {
                names.Add(store.ToString());

                if (names.Count >= 5) break;
            }

            var _name = String.Join(",", names.ToArray());

            if (!String.IsNullOrEmpty(_name))
                _name += "...";

            g.DrawString(_name, Manager.Font, fontBrush, 350, 13);
        }


        private void ItemPanelMouseClick(Object sender, MouseEventArgs e)
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
                if (OnItemEditClick != null)
                    OnItemEditClick(this, e);
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
