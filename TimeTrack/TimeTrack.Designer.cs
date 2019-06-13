using System;
using System.Windows.Forms;

namespace TimeTrack
{
    partial class TimeTrack
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        protected void InitializeComponent()
        {
            this.trackPanel = new System.Windows.Forms.Panel();
            this.pointerLabel = new System.Windows.Forms.Label();
            this.rangeRightPanel = new System.Windows.Forms.Panel();
            this.rangeLeftPanel = new System.Windows.Forms.Panel();
            this.loadingLabel = new PanelBase.DoubleBufferLabel();
            this.devicePanel = new PanelBase.DoubleBufferPanel();
            this.switchPanel = new PanelBase.DoubleBufferPanel();
            this.upButton = new System.Windows.Forms.Panel();
            this.downButton = new System.Windows.Forms.Panel();
            this.toolPanel = new PanelBase.DoubleBufferPanel();
            this.eventPanelButton = new System.Windows.Forms.Panel();
            this.eventPanel = new PanelBase.DoubleBufferPanel();
            this.temperingDetectionButton = new EventButton();
            this.audioDetectionButton = new EventButton();
            this.objectCountingOutButton = new EventButton();
            this.objectCountingInButton = new EventButton();
            this.loiteringDetectionButton = new EventButton();
            this.instrusionDetectionButton = new EventButton();
            this.crossLineButton = new EventButton();
            this.networkLossButton = new EventButton();
            this.networkRecoveryButton = new EventButton();
            this.doButton = new EventButton();
            this.audioInButton = new EventButton();
            this.audioOutButton = new EventButton();
            this.videoLossButton = new EventButton();
            this.videoRecoveryButton = new EventButton();
            this.panicButton = new EventButton();
            this.motionButton = new EventButton();
            this.manualRecordButton = new EventButton();
            this.diButton = new EventButton();
            this.userdefineButton = new EventButton();
            this.scalePanel = new PanelBase.DoubleBufferPanel();
            this.plusButton = new System.Windows.Forms.Panel();
            this.scaleButton = new System.Windows.Forms.Panel();
            this.minusButton = new System.Windows.Forms.Panel();
            this.gotoCurrentButton = new System.Windows.Forms.Panel();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.minimizePanel = new System.Windows.Forms.Panel();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.gotoBeginButton = new System.Windows.Forms.Panel();
            this.controllerPanel = new PanelBase.DoubleBufferPanel();
            this.enablePlaybackSmoothCheckBox = new System.Windows.Forms.CheckBox();
            this.withArchiveServerCheckBox = new System.Windows.Forms.CheckBox();
            this.clearSelectionButton = new System.Windows.Forms.Panel();
            this.setStartButton = new System.Windows.Forms.Panel();
            this.setEndButton = new System.Windows.Forms.Panel();
            this.addBookmarkButton = new System.Windows.Forms.Panel();
            this.eraserBookmarkButton = new System.Windows.Forms.Panel();
            this.previousBookmarkButton = new System.Windows.Forms.Panel();
            this.nextBookmarkButton = new System.Windows.Forms.Panel();
            this.previousRecordButton = new System.Windows.Forms.Panel();
            this.nextRecordButton = new System.Windows.Forms.Panel();
            this.trackPanel.SuspendLayout();
            this.switchPanel.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.eventPanel.SuspendLayout();
            this.scalePanel.SuspendLayout();
            this.controllerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackPanel
            // 
            this.trackPanel.BackColor = System.Drawing.Color.Gray;
            this.trackPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.trackPanel.Controls.Add(this.pointerLabel);
            this.trackPanel.Controls.Add(this.rangeRightPanel);
            this.trackPanel.Controls.Add(this.rangeLeftPanel);
            this.trackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackPanel.Location = new System.Drawing.Point(0, 74);
            this.trackPanel.Name = "trackPanel";
            this.trackPanel.Size = new System.Drawing.Size(1010, 130);
            this.trackPanel.TabIndex = 1;
            // 
            // pointerLabel
            // 
            this.pointerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pointerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(57)))), ((int)(((byte)(28)))));
            this.pointerLabel.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
            this.pointerLabel.Location = new System.Drawing.Point(505, 46);
            this.pointerLabel.Name = "pointerLabel";
            this.pointerLabel.Size = new System.Drawing.Size(1, 83);
            this.pointerLabel.TabIndex = 0;
            // 
            // rangeRightPanel
            // 
            this.rangeRightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rangeRightPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(110)))), ((int)(((byte)(116)))));
            this.rangeRightPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rangeRightPanel.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.rangeRightPanel.Location = new System.Drawing.Point(1005, 46);
            this.rangeRightPanel.Name = "rangeRightPanel";
            this.rangeRightPanel.Size = new System.Drawing.Size(5, 83);
            this.rangeRightPanel.TabIndex = 1;
            this.rangeRightPanel.Visible = false;
            // 
            // rangeLeftPanel
            // 
            this.rangeLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rangeLeftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(110)))), ((int)(((byte)(116)))));
            this.rangeLeftPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rangeLeftPanel.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.rangeLeftPanel.Location = new System.Drawing.Point(0, 46);
            this.rangeLeftPanel.Name = "rangeLeftPanel";
            this.rangeLeftPanel.Size = new System.Drawing.Size(5, 83);
            this.rangeLeftPanel.TabIndex = 2;
            this.rangeLeftPanel.Visible = false;
            // 
            // loadingLabel
            // 
            this.loadingLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.BackColor = System.Drawing.Color.Transparent;
            this.loadingLabel.Font = new System.Drawing.Font("Arial", 12F);
            this.loadingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(133)))), ((int)(((byte)(185)))));
            this.loadingLabel.Location = new System.Drawing.Point(474, 120);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(65, 18);
            this.loadingLabel.TabIndex = 3;
            this.loadingLabel.Text = "Loading";
            this.loadingLabel.Visible = false;
            // 
            // devicePanel
            // 
            this.devicePanel.AutoSize = true;
            this.devicePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.devicePanel.Location = new System.Drawing.Point(0, 74);
            this.devicePanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.devicePanel.Name = "devicePanel";
            this.devicePanel.Size = new System.Drawing.Size(0, 130);
            this.devicePanel.TabIndex = 0;
            // 
            // switchPanel
            // 
            this.switchPanel.BackgroundImage = global::TimeTrack.Properties.Resources.deviceContainerBG;
            this.switchPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchPanel.Controls.Add(this.upButton);
            this.switchPanel.Controls.Add(this.downButton);
            this.switchPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.switchPanel.Location = new System.Drawing.Point(1010, 74);
            this.switchPanel.Margin = new System.Windows.Forms.Padding(0);
            this.switchPanel.Name = "switchPanel";
            this.switchPanel.Padding = new System.Windows.Forms.Padding(0, 46, 0, 0);
            this.switchPanel.Size = new System.Drawing.Size(26, 130);
            this.switchPanel.TabIndex = 2;
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.BackColor = System.Drawing.Color.Transparent;
            this.upButton.BackgroundImage = global::TimeTrack.Properties.Resources.up;
            this.upButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.upButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.upButton.Location = new System.Drawing.Point(3, 45);
            this.upButton.Margin = new System.Windows.Forms.Padding(0);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(20, 43);
            this.upButton.TabIndex = 0;
            this.upButton.Visible = false;
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.BackColor = System.Drawing.Color.Transparent;
            this.downButton.BackgroundImage = global::TimeTrack.Properties.Resources.down;
            this.downButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downButton.Location = new System.Drawing.Point(3, 88);
            this.downButton.Margin = new System.Windows.Forms.Padding(0);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(20, 42);
            this.downButton.TabIndex = 1;
            this.downButton.Visible = false;
            // 
            // toolPanel
            // 
            this.toolPanel.BackColor = System.Drawing.Color.Transparent;
            this.toolPanel.BackgroundImage = global::TimeTrack.Properties.Resources.banner;
            this.toolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolPanel.Controls.Add(this.eventPanelButton);
            this.toolPanel.Controls.Add(this.eventPanel);
            this.toolPanel.Controls.Add(this.scalePanel);
            this.toolPanel.Controls.Add(this.gotoCurrentButton);
            this.toolPanel.Controls.Add(this.scaleLabel);
            this.toolPanel.Controls.Add(this.minimizePanel);
            this.toolPanel.Controls.Add(this.datePicker);
            this.toolPanel.Controls.Add(this.timePicker);
            this.toolPanel.Controls.Add(this.gotoBeginButton);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 37);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(1036, 37);
            this.toolPanel.TabIndex = 3;
            // 
            // eventPanelButton
            // 
            this.eventPanelButton.BackgroundImage = global::TimeTrack.Properties.Resources.expand;
            this.eventPanelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.eventPanelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.eventPanelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.eventPanelButton.Location = new System.Drawing.Point(150, 0);
            this.eventPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.eventPanelButton.Name = "eventPanelButton";
            this.eventPanelButton.Size = new System.Drawing.Size(38, 37);
            this.eventPanelButton.TabIndex = 0;
            this.eventPanelButton.Visible = false;
            // 
            // eventPanel
            // 
            this.eventPanel.Controls.Add(this.userdefineButton);
            this.eventPanel.Controls.Add(this.temperingDetectionButton);
            this.eventPanel.Controls.Add(this.audioDetectionButton);
            this.eventPanel.Controls.Add(this.objectCountingOutButton);
            this.eventPanel.Controls.Add(this.objectCountingInButton);
            this.eventPanel.Controls.Add(this.loiteringDetectionButton);
            this.eventPanel.Controls.Add(this.instrusionDetectionButton);
            this.eventPanel.Controls.Add(this.crossLineButton);
            this.eventPanel.Controls.Add(this.networkLossButton);
            this.eventPanel.Controls.Add(this.networkRecoveryButton);
            this.eventPanel.Controls.Add(this.doButton);
            this.eventPanel.Controls.Add(this.audioInButton);
            this.eventPanel.Controls.Add(this.audioOutButton);
            this.eventPanel.Controls.Add(this.videoLossButton);
            this.eventPanel.Controls.Add(this.videoRecoveryButton);
            this.eventPanel.Controls.Add(this.panicButton);
            this.eventPanel.Controls.Add(this.motionButton);
            this.eventPanel.Controls.Add(this.manualRecordButton);
            this.eventPanel.Controls.Add(this.diButton);
            this.eventPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.eventPanel.Location = new System.Drawing.Point(16, 0);
            this.eventPanel.Margin = new System.Windows.Forms.Padding(0);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.eventPanel.Size = new System.Drawing.Size(134, 37);
            this.eventPanel.TabIndex = 1;
            this.eventPanel.Visible = false;
            // 
            // temperingDetectionButton
            // 
            this.temperingDetectionButton.ActiveImage = null;
            this.temperingDetectionButton.BackColor = System.Drawing.Color.Transparent;
            this.temperingDetectionButton.BackgroundImage = global::TimeTrack.Properties.Resources.objectCountingOut;
            this.temperingDetectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.temperingDetectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.temperingDetectionButton.EventType = Constant.EventType.TamperDetection;
            this.temperingDetectionButton.Image = null;
            this.temperingDetectionButton.Location = new System.Drawing.Point(550, 0);
            this.temperingDetectionButton.Name = "temperingDetectionButton";
            this.temperingDetectionButton.Size = new System.Drawing.Size(32, 37);
            this.temperingDetectionButton.TabIndex = 17;
            // 
            // audioDetectionButton
            // 
            this.audioDetectionButton.ActiveImage = null;
            this.audioDetectionButton.BackColor = System.Drawing.Color.Transparent;
            this.audioDetectionButton.BackgroundImage = global::TimeTrack.Properties.Resources.objectCountingOut;
            this.audioDetectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.audioDetectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.audioDetectionButton.EventType = Constant.EventType.AudioDetection;
            this.audioDetectionButton.Image = null;
            this.audioDetectionButton.Location = new System.Drawing.Point(518, 0);
            this.audioDetectionButton.Name = "audioDetectionButton";
            this.audioDetectionButton.Size = new System.Drawing.Size(32, 37);
            this.audioDetectionButton.TabIndex = 16;
            // 
            // objectCountingOutButton
            // 
            this.objectCountingOutButton.ActiveImage = null;
            this.objectCountingOutButton.BackColor = System.Drawing.Color.Transparent;
            this.objectCountingOutButton.BackgroundImage = global::TimeTrack.Properties.Resources.objectCountingOut;
            this.objectCountingOutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.objectCountingOutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.objectCountingOutButton.EventType = Constant.EventType.ObjectCountingOut;
            this.objectCountingOutButton.Image = null;
            this.objectCountingOutButton.Location = new System.Drawing.Point(486, 0);
            this.objectCountingOutButton.Name = "objectCountingOutButton";
            this.objectCountingOutButton.Size = new System.Drawing.Size(32, 37);
            this.objectCountingOutButton.TabIndex = 15;
            // 
            // objectCountingInButton
            // 
            this.objectCountingInButton.ActiveImage = null;
            this.objectCountingInButton.BackColor = System.Drawing.Color.Transparent;
            this.objectCountingInButton.BackgroundImage = global::TimeTrack.Properties.Resources.objectCountingIn;
            this.objectCountingInButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.objectCountingInButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.objectCountingInButton.EventType = Constant.EventType.ObjectCountingIn;
            this.objectCountingInButton.Image = null;
            this.objectCountingInButton.Location = new System.Drawing.Point(454, 0);
            this.objectCountingInButton.Name = "objectCountingInButton";
            this.objectCountingInButton.Size = new System.Drawing.Size(32, 37);
            this.objectCountingInButton.TabIndex = 14;
            // 
            // loiteringDetectionButton
            // 
            this.loiteringDetectionButton.ActiveImage = null;
            this.loiteringDetectionButton.BackColor = System.Drawing.Color.Transparent;
            this.loiteringDetectionButton.BackgroundImage = global::TimeTrack.Properties.Resources.loiteringDetection;
            this.loiteringDetectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.loiteringDetectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loiteringDetectionButton.EventType = Constant.EventType.LoiteringDetection;
            this.loiteringDetectionButton.Image = null;
            this.loiteringDetectionButton.Location = new System.Drawing.Point(422, 0);
            this.loiteringDetectionButton.Name = "loiteringDetectionButton";
            this.loiteringDetectionButton.Size = new System.Drawing.Size(32, 37);
            this.loiteringDetectionButton.TabIndex = 13;
            // 
            // instrusionDetectionButton
            // 
            this.instrusionDetectionButton.ActiveImage = null;
            this.instrusionDetectionButton.BackColor = System.Drawing.Color.Transparent;
            this.instrusionDetectionButton.BackgroundImage = global::TimeTrack.Properties.Resources.intrusionDetection;
            this.instrusionDetectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.instrusionDetectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.instrusionDetectionButton.EventType = Constant.EventType.IntrusionDetection;
            this.instrusionDetectionButton.Image = null;
            this.instrusionDetectionButton.Location = new System.Drawing.Point(390, 0);
            this.instrusionDetectionButton.Name = "instrusionDetectionButton";
            this.instrusionDetectionButton.Size = new System.Drawing.Size(32, 37);
            this.instrusionDetectionButton.TabIndex = 12;
            // 
            // crossLineButton
            // 
            this.crossLineButton.ActiveImage = null;
            this.crossLineButton.BackColor = System.Drawing.Color.Transparent;
            this.crossLineButton.BackgroundImage = global::TimeTrack.Properties.Resources.crossline;
            this.crossLineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.crossLineButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.crossLineButton.EventType = Constant.EventType.Motion;
            this.crossLineButton.Image = null;
            this.crossLineButton.Location = new System.Drawing.Point(358, 0);
            this.crossLineButton.Name = "crossLineButton";
            this.crossLineButton.Size = new System.Drawing.Size(32, 37);
            this.crossLineButton.TabIndex = 11;
            // 
            // networkLossButton
            // 
            this.networkLossButton.ActiveImage = null;
            this.networkLossButton.BackColor = System.Drawing.Color.Transparent;
            this.networkLossButton.BackgroundImage = global::TimeTrack.Properties.Resources.networkloss;
            this.networkLossButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.networkLossButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.networkLossButton.EventType = Constant.EventType.Motion;
            this.networkLossButton.Image = null;
            this.networkLossButton.Location = new System.Drawing.Point(326, 0);
            this.networkLossButton.Name = "networkLossButton";
            this.networkLossButton.Size = new System.Drawing.Size(32, 37);
            this.networkLossButton.TabIndex = 0;
            // 
            // networkRecoveryButton
            // 
            this.networkRecoveryButton.ActiveImage = null;
            this.networkRecoveryButton.BackColor = System.Drawing.Color.Transparent;
            this.networkRecoveryButton.BackgroundImage = global::TimeTrack.Properties.Resources.networkrecovery;
            this.networkRecoveryButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.networkRecoveryButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.networkRecoveryButton.EventType = Constant.EventType.Motion;
            this.networkRecoveryButton.Image = null;
            this.networkRecoveryButton.Location = new System.Drawing.Point(294, 0);
            this.networkRecoveryButton.Name = "networkRecoveryButton";
            this.networkRecoveryButton.Size = new System.Drawing.Size(32, 37);
            this.networkRecoveryButton.TabIndex = 1;
            // 
            // doButton
            // 
            this.doButton.ActiveImage = null;
            this.doButton.BackColor = System.Drawing.Color.Transparent;
            this.doButton.BackgroundImage = global::TimeTrack.Properties.Resources._do;
            this.doButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.doButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doButton.EventType = Constant.EventType.Motion;
            this.doButton.Image = null;
            this.doButton.Location = new System.Drawing.Point(262, 0);
            this.doButton.Name = "doButton";
            this.doButton.Size = new System.Drawing.Size(32, 37);
            this.doButton.TabIndex = 2;
            // 
            // audioInButton
            // 
            this.audioInButton.ActiveImage = null;
            this.audioInButton.BackColor = System.Drawing.Color.Transparent;
            this.audioInButton.BackgroundImage = global::TimeTrack.Properties.Resources.audioin;
            this.audioInButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.audioInButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.audioInButton.EventType = Constant.EventType.Motion;
            this.audioInButton.Image = null;
            this.audioInButton.Location = new System.Drawing.Point(230, 0);
            this.audioInButton.Name = "audioInButton";
            this.audioInButton.Size = new System.Drawing.Size(32, 37);
            this.audioInButton.TabIndex = 3;
            // 
            // audioOutButton
            // 
            this.audioOutButton.ActiveImage = null;
            this.audioOutButton.BackColor = System.Drawing.Color.Transparent;
            this.audioOutButton.BackgroundImage = global::TimeTrack.Properties.Resources.audioout;
            this.audioOutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.audioOutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.audioOutButton.EventType = Constant.EventType.Motion;
            this.audioOutButton.Image = null;
            this.audioOutButton.Location = new System.Drawing.Point(198, 0);
            this.audioOutButton.Name = "audioOutButton";
            this.audioOutButton.Size = new System.Drawing.Size(32, 37);
            this.audioOutButton.TabIndex = 4;
            // 
            // videoLossButton
            // 
            this.videoLossButton.ActiveImage = null;
            this.videoLossButton.BackColor = System.Drawing.Color.Transparent;
            this.videoLossButton.BackgroundImage = global::TimeTrack.Properties.Resources.videoloss;
            this.videoLossButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.videoLossButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.videoLossButton.EventType = Constant.EventType.Motion;
            this.videoLossButton.Image = null;
            this.videoLossButton.Location = new System.Drawing.Point(166, 0);
            this.videoLossButton.Name = "videoLossButton";
            this.videoLossButton.Size = new System.Drawing.Size(32, 37);
            this.videoLossButton.TabIndex = 5;
            // 
            // videoRecoveryButton
            // 
            this.videoRecoveryButton.ActiveImage = null;
            this.videoRecoveryButton.BackColor = System.Drawing.Color.Transparent;
            this.videoRecoveryButton.BackgroundImage = global::TimeTrack.Properties.Resources.videorecovery;
            this.videoRecoveryButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.videoRecoveryButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.videoRecoveryButton.EventType = Constant.EventType.Motion;
            this.videoRecoveryButton.Image = null;
            this.videoRecoveryButton.Location = new System.Drawing.Point(134, 0);
            this.videoRecoveryButton.Name = "videoRecoveryButton";
            this.videoRecoveryButton.Size = new System.Drawing.Size(32, 37);
            this.videoRecoveryButton.TabIndex = 6;
            // 
            // panicButton
            // 
            this.panicButton.ActiveImage = null;
            this.panicButton.BackColor = System.Drawing.Color.Transparent;
            this.panicButton.BackgroundImage = global::TimeTrack.Properties.Resources.panic;
            this.panicButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panicButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panicButton.EventType = Constant.EventType.Motion;
            this.panicButton.Image = null;
            this.panicButton.Location = new System.Drawing.Point(102, 0);
            this.panicButton.Name = "panicButton";
            this.panicButton.Size = new System.Drawing.Size(32, 37);
            this.panicButton.TabIndex = 7;
            // 
            // motionButton
            // 
            this.motionButton.ActiveImage = null;
            this.motionButton.BackColor = System.Drawing.Color.Transparent;
            this.motionButton.BackgroundImage = global::TimeTrack.Properties.Resources.motion;
            this.motionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.motionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.motionButton.EventType = Constant.EventType.Motion;
            this.motionButton.Image = null;
            this.motionButton.Location = new System.Drawing.Point(70, 0);
            this.motionButton.Margin = new System.Windows.Forms.Padding(0);
            this.motionButton.Name = "motionButton";
            this.motionButton.Size = new System.Drawing.Size(32, 37);
            this.motionButton.TabIndex = 8;
            // 
            // manualRecordButton
            // 
            this.manualRecordButton.ActiveImage = null;
            this.manualRecordButton.BackColor = System.Drawing.Color.Transparent;
            this.manualRecordButton.BackgroundImage = global::TimeTrack.Properties.Resources.manualrecord;
            this.manualRecordButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.manualRecordButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.manualRecordButton.EventType = Constant.EventType.Motion;
            this.manualRecordButton.Image = null;
            this.manualRecordButton.Location = new System.Drawing.Point(38, 0);
            this.manualRecordButton.Name = "manualRecordButton";
            this.manualRecordButton.Size = new System.Drawing.Size(32, 37);
            this.manualRecordButton.TabIndex = 9;
            // 
            // diButton
            // 
            this.diButton.ActiveImage = null;
            this.diButton.BackColor = System.Drawing.Color.Transparent;
            this.diButton.BackgroundImage = global::TimeTrack.Properties.Resources.di;
            this.diButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.diButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.diButton.EventType = Constant.EventType.Motion;
            this.diButton.Image = null;
            this.diButton.Location = new System.Drawing.Point(6, 0);
            this.diButton.Name = "diButton";
            this.diButton.Size = new System.Drawing.Size(32, 37);
            this.diButton.TabIndex = 10;
            // 
            // userdefineButton
            // 
            this.userdefineButton.ActiveImage = null;
            this.userdefineButton.BackColor = System.Drawing.Color.Transparent;
            this.userdefineButton.BackgroundImage = global::TimeTrack.Properties.Resources.di;
            this.userdefineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.userdefineButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.userdefineButton.EventType = Constant.EventType.Motion;
            this.userdefineButton.Image = null;
            this.userdefineButton.Location = new System.Drawing.Point(582, 0);
            this.userdefineButton.Name = "userdefineButton";
            this.userdefineButton.Size = new System.Drawing.Size(32, 37);
            this.userdefineButton.TabIndex = 10;
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.scalePanel.BackgroundImage = global::TimeTrack.Properties.Resources.scaleBar;
            this.scalePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.scalePanel.Controls.Add(this.plusButton);
            this.scalePanel.Controls.Add(this.scaleButton);
            this.scalePanel.Controls.Add(this.minusButton);
            this.scalePanel.Location = new System.Drawing.Point(839, 3);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Size = new System.Drawing.Size(127, 37);
            this.scalePanel.TabIndex = 2;
            // 
            // plusButton
            // 
            this.plusButton.BackgroundImage = global::TimeTrack.Properties.Resources.plus_scale;
            this.plusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plusButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plusButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.plusButton.Location = new System.Drawing.Point(109, 0);
            this.plusButton.Name = "plusButton";
            this.plusButton.Size = new System.Drawing.Size(18, 37);
            this.plusButton.TabIndex = 0;
            // 
            // scaleButton
            // 
            this.scaleButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.scaleButton.BackgroundImage = global::TimeTrack.Properties.Resources.scalePoint;
            this.scaleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.scaleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.scaleButton.Location = new System.Drawing.Point(34, 7);
            this.scaleButton.Name = "scaleButton";
            this.scaleButton.Size = new System.Drawing.Size(11, 23);
            this.scaleButton.TabIndex = 1;
            // 
            // minusButton
            // 
            this.minusButton.BackgroundImage = global::TimeTrack.Properties.Resources.minus_scale;
            this.minusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minusButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minusButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.minusButton.Location = new System.Drawing.Point(0, 0);
            this.minusButton.Name = "minusButton";
            this.minusButton.Size = new System.Drawing.Size(18, 37);
            this.minusButton.TabIndex = 2;
            // 
            // gotoCurrentButton
            // 
            this.gotoCurrentButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gotoCurrentButton.BackgroundImage = global::TimeTrack.Properties.Resources.current;
            this.gotoCurrentButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gotoCurrentButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoCurrentButton.Location = new System.Drawing.Point(598, 6);
            this.gotoCurrentButton.Name = "gotoCurrentButton";
            this.gotoCurrentButton.Size = new System.Drawing.Size(42, 26);
            this.gotoCurrentButton.TabIndex = 3;
            // 
            // scaleLabel
            // 
            this.scaleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scaleLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleLabel.ForeColor = System.Drawing.Color.White;
            this.scaleLabel.Location = new System.Drawing.Point(968, 0);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(68, 37);
            this.scaleLabel.TabIndex = 4;
            this.scaleLabel.Text = "10 Sec";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // minimizePanel
            // 
            this.minimizePanel.BackColor = System.Drawing.Color.Transparent;
            this.minimizePanel.BackgroundImage = global::TimeTrack.Properties.Resources.mini2;
            this.minimizePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minimizePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.minimizePanel.Location = new System.Drawing.Point(0, 0);
            this.minimizePanel.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePanel.Name = "minimizePanel";
            this.minimizePanel.Size = new System.Drawing.Size(16, 37);
            this.minimizePanel.TabIndex = 5;
            // 
            // datePicker
            // 
            this.datePicker.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datePicker.CustomFormat = "yyyy-MM-dd";
            this.datePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(414, 7);
            this.datePicker.Margin = new System.Windows.Forms.Padding(0);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(101, 23);
            this.datePicker.TabIndex = 0;
            // 
            // timePicker
            // 
            this.timePicker.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.timePicker.CustomFormat = "HH:mm:ss";
            this.timePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePicker.Location = new System.Drawing.Point(520, 7);
            this.timePicker.Margin = new System.Windows.Forms.Padding(0);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(75, 23);
            this.timePicker.TabIndex = 1;
            // 
            // gotoBeginButton
            // 
            this.gotoBeginButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gotoBeginButton.BackgroundImage = global::TimeTrack.Properties.Resources.begin;
            this.gotoBeginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gotoBeginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoBeginButton.Location = new System.Drawing.Point(369, 6);
            this.gotoBeginButton.Name = "gotoBeginButton";
            this.gotoBeginButton.Size = new System.Drawing.Size(42, 26);
            this.gotoBeginButton.TabIndex = 6;
            // 
            // controllerPanel
            // 
            this.controllerPanel.BackColor = System.Drawing.Color.Transparent;
            this.controllerPanel.BackgroundImage = global::TimeTrack.Properties.Resources.controllerBG;
            this.controllerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.controllerPanel.Controls.Add(this.enablePlaybackSmoothCheckBox);
            this.controllerPanel.Controls.Add(this.withArchiveServerCheckBox);
            this.controllerPanel.Controls.Add(this.clearSelectionButton);
            this.controllerPanel.Controls.Add(this.setStartButton);
            this.controllerPanel.Controls.Add(this.setEndButton);
            this.controllerPanel.Controls.Add(this.addBookmarkButton);
            this.controllerPanel.Controls.Add(this.eraserBookmarkButton);
            this.controllerPanel.Controls.Add(this.previousBookmarkButton);
            this.controllerPanel.Controls.Add(this.nextBookmarkButton);
            this.controllerPanel.Controls.Add(this.previousRecordButton);
            this.controllerPanel.Controls.Add(this.nextRecordButton);
            this.controllerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controllerPanel.Location = new System.Drawing.Point(0, 0);
            this.controllerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.controllerPanel.Name = "controllerPanel";
            this.controllerPanel.Size = new System.Drawing.Size(1036, 37);
            this.controllerPanel.TabIndex = 3;
            // 
            // enablePlaybackSmoothCheckBox
            // 
            this.enablePlaybackSmoothCheckBox.AutoSize = true;
            this.enablePlaybackSmoothCheckBox.Font = new System.Drawing.Font("Arial", 9F);
            this.enablePlaybackSmoothCheckBox.ForeColor = System.Drawing.Color.White;
            this.enablePlaybackSmoothCheckBox.Location = new System.Drawing.Point(190, 9);
            this.enablePlaybackSmoothCheckBox.Name = "enablePlaybackSmoothCheckBox";
            this.enablePlaybackSmoothCheckBox.Size = new System.Drawing.Size(171, 19);
            this.enablePlaybackSmoothCheckBox.TabIndex = 17;
            this.enablePlaybackSmoothCheckBox.Text = "Enable Playback Smoothly";
            this.enablePlaybackSmoothCheckBox.UseVisualStyleBackColor = true;
            this.enablePlaybackSmoothCheckBox.Visible = false;
            // 
            // withArchiveServerCheckBox
            // 
            this.withArchiveServerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.withArchiveServerCheckBox.AutoSize = true;
            this.withArchiveServerCheckBox.Font = new System.Drawing.Font("Arial", 9F);
            this.withArchiveServerCheckBox.ForeColor = System.Drawing.Color.White;
            this.withArchiveServerCheckBox.Location = new System.Drawing.Point(698, 10);
            this.withArchiveServerCheckBox.Name = "withArchiveServerCheckBox";
            this.withArchiveServerCheckBox.Size = new System.Drawing.Size(129, 19);
            this.withArchiveServerCheckBox.TabIndex = 16;
            this.withArchiveServerCheckBox.Text = "With Archive Server";
            this.withArchiveServerCheckBox.UseVisualStyleBackColor = true;
            this.withArchiveServerCheckBox.Visible = false;
            // 
            // clearSelectionButton
            // 
            this.clearSelectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearSelectionButton.BackgroundImage = global::TimeTrack.Properties.Resources.set_clear;
            this.clearSelectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.clearSelectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearSelectionButton.Location = new System.Drawing.Point(830, 5);
            this.clearSelectionButton.Margin = new System.Windows.Forms.Padding(0);
            this.clearSelectionButton.Name = "clearSelectionButton";
            this.clearSelectionButton.Size = new System.Drawing.Size(40, 26);
            this.clearSelectionButton.TabIndex = 15;
            this.clearSelectionButton.Visible = false;
            // 
            // setStartButton
            // 
            this.setStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setStartButton.BackgroundImage = global::TimeTrack.Properties.Resources.set_start;
            this.setStartButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.setStartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setStartButton.Location = new System.Drawing.Point(870, 5);
            this.setStartButton.Margin = new System.Windows.Forms.Padding(0);
            this.setStartButton.Name = "setStartButton";
            this.setStartButton.Size = new System.Drawing.Size(35, 26);
            this.setStartButton.TabIndex = 14;
            this.setStartButton.Visible = false;
            // 
            // setEndButton
            // 
            this.setEndButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setEndButton.BackgroundImage = global::TimeTrack.Properties.Resources.set_end;
            this.setEndButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.setEndButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setEndButton.Location = new System.Drawing.Point(905, 5);
            this.setEndButton.Margin = new System.Windows.Forms.Padding(0);
            this.setEndButton.Name = "setEndButton";
            this.setEndButton.Size = new System.Drawing.Size(36, 26);
            this.setEndButton.TabIndex = 13;
            this.setEndButton.Visible = false;
            // 
            // addBookmarkButton
            // 
            this.addBookmarkButton.BackgroundImage = global::TimeTrack.Properties.Resources.bookmark;
            this.addBookmarkButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addBookmarkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addBookmarkButton.Location = new System.Drawing.Point(31, 5);
            this.addBookmarkButton.Margin = new System.Windows.Forms.Padding(0);
            this.addBookmarkButton.Name = "addBookmarkButton";
            this.addBookmarkButton.Size = new System.Drawing.Size(40, 26);
            this.addBookmarkButton.TabIndex = 5;
            this.addBookmarkButton.Visible = false;
            // 
            // eraserBookmarkButton
            // 
            this.eraserBookmarkButton.BackgroundImage = global::TimeTrack.Properties.Resources.eraser;
            this.eraserBookmarkButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.eraserBookmarkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.eraserBookmarkButton.Location = new System.Drawing.Point(71, 5);
            this.eraserBookmarkButton.Margin = new System.Windows.Forms.Padding(0);
            this.eraserBookmarkButton.Name = "eraserBookmarkButton";
            this.eraserBookmarkButton.Size = new System.Drawing.Size(37, 26);
            this.eraserBookmarkButton.TabIndex = 12;
            this.eraserBookmarkButton.Visible = false;
            // 
            // previousBookmarkButton
            // 
            this.previousBookmarkButton.BackgroundImage = global::TimeTrack.Properties.Resources.previous_bookmark;
            this.previousBookmarkButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.previousBookmarkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousBookmarkButton.Location = new System.Drawing.Point(108, 5);
            this.previousBookmarkButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousBookmarkButton.Name = "previousBookmarkButton";
            this.previousBookmarkButton.Size = new System.Drawing.Size(35, 26);
            this.previousBookmarkButton.TabIndex = 11;
            this.previousBookmarkButton.Visible = false;
            // 
            // nextBookmarkButton
            // 
            this.nextBookmarkButton.BackgroundImage = global::TimeTrack.Properties.Resources.next_bookmark;
            this.nextBookmarkButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextBookmarkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextBookmarkButton.Location = new System.Drawing.Point(143, 5);
            this.nextBookmarkButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextBookmarkButton.Name = "nextBookmarkButton";
            this.nextBookmarkButton.Size = new System.Drawing.Size(38, 26);
            this.nextBookmarkButton.TabIndex = 10;
            this.nextBookmarkButton.Visible = false;
            // 
            // previousRecordButton
            // 
            this.previousRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previousRecordButton.BackgroundImage = global::TimeTrack.Properties.Resources.previous_record;
            this.previousRecordButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.previousRecordButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousRecordButton.Location = new System.Drawing.Point(941, 5);
            this.previousRecordButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousRecordButton.Name = "previousRecordButton";
            this.previousRecordButton.Size = new System.Drawing.Size(36, 26);
            this.previousRecordButton.TabIndex = 12;
            this.previousRecordButton.Visible = false;
            // 
            // nextRecordButton
            // 
            this.nextRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextRecordButton.BackgroundImage = global::TimeTrack.Properties.Resources.next_record;
            this.nextRecordButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextRecordButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextRecordButton.Location = new System.Drawing.Point(977, 5);
            this.nextRecordButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextRecordButton.Name = "nextRecordButton";
            this.nextRecordButton.Size = new System.Drawing.Size(38, 26);
            this.nextRecordButton.TabIndex = 11;
            this.nextRecordButton.Visible = false;
            // 
            // TimeTrack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::TimeTrack.Properties.Resources.deviceContainerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.devicePanel);
            this.Controls.Add(this.trackPanel);
            this.Controls.Add(this.switchPanel);
            this.Controls.Add(this.toolPanel);
            this.Controls.Add(this.controllerPanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TimeTrack";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.Size = new System.Drawing.Size(1036, 210);
            this.trackPanel.ResumeLayout(false);
            this.switchPanel.ResumeLayout(false);
            this.toolPanel.ResumeLayout(false);
            this.eventPanel.ResumeLayout(false);
            this.scalePanel.ResumeLayout(false);
            this.controllerPanel.ResumeLayout(false);
            this.controllerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel toolPanel;
        protected PanelBase.DoubleBufferPanel controllerPanel;
        protected Panel trackPanel;
        protected Panel plusButton;
        private Panel minimizePanel;
        protected Panel addBookmarkButton;
        protected Panel rangeRightPanel;
        protected Panel rangeLeftPanel;
        protected Panel minusButton;
        private PanelBase.DoubleBufferPanel scalePanel;
        protected Panel scaleButton;
        private Label scaleLabel;
        protected Panel previousBookmarkButton;
        protected Panel nextBookmarkButton;
        protected Panel previousRecordButton;
        protected Panel nextRecordButton;
        protected DateTimePicker datePicker;
        protected DateTimePicker timePicker;
        protected Panel gotoCurrentButton;
        protected Panel eraserBookmarkButton;
        protected PanelBase.DoubleBufferPanel devicePanel;
        protected PanelBase.DoubleBufferPanel switchPanel;
        protected Panel setStartButton;
        protected Panel setEndButton;
        private Label pointerLabel;
        protected Panel clearSelectionButton;
        protected EventButton motionButton;
        protected EventButton videoRecoveryButton;
        protected EventButton videoLossButton;
        protected EventButton networkRecoveryButton;
        protected EventButton networkLossButton;
        protected EventButton manualRecordButton;
        protected EventButton doButton;
        protected EventButton diButton;
        protected EventButton audioOutButton;
        protected EventButton audioInButton;
        protected EventButton panicButton;
        protected Panel upButton;
        protected Panel downButton;
        private PanelBase.DoubleBufferLabel loadingLabel;
        protected EventButton crossLineButton;
        protected PanelBase.DoubleBufferPanel eventPanel;
        protected Panel eventPanelButton;
        protected Panel gotoBeginButton;
        protected EventButton objectCountingOutButton;
        protected EventButton objectCountingInButton;
        protected EventButton loiteringDetectionButton;
        protected EventButton instrusionDetectionButton;
        protected EventButton audioDetectionButton;
        protected EventButton temperingDetectionButton;
        protected EventButton userdefineButton;
        private CheckBox withArchiveServerCheckBox;
        private CheckBox enablePlaybackSmoothCheckBox;
    }
}
