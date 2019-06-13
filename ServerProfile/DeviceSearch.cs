using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Constant;
using Device;
using DeviceCab;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String CgiSearchDevice = @"cgi-bin/searchdevice?vendor={VENDOR}";

        private const UInt16 SearchDeviceTimeout = 120;//2 min
        public List<ICamera> Search(String manufacturer)
        {
            //stop support search ALL
            if (String.Equals(manufacturer, "ALL") || String.IsNullOrEmpty(manufacturer))
            {
                return new List<ICamera>();
            }

            var searchResult = new List<ICamera>();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiSearchDevice.Replace("{VENDOR}", manufacturer), Server.Credential, SearchDeviceTimeout);

            if (xmlDoc != null)
            {
                var datas = xmlDoc.GetElementsByTagName("DATA");
                if (datas.Count > 0)
                {
                    foreach (XmlElement node in datas)
                    {
                        ICamera camera = ParseDeviceProfileFromSearchXml(node);
                        if (camera != null)
                            searchResult.Add(camera);
                    }
                }
            }
            if (searchResult.Count > 1)
                searchResult.Sort(SortByIp);

            //flow id as search id number
            UInt16 id = 1;
            foreach (ICamera camera in searchResult)
            {
                camera.Id = id++;
                camera.ReadyState = ReadyState.NotInUse;

                if (camera.Profile.NetworkAddress == "") continue;

                foreach (KeyValuePair<UInt16, IDevice> obj in Server.Device.Devices)
                {
                    if (!(obj.Value is ICamera)) continue;

                    if (((ICamera)obj.Value).Profile.NetworkAddress == camera.Profile.NetworkAddress)
                    {
                        camera.IsInUse = true;
                        break;
                    }
                }
            }
            return searchResult;
        }

        private readonly CultureInfo _enus = new CultureInfo("en-US");
        private ICamera ParseDeviceProfileFromSearchXml(XmlElement node)
        {
            String manufacture = Xml.GetFirstElementValueByTagName(node, "COMPANY");
            String productionId = Xml.GetFirstElementValueByTagName(node, "PRODUCTIONID");
            
            //search result chartar senstive is wrong, convert to upper and compare
            manufacture = manufacture.ToUpper(_enus);
            productionId = productionId.ToUpper(_enus);

            List<CameraModel> models = null;
            foreach (var obj in Server.Device.Manufacture)
            {
                if(obj.Key.ToUpper(_enus) != manufacture) continue;
                models = obj.Value;
                break;
            }

            if (models == null) return null;
            if (models.Count == 0) return null;

            CameraModel model = null;
            if (productionId == "")
            {
                model = models[0];
            }
            else
            {
                foreach (var cameraModel in models)
                {
                    if (!String.Equals(cameraModel.Model.ToUpper(_enus), productionId) && !String.Equals(cameraModel.Alias.ToUpper(_enus), productionId)) continue;
                    model = cameraModel;
                    break;
                }

                //Ignore "-" and fine match device
                if ((String.Equals(manufacture, "ETROVISION")) && model == null)
                {
                    if (productionId.IndexOf('-') > -1)
                    {
                        productionId = productionId.Split('-')[0];

                        foreach (var cameraModel in models)
                        {
                            if (!String.Equals(cameraModel.Model, productionId)) continue;
                            model = cameraModel;
                            break;
                        }
                    }
                }

                if (String.Equals(manufacture, "IPSURVEILLANCE") && String.Equals(productionId, "CAMERA"))
                {
                    //UNKNOWN
                    model = new EtrovisionCameraModel {Manufacture = "IPSurveillance"};
                }
            }

            String hostname = Xml.GetFirstElementValueByTagName(node, "HOSTNAME");//not good for identity

            String wanip = Xml.GetFirstElementValueByTagName(node, "WANIP");
            String lanip = Xml.GetFirstElementValueByTagName(node, "LANIP");
            String tvsyatem = Xml.GetFirstElementValueByTagName(node, "TVSYSTEM");

            var profile = new CameraProfile
            {
                NetworkAddress = String.Equals(lanip, "") ? wanip : lanip
            };

            String httpPort = Xml.GetFirstElementValueByTagName(node, "HTTPPORT");
            if (httpPort != "")
                profile.HttpPort = Convert.ToUInt16(httpPort);

            if (tvsyatem != "")
                profile.TvStandard = (tvsyatem == "NTSC") ? TvStandard.NTSC : TvStandard.PAL;

            String channelId = Xml.GetFirstElementValueByTagName(node, "CHANNELID");
            if(String.IsNullOrEmpty(channelId))
                channelId = Xml.GetFirstElementValueByTagName(node, "CHANNEL");

            var streamConfig = new StreamConfig
            {
                Channel = (channelId == "" || channelId == "0")
                    ? (UInt16)1
                    : Convert.ToUInt16(channelId),
            };

            profile.StreamConfigs.Add(1, streamConfig);

            Boolean setRtspPort = false;
            String rtspPort = Xml.GetFirstElementValueByTagName(node, "RTSPPORT");
            if (rtspPort != "")
            {
                streamConfig.ConnectionPort.Rtsp = Convert.ToUInt16(rtspPort);
                setRtspPort = true;
            }

            String rtspPort1 = Xml.GetFirstElementValueByTagName(node, "RTSPPORT1");
            if (rtspPort1 != "")
            {
                streamConfig.ConnectionPort.Rtsp = Convert.ToUInt16(rtspPort1);
                setRtspPort = true;
                String rtspPort2 = Xml.GetFirstElementValueByTagName(node, "RTSPPORT2");
                if (rtspPort2 != "")
                {
                    StreamConfig streamConfig2 = StreamConfigs.Clone(streamConfig);
                    streamConfig2.ConnectionPort.Rtsp = Convert.ToUInt16(rtspPort2);
                    profile.StreamConfigs.Add(2, streamConfig2);
                }
            }

            String controlPort = Xml.GetFirstElementValueByTagName(node, "CONTROLPORT");
            if (controlPort != "")
                streamConfig.ConnectionPort.Control = Convert.ToUInt16(controlPort);

            String videoPort = Xml.GetFirstElementValueByTagName(node, "VIDEOPORT");
            if (videoPort != "")
                streamConfig.ConnectionPort.Streaming = Convert.ToUInt16(videoPort);

            var camera = new Camera
            {
                Server = Server,
                Profile = profile,
            };

            if (model != null)
            {
                camera.Model = model;
                camera.Mode = model.CameraMode[0];
                camera.Name = String.IsNullOrEmpty(hostname) ? model.Model : hostname;
            }
            else
            {
                camera.Model = new BasicCameraModel
                                   {
                                       Manufacture = @"*" + models[0].Manufacture,
                                       Model = Localization["Data_NotSupport"],
                                   };
                camera.Name = String.IsNullOrEmpty(hostname) ? productionId : hostname;
            }

            SetDeviceCapabilityByModel(camera);

            if (!setRtspPort)
                ProfileChecker.SetDefaultPort(camera, camera.Model);

            return camera;
        }

        public void SetDeviceCapabilityByModel(ICamera camera)
        {
            switch (camera.Mode)
            {
                case CameraMode.Single:
                    camera.Profile.StreamConfigs.Remove(2);
                    camera.Profile.StreamConfigs.Remove(3);
                    camera.Profile.StreamConfigs.Remove(4);
                    camera.Profile.StreamConfigs.Remove(5);
                    camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Dual:
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;

                case CameraMode.Triple:
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    camera.Profile.StreamConfigs.Remove(4);
                    camera.Profile.StreamConfigs.Remove(5);
                    camera.Profile.StreamConfigs.Remove(6);
                    break;

                //case CameraMode.Quad:
                //    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                //        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                //    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                //        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                //    camera.Profile.StreamConfigs.Remove(4);
                //    camera.Profile.StreamConfigs.Remove(5);
                //    camera.Profile.StreamConfigs.Remove(6);
                //    break;

                case CameraMode.Multi:
                case CameraMode.FourVga:
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    camera.Profile.StreamConfigs.Remove(5);
                    camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Five:
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(5))
                        camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.SixVga:
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(5))
                        camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                    if (!camera.Profile.StreamConfigs.ContainsKey(6))
                        camera.Profile.StreamConfigs.Add(6, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                    break;
            }

            if (camera.Model.Model == Localization["Data_NotSupport"])
            {
                foreach (var config in camera.Profile.StreamConfigs)
                {
                    config.Value.Compression = Compression.Off;
                }
                return;
            }

            if (camera.Model.Type == "CaptureCard")
            {
                camera.Profile.CaptureCardConfig = new CaptureCardConfig();
            }

            ProfileChecker.SetDefaultAccountPassword(camera, camera.Model);
            ProfileChecker.SetDefaultProtocol(camera, camera.Model);
            ProfileChecker.SetDefaultMode(camera, camera.Model);
            ProfileChecker.SetDefaultAudioOutPort(camera, camera.Model);
            ProfileChecker.SetDefaultTvStandard(camera, camera.Model);
            ProfileChecker.SetDefaultSensorMode(camera, camera.Model);
            ProfileChecker.SetDefaultPowerFrequency(camera, camera.Model);
            ProfileChecker.SetDefaultAspectRatio(camera, camera.Model);
            ProfileChecker.SetDefaultIOPort(camera, camera.Model);

            foreach (var config in camera.Profile.StreamConfigs)
            {
                ProfileChecker.SetDefaultCompression(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultDewarpMode(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultResolution(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultFramerate(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultBitrate(camera, camera.Model, config.Value, config.Key);
                ProfileChecker.SetDefaultBitrateControl(camera.Model, config.Value);
            }
        }
    }
}