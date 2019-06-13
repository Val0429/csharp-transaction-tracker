using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class MessoaCameraModel : CameraModel
    {
        public UInt16 AudioOutPort;
        public List<TvStandard> TvStandard = new List<TvStandard>();

        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency> { DeviceConstant.PowerFrequency.NonSpecific };

        public Dictionary<MessoaStreamCondition, Compression[]> Compression = new Dictionary<MessoaStreamCondition, Compression[]>();
        public Dictionary<MessoaStreamCondition, Resolution[]> Resolution = new Dictionary<MessoaStreamCondition, Resolution[]>();
        public Dictionary<MessoaStreamCondition, UInt16[]> Framerate = new Dictionary<MessoaStreamCondition, UInt16[]>();
        public Dictionary<MessoaStreamCondition, Bitrate[]> Bitrate = new Dictionary<MessoaStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, StreamConfig streamConfig, TvStandard tvStandard)
        {
            if (Compression.Count == 0 || streamConfig == null) return null;

            if (streamId == 1)
            {
                foreach (var compressionse in Compression)
                {
                    if (compressionse.Key.TvStandard == tvStandard && compressionse.Key.StreamId == streamId)
                        return compressionse.Value;
                }
            }
            else
            {
                foreach (var compressionse in Compression)
                {
                    if (compressionse.Key.TvStandard == tvStandard && compressionse.Key.Compression == streamConfig.Compression &&
                        compressionse.Key.Resolution == streamConfig.Resolution && compressionse.Key.StreamId == streamId)
                    {
                        return compressionse.Value;
                    }
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig streamConfig, TvStandard tvStandard)
        {
            if (Resolution.Count == 0 || streamConfig == null) return null;

            if (streamId == 1)
            {
                foreach (var resolutionse in Resolution)
                {
                    if (resolutionse.Key.TvStandard == tvStandard && resolutionse.Key.Compression == streamConfig.Compression && resolutionse.Key.StreamId == streamId)
                    {
                        return resolutionse.Value;
                    }
                }
            }
            else
            {
                foreach (var resolutionse in Resolution)
                {
                    if (resolutionse.Key.TvStandard == tvStandard && resolutionse.Key.Compression == streamConfig.Compression &&
                        resolutionse.Key.Resolution == streamConfig.Resolution && resolutionse.Key.StreamId == streamId)
                    {
                        return resolutionse.Value;
                    }
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, TvStandard tvStandard)
        {
            if (Framerate.Count == 0 || streamConfig == null) return null;

            foreach (var framerate in Framerate)
            {
                if (framerate.Key.TvStandard == tvStandard && framerate.Key.Compression == streamConfig.Compression &&
                    framerate.Key.Resolution == streamConfig.Resolution && framerate.Key.StreamId == streamId)
                {
                    return framerate.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig, TvStandard tvStandard)
        {
            if (Bitrate.Count == 0 || streamConfig == null) return null;

            foreach (var bitratese in Bitrate)
            {
                if (bitratese.Key.TvStandard == tvStandard && bitratese.Key.Compression == streamConfig.Compression &&
                    bitratese.Key.Resolution == streamConfig.Resolution && bitratese.Key.StreamId == streamId)
                {
                    return bitratese.Value;
                }
            }

            return null;
        }
    }

    public class MessoaStreamCondition
    {
        public TvStandard TvStandard;
        public Compression Compression;
        public Resolution Resolution;
        public UInt16 StreamId = 1;
    }

    public partial class ParseCameraModel
    {
        public static void ParseMessoa(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new MessoaCameraModel();

            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyMessoaProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");
            var powerFrequencies = Array.ConvertAll(Xml.GetFirstElementValueByTagName(node, "PowerFrequency").Split(','), PowerFrequencies.ToIndex);

            foreach (var powerFrequency in powerFrequencies)
            {
                if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                    cameraModel.PowerFrequency.Add(powerFrequency);
            }

            cameraModel.AudioOutPort = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "AudioOutPort"));

            XmlNodeList tvstandards = node.GetElementsByTagName("TVStandard");

            cameraModel.CameraMode.Clear();
            cameraModel.CameraMode.Add(CameraMode.Triple);

            foreach (XmlElement tvstandard in tvstandards)
            {
                TvStandard tvStandard = TvStandards.ToIndex(tvstandard.GetAttribute("value"));
                cameraModel.TvStandard.Add(tvStandard);

                XmlNodeList primaryStreamings = tvstandard.GetElementsByTagName("PrimaryStreaming");
                var primaryCompress = new List<Compression>();
                foreach (XmlElement primaryStreaming in primaryStreamings)
                {
                    var compressionNode = Xml.GetFirstElementByTagName(primaryStreaming, "Compression");
                    var compression = Compressions.ToIndex(compressionNode.GetAttribute("codes"));
                    primaryCompress.Add(compression);

                    var primaryResolution = new List<Resolution>();
                    foreach (XmlElement resolutionNode in compressionNode.ChildNodes)
                    {
                        var resolution = Resolutions.ToIndex(resolutionNode.GetAttribute("value"));
                        primaryResolution.Add(resolution);

                        var condition = new MessoaStreamCondition
                        {
                            TvStandard = tvStandard,
                            Compression = compression,
                            Resolution = resolution,
                            StreamId = 1
                        };

                        XmlElement secondStreamingNode = Xml.GetFirstElementByTagName(resolutionNode, "SecondStreaming");
                        XmlElement thridStreamingNode = Xml.GetFirstElementByTagName(resolutionNode, "ThirdStreaming");

                        if (secondStreamingNode != null)
                        {
                            ParseMessoaSubStreamingNode(secondStreamingNode, 2, compression, resolution, tvStandard, cameraModel);
                            resolutionNode.RemoveChild(secondStreamingNode);
                        }

                        if (thridStreamingNode != null)
                        {
                            ParseMessoaSubStreamingNode(thridStreamingNode, 3, compression, resolution, tvStandard, cameraModel);
                            resolutionNode.RemoveChild(thridStreamingNode);
                        }

                        var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");
                        if (framerateStr != "")
                        {
                            var framerates = (Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16));
                            Array.Sort(framerates);
                            cameraModel.Framerate.Add(condition, framerates);
                        }

                        var bitrateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "Bitrate");
                        if (bitrateStr == "") continue;

                        var bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                        Array.Sort(bitrates);
                        cameraModel.Bitrate.Add(condition, bitrates);
                    }

                    primaryResolution.Sort();
                    cameraModel.Resolution.Add(new MessoaStreamCondition
                    {
                        TvStandard = tvStandard,
                        Compression = compression,
                        StreamId = 1
                    }, primaryResolution.ToArray());
                }

                primaryCompress.Sort();
                cameraModel.Compression.Add(new MessoaStreamCondition
                {
                    TvStandard = tvStandard,
                    StreamId = 1
                }, primaryCompress.ToArray());
            }

            if (cameraModel.PowerFrequency.Count > 1)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.Sort();
            }
        }

        private static void CopyMessoaProfile(XmlElement node, MessoaCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            MessoaCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (MessoaCameraModel)mode;
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
            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            cameraModel.AudioOutPort = copyFrom.AudioOutPort;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;

            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseMessoaSubStreamingNode(XmlElement streamNode, UInt16 streamId, Compression compression, Resolution resolution, TvStandard tvStandard, MessoaCameraModel cameraModel)
        {
            if (streamNode.InnerXml == "") return;

            var compressionNodes = streamNode.GetElementsByTagName("Compression");
            var subCompress = new List<Compression>();

            var condition = new MessoaStreamCondition
            {
                TvStandard = tvStandard,
                Compression = compression,
                Resolution = resolution,
                StreamId = streamId
            };

            foreach (XmlElement compressionNode in compressionNodes)
            {
                subCompress.AddRange(Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex));
                var resolutions = Array.ConvertAll(Xml.GetFirstElementValueByTagName(compressionNode, "Resolution").Split(','), Resolutions.ToIndex);
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(condition, resolutions);

                var bitrateStr = Xml.GetFirstElementValueByTagName(compressionNode, "Bitrate");
                if (bitrateStr != "")
                {
                    Bitrate[] bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                    Array.Sort(bitrates);
                    cameraModel.Bitrate.Add(condition, bitrates);
                }

                String framerateStr = Xml.GetFirstElementValueByTagName(compressionNode, "FrameRate");
                if (framerateStr != "")
                {
                    UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);
                    Array.Sort(framerates);
                    cameraModel.Framerate.Add(condition, framerates);
                }
            }

            subCompress.Sort();
            subCompress.Add(Compression.Off);
            cameraModel.Compression.Add(condition, subCompress.ToArray());
        }
    }
}
