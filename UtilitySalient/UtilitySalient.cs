using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using Interface;
namespace UtilitySalient
{
	public partial class UtilitySalient : UserControl, IUtility
	{
		public event EventHandler<EventArgs<JoystickEvent>> OnMoveAxis;
		public event EventHandler<EventArgs<Dictionary<UInt16, Boolean>>> OnClickButton;
	    public event EventHandler<EventArgs<AxisJoystickEvent>> OnAxisJoystickRotate;
	    public event EventHandler<EventArgs<AxisJoystickButton>> OnAxisJoystickButtonDown;
	    public event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialRotate;
	    public event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialShuttle;
	    public event EventHandler<EventArgs<AxisJogDialButton>> OnAxisJogDialButtonDown;
	    public event EventHandler<EventArgs<AxisKeyPadButton>> OnAxisKeyPadButtonDown;

	    private readonly System.Timers.Timer _completedTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _timeoutTimer = new System.Timers.Timer();
		public UtilitySalient()
		{
			InitializeComponent();

			JoystickEnabled = false;
            _control.OnExportProgress += ControlOnExportProgress;
			
			AudioOutChannelCount = 0;

			_completedTimer.Interval = 2000;
			_completedTimer.Elapsed += ExportCompleted;
			_completedTimer.SynchronizingObject = this;


			_timeoutTimer.Interval = 5000;
			_timeoutTimer.Elapsed += ExportFailure;
			_timeoutTimer.SynchronizingObject = this; 
		}

		private void ControlOnLoginConnected(object sender, EventArgs e)
		{
            _control.OnConnected -= ControlOnLoginConnected;
            _control.OnFailedConnection -= ControlOnLoginFailedConnection;

			SetSalientNVRTimeZone();
		}

		private void ControlOnLoginFailedConnection(Object sender, AxCVClientControlLib._ICVVideoEvents_OnFailedConnectionEvent e)
		{
            _control.OnConnected -= ControlOnLoginConnected;
            _control.OnFailedConnection -= ControlOnLoginFailedConnection;
			
			SetSalientNVRTimeZone();
		}

		private void SetSalientNVRTimeZone()
		{
			Int32 time;
			String zone;
            _control.GetServerTimeZone(out time, out zone);

			if (zone != null)
			{
				_nvr.Server.TimeZone = time;
			}
			else
			{
				DateTime local;
				DateTime utc;
                _control.GetServerTime(out local, out utc);
				_nvr.Server.TimeZone = Convert.ToInt32((local - utc).TotalSeconds);
			}
            _control.Disconnect();
		}

		private IServer _nvr;
		public IServer Server
		{
			set
			{
				_nvr = value;
				if (_nvr is INVR)
				{
                    _control.OnConnected -= ControlOnLoginConnected;
                    _control.OnConnected += ControlOnLoginConnected;
                    _control.OnFailedConnection -= ControlOnLoginFailedConnection;
                    _control.OnFailedConnection += ControlOnLoginFailedConnection;

                    var server = String.Format("{0}:{1}", _nvr.Credential.Domain, _nvr.Credential.Port);

                    _control.Server = _nvr.Credential.Domain;
                    _control.Username = _nvr.Credential.UserName;
                    _control.Password = _nvr.Credential.Password;
                    _control.Camera = 1;
                    _control.Connect();
				}
			}
		}

		public String Version
		{
			get
			{
				return "4.3.0.8";
			}
		}

		public String ComponentName
		{
			get
			{
				return "CVServerControl";
			}
		}

		public void Quit()
		{
		}

		public void StartEventReceive()
		{
		}

		public void StopEventReceive()
		{
		}

		public void UpdateEventRecive()
		{
		}

		public void GetAllChannelStatus()
		{
		}

		public Boolean JoystickEnabled { get; private set; }

		public void InitializeJoystick()
		{
		}

	    public void InitializeAxisJoystick()
	    {

	    }

	    public void StartJoystickTread()
		{
		}

		public void StopJoystickTread()
		{
		}

		public void PlaySystemSound(UInt16 times, UInt16 duration, UInt16 interval)
		{
		}

		public Int32 AudioOutChannelCount { get; private set; }
		public Int32 StartAudioTransfer(String channels)
		{
			return 0;
		}
		public void StopAudioTransfer()
		{
		}

		public static void CreateDirectory(String path)
		{
			if (Directory.Exists(path) || path.Equals(String.Empty)) return;

			var paths = path.Split('\\');
			path = paths[0];

			for (var i = 1; i < paths.Length; i++)
			{
				path = path + "\\" + paths[i];

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			}
		}

		private ICamera _exportingCamera;
		private DateTime _start;
		private DateTime _end;
		private String _filename;
		private String _path;

