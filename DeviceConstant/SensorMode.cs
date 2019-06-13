using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum SensorMode : ushort
    {
        NonSpecific,
        Progressive720P,
        Progressive720P25,
        Progressive720P30,
        Progressive720P50,
        Progressive720P60,
        Progressive1080I,
        Progressive1080I25,
        Progressive1080I30,
        Progressive1080I50,
        Progressive1080I60,
        Progressive1080P,
        Progressive1080P25,
        Progressive1080P30,
        Progressive1080P50,
        Progressive1080P60,
        Megapixel1,
        Megapixel1Dot3,
        Megapixel2,
        Megapixel3,
        Megapixel5,
        Fisheye,
        Computer,
        Mobile,
        BinningDayMode,
        BinningNightMode,
        BinningOff,
        VGA,
        XGA,
        SXGA,
        Atlantis,
        Axis,
        BlackHot,
        IceAndFire,
        Nightvision,
        Planck,
        Rainbow,
        WhiteHot,
        WUXGA,
        WDR1080p,
        WDR1080p25,
        S3M30FPS
    }

    public static class SensorModes
    {
        public static SensorMode ToIndex(String value)
        {
            if (String.IsNullOrEmpty(value)) return SensorMode.NonSpecific;

            foreach (KeyValuePair<SensorMode, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            value = value.ToLower();
            foreach (KeyValuePair<SensorMode, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

              return SensorMode.NonSpecific;
        }

        public static SensorMode DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<SensorMode, String> keyValuePair in DisplayList)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(SensorMode index)
        {
            foreach (KeyValuePair<SensorMode, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(SensorMode index)
        {
            foreach (KeyValuePair<SensorMode, String> keyValuePair in DisplayList)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<SensorMode, String> List = new Dictionary<SensorMode, String>
                                                             {
                                                                 { SensorMode.Progressive720P, "720p" },
                                                                 { SensorMode.Progressive720P25, "720p25" },
                                                                 { SensorMode.Progressive720P30, "720p30" },
                                                                 { SensorMode.Progressive720P50, "720p50" },
                                                                 { SensorMode.Progressive720P60, "720p60" },

                                                                 { SensorMode.Progressive1080I, "1080i" },
                                                                 { SensorMode.Progressive1080I25, "1080i25" },
                                                                 { SensorMode.Progressive1080I30, "1080i30" },
                                                                 { SensorMode.Progressive1080I50, "1080i50" },
                                                                 { SensorMode.Progressive1080I60, "1080i60" },
                                                                 
                                                                 { SensorMode.Progressive1080P, "1080p" },
                                                                 { SensorMode.Progressive1080P25, "1080p25" },
                                                                 { SensorMode.Progressive1080P30, "1080p30" },
                                                                 { SensorMode.Progressive1080P50, "1080p50" },
                                                                 { SensorMode.Progressive1080P60, "1080p60" },

                                                                 { SensorMode.WDR1080p, "1080pWDR" },
                                                                 { SensorMode.WDR1080p25, "1080p25WDR" },

                                                                 { SensorMode.Megapixel1, "1MP" },
                                                                 { SensorMode.Megapixel1Dot3, "1.3MP" },
                                                                 { SensorMode.Megapixel2, "2MP" },
                                                                 { SensorMode.Megapixel3, "3MP" },
                                                                 { SensorMode.Megapixel5, "5MP" },

                                                                 { SensorMode.Fisheye, "Fisheye" },
                                                                 { SensorMode.Computer, "Computer" },
                                                                 { SensorMode.Mobile, "Mobile" },
                                                                  { SensorMode.BinningDayMode, "BinningDayMode" },
                                                                 { SensorMode.BinningNightMode, "BinningNightMode" },
                                                                 { SensorMode.BinningOff, "BinningOff" },
                                                                 { SensorMode.VGA, "VGA" },
                                                                 { SensorMode.XGA, "XGA" },
                                                                 { SensorMode.SXGA, "SXGA" },

                                                                 { SensorMode.Atlantis, "Atlantis" },
                                                                 { SensorMode.Axis, "Axis" },
                                                                 { SensorMode.BlackHot, "Black-hot" },
                                                                  { SensorMode.IceAndFire, "Ice-and-fire" },
                                                                 { SensorMode.Nightvision, "Nightvision" },
                                                                 { SensorMode.Planck, "Planck" },
                                                                 { SensorMode.Rainbow, "Rainbow" },
                                                                 { SensorMode.WhiteHot, "White-hot" },
                                                                 { SensorMode.WUXGA, "WUXGA" },
                                                                  { SensorMode.S3M30FPS, "3M30FPS" }
                                                             };
        public static readonly Dictionary<SensorMode, String> DisplayList = new Dictionary<SensorMode, String>
                                                             {
                                                                { SensorMode.Progressive720P, "720p" },
                                                                 { SensorMode.Progressive720P25, "720p25" },
                                                                 { SensorMode.Progressive720P30, "720p30" },
                                                                 { SensorMode.Progressive720P50, "720p50" },
                                                                 { SensorMode.Progressive720P60, "720p60" },

                                                                 { SensorMode.Progressive1080I, "1080i" },
                                                                 { SensorMode.Progressive1080I25, "1080i25" },
                                                                 { SensorMode.Progressive1080I30, "1080i30" },
                                                                 { SensorMode.Progressive1080I50, "1080i50" },
                                                                 { SensorMode.Progressive1080I60, "1080i60" },
                                                                 
                                                                 { SensorMode.Progressive1080P, "1080p" },
                                                                 { SensorMode.Progressive1080P25, "1080p25" },
                                                                 { SensorMode.Progressive1080P30, "1080p30" },
                                                                 { SensorMode.Progressive1080P50, "1080p50" },
                                                                 { SensorMode.Progressive1080P60, "1080p60" },

                                                                 { SensorMode.WDR1080p, "1080p WDR" },
                                                                 { SensorMode.WDR1080p25, "1080p25 WDR" },

                                                                 { SensorMode.Megapixel1, "1MP" },
                                                                 { SensorMode.Megapixel1Dot3, "1.3MP" },
                                                                 { SensorMode.Megapixel2, "2MP" },
                                                                 { SensorMode.Megapixel3, "3MP" },
                                                                 { SensorMode.Megapixel5, "5MP" },

                                                                 { SensorMode.Fisheye, "Fisheye" },
                                                                 { SensorMode.Computer, "Computer" },
                                                                 { SensorMode.Mobile, "Mobile" },
                                                                  { SensorMode.BinningDayMode, "Binning day mode" },
                                                                 { SensorMode.BinningNightMode, "Binning night mode" },
                                                                 { SensorMode.BinningOff, "Binning off" },
                                                                 { SensorMode.VGA, "VGA" },
                                                                 { SensorMode.XGA, "XGA" },
                                                                 { SensorMode.SXGA, "SXGA" },

                                                                 { SensorMode.Atlantis, "Atlantis" },
                                                                 { SensorMode.Axis, "Axis" },
                                                                 { SensorMode.BlackHot, "Black hot" },
                                                                 { SensorMode.IceAndFire, "Ice and fire" },
                                                                 { SensorMode.Nightvision, "Nightvision" },
                                                                 { SensorMode.Planck, "Planck" },
                                                                 { SensorMode.Rainbow, "Rainbow" },
                                                                 { SensorMode.WhiteHot, "White hot" },
                                                                 { SensorMode.WUXGA, "WUXGA" },
                                                                 { SensorMode.S3M30FPS, "3M30FPS" }
                                                             };
    }
}