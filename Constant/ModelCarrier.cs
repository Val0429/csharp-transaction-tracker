using System;
using System.Collections.Generic;

namespace Constant
{
	public enum ModelCarrier : ushort
	{
		Unknow = 0,
		Vehicle = 1,
		Personal = 2,
	}

	public class ModelCarriers
	{
		public static readonly Dictionary<ModelCarrier, String> List = new Dictionary<ModelCarrier, String>
                                                             {
                                                                 { ModelCarrier.Unknow, ""},
                                                                 { ModelCarrier.Vehicle, "Vehicle" },
                                                                 { ModelCarrier.Personal, "Personal" },
                                                             };

		public static ModelCarrier ToIndex(String value)
		{
			foreach (KeyValuePair<ModelCarrier, String> keyValuePair in List)
			{
				if (String.Equals(value, keyValuePair.Value))
					return keyValuePair.Key;
			}

			return 0;
		}

		public static String ToString(ModelCarrier index)
		{
			foreach (KeyValuePair<ModelCarrier, String> keyValuePair in List)
			{
				if (index == keyValuePair.Key)
					return keyValuePair.Value;
			}

			return "";
		}
	}
}
