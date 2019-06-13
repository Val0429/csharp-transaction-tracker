using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum BitrateControl : ushort
    {
        NA = 0,
        VBR = 1,
        CBR = 2
    }

    public static class BitrateControls
    {
        public static BitrateControl ToIndex(String value)
        {
            foreach (KeyValuePair<BitrateControl, String> keyValuePair in List)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(BitrateControl index)
        {
            foreach (KeyValuePair<BitrateControl, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<BitrateControl, String> List = new Dictionary<BitrateControl, String>
                                                             {
                                                                 { BitrateControl.NA, "N/A" },
                                                                 { BitrateControl.VBR, "VBR" },
                                                                 { BitrateControl.CBR, "CBR" },
                                                             };
    }
}
