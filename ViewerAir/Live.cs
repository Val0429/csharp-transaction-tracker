using System;
using System.IO;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        public UInt64 Timecode
        {
            get
            {
                return _timecode;
                //try
                //{
                //    return (UInt64)Int64.Parse(_timecode);
                //    //return (_control != null) ? (UInt64)Int64.Parse(_control.TimeCode) : 0;
                //}
                //catch (Exception)
                //{
                //}
                //return 0;
            }
        }

        private UInt64 _timecode;

        private bool takeSnapshot = false;

        private void ControlOnTimeCode(Object sender, AxNvrViewerLib._INvrViewerEvents_OnTimeCodeEvent e)
        {
            try
            {
                _timecode = (UInt64)Int64.Parse(e.t);

                if (takeSnapshot)
                {
                    takeSnapshot = false;

                    var path = Path.Combine(Application.StartupPath, "LastPicture");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    Camera.LastPicture = Path.Combine(path, String.Format("{0}-{1}.jpg", Camera.Server.Id, Camera.Id));
                    _control.SnapShot(1, Camera.LastPicture, 0, 0);
                }
                //return (_control != null) ? (UInt64)Int64.Parse(_control.TimeCode) : 0;
            }
            catch (Exception)
            {
                _timecode = 0;
            }

            //update playback timecode, when disconnect/reconnect, should continue play at the lasted timecode
            if (_timecode != 0)
                _playbackTimecode = _timecode;//
        }

        private void ControlOnUpdateBitrate(Object sender, AxNvrViewerLib._INvrViewerEvents_OnUpdateBitrateEvent e)
        {
            if (OnBitrateUpdate != null)
                OnBitrateUpdate(this, new EventArgs<Int32>(e.k_rate));
        }

        public virtual void Play()
        {
            if (_control == null || Camera == null) return;

            if (PlayMode != PlayMode.LiveStreaming)
                _control.SetPlayMode((UInt16)PlayMode.LiveStreaming);//0:Live 1:Playback

            PlayMode = PlayMode.LiveStreaming;

            if (IsDisconnecting)
            {
                Reconnect();
            }
            else if (NetworkStatus != NetworkStatus.Connected && NetworkStatus != NetworkStatus.Streaming)
            {
                Connect();
            }
        }

        private void ControlOnPlay(Object sender, EventArgs e)
        {
            takeSnapshot = true;

            if (PlayMode == PlayMode.LiveStreaming)
            {
                SetVisible(true);
            }

            if (_ptzMode != PTZMode.None)
                PtzMode = _ptzMode;

            RaisePlay(0);

            RaiseNetworkStatusChange();
        }

        protected virtual String GetLiveStream()
        {
            if (Camera.Profile == null) return "";

            var stream = String.Empty;
            //default user stream setting or auto switch streaming setting

            if (Camera.Server.Configure.EnableBandwidthControl)
            {
                ProfileId =
                Camera.Profile.StreamId = 
                Camera.Server.Configure.BandwidthControlStream;

                if (Camera.Server.Configure.BandwidthControlBitrate != Bitrate.NA)
                {
                    stream += CustomStreamSetting(Camera.Server.Configure.CustomStreamSetting);
                }
                else
                {
                    stream = "&stream=" + ((ProfileId == 0) ? Camera.Profile.StreamId : ProfileId);
                }
            }
            else
            {
                stream = "&stream=" + ((ProfileId == 0) ? Camera.Profile.StreamId : ProfileId);
                if (App.CustomStreamSetting.Enable)
                    stream += CustomStreamSetting(App.CustomStreamSetting);
            }

            return stream;
        }

        private void ControlOnRecountToLive(Object sender, EventArgs e)
        {
            Play();
            _control.OnDisconnect -= ControlOnRecountToLive;
        }
    }
}
