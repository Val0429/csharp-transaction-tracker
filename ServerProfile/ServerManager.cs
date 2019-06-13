using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;
using PanelBase;
using TimeZone = Constant.TimeZone;

namespace ServerProfile
{
    public class ServerManager : IServerManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;
        public event EventHandler OnStorageStatusUpdate;
        public event EventHandler OnRAIDProcessUpdate;
        public event EventHandler OnDateTimeUpdate;
        public event EventHandler OnCompleteUpdateEthernetLogout;
        public event EventHandler<EventArgs<String>> OnServerTimeZoneChange;

        protected void RaiseOnServerTimeZoneChange(EventArgs<string> e)
        {
            var handler = OnServerTimeZoneChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs<UInt16>> OnEthernetUpdateCableStatus;

        private const String CgiLoadCapability = @"cgi-bin/sysconfig?action=loadcapability";
        protected const String CgiLoadServer = @"cgi-bin/sysconfig?action=loadserver";
        protected const String CgiSaveServer = @"cgi-bin/sysconfig?action=saveserver";

        private const String CgiLoadTimeConfig = @"cgi-bin/sysconfig?action=loadtimeconfig";
        private const String CgiSaveTimeConfig = @"cgi-bin/sysconfig?action=savetimeconfig";
        private const String CgiLoadTimeZone = @"cgi-bin/sysconfig?action=loadtimezone";

        private const String CgiLoadStorage = @"cgi-bin/sysconfig?action=loadstorage";
        private const String CgiSaveStorage = @"cgi-bin/sysconfig?action=savestorage";
        private const String CgiDiskspace = @"cgi-bin/sysconfig?action=diskspace";
        private const String CgiLoadRAID = @"cgi-bin/sysconfig?action=loadraid";
        private const String CgiSaveRAID = @"cgi-bin/sysconfig?action=saveraid";

        private const String CgiLoadEthernet = @"cgi-bin/sysconfig?action=loadnetconfig&eth=%1";
        private const String CgiSaveEthernet = @"cgi-bin/sysconfig?action=savenetconfig&eth=%1";

        private const String CgiLoadLog = @"cgi-bin/log?starttime={STARTTIME}&endtime={ENDTIME}&type={TYPE}";

        private const String CgiBackup = @"cgi-bin/sysconfig?action=backup&content={CONTENT}";
        private const String CgiRestore = @"cgi-bin/sysconfig?action=restore&content={CONTENT}";

        private const String CgiReboot = @"cgi-bin/sysconfig?action=reboot";
        private const String CgiShutdown = @"cgi-bin/sysconfig?action=shutdown";
        private const String CgiUpgrade = @"cgi-bin/upgrade";

        private const String CgiLoadDatabase = @"cgi-bin/sysconfig?action=loaddbserver";
        private const String CgiSaveDatabase = @"cgi-bin/sysconfig?action=savedbserver";

        private const String CgiReadKeepMonth = @"cgi-bin/posdbquery?method=ReadKeepMonth";
        private const String CgiUpdateKeepMonth = @"cgi-bin/posdbquery?method=UpdateKeepMonth";

        private const String CgiLoadVersionInfo = @"cgi-bin/versioninfo";

        public Dictionary<String, String> Localization;
        public List<TimeZone> TimeZones { get; private set; }

        private Int64 _timeDiff;
        public DateTime DateTime
        {
            set
            {
                _timeDiff = value.Ticks - DateTime.UtcNow.Ticks;
            }
            get
            {
                return DateTime.UtcNow.AddTicks(_timeDiff);
            }
        }

        public DateTime ChangedDateTime { set; get; }

        public Int32 TimeZone { get; set; }
        public Int32 ChangedTimeZone { get; set; }

        public String Location { get; private set; }
        public String ChangedLocation { get; set; }

        public Boolean EnableDaylight { get; set; }
        public Boolean ChangedEnableDaylight { get; set; }

        public Int32 Daylight { get; set; }
        public Int32 ChangedDaylight { get; set; }

        public Boolean EnableNTPServer { get; set; }
        public String NTPServer { get; set; }

        public String Brand { get; protected set; }
        public String ProductNo { get; protected set; }
        public Platform Platform { get; protected set; }
        public UInt16 KeepDays { get; set; }
        public RAID RAID { get; private set; }
        public Dictionary<UInt16, Ethernet> Ethernets { get; set; }
        public DNS DNS { get; set; }
        public List<String> DisabledItems { get; private set; }
        public List<UInt16> NotAllowPorts { get; private set; }

        public ServerCredential Database { get; private set; }
        public UInt16 DatabaseKeepMonths { get; set; }
        public ReadyState DatabaseReadyStatus { get; set; }
        public ReadyState DatabaseKeepMonthsReadyStatus { get; set; }

        public ManagerReadyState ReadyStatus { get; set; }
        public IServer Server;

        //For Edit and Apply to Port after save.
        public UInt16 Port { get; set; }
        public UInt16 SSLPort { get; set; }

        public Int32 CPUUsage { get; set; }

        public String ServerVersion { get; private set; }
        public String DevicePackVersion { get; private set; }
        /// <summary>
        /// NVR Only
        /// </summary>
        public Dictionary<String, CameraManufactureFile> DeviceManufacture { get; private set; }
        public Dictionary<String, XmlElement> PageList { get; protected set; }
        public List<Storage> ChangedStorage { get; private set; }// C , 10(10GC keep space)
        public List<Storage> Storage { get; private set; }// C , 10(10GC keep space)
        public Dictionary<String, DiskInfo> StorageInfo { get; private set; }
        public Boolean KeepDaysEnabled { get; set; }
        public UInt16 DefaultDeepDays { get; set; }
        public Dictionary<UInt16, UInt16> DeviceRecordKeepDays { get; private set; }
        public Boolean IsPortChange { get; protected set; }
        public Boolean IsSSLPortChange { get; protected set; }
        public Boolean SupportAchiveServer { get; set; }
        public Boolean SupportPIP { get; set; }
        public Boolean EnableUPNP { get; set; }
        public Boolean HideExceptionAmount { get; set; }

        public ServerManager(IServer server)
            : this()
        {
            Server = server;
        }

        public ServerManager()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Error", "Error"},
                                   {"MessageBox_Information", "Information"},
                                   {"MessageBox_Confirm", "Confirm"},

                                   {"Application_SaveCompleted", "Save Completed"},
                                   {"Application_ConfirmEthernetCableUnplugged","Network cable is unplugged. \n Do you want to continue?"},
                                   {"Application_ServerChange","Server's setting has been modified. Please login again."},
                                   {"Application_BackupFailure", "Backup failure"},
                                   {"Application_BackupFileAlreadyExists", "The \"%1\" already exists.\n Do you want to continue?"}
                               };
            Localizations.Update(Localization);

            ReadyStatus = ManagerReadyState.New;
            DeviceManufacture = new Dictionary<String, CameraManufactureFile>();
            PageList = new Dictionary<String, XmlElement>();
            Storage = new List<Storage>();
            ChangedStorage = new List<Storage>();
            StorageInfo = new Dictionary<String, DiskInfo>();
            DeviceRecordKeepDays = new Dictionary<UInt16, UInt16>();
            DefaultDeepDays = 90;
            KeepDaysEnabled = false;

            Database = new ServerCredential
            {
                Domain = "127.0.0.1",
                Port = 7778
            };
            DatabaseReadyStatus = ReadyState.New;
            DatabaseKeepMonthsReadyStatus = ReadyState.New;

            DatabaseKeepMonths = 3;
            IsPortChange = false;
            IsSSLPortChange = false;

            CPUUsage = 0;

            ServerVersion = "1.00.00";// default, for those server without cgi.
            DevicePackVersion = String.Empty;// default, get version info from server cgi // not from version.xml (from DeviceCab project)

