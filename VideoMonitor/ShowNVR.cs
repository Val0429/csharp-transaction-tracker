using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public void ShowNVR(Object sender, EventArgs<INVR> e)
        {
            if (e.Value != null)
            {
                if (e.Value.ReadyState != ReadyState.Ready && e.Value.ReadyState != ReadyState.Modify) return;

                if (e.Value.Device.Groups.Count > 0)
                {
                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in e.Value.Device.Groups)
                    {
                        if (obj.Value.Items.Count > 0)
                        {
                            ShowGroup(obj.Value);
                            break;
                        }
                    }
                }
            }
        }

        public void ShowNVRDevice(Object sender, EventArgs<INVR> e)
        {
            if (e.Value != null)
            {
                if (e.Value.ReadyState != ReadyState.Ready && e.Value.ReadyState != ReadyState.Modify) return;

                if (e.Value.Device.Devices.Count > 0)
                {
                    var sortResult = GetSortedDevice(e.Value);
                    foreach (IDevice device in sortResult)
                    {
                        if (!IsCoolDown)
                        {
                            System.Threading.Thread.Sleep(500);
                            IsCoolDown = true;
                        }

                        if (e.Value.Manufacture == "iSAP Failover Server")
                        {
                            if(e.Value.FailoverDeviceList != null)
                            {
                                if(e.Value.FailoverDeviceList.Contains(device))
                                AppendDevice(device);
                            }
                        }
                        else
                        {
                            AppendDevice(device);
                        }
                        
                    }
                }
            }
        }

        protected List<IDevice> GetSortedDevice(IServer server)
        {
            var sortResult = server.Device.Devices.Values.OrderBy(d => d.Id).ToList();

            return sortResult;
        }
    }
}
