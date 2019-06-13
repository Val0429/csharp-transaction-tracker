using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        protected Boolean IsConnecting;
        protected Boolean IsReconnecting;
        protected Boolean IsDisconnecting;
        public virtual void Connect()
        {
            //dont connect try memory leak problem
            //return;

            if (_control == null || Camera == null || Camera.Server == null) return;

            var isLayout = (Camera is IDeviceLayout || Camera is ISubLayout);
            if (Camera.ReadyState == ReadyState.New && !isLayout) return;
            if (NetworkStatus != NetworkStatus.Idle) return;

            if (string.IsNullOrEmpty(Host))
            {
                Host = Camera.Server.Credential.Domain;
                Port = Camera.Server.Credential.Port;
                UserName = Camera.Server.Credential.UserName;
                UserPwd = Camera.Server.Credential.Password;
                _control.ServerSSL = Camera.Server.Credential.SSLEnable ? 1 : 0;
            }

            if (Camera.Server.Configure.CPULoadingUpperBoundary > 0)
                _control.SetCpuThreshold(Camera.Server.Configure.CPULoadingUpperBoundary);

            TimeZone = Camera.Server.Server.TimeZone;
            IsConnecting = true;

            if (PlayMode == PlayMode.LiveStreaming) //Live
            {
                if (_deviceLayout != null)
                {
                    _control.ServerUri = "/airprovider/media?guid=" + _deviceLayout.LiveGUID;
                }
                else if (_subLayout != null)
                {
                    _control.ServerUri = "/airprovider/media?x=" + _subLayout.X + "&y=" + _subLayout.Y + "&w=" + _subLayout.Width + "&h=" + _subLayout.Height + "&option=" + _subLayout.Dewarp + "&guid=" + _subLayout.DeviceLayout.LiveGUID;
                }
                else
                {
                    var uri = "/airvideo/media?channel=channel" + Camera.Id;
                    var stream = GetLiveStream();

                    _control.ServerUri = uri + stream;
                }
            }
            else if (PlayMode == PlayMode.Playback1X || PlayMode == PlayMode.GotoTimestamp)
            {
                if (_deviceLayout != null)
                {
                    _control.ServerUri = "/airprovider/playback?guid=" + _deviceLayout.PlaybackGUID;
                }
                else if (_subLayout != null)
                {
                    _control.ServerUri = "/airprovider/playback?x=" + _subLayout.X + "&y=" + _subLayout.Y + "&w=" + _subLayout.Width + "&h=" + _subLayout.Height + "&option=" + _subLayout.Dewarp + "&guid=" + _subLayout.DeviceLayout.PlaybackGUID;
                }
                else
                {
                    var uri = "/airvideo/playback?channel=channel" + Camera.Id;

                    var stream = GetPlaybackStream();
                    AutoDropFrame();

                    _control.ServerUri = uri + stream;
                }
            }
            else
                return;

            _control.SetDisplayTimeZone(_timezone);

            var format = new List<String> { "   " };

            foreach (VideoWindowTitleBarInformation information in Camera.Server.Configure.VideoWindowTitleBarInformations)
            {
                switch (information)
                {
                    case VideoWindowTitleBarInformation.CameraName:
                        format.Add(Camera.ToString().Replace("%", " "));
                        break;

                    case VideoWindowTitleBarInformation.Compression:
                        if (_deviceLayout == null && _subLayout == null)
                            format.Add("%%type");
                        break;

                    case VideoWindowTitleBarInformation.Resolution:
                        format.Add("%%res");
                        break;

                    case VideoWindowTitleBarInformation.Bitrate:
                        if (_deviceLayout == null && _subLayout == null && PlayMode == PlayMode.LiveStreaming)
                            format.Add("%%bitrate");
                        break;

                    case VideoWindowTitleBarInformation.FPS:
                        if (_deviceLayout == null && _subLayout == null)
                            format.Add("%%fps");
                        break;

                    case VideoWindowTitleBarInformation.DateTime:
                        format.Add("%Y-%m-%d %H:%M:%S");// %%ms
                        break;
                }
            }

            _control.SetDateTimeLayout(String.Join("   ", format.ToArray()));

            EnableKeepLastFrame((ushort)(Camera.Server.Configure.KeepLastFrame ? 1 : 0));

            _control.AutoReconnect = 1;
            _control.AudioTrack = 1;
            _control.Mute = 1;
            _control.Connect();

            var bghtml = Camera.Server.Configure.VideoTitleBarBackgroundColor.Replace("#", "");
            var r = bghtml.Substring(0, 2);
            var g = bghtml.Substring(2, 2);
            var b = bghtml.Substring(4, 2);
            var bg = UInt32.Parse(b + g + r, System.Globalization.NumberStyles.HexNumber);

            var colorhtml = Camera.Server.Configure.VideoTitleBarFontColor.Replace("#", "");
            r = colorhtml.Substring(0, 2);
            g = colorhtml.Substring(2, 2);
            b = colorhtml.Substring(4, 2);
            var color = UInt32.Parse(b + g + r, System.Globalization.NumberStyles.HexNumber);

            _control.SetTitleBarFormat(000000, bg, color);
            EnablePlaybackSmoothMode((ushort)(Camera.Server.Configure.EnablePlaybackSmooth ? 1 : 0));
            RaiseNetworkStatusChange();
        }


        private BackgroundWorker _latImageBackgroundWorker;

        public virtual void Stop()
        {
            PlayMode = PlayMode.Idle;

            if (_control == null) return;

            if (_control.IsFullScreen() == 1) _control.CloseFullScreenWindow();

            if (IsDisconnecting) return;
            if (NetworkStatus == NetworkStatus.Idle) return;

            IsConnecting = false;

            var status = NetworkStatus;
            if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting)
                IsDisconnecting = true;

            // Add By Tulip For Keep Last Image 
            //if (Camera.Server.Configure.CameraLastImage)
            //{
            //    if ((_control.ConnectionStatus == "CONNECTED") || (_control.ConnectionStatus == "STREAMING"))
            //    {
            //        var path = Path.Combine(Application.StartupPath, "LastPicture");
            //        if (!Directory.Exists(path))
            //            Directory.CreateDirectory(path);

            //        Camera.LastPicture = Path.Combine(path, String.Format("{0}-{1}.jpg", Camera.Server.Id, Camera.Id));
            //        _control.SnapShot(1, Camera.LastPicture, 0, 0);
            //    }
            //}


            if (_adjustBrightness != 0)
            {
                _adjustBrightness = 0;
                _control.AdjustBrightness(0);
            }
            SetDigitalPtzRegionCount(4);
            _control.Disconnect();

            SetVisible(false);

            RaiseNetworkStatusChange();
        }

        public void Reconnect()
        {
            if (_control == null || Camera == null) return;

            if (PlayMode == PlayMode.Playback1X)
            {
                var timecode = (UInt64)Int64.Parse(_control.TimeCode);
                if (timecode > 0)
                    PlaybackTimecode = timecode;
            }

            switch (PlayMode)
            {
                case PlayMode.LiveStreaming:
                    _control.OnDisconnect -= ControlOnRecountToLive;
                    _control.OnDisconnect += ControlOnRecountToLive;
                    break;

                case PlayMode.GotoTimestamp:
                    _control.OnDisconnect -= ControlOnRecountToGoto;
                    _control.OnDisconnect += ControlOnRecountToGoto;
                    break;

                case PlayMode.Playback1X:
                    _control.OnDisconnect -= ControlOnRecountToPlayback;
                    _control.OnDisconnect += ControlOnRecountToPlayback;
                    break;
            }
            var profileId = ProfileId;
            Stop();
            ProfileId = profileId;
        }

        private void ControlOnConnect(Object sender, AxNvrViewerLib._INvrViewerEvents_OnConnectEvent e)
        {
            if (Camera == null) { Stop(); return; }

            IsConnecting = false;

            if (e.connectSuccessful == 1)
                IsReconnecting = false;
            else
                IsReconnecting = true;

            if (PtzMode == PTZMode.None)
                PtzMode = PTZMode.Digital;

            if (PlayMode != PlayMode.LiveStreaming && e.connectSuccessful == 1)
            {
                SetVisible(true);
            }

            RaiseConnect(e.connectSuccessful);

            RaiseNetworkStatusChange();

            UpdateRecordStatus();

            if (!String.IsNullOrEmpty(FullScreenPTZRegion))
            {
                SetDigitalPtzRegion(FullScreenPTZRegion);
                ShowRIPWindow(true);
                FullScreenPTZRegion = String.Empty;
            }
        }

        private void ControlOnConnectionRecovery(Object sender, AxNvrViewerLib._INvrViewerEvents_OnConnectionRecoveryEvent e)
        {
            if (Camera == null)
            {
                Stop();
                return;
            }

            switch (PlayMode)
            {
                case PlayMode.GotoTimestamp:
                    //maybe bind event on OnConnect, but trigger at OnRecovery
                    OnConnect -= VideoPlayerOnPlaybackConnect;
                    SetVisible(true);
                    if (PlaybackTimecode > 0)
                    {
                        _control.Goto(PlaybackTimecode, (UInt16)PlayMode.GotoTimestamp);
                    }
                    break;

                case PlayMode.Playback1X:
                    OnConnect -= VideoPlayerOnPlaybackConnect;
                    SetVisible(true);
                    if (PlaybackTimecode > 0)
                    {
                        _control.Goto(PlaybackTimecode, (UInt16)PlayMode.Playback1X);
                    }
                    break;
            }

            IsReconnecting = false;

            //for QA check
            Log.Write(String.Format("NVR: {0}, Id: {1}, OnConnectionRecovery", Camera.Server.Name, Camera));

            //enable digital ptz after reconnect
            PtzMode = PTZMode.Digital;

            RaiseNetworkStatusChange();
        }

        private void ControlOnNetworkLoss(Object sender, EventArgs e)
        {
            if (Camera == null) { Stop(); return; }

            //when disconnect and audio out is on
            if (Camera.IsAudioOut)
            {
                //if only 1-ch audio out
                if (App.AudioOutChannelCount == 1)
                    Camera.StopAudioTransfer();
            }

            SetVisible(false);

            IsReconnecting = true;

            RaiseNetworkStatusChange();
        }

        private void ControlOnDisconnect(Object sender, EventArgs e)
        {
            IsDisconnecting = false;
            IsReconnecting = false;
            IsConnecting = false;

            //when disconnect and audio out is on
            if (Camera != null && Camera.IsAudioOut)
            {
                //if only 1-ch audio out
                if (App.AudioOutChannelCount == 1)
                    Camera.StopAudioTransfer();
            }

            SetVisible(false);

            //for QA check
            if (Camera != null)
            {
                Log.Write(String.Format("NVR: {0}, Id: {1}, OnDisconnect", Camera.Server.Name, Camera));
            }

            _ptzMode = PTZMode.None;// PtzMode.None;

            RaiseDisconnect(0);

            RaiseNetworkStatusChange();
        }
    }
}
