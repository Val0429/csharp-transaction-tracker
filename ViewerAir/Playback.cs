using System;
using Constant;
using DeviceConstant;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        private UInt64 _playbackTimecode;
        public UInt64 PlaybackTimecode
        {
            get
            {
                if (_playbackTimecode == 0)
                    _playbackTimecode = Timecode;
                return _playbackTimecode;
            }
            set
            {
                _playbackTimecode = value;
            }
        }

        public void NextFrame()
        {
            if (_control == null || Camera == null) return;

            PlayMode = PlayMode.GotoTimestamp;

            _control.OnTimeCode -= ControlOnFrameTimeCode;
            _control.OnTimeCode += ControlOnFrameTimeCode;

            if (_deviceLayout == null && _subLayout == null)
                _control.NextFrame();
        }

        public void PreviousFrame()
        {
            if (_control == null || Camera == null) return;

            PlayMode = PlayMode.GotoTimestamp;

            _control.OnTimeCode -= ControlOnFrameTimeCode;
            _control.OnTimeCode += ControlOnFrameTimeCode;

            if (_deviceLayout == null && _subLayout == null)
                _control.PreviousFrame();
        }

        private void ControlOnFrameTimeCode(Object sender, AxNvrViewerLib._INvrViewerEvents_OnTimeCodeEvent e)
        {
            if (_control == null || Camera == null) return;

            _control.OnTimeCode -= ControlOnFrameTimeCode;

            if (OnFrameTimecodeUpdate != null)
                OnFrameTimecodeUpdate(this, new EventArgs<String>(e.t));
        }

        public virtual void GoTo(UInt64 timecode)
        {
            if (timecode == 0) return;

            PlaybackTimecode = timecode;
            if (_control == null || Camera == null) return;

            if (PlayMode != PlayMode.GotoTimestamp)
            {                
                //goto mode, only decode I frame
                _control.SetDecodeI(1);
                _control.SetPlayMode((UInt16)PlayMode.GotoTimestamp);//0:Live 1:Playback
            }

            PlayMode = PlayMode.GotoTimestamp;

            if (IsDisconnecting)
            {
                Reconnect();
            }
            else
            {
                var status = NetworkStatus;
                if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting)
                {
                    if (PlaybackTimecode > 0)
                    {
                        _control.Goto(PlaybackTimecode, (UInt16)PlayMode.GotoTimestamp);//1 Goto 2 1xPlayback
                    }
                }
                else
                {
                    OnConnect -= VideoPlayerOnPlaybackConnect;
                    OnConnect += VideoPlayerOnPlaybackConnect;
                    Connect();
                }
            }
        }

        public void Playback(UInt64 timecode)
        {
            if (timecode == 0) return;

            PlaybackTimecode = timecode;
            if (_control == null || Camera == null) return;

            if (PlayMode != PlayMode.Playback1X)
            {                
                //1x playback, auto drop frame
                _control.SetDecodeI(0);
                _control.SetPlayMode((UInt16)PlayMode.GotoTimestamp);//0:Live 1:Playback
            }

            PlayMode = PlayMode.Playback1X;

            var status = NetworkStatus;
            if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting)
            {
                if (PlaybackTimecode <= 0) return;

                _control.Goto(PlaybackTimecode, (UInt16)PlayMode.Playback1X);//1 Goto 2 1xPlayback
            }
            else
            {
                OnConnect -= VideoPlayerOnPlaybackConnect;
                OnConnect += VideoPlayerOnPlaybackConnect;
                Connect();
            }
        }

        protected virtual String GetPlaybackStream()
        {
            //var stream = "";
            //if (App.CustomStreamSetting.Enable)
            //    stream += CustomStreamSetting(App.CustomStreamSetting);

            var stream = String.Empty;
            //default user stream setting or auto switch streaming setting

            if (Camera.Server.Configure.EnableBandwidthControl)
            {
                if (Camera.Server.Configure.BandwidthControlBitrate != Bitrate.NA)
                    stream += CustomStreamSetting(Camera.Server.Configure.CustomStreamSetting);
            }
            else
            {
                if (App.CustomStreamSetting.Enable)
                    stream += CustomStreamSetting(App.CustomStreamSetting);
            }
            return stream;
        }

        private void ControlOnRecountToGoto(Object sender, EventArgs e)
        {
            GoTo(PlaybackTimecode);
            _control.OnDisconnect -= ControlOnRecountToGoto;
        }

        private void ControlOnRecountToPlayback(Object sender, EventArgs e)
        {
            Playback(PlaybackTimecode);
            _control.OnDisconnect -= ControlOnRecountToPlayback;
        }

        private void VideoPlayerOnPlaybackConnect(Object sender, EventArgs<Int32> e)
        {
            OnConnect -= VideoPlayerOnPlaybackConnect;
            //1 Goto 2 1xPlayback
            if (PlaybackTimecode <= 0) return;

            _control.Goto(PlaybackTimecode, (UInt16)PlayMode);
        }
    }
}
