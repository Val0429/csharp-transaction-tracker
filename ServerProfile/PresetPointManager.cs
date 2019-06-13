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
        private const String CgiLoadAllPreset = @"cgi-bin/patrolconfig?action=loadallpreset";
        //private const String CgiDeleteAllPreset = @"cgi-bin/patrolconfig?action=deleteallpreset";
        //private const String CgiLoadPreset = @"cgi-bin/patrolconfig?channel=channel%1&action=loadpreset";
        private const String CgiSavePreset = @"cgi-bin/patrolconfig?channel=channel%1&action=savepreset";
        //private const String CgiDeletePreset = @"cgi-bin/patrolconfig?channel=channel%1&action=deletepreset";
        
        private void LoadAllPresetPoint()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPreset, Server.Credential);
            if (xmlDoc == null) return;

            var presetsList = xmlDoc.GetElementsByTagName("PresetPoints");
            foreach (XmlElement node in presetsList)
            {
                if (node.GetAttribute("id") == "") continue;
                var id = Convert.ToUInt16(node.GetAttribute("id"));
                if (!Devices.ContainsKey(id)) continue;

                if (Devices[id] is ICamera)
                {
                    LoadPresetPoint((ICamera)(Devices[id]), node);
                }
            }
        }

        private static void LoadPresetPoint(ICamera camera, XmlElement rootNode)
        {
            camera.PresetPoints.Clear();

            if (rootNode == null) return;
            if (!camera.Model.IsSupportPTZ) return;

            var presetPoints = rootNode.GetElementsByTagName("PresetPoint");
            if (presetPoints.Count > 0)
            {
                foreach (XmlElement presetPoint in presetPoints)
                {
                    var point = new PresetPoint
                                            {
                                                Id = Convert.ToUInt16(presetPoint.GetAttribute("id")),
                                                Name = presetPoint.InnerText,
                                            };
                    camera.PresetPoints.Add(point.Id, point);
                }
            }
        }

        private void SavePresetPoint()
        {
            foreach (KeyValuePair<UInt16, IDevice> obj in Devices)
            {
                var camera = obj.Value as ICamera;
                if (camera == null) continue;
                if (!camera.PresetPoints.IsModify) continue;

                Xml.PostXmlToHttp(CgiSavePreset.Replace("%1", obj.Key.ToString()), ParsePresetPointToXml(camera), Server.Credential);
            }
        }

        private static XmlDocument ParsePresetPointToXml(ICamera camera)
        {
            camera.PresetPoints.IsModify = false;

            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("PresetPoints");
            xmlRoot.SetAttribute("id", camera.Id.ToString());
            xmlDoc.AppendChild(xmlRoot);

            if (camera.PresetPoints != null)
            {
                var points = camera.PresetPoints.Values.OrderBy(g => g.Id);
                
                foreach (var point in points)
                {
                    var xmlele = Xml.CreateXmlElementWithText(xmlDoc, "PresetPoint", point.Name);
                    xmlele.SetAttribute("id", point.Id.ToString());
                    xmlRoot.AppendChild(xmlele);
                }
            }

            return xmlDoc;
        }
    }
}