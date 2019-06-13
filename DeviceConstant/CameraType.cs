using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum CameraType : ushort
    {
        Unknown = 0,
        Single = 1,
        Quad = 2,
        Multi = 3,
        Encoder = 4,
        Card = 5,
        VideoServer = 6
    }

    public static class CameraTypes
    {
        public static CameraType ToIndex(String value)
        {
            foreach (KeyValuePair<CameraType, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return CameraType.Unknown;
        }

        public static String ToString(CameraType index)
        {
            foreach (KeyValuePair<CameraType, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<CameraType, String> List = new Dictionary<CameraType, String>
                                                             {
                                                                 { CameraType.Single, "SINGLE"},
                                                                 { CameraType.Quad, "QUAD"},
                                                                 { CameraType.Multi, "MULTI" },
                                                                 { CameraType.Encoder, "ENCODER"},
                                                                 { CameraType.Card, "CARD" },
                                                                 { CameraType.VideoServer, "VIDEO SERVER" }
                                                             };
    }
}