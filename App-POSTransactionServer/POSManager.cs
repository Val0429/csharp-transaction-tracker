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

namespace App_POSTransactionServer
{
    public partial class POSManager : IPOSManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;
        public event EventHandler<EventArgs<POS_Exception.TransactionItem>> OnPOSLiveEventReceive;

        
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

        public List<IPOS> POSServer { get; }

        public Dictionary<UInt16, IDivision> DivisionManager { get; }
        public Dictionary<UInt16, IRegion> RegionManager { get; }
        public Dictionary<UInt16, IStore> StoreManager { get; }

        public ScheduleReports ScheduleReports { get; protected set; }
        public List<POS_Exception.TemplateConfig> TemplateConfigs { get; protected set; }
        
        public POSManager()
        {
            Localization = new Dictionary<String, String>();
            Localizations.Update(Localization);

            Exceptions = new Dictionary<UInt16, POS_Exception>();
            GenericPOSSetting = new Dictionary<UInt16, IPOSConnection>();
            ExceptionThreshold = new Dictionary<String, POS_Exception.ExceptionThreshold>();
            POSServer = new List<IPOS>();
            DivisionManager = new Dictionary<UInt16, IDivision>();
            RegionManager = new Dictionary<UInt16, IRegion>();
            StoreManager = new Dictionary<UInt16, IStore>();
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
           // _listenPOSTimer.SynchronizingObject = Server.Form; 
        }

        public String Status
        {
            get { return "POS : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private readonly Stopwatch _watch = new Stopwatch();
        public void Load(String xml)
        {
        }

        private delegate void LoadExceptionDelegate();
        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            _watch.Reset();
            _watch.Start();

            LoadExceptionDelegate loadExceptionDelegate = LoadException;
            loadExceptionDelegate.BeginInvoke(LoadExceptionCallback, loadExceptionDelegate);
        }

        private delegate void LoadExceptionGroupDelegate();
        private void LoadExceptionCallback(IAsyncResult result)
        {
            LoadExceptionGroupDelegate loadExceptionGroupDelegate = LoadExceptionGroup;
            loadExceptionGroupDelegate.BeginInvoke(LoadExceptionGroupCallback, loadExceptionGroupDelegate);
        }

        private delegate void LoadPOSDelegate();
        private void LoadExceptionGroupCallback(IAsyncResult result)
        {
            LoadPOSDelegate loadPOSDelegate = LoadPOS;
            loadPOSDelegate.BeginInvoke(LoadPOSCallback, loadPOSDelegate);
        }

        private delegate void LoadPOSConnectionDelegate();
        private void LoadPOSCallback(IAsyncResult result)
        {
            LoadPOSConnectionDelegate loadPOSConnectionDelegate = LoadPOSConnection;
            loadPOSConnectionDelegate.BeginInvoke(LoadPOSConnectionCallback, loadPOSConnectionDelegate);
        }

        private delegate void LoadStoreDelegate();
        private void LoadPOSConnectionCallback(IAsyncResult result)
        {
            LoadStoreDelegate loadStoreDelegate = LoadStore;
            loadStoreDelegate.BeginInvoke(LoadStoreCallback, loadStoreDelegate);
        }

        private delegate void LoadRegionDelegate();
        private void LoadStoreCallback(IAsyncResult result)
        {
            LoadRegionDelegate loadRegionDelegate = LoadRegion;
            loadRegionDelegate.BeginInvoke(LoadRegionCallback, loadRegionDelegate);
        }

        private delegate void LoadDivisionDelegate();
        private void LoadRegionCallback(IAsyncResult result)
        {
            LoadDivisionDelegate loadDivisionDelegate = LoadDivision;
            loadDivisionDelegate.BeginInvoke(LoadDivisionCallback, loadDivisionDelegate);
        }

        private delegate void LoadScheduleReportDelegate();
        private void LoadDivisionCallback(IAsyncResult result)
        {
            LoadScheduleReportDelegate loadScheduleReportDelegate = LoadScheduleReport;
            loadScheduleReportDelegate.BeginInvoke(LoadScheduleReportCallback, loadScheduleReportDelegate);
        }

        private delegate void LoadExceptionReportDelegate();
        private void LoadScheduleReportCallback(IAsyncResult result)
        {
            LoadExceptionReportDelegate loadExceptionReportDelegate = LoadExceptionReport;
            loadExceptionReportDelegate.BeginInvoke(LoadExceptionReportCallback, loadExceptionReportDelegate);
        }

        private delegate void LoadTemplateDelegate();
        private void LoadExceptionReportCallback(IAsyncResult result)
        {
            LoadTemplateDelegate loadTemplateDelegate = LoadTemplate;
            loadTemplateDelegate.BeginInvoke(LoadTemplateCallback, loadTemplateDelegate);
        }

        private void LoadTemplateCallback(IAsyncResult result)
        {
            ((LoadTemplateDelegate)result.AsyncState).EndInvoke(result);

            _watch.Stop();
            //const String msg = "POS Ready";
            //Console.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            ReadyStatus = ManagerReadyState.Ready;

            if (OnLoadComplete != null)
                OnLoadComplete(this, null);
        }

        public Boolean UsePTSId(String posId)
        {
            var pts = Server as IPTS;
            if (pts == null) return false;

            foreach (IPOS pos in POSServer)
            {
                if (pos.Id == posId)
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in Exceptions)
                    {
                        if (posException.Key == pos.Exception)
                        {
                            return !posException.Value.IsSupportPOSId;
                        }
                    }

                    return false;
                }
            }

