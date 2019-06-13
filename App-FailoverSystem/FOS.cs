using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App;
using Constant;
using DeviceConstant;
using Interface;
using ServerProfile;

namespace App_FailoverSystem
{
	public class FOS : IFOS
	{
		protected const String CgiLogin = @"cgi-bin/login?action=login";
		protected const String CgiLogout = @"cgi-bin/login?action=logout";

		public event EventHandler<EventArgs<String>> OnLoadComplete;
		public event EventHandler<EventArgs<String>> OnLoadFailure;
		public event EventHandler<EventArgs<String>> OnSaveComplete;
		public event EventHandler<EventArgs<String>> OnSaveFailure;

		public EventHandling EventHandling { get; set; }
		public IUtility Utility
		{
			get { return null; }
		}

	    public NVRStatus NVRStatus { get; set; }

	    public UInt16 Id { get; set; }
		public String Name { get; set; }
		public String Manufacture { get; set; }

        public String Driver { get; set; }

	    public Form Form { get; set; }
		public Boolean IsPatrolInclude { get; set; }
        public UInt16 ServerPort { get; set; }
        public UInt16 ServerStatusCheckInterval { get; set; }
        public List<IDevice> FailoverDeviceList { get; set; }
		public ServerCredential Credential { get; set; }

	    public Dictionary<ushort, IDevice> ReadNVRDeviceWithoutSaving()
	    {
	        return null;
	    }
        public List<IDevice> ReadDeviceList()
        {
            return null;
        }
	    public ILicenseManager License { get; private set; }
		public IServerManager Server { get; private set; }
		public IUserManager User { get; private set; }
		public INVRManager NVR { get; private set; }
		public IConfigureManager Configure { get; private set; }
		public IDeviceManager Device
		{
			get { return null; }
		}
		public IIOModelManager IOModel { get; set; }

		public ReadyState ReadyState { get; set; }
		public string LoginProgress { get; set; }

		private readonly System.Timers.Timer _loginTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _savingTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _serverTimeTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _storageUsageTimer = new System.Timers.Timer();
		public Dictionary<String, String> Localization;

		public FOS()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},
								   
								   {"App_Loading", "Loading"},
								   {"Loading_NVR", "NVR"},
								   {"Loading_License", "License"},
								   {"Loading_Server", "Server"},
								   {"Loading_User", "User"},
							   };
			Localizations.Update(Localization);

			Name = "Failover System";
			ReadyState = ReadyState.New;
			IsPatrolInclude = false;

			EventHandling = new EventHandling();
			EventHandling.SetDefaultFailoverEventHandling();
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

			NVR = new NVRManager
			{
				Server = this,
			};
			NVR.Initialize();
		}


		private const UInt16 LoginTimeout = 60000;
		public void Login()
		{
			_storageUsageTimer.Elapsed += ShowStorageUsage;
			_storageUsageTimer.Interval = 60000; //1 min
			_storageUsageTimer.Enabled = true;
			_storageUsageTimer.SynchronizingObject = Form;

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

			_loginTimer.Enabled = false;
			ReadyState = ReadyState.Ready;

			var groups = new List<IUserGroup>(User.Groups.Values);
			User.Groups.Clear();
			foreach (IUserGroup group in groups)
			{
				if ((group.Name == "Administrator" && group.Id == 0) || (group.Name == "Superuser" && group.Id == 1))
					User.Groups.Add(group.Id, group);
			}
			groups.Clear();

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
			ApplicationForms.ProgressBarValue = 90;

			if (User.Current.Group.CheckPermission("Setup", Permission.Server))
			{
				Server.OnSaveComplete -= SaveServerCallback;
				Server.OnSaveComplete += SaveServerCallback;
				Server.Save();
			}
			else
				FOSSaveComplete();
		}

		private void SaveServerCallback(Object sender, EventArgs e)
		{
			FOSSaveComplete();
		}

		private void FOSSaveComplete()
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

			_storageUsageTimer.Enabled =
			_serverTimeTimer.Enabled = false;
		}

		private void CheckLoginTimeout(Object sender, EventArgs e)
		{
			License.OnLoadComplete -= LoadLicenseCallback;
			Server.OnLoadComplete -= LoadServerCallback;
			NVR.OnLoadComplete -= LoadNVRCallback;
			User.OnLoadComplete -= LoadUserCallback;

			_loginTimer.Enabled = false;

			ReadyState = ReadyState.Unavailable;

			if (OnLoadFailure != null)
				OnLoadFailure(this, new EventArgs<String>(Status));
		}

		private void CheckSaveTimeout(Object sender, EventArgs e)
		{
			User.OnSaveComplete -= SaveUserCallback;
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
		}

		public void GroupModify(IDeviceGroup group)
		{
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
