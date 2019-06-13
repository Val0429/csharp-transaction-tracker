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

namespace CMSViewer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class CMSViewer : UserControl, IViewer
    {
        public event EventHandler OnFullScreen;
        public event EventHandler<EventArgs<String>> OnCloseFullScreen;

        public event EventHandler OnNetworkStatusChange;
        protected void RaiseNetworkStatusChange()
        {
            if (OnNetworkStatusChange != null)
                OnNetworkStatusChange(this, EventArgs.Empty);
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

        public CMSViewer()
        {
            InitializeComponent();
            
            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            PlayMode = PlayMode.Idle;
        }

        public void InitFisheyeLibrary(Boolean dewarpEnable, UInt16 mountType)
        {

        }

        public void EnableKeepLastFrame(UInt16 enable)
        {
            _control.KeepLiveLastFrame(enable);
        }

        public void ShowRIPWindow(Boolean enable)
        {

        }

        public String GetDigitalPtzRegion()
        {
            return null;
        }

        public void SetDigitalPtzRegion(String xmlDoc)
        {
         
        }

        public void SetDigitalPtzRegionCount(UInt16 count)
        {

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
                return "CMSViewer";
            }
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
        private ICamera _camera;
        public virtual ICamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;
            }
        }

        public virtual PlayMode PlayMode { get; set; }

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

                if (Camera != null && Camera.Profile != null && !String.IsNullOrEmpty(Camera.Profile.DewarpType))
                {
                    _isDewarp = value;
                    //_control.SetImmerVisionCameraDewarped((value) ? 1 : 0, Camera.Profile.DewarpType, 0);
                }
                //else
                    //_control.SetImmerVisionCameraDewarped(0, "", 0);
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
                        
                        case "RECONNECT":
                            return NetworkStatus.Reconnect;

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

            ICamera camera = Camera;

            //----------------------------------------------------------------------

            if (camera.ReadyState != ReadyState.New)
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

        public void Snapshot(String filename, Boolean withTimestamp)
        {
            _control.SnapShot(1, filename, ((withTimestamp) ? 1 : 0), 1);

            //save to clipboard, no file to open.
            if (String.IsNullOrEmpty(filename)) return;

            //wait until file create
            var retry = 5;
            while (!File.Exists(filename))
            {
                Thread.Sleep(200);
                retry--;
                if (retry == 0) break;
            }

            if (!File.Exists(filename)) return;

            // Add Watermark
            var checksum = "";
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    checksum = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
            var datetime = DateTimes.ToDateTimeString(Timecode, Camera.Server.Server.TimeZone, "yyyy/MM/dd HH:mm:ss");
            //2013/09/11 03:30:09,2013/09/11 03:46:49,05,4F4D20AC3B8ED550126908A4A1DE720F,EV6250A
            var str = String.Format("{0},{1},{2},{3},{4}", datetime, datetime, Camera.Id.ToString().PadLeft(2, '0'), checksum, Camera.Name);

            var bytes = Convert.FromBase64String(Encryptions.EncryptDES(str, "a$*D%#3("));

            var data = new byte[bytes.Length + 4];
            var len = BitConverter.GetBytes(bytes.Length);

            bytes.CopyTo(data, 0);
            len.CopyTo(data, bytes.Length);

            using (var sw = new FileStream(filename, FileMode.Append))
            {
                sw.Write(data, 0, data.Length);
                sw.Flush();
                sw.Close();
            }
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

                //try
                //{
                //    return (_control.IsDigitalPtzZoom() == 1);
                //}
                //catch (Exception)
                //{
                //}

                return false;
            }
        }
    }
}