		public String ExportVideo(ICamera camera, UInt64 startTime, UInt64 endTime, Boolean displayOsd, Boolean audioIn, Boolean audioOut, UInt16 encode, String path, UInt16 quality, UInt16 scale, UInt16 osdWatermark) //Boolean encode
		{
			_exportingCamera = camera;
			_path = path;

            _control.Server = camera.Server.Credential.Domain;
            _control.Username = camera.Server.Credential.UserName;
            _control.Password = camera.Server.Credential.Password;
            _control.Camera = Convert.ToInt16(camera.Id);

			var start = DateTimes.ToDateTime(startTime, camera.Server.Server.TimeZone);
			var end = DateTimes.ToDateTime(endTime, camera.Server.Server.TimeZone);
			_start = start;
			_end = end;

			CreateDirectory(path);

			_filename = Regex.Replace(camera.ToString(), "[^a-zA-Z0-9 \\-]", "") + "_" +
						   start.ToString("yyyy-MM-dd-HH-mm-ss") + "_" +
						   end.ToString("yyyy-MM-dd-HH-mm-ss") + ".avi";

            _control.OnConnected += ControlOnConnected;
            _control.OnFailedConnection += ControlOnFailedConnection;
            _control.Connect();

			return _filename;
		}

		private System.Timers.Timer _exportVideoTimer;
		private void ControlOnConnected(Object sender, EventArgs e)
		{
            _control.OnConnected -= ControlOnConnected;
            _control.OnFailedConnection -= ControlOnFailedConnection;

            _control.Playback(_start, _end);
			RetryGoto(_start);

			//wait 1sec to get snapshot, //salient's limitation
			if (_exportVideoTimer == null)
			{
				_exportVideoTimer = new System.Timers.Timer(1000);
				_exportVideoTimer.Elapsed += ExportVideo;
				_exportVideoTimer.SynchronizingObject = this;
			}
			_retryExportVideoTimes = 5;
			_exportVideoTimer.Enabled = true;
			//_control.ExportClip(Path.Combine(_path, _filename));

		}

		private UInt16 _retryExportVideoTimes = 5;
		private void ExportVideo(Object sender, EventArgs e)
		{
			_exportVideoTimer.Enabled = false;

			if (Math.Abs((_control.Position() - _start).TotalSeconds) < 3)
			{
                _control.ExportClip(Path.Combine(_path, _filename));
				_timeoutTimer.Enabled = true;
				return;
			}

			if (_retryExportVideoTimes > 0)//retry 5 times wait for export video
			{
				_retryExportVideoTimes--;
				_exportVideoTimer.Enabled = true;
				return;
			}

			_exportingCamera.ExportVideoProgress(100, ExportVideoStatus.ExportFailed);
		}

		private Timer _retryGotoTimer;
		private void RetryGoto(DateTime now)
		{
			var retry = 10;
			//stop previous timer
			if (_retryGotoTimer != null)
				_retryGotoTimer.Enabled = false;

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

		private void ControlOnFailedConnection(Object sender, AxCVClientControlLib._ICVVideoEvents_OnFailedConnectionEvent e)
		{
            _control.OnConnected -= ControlOnConnected;
            _control.OnFailedConnection -= ControlOnFailedConnection;

			_exportingCamera.ExportVideoProgress(100, ExportVideoStatus.ExportFailed);
		}

		public void StopExportVideo()
		{
			_exportingCamera = null;
			_timeoutTimer.Enabled = false;
		}

		private void ControlOnExportProgress(Object sender, AxCVClientControlLib._ICVVideoEvents_OnExportProgressEvent e)
		{
			if (_exportingCamera == null)
			{
				_timeoutTimer.Enabled = false;
				return;
			}

			//if(e.pos == _end)
			//    status = ExportVideoStatus.ExportAVIFinished;
			//Console.WriteLine(e.pos + @"   " + e.exportId);
			
			//re count 5 secs
			_timeoutTimer.Enabled = true;

            var currentDatetime = DateTimes.ToDateTime(DateTimes.ToUtc(e.pos, 0), _nvr.Server.TimeZone);//modify to local time
            var progress = Convert.ToUInt16(((currentDatetime.Ticks - _start.Ticks * 1.0) / (_end.Ticks - _start.Ticks * 1.0)) * 100);

			if (progress == 100)
			{
                _timeoutTimer.Enabled = false;
				_completedTimer.Enabled = false;
				_completedTimer.Enabled = true;
			}

			_exportingCamera.ExportVideoProgress(progress, ExportVideoStatus.ConvertToAVI);
		}

		private void ExportCompleted(Object sender, EventArgs e)
		{
			_timeoutTimer.Enabled = false;
			_completedTimer.Enabled = false;
			if (_exportingCamera == null) return;

			var camera = _exportingCamera;
			_exportingCamera = null;

			camera.ExportVideoProgress(100, ExportVideoStatus.ExportFinished);
		}

		private void ExportFailure(Object sender, EventArgs e)
		{
			_timeoutTimer.Enabled = false;
			if (_exportingCamera == null) return;

			var camera = _exportingCamera;
			_exportingCamera = null;

			camera.ExportVideoProgress(100, ExportVideoStatus.ExportFailed);
		}

		public void UploadPack(String fileName)
		{
		}

		public void StopUploadPack(String fileName)
		{
		}

	    public void InitFisheyeLibrary(ICamera camera, bool dewarpEnable, short mountType)
	    {

	    }

	    public void GetAllNVRStatus()
		{
		}
	}
}