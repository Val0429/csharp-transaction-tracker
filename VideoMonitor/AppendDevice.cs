using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public virtual void AppendDevice(Object sender, EventArgs<IDevice> e)
        {
            if (e.Value == null) return;

            AppendDevice(e.Value);
        }

        public virtual void AppendDevice(IDevice device)
        {
            if (!IsCoolDown) return;

            if (!AllowDuplicate && Devices.ContainsValue(device)) return;

            if (ActivateVideoWindow != null)
            {
                ActivateVideoWindow.Active = false;
            }

            ActivateVideoWindow = null;

            var isVisible = false;
            foreach (var videoWindow in VideoWindows)
            {
                if (ActivateVideoWindow == null && videoWindow.Camera == null)
                {
                    ActivateVideoWindow = videoWindow;
                    isVisible = videoWindow.Visible;
                    break;
                }
            }

            var appendId = (ActivateVideoWindow != null) ? VideoWindows.IndexOf(ActivateVideoWindow) : VideoWindows.Count;

            if (!Devices.ContainsKey(appendId))
            {
                if (Devices.Count >= MaxConnection) return;

                Devices.Add(appendId, device);
            }
            else
                Devices[appendId] = device;

            Int32 deviceCount = (Devices.Count > 0) ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            //if (WindowLayout != null && WindowLayout.Count >= deviceCount)
            if (WindowLayout != null && isVisible)
                SetLayout(WindowLayout);
            else
                SetLayout(WindowLayouts.LayoutGenerate((UInt16)deviceCount));

            if (ActivateVideoWindow != null && ActivateVideoWindow != VideoWindows[appendId])
                ActivateVideoWindow.Active = false;

            ActivateVideoWindow = VideoWindows[appendId];
            ActivateVideoWindow.Active = true;

            if (OnSelectionChange != null)
            {
                RaiseOnSelectionChange(new EventArgs<ICamera, PTZMode>(ActivateVideoWindow.Camera, ActivateVideoWindow.Viewer != null ? ActivateVideoWindow.Viewer.PtzMode : PTZMode.Disable));
            }

            RaiseOnContentChange(ToArray());

            if (OnViewingDeviceNumberChange != null)
            {
                deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }

            FocusGroup = null;

            ChangeNameLabelText();

            //fast append "SAME" device, enable cool down system, to avoid fast append
            if (PreviousAppendDevice == device)
            {
                IsCoolDown = false;
                CoolDownTimer.Enabled = true;
            }

            PreviousAppendDevice = device;
        }

        protected Boolean IsCoolDown = true;

        protected void AppendDeviceCoolDown(Object sender, EventArgs e)
        {
            CoolDownTimer.Enabled = false;

            IsCoolDown = true;
        }
    }
}
