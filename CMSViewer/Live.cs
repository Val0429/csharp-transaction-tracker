using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;

namespace CMSViewer
{
    public partial class CMSViewer
    {
        public UInt64 Timecode { get; private set; }

        private bool takeSnapshot = false;

        private void ControlOnTimeCode(Object sender, AxiCMSViewerLib._IiCMSViewerEvents_OnTimeCodeEvent e)
        {
            try
            {
                if (takeSnapshot)
                {
                    takeSnapshot = false;

                    if (Camera != null)
                    {
                        var path = Path.Combine(Application.StartupPath, "LastPicture");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        Camera.LastPicture = Path.Combine(path, String.Format("{0}-{1}.jpg", Camera.Server.Id, Camera.Id));

                        _control.SnapShot(1, Camera.LastPicture, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            try
            {
                var timestamp = ConvertUtility.ToUInt64(e.t);
                OnTimestampChanged(timestamp);

                if (Camera == null) return;

                var switchConnect = false;
                var isOnArchiveServer = Camera.IsArchiveRecord(timestamp);
                if (isOnArchiveServer != _isOnArchiveServer)
                {
                    switchConnect = true;
                    _isOnArchiveServer = isOnArchiveServer;
                }

                if (switchConnect)
                {
                    OnConnect -= VideoPlayerOnPlaybackConnect;
                    OnConnect += VideoPlayerOnPlaybackConnect;
                    StopForAchive();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected virtual void OnTimestampChanged(ulong timestamp)
        {
            Timecode = timestamp;
        }

        private void ControlOnUpdateBitrate(Object sender, AxiCMSViewerLib._IiCMSViewerEvents_OnUpdateBitrateEvent e)
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

        public void SetSubLayoutRegion(List<ISubLayout> subLayouts)
        {

        }

        public string UpdateSubLayoutRegion()
        {
            return null;
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

            //default user stream setting or auto switch streaming setting
            var stream = String.Empty;
            if (Camera.CMS.Configure.EnableBandwidthControl)
            {
                ProfileId =
                Camera.Profile.StreamId = Camera.Server.Configure.BandwidthControlStream;

                if (Camera.Server.Configure.BandwidthControlBitrate != Bitrate.NA)
                {
                    stream += CustomStreamSetting(Camera.Server.Configure.CustomStreamSetting);
                }
                else
                {
                    stream = string.Format("&stream={0}",ProfileId == 0 ? Camera.Profile.StreamId : ProfileId);
                    var customStreamSetting = Camera.CMS.Configure.CustomStreamSetting;
                    if (customStreamSetting.Enable)
                    {
                        stream = string.Format("{0}&width={1}&height={2}&fps={3}", stream, customStreamSetting.Resolution.ToWidth(),
                            customStreamSetting.Resolution.ToHeight(), customStreamSetting.Framerate);
                    }
                }
            }
            else
            {
                stream = "&stream=" + ((ProfileId == 0) ? Camera.Profile.StreamId : ProfileId);
                if (App.CustomStreamSetting.Enable)
                    stream += CustomStreamSetting(App.CustomStreamSetting);
            }

            Debug.WriteLine(stream);
            return stream;
        }

        private void ControlOnRecountToLive(Object sender, EventArgs e)
        {
            Play();
            _control.OnDisconnect -= ControlOnRecountToLive;
        }

    }
}
