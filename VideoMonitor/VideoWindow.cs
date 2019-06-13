using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;
using PanelBase;
using TimeTrack;

namespace VideoMonitor
{
    public partial class VideoWindow : UserControl, IControl, IAppUse, IDrop, IMouseHandler, IVideoWindow
    {
        public event EventHandler OnSelectionChange;

        protected void RaiseOnSelectionChange()
        {
            if (OnSelectionChange != null)
            {
                OnSelectionChange(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnTitleBarClick;
        public event MouseEventHandler OnWindowMouseDrag;

        public event EventHandler<EventArgs<Int32>> OnBitrateUpdate;
        event EventHandler<MouseEventArgs> IVideoWindow.OnMouseDown
        {
            add { OnWindowMouseDown += value; }
            remove { OnWindowMouseDown -= value; }
        }

        protected event EventHandler<MouseEventArgs> OnWindowMouseDown;

        protected void RaiseOnMouseDown(MouseEventArgs e)
        {
            var handler = OnWindowMouseDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler OnFullScreen;
        public event EventHandler<EventArgs<String>> OnCloseFullScreen;

        public IApp App { get; set; }

        public String TitleName { get; set; }

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (_server != null)
                    _getTimecodeTimer.SynchronizingObject = _server.Form;
            }
        }

        public static Dictionary<String, String> Localization { get; set; }

        public Boolean IsDecodeIFrame { get; private set; }
        public Int32 LiveBitrate { get; private set; }
        public UInt16 LiveVideoStreamId { get; private set; }

        private PlayMode _playMode = PlayMode.LiveStreaming;
        public PlayMode PlayMode
        {
            get { return _playMode; }
            set { _playMode = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean Stretch
        {
            get
            {
                return Viewer.StretchToFit;
            }
            set
            {
                if (Viewer != null)
                    Viewer.StretchToFit = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean Dewarp
        {
            get
            {
                return Viewer.Dewarp;
            }
            set
            {
                if (Viewer != null)
                    Viewer.Dewarp = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int16 DewarpType
        {
            get
            {
                return Viewer.DewarpType;
            }
            set
            {
                if (Viewer != null)
                    Viewer.DewarpType = value;
            }
        }

        private Label _timecodeLabel;
        private Panel _trackerPanel;
        private Panel _controllerPanel;
        protected readonly System.Timers.Timer _getTimecodeTimer = new System.Timers.Timer();

        private static readonly Image _bg = Resources.GetResources(Properties.Resources.viewerBG, Properties.Resources.BGViewerBG);

        private IVideoMenu _toolMenu;
        public virtual IVideoMenu ToolMenu
        {
            get { return _toolMenu ?? (_toolMenu = CreateVideoMenu()); }
            set
            {
                _toolMenu = value;
            }
        }

        protected virtual IVideoMenu CreateVideoMenu()
        {
            return new VideoMenu();
        }

        protected Boolean _active;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Boolean Active
        {
            get { return _active; }
            set
            {
                _active = Viewer.Active = value;

                BackColor = (_active) ? Color.FromArgb(65, 177, 225) : Color.FromArgb(40, 43, 48);
                //Close Audio In when disconnect
                if (!value)
                    Viewer.AudioIn = false;

                if (ToolMenu != null)
                {
                    if (value)
                    {
                        ToolMenu.VideoWindow = this;
                        ToolMenu.CheckStatus();
                        ToolMenu.UpdateLocation();

                        if (Camera == null)
                            ToolMenu.Visible = false;
                        else
                            ToolMenu.Visible = true;

                        Viewer.ShowRIPWindow(ToolMenu.Visible);
                    }
                    else
                    {
                        if (ToolMenu.VideoWindow == this)
                        {
                            ToolMenu.Visible = false;
                            Viewer.ShowRIPWindow(false);
                            ToolMenu.VideoWindow = null;
                        }
                    }
                }
            }
        }


        // Constructor
        static VideoWindow()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Error", "Error"},

                                   {"VideoWindow_SaveBeforeConnect", "Save setting before connecting."},
                                   {"VideoWindow_Idle", "Idle"},
                                   {"VideoWindow_Connecting", "Connecting..."},
                                   {"VideoWindow_Connected", "Connected"},
                                   {"VideoWindow_Streaming", "Streaming..."},
                                   {"VideoWindow_Disconnecting", "Disconnecting..."},
                                   {"VideoWindow_Reconnecting", "Reconnecting..."},

                                   {"Application_CantCreateFolder", "Can't create folder %1"},
                               };
            Localizations.Update(Localization);
        }

        public VideoWindow()
        {

        }

        private Boolean _isInitialize;

        public virtual void Initialize()
        {
            //dont worry, it will check if already initialize
            if (_isInitialize) return;

            InitializeComponent();

            DoubleBuffered = true;

            Controls.Remove(instantPlaybackDoubleBufferPanel);

            IsDecodeIFrame = false;
            LiveVideoStreamId = 0;
            LiveBitrate = 0;

            RegisterViewer();

            _getTimecodeTimer.Elapsed += GetTimecodeFromViewer;
            _getTimecodeTimer.Interval = 1000;

            viewerDoubleBufferPanel.MouseUp += VideoWindowMouseUp;
            viewerDoubleBufferPanel.MouseDown += VideoWindowMouseDown;

            viewerDoubleBufferPanel.Paint += VideoWindowPaint;

            viewerDoubleBufferPanel.SizeChanged += ViewerDoubleBufferPanelSizeChanged;

            _isInitialize = true;
        }

        private Boolean _displayTitleBar;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean DisplayTitleBar
        {
            get { return _displayTitleBar; }
            set
            {
                _displayTitleBar = Viewer.DisplayTitleBar = value;
            }
        }

        public virtual IViewer Viewer { get; set; }

        public ITrackerContainer Track { get; set; }

        public MiniTimeTrackController Controller { get; set; }

        public WindowLayout WindowLayout { get; set; }

        private BackgroundWorker _getSnapshotBackgroundWorker;
        private ICamera _camera;
        public virtual ICamera Camera
        {
            get
            {
                return _camera;
            }
            set
            {
                _camera = value;

                viewerDoubleBufferPanel.Invalidate();

                if (Viewer == null) return;

                if (Viewer.Camera != null && Viewer.Camera != _camera)
                    RegisterViewer(_camera);
                else if (_camera != null && _camera.Server != null && _camera.Server.Manufacture == "Salient")
                    RegisterViewer(_camera);


                if ((Server != null) && Server.Configure.CameraLastImage && Patroling())
                {
                    ChangeBackgroundImage();
                }
                else
                {
                    ChangeBackgroundImage(_bg, ImageLayout.Stretch);
                }

                if (_playMode == PlayMode.Snapshot)
                {
                    if (_camera != null && _camera.Snapshot == null)
                    {
                        var layout = viewerDoubleBufferPanel.BackgroundImageLayout;
                        ChangeBackgroundImage(null, layout);
                        if (_getSnapshotBackgroundWorker == null)
                        {
                            _getSnapshotBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
                            _getSnapshotBackgroundWorker.DoWork += GetSnapshot;
                            _getSnapshotBackgroundWorker.RunWorkerCompleted += GetSnapshotCompleted;
                        }

                        if (!_getSnapshotBackgroundWorker.IsBusy)
                            _getSnapshotBackgroundWorker.RunWorkerAsync();
                    }
                }

                Viewer.Camera = _camera;
                viewerDoubleBufferPanel.Invalidate();
            }
        }

        private bool Patroling()
        {
            if (App == null || App.StartupOption == null)
            {
                return false;
            }

            return App.StartupOption.DevicePatrol || App.StartupOption.GroupPatrol || VideoMonitor._isPatrol;
        }

        private void ChangeBackgroundImage()
        {
            // Add By Tulip For Keep Last Image 
            try
            {
                if (PlayMode == PlayMode.LiveStreaming && (_camera != null) && (Width >= 10) && (Height >= 10))
                {
                    var path = Path.Combine(Application.StartupPath, "LastPicture");
                    var filename = Path.Combine(path, String.Format("{0}-{1}.jpg", Camera.Server.Id, Camera.Id));
                    if (!File.Exists(filename)) return;

                    ImageLayout layout;
                    var img = Image.FromFile(filename);
                    if (Server.Configure.StretchLiveVideo)
                    {
                        layout = ImageLayout.Stretch;
                    }
                    else
                    {
                        layout = ImageLayout.Center;

                        var imgW = img.Width;
                        var imgH = img.Height;

                        if ((imgW > Width) || (imgH > Height))
                        {
                            // Image Bigger then window
                            img = ResizeImage(img, new Size(Width - 4, Height - 4), true);
                        }
                    }

                    ChangeBackgroundImage(img, layout);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        private void ChangeBackgroundImage(Image img, ImageLayout layout)
        {
            if (App.Form.InvokeRequired)
            {
                App.Form.BeginInvoke(new Action<Image, ImageLayout>(ChangeBackgroundImage), img, layout);
                return;
            }

            viewerDoubleBufferPanel.BackgroundImageLayout = layout;
            viewerDoubleBufferPanel.BackgroundImage = img;
        }

        private static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            try
            {
                int newWidth;
                int newHeight;
                if (preserveAspectRatio)
                {
                    var originalWidth = image.Width;
                    var originalHeight = image.Height;
                    var percentWidth = size.Width / (float)originalWidth;
                    var percentHeight = size.Height / (float)originalHeight;
                    var percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                    newWidth = (int)(originalWidth * percent);
                    newHeight = (int)(originalHeight * percent);
                }
                else
                {
                    newWidth = size.Width;
                    newHeight = size.Height;
                }
                var newImage = new Bitmap(newWidth, newHeight);
                using (var graphicsHandle = Graphics.FromImage(newImage))
                {
                    graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                return newImage;
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }

        private void GetSnapshot(Object sender, DoWorkEventArgs e)
        {
            if (Camera != null)
                Camera.GetSnapshot();
        }

        private void GetSnapshotCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            viewerDoubleBufferPanel.Invalidate();
        }

        public PTZMode PtzMode
        {
            get
            {
                return (Viewer != null) ? Viewer.PtzMode : PTZMode.Disable;
            }
            set
            {
                if (Viewer != null)
                    Viewer.PtzMode = value;
            }
        }

        public virtual void Activate()
        {
            if (Viewer.PtzMode != PTZMode.Optical)
                Viewer.PtzMode = PTZMode.Digital;

            switch (PlayMode)
            {
                case PlayMode.LiveStreaming:
                    //Stretch = Server.Configure.StretchLiveVideo;
                    if ((Server != null) && Server.Configure.CameraLastImage && Patroling())
                    {
                        ChangeBackgroundImage();
                    }
                    else
                    {
                        ChangeBackgroundImage(_bg, ImageLayout.Stretch);
                    }

                    Play();
                    break;

                case PlayMode.GotoTimestamp:
                    //Stretch = Server.Configure.StretchPlaybackVideo;
                    GoTo();
                    break;

                case PlayMode.Playback1X:
                    //Stretch = Server.Configure.StretchPlaybackVideo;
                    if (Controller != null)
                    {
                        Controller.ActiveButton("Play");

                        if (_rate == 0) return;

                        _rate = 0;
                        Track.Rate = _rate;
                        GoTo(DateTimes.ToUtc(Track.DateTime, Server.Server.TimeZone));
                    }
                    GoTo();
                    break;
            }
        }

        public virtual void Deactivate()
        {
            Viewer.AudioIn = false;
            Boolean stretch = Viewer.StretchToFit;
            UInt16 profileId = Viewer.ProfileId;

            Stop();

            RegisterViewer(Camera);
            Viewer.Camera = Camera;
            Viewer.StretchToFit = stretch;
            Viewer.Dewarp = false;
            Viewer.DewarpType = 0;
            Viewer.ProfileId = profileId;

            if (ToolMenu != null && Active)
            {
                ToolMenu.Visible = false;
                Viewer.ShowRIPWindow(false);
                ToolMenu.VideoWindow = this;
            }
        }


        private void TrackOnDateTimeChange(Object sender, EventArgs e)
        {
            _timecodeLabel.Text = Track.DateTime.ToDateTimeString();
        }

        private void TrackOnTimecodeChange(Object sender, EventArgs<String> e)
        {
            String val = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "Timestamp");
            if (string.IsNullOrEmpty(val)) return;

            _getTimecodeTimer.Enabled = false;
            GoTo(Convert.ToUInt64(val));
        }

        private void TrackOnSelectionChange(Object sender, EventArgs e)
        {
            ControllerOnStop(this, null);

            if (!Active)
            {
                RaiseOnSelectionChange();
            }
        }

        protected void ViewerOnMouseKeyDown(Object sender, EventArgs<Int32, Int32, Int32> e)
        {
            if (_displayTitleBar && e.Value2 <= 15 && OnTitleBarClick != null)
            {
                if (OnTitleBarClick != null)
                    OnTitleBarClick(this, null);
            }

            Viewer.ShowRIPWindow(true);
            RaiseOnSelectionChange();

            //auto enable audio in when it's audiobox
            if (Camera != null && Camera.Model != null && Camera.Model.Type == "AudioBox")
                Viewer.AudioIn = true;

            if (e.Value3 == 3)
            {
                RaiseOnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, e.Value1, e.Value2, 0));
            }
        }

        protected void ViewerOnBitrateUpdate(Object sender, EventArgs<Int32> e)
        {
            LiveBitrate = Math.Max(e.Value, 0);
        }

        private Point _position;
        private void VideoWindowMouseDown(Object sender, MouseEventArgs e)
        {
            if (_playMode == PlayMode.Snapshot)
            {
                _position = e.Location;
                viewerDoubleBufferPanel.MouseMove -= VideoWindowMouseMove;
                viewerDoubleBufferPanel.MouseMove += VideoWindowMouseMove;
            }

            if (OnTitleBarClick != null && Viewer.Visible == false)
                OnTitleBarClick(this, null);

            //auto enable audio in when it's audiobox
            if (Camera != null && Camera.Model != null && Camera.Model.Type == "AudioBox")
                Viewer.AudioIn = true;

            RaiseOnSelectionChange();
        }

        private void VideoWindowMouseUp(Object sender, MouseEventArgs e)
        {
            viewerDoubleBufferPanel.MouseMove -= VideoWindowMouseMove;
        }

        private void VideoWindowMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnWindowMouseDrag != null)
            {
                viewerDoubleBufferPanel.MouseMove -= VideoWindowMouseMove;
                OnWindowMouseDrag(this, e);
            }
        }

        protected void ViewerOnDisconnect(Object sender, EventArgs<Int32> e)
        {
            if (ToolMenu != null && ToolMenu.VideoWindow == this)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action<object, EventArgs<int>>(ViewerOnDisconnect), sender, e);
                    return;
                }
                ToolMenu.Visible = false;
                Viewer.ShowRIPWindow(false);
            }
        }

        public void Play()
        {
            if (Camera == null) return;

            PlayMode = PlayMode.LiveStreaming;

            if (Visible)
            {
                if (Viewer != null && Viewer.Camera != null)
                {
                    _getTimecodeTimer.Enabled = false;
                    if (Viewer.NetworkStatus == NetworkStatus.Disconnecting)
                    {
                        RegisterViewer(Camera);
                        Viewer.Camera = Camera;
                    }
                    else if (Viewer.NetworkStatus == NetworkStatus.Connecting && Viewer.Camera != Camera)
                    {
                        RegisterViewer(Camera);
                        Viewer.Camera = Camera;
                    }
                    else if (Camera.Server != null && Camera.Server.Manufacture == "Salient")
                    {
                        //why?
                        //RegisterViewer(Camera);
                        Viewer.Camera = Camera;
                    }
                    Viewer.Play();
                }
            }
            else
            {
                Stop();
            }
        }

        private UInt64 _timecode;
        public void GoTo(UInt64 timecode)
        {
            PlayMode = PlayMode.GotoTimestamp;
            _timecode = timecode;
            if (Camera == null) return;

            if (Visible && _timecode > 0)
            {
                if (Viewer != null)
                {
                    _getTimecodeTimer.Enabled = false;
                    if (Viewer.NetworkStatus == NetworkStatus.Disconnecting)
                    {
                        RegisterViewer(Camera);
                        Viewer.Camera = Camera;
                    }

                    if (Camera.Server.Server.TimeZone != Server.Server.TimeZone)
                    {
                        Int64 time = Convert.ToInt64(timecode);
                        time += (Server.Server.TimeZone * 1000);
                        time -= (Camera.Server.Server.TimeZone * 1000);
                        timecode = Convert.ToUInt64(time);
                    }
                    Viewer.GoTo(timecode);
                }
            }
            else
            {
                Stop();
            }
        }

        public void NextFrame()
        {
            Viewer.NextFrame();
        }

        public void PreviousFrame()
        {
            Viewer.PreviousFrame();
        }

        public void EnablePlaybackSmooth()
        {
            Viewer.EnablePlaybackSmoothMode((ushort)(Server.Configure.EnablePlaybackSmooth ? 1 : 0));
        }

        public void SetPlaySpeed(UInt16 speed)
        {
            Viewer.SetPlaySpeed(speed);
        }

        public void GoTo()
        {
            GoTo(_timecode);
        }

        public void Playback(UInt64 timecode)
        {
            PlayMode = PlayMode.Playback1X;
            _timecode = timecode;

            if (Visible)
            {
                if (Viewer != null)
                {
                    if (Viewer.NetworkStatus == NetworkStatus.Disconnecting)
                    {
                        RegisterViewer(Camera);
                        Viewer.Camera = Camera;
                    }

                    if (Camera != null && Camera.Server.Server.TimeZone != Server.Server.TimeZone)
                    {
                        Int64 time = Convert.ToInt64(timecode);
                        time += (Server.Server.TimeZone * 1000);
                        time -= (Camera.Server.Server.TimeZone * 1000);
                        timecode = Convert.ToUInt64(time);
                    }

                    if ((Camera is ISubLayout) || (Camera is IDeviceLayout))
                    {
                        PlayMode = PlayMode.GotoTimestamp;
                        Viewer.GoTo(timecode);
                        if (Track != null)
                        {
                            Track.Rate = 1;
                            Track.PlayOnRate();
                        }
                    }
                    else
                    {
                        PlayMode = PlayMode.Playback1X;
                        Viewer.Playback(timecode);
                    }
                }
            }
            else
            {
                Stop();
            }
        }

        public UInt64 GetTimecode()
        {
            if (Visible && Camera != null)
            {
                var timecode = Viewer.Timecode;

                if (timecode != 0)
                {
                    if (Camera.Server.Server.TimeZone != Server.Server.TimeZone)
                    {
                        Int64 time = Convert.ToInt64(timecode);
                        time += (Camera.Server.Server.TimeZone * 1000);
                        time -= (Server.Server.TimeZone * 1000);
                        timecode = Convert.ToUInt64(time);
                    }
                    _timecode = timecode;
                }

                return _timecode;
            }

            return 0;
        }

        public void Playback()
        {
            Playback(_timecode);
        }

        public void Stop()
        {
            LiveBitrate = 0;
            _getTimecodeTimer.Enabled = false;
            if (Viewer == null) return;

            Viewer.AudioIn = false;
            Viewer.Stop();

            if (Controller == null) return;

            if (PlayMode == PlayMode.Playback1X)
            {
                Controller.ActiveButton("Play");

                if (_rate == 0) return;

                _rate = 0;
                Track.Rate = _rate;
                GoTo(DateTimes.ToUtc(Track.DateTime, Server.Server.TimeZone));
            }
            else
            {
                if (_rate == 0) return;

                Controller.ActiveButton("Play");

                _rate = 0;
                Track.Rate = _rate;
                GoTo(DateTimes.ToUtc(Track.DateTime, Server.Server.TimeZone));
            }
        }

        public void Reconnect()
        {
            if (Viewer != null && Visible)
            {
                //Viewer.StretchToFit = true;
                Viewer.AudioIn = false;
                Viewer.Reconnect();
            }
        }

        public void AutoDropFrame()
        {
            IsDecodeIFrame = false;
            if (Viewer != null)
                Viewer.AutoDropFrame();
        }

        public void DecodeIframe()
        {
            IsDecodeIFrame = true;
            if (Viewer != null)
                Viewer.DecodeIframe();
        }

        public void SwitchVideoStream(UInt16 streamId)
        {
            if (LiveVideoStreamId != streamId)
            {
                LiveVideoStreamId = streamId;

                if (Viewer != null)
                    Viewer.SwitchVideoStream(streamId);
            }
            else
            {
                //if is is the same and viewer is already connected, no need reconnect
                var status = Viewer.NetworkStatus;
                if (status == NetworkStatus.Connecting || status == NetworkStatus.Connected ||
                    status == NetworkStatus.Streaming || status == NetworkStatus.Reconnecting)
                {
                    return;
                }

                if (Viewer != null)
                {
                    Viewer.SwitchVideoStream(streamId);
                }
            }
        }

        //clear device(if ther is one), close connection,
        public void Reset()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Reset));

