using System;

namespace ServerProfile.Plugin
{
	public class CMS : ServerProfile.CMS
	{
		public override void Initialize()
		{
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

		//public void WriteOperationLog(String msg)
		//{
		//    WriteOperationLogDelegate writeOperationLogDelegate = WriteOperationLogOnBackground;
		//    writeOperationLogDelegate.BeginInvoke(msg, null, null);
		//}

		//private delegate void WriteOperationLogDelegate(String msg);
		//private void WriteOperationLogOnBackground(String msg)
		//{
		//    Xml.LoadXmlFromHttp("cgi-bin/log?action=add&data=" + msg, Credential);
		//}
	}
}
