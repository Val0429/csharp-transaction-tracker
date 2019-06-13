using System;
using System.Collections.Generic;

namespace Constant
{
    public class Storage
    {
        public Storage()
        {
            
        }

        /// <summary>
        /// Drive letter
        /// </summary>
        public String Key { get; set; }
        public String Name { get; set; }
        public String Path { get; set; }
        public UInt16 KeepSpace { get; set; }
    }

    public class DiskInfo
    {
        public Int64 Total { get; set; }
        public String Name { get; set; }
        public Int64 Used { get; set; }
        public Int64 Free { get; set; }
    }

    public class RAID
    {
        public RAIDMode Mode { get; set; }
        public RAIDStatus Status { get; set; }
        public UInt16 Disks { get; set; }
        public Dictionary<String, RAIDDisk> DiskStatus { get; set; }
        public UInt16 Process { get; set; }

        public static RAIDMode FindRAIDMode(String mode)
        {
            switch (mode)
            {
                case "RAID0":
                    return RAIDMode.RAID0;

                case "RAID1":
                    return RAIDMode.RAID1;

                case "RAID5":
                    return RAIDMode.RAID5;

                case "RAID10":
                    return RAIDMode.RAID10;

                default:
                    return RAIDMode.None;

            }
        }

        public static RAIDStatus FindRAIDStatus(String status)
        {
            switch (status)
            {
                case "STANDBY":
                    return RAIDStatus.Standby;

                case "CHECK":
                    return RAIDStatus.Check;

                case "FORMAT":
                    return RAIDStatus.Format;

                case "ACTIVE":
                    return RAIDStatus.Active;

                case "INACTIVE":
                    return RAIDStatus.Inactive;

                case "RECOVERY":
                    return RAIDStatus.Recovery;

                case "RESYNC":
                    return RAIDStatus.Resync;

                case "DEGRADED":
                    return RAIDStatus.Degrade;

                default:
                    return RAIDStatus.Inactive;
            }
        }
    }

    public class RAIDDisk
    {
        public RAIDDiskStatus Status { get; set; }
        public String Description { get; set; }
        public String DescValue { get; set; }

        public static RAIDDiskStatus FindRAIDDiskStatus(String status)
        {
            switch (status)
            {
                case "ERROR":
                    return RAIDDiskStatus.Error;

                case "LOST":
                    return RAIDDiskStatus.Lost;

                case "UNUSED":
                    return RAIDDiskStatus.Unused;

                case "OK":
                    return RAIDDiskStatus.Ok;

                case "REBUILD":
                    return RAIDDiskStatus.Rebuild;

                case "WARNING":
                    return RAIDDiskStatus.Warning;

            }
            return RAIDDiskStatus.Unused;
        }
    }

    public enum RAIDMode
    {
        None = -1,
        RAID0 = 0,
        RAID1 = 1,
        RAID5 = 5,
        RAID10 = 10
    }

    public enum RAIDStatus
    {
        Standby = 1,
        Check = 2,
        Active = 3,
        Inactive = 4,
        Recovery = 5,
        Resync = 6,
        Degrade = 7,
        Format = 8
    }

    public enum RAIDDiskStatus
    {
        Unused = 0,
        Ok = 1,
        Error = 2,
        Lost = 3,
        Rebuild = 4,
        Warning = 5 
    }
}