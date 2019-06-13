using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using DeviceConstant;
using Interface;

namespace DeviceCab
{
    public static class ProfileChecker
    {
        public static void SetDefaultAccountPassword(ICamera camera, CameraModel model)
        {
            if (model == null) return;

            camera.Profile.Authentication.Encryption = model.Encryption[0];
            camera.Type = CameraType.Single;
            if (model.Type == "VideoServer") 
                camera.Type = CameraType.VideoServer;

            switch (model.Manufacture)
            {
                case "ACTi":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "123456";
                    break;

                case "Axis":
                    camera.Profile.Authentication.UserName = "root";
                    camera.Profile.Authentication.Password = "";

                    //if (model.Model == "M7016" || model.Model == "P7210") camera.Type = CameraType.VideoServer;
                    break;

                case "VIVOTEK":
                    camera.Profile.Authentication.UserName = "root";
                    camera.Profile.Authentication.Password = "";
                    break;

                case "ArecontVision":
                case "DLink":
                case "GeoVision":
                case "A-MTK":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "";
                    break;

                case "MegaSys":
                case "DivioTec":
                case "Certis":
                    camera.Profile.Authentication.UserName = "Admin";
                    camera.Profile.Authentication.Password = "1234";
                    break;

                case "Messoa":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "1234";
                    break;

                case "HIKVISION":
                case "Panasonic":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "12345";
                    break;

                case "ZeroOne":
                    camera.Profile.Authentication.UserName = "root";
                    camera.Profile.Authentication.Password = "root";
                    break;

                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    camera.Profile.Authentication.UserName = "root";
                    camera.Profile.Authentication.Password = "pass";
                    break;

                case "FITIVISION":
                case "ZAVIO":
                case "Dahua":
                case "GoodWill":
                case "FINE":
                case "Surveon":
                case "Brickcom":
                case "VIGZUL":
                case "PULSE":
                case "SIEMENS":
                case "Kedacom":
                case "inskytec":
                case "Avigilon":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "admin";
                    break;

                case "MOBOTIX":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "meinsm";
                    break;

                case "SAMSUNG":
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "4321";
                    break;

                case "EverFocus":
                    camera.Profile.Authentication.UserName = "user1";
                    camera.Profile.Authentication.Password = "11111111";
                    break;

                case "ONVIF":
                case "Customization":
                    camera.Profile.Authentication.UserName = "";
                    camera.Profile.Authentication.Password = "";
                    break;
                case "iSapSolution":
                    camera.Profile.Authentication.UserName = "Admin";
                    camera.Profile.Authentication.Password = "";
                    break;

                default:
                    camera.Profile.Authentication.UserName = "admin";
                    camera.Profile.Authentication.Password = "123456";
                    break;
            }
        }

        public static void SetDefaultProtocol(ICamera camera, CameraModel model)
        {
            if (model == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                SetDefaultProtocol(camera, model, obj.Value, obj.Key);
            }

            SetDefaultChannelId(camera, model);
            SetDefaultDewarpType(camera);
        }

        public static void SetDefaultProtocol(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            var connectionProtocol = GetConnectionProtocol(camera, model, streamId);
            var orginalConnectionProtocol = streamConfig.ConnectionProtocol;

            if (connectionProtocol.Length > 0)
                streamConfig.ConnectionProtocol = connectionProtocol.First();
            else
                streamConfig.ConnectionProtocol = ConnectionProtocol.NonSpecific;

            //streamConfig.ConnectionProtocol = (ConnectionProtocol)model.ConnectionProtocol.GetValue(0);

            switch (model.Manufacture)
            {
                case "ACTi":
                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.Tcp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "MegaSys":
                case "Avigilon":
                case "DivioTec":
                case "SIEMENS":
                case "HIKVISION":
                case "PULSE":
                case "ZeroOne":
                    if (orginalConnectionProtocol == ConnectionProtocol.RtspOverUdp || orginalConnectionProtocol == ConnectionProtocol.RtspOverTcp || orginalConnectionProtocol == ConnectionProtocol.RtspOverHttp)
                    {
                        streamConfig.ConnectionProtocol = orginalConnectionProtocol;
                        return;
                    }

                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverUdp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "Axis":
                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "Brickcom":
                case "VIGZUL":
                    if (model.Series == "DynaColor")
                    {
                        if (orginalConnectionProtocol == ConnectionProtocol.RtspOverUdp || orginalConnectionProtocol == ConnectionProtocol.RtspOverTcp || orginalConnectionProtocol == ConnectionProtocol.RtspOverHttp)
                        {
                            streamConfig.ConnectionProtocol = orginalConnectionProtocol;
                            return;
                        }
                    }

                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "Surveon":
                case "Certis":
                    if (model.Series == "DynaColor")
                    {
                        if (orginalConnectionProtocol == ConnectionProtocol.RtspOverUdp || orginalConnectionProtocol == ConnectionProtocol.RtspOverTcp || orginalConnectionProtocol == ConnectionProtocol.RtspOverHttp)
                        {
                            streamConfig.ConnectionProtocol = orginalConnectionProtocol;
                            return;
                        }
                    }

                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;
                case "FINE":
                    if (model.Series == "DynaColor")
                    {
                        if (orginalConnectionProtocol == ConnectionProtocol.RtspOverUdp || orginalConnectionProtocol == ConnectionProtocol.RtspOverTcp || orginalConnectionProtocol == ConnectionProtocol.RtspOverHttp)
                        {
                            streamConfig.ConnectionProtocol = orginalConnectionProtocol;
                            return;
                        }
                    }

                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "DLink":
                    if (model.Type == "DynaColor")
                    {
                        if (orginalConnectionProtocol == ConnectionProtocol.RtspOverUdp || orginalConnectionProtocol == ConnectionProtocol.RtspOverTcp || orginalConnectionProtocol == ConnectionProtocol.RtspOverHttp)
                        {
                            streamConfig.ConnectionProtocol = orginalConnectionProtocol;
                            return;
                        }
                    }

                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "Panasonic":
                    if (connectionProtocol.Contains(ConnectionProtocol.RtspOverHttp))
                    {
                        foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                        {
                            if (protocol != ConnectionProtocol.RtspOverHttp) continue;
                            streamConfig.ConnectionProtocol = protocol;
                            return;
                        }
                    }
                    else
                    {
                        foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                        {
                            if (protocol != ConnectionProtocol.Http) continue;
                            streamConfig.ConnectionProtocol = protocol;
                            return;
                        }
                    }
                    
                    break;

                case "ArecontVision":
                case "ONVIF":
                case "Kedacom":
                case "Customization":
                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverUdp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                case "SAMSUNG":
                case "inskytec":
                case "VIVOTEK":
                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.RtspOverTcp) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;

                default:
                    foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
                    {
                        if (protocol != ConnectionProtocol.Http) continue;
                        streamConfig.ConnectionProtocol = protocol;
                        return;
                    }
                    break;
            }

            //Set First ConnectionType
            foreach (ConnectionProtocol protocol in model.ConnectionProtocol)
            {
                streamConfig.ConnectionProtocol = protocol;
                break;
            }
        }

