using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String CgiLoadAllPatrol = @"cgi-bin/patrolconfig?action=loadallpatrol";
        //private const String CgiDeleteAllPatrol = @"cgi-bin/patrolconfig?action=deleteallpatrol";
        //private const String CgiLoadPatrol = @"cgi-bin/patrolconfig?channel=channel%1&action=loadpatrol";
        private const String CgiSavePatrol = @"cgi-bin/patrolconfig?channel=channel%1&action=savepatrol";
        //private const String CgiDeletePatrol = @"cgi-bin/patrolconfig?channel=channel%1&action=deletepatrol";
        
        private void LoadAllPresetTour()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPatrol, Server.Credential);
            if (xmlDoc == null) return;

            var toursList = xmlDoc.GetElementsByTagName("PatrolConfiguration");
            foreach (XmlElement node in toursList)
            {
                if (node.GetAttribute("id") == "") continue;
                var id = Convert.ToUInt16(node.GetAttribute("id"));
                if (!Devices.ContainsKey(id)) continue;

                if (Devices[id] is ICamera)
                {
                    LoadPresetTour((ICamera)(Devices[id]), node);
                }
            }
        }

        private static void LoadPresetTour(ICamera camera, XmlElement rootNode)
        {
            camera.PresetTours.Clear();

            if (rootNode == null) return;
            if (!camera.Model.IsSupportPTZ) return;

            var presetTours = rootNode.GetElementsByTagName("PresetTour");
            if (presetTours.Count > 0)
            {
                foreach (XmlElement presetTour in presetTours)
                {
                    var tour = new PresetTour
                    {
                        Id = Convert.ToUInt16(presetTour.GetAttribute("id")),
                        Name = presetTour.GetAttribute("name"),
                    };

                    String temp = presetTour.InnerText;
                    while (temp.Length > 0)
                    {
                        Int32 end = temp.IndexOf('"', temp.IndexOf('"') + 1);

                        if (end == -1) break;

                        UInt16[] data = temp.Substring(0, end).Split('"')[1].Split(',').Select(s => Convert.ToUInt16(s)).ToArray();

                        tour.Tour.Add(new TourPoint
                        {
                            Id = data[0],
                            Duration = data[1],
                        });

                        if (temp.Length < (end + 1))
                            break;

                        temp = temp.Substring(end + 1);
                    }

                    camera.PresetTours.Add(tour.Id, tour);
                }
            }

            String goTour = Xml.GetFirstElementValueByTagName(rootNode, "PresetTourGo");
            if ((goTour != ""))
                camera.PresetTourGo = Convert.ToUInt16(goTour);
        }

        private void SavePresetTour()
        {
            foreach (KeyValuePair<UInt16, IDevice> obj in Devices)
            {
                SavePresetTour(obj.Value as ICamera);
            }
        }

        public void SavePresetTour(ICamera camera)
        {
            if (camera == null) return;
            if (!camera.PresetTours.IsModify) return;

            Xml.PostXmlToHttp(CgiSavePatrol.Replace("%1", camera.Id.ToString()), ParsePresetTourToXml(camera), Server.Credential);
        }

        private static XmlDocument ParsePresetTourToXml(ICamera camera)
        {
            camera.PresetTours.IsModify = false;

            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("PatrolConfiguration");
            xmlRoot.SetAttribute("id", camera.Id.ToString());
            xmlDoc.AppendChild(xmlRoot);

            if (!camera.Model.IsSupportPTZ)
                camera.PresetTourGo = 0;

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "PresetTourGo", ((camera.PresetTourGo > 0) ? camera.PresetTourGo.ToString() : "")));

            var toursRoot = xmlDoc.CreateElement("PresetTours");
            xmlRoot.AppendChild(toursRoot);

            if (camera.PresetTours != null)
            {
                foreach (KeyValuePair<UInt16, PresetTour> obj in camera.PresetTours)
                {
                    var xmlele = Xml.CreateXmlElementWithText(xmlDoc, "PresetTour", obj.Value.PointToString());
                    xmlele.SetAttribute("id", obj.Key.ToString());
                    xmlele.SetAttribute("name", obj.Value.Name);
                    toursRoot.AppendChild(xmlele);
                }
            }

            return xmlDoc;
        }
    }
}