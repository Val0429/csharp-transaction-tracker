using Constant;
using DeviceConstant;
using Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace TimeTrack
{
    public class TrackerContainer : Panel, ITrackerContainer
    {
        public event EventHandler OnSelectionChange;
        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler OnGetPartStart;
        public event EventHandler OnGetPartCompleted;
        public event EventHandler OnGetDownloadPartCompleted;
        public event EventHandler OnStop;

        protected void RaiseOnTimecodeChange()
        {
            try
            {
                if (OnTimecodeChange != null && CanTimecodeChange)
                {
                    string xml = TimecodeChangeXml(_dateTime.ToUtcString(Server.Server.TimeZone),
                                                   VisibleMinDateTime.ToUtcString(Server.Server.TimeZone),
                                                   VisibleMaxDateTime.ToUtcString(Server.Server.TimeZone),
                                                   (RangeStartDate != DateTime.MinValue) ? RangeStartDate.ToUtcString(Server.Server.TimeZone) : "",
                                                   (RangeEndDate != DateTime.MaxValue) ? RangeEndDate.ToUtcString(Server.Server.TimeZone) : "");

                    OnTimecodeChange(this, new EventArgs<String>(xml));
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
            }
        }

        private bool CanTimecodeChange
        {
            get
            {
                PlayMode mode = PlayMode.Playback1X;

                if (Server.Configure.EnablePlaybackSmooth && Rate > 0)
                {
                    
                }
                else
                {
                    if (Rate != 1)
                        mode = PlayMode.GotoTimestamp;
                }

                foreach (var tracker in Trackers.Values)
                {
                    if (tracker == null) continue;

                    if (tracker.Camera is IDeviceLayout || tracker.Camera is ISubLayout)
                    {
                        mode = PlayMode.GotoTimestamp;
                    }
                }
                return mode == PlayMode.GotoTimestamp;
            }
        }

        public event EventHandler<EventArgs<TimeUnit, UInt64[]>> OnTimeUnitChange;
        public event EventHandler OnDateTimeChange;
        public event EventHandler OnDateTimeRangeChange;
        public event EventHandler<EventArgs<Dictionary<IDevice, Record>>> OnRecordDataChange;

        public event EventHandler<MouseEventArgs> OnTrackerContainerMouseDown;

        public List<EventType> SearchEventType { get; private set; }
        public List<IVideoWindow> VideoWindows { get; set; }

        public Dictionary<ushort, ITracker> Trackers{ get; set; }

        //public readonly Dictionary<UInt16, ITracker> Trackers = new Dictionary<UInt16, ITracker>();
        protected static readonly Queue<ITracker> RecycleTrackers = new Queue<ITracker>();

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (_server != null)
                {
                    PlayOnRateTimer.SynchronizingObject =
                    RefreshTimer.SynchronizingObject = value.Form;
                }
            }
        }
        public Boolean IsMinimize { get; set; }
        protected const UInt16 QueueLength = 3000;//1500

        public Int32 Count
        {
            get
            {
                return Trackers.Count(tracker => (tracker.Value != null && tracker.Value.Using));
                //return Trackers.Count;
            }
        }

        protected ScaleUnit UnitScale;

        public DateTime StartDate
        {
            get
            {
                if (UnitScale == 0 || _unitTime == 0 || _dateTime.Ticks == 0 || Parent == null) return _dateTime;

                //int length = QueueLength; //3000 px
                int length = (Parent.Width == 0) ? QueueLength : Convert.ToInt32(Parent.Width * 0.65); //current display width

                DateTime startDate;
                try
                {
                    startDate = _dateTime.AddTicks(-1 * (length) * (Int64)TicksPerPixel);
                    //startDate = startDate.AddTicks((startDate.Ticks % (Int64)_unitTime) * -1); //try to meet range is good think, but will cause end time short than expect end time(in 1-day scale)
                }
                catch (Exception) //avoid camera has no record
                {
                    return _dateTime;
                }


                return startDate;
            }
        }
        public DateTime EndDate
        {
            get
            {
                if (UnitScale == 0 || _unitTime == 0 || _dateTime.Ticks == 0 || Parent == null) return _dateTime;

                //var length = QueueLength; //3000 px , total queue 6000 px ,  start <- 3000 -> now <- 3000 -> end
                int length = (Parent.Width == 0) ? QueueLength : Convert.ToInt32(Parent.Width * 0.65); //current display width

                DateTime endDate = StartDate.AddTicks(length * 2 * (Int64)TicksPerPixel);

                return endDate;
            }
        }

        public UInt64 StartTime
        {
            get
            {
                return DateTimes.ToUtc(StartDate, Server.Server.TimeZone);
                //Utility.DateTimeToUtcMilliseconds((StartDate > Utility.App.ServerDateTime) ? Utility.App.ServerDateTime : StartDate);
            }
        }
        public UInt64 EndTime
        {
            get
            {
                return DateTimes.ToUtc(EndDate, Server.Server.TimeZone);
                //Utility.DateTimeToUtcMilliseconds((EndDate > Utility.App.ServerDateTime) ? Utility.App.ServerDateTime : EndDate);
            }
        }

        protected static String TimecodeChangeXml(String timestamp, String startTime, String endTime, String rangeStartTime, String rangeEndTime)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Timestamp", timestamp));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartTime", startTime));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndTime", endTime));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RangeStartTime", rangeStartTime));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RangeEndTime", rangeEndTime));

            return xmlDoc.InnerXml;
        }

        private DateTime _rateStartDate = DateTime.MinValue;
        public DateTime RangeStartDate
        {
            get { return _rateStartDate; }
            set { _rateStartDate = value; }
        }

        private DateTime _rangeEndDate = DateTime.MaxValue;
        public DateTime RangeEndDate
        {
            get { return _rangeEndDate; }
            set { _rangeEndDate = value; }
        }

        public Boolean IgnoreTriggerOnTimecodeChange { get; set; }
        public DateTime VisibleMinDateTime { get; set; }
        public DateTime VisibleMaxDateTime { get; set; }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime.CompareTo(value) == 0) return;

                //if (Server is ICMS && _dateTime.Ticks > 0)
                //{
                //    foreach (KeyValuePair<ushort, ITracker> tracker in Trackers)
                //    {
                //        var track = tracker.Value as Tracker;
                //        if (track == null) continue;

                //        var latestDownloadRecords = (from record in track.PlaybackParts
                //                                     where
                //                                        record.EndDateTime > value
                //                                     select record);

                //        if (latestDownloadRecords.Count() == 0)
                //        {
                //            if (OnStop != null)
                //            {
                //                OnStop(this, null);
                //            }
                //            return;
                //        }
                //    }
                //}

                _dateTime = value;

                if (IsMinimize)
                {
                    UpdateTimecodeRange();
                }
                else
                {
                    if (UnitScale == 0 || _unitTime == 0 || Parent == null) return;

                    UpdateTimecodeRange();

                    if (VisibleMinDateTime < _getPartStartDate || VisibleMaxDateTime > _getPartEndDate)
                    {
                        ShowRecord();
                    }

                    Invalidate();
                }

                if (OnDateTimeChange != null)
                    OnDateTimeChange(this, null);

                if (IgnoreTriggerOnTimecodeChange)
                {
                    //only ignore "once"
                    IgnoreTriggerOnTimecodeChange = false;
                    return;
                }

                RaiseOnTimecodeChange();
            }
        }

        public UInt16 RecordHeight = 20;
        public UInt16 TrackerHeight = 21;
        public UInt16 TopPosition = 46;
        public UInt16 PaintTop = 0;
        protected const Int32 Tick = 166667;// 1000000 0.1sec 0.015sec, 0.0167sec = 60fps
        protected readonly System.Timers.Timer PlayOnRateTimer = new System.Timers.Timer();
        protected readonly System.Timers.Timer RefreshTimer = new System.Timers.Timer();


        public UInt16 PageIndex { get; set; }
        public UInt16 MaxConnection { get; set; }
        public UInt16 TrackerNumPerPage = 4;


        // Constructor
        public TrackerContainer()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(49, 50, 55);// Color.White;// Color.Transparent;
            Cursor = Cursors.NoMoveHoriz;
            Size = new Size(201, 110);
            Dock = DockStyle.Fill;

            PageIndex = 1;
            MaxConnection = 64;//TimeTrackSwitch
            Trackers = new Dictionary<UInt16, ITracker>();
            SearchEventType = new List<EventType>();

            IgnoreTriggerOnTimecodeChange = false;

            MouseDown += ContainerMouseDown;
            MouseUp += ContainerMouseUp;
            MouseDoubleClick += ContainerMouseDoubleClick;

            PlayOnRateTimer.Elapsed += PlayOnRate;
            PlayOnRateTimer.Interval = Tick / 10000;

            RefreshTimer.Elapsed += ShowRecord;
            RefreshTimer.Interval = MinimumReShowTime;

            Paint += TrackerContainerPaint;
            SizeChanged += ContainerSizeChanged;
            VideoWindows = new List<IVideoWindow>();
        }

        public float Rate { get; set; }

        private void PlayOnRate(Object sender, EventArgs e)
        {
            PlayOnRate();
        }

        private readonly Stopwatch _watch = new Stopwatch();
        public void PlayOnRate()
        {
            if (CanTimecodeChange)
            {
                if(Server.Configure.EnablePlaybackSmooth && Rate > 0)
                {
                    return;
                }
                else
                {
                    PlayOnRateTimer.Enabled = false;
                    PlayOnRateTimer.Enabled = true;
                }
            }
            else
            {
                Stop();
            }

            _watch.Stop();
            var elapsedMilliseconds = _watch.ElapsedMilliseconds;
            _watch.Reset();
            _watch.Start();

            //first time
            //if (elapsedMilliseconds == 0)
            //{
            //    DateTime = new DateTime(DateTime.Ticks + (Int64)(Tick * Rate));
            //}
            //else
            //{
            //    DateTime = new DateTime(DateTime.Ticks + (Int64)(elapsedMilliseconds * 10000 * Rate));
            //}
            var datetime = elapsedMilliseconds == 0
                               ? new DateTime(DateTime.Ticks + (Int64)(Tick * Rate))
                               : new DateTime(DateTime.Ticks + (Int64)(elapsedMilliseconds * 10000 * Rate));

            //playback auto break for waiting download bar 
            if (Server is ICMS && _dateTime.Ticks > 0)
            {
                foreach (KeyValuePair<ushort, ITracker> tracker in Trackers)
                {
                    var track = tracker.Value as Tracker;
                    if (track == null) continue;

                    //var videoWindow = VideoWindows.First(x => x.Camera == track.Camera);
                    //if (videoWindow == null) continue;
                    //if (videoWindow.Visible == false) continue;

                    var check = false;
                    foreach (IVideoWindow videoWindow in VideoWindows)
                    {
                        if (videoWindow.Visible && videoWindow.Camera == track.Camera)
                        {
                            check = true;
                            break;
                        }
                    }

                    if (!check) continue;

                    var latestDownloadRecords = (from record in track.PlaybackParts
                                                 where
                                                    record.EndDateTime > datetime
                                                 select record);

                    if (track.GetRecordFromDateTime() != null)
                    {
                        if (latestDownloadRecords.Count() == 0 && track.GetRightNowRecord() != null)
                        {
                            if (OnStop != null)
                            {
                                OnStop(this, null);
                            }
                            return;
                        }
                    }
                }
            }

            DateTime = datetime;
            Trace.WriteLine(DateTime.ToString("HHmmss.fff"));
        }


        public void Stop()
        {
            _watch.Stop();
            _watch.Reset();
            PlayOnRateTimer.Enabled = false;
        }

        private void ContainerSizeChanged(Object sender, EventArgs e)
        {
            if (Parent.Width > 0 && Parent.Height > 0 && _dateTime.Ticks != 0)
            {
                //ResetBackgroundScalePosition();
                UpdateTimecodeRange();
                Invalidate();
            }
        }

        public const UInt16 MinimumReShowTime = 30000;
        public TicksPerPixel TicksPerPixel;
        protected String _textFormat;
        protected String _textFormatSmall;
        protected UInt16 _textDiff;
        protected Int64 _colorSplit;
        protected TimeUnit _unitTime;
        public TimeUnit UnitTime
        {
            get { return _unitTime; }
            set
            {
                if (_unitTime == value) return;

                _unitTime = value;

                switch (value)
                {
                    case TimeUnit.Unit1Senond:
                        UnitScale = ScaleUnit.Unit1Senond;
                        TicksPerPixel = TicksPerPixel.Unit1Senond;
                        _colorSplit = (Int64)TimeUnit.Unit10Senonds;
                        _textFormat = "ss";
                        _textDiff = 7;
                        break;

                    case TimeUnit.Unit10Senonds:
                        UnitScale = ScaleUnit.Unit10Senonds;
                        TicksPerPixel = TicksPerPixel.Unit10Senonds;
                        _colorSplit = (Int64)TimeUnit.Unit1Minute;
                        _textFormat = "ss";
                        _textDiff = 7;
                        break;

                    case TimeUnit.Unit1Minute:
                        UnitScale = ScaleUnit.Unit1Minute;
                        TicksPerPixel = TicksPerPixel.Unit1Minute;
                        _colorSplit = (Int64)TimeUnit.Unit10Minutes;
                        _textFormat = "HH:mm";
                        _textDiff = 13;
                        break;

                    case TimeUnit.Unit10Minutes:
                        UnitScale = ScaleUnit.Unit10Minutes;
                        TicksPerPixel = TicksPerPixel.Unit10Minutes;
                        _colorSplit = (Int64)TimeUnit.Unit10Minutes;
                        _textFormat = "HH:mm";
                        _textFormatSmall = "HH:mm";
                        _textDiff = 13;
                        break;

                    case TimeUnit.Unit1Hour:
                        UnitScale = ScaleUnit.Unit1Hour;
                        TicksPerPixel = TicksPerPixel.Unit1Hour;
                        _colorSplit = (Int64)TimeUnit.Unit1Hour;
                        _textFormat = "HH:mm";
                        _textFormatSmall = "HH:mm";
                        _textDiff = 13;
                        break;

                    //case TimeUnit.Unit4Hours:
                    //    UnitScale = ScaleUnit.Unit4Hours;
                    //    TicksPerPixel = TicksPerPixel.Unit4Hours;
                    //    _colorSplit = (Int64)TimeUnit.Unit4Hours;
                    //    _textFormat = "MM-dd HH:mm";
                    //    _textDiff = 29;
                    //    break;

                    case TimeUnit.Unit1Day:
                        UnitScale = ScaleUnit.Unit1Day;
                        TicksPerPixel = TicksPerPixel.Unit1Day;
                        _colorSplit = (Int64)TimeUnit.Unit1Day;
                        _textFormat = "MM-dd";
                        _textFormatSmall = "HH:mm";
                        _textDiff = 13;
                        break;
                }

                //1sec, 10sec, 1min, 10min => mini  ; 1h => 180sec
                RefreshTimer.Interval = (Int32)Math.Max((UInt64)_unitTime / 200000, MinimumReShowTime); //  /20sec

                if (_dateTime.Ticks != 0)
                {
                    //ResetBackgroundScalePosition();
                    ShowRecord();

                    UpdateTimecodeRange();
                    Invalidate();
                }

                if (OnTimeUnitChange != null)
                    OnTimeUnitChange(this, new EventArgs<TimeUnit, UInt64[]>(_unitTime,
                        new[]{
							(VisibleMinDateTime != DateTime.MinValue)
							? DateTimes.ToUtc(VisibleMinDateTime, Server.Server.TimeZone) : 0,
							(VisibleMaxDateTime != DateTime.MinValue)
							? DateTimes.ToUtc(VisibleMaxDateTime, Server.Server.TimeZone) : 0,
							(RangeStartDate != DateTime.MinValue)
							? DateTimes.ToUtc(RangeStartDate, Server.Server.TimeZone) : 0,
							(RangeEndDate != DateTime.MaxValue)
							? DateTimes.ToUtc(RangeEndDate, Server.Server.TimeZone) : 0,
						}));
            }
        }

        public UInt16 Parts
        {
            get
            {
                return DurationToParts(StartTime, EndTime);
            }
        }

        public UInt16 DurationToParts(UInt64 start, UInt64 end)
        {
            try
            {
                //new Da .ToString("yyyy-MM-dd HH:mm:ss")
                if (end > start)
                    return Convert.ToUInt16((end - start) * (UInt16)UnitScale / ((UInt64)_unitTime / 10000));
                if (end < start)
                    return Convert.ToUInt16((start - end) * (UInt16)UnitScale / ((UInt64)_unitTime / 10000));
            }
            catch (Exception)
            {
                //MessageBox.Show("start " + start + " end " + end);
            }

            return 0;
        }

        private void ShowRecord(Object sender, EventArgs e)
        {
            ShowRecord();
        }

        private DateTime _getPartStartDate = DateTime.MaxValue;
        private DateTime _getPartEndDate = DateTime.MinValue;
        public void ShowRecord()
        {
            _getPartStartDate = StartDate;
            _getPartEndDate = EndDate;

            foreach (var tracker in Trackers.Values.OfType<Tracker>())
            {
                if (tracker == null) continue;

                if (tracker.Using)
                {
                    tracker.ShowRecord();

                    if(Server is ICMS)
                        tracker.ShowDownloadPart();
                }
            }
        }

        public void UpdateContent(IDevice[] devices)
        {
            var start = Convert.ToUInt16((PageIndex - 1) * TrackerNumPerPage);
            for (UInt16 index = 0; index < MaxConnection; index++)
            {
                var inUse = true;
                //not in this page, skip
                if (index < start || index >= start + TrackerNumPerPage)
                {
                    inUse = false;
                }


                if ((index < devices.Length) && devices[index] is ICamera)
                {
                    if (Trackers.ContainsKey(index))
                    {
                        Trackers[index].Camera = (ICamera)devices[index];
                        Trackers[index].Using = inUse;
                    }
                    else
                    {
                        AppendDevice(index, devices[index]);
                        Trackers[index].Using = inUse;
                    }
                }
                else
                {
                    RemoveAt(index);
                }
            }
        }

        public virtual void AppendDevice(UInt16 index, IDevice device)
        {
            if (!(device is ICamera)) return;

            Tracker tracker;
            if (RecycleTrackers.Count > 0)
            {
                tracker = RecycleTrackers.Dequeue() as Tracker;
            }
            else
            {
                tracker = CreateTracker(Server);
            }

            if (tracker == null) return;

            tracker.Height = TrackerHeight;
            tracker.TrackerContainer = this;
            tracker.OnDragStart += TrackerOnDragStart;
            tracker.OnGetPartStart += CheckBusyStatus;
            tracker.OnGetPartCompleted += CheckBusyStatus;
            tracker.OnGetDownloadPartCompleted += TrackerOnGetDownloadPartCompleted;
            tracker.MouseUp += ContainerMouseUp;
            tracker.MouseDoubleClick += ContainerMouseDoubleClick;
            tracker.MouseDown += TrackerMouseDown;

            tracker.Using = true;
            tracker.Width = Width;

            Trackers.Add(index, tracker);
            Controls.Add(tracker);

            RefreshTimer.Enabled = false;
            RefreshTimer.Enabled = true;

            tracker.Camera = (ICamera)device;
            tracker.Position = Convert.ToUInt16(index % TrackerNumPerPage);
        }

        protected virtual Tracker CreateTracker(IServer server)
        {
            var tracker = new Tracker
            {
                Server = Server
            };

            return tracker;
        }

        public virtual void RemoveAll()
        {
            RefreshTimer.Enabled = false;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (!RecycleTrackers.Contains(tracker))
                    RecycleTrackers.Enqueue(tracker);

                tracker.Camera = null;

                tracker.OnDragStart -= TrackerOnDragStart;
                tracker.OnGetPartStart -= CheckBusyStatus;
                tracker.OnGetPartCompleted -= CheckBusyStatus;
                tracker.OnGetDownloadPartCompleted -= TrackerOnGetDownloadPartCompleted;
                tracker.MouseUp -= ContainerMouseUp;
                tracker.MouseDoubleClick -= ContainerMouseDoubleClick;
                tracker.MouseDown -= TrackerMouseDown;

                tracker.Using = false;
                tracker.Parent = null;
            }

            Trackers.Clear();
            Invalidate();
        }

        public virtual void RemoveAt(UInt16 index)
        {
            if (Trackers.ContainsKey(index))
            {
                var tracker = Trackers[index] as Tracker;

                if (!RecycleTrackers.Contains(tracker))
                    RecycleTrackers.Enqueue(tracker);

                if (tracker != null)
                {
                    tracker.Camera = null;

                    tracker.OnDragStart -= TrackerOnDragStart;
                    tracker.OnGetPartStart -= CheckBusyStatus;
                    tracker.OnGetPartCompleted -= CheckBusyStatus;
                    tracker.OnGetDownloadPartCompleted -= TrackerOnGetDownloadPartCompleted;
                    tracker.MouseUp -= ContainerMouseUp;
                    tracker.MouseDoubleClick -= ContainerMouseDoubleClick;
                    tracker.MouseDown -= TrackerMouseDown;

                    tracker.Using = false;

                    Controls.Remove(tracker);
                }

                Trackers.Remove(index);
            }

            if (Count == 0)
                RefreshTimer.Enabled = false;

            Invalidate();
        }

        public Boolean RefreshTracker
        {
            set
            {
                RefreshTimer.Enabled = value;
            }
            get
            {
                return RefreshTimer.Enabled;
            }
        }

        public void UpdateTimecodeRange()
        {
            try
            {
                Int64 range = Convert.ToInt64(Parent.Width * (Int64)TicksPerPixel / 2);
                VisibleMinDateTime = _dateTime.AddTicks(range * -1);
                VisibleMaxDateTime = _dateTime.AddTicks(range);

                if (OnDateTimeRangeChange != null)
                    OnDateTimeRangeChange(this, null);
            }
            catch (Exception)
            {
               
            }
        }

        public void AddBookmark()
        {
            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                tracker.AddBookmark();

                if (tracker.Using)
                    tracker.Invalidate();
            }
        }

        public void EraserBookmark()
        {
            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                tracker.EraserBookmark();

                if (tracker.Using)
                    tracker.Invalidate();
            }
        }

        public void BookmarkRemoved(ICamera camera)
        {
            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                    tracker.Invalidate();
            }
        }

        private Int32 _startX;
        private Int32 _moveDiff;
        private Int32 _mouseX;
        private Boolean _isDrag;

        protected void ContainerMouseDown(Object sender, MouseEventArgs e)
        {
            _isDrag = false;
            _mouseX = e.X;

            MouseMove -= ContainerMouseMove;
            MouseMove += ContainerMouseMove;
        }

        protected void ContainerMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= ContainerMouseMove;
            Capture = false;

            if (OnSelectionChange != null)
                OnSelectionChange(this, null);
        }

        protected void ContainerMouseMove(Object sender, MouseEventArgs e)
        {
            if (_isDrag)
            {
                Int32 moveValue = e.X - _startX - _moveDiff;

                if (moveValue == 0) return;

                DateTime = new DateTime(_dateTime.Ticks - Convert.ToInt64(moveValue * (Int64)TicksPerPixel));

                _moveDiff = e.X - _startX;
                return;
            }

            if (_mouseX != e.X)
            {
                _isDrag = true;
                _moveDiff = 0;
                _startX = e.X;

                if (OnSelectionChange != null)
                    OnSelectionChange(this, null);
            }
        }

        protected void TrackerOnDragStart(Object sender, EventArgs<Int32> e)
        {
            _isDrag = true;
            _moveDiff = 0;
            _startX = e.Value;
            MouseMove += ContainerMouseMove;
            Capture = true;

            if (OnSelectionChange != null)
                OnSelectionChange(this, null);
        }

        protected void CheckBusyStatus(Object sender, EventArgs e)
        {
            var isBusy = false;
            foreach (var tracker in Trackers.Values)
            {
                if (tracker.IsBusy)
                {
                    isBusy = true;
                    break;
                }
            }

            if (isBusy && OnGetPartStart != null)
                OnGetPartStart(this, null);

            if (!isBusy && OnGetPartCompleted != null)
                OnGetPartCompleted(this, null);
        }

        private void TrackerOnGetDownloadPartCompleted(object sender, EventArgs e)
        {
            if (OnGetDownloadPartCompleted != null)
                OnGetDownloadPartCompleted(this, null);
        }

        protected void ContainerMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DateTime = VisibleMinDateTime.AddTicks(e.X * (Int64)TicksPerPixel);

                if (OnSelectionChange != null)
                    OnSelectionChange(this, null);
            }
        }

        private void TrackerMouseDown(Object sender, MouseEventArgs e)
        {
            if (OnTrackerContainerMouseDown != null)
                OnTrackerContainerMouseDown(this, null);
        }

        protected Int32 _startPosition;
        public void ExportRangeStart(Int32 position, Int32 min)
        {
            if (_startPosition == position) return;

            _startPosition = position;

            RangeStartDate = (_startPosition != min)
                ? VisibleMinDateTime.AddTicks(Convert.ToInt64((_startPosition) * (Int64)TicksPerPixel))
                : DateTime.MinValue;

            Invalidate();
        }

        protected Int32 _endPosition;
        public void ExportRangeEnd(Int32 position, Int32 max)
        {
            if (_endPosition == position) return;

            _endPosition = position;

            RangeEndDate = (_endPosition != max)
                ? VisibleMinDateTime.AddTicks(Convert.ToInt64((_endPosition) * (Int64)TicksPerPixel))
                : DateTime.MaxValue;

            Invalidate();
        }

        private readonly SolidBrush _yellowBrushes = new SolidBrush(Color.FromArgb(10, Color.Gold));//Color.Yellow

        private readonly Font _scaleFont = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly SolidBrush _blackBrushe = new SolidBrush(Color.FromArgb(48, 51, 57));
        private readonly SolidBrush _grayBrushe = new SolidBrush(Color.FromArgb(90, 90, 90));
        private readonly Pen _scaledPen = new Pen(Color.FromArgb(107, 114, 125));

        public Boolean PaintTitle = true;
        protected void TrackerContainerPaint(Object sender, PaintEventArgs e)
        {
            if (!PaintTitle) return;

            if (UnitScale == 0 || _unitTime == 0) return;

            Graphics g = e.Graphics;

            g.FillRectangle(_blackBrushe, 0, 0, Width, TopPosition);
            g.DrawLine(Pens.Black, 0, TopPosition - 1, Width, TopPosition - 1);

            for (var i = 1; i <= 4; i++)
            {
                g.DrawLine(Pens.Black, 0, TopPosition + TrackerHeight * i - 1, Width, TopPosition + TrackerHeight * i - 1);
            }

            var scaleDate = VisibleMinDateTime.AddTicks((VisibleMinDateTime.Ticks % (Int64)_unitTime) * -1);

            Int32 position = TicksToX(scaleDate.Ticks);

            while (scaleDate < VisibleMaxDateTime)
            {
                //draw line (big scale) on panel
                //if (!PaintTitle)
                //	g.DrawLine(_scaledPen, position, 0, position, Height);

                g.DrawString(scaleDate.ToString(_textFormat), _scaleFont, Brushes.White, position - _textDiff, 14);

                //draw small scale on time panel
                g.DrawLine(_scaledPen, position, 32, position, 41);

                //Unit1Senond = 25,//20
                //Unit10Senonds = 50,
                //Unit1Minute = 125,
                //Unit10Minutes = 300, 
                //Unit1Hour = 1000,
                //Unit1Day = 800, 

                double start = position;
                double diff = 0;
                switch (UnitScale)
                {
                    //10 small line
                    case ScaleUnit.Unit10Senonds:
                    case ScaleUnit.Unit1Minute:
                        diff = ((UInt16)UnitScale) / 10.0;
                        for (var i = 1; i < 10; i++)
                        {
                            start += diff;
                            g.DrawLine(_scaledPen, Convert.ToInt32(start), 37, Convert.ToInt32(start), 41);
                        }
                        break;

                    //10 small line
                    case ScaleUnit.Unit10Minutes:
                        //draw extra line make it look bold
                        g.DrawLine(_scaledPen, position + 1, 32, position + 1, 41);

                        diff = ((UInt16)UnitScale) / 10.0;
                        for (var i = 1; i < 10; i++)
                        {
                            start += diff;

                            if (i % 5 == 0)
                            {
                                g.DrawString(scaleDate.AddMinutes(i).ToString(_textFormatSmall), _scaleFont, Brushes.White, Convert.ToInt32(start) - _textDiff, 14);
                                g.DrawLine(_scaledPen, Convert.ToInt32(start), 35, Convert.ToInt32(start), 41); //longer
                            }
                            else
                            {
                                g.DrawLine(_scaledPen, Convert.ToInt32(start), 37, Convert.ToInt32(start), 41);
                            }
                        }
                        break;

                    //60 small line
                    case ScaleUnit.Unit1Hour:
                        //draw extra line make it look bold
                        g.DrawLine(_scaledPen, position + 1, 32, position + 1, 41);

                        diff = ((UInt16)UnitScale) / 60.0;
                        for (var i = 1; i < 60; i++)
                        {
                            start += diff;

                            if (i % 10 == 0)
                            {
                                g.DrawString(scaleDate.AddMinutes(i).ToString(_textFormatSmall), _scaleFont, Brushes.White, Convert.ToInt32(start) - _textDiff, 14);
                                g.DrawLine(_scaledPen, Convert.ToInt32(start), 35, Convert.ToInt32(start), 41); //longer
                            }
                            else
                            {
                                g.DrawLine(_scaledPen, Convert.ToInt32(start), 37, Convert.ToInt32(start), 41);
                            }
                        }
                        break;

                    //24 small line
                    case ScaleUnit.Unit1Day:
                        //draw extra line make it look bold
                        g.DrawLine(_scaledPen, position + 1, 32, position + 1, 41);

                        diff = ((UInt16)UnitScale) / 24.0;
                        for (var i = 1; i < 24; i++)
                        {
                            start += diff;
                            g.DrawLine(_scaledPen, Convert.ToInt32(start), 37, Convert.ToInt32(start), 41);

                            if (i % 2 == 0)
                                g.DrawString(scaleDate.AddHours(i).ToString(_textFormatSmall), _scaleFont, Brushes.White, Convert.ToInt32(start) - _textDiff, 14);
                        }
                        break;
                }

                if (scaleDate > VisibleMaxDateTime)
                    break;

                position += (UInt16)UnitScale;
                scaleDate = scaleDate.AddTicks((Int64)_unitTime);
            }

            //Draw Yellow On Selection Area
            //if (Trackers.Count > 0 && RangeStartDate != RangeEndDate && (RangeEndDate > VisibleMinDateTime || RangeStartDate < VisibleMaxDateTime) &&
            //    !(RangeStartDate == DateTime.MinValue && RangeEndDate == DateTime.MaxValue))
            //{
            //    Int64 start = Math.Max(RangeStartDate.Ticks, VisibleMinDateTime.Ticks);
            //    Int64 width = Math.Min(VisibleMaxDateTime.Ticks, RangeEndDate.Ticks);

            //    g.FillRectangle(_yellowBrushes, TicksToX(start), TopPosition, TicksToWidth(width - start), Parent.Height);
            //}

            //g.DrawLine(Pens.DarkRed, Width / 2, TopPosition, Width / 2, Height);
        }

        public Int64 NextBookmark()
        {
            Int64 nextBookmarkTicks = DateTime.MaxValue.Ticks;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                    nextBookmarkTicks = Math.Min(nextBookmarkTicks, tracker.NextBookmarkTicks());
            }

            return nextBookmarkTicks;
        }

        public Int64 PreviousBookmark()
        {
            Int64 previousBookmarkTicks = DateTime.MinValue.Ticks;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                    previousBookmarkTicks = Math.Max(previousBookmarkTicks, tracker.PreviousBookmarkTicks());
            }

            return previousBookmarkTicks;
        }

        public Int64 NextRecord()
        {
            if (!Trackers.Values.Any()) return DateTime.MaxValue.Ticks;

            Int64 nextRecordTicks = DateTime.MaxValue.Ticks;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                    nextRecordTicks = Math.Min(nextRecordTicks, tracker.NextRecordTicksViaCGI());
            }

            return nextRecordTicks;
        }

        public Int64 PreviousRecord()
        {
            if (!Trackers.Values.Any()) return DateTime.MinValue.Ticks;

            Int64 previousRecordTicks = DateTime.MinValue.Ticks;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                    previousRecordTicks = Math.Max(previousRecordTicks, tracker.PreviousRecordTicksViaCGI());
            }

            return previousRecordTicks;
        }

        public Int64 BeginRecord()
        {
            if (!Trackers.Values.Any()) return DateTime.MinValue.Ticks;

            Int64 previousRecordTicks = DateTime.MaxValue.Ticks;

            foreach (Tracker tracker in Trackers.Values)
            {
                if (tracker == null) continue;

                if (tracker.Using)
                {
                    var beginRecordTicks = tracker.BeginRecordTicksViaCGI();
                    if (beginRecordTicks == DateTime.MinValue.Ticks)
                        continue;

                    previousRecordTicks = Math.Min(previousRecordTicks, beginRecordTicks);
                }
            }

            if (previousRecordTicks != DateTime.MaxValue.Ticks)
                return (new DateTime(previousRecordTicks).AddSeconds(1)).Ticks;
            
            return DateTime.MinValue.Ticks;
        }

        public Int32 TicksToX(Int64 ticks)
        {
            Int64 value = Convert.ToInt64((ticks - VisibleMinDateTime.Ticks + 0.0) / (Int64)TicksPerPixel);
            return Convert.ToInt32(Math.Min(Math.Max(value, -2147483648), 2147483647));
        }

        public Int32 TicksToWidth(Int64 ticks)
        {
            Int64 value = Convert.ToInt64((ticks + 0.0) / (Int64)TicksPerPixel);
            return Convert.ToInt32(Math.Min(Math.Max(value, -2147483648), 2147483647));
        }

        public UInt64 PartsToTime(UInt16 parts)
        {
            return Convert.ToUInt64(parts * ((UInt64)_unitTime / 10000) / (UInt16)UnitScale);
        }

        public void SearchEventAdd(EventType type)
        {
            foreach (var tracker in Trackers.Values)
            {
                if (tracker == null) continue;
                //if (!tracker.Using) continue;

                tracker.SearchEventAdd(type);
            }
        }

        public void SearchEventRemove(EventType type)
        {
            foreach (var tracker in Trackers.Values)
            {
                if (tracker == null) continue;
                if (!tracker.Using) continue;

                tracker.SearchEventRemove(type);
            }
        }
    }
}