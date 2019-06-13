using System;
using System.Collections.Generic;
using System.Drawing;
using Constant;

namespace Interface
{
	public interface IDeviceLayout : IDevice
	{
		String LiveGUID { get;  }
		String PlaybackGUID { get; }
		Int32 WindowWidth { get; set; }
		Int32 WindowHeight { get; set; }
		Int32 Width { get; }
		Int32 Height { get; }
		UInt16 LayoutX { get; set; }
		UInt16 LayoutY { get; set; }
        List<IDevice> Items { get; set; }
        List<Boolean> Dewarps { get; set; }
        Dictionary<UInt16, ISubLayout> SubLayouts { get; set; }

		Boolean isImmerVision { get; set; }
		Boolean isIncludeDevice { get; set; }
		String DefineLayout { get; set; }
		MountingType MountingType { get; set; }
		LensSetting LensSetting { get; set; }
	}

	public interface ISubLayout : IDevice
	{
		IDeviceLayout DeviceLayout { get; set; }
		Int32 X { get; set; }
		Int32 Y { get; set; }
		Int32 Width { get; set; }
		Int32 Height { get; set; }
        Int32 Dewarp { get; set; }
	}
	
	public class SubLayoutUtility
	{
		public static Int32 CheckSubLayoutRelativeCamera(ISubLayout subLayout)
		{
			var fillPercent = 0;
			var targetItem = -1;
			var count = 0;
			for (var layoutY = 0; layoutY < subLayout.DeviceLayout.LayoutY; layoutY++)
			{
				for (var layoutX = 0; layoutX < subLayout.DeviceLayout.LayoutX; layoutX++)
				{
					var target = new Rectangle(
						layoutX * subLayout.DeviceLayout.WindowWidth,
						layoutY * subLayout.DeviceLayout.WindowHeight,
						subLayout.DeviceLayout.WindowWidth,
						subLayout.DeviceLayout.WindowHeight);

					var width = Math.Min(target.Width + target.X, subLayout.Width + subLayout.X) - Math.Max(target.X, subLayout.X);
					var height = Math.Min(target.Height + target.Y, subLayout.Height + subLayout.Y) - Math.Max(target.Y, subLayout.Y);

					if (width < 0) width = 0;
					if (height < 0) height = 0;

					if (width * height > fillPercent && (subLayout.DeviceLayout.Items.Count > count && subLayout.DeviceLayout.Items[count] != null))
					{
						fillPercent = width * height;
						targetItem = count;
					}

					count++;
				}
			}

			if (targetItem != -1 && subLayout.DeviceLayout.Items.Count > targetItem && subLayout.DeviceLayout.Items[targetItem] != null)
				return subLayout.DeviceLayout.Items[targetItem].Id;
			return -1;
		}
	}
}
