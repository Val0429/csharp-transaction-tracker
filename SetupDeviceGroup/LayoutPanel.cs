using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class LayoutPanel : UserControl
    {
        public IApp App;
        public IServer Server;
        public IDeviceGroup Group;

        public LayoutPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private Boolean _isEditing;
        private Pager.Pager _pager;
        private VideoMonitor.VideoMonitor _monitor;
        public void Initialize()
        {
            _monitor = new VideoMonitor.VideoMonitor { App = App, Server = Server };
            containerPanel.Controls.Add(_monitor);
            _monitor.Initialize();
            _monitor.SetEditProperty();

            _pager = new Pager.Pager
                {
                    MinimumSize = new Size(0, 40),
                    Height = 40,
                    Padding = new Padding(5, 5, 5, 5),
                    Dock = DockStyle.Top,
                    Server = Server
                };

            _pager.SetLayoutProperty();
            pagerPanel.Controls.Add(_pager);

            _pager.OnLayoutChange += PagerOnLayoutChange;
            _monitor.OnLayoutChange += MonitorOnLayoutChange;
        }

        private void MonitorOnLayoutChange(Object sender, EventArgs<System.Collections.Generic.List<WindowLayout>> e)
        {
            _pager.LayoutChange(sender, e);

            if (_isEditing && Group != null)
                Server.GroupModify(Group);
        }

        private void PagerOnLayoutChange(Object sender, EventArgs<System.Collections.Generic.List<WindowLayout>> e)
        {
            _monitor.LayoutChange(sender, e);
        }

        public void ShowLayout()
        {
            _isEditing = false;
            _monitor.ClearAll();
            _monitor.ShowGroup(Group);
            _isEditing = true;
        }
    } 
}