                return;
            }

            _getTimecodeTimer.Enabled = false;
            if (Viewer == null) return;

            Viewer.AudioIn = false;
            Viewer.Stop();
            LiveVideoStreamId = 0;
            LiveBitrate = 0;
            _rate = 0;

            if (Track != null)
            {
                UnregisterController();
                UnregisterTracker();

                Controls.Remove(instantPlaybackDoubleBufferPanel);
            }

            if (ToolMenu != null && ToolMenu.VideoWindow == this)
            {
                ToolMenu.Visible = false;
                Viewer.ShowRIPWindow(false);
                ToolMenu.VideoWindow = null;
            }

            Camera = null;

            RegisterViewer();
        }

        public virtual void ClearAllEventHandler()
        {
            OnTitleBarClick = null;
            OnSelectionChange = null;
            OnWindowMouseDrag = null;
            OnWindowMouseDown = null;
            OnFullScreen = null;
            OnCloseFullScreen = null;
        }

        public void SendPTZCommand(String cmd)
        {
            if (Viewer == null || Camera == null || Camera.Model == null || !Camera.Model.IsSupportPTZ) return;

            Viewer.PtzMode = PTZMode.Optical;

            Viewer.SendPTZCommand(cmd);
        }

        public virtual void Snapshot(Boolean withTimestamp)
        {
            if (Viewer != null && Camera != null)
            {
                Viewer.Snapshot("", withTimestamp);
            }
        }

        private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        protected virtual string GetFoldType()
        {
            if (Server is ICMS)
                return "CMS";

            var pts = Server as IPTS;
            if (pts != null)
            {
                switch (pts.ReleaseBrand)
                {
                    case "Salient":
                        return "TransactionTracker";
                    default:
                        return "PTS";
                }
            }

            return "NVR";
        }

        public virtual void SaveImage(Boolean withTimestamp)
        {
            //Environment.SpecialFolder.Personal
            if (Viewer != null && Camera != null && Viewer.Visible)
            {
                String rootPath = string.Empty;
                switch (Server.Configure.SaveImagePath)
                {
                    case "Desktop":
                        rootPath = _desktopPath;
                        break;

                    case "Document":
                        rootPath = _documentPath;
                        break;

                    case "Picture":
                        rootPath = _picturePath;
                        break;

                    default:
                        try
                        {
                            if (!Directory.Exists(Server.Configure.SaveImagePath))
                            {
                                Directory.CreateDirectory(Server.Configure.SaveImagePath);
                            }

                            rootPath = Directory.Exists(Server.Configure.SaveImagePath)
                                            ? Server.Configure.SaveImagePath
                                            : _desktopPath;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                        break;
                }

                String folderType = GetFoldType();

                String folderName = "\\" + folderType + " Image (" + Server.Credential.Domain + ")\\" + Server.Server.DateTime.ToFileDateString() + "\\";

                Boolean pathIsExists = System.IO.Directory.Exists(rootPath + folderName);
                if (!pathIsExists)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(rootPath + folderName);
                    }
                    catch (Exception)
                    {
                        rootPath = _desktopPath;

                        pathIsExists = System.IO.Directory.Exists(rootPath + folderName);

                        if (!pathIsExists)
                        {
                            try
                            {
                                System.IO.Directory.CreateDirectory(rootPath + folderName);
                            }
                            catch (Exception)
                            {
                                TopMostMessageBox.Show(Localization["Application_CantCreateFolder"].Replace("%1", rootPath + folderName),
                                                Localization["MessageBox_Error"], MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);

                                return;
                            }
                        }
                    }
                }

                String name = Regex.Replace(Camera.ToString(), "[^a-zA-Z0-9 \\-]", "");

                var timecode = Viewer.Timecode;
                // The characters ':', '/' are not prohibited
                String fileName;
                if (timecode == 0)
                {
                    fileName = name + "_" + Server.Server.DateTime.ToFileDateTimeString();
                }
                else
                {
                    fileName = name + "_" + Viewer.Timecode.ToLocalTime(Server.Server.TimeZone).ToFileDateTimeString();
                }

                //dont add (1) (2), just replace the image file
                //String temp = fileName;
                //UInt16 count = 1;
                //while (System.IO.File.Exists(rootPath + folderName + fileName + ".jpg"))
                //{
                //    fileName = temp + " (" + count++ + ")";
                //}
                Viewer.Snapshot(rootPath + folderName + fileName + ".jpg", withTimestamp);
            }
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is IDevice);
        }

        public void DragStop(Point point, EventArgs<Object> e)
        {
            if (!Drag.IsDrop(this, point)) return;

            if (!(e.Value is ICamera)) return;

            Camera = (ICamera)e.Value;

            if (Track != null)
            {
                UnregisterController();
                UnregisterTracker();

                Controls.Remove(instantPlaybackDoubleBufferPanel);
            }

            //auto enable audio in when it's audiobox
            //won't enable when connect is not yet ready
            //if (_camera != null && _camera.Model.Type == "AudioBox")
            //    Viewer.AudioIn = true;

            RaiseOnSelectionChange();
        }

        public void DragMove(MouseEventArgs e)
        {
        }

        public void ServerTimeZoneChange()
        {
            if (Viewer.NetworkStatus != NetworkStatus.Idle)
            {
                Viewer.TimeZone = (Viewer.Camera != null) ? Viewer.Camera.Server.Server.TimeZone : Server.Server.TimeZone;

                if (Track != null)
                {
                    //Track.ResetBackgroundScalePosition();
                    Track.ShowRecord();
                    Track.Invalidate();
                }
            }
        }

        public virtual void GlobalMouseHandler()
        {
            if (Camera == null)
            {
                ToolMenu.HideMenuTimer.Enabled = true;
                return;
            }

            if (Drag.IsDrop(viewerDoubleBufferPanel))
            {
                //ToolMenu.CheckStatus();
                ToolMenu.UpdateLocation();
                ToolMenu.Visible = true;
                ToolMenu.HideMenuTimer.Enabled = false;
                Viewer.ShowRIPWindow(true);
                return;
            }

            if (Drag.IsDrop(((Control)ToolMenu)))
            {
                ToolMenu.HideMenuTimer.Enabled = false;

                return;
            }

            //when audio out is active, dont hide menu(even mouse is leave video window)
            var videoMenu = ToolMenu as VideoMenu;//for patch, i t can remove when upgrade
            if (videoMenu != null)
            {
                if (videoMenu.IsPressButton)
                {
                    ToolMenu.HideMenuTimer.Enabled = false;

                    return;
                }
            }

            ToolMenu.HideMenuTimer.Enabled = true;
        }

        protected void ViewerOnNetworkStatusChange(Object sender, EventArgs e)
        {
            if (Active && ToolMenu.VideoWindow == this)
            {
                //auto enable audio in when it's audiobox
                //after it's connected and still on focus

                if (Camera != null && Camera.Model != null && Camera.Model.Type == "AudioBox" && Viewer.NetworkStatus == NetworkStatus.Connecting)
                    Viewer.AudioIn = true;

                ToolMenu.CheckStatus();
            }


            viewerDoubleBufferPanel.Invalidate();
        }

        private readonly Font _nameFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, (0));

        private void VideoWindowPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null || Camera == null || Viewer == null || Viewer.Visible) return;

            Graphics g = e.Graphics;

            var isLayout = (Camera is IDeviceLayout || Camera is ISubLayout);
            String status = "";
            if (Camera.ReadyState == ReadyState.New && !isLayout)
            {
                status = Localization["VideoWindow_SaveBeforeConnect"];
            }
            else
            {
                if (_playMode == PlayMode.Snapshot)
                {
                    if (Camera.Snapshot != null)
                    {
                        //center
                        var widthpercent = Camera.Snapshot.Width / (Width * 1.0);
                        var heightpercent = Camera.Snapshot.Height / (Height * 1.0);

                        //resize the small side(width / height)
                        if (widthpercent == heightpercent)//nearly impossible, but maybe...result equal stretch = true
                        {
                            g.DrawImage(Camera.Snapshot, 0, 0, Width, Height);
                        }

                        //resize height
                        if (widthpercent > heightpercent)
                        {
                            var newheight = Convert.ToInt32(Camera.Snapshot.Height / widthpercent);
                            var yspace = (Camera.Snapshot.Height - newheight) / 2;
                            g.DrawImage(Camera.Snapshot, 0, yspace, Width, newheight);
                        }
                        else//resize width
                        {
                            var newwidth = Convert.ToInt32(Camera.Snapshot.Width / heightpercent);
                            var xspace = (Width - newwidth) / 2;
                            g.DrawImage(Camera.Snapshot, xspace, 0, newwidth, Height);
                        }

                        //fill 
                        //g.DrawImage(Camera.Snapshot, 0, 0, Width, Height);
                    }
                }
                else
                {
                    switch (Viewer.NetworkStatus)
                    {
                        case NetworkStatus.Idle:
                            //status = Localization["VideoWindow_Idle"];
                            break;

                        case NetworkStatus.Connecting:
                            status = Localization["VideoWindow_Connecting"];
                            break;

                        case NetworkStatus.Connected:
                            status = Localization["VideoWindow_Connected"];
                            break;

                        case NetworkStatus.Streaming:
                            //deray: display streaming is also no meaning. because it already show video stream on screen.
                            //status = Localization["VideoWindow_Streaming"];
                            break;

                        case NetworkStatus.Disconnecting
                        :
                            //deray: dont show disconnecting. this msg is no meaning, 
                            //and will happen when auto switch video stream. it should display "connecting" but display"diconnecting"
                            //because it's doing "reconnec"
                            //status = Localization["VideoWindow_Disconnecting"];
                            break;

                        case NetworkStatus.Reconnecting:
                        case NetworkStatus.Reconnect:
                            status = Localization["VideoWindow_Reconnecting"];
                            break;
                    }
                }
            }

            SizeF fSize = g.MeasureString(Camera.Name, _nameFont);
            g.DrawString(Camera.Name, _nameFont, Brushes.White, (Width - fSize.Width) / 2, (Height - fSize.Height) / 2);

            if (Server.Configure.DisplayDeviceId)
            {
                String id = Camera.Id.ToString().PadLeft(2, '0');
                SizeF iSize = g.MeasureString(id, _nameFont);
                g.DrawString(id, _nameFont, Brushes.White, (Width - iSize.Width) / 2, ((Height - iSize.Height) / 2 - fSize.Height) - 20);
            }

            if (String.IsNullOrEmpty(status)) return;
            SizeF sSize = g.MeasureString(status, _nameFont);
            g.DrawString(status, _nameFont, Brushes.White, (Width - sSize.Width) / 2, (Height - sSize.Height) / 2 + fSize.Height);
        }

        private void ViewerDoubleBufferPanelSizeChanged(Object sender, EventArgs e)
        {
            ViewerDoubleBufferPanelSizeChanged();
        }

        public void ViewerDoubleBufferPanelSizeChanged()
        {
            if (Viewer != null)
                Viewer.Size = viewerDoubleBufferPanel.Size;
        }

        public void NoBorder()
        {
            Padding = new Padding(0);
        }

        public virtual void SwitchProfileId(UInt16 profileid)
        {

        }

        private void VideoWindow_SizeChanged(object sender, EventArgs e)
        {
            if ((Server != null) && Server.Configure.CameraLastImage && Patroling())
            {
                ChangeBackgroundImage();
            }
            else
            {
                ChangeBackgroundImage(_bg, ImageLayout.Stretch);
            }
        }
    }
}
