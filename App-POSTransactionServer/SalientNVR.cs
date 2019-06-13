using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Constant;
using CVServerControlLib;
using Device;
using DeviceConstant;
using Interface;
using ServerProfile;
using System.Xml;
namespace App_POSTransactionServer
{
	public class SalientNVR : INVR
	{
		public event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;
		public event EventHandler<EventArgs<List<ICamera>>> OnCameraStatusReceive;

		public event EventHandler<EventArgs<IDevice>> OnDeviceModify;
		public event EventHandler<EventArgs<IDeviceGroup>> OnGroupModify;
		public event EventHandler<EventArgs<IDeviceLayout>> OnDeviceLayoutModify;
		public event EventHandler<EventArgs<ISubLayout>> OnSubLayoutModify;

		public event EventHandler<EventArgs<String>> OnLoadComplete;
		public event EventHandler<EventArgs<String>> OnLoadFailure;
		public event EventHandler<EventArgs<String>> OnSaveComplete;
		public event EventHandler<EventArgs<String>> OnSaveFailure;

		public event EventHandler OnDevicePackUpdateCompleted;

		private readonly CCVServerControlClass _serverControl;

		public IUtility Utility { get; private set; }

        public NVRStatus NVRStatus { get; set; }

	    public UInt16 Id { get; set; }
		public String Name { get; set; }
		public String Manufacture { get; set; }

	    public string Driver{ get; set; }
	    
	    public Form Form { get; set; }
		public Boolean IsListenEvent { get; set; }
		public Boolean IsPatrolInclude { get; set; }

		public FailoverSetting FailoverSetting { get; set; }

		public UInt64 ModifiedDate { get; set; } //last modified date (save)

		public ServerCredential Credential { get; set; }

	    public ushort ServerPort { get; set; }
        public UInt16 ServerStatusCheckInterval { get; set; }
        public List<IDevice> FailoverDeviceList { get; set; }
	    public Dictionary<ushort, IDevice> ReadNVRDeviceWithoutSaving()
	    {
	        return null;
	    }
        public List<IDevice> ReadDeviceList()
        {
            return null;
        }
	    public ILicenseManager License { get; protected set; }
		public IServerManager Server { get; protected set; }
		public IConfigureManager Configure { get; private set; }
		public IUserManager User { get; protected set; }
		public IDeviceManager Device { get; protected set; }
		public IIOModelManager IOModel { get; set; }
		public ReadyState ReadyState { get; set; }
		public string LoginProgress { get; set; }

		public Dictionary<String, String> Localization;

		public SalientNVR()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   //{"Application_UtilityInitError", "Utility Initialize Error"},
								   {"Data_AllDevices", "All Devices"},

									{"LoginForm_SignInFailed", "Sign in failed"},
									{"LoginForm_SignInFailedConnectFailure", "Can not connect to server. Please confirm host and port is correct."},
									{"LoginForm_SignInTimeout", "Login timeout. Please check firewall setting."},
									{"LoginForm_SignInFailedAuthFailure", "Login failure. Please confirm account and password is correct."},
									{"LoginForm_SignInFailedPortOccupation", "Login failure. Please verify if port %1 is already used by another application."},
							   };
			Localizations.Update(Localization);

			Name = "Salient NVR";
			Manufacture = "Salient";
			ReadyState = ReadyState.New;
			IsListenEvent = false;
			IsPatrolInclude = true;

			Credential = new ServerCredential
			{
				Domain = "",
				Port = 80,
				UserName = "admin",
				Password = "",
			};

