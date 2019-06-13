using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using Constant;
using Interface;
using ServerProfile;

namespace App_POSTransactionServer
{
	public class NVRManager : INVRManager
	{
		public event EventHandler OnLoadComplete;
		public event EventHandler OnSaveComplete;
		public event EventHandler OnNVRStatusUpdate;

		private const String CgiLoadNVR = @"cgi-bin/sysconfig?action=loadnvr";
		private const String CgiSaveNVR = @"cgi-bin/sysconfig?action=savenvr";
        private const String CgiLoadNVRAllDevice = @"cgi-bin/nvrconfig?action=loadalldevice";

        public ManagerReadyState ReadyStatus { get; set; }
		public IServer Server;

		public FailoverStatus FailoverStatus { get; private set; }
		public UInt16 SynchronizeProgress { get; private set; }

		public Dictionary<UInt16, INVR> NVRs { get; private set; }
		public Dictionary<String, MapAttribute> Maps { get; private set; }
	    public void LoadNVRDevicePresetPoint(INVR nvr)
	    {
	        
	    }

	    public ServerCredential ArchiveServer { get; set; }
		public NVRManager()
		{
			ReadyStatus = ManagerReadyState.New;
			FailoverStatus = FailoverStatus.Ping;
			SynchronizeProgress = 0;

			NVRs = new Dictionary<UInt16, INVR>();
			Maps = new Dictionary<String, MapAttribute>();
		}

		public void Initialize()
		{
		}

		public String Status
		{
			get { return "NVR : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
		}

		private readonly Stopwatch _watch = new Stopwatch();

		private Boolean _loadPTSFlag;
        public List<INVR> SearchNVR(String manufacture)
        {
            return null;
        }

	    public void Load()
		{
			ReadyStatus = ManagerReadyState.Loading;

			_watch.Reset();
			_watch.Start();

			LoadDelegate loadNVRDelegate = LoadNVR;
			loadNVRDelegate.BeginInvoke(LoadCallback, loadNVRDelegate);
		}

		public void Load(String xml)
		{
		}

		public void Save()
		{
			ReadyStatus = ManagerReadyState.Saving;

			_watch.Reset();
			_watch.Start();

			SaveDelegate savNVRDelegate = SaveNVR;
			savNVRDelegate.BeginInvoke(SaveCallback, savNVRDelegate);
		}

		public void Save(String xml)
		{
		}

		protected void LoadNVR()
		{
			NVRs.Clear();

			_loadPTSFlag = false;

			LoadNVRList();
            LoadNVRDevice();
            _loadPTSFlag = true;
		}

		private void LoadNVRList()
		{
			//<CMS>
			//<NVR id="1" name="gaga">
			//    <Domain>172.16.1.99</Domain>
			//    <Port>88</Port>
			//    <Account>2c5ZjyTLEx0=</Account>
			//    <Password>vNIHQ8oOrg0=</Password>
			//</NVR>
			//</CMS>

			XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential);
			
			if (xmlDoc == null) return;

			XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
			foreach (XmlElement nvrNode in nvrNodes)
			{
				var modified = Xml.GetFirstElementValueByTagName(nvrNode, "Modified");
				var manufacture = Xml.GetFirstElementValueByTagName(nvrNode, "Manufacture");
				INVR nvr;
				//switch (manufacture)
				//{
				//    case "Salient":
				//        nvr = new SalientNVR();
				//        break;

				//    default:
				//        nvr = new NVR();
				//        break;
				//}
				// manufacture doesn't matter~
				switch (((IPTS)Server).ReleaseBrand)
				{
					case "Salient":
						nvr = new SalientNVR();
						break;

					default:
						nvr = new NVR();
						break;
				}

				nvr.Id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
				nvr.Name = nvrNode.GetAttribute("name");
				nvr.Form = Server.Form;
				nvr.ModifiedDate = (String.IsNullOrEmpty(modified)) ? 0 : Convert.ToUInt64(modified);
				nvr.Manufacture = Xml.GetFirstElementValueByTagName(nvrNode, "Manufacture");
				nvr.Credential = new ServerCredential
									 {
										 Domain = Xml.GetFirstElementValueByTagName(nvrNode, "Domain").Trim(),
										 Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "Port")),
										 UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Account")),
										 Password =
											 Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Password")),
									 };
				nvr.IsListenEvent = (Xml.GetFirstElementValueByTagName(nvrNode, "IsListenEvent") != "false");
				nvr.IsPatrolInclude = (Xml.GetFirstElementValueByTagName(nvrNode, "IsPatrolInclude") != "false");

				if (NVRs.ContainsKey(nvr.Id)) continue;

