using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupUser
{
    public partial class Setup : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public String TitleName { get; set; }
        public IApp App { get; set; }

        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        private ListPanel _listPanel;
        private EditUserPanel _editUserPanel;
        protected EditGroupPanel _editGroupPanel;

        public Setup()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_User", "User"},

								   {"MessageBox_Error", "Error"},
								   
								   {"SetupUser_DeleteDevice", "Delete User"},
								   {"SetupUser_IdCantBeZero", "User Id can't be 0"},
							   };
            Localizations.Update(Localization);

            Name = "User";
            TitleName = Localization["Control_User"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_User"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            _listPanel = new ListPanel
            {
                Server = Server,
            };
            _listPanel.Initialize();

            _editUserPanel = CreateEditUserPanel();
            _editUserPanel.Initialize();

            if (_editGroupPanel == null)
            {
                _editGroupPanel = CreateEditGroupPanel();
                _editGroupPanel.Initialize();
            }

            _listPanel.OnUserGroupEdit += ListPanelOnUserGroupEdit;
            _listPanel.OnUserEdit += ListPanelOnUserEdit;
            _listPanel.OnUserAdd += ListPanelOnUserAdd;

            Server.OnLoadComplete += ServerOnLoadComplete;

            contentPanel.Controls.Add(_listPanel);
        }

        protected virtual EditUserPanel CreateEditUserPanel()
        {
            return new EditUserPanel
            {
                App = App,
                Server = Server,
            };
        }

        protected virtual EditGroupPanel CreateEditGroupPanel()
        {
            return new EditGroupPanel
            {
                App = App,
                Server = Server,
            };
        }

        private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);
        private void ServerOnLoadComplete(object sender, EventArgs<string> e)
        {
            if (Parent != null && Parent.Visible)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new RefreshContentDelegate(ServerOnLoadComplete), sender, e);
                        return;
                    }
                }
                catch (Exception)
                {
                }

                if (_focusControl == null) _focusControl = _listPanel;
                Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowUserGroupList);
            }
        }

        private Control _focusControl;

        private void ShowUserGroupList()
        {
            _listPanel.Enabled = true;
            if (!contentPanel.Controls.Contains(_listPanel))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(_listPanel);
            }

            _listPanel.GenerateViewModel();

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
                    "", (Server.User.Users.Count > 1) ? "Delete" : "")));
        }

        public void Activate()
        {
            if (!_editGroupPanel.PagePanelInitialized)
                _editGroupPanel.GeneratorPagePermissionList();
        }

        public void Deactivate()
        {
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            ShowUserGroupList();
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "Confirm":
                    _listPanel.RemoveSelectedUsers();

                    ShowUserGroupList();
                    break;

                case "Delete":
                    DeleteUser();
                    break;

                default:
                    if (item == TitleName || item == "Back")
                    {
                        Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowUserGroupList);
                    }
                    break;
            }
        }

        private void ListPanelOnUserGroupEdit(Object sender, EventArgs<IUserGroup> e)
        {
            if (e.Value == null) return;

            _focusControl = _editGroupPanel;

            _editGroupPanel.Group = e.Value;
            _editGroupPanel.ParseGroup();

            Manager.ReplaceControl(_listPanel, _focusControl, contentPanel, ManagerMoveToGroupEditComplete);
        }

        private void ListPanelOnUserEdit(Object sender, EventArgs<IUser> e)
        {
            if (e.Value == null) return;

            _focusControl = _editUserPanel;

            _editUserPanel.User = e.Value;
            _editUserPanel.ParseUser();

            Manager.ReplaceControl(_listPanel, _focusControl, contentPanel, ManagerMoveToUserEditComplete);
        }

        private void ListPanelOnUserAdd(Object sender, EventArgs e)
        {
            _focusControl = _editUserPanel;

            IUser user = new User
            {
                Id = Server.User.GetNewUserId(),
            };

            if (user.Id == 0)
            {
                TopMostMessageBox.Show(Localization["SetupUser_IdCantBeZero"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var userName = @"user" + user.Id;

            while (Server.User.Users.Any(obj => (obj.Value.Credential.UserName == userName)))
            {
                userName += "1";
            }

            user.Credential.UserName = userName;
            //user.Credential.Password = "123456";

            Server.WriteOperationLog("Add New User %1".Replace("%1", user.Id.ToString() + " " + user.Credential.UserName));

            //VAS , FOS default userfroup is "superuser"
            if (Server is IVAS || Server is IFOS)
            {
                foreach (KeyValuePair<UInt16, IUserGroup> obj in Server.User.Groups)
                {
                    if (obj.Value.Name == "Superuser" && obj.Value.Id == 1)
                    {
                        obj.Value.AddUser(user);
                        break;
                    }
                }
            }
            else if (Server is ICMS)//CMS default userfroup is "guest"
            {
                foreach (KeyValuePair<UInt16, IUserGroup> obj in Server.User.Groups)
                {
                    if (obj.Value.Name == "Guest" && obj.Value.Id == 3)
                    {
                        obj.Value.AddUser(user);
                        break;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<UInt16, IUserGroup> obj in Server.User.Groups)
                {
                    if (obj.Value.Name == "User" && obj.Value.Id == 2)
                    {
                        obj.Value.AddUser(user);
                        break;
                    }
                }
            }

            if (!Server.User.Users.ContainsKey(user.Id))
                Server.User.Users.Add(user.Id, user);

            _editUserPanel.User = user;
            _editUserPanel.ParseUser();

            Manager.ReplaceControl(_listPanel, _focusControl, contentPanel, ManagerMoveToUserEditComplete);
        }

        private void ManagerMoveToGroupEditComplete()
        {
            var text = TitleName + "  /  " + _editGroupPanel.Group.TitleName;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
        }


        private void ManagerMoveToUserEditComplete()
        {
            var text = TitleName + "  /  " + _editUserPanel.User.Credential.UserName;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
        }

        private void DeleteUser()
        {
            _listPanel.ShowGroup();
            var text = TitleName + "  /  " + Localization["SetupUser_DeleteDevice"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowUserGroupList();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }
}
