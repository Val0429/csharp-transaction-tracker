using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class DLinkCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<AspectRatio> AspectRatio = new List<AspectRatio>();
        public List<SensorMode> SensorMode = new List<SensorMode>();
        public Dictionary<DLinkBasicCondition, Compression[]> Compression = new Dictionary<DLinkBasicCondition, Compression[]>();
        public Dictionary<DLinkResolutionCondition, Resolution[]> Resolution = new Dictionary<DLinkResolutionCondition, Resolution[]>();
        public Dictionary<DLinkStreamCondition, UInt16[]> Framerate = new Dictionary<DLinkStreamCondition, UInt16[]>();
        public Dictionary<DLinkBitrateCondition, Bitrate[]> Bitrate = new Dictionary<DLinkBitrateCondition, Bitrate[]>();
        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency> { DeviceConstant.PowerFrequency.NonSpecific };

        public Compression[] GetCompressionByCondition(UInt16 streamId, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode, ConnectionProtocol connectionProtocol, String model, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Type == "DynaColor")
                return GetCompressionByCondition(tvStandard, cameraMode, connectionProtocol, streamId == 1 ? null : condition1, condition2, condition3);

            if (Compression.Count == 0) return null;

            var result = new List<Compression>();
            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.PowerFrequency == powerFrequency && compressionse.Key.AspectRatio == aspectRatio && compressionse.Key.SensorMode == sensorMode)
                {
                    if ((model == "DCS-6112" || model == "DCS-6113" )&& connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
                    {
                        if (compressionse.Value.Contains(DeviceConstant.Compression.Mjpeg))
                            return new[] { DeviceConstant.Compression.Mjpeg };
                        return null;
                    }

                   result.AddRange(compressionse.Value); 
                }
            }
            return result.Count == 0 ? null : result.ToArray(); 
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig streamConfig, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Type == "DynaColor")
                return GetResolutionByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            if (Resolution.Count == 0) return null;

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.PowerFrequency == powerFrequency && resolutionse.Key.AspectRatio == aspectRatio && resolutionse.Key.SensorMode == sensorMode && resolutionse.Key.Compression == streamConfig.Compression && resolutionse.Key.StreamId == streamId)
                {
                    return resolutionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Type == "DynaColor")
                return GetFramerateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.Compression == streamConfig.Compression && framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.PowerFrequency == powerFrequency && framerates.Key.AspectRatio == aspectRatio && framerates.Key.SensorMode == sensorMode
                    && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Type == "DynaColor")
                return GetBitrateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.Compression == streamConfig.Compression && bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.PowerFrequency == powerFrequency && bitrates.Key.AspectRatio == aspectRatio && bitrates.Key.SensorMode == sensorMode
                    && bitrates.Key.StreamId == streamId)//if (bitrates.Key.StreamId == streamId && bitrates.Key.PowerFrequency == powerFrequency && bitrates.Key.AspectRatio == aspectRatio && bitrates.Key.SensorMode == sensorMode)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        //For DynaColor type
        //===========================================================
        public List<DLinkProfileSetting> ProfileSettings = new List<DLinkProfileSetting>();
        private static List<DLinkProfileSetting> FindProfileSettingByCondition(IEnumerable<DLinkProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<DLinkProfileSetting>();

            foreach (DLinkProfileSetting node in nodes)
            {
                foreach (DLinkProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (DLinkProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (DLinkProfileFrameRate frameRate in resolution.FrameRates)
                                {
                                    if (frameRate.FrameRates.IndexOf(condition.Framerate) > -1)
                                    {
                                        if (frameRate.ProfileSetting != null) profileSettings.Add(frameRate.ProfileSetting);
                                        //return frameRate.ProfileSetting;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return profileSettings.Count > 0 ? profileSettings : null;
        }
        public ConnectionProtocol[] GetConnectionProtocolByCondition(ConnectionProtocol[] connectionProtocols, Compression[] compressions)
        {
            if (Type != "DynaColor" || (Model == "DCS-5010L" || Model == "DCS-5020L" || Model == "DVS-310-1"))
                return connectionProtocols;

            if (!compressions.Contains(DeviceConstant.Compression.Mjpeg))
            {
                var newConnectionProtocols = new List<ConnectionProtocol>();
                foreach (ConnectionProtocol protocol in connectionProtocols)
                {
                    if (protocol == DeviceConstant.ConnectionProtocol.Http) continue;
                    newConnectionProtocols.Add(protocol);
                }
                
                return newConnectionProtocols.ToArray();
            }

            return connectionProtocols;
        }
        //---------------------------------------------------------------------------------------------
        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (ProfileSettings.Count == 0) return null;
            DLinkProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition1 == null) return GetCompressionsByProfileSetting(targetProfile1.Compressions, connectionProtocol);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DLinkProfileSetting> { targetProfile1 }, condition1);
            if (targetProfile2 == null) return null;
            if (condition2 == null) return GetCompressionsByProfileSetting(targetProfile2[0].Compressions, connectionProtocol);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2);
            if (targetProfile3 == null) return null;
            if (condition3 == null) return GetCompressionsByProfileSetting(targetProfile3[0].Compressions, connectionProtocol);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3);
            if (targetProfile4 == null) return null;
            return GetCompressionsByProfileSetting(targetProfile4[0].Compressions, connectionProtocol);
        }
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<DLinkProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (DLinkProfileCompression compression in profileCompressions)
            {
                compressions.Add(compression.Compression);
            }

            if (Model == "DCS-5010L" || Model == "DCS-5020L" || Model == "DVS-310-1") return compressions.Count == 0 ? null : compressions.ToArray();

            if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
            {
                if (compressions.Contains(DeviceConstant.Compression.Mjpeg))
                    return new[] { DeviceConstant.Compression.Mjpeg };
                return null;
            }

            return compressions.Count == 0 ? null : compressions.ToArray();
        }

        //---------------------------------------------------------------------------------------------
        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            DLinkProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<DLinkProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DLinkProfileSetting> { targetProfile1 }, condition1);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetFrameRateByProfileSetting(targetProfile2, condition2);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetFrameRateByProfileSetting(targetProfile3, condition3);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3);
            if (targetProfile4 == null) return null;
            return GetFrameRateByProfileSetting(targetProfile4, condition4);
        }
        public UInt16[] GetFrameRateByProfileSetting(List<DLinkProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (DLinkProfileSetting profileSetting in profileSettings)
            {
                foreach (DLinkProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (DLinkProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (DLinkProfileFrameRate frameRate in resolution.FrameRates)
                                {
                                    list.Add(frameRate.FrameRates.ToArray());
                                    //frameRates.AddRange(frameRate.FrameRates);
                                }
                            }
                        }
                    }
                }
            }

            List<UInt16> frameRates = list.SelectMany(x => x).Distinct().ToList();
            return frameRates.Count == 0 ? null : frameRates.ToArray();
        }

        //---------------------------------------------------------------------------------------------
        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            DLinkProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetBiterateByProfileSetting(targetProfile1, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DLinkProfileSetting> { targetProfile1 }, condition1);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetBiterateByProfileSetting(targetProfile2[0], condition2);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetBiterateByProfileSetting(targetProfile3[0], condition3);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3);
            if (targetProfile4 == null) return null;
            return GetBiterateByProfileSetting(targetProfile4[0], condition4);
        }
        public Bitrate[] GetBiterateByProfileSetting(DLinkProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (DLinkProfileCompression compression in profileSetting.Compressions)
            {
                if (compression.Compression == condiction.Compression)
                {
                    if (compression.Bitrates == null) return null;
                    return compression.Bitrates.Length == 0 ? null : compression.Bitrates;
                }
            }
            return null;
        }

        //---------------------------------------------------------------------------------------------
        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;
            DLinkProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetResolutionByProfileSetting(new List<DLinkProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DLinkProfileSetting> { targetProfile1 }, condition1);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetResolutionByProfileSetting(targetProfile2, condition2);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetResolutionByProfileSetting(targetProfile3, condition3);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3);
            if (targetProfile4 == null) return null;
            return GetResolutionByProfileSetting(targetProfile4, condition4);
        }
        public Resolution[] GetResolutionByProfileSetting(List<DLinkProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (DLinkProfileSetting profileSetting in profileSettings)
            {
                foreach (DLinkProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (DLinkProfileResolution resolution in compression.Resolutions)
                        {
                            list.Add(resolution.Resolutions.ToArray());
                        }
                    }
                }
            }
            var resolutions = list.SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
            return resolutions.Count == 0 ? null : resolutions.ToArray();
        }

        //===========================================================
    }

    public class DLinkBasicCondition
    {
        public UInt16 StreamId = 1;
        public PowerFrequency PowerFrequency = PowerFrequency.NonSpecific;
        public AspectRatio AspectRatio = AspectRatio.NonSpecific;
        public SensorMode SensorMode = SensorMode.NonSpecific;
    }

    public class DLinkResolutionCondition : DLinkBasicCondition
    {
        public Compression Compression;
    }

    public class DLinkStreamCondition : DLinkResolutionCondition
    {
        public Resolution Resolution = Resolution.NA;
    }

    public class DLinkBitrateCondition : DLinkResolutionCondition
    {
        public Resolution Resolution = Resolution.NA;
    }

    public class DLinkProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<DLinkProfileCompression> Compressions;
    }

    public class DLinkProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<DLinkProfileResolution> Resolutions;
    }

    public class DLinkProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<DLinkProfileFrameRate> FrameRates;
    }

    public class DLinkProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public DLinkProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseDLink(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new DLinkCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyDlinkProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.CameraMode.Clear();

            if(!String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(node, "PowerFrequency")))
            {
                var powerFrequencies = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "PowerFrequency").Split(','), PowerFrequencies.ToIndex);

                if (powerFrequencies.Length > 0) cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);

                foreach (var powerFrequency in powerFrequencies)
                {
                    if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                        cameraModel.PowerFrequency.Add(powerFrequency);
                }
            }

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            if (cameraModel.Type == "DynaColor")
            {
                ParseDLinkByDynaColor(node, list, cameraModel);
                return;
            }

            cameraModel.TvStandard.Add(TvStandard.NonSpecific);

            //have PowerFrequency
            var powerFrequencys = node.GetElementsByTagName("PowerFrequency");
            if (powerFrequencys.Count > 0)
            {
                foreach (XmlElement powerFrequency in powerFrequencys)
                {
                    var frequency = PowerFrequencies.ToIndex(powerFrequency.GetAttribute("value"));
                    if (!cameraModel.PowerFrequency.Contains(frequency))
                        cameraModel.PowerFrequency.Add(frequency);
                    ParseDLinkPowerFrequency(cameraModel, powerFrequency, frequency, SensorMode.NonSpecific);
                }
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                return;
            }

            //have AspectRatio
            var aspectRatios = node.GetElementsByTagName("AspectRatio");
            if (aspectRatios.Count > 0)
            {
                foreach (XmlElement aspectRatio in aspectRatios)
                {
                    var ratio = AspectRatios.ToIndex(aspectRatio.GetAttribute("value"));
                    if (!cameraModel.AspectRatio.Contains(ratio))
                        cameraModel.AspectRatio.Add(ratio);
                    ParseDLinkProfileMode(cameraModel, aspectRatio, PowerFrequency.NonSpecific, ratio, SensorMode.NonSpecific);
                }
                return;
            }

            //have SensorMode
            var sensorModes = node.GetElementsByTagName("SensorMode");
            if (sensorModes.Count > 0)
            {
                foreach (XmlElement sensorMode in sensorModes)
                {
                    var mode = SensorModes.ToIndex(sensorMode.GetAttribute("value"));
                    if (!cameraModel.SensorMode.Contains(mode))
                        cameraModel.SensorMode.Add(mode);
                    ParseDLinkProfileMode(cameraModel, sensorMode, PowerFrequency.NonSpecific, AspectRatio.NonSpecific, mode);
                }
                return;
            }

            //Normal
            ParseDLinkProfileMode(cameraModel, node, PowerFrequency.NonSpecific, AspectRatio.NonSpecific, SensorMode.NonSpecific);
        }

        private static void ParseDLinkPowerFrequency(DLinkCameraModel cameraModel, XmlElement node, PowerFrequency powerFrequency, SensorMode sensorMode)
        {
            var aspectRatios = node.GetElementsByTagName("AspectRatio");
            if (aspectRatios.Count > 0)
            {
                foreach (XmlElement aspectRatio in aspectRatios)
                {
                    var ratio = AspectRatios.ToIndex(aspectRatio.GetAttribute("value"));
                    if (!cameraModel.AspectRatio.Contains(ratio))
                        cameraModel.AspectRatio.Add(ratio);
                    ParseDLinkProfileMode(cameraModel, aspectRatio, powerFrequency, ratio, sensorMode);
                }
                return;
            }
        }

        private static void ParseDLinkProfileMode(DLinkCameraModel cameraModel, XmlElement node, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode)
        {
            var profileModes = node.GetElementsByTagName("ProfileMode");
            if (profileModes.Count == 0) return;

            XmlElement profileModeNode = (XmlElement)profileModes[0];

            var profiles = profileModeNode.SelectNodes("Profile");
            if (profiles == null || profiles.Count == 0) return;

            String mode = profileModeNode.GetAttribute("value");
            CameraMode cameraMode = CameraMode.Single;
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
            }

            if (!cameraModel.CameraMode.Contains(cameraMode))
                cameraModel.CameraMode.Add(cameraMode);

            foreach (XmlElement profileNode in profiles)
            {
                ParseDLinkProfile(cameraModel, profileNode, powerFrequency, aspectRatio, sensorMode);
            }
        }

        private static void CopyDlinkProfile(XmlElement node, DLinkCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            DLinkCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (DLinkCameraModel)mode;
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
            cameraModel.TvStandard = copyFrom.TvStandard;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.AspectRatio = copyFrom.AspectRatio;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            cameraModel.ProfileSettings = copyFrom.ProfileSettings;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseDLinkProfile(DLinkCameraModel cameraModel, XmlElement node, PowerFrequency powerFrequency, AspectRatio aspectRatio, SensorMode sensorMode)
        {
            var ids = Array.ConvertAll(node.GetAttribute("id").Split(','), Convert.ToUInt16);

            var compressionNodes = node.SelectNodes("Compression");

            if (compressionNodes != null)
                foreach (XmlElement compressionNode in compressionNodes)
                {
                    foreach (var id in ids)
                    {
                        String compressionStr = compressionNode.GetAttribute("codes");
                        Compression[] compressions = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);

                        Array.Sort(compressions);
                        cameraModel.Compression.Add(new DLinkBasicCondition{StreamId = id, PowerFrequency = powerFrequency, AspectRatio = aspectRatio, SensorMode = sensorMode}, compressions);

                        String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");

                        foreach (Compression compression in compressions)
                        {
                            var resolutions = node.GetElementsByTagName("Resolution");
                            if (resolutions.Count == 0) continue;
                            var condition = new DLinkResolutionCondition { StreamId = id, PowerFrequency = powerFrequency, AspectRatio = aspectRatio, SensorMode = sensorMode, Compression = compression };
                            foreach (XmlElement resolutionNode in resolutions)
                                ParseDLinkResolution(resolutionNode, condition, cameraModel, bitrateStr);
                        }

                        //-----------------------------------
                       
                        
                      
                        //var aspectRatios = node.GetElementsByTagName("AspectRatio");
                        //if (aspectRatios.Count == 0)//no AspectRatio tag, just parse Resolution 
                        //{
                            
                        //}

                        //foreach (XmlElement aspectRatio in aspectRatios)
                        //{
                        //    var resolutions = aspectRatio.GetElementsByTagName("Resolution");
                        //    if (resolutions.Count == 0) continue;

                        //    var ratio = AspectRatios.ToIndex(aspectRatio.GetAttribute("value"));
                        //    if (!cameraModel.AspectRatio.Contains(ratio))
                        //        cameraModel.AspectRatio.Add(ratio);

                        //    var condition = new DLinkResolutionCondition { StreamId = id, AspectRatio = ratio };
                        //    foreach (XmlElement resolutionNode in resolutions)
                        //    {
                        //        ParseDLinkResolution(resolutionNode, condition, cameraModel);
                        //    }
                        //}
                        //-----------------------------------
                    }
                }
        }

        private static void ParseDLinkResolution(XmlElement node, DLinkResolutionCondition condition, DLinkCameraModel cameraModel, String bitrateStr)
        {
            var resolutions = Array.ConvertAll(node.GetAttribute("value").Split(','), Resolutions.ToIndex);

            if (cameraModel.Resolution.ContainsKey(condition))
            {
                List<Resolution> temp = new List<Resolution>(cameraModel.Resolution[condition]);
                foreach (var resolution in resolutions)
                {
                    if (!temp.Contains(resolution))
                        temp.Add(resolution);
                }
                temp.Sort();
                cameraModel.Resolution[condition] = temp.ToArray();
            }
            else
            {
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(condition, resolutions);
            }

            String resolutionBitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
            if (!String.IsNullOrEmpty(resolutionBitrateStr))
                bitrateStr = resolutionBitrateStr;

            String framerateStr = Xml.GetFirstElementValueByTagName(node, "FrameRate");
            UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);

            Array.Sort(framerates);
            foreach (var resolution in resolutions)
            {
                if (bitrateStr != "")
                {
                    Bitrate[] bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                    Array.Sort(bitrates);
                    cameraModel.Bitrate.Add(new DLinkBitrateCondition { StreamId = condition.StreamId, PowerFrequency = condition.PowerFrequency, AspectRatio = condition.AspectRatio, SensorMode = condition.SensorMode, Compression = condition.Compression, Resolution = resolution }, bitrates);
                }

                cameraModel.Framerate.Add(new DLinkStreamCondition { StreamId = condition.StreamId, PowerFrequency = condition.PowerFrequency, AspectRatio = condition.AspectRatio, SensorMode = condition.SensorMode, Compression = condition.Compression, Resolution = resolution }, framerates);
            }
        }

        public static void ParseDLinkByDynaColor(XmlElement node, List<CameraModel> list, DLinkCameraModel cameraModel)
        {
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);

                ParseDLinkByDynaColorProfileMode(TvStandard.NonSpecific, node, cameraModel);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParseDLinkByDynaColorProfileMode(tvStandard, tvStandardNode, cameraModel);
                }
            }

            //var profileModes = node.GetElementsByTagName("ProfileMode");

            //foreach (XmlElement profileMode in profileModes)
            //{
            //    var profileModeValue = profileMode.GetAttribute("value");

            //    CameraMode cameraMode = CameraMode.Single;
            //    switch (profileModeValue)
            //    {
            //        case "1":
            //            cameraMode = CameraMode.Single;
            //            break;

            //        case "2":
            //            cameraMode = CameraMode.Dual;
            //            break;

            //        case "3":
            //            cameraMode = CameraMode.Triple;
            //            break;

            //        case "4":
            //            cameraMode = CameraMode.Multi;
            //            break;
            //    }

            //    if (!cameraModel.CameraMode.Contains(cameraMode))
            //        cameraModel.CameraMode.Add(cameraMode);

            //    //cameraModel.TvStandard.Add(TvStandard.NonSpecific);

            //    var profiles = profileMode.SelectNodes("Profile");
            //    if (profiles != null && profiles.Count > 0)
            //    {
            //        foreach (XmlElement profile in profiles)
            //            cameraModel.ProfileSettings.Add(ParseDLinkByDynaColorProfileNode(TvStandard.NonSpecific, cameraMode, profile));
            //    }
            //}
        }

        private static void ParseDLinkByDynaColorProfileMode(TvStandard tvStandard, XmlElement node, DLinkCameraModel cameraModel)
        {
            var profileModes = node.GetElementsByTagName("ProfileMode");

            foreach (XmlElement profileMode in profileModes)
            {
                var profileModeValue = profileMode.GetAttribute("value");

                CameraMode cameraMode = CameraMode.Single;
                switch (profileModeValue)
                {
                    case "1":
                        cameraMode = CameraMode.Single;
                        break;

                    case "2":
                        cameraMode = CameraMode.Dual;
                        break;

                    case "3":
                        cameraMode = CameraMode.Triple;
                        break;

                    case "4":
                        cameraMode = CameraMode.Multi;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                var profiles = profileMode.SelectNodes("Profile");
                if (profiles != null && profiles.Count > 0)
                {
                    foreach (XmlElement profile in profiles)
                        cameraModel.ProfileSettings.Add(ParseDLinkByDynaColorProfileNode(tvStandard, cameraMode, profile));
                }
            }
        }

        private static DLinkProfileSetting ParseDLinkByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new DLinkProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<DLinkProfileCompression>()
            };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                var newCompression = new DLinkProfileCompression
                {
                    Compression = codec,
                    Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                    Resolutions = new List<DLinkProfileResolution>()
                };

                var resolutions = compression.SelectNodes("Resolution");
                if (resolutions != null)
                {
                    foreach (XmlElement resolution in resolutions)
                    {
                        var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        var newResolution = new DLinkProfileResolution
                        {
                            Resolutions = res.ToList(),
                            FrameRates = new List<DLinkProfileFrameRate>()
                        };

                        var framerates = resolution.SelectNodes("FrameRate");
                        if (framerates != null)
                        {
                            foreach (XmlElement framerate in framerates)
                            {
                                var newFrameRate = new DLinkProfileFrameRate
                                {
                                    FrameRates = Array.ConvertAll(framerate.GetAttribute("value").Split(','), Convert.ToUInt16).ToList()
                                };
                                newResolution.FrameRates.Add(newFrameRate);

                                var profiles = framerate.SelectNodes("Profile");
                                //recursive
                                if (profiles != null && profiles.Count > 0)
                                {
                                    foreach (XmlElement profile in profiles)
                                    {
                                        newFrameRate.ProfileSetting = ParseDLinkByDynaColorProfileNode(tvStandard, mode, profile);
                                    }
                                }
                            }
                        }

                        newCompression.Resolutions.Add(newResolution);
                    }
                }

                profileSetting.Compressions.Add(newCompression);
            }

            return profileSetting;
        }
    }
}
