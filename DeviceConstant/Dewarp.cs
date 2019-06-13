using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum Dewarp : ushort
    {
        NonSpecific = 0,
        Dewarp1O = 1,
        Dewarp1P = 2,
        Dewarp2P = 3,
        Dewarp1R = 4,
        Dewarp4R = 5,
        DewarpViewArea1 = 6,
        DewarpViewArea2 = 7,
        DewarpViewArea3 = 8,
        DewarpViewArea4 = 9,
    }

    public static class Dewarps 
    {
        public static Dewarp ToIndex(String value)
        {
            foreach (KeyValuePair<Dewarp, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return Dewarp.NonSpecific;
        }

        public static String ToString(Dewarp index)
        {
            foreach (KeyValuePair<Dewarp, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<Dewarp, String> List = new Dictionary<Dewarp, String>
                                                             {
                                                                 { Dewarp.NonSpecific, "" },
                                                                 { Dewarp.Dewarp1O, "1O" },
                                                                 { Dewarp.Dewarp1P, "1P" },
                                                                 { Dewarp.Dewarp2P, "2P" },
                                                                 { Dewarp.Dewarp1R, "1R" },
                                                                 { Dewarp.Dewarp4R, "4R" },
                                                                 { Dewarp.DewarpViewArea1, "View Area 1" },
                                                                 { Dewarp.DewarpViewArea2, "View Area 2" },
                                                                 { Dewarp.DewarpViewArea3, "View Area 3" },
                                                                 { Dewarp.DewarpViewArea4, "View Area 4" }
                                                             };

    }
}