            SupportAchiveServer = false;
            SupportPIP = false;
            EnableUPNP = false;
            HideExceptionAmount = false;
        }

        public virtual void Initialize()
        {

        }

        public String Status
        {
            get { return "Server : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private readonly Stopwatch _watch = new Stopwatch();
        public virtual void Load()
        {
            //Port = Server.Credential.Port;
            ReadyStatus = ManagerReadyState.Loading;

            DeviceManufacture.Clear();
            PageList.Clear();
            Storage.Clear();
            ChangedStorage.Clear();
            StorageInfo.Clear();

            _watch.Reset();
            _watch.Start();

            DisabledItems = new List<String>();
            NotAllowPorts = new List<UInt16>();

            LoadServer();
            LoadCapability();
            LoadServerTime();
            LoadServerVersion();
            //LoadDevicePackVersion();
            //LoadDelegate loadServerTimeDelegate = LoadServerTime;
            //loadServerTimeDelegate.BeginInvoke(LoadCallback, loadServerTimeDelegate);

            //SSLPort = 0;
            TimeZones = new List<TimeZone>();
            RAID = new RAID { DiskStatus = new Dictionary<String, RAIDDisk>() };
            Ethernets = new Dictionary<UInt16, Ethernet>();
            DNS = new DNS();
            Daylight = ChangedDaylight = 0;

            if (Server is IVAS) //Server is ICMS || 
            {
                LoadCompleted();
                return;
            }

            if (Server is IPTS)
            {
                LoadDelegate loadDatabaseDelegate = LoadDatabase;
                loadDatabaseDelegate.BeginInvoke(LoadCallback, loadDatabaseDelegate);
            }

            if (Platform == Platform.Linux)
            {
                LoadLinuxServerTimeZones();
                LoadRAID();
                LoadEthernets();
            }

            LoadDelegate loadStorageDelegate = LoadStorage;
            loadStorageDelegate.BeginInvoke(LoadCallback, loadStorageDelegate);
        }

        protected virtual void LoadServer()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadServer, Server.Credential);
            if (xmlDoc == null) return;

            var node = xmlDoc.SelectSingleNode("Config");
            if (node != null)
            {
                Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Port"));

                var sslportNode = Xml.GetFirstElementValueByTagName(node, "SSLPort");
                if (!String.IsNullOrEmpty(sslportNode))
                    SSLPort = Convert.ToUInt16(sslportNode);
                else
                    SSLPort = 443;

                if (!String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(node, "EnableUPNP")))
                {
                    EnableUPNP = Xml.GetFirstElementValueByTagName(node, "EnableUPNP") == "true";
                }
            }
            else
            {
                Port = Server.Credential.Port;
                SSLPort = Server.Credential.Port;
            }
        }

        protected virtual void LoadCapability()
        {
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadCapability, Server.Credential);
            if (xmlDoc == null) return;

            XmlNode capabilityNode = xmlDoc.SelectSingleNode("Capability");
            if (capabilityNode == null) return;

            Brand = Xml.GetFirstElementValueByTagName(capabilityNode, "Brand");
            ProductNo = Xml.GetFirstElementValueByTagName(capabilityNode, "ProductNO");
            var platform = Xml.GetFirstElementValueByTagName(capabilityNode, "Platform");
            Platform = platform == "" || platform == "Windows" ? Platform.Windows : Platform.Linux;

            var supportAchiveServer = Xml.GetFirstElementValueByTagName(capabilityNode, "SupportAchiveServer");
            if (!String.IsNullOrEmpty(supportAchiveServer))
            {
                SupportAchiveServer = supportAchiveServer == "true";
            }

            var supportPIP = Xml.GetFirstElementValueByTagName(capabilityNode, "SupportPIP");
            if (!String.IsNullOrEmpty(supportPIP))
            {
                SupportPIP = supportPIP == "true";
            }

            var hideExceptionAmount = Xml.GetFirstElementValueByTagName(capabilityNode, "HideExceptionAmount");
            if (!String.IsNullOrEmpty(hideExceptionAmount))
            {
                HideExceptionAmount = hideExceptionAmount == "true";
            }

            XmlNode apNode = capabilityNode.SelectSingleNode("AP");
            if (apNode == null) return;

            XmlNode manufacturesNode = apNode.SelectSingleNode("Manufactures");
            if (manufacturesNode != null)
            {
                var manufactures = ((XmlElement)manufacturesNode).GetElementsByTagName("Manufacture");
                if (manufactures.Count != 0)
                {
                    foreach (XmlElement node in manufactures)
                    {
                        String key = Xml.GetFirstElementValueByTagName(node, "Name");
                        if (!DeviceManufacture.ContainsKey(key))
                        {
                            DeviceManufacture.Add(key, new CameraManufactureFile
                            {
                                Name = key,
                                File = Xml.GetFirstElementValueByTagName(node, "File"),
                            });
                        }
                    }
                }

                //Sort Manufacture by A-Z
                if (DeviceManufacture.Count > 1)
                {
                    var deviceManufacture = new Dictionary<String, CameraManufactureFile>(DeviceManufacture);
                    DeviceManufacture.Clear();

                    var keys = new List<String>(deviceManufacture.Keys.ToList());
                    keys.Sort();

                    foreach (var key in keys)
                        DeviceManufacture.Add(key, deviceManufacture[key]);
                    deviceManufacture.Clear();
                    keys.Clear();
                }
            }

            //parse *DisabledSetupItems* -> disabledSetup
            var disalbedItemsString = Xml.GetFirstElementValueByTagName(apNode, "DisabledSetupItems");
            if (!String.IsNullOrEmpty(disalbedItemsString))
            {
                var disalbedItemsNodes = disalbedItemsString.Split(',');
                foreach (String item in disalbedItemsNodes)
                {
                    if (item.IndexOf("Port") > -1)
                    {
                        var portsString = item.Substring(item.IndexOf('[') + 1, item.Length - item.IndexOf('[') - 2);
                        if (!String.IsNullOrEmpty(portsString))
                        {
                            var ports = portsString.Split(';');
                            foreach (String port in ports)
                            {
                                NotAllowPorts.Add(Convert.ToUInt16(port));
                            }
                        }
                        else
                        {
                            DisabledItems.Add("Port");
                        }
                    }
                    else
                    {
                        DisabledItems.Add(item);
                    }
                }
            }

            var pagesNode = apNode.SelectSingleNode("Pages") as XmlElement;
            if (pagesNode == null) return;

            var pages = pagesNode.GetElementsByTagName("Page");
            if (pages.Count == 0) return;

            //App.Name = ((XmlElement)pagesNode).GetAttribute("name");

            foreach (XmlElement node in pages)
            {
                AddPageList(node);
            }
        }

        protected virtual void AddPageList(XmlElement pageNode)
        {
            String pageName = Xml.GetFirstElementValueByTagName(pageNode, "Name");

            PageList.Add(pageName, pageNode);
        }

        private Boolean _loadServerFlag;
        public virtual void LoadServerTime()
        {
            if (Platform == Platform.Linux)
            {
                LoadLinuxServerTime();
                return;
            }
            _loadServerFlag = false;
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadTimeConfig, Server.Credential);

            if (xmlDoc != null)
            {
                XmlNode node = xmlDoc.SelectSingleNode("Config");

                if (node != null)
                    ParseDateTimeXML(node);
            }

            _loadServerFlag = true;
        }

        private void LoadLinuxServerTime()
        {
            _loadServerFlag = false;
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadTimeConfig, Server.Credential);

            if (xmlDoc != null)
            {
                XmlNode node = xmlDoc.SelectSingleNode("Config");

                if (node != null)
                {
                    var currentProfile = Xml.GetFirstElementValueByTagName(node, "CurrentProfile");

                    if (!String.IsNullOrEmpty(currentProfile))
                    {
                        var profile = Xml.GetFirstElementByTagName(node, currentProfile);
                        if (profile != null)
                            ParseLinuxDateTimeXML(profile);
                    }
                }
            }

            _loadServerFlag = true;
        }

        private void ParseDateTimeXML(XmlNode profile)
        {
            if (Location == null)
            {
                Int32 daylight = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "Daylight"));
                Int32 timeZone = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "TimeZone"));
                TimeZone = timeZone + daylight;
                Location = Xml.GetFirstElementValueByTagName(profile, "TimeName");
                DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(profile, "Time")), Server.Server.TimeZone);
            }
            else
            {
                String newLocation = Xml.GetFirstElementValueByTagName(profile, "TimeName");
                Int32 newTimeZone = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "Daylight")) +
                    Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "TimeZone"));
                if (Location == newLocation && TimeZone == newTimeZone)
                {
                    DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(profile, "Time")), Server.Server.TimeZone);
                }
                else
                {
                    RaiseOnServerTimeZoneChange(new EventArgs<String>(newLocation));
                }
            }
        }

        private void ParseLinuxDateTimeXML(XmlNode profile)
        {
            if (ChangedLocation == Location && ChangedEnableDaylight == EnableDaylight)
            {
                ChangedTimeZone = TimeZone = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "TimeZone"));
                ChangedLocation = Location = Xml.GetFirstElementValueByTagName(profile, "TimeName");
                ChangedEnableDaylight = EnableDaylight = (Xml.GetFirstElementValueByTagName(profile, "EnableDaylight") == "true");
                ChangedDaylight = Daylight = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "Daylight"));

                if (EnableDaylight && Daylight != 0)
                    TimeZone += Daylight;

                ChangedDateTime = DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(profile, "Time")), Server.Server.TimeZone);
            }
            else
            {
                TimeZone = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "TimeZone"));
                Location = Xml.GetFirstElementValueByTagName(profile, "TimeName");
                EnableDaylight = (Xml.GetFirstElementValueByTagName(profile, "EnableDaylight") == "true");
                Daylight = Convert.ToInt32(Xml.GetFirstElementValueByTagName(profile, "Daylight"));

                if (EnableDaylight && Daylight != 0)
                    TimeZone += Daylight;

                DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(profile, "Time")), Server.Server.TimeZone);
            }

            NTPServer = Xml.GetFirstElementValueByTagName(profile, "NTPServer");
            EnableNTPServer = (!String.IsNullOrEmpty(NTPServer.Trim()));
        }

        public virtual void LoadServerVersion()
        {
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadVersionInfo, Server.Credential);

            if (xmlDoc != null)
            {
                XmlNode node = xmlDoc.SelectSingleNode("VERSION");

                if (node != null)
                    ParseServerVersionXML(node);
            }
        }

        private void ParseServerVersionXML(XmlNode node)
        {
            ServerVersion = Xml.GetFirstElementValueByTagName(node, "Server");
            DevicePackVersion = Xml.GetFirstElementValueByTagName(node, "DevicePack");

            if (String.IsNullOrEmpty(ServerVersion))
                ServerVersion = "1.00.00";
        }

        private void LoadLinuxServerTimeZones()
        {
            _loadServerFlag = false;
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadTimeZone, Server.Credential);
            if (xmlDoc != null)
            {
                TimeZones.Clear();
                XmlNodeList timeZones = xmlDoc.GetElementsByTagName("Zone");
                foreach (XmlElement timeZone in timeZones)
                {
                    TimeZones.Add(new TimeZone
                    {
                        Name = Xml.GetFirstElementValueByTagName(timeZone, "TimeName"),
                        Value = Convert.ToInt32(Xml.GetFirstElementValueByTagName(timeZone, "TimeZone")),
                    });
                }

                TimeZones.Sort(SortTimeZones);
                //TimeZones.OrderBy(KeyValuePair<>);
            }
            _loadServerFlag = true;
        }

        private static Int32 SortTimeZones(TimeZone x, TimeZone y)
        {
            if (x.Value != y.Value)
                return (x.Value - y.Value);

            return String.Compare(x.Name, y.Name);
        }

        public void LoadStorageInfo()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiDiskspace, Server.Credential);

            if (xmlDoc == null) return;
            //if (Platform == Platform.Linux)
            if (Server.Server.CheckProductNoToSupport("raid"))
            {
                ParseStorageInfoFromXmlForRAID((XmlElement)xmlDoc.SelectSingleNode("Config"));
            }
            else
            {
                ParseStorageInfoFromXml((XmlElement)xmlDoc.SelectSingleNode("DiskInfo"));
            }

            if (_loadStorageFlag && OnStorageStatusUpdate != null)
                OnStorageStatusUpdate(this, null);
        }

        public void LoadRAID()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadRAID, Server.Credential);

            if (xmlDoc != null)
            {
                var xmlNode = (XmlElement)xmlDoc.SelectSingleNode("Config");
                RAID.Mode = RAID.FindRAIDMode(Xml.GetFirstElementValueByTagName(xmlNode, "Mode"));
            }
            else
            {
                if (ReadyStatus == ManagerReadyState.Ready)
                {
                    if (OnRAIDProcessUpdate != null)
                        OnRAIDProcessUpdate(this, null);
                }
                return;
            }

            var disks = Xml.GetFirstElementValueByTagName(xmlDoc, "Disks");
            RAID.Disks = (ushort)(String.IsNullOrEmpty(disks) ? 0 : Convert.ToUInt16(disks));
            var process = Xml.GetFirstElementValueByTagName(xmlDoc, "Progress");
            RAID.Process = (ushort)(String.IsNullOrEmpty(process) ? 0 : Convert.ToUInt16(process));

            RAID.Status = RAID.FindRAIDStatus(Xml.GetFirstElementValueByTagName(xmlDoc, "Status"));

            RAID.DiskStatus.Clear();
            XmlNodeList diskStatusNodes = xmlDoc.GetElementsByTagName("Disk");
            foreach (XmlElement diskStatusNode in diskStatusNodes)
            {
                if (String.IsNullOrEmpty(diskStatusNode.GetAttribute("status")))
                {
                    if (String.IsNullOrEmpty(diskStatusNode.InnerText)) continue;

                    RAID.DiskStatus.Add(diskStatusNode.GetAttribute("id"), new RAIDDisk
                    {
                        Status = RAIDDisk.FindRAIDDiskStatus(diskStatusNode.InnerText),
                        Description = diskStatusNode.InnerText
                    });
                }
                else
                {
                    var descNode = Xml.GetFirstElementByTagName(diskStatusNode, "Desc");
                    if (descNode == null)
                    {
                        RAID.DiskStatus.Add(diskStatusNode.GetAttribute("id"), new RAIDDisk
                        {
                            Status = RAIDDisk.FindRAIDDiskStatus(diskStatusNode.GetAttribute("status")),
                            Description = diskStatusNode.GetAttribute("status")
                        });
                    }
                    else
                    {
                        RAID.DiskStatus.Add(diskStatusNode.GetAttribute("id"), new RAIDDisk
                        {
                            Status = RAIDDisk.FindRAIDDiskStatus(diskStatusNode.GetAttribute("status")),
                            Description = descNode.InnerText,
                            DescValue = descNode.GetAttribute("value")
                        });
                    }
                }

            }

            if (ReadyStatus == ManagerReadyState.Ready)
            {
                if (OnRAIDProcessUpdate != null)
                    OnRAIDProcessUpdate(this, null);
            }
        }

        public void LoadEthernets()
        {
            Ethernets.Clear();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadEthernet.Replace("%1", "0"), Server.Credential);
            if (xmlDoc != null)
            {
                ParseEthernetFromXml(1, xmlDoc.SelectSingleNode("Config"));
            }

            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadEthernet.Replace("%1", "1"), Server.Credential);
            if (xmlDoc != null)
            {
                ParseEthernetFromXml(2, xmlDoc.SelectSingleNode("Config"));
            }
        }

        private void ParseEthernetFromXml(UInt16 id, XmlNode xmlNode)
        {
            //<DeviceExist>1</DeviceExist>
            //<DynamicIP>1</DynamicIP>
            //<IPAddress>172.16.1.31</IPAddress>
            //<Mask>255.255.255.0</Mask>
            //<MACAddress>00:25:22:BC:C9:07</MACAddress>
            //<DeviceCarrier>1</DeviceCarrier>
            //<Gateway>172.16.1.254</Gateway>
            //<Hostname>NVR</Hostname>
            //<DynamicDNS>0</DynamicDNS>
            //<PrimaryDNS>172.16.1.201</PrimaryDNS>
            //<SecondaryDNS/>
            //<LastModify>0</LastModify>
            if (xmlNode == null) return;
            var ethernet = new Ethernet
            {
                ReadyStatus = ManagerReadyState.Ready,
                DeviceExist = Xml.GetFirstElementValueByTagName(xmlNode, "DeviceExist") == "1",
                DynamicIP = Xml.GetFirstElementValueByTagName(xmlNode, "DynamicIP") == "1",
                IPAddress = Xml.GetFirstElementValueByTagName(xmlNode, "IPAddress"),
                Mask = Xml.GetFirstElementValueByTagName(xmlNode, "Mask"),
                MACAddress = Xml.GetFirstElementValueByTagName(xmlNode, "MACAddress"),
                DeviceCarrier = Xml.GetFirstElementValueByTagName(xmlNode, "DeviceCarrier") == "1",
                Gateway = Xml.GetFirstElementValueByTagName(xmlNode, "Gateway"),
                LastModify = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(xmlNode, "LastModify"))
            };
            Ethernets.Add(id, ethernet);

            DNS.Hostname = Xml.GetFirstElementValueByTagName(xmlNode, "Hostname");
            DNS.DynamicDNS = Xml.GetFirstElementValueByTagName(xmlNode, "DynamicDNS") == "1";
            DNS.PrimaryDNS = Xml.GetFirstElementValueByTagName(xmlNode, "PrimaryDNS");
            DNS.SecondDNS = Xml.GetFirstElementValueByTagName(xmlNode, "SecondaryDNS");
        }

        private Boolean _loadStorageFlag;
        public void LoadStorage()
        {
            _loadStorageFlag = false;

            LoadStorageInfo();

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadStorage, Server.Credential);
            if (xmlDoc != null)
                ParseStorageFromXml((XmlElement)xmlDoc.SelectSingleNode("StorageConfiguration"));

            if (OnStorageStatusUpdate != null)
                OnStorageStatusUpdate(this, null);

            _loadStorageFlag = true;
        }

        private void ParseStorageFromXml(XmlElement xmlNode)
        {
            if (xmlNode == null) return;

            Storage.Clear();
            ChangedStorage.Clear();

            var paths = xmlNode.GetElementsByTagName("Path");

            if (paths.Count == 0) return;

            foreach (XmlElement node in paths)
            {
                //String drive = node.InnerText.Split(':')[0];
                String drive = Platform == Platform.Linux ? "\\" : node.InnerText.Split(':')[0];
                String name = node.GetAttributeNode("name") == null ? null : node.GetAttribute("name");
                if (name != null)
                {
                    drive = name;
                }

                if (!StorageInfo.ContainsKey(drive)) continue;

                Storage.Add(new Storage
                {
                    Key = drive,
                    Name = name,
                    Path = node.InnerText,
                    KeepSpace = Convert.ToUInt16(node.GetAttribute("keepspace")),
                });

                ChangedStorage.Add(new Storage
                {
                    Key = drive,
                    Name = name,
                    Path = node.InnerText,
                    KeepSpace = (ushort)(Server is ICMS ? 0 : Convert.ToUInt16(node.GetAttribute("keepspace"))),
                });
            }

            DeviceRecordKeepDays.Clear();
            var deviceRecordKeepDaysNode = xmlNode.GetElementsByTagName("DeviceRecordKeepDays");
            if (deviceRecordKeepDaysNode.Count == 0) return;
            var deviceRecordKeepDays = ((XmlElement)deviceRecordKeepDaysNode[0]);
            if (deviceRecordKeepDays == null) return;
            KeepDaysEnabled = deviceRecordKeepDays.GetAttributeNode("enabled") == null ? false : deviceRecordKeepDays.GetAttribute("enabled").ToLower() == "true";
            DefaultDeepDays = (ushort)(deviceRecordKeepDays.GetAttributeNode("default") == null ? 90 : Convert.ToUInt16(deviceRecordKeepDays.GetAttribute("default")));

            var devices = xmlNode.GetElementsByTagName("Device");

            if (devices.Count == 0) return;

            foreach (XmlElement node in devices)
            {
                var deviceId = node.GetAttribute("id");
                if (String.IsNullOrEmpty(deviceId)) continue;

                var keepdays = node.InnerText;
                if (String.IsNullOrEmpty(keepdays)) continue;

                if (DeviceRecordKeepDays.ContainsKey(Convert.ToUInt16(deviceId))) continue;
                if (Convert.ToUInt16(keepdays) == 0) continue;

                DeviceRecordKeepDays.Add(Convert.ToUInt16(deviceId), Convert.ToUInt16(keepdays));
            }
        }

        private void LoadDatabase()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadDatabase, Server.Credential, false);

            if (xmlDoc != null)
            {
                DatabaseReadyStatus = ReadyState.Ready;
                ParseDatabaseFromXml((XmlElement)xmlDoc.SelectSingleNode("Config"));
            }

            var requestXmlDoc = new XmlDocument();
            var xmlRoot = requestXmlDoc.CreateElement("Request");
            requestXmlDoc.AppendChild(xmlRoot);
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(requestXmlDoc, "UTC", Guid.NewGuid().ToString()));

            xmlDoc = Xml.PostXmlToHttp(CgiReadKeepMonth, requestXmlDoc, Server.Credential, retry: false);

            if (xmlDoc == null) return;
            if (String.IsNullOrEmpty(xmlDoc.InnerText)) return;

            try
            {
                DatabaseKeepMonthsReadyStatus = ReadyState.Ready;
                var month = Convert.ToUInt16(xmlDoc.InnerText);
                DatabaseKeepMonths = month;
            }
            catch (Exception)
            {
                DatabaseKeepMonths = 3;
            }
        }

        private void ParseDatabaseFromXml(XmlElement xmlNode)
        {
            //<Config>
            //    <IPAddress>localhost</IPAddress>
            //    <Port>83</Port>
            //</Config>

            if (xmlNode == null) return;

            Database.Domain = Xml.GetFirstElementValueByTagName(xmlNode, "IPAddress");
            Database.Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlNode, "Port"));
        }

        private const String CMSBackup = @"device,user,general,emap,license,nvr,server,storage";
        private const String NVRBackup = @"device,user,general,license,storage,server";
        public void Backup()
        {
            try
            {
                var client = new WebClient { Credentials = Server.Credential };
                var filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var httpMode = (Server.Credential.SSLEnable) ? "https://" : "http://";
                var uri = httpMode + Server.Credential.Domain + ":" + Server.Credential.Port + "/" + CgiBackup;
                if (Server is ICMS)
                {
                    uri = uri.Replace("{CONTENT}", CMSBackup);
                    filename = filename + "\\CMS [" + Server.Credential.Domain + "] " + DateTime.Now.ToFileDateString() + ".setting";
                }
                else
                {
                    uri = uri.Replace("{CONTENT}", NVRBackup);
                    filename = filename + "\\NVR [" + Server.Credential.Domain + "] " + DateTime.Now.ToFileDateString() + ".setting";
                }

                var fileInfo = new FileInfo(filename);
                if (fileInfo.Exists)
                {
                    var backupResult = TopMostMessageBox.Show(Localization["Application_BackupFileAlreadyExists"].Replace("%1", filename), Localization["MessageBox_Confirm"],
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (backupResult == DialogResult.No) return;
                }

                client.DownloadFile(new Uri(uri), filename);
                Process.Start("explorer.exe", "/select," + filename);
            }
            catch (Exception exception)
            {
                TopMostMessageBox.Show(Localization["Application_BackupFailure"] + Environment.NewLine + exception, Localization["MessageBox_Error"],
                       MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        public void RAIDFormat()
        {
            //<Config>
            //  <Mode>RAID0</Mode>
            //</Config>
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Config");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mode", String.Format("{0}", RAID.Mode)));

            Xml.PostXmlToHttp(CgiSaveRAID, xmlDoc, Server.Credential);
        }

        public void SaveEthernet(UInt16 id)
        {
            ApplicationForms.ShowLoadingIcon(Server.Form);

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadEthernet.Replace("%1", (id - 1).ToString()), Server.Credential);
            if (xmlDoc != null)
            {
                var xmlNode = (XmlElement)xmlDoc.SelectSingleNode("Config");
                if (xmlNode == null || !Ethernets.ContainsKey(id))
                {
                    SaveEthernetCompleted();
                    return;
                }

                var ethernet = Ethernets[id];
                ethernet.DeviceCarrier = Xml.GetFirstElementValueByTagName(xmlNode, "DeviceExist") == "1";
                if (!ethernet.DeviceCarrier)
                {
                    if (OnEthernetUpdateCableStatus != null)
                        OnEthernetUpdateCableStatus(this, new EventArgs<UInt16>(id));
                    var ethernetResult = MessageBox.Show(Localization["Application_ConfirmEthernetCableUnplugged"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ethernetResult != DialogResult.Yes)
                    {
                        ApplicationForms.HideLoadingIcon();
                        return;
                    }
                }
                SendEthernet(id, ethernet, Xml.GetFirstElementValueByTagName(xmlNode, "LastModify"));
            }

            SaveEthernetCompleted();
        }

        private void SendEthernet(UInt16 id, Ethernet ethernet, String serverLastModify)
        {
            if (ethernet.LastModify.ToString() != serverLastModify || ethernet.ReadyStatus == ManagerReadyState.MajorModify)
            {
                var xmlDoc = new XmlDocument();
                var xmlRoot = xmlDoc.CreateElement("Config");
                xmlDoc.AppendChild(xmlRoot);

                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DynamicIP", ethernet.DynamicIP ? "1" : "0"));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IPAddress", ethernet.IPAddress));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mask", ethernet.Mask));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Gateway", ethernet.Gateway));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DynamicDNS", DNS.DynamicDNS ? "1" : "0"));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "PrimaryDNS", DNS.PrimaryDNS));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SecondaryDNS", DNS.SecondDNS));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "LastModify", DateTime.UtcNow.Ticks.ToString()));

                Xml.PostXmlToHttp(CgiSaveEthernet.Replace("%1", (id - 1).ToString()), xmlDoc, Server.Credential);

                ethernet.ReadyStatus = ManagerReadyState.Ready;
            }
            else
            {
                SaveEthernetCompleted();
            }
        }

        private void SaveEthernetCompleted()
        {
            ApplicationForms.HideLoadingIcon();

            var result = MessageBox.Show(Localization["Application_ServerChange"], Localization["MessageBox_Information"],
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (result == DialogResult.OK && OnCompleteUpdateEthernetLogout != null)
                OnCompleteUpdateEthernetLogout(this, new EventArgs());
        }

        public void UpdateEthernetCableStatus(UInt16 id)
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadEthernet.Replace("%1", (id - 1).ToString()), Server.Credential);
            if (xmlDoc != null)
            {
                var xmlNode = (XmlElement)xmlDoc.SelectSingleNode("Config");
                if (xmlNode == null || !Ethernets.ContainsKey(id)) return;

                var ethernet = Ethernets[id];
                ethernet.DeviceCarrier = Xml.GetFirstElementValueByTagName(xmlNode, "DeviceCarrier") == "1";

                if (OnEthernetUpdateCableStatus != null)
                    OnEthernetUpdateCableStatus(this, new EventArgs<UInt16>(id));
            }
        }

        public void Restore(Stream configFile, List<String> contents)
        {
            Xml.PostFileStreamToHttp(CgiRestore.Replace("{CONTENT}", String.Join(",", contents.ToArray())), configFile, Server.Credential);
        }

        public String Upgrade(Stream configFile)
        {
            return Xml.PostFileStreamToHttp(CgiUpgrade, configFile, Server.Credential);
        }

        public void Reboot()
        {
            Xml.LoadXmlFromHttp(CgiReboot, Server.Credential);
        }

        public void Shutdown()
        {
            Xml.LoadXmlFromHttp(CgiShutdown, Server.Credential);
        }

        private void ParseStorageInfoFromXml(XmlElement xmlNode)
        {
            if (xmlNode == null) return;
            //<Disk>
            //  <Letter>C:</Letter> 
            //  <Label>HD</Label> 
            //  <TotalBytes>322017685504</TotalBytes> 
            //  <FreeBytes>210096852992</FreeBytes> 
            //</Disk>

            StorageInfo.Clear();
            var diskNodes = xmlNode.GetElementsByTagName("Disk");
            foreach (XmlElement diskNode in diskNodes)
            {
                var letter = Xml.GetFirstElementValueByTagName(diskNode, "Letter");
                var total = Convert.ToInt64(Xml.GetFirstElementValueByTagName(diskNode, "TotalBytes"));
                var free = String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(diskNode, "FreeBytes")) ? 0 : Convert.ToInt64(Xml.GetFirstElementValueByTagName(diskNode, "FreeBytes"));
                if (total == 0) continue;

                var drive = letter.Split(':')[0];

                var name = String.Empty;
                if (Server.Server.Platform == Platform.Linux)
                {
                    if (Server.Server.CheckProductNoToSupport("storage"))
                    {
                        name = letter;
                    }
                }
                else
                {
                    name = null;
                }

                if (!StorageInfo.ContainsKey(drive))
                    StorageInfo.Add(drive, new DiskInfo
                    {
                        Name = name,
                        Total = total,
                        Used = (total - free),
                        Free = free,
                    });
            }
        }

        private void ParseStorageInfoFromXmlForRAID(XmlElement xmlNode)
        {
            if (xmlNode == null) return;
            //<Config>
            //<RAID>
            //    <TotalBytes>2953090686976</TotalBytes>
            //    <FreeBytes>2886796865536</FreeBytes>
            //</RAID>
            //<iSCSI>
            //    <TotalBytes>0</TotalBytes>
            //    <FreeBytes>0</FreeBytes>
            //</iSCSI>
            //</Config>
            StorageInfo.Clear();
            var diskNodes = xmlNode.GetElementsByTagName("RAID");
            if (diskNodes.Count > 0)
            {
                Int64 total = Convert.ToInt64(Xml.GetFirstElementValueByTagName(diskNodes[0], "TotalBytes"));
                Int64 free = Convert.ToInt64(Xml.GetFirstElementValueByTagName(diskNodes[0], "FreeBytes"));

                StorageInfo.Add("\\", new DiskInfo
                {
                    Total = total,
                    Used = (total - free),
                    Free = free,
                });
            }
        }

        public List<Log> LoadLog(DateTime dateTime, LogType[] types)
        {
            if (types.Length == 0) return new List<Log>();

            var logType = new List<String>();

            //user's log first
            if (types.Contains(LogType.Action))
                logType.Add("event");

            if (types.Contains(LogType.Operation))
                logType.Add("app");

            //system log later
            if (types.Contains(LogType.Server))
                logType.Add("system");

            if (logType.Count == 0) return new List<Log>();

            ////Today
            String targetTime = dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            String startTime = targetTime;
            String endTime = targetTime;

            if (TimeZone > 0)
            {
                // + Yesterday
                startTime = dateTime.AddDays(-1).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else if (TimeZone < 0)
            {
                // + Tomorrow
                endTime = dateTime.AddDays(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            }

            var times = new List<String> { startTime };
            //start != end //cross day
            if (!times.Contains(endTime))
                times.Add(endTime);

            //split queue log
            var logs = new List<Log>();

            var recorderLogCount = 0;

            foreach (var time in times)
            {
                foreach (var type in logType)
                {
                    XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadLog.Replace("{STARTTIME}", time)
                        .Replace("{ENDTIME}", time).Replace("{TYPE}", type), Server.Credential);

                    if (xmlDoc != null)
                    {
                        LogType logtype;
                        switch (type)
                        {
                            case "system":
                                logtype = LogType.Server;
                                break;

                            default:
                                logtype = LogType.Action;
                                break;
                        }

                        var loglists = ParseLogFromXml((XmlElement)xmlDoc.SelectSingleNode("Log"), targetTime, logtype, ref recorderLogCount);

                        logs.AddRange(loglists);

                        //already log "login" & "logout" in operation
                        if (types.Contains(LogType.Operation))
                        {
                            var removeLog = new List<Log>();
                            foreach (var log in logs)
                            {
                                if (log.Type != LogType.Action) continue;

                                if (log.Description == "User Login" || log.Description == "User Logout")
                                    removeLog.Add(log);
                            }

                            foreach (var log in removeLog)
                            {
                                logs.Remove(log);
                            }
                        }
                    }
                }
            }

            if (logs.Count > 1)
            {
                var sortLog = from c in logs
                              orderby c.Timestamp, c.User
                              select c;

                return sortLog.ToList();
            }

            return logs;
        }

        private const UInt16 MaximumLog = 3000;
        private List<Log> ParseLogFromXml(XmlElement xmlNode, String targetTime, LogType type, ref Int32 recorderLogCount)
        {
            if (xmlNode == null) return new List<Log>();

            var logNodes = xmlNode.GetElementsByTagName("DATA");
            if (logNodes.Count == 0) return new List<Log>();

            var logs = new List<Log>();

            _watch.Reset();
            _watch.Start();

            foreach (XmlElement logNode in logNodes)
            {
                var timestamp = Xml.GetFirstElementValueByTagName(logNode, "Time");
                DateTime dateTime = DateTimes.ToDateTime(Convert.ToUInt64(timestamp) * 1000, Server.Server.TimeZone);
                if (dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture) != targetTime) continue;

                var user = Xml.GetFirstElementValueByTagName(logNode, "User");
                if (type == LogType.Server)
                {
                    user = user.Replace("System:", "");
                    if (user == "Recorder")
                    {
                        recorderLogCount++;
                        // recorder log maximun 3000. no more recorder log, BUT other log still needed.
                        if (recorderLogCount > MaximumLog)
                            continue;
                    }
                }

                logs.Add(new Log
                {
                    Timestamp = timestamp,
                    DateTime = dateTime,
                    User = user,
                    Type = type,
                    //Type = (Xml.GetFirstElementValueByTagName(logNode, "Type") == "System") ? LogType.Server : LogType.Action,
                    Description = Xml.GetFirstElementValueByTagName(logNode, "Desc"),
                });

                //if (logs.Count >= MaximumLog) 
                //    break;
            }

            _watch.Stop();
            //Console.WriteLine(@"Log Parse : " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            return logs;
        }

        public void Load(String xml)
        {
        }

        public void Save()
        {
            _watch.Reset();
            _watch.Start();

            ReadyStatus = ManagerReadyState.Saving;

            SaveDelegate saveServerSettingDelegate = SaveServerSetting;
            saveServerSettingDelegate.BeginInvoke(SaveCallback, saveServerSettingDelegate);
        }

        public void Save(String xml)
        {
            switch (xml)
            {
                case "DateTime":
                    if (Platform == Platform.Linux)
                    {
                        ApplicationForms.ShowLoadingIcon(Server.Form);
                        Application.RaiseIdle(null);

                        SaveTimeConfig();

                        TopMostMessageBox.Show(Localization["Application_SaveCompleted"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ApplicationForms.HideProgressBar();
                    }
                    break;
            }
        }

        protected Boolean _saveServerFlag;
        protected virtual void SaveServerSetting()
        {
            _saveServerFlag = false;
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadServer, Server.Credential);

            //retry 10 times!
            var retrys = 10;
            while (retrys > 0 && xmlDoc == null)
            {
                xmlDoc = Xml.LoadXmlFromHttp(CgiLoadServer, Server.Credential);
                if (xmlDoc != null) break;
                Thread.Sleep(3000); //wait 3 sec to get cgi again. maybe server is RE-STARTING
                retrys--;
            }

            //still no config -> server is down(or port changed)
            if (xmlDoc == null)
            {
                ReadyStatus = ManagerReadyState.Unavailable;
                return;
            }

            var isPortChange = false;
            IsPortChange = IsSSLPortChange = false;
            var node = xmlDoc.SelectSingleNode("Config");
            if (node != null)
            {
                if (Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Port")) != Port)
                {
                    IsPortChange = true; //for alert msg check
                }

                var sslportNode = Xml.GetFirstElementValueByTagName(node, "SSLPort");
                if (!String.IsNullOrEmpty(sslportNode))
                {
                    if (Convert.ToUInt16(sslportNode) != SSLPort && SSLPort > 0)
                    {
                        IsSSLPortChange = true; //for alert msg check
                    }
                }
                else
                {
                    IsSSLPortChange = true;
                }

                var upnpNode = Xml.GetFirstElementValueByTagName(node, "EnableUPNP");
                if (!String.IsNullOrEmpty(upnpNode))
                {
                    if (EnableUPNP != (upnpNode == "true"))
                    {
                        isPortChange = true;
                    }
                }
                else
                {
                    isPortChange = true;
                }
            }

            if (Server is IFOS)
            {
                if (IsPortChange)
                    isPortChange = true;
            }
            else
            {
                if (IsPortChange || IsSSLPortChange)
                    isPortChange = true;
            }


            //when port is not set, dont save back to server
            if (Port == 0)
                isPortChange = false;

            //ICMS inherit INVR,
            if (Server is ICMS)
            {
                //2 service's port is the same, dont save BOTH
                if (SSLPort == Port)
                    isPortChange = false;

                SaveStorage();
            }
            else if (Server is IFOS)
            {
                SaveStorage();
            }
            else if (Server is IPTS)
            {
                //2 service's port is the same, dont save BOTH
                if (Database.Port == Port)
                    isPortChange = false;
                else
                    SaveDatabase();
            }
            else if (Server is INVR)
            {
                //2 service's port is the same, dont save BOTH
                if (SSLPort == Port)
                    isPortChange = false;
                //if (Platform == Platform.Linux)
                //    SaveTimeConfig();

                SaveStorage();
            }
            _saveServerFlag = true;

            if (isPortChange)
            {
                SaveServer(xmlDoc);
                if (IsPortChange || IsSSLPortChange)
                    ReadyStatus = ManagerReadyState.MajorModify;
                return;
            }

            IsPortChange = IsSSLPortChange = false;
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            //if (NVR.Form.InvokeRequired)
            //{
            //    try
            //    {
            //        NVR.Form.Invoke(new SaveCallbackDelegate(SaveCallback), result);
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    return;
            //}

            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            if (_saveServerFlag)
            {
                _watch.Stop();
                Console.WriteLine(@"Server Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

                if (ReadyStatus == ManagerReadyState.Saving)
                    ReadyStatus = ManagerReadyState.Ready;

                if (OnSaveComplete != null)
                    OnSaveComplete(this, null);
            }
        }

        private void SaveServer(XmlDocument xmlDoc)
        {
            if (xmlDoc != null)
            {
                var configNode = xmlDoc.SelectSingleNode("Config");
                if (configNode != null)
                {
                    var portNode = configNode.SelectSingleNode("Port");
                    if (portNode != null && Port > 0)
                        portNode.InnerText = Port.ToString();

                    var sslPortNode = configNode.SelectSingleNode("SSLPort");
                    if (sslPortNode != null && SSLPort > 0)
                        sslPortNode.InnerText = SSLPort.ToString();

                    var upnpNode = configNode.SelectSingleNode("EnableUPNP");
                    if (upnpNode != null)
                        upnpNode.InnerText = EnableUPNP ? "true" : "false";
                    else
                    {
                        configNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EnableUPNP",
                                                                            EnableUPNP ? "true" : "false"));
                    }
                    if (portNode != null || sslPortNode != null || upnpNode != null)
                        Xml.PostXmlToHttp(CgiSaveServer, xmlDoc, Server.Credential);
                    return;
                }
            }

            SaveServer();
        }

        private void SaveServer()
        {
            //<Config>
            //  <Port>80</Port>
            //  <HomePage>/index.htm</HomePage>
            //	<NonAuthenicationPages>
            //        <Page>/images/app/logo.png</Page>
            //  </NonAuthenicationPages>
            //</Config>
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Config");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", Port));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SSLPort", SSLPort));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EnableUPNP", EnableUPNP ? "true" : "false"));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "HomePage", "/index.htm"));
            var nonAuthenicationPages = xmlDoc.CreateElement("NonAuthenicationPages");
            xmlRoot.AppendChild(nonAuthenicationPages);
            nonAuthenicationPages.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Page", "/images/app/logo.png"));

            Xml.PostXmlToHttp(CgiSaveServer, xmlDoc, Server.Credential);
        }

        public void SaveStorage()
        {
            //Compare Change before save
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadStorage, Server.Credential);
            var storageChange = true;

            if (xmlDoc != null)
            {
                var xmlNode = (XmlElement)xmlDoc.SelectSingleNode("StorageConfiguration");
                if (xmlNode != null)
                {
                    Boolean hasSame = false;

                    var paths = xmlNode.GetElementsByTagName("Path");
                    if (paths.Count == ChangedStorage.Count)
                    {
                        foreach (XmlElement node in paths)
                        {
                            hasSame = false;
                            String drive = node.InnerText.Split(':')[0];
                            foreach (Storage storage in ChangedStorage)
                            {
                                if (storage.Key == drive && storage.KeepSpace == Convert.ToUInt16(node.GetAttribute("keepspace")))
                                {
                                    hasSame = true;
                                    break;
                                }
                            }
                            if (!hasSame)
                                break;
                        }
                        if (hasSame)
                            storageChange = false;
                    }

                    //-------------------------------------------------------------------------------------------
                    //check change of default and enabled
                    var deviceRecordKeepDaysNode = xmlNode.GetElementsByTagName("DeviceRecordKeepDays");
                    if (deviceRecordKeepDaysNode.Count > 0)
                    {
                        var checkDeviceRecordKeepDays = ((XmlElement)deviceRecordKeepDaysNode[0]);
                        if (checkDeviceRecordKeepDays != null)
                        {
                            var keepDaysEnabled = checkDeviceRecordKeepDays.GetAttributeNode("enabled") == null ? false : checkDeviceRecordKeepDays.GetAttribute("enabled").ToLower() == "true";
                            var defaultDeepDays = (ushort)(checkDeviceRecordKeepDays.GetAttributeNode("default") == null ? 90 : Convert.ToUInt16(checkDeviceRecordKeepDays.GetAttribute("default")));

                            if (KeepDaysEnabled != keepDaysEnabled)
                                storageChange = true;

                            if (DefaultDeepDays != defaultDeepDays)
                                storageChange = true;
                        }
                    }

                    //check device without keep days recording 
                    if (DeviceRecordKeepDays.Count == 0)
                    {
                        var loadDeviceRecordKeepDays = xmlNode.GetElementsByTagName("Device");
                        if (loadDeviceRecordKeepDays.Count > 0)
                            storageChange = true;
                    }

                    var deleteKeepDaysRecordingDevice = new List<UInt16>();
                    foreach (var setting in DeviceRecordKeepDays)
                    {
                        if (!Server.Device.Devices.ContainsKey(setting.Key))
                        {
                            deleteKeepDaysRecordingDevice.Add(setting.Key);
                        }
                    }

                    //delete keep days recording without device
                    if (deleteKeepDaysRecordingDevice.Count > 0)
                    {
                        foreach (ushort deviceId in deleteKeepDaysRecordingDevice)
                            DeviceRecordKeepDays.Remove(deviceId);

                        storageChange = true;
                    }

                    if (!storageChange)
                    {
                        var devices = xmlNode.GetElementsByTagName("Device");

                        if (devices.Count == DeviceRecordKeepDays.Count)
                        {
                            foreach (XmlElement node in devices)
                            {
                                hasSame = false;
                                var deviceId = Convert.ToUInt16(node.GetAttribute("id"));
                                foreach (var setting in DeviceRecordKeepDays)
                                {
                                    if (setting.Key == deviceId && setting.Value == Convert.ToUInt16(node.InnerText))
                                    {
                                        hasSame = true;
                                        break;
                                    }
                                }
                                if (!hasSame)
                                    break;
                            }
                            if (!hasSame)
                                storageChange = true;
                        }
                        else
                        {
                            storageChange = true;
                        }
                    }
                }
            }

            if (!storageChange) return;

            //<StorageConfiguration>
            //  <Path keepspace="1">D:\Record\</Path> 
            //  <Path keepspace="1">C:\Record\</Path> 
            //  <DeviceRecordKeepDays>
            //    <Device id="1">14</Device>
            //    <Device id="2">30</Device>
            //    <Device id="3">5</Device>
            //  </DeviceRecordKeepDays>
            //</StorageConfiguration>

            xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("StorageConfiguration");
            xmlDoc.AppendChild(xmlRoot);

            Storage.Clear();
            foreach (Storage storage in ChangedStorage)
            {
                XmlElement path = xmlDoc.CreateElement("Path");
                if (Platform == Platform.Linux)
                {
                    path.InnerText = path.InnerText = "/mnt/storage/";
                }
                else
                {
                    if (Server is IFOS)
                    {
                        path.InnerText = storage.Key + ":\\BackupRecord\\"; //Fixed folder name for failover server
                    }
                    else if (Server is ICMS)
                    {
                        path.InnerText = storage.Key + ":\\CMSRecord\\"; //Fixed folder name
                    }
                    else
                    {
                        path.InnerText = storage.Key + ":\\Record\\"; //Fixed folder name
                    }
                }

                path.SetAttribute("keepspace", storage.KeepSpace.ToString());
                if (storage.Name != null)
                    path.SetAttribute("name", storage.Name);

                xmlRoot.AppendChild(path);
                Storage.Add(new Storage
                {
                    Key = storage.Key,
                    KeepSpace = storage.KeepSpace,
                });
            }

            //------------------------------------
            XmlElement deviceRecordKeepDays = xmlDoc.CreateElement("DeviceRecordKeepDays");
            deviceRecordKeepDays.SetAttribute("enabled", KeepDaysEnabled.ToString().ToLower());
            deviceRecordKeepDays.SetAttribute("default", DefaultDeepDays.ToString());
            xmlRoot.AppendChild(deviceRecordKeepDays);

            foreach (var setting in DeviceRecordKeepDays)
            {
                if (!Server.Device.Devices.ContainsKey(setting.Key)) continue;

                XmlElement deviceNode = xmlDoc.CreateElement("Device");
                deviceNode.SetAttribute("id", setting.Key.ToString());
                deviceNode.InnerText = (setting.Value == 0) ? DefaultDeepDays.ToString() : setting.Value.ToString();
                deviceRecordKeepDays.AppendChild(deviceNode);
            }
            //------------------------------------

            if (Storage.Count == 0)
            {
                if (Platform == Platform.Linux)
                {
                    var path = xmlDoc.CreateElement("Path");
                    path.InnerText = "/mnt/storage/";
                    path.SetAttribute("keepspace", "30");
                    xmlRoot.AppendChild(path);
                }
                else
                {
                    //user can now remove ALL storage disk -> no recording
                    //if (ChangedStorage.Count == 0) return;
                }
            }

            Xml.PostXmlToHttp(CgiSaveStorage, xmlDoc, Server.Credential);
        }

        public void SaveTimeConfig()
        {
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Config");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CurrentProfile", "Profile1"));

            var config = xmlDoc.CreateElement("Profile1");
            xmlRoot.AppendChild(config);

            if (!EnableNTPServer)
            {
                var timecode = DateTimes.ToUtc(ChangedDateTime, ChangedTimeZone);

                if (ChangedEnableDaylight && Daylight != 0)
                    timecode = Convert.ToUInt64(Convert.ToInt64(timecode) - (Daylight * 1000));

                config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Time", timecode));
            }

            config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeZone", ChangedTimeZone));
            config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeName", ChangedLocation));
            config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Daylight", ChangedDaylight));
            config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EnableDaylight", ChangedEnableDaylight ? "true" : "false"));
            config.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NTPServer", EnableNTPServer ? NTPServer : ""));

            if (!EnableNTPServer) NTPServer = "";

            Location = ChangedLocation;

            Xml.PostXmlToHttp(CgiSaveTimeConfig, xmlDoc, Server.Credential);

            var updateDateTime = (!EnableDaylight && ChangedEnableDaylight);
            LoadLinuxServerTime();

            if (!EnableNTPServer)
            {
                if (updateDateTime && Daylight != 0)
                {
                    ChangedDateTime = ChangedDateTime.AddSeconds(Daylight);
                }
            }
            if (OnDateTimeUpdate != null)
                OnDateTimeUpdate(this, null);
        }

        public void SaveDatabase()
        {
            //Compare Change before save
            var orgXmlDoc = Xml.LoadXmlFromHttp(CgiLoadDatabase, Server.Credential);

            //<Config>
            //    <IPAddress>localhost</IPAddress>
            //    <Port>83</Port>
            //</Config>

            //save keep months first ehan change db port
            if (DatabaseKeepMonthsReadyStatus != ReadyState.Ready)
            {
                var requestXmlDoc = new XmlDocument();
                var xmlRoot = requestXmlDoc.CreateElement("Request");
                requestXmlDoc.AppendChild(xmlRoot);

                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(requestXmlDoc, "KeepMonth", DatabaseKeepMonths));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(requestXmlDoc, "UTC", Guid.NewGuid().ToString()));

                Xml.PostXmlToHttp(CgiUpdateKeepMonth, requestXmlDoc, Server.Credential, retry: false);
                DatabaseKeepMonthsReadyStatus = ReadyState.Ready;
            }

            if (DatabaseReadyStatus != ReadyState.Ready)
            {
                var xmlDoc = new XmlDocument();
                var xmlRoot = xmlDoc.CreateElement("Config");
                xmlDoc.AppendChild(xmlRoot);

                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IPAddress", Database.Domain));
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", Database.Port));

                if (orgXmlDoc == null || (orgXmlDoc.InnerXml != xmlDoc.InnerXml))
                {
                    Xml.PostXmlToHttp(CgiSaveDatabase, xmlDoc, Server.Credential);
                }
                DatabaseReadyStatus = ReadyState.Ready;
            }
        }

        private delegate void LoadDelegate();
        private void LoadCallback(IAsyncResult result)
        {
            ((LoadDelegate)result.AsyncState).EndInvoke(result);

            if (_loadServerFlag && _loadStorageFlag)
                LoadCompleted();
        }

        private void LoadCompleted()
        {
            _watch.Stop();
            //const String msg = "Server Ready";
            //Console.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));
            ReadyStatus = ManagerReadyState.Ready;

            if (OnLoadComplete != null)
                OnLoadComplete(this, null);
        }

        public Boolean CheckSetupEnabled(String item)
        {
            return DisabledItems.Contains(item) != true;
        }

        public Boolean CheckPorts(UInt16 port)
        {
            return NotAllowPorts.Contains(port) != true;
        }

        public Boolean CheckProductNoToSupport(String supportFunction)
        {
            switch (supportFunction)
            {
                case "snapshot":
                    switch (ProductNo)
                    {
                        case "00121":
                        case "00122":
                            return false;
                    }
                    break;

                case "smartSearch":
                    //switch (ProductNo)
                    //{
                    //    case "00121":
                    //    case "00122":
                    //        return false;
                    //}
                    return true;

                case "bandwidthControl":
                    switch (ProductNo)
                    {
                        case "00010": 
                        case "00005":
                        case "00121":
                        case "00122":
                        case "00123":
                        case "00124":
                            return false;
                    }
                    break;

                case "upgrade":
                    switch (ProductNo)
                    {
                        case "00121":
                        case "00122":
                            return false;
                    }
                    break;

                case "storage":
                    if (Server.Server.Platform == Platform.Linux)
                    {
                        switch (ProductNo)
                        {
                            case "00122":
                            case "00123":
                            case "00124":
                                return true;

                            default:
                                return false;
                        }
                    }
                    return true;

                case "raid":
                    if (Server.Server.Platform == Platform.Linux)
                    {
                        switch (ProductNo)
                        {
                            case "00122":
                            case "00123":
                            case "00124":
                                return false;

                            default:
                                return true;
                        }
                    }
                    return false;

                case "keepDays":
                    switch (ProductNo)
                    {
                        case "00122":
                        case "00123":
                        case "00124":
                            return false;

                        default:
                            return true;
                    }
            }

            return true;
        }

        public UInt16 CheckProductNoToSupportNumber(String supportFunction)
        {
            switch (supportFunction)
            {
                case "liveChannel":
                    switch (ProductNo)
                    {
                        case "00121":
                        case "00122":
                            return 16;
                        default:
                            return 64;
                    }

                case "playbackChannel":
                    switch (ProductNo)
                    {
                        case "00121":
                        case "00122":
                            return 4;
                        default:
                            return 16;
                    }



                case "minimumSizeRequire":
                    switch (ProductNo)
                    {
                        case "00122":
                        case "00123":
                        case "00124":
                            return 1;

                        default:
                            return 30;
                    }

                case "minimizeKeepSpace":
                    switch (ProductNo)
                    {
                        case "00122":
                        case "00123":
                        case "00124":
                            return 1;

                        default:
                            return 15;
                    }

                case "maximizeKeepSpace":
                    switch (ProductNo)
                    {
                        case "00122":
                        case "00123":
                        case "00124":
                            return 1;

                        default:
                            return 50;
                    }

                case "defaultKeepSpace":
                    switch (ProductNo)
                    {
                        case "00122":
                        case "00123":
                        case "00124":
                            return 1;

                        default:
                            return 30;
                    }
            }

            return 0;
        }

        public String DisplayManufactures(String formal)
        {
            var isNotSupport = false;
            if (formal.IndexOf("*") == 0)//ex: *DLink
            {
                isNotSupport = true;
                formal = formal.Substring(1);
            }

            switch (formal)
            {
                case "IPSurveillance":
                    return String.Format("{0}IP Surveillance", isNotSupport ? "*" : String.Empty);

                case "XTS":
                    return String.Format("{0}XTS Corp.", isNotSupport ? "*" : String.Empty);

                case "DLink":
                    return String.Format("{0}D-Link", isNotSupport ? "*" : String.Empty);
            }

            return String.Format("{0}{1}", isNotSupport ? "*" : String.Empty, formal);
        }

        public String FormalManufactures(String display)
        {
            switch (display)
            {
                case "IP Surveillance":
                    return "IPSurveillance";

                case "XTS Corp.":
                    return "XTS";

                case "D-Link":
                    return "DLink";
            }

            return display;
        }
    }
}
