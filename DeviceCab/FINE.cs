using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class FINECameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<FINECompression> Compression = new List<FINECompression>();
        public Dictionary<FINEResolutionCondition, Resolution[]> Resolution = new Dictionary<FINEResolutionCondition, Resolution[]>();
        public Dictionary<FINEStreamCondition, UInt16[]> Framerate = new Dictionary<FINEStreamCondition, UInt16[]>();
        public Dictionary<FINEStreamCondition, Bitrate[]> Bitrate = new Dictionary<FINEStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Series == "DynaColor")
                return GetCompressionByCondition(tvStandard, cameraMode, connectionProtocol, streamId == 1 ? null : condition1, condition2, condition3);


            if (Compression.Count == 0) return null;

            var result = new List<Compression>();

            foreach (FINECompression compressionse in Compression)
            {
                if (compressionse.StreamId == streamId)
                {
                    if(!result.Contains(compressionse.Compression))
                        result.Add(compressionse.Compression);
                }
            }

            return result.Count  == 0 ? null : result.ToArray();
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, TvStandard tvStandard, CameraModel model, StreamConfig streamConfig, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetFramerateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

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

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, TvStandard tvStandard, StreamConfig streamConfig, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetBitrateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

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

        public Resolution[] GetResolutionByCondition(UInt16 streamId, Compression compression, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetResolutionByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

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

        //For DynaColor type
        //===========================================================
        public List<FINEProfileSetting> ProfileSettings = new List<FINEProfileSetting>();
        private static List<FINEProfileSetting> FindProfileSettingByCondition(IEnumerable<FINEProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<FINEProfileSetting>();

            foreach (FINEProfileSetting node in nodes)
            {
                foreach (FINEProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (FINEProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (FINEProfileFrameRate frameRate in resolution.FrameRates)
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
            FINEProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<FINEProfileSetting> { targetProfile1 }, condition1);
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
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<FINEProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (FINEProfileCompression compression in profileCompressions)
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

            FINEProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<FINEProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<FINEProfileSetting> { targetProfile1 }, condition1);
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
        public UInt16[] GetFrameRateByProfileSetting(List<FINEProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (FINEProfileSetting profileSetting in profileSettings)
            {
                foreach (FINEProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (FINEProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (FINEProfileFrameRate frameRate in resolution.FrameRates)
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

            FINEProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetBiterateByProfileSetting(new List<FINEProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<FINEProfileSetting> { targetProfile1 }, condition1);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetBiterateByProfileSetting(targetProfile2, condition2);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetBiterateByProfileSetting(targetProfile3, condition3);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3);
            if (targetProfile4 == null) return null;
            return GetBiterateByProfileSetting(targetProfile4, condition4);
        }
        public Bitrate[] GetBiterateByProfileSetting(List<FINEProfileSetting> profileSettings, StreamConfig condiction)
        {
            foreach (FINEProfileSetting profileSetting in profileSettings)
            {
                foreach (FINEProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (FINEProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (FINEProfileFrameRate frameRate in resolution.FrameRates)
                                {
                                    if (frameRate.Bitrates == null) return null;
                                    return frameRate.Bitrates.Length == 0 ? null : frameRate.Bitrates;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        //---------------------------------------------------------------------------------------------
        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;
            FINEProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<FINEProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<FINEProfileSetting> { targetProfile1 }, condition1);
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
        public Resolution[] GetResolutionByProfileSetting(List<FINEProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (FINEProfileSetting profileSetting in profileSettings)
            {
                foreach (FINEProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (FINEProfileResolution resolution in compression.Resolutions)
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

    public class FINECompression
    {
        public TvStandard TvStandard;
        public UInt16 StreamId = 1;
        public Compression Compression;
    }

    public class FINEResolutionCondition
    {
        public TvStandard TvStandard;
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class FINEStreamCondition : FINEResolutionCondition
    {
        public Resolution Resolution;
    }

    public class FINEProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<FINEProfileCompression> Compressions;
    }

    public class FINEProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<FINEProfileResolution> Resolutions;
    }

    public class FINEProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<FINEProfileFrameRate> FrameRates;
    }

    public class FINEProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public FINEProfileSetting ProfileSetting;
        public Bitrate[] Bitrates;
    }

    public partial class ParseCameraModel
    {
        public static void ParseFINE(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new FINECameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            if (cameraModel.Series == "DynaColor")
            {
                ParseFINEByDynaColor(node, list, cameraModel);
                return;
            }

            cameraModel.CameraMode.Clear();

            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0) return;

            foreach (XmlElement tvStandardNode in tvStandards)
            {
                var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                if (!cameraModel.TvStandard.Contains(tvStandard))
                    cameraModel.TvStandard.Add(tvStandard);

                var profileModes = tvStandardNode.GetElementsByTagName("ProfileMode");
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
                        ParseFINEProfile(cameraModel, tvStandard, profileNode);
                    }
                }
            }
            
        }

        private static void ParseFINEProfile(FINECameraModel cameraModel, TvStandard tvStandard, XmlElement node)
        {
            var compressionNodes = node.GetElementsByTagName("Compression");
            if (compressionNodes.Count == 0) return;

            UInt16 id = Convert.ToUInt16(node.GetAttribute("id"));

            var compressions = new List<Compression>();
            foreach (XmlElement compressionNode in compressionNodes)
            {
                if (compressionNode == null) continue;

                var compressionArray = Array.ConvertAll(compressionNode.GetAttribute("value").Split(','), Compressions.ToIndex);

                compressions.AddRange(compressionArray);

                var resolutionConditions = new List<FINEResolutionCondition>();

                foreach (var compression in compressionArray)
                {
                    resolutionConditions.Add(new FINEResolutionCondition { StreamId = id, TvStandard = tvStandard, Compression = compression});
                }
                //-----------------------------------
                var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                var resolutionList = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                    Array.Sort(resolutionArray);

                    resolutionList.AddRange(resolutionArray);

                    var streamConditions = new List<FINEStreamCondition>();
                    foreach (FINEResolutionCondition condition in resolutionConditions)
                    {
                        foreach (Resolution resolution in resolutionList)
                        {
                            streamConditions.Add(new FINEStreamCondition { StreamId = id, Compression = condition.Compression, Resolution = resolution, TvStandard = tvStandard });
                        }
                    }

                    //-----------------------------------
                    var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                    if (framerateStr != "")
                    {
                        var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                        Array.Sort(framerates);

                        foreach (FINEStreamCondition condition in streamConditions)
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
                cameraModel.Compression.Add(new FINECompression{Compression = compression, StreamId = id, TvStandard = tvStandard});
            }

        }

        public static void ParseFINEByDynaColor(XmlElement node, List<CameraModel> list, FINECameraModel cameraModel)
        {
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);

                ParseFINEByDynaColorProfileMode(TvStandard.NonSpecific, node, cameraModel);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandardsValue = tvStandardNode.GetAttribute("value").Split(',');
                    foreach (String value in tvStandardsValue)
                    {
                        var tvStandard = TvStandards.ToIndex(value);

                        if (!cameraModel.TvStandard.Contains(tvStandard))
                            cameraModel.TvStandard.Add(tvStandard);

                        ParseFINEByDynaColorProfileMode(tvStandard, tvStandardNode, cameraModel);
                    }
                    
                }
            }
        }

        private static void ParseFINEByDynaColorProfileMode(TvStandard tvStandard, XmlElement node, FINECameraModel cameraModel)
        {
            var profileModes = node.GetElementsByTagName("ProfileMode");
            cameraModel.CameraMode.Clear();

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
                        cameraModel.ProfileSettings.Add(ParseFINEByDynaColorProfileNode(tvStandard, cameraMode, profile));
                }
            }
        }


        private static FINEProfileSetting ParseFINEByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new FINEProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<FINEProfileCompression>()
            };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codecs = Array.ConvertAll(compression.GetAttribute("value").Split(','), Compressions.ToIndex);
                //var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                foreach (Compression codec in codecs)
                {
                    var newCompression = new FINEProfileCompression
                    {
                        Compression = codec,
                        Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                        Resolutions = new List<FINEProfileResolution>()
                    };

                    var resolutions = compression.SelectNodes("Resolution");
                    if (resolutions != null)
                    {
                        foreach (XmlElement resolution in resolutions)
                        {
                            var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                            var newResolution = new FINEProfileResolution
                            {
                                Resolutions = res.ToList(),
                                FrameRates = new List<FINEProfileFrameRate>()
                            };

                            var framerates = resolution.SelectNodes("FrameRate");
                            if (framerates != null)
                            {
                                foreach (XmlElement framerate in framerates)
                                {
                                    var newFrameRate = new FINEProfileFrameRate
                                    {
                                        FrameRates = Array.ConvertAll(framerate.GetAttribute("value").Split(','), Convert.ToUInt16).ToList(),
                                        Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(framerate, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                                    };
                                    newResolution.FrameRates.Add(newFrameRate);

                                    var profiles = framerate.SelectNodes("Profile");
                                    //recursive
                                    if (profiles != null && profiles.Count > 0)
                                    {
                                        foreach (XmlElement profile in profiles)
                                        {
                                            newFrameRate.ProfileSetting = ParseFINEByDynaColorProfileNode(tvStandard, mode, profile);
                                        }
                                    }
                                }
                            }

                            newCompression.Resolutions.Add(newResolution);
                        }
                    }

                    profileSetting.Compressions.Add(newCompression);
                }

            }

            return profileSetting;
        }
    }
}
