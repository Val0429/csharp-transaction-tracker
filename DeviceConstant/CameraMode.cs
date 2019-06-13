using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum CameraMode : ushort
    {
        Single = 1,
        Quad = 2,
        //Sequential = 3,
        Dual = 4,
        FourVga = 5,
        Triple = 6,
        Multi= 7,
        Five = 8,
        SixVga = 9,
    }
    public static class CameraModes
    {
        public static CameraMode ToIndex(String value)
        {
            foreach (KeyValuePair<CameraMode, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static CameraMode DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<CameraMode, String> keyValuePair in List2)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(CameraMode index)
        {
            foreach (KeyValuePair<CameraMode, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(CameraMode index)
        {
            foreach (KeyValuePair<CameraMode, String> keyValuePair in List2)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<CameraMode, String> List = new Dictionary<CameraMode, String>
                                                             {
                                                                 { CameraMode.Single, "SINGLE"},
                                                                 { CameraMode.Quad, "QUAD"},
                                                                 //{ CameraMode.Sequential, "SEQUENTIAL" },
                                                                 { CameraMode.Dual, "DUAL" },
                                                                 { CameraMode.FourVga, "FOURVGA" },
                                                                 { CameraMode.Triple, "TRIPLE" },
                                                                 { CameraMode.Multi, "MULTI" },
                                                                 { CameraMode.Five, "FIVE" },
                                                                 { CameraMode.SixVga, "SIXVGA" },
                                                             };

        public static readonly Dictionary<CameraMode, String> List2 = new Dictionary<CameraMode, String>
                                                             {
                                                                 { CameraMode.Single, "Single Stream"},
                                                                 { CameraMode.Quad, "Quad"},
                                                                 //{ CameraMode.Sequential, "SEQUENTIAL" },
                                                                 { CameraMode.Dual, "Dual Stream" },
                                                                 { CameraMode.FourVga, "Four VGA" },
                                                                 { CameraMode.Triple, "Triple Stream" },
                                                                 { CameraMode.Multi, "Multi Stream" },
                                                                 { CameraMode.Five, "Five" },
                                                                 { CameraMode.SixVga, "Six VGA" },
                                                             };
    }
}