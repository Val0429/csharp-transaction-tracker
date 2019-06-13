using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Xml;
using Constant;
using Device;
using DeviceCab;
using DeviceConstant;
using Interface;
using System.Globalization;

namespace ServerProfile
{
    public partial class DeviceManager : IDeviceManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;

        private const String CgiLoadAllDevice = @"cgi-bin/deviceconfig?action=loadalldevice";
        private const String CgiSaveAllDevice = @"cgi-bin/deviceconfig?action=savealldevice";
        //private const String CgiDeleteAllDevice = @"cgi-bin/deviceconfig?action=deletealldevice";
        private const String CgiSendCommand = @"cgi-bin/sendcommand?channel=channel%1";
        //private const String CgiLoadDevice = @"cgi-bin/deviceconfig?channel=channel%1&action=load";
        //private const String CgiSaveDevice = @"cgi-bin/deviceconfig?channel=channel%1&action=save";
        //private const String CgiDeleteDevice = @"cgi-bin/deviceconfig?channel=channel%1&action=delete";


        private const UInt16 SendcommandTimeout = 30;//30 sec
        public ManagerReadyState ReadyStatus { get; set; }
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public Dictionary<String, String> Localization;



        public DeviceManager()
        {
            Localization = new Dictionary<String, String>
							   {
									{ "Data_AllDevices", "All Devices" }, 
									{ "Data_NotSupport", "Not Support" },
							   };
            Localizations.Update(Localization);

            ReadyStatus = ManagerReadyState.New;

            Devices = new Dictionary<UInt16, IDevice>();
            Groups = new Dictionary<UInt16, IDeviceGroup>();
            // Add By Tulip for User Define Device Group
            UserDefineGroups = new Dictionary<ushort, IDeviceGroup>();
            DeviceLayouts = new Dictionary<UInt16, IDeviceLayout>();

            Manufacture = new Dictionary<String, List<CameraModel>>();
            ScheduleModes.CreateDefaultScheduleSet();
        }

        public void Initialize()
        {
            InitializeBookmarkTimer();
        }



        public String Status
        {
            get { return "Device : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        public Dictionary<UInt16, IDevice> Devices { get; private set; }
        public Dictionary<UInt16, IDeviceGroup> Groups { get; private set; }
        // Add By Tulip for User Define Device Group
        public Dictionary<UInt16, IDeviceGroup> UserDefineGroups { get; set; }
        public Dictionary<UInt16, IDeviceLayout> DeviceLayouts { get; private set; }

        //dont load device layout, if plug-in support, remember to open here.
        //alwary true switch on/off will depanse on setpup.xml have 
        // <Control>
        //  <Name>Device Layout</Name>
        //  <Assembly>SetupDeviceGroup.dll</Assembly>
        //  <ClassName>SetupDeviceGroup.LayoutSetup</ClassName>
        //  <Height>auto</Height>
        //</Control>
        private static Boolean SupportDeviceLayout = true;
        private readonly Stopwatch _watch = new Stopwatch();
        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            Devices.Clear();
            DeviceLayouts.Clear();
            Groups.Clear();
            Manufacture.Clear();

            _watch.Reset();
            _watch.Start();

            if (Server is ICMS)
            {
                LoadDeviceGroups();
            }
            else if (Server is IVAS)
            {
                LoadDevices();
                LoadDeviceGroups();
            }
            else if (Server is INVR)
            {
                LoadDeviceCapability();
                LoadDevices();

                if (SupportDeviceLayout)
                    LoadDeviceLayout();

                LoadDeviceGroups();
            }
        }

        public void Load(String xml)
        {
        }

        public void Save()
        {
            ReadyStatus = ManagerReadyState.Saving;

            _watch.Reset();
            _watch.Start();

            SaveDelegate saveDeviceDelegate = SaveDeviceAndGroup;
            saveDeviceDelegate.BeginInvoke(SaveCallback, saveDeviceDelegate);
        }

        public void Save(String xml)
        {
            XmlDocument xmlDoc = Xml.LoadXml(xml);
            String type = Xml.GetFirstElementValueByTagName(xmlDoc, "Type");
            String id = Xml.GetFirstElementValueByTagName(xmlDoc, "Id");
            switch (type)
            {
                case "Device":
                    SaveDevices();
                    break;

                case "DeviceGroup":
                    SaveDeviceGroups();
                    break;

                //case "PresetTour":
                //    if (id != "")
                //    {
                //        SavePresetTour(Convert.ToUInt16(id));
                //    }
                //    break;

                case "Bookmark":
                    if (id != "")
                    {
                        SaveBookmarkDelegate saveBookmarkDelegate = SaveBookmark;
                        saveBookmarkDelegate.BeginInvoke(Convert.ToUInt16(id), null, null);
                        //SaveBookmark(Convert.ToUInt16(id));
                    }
                    break;
            }
        }

        private void LoadDevices()
        {
            Devices.Clear();

            if (Server is IVAS)
            {
                LoadAllDevicePeopleCounting();
            }
            else if (Server is INVR)
            {
                LoadAllDevice();
            }
        }

        private static Dictionary<ICamera, UInt16> _tempForPIP = new Dictionary<ICamera, UInt16>();//Device, PIP device id
        private void LoadAllDevice()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllDevice, Server.Credential);

            var temp = new Dictionary<UInt16, IDevice>();
            _tempForPIP.Clear();

            if (xmlDoc != null)
            {
                XmlNodeList devicesList = xmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");
                foreach (XmlNode node in devicesList)
                {
                    UInt16 id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

                    IDevice device = ParseDeviceProfileFromXml((XmlElement)node);
                    if (device != null && !temp.ContainsKey(id))
                    {
                        temp.Add(id, device);
                    }
                }
                var ids = temp.Keys.ToList();
                ids.Sort();

                foreach (var id in ids)
                {
                    Devices.Add(id, temp[id]);
                }
            }

            foreach (var obj in Groups)
            {
                IDevice[] devices = obj.Value.Items.Where(device => !Devices.ContainsKey(device.Id)).ToArray();
                foreach (IDevice device in devices)
                {
                    obj.Value.Items.Remove(device);
                }

                devices = obj.Value.View.Where(device => device != null && !Devices.ContainsKey(device.Id)).ToArray();
                foreach (IDevice device in devices)
                {
                    obj.Value.View.Remove(device);
                }
            }

            foreach (var obj in Groups)
            {
                while (obj.Value.View.Count > 0 && obj.Value.View[obj.Value.View.Count - 1] == null)
                {
                    obj.Value.View.RemoveAt(obj.Value.View.Count - 1);
                }
            }

            //------------------------------------------------------------------------

            foreach (var obj in DeviceLayouts)
            {
                IDevice[] devices = obj.Value.Items.Where(device => device != null && !Devices.ContainsKey(device.Id)).ToArray();
                foreach (IDevice device in devices)
                {
                    obj.Value.Items.Remove(device);
                }
            }

            //-----------------------------------------------------------------------
            //PIP
            foreach (KeyValuePair<ICamera, UInt16> device in _tempForPIP)
            {
                if(Devices.ContainsKey(device.Value))
                {
                    device.Key.PIPDevice = Devices[device.Value] as ICamera;
                }
            }

