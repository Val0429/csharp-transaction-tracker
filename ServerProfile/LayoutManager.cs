using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using Device;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String CgiLoadLayout = @"cgi-bin/deviceconfig?action=loadlayout";
        private const String CgiSaveLayout = @"cgi-bin/deviceconfig?action=savelayout";

        private void LoadDeviceLayout()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadLayout, Server.Credential);
            if (xmlDoc == null) return;
            DeviceLayouts.Clear();

            XmlNodeList layoutList = xmlDoc.GetElementsByTagName("Layout");

            foreach (XmlElement layoutNode in layoutList)
            {
                XmlElement regions = Xml.GetFirstElementByTagName(layoutNode, "Regions");
                if (regions != null)
                    layoutNode.RemoveChild(regions);

                var id = Convert.ToUInt16(layoutNode.GetAttribute("id"));
                List<UInt16> listId = DeviceConverter.XmlElementToUIntList(layoutNode, "Items");
                
                var layout = new DeviceLayout
                                 {
                                     Id = id,
                                     Name = Xml.GetFirstElementValueByTagName(layoutNode, "Name"),
                                     ReadyState = ReadyState.Ready,
                                     LayoutX = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(layoutNode, "LayoutX")),
                                     LayoutY = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(layoutNode, "LayoutY")),
                                     Server = Server,
                                     isImmerVision = Xml.GetFirstElementValueByTagName(layoutNode, "isImmerVision") == "true",
                                     isIncludeDevice = Xml.GetFirstElementValueByTagName(layoutNode, "isIncludeDevice") == "true",
                                     DefineLayout = Xml.GetFirstElementValueByTagName(layoutNode, "DefineLayout"),
                                     MountingType = MountingTypes.ToIndex(Xml.GetFirstElementValueByTagName(layoutNode, "MountingType")),
                                     LensSetting = LensSettings.ToIndex(Xml.GetFirstElementValueByTagName(layoutNode, "LensSetting")),
                                 };
                layout.Dewarps.AddRange(ConvertStringToBoolList(Xml.GetFirstElementValueByTagName(layoutNode, "Dewarps")));

                while (layout.Dewarps.Count < 4)
                {
                    layout.Dewarps.Add(false);
                }

                var width = Xml.GetFirstElementValueByTagName(layoutNode, "Width");
                if (!String.IsNullOrEmpty(width))
                    layout.WindowWidth = Convert.ToInt32(width);

                var height = Xml.GetFirstElementValueByTagName(layoutNode, "Height");
                if (!String.IsNullOrEmpty(height))
                    layout.WindowHeight = Convert.ToInt32(height);

                if (regions != null)
                {
                    XmlNodeList regionList = regions.GetElementsByTagName("Region");

                    foreach (XmlElement regionNode in regionList)
                    {
                        var regionId = Convert.ToUInt16(regionNode.GetAttribute("id"));
                        var subLayout = new SubLayout
                        {
                            DeviceLayout = layout,
                            Id = regionId,
                            ReadyState = ReadyState.Ready,
                            Name = Xml.GetFirstElementValueByTagName(regionNode, "Name"),
                            X = Convert.ToInt32(Xml.GetFirstElementValueByTagName(regionNode, "X")),
                            Y = Convert.ToInt32(Xml.GetFirstElementValueByTagName(regionNode, "Y")),
                            Width = Convert.ToInt32(Xml.GetFirstElementValueByTagName(regionNode, "Width")),
                            Height = Convert.ToInt32(Xml.GetFirstElementValueByTagName(regionNode, "Height")),
                            Server = Server,
                        };

                        subLayout.Dewarp = 0;
                        try
                        {
                            var dewarp = Xml.GetFirstElementValueByTagName(regionNode, "Dewarp");
                            if (dewarp != "")
                                subLayout.Dewarp = Convert.ToInt32(dewarp);
                        }
                        catch { }

                        if (layout.SubLayouts.ContainsKey(regionId))
                            layout.SubLayouts[regionId] = subLayout;
                        else
                            layout.SubLayouts.Add(regionId, subLayout);
                    }
                }

                foreach (UInt16 deviceId in listId)
                {
                    if (deviceId == 0)
                    {
                        layout.Items.Add(null);
                        continue;
                    }
                    if(Devices.ContainsKey(deviceId))
                        layout.Items.Add(Devices[deviceId]);
                    else
                        layout.Items.Add(null);
                }

                if (!DeviceLayouts.ContainsKey(id))
                    DeviceLayouts.Add(id, layout);
                else
                    DeviceLayouts[id] = layout;
            }
        }

        //<Layout id="1" name="layout 1">
          //<Name>layout 1<Name>
          //<Width>1280<Width> //640 x 2
          //<Height>960<Height> //480 x 2
          //<LayoutX>2<LayoutX>
          //<LayoutY>2<LayoutY>
          //<Items>1,2,3,4<Items> //device's id
          //<Regions>
            //<Region is="1">
              //<Name>0<Name>
              //<X>0<X>
              //<Y>0<Y>
              //<Width>0<Width>
              //<Height>0<Height>
              //</Region>
          //</Regions>
        //</Layout>

        private void SaveDeviceLayout()
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Layouts");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IDeviceLayout>(DeviceLayouts.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (var deviceLayout in sortResult)
            {
                var layoutNode = xmlDoc.CreateElement("Layout");
                layoutNode.SetAttribute("id", deviceLayout.Id.ToString());
                xmlRoot.AppendChild(layoutNode);
                
                deviceLayout.ReadyState = ReadyState.Ready;
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", deviceLayout.Name));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Width", deviceLayout.WindowWidth));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Height", deviceLayout.WindowHeight));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "LayoutX", deviceLayout.LayoutX));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "LayoutY", deviceLayout.LayoutY));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", DeviceConverter.DeviceViewToString(deviceLayout.Items)));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Dewarps", ConvertIBooleanToString(deviceLayout.Dewarps)));

                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "isImmerVision", deviceLayout.isImmerVision ? "true" : "false"));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "isIncludeDevice", deviceLayout.isIncludeDevice ? "true" : "false"));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DefineLayout", deviceLayout.DefineLayout));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "MountingType", MountingTypes.ToString(deviceLayout.MountingType)));
                layoutNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "LensSetting", LensSettings.ToString(deviceLayout.LensSetting)));

                var regionsNode = xmlDoc.CreateElement("Regions");
                layoutNode.AppendChild(regionsNode);
                foreach (var subLayout in deviceLayout.SubLayouts)
                {
                    if (subLayout.Value == null) continue;

                    subLayout.Value.ReadyState = ReadyState.Ready;
                    var regionNode = xmlDoc.CreateElement("Region");
                    regionNode.SetAttribute("id", subLayout.Key.ToString());

                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", subLayout.Value.Name));
                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "X", subLayout.Value.X));
                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Y", subLayout.Value.Y));
                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Width", subLayout.Value.Width));
                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Height", subLayout.Value.Height));
                    regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Dewarp", subLayout.Value.Dewarp));

                    regionsNode.AppendChild(regionNode);
                }
            }

            Xml.PostXmlToHttp(CgiSaveLayout, xmlDoc, Server.Credential);
        }

        public void CheckSubLayoutPositionAndResolution(IDevice changedDevice)
        {
            var resolutions = new List<Resolution>();

            foreach (var deviceLayout in DeviceLayouts)
            {
                //containes device not include changed device -> no need update resolution
                if (!deviceLayout.Value.Items.Contains(changedDevice)) continue;

                resolutions.Clear();
                resolutions.Add(Resolution.R640X480);

                foreach (var device in deviceLayout.Value.Items)
                {
                    var camera = device as ICamera;
                    if (camera != null && !resolutions.Contains(camera.StreamConfig.Resolution))
                    {
                        resolutions.Add(camera.StreamConfig.Resolution);
                    }
                }

                resolutions.Sort();

                var deviceResolution = Resolutions.ToIndex(deviceLayout.Value.WindowWidth + "x" + deviceLayout.Value.WindowHeight);
                if (!resolutions.Contains(deviceResolution))
                {
                    deviceLayout.Value.WindowWidth = 640;
                    deviceLayout.Value.WindowHeight = 480;

                    CheckSubLayoutRange(deviceLayout.Value);
                }
            }
        }

        public void CheckSubLayoutRange(IDeviceLayout deviceLayout)
        {
            var width = deviceLayout.WindowWidth * deviceLayout.LayoutX;
            var height = deviceLayout.WindowHeight * deviceLayout.LayoutY;

            foreach (var obj in deviceLayout.SubLayouts)
            {
                var sublayout = obj.Value;
                if (sublayout == null) continue;

                //reset over range sublayout to default position
                if ((sublayout.X + sublayout.Width > width) || (sublayout.Y + sublayout.Height > height))
                {
                    sublayout.X = 0;
                    sublayout.Y = 0;
                    sublayout.Width = deviceLayout.WindowWidth;
                    sublayout.Height = deviceLayout.WindowHeight;
                }
            }
        }
    }
}