        public static void SetDefaultChannelId(ICamera camera, CameraModel model)
        {
            switch (camera.Mode)
            {
                case CameraMode.SixVga:
                    foreach (var streamConfig in camera.Profile.StreamConfigs)
                        streamConfig.Value.Channel = streamConfig.Key;
                    break;

                case CameraMode.FourVga:
                    switch (camera.Model.Manufacture)
                    {
                        case "ACTi":
                            foreach (var streamConfig in camera.Profile.StreamConfigs)
                                streamConfig.Value.Channel = streamConfig.Key;
                            break;

                        default:
                            foreach (var streamConfig in camera.Profile.StreamConfigs)
                            {
                                if (streamConfig.Value.Channel == 0 || streamConfig.Value.Channel > model.NumberOfChannel)
                                    streamConfig.Value.Channel = 1;
                            }
                            break;
                    }

                    break;

                default:
                    foreach (var streamConfig in camera.Profile.StreamConfigs)
                    {
                        if (streamConfig.Value.Channel == 0 || streamConfig.Value.Channel > model.NumberOfChannel)
                            streamConfig.Value.Channel = 1;
                    }
                    break;
            }
        }

        public static void SetDefaultDewarpType(ICamera camera)
        {
            switch (camera.Model.Manufacture)
            {
                case "ACTi":
                    if (camera.Model.LensType.Contains(camera.Model.Model))
                        camera.Profile.DewarpType = camera.Model.Model;
                    else
                        camera.Profile.DewarpType = "";
                    break;

                default:
                    camera.Profile.DewarpType = "";
                    break;
            }
        }

