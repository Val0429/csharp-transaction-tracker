using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class EverFocusCameraModel : CameraModel
    {
        public Dictionary<UInt16, Compression[]> Compression = new Dictionary<UInt16, Compression[]>();
        public Dictionary<UInt16, Resolution[]> Resolution = new Dictionary<UInt16, Resolution[]>();
        public Dictionary<UInt16, UInt16[]> Framerate = new Dictionary<UInt16, UInt16[]>();
        public Dictionary<UInt16, Bitrate[]> Bitrate = new Dictionary<UInt16, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key == streamId)
                    return compressionse.Value;
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key == streamId)
                    return resolutionse.Value;
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerate in Framerate)
            {
                if (framerate.Key == streamId)
                    return framerate.Value;
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitratese in Bitrate)
            {
                if (bitratese.Key == streamId)
                    return bitratese.Value;
            }

            return null;
        }
    }

    public partial class ParseCameraModel
    {
        public static void ParseEverFocus(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new EverFocusCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var profileModeNode = Xml.GetFirstElementByTagName(node, "ProfileMode");

            var profiles = profileModeNode.SelectNodes("Profile");
            if (profiles == null || profiles.Count == 0) return;

            cameraModel.CameraMode.Clear();
            String mode = profileModeNode.GetAttribute("value");
            switch (mode)
            {
                case "2":
                    cameraModel.CameraMode.Add(CameraMode.Dual);
                    break;

                case "3":
                    cameraModel.CameraMode.Add(CameraMode.Triple);
                    break;
            }

            foreach (XmlElement profileNode in profiles)
            {
                ParseEverFocusProfile(profileNode, cameraModel);
            }
        }

        private static void ParseEverFocusProfile(XmlElement streamNode, EverFocusCameraModel cameraModel)
        {
            if (streamNode.InnerXml == "") return;

            var id = Convert.ToUInt16(streamNode.GetAttribute("id"));
            var compress = new List<Compression>();
            compress.AddRange(Array.ConvertAll(Xml.GetFirstElementValueByTagName(streamNode, "Compression").Split(','), Compressions.ToIndex));

            var resolutionStr = Xml.GetFirstElementValueByTagName(streamNode, "Resolution");
            if (resolutionStr == "") return;
            var resolutions = Array.ConvertAll(resolutionStr.Split(','), Resolutions.ToIndex);
            Array.Sort(resolutions);
            cameraModel.Resolution.Add(id, resolutions);

            var framerateStr = Xml.GetFirstElementValueByTagName(streamNode, "FrameRate");
            if (framerateStr == "") return;
            cameraModel.Framerate.Add(id, Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16));

            var bitrateStr = Xml.GetFirstElementValueByTagName(streamNode, "Bitrate");
            if (bitrateStr == "") return;
            cameraModel.Bitrate.Add(id, Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex));

            compress.Sort();
            if (id != 1)
                compress.Add(Compression.Off);
            cameraModel.Compression.Add(id, compress.ToArray());
        }
    }
}
