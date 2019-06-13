using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class DivioTecCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<SensorMode> SensorMode = new List<SensorMode>();

        public List<DivioTecProfileSetting> ProfileSettings = new List<DivioTecProfileSetting>();

        private static List<DivioTecProfileSetting> FindProfileSettingByCondition(IEnumerable<DivioTecProfileSetting> nodes , StreamConfig condition)
        {
            var profileSettings = new List<DivioTecProfileSetting>();

            foreach (DivioTecProfileSetting node in nodes)
            {
                foreach (DivioTecCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (DivioTecResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (DivioTecFrameRate frameRate in resolution.FrameRates)
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

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, ConnectionProtocol connectionProtocol)
        {
            return GetCompressionByCondition(tvStandard, sensorMode, mode, connectionProtocol, null, null, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1)
        {
            return GetCompressionByCondition(tvStandard, sensorMode, mode, connectionProtocol, condition1, null, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2)
        {
            return GetCompressionByCondition(tvStandard, sensorMode, mode, connectionProtocol, condition1, condition2, null);
        }

        public Compression[] GetCompressionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, ConnectionProtocol connectionProtocol, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (ProfileSettings.Count == 0) return null;
            DivioTecProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.SensorMode == sensorMode && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition1 == null) return GetCompressionsByProfileSetting(targetProfile1.Compressions, connectionProtocol);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DivioTecProfileSetting>{targetProfile1}, condition1);
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

        public Compression[] GetCompressionsByProfileSetting(IEnumerable<DivioTecCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (DivioTecCompression compression in profileCompressions)
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

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1)
        {
            return GetFramerateByCondition(tvStandard, sensorMode, mode, condition1, null, null, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetFramerateByCondition(tvStandard, sensorMode, mode, condition1, condition2, null, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetFramerateByCondition(tvStandard, sensorMode, mode, condition1, condition2, condition3, null);
        }

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            DivioTecProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.SensorMode == sensorMode && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<DivioTecProfileSetting>{targetProfile1}, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DivioTecProfileSetting>{targetProfile1}, condition1);
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

        public UInt16[] GetFrameRateByProfileSetting(List<DivioTecProfileSetting>profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (DivioTecProfileSetting profileSetting in profileSettings)
            {
                foreach (DivioTecCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (DivioTecResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (DivioTecFrameRate frameRate in resolution.FrameRates)
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

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1)
        {
            return GetBitrateByCondition(tvStandard, sensorMode, mode, condition1, null, null, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetBitrateByCondition(tvStandard, sensorMode, mode, condition1, condition2, null, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetBitrateByCondition(tvStandard, sensorMode, mode, condition1, condition2, condition3, null);
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            DivioTecProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.SensorMode == sensorMode && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetBiterateByProfileSetting(targetProfile1, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DivioTecProfileSetting>{targetProfile1}, condition1);
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

        public Bitrate[] GetBiterateByProfileSetting(DivioTecProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (DivioTecCompression compression in profileSetting.Compressions)
            {
                if (compression.Compression == condiction.Compression)
                {
                    if (compression.Bitrates == null)
                    {
                        var list = new List<Bitrate>();
                        foreach (DivioTecResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (DivioTecBitrate bitrate in resolution.Bitrates)
                                {
                                    if (bitrate.Bitrates == null) continue;
                                    list.AddRange(bitrate.Bitrates);
                                }
                            }
                        }
                        return list.Count == 0 ? null : list.ToArray();
                    }
                    return compression.Bitrates.Length == 0 ? null : compression.Bitrates;
                }
            }
            return null;
        }

        //---------------------------------------------------------------------------------------------

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1)
        {
            return GetResolutionByCondition(tvStandard, sensorMode, mode, condition1, null, null, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2)
        {
            return GetResolutionByCondition(tvStandard, sensorMode, mode, condition1, condition2, null, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetResolutionByCondition(tvStandard, sensorMode, mode, condition1, condition2, condition3, null);
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard, SensorMode sensorMode, CameraMode mode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;
            DivioTecProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.TvStandard == tvStandard && profileSetting.SensorMode == sensorMode && profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetResolutionByProfileSetting(new List<DivioTecProfileSetting>{ targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<DivioTecProfileSetting>{targetProfile1}, condition1);
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

        public Resolution[] GetResolutionByProfileSetting(List<DivioTecProfileSetting>profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (DivioTecProfileSetting profileSetting in profileSettings)
            {
                foreach (DivioTecCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (DivioTecResolution resolution in compression.Resolutions)
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

    public class DivioTecProfileSetting
    {
        public TvStandard TvStandard;
        public SensorMode SensorMode;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<DivioTecCompression> Compressions;
    }

    public class DivioTecCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<DivioTecResolution> Resolutions;
    }

    public class DivioTecResolution
    {
        public List<Resolution> Resolutions;
        public List<DivioTecBitrate> Bitrates; 
        public List<DivioTecFrameRate> FrameRates;
    }

    public class DivioTecFrameRate
    {
        public List<UInt16> FrameRates;
        public DivioTecProfileSetting ProfileSetting;
    }

    public class DivioTecBitrate
    {
        public Bitrate[] Bitrates;
        public DivioTecProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseDivioTec(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new DivioTecCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyDivioTecProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0) return;

            cameraModel.CameraMode.Clear();

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

                    var sensorModes = profileMode.GetElementsByTagName("SensorMode");

                    if (sensorModes.Count == 0)
                    {
                        var profiles = profileMode.SelectNodes("Profile");
                        if (profiles != null && profiles.Count > 0)
                        {
                            foreach (XmlElement profile in profiles)
                            {
                                cameraModel.ProfileSettings.Add(ParseDivioTecProfileNode(tvStandard, cameraMode, profile, SensorMode.NonSpecific));
                            }
                        }
                    }
                    else
                    {
                        foreach (XmlElement sensorMode in sensorModes)
                        {
                            SensorMode sensor = SensorModes.ToIndex(sensorMode.GetAttribute("value"));

                            if (!cameraModel.SensorMode.Contains(sensor))
                                cameraModel.SensorMode.Add(sensor);

                            var profiles = sensorMode.SelectNodes("Profile");
                            if (profiles != null && profiles.Count > 0)
                            {
                                foreach (XmlElement profile in profiles)
                                {
                                    cameraModel.ProfileSettings.Add(ParseDivioTecProfileNode(tvStandard, cameraMode, profile, sensor));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static DivioTecProfileSetting ParseDivioTecProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node, SensorMode sensorMode)
        {
            
            var profileSetting = new DivioTecProfileSetting
                                     {
                                         TvStandard = tvStandard,
                                         SensorMode = sensorMode,
                                         Mode = mode,
                                         ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                                         Compressions = new List<DivioTecCompression>()
                                     };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codecs = compression.GetAttribute("codes").Split(',');
                foreach (String codecValue in codecs)
                {
                    var codec = Compressions.ToIndex(codecValue);
                    var bitrateNode = compression.SelectNodes("Bitrate");
                    var bitrateString = bitrateNode == null ? String.Empty : bitrateNode.Count == 0 ? String.Empty : bitrateNode[0].InnerText;
                    var newCompression = new DivioTecCompression
                    {
                        Compression = codec,
                        Bitrates = (codec == Compression.Mjpeg || String.IsNullOrEmpty(bitrateString)) ? null : Array.ConvertAll(bitrateString.Split(','), Bitrates.DisplayStringToIndex),
                        Resolutions = new List<DivioTecResolution>()
                    };

                    var resolutions = compression.SelectNodes("Resolution");
                    if (resolutions != null)
                    {
                        foreach (XmlElement resolution in resolutions)
                        {
                            var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                            var newResolution = new DivioTecResolution
                            {
                                Resolutions = res.ToList(),
                                Bitrates = new List<DivioTecBitrate>(),
                                FrameRates = new List<DivioTecFrameRate>()
                            };

                            var bitrates = resolution.SelectNodes("Bitrate");
                            if (bitrates != null)
                            {
                                foreach (XmlElement bitrate in bitrates)
                                {
                                    var newBitRate = new DivioTecBitrate()
                                    {
                                        Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(bitrate.InnerText.Split(','), Bitrates.DisplayStringToIndex)
                                    };
                                    newResolution.Bitrates.Add(newBitRate);
                                }
                            }

                            var framerates = resolution.SelectNodes("FrameRate");
                            if (framerates != null)
                            {
                                foreach (XmlElement framerate in framerates)
                                {
                                    var newFrameRate = new DivioTecFrameRate
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
                                            newFrameRate.ProfileSetting = ParseDivioTecProfileNode(tvStandard, mode, profile,sensorMode);
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

        private static void CopyDivioTecProfile(XmlElement node, DivioTecCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            DivioTecCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (DivioTecCameraModel)mode;
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