        public static void SetDefaultCompression(ICamera camera, CameraModel model)
        {
            if (camera.StreamConfig == null || model == null) return;

            if (camera.Profile.StreamConfigs.ContainsKey(1))
            {
                //check primary config first, it connect to sub-stream
                switch (camera.Model.Manufacture)
                {
                    case "ACTi":
                        SetDefaultCompression(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        CheckAvailableSetDefaultResolution(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        break;

                    case "MegaSys":
                    case "Avigilon":
                    case "DivioTec":
                    case "SIEMENS":
                    case "SAMSUNG":
                    case "inskytec":
                    case "HIKVISION":
                    case "PULSE":
                        SetDefaultCompression(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        CheckAvailableSetDefaultResolution(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        CheckAvailableSetDefaultFramerate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        CheckAvailableSetDefaultBitrate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        break;

                    case "Brickcom":
                    case "VIGZUL":
                    case "Surveon":
                    case "Certis":
                    case "FINE":
                        if (camera.Model.Series == "DynaColor")
                        {
                            SetDefaultCompression(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultResolution(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultFramerate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultBitrate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        }
                        break;

                    case "DLink":
                        if (camera.Model.Type == "DynaColor")
                        {
                            SetDefaultCompression(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultResolution(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultFramerate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultBitrate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        }
                        break;

                    case "VIVOTEK":
                        if (camera.Model.Type == "fisheye")
                        {
                            if (model is VIVOTEKCameraModel)
                            {
                                var tmp = model as VIVOTEKCameraModel;
                                if (tmp.DeviceMountType.Count > 0)
                                {
                                    camera.Profile.DeviceMountType = tmp.DeviceMountType[0];
                                    camera.Profile.SensorMode = SensorMode.Fisheye;
                                }
                            }
                            SetDefaultCompression(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultDewarpMode(camera, camera.Model);
                            CheckAvailableSetDefaultResolution(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultFramerate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                            CheckAvailableSetDefaultBitrate(camera, camera.Model, camera.Profile.StreamConfigs[1], 1);
                        }
                        break;

                    case "Axis":
                        if (model is AxisCameraModel)
                        {
                            var tmp = model as AxisCameraModel;
                            if (tmp.DeviceMountType.Count > 0)
                            {
                                camera.Profile.DeviceMountType = tmp.DeviceMountType[0];
                            }
                        }
                        break;
                }
            }

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                SetDefaultCompression(camera, model, obj.Value, obj.Key);
            }
        }

        public static void SetDefaultCompression(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            if (camera.StreamConfig == null || model == null) return;

            IEnumerable<Compression> compressions = GetCompression(camera, model, streamId);

            if (compressions == null)
            {
                streamConfig.Compression = Compression.Off;
                return;
            }

            //If current compression is in support list
            if (compressions.Contains(streamConfig.Compression))
            {
                switch (camera.Mode)
                {
                    case CameraMode.SixVga:
                        if (camera.Mode == CameraMode.SixVga)
                            streamConfig.Compression = camera.Profile.StreamConfigs[1].Compression;
                        return;

                    case CameraMode.FourVga:
                        switch (model.Manufacture)
                        {
                            case "ACTi":
                                streamConfig.Compression = camera.Profile.StreamConfigs[1].Compression;
                                return;
                        }

                        break;
                }
                return;
            }

            //Set compression to first value
            foreach (Compression compression in compressions)
            {
                streamConfig.Compression = compression;
                break;
            }

            switch (model.Manufacture)
            {
                case "Messoa":
                    if (streamConfig.Compression == Compression.H264)
                        return;

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "ACTi":
                case "MegaSys":
                case "Avigilon":
                case "DivioTec":
                case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
                case "HIKVISION":
                case "PULSE":
                case "ZeroOne":
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                case "NEXCOM":
                case "YUAN":
                case "Stretch":
                case "BOSCH":
                case "Axis":
                case "Brickcom":
                case "VIGZUL":
                case "Certis":
                case "FINE":
                case "DLink":
                case "Panasonic":
                case "ArecontVision":
                case "Dahua":
                case "GoodWill":
                    if (streamConfig.Compression == Compression.H264)
                        return;

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.Mpeg4) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "VIVOTEK":
                    if (streamConfig.Compression == Compression.H264 || streamConfig.Compression == Compression.Mpeg4 || streamConfig.Compression == Compression.Svc)
                        return;

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.Mpeg4) continue;

                        streamConfig.Compression = compression;
                        return;
                    }

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.Svc) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "GeoVision":
                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "Surveon":
                    if (model.Series != "DynaColor") return;
                    if (streamConfig.Compression == Compression.H264)
                        return;

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.Mpeg4) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "EverFocus":
                case "A-MTK":
                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.H264) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;

                case "ONVIF":
                case "Kedacom":
                case "Customization":
                    return;

                default:
                    if (streamConfig.Compression == Compression.Mjpeg)
                        return;

                    foreach (Compression compression in compressions)
                    {
                        if (compression != Compression.Mjpeg) continue;

                        streamConfig.Compression = compression;
                        return;
                    }
                    break;
            }
        }

        public static void SetDefaultPort(ICamera camera, CameraModel model)
        {
            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
                SetDefaultPort(camera, model, obj.Value, obj.Key);
        }

        public static void SetDefaultPort(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "ACTi":
                    streamConfig.ConnectionPort.Rtsp = 7070;
                    streamConfig.ConnectionPort.Streaming = 6002;
                    break;

                case "EverFocus":
                    if (streamId != 1 && camera.Profile.StreamConfigs.ContainsKey(1))
                    {
                        streamConfig.ConnectionPort.Rtsp = camera.Profile.StreamConfigs[1].ConnectionPort.Rtsp;
                    }
                    else
                    {
                        streamConfig.ConnectionPort.Rtsp = 554;
                    }
                    streamConfig.ConnectionPort.Streaming = 6002;
                    break;

                case "GeoVision":
                    if (streamId != 1 && camera.Profile.StreamConfigs.ContainsKey(1))
                    {
                        streamConfig.ConnectionPort.Rtsp = camera.Profile.StreamConfigs[1].ConnectionPort.Rtsp;
                    }
                    else
                    {
                        streamConfig.ConnectionPort.Rtsp = 8554;
                    }
                    streamConfig.ConnectionPort.Streaming = 6002;
                    break;

                case "MegaSys":
                case "Avigilon":
                case "DivioTec":
                case "HIKVISION":
                case "PULSE":
                    if (streamId != 1 && camera.Profile.StreamConfigs.ContainsKey(1))
                    {
                        streamConfig.ConnectionPort.Rtsp = camera.Profile.StreamConfigs[1].ConnectionPort.Rtsp;
                    }
                    else
                    {
                        streamConfig.ConnectionPort.Rtsp = 554;
                    }
                    streamConfig.ConnectionPort.Streaming = 8008;
                    break;

                case "SIEMENS":
                //case "SAMSUNG":
                    switch (streamId)
                    {
                        case 1:
                            streamConfig.ConnectionPort.Rtsp = 554;
                            break;

                        case 2:
                            streamConfig.ConnectionPort.Rtsp = 555;
                            break;

                        case 3:
                            streamConfig.ConnectionPort.Rtsp = 556;
                            break;
                    }
                   
                    streamConfig.ConnectionPort.Streaming = 8008;
                    break;

                case "Brickcom":
                case "VIGZUL":
                case "Surveon":
                case "Certis":
                case "FINE":
                    if (camera.Model.Type == "Badge")
                    {
                        streamConfig.ConnectionPort.Rtsp = 8554;
                        streamConfig.ConnectionPort.Streaming = 6002;
                    }
                    else if (camera.Model.Series == "DynaColor")
                    {
                        if (streamId != 1 && camera.Profile.StreamConfigs.ContainsKey(1))
                        {
                            streamConfig.ConnectionPort.Rtsp = camera.Profile.StreamConfigs[1].ConnectionPort.Rtsp;
                        }
                        else
                        {
                            streamConfig.ConnectionPort.Rtsp = 554;
                        }
                        streamConfig.ConnectionPort.Streaming = 8008;
                    }
                    else
                    {
                        streamConfig.ConnectionPort.Rtsp = 554;
                        streamConfig.ConnectionPort.Streaming = 6002;
                    }
                    break;

                case "DLink":
                    if (camera.Model.Type == "DynaColor")
                    {
                        if (streamId != 1 && camera.Profile.StreamConfigs.ContainsKey(1))
                        {
                            streamConfig.ConnectionPort.Rtsp = camera.Profile.StreamConfigs[1].ConnectionPort.Rtsp;
                        }
                        else
                        {
                            streamConfig.ConnectionPort.Rtsp = 554;
                        }
                        streamConfig.ConnectionPort.Streaming = 8008;
                    }
                    else
                    {
                        streamConfig.ConnectionPort.Rtsp = 554;
                        streamConfig.ConnectionPort.Streaming = 6002;
                    }

                    streamConfig.ConnectionPort.VideoIn = 5560;
                    streamConfig.ConnectionPort.AudioIn = 5562;
                    break;

                default:
                    streamConfig.ConnectionPort.Rtsp = 554;
                    streamConfig.ConnectionPort.Streaming = 6002;
                    break;
            }

            streamConfig.ConnectionPort.Control = 6001;
        }

        public static void SetDefaultMulticastNetworkAddress(ICamera camera, CameraModel model)
        {
            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
                SetDefaultMulticastNetworkAddress(camera, model, obj.Value);
        }

        public static void SetDefaultMulticastNetworkAddress(ICamera camera, CameraModel model, StreamConfig streamConfig)
        {
            switch (model.Manufacture)
            {
                case "DLink":
                    streamConfig.MulticastNetworkAddress = "239.128.1.99";
                    break;

                default:
                    streamConfig.MulticastNetworkAddress = String.Empty;
                    break;
            }
        }

        public static void SetDefaultMode(ICamera camera, CameraModel model)
        {
            camera.Mode = model.CameraMode[0];
        }

        public static void SetDefaultAudioOutPort(ICamera camera, CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "Messoa":
                    camera.Profile.AudioOutPort = ((MessoaCameraModel)model).AudioOutPort;
                    break;

                default:
                    camera.Profile.AudioOutPort = 0;
                    break;
            }
        }

        public static void SetDefaultTvStandard(ICamera camera, CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "NEXCOM":
                    camera.Profile.TvStandard = ((NexcomCameraModel)model).TvStandard[0];
                    break;

                case "YUAN":
                    camera.Profile.TvStandard = ((YuanCameraModel)model).TvStandard[0];
                    break;

                case "Stretch":
                    camera.Profile.TvStandard = ((StretchCameraModel)model).TvStandard[0];
                    break;

                case "Messoa":
                    if (camera.Profile.TvStandard == TvStandard.NonSpecific)
                        camera.Profile.TvStandard = ((MessoaCameraModel)model).TvStandard[0];
                    break;

                case "MegaSys":
                    camera.Profile.TvStandard = ((MegaSysCameraModel)model).TvStandard[0];
                    break;

                case "Avigilon":
                    camera.Profile.TvStandard = ((AvigilonCameraModel)model).TvStandard[0];
                    break;

                case "DivioTec":
                    camera.Profile.TvStandard = ((DivioTecCameraModel)model).TvStandard.Count > 0 ? ((DivioTecCameraModel)model).TvStandard[0] : TvStandard.NonSpecific;
                    break;

                case "SIEMENS":
                    camera.Profile.TvStandard = ((SIEMENSCameraModel)model).TvStandard[0];
                    break;

                case "SAMSUNG":
                    camera.Profile.TvStandard = ((SAMSUNGCameraModel)model).TvStandard[0];
                    break;

                case "inskytec":
                    camera.Profile.TvStandard = ((InskytecCameraModel)model).TvStandard[0];
                    break;

                case "VIVOTEK":
                    camera.Profile.TvStandard = ((VIVOTEKCameraModel)model).TvStandard[0];
                    break;

                case "Brickcom":
                    camera.Profile.TvStandard = ((BrickcomCameraModel)model).TvStandard[0];
                    break;

                case "VIGZUL":
                    camera.Profile.TvStandard = ((VIGZULCameraModel)model).TvStandard[0];
                    break;

                case "Certis":
                    camera.Profile.TvStandard = ((CertisCameraModel)model).TvStandard.Count > 0 ? ((CertisCameraModel)model).TvStandard[0] : TvStandard.NonSpecific;
                    break;

                case "DLink":
                    camera.Profile.TvStandard = ((DLinkCameraModel)model).TvStandard[0];
                    break;

                case "Panasonic":
                    camera.Profile.TvStandard = ((PanasonicCameraModel)model).TvStandard[0];
                    break;

                case "FINE":
                    camera.Profile.TvStandard = ((FINECameraModel)model).TvStandard[0];
                    break;

                case "GoodWill":
                    camera.Profile.TvStandard = ((GoodWillCameraModel)model).TvStandard[0];
                    break;

                case "MOBOTIX":
                    camera.Profile.TvStandard = ((MOBOTIXCameraModel)model).TvStandard.Count > 0 ? ((MOBOTIXCameraModel)model).TvStandard[0] : TvStandard.NonSpecific;
                    break;

                default:
                    camera.Profile.TvStandard = TvStandard.NonSpecific;
                    break;
            }
        }

