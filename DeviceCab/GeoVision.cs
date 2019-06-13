using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class GeoVisionCameraModel : CameraModel
    {
        public Dictionary<UInt16, Compression[]> Compression = new Dictionary<UInt16, Compression[]>();
        public Dictionary<UInt16, UInt16[]> Framerate = new Dictionary<UInt16, UInt16[]>();
        public Dictionary<UInt16, Resolution[]> Resolution = new Dictionary<UInt16, Resolution[]>();
        public Dictionary<GeoVisionStreamCondition, Bitrate[]> Bitrate = new Dictionary<GeoVisionStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key == streamId)
                {
                    return compressionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig primaryStreamConfig)
        {
            if (Resolution.Count == 0) return null;

            switch (streamId)
            {
                case 1:
                    foreach (var resolution in Resolution)
                    {
                        if (resolution.Key == streamId)
                        {
                            return resolution.Value;
                        }
                    }
                    break;

                case 2:
                    List<Resolution> resolutions = new List<Resolution>();
                    foreach (var resolution in Resolution)
                    {
                        if (resolution.Key == streamId)
                        {
                            resolutions.AddRange(resolution.Value);
                            break;
                        }
                    }

                    List<Resolution> subResolution = new List<Resolution>();
                    switch (primaryStreamConfig.Resolution)
                    {
                        case DeviceConstant.Resolution.R448X252:
                        case DeviceConstant.Resolution.R640X360:
                        case DeviceConstant.Resolution.R1280X720:
                            subResolution.AddRange(resolutions.Where(resolution => resolution == DeviceConstant.Resolution.R448X252 || resolution == DeviceConstant.Resolution.R640X360));
                            break;

                        case DeviceConstant.Resolution.R320X240:
                        case DeviceConstant.Resolution.R640X480:
                        case DeviceConstant.Resolution.R1280X960:
                            subResolution.AddRange(resolutions.Where(resolution => resolution == DeviceConstant.Resolution.R320X240 || resolution == DeviceConstant.Resolution.R640X480));
                            break;

                        case DeviceConstant.Resolution.R320X256:
                        case DeviceConstant.Resolution.R640X512:
                        case DeviceConstant.Resolution.R1280X1024:
                            subResolution.AddRange(resolutions.Where(resolution => resolution == DeviceConstant.Resolution.R320X256 || resolution == DeviceConstant.Resolution.R640X512));
                            break;
                    }

                    return subResolution.ToArray();
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.StreamId == streamId)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }
    }

    public class GeoVisionStreamCondition
    {
        public Resolution Resolution;
        public UInt16 StreamId = 1;
    }

    public partial class ParseCameraModel
    {
        public static void ParseGeoVision(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new GeoVisionCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.CameraMode.Clear();

            var profileModes = node.GetElementsByTagName("ProfileMode");
            if (profileModes.Count == 0) return;

            foreach (XmlElement profileModeNode in profileModes)
            {
                var profiles = profileModeNode.SelectNodes("Profile");
                if (profiles == null || profiles.Count == 0) continue;

                var mode = profileModeNode.GetAttribute("value");
                var cameraMode = CameraMode.Single;
                switch (mode)
                {
                    case "2":
                        cameraMode = CameraMode.Dual;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                foreach (XmlElement profileNode in profiles)
                {
                    ParseGeoVisionProfile(cameraModel, profileNode);
                }
            }
        }

        private static void ParseGeoVisionProfile(GeoVisionCameraModel cameraModel, XmlElement node)
        {
            var compressionStr = Xml.GetFirstElementValueByTagName(node, "Compression");
            if (compressionStr == "") return;

            var id = Convert.ToUInt16(node.GetAttribute("id"));
            var compressions = new List<Compression>(Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex));
            if (id == 2)
                compressions.Add(Compression.Off);
            cameraModel.Compression.Add(id, compressions.ToArray());

            var framerateStr = Xml.GetFirstElementValueByTagName(node, "FrameRate");
            if (framerateStr == "") return;
            cameraModel.Framerate.Add(id, Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16));

            var resolutionNodes = node.GetElementsByTagName("Resolution");
            if (resolutionNodes.Count == 0) return;

            var resolutions = new List<Resolution>();
            foreach (XmlElement resolutionNode in resolutionNodes)
            {
                var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                Array.Sort(resolutionArray);
                resolutions.AddRange(resolutionArray);

                var bitrateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "Bitrate");
                if (bitrateStr == "") return;

                foreach (var resolution in resolutionArray)
                {
                    cameraModel.Bitrate.Add(new GeoVisionStreamCondition { StreamId = id, Resolution = resolution },
                        Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex));
                }
            }
            resolutions.Sort();
            cameraModel.Resolution.Add(id, resolutions.ToArray());
        }
    }
}
