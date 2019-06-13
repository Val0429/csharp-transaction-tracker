using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using DeviceTree.GPS.Objects;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.GPS
{
	public class ListViewGPS : ListView
	{
        public override event MouseEventHandler OnDeviceMouseDrag;
        public override event MouseEventHandler OnDeviceMouseDown;
        public override event MouseEventHandler OnDeviceMouseDoubleClick;

        public override event MouseEventHandler OnGroupMouseDrag;
        public override event MouseEventHandler OnGroupMouseDown;
        public override event MouseEventHandler OnGroupMouseDoubleClick;

    	public override void GenerateViewModel()
        {
            //ViewModelPanel.Visible = false;
            var sortResult = DeviceGroupsSortById();
			//sortResult.Reverse();

            var groupControls = new List<GroupControl>();
            foreach (IDeviceGroup deviceGroup in sortResult)
            {
                if(deviceGroup.Items.Count == 0) continue;

				if (deviceGroup.Id != 0) continue;

                var groupControl = GetGroupControl();

                groupControl.DeviceGroup = deviceGroup;
                groupControls.Add(groupControl);

                deviceGroup.Items.Sort((x, y) => (y.Id - x.Id));
                var list = new List<IDevice>(deviceGroup.Items);
                var deviceControls = new List<DeviceControl>();

                foreach (var device in list)
                {
                    if (device == null) continue;

                    var deviceControl = GetDeviceControl();

                    deviceControl.Device = device.CloneGPS();

					if (deviceControl.Device.GPSInfo.ModelHost == ModelHost.Unknow) continue;

                    deviceControls.Add(deviceControl as DeviceControl);
                }
                if (deviceControls.Count > 0)
                    groupControl.DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
            }
            if (groupControls.Count > 0)
                ViewModelPanel.Controls.AddRange(groupControls.ToArray());

            //ViewModelPanel.Visible = true;
        }

		public override IDeviceControl GetDeviceControl()
        {
            if (RecycleDevice.Count > 0)
            {
                return RecycleDevice.Dequeue();
            }

			var deviceControl = new GPSControl {NVR = NVR};

			deviceControl.OnDeviceMouseDown += OnDeviceMouseDown;
            deviceControl.OnDeviceMouseDrag += OnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick += OnDeviceMouseDoubleClick;
            
            return deviceControl;
        }

		public override void UpdateRecordingStatus()
		{
			foreach (GroupControl groupControl in ViewModelPanel.Controls)
			{
				//groupControl.UpdateRecordingStatus();

				foreach (GPSControl deviceControl in groupControl.DeviceControlContainer.Controls)
				{
				    if (!((deviceControl).Device is ICamera)) continue;
				    (deviceControl).Invalidate();
				}
			}
		}
    }
}