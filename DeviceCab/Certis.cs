using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class CertisCameraModel : CameraModel
    {
        public List<SensorMode> SensorMode = new List<SensorMode>
        {
            DeviceConstant.SensorMode.NonSpecific
        };
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<CertisAMTKCompression> Compression = new List<CertisAMTKCompression>();
        public Dictionary<CertisAMTKResolutionCondition, Resolution[]> Resolution = new Dictionary<CertisAMTKResolutionCondition, Resolution[]>();
        public Dictionary<CertisAMTKStreamCondition, UInt16[]> Framerate = new Dictionary<CertisAMTKStreamCondition, UInt16[]>();
        public Dictionary<CertisAMTKStreamCondition, Bitrate[]> Bitrate = new Dictionary<CertisAMTKStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, TvStandard tvStandard, SensorMode sensorMode, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Series == "DynaColor")
                return GetCompressionByCondition(tvStandard, cameraMode, connectionProtocol, streamId == 1 ? null : condition1, condition2, condition3);

            //A-MTK Series
            if (Compression.Count == 0) return null;

            var result = new List<Compression>();

            foreach (CertisAMTKCompression compressionse in Compression)
            {
                if (compressionse.StreamId == streamId && compressionse.SensorMode == sensorMode)
                {
                    if (!result.Contains(compressionse.Compression))
                        result.Add(compressionse.Compression);
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, Compression compression, Resolution resolution, TvStandard tvStandard, SensorMode sensorMode, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetFramerateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            //A-MTK Series
            if (Framerate.Count == 0) return null;

            var condition = condition1;
            if (streamId == 2) condition = condition2;
            if (streamId == 3) condition = condition3;
            if (streamId == 4) condition = condition4;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.SensorMode == sensorMode && framerates.Key.TvStandard == tvStandard && framerates.Key.Compression == condition.Compression &&
                    framerates.Key.Resolution == condition.Resolution && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, TvStandard tvStandard, SensorMode sensorMode, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetBitrateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            //A-MTK Series
            if (Bitrate.Count == 0) return null;

            var condition = condition1;
            if (streamId == 2) condition = condition2;
            if (streamId == 3) condition = condition3;
            if (streamId == 4) condition = condition4;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.SensorMode == sensorMode && bitrates.Key.Compression == condition.Compression && bitrates.Key.TvStandard == tvStandard &&
                    bitrates.Key.Resolution == condition.Resolution && bitrates.Key.StreamId == streamId)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, Compression compression, TvStandard tvStandard, SensorMode sensorMode, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetResolutionByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            //A-MTK Series
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

        //For DynaColor type
        //===========================================================
        public List<CertisProfileSetting> ProfileSettings = new List<CertisProfileSetting>();
        private static List<CertisProfileSetting> FindProfileSettingByCondition(IEnumerable<CertisProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<CertisProfileSetting>();

            foreach (CertisProfileSetting node in nodes)
            {
                foreach (CertisProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (CertisProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (CertisProfileFrameRate frameRate in resolution.FrameRates)
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
            CertisProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<CertisProfileSetting> { targetProfile1 }, condition1);
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
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<CertisProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (CertisProfileCompression compression in profileCompressions)
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

            CertisProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<CertisProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<CertisProfileSetting> { targetProfile1 }, condition1);
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
        public UInt16[] GetFrameRateByProfileSetting(List<CertisProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (CertisProfileSetting profileSetting in profileSettings)
            {
                foreach (CertisProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (CertisProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (CertisProfileFrameRate frameRate in resolution.FrameRates)
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

            CertisProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<CertisProfileSetting> { targetProfile1 }, condition1);
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
        public Bitrate[] GetBiterateByProfileSetting(CertisProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (CertisProfileCompression compression in profileSetting.Compressions)
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
            CertisProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<CertisProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<CertisProfileSetting> { targetProfile1 }, condition1);
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
        public Resolution[] GetResolutionByProfileSetting(List<CertisProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (CertisProfileSetting profileSetting in profileSettings)
            {
                foreach (CertisProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (CertisProfileResolution resolution in compression.Resolutions)
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

    public class CertisCompressionCondition
    {
        public Resolution Resolution;
        public UInt16 StreamId = 1;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class CertisStreamCondition
    {
        public Compression Compression;
        public UInt16 StreamId = 1;
        public Resolution Resolution;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class CertisFramrateCondition
    {
        public UInt16 StreamId = 1;
        public Compression PrimaryCompression;
        public Resolution PrimaryResolution;
        public Compression Compression;
        public Resolution Resolution;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class CertisProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<CertisProfileCompression> Compressions;
    }

    public class CertisProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<CertisProfileResolution> Resolutions;
    }

    public class CertisProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<CertisProfileFrameRate> FrameRates;
    }

    public class CertisProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public CertisProfileSetting ProfileSetting;
    }

    public class CertisAMTKCompression
    {
        public SensorMode SensorMode;
        public TvStandard TvStandard;
        public UInt16 StreamId = 1;
        public Compression Compression;
    }

    public class CertisAMTKResolutionCondition
    {
        public SensorMode SensorMode;
        public TvStandard TvStandard;
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public class CertisAMTKStreamCondition : CertisAMTKResolutionCondition
    {
        public Resolution Resolution;
    }

    public partial class ParseCameraModel
    {
        public static void ParseCertis(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new CertisCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyCertisProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            if(cameraModel.Type == "fisheye")
            {
                cameraModel.Alias = Xml.GetFirstElementValueByTagName(node, "Alias");
            }

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            if (cameraModel.Series == "DynaColor")
            {
                ParseCertisByDynaColor(node, list, cameraModel);
                return;
            }

            if (cameraModel.Series == "A-MTK")
            {
                ParseCertisByAMTK(node, list, cameraModel);
                return;
            }

            //cameraModel.CameraMode.Clear();

            //var profileMode = Xml.GetFirstElementByTagName(node, "ProfileMode");
            //if (profileMode == null) return;

            //String mode = profileMode.GetAttribute("value");
            //CameraMode cameraMode = CameraMode.Single;
            //switch (mode)
            //{
            //    case "2":
            //        cameraMode = CameraMode.Dual;
            //        break;

            //    case "3":
            //        cameraMode = CameraMode.Triple;
            //        break;
            //}

            //if (!cameraModel.CameraMode.Contains(cameraMode))
            //    cameraModel.CameraMode.Add(cameraMode);

            //String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
            //if (bitrateStr == "") return;
            //cameraModel.Bitrate = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

            //cameraModel.TvStandard.Clear();
            //var tvStandards = node.GetElementsByTagName("TVStandard");

            //if (tvStandards.Count == 0)
            //{
            //    cameraModel.TvStandard.Add(TvStandard.NonSpecific);
            //    ParseCertisProfile(cameraModel, node, TvStandard.NonSpecific);
            //}
            //else
            //{
            //    foreach (XmlElement tvStandardNode in tvStandards)
            //    {
            //        var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

            //        if (!cameraModel.TvStandard.Contains(tvStandard))
            //            cameraModel.TvStandard.Add(tvStandard);

            //        ParseCertisProfile(cameraModel, tvStandardNode, tvStandard);
            //    }
            //}
        }

        private static void CopyCertisProfile(XmlElement node, CertisCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            CertisCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (CertisCameraModel)mode;
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
            cameraModel.Alias = model;
            cameraModel.Series = copyFrom.Series;
            cameraModel.TvStandard = copyFrom.TvStandard;
            cameraModel.SensorMode = copyFrom.SensorMode;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            cameraModel.ProfileSettings = copyFrom.ProfileSettings;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseCertisProfile(CertisCameraModel cameraModel, XmlElement node, TvStandard tvStandard)
        {
            var primaryStreaming = Xml.GetFirstElementByTagName(node, "PrimaryStreaming");
            if (primaryStreaming == null) return;

            if (primaryStreaming.ChildNodes.Count == 0) return;

            var compressions = new List<Compression>();
            foreach (XmlElement compressionNode in primaryStreaming.ChildNodes)
            {
                if (compressionNode.ChildNodes.Count == 0) return;

                String compressionStr = compressionNode.GetAttribute("codes");
                if (compressionStr == "") return;
                Compression[] comps = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);

                foreach (Compression compression in comps)
                {
                    if (!compressions.Contains(compression))
                        compressions.Add((compression));

                    var resolutions = new List<Resolution>();

                    foreach (XmlElement resolutionNode in compressionNode.ChildNodes)
                    {
                        ParseCertisPrimaryResolution(cameraModel, compression, tvStandard, resolutions, resolutionNode);
                    }

                    Boolean added = false;
                    foreach (var obj in cameraModel.Resolution)
                    {
                        if (obj.Key.StreamId == 1 && obj.Key.Compression == compression && obj.Key.TvStandard == tvStandard)
                        {
                            if (obj.Value != null && obj.Value.Length != 0)
                            {
                                foreach (Resolution resolution in obj.Value)
                                {
                                    if (!resolutions.Contains(resolution))
                                        resolutions.Add((resolution));
                                }
                            }
                            resolutions.Sort();
                            cameraModel.Resolution[obj.Key] = resolutions.ToArray();
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        resolutions.Sort();
                        //cameraModel.Resolution.Add(new CertisStreamCondition { StreamId = 1, Compression = compression, TvStandard = tvStandard }, resolutions.ToArray());
                    }
                }
            }
            compressions.Sort();
            //cameraModel.Compression.Add(new CertisCompressionCondition { StreamId = 1, TvStandard = tvStandard }, compressions.ToArray());
        }

        private static void ParseCertisPrimaryResolution(CertisCameraModel cameraModel, Compression compression, TvStandard tvStandard, List<Resolution> resolutionList, XmlElement node)
        {
            var resolutions = Array.ConvertAll(node.GetAttribute("value").Split(','), Resolutions.ToIndex);

            String framerateStr = Xml.GetFirstElementValueByTagName(node, "FrameRate");
            if (framerateStr == "") return;
            UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);

            var secondStreamingNode = Xml.GetFirstElementByTagName(node, "SecondStreaming");
            var thirdStreamingNode = Xml.GetFirstElementByTagName(node, "ThirdStreaming");

            foreach (var resolution in resolutions)
            {
                if (!resolutionList.Contains(resolution))
                    resolutionList.Add((resolution));

                if (secondStreamingNode != null)
                    ParseCertisSubProfile(cameraModel, 2, compression, resolution, tvStandard, secondStreamingNode);

                if (thirdStreamingNode != null)
                    ParseCertisSubProfile(cameraModel, 3, compression, resolution, tvStandard, thirdStreamingNode);

                //cameraModel.Framerate.Add(new CertisFramrateCondition
                //{
                //    StreamId = 1,
                //    PrimaryCompression = compression,
                //    PrimaryResolution = resolution,
                //    Compression = compression,
                //    Resolution = resolution,
                //    TvStandard = tvStandard
                //}, framerates);
            }
        }

        private static void ParseCertisSubProfile(CertisCameraModel cameraModel, UInt16 streamId, Compression primaryCompression, Resolution primaryResolution, TvStandard tvStandard, XmlElement node)
        {
            //if (node.ChildNodes.Count == 0) return;

            //var compressions = new List<Compression>();
            //foreach (XmlElement compressionNode in node.ChildNodes)
            //{
            //    if (compressionNode.ChildNodes.Count == 0) continue;

            //    String compressionStr = compressionNode.GetAttribute("codes");
            //    if (compressionStr == "") return;
            //    Compression[] comps = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);

            //    var resolutionList = new List<Resolution>();
            //    foreach (Compression compression in comps)
            //    {
            //        if (!compressions.Contains(compression))
            //            compressions.Add((compression));

            //        foreach (XmlElement resolutionNode in compressionNode.ChildNodes)
            //        {
            //            var resolutions = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);

            //            String framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");
            //            if (framerateStr == "") return;
            //            UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
            //            Array.Sort(framerates);

            //            foreach (var resolution in resolutions)
            //            {
            //                if (!resolutionList.Contains(resolution))
            //                    resolutionList.Add((resolution));

            //                cameraModel.Framerate.Add(new CertisFramrateCondition { StreamId = streamId, PrimaryCompression = primaryCompression, PrimaryResolution = primaryResolution, Compression = compression, Resolution = resolution, TvStandard = tvStandard }, framerates);
            //            }
            //        }

            //        resolutionList.Sort();

            //        Boolean added = false;
            //        foreach (var obj in cameraModel.Resolution)
            //        {
            //            if (obj.Key.StreamId == streamId && obj.Key.Compression == compression && obj.Key.Resolution == primaryResolution && obj.Key.TvStandard == tvStandard)
            //            {
            //                cameraModel.Resolution[obj.Key] = resolutionList.ToArray();
            //                added = true;
            //                break;
            //            }
            //        }
            //        if (!added)
            //            cameraModel.Resolution.Add(new CertisStreamCondition { StreamId = streamId, Compression = compression, Resolution = primaryResolution, TvStandard = tvStandard }, resolutionList.ToArray());
            //    }
            //}

            //compressions.Sort();
            //compressions.Add(Compression.Off);
            //cameraModel.Compression.Add(new CertisCompressionCondition { StreamId = streamId, Resolution = primaryResolution, TvStandard = tvStandard }, compressions.ToArray());
        }

        public static void ParseCertisByDynaColor(XmlElement node, List<CameraModel> list, CertisCameraModel cameraModel)
        {
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);

                ParseCertisByDynaColorProfileMode(TvStandard.NonSpecific, node, cameraModel);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParseCertisByDynaColorProfileMode(tvStandard, tvStandardNode, cameraModel);
                }
            }


        }

        private static void ParseCertisByDynaColorProfileMode(TvStandard tvStandard, XmlElement node, CertisCameraModel cameraModel)
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
                        cameraModel.ProfileSettings.Add(ParseCertisByDynaColorProfileNode(tvStandard, cameraMode, profile));
                }
            }
        }

        private static CertisProfileSetting ParseCertisByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new CertisProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<CertisProfileCompression>()
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
                    var newCompression = new CertisProfileCompression
                    {
                        Compression = codec,
                        Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                        Resolutions = new List<CertisProfileResolution>()
                    };

                    var resolutions = compression.SelectNodes("Resolution");
                    if (resolutions != null)
                    {
                        foreach (XmlElement resolution in resolutions)
                        {
                            var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                            var newResolution = new CertisProfileResolution
                            {
                                Resolutions = res.ToList(),
                                FrameRates = new List<CertisProfileFrameRate>()
                            };

                            var framerates = resolution.SelectNodes("FrameRate");
                            if (framerates != null)
                            {
                                foreach (XmlElement framerate in framerates)
                                {
                                    var newFrameRate = new CertisProfileFrameRate
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
                                            newFrameRate.ProfileSetting = ParseCertisByDynaColorProfileNode(tvStandard, mode, profile);
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

        public static void ParseCertisByAMTK(XmlElement node, List<CameraModel> list, CertisCameraModel cameraModel)
        {
            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            cameraModel.CameraMode.Clear();

            var sensorModes = node.GetElementsByTagName("SensorMode");
            if (sensorModes.Count > 0)
            {
                foreach (XmlElement profileSensorModeNode in sensorModes)
                {
                    var sensorMode = SensorModes.ToIndex(profileSensorModeNode.GetAttribute("value"));
                    ParseCertisByAMTKSensorModeProfile(cameraModel, sensorMode, TvStandard.NonSpecific, profileSensorModeNode);

                    if (!cameraModel.SensorMode.Contains(sensorMode))
                        cameraModel.SensorMode.Add(sensorMode);
                }
            }
            else
            {
                ParseCertisByAMTKSensorModeProfile(cameraModel, SensorMode.NonSpecific, TvStandard.NonSpecific, node);
            }

            if (cameraModel.SensorMode.Count > 1)
                cameraModel.SensorMode.Remove(SensorMode.NonSpecific);
        }

        private static void ParseCertisByAMTKSensorModeProfile(CertisCameraModel cameraModel, SensorMode sensorMode, TvStandard tvStandard, XmlElement node)
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
                    ParseCertisByAMTKProfile(cameraModel, sensorMode, tvStandard, profileNode);
                }
            }
        }

        private static void ParseCertisByAMTKProfile(CertisCameraModel cameraModel, SensorMode sensorMode, TvStandard tvStandard, XmlElement node)
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

                    var resolutionConditions = new List<CertisAMTKResolutionCondition>();

                    foreach (var compression in compressionArray)
                    {
                        resolutionConditions.Add(new CertisAMTKResolutionCondition { StreamId = id, SensorMode = sensorMode, TvStandard = tvStandard, Compression = compression });
                    }
                    //-----------------------------------
                    var resolutionNodes = compressionNode.GetElementsByTagName("Resolution");
                    var resolutionList = new List<Resolution>();
                    foreach (XmlElement resolutionNode in resolutionNodes)
                    {
                        var resolutionArray = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        Array.Sort(resolutionArray);

                        resolutionList.AddRange(resolutionArray);

                        var streamConditions = new List<CertisAMTKStreamCondition>();
                        foreach (CertisAMTKResolutionCondition condition in resolutionConditions)
                        {
                            foreach (Resolution resolution in resolutionList)
                            {
                                streamConditions.Add(new CertisAMTKStreamCondition { StreamId = id, SensorMode = sensorMode, Compression = condition.Compression, Resolution = resolution, TvStandard = tvStandard });
                            }
                        }

                        //-----------------------------------
                        var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                        if (framerateStr != "")
                        {
                            var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                            Array.Sort(framerates);

                            foreach (CertisAMTKStreamCondition condition in streamConditions)
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
                    cameraModel.Compression.Add(new CertisAMTKCompression { Compression = compression, StreamId = id, SensorMode = sensorMode, TvStandard = tvStandard });
                }
            }

        }
    }
}
