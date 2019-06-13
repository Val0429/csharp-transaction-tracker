using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Constant;
using PanelBase;

namespace App_NetworkVideoRecorder
{
	public class LoginForm : App.LoginForm
	{
		public LoginForm()
		{
			DefaultPort = 80;
			Icon = Properties.Resources.icon;

			Localization.Add("LoginForm_NVR", "NVR");
			Localization.Add("Application_DevicePackUpdate", "New device pack detected (version %1).  Please confirm for upgrade.");

			AppProperties = new AppClientProperties();
		}

		public override void ShowSplash()
		{
			//ExecutionFile = "NVR.exe";
			// ====	Modify by Tulip
			// For Plugin use check if App is created in Plugin
			if (App == null)
			{
				App = new NetworkVideoRecorder
						{
							Credential = new ServerCredential
											{
												Port = Port,
												Domain = Host,
												UserName = Account,
												Password = Password,
												SSLEnable = SSLEnable,
											},
							Language = AppProperties.DefaultLanguage
						};
			}

			base.ShowSplash();

			CheckDevicePackVersion();

			IsLoading = false;
			Application.Idle -= LoginServer;
			Application.Idle += LoginServer;
		}

		private const String CgiLoadCapability = @"cgi-bin/sysconfig?action=loadcapability";
		private const String CgiLoadVersionInfo = @"cgi-bin/versioninfo";
		private const String DevicePackLink = @"clientpack/%1/client.pak";
		private void CheckDevicePackVersion()
		{
			//in debug mode DONT update device pack
			if (Debugger.IsAttached || (ModifierKeys & Keys.Shift) == Keys.Shift)
				return;

			var appInfo = new FileInfo("DevicePackUpdater.exe");
			if (!appInfo.Exists)
				return;

			XmlDocument xmlDoc;
			var devicePackVersion = "";
			var minDevicePackVersion = "";
			try
			{
				if (File.Exists("version.xml"))
				{
					xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(File.ReadAllText("version.xml"));

					devicePackVersion = Xml.GetFirstElementValueByTagName(xmlDoc, "DevicePack");
					minDevicePackVersion = Xml.GetFirstElementValueByTagName(xmlDoc, "MinDevicePack");
				}
				//var myFileVersionInfo = FileVersionInfo.GetVersionInfo("DeviceCab.dll");
				//devicePackVersion = myFileVersionInfo.FileVersion;
			}
			catch (Exception)
			{
			}

			var needUpdateDevicePack = false;

			//can't get xml version -> something wrong
			if (String.IsNullOrEmpty(devicePackVersion))
			{
				needUpdateDevicePack = true;
			}

			//compare with server version.
			if (needUpdateDevicePack) return;

			var credential = new ServerCredential
			{
				Port = Port,
				Domain = Host,
				UserName = Account,
				Password = Password,
				SSLEnable = SSLEnable,
			};

			var serverDevicePackVersion = "";
			var brand = "";
			var productorNo = "";

			xmlDoc = Xml.LoadXmlFromHttp(CgiLoadVersionInfo, credential);

			if (xmlDoc == null) return;
			XmlNode node = xmlDoc.SelectSingleNode("VERSION");

			if (node != null)
			{
				serverDevicePackVersion = Xml.GetFirstElementValueByTagName(node, "DevicePack");
			}

			//can't get server device pack version, do nothing.
			//if (String.IsNullOrEmpty(serverDevicePackVersion)) return;


			if(String.IsNullOrEmpty(serverDevicePackVersion)) return;
			var serverDevicePackVer = Convert.ToInt32(serverDevicePackVersion.Replace(".", ""));

			var devicePackVer = Convert.ToInt32(devicePackVersion.Replace(".", ""));
			var minDevicePackVer = Convert.ToInt32(minDevicePackVersion.Replace(".", ""));

			//server device pack version is OLDER than client's mini device pack version, DONT UPDATE
			if (Convert.ToInt32(serverDevicePackVer) < Convert.ToInt32(minDevicePackVer)) return;

			//if version is THE SAME, dont patch
			if (devicePackVer == serverDevicePackVer) return;
			//--------------------------------------------------------

			xmlDoc = Xml.LoadXmlFromHttp(CgiLoadCapability, credential);

			if (xmlDoc == null) return;
			node = xmlDoc.SelectSingleNode("Capability");

			if (node != null)
			{
				brand = Xml.GetFirstElementValueByTagName(node, "Brand");
				productorNo = Xml.GetFirstElementValueByTagName(node, "ProductNO");
			}

			//can't get server brand or productorNO
			if (String.IsNullOrEmpty(brand) || String.IsNullOrEmpty(productorNo)) return;
			//--------------------------------------------------------

			TopMostMessageBox.Show(Localization["Application_DevicePackUpdate"].Replace("%1", serverDevicePackVersion), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

			//var path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;

			var args = String.Format("{0} {1} {2}", Process.GetCurrentProcess().MainModule.ModuleName, brand, productorNo);

			try
			{
				var client = new WebClient { Credentials = credential };
				var httpMode = (credential.SSLEnable) ? "https://" : "http://";
				var uri = httpMode + credential.Domain + ":" + credential.Port + "/" + DevicePackLink.Replace("%1", (IntPtr.Size == 8) ? "64" : "32");

				var fileInfo = new FileInfo("client.pak");
				if (fileInfo.Exists)
				{
					File.Delete("client.pak");
				}

				client.DownloadFile(new Uri(uri), "client.pak");
			}
			catch (Exception)
			{
			}

			//if client.pak is exists, start update
			var clientFileInfo = new FileInfo("client.pak");
			if (clientFileInfo.Exists)
			{
				//call device pack updater, quit current ap
				try
				{
					Process.Start("DevicePackUpdater.exe", args);
				}
				catch (Exception exception)
				{
					TopMostMessageBox.Show("Device Pack Update Error" + Environment.NewLine + 
					exception, Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				Application.Exit();
			}
		}

		protected override void UpdateUI()
		{
			base.UpdateUI();

			signInLabel.Text += @" " + Localization["LoginForm_NVR"];
		}
	}
}
