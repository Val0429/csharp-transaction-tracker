using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using Constant;
using Device;
using Interface;

namespace ServerProfile
{
    public partial class POSManager : IPOSManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;
        public event EventHandler<EventArgs<POS_Exception.TransactionItem>> OnPOSLiveEventReceive;

        private const String CgiLoadAllPOS = @"cgi-bin/posconfig?action=loadallpos";
        private const String CgiSaveAllPOS = @"cgi-bin/posconfig?action=saveallpos";
        private const String CgiLoadPOSGroup = @"cgi-bin/posconfig?action=loadposgroup";
        private const String CgiSavePOSGroup = @"cgi-bin/posconfig?action=saveposgroup";

        private const String CgiPOSLiveEvent = @"cgi-bin/pos?type=raw";
        //private const String CgiSearchPOSEvent = @"cgi-bin/posinfo?channel=channel%1&action= searchEvent&starttime={START}&endtime={END}";

        public Dictionary<String, String> Localization;

        public ManagerReadyState ReadyStatus { get; set; }
        public IServer Server;

        private Boolean _listenPOSLiveEvent;
        private BackgroundWorker _listenPOSLiveEventBackgroundWorker;
        private readonly System.Timers.Timer _listenPOSTimer = new System.Timers.Timer();

        public List<IPOS> POS { get; private set; }
        public ScheduleReports ScheduleReports { get; protected set; }
        public List<POS_Exception.TemplateConfig> TemplateConfigs { get; protected set; }
        
        public POSManager()
        {
            Localization = new Dictionary<String, String>();
            Localizations.Update(Localization);

            Exceptions = new Dictionary<UInt16, POS_Exception>();
            ExceptionThreshold = new Dictionary<String, POS_Exception.ExceptionThreshold>();
            POS = new List<IPOS>();
            Connections = new Dictionary<UInt16, IPOSConnection>();
            ScheduleReports = new ScheduleReports();
            TemplateConfigs = new List<POS_Exception.TemplateConfig>();

            ReadyStatus = ManagerReadyState.New;
        }

        public void Initialize()
        {
            _listenPOSLiveEventBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _listenPOSLiveEventBackgroundWorker.DoWork += ListenPOSLiveEvent;

            _listenPOSTimer.Interval = 5000; //5 sec
            _listenPOSTimer.Elapsed += ListenPOSLiveEventTimer;
            _listenPOSTimer.SynchronizingObject = Server.Form; 
        }

