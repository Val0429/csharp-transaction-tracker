using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using VideoMonitor;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public partial class PresetPointPanel : UserControl
    {
    	public IApp App;
        public IServer Server;
        private VideoWindow _videoWindow;
        private VideoMenu _toolMenu;
        public ICamera Camera;

        public Boolean IsEditing;
        private readonly List<PointPanel> _pointPanelList = new List<PointPanel>();
        public void Initialize()
        {
            IsEditing = false;
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;

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

            videoWindowPanel.Controls.Add(_toolMenu);
            _toolMenu.BringToFront();
            _toolMenu.GeneratePresetPointToolMenu();
            videoWindowPanel.SizeChanged += VideoWindowPanelSizeChanged;

            _videoWindow.ToolMenu = _toolMenu;
            _videoWindow.NoBorder();

            for (UInt16 i = 1; i <= 16; i++ )
            {
                _pointPanelList.Add(new PointPanel
                {
                    PointId = i,
                    Server = Server,
                });
            }
            _pointPanelList.Reverse();

            foreach (var panel in _pointPanelList)
            {
                pointPanel.Controls.Add(panel);
            }
            var titlePanel = new PointPanel {IsTitle = true};
            pointPanel.Controls.Add(titlePanel);
        }

        private void VideoWindowPanelSizeChanged(Object sender, EventArgs e)
        {
            _toolMenu.UpdateLocation();
        }

        public void ParseDevice()
        {
            IsEditing = false;

            _videoWindow.Stretch = Server.Configure.StretchLiveVideo;
            _videoWindow.Camera = Camera;

            foreach (var panel in _pointPanelList)
            {
                panel.Camera = Camera;
                panel.ParseDevice();
            }

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
            _videoWindow.PtzMode = PTZMode.Optical;
            pointPanel.AutoScrollPosition = new Point(0, 0);
        }

        public void Deactivate()
        {
            _videoWindow.Deactivate();
        }
    }
}