            return false;
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
            //for generic pos connection
            SaveDelegate saveDelegate = SaveGenericSettings;
            saveDelegate.BeginInvoke(SaveCallback, saveDelegate);
        }

        private void SaveSettings()
        {
            SaveException();
            SaveDivision();
            SaveRegion();
            SaveStore();
            SavePOS();
            SavePOSGroup();
            SaveConnection();
            SaveConnectionGroup();
            SaveScheduleReport();
            SaveExceptionReport();
        }

        private void SaveGenericSettings()
        {
            SaveException();
            SaveConnection();
            SaveConnectionGroup();
        }

        

        private void SavePOSGroup()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadPOSGroup, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IPOS>(POSServer);
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
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsCaputure", pos.IsCapture ? "true" : "false"));
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

            var request = Xml.GetHttpRequest(CgiPOSLiveEvent, Server.Credential);
            request.Method = "GET";
            
            var buffer = new byte[2000];

            WebResponse response;
            Stream stream;
            try
            {
                response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch (Exception exception)
            {
                Log.Write("-----error1- " + exception.Message);
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

                    //if (!text.StartsWith("<Transaction>")) continue;
                    if (text.IndexOf("<Transaction>") < 0) continue;
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
                        var start = text.IndexOf("<Transaction>");
                        var end = text.IndexOf("</Transaction>");
                        text = text.Substring(start, (end + "</Transaction>".Length) - start);
                        XmlDocument xmlDoc = Xml.LoadXml(text);
                        var posIdStr = Xml.GetFirstElementValueByTagName(xmlDoc, "POSID");
                        if (String.IsNullOrEmpty(posIdStr)) continue;
                        var posId = posIdStr;

                        if (posId != "PTSDemo" && posId != "PTS")
                        {
                            if (FindPOSById(posId) == null) continue;
                        }

                        var dateTime = Xml.GetFirstElementValueByTagName(xmlDoc, "StartTime");
                        if (String.IsNullOrEmpty(dateTime)) continue;

                        if(posId == "PTSDemo")
                        {
                            var rows = Xml.GetFirstElementValueByTagName(xmlDoc, "Content");

                            if (String.IsNullOrEmpty(rows)) continue;

                            OnPOSLiveEventReceive(this, new EventArgs<POS_Exception.TransactionItem>(new POS_Exception.TransactionItem
                            {
                                POS = posId,
                                DateTime = DateTimes.ToDateTime(Convert.ToUInt64(dateTime), Server.Server.TimeZone),
                                Content = rows,
                            }));
                        }
                        else
                        {
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

        public IStore FindStoreById(Int32 Id)
        {
            foreach (var store in StoreManager.Values )
            {
                if (store.Id == Id)
                {
                    return store;
                }
            }

            return null;
        }

        public IPOS FindPOSById(String posId)
        {
            if(posId == "PTS")
            {
                foreach (IPOS pos in POSServer)
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in Exceptions)
                    {
                        if (posException.Key == pos.Exception)
                        {
                            if(!posException.Value.IsSupportPOSId)
                            {
                                return pos;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var pos in POSServer)
                {
                    if (pos.Id == posId)
                    {
                        return pos;
                    }
                }
            }
            
            return null;
        }

        private IPOS FindPOSByLicenseId(UInt16 licenseId)
        {
            foreach (var pos in POSServer)
            {
                if (pos.LicenseId == licenseId)
                {
                    return pos;
                }
            }
            return null;
        }
        
        public String GetNewPOSId()
        {
            for (UInt16 id = 1; id <= 65535; id++)
            {
                if (FindPOSById(id.ToString()) != null) continue;

                return id.ToString();
            }

            return "0";
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

        public UInt16 GetNewDivisionId()
        {
            UInt16 max = 65535;
            for (UInt16 id = 1; id <= max; id++)
            {
                if (!DivisionManager.ContainsKey(id))
                return id;
            }

            return 0;
        }

        public UInt16 GetNewRegionId()
        {
            UInt16 max = 65535;
            for (UInt16 id = 1; id <= max; id++)
            {
                if (!RegionManager.ContainsKey(id))
                    return id;
            }

            return 0;
        }

        public UInt16 GetNewStoreId()
        {
            UInt16 max = 65535;
            for (UInt16 id = 1; id <= max; id++)
            {
                if (!StoreManager.ContainsKey(id))
                    return id;
            }

            return 0;
        }
    }
}
