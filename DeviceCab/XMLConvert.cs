using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace DeviceCab
{
    public  class XMLConvert
    {
        public static void ConvertSensorModeToXml(ICamera camera, XmlElement deviceSetting, XmlDocument xmlDoc)
        {
            if (camera.Profile.SensorMode == SensorMode.NonSpecific)
                deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SensorMode", ""));
            else
            {
                var sensorNode = Xml.CreateXmlElementWithText(xmlDoc, "SensorMode", SensorModes.ToString(camera.Profile.SensorMode));
                switch (camera.Model.Manufacture)
                {
                    case "ETROVISION":
                    case "IPSurveillance":
                    case "XTS":
                        sensorNode.SetAttribute("maximum", SensorModes.ToString(((EtrovisionCameraModel)camera.Model).SensorMode.Last()));
                        break;

                    case "Axis":
                        sensorNode.SetAttribute("maximum", SensorModes.ToString(((AxisCameraModel)camera.Model).SensorMode.Last()));
                        break;
                }
                deviceSetting.AppendChild(sensorNode);
            }
        }
        
        public static void ConvertViewingWindowToXml(ICamera camera, XmlElement deviceSetting, XmlDocument xmlDoc)
        {
            switch (camera.Model.Manufacture)
            {
                case "VIVOTEK":
                    var viewingWindow = ((VIVOTEKCameraModel)camera.Model).ViewingWindows.ContainsKey(camera.Profile.SensorMode)
                        ? ((VIVOTEKCameraModel)camera.Model).ViewingWindows[camera.Profile.SensorMode]
                        : String.Empty;
                    deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ViewingWindow", viewingWindow));
                    break;

                default:
                    //deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ViewingWindow", camera.Model.ViewingWindow));
                    break;
            }
        }

        public static void ConvertProfileModeToXml(ICamera camera, XmlElement streamConfig, XmlDocument xmlDoc, StreamConfig config)
        {
            switch (camera.Model.Manufacture)
            {
                case "BOSCH":
                    streamConfig.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ProfileMode", config.ProfileMode));
                    break;
            }
        }

        public static void ConvertRegionToXml(ICamera camera, XmlElement video, XmlDocument xmlDoc, StreamConfig config)
        {
            switch (camera.Model.Manufacture)
            {
                case "ArecontVision":
                    // x and y MUST match 32 (like resolution)
                    var regionX = Convert.ToInt32(Math.Round(config.RegionStartPointX / 32.0) * 32);
                    var regionY = Convert.ToInt32(Math.Round(config.RegionStartPointY / 32.0) * 32);
                    video.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegionStartPointX", regionX));
                    video.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegionStartPointY", regionY));
                    video.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "MotionThreshold", config.MotionThreshold));
                    video.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "BitrateControl", BitrateControls.ToString(config.BitrateControl)));
                    break;
            }
        }

        public static void ConvertRecordStreamToXml(ICamera camera, XmlElement deviceSetting, XmlDocument xmlDoc, Boolean hasStream)
        {
            switch (camera.Model.Manufacture)
            {
                //acti in fourvga / sixvga, record stream follow live stream.
                case "ACTi":
                    if (camera.Mode == CameraMode.FourVga || camera.Mode == CameraMode.SixVga)
                        deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RecordStream", (hasStream) ? camera.Profile.StreamId : 0));
                    else
                        deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RecordStream", (hasStream) ? camera.Profile.RecordStreamId : 0));
                    break;

                default:
                    deviceSetting.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RecordStream", (hasStream) ? camera.Profile.RecordStreamId : 0));
                    break;
            }
        }

        public static void ParsePTZCommand(ICamera camera, XmlDocument xmlDoc, XmlNode deviceSetting)
        {
            if (camera.Model.Manufacture != "Customization") return;

            var ptzCommand = xmlDoc.CreateElement("PTZCommand");
            deviceSetting.AppendChild(ptzCommand);

            var cmdMoveUp = xmlDoc.CreateElement("cmdMoveUp");
            ptzCommand.AppendChild(cmdMoveUp);
            ParsePTZCommandToXml(xmlDoc, cmdMoveUp, camera.Profile.PtzCommand.Up);

            var cmdMoveDown = xmlDoc.CreateElement("cmdMoveDown");
            ptzCommand.AppendChild(cmdMoveDown);
            ParsePTZCommandToXml(xmlDoc, cmdMoveDown, camera.Profile.PtzCommand.Down);

            var cmdMoveRight = xmlDoc.CreateElement("cmdMoveRight");
            ptzCommand.AppendChild(cmdMoveRight);
            ParsePTZCommandToXml(xmlDoc, cmdMoveRight, camera.Profile.PtzCommand.Right);

            var cmdMoveLeft = xmlDoc.CreateElement("cmdMoveLeft");
            ptzCommand.AppendChild(cmdMoveLeft);
            ParsePTZCommandToXml(xmlDoc, cmdMoveLeft, camera.Profile.PtzCommand.Left);

            var cmdMoveUpRight = xmlDoc.CreateElement("cmdMoveUpRight");
            ptzCommand.AppendChild(cmdMoveUpRight);
            ParsePTZCommandToXml(xmlDoc, cmdMoveUpRight, camera.Profile.PtzCommand.UpRight);

            var cmdMoveDownRight = xmlDoc.CreateElement("cmdMoveDownRight");
            ptzCommand.AppendChild(cmdMoveDownRight);
            ParsePTZCommandToXml(xmlDoc, cmdMoveDownRight, camera.Profile.PtzCommand.DownRight);

            var cmdMoveUpLeft = xmlDoc.CreateElement("cmdMoveUpLeft");
            ptzCommand.AppendChild(cmdMoveUpLeft);
            ParsePTZCommandToXml(xmlDoc, cmdMoveUpLeft, camera.Profile.PtzCommand.UpLeft);

            var cmdMoveDownLeft = xmlDoc.CreateElement("cmdMoveDownLeft");
            ptzCommand.AppendChild(cmdMoveDownLeft);
            ParsePTZCommandToXml(xmlDoc, cmdMoveDownLeft, camera.Profile.PtzCommand.DownLeft);

            var cmdMoveStop = xmlDoc.CreateElement("cmdMoveStop");
            ptzCommand.AppendChild(cmdMoveStop);
            ParsePTZCommandToXml(xmlDoc, cmdMoveStop, camera.Profile.PtzCommand.Stop);

            var cmdZoomIn = xmlDoc.CreateElement("cmdZoomIn");
            ptzCommand.AppendChild(cmdZoomIn);
            ParsePTZCommandToXml(xmlDoc, cmdZoomIn, camera.Profile.PtzCommand.ZoomIn);

            var cmdZoomOut = xmlDoc.CreateElement("cmdZoomOut");
            ptzCommand.AppendChild(cmdZoomOut);
            ParsePTZCommandToXml(xmlDoc, cmdZoomOut, camera.Profile.PtzCommand.ZoomOut);

            var cmdZoomStop = xmlDoc.CreateElement("cmdZoomStop");
            ptzCommand.AppendChild(cmdZoomStop);
            ParsePTZCommandToXml(xmlDoc, cmdZoomStop, camera.Profile.PtzCommand.ZoomStop);

            var cmdFocusIn = xmlDoc.CreateElement("cmdFocusIn");
            ptzCommand.AppendChild(cmdFocusIn);
            ParsePTZCommandToXml(xmlDoc, cmdFocusIn, camera.Profile.PtzCommand.FocusIn);

            var cmdFocusOut = xmlDoc.CreateElement("cmdFocusOut");
            ptzCommand.AppendChild(cmdFocusOut);
            ParsePTZCommandToXml(xmlDoc, cmdFocusOut, camera.Profile.PtzCommand.FocusOut);

            var cmdFocusStop = xmlDoc.CreateElement("cmdFocusStop");
            ptzCommand.AppendChild(cmdFocusStop);
            ParsePTZCommandToXml(xmlDoc, cmdFocusStop, camera.Profile.PtzCommand.FocusStop);

            var cmdAddPTZPreset = xmlDoc.CreateElement("cmdAddPTZPreset");
            ptzCommand.AppendChild(cmdAddPTZPreset);
            foreach (KeyValuePair<ushort, PtzCommandCgi> ptzCommandCgi in camera.Profile.PtzCommand.PresetPoints)
            {
                if (String.IsNullOrEmpty(ptzCommandCgi.Value.Cgi)) continue;
                var cmdPresetPoint = xmlDoc.CreateElement("Point");
                cmdPresetPoint.SetAttribute("id", ptzCommandCgi.Key.ToString());
                cmdAddPTZPreset.AppendChild(cmdPresetPoint);
                ParsePTZCommandToXml(xmlDoc, cmdPresetPoint, ptzCommandCgi.Value);
            }

            var cmdDelPTZPreset = xmlDoc.CreateElement("cmdDelPTZPreset");
            ptzCommand.AppendChild(cmdDelPTZPreset);
            foreach (KeyValuePair<ushort, PtzCommandCgi> ptzCommandCgi in camera.Profile.PtzCommand.DeletePresetPoints)
            {
                if (String.IsNullOrEmpty(ptzCommandCgi.Value.Cgi)) continue;
                var cmdPresetPoint = xmlDoc.CreateElement("Point");
                cmdPresetPoint.SetAttribute("id", ptzCommandCgi.Key.ToString());
                cmdDelPTZPreset.AppendChild(cmdPresetPoint);
                ParsePTZCommandToXml(xmlDoc, cmdPresetPoint, ptzCommandCgi.Value);
            }

            var cmdPTZPresetGo = xmlDoc.CreateElement("cmdPTZPresetGo");
            ptzCommand.AppendChild(cmdPTZPresetGo);
            foreach (KeyValuePair<ushort, PtzCommandCgi> ptzCommandCgi in camera.Profile.PtzCommand.GotoPresetPoints)
            {
                if (String.IsNullOrEmpty(ptzCommandCgi.Value.Cgi)) continue;
                var cmdPresetPoint = xmlDoc.CreateElement("Point");
                cmdPresetPoint.SetAttribute("id", ptzCommandCgi.Key.ToString());
                cmdPTZPresetGo.AppendChild(cmdPresetPoint);
                ParsePTZCommandToXml(xmlDoc, cmdPresetPoint, ptzCommandCgi.Value);
            }
        }

        private static void ParsePTZCommandToXml(XmlDocument xmlDoc, XmlElement node, PtzCommandCgi cmd)
        {
            if (!String.IsNullOrEmpty(cmd.Cgi))
            {
                node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Method", PtzCommandMethods.ToString(cmd.Method)));
                node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Cgi", cmd.Cgi));
                if (cmd.Method == PtzCommandMethod.Post)
                    node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Parameter", cmd.Parameter));
            }
        }

        public static Boolean CheckIfNeedParseStreamConfig(ICamera camera, StreamConfig config)
        {
            switch (camera.Model.Manufacture)
            {
                case "ONVIF":
                case "Kedacom":
                case "Customization":
                    break;

                case "Axis":
                    if (camera.Model.Type != "AudioBox")
                    {
                        if (config.Compression == Compression.Off)
                            return false;
                    }
                    break;

                default:
                    if (config.Compression == Compression.Off)
                        return false;
                    break;
            }

            return true;
        }
    }
}
