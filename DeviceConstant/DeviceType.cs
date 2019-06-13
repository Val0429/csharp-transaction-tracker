using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum DeviceType : ushort
    {
        Unknown = 0,
        Device = 1,
        Map = 2,
        GPS = 3,
        Tracker = 4,
        Layout = 5,
        LPR = 6,
        Door = 7,
        Drone = 8,
    }

    public static class DeviceTypes
    {
        public static DeviceType ToIndex(UInt16 key)
        {
            foreach (KeyValuePair<DeviceType, String> keyValuePair in List)
            {
                if (Equals(key, ToIndex(keyValuePair.Key)))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static UInt16 ToIndex(DeviceType key)
        {
            foreach (KeyValuePair<DeviceType, String> keyValuePair in List)
            {
                if (Equals(key, keyValuePair.Key))
                    return (ushort)keyValuePair.Key;
            }

            return 0;
        }

        public static String ToString(DeviceType index)
        {
            foreach (KeyValuePair<DeviceType, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<DeviceType, String> List = new Dictionary<DeviceType, String>
                                                             {
                                                                 { DeviceType.Unknown, "Unknown" },
                                                                 { DeviceType.Device, "Device" },
                                                                 { DeviceType.Map, "Map" },
                                                                 { DeviceType.GPS, "GPS" },
                                                                 { DeviceType.Tracker, "Tracker" }
                                                             };
    }

}
