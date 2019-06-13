using System;
using System.Collections.Generic;
using Constant;
using DeviceConstant;
using Interface;
using TimeTrack;

namespace VideoMonitor
{
    public partial class VideoWindow
    {
        private static readonly Queue<ITrackerContainer> StoredTrackerContainer = new Queue<ITrackerContainer>();
        private void RegisterTracker()
        {
            UnregisterTracker();

            if (StoredTrackerContainer.Count > 0)
            {
                Track = StoredTrackerContainer.Dequeue();

                return;
            }

            Track = CreateTrackerContainer();
        }

        protected virtual ITrackerContainer CreateTrackerContainer()
        {
            var track = new TrackerContainer
            {
                Server = Server,
                //RecordHeight = 13,
                //TrackerHeight = 19,
                TrackerNumPerPage = 1,
                MaxConnection = 1,
                TopPosition = 3,
                PaintTop = 3,
                UnitTime = TimeUnit.Unit10Senonds,
                PaintTitle = false,
            };

            return track;
        }

        protected void UnregisterTracker()
        {
            if (Track == null) return;

            Track.Rate = _rate = 0;
            Track.Stop();
            Track.RemoveAll();
            Track.Parent = null;
            _getTimecodeTimer.Enabled = false;

            Track.OnTimecodeChange -= TrackOnTimecodeChange;
            Track.OnDateTimeChange -= TrackOnDateTimeChange;
            Track.OnSelectionChange -= TrackOnSelectionChange;

            if (!StoredTrackerContainer.Contains(Track))
                StoredTrackerContainer.Enqueue(Track);

            Track = null;
        }

        protected float _rate;

        public void ControllerOnPlay(Object sender, EventArgs e)
        {
            Controller.ActiveButton("Pause");

            if (_rate == 1) return;

            _rate = 1;
            _getTimecodeTimer.Enabled = true;
            Track.Rate = _rate;
            //Track.PlayOnRate();
            Playback(DateTimes.ToUtc(Track.DateTime, Server.Server.TimeZone));
        }

        private void ControllerOnStop(Object sender, EventArgs e)
        {
            _getTimecodeTimer.Enabled = false;
            Controller.ActiveButton("Play");

            if (_rate == 0) return;

            _rate = 0;
            Track.Rate = _rate;
            Track.Stop();
            GoTo(DateTimes.ToUtc(Track.DateTime, Server.Server.TimeZone));
        }

        private void GetTimecodeFromViewer(Object sender, EventArgs e)
        {
            GetTimecodeFromViewer();
        }

        private void GetTimecodeFromViewer()
        {
            if (Viewer != null && Track != null && Camera != null)
            {
                UInt64 timecode = Viewer.Timecode;

                if (timecode == 0) return;

                if (Server.Server.TimeZone != Camera.Server.Server.TimeZone)
                {
                    Int64 time = Convert.ToInt64(timecode);
                    time += Camera.Server.Server.TimeZone * 1000;
                    time -= Server.Server.TimeZone * 1000;
                    timecode = Convert.ToUInt64(time);
                }

                _timecode = timecode;
                Track.DateTime = DateTimes.ToDateTime(_timecode, Server.Server.TimeZone);
            }
            else
                _getTimecodeTimer.Enabled = false;
        }
        //-----------------------------------------------------------------------------------

        private static readonly Queue<MiniTimeTrackController> StoredMiniTimeTrackController = new Queue<MiniTimeTrackController>();
        private void RegisterController()
        {
            UnregisterController();

            if (StoredMiniTimeTrackController.Count > 0)
            {
                Controller = StoredMiniTimeTrackController.Dequeue();

                return;
            }

            Controller = new MiniTimeTrackController();
        }

        protected void UnregisterController()
        {
            if (Controller == null) return;

            Controller.ActiveButton("Play");
            Controller.OnPlay -= ControllerOnPlay;
            Controller.OnStop -= ControllerOnStop;

            Controller.Parent = null;

            if (!StoredMiniTimeTrackController.Contains(Controller))
                StoredMiniTimeTrackController.Enqueue(Controller);

            Controller = null;
        }

        //-----------------------------------------------------------------------------------

        public virtual void RegisterViewer(ICamera camera = null)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ICamera>(RegisterViewer), camera);

                return;
            }

            UnregisterViewer();

            Viewer = CreateViewer(camera);

            if (Viewer == null) return;

            if (IsDecodeIFrame)
                Viewer.DecodeIframe();
            else
                Viewer.AutoDropFrame();

            Viewer.SwitchVideoStream(LiveVideoStreamId);

            if (Server != null)
            {
                Viewer.StretchToFit = (PlayMode == PlayMode.LiveStreaming)
                                                  ? Server.Configure.StretchLiveVideo
                                                  : Server.Configure.StretchPlaybackVideo;
            }

            Viewer.Size = viewerDoubleBufferPanel.Size;
            Viewer.Parent = viewerDoubleBufferPanel;
            Viewer.BringToFront();
            Viewer.DisplayTitleBar = _displayTitleBar;

            Viewer.OnMouseKeyDown += ViewerOnMouseKeyDown;
            Viewer.OnBitrateUpdate += ViewerOnBitrateUpdate;
            Viewer.OnDisconnect += ViewerOnDisconnect;
            Viewer.OnNetworkStatusChange += ViewerOnNetworkStatusChange;
            Viewer.OnFullScreen += ViewerOnFullScreen;
            Viewer.OnCloseFullScreen += ViewerOnCloseFullScreen;
        }

        protected virtual IViewer CreateViewer(ICamera camera)
        {
            return App.RegistViewer();
        }

        private void ViewerOnCloseFullScreen(Object sender, EventArgs<String> e)
        {
            if (OnCloseFullScreen != null)
                OnCloseFullScreen(this, e);
        }

        private void ViewerOnFullScreen(Object sender, EventArgs e)
        {
            if (OnFullScreen != null)
            {
                OnFullScreen(this, EventArgs.Empty);
            }
        }

        public virtual void UnregisterViewer()
        {
            if (Viewer == null) return;

            LiveBitrate = 0;
            Viewer.SwitchVideoStream(0);
            Viewer.OnMouseKeyDown -= ViewerOnMouseKeyDown;
            Viewer.OnBitrateUpdate -= ViewerOnBitrateUpdate;
            Viewer.OnDisconnect -= ViewerOnDisconnect;
            Viewer.OnNetworkStatusChange -= ViewerOnNetworkStatusChange;
            Viewer.OnFullScreen -= ViewerOnFullScreen;
            Viewer.OnCloseFullScreen -= ViewerOnCloseFullScreen;

            if (_getSnapshotBackgroundWorker != null && _getSnapshotBackgroundWorker.IsBusy)
            {
                _getSnapshotBackgroundWorker.RunWorkerCompleted -= GetSnapshotCompleted;
                _getSnapshotBackgroundWorker.CancelAsync();
                _getSnapshotBackgroundWorker = null;
            }

            //App.UnregistViewer(Viewer);
            RecoverViewer(Viewer);

            Viewer = null;
        }

        protected virtual void RecoverViewer(IViewer viewer)
        {
            App.UnregistViewer(viewer);
        }
        //-----------------------------------------------------------------------------------
    }
}
