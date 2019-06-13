using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum Bitrate : ushort
    {
        NA = 0,
        Bitrate2K,
        Bitrate5K,
        Bitrate10K,
        Bitrate20K,
        Bitrate28K,
        Bitrate30K,
        Bitrate32K,
        Bitrate40K,
        Bitrate48K,
        Bitrate50K,
        Bitrate56K,
        Bitrate64K,
        Bitrate96K,
        Bitrate128K,
        Bitrate192K,
        Bitrate200K,
        Bitrate256K,
        Bitrate320K,
        Bitrate384K,
        Bitrate448K,
        Bitrate500K,
        Bitrate512K,
        Bitrate576K,
        Bitrate640K,
        Bitrate704K,
        Bitrate750K,
        Bitrate768K,
        Bitrate1M,
        Bitrate1M200K,
        Bitrate1M250K,
        Bitrate1M500K,
        Bitrate1M750K,
        Bitrate2M,
        Bitrate2M500K,
        Bitrate3M,
        Bitrate3M500K,
        Bitrate4M,
        Bitrate4M500K,
        Bitrate5M,
        Bitrate5M500K,
        Bitrate6M,
        Bitrate6M500K,
        Bitrate7M,
        Bitrate7M500K,
        Bitrate8M,
        Bitrate10M,
        Bitrate12M,
        Bitrate14M,
        Bitrate15M,
        Bitrate16M,
        Bitrate18M,
        Bitrate20M,
        Bitrate24M,
        Bitrate28M,
        Bitrate32M,
        Bitrate100M
    }

    public static class Bitrates
    {
        public static Bitrate ToIndex(String value)
        {
            foreach (KeyValuePair<Bitrate, String> keyValuePair in List)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static Bitrate DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<Bitrate, String> keyValuePair in DisplayList)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(Bitrate index)
        {
            foreach (KeyValuePair<Bitrate, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(Bitrate index)
        {
            foreach (KeyValuePair<Bitrate, String> keyValuePair in DisplayList)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<Bitrate, String> List = new Dictionary<Bitrate, String>
                                                             {
                                                                 { Bitrate.NA, "0" },
                                                                 { Bitrate.Bitrate2K, "2" },
                                                                 { Bitrate.Bitrate5K, "5" },
                                                                 { Bitrate.Bitrate10K, "10" },
                                                                 { Bitrate.Bitrate20K, "20" },
                                                                 { Bitrate.Bitrate28K, "28" },
                                                                 { Bitrate.Bitrate30K, "30" },
                                                                 { Bitrate.Bitrate32K, "32" },
                                                                 { Bitrate.Bitrate40K, "40" },
                                                                 { Bitrate.Bitrate48K, "48" },
                                                                 { Bitrate.Bitrate50K, "50" },
                                                                 { Bitrate.Bitrate56K, "56" },
                                                                 { Bitrate.Bitrate64K, "64" },
                                                                 { Bitrate.Bitrate96K, "96" },
                                                                 { Bitrate.Bitrate128K, "128" },
                                                                 { Bitrate.Bitrate192K, "192" },
                                                                 { Bitrate.Bitrate200K, "200" },
                                                                 { Bitrate.Bitrate256K, "256" },
                                                                 { Bitrate.Bitrate320K, "320" },
                                                                 { Bitrate.Bitrate384K, "384" },
                                                                  { Bitrate.Bitrate448K, "448" },
                                                                 { Bitrate.Bitrate500K, "500" },
                                                                 { Bitrate.Bitrate512K, "512" },
                                                                 { Bitrate.Bitrate576K, "576" },
                                                                 { Bitrate.Bitrate640K, "640" },
                                                                 { Bitrate.Bitrate704K, "704" },
                                                                 { Bitrate.Bitrate750K, "750" },
                                                                 { Bitrate.Bitrate768K, "768" },
                                                                 { Bitrate.Bitrate1M, "1000" },
                                                                 { Bitrate.Bitrate1M200K, "1200" },
                                                                 { Bitrate.Bitrate1M250K, "1250" },
                                                                 { Bitrate.Bitrate1M500K, "1500" },
                                                                 { Bitrate.Bitrate1M750K, "1750" },
                                                                 { Bitrate.Bitrate2M, "2000" },
                                                                 { Bitrate.Bitrate2M500K, "2500" },
                                                                 { Bitrate.Bitrate3M, "3000" },
                                                                 { Bitrate.Bitrate3M500K, "3500" },
                                                                 { Bitrate.Bitrate4M, "4000" },
                                                                 { Bitrate.Bitrate4M500K, "4500" },
                                                                 { Bitrate.Bitrate5M, "5000" },
                                                                 { Bitrate.Bitrate5M500K, "5500" },
                                                                 { Bitrate.Bitrate6M, "6000" },
                                                                 { Bitrate.Bitrate6M500K, "6500" },
                                                                 { Bitrate.Bitrate7M, "7000" },
                                                                 { Bitrate.Bitrate7M500K, "7500" },
                                                                 { Bitrate.Bitrate8M, "8000" },
                                                                 { Bitrate.Bitrate10M, "10000" },
                                                                 { Bitrate.Bitrate12M, "12000" },
                                                                 { Bitrate.Bitrate14M, "14000" },
                                                                 { Bitrate.Bitrate15M, "15000" },
                                                                 { Bitrate.Bitrate16M, "16000" },
                                                                 { Bitrate.Bitrate18M, "18000" },
                                                                 { Bitrate.Bitrate20M, "20000" },
                                                                 { Bitrate.Bitrate24M, "24000" },
                                                                 { Bitrate.Bitrate28M, "28000" },
                                                                 { Bitrate.Bitrate32M, "32000" },
                                                                 { Bitrate.Bitrate100M, "100000" }
                                                             };

        public static readonly Dictionary<Bitrate, String> DisplayList = new Dictionary<Bitrate, String>
                                                             {
                                                                 { Bitrate.NA, "N/A" },
                                                                 { Bitrate.Bitrate2K, "2K" },
                                                                 { Bitrate.Bitrate5K, "5K" },
                                                                 { Bitrate.Bitrate10K, "10K" },
                                                                 { Bitrate.Bitrate20K, "20K" },
                                                                 { Bitrate.Bitrate28K, "28K" },
                                                                 { Bitrate.Bitrate30K, "30K" },
                                                                 { Bitrate.Bitrate32K, "32K" },
                                                                 { Bitrate.Bitrate40K, "40K" },
                                                                 { Bitrate.Bitrate48K, "48K" },
                                                                 { Bitrate.Bitrate50K, "50K" },
                                                                 { Bitrate.Bitrate56K, "56K" },
                                                                 { Bitrate.Bitrate64K, "64K" },
                                                                 { Bitrate.Bitrate96K, "96K" },
                                                                 { Bitrate.Bitrate128K, "128K" },
                                                                 { Bitrate.Bitrate192K, "192K" },
                                                                 { Bitrate.Bitrate200K, "200K" },
                                                                 { Bitrate.Bitrate256K, "256K" },
                                                                 { Bitrate.Bitrate320K, "320K" },
                                                                 { Bitrate.Bitrate384K, "384K" },
                                                                 { Bitrate.Bitrate448K, "448K" },
                                                                 { Bitrate.Bitrate500K, "500K" },
                                                                 { Bitrate.Bitrate512K, "512K" },
                                                                 { Bitrate.Bitrate576K, "576K" },
                                                                 { Bitrate.Bitrate640K, "640K" },
                                                                 { Bitrate.Bitrate704K, "704K" },
                                                                 { Bitrate.Bitrate750K, "750K" },
                                                                 { Bitrate.Bitrate768K, "768K" },
                                                                 { Bitrate.Bitrate1M, "1M" },
                                                                 { Bitrate.Bitrate1M200K, "1.2M" },
                                                                 { Bitrate.Bitrate1M250K, "1.25M" },
                                                                 { Bitrate.Bitrate1M500K, "1.5M" },
                                                                 { Bitrate.Bitrate1M750K, "1.75M" },
                                                                 { Bitrate.Bitrate2M, "2M" },
                                                                 { Bitrate.Bitrate2M500K, "2.5M" },
                                                                 { Bitrate.Bitrate3M, "3M" },
                                                                 { Bitrate.Bitrate3M500K, "3.5M" },
                                                                 { Bitrate.Bitrate4M, "4M" },
                                                                 { Bitrate.Bitrate4M500K, "4.5M" },
                                                                 { Bitrate.Bitrate5M, "5M" },
                                                                 { Bitrate.Bitrate5M500K, "5.5M" },
                                                                 { Bitrate.Bitrate6M, "6M" },
                                                                 { Bitrate.Bitrate6M500K, "6.5M" },
                                                                 { Bitrate.Bitrate7M, "7M" },
                                                                 { Bitrate.Bitrate7M500K, "7.5M" },
                                                                 { Bitrate.Bitrate8M, "8M" },
                                                                 { Bitrate.Bitrate10M, "10M" },
                                                                 { Bitrate.Bitrate12M, "12M" },
                                                                 { Bitrate.Bitrate14M, "14M" },
                                                                 { Bitrate.Bitrate15M, "15M" },
                                                                 { Bitrate.Bitrate16M, "16M" },
                                                                 { Bitrate.Bitrate18M, "18M" },
                                                                 { Bitrate.Bitrate20M, "20M" },
                                                                 { Bitrate.Bitrate24M, "24M" },
                                                                 { Bitrate.Bitrate28M, "28M" },
                                                                 { Bitrate.Bitrate32M, "32M" },
                                                                 { Bitrate.Bitrate100M, "100M" }
                                                             };
    }
}