        public static void SetDefaultSensorMode(ICamera camera, CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    camera.Profile.SensorMode = ((EtrovisionCameraModel)model).SensorMode[0];
                    break;

                case "Axis":
                    camera.Profile.SensorMode = ((AxisCameraModel)model).SensorMode[0];
                    break;

                case "ArecontVision":
                    if (((ArecontVisionCameraModel)model).SensorMode.Contains(SensorMode.BinningOff))
                    {
                        camera.Profile.SensorMode = SensorMode.BinningOff;
                    }
                    else
                    {
                        camera.Profile.SensorMode = ((ArecontVisionCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((ArecontVisionCameraModel)model).SensorMode[0];
                    }
                    break;

                case "VIVOTEK":
                    camera.Profile.SensorMode = ((VIVOTEKCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((VIVOTEKCameraModel)model).SensorMode[0];
                    break;

                case "DLink":
                    camera.Profile.SensorMode = ((DLinkCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((DLinkCameraModel)model).SensorMode[0];
                    break;

                case "Panasonic":
                    camera.Profile.SensorMode = ((PanasonicCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((PanasonicCameraModel)model).SensorMode[0];
                    break;

                case "A-MTK":
                    camera.Profile.SensorMode = ((AMTKCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((AMTKCameraModel)model).SensorMode[0];
                    break;

                case "Certis":
                    camera.Profile.SensorMode = ((CertisCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((CertisCameraModel)model).SensorMode[0];
                    break;

                case "DivioTec":
                    camera.Profile.SensorMode = ((DivioTecCameraModel)model).SensorMode.Count == 0 ? SensorMode.NonSpecific : ((DivioTecCameraModel)model).SensorMode[0];
                    break;

                default:
                    camera.Profile.SensorMode = SensorMode.NonSpecific;
                    break;
            }
        }

        public static void SetDefaultPowerFrequency(ICamera camera, CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    camera.Profile.PowerFrequency = ((EtrovisionCameraModel)model).PowerFrequency[0];
                    break;

                case "Messoa":
                    camera.Profile.PowerFrequency = ((MessoaCameraModel)model).PowerFrequency[0];
                    break;

                case "Axis":
                    camera.Profile.PowerFrequency = ((AxisCameraModel)model).PowerFrequency[0];
                    break;

                case "VIVOTEK":
                    camera.Profile.PowerFrequency = ((VIVOTEKCameraModel)model).PowerFrequency[0];
                    break;

                case "ArecontVision":
                    camera.Profile.PowerFrequency = ((ArecontVisionCameraModel)model).PowerFrequency[0];
                    break;

                case "PULSE":
                    camera.Profile.PowerFrequency = ((PULSECameraModel)model).PowerFrequency[0];
                    break;

                case "HIKVISION":
                    camera.Profile.PowerFrequency = ((HIKVISIONCameraModel)model).PowerFrequency[0];
                    break;

                case "DLink":
                    camera.Profile.PowerFrequency = ((DLinkCameraModel)model).PowerFrequency[0];
                    break;

                case "Panasonic":
                    camera.Profile.PowerFrequency = ((PanasonicCameraModel)model).PowerFrequency[0];
                    break;

                case "ZeroOne":
                    camera.Profile.PowerFrequency = ((ZeroOneCameraModel)model).PowerFrequency[0];
                    break;

                default:
                    camera.Profile.PowerFrequency = PowerFrequency.NonSpecific;
                    break;
            }
        }

        public static void SetDefaultAspectRatio(ICamera camera, CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "ACTi":
                    camera.Profile.AspectRatio = (((ACTiCameraModel)model).AspectRatio.Count > 0)
                           ? ((ACTiCameraModel)model).AspectRatio[0]
                           : AspectRatio.NonSpecific;
                    break;

                case "DLink":
                    camera.Profile.AspectRatio = (((DLinkCameraModel)model).AspectRatio.Count > 0)
                           ? ((DLinkCameraModel)model).AspectRatio[0]
                           : AspectRatio.NonSpecific;
                    break;

                case "Panasonic":
                    camera.Profile.AspectRatio = (((PanasonicCameraModel)model).AspectRatio.Count > 0)
                           ? ((PanasonicCameraModel)model).AspectRatio[0]
                           : AspectRatio.NonSpecific;
                    break;

                default:
                    camera.Profile.AspectRatio = AspectRatio.NonSpecific;
                    break;
            }

            if (camera.Model.Series != "ARC")
                camera.Profile.AspectRatioCorrection = false;
        }

        public static void SetDefaultIOPort(ICamera camera, CameraModel model)
        {
            camera.IOPort.Clear();

            foreach (var ioPort in model.IOPorts)
                camera.IOPort.Add(ioPort.Key, ioPort.Value);
        }

        public static void CheckAvailableSetDefaultResolution(ICamera camera, CameraModel model)
        {
            if (camera.StreamConfig == null || model == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                CheckAvailableSetDefaultResolution(camera, model, obj.Value, obj.Key);
            }
        }

        public static void CheckAvailableSetDefaultResolution(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            if (camera.StreamConfig == null || model == null) return;

            Resolution[] resolutions = GetResolution(camera, model, streamId);

            if (resolutions == null)
            {
                streamConfig.Resolution = Resolution.NA;
                return;
            }

            if (streamConfig.Resolution == 0)
            {
                SetDefaultResolution(streamConfig, resolutions);
                return;
            }

            if (!resolutions.Any(resolution => resolution == streamConfig.Resolution))
            {
                streamConfig.Resolution = 0;
                SetDefaultResolution(streamConfig, resolutions);
            }
        }

        private static void SetDefaultResolution(StreamConfig streamConfig, Resolution[] resolutions)
        {
            foreach (Resolution resolution in resolutions)
            {
                if (resolution == Resolution.R720X480 || resolution == Resolution.R640X480)
                {
                    streamConfig.Resolution = resolution;
                    break;
                }
            }
            if (streamConfig.Resolution == 0)
            {
                if (resolutions.Length == 1)
                    streamConfig.Resolution = resolutions[0];
                else
                    streamConfig.Resolution = resolutions[Convert.ToUInt16(Math.Floor((resolutions.Length - 1) / 2.0))];
            }
        }

        public static void CheckAvailableSetDefaultDewarpMode(ICamera camera, CameraModel model)
        {
            if (camera.StreamConfig == null || model == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                CheckAvailableSetDefaultDewarpMode(camera, model, obj.Value, obj.Key);
            }
        }

        public static void CheckAvailableSetDefaultDewarpMode(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            if (camera.StreamConfig == null || model == null) return;

            Dewarp[] dewarps = GetDewarpMode(camera, model, streamId);

            if (dewarps == null)
            {
                streamConfig.Dewarp = Dewarp.NonSpecific;
                return;
            }

            if (streamConfig.Dewarp == 0)
            {
                SetDefaultDewarpMode(streamConfig, dewarps);
                return;
            }

            if (!dewarps.Any(dewarp => dewarp == streamConfig.Dewarp))
            {
                streamConfig.Dewarp = 0;
                SetDefaultDewarpMode(streamConfig, dewarps);
            }
        }

        private static void SetDefaultDewarpMode(StreamConfig streamConfig, Dewarp[] dewarps)
        {
            foreach (Dewarp dewarp in dewarps)
            {
                if (dewarp == Dewarp.Dewarp1O)
                {
                    streamConfig.Dewarp = dewarp;
                    break;
                }
            }
            if (streamConfig.Dewarp == 0)
            {
                if (dewarps.Length == 1)
                    streamConfig.Dewarp = dewarps[0];
                else
                    streamConfig.Dewarp = dewarps[Convert.ToUInt16(Math.Floor((dewarps.Length - 1) / 2.0))];
            }
        }

        

        public static void CheckAvailableSetDefaultBitrate(ICamera camera, CameraModel model)
        {
            if (camera.StreamConfig == null || model == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                CheckAvailableSetDefaultBitrate(camera, model, obj.Value, obj.Key);
            }
        }

        public static void CheckAvailableSetDefaultBitrate(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            Bitrate[] bitrates = GetBitrate(camera, model, streamId);

            if (bitrates == null)
            {
                streamConfig.Bitrate = Bitrate.NA;
                return;
            }

            if (streamConfig.Bitrate == 0)
            {
                SetDefaultBitrate(streamConfig, bitrates);
                return;
            }

            if (!bitrates.Any(bitrate => bitrate == streamConfig.Bitrate))
            {
                streamConfig.Bitrate = 0;
                SetDefaultBitrate(streamConfig, bitrates);
            }
        }

        private static void SetDefaultBitrate(StreamConfig streamConfig, Bitrate[] bitrates)
        {
            if (bitrates == null) return;

            foreach (Bitrate bitrate in bitrates)
            {
                if (bitrate == Bitrate.Bitrate1M500K)
                {
                    streamConfig.Bitrate = bitrate;
                    return;
                }
            }

            foreach (Bitrate bitrate in bitrates)
            {
                if (bitrate == Bitrate.Bitrate1M)
                {
                    streamConfig.Bitrate = bitrate;
                    return;
                }
            }

            if (streamConfig.Bitrate == 0)
            {
                if (bitrates.Length == 1)
                    streamConfig.Bitrate = bitrates[0];
                else
                    streamConfig.Bitrate = bitrates[Convert.ToUInt16(Math.Floor((bitrates.Length - 1) / 4.0))];
            }
        }

        public static void SetDefaultBitrateControl(CameraModel model, StreamConfig streamConfig)
        {
            switch (model.Manufacture)
            {
                case "ArecontVision":
                    if (streamConfig.BitrateControl == BitrateControl.NA)
                        streamConfig.BitrateControl = BitrateControl.CBR;
                    break;

                default:
                    streamConfig.BitrateControl = BitrateControl.NA;
                    break;
            }
        }

        public static void CheckAvailableSetDefaultFramerate(ICamera camera, CameraModel model)
        {
            if (camera.StreamConfig == null || model == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> obj in camera.Profile.StreamConfigs)
            {
                CheckAvailableSetDefaultFramerate(camera, model, obj.Value, obj.Key);
            }
        }

        public static void CheckAvailableSetDefaultFramerate(ICamera camera, CameraModel model, StreamConfig streamConfig, UInt16 streamId)
        {
            UInt16[] framerates = GetFramerate(camera, model, streamId);

            if (framerates == null)
            {
                streamConfig.Framerate = 0;
                return;
            }

            if (streamConfig.Framerate == 0)
            {
                SetDefaultFramerate(streamConfig, framerates);
                return;
            }

            if (!framerates.Any(framerate => framerate == streamConfig.Framerate))
            {
                streamConfig.Framerate = 0;
                SetDefaultFramerate(streamConfig, framerates);
            }
        }

        private static void SetDefaultFramerate(StreamConfig streamConfig, UInt16[] framerates)
        {
            if (framerates == null)
            {
                streamConfig.Framerate = 0;
                return;
            }

            //set middle

            if (streamConfig.Framerate == 0)
            {
                if (framerates.Length == 1)
                    streamConfig.Framerate = framerates[0];
                else
                    streamConfig.Framerate = framerates[Convert.ToUInt16(Math.Floor((framerates.Length - 1) / 2.0))];
            }

            //foreach (UInt16 framerate in framerates)
            //{
            //    if (framerate == 30)
            //    {
            //        streamConfig.Framerate = framerate;
            //        break;
            //    }
            //}

            //if (streamConfig.Framerate == 0)
            //    streamConfig.Framerate = framerates[framerates.Length - 1];//try to set to 30(if avaliable) if not avaliable try maximum one
        }

        public static ConnectionProtocol[] GetConnectionProtocol(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "MegaSys":
                    var megaSysCompressions = GetCompression(camera, model, streamId);
                    if (megaSysCompressions == null) return model.ConnectionProtocol;
                    return ((MegaSysCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])megaSysCompressions);

                case "Avigilon":
                    var avigilonCompressions = GetCompression(camera, model, streamId);
                    if (avigilonCompressions == null) return model.ConnectionProtocol;
                    return ((AvigilonCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])avigilonCompressions);

                case "DivioTec":
                    var divioTecCompressions = GetCompression(camera, model, streamId);
                    if (divioTecCompressions == null) return model.ConnectionProtocol;
                    return ((DivioTecCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])divioTecCompressions);

                case "SIEMENS":
                    var siemensCompressions = GetCompression(camera, model, streamId);
                    if (siemensCompressions == null) return model.ConnectionProtocol;
                    return ((SIEMENSCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])siemensCompressions);

                case "SAMSUNG":
                    var samsungCompressions = GetCompression(camera, model, streamId);
                    if (samsungCompressions == null) return model.ConnectionProtocol;
                    return ((SAMSUNGCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])samsungCompressions);

                case "inskytec":
                    var inskytecCompressions = GetCompression(camera, model, streamId);
                    if (inskytecCompressions == null) return model.ConnectionProtocol;
                    return ((InskytecCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])inskytecCompressions);

                case "HIKVISION":
                    var hikvisionCompressions = GetCompression(camera, model, streamId);
                    if (hikvisionCompressions == null) return model.ConnectionProtocol;
                    return ((HIKVISIONCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])hikvisionCompressions);

                case "PULSE":
                    var pulseCompressions = GetCompression(camera, model, streamId);
                    if (pulseCompressions == null) return model.ConnectionProtocol;
                    return ((PULSECameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])pulseCompressions);

                case "ZeroOne":
                    var zeroOneCompressions = GetCompression(camera, model, streamId);
                    if (zeroOneCompressions == null) return model.ConnectionProtocol;
                    return ((ZeroOneCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])zeroOneCompressions);

                case "Brickcom":
                    var brickcomCompressions = GetCompression(camera, model, streamId);
                    if (brickcomCompressions == null) return model.ConnectionProtocol;
                    return ((BrickcomCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])brickcomCompressions);

