using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum Compression : ushort
    {
        H264 = 1,
        Mjpeg = 2,
        Mpeg4 = 3,
        Off = 4,
        Svc = 5,
        Disable = 6
    }

    public static class Compressions
    {
        public static Compression DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<Compression, String> keyValuePair in DisplayList)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static Compression ToIndex(String value)
        {
            foreach (var keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static String ToString(Compression compression)
        {
            foreach (var keyValuePair in List)
            {
                if (compression == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(Compression index)
        {
            foreach (KeyValuePair<Compression, String> keyValuePair in DisplayList)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<Compression, String> List = new Dictionary<Compression, String>
                                                             {
                                                                 { Compression.H264, "H264"},
                                                                 { Compression.Mjpeg, "MJPEG"},
                                                                 { Compression.Mpeg4, "MPEG4"},
                                                                 { Compression.Off, "Off"},
                                                                 { Compression.Svc, "SVC"},
                                                                 { Compression.Disable, "DISABLE"}
                                                             };

        public static readonly Dictionary<Compression, String> DisplayList = new Dictionary<Compression, String>
                                                             {
                                                                 { Compression.H264, "H.264"},
                                                                 { Compression.Mjpeg, "MJPEG"},
                                                                 { Compression.Mpeg4, "MPEG-4"},
                                                                 { Compression.Off, "Off"},
                                                                 { Compression.Disable, "DISABLE"}
                                                             };
    }

    public enum DecodeMode : ushort
    {
        DropFrame = 0,
        DecodeI = 1,
    }
}