using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Interface;
using SetupBase;

namespace SetupUser
{
    public sealed class UserGroupUserPanel : Panel
    {
        public event EventHandler OnUserGroupEditClick;
        public event EventHandler OnUserEditClick;

        public IServer Server { get; set; }
        private readonly Queue<UserPanel> _recycleUser = new Queue<UserPanel>();
        private readonly UserGroupPanel _groupPanel;

        public IUser User;
        public IUserGroup Group{
            get
            {
                return GroupPanel.Group;
            }
            set
            {
                GroupPanel.Group = value;
            }
        }

        public UserGroupUserPanel()
        {
            Dock = DockStyle.Top;
            Padding = new Padding(0, 0, 0, 15);
            DoubleBuffered = true;
            AutoSize = true;
            MinimumSize = new Size(0, 15);
            _groupPanel = new UserGroupPanel();
            GroupPanel.OnGroupEditClick += EditButtonMouseClick;
        }

        public void ShowUsers()
        {
            _isEditing = false;
            if (Group.Users.Count > 0)
            {
                Group.Users.Sort((x, y) => (y.Id - x.Id));
                foreach (IUser user in Group.Users)
                {
                    if (user == null) continue;

                    UserPanel userPanel = GetUserControl();
                    userPanel.User = user;

                    Controls.Add(userPanel);
                }

                UserPanel userTitlePanel = GetUserControl();
                userTitlePanel.IsTitle = true;
                userTitlePanel.Cursor = Cursors.Default;
                userTitlePanel.EditVisible = false;
                userTitlePanel.OnSelectAll += UserPanelOnSelectAll;
                userTitlePanel.OnSelectNone += UserPanelOnSelectNone;
                Controls.Add(userTitlePanel);
            }

            GroupPanel.Cursor = Cursors.Hand;
            Controls.Add(GroupPanel);
            _isEditing = true;
        }

        private UserPanel GetUserControl()
        {
            UserPanel userPanel;
            if (_recycleUser.Count > 0)
            {
                userPanel = _recycleUser.Dequeue();
            }
            else
            {
                userPanel = new UserPanel
                {
                    Server = Server,
                    EditVisible = true,
                    SelectionVisible = false,
                };

                userPanel.OnUserEditClick += UserControlOnUserEdit;
                userPanel.OnSelectChange += UserControlOnSelectChange;
            }

            userPanel.Cursor = Cursors.Hand;
            return userPanel;
        }

        public List<IUser> UserSelection
        {
            get
            {
                var users = new List<IUser>();

                foreach (Control control in Controls)
                {
                    if (control is UserGroupPanel) continue;

                    if (!((UserPanel)control).Checked || ((UserPanel)control).User == null) continue;

                    users.Add(((UserPanel)control).User);
                }

                return users;
            }
        }

        private Boolean _isEditing;

        private void UserControlOnUserEdit(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            if (OnUserEditClick != null)
            {
                User = ((UserPanel) sender).User;
                OnUserEditClick(this, null);
            }
        }

        private void UserControlOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            var panel = sender as UserPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (Control control in Controls)
                {
                    var userPanel = control as UserPanel;
                    if (userPanel == null) continue;

                    if (userPanel.IsTitle) continue;
                    if (!userPanel.Checked)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = Controls[Controls.Count - 2] as UserPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= UserPanelOnSelectAll;
                title.OnSelectNone -= UserPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += UserPanelOnSelectAll;
                title.OnSelectNone += UserPanelOnSelectNone;
            }
        }

        public Boolean EditVisible
        {
            set
            {
                GroupPanel.EditVisible = value;
            }
        }

        public UserGroupPanel GroupPanel
        {
            get { return _groupPanel; }
        }

        public void ShowGroup()
        {
            _isEditing = true;
            foreach (Control control in Controls)
            {
                var userPanel = control as UserPanel;
                if (userPanel == null) continue;

                //if (userPanel.IsTitle) continue;

                userPanel.SelectionVisible = true;
                userPanel.Checked = false;
                userPanel.EditVisible = false;
                userPanel.Cursor = (userPanel.SelectionVisible) ? Cursors.Hand : Cursors.Default;
            }
            GroupPanel.EditVisible = false;
        }

        private void EditButtonMouseClick(Object sender, EventArgs e)
        {
            if(Group == null )return;

            if (OnUserGroupEditClick != null)
                OnUserGroupEditClick(this, e);
        }

        public void ClearViewModel()
        {
            _isEditing = false;
            GroupPanel.Group = null;
            foreach (Control control in Controls)
            {
                if (control is UserPanel)
                {
                    var userPanel = control as UserPanel;

                    userPanel.SelectionVisible = false;
                    userPanel.Checked = false;
                    userPanel.User = null;
                    userPanel.EditVisible = true;

                    if (userPanel.IsTitle)
                    {
                        userPanel.OnSelectAll -= UserPanelOnSelectAll;
                        userPanel.OnSelectNone -= UserPanelOnSelectNone;
                        userPanel.IsTitle = false;
                    }

                    if (!_recycleUser.Contains(userPanel))
                    {
                        _recycleUser.Enqueue(userPanel);
                    }
                }
                else if (control is UserGroupPanel)
                {
                    ((UserGroupPanel)control).Group = null;
                }
            }

            Controls.Clear();
        }

        private void UserPanelOnSelectAll(Object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is UserGroupPanel) continue;

                ((UserPanel)control).Checked = true;
            }
        }

        private void UserPanelOnSelectNone(Object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is UserGroupPanel) continue;

                ((UserPanel)control).Checked = false;
            }
        }
    }
}
