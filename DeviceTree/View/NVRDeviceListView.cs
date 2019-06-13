using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.View
{
    public class NVRDeviceListView : ViewBase
    {
        public event MouseEventHandler OnDeviceMouseDrag;
        public event MouseEventHandler OnDeviceMouseDown;
        public event MouseEventHandler OnDeviceMouseDoubleClick;

        public event MouseEventHandler OnNVRMouseDown;
        public event MouseEventHandler OnNVRMouseDrag;
        public event MouseEventHandler OnNVRMouseDoubleClick;

        protected void RaiseOnDeviceMouseDrag(Object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDrag != null)
            {
                OnDeviceMouseDrag(sender, e);
            }
        }

        protected void RaiseOnDeviceMouseDown(Object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDown != null)
            {
                OnDeviceMouseDown(sender, e);
            }
        }

        protected void RaiseOnDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDoubleClick != null)
            {
                OnDeviceMouseDoubleClick(sender, e);
            }
        }

        protected void RaiseOnNVRMouseDown(Object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDown != null)
            {
                OnNVRMouseDown(sender, e);
            }
        }
        protected void RaiseOnNVRMouseDrag(Object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDrag != null)
            {
                OnNVRMouseDrag(sender, e);
            }
        }
        protected void RaiseOnNVRMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (OnNVRMouseDoubleClick != null)
            {
                OnNVRMouseDoubleClick(sender, e);
            }
        }


        public NVRDeviceListView()
        {

        }


        public ICMS CMS { set; get; }
        public IPTS PTS { set; get; }

        private void GenerateViewModelByList(List<IServer> sortResult, Boolean isFailover)
        {
            var nvrControls = new List<NVRControlUI2>();
            foreach (IServer nvr in sortResult)
            {
                if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;

                NVRControlUI2 nvrControl = GetNVRControl();
                nvrControl.IsFailover = isFailover;

                nvrControl.NVR = nvr;

                if (PTS != null)
                    nvrControl.IsPTS = true;

                nvrControls.Add(nvrControl);

                if (nvr is ICMS)
                {
                    nvrControl.CreateTree();
                }
            }
            if (nvrControls.Count > 0)
            {
                ViewModelPanel.Controls.AddRange(nvrControls.ToArray());
                nvrControls.Clear();
            }
        }

        public override void GenerateViewModel()
        {
            if (CMS != null)
            {
                var sortFailoverResult = GetSortedFailoverNvrs();
                GenerateViewModelByList(sortFailoverResult, true);
            }

            var sortResult = GetSortedNvrs();
            GenerateViewModelByList(sortResult, false);
        }

        protected List<IServer> GetSortedNvrs()
        {
            if (CMS != null)
            {
                var sortResult = CMS.NVRManager.NVRs.Values.OfType<IServer>().Where(s => s.Manufacture != "iSAP Failover Server").OrderByDescending(s => s.Id).ToList();

                //var hasDevice = CMS.Device.Groups.Any(group => group.Value.Items.Any());

                //if (hasDevice)
                //    sortResult.Add(CMS);

                return sortResult;
            }

            if (PTS != null)
            {
                var sortResult = PTS.NVR.NVRs.Values.OfType<IServer>().OrderByDescending(s => s.Id).ToList();

                return sortResult;
            }

            return new List<IServer>();
        }

        protected List<IServer> GetSortedFailoverNvrs()
        {
            if (CMS != null)
            {
                var sortResult = CMS.NVRManager.NVRs.Values.OfType<IServer>().Where(s => s.Manufacture == "iSAP Failover Server").OrderByDescending(s => s.Id).ToList();

                return sortResult;
            }

            return new List<IServer>();
        }

        protected List<IDeviceGroup> GetSortedDeviceGroups(IServer server)
        {
            var sortResult = server.Device.Groups.Values.OrderByDescending(d => d.Id).ToList();

            return sortResult;
        }

        protected static Int32 SortByIdThenNVR(IDevice x, IDevice y)
        {
            if (x.Id != y.Id)
                return (y.Id - x.Id);

            return (y.Server.Id - x.Server.Id);
        }

        protected Queue<NVRControlUI2> RecycleNVR = new Queue<NVRControlUI2>();
        protected NVRControlUI2 GetNVRControl()
        {
            if (RecycleNVR.Count > 0)
            {
                return RecycleNVR.Dequeue();
            }

            var nvrControl = CreateNvrControl();
            nvrControl.OnNVRMouseDown += RaiseOnNVRMouseDown;
            nvrControl.OnNVRMouseDrag += RaiseOnNVRMouseDrag;
            nvrControl.OnNVRMouseDoubleClick += RaiseOnNVRMouseDoubleClick;

            nvrControl.OnDeviceMouseDown += RaiseOnDeviceMouseDown;
            nvrControl.OnDeviceMouseDrag += RaiseOnDeviceMouseDrag;
            nvrControl.OnDeviceMouseDoubleClick += RaiseOnDeviceMouseDoubleClick;

            return nvrControl;
        }

        protected virtual NVRControlUI2 CreateNvrControl()
        {
            return new NVRControlUI2();
        }

        public override void Refresh()
        {
            foreach (NVRControlUI2 nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.Invalidate();
                foreach (DeviceControlUI2 deviceControl in nvrControl.DeviceControlContainer.Controls)
                {
                    deviceControl.Invalidate();
                }
            }
        }

        public override void UpdateRecordingStatus()
        {
            foreach (NVRControlUI2 nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.UpdateRecordingStatus();
            }
        }

        public override void UpdateFailoverSyncTimer(Boolean enable)
        {
            foreach (NVRControlUI2 nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.UpdateFailoverSyncTimer(enable);
            }
        }

        public override void UpdateView()
        {
            UpdateView("ID");
        }

        public override void UpdateView(String sort)
        {
            if (CMS == null && PTS == null) return;
            if (CMS != null && CMS.NVRManager.NVRs == null) return;
            if (PTS != null && PTS.NVR.NVRs == null) return;

            ClearNVRViewModel();
            GenerateViewModel();
        }

        public override void UpdateToolTips()
        {
            foreach (NVRControlUI2 nvrControl in ViewModelPanel.Controls)
            {
                nvrControl.UpdateToolTips();
            }
        }

        protected void ClearNVRViewModel()
        {
            if (ViewModelPanel == null) return;

            foreach (var nvrControl in ViewModelPanel.Controls.OfType<NVRControlUI2>())
            {
                if (!RecycleNVR.Contains(nvrControl))
                    RecycleNVR.Enqueue(nvrControl);

                nvrControl.ClearViewModel();
            }

            ViewModelPanel.Controls.Clear();
        }
    }
}