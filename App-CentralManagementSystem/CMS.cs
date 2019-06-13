using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;
using ApplicationForms = App.ApplicationForms;

namespace App_CentralManagementSystem
{
    public class CMS : ICMS
    {
        // Const
        private const String CgiLogout = @"cgi-bin/login?action=logout";
        private const UInt16 LoginTimeout = 60000;
        private const UInt16 SavingTimeout = 60000;

        // Fields
        private readonly System.Timers.Timer _loginTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _savingTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _serverTimeTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _statusTimeOutTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _getAllChannelStatusTimer = new System.Timers.Timer();


        // Constructor
        public CMS()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},
								   {"Application_UtilityInitError", "Utility Initialize Error"},
								   
								   {"App_Loading", "Loading"},
								   {"Loading_Config", "Config"},
								   {"Loading_Device", "Device"},
								   {"Loading_License", "License"},
								   {"Loading_Server", "Server"},
								   {"Loading_User", "User"},
								   {"Loading_NVR", "NVR"},
							   };
            Localizations.Update(Localization);

            Name = "Central Management System";
            ReadyState = ReadyState.New;
            IsListenEvent = true;
            IsPatrolInclude = true;

            LoginProgress = Localization["App_Loading"];
        }


        // Properties
        public IUtility Utility { get; private set; }
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public String Manufacture { get; set; }
        public String Driver { get; set; }
        public UInt16 ServerPort { get; set; }
        public UInt16 ServerStatusCheckInterval { get; set; }
        public List<IDevice> FailoverDeviceList { get; set; }
        public Form Form { get; set; }
        public Boolean IsListenEvent { get; set; }
        public Boolean IsPatrolInclude { get; set; }

        public FailoverSetting FailoverSetting { get; set; }
        public UInt64 ModifiedDate { get; set; }

        public ServerCredential Credential { get; set; }

        public ILicenseManager License { get; private set; }
        public IServerManager Server { get; private set; }
        public IConfigureManager Configure { get; private set; }
        public IUserManager User { get; set; }
        public IDeviceManager Device { get; private set; }
        public INVRManager NVRManager { get; private set; }
        public IIOModelManager IOModel { get; set; }
        public ReadyState ReadyState { get; set; }
        public NVRStatus NVRStatus { get; set; }
        public string LoginProgress { get; set; }
        public Dictionary<String, String> Localization { get; set; }


        public String Status
        {
            get
            {
                return String.Join(Environment.NewLine, new[]
				{
					License.Status,
					Configure.Status,
					User.Status,
					NVRManager.Status, 
					Server.Status, 
				});
            }
        }

        public List<IDevice> ReadDeviceList()
        {
            return null;
        }

        public void ListenNVREvent(INVR nvr)
        {
            if (nvr != null)
            {
                //nvr.OnEventReceive -= NVROnEventReceive;
                //nvr.OnCameraStatusReceive -= NVROnCameraStatusReceive;

                //nvr.OnEventReceive += NVROnEventReceive;
                //nvr.OnCameraStatusReceive += NVROnCameraStatusReceive;

                //check recording status (alert when not recording)
                nvr.Server.LoadStorageInfo();

                //nvr.Utility.StartEventReceive();
                return;
            }

            foreach (KeyValuePair<UInt16, INVR> obj in NVRManager.NVRs)
            {
                nvr = obj.Value;
                if (nvr.ReadyState != ReadyState.Ready) continue;

                nvr.OnDeviceModify -= NVROnDeviceModify;
                //nvr.OnEventReceive -= NVROnEventReceive;
                //nvr.OnCameraStatusReceive -= NVROnCameraStatusReceive;

                nvr.OnDeviceModify += NVROnDeviceModify;
                //nvr.OnEventReceive += NVROnEventReceive;
                //nvr.OnCameraStatusReceive += NVROnCameraStatusReceive;

                //check recording status (alert when not recording)
                //nvr.Server.LoadStorageInfo();

                //nvr.Utility.StartEventReceive();
            }
        }


        // Methods
        public virtual void Initialize()
        {
            try
            {
                Utility = new CMSUtility.CMSUtility
                {
                    Server = this,
                };
            }
            catch (Exception)
            {
                TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            License = new LicenseManager
            {
                Server = this,
            };
            License.Initialize();

            Server = new ServerManager
            {
                Server = this,
            };
            Server.Initialize();

            Configure = CreateConfigureManager(this);
            Configure.Initialize();

            if (User == null)
            {
                User = new UserManager
                        {
                            Server = this,
                        };
                User.Initialize();
            }

            Device = new DeviceManager
            {
                Server = this,
            };
            Device.Initialize();

            NVRManager = new NVRManager
            {
                Server = this,
            };
            NVRManager.Initialize();
        }

        public void Login()
        {
            _getAllChannelStatusTimer.Elapsed += GetAllChannelStatusTimer;
            _getAllChannelStatusTimer.Interval = 30000;//30 secs
            _getAllChannelStatusTimer.Enabled = true;
            _getAllChannelStatusTimer.SynchronizingObject = Form;

            _statusTimeOutTimer.Elapsed += StatusTimeOut;
            _statusTimeOutTimer.Interval = 60000;//30sec + 30sec if no device status return, mark all recording status false
            _statusTimeOutTimer.Enabled = true;
            _statusTimeOutTimer.SynchronizingObject = Form;

            _statusTimeOutTimer.Elapsed += StatusTimeOut;
            _statusTimeOutTimer.Interval = 60000;//30sec + 30sec if no device status return, mark all recording status false
            _statusTimeOutTimer.Enabled = true;
            _statusTimeOutTimer.SynchronizingObject = Form;

            _serverTimeTimer.Elapsed += GetServerTimeTimer;
            _serverTimeTimer.Interval = 60000;//1 min
            _serverTimeTimer.Enabled = true;
            _serverTimeTimer.SynchronizingObject = Form;

            _loginTimer.Elapsed += CheckLoginTimeout;
            _loginTimer.Interval = LoginTimeout;
            _loginTimer.Enabled = true;
            _loginTimer.SynchronizingObject = Form;

            _savingTimer.Elapsed += CheckSaveTimeout;
            _savingTimer.Interval = SavingTimeout;
            _savingTimer.SynchronizingObject = Form;

            LoadServer();
        }

        protected virtual IConfigureManager CreateConfigureManager(IServer server)
        {
            var configureManager = new ConfigureManager
            {
                Server = server,
            };
            return configureManager;
        }

        private delegate void GetAllChannelStatusDelegate();
        private void GetAllChannelStatusTimer(Object sender, EventArgs e)
        {
            try
            {
                if (NVRManager.DeviceChannelTable.Count == 0) return;
                if (Utility != null)
                {
                    GetAllChannelStatusDelegate getAllChannelStatusDelegate = Utility.GetAllChannelStatus;
                    getAllChannelStatusDelegate.BeginInvoke(null, null);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void StatusTimeOut(Object sender, EventArgs e)
        {
            _statusTimeOutTimer.Enabled = false;

            var cameras = new List<ICamera>();
            foreach (var camera in NVRManager.DeviceChannelTable.Keys.OfType<ICamera>())
            {
                camera.Status = CameraStatus.Nosignal;
                cameras.Add(camera);
            }

            if (OnCameraStatusReceive != null)
                OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private void LoadServer()
        {
            Server.OnLoadComplete -= LoadServerCallback;
            Server.OnLoadComplete += LoadServerCallback;
            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_Server"];

            Server.Load();
        }

        private void LoadServerCallback(Object sender, EventArgs e)
        {
            Server.OnLoadComplete -= LoadServerCallback;

            LoadLicense();
        }

        private void LoadLicense()
        {
            License.OnLoadComplete -= LoadLicenseCallback;
            License.OnLoadComplete += LoadLicenseCallback;

            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_License"];
            License.Load();
        }

        private void LoadLicenseCallback(Object sender, EventArgs e)
        {
            License.OnLoadComplete -= LoadLicenseCallback;

            LoadNVR();
        }

        private void LoadNVR()
        {
            NVRManager.OnLoadComplete -= LoadNVRCallback;
            NVRManager.OnLoadComplete += LoadNVRCallback;

            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_NVR"];
            NVRManager.Load();
        }

        private void LoadNVRCallback(Object sender, EventArgs e)
        {
            NVRManager.OnLoadComplete -= LoadNVRCallback;

            LoadUser();
        }

        private void LoadUser()
        {
            User.OnLoadComplete -= LoadUserCallback;
            User.OnLoadComplete += LoadUserCallback;

            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_User"];
            User.Load();
        }

        private void LoadUserCallback(Object sender, EventArgs e)
        {
            User.OnLoadComplete -= LoadUserCallback;

            LoadConfigure();
        }

        private void LoadConfigure()
        {
            Configure.OnLoadComplete -= LoadConfigureCallback;
            Configure.OnLoadComplete += LoadConfigureCallback;

            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_Config"];
            Configure.Load();
        }

        private void LoadConfigureCallback(Object sender, EventArgs e)
        {
            Configure.OnLoadComplete -= LoadConfigureCallback;

            AddNVRPermission();

            while (NVRManager.NVRs.Count > License.Amount)
            {
                NVRManager.NVRs.Remove(NVRManager.NVRs.Last().Key);
            }

            LoadDevice();
        }

        private void LoadDevice()
        {
            Device.OnLoadComplete -= LoadDeviceCallback;
            Device.OnLoadComplete += LoadDeviceCallback;

            LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_Device"];
            Device.Load();
        }

        protected virtual void LoadDeviceCallback(Object sender, EventArgs e)
        {
            Device.OnLoadComplete -= LoadDeviceCallback;

            _loginTimer.Enabled = false;
            ReadyState = ReadyState.Ready;

            RaiseOnLoadComplete(new EventArgs<String>(Status));
        }

        public void Save()
        {
            _savingTimer.Enabled = true;
            ReadyState = ReadyState.Saving;

            ApplicationForms.ShowProgressBar(Form);
            Application.RaiseIdle(null);

            //if separately call manager.save, it will complete in 0 sec, so each time save will trigger save-completed, and save-server MANY times
            SaveConfigure();
        }

        private void SaveConfigure()
        {
            ApplicationForms.ProgressBarValue = 10;

            if (User.Current.Group.CheckPermission("Setup", Permission.General))
            {
                Configure.OnSaveComplete -= SaveConfigureCallback;
                Configure.OnSaveComplete += SaveConfigureCallback;
                Configure.Save();
            }
            else
            {
                SaveUser();
            }
        }

        private void SaveConfigureCallback(Object sender, EventArgs e)
        {
            Configure.OnSaveComplete -= SaveConfigureCallback;

            SaveUser();
        }

        private void SaveUser()
        {
            ApplicationForms.ProgressBarValue = 30;

            if (User.Current.Group.CheckPermission("Setup", Permission.User))
            {
                User.OnSaveComplete -= SaveUserCallback;
                User.OnSaveComplete += SaveUserCallback;
                User.Save();
            }
            else
                SaveDevice();
        }

        private void SaveUserCallback(Object sender, EventArgs e)
        {
            User.OnSaveComplete -= SaveUserCallback;

            SaveDevice();
        }

        private void SaveDevice()
        {
            ApplicationForms.ProgressBarValue = 50;

            if (User.Current.Group.CheckPermission("Setup", Permission.Device))
            {
                Device.OnSaveComplete -= SaveDeviceCallback;
                Device.OnSaveComplete += SaveDeviceCallback;
                Device.Save();
            }
            else
                SaveNVR();
        }

        protected virtual void SaveDeviceCallback(Object sender, EventArgs e)
        {
            Device.OnSaveComplete -= SaveDeviceCallback;

            SaveNVR();
        }

        private void SaveNVR()
        {
            ApplicationForms.ProgressBarValue = 70;

            if (User.Current.Group.CheckPermission("Setup", Permission.NVR))
            {
                NVRManager.OnSaveComplete -= SaveNVRCallback;
                NVRManager.OnSaveComplete += SaveNVRCallback;
                NVRManager.Save();
            }
            else
                SaveServer();
        }

        private void SaveNVRCallback(Object sender, EventArgs e)
        {
            NVRManager.OnSaveComplete -= SaveNVRCallback;

            SaveServer();
        }

        private void SaveServer()
        {
            ApplicationForms.ProgressBarValue = 90;

            if (User.Current.Group.CheckPermission("Setup", Permission.Server))
            {
                Server.OnSaveComplete -= SaveServerCallback;
                Server.OnSaveComplete += SaveServerCallback;
                Server.Save();
            }
            else
                CMSSaveComplete();
        }

        private void SaveServerCallback(Object sender, EventArgs e)
        {
            CMSSaveComplete();
        }

        private void CMSSaveComplete()
        {
            ReadyState = ReadyState.Ready;
            Server.OnSaveComplete -= SaveServerCallback;
            _savingTimer.Enabled = false;

            ApplicationForms.ProgressBarValue = 100;

            RaiseOnSaveComplete(new EventArgs<String>(Status));

            ApplicationForms.HideProgressBar();
        }

        public override String ToString()
        {
            return "CMS";
        }

        public void Logout()
        {
            Xml.LoadXmlFromHttp(CgiLogout, Credential, 2, false, false);

            foreach (KeyValuePair<UInt16, INVR> obj in NVRManager.NVRs)
            {
                if (obj.Value.Utility != null)
                {
                    obj.Value.Utility.StopEventReceive();
                    obj.Value.Utility.StopAudioTransfer();
                }
            }
        }

        private void CheckLoginTimeout(Object sender, EventArgs e)
        {
            License.OnLoadComplete -= LoadLicenseCallback;
            Server.OnLoadComplete -= LoadServerCallback;
            NVRManager.OnLoadComplete -= LoadNVRCallback;
            User.OnLoadComplete -= LoadUserCallback;
            Configure.OnLoadComplete -= LoadConfigureCallback;
            Device.OnLoadComplete -= LoadDeviceCallback;
            _loginTimer.Enabled = false;

            ReadyState = ReadyState.Ready;
            if (OnLoadFailure != null)
                OnLoadFailure(this, new EventArgs<String>(Status));
        }

        private void CheckSaveTimeout(Object sender, EventArgs e)
        {
            Configure.OnSaveComplete -= SaveConfigureCallback;
            User.OnSaveComplete -= SaveUserCallback;
            Device.OnSaveComplete -= SaveDeviceCallback;
            NVRManager.OnSaveComplete -= SaveNVRCallback;
            Server.OnSaveComplete -= SaveServerCallback;
            _savingTimer.Enabled = false;

            ApplicationForms.HideProgressBar();

            if (OnSaveFailure != null)
                OnSaveFailure(this, new EventArgs<String>(Status));
        }

        public void DeviceModify(IDevice device)
        {
            if (device.ReadyState == ReadyState.Ready)
            {
                device.ReadyState = ReadyState.Modify;
            }
        }

        public void GroupModify(IDeviceGroup group)
        {
            if (group.ReadyState == ReadyState.Ready)
                group.ReadyState = ReadyState.Modify;

            if (OnGroupModify != null)
                OnGroupModify(this, new EventArgs<IDeviceGroup>(group));
        }

        public void NVRModify(INVR nvr)
        {
            switch (nvr.ReadyState)
            {
                case ReadyState.Delete:
                    if (nvr.Utility != null)
                    {
                        nvr.Utility.StopEventReceive();
                        nvr.Utility.StopAudioTransfer();
                    }

                    foreach (var obj in Device.Groups)
                    {
                        if (!obj.Value.Items.Any(device => (device.Server == nvr))) continue;

                        var list = obj.Value.Items.FindAll(device => device.Server == nvr);
                        foreach (var device in list)
                        {
                            obj.Value.Items.Remove(device);
                            obj.Value.View.Remove(device);
                        }
                        if (OnGroupModify != null)
                            OnGroupModify(this, new EventArgs<IDeviceGroup>(obj.Value));
                    }

                    foreach (var obj in User.Current.DeviceGroups)
                    {
                        if (!obj.Value.Items.Any(device => (device.Server == nvr))) continue;

                        var list = obj.Value.Items.FindAll(device => device.Server == nvr);
                        foreach (var device in list)
                        {
                            obj.Value.Items.Remove(device);
                            obj.Value.View.Remove(device);
                        }
                        if (OnGroupModify != null)
                            OnGroupModify(this, new EventArgs<IDeviceGroup>(obj.Value));
                    }

                    break;

                case ReadyState.JustAdd:
                    foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
                    {
                        if (!obj.Value.Group.IsFullAccessToDevices) continue;

                        if (!obj.Value.NVRPermissions.ContainsKey(nvr))
                            obj.Value.NVRPermissions.Add(nvr, new List<Permission> { Permission.Access });
                    }
                    break;
            }

            if (OnNVRModify != null)
                OnNVRModify(this, new EventArgs<INVR>(nvr));
        }

        public void DeviceLayoutModify(IDeviceLayout deviceLayout)
        {
        }

        public void SubLayoutModify(ISubLayout subLayout)
        {
        }

        public void AddNVRPermission()
        {
            foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
            {
                IUser user = obj.Value;
                //Full permission of all devices
                if (user.Group.IsFullAccessToDevices)
                {
                    foreach (KeyValuePair<UInt16, INVR> nvr in NVRManager.NVRs)
                    {
                        user.NVRPermissions.Add(nvr.Value, new List<Permission> { Permission.Access });
                    }
                    continue;
                }

                user.NVRPermissions.Clear();
                //Other User Group, like "user" & guest have device permission need to parse
                if (!User.UserStringPermission.ContainsKey(user))
                    continue;

                IEnumerable<String> permissions = User.UserStringPermission[user];

                foreach (String permission in permissions)
                {
                    var nvrPermission = permission.Split('.');
                    if (nvrPermission.Length != 3) continue;
                    if (nvrPermission[0] != "NVR") continue;
                    if (!Enum.IsDefined(typeof(Permission), nvrPermission[2])) continue;

                    var nvr = NVRManager.FindNVRById(Convert.ToUInt16(nvrPermission[1]));
                    if (nvr == null) continue;

                    var availablePermission = (Permission)Enum.Parse(typeof(Permission), nvrPermission[2], true);
                    if (!user.NVRPermissions.ContainsKey(nvr))
                        user.NVRPermissions.Add(nvr, new List<Permission> { availablePermission });
                    else
                        user.NVRPermissions[nvr].Add(availablePermission);
                }
            }

            var nvrs = new List<INVR>(NVRManager.NVRs.Values);

            foreach (var nvr in nvrs)
            {
                if (!User.Current.CheckPermission(nvr, Permission.Access))
                    NVRManager.NVRs.Remove(nvr.Id);
            }
        }

        private delegate void GetServerTimeDelegate();
        private void GetServerTimeTimer(Object sender, EventArgs e)
        {
            try
            {
                GetServerTimeDelegate getServerTimeDelegate = Server.LoadServerTime;
                getServerTimeDelegate.BeginInvoke(null, null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public Dictionary<ushort, IDevice> TempDevices
        {
            get { return null; }
        }

        public void AddDevicePermission()
        {
        }

        public void SilentLoad()
        {
        }

        public void UndoReload()
        {
            //ApplicationForms.ShowProgressBar(Form);
            Application.RaiseIdle(null);
            ApplicationForms.ProgressBarValue = 80;
            _loginTimer.Enabled = true;

            LoadServer();
        }

        public void StopTimer()
        {
        }

        public Boolean ValidateCredential()
        {
            return true;
        }

        public Boolean ValidateCredentialWithMessage()
        {
            return true;
        }

        public Dictionary<UInt16, IDevice> ReadNVRDeviceWithoutSaving()
        {
            return null;
        }

        private void NVROnDeviceModify(Object sender, EventArgs<IDevice> e)
        {
            if (e.Value == null) return;
            if (Device.Groups.Count == 0) return;

            var device = e.Value;
            if (device.ReadyState != ReadyState.Delete) return;

            foreach (var obj in Device.Groups)
            {
                if (!obj.Value.Items.Contains(device)) continue;

                obj.Value.Items.Remove(device);
                obj.Value.View.Remove(device);

                if (OnGroupModify != null)
                    OnGroupModify(this, new EventArgs<IDeviceGroup>(obj.Value));
            }
        }

        public void UtilityOnServerEventReceive(String msg)
        {
            var xmlDoc = Xml.LoadXml(msg);
            var rootNode = xmlDoc.FirstChild;

            if (rootNode == null) return;

            if (rootNode.Name == "CPUUsage")
            {
                String from = Xml.GetFirstElementValueByTagName(xmlDoc, "From");
                if (from == "CMS")
                    Server.CPUUsage = Convert.ToInt32(Xml.GetFirstElementValueByTagName(xmlDoc, "Status"));
                return;
            }

            if (rootNode.Name == "Message" && OnCameraStatusReceive != null)
            {
                ParseDeviceStatus(xmlDoc);
                return;
            }

            if (rootNode.Name == "AudioOutStatus" && OnCameraStatusReceive != null)
            {
                ParseAudioOut(xmlDoc);
                Console.WriteLine(DateTime.Now);
                Console.WriteLine(xmlDoc.InnerText);
                Console.WriteLine(DateTime.Now);
                return;
            }

            //--------------

            if (rootNode.Name != "Event" || OnEventReceive == null || !IsListenEvent) return;
            //<Event>
            //    <DeviceID>4</DeviceID>
            //    <Type>Motion</Type>
            //    <LocalTime>1310457814548</LocalTime>
            //    <DeviceTime>1310457814548</DeviceTime>
            //    <Count>2</Count>
            //    <Status id="1" trigger="1" value="1">Hello</Status>
            //    <Status id="3" trigger="1" value="1">Hello</Status>
            //</Event>
            //Console.WriteLine(xmlDoc.InnerXml);
            ParseEvent(xmlDoc, rootNode);
        }

        private void ParseDeviceStatus(XmlDocument xmlDoc)
        {
            _statusTimeOutTimer.Enabled = false;
            _statusTimeOutTimer.Enabled = true;

            String type = Xml.GetFirstElementValueByTagName(xmlDoc, "Type");
            String nvrId = Xml.GetFirstElementValueByTagName(xmlDoc, "NvrID");
            if (!NVRManager.NVRs.ContainsKey(Convert.ToUInt16(nvrId))) return;
            var nvr = NVRManager.NVRs[Convert.ToUInt16(nvrId)];

            if (type == "NVR_STATUS_RSP")
            {
                String status = Xml.GetFirstElementValueByTagName(xmlDoc, "Status");

                if (status != "Green" && nvr.NVRStatus == NVRStatus.Health)
                {
                    //because no nvr singal, no device status, devices shuould show no-signal
                    foreach (KeyValuePair<ushort, IDevice> device in nvr.Device.Devices)
                    {
                        if (device.Value.ReadyState == ReadyState.New) continue;

                        var camera = device.Value as ICamera;
                        if (camera == null) continue;

                        camera.Status = CameraStatus.Nosignal;
                    }
                }

                switch (status)
                {
                    case "Yellow":
                        nvr.NVRStatus = NVRStatus.WrongAccountPassowrd;
                        break;

                    case "Green":
                        nvr.NVRStatus = NVRStatus.Health;
                        break;

                    default:
                        nvr.NVRStatus = NVRStatus.Bad;
                        break;
                }

                if (OnNVRStatusReceive != null)
                    OnNVRStatusReceive(this, new EventArgs<INVR>(nvr));
                return;
            }

            var devicesNode = xmlDoc.GetElementsByTagName("Device");

            var cameras = new List<ICamera>();

            foreach (XmlElement deviceNode in devicesNode)
            {
                String deviceId = Xml.GetFirstElementValueByTagName(deviceNode, "IdentifyName");
                if (String.IsNullOrEmpty(deviceId)) continue;

                UInt16 id = 0;
                try
                {
                    id = Convert.ToUInt16(deviceId);
                }
                catch (Exception)
                {
                    continue;
                }
                if (id == 0)
                    continue;

                if (!String.IsNullOrEmpty(nvrId))
                {
                    if (!nvr.Device.Devices.ContainsKey(Convert.ToUInt16(id))) continue;
                    var device = nvr.Device.Devices[Convert.ToUInt16(id)];

                    if (device.ReadyState == ReadyState.New) continue;

                    if (!(device is ICamera)) continue;
                    var camera = device as ICamera;

                    switch (Xml.GetFirstElementValueByTagName(deviceNode, "Status"))
                    {
                        case "recording":
                            camera.Status = CameraStatus.Recording;
                            break;

                        case "streaming":
                            camera.Status = CameraStatus.Streaming;
                            break;

                        default:
                            camera.Status = CameraStatus.Nosignal;
                            break;
                    }

                    cameras.Add(camera);
                }
            }

            if (OnCameraStatusReceive != null)
                OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private void ParseAudioOut(XmlDocument xmlDoc)
        {
            var devices = xmlDoc.GetElementsByTagName("AudioOutStatus");

            if (devices.Count <= 0) return;

            var cameras = new List<ICamera>();
            foreach (XmlElement deviceNode in devices)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(deviceNode, "NVRid");
                if (!NVRManager.NVRs.ContainsKey(Convert.ToUInt16(nvrId))) continue;
                var nvr = NVRManager.NVRs[Convert.ToUInt16(nvrId)];

                var channelNodes = deviceNode.SelectNodes("Channel");
                if (channelNodes == null) continue;
                if (channelNodes.Count == 0) continue;
                var channelNode = channelNodes[0];

                if (channelNode.Attributes != null)
                {
                    String id = channelNode.Attributes["id"].Value;

                    if (id != "")
                    {
                        IDevice device = nvr.Device.Devices[Convert.ToUInt16(id.Replace("channel", ""))];
                        if (device == null) continue;
                        if (device.ReadyState == ReadyState.New) continue;

                        if (!(device is ICamera)) continue;
                        var camera = device as ICamera;

                        if (camera.IsAudioOut == String.Equals(channelNode.InnerText, "on")) continue;

                        camera.IsAudioOut = String.Equals(channelNode.InnerText, "on");
                        cameras.Add(camera);
                    }
                }
            }

            OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private void ParseEvent(XmlDocument xmlDoc, XmlNode rootNode)
        {
            //<Event>
            //<DeviceID>35</DeviceID> 
            //<Type>Motion</Type> 
            //<LocalTime>1399028361823</LocalTime> 
            //<DeviceTime>1399028361823</DeviceTime> 
            //<Count>1</Count> 
            //<Status id="2" trigger="1" value="1" /> 
            //<NvrID>1</NvrID> 
            //</Event>
            var cameraEvents = new List<ICameraEvent>();

            var id = Xml.GetFirstElementValueByTagName(rootNode, "DeviceID");
            var nvrId = Xml.GetFirstElementValueByTagName(rootNode, "NvrID");
            var statusNode = xmlDoc.GetElementsByTagName("Status");

            var eventType = EventType.UserDefine;
            var type = Xml.GetFirstElementValueByTagName(rootNode, "Type");
            switch (type)
            {
                case "Motion":
                case "DigitalInput":
                case "DigitalOutput":
                case "NetworkLoss":
                case "NetworkRecovery":
                case "VideoLoss":
                case "VideoRecovery":
                case "RecordFailed":
                case "RecordRecovery":
                case "UserDefine":
                case "ManualRecord":
                case "Panic":
                case "CrossLine":
                case "IntrusionDetection":
                case "LoiteringDetection":
                case "ObjectCountingIn":
                case "ObjectCountingOut":
                case "AudioDetection":
                case "TamperDetection":
                    eventType = (EventType)Enum.Parse(typeof(EventType), type, true);
                    break;
            }

            if (!String.IsNullOrEmpty(nvrId))
            {
                if (!NVRManager.NVRs.ContainsKey(Convert.ToUInt16(nvrId))) return;
                var nvr = NVRManager.NVRs[Convert.ToUInt16(nvrId)];
                if (!nvr.IsListenEvent) return;

                if (!String.IsNullOrEmpty(id))
                {
                    if (!nvr.Device.Devices.ContainsKey(Convert.ToUInt16(id))) return;
                    var device = nvr.Device.Devices[Convert.ToUInt16(id)];
                    if (device == null) return;

                    foreach (XmlElement status in statusNode)
                    {
                        //Is Trigger
                        if (String.Equals(status.GetAttribute("trigger"), "0")) continue;

                        ICameraEvent cameraEvent = new CameraEvents
                        {
                            Id = Convert.ToUInt16(status.GetAttribute("id")),
                            Device = device,
                            Type = eventType,
                        };

                        if (!CheckCameraEvent(cameraEvent)) continue;

                        cameraEvent.NVR = (INVR)device.Server;
                        cameraEvent.Status = status.InnerText;
                        cameraEvent.Value = (status.GetAttribute("value") == "1");
                        cameraEvent.DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(rootNode, "LocalTime")), Server.TimeZone);

                        cameraEvents.Add(cameraEvent);
                    }
                }
                else
                {
                    var timecode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(rootNode, "LocalTime"));
                    cameraEvents.Add(new CameraEvents
                    {
                        NVR = this,
                        Type = eventType,
                        DateTime = DateTimes.ToDateTime(timecode, Server.TimeZone),
                        Timecode = timecode
                    });
                }
            }

            RaiseOnEventReceive(cameraEvents);
        }

        public Boolean CheckCameraEvent(ICameraEvent cameraEvent)
        {
            if (cameraEvent.Device == null) return false;

            var camera = cameraEvent.Device as ICamera;
            switch (cameraEvent.Type)
            {
                case EventType.DigitalInput:
                    if (camera != null)
                    {
                        if (camera.IOPort.Count > 0)
                        {
                            if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Input)
                                return false;
                        }
                        else
                            if (camera.Model.NumberOfDi < cameraEvent.Id)
                                return false;
                    }
                    else
                        return false;

                    break;

                case EventType.DigitalOutput:
                    if (camera != null)
                    {
                        if (camera.IOPort.Count > 0)
                        {
                            if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Output)
                                return false;
                        }
                        else
                            if (camera.Model.NumberOfDo < cameraEvent.Id)
                                return false;
                    }
                    else
                        return false;

                    break;

                case EventType.Motion:
                    if (camera != null)
                    {
                        if (camera.Model.NumberOfMotion < cameraEvent.Id)
                            return false;
                    }
                    else
                        return false;

                    break;
            }

            return true;
        }

        public void UtilityOnUploadProgress(Int32 progress)
        {
        }

        public void DeletePresetPointRelativeEventHandle(ICamera device, UInt16 pointId)
        {
        }

        private readonly Queue<String> _queueLogToSend = new Queue<String>();
        private WriteOperationLogDelegate _writeOperationLogDelegate;
        public void WriteOperationLog(String msg)
        {
            //do it one by one, dont send too much, will cause server close connection random.

            if (_writeOperationLogDelegate != null)
            {
                _queueLogToSend.Enqueue(msg);
                return;
            }

            _writeOperationLogDelegate = WriteOperationLogOnBackground;
            _writeOperationLogDelegate.BeginInvoke(msg, null, null);
        }

        private delegate void WriteOperationLogDelegate(String msg);
        private void WriteOperationLogOnBackground(String msg)
        {
            Xml.LoadXmlFromHttp("cgi-bin/log?action=add&data=" + msg, Credential, Xml.Timeout, false, false);

            _writeOperationLogDelegate = null;
            if (_queueLogToSend.Count > 0)
                WriteOperationLog(_queueLogToSend.Dequeue());
        }


        // Events
        public event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;

        private void RaiseOnEventReceive(ICameraEvent cameraEvent)
        {
            RaiseOnEventReceive(new List<ICameraEvent> { cameraEvent });
        }

        private void RaiseOnEventReceive(List<ICameraEvent> cameraEvents)
        {
            if (OnEventReceive != null)
                OnEventReceive(this, new EventArgs<List<ICameraEvent>>(cameraEvents));
        }

        public event EventHandler<EventArgs<List<ICamera>>> OnCameraStatusReceive;
        public event EventHandler<EventArgs<INVR>> OnNVRStatusReceive;

        public event EventHandler<EventArgs<IDevice>> OnDeviceModify;
        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupModify;
        public event EventHandler<EventArgs<IDeviceLayout>> OnDeviceLayoutModify;
        public event EventHandler<EventArgs<ISubLayout>> OnSubLayoutModify;
        public event EventHandler<EventArgs<INVR>> OnNVRModify;

        public event EventHandler<EventArgs<String>> OnLoadComplete;

        protected virtual void RaiseOnLoadComplete(EventArgs<string> e)
        {
            var handler = OnLoadComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs<String>> OnLoadFailure;
        public event EventHandler<EventArgs<String>> OnSaveComplete;

        protected virtual void RaiseOnSaveComplete(EventArgs<string> e)
        {
            var handler = OnSaveComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs<String>> OnSaveFailure;

        public event EventHandler OnDevicePackUpdateCompleted;
    }
}
