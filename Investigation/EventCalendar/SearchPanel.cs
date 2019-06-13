using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ApplicationForms = PanelBase.ApplicationForms;

namespace Investigation.EventCalendar
{
    public sealed partial class SearchPanel : UserControl
    {
        public event EventHandler<EventArgs<CameraEventSearchCriteria>> OnEventSearchCriteriaChange;

        public IApp App;
        public ICMS CMS;
        public INVR NVR;
        public CameraEventSearchCriteria SearchCriteria;
        public Dictionary<String, String> Localization;

        private static readonly Image _motionOn = Resources.GetResources(Properties.Resources.motion_on, Properties.Resources.IMGMotionOn);
        private static readonly Image _diOn = Resources.GetResources(Properties.Resources.di_on, Properties.Resources.IMGDIOn);
        private static readonly Image _doOn = Resources.GetResources(Properties.Resources.do_on, Properties.Resources.IMGDOOn);
        private static readonly Image _networkLossOn = Resources.GetResources(Properties.Resources.networkloss_on, Properties.Resources.IMGNetworkLossOn);
        private static readonly Image _networkRecoveryOn = Resources.GetResources(Properties.Resources.networkrecovery_on, Properties.Resources.IMGNetworkRecoveryOn);
        private static readonly Image _videoLossOn = Resources.GetResources(Properties.Resources.videoloss_on, Properties.Resources.IMGVideoLossOn);
        private static readonly Image _videoRecoveryOn = Resources.GetResources(Properties.Resources.videorecovery_on, Properties.Resources.IMGVideoRecoveryOn);
        private static readonly Image _manualRecordOn = Resources.GetResources(Properties.Resources.manualrecord_on, Properties.Resources.IMGManualRecordOn);
        private static readonly Image _panicOn = Resources.GetResources(Properties.Resources.panic_on, Properties.Resources.IMGPanicOn);
        private static readonly Image __crossLineOn = Resources.GetResources(Properties.Resources.crossline_on, Properties.Resources.IMGCrossLineOn);
        private static readonly Image _intrusionDetectionOn = Resources.GetResources(Properties.Resources.intrusionDetection_on, Properties.Resources.IMGIntrusionDetectionOn);
        private static readonly Image _loiteringDetectionOn = Resources.GetResources(Properties.Resources.loiteringDetection_on, Properties.Resources.IMGLoiteringDetectionOn);
        private static readonly Image _objectCountingInOn = Resources.GetResources(Properties.Resources.objectCountingIn_on, Properties.Resources.IMGObjectCountingInOn);
        private static readonly Image _objectCountingOutOn = Resources.GetResources(Properties.Resources.objectCountingOut_on, Properties.Resources.IMGObjectCountingOutOn);
        private static readonly Image _audioDetection = Resources.GetResources(Properties.Resources.audioDetection_on, Properties.Resources.IMGAudioDetectionOn);
        private static readonly Image _temperingDetection = Resources.GetResources(Properties.Resources.temperingDetection_on, Properties.Resources.IMGTemperingDetectionOn);
        private static readonly Image _userdefine = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserdefined);

