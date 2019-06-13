using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Device;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.View
{
    public class DeviceListView : ViewBase
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

        public event MouseEventHandler OnGroupMouseDrag;
        public event MouseEventHandler OnGroupMouseDown;
        public event MouseEventHandler OnGroupMouseDoubleClick;

        public event MouseEventHandler OnDeviceLayoutMouseDrag;
        public event MouseEventHandler OnDeviceLayoutMouseDown;
        public event MouseEventHandler OnDeviceLayoutMouseDoubleClick;

        public event MouseEventHandler OnSubLayoutMouseDrag;
        public event MouseEventHandler OnSubLayoutMouseDown;
        public event MouseEventHandler OnSubLayoutMouseDoubleClick;

        public Dictionary<String, String> Localization;

        public IApp App { set; get; }
        public ICMS CMS { set; get; }
        public INVR NVR { set; get; }


        public override void GenerateViewModel()
        {
            //------ Device Layout 
            if (App.isSupportImageStitching)
            {
                var layoutGroupControl = GetDeviceLayoutGroupControl();
                layoutGroupControl.NVR = NVR;
                layoutGroupControl.DeviceGroup = new DeviceGroup
                    {
                        Id = 0,
                        Name = Localization["DeviceTree_ImageStitching"]
                    };
                layoutGroupControl.CreateTree();

                if (layoutGroupControl.DeviceLayoutControlContainer.Controls.Count > 0)
                {
                    ViewModelPanel.Controls.Add(layoutGroupControl);
                }
            }
            //------- True Camera
            var groupControl = GetGroupControl();
            groupControl.DeviceGroup = NVR.Device.Groups[0]; //all device group
            groupControl.CreateTree();

            ViewModelPanel.Controls.Add(groupControl);
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

            foreach (Control control in ViewModelPanel.Controls)
            {
                var groupControl = control as GroupControlUI2;

                if (groupControl != null)
                {
                    if (!RecycleGroup.Contains(groupControl))
                        RecycleGroup.Enqueue(groupControl);

                    groupControl.ClearViewModel();
                    continue;
                }

                var deviceLayoutGroup = control as DeviceLayoutGroupControl;
                if (deviceLayoutGroup != null)
                {
                    if (!RecycleDeviceLayoutGroupControl.Contains(deviceLayoutGroup))
                        RecycleDeviceLayoutGroupControl.Enqueue(deviceLayoutGroup);

                    deviceLayoutGroup.Clear();

                    deviceLayoutGroup.DeviceLayoutControlContainer.Controls.Clear();
                    continue;
                }
            }

            ViewModelPanel.Controls.Clear();
        }

        public override void UpdateToolTips()
        {
            foreach (Control control in ViewModelPanel.Controls)
            {
                var groupControl = control as GroupControlUI2;

                if (groupControl != null)
                {
                    groupControl.UpdateToolTips();
                    continue;
                }

                var deviceLayoutGroup = control as DeviceLayoutGroupControl;
                if (deviceLayoutGroup != null)
                {
                    deviceLayoutGroup.UpdateToolTips();
                }
            }
        }

        public override void UpdateRecordingStatus()
        {
            foreach (Control control in ViewModelPanel.Controls)
            {
                var groupControl = control as GroupControlUI2;

                if (groupControl != null)
                {
                    groupControl.UpdateRecordingStatus();
                    continue;
                }

                var deviceLayoutGroupControl = control as DeviceLayoutGroupControl;
                if (deviceLayoutGroupControl != null)
                {
                    deviceLayoutGroupControl.UpdateRecordingStatus();
                }
            }
        }

        protected Queue<GroupControlUI2> RecycleGroup = new Queue<GroupControlUI2>();
        protected Queue<DeviceLayoutGroupControl> RecycleDeviceLayoutGroupControl = new Queue<DeviceLayoutGroupControl>();

        private GroupControlUI2 GetGroupControl()
        {
            var groupControl = CreateGroupControl();
            groupControl.OnDeviceMouseDrag -= OnDeviceMouseDrag;
            groupControl.OnDeviceMouseDrag += OnDeviceMouseDrag;

            groupControl.OnDeviceMouseDown -= OnDeviceMouseDown;
            groupControl.OnDeviceMouseDown += OnDeviceMouseDown;

            groupControl.OnDeviceMouseDoubleClick -= OnDeviceMouseDoubleClick;
            groupControl.OnDeviceMouseDoubleClick += OnDeviceMouseDoubleClick;

            groupControl.OnGroupMouseDrag -= OnGroupMouseDrag;
            groupControl.OnGroupMouseDrag += OnGroupMouseDrag;

            groupControl.OnGroupMouseDown -= OnGroupMouseDown;
            groupControl.OnGroupMouseDown += OnGroupMouseDown;

            groupControl.OnGroupMouseDoubleClick -= OnGroupMouseDoubleClick;
            groupControl.OnGroupMouseDoubleClick += OnGroupMouseDoubleClick;

            return groupControl;
        }

        protected virtual GroupControlUI2 CreateGroupControl()
        {
            if (RecycleGroup.Count > 0)
            {
                return RecycleGroup.Dequeue();
            }

            return new GroupControlUI2();
        }

        public DeviceLayoutGroupControl GetDeviceLayoutGroupControl()
        {
            if (RecycleDeviceLayoutGroupControl.Count > 0)
            {
                return RecycleDeviceLayoutGroupControl.Dequeue();
            }

            var groupControl = new DeviceLayoutGroupControl();

            groupControl.OnDeviceLayoutGroupMouseDrag += OnGroupMouseDrag;
            groupControl.OnDeviceLayoutGroupMouseDown += OnGroupMouseDown;
            groupControl.OnDeviceLayoutGroupMouseDoubleClick += OnGroupMouseDoubleClick;

            groupControl.OnDeviceLayoutMouseDrag += OnDeviceLayoutMouseDrag;
            groupControl.OnDeviceLayoutMouseDown += OnDeviceLayoutMouseDown;
            groupControl.OnDeviceLayoutMouseDoubleClick += OnDeviceLayoutMouseDoubleClick;

            groupControl.OnSubLayoutMouseDrag += OnSubLayoutMouseDrag;
            groupControl.OnSubLayoutMouseDown += OnSubLayoutMouseDown;
            groupControl.OnSubLayoutMouseDoubleClick += OnSubLayoutMouseDoubleClick;

            return groupControl;
        }

        public override void Refresh()
        {
            foreach (Control control in ViewModelPanel.Controls)
            {
                var groupControl = control as GroupControlUI2;

                if (groupControl != null)
                {
                    groupControl.Invalidate();
                    groupControl.UpdateRecordingStatus();
                    continue;
                }

                var deviceLayoutGroup = control as DeviceLayoutGroupControl;
                if (deviceLayoutGroup != null)
                {
                    deviceLayoutGroup.UpdateRecordingStatus();
                }
            }
        }
    }
}