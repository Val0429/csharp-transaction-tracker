using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.View
{
    public class ListView : ViewBase
    {
        public event MouseEventHandler OnDeviceMouseDrag;

        protected void RaiseOnDeviceMouseDrag(object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDrag != null)
            {
                OnDeviceMouseDrag(sender, e);
            }
        }

        public event MouseEventHandler OnDeviceMouseDown;
        protected void RaiseOnDeviceMouseDown(object sender, MouseEventArgs e)
        {
            RaiseOnDeviceMouseDown(e);
        }

        protected void RaiseOnDeviceMouseDown(MouseEventArgs e)
        {
            if (OnDeviceMouseDown != null)
            {
                OnDeviceMouseDown(this, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDoubleClick;
        protected void RaiseOnDeviceMouseDoubleClick(object sender, MouseEventArgs e)
        {
            RaiseOnDeviceMouseDoubleClick(e);
        }

        protected void RaiseOnDeviceMouseDoubleClick(MouseEventArgs e)
        {
            if (OnDeviceMouseDoubleClick != null)
            {
                OnDeviceMouseDoubleClick(this, e);
            }
        }

        public event MouseEventHandler OnGroupMouseDrag;
        protected void RaiseOnGroupMouseDrag(object sender, MouseEventArgs e) { RaiseOnGroupMouseDrag(e); }
        protected void RaiseOnGroupMouseDrag(MouseEventArgs e)
        {
            if (OnGroupMouseDrag != null)
            {
                OnGroupMouseDrag(this, e);
            }
        }
        public event MouseEventHandler OnGroupMouseDown;
        protected void RaiseOnGroupMouseDown(object sender, MouseEventArgs e) { RaiseOnGroupMouseDown(e); }
        protected void RaiseOnGroupMouseDown(MouseEventArgs e)
        {
            if (OnGroupMouseDown != null)
            {
                OnGroupMouseDown(this, e);
            }
        }
        public event MouseEventHandler OnGroupMouseDoubleClick;
        protected void RaiseOnGroupMouseDoubleClick(object sender, MouseEventArgs e) { RaiseOnGroupMouseDoubleClick(e); }
        protected void RaiseOnGroupMouseDoubleClick(MouseEventArgs e)
        {
            if (OnGroupMouseDoubleClick != null)
            {
                OnGroupMouseDoubleClick(this, e);
            }
        }

        public INVR NVR { get; set; }


        public override void GenerateViewModel()
        {
            var groupControls = new List<GroupControl>();

            // User Define Device Group
            var sortResult = new List<IDeviceGroup>(NVR.User.Current.DeviceGroups.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            foreach (var deviceGroup in sortResult)
            {
                if (deviceGroup.Items.Count == 0) continue;

                var groupControl = GetGroupControl();

                groupControl.DeviceGroup = deviceGroup;
                groupControls.Add(groupControl);

                var list = new List<IDevice>(deviceGroup.Items);
                list.Sort((x, y) => (y.Id - x.Id));
                var deviceControls = new List<DeviceControl>();

                foreach (IDevice device in list)
                {
                    if (device == null) continue;
                    var deviceControl = GetDeviceControl();

                    deviceControl.Device = device;
                    deviceControls.Add(deviceControl as DeviceControl);
                }
                if (deviceControls.Count > 0)
                    groupControl.DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
            }

            sortResult = DeviceGroupsSortById();

            foreach (IDeviceGroup deviceGroup in sortResult)
            {
                if (deviceGroup.Items.Count == 0) continue;

                var groupControl = GetGroupControl();

                groupControl.DeviceGroup = deviceGroup;
                groupControls.Add(groupControl);

                var list = new List<IDevice>(deviceGroup.Items);
                list.Sort((x, y) => (y.Id - x.Id));
                var deviceControls = new List<DeviceControl>();

                foreach (IDevice device in list)
                {
                    if (device == null) continue;
                    var deviceControl = GetDeviceControl();

                    deviceControl.Device = device;
                    deviceControls.Add(deviceControl as DeviceControl);
                }
                if (deviceControls.Count > 0)
                {
                    groupControl.DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                    deviceControls.Clear();
                }
            }

            if (groupControls.Count > 0)
            {
                ViewModelPanel.Controls.AddRange(groupControls.ToArray());
                groupControls.Clear();
            }
        }

        protected List<IDeviceGroup> DeviceGroupsSortById()
        {
            var sortResult = new List<IDeviceGroup>(NVR.Device.Groups.Values);

            //reverse
            sortResult.Sort((x, y) => (y.Id - x.Id));

            return sortResult;
        }

        public override void UpdateView()
        {
            UpdateView("ID");
        }

        public override void UpdateView(String sort)
        {
            if (NVR == null || NVR.Device.Devices == null) return;

            ClearViewModel();

            GenerateViewModel();
        }

        public override void ClearViewModel()
        {
            if (ViewModelPanel == null) return;

            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                if (!RecycleGroup.Contains(groupControl))
                    RecycleGroup.Enqueue(groupControl);

                foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
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

        public override void UpdateToolTips()
        {
            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
                {
                    deviceControl.UpdateToolTips();
                }
            }
        }

        public override void UpdateRecordingStatus()
        {
            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                groupControl.UpdateRecordingStatus();
            }
        }

        protected Queue<GroupControl> RecycleGroup = new Queue<GroupControl>();
        protected Queue<DeviceControl> RecycleDevice = new Queue<DeviceControl>();
        public virtual GroupControl GetGroupControl()
        {
            if (RecycleGroup.Count > 0)
            {
                return RecycleGroup.Dequeue();
            }

            var groupControl = new GroupControl();

            groupControl.OnGroupMouseDrag += OnGroupMouseDrag;
            groupControl.OnGroupMouseDown += OnGroupMouseDown;
            groupControl.OnGroupMouseDoubleClick += OnGroupMouseDoubleClick;

            return groupControl;
        }

        public virtual IDeviceControl GetDeviceControl()
        {
            if (RecycleDevice.Count > 0)
            {
                return RecycleDevice.Dequeue();
            }

            var deviceControl = new DeviceControl();
            deviceControl.OnDeviceMouseDown += RaiseOnDeviceMouseDown;
            deviceControl.OnDeviceMouseDrag += RaiseOnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick += RaiseOnDeviceMouseDoubleClick;

            return deviceControl;
        }

        public override void Refresh()
        {
            foreach (GroupControl groupControl in ViewModelPanel.Controls)
            {
                groupControl.Invalidate();
                foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
                {
                    deviceControl.Invalidate();
                }
            }
        }
    }
}