				NVRs.Add(nvr.Id, nvr);
			}
		}

		public void SaveNVRDocument()
		{
		}


		private Boolean _saveNVRFlag;

		private void SaveNVR()
		{
			_saveNVRFlag = false;
            var originalXml = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential);

            Xml.PostXmlToHttp(CgiSaveNVR, ParseNVRToXml(), Server.Credential);

			_saveNVRFlag = true;
		}

		public void UpdateFailoverDeviceList(UInt16 nvrId, INVR nvr)
		{
		}

		private XmlDocument ParseNVRToXml()
		{
			var xmlDoc = new XmlDocument();

			var xmlRoot = xmlDoc.CreateElement("CMS");
			xmlDoc.AppendChild(xmlRoot);

			var nvrSortResult = new List<INVR>(NVRs.Values);
			nvrSortResult.Sort((x, y) => (x.Id - y.Id));

			foreach (INVR nvr in nvrSortResult)
			{
				var nvrNode = xmlDoc.CreateElement("NVR");
				nvrNode.SetAttribute("id", nvr.Id.ToString());
				nvrNode.SetAttribute("name", nvr.Name);
				
				if (nvr.ReadyState != ReadyState.Ready) 
					nvr.ModifiedDate = DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone);

				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Manufacture", nvr.Manufacture));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Domain", nvr.Credential.Domain));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", nvr.Credential.Port.ToString()));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", Encryptions.EncryptDES(nvr.Credential.UserName)));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", Encryptions.EncryptDES(nvr.Credential.Password)));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsListenEvent", nvr.IsListenEvent ? "true" : "false"));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsPatrolInclude", nvr.IsPatrolInclude ? "true" : "false"));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Modified", nvr.ModifiedDate));

				xmlRoot.AppendChild(nvrNode);
			}

			return xmlDoc;
		}

		public void UpdateNVRStatus()
		{
		}

		//private delegate void LoadCallbackDelegate(IAsyncResult result);
		private delegate void LoadDelegate();
		private void LoadCallback(IAsyncResult result)
		{
			//if (CMS.Form.InvokeRequired)
			//{
			//    try
			//    {
			//        CMS.Form.Invoke(new LoadCallbackDelegate(LoadCallback), result);
			//    }
			//    catch (Exception)
			//    {
			//    }
			//    return;
			//}
			
			((LoadDelegate)result.AsyncState).EndInvoke(result);

			if (_loadPTSFlag)
			{
				_watch.Stop();
                //const String msg = "NVR Ready";
                //Console.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));
				ReadyStatus = ManagerReadyState.Ready;

				if (OnLoadComplete != null)
					OnLoadComplete(this, null);
			}
		}

		//private delegate void SaveCallbackDelegate(IAsyncResult result);
		private delegate void SaveDelegate();
		private void SaveCallback(IAsyncResult result)
		{
			//if (CMS.Form.InvokeRequired)
			//{
			//    try
			//    {
			//        CMS.Form.Invoke(new SaveCallbackDelegate(SaveCallback), result);
			//    }
			//    catch (Exception)
			//    {
			//    }
			//    return;
			//}

			((SaveDelegate)result.AsyncState).EndInvoke(result);

			if (!_saveNVRFlag) return;

			_watch.Stop();
			Console.WriteLine(@"NVR Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

			foreach (var obj in NVRs)
			{
				//if nvr not modify, ignore verify it
				if (obj.Value.ReadyState == ReadyState.Ready) continue;

				obj.Value.ReadyState = (obj.Value.ValidateCredential()) ? ReadyState.ReSync : ReadyState.Unavailable;
			}

			ReadyStatus = ManagerReadyState.Ready;

			if (OnSaveComplete != null)
				OnSaveComplete(this, null);
		}

		public void LoadMap()
		{
		}

		public void SaveMap(XmlDocument mapDocument)
		{
		}

		public Boolean UploadMap(Bitmap map, String filename)
		{
			return true;
		}

		public Bitmap GetMap(String filename)
		{
			return null;
		}

		public INVR FindNVRById(UInt16 nvrId)
		{
			return NVRs.ContainsKey(nvrId) ? NVRs[nvrId] : null;
		}

		public MapAttribute FindMapById(String mapId)
		{
			return null;
		}

		public UInt16 GetNewNVRId()
		{
			const ushort max = 65535;

			for (UInt16 id = 1; id <= max; id++)
			{
				if (NVRs.ContainsKey(id)) continue;
				return id;
			}

			return 0;
		}

        public event EventHandler DeviceChanged;

        private void OnDeviceChanged(EventArgs e)
        {
            var handler = DeviceChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public ushort MaximunNVRAmount { get; private set; }

        public Dictionary<IDevice, ushort> DeviceChannelTable { get; private set; }

        public void AddDeviceChannelTable(IDevice device)
        {
            throw new NotImplementedException();
        }

        public void RemoveDeviceChannelTable(IDevice device)
        {
            throw new NotImplementedException();
        }

        protected void LoadNVRDevice()
        {
            XmlDocument xmlDoc = null;
            xmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVRAllDevice, Server.Credential);
            if (xmlDoc == null) return;

            XmlNodeList nvrList = xmlDoc.GetElementsByTagName("NVR");
            foreach (XmlElement nvrNode in nvrList)
            {
                var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
                if (!NVRs.ContainsKey(id)) continue;
                var nvr = NVRs[id];
                nvr.TempDevices.Clear();
                XmlNodeList devicesList = nvrNode.GetElementsByTagName("DeviceConnectorConfiguration");
                foreach (XmlElement node in devicesList)
                {
                    UInt16 channelId = Convert.ToUInt16(node.GetAttribute("id"));
                    String name = node.GetAttribute("name");
                    UInt16 deviceId = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DeviceID"));

                    //IDevice device = ParseDeviceProfileFromXml(node, nvr);
                    //if (device != null && !nvr.Device.Devices.ContainsKey(deviceId))
                    //{
                    //    if (!String.IsNullOrEmpty(name))
                    //        device.Name = name;
                    //    //nvr.Device.Groups[0].Items.Add(device);
                    //    if (!DeviceChannelTable.ContainsKey(device))
                    //        DeviceChannelTable.Add(device, channelId);
                    //    nvr.Device.Devices.Add(deviceId, device);
                    //    nvr.TempDevices.Add(deviceId, device);
                    //    device.ReadyState = ReadyState.Ready;
                    //}
                }
            }

            //LoadNVRDevicePresetPoint();
           // LoadNVRDeviceBookmark();
            Server.Device.LoadAllEvent();
        }

    }
}
