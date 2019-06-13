
using System.Collections.Generic;
using System.Windows.Forms;
using GPSTree.Objects;
using Interface;

namespace GPSTree
{
	public class ListView : ViewBase
	{
		public event MouseEventHandler OnDeviceMouseDrag;
		public event MouseEventHandler OnDeviceMouseDoubleClick;

		public event MouseEventHandler OnGroupMouseDrag;
		public event MouseEventHandler OnGroupMouseDoubleClick;

		protected override void GenerateViewModel()
		{
			ViewModelPanel.Visible = false;
			var sortResult = DeviceGroupsSortById();
			sortResult.Reverse();

			foreach (var deviceGroup in sortResult)
			{
				if (deviceGroup.Items.Count == 0) continue;

				if (deviceGroup.Id != 0) continue;

				var groupControl = GetGroupControl();

				groupControl.DeviceGroup = deviceGroup;
				ViewModelPanel.Controls.Add(groupControl);

				deviceGroup.Items.Sort((x, y) => (y.Id - x.Id));
				var list = new List<IDevice>(deviceGroup.Items);

				foreach (IDevice device in list)
				{
					if (device == null) continue;

					var deviceControl = GetDeviceControl();
					deviceControl.Device = device;
					groupControl.DeviceControlContainer.Controls.Add(deviceControl);
				}
			}

			ViewModelPanel.Visible = true;
		}

		private GroupControl GetGroupControl()
		{
			if (RecycleGroup.Count > 0)
			{
				return RecycleGroup.Dequeue();
            }

            GroupControl groupControl = new GroupControl();

            groupControl.OnGroupMouseDrag += OnGroupMouseDrag;
            groupControl.OnGroupMouseDoubleClick += OnGroupMouseDoubleClick;

			return groupControl;
		}

		private GPSControl GetDeviceControl()
		{
			if (RecycleDevice.Count > 0)
			{
				return RecycleDevice.Dequeue();
            }

            GPSControl deviceControl = new GPSControl();
            deviceControl.OnDeviceMouseDrag += OnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick += OnDeviceMouseDoubleClick;

			return deviceControl;
		}
	}
}