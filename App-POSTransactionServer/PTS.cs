using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using ServerProfile;
using ApplicationForms = App.ApplicationForms;

namespace App_POSTransactionServer
{
	public  class PTS : IPTS
	{
		private const String CgiLogout = @"cgi-bin/login?action=logout";
		public event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;
		public event EventHandler<EventArgs<POS_Exception.TransactionItem>> OnPOSEventReceive;
		public event EventHandler<EventArgs<List<ICamera>>> OnCameraStatusReceive;
		public event EventHandler<EventArgs<INVR>> OnNVRModify;
		public event EventHandler<EventArgs<IPOS>> OnPOSModify;

		public event EventHandler<EventArgs<String>> OnLoadComplete;
		public event EventHandler<EventArgs<String>> OnLoadFailure;
		public event EventHandler<EventArgs<String>> OnSaveComplete;
		public event EventHandler<EventArgs<String>> OnSaveFailure;

		public IUtility Utility { get; private set; }

	    public NVRStatus NVRStatus { get; set; }

	    public UInt16 Id { get; set; }
		public String Name { get; set; }
		public String Manufacture { get; set; }

	    public string Driver{ get; set; }

	    public Form Form { get; set; }
		public Boolean IsPatrolInclude { get; set; }

		public UInt64 ModifiedDate { get; set; }

		public ServerCredential Credential { get; set; }

	    public ushort ServerPort { get; set; }
        public UInt16 ServerStatusCheckInterval { get; set; }
        public List<IDevice> FailoverDeviceList { get; set; }
	    public Dictionary<UInt16, IDevice> ReadNVRDeviceWithoutSaving()
        {
            return null;
        }
        public List<IDevice> ReadDeviceList()
        {
            return null;
        }
	    public ILicenseManager License { get; private set; }
		public IServerManager Server { get; private set; }
		public IConfigureManager Configure { get; private set; }
		public IUserManager User { get; private set; }
		public IDeviceManager Device { get; private set; }
		public INVRManager NVR { get; private set; }
		public IPOSManager POS { get; private set; }
		public IIOModelManager IOModel { get; set; }

		public ReadyState ReadyState { get; set; }
		public string LoginProgress { get; set; }

		private readonly System.Timers.Timer _loginTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _savingTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _serverTimeTimer = new System.Timers.Timer();
		public Dictionary<String, String> Localization;

		public String ReleaseBrand { get; private set; }

		public PTS()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},
								   {"Application_UtilityInitError", "Utility Initialize Error"},
								   
