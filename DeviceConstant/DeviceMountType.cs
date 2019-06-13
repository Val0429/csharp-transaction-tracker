using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum DeviceMountType : ushort
    {
        NonSpecific = 0,
        Ceiling = 1,
        Floor = 2,
        Wall = 3
    }

    public static class DeviceMountTypes 
    {
        public static DeviceMountType ToIndex(String value)
        {
            foreach (KeyValuePair<DeviceMountType, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return DeviceMountType.NonSpecific;
        }

        public static String ToString(DeviceMountType index)
        {
            foreach (KeyValuePair<DeviceMountType, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<DeviceMountType, String> List = new Dictionary<DeviceMountType, String>
                                                             {
                                                                 { DeviceMountType.NonSpecific, "" },
                                                                 { DeviceMountType.Ceiling, "Ceiling" },
                                                                 { DeviceMountType.Floor, "Floor" },
                                                                 { DeviceMountType.Wall, "Wall" }
                                                             };

    }
}