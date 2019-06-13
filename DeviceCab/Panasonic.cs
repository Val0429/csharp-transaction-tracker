using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class PanasonicCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<AspectRatio> AspectRatio = new List<AspectRatio>();
        public List<SensorMode> SensorMode = new List<SensorMode>();
        public Dictionary<PanasonicBasicCondition, Compression[]> Compression = new Dictionary<PanasonicBasicCondition, Compression[]>();
        public Dictionary<PanasonicResolutionCondition, Resolution[]> Resolution = new Dictionary<PanasonicResolutionCondition, Resolution[]>();
        public Dictionary<PanasonicStreamCondition, UInt16[]> Framerate = new Dictionary<PanasonicStreamCondition, UInt16[]>();
        public Dictionary<PanasonicBasicCondition, Bitrate[]> Bitrate = new Dictionary<PanasonicBasicCondition, Bitrate[]>();
        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency> { DeviceConstant.PowerFrequency.NonSpecific };

        public Compression[] GetCompressionByCondition(UInt16 streamId, AspectRatio aspectRatio, SensorMode sensorMode, ConnectionProtocol connectionProtocol, String model, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Compression.Count == 0) return null;

            var result = new List<Compression>();
            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.AspectRatio == aspectRatio && compressionse.Key.SensorMode == sensorMode)
                {
                    if(connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
                    {
                        if (compressionse.Value.Contains(DeviceConstant.Compression.Mjpeg))
                        {
                            return new[] { DeviceConstant.Compression.Mjpeg };
                        }
                        result.AddRange(compressionse.Value);
                        result.Remove(DeviceConstant.Compression.Mjpeg);
                    }
                    else
                    {
                        result.AddRange(compressionse.Value);
                        result.Remove(DeviceConstant.Compression.Mjpeg);
                    }
                }
            }

            //if use rtsp without compression, use http to find
            if (result.Count == 0 && connectionProtocol != DeviceConstant.ConnectionProtocol.Http)
            {
                var httpResult = GetCompressionByCondition(streamId, aspectRatio, sensorMode,
                                                           DeviceConstant.ConnectionProtocol.Http, model, tvStandard,
                                                           cameraMode, condition1, condition2, condition3);
                if(httpResult != null)
                    result.AddRange(httpResult);
            }

            if (Series == "SW3X6" && streamId > 1)
            {
                if(condition1.Compression == DeviceConstant.Compression.H264 || condition1.Compression ==DeviceConstant.Compression.Mpeg4)
                {
                    if (result.Contains(condition1.Compression))
                        return new Compression[] { condition1.Compression };
                }
            }

            return result.Count == 0 ? null : result.ToArray(); 
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig streamConfig, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.AspectRatio == aspectRatio && resolutionse.Key.SensorMode == sensorMode && resolutionse.Key.Compression == streamConfig.Compression && resolutionse.Key.StreamId == streamId)
                {
                    return resolutionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.Compression == streamConfig.Compression && framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.AspectRatio == aspectRatio && framerates.Key.SensorMode == sensorMode
                    && framerates.Key.StreamId == streamId && framerates.Key.Mode == cameraMode)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, AspectRatio aspectRatio, SensorMode sensorMode, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.StreamId == streamId && bitrates.Key.AspectRatio == aspectRatio && bitrates.Key.SensorMode == sensorMode)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        //For DynaColor type
        //===========================================================
        public List<PanasonicProfileSetting> ProfileSettings = new List<PanasonicProfileSetting>();
        private static List<PanasonicProfileSetting> FindProfileSettingByCondition(IEnumerable<PanasonicProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<PanasonicProfileSetting>();

            foreach (PanasonicProfileSetting node in nodes)
            {
                foreach (PanasonicProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (PanasonicProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (PanasonicProfileFrameRate frameRate in resolution.FrameRates)
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
        public ConnectionProtocol[] GetConnectionProtocolByCondition(UInt16 streamId, ConnectionProtocol[] connectionProtocols, Compression[] compressions, AspectRatio aspectRatio)
        {
            //if (streamId == 1)
            //    return connectionProtocols;

            if (streamId == 1 && (aspectRatio != DeviceConstant.AspectRatio.Ratio43_3000K && aspectRatio != DeviceConstant.AspectRatio.Ratio43_VGA_QuadStreams_Ceiling && aspectRatio != DeviceConstant.AspectRatio.Ratio43_3000K_Fisheye_Ceiling))
            {
                return connectionProtocols;
            }

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
            else
            {
                if(compressions.Length == 1)//only have mjpeg
                {
                    var newConnectionProtocols = new List<ConnectionProtocol>();
                    newConnectionProtocols.Add(DeviceConstant.ConnectionProtocol.Http);
                    return newConnectionProtocols.ToArray();
                }
            }

            return connectionProtocols;
        }
        //---------------------------------------------------------------------------------------------
        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (ProfileSettings.Count == 0) return null;
            PanasonicProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);
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
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<PanasonicProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (PanasonicProfileCompression compression in profileCompressions)
            {
                compressions.Add(compression.Compression);
            }

            if (Model == "DCS-5010L" || Model == "DCS-5020L") return compressions.Count == 0 ? null : compressions.ToArray();

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

            PanasonicProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);
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
        public UInt16[] GetFrameRateByProfileSetting(List<PanasonicProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (PanasonicProfileSetting profileSetting in profileSettings)
            {
                foreach (PanasonicProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (PanasonicProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (PanasonicProfileFrameRate frameRate in resolution.FrameRates)
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

            PanasonicProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);
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
        public Bitrate[] GetBiterateByProfileSetting(PanasonicProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (PanasonicProfileCompression compression in profileSetting.Compressions)
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
            PanasonicProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<PanasonicProfileSetting> { targetProfile1 }, condition1);
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
        public Resolution[] GetResolutionByProfileSetting(List<PanasonicProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (PanasonicProfileSetting profileSetting in profileSettings)
            {
                foreach (PanasonicProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (PanasonicProfileResolution resolution in compression.Resolutions)
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

    public class PanasonicBasicCondition
    {
        public UInt16 StreamId = 1;
        public AspectRatio AspectRatio = AspectRatio.NonSpecific;
        public SensorMode SensorMode = SensorMode.NonSpecific;
    }

    public class PanasonicResolutionCondition : PanasonicBasicCondition
    {
        public Compression Compression;
    }

    public class PanasonicStreamCondition : PanasonicResolutionCondition
    {
        public CameraMode Mode = CameraMode.Single;
        public Resolution Resolution = Resolution.NA;
    }

    public class PanasonicProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<PanasonicProfileCompression> Compressions;
    }

    public class PanasonicProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<PanasonicProfileResolution> Resolutions;
    }

    public class PanasonicProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<PanasonicProfileFrameRate> FrameRates;
    }

    public class PanasonicProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public PanasonicProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParsePanasonic(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new PanasonicCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyPanasonicProfile(node, cameraModel, sameAs, list);
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
                ParsePanasonicByDynaColor(node, list, cameraModel);
                return;
            }

            cameraModel.TvStandard.Add(TvStandard.NonSpecific);
            //have AspectRatio
            var aspectRatios = node.GetElementsByTagName("AspectRatio");
            if (aspectRatios.Count > 0)
            {
                foreach (XmlElement aspectRatio in aspectRatios)
                {
                    var ratio = AspectRatios.ToIndex(aspectRatio.GetAttribute("value"));
                    if (!cameraModel.AspectRatio.Contains(ratio))
                        cameraModel.AspectRatio.Add(ratio);
                    ParsePanasonicProfileMode(cameraModel, aspectRatio, ratio, SensorMode.NonSpecific);
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
                    ParsePanasonicProfileMode(cameraModel, sensorMode, AspectRatio.NonSpecific, mode);
                }
                return;
            }

            //Normal
            ParsePanasonicProfileMode(cameraModel, node, AspectRatio.NonSpecific, SensorMode.NonSpecific);
        }

        private static void ParsePanasonicProfileMode(PanasonicCameraModel cameraModel, XmlElement node, AspectRatio aspectRatio, SensorMode sensorMode)
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
                ParsePanasonicProfile(cameraModel, profileNode, aspectRatio, sensorMode, cameraMode);
            }
        }

        private static void CopyPanasonicProfile(XmlElement node, PanasonicCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            PanasonicCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (PanasonicCameraModel)mode;
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

        private static void ParsePanasonicProfile(PanasonicCameraModel cameraModel, XmlElement node, AspectRatio aspectRatio, SensorMode sensorMode, CameraMode cameraMode)
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
                        cameraModel.Compression.Add(new PanasonicBasicCondition{StreamId = id, AspectRatio = aspectRatio, SensorMode = sensorMode}, compressions);

                        foreach (Compression compression in compressions)
                        {
                            var resolutions = compressionNode.GetElementsByTagName("Resolution");
                            if (resolutions.Count == 0) continue;
                            var condition = new PanasonicResolutionCondition { StreamId = id, AspectRatio = aspectRatio, SensorMode = sensorMode, Compression = compression };
                            foreach (XmlElement resolutionNode in resolutions)
                                ParsePanasonicResolution(resolutionNode, condition, cameraModel, cameraMode);
                        }

                        //-----------------------------------
                        String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
                        if (bitrateStr != "")
                        {
                            Bitrate[] bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                            Array.Sort(bitrates);
                            cameraModel.Bitrate.Add(new PanasonicBasicCondition{StreamId = id, AspectRatio = aspectRatio, SensorMode = sensorMode}, bitrates);
                        }
                      
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

                        //    var condition = new PanasonicResolutionCondition { StreamId = id, AspectRatio = ratio };
                        //    foreach (XmlElement resolutionNode in resolutions)
                        //    {
                        //        ParsePanasonicResolution(resolutionNode, condition, cameraModel);
                        //    }
                        //}
                        //-----------------------------------
                    }
                }
        }

        private static void ParsePanasonicResolution(XmlElement node, PanasonicResolutionCondition condition, PanasonicCameraModel cameraModel, CameraMode cameraMode)
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

            String framerateStr = Xml.GetFirstElementValueByTagName(node, "FrameRate");
            UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);

            Array.Sort(framerates);
            foreach (var resolution in resolutions)
            {
                cameraModel.Framerate.Add(new PanasonicStreamCondition { StreamId = condition.StreamId, AspectRatio = condition.AspectRatio, SensorMode = condition.SensorMode, Compression = condition.Compression, Resolution = resolution, Mode = cameraMode }, framerates);
            }
        }

        //keep for DynaColor in case
        public static void ParsePanasonicByDynaColor(XmlElement node, List<CameraModel> list, PanasonicCameraModel cameraModel)
        {
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);

                ParsePanasonicByDynaColorProfileMode(TvStandard.NonSpecific, node, cameraModel);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParsePanasonicByDynaColorProfileMode(tvStandard, tvStandardNode, cameraModel);
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
            //            cameraModel.ProfileSettings.Add(ParsePanasonicByDynaColorProfileNode(TvStandard.NonSpecific, cameraMode, profile));
            //    }
            //}
        }

        private static void ParsePanasonicByDynaColorProfileMode(TvStandard tvStandard, XmlElement node, PanasonicCameraModel cameraModel)
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
                        cameraModel.ProfileSettings.Add(ParsePanasonicByDynaColorProfileNode(tvStandard, cameraMode, profile));
                }
            }
        }

        private static PanasonicProfileSetting ParsePanasonicByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new PanasonicProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<PanasonicProfileCompression>()
            };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                var newCompression = new PanasonicProfileCompression
                {
                    Compression = codec,
                    Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                    Resolutions = new List<PanasonicProfileResolution>()
                };

                var resolutions = compression.SelectNodes("Resolution");
                if (resolutions != null)
                {
                    foreach (XmlElement resolution in resolutions)
                    {
                        var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        var newResolution = new PanasonicProfileResolution
                        {
                            Resolutions = res.ToList(),
                            FrameRates = new List<PanasonicProfileFrameRate>()
                        };

                        var framerates = resolution.SelectNodes("FrameRate");
                        if (framerates != null)
                        {
                            foreach (XmlElement framerate in framerates)
                            {
                                var newFrameRate = new PanasonicProfileFrameRate
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
                                        newFrameRate.ProfileSetting = ParsePanasonicByDynaColorProfileNode(tvStandard, mode, profile);
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
