using Constant;
using DeviceConstant;
using Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ServerProfile
{
    public class DeviceConverter
    {
        // Modify by Tulip 20141112

        // Group 0 All Device Parse here
        public static String AllDeviceListToString(List<IDevice> items)
        {
            items.Sort((x, y) => (x.Id - y.Id));

            var temp = new List<String>();

            foreach (var item in items)
            {
                if (item == null) continue;

                if (item is ICamera && item.DeviceType == DeviceType.Device)
                {
                    var nvrDevice = item as ICamera;
                    if (nvrDevice.CMS != null)
                    {
                        temp.Add(nvrDevice.Server.Id + "-" + nvrDevice.Id);
                    }
                    else if (item.Id != 0)
                    {
                        temp.Add(item.Id.ToString(CultureInfo.InvariantCulture));
                    }
                }

            }
            return String.Join(",", temp.ToArray());
        }

        public static List<IDevice> StringToAllDeviceList(IServer server, List<String> itemList)
        {
            var list = new List<IDevice>();
            while (itemList.Contains("0"))
                itemList.Remove("0");

            foreach (string items in itemList)
            {
                IDevice dev = null;
                var item = items.Split('-');

                if (item.Length == 2)
                {
                    // CMS
                    var svrId = Convert.ToUInt16(item[0]);
                    var chnId = Convert.ToUInt16(item[1]);

                    var cms = server as ICMS;
                    if (cms != null && cms.NVRManager.NVRs.ContainsKey(svrId))
                    {
                        var nvr = cms.NVRManager.NVRs[svrId];
                        if (nvr.Device.Devices.ContainsKey(chnId))
                        {
                            dev = nvr.Device.Devices[chnId];
                        }
                    }
                }
                else
                {
                    // NVR
                    if (String.IsNullOrEmpty(item[0]))
                        continue;
                    var chnId = Convert.ToUInt16(item[0]);
                    if (server.Device.Devices.ContainsKey(chnId))
                        dev = server.Device.Devices[chnId];
                }

                if (dev != null)
                {
                    dev.DeviceType = DeviceType.Device;
                    list.Add(dev);
                }
            }

            return list;
        }

        // Device Group Exclude Group 0 All Devices
        // Items and Views Format : DeviceType-NVR-Channel-x-y-x.

        // NVR Channel : D-0-3 ==> Device Nvr 0 (itself) channel 3
        // CMS Channel : D-2-3 ==> Device Nvr 2 Channel 3
        // Layout Splice : L-4-0 ==> Layout View 4 Channel 0 (Splice)
        // Sublayout : L-4-2 ==> Layout View 4 Channel 3 (SubLayout)
        // GPS : G-0-5 ==>GPS Nvr 0 Channel 5 (not implement)
        // LPR : P-2-5 ==>LPR Nvr 2 Channel 5
        public static string DeviceToString(IDevice item)
        {
            if (item is IDeviceLayout)
            {
                // Device-Layout.Id-0
                return "L-" + item.Id + "-0";
            }

            if (item is ISubLayout)
            {
                // Device-Layout.Id-subLayout.Id
                var sub = item as ISubLayout;
                return "L-" + sub.DeviceLayout.Id + "-" + sub.Id;
            }

            if (item is ICamera)
            {
                var dev = item as ICamera;

                switch (item.DeviceType)
                {
                    case DeviceType.Drone:
                        // Drone, C for craft
                        return string.Format("C-0-{0}", dev.Id);
                    case DeviceType.GPS:
                        // Transportation (GPS)
                        return String.Format("G-{0}-{1}", dev.CMS != null ? dev.Server.Id : 0, dev.Id);
                    case DeviceType.LPR:
                        return String.Format("P-{0}-{1}", dev.CMS != null ? dev.Server.Id : 0, dev.Id);
                    case DeviceType.Door:
                        Debug.Assert(false, "Covert door at derived class.");
                        return String.Format("A-0-{0}", dev.Id);
                    default:
                        if (dev.CMS != null)
                        {
                            return String.Format("D-{0}-{1}", dev.Server.Id, dev.Id);
                        }

                        return string.Format("D-{0}-{1}", 0, dev.Id);
                }
            }

            return item != null && item.Id != 0 ? item.Id.ToString(CultureInfo.InvariantCulture) : "";
        }

        //need sort and skip empty string
        public static String DeviceListToString(List<IDevice> items)
        {
            items.Sort((x, y) => (x.Id - y.Id));

            var temp = new List<String>();

            foreach (var item in items)
            {
                if (item == null) continue;

                var val = DeviceToString(item);
                temp.Add(val);
            }

            return String.Join(",", temp.ToArray());
        }

        //dont need sort and keep mepty string
        public static String DeviceViewToString(List<IDevice> items)
        {
            var temp = new List<String>();

            if (items == null)
            {
                return String.Join(",", temp.ToArray());
            }

            foreach (var item in items)
            {
                var val = DeviceToString(item);
                temp.Add(val);
            }

            return String.Join(",", temp.ToArray());
        }

        public static IDevice ParseDevice(IServer server, string items)
        {
            IDevice dev = null;

            var item = items.Split('-');
            if (item.Length == 3)
            {
                var type = item[0];
                var svrId = Convert.ToUInt16(item[1]);
                var chnId = Convert.ToUInt16(item[2]);

                switch (type)
                {
                    case "D": // Device
                    case "G": // GPS
                    case "P": // LPR
                    case "A": // ACS
                        {
                            if (svrId == 0)
                            {
                                // NVR
                                if (server.Device.Devices.ContainsKey(chnId))
                                    dev = server.Device.Devices[chnId];
                            }
                            else
                            {
                                // CMS
                                var cms = server as ICMS;
                                if (cms.NVRManager.NVRs.ContainsKey(svrId))
                                {
                                    var nvr = cms.NVRManager.NVRs[svrId];
                                    if (nvr.Device.Devices.ContainsKey(chnId))
                                        dev = nvr.Device.Devices[chnId];
                                }
                            }

                            if (dev != null)
                            {
                                switch (type)
                                {
                                    case "D":
                                        dev.DeviceType = DeviceType.Device;
                                        break;
                                    case "G":
                                        dev = dev.CloneDevice();
                                        dev.DeviceType = DeviceType.GPS;
                                        break;
                                    case "P":
                                        dev = dev.CloneDevice();
                                        dev.DeviceType = DeviceType.LPR;
                                        break;
                                    case "A":
                                        dev = dev.CloneDevice();
                                        dev.DeviceType = DeviceType.Door;
                                        break;
                                }
                            }

                            break;
                        }

                    case "L": // Layput
                        {
                            if (server.Device.DeviceLayouts.ContainsKey(svrId))
                            {
                                var layout = server.Device.DeviceLayouts[svrId];

                                if (chnId == 0)
                                {
                                    // Splice
                                    dev = layout;
                                }
                                else
                                {
                                    // Sublayout
                                    if (layout.SubLayouts.ContainsKey(chnId))
                                        dev = layout.SubLayouts[chnId];
                                }
                            }

                            if (dev != null)
                                dev.DeviceType = DeviceType.Layout;

                            break;
                        }
                }
            }
            else if (item.Length == 2)
            {
                var svrId = Convert.ToUInt16(item[0]);
                var chnId = Convert.ToUInt16(item[1]);
                // CMS
                var cms = server as ICMS;
                if (cms != null)
                {
                    if (cms.NVRManager.NVRs.ContainsKey(svrId))
                    {
                        var nvr = cms.NVRManager.NVRs[svrId];
                        if (nvr.Device.Devices.ContainsKey(chnId))
                            dev = nvr.Device.Devices[chnId];
                    }

                    if (dev != null)
                    {
                        dev.DeviceType = DeviceType.Device;
                    }
                }
            }
            else if (item.Length == 1)
            {
                if (!String.IsNullOrEmpty(item[0]))
                {
                    var chnId = Convert.ToUInt16(item[0]);
                    if (server.Device.Devices.ContainsKey(chnId))
                        dev = server.Device.Devices[chnId];

                    if (dev != null)
                    {
                        dev.DeviceType = DeviceType.Device;
                    }
                }
            }

            return dev;
        }

        public static List<IDevice> StringToDeviceList(IServer server, List<String> itemList)
        {
            var list = new List<IDevice>();
            while (itemList.Contains("0"))
                itemList.Remove("0");

            foreach (string items in itemList)
            {
                var dev = ParseDevice(server, items);

                if (dev != null && dev.DeviceType == DeviceType.Device)
                {
                    list.Add(dev);
                }
            }

            return list;
        }

        public static List<IDevice> StringToDeviceView(IServer server, List<String> itemList, List<IDevice> list)
        {
            var view = new List<IDevice>();

            // There is only one item in the device group and the item is empty.
            if (itemList.Count == 1 && itemList[0] == "")
            {
                return view;
            }

            foreach (string items in itemList)
            {
                var dev = ParseDevice(server, items);

                view.Add(dev != null && dev.DeviceType == DeviceType.Device ? dev : null);
            }

            return view;
        }

        public static List<String> XmlElementToStringList(XmlElement element, string tagName)
        {
            string innerText = Xml.GetFirstElementValueByTagName(element, tagName);

            var items = innerText.Split(',');

            return items.ToList();
        }

        // To UIntList
        public static List<UInt16> XmlElementToUIntList(XmlElement element, string tagName)
        {
            string innerText = Xml.GetFirstElementValueByTagName(element, tagName);

            return StringToUIntList(innerText);
        }

        private static List<UInt16> StringToUIntList(String value)
        {
            return value.Split(',').Select(s => (String.Equals(s, "") || s.Contains(":")) ? (UInt16)0 : Convert.ToUInt16(s)).ToList();
        }

        // To WindowLayoutList
        public static List<WindowLayout> XmlElementToLayout(XmlElement element, string tagName)
        {
            string innerText = Xml.GetFirstElementValueByTagName(element, tagName);

            return StringToLayout(innerText);
        }

        // To Device dewarp region
        public static List<XmlElement> XmlElementToRegion(XmlElement element)
        {
            var result = new List<XmlElement>();
            var region = Xml.GetFirstElementByTagName(element, "Region");
            if (region == null) return result;

            var regions = region.SelectNodes("DeviceRegion");
            if (regions == null) return result;

            foreach (XmlElement regionNode in regions)
            {
                result.Add((XmlElement)regionNode.SelectSingleNode("PTZRegions"));
            }

            return result;
        }

        // To Device mount type 
        public static List<Int16> XmlElementToMountType(XmlElement element)
        {
            var result = new List<Int16>();
            var region = Xml.GetFirstElementByTagName(element, "MountTypes");
            if (region == null) return result;

            var regions = region.SelectNodes("MountType");
            if (regions == null) return result;

            foreach (XmlElement regionNode in regions)
            {
                if (String.IsNullOrEmpty(regionNode.InnerText))
                {
                    result.Add(0);
                    continue;
                }

                try
                {
                    result.Add(Convert.ToInt16(regionNode.InnerText));
                }
                catch (Exception)
                {
                    result.Add(-1);
                }

            }

            return result;
        }

        // To Device dewarp enable
        public static List<Boolean> XmlElementToDewarpEnable(XmlElement element)
        {
            var result = new List<Boolean>();
            var region = Xml.GetFirstElementByTagName(element, "DewarpEnable");
            if (region == null) return result;

            var regions = region.SelectNodes("Enable");
            if (regions == null) return result;

            foreach (XmlElement regionNode in regions)
            {
                result.Add(regionNode.InnerText == "1");
            }

            return result;
        }

        public static List<WindowLayout> StringToLayout(String value)
        {
            if (value.Length == 0) return null;

            var numbers = new List<Double[]>();

            while (value.Length > 0)
            {
                Int32 start = value.IndexOf('[');
                Int32 end = value.IndexOf(']');

                if (start == -1 || end == -1) break;

                numbers.Add(ConvertStringToDoubleList(value.Substring(start + 1, end - 1)));

                if (end + 2 > value.Length) break;

                value = value.Substring(end + 2);
            }

            if (numbers.Count == 0) return null;

            return WindowLayouts.LayoutGenerate(numbers);
        }

        private static Double[] ConvertStringToDoubleList(String value)
        {
            var provider = new NumberFormatInfo
                           {
                               NumberDecimalSeparator = ".",
                               NumberGroupSeparator = "."
                           };

            return value.Split(',').Select(s => String.Equals(s, "") ? (Double)0 : Convert.ToDouble(s, provider)).ToArray();
        }

    }
}