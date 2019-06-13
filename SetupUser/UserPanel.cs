using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupUser
{
    public sealed class UserPanel : Panel
    {
        public event EventHandler OnUserEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;
        private readonly  CheckBox _checkBox = new CheckBox();

        public IServer Server { get; set; }
        public Boolean IsTitle;
        public IUser User;

        public UserPanel()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            Localization = new Dictionary<String, String>
                               {
                                   {"User_Name", "Name"},
                                   {"User_Email", "Email"},
                               };
            Localizations.Update(Localization);

            BackColor = Color.Transparent;
            
            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += UserPanelMouseClick;
            Paint += UserPanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(44, 13, 156, 17);

        private void UserPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, this);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            if(IsTitle)
            {
                if (Width <= 200) return;
                Manager.PaintText(g, Localization["User_Name"]);

                if (Width <= 350) return;
                g.DrawString(Localization["User_Email"], Manager.Font, Brushes.Black, 200, 13);
                return;
            }

            Manager.PaintStatus(g, User.ReadyState);

            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;
            g.DrawString(User.Credential.UserName, Manager.Font, fontBrush, _nameRectangleF);
            
            if (Width <= 350) return;
            g.DrawString(User.Email, Manager.Font, fontBrush, 200, 13);
        }

        private void UserPanelMouseClick(Object sender, MouseEventArgs e)
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
                if (OnUserEditClick != null)
                    OnUserEditClick(this, e);
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
                _checkBox.Checked = (value && _checkBox.Visible);
            }
        }

        /// <summary>
        /// Visibility of CheckBox 
        /// </summary>
        public Boolean SelectionVisible
        {
            get { return _checkBox.Visible; }
            set
            {
                if (value)
                {
                    if (User != null && (User.Id == 0 || User.Credential.UserName == Server.Credential.UserName || Server.User.Current.Id == User.Id))
                        _checkBox.Visible = false;
                    else
                        _checkBox.Visible = true;
                }
                else
                {
                    _checkBox.Visible = false;
                }
            }
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