                case "VIGZUL":
                    var vigzulCompressions = GetCompression(camera, model, streamId);
                    if (vigzulCompressions == null) return model.ConnectionProtocol;
                    return ((VIGZULCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])vigzulCompressions);

                case "Surveon":
                    var surveonCompressions = GetCompression(camera, model, streamId);
                    if (surveonCompressions == null) return model.ConnectionProtocol;
                    return ((SurveonCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])surveonCompressions);

                case "Certis":
                    var certisCompressions = GetCompression(camera, model, streamId);
                    if (certisCompressions == null) return model.ConnectionProtocol;
                    return ((CertisCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])certisCompressions);

                case "FINE":
                    var fineCompressions = GetCompression(camera, model, streamId);
                    if (fineCompressions == null) return model.ConnectionProtocol;
                    return ((FINECameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])fineCompressions);

                case "DLink":
                    var dlinkCompressions = GetCompression(camera, model, streamId);
                    if (dlinkCompressions == null) return model.ConnectionProtocol;
                    return ((DLinkCameraModel)model).GetConnectionProtocolByCondition(model.ConnectionProtocol, (Compression[])dlinkCompressions);

                case "Panasonic":
                    var panasonicCompressions = GetCompression(camera, model, streamId);
                    if (panasonicCompressions == null) return model.ConnectionProtocol;
                    return ((PanasonicCameraModel)model).GetConnectionProtocolByCondition(streamId, model.ConnectionProtocol, (Compression[])panasonicCompressions, camera.Profile.AspectRatio);

                default:
                    return model.ConnectionProtocol;
            }
        }

        public static IEnumerable<Compression> GetCompression(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "Axis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AxisCameraModel)model).GetCompressionByCondition(camera.Profile.StreamConfigs[streamId].ConnectionProtocol);
                    return null;

                case "ArecontVision":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ArecontVisionCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.StreamConfigs[streamId].ConnectionProtocol);
                    return null;

                case "DLink":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((DLinkCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((DLinkCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((DLinkCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "Panasonic":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((PanasonicCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((PanasonicCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((PanasonicCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, connectionProtocol, camera.Model.Model, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "VIVOTEK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((VIVOTEKCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.StreamConfigs[streamId].ConnectionProtocol, camera.Profile.TvStandard, camera.Profile.SensorMode);
                    return null;

                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    return ((EtrovisionCameraModel)model).GetCompressionByCondition(streamId, camera.Mode, camera.Profile.SensorMode);

                case "NEXCOM":
                    return ((NexcomCameraModel)model).Compression;

                case "YUAN":
                    return ((YuanCameraModel)model).Compression;

                case "Stretch":
                    return ((StretchCameraModel)model).Compression;

                case "BOSCH":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((BOSCHCameraModel)model).GetCompressionByCondition(streamId);
                    return null;

                case "Messoa":
                    return ((MessoaCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.StreamConfigs[1], camera.Profile.TvStandard);

                case "ZAVIO":
                    return ((ZAVIOCameraModel)model).GetCompressionByCondition(streamId);

                case "Dahua":
                    return ((DahuaCameraModel)model).GetCompressionByCondition(streamId);

                case "GoodWill":
                    return ((GoodWillCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.TvStandard);

                case "FINE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((FINECameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((FINECameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((FINECameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "MOBOTIX":
                    return ((MOBOTIXCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.TvStandard);

                case "GeoVision":
                    return ((GeoVisionCameraModel)model).GetCompressionByCondition(streamId);

                case "Surveon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((SurveonCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((SurveonCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((SurveonCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "Certis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((CertisCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((CertisCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((CertisCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "EverFocus":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((EverFocusCameraModel)model).GetCompressionByCondition(streamId);
                    return null;

                case "A-MTK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AMTKCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.SensorMode, camera.Profile.TvStandard);
                    return null;

                case "Brickcom":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((BrickcomCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((BrickcomCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((BrickcomCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "VIGZUL":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                            case 2:
                                return ((VIGZULCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null);

                            case 3:
                                return ((VIGZULCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null);

                            case 4:
                                return ((VIGZULCameraModel)model).GetCompressionByCondition(streamId, connectionProtocol, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "ACTi":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (camera.Mode)
                        {
                            case CameraMode.Single:
                            case CameraMode.FourVga:
                            case CameraMode.SixVga:
                                return ((ACTiCameraModel)model).GetCompressionByCondition(streamId, camera.Mode);

                            case CameraMode.Dual:
                                if (streamId == 1)
                                    return ((ACTiCameraModel)model).GetCompressionByCondition(streamId, camera.Mode);

                                return ((ACTiCameraModel)model).GetCompressionByStreamCondition(streamId, camera.Mode, camera.Profile.AspectRatio, camera.Profile.StreamConfigs[streamId], camera.Profile.StreamConfigs[1]);
                        }
                    }
                    return null;

                case "MegaSys":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((MegaSysCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol);

                            case 2:
                                return ((MegaSysCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((MegaSysCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((MegaSysCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "Avigilon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((AvigilonCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol);

                            case 2:
                                return ((AvigilonCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((AvigilonCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((AvigilonCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "DivioTec":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((DivioTecCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, connectionProtocol);

                            case 2:
                                return ((DivioTecCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((DivioTecCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((DivioTecCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "SIEMENS":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((SIEMENSCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol);

                            case 2:
                                return ((SIEMENSCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((SIEMENSCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((SIEMENSCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "SAMSUNG":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        return ((SAMSUNGCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.TvStandard, camera.Mode, connectionProtocol);
                    }
                    return null;

                case "inskytec":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        return ((InskytecCameraModel)model).GetCompressionByCondition(streamId, camera.Profile.TvStandard, camera.Mode, connectionProtocol);
                    }
                    return null;

                case "HIKVISION":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((HIKVISIONCameraModel)model).GetCompressionByCondition(camera.Mode, connectionProtocol, camera.Profile.PowerFrequency);

                            case 2:
                                return ((HIKVISIONCameraModel)model).GetCompressionByCondition(camera.Mode, connectionProtocol, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((HIKVISIONCameraModel)model).GetCompressionByCondition(camera.Mode, connectionProtocol, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((HIKVISIONCameraModel)model).GetCompressionByCondition(camera.Mode, connectionProtocol, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 5:
                                return ((HIKVISIONCameraModel)model).GetCompressionByCondition(camera.Mode, connectionProtocol, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "PULSE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((PULSECameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol);

                            case 2:
                                return ((PULSECameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((PULSECameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((PULSECameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                case "ZeroOne":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        switch (streamId)
                        {
                            case 1:
                                return ((ZeroOneCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol);

                            case 2:
                                return ((ZeroOneCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1]);

                            case 3:
                                return ((ZeroOneCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 4:
                                return ((ZeroOneCameraModel)model).GetCompressionByCondition(camera.Profile.TvStandard, camera.Mode, connectionProtocol, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);
                        }
                    }
                    return null;

                default:
                    return ((BasicCameraModel)model).Compression;
            }
        }

        public static Dewarp[] GetDewarpMode(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "VIVOTEK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((VIVOTEKCameraModel)model).GetDewarpModeByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Profile.DeviceMountType);
                    return null;

                case "Axis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AxisCameraModel)model).GetDewarpModeByCondition(streamId, camera.Mode, camera.Profile.DeviceMountType);
                    return null;
            }
            return null;
        }

        public static Resolution[] GetResolution(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "Axis":
                    return ((AxisCameraModel)model).GetResolutionByCondition(streamId, camera.Mode, camera.Profile.SensorMode, camera.Profile.AspectRatioCorrection, camera.Profile.PowerFrequency, camera.Profile.DeviceMountType, camera.Profile.StreamConfigs[streamId].Dewarp);

                case "ArecontVision":
                    return ((ArecontVisionCameraModel)model).GetResolutionByCondition(streamId);

                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    return ((EtrovisionCameraModel)model).GetResolutionByCondition(streamId, camera.Mode, camera.Profile.SensorMode, camera.Profile.PowerFrequency);

                case "NEXCOM":
                    return ((NexcomCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard);

                case "YUAN":
                    return ((YuanCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard);

                case "Stretch":
                    return ((StretchCameraModel)model).GetResolutionByCondition(streamId, camera.Mode, camera.Profile.TvStandard);

                case "BOSCH":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((BOSCHCameraModel)model).GetResolutionByCondition(streamId);
                    return null;

                case "Messoa":
                    return ((MessoaCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[1], camera.Profile.TvStandard);

                case "DLink":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DLinkCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((DLinkCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((DLinkCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((DLinkCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Panasonic":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PanasonicCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((PanasonicCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((PanasonicCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((PanasonicCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIVOTEK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((VIVOTEKCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Profile.DeviceMountType, camera.Profile.StreamConfigs[streamId].Dewarp);
                    return null;

                case "ZAVIO":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ZAVIOCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression);
                    return null;

                case "Dahua":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((DahuaCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression);
                    return null;

                case "GoodWill":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GoodWillCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId].Compression);
                    return null;

                case "FINE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        var compression = camera.Profile.StreamConfigs[streamId].Compression;
                        switch (streamId)
                        {
                            case 1:
                                return ((FINECameraModel)model).GetResolutionByCondition(streamId, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((FINECameraModel)model).GetResolutionByCondition(streamId, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                if(camera.Model.Series == "DynaColor")
                                    return ((FINECameraModel)model).GetResolutionByCondition(streamId, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);
                                else
                                {
                                    var streamConfig2 = camera.Profile.StreamConfigs[2];
                                    return new[] { streamConfig2.Resolution };
                                }
                            case 4:
                                return ((FINECameraModel)model).GetResolutionByCondition(streamId, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;
                 
                case "MOBOTIX":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((MOBOTIXCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression);
                    }
                    return null;

                case "Surveon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SurveonCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((SurveonCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((SurveonCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((SurveonCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Certis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        var compression = camera.Profile.StreamConfigs[streamId].Compression;
                        switch (streamId)
                        {
                            case 1:
                                return ((CertisCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((CertisCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((CertisCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((CertisCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "EverFocus":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((EverFocusCameraModel)model).GetResolutionByCondition(streamId);
                    return null;

                case "A-MTK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AMTKCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.SensorMode, camera.Profile.StreamConfigs[streamId].Compression);
                    return null;

                case "GeoVision":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GeoVisionCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[1]);
                    return null;

                case "Brickcom":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        var compression = camera.Profile.StreamConfigs[streamId].Compression;
                        switch (streamId)
                        {
                            case 1:
                                return ((BrickcomCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((BrickcomCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((BrickcomCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((BrickcomCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIGZUL":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        var connectionProtocol = camera.Profile.StreamConfigs[streamId].ConnectionProtocol;
                        var compression = camera.Profile.StreamConfigs[streamId].Compression;
                        switch (streamId)
                        {
                            case 1:
                                return ((VIGZULCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((VIGZULCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((VIGZULCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((VIGZULCameraModel)model).GetResolutionByCondition(streamId, connectionProtocol, compression, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ACTi":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (camera.Mode)
                        {
                            case CameraMode.Single:
                            case CameraMode.FourVga:
                            case CameraMode.SixVga:
                                return ((ACTiCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio);

                            case CameraMode.Dual:
                                if (streamId == 1)
                                    return ((ACTiCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio);

                                return ((ACTiCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio, camera.Profile.StreamConfigs[1]);
                        }
                    }
                    return null;

                case "MegaSys":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((MegaSysCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((MegaSysCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((MegaSysCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((MegaSysCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Avigilon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((AvigilonCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((AvigilonCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((AvigilonCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((AvigilonCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "DivioTec":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DivioTecCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((DivioTecCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((DivioTecCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((DivioTecCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SIEMENS":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SIEMENSCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((SIEMENSCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((SIEMENSCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((SIEMENSCameraModel)model).GetResolutionByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SAMSUNG":
                     if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((SAMSUNGCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "inskytec":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((InskytecCameraModel)model).GetResolutionByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "HIKVISION":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((HIKVISIONCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((HIKVISIONCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((HIKVISIONCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((HIKVISIONCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);

                            case 5:
                                return ((HIKVISIONCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4], camera.Profile.StreamConfigs[5]);
                        }
                    }
                    return null;

                case "PULSE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PULSECameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((PULSECameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((PULSECameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((PULSECameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ZeroOne":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((ZeroOneCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((ZeroOneCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((ZeroOneCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((ZeroOneCameraModel)model).GetResolutionByCondition(camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                default:
                    return ((BasicCameraModel)model).Resolution;
            }
        }

        public static Bitrate[] GetBitrate(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "Axis":
                    return ((AxisCameraModel)model).GetBitrateByCondition(streamId, camera.Mode, camera.Profile.SensorMode, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.DeviceMountType, camera.Profile.StreamConfigs[streamId].Dewarp);

                case "ArecontVision":
                    return ((ArecontVisionCameraModel)model).GetBitrateByCondition(streamId);

                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    return ((EtrovisionCameraModel)model).GetBitrateByCondition(streamId, camera.Mode, camera.Profile.SensorMode);

                case "NEXCOM":
                    return ((NexcomCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard);

                case "YUAN":
                    return ((YuanCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard);

                case "Stretch":
                    return ((StretchCameraModel)model).GetBitrateByCondition(streamId, camera.Mode, camera.Profile.TvStandard);

                case "BOSCH":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((BOSCHCameraModel)model).GetBitrateByCondition(streamId);
                    return null;

                case "Messoa":
                    return ((MessoaCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[1], camera.Profile.TvStandard);

                case "ZAVIO":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ZAVIOCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "Dahua":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((DahuaCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "GoodWill":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GoodWillCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "FINE":
                     if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((FINECameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((FINECameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((FINECameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((FINECameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "MOBOTIX":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((MOBOTIXCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "GeoVision":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GeoVisionCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "Surveon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SurveonCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((SurveonCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((SurveonCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((SurveonCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Certis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((CertisCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((CertisCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((CertisCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((CertisCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "DLink":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DLinkCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[1], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((DLinkCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[2], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((DLinkCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[3], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((DLinkCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[4], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Panasonic":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PanasonicCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((PanasonicCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((PanasonicCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((PanasonicCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIVOTEK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((VIVOTEKCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.SensorMode);
                    return null;

                case "EverFocus":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((EverFocusCameraModel)model).GetBitrateByCondition(streamId);
                    return null;

                case "A-MTK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AMTKCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "Brickcom":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((BrickcomCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((BrickcomCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((BrickcomCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((BrickcomCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIGZUL":
                     if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((VIGZULCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((VIGZULCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((VIGZULCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((VIGZULCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ACTi":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ACTiCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "MegaSys":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((MegaSysCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((MegaSysCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((MegaSysCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((MegaSysCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Avigilon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((AvigilonCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((AvigilonCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((AvigilonCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((AvigilonCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "DivioTec":
                     if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DivioTecCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((DivioTecCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((DivioTecCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((DivioTecCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SIEMENS":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SIEMENSCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((SIEMENSCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((SIEMENSCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((SIEMENSCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SAMSUNG":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((SAMSUNGCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "inskytec":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((InskytecCameraModel)model).GetBitrateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "HIKVISION":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((HIKVISIONCameraModel)model).GetBitrateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((HIKVISIONCameraModel)model).GetBitrateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((HIKVISIONCameraModel)model).GetBitrateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((HIKVISIONCameraModel)model).GetBitrateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);

                            case 5:
                                return ((HIKVISIONCameraModel)model).GetBitrateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4], camera.Profile.StreamConfigs[5]);
                        }
                    }
                    return null;

                case "PULSE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PULSECameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((PULSECameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((PULSECameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((PULSECameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ZeroOne":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((ZeroOneCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((ZeroOneCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((ZeroOneCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((ZeroOneCameraModel)model).GetBitrateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                default:
                    return ((BasicCameraModel)model).Bitrate;
            }
        }

        public static UInt16[] GetFramerate(ICamera camera, CameraModel model, UInt16 streamId)
        {
            switch (model.Manufacture)
            {
                case "Axis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AxisCameraModel)model).GetFramerateByCondition(streamId, camera.Mode, camera.Profile.SensorMode, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.PowerFrequency, camera.Profile.DeviceMountType, camera.Profile.StreamConfigs[streamId].Dewarp);
                    return null;

                case "ArecontVision":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ArecontVisionCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Resolution);
                    return null;

                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    UInt16[] framerates = ((EtrovisionCameraModel)model).GetFramerateByCondition(streamId, camera.Mode, camera.Profile.SensorMode);
                    if (framerates != null && camera.Profile.PowerFrequency == PowerFrequency.Hertz50)
                    {
                        var temp = new List<UInt16>(framerates);

                        if (temp.Contains(30))
                        {
                            temp.Remove(30);
                            framerates = temp.ToArray();
                        }
                    }
                    return framerates;

                case "NEXCOM":
                    return ((NexcomCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard);

                case "YUAN":
                    return ((YuanCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard);

                case "Stretch":
                    return ((StretchCameraModel)model).GetFramerateByCondition(streamId, camera.Mode, camera.Profile.TvStandard);

                case "BOSCH":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((BOSCHCameraModel)model).GetFramerateByCondition(streamId);
                    return null;

                case "Messoa":
                    return ((MessoaCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[1], camera.Profile.TvStandard);

                case "DLink":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DLinkCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((DLinkCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((DLinkCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((DLinkCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.PowerFrequency, camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Panasonic":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PanasonicCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((PanasonicCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((PanasonicCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((PanasonicCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.AspectRatio, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIVOTEK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((VIVOTEKCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Profile.PowerFrequency);
                    return null;

                case "ZAVIO":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((ZAVIOCameraModel)model).GetFramerateByCondition(streamId, camera.Model, camera.Profile.StreamConfigs);
                    return null;

                case "Dahua":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((DahuaCameraModel)model).GetFramerateByCondition(streamId, camera.Model, camera.Profile.StreamConfigs);
                    return null;

                case "GoodWill":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GoodWillCameraModel)model).GetFramerateByCondition(streamId, camera.Model, camera.Profile.TvStandard, camera.Profile.StreamConfigs);
                    return null;

                case "FINE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((FINECameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((FINECameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((FINECameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((FINECameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "MOBOTIX":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((MOBOTIXCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "GeoVision":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((GeoVisionCameraModel)model).GetFramerateByCondition(streamId);
                    return null;

                case "Surveon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SurveonCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((SurveonCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((SurveonCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((SurveonCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Certis":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((CertisCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((CertisCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((CertisCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((CertisCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "EverFocus":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((EverFocusCameraModel)model).GetFramerateByCondition(streamId);
                    return null;

                case "A-MTK":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                        return ((AMTKCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.SensorMode, camera.Profile.TvStandard, camera.Model, camera.Profile.StreamConfigs[streamId]);
                    return null;

                case "Brickcom":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((BrickcomCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((BrickcomCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((BrickcomCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((BrickcomCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "VIGZUL":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((VIGZULCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], null, null, null);

                            case 2:
                                return ((VIGZULCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], null, null);

                            case 3:
                                return ((VIGZULCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], null);

                            case 4:
                                return ((VIGZULCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId].Compression, camera.Profile.StreamConfigs[streamId].Resolution, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ACTi":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (camera.Mode)
                        {
                            case CameraMode.Single:
                            case CameraMode.FourVga:
                            case CameraMode.SixVga:
                                return ((ACTiCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio);

                            case CameraMode.Dual:
                                if (streamId == 1)
                                    return ((ACTiCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio);

                                return ((ACTiCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.StreamConfigs[streamId], camera.Mode, camera.Profile.AspectRatio, camera.Profile.StreamConfigs[1]);
                        }
                    }
                    return null;

                case "MegaSys":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((MegaSysCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((MegaSysCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((MegaSysCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((MegaSysCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "Avigilon":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((AvigilonCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((AvigilonCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((AvigilonCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((AvigilonCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "DivioTec":
                   if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((DivioTecCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((DivioTecCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((DivioTecCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((DivioTecCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Profile.SensorMode, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SIEMENS":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((SIEMENSCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((SIEMENSCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((SIEMENSCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((SIEMENSCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "SAMSUNG":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((SAMSUNGCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "inskytec":
                     if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        return ((InskytecCameraModel)model).GetFramerateByCondition(streamId, camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[streamId]);
                    }
                    return null;

                case "HIKVISION":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((HIKVISIONCameraModel)model).GetFramerateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((HIKVISIONCameraModel)model).GetFramerateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((HIKVISIONCameraModel)model).GetFramerateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((HIKVISIONCameraModel)model).GetFramerateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);

                            case 5:
                                return ((HIKVISIONCameraModel)model).GetFramerateByCondition(camera.Mode, camera.Profile.PowerFrequency, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4], camera.Profile.StreamConfigs[5]);
                        }
                    }
                    return null;

                case "PULSE":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((PULSECameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((PULSECameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((PULSECameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((PULSECameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;

                case "ZeroOne":
                    if (camera.Profile.StreamConfigs.ContainsKey(streamId))
                    {
                        switch (streamId)
                        {
                            case 1:
                                return ((ZeroOneCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1]);

                            case 2:
                                return ((ZeroOneCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2]);

                            case 3:
                                return ((ZeroOneCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3]);

                            case 4:
                                return ((ZeroOneCameraModel)model).GetFramerateByCondition(camera.Profile.TvStandard, camera.Mode, camera.Profile.StreamConfigs[1], camera.Profile.StreamConfigs[2], camera.Profile.StreamConfigs[3], camera.Profile.StreamConfigs[4]);
                        }
                    }
                    return null;


                default:
                    return ((BasicCameraModel)model).Framerate;
            }
        }

    }
}
