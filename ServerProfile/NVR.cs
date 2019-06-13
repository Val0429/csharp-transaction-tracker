using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;

namespace ServerProfile
{
    public class NVR : INVR
    {
        protected const String CgiLogin = @"cgi-bin/login?action=login";
        protected const String CgiLogout = @"cgi-bin/login?action=logout";
        protected const String CgiSearchWithoutSavingNVR = @"cgi-bin/nvrconfig?action=getdevicelistByXMLdata";

        public event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;
        public event EventHandler OnDevicePackUpdateCompleted;

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

        public event EventHandler<EventArgs<IDevice>> OnDeviceModify;
        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupModify;
        public event EventHandler<EventArgs<IDeviceLayout>> OnDeviceLayoutModify;
        public event EventHandler<EventArgs<ISubLayout>> OnSubLayoutModify;

        public event EventHandler<EventArgs<String>> OnLoadComplete;
        public event EventHandler<EventArgs<String>> OnLoadFailure;
        public event EventHandler<EventArgs<String>> OnSaveComplete;
        public event EventHandler<EventArgs<String>> OnSaveFailure;

        public IUtility Utility { get; protected set; }
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public String Manufacture { get; set; }
        public String Driver { get; set; }
        public String Model { get; set; }
        public UInt16 ServerPort { get; set; }

        public UInt16 ServerStatusCheckInterval { get; set; }
        public List<IDevice> FailoverDeviceList { get; set; }
        public Form Form { get; set; }
        public Boolean IsListenEvent { get; set; }
        public Boolean IsPatrolInclude { get; set; }
        public Dictionary<UInt16, IDevice> TempDevices { get; set; }
        public FailoverSetting FailoverSetting { get; set; }
        public UInt64 ModifiedDate { get; set; } //last modified date (save)

        public ServerCredential Credential { get; set; }
        public CustomStreamSetting CustomStreamSetting { get; set; }
        public IServer ServerManager { set; get; }
        public ILicenseManager License { get; protected set; }
        public IServerManager Server { get; protected set; }
        public IConfigureManager Configure { get; protected set; }
        public IUserManager User { get; protected set; }
        public IDeviceManager Device { get; protected set; }
        public IIOModelManager IOModel { get; set; }
        public static Boolean DisplayNVRId = true;

        public ReadyState ReadyState { get; set; }
        public NVRStatus NVRStatus { get; set; }
        public string LoginProgress { get; set; }

        private readonly System.Timers.Timer _loginTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _savingTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _statusTimeOutTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _getAllChannelStatusTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _serverTimeTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _storageUsageTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _devicePackUploadCompletedTimer = new System.Timers.Timer();
        public Dictionary<String, String> Localization;


        // Constructor
        public NVR()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   {"MessageBox_Error", "Error"},
								   //{"Application_UtilityInitError", "Utility Initialize Error"},
								   
								   {"App_Loading", "Loading"},
                                   {"Loading_Config", "Config"},
                                   {"Loading_Device", "Device"},
                                   {"Loading_License", "License"},
                                   {"Loading_Server", "Server"},
                                   {"Loading_User", "User"},

                                    {"LoginForm_SignInFailed", "Sign in failed"},
                                    {"LoginForm_SignInFailedConnectFailure", "Can not connect to server. Please confirm host and port is correct."},
                                    {"LoginForm_SignInTimeout", "Login timeout. Please check firewall setting."},
                                    {"LoginForm_SignInFailedAuthFailure", "Login failure. Please confirm account and password is correct."},
                                    {"LoginForm_SignInFailedPortOccupation", "Login failure. Please verify if port %1 is already used by another application."},
                               };
            Localizations.Update(Localization);

            Name = "Network Video Recorder";
            Manufacture = "iSap";
            ReadyState = ReadyState.New;
            IsListenEvent = true;
            IsPatrolInclude = true;
            ServerPort = 8000;
            ServerStatusCheckInterval = 600;
            Credential = new ServerCredential
            {
                Domain = "",
                Port = 80,
                UserName = "Admin",
                Password = "",
            };

