using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Constant;
using Interface;
//using PosBase;

namespace ServerProfile.Plugin
{
	public class PosExceptionManager : IPosExceptionManager
	{
		public event EventHandler OnLoadComplete;
		public event EventHandler OnSaveComplete;

		public ManagerReadyState ReadyStatus { get; set; }
		public IServer Server;

		public Dictionary<Int32, IPosException> PosExceptions { get; private set; }

		public PosExceptionManager()
		{
			ReadyStatus = ManagerReadyState.New;

			PosExceptions = new Dictionary<int, IPosException>();
		}

		public void Initialize()
		{
		}

		public String Status
		{
			get { return "PosExceptions : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
		}

		private readonly Stopwatch _watch = new Stopwatch();

		public void Load()
		{
			ReadyStatus = ManagerReadyState.Ready;

			if (OnLoadComplete != null)
				OnLoadComplete(this, null);

			return;
			ReadyStatus = ManagerReadyState.Loading;

			PosExceptions.Clear();

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

		private Boolean _loadFlag;
		private void LoadDatas()
		{
			//PosExceptions.Clear();

			//_loadFlag = false;

			//XmlDocument xmlDoc = _pos.LoadEventExceptionFromDatabase();

			//if (xmlDoc != null)
			//{
			//    XmlNodeList regNodes = xmlDoc.GetElementsByTagName("Exception");
			//    foreach (XmlElement nvrNode in regNodes)
			//    {
			//        var excep = new PosException
			//        {
			//            Id = Convert.ToUInt16(nvrNode.GetAttribute("id")),
			//            Name = nvrNode.GetAttribute("name"),
			//            Keyword = Xml.GetFirstElementsValueByTagName(nvrNode, "Keyword").Trim(),
			//            Description = Xml.GetFirstElementsValueByTagName(nvrNode, "Description").Trim(),
			//            Enabled = Convert.ToInt32(Xml.GetFirstElementsValueByTagName(nvrNode, "Enabled").Trim()),

			//            Threshole1 = Convert.ToInt32(Xml.GetFirstElementsValueByTagName(nvrNode, "Threshold1").Trim()),
			//            Threshole2 = Convert.ToInt32(Xml.GetFirstElementsValueByTagName(nvrNode, "Threshold2").Trim()),
			//            MailTo1 = new List<string>(),
			//            MailTo2 = new List<string>(),
			//        };
			//        excep.ToMailTo1(Xml.GetFirstElementsValueByTagName(nvrNode, "ToMailTo1").Trim());
			//        excep.ToMailTo2(Xml.GetFirstElementsValueByTagName(nvrNode, "ToMailTo2").Trim());

			//        if (PosExceptions.ContainsKey(excep.Id)) continue;

			//        PosExceptions.Add(excep.Id, excep);
			//    }
			//}

			//_loadFlag = true;
		}

		private Boolean _saveFlag;
		private void SaveDatas()
		{
			//_saveFlag = false;

			//XmlDocument doc = ParseDataToXml();

			//if (doc != null)
			//    _pos.SaveEventExceptionToDatabase(doc);

			//_saveFlag = true;
		}

		private XmlDocument ParseDataToXml()
		{
			XmlDocument xmlDoc = new XmlDocument();

			XmlElement xmlRoot = xmlDoc.CreateElement("CMS");
			xmlDoc.AppendChild(xmlRoot);

			List<IPosException> dataSortResult = new List<IPosException>(PosExceptions.Values);
			dataSortResult.Sort((x, y) => (x.Id - y.Id));

			foreach (IPosException item in dataSortResult)
			{
				XmlElement node = xmlDoc.CreateElement("Exception");
				node.SetAttribute("id", item.Id.ToString());
				node.SetAttribute("name", item.Name);

				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Keyword", item.Keyword));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Description", item.Description));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Enabled", item.Enabled));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Threshold1", item.Threshole1.ToString()));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Threshold2", item.Threshole2.ToString()));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "MailTo1", item.MailTo1ToString()));
				node.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "MailTo2", item.MailTo2ToString()));

				xmlRoot.AppendChild(node);
			}

			return xmlDoc;
		}

		private delegate void LoadCallbackDelegate(IAsyncResult result);
		private delegate void LoadDelegate();
		private void LoadCallback(IAsyncResult result)
		{
			((LoadDelegate)result.AsyncState).EndInvoke(result);

			if (_loadFlag)
			{
				_watch.Stop();
				const String msg = "Pos Exception Ready";
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

			if (_saveFlag)
			{
				_watch.Stop();
				Console.WriteLine(@"Pos Exception Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

				Boolean hasNew = false;

				foreach (KeyValuePair<Int32, IPosException> obj in PosExceptions)
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

		public UInt16 GetNewId()
		{
			UInt16 max = (UInt16)65535;
			for (UInt16 id = 1; id <= max; id++)
			{
				if (PosExceptions.ContainsKey(id)) continue;
				return id;
			}

			return 0;
		}
	}
}
