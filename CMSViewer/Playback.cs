using System;
using Constant;
using DeviceConstant;
using Interface;

namespace CMSViewer
{
    public partial class CMSViewer
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
            return;
            //not support
            //if (_control == null || Camera == null) return;

            //PlayMode = PlayMode.GotoTimestamp;

            //_control.OnTimeCode -= ControlOnFrameTimeCode;
            //_control.OnTimeCode += ControlOnFrameTimeCode;

            //_control.NextFrame();
        }

        public void PreviousFrame()
        {
            return;
            //not support
            //if (_control == null || Camera == null) return;

            //PlayMode = PlayMode.GotoTimestamp;

            //_control.OnTimeCode -= ControlOnFrameTimeCode;
            //_control.OnTimeCode += ControlOnFrameTimeCode;

            //_control.PreviousFrame();
        }

        //private void ControlOnFrameTimeCode(Object sender,  AxiCMSViewerLib._IiCMSViewerEvents_OnTimeCodeEvent e)
        //{
        //    if (_control == null || Camera == null) return;

        //    _control.OnTimeCode -= ControlOnFrameTimeCode;

        //    if(OnFrameTimecodeUpdate != null)
        //        OnFrameTimecodeUpdate(this, new EventArgs<String>(e.t));
        //}

        private Boolean _isOnArchiveServer;
        public Boolean isOnSwitchConnect;
        public virtual void GoTo(UInt64 timecode)
        {
            if (timecode == 0) return;

            PlaybackTimecode = timecode;
            if (_control == null || Camera == null) return;

            //Console.WriteLine("Playback GoTo"););
            if (PlayMode != PlayMode.GotoTimestamp)
            {
                _control.SetDecodeI(1);
                _control.SetPlayMode((UInt16)PlayMode.GotoTimestamp);//0:Live 1:Playback
            }

            PlayMode = PlayMode.GotoTimestamp;

            var switchConnect = false;
            var isOnArchiveServer = Camera.IsArchiveRecord(PlaybackTimecode);
            if (isOnArchiveServer != _isOnArchiveServer)
            {
                switchConnect = true;
                _isOnArchiveServer = isOnArchiveServer;
            }

            if (IsDisconnecting)
            {
                Reconnect();
            }
            else
            {
                //Console.WriteLine(NetworkStatus + " " + timecode + " " + PlayMode);
                //Camera.in
                var status = NetworkStatus;
                if ((status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting) && !switchConnect)
                {
                    if (PlaybackTimecode > 0)
                    {
                        _control.Goto(PlaybackTimecode, (UInt16)PlayMode.GotoTimestamp);//1 Goto 2 1xPlayback
                    }
                }
                else
                {
                    if (!switchConnect)
                    {
                        OnConnect -= VideoPlayerOnPlaybackConnect;
                        OnConnect += VideoPlayerOnPlaybackConnect;
                        Connect();
                    }
                    else
                    {
                        StopForAchive();
                    }
                }
            }
        }

        public void Playback(UInt64 timecode)
        {
            if (timecode == 0) return;

            PlaybackTimecode = timecode;
            if (_control == null || Camera == null) return;

            //Console.WriteLine("Playback 1X");
            if (PlayMode != PlayMode.Playback1X)
            {
                _control.SetDecodeI(0);
                _control.SetPlayMode((UInt16)PlayMode.GotoTimestamp);//0:Live 1:Playback
            }

            PlayMode = PlayMode.Playback1X;

            //Console.WriteLine(NetworkStatus + " " + timecode + " " + PlayMode);
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
            var stream = "";

            if (Camera.CMS.Configure.EnableBandwidthControl)
            {
                if (Camera.Server.Configure.BandwidthControlBitrate != Bitrate.NA)
                    stream += CustomStreamSetting(Camera.Server.Configure.CustomStreamSetting);

#if Salient // NOTE: Salient not supported
                var customStreamSetting = Camera.CMS.Configure.CustomStreamSetting;
                if (customStreamSetting.Enable)
                {
                    stream = string.Format("{0}&width={1}&height={2}&fps={3}",
                                    stream, customStreamSetting.Resolution.ToWidth(), customStreamSetting.Resolution.ToHeight(), customStreamSetting.Framerate);
                } 
#endif
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
            //Console.WriteLine(_control.ConnectionStatus);
            if (PlaybackTimecode <= 0) return;

            //_control.Goto(1399626600000, (UInt16)PlayMode);
            _control.Goto(PlaybackTimecode, (UInt16)PlayMode);
        }
    }
}
