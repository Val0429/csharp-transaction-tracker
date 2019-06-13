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
    public sealed class NVRPermissionPanel : Panel
    {
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;
        private readonly CheckBox _checkBox;

        public IApp App;
        public Boolean IsTitle;
        private Boolean _isEdit;
        private INVR _nvr;
        public INVR NVR
        {
            get { return _nvr; }
            set
            {
                _nvr = value;
                CheckPermission();
            }
        }

        public IUser User;

        public NVRPermissionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePermissionPanel_ID", "ID"},
                                   {"DevicePermissionPanel_Name", "Name"},
                                   {"DevicePermissionPanel_Domain", "Domain"},
                                   {"DevicePermissionPanel_Port", "Port"}
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _checkBox = new CheckBox
            {
                Padding = new Padding(10, 0, 0, 0),
                Dock = DockStyle.Left,
                Width = 25,
            };
            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += NVRPanelMouseClick;
            Paint += NVRPanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);

        private void NVRPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, this);

            if (IsTitle)
            {
                if (Width <= 200) return;
                Manager.PaintText(g, Localization["DevicePermissionPanel_ID"]);

                g.DrawString(Localization["DevicePermissionPanel_Name"], Manager.Font, Brushes.Black, 74, 13);

                g.DrawString(Localization["DevicePermissionPanel_Domain"], Manager.Font, Brushes.Black, 210, 13);

                g.DrawString(Localization["DevicePermissionPanel_Port"], Manager.Font, Brushes.Black, 350, 13);
            }
            
            if(_nvr == null) return;

            Manager.PaintStatus(g, _nvr.ReadyState);

            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;
            Manager.PaintText(g, _nvr.Id.ToString());
            g.DrawString(_nvr.Name, Manager.Font, fontBrush, _nameRectangleF);
            g.DrawString(_nvr.Credential.Domain, Manager.Font, fontBrush, 210, 13);
            g.DrawString(_nvr.Credential.Port.ToString(), Manager.Font, fontBrush, 350, 13);
        }

        private void NVRPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (_checkBox.Visible)
            {
                _checkBox.Checked = !_checkBox.Checked;
                return;
            }
        }
    
        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if(IsTitle)
            {
                if (_checkBox.Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!_checkBox.Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }

            if (!_isEdit || _nvr == null) return;

            //Add Device Permission
            if (_checkBox.Checked)
            {
                User.NVRPermissions.Add(_nvr, new List<Permission>{Permission.Access});
            }
            else//Remove Device Permission
            {
                User.NVRPermissions.Remove(_nvr);
            }

            CheckPermission();
            UserIsModify();

            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        private void CheckPermission()
        {
            if (_nvr != null)
            {
                _isEdit = false;

                _checkBox.Checked = User.CheckPermission(_nvr, Permission.Access);

                _isEdit = true;
                Invalidate();
            }
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
            set{ _checkBox.Visible = value; }
        }

        public void UserIsModify()
        {
            if (User.ReadyState == ReadyState.Ready)
                User.ReadyState = ReadyState.Modify;
        }
    }
}