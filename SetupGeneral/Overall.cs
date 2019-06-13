using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupGeneral
{
    public partial class OverallControl : UserControl
    {
        public event EventHandler OnPatrolEdit;
        public event EventHandler OnDurationEdit;
        public event EventHandler OnMailServerEdit;
        public event EventHandler OnFtpServerEdit;
        public event EventHandler OnSaveImagePathEdit;
        public event EventHandler OnExportVideoPathEdit;
        public event EventHandler OnVideoWindowTitleBarEdit;
        public event EventHandler OnWatermarkEdit;
        public event EventHandler OnCPULoadingEdit;
        public event EventHandler OnAutoSwitchLiveVideoStreamEdit;
        public event EventHandler OnAutoSwitchDecodeIFrameEdit;
        public event EventHandler OnStartupOptionsEdit;
        public event EventHandler OnBandwidthControlEdit;
        public event EventHandler OnAutoLockApplicationEdit;

        public Dictionary<String, String> Localization;

        public IServer Server;
        public IApp App;
        public OverallControl()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Common_Sec", "Sec"},
								   {"Common_Min", "Min"},
								   {"Common_Hr", "Hr"},
								   
								   {"Page_Live", "Live"},
								   {"Page_Playback", "Playback"},

								   {"SetupGeneral_LivePatrolInterval", "Dwell Time of Patrol"},
								   {"SetupGeneral_DisplayDeviceId", "Display Device ID"},
								   {"SetupGeneral_DisplayGroupId", "Display View ID"},
                                   {"SetupGeneral_DisplayNVRId", "Display NVR ID"},
								   {"SetupGeneral_Display", "Display"},
								   {"SetupGeneral_ManualRecordDuration", "Manual record duration"},
								   {"SetupGeneral_SnapshotSaveImage", "Snapshot / Save image"},
								   {"SetupGeneral_MailServer", "Mail server"},
								   {"SetupGeneral_FTPServer", "FTP server"},
								   {"SetupGeneral_SaveImagePath", "Save image path"},
								   {"SetupGeneral_ExportVideoPath", "Export video path"},
								   {"SetupGeneral_TimestampWatermark", "Timestamp OSD"},
								   {"SetupGeneral_StorageAlert", "Storage disk not selected alert"},
								   {"SetupGeneral_CustomPlaybackSetting", "Custom playback setting"},
								   {"SetupGeneral_OriginalVideo", "Original video"},
								   {"SetupGeneral_StretchVideo", "Stretch video"},
								   {"SetupGeneral_VideoWindowTitleBar", "Video window title bar"},
								   {"SetupGeneral_CPULoadingUpperBoundary", "CPU loading upper boundary"},
								   {"SetupGeneral_AutoSwitchLiveVideoStream", "Auto switch live video stream"},
								   {"SetupGeneral_AutoSwitchDecodeIFrame", "Auto switch decode I-frame"},
								   {"SetupGeneral_BandwidthControlSetting", "Bandwidth Control Setting"},
                                   {"SetupGeneral_AutoLockApplicationTimer", "Auto Lock Application Timer"},
                                   {"SetupGeneral_TransactionTimeReference", "Transaction Time Reference"},
								   {"SetupGeneral_TransactionTimeOption1", "Time when transaction data is received by server"},
								   {"SetupGeneral_TransactionTimeOption2", "Time of transaction as shown on the transaction data"},
								   {"SetupGeneral_StartupOptions", "Startup Options"},
								   {"SetupGeneral_KeepLastFrame", "Keep the last scene received as a still when camera disconnects"},

									{"SetupGeneral_Enabled", "Enabled"},
									{"SetupGeneral_Disabled", "Disabled"},

								   {"SetupGeneral_Desktop", "Desktop"},
								   {"SetupGeneral_Document", "My Documents"},
								   {"SetupGeneral_Picture", "My Pictures"},
								   {"HandlePanel_Beep", "Beep"},
								   {"SetupGeneral_Patrol", "Patrol"},
								   {"SetupGeneral_LastPicture", "Keep the last scene received as a still when view patrols"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;

            _enableLastPicturePanel.Text = Localization["SetupGeneral_LastPicture"];
            _enableLastPictureCheckBox.Text = Localization["SetupGeneral_Enabled"];
            _enableLastPictureCheckBox.LostFocus += EnableLastPictureCheckBoxLostFocus;
        }


        private void EnableLastPictureCheckBoxLostFocus(object sender, EventArgs e)
        {
            Server.Configure.CameraLastImage = _enableLastPictureCheckBox.Checked;
        }

        public virtual void Initialize()
        {
            patrolDoubleBufferPanel.Paint += PatrolPanelPaint;
            displayDeviceIdDoubleBufferPanel.Paint += InputPanelPaint;
            displayGroupIdDoubleBufferPanel.Paint += InputPanelPaint;
            DisplayNVRIdDoubleBufferPanel.Paint += InputPanelPaint;
            recordDurationDoubleBufferPanel.Paint += RecordDurationPanelPaint;
            watermarkDoubleBufferPanel.Paint += InputPanelPaint;
            mailServerDoubleBufferPanel.Paint += MailServerPanelPaint;
            ftpServerDoubleBufferPanel.Paint += FtpServerPanelPaint;
            saveImagePathDoubleBufferPanel.Paint += SaveImagePathPanelPaint;
            exportVideoPathDoubleBufferPanel.Paint += ExportVideoPathPanelPaint;
            //exportVideoDoubleBufferPanel.Paint += ExportVideoDoubleBufferPanelPaint;
            storageAlertDoubleBufferPanel.Paint += InputPanelPaint;
            stretchVideoDoubleBufferPanel.Paint += InputPanelPaint;
            videoWindowtitleBarDoubleBufferPanel.Paint += VideoWindowTitleBarPanelPaint;
            watermarkdoubleBufferPanel2.Paint += WatermarkPanelPaint;
            cpuLoadingDoubleBufferPanel.Paint += CpuLoadingDoubleBufferPanelPaint;

            autoSwitchLiveVideoStreamDoubleBufferPanel.Paint += AutoSwitchLiveVideoStreamDoubleBufferPanelPaint;
            autoSwitchDecodeIFrameDoubleBufferPanel.Paint += AutoSwitchDecodeIFrameDoubleBufferPanelPaint;

            startupOptionsDoubleBufferPanel.Paint += StartupOptionsDoubleBufferPanelPaint;

            bandwidthControlDoubleBufferPanel.Paint += BandwidthControlDoubleBufferPanelPaint;
            transactionDoubleBufferPanel.Paint += InputPanelPaint;
            autoLockApplicationTimerDoubleBufferPanel.Paint += AutoLockApplicationTimerDoubleBufferPanelPaint;

            keepLastFrameDoubleBufferPanel.Paint += InputPanelPaint;

            withTimestampCheckBox.Text = Localization["SetupGeneral_TimestampWatermark"];
            withTimestampCheckBox.Checked = Server.Configure.ImageWithTimestamp;
            withTimestampCheckBox.CheckedChanged += WithTimestampCheckBoxCheckedChanged;

            displayDeviceIdCheckBox.Text = Localization["SetupGeneral_Display"];
            displayDeviceIdCheckBox.Checked = Server.Configure.DisplayDeviceId;
            displayDeviceIdCheckBox.CheckedChanged += DisplayDeviceIdCheckBoxCheckedChanged;

            displayGroupIdCheckBox.Text = Localization["SetupGeneral_Display"];
            displayGroupIdCheckBox.Checked = Server.Configure.DisplayGroupId;
            displayGroupIdCheckBox.CheckedChanged += DisplayGroupIdCheckBoxCheckedChanged;

            DisplayNVRIdCheckBox.Text = Localization["SetupGeneral_Display"];
            DisplayNVRIdCheckBox.Checked = Server.Configure.DisplayNVRId;
            DisplayNVRIdCheckBox.CheckedChanged += DisplayNVRIdCheckBoxCheckedChanged;

            beepCheckBox.Text = Localization["HandlePanel_Beep"];
            beepCheckBox.Checked = Server.Configure.StorageAlert;
            beepCheckBox.CheckedChanged += BeepCheckBoxCheckedChanged;

            liveStretchCheckBox.Text = Localization["Page_Live"];
            liveStretchCheckBox.Checked = Server.Configure.StretchLiveVideo;
            liveStretchCheckBox.CheckedChanged += LiveStretchCheckBoxCheckedChanged;

            playbackStretchCheckBox.Text = Localization["Page_Playback"];
            playbackStretchCheckBox.Checked = Server.Configure.StretchPlaybackVideo;
            playbackStretchCheckBox.CheckedChanged += PlaybackStretchCheckBoxCheckedChanged;

            keepLastFrameCheckBox.Text = Localization["SetupGeneral_Enabled"];
            keepLastFrameCheckBox.Checked = Server.Configure.KeepLastFrame;
            keepLastFrameCheckBox.Click += KeepLastFrameCheckBoxClick;

            transactionComboBox.Items.Add(Localization["SetupGeneral_TransactionTimeOption1"]);
            transactionComboBox.Items.Add(Localization["SetupGeneral_TransactionTimeOption2"]);

            _enableLastPictureCheckBox.Checked = Server.Configure.CameraLastImage;

            watermarklabel2.Visible = watermarkdoubleBufferPanel2.Visible = false;
            if (Server is ICMS)
            {
                recordDurationDoubleBufferPanel.Visible = recordDurationLabel.Visible =
                storageAlertDoubleBufferPanel.Visible = storageAlertLabel.Visible =
                transactionDoubleBufferPanel.Visible = transactionLabel.Visible = false;

                keepLastFrameDoubleBufferPanel.Visible = keepLastFrameLabel.Visible =
                DisplayNVRIdDoubleBufferPanel.Visible = DisplayNVRIdDisplayLabel.Visible = true;
            }
            else if (Server is IFOS)
            {
                patrolDoubleBufferPanel.Visible = patrolLabel.Visible =
                    displayGroupIdDoubleBufferPanel.Visible = displayDeviceIdDoubleBufferPanel.Visible = displayDeviceIdLabel.Visible =
                    recordDurationDoubleBufferPanel.Visible = recordDurationLabel.Visible =
                    watermarkDoubleBufferPanel.Visible = watermarkLabel.Visible =
                    ftpServerDoubleBufferPanel.Visible = ftpServerLabel.Visible =
                    saveImagePathDoubleBufferPanel.Visible = saveImagePathLabel.Visible =
                    exportVideoPathDoubleBufferPanel.Visible = exportVideoPathLabel.Visible =
                    storageAlertDoubleBufferPanel.Visible = storageAlertLabel.Visible =
                    stretchVideoDoubleBufferPanel.Visible = stretchVideoLabel.Visible =
                    bandwithControlLabel.Visible = bandwidthControlDoubleBufferPanel.Visible =
                    transactionDoubleBufferPanel.Visible = transactionLabel.Visible =
                    autoLockApplicationTimerDoubleBufferLabel.Visible = autoLockApplicationTimerDoubleBufferPanel.Visible =
                    autoSwitchLiveVideoStreamLabel.Visible = autoSwitchLiveVideoStreamDoubleBufferPanel.Visible =
                    autoSwitchDecodeIFrameLabel.Visible = autoSwitchDecodeIFrameDoubleBufferPanel.Visible =
                    startOptionsLabel.Visible = startupOptionsDoubleBufferPanel.Visible =
                    bandwithControlLabel.Visible = bandwidthControlDoubleBufferPanel.Visible =
                    videoWindowTitleBarLabel.Visible = videoWindowtitleBarDoubleBufferPanel.Visible =
                    cpuLoadingLabel.Visible = cpuLoadingDoubleBufferPanel.Visible =
                    watermarklabel2.Visible = watermarkdoubleBufferPanel2.Visible =
                    false;
            }
            else if (Server is IPTS)
            {
                recordDurationDoubleBufferPanel.Visible = recordDurationLabel.Visible =
                ftpServerDoubleBufferPanel.Visible = ftpServerLabel.Visible =
                storageAlertDoubleBufferPanel.Visible = storageAlertLabel.Visible =
                bandwithControlLabel.Visible = bandwidthControlDoubleBufferPanel.Visible =
                videoWindowTitleBarLabel.Visible = videoWindowtitleBarDoubleBufferPanel.Visible =
                watermarklabel2.Visible = watermarkdoubleBufferPanel2.Visible =
                cpuLoadingLabel.Visible = cpuLoadingDoubleBufferPanel.Visible =
                displayGroupIdLabel.Visible = displayGroupIdDoubleBufferPanel.Visible = displayGroupIdCheckBox.Visible =
                autoSwitchLiveVideoStreamLabel.Visible = autoSwitchLiveVideoStreamDoubleBufferPanel.Visible =
                startOptionsLabel.Visible = startupOptionsDoubleBufferPanel.Visible =
                autoLockApplicationTimerDoubleBufferLabel.Visible = autoLockApplicationTimerDoubleBufferPanel.Visible = false;

                switch (Server.Configure.TransactionTimeOption)
                {
                    case "Transaction":
                        transactionComboBox.SelectedIndex = 1;
                        break;

                    default:
                        transactionComboBox.SelectedIndex = 0;
                        break;
                }
                transactionDoubleBufferPanel.Visible = transactionLabel.Visible = true;
                transactionComboBox.SelectedIndexChanged += TransactionComboBoxSelectedIndexChanged;

                var hideUnSupportButtons = ((IPTS)Server).ReleaseBrand == "Salient";

                if (hideUnSupportButtons)
                {
                    displayGroupIdDoubleBufferPanel.Visible = displayGroupIdLabel.Visible =
                    autoSwitchLiveVideoStreamDoubleBufferPanel.Visible = autoSwitchLiveVideoStreamLabel.Visible =
                    autoSwitchDecodeIFrameDoubleBufferPanel.Visible = autoSwitchDecodeIFrameLabel.Visible =
                    videoWindowtitleBarDoubleBufferPanel.Visible = videoWindowTitleBarLabel.Visible =
                    cpuLoadingDoubleBufferPanel.Visible = cpuLoadingLabel.Visible =
                    startupOptionsDoubleBufferPanel.Visible = startOptionsLabel.Visible =
                    patrolDoubleBufferPanel.Visible = patrolLabel.Visible =
                    recordDurationDoubleBufferPanel.Visible = recordDurationLabel.Visible =
                    watermarkDoubleBufferPanel.Visible = watermarkLabel.Visible = false;
                }
            }
        }

        public void WatermarkShow()
        {
            watermarklabel2.Visible = watermarkdoubleBufferPanel2.Visible = true;
        }

        private void TransactionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (transactionComboBox.SelectedIndex)
            {
                case 1:
                    Server.Configure.TransactionTimeOption = "Transaction";
                    break;

                default:
                    Server.Configure.TransactionTimeOption = "System";
                    break;
            }
        }

        public void RefreshContent()
        {
            withTimestampCheckBox.Checked = Server.Configure.ImageWithTimestamp;
            displayDeviceIdCheckBox.Checked = Server.Configure.DisplayDeviceId;
            displayGroupIdCheckBox.Checked = Server.Configure.DisplayGroupId;
            DisplayNVRIdCheckBox.Checked = Server.Configure.DisplayNVRId;
            beepCheckBox.Checked = Server.Configure.StorageAlert;
            liveStretchCheckBox.Checked = Server.Configure.StretchLiveVideo;
            playbackStretchCheckBox.Checked = Server.Configure.StretchPlaybackVideo;
            keepLastFrameCheckBox.Checked = Server.Configure.KeepLastFrame;

            patrolDoubleBufferPanel.Invalidate();
            displayDeviceIdDoubleBufferPanel.Invalidate();
            displayGroupIdDoubleBufferPanel.Invalidate();
            recordDurationDoubleBufferPanel.Invalidate();
            watermarkDoubleBufferPanel.Invalidate();
            mailServerDoubleBufferPanel.Invalidate();
            ftpServerDoubleBufferPanel.Invalidate();
            saveImagePathDoubleBufferPanel.Invalidate();
            exportVideoPathDoubleBufferPanel.Invalidate();
            storageAlertDoubleBufferPanel.Invalidate();
            stretchVideoDoubleBufferPanel.Invalidate();
            videoWindowtitleBarDoubleBufferPanel.Invalidate();
            cpuLoadingDoubleBufferPanel.Invalidate();

            autoSwitchLiveVideoStreamDoubleBufferPanel.Invalidate();
            autoSwitchDecodeIFrameDoubleBufferPanel.Invalidate();

            startupOptionsDoubleBufferPanel.Invalidate();
            bandwidthControlDoubleBufferPanel.Invalidate();
            autoLockApplicationTimerDoubleBufferPanel.Invalidate();
        }

        private void DisplayDeviceIdCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.DisplayDeviceId = displayDeviceIdCheckBox.Checked;
            BasicDevice.DisplayDeviceId = Server.Configure.DisplayDeviceId;
        }

        private void DisplayGroupIdCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.DisplayGroupId = displayGroupIdCheckBox.Checked;
            DeviceGroup.DisplayGroupId = Server.Configure.DisplayGroupId;
        }

        private void DisplayNVRIdCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.DisplayNVRId = DisplayNVRIdCheckBox.Checked;
            NVR.DisplayNVRId = Server.Configure.DisplayNVRId;
        }

        private void LiveStretchCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.StretchLiveVideo = liveStretchCheckBox.Checked;
        }

        private void PlaybackStretchCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.StretchPlaybackVideo = playbackStretchCheckBox.Checked;
        }

        private void KeepLastFrameCheckBoxClick(object sender, EventArgs e)
        {
            Server.Configure.KeepLastFrame = keepLastFrameCheckBox.Checked;
        }

        private void BeepCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.StorageAlert = beepCheckBox.Checked;
        }

        private void WithTimestampCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Server.Configure.ImageWithTimestamp = withTimestampCheckBox.Checked;
        }

        protected void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            if (Server is IPTS)
            {
                Manager.PaintHighLightInput(g, control);
            }
            else
            {
                Manager.PaintSingleInput(g, control);
            }

            if (Localization.ContainsKey("SetupGeneral_" + control.Tag))
                Manager.PaintText(g, Localization["SetupGeneral_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private String SecToStr(Int32 sec)
        {
            return sec + Localization["Common_Sec"];
        }

        private String MinToStr(Int32 min)
        {
            String str = "";
            if (min >= 60)
                str = (min / 60) + Localization["Common_Min"];

            if (min % 60 != 0)
                str += " " + SecToStr(min % 60);

            return str;
        }

        private String HrToStr(Int32 hr)
        {
            String str = (hr / 3600) + Localization["Common_Hr"];
            if (hr % 3600 != 0)
                str += " " + MinToStr(hr % 3600);

            return str;
        }

        private String _intervalStr;
        private UInt16 _interval;
        private void PatrolPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            if (_interval != Server.Configure.PatrolInterval)
            {
                _interval = Server.Configure.PatrolInterval;
                if (_interval < 60)
                    _intervalStr = SecToStr(_interval);
                else if (_interval < 3600)
                    _intervalStr = MinToStr(_interval);
                else
                    _intervalStr = HrToStr(_interval);
            }

            Manager.PaintTextRight(g, patrolDoubleBufferPanel, _intervalStr);
            Manager.PaintEdit(g, patrolDoubleBufferPanel);
        }

        private String _durationStr;
        private UInt16 _duration;
        private void RecordDurationPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            if (_duration != Server.Configure.ManualRecordDuration)
            {
                _duration = Server.Configure.ManualRecordDuration;
                if (_duration < 60)
                    _durationStr = SecToStr(_duration);
                else if (_duration < 3600)
                    _durationStr = MinToStr(_duration);
                else
                    _durationStr = HrToStr(_duration);
            }

            Manager.PaintTextRight(g, recordDurationDoubleBufferPanel, _durationStr);
            Manager.PaintEdit(g, recordDurationDoubleBufferPanel);
        }

        private void MailServerPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            Manager.PaintTextRight(g, mailServerDoubleBufferPanel, Server.Configure.MailServer.Credential.Domain);
            Manager.PaintEdit(g, mailServerDoubleBufferPanel);
        }

        private void FtpServerPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            Manager.PaintTextRight(g, ftpServerDoubleBufferPanel, Server.Configure.FtpServer.Credential.Domain);
            Manager.PaintEdit(g, ftpServerDoubleBufferPanel);
        }

        private void VideoWindowTitleBarPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            //Manager.PaintTextRight(g, videoWindowtitleBarDoubleBufferPanel, String.Empty);
            Manager.PaintEdit(g, videoWindowtitleBarDoubleBufferPanel);
        }
        private void WatermarkPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            //Manager.PaintTextRight(g, videoWindowtitleBarDoubleBufferPanel, String.Empty);
            Manager.PaintEdit(g, watermarkdoubleBufferPanel2);
        }

        private void CpuLoadingDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            Manager.PaintTextRight(g, cpuLoadingDoubleBufferPanel, String.Format("{0}%", Server.Configure.CPULoadingUpperBoundary));
            Manager.PaintEdit(g, cpuLoadingDoubleBufferPanel);
        }

        private void AutoSwitchLiveVideoStreamDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            var text = (Server.Configure.EnableAutoSwitchLiveStream)
                           ? Localization["SetupGeneral_Enabled"]
                           : Localization["SetupGeneral_Disabled"];

            Manager.PaintTextRight(g, autoSwitchLiveVideoStreamDoubleBufferPanel, text);
            Manager.PaintEdit(g, autoSwitchLiveVideoStreamDoubleBufferPanel);
        }

        private void AutoSwitchDecodeIFrameDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            var text = (Server.Configure.EnableAutoSwitchDecodeIFrame)
                           ? Localization["SetupGeneral_Enabled"]
                           : Localization["SetupGeneral_Disabled"];

            Manager.PaintTextRight(g, autoSwitchDecodeIFrameDoubleBufferPanel, text);
            Manager.PaintEdit(g, autoSwitchDecodeIFrameDoubleBufferPanel);
        }

        private void SaveImagePathPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            switch (Server.Configure.SaveImagePath)
            {
                case "Desktop":
                    Manager.PaintTextRight(g, saveImagePathDoubleBufferPanel, Localization["SetupGeneral_Desktop"]);
                    break;

                case "Document":
                    Manager.PaintTextRight(g, saveImagePathDoubleBufferPanel, Localization["SetupGeneral_Document"]);
                    break;

                case "Picture":
                    Manager.PaintTextRight(g, saveImagePathDoubleBufferPanel, Localization["SetupGeneral_Picture"]);
                    break;

                default:
                    Manager.PaintTextRight(g, saveImagePathDoubleBufferPanel, Server.Configure.SaveImagePath);
                    break;
            }

            Manager.PaintEdit(g, saveImagePathDoubleBufferPanel);
        }

        private void ExportVideoPathPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            switch (Server.Configure.ExportVideoPath)
            {
                case "Desktop":
                    Manager.PaintTextRight(g, exportVideoPathDoubleBufferPanel, Localization["SetupGeneral_Desktop"]);
                    break;

                case "Document":
                    Manager.PaintTextRight(g, exportVideoPathDoubleBufferPanel, Localization["SetupGeneral_Document"]);
                    break;

                case "Picture":
                    Manager.PaintTextRight(g, exportVideoPathDoubleBufferPanel, Localization["SetupGeneral_Picture"]);
                    break;

                default:
                    Manager.PaintTextRight(g, exportVideoPathDoubleBufferPanel, Server.Configure.ExportVideoPath);
                    break;
            }

            Manager.PaintEdit(g, exportVideoPathDoubleBufferPanel);
        }

        private void StartupOptionsDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            var text = (Server.Configure.StartupOptions.Enabled)
                           ? Localization["SetupGeneral_Enabled"]
                           : Localization["SetupGeneral_Disabled"];

            Manager.PaintTextRight(g, startupOptionsDoubleBufferPanel, text);
            Manager.PaintEdit(g, startupOptionsDoubleBufferPanel);
        }


        private void BandwidthControlDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            var text = (Server.Configure.EnableBandwidthControl)
                           ? Localization["SetupGeneral_Enabled"]
                           : Localization["SetupGeneral_Disabled"];

            Manager.PaintTextRight(g, bandwidthControlDoubleBufferPanel, text);
            Manager.PaintEdit(g, bandwidthControlDoubleBufferPanel);
        }

        private void AutoLockApplicationTimerDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            var lockTimer = Server.Configure.AutoLockApplicationTimer;
            var lockTimerStr = "";

            if (lockTimer == 0)
            {
                lockTimerStr = Localization["SetupGeneral_Disabled"];
            }
            else
            {
                if (lockTimer < 60)
                    lockTimerStr = SecToStr(lockTimer);
                else if (_duration < 3600)
                    lockTimerStr = MinToStr(lockTimer);
                else
                    lockTimerStr = HrToStr(lockTimer);
            }

            Manager.PaintTextRight(g, autoLockApplicationTimerDoubleBufferPanel, lockTimerStr);
            Manager.PaintEdit(g, autoLockApplicationTimerDoubleBufferPanel);
        }

        private void PatrolPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPatrolEdit != null)
                OnPatrolEdit(this, null);
        }

        private void RecordDurationPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDurationEdit != null)
                OnDurationEdit(this, null);
        }

        private void MailServerPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnMailServerEdit != null)
                OnMailServerEdit(this, null);
        }

        private void FtpServerPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnFtpServerEdit != null)
                OnFtpServerEdit(this, null);
        }

        private void SaveImagePathPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSaveImagePathEdit != null)
                OnSaveImagePathEdit(this, null);
        }

        private void ExportVideoPathPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnExportVideoPathEdit != null)
                OnExportVideoPathEdit(this, null);
        }

        private void VideoWindowTitleBarDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnVideoWindowTitleBarEdit != null)
                OnVideoWindowTitleBarEdit(this, null);
        }
        private void WatermarkDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnWatermarkEdit != null)
                OnWatermarkEdit(this, null);
        }

        private void CPULoadingDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnCPULoadingEdit != null)
                OnCPULoadingEdit(this, null);
        }

        private void AutoSwitchLiveVideoStreamDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnAutoSwitchLiveVideoStreamEdit != null)
                OnAutoSwitchLiveVideoStreamEdit(this, null);
        }

        private void AutoSwitchDecodeIFrameDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnAutoSwitchDecodeIFrameEdit != null)
                OnAutoSwitchDecodeIFrameEdit(this, null);
        }

        private void StartupOptionsDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnStartupOptionsEdit != null)
                OnStartupOptionsEdit(this, null);
        }

        private void BandwidthControlDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnBandwidthControlEdit != null)
                OnBandwidthControlEdit(this, null);
        }

        private void AutoLockApplicationTimerDoubleBufferPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (OnAutoLockApplicationEdit != null)
                OnAutoLockApplicationEdit(this, null);
        }

        internal void SISSetupView()
        {
            patrolLabel.Visible = false;
            patrolDoubleBufferPanel.Visible = false;

            bandwithControlLabel.Visible = false;
            bandwidthControlDoubleBufferPanel.Visible = false;

            autoSwitchLiveVideoStreamLabel.Visible = false;
            autoSwitchLiveVideoStreamDoubleBufferPanel.Visible = false;

            autoSwitchDecodeIFrameLabel.Visible = false;
            autoSwitchDecodeIFrameDoubleBufferPanel.Visible = false;
        }

        internal void MobileNvrView()
        {
            patrolLabel.Visible = false;
            patrolDoubleBufferPanel.Visible = false;
        }

        protected void CommandCenterView()
        {
            this.SuspendLayout();

            SISSetupView();

            displayGroupIdLabel.Visible = false;
            displayGroupIdDoubleBufferPanel.Visible = false;

            videoWindowTitleBarLabel.Visible = false;
            videoWindowtitleBarDoubleBufferPanel.Visible = false;

            autoLockApplicationTimerDoubleBufferLabel.Visible = false;
            autoLockApplicationTimerDoubleBufferPanel.Visible = false;

            this.ResumeLayout();
        }
    }
}