        public SearchPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"Common_Monday", "Monday"},
								   {"Common_Tuesday", "Tuesday"},
								   {"Common_Wednesday", "Wednesday"},
								   {"Common_Thursday", "Thursday"},
								   {"Common_Friday", "Friday"},
								   {"Common_Saturday", "Saturday"},
								   {"Common_Sunday", "Sunday"},

								   {"Common_January", "January"},
								   {"Common_February", "February"},
								   {"Common_March", "March"},
								   {"Common_April", "April"},
								   {"Common_May", "May"},
								   {"Common_June", "June"},
								   {"Common_July", "July"},
								   {"Common_August", "August"},
								   {"Common_September", "September"},
								   {"Common_October", "October"},
								   {"Common_November", "November"},
								   {"Common_December", "December"},
								   
								   {"Common_NextMonth", "Next month"},
								   {"Common_PreviousMonth", "Previous month"},
								   {"Investigation_SearchResult", "Search Result"},
								   {"Investigation_SearchNoResult", "No Result Found"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            BackgroundImage = Manager.BackgroundNoBorder;
            DoubleBuffered = true;
            Dock = DockStyle.None;

            SharedToolTips.SharedToolTip.SetToolTip(nextPictureBox, Localization["Common_NextMonth"]);
            SharedToolTips.SharedToolTip.SetToolTip(previousPictureBox, Localization["Common_PreviousMonth"]);
            monthPanel.Paint += MonthPanelPaint;

            resultLabel.Text = Localization["Investigation_SearchResult"];
            foundLabel.Text = "";
        }

        private readonly Dictionary<UInt16, CalendarCell> _calendarDays = new Dictionary<UInt16, CalendarCell>();
        public void Initialize()
        {
            var range = DateTimes.UpdateStartAndEndDateTime(NVR.Server.DateTime, NVR.Server.TimeZone, SearchCriteria.DateTimeSet);
            SearchCriteria.StartDateTime = range[0];
            SearchCriteria.EndDateTime = range[1];

            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Sunday"]), 0, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Monday"]), 1, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Tuesday"]), 2, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Wednesday"]), 3, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Thursday"]), 4, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Friday"]), 5, 0);
            weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Saturday"]), 6, 0);
        }

        private static Label GetWeekDayLabel(String weekday)
        {
            return new Label
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Text = weekday,
                BackColor = Color.FromArgb(50, 51, 53),// Color.FromArgb(141, 180, 227),
                ForeColor = Color.FromArgb(224, 225, 227),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(8, 0, 0, 0)
            };
        }

        private readonly Queue<Label> _recycleEventLabel = new Queue<Label>();
        private Label GetEventLabel(String eventName, Color color, Image image)
        {
            //add space in front for image to display
            eventName = @"            " + eventName;

            if (_recycleEventLabel.Count > 0)
            {
                var label = _recycleEventLabel.Dequeue();
                label.BackgroundImage = image;
                label.Text = eventName;
                label.BackColor = color;

                return label;
            }

            return new Label
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Text = eventName,
                BackgroundImage = image,
                BackgroundImageLayout = ImageLayout.None,
                ForeColor = Color.White,
                BackColor = color,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(3),
                Padding = new Padding(0),
                MinimumSize = new Size(125, 25),
                AutoSize = true
            };
        }

        private void MonthPanelPaint(object sender, PaintEventArgs e)
        {
            Manager.PaintTop(e.Graphics, monthPanel);
        }

        private Boolean _isSearching;

        private readonly Stopwatch _watch = new Stopwatch();

        private UInt64 _startTime;
        private UInt64 _endTime;
        private readonly Queue<ICamera> _queueSearchEventCamera = new Queue<ICamera>();
	    private int _TotalSearchCamera = 0;

        private ICamera _searchingCamera;
        public void SearchEvent(UInt64 start, UInt64 end)
        {
            if (_isSearching) return;

            if (CMS != null)
            {
                if (SearchCriteria.NVRDevice.Count == 0) return;
            }
            else
            {
                if (SearchCriteria.Device.Count == 0) return;
            }

            if (SearchCriteria.Event.Count == 0) return;

            _startTime = start;
            _endTime = end;
            _searchResult.Clear();
            ClearViewModel();

            //ApplicationForms.ShowLoadingIcon(NVR.Form);

            _isSearching = true;
            _watch.Reset();
            _watch.Start();

            _queueSearchEventCamera.Clear();
		    _TotalSearchCamera = 0;
            if (CMS != null)
            {
                foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
                {
                    if (!CMS.NVRManager.NVRs.ContainsKey(nvrDevice.NVRId)) continue;
                    var nvr = CMS.NVRManager.NVRs[nvrDevice.NVRId];
                    var device = nvr.Device.FindDeviceById(nvrDevice.DeviceId);
                    if (device == null) continue;

                    var camera = device as ICamera;
                    if (camera == null) continue;

				    _TotalSearchCamera++;
                    _queueSearchEventCamera.Enqueue(camera);
                }
            }
            else
            {
                foreach (var deviceId in SearchCriteria.Device)
                {
                    var device = NVR.Device.FindDeviceById(deviceId);
                    if (device == null) continue;

                    var camera = device as ICamera;
                    if (camera == null) continue;

                    _TotalSearchCamera++;
                    _queueSearchEventCamera.Enqueue(camera);
                }
            }

            //do search
            _watch.Reset();
            _searchResult.Clear();

            if (_queueSearchEventCamera.Count > 0)
            {
				//ApplicationForms.ShowLoadingIcon(NVR.Form);
			    ApplicationForms.ProgressBarValue = 0;
			    ApplicationForms.ShowProgressBar(NVR.Form);

                _isSearching = true;
                _watch.Start();

                CheckIfNeedSearchNextCamera();
            }
            else //nothing to search
            {
                //get result to display
                UpdateResult();
            }
        }

        private Boolean CheckIfNeedSearchNextCamera()
        {
            if (_queueSearchEventCamera.Count == 0)
            {
                _searchingCamera = null;
                return false;
            }

            ApplicationForms.ProgressBarValue = (_TotalSearchCamera - _queueSearchEventCamera.Count) * 100 / _TotalSearchCamera;

            _searchingCamera = _queueSearchEventCamera.Dequeue();
            _searchingCamera.OnSmartSearchResult -= SmartSearchResult;
            _searchingCamera.OnSmartSearchResult += SmartSearchResult;
            _searchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
            _searchingCamera.OnSmartSearchComplete += SmartSearchComplete;

            EventSearchDelegate eventSearchDelegate = _searchingCamera.EventSearch;                                                                    //period callback  delegate
            eventSearchDelegate.BeginInvoke(_startTime, _endTime, SearchCriteria.Event, null, null, null);

            return true;
        }

        private delegate void SmartSearchResultDelegate(Object sender, EventArgs<String> e);
        public void SmartSearchResult(Object sender, EventArgs<String> e)
        {
            if (!_isSearching) return;

            if (InvokeRequired)
            {
                Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
                return;
            }

            var xmlDoc = Xml.LoadXml(e.Value);

            var rootNode = Xml.GetFirstElementByTagName(xmlDoc, "SmartSearch");
            var deviceIdStr = rootNode.GetAttribute("Id");
            if (String.IsNullOrEmpty(deviceIdStr)) return;
            var deviceId = Convert.ToUInt16(deviceIdStr);
            if (CMS != null)
            {
                var nvrIdStr = rootNode.GetAttribute("nvrId");
                if (String.IsNullOrEmpty(nvrIdStr)) return;
                var nvrId = Convert.ToUInt16(nvrIdStr);
                if (!CMS.NVRManager.NVRs.ContainsKey(nvrId)) return;
                var nvr = CMS.NVRManager.NVRs[nvrId];
                var device = nvr.Device.FindDeviceById(deviceId);
                if (device == null) return;
            }
            else
            {
                var device = NVR.Device.FindDeviceById(deviceId);
                if (device == null) return;
            }

            var times = xmlDoc.GetElementsByTagName("Time");

            //Parse search result
            foreach (XmlElement time in times)
            {
                EventCount eventCount = null;
                var type = (EventType)Enum.Parse(typeof(EventType), time.GetAttribute("type"), true);
                var dateTime = DateTimes.ToDateTime(Convert.ToUInt64(time.InnerText), NVR.Server.TimeZone).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                foreach (var count in _searchResult)
                {
                    if (count.EventType != type) continue;
                    if (count.DateTime != dateTime) continue;

                    eventCount = count;
                    break;
                }

                if (eventCount == null)
                {
                    eventCount = new EventCount
                    {
                        EventType = type,
                        DateTime = dateTime,
                        Color = CameraEventSearchCriteria.GetEventTypeDefaultColor(type)
                    };
                    _searchResult.Add(eventCount);
                }
                eventCount.Count++;
            }
        }

        private delegate void EventSearchDelegate(UInt64 startTime, UInt64 endTime, List<EventType> events, List<UInt64> period);
        private delegate void SmartSearchCompleteDelegate(Object sender, EventArgs e);
        public void SmartSearchComplete(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new SmartSearchCompleteDelegate(SmartSearchComplete), sender, e);
                return;
            }
		    ApplicationForms.ProgressBarValue = 99;

            if (_searchingCamera != null)
            {
                _searchingCamera.OnSmartSearchResult -= SmartSearchResult;
                _searchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
            }

            if (CheckIfNeedSearchNextCamera())
                return;

            _watch.Stop();

            //get result to display
            UpdateResult();
        }

        private List<EventCount> _searchResult = new List<EventCount>();
        private void UpdateResult()
        {
            if (_searchResult == null) return;

            ClearViewModel();
            if (_searchResult.Count == 0)
            {
                foundLabel.Text = Localization["Investigation_SearchNoResult"] + @" ";
            }
            else
            {
                foundLabel.Text = "";
            }
            var events = new List<EventType>();
            events.AddRange(SearchCriteria.Event);
            events.Sort((x, y) => (x.CompareTo(y)));

            foreach (var eventType in events)
            {
                Image image = null;
                switch (eventType)
                {
                    case EventType.Motion:
                        image = _motionOn;
                        break;

                    case EventType.DigitalInput:
                        image = _diOn;
                        break;

                    case EventType.DigitalOutput:
                        image = _doOn;
                        break;

                    case EventType.NetworkLoss:
                        image = _networkLossOn;
                        break;

                    case EventType.NetworkRecovery:
                        image = _networkRecoveryOn;
                        break;

                    case EventType.VideoLoss:
                        image = _videoLossOn;
                        break;

                    case EventType.VideoRecovery:
                        image = _videoRecoveryOn;
                        break;

                    case EventType.ManualRecord:
                        image = _manualRecordOn;
                        break;

                    case EventType.Panic:
                        image = _panicOn;
                        break;

                    case EventType.CrossLine:
                        image = __crossLineOn;
                        break;

                    case EventType.IntrusionDetection:
                        image = _intrusionDetectionOn;
                        break;

                    case EventType.LoiteringDetection:
                        image = _loiteringDetectionOn;
                        break;

                    case EventType.ObjectCountingIn:
                        image = _objectCountingInOn;
                        break;

                    case EventType.ObjectCountingOut:
                        image = _objectCountingOutOn;
                        break;

                    case EventType.AudioDetection:
                        image = _audioDetection;
                        break;

                    case EventType.TamperDetection:
                        image = _temperingDetection;
                        break;
                    case EventType.UserDefine:
                        image = _userdefine;
                        break;
                }
                eventFlowLayoutPanel.Controls.Add(GetEventLabel(CameraEventSearchCriteria.EventTypeToLocalizationString(eventType),
                    CameraEventSearchCriteria.GetEventTypeDefaultColor(eventType), image));
            }

            //foundLabel.Text += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
            var dateTime = DateTimes.ToDateTime(_startTime, NVR.Server.TimeZone);
            var month = "";
            switch (dateTime.Month)
            {
                case 1:
                    month = Localization["Common_January"];
                    break;

                case 2:
                    month = Localization["Common_February"];
                    break;

                case 3:
                    month = Localization["Common_March"];
                    break;

                case 4:
                    month = Localization["Common_April"];
                    break;

                case 5:
                    month = Localization["Common_May"];
                    break;

                case 6:
                    month = Localization["Common_June"];
                    break;

                case 7:
                    month = Localization["Common_July"];
                    break;

                case 8:
                    month = Localization["Common_August"];
                    break;

                case 9:
                    month = Localization["Common_September"];
                    break;

                case 10:
                    month = Localization["Common_October"];
                    break;

                case 11:
                    month = Localization["Common_November"];
                    break;

                case 12:
                    month = Localization["Common_December"];
                    break;

            }
            monthLabel.Text = dateTime.Year + @" " + month;

            _isSearching = false;

            UpdateCell(dateTime);

			//ApplicationForms.HideLoadingIcon();
            ApplicationForms.HideProgressBar();

        }

        public void UpdateCell(DateTime dateTime)
        {
            _calendarDays.Clear();

            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var offset = 0;
            switch (firstDayOfTheMonth.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    offset = 0;
                    break;

                case DayOfWeek.Monday:
                    offset = 1;
                    break;

                case DayOfWeek.Tuesday:
                    offset = 2;
                    break;

                case DayOfWeek.Wednesday:
                    offset = 3;
                    break;

                case DayOfWeek.Thursday:
                    offset = 4;
                    break;

                case DayOfWeek.Friday:
                    offset = 5;
                    break;

                case DayOfWeek.Saturday:
                    offset = 6;
                    break;
            }

            var days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            //UInt16 start = 1;

            var rows = Convert.ToUInt16(Math.Ceiling((days - (7 - offset)) / 7.0) + 1);
            var height = (float)100.0 / rows;
            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.RowStyles.Clear();
            for (int i = 0; i < rows; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, height));
            }

            var thisMonth = dateTime.Month;

            //var dateTimeUtc = DateTimes.ToUtc(dateTime, PTS.Server.TimeZone);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    var control = GetCalendarCell();
                    tableLayoutPanel.Controls.Add(control, x, y);

                    if (thisMonth != dateTime.Month)
                    {
                        control.Day = 0;
                        control.IsToday = false;
                        control.Events = null;
                        continue;
                    }

                    if (offset > 0)
                    {
                        control.Day = 0;
                        control.IsToday = false;
                        control.Events = null;
                        offset--;
                        continue;
                    }

                    if (dateTime.Day > days)
                    {
                        control.Day = 0;
                        control.IsToday = false;
                        control.Events = null;
                        continue;
                    }

                    var events = new List<EventCount>();
                    foreach (var exceptionCount in _searchResult)
                    {
                        if (exceptionCount.DateTime == dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                        {
                            events.Add(exceptionCount);
                        }
                    }

                    //sort by count
                    //events.Sort((a, b) => ((b.Count != a.Count) ? b.Count - a.Count : b.EventType.CompareTo(a.EventType)));

                    //sort by event order
                    events.Sort((a, b) => (a.EventType.CompareTo(b.EventType)));

                    control.Events = events;

                    control.Day = Convert.ToUInt16(dateTime.Day);
                    control.IsToday = (dateTime.Year == NVR.Server.DateTime.Year && dateTime.Month == NVR.Server.DateTime.Month &&
                                       dateTime.Date == NVR.Server.DateTime.Date);

                    control.StartDateTime = DateTimes.ToUtc(dateTime, NVR.Server.TimeZone);//dateTimeUtc + (Convert.ToUInt64((dateTime.Day - 1) * 86400) * 1000);
                    control.EndDateTime = DateTimes.ToUtc(dateTime.AddHours(24).AddMilliseconds(-1), NVR.Server.TimeZone);//control.StartDateTime + 86399000;
                    dateTime = dateTime.AddDays(1);
                }
            }
        }

        public void ClearViewModel()
        {
            monthLabel.Text = "";
            foundLabel.Text = "";

            foreach (CalendarCell control in tableLayoutPanel.Controls)
            {
                if (!_recycleCalendarCell.Contains(control))
                    _recycleCalendarCell.Enqueue(control);
            }

            tableLayoutPanel.Controls.Clear();

            foreach (Label label in eventFlowLayoutPanel.Controls)
            {
                if (!_recycleEventLabel.Contains(label))
                    _recycleEventLabel.Enqueue(label);
            }
            eventFlowLayoutPanel.Controls.Clear();
        }

        private void PreviousPictureBoxClick(Object sender, EventArgs e)
        {
            var month = DateTimes.ToDateTime(_startTime, NVR.Server.TimeZone);
            month = month.AddMonths(-1);
            _startTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, 1), NVR.Server.TimeZone);
            _endTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 23, 59, 59, 999), NVR.Server.TimeZone);

            SearchEvent(_startTime, _endTime);
        }

        private void NextPictureBoxClick(Object sender, EventArgs e)
        {
            var month = DateTimes.ToDateTime(_startTime, NVR.Server.TimeZone);
            month = month.AddMonths(1);
            _startTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, 1), NVR.Server.TimeZone);
            _endTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 23, 59, 59, 999), NVR.Server.TimeZone);

            SearchEvent(_startTime, _endTime);
        }

        private readonly Queue<CalendarCell> _recycleCalendarCell = new Queue<CalendarCell>();
        private CalendarCell GetCalendarCell()
        {
            if (_recycleCalendarCell.Count > 0)
                return _recycleCalendarCell.Dequeue();

            var cell = new CalendarCell();

            //cell.OnSelectionChange += CellOnSelectionChange;
            cell.OnEventSearch += CellOnEventSearch;

            return cell;
        }

        private void CellOnEventSearch(Object sender, EventArgs<EventType, UInt64, UInt64> e)
        {
            if (OnEventSearchCriteriaChange != null)
            {
                var searchCriteria = new CameraEventSearchCriteria();
                if (CMS != null)
                {
                    searchCriteria.NVRDevice.AddRange(SearchCriteria.NVRDevice);
                }
                else
                {
                    searchCriteria.Device.AddRange(SearchCriteria.Device);
                }

                searchCriteria.Event.Add(e.Value1);
                searchCriteria.StartDateTime = e.Value2;
                searchCriteria.EndDateTime = e.Value3;

                OnEventSearchCriteriaChange(this, new EventArgs<CameraEventSearchCriteria>(searchCriteria));
            }
        }
    }

    public sealed class CalendarCell : Panel
    {
        public event EventHandler<EventArgs<EventType, UInt64, UInt64>> OnEventSearch;
        private readonly Label _dayLabel;
        private readonly DoubleBufferFlowLayoutPanel _resultPanel;

        public UInt64 StartDateTime;
        public UInt64 EndDateTime;

        public UInt16 Day
        {
            set
            {
                if (value == 0)
                {
                    _dayLabel.Text = "";
                    //_dayLabel.BackColor = Color.FromArgb(229, 229, 229);
                    return;
                }
                _dayLabel.Text = value.ToString();
                //keep white
                //_dayLabel.BackColor = Color.FromArgb(219, 229, 241);
            }
        }

        public List<EventCount> Events
        {
            set
            {
                foreach (Label label in _resultPanel.Controls)
                {
                    label.Click -= LabelClick;
                    //暫時取消, 因為她會一直掛上ToolTips在同一個Label , 而無法UNBIND
                    //SharedToolTips.SharedToolTip.ReleaseToolTip(label);
                    //if (!_recycleEventLabel.Contains(label))
                    //    _recycleEventLabel.Enqueue(label);
                }
                _resultPanel.Controls.Clear();

                //_listBox.Items.Clear();
                if (value == null) return;

                foreach (var eventCount in value)
                {
                    _resultPanel.Controls.Add(GetEventLabel(eventCount.Count, eventCount.Color, eventCount.EventType));
                }
            }
        }

        public Boolean IsToday
        {
            set
            {
                if (value)
                {
                    BackColor = Color.FromArgb(219, 241, 251);
                }
                else
                {
                    if (String.IsNullOrEmpty(_dayLabel.Text))
                        BackColor = Color.FromArgb(238, 239, 242);
                    else
                        BackColor = Color.White;
                }
            }
        }

        private static readonly Queue<Label> _recycleEventLabel = new Queue<Label>();
        private Label GetEventLabel(Int32 count, Color color, EventType eventType)
        {
            Label label = null;
            if (_recycleEventLabel.Count > 0)
            {
                label = _recycleEventLabel.Dequeue();
                label.Text = count.ToString();//"99999"
                label.Tag = eventType;//"99999"
                label.BackColor = color;
                label.Click += LabelClick;

                return label;
            }

            label = new Label
            {
                Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Text = count.ToString(),//"99999"
                ForeColor = Color.White,
                BackColor = color,
                Tag = eventType,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0),
                Padding = new Padding(0),
                MinimumSize = new Size(44, 25),
                Size = new Size(44, 25),
                Cursor = Cursors.Hand,
                AutoSize = true,
            };

            SharedToolTips.SharedToolTip.SetToolTip(label, CameraEventSearchCriteria.EventTypeToLocalizationString(eventType) + " : " + count);
            label.Click += LabelClick;

            return label;
        }

        public CalendarCell()
        {
            DoubleBuffered = true;
            Margin = new Padding(0);
            Dock = DockStyle.Fill;
            //BorderStyle = BorderStyle.FixedSingle;

            _dayLabel = new Label
            {
                BackColor = Color.Transparent,
                Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0),
                Padding = new Padding(8, 0, 0, 0),
                Height = 20,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.None,
            };

            _resultPanel = new DoubleBufferFlowLayoutPanel
            {
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                AutoScroll = true

            };

            Controls.Add(_resultPanel);
            Controls.Add(_dayLabel);
        }

        private void LabelClick(Object sender, EventArgs e)
        {
            var label = sender as Label;
            if (label == null) return;

            if (OnEventSearch != null)
                OnEventSearch(this, new EventArgs<EventType, UInt64, UInt64>((EventType)label.Tag, StartDateTime, EndDateTime));
        }
    }
}
