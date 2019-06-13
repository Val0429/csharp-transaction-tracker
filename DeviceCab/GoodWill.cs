using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class GoodWillCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public Dictionary<GoodWillCompressionCondition, Compression[]> Compression = new Dictionary<GoodWillCompressionCondition, Compression[]>();
        public Dictionary<GoodWillResolutionCondition, Resolution[]> Resolution = new Dictionary<GoodWillResolutionCondition, Resolution[]>();
        public Dictionary<GoodWillStreamCondition, UInt16[]> Framerate = new Dictionary<GoodWillStreamCondition, UInt16[]>();
        public Dictionary<GoodWillBitrateCondition, Bitrate[]> Bitrate = new Dictionary<GoodWillBitrateCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, TvStandard tvStandard)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.TvStandard == tvStandard)
                {
                    return compressionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, CameraModel model, TvStandard tvStandard, Dictionary<UInt16, StreamConfig> streamConfigs)
        {
            if (Framerate.Count == 0) return null;

            UInt16[] framerates = GetFullFramerateByCondition(streamConfigs[streamId], streamId, tvStandard);
            if (framerates == null) return null;
            var framerateLists = new List<UInt16>(framerates);

            //while (framerateLists[framerateLists.Count - 1] > 15)
            //    framerateLists.RemoveAt(framerateLists.Count - 1);

            return (framerateLists.Count > 0) ? framerateLists.ToArray() : null;
        }

        private UInt16[] GetFullFramerateByCondition(StreamConfig streamConfig, UInt16 streamId, TvStandard tvStandard)
        {
            foreach (var framerates in Framerate)
            {
                if (framerates.Key.Compression == streamConfig.Compression &&
                    framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.StreamId == streamId && framerates.Key.TvStandard == tvStandard)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, TvStandard tvStandard, StreamConfig streamConfig)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.Compression == streamConfig.Compression &&
                    bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.StreamId == streamId && bitrates.Key.TvStandard == tvStandard && bitrates.Key.FrameRates.Contains(streamConfig.Framerate))
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, TvStandard tvStandard, Compression compression)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutions in Resolution)
            {
                if (resolutions.Key.Compression == compression && resolutions.Key.StreamId == streamId && resolutions.Key.TvStandard == tvStandard)
                {
                    return resolutions.Value;
                }
            }

            return null;
        }
    }

    public class GoodWillCompressionCondition
    {
        public TvStandard TvStandard = TvStandard.NonSpecific;
        public UInt16 StreamId = 1;
    }

    public class GoodWillResolutionCondition : GoodWillCompressionCondition
    {
        public Compression Compression;
    }

    public class GoodWillStreamCondition : GoodWillResolutionCondition
    {
        public Resolution Resolution;
    }

    public class GoodWillBitrateCondition : GoodWillStreamCondition
    {
        public List<UInt16> FrameRates;
    }

    public partial class ParseCameraModel
    {
        public static void ParseGoodWill(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new GoodWillCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyGoodWillProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            cameraModel.CameraMode.Clear();

            var tvStandards = node.GetElementsByTagName("TVStandard");

            foreach (XmlElement tvStandardNode in tvStandards)
            {
                var tvStandardStr = tvStandardNode.GetAttribute("value");
                if(String.IsNullOrEmpty(tvStandardStr)) continue;

                var tvStandardArr = tvStandardStr.Split(',');

                var profileModes = tvStandardNode.GetElementsByTagName("ProfileMode");
                if (profileModes.Count == 0) continue;

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

                    foreach (String item in tvStandardArr)
                    {
                        var tvStandard = TvStandards.ToIndex(item);

                        if (!cameraModel.TvStandard.Contains(tvStandard))
                            cameraModel.TvStandard.Add(tvStandard);

                        foreach (XmlElement profileNode in profiles)
                        {
                            ParseGoodWillProfile(cameraModel, profileNode, tvStandard);
                        }
                    }

                }
            }

        }

        private static void ParseGoodWillProfile(GoodWillCameraModel cameraModel, XmlElement node, TvStandard tvStandard)
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

                var resolutionConditions = new List<GoodWillResolutionCondition>();

                foreach (var compression in compressionArray)
                {
                    resolutionConditions.Add(new GoodWillResolutionCondition { StreamId = id, TvStandard = tvStandard, Compression = compression });
                }
                //-----------------------------------
                var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                var resolutionList = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                    Array.Sort(resolutionArray);

                    resolutionList.AddRange(resolutionArray);

                    var streamConditions = new List<GoodWillStreamCondition>();
                    foreach (GoodWillResolutionCondition condition in resolutionConditions)
                    {
                        foreach (var resolution in resolutionArray)
                        {
                            streamConditions.Add(new GoodWillStreamCondition { StreamId = id, TvStandard = tvStandard, Compression = condition.Compression, Resolution = resolution });
                        }
                    }

                    //-----------------------------------
                    var frameList = new List<UInt16>();
                    var framerateNodes = resolutionNode.GetElementsByTagName("FrameRate");
                    if (framerateNodes.Count > 0)
                    {
                        foreach (XmlElement framerateNode in framerateNodes)
                        {
                            var bitrateConditions = new List<GoodWillStreamCondition>();
                            var framerateStr = framerateNode.GetAttribute("value");

                            if (!String.IsNullOrEmpty(framerateStr))
                            {
                                var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                                Array.Sort(framerates);

                                frameList.AddRange(framerates);

                                foreach (GoodWillStreamCondition condition in streamConditions)
                                {
                                    bitrateConditions.Add(new GoodWillBitrateCondition { StreamId = id, TvStandard = tvStandard, Compression = condition.Compression, Resolution = condition.Resolution, FrameRates = new List<UInt16>(framerates) });
                                }

                                //-----------------------------------
                                var bitrateStr = Xml.GetFirstElementValueByTagName(framerateNode, "Bitrate");

                                if (bitrateStr == "") continue;

                                var bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                                Array.Sort(bitrates);
 
                                foreach (GoodWillBitrateCondition condition in bitrateConditions)
                                {
                                    cameraModel.Bitrate.Add(condition, bitrates);
                                }
                                //-----------------------------------
                            }

                        }
                    }

                    foreach (GoodWillStreamCondition condition in streamConditions)
                    {
                        cameraModel.Framerate.Add(condition, frameList.ToArray());
                    }

                }
                foreach (var condition in resolutionConditions)
                {
                    cameraModel.Resolution.Add(condition, resolutionList.ToArray());
                }
            }

            cameraModel.Compression.Add(new GoodWillCompressionCondition{StreamId = id, TvStandard = tvStandard}, compressions.ToArray());
        }

        private static void CopyGoodWillProfile(XmlElement node, GoodWillCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            GoodWillCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (GoodWillCameraModel)mode;
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
            cameraModel.TvStandard = copyFrom.TvStandard;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}
