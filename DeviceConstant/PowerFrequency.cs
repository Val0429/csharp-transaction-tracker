using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum PowerFrequency : ushort
    {
        NonSpecific = 0,
        Hertz50 = 1,
        Hertz60 = 2,
        Outdoor = 3,
    }

    public static class PowerFrequencies 
    {
        public static PowerFrequency ToIndex(String value)
        {
            foreach (KeyValuePair<PowerFrequency, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return PowerFrequency.NonSpecific;
        }

        public static PowerFrequency DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<PowerFrequency, String> keyValuePair in List2)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return PowerFrequency.NonSpecific;
        }

        public static String ToString(PowerFrequency index)
        {
            foreach (KeyValuePair<PowerFrequency, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(PowerFrequency index)
        {
            foreach (KeyValuePair<PowerFrequency, String> keyValuePair in List2)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<PowerFrequency, String> List = new Dictionary<PowerFrequency, String>
                                                             {
                                                                 { PowerFrequency.Hertz50, "50" },
                                                                 { PowerFrequency.Hertz60, "60" },
                                                                 { PowerFrequency.Outdoor, "61" },
                                                             };

        public static readonly Dictionary<PowerFrequency, String> List2 = new Dictionary<PowerFrequency, String>
                                                             {
                                                                 { PowerFrequency.Hertz50, "50Hz" },
                                                                 { PowerFrequency.Hertz60, "60Hz" },
                                                                 { PowerFrequency.Outdoor, "Outdoor" },
                                                             };
    }
}