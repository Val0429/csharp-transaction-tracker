using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public class VIVOTEKCameraModel : CameraModel
    {
        public List<PowerFrequency> PowerFrequency = new List<PowerFrequency>
        {
            DeviceConstant.PowerFrequency.NonSpecific
        };

        public List<SensorMode> SensorMode = new List<SensorMode>();
        public List<TvStandard> TvStandard = new List<TvStandard>();
        public List<DeviceMountType> DeviceMountType = new List<DeviceMountType>();
        public Dictionary<VIVOTEKCompressionCondition, Compression[]> Compression = new Dictionary<VIVOTEKCompressionCondition, Compression[]>();
        public Dictionary<VIVOTEKResolutionCondition, Resolution[]> Resolution = new Dictionary<VIVOTEKResolutionCondition, Resolution[]>();
        public Dictionary<VIVOTEKStreamCondition, UInt16[]> Framerate = new Dictionary<VIVOTEKStreamCondition, UInt16[]>();
        public Dictionary<VIVOTEKStreamCondition, Bitrate[]> Bitrate = new Dictionary<VIVOTEKStreamCondition, Bitrate[]>();
        public Dictionary<SensorMode, String> ViewingWindows = new Dictionary<SensorMode, String>();

        public Compression[] GetCompressionByCondition(UInt16 streamId, ConnectionProtocol connectionProtocol, TvStandard tvStandard, SensorMode sensorMode)
        {
            if (Compression.Count == 0) return null;

            foreach (var compressionse in Compression)
            {
                if (compressionse.Key.StreamId == streamId && compressionse.Key.SensorMode == sensorMode && compressionse.Key.TvStandard == tvStandard)
                {
                    if (Model == "PZ61X2" || Model == "PZ6112")
                    {
                        return compressionse.Value;
                    }

                    if (connectionProtocol == DeviceConstant.ConnectionProtocol.Http)
                    {
                        if (compressionse.Value.Contains(DeviceConstant.Compression.Mjpeg))
                        {
                            return new[] { DeviceConstant.Compression.Mjpeg };
                        }

                        return sensorMode == DeviceConstant.SensorMode.Computer ||
                               sensorMode == DeviceConstant.SensorMode.Mobile
                                   ? compressionse.Value
                                   : null;
                    }

                    if (Model == "IP8372" || Model == "IP8172" || Model == "IP8172P" || Model == "SD7151" || Model == "PZ71X1" || Model == "PZ7121" || Model == "IP7133")
                    {
                        var newCompressions = new List<Compression>();
                        foreach (Compression item in compressionse.Value)
                        {
                            if(item != DeviceConstant.Compression.Mjpeg) newCompressions.Add(item);
                        }

                        return newCompressions.ToArray();
                    }

                    return compressionse.Value;
                }
            }

            return null;
        }

        public Dewarp[] GetDewarpModeByCondition(UInt16 streamId, Compression compression, TvStandard tvStandard, SensorMode sensorMode, DeviceMountType deviceMountType)
        {
            if (Resolution.Count == 0) return null;

            var result = new List<Dewarp>();

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.Compression == compression && resolutionse.Key.SensorMode == sensorMode && resolutionse.Key.StreamId == streamId && resolutionse.Key.TvStandard == tvStandard && resolutionse.Key.DeviceMountType == deviceMountType)
                {
                    if(!result.Contains(resolutionse.Key.Dewarp))
                        result.Add(resolutionse.Key.Dewarp);
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        public Resolution[] GetResolutionByCondition(UInt16 streamId, Compression compression, TvStandard tvStandard, SensorMode sensorMode, DeviceMountType deviceMountType, Dewarp dewarp)
        {
            if (Resolution.Count == 0) return null;

            if((Type == "fisheye" && streamId > 2) || sensorMode != DeviceConstant.SensorMode.Fisheye)
            {
                deviceMountType = DeviceConstant.DeviceMountType.NonSpecific;
                dewarp = Dewarp.NonSpecific;
            }

            foreach (var resolutionse in Resolution)
            {
                if (resolutionse.Key.Compression == compression && resolutionse.Key.SensorMode == sensorMode && resolutionse.Key.StreamId == streamId && resolutionse.Key.TvStandard == tvStandard && 
                    resolutionse.Key.DeviceMountType == deviceMountType && resolutionse.Key.Dewarp == dewarp)
                {
                    return resolutionse.Value;
                }
            }

            return null;
        }

        public UInt16[] GetFramerateByCondition(UInt16 streamId, StreamConfig streamConfig, TvStandard tvStandard, SensorMode sensorMode, PowerFrequency powerFrequency)
        {
            if (Framerate.Count == 0) return null;

            foreach (var framerates in Framerate)
            {
                if (framerates.Key.Compression == streamConfig.Compression && framerates.Key.Resolution == streamConfig.Resolution && framerates.Key.SensorMode == sensorMode
                     && framerates.Key.TvStandard == tvStandard && framerates.Key.PowerFrequency == powerFrequency && framerates.Key.StreamId == streamId)
                {
                    return framerates.Value;
                }
            }

            return null;
        }

        public Bitrate[] GetBitrateByCondition(UInt16 streamId, StreamConfig streamConfig, SensorMode sensorMode)
        {
            if (Bitrate.Count == 0) return null;

            foreach (var bitrates in Bitrate)
            {
                if (bitrates.Key.Compression == streamConfig.Compression && bitrates.Key.Resolution == streamConfig.Resolution && bitrates.Key.SensorMode == sensorMode
                    && bitrates.Key.StreamId == streamId)
                {
                    return bitrates.Value;
                }
            }

            return null;
        }
    }

    public class VIVOTEKCompressionCondition
    {
        public UInt16 StreamId = 1;
        public SensorMode SensorMode = SensorMode.NonSpecific;
        public TvStandard TvStandard = TvStandard.NonSpecific;
    }

    public class VIVOTEKResolutionCondition: VIVOTEKCompressionCondition
    {
        public Dewarp Dewarp = Dewarp.NonSpecific;
        public DeviceMountType DeviceMountType = DeviceMountType.NonSpecific;
        public Compression Compression = Compression.Off;
    }

    public class VIVOTEKStreamCondition : VIVOTEKResolutionCondition
    {
        public Resolution Resolution = Resolution.NA;
        public PowerFrequency PowerFrequency = PowerFrequency.NonSpecific;
    }

    public partial class ParseCameraModel
    {
        public static void ParseVIVOTEK(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new VIVOTEKCameraModel();
            String sameAs = Xml.GetFirstElementValueByTagName(node, "SameAs");
            if (!String.Equals(sameAs, ""))
            {
                CopyVIVOTEKProfile(node, cameraModel, sameAs, list);
                return;
            }

            if (!ParseStandardCameraModel(cameraModel, node, list)) return;
            cameraModel.CameraMode.Clear();

            cameraModel.Series = Xml.GetFirstElementValueByTagName(node, "Series");

            var viewingWindows = node.GetElementsByTagName("ViewingWindow");
            cameraModel.ViewingWindows.Clear();
            foreach (XmlElement viewingWindow in viewingWindows)
            {
                SensorMode sensor = SensorModes.ToIndex(viewingWindow.GetAttribute("mode"));
                cameraModel.ViewingWindows.Add(sensor, viewingWindow.InnerText);
            }

            cameraModel.TvStandard.Clear();
            //cameraModel.ViewingWindow = Xml.GetFirstElementsValueByTagName(node, "ViewingWindow");
            var tvStandards = node.GetElementsByTagName("TVStandard");
            if (tvStandards.Count == 0)
            {
                cameraModel.TvStandard.Add(TvStandard.NonSpecific);
                ParseVIVOTEKProfile(cameraModel, node, TvStandard.NonSpecific);
            }
            else
            {
                foreach (XmlElement tvStandardNode in tvStandards)
                {
                    var tvStandard = TvStandards.ToIndex(tvStandardNode.GetAttribute("value"));

                    if (!cameraModel.TvStandard.Contains(tvStandard))
                        cameraModel.TvStandard.Add(tvStandard);

                    ParseVIVOTEKProfile(cameraModel, tvStandardNode, tvStandard);
                }
            }

            if (cameraModel.PowerFrequency.Count > 1)
            {
                cameraModel.PowerFrequency.Remove(PowerFrequency.NonSpecific);
                cameraModel.PowerFrequency.Sort();
            }
        }

        private static void CopyVIVOTEKProfile(XmlElement node, VIVOTEKCameraModel cameraModel, String sameAs, List<CameraModel> list)
        {
            VIVOTEKCameraModel copyFrom = null;
            foreach (var mode in list)
            {
                if (!String.Equals(mode.Model, sameAs)) continue;
                copyFrom = (VIVOTEKCameraModel)mode;
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

            var alias = Xml.GetFirstElementValueByTagName(node, "Alias");
            cameraModel.Alias = (String.Equals(alias, "") ? model : alias);

            cameraModel.Series = copyFrom.Series;
            //cameraModel.ViewingWindow = copyFrom.ViewingWindow;
            cameraModel.ViewingWindows = copyFrom.ViewingWindows;
            cameraModel.DeviceMountType = copyFrom.DeviceMountType;
            cameraModel.ConnectionProtocol = copyFrom.ConnectionProtocol;
            cameraModel.Encryption = copyFrom.Encryption;
            cameraModel.TvStandard = copyFrom.TvStandard;
            cameraModel.CameraMode = copyFrom.CameraMode;
            cameraModel.Compression = copyFrom.Compression;
            cameraModel.Resolution = copyFrom.Resolution;
            cameraModel.Framerate = copyFrom.Framerate;
            cameraModel.Bitrate = copyFrom.Bitrate;
            cameraModel.PowerFrequency = copyFrom.PowerFrequency;
            cameraModel.SensorMode = copyFrom.SensorMode;
            SameAsCameraModel(cameraModel, node, copyFrom);

            cameraModel.NumberOfChannel = copyFrom.NumberOfChannel;
            cameraModel.NumberOfMotion = copyFrom.NumberOfMotion;
        }

        private static void ParseVIVOTEKProfile(VIVOTEKCameraModel cameraModel, XmlElement node, TvStandard tvStandard)
        {
            var profileModes = node.GetElementsByTagName("ProfileMode");
            if (profileModes.Count == 0) return;

            XmlElement profileModeNode = (XmlElement)profileModes[0];

            var profiles = profileModeNode.SelectNodes("Profile");
            if (profiles == null || profiles.Count == 0) return;

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
            }

            if (!cameraModel.CameraMode.Contains(cameraMode))
                cameraModel.CameraMode.Add(cameraMode);

            foreach (XmlElement profileNode in profiles)
            {
                ParseVIVOTEKSensorMode(cameraModel, profileNode, tvStandard);
            }
        }

        private static void ParseVIVOTEKSensorMode(VIVOTEKCameraModel cameraModel, XmlElement node, TvStandard tvStandard)
        {
            var ids = Array.ConvertAll(node.GetAttribute("id").Split(','), Convert.ToUInt16);

            foreach (var id in ids)
            {
                var sensorModes = node.GetElementsByTagName("SensorMode");

                if (sensorModes.Count == 0)//no SensorMode tag, just parse Resolution 
                {
                    var compressions = node.GetElementsByTagName("Compression");
                    if (compressions.Count > 0)
                    {
                        var compressionTemp = new List<Compression>();
                        foreach (XmlElement compressionNode in compressions)
                        {
                            compressionTemp.AddRange(ParseVIVOTEKCompression(id, node, compressionNode, SensorMode.NonSpecific, tvStandard, cameraModel));
                        }
                        var condition = new VIVOTEKCompressionCondition { StreamId = id, SensorMode = SensorMode.NonSpecific, TvStandard = tvStandard};
                        cameraModel.Compression.Add(condition, compressionTemp.ToArray());
                    }
                }

                foreach (XmlElement sensorMode in sensorModes)
                {
                    SensorMode sensor = SensorModes.ToIndex(sensorMode.GetAttribute("value"));
  
                    if (!cameraModel.SensorMode.Contains(sensor))
                        cameraModel.SensorMode.Add(sensor);

                    var compressions = sensorMode.GetElementsByTagName("Compression");
                    if (compressions.Count > 0)
                    {
                        var compressionTemp = new List<Compression>();
                        foreach (XmlElement compressionNode in compressions)
                        {
                            compressionTemp.AddRange(ParseVIVOTEKCompression(id, sensorMode, compressionNode, sensor, tvStandard, cameraModel));
                        }
                        var condition = new VIVOTEKCompressionCondition { StreamId = id, SensorMode = sensor, TvStandard = tvStandard};
                        cameraModel.Compression.Add(condition, compressionTemp.ToArray());
                    }
                }

                //Compression[] compressions = compressionArr.ToArray();
                

                //var compressionNodes = node.SelectNodes("Compression");
                //if(compressionNodes == null) continue;
                //var compressionArr = new List<Compression>();
                //foreach (XmlElement compressionNode in compressionNodes)
                //{
                //    Compression[] compressionTemp;
                //    XmlElement target;
                //    //if resolution is relative with framerate and bitrate , compression will use codec in <Compression codec="H264"> not <Compression>H264<Compression>.
                //    if(!String.IsNullOrEmpty(compressionNode.GetAttribute("codes")))
                //    {
                //        compressionTemp = Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex);
                //        compressionArr.AddRange(compressionTemp);
                //        target = compressionNode;
                //    }
                //    else
                //    {
                //        compressionTemp = Array.ConvertAll(compressionNode.InnerText.Split(','), Compressions.ToIndex);
                //        compressionArr.AddRange(compressionTemp);
                //        target = node;
                //    }

                //    foreach (Compression compression in compressionTemp)
                //    {
                //        ParseVIVOTEKSensorMode(id, target, compression, cameraModel);
                //    }
                //}

                //Compression[] compressions = compressionArr.ToArray();
                //Array.Sort(compressions);
                //var condition = new VIVOTEKCompressionCondition() { StreamId = id };
                //cameraModel.Compression.Add(condition, compressions);
            }
        }

        private static Compression[] ParseVIVOTEKCompression(UInt16 streamId, XmlElement node, XmlElement compressionNode, SensorMode sensorMode, TvStandard tvStandard, VIVOTEKCameraModel cameraModel)
        {
            Compression[] compressions;
            XmlElement target;

            //if resolution is relative with framerate and bitrate , compression will use codec in <Compression codec="H264"> not <Compression>H264<Compression>.
            if (!String.IsNullOrEmpty(compressionNode.GetAttribute("codes")))
            {
                compressions = Array.ConvertAll(compressionNode.GetAttribute("codes").Split(','), Compressions.ToIndex);
                target = compressionNode;
            }
            else
            {
                compressions = Array.ConvertAll(compressionNode.InnerText.Split(','), Compressions.ToIndex);
                target = node;
            }

            Array.Sort(compressions);

            foreach (Compression compression in compressions)
            {
                var dewarps = target.GetElementsByTagName("DewarpMode");

                if (dewarps.Count == 0)//no Dewarp tag, just parse Resolution 
                {
                    var resolutions = target.GetElementsByTagName("Resolution");
                    if (resolutions.Count == 0) continue;
                    var resolutionCondition = new VIVOTEKResolutionCondition { StreamId = streamId, Compression = compression, SensorMode = sensorMode, TvStandard = tvStandard};
                    foreach (XmlElement resolutionNode in resolutions)
                        ParseVIVOTEKResolution(resolutionNode, resolutionCondition, cameraModel);
                }
                else
                {
                    foreach (XmlElement dewarpNode in dewarps)
                    {
                        var dewarp = Dewarps.ToIndex(dewarpNode.GetAttribute("value"));

                        var mountTypeNodes = dewarpNode.GetElementsByTagName("DeviceMountType");
                        if(mountTypeNodes.Count == 0) continue;

                        foreach (XmlElement mountTypeNode in mountTypeNodes)
                        {
                            var moutTypes = Array.ConvertAll(mountTypeNode.GetAttribute("value").Split(','), DeviceMountTypes.ToIndex);

                            foreach (DeviceMountType deviceMountType in moutTypes)
                            {
                                if(!cameraModel.DeviceMountType.Contains(deviceMountType))
                                    cameraModel.DeviceMountType.Add(deviceMountType);

                                var resolutions = dewarpNode.GetElementsByTagName("Resolution");
                                if (resolutions.Count == 0) continue;
                                var resolutionCondition = new VIVOTEKResolutionCondition { StreamId = streamId, Compression = compression, SensorMode = sensorMode, TvStandard = tvStandard, Dewarp = dewarp, DeviceMountType = deviceMountType};
                                foreach (XmlElement resolutionNode in resolutions)
                                    ParseVIVOTEKResolution(resolutionNode, resolutionCondition, cameraModel);
                            }
                        }
                    }
                }
            }

            return compressions;
        }

        //private static void ParseVIVOTEKSensorMode(UInt16 streamId, XmlElement node, Compression compression, VIVOTEKCameraModel cameraModel )
        //{
        //    var sensorModes = node.GetElementsByTagName("SensorMode");
        //    if (sensorModes.Count == 0)//no SensorMode tag, just parse Resolution 
        //    {
        //        var resolutions = node.GetElementsByTagName("Resolution");
        //        if (resolutions.Count == 0) return;
        //        var condition = new VIVOTEKResolutionCondition { StreamId = streamId, Compression = compression };
        //        foreach (XmlElement resolutionNode in resolutions)
        //            ParseVIVOTEKResolution(resolutionNode, condition, cameraModel);
        //        return;
        //    }

        //    foreach (XmlElement sensorMode in sensorModes)
        //    {
        //        SensorMode sensor = SensorModes.ToIndex(sensorMode.GetAttribute("value"));

        //        if (!cameraModel.SensorMode.Contains(sensor))
        //            cameraModel.SensorMode.Add(sensor);

        //        var resolutions = sensorMode.GetElementsByTagName("Resolution");
        //        if (resolutions.Count == 0) continue;

        //        var condition = new VIVOTEKResolutionCondition { StreamId = streamId, Compression = nodeCompression, SensorMode = sensor };
        //        foreach (XmlElement resolutionNode in resolutions)
        //        {
        //            ParseVIVOTEKResolution(resolutionNode, condition, cameraModel);
        //        }

        //    }
        //}

        private static void ParseVIVOTEKResolution(XmlElement node, VIVOTEKResolutionCondition condition, VIVOTEKCameraModel cameraModel)
        {
            var resolutions = Array.ConvertAll(node.GetAttribute("value").Split(','), Resolutions.ToIndex);

            if (cameraModel.Resolution.ContainsKey(condition))
            {
                List<Resolution> temp = new List<Resolution>(cameraModel.Resolution[condition]);
                foreach (var resolution in resolutions)
                {
                    if (!temp.Contains(resolution))
                        temp.Add(resolution);
                }
                temp.Sort();
                cameraModel.Resolution[condition] = temp.ToArray();
            }
            else
            {
                Array.Sort(resolutions);
                cameraModel.Resolution.Add(condition, resolutions);
            }

            var frameRateNodes = node.SelectNodes("FrameRate");
            //String framerateStr = Xml.GetFirstElementsValueByTagName(node, "FrameRate");
            //UInt16[] framerates = Array.ConvertAll(framerateStr.Split(','), Convert.ToUInt16);

            //-----------------------------------

            String bitrateStr = Xml.GetFirstElementValueByTagName(node, "Bitrate");
            var bitrates = new Bitrate[] {};
            if (bitrateStr != "")
            {
                bitrates = Array.ConvertAll(bitrateStr.Split(','), Bitrates.DisplayStringToIndex);

                Array.Sort(bitrates);
            }

            foreach (var resolution in resolutions)
            {
                if (bitrates.Length > 0) cameraModel.Bitrate.Add(new VIVOTEKStreamCondition { StreamId = condition.StreamId, Compression = condition.Compression, Resolution = resolution, SensorMode = condition.SensorMode, TvStandard = condition.TvStandard}, bitrates);
                
               
                if (frameRateNodes == null || frameRateNodes.Count == 0) continue;
                foreach (XmlElement frameRateNode in frameRateNodes)
                {
                    var powerFrequency = PowerFrequencies.ToIndex(frameRateNode.GetAttribute("PowerFrequency"));
                    UInt16[] framerates = Array.ConvertAll(frameRateNode.InnerText.Split(','), Convert.ToUInt16);
                    Array.Sort(framerates);

                    if (!cameraModel.PowerFrequency.Contains(powerFrequency))
                        cameraModel.PowerFrequency.Add(powerFrequency);

                    cameraModel.Framerate.Add(new VIVOTEKStreamCondition { StreamId = condition.StreamId, Compression = condition.Compression, Resolution = resolution, SensorMode = condition.SensorMode, TvStandard = condition.TvStandard, PowerFrequency = powerFrequency }, framerates);
                }
                
            }
        }
    }
}
