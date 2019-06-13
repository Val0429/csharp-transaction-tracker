using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class HIKVISIONCameraModel : CameraModel
    {
        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency>
        {
            DeviceConstant.PowerFrequency.NonSpecific
        };
        public List<HIKVISIONProfileSetting> ProfileSettings = new List<HIKVISIONProfileSetting>();

        private static List<HIKVISIONProfileSetting> FindProfileSettingByCondition(IEnumerable<HIKVISIONProfileSetting> nodes , StreamConfig condition, PowerFrequency powerFrequency)
        {
            var profileSettings = new List<HIKVISIONProfileSetting>();

            foreach (HIKVISIONProfileSetting node in nodes)
            {
                foreach (HIKVISIONCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (HIKVISIONResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1 && resolution.PowerFrequency == powerFrequency)
                            {
                                foreach (HIKVISIONFrameRate frameRate in resolution.FrameRates)
                                {
                                    if (frameRate.FrameRates.IndexOf(condition.Framerate) > -1)
                                    {
                                        if (frameRate.ProfileSetting != null)
                                        {
                                            if (frameRate.PowerFrequency == powerFrequency)
                                                profileSettings.Add(frameRate.ProfileSetting);
                                        }
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

        public Compression[] GetCompressionByCondition(CameraMode mode, ConnectionProtocol connectionProtocol, PowerFrequency powerFrequency)
        {
            return GetCompressionByCondition(mode, connectionProtocol, powerFrequency, null, null, null, null);
        }

        public Compression[] GetCompressionByCondition(CameraMode mode, ConnectionProtocol connectionProtocol, PowerFrequency powerFrequency, StreamConfig condition1)
        {
            return GetCompressionByCondition(mode, connectionProtocol, powerFrequency, condition1, null, null, null);
        }

        public Compression[] GetCompressionByCondition(CameraMode mode, ConnectionProtocol connectionProtocol, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2)
        {
            return GetCompressionByCondition(mode, connectionProtocol, powerFrequency, condition1, condition2, null, null);
        }

        public Compression[] GetCompressionByCondition(CameraMode mode, ConnectionProtocol connectionProtocol, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetCompressionByCondition(mode, connectionProtocol, powerFrequency, condition1, condition2, condition3, null);
        }

        public Compression[] GetCompressionByCondition(CameraMode mode, ConnectionProtocol connectionProtocol, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (ProfileSettings.Count == 0) return null;
            HIKVISIONProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition1 == null) return GetCompressionsByProfileSetting(targetProfile1.Compressions, connectionProtocol);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<HIKVISIONProfileSetting> { targetProfile1 }, condition1, powerFrequency);
            if (targetProfile2 == null) return null;
            if (condition2 == null) return GetCompressionsByProfileSetting(targetProfile2[0].Compressions, connectionProtocol);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2, powerFrequency);
            if (targetProfile3 == null) return null;
            if (condition3 == null) return GetCompressionsByProfileSetting(targetProfile3[0].Compressions, connectionProtocol);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3, powerFrequency);
            if (targetProfile4 == null) return null;
            if (condition4 == null) return GetCompressionsByProfileSetting(targetProfile4[0].Compressions, connectionProtocol);

            //find to profile5
            var targetProfile5 = FindProfileSettingByCondition(targetProfile4, condition4, powerFrequency);
            if (targetProfile5 == null) return null;
            return GetCompressionsByProfileSetting(targetProfile5[0].Compressions, connectionProtocol);
        }

        public Compression[] GetCompressionsByProfileSetting(IEnumerable<HIKVISIONCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (HIKVISIONCompression compression in profileCompressions)
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

        public UInt16[] GetFramerateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1)
        {
            return GetFramerateByCondition(mode, powerFrequency, condition1, null, null, null, null);
        }

        public UInt16[] GetFramerateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2)
        {
            return GetFramerateByCondition(mode, powerFrequency, condition1, condition2, null, null, null);
        }

        public UInt16[] GetFramerateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetFramerateByCondition(mode, powerFrequency, condition1, condition2, condition3, null, null);
        }

        public UInt16[] GetFramerateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            return GetFramerateByCondition(mode, powerFrequency, condition1, condition2, condition3, condition4, null);
        }

        public UInt16[] GetFramerateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4, StreamConfig condition5)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            HIKVISIONProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<HIKVISIONProfileSetting> { targetProfile1 }, condition1, powerFrequency);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<HIKVISIONProfileSetting> { targetProfile1 }, condition1, powerFrequency);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetFrameRateByProfileSetting(targetProfile2, condition2, powerFrequency);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2, powerFrequency);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetFrameRateByProfileSetting(targetProfile3, condition3, powerFrequency);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3, powerFrequency);
            if (targetProfile4 == null) return null;
            if (condition5 == null) return GetFrameRateByProfileSetting(targetProfile4, condition4, powerFrequency);

            //find to profile5
            var targetProfile5 = FindProfileSettingByCondition(targetProfile4, condition4, powerFrequency);
            if (targetProfile5 == null) return null;
            return GetFrameRateByProfileSetting(targetProfile5, condition5, powerFrequency);
        }

        public UInt16[] GetFrameRateByProfileSetting(List<HIKVISIONProfileSetting>profileSettings, StreamConfig condiction, PowerFrequency powerFrequency)
        {
            var list = new List<UInt16[]>();

            foreach (HIKVISIONProfileSetting profileSetting in profileSettings)
            {
                foreach (HIKVISIONCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (HIKVISIONResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (HIKVISIONFrameRate frameRate in resolution.FrameRates)
                                {
                                    if (frameRate.PowerFrequency == powerFrequency)
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

        public Bitrate[] GetBitrateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1)
        {
            return GetBitrateByCondition(mode, powerFrequency, condition1, null, null, null, null);
        }

        public Bitrate[] GetBitrateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2)
        {
            return GetBitrateByCondition(mode, powerFrequency, condition1, condition2, null, null, null);
        }

        public Bitrate[] GetBitrateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetBitrateByCondition(mode, powerFrequency, condition1, condition2, condition3, null, null);
        }

        public Bitrate[] GetBitrateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            return GetBitrateByCondition(mode, powerFrequency, condition1, condition2, condition3, condition4, null);
        }

        public Bitrate[] GetBitrateByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4, StreamConfig condition5)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;

            HIKVISIONProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetBiterateByProfileSetting(targetProfile1, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<HIKVISIONProfileSetting>{targetProfile1}, condition1, powerFrequency);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetBiterateByProfileSetting(targetProfile2[0], condition2);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2, powerFrequency);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetBiterateByProfileSetting(targetProfile3[0], condition3);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3, powerFrequency);
            if (targetProfile4 == null) return null;
            if (condition5 == null) return GetBiterateByProfileSetting(targetProfile4[0], condition4);

            //find to profile5
            var targetProfile5 = FindProfileSettingByCondition(targetProfile4, condition4, powerFrequency);
            if (targetProfile5 == null) return null;
            return GetBiterateByProfileSetting(targetProfile5[0], condition5);
        }

        public Bitrate[] GetBiterateByProfileSetting(HIKVISIONProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (HIKVISIONCompression compression in profileSetting.Compressions)
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

        public Resolution[] GetResolutionByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1)
        {
            return GetResolutionByCondition(mode, powerFrequency, condition1, null, null, null, null);
        }

        public Resolution[] GetResolutionByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2)
        {
            return GetResolutionByCondition(mode, powerFrequency, condition1, condition2, null, null, null);
        }

        public Resolution[] GetResolutionByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            return GetResolutionByCondition(mode, powerFrequency, condition1, condition2, condition3, null, null);
        }

        public Resolution[] GetResolutionByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            return GetResolutionByCondition(mode, powerFrequency, condition1, condition2, condition3, condition4, null);
        }

        public Resolution[] GetResolutionByCondition(CameraMode mode, PowerFrequency powerFrequency, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4, StreamConfig condition5)
        {
            if (ProfileSettings.Count == 0) return null;
            if (condition1 == null) return null;
            HIKVISIONProfileSetting targetProfile1 = null;

            foreach (var profileSetting in ProfileSettings)
            {
                if (profileSetting.Mode == mode && profileSetting.ProfileId == 1)
                {
                    targetProfile1 = profileSetting;
                    break;
                }
            }

            //find to profile1
            if (targetProfile1 == null) return null;
            if (condition2 == null) return GetResolutionByProfileSetting(new List<HIKVISIONProfileSetting>() { targetProfile1 }, condition1, powerFrequency);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<HIKVISIONProfileSetting> { targetProfile1 }, condition1, powerFrequency);
            if (targetProfile2 == null) return null;
            if (condition3 == null) return GetResolutionByProfileSetting(targetProfile2, condition2, powerFrequency);

            //find to profile3
            var targetProfile3 = FindProfileSettingByCondition(targetProfile2, condition2, powerFrequency);
            if (targetProfile3 == null) return null;
            if (condition4 == null) return GetResolutionByProfileSetting(targetProfile3, condition3, powerFrequency);

            //find to profile4
            var targetProfile4 = FindProfileSettingByCondition(targetProfile3, condition3, powerFrequency);
            if (targetProfile4 == null) return null;
            if (condition5 == null) return GetResolutionByProfileSetting(targetProfile4, condition4, powerFrequency);

            //find to profile5
            var targetProfile5 = FindProfileSettingByCondition(targetProfile4, condition4, powerFrequency);
            if (targetProfile5 == null) return null;
            return GetResolutionByProfileSetting(targetProfile5, condition5, powerFrequency);
        }

        public Resolution[] GetResolutionByProfileSetting(List<HIKVISIONProfileSetting> profileSettings, StreamConfig condiction, PowerFrequency powerFrequency)
        {
            var list = new List<Resolution[]>();
            foreach (HIKVISIONProfileSetting profileSetting in profileSettings)
            {
                foreach (HIKVISIONCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (HIKVISIONResolution resolution in compression.Resolutions)
                        {
                            if(resolution.PowerFrequency == powerFrequency)
                                list.Add(resolution.Resolutions.ToArray());
                        }
                    }
                }
            }
            var resolutions = list.SelectMany(x => x).Distinct().OrderBy(x=>x).ToList();
            return resolutions.Count == 0 ? null : resolutions.ToArray();
        }
    }

    public class HIKVISIONProfileSetting
    {
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<HIKVISIONCompression> Compressions;
    }

    public class HIKVISIONCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<HIKVISIONResolution> Resolutions;
    }

    public class HIKVISIONResolution
    {
        public PowerFrequency PowerFrequency;
        public List<Resolution> Resolutions;
        public List<HIKVISIONFrameRate> FrameRates;
    }

    public class HIKVISIONFrameRate
    {
        public PowerFrequency PowerFrequency;
        public List<UInt16> FrameRates;
        public HIKVISIONProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseHIKVISION(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new HIKVISIONCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyHIKVISIONProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var profileModes = node.GetElementsByTagName("ProfileMode");

            foreach (XmlElement profileMode in profileModes)
            {
                var profileModeValue = profileMode.GetAttribute("value");

                cameraModel.CameraMode.Clear();
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

                    case "5":
                        cameraMode = CameraMode.Five;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                var profiles = profileMode.SelectNodes("Profile");
                if (profiles != null && profiles.Count > 0)
                {
                    foreach (XmlElement profile in profiles)
                        cameraModel.ProfileSettings.Add(ParseHIKVISIONProfileNode(cameraModel, cameraMode, profile, PowerFrequency.NonSpecific));
                }
            }

            if (cameraModel.PowerFrequency.Count > 1)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.Sort();
            }
        }

        private static HIKVISIONProfileSetting ParseHIKVISIONProfileNode(HIKVISIONCameraModel cameraModel, CameraMode mode, XmlElement node, PowerFrequency powerFrequency)
        {
            var profileSetting = new HIKVISIONProfileSetting
                                     {
                                         Mode = mode,
                                         ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                                         Compressions = new List<HIKVISIONCompression>()
                                     };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codecs = Array.ConvertAll(compression.GetAttribute("codes").Split(','),
                                              Compressions.ToIndex);

                foreach (Compression codec in codecs)
                {
                    //var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                    var newCompression = new HIKVISIONCompression
                    {
                        Compression = codec,
                        Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                        Resolutions = new List<HIKVISIONResolution>()
                    };

                    var resolutions = compression.SelectNodes("Resolution");
                    if (resolutions != null)
                    {
                        foreach (XmlElement resolution in resolutions)
                        {
                            var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                            var powerResFreq = String.IsNullOrEmpty(resolution.GetAttribute("powerFrequency")) ? powerFrequency : PowerFrequencies.ToIndex(resolution.GetAttribute("powerFrequency"));

                            if (!cameraModel.PowerFrequency.Contains(powerResFreq))
                                cameraModel.PowerFrequency.Add(powerResFreq);

                            var newResolution = new HIKVISIONResolution
                            {
                                PowerFrequency = powerResFreq,
                                Resolutions = res.ToList(),
                                FrameRates = new List<HIKVISIONFrameRate>()
                            };

                            var framerates = resolution.SelectNodes("FrameRate");
                            if (framerates != null)
                            {
                                foreach (XmlElement framerate in framerates)
                                {
                                    var powerFreq = powerResFreq ==PowerFrequency.NonSpecific ?
                                        String.IsNullOrEmpty(framerate.GetAttribute("powerFrequency")) ? powerFrequency : PowerFrequencies.ToIndex(framerate.GetAttribute("powerFrequency")):
                                        powerResFreq;
                                    

                                    if(!cameraModel.PowerFrequency.Contains(powerFreq))
                                        cameraModel.PowerFrequency.Add(powerFreq);

                                    var newFrameRate = new HIKVISIONFrameRate
                                    {
                                        FrameRates = Array.ConvertAll(framerate.GetAttribute("value").Split(','), Convert.ToUInt16).ToList(),
                                        PowerFrequency = powerFreq
                                    };
                                    newResolution.FrameRates.Add(newFrameRate);

                                    var profiles = framerate.SelectNodes("Profile");
                                    //recursive
                                    if (profiles != null && profiles.Count > 0)
                                    {
                                        foreach (XmlElement profile in profiles)
                                        {
                                            newFrameRate.ProfileSetting = ParseHIKVISIONProfileNode(cameraModel, mode, profile, powerFreq);
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

        private static void CopyHIKVISIONProfile(XmlElement node, HIKVISIONCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            HIKVISIONCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (HIKVISIONCameraModel)mode;
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

            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.ProfileSettings = copyFrom.ProfileSettings;

            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}
