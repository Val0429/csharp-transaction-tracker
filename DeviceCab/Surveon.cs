using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class SurveonCameraModel : CameraModel
    {
        public Dictionary<SurveonResolutionCondition, Resolution[]> Resolution = new Dictionary<SurveonResolutionCondition, Resolution[]>();
        public Dictionary<UInt16, Compression[]> Compression = new Dictionary<UInt16, Compression[]>();
        public Dictionary<SurveonStreamCondition, UInt16[]> Framerate = new Dictionary<SurveonStreamCondition, UInt16[]>();
        public Dictionary<SurveonStreamCondition, Bitrate[]> Bitrate = new Dictionary<SurveonStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Series == "DynaColor")
                return GetCompressionByCondition(TvStandard.NonSpecific, cameraMode, connectionProtocol, streamId == 1 ? null : condition1, condition2, condition3);

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

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetFramerateByCondition(TvStandard.NonSpecific, cameraMode, condition1, condition2, condition3, condition4);

            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.StreamId == streamId && framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.Compression == streamConfig.Compression)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, Compression compression, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetResolutionByCondition(TvStandard.NonSpecific, cameraMode, condition1, condition2, condition3, condition4);

            if (Resolution.Count == 0) return null;

            foreach (var resolution in Resolution)
            {
                if (resolution.Key.StreamId == streamId && resolution.Key.Compression == compression)
                {
                    return resolution.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetBitrateByCondition(TvStandard.NonSpecific, cameraMode, condition1, condition2, condition3, condition4);

            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.StreamId == streamId && bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.Compression == streamConfig.Compression)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        //For DynaColor type
        //===========================================================
        public List<SurveonProfileSetting> ProfileSettings = new List<SurveonProfileSetting>();
        private static List<SurveonProfileSetting> FindProfileSettingByCondition(IEnumerable<SurveonProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<SurveonProfileSetting>();

            foreach (SurveonProfileSetting node in nodes)
            {
                foreach (SurveonProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (SurveonProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (SurveonProfileFrameRate frameRate in resolution.FrameRates)
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
            if (Series != "DynaColor")
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
            SurveonProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<SurveonProfileSetting> { targetProfile1 }, condition1);
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
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<SurveonProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (SurveonProfileCompression compression in profileCompressions)
            {
                compressions.Add(compression.Compression);
            }

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

            SurveonProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<SurveonProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<SurveonProfileSetting> { targetProfile1 }, condition1);
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
        public UInt16[] GetFrameRateByProfileSetting(List<SurveonProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (SurveonProfileSetting profileSetting in profileSettings)
            {
                foreach (SurveonProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (SurveonProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (SurveonProfileFrameRate frameRate in resolution.FrameRates)
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

            SurveonProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<SurveonProfileSetting> { targetProfile1 }, condition1);
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
        public Bitrate[] GetBiterateByProfileSetting(SurveonProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (SurveonProfileCompression compression in profileSetting.Compressions)
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
            SurveonProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<SurveonProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<SurveonProfileSetting> { targetProfile1 }, condition1);
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
        public Resolution[] GetResolutionByProfileSetting(List<SurveonProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (SurveonProfileSetting profileSetting in profileSettings)
            {
                foreach (SurveonProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (SurveonProfileResolution resolution in compression.Resolutions)
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

    public class SurveonResolutionCondition
    {
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class SurveonStreamCondition : SurveonResolutionCondition
    {
        public Resolution Resolution;
    }

    public class SurveonProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<SurveonProfileCompression> Compressions;
    }

    public class SurveonProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<SurveonProfileResolution> Resolutions;
    }

    public class SurveonProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<SurveonProfileFrameRate> FrameRates;
    }

    public class SurveonProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public SurveonProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseSurveon(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new SurveonCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            if (cameraModel.Series == "DynaColor")
            {
                ParseSurveonByDynaColor(node, list, cameraModel);
                return;
            }

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
                    ParseSurveonProfile(cameraModel, profileNode);
                }
            }
        }

        private static void ParseSurveonProfile(SurveonCameraModel cameraModel, XmlElement node)
        {
            var compressionNodes = node.GetElementsByTagName("Compression");
            if (compressionNodes.Count == 0) return;

            var id = Convert.ToUInt16(node.GetAttribute("id"));

            var compressions = new List<Compression>();
            foreach (XmlElement compressionNode in compressionNodes)
            {
                String compressionStr = compressionNode.GetAttribute("codes");
                if (compressionStr == "") continue;
                Compression compression = Compressions.ToIndex(compressionStr);
                compressions.Add(compression);

                var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                if (resolutionNodes.Count == 0) return;

                SurveonResolutionCondition resolutionCondition = new SurveonResolutionCondition { StreamId = id, Compression = compression };
                var resolutions = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolution = Resolutions.ToIndex(resolutionNode.GetAttribute("value"));
                    resolutions.Add(resolution);

                    SurveonStreamCondition condition = new SurveonStreamCondition { StreamId = id, Compression = compression, Resolution = resolution };
                    var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");
                    if (framerateStr == "") return;
                    cameraModel.Framerate.Add(condition, Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16));

                    var bitrateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "Bitrate");
                    if (bitrateStr == "") return;
                    cameraModel.Bitrate.Add(condition, Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex));
                }
                resolutions.Sort();
                cameraModel.Resolution.Add(resolutionCondition, resolutions.ToArray());
            }
            compressions.Sort();
            cameraModel.Compression.Add(id, compressions.ToArray());
        }

        public static void ParseSurveonByDynaColor(XmlElement node, List<CameraModel> list, SurveonCameraModel cameraModel)
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
                        cameraModel.ProfileSettings.Add(ParseSurveonByDynaColorProfileNode(TvStandard.NonSpecific, cameraMode, profile));
                }
            }
        }

        private static SurveonProfileSetting ParseSurveonByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new SurveonProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<SurveonProfileCompression>()
            };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                var newCompression = new SurveonProfileCompression
                {
                    Compression = codec,
                    Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                    Resolutions = new List<SurveonProfileResolution>()
                };

                var resolutions = compression.SelectNodes("Resolution");
                if (resolutions != null)
                {
                    foreach (XmlElement resolution in resolutions)
                    {
                        var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        var newResolution = new SurveonProfileResolution
                        {
                            Resolutions = res.ToList(),
                            FrameRates = new List<SurveonProfileFrameRate>()
                        };

                        var framerates = resolution.SelectNodes("FrameRate");
                        if (framerates != null)
                        {
                            foreach (XmlElement framerate in framerates)
                            {
                                var newFrameRate = new SurveonProfileFrameRate
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
                                        newFrameRate.ProfileSetting = ParseSurveonByDynaColorProfileNode(tvStandard, mode, profile);
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
