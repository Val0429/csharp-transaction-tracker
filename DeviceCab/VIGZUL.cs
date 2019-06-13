using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class VIGZULCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public Dictionary<VIGZULCompressionCondition, Compression[]> Compression = new Dictionary<VIGZULCompressionCondition, Compression[]>();
        public Dictionary<VIGZULStreamCondition, Resolution[]> Resolution = new Dictionary<VIGZULStreamCondition, Resolution[]>();
        public Dictionary<VIGZULFramrateCondition, UInt16[]> Framerate = new Dictionary<VIGZULFramrateCondition, UInt16[]>();
        public Bitrate[] Bitrate;

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3)
        {
            if (Series == "DynaColor")
                return GetCompressionByCondition(tvStandard, cameraMode, connectionProtocol, streamId == 1 ? null : condition1, condition2, condition3);

            if (Compression.Count == 0) return null;

            Compression[] compression = null;
            if (streamId == 1)
            {
                foreach (var compressionse in Compression)
                {
                    if (compressionse.Key.StreamId == streamId && compressionse.Key.TvStandard == tvStandard)
                    {
                        compression = compressionse.Value;
                    }
                }
            }
            else
            {
                foreach (var compressionse in Compression)
                {
                    if (compressionse.Key.StreamId == streamId && compressionse.Key.Resolution == condition1.Resolution && compressionse.Key.TvStandard == tvStandard)
                    {
                        compression = compressionse.Value;
                    }
                }
            }

            if (compression != null)
            {
                if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
                {
                    if (compression.Contains(DeviceConstant.Compression.Mjpeg))
                    {
                        if (streamId == 1)
                            return new[] { DeviceConstant.Compression.Mjpeg };

                        if (compression.Contains(DeviceConstant.Compression.Disable))
                        {
                            return new[] { DeviceConstant.Compression.Mjpeg, DeviceConstant.Compression.Disable };
                        }
                        return new[] { DeviceConstant.Compression.Mjpeg, DeviceConstant.Compression.Off };
                    }
                }
            }

            return compression;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, Compression compression, Resolution resolution, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetFramerateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.StreamId == streamId && framerates.Key.Compression == compression && framerates.Key.Resolution == resolution && framerates.Key.TvStandard == tvStandard &&
                    framerates.Key.PrimaryCompression == condition1.Compression && framerates.Key.PrimaryResolution == condition1.Resolution)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetBitrateByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            return Bitrate;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, Compression compression, TvStandard tvStandard, CameraMode cameraMode, StreamConfig condition1, StreamConfig condition2, StreamConfig condition3, StreamConfig condition4)
        {
            if (Series == "DynaColor")
                return GetResolutionByCondition(tvStandard, cameraMode, condition1, condition2, condition3, condition4);

            if (Resolution.Count == 0) return null;
            //streamConfig.ConnectionProtocol
            List<Resolution> resolutions = null;
            if (streamId == 1)
            {
                foreach (var resolution in Resolution)
                {
                    if (resolution.Key.StreamId == streamId && resolution.Key.Compression == condition1.Compression && resolution.Key.TvStandard == tvStandard)
                    {
                        resolutions = new List<Resolution>(resolution.Value);
                        break;
                    }
                }
            }
            else
            {
                foreach (var resolution in Resolution)
                {
                    if (resolution.Key.StreamId == streamId && resolution.Key.PrimaryCompression == condition1.Compression && resolution.Key.Compression == compression
                        && resolution.Key.Resolution == condition1.Resolution && resolution.Key.TvStandard == tvStandard)
                    {
                        resolutions = new List<Resolution>(resolution.Value);
                        break;
                    }
                }
            }

            if (resolutions != null && connectionProtocol != DeviceConstant.ConnectionProtocol.Http)
            {
                resolutions.Remove(DeviceConstant.Resolution.R2304X1728);
                resolutions.Remove(DeviceConstant.Resolution.R2592X1944);
            }

            //Only HTTP with MJPEG support resolution(height or width) >= 2048 
            if (resolutions != null && (connectionProtocol != DeviceConstant.ConnectionProtocol.Http || compression != DeviceConstant.Compression.Mjpeg))
            {
                var removeResolutions = new List<Resolution>();
                foreach (Resolution resolution in resolutions)
                {
                    if (Resolutions.ToHeight(resolution) >= 2048 || Resolutions.ToWidth(resolution) >= 2048)
                        removeResolutions.Add(resolution);
                }

                if (removeResolutions.Count > 0)
                {
                    foreach (Resolution removeResolution in removeResolutions)
                    {
                        resolutions.Remove(removeResolution);
                    }
                }
            }

            return (resolutions != null) ? resolutions.ToArray() : null;
        }

        //For DynaColor type
        //===========================================================
        public List<VIGZULProfileSetting> ProfileSettings = new List<VIGZULProfileSetting>();
        private static List<VIGZULProfileSetting> FindProfileSettingByCondition(IEnumerable<VIGZULProfileSetting> nodes, StreamConfig condition)
        {
            var profileSettings = new List<VIGZULProfileSetting>();

            foreach (VIGZULProfileSetting node in nodes)
            {
                foreach (VIGZULProfileCompression compression in node.Compressions)
                {
                    if (compression.Compression == condition.Compression)
                    {
                        foreach (VIGZULProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condition.Resolution) > -1)
                            {
                                foreach (VIGZULProfileFrameRate frameRate in resolution.FrameRates)
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
            VIGZULProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);
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
        public Compression[] GetCompressionsByProfileSetting(IEnumerable<VIGZULProfileCompression> profileCompressions, ConnectionProtocol connectionProtocol)
        {
            var compressions = new List<Compression>();

            foreach (VIGZULProfileCompression compression in profileCompressions)
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

            VIGZULProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetFrameRateByProfileSetting(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);
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
        public UInt16[] GetFrameRateByProfileSetting(List<VIGZULProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<UInt16[]>();

            foreach (VIGZULProfileSetting profileSetting in profileSettings)
            {
                foreach (VIGZULProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (VIGZULProfileResolution resolution in compression.Resolutions)
                        {
                            if (resolution.Resolutions.IndexOf(condiction.Resolution) > -1)
                            {
                                foreach (VIGZULProfileFrameRate frameRate in resolution.FrameRates)
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

            VIGZULProfileSetting targetProfile1 = null;

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
            var targetProfile2 = FindProfileSettingByCondition(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);
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
        public Bitrate[] GetBiterateByProfileSetting(VIGZULProfileSetting profileSetting, StreamConfig condiction)
        {
            foreach (VIGZULProfileCompression compression in profileSetting.Compressions)
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
            VIGZULProfileSetting targetProfile1 = null;

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
            if (condition2 == null) return GetResolutionByProfileSetting(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);

            //find to profile2
            var targetProfile2 = FindProfileSettingByCondition(new List<VIGZULProfileSetting> { targetProfile1 }, condition1);
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
        public Resolution[] GetResolutionByProfileSetting(List<VIGZULProfileSetting> profileSettings, StreamConfig condiction)
        {
            var list = new List<Resolution[]>();
            foreach (VIGZULProfileSetting profileSetting in profileSettings)
            {
                foreach (VIGZULProfileCompression compression in profileSetting.Compressions)
                {
                    if (compression.Compression == condiction.Compression)
                    {
                        foreach (VIGZULProfileResolution resolution in compression.Resolutions)
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

    public class VIGZULCompressionCondition
    {
        public Resolution Resolution;
        public UInt16 StreamId = 1;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class VIGZULStreamCondition
    {
        public Compression PrimaryCompression;
        public Compression Compression;
        public UInt16 StreamId = 1;
        public Resolution Resolution;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class VIGZULFramrateCondition
    {
        public UInt16 StreamId = 1;
        public Compression PrimaryCompression;
        public Resolution PrimaryResolution;
        public Compression Compression;
        public Resolution Resolution;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class VIGZULProfileSetting
    {
        public TvStandard TvStandard;
        public CameraMode Mode;
        public UInt16 ProfileId;
        public List<VIGZULProfileCompression> Compressions;
    }

    public class VIGZULProfileCompression
    {
        public Compression Compression;
        public Bitrate[] Bitrates;
        public List<VIGZULProfileResolution> Resolutions;
    }

    public class VIGZULProfileResolution
    {
        public List<Resolution> Resolutions;
        public List<VIGZULProfileFrameRate> FrameRates;
    }

    public class VIGZULProfileFrameRate
    {
        public List<UInt16> FrameRates;
        public VIGZULProfileSetting ProfileSetting;
    }

    public partial class ParseCameraModel
    {
        public static void ParseVIGZUL(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new VIGZULCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyVIGZULProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            if (cameraModel.Series == "DynaColor")
            {
                ParseVIGZULByDynaColor(node, list, cameraModel);
                return;
            }

            cameraModel.CameraMode.Clear();

            var profileMode = Xml.GetFirstElementByTagName(node, "ProfileMode");
            if (profileMode == null) return;

            String mode = profileMode.GetAttribute("value");
            CameraMode cameraMode = CameraMode.Single;
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

            String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
            if (bitrateStr == "") return;
            cameraModel.Bitrate = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

            cameraModel.TvStandard.Clear();
            var tvStandards = node.GetElementsByTagName("TVStandard");

            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);
                ParseVIGZULProfile(cameraModel, node, TvStandard.NonSpecific);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParseVIGZULProfile(cameraModel, tvStandardNode, tvStandard);
                }
            }
        }

        private static void CopyVIGZULProfile(XmlElement node, VIGZULCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            VIGZULCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (VIGZULCameraModel)mode;
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

        private static void ParseVIGZULProfile(VIGZULCameraModel cameraModel, XmlElement node, TvStandard tvStandard)
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
                        ParseVIGZULPrimaryResolution(cameraModel, compression, tvStandard, resolutions, resolutionNode);
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
                        cameraModel.Resolution.Add(new VIGZULStreamCondition { StreamId = 1, Compression = compression, TvStandard = tvStandard }, resolutions.ToArray());
                    }
                }
            }
            compressions.Sort();
            cameraModel.Compression.Add(new VIGZULCompressionCondition { StreamId = 1, TvStandard = tvStandard }, compressions.ToArray());
        }

        private static void ParseVIGZULPrimaryResolution(VIGZULCameraModel cameraModel, Compression compression, TvStandard tvStandard, List<Resolution> resolutionList, XmlElement node)
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
                    ParseVIGZULSubProfile(cameraModel, 2, compression, resolution, tvStandard, secondStreamingNode);

                if (thirdStreamingNode != null)
                    ParseVIGZULSubProfile(cameraModel, 3, compression, resolution, tvStandard, thirdStreamingNode);

                cameraModel.Framerate.Add(new VIGZULFramrateCondition
                {
                    StreamId = 1,
                    PrimaryCompression = compression,
                    PrimaryResolution = resolution,
                    Compression = compression,
                    Resolution = resolution,
                    TvStandard = tvStandard
                }, framerates);
            }
        }

        private static void ParseVIGZULSubProfile(VIGZULCameraModel cameraModel, UInt16 streamId, Compression primaryCompression, Resolution primaryResolution, TvStandard tvStandard, XmlElement node)
        {
            if (node.ChildNodes.Count == 0) return;

            var compressions = new List<Compression>();
            foreach (XmlElement compressionNode in node.ChildNodes)
            {
                if (compressionNode.ChildNodes.Count == 0) continue;

                String compressionStr = compressionNode.GetAttribute("codes");
                if (compressionStr == "") return;
                Compression[] comps = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);

                var resolutionList = new List<Resolution>();
                foreach (Compression compression in comps)
                {
                    if (!compressions.Contains(compression))
                        compressions.Add((compression));

                    foreach (XmlElement resolutionNode in compressionNode.ChildNodes)
                    {
                        var resolutions = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);

                        String framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");
                        if (framerateStr == "") return;
                        UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                        Array.Sort(framerates);

                        foreach (var resolution in resolutions)
                        {
                            if (!resolutionList.Contains(resolution))
                                resolutionList.Add((resolution));

                            cameraModel.Framerate.Add(new VIGZULFramrateCondition { StreamId = streamId, PrimaryCompression = primaryCompression, PrimaryResolution = primaryResolution, Compression = compression, Resolution = resolution, TvStandard = tvStandard }, framerates);
                        }
                    }

                    resolutionList.Sort();

                    Boolean added = false;
                    foreach (var obj in cameraModel.Resolution)
                    {
                        if (obj.Key.StreamId == streamId && obj.Key.PrimaryCompression == primaryCompression && obj.Key.Compression == compression && obj.Key.Resolution == primaryResolution && obj.Key.TvStandard == tvStandard)
                        {
                            cameraModel.Resolution[obj.Key] = resolutionList.ToArray();
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                        cameraModel.Resolution.Add(new VIGZULStreamCondition { StreamId = streamId, PrimaryCompression = primaryCompression, Compression = compression, Resolution = primaryResolution, TvStandard = tvStandard }, resolutionList.ToArray());
                }
            }

            compressions.Sort();
            if (!compressions.Contains(Compression.Disable))
                compressions.Add(Compression.Off);
            cameraModel.Compression.Add(new VIGZULCompressionCondition { StreamId = streamId, Resolution = primaryResolution, TvStandard = tvStandard }, compressions.ToArray());
        }

        public static void ParseVIGZULByDynaColor(XmlElement node, List<CameraModel> list, VIGZULCameraModel cameraModel)
        {
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);

                ParseVIGZULByDynaColorProfileMode(TvStandard.NonSpecific, node, cameraModel);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParseVIGZULByDynaColorProfileMode(tvStandard, tvStandardNode, cameraModel);
                }
            }


        }

        private static void ParseVIGZULByDynaColorProfileMode(TvStandard tvStandard, XmlElement node, VIGZULCameraModel cameraModel)
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
                        cameraModel.ProfileSettings.Add(ParseVIGZULByDynaColorProfileNode(tvStandard, cameraMode, profile));
                }
            }
        }


        private static VIGZULProfileSetting ParseVIGZULByDynaColorProfileNode(TvStandard tvStandard, CameraMode mode, XmlElement node)
        {
            var profileSetting = new VIGZULProfileSetting
            {
                TvStandard = tvStandard,
                Mode = mode,
                ProfileId = Convert.ToUInt16(node.GetAttribute("id")),
                Compressions = new List<VIGZULProfileCompression>()
            };

            var compressions = node.SelectNodes("Compression");
            if (compressions == null)
                return profileSetting;

            foreach (XmlElement compression in compressions)
            {
                var codec = Compressions.ToIndex(compression.GetAttribute("codes"));
                var newCompression = new VIGZULProfileCompression
                {
                    Compression = codec,
                    Bitrates = (codec == Compression.Mjpeg) ? null : Array.ConvertAll(Xml.GetFirstElementValueByTagName(compression, "Bitrate").Split(','), Bitrates.DisplayStringToIndex),
                    Resolutions = new List<VIGZULProfileResolution>()
                };

                var resolutions = compression.SelectNodes("Resolution");
                if (resolutions != null)
                {
                    foreach (XmlElement resolution in resolutions)
                    {
                        var res = Array.ConvertAll(resolution.GetAttribute("value").Split(','), Resolutions.ToIndex);
                        var newResolution = new VIGZULProfileResolution
                        {
                            Resolutions = res.ToList(),
                            FrameRates = new List<VIGZULProfileFrameRate>()
                        };

                        var framerates = resolution.SelectNodes("FrameRate");
                        if (framerates != null)
                        {
                            foreach (XmlElement framerate in framerates)
                            {
                                var newFrameRate = new VIGZULProfileFrameRate
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
                                        newFrameRate.ProfileSetting = ParseVIGZULByDynaColorProfileNode(tvStandard, mode, profile);
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
