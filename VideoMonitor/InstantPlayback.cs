using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class InstantPlayback : Form
    {
        protected VideoWindow _videoWindow;
        private VideoMenu _toolMenu;

        public Dictionary<String, String> Localization { get; set; }

        public IApp App { get; set; }
        public IServer Server { get; set; }

        private ICamera _camera;
        public ICamera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }
        public DateTime DateTime = DateTime.MinValue;
        private readonly DateTime _datetime1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        protected GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();

        public InstantPlayback()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"InstantPlayback_Title", "Instant Playback"},
                                   {"InstantPlayback_CloseAfterSec", "Close after %1 secs"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            TopMost = true;

            _autoCloseFlag.Interval = 20000;
            _autoCloseFlag.Elapsed += CloseWindow;
            _autoCloseFlag.SynchronizingObject = this;

            _countDownFlag.Interval = 1000;
            _countDownFlag.Elapsed += CountDown;
            _countDownFlag.SynchronizingObject = this;

            FormClosing += InstantPlaybackFormClosing;
        }

        public void UpdateRecordStatus()
        {
            _videoWindow.Viewer.UpdateRecordStatus();
        }

        public void Reset()
        {
            _countdown = 20;

            VisibleChanged -= InstantPlaybackShown;
            SizeChanged -= InstantPlaybackSizeChanged;

            VisibleChanged += InstantPlaybackShown;
            SizeChanged += InstantPlaybackSizeChanged;

            Application.RemoveMessageFilter(_globalMouseHandler);
            Application.AddMessageFilter(_globalMouseHandler);
        }

        protected virtual void InstantPlaybackShown(Object sender, EventArgs e)
        {
            VisibleChanged -= InstantPlaybackShown;
            Initialize();
            Goto();

            _autoCloseFlag.Enabled = true;
            _countDownFlag.Enabled = true;
        }

        protected virtual void StopCountDown()
        {
            if (!_autoCloseFlag.Enabled) return;

            _autoCloseFlag.Enabled = false;
            _countDownFlag.Enabled = false;

            Text = Camera + @" - " + Localization["InstantPlayback_Title"];// +" " + _reason;
        }

        private readonly System.Timers.Timer _autoCloseFlag = new System.Timers.Timer();
        private readonly System.Timers.Timer _countDownFlag = new System.Timers.Timer();


        protected virtual VideoWindow CreateVideoWindow()
        {
            return VideoWindowProvider.RegistVideoWindow();
        }

        public void Initialize()
        {
            if (Camera == null) return;

            if (_videoWindow == null)
                _videoWindow = CreateVideoWindow();

            _videoWindow.App = App;
            _videoWindow.Server = Server;
            _videoWindow.Initialize();

            _videoWindow.Stretch = Server.Configure.StretchPlaybackVideo;
            _videoWindow.DisplayTitleBar = true;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = GetVideoWindowParent();

            _videoWindow.Camera = Camera;

            if (DateTime == DateTime.MinValue || DateTime.Ticks == _datetime1970.AddSeconds(Server.Server.TimeZone).Ticks)
                DateTime = Server.Server.DateTime;

            _videoWindow.StartInstantPlayback(DateTime);
            _videoWindow.Track.OnTimecodeChange -= TrackOnTimecodeChange;
            _videoWindow.Track.OnTimecodeChange += TrackOnTimecodeChange;

            _videoWindow.Controller.OnPlay -= ControllerOnPlay;
            _videoWindow.Controller.OnPlay += ControllerOnPlay;

            if (_toolMenu == null)
            {
                _toolMenu = CreateVideoMenu();

                Controls.Add(_toolMenu);
                _toolMenu.BringToFront();
                _toolMenu.GenerateInstantPlaybackToolMenu();
                _toolMenu.CheckPermission();
                _toolMenu.OnButtonClick += ToolManuButtonClick;
            }

            Text = Camera + @" - " + Localization["InstantPlayback_Title"];

            _videoWindow.ToolMenu = _toolMenu;
        }

        protected virtual VideoMenu CreateVideoMenu()
        {
            var videoMenu = new VideoMenu
            {
                PanelPoint = _videoWindow.Location,
                HasPlaybackPage = true,
                Server = Server,
                App = App
            };

            return videoMenu;
        }

        //trigger by count-down timer
        private void CloseWindow(Object sender, EventArgs e)
        {
            //will trigger InstantPlaybackFormClosing
            Close();
        }

        private UInt16 _countdown = 20;
        private void CountDown(Object sender, EventArgs e)
        {
            if (!_autoCloseFlag.Enabled || _countdown <= 0 || _videoWindow == null || _videoWindow.Viewer == null) return;

            _countdown--;
            Text = Camera + @" - " + Localization["InstantPlayback_Title"] + @" (" +
                Localization["InstantPlayback_CloseAfterSec"].Replace("%1", _countdown.ToString()) + @")";

            //user trigger digital ptz
            if (_videoWindow.Viewer.IsDigitalPtzZoom)
            {
                //_reason = "DigitalPtzZoom";
                StopCountDown();
            }
        }

        protected void InstantPlaybackSizeChanged(Object sender, EventArgs e)
        {
            //_reason = "InstantPlaybackSizeChanged";
            _toolMenu.UpdateLocation();
            StopCountDown();
        }

        private void TrackOnTimecodeChange(Object sender, EventArgs<String> e)
        {
            _videoWindow.Track.OnTimecodeChange -= TrackOnTimecodeChange;
            _videoWindow.Controller.OnPlay -= ControllerOnPlay;
            //_reason = "TimecodeChange";
            StopCountDown();
        }


        private void ControllerOnPlay(Object sender, EventArgs e)
        {
            _videoWindow.Track.OnTimecodeChange -= TrackOnTimecodeChange;
            _videoWindow.Controller.OnPlay -= ControllerOnPlay;
            //_reason = "TimecodeChange";
            StopCountDown();
        }

        public void Goto()
        {
            _videoWindow.GoTo();
            _videoWindow.Active = true;
        }

        public void GlobatMouseMoveHandler()
        {
            if (_videoWindow != null)
                _videoWindow.GlobalMouseHandler();
        }

        private void ToolManuButtonClick(Object sender, EventArgs<String> e)
        {
            XmlDocument xmlDoc = Xml.LoadXml(e.Value);
            String button = Xml.GetFirstElementValueByTagName(xmlDoc, "Button");

            if (button == "") return;
            switch (button)
            {
                case "Playback":
                    if (_videoWindow.Camera != null && _videoWindow.Viewer != null && _videoWindow.Track != null)
                    {
                        App.SwitchPage("Playback", new PlaybackParameter
                        {
                            Device = _videoWindow.Camera,
                            Timecode = DateTimes.ToUtc(_videoWindow.Track.DateTime, Server.Server.TimeZone),
                            TimeUnit = _videoWindow.Track.UnitTime
                        });
                    }
                    //will trigger InstantPlaybackFormClosing
                    Close();
                    break;
            }
        }

        private void InstantPlaybackFormClosing(Object sender, FormClosingEventArgs e)
        {
            _autoCloseFlag.Enabled = false;
            _countDownFlag.Enabled = false;

            if (_videoWindow != null)
            {
                if (_videoWindow.Track != null)
                    _videoWindow.Track.OnTimecodeChange -= TrackOnTimecodeChange;

                if (_videoWindow.Controller != null)
                    _videoWindow.Controller.OnPlay -= ControllerOnPlay;

                UnregisterVideoWindow(_videoWindow);
            }

            _videoWindow = null;

            //remove from using
            if (VideoMonitor.UsingInstantPlayback.Contains(this))
                VideoMonitor.UsingInstantPlayback.Remove(this);

            //recycle
            if (!VideoMonitor.RecycleInstantPlayback.Contains(this))
                VideoMonitor.RecycleInstantPlayback.Enqueue(this);

            //dont close window, recycle use form on next
            Hide();
            e.Cancel = true;
        }

        protected virtual void UnregisterVideoWindow(VideoWindow window)
        {
            VideoWindowProvider.UnregisterVideoWindow(window);
        }

        private void InstantPlaybackActivated(Object sender, EventArgs e)
        {
            if (_globalMouseHandler != null)
            {
                _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;
                _globalMouseHandler.TheMouseMoved += GlobatMouseMoveHandler;
            }

            if (_videoWindow != null)
                _videoWindow.Active = true;
        }

        private void InstantPlaybackDeactivate(Object sender, EventArgs e)
        {
            if (_globalMouseHandler != null)
                _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;

            if (_videoWindow != null)
            {
                _videoWindow.Active = false;
                _videoWindow.Viewer.ShowRIPWindow(true);
            }


        }

        protected virtual Control GetVideoWindowParent()
        {
            return this;
        }
    }
}
