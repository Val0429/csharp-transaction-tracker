using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using ServerProfile;

namespace App_VideoAnalyticsServer
{
	public class VAS : IVAS
	{
		protected const String CgiLogin = @"cgi-bin/login?action=login";
		protected const String CgiLogout = @"cgi-bin/login?action=logout";

		public event EventHandler<EventArgs<String>> OnLoadComplete;
		public event EventHandler<EventArgs<String>> OnLoadFailure;
		public event EventHandler<EventArgs<String>> OnSaveComplete;
		public event EventHandler<EventArgs<String>> OnSaveFailure;

		public IUtility Utility
		{
			get { return null; }
		}

		public UInt16 Id { get; set; }
		public String Name { get; set; }
		public String Manufacture { get; set; }
		public Form Form { get; set; }
		public Boolean IsPatrolInclude { get; set; }

		public ServerCredential Credential { get; set; }

		public ILicenseManager License { get; private set; }
		public IServerManager Server { get; private set; }
		public IUserManager User { get; private set; }
		public IDeviceManager Device { get; private set; }
		public INVRManager NVR { get; private set; }
		public IConfigureManager Configure
		{
			get { return null; }
		}
		public IIOModelManager IOModel { get; set; }

		public ReadyState ReadyState { get; set; }
		public string LoginProgress { get; set; }

		private readonly System.Timers.Timer _loginTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _savingTimer = new System.Timers.Timer();
		public Dictionary<String, String> Localization;

		public VAS()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},
								   
								   {"App_Loading", "Loading"},
								   {"Loading_Config", "Config"},
								   {"Loading_Device", "Device"},
								   {"Loading_License", "License"},
								   {"Loading_Server", "Server"},
								   {"Loading_User", "User"},
								   {"Loading_NVR", "NVR"},
							   };
			Localizations.Update(Localization);

			Name = "Network Video Recorder";
			ReadyState = ReadyState.New;
			IsPatrolInclude = false;

			LoginProgress = Localization["App_Loading"];
		}

		public String Status
		{
			get
			{
				return String.Join(Environment.NewLine, new[]
				{
					License.Status,
					User.Status,
					Device.Status, 
					Server.Status, 
					NVR.Status, 
				});
			}
		}

		public void Initialize()
		{
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
		}

		private const UInt16 LoginTimeout = 60000;
		public void Login()
		{
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

			var groups = new List<IUserGroup>(User.Groups.Values);
			User.Groups.Clear();
			foreach (IUserGroup group in groups)
			{
				if ((group.Name == "Administrator" && group.Id == 0) || (group.Name == "Superuser" && group.Id == 1))
					User.Groups.Add(group.Id, group);
			}
			groups.Clear();

			LoadDevice();
		}

		private void LoadDevice()
		{
			Device.OnLoadComplete -= LoadDeviceCallback;
			Device.OnLoadComplete += LoadDeviceCallback;

			LoginProgress = Localization["App_Loading"] + " " + Localization["Loading_Device"];
			Device.Load();
		}

		private void LoadDeviceCallback(Object sender, EventArgs e)
		{
			Device.OnLoadComplete -= LoadDeviceCallback;

			_loginTimer.Enabled = false;
			ReadyState = ReadyState.Ready;

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
			SaveUser();
		}

		private void SaveUser()
		{
			ApplicationForms.ProgressBarValue = 20;

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
			ApplicationForms.ProgressBarValue = 40;

			if (User.Current.Group.CheckPermission("Setup", Permission.PeopleCounting))
			{
				Device.OnSaveComplete -= SaveDeviceCallback;
				Device.OnSaveComplete += SaveDeviceCallback;
				Device.Save();
			}
			else
				SaveNVR();
		}

		private void SaveDeviceCallback(Object sender, EventArgs e)
		{
			Device.OnSaveComplete -= SaveDeviceCallback;

			SaveNVR();
		}

		private void SaveNVR()
		{
			ApplicationForms.ProgressBarValue = 60;

			if (User.Current.Group.CheckPermission("Setup", Permission.NVR))
			{
				NVR.OnSaveComplete -= SaveNVRCallback;
				NVR.OnSaveComplete += SaveNVRCallback;
				NVR.Save();
			}
			else
				SaveServer();
		}

		private void SaveNVRCallback(Object sender, EventArgs e)
		{
			NVR.OnSaveComplete -= SaveNVRCallback;

			SaveServer();
		}

		private void SaveServer()
		{
			ApplicationForms.ProgressBarValue = 80;

			if (User.Current.Group.CheckPermission("Setup", Permission.Server))
			{
				Server.OnSaveComplete -= SaveServerCallback;
				Server.OnSaveComplete += SaveServerCallback;
				Server.Save();
			}
			else
				VASSaveComplete();
		}

		private void SaveServerCallback(Object sender, EventArgs e)
		{
			VASSaveComplete();
		}

		private void VASSaveComplete()
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
		}

		private void CheckLoginTimeout(Object sender, EventArgs e)
		{
			License.OnLoadComplete -= LoadLicenseCallback;
			Server.OnLoadComplete -= LoadServerCallback;
			NVR.OnLoadComplete -= LoadNVRCallback;
			User.OnLoadComplete -= LoadUserCallback;
			Device.OnLoadComplete -= LoadDeviceCallback;

			_loginTimer.Enabled = false;

			ReadyState = ReadyState.Unavailable;

			if (OnLoadFailure != null)
				OnLoadFailure(this, new EventArgs<String>(Status));
		}

		private void CheckSaveTimeout(Object sender, EventArgs e)
		{
			User.OnSaveComplete -= SaveUserCallback;
			Device.OnSaveComplete -= SaveDeviceCallback;
			NVR.OnSaveComplete -= SaveNVRCallback;
			Server.OnSaveComplete -= SaveServerCallback;
			_savingTimer.Enabled = false;

			ReadyState = ReadyState.Modify;

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
		}

		public void NVRModify(INVR nvr)
		{
			if (nvr.ReadyState != ReadyState.Delete) return;
			var list = new Queue<IDevice>();

			foreach (KeyValuePair<UInt16, IDevice> obj in Device.Devices)
			{
				if (obj.Value.Server == nvr)
				{
					list.Enqueue(obj.Value);
				}
			}

			while (list.Count > 0)
			{
				IDevice device = list.Dequeue();
				Device.Groups.Values.First().Items.Remove(device);
				Device.Groups.Values.First().View.Remove(device);
				Device.Devices.Remove(device.Id);
			}
		}

		public Boolean ValidateCredential()
		{
			return false;
		}

		public Boolean ValidateCredentialWithMessage()
		{
			return true;
		}

		public void WriteOperationLog(String msg)
		{
		}
	}
}
