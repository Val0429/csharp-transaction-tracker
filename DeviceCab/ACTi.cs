using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class ACTiCameraModel : CameraModel
    {
        public Dictionary<ACTiCompressionCondition, Compression[]> Compression = new Dictionary<ACTiCompressionCondition, Compression[]>();
        public Dictionary<ACTiResolutionCondition, Resolution[]> Resolution = new Dictionary<ACTiResolutionCondition, Resolution[]>();
        public Dictionary<ACTiFramerateCondition, UInt16[]> Framerate = new Dictionary<ACTiFramerateCondition, UInt16[]>();
        public Dictionary<ACTiBitrateCondition, Bitrate[]> Bitrate = new Dictionary<ACTiBitrateCondition, Bitrate[]>();
        public List<AspectRatio> AspectRatio = new List<AspectRatio>();
        public List<DeviceMountType> DeviceMountType = new List<DeviceMountType>();
        
        public Compression[] GetCompressionByCondition(UInt16 streamId, CameraMode mode)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.Mode == mode)
                    return compressionse.Value;
            }

            return null;
        }

        public Compression[] GetCompressionByCondition(UInt16 streamId, CameraMode mode, Compression primaryCompression)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.PrimaryCompression == primaryCompression && compressionse.Key.Mode == mode)
                    return compressionse.Value;
            }

            return null;
        }

        public Compression[] GetCompressionByStreamCondition(UInt16 streamId, CameraMode mode, AspectRatio aspectRatio, StreamConfig streamConfig, StreamConfig primaryConfig)
        {
            if (Compression.Count == 0) return null;

            var compressions = new List<Compression>();
            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.PrimaryCompression == primaryConfig.Compression && compressionse.Key.Mode == mode)
                {
                    //Because series E & B has different framerate for each resolutions
                    if (Series == "E" || Series == "B") return compressionse.Value;

                    //Because series K has different profile for each framerate
                    if (Series == "K" && mode == DeviceConstant.CameraMode.Dual && !compressionse.Key.PrimaryFramerates.Contains(primaryConfig.Framerate))
                        continue;

                    var compressionsList = new List<Compression>(compressionse.Value);
                    var copyConfig = StreamConfigs.Clone(streamConfig);
                    foreach (var compression in compressionse.Value)
                    {
                        copyConfig.Compression = compression;
                        Resolution[] resolution = null;
                        switch (mode)
                        {
                            case DeviceConstant.CameraMode.Single:
                            case DeviceConstant.CameraMode.SixVga:
                                resolution = GetResolutionByCondition(streamId, copyConfig, mode, aspectRatio);
                                break;

                            case DeviceConstant.CameraMode.Dual:
                                if (streamId == 1)
                                    resolution = GetResolutionByCondition(streamId, copyConfig, mode, aspectRatio);
                                else
                                    resolution = GetResolutionByCondition(streamId, copyConfig, mode, aspectRatio, primaryConfig);
                                break;
                        }
                        if (resolution != null) continue;

                        compressionsList.Remove(compression);
                    }

                    if (Series == "K" && mode == DeviceConstant.CameraMode.Dual)
                    {
                        if (compressionsList.Count > 0)
                        {
                            foreach (Compression compression in compressionsList)
                            {
                                if (!compressions.Contains(compression)) compressions.Add(compression);
                            }
                        }

                        continue;
                    }
                    return (compressionsList.Count > 0) ? compressionsList.ToArray() : null;
                }
            }

            if (Series == "K" && mode == DeviceConstant.CameraMode.Dual)
                return (compressions.Count > 0) ? compressions.ToArray() : null;

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode mode, AspectRatio aspectRatio)
        {
            if (Resolution.Count == 0 || streamConfig == null) return null;

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.StreamId == streamId && resolutionse.Key.Mode == mode && resolutionse.Key.Compression == streamConfig.Compression && resolutionse.Key.AspectRatio == aspectRatio)
                {
                    return resolutionse.Value;
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode mode, AspectRatio aspectRatio, StreamConfig primaryConfig)
        {
            if (Resolution.Count == 0 || streamConfig == null) return null;

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.StreamId == streamId && resolutionse.Key.Mode == mode && resolutionse.Key.Compression == streamConfig.Compression 
                    && resolutionse.Key.PrimaryCompression == primaryConfig.Compression && resolutionse.Key.PrimaryResolution == primaryConfig.Resolution && resolutionse.Key.AspectRatio == aspectRatio)
                {
                    //Because series E & B has different framerate for each resolutions
                    if (Series == "E" || Series == "B") return resolutionse.Value;

                    //Because series K has different profile for each framerate
                    if (Series == "K" && mode == DeviceConstant.CameraMode.Dual && !resolutionse.Key.PrimaryFramerates.Contains(primaryConfig.Framerate))
                        continue;

                    var resolutionList = new List<Resolution>(resolutionse.Value);

                    var config = StreamConfigs.Clone(streamConfig);
                    foreach (var resolution in resolutionse.Value)
                    {
                        config.Resolution = resolution;
                        var frames = GetFramerateByCondition(streamId, config, mode, aspectRatio, primaryConfig);
                        if (frames == null)
                        {
                            resolutionList.Remove(resolution);
                            continue;
                        }

                        if (Series == "K" && mode == DeviceConstant.CameraMode.Dual)//Because series K has different profile for each framerate
                            continue;

                        if (!frames.Contains(primaryConfig.Framerate))
                            resolutionList.Remove(resolution);
                    }
                    return (resolutionList.Count > 0) ? resolutionList.ToArray() : null;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode mode, AspectRatio aspectRatio)
        {
            if (Framerate.Count == 0 || streamConfig == null) return null;

            foreach (var framerate in Framerate)
            {
                if (framerate.Key.StreamId == streamId && framerate.Key.Mode == mode
                    && framerate.Key.Resolution == streamConfig.Resolution && framerate.Key.Compression == streamConfig.Compression && framerate.Key.AspectRatio == aspectRatio)
                {
                    return framerate.Value;
                }
            }
            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, CameraMode mode, AspectRatio aspectRatio, StreamConfig primaryConfig)
        {
            if (Framerate.Count == 0 || streamConfig == null) return null;

            foreach (var framerate in Framerate)
            {
                if (framerate.Key.StreamId == streamId && framerate.Key.Mode == mode
                    && framerate.Key.Resolution == streamConfig.Resolution && framerate.Key.Compression == streamConfig.Compression
                    && framerate.Key.PrimaryResolution == primaryConfig.Resolution && framerate.Key.PrimaryCompression == primaryConfig.Compression && framerate.Key.AspectRatio == aspectRatio
                     )
                {
                    //Because series K has different profile for each framerate
                    if (Series == "K" && mode == DeviceConstant.CameraMode.Dual && !framerate.Key.PrimaryFramerates.Contains(primaryConfig.Framerate))
                        continue;
                    return framerate.Value;
                }
            }
            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig)
        {
            if (Bitrate.Count == 0 || streamConfig == null) return null;

            foreach (var bitratese in Bitrate)
            {
                if (bitratese.Key.StreamId == streamId && bitratese.Key.Compression == streamConfig.Compression)
                {
                    return bitratese.Value;
                }
            }

            return null;
        }
    }

    public class ACTiCompressionCondition
    {
        public Compression PrimaryCompression;
        public UInt16 StreamId = 1;
        public CameraMode Mode;
        public List<UInt16> PrimaryFramerates = new List<UInt16>();
    }

    public class ACTiResolutionCondition : ACTiCompressionCondition
    {
        public Resolution PrimaryResolution;
        public Compression Compression;
        public AspectRatio AspectRatio;
    }

    public class ACTiFramerateCondition : ACTiResolutionCondition
    {
        public Resolution Resolution;
    }

    public class ACTiBitrateCondition
    {
        public Compression Compression;
        public UInt16 StreamId = 1;
    }

    public partial class ParseCameraModel
    {
        public static void ParseACTi(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new ACTiCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyACTiProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");
            var profileModes = node.SelectNodes("ProfileMode");
            if (profileModes == null || profileModes.Count == 0) return;

            cameraModel.CameraMode.Clear();
            foreach (XmlElement profileModeNode in profileModes)
            {
                var profiles = profileModeNode.SelectNodes("Profile");
                if (profiles == null || profiles.Count == 0) continue;

                String mode = profileModeNode.GetAttribute("value");
                CameraMode cameraMode = CameraMode.Single;
                switch (mode)
                {
                    case "1":
                        cameraMode = CameraMode.Single;
                        break;

                    case "2":
                        cameraMode = CameraMode.Dual;
                        break;

                    case "4":
                        cameraMode = CameraMode.FourVga;
                        break;

                    case "6":
                        cameraMode = CameraMode.SixVga;
                        break;
                }

                if (!cameraModel.CameraMode.Contains(cameraMode))
                    cameraModel.CameraMode.Add(cameraMode);

                var deviceMountType = Xml.GetFirstElementByTagName(node, "DeviceMountType");
                if(deviceMountType != null)
                {
                    var moutTypes = Array.ConvertAll(deviceMountType.GetAttribute("value").Split(','), DeviceMountTypes.ToIndex);
                    foreach (DeviceMountType deviceMountTypeValue in moutTypes)
                    {
                        if (!cameraModel.DeviceMountType.Contains(deviceMountTypeValue))
                            cameraModel.DeviceMountType.Add(deviceMountTypeValue);
                    }
                }
                
                foreach (XmlElement profileNode in profiles)
                {
                    var ids = Array.ConvertAll(profileNode.GetAttribute("id").Split(','), Convert.ToUInt16);
                    foreach (var id in ids)
                    {
                        ParseProfile(cameraModel, profileNode, cameraMode, id);
                    }
                }
            }

            cameraModel.LensType = new List<String> { "KCM3911", "KCM7911", "B54", "B55", "B56", "E96", "E919", "E921", "E923", "I51", "I71", "E98", "E15", "E16", "E925", "E927", "E929", "I73" };
        }

        private static void ParseProfile(ACTiCameraModel cameraModel, XmlElement profileNode, CameraMode mode, UInt16 streamId)
        {
            if (profileNode.InnerXml == "") return;

            var compressionNodes = profileNode.SelectNodes("Compression");
            if (compressionNodes == null || compressionNodes.Count == 0) return;

            var compressions = new List<Compression>();

            var compressCondition = new ACTiCompressionCondition
            {
                StreamId = streamId,
                Mode = mode
            };

            foreach (XmlElement compressionNode in compressionNodes)
            {
                String compressionStr = compressionNode.GetAttribute("codes");
                if (compressionStr == "") continue;
                var compressionsList = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);
                compressions.AddRange(compressionsList);
                var aspectRatioNodes = compressionNode.SelectNodes("AspectRatio");

                if (aspectRatioNodes != null && aspectRatioNodes.Count > 0)
                {
                    foreach (XmlElement aspectRatioNode in aspectRatioNodes)
                    {
                        var aspectRatios = Array.ConvertAll(aspectRatioNode.GetAttribute("value").Split(','), AspectRatios.ToIndex);

                        foreach (AspectRatio aspectRatio in aspectRatios)
                        {
                            ParseACTiCompression(cameraModel, aspectRatioNode, mode, compressionsList, streamId, aspectRatio);

                            if (!cameraModel.AspectRatio.Contains(aspectRatio) && aspectRatio != AspectRatio.NonSpecific)
                                cameraModel.AspectRatio.Add(aspectRatio);
                        }
                    }
                }
                else
                {
                    ParseACTiCompression(cameraModel, compressionNode, mode, compressionsList, streamId, AspectRatio.NonSpecific);
                }
            }

            var result = cameraModel.GetCompressionByCondition(streamId, mode, Compression.Off);
            if (result == null)
            {
                compressions.Sort();
                cameraModel.Compression.Add(compressCondition, compressions.ToArray());
            }
            else
            {
                var targetKey = (from compressionse in cameraModel.Compression
                                                      where compressionse.Key.StreamId == 1
                                                      select compressionse.Key).FirstOrDefault();
                if (targetKey != null)
                {
                    List<Compression> compressionList = null;
                    foreach (var compression in compressions)
                    {
                        if (!result.Contains(compression))
                        {
                            if (compressionList == null)
                                compressionList = new List<Compression>(result);
                            compressionList.Add(compression);
                        }
                    }
                    if (compressionList != null)
                    {
                        compressionList.Sort();
                        cameraModel.Compression[targetKey] = compressionList.ToArray();
                    }
                }
            }
        }

        private static void ParseACTiCompression(ACTiCameraModel cameraModel, XmlElement node, CameraMode mode, Compression[] compressionsList, UInt16 streamId, AspectRatio aspectRatio)
        {
            foreach (var compression in compressionsList)
            {
                //compressions.Add(compression);

                var condition = new ACTiResolutionCondition
                {
                    Compression = compression,
                    StreamId = streamId,
                    Mode = mode,
                    AspectRatio = aspectRatio
                };

                var bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");

                if (bitrateStr != "")
                {
                    ACTiBitrateCondition targetKey = (from compressionse in cameraModel.Bitrate
                                                      where (compressionse.Key.StreamId == streamId && compressionse.Key.Compression == compression)
                                                      select compressionse.Key).FirstOrDefault();

                    Bitrate[] bitrateArray = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

                    if (targetKey != null)
                    {
                        var bitrateValue = cameraModel.Bitrate[targetKey].ToList();
                        foreach (var bitrate in bitrateArray)
                        {
                            if (!bitrateValue.Contains(bitrate))
                                bitrateValue.Add(bitrate);
                        }
                        bitrateValue.Sort();

                        cameraModel.Bitrate[targetKey] = bitrateValue.ToArray();
                    }
                    else
                    {
                        cameraModel.Bitrate.Add(new ACTiBitrateCondition
                        {
                            Compression = compression,
                            StreamId = streamId
                        }, bitrateArray);
                    }
                }
                //----------------------------
                var resolutionNodes = node.SelectNodes("Resolution");
                if (resolutionNodes == null) return;

                var resolutions = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArr = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);

                    if (mode == CameraMode.Single || mode == CameraMode.FourVga || mode == CameraMode.SixVga)
                    {
                        var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                        foreach (Resolution resolution in resolutionArr)
                        {
                            if (resolutions.Contains(resolution)) continue;

                            resolutions.Add(resolution);

                            if (framerateStr == "") continue;
                            cameraModel.Framerate.Add(new ACTiFramerateCondition
                            {
                                Resolution = resolution,
                                Compression = compression,
                                StreamId = streamId,
                                Mode = mode,
                                AspectRatio = aspectRatio
                            }, Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16));
                        }
                    }
                    else
                    {
                        var profileNode2 = Xml.GetFirstElementByTagName(resolutionNode, "Profile");
                        if (profileNode2 == null) continue;

                        var compressionNodes2 = profileNode2.SelectNodes("Compression");
                        if (compressionNodes2 == null || compressionNodes2.Count == 0) return;

                        foreach (Resolution resolution in resolutionArr)
                        {
                            if (resolutions.Contains(resolution)) continue;

                            resolutions.Add(resolution);

                            var framerates = new List<UInt16>();

                            //Series E has different framerate for each resolution.
                            //Because series K has different profile for each framerate
                            var collectFrameRates = new List<UInt16>();
                            var frameNodes = resolutionNode.SelectNodes("FrameRate");
                            if (frameNodes != null)
                            {
                                foreach (XmlNode frameNode in frameNodes)
                                {
                                    var framerateStr = String.Empty;
                                    var profileNode = frameNode.SelectNodes("Profile");
                                    if (profileNode != null && profileNode.Count > 0)//Because series K has different profile for each framerate
                                    {
                                        if (frameNode.Attributes != null)
                                            framerateStr = String.IsNullOrEmpty(frameNode.Attributes["value"].Value) ? String.Empty : frameNode.Attributes["value"].Value;

                                        var profileCompressionNodes = profileNode[0].SelectNodes("Compression");
                                        framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16).ToList();

                                        if (profileCompressionNodes != null)
                                            foreach (XmlElement profileCompressionNode in profileCompressionNodes)
                                                ParseProfile2(cameraModel, profileCompressionNode, resolution, compression, framerates, mode, aspectRatio);
                                    }
                                    else//Series E has different framerate for each resolution.
                                    {
                                        framerateStr = frameNode.InnerText;
                                    }

                                    if (!String.IsNullOrEmpty(framerateStr))
                                    {
                                        framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16).ToList();
                                        collectFrameRates.AddRange(framerates);
                                        //cameraModel.Framerate.Add(new ACTiFramerateCondition
                                        //{
                                        //    Resolution = resolution,
                                        //    Compression = compression,
                                        //    StreamId = streamId,
                                        //    Mode = mode,
                                        //    AspectRatio = aspectRatio
                                        //}, framerates.ToArray());
                                    }
                                }

                                if (collectFrameRates.Count > 0)
                                {
                                    collectFrameRates.Sort();
                                    cameraModel.Framerate.Add(new ACTiFramerateCondition
                                    {
                                        Resolution = resolution,
                                        Compression = compression,
                                        StreamId = streamId,
                                        Mode = mode,
                                        AspectRatio = aspectRatio
                                    }, collectFrameRates.ToArray());
                                }
                            }

                            if (cameraModel.Series == "K") continue;

                            foreach (XmlElement compressionNode2 in compressionNodes2)
                            {
                                ParseProfile2(cameraModel, compressionNode2, resolution, compression, framerates, mode, aspectRatio);
                            }
                        }
                    }
                }
                resolutions.Sort();
                cameraModel.Resolution.Add(condition, resolutions.ToArray());
            }
        }

        private static void ParseProfile2(ACTiCameraModel cameraModel, XmlElement compressionNode, Resolution primaryResolution, Compression primaryCompression, List<UInt16> primaryFramrates, CameraMode mode, AspectRatio aspectRatio)
        {
            var streamConfig = new StreamConfig { Resolution = primaryResolution, Compression = primaryCompression };
            var compressions = Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex);

            var bitrateStr = Xml.GetFirstElementValueByTagName(compressionNode, "Bitrate");

            if (bitrateStr != "")
            {
                Bitrate[] bitrateArray = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);
                foreach (var compression in compressions)
                {
                    var targetKey = (from compressionse in cameraModel.Bitrate
                                                      where (compressionse.Key.StreamId == 2 && compressionse.Key.Compression == compression)
                                                      select compressionse.Key).FirstOrDefault();

                    if (targetKey != null)
                    {
                        var bitrateValue = cameraModel.Bitrate[targetKey].ToList();
                        foreach (var bitrate in bitrateArray)
                        {
                            if (!bitrateValue.Contains(bitrate))
                                bitrateValue.Add(bitrate);
                        }
                        bitrateValue.Sort();

                        cameraModel.Bitrate[targetKey] = bitrateValue.ToArray();
                    }
                    else
                    {
                        cameraModel.Bitrate.Add(new ACTiBitrateCondition
                        {
                            Compression = compression,
                            StreamId = 2
                        }, bitrateArray);
                    }
                }
            }
            //-------------------------
            var result = cameraModel.GetCompressionByCondition(2, mode, primaryCompression);
            if (result == null || cameraModel.Series == "K")
            {
                cameraModel.Compression.Add(new ACTiCompressionCondition
                {
                    PrimaryCompression = primaryCompression,
                    StreamId = 2,
                    Mode = mode,
                    PrimaryFramerates = primaryFramrates
                }, compressions.ToArray());
            }
            else
            {
                var targetKey = (from compressionse in cameraModel.Compression
                                                      where compressionse.Key.StreamId == 2 && compressionse.Key.PrimaryCompression == primaryCompression
                                                      select compressionse.Key).FirstOrDefault();
                if (targetKey != null)
                {
                    List<Compression> compressionList = null;
                    foreach (var compression in compressions)
                    {
                        if (!result.Contains(compression))
                        {
                            if (compressionList == null)
                                compressionList = new List<Compression>(result);
                            compressionList.Add(compression);
                        }
                    }
                    if (compressionList != null)
                    {
                        compressionList.Sort();
                        cameraModel.Compression[targetKey] = compressionList.ToArray();
                    }

                    foreach (UInt16 primaryFramrate in primaryFramrates)
                    {
                        if(!targetKey.PrimaryFramerates.Contains(primaryFramrate))
                            targetKey.PrimaryFramerates.Add(primaryFramrate);
                    }
                    
                }
            }

            var resolutionNodes = compressionNode.SelectNodes("Resolution");
            if (resolutionNodes == null) return;

            foreach (var compression in compressions)
            {
                var resolutions = new List<Resolution>();
                foreach (XmlElement resolutionNode in resolutionNodes)
                {
                    var resolutionArr = Array.ConvertAll(resolutionNode.GetAttribute("value").Split(','), Resolutions.ToIndex);

                    var framerateStr = Xml.GetFirstElementValueByTagName(resolutionNode, "FrameRate");

                    if (framerateStr == "") continue;
                    foreach (Resolution resolution in resolutionArr)
                    {
                        if (resolutions.Contains(resolution)) continue;

                        resolutions.Add(resolution);

                        var framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);

                        cameraModel.Framerate.Add(new ACTiFramerateCondition
                        {
                            PrimaryCompression = primaryCompression,
                            PrimaryResolution = primaryResolution,
                            PrimaryFramerates = primaryFramrates,
                            Compression = compression,
                            Resolution = resolution,
                            Mode = CameraMode.Dual,
                            StreamId = 2,
                            AspectRatio = aspectRatio
                        }, framerates);

                        //Because series K has different profile for each framerate
                        if (cameraModel.Series == "K") continue;
                        var primaryFrame = cameraModel.GetFramerateByCondition(1, streamConfig, mode, aspectRatio);
                        if (primaryFrame == null)
                        {
                            cameraModel.Framerate.Add(
                                new ACTiFramerateCondition
                                {
                                    StreamId = 1,
                                    Resolution = primaryResolution,
                                    Compression = primaryCompression,
                                    Mode = CameraMode.Dual,
                                    AspectRatio = aspectRatio
                                }, framerates);
                        }
                        else
                        {
                            var targetKey = (from framerate in cameraModel.Framerate
                                                                  where ((framerate.Key.StreamId == 1 && framerate.Key.Resolution == primaryResolution) && framerate.Key.Compression == primaryCompression) && framerate.Key.Mode == CameraMode.Dual
                                                                  select framerate.Key).FirstOrDefault();
                            
                            if (targetKey == null) return;

                            

                            List<UInt16> primaryFrameList = null;
                            foreach (UInt16 framerate in framerates)
                            {
                                if (primaryFrame.Contains(framerate)) continue;
                                if (primaryFrameList == null)
                                    primaryFrameList = primaryFrame.ToList();
                                primaryFrameList.Add(framerate);

                            }

                            if (primaryFrameList != null)
                            {
                                primaryFrameList.Sort();
                                cameraModel.Framerate[targetKey] = primaryFrameList.ToArray();
                            }
                        }
                    }
                }

                resolutions.Sort();
                cameraModel.Resolution.Add(new ACTiResolutionCondition
                {
                    PrimaryCompression = primaryCompression,
                    PrimaryResolution = primaryResolution,
                    PrimaryFramerates = primaryFramrates,
                    Compression = compression,
                    StreamId = 2,
                    Mode = CameraMode.Dual,
                    AspectRatio = aspectRatio
                }, resolutions.ToArray());
            }
        }

        private static void CopyACTiProfile(XmlElement node, ACTiCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            ACTiCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (ACTiCameraModel)mode;
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
            cameraModel.AspectRatio = copyFrom.AspectRatio;
            cameraModel.DeviceMountType = copyFrom.DeviceMountType;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }
    }
}