								   {"App_Loading", "Loading"},
								   {"Loading_POS", "POS"},
								   {"Loading_Config", "Config"},
								   {"Loading_Device", "Device"},
								   {"Loading_License", "License"},
								   {"Loading_NVR", "NVR"},
								   {"Loading_Server", "Server"},
								   {"Loading_User", "User"},
							   };
			Localizations.Update(Localization);

			var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

			switch (name)
			{
				case "TransactionTracker":
					ReleaseBrand = "Salient";
					Name = "Salient TransactionTracker";
					break;

				default:
					ReleaseBrand = "iSap";
					Name = "POS Transaction Server";
					break;
			}

			ReadyState = ReadyState.New;
			IsPatrolInclude = false;

			LoginProgress = Localization["App_Loading"];
		}

		public void ListenNVREvent(INVR nvr = null)
		{
			if (nvr != null)
			{
				nvr.OnEventReceive -= NVROnEventReceive;
				nvr.OnCameraStatusReceive -= NVROnCameraStatusReceive;

				nvr.OnEventReceive += NVROnEventReceive;
				nvr.OnCameraStatusReceive += NVROnCameraStatusReceive;

				//check recording status (alert when not recording)
				nvr.Server.LoadStorageInfo();

				nvr.Utility.StartEventReceive();
				return;
			}

			foreach (KeyValuePair<UInt16, INVR> obj in NVR.NVRs)
			{
				nvr = obj.Value;
				if (obj.Value.ReadyState != ReadyState.Ready) continue;

				nvr.OnEventReceive -= NVROnEventReceive;
				nvr.OnCameraStatusReceive -= NVROnCameraStatusReceive;
					
				nvr.OnEventReceive += NVROnEventReceive;
				nvr.OnCameraStatusReceive += NVROnCameraStatusReceive;

				//check recording status (alert when not recording)
				nvr.Server.LoadStorageInfo();

				nvr.Utility.StartEventReceive();
			}
		}

		public void ListenPOSEvent()
		{
			POS.OnPOSLiveEventReceive -= POSOnEventReceive;
			POS.OnPOSLiveEventReceive += POSOnEventReceive;
			POS.StartListenPOSLiveEvent();
		}

		public String Status
		{
			get
			{
				return String.Join(Environment.NewLine, new[]
				{
					License.Status,
					Configure.Status,
					User.Status,
					NVR.Status, 
					POS.Status, 
					Server.Status, 
				});
			}
		}

		public void Initialize()
		{
			try
			{
                switch (ReleaseBrand)
                {
                    case "Salient":
                        Utility = new UtilitySalient.UtilitySalient
                        {
                            Server = this,
                        };
                        break;

                    default:
                        Utility = new UtilityAir.UtilityAir
                        {
                            Server = this,
                        };
                        break;
                }
			}
			catch (Exception exception)
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

			Configure = new ConfigureManager
			{
				Server = this,
			};
			Configure.Initialize();

			User = new UserManager
			{
				Server = this,
			};
			User.Initialize();

			Device = new DeviceManager
			{
				Server = this,
			};
			Device.Initialize();

			NVR = new NVRManager
			{
				Server = this,
			};
			NVR.Initialize();

			POS = new POSManager
			{
				Server = this,
			};
			POS.Initialize();
		}

		private const UInt16 LoginTimeout = 60000;
		public void Login()
		{
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
			NVR.OnLoadComplete -= LoadNVRCallback;
			NVR.OnLoadComplete += LoadNVRCallback;

			LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_NVR"];
			NVR.Load();
		}

		private void LoadNVRCallback(Object sender, EventArgs e)
		{
			NVR.OnLoadComplete -= LoadNVRCallback;

			LoadPOS();
		}

		private void LoadPOS()
		{
			POS.OnLoadComplete -= LoadPOSCallback;
			POS.OnLoadComplete += LoadPOSCallback;

			LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_POS"];
			POS.Load();
		}

		private void LoadPOSCallback(Object sender, EventArgs e)
		{
			POS.OnLoadComplete -= LoadPOSCallback;

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

			AddNVRPermission();

			//Configure.CustomPlaybackSetting.Enable = Configure.CustomLiveSetting.Enable = false;

			if (OnLoadComplete != null)
				OnLoadComplete(this, new EventArgs<String>(Status));
		}

		private const UInt16 SavingTimeout = 60000;
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
				SaveUser();
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
				SaveNVR();
		}

		private void SaveUserCallback(Object sender, EventArgs e)
		{
			User.OnSaveComplete -= SaveUserCallback;

			SaveNVR();
		}

		private void SaveNVR()
		{
			ApplicationForms.ProgressBarValue = 50;

			if (User.Current.Group.CheckPermission("Setup", Permission.NVR))
			{
				NVR.OnSaveComplete -= SaveNVRCallback;
				NVR.OnSaveComplete += SaveNVRCallback;
				NVR.Save();
			}
			else
				SavePOS();
		}

		private void SaveNVRCallback(Object sender, EventArgs e)
		{
			NVR.OnSaveComplete -= SaveNVRCallback;

			SavePOS();
		}

		private void SavePOS()
		{
			ApplicationForms.ProgressBarValue = 70;

			if (User.Current.Group.CheckPermission("Setup", Permission.POS))
			{
				POS.OnSaveComplete -= SavePOSCallback;
				POS.OnSaveComplete += SavePOSCallback;
				POS.Save();
			}
			else
				SaveServer();
		}

		private void SavePOSCallback(Object sender, EventArgs e)
		{
			POS.OnSaveComplete -= SavePOSCallback;

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
				PTSSaveComplete();
		}

		private void SaveServerCallback(Object sender, EventArgs e)
		{
			PTSSaveComplete();
		}

		private void PTSSaveComplete()
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

			POS.StopListenPOSLiveEvent();
			foreach (KeyValuePair<UInt16, INVR> obj in NVR.NVRs)
			{
				//if (obj.Value.ReadyState != ReadyState.Ready) continue;
				if (obj.Value.Utility != null)
				{
					obj.Value.Utility.StopEventReceive();
					obj.Value.Utility.StopAudioTransfer();
				}
			}
			//Utility.StopEventReceive();
			//Utility.StopAudioTransfer();
		}

		private void CheckLoginTimeout(Object sender, EventArgs e)
		{
			License.OnLoadComplete -= LoadLicenseCallback;
			Server.OnLoadComplete -= LoadServerCallback;
			NVR.OnLoadComplete -= LoadNVRCallback;
			POS.OnLoadComplete -= LoadPOSCallback;
			User.OnLoadComplete -= LoadUserCallback;
			Configure.OnLoadComplete -= LoadConfigureCallback;
			_loginTimer.Enabled = false;

			if (OnLoadFailure != null)
				OnLoadFailure(this, new EventArgs<String>(Status));
		}

		private void CheckSaveTimeout(Object sender, EventArgs e)
		{
			Configure.OnSaveComplete -= SaveConfigureCallback;
			User.OnSaveComplete -= SaveUserCallback;
			NVR.OnSaveComplete -= SaveNVRCallback;
			POS.OnSaveComplete -= SavePOSCallback;
			Server.OnSaveComplete -= SaveServerCallback;
			_savingTimer.Enabled = false;

			ApplicationForms.HideProgressBar();

			if(OnSaveFailure != null)
				OnSaveFailure(this, new EventArgs<String>(Status));
		}

		public INVR CreateNewNVR()
		{
			INVR nvr;
			switch (ReleaseBrand)
			{
				case "Salient":
					nvr = new SalientNVR();
					break;

				default:
					nvr = new NVR();
					break;
			}

			nvr.Id = NVR.GetNewNVRId();

			return nvr;
		}

		public void DeviceModify(IDevice device)
		{
		}

		public void GroupModify(IDeviceGroup group)
		{
		}

		public void NVRModify(INVR nvr)
		{
			if (nvr.ReadyState == ReadyState.Delete)
			{
				if (nvr.Utility != null)
				{
					nvr.Utility.StopEventReceive();
					nvr.Utility.StopAudioTransfer();
				}

				foreach (IPOS pos in POS.POSServer)
				{
					if (!pos.Items.Any(device => (device.Server == nvr))) continue;

					var list = pos.Items.FindAll(device => device.Server == nvr);
					foreach (var device in list)
					{
						if(!pos.Items.Contains(device)) continue;

						pos.Items.Remove(device);
						pos.View.Remove(device);

						if (pos.ReadyState != ReadyState.New)
							pos.ReadyState = ReadyState.Modify;
					}

					if (OnPOSModify != null)
						OnPOSModify(this, new EventArgs<IPOS>(pos));
				}
			}
			else
			{
				//check if pos's device is change or missing after nvr is modify
				foreach (IPOS pos in POS.POSServer)
				{
					if (!pos.Items.Any(device => (device.Server == nvr))) continue;

					var list = pos.Items.FindAll(device => device.Server == nvr);
                    pos.View.Clear();      
                    foreach (var device in list)
					{
						if (device.Server.Device.Devices.ContainsKey(device.Id))
						{
							var newDevice = device.Server.Device.Devices[device.Id];
							pos.Items[pos.Items.IndexOf(device)] = newDevice;
                            pos.View.Add(newDevice);
						}
						else
						{
							pos.Items.Remove(device);
							pos.View.Remove(device);
						}
					}

					if (OnPOSModify != null)
						OnPOSModify(this, new EventArgs<IPOS>(pos));
				}
			}
			
			if (OnNVRModify != null)
				OnNVRModify(this, new EventArgs<INVR>(nvr));
		}

		public void POSModify(IPOS pos)
		{
			if (pos.ReadyState == ReadyState.Delete)
			{
				foreach (var posConnection in POS.Connections)
				{
					if (!posConnection.Value.POS.Contains(pos)) continue;

					posConnection.Value.POS.Remove(pos);

					if (posConnection.Value.ReadyState == ReadyState.Ready)
						posConnection.Value.ReadyState = ReadyState.Modify;
				}
			}

			if (OnPOSModify != null)
				OnPOSModify(this, new EventArgs<IPOS>(pos));
		}

		public void AddNVRPermission()
		{
			foreach (KeyValuePair<UInt16, IUser> obj in User.Users)
			{
				IUser user = obj.Value;
				//Full permission of all devices
				foreach (KeyValuePair<UInt16, INVR> nvr in NVR.NVRs)
				{
					if (user.NVRPermissions.ContainsKey(nvr.Value)) continue;

					user.NVRPermissions.Add(nvr.Value, new List<Permission> { Permission.Access });
				}
			}
		}

		//private delegate void GetServerTimeTimerDelegate(Object sender, ElapsedEventArgs e);
		private delegate void GetServerTimeDelegate();
		private void GetServerTimeTimer(Object sender, EventArgs e)
		{
			try
			{
				//if (Form.InvokeRequired)
				//{
				//    Form.Invoke(new GetServerTimeTimerDelegate(GetServerTimeTimer), sender, e);
				//    return;
				//}

				GetServerTimeDelegate getServerTimeDelegate = Server.LoadServerTime;
				getServerTimeDelegate.BeginInvoke(null, null);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		public void AddDevicePermission()
		{
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

		private void NVROnEventReceive(Object sender, EventArgs<List<ICameraEvent>> eventList)
		{
			if (OnEventReceive != null)
				OnEventReceive(this, eventList);
		}

		private void POSOnEventReceive(Object sender, EventArgs<POS_Exception.TransactionItem> transaction)
		{
			if (OnPOSEventReceive != null)
				OnPOSEventReceive(this, transaction);
		}

		private void NVROnCameraStatusReceive(Object sender, EventArgs<List<ICamera>> msg)
		{
			if (OnCameraStatusReceive != null)
				OnCameraStatusReceive(this, msg);
		}

		public void UtilityOnServerEventReceive(String msg)
		{
		}

		public void WriteOperationLog(String msg)
		{
		}
	}
}
