using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class MOBOTIXCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<MOBOTIXCompression> Compression = new List<MOBOTIXCompression>();
        public Dictionary<MOBOTIXResolutionCondition, Resolution[]> Resolution = new Dictionary<MOBOTIXResolutionCondition, Resolution[]>();
        public Dictionary<MOBOTIXStreamCondition, UInt16[]> Framerate = new Dictionary<MOBOTIXStreamCondition, UInt16[]>();
        public Dictionary<MOBOTIXStreamCondition, Bitrate[]> Bitrate = new Dictionary<MOBOTIXStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, TvStandard tvStandard)
        {
            if (Compression.Count == 0) return null;

            var result = new List<Compression>();

            foreach (MOBOTIXCompression compressionse in Compression)
            {
                if (compressionse.StreamId == streamId)
                {
                    if (!result.Contains(compressionse.Compression))
                        result.Add(compressionse.Compression);
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, TvStandard tvStandard, CameraModel model, StreamConfig streamConfig)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.TvStandard == tvStandard && framerates.Key.Compression == streamConfig.Compression &&
                    framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.StreamId == streamId)
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
                if (bitrates.Key.Compression == streamConfig.Compression && bitrates.Key.TvStandard == tvStandard &&
                    bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.StreamId == streamId)
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
                    Array.Sort(resolutions.Value);
                    return resolutions.Value;
                }
            }

            return null;
        }
    }

    public class MOBOTIXCompression
    {
        public TvStandard TvStandard;
        public UInt16 StreamId = 1;
        public Compression Compression;
    }

    public class MOBOTIXResolutionCondition
    {
        public TvStandard TvStandard;
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class MOBOTIXStreamCondition : MOBOTIXResolutionCondition
    {
        public Resolution Resolution;
    }

    public partial class ParseCameraModel
    {
        public static void ParseMOBOTIX(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new MOBOTIXCameraModel();
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
                    ParseMOBOTIXProfile(cameraModel, TvStandard.NonSpecific, profileNode);
                }
            }
        }

        private static void ParseMOBOTIXProfile(MOBOTIXCameraModel cameraModel, TvStandard tvStandard, XmlElement node)
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

                var resolutionConditions = new List<MOBOTIXResolutionCondition>();

                foreach (var compression in compressionArray)
                {
                    resolutionConditions.Add(new MOBOTIXResolutionCondition { StreamId = id, TvStandard = tvStandard, Compression = compression });
                }
                //-----------------------------------
                var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                var resolutionList = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                    Array.Sort(resolutionArray);

                    resolutionList.AddRange(resolutionArray);

                    var streamConditions = new List<MOBOTIXStreamCondition>();
                    foreach (MOBOTIXResolutionCondition condition in resolutionConditions)
                    {
                        foreach (Resolution resolution in resolutionList)
                        {
                            streamConditions.Add(new MOBOTIXStreamCondition { StreamId = id, Compression = condition.Compression, Resolution = resolution, TvStandard = tvStandard });
                        }
                    }

                    //-----------------------------------
                    var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                    if (framerateStr != "")
                    {
                        var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                        Array.Sort(framerates);

                        foreach (MOBOTIXStreamCondition condition in streamConditions)
                            cameraModel.Framerate.Add(condition, framerates);
                    }
                    //-----------------------------------
                    var bitrateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "Bitrate");

                    if (bitrateStr == "") continue;

                    var bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                    Array.Sort(bitrates);
                    foreach (var condition in streamConditions)
                        cameraModel.Bitrate.Add(condition, bitrates);
                    //-----------------------------------
                }
                foreach (var condition in resolutionConditions)
                {
                    cameraModel.Resolution.Add(condition, resolutionList.ToArray());
                }
            }

            foreach (Compression compression in compressions)
            {
                cameraModel.Compression.Add(new MOBOTIXCompression { Compression = compression, StreamId = id, TvStandard = tvStandard });
            }

        }
    }
}
