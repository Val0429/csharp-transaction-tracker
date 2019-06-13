using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace CMSViewer
{
    public partial class CMSViewer
    {
        protected Boolean IsConnecting;
        protected Boolean IsReconnecting;
        protected Boolean IsDisconnecting;

        public virtual void Connect()
        {
            //dont connect try memory leak problem
            //return;
            if (_control == null || Camera == null || Camera.Server == null) return;
            _control.Dock = System.Windows.Forms.DockStyle.Fill;
            _control.Size = new Size(Width, Height);
            var isLayout = (Camera is IDeviceLayout || Camera is ISubLayout);
            if (Camera.ReadyState == ReadyState.New && !isLayout) return;
            //if (NetworkStatus != NetworkStatus.Idle)
            //    System.Threading.Thread.Sleep(2000);

            //if (NetworkStatus != NetworkStatus.Idle)
            //    return;

            if (Host == "")
            {
                Host = Camera.CMS.Credential.Domain;
                Port = Camera.CMS.Credential.Port;
                UserName = Camera.CMS.Credential.UserName;
                UserPwd = Camera.CMS.Credential.Password;
                _control.ServerSSL = Camera.CMS.Credential.SSLEnable ? 1 : 0;
            }

            if (Camera.CMS.Configure.CPULoadingUpperBoundary > 0)
                _control.SetCpuThreshold(Camera.CMS.Configure.CPULoadingUpperBoundary);

            TimeZone = Camera.CMS.Server.TimeZone;
            IsConnecting = true;

            
            _control.ServerIp = Camera.CMS.Credential.Domain;
            _control.ServerUsername = Camera.CMS.Credential.UserName;
            _control.ServerPassword = Camera.CMS.Credential.Password;
            _control.ServerPort = Camera.CMS.Credential.Port;

            if (PlayMode == PlayMode.LiveStreaming) //Live
            {
                var uri = String.Format("/airvideo/media?nvr=nvr{0}&channel=channel{1}", Camera.Server.Id, Camera.Id);
                var stream = GetLiveStream();
                _control.ServerUri = uri + stream;
                //_control.SetPlayMode(0);

            }
            else if (PlayMode == PlayMode.Playback1X || PlayMode == PlayMode.GotoTimestamp)
            {
                ProfileId =
                Camera.Profile.StreamId = 1;

                var uri = String.Format("/airvideo/playback?nvr=nvr{0}&channel=channel{1}&track=video,audio1&wait=0", Camera.Server.Id, Camera.Id);
                var stream = GetPlaybackStream();
                //AutoDropFrame();
                if (_isOnArchiveServer) stream += "&archive=1";

                _control.ServerUri = uri + stream;

                //_control.SetPlayMode(1);
            }
            else
                return;

            Debug.WriteLine(_control.ServerUri);
            _control.AutoReconnect = 1;
            _control.SetDisplayTimeZone(_timezone);

            var format = new List<String> { "   " };

            foreach (VideoWindowTitleBarInformation information in Camera.CMS.Configure.VideoWindowTitleBarInformations)
            {
                switch (information)
                {
                    case VideoWindowTitleBarInformation.CameraName:
                        format.Add(Camera.ToString().Replace("%", " "));
                        break;

                    case VideoWindowTitleBarInformation.Compression:
                        format.Add("%%type");
                        break;

                    case VideoWindowTitleBarInformation.Resolution:
                        format.Add("%%res");
                        break;

                    case VideoWindowTitleBarInformation.Bitrate:
                        //if (PlayMode == PlayMode.LiveStreaming)
                            format.Add("%%bitrate");
                        break;

                    case VideoWindowTitleBarInformation.FPS:
                        format.Add("%%fps");
                        break;

                    case VideoWindowTitleBarInformation.DateTime:
                        format.Add("%Y-%m-%d %H:%M:%S");// %%ms
                        break;
                }
            }

            _control.SetDateTimeLayout(String.Join("   ", format.ToArray()));

            if (Camera.CMS != null)
                EnableKeepLastFrame((ushort)(Camera.CMS.Configure.KeepLastFrame ? 1 : 0));

            //_control.AudioTrack = 1; //not support?
            _control.Mute = 1;
            _control.Connect();

            /*var bghtml = Camera.Server.Configure.VideoTitleBarBackgroundColor.Replace("#", "");
            var r = bghtml.Substring(0, 2);
            var g = bghtml.Substring(2, 2);
            var b = bghtml.Substring(4, 2);
            var bg = UInt32.Parse(b + g + r, System.Globalization.NumberStyles.HexNumber);

            var colorhtml = Camera.Server.Configure.VideoTitleBarFontColor.Replace("#", "");
            r = colorhtml.Substring(0, 2);
            g = colorhtml.Substring(2, 2);
            b = colorhtml.Substring(4, 2);
            var color = UInt32.Parse(b + g + r, System.Globalization.NumberStyles.HexNumber);
            _control.SetTitleBarFormat(000000, bg, color, "", 10);*/
            
            //_control.SetControlActive(0);
            //_control.SetDecodeI(0);
            RaiseNetworkStatusChange();
        }

        public virtual void Stop()
        {
            PlayMode = PlayMode.Idle;

            if (_control == null || Camera == null) return;

            if (_control.IsFullScreen() == 1) _control.CloseFullScreenWindow();

            if (IsDisconnecting) return;
            if (NetworkStatus == NetworkStatus.Idle) return;

            IsConnecting = false;

            var status = NetworkStatus;
            if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting)
                IsDisconnecting = true;

            // Add By Tulip For Keep Last Image 
            //if (Camera.CMS.Configure.CameraLastImage)
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

            SetVisible(false);

            if (_adjustBrightness != 0)
            {
                _adjustBrightness = 0;
                _control.AdjustBrightness(0);
            }

            _control.Disconnect();

            _isOnArchiveServer = false;

            RaiseNetworkStatusChange();
        }

        public virtual void StopForAchive()
        {
            //if (isOnSwitchConnect) return;
            _control.Disconnect();
            isOnSwitchConnect = true;

            //PlayMode = PlayMode.Idle;

            //if (_control == null) return;

            //if (_control.IsFullScreen() == 1) _control.CloseFullScreenWindow();

            //if (IsDisconnecting) return;
            ////if (NetworkStatus == NetworkStatus.Idle) return;

            //IsConnecting = false;

            //var status = NetworkStatus;
            //if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Connecting)
            //    IsDisconnecting = true;

            //SetVisible(false);

            //if (_adjustBrightness != 0)
            //{
            //    _adjustBrightness = 0;
            //    _control.AdjustBrightness(0);
            //}

            //_control.OnDisconnect -= ControlOnDisconnectForArchiveServer;
            //_control.OnDisconnect += ControlOnDisconnectForArchiveServer;

            //_control.Disconnect();

            //_isOnArchiveServer = false;

            //RaiseNetworkStatusChange();
        }

        private void ControlOnDisconnectForArchiveServer(object sender, EventArgs e)
        {
            _control.OnDisconnect -= ControlOnDisconnectForArchiveServer;
            Connect();
        }

        public void Reconnect()
        {
            if (_control == null || Camera == null) return;
            _control.Size = new Size(Width, Height);

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

        private void ControlOnConnect(Object sender,  AxiCMSViewerLib._IiCMSViewerEvents_OnConnectEvent e)
        {
            if (Camera == null) { Stop(); return; }
            
            Console.WriteLine("Connect Status: " + e.connectSuccessful);
            Console.WriteLine("Session Id: " + e.playback_sessionid);
            Camera.PlaybackSessionId = e.playback_sessionid;

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
                //GoTo(PlaybackTimecode);
                _control.Goto(PlaybackTimecode, (UInt16)PlayMode.GotoTimestamp);//1 Goto 2 1xPlayback
            }
          
            RaiseConnect(e.connectSuccessful);

            RaiseNetworkStatusChange();

            UpdateRecordStatus();
            _control.Size = new Size(Width, Height);
            _control.SetDisplayTimeZone(_timezone);
        }

        private void ControlOnConnectionRecovery(Object sender,  AxiCMSViewerLib._IiCMSViewerEvents_OnConnectionRecoveryEvent e)
        {
            if (Camera == null)
            {
                Stop();
                return;
            }
            _control.Size = new Size(Width, Height);
            Camera.PlaybackSessionId = e.playback_sessionid;

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

            var status = NetworkStatus;
            if (Camera.CMS != null)
            {
                if (Camera.CMS.Configure.KeepLastFrame && (status == NetworkStatus.Reconnect || status == NetworkStatus.Reconnecting || status == NetworkStatus.Idle))
                {
                    SetVisible(_control.IsLiveLastFrameExist() == 1);
                }
                else
                    SetVisible(false);
            }
            else
            {
                SetVisible(false);
            }

            //SetVisible(false);

            IsReconnecting = true;

            RaiseNetworkStatusChange();
        }

        private void ControlOnDisconnect(Object sender, EventArgs e)
        {
            Console.WriteLine("ControlOnDisconnect");
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

            if(isOnSwitchConnect)
            {
                Connect();
                isOnSwitchConnect = false;
            }
        }
    }
}