            if (Devices.Count > 0)
            {
                LoadAllEvent();
                LoadAllPresetPoint();
                LoadAllPresetTour();
                LoadAllBookmark();

                ConvertEventDeviceIdToDevice();
            }
        }

        private void LoadAllDevicePeopleCounting()
        {
            Devices.Clear();
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllDevice, Server.Credential);

            if (xmlDoc != null && Server is IVAS)
            {
                XmlNodeList devicesList = xmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");

                var vas = Server as IVAS;
                foreach (XmlNode node in devicesList)
                {
                    UInt16 id = Convert.ToUInt16(((XmlElement)node).GetAttribute("id"));
                    if (!Devices.ContainsKey(id)) continue;

                    XmlElement nvr = Xml.GetFirstElementByTagName(node, "NVR");
                    XmlNode scheduleNode = node.SelectSingleNode("Schedule");
                    UInt16 nvrId = Convert.ToUInt16(nvr.GetAttribute("id"));

                    UInt16 deviceId = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

                    //make temp device/ link to NVR's Device
                    if (!vas.NVR.NVRs.ContainsKey(nvrId)) continue;

                    ICamera camera = new Camera
                    {
                        Server = vas.NVR.NVRs[nvrId],
                        Id = deviceId,
                        ReadyState = ReadyState.Ready,
                    };

                    Devices[id] = camera;

                    //Parse Rectangles and DispatcherConfiguration
                    XmlElement algorithm = Xml.GetFirstElementByTagName(node, "Algorithm");
                    XmlElement rectangles = Xml.GetFirstElementByTagName(algorithm, "Rectangles");
                    XmlElement dispatcherConfiguration = Xml.GetFirstElementByTagName(algorithm, "DispatcherConfiguration");

                    camera.Rectangles = new List<PeopleCountingRectangle>();
                    XmlNodeList rectangleList = rectangles.GetElementsByTagName("Rectangle");
                    foreach (XmlElement rectangleNode in rectangleList)
                    {
                        var rectangle = new PeopleCountingRectangle
                        {
                            Rectangle = new Rectangle
                            {
                                Location = new Point(Convert.ToInt32(rectangleNode.GetAttribute("x")), Convert.ToInt32(rectangleNode.GetAttribute("y"))),
                                Size = new Size(Convert.ToInt32(rectangleNode.GetAttribute("width")), Convert.ToInt32(rectangleNode.GetAttribute("height"))),
                            }
                        };

                        XmlElement startPoint = Xml.GetFirstElementByTagName(rectangleNode, "StartPoint");
                        XmlElement endPoint = Xml.GetFirstElementByTagName(rectangleNode, "EndPoint");
                        rectangle.StartPoint = new Point(Convert.ToInt32(startPoint.GetAttribute("x")), Convert.ToInt32(startPoint.GetAttribute("y")));
                        rectangle.EndPoint = new Point(Convert.ToInt32(endPoint.GetAttribute("x")), Convert.ToInt32(endPoint.GetAttribute("y")));
                        rectangle.In = (Direction)Enum.Parse(typeof(Direction), Xml.GetFirstElementValueByTagName(rectangleNode, "In"), true);
                        rectangle.Out = (Direction)Enum.Parse(typeof(Direction), Xml.GetFirstElementValueByTagName(rectangleNode, "Out"), true);
                        camera.PeopleCountingSetting.FeatureThreshold = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(rectangleNode, "FeatureThreshold"));
                        camera.PeopleCountingSetting.FeatureNumberThreshold = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(rectangleNode, "FeatureNumberThreshold"));
                        camera.PeopleCountingSetting.PersonNumber = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(rectangleNode, "PersonNumber"));
                        camera.PeopleCountingSetting.DirectNumber = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(rectangleNode, "DirectNumber"));
                        camera.PeopleCountingSetting.FrameIndex = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(rectangleNode, "FrameIndex"));
                        camera.Rectangles.Add(rectangle);
                    }

                    //Parse Dispatcher
                    camera.Dispatcher = new NetworkCredential();

                    XmlElement retry = Xml.GetFirstElementByTagName(dispatcherConfiguration, "Retry");
                    if (retry != null)
                    {
                        camera.PeopleCountingSetting.Retry = Convert.ToUInt16(retry.GetAttribute("count"));
                        camera.PeopleCountingSetting.Interval = Convert.ToUInt16(retry.GetAttribute("period"));
                    }

                    XmlElement notify = Xml.GetFirstElementByTagName(dispatcherConfiguration, "Notify");
                    if (notify != null)
                        camera.Dispatcher.Domain = Xml.GetFirstElementValueByTagName(notify, "URI");

                    camera.EventSchedule = new Schedule();
                    if (scheduleNode != null)
                        camera.EventSchedule.AddRange(ConvertWeekScheduleToScheduleData(
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Mon") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Tue") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Wed") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Thu") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Fri") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Sat") +
                            Xml.GetFirstElementValueByTagName(scheduleNode, "Sun"), "Event"));
                    camera.EventSchedule.Description = ScheduleModes.CheckMode(camera.EventSchedule);

                    //copy schedule when it's CUSTOM
                    if (camera.EventSchedule.Description == ScheduleMode.CustomSchedule)
                    {
                        camera.EventSchedule.CustomSchedule = new List<ScheduleData>();
                        ScheduleModes.Clone(camera.EventSchedule.CustomSchedule, camera.EventSchedule);
                    }
                }

                List<UInt16> lostDevice = (from obj in Devices where !(obj.Value is ICamera) select obj.Value.Id).ToList();

                foreach (UInt16 id in lostDevice)
                {
                    Devices.Remove(id);
                }
            }
            else
            {
                Devices.Clear();
            }
        }

        public XmlDocument LoadAllDeviceAsXmlDocument()
        {
            return Xml.LoadXmlFromHttp(CgiLoadAllDevice, Server.Credential);
        }

        private void DeviceManagerLoadReady()
        {
            _watch.Stop();

            ReadyStatus = ManagerReadyState.Ready;

            if (OnLoadComplete != null)
                OnLoadComplete(this, null);
        }

        protected virtual IDevice ParseDeviceProfileFromXml(XmlElement node)
        {
            try
            {
                XmlNode settingNode = node.SelectSingleNode("DeviceSetting");
                XmlNode scheduleNode = node.SelectSingleNode("Schedule");
                XmlNode recordNode = node.SelectSingleNode("Record");

                if (settingNode == null) return null;

                String manufacture = Xml.GetFirstElementValueByTagName(settingNode, "Brand");
                String productionId = Xml.GetFirstElementValueByTagName(settingNode, "Model");

                if (!Manufacture.ContainsKey(manufacture)) return null;
                var list = Manufacture[manufacture];

                CameraModel model = list.FirstOrDefault(cameraModel => String.Equals(cameraModel.Alias, productionId));

                if (model == null) return null;

                String name = Xml.GetFirstElementValueByTagName(settingNode, "Name");
                if (name == "") name = manufacture + " " + productionId;

                XmlNode authenticationNode = settingNode.SelectSingleNode("Authentication");
                if (authenticationNode == null) return null;

                var streamConfigs = settingNode.SelectNodes("StreamConfig");
                if (streamConfigs == null) return null;

                foreach (XmlNode streamConfig in streamConfigs)
                    settingNode.RemoveChild(streamConfig);

                var camera = new Camera
                {
                    Server = Server,
                    Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID")),
                    Type = CameraTypes.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "Type")),
                    Mode = CameraModes.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "Mode")),
                    //SensorMode
                    //PowerFrequency
                    Name = name,
                    ReadyState = ReadyState.Ready,
                };

                settingNode.RemoveChild(authenticationNode);

                String port = Xml.GetFirstElementValueByTagName(settingNode, "Http");
                String audioPort = Xml.GetFirstElementValueByTagName(settingNode, "AudioOut");
                String recordStream = Xml.GetFirstElementValueByTagName(settingNode, "RecordStream");

                var highProfile = "1";
                var mediumProfile = "1";
                var lowProfile = "1";
                var multiStreamNode = settingNode.SelectSingleNode("Multi-Stream");
                if (multiStreamNode != null)
                {
                    highProfile = Xml.GetFirstElementValueByTagName(multiStreamNode, "HighProfile");
                    mediumProfile = Xml.GetFirstElementValueByTagName(multiStreamNode, "MediumProfile");
                    lowProfile = Xml.GetFirstElementValueByTagName(multiStreamNode, "LowProfile");
                }

                var remoteRecovery = false;
                var remoteRecoveryNode = Xml.GetFirstElementByTagName(settingNode, "RemoteRecovery");
                if (remoteRecoveryNode != null)
                {
                    remoteRecovery = Xml.GetFirstElementValueByTagName(remoteRecoveryNode, "Enable") == "1";
                }

                camera.Profile = new CameraProfile
                {
                    NetworkAddress = Xml.GetFirstElementValueByTagName(settingNode, "IPAddress"),
                    ConnectionTimeout = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(settingNode, "ConnectTimeOut")),
                    ReceiveTimeout = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(settingNode, "ReceiveTimeOut")),
                    //MulticastIp = Xml.GetFirstElementsValueByTagName(node, "MulticastIP"),
                    StreamId = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(settingNode, "Stream")),
                    RecordStreamId = String.IsNullOrEmpty(recordStream) ? (UInt16)1 : Convert.ToUInt16(recordStream),
                    Authentication =
                    {
                        UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(authenticationNode, "Account")),
                        Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(authenticationNode, "Password")),
                        Encryption = Encryptions.ToIndex(Xml.GetFirstElementValueByTagName(authenticationNode, "Encryption")),
                        OccupancyPriority = (uint)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(authenticationNode, "OccupancyPriority")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(authenticationNode, "OccupancyPriority")))
                    },

                    HttpPort = (port != "") ? Convert.ToUInt16(port) : (UInt16)0,
                    AudioOutPort = (audioPort != "") ? Convert.ToUInt16(audioPort) : (UInt16)0,
                    TvStandard = TvStandards.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "TVStandard")),
                    DewarpType = Xml.GetFirstElementValueByTagName(settingNode, "DewarpType"),
                    DeviceMountType = DeviceMountTypes.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "DeviceMountType")),
                    SensorMode = SensorModes.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "SensorMode")),
                    PowerFrequency = PowerFrequencies.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "PowerFrequency")),
                    AspectRatioCorrection = (Xml.GetFirstElementValueByTagName(settingNode, "AspectRatioCorrection") == "true"),
                    RemoteRecovery = remoteRecovery,
                    AspectRatio = AspectRatios.ToIndex(Xml.GetFirstElementValueByTagName(settingNode, "AspectRatio")),
                    HighProfile = Convert.ToUInt16(String.IsNullOrEmpty(highProfile) ? "1" : highProfile),
                    MediumProfile = Convert.ToUInt16(String.IsNullOrEmpty(mediumProfile) ? "1" : mediumProfile),
                    LowProfile = Convert.ToUInt16(String.IsNullOrEmpty(lowProfile) ? "1" : lowProfile),
                };

                if (streamConfigs.Count > 0)
                {
                    foreach (XmlElement configs in streamConfigs)
                    {
                        XmlNode portNode = configs.SelectSingleNode("Port");
                        XmlNode videoNode = configs.SelectSingleNode("Video");

                        var quality = Xml.GetFirstElementValueByTagName(videoNode, "Quality");
                        var fps = Xml.GetFirstElementValueByTagName(videoNode, "Fps");
                        var regionStartPointX = Xml.GetFirstElementValueByTagName(videoNode, "RegionStartPointX");
                        var regionStartPointY = Xml.GetFirstElementValueByTagName(videoNode, "RegionStartPointY");
                        var motionThreshold = Xml.GetFirstElementValueByTagName(videoNode, "MotionThreshold");
                        var resolution = Xml.GetFirstElementValueByTagName(videoNode, "Resolution");
                        camera.Profile.StreamConfigs.Add(Convert.ToUInt16(configs.GetAttribute("id")), new StreamConfig
                        {
                            Channel = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configs, "Channel")),
                            Dewarp = Dewarps.ToIndex(Xml.GetFirstElementValueByTagName(configs, "DewarpMode")),
                            ConnectionProtocol = ConnectionProtocols.ToIndex(Xml.GetFirstElementValueByTagName(configs, "Protocol")),
                            Compression = Compressions.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Encode")),
                            Resolution =  String.IsNullOrEmpty(resolution) ? Resolutions.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Width") + "x" + Xml.GetFirstElementValueByTagName(videoNode, "Height")) : Resolutions.ToIndex(resolution),
                            VideoQuality = String.IsNullOrEmpty(quality) ? (UInt16)60 : Convert.ToUInt16(quality),
                            Framerate = String.IsNullOrEmpty(fps) ? (UInt16)1 : Convert.ToUInt16(fps),
                            Bitrate = Bitrates.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Bitrate")),
                            BitrateControl = BitrateControls.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "BitrateControl")),
                            RegionStartPointX = String.IsNullOrEmpty(regionStartPointX) ? 0 : Convert.ToInt32(regionStartPointX),
                            RegionStartPointY = String.IsNullOrEmpty(regionStartPointY) ? 0 : Convert.ToInt32(regionStartPointY),
                            MotionThreshold = String.IsNullOrEmpty(motionThreshold) ? (UInt16)0 : Convert.ToUInt16(motionThreshold),
                            ConnectionPort =
                            {
                                Control = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "Control")),
                                Streaming = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "Stream")),
                                Https = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(portNode, "Https")) ? 443 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "Https"))),
                                Rtsp = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "RTSP")),
                                VideoIn = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(portNode, "VideoIn")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "VideoIn"))),
                                AudioIn = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(portNode, "AudioIn")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(portNode, "AudioIn"))),
                            },
                            URI = Xml.GetFirstElementValueByTagName(configs, "URI"),
                            MulticastNetworkAddress = Xml.GetFirstElementValueByTagName(configs, "MulticastNetworkAddress"),
                            ProfileMode = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(configs, "ProfileMode")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configs, "ProfileMode"))),
                        });
                    }
                }

                if (camera.Mode == CameraMode.Dual)
                {
                    //stream 2/3 is off, added(clone) and set as off
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                    {
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[2].Compression = Compression.Off;
                    }
                }

                if (camera.Mode == CameraMode.Triple)
                {
                    //stream 2/3 is off, added(clone) and set as off
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                    {
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[2].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                    {
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[3].Compression = Compression.Off;
                    }
                }

                if (camera.Mode == CameraMode.Multi || camera.Mode == CameraMode.FourVga)
                {
                    //stream 2/3 is off, added(clone) and set as off
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                    {
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[2].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                    {
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[3].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                    {
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[4].Compression = Compression.Off;
                    }
                }

                if (camera.Mode == CameraMode.Five)
                {
                    //stream 2/3/4/5 is off, added(clone) and set as off
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                    {
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[2].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                    {
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[3].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                    {
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[4].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(5))
                    {
                        camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[5].Compression = Compression.Off;
                    }
                }

                if (camera.Mode == CameraMode.SixVga)
                {
                    //stream 2/3/4/5/6 is off, added(clone) and set as off
                    if (!camera.Profile.StreamConfigs.ContainsKey(2))
                    {
                        camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[2].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(3))
                    {
                        camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[3].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(4))
                    {
                        camera.Profile.StreamConfigs.Add(4, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[4].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(5))
                    {
                        camera.Profile.StreamConfigs.Add(5, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[5].Compression = Compression.Off;
                    }
                    if (!camera.Profile.StreamConfigs.ContainsKey(6))
                    {
                        camera.Profile.StreamConfigs.Add(6, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        camera.Profile.StreamConfigs[6].Compression = Compression.Off;
                    }
                }

                camera.Model = model;
                camera.PreRecord = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(recordNode, "PreRecord"));
                camera.PostRecord = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(recordNode, "PostRecord"));

                if (camera.Model.Manufacture == "Messoa")
                    camera.Profile.PowerFrequency = PowerFrequency.NonSpecific;

                camera.RecordSchedule = new Schedule();
                camera.RecordSchedule.AddRange(ConvertWeekScheduleToScheduleData(
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Mon") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Tue") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Wed") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Thu") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Fri") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Sat") +
                        Xml.GetFirstElementValueByTagName(scheduleNode, "Sun"), "Record"));
                camera.RecordSchedule.Description = ScheduleModes.CheckMode(camera.RecordSchedule);

                //copy schedule when it's CUSTOM
                if (camera.RecordSchedule.Description == ScheduleMode.CustomSchedule)
                {
                    camera.RecordSchedule.CustomSchedule = new List<ScheduleData>();
                    ScheduleModes.Clone(camera.RecordSchedule.CustomSchedule, camera.RecordSchedule);
                }

                if (model.Type == "CaptureCard")
                    ParseDeviceCaptureCardProfileFromXml(camera, node);

                if (model.IOPortSupport != null)
                    ParseDeviceIOPortProfileFromXml(camera, node);

                //Live Check
                var liveCheckCommand = Xml.GetFirstElementByTagName(node, "LiveCheckCommand");
                if (liveCheckCommand != null)
                {
                    var cmdLiveCheck = Xml.GetFirstElementByTagName(node, "cmdLiveCheck");
                    if (cmdLiveCheck != null)
                    {
                        camera.Profile.LiveCheckURI = Xml.GetFirstElementValueByTagName(cmdLiveCheck, "Cgi");
                        var interval = Xml.GetFirstElementValueByTagName(cmdLiveCheck, "RetryInterval");
                        camera.Profile.LiveCheckInterval = String.IsNullOrEmpty(interval) ? 5 : Convert.ToUInt64(interval);

                        var retryCount = Xml.GetFirstElementValueByTagName(cmdLiveCheck, "RetryCount");
                        camera.Profile.LiveCheckRetryCount = String.IsNullOrEmpty(retryCount) ? 0 : Convert.ToUInt64(retryCount);
                    }
                }

                //PTZ Command
                var ptzCommand = Xml.GetFirstElementByTagName(node, "PTZCommand");
                if (ptzCommand != null)
                {
                    ParseDevicePTZCommandProfileFromXml(camera, ptzCommand);
                }

                //PIP
                if(Server.Server.SupportPIP)
                {
                    var pip = Xml.GetFirstElementByTagName(node, "PIP");
                    if (pip != null)
                    {
                        ParseDevicePIPProfileFromXml(camera, pip);
                    }
                }

                //Digital PTZ region
                var digitalPTZRegion = Xml.GetFirstElementByTagName(node, "DigitalPTZRegions");
                if (digitalPTZRegion != null)
                {
                    var interval = digitalPTZRegion.GetAttribute("interval");
                    if(String.IsNullOrEmpty(interval))
                    {
                        camera.PatrolInterval = 5;
                    }
                    else
                    {
                        camera.PatrolInterval = Convert.ToUInt16(interval);
                    }
                    ParseDeviceDigitalPTZRegionProfileFromXml(camera, digitalPTZRegion);
                }

                return camera;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(@"Parse Device XML Error " + exception);
            }

            return null;
        }

        private static void ParseDeviceDigitalPTZRegionProfileFromXml(ICamera camera, XmlElement node)
        {
            if (node == null) return;
            var regionsNodes = node.SelectNodes("DigitalPTZRegion");
            if(regionsNodes == null) return;

            foreach (XmlElement regionsNode in regionsNodes)
            {
                var id = regionsNode.GetAttribute("id");
                var name = regionsNode.GetAttribute("name");

                if (!String.IsNullOrEmpty(id))
                {
                    var pointId = Convert.ToUInt16(id);
                    if (camera.PatrolPoints.ContainsKey(pointId))
                    {
                        camera.PatrolPoints[pointId] = new WindowPTZRegionLayout
                        {
                            Id = (short)pointId,
                            Name = name,
                            RegionXML = (XmlElement) regionsNode.SelectSingleNode("PTZRegions")
                        };
                    }
                }
            }
        }

        private static void ParseDevicePIPProfileFromXml(ICamera camera, XmlElement node)
        {
            if (node == null) return;
            var deviceId = Xml.GetFirstElementValueByTagName(node, "DeviceID");
            if(!String.IsNullOrEmpty(deviceId))
            {
                _tempForPIP.Add(camera, Convert.ToUInt16(deviceId));

                var streamId = Xml.GetFirstElementValueByTagName(node, "Stream");
                camera.PIPStreamId = (ushort) (String.IsNullOrEmpty(streamId) ? 1 : Convert.ToUInt16(streamId));

                var position = Xml.GetFirstElementValueByTagName(node, "Position");
                camera.PIPPosition = Positions.ToXMLIndex(position);
            }
        }

        private static void ParseDevicePTZCommandProfileFromXml(ICamera camera, XmlElement node)
        {
            if (node == null) return;
            var cmdMoveUp = Xml.GetFirstElementByTagName(node, "cmdMoveUp");
            if (cmdMoveUp != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveUp, camera.Profile.PtzCommand.Up);

            var cmdMoveDown = Xml.GetFirstElementByTagName(node, "cmdMoveDown");
            if (cmdMoveDown != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveDown, camera.Profile.PtzCommand.Down);

            var cmdMoveRight = Xml.GetFirstElementByTagName(node, "cmdMoveRight");
            if (cmdMoveRight != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveRight, camera.Profile.PtzCommand.Right);

            var cmdMoveLeft = Xml.GetFirstElementByTagName(node, "cmdMoveLeft");
            if (cmdMoveLeft != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveLeft, camera.Profile.PtzCommand.Left);

            var cmdMoveUpRight = Xml.GetFirstElementByTagName(node, "cmdMoveUpRight");
            if (cmdMoveUpRight != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveUpRight, camera.Profile.PtzCommand.UpRight);

            var cmdMoveDownRight = Xml.GetFirstElementByTagName(node, "cmdMoveDownRight");
            if (cmdMoveDownRight != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveDownRight, camera.Profile.PtzCommand.DownRight);

            var cmdMoveUpLeft = Xml.GetFirstElementByTagName(node, "cmdMoveUpLeft");
            if (cmdMoveUpLeft != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveUpLeft, camera.Profile.PtzCommand.UpLeft);

            var cmdMoveDownLeft = Xml.GetFirstElementByTagName(node, "cmdMoveDownLeft");
            if (cmdMoveDownLeft != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveDownLeft, camera.Profile.PtzCommand.DownLeft);

            var cmdMoveStop = Xml.GetFirstElementByTagName(node, "cmdMoveStop");
            if (cmdMoveStop != null) ParseDevicePTZCommandContentProfileFromXml(cmdMoveStop, camera.Profile.PtzCommand.Stop);

            var cmdZoomIn = Xml.GetFirstElementByTagName(node, "cmdZoomIn");
            if (cmdZoomIn != null) ParseDevicePTZCommandContentProfileFromXml(cmdZoomIn, camera.Profile.PtzCommand.ZoomIn);

            var cmdZoomOut = Xml.GetFirstElementByTagName(node, "cmdZoomOut");
            if (cmdZoomOut != null) ParseDevicePTZCommandContentProfileFromXml(cmdZoomOut, camera.Profile.PtzCommand.ZoomOut);

            var cmdZoomStop = Xml.GetFirstElementByTagName(node, "cmdZoomStop");
            if (cmdZoomStop != null) ParseDevicePTZCommandContentProfileFromXml(cmdZoomStop, camera.Profile.PtzCommand.ZoomStop);

            var cmdFocusIn = Xml.GetFirstElementByTagName(node, "cmdFocusIn");
            if (cmdFocusIn != null) ParseDevicePTZCommandContentProfileFromXml(cmdFocusIn, camera.Profile.PtzCommand.FocusIn);

            var cmdFocusOut = Xml.GetFirstElementByTagName(node, "cmdFocusOut");
            if (cmdFocusOut != null) ParseDevicePTZCommandContentProfileFromXml(cmdFocusOut, camera.Profile.PtzCommand.FocusOut);

            var cmdFocusStop = Xml.GetFirstElementByTagName(node, "cmdFocusStop");
            if (cmdFocusStop != null) ParseDevicePTZCommandContentProfileFromXml(cmdFocusStop, camera.Profile.PtzCommand.FocusStop);

            var cmdAddPTZPreset = Xml.GetFirstElementByTagName(node, "cmdAddPTZPreset");
            if (cmdAddPTZPreset != null)
            {
                var points = cmdAddPTZPreset.SelectNodes("Point");
                foreach (XmlElement point in points)
                {
                    if (point.Attributes == null) continue;
                    var id = Convert.ToUInt16(point.Attributes["id"].Value);
                    if (!camera.Profile.PtzCommand.PresetPoints.ContainsKey(id)) continue;
                    ParseDevicePTZCommandContentProfileFromXml(point, camera.Profile.PtzCommand.PresetPoints[id]);
                }
            }

            var cmdDelPTZPreset = Xml.GetFirstElementByTagName(node, "cmdDelPTZPreset");
            if (cmdDelPTZPreset != null)
            {
                var points = cmdDelPTZPreset.SelectNodes("Point");
                foreach (XmlElement point in points)
                {
                    if (point.Attributes == null) continue;
                    var id = Convert.ToUInt16(point.Attributes["id"].Value);
                    if (!camera.Profile.PtzCommand.DeletePresetPoints.ContainsKey(id)) continue;
                    ParseDevicePTZCommandContentProfileFromXml(point, camera.Profile.PtzCommand.DeletePresetPoints[id]);
                }
            }

            var cmdPTZPresetGo = Xml.GetFirstElementByTagName(node, "cmdPTZPresetGo");
            if (cmdPTZPresetGo != null)
            {
                var points = cmdPTZPresetGo.SelectNodes("Point");
                foreach (XmlElement point in points)
                {
                    if (point.Attributes == null) continue;
                    var id = Convert.ToUInt16(point.Attributes["id"].Value);
                    if (!camera.Profile.PtzCommand.GotoPresetPoints.ContainsKey(id)) continue;
                    ParseDevicePTZCommandContentProfileFromXml(point, camera.Profile.PtzCommand.GotoPresetPoints[id]);
                }
            }
        }

        private static void ParseDevicePTZCommandContentProfileFromXml(XmlElement node, PtzCommandCgi cmd)
        {
            if (node == null) return;
            cmd.Method = PtzCommandMethods.ToIndex(Xml.GetFirstElementValueByTagName(node, "Method"));
            cmd.Cgi = Xml.GetFirstElementValueByTagName(node, "Cgi");
            cmd.Parameter = cmd.Method == PtzCommandMethod.Get ? String.Empty : Xml.GetFirstElementValueByTagName(node, "Parameter");
        }

        private static void ParseDeviceCaptureCardProfileFromXml(ICamera camera, XmlElement node)
        {
            camera.Profile.CaptureCardConfig = new CaptureCardConfig();
            XmlNode captureCardConfigNode = node.SelectSingleNode("CaptureCardConfig");
            XmlNode motionRegionsNode = node.SelectSingleNode("MotionRegions");

            if (captureCardConfigNode != null)
            {
                camera.Profile.CaptureCardConfig.Brightness = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Brightness"));
                camera.Profile.CaptureCardConfig.Contrast = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Contrast"));
                camera.Profile.CaptureCardConfig.Hue = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Hue"));
                camera.Profile.CaptureCardConfig.Saturation = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Saturation"));
                camera.Profile.CaptureCardConfig.Sharpness = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Sharpness"));
                camera.Profile.CaptureCardConfig.Gamma = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Gamma"));
                camera.Profile.CaptureCardConfig.ColorEnable = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "ColorEnable"));
                camera.Profile.CaptureCardConfig.WhiteBalance = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "WhiteBalance"));
                camera.Profile.CaptureCardConfig.BacklightCompensation = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "BacklightCompensation"));
                camera.Profile.CaptureCardConfig.Gain = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Gain"));
                camera.Profile.CaptureCardConfig.TemporalSensitivity = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "TemporalSensitivity"));
                camera.Profile.CaptureCardConfig.SpatialSensitivity = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "SpatialSensitivity"));
                camera.Profile.CaptureCardConfig.LevelSensitivity = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "LevelSensitivity"));
                camera.Profile.CaptureCardConfig.Speed = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(captureCardConfigNode, "Speed"));
            }

            if (motionRegionsNode == null) return;

            var regionNodes = motionRegionsNode.SelectNodes("MotionRegion");
            if (regionNodes == null || regionNodes.Count <= 0) return;

            var config = camera.StreamConfig;
            if (config == null) return;

            var width = Resolutions.ToWidth(config.Resolution);
            var height = Resolutions.ToHeight(config.Resolution);

            foreach (XmlElement regionNode in regionNodes)
            {
                var id = Convert.ToUInt16(regionNode.GetAttribute("id"));
                if (camera.MotionThreshold.ContainsKey(id))
                    camera.MotionThreshold[id] = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(regionNode, "Threshold"));
                else
                    camera.MotionThreshold.Add(id, Convert.ToUInt16(Xml.GetFirstElementValueByTagName(regionNode, "Threshold")));

                var start = Xml.GetFirstElementValueByTagName(regionNode, "Start").Split(',');
                var end = Xml.GetFirstElementValueByTagName(regionNode, "End").Split(',');

                if (start.Length != 2 || end.Length != 2) continue;
                var startArr = Array.ConvertAll(start, Convert.ToUInt16);
                var endArr = Array.ConvertAll(end, Convert.ToUInt16);

                var rectangle = new Rectangle
                                    {
                                        X = Convert.ToInt32((startArr[0] / 16.0) * width),
                                        Y = Convert.ToInt32((startArr[1] / 12.0) * height),
                                        Width = Convert.ToInt32(((endArr[0] - startArr[0] + 1) / 16.0) * width),
                                        Height = Convert.ToInt32(((endArr[1] - startArr[1] + 1) / 12.0) * height)
                                    };
                camera.MotionRectangles.Add(id, rectangle);
            }
        }

        private static void ParseDeviceIOPortProfileFromXml(ICamera camera, XmlElement node)
        {
            var ioPortNode = node.SelectSingleNode("IOPort");

            if (ioPortNode != null)
            {
                var ports = ioPortNode.SelectNodes("Port");
                if (ports == null) return;

                foreach (XmlElement port in ports)
                {
                    var id = Convert.ToUInt16(port.GetAttribute("id"));
                    if (camera.IOPort.ContainsKey(id)) continue;

                    camera.IOPort.Add(id, IOPorts.ToIndex(port.InnerText));
                }
            }
            else
            {
                if (camera.Model.IOPorts.Count > 0)
                {
                    foreach (var port in camera.Model.IOPorts)
                    {
                        camera.IOPort.Add(port.Key, port.Value);
                    }
                }
            }
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            if (_saveDeviceFlag)
            {
                _watch.Stop();
                Trace.WriteLine(@"Device Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));
                ReadyStatus = ManagerReadyState.Ready;

                if (OnSaveComplete != null)
                    OnSaveComplete(this, null);
            }
        }

        private Boolean _saveDeviceFlag;

        private void SaveDevices()
        {
            RemoveUnknownDevice();

            SaveDevice();
            SaveEvent();
            SavePresetPoint();
            SavePresetTour();
        }

        private void SaveDevice()
        {
            //Save all
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("AllDevices");
            xmlDoc.AppendChild(xmlRoot);

            var changedDevice = new List<ICamera>();
            foreach (KeyValuePair<UInt16, IDevice> obj in Devices)
            {
                var camera = obj.Value as ICamera;
                if (camera == null) continue;
                if (camera.ReadyState == ReadyState.Ready) continue;

                changedDevice.Add(camera);

                Server.WriteOperationLog("Device [%1] has been modified".Replace("%1", camera.Id.ToString()));

                XmlDocument deviceNode = ParseDeviceProfileToXml(camera);
                if (deviceNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(deviceNode.FirstChild, true));
                camera.ReadyState = ReadyState.Ready;
            }

            if (changedDevice.Count > 0)
            {
                Xml.PostXmlToHttp(CgiSaveAllDevice, xmlDoc, Server.Credential);

                SendSaveCommandDelegate sendSaveCommandDelegate = SendSaveCommand;
                sendSaveCommandDelegate.BeginInvoke(changedDevice, null, null);
            }
        }

        private void SaveDevicesPeopleCounting()
        {
            //Save all
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("AllDevices");
            xmlDoc.AppendChild(xmlRoot);

            if (Devices.Count > 0)
            {
                foreach (var obj in Devices)
                {
                    if (obj.Value.Server == null) continue;
                    obj.Value.ReadyState = ReadyState.Ready;

                    var deviceNode = ParseDevicePeopleCountingToXml(obj.Value);
                    if (deviceNode.FirstChild != null)
                        xmlRoot.AppendChild(xmlDoc.ImportNode(deviceNode.FirstChild, true));
                }
                Xml.PostXmlToHttp(CgiSaveAllDevice, xmlDoc, Server.Credential);
            }
        }

        private delegate void SendSaveCommandDelegate(List<ICamera> changedDevice);
        private void SendSaveCommand(List<ICamera> changedDevice)
        {
            Log.Write("---------------cmdSetVideoConfig----------------", false);
            foreach (var camera in changedDevice)
            {
                if (Devices.ContainsValue(camera))//if it's STILL there,than save. sendCmd will use 10+ sec.
                    Xml.PostTextToHttp(CgiSendCommand.Replace("%1", camera.Id.ToString(CultureInfo.InvariantCulture)), "cmdSetVideoConfig", Server.Credential, SendcommandTimeout);
            }

            Log.Write("-----------------------------------------------\r\n", false);
        }

        protected virtual XmlDocument ParseDeviceProfileToXml(IDevice device)
        {
            var xmlDoc = new XmlDocument();
            if (!(device is ICamera)) return xmlDoc;

            var camera = device as ICamera;
            camera.IsInUse = false;

            var xmlRoot = xmlDoc.CreateElement("DeviceConnectorConfiguration");
            xmlRoot.SetAttribute("id", camera.Id.ToString());
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("DeviceID", camera.Id));
            //xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "LastModified", camera.LastModified.Ticks.ToString()));

            //----------------------------------DeviceSetting
            var deviceSetting = xmlDoc.CreateElement("DeviceSetting");
            xmlRoot.AppendChild(deviceSetting);

            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Brand", camera.Model.Manufacture));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Model", camera.Model.Alias));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Series", camera.Model.Series));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Type", CameraTypes.ToString(camera.Type)));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Mode", CameraModes.ToString(camera.Mode)));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Name", camera.Name));
            switch (camera.Model.Manufacture)
            {
                case "iSapSolution":
                    deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("IPAddress", ""));
                    break;

                default:
                    deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("IPAddress", camera.Profile.NetworkAddress));
                    break;
            }
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Http", camera.Profile.HttpPort));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("AudioOut", camera.Profile.AudioOutPort));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("TVStandard", TvStandards.ToString(camera.Profile.TvStandard)));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("DewarpType", camera.Profile.DewarpType));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("DeviceMountType", DeviceMountTypes.ToString(camera.Profile.DeviceMountType)));
            //move everything about device(different) to devicecab, in order to just replace devicecab.dll can upgrade nvr support device list.
            XMLConvert.ConvertSensorModeToXml(camera, deviceSetting, xmlDoc);

            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("PowerFrequency", PowerFrequencies.ToString(camera.Profile.PowerFrequency)));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("AspectRatioCorrection", (camera.Profile.AspectRatioCorrection ? "true" : "false")));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("AspectRatio", AspectRatios.ToString(camera.Profile.AspectRatio)));

            //move everything about device(different) to devicecab, in order to just replace devicecab.dll can upgrade nvr support device list.
            XMLConvert.ConvertViewingWindowToXml(camera, deviceSetting, xmlDoc);

            //----------------------------------Authentication
            var authentication = xmlDoc.CreateElement("Authentication");
            deviceSetting.AppendChild(authentication);

            authentication.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(camera.Profile.Authentication.UserName)));
            authentication.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(camera.Profile.Authentication.Password)));
            authentication.AppendChild(xmlDoc.CreateXmlElementWithText("Encryption", Encryptions.ToString(camera.Profile.Authentication.Encryption)));
            authentication.AppendChild(xmlDoc.CreateXmlElementWithText("OccupancyPriority", camera.Profile.Authentication.OccupancyPriority));

            //----------------------------------Remote Recovery
            var removeRecovery = xmlDoc.CreateElement("RemoteRecovery");
            deviceSetting.AppendChild(removeRecovery);
            removeRecovery.AppendChild(xmlDoc.CreateXmlElementWithText("Enable", camera.Profile.RemoteRecovery ? "1" : "0"));

            //----------------------------------PTZSupport
            var ptzSupport = xmlDoc.CreateElement("PTZSupport");
            deviceSetting.AppendChild(ptzSupport);

            ptzSupport.AppendChild(xmlDoc.CreateXmlElementWithText("Pan", camera.Model.PanSupport ? "true" : "false"));
            ptzSupport.AppendChild(xmlDoc.CreateXmlElementWithText("Tilt", camera.Model.TiltSupport ? "true" : "false"));
            ptzSupport.AppendChild(xmlDoc.CreateXmlElementWithText("Zoom", camera.Model.ZoomSupport ? "true" : "false"));

            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("ConnectTimeOut", camera.Profile.ConnectionTimeout));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("ReceiveTimeOut", camera.Profile.ReceiveTimeout));
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("URI", ""));

            //-----------------------------------Live Check Command
            var liveCheck = xmlDoc.CreateElement("LiveCheckCommand");
            deviceSetting.AppendChild(liveCheck);
            var liveCheckCmd = xmlDoc.CreateElement("cmdLiveCheck");
            liveCheck.AppendChild(liveCheckCmd);
            liveCheckCmd.AppendChild(xmlDoc.CreateXmlElementWithText("Method", "get"));
            liveCheckCmd.AppendChild(xmlDoc.CreateXmlElementWithText("Cgi", camera.Profile.LiveCheckURI));
            liveCheckCmd.AppendChild(xmlDoc.CreateXmlElementWithText("RetryCount", camera.Profile.LiveCheckRetryCount));
            liveCheckCmd.AppendChild(xmlDoc.CreateXmlElementWithText("RetryInterval", camera.Profile.LiveCheckInterval));

            //-----------------------------------PTZ Command

            XMLConvert.ParsePTZCommand(camera, xmlDoc, deviceSetting);

            //----------------------------------Multi Stream--------------------------------
            var multiStream = xmlDoc.CreateElement("Multi-Stream");
            deviceSetting.AppendChild(multiStream);

            multiStream.AppendChild(xmlDoc.CreateXmlElementWithText("HighProfile", camera.Profile.HighProfile));
            multiStream.AppendChild(xmlDoc.CreateXmlElementWithText("MediumProfile", camera.Profile.MediumProfile));
            multiStream.AppendChild(xmlDoc.CreateXmlElementWithText("LowProfile", camera.Profile.LowProfile));


            //----------------------------------StreamConfig
            ParseStreamConfigToXml(camera, xmlDoc, deviceSetting);

            //----------------------------------Schedule
            String weekSchedule = ConvertScheduleDataToWeekSchedule(camera.RecordSchedule);

            XmlElement schedule = xmlDoc.CreateElement("Schedule");
            xmlRoot.AppendChild(schedule);
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Mon", weekSchedule.Substring(0, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Tue", weekSchedule.Substring(144, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Wed", weekSchedule.Substring(288, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Thu", weekSchedule.Substring(432, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Fri", weekSchedule.Substring(576, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Sat", weekSchedule.Substring(720, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Sun", weekSchedule.Substring(864, 144)));

            //----------------------------------Record
            XmlElement record = xmlDoc.CreateElement("Record");
            xmlRoot.AppendChild(record);

            record.AppendChild(xmlDoc.CreateXmlElementWithText("PreRecord", camera.PreRecord));
            record.AppendChild(xmlDoc.CreateXmlElementWithText("PostRecord", camera.PostRecord));

            //----------------------------------CaptureCardConfig
            if (camera.Model.Type == "CaptureCard")
                ParseCaptureCardSettingToXml(camera, xmlDoc, xmlRoot);

            //----------------------------------IOPorts
            if (camera.Model.IOPortSupport != null)
                ParseIOPortSettingToXml(camera, xmlDoc, xmlRoot);

            //--------------------------------Capability for CMS server
            XmlElement capability = xmlDoc.CreateElement("Capability");
            xmlRoot.AppendChild(capability);

            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfAudioOut", camera.Model.NumberOfAudioOut));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfAudioIn", camera.Model.NumberOfAudioIn));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfMotion", camera.Model.NumberOfMotion));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfChannel", camera.Model.NumberOfChannel));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfDi", camera.Model.NumberOfDi));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("NumberOfDo", camera.Model.NumberOfDo));
            capability.AppendChild(xmlDoc.CreateXmlElementWithText("FocusSupport", camera.Model.FocusSupport ? "true" : "false"));

            //--------------------------------PIP 
            if(Server.Server.SupportPIP && camera.PIPDevice != null)
            {
                XmlElement pip = xmlDoc.CreateElement("PIP");
                xmlRoot.AppendChild(pip);
                pip.AppendChild(xmlDoc.CreateXmlElementWithText("DeviceID", camera.PIPDevice.Id));
                pip.AppendChild(xmlDoc.CreateXmlElementWithText("Stream", camera.PIPStreamId));
                pip.AppendChild(xmlDoc.CreateXmlElementWithText("Position", Positions.ToXMLString(camera.PIPPosition)));
                if (camera.PIPDevice.Profile.StreamConfigs.ContainsKey(camera.PIPStreamId))
                {
                    var pipStreamConfig = camera.PIPDevice.Profile.StreamConfigs[camera.PIPStreamId];
                    var transcoding = xmlDoc.CreateXmlElementWithText("Transcoding", String.Empty);
                    transcoding.SetAttribute("streamtype", pipStreamConfig.Compression == Compression.Mjpeg ? (Compression.Mjpeg).ToString().ToLower() : (Compression.H264).ToString().ToLower());

                    transcoding.SetAttribute("bitrate", pipStreamConfig.Compression != Compression.H264 ? "200000" : (Convert.ToUInt64(Bitrates.ToString(pipStreamConfig.Bitrate))*1000).ToString());
                    pip.AppendChild(transcoding);
                }
            }
            
            //----------------------------Digital PTZ region
            XmlElement ptzRegion = xmlDoc.CreateElement("DigitalPTZRegions");
            ptzRegion.SetAttribute("interval", camera.PatrolInterval.ToString());
            xmlRoot.AppendChild(ptzRegion);
            foreach (KeyValuePair<ushort, WindowPTZRegionLayout> ptzRegionLayout in camera.PatrolPoints)
            {
                if(ptzRegionLayout.Value == null) continue;
                XmlElement region = xmlDoc.CreateElement("DigitalPTZRegion");
                region.SetAttribute("id", ptzRegionLayout.Key.ToString());
                region.SetAttribute("name", ptzRegionLayout.Value.Name);
                var imported = xmlDoc.ImportNode(ptzRegionLayout.Value.RegionXML, true);
                region.AppendChild(imported);
                ptzRegion.AppendChild(region);
            }

            return xmlDoc;
        }

        private static void ParseStreamConfigToXml(ICamera camera, XmlDocument xmlDoc, XmlElement deviceSetting)
        {
            var keys = camera.Profile.StreamConfigs.Keys.ToList();
            keys.Sort();
            Boolean hasStream = false;

            //----------------------------------StreamConfig
            foreach (UInt16 key in keys)
            {
                if (!camera.Profile.StreamConfigs.ContainsKey(key)) continue;

                StreamConfig config = camera.Profile.StreamConfigs[key];
                if (!XMLConvert.CheckIfNeedParseStreamConfig(camera, config)) continue;

                hasStream = true;
                var streamConfig = xmlDoc.CreateElement("StreamConfig");
                streamConfig.SetAttribute("id", key.ToString(CultureInfo.InvariantCulture));
                deviceSetting.AppendChild(streamConfig);

                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("Channel", config.Channel));
                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("DewarpMode", Dewarps.ToString(config.Dewarp)));
                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("Protocol", ConnectionProtocols.ToString(config.ConnectionProtocol)));
                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("SaveReboot", 1));
                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("URI", config.URI));
                streamConfig.AppendChild(xmlDoc.CreateXmlElementWithText("MulticastNetworkAddress", config.MulticastNetworkAddress));

                XMLConvert.ConvertProfileModeToXml(camera, streamConfig, xmlDoc, config);
                //----------------------------------Video
                var video = xmlDoc.CreateElement("Video");
                streamConfig.AppendChild(video);

                video.AppendChild(xmlDoc.CreateXmlElementWithText("Encode", Compressions.ToString(config.Compression)));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Width", Resolutions.ToWidth(config.Resolution)));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Height", Resolutions.ToHeight(config.Resolution)));
                if (Resolutions.ToString(config.Resolution).IndexOf("-") > -1)
                    video.AppendChild(xmlDoc.CreateXmlElementWithText("Resolution", Resolutions.ToString(config.Resolution)));

                XMLConvert.ConvertRegionToXml(camera, video, xmlDoc, config);

                video.AppendChild(xmlDoc.CreateXmlElementWithText("Quality", config.VideoQuality));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Fps", config.Framerate));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Bitrate", Bitrates.ToString(config.Bitrate)));

                //----------------------------------Audio
                var audio = xmlDoc.CreateElement("Audio");
                streamConfig.AppendChild(audio);

                audio.AppendChild(xmlDoc.CreateXmlElementWithText("Input", ""));
                audio.AppendChild(xmlDoc.CreateXmlElementWithText("Output", ""));

                //----------------------------------Port
                var port = xmlDoc.CreateElement("Port");
                streamConfig.AppendChild(port);

                port.AppendChild(xmlDoc.CreateXmlElementWithText("Control", config.ConnectionPort.Control));
                port.AppendChild(xmlDoc.CreateXmlElementWithText("Stream", config.ConnectionPort.Streaming));
                port.AppendChild(xmlDoc.CreateXmlElementWithText("Https", config.ConnectionPort.Https));
                port.AppendChild(xmlDoc.CreateXmlElementWithText("RTSP", config.ConnectionPort.Rtsp));
                port.AppendChild(xmlDoc.CreateXmlElementWithText("VideoIn", config.ConnectionPort.VideoIn));
                port.AppendChild(xmlDoc.CreateXmlElementWithText("AudioIn", config.ConnectionPort.AudioIn));
            }

            //isap cloud camera have no streamId, set as 0
            deviceSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Stream", (hasStream) ? camera.Profile.StreamId : 0));

            XMLConvert.ConvertRecordStreamToXml(camera, deviceSetting, xmlDoc, hasStream);

        }

        private static void ParseCaptureCardSettingToXml(ICamera camera, XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            var config = camera.Profile.CaptureCardConfig;
            if (config == null) return;

            //----------------------------------CaptureCardConfig
            XmlElement captureCard = xmlDoc.CreateElement("CaptureCardConfig");
            xmlRoot.AppendChild(captureCard);

            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Brightness", config.Brightness));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Contrast", config.Contrast));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Hue", config.Hue));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Saturation", config.Saturation));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Sharpness", config.Sharpness));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Gamma", config.Gamma));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("ColorEnable", config.ColorEnable));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("WhiteBalance", config.WhiteBalance));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("BacklightCompensation", config.BacklightCompensation));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Gain", config.Gain));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("TemporalSensitivity", config.TemporalSensitivity));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("SpatialSensitivity", config.SpatialSensitivity));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("LevelSensitivity", config.LevelSensitivity));
            captureCard.AppendChild(xmlDoc.CreateXmlElementWithText("Speed", config.Speed));

            //----------------------------------MotionRegions
            XmlElement motionRegions = xmlDoc.CreateElement("MotionRegions");
            xmlRoot.AppendChild(motionRegions);

            var streamConfig = camera.StreamConfig;
            if (streamConfig != null)
            {
                var width = Resolutions.ToWidth(streamConfig.Resolution) * 1.0;
                var height = Resolutions.ToHeight(streamConfig.Resolution) * 1.0;
                foreach (var pair in camera.MotionRectangles)
                {
                    if (!camera.MotionThreshold.ContainsKey(pair.Key)) continue;
                    //  <MotionRegion id="2">
                    //    <Start>8,4</Start>
                    //    <End>10,6</End>
                    //    <Threshold>20</Threshold>
                    //</MotionRegion>

                    XmlElement motionRegion = xmlDoc.CreateElement("MotionRegion");
                    motionRegions.AppendChild(motionRegion);
                    motionRegion.SetAttribute("id", pair.Key.ToString(CultureInfo.InvariantCulture));

                    var w1 = Math.Min(pair.Value.X, width - 1);
                    var h1 = Math.Min(pair.Value.Y, height - 1);
                    var w2 = Math.Min(pair.Value.X + pair.Value.Width, width - 1);
                    var h2 = Math.Min(pair.Value.Y + pair.Value.Height, height - 1);

                    if (w1 >= w2)
                        w1 = w2 - 1;
                    if (h1 >= h2)
                        h1 = h2 - 1;

                    XmlElement start = xmlDoc.CreateElement("Start");
                    motionRegion.AppendChild(start);
                    start.InnerText = Convert.ToUInt16(Math.Floor((w1 / width) * 16.0)) + "," + Convert.ToUInt16(Math.Floor((h1 / height) * 12.0));

                    XmlElement end = xmlDoc.CreateElement("End");
                    motionRegion.AppendChild(end);
                    end.InnerText = Convert.ToUInt16(Math.Floor((w2 / width) * 16.0)) + "," + Convert.ToUInt16(Math.Floor((h2 / height) * 12.0));

                    motionRegion.AppendChild(xmlDoc.CreateXmlElementWithText("Threshold", camera.MotionThreshold[pair.Key]));
                }
            }
        }

        private static void ParseIOPortSettingToXml(ICamera camera, XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            //----------------------------------IOPorts
            var ports = camera.IOPort;
            XmlElement ioPort = xmlDoc.CreateElement("IOPort");
            ioPort.SetAttribute("configurable", (camera.Model.IOPortConfigurable ? "true" : "false"));

            foreach (var port in ports)
            {
                var portNode = xmlDoc.CreateXmlElementWithText("Port", IOPorts.ToString(port.Value));
                portNode.SetAttribute("id", port.Key.ToString(CultureInfo.InvariantCulture));
                ioPort.AppendChild(portNode);
            }
            xmlRoot.AppendChild(ioPort);
        }

        private XmlDocument ParseDevicePeopleCountingToXml(IDevice device)
        {
            var xmlDoc = new XmlDocument();
            if (!(device is ICamera)) return xmlDoc;
            var camera = device as ICamera;

            IDevice copyFrom = null;
            foreach (var nvr in ((IVAS)Server).NVR.NVRs)
            {
                copyFrom = (from obj in nvr.Value.Device.Devices
                            where (obj.Value is ICamera)
                            where ((ICamera)obj.Value).Profile == camera.Profile
                            select obj.Value).FirstOrDefault();
                if (copyFrom != null) break;
            }

            if (copyFrom == null) return xmlDoc;

            var xmlRoot = xmlDoc.CreateElement("DeviceConnectorConfiguration");
            xmlRoot.SetAttribute("id", device.Id.ToString(CultureInfo.InvariantCulture));
            xmlDoc.AppendChild(xmlRoot);

            //----------------------------------NVR
            var nvrSetting = xmlDoc.CreateElement("NVR");
            nvrSetting.SetAttribute("id", device.Server.Id.ToString(CultureInfo.InvariantCulture));
            xmlRoot.AppendChild(nvrSetting);

            nvrSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Domain", camera.Server.Credential.Domain));
            nvrSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Port", camera.Server.Credential.Port));
            nvrSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(camera.Server.Credential.UserName)));
            nvrSetting.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(camera.Server.Credential.Password)));

            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("DeviceID", copyFrom.Id));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("StreamID", camera.Profile.StreamId));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("FullFrameRate", "true"));

            //----------------------------------Algorithm
            var algorithmSetting = xmlDoc.CreateElement("Algorithm");
            algorithmSetting.SetAttribute("name", "PeopleCounting");
            xmlRoot.AppendChild(algorithmSetting);

            //----------------------------------Rectangles
            var rectanglesSetting = xmlDoc.CreateElement("Rectangles");
            algorithmSetting.AppendChild(rectanglesSetting);

            foreach (PeopleCountingRectangle peopleCountingRectangle in camera.Rectangles)
            {
                XmlElement rectangle = xmlDoc.CreateElement("Rectangle");
                rectanglesSetting.AppendChild(rectangle);

                rectangle.SetAttribute("x", peopleCountingRectangle.Rectangle.X.ToString(CultureInfo.InvariantCulture));
                rectangle.SetAttribute("y", peopleCountingRectangle.Rectangle.Y.ToString(CultureInfo.InvariantCulture));
                rectangle.SetAttribute("width", peopleCountingRectangle.Rectangle.Width.ToString(CultureInfo.InvariantCulture));
                rectangle.SetAttribute("height", peopleCountingRectangle.Rectangle.Height.ToString(CultureInfo.InvariantCulture));

                var startPoint = xmlDoc.CreateElement("StartPoint");
                startPoint.SetAttribute("x", peopleCountingRectangle.StartPoint.X.ToString(CultureInfo.InvariantCulture));
                startPoint.SetAttribute("y", peopleCountingRectangle.StartPoint.Y.ToString(CultureInfo.InvariantCulture));
                rectangle.AppendChild(startPoint);

                var endPoint = xmlDoc.CreateElement("EndPoint");
                endPoint.SetAttribute("x", peopleCountingRectangle.EndPoint.X.ToString(CultureInfo.InvariantCulture));
                endPoint.SetAttribute("y", peopleCountingRectangle.EndPoint.Y.ToString(CultureInfo.InvariantCulture));
                rectangle.AppendChild(endPoint);

                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("In", peopleCountingRectangle.In.ToString()));
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("Out", peopleCountingRectangle.Out.ToString()));

                //FIXED
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("FeatureThreshold", camera.PeopleCountingSetting.FeatureThreshold));
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("FeatureNumberThreshold", camera.PeopleCountingSetting.FeatureNumberThreshold));
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("PersonNumber", camera.PeopleCountingSetting.PersonNumber));
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("DirectNumber", camera.PeopleCountingSetting.DirectNumber));
                rectangle.AppendChild(xmlDoc.CreateXmlElementWithText("FrameIndex", camera.PeopleCountingSetting.FrameIndex));
            }

            //----------------------------------DispatcherConfiguration
            XmlElement dispatcherConfiguration = xmlDoc.CreateElement("DispatcherConfiguration");
            algorithmSetting.AppendChild(dispatcherConfiguration);
            //----------------------------------Schedule

            String weekSchedule = ConvertScheduleDataToWeekSchedule(camera.EventSchedule);
            XmlElement schedule = xmlDoc.CreateElement("Schedule");
            xmlRoot.AppendChild(schedule);
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Mon", weekSchedule.Substring(0, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Tue", weekSchedule.Substring(144, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Wed", weekSchedule.Substring(288, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Thu", weekSchedule.Substring(432, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Fri", weekSchedule.Substring(576, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Sat", weekSchedule.Substring(720, 144)));
            schedule.AppendChild(xmlDoc.CreateXmlElementWithText("Sun", weekSchedule.Substring(864, 144)));

            var retry = xmlDoc.CreateElement("Retry");
            retry.SetAttribute("count", camera.PeopleCountingSetting.Retry.ToString(CultureInfo.InvariantCulture));
            retry.SetAttribute("period", camera.PeopleCountingSetting.Interval.ToString(CultureInfo.InvariantCulture));
            dispatcherConfiguration.AppendChild(retry);
            //----------------------------------Notify
            var notify = xmlDoc.CreateElement("Notify");
            dispatcherConfiguration.AppendChild(notify);

            camera.Dispatcher.Domain = "http://" + camera.Dispatcher.Domain.Replace("http://", "").Replace("HTTP://", "");
            notify.AppendChild(xmlDoc.CreateXmlElementWithText("URI", camera.Dispatcher.Domain));
            notify.AppendChild(xmlDoc.CreateXmlElementWithText("Account", ""));
            notify.AppendChild(xmlDoc.CreateXmlElementWithText("Password", ""));

            return xmlDoc;
        }

        private void RemoveUnknownDevice()
        {
            var unknownDevice = new List<IDevice>();
            foreach (var obj in Devices)
            {
                var camera = obj.Value as ICamera;
                if (camera == null) continue;

                if (camera.Model.Model == "UNKNOWN")
                    unknownDevice.Add(obj.Value);
            }
            if (unknownDevice.Count == 0) return;

            foreach (IDevice device in unknownDevice)
            {
                Devices.Remove(device.Id);
                foreach (var obj in Groups)
                {
                    obj.Value.Items.Remove(device);
                    obj.Value.View.Remove(device);
                }
                foreach (var obj in DeviceLayouts)
                {
                    obj.Value.Items.Remove(device);
                }
            }
        }

        public UInt16 GetNewDeviceId()
        {
            UInt16 max = (Server is IVAS) ? (UInt16)65535 : Server.License.Amount;
            for (UInt16 id = 1; id <= max; id++)
            {
                if (Devices.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }

        public static UInt16 MaximumDeviceGroupsAmount = 100;
        public UInt16 GetNewGroupId()
        {
            for (UInt16 id = 1; id <= MaximumDeviceGroupsAmount; id++)
            {
                if (Groups.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }

        public IDevice FindDeviceById(UInt16 deviceId)
        {
            return Devices.ContainsKey(deviceId) ? Devices[deviceId] : null;
        }

        private static Int32 SortByIp(ICamera x, ICamera y)
        {
            try
            {
                if (!String.IsNullOrEmpty(x.Profile.NetworkAddress) && !String.IsNullOrEmpty(y.Profile.NetworkAddress))
                {
                    IPAddress ipx = IPAddress.Parse(x.Profile.NetworkAddress);
                    IPAddress ipy = IPAddress.Parse(y.Profile.NetworkAddress);

                    Byte[] bytesx = ipx.GetAddressBytes();
                    Byte[] bytesy = ipy.GetAddressBytes();
                    for (int i = 0; i < bytesx.Length; i++)
                    {
                        var result = bytesx[i].CompareTo(bytesy[i]);
                        if (result != 0)
                            return result;
                    }
                }

                if (x.StreamConfig.Channel > y.StreamConfig.Channel)
                    return 1;
                if (y.StreamConfig.Channel > x.StreamConfig.Channel)
                    return -1;

                return 0;
            }
            catch (Exception)
            {
            }
            return 0;
        }

        private static String ConvertIBooleanToString(IEnumerable<Boolean> list)
        {
            var temp = list.Select(dewarp => (dewarp) ? "true" : "false").ToArray();

            return String.Join(",", temp);
        }

        private static IEnumerable<Boolean> ConvertStringToBoolList(String value)
        {
            return value.Split(',').Select(s => s == "true").ToList();
        }

        public String ReadFisheyeVendorByCamera(ICamera camera)
        {
            var vendor = "immervision";
            if (String.IsNullOrEmpty(camera.Profile.DewarpType) && camera.Model.Type == "fisheye")
                vendor = "vivotek";

            if (camera.Model.Manufacture == "ACTi")
            {
                if (camera.Model.LensType.Contains(camera.Profile.DewarpType))
                    vendor = "acti";
            }
            else if (camera.Model.Manufacture == "Axis")
            {
                if (String.IsNullOrEmpty(camera.Profile.DewarpType) && camera.Model.Type == "fisheye")
                    vendor = "axis";
            }

            if (String.IsNullOrEmpty(camera.Profile.DewarpType) && camera.Model.Series == "DynaColor" && camera.Model.Type == "fisheye")
            {
                vendor = camera.Model.Series;
            }

            return vendor;
        }

        public String ReadFisheyeDewarpTypeByCamera(ICamera camera)
        {
            if (String.IsNullOrEmpty(camera.Profile.DewarpType) && camera.Model.Series == "DynaColor" && camera.Model.Type == "fisheye")
            {
                return camera.Model.Alias;
            }

            return camera.Profile.DewarpType;
        }
    }
}