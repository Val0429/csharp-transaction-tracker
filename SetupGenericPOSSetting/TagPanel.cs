using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using SetupBase;

namespace SetupGenericPOSSetting
{
    public sealed class TagPanel : Panel
    {
        //public event EventHandler OnExceptionEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        private POS_Exception.Tag _tag;
        public POS_Exception.Tag Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;

                if (_tag == null)
                {
                    _tagTextBox.Visible =
                    _valueTextBox.Visible =
                    _tagEndTextBox.Visible = false;
                    return;
                }

                _isEditing = false;
                _tagTextBox.Visible = true;
                _tagTextBox.Text = _tag.Key;
                _tagTextBox.Enabled = _tag.Editable;
                _valueTextBox.Visible = true;
                _valueTextBox.Text = _tag.Value;
                _valueTextBox.Enabled = _tag.Editable;
                _tagEndTextBox.Visible = true;
                _tagEndTextBox.Text = _tag.TagEnd;
                _tagEndTextBox.Enabled = _tag.Editable;
                _isEditing = true;
            }
        }
        private readonly TextBox _tagTextBox;
        private readonly TextBox _valueTextBox;
        private readonly TextBox _tagEndTextBox;
        private readonly TableLayoutPanel _table;
        public TagPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Exception_Tag", "Tag"},
                                   {"Exception_Value", "Value"},
                                   {"Exception_TagEnd", "Tag End"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            Width = 325;

            BackColor = Color.Transparent;

            _tagTextBox = new TextBox
            {
                Visible = false,
                MaxLength = 50,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(40, 10),
                Size = new Size(250, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Dock = DockStyle.Fill,
                
            };

            _valueTextBox = new TextBox
            {
                Visible = false,
                MaxLength = 50,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(-59, 10),
                Size = new Size(250, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Dock = DockStyle.Fill
            };

            _tagEndTextBox = new TextBox
            {
                Visible = false,
                MaxLength = 50,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(210, 10),
                Size = new Size(100, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Dock = DockStyle.Fill
            };
            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            _table = new TableLayoutPanel
            {
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                Name = "tableLayoutPanel1",
                TabIndex = 0,
                RowCount = 1,
                Location = new Point(0, 0),
                Padding= new Padding(7,7,7,7)
            };
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6F));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            _table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));


            _table.Controls.Add(_checkBox, 0, 0);
            _table.Controls.Add(_tagTextBox, 1, 0);
            _table.Controls.Add(_valueTextBox, 2, 0);
            _table.Controls.Add(_tagEndTextBox, 3, 0);
            Controls.Add(_table);

            //Controls.Add(_checkBox);
            //Controls.Add(_tagTextBox);
            //Controls.Add(_valueTextBox);
            //Controls.Add(_tagEndTextBox);

            _tagTextBox.TextChanged += TagTextBoxTextChanged;
            _valueTextBox.TextChanged += ValueTextBoxTextChanged;
            _tagEndTextBox.TextChanged += TagEndTextBoxTextChanged;
            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            //MouseClick += ExceptionControlMouseClick;
            Paint += ExceptionPanelPaint;
        }

        private Boolean _isEditing;

        private void TagTextBoxTextChanged(object sender, EventArgs e)
        {
            if (_tag == null || !_isEditing) return;

            _tag.Key = _tagTextBox.Text;
        }

        private void ValueTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (_tag == null || !_isEditing) return;

            _tag.Value = _valueTextBox.Text;
        }

        private void TagEndTextBoxTextChanged(object sender, EventArgs e)
        {
            if (_tag == null || !_isEditing) return;

            _tag.TagEnd = _tagEndTextBox.Text;
        }

        private static RectangleF _nameRectangleF = new RectangleF(44, 13, 306, 17);
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 350) return;
            Manager.PaintTitleText(g, Localization["Exception_Tag"]);

            if (Width <= 450) return;
            g.DrawString(Localization["Exception_Value"], Manager.Font, Manager.TitleTextColor, Width - 386, 13);

            if (Width <= 550) return;
            g.DrawString(Localization["Exception_TagEnd"], Manager.Font, Manager.TitleTextColor, Width - 116, 13);
        }

        private void ExceptionPanelPaint(Object sender, PaintEventArgs e)
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

            //if (_editVisible)
            //    Manager.PaintEdit(g, this);

            Brush fontBrush = Brushes.Black;

            //if (_checkBox.Visible && Checked)
            //{
            //    fontBrush = SelectedColor;
            //}

            //if (Width <= 350) return;

            //g.DrawString(Name, Manager.Font, fontBrush, _nameRectangleF);

            //if (Width <= 320) return;
            //g.DrawString(_exception.Value, Manager.Font, fontBrush, 200, 13);
        }

        //private void ExceptionControlMouseClick(Object sender, MouseEventArgs e)
        //{
        //    if (IsTitle)
        //    {
        //        if (_checkBox.Visible)
        //        {
        //            _checkBox.Checked = !_checkBox.Checked;
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        if (_checkBox.Visible)
        //        {
        //            _checkBox.Checked = !_checkBox.Checked;
        //            return;
        //        }
        //        if (OnExceptionEditClick != null)
        //            OnExceptionEditClick(this, e);
        //    }
        //}

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
