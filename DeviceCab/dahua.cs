using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class DahuaCameraModel : CameraModel
    {
        public Dictionary<UInt16, Compression[]> Compression = new Dictionary<UInt16, Compression[]>();
        public Dictionary<DahuaResolutionCondition, Resolution[]> Resolution = new Dictionary<DahuaResolutionCondition, Resolution[]>();
        public Dictionary<DahuaStreamCondition, UInt16[]> Framerate = new Dictionary<DahuaStreamCondition, UInt16[]>();
        public Dictionary<DahuaBitrateCondition, Bitrate[]> Bitrate = new Dictionary<DahuaBitrateCondition, Bitrate[]>();

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

        public UInt16[] GetFramerateByCondition(UInt16 streamId, CameraModel model, Dictionary<UInt16, StreamConfig> streamConfigs)
        {
            if (Framerate.Count == 0) return null;

            if (model.Alias == "F7210")
            {
                if (!streamConfigs.ContainsKey(streamId)) return null;

                return GetFullFramerateByCondition(streamConfigs[streamId], streamId);
            }

            Boolean over640X480 = false;
            foreach (var obj in streamConfigs)
            {
                if (obj.Value.Resolution > DeviceConstant.Resolution.R640X480)
                {
                    over640X480 = true;
                    break;
                }
            }

            if (!over640X480)
                return GetFullFramerateByCondition(streamConfigs[streamId], streamId);

            UInt16[] framerates = GetFullFramerateByCondition(streamConfigs[streamId], streamId);
            if (framerates == null) return null;
            var framerateLists = new List<UInt16>(framerates);

            //while (framerateLists[framerateLists.Count - 1] > 15)
            //    framerateLists.RemoveAt(framerateLists.Count - 1);

            return (framerateLists.Count > 0) ? framerateLists.ToArray() : null;
        }

        private UInt16[] GetFullFramerateByCondition(StreamConfig streamConfig, UInt16 streamId)
        {
            foreach (var framerates in Framerate)
            {
                if (framerates.Key.Compression == streamConfig.Compression &&
                    framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.Compression == streamConfig.Compression &&
                    bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.StreamId == streamId && bitrates.Key.FrameRates.Contains(streamConfig.Framerate))
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, Compression compression)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutions in Resolution)
            {
                if (resolutions.Key.Compression == compression && resolutions.Key.StreamId == streamId)
                {
                    return resolutions.Value;
                }
            }

            return null;
        }
    }

    public class DahuaResolutionCondition
    {
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class DahuaStreamCondition : DahuaResolutionCondition
    {
        public Resolution Resolution;
    }

    public class DahuaBitrateCondition : DahuaStreamCondition
    {
        public List<UInt16> FrameRates;
    }

    public partial class ParseCameraModel
    {
        public static void ParseDahua(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new DahuaCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyDahuaProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

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

                    case "3":
                        cameraMode = CameraMode.Triple;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                foreach (XmlElement profileNode in profiles)
                {
                    ParseDahuaProfile(cameraModel, profileNode);
                }
            }
        }

        private static void ParseDahuaProfile(DahuaCameraModel cameraModel, XmlElement node)
        {
            var compressionNodes = node.GetElementsByTagName("Compression");
            if (compressionNodes.Count == 0) return;

            UInt16 id = Convert.ToUInt16(node.GetAttribute("id"));

            var compressions = new List<Compression>();
            foreach (XmlElement compressionNode in compressionNodes)
            {
                if (compressionNode == null) continue;

                var compressionArray = Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex);

                compressions.AddRange(compressionArray);

                var resolutionConditions = new List<DahuaResolutionCondition>();

                foreach (var compression in compressionArray)
                {
                    resolutionConditions.Add(new DahuaResolutionCondition { StreamId = id, Compression = compression });
                }
                //-----------------------------------
                var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                var resolutionList = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                    Array.Sort(resolutionArray);

                    resolutionList.AddRange(resolutionArray);

                    var streamConditions = new List<DahuaStreamCondition>();
                    foreach (DahuaResolutionCondition condition in resolutionConditions)
                    {
                        foreach (var resolution in resolutionArray)
                        {
                            streamConditions.Add(new DahuaStreamCondition { StreamId = id, Compression = condition.Compression, Resolution = resolution });
                        }
                    }

                    //-----------------------------------
                    var frameList = new List<UInt16>();
                    var framerateNodes = resolutionNode.GetElementsByTagName("FrameRate");
                    if (framerateNodes.Count > 0)
                    {
                        foreach (XmlElement framerateNode in framerateNodes)
                        {
                            var bitrateConditions = new List<DahuaStreamCondition>();
                            var framerateStr = framerateNode.GetAttribute("value");

                            if (!String.IsNullOrEmpty(framerateStr))
                            {
                                var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                                Array.Sort(framerates);

                                frameList.AddRange(framerates);

                                foreach (DahuaStreamCondition condition in streamConditions)
                                {
                                    bitrateConditions.Add(new DahuaBitrateCondition { StreamId = id, Compression = condition.Compression, Resolution = condition.Resolution, FrameRates = new List<UInt16>(framerates)});
                                }

                                //-----------------------------------
                                var bitrateStr = Xml.GetFirstElementValueByTagName(framerateNode, "Bitrate");

                                if (bitrateStr == "") continue;

                                var bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                                Array.Sort(bitrates);
 
                                foreach (DahuaBitrateCondition condition in bitrateConditions)
                                {
                                    cameraModel.Bitrate.Add(condition, bitrates);
                                }
                                //-----------------------------------
                            }

                        }
                    }

                    foreach (DahuaStreamCondition condition in streamConditions)
                    {
                        cameraModel.Framerate.Add(condition, frameList.ToArray());
                    }

                }
                foreach (var condition in resolutionConditions)
                {
                    cameraModel.Resolution.Add(condition, resolutionList.ToArray());
                }
            }

            cameraModel.Compression.Add(id, compressions.ToArray());
        }

        private static void CopyDahuaProfile(XmlElement node, DahuaCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            DahuaCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (DahuaCameraModel)mode;
                break;
            }
            if (copyFrom == null)
                return;

            var model = Xml.GetFirstElementValueByTagName(node, "Model");

            foreach (var mode in list)
            {
                if (String.Equals(mode.Model, model)) return;
            }

            list.Add(cameraModel);

            cameraModel.Manufacture = copyFrom.Manufacture;
            var type = Xml.GetFirstElementValueByTagName(node, "Type");
            cameraModel.Type = (String.Equals(type, "") ? copyFrom.Type : type);
            cameraModel.Model = model;

            var alias = Xml.GetFirstElementValueByTagName(node, "Alias");
            cameraModel.Alias = (String.Equals(alias, "") ? model : alias);

            cameraModel.Series = copyFrom.Series;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}
