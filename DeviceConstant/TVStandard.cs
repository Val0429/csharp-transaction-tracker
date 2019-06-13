
using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum TvStandard : ushort
    {
        NonSpecific = 0,
        NTSC = 1,
        PAL = 2,
        NTSC720p60fps = 3,
        PAL720p50fps = 4,
        NTSC1080p30fps = 5,
        PAL1080p25fps = 6,
        PAL50fps = 7,
        PAL25fps = 8,
        NTSC60fps = 9,
        NTSC30fps = 10
    }

    public static class TvStandards
    {
        public static TvStandard ToIndex(String value)
        {
            foreach (KeyValuePair<TvStandard, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return TvStandard.NonSpecific;
        }

        public static String ToString(TvStandard index)
        {
            foreach (KeyValuePair<TvStandard, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<TvStandard, String> List = new Dictionary<TvStandard, String>
                                                             {
                                                                 { TvStandard.NTSC, "NTSC" },
                                                                 { TvStandard.PAL, "PAL" },
                                                                 { TvStandard.NTSC720p60fps, "NTSC (720p 60 fps)" },
                                                                 { TvStandard.PAL720p50fps, "PAL (720p 50 fps)" },
                                                                 {TvStandard.NTSC1080p30fps,"NTSC (1080p 30 fps)"},
                                                                 {TvStandard.PAL1080p25fps, "PAL (1080p 25 fps)"},
                                                                 {TvStandard.PAL50fps, "PAL (50 fps)"},
                                                                 {TvStandard.PAL25fps, "PAL (25 fps)"},
                                                                 {TvStandard.NTSC60fps, "NTSC (60 fps)"},
                                                                 {TvStandard.NTSC30fps, "NTSC (30 fps)"}
                                                             };
    }
}