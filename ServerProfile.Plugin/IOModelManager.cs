using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Xml;
using Constant;
using Device;
using Interface;

namespace ServerProfile.Plugin
{
	public class IOModelManager : IIOModelManager
	{
		public event EventHandler OnLoadComplete;
		public event EventHandler OnSaveComplete;
		private const String CgiLoadDevice = @"cgi-bin/dioconfig?action=loadall";
		private const String CgiSaveDevice = @"cgi-bin/dioconfig?action=saveall";
		private const String CgiDeleteDevice = @"cgi-bin/dioconfig?action=deleteall";

		public ManagerReadyState ReadyStatus { get; set; }
		public IServer Server;

		public Dictionary<String, String> Localization;
		public Dictionary<UInt16, IIOModel> IOModels { get; private set; }

		public IOModelManager()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"UserGroup_Administrator", "Administrator"},
                                   {"UserGroup_Superuser", "Superuser"},
                                   {"UserGroup_User", "User"},
                                   {"UserGroup_Guest", "Guest"},
                               };
            Localizations.Update(Localization);

            ReadyStatus = ManagerReadyState.New;
			IOModels = new Dictionary<UInt16, IIOModel>();
        }

		public void Initialize()
		{
		}

		public String Status
		{
			get { return "IO Model : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
		}

		private readonly Stopwatch _watch = new Stopwatch();

		private delegate void LoadDelegate();
		private delegate void SaveDelegate();

		public void Load()
		{
			ReadyStatus = ManagerReadyState.Loading;

			IOModels.Clear();
			
			_watch.Reset();
			_watch.Start();

			LoadDelegate loadDeviceDelegate = LoadIODevice;
			loadDeviceDelegate.BeginInvoke(LoadIODeviceCallback, loadDeviceDelegate);
		}

		public void Load(String xml)
		{
		}

		private void LoadIODeviceCallback(IAsyncResult result)
		{
			((LoadDelegate)result.AsyncState).EndInvoke(result);

			if (_loadIODeviceFlag)
			{
				_watch.Stop();
				const String msg = "IODevice Ready";
				Console.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));
				ReadyStatus = ManagerReadyState.Ready;

				if (OnLoadComplete != null)
					OnLoadComplete(this, null);
			}
		}

		private Boolean _loadIODeviceFlag;
		private void LoadIODevice()
		{
			IOModels.Clear();

			_loadIODeviceFlag = false;
			XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadDevice, Server.Credential);

			if (xmlDoc != null)
			{
				XmlNodeList devicesList = xmlDoc.GetElementsByTagName("DeviceConnectorConfiguration");
				foreach (XmlNode node in devicesList)
				{
					UInt16 id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

					IIOModel device = ParseIODeviceProfileFromXml((XmlElement) node);
					if (device != null)
						IOModels[id] = device;
				}
			}
			_loadIODeviceFlag = true ;
		}

		protected virtual IIOModel ParseIODeviceProfileFromXml(XmlElement node)
		{
			try
			{
				var model = new IOModel
				{
					Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID")),
					Name = Xml.GetFirstElementValueByTagName(node, "Name"),
					Manufacture = Xml.GetFirstElementValueByTagName(node, "Brand"),
					Credential = new ServerCredential
					             	{
					             		Domain = Xml.GetFirstElementValueByTagName(node, "IPAddress"),
										Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Password")),
										Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Port")),
										UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Account"))
					             	},
					DICount = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DICount")),
					DOCount = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DOCount")),
					ReadyState = ReadyState.Ready,
				};

				XmlNodeList handlesList = node.GetElementsByTagName("Handle");
                foreach (XmlElement item in handlesList)
                {
                	var nvrid = Xml.GetFirstElementValueByTagName(item, "NVR");
                	INVR nvr = null;
					
					UInt16 camera = 0;
					var cid = Xml.GetFirstElementValueByTagName(item, "Camera");

                    if (nvrid == "0")
                        nvr = (Server as INVR);
                    else if (nvrid != "")
						nvr = (Server as ICMS).NVR.FindNVRById( Convert.ToUInt16(nvrid) );

					if (cid != "")
						camera = Convert.ToUInt16(cid);

					model.Handles.Add(
						Xml.GetFirstElementValueByTagName(item, "Name"),
						new IOEventHandle
							{
								Name = Xml.GetFirstElementValueByTagName(item, "Name"),
								NVR = nvr,
								Camera = camera
							}
						);
				}

				return model;
			}
			catch (Exception exception)
			{
				Console.WriteLine(@"Parse Device XML Error " + exception);
			}

			return null;
		}

		public void Save()
		{
			ReadyStatus = ManagerReadyState.Saving;

			_watch.Reset();
			_watch.Start();

			SaveDelegate savIODeviceDelegate = SaveIODevice;
			savIODeviceDelegate.BeginInvoke(SaveCallback, savIODeviceDelegate);
		}

		public void Save(String xml)
		{
		}

		private Boolean _saveIODeviceFlag;
		private void SaveCallback(IAsyncResult result)
		{
			((SaveDelegate)result.AsyncState).EndInvoke(result);

			if (_saveIODeviceFlag)
			{
				_watch.Stop();
				Console.WriteLine(@"IODevice Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

				//if (ReadyStatus == ManagerReadyState.Saving)
					ReadyStatus = ManagerReadyState.Ready;

				if (OnSaveComplete != null)
					OnSaveComplete(this, null);
			}
		}

		private void SaveIODevice()
		{
			_saveIODeviceFlag = false;

			var xmlDoc = new XmlDocument();
			XmlElement xmlRoot = xmlDoc.CreateElement("AllDevices");
			xmlDoc.AppendChild(xmlRoot);

			foreach (var keyValuePair in IOModels)
			{
				var ioModel = keyValuePair.Value;

				var device = xmlDoc.CreateElement("DeviceConnectorConfiguration");
				device.SetAttribute("id", keyValuePair.Key.ToString());
				xmlRoot.AppendChild(device);

				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DeviceID", keyValuePair.Key));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", ioModel.Name));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Brand", ioModel.Manufacture));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IPAddress", ioModel.Credential.Domain));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", ioModel.Credential.Port));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", Encryptions.EncryptDES(ioModel.Credential.UserName)));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", Encryptions.EncryptDES(ioModel.Credential.Password)));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DICount", ioModel.DICount));
				device.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DOCount", ioModel.DOCount));

				var handles = xmlDoc.CreateElement("Handles");
				device.AppendChild(handles);

				foreach (var ioEventHandle in ioModel.Handles)
				{
					var handle = xmlDoc.CreateElement("Handle");
					handle.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", ioEventHandle.Key));
					handle.AppendChild(ioEventHandle.Value.NVR == null
					                   	? Xml.CreateXmlElementWithText(xmlDoc, "NVR", "")
					                   	: Xml.CreateXmlElementWithText(xmlDoc, "NVR", ioEventHandle.Value.NVR.Id.ToString()));

					handle.AppendChild(ioEventHandle.Value.Camera == null
					                   	? Xml.CreateXmlElementWithText(xmlDoc, "Camera", "")
					                   	: Xml.CreateXmlElementWithText(xmlDoc, "Camera", ioEventHandle.Value.Camera));
					handles.AppendChild(handle);
				}

                ioModel.ReadyState = ReadyState.Ready;
			}

			//Delete Old Config
			var xmlDel = new XmlDocument();
            xmlDel.LoadXml("<ID>1,2,3,4,5</ID>");
			Xml.PostXmlToHttp(CgiDeleteDevice, xmlDel, Server.Credential);

			//Save New Config
			Xml.PostXmlToHttp(CgiSaveDevice, xmlDoc, Server.Credential);

            int  timeoutMilliseconds = 20000;
            ServiceController service = new ServiceController("iSAPIOBridgeService");
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }


			_saveIODeviceFlag = true;
		}

		public UInt16 GetNewIOModelId()
		{
			for (UInt16 id = 1; id <= IOModels.Count + 2; id++)
			{
				if (IOModels.ContainsKey(id)) continue;
				return id;
			}

			return 0;
		}
	}
}
