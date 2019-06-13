using System.Collections.Generic;
using DeviceTree.GPS.Objects;

namespace DeviceTree.GPS
{
	public class ViewBaseGPS : ViewBase
	{
		protected new Queue<GPSControl> RecycleDevice = new Queue<GPSControl>();

		public override void UpdateView()
		{
			UpdateView("ID");
		}
	}
}
