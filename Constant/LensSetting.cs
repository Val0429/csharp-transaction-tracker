using System;
using System.Collections.Generic;

namespace Constant
{
	public enum LensSetting : ushort
	{
		PTZ = 1,
		Quad = 2,
        Perimeter = 3,
	}

	public static class LensSettings
	{
		public static readonly Dictionary<LensSetting, String> List =
			new Dictionary<LensSetting, String>
			{
				{LensSetting.PTZ, "PTZ"},
				{LensSetting.Quad, "Quad"},
				{LensSetting.Perimeter, "Perimeter"},
			};

		public static LensSetting ToIndex(String value)
		{
			foreach (KeyValuePair<LensSetting, String> keyValuePair in List)
			{
				if (String.Equals(value, keyValuePair.Value))
					return keyValuePair.Key;
			}

			return 0;
		}

		public static String ToString(LensSetting index)
		{
			foreach (KeyValuePair<LensSetting, String> keyValuePair in List)
			{
				if (index == keyValuePair.Key)
					return keyValuePair.Value;
			}

			return "";
		}
	}
}
