using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace ServerProfile
{
    public class UserGroup : IUserGroup
    {
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public String TitleName { get; set; }
        public List<IUser> Users { get; set; }
        public String Description { get; set; }
        public ReadyState ReadyState { get; set; }

        public UserGroup()
        {
            Users = new List<IUser>();
            ReadyState = ReadyState.New;
            Permissions = new Dictionary<String, List<Permission>>();
        }

        public void AddUser(IUser user)
        {
            if (user.Group == this) return;

            if(user.Group != null)
                user.Group.Users.Remove(user);

            if(!Users.Contains(user))
                Users.Add(user);

            user.Group = this;

            if (user.ReadyState == ReadyState.Ready)
                user.ReadyState = ReadyState.Modify;
        }

        public Boolean IsFullAccessToDevices {
            get { return (Name == "Administrator" && Id == 0) || (Name == "Superuser" && Id == 1); }
        }

        public Dictionary<String, List<Permission>> Permissions { get; set; }
        public Boolean CheckPermission(String pageName, Permission permission)
        {
            if (pageName == "" || Permissions == null || Permissions.Count == 0) return false;

            return (Permissions.ContainsKey(pageName) && Permissions[pageName].Contains(permission));
        }
    }
}
