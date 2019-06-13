﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using SetupBase;

namespace SetupException
{
    public sealed class ExceptionPanel : Panel
    {
        //public event EventHandler OnExceptionEditClick;
        //public event EventHandler OnSelectAll;
        //public event EventHandler OnSelectNone;
        //public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        //private readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        private POS_Exception.Exception _exception;
        public POS_Exception.Exception Exception
        {
            set
            {
                _exception = value;

                if (_exception == null) return;

                _isEditing = false;
                _valueTextBox.Visible = true;
                _valueTextBox.Text = _exception.Value;
                _valueTextBox.Enabled = _exception.Editable;
                _isEditing = true;
            }
        }

        private readonly TextBox _valueTextBox;
        public ExceptionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Exception_Tag", "Tag"},
                                   {"Exception_Value", "Value"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            Width = 325;

            BackColor = Color.Transparent;

            _valueTextBox = new PanelBase.HotKeyTextBox
            {
                Visible = false,
                MaxLength = 50,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(59, 10),
                Size = new Size(250, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            //_checkBox.Padding = new Padding(10, 0, 0, 0);
            //_checkBox.Dock = DockStyle.Left;
            //_checkBox.Width = 25;

            //Controls.Add(_checkBox);
            Controls.Add(_valueTextBox);

            _valueTextBox.TextChanged += ValueTextBoxTextChanged;
            //_checkBox.CheckedChanged += CheckBoxCheckedChanged;

            //MouseClick += ExceptionControlMouseClick;
            Paint += ExceptionPanelPaint;
        }

        private Boolean _isEditing;
        private void ValueTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (_exception == null || !_isEditing) return;

            _exception.Value = _valueTextBox.Text;
        }

        private static RectangleF _nameRectangleF = new RectangleF(44, 13, 306, 17);
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 350) return;
            Manager.PaintTitleText(g, Localization["Exception_Tag"]);

            if (Width <= 450) return;
            g.DrawString(Localization["Exception_Value"], Manager.Font, Manager.TitleTextColor, Width - 196, 13);
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

            if (Width <= 350) return;

            g.DrawString(Name, Manager.Font, fontBrush, _nameRectangleF);

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

        //private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        //{
        //    Invalidate();

        //    if (IsTitle)
        //    {
        //        if (Checked && OnSelectAll != null)
        //            OnSelectAll(this, null);
        //        else if (!Checked && OnSelectNone != null)
        //            OnSelectNone(this, null);

        //        return;
        //    }

        //    _checkBox.Focus();
        //    if (OnSelectChange != null)
        //        OnSelectChange(this, null);
        //}

        public Brush SelectedColor = Manager.SelectedTextColor;

        //public Boolean Checked
        //{
        //    get
        //    {
        //        return _checkBox.Checked;
        //    }
        //    set
        //    {
        //        _checkBox.Checked = value;
        //    }
        //}

        //public Boolean SelectionVisible
        //{
        //    set { _checkBox.Visible = value; }
        //}

        //private Boolean _editVisible;
        //public Boolean EditVisible
        //{
        //    set
        //    {
        //        _editVisible = value;
        //        Invalidate();
        //    }
        //}
    }
}