        public String Status
        {
            get { return "POS : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private readonly Stopwatch _watch = new Stopwatch();
        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            _watch.Reset();
            _watch.Start();

            LoadExceptionDelegate loadExceptionDelegate = LoadException;
            loadExceptionDelegate.BeginInvoke(LoadExceptionCallback, loadExceptionDelegate);
        }

        public void Load(String xml)
        {
        }

        private delegate void LoadPOSDelegate();
        public void LoadPOS()
        {
            var pts = Server as IPTS;
            if (pts == null) return;

            //<AllPos>
            //<POSConfiguration id="1">
            //    <Name></Name>
            //    <POSRegisterID>3</POSRegisterID>
            //    <Manufacture>MaitreD</Manufacture>
            //    <Model></Model>
            //    <ExceptionConfig>1</ExceptionConfig>
            //    <KeywordConfig>1</KeywordConfig>
            //    <Items>1,2,3</Items>
            //</POSConfiguration>
            //</AllPos>

            POS.Clear();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPOS, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList configurationList = xmlDoc.GetElementsByTagName("POSConfiguration");
            foreach (XmlElement configurationNode in configurationList)
            {
                var pos = new POS
                              {
                                  LicenseId = Convert.ToUInt16(configurationNode.GetAttribute("id")),
                                  Name = Xml.GetFirstElementValueByTagName(configurationNode, "Name"),
                                  Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "POSRegisterID")),
                                  Manufacture = Xml.GetFirstElementValueByTagName(configurationNode, "Manufacture"),
                                  Exception = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "ExceptionConfig")),
                                  Keyword = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "KeywordConfig")),
                                  ReadyState = ReadyState.Ready
                              };

                String[] listId = Xml.GetFirstElementValueByTagName(configurationNode, "Items").Split(',');
                foreach (String str in listId)
                {
                    UInt16[] arr = str.Split(':').Select(s => String.Equals(s, "") ? (UInt16)0 : Convert.ToUInt16(s)).ToArray();
                    if (arr.Length != 2)
                        continue;

                    if (!pts.NVR.NVRs.ContainsKey(arr[0])) continue;

                    var device = new BasicDevice
                                     {
                                         Server = pts.NVR.NVRs[arr[0]],
                                         Id = arr[1]
                                     };

                    pos.Items.Add(device);
                }

                POS.Add(pos);
            }
        }
        
        private void LoadPOSCallback(IAsyncResult result)
        {
            LoadPOSConnectionDelegate loadPOSConnectionDelegate = LoadPOSConnection;
            loadPOSConnectionDelegate.BeginInvoke(LoadPOSConnectionCallback, loadPOSConnectionDelegate);
        }

        public void Save()
        {
            ReadyStatus = ManagerReadyState.Saving;

            _watch.Reset();
            _watch.Start();

            SaveDelegate saveDelegate = SaveSettings;
            saveDelegate.BeginInvoke(SaveCallback, saveDelegate);
        }

        public void Save(String xml)
        {
        }

        private void SaveSettings()
        {
            SaveException();
            SavePOS();
            SavePOSGroup();
            SaveConnection();
            SaveConnectionGroup();
            SaveScheduleReport();
            SaveExceptionReport();
        }

        private void SavePOS()
        {
            //var orgXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPOS, Server.Credential);

            //XmlNodeList orgPOSNodeList = null;
            //if (orgXmlDoc != null)
            //    orgPOSNodeList = orgXmlDoc.GetElementsByTagName("POSConfiguration");

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("AllPos");
            xmlDoc.AppendChild(xmlRoot);

            foreach (IPOS pos in POS)
            {
                if (pos.ReadyState == ReadyState.Ready) continue;

                var posNode = ParsePOSToXml(pos);

                //var isChange = true;
                //if (orgPOSNodeList != null && posNode.FirstChild != null)
                //{
                //    foreach (XmlElement orgPOSNode in orgPOSNodeList)
                //    {
                //        var id = Convert.ToUInt16(orgPOSNode.GetAttribute("id"));
                //        if (id == obj.Key)
                //        {
                //            if (orgPOSNode.InnerXml == posNode.FirstChild.InnerXml)
                //                isChange = false;
                //            break;
                //        }
                //    }
                //}

                if (posNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(posNode.FirstChild, true));

                pos.ReadyState = ReadyState.Ready;
            }

            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveAllPOS, xmlDoc, Server.Credential);
        }

        private void SavePOSGroup()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadPOSGroup, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IPOS>(POS);
            sortResult.Sort((x, y) => (x.LicenseId - y.LicenseId));

            IEnumerable<String> temp = sortResult.Select(pos => (pos.LicenseId != 0) ? pos.LicenseId.ToString() : "");

            var groupNode = xmlDoc.CreateElement("Group");
            groupNode.SetAttribute("id", "0");
            groupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", String.Join(",", temp.ToArray())));
            xmlRoot.AppendChild(groupNode);

            if (orangeXmlDoc != null && !String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                Xml.PostXmlToHttp(CgiSavePOSGroup, xmlDoc, Server.Credential);
        }

        private static XmlDocument ParsePOSToXml(IPOS pos)
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("POSConfiguration");
            xmlRoot.SetAttribute("id", pos.LicenseId.ToString());
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", pos.Name));//pos.Name
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "POSRegisterID", pos.Id));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Manufacture", pos.Manufacture));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Model", pos.Model));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ExceptionConfig", pos.Exception));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "KeywordConfig", pos.Keyword));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", ConvertIDeviceListToStringWithNVRId(pos.Items)));

            //-------------------------------------------------------------------------
            var scheduleNode = xmlDoc.CreateElement("Schedule");
            xmlRoot.AppendChild(scheduleNode);
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mon", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Tue", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Wed", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Thu", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Fri", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Sat", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Sun", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            //-------------------------------------------------------------------------
            
            return xmlDoc;
        }

        private static String ConvertIDeviceListToStringWithNVRId(List<IDevice> list)
        {
            list.Sort(SortByIdThenNVR);
            IEnumerable<String> temp = list.Select(device => (device.Id != 0) ? (device.Server.Id + ":" + device.Id) : "");

            return String.Join(",", temp.ToArray());
        }

        private static Int32 SortByIdThenNVR(IDevice x, IDevice y)
        {
            if (x.Id != y.Id)
                return (x.Id - y.Id);

            return (x.Server.Id - y.Server.Id);
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            _watch.Stop();
            Console.WriteLine(@"POS Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));
            ReadyStatus = ManagerReadyState.Ready;

            if (OnSaveComplete != null)
                OnSaveComplete(this, null);
        }

        public void StartListenPOSLiveEvent()
        {
            //nobody is listening, dont receive
            if (OnPOSLiveEventReceive == null) return;

            _listenPOSLiveEvent = true;
            if (!_listenPOSLiveEventBackgroundWorker.IsBusy)
                _listenPOSLiveEventBackgroundWorker.RunWorkerAsync();
        }

        public void StopListenPOSLiveEvent()
        {
            _listenPOSLiveEvent = false;

            _listenPOSLiveEventBackgroundWorker.CancelAsync();
            _listenPOSLiveEventBackgroundWorker.Dispose();
            _listenPOSLiveEventBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _listenPOSLiveEventBackgroundWorker.DoWork += ListenPOSLiveEvent;
        }

        private void ListenPOSLiveEventTimer(Object sender, EventArgs e)
        {
            _listenPOSTimer.Enabled = false;
            StartListenPOSLiveEvent();
        }

        private void ListenPOSLiveEvent(Object sender, DoWorkEventArgs e)
        {
            if (!_listenPOSLiveEvent) return;

            HttpWebRequest request = Xml.GetHttpRequest(CgiPOSLiveEvent, Server.Credential);
            request.Method = "GET";

            var buffer = new byte[2000];

            WebResponse response;
            Stream stream;
            try
            {
                response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch (Exception)
            {
                StopListenPOSLiveEvent();
                _listenPOSTimer.Enabled = true;
                return;
            }

            String text;
            try
            {
                if (stream == null)
                {
                    response.Close();
                    StopListenPOSLiveEvent();
                    //can't connect, wait 5 sec, connect again
                    _listenPOSTimer.Enabled = true;
                    return;
                }

                Int32 read;
                while (_listenPOSLiveEvent && stream.CanRead && (read = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    var reader = new StreamReader(new MemoryStream(buffer, 0, read));
                    text = reader.ReadToEnd();
                    if (!text.StartsWith("<Transaction>")) continue;

                    //<Transaction>
                    //    <Info>
                    //        <ChannelID>1</ChannelID>
                    //        <POSID>1</POSID>
                    //        <StartTime>1356499701535</StartTime>
                    //        <EndTime>1356499701535</EndTime>
                    //        <ConnectionType>201</ConnectionType>
                    //        <DataType>201</DataType>
                    //    </Info>
                    //    <Content>01,--Begin--`FRI DECEMBER 14,2012 - 16:43:45`Peter Frampton - 1</Content>
                    //</Transaction>

                    if (OnPOSLiveEventReceive != null)
                    {
                        var end = text.IndexOf("</Transaction>");
                        text = text.Substring(0, end + "</Transaction>".Length);
                        XmlDocument xmlDoc = Xml.LoadXml(text);
                        var posIdStr = Xml.GetFirstElementValueByTagName(xmlDoc, "POSID");
                        if (String.IsNullOrEmpty(posIdStr)) continue;
                        var posId = Convert.ToUInt16(posIdStr);
                        
                        if (FindPOSById(posId) == null) continue;

                        var dateTime = Xml.GetFirstElementValueByTagName(xmlDoc, "StartTime");
                        if (String.IsNullOrEmpty(dateTime)) continue;

                        var rows = Xml.GetFirstElementValueByTagName(xmlDoc, "Content").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        for (var i = 0; i < rows.Length; i++)
                        {
                            rows[i] = rows[i].Replace("\t", "    ");
                        }
                        
                        foreach (var row in rows)
                        {
                            if (String.IsNullOrEmpty(row)) continue;

                            OnPOSLiveEventReceive(this, new EventArgs<POS_Exception.TransactionItem>(new POS_Exception.TransactionItem
                            {
                                POS = posId,
                                DateTime = DateTimes.ToDateTime(Convert.ToUInt64(dateTime), Server.Server.TimeZone),
                                Content = row,
                            }));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Write("-------------------------------Live POS Event Exception-----------------------");
                Log.Write(exception.ToString());
                Log.Write("Get Live POS Event Fail, Restart");
                if (stream != null)
                    stream.Close();
                response.Close();
                StopListenPOSLiveEvent();
                StartListenPOSLiveEvent();
                return;
            }

            stream.Close();
            response.Close();
            StopListenPOSLiveEvent();
            //can't connect, wait 5 sec, connect again
            _listenPOSTimer.Enabled = true;
        }

        public IPOS FindPOSById(UInt16 posId)
        {
            foreach (var pos in POS)
            {
                if (pos.Id == posId)
                {
                    return pos;
                }
            }
            return null;
        }

        private IPOS FindPOSByLicenseId(UInt16 licenseId)
        {
            foreach (var pos in POS)
            {
                if (pos.LicenseId == licenseId)
                {
                    return pos;
                }
            }
            return null;
        }
        
        public UInt16 GetNewPOSId()
        {
            for (UInt16 id = 1; id <= 65535; id++)
            {
                if (FindPOSById(id) != null) continue;

                return id;
            }

            return 0;
        }

        public UInt16 GetNewPOSLicenseId()
        {
            UInt16 max = Server.License.Amount;
            for (UInt16 id = 1; id <= max; id++)
            {
                if (FindPOSByLicenseId(id) != null) continue;
                return id;
            }

            return 0;
        }
        
        //call by reference
        private void CheckDateTimeDontGoToFar(ref UInt64 startutc, ref UInt64 endutc)
        {
            if (startutc > endutc)
            {
                var temp = startutc;
                startutc = endutc;
                endutc = temp;
            }

            var utcnow = DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone);
            startutc = Math.Min(startutc, utcnow);
            endutc = Math.Min(endutc, utcnow);
        }

        //public UInt16 GetNewKeywordId()
        //{
        //    //UInt16 max = 65535;
        //    //for (UInt16 id = 1; id <= max; id++)
        //    //{
        //    //    if (Keywords.ContainsKey(id)) continue;
        //    //    return id;
        //    //}

        //    return 0;
        //}
    }
}
