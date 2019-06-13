using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using Constant;
using Constant.Utility;
using Device;
using Interface;

namespace ServerProfile
{
    public class UserManager : IUserManager
    {
        protected const String CgiLoadUser = @"cgi-bin/usergroup?action=load";
        private const String CgiLoadPagePermission = @"cgi-bin/usergroup?action=loadpagepermission";
        private const String CgiSaveUser = @"cgi-bin/usergroup?action=save";
        private const String CgiLoadPermission = @"cgi-bin/permission?action=load";
        private const String CgiSavePermission = @"cgi-bin/permission?action=save";

        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;

        public ManagerReadyState ReadyStatus { get; set; }
        public IServer Server { get; set; }
        public Dictionary<String, String> Localization;
        public IUser Current { get; private set; }
        public Dictionary<UInt16, IUser> Users { get; private set; }
        public Dictionary<UInt16, IUserGroup> Groups { get; private set; }


        // Constructor
        public UserManager(IServer server)
            : this()
        {
            Server = server;
        }

        public UserManager()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"UserGroup_Administrator", "Administrator"},
								   {"UserGroup_Superuser", "Superuser"},
								   {"UserGroup_User", "User"},
								   {"UserGroup_Guest", "Guest"},
							   };
            Localizations.Update(Localization);

            ReadyStatus = ManagerReadyState.New;
            Current = new User();
            Users = new Dictionary<UInt16, IUser>();
            Groups = new Dictionary<UInt16, IUserGroup>();
            UserStringPermission = new Dictionary<IUser, IEnumerable<String>>();
        }

        public void Initialize()
        {
        }


        public String Status
        {
            get { return "User : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private readonly Stopwatch _watch = new Stopwatch();

        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            Users.Clear();
            Groups.Clear();
            UserStringPermission.Clear();

            _watch.Reset();
            _watch.Start();

            LoadDelegate loadUserDelegate = LoadUser;
            loadUserDelegate.BeginInvoke(LoadUserCallback, loadUserDelegate);
        }

        public void Load(String xml)
        {
        }

        public void Save()
        {
            ReadyStatus = ManagerReadyState.Saving;

            _watch.Reset();
            _watch.Start();

            SaveDelegate savUserDelegate = SaveUser;
            savUserDelegate.BeginInvoke(SaveCallback, savUserDelegate);
        }

        public void Save(String xml)
        {
        }


        private void LoadUserCallback(IAsyncResult result)
        {
            LoadDelegate loadPermissionDelegate = LoadPermission;
            loadPermissionDelegate.BeginInvoke(LoadPermissionCallback, loadPermissionDelegate);
        }

        protected Boolean _haveToSaveGroupTag;
        protected Boolean _loadUserFlag;

        protected virtual void LoadUser()
        {
            Users.Clear();
            Groups.Clear();

            _loadUserFlag = false;

            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadPagePermission, Server.Credential);
            XmlNode node;

            if (xmlDoc != null)
            {
                node = xmlDoc.SelectSingleNode("UserGroups");
                if (node != null)
                {
                    var groupRootNode = (XmlElement)node.SelectSingleNode("Groups");
                    ParseUserGroup(groupRootNode);
                }
            }

            //---------------------------------------------------------------------------------------------------

            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadUser, Server.Credential);
            if (xmlDoc == null) return;
            node = xmlDoc.SelectSingleNode("UserGroups");
            if (node != null)
            {
                //can't load group from new cgi, load from old one
                if (Groups.Count == 0)
                {
                    var groupRootNode = (XmlElement)node.SelectSingleNode("Groups");
                    ParseUserGroup(groupRootNode);
                    _haveToSaveGroupTag = true;
                }

                var userRootNode = (XmlElement)node.SelectSingleNode("Users");
                ParseUser(userRootNode);
            }
        }

        protected void ParseUserGroup(XmlElement groupRootNode)
        {
            if (groupRootNode == null) return;

            XmlNodeList groupNodes = groupRootNode.GetElementsByTagName("Group");
            foreach (XmlElement groupNode in groupNodes)
            {
                IUserGroup group = new UserGroup
                                       {
                                           Id = Convert.ToUInt16(groupNode.GetAttribute("id")),
                                           Name = groupNode.GetAttribute("name"),
                                           Description = Xml.GetFirstElementValueByTagName(groupNode, "Description"),
                                           ReadyState = ReadyState.Ready,
                                       };

                group.TitleName = (Localization.ContainsKey("UserGroup_" + group.Name) ? Localization["UserGroup_" + group.Name] : group.Name);

                ParsePagePermissionFromXml((XmlElement)groupNode.SelectSingleNode("Permissions"), group);
                Groups.Add(group.Id, group);
            }
        }

        protected void ParseUser(XmlElement userRootNode)
        {
            if (userRootNode == null) return;
            XmlNodeList userNodes = userRootNode.GetElementsByTagName("User");
            foreach (XmlElement userNode in userNodes)
            {
                String groupName = Xml.GetFirstElementValueByTagName(userNode, "Group");

                var deviceGroups = new Dictionary<ushort, IDeviceGroup>();
                XmlNodeList listDeviceGroups = userNode.GetElementsByTagName("DeviceGroup");
                if (listDeviceGroups.Count > 0)
                {
                    foreach (XmlElement nodeDeviceGroup in listDeviceGroups)
                    {
                        IDeviceGroup deviceGroup = ParseGroupProfileFromXml(nodeDeviceGroup);
                        if (deviceGroup == null) continue;

                        if (!deviceGroups.ContainsKey(deviceGroup.Id))
                            deviceGroups.Add(deviceGroup.Id, deviceGroup);
                    }
                }

                IUser user = new User
                                 {
                                     Id = Convert.ToUInt16(userNode.GetAttribute("id")),
                                     Email = Xml.GetFirstElementValueByTagName(userNode, "Email"),
                                     Credential =
                                         {
                                             UserName = Encryptions.DecryptDES(userNode.GetAttribute("name")),
                                             Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(userNode, "Password"))
                                         },
                                     DeviceGroups = deviceGroups
                                 };

                foreach (var group in Groups.Values)
                {
                    if (group.Name == groupName)
                    {
                        group.AddUser(user);
                        break;
                    }
                }

                user.ReadyState = ReadyState.Ready;
                Users.Add(user.Id, user);

                if (Server.Credential.UserName == user.Credential.UserName && Server.Credential.Password == user.Credential.Password)
                    Current = user;
            }
        }

        private IDeviceGroup ParseGroupProfileFromXml(XmlElement node)
        {
            List<String> listId = DeviceConverter.XmlElementToStringList(node, "Items");
            var list = ConvertToDevices(listId);

            var viewId = DeviceConverter.XmlElementToStringList(node, "View");
            var view = ConvertToDeviceView(viewId, list);

            var groupId = Convert.ToUInt16(node.GetAttribute("id"));
            return new DeviceGroup
            {
                ReadyState = ReadyState.Ready,
                Id = groupId,
                Server = Server,
                Name = Xml.GetFirstElementValueByTagName(node, "Name"),
                Items = list,
                View = view,
                Layout = ((groupId == 0) ? null : DeviceConverter.XmlElementToLayout(node, "Layout")),
                Regions = DeviceConverter.XmlElementToRegion(node),
                MountType = DeviceConverter.XmlElementToMountType(node),
                DewarpEnable = DeviceConverter.XmlElementToDewarpEnable(node)
            };
        }

        protected virtual List<IDevice> ConvertToDevices(List<string> listId)
        {
            return DeviceConverter.StringToDeviceList(Server, listId);
        }

        protected virtual List<IDevice> ConvertToDeviceView(List<String> viewId, List<IDevice> list)
        {
            return DeviceConverter.StringToDeviceView(Server, viewId, list);
        }

        private void LoadPermission()
        {
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadPermission, Server.Credential);

            if (xmlDoc != null)
            {
                XmlNode node = xmlDoc.SelectSingleNode("Users");
                if (node != null)
                {
                    XmlNodeList userNodes = ((XmlElement)node).GetElementsByTagName("User");
                    foreach (XmlElement userNode in userNodes)
                    {
                        UInt16 id = Convert.ToUInt16(userNode.GetAttribute("id"));
                        String name = Encryptions.DecryptDES(userNode.GetAttribute("name"));

                        IUser user = null;
                        foreach (KeyValuePair<UInt16, IUser> obj in Users)
                        {
                            if (obj.Key == id && obj.Value.Credential.UserName == name)
                            {
                                user = obj.Value;
                                break;
                            }
                        }

                        if (user == null) continue;

                        ParseDevicePermissionFromXml((XmlElement)userNode.SelectSingleNode("Permissions"), user);
                    }
                }
            }

            _loadUserFlag = true;
        }

        protected virtual void ParsePagePermissionFromXml(XmlElement node, IUserGroup group)
        {
            if (node == null) return;

            XmlNodeList permissionNodes = node.GetElementsByTagName("Permission");

            foreach (XmlElement permissionNode in permissionNodes)
            {
                String name = permissionNode.GetAttribute("name");
                String[] permissions = permissionNode.InnerText.Split(',');

                AddPagePermission(group, name, permissions);
            }
        }

        protected void AddPagePermission(IUserGroup group, String pageName, IEnumerable<String> permisions)
        {
            if (!group.Permissions.ContainsKey(pageName))
            {
                group.Permissions.Add(pageName, new List<Permission> { Permission.Access });
            }
            else
            {
                if (!group.Permissions[pageName].Contains(Permission.Access))
                    group.Permissions[pageName].Add(Permission.Access);
            }

            // NOTE: Assigning the setup permission
            if (pageName != "Setup") return;
            if (!group.CheckPermission(pageName, Permission.Access)) return;

            foreach (String permision in permisions)
            {
                var temp = permision;
                if (Server is IPTS)
                {
                    switch (permision)
                    {
                        case "Schedule":
                            temp = "ScheduleReport";
                            break;

                        case "Event":
                            temp = "ExceptionReport";
                            break;
                    }
                }

                try
                {
                    var availablePermission = temp.ToEnum<Permission>();

                    if (!group.Permissions[pageName].Contains(availablePermission))
                        group.Permissions[pageName].Add(availablePermission);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }

        public Dictionary<IUser, IEnumerable<String>> UserStringPermission { get; private set; }

        private void ParseDevicePermissionFromXml(XmlElement node, IUser user)
        {
            if (node == null) return;

            XmlNodeList permissionNodes = node.GetElementsByTagName("Permission");

            List<String> temp = new List<String>();
            foreach (XmlElement permissionNode in permissionNodes)
            {
                UInt16 id = Convert.ToUInt16(permissionNode.GetAttribute("id"));
                String name = permissionNode.GetAttribute("name");
                String[] permissions = permissionNode.InnerText.Split(',');
                temp.AddRange(permissions.Select(permission => name + "." + id + "." + permission));
            }

            UserStringPermission.Add(user, temp.AsEnumerable());
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            if (_saveUserFlag)
            {
                _watch.Stop();
                Trace.WriteLine(@"User Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

                if (ReadyStatus == ManagerReadyState.Saving)
                    ReadyStatus = ManagerReadyState.Ready;

                if (OnSaveComplete != null)
                    OnSaveComplete(this, EventArgs.Empty);
            }
        }

        private Boolean _saveUserFlag;
        private void SaveUser()
        {
            _saveUserFlag = false;
            SaveUserPermissionToXml();
            SaveUserGroupToXml();

            _saveUserFlag = true;

            Boolean loginUserModify = true;
            foreach (KeyValuePair<UInt16, IUser> obj in Users)
            {
                if (Current.Id == obj.Key)
                {
                    switch (obj.Value.ReadyState)
                    {
                        case ReadyState.Ready:
                            loginUserModify = false;
                            break;

                        case ReadyState.New:
                            break;

                        default:
                            //if user name / password not change, don't login out
                            if (String.Equals(Server.Credential.UserName, obj.Value.Credential.UserName) && String.Equals(Server.Credential.Password, obj.Value.Credential.Password))
                                loginUserModify = false;
                            break;
                    }
                }
            }

            if (loginUserModify)
            {
                foreach (var obj in Users)
                {
                    if (obj.Value.Id == Current.Id)
                    {
                        Server.Credential.UserName = obj.Value.Credential.UserName;
                        Server.Credential.Password = obj.Value.Credential.Password;
                        break;
                    }
                }
                ReadyStatus = ManagerReadyState.MajorModify;
            }

            foreach (var obj in Groups)
                obj.Value.ReadyState = ReadyState.Ready;

            foreach (var obj in Users)
                obj.Value.ReadyState = ReadyState.Ready;
        }

        private void SaveUserGroupToXml()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadUser, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("UserGroups");
            xmlDoc.AppendChild(xmlRoot);

            //no need save group group back to server
            if (_haveToSaveGroupTag)
            {
                var groupRootNode = xmlDoc.CreateElement("Groups");
                xmlRoot.AppendChild(groupRootNode);

                var groupSortResult = Groups.Values.OrderBy(g => g.Id);

                foreach (var group in groupSortResult)
                {
                    //----------------------------------DeviceSetting
                    var groupNode = xmlDoc.CreateElement("Group");
                    groupNode.SetAttribute("id", group.Id.ToString(CultureInfo.InvariantCulture));
                    groupNode.SetAttribute("name", group.Name);
                    groupRootNode.AppendChild(groupNode);

                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Description", @group.Description));
                    groupNode.AppendChild(ParsePermissionToXml(xmlDoc, group));
                }
            }

            var userRootNode = xmlDoc.CreateElement("Users");
            xmlRoot.AppendChild(userRootNode);

            var userSortResult = Users.Values.OrderBy(u => u.Id);

            foreach (var user in userSortResult)
            {
                //----------------------------------DeviceSetting
                var userNode = xmlDoc.CreateElement("User");
                userNode.SetAttribute("id", user.Id.ToString(CultureInfo.InvariantCulture));
                userNode.SetAttribute("name", Encryptions.EncryptDES(user.Credential.UserName));
                userRootNode.AppendChild(userNode);

                userNode.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(user.Credential.Password)));
                userNode.AppendChild(xmlDoc.CreateXmlElementWithText("Email", user.Email));
                userNode.AppendChild(xmlDoc.CreateXmlElementWithText("Group", user.Group.Name));

                foreach (KeyValuePair<UInt16, IDeviceGroup> deviceGroup in user.DeviceGroups)
                {
                    deviceGroup.Value.ReadyState = ReadyState.Ready;
                    var groupNode = xmlDoc.CreateElement("DeviceGroup");
                    groupNode.SetAttribute("id", deviceGroup.Key.ToString(CultureInfo.InvariantCulture));
                    userNode.AppendChild(groupNode);

                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Name", deviceGroup.Value.Name));

                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Items", DeviceConverter.DeviceListToString(deviceGroup.Value.Items)));
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("View", DeviceConverter.DeviceViewToString(deviceGroup.Value.View)));
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Layout", WindowLayouts.LayoutToString(deviceGroup.Value.Layout)));
                    var regionNode = xmlDoc.CreateXmlElementWithText("Region", "");
                    groupNode.AppendChild(regionNode);
                    foreach (XmlElement region in deviceGroup.Value.Regions)
                    {
                        var deviceRegion = xmlDoc.CreateXmlElementWithText("DeviceRegion", "");
                        regionNode.AppendChild(deviceRegion);
                        if (region == null) continue;
                        var imported = xmlDoc.ImportNode(region, true);
                        deviceRegion.AppendChild(imported);
                    }

                    var mountTypeNode = xmlDoc.CreateXmlElementWithText("MountTypes", "");
                    groupNode.AppendChild(mountTypeNode);
                    foreach (Int16 mountType in deviceGroup.Value.MountType)
                    {
                        var deviceMountType = xmlDoc.CreateXmlElementWithText("MountType", mountType);
                        mountTypeNode.AppendChild(deviceMountType);
                    }

                    var dewarpEnableNode = xmlDoc.CreateXmlElementWithText("DewarpEnable", "");
                    groupNode.AppendChild(dewarpEnableNode);
                    foreach (Boolean enable in deviceGroup.Value.DewarpEnable)
                    {
                        var deviceDewarpEnable = xmlDoc.CreateXmlElementWithText("Enable", enable ? 1 : 0);
                        dewarpEnableNode.AppendChild(deviceDewarpEnable);
                    }
                }
            }

            if (orangeXmlDoc != null && !String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                Xml.PostXmlToHttp(CgiSaveUser, xmlDoc, Server.Credential);
        }

        private static XmlElement ParsePermissionToXml(XmlDocument xmlDoc, IUserGroup group)
        {
            var permissionNode = xmlDoc.CreateElement("Permissions");
            foreach (var obj in group.Permissions)
            {
                var permission = xmlDoc.CreateElement("Permission");
                permission.SetAttribute("name", obj.Key);

                var str = obj.Value.Select(accessType => accessType.ToString()).ToArray();
                permission.InnerText = String.Join(",", str);

                permissionNode.AppendChild(permission);
            }

            return permissionNode;
        }

        private void SaveUserPermissionToXml()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadPermission, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Users");
            xmlDoc.AppendChild(xmlRoot);

            var userSortResult = Users.Values.OrderBy(u => u.Id);

            foreach (var user in userSortResult)
            {
                if (user.Group.IsFullAccessToDevices) continue;

                var userNode = xmlDoc.CreateElement("User");
                userNode.SetAttribute("id", user.Id.ToString(CultureInfo.InvariantCulture));
                userNode.SetAttribute("name", Encryptions.EncryptDES(user.Credential.UserName));

                var permissionRoot = xmlDoc.CreateElement("Permissions");

                if (Server is ICMS)
                {
                    var nvrs = user.NVRPermissions.Keys.OrderBy(k => k.Id);

                    foreach (var nvr in nvrs)
                    {
                        var permission = xmlDoc.CreateElement("Permission");
                        permission.SetAttribute("id", nvr.Id.ToString(CultureInfo.InvariantCulture));
                        permission.SetAttribute("name", "NVR");

                        var str = user.NVRPermissions[nvr].Select(accessType => accessType.ToString()).ToArray();
                        permission.InnerText = String.Join(",", str);

                        permissionRoot.AppendChild(permission);
                    }
                }
                else
                {
                    var devices = user.Permissions.Keys.OrderBy(k => k.Id);

                    foreach (var device in devices)
                    {
                        var permission = xmlDoc.CreateElement("Permission");
                        permission.SetAttribute("id", device.Id.ToString(CultureInfo.InvariantCulture));
                        permission.SetAttribute("name", "Device");

                        var str = user.Permissions[device].Select(accessType => accessType.ToString()).ToArray();
                        permission.InnerText = String.Join(",", str);

                        permissionRoot.AppendChild(permission);
                    }
                }
                userNode.AppendChild(permissionRoot);

                xmlRoot.AppendChild(userNode);
            }

            if (orangeXmlDoc != null && !String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                Xml.PostXmlToHttp(CgiSavePermission, xmlDoc, Server.Credential);
        }

        private delegate void LoadDelegate();
        private void LoadPermissionCallback(IAsyncResult result)
        {
            ((LoadDelegate)result.AsyncState).EndInvoke(result);

            if (_loadUserFlag)
            {
                _watch.Stop();

                ReadyStatus = ManagerReadyState.Ready;

                if (OnLoadComplete != null)
                    OnLoadComplete(this, EventArgs.Empty);
            }
        }

        public UInt16 GetNewUserId()
        {
            for (UInt16 id = 1; id <= Users.Count + 2; id++)
            {
                if (Users.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }
    }
}
