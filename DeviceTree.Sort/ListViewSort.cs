using System;
using System.Collections.Generic;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.Sort
{
	public class ListViewSort : ListView
	{
		public override void UpdateView(String sort)
		{
			if (NVR == null || NVR.Device.Devices == null) return;

			ClearViewModel();

			switch (sort)
			{
				case "ID":
					GenerateViewModel();
					break;
				case "NAME":
					GenerateViewModelByName();
					break;
				case "GROUP":
					GenerateViewModelByGroup();
					break;
				case "STATUS":
					GenerateViewModelByStatus();
					break;
			}
		}
		
		protected void GenerateViewModelByName()
		{
			ViewModelPanel.Visible = false;

			List<IDeviceGroup> sortResult = DeviceGroupsSortById();

			foreach (IDeviceGroup deviceGroup in sortResult)
			{
				if (deviceGroup.Items.Count == 0) continue;

				GroupControl groupControl = GetGroupControl();

				groupControl.DeviceGroup = deviceGroup;
				ViewModelPanel.Controls.Add(groupControl);

				//deviceGroup.Items.Sort((x, y) => (y.Id - x.Id));
				deviceGroup.Items.Sort((x, y) => (y.Name.CompareTo(x.Name)));
				List<IDevice> list = new List<IDevice>(deviceGroup.Items);

				foreach (IDevice device in list)
				{
					if (device != null)
					{
						var deviceControl = GetDeviceControl();

						deviceControl.Device = device;
						groupControl.DeviceControlContainer.Controls.Add(deviceControl as DeviceControl);
					}
				}
			}

			ViewModelPanel.Visible = true;
		}

		protected void GenerateViewModelByGroup()
		{
			ViewModelPanel.Visible = false;
			List<IDeviceGroup> sortResult = DeviceGroupsSortByName();

			foreach (IDeviceGroup deviceGroup in sortResult)
			{
				if (deviceGroup.Items.Count == 0) continue;

				GroupControl groupControl = GetGroupControl();

				groupControl.DeviceGroup = deviceGroup;
				ViewModelPanel.Controls.Add(groupControl);

				deviceGroup.Items.Sort((x, y) => (y.Id - x.Id));
				List<IDevice> list = new List<IDevice>(deviceGroup.Items);

				foreach (IDevice device in list)
				{
					if (device != null)
					{
						var deviceControl = GetDeviceControl();

						deviceControl.Device = device;
						groupControl.DeviceControlContainer.Controls.Add(deviceControl as DeviceControl);
					}
				}
			}

			ViewModelPanel.Visible = true;
		}

		protected void GenerateViewModelByStatus()
		{
			ViewModelPanel.Visible = false;

			List<IDeviceGroup> sortResult = DeviceGroupsSortById();

			foreach (IDeviceGroup deviceGroup in sortResult)
			{
				if (deviceGroup.Items.Count == 0) continue;

				GroupControl groupControl = GetGroupControl();

				groupControl.DeviceGroup = deviceGroup;
				ViewModelPanel.Controls.Add(groupControl);

				deviceGroup.Items.Sort((x, y) => (((ICamera) x).Status - ((ICamera) y).Status));
				List<IDevice> list = new List<IDevice>(deviceGroup.Items);

				foreach (IDevice device in list)
				{
					if (device != null)
					{
						var deviceControl = GetDeviceControl();

						deviceControl.Device = device;
						groupControl.DeviceControlContainer.Controls.Add(deviceControl as DeviceControl);
					}
				}
			}

			ViewModelPanel.Visible = true;
		}
		
		protected List<IDeviceGroup> DeviceGroupsSortByName()
		{
			List<IDeviceGroup> sortResult = new List<IDeviceGroup>(NVR.Device.Groups.Values);

			//reverse
			sortResult.Sort((x, y) => (y.Name.CompareTo(x.Name)));

			return sortResult;
		}
	}
}
