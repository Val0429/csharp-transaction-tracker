using System;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using ServerProfile;
using VideoMonitor;

namespace InstantPlayback
{
    public partial class InstantPlaybackForm : Form
    {
        private DateTime _dateTime;
        private VideoWindow _videoWindow;
        private readonly IServer _server;
        private readonly ICamera _camera;

        public InstantPlaybackForm(String[] arguments)
        {
            //arguments = new[]
            //                {
            //                    "172.16.1.35", "82", "Admin", "123456", "0", "4",
            //                    "test", "1", "1331193179476"
            //                };

            InitializeComponent();

            //SERVER
            //IP, Port, Account, Password, Timezone

            //Device
            //ID, Stream Id, Timecode
            
            if(arguments.Length < 6)
            {
                MessageBox.Show(@"Variable is not match.", @"Information" , MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                throw new Exception();
            }

            try
            {
                _server = new NVR();
                _server.Credential = new ServerCredential
                {
                    Domain = arguments[0],
                    Port = Convert.ToUInt16(arguments[1]),
                    UserName = arguments[2],
                    Password = arguments[3]
                };
                _server.Initialize();

                _server.Server.TimeZone = Convert.ToInt32(arguments[4]);

                _camera = new Camera
                {
                    Id = Convert.ToUInt16(arguments[5]),
                    ReadyState = ReadyState.Ready,
                    Server = _server,
                };

                if (arguments.Length >= 7)
                    _camera.Name = arguments[6];

                if (arguments.Length >= 8)
                {
                    _camera.Profile = new CameraProfile
                    {
                        StreamId = Convert.ToUInt16(arguments[7])
                    };
                }
                else
                {
                    _camera.Profile = new CameraProfile
                    {
                        StreamId = 1
                    };
                }

                if (arguments.Length >= 9)
                {
                    _dateTime = (String.Equals(arguments[8], "0"))
                        ? new DateTime()
                        : DateTimes.ToDateTime(Convert.ToUInt64(arguments[8]), _server.Server.TimeZone);
                }
                else
                {
                    _dateTime = DateTime.UtcNow;
                    _dateTime = _dateTime.AddSeconds(_server.Server.TimeZone);
                }

                Shown += InstantPlaybackShown;
                Show();
            }
            catch(Exception exception)
            {
                MessageBox.Show(@"Parse variable error." + Environment.NewLine + exception, @"Information", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                throw new Exception();
            }
        }

        public void Initialize()
        {
            if (_camera == null) return;

            _videoWindow = VideoWindowProvider.RegistVideoWindow();
            _videoWindow.Stretch = _server.Configure.StretchPlaybackVideo;
            _videoWindow.Server = _server;

            _videoWindow.DisplayTitleBar = true;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = this;

            _videoWindow.Device = _camera;

            _videoWindow.StartInstantPlayback(_dateTime);
            _videoWindow.GoTo();
            _videoWindow.Active = true;
        }

        private void InstantPlaybackShown(Object sender, EventArgs e)
        {
            Shown -= InstantPlaybackShown;
            Initialize();
        }
    }
}
