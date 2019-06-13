using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using VideoMonitor;

namespace GoogleSearch
{
    public partial class SearchCondition : UserControl, IControl, IAppUse, IServerUse, IDrop, IMouseHandler, IMinimize
    {
        public event EventHandler<EventArgs<String>> OnConditionChange;
        private const String OnConditionChangeXml =
            "<XML><Channel>{CHANNEL}</Channel><StartTime>{STARTTIME}</StartTime><EndTime>{ENDTIME}</EndTime><Spaceing>{SPACEING}</Spaceing><Limit>{LIMIT}</Limit></XML>";

        public event EventHandler<EventArgs<Object>> OnContentChange;

        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        private const String OnTimecodeChangeXml = "<XML><Timestamp>{TIMESTAMP}</Timestamp></XML>";

        public event EventHandler<EventArgs<String>> OnSearchStartDateTimeChange;
        private const String OnExportStartChangeXml = "<XML><StartTime>{STARTTIME}</StartTime></XML>";
		
        public event EventHandler<EventArgs<String>> OnSearchEndDateTimeChange;
        private const String OnExportEndChangeXml = "<XML><EndTime>{ENDTIME}</EndTime></XML>";

        public IApp App { get; set; }
        protected INVR NVR;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    NVR = value as INVR;
            }
        }

        private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();
        public String TitleName
        {
            get
            {
                return _panelTitleBar.Text;
            }
            set
            {
                _panelTitleBar.Text = value;
            }
        }

        public Image Icon
        {
            get
            {
                return Properties.Resources.icon;
            }
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public UInt16 MinimizeHeight
        {
            get
            {
                return (UInt16)titlePanel.Size.Height;
            }
        }

        public Boolean IsMinimize { get; private set; }
        public event EventHandler OnMinimizeChange;
        public void Minimize()
        {
            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        private VideoWindow _videoWindow;
        public void GlobalMouseHandler()
        {
            _videoWindow.GlobalMouseHandler();
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public void DragStop(Point point, EventArgs<Object> e)
        {
            if (!Drag.IsDrop(this, point)) return;

            if(e.Value is IDevice)
            {
                _videoWindow.DragStop(point, e);

                if (_videoWindow.Device != null)
                {
                    _videoWindow.Active = true;

                    searchButton.Enabled = true;
                    //                        if (OnContentChange != null)
                    //                            OnContentChange(this, new EventArgs<String>(OnContentChangeXml.Replace("{CONTENT}", _videoWindow.Device.Id.ToString())));
                }
            }
        }

        public void DragMove(MouseEventArgs e)
        {

        }


        private readonly Timer _getTimecodeTimer = new Timer();
        private VideoMonitor.Menu _toolMenu;

        public void Initialize()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            _panelTitleBar.Panel = this;
            titlePanel.Controls.Add(_panelTitleBar);

            _videoWindow = VideoWindowProvider.RegistVideoWindow();
            _videoWindow.Server = Server;

            _videoWindow.Stretch = false;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = videoWindowPanel;

            _getTimecodeTimer.Tick += GetTimecode;
            _getTimecodeTimer.Interval = 1000;

            _toolMenu = new VideoMonitor.Menu
            {
                PanelPoint = _videoWindow.Location,
                HasPlaybackPage = App.Pages.ContainsKey("Playback"),
                Server = Server,
            };
            videoWindowPanel.Controls.Add(_toolMenu);
            _toolMenu.BringToFront();
            _toolMenu.GeneratorSmartSearchToolMenu();

            _toolMenu.OnButtonClick += ToolManuButtonClick;

            _videoWindow.ToolMenu = _toolMenu;
            _videoWindow.NoBorder();

            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            datePicker.Value = Server.Server.DateTime;
            startTimePicker.Value = dt;
            endTimePicker.Value = dt.AddSeconds(-1);
            speacingComboBox.Text = @"120";

            if (NVR != null)
                NVR.OnDeviceModify += DeviceModify;
        }

        private void SearchConditionLoad(object sender, EventArgs e)
        {
            searchButton.PerformClick();
        }

        private void GetTimecode(Object sender, EventArgs e)
        {
            if (_videoWindow == null) return;

            if (OnTimecodeChange != null)
            {
                UInt64 timecode = _videoWindow.Viewer.Timecode;
                if (timecode == 0) return;

                OnTimecodeChange(this, new EventArgs<String>(OnTimecodeChangeXml.Replace("{TIMESTAMP}", _videoWindow.Viewer.Timecode.ToString())));
            }
        }

        private void ToolManuButtonClick(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);
            var button = Xml.GetFirstElementsValueByTagName(xmlDoc, "Button");

            if (button == "") return;

            switch (button)
            {
                case "Disconnect":
                    _videoWindow.Reset();
                    _videoWindow.Active = true;

                    searchButton.Enabled = false;
//                    if (OnContentChange != null)
//                        OnContentChange(this, new EventArgs<String>(OnContentChangeXml.Replace("{CONTENT}", "")));
                    break;

                case "Playback":
                    {
                        UInt64 timecode = _videoWindow.Viewer.Timecode;

                        App.SwitchPlayback(_videoWindow.Device,
                            ((timecode != 0)
                            ? timecode
                            : DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone))
                            , _timeunit);
                    }
                    break;
            }
        }

        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            if (_videoWindow.Device == null) return;

            if(e.Value != null)
            {
                IDevice device = e.Value;

                if (_videoWindow.Device != device) return;

                _videoWindow.Reset();
                _videoWindow.Active = true;

                searchButton.Enabled = false;
            }
        }

        private Boolean _displayTitleBar;
        public void VideoTitleBar()
        {
            _displayTitleBar = !_displayTitleBar;
            _videoWindow.DisplayTitleBar = _displayTitleBar;
        }

        public void VideoTitleBar(Object sender, EventArgs e)
        {
            VideoTitleBar();
        }

        public void AppendDevice(ICamera camera)
        {
            _videoWindow.Device = camera;

            _videoWindow.Active = true;

            searchButton.Enabled = true;

            //            if (OnContentChange != null)
            //                OnContentChange(this, new EventArgs<String>(OnContentChangeXml.Replace("{CONTENT}", _videoWindow.Device.Id.ToString())));
        }

        public void AppendDevice(Object sender, EventArgs<IDevice> e)
        {
            if (e.Value is ICamera)
                AppendDevice((ICamera)e.Value);
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            if(e.Value is ICamera)
                AppendDevice((ICamera)e.Value);
        }

        //private UInt64 _timecode;
        public void Playback(Object sender, EventArgs<UInt64> e)
        {
            Playback(e.Value);
        }

        //public void Playback()
        //{
        //    Playback(_timecode);
        //}

        public void Playback(UInt64 timecode)
        {
            //_timecode = timecode;
            _videoWindow.Playback(timecode);

            _getTimecodeTimer.Enabled = true;
        }

        private UInt64 _keyFrame;
        public void GoTo(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);
            var value = Xml.GetFirstElementsValueByTagName(xmlDoc, "Timestamp");

            if (value == "") return;

            _keyFrame = Convert.ToUInt64(value);
            GoTo(_keyFrame);
        }

        //public void GoTo()
        //{
        //    GoTo(_timecode);
        //}

        public void GoTo(UInt64 timecode)
        {
            //_timecode = timecode;
            _videoWindow.GoTo(timecode);
            _getTimecodeTimer.Enabled = false;
        }

        private TimeUnit _timeunit = TimeUnit.Unit10Senonds;
        public void TimeUnitChange(Object sender, EventArgs<TimeUnit, UInt64[]> e)
        {
            _timeunit = e.Value1;
        }

        private Boolean _fireStartDateEvent;
        private Boolean _fireEndDateEvent;
        private Boolean _isStartChange;
        private Boolean _isEndChange;
        public void ExportStartDateTimeChange(Object sender, EventArgs<String> e)
        {
            var value = Xml.GetFirstElementsValueByTagName(Xml.LoadXml(e.Value), "StartTime");

            if (value == "") return;

            _fireStartDateEvent = false;
            datePicker.Value = startTimePicker.Value = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
            _fireStartDateEvent = true;
        }

        public void ExportEndDateTimeChange(Object sender, EventArgs<String> e)
        {
            var value = Xml.GetFirstElementsValueByTagName(Xml.LoadXml(e.Value), "EndTime");

            if (value == "") return;

            _fireEndDateEvent = false;
            //datePicker.Value = endTimePicker.Value = Utility.UtcMillisecondsToDateTime(Convert.ToUInt64(value));
            endTimePicker.Value = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
            _fireEndDateEvent = true;
        }

        private void DatePickerValueChanged(object sender, EventArgs e)
        {
            StartTimePickerValueChanged(null, EventArgs.Empty);
            EndTimePickerValueChanged(null, EventArgs.Empty);
        }

        private void StartTimePickerValueChanged(object sender, EventArgs e)
        {
            var startDate = new DateTime(
                           datePicker.Value.Year, datePicker.Value.Month, datePicker.Value.Day,
                           startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second);

            if (_isEndChange)
                endTimePicker.MinDate = endTimePicker.MinDate = startDate;

            if (!_fireStartDateEvent || OnSearchStartDateTimeChange == null) return;
            _isStartChange = true;
            OnSearchStartDateTimeChange(this, new EventArgs<String>(OnExportStartChangeXml.Replace("{STARTTIME}",
                DateTimes.ToUtcString(startDate, Server.Server.TimeZone))));
        }

        private void EndTimePickerValueChanged(object sender, EventArgs e)
        {
            var endDate = new DateTime(
                datePicker.Value.Year, datePicker.Value.Month, datePicker.Value.Day,
                endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second);

            if (_isStartChange)
                datePicker.MaxDate = startTimePicker.MaxDate = endDate;

            if (!_fireEndDateEvent || OnSearchEndDateTimeChange == null) return;
            _isEndChange = true;
            OnSearchEndDateTimeChange(this, new EventArgs<String>(OnExportEndChangeXml.Replace("{ENDTIME}", DateTimes.ToUtcString(endDate, Server.Server.TimeZone))));
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            if (_videoWindow.Device == null)
                return;

            var bt = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (OnConditionChange != null)
            {
                var start = Convert.ToDateTime(datePicker.Text + ' ' + startTimePicker.Text).ToUniversalTime();
                var end = Convert.ToDateTime(datePicker.Text + ' ' + endTimePicker.Text).ToUniversalTime();
                var startUTime = Convert.ToUInt64((start - bt).TotalMilliseconds);
                var endUTime = Convert.ToUInt64((end - bt).TotalMilliseconds);

                int limit;
                int.TryParse(speedLimitText.Text, out limit);
                if (limit == 0) limit = 9999;

                var str = OnConditionChangeXml
                    .Replace("{CHANNEL}", _videoWindow.Device.Id.ToString())
                    .Replace("{STARTTIME}", startUTime.ToString())
                    .Replace("{ENDTIME}", endUTime.ToString())
                    .Replace("{SPACEING}", speacingComboBox.Text)
                    .Replace("{LIMIT}", limit.ToString());

                OnConditionChange(this, new EventArgs<String>(str));
            }

            if (OnTimecodeChange != null)
            {
                var diff = Convert.ToUInt64((datePicker.Value.ToUniversalTime() - bt).TotalMilliseconds);
                OnTimecodeChange(this,
                                 new EventArgs<String>(OnTimecodeChangeXml.Replace("{TIMESTAMP}", diff.ToString())));
            }

            if (OnContentChange != null)
                OnContentChange(this, new EventArgs<Object>(_videoWindow.Device));

        }

        private void SpeedLimitCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            var obj = sender as CheckBox;
            if (obj == null) return;

            if (obj.CheckState == CheckState.Checked)
            {
                speedLimitText.Enabled = true;
            }
            else
            {
                speedLimitText.Text = "";
                speedLimitText.Enabled = false;
            }
        }

        private void SpeedLimitTextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 48 || e.KeyChar == 49 || e.KeyChar == 50
                           || e.KeyChar == 51 || e.KeyChar == 52 || e.KeyChar == 53
                           || e.KeyChar == 54 || e.KeyChar == 55 || e.KeyChar == 56
                           || e.KeyChar == 57 || e.KeyChar == 13 || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
