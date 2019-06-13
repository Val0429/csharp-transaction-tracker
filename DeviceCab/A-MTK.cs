using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class AMTKCameraModel : CameraModel
    {
        public List<SensorMode> SensorMode = new List<SensorMode>
        {
            DeviceConstant.SensorMode.NonSpecific
        };
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<AMTKCompression> Compression = new List<AMTKCompression>();
        public Dictionary<AMTKResolutionCondition, Resolution[]> Resolution = new Dictionary<AMTKResolutionCondition, Resolution[]>();
        public Dictionary<AMTKStreamCondition, UInt16[]> Framerate = new Dictionary<AMTKStreamCondition, UInt16[]>();
        public Dictionary<AMTKStreamCondition, Bitrate[]> Bitrate = new Dictionary<AMTKStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, SensorMode sensorMode, TvStandard tvStandard)
        {
            if (Compression.Count == 0) return null;

            var result = new List<Compression>();

            foreach (AMTKCompression compressionse in Compression)
            {
                if (compressionse.StreamId == streamId && compressionse.SensorMode == sensorMode)
                {
                    if (!result.Contains(compressionse.Compression))
                        result.Add(compressionse.Compression);
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, SensorMode sensorMode, TvStandard tvStandard, CameraModel model, StreamConfig streamConfig)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.SensorMode == sensorMode && framerates.Key.TvStandard == tvStandard && framerates.Key.Compression == streamConfig.Compression &&
                    framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, SensorMode sensorMode, TvStandard tvStandard, StreamConfig streamConfig)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.SensorMode == sensorMode && bitrates.Key.Compression == streamConfig.Compression && bitrates.Key.TvStandard == tvStandard &&
                    bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.StreamId == streamId)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, SensorMode sensorMode, Compression compression)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutions in Resolution)
            {
                if (resolutions.Key.SensorMode == sensorMode && resolutions.Key.Compression == compression && resolutions.Key.StreamId == streamId)
                {
                    Array.Sort(resolutions.Value);
                    return resolutions.Value;
                }
            }

            return null;
        }
    }

    public class AMTKCompression
    {
        public SensorMode SensorMode;
        public TvStandard TvStandard;
        public UInt16 StreamId = 1;
        public Compression Compression;
    }

    public class AMTKResolutionCondition
    {
        public SensorMode SensorMode;
        public TvStandard TvStandard;
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class AMTKStreamCondition : AMTKResolutionCondition
    {
        public Resolution Resolution;
    }

    public partial class ParseCameraModel
    {
        public static void ParseAMTK(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new AMTKCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            cameraModel.CameraMode.Clear();

            var sensorModes = node.GetElementsByTagName("SensorMode");
            if (sensorModes.Count > 0)
             {
                 foreach (XmlElement profileSensorModeNode in sensorModes)
                 {
                     var sensorMode = SensorModes.ToIndex(profileSensorModeNode.GetAttribute("value"));
                     ParseAMTKSensorModeProfile(cameraModel, sensorMode, TvStandard.NonSpecific, profileSensorModeNode);

                     if (!cameraModel.SensorMode.Contains(sensorMode))
                         cameraModel.SensorMode.Add(sensorMode);
                 }
             }
            else
            {
                ParseAMTKSensorModeProfile(cameraModel, SensorMode.NonSpecific, TvStandard.NonSpecific, node);
            }

            if (cameraModel.SensorMode.Count > 1)
                cameraModel.SensorMode.Remove(SensorMode.NonSpecific);
        }

        private static void ParseAMTKSensorModeProfile(AMTKCameraModel cameraModel, SensorMode sensorMode, TvStandard tvStandard, XmlElement node)
        {
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

                    case "4":
                        cameraMode = CameraMode.Multi;
                        break;

                    case "5":
                        cameraMode = CameraMode.Five;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                foreach (XmlElement profileNode in profiles)
                {
                    ParseAMTKProfile(cameraModel, sensorMode, tvStandard, profileNode);
                }
            }
        }

        private static void ParseAMTKProfile(AMTKCameraModel cameraModel, SensorMode sensorMode, TvStandard tvStandard, XmlElement node)
        {
            var compressionNodes = node.GetElementsByTagName("Compression");
            if (compressionNodes.Count == 0) return;

            var ids = node.GetAttribute("id").Split(',');

            foreach (String idString in ids)
            {
                UInt16 id = Convert.ToUInt16(idString);

                var compressions = new List<Compression>();
                foreach (XmlElement compressionNode in compressionNodes)
                {
                    if (compressionNode == null) continue;

                    var compressionArray = Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex);

                    compressions.AddRange(compressionArray);

                    var resolutionConditions = new List<AMTKResolutionCondition>();

                    foreach (var compression in compressionArray)
                    {
                        resolutionConditions.Add(new AMTKResolutionCondition { StreamId = id, SensorMode = sensorMode, TvStandard = tvStandard, Compression = compression });
                    }
                    //-----------------------------------
                    var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                    var resolutionList = new List<Resolution>();
                    foreach (XmlElement resolutionNode in resolutionNodes)
                    {
                        var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        Array.Sort(resolutionArray);

                        resolutionList.AddRange(resolutionArray);

                        var streamConditions = new List<AMTKStreamCondition>();
                        foreach (AMTKResolutionCondition condition in resolutionConditions)
                        {
                            foreach (Resolution resolution in resolutionList)
                            {
                                streamConditions.Add(new AMTKStreamCondition { StreamId = id, SensorMode = sensorMode, Compression = condition.Compression, Resolution = resolution, TvStandard = tvStandard });
                            }
                        }

                        //-----------------------------------
                        var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                        if (framerateStr != "")
                        {
                            var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                            Array.Sort(framerates);

                            foreach (AMTKStreamCondition condition in streamConditions)
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
                    cameraModel.Compression.Add(new AMTKCompression { Compression = compression, StreamId = id, SensorMode = sensorMode, TvStandard = tvStandard });
                }
            }

        }
    }
}
