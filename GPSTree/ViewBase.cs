using System;
using System.Collections.Generic;
using GPSTree.Objects;
using Interface;
using PanelBase;

namespace GPSTree
{
    public class ViewBase
    {
        public String Name;
        public DoubleBufferPanel ViewModelPanel;

        public ICMS CMS;
        public INVR NVR;
        protected Queue<GroupControl> RecycleGroup = new Queue<GroupControl>();
        protected Queue<GPSControl> RecycleDevice = new Queue<GPSControl>();

        public void UpdateView()
        {
            if (NVR == null || NVR.Device.Devices == null) return;

            ClearViewModel();

            GenerateViewModel();
        }

        public void UpdateNVRView()
        {
//            if (CMS == null || CMS.NVR.NVRs == null) return;
//            ClearNVRViewModel();
//            GenerateViewModel();
        }

        public void UpdateNVRRecordingStatus()
        {
//            foreach (NVRControl nvrControl in ViewModelPanel.Controls)
//            {
//                nvrControl.UpdateRecordingStatus();
//            }
        }

        public void UpdateRecordingStatus()
        {
            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                groupControl.UpdateRecordingStatus();
            }
        }

        protected List<INVR> NVRsSortById()
        {
            List<INVR> sortResult = new List<INVR>(CMS.NVR.NVRs.Values);

            sortResult.Sort((x, y) => (x.Id - y.Id));

            return sortResult;
        }

        protected List<IDeviceGroup> DeviceGroupsSortById()
        {
            List<IDeviceGroup> sortResult = new List<IDeviceGroup>(NVR.Device.Groups.Values);

            sortResult.Sort((x, y) => (x.Id - y.Id));

            return sortResult;
        }

        protected List<IDeviceGroup> DeviceGroupsSortById(INVR nvr)
        {
            List<IDeviceGroup> sortResult = new List<IDeviceGroup>(nvr.Device.Groups.Values);

            sortResult.Sort((x, y) => (x.Id - y.Id));

            return sortResult;
        }

        protected virtual void GenerateViewModel()
        {
            //List<IDeviceGroup> sortResult = DeviceGroupsSortById();

            //foreach (IDeviceGroup deviceGroup in sortResult)
            //{
            //    Console.WriteLine(@"Group Name: " + deviceGroup.Name);

            //    deviceGroup.Items.Sort();
            //    foreach (UInt16 id in deviceGroup.Items)
            //    {
            //        IDevice device = NVR.Device.FindDeviceById(id);
            //        if (device != null)
            //            Console.WriteLine(@"Device Name: " + device.Name);    
            //    }
            //}
        }

        protected void ClearNVRViewModel()
        {
			//if(ViewModelPanel == null) return;

			//foreach (NVRControl nvrControl in ViewModelPanel.Controls)
			//{
			//    if (!RecycleNVR.Contains(nvrControl))
			//        RecycleNVR.Add(nvrControl);

			//    foreach (GroupControl groupControl in nvrControl.GroupControlContainer.Controls)
			//    {
			//        if (!RecycleGroup.Contains(groupControl))
			//            RecycleGroup.Add(groupControl);

			//        foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
			//        {
			//            if (!RecycleDevice.Contains(deviceControl))
			//            {
			//                deviceControl.Device = null;
			//                RecycleDevice.Add(deviceControl);
			//            }
			//        }

			//        groupControl.DeviceControlContainer.Controls.Clear();
			//    }

			//    nvrControl.GroupControlContainer.Controls.Clear();
			//}

			//ViewModelPanel.Controls.Clear();
        }

        protected void ClearViewModel()
        {
            if(ViewModelPanel == null) return;

            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                if (!RecycleGroup.Contains(groupControl))
                    RecycleGroup.Enqueue(groupControl);

                foreach (GPSControl deviceControl in groupControl.DeviceControlContainer.Controls)
                {
                    if (!RecycleDevice.Contains(deviceControl))
                    {
                        deviceControl.Device = null;
                        RecycleDevice.Enqueue(deviceControl);
                    }
                }

                groupControl.DeviceControlContainer.Controls.Clear();
            }

            ViewModelPanel.Controls.Clear();
        }
    }
}
