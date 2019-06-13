using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Constant;
using Interface;

namespace ServerProfile
{
    public class User : IUser
    {
        public UInt16 Id { get; set; }
        public NetworkCredential Credential { get; set; }
        public IUserGroup Group { get; set; }
        public String Email { get; set; }
        public ReadyState ReadyState { get; set; }
        public Dictionary<UInt16, IDeviceGroup> DeviceGroups { get; set; }

        public User()
        {
            Credential = new NetworkCredential
            {
                UserName = "",
                Password = "",
            };
            Email = "";
            ReadyState = ReadyState.New;
            Permissions = new Dictionary<IDevice, List<Permission>>();
            NVRPermissions = new Dictionary<INVR, List<Permission>>();

            DeviceGroups = new Dictionary<UInt16, IDeviceGroup>();
        }

        public Dictionary<IDevice, List<Permission>> Permissions { get; set; }
        public Dictionary<INVR, List<Permission>> NVRPermissions { get; set; }

        public Boolean CheckPermission(IDevice device, Permission permission)
        {
            if(Permissions == null|| Permissions.Count == 0) return false;
            if (!Permissions.ContainsKey(device))
                return false;

            return Permissions[device].Any(value => value == permission);
        }

        public Boolean CheckPermission(INVR nvr, Permission permission)
        {
            if (NVRPermissions == null || NVRPermissions.Count == 0) return false;
            if (!NVRPermissions.ContainsKey(nvr))
                return false;

            return NVRPermissions[nvr].Any(value => value == permission);
        }

        public void AddFullDevicePermission(IDevice device)
        {
            Permissions.Remove(device);

            Boolean includePlayback = Group.IsFullAccessToDevices;
            if (!includePlayback)
            {
                foreach (KeyValuePair<String, List<Permission>> obj in Group.Permissions)
                {
                    if (obj.Key != "Playback") continue;

                    includePlayback = true;
                    break;
                }
            }

            Permissions.Add(device, new List<Permission>
                {
                    Permission.Access,
                    Permission.AudioIn,
                    Permission.AudioOut,
                    Permission.ManualRecord,
                    Permission.OpticalPTZ,
                });

            if (includePlayback)
            {
                Permissions[device].AddRange(new[]
                {
                    Permission.ExportVideo,
                    Permission.PrintImage,
                });
            }
        }

        public static UInt16 MaximumDeviceGroupsAmount = 20;
        public UInt16 GetNewGroupId()
        {
            for (UInt16 id = 1; id <= MaximumDeviceGroupsAmount; id++)
            {
                if (DeviceGroups.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }
    }
}
