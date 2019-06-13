﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class SIEMENSCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();

        public List<SIEMENSProfileSetting> ProfileSettings = new List<SIEMENSProfileSetting>();

        private static List<SIEMENSProfileSetting> FindProfileSettingByCondition(IEnumerable<SIEMENSProfileSetting> nodes , StreamConfig condition)
        {
            var profileSettings = new List<SIEMENSProfileSetting>();

            foreach (SIEMENSProfileSetting node in nodes)
            {
                foreach (SIEMENSCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (SIEMENSResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (SIEMENSFrameRate frameRate in resolution.FrameRates)
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

        public ConnectionProtocol[] GetConnectionProtocolByCondition( ConnectionProtocol[] connectionProtocols, Compression[] compressions )
        {
            if (!compressions.Contains(Compression.Mjpeg))
            {
                var newConnectionProtocols = new List<ConnectionProtocol>();
                foreach (ConnectionProtocol protocol in connectionProtocols)
                {
                    if(protocol == DeviceConstant.ConnectionProtocol.Http) continue;
                    newConnectionProtocols.Add(protocol);
                }

                return newConnectionProtocols.ToArray();
            }

            return connectionProtocols;
        }

        //---------------------------------------------------------------------------------------------

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol)
        {
            return GetCompressionByCondition(tvStandard, mode, connectionProtocol, null, null, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1)
        {
            return GetCompressionByCondition(tvStandard, mode, connectionProtocol, condition1, null, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2)
        {
            return GetCompressionByCondition(tvStandard, mode, connectionProtocol, condition1, condition2, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (ProfileSettings.Count == 0) return null;
            SIEMENSProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<SIEMENSProfileSetting>{targetProfile1}, condition1);
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

        public Compression[] GetCompressionsByProfileSetting(IEnumerable<SIEMENSCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (SIEMENSCompression compression in profileCompressions)
            {
                compressions.Add(compression.Compression);
            }

            if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
            {
                if (compressions.Contains(Compression.Mjpeg))
                    return new[] { Compression.Mjpeg };
                return null;
            }

            return compressions.Count == 0 ? null : compressions.ToArray();
        }
        
        //---------------------------------------------------------------------------------------------

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard,CameraMode mode,  StreamConfig condition1)
        {
            return GetFramerateByCondition(tvStandard, mode, condition1, null, null, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetFramerateByCondition(tvStandard, mode, condition1, condition2, null, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetFramerateByCondition(tvStandard, mode, condition1, condition2, condition3, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            SIEMENSProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<SIEMENSProfileSetting>{targetProfile1}, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<SIEMENSProfileSetting>{targetProfile1}, condition1);
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

        public UInt16[] GetFrameRateByProfileSetting(List<SIEMENSProfileSetting>profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (SIEMENSProfileSetting profileSetting in profileSettings)
            {
                foreach (SIEMENSCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (SIEMENSResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (SIEMENSFrameRate frameRate in resolution.FrameRates)
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

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1)
        {
            return GetBitrateByCondition(tvStandard, mode, condition1, null, null, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetBitrateByCondition(tvStandard, mode, condition1, condition2, null, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetBitrateByCondition(tvStandard, mode, condition1, condition2, condition3, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            SIEMENSProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<SIEMENSProfileSetting>{targetProfile1}, condition1);
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

        public Bitrate[] GetBiterateByProfileSetting(SIEMENSProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (SIEMENSCompression compression in profileSetting.Compressions)
            {
                if (compression.Compression == condiction.Compression)
                {
                    foreach (SIEMENSResolution resolution in compression.Resolutions)
                    {
                        if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                        {
                            if (resolution.Bitrates == null) return null;
                            return resolution.Bitrates.Length == 0 ? null : resolution.Bitrates;
                        }
                    }
                    
                }
            }
            return null;
        }

        //---------------------------------------------------------------------------------------------

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1)
        {
            return GetResolutionByCondition(tvStandard, mode, condition1, null, null, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetResolutionByCondition(tvStandard, mode, condition1, condition2, null, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetResolutionByCondition(tvStandard, mode, condition1, condition2, condition3, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;
            SIEMENSProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<SIEMENSProfileSetting>{ targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<SIEMENSProfileSetting>{targetProfile1}, condition1);
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

        public Resolution[] GetResolutionByProfileSetting(List<SIEMENSProfileSetting>profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (SIEMENSProfileSetting profileSetting in profileSettings)
            {
                foreach (SIEMENSCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (SIEMENSResolution resolution in compression.Resolutions)
                        {
                            list.Add(resolution.Resolutions.ToArray());
                        }
                    }
                }
            }
            var resolutions = list.SelectMany(x => x).Distinct().OrderBy(x=>x).ToList();
            return resolutions.Count == 0 ? null : resolutions.ToArray();
        }
    }

    public class SIEMENSProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<SIEMENSCompression> Compressions;
    }

    public class SIEMENSCompression
    {
        public Compression Compression;
        public List<SIEMENSResolution> Resolutions;
    }

    public class SIEMENSResolution
    {
        public List<Resolution> Resolutions;
        public Bitrate[] Bitrates;
        public List<SIEMENSFrameRate> FrameRates;
    }

    public class SIEMENSFrameRate
    {
        public List<UInt16> FrameRates;
        public SIEMENSProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseSIEMENS(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new SIEMENSCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopySIEMENSProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0) return;

            foreach (XmlElement tvStandardNode in tvStandards)
            {
                var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                if (!cameraModel.TvStandard.Contains(tvStandard))
                    cameraModel.TvStandard.Add(tvStandard);

                var profileModes = tvStandardNode.GetElementsByTagName("ProfileMode");

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
                            cameraModel.ProfileSettings.Add(ParseSIEMENSProfileNode(tvStandard, cameraMode, profile));
                    }
                }
            }
        }

        private static SIEMENSProfileSetting ParseSIEMENSProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new SIEMENSProfileSetting
                                     {
                                         TvStandard = tvStandard,
                                         Mode = mode,
                                         ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                                         Compressions = new List<SIEMENSCompression>()
                                     };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codecs = Array.ConvertAll(compression.GetAttribute("codes").Split(','), Compressions.ToIndex);
                //var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                foreach (Compression codec in codecs)
                {
                    var newCompression = new SIEMENSCompression
                    {
                        Compression = codec,
                        Resolutions = new List<SIEMENSResolution>()
                    };

                    var resolutions = compression.SelectNodes("Resolution");
                    if (resolutions != null)
                    {
                        foreach (XmlElement resolution in resolutions)
                        {
                            var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                            var newResolution = new SIEMENSResolution
                            {
                                Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(resolution, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                                Resolutions = res.ToList(),
                                FrameRates = new List<SIEMENSFrameRate>()
                            };

                            var framerates = resolution.SelectNodes("FrameRate");
                            if (framerates != null)
                            {
                                foreach (XmlElement framerate in framerates)
                                {
                                    var newFrameRate = new SIEMENSFrameRate
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
                                            newFrameRate.ProfileSetting = ParseSIEMENSProfileNode(tvStandard, mode, profile);
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

        private static void CopySIEMENSProfile(XmlElement node, SIEMENSCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            SIEMENSCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (SIEMENSCameraModel)mode;
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

            cameraModel.Series = copyFrom.Series;
            cameraModel.Manufacture = copyFrom.Manufacture;
            var type = Xml.GetFirstElementValueByTagName(node, "Type");
            cameraModel.Type = (String.Equals(type, "") ? copyFrom.Type : type);
            cameraModel.Model = model;

            var alias = Xml.GetFirstElementValueByTagName(node, "Alias");
            cameraModel.Alias = (String.Equals(alias, "") ? model : alias);

            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.TvStandard = copyFrom.TvStandard;
            cameraModel.ProfileSettings = copyFrom.ProfileSettings;

            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}