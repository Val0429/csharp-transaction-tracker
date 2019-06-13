using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class YuanCameraModel : CameraModel
    {
        public List<TvStandard> TvStandard = new List<TvStandard>
        {
            DeviceConstant.TvStandard.NonSpecific
        };

        public Compression[] Compression;
        public Dictionary<TvStandard, UInt16[]> Framerate = new Dictionary<TvStandard, UInt16[]>();
        public Dictionary<TvStandard, Resolution[]> Resolution = new Dictionary<TvStandard, Resolution[]>();
        public Dictionary<TvStandard, Bitrate[]> Bitrate = new Dictionary<TvStandard, Bitrate[]>();

        public UInt16[] GetFramerateByCondition(TvStandard tvStandard)
        {
            if (Framerate.Count == 0) return null;

            return Framerate.ContainsKey(tvStandard) ? Framerate[tvStandard] : null;
        }

        public Bitrate[] GetBitrateByCondition(TvStandard tvStandard)
        {
            if (Bitrate.Count == 0) return null;

            return Bitrate.ContainsKey(tvStandard) ? Bitrate[tvStandard] : null;
        }

        public Resolution[] GetResolutionByCondition(TvStandard tvStandard)
        {
            if (Resolution.Count == 0) return null;

            return Resolution.ContainsKey(tvStandard) ? Resolution[tvStandard] : null;
        }
    }

    public partial class ParseCameraModel
    {
        public static void ParseYuan(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new YuanCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Compression = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "Compression").Split(','), Compressions.ToIndex);
            Array.Sort(cameraModel.Compression);

            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0) return;

            foreach (XmlElement tvStandardNode in tvStandards)
            {
                var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                if (!cameraModel.TvStandard.Contains(tvStandard))
                    cameraModel.TvStandard.Add(tvStandard);

                var framerates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(tvStandardNode, "FrameRate").Split(','), Convert.ToUInt16);
                Array.Sort(framerates);
                cameraModel.Framerate.Add(tvStandard, framerates);

                var bitrates = Array.ConvertAll(Xml.GetFirstElementValueByTagName(tvStandardNode, "Bitrate").Split(','), Bitrates.DisplayStringToIndex);
                Array.Sort(bitrates);
                cameraModel.Bitrate.Add(tvStandard, bitrates);

                var resolutions = Array.ConvertAll(Xml.GetFirstElementValueByTagName(tvStandardNode, "Resolution").Split(','), Resolutions.ToIndex);
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(tvStandard, resolutions);
            }

            if (cameraModel.TvStandard.Count > 1)
                cameraModel.TvStandard.Remove(TvStandard.NonSpecific);
        }
    }
}
