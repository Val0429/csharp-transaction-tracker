using System;
using System.Collections.Generic;

namespace Constant
{
    public enum AlarmStatus : short
    {
        Open,
        Acknowledged,
        Processing,
        Closed,

        All
    }

    public static class AlarmStatusMgr
    {
        public static readonly Dictionary<AlarmStatus, String> List =
            new Dictionary<AlarmStatus, String>
			{
				{AlarmStatus.Open, "Open"},
				{AlarmStatus.Acknowledged, "Acknowledged"},
				{AlarmStatus.Processing, "Processing"},
                {AlarmStatus.Closed, "Closed"},
                {AlarmStatus.All, "All"},
			};

        public static AlarmStatus ToIndex(String value)
        {
            foreach (KeyValuePair<AlarmStatus, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static String ToString(AlarmStatus index)
        {
            foreach (KeyValuePair<AlarmStatus, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }
    }
}
