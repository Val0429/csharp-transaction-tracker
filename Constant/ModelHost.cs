using System;
using System.Collections.Generic;

namespace Constant
{
	public enum ModelHost: ushort 
	{
		Unknow = 0,
		Vehicle = 1,
		Personal = 2,
	}

	public class ModelHosts
	{
		public static readonly Dictionary<ModelHost, String> List = new Dictionary<ModelHost, String>
                                                             {
                                                                 { ModelHost.Unknow, ""},
                                                                 { ModelHost.Vehicle, "Vehicle" },
                                                                 { ModelHost.Personal, "Personal" },
                                                             };

		public static ModelHost ToIndex(String value)
		{
			foreach (KeyValuePair<ModelHost, String> keyValuePair in List)
			{
				if (String.Equals(value, keyValuePair.Value))
					return keyValuePair.Key;
			}

			return 0;
		}

		public static String ToString(ModelHost index)
		{
			foreach (KeyValuePair<ModelHost, String> keyValuePair in List)
			{
				if (index == keyValuePair.Key)
					return keyValuePair.Value;
			}

			return "";
		}
	}
}
