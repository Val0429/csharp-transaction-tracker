using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Constant;
using DeviceCab;
using DeviceConstant;
using Interface;
using Report;

namespace Device
{
    public class Camera : BasicDevice, ICamera
    {
        private static readonly ILogger Logger = LoggerManager.Instance.GetLogger();
        public event EventHandler<EventArgs<UInt16, ExportVideoStatus>> OnExportVideoProgress;
        public event EventHandler OnExportVideoComplete;

        public event EventHandler<EventArgs<String>> OnSmartSearchResult;
        public event EventHandler OnSmartSearchComplete;

        private const String EventCount = "200";
        private const String CgiSearchEvent = @"cgi-bin/playbackinfo?channel=channel%1&action=searchEvent&starttime={START}&endtime={END}&eventtype={TYPE}&count={COUNT}";
        private const String CgiGetParts = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=record";
        private const String CgiGetTrack2Parts = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=record&trackid=2";
        private const String CgiGetPlaybackDownloadParts = @"cgi-bin/playbackinfo?channel=channel%1&action=getPartsBySession&part={PART}&starttime={START}&endtime={END}&type=record&nvr=nvr{NVRID}&session={SESSION}";
        private const String CgiGetAudioInParts = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=audio1";
        private const String CgiGetAudioOutParts = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=audio2";
        private const String CgiGetEventParts = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=event&eventtype={EVENT}";
        //private const String CgiGetNextPreviousTime = @"cgi-bin/playbackinfo?channel=channel%1&action=getNextPreviousTime&currenttime={NOW}&type=Record";
        private const String CgiGetNextTime = @"cgi-bin/playbackinfo?channel=channel%1&action=getNextTime&currenttime={NOW}&type=Record";
        private const String CgiGetPreviousTime = @"cgi-bin/playbackinfo?channel=channel%1&action=getPreviousTime&currenttime={NOW}&type=Record";
        private const String CgiDigitalOutput = @"cgi-bin/sendcommand/digitaloutput?channel=channel%1&cmdDigitalOutput=%2";
        private const String CgiSnapshot = @"cgi-bin/snapshot?channel=channel%1";
        private const String CgiSnapshotWithSize = @"cgi-bin/snapshot?channel=channel%1&width={WIDTH}&height={HEIGHT}";
        private const String CgiSnapshotWithTimecode = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}";
        private const String CgiSnapshotWithTimecodeAndSize = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}&width={WIDTH}&height={HEIGHT}";
        private const String CgiManualRecord = @"cgi-bin/sendcommand/manualrecord?channel=channel%1&duration={DURATION}";
        private const String CgiPanic = @"cgi-bin/sendcommand/panic?channel=channel%1";
        private const String CgiSmartSearch = @"cgi-bin/smartsearch?channel=channel%1";
        private const String CgiSendCommand = @"cgi-bin/sendcommand?channel=channel%1";
        private const String CgiRequeryImage = @"cgi-bin/requireimage?channel=channel%1";
        private const String CgiReadEventByCondition = @"cgi-bin/clidb?method=ReadEventByCondition";
        private const String CgiReadEventPartByCondition = @"cgi-bin/clidb?method=ReadEventPartByCondition";
        private const String CgiUserDefine = @"cgi-bin/userdef?channel=channel%1&event=event%2&data=%3";

        private const String CgiSearchEventWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&nvr=nvr{NVRID}&action=searchEvent&starttime={START}&endtime={END}&eventtype={TYPE}&count={COUNT}";
        private const String CgiGetPartsWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=record&nvr=nvr{NVRID}";
        private const String CgiGetArchiveServerPartsWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=record&nvr=nvr{NVRID}&archive=1";
        private const String CgiGetTrack2PartsWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=record&nvr=nvr{NVRID}&trackid=2";
        private const String CgiGetAudioInPartsWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=audio1&nvr=nvr{NVRID}";
        private const String CgiGetAudioOutPartsWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getParts&part={PART}&starttime={START}&endtime={END}&type=audio2&nvr=nvr{NVRID}";
        private const String CgiGetNextTimeWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getNextTime&currenttime={NOW}&type=Record&nvr=nvr{NVRID}";
        private const String CgiGetArchiveServerNextTimeWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getNextTime&currenttime={NOW}&type=Record&nvr=nvr{NVRID}&archive=1";
        private const String CgiGetPreviousTimeWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getPreviousTime&currenttime={NOW}&type=Record&nvr=nvr{NVRID}";
        private const String CgiGetArchiveServerPreviousTimeWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getPreviousTime&currenttime={NOW}&type=Record&nvr=nvr{NVRID}&archive=1";
        private const String CgiGetArchiveRecordEndTimeWithNVR = @"cgi-bin/playbackinfo?channel=channel%1&action=getPreviousTime&currenttime={NOW}&type=Record&nvr=nvr{NVRID}&archive=1";
        private const String CgiDigitalOutputWithNVR = @"cgi-bin/sendcommand/digitaloutput?channel=channel%1&cmdDigitalOutput=%2&nvr=nvr{NVRID}";
        private const String CgiSnapshotWithNVR = @"cgi-bin/snapshot?channel=channel%1&nvr=nvr{NVRID}";
        private const String CgiSnapshotWithSizeWithNVR = @"cgi-bin/snapshot?channel=channel%1&width={WIDTH}&height={HEIGHT}&nvr=nvr{NVRID}";
        private const String CgiSnapshotWithTimecodeWithNVR = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}&nvr=nvr{NVRID}";
        private const String CgiSnapshotWithTimecodeAndSizeWithNVR = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}&width={WIDTH}&height={HEIGHT}&nvr=nvr{NVRID}";
        private const String CgiManualRecordWithNVR = @"cgi-bin/sendcommand/manualrecord?channel=channel%1&duration={DURATION}&nvr=nvr{NVRID}";
        private const String CgiPanicWithNVR = @"cgi-bin/sendcommand/panic?channel=channel%1&nvr=nvr{NVRID}";
        private const String CgiSmartSearchWithNVR = @"cgi-bin/smartsearch?channel=channel%1&nvr=nvr{NVRID}";
        private const String CgiSendCommandWithNVR = @"cgi-bin/sendcommand?channel=channel%1&nvr=nvr{NVRID}";
        private const String CgiRequeryImageWithNVR = @"cgi-bin/requireimage?channel=channel%1&nvr=nvr{NVRID}";
        //cmdAddPTZPreset
        //cmdDelPTZPreset
        //<CmdParam> <Param id="Entry" prefix="PTZ_PRESET_SET=" end="64" start="1" default="1" type="int"/> <Param id="State" prefix="," end="1" start="0" default="1" type="int"/> <Param id="PosX" prefix="," end="65535" start="0" default="65535" type="int"/> <Param id="PosY" prefix="," end="65535" start="0" default="65535" type="int"/> <Param id="PosZ" prefix="," end="65535" start="0" default="65535" type="int"/> <Param id="SpeedX" prefix="," end="5" start="1" default="3" type="int"/> <Param id="SpeedY" prefix="," end="5" start="1" default="3" type="int"/> <Param id="SpeedZ" prefix="," end="7" start="2" default="5" type="int"/> <Param id="DWell" prefix="," end="100" start="1" default="10" type="int"/> <Param id="Name" prefix="," end="" start="" default="" type="string"/> </CmdParam>

        private Boolean _isManualRecord;
        public XmlElement XmlFromServer { get; set; }
        public CameraStatus Status { get; set; }
        public Boolean IsAudioOut { get; set; }
        public Boolean IsManualRecord
        {
            get
            {
                return (_isManualRecord && Status == CameraStatus.Recording);
            }
            set
            {
                _isManualRecord = value;
                if (value && Server != null && Server.Configure != null)
                {
                    _manualRecordTimer.Interval = Server.Configure.ManualRecordDuration * 1000;
                    _manualRecordTimer.Enabled = true;
                }
            }
        }
        public Boolean IsInUse { get; set; } //Search result'IP already in Devices's List = true

        public CameraProfile Profile { get; set; }
        public UInt32 PreRecord { get; set; }
        public UInt32 PostRecord { get; set; }
        public Schedule RecordSchedule { get; set; }
        public Schedule EventSchedule { get; set; }
        public EventHandling EventHandling { get; set; }
        public String PlaybackSessionId { get; set; }
        //Preset
        private UInt16 _presetPoint;// = 0;
        public UInt16 PresetPointGo
        {
            get { return _presetPoint; }
            set
            {
                _presetPoint = value;

                SendPresetPointCommandDelegate sendSaveCommandDelegate = SendPresetPointCommand;
                sendSaveCommandDelegate.BeginInvoke("cmdPTZPresetGO=" + _presetPoint, null, null);

                //Xml.PostTextToHttp(CgiSendCommand.Replace("%1", Id.ToString()), "cmdPTZPresetGO=" + _presetPoint, Server.Credential, SendcommandTimeout);
            }
        }

        public UInt16 PresetTourGo { get; set; }
        public PresetPoints PresetPoints { get; set; }
        public PresetTours PresetTours { get; set; }
        public Boolean IsLoadPresetPoint { get; set; }
        public void AddPresetPoint(UInt16 id)
        {
            SendPresetPointCommandDelegate sendSaveCommandDelegate = SendPresetPointCommand;
            sendSaveCommandDelegate.BeginInvoke("cmdAddPTZPreset=" + id, null, null);
        }

        public void DeletePresetPoint(UInt16 id)
        {
            SendPresetPointCommandDelegate sendSaveCommandDelegate = SendPresetPointCommand;
            sendSaveCommandDelegate.BeginInvoke("cmdDelPTZPreset=" + id, null, null);
        }

        private delegate void SendPresetPointCommandDelegate(String cmd);
        private void SendPresetPointCommand(String cmd)
        {
            //change to PTZOperation.cs
            //if (CMS != null)
            //{
            //    Xml.PostTextToHttp(CgiSendCommandWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), cmd, CMS.Credential, SendcommandTimeout);
            //    return;
            //}
            Xml.PostTextToHttp(CgiSendCommand.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)), cmd, Server.Credential, SendcommandTimeout);
        }

        public Dictionary<UInt16, Boolean> DigitalOutputStatus { get; set; }

        public List<Bookmark> Bookmarks { get; set; }

        //Motion
        public Dictionary<UInt16, Rectangle> MotionRectangles { get; set; }
        public Dictionary<UInt16, UInt16> MotionThreshold { get; set; }

        //VAS
        public List<PeopleCountingRectangle> Rectangles { get; set; }
        public NetworkCredential Dispatcher { get; set; }
        public PeopleCountingSetting PeopleCountingSetting { get; set; }

        //IOPort
        public Dictionary<UInt16, IOPort> IOPort { get; set; } //Editable value

        public CameraType Type { get; set; }
        public CameraMode Mode { get; set; }

        private const UInt16 SendcommandTimeout = 30;//30 sec

        private readonly System.Timers.Timer _manualRecordTimer = new System.Timers.Timer();
        
        public ICMS CMS { get; set; }
        public List<Record> ArchiveServerRecord { get; set; }
        public Boolean IsArchiveRecord(UInt64 now)
        {
            if (CMS == null) return false;
            if (!CMS.Configure.WithArchiveServer) return false;

            var latestDownloadRecords = (from record in ArchiveServerRecord
                                         where
                                            record.Type == EventType.ArchiveServerRecord && record.StartDateTime <= DateTimes.ToDateTime(now, CMS.Server.TimeZone) && record.EndDateTime >= DateTimes.ToDateTime(now, CMS.Server.TimeZone)
                                         select record);
            return latestDownloadRecords.Count() > 0;
            return ArchiveRecordEntUTC > now;
        }

        public ICamera PIPDevice { get; set; }
        public UInt16 PIPStreamId { get; set; }
        public Position PIPPosition { get; set; }
        public UInt16 PatrolInterval { get; set; }
        public Dictionary<UInt16, WindowPTZRegionLayout> PatrolPoints { get; set; }
        // Constructor
        public Camera()
        {
            MaximumSearchResult = 0;// 600; 0 -> no limit
            PresetTourGo = 0;
            //DescBookmarks = 
            Bookmarks = new List<Bookmark>();
            DigitalOutputStatus = new Dictionary<UInt16, Boolean>
			{
				{1, false}, 
				{2, false},
				{3, false},
				{4, false},
				{5, false}, 
				{6, false},
				{7, false},
				{8, false}
			};
            PresetPoints = new PresetPoints();
            PresetTours = new PresetTours();
            IsLoadPresetPoint = false;

            PreRecord = 5000; //5 secs
            PostRecord = 30000; //30 secs
            ReadyState = ReadyState.New;
            Status = CameraStatus.Nosignal;
            Type = CameraType.Single;
            Mode = CameraMode.Single;
            IsAudioOut = false;
            IsInUse = false;

            _manualRecordTimer.Elapsed += ChangeManualRecordStatus;

            MotionRectangles = new Dictionary<UInt16, Rectangle>();
            MotionThreshold = new Dictionary<UInt16, UInt16> { { 1, 50 }, { 2, 50 }, { 3, 50 } };

            Rectangles = new List<PeopleCountingRectangle>();
            Dispatcher = new NetworkCredential();
            PeopleCountingSetting = new PeopleCountingSetting();

            IOPort = new Dictionary<UInt16, IOPort>();

            DeviceType = DeviceType.Device;

            ArchiveServerRecord = new List<Record>();
            PIPDevice = null;
            PIPStreamId = 1;
            PIPPosition = Position.RightTop;

            PatrolInterval = 5;
            PatrolPoints = new Dictionary<UInt16, WindowPTZRegionLayout>();
            for (int i = 1; i <= 16; i++)
            {
                PatrolPoints.Add((ushort) i, null);
            }
        }

        private void ChangeManualRecordStatus(Object sender, EventArgs e)
        {
            _manualRecordTimer.Enabled = false;
            _isManualRecord = false;
        }

        public StreamConfig StreamConfig
        {
            get
            {
                return Profile.StreamConfigs.ContainsKey(Profile.StreamId) ? Profile.StreamConfigs[Profile.StreamId] : null;
            }
        }

        private CameraModel _model;
        public CameraModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                //those SetDefault will change data that load from CGI
                //ProfileChecker.SetDefaultAccountPassword(this, _model);
                //ProfileChecker.SetDefaultProtocol(this, _model);
                //ProfileChecker.SetDefaultCompression(this, _model);
                //ProfileChecker.SetDefaultPort(this, _model);

                if (CMS != null)
                    return;

                if(Model.Type == "fisheye")
                {
                    ProfileChecker.SetDefaultCompression(this, _model);
                }

                ProfileChecker.CheckAvailableSetDefaultResolution(this, _model);
                ProfileChecker.CheckAvailableSetDefaultBitrate(this, _model);
                ProfileChecker.CheckAvailableSetDefaultFramerate(this, _model);

                if (EventHandling != null)
                {
                    EventHandling.SetEventHandlingViaCameraModel(_model);
                }

                if (!_model.IsSupportPTZ)
                {
                    if (PresetPoints.Count > 0 || PresetTours.Count > 0)
                    {
                        PresetPoints.Clear();
                        PresetTours.Clear();
                        PresetPointGo = 0;
                        PresetTourGo = 0;

                        if (Server is INVR)
                        {
                            ((INVR)Server).DeletePresetPointRelativeEventHandle(this, 0);
                        }
                    }
                }
            }
        }

        private Int32 GetDeviceId()
        {
            var id = -1;

            var layout = this as IDeviceLayout;
            if (layout != null)
            {
                foreach (var device in layout.Items.Where(device => device != null))
                {
                    id = device.Id;
                    break;
                }

                return id;
            }

            if (this is ISubLayout)
            {
                var subLayout = this as ISubLayout;
                if (subLayout == null) return -1;

                id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                return id;
            }

            //normal device
            return Id;
        }

        private void ParseRecord(Queue<Record> parts, XmlDocument xmlDoc, UInt64 splitStart, float time, EventType type)
        {
            if (xmlDoc == null || xmlDoc.InnerXml == "") return;

            var records = ParseRecord(xmlDoc, splitStart, time, type);
            foreach (var record in records)
            {
                parts.Enqueue(record);
            }
        }

        protected IEnumerable<Record> ParseRecord(XmlDocument xmlDoc, UInt64 splitStart, float time, EventType type)
        {
            var records = new List<Record>();

            // <Parts status="500">No Event.</Parts>
            if (xmlDoc == null || xmlDoc.InnerXml == "" ||
                xmlDoc.GetElementsByTagName("Parts").OfType<XmlNode>().Any(p => p.Attributes != null && p.Attributes["status"] != null && p.Attributes["status"].Value == "500"))
            {
                return records;
            }

            var partsArray = Array.ConvertAll(Xml.GetFirstElementValueByTagName(xmlDoc, "Parts").Split('-'), Convert.ToUInt16);

            if (partsArray.Length > 0)
            {
                UInt16 left = 0;
                Boolean isRecord = (partsArray[0] == 1);
                for (int i = 1; i < partsArray.Length; i++)
                {
                    if (isRecord)
                    {
                        try
                        {
                            var record = new Record
                            {
                                Type = type,
                                StartDateTime = DateTimes.ToDateTime(splitStart + Convert.ToUInt64(left * time), Server.Server.TimeZone),
                                EndDateTime = DateTimes.ToDateTime(splitStart + Convert.ToUInt64((left + partsArray[i]) * time), Server.Server.TimeZone),
                            };
                            records.Add(record);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    left += partsArray[i];
                    isRecord = !isRecord;
                }
            }

            return records;
        }

        //max end - start length is 15 Hrs,  more than 14 hrs, use multi queue and merge result
        public Queue<Record> GetParts(UInt16 part, UInt64 start, UInt64 end)
        {
            var parts = new Queue<Record>();

            var id = GetDeviceId();

            if (id == -1)
                return parts;

            start = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Floor(ConvertMillisecondsToSeconds(start))));
            end = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Ceiling(ConvertMillisecondsToSeconds(end))));

            //like... 20 sec but 500 part -> only need 20 part. 1 part for 1 sec.
            if (part > ConvertMillisecondsToSeconds(end - start))
                part = Convert.ToUInt16(ConvertMillisecondsToSeconds(end - start));

            var section = Math.Ceiling(((end - start) / (3600000.0)) / 14.0); // check if periods more than 14 hours, and can split by 14 hours per section.

            UInt64 diff = Convert.ToUInt64(Math.Round((end - start) / section));
            var splitPart = Math.Round(part / section);
            var splitStart = start;
            var splitEnd = splitStart + diff;

            for (var index = 1; index <= section; index++)
            {
                if (index > 1)
                {
                    splitStart += diff;
                    splitEnd = splitStart + diff;
                }

                var time = (splitEnd - splitStart) / (float)splitPart;

                XmlDocument xmlDoc;
                XmlDocument xmlDocTrack2;
                if (CMS != null)
                {
                    //if (CMS.Configure.WithArchiveServer)
                        //GetArchiveRecordEndTime(DateTimes.UtcNow);

                    String url = CgiGetPartsWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                    xmlDoc = Xml.LoadXmlFromHttp(url, CMS.Credential);

                    String track2Url = CgiGetArchiveServerPartsWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));

                    if(CMS.Configure.WithArchiveServer)
                        xmlDocTrack2 = Xml.LoadXmlFromHttp(track2Url, CMS.Credential);
                    else
                        xmlDocTrack2 = new XmlDocument();
                }
                else
                {
                    String url = CgiGetParts.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture));
                    xmlDoc = Xml.LoadXmlFromHttp(url, Server.Credential);

                    String track2Url = CgiGetTrack2Parts.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                    .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture));
                    xmlDocTrack2 = Xml.LoadXmlFromHttp(track2Url, Server.Credential);
                }
                ParseRecord(parts, xmlDoc, splitStart, time, EventType.VideoRecord);

                if(CMS != null)
                {
                    ParseRecord(parts, xmlDocTrack2, splitStart, time, EventType.ArchiveServerRecord);
                }
                else if(Server is INVR)
                {
                    ParseRecord(parts, xmlDocTrack2, splitStart, time, EventType.SDRecord);
                }

            }

            return parts;
        }

        public Queue<Record> GetArchiveServerRecordPart(UInt16 part, UInt64 start, UInt64 end)
        {
            var parts = new Queue<Record>();

            if (CMS == null)
            {
                return parts;
            }

            var id = GetDeviceId();

            if (id == -1)
                return parts;

            start = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Floor(ConvertMillisecondsToSeconds(start))));
            end = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Ceiling(ConvertMillisecondsToSeconds(end))));

            //like... 20 sec but 500 part -> only need 20 part. 1 part for 1 sec.
            if (part > ConvertMillisecondsToSeconds(end - start))
                part = Convert.ToUInt16(ConvertMillisecondsToSeconds(end - start));

            var section = Math.Ceiling(((end - start) / (3600000.0)) / 14.0); // check if periods more than 14 hours, and can split by 14 hours per section.

            UInt64 diff = Convert.ToUInt64(Math.Round((end - start) / section));
            var splitPart = Math.Round(part / section);
            var splitStart = start;
            var splitEnd = splitStart + diff;

            for (var index = 1; index <= section; index++)
            {
                if (index > 1)
                {
                    splitStart += diff;
                    splitEnd = splitStart + diff;
                }

                var time = (splitEnd - splitStart) / (float)splitPart;

                String track2Url = CgiGetArchiveServerPartsWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                    .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                    .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                    .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                    .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                XmlDocument xmlDocTrack2 = Xml.LoadXmlFromHttp(track2Url, Server.Credential);
                ParseRecord(parts, xmlDocTrack2, splitStart, time, EventType.ArchiveServerRecord);
            }

            return parts;
        }

        public Queue<Record> GetSDRecordPart(UInt16 part, UInt64 start, UInt64 end)
        {
            var parts = new Queue<Record>();

            if(Server is INVR == false)
            {
                return parts;
            }

            var id = GetDeviceId();

            if (id == -1)
                return parts;

            start = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Floor(ConvertMillisecondsToSeconds(start))));
            end = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Ceiling(ConvertMillisecondsToSeconds(end))));

            //like... 20 sec but 500 part -> only need 20 part. 1 part for 1 sec.
            if (part > ConvertMillisecondsToSeconds(end - start))
                part = Convert.ToUInt16(ConvertMillisecondsToSeconds(end - start));

            var section = Math.Ceiling(((end - start) / (3600000.0)) / 14.0); // check if periods more than 14 hours, and can split by 14 hours per section.

            UInt64 diff = Convert.ToUInt64(Math.Round((end - start) / section));
            var splitPart = Math.Round(part / section);
            var splitStart = start;
            var splitEnd = splitStart + diff;

            for (var index = 1; index <= section; index++)
            {
                if (index > 1)
                {
                    splitStart += diff;
                    splitEnd = splitStart + diff;
                }

                var time = (splitEnd - splitStart)/(float) splitPart;

                String track2Url = CgiGetTrack2Parts.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                    .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                    .Replace("{START}", (splitStart/1000).ToString(CultureInfo.InvariantCulture))
                    .Replace("{END}", (splitEnd/1000).ToString(CultureInfo.InvariantCulture))
                    .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                XmlDocument xmlDocTrack2 = Xml.LoadXmlFromHttp(track2Url, Server.Credential);
                ParseRecord(parts, xmlDocTrack2, splitStart, time, EventType.SDRecord);
            }

            return parts;
        }

        public Boolean EnableArchiveServer = true;

        public Queue<Record> GetPlaybackDownloadPart(UInt16 part, UInt64 start, UInt64 end)
        {
            var parts = new Queue<Record>();
            if (CMS == null)
            {
                //enabled sd record can get part
                return !Profile.RemoteRecovery ? parts : GetSDRecordPart(part, start, end);
            }
            else
            {
                //if(EnableArchiveServer)
                //{
                //    return GetArchiveServerRecordPart(part, start, end);
                //}
            }

            if (String.IsNullOrEmpty(PlaybackSessionId)) return parts;

            var id = GetDeviceId();

            if (id == -1)
                return parts;

            start = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Floor(ConvertMillisecondsToSeconds(start))));
            end = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Ceiling(ConvertMillisecondsToSeconds(end))));

            //like... 20 sec but 500 part -> only need 20 part. 1 part for 1 sec.
            if (part > ConvertMillisecondsToSeconds(end - start))
                part = Convert.ToUInt16(ConvertMillisecondsToSeconds(end - start));

            var section = Math.Ceiling(((end - start) / (3600000.0)) / 14.0); // check if periods more than 14 hours, and can split by 14 hours per section.

            UInt64 diff = Convert.ToUInt64(Math.Round((end - start) / section));
            var splitPart = Math.Round(part / section);
            var splitStart = start;
            var splitEnd = splitStart + diff;

            for (var index = 1; index <= section; index++)
            {
                if (index > 1)
                {
                    splitStart += diff;
                    splitEnd = splitStart + diff;
                }

                var time = (splitEnd - splitStart) / (float)splitPart;

                String url = CgiGetPlaybackDownloadParts.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                                        .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture))
                                        .Replace("{SESSION}", PlaybackSessionId);

                XmlDocument xmlDoc = Xml.LoadXmlFromHttp(url, CMS.Credential);

                ParseRecord(parts, xmlDoc, splitStart, time, EventType.PlaybackDownload);
            }

            return parts;
        }

        public static double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
        }

        public static double ConvertSecondsToMilliseconds(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
        }

        //Once search " 1 " event, different event different color
        public virtual Queue<Record> GetEventParts(UInt16 part, UInt64 start, UInt64 end, EventType eventType)
        {
            var parts = new Queue<Record>();

            if (eventType == EventType.LPR_BlackList || eventType == EventType.LPR_Employee || eventType == EventType.LPR_Unregistered || 
                eventType == EventType.LPR_VIP || eventType == EventType.LPR_WhiteList)
            {
                return parts;
            }

            start = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Floor(ConvertMillisecondsToSeconds(start))));
            end = Convert.ToUInt64(ConvertSecondsToMilliseconds(Math.Ceiling(ConvertMillisecondsToSeconds(end))));

            //like... 20 sec but 500 part -> only need 20 part. 1 part for 1 sec.
            if (part > ConvertMillisecondsToSeconds(end - start))
                part = Convert.ToUInt16(ConvertMillisecondsToSeconds(end - start));

            var time = (end - start) / (float)part;

            var section = Math.Ceiling(((end - start) / (3600000.0)) / 14.0); // check if periods more than 14 hours, and can split by 14 hours per section.

            UInt64 diff = Convert.ToUInt64(Math.Round((end - start) / section));
            var splitPart = Math.Round(part / section);
            var splitStart = start;
            var splitEnd = splitStart + diff;

            for (var index = 1; index <= section; index++)
            {
                if (index > 1)
                {
                    splitStart += diff;
                    splitEnd = splitStart + diff;
                }
                XmlDocument xmlDoc;
                if (eventType == EventType.AudioIn)
                {
                    if (CMS != null)
                    {
                        String audioInUrl = CgiGetAudioInPartsWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                        xmlDoc = Xml.LoadXmlFromHttp(audioInUrl, CMS.Credential);
                    }
                    else
                    {
                        String audioInUrl = CgiGetAudioInParts.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                                          .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture));

                        xmlDoc = Xml.LoadXmlFromHttp(audioInUrl, Server.Credential);
                    }
                }
                else if (eventType == EventType.AudioOut)
                {
                    if (CMS != null)
                    {
                        String audioOutUrl = CgiGetAudioOutPartsWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));

                        xmlDoc = Xml.LoadXmlFromHttp(audioOutUrl, CMS.Credential);
                    }
                    else
                    {
                        String audioOutUrl = CgiGetAudioOutParts.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                                                            .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture));

                        xmlDoc = Xml.LoadXmlFromHttp(audioOutUrl, Server.Credential);
                    }
                }
                else
                {
                    if (CMS != null)
                    {
                        var qryDoc = new XmlDocument();
                        var xmlRoot = qryDoc.CreateElement("Request");
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("NVRId", Server.Id.ToString(CultureInfo.InvariantCulture)));
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("DeviceId", Id.ToString(CultureInfo.InvariantCulture)));
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("Part", splitPart.ToString(CultureInfo.InvariantCulture)));
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("StartTime", (splitStart).ToString(CultureInfo.InvariantCulture)));
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("EndTime", (splitEnd).ToString(CultureInfo.InvariantCulture)));
                        xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("Event", eventType.ToSearchCMSCondition()));
                        qryDoc.AppendChild(xmlRoot);
                        xmlDoc = Query.Send(CgiReadEventPartByCondition, qryDoc, CMS.Credential);
                        if (xmlDoc == null) return parts;
                        if (Xml.GetFirstElementByTagName(xmlDoc, "Parts") == null)
                            return parts;
                    }
                    else
                    {
                        if (eventType == EventType.Motion && Model.NumberOfMotion == 0)//because event search will block NumberOfMotion == 0, so as event map
                        {
                            return parts;
                        }
                        String eventPartUrl = CgiGetEventParts.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                            .Replace("{PART}", splitPart.ToString(CultureInfo.InvariantCulture))
                            .Replace("{START}", (splitStart / 1000).ToString(CultureInfo.InvariantCulture))
                            .Replace("{END}", (splitEnd / 1000).ToString(CultureInfo.InvariantCulture))
                            .Replace("{EVENT}", eventType.ToSearchCondition());

                        Console.WriteLine(eventPartUrl);
                        xmlDoc = Xml.LoadXmlFromHttp(eventPartUrl, Server.Credential);
                        Console.WriteLine(xmlDoc.InnerText);
                    }
                }

                ParseRecord(parts, xmlDoc, splitStart, time, eventType);
            }

            return parts;
        }

        public List<CameraEvent> SearchCameraEvent(UInt64 start, UInt64 end, EventType eventType)
        {
            var result = new List<CameraEvent>();

            var condition = new List<String>();

            switch (eventType)
            {
                case EventType.Motion:
                case EventType.DigitalInput:
                case EventType.DigitalOutput:
                case EventType.NetworkLoss:
                case EventType.NetworkRecovery:
                case EventType.VideoLoss:
                case EventType.VideoRecovery:
                //case EventType.RecordFailed:
                //case EventType.RecordRecovery:
                case EventType.ManualRecord:
                case EventType.UserDefine:
                case EventType.Panic:
                case EventType.CrossLine:
                case EventType.IntrusionDetection:
                case EventType.LoiteringDetection:
                case EventType.ObjectCountingIn:
                case EventType.ObjectCountingOut:
                case EventType.AudioDetection:
                case EventType.TamperDetection:
                    condition.Add(eventType.ToString());
                    break;
            }

            if (condition.Count == 0)
                return result;

            XmlDocument xmlDoc;
            if (CMS != null)
            {
                var qryDoc = new XmlDocument();
                var xmlRoot = qryDoc.CreateElement("Request");
                xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("NVRId", Server.Id.ToString(CultureInfo.InvariantCulture)));
                xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("DeviceId", Id.ToString(CultureInfo.InvariantCulture)));
                xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("StartTime", start.ToString(CultureInfo.InvariantCulture)));
                xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("EndTime", end.ToString(CultureInfo.InvariantCulture)));

                foreach (String type in condition)
                {
                    xmlRoot.AppendChild(qryDoc.CreateXmlElementWithText("Type", type));
                }
                qryDoc.AppendChild(xmlRoot);
                xmlDoc = Query.Send(CgiReadEventByCondition, qryDoc, CMS.Credential);

                if(!CMS.Configure.WithArchiveServer)
                {
                    if (xmlDoc == null) return result;
                    var eventNodes = xmlDoc.SelectNodes("/Events/Event");
                    if (eventNodes == null) return result;
                    if (eventNodes.Count == 0) return result;
                }
                else
                {
                    XmlDocument xmlDocArchive;

                    xmlDocArchive = Xml.LoadXmlFromHttp(CgiSearchEvent.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{START}", start.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{END}", end.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{TYPE}", String.Join(",", condition.ToArray()))
                                                           .Replace("{COUNT}", EventCount),
                                                            Server.Credential);

                    if (xmlDocArchive != null)
                    {
                        ParserEvemtFromXML(result, xmlDocArchive, eventType);
                    }
                }
            }
            else
            {
                xmlDoc = Xml.LoadXmlFromHttp(CgiSearchEvent.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{START}", start.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{END}", end.ToString(CultureInfo.InvariantCulture))
                                                           .Replace("{TYPE}", String.Join(",", condition.ToArray()))
                                                           .Replace("{COUNT}", EventCount),
                                                            Server.Credential);

                if (xmlDoc == null) return result;
            }

            //<Events>
            //  <Event>
            //      <DeviceID>1</DeviceID>
            //      <Type>ManualRecord</Type>
            //      <LocalTime>1319167634600</LocalTime>
            //      <DeviceTime>0</DeviceTime>
            //      <Count>1</Count>
            //      <Status value="1" trigger="1" id="1">30</Status>
            //  </Event>
            //</Events>

            ParserEvemtFromXML(result, xmlDoc, eventType);
            //XmlNodeList eventsList = xmlDoc.GetElementsByTagName("Event");
            //foreach (XmlElement node in eventsList)
            //{
            //    var statusNodes = node.GetElementsByTagName("Status");

            //    var timecode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(node, "LocalTime"));
            //    DateTime dateTime = DateTimes.ToDateTime(timecode, Server.Server.TimeZone);

            //    String status = Xml.GetFirstElementValueByTagName(node, "Status");

            //    foreach (XmlElement statusNode in statusNodes)
            //    {
            //        //Is Trigger
            //        if (String.Equals(statusNode.GetAttribute("trigger"), "0")) continue;
            //        UInt16 id = Convert.ToUInt16(statusNode.GetAttribute("id"));

            //        switch (eventType)
            //        {
            //            case EventType.Motion:
            //                if (id > Model.NumberOfMotion)
            //                    continue;
            //                break;

            //            case EventType.DigitalInput:
            //                if (!IOPort.ContainsKey(id) && id > Model.NumberOfDi)
            //                    continue;
            //                break;

            //            case EventType.DigitalOutput:
            //                if (!IOPort.ContainsKey(id) && id > Model.NumberOfDo)
            //                    continue;
            //                break;
            //        }

            //        result.Add(new CameraEvent
            //        {
            //            Id = id,
            //            Type = eventType,
            //            Value = (statusNode.GetAttribute("value") == "1"),
            //            DateTime = dateTime,
            //            Timecode = timecode,
            //            Status = status
            //        });
            //    }
            //}

            return result;
        }

        private List<CameraEvent> ParserEvemtFromXML(List<CameraEvent> result, XmlDocument xmlDoc, EventType eventType)
        {
            XmlNodeList eventsList = xmlDoc.GetElementsByTagName("Event");
            foreach (XmlElement node in eventsList)
            {
                var statusNodes = node.GetElementsByTagName("Status");

                var timecode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(node, "LocalTime"));
                DateTime dateTime = DateTimes.ToDateTime(timecode, Server.Server.TimeZone);

                String status = Xml.GetFirstElementValueByTagName(node, "Status");

                foreach (XmlElement statusNode in statusNodes)
                {
                    //Is Trigger
                    if (String.Equals(statusNode.GetAttribute("trigger"), "0")) continue;
                    UInt16 id = Convert.ToUInt16(statusNode.GetAttribute("id"));

                    switch (eventType)
                    {
                        case EventType.Motion:
                            if (id > Model.NumberOfMotion)
                                continue;
                            break;

                        case EventType.DigitalInput:
                            if (!IOPort.ContainsKey(id) && id > Model.NumberOfDi)
                                continue;
                            break;

                        case EventType.DigitalOutput:
                            if (!IOPort.ContainsKey(id) && id > Model.NumberOfDo)
                                continue;
                            break;
                    }

                    result.Add(new CameraEvent
                    {
                        Id = id,
                        Type = eventType,
                        Value = (statusNode.GetAttribute("value") == "1"),
                        DateTime = dateTime,
                        Timecode = timecode,
                        Status = status
                    });
                }
            }

            return result;
        }

        private const UInt16 Timeout = 40;//40 sec
        public DateTime GetNextRecord(UInt64 now)
        {
            var id = -1;

            if (this is IDeviceLayout)
            {
                foreach (var device in ((IDeviceLayout)this).Items)
                {
                    if (device == null) continue;
                    id = device.Id;
                    break;
                }
                if (id == -1)
                    return DateTime.MaxValue;
            }
            else if (this is ISubLayout)
            {
                var subLayout = this as ISubLayout;

                id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                if (id == -1)
                    return DateTime.MaxValue;
            }

            if (id == -1)
                id = Id;

            XmlDocument xmlDoc;
            XmlDocument archiveXmlDoc = null;
            if (CMS != null)
            {
                var url = CgiGetNextTimeWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                xmlDoc = Xml.LoadXmlFromHttp(url, CMS.Credential, Timeout);

                if(CMS.Configure.WithArchiveServer)
                {
                    url = CgiGetArchiveServerNextTimeWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture));
                    archiveXmlDoc = Xml.LoadXmlFromHttp(url, CMS.Credential, Timeout);
                }
            }
            else
            {
                var url = CgiGetNextTime.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                    .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture));
                xmlDoc = Xml.LoadXmlFromHttp(url, Server.Credential, Timeout);
            }

            if (xmlDoc != null)
            {
                String value = Xml.GetFirstElementValueByTagName(xmlDoc, "Next");
                if (archiveXmlDoc != null) //compare with archive server record
                {
                    String archiveValue = Xml.GetFirstElementValueByTagName(archiveXmlDoc, "Next");

                    if(String.IsNullOrEmpty(archiveValue))
                    {
                        if (value != "0")
                            return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                    }
                    else
                    {
                        if (archiveValue != "0")
                        {
                            var earliestValue = Math.Min(Convert.ToUInt64(String.IsNullOrEmpty(value) ? "0" : value), Convert.ToUInt64(archiveValue));
                            if (earliestValue.ToString() != "0")
                                return DateTimes.ToDateTime(Convert.ToUInt64(earliestValue), Server.Server.TimeZone);
                        }
                        else if (value != "0")
                        {
                            return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                        }
                    }
                }
                else
                {
                    if (value != "0")
                        return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                }

                //why? (deray)
                //return DateTime.MaxValue.AddTicks(-1);
            }

            return DateTime.MaxValue;
        }

        public UInt64 ArchiveRecordEntUTC;

        public void GetArchiveRecordEndTime(UInt64 now)
        {
            var id = -1;

            if (id == -1)
                id = Id;

            XmlDocument xmlDoc;
            xmlDoc = Xml.LoadXmlFromHttp(CgiGetArchiveRecordEndTimeWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                                               .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), CMS.Credential, Timeout);

            if (xmlDoc != null)
            {
                String value = Xml.GetFirstElementValueByTagName(xmlDoc, "Previous");
                if (value != "0" && !String.IsNullOrEmpty(value))
                {
                    ArchiveRecordEntUTC = Convert.ToUInt64(value);
                    return;
                }

                //why? (deray)
                //return DateTime.MinValue.AddTicks(1);
            }

            ArchiveRecordEntUTC = 0;
        }

        public DateTime GetPreviousRecord(UInt64 now)
        {
            var id = -1;
            if (this is IDeviceLayout)
            {
                foreach (var device in ((IDeviceLayout)this).Items)
                {
                    if (device == null) continue;
                    id = device.Id;
                    break;
                }
                if (id == -1)
                    return DateTime.MinValue;
            }
            else if (this is ISubLayout)
            {
                var subLayout = this as ISubLayout;
                if (subLayout == null) return DateTime.MinValue;

                id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                if (id == -1)
                    return DateTime.MinValue;
            }

            if (id == -1)
                id = Id;

            XmlDocument xmlDoc;
            XmlDocument archiveXmlDoc = null;
            if (CMS != null)
            {
                xmlDoc = Xml.LoadXmlFromHttp(CgiGetPreviousTimeWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                                               .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), CMS.Credential, Timeout);

                if(CMS.Configure.WithArchiveServer)
                {
                    archiveXmlDoc = Xml.LoadXmlFromHttp(CgiGetArchiveServerPreviousTimeWithNVR.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                                               .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), CMS.Credential, Timeout);
                }
            }
            else
            {
                xmlDoc = Xml.LoadXmlFromHttp(CgiGetPreviousTime.Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                                                               .Replace("{NOW}", now.ToString(CultureInfo.InvariantCulture)), Server.Credential, Timeout);

            }

            if (xmlDoc != null)
            {
                String value = Xml.GetFirstElementValueByTagName(xmlDoc, "Previous");
                if (archiveXmlDoc != null) //compare with archive server record
                {
                    String archiveValue = Xml.GetFirstElementValueByTagName(archiveXmlDoc, "Previous");

                    if (String.IsNullOrEmpty(archiveValue))
                    {
                        if (value != "0")
                            return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                    }
                    else
                    {
                        if (archiveValue != "0")
                        {
                            var earliestValue = Math.Max(Convert.ToUInt64(String.IsNullOrEmpty(value) ? "0" : value), Convert.ToUInt64(archiveValue));
                            if (earliestValue.ToString() != "0")
                                return DateTimes.ToDateTime(Convert.ToUInt64(earliestValue), Server.Server.TimeZone);
                        }
                        else if (value != "0")
                        {
                            return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                        }
                    }
                }
                else
                {
                    if (value != "0")
                        return DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                }

                //why? (deray)
                //return DateTime.MinValue.AddTicks(1);
            }

            return DateTime.MinValue;
        }

        public DateTime GetBeginRecord()
        {
            DateTime begin = GetNextRecord(0);
            
            return (begin == DateTime.MaxValue)
                ? DateTime.MinValue: begin;
        }

        //ACTi only, send ALL do status
        public void DigitalOutput()
        {
            if (Model.NumberOfDo == 0) return;

            var doStatus = new List<String>();
            foreach (var digitalOutputStatu in DigitalOutputStatus)
            {
                if (digitalOutputStatu.Key <= Model.NumberOfDo)
                    doStatus.Add(digitalOutputStatu.Key + "," + (digitalOutputStatu.Value ? "on" : "off"));
            }

            if (doStatus.Count < 2)
                doStatus.Add("2,off");

            if (CMS != null)
            {
                Xml.LoadXmlFromHttp(CgiDigitalOutputWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("%2", String.Join(",", doStatus.ToArray())).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), CMS.Credential);
            }
            else
            {
                Xml.LoadXmlFromHttp(CgiDigitalOutput.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("%2", String.Join(",", doStatus.ToArray())), Server.Credential);
            }
        }

        public void DigitalOutput(UInt16 id)
        {
            if (Model.IOPortSupport != null)
            {
                if (!IOPort.ContainsKey(id) || IOPort[id] != DeviceConstant.IOPort.Output) return;
            }
            else
            {
                if (Model.NumberOfDo == 0 || id > Model.NumberOfDo) return;
            }

            var doStatus = id + "," + (DigitalOutputStatus[id] ? "on" : "off");

            if (CMS != null)
            {
                Xml.LoadXmlFromHttp(CgiDigitalOutputWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("%2", doStatus).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)), CMS.Credential);
            }
            else
            {
                Xml.LoadXmlFromHttp(CgiDigitalOutput.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("%2", doStatus), Server.Credential);
            }
        }

        public Image Snapshot { get; set; }

        //------------------------------------------------------------------------------------
        public Image GetSnapshot()
        {
            if (CMS != null)
            {
                Snapshot = LoadImageFromHttp(CgiSnapshotWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                Snapshot = LoadImageFromHttp(CgiSnapshot.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)));
            }

            return Snapshot;
        }

        public Image GetSnapshot(Size size)
        {
            if (CMS != null)
            {
                return LoadImageFromHttp(CgiSnapshotWithSizeWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                        .Replace("{WIDTH}", size.Width.ToString(CultureInfo.InvariantCulture))
                                                        .Replace("{HEIGHT}", size.Height.ToString(CultureInfo.InvariantCulture))
                                                        .Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)));
            }

            return LoadImageFromHttp(CgiSnapshotWithSize.Replace("%1", Id.ToString(CultureInfo.InvariantCulture))
                                                        .Replace("{WIDTH}", size.Width.ToString(CultureInfo.InvariantCulture))
                                                        .Replace("{HEIGHT}", size.Height.ToString(CultureInfo.InvariantCulture)));
        }

        public Image GetSnapshot(UInt64 timecode)
        {
            if (CMS != null)
            {
                return (timecode != 0)
                ? LoadImageFromHttp(CgiSnapshotWithTimecodeWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("{TIMECODE}", timecode.ToString(CultureInfo.InvariantCulture)).Replace("{NVRID}", Server.Id.ToString(CultureInfo.InvariantCulture)))
                : null;
            }

            return (timecode != 0)
                ? LoadImageFromHttp(CgiSnapshotWithTimecode.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace("{TIMECODE}", timecode.ToString(CultureInfo.InvariantCulture)))
                : null;
        }

        public Image GetSnapshot(UInt64 timecode, Size size)
        {
            if (CMS != null)
            {
                return (timecode != 0)
                       ? LoadImageFromHttp(CgiSnapshotWithTimecodeAndSizeWithNVR.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace(
                           "{TIMECODE}", timecode.ToString()).Replace("{WIDTH}", size.Width.ToString()).Replace("{HEIGHT}", size.Height.ToString()).Replace("{NVRID}", Server.Id.ToString()))
                       : null;
            }

            return (timecode != 0)
                       ? LoadImageFromHttp(CgiSnapshotWithTimecodeAndSize.Replace("%1", Id.ToString(CultureInfo.InvariantCulture)).Replace(
                           "{TIMECODE}", timecode.ToString()).Replace("{WIDTH}", size.Width.ToString()).Replace("{HEIGHT}", size.Height.ToString()))
                       : null;
        }

        private const String ImageCmd = "cmdGetSnapShot=%1,0,0,%2,%3";
        public Image GetFullResolutionSnapshot(Size size, UInt16 channelId)
        {
            if (CMS != null)
            {
                var imageForCMS = Xml.LoadImageFromHttpWithPostData(CgiRequeryImageWithNVR.Replace("%1", Id.ToString()), CMS.Credential,
                ImageCmd.Replace("%1", channelId.ToString()).Replace("%2", size.Width.ToString()).Replace("%3", size.Height.ToString()).Replace("{NVRID}", Server.Id.ToString()));

                return imageForCMS;
            }

            var image = Xml.LoadImageFromHttpWithPostData(CgiRequeryImage.Replace("%1", Id.ToString()), Server.Credential,
            ImageCmd.Replace("%1", channelId.ToString()).Replace("%2", size.Width.ToString()).Replace("%3", size.Height.ToString()));

            return image;
        }

        public override Boolean CheckPermission(Permission permission)
        {
            if (CMS != null)
            {
                //can not control belong nvr's permission
                return true;
            }
            return (Server != null && Server.User.Current.CheckPermission(this, permission));
        }
        //------------------------------------------------------------------------------------
        private Image LoadImageFromHttp(String url)
        {
            //add different random to url, avoid retry get image still no result(cache on server)
            //url += "&uuid=" + DateTime.Now.Ticks;

            Image tmpImage = null;
            try
            {
                var request = Xml.GetHttpRequest(url, CMS != null ? CMS.Credential : Server.Credential);

                request.Timeout = 5000;// 5 secs timeout
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();

                if (webStream != null)
                {
                    tmpImage = Image.FromStream(webStream);
                    tmpImage.Tag = url;
                    webStream.Close();
                }

                webResponse.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return tmpImage;
        }

        public void ManualRecord()
        {
            //if (CMS != null)
            //{
            //    Xml.LoadXmlFromHttp(CgiManualRecordWithNVR.Replace("%1", Id.ToString()).Replace("{DURATION}", Server.Configure.ManualRecordDuration.ToString()).Replace("{NVRID}", Server.Id.ToString()), CMS.Credential);
            //    return;
            //}
            //Xml.LoadXmlFromHttp(CgiManualRecord.Replace("%1", Id.ToString()).Replace("{DURATION}", Server.Configure.ManualRecordDuration.ToString()), Server.Credential);

            ManualRecord(Server.Configure.ManualRecordDuration.ToString());
        }

        public void ManualRecord(String duration)
        {
            if (CMS != null)
            {
                Xml.LoadXmlFromHttp(CgiManualRecordWithNVR.Replace("%1", Id.ToString()).Replace("{DURATION}", duration).Replace("{NVRID}", Server.Id.ToString()), CMS.Credential);
                return;
            }
            Xml.LoadXmlFromHttp(CgiManualRecord.Replace("%1", Id.ToString()).Replace("{DURATION}", duration), Server.Credential);
        }

        public void UserDefine(int eventId, String data)
        {
            Xml.LoadXmlFromHttp(CgiUserDefine.Replace("%1", Id.ToString()).Replace("%2", eventId.ToString()).Replace("%3", data), Server.Credential);
        }

        public void Panic()
        {
            if (CMS != null)
            {
                Xml.LoadXmlFromHttp(CgiPanicWithNVR.Replace("%1", Id.ToString()).Replace("{NVRID}", Server.Id.ToString()), CMS.Credential);
                return;
            }
            Xml.LoadXmlFromHttp(CgiPanic.Replace("%1", Id.ToString()), Server.Credential);
        }

        public void  InitFisheyeLibrary(ICamera camera, bool dewarpEnable, short mountType)
        {
            if (CMS != null) return;
            Server.Utility.InitFisheyeLibrary(camera, dewarpEnable, mountType);
        }

        public String ExportVideo(UInt64 startTime, UInt64 endTime, Boolean displayOsd, Boolean audioIn, Boolean audioOut, UInt16 encode, String path, UInt16 quality, UInt16 scale, UInt16 osdWatermark)
        {
            if (CMS != null)
                return CMS.Utility.ExportVideo(this, startTime, endTime, displayOsd, audioIn, audioOut, encode, path, quality, scale, osdWatermark);

            return Server.Utility.ExportVideo(this, startTime, endTime, displayOsd, audioIn, audioOut, encode, path, quality, scale, osdWatermark);
        }

        public void StopExportVideo()
        {
            if (CMS != null)
            {
                CMS.Utility.StopExportVideo();
                return;
            }
            Server.Utility.StopExportVideo();
        }

        public void ExportVideoProgress(UInt16 progress, ExportVideoStatus status)
        {
            if (OnExportVideoProgress != null)
                OnExportVideoProgress(this, new EventArgs<UInt16, ExportVideoStatus>(progress, status));

            if (progress == 100 && status == ExportVideoStatus.ExportFinished && OnExportVideoComplete != null)
                OnExportVideoComplete(this, null);
        }

        public Int32 StartAudioTransfer()
        {
            IsAudioOut = true;

            Int32 result = CMS != null ? CMS.Utility.StartAudioTransfer(String.Format("nvr{0}:{1}", Server.Id, Id)) : Server.Utility.StartAudioTransfer("channel" + Id);
            if (result == 0)
                IsAudioOut = false;

            return result;
        }

        public void StopAudioTransfer()
        {
            IsAudioOut = false;

            if (CMS != null)
            {
                CMS.Utility.StopAudioTransfer();
                return;
            }
            Server.Utility.StopAudioTransfer();
        }

        public UInt16 MaximumSearchResult { get; private set; }
        private const UInt16 SmartSearchTimeout = 40;//40 sec
        private Boolean _isStopSearch;
        private Int32 _eventCount;
        public void SmartSearch(UInt32 period, UInt64 keyFrame, UInt64 startTime, UInt64 endTime, Rectangle[] areas)
        {
            if (endTime < startTime)
            {
                var tmp = endTime;
                endTime = startTime;
                startTime = tmp;
            }

            _isStopSearch = false;

            while (endTime > startTime)
            {
                var periodEnd = Math.Min(endTime, startTime + (period * 10 * 1000));
                var result = SmartSearchShortTimePeriod(period, keyFrame, startTime, periodEnd, areas);
                startTime = periodEnd;

                if (_isStopSearch)
                {
                    _eventCount = 0;
                    SearchCompleted();
                    return;
                }

                //parse result
                if (result != null)
                {
                    //<SmartSearch>
                    //    <Time>1314098325000</Time>
                    //<SmartSearch>

                    var times = result.GetElementsByTagName("Time");

                    if (times.Count > 0)
                    {
                        _eventCount += times.Count;
                        foreach (XmlElement xmlElement in times)
                        {
                            xmlElement.SetAttribute("type", "IVS");
                            xmlElement.SetAttribute("desc", "IVS");
                            xmlElement.SetAttribute("status", "");
                        }

                        SearchResult(result.InnerXml);
                    }
                }

                if (MaximumSearchResult > 0 && _eventCount >= MaximumSearchResult)
                    break;
            }

            _eventCount = 0;
            SearchCompleted();
        }

        //apart ivs search(long time search and STOP it will wait very long time,
        //each period's  MAX length is period x 10, ex, period: 1sex => search 10sec time period
        public XmlDocument SmartSearchShortTimePeriod(UInt32 period, UInt64 keyFrame, UInt64 startTime, UInt64 endTime, Rectangle[] areas)
        {
            //<SmartSearch>
            //    <Key>1311052200000</Key>
            //    <StartTime>1311052200000</StartTime>
            //    <EndTime>1311052400000</EndTime>
            //    <Period>4</Period>
            //    <!--Condition>Motion</Condition>
            //    <Condition>DigitalInput</Condition>
            //    <Condition>DigitalOutput</Condition>
            //    <Condition>ManualRecord</Condition>
            //    <Condition>NetworkLoss</Condition>
            //    <Condition>NetworkRecovery</Condition>
            //    <Condition>VideoLoss</Condition>
            //    <Condition>VideoRecovery</Condition>
            //    <Condition>RecordFailed</Condition>
            //    <Condition>RecordRecovery</Condition>
            //    <Condition>UserDefine</Condition-->
            //    <MotionArea>1,1,640,480</MotionArea>
            //    <MotionArea>1,1,320,240</MotionArea>
            //</SmartSearch>

            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("SmartSearch");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Key", keyFrame.ToString()));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("StartTime", startTime.ToString()));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("EndTime", endTime.ToString()));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Period", period.ToString()));

            if (areas.Length > 0)
            {
                foreach (Rectangle area in areas)
                {
                    xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("MotionArea", area.X + "," + area.Y + "," + area.Width + "," + area.Height));
                }
            }

            if (CMS != null)
            {
                return Xml.PostXmlToHttp(CgiSmartSearchWithNVR.Replace("%1", Id.ToString()).Replace("{NVRID}", Server.Id.ToString()), xmlDoc, CMS.Credential, SmartSearchTimeout);
            }

            return Xml.PostXmlToHttp(CgiSmartSearch.Replace("%1", Id.ToString()), xmlDoc, Server.Credential, SmartSearchTimeout);
        }

        public void TimePeriodSearch(UInt32 period, UInt64 startTime, UInt64 endTime)
        {
            if (endTime < startTime)
            {
                var tmp = endTime;
                endTime = startTime;
                startTime = tmp;
            }

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("SmartSearch");
            xmlDoc.AppendChild(root);

            while (startTime <= endTime)
            {
                //if (startTime > DateTimes.UtcNow) break;
                root.AppendChild(xmlDoc.CreateXmlElementWithText("Time", startTime.ToString()));
                if (MaximumSearchResult > 0 && root.ChildNodes.Count == MaximumSearchResult) break;

                startTime += period * 1000;
            }

            SearchResult(xmlDoc.InnerXml);

            SearchCompleted();
        }

        //search ONE event at one time., search ALL event at one time will miss some event by use next-timecode to continue search.
        public void EventSearch(UInt64 startTime, UInt64 endTime, List<EventType> events, List<UInt64> period)
        {
            if (events.Count == 0)
            {
                _eventCount = 0;
                SearchCompleted();
                return;
            }

            if (endTime < startTime)
            {
                var tmp = endTime;
                endTime = startTime;
                startTime = tmp;
            }

            _isStopSearch = false;

            if (CMS != null)
            {
                for (ulong i = startTime; i < endTime; i += 86400000)
                {
                    foreach (var eventType in events)
                    {
                        EventSearch(i, i + 86399000, eventType, period);

                        if (_isStopSearch)
                        {
                            _eventCount = 0;
                            SearchCompleted();
                            return;
                        }
                    }
                }

                //foreach (var eventType in events)
                //{
                //    EventSearch(startTime, endTime, eventType, period);

                //    if (_isStopSearch)
                //    {
                //        _eventCount = 0;
                //        SearchCompleted();
                //        return;
                //    }
                //}
            }
            else
            {
                foreach (var eventType in events)
                {
                    EventSearch(startTime, endTime, eventType, period);

                    if (_isStopSearch)
                    {
                        _eventCount = 0;
                        SearchCompleted();
                        return;
                    }
                }
            }

            SearchCompleted();
        }

        public void EventSearch(UInt64 startTime, UInt64 endTime, EventType eventType, List<UInt64> period)
        {
            var result = SearchCameraEvent(startTime, endTime, eventType);

            if (_isStopSearch)
                return;

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("SmartSearch");
            root.SetAttribute("Id", Id.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("nvrId", Server.Id.ToString(CultureInfo.InvariantCulture));
            xmlDoc.AppendChild(root);

            if (result.Count == 0)
                return;

            UInt64 nextTimeCode = 0;

            if (period == null || period.Count == 0)
            {
                _eventCount += result.Count;
                foreach (var cameraEvent in result)
                {
                    nextTimeCode = DateTimes.ToUtc(cameraEvent.DateTime, Server.Server.TimeZone);

                    ConvertEventToXml(xmlDoc, root, cameraEvent, nextTimeCode);
                }
            }
            else
            {
                foreach (var cameraEvent in result)
                {
                    nextTimeCode = DateTimes.ToUtc(cameraEvent.DateTime, Server.Server.TimeZone);

                    foreach (var time in period)
                    {
                        //diff between 1 sec
                        if (nextTimeCode > time && Math.Abs(Convert.ToInt32(nextTimeCode - time)) < 1000)
                        {
                            _eventCount++;
                            ConvertEventToXml(xmlDoc, root, cameraEvent, nextTimeCode);
                            break;
                        }
                    }
                }

                while (period.Count > 0 && period.First() - nextTimeCode < 0)
                {
                    period.Remove(period.First());
                }
            }

            SearchResult(xmlDoc.InnerXml);
            nextTimeCode += 1; //1000, if add 1 sec will lose event between this 1 sec

            if ((MaximumSearchResult == 0 || _eventCount < MaximumSearchResult) && nextTimeCode < endTime)
                EventSearch(nextTimeCode, endTime, eventType, period);
        }

        private static void ConvertEventToXml(XmlDocument xmlDoc, XmlElement root, CameraEvent cameraEvent, UInt64 nextTimeCode)
        {
            var xmlElement = xmlDoc.CreateElement("Time");
            xmlElement.SetAttribute("type", cameraEvent.Type.ToString());
            xmlElement.SetAttribute("id", cameraEvent.Id.ToString());
            xmlElement.SetAttribute("desc", cameraEvent.ToLocalizationString());
            xmlElement.SetAttribute("status", cameraEvent.Status);
            xmlElement.SetAttribute("value", cameraEvent.Value ? "true" : "false");
            xmlElement.InnerText = nextTimeCode.ToString(CultureInfo.InvariantCulture);
            root.AppendChild(xmlElement);
        }


        public void StopSearch()
        {
            if (_isStopSearch) return;
            _isStopSearch = true;
        }

        private void SearchResult(String msg)
        {
            if (OnSmartSearchResult != null)
                OnSmartSearchResult(this, new EventArgs<String>(msg));
        }

        private void SearchCompleted()
        {
            _eventCount = 0; //clear counter af finish
            if (OnSmartSearchComplete != null)
                OnSmartSearchComplete(this, null);
        }
    }

    static class EventTypeConverter
    {
        public static string ToSearchCondition(this EventType eventType)
        {
            String condition = "";

            switch (eventType)
            {
                case EventType.Motion:
                    condition = "md";
                    break;

                case EventType.DigitalInput:
                    condition = "di";
                    break;

                case EventType.DigitalOutput:
                    condition = "do";
                    break;

                case EventType.NetworkLoss:
                    condition = "networkloss";
                    break;

                case EventType.NetworkRecovery:
                    condition = "networkrecovery";
                    break;

                case EventType.VideoLoss:
                    condition = "videoloss";
                    break;

                case EventType.VideoRecovery:
                    condition = "videorecovery";
                    break;

                case EventType.ManualRecord:
                    condition = "manualrecord";
                    break;

                case EventType.UserDefine:
                    condition = "userdefine";
                    break;

                case EventType.Panic:
                    condition = "panic";
                    break;

                case EventType.CrossLine:
                    condition = "crossline";
                    break;

                case EventType.IntrusionDetection:
                    condition = "intrusiondetection";
                    break;

                case EventType.LoiteringDetection:
                    condition = "loiteringdetection";
                    break;

                case EventType.ObjectCountingIn:
                    condition = "objectcountingin";
                    break;

                case EventType.ObjectCountingOut:
                    condition = "objectcountingout";
                    break;

                case EventType.AudioDetection:
                    condition = "audiodetection";
                    break;

                case EventType.TamperDetection:
                    condition = "tamperdetection";
                    break;
            }

            return condition;
        }

        public static string ToSearchCMSCondition(this EventType eventType)
        {
            String condition = "";

            switch (eventType)
            {
                case EventType.Motion:
                    condition = "Motion";
                    break;

                case EventType.DigitalInput:
                    condition = "DigitalInput";
                    break;

                case EventType.DigitalOutput:
                    condition = "DigitalOutput";
                    break;

                case EventType.NetworkLoss:
                    condition = "NetworkLoss";
                    break;

                case EventType.NetworkRecovery:
                    condition = "NetworkRecovery";
                    break;

                case EventType.VideoLoss:
                    condition = "VideoLoss";
                    break;

                case EventType.VideoRecovery:
                    condition = "VideoRecovery";
                    break;

                case EventType.ManualRecord:
                    condition = "ManualRecord";
                    break;

                case EventType.UserDefine:
                    condition = "UserDefine";
                    break;

                case EventType.Panic:
                    condition = "Panic";
                    break;

                case EventType.CrossLine:
                    condition = "CrossLine";
                    break;

                case EventType.IntrusionDetection:
                    condition = "IntrusionDetection";
                    break;

                case EventType.LoiteringDetection:
                    condition = "LoiteringDetection";
                    break;

                case EventType.ObjectCountingIn:
                    condition = "ObjectCountingIn";
                    break;

                case EventType.ObjectCountingOut:
                    condition = "ObjectCountingOut";
                    break;

                case EventType.AudioDetection:
                    condition = "AudioDetection";
                    break;

                case EventType.TamperDetection:
                    condition = "TamperDetection";
                    break;
            }

            return condition;
        }
    }
}
