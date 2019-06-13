using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using DeviceConstant;
using Interface;
using VideoMonitor;

namespace SmartSearch
{
    public partial class SmartSearch : UserControl, IControl, IAppUse, IServerUse, IDrop, IMouseHandler, IKeyPress
    {
        public event EventHandler<EventArgs<Object>> OnContentChange;
        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler<EventArgs<Boolean>> OnTitleBarVisibleChange;
        public event EventHandler<EventArgs<Int32>> OnViewingDeviceNumberChange;

        public SearchCondition SearchConditionControl;

        public String TitleName { get; set; }

        public IApp App { get; set; }
        private INVR _nvr;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    _nvr = value as INVR;
            }
        }

        public VideoWindow VideoWindow { get; set; }
        public VideoMenu ToolMenu;
        public readonly System.Timers.Timer GetTimecodeTimer = new System.Timers.Timer();

        public SmartSearch()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
        }

        public Boolean StretchStatus;//= false;

        public virtual void Initialize()
        {
            VideoWindow = VideoWindowProvider.RegistVideoWindow();
            VideoWindow.Server = Server;
            VideoWindow.App = App;
            VideoWindow.Initialize();

            StretchStatus = Server.Configure.StretchPlaybackVideo;

            VideoWindow.DisplayTitleBar = false;
            VideoWindow.Stretch = Server.Configure.StretchPlaybackVideo;
            VideoWindow.Dock = DockStyle.Fill;
            VideoWindow.Parent = videoWindowPanel;

            GetTimecodeTimer.Elapsed += GetTimecode;
            GetTimecodeTimer.Interval = 1000;
            GetTimecodeTimer.SynchronizingObject = Server.Form;

            ToolMenu = new VideoMenu
            {
                PanelPoint = VideoWindow.Location,
                HasPlaybackPage = App.Pages.ContainsKey("Playback"),
                Server = Server,
                App = App
            };
            videoWindowPanel.Controls.Add(ToolMenu);

            ToolMenu.BringToFront();
            ToolMenu.GenerateSmartSearchToolMenu();

            ToolMenu.OnButtonClick += ToolManuButtonClick;

            VideoWindow.ToolMenu = ToolMenu;
            VideoWindow.NoBorder();

            if (_nvr != null)
                _nvr.OnDeviceModify += DeviceModify;

            App.OnCustomVideoStream += AppOnCustomVideoStream;
        }


        private void AppOnCustomVideoStream(Object sender, EventArgs e)
        {
            if (!_isActivate) return;

            if (!VideoWindow.Visible) return;
            if (VideoWindow.Camera is IDeviceLayout || VideoWindow.Camera is ISubLayout) return;

            VideoWindow.Reconnect();
        }

        private UInt64 _timecode;
        public void Playback()
        {
            Playback(_timecode);
        }

        public void Playback(UInt64 timecode)
        {
            _timecode = timecode;
            VideoWindow.Playback(timecode);

            GetTimecodeTimer.Enabled = true;
        }

        public void Playback(Object sender, EventArgs<UInt64> e)
        {
            VideoWindow.SetPlaySpeed(1);
            Playback(e.Value);
        }

        public void PlaybackOnRate(Object sender, EventArgs<UInt64, UInt16> e)
        {
            VideoWindow.SetPlaySpeed(e.Value2);
            Playback(e.Value1);
        }

        public void GoTo()
        {
            GoTo(_timecode);
        }

        public void GoTo(UInt64 timecode)
        {
            _timecode = timecode;

            if (SearchConditionControl.IsIVSMotionChecked)
                VideoWindow.Viewer.EnableMotionDetection(true);

            VideoWindow.Visible = true;
            VideoWindow.GoTo(timecode);

            GetTimecodeTimer.Enabled = false;
        }

        public void GoTo(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);
            var value = Xml.GetFirstElementValueByTagName(xmlDoc, "Timestamp");

            if (value != "")
            {
                GoTo(Convert.ToUInt64(value));
            }
        }

        private void GetTimecode(Object sender, EventArgs e)
        {
            if (VideoWindow == null) return;

            UInt64 timecode = VideoWindow.Viewer.Timecode;
            if (timecode == 0) return;

            if (Server.Server.TimeZone != VideoWindow.Camera.Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(timecode);
                time += (VideoWindow.Camera.Server.Server.TimeZone * 1000);
                time -= (Server.Server.TimeZone * 1000);
                timecode = Convert.ToUInt64(time);
            }

            _timecode = timecode;
            if (OnTimecodeChange != null)
                OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(timecode.ToString())));
        }

        private void ToolManuButtonClick(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);
            var button = Xml.GetFirstElementValueByTagName(xmlDoc, "Button");

            if (button != "")
            {
                switch (button)
                {
                    case "Disconnect":
                        Disconnect();
                        break;

                    case "Playback":
                        if (VideoWindow.Camera != null && VideoWindow.Viewer != null)
                        {
                            UInt64 timecode = VideoWindow.Viewer.Timecode;

                            if (timecode == 0)
                            {
                                timecode = DateTimes.ToUtc(Server.Server.DateTime.AddSeconds(Server.Configure.InstantPlaybackSeconds), Server.Server.TimeZone);
                            }
                            else
                            {
                                if (Server.Server.TimeZone != VideoWindow.Camera.Server.Server.TimeZone)
                                {
                                    Int64 time = Convert.ToInt64(timecode);
                                    time += (VideoWindow.Camera.Server.Server.TimeZone * 1000);
                                    time -= (Server.Server.TimeZone * 1000);
                                    timecode = Convert.ToUInt64(time);
                                }
                            }
                            //UInt64 timecode = _keyFrame;
                            App.SwitchPage("Playback", new PlaybackParameter
                            {
                                Device = VideoWindow.Camera,
                                Timecode = timecode,
                                TimeUnit = SearchConditionControl.Timeunit
                            });
                        }
                        break;
                }
            }
        }

        private Boolean _displayTitleBar;
        public void VideoTitleBar()
        {
            _displayTitleBar = !_displayTitleBar;
            VideoWindow.DisplayTitleBar = _displayTitleBar;

            if (OnTitleBarVisibleChange != null)
                OnTitleBarVisibleChange(this, new EventArgs<Boolean>(_displayTitleBar));
        }

        public void VideoTitleBar(Object sender, EventArgs e)
        {
            VideoTitleBar();
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            if (e.Value is IDevice)
                AppendDevice((IDevice)e.Value);
            else if (e.Value is IDeviceGroup)
                ShowGroup((IDeviceGroup)e.Value);
        }

        public void AppendDevice(Object sender, EventArgs<IDevice> e)
        {
            if (e.Value is ICamera)
            {
                AppendDevice(e.Value);
            }
        }

        public virtual void AppendDevice(IDevice device)
        {
            if (device is ICamera)
            {
                SearchConditionControl.IsSearch = false;
                if (SearchConditionControl.SearchingCamera != null)
                    SearchConditionControl.SearchingCamera.StopSearch();

                if (SearchConditionControl.IsIVSMotionChecked)
                {
                    VideoWindow.Viewer.PtzMode = PTZMode.Disable;
                    VideoWindow.Viewer.EnableMotionDetection(true);
                }
                else
                {
                    VideoWindow.Viewer.EnableMotionDetection(false);
                    VideoWindow.Viewer.PtzMode = PTZMode.Digital;
                }
                VideoWindow.Camera = (ICamera)device;

                VideoWindow.Stretch = Server.Configure.StretchPlaybackVideo;

                VideoWindow.Active = true;

                if (Server is ICMS)
                    VideoWindow.ViewerDoubleBufferPanelSizeChanged();

                SearchConditionControl.StopButtonEnabled = true;

                SearchConditionControl.MotionAreas.Clear();
                if (OnContentChange != null)
                    OnContentChange(this, new EventArgs<Object>(VideoWindow.Camera));

                if (OnViewingDeviceNumberChange != null)
                    OnViewingDeviceNumberChange(this, new EventArgs<Int32>(1));
            }
        }

        public void ShowGroup(Object sender, EventArgs<IDeviceGroup> e)
        {
            if (e.Value != null)
                ShowGroup(e.Value);
        }

        public void ShowGroup(IDeviceGroup group)
        {
            if (group != null && group.Items.Count > 0)
            {
                group.Items.Sort((x, y) => (x.Id - y.Id));
                IDevice device = group.Items[0];
                if (!(device is ICamera)) return;

                SearchConditionControl.IsSearch = false;
                if (SearchConditionControl.SearchingCamera != null)
                    SearchConditionControl.SearchingCamera.StopSearch();

                VideoWindow.Viewer.EnableMotionDetection(true);
                VideoWindow.Camera = (ICamera)device;

                VideoWindow.Stretch = Server.Configure.StretchPlaybackVideo;

                VideoWindow.Active = true;

                SearchConditionControl.StopButtonEnabled = true;

                SearchConditionControl.MotionAreas.Clear();
                if (OnContentChange != null)
                    OnContentChange(this, new EventArgs<Object>(VideoWindow.Camera));

                if (OnViewingDeviceNumberChange != null)
                    OnViewingDeviceNumberChange(this, new EventArgs<Int32>(1));
            }
        }

        public void ShowNVR(Object sender, EventArgs<INVR> e)
        {
            if (e.Value != null)
            {
                if (e.Value.ReadyState != ReadyState.Ready && e.Value.ReadyState != ReadyState.Modify) return;

                if (e.Value.Device.Groups.Count > 0)
                {
                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in e.Value.Device.Groups)
                    {
                        if (obj.Value.Items.Count > 0)
                        {
                            ShowGroup(obj.Value);
                            break;
                        }
                    }
                }
            }
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public virtual void DragStop(Point point, EventArgs<Object> e)
        {
            if (!Drag.IsDrop(this, point)) return;

            if (e.Value is IDevice)
            {
                var device = VideoWindow.Camera;
                VideoWindow.DragStop(point, e);

                if (device != VideoWindow.Camera)
                {
                    SearchConditionControl.IsSearch = false;
                    if (SearchConditionControl.SearchingCamera != null)
                        SearchConditionControl.SearchingCamera.StopSearch();

                    VideoWindow.Active = true;
                    VideoWindow.Stretch = Server.Configure.StretchPlaybackVideo;

                    SearchConditionControl.StopButtonEnabled = true;

                    SearchConditionControl.MotionAreas.Clear();
                    if (OnContentChange != null)
                        OnContentChange(this, new EventArgs<Object>(VideoWindow.Camera));
                }
            }
            else if (e.Value is IDeviceGroup)
            {
                ShowGroup((IDeviceGroup)e.Value);
            }
        }

        public void DragMove(MouseEventArgs e)
        {
        }

        private static String TimecodeChangeXml(String timestamp)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Timestamp", timestamp));

            return xmlDoc.InnerXml;
        }

        private Boolean _isActivate;
        public void Activate()
        {
            _isActivate = true;
            if (VideoWindow.Camera == null)
            {
                if (OnViewingDeviceNumberChange != null)
                    OnViewingDeviceNumberChange(this, new EventArgs<Int32>(0));
                return;
            }

            if (SearchConditionControl.IsIVSMotionChecked)
                VideoWindow.Viewer.EnableMotionDetection(true);
            else
                VideoWindow.Viewer.PtzMode = PTZMode.Digital;

            //trigger device number change for custom bitrate control
            if (OnViewingDeviceNumberChange != null)
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(1));

            VideoWindow.Activate();
            VideoWindow.Active = true;

            if (StretchStatus != Server.Configure.StretchPlaybackVideo)
            {
                StretchStatus = Server.Configure.StretchPlaybackVideo;
                VideoWindow.Stretch = Server.Configure.StretchPlaybackVideo;
            }

            if (Server is ICMS)
                VideoWindow.ViewerDoubleBufferPanelSizeChanged();
        }

        public void Deactivate()
        {
            _isActivate = false;

            VideoWindow.Deactivate();
        }

        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            if (VideoWindow.Camera == null) return;

            if (e.Value != null)
            {
                IDevice device = e.Value;

                if (VideoWindow.Camera == device)
                {
                    SearchConditionControl.IsSearch = false;
                    if (SearchConditionControl.SearchingCamera != null)
                        SearchConditionControl.SearchingCamera.StopSearch();

                    VideoWindow.Reset();
                    VideoWindow.Active = true;

                    SearchConditionControl.StopButtonEnabled = false;

                    SearchConditionControl.MotionAreas.Clear();
                    if (OnContentChange != null)
                        OnContentChange(this, new EventArgs<Object>(null));
                }
            }
        }

        public void GlobalMouseHandler()
        {
            if (VideoWindow.Active)
                VideoWindow.GlobalMouseHandler();
        }

        public delegate void ServerTimeZoneChangeDelegate(Object sender, EventArgs e);
        public void ServerTimeZoneChange(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new ServerTimeZoneChangeDelegate(ServerTimeZoneChange), sender, e);
                return;
            }
            if (VideoWindow.Viewer.NetworkStatus != NetworkStatus.Idle)
                VideoWindow.Viewer.TimeZone = Server.Server.TimeZone;
        }

        public void Disconnect(Object sender, EventArgs e)
        {
            Disconnect();
        }

        public void Disconnect()
        {
            VideoWindow.Reset();
            VideoWindow.Active = true;

            if (OnContentChange != null)
                OnContentChange(this, new EventArgs<Object>(null));
        }

        public void KeyboardPress(Keys keyData)
        {
            if (VideoWindow == null) return;
            if (VideoWindow.Camera == null) return;
            if (VideoWindow.PlayMode != PlayMode.Playback1X && VideoWindow.PlayMode != PlayMode.GotoTimestamp) return;

            switch (keyData)
            {
                case Keys.Add:
                    VideoWindow.Viewer.AdjustBrightness += 10;
                    break;

                case Keys.Subtract:
                    VideoWindow.Viewer.AdjustBrightness -= 10;
                    break;
            }
        }

        public void NextFrame(Object sender, EventArgs e)
        {
            GetTimecodeTimer.Enabled = false;

            VideoWindow.Viewer.OnFrameTimecodeUpdate -= UpdateWindowTimecode;
            VideoWindow.Viewer.OnFrameTimecodeUpdate += UpdateWindowTimecode;
            VideoWindow.NextFrame();
        }

        public void PreviousFrame(Object sender, EventArgs e)
        {
            GetTimecodeTimer.Enabled = false;

            VideoWindow.Viewer.OnFrameTimecodeUpdate -= UpdateWindowTimecode;
            VideoWindow.Viewer.OnFrameTimecodeUpdate += UpdateWindowTimecode;
            VideoWindow.PreviousFrame();
        }

        private void UpdateWindowTimecode(Object sender, EventArgs<String> e)
        {
            if (VideoWindow == null || VideoWindow.Camera == null) return;

            VideoWindow.Viewer.OnFrameTimecodeUpdate -= UpdateWindowTimecode;

            if (OnTimecodeChange != null)
                OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(VideoWindow.GetTimecode().ToString())));
        }
    }
}
