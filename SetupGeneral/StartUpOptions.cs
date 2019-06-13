using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupGeneral
{
    public sealed partial class StartupOptions : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public StartupOptions()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"SetupGeneral_StartupOptions", "Startup Options"},
								   {"Control_General", "General"},
								   {"SetupGeneral_LatestView", "Latest View"},
								   {"SetupGeneral_TotalBitrate", "Bandwidth"},
								   {"SetupGeneral_FullScreen", "Full Screen"},
                                   {"SetupGeneral_VideoTitleBar", "Video Title Bar"},
								   {"SetupGeneral_HidePanel", "Hide Panel"},
								   {"Common_Hide", "Hide"},
								   {"Page_Live", "Live"},
								   {"SetupGeneral_Enabled", "Enabled"},
								   {"SetupGeneral_ViewTour", "View Patrol"},
								   {"SetupGeneral_GroupView", "View"},
								   {"SetupGeneral_None", "None"},

								   {"Menu_OriginalStreaming", "Original Streaming"},
								   {"Menu_1M", "1Mbps"},
								   {"Menu_512K", "512Kbps"},
								   {"Menu_256K", "256Kbps"},
								   {"Menu_56K", "56Kbps"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Name = "StartupOptions";
            Dock = DockStyle.None;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            restoreClientPanel.Paint += InputPanelPaint;
            titleBarPanel.Paint += InputPanelPaint;
            fullscreenPanel.Paint += InputPanelPaint;
            hidePanelPanel.Paint += InputPanelPaint;
            totalBitratePanel.Paint += InputPanelPaint;
            patrolDoubleBufferPanel.Paint += InputPanelPaint;
            groupPanel.Paint += InputPanelPaint;

            totalBitrateComboBox.Items.Add(Localization["Menu_OriginalStreaming"]);
            totalBitrateComboBox.Items.Add(Localization["Menu_1M"]);
            totalBitrateComboBox.Items.Add(Localization["Menu_512K"]);
            totalBitrateComboBox.Items.Add(Localization["Menu_256K"]);
            totalBitrateComboBox.Items.Add(Localization["Menu_56K"]);

            enabledCheckBox.Text = videoTitleBarCheckBox.Text = fullScreenCheckBox.Text = patrolCheckBox.Text = Localization["SetupGeneral_Enabled"];
            hidePanelCheckBox.Text = Localization["Common_Hide"];
        }

        public void SetSISView()
        {
            patrolDoubleBufferPanel.Visible = false;
            totalBitratePanel.Visible = false;
        }

        public void SetCommandCenterView()
        {
            patrolDoubleBufferPanel.Visible = false;
            groupPanel.Visible = false;
        }

        private Boolean _isEdit = false;
        public void ParseSetting()
        {
            _isEdit = true;

            groupComboBox.Items.Clear();

            var item = new ComboxItem(Localization["SetupGeneral_None"], "-1");
            groupComboBox.Items.Add(item);

            //if ("-1" == Server.Configure.StartupOptions.DeviceGroup)
            groupComboBox.SelectedItem = item;

            foreach (var deviceGroup in Server.Device.Groups)
            {
                item = new ComboxItem(deviceGroup.Value.ToString(), deviceGroup.Key.ToString());
                groupComboBox.Items.Add(item);

                if (deviceGroup.Key.ToString() == Server.Configure.StartupOptions.DeviceGroup)
                {
                    groupComboBox.SelectedItem = item;
                }
            }

            patrolCheckBox.Checked = Server.Configure.StartupOptions.GroupPatrol;
            PatrolCheckBoxClick(this, EventArgs.Empty);

            var text = Server.Configure.StartupOptions.TotalBitrate.ToString();

            switch (text)
            {
                case "1024":
                    totalBitrateComboBox.SelectedIndex = 1;
                    break;

                case "512":
                    totalBitrateComboBox.SelectedIndex = 2;
                    break;

                case "256":
                    totalBitrateComboBox.SelectedIndex = 3;
                    break;

                case "56":
                    totalBitrateComboBox.SelectedIndex = 4;
                    break;

                default:
                    totalBitrateComboBox.SelectedIndex = 0;
                    break;
            }

            hidePanelCheckBox.Checked = Server.Configure.StartupOptions.HidePanel;

            videoTitleBarCheckBox.Checked = Server.Configure.StartupOptions.VideoTitleBar;
            VideoTitleBarCheckBoxClick(this, EventArgs.Empty);

            fullScreenCheckBox.Checked = Server.Configure.StartupOptions.FullScreen;
            FullScreenCheckBoxClick(this, EventArgs.Empty);

            enabledCheckBox.Checked = Server.Configure.StartupOptions.Enabled;
            EnabledCheckBoxClick(this, EventArgs.Empty);

            _isEdit = false;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("SetupGeneral_" + control.Tag))
                Manager.PaintText(g, Localization["SetupGeneral_" + control.Tag]);
            else if (Localization.ContainsKey("Control_" + control.Tag))
                Manager.PaintText(g, Localization["Control_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void EnabledCheckBoxClick(Object sender, EventArgs e)
        {
            if (!enabledCheckBox.Checked)
            {
                videoTitleBarCheckBox.Enabled = 
                fullScreenCheckBox.Enabled = hidePanelCheckBox.Enabled =
                totalBitrateComboBox.Enabled = patrolCheckBox.Enabled = groupComboBox.Enabled = false;

                Server.Configure.StartupOptions.Enabled = false;
            }
            else
            {
                Server.Configure.StartupOptions.Enabled = true;
                videoTitleBarCheckBox.Enabled = 
                fullScreenCheckBox.Enabled = totalBitrateComboBox.Enabled = patrolCheckBox.Enabled = true;
                VideoTitleBarCheckBoxClick(this, EventArgs.Empty);
                FullScreenCheckBoxClick(this, EventArgs.Empty);
                PatrolCheckBoxClick(this, EventArgs.Empty);
            }
        }

        private void VideoTitleBarCheckBoxClick(Object sender, EventArgs e)
        {
            Server.Configure.StartupOptions.VideoTitleBar = videoTitleBarCheckBox.Checked;
        }

        private void FullScreenCheckBoxClick(Object sender, EventArgs e)
        {
            Server.Configure.StartupOptions.FullScreen = fullScreenCheckBox.Checked;

            hidePanelCheckBox.Enabled = !fullScreenCheckBox.Checked;
        }

        private void HidePanelCheckBoxClick(Object sender, EventArgs e)
        {
            Server.Configure.StartupOptions.HidePanel = hidePanelCheckBox.Checked;
        }

        private void PatrolCheckBoxClick(Object sender, EventArgs e)
        {
            Server.Configure.StartupOptions.GroupPatrol = patrolCheckBox.Checked;

            groupComboBox.Enabled = !patrolCheckBox.Checked;
        }

        private void GroupComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (_isEdit) return;

            Server.Configure.StartupOptions.DeviceGroup = ((ComboxItem)groupComboBox.SelectedItem).Value;
        }

        private void TotalBitrateComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            var index = totalBitrateComboBox.SelectedIndex;

            switch (index)
            {
                case 1:
                    Server.Configure.StartupOptions.TotalBitrate = 1024;
                    break;

                case 2:
                    Server.Configure.StartupOptions.TotalBitrate = 512;
                    break;

                case 3:
                    Server.Configure.StartupOptions.TotalBitrate = 256;
                    break;

                case 4:
                    Server.Configure.StartupOptions.TotalBitrate = 56;
                    break;
                default:
                    Server.Configure.StartupOptions.TotalBitrate = -1;
                    break;
            }
        }
    }
}
