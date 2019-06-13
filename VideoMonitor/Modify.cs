using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            if (Devices.Count == 0) return;

            if (e.Value == null) return;
            var device = e.Value;

            if (!Devices.Values.Contains(device)) return;

            Boolean inUse = false;
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;

                if (videoWindow.Camera == device)
                {
                    inUse = true;
                    DisconnectWindow(videoWindow);
                }
            }

            if (inUse)
            {
                RaiseOnContentChange(ToArray());

                if (OnViewingDeviceNumberChange != null)
                {
                    var deviceCount = (Devices == null) ? 0 : Devices.Count;
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
            }
        }
        
        public void DeviceLayoutModify(Object sender, EventArgs<IDeviceLayout> e)
        {
            if (Devices.Count == 0) return;

            if (e.Value == null) return;
            var device = e.Value;

            if (!Devices.Values.Contains(device)) return;

            Boolean inUse = false;
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;

                if (videoWindow.Camera == device)
                {
                    inUse = true;
                    DisconnectWindow(videoWindow);
                }
            }

            if (inUse)
            {
                RaiseOnContentChange(ToArray());

                if (OnViewingDeviceNumberChange != null)
                {
                    var deviceCount = (Devices == null) ? 0 : Devices.Count;
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
            }
        }
        
        public void SubLayoutModify(Object sender, EventArgs<ISubLayout> e)
        {
            if (Devices.Count == 0) return;

            if (e.Value == null) return;
            var device = e.Value;

            if (!Devices.Values.Contains(device)) return;

            Boolean inUse = false;
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;

                if (videoWindow.Camera == device)
                {
                    inUse = true;
                    DisconnectWindow(videoWindow);
                }
            }

            if (inUse)
            {
                RaiseOnContentChange(ToArray());

                if (OnViewingDeviceNumberChange != null)
                {
                    var deviceCount = (Devices == null) ? 0 : Devices.Count;
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
            }
        }

        public void GroupModify(Object sender, EventArgs<IDeviceGroup> e)
        {
            if (FocusGroup == null) return;
            if (e.Value == null) return;

            if (FocusGroup == e.Value)
            {
                ClearAll();
                SetLayout(WindowLayouts.LayoutGenerate(4));
            }
        }

        public void NVRModify(Object sender, EventArgs<INVR> e)
        {
            if (e.Value == null) return;

            if (FocusGroup != null)
            {
                if (FocusGroup.Server == e.Value)
                {
                    ClearAll();
                    SetLayout(WindowLayouts.LayoutGenerate(4));
                    return;
                }
            }

            var inUse = false;
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;

                if (videoWindow.Camera.Server == e.Value)
                {
                    inUse = true;
                    DisconnectWindow(videoWindow);
                }
            }

            if (inUse)
            {
                RaiseOnContentChange(ToArray());

                if (OnViewingDeviceNumberChange != null)
                {
                    var deviceCount = (Devices == null) ? 0 : Devices.Count;
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
            }
        }

        public delegate void POSModifyDelegate(Object sender, EventArgs<IPOS> e);
        public void POSModify(Object sender, EventArgs<IPOS> e)
        {
            if (FocusGroup == null) return;

            if (InvokeRequired)
            {
                Invoke(new POSModifyDelegate(POSModify), sender, e);
                return;
            }

            if (e.Value == null) return;

            if (FocusGroup == e.Value)
            {
                ClearAll();
                SetLayout(WindowLayouts.LayoutGenerate(4));
            }
        }
    }
}