			_serverControl = new CCVServerControlClass();
			_serverControl.OnConnectionLost += ServerControlOnConnectionLost;
		}

		public virtual String Status
		{
			get
			{
				return String.Join(Environment.NewLine, new[]
				{
					Device.Status, 
				});
			}
		}

		public virtual void Initialize()
		{
            try
            {
                if (Utility == null)
                {
                    Utility = new UtilitySalient.UtilitySalient
                    {
                        Server = this,
                    };
                }
                else
                    Utility.Server = this;
            }
            catch (Exception)
            {
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

		public virtual void Login()
		{
			LoadDevice();
		}

		private void LoadDevice()
		{
			Device.ReadyStatus = ManagerReadyState.Loading;
			Device.Devices.Clear();
			Device.Groups.Clear();
			Device.Manufacture.Clear();


            Boolean connect = false;
            _serverControl.IsServerConnected(out connect);
            
              Int16 deviceCount = 0;
			_serverControl.GetNumberOfCameras(out deviceCount);

            IDeviceGroup defaultGroup = new DeviceGroup
			{
				Id = 0,
				Server = this,
				ReadyState = ReadyState.Ready,
				Name = Localization["Data_AllDevices"],
			};

			Device.Groups.Add(defaultGroup.Id, defaultGroup);

			for (short index = 1; index <= deviceCount; index++)
			{
				String cameraName = "";
				UInt32 cameraStatus;
				_serverControl.GetCameraInfo(index, out cameraName, out cameraStatus);
				var device = new Camera
								 {
									 Id = Convert.ToUInt16(index),
									 Name = cameraName,
									 ReadyState = ReadyState.Ready,
									 Server = this,
									 Profile = new CameraProfile(),
									 Model = new CameraModel()
								 };

				var bitArray = new BitArray(BitConverter.GetBytes(cameraStatus));
				//Bit Position              Meaning when Set
				//0 (least significant) Scheduled recording is active.
				//1                             External alarm detected, external alarm recording is active.
				//2                             Motion recording is active.
				//3                             Motion has been detected.
				//4                             Camera is a PTZ camera.
				//5                             Prealarm recording is active.
				//6                             Not connected to server (video acquisition failure).
				//7                             Tour is enabled.
				//8                             Camera is inactive/stopped (no attempt by server to acquire video), though it may be enabled.
				//9                             Camera supports audio.
				//10                           Camera is an IP camera.
				//11                           Camera is an ACTI model.
				//13                           Camera is transitioning to or from the stopped state.  Bit 8 is the intended next state (0 = started, 1 = stopped) but the camera is not there yet.

				device.Status = CameraStatus.Streaming;

				if (bitArray[0] || bitArray[1] || bitArray[2])
					device.Status = CameraStatus.Recording;

				//if (bitArray[4])
				//    device.Model.PanSupport = device.Model.TiltSupport = device.Model.ZoomSupport = true;

				if (bitArray[6] || bitArray[8])
					device.Status = CameraStatus.Nosignal;

				if (bitArray[9])
					device.Model.NumberOfAudioIn = 1;

				Device.Devices.Add(device.Id, device);
				defaultGroup.Items.Add(device);
				defaultGroup.View.Add(device);
			}
			
			Device.ReadyStatus = ManagerReadyState.Ready;

			ReadyState = ReadyState.Ready;

			AddDevicePermission();

			if (OnLoadComplete != null)
				OnLoadComplete(this, new EventArgs<String>(Status));
		}

		public virtual void Save()
		{
		}

		public void Logout()
		{
			if (_serverControl != null)
				_serverControl.Disconnect();
		}

        public Dictionary<ushort, IDevice> TempDevices { get; set; }
	    public void AddDevicePermission()
		{
            User.Current.Group = new ServerProfile.UserGroup
            {
                Id = 0,
                Name = "Administrator",
                ReadyState = ReadyState.Ready,
            };
            User.Users.Clear();
            User.Users.Add(0, User.Current);
            foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
            {
                IUser user = obj.Value;
                //Full permission of all devices

                foreach (KeyValuePair<UInt16, IDevice> device in Device.Devices)
                {
                    user.AddFullDevicePermission(device.Value);
                    if (user.Permissions.ContainsKey(device.Value))
                    {
                        var permissions = user.Permissions[device.Value];

                        permissions.Remove(Permission.AudioOut);
                        permissions.Remove(Permission.ManualRecord);
                        permissions.Remove(Permission.OpticalPTZ);
                    }
                }
                continue;
            }
		}

		public void StopTimer()
		{
		}

		public void SilentLoad()
		{
			LoadDevice();
		}

		public void DeviceModify(IDevice device)
		{
		}

		public void GroupModify(IDeviceGroup group)
		{
		}

		public void DeviceLayoutModify(IDeviceLayout deviceLayout)
		{
		}

		public void SubLayoutModify(ISubLayout subLayout)
		{
		}

		public void StorageStatusUpdate()
		{
		}

		public void UtilityOnServerEventReceive(String msg)
		{
		}

		public void UtilityOnUploadProgress(Int32 progress)
		{
		}

		public Boolean CheckCameraEvent(ICameraEvent cameraEvent)
		{
			return false;
		}

		public Boolean ValidateCredential()
		{
			if (Credential.UserName == "" || Credential.Domain == "") return false;

			_serverControl.UserName = Credential.UserName;
			_serverControl.Password = Credential.Password;
			_serverControl.Server = Credential.Domain;
			_serverStatus = ServerStatus.Connecting;

			_serverControl.Connect();

			Boolean isConnected = false;
			
			//var retry = 10;
			//_serverControl.IsServerConnected(out isConnected);
			//while (!isConnected)
			//{
			//    retry--;
			//    if (retry > 0)
			//    {
			//        Thread.Sleep(1000);//wait 1 sec
			//        _serverControl.IsServerConnected(out isConnected);
			//        if (isConnected)
			//            break;
			//    }
			//    else
			//        break;
			//}

			var watch = new Stopwatch();
			var count = 1;
			watch.Start();
			while (_serverStatus == ServerStatus.Connecting)
			{
				//count++;
				_serverControl.IsServerConnected(out isConnected);

				if (!isConnected)
					Application.DoEvents();
				else
					_serverStatus = ServerStatus.Online;
			}

			Console.WriteLine("count: " + count + " time: " + watch.Elapsed + " status: " + _serverStatus);

			if (isConnected)
			{
				ReadyState = ReadyState.Ready;
				return true;
			}

			_serverControl.Disconnect();
			ReadyState = ReadyState.Unavailable;

			return false;
		}

		private void ServerControlOnConnectionLost(string serverIP, DateTime eventTime)
		{
			_serverStatus = ServerStatus.Offline;
		}

		public Boolean ValidateCredentialWithMessage()
		{
			if (Credential.UserName == "" || Credential.Domain == "") return false;

			Boolean connected = false;
			//if you ckeck is connected before call connect, it will crash!
			if (!String.IsNullOrEmpty(_serverControl.Server) && !String.IsNullOrEmpty(_serverControl.UserName))
				_serverControl.IsServerConnected(out connected);

			if (connected)
				_serverControl.Disconnect();
			
			_serverControl.UserName = Credential.UserName;
			_serverControl.Password = Credential.Password;
			_serverControl.Server = Credential.Domain;

			_serverControl.Connect();

			var retry = 10;
			Boolean isConnected = false;
			_serverControl.IsServerConnected(out isConnected);
			while (!isConnected)
			{
				retry--;
				if (retry > 0)
				{
					Thread.Sleep(1000);//wait 1 sec
					_serverControl.IsServerConnected(out isConnected);
					if (isConnected)
						break;
				}
				else
					break;
			}

			if (isConnected)
				return true;

			Device.Devices.Clear();
			foreach (var deviceGroup in Device.Groups)
			{
				deviceGroup.Value.Items.Clear();
			}

			MessageBox.Show(Localization["LoginForm_SignInFailedAuthFailure"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

			return false;
		}

		public void DeletePresetPointRelativeEventHandle(ICamera device, UInt16 pointId)
		{
		}

		public void WriteOperationLog(String msg)
		{
		}

		public override String ToString()
		{
			return Id.ToString().PadLeft(2, '0') + " " + Name;
		}
		
		public void UndoReload()
		{
		}

		private ServerStatus _serverStatus = ServerStatus.Offline;
		private enum ServerStatus
		{
			Connecting,
			Online,
			Offline,
		}


    }
}
