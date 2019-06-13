using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace ServerProfile.Plugin
{
	public class PluginLicenseManager : LicenseManager
	{
		private Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		private ClientSettingsSection _section = null;

		public override event EventHandler OnPluginSaveComplete;

		private const String CgiVerifyPluginLicenseKey = @"cgi-bin/sysconfig?action=verifypluginlicensekey"; //check online license-amount
		private const String CgiAddPluginLicenseKey = @"cgi-bin/sysconfig?action=addpluginlicensekey"; //online
		private const String CgiAddPluginLicenseFile = @"cgi-bin/sysconfig?action=addpluginlicensexml"; //offline

		private readonly Dictionary<PluginPackage, Int16> _pluginLicnese;
		public override Dictionary<PluginPackage, Int16> PluginLicnese
		{
			get { return _pluginLicnese; }
		}

		public PluginLicenseManager()
		{
			_pluginLicnese = new Dictionary<PluginPackage, Int16>
			                	{
			                		{PluginPackage.Unknow, 0},
			                		{PluginPackage.Google, 0},
			                	};

			_section = ((ClientSettingsSection)_config.GetSection("applicationSettings/SNVR.Properties.Settings"));
		}

		public override void LoadLicense()
		{
			_loadLicenseFlag = false;

			base.LoadLicense();

			String amount = "";

			var key = _section.Settings.Get("PluginKey").Value.ValueXml.InnerText;
			String Post = Xml.PostTextToHttpWithGzip(CgiVerifyPluginLicenseKey, key, Server.Credential);
			if (!String.IsNullOrEmpty(Post))
			{
				var ret = Xml.LoadXml(Post);
				var enable = Xml.GetFirstElementsValueByTagName(ret, "Enable");

				if ( enable == "1")
					amount = Xml.GetFirstElementsValueByTagName(ret, "Count");
			}

			if (amount != "")
			{
				Int16 plugin = 0;
				try
				{
					plugin = Convert.ToInt16(amount);
				}
				catch (Exception)
				{
					plugin = 0;
				}


				PluginLicnese[PluginPackage.Google] = plugin;
			}

			_loadLicenseFlag = true;
		}

		public override void SavePlugin()
		{
		}

		public override void SavePlugin(String xml)
		{
			_watch.Reset();
			_watch.Start();
			AddKeyTimer.Enabled = true;

			ApplicationForms.ShowProgressBar(Server.Form);

			if (xml.Length == 29)
			{
				_licenseKey = xml;
				//VeriflyKeyDelegate veriflyKeyDelegate = VeriflyLicenseKey;
				//veriflyKeyDelegate.BeginInvoke(xml, VerifyCallback, veriflyKeyDelegate);
				SubmitLicenseDelegate submitLicenseDelegate = SubmitLicense;
				submitLicenseDelegate.BeginInvoke(_licenseKey, SaveCallback, submitLicenseDelegate);

				ApplicationForms.ProgressBarValue = 30;
			}
			else
			{
				_previousAmount = PluginLicnese[PluginPackage.Google] ;
				SubmitLicenseDelegate submitLicenseDelegate = SubmitLicense;
				submitLicenseDelegate.BeginInvoke(xml, SaveCallback, submitLicenseDelegate);

				ApplicationForms.ProgressBarValue = 80;
			}

		}

		protected new void SaveCallback(IAsyncResult result)
		{
			var submitLicense = (SubmitLicenseDelegate)result.AsyncState;

			String verifly = submitLicense.EndInvoke(result);

			if (AddKeyTimer.Enabled)
			{
				_section.Settings.Get("PluginKey").Value.ValueXml.InnerText = _licenseKey;
				_config.Save(ConfigurationSaveMode.Minimal, true);
				ConfigurationManager.RefreshSection("applicationSettings/SNVR.Properties.Settings");

				_watch.Stop();
				Console.WriteLine(@"License Add: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));
				ReadyStatus = ManagerReadyState.Ready;

				Thread.Sleep(5000);//wait 5 sec to get new license amount, server need a little time to update license file
				LoadLicense();

				ApplicationForms.ProgressBarValue = 80;

				AddKeyTimer.Enabled = false;

				Boolean passVerifly = true;

				switch (verifly)
				{
					case "-1":
						TopMostMessageBox.Show(Localization["Application_RegistrationServerNotAvailable"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;

					case "-2":
						TopMostMessageBox.Show(Localization["Application_LicenseKeyError"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;

					case "-3":
						TopMostMessageBox.Show(Localization["Application_LicenseKeyUsed"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;

					case "-4":
						TopMostMessageBox.Show(Localization["Application_LicenseKeyExpired"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;

					case "-5":
						TopMostMessageBox.Show(Localization["Application_NetworkCardNotAvailable"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;

					case "-6":
						TopMostMessageBox.Show(Localization["Application_LicenseFileCantUpdate"], Localization["MessageBox_Error"],
							   MessageBoxButtons.OK, MessageBoxIcon.Error);
						passVerifly = false;
						break;
				}

				ApplicationForms.ProgressBarValue = 100;

				if (passVerifly)
				{
					if (Amount > _previousAmount)
						TopMostMessageBox.Show(Localization["Application_RegistrationCompleted"].Replace("{AMOUNT}", PluginLicnese[PluginPackage.Google].ToString()), Localization["MessageBox_Information"],
						   MessageBoxButtons.OK, MessageBoxIcon.Information);
					//else
					//    TopMostMessageBox.Show(Localization["Application_RegistrationFailed"], Localization["MessageBox_Error"],
					//       MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if (OnPluginSaveComplete != null)
					OnPluginSaveComplete(this, null);

				ApplicationForms.HideProgressBar(Server.Form);
			}
		}

		protected new Int16 _previousAmount;
		protected new String _licenseKey = "";

		protected new String SubmitLicense(String key)
		{
			if (key.Length == 29)
				return Xml.PostTextToHttpWithGzip(CgiAddPluginLicenseKey, key, Server.Credential);

			return Xml.PostTextToHttpWithGzip(CgiAddPluginLicenseFile, key, Server.Credential);
		}

		//protected new String VeriflyLicenseKey(String key)
		//{
		//    var num = "";
		//    if (key.Length == 29)
		//    {
		//        //InnerXml	"<LICENSE><Enable>0</Enable><Count>0</Count></LICENSE>"	string

		//        var ret = Xml.LoadXml(Xml.PostTextToHttpWithGzip(CgiVerifyPluginLicenseKey, key, Server.Credential));

		//        num = Xml.GetFirstElementsByTagName(ret, "Count").InnerText;

		//    }
		//    return num ;
		//}

	}
}
