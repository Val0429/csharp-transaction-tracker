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
    public sealed class UserGroupPanel : Panel
    {
        public event EventHandler OnGroupEditClick;

        public IUserGroup Group;
        public Dictionary<String, String> Localization;

        public UserGroupPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"UserGroupPanel_NumUser", "(%1 User)"},
                                   {"UserGroupPanel_NumUsers", "(%1 Users)"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Size = new Size(300, 40);

            BackColor = Color.Transparent;

            MouseClick += UserGroupPanelMouseClick;
            Paint += UserGroupPanelPaint;
        }

        public Brush SelectedColor = Manager.DeleteTextColor;

        private static RectangleF _nameRectangleF = new RectangleF(44, 13, 236, 17);

        private void UserGroupPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, this, true);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            Manager.PaintStatus(g, Group.ReadyState);

            if (Width <= 300) return;

            Brush fontBrush = Brushes.Black;

            Int32 count = Group.Users.Count;
            if(count == 0)
            {
                g.DrawString(Group.TitleName, Manager.Font, fontBrush, _nameRectangleF);
            }
            else
            {
                var name = Group.TitleName + "   " + ((count == 1)
                        ? Localization["UserGroupPanel_NumUser"]
                        : Localization["UserGroupPanel_NumUsers"]).Replace("%1", count.ToString());
                
                g.DrawString(name, Manager.Font, fontBrush, _nameRectangleF);g.DrawString(name, Manager.Font, fontBrush, _nameRectangleF);
            }
        }

        private void UserGroupPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (_editVisible && OnGroupEditClick != null)
                OnGroupEditClick(this, e);

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
