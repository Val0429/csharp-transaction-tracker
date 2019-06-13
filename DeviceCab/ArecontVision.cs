using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class ArecontVisionCameraModel : CameraModel
    {
        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency>
        {
            DeviceConstant.PowerFrequency.NonSpecific
        };
        public List<SensorMode> SensorMode = new List<SensorMode>
        {
            DeviceConstant.SensorMode.NonSpecific
        };
        public Dictionary<UInt16, Compression[]> Compression = new Dictionary<UInt16, Compression[]>();
        public Dictionary<ArecontVisionFramerateCondition, UInt16[]> Framerate = new Dictionary<ArecontVisionFramerateCondition, UInt16[]>();
        public Dictionary<UInt16, Resolution[]> Resolution = new Dictionary<UInt16, Resolution[]>();
        public Dictionary<UInt16, Bitrate[]> Bitrate = new Dictionary<UInt16, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol)
        {
            if (Compression.Count == 0) return null;

            foreach (KeyValuePair<UInt16, Compression[]> compresionItem in Compression)
            {
                if(compresionItem.Key == streamId)
                {
                    if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http) return compresionItem.Value;

                    var newCompression = new List<Compression>();
                    foreach (Compression compression in compresionItem.Value)
                    {
                        if (compression == DeviceConstant.Compression.Mjpeg) continue;
                        newCompression.Add(compression);
                    }
                    return newCompression.Count > 0 ? newCompression.ToArray() : null;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, Resolution resolution)
        {
            if (Framerate.Count == 0) return null;

            foreach (KeyValuePair<ArecontVisionFramerateCondition, UInt16[]> framerateItrm in Framerate)
            {
                if(framerateItrm.Key.StreamId == streamId && framerateItrm.Key.Resolution == resolution)
                {
                    return framerateItrm.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId)
        {
            if (Bitrate.Count == 0) return null;

            if (!Bitrate.ContainsKey(streamId)) return null;

            return Bitrate[streamId];
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId)
        {
            if (Resolution.Count == 0) return null;

            if (!Resolution.ContainsKey(streamId)) return null;

            return Resolution[streamId];
        }

        public Resolution GetMaximumResilotion()
        {
            if (Resolution.Count == 0) return DeviceConstant.Resolution.NA;
            if (!Resolution.ContainsKey(1)) return DeviceConstant.Resolution.NA;
            var resolutions = Resolution[1];
            return resolutions[resolutions.Length - 1];
        }
    }

    public class ArecontVisionFramerateCondition
    {
        public Resolution Resolution = Resolution.NA;
        public UInt16 StreamId = 1;
    }

    public partial class ParseCameraModel
    {
        public static void ParseArecontVision(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new ArecontVisionCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyArecontVisionProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");
            var powerFrequencies = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "PowerFrequency").Split(','),
                                                  PowerFrequencies.ToIndex);

            if(powerFrequencies.Length>0)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.AddRange(powerFrequencies);
                cameraModel.PowerFrequency.Sort();
            }

            SensorMode[] sensorModes = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "SensorMode").Split(','), SensorModes.ToIndex);
            if(sensorModes.Length > 0)
            {
                cameraModel.SensorMode.Clear();
                cameraModel.SensorMode.AddRange(sensorModes);
            }

            var profileModes = node.GetElementsByTagName("ProfileMode");
            if (profileModes.Count == 0) return;

            XmlElement profileModeNode = (XmlElement)profileModes[0];

            var profiles = profileModeNode.SelectNodes("Profile");
            if (profiles == null || profiles.Count == 0) return;

            String mode = profileModeNode.GetAttribute("value");
            cameraModel.CameraMode.Clear();

            cameraModel.CameraMode.Add(CameraMode.Single);

            if (Convert.ToInt16(mode) >= 2) cameraModel.CameraMode.Add(CameraMode.Dual);

            if (Convert.ToInt16(mode) >= 3)cameraModel.CameraMode.Add(CameraMode.Triple);

            if (Convert.ToInt16(mode) >= 4)cameraModel.CameraMode.Add(CameraMode.Multi);

            foreach (XmlElement profileNode in profiles)
            {
                ParseArecontVisionProfile(profileNode, cameraModel);
            }

        }

        private static void ParseArecontVisionProfile(XmlElement profileNode, ArecontVisionCameraModel cameraModel)
        {
            var ids = Array.ConvertAll(profileNode.GetAttribute("id").Split(','), Convert.ToUInt16);
            foreach (var id in ids)
            {
                var compressions = Array.ConvertAll(Xml.GetFirstElementValueByTagName(profileNode, "Compression").Split(','), Compressions.ToIndex);
                Array.Sort(compressions);
                cameraModel.Compression.Add(id, compressions);

                var bitrates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(profileNode, "Bitrate").Split(','), Bitrates.DisplayStringToIndex);
                Array.Sort(bitrates);
                cameraModel.Bitrate.Add(id, bitrates);

                var resolutionNodes = profileNode.GetElementsByTagName("Resolution");
                if (resolutionNodes.Count == 0) continue;
                var resoolutionValues = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var framerates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate").Split(','), Convert.ToUInt16);
                    Array.Sort(framerates);

                    var resolutions = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','),
                                                       Resolutions.ToIndex);
                    foreach (Resolution resolution in resolutions)
                    {
                        var framerateCondition = new ArecontVisionFramerateCondition
                        {
                            StreamId = id,
                            Resolution = resolution
                        };
                        cameraModel.Framerate.Add(framerateCondition, framerates);
                    }
                    resoolutionValues.AddRange(resolutions);
                }

                var resolutionArray = resoolutionValues.ToArray();
                Array.Sort(resolutionArray);
                cameraModel.Resolution.Add(id, resolutionArray);
            }
        }

        private static void CopyArecontVisionProfile(XmlElement node, ArecontVisionCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            ArecontVisionCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (ArecontVisionCameraModel)mode;
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
            cameraModel.SensorMode = copyFrom.SensorMode;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}
