using System;
using System.Collections.Generic;
using System.Net;
using Constant;

namespace Interface
{
    public interface IUser
    {
        /// <summary>
        /// User ID
        /// </summary>
        UInt16 Id { get; set; }
        /// <summary>
        /// Login server credential
        /// </summary>
        NetworkCredential Credential { get; set; }
        /// <summary>
        /// Belonging group
        /// </summary>
        IUserGroup Group { get; set; }
        /// <summary>
        /// User contact Email
        /// </summary>
        String Email { get; set; }
        /// <summary>
        /// To indicate this instance is just or already created, ...etc.
        /// </summary>
        ReadyState ReadyState { get; set; }

        Dictionary<UInt16, IDeviceGroup> DeviceGroups { get; set; }

        Dictionary<IDevice, List<Permission>> Permissions { get; set; }
        
        Dictionary<INVR, List<Permission>> NVRPermissions { get; set; }
        /// <summary>
        /// To check the user has permission to use it or not.
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="permission">Operation permission (Access, ExportVideo, ManualRecord, ...etc.)</param>
        /// <returns></returns>
        Boolean CheckPermission(IDevice device, Permission permission);
        /// <summary>
        /// For CMS
        /// </summary>
        /// <param name="nvr"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Boolean CheckPermission(INVR nvr, Permission permission);

        void AddFullDevicePermission(IDevice device);

        UInt16 GetNewGroupId();
    }
}
