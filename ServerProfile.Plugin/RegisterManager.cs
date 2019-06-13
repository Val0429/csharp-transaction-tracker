using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Constant;
using Interface;
//using PosBase;

namespace ServerProfile.Plugin
{
	public class RegisterManager : IRegisterManager
	{
		public event EventHandler OnLoadComplete;
		public event EventHandler OnSaveComplete;

		public ManagerReadyState ReadyStatus { get; set; }
		public IServer Server;

		public Dictionary<Int32, IRegister> Registers { get; private set; }

		public RegisterManager()
		{
			ReadyStatus = ManagerReadyState.New;

			Registers = new Dictionary<int, IRegister>();
		}

		public void Initialize()
		{
		}

		public String Status
		{
			get { return "Register : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
		}

		private readonly Stopwatch _watch = new Stopwatch();

		public void Load()
        {
            ReadyStatus = ManagerReadyState.Ready;

            if (OnLoadComplete != null)
                OnLoadComplete(this, null);

            return;
			ReadyStatus = ManagerReadyState.Loading;

			Registers.Clear();

			_watch.Reset();
			_watch.Start();

			LoadDelegate loadDataDelegate = LoadDatas;
			loadDataDelegate.BeginInvoke(LoadCallback, loadDataDelegate);
		}

		public void Load(String xml)
		{
		}

		public void Save()
		{
			ReadyStatus = ManagerReadyState.Saving;

			_watch.Reset();
			_watch.Start();

			SaveDelegate savDataDelegate = SaveDatas;
			savDataDelegate.BeginInvoke(SaveCallback, savDataDelegate);
		}

		public void Save(String xml)
		{
		}

		//private readonly POS _pos = new POS();

		private Boolean _loadRegisterFlag;
		private void LoadDatas()
		{
            //Registers.Clear();

            //_loadRegisterFlag = false;

            //XmlDocument xmlDoc = _pos.LoadRegisterFromDatabase();

            //if (xmlDoc != null)
            //{
            //    XmlNodeList regNodes = xmlDoc.GetElementsByTagName("Register");
            //    foreach (XmlElement nvrNode in regNodes)
            //    {
            //        var register = new Register
            //        {
            //            Id = Convert.ToUInt16(nvrNode.GetAttribute("id")),
            //            Name = nvrNode.GetAttribute("name"),

            //            Store = Xml.GetFirstElementsValueByTagName(nvrNode, "Store").Trim().PadLeft(5, '0'),
            //            IpAddress = Xml.GetFirstElementsValueByTagName(nvrNode, "IpAddress").Trim(),
            //            Layout = Convert.ToInt32(Xml.GetFirstElementsValueByTagName(nvrNode, "Layout").Trim()),
            //            Devices = new List<int>()
            //        };
            //        register.ToDevices(Xml.GetFirstElementsValueByTagName(nvrNode, "Devices").Trim());

            //        if (Registers.ContainsKey(register.Id)) continue;

            //        Registers.Add(register.Id, register);
            //    }
            //}

            //_loadRegisterFlag = true;
		}

		private Boolean _saveRegistersFlag;
		private void SaveDatas()
		{
            //_saveRegistersFlag = false;

            //XmlDocument doc = ParseRegistersToXml();

            //if (doc != null)
            //    _pos.SaveRegisterToDatabase(doc) ;


            //_saveRegistersFlag = true;
		}

		private XmlDocument ParseRegistersToXml()
		{
            //XmlDocument xmlDoc = new XmlDocument();

            //XmlElement xmlRoot = xmlDoc.CreateElement("CMS");
            //xmlDoc.AppendChild(xmlRoot);

            //List<IRegister> regSortResult = new List<IRegister>(Registers.Values);
            //regSortResult.Sort((x, y) => (x.Id - y.Id));

            //foreach (IRegister reg in regSortResult)
            //{
            //    XmlElement node = xmlDoc.CreateElement("Register");
            //    node.SetAttribute("id", reg.Id.ToString());
            //    node.SetAttribute("name", reg.Name);

            //    node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Store", reg.Store));
            //    node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IpAddress", reg.IpAddress));
            //    node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Layout", reg.Layout.ToString()));
            //    node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Devices", reg.ToString()));

            //    xmlRoot.AppendChild(node);
            //}

            //return xmlDoc;
		    return null;
		}

		private delegate void LoadCallbackDelegate(IAsyncResult result);
		private delegate void LoadDelegate();
		private void LoadCallback(IAsyncResult result)
		{
			((LoadDelegate)result.AsyncState).EndInvoke(result);

			if (_loadRegisterFlag)
			{
				_watch.Stop();
				const String msg = "Register Ready";
				Console.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));
				ReadyStatus = ManagerReadyState.Ready;

				if (OnLoadComplete != null)
					OnLoadComplete(this, null);

				Server.LoginProgress = msg;
			}
		}

		private delegate void SaveCallbackDelegate(IAsyncResult result);
		private delegate void SaveDelegate();
		private void SaveCallback(IAsyncResult result)
		{
			((SaveDelegate)result.AsyncState).EndInvoke(result);

			if (_saveRegistersFlag)
			{
				_watch.Stop();
				Console.WriteLine(@"Register Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

				Boolean hasNew = false;

				foreach (KeyValuePair<Int32, IRegister> obj in Registers)
				{
					//if (!hasNew && (obj.Value.ReadyState == ReadyState.New/* || obj.Value.ReadyState == ReadyState.Unavailable*/ || obj.Value.Server.ReadyStatus == ManagerReadyState.MajorModify))
					//    hasNew = true;

					//Boolean validate = obj.Value.ValidateCredential();
					//if (validate)
					//{
					//    if (obj.Value.ReadyState == ReadyState.Unavailable || obj.Value.ReadyState == ReadyState.Modify)
					//        hasNew = true;
					//}
					//else
					//    obj.Value.ReadyState = ReadyState.Unavailable;

				}

				ReadyStatus = (hasNew)
					? ManagerReadyState.MajorModify : ManagerReadyState.Ready;

				if (OnSaveComplete != null)
					OnSaveComplete(this, null);
			}
		}

		public UInt16 GetNewRegisterId()
		{
			UInt16 max = (UInt16)65535;
			for (UInt16 id = 1; id <= max; id++)
			{
				if (Registers.ContainsKey(id)) continue;
				return id;
			}

			return 0;
		}
	}
}
