using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using VideoMonitor;

namespace SetupDevice
{
    public partial class PresetPointPanel : UserControl
    {
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

            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
            _videoWindow = VideoWindowProvider.RegistVideoWindow();
            _videoWindow.Server = Server;

            _videoWindow.DisplayTitleBar = true;
            _videoWindow.Stretch = Server.Configure.StretchLiveVideo;
            _videoWindow.Stretch = false;
            _videoWindow.Dock = DockStyle.Fill;
            _videoWindow.Parent = videoWindowPanel;

            _toolMenu = new VideoMenu
            {
                PanelPoint = _videoWindow.Location,
                HasPlaybackPage = false,
                Server = Server,
            };

            videoWindowPanel.Controls.Add(_toolMenu);
            _toolMenu.BringToFront();
            _toolMenu.GeneratorPresetPointToolMenu();
            videoWindowPanel.SizeChanged += VideoWindowPanelSizeChanged;

            _videoWindow.ToolMenu = _toolMenu;
            _videoWindow.NoBorder();

            for (UInt16 i = 1; i <= 16; i++ )
            {
                _pointPanelList.Add(new PointPanel
                {
                    PointId = i
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

            _videoWindow.Device = Camera;
            _videoWindow.Stretch = false;

            foreach (var panel in _pointPanelList)
            {
                panel.Device = Camera;
                panel.ParseDevice();
            }

            IsEditing = true;
        }

        public void Activate()
        {
            _videoWindow.Activate();
            _videoWindow.Active = true;
            _videoWindow.PtzMode = PTZMode.Optical;
        }

        public void Deactivate()
        {
            _videoWindow.Deactivate();
        }
    }
}
