using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class EtrovisionCameraModel : CameraModel
    {
        public List<SensorMode> SensorMode = new List<SensorMode>
        {
            DeviceConstant.SensorMode.NonSpecific
        };

        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency>
        {
            DeviceConstant.PowerFrequency.NonSpecific
        };

        public Dictionary<EtrovisionStreamCondition, Compression[]> Compression = new Dictionary<EtrovisionStreamCondition, Compression[]>();
        public Dictionary<EtrovisionResolutionCondition, Resolution[]> Resolution = new Dictionary<EtrovisionResolutionCondition, Resolution[]>();
        public Dictionary<EtrovisionStreamCondition, UInt16[]> Framerate = new Dictionary<EtrovisionStreamCondition, UInt16[]>();
        public Dictionary<EtrovisionStreamCondition, Bitrate[]> Bitrate = new Dictionary<EtrovisionStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.CameraMode == cameraMode && compressionse.Key.SensorMode == sensorMode
                    && compressionse.Key.StreamId == streamId)
                {
                    return compressionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.CameraMode == cameraMode && framerates.Key.SensorMode == sensorMode
                    && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.CameraMode == cameraMode && bitrates.Key.SensorMode == sensorMode
                    && bitrates.Key.StreamId == streamId)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode, PowerFrequency powerFrequency)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutions in Resolution)
            {
                if (resolutions.Key.CameraMode == cameraMode && resolutions.Key.SensorMode == sensorMode
                    && resolutions.Key.StreamId == streamId && resolutions.Key.PowerFrequency == powerFrequency)
                {
                    return resolutions.Value;
                }
            }

            return null;
        }
    }

    public class EtrovisionStreamCondition
    {
        public CameraMode CameraMode = CameraMode.Single;
        public SensorMode SensorMode = SensorMode.NonSpecific;
        public UInt16 StreamId = 1;
    }

    public class EtrovisionResolutionCondition : EtrovisionStreamCondition
    {
        public PowerFrequency PowerFrequency = PowerFrequency.NonSpecific;
    }

    public partial class ParseCameraModel
    {
        public static void ParseEtroVision(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new EtrovisionCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyEtroVisionProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var sensorModes = node.GetElementsByTagName("SensorMode");
            if (sensorModes.Count == 0) return;

            cameraModel.CameraMode.Clear();

            foreach (XmlElement sensorModeNode in sensorModes)
            {
                SensorMode sensorMode = SensorModes.ToIndex(sensorModeNode.GetAttribute("value"));

                if (!cameraModel.SensorMode.Contains(sensorMode))
                    cameraModel.SensorMode.Add(sensorMode);

                var profileModes = sensorModeNode.GetElementsByTagName("ProfileMode");
                if (profileModes.Count == 0) continue;

                foreach (XmlElement profileModeNode in profileModes)
                {
                    var profiles = profileModeNode.SelectNodes("Profile");
                    if (profiles == null || profiles.Count == 0) continue;

                    String mode = profileModeNode.GetAttribute("value");
                    CameraMode cameraMode = CameraMode.Single;
                    switch (mode)
                    {
                        case "2":
                            cameraMode = CameraMode.Dual;
                            break;

                        case "4":
                            cameraMode = CameraMode.FourVga;
                            break;
                    }

                    if (!cameraModel.CameraMode.Contains(cameraMode))
                        cameraModel.CameraMode.Add(cameraMode);

                    foreach (XmlElement profileNode in profiles)
                    {
                        ParseEtroVisionProfile(cameraMode, sensorMode, cameraModel, profileNode);
                    }
                }
            }

            if (cameraModel.SensorMode.Count > 1)
            {
                cameraModel.SensorMode.Remove(SensorMode.NonSpecific);
                cameraModel.SensorMode.Sort();
            }

            if (cameraModel.PowerFrequency.Count > 1)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.Sort();
            }
        }

        private static void CopyEtroVisionProfile(XmlElement node, EtrovisionCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            EtrovisionCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (EtrovisionCameraModel)mode;
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

            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.SensorMode = copyFrom.SensorMode;
            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;

            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseEtroVisionProfile(CameraMode cameraMode, SensorMode sensorMode, EtrovisionCameraModel cameraModel, XmlElement node)
        {
            var ids = Array.ConvertAll(node.GetAttribute("id").Split(','), Convert.ToUInt16);

            foreach (var id in ids)
            {
                var condition = new EtrovisionStreamCondition
                {
                    CameraMode = cameraMode,
                    SensorMode = sensorMode,
                    StreamId = id,
                };

                //-----------------------------------

                String framerateStr = Xml.GetFirstElementValueByTagName(node, "FrameRate");
                UInt16[] framerates = (framerateStr != "")
                    ? Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16)
                    : cameraModel.GetFramerateByCondition(1, cameraMode, sensorMode);

                if (framerates != null)
                {
                    Array.Sort(framerates);
                    cameraModel.Framerate.Add(condition, framerates);
                }

                //-----------------------------------

                String compressionStr = Xml.GetFirstElementValueByTagName(node, "Compression");
                Compression[] compressions = (compressionStr != "")
                    ? Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex)
                    : cameraModel.GetCompressionByCondition(1, cameraMode, sensorMode);

                if (compressions != null)
                {
                    Array.Sort(compressions);
                    cameraModel.Compression.Add(condition, compressions);
                }

                //-----------------------------------

                String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
                Bitrate[] bitrates = (bitrateStr != "")
                    ? Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex)
                    : cameraModel.GetBitrateByCondition(1, cameraMode, sensorMode);

                if (bitrates != null)
                {
                    Array.Sort(bitrates);
                    cameraModel.Bitrate.Add(condition, bitrates);
                }

                //-----------------------------------

                var resolutions = node.GetElementsByTagName("Resolution");
                if (resolutions.Count == 0) continue;

                foreach (XmlElement resolutionNode in resolutions)
                {
                    ParseEtrovisionResolution(resolutionNode, condition, cameraModel);
                }
            }
        }

        private static void ParseEtrovisionResolution(XmlElement node, EtrovisionStreamCondition condition, EtrovisionCameraModel cameraModel)
        {
            var powerFrequencies = Array.ConvertAll(node.GetAttribute("PowerFrequency").Split(','), PowerFrequencies.ToIndex);

            foreach (var powerFrequency in powerFrequencies)
            {
                if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                    cameraModel.PowerFrequency.Add(powerFrequency);

                var etrovisionResolutionCondition = new EtrovisionResolutionCondition
                {
                    CameraMode = condition.CameraMode,
                    SensorMode = condition.SensorMode,
                    StreamId = condition.StreamId,
                    PowerFrequency = powerFrequency,
                };

                var resolutions = Array.ConvertAll(node.InnerText.Split(','), Resolutions.ToIndex);
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(etrovisionResolutionCondition, resolutions);
            }
        }
    }
}
