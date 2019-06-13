using System;

namespace ServerProfile.Plugin
{
	public class NVR : ServerProfile.NVR
	{
		public override void Initialize()
		{
			if (Server == null)
			{
				Server = new ServerManager
				{
					Server = this,
				};
				Server.Initialize();
			}

			if (Device == null)
			{
				Device = new DeviceManager
				{
					Server = this,
				};
				Device.Initialize();
			}

			if (User == null)
			{
				User = new UserManager
				{
					Server = this,
				};
				User.Initialize();
			}

            if (IOModel == null)
            {
                IOModel = new IOModelManager
                {
                    Server = this,
                };
                IOModel.Initialize();
            }

			base.Initialize();
		}

        protected override void LoadDeviceCallback(Object sender, EventArgs e)
        {
            IOModel.OnLoadComplete -= LoadIoDeviceCallback;
            IOModel.OnLoadComplete += LoadIoDeviceCallback;
            IOModel.Load();
        }

        protected void LoadIoDeviceCallback(Object sender, EventArgs e)
        {
            IOModel.OnLoadComplete -= LoadIoDeviceCallback;

            base.LoadDeviceCallback(this, EventArgs.Empty);
        }

        protected override void SaveDeviceCallback(Object sender, EventArgs e)
        {
            IOModel.OnSaveComplete -= SaveIoDeviceCallback;
            IOModel.OnSaveComplete += SaveIoDeviceCallback;
            IOModel.Save();
        }

        private void SaveIoDeviceCallback(Object sender, EventArgs e)
        {
            IOModel.OnSaveComplete -= SaveIoDeviceCallback;

            base.SaveDeviceCallback(this, EventArgs.Empty);
        }
	}
}
