using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Constant.Utility;

namespace VideoMonitor
{
    public partial class VideoWindow
    {
        private static Image _bgBanner = Resources.GetResources(Properties.Resources.banner, Properties.Resources.BGBanner);

        private void CreateInstantPlaybackPanel()
        {
            _timecodeLabel = new Label
            {
                AutoSize = false,
                BackgroundImage = _bgBanner,
                BackgroundImageLayout = ImageLayout.Stretch,
                Dock = DockStyle.Top,
                Size = new Size(2048, 15),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            _trackerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Size = new Size(300, 31),
            };

            _controllerPanel = new Panel
            {
                Dock = DockStyle.Left,
                Size = new Size(44, 31),
            };

            var pointer = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                BackColor = Color.FromArgb(237, 57, 28),
                Cursor = Cursors.NoMoveHoriz,
                Size = new Size(1, 30),
                TabStop = false
            };

            _trackerPanel.Controls.Add(pointer);//for the instant play back at have no pointer

            _trackerPanel.MouseDown += VideoWindowMouseDown;

            instantPlaybackDoubleBufferPanel.Controls.Add(_trackerPanel);
            instantPlaybackDoubleBufferPanel.Controls.Add(_controllerPanel);
            instantPlaybackDoubleBufferPanel.Controls.Add(_timecodeLabel);

            pointer.Location = new Point(_trackerPanel.Width / 2, 0);
        }

        public void StartInstantPlayback()
        {
            UInt64 timecode = Viewer.Timecode;
            //Reverse 5 sec from now
            StartInstantPlayback(timecode != 0
                 ? DateTimes.ToDateTime(timecode, Camera.Server.Server.TimeZone).AddSeconds(Server.Configure.InstantPlaybackSeconds)
                 : Server.Server.DateTime.AddSeconds(Server.Configure.InstantPlaybackSeconds));
        }

        public virtual void StartInstantPlayback(DateTime dateTime)
        {
            if (instantPlaybackDoubleBufferPanel.Controls.Count == 0)
            {
                CreateInstantPlaybackPanel();
            }

            if (Track == null)
            {
                RegisterController();
                RegisterTracker();

                if (Controller != null)
                {
                    Controller.Parent = _controllerPanel;

                    Controller.OnPlay += ControllerOnPlay;
                    Controller.OnStop += ControllerOnStop;
                }

                if (Track != null)
                {
                    Track.Parent = _trackerPanel;
                    Track.DateTime = dateTime;

                    Track.OnTimecodeChange += TrackOnTimecodeChange;
                    Track.OnDateTimeChange += TrackOnDateTimeChange;
                    Track.OnSelectionChange += TrackOnSelectionChange;
                }

                _timecodeLabel.Text = dateTime.ToDateTimeString();

                Controls.Add(instantPlaybackDoubleBufferPanel);

                if (Track != null)
                    Track.AppendDevice(0, Camera);

                RegisterViewer(Camera);
                //if (App != null)
                //	Viewer.ClientMode = App.ClientMode;
                Viewer.StretchToFit = Server.Configure.StretchPlaybackVideo;
                Viewer.Camera = Camera;
            }
            else
            {
                Track.DateTime = dateTime;
            }


            if (Track == null)
                return;

            Viewer.Active = true;
            Viewer.AutoDropFrame();
            GoTo(DateTimes.ToUtc(dateTime, Server.Server.TimeZone));
        }

        public virtual void StopInstantPlayback()
        {
            if (Track == null) return;

            UnregisterController();
            UnregisterTracker();

            Controls.Remove(instantPlaybackDoubleBufferPanel);

            RegisterViewer(Camera);

            Viewer.Active = true;
            Viewer.StretchToFit = Server.Configure.StretchLiveVideo;
            Viewer.Camera = Camera;
            if (IsDecodeIFrame)
                Viewer.DecodeIframe();
            else
                Viewer.AutoDropFrame();

            Viewer.SwitchVideoStream(LiveVideoStreamId);

            Play();
        }
    }
}