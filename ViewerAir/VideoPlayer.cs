using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace ViewerAir
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class VideoPlayer : UserControl, IViewer
    {
        public event EventHandler OnFullScreen;
        public event EventHandler<EventArgs<String>> OnCloseFullScreen;

        public event EventHandler OnNetworkStatusChange;
        protected void RaiseNetworkStatusChange()
        {
            if (OnNetworkStatusChange != null)
            {
                OnNetworkStatusChange(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs<Int32>> OnConnect;
        protected void RaiseConnect(int value)
        {
            if (OnConnect != null)
                OnConnect(this, new EventArgs<Int32>(value));
        }

        public event EventHandler<EventArgs<Int32>> OnPlay;
        protected void RaisePlay(int value)
        {
            if (OnPlay != null)
                OnPlay(this, new EventArgs<Int32>(value));
        }

        public event EventHandler<EventArgs<Int32>> OnDisconnect;
        protected void RaiseDisconnect(int value)
        {
            if (OnDisconnect != null)
                OnDisconnect(this, new EventArgs<Int32>(value));
        }

        public event EventHandler<EventArgs<String>> OnFrameTimecodeUpdate;
        public event EventHandler<EventArgs<Int32>> OnBitrateUpdate;
        public event EventHandler<EventArgs<Int32, Int32, Int32>> OnMouseKeyDown;
        protected void RaiseMouseKeyDown(int x, int y, int btn)
        {
            if (OnMouseKeyDown != null)
                OnMouseKeyDown(this, new EventArgs<Int32, Int32, Int32>(x, y, btn));
        }


        // Constructor
        public VideoPlayer()
        {
            InitializeComponent();

            _control.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
            Dock = DockStyle.None;
            DoubleBuffered = true;

            PlayMode = PlayMode.Idle;
        }

        public String Version
        {
            get
            {
                return _control.Version;
            }
        }

        public String ComponentName
        {
            get
            {
                return "NvrViewer";
            }
        }

        public void EnablePlaybackSmoothMode(UInt16 mode)
        {
            if (_control != null)
            {
                //_control.EnablePlaybackSmoothMode(mode);
            }
        }

        public void SetPlaySpeed(UInt16 speed)
        {
            if (_control != null)
            {
                _control.SetPlaySpeed(speed);
                Console.WriteLine("Rate speed : " + speed);
            }
        }

        public void InitFisheyeLibrary(Boolean dewarpEnable, UInt16 mountType)
        {
            //if (_control != null)
            //{
            //    var vendor = "immervision";
            //    if (String.IsNullOrEmpty(Camera.Profile.DewarpType) && Camera.Model.Type == "fisheye")
            //        vendor = "vivotek";
            //    _control.InitFisheyeLibrary(vendor, Camera.Profile.DewarpType, dewarpEnable ? 1 : 0, mountType);
            //}
        }

        public void ShowRIPWindow(Boolean enable)
        {
            if (_control == null)
                return;

            if (Camera == null)
                return;

            //if (String.IsNullOrEmpty(Camera.Profile.DewarpType) && Camera.Model.Type != "fisheye")
            //    return;

            _control.ShowRIPWindow(enable ? 1 : 0);
        }

        public String GetDigitalPtzRegion()
        {
            if (_control == null)
                return null;

            //if (String.IsNullOrEmpty(Camera.Profile.DewarpType) && Camera.Model.Type != "fisheye")
            //    return null;

            return _control.GetDigitalPtzRegion();
        }

        public void SetDigitalPtzRegion(String xmlDoc)
        {
            if (_control == null)
                return;

            if(String.IsNullOrEmpty(xmlDoc))
            {
                _control.DisableMouseDigitalPTZ();
                _control.EnableMouseDigitalPTZ();
                return;
            }

            _control.SetDigitalPtzRegion(xmlDoc);
        }

        public void SetDigitalPtzRegionCount(UInt16 count)
        {
            if (_control == null)
                return;

            _control.SetDigitalPtzRegionCount(count);
        }

        public void AutoDropFrame()
        {
            if (_control != null)
                _control.SetDecodeI(0);
        }

        public void DecodeIframe()
        {
            if (_control != null)
            {
                _control.SetDecodeI(1);
                //when decodeI -> audio-in disable
                AudioIn = false;
            }
        }

        public void SwitchVideoStream(UInt16 streamId)
        {
            ProfileId = streamId;
            //no need re-connect, just wait until call connect

            if (PlayMode != PlayMode.LiveStreaming) return;
            if (Camera == null) return;

            Reconnect();
        }

        public new void Focus()
        {
            base.Focus();
            if (_control != null)
                _control.Focus();
        }

        public void SetVisible(Boolean visible)
        {
            if (visible)
            {
                Visible = true;
                _control.ShowControl = 1;
            }
            else
            {
                _control.ShowControl = 0;
                Visible = false;
            }
        }

        public UInt16 Port
        {
            set
            {
                if (_control != null)
                    _control.ServerPort = value;
            }
        }

        public String Host
        {
            set
            {
                if (_control != null)
                    _control.ServerIp = value;
            }
            get
            {
                return (_control != null) ? _control.ServerIp : "";
            }
        }

        public String UserName
        {
            set
            {
                if (_control != null)
                    _control.ServerUsername = value;
            }
        }

        public String UserPwd
        {
            set
            {
                if (_control != null)
                    _control.ServerPassword = value;
            }
        }

        public IApp App { get; set; }
        private IDeviceLayout _deviceLayout;
        private ISubLayout _subLayout;
        private ICamera _camera;
        public virtual ICamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;

                if (_camera is IDeviceLayout)
                    _deviceLayout = _camera as IDeviceLayout;
                else
                    _deviceLayout = null;

                if (_camera is ISubLayout)
                    _subLayout = _camera as ISubLayout;
                else
                    _subLayout = null;
            }
        }

        public PlayMode PlayMode { get; set; }

        public Boolean Active
        {
            set
            {
                if (_control != null)
                {
                    _control.SetControlActive(value ? 1 : 0);
                }
            }
        }

        public Boolean StretchToFit
        {
            get
            {
                return (_control != null) && (_control.StretchToFit == 1);
            }
            set
            {
                if (_control != null)
                    _control.StretchToFit = (value) ? 1 : 0;
            }
        }

        Boolean _isDewarp;
        public Boolean Dewarp
        {
            get
            {
                return _isDewarp;
            }
            set
            {
                _isDewarp = false;
                if (_control == null) return;

                if (Camera != null && Camera.Profile != null && (!String.IsNullOrEmpty(Camera.Profile.DewarpType) || Camera.Model.Type == "fisheye"))
                {
                    _isDewarp = value;
                }
            }
        }

        public virtual NetworkStatus NetworkStatus
        {
            get
            {
                try
                {
                    if (_control == null) return NetworkStatus.Idle;

                    if (IsReconnecting)
                        return NetworkStatus.Reconnecting;

                    if (IsConnecting)
                        return NetworkStatus.Connecting;

                    if (IsDisconnecting)
                        return NetworkStatus.Disconnecting;

                    switch (_control.ConnectionStatus)
                    {
                        case "IDLE":
                            return NetworkStatus.Idle;
                        case "CONNECTING":
                            return NetworkStatus.Connecting;
                        case "CONNECTED":
                            return NetworkStatus.Connected;
                        case "STREAMING":
                            return NetworkStatus.Streaming;
                        case "DISCONNECTING":
                            return NetworkStatus.Disconnecting;
                        default:
                            return NetworkStatus.Idle;
                    }
                }
                catch (Exception)
                {
                    return NetworkStatus.Idle;
                }
            }
        }

        public String Title
        {
            set
            {
                if (_control != null)
                    _control.Caption = value;
            }
        }

        private Int32 _timezone;
        public Int32 TimeZone
        {
            set
            {
                _timezone = value;
                _control.SetDisplayTimeZone(_timezone);
            }
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

                try
                {
                    if (value)
                    {
                        _control.DisplayTitleBar(1);
                    }
                    else
                    {
                        _control.DisplayTitleBar(0);
                    }

                    UpdateRecordStatus();
                }
                catch (Exception)
                {
                }
            }
        }

        public Boolean AudioIn
        {
            get
            {
                if (_control == null) return false;

                try
                {
                    return (_control.Mute == 0);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                if (_control == null) return;

                try
                {
                    _control.Mute = (value) ? 0 : 1;
                }
                catch (Exception)
                {
                    _control.Mute = 1;
                }
            }
        }

        public void SetText(Int16 x, Int16 y, String text, Int16 fontsize, Int16 colorRed, Int16 colorGreen, Int16 colorBlue)
        {
            _control.SetTextOut(0, x - 1, y - 1, text, "Arial", 0, fontsize - 6, fontsize - 2, 0, 0, 0);
            _control.SetTextOut(1, x + 1, y - 1, text, "Arial", 0, fontsize - 6, fontsize - 2, 0, 0, 0);
            _control.SetTextOut(2, x - 1, y + 1, text, "Arial", 0, fontsize - 6, fontsize - 2, 0, 0, 0);
            _control.SetTextOut(3, x + 1, y + 1, text, "Arial", 0, fontsize - 6, fontsize - 2, 0, 0, 0);
            _control.SetTextOut(4, x, y, text, "Arial", 0, fontsize - 6, fontsize - 2, colorRed, colorGreen, colorBlue);
        }

        private static Image _record = Properties.Resources.record;
        private static Image _online = Properties.Resources.online;
        private static Image _normal = Properties.Resources.normal;
        public void UpdateRecordStatus()
        {
            if (Camera == null)
            {
                recordStatusPanel.Visible = false;
                return;
            }

            if (_displayTitleBar || App.IsFullScreen)
            {
                recordStatusPanel.Visible = true;
            }
            else
            {
                recordStatusPanel.Visible = false;
                return;
            }

            ICamera camera = null;

            if (_deviceLayout != null)
            {
                foreach (var device in _deviceLayout.Items)
                {
                    if (device == null) continue;
                    camera = device as ICamera;
                    break;
                }
            }
            else if (_subLayout != null)
            {
                var id = SubLayoutUtility.CheckSubLayoutRelativeCamera(_subLayout);
                if (id > 0 && _subLayout.Server.Device.Devices.ContainsKey(Convert.ToUInt16(id)))
                {
                    camera = _subLayout.Server.Device.Devices[Convert.ToUInt16(id)] as ICamera;
                }
            }
            else
                camera = Camera;

            //----------------------------------------------------------------------

            if (camera != null && camera.ReadyState != ReadyState.New)
            {
                switch (camera.Status)
                {
                    case CameraStatus.Recording:
                        recordStatusPanel.BackgroundImage = _record;
                        break;

                    case CameraStatus.Streaming:
                        recordStatusPanel.BackgroundImage = _online;
                        break;

                    default:
                        recordStatusPanel.BackgroundImage = _normal;
                        break;
                }
            }
        }

        private Boolean _withTimestamp;
        public void Snapshot(String filename, Boolean withTimestamp)
        {
            _withTimestamp = withTimestamp;

            //do it at thread will cause snapshot ane merge image can't get image at the process.
            //save to clipboard, no file to open.
            //control will help to do add water mark.
            _control.SnapShotWithWaterMark(1, filename, ((_withTimestamp) ? 1 : 0), 1, Camera.Id, Camera.ToString(), (int)(DateTimes.ToUtc(DateTime.Now, Camera.Server.Server.TimeZone/1000)));
        }

        public virtual void UserDefineEventTrigger(String msg) { }
        public virtual void PreDefineEventTrigger(String msg) { }
        public virtual UInt16 ProfileId { get; set; }

        public virtual Boolean TranscodeStream { get; set; }
        public Boolean IsDigitalPtzZoom
        {
            get
            {
                if (_control == null) return false;

                try
                {
                    return (_control.IsDigitalPtzZoom() == 1);
                }
                catch (Exception)
                {
                }

                return false;
            }
        }
    }
}
