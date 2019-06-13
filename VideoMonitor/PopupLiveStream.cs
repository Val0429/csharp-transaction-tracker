using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class PopupLiveStream : Form
    {
        private VideoWindow _videoWindow;
        private IVideoMenu _toolMenu;

        public Dictionary<String, String> Localization;

        public IApp App;
        public IServer Server { get; set; }
        public ICamera Camera;
        public UInt16 Preset = 0;

        public PopupLiveStream()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"LiveStream_Title", "Live Stream"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            Application.AddMessageFilter(_globalMouseHandler);

            SizeChanged += LiveStreamSizeChanged;

            FormClosing += LiveStreamFormClosing;
        }

        public void UpdateRecordStatus()
        {
            _videoWindow.Viewer.UpdateRecordStatus();
        }

        public void Reset()
        {
            VisibleChanged -= LiveStreamShown;
            VisibleChanged += LiveStreamShown;

            Application.RemoveMessageFilter(_globalMouseHandler);
            Application.AddMessageFilter(_globalMouseHandler);
        }

        private void LiveStreamSizeChanged(Object sender, EventArgs e)
        {
            _toolMenu.UpdateLocation();
        }

        private void LiveStreamShown(Object sender, EventArgs e)
        {
            VisibleChanged -= LiveStreamShown;
            Initialize();
            Play();
        }

        public void Initialize()
        {
            if (Camera == null) return;

            if (_videoWindow == null)
                _videoWindow = CreateVideoWindow();

            _videoWindow.App = App;
            _videoWindow.Server = Server;
            _videoWindow.Initialize();

            _videoWindow.Stretch = Server.Configure.StretchLiveVideo;
            _videoWindow.DisplayTitleBar = true;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = this;

            _videoWindow.Camera = Camera;
            _videoWindow.Play();

            if (_toolMenu == null)
            {
                _toolMenu = CreateVideoMenu(_videoWindow.Location, App.Pages.ContainsKey("Playback"), Server);

                var toolMenu = _toolMenu as Control;
                if (toolMenu != null)
                {
                    Controls.Add(toolMenu);
                    toolMenu.BringToFront();
                }
                _toolMenu.GenerateLiveToolMenu();
                _toolMenu.CheckPermission();

                _toolMenu.OnButtonClick += ToolManuButtonClick;
            }

            Text = Camera + @" - " + Localization["LiveStream_Title"];

            _videoWindow.ToolMenu = _toolMenu;
        }

        protected virtual VideoWindow CreateVideoWindow()
        {
            return VideoWindowProvider.RegistVideoWindow();
        }

        protected virtual void UnregisterVideoWindow(VideoWindow window)
        {
            VideoWindowProvider.UnregisterVideoWindow(window);
        }

        protected virtual IVideoMenu CreateVideoMenu(Point panelPoint, bool hasPlaybackPage, IServer server)
        {
            return new VideoMenu
            {
                PanelPoint = panelPoint,
                HasPlaybackPage = hasPlaybackPage,
                Server = server,
                App = App,
            };
        }

        public void Play()
        {
            _videoWindow.Play();
            _videoWindow.Active = true;

            if (Preset != 0)
                Camera.PresetPointGo = Preset;
        }

        private readonly GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();

        public void GlobatMouseMoveHandler()
        {
            if (_videoWindow != null)
                _videoWindow.GlobalMouseHandler();
        }

        private void ToolManuButtonClick(Object sender, EventArgs<String> e)
        {
            XmlDocument xmlDoc = Xml.LoadXml(e.Value);
            String button = Xml.GetFirstElementValueByTagName(xmlDoc, "Button");

            if (button != "")
            {
                switch (button)
                {
                    case "Disconnect":
                        Close();
                        break;

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
                        //will trigger LiveStreamFormClosing
                        Close();
                        break;
                }
            }
        }

        private void LiveStreamFormClosing(Object sender, FormClosingEventArgs e)
        {
            //if (_globalMouseHandler != null)
            //{
            //    Application.RemoveMessageFilter(_globalMouseHandler);
            //    _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;
            //}

            UnregisterVideoWindow(_videoWindow);

            _videoWindow = null;

            //remove from using
            if (VideoMonitor.UsingPopupLive.Contains(this))
                VideoMonitor.UsingPopupLive.Remove(this);

            //recycle
            if (!VideoMonitor.RecyclePopupLive.Contains(this))
                VideoMonitor.RecyclePopupLive.Enqueue(this);

            //dont close window, recycle use form on next
            Hide();
            e.Cancel = true;
        }

        private void LiveStreamActivated(Object sender, EventArgs e)
        {
            if (_globalMouseHandler != null)
            {
                _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;
                _globalMouseHandler.TheMouseMoved += GlobatMouseMoveHandler;
            }
            if (_videoWindow != null)
                _videoWindow.Active = true;
        }

        private void LiveStreamDeactivate(Object sender, EventArgs e)
        {
            if (_globalMouseHandler != null)
                _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;

            if (_videoWindow != null)
                _videoWindow.Active = false;
        }
    }
}
