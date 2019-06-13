using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class StretchCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>
        {
            DeviceConstant.TvStandard.NonSpecific
        };

        public Compression[] Compression;
        public Dictionary<StretchStreamCondition, UInt16[]> Framerate = new Dictionary<StretchStreamCondition, UInt16[]>();
        public Dictionary<StretchStreamCondition, Resolution[]> Resolution = new Dictionary<StretchStreamCondition, Resolution[]>();
        public Dictionary<StretchStreamCondition, Bitrate[]> Bitrate = new Dictionary<StretchStreamCondition, Bitrate[]>();

        public UInt16[] GetFramerateByCondition(UInt16 streamId, CameraMode cameraMode, TvStandard tvStandard)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerate in Framerate)
            {
                if (framerate.Key.CameraMode == cameraMode && framerate.Key.StreamId == streamId && framerate.Key.TvStandard == tvStandard)
                {
                    return framerate.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, CameraMode cameraMode, TvStandard tvStandard)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.CameraMode == cameraMode  && bitrates.Key.StreamId == streamId && bitrates.Key.TvStandard == tvStandard)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, CameraMode cameraMode, TvStandard tvStandard)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolution in Resolution)
            {
                if (resolution.Key.CameraMode == cameraMode && resolution.Key.StreamId == streamId && resolution.Key.TvStandard == tvStandard)
                {
                    return resolution.Value;
                }
            }

            return null;
        }
    }

    public class StretchStreamCondition
    {
        public UInt16 StreamId = 1;
        public TvStandard TvStandard = TvStandard.NonSpecific;
        public CameraMode CameraMode = CameraMode.Single;
    }

    public partial class ParseCameraModel
    {
        public static void ParseStrech(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new StretchCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyStretchProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0) return;

            cameraModel.Compression = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "Compression").Split(','), Compressions.ToIndex);
            Array.Sort(cameraModel.Compression);

            foreach (XmlElement tvStandardNode in tvStandards)
            {
                var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                var profileModes = tvStandardNode.GetElementsByTagName("ProfileMode");

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
                    }

                    if (!cameraModel.CameraMode.Contains(cameraMode))
                        cameraModel.CameraMode.Add(cameraMode);

                    foreach (XmlElement profileNode in profiles)
                    {
                        ParseStretchProfile(profileNode, cameraMode, cameraModel, tvStandard);
                    }
                }

                if (!cameraModel.TvStandard.Contains(tvStandard))
                    cameraModel.TvStandard.Add(tvStandard);
            }

            if (cameraModel.TvStandard.Count > 1)
                cameraModel.TvStandard.Remove(TvStandard.NonSpecific);
        }

        private static void CopyStretchProfile(XmlElement node, StretchCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            StretchCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (StretchCameraModel)mode;
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
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseStretchProfile(XmlElement node, CameraMode cameraMode, StretchCameraModel cameraModel, TvStandard tvStandard)
        {
            var ids = Array.ConvertAll(node.GetAttribute("id").Split(','), Convert.ToUInt16);

            foreach (var id in ids)
            {
                var condition = new StretchStreamCondition()
                                    {
                                        TvStandard = tvStandard,
                                        StreamId = id,
                                        CameraMode = cameraMode
                                    };

                var framerates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "FrameRate").Split(','), Convert.ToUInt16);
                Array.Sort(framerates);
                cameraModel.Framerate.Add(condition, framerates);

                var bitrates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "Bitrate").Split(','), Bitrates.DisplayStringToIndex);
                Array.Sort(bitrates);
                cameraModel.Bitrate.Add(condition, bitrates);

                var resolutions = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "Resolution").Split(','), Resolutions.ToIndex);
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(condition, resolutions);
            }
        }
    }
}
