using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class AxisCameraModel : CameraModel
    {
        public List<SensorMode> SensorMode = new List<SensorMode>
        {
            DeviceConstant.SensorMode.NonSpecific
        };

        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency>
        {
            DeviceConstant.PowerFrequency.NonSpecific
        };

        public List<AxisStreamCondition> Dewarp = new List<AxisStreamCondition>
        {
            //DeviceConstant.Dewarp.NonSpecific
        };

        public List<DeviceMountType> DeviceMountType = new List<DeviceMountType>
        {
            //DeviceConstant.DeviceMountType.NonSpecific
        };

        public Compression[] Compression;
        public Dictionary<AxisResolutionCondition, Resolution[]> Resolution = new Dictionary<AxisResolutionCondition, Resolution[]>();
        public Dictionary<AxisStreamCondition, UInt16[]> Framerate = new Dictionary<AxisStreamCondition, UInt16[]>();
        public Dictionary<AxisStreamCondition, Bitrate[]> Bitrate = new Dictionary<AxisStreamCondition, Bitrate[]>();

        public Compression[] GetCompressionByCondition(ConnectionProtocol connectionProtocol)
        {
            if (Compression.Length == 0) return null;

            if (Type == "AudioBox")
                return new[] { DeviceConstant.Compression.Off };

            if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
            {
                if (Compression.Contains(DeviceConstant.Compression.Mjpeg))
                    return new[] { DeviceConstant.Compression.Mjpeg };
                return null;
            }

            return Compression.ToArray();
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode, Compression compression, PowerFrequency powerFrequency, DeviceMountType deviceMountType, Dewarp dewarp)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.CameraMode == cameraMode && framerates.Key.SensorMode == sensorMode && framerates.Key.StreamId == streamId &&
                    (framerates.Key.PowerFrequency == DeviceConstant.PowerFrequency.NonSpecific || framerates.Key.PowerFrequency == powerFrequency) && (framerates.Key.DeviceMountType == DeviceConstant.DeviceMountType.NonSpecific || framerates.Key.DeviceMountType == deviceMountType) && (framerates.Key.Dewarp == DeviceConstant.Dewarp.NonSpecific || framerates.Key.Dewarp == dewarp))
                {
                    if (Model == "Q6035" && sensorMode == DeviceConstant.SensorMode.Progressive1080P30 && compression == DeviceConstant.Compression.Mjpeg)
                    {
                        var values = new List<UInt16>(framerates.Value);
                        while (values.Last() > 25)
                        {
                            values.RemoveAt(values.Count - 1);
                        }
                        return values.ToArray();
                    }

                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode, Compression compression, DeviceMountType deviceMountType, Dewarp dewarp)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.CameraMode == cameraMode && bitrates.Key.SensorMode == sensorMode && bitrates.Key.StreamId == streamId && bitrates.Key.DeviceMountType == deviceMountType && bitrates.Key.Dewarp == dewarp)
                {
                    return bitrates.Value;

                    //if (compression != DeviceConstant.Compression.H264)
                    //    return bitrates.Value;

                    //var values = new List<Bitrate>(bitrates.Value);
                    //values.Add(Constant.Bitrate.Bitrate100M);

                    //return values.ToArray();
                }
            }

            return null;
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, CameraMode cameraMode, SensorMode sensorMode, Boolean aspectRatioCorrection, PowerFrequency powerFrequency, DeviceMountType deviceMountType, Dewarp dewarp)
        {
            if (Resolution.Count == 0) return null;

            foreach (var resolutions in Resolution)
            {
                if (resolutions.Key.CameraMode == cameraMode && resolutions.Key.SensorMode == sensorMode && resolutions.Key.StreamId == streamId
                    && resolutions.Key.AspectRatioCorrection == aspectRatioCorrection &&
                    (resolutions.Key.PowerFrequency == DeviceConstant.PowerFrequency.NonSpecific || resolutions.Key.PowerFrequency == powerFrequency) && (resolutions.Key.Dewarp == DeviceConstant.Dewarp.NonSpecific || resolutions.Key.Dewarp == dewarp))
                {
                    return resolutions.Value;
                }
            }

            return null;
        }

        public Dewarp[] GetDewarpModeByCondition(UInt16 streamId, CameraMode cameraMode, DeviceMountType deviceMountType)
        {
            var result = new List<Dewarp>();
            foreach (AxisStreamCondition condition in Dewarp)
            {
                if (condition.CameraMode == cameraMode && condition.StreamId == streamId && condition.DeviceMountType == deviceMountType)
                {
                    if(!result.Contains(condition.Dewarp))
                        result.Add(condition.Dewarp);
                }
            }
            return result.Count == 0 ? null : result.ToArray();
        }
    }

    public class AxisStreamCondition
    {
        public UInt16 StreamId = 1;
        public SensorMode SensorMode = SensorMode.NonSpecific;
        public PowerFrequency PowerFrequency = PowerFrequency.NonSpecific;
        public CameraMode CameraMode = CameraMode.Dual;
        public DeviceMountType DeviceMountType = DeviceMountType.NonSpecific;
        public Dewarp Dewarp = Dewarp.NonSpecific;
    }

    public class AxisResolutionCondition : AxisStreamCondition
    {
        public Boolean AspectRatioCorrection;
    }

    public partial class ParseCameraModel
    {
        public static void ParseAxis(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new AxisCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyAxisProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            String compressionStr = Xml.GetFirstElementValueByTagName(node, "Compression");
            Compression[] compressions = Array.ConvertAll(compressionStr.Split(','), Compressions.ToIndex);

            Array.Sort(compressions);
            cameraModel.Compression = compressions;

            //-----------------------------------

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            var sensorModeNodes = node.GetElementsByTagName("SensorMode");
            if (sensorModeNodes.Count == 0) return;

            cameraModel.CameraMode.Clear();

            foreach (XmlElement sensorModeNode in sensorModeNodes)
            {
                SensorMode[] sensorModes = Array.ConvertAll(sensorModeNode.GetAttribute("value").Split(','), SensorModes.ToIndex);
                foreach (var sensorMode in sensorModes)
                {
                    if (!cameraModel.SensorMode.Contains(sensorMode))
                        cameraModel.SensorMode.Add(sensorMode);

                    var profileModes = sensorModeNode.GetElementsByTagName("ProfileMode");
                    if (profileModes.Count == 0) continue;

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

                            case "3":
                                cameraMode = CameraMode.Triple;
                                break;

                            case "4":
                                cameraMode = CameraMode.Multi;
                                break;

                            case "5":
                                cameraMode = CameraMode.Quad;
                                break;
                        }

                        if (!cameraModel.CameraMode.Contains(cameraMode))
                            cameraModel.CameraMode.Add(cameraMode);

                        foreach (XmlElement profileNode in profiles)
                        {
                            ParseAxisProfile(profileNode, cameraMode, sensorMode, cameraModel);
                        }
                    }
                }
            }

            XmlElement ioPortNode = Xml.GetFirstElementByTagName(node, "IOPort");
            if (ioPortNode != null)
            {
                ParseAxisIOPort(ioPortNode, cameraModel);
            }

            if (cameraModel.SensorMode.Count > 1)
            {
                cameraModel.SensorMode.Remove(SensorMode.NonSpecific);
                cameraModel.SensorMode.Sort();
            }

            if (cameraModel.PowerFrequency.Count > 1)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.Sort();
            }
        }

        private static void CopyAxisProfile(XmlElement node, AxisCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            AxisCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (AxisCameraModel)mode;
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

            XmlElement ioPortNodes = Xml.GetFirstElementByTagName(node, "IOPort");
            if (ioPortNodes != null)
            {
                ParseAxisIOPort(ioPortNodes, cameraModel);
            }
            else
            {
                cameraModel.IOPortConfigurable = copyFrom.IOPortConfigurable;
                cameraModel.IOPorts = copyFrom.IOPorts;
                cameraModel.IOPortSupport = copyFrom.IOPortSupport;
            }

            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            cameraModel.SensorMode = copyFrom.SensorMode;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            cameraModel.Dewarp = copyFrom.Dewarp;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseAxisProfile(XmlElement node, CameraMode cameraMode, SensorMode sensorMode, AxisCameraModel cameraModel)
        {
            var ids = Array.ConvertAll(node.GetAttribute("id").Split(','), Convert.ToUInt16);

            var deviceMountTypes = node.GetElementsByTagName("DeviceMountType");
            var deviceMountTypeList = new Dictionary<DeviceMountType, XmlElement>();

            if (deviceMountTypes.Count == 0)//no DeviceMountType tag, just parse Resolution 
            {
                deviceMountTypeList.Add(DeviceMountType.NonSpecific, node);
            }
            else
            {
                cameraModel.DeviceMountType.Remove(DeviceMountType.NonSpecific);
                foreach (XmlElement deviceMountTypeNode in deviceMountTypes)
                {
                    var values = deviceMountTypeNode.GetAttribute("value").Split(',');
                    foreach (String value in values)
                    {
                        var deviceMountType = DeviceMountTypes.ToIndex(value);
                        deviceMountTypeList.Add(deviceMountType, deviceMountTypeNode);

                        if (!cameraModel.DeviceMountType.Contains(deviceMountType))
                            cameraModel.DeviceMountType.Add(deviceMountType);
                    }
                }

                //foreach (XmlElement dewarpNode in dewarps)
                //{
                //    var dewarp = Dewarps.ToIndex(dewarpNode.GetAttribute("value"));
                //    dewarpList.Add(dewarp, dewarpNode);

                //    if (!cameraModel.Dewarp.Contains(dewarp))
                //        cameraModel.Dewarp.Add(dewarp);
                //}
            }

            foreach (KeyValuePair<DeviceMountType, XmlElement> deviceMountType in deviceMountTypeList)
            {
                var deviceMountTypeNode = deviceMountType.Value;

                var dewarps = deviceMountTypeNode.GetElementsByTagName("DewarpMode");

                var dewarpList = new Dictionary<Dewarp, XmlElement>();

                if (dewarps.Count == 0)//no DewarpMode tag, just parse Resolution 
                {
                    dewarpList.Add(Dewarp.NonSpecific, node);
                }
                else
                {
                    foreach (XmlElement dewarpNode in dewarps)
                    {
                        var dewarp = Dewarps.ToIndex(dewarpNode.GetAttribute("value"));
                        dewarpList.Add(dewarp, dewarpNode);
                    }
                }

                foreach (KeyValuePair<Dewarp, XmlElement> dewarp in dewarpList)
                {

                    var dewarpNode = dewarp.Value;

                    foreach (var id in ids)
                    {
                        cameraModel.Dewarp.Add(new AxisStreamCondition{StreamId = id, CameraMode = cameraMode, DeviceMountType = deviceMountType.Key, Dewarp = dewarp.Key});

                        var condition = new AxisStreamCondition
                        {
                            SensorMode = sensorMode,
                            StreamId = id,
                            CameraMode = cameraMode,
                            DeviceMountType = deviceMountType.Key,
                            Dewarp = dewarp.Key
                        };

                        var resolutionCondition = new AxisResolutionCondition
                        {
                            SensorMode = sensorMode,
                            StreamId = id,
                            CameraMode = cameraMode,
                            DeviceMountType = deviceMountType.Key,
                            Dewarp = dewarp.Key
                        };

                        //-----------------------------------

                        var frameRateNodes = dewarpNode.SelectNodes("FrameRate");
                        if (frameRateNodes == null || frameRateNodes.Count == 0) continue;

                        foreach (XmlElement frameRateNode in frameRateNodes)
                        {
                            var powerFrequency = PowerFrequencies.ToIndex(frameRateNode.GetAttribute("PowerFrequency"));
                            UInt16[] framerates = Array.ConvertAll(frameRateNode.InnerText.Split(','), Convert.ToUInt16);

                            Array.Sort(framerates);
                            if (powerFrequency == PowerFrequency.NonSpecific)
                                cameraModel.Framerate.Add(condition, framerates);
                            else
                            {
                                if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                                    cameraModel.PowerFrequency.Add(powerFrequency);

                                cameraModel.Framerate.Add(new AxisStreamCondition
                                {
                                    SensorMode = sensorMode,
                                    StreamId = id,
                                    PowerFrequency = powerFrequency,
                                    CameraMode = cameraMode,
                                    Dewarp = dewarp.Key
                                }, framerates);
                            }
                        }

                        //-----------------------------------

                        String bitrateStr = Xml.GetFirstElementValueByTagName(dewarpNode, "Bitrate");
                        Bitrate[] bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

                        Array.Sort(bitrates);
                        cameraModel.Bitrate.Add(condition, bitrates);

                        //-----------------------------------

                        if (String.IsNullOrEmpty(cameraModel.Series))
                        {
                            String resolutionStr = Xml.GetFirstElementValueByTagName(dewarpNode, "Resolution");
                            Resolution[] resolutions = Array.ConvertAll(resolutionStr.Split(','), Resolutions.ToIndex);

                            Array.Sort(resolutions);

                            cameraModel.Resolution.Add(resolutionCondition, resolutions);
                        }
                        else
                        {
                            var resolutionNodes = dewarpNode.SelectNodes("Resolution");
                            if (resolutionNodes == null || resolutionNodes.Count == 0) return;

                            foreach (XmlElement resolutionNode in resolutionNodes)
                            {
                                Resolution[] resolutions = Array.ConvertAll(resolutionNode.InnerText.Split(','), Resolutions.ToIndex);

                                Array.Sort(resolutions);

                                var powerFrequency = PowerFrequencies.ToIndex(resolutionNode.GetAttribute("PowerFrequency"));
                                var aspectRatioCorrection = resolutionNode.GetAttribute("arc") == "true";

                                if (powerFrequency == PowerFrequency.NonSpecific && !aspectRatioCorrection)
                                {
                                    cameraModel.Resolution.Add(resolutionCondition, resolutions);
                                }
                                else
                                {
                                    if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                                        cameraModel.PowerFrequency.Add(powerFrequency);

                                    cameraModel.Resolution.Add(new AxisResolutionCondition
                                    {
                                        SensorMode = sensorMode,
                                        StreamId = id,
                                        AspectRatioCorrection = aspectRatioCorrection,
                                        PowerFrequency = powerFrequency,
                                        CameraMode = cameraMode,
                                        Dewarp = dewarp.Key
                                    }, resolutions);
                                }
                            }
                        }
                        //-----------------------------------
                    }

                }
                
            }

            //foreach (KeyValuePair<Dewarp, XmlElement> dewarp in dewarpList)
            //{
            //    var dewarpNode = dewarp.Value;

            //    foreach (var id in ids)
            //    {
            //        var condition = new AxisStreamCondition
            //        {
            //            SensorMode = sensorMode,
            //            StreamId = id,
            //            CameraMode = cameraMode,
            //            Dewarp = dewarp.Key
            //        };

            //        var resolutionCondition = new AxisResolutionCondition
            //        {
            //            SensorMode = sensorMode,
            //            StreamId = id,
            //            CameraMode = cameraMode,
            //            Dewarp = dewarp.Key
            //        };

            //        //-----------------------------------

            //        var frameRateNodes = dewarpNode.SelectNodes("FrameRate");
            //        if (frameRateNodes == null || frameRateNodes.Count == 0) continue;

            //        foreach (XmlElement frameRateNode in frameRateNodes)
            //        {
            //            var powerFrequency = PowerFrequencies.ToIndex(frameRateNode.GetAttribute("PowerFrequency"));
            //            UInt16[] framerates = Array.ConvertAll(frameRateNode.InnerText.Split(','), Convert.ToUInt16);

            //            Array.Sort(framerates);
            //            if (powerFrequency == PowerFrequency.NonSpecific)
            //                cameraModel.Framerate.Add(condition, framerates);
            //            else
            //            {
            //                if (!cameraModel.PowerFrequency.Contains(powerFrequency))
            //                    cameraModel.PowerFrequency.Add(powerFrequency);

            //                cameraModel.Framerate.Add(new AxisStreamCondition
            //                {
            //                    SensorMode = sensorMode,
            //                    StreamId = id,
            //                    PowerFrequency = powerFrequency,
            //                    CameraMode = cameraMode,
            //                    Dewarp = dewarp.Key
            //                }, framerates);
            //            }
            //        }

            //        //-----------------------------------

            //        String bitrateStr = Xml.GetFirstElementValueByTagName(dewarpNode, "Bitrate");
            //        Bitrate[] bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

            //        Array.Sort(bitrates);
            //        cameraModel.Bitrate.Add(condition, bitrates);

            //        //-----------------------------------

            //        if (String.IsNullOrEmpty(cameraModel.Series))
            //        {
            //            String resolutionStr = Xml.GetFirstElementValueByTagName(dewarpNode, "Resolution");
            //            Resolution[] resolutions = Array.ConvertAll(resolutionStr.Split(','), Resolutions.ToIndex);

            //            Array.Sort(resolutions);

            //            cameraModel.Resolution.Add(resolutionCondition, resolutions);
            //        }
            //        else
            //        {
            //            var resolutionNodes = dewarpNode.SelectNodes("Resolution");
            //            if (resolutionNodes == null || resolutionNodes.Count == 0) return;

            //            foreach (XmlElement resolutionNode in resolutionNodes)
            //            {
            //                Resolution[] resolutions = Array.ConvertAll(resolutionNode.InnerText.Split(','), Resolutions.ToIndex);

            //                Array.Sort(resolutions);

            //                var powerFrequency = PowerFrequencies.ToIndex(resolutionNode.GetAttribute("PowerFrequency"));
            //                var aspectRatioCorrection = resolutionNode.GetAttribute("arc") == "true";

            //                if (powerFrequency == PowerFrequency.NonSpecific && !aspectRatioCorrection)
            //                {
            //                    cameraModel.Resolution.Add(resolutionCondition, resolutions);
            //                }
            //                else
            //                {
            //                    if (!cameraModel.PowerFrequency.Contains(powerFrequency))
            //                        cameraModel.PowerFrequency.Add(powerFrequency);

            //                    cameraModel.Resolution.Add(new AxisResolutionCondition
            //                    {
            //                        SensorMode = sensorMode,
            //                        StreamId = id,
            //                        AspectRatioCorrection = aspectRatioCorrection,
            //                        PowerFrequency = powerFrequency,
            //                        CameraMode = cameraMode,
            //                        Dewarp = dewarp.Key
            //                    }, resolutions);
            //                }
            //            }
            //        }
            //        //-----------------------------------
            //    }
            //}
            //--------for end
        }

        private static void ParseAxisIOPort(XmlElement node, AxisCameraModel cameraModel)
        {
            cameraModel.IOPortConfigurable = (node.GetAttribute("configurable") == "true");

            var portNodes = node.SelectNodes("Port");
            if(portNodes == null || portNodes.Count == 0)
                return;

            cameraModel.IOPortSupport = Array.ConvertAll(node.GetAttribute("setting").Split(','), IOPorts.ToIndex);
            //Array.Sort(cameraModel.IOPortSupport);

            foreach (XmlElement portNode in portNodes)
            {
                var id = Convert.ToUInt16(portNode.GetAttribute("id"));
                if(cameraModel.IOPorts.ContainsKey(id)) continue;

                var io = IOPorts.ToIndex(portNode.InnerText);
                if (io == IOPort.NonSpecific || !cameraModel.IOPortSupport.Contains(io)) continue;

                cameraModel.IOPorts.Add(id, io);
            }
        }
    }
}
