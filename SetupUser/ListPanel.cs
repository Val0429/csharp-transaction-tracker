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
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IUserGroup>> OnUserGroupEdit;
        public event EventHandler<EventArgs<IUser>> OnUserEdit;
        public event EventHandler OnUserAdd;

        public IServer Server;
        public Dictionary<String, String> Localization;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupUser_AddNewUser", "Add new user..."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupUser_AddNewUser"]);
        }

        private readonly Queue<UserGroupUserPanel> _recycleGroup = new Queue<UserGroupUserPanel>();

        public void GenerateViewModel()
        {
            ClearViewModel();
            label1.Visible = addNewDoubleBufferPanel.Visible = true;

            if (Server == null) return;

            List<IUserGroup> sortResult = new List<IUserGroup>(Server.User.Groups.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            containerPanel.Visible = false;
            foreach (IUserGroup group in sortResult)
            {
                if (group == null) continue;
                //VAS , FOS has no guest and user
                if (Server is IVAS || Server is IFOS)
                {
                    if (group.Name == "Guest" || group.Name == "User") continue;
                }

                UserGroupUserPanel userGroupPanel = GetUserGroupUserControl();

                userGroupPanel.Group = group;
                userGroupPanel.EditVisible = true;

                userGroupPanel.ShowUsers();
                containerPanel.Controls.Add(userGroupPanel);
            }
            containerPanel.Visible = true;
            containerPanel.Focus();
        }

        private UserGroupUserPanel GetUserGroupUserControl()
        {
            if (_recycleGroup.Count > 0)
            {
                return _recycleGroup.Dequeue();
            }

            UserGroupUserPanel userGroupUserPanel = new UserGroupUserPanel
            {
                Server = Server,
                EditVisible = true,
            };
            userGroupUserPanel.OnUserGroupEditClick += UserGroupControlOnUserGroupEditClick;
            userGroupUserPanel.OnUserEditClick += UserGroupControlOnUserEditClick;

            return userGroupUserPanel;
        }

        public void RemoveSelectedUsers()
        {
            var rusers = new List<String>();

            foreach (UserGroupUserPanel userGroupUserPanel in containerPanel.Controls)
            {
                List<IUser> users = userGroupUserPanel.UserSelection;
                foreach (IUser user in users)
                {
                    userGroupUserPanel.Group.Users.Remove(user);
                    Server.User.Users.Remove(user.Id);

                    rusers.Add(user.Id.ToString());
                }
            }

            Server.WriteOperationLog("Remove User %1".Replace("%1", String.Join(",", rusers.ToArray())));
        }

        private void UserGroupControlOnUserGroupEditClick(Object sender, EventArgs e)
        {
            if (OnUserGroupEdit != null)
                OnUserGroupEdit(this, new EventArgs<IUserGroup>(((UserGroupUserPanel)sender).Group));
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, EventArgs e)
        {
            if (OnUserAdd != null)
                OnUserAdd(this, e);
        }

        private void UserGroupControlOnUserEditClick(Object sender, EventArgs e)
        {
            if (OnUserEdit != null)
                OnUserEdit(this, new EventArgs<IUser>(((UserGroupUserPanel)sender).User));
        }

        public void ShowGroup()
        {
            label1.Visible = addNewDoubleBufferPanel.Visible = false;

            foreach (UserGroupUserPanel userGroupUserPanel in containerPanel.Controls)
            {
                userGroupUserPanel.ShowGroup();
            }
        }

        private void ClearViewModel()
        {
            foreach (UserGroupUserPanel userGroupUserPanel in containerPanel.Controls)
            {
                userGroupUserPanel.ClearViewModel();
                userGroupUserPanel.Group = null;
                if (!_recycleGroup.Contains(userGroupUserPanel))
                    _recycleGroup.Enqueue(userGroupUserPanel);
            }

            containerPanel.Controls.Clear();
        }
    } 
}
