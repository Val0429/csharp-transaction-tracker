using System;
using System.Collections.Generic;
using Constant;

namespace Interface
{
    public interface IUserGroup
    {
        /// <summary>
        /// Gets or sets the value group ID
        /// </summary>
        UInt16 Id { get; set; }
        /// <summary>
        /// Gets or sets the value Group Name
        /// </summary>
        String Name { get; set; }
        String TitleName { get; set; }
        /// <summary>
        /// Gets or set the user list of the current group
        /// </summary>
        List<IUser> Users { get; set; }
        /// <summary>
        /// Gets or sets the value group description
        /// </summary>
        String Description { get; set; }
        ReadyState ReadyState { get; set; }
        Boolean IsFullAccessToDevices { get; }
        Dictionary<String, List<Permission>> Permissions { get; set; }

        void AddUser(IUser user);

        Boolean CheckPermission(String pageName, Permission permission);
    }
}
