using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using DeviceConstant;
using Interface;
using ServerProfile;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
    {
        private void RemoveNoPermissionDevice()
        {
            if (Nvr.User.Current.Group.IsFullAccessToDevices) return;

            IDevice[] devices = (from obj in Nvr.Device.Devices where !obj.Value.CheckPermission(Permission.Access) select obj.Value).ToArray();

            foreach (IDevice device in devices)
            {
                Nvr.Device.Devices.Remove(device.Id);

                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Nvr.Device.Groups)
                {
                    RemoveNoPermissionDeviceFromDeviceGroup(obj.Value, device);
                }

                foreach (KeyValuePair<UInt16, IDeviceLayout> obj in Nvr.Device.DeviceLayouts)
                {
                    RemoveNoPermissionDeviceFromDeviceLayout(obj.Value, device);
                }

                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Nvr.Device.Groups)
                {
                    // remove sublayout and devicelayout THAT DONT HAVE DEVICE
                    RemoveNoDataDeviceLayoutAndSubLayoutFromDevices(obj.Value.Items);
                    RemoveNoDataDeviceLayoutAndSubLayoutFromDevices(obj.Value.View);
                }

                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Nvr.User.Current.DeviceGroups)
                {
                    RemoveNoPermissionDeviceFromDeviceGroup(obj.Value, device);
                }

                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Nvr.User.Current.DeviceGroups)
                {
                    // remove sublayout and devicelayout THAT DONT HAVE DEVICE
                    RemoveNoDataDeviceLayoutAndSubLayoutFromDevices(obj.Value.Items);
                    RemoveNoDataDeviceLayoutAndSubLayoutFromDevices(obj.Value.View);
                }
            }

            RemoveNoPermissionEventHandle();
        }

        private static void RemoveNoPermissionDeviceFromDeviceGroup(IDeviceGroup group, IDevice device)
        {
            while (group.Items.Contains(device))
            {
                group.Items.Remove(device);
            }

            while (group.View.Contains(device))
            {
                group.View[group.View.IndexOf(device)] = null;
            }

            //remove last null device
            while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
            {
                group.View.RemoveAt(group.View.Count - 1);
            }

            //remove null from all device group
            if (group.Id == 0)
            {
                while (group.Items.Contains(null))
                {
                    group.Items.Remove(null);
                }

                while (group.View.Contains(null))
                {
                    group.View.Remove(null);
                }
            }
        }

        private static void RemoveNoPermissionDeviceFromDeviceLayout(IDeviceLayout deviceLayout, IDevice device)
        {
            while (deviceLayout.Items.Contains(device))
            {
                deviceLayout.Items[deviceLayout.Items.IndexOf(device)] = null;
            }
        }
        
        private static void RemoveNoDataDeviceLayoutAndSubLayoutFromDevices(IList<IDevice> container)
        {
            var devices = container.ToArray();
            foreach (var device in devices)
            {
                if (device is IDeviceLayout)
                {	//no device, dont show this device, it will ALSO not show on device list
                    var count = ((IDeviceLayout)device).Items.Count(item => item != null);
                    if (count == 0)
                        container.Remove(device);
                    continue;
                }

                if (device is ISubLayout)
                {	//no device, dont show this device, it will ALSO not show on device list
                    var count = ((ISubLayout)device).DeviceLayout.Items.Count(item => item != null);
                    if (count == 0)
                        container.Remove(device);
                    continue;
                }
            }
        }

        private void RemoveNoPermissionEventHandle()
        {
            foreach (KeyValuePair<UInt16, IDevice> obj in Nvr.Device.Devices)
            {
                var device = obj.Value as ICamera;
                if (device == null) continue;

                foreach (KeyValuePair<EventCondition, List<EventHandle>> eventHandle in device.EventHandling)
                {
                    var handles = eventHandle.Value;
                    if (handles.Count == 0) continue;

                    var noPermissionHandle = new List<EventHandle>();

                    foreach (EventHandle handle in handles)
                    {
                        if (handle is HotSpotEventHandle)
                        {
                            var hotSpotEventHandle = handle as HotSpotEventHandle;
                            if (hotSpotEventHandle.Device != null && !Nvr.Device.Devices.ContainsKey(hotSpotEventHandle.Device.Id))
                                noPermissionHandle.Add(handle);
                            continue;
                        }

                        if (handle is GotoPresetEventHandle)
                        {
                            var gotoPresetEventHandle = handle as GotoPresetEventHandle;
                            if (gotoPresetEventHandle.Device != null && !Nvr.Device.Devices.ContainsKey(gotoPresetEventHandle.Device.Id))
                                noPermissionHandle.Add(handle);
                            continue;
                        }


                        if (handle is PopupPlaybackEventHandle)
                        {
                            var popupPlaybackEventHandle = handle as PopupPlaybackEventHandle;
                            if (popupPlaybackEventHandle.Device != null && !Nvr.Device.Devices.ContainsKey(popupPlaybackEventHandle.Device.Id))
                                noPermissionHandle.Add(handle);
                            continue;
                        }

                        if (handle is PopupLiveEventHandle)
                        {
                            var popupLiveEventHandle = handle as PopupLiveEventHandle;
                            if (popupLiveEventHandle.Device != null && !Nvr.Device.Devices.ContainsKey(popupLiveEventHandle.Device.Id))
                                noPermissionHandle.Add(handle);
                            continue;
                        }

                        if (handle is TriggerDigitalOutEventHandle)
                        {
                            var triggerDigitalOutEventHandle = handle as TriggerDigitalOutEventHandle;
                            if (triggerDigitalOutEventHandle.Device != null && !Nvr.Device.Devices.ContainsKey(triggerDigitalOutEventHandle.Device.Id))
                                noPermissionHandle.Add(handle);
                            continue;
                        }
                    }
                    foreach (EventHandle handle in noPermissionHandle)
                    {
                        handles.Remove(handle);
                    }
                }
            }
        }
    }
}
