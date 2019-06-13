using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum Position : ushort
    {
        NonSpecific = 0,
        RightTop = 1,
        RightBottom = 2,
        LeftTop = 3,
        LeftBottom = 4
    }

    public static class Positions
    {
        public static Position ToIndex(String value)
        {
            foreach (KeyValuePair<Position, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return Position.NonSpecific;
        }

        public static String ToString(Position index)
        {
            foreach (KeyValuePair<Position, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static Position ToXMLIndex(String value)
        {
            foreach (KeyValuePair<Position, String> keyValuePair in XmlList)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return Position.NonSpecific;
        }

        public static String ToXMLString(Position index)
        {
            foreach (KeyValuePair<Position, String> keyValuePair in XmlList)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<Position, String> List = new Dictionary<Position, String>
                                                             {
                                                                 { Position.RightTop, "Right Top" },
                                                                 { Position.RightBottom, "Right Bottom" },
                                                                 { Position.LeftTop, "Left Top" },
                                                                 { Position.LeftBottom, "Left Bottom" },
                                                             };

        public static readonly Dictionary<Position, String> XmlList = new Dictionary<Position, String>
                                                             {
                                                                 { Position.RightTop, "RightTop" },
                                                                 { Position.RightBottom, "RightBottom" },
                                                                 { Position.LeftTop, "LeftTop" },
                                                                 { Position.LeftBottom, "LeftBottom" },
                                                             };
    }
}