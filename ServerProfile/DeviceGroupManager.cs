using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Constant;
using Device;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        /// <summary>
        /// Load group for NVR / CMS 1.0
        /// </summary>
        protected const String CgiLoadGroup = @"cgi-bin/deviceconfig?action=loadgroup";
        private const String CgiSaveGroup = @"cgi-bin/deviceconfig?action=savegroup";

        /// <summary>
        /// Load group for CMS 2.0
        /// </summary>
        private const String CgiLoadNVRGroup = @"cgi-bin/nvrconfig?action=loadview";
        private const String CgiSaveNVRGroup = @"cgi-bin/nvrconfig?action=saveview";

        protected virtual DeviceGroup ParseDeviceGroup(XmlElement groupNode)
        {
            var groupId = Convert.ToUInt16(groupNode.GetAttribute("id"));

            List<String> listId = DeviceConverter.XmlElementToStringList(groupNode, "Items");
            List<IDevice> list = ConvertToDeviceList(groupId, listId);

            List<String> viewId = DeviceConverter.XmlElementToStringList(groupNode, "View");
            var view = ConvertToDeviceView(viewId, list);

            //append "no in view " device to end
            foreach (var device in list.OrderBy(d => d.Id))
            {
                Boolean containes = view.Any(viewDevice => viewDevice != null && viewDevice.Id == device.Id);

                if (!containes && Devices.ContainsKey(device.Id))
                {
                    view.Add(Devices[device.Id]);
                }
            }

            return new DeviceGroup
            {
                ReadyState = ReadyState.Ready,
                Id = groupId,
                Server = Server,
                Name = Xml.GetFirstElementValueByTagName(groupNode, "Name"),
                Items = list,
                View = view,
                Layout = ((groupId == 0) ? null : DeviceConverter.XmlElementToLayout(groupNode, "Layout")),
                Regions = DeviceConverter.XmlElementToRegion(groupNode),
                MountType = DeviceConverter.XmlElementToMountType(groupNode),
                DewarpEnable = DeviceConverter.XmlElementToDewarpEnable(groupNode)
            };
        }

        protected virtual List<IDevice> ConvertToDeviceList(ushort groupId, List<String> listId)
        {
            return groupId == 0 ? DeviceConverter.StringToAllDeviceList(Server, listId) : DeviceConverter.StringToDeviceList(Server, listId);
        }

        protected virtual List<IDevice> ConvertToDeviceView(List<String> viewId, List<IDevice> list)
        {
            var view = DeviceConverter.StringToDeviceView(Server, viewId, list);

            return view;
        }

        protected virtual string LoadDeviceGroupCgi { get { return Server is ICMS ? CgiLoadNVRGroup : CgiLoadGroup; } }

        private void LoadDeviceGroups()
        {
            Groups.Clear();

            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(LoadDeviceGroupCgi, Server.Credential);

            if (xmlDoc != null)
            {
                XmlNodeList groups = xmlDoc.GetElementsByTagName("Group");
                if (groups.Count > 0)
                {
                    foreach (XmlElement node in groups)
                    {
                        IDeviceGroup group = ParseDeviceGroup(node);
                        if (group == null) continue;

                        if (!Groups.ContainsKey(group.Id))
                            Groups.Add(group.Id, group);

                        if (group.Id == 0)
                        {
                            if (Server is ICMS)
                                Groups.Remove(group.Id);//CMS not need "all device"
                            else
                                group.Name = Localization["Data_AllDevices"];
                        }
                    }
                }
            }

            if (Server is ICMS)
            {
                DeviceManagerLoadReady();
                return;
            }

            if (Groups.ContainsKey(0))
            {
                DeviceManagerLoadReady();
                return;
            }

            IDeviceGroup defaultGroup = new DeviceGroup
            {
                Id = 0,
                Server = Server,
                ReadyState = ReadyState.Ready,
                Name = Localization["Data_AllDevices"],
            };

            Groups.Add(defaultGroup.Id, defaultGroup);

            DeviceManagerLoadReady();
        }

        protected virtual string DeviceItemToString(List<IDevice> list)
        {
            return DeviceConverter.DeviceListToString(list);
        }

        protected virtual string DeviceViewToString(List<IDevice> list)
        {
            return DeviceConverter.DeviceViewToString(list);
        }

        private void SaveDeviceAndGroup()
        {
            _saveDeviceFlag = false;
            if (Server is ICMS)
            {
                SaveDeviceGroups();
                SaveEvent();
            }
            else if (Server is IVAS)//save device with ROI/group
            {
                SaveDevicesPeopleCounting();
                SaveDeviceGroups();
            }
            else if (Server is IFOS)
            {
            }
            else if (Server is INVR)
            {
                SaveDevices();
                SaveDeviceGroups();

                if (SupportDeviceLayout)
                    SaveDeviceLayout();
            }
            _saveDeviceFlag = true;
        }

        private void SaveDeviceGroups()
        {
            SaveDeviceGroupToXml();

            foreach (var obj in Groups)
                obj.Value.ReadyState = ReadyState.Ready;
        }

        private void SaveDeviceGroupToXml()
        {
            var cgi = Server is ICMS ? CgiLoadNVRGroup : CgiLoadGroup;

            var originalXmlDoc = Xml.LoadXmlFromHttp(cgi, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            foreach (IDeviceGroup group in Groups.Values.OrderBy(g => g.Id))
            {
                //----------------------------------DeviceSetting
                var groupNode = xmlDoc.CreateElement("Group");
                groupNode.SetAttribute("id", group.Id.ToString(CultureInfo.InvariantCulture));
                xmlRoot.AppendChild(groupNode);

                groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Name", group.Id > 0 ? group.Name : ""));

                if (group.Id == 0)
                {
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Items", DeviceConverter.AllDeviceListToString(group.Items)));
                }
                else
                {
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Items", DeviceItemToString(group.Items)));
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("View", DeviceViewToString(group.View)));
                    groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Layout", WindowLayouts.LayoutToString(@group.Layout)));
                    var regionNode = xmlDoc.CreateXmlElementWithText("Region", "");
                    groupNode.AppendChild(regionNode);
                    foreach (XmlElement region in group.Regions)
                    {
                        var deviceRegion = xmlDoc.CreateXmlElementWithText("DeviceRegion", "");
                        regionNode.AppendChild(deviceRegion);
                        if (region == null) continue;
                        var imported = xmlDoc.ImportNode(region, true);
                        deviceRegion.AppendChild(imported);
                    }

                    var mountTypeNode = xmlDoc.CreateXmlElementWithText("MountTypes", "");
                    groupNode.AppendChild(mountTypeNode);
                    foreach (Int16 mountType in group.MountType)
                    {
                        var deviceMountType = xmlDoc.CreateXmlElementWithText("MountType", mountType);
                        mountTypeNode.AppendChild(deviceMountType);
                    }

                    var dewarpEnableNode = xmlDoc.CreateXmlElementWithText("DewarpEnable", "");
                    groupNode.AppendChild(dewarpEnableNode);
                    foreach (Boolean enable in group.DewarpEnable)
                    {
                        var deviceDewarpEnable = xmlDoc.CreateXmlElementWithText("Enable", enable ? 1 : 0);
                        dewarpEnableNode.AppendChild(deviceDewarpEnable);
                    }
                }
            }

            if (originalXmlDoc != null && !String.Equals(originalXmlDoc.InnerXml, xmlDoc.InnerXml))
            {
                if (Server is ICMS)
                {
                    Xml.PostXmlToHttp(CgiSaveNVRGroup, xmlDoc, Server.Credential);
                    return;
                }
                Xml.PostXmlToHttp(CgiSaveGroup, xmlDoc, Server.Credential);
            }
        }
    }
}