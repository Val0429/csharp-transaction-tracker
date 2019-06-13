using System;
using System.Collections.Generic;

namespace Constant
{
	public enum MountingType : ushort
	{
		Wall = 0,
		Ground = 2,
		Ceiling = 1,
	}

	public static class MountingTypes
	{
		public static readonly Dictionary<MountingType, String> List = 
			new Dictionary<MountingType, String>
			{
				{MountingType.Wall, "Wall"},
				{MountingType.Ceiling, "Ceiling"},
				{MountingType.Ground, "Ground"},
			};

		public static MountingType ToIndex(String value)
		{
			foreach (KeyValuePair<MountingType, String> keyValuePair in List)
			{
				if (String.Equals(value, keyValuePair.Value))
					return keyValuePair.Key;
			}

			return 0;
		}

		public static Int16 ToIndex(MountingType index)
		{
			switch (index)
			{
				case MountingType.Wall:
					return 0;
				case MountingType.Ceiling:
					return 2;
				case MountingType.Ground:
					return 1;
			}
			return -1;
		}

		public static String ToString(MountingType index)
		{
			foreach (KeyValuePair<MountingType, String> keyValuePair in List)
			{
				if (index == keyValuePair.Key)
					return keyValuePair.Value;
			}

			return "";
		}
	}
}