            LoginProgress = Localization["App_Loading"];
            TempDevices = new Dictionary<UInt16, IDevice>();
        }

        public virtual String Status
        {
            get
            {
                return String.Join(Environment.NewLine, new[]
                {
                    License.Status,
                    Configure.Status,
                    User.Status,
                    Device.Status,
                    Server.Status,
                });
            }
        }

        public virtual void Initialize()
        {

            try
            {
                if (ServerManager is ICMS == false)
                {
                    if (Utility == null)
                    {
                        Utility = new UtilityAir.UtilityAir()
                        {
                            Server = this,
                        };
                    }
                    else
                        Utility.Server = this;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            if (License == null)
            {
                License = new LicenseManager
                {
                    Server = this,
                };
                License.Initialize();
            }

            if (Server == null)
            {
                Server = new ServerManager
                {
                    Server = this,
                };
                Server.Initialize();
            }

            if (Configure == null)
            {
                Configure = new ConfigureManager
                {
                    Server = this,
                };
                Configure.Initialize();

                if (ServerManager is ICMS)
                {
                    Configure.CustomStreamSetting.Enable = ServerManager.Configure.EnableBandwidthControl;
                }
            }

            if (User == null)
            {
                User = new UserManager
                {
                    Server = this,
                };
                User.Initialize();
            }

            if (Device == null)
            {
                Device = new DeviceManager
                {
                    Server = this,
                };
                Device.Initialize();
            }

        }

        private const UInt16 LoginTimeout = 60000;
        private Boolean _isAttachEvent;
        public virtual void Login()
        {
            if (!_isAttachEvent)
            {
                _getAllChannelStatusTimer.Elapsed += GetAllChannelStatusTimer;
                _getAllChannelStatusTimer.Interval = 30000;//30 secs
                _getAllChannelStatusTimer.Enabled = true;
                _getAllChannelStatusTimer.SynchronizingObject = Form;

                _statusTimeOutTimer.Elapsed += StatusTimeOut;
                _statusTimeOutTimer.Interval = 60000;//30sec + 30sec if no device status return, mark all recording status false
                _statusTimeOutTimer.Enabled = true;
                _statusTimeOutTimer.SynchronizingObject = Form;

                _storageUsageTimer.Elapsed += ShowStorageUsage;
                _storageUsageTimer.Interval = 60000; //1 min
                _storageUsageTimer.Enabled = true;
                _storageUsageTimer.SynchronizingObject = Form;

                _serverTimeTimer.Elapsed += GetServerTimeTimer;
                _serverTimeTimer.Interval = 60000;//1 min
                _serverTimeTimer.Enabled = true;
                _serverTimeTimer.SynchronizingObject = Form;

                _devicePackUploadCompletedTimer.Elapsed += UploadCompleted;
                _devicePackUploadCompletedTimer.Interval = 2000;//5sec
                _devicePackUploadCompletedTimer.SynchronizingObject = Form;

                _loginTimer.Elapsed += CheckLoginTimeout;
                _loginTimer.Interval = LoginTimeout;
                _loginTimer.Enabled = true;
                _loginTimer.SynchronizingObject = Form;

                _savingTimer.Elapsed += CheckSaveTimeout;
                _savingTimer.Interval = SavingTimeout;
                _savingTimer.SynchronizingObject = Form;

                Server.OnStorageStatusUpdate += ServerOnStorageStatusUpdate;

                _isAttachEvent = true;
            }

            LoadServer();
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

            _loginTimer.Enabled = false;
            ReadyState = ReadyState.Ready;

            AddDevicePermission();

            if (OnLoadComplete != null)
                OnLoadComplete(this, new EventArgs<String>(Status));
        }

        private const UInt16 SavingTimeout = 60000;
        public virtual void Save()
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
            ApplicationForms.ProgressBarValue = 20;

            if (User.Current.Group.CheckPermission("Setup", Permission.General))
            {
                Configure.OnSaveComplete -= SaveConfigureCallback;
                Configure.OnSaveComplete += SaveConfigureCallback;
                Configure.Save();
            }
            else
                SaveUser();
        }

        private void SaveConfigureCallback(Object sender, EventArgs e)
        {
            Configure.OnSaveComplete -= SaveConfigureCallback;

            SaveUser();
        }

        private void SaveUser()
        {
            ApplicationForms.ProgressBarValue = 40;

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
            ApplicationForms.ProgressBarValue = 60;

            if (User.Current.Group.CheckPermission("Setup", Permission.Device))
            {
                Device.OnSaveComplete -= SaveDeviceCallback;
                Device.OnSaveComplete += SaveDeviceCallback;
                Device.Save();
            }
            else
                SaveServer();
        }

        protected virtual void SaveDeviceCallback(Object sender, EventArgs e)
        {
            Device.OnSaveComplete -= SaveDeviceCallback;

            SaveServer();
        }

        private void SaveServer()
        {
            ApplicationForms.ProgressBarValue = 80;

            if (User.Current.Group.CheckPermission("Setup", Permission.Server))
            {
                Server.OnSaveComplete -= SaveServerCallback;
                Server.OnSaveComplete += SaveServerCallback;

                //reload license amount after save system datetime, change system datetime will cause license amount change(license expire)
                var loadLicense = (!Server.EnableNTPServer && Server.ChangedDateTime != Server.DateTime);
                Server.Save();
                if (loadLicense) License.Load();
            }
            else
                NVRSaveComplete();
        }

        private void SaveServerCallback(Object sender, EventArgs e)
        {
            Server.OnSaveComplete -= SaveServerCallback;

            NVRSaveComplete();
        }

        private void NVRSaveComplete()
        {
            ReadyState = ReadyState.Ready;
            Server.OnSaveComplete -= SaveServerCallback;
            _savingTimer.Enabled = false;

            ApplicationForms.ProgressBarValue = 100;

            if (OnSaveComplete != null)
                OnSaveComplete(this, new EventArgs<String>(Status));

            ApplicationForms.HideProgressBar();
        }

        public void Logout()
        {
            Xml.LoadXmlFromHttp(CgiLogout, Credential, 2, false, false);

            _getAllChannelStatusTimer.Enabled =
            _storageUsageTimer.Enabled =
            _serverTimeTimer.Enabled = false;
        }

        protected virtual void CheckLoginTimeout(Object sender, EventArgs e)
        {
            License.OnLoadComplete -= LoadLicenseCallback;
            Server.OnLoadComplete -= LoadServerCallback;
            Device.OnLoadComplete -= LoadDeviceCallback;
            User.OnLoadComplete -= LoadUserCallback;
            Configure.OnLoadComplete -= LoadConfigureCallback;
            _loginTimer.Enabled = false;

            ReadyState = ReadyState.Unavailable;

            //clear device after timeout
            Device.Devices.Clear();
            Device.Groups.Clear();

            if (OnLoadFailure != null)
                OnLoadFailure(this, new EventArgs<String>(Status));
        }

        protected virtual void CheckSaveTimeout(Object sender, EventArgs e)
        {
            Configure.OnSaveComplete -= SaveConfigureCallback;
            User.OnSaveComplete -= SaveUserCallback;
            Device.OnSaveComplete -= SaveDeviceCallback;
            Server.OnSaveComplete -= SaveServerCallback;
            _savingTimer.Enabled = false;

            ReadyState = ReadyState.Modify;

            ApplicationForms.HideProgressBar();

            if (OnSaveFailure != null)
                OnSaveFailure(this, new EventArgs<String>(Status));
        }

        public void AddDevicePermission()
        {
            foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
            {
                IUser user = obj.Value;
                if (user.Group == null) continue;
                //for kevin
                //if (!string.IsNullOrEmpty(user.DisplayName))
                //{
                //    continue;                 
                //    user.Permissions = user.Group.DevicePermissions;

                //}
                //-----------------------------
                //Full permission of all devices
                if (user.Group.IsFullAccessToDevices)
                {
                    foreach (KeyValuePair<UInt16, IDevice> device in Device.Devices)
                    {
                        user.AddFullDevicePermission(device.Value);
                    }
                    continue;
                }

                //Other User Group, like "user" & guest have device permission need to parse

                user.Permissions.Clear();

                if (!User.UserStringPermission.ContainsKey(user))
                    continue;

                IEnumerable<String> permissions = User.UserStringPermission[user];

                foreach (String permission in permissions)
                {
                    var devicePermission = permission.Split('.');
                    if (devicePermission.Length != 3) continue;
                    if (devicePermission[0] != "Device") continue;
                    if (!Enum.IsDefined(typeof(Permission), devicePermission[2])) continue;

                    var device = Device.FindDeviceById(Convert.ToUInt16(devicePermission[1]));
                    if (device == null) continue;

                    var availablePermission = (Permission)Enum.Parse(typeof(Permission), devicePermission[2], true);
                    if (!user.Permissions.ContainsKey(device))
                        user.Permissions.Add(device, new List<Permission> { availablePermission });
                    else
                        user.Permissions[device].Add(availablePermission);
                }
            }
        }

        public void StopTimer()
        {
            _storageUsageTimer.Enabled = _serverTimeTimer.Enabled = false;
        }

        public void SilentLoad()
        {
            _loginTimer.Enabled = true;

            LoadServer();
        }

        public void UndoReload()
        {
            ApplicationForms.ProgressBarValue = 80;
            _loginTimer.Enabled = true;

            //send device & group modift to let other control know it time to reload
            if (OnGroupModify != null)
            {
                foreach (var deviceGroup in Device.Groups)
                {
                    OnGroupModify(this, new EventArgs<IDeviceGroup>(deviceGroup.Value));
                }
            }
            if (OnDeviceModify != null)
            {
                foreach (var device in Device.Devices)
                {
                    OnDeviceModify(this, new EventArgs<IDevice>(device.Value));
                }
            }

            LoadServer();
        }

        public void DeviceModify(IDevice device)
        {
            //Remove device permission from user
            switch (device.ReadyState)
            {
                case ReadyState.Ready:
                    device.ReadyState = ReadyState.Modify;
                    //Modify relative PIP
                    if (Server.SupportPIP)
                        PIPCameraModify(device);
                    break;

                case ReadyState.Delete:
                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Device.Groups)
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
                        //remove last null device
                        //while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                        //{
                        //    //group.View.RemoveAt(group.View.Count - 1);
                        //}

                        GroupModify(obj.Value);
                    }

                    //delete all users' private view to avoid adding back the same channel id will show un-authorized camera
                    foreach (KeyValuePair<UInt16, IUser> user in User.Users)
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

                            //while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                            //{
                            //    //group.View.RemoveAt(group.View.Count - 1);
                            //}

                            GroupModify(obj.Value);
                        }
                    }

                    foreach (KeyValuePair<UInt16, IDeviceLayout> obj in Device.DeviceLayouts)
                    {
                        var layout = obj.Value;
                        if (!layout.Items.Contains(device)) continue;

                        while (layout.Items.Contains(device))
                        {
                            layout.Items[layout.Items.IndexOf(device)] = null;
                        }

                        DeviceLayoutModify(layout);
                        foreach (var subLayout in layout.SubLayouts)
                        {
                            if (subLayout.Value == null) continue;

                            SubLayoutModify(subLayout.Value);
                        }
                    }

                    foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
                    {
                        obj.Value.Permissions.Remove(device);
                    }

                    //Modify relative PIP
                    if (Server.SupportPIP)
                        PIPCameraModify(device);

                    //Remove relative event
                    foreach (var obj in Device.Devices)
                    {
                        if (!(obj.Value is ICamera)) continue;
                        if (obj.Value == device) continue;

                        var camera = ((ICamera)obj.Value);
                        foreach (var list in camera.EventHandling)
                        {
                            var removeList = new List<EventHandle>();
                            foreach (var eventHandle in list.Value)
                            {
                                if (eventHandle is HotSpotEventHandle)
                                {
                                    if (((HotSpotEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is GotoPresetEventHandle)
                                {
                                    if (((GotoPresetEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is PopupPlaybackEventHandle)
                                {
                                    if (((PopupPlaybackEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is PopupLiveEventHandle)
                                {
                                    if (((PopupLiveEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is TriggerDigitalOutEventHandle)
                                {
                                    if (((TriggerDigitalOutEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is SendMailEventHandle)
                                {
                                    if (((SendMailEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }

                                if (eventHandle is UploadFtpEventHandle)
                                {
                                    if (((UploadFtpEventHandle)eventHandle).Device == device)
                                        removeList.Add(eventHandle);
                                }
                            }

                            if (removeList.Count > 0)
                            {
                                DeviceModify(camera);
                                camera.EventHandling.ReadyState = ReadyState.Modify;
                                foreach (var eventHandle in removeList)
                                {
                                    list.Value.Remove(eventHandle);
                                }
                            }
                        }
                    }
                    break;

                case ReadyState.JustAdd:
                    foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
                    {
                        if (!obj.Value.Group.IsFullAccessToDevices) continue;

                        obj.Value.AddFullDevicePermission(device);
                    }
                    device.ReadyState = ReadyState.New;
                    //if you want to get add new device notice, you should use  OnGroupModify
                    return; //<--it's return not break
            }

            if (OnDeviceModify != null)
                OnDeviceModify(this, new EventArgs<IDevice>(device));
        }

        private void PIPCameraModify(IDevice modifyDevice)
        {
            var modifyCamera = modifyDevice as ICamera;
            if (modifyCamera == null) return;

            foreach (KeyValuePair<UInt16, IDevice> device in Device.Devices)
            {
                if (device.Value.ReadyState != ReadyState.Ready) continue;
                var camera = device.Value as ICamera;
                if (camera == null) continue;
                if (camera.PIPDevice == modifyCamera)
                {
                    device.Value.ReadyState = ReadyState.Modify;
                    if (OnDeviceModify != null)
                        OnDeviceModify(this, new EventArgs<IDevice>(device.Value));
                }
            }
        }

        public void GroupModify(IDeviceGroup group)
        {
            if (group.ReadyState == ReadyState.Ready)
                group.ReadyState = ReadyState.Modify;

            if (OnGroupModify != null)
                OnGroupModify(this, new EventArgs<IDeviceGroup>(group));
        }

        public void DeviceLayoutModify(IDeviceLayout deviceLayout)
        {
            switch (deviceLayout.ReadyState)
            {
                case ReadyState.Ready:
                    deviceLayout.ReadyState = ReadyState.Modify;
                    break;

                case ReadyState.Delete:
                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Device.Groups)
                    {
                        var group = obj.Value;
                        if (!group.Items.Contains(deviceLayout)) continue;

                        group.Items.Remove(deviceLayout);
                        group.View.Remove(deviceLayout);

                        while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                        {
                            group.View.RemoveAt(group.View.Count - 1);
                        }

                        GroupModify(obj.Value);
                    }

                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in User.Current.DeviceGroups)
                    {
                        var group = obj.Value;
                        if (!group.Items.Contains(deviceLayout)) continue;

                        group.Items.Remove(deviceLayout);
                        group.View.Remove(deviceLayout);

                        while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                        {
                            group.View.RemoveAt(group.View.Count - 1);
                        }

                        GroupModify(obj.Value);
                    }

                    foreach (var subLayout in deviceLayout.SubLayouts)
                    {
                        if (subLayout.Value == null) continue;

                        subLayout.Value.ReadyState = ReadyState.Delete;
                        SubLayoutModify(subLayout.Value);
                    }
                    break;
            }

            if (OnDeviceLayoutModify != null)
                OnDeviceLayoutModify(this, new EventArgs<IDeviceLayout>(deviceLayout));
        }

        public void SubLayoutModify(ISubLayout subLayout)
        {
            switch (subLayout.ReadyState)
            {
                case ReadyState.Ready:
                    subLayout.ReadyState = ReadyState.Modify;
                    break;

                case ReadyState.Delete:
                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in Device.Groups)
                    {
                        var group = obj.Value;
                        if (!group.Items.Contains(subLayout)) continue;

                        group.Items.Remove(subLayout);
                        group.View.Remove(subLayout);

                        while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                        {
                            group.View.RemoveAt(group.View.Count - 1);
                        }

                        GroupModify(obj.Value);
                    }

                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in User.Current.DeviceGroups)
                    {
                        var group = obj.Value;
                        if (!group.Items.Contains(subLayout)) continue;

                        group.Items.Remove(subLayout);
                        group.View.Remove(subLayout);

                        while (group.View.Count > 0 && group.View[group.View.Count - 1] == null)
                        {
                            group.View.RemoveAt(group.View.Count - 1);
                        }

                        GroupModify(obj.Value);
                    }
                    break;
            }

            if (OnSubLayoutModify != null)
                OnSubLayoutModify(this, new EventArgs<ISubLayout>(subLayout));
        }

        private void ServerOnStorageStatusUpdate(Object sender, EventArgs e)
        {
            StorageStatusUpdate();
        }

        private Boolean _isWarningRecordFailed;
        private Boolean _isWarningRAIDDowngrade;
        public void StorageStatusUpdate()
        {
            //<Event>
            //    <Type>RecordFailed</Type>
            //    <LocalTime>1310457814548</LocalTime>
            //</Event>

            var usedStorageCount = Server.Storage.Count;
            if (usedStorageCount != 0)
            {
                foreach (Storage storage in Server.Storage)
                {
                    if (!Server.StorageInfo.ContainsKey(storage.Key))
                        usedStorageCount--;
                }
            }

            //have device, should check if it's recording
            if (Device.Devices.Count > 0)
            {
                if (usedStorageCount < 1)
                {
                    if (OnEventReceive != null)
                    {
                        var cameraEvent = new CameraEvents
                        {
                            NVR = this,
                            Type = EventType.RecordFailed,
                            DateTime = Server.DateTime,
                            Timecode = DateTimes.ToUtc(Server.DateTime, Server.TimeZone)
                        };

                        if (Configure.StorageAlert)
                            Utility.PlaySystemSound(3, 1000, 100);

                        RaiseOnEventReceive(cameraEvent);
                        _isWarningRecordFailed = true;
                    }
                }
                else
                {
                    //warning before, if record is backm should let use know
                    if (_isWarningRecordFailed)
                    {
                        var cameraEvent = new CameraEvents
                        {
                            NVR = this,
                            Type = EventType.RecordRecovery,
                            DateTime = Server.DateTime,
                            Timecode = DateTimes.ToUtc(Server.DateTime, Server.TimeZone)
                        };
                        RaiseOnEventReceive(cameraEvent);
                        _isWarningRecordFailed = false;
                    }
                }
            }

            if (Server.Platform == Platform.Linux)
            {
                if (Server.RAID.Status == RAIDStatus.Active || Server.RAID.Status == RAIDStatus.Recovery || Server.RAID.Status == RAIDStatus.Resync)
                {
                    //warning before, if record is backm should let use know
                    if (_isWarningRAIDDowngrade)
                    {
                        var cameraEvent = new CameraEvents
                        {
                            NVR = this,
                            Type = EventType.RecordRecovery,
                            DateTime = Server.DateTime,
                            Timecode = DateTimes.ToUtc(Server.DateTime, Server.TimeZone)
                        };
                        RaiseOnEventReceive(cameraEvent);
                        _isWarningRAIDDowngrade = false;
                    }
                    return;
                }

                if (OnEventReceive != null)
                {
                    var type = EventType.RecordFailed;

                    if (Server.RAID.Status == RAIDStatus.Degrade)
                        type = EventType.RAIDDegraded;
                    if (Server.RAID.Status == RAIDStatus.Inactive)
                        type = EventType.RAIDInactive;

                    var cameraEvent = new CameraEvents
                    {
                        NVR = this,
                        Type = type,
                        DateTime = Server.DateTime,
                        Timecode = DateTimes.ToUtc(Server.DateTime, Server.TimeZone)
                    };

                    if (Configure.StorageAlert)
                        Utility.PlaySystemSound(3, 1000, 100);

                    RaiseOnEventReceive(cameraEvent);
                    _isWarningRAIDDowngrade = true;
                }
            }
        }

        public void UtilityOnServerEventReceive(String msg)
        {
            var xmlDoc = Xml.LoadXml(msg);
            var rootNode = xmlDoc.FirstChild;

            if (rootNode == null) return;

            if (rootNode.Name == "CPUUsage")
            {
                Server.CPUUsage = Convert.ToInt32(rootNode.InnerText);
                return;
            }

            if (rootNode.Name == "DeviceStatus" && OnCameraStatusReceive != null)
            {
                ParseDeviceStatus(xmlDoc);
                return;
            }

            if (rootNode.Name == "AudioOut" && OnCameraStatusReceive != null)
            {
                ParseAudioOut(xmlDoc);
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
            //Console.WriteLine(msg); //check event from control
            ParseEvent(xmlDoc, rootNode);
        }

        private void ParseDeviceStatus(XmlDocument xmlDoc)
        {
            _statusTimeOutTimer.Enabled = false;
            _statusTimeOutTimer.Enabled = true;

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

                var device = Device.FindDeviceById(id);
                if (device == null) continue;
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

            if (OnCameraStatusReceive != null)
                OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private void ParseAudioOut(XmlDocument xmlDoc)
        {
            var devices = xmlDoc.GetElementsByTagName("Device");

            if (devices.Count <= 0) return;

            var cameras = new List<ICamera>();
            foreach (XmlElement deviceNode in devices)
            {
                String id = deviceNode.GetAttribute("id");

                if (id != "")
                {
                    IDevice device = Device.FindDeviceById(Convert.ToUInt16(id.Replace("channel", "")));
                    if (device == null) continue;
                    if (device.ReadyState == ReadyState.New) continue;

                    if (!(device is ICamera)) continue;
                    var camera = device as ICamera;

                    if (camera.IsAudioOut == String.Equals(deviceNode.InnerText, "1")) continue;

                    camera.IsAudioOut = String.Equals(deviceNode.InnerText, "1");
                    cameras.Add(camera);
                }
            }

            OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private void ParseEvent(XmlDocument xmlDoc, XmlNode rootNode)
        {
            var cameraEvents = new List<ICameraEvent>();

            var id = Xml.GetFirstElementValueByTagName(rootNode, "DeviceID");
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
                case "CrossLine":
                case "Panic":
                case "IntrusionDetection":
                case "LoiteringDetection":
                case "ObjectCountingIn":
                case "ObjectCountingOut":
                case "AudioDetection":
                case "TamperDetection":
                    eventType = (EventType)Enum.Parse(typeof(EventType), type, true);
                    break;
            }

            if (id != "")
            {
                var device = Device.FindDeviceById(Convert.ToUInt16(id));
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

                    cameraEvent.NVR = this;
                    cameraEvent.Status = status.InnerText;
                    cameraEvent.Value = (status.GetAttribute("value") == "1");
                    cameraEvent.DateTime = DateTimes.ToDateTime(Convert.ToUInt64(Xml.GetFirstElementValueByTagName(rootNode, "LocalTime")), Server.TimeZone);

                    cameraEvents.Add(cameraEvent);
                }
            }
            else//NVR Event (no device relative)
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

            RaiseOnEventReceive(cameraEvents);
        }

        public void UtilityOnUploadProgress(Int32 progress)
        {
            ApplicationForms.ProgressBarValue = progress;

            //wait 2 sec to trigger completed event
            if (progress >= 100)
            {
                _devicePackUploadCompletedTimer.Enabled = false;
                _devicePackUploadCompletedTimer.Enabled = true;
            }
        }


        private void UploadCompleted(Object sender, EventArgs e)
        {
            _devicePackUploadCompletedTimer.Enabled = false;

            if (OnDevicePackUpdateCompleted != null)
                OnDevicePackUpdateCompleted(this, null);
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

        private delegate void GetAllChannelStatusDelegate();
        private void GetAllChannelStatusTimer(Object sender, EventArgs e)
        {
            try
            {
                if (Device.Devices.Count == 0) return;
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

        private delegate void ShowStorageUsageDelegate();
        private void ShowStorageUsage(Object sender, EventArgs e)
        {
            try
            {
                ShowStorageUsageDelegate showStorageUsageDelegate = Server.LoadStorageInfo;
                showStorageUsageDelegate.BeginInvoke(null, null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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

        private void StatusTimeOut(Object sender, EventArgs e)
        {
            _statusTimeOutTimer.Enabled = false;

            var cameras = new List<ICamera>();
            foreach (KeyValuePair<UInt16, IDevice> obj in Device.Devices)
            {
                if (!(obj.Value is ICamera)) continue;
                var camera = obj.Value as ICamera;

                camera.Status = CameraStatus.Nosignal;
                cameras.Add(camera);
            }

            if (OnCameraStatusReceive != null)
                OnCameraStatusReceive(this, new EventArgs<List<ICamera>>(cameras));
        }

        private const UInt16 Timeout = 10;//10 sec
        public Boolean ValidateCredential()
        {
            if (Credential.UserName == "" || Credential.Domain == "") return false;

            HttpWebResponse response = null;
            try
            {
                var request = Xml.GetHttpRequest(CgiLogin, Credential, Timeout);

                response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();

                        return true;
                    }
                    response.Close();
                }
            }
            catch (Exception)
            {
                if (response != null)
                    response.Close();
            }

            return false;
        }

        public Boolean ValidateCredentialWithMessage()
        {
            if (Credential.UserName == "" || Credential.Domain == "") return false;

            HttpWebResponse response;
            try
            {
                var request = Xml.GetHttpRequest(CgiLogin, Credential, Timeout);

                if (request == null)
                {
                    //URI Format Error
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailedConnectFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();

                        return true;
                    }
                    response.Close();
                }
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ConnectFailure)
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailedConnectFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpWebResponse = ((HttpWebResponse)exception.Response);
                    if (httpWebResponse != null)
                    {
                        switch (httpWebResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                TopMostMessageBox.Show(Localization["LoginForm_SignInFailedAuthFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case HttpStatusCode.NotFound:
                                TopMostMessageBox.Show(Localization["LoginForm_SignInFailedPortOccupation"].Replace("%1", Credential.Port.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                        httpWebResponse.Close();
                    }
                }
                else if (exception.Status == WebExceptionStatus.Timeout)
                    TopMostMessageBox.Show(Localization["LoginForm_SignInTimeout"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else //unknown reason
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailed"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return false;
        }

        private List<IDevice> _compareDevices = new List<IDevice>();
        private List<IDevice> _removeDevices = new List<IDevice>();
        private List<IDevice> _listDevices = new List<IDevice>();
        public List<IDevice> ReadDeviceList()
        {
            _listDevices.Clear();
            if (Credential.UserName == "" || Credential.Domain == "") return _listDevices;

            try
            {
                var xmlDoc = new XmlDocument();
                var nvrNode = xmlDoc.CreateElement("NVR");
                nvrNode.SetAttribute("name", Name);

                var driver = Driver;
                switch (Driver)
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

                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Driver", driver));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Domain", Credential.Domain));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", Credential.Port.ToString()));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ServerPort", ServerPort.ToString()));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", Encryptions.EncryptDES(Credential.UserName)));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", Encryptions.EncryptDES(Credential.Password)));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SSLEnable", Credential.SSLEnable ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsListenEvent", IsListenEvent ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsPatrolInclude", IsPatrolInclude ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Modified", ModifiedDate));
                xmlDoc.AppendChild(nvrNode);

                XmlDocument resultXmlDoc;
                resultXmlDoc = Xml.PostXmlToHttp(CgiSearchWithoutSavingNVR, xmlDoc, ServerManager.Credential, 60);

                if (resultXmlDoc != null)
                {
                    XmlNodeList devicesList = resultXmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");
                    foreach (XmlNode node in devicesList)
                    {
                        IDevice device = ParseDeviceProfileFromXml((XmlElement)node);
                        _listDevices.Add(device);
                    }
                }

                return _listDevices;
            }
            catch (Exception)
            {
                return _listDevices;
            }
        }

        private Dictionary<UInt16, IDevice> ReadFailoverDeviceWithoutSaving()
        {
            TempDevices.Clear();

            var cms = ServerManager as ICMS;
            if (cms == null) return TempDevices;

            _compareDevices.Clear();
            _removeDevices.Clear();

            for (UInt16 id = 1; id <= 64; id++)
            {
                var device = new Camera
                {
                    Server = this,
                    Id = id,
                    Name = "Failover Device " + id,
                    ReadyState = ReadyState.NotInUse,
                    CMS = (ICMS)ServerManager
                };

                CameraModel cameraModel = new CameraModel
                {
                    Manufacture = String.Empty,
                    Model = String.Empty
                };

                device.Model = cameraModel;
                device.Profile = new CameraProfile
                {
                    NetworkAddress = String.Empty,
                    HighProfile = 1,
                    MediumProfile = 1,
                    LowProfile = 1,
                };

                device.Profile.StreamConfigs.Add(1, new StreamConfig
                {
                    Compression = Compression.Off,
                    Resolution = Resolution.NA,
                    VideoQuality = 60,
                    Framerate = 1,
                    Bitrate = Bitrate.NA,
                });

                _compareDevices.Add(device);

                if (!TempDevices.ContainsKey(id))
                {
                    TempDevices.Add(id, device);

                    foreach (KeyValuePair<IDevice, UInt16> obj in cms.NVRManager.DeviceChannelTable)
                    {
                        if (obj.Key.Server.Id == device.Server.Id && obj.Key.Id == device.Id)
                        {
                            if (!cms.NVRManager.NVRs.ContainsKey(obj.Key.Server.Id)) continue;
                            var nvr = cms.NVRManager.NVRs[obj.Key.Server.Id];
                            if (!nvr.Device.Devices.ContainsKey(obj.Key.Id)) continue;

                            var value = obj.Value;
                            //selected device status should be modify by sync
                            device.ReadyState = ReadyState.Modify;
                            ((ICamera)device).EventSchedule = ((ICamera)obj.Key).EventSchedule;
                            ((ICamera)device).EventHandling = ((ICamera)obj.Key).EventHandling;
                            nvr.Device.Devices.Remove(obj.Key.Id);
                            nvr.Device.Devices.Add(obj.Key.Id, device);

                            cms.NVRManager.DeviceChannelTable.Remove(obj.Key);
                            cms.NVRManager.DeviceChannelTable.Add(device, value);
                            break;
                        }
                        ((ICamera)device).EventSchedule = new Schedule();
                        ((ICamera)device).EventHandling = new EventHandling();
                        ((ICamera)device).EventHandling.SetDefaultEventHandling(((ICamera)device).Model);
                        ((ICamera)device).EventSchedule.Description = ScheduleMode.FullTimeEventHandling;
                        ((ICamera)device).EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
                    }
                }
            }

            return TempDevices;
        }

        public Dictionary<UInt16, IDevice> ReadNVRDeviceWithoutSaving()
        {
            if (Manufacture == "iSAP Failover Server")
                return ReadFailoverDeviceWithoutSaving();

            TempDevices.Clear();
            if (Credential.UserName == "" || Credential.Domain == "") return TempDevices;

            try
            {
                var xmlDoc = new XmlDocument();
                var nvrNode = xmlDoc.CreateElement("NVR");
                nvrNode.SetAttribute("name", Name);

                var driver = Driver;
                switch (Driver)
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

                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Driver", driver));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Domain", Credential.Domain));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", Credential.Port.ToString()));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ServerPort", ServerPort.ToString()));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", Encryptions.EncryptDES(Credential.UserName)));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", Encryptions.EncryptDES(Credential.Password)));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SSLEnable", Credential.SSLEnable ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsListenEvent", IsListenEvent ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsPatrolInclude", IsPatrolInclude ? "true" : "false"));
                nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Modified", ModifiedDate));
                xmlDoc.AppendChild(nvrNode);

                XmlDocument resultXmlDoc;
                resultXmlDoc = Xml.PostXmlToHttp(CgiSearchWithoutSavingNVR, xmlDoc, ServerManager.Credential, 60);

                if (resultXmlDoc == null)
                {
                    ReadyState = ReadyState.Unavailable;
                    //URI Format Error
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailedConnectFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return TempDevices;
                }
                var cms = ServerManager as ICMS;
                if (cms == null) return TempDevices;

                _compareDevices.Clear();
                _removeDevices.Clear();

                XmlNodeList devicesList = resultXmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");
                foreach (XmlNode node in devicesList)
                {
                    UInt16 id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

                    IDevice device = ParseDeviceProfileFromXml((XmlElement)node);
                    ((ICamera)device).XmlFromServer = (XmlElement)node;

                    _compareDevices.Add(device);

                    if (device != null && !TempDevices.ContainsKey(id))
                    {
                        TempDevices.Add(id, device);

                        foreach (KeyValuePair<IDevice, UInt16> obj in cms.NVRManager.DeviceChannelTable)
                        {
                            if (obj.Key.Server.Id == device.Server.Id && obj.Key.Id == device.Id)
                            {
                                if (!cms.NVRManager.NVRs.ContainsKey(obj.Key.Server.Id)) continue;
                                var nvr = cms.NVRManager.NVRs[obj.Key.Server.Id];
                                if (!nvr.Device.Devices.ContainsKey(obj.Key.Id)) continue;

                                var value = obj.Value;
                                //selected device status should be modify by sync
                                device.ReadyState = ReadyState.Modify;
                                ((ICamera)device).EventSchedule = ((ICamera)obj.Key).EventSchedule;
                                ((ICamera)device).EventHandling = ((ICamera)obj.Key).EventHandling;
                                nvr.Device.Devices.Remove(obj.Key.Id);
                                nvr.Device.Devices.Add(obj.Key.Id, device);

                                cms.NVRManager.DeviceChannelTable.Remove(obj.Key);
                                cms.NVRManager.DeviceChannelTable.Add(device, value);
                                break;
                            }
                        }
                        ((ICamera)device).EventSchedule = new Schedule();
                        ((ICamera)device).EventHandling = new EventHandling();
                        ((ICamera)device).EventHandling.SetDefaultEventHandling(((ICamera)device).Model);
                        ((ICamera)device).EventSchedule.Description = ScheduleMode.FullTimeEventHandling;
                        ((ICamera)device).EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
                    }
                }

                foreach (KeyValuePair<IDevice, UInt16> obj in cms.NVRManager.DeviceChannelTable)
                {
                    if (obj.Key.Server != this) continue;

                    if (!_compareDevices.Contains(obj.Key))
                    {
                        _removeDevices.Add(obj.Key);
                    }
                }

                foreach (IDevice removeDevice in _removeDevices)
                {
                    Device.Devices.Remove(removeDevice.Id);
                    cms.NVRManager.DeviceChannelTable.Remove(removeDevice);
                }

                _compareDevices.Clear();
                _removeDevices.Clear();
                return TempDevices;
            }
            catch (WebException exception)
            {
                ReadyState = ReadyState.Unavailable;
                if (exception.Status == WebExceptionStatus.ConnectFailure)
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailedConnectFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpWebResponse = ((HttpWebResponse)exception.Response);
                    if (httpWebResponse != null)
                    {
                        switch (httpWebResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                TopMostMessageBox.Show(Localization["LoginForm_SignInFailedAuthFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case HttpStatusCode.NotFound:
                                TopMostMessageBox.Show(Localization["LoginForm_SignInFailedPortOccupation"].Replace("%1", Credential.Port.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }
                }
                else if (exception.Status == WebExceptionStatus.Timeout)
                    TopMostMessageBox.Show(Localization["LoginForm_SignInTimeout"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else //unknown reason
                    TopMostMessageBox.Show(Localization["LoginForm_SignInFailed"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return TempDevices;
            }
        }

        protected virtual IDevice ParseDeviceProfileFromXml(XmlElement node)
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

                var streamConfigs = settingNode.SelectNodes("StreamConfig");
                //if (streamConfigs == null) return null;

                //foreach (XmlNode streamConfig in streamConfigs)
                //    settingNode.RemoveChild(streamConfig);


                var camera = new Camera
                {
                    Server = this,
                    Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID")),
                    Name = name,
                    ReadyState = ReadyState.New,
                    CMS = (ICMS)ServerManager
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
                    camera.Model.NumberOfAudioOut = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioOut")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioOut")));
                    camera.Model.NumberOfAudioIn = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioIn")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfAudioIn")));
                    camera.Model.NumberOfMotion = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfMotion")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfMotion")));
                    camera.Model.NumberOfChannel = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfChannel")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfChannel")));
                    camera.Model.NumberOfDi = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDi")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDi")));
                    camera.Model.NumberOfDo = (ushort)(String.IsNullOrEmpty(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDo")) ? 0 : Convert.ToUInt16(Xml.GetFirstElementValueByTagName(capabilityNode, "NumberOfDo")));
                }

                camera.ReadyState = ReadyState.NotInUse;
                return camera;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(@"Parse Device XML Error " + exception);
            }

            return null;
        }

        public void DeletePresetPointRelativeEventHandle(ICamera device, UInt16 pointId)
        {
            foreach (var obj in Device.Devices)
            {
                if (!(obj.Value is ICamera)) continue;

                var camera = ((ICamera)obj.Value);
                foreach (var list in camera.EventHandling)
                {
                    var removeList = new List<EventHandle>();
                    foreach (var eventHandle in list.Value)
                    {
                        if (eventHandle is GotoPresetEventHandle)
                        {
                            if (((GotoPresetEventHandle)eventHandle).Device == device && (pointId == 0 || ((GotoPresetEventHandle)eventHandle).PresetPoint == pointId))
                                removeList.Add(eventHandle);
                        }
                    }

                    if (removeList.Count > 0)
                    {
                        DeviceModify(camera);
                        camera.EventHandling.ReadyState = ReadyState.Modify;
                        foreach (var eventHandle in removeList)
                        {
                            list.Value.Remove(eventHandle);
                        }
                    }
                }
            }
        }

        public override String ToString()
        {
            return (DisplayNVRId) ? (Id.ToString().PadLeft(2, '0') + " " + Name) : Name;
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
    }
}
