using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using VideoMonitor;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public partial class PtzPatrolPanel : UserControl
    {
    	public IApp App;
        public IServer Server;
        private VideoWindow _videoWindow;
        private VideoMenu _toolMenu;
        public ICamera Camera;
        public Dictionary<String, String> Localization;
        public Boolean IsEditing;
        private readonly List<RegionPanel> _pointPanelList = new List<RegionPanel>();

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, control);

            Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
        }

        private UInt16 _miniInterval = 5;
        private UInt16 _maxiInterval = 3600;
        public void Initialize()
        {
            Localization = new Dictionary<String, String>
                                {
                                    {"DevicePanel_PatrolInterval", "Patrol Interval"},
                                    {"Common_Sec", "Sec"},
                                    {"SetupGeneral_PatrolIntervaValue", "Patrol interval should between %1 secs to %2 hour"},
                                };
            Localizations.Update(Localization);

            IsEditing = false;
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;

            intervalPanel.Paint += PaintInput;
            intervalTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            intervalTextBox.TextChanged += IntervalTextBoxTextChanged;

            secLabel.Text = Localization["Common_Sec"];
            infoLabel.Text = Localization["SetupGeneral_PatrolIntervaValue"].Replace("%1", _miniInterval.ToString()).Replace("%2", (_maxiInterval / 60 / 60).ToString());

            BackgroundImage = Manager.BackgroundNoBorder;
            _videoWindow = VideoWindowProvider.RegistVideoWindow();
            
            _videoWindow.Server = Server;
            _videoWindow.App = App;
            _videoWindow.Initialize();

            _videoWindow.DisplayTitleBar = true;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = videoWindowPanel;

            _toolMenu = new VideoMenu
            {
                PanelPoint = _videoWindow.Location,
                HasPlaybackPage = false,
                Server = Server,
                App = App
            };

            //videoWindowPanel.Controls.Add(_toolMenu);
            _toolMenu.BringToFront();
            _toolMenu.GeneratePresetPointToolMenu();
            videoWindowPanel.SizeChanged += VideoWindowPanelSizeChanged;

            _videoWindow.ToolMenu = _toolMenu;
            _videoWindow.NoBorder();

            //for (UInt16 i = 1; i <= 16; i++ )
            //{
            //    _pointPanelList.Add(new RegionPanel
            //    {
            //        PointId = i,
            //        Server = Server,
            //    });
            //}
            //_pointPanelList.Reverse();

            //foreach (var panel in _pointPanelList)
            //{
            //    pointPanel.Controls.Add(panel);
            //}
            //var titlePanel = new RegionPanel { IsTitle = true };
            //pointPanel.Controls.Add(titlePanel);
        }

        private void IntervalTextBoxTextChanged(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(intervalTextBox.Text))
            {
                Camera.PatrolInterval = _miniInterval;
                CameraIsModify();
                return;
            }

            var value = Convert.ToUInt16(intervalTextBox.Text);
            if(value < _miniInterval)
            {
                value = _miniInterval;
            }
            else if (value > _maxiInterval)
            {
                value = _maxiInterval;
            }

            Camera.PatrolInterval = value;
            CameraIsModify();
        }

        public void CameraIsModify()
        {
            Server.DeviceModify(Camera);
        }

        private void VideoWindowPanelSizeChanged(Object sender, EventArgs e)
        {
            _toolMenu.UpdateLocation();
        }

        public void ParseDevice()
        {
            IsEditing = false;

            if(_pointPanelList.Count == 0)
            {
                foreach (KeyValuePair<UInt16, WindowPTZRegionLayout> windowPTZRegionLayout in Camera.PatrolPoints)
                {
                    _pointPanelList.Add(new RegionPanel
                    {
                        PointId = windowPTZRegionLayout.Key,
                        Server = Server,
                    });
                }
                _pointPanelList.Reverse();

                foreach (var panel in _pointPanelList)
                {
                    pointPanel.Controls.Add(panel);
                }
                var titlePanel = new RegionPanel { IsTitle = true };
                pointPanel.Controls.Add(titlePanel);
            }

            intervalTextBox.TextChanged -= IntervalTextBoxTextChanged;
            intervalTextBox.Text = Camera.PatrolInterval.ToString();
            intervalTextBox.TextChanged += IntervalTextBoxTextChanged;
            _videoWindow.Stretch = Server.Configure.StretchLiveVideo;
            _videoWindow.Camera = Camera;
            
            foreach (var panel in _pointPanelList)
            {
                panel.Camera = Camera;
                panel.VideoWindow = _videoWindow;
                panel.ParseDevice();
            }
            _toolMenu.UpdateLocation();
            IsEditing = true;
        }
       
        public void GlobalMouseHandler()
        {
            if (_videoWindow.Active)
                _videoWindow.GlobalMouseHandler();
        }

        public void Activate()
        {
        	_videoWindow.App = App;
            _videoWindow.Stretch = Server.Configure.StretchLiveVideo;
            _videoWindow.Activate();
            _videoWindow.Active = true;
            _videoWindow.PtzMode = PTZMode.Digital;
            _videoWindow.Viewer.SetDigitalPtzRegionCount(1);
            pointPanel.AutoScrollPosition = new Point(0, 0);
            //_toolMenu.GeneratorPresetPointToolMenu();
            //_toolMenu.UpdateLocation();
        }

        public void Deactivate()
        {
            _videoWindow.Deactivate();
        }
    }
}
