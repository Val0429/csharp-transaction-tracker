using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using Constant;
using Device;
using DeviceCab;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public class NVRManager : INVRManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;
        public event EventHandler OnNVRStatusUpdate;

        private const String CgiLoadCMS = @"cgi-bin/sysconfig?action=loadcms";
        private const String CgiSaveCMS = @"cgi-bin/sysconfig?action=savecms";
        private const String CgiLoadMap = @"cgi-bin/sysconfig?action=loadmap";
        private const String CgiSaveMap = @"cgi-bin/sysconfig?action=savemap";
        private const String CgiUploadMap = @"cgi-bin/sysconfig?action=uploadmap&filename={MAPNAME}";
        private const String CgiGetMap = @"Maps/{MAPNAME}";

        private const String CgiLoadFailover = @"cgi-bin/sysconfig?action=loadfailover";
        private const String CgiSaveFailover = @"cgi-bin/sysconfig?action=savefailover";
        private const String CgiDeletefailover = @"cgi-bin/sysconfig?action=deletefailover";
        private const String CgiLoadEventhandler = @"cgi-bin/sysconfig?action=loadeventhandler";
        private const String CgiSaveEventhandler = @"cgi-bin/sysconfig?action=saveeventhandler";

        private const String CgiLoadDevice = @"cgi-bin/deviceconfig?action=loadalldevice&profile=profile%1";
        private const String CgiSaveDevice = @"cgi-bin/deviceconfig?action=savealldevice&profile=profile%1";
        //private const String CgiDeleteDevice = @"cgi-bin/deviceconfig?action=deletealldevice&profile=profile%1";
        //private const String CgiLoadGroup = @"cgi-bin/deviceconfig?action=loadgroup&profile=profile%1";
        private const String CgiSaveGroup = @"cgi-bin/deviceconfig?action=savegroup&profile=profile%1";

        private const String CgiQuery = @"cgi-bin/failoverserver?action=query";
        private const String CgiSearchNVR = @"cgi-bin/searchdevice?vendor=%1";
        private const String CgiLoadNVR = @"cgi-bin/nvrconfig?action=loadallnvr";
        private const String CgiSaveNVR = @"cgi-bin/nvrconfig?action=saveallnvr";
        private const String CgiDeleteNVR = @"cgi-bin/nvrconfig?action=deleteallnvr";
        private const String CgiLoadNVRDeviceGroup = @"cgi-bin/nvrconfig?action=loadallgroup";
        private const String CgiSaveNVRDeviceGroup = @"cgi-bin/nvrconfig?action=saveallgroup";
        private const String CgiLoadNVRAllDevice = @"cgi-bin/nvrconfig?action=loadalldevice";
        private const String CgiSaveNVRAllDevice = @"cgi-bin/nvrconfig?action=savealldevice";
        private const String CgiDeleteNVRAllDevice = @"cgi-bin/nvrconfig?action=deletealldevice";
        private const String CgiLoadNVRAllDevicePresetPoint = @"cgi-bin/nvrconfig?action=loadpresetlist&nvr=nvr{NVRID}";
        private const String CgiLoadNVRAllDeviceBookmark = @"cgi-bin/bookmark?action=loadallbookmark";
        private const String CgiLoadArchiveServer = @"cgi-bin/sysconfig?action=loadarcserver";
        private const String CgiSaveArchiveServer = @"cgi-bin/sysconfig?action=savearcserver";

        public ManagerReadyState ReadyStatus { get; set; }
        public IServer Server { set; get; }
        public UInt16 MaximunNVRAmount { get; set; }
        public FailoverStatus FailoverStatus { get; private set; }
        public UInt16 SynchronizeProgress { get; private set; }

        public Dictionary<UInt16, INVR> NVRs { get; private set; }
        public Dictionary<String, MapAttribute> Maps { get; private set; }
        public ServerCredential ArchiveServer { get; set; }

        public Dictionary<IDevice, UInt16> DeviceChannelTable { get; private set; }


        public NVRManager()
        {
            ReadyStatus = ManagerReadyState.New;
            FailoverStatus = FailoverStatus.Ping;
            SynchronizeProgress = 0;

            NVRs = new Dictionary<UInt16, INVR>();
            Maps = new Dictionary<String, MapAttribute>();
            DeviceChannelTable = new Dictionary<IDevice, UInt16>();
        }

        public void Initialize()
        {
            MaximunNVRAmount = 200;
        }

        public List<INVR> SearchNVR(String manufacturer)
        {
            var result = new List<INVR>();
            XmlDocument xmlDoc = null;
            if (Server is ICMS)
                xmlDoc = Xml.LoadXmlFromHttp(CgiSearchNVR.Replace("%1", manufacturer), Server.Credential, 60);

            if (xmlDoc == null) return result;

            XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
            foreach (XmlElement nvrNode in nvrNodes)
            {
                var name = nvrNode.GetAttribute("DeviceName");
                var model = nvrNode.GetAttribute("Model");
                var ip = nvrNode.GetAttribute("IP");
                var port = nvrNode.GetAttribute("Port");
                var driver = nvrNode.GetAttribute("Driver");
                INVR nvr = new NVR
                {
                    Id = 0,
                    Name = name,
                    Model = model,
                    Driver = driver,
                    Manufacture = driver,
                    ServerManager = Server
                };

                nvr.ReadyState = ReadyState.NotInUse;
                nvr.Credential.Domain = ip;
                nvr.Credential.Port = Convert.ToUInt16(port);
                nvr.Initialize();
                if (Server is ICMS)
                    nvr.Server.TimeZone = Server.Server.TimeZone;
                result.Add(nvr);
            }

            return result;
        }

        public String Status
        {
            get { return "NVR : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private readonly Stopwatch _watch = new Stopwatch();

        protected Boolean _loadCMSFlag;

        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            _watch.Reset();
            _watch.Start();

            LoadDelegate loadNVRDelegate = LoadNVR;
            loadNVRDelegate.BeginInvoke(LoadCallback, loadNVRDelegate);
        }

        public void Load(String xml)
        {
        }

        public void Save()
        {
            ReadyStatus = ManagerReadyState.Saving;

            _watch.Reset();
            _watch.Start();

            SaveDelegate savNVRDelegate = SaveNVR;
            savNVRDelegate.BeginInvoke(SaveCallback, savNVRDelegate);
        }

        public void Save(String xml)
        {

        }

        protected virtual void LoadNVR()
        {
            NVRs.Clear();

            _loadCMSFlag = false;

            if (Server is IVAS)
            {
                LoadNVRList();
            }
            if (Server is ICMS)
            {
                LoadNVRList();
                LoadNVRDevice();
                LoadDeviceChennel();
                LoadMap();
                LoadArchiveServer();
            }
            else if (Server is IFOS)
            {
                LoadFailoverNVRList();
                UpdateNVRStatus();
            }

            _loadCMSFlag = true;
        }

        //private void LoadAllNVRList()
        //{
        //    //<AllNVR>
        //    //<NVR name="New NVR 1" id="1">
        //    //<Driver>iSap</Driver>
        //    //<Domain>127.0.0.1</Domain>
        //    //<Port>80</Port>
        //    //<Account>zQjDgOyQcPU=</Account>
        //    //<Password>JRp9eL+fp18=</Password>
        //    //<SSLEnable>false</SSLEnable>
        //    //<IsListenEvent>true</IsListenEvent>
        //    //<IsPatrolInclude>true</IsPatrolInclude>
        //    //<Modified>1383707120404</Modified>
        //    //</NVR>
        //    //</AllNVR>

        //    XmlDocument xmlDoc = null;
        //    if (Server is ICMS)
        //        xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential, 60);

        //    if (xmlDoc == null) return;

        //    XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
        //    foreach (XmlElement nvrNode in nvrNodes)
        //    {
        //        var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
        //        var name = nvrNode.GetAttribute("name");
        //        var account = Xml.GetFirstElementValueByTagName(nvrNode, "Account");
        //        var password = Xml.GetFirstElementValueByTagName(nvrNode, "Password");
        //        var domain = Xml.GetFirstElementValueByTagName(nvrNode, "Domain");
        //        var port = Xml.GetFirstElementValueByTagName(nvrNode, "Port");
        //        var driver = Xml.GetFirstElementValueByTagName(nvrNode, "Driver");
        //        var modified = Xml.GetFirstElementValueByTagName(nvrNode, "Modified");

        //        INVR nvr = new NVR
        //        {
        //            Id = id,
        //            Name = name,
        //            Model = driver,
        //            Driver = driver,
        //            ServerManager = Server,
        //            IsListenEvent = (Xml.GetFirstElementValueByTagName(nvrNode, "IsListenEvent") != "false"),
        //            IsPatrolInclude = (Xml.GetFirstElementValueByTagName(nvrNode, "IsPatrolInclude") != "false"),
        //            ModifiedDate = Convert.ToUInt64(modified)
        //        };
        //        nvr.ReadyState = ReadyState.Ready;

        //        nvr.Credential = new ServerCredential
        //        {
        //            Domain = domain.Trim(),
        //            Port = Convert.ToUInt16(port),
        //            UserName = Encryptions.DecryptDES(account),
        //            Password = Encryptions.DecryptDES(password),
        //            SSLEnable = (Xml.GetFirstElementValueByTagName(nvrNode, "SSLEnable") == "true")
        //        };

        //        if (NVRs.ContainsKey(nvr.Id)) continue;
        //        NVRs.Add(nvr.Id, nvr);
        //    }
        //}

        private void LoadArchiveServer()
        {
            //<ArchiveConfig>
            //<Domain>127.0.0.1</Domain>
            //<Port>19999</Port>
            //<Account>zQjDgOyQcPU=</Account>
            //<Password>JRp9eL+fp18=</Password>
            //<SSLEnable>false</SSLEnable>
            //</ArchiveConfig>
            XmlDocument xmlDoc = null;
            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadArchiveServer, Server.Credential);

            if (xmlDoc == null || Xml.GetFirstElementByTagName(xmlDoc, "ArchiveConfig") == null)
            {
                ArchiveServer = new ServerCredential
                {
                    Domain = "127.0.0.1",
                    Port = 19999,
                    UserName = "Admin",
                    Password = "123456",
                    SSLEnable = false
                };
            }
            else
            {
                ArchiveServer = new ServerCredential
                {
                    Domain = Xml.GetFirstElementValueByTagName(xmlDoc, "Domain").Trim(),
                    Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlDoc, "Port")),
                    UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(xmlDoc, "Account")),
                    Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(xmlDoc, "Password")),
                    SSLEnable = (Xml.GetFirstElementValueByTagName(xmlDoc, "SSLEnable") == "true")
                };
            }
        }

        protected void LoadNVRList()
        {
            //<CMS>
            //<NVR id="1" name="gaga">
            //    <Domain>172.16.1.99</Domain>
            //    <Port>88</Port>
            //    <Account>2c5ZjyTLEx0=</Account>
            //    <Password>vNIHQ8oOrg0=</Password>
            //    <SSLEnable>true</SSLEnable>
            //</NVR>
            //</CMS>

            XmlDocument xmlDoc = null;
            if (Server is ICMS)
                xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential);

            if (Server is IVAS)
                xmlDoc = Xml.LoadXmlFromHttp(CgiLoadCMS, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
            foreach (XmlElement nvrNode in nvrNodes)
            {
                INVR nvr = ParseNVR(nvrNode);

                if (NVRs.ContainsKey(nvr.Id)) continue;
                nvr.ReadyState = ReadyState.Ready;
                nvr.NVRStatus = NVRStatus.NoSignal;
                NVRs.Add(nvr.Id, nvr);
            }
        }

        private INVR ParseNVR(XmlElement nvrNode)
        {
            var modified = Xml.GetFirstElementValueByTagName(nvrNode, "Modified");

            var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
            var name = nvrNode.GetAttribute("name");
            var modifiedDate = (String.IsNullOrEmpty(modified)) ? 0 : Convert.ToUInt64(modified);
            var manufacture = Xml.GetFirstElementValueByTagName(nvrNode, "Manufacture");
            var driver = Xml.GetFirstElementValueByTagName(nvrNode, "Driver");

            if (manufacture == "ACTi_E")
            {
                manufacture = "ACTi Enterprise";
            }

            if (driver == "ACTi_E")
            {
                driver = "ACTi Enterprise";
            }

            var credential = new ServerCredential
            {
                Domain = Xml.GetFirstElementValueByTagName(nvrNode, "Domain").Trim(),
                Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "Port")),
                UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Account")),
                Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Password")),
                SSLEnable = (Xml.GetFirstElementValueByTagName(nvrNode, "SSLEnable") == "true")
            };
            var serverPort = String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(nvrNode, "ServerPort")) ? Convert.ToUInt16("8000") : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "ServerPort"));
            var serverStatusCheckInterval = String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(nvrNode, "ServerStatusCheckInterval")) ? Convert.ToUInt16("600") : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "ServerStatusCheckInterval"));
            var isListenEvent = (Xml.GetFirstElementValueByTagName(nvrNode, "IsListenEvent") != "false");
            var isPatrolInclude = (Xml.GetFirstElementValueByTagName(nvrNode, "IsPatrolInclude") != "false");

            var bandwidthBitrate = Bitrate.NA;
            var stream = 1;
            if (Server is ICMS)
            {
                bandwidthBitrate = Bitrates.ToIndex(Xml.GetFirstElementValueByTagName(nvrNode, "BandwidthBitrate"));
                stream = String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(nvrNode, "BandwidthStream"))
                             ? 1
                             : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "BandwidthStream"));
            }

            return CreateNVR(id, name, modifiedDate, manufacture, credential, serverPort, serverStatusCheckInterval, isListenEvent, isPatrolInclude, driver, bandwidthBitrate, (ushort)stream);
        }

        protected virtual INVR CreateNVR(ushort id, String name, ulong modifiedDate, String manufacture, ServerCredential credential, UInt16 serverPort, UInt16 serverStatusCheckInterval, bool isListenEvent, bool isPatrolInclude, String driver = null, Bitrate bitrate = Bitrate.NA, UInt16 stream = (ushort)1)
        {
            INVR nvr = new NVR
            {
                Id = id,
                Name = name,
                Form = Server.Form,
                ModifiedDate = modifiedDate,
                Manufacture = manufacture,
                Credential = credential,
                IsListenEvent = isListenEvent,
                IsPatrolInclude = isPatrolInclude,
                Driver = driver,
                ServerManager = Server,
                ServerPort = serverPort,
                ServerStatusCheckInterval = serverStatusCheckInterval
            };

            if (Server is ICMS || Server is IFOS)
            {
                nvr.Initialize();
                nvr.Server.TimeZone = Server.Server.TimeZone;
            }

            nvr.Configure.BandwidthControlBitrate = bitrate;
            nvr.Configure.BandwidthControlStream = stream;
            nvr.Configure.CustomStreamSetting.Enable = true;
            return nvr;
        }

        public void AddDeviceChannelTable(IDevice device)
        {
            for (var i = 1; i <= Server.License.Maximum; i++)
            {
                if (DeviceChannelTable.ContainsValue((ushort)i)) continue;
                if (DeviceChannelTable.ContainsKey(device)) continue;
                DeviceChannelTable.Add(device, (ushort)i);
                device.ReadyState = ReadyState.JustAdd;
                break;
            }
        }

        public void RemoveDeviceChannelTable(IDevice device)
        {
            foreach (KeyValuePair<IDevice, UInt16> obj in DeviceChannelTable)
            {
                if (device.Server.Id == obj.Key.Server.Id && device.Id == obj.Key.Id)
                {
                    DeviceChannelTable.Remove(obj.Key);
                    break;
                }
            }

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Server.Device.Groups)
            {
                var group = obj.Value;

                var hasDevice = false;
                while (group.Items.Contains(device))
                {
                    hasDevice = true;
                    group.Items.Remove(device);
                }

                while (group.View.Contains(device))
                {
                    hasDevice = true;
                    //刪除之後不往前遞補
                    if (group.Id == 0)//all device 還是要刪除
                    {
                        group.View.Remove(device);
                    }
                    else
                    {
                        group.View[group.View.IndexOf(device)] = null;
                    }
                }

                if (!hasDevice) continue;
            }

            //delete all users' private view to avoid adding back the same channel id will show un-authorized camera
            foreach (KeyValuePair<UInt16, IUser> user in Server.User.Users)
            {
                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in user.Value.DeviceGroups)
                {
                    var group = obj.Value;

                    var hasDevice = false;
                    while (group.Items.Contains(device))
                    {
                        hasDevice = true;
                        group.Items.Remove(device);
                    }

                    while (group.View.Contains(device))
                    {
                        hasDevice = true;
                        //group.View.Remove(device); 刪除之後不往前遞補
                        group.View[group.View.IndexOf(device)] = null;
                    }

                    if (!hasDevice) continue;
                }
            }
        }

        protected void LoadDeviceChennel()
        {
            //<AllNVR>
            //  <NVRGroup id="1">
            //      <CH device_id="4">1</CH>
            //      <CH device_id="3">2</CH>
            //      <CH device_id="2">3</CH>
            //      <CH device_id="1">4</CH>
            //  </NVRGroup>
            //</AllNVR>

            DeviceChannelTable.Clear();
            XmlDocument xmlDoc = null;
            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRDeviceGroup, Server.Credential);
            if (xmlDoc == null)
            {
                return;
            }
            XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVRGroup");
            foreach (XmlElement nvrNode in nvrNodes)
            {
                var nvrId = nvrNode.GetAttribute("id");
                if (String.IsNullOrEmpty(nvrId)) continue;
                if (!NVRs.ContainsKey(Convert.ToUInt16(nvrId))) continue;
                var nvr = NVRs[Convert.ToUInt16(nvrId)];

                XmlNodeList chNodes = nvrNode.GetElementsByTagName("CH");
                foreach (XmlElement chNode in chNodes)
                {
                    var deviceId = chNode.GetAttribute("device_id");
                    if (String.IsNullOrEmpty(deviceId)) continue;
                    if (!nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId))) continue;
                    var device = nvr.Device.Devices[Convert.ToUInt16(deviceId)];
                    var channelId = chNode.InnerText;
                    if (String.IsNullOrEmpty(channelId)) continue;
                    DeviceChannelTable.Add(device, Convert.ToUInt16(channelId));
                }
            }
        }

        protected void LoadNVRDevice()
        {
            XmlDocument xmlDoc = null;
            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRAllDevice, Server.Credential);
            if (xmlDoc == null) return;

            XmlNodeList nvrList = xmlDoc.GetElementsByTagName("NVR");
            foreach (XmlElement nvrNode in nvrList)
            {
                var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
                if (!NVRs.ContainsKey(id)) continue;
                var nvr = NVRs[id];
                nvr.TempDevices.Clear();
                XmlNodeList devicesList = nvrNode.GetElementsByTagName("DeviceConnectorConfiguration");
                foreach (XmlElement node in devicesList)
                {
                    UInt16 channelId = Convert.ToUInt16(node.GetAttribute("id"));
                    String name = node.GetAttribute("name");
                    UInt16 deviceId = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

                    IDevice device = ParseDeviceProfileFromXml(node, nvr);
                    if (device != null && !nvr.Device.Devices.ContainsKey(deviceId))
                    {
                        if (!String.IsNullOrEmpty(name))
                            device.Name = name;
                        //nvr.Device.Groups[0].Items.Add(device);
                        if (!DeviceChannelTable.ContainsKey(device))
                            DeviceChannelTable.Add(device, channelId);
                        nvr.Device.Devices.Add(deviceId, device);
                        nvr.TempDevices.Add(deviceId, device);
                        device.ReadyState = ReadyState.Ready;
                    }
                }
            }

            //LoadNVRDevicePresetPoint();
            LoadNVRDeviceBookmark();
            Server.Device.LoadAllEvent();
        }

        protected virtual IDevice ParseDeviceProfileFromXml(XmlElement node, INVR nvr)
        {
            try
            {
                XmlNode settingNode = node.SelectSingleNode("DeviceSetting");

                if (settingNode == null) return null;

                String manufacture = Xml.GetFirstElementValueByTagName(settingNode, "Brand");
                String model = Xml.GetFirstElementValueByTagName(settingNode, "Model");

                if (model == null) return null;

                String name = Xml.GetFirstElementValueByTagName(settingNode, "Name");
                if (name == "") name = manufacture + " " + model;

                var camera = new Camera
                {
                    Server = nvr,
                    Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID")),
                    Name = name,
                    ReadyState = ReadyState.New,
                    CMS = (ICMS)Server,
                    XmlFromServer = node
                };

                CameraModel cameraModel = new CameraModel
                {
                    Manufacture = manufacture,
                    Model = model
                };

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

                camera.Model = cameraModel;
                camera.Profile = new CameraProfile
                {
                    NetworkAddress = Xml.GetFirstElementValueByTagName(settingNode, "IPAddress"),
                    HighProfile = Convert.ToUInt16(String.IsNullOrEmpty(highProfile) ? "1" : highProfile),
                    MediumProfile = Convert.ToUInt16(String.IsNullOrEmpty(mediumProfile) ? "1" : mediumProfile),
                    LowProfile = Convert.ToUInt16(String.IsNullOrEmpty(lowProfile) ? "1" : lowProfile),
                };

                var streamConfigs = settingNode.SelectNodes("StreamConfig");

                if (streamConfigs != null)
                    if (streamConfigs.Count > 0)
                    {
                        foreach (XmlElement configs in streamConfigs)
                        {
                            XmlNode videoNode = configs.SelectSingleNode("Video");
                            if (videoNode != null)
                            {
                                var quality = Xml.GetFirstElementValueByTagName(videoNode, "Quality");
                                var fps = Xml.GetFirstElementValueByTagName(videoNode, "Fps");
                                camera.Profile.StreamConfigs.Add(Convert.ToUInt16(configs.GetAttribute("id")), new StreamConfig
                                {
                                    Compression = Compressions.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Encode")),
                                    Resolution = Resolutions.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Width") + "x" + Xml.GetFirstElementValueByTagName(videoNode, "Height")),
                                    VideoQuality = String.IsNullOrEmpty(quality) ? (UInt16)60 : Convert.ToUInt16(quality),
                                    Framerate = String.IsNullOrEmpty(fps) ? (UInt16)1 : Convert.ToUInt16(fps),
                                    Bitrate = Bitrates.ToIndex(Xml.GetFirstElementValueByTagName(videoNode, "Bitrate")),
                                });
                            }
                        }
                    }

                //PTZ Command
                var ptzCommand = Xml.GetFirstElementByTagName(settingNode, "PTZSupport");
                if (ptzCommand != null)
                {
                    camera.Model.PanSupport = Xml.GetFirstElementValueByTagName(ptzCommand, "Pan") == "true";
                    camera.Model.TiltSupport = Xml.GetFirstElementValueByTagName(ptzCommand, "Tilt") == "true";
                    camera.Model.ZoomSupport = Xml.GetFirstElementValueByTagName(ptzCommand, "Zoom") == "true";
                    camera.Model.FocusSupport = Xml.GetFirstElementValueByTagName(ptzCommand, "FocusSupport") == "true";
                }

                //Capability
                var capabilityNode = node.SelectSingleNode("Capability");
                if (capabilityNode != null)
                {
                    camera.Model.NumberOfAudioOut = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioOut") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioOut")));
                    camera.Model.NumberOfAudioIn = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioIn") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioIn")));
                    camera.Model.NumberOfMotion = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfMotion") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfMotion")));
                    camera.Model.NumberOfChannel = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfChannel") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfChannel")));
                    camera.Model.NumberOfDi = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDi") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDi")));
                    camera.Model.NumberOfDo = (ushort)(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDo") == null ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDo")));
                }
                camera.DeviceType = DeviceType.Device;
                camera.ReadyState = ReadyState.Ready;
                return camera;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(@"Parse Device XML Error " + exception);
            }

            return null;
        }

        private void LoadNVRDeviceBookmark()
        {
            XmlDocument xmlDoc = null;
            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRAllDeviceBookmark, Server.Credential);
            if (xmlDoc == null) return;

            XmlNodeList nvrList = xmlDoc.GetElementsByTagName("NVR");
            foreach (XmlElement node in nvrList)
            {
                if (node.GetAttribute("id") == "") continue;
                var nvrId = Convert.ToUInt16(node.GetAttribute("id"));
                if (!NVRs.ContainsKey(nvrId)) continue;
                var nvr = NVRs[nvrId];
                XmlNodeList bookmarkList = node.GetElementsByTagName("Bookmarks");
                foreach (XmlElement bookmarkNode in bookmarkList)
                {
                    if (bookmarkNode.GetAttribute("device_id") == "") continue;
                    UInt16 deviceId = Convert.ToUInt16(bookmarkNode.GetAttribute("device_id"));
                    if (!nvr.Device.Devices.ContainsKey(deviceId)) continue;

                    if (nvr.Device.Devices[deviceId] is ICamera)
                    {
                        LoadBookmark((ICamera)(nvr.Device.Devices[deviceId]), bookmarkNode);
                    }
                }
            }
        }

        //<Bookmarks id="1">
        //  <Creator name ="">//DES & BASE64 User Name
        //      <Bookmark>
        //          <DateTime>1234567890</DateTime> //DateTime.Ticks
        //          <CreateDateTime>1234567890</CreateDateTime> //DateTime.Ticks
        //      </Bookmark>
        //  <Creator>
        //</Bookmarks>
        private void LoadBookmark(ICamera camera, XmlElement rootNode)
        {//same as DeviceManager.cs LoadBookmark
            var creators = rootNode.GetElementsByTagName("Creator");
            if (creators.Count <= 0) return;

            foreach (XmlElement node in creators)
            {
                String name = Encryptions.DecryptDES(node.GetAttribute("name"));
                if (name != Server.Credential.UserName) continue;

                XmlNodeList bookmarks = node.GetElementsByTagName("Bookmark");
                foreach (XmlElement bookmark in bookmarks)
                {
                    camera.Bookmarks.Add(new Bookmark
                    {
                        Creator = Server.Credential.UserName,
                        DateTime = new DateTime(Convert.ToInt64(Xml.GetFirstElementValueByTagName(bookmark, "DateTime"))),
                        CreateDateTime = new DateTime(Convert.ToInt64(Xml.GetFirstElementValueByTagName(bookmark, "CreateDateTime"))),
                    });
                }

                camera.Bookmarks.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

                //camera.DescBookmarks = new List<Bookmark>(camera.Bookmarks);
                //camera.DescBookmarks.Reverse();
                break;
            }
        }

        public void LoadNVRDevicePresetPoint(INVR nvr)
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRAllDevicePresetPoint.Replace("{NVRID}", nvr.Id.ToString(CultureInfo.InvariantCulture)), Server.Credential, 10, true, false);
            if (xmlDoc == null) return;

            XmlNodeList devicesList = xmlDoc.GetElementsByTagName("PresetPoints");
            foreach (XmlElement node in devicesList)
            {
                var deviceId = Convert.ToUInt16(node.GetAttribute("id"));
                if (!nvr.Device.Devices.ContainsKey(deviceId)) continue;
                var device = nvr.Device.Devices[deviceId] as ICamera;
                if (device == null) continue;
                XmlNodeList pointList = node.GetElementsByTagName("PresetPoint");
                device.PresetPoints.Clear();
                foreach (XmlElement pointNode in pointList)
                {
                    var presetId = Convert.ToUInt16(pointNode.GetAttribute("id"));
                    var presetName = pointNode.InnerText;
                    device.PresetPoints.Add(presetId, new PresetPoint { Id = presetId, Name = presetName });
                }
                device.IsLoadPresetPoint = true;
            }

            foreach (KeyValuePair<ushort, IDevice> device in nvr.Device.Devices)
            {
                var camera = device.Value as ICamera;
                if (camera == null) return;
                camera.IsLoadPresetPoint = true;
            }
        }

        private void LoadNVRDevicePresetPoint()
        {
            foreach (KeyValuePair<ushort, INVR> nvr in NVRs)
            {
                var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRAllDevicePresetPoint.Replace("{NVRID}", nvr.Key.ToString(CultureInfo.InvariantCulture)), Server.Credential, 10, true, false);
                if (xmlDoc == null) return;

                XmlNodeList devicesList = xmlDoc.GetElementsByTagName("PresetPoints");
                foreach (XmlElement node in devicesList)
                {
                    var deviceId = Convert.ToUInt16(node.GetAttribute("id"));
                    if (!nvr.Value.Device.Devices.ContainsKey(deviceId)) continue;
                    var device = nvr.Value.Device.Devices[deviceId] as ICamera;
                    if (device == null) continue;
                    XmlNodeList pointList = node.GetElementsByTagName("PresetPoint");
                    device.PresetPoints.Clear();
                    foreach (XmlElement pointNode in pointList)
                    {
                        var presetId = Convert.ToUInt16(pointNode.GetAttribute("id"));
                        var presetName = pointNode.InnerText;
                        device.PresetPoints.Add(presetId, new PresetPoint { Id = presetId, Name = presetName });
                    }
                }
            }
        }

        private void LoadFailoverNVRList()
        {
            //<Failover>
            //<NVR id="1" name="New NVR 1">
            //    <Domain>172.16.1.87</Domain>
            //    <Port>80</Port>
            //    <Account>zQjDgOyQcPU=</Account>
            //    <Password>JRp9eL+fp18=</Password>
            //    <FailoverPort>8801</FailoverPort>
            //    <LaunchTime>12000</LaunchTime>
            //    <PingTime>4000</PingTime>
            //    <BlockSize>4194304</BlockSize>
            //</NVR>
            //<EventHandlerConfiguration>	
            //<Event>
            //    <Description>Network Loss</Description>
            //    <Conditions operation="and" dwell="60" interval="0">
            //        <Condition type="NVRFail" id="1" value="1" trigger="1" interval="0"/>
            //    </Conditions>		
            //    <SendMail>
            //        <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //        <Subject>NVR: 01 Parise 172.16.1.191 - Event: Fail</Subject> 
            //        <Body>NVR: 01 Parise 172.16.1.191 Event: Fail Server: 172.16.1.80</Body> 
            //        <Attach>false</Attach> 
            //        <AttachSource />
            //    </SendMail>
            //</Event>
            //</EventHandlerConfiguration>	
            //</Failover>

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadFailover, Server.Credential);

            if (xmlDoc == null) return;

            var nvrNodes = xmlDoc.GetElementsByTagName("NVR");

            foreach (XmlElement nvrNode in nvrNodes)
            {
                var failoverSetting = new FailoverSetting
                {
                    FailoverPort = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "FailoverPort")),
                    LaunchTime = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(nvrNode, "LaunchTime")),
                    PingTime = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(nvrNode, "PingTime")),
                    BlockSize = Convert.ToUInt32(Xml.GetFirstElementValueByTagName(nvrNode, "BlockSize")),
                };

                INVR nvr = ParseNVR(nvrNode);
                nvr.FailoverSetting = failoverSetting;

                if (NVRs.ContainsKey(nvr.Id)) continue;

                NVRs.Add(nvr.Id, nvr);
            }

            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadEventhandler, Server.Credential);

            if (xmlDoc == null) return;

            var eventsList = xmlDoc.GetElementsByTagName("Event");
            foreach (XmlElement node in eventsList)
                LoadEvent(node);

            ParseMailServerFromXml(Xml.GetFirstElementByTagName(xmlDoc, "Mail"));
        }

        private void ParseMailServerFromXml(XmlElement node)
        {
            //<Mail>
            //  <Sender>Deray</Sender>
            //  <MailAddress>deray@deray.org</MailAddress>
            //  <Server>mail.deray.org</Server>
            //  <Security>PLAIN</Security>
            //  <Port>25</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Mail>
            if (node == null || Server.Configure == null) return;

            Server.Configure.MailServer.Sender = Xml.GetFirstElementValueByTagName(node, "Sender");
            Server.Configure.MailServer.MailAddress = Xml.GetFirstElementValueByTagName(node, "MailAddress");
            String security = Xml.GetFirstElementValueByTagName(node, "Security");

            if (Enum.IsDefined(typeof(SecurityType), security))
                Server.Configure.MailServer.Security = (SecurityType)Enum.Parse(typeof(SecurityType), security, true);

            String port = Xml.GetFirstElementValueByTagName(node, "Port");
            if (port != "")
                Server.Configure.MailServer.Port = Convert.ToUInt16(port);

            Server.Configure.MailServer.Credential.Domain = Xml.GetFirstElementValueByTagName(node, "Server").Trim();
            Server.Configure.MailServer.Credential.UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Account"));
            Server.Configure.MailServer.Credential.Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Password"));
        }

        private XmlElement ParseMailServerToXml(XmlDocument xmlDoc)
        {
            //<Mail>
            //  <Sender>Deray</Sender>
            //  <MailAddress>deray@deray.org</MailAddress>
            //  <Server>mail.deray.org</Server>
            //  <Security>PLAIN</Security>
            //  <Port>25</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Mail>
            XmlElement node = xmlDoc.CreateElement("Mail");

            node.AppendChild(xmlDoc.CreateXmlElementWithText("Sender", Server.Configure.MailServer.Sender));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("MailAddress", Server.Configure.MailServer.MailAddress));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Server", Server.Configure.MailServer.Credential.Domain));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Security", Server.Configure.MailServer.Security.ToString()));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Port", Server.Configure.MailServer.Port.ToString(CultureInfo.InvariantCulture)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(Server.Configure.MailServer.Credential.UserName)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(Server.Configure.MailServer.Credential.Password)));

            return node;
        }

        private void LoadEvent(XmlElement eventNode)
        {
            if (eventNode == null) return;

            var conditionNodes = eventNode.GetElementsByTagName("Condition");

            List<EventHandle> eventHandle = null;
            if (conditionNodes.Count == 0) return;

            foreach (XmlElement conditionNode in conditionNodes)
            {
                String type = conditionNode.GetAttribute("type");

                if (!Enum.IsDefined(typeof(EventType), type)) continue;

                var failoverEvent = new CameraEvent
                {
                    Type = (EventType)Enum.Parse(typeof(EventType), type, true)
                };

                String interval = conditionNode.GetAttribute("interval");

                foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in ((IFOS)Server).EventHandling)
                {
                    if (obj.Key.CameraEvent.Type == failoverEvent.Type)
                    {
                        if (interval != "")
                            obj.Key.Interval = Convert.ToUInt16(interval);
                        eventHandle = obj.Value;
                        break;
                    }
                }
            }

            if (eventHandle == null)
                return;

            var mailNodes = eventNode.GetElementsByTagName("SendMail");
            if (mailNodes.Count > 0)
            {
                foreach (XmlElement mailNode in mailNodes)
                {
                    SendMailHandleFromXml(mailNode, eventHandle);
                }
            }
        }

        private static void SendMailHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
        {
            //<SendMail>
            //  <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //  <Subject>NVR: 01 Parise 172.16.1.191 - Event: Fail</Subject> 
            //  <Body>NVR: 01 Parise 172.16.1.191 Event: Fail Server: 172.16.1.80</Body> 
            //  <Attach>false</Attach> 
            //  <AttachSource />
            //</SendMail>

            var handle = new SendMailEventHandle
            {
                MailReceiver = Xml.GetFirstElementValueByTagName(node, "Recipient"),
                Subject = "",//Xml.GetFirstElementsValueByTagName(node, "Subject"),
                Body = "",//Xml.GetFirstElementsValueByTagName(node, "Body"),
                ReadyState = ReadyState.Ready,
            };
            eventHandle.Add(handle);
        }

        protected Boolean _saveNVRFlag;

        private XmlDocument _saveXmlDoc = new XmlDocument();
        protected virtual void SaveNVR()
        {
            _saveNVRFlag = false;
            if (Server is ICMS)
            {
                var originalXml = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential);
                if (originalXml != null)
                {
                    var removedNVR = new List<String>();
                    var nvrNodes = originalXml.GetElementsByTagName("NVR");
                    foreach (XmlElement nvrNode in nvrNodes)
                    {
                        var nvrId = Convert.ToUInt16(nvrNode.GetAttribute("id"));
                        if (!NVRs.ContainsKey(nvrId))
                            removedNVR.Add(nvrId.ToString(CultureInfo.InvariantCulture));
                    }

                    var nvrXml = ParseNVRToXml();
                    if (Xml.GetFirstElementByTagName(nvrXml, "NVR") != null)
                        Xml.PostXmlToHttp(CgiSaveNVR, nvrXml, Server.Credential);

                    var deviceXml = ParseNVRAllDeviceToXml();
                    if (Xml.GetFirstElementByTagName(deviceXml, "NVR") != null)
                    {
                        Xml.PostXmlToHttp(CgiSaveNVRAllDevice, deviceXml, Server.Credential);

                        OnDeviceChanged(EventArgs.Empty);
                    }

                    if (Xml.GetFirstElementByTagName(nvrXml, "NVR") != null || Xml.GetFirstElementByTagName(deviceXml, "NVR") != null)
                        Xml.PostXmlToHttp(CgiSaveNVRDeviceGroup, ParseNVRDeviceGroupToXml(), Server.Credential);

                    if (removedNVR.Count > 0)
                        Xml.PostXmlToHttp(CgiDeleteNVR, ParseRemovedNVRToXml(removedNVR), Server.Credential);

                    Server.Utility.GetAllNVRStatus();
                }

                var originalArchiveServerConfig = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential);
                var archiveServerConfig = ParseArchiveServerToXml();
                if (!String.Equals(originalArchiveServerConfig.InnerXml, archiveServerConfig.InnerXml))
                {
                    Xml.PostXmlToHttp(CgiSaveArchiveServer, archiveServerConfig, Server.Credential);
                }

                //LoadNVRDevicePresetPoint();
            }
            if (Server is IVAS)
            {
                Xml.PostXmlToHttp(CgiSaveCMS, ParseNVRToXml(), Server.Credential);
            }
            if (Server is IFOS)
            {
                DeleteUnusedNVR();
                //convert nvr to xml, save it until everything is done(mostly is device save)
                _saveXmlDoc = ParseFailoverNVRToXml();

                //parse event
                Xml.PostXmlToHttp(CgiSaveEventhandler, ParseEventHandleToXml(Server as IFOS), Server.Credential);
                UpdateNVRStatus();
                //Xml.PostXmlToHttp(CgiSaveFailover, ParseFailoverNVRToXml(), Server.Credential);
                //UpdateNVRStatus();
            }

            _saveNVRFlag = true;
        }

        private XmlDocument ParseArchiveServerToXml()
        {
            var saveDoc = new XmlDocument();
            var xmlRoot = saveDoc.CreateElement("ArchiveConfig");
            xmlRoot.AppendChild(saveDoc.CreateXmlElementWithText("Domain", ArchiveServer.Domain));
            xmlRoot.AppendChild(saveDoc.CreateXmlElementWithText("Port", ArchiveServer.Port));
            xmlRoot.AppendChild(saveDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(ArchiveServer.UserName)));
            xmlRoot.AppendChild(saveDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(ArchiveServer.Password)));
            xmlRoot.AppendChild(saveDoc.CreateXmlElementWithText("SSLEnable", ArchiveServer.SSLEnable ? "true" : "false"));
            saveDoc.AppendChild(xmlRoot);
            return saveDoc;
        }

        private XmlDocument ParseRemovedNVRToXml(List<String> removedNVRId)
        {
            var saveDoc = new XmlDocument();
            var idString = String.Join(",", removedNVRId.ToArray());
            saveDoc.AppendChild(saveDoc.CreateXmlElementWithText("ID", idString));
            return saveDoc;
        }

        private XmlDocument ParseNVRDeviceGroupToXml()
        {
            var saveDoc = new XmlDocument();
            var xmlRoot = saveDoc.CreateElement("AllNVR");
            var sortValue = NVRs.Values.OrderBy(x => x.Id);

            foreach (INVR nvr in sortValue)
            {
                var nvrNode = saveDoc.CreateElement("NVRGroup");
                nvrNode.SetAttribute("id", nvr.Id.ToString(CultureInfo.InvariantCulture));

                var sortResult = new List<IDevice>(nvr.Device.Devices.Values);
                foreach (IDevice device in sortResult.OrderBy(x => x.Id))
                {
                    if (!DeviceChannelTable.ContainsKey(device)) continue;

                    var deviceNode = saveDoc.CreateXmlElementWithText("CH", DeviceChannelTable[device]);
                    deviceNode.SetAttribute("device_id", device.Id.ToString(CultureInfo.InvariantCulture));
                    nvrNode.AppendChild(deviceNode);
                }

                xmlRoot.AppendChild(nvrNode);
            }
            saveDoc.AppendChild(xmlRoot);
            return saveDoc;
        }

        private XmlDocument ParseNVRAllDeviceToXml()
        {
            var saveDoc = new XmlDocument();
            var xmlRoot = saveDoc.CreateElement("AllNVR");
            //var licenseId = 0;
            foreach (KeyValuePair<UInt16, INVR> nvr in NVRs)
            {
                if (nvr.Value.ReadyState == ReadyState.Ready)
                {
                    foreach (IDevice device in nvr.Value.Device.Devices.Values)
                    {
                        device.ReadyState = ReadyState.Ready;
                    }
                    continue;
                }
                var nvrNode = saveDoc.CreateElement("NVR");
                nvrNode.SetAttribute("id", nvr.Key.ToString(CultureInfo.InvariantCulture));

                var allDeviceNode = saveDoc.CreateElement("AllDevices");

                var sortResult = new List<IDevice>(nvr.Value.Device.Devices.Values);
                sortResult.Sort((x, y) => (y.Id - x.Id));

                foreach (IDevice device in sortResult)
                {
                    var camera = (ICamera)device;
                    if (camera == null) continue;
                    if (camera.ReadyState == ReadyState.Ready) continue;
                    if (!DeviceChannelTable.ContainsKey(device)) continue;
                    //licenseId++;

                    var cameraNode = camera.XmlFromServer;
                    //var deviceConnectorConfigurationNode = cameraNode.SelectSingleNode("DeviceConnectorConfiguration") as XmlElement;

                    if (cameraNode == null)
                    {
                        var config = saveDoc.CreateElement("DeviceConnectorConfiguration");
                        if (nvr.Value.Manufacture == "iSAP Failover Server") //make a temp data for fialover server
                        {
                            config.SetAttribute("id", DeviceChannelTable[device].ToString(CultureInfo.InvariantCulture));
                            config.SetAttribute("name", camera.Name);

                            config.AppendChild(saveDoc.CreateXmlElementWithText("DeviceID", device.Id));

                            var setting = saveDoc.CreateElement("DeviceSetting");
                            setting.AppendChild(saveDoc.CreateXmlElementWithText("Brand", camera.Model.Manufacture));
                            setting.AppendChild(saveDoc.CreateXmlElementWithText("Model", camera.Model.Model));
                            setting.AppendChild(saveDoc.CreateXmlElementWithText("Name", camera.Name));
                            setting.AppendChild(saveDoc.CreateXmlElementWithText("AudioOut", 0));
                            setting.AppendChild(saveDoc.CreateXmlElementWithText("IPAddress", camera.Profile.NetworkAddress));

                            var ptzSupport = saveDoc.CreateElement("PTZSupport");
                            ptzSupport.AppendChild(saveDoc.CreateXmlElementWithText("Pan", camera.Model.PanSupport ? "true" : "false"));
                            ptzSupport.AppendChild(saveDoc.CreateXmlElementWithText("Tilt", camera.Model.TiltSupport ? "true" : "false"));
                            ptzSupport.AppendChild(saveDoc.CreateXmlElementWithText("Zoom", camera.Model.ZoomSupport ? "true" : "false"));
                            setting.AppendChild(ptzSupport);

                            //----------------------------------Multi Stream--------------------------------
                            var multiStream = saveDoc.CreateElement("Multi-Stream");
                            multiStream.AppendChild(saveDoc.CreateXmlElementWithText("HighProfile", camera.Profile.HighProfile));
                            multiStream.AppendChild(saveDoc.CreateXmlElementWithText("MediumProfile", camera.Profile.MediumProfile));
                            multiStream.AppendChild(saveDoc.CreateXmlElementWithText("LowProfile", camera.Profile.LowProfile));
                            setting.AppendChild(multiStream);

                            //----------------------------------StreamConfig
                            ParseStreamConfigToXml(camera, saveDoc, setting);

                            config.AppendChild(setting);
                            //----------------------------------Capability
                            XmlElement capability = saveDoc.CreateElement("Capability");

                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfAudioOut", camera.Model.NumberOfAudioOut));
                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfAudioIn", camera.Model.NumberOfAudioIn));
                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfMotion", camera.Model.NumberOfMotion));
                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfChannel", camera.Model.NumberOfChannel));
                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfDi", camera.Model.NumberOfDi));
                            capability.AppendChild(saveDoc.CreateXmlElementWithText("NumberOfDo", camera.Model.NumberOfDo));
                            config.AppendChild(capability);

                            allDeviceNode.AppendChild(config);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        cameraNode.SetAttribute("id", DeviceChannelTable[device].ToString(CultureInfo.InvariantCulture));
                        cameraNode.SetAttribute("name", camera.Name);
                        var imported = saveDoc.ImportNode(cameraNode, true);
                        allDeviceNode.AppendChild(imported);
                    }

                    camera.ReadyState = ReadyState.Ready;
                    camera.IsLoadPresetPoint = false;
                }

                nvrNode.AppendChild(allDeviceNode);
                xmlRoot.AppendChild(nvrNode);
                nvr.Value.ReadyState = ReadyState.Ready;
            }
            saveDoc.AppendChild(xmlRoot);
            return saveDoc;
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
                hasStream = true;
                var streamConfig = xmlDoc.CreateElement("StreamConfig");
                streamConfig.SetAttribute("id", key.ToString(CultureInfo.InvariantCulture));
                deviceSetting.AppendChild(streamConfig);

                //----------------------------------Video
                var video = xmlDoc.CreateElement("Video");
                streamConfig.AppendChild(video);

                video.AppendChild(xmlDoc.CreateXmlElementWithText("Encode", Compressions.ToString(config.Compression)));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Width", Resolutions.ToWidth(config.Resolution)));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Height", Resolutions.ToHeight(config.Resolution)));

                XMLConvert.ConvertRegionToXml(camera, video, xmlDoc, config);

                video.AppendChild(xmlDoc.CreateXmlElementWithText("Quality", config.VideoQuality));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Fps", config.Framerate));
                video.AppendChild(xmlDoc.CreateXmlElementWithText("Bitrate", Bitrates.ToString(config.Bitrate)));
            }

            XMLConvert.ConvertRecordStreamToXml(camera, deviceSetting, xmlDoc, hasStream);
        }

        private void DeleteUnusedNVR()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadFailover, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
            var ids = new List<String>();
            foreach (XmlElement nvrNode in nvrNodes)
            {
                var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
                if (!NVRs.ContainsKey(id) || NVRs[id].ReadyState == ReadyState.New)
                    ids.Add(nvrNode.GetAttribute("id"));
            }

            if (ids.Count > 0)
            {
                xmlDoc = new XmlDocument();
                var xmlRoot = xmlDoc.CreateElement("ID");
                xmlRoot.InnerText = String.Join(",", ids.ToArray());
                xmlDoc.AppendChild(xmlRoot);
                Xml.PostXmlToHttp(CgiDeletefailover, xmlDoc, Server.Credential);
            }
        }

        private Dictionary<UInt16, List<IDevice>> _failoverDevieList = new Dictionary<UInt16, List<IDevice>>();
        public Dictionary<UInt16, List<IDevice>> ReadFailoverAllDeviceList()
        {
            _failoverDevieList.Clear();
            foreach (KeyValuePair<UInt16, INVR> nvr in NVRs)
            {
                if (nvr.Value.ReadyState != ReadyState.Ready) return _failoverDevieList;
                if (nvr.Value.Manufacture != "iSAP Failover Server") continue;

                _failoverDevieList.Add(nvr.Key, nvr.Value.ReadDeviceList());
            }
            return _failoverDevieList;
        }

        public void UpdateFailoverDeviceList(UInt16 nvrId, INVR nvr)
        {
            if (nvr.ReadyState != ReadyState.Ready) return;

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadDevice.Replace("%1", nvrId.ToString(CultureInfo.InvariantCulture)), Server.Credential);
            var nvrXmlDoc = nvr.Device.LoadAllDeviceAsXmlDocument();
            if (xmlDoc == null || nvrXmlDoc == null) return;

            SaveNVRDevices(nvrId, xmlDoc, nvrXmlDoc);
            SaveNVRDeviceGroup(nvrId, nvrXmlDoc);
        }

        public void SaveNVRDocument()
        {
            if (_saveXmlDoc != null && _saveXmlDoc.GetElementsByTagName("NVR").Count > 0)
                Xml.PostXmlToHttp(CgiSaveFailover, _saveXmlDoc, Server.Credential);
            _saveXmlDoc = null;
            UpdateNVRStatus();
        }

        private void SaveNVRDevices(UInt16 nvrId, XmlDocument xmlDoc, XmlDocument nvrXmlDoc)
        {
            var devicesList = xmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");
            var nvrDeviceList = nvrXmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");

            var devicesNodes = new Dictionary<UInt16, XmlNode>();
            var nvrDevicesNodes = new Dictionary<UInt16, XmlNode>();

            foreach (XmlNode node in devicesList)
            {
                UInt16 id = Convert.ToUInt16(((XmlElement)node).GetAttribute("id"));
                if (devicesNodes.ContainsKey(id)) continue;
                devicesNodes.Add(id, node);
            }

            foreach (XmlNode node in nvrDeviceList)
            {
                UInt16 id = Convert.ToUInt16(((XmlElement)node).GetAttribute("id"));
                if (nvrDevicesNodes.ContainsKey(id)) continue;
                nvrDevicesNodes.Add(id, node);
            }

            var saveDoc = new XmlDocument();
            var xmlRoot = saveDoc.CreateElement("AllDevices");
            saveDoc.AppendChild(xmlRoot);
            foreach (var obj in nvrDevicesNodes)
            {
                //if (devicesNodes.ContainsKey(obj.Key))
                //{
                //    if (String.Equals(Xml.GetFirstElementValueByTagName(obj.Value, "LastModified"), Xml.GetFirstElementValueByTagName(devicesNodes[obj.Key], "LastModified")))
                //        continue;
                //}
                //if(obj.Key != nvrId) continue;
                var node = saveDoc.CreateElement("DeviceConnectorConfiguration");
                node.SetAttribute("id", obj.Key.ToString(CultureInfo.InvariantCulture));
                node.InnerXml = obj.Value.InnerXml;
                xmlRoot.AppendChild(node);
            }

            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveDevice.Replace("%1", nvrId.ToString(CultureInfo.InvariantCulture)), saveDoc, Server.Credential);
        }

        private void SaveNVRDeviceGroup(UInt16 nvrId, XmlDocument nvrXmlDoc)
        {
            var nvrDeviceList = nvrXmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");

            var ids = (from XmlElement node in nvrDeviceList select node.GetAttribute("id")).ToArray();
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            var groupNode = xmlDoc.CreateElement("Group");
            groupNode.SetAttribute("id", "0");
            xmlRoot.AppendChild(groupNode);

            groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Name", "All Devices"));
            groupNode.AppendChild(xmlDoc.CreateXmlElementWithText("Items", String.Join(",", ids)));

            Xml.PostXmlToHttp(CgiSaveGroup.Replace("%1", nvrId.ToString()), xmlDoc, Server.Credential);
        }

        protected virtual XmlDocument ParseNVRToXml()
        {
            var server = Server as ICMS;
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement(Server is ICMS ? "AllNVR" : "CMS");
            xmlDoc.AppendChild(xmlRoot);

            var nvrSortResult = NVRs.Values.OrderBy(nvr => nvr.Id);

            foreach (INVR nvr in nvrSortResult)
            {
                if (nvr.ReadyState == ReadyState.Ready) continue;

                if (nvr.ReadyState == ReadyState.New)
                {
                    nvr.ReadyState = ReadyState.JustAdd;
                    server.NVRModify(nvr);
                }

                var nvrNode = xmlDoc.CreateElement("NVR");
                nvrNode.SetAttribute("id", nvr.Id.ToString(CultureInfo.InvariantCulture));
                nvrNode.SetAttribute("name", nvr.Name);

                if (nvr.ReadyState != ReadyState.Ready)
                    nvr.ModifiedDate = DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone);

                if (Server is ICMS)
                {
                    var driver = nvr.Driver;
                    switch (nvr.Driver)
                    {
                        case "ACTi Enterprise":
                            driver = "ACTi_E";
                            break;

                        case "Diviotec":
                        case "3TSmart":
                        case "Siemens":
                        case "Certis":
                        case "Customization":
                            driver = "iSap";
                            break;
                    }
                    nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Driver", driver));
                    nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Manufacture", nvr.Manufacture == "ACTi Enterprise" ? "ACTi_E" : nvr.Manufacture));
                }

                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Domain", nvr.Credential.Domain));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Port", nvr.Credential.Port.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("ServerPort", nvr.ServerPort.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("ServerStatusCheckInterval", nvr.ServerStatusCheckInterval.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(nvr.Credential.UserName)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(nvr.Credential.Password)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("SSLEnable", nvr.Credential.SSLEnable ? "true" : "false"));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("IsListenEvent", nvr.IsListenEvent ? "true" : "false"));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("IsPatrolInclude", nvr.IsPatrolInclude ? "true" : "false"));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Modified", nvr.ModifiedDate));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("BandwidthBitrate", Server.Configure.EnableBandwidthControl ? Bitrates.ToString(nvr.Configure.BandwidthControlBitrate) : Bitrates.ToString(Bitrate.NA)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("BandwidthStream", Server.Configure.EnableBandwidthControl ? nvr.Configure.BandwidthControlStream.ToString() : "1"));
                xmlRoot.AppendChild(nvrNode);
            }

            return xmlDoc;
        }

        private XmlDocument ParseFailoverNVRToXml()
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Failover");
            xmlDoc.AppendChild(xmlRoot);

            var nvrSortResult = NVRs.Values.Where(n => n.ReadyState != ReadyState.Ready).OrderBy(n => n.Id);

            foreach (INVR nvr in nvrSortResult)
            {
                var nvrNode = xmlDoc.CreateElement("NVR");
                nvrNode.SetAttribute("id", nvr.Id.ToString(CultureInfo.InvariantCulture));
                nvrNode.SetAttribute("name", nvr.Name);
                nvr.ModifiedDate = DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone);

                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Domain", nvr.Credential.Domain));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Port", nvr.Credential.Port.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(nvr.Credential.UserName)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(nvr.Credential.Password)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("FailoverPort", nvr.FailoverSetting.FailoverPort.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("LaunchTime", nvr.FailoverSetting.LaunchTime.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("PingTime", nvr.FailoverSetting.PingTime.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("BlockSize", nvr.FailoverSetting.BlockSize.ToString(CultureInfo.InvariantCulture)));
                nvrNode.AppendChild(xmlDoc.CreateXmlElementWithText("Modified", nvr.ModifiedDate));

                xmlRoot.AppendChild(nvrNode);
            }

            return xmlDoc;
        }

        private XmlDocument ParseEventHandleToXml(IFOS fos)
        {
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("EventHandlerConfiguration");
            xmlDoc.AppendChild(xmlRoot);

            if (fos == null) return xmlDoc;

            xmlRoot.AppendChild(ParseMailServerToXml(xmlDoc));

            foreach (var obj in ((IFOS)Server).EventHandling)
            {
                var eventNode = xmlDoc.CreateElement("Event");
                xmlRoot.AppendChild(eventNode);

                eventNode.AppendChild(xmlDoc.CreateXmlElementWithText("Description", obj.Key.CameraEvent.ToString()));

                var conditionsNode = xmlDoc.CreateElement("Conditions");
                conditionsNode.SetAttribute("operation", "and");
                conditionsNode.SetAttribute("dwell", "60");
                conditionsNode.SetAttribute("interval", obj.Key.Interval.ToString(CultureInfo.InvariantCulture));
                conditionsNode.AppendChild(ParseConditionToXml(xmlDoc, obj.Key));
                eventNode.AppendChild(conditionsNode);

                //Handle
                var eventHandles = obj.Value;
                foreach (var eventHandle in eventHandles)
                {
                    switch (eventHandle.Type)
                    {
                        case HandleType.SendMail:
                            SendMailHandleToXml(xmlDoc, (SendMailEventHandle)eventHandle, obj.Key, eventNode);
                            break;
                    }

                    eventHandle.ReadyState = ReadyState.Ready;
                }
            }

            return xmlDoc;
        }

        private static XmlElement ParseConditionToXml(XmlDocument xmlDoc, EventCondition eventCondition)
        {
            var conditionNode = xmlDoc.CreateElement("Condition");

            conditionNode.SetAttribute("type", eventCondition.CameraEvent.Type.ToString());

            switch (eventCondition.CameraEvent.Type)
            {
                case EventType.NVRFail:
                case EventType.FailoverStartRecord:
                case EventType.FailoverDataStartSync:
                case EventType.FailoverSyncCompleted:
                    conditionNode.SetAttribute("id", "1");
                    conditionNode.SetAttribute("value", "1");
                    break;
            }

            conditionNode.SetAttribute("trigger", "1"); //fixed 1
            conditionNode.SetAttribute("interval", eventCondition.Interval.ToString(CultureInfo.InvariantCulture));

            return conditionNode;
        }

        private void SendMailHandleToXml(XmlDocument xmlDoc, SendMailEventHandle handle, EventCondition condition, XmlElement eventNode)
        {
            //<SendMail>
            //  <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //  <Subject>Event: NVR Fail</Subject> 
            //  <Body>(Fail Server: 172.16.1.80)</Body> 
            //  <Attach>false</Attach> 
            //  <AttachSource />
            //</SendMail>
            XmlElement mailNode = xmlDoc.CreateElement("SendMail");

            String subject = "Event:  " + condition.CameraEvent.ToLocalizationString();
            String body = " (Failover Server " + Server.Credential.Domain + ")";

            mailNode.AppendChild(xmlDoc.CreateXmlElementWithText("Recipient", handle.MailReceiver));
            mailNode.AppendChild(xmlDoc.CreateXmlElementWithText("Subject", subject));//handle.Subject
            mailNode.AppendChild(xmlDoc.CreateXmlElementWithText("Body", body)); // handle.Body
            mailNode.AppendChild(xmlDoc.CreateXmlElementWithText("Attach", "false"));
            mailNode.AppendChild(xmlDoc.CreateXmlElementWithText("AttachSource", ""));

            eventNode.AppendChild(mailNode);
        }

        public void UpdateNVRStatus()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiQuery, Server.Credential);

            if (xmlDoc == null) return;
            var status = Xml.GetFirstElementValueByTagName(xmlDoc, "Status");
            SynchronizeProgress = 0;
            switch (status)
            {
                case "Recording":
                    FailoverStatus = FailoverStatus.Recording;
                    break;

                case "Synchronize":
                    FailoverStatus = FailoverStatus.Synchronize;
                    try
                    {
                        SynchronizeProgress = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlDoc, "Progress"));
                    }
                    catch (Exception)
                    {
                    }
                    break;

                case "Merge Database":
                    FailoverStatus = FailoverStatus.MergeDatabase;
                    break;

                default:
                    FailoverStatus = FailoverStatus.Ping;
                    break;
            }

            var profileNodes = xmlDoc.GetElementsByTagName("Profile");
            var nvrs = new List<INVR>(NVRs.Values);
            foreach (XmlElement profileNode in profileNodes)
            {
                var id = Convert.ToUInt16(profileNode.GetAttribute("name").Replace("profile", ""));
                if (!NVRs.ContainsKey(id) || NVRs[id].FailoverSetting == null) continue;

                var nvr = NVRs[id];
                if (nvr.ReadyState == ReadyState.New) continue;
                nvrs.Remove(nvr);

                nvr.FailoverSetting.ActiveProfile = (Xml.GetFirstElementValueByTagName(profileNode, "IsActiveProfile") == "1");

                var pingNode = Xml.GetFirstElementByTagName(profileNode, "Ping");
                var ping = pingNode.InnerText;
                if ((nvr.ReadyState == ReadyState.Ready || nvr.ReadyState == ReadyState.Unavailable || nvr.ReadyState == ReadyState.Modify || nvr.FailoverSetting.ActiveProfile) && ping.Length > 0)
                {
                    //var times = (nvr.FailoverSetting.LaunchTime / nvr.FailoverSetting.PingTime);
                    var times = Convert.ToUInt16(pingNode.GetAttribute("n"));
                    var failtimes = ping.Substring(ping.LastIndexOf('1') + 1);
                    var count = failtimes.Count(c => c == '0') * 100.0;
                    nvr.FailoverSetting.FailPercent = Convert.ToInt16(Math.Floor(count / times));
                }
                else
                {
                    if (nvr.FailoverSetting.ActiveProfile)
                        nvr.FailoverSetting.FailPercent = 100;
                    else
                        nvr.FailoverSetting.FailPercent = -1;
                }
            }

            //not in query return, set -1
            foreach (var nvr in nvrs)
            {
                if (nvr.FailoverSetting != null)
                {
                    nvr.FailoverSetting.FailPercent = -1;
                    nvr.FailoverSetting.ActiveProfile = false;
                }
            }

            if (OnNVRStatusUpdate != null)
                OnNVRStatusUpdate(this, null);
        }

        private delegate void LoadDelegate();
        private void LoadCallback(IAsyncResult result)
        {
            ((LoadDelegate)result.AsyncState).EndInvoke(result);

            if (_loadCMSFlag)
            {
                _watch.Stop();

                ReadyStatus = ManagerReadyState.Ready;

                if (OnLoadComplete != null)
                    OnLoadComplete(this, null);
            }
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            if (!_saveNVRFlag) return;

            _watch.Stop();
            Trace.WriteLine(@"NVR Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            foreach (var obj in NVRs)
            {
                //if nvr not modify, ignore verify it
                if (obj.Value.ReadyState == ReadyState.Ready) continue;
                obj.Value.ReadyState = ReadyState.Ready;
                //mark this becuz CMS server will send status
                if (Server is IFOS)
                    obj.Value.ReadyState = (obj.Value.ValidateCredential()) ? ReadyState.ReSync : ReadyState.Unavailable;
            }

            ReadyStatus = ManagerReadyState.Ready;

            if (OnSaveComplete != null)
                OnSaveComplete(this, null);
        }

        private void CreateMapAttributes(XmlNode mapXml)
        {
            if (mapXml.Attributes == null) return;

            var odlPath = mapXml.Attributes["Path"] != null ? mapXml.Attributes["Path"].Value : String.Empty;

            var path = (mapXml.Attributes["OriginalFile"] != null)
                           ? mapXml.Attributes["OriginalFile"].Value
                           : odlPath;

            var provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };

            var map = new MapAttribute
            {
                Id = mapXml.Attributes["Id"].Value,
                ParentId = mapXml.Attributes["parentId"] == null ? String.Empty : mapXml.Attributes["parentId"].Value,
                Name = mapXml.Attributes["Name"].Value,
                OriginalFile = (mapXml.Attributes["OriginalFile"] != null) ? mapXml.Attributes["OriginalFile"].Value : path,
                SystemFile = (mapXml.Attributes["SystemFile"] != null) ? mapXml.Attributes["SystemFile"].Value : path,
                Width = int.Parse(mapXml.Attributes["Width"].Value == "" ? "0" : mapXml.Attributes["Width"].Value),
                Height = int.Parse(mapXml.Attributes["Height"].Value == "" ? "0" : mapXml.Attributes["Height"].Value),
                IsDefault = mapXml.Attributes["IsDefault"].Value == "Y",
                Scale = int.Parse(mapXml.Attributes["Scale"].Value == "" ? "0" : mapXml.Attributes["Scale"].Value),
                X = Convert.ToDouble((mapXml.Attributes["X"].Value == "" ? "0" : mapXml.Attributes["X"].Value), provider),
                Y = Convert.ToDouble((mapXml.Attributes["Y"].Value == "" ? "0" : mapXml.Attributes["Y"].Value), provider),
                ScaleCenterX = Convert.ToDouble(mapXml.Attributes["ScaleCenterX"].Value == "" ? "0" : mapXml.Attributes["ScaleCenterX"].Value, provider),
                ScaleCenterY = Convert.ToDouble(mapXml.Attributes["ScaleCenterY"].Value == "" ? "0" : mapXml.Attributes["ScaleCenterY"].Value, provider),
                //NVRs = new List<NVRAttributes>(),
                NVRs = new Dictionary<String, NVRAttributes>(),
                //Cameras = new List<CameraAttributes>(),
                Cameras = new Dictionary<String, CameraAttributes>(),
                //Vias = new List<ViaAttributes>()
                Vias = new Dictionary<String, ViaAttributes>(),
                HotZones = new Dictionary<String, MapHotZoneAttributes>()
            };

            //Add NVRs in maps
            var nvrs = ((XmlElement)mapXml).GetElementsByTagName("NVR");
            foreach (XmlElement nvr in nvrs)
            {
                var newNVR = new NVRAttributes
                {
                    Id = nvr.Attributes["Id"].Value,
                    SystemId = Convert.ToUInt16(nvr.Attributes["SystemId"].Value),
                    Name = nvr.Attributes["Name"].Value, //nvrServer.Name,
                    X = Convert.ToDouble(nvr.Attributes["X"].Value, provider),
                    Y = Convert.ToDouble(nvr.Attributes["Y"].Value, provider),
                    LinkToMap = nvr.Attributes["LinkToMap"].Value,
                    DescX = Double.Parse(nvr.Attributes["DescX"].Value, provider),
                    DescY = Double.Parse(nvr.Attributes["DescY"].Value, provider),
                    Type = "NVR",
                    Status = NVRStatus.NoSignal
                };

                map.NVRs.Add(newNVR.Id, newNVR);
            }

            //Add Cameras in map
            var cams = ((XmlElement)mapXml).GetElementsByTagName("Camera");

            foreach (XmlElement cam in cams)
            {
                var camera = new CameraAttributes
                {
                    Id = (cam.Attributes["Id"].Value),// Convert.ToUInt16
                    SystemId = Convert.ToUInt16(cam.Attributes["SystemId"].Value),
                    NVRSystemId = Convert.ToUInt16(cam.Attributes["NVRSystemId"].Value),
                    Name = cam.Attributes["Name"].Value, //String.Format("{0} ({1})", cameraDevice.ToString(), nvr.Name),//  devices[Convert.ToUInt16(cam.Attributes["SystemId"].Value)].ToString(),
                    Type = "Camera",
                    X = Double.Parse(cam.Attributes["X"].Value, provider),
                    Y = Double.Parse(cam.Attributes["Y"].Value, provider),
                    Rotate = int.Parse(cam.Attributes["Rotate"].Value),
                    DescX = Double.Parse(cam.Attributes["DescX"].Value, provider),
                    DescY = Double.Parse(cam.Attributes["DescY"].Value, provider),
                    IsSpeakerEnabled = cam.Attributes["IsSpeakerEnabled"].Value == "Y",    //audoiIn == 0 ? false : true,
                    SpeakerX = Double.Parse(cam.Attributes["SpeakerX"].Value, provider),
                    SpeakerY = Double.Parse(cam.Attributes["SpeakerY"].Value, provider),
                    IsAudioEnabled = cam.Attributes["IsAudioEnabled"].Value == "Y", //audioOut == 0 ? false : true,
                    AudioX = Double.Parse(cam.Attributes["AudioX"].Value, provider),
                    AudioY = Double.Parse(cam.Attributes["AudioY"].Value, provider),
                    IsDefaultOpenVideoWindow = false,
                    VideoWindowX = cam.Attributes["VideoWindowX"] == null ? Double.Parse(cam.Attributes["X"].Value, provider) : Double.Parse(cam.Attributes["VideoWindowX"].Value, provider),
                    VideoWindowY = cam.Attributes["VideoWindowY"] == null ? Double.Parse(cam.Attributes["Y"].Value, provider) : Double.Parse(cam.Attributes["VideoWindowY"].Value, provider),
                    VideoWindowSize = cam.Attributes["VideoWindowSize"] == null ? 1 : Convert.ToInt32(cam.Attributes["VideoWindowSize"].Value),
                    EventRecords = new List<CameraEventRecord>(),
                    CameraStatus = CameraStatus.Nosignal.ToString()
                };

                map.Cameras.Add(camera.Id, camera);
                //}
            }

            //Add Vias in map
            var vias = ((XmlElement)mapXml).GetElementsByTagName("Via");

            foreach (XmlElement via in vias)
            {

                var tVia = new ViaAttributes
                {
                    Id = via.Attributes["Id"].Value,
                    Name = via.Attributes["Name"].Value,
                    Type = "Via",
                    X = Double.Parse(via.Attributes["X"].Value, provider),
                    Y = Double.Parse(via.Attributes["Y"].Value, provider),
                    LinkToMap = via.Attributes["LinkToMap"].Value,
                    DescX = Double.Parse(via.Attributes["DescX"].Value, provider),
                    DescY = Double.Parse(via.Attributes["DescY"].Value, provider)
                };

                map.Vias.Add(tVia.Id, tVia);

            }

            //Add Hotzone in map
            var zones = ((XmlElement)mapXml).GetElementsByTagName("HotZone");

            foreach (XmlElement zone in zones)
            {
                var color = Color.FromArgb(Convert.ToInt32(zone.Attributes["Color"].Value));
                var tZone = new MapHotZoneAttributes
                {
                    Id = zone.Attributes["Id"].Value,
                    Type = "HotZone",
                    LinkToMap = zone.Attributes["LinkToMap"].Value,
                    Points = new List<Point>(),
                    Opacity = Convert.ToDouble(zone.Attributes["Opacity"].Value, provider),
                    Color = color,
                    Name = zone.Attributes["Name"] == null ? "New Region" : zone.Attributes["Name"].Value,
                    DescX = Convert.ToDouble((zone.Attributes["DescX"] == null ? "-10000" : zone.Attributes["DescX"].Value), provider),
                    DescY = Convert.ToDouble((zone.Attributes["DescY"] == null ? "-10000" : zone.Attributes["DescY"].Value), provider)
                };

                var totalX = 0.0;
                var totalY = 0.0;

                XmlNodeList points = zone.GetElementsByTagName("Point");
                foreach (XmlElement point in points)
                {
                    tZone.Points.Add(new Point(Int32.Parse(point.Attributes["X"].Value), Int32.Parse(point.Attributes["Y"].Value)));
                    totalX += Int32.Parse(point.Attributes["X"].Value);
                    totalY += Int32.Parse(point.Attributes["Y"].Value);
                }

                if (tZone.DescX == -10000 && tZone.DescY == -10000)
                {
                    if (tZone.Points.Count > 0)
                    {
                        tZone.DescX = totalX / tZone.Points.Count;
                        tZone.DescY = totalY / tZone.Points.Count;
                    }
                }

                map.HotZones.Add(tZone.Id, tZone);

            }

            Maps.Add(map.Id, map);
        }

        public void LoadMap()
        {
            Maps.Clear();

            var mapDoc = Xml.LoadXmlFromHttp(CgiLoadMap, Server.Credential);
            if (mapDoc != null)
            {
                var maps = mapDoc.GetElementsByTagName("Map");

                foreach (XmlNode map in maps)
                {
                    CreateMapAttributes(map);
                }
            }
        }

        public void SaveMap(XmlDocument mapDocument)
        {
            Xml.PostXmlToHttp(CgiSaveMap, mapDocument, Server.Credential);
        }

        public Boolean UploadMap(Bitmap map, String filename)
        {
            try
            {
                Xml.PostImageToHttp(CgiUploadMap.Replace("{MAPNAME}", filename), map, Server.Credential);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public Bitmap GetMap(String filename)
        {
            try
            {
                return Xml.LoadImageFromHttp(CgiGetMap.Replace("{MAPNAME}", filename), Server.Credential);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public INVR FindNVRById(UInt16 nvrId)
        {
            return NVRs.ContainsKey(nvrId) ? NVRs[nvrId] : null;
        }

        public MapAttribute FindMapById(String id)
        {
            return Maps.ContainsKey(id) ? Maps[id] : null;
        }

        public UInt16 GetNewNVRId()
        {
            UInt16 max = Server.License.Amount;
            if (Server is IVAS)
                max = 65535;
            else if (Server is ICMS)
                max = MaximunNVRAmount;

            for (UInt16 id = 1; id <= max; id++)
            {
                if (NVRs.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }

        public event EventHandler DeviceChanged;

        private void OnDeviceChanged(EventArgs e)
        {
            var handler = DeviceChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
