using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class BasicCameraModel : CameraModel
    {
        public Compression[] Compression;
        public UInt16[] Framerate;
        public Resolution[] Resolution;
        public Bitrate[] Bitrate;
    }

    public partial class ParseCameraModel
    {
        public static void ParseStandard(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new BasicCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var compression = Xml.GetFirstElementValueByTagName(node, "Compression");
            if (!String.IsNullOrEmpty(compression))
            {
                cameraModel.Compression = Array.ConvertAll(compression.Split(','), Compressions.ToIndex);
                Array.Sort(cameraModel.Compression);
            }

            var frameRate = Xml.GetFirstElementValueByTagName(node, "FrameRate");
            if (!String.IsNullOrEmpty(frameRate))
            {
                cameraModel.Framerate = Array.ConvertAll(frameRate.Split(','), Convert.ToUInt16);
                Array.Sort(cameraModel.Framerate);
            }

            var bitrate = Xml.GetFirstElementValueByTagName(node, "Bitrate");
            if (!String.IsNullOrEmpty(bitrate))
            {
                cameraModel.Bitrate = Array.ConvertAll(bitrate.Split(','), Bitrates.DisplayStringToIndex);
                Array.Sort(cameraModel.Bitrate);
            }

            var resolution = Xml.GetFirstElementValueByTagName(node, "Resolution");
            if (!String.IsNullOrEmpty(resolution))
            {
                cameraModel.Resolution = Array.ConvertAll(resolution.Split(','), Resolutions.ToIndex);
                Array.Sort(cameraModel.Resolution);
            }
        }

        public static Boolean ParseStandardCameraModel(CameraModel cameraModel, XmlElement node, List<CameraModel> list)
        {
            var manufacture = Xml.GetFirstElementValueByTagName(node, "Manufacture");

            var model = Xml.GetFirstElementValueByTagName(node, "Model");

            foreach (var mode in list)
            {
                if (String.Equals(mode.Model, model)) return false;
            }
            list.Add(cameraModel);

            cameraModel.Manufacture = manufacture;
            cameraModel.Model = model;
            var alias = Xml.GetFirstElementValueByTagName(node, "Alias");
            cameraModel.Alias = (String.Equals(alias, "") ? model : alias);
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");
            cameraModel.Type = Xml.GetFirstElementValueByTagName(node, "Type");
            cameraModel.ConnectionProtocol = Array.ConvertAll(
                Xml.GetFirstElementValueByTagName(node, "ConnectionProtocol").
                    Split(','), ConnectionProtocols.ToIndex);

            var numberOfChannel = Xml.GetFirstElementValueByTagName(node, "NumberOfChannel");
            if (!String.IsNullOrEmpty(numberOfChannel))
                cameraModel.NumberOfChannel = Convert.ToUInt16(numberOfChannel);

            var numberOfAudioIn = Xml.GetFirstElementValueByTagName(node, "NumberOfAudioIn");
            if (!String.IsNullOrEmpty(numberOfAudioIn))
                cameraModel.NumberOfAudioIn = Convert.ToUInt16(numberOfAudioIn);

            var numberOfAudioOut = Xml.GetFirstElementValueByTagName(node, "NumberOfAudioOut");
            if (!String.IsNullOrEmpty(numberOfAudioOut))
                cameraModel.NumberOfAudioOut = Convert.ToUInt16(numberOfAudioOut);

            var numberOfDI = Xml.GetFirstElementValueByTagName(node, "NumberOfDI");
            if (!String.IsNullOrEmpty(numberOfDI))
                cameraModel.NumberOfDi = Convert.ToUInt16(numberOfDI);

            var numberOfDO = Xml.GetFirstElementValueByTagName(node, "NumberOfDO");
            if (!String.IsNullOrEmpty(numberOfDO))
                cameraModel.NumberOfDo = Convert.ToUInt16(numberOfDO);

            var numberOfMotion = Xml.GetFirstElementValueByTagName(node, "NumberOfMotion");
            if (!String.IsNullOrEmpty(numberOfMotion))
                cameraModel.NumberOfMotion = Convert.ToUInt16(numberOfMotion);

            var panSupport = Xml.GetFirstElementValueByTagName(node, "PanSupport");
            if (!String.IsNullOrEmpty(panSupport))
                cameraModel.PanSupport = Convert.ToBoolean(panSupport);

            var tiltSupport = Xml.GetFirstElementValueByTagName(node, "TiltSupport");
            if (!String.IsNullOrEmpty(tiltSupport))
                cameraModel.TiltSupport = Convert.ToBoolean(tiltSupport);

            var zoomSupport = Xml.GetFirstElementValueByTagName(node, "ZoomSupport");
            if (!String.IsNullOrEmpty(zoomSupport))
                cameraModel.ZoomSupport = Convert.ToBoolean(zoomSupport);

            var focusSupport = Xml.GetFirstElementValueByTagName(node, "FocusSupport");
            if (!String.IsNullOrEmpty(focusSupport))
                cameraModel.FocusSupport = Convert.ToBoolean(focusSupport);

            var autoTrackingSupport = Xml.GetFirstElementValueByTagName(node, "AutoTrackingSupport");
            if (!String.IsNullOrEmpty(autoTrackingSupport))
                cameraModel.AutoTrackingSupport = Convert.ToBoolean(autoTrackingSupport);

            return true;
        }

        public static void SameAsCameraModel(CameraModel cameraModel, XmlElement node, CameraModel copyFrom)
        {
            var numberOfAudioIn = Xml.GetFirstElementValueByTagName(node, "NumberOfAudioIn");
            cameraModel.NumberOfAudioIn = String.Equals(numberOfAudioIn, "") ? copyFrom.NumberOfAudioIn : Convert.ToUInt16(numberOfAudioIn);

            var numberOfAudioOut = Xml.GetFirstElementValueByTagName(node, "NumberOfAudioOut");
            cameraModel.NumberOfAudioOut = String.Equals(numberOfAudioOut, "") ? copyFrom.NumberOfAudioOut : Convert.ToUInt16(numberOfAudioOut);

            var numberOfDI = Xml.GetFirstElementValueByTagName(node, "NumberOfDI");
            cameraModel.NumberOfDi = String.Equals(numberOfDI, "") ? copyFrom.NumberOfDi : Convert.ToUInt16(numberOfDI);

            var numberOfDO = Xml.GetFirstElementValueByTagName(node, "NumberOfDO");
            cameraModel.NumberOfDo = String.Equals(numberOfDO, "") ? copyFrom.NumberOfDo : Convert.ToUInt16(numberOfDO);

            var panSupport = Xml.GetFirstElementValueByTagName(node, "PanSupport");
            cameraModel.PanSupport = String.Equals(panSupport, "") ? copyFrom.PanSupport : Convert.ToBoolean(panSupport);

            var tiltSupport = Xml.GetFirstElementValueByTagName(node, "TiltSupport");
            cameraModel.TiltSupport = String.Equals(tiltSupport, "") ? copyFrom.TiltSupport : Convert.ToBoolean(tiltSupport);

            var zoomSupport = Xml.GetFirstElementValueByTagName(node, "ZoomSupport");
            cameraModel.ZoomSupport = String.Equals(zoomSupport, "") ? copyFrom.ZoomSupport : Convert.ToBoolean(zoomSupport);

            var focusSupport = Xml.GetFirstElementValueByTagName(node, "FocusSupport");
            cameraModel.FocusSupport = String.Equals(focusSupport, "") ? copyFrom.FocusSupport : Convert.ToBoolean(focusSupport);

            var autoTrackingSupport = Xml.GetFirstElementValueByTagName(node, "AutoTrackingSupport");
            cameraModel.AutoTrackingSupport = String.Equals(autoTrackingSupport, "") ? copyFrom.AutoTrackingSupport : Convert.ToBoolean(autoTrackingSupport);
        }
    }
}
