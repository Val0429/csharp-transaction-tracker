using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using Manager = SetupBase.Manager;

namespace SetupUser
{
    public partial class EditGroupPanel : UserControl
    {
        public IApp App;
        public IServer Server;
        public IUserGroup Group;
        public Dictionary<String, String> Localization;
        private readonly Dictionary<String, PermissionPanel> _permissionPanels = new Dictionary<String, PermissionPanel>();
        private readonly Dictionary<Permission, PermissionPanel> _functionPanels = new Dictionary<Permission, PermissionPanel>();

        public Boolean IsEditing;
        public Boolean PagePanelInitialized;

        public EditGroupPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Server", "Server"},
								   {"Control_NVR", "NVR"},
								   {"Control_Device", "Device"},
								   {"Control_DeviceGroup", "Group"},
								   {"Control_ImageStitching", "Image Stitching"},
								   {"Control_General", "General"},
								   {"Control_User", "User"},
								   {"Control_Event", "Event"},
								   {"Control_Schedule", "Schedule"},
								   {"Control_Joystick", "Joystick"},
								   {"Control_License", "License"},
								   {"Control_Log", "Log"},
								   {"Control_POS", "POS"},
								   {"Control_POSConnection", "POS Connection"},
								   {"Control_Exception", "Exception"},
								   {"Control_ExceptionReport", "Exception Report"},
								   {"Control_ScheduleReport", "Schedule Report"},

								   {"Control_PeopleCounting", "People Counting"},

								   {"EditGroupPanel_ApplicationPermission", "Application Permission"},
								   {"EditGroupPanel_SetupPermission", "Setup Permission"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            permissionLabel.Text = Localization["EditGroupPanel_ApplicationPermission"];
            functionLabel.Text = Localization["EditGroupPanel_SetupPermission"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
        }

        public void GeneratorPagePermissionList()
        {
            var pages = new List<IPage>();

            pages.AddRange(App.Pages.Values);

            foreach (IPage page in pages)
            {
                var panel = new PermissionPanel
                {
                    PermissionName = page.TitleName,
                };

                _permissionPanels.Add(page.Name, panel);

                permissionPanel.Controls.Add(panel);
                panel.BringToFront();
            }

            GeneratorSetupFunctionList();

            PagePanelInitialized = true;
        }

        protected virtual IEnumerable<Permission> GetSetupPermissions()
        {
            var permissions = new Permission[0];

            if (Server is ICMS)
            {
                permissions = new[] {Permission.Server, Permission.NVR, Permission.General, Permission.User, Permission.Event, Permission.Schedule,
					Permission.Joystick, Permission.License, Permission.Log};
            }
            else if (Server is IVAS)
            {
                permissions = new[] { Permission.Server, Permission.NVR, Permission.PeopleCounting, Permission.Schedule, Permission.User, Permission.License, Permission.Log };
            }
            else if (Server is IFOS)
            {
                permissions = new[] { Permission.Server, Permission.NVR, Permission.General, Permission.User, Permission.Event, Permission.License, Permission.Log };
            }
            else if (Server is INVR)
            {
                permissions = new[] {Permission.Server, Permission.Device, Permission.General, Permission.User,
					Permission.Event, Permission.Schedule, Permission.Joystick, Permission.License , Permission.Log};//Permission.DeviceGroup, Permission.ImageStitching,
            }
            else if (Server is IPTS)
            {
                permissions = new[] {Permission.Server, Permission.NVR, Permission.Exception, Permission.POS, Permission.POSConnection,
					Permission.General, Permission.User, Permission.ExceptionReport, Permission.ScheduleReport, Permission.License , Permission.Log};
            }

            return permissions;
        }

        private void GeneratorSetupFunctionList()
        {
            var permissions = GetSetupPermissions();

            foreach (Permission permission in permissions)
            {
                var key = "Control_" + permission;

                var panel = new PermissionPanel
                {
                    PermissionName = (Localization.ContainsKey(key) ? Localization[key] : permission.ToString()),
                };

                _functionPanels.Add(permission, panel);

                functionPanel.Controls.Add(panel);
                panel.BringToFront();
            }
        }

        public virtual void ParseGroup()
        {
            if (Group == null) return;

            IsEditing = false;

            foreach (KeyValuePair<String, PermissionPanel> obj in _permissionPanels)
            {
                obj.Value.HasPermission = Group.CheckPermission(obj.Key, Permission.Access);

                if (obj.Key == "Setup" && obj.Value.HasPermission)
                {
                    functionLabel.Visible = functionPanel.Visible = true;

                    foreach (KeyValuePair<Permission, PermissionPanel> panel in _functionPanels)
                    {
                        panel.Value.HasPermission = Group.CheckPermission(obj.Key, panel.Key);
                    }
                }
                else
                {
                    functionLabel.Visible = functionPanel.Visible = false;
                }
            }

            IsEditing = true;
            functionPanel.Focus();
        }
    }
}
