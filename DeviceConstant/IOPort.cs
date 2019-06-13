using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum IOPort : ushort
    {
        NonSpecific = 0,
        Input = 1,
        Output = 2,
        PIR = 3
    }

    public static class IOPorts 
    {
        public static IOPort ToIndex(String value)
        {
            foreach (KeyValuePair<IOPort, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return IOPort.NonSpecific;
        }

        public static String ToString(IOPort index)
        {
            foreach (KeyValuePair<IOPort, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<IOPort, String> List = new Dictionary<IOPort, String>
                                                             {
                                                                 { IOPort.Input, "Input" },
                                                                 { IOPort.Output, "Output" },
                                                                 { IOPort.PIR, "PIR" },
                                                             };
    }
}