﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree
{
    public class NVRListView : ViewBase
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
            if (OnDeviceMouseDown != null)
            {
                OnDeviceMouseDown(sender, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDoubleClick;
        protected void RaiseOnDeviceMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDoubleClick != null)
            {
                OnDeviceMouseDoubleClick(sender, e);
            }
        }

        public event MouseEventHandler OnGroupMouseDown;
        protected void RaiseOnGroupMouseDown(object sender, MouseEventArgs e)
        {
            if (OnGroupMouseDown != null)
            {
                OnGroupMouseDown(sender, e);
            }
        }

        public event MouseEventHandler OnGroupMouseDrag;
        protected void RaiseOnGroupMouseDrag(object sender, MouseEventArgs e)
        {
            if (OnGroupMouseDrag != null)
            {
                OnGroupMouseDrag(sender, e);
            }
        }
        public event MouseEventHandler OnGroupMouseDoubleClick;
        protected void RaiseOnGroupMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnGroupMouseDoubleClick != null)
            {
                OnGroupMouseDoubleClick(sender, e);
            }
        }

        public event MouseEventHandler OnNVRMouseDown;
        protected void RaiseOnNVRMouseDown(object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDown != null)
            {
                OnNVRMouseDown(sender, e);
            }
        }
        public event MouseEventHandler OnNVRMouseDrag;
        protected void RaiseOnNVRMouseDrag(object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDrag != null)
            {
                OnNVRMouseDrag(sender, e);
            }
        }
        public event MouseEventHandler OnNVRMouseDoubleClick;
        protected void RaiseOnNVRMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDoubleClick != null)
            {
                OnNVRMouseDoubleClick(sender, e);
            }
        }

        public ICMS CMS;
        public IPTS PTS;
        public override void GenerateViewModel()
        {
            var sortResult = NVRsSortById();

            var nvrControls = new List<NVRControl>();
            foreach (IServer nvr in sortResult)
            {
                if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;

                NVRControl nvrControl = GetNVRControl();

                nvrControl.NVR = nvr;
                nvrControls.Add(nvrControl);

                var groupSortResult = DeviceGroupsSortById(nvr);

                var groupControls = new List<GroupControl>();
                foreach (IDeviceGroup deviceGroup in groupSortResult)
                {
                    if (deviceGroup.Items.Count == 0) continue;

                    var groupControl = GetGroupControl();

                    groupControl.DeviceGroup = deviceGroup;
                    groupControls.Add(groupControl);

                    var list = new List<IDevice>(deviceGroup.Items);
                    list.Sort(SortByIdThenNVR);
                    var deviceControls = new List<DeviceControl>();
                    foreach (IDevice device in list)
                    {
                        if (device == null) continue;
                        var deviceControl = GetDeviceControl();

                        deviceControl.Device = device;
                        deviceControls.Add(deviceControl);
                    }
                    if (deviceControls.Count > 0)
                        groupControl.DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                }
                if (groupControls.Count > 0)
                    nvrControl.GroupControlContainer.Controls.AddRange(groupControls.ToArray());
            }
            if (nvrControls.Count > 0)
                ViewModelPanel.Controls.AddRange(nvrControls.ToArray());
        }

        protected List<IServer> NVRsSortById()
        {
            var sortResult = new List<IServer>();

            if (CMS != null)
            {
                foreach (INVR nvr in CMS.NVR.NVRs.Values)
                {
                    sortResult.Add(nvr);
                }
            }

            if (PTS != null)
            {
                foreach (INVR nvr in PTS.NVR.NVRs.Values)
                {
                    sortResult.Add(nvr);
                }
            }
            //reverse
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (CMS != null)
            {
                var hasDevice = CMS.Device.Groups.Any(group => group.Value.Items.Any());

                if (hasDevice)
                    sortResult.Add(CMS);
            }

            return sortResult;
        }

        protected List<IDeviceGroup> DeviceGroupsSortById(IServer nvr)
        {
            var sortResult = new List<IDeviceGroup>(nvr.Device.Groups.Values);

            //reverse
            sortResult.Sort((x, y) => (y.Id - x.Id));

            return sortResult;
        }

        private static Int32 SortByIdThenNVR(IDevice x, IDevice y)
        {
            if (x.Id != y.Id)
                return (y.Id - x.Id);

            return (y.Server.Id - x.Server.Id);
        }

        protected Queue<NVRControl> RecycleNVR = new Queue<NVRControl>();
        protected Queue<GroupControl> RecycleGroup = new Queue<GroupControl>();
        protected Queue<DeviceControl> RecycleDevice = new Queue<DeviceControl>();
        private NVRControl GetNVRControl()
        {
            if (RecycleNVR.Count > 0)
            {
                return RecycleNVR.Dequeue();
            }

            var nvrControl = new NVRControl();
            nvrControl.OnNVRMouseDown += RaiseOnNVRMouseDown;
            nvrControl.OnNVRMouseDrag += RaiseOnNVRMouseDrag;
            nvrControl.OnNVRMouseDoubleClick += RaiseOnNVRMouseDoubleClick;

            return nvrControl;
        }

        private GroupControl GetGroupControl()
        {
            if (RecycleGroup.Count > 0)
            {
                return RecycleGroup.Dequeue();
            }

            var groupControl = new GroupControl();

            groupControl.OnGroupMouseDown += RaiseOnGroupMouseDown;
            groupControl.OnGroupMouseDrag += RaiseOnGroupMouseDrag;
            groupControl.OnGroupMouseDoubleClick += RaiseOnGroupMouseDoubleClick;

            return groupControl;
        }

        private DeviceControl GetDeviceControl()
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
            foreach (NVRControl nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.Invalidate();
                foreach (GroupControl groupControl in nvrControl.GroupControlContainer.Controls)
                {
                    groupControl.Invalidate();
                    foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
                    {
                        deviceControl.Invalidate();
                    }
                }
            }
        }

        public override void UpdateRecordingStatus()
        {
            foreach (NVRControl nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.UpdateRecordingStatus();
            }
        }

        public override void UpdateView()
        {
            UpdateView("ID");
        }

        public override void UpdateView(String sort)
        {
            if (CMS == null && PTS == null) return;
            if (CMS != null && CMS.NVR.NVRs == null) return;
            if (PTS != null && PTS.NVR.NVRs == null) return;

            ClearNVRViewModel();
            GenerateViewModel();
        }

        public override void UpdateToolTips()
        {
            foreach (NVRControl nvrControl in ViewModelPanel.Controls)
            {
                foreach (GroupControl groupControl in nvrControl.GroupControlContainer.Controls)
                {
                    foreach (DeviceControl deviceControl in groupControl.DeviceControlContainer.Controls)
                    {
                        deviceControl.UpdateToolTips();
                    }
                }
            }
        }

        protected void ClearNVRViewModel()
        {
            if (ViewModelPanel == null) return;

            foreach (NVRControl nvrControl in ViewModelPanel.Controls)
            {
                if (!RecycleNVR.Contains(nvrControl))
                    RecycleNVR.Enqueue(nvrControl);

                foreach (GroupControl groupControl in nvrControl.GroupControlContainer.Controls)
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

                nvrControl.GroupControlContainer.Controls.Clear();
            }

            ViewModelPanel.Controls.Clear();
        }
    }
}