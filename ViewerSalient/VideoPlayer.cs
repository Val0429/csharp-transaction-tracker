using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using Constant;
using CVClientControlLib;
using DeviceConstant;
using Interface;
using Timer = System.Windows.Forms.Timer;

namespace ViewerSalient
{
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisibleAttribute(true)]

	public sealed partial class VideoPlayer : UserControl, IViewer
	{
		public event EventHandler OnFullScreen;
		public event EventHandler<EventArgs<String>> OnCloseFullScreen;

		public event EventHandler OnNetworkStatusChange;

		public Boolean TranscodeStream { get; set; }
		public Boolean IsDigitalPtzZoom
		{
			get { return false; }
		}
		public Int32 AdjustBrightness { get; set; }
		public event EventHandler<EventArgs<Int32>> OnConnect;
		public event EventHandler<EventArgs<Int32>> OnPlay;
		public event EventHandler<EventArgs<Int32>> OnDisconnect;
		public event EventHandler<EventArgs<Int32, Int32, Int32>> OnMouseKeyDown;
		public event EventHandler<EventArgs<String>> OnFrameTimecodeUpdate;
		public event EventHandler<EventArgs<Int32>> OnBitrateUpdate;

		public VideoPlayer()
		{
			InitializeComponent();

			_control.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
			Dock = DockStyle.None;
			//DoubleBuffered = true;
			PlayMode = PlayMode.Idle;
		}

		public String Version
		{
			get
			{
				return "4.1.1.8";
			}
		}

		public String ComponentName
		{
			get
			{
				return "CVClientControl";
			}
		}

        public void SetPlaySpeed(UInt16 speed)
        {

        }

        public void EnablePlaybackSmoothMode(UInt16 mode)
        {

        }

	    public void EnableKeepLastFrame(ushort enable)
	    {
	    }

	    public void SetDigitalPtzRegion(string xmlDoc)
	    {
	        
	    }

        public void SetDigitalPtzRegionCount(UInt16 count)
        {

        }

	    public void AutoDropFrame()
		{
		}

		public void DecodeIframe()
		{
		}

		public void SwitchVideoStream(UInt16 streamId)
		{
		}
		
		public new void Focus()
		{
			base.Focus();
			if (_control != null)
				_control.Focus();
		}

		public void SetupMotionStart()
		{
			PtzMode = PTZMode.Disable;
		}

		public String GetMotionRegion()
		{
			return "";
		}

		public void SetupMotionEnd()
		{
            
		}

		public void EnableMotionDetection(Boolean enable)
		{
		}

		public void SetSubLayoutRegion(ISubLayout subLayout)
		{
		}

	    public void SetSubLayoutRegion(List<ISubLayout> subLayouts)
	    {
	        
	    }

	    public void UpdateSubLayoutRegion(ISubLayout subLayout)
		{
		}

	    public string UpdateSubLayoutRegion()
	    {
	        return null;
	    }

	    public void SetVisible(Boolean visible)
		{
			Visible = visible;
		}

		public UInt16 Port
		{
			set
			{
			}
		}

		public String Host
		{
			set
			{
				if (_control != null)
					_control.Server = value;
			}
			get
			{
				return (_control != null) ? _control.Server : "";
			}
		}

		public String UserName
		{
			set
			{
				if (_control != null)
					_control.Username = value;
			}
		}

		public String UserPwd
		{
			set
			{
				if (_control != null)
					_control.Password = value;
			}
		}

		public IApp App { get; set; }
		public ICamera Camera { get; set; }

		public PlayMode PlayMode { get; set; }

		public Boolean Active
		{
			set
			{
			}
		}

		private ViewOptions _viewOptions;
		public Boolean StretchToFit
		{
			get
			{
				return (_viewOptions == ViewOptions.VIEW_STRETCH_TO_FIT);
			}
			set
			{
				if (value)
				{
					_viewOptions = ViewOptions.VIEW_STRETCH_TO_FIT;
				}
				else
				{
					_viewOptions = ViewOptions.VIEW_LETTERBOX;
				}
				_control.SetViewOption(_viewOptions);
			}
		}

		public Boolean Dewarp { get; set; }

        private Int16 _dewarpType = 0;
        public short DewarpType
        {
            get { return _dewarpType; }
            set { _dewarpType = value; }
        }

	    private NetworkStatus _networkStatus = NetworkStatus.Idle;
		public NetworkStatus NetworkStatus
		{
			get
			{
				try
				{
					if (_control == null) return NetworkStatus.Idle;

					if (_isConnecting)
						return NetworkStatus.Connecting;

					return _networkStatus;
				}
				catch (Exception)
				{
					return NetworkStatus.Idle;
				}
			}
		}

		public PTZMode PtzMode
		{
			get
			{
				return PTZMode.Disable;
			}
			set
			{
			}
		}

		public UInt64 Timecode
		{
			get
			{
				try
				{
					var date = _control.Position();
                    Console.WriteLine("Position : " + date.ToString());
					if (date < new DateTime(1970, 1, 1, 0, 0, 0))
						return 0;
					return DateTimes.ToUtc(date, Camera.Server.Server.TimeZone);
				}
				catch(Exception)
				{
				}
				return 0;
			}
		}

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

		public String Title
		{
			set
			{
			}
		}

		public Int32 TimeZone
		{
			set
			{
			}
		}

		private Boolean _isConnecting;
		public void Connect()
		{
			if (_control == null || Camera == null || Camera.Server == null || Camera.ReadyState == ReadyState.New) return;
			if (NetworkStatus != NetworkStatus.Idle) return;

			_networkStatus = NetworkStatus.Connecting;

            var server = String.Format("{0}:{1}", Camera.Server.Credential.Domain, Camera.Server.Credential.Port);

            Host = Camera.Server.Credential.Domain;
			UserName = Camera.Server.Credential.UserName;
			UserPwd = Camera.Server.Credential.Password;

			_control.SetCameraOverlay(Convert.ToInt16(Camera.Id), "", true, true);
			TimeZone = Camera.Server.Server.TimeZone;
			_isConnecting = true;
			
			_control.Camera = Convert.ToInt16(Camera.Id);

			_control.Connect();

			if (OnNetworkStatusChange != null)
				OnNetworkStatusChange(this, null);
		}

		private Boolean _displayTitleBar;
		public Boolean DisplayTitleBar
		{
			get
			{
				return _displayTitleBar;
			}
			set
			{
				_displayTitleBar = value;
			}
		}

		private Boolean _audioIn;
		public Boolean AudioIn {  
			get
			{
				return _audioIn;
			}
			set
			{
				if(_control == null) return;
				_audioIn = value;
				_control.EnableAudio(value);
			}
		}

		public void Play()
		{
			if (_control == null || Camera == null) return;

			//Console.WriteLine("Live");

			PlayMode = PlayMode.LiveStreaming;

			//PlayMode = DeviceProfile.PlayMode.Live;
			var status = NetworkStatus;
			if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming)
			{
				//no nothing, it will automatic play
			}
			else
			{
				if (App.CustomStreamSetting.Enable)
				{
                    _control.XResolution = Resolutions.ToWidth(App.CustomStreamSetting.Resolution);
                    _control.YResolution = Resolutions.ToHeight(App.CustomStreamSetting.Resolution);
                    _control.FPS = App.CustomStreamSetting.Framerate == 0 ? -1 : App.CustomStreamSetting.Framerate;
				}
				else
				{
					_control.XResolution = -1;
					_control.YResolution = -1;
				    _control.FPS = -1;
				}

				Connect();
			}
		}

		public void GoTo(UInt64 timecode)
		{
			if (timecode == 0) return;

			PlaybackTimecode = timecode;
			if (_control == null || Camera == null) return;

			//Console.WriteLine("Playback GoTo"););

			PlayMode = PlayMode.GotoTimestamp;

			//Console.WriteLine(NetworkStatus + " " + timecode + " " + PlayMode);
			var status = NetworkStatus;
			if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming)
			{
				var now = DateTimes.ToDateTime(PlaybackTimecode, Camera.Server.Server.TimeZone);
				_control.Stop();
				if (now < _start || now > _end || (_start == DateTime.MinValue || _end == DateTime.MaxValue))
				{
				    Console.WriteLine("GOTO:" + now);
                    //_start = now;// now.AddSeconds(-1200);//1hrs,  -43200  12hrs
                    _start = now.AddSeconds(-3000);
                    _end = now.AddSeconds(3000);//1hrs,  43200 12hrs

                    Host = Camera.Server.Credential.Domain;
                    UserName = Camera.Server.Credential.UserName;
                    UserPwd = Camera.Server.Credential.Password;

                    //_control.SetCameraOverlay(Convert.ToInt16(Camera.Id), "", true, true);
                    //TimeZone = Camera.Server.Server.TimeZone;
                    //_isConnecting = true;

                    _control.Camera = Convert.ToInt16(Camera.Id);

				    try
				    {
                        _control.Playback(_start, _end);
				    }
				    catch (Exception exception)
				    {
                        Console.WriteLine("Exception:" + exception.ToString());
				    }
					

					_control.Stop();
				}
				
				if (PlaybackTimecode > 0)
				{
					_control.Seek(now);
                    Console.WriteLine("Seek :" + now.ToString());
					//dont have to retry now, because start = now
					//RetryGoto(now);
				}

				//var date = _control.Position();
			}
			else
			{
				OnConnect -= VideoPlayerOnPlaybackConnect;
				OnConnect += VideoPlayerOnPlaybackConnect;

                if (App.CustomStreamSetting.Enable)
                {
                    _control.XResolution = Resolutions.ToWidth(App.CustomStreamSetting.Resolution);
                    _control.YResolution = Resolutions.ToHeight(App.CustomStreamSetting.Resolution);
                    _control.FPS = App.CustomStreamSetting.Framerate == 0 ? -1 : App.CustomStreamSetting.Framerate;
                }
                else
                {
                    _control.XResolution = -1;
                    _control.YResolution = -1;
                    _control.FPS = -1;
                }


				Connect();
			}
		}

		public void Playback(UInt64 timecode)
		{
			if (timecode == 0) return;

			PlaybackTimecode = timecode;
			if (_control == null || Camera == null) return;

			//Console.WriteLine("Playback 1X");

			PlayMode = PlayMode.Playback1X;

			//Console.WriteLine(NetworkStatus + " " + timecode + " " + PlayMode);
			var status = NetworkStatus;
			if (status == NetworkStatus.Connected || status == NetworkStatus.Streaming)
			{
				var now = DateTimes.ToDateTime(PlaybackTimecode, Camera.Server.Server.TimeZone);

				if (now < _start || now > _end || (_start == DateTime.MinValue || _end == DateTime.MaxValue))
				{
					_start = now;//now.AddSeconds(-1200);//1hrs,  -43200  12hrs
					_end = now.AddSeconds(1200);//1hrs,  43200 12hrs
					_control.Playback(_start, _end);

					_control.Stop();
				}

				if (PlaybackTimecode > 0)
				{
					_control.PlaybackRate(1.0);
					_control.Seek(now);
					_control.Start();
				}
			}
			else
			{
				OnConnect -= VideoPlayerOnPlaybackConnect;
				OnConnect += VideoPlayerOnPlaybackConnect;

                if (App.CustomStreamSetting.Enable)
                {
                    _control.XResolution = Resolutions.ToWidth(App.CustomStreamSetting.Resolution);
                    _control.YResolution = Resolutions.ToHeight(App.CustomStreamSetting.Resolution);
                    _control.FPS = App.CustomStreamSetting.Framerate == 0 ? -1 : App.CustomStreamSetting.Framerate;
                }
                else
                {
                    _control.XResolution = -1;
                    _control.YResolution = -1;
                    _control.FPS = -1;
                }


				Connect();
			}
		}

		public void Stop()
		{
			PlayMode = PlayMode.Idle;
			_audioIn = false;
			if (_control == null) return;
			
			if (NetworkStatus == NetworkStatus.Idle) return;
			
			_isConnecting = false;

			SetVisible(false);
			_networkStatus = NetworkStatus.Idle;
			_control.Disconnect();

			if (OnNetworkStatusChange != null)
				OnNetworkStatusChange(this, null);

			if (OnDisconnect != null)
				OnDisconnect(this, new EventArgs<Int32>(0));
		}

		public void Reconnect()
		{
			if (_control == null || Camera == null) return;

			//if (PlayMode == PlayMode.Playback)
			//    PlaybackTimecode = (UInt64) Int64.Parse(_control.TimeCode);
			PlayMode mode = PlayMode;
			Stop();
			switch (mode)
			{
				case PlayMode.LiveStreaming:
					Play();
					break;

				case PlayMode.GotoTimestamp:
					GoTo(PlaybackTimecode);
					break;

				case PlayMode.Playback1X:
					Playback(PlaybackTimecode);
					break;
			}
		}

		private String _previousCmd;
		public void SendPTZCommand(String cmd)
		{
			if (String.Equals(_previousCmd, cmd)) return;

			_previousCmd = cmd;
		}

		private DateTime _start = DateTime.MinValue;
		private DateTime _end = DateTime.MaxValue;
		private Timer _retryGotoTimer;
		private void VideoPlayerOnPlaybackConnect(Object sender, EventArgs<Int32> e)
		{
			OnConnect -= VideoPlayerOnPlaybackConnect;

			_networkStatus = NetworkStatus.Streaming;

			var now = DateTimes.ToDateTime(PlaybackTimecode, Camera.Server.Server.TimeZone);

			_start = now;// now.AddSeconds(-1200);//-43200  12hrs
			_end = now.AddSeconds(1200);//43200 12hrs
			_control.Playback(_start, _end);
			_control.Seek(now);

			if (PlayMode == PlayMode.Playback1X)
			{
                _control.PlaybackRate(1.0);
			    _control.Start();
			    RetryPlayback(now);
			}

			//dont have to retry now, because start = now
			//RetryGoto(now);
		}

        private void RetryPlayback(DateTime now)
        {
            var retry = 10;
            //stop previous timer
            if (_retryGotoTimer != null)
            {
                _retryGotoTimer.Enabled = false;
                _retryGotoTimer = null;
            }

            _retryGotoTimer = new Timer { Interval = 50 };
            _retryGotoTimer.Tick += (sender2, e2) =>
            {
                //timecode not right, pnly retry 20 times(1 secend)
                if (Math.Abs((_control.Position() - now).TotalSeconds) > 3 && retry-- > 0)
                {
                    _control.Start();
                    return;
                }
                _retryGotoTimer.Enabled = false;
                _retryGotoTimer = null;
            };

            _retryGotoTimer.Enabled = true;
        }

		private void RetryGoto(DateTime now)
		{
			var retry = 10;
			//stop previous timer
			if (_retryGotoTimer != null)
			{
				_retryGotoTimer.Enabled = false;
				_retryGotoTimer = null;
			}

			_retryGotoTimer = new Timer { Interval = 50 };
			_retryGotoTimer.Tick += (sender2, e2) =>
			{
				//timecode not right, pnly retry 20 times(1 secend)
				if (Math.Abs((_control.Position() - now).TotalSeconds) > 3 && retry-- > 0)
				{
					_control.Seek(now);
					return;
				}
				_retryGotoTimer.Enabled = false;
				_retryGotoTimer = null;
			};

			_retryGotoTimer.Enabled = true;
		}

		private void ControlOnConnected(object sender, EventArgs e)
		{
			 if(Camera.Model != null)
				Camera.Model.NumberOfAudioIn = Convert.ToUInt16(_control.HasAudio ? 1 : 0);

			if (Camera == null) { Stop(); return; }

			_networkStatus = NetworkStatus.Streaming;
			_isConnecting = false;

			//Console.WriteLine("ControlOnConnect " + _control.ConnectionStatus);

			if (PtzMode == PTZMode.Disable)
				PtzMode = PTZMode.Digital;

			SetVisible(true);
			_control.SetViewOption(_viewOptions);

			if (OnConnect != null)
				OnConnect(this, new EventArgs<Int32>(1));

			if (OnNetworkStatusChange != null)
				OnNetworkStatusChange(this, null);
		}

		private void ControlOnLostConnection(object sender, EventArgs e)
		{
			_networkStatus = NetworkStatus.Idle;
			//or it will continue use bindwidth(?)
			_control.Disconnect();

			//Salient cont reconnect automatic
			//_networkStatus = NetworkStatus.Reconnecting;
			
			_isConnecting = false;
			//Console.WriteLine("ControlOnDisconnect");

			SetVisible(false);

			if (OnNetworkStatusChange != null)
				OnNetworkStatusChange(this, null);
		}

		public void Snapshot(String filename, Boolean withTimestamp)
		{
			object snapshot;
			
			if (withTimestamp)
				_control.SetCameraOverlay((Int16)Camera.Id, "", true, true);
			
			var result = _control.GetSnapshot(filename, out snapshot);
			
			if (result == 0)
			{
				var data = snapshot as byte[];
				if (data != null)
				{
					if (String.IsNullOrEmpty(filename))
					{
						Clipboard.SetImage(Image.FromStream(new MemoryStream(data)));
					}
					else
					{
						File.WriteAllBytes(filename, data);
					}
				}
			}
		}

		public void ExportVideo()
		{
			_control.ExportWizard();
		}
		
		public void SetText(Int16 x, Int16 y, String text, Int16 fontsize, Int16 colorRed, Int16 colorGreen, Int16 colorBlue)
		{
		}

		public void CloseFullScreen()
		{
		}
		
		public void UserDefineEventTrigger(String msg) { }
		public void PreDefineEventTrigger(String msg) { }
		public UInt16 StreamId { get; set; }
		public void SwitchStreamId(UInt16 streamid) { }

		private void ControlOnFailedConnection(Object sender, AxCVClientControlLib._ICVVideoEvents_OnFailedConnectionEvent e)
		{
			_networkStatus = NetworkStatus.Idle;
			_control.Disconnect();
			
			if (OnConnect != null)
				OnConnect(this, new EventArgs<Int32>(0));
		}

		private void ControlOnMouseDown(Object sender, AxCVClientControlLib._ICVVideoEvents_OnMouseDownEvent e)
		{
			if (OnMouseKeyDown != null)
				OnMouseKeyDown(this, new EventArgs<Int32, Int32, Int32>(0, 0, 0));//e.button
		}

		public Boolean ShowRecordingStatus { get; set; }
		public void UpdateRecordStatus()
		{
		}

		public UInt16 ProfileId { get; set; }

		public void NextFrame()
		{
		}

		public void PreviousFrame()
		{
		}

	    public void InitFisheyeLibrary(bool dewarpEnable, ushort mountType)
	    {
	        
	    }

	    public void ShowRIPWindow(bool enable)
	    {
	        
	    }

	    public string GetDigitalPtzRegion()
	    {
	        return null;
	    }
	}
}
