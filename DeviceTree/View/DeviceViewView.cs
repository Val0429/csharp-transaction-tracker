using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.View
{
    public class DeviceViewView : ViewBase
    {
        public event MouseEventHandler OnDeviceMouseDrag;
        protected void RaiseOnDeviceMouseDrag(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDrag;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDown;
        protected void RaiseOnDeviceMouseDown(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnDeviceMouseDoubleClick;
        protected void RaiseOnDeviceMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var handler = OnDeviceMouseDoubleClick;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public event MouseEventHandler OnGroupMouseDrag;
        protected void RaiseOnGroupMouseDrag(object sender, MouseEventArgs e)
        {
            var handler = OnGroupMouseDrag;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnGroupMouseDown;
        protected void RaiseOnGroupMouseDown(object sender, MouseEventArgs e)
        {
            var handler = OnGroupMouseDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public event MouseEventHandler OnGroupMouseDoubleClick;
        protected void RaiseOnGroupMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var handler = OnGroupMouseDoubleClick;
            if (handler != null)
            {
                handler(sender, e);
            }
        }


        // Constructor
        public DeviceViewView()
        {

        }


        public Dictionary<String, String> Localization { get; set; }

        public ICMS CMS { get; set; }
        public INVR NVR { get; set; }


        public override void GenerateViewModel()
        {
            var privateViewControl = GetFolderControl();
            privateViewControl.Name = Localization["FolderControl_Private"];

            var deviceGroups = CMS != null ? CMS.User.Current.DeviceGroups.Values : NVR.User.Current.DeviceGroups.Values;

            var sortResult = deviceGroups.OrderByDescending(d => d.Id).ToList();

            ViewModelPanel.Controls.Add(privateViewControl);
            AddContentToFolderControl(privateViewControl, sortResult); //Private

            //-----------------------------------------------------------------------------------------
            var sharedViewControl = GetFolderControl();
            sharedViewControl.Name = Localization["FolderControl_Shared"];

            ViewModelPanel.Controls.Add(sharedViewControl);

            AddContentToFolderControl(sharedViewControl, DeviceGroupsSortById()); // Public
        }

        private void AddContentToFolderControl(FolderControl folderControl, IEnumerable<IDeviceGroup> sortResult)
        {
            var groupControls = new List<Control>();

            var canAccessSetup = false;
            if (CMS != null)
                canAccessSetup = CMS.User.Current.Group.CheckPermission("Setup", Permission.User);
            else if (NVR != null)
                canAccessSetup = NVR.User.Current.Group.CheckPermission("Setup", Permission.User);

            foreach (IDeviceGroup deviceGroup in sortResult)
            {
                if (deviceGroup.Id == 0) continue; //dont show "all device" group

                //if user dont have setup permission, then dont show no camera view(user can't delete those view ether)
                if (deviceGroup.Items.Count == 0 && !canAccessSetup) continue;

                var groupControl = GetGroupControl();

                groupControl.DeviceGroup = deviceGroup;
                groupControls.Add(groupControl);
            }

            if (groupControls.Count > 0)
            {
                folderControl.GroupControlContainer.Controls.AddRange(groupControls.ToArray());
                groupControls.Clear();
            }
        }

        protected List<IDeviceGroup> DeviceGroupsSortById()
        {
            var deviceGroups = CMS != null ? CMS.Device.Groups.Values : NVR.Device.Groups.Values;

            var sortResult = deviceGroups.OrderByDescending(d => d.Id).ToList();

            return sortResult;
        }

        public override void UpdateView()
        {
            UpdateView("ID");
        }

        public override void UpdateView(String sort)
        {
            if (NVR == null || NVR.Device.Devices == null)
            {
                if (CMS == null || CMS.Device.Devices == null)
                    return;
            }

            ClearViewModel();

            GenerateViewModel();
        }

        public override void ClearViewModel()
        {
            if (ViewModelPanel == null) return;

            foreach (FolderControl folderControl in ViewModelPanel.Controls)
            {
                if (!RecycleFolder.Contains(folderControl))
                    RecycleFolder.Enqueue(folderControl);

                foreach (var groupControl in folderControl.GroupControlContainer.Controls.OfType<GroupControlUI2>())
                {
                    if (!RecycleGroup.Contains(groupControl))
                        RecycleGroup.Enqueue(groupControl);

                    groupControl.ClearViewModel();
                }
                folderControl.GroupControlContainer.Controls.Clear();
            }

            ViewModelPanel.Controls.Clear();
        }

        public override void UpdateToolTips()
        {
            foreach (FolderControl folderControl in ViewModelPanel.Controls)
            {
                foreach (var groupControl in folderControl.GroupControlContainer.Controls.OfType<GroupControlUI2>())
                {
                    foreach (Control control in groupControl.DeviceControlContainer.Controls)
                    {
                        var deviceControl = control as DeviceControlUI2;
                        if (deviceControl != null)
                        {
                            deviceControl.UpdateToolTips();
                        }
                    }
                }
            }
        }

        public override void UpdateRecordingStatus()
        {
            foreach (FolderControl folderControl in ViewModelPanel.Controls)
            {
                foreach (var groupControl in folderControl.GroupControlContainer.Controls.OfType<GroupControlUI2>())
                {
                    groupControl.UpdateRecordingStatus();
                }
            }
        }

        protected Queue<FolderControl> RecycleFolder = new Queue<FolderControl>();
        protected Queue<GroupControlUI2> RecycleGroup = new Queue<GroupControlUI2>();

        public FolderControl GetFolderControl()
        {
            if (RecycleFolder.Count > 0)
            {
                return RecycleFolder.Dequeue();
            }

            return new FolderControl();
        }

        protected virtual GroupControlUI2 GetGroupControl()
        {
            if (RecycleGroup.Count > 0)
            {
                return RecycleGroup.Dequeue();
            }

            var groupControl = new GroupControlUI2();

            groupControl.OnDeviceMouseDrag += RaiseOnDeviceMouseDrag;
            groupControl.OnDeviceMouseDown += RaiseOnDeviceMouseDown;
            groupControl.OnDeviceMouseDoubleClick += RaiseOnDeviceMouseDoubleClick;

            groupControl.OnGroupMouseDrag += RaiseOnGroupMouseDrag;
            groupControl.OnGroupMouseDown += RaiseOnGroupMouseDown;
            groupControl.OnGroupMouseDoubleClick += RaiseOnGroupMouseDoubleClick;

            return groupControl;
        }

        public override void Refresh()
        {
            foreach (FolderControl folderControl in ViewModelPanel.Controls)
            {
                foreach (GroupControlUI2 groupControl in folderControl.GroupControlContainer.Controls)
                {
                    groupControl.Invalidate();

                    foreach (Control control in groupControl.DeviceControlContainer.Controls)
                    {
                        control.Invalidate();
                    }
                }
            }
        }
    }
}