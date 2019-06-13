using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using POSRegister.Objects;

namespace POSRegister
{
    public class POSListView : ViewBase
    {
        public virtual event MouseEventHandler OnDeviceMouseDrag;
        public virtual event MouseEventHandler OnDeviceMouseDown;
        public virtual event MouseEventHandler OnDeviceMouseDoubleClick;

        public virtual event MouseEventHandler OnPOSMouseDrag;
        public virtual event MouseEventHandler OnPOSMouseDown;
        public virtual event MouseEventHandler OnPOSMouseDoubleClick;

        public IPTS PTS;

        public override void GenerateViewModel()
        {
            //ViewModelPanel.Visible = false;
            var sortResult = POSsSortById();

            var posControls = new List<POSControlUI2>();

            //INVR nvr = null;
            //if(PTS.NVR.NVRs.Count > 0)
            //    nvr = PTS.NVR.NVRs.First().Value;

            foreach (IPOS pos in sortResult)
            {
                //if (pos.Devices.Count == 0) continue;

                var posControl = GetPOSControl();

                posControl.POS = pos;
                posControls.Add(posControl);

                var list = new List<IDevice>(pos.Items);
                list.Sort(SortByIdThenNVR);
                var deviceControls = new List<DeviceControlUI2>();

                foreach (IDevice device in list)
                {
                    if (device == null) continue;
                    var deviceControl = GetDevicePanel();

                    deviceControl.Device = device;
                    deviceControls.Add(deviceControl as DeviceControlUI2);
                }

                if (deviceControls.Count > 0)
                {
                    posControl.DeviceControlContainer.Controls.AddRange(deviceControls.ToArray());
                    deviceControls.Clear();
                }
            }
            if (posControls.Count > 0)
            {
                ViewModelPanel.Controls.AddRange(posControls.ToArray());
                posControls.Clear();
            }

            //ViewModelPanel.Visible = true;
        }

        protected List<IPOS> POSsSortById()
        {


            var sortResult = new List<IPOS>(PTS.POS.POSServer);
            //var sortResult = new List<IPOS>(PTS.Pos.Values);

            //reverse
            sortResult.Sort((x, y) => (y.Id.CompareTo(x.Id)));

            return sortResult;
        }

        public override void UpdateView()
        {
            UpdateView("ID");
        }

        public override void UpdateView(String sort)
        {
            if (PTS == null || PTS.POS == null) return;

            ClearViewModel();

            GenerateViewModel();
        }

        public override void ClearViewModel()
        {
            if (ViewModelPanel == null) return;

            foreach (POSControlUI2 posControl in ViewModelPanel.Controls)
            {
                if (!RecyclePOS.Contains(posControl))
                    RecyclePOS.Enqueue(posControl);

                foreach (DeviceControlUI2 deviceControl in posControl.DeviceControlContainer.Controls)
                {
                    if (!RecycleDevice.Contains(deviceControl))
                    {
                        deviceControl.Device = null;
                        RecycleDevice.Enqueue(deviceControl);
                    }
                }

                posControl.DeviceControlContainer.Controls.Clear();
            }

            ViewModelPanel.Controls.Clear();
        }

        private static Int32 SortByIdThenNVR(IDevice x, IDevice y)
        {
            if (x.Id != y.Id)
                return (y.Id - x.Id);

            return (y.Server.Id - x.Server.Id);
        }

        protected Queue<POSControlUI2> RecyclePOS = new Queue<POSControlUI2>();
        protected Queue<DeviceControlUI2> RecycleDevice = new Queue<DeviceControlUI2>();
        public virtual POSControlUI2 GetPOSControl()
        {
            if (RecyclePOS.Count > 0)
            {
                return RecyclePOS.Dequeue();
            }

            var posControl = new POSControlUI2();

            posControl.OnPOSMouseDrag += OnPOSMouseDrag;
            posControl.OnPOSMouseDown += OnPOSMouseDown;
            posControl.OnPOSMouseDoubleClick += OnPOSMouseDoubleClick;

            return posControl;
        }

        public virtual IDeviceControl GetDevicePanel()
        {
            if (RecycleDevice.Count > 0)
            {
                return RecycleDevice.Dequeue();
            }

            var deviceControl = new DeviceControlUI2();
            deviceControl.OnDeviceMouseDown += OnDeviceMouseDown;
            deviceControl.OnDeviceMouseDrag += OnDeviceMouseDrag;
            deviceControl.OnDeviceMouseDoubleClick += OnDeviceMouseDoubleClick;

            return deviceControl;
        }

        public override void Refresh()
        {
            foreach (POSControlUI2 posControl in ViewModelPanel.Controls)
            {
                posControl.Invalidate();
                foreach (DeviceControlUI2 deviceControl in posControl.DeviceControlContainer.Controls)
                {
                    deviceControl.Invalidate();
                }
            }
        }

        public void UpdateRecordingStatus()
        {
            foreach (POSControlUI2 posControl in ViewModelPanel.Controls)
            {
                posControl.UpdateRecordingStatus();
            }
        }
    }
}