namespace SetupDevice
{
    sealed partial class VideoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtspPanel = new PanelBase.DoubleBufferPanel();
            this.rtspPortTextBox = new PanelBase.HotKeyTextBox();
            this.streamPanel = new PanelBase.DoubleBufferPanel();
            this.streamPortTextBox = new PanelBase.HotKeyTextBox();
            this.controlPanel = new PanelBase.DoubleBufferPanel();
            this.controlPortTextBox = new PanelBase.HotKeyTextBox();
            this.bitratePanel = new PanelBase.DoubleBufferPanel();
            this.bitrateComboBox = new System.Windows.Forms.ComboBox();
            this.qualityPanel = new PanelBase.DoubleBufferPanel();
            this.qualityComboBox = new System.Windows.Forms.ComboBox();
            this.fpsPanel = new PanelBase.DoubleBufferPanel();
            this.fpsComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionPanel = new PanelBase.DoubleBufferPanel();
            this.resolutionComboBox = new System.Windows.Forms.ComboBox();
            this.compressionPanel = new PanelBase.DoubleBufferPanel();
            this.compressionComboBox = new System.Windows.Forms.ComboBox();
            this.protocolPanel = new PanelBase.DoubleBufferPanel();
            this.protocolComboBox = new System.Windows.Forms.ComboBox();
            this.channelIdPanel = new PanelBase.DoubleBufferPanel();
            this.channelIdComboBox = new System.Windows.Forms.ComboBox();
            this.uriPanel = new PanelBase.DoubleBufferPanel();
            this.uriTextBox = new System.Windows.Forms.TextBox();
            this.thresholdPanel = new PanelBase.DoubleBufferPanel();
            this.thresholdComboBox = new System.Windows.Forms.ComboBox();
            this.bitrateControlPanel = new PanelBase.DoubleBufferPanel();
            this.bitrateControlcomboBox = new System.Windows.Forms.ComboBox();
            this.multicastNetwordAddressPanel = new PanelBase.DoubleBufferPanel();
            this.multicastNetworkAddressTextBox = new System.Windows.Forms.TextBox();
            this.videoInPortPanel = new PanelBase.DoubleBufferPanel();
            this.videoInPortTextBox = new PanelBase.HotKeyTextBox();
            this.audioInPortPanel = new PanelBase.DoubleBufferPanel();
            this.audioInPortTextBox = new PanelBase.HotKeyTextBox();
            this.profileModePanel = new PanelBase.DoubleBufferPanel();
            this.profileModeComboBox = new System.Windows.Forms.ComboBox();
            this.dewarpModePanel = new PanelBase.DoubleBufferPanel();
            this.dewarpModeComboBox = new System.Windows.Forms.ComboBox();
            this.httpsPanel = new PanelBase.DoubleBufferPanel();
            this.httpsTextBox = new PanelBase.HotKeyTextBox();
            this.rtspPanel.SuspendLayout();
            this.streamPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.bitratePanel.SuspendLayout();
            this.qualityPanel.SuspendLayout();
            this.fpsPanel.SuspendLayout();
            this.resolutionPanel.SuspendLayout();
            this.compressionPanel.SuspendLayout();
            this.protocolPanel.SuspendLayout();
            this.channelIdPanel.SuspendLayout();
            this.uriPanel.SuspendLayout();
            this.thresholdPanel.SuspendLayout();
            this.bitrateControlPanel.SuspendLayout();
            this.multicastNetwordAddressPanel.SuspendLayout();
            this.videoInPortPanel.SuspendLayout();
            this.audioInPortPanel.SuspendLayout();
            this.profileModePanel.SuspendLayout();
            this.dewarpModePanel.SuspendLayout();
            this.httpsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtspPanel
            // 
            this.rtspPanel.BackColor = System.Drawing.Color.Transparent;
            this.rtspPanel.Controls.Add(this.rtspPortTextBox);
            this.rtspPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtspPanel.Location = new System.Drawing.Point(0, 465);
            this.rtspPanel.Name = "rtspPanel";
            this.rtspPanel.Size = new System.Drawing.Size(465, 40);
            this.rtspPanel.TabIndex = 18;
            this.rtspPanel.Tag = "RTSPPort";
            // 
            // rtspPortTextBox
            // 
            this.rtspPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtspPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtspPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.rtspPortTextBox.Location = new System.Drawing.Point(269, 8);
            this.rtspPortTextBox.MaxLength = 5;
            this.rtspPortTextBox.Name = "rtspPortTextBox";
            this.rtspPortTextBox.ShortcutsEnabled = false;
            this.rtspPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.rtspPortTextBox.TabIndex = 2;
            // 
            // streamPanel
            // 
            this.streamPanel.BackColor = System.Drawing.Color.Transparent;
            this.streamPanel.Controls.Add(this.streamPortTextBox);
            this.streamPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.streamPanel.Location = new System.Drawing.Point(0, 425);
            this.streamPanel.Name = "streamPanel";
            this.streamPanel.Size = new System.Drawing.Size(465, 40);
            this.streamPanel.TabIndex = 17;
            this.streamPanel.Tag = "StreamPort";
            // 
            // streamPortTextBox
            // 
            this.streamPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.streamPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.streamPortTextBox.Location = new System.Drawing.Point(269, 8);
            this.streamPortTextBox.MaxLength = 5;
            this.streamPortTextBox.Name = "streamPortTextBox";
            this.streamPortTextBox.ShortcutsEnabled = false;
            this.streamPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.streamPortTextBox.TabIndex = 2;
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.Transparent;
            this.controlPanel.Controls.Add(this.controlPortTextBox);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 385);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(465, 40);
            this.controlPanel.TabIndex = 16;
            this.controlPanel.Tag = "ControlPort";
            // 
            // controlPortTextBox
            // 
            this.controlPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.controlPortTextBox.Location = new System.Drawing.Point(269, 8);
            this.controlPortTextBox.MaxLength = 5;
            this.controlPortTextBox.Name = "controlPortTextBox";
            this.controlPortTextBox.ShortcutsEnabled = false;
            this.controlPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.controlPortTextBox.TabIndex = 2;
            // 
            // bitratePanel
            // 
            this.bitratePanel.BackColor = System.Drawing.Color.Transparent;
            this.bitratePanel.Controls.Add(this.bitrateComboBox);
            this.bitratePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.bitratePanel.Location = new System.Drawing.Point(0, 345);
            this.bitratePanel.Name = "bitratePanel";
            this.bitratePanel.Size = new System.Drawing.Size(465, 40);
            this.bitratePanel.TabIndex = 13;
            this.bitratePanel.Tag = "Bitrate";
            // 
            // bitrateComboBox
            // 
            this.bitrateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bitrateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitrateComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bitrateComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bitrateComboBox.FormattingEnabled = true;
            this.bitrateComboBox.IntegralHeight = false;
            this.bitrateComboBox.Location = new System.Drawing.Point(269, 8);
            this.bitrateComboBox.MaxDropDownItems = 20;
            this.bitrateComboBox.Name = "bitrateComboBox";
            this.bitrateComboBox.Size = new System.Drawing.Size(181, 23);
            this.bitrateComboBox.TabIndex = 3;
            // 
            // qualityPanel
            // 
            this.qualityPanel.BackColor = System.Drawing.Color.Transparent;
            this.qualityPanel.Controls.Add(this.qualityComboBox);
            this.qualityPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.qualityPanel.Location = new System.Drawing.Point(0, 305);
            this.qualityPanel.Name = "qualityPanel";
            this.qualityPanel.Size = new System.Drawing.Size(465, 40);
            this.qualityPanel.TabIndex = 11;
            this.qualityPanel.Tag = "Quality";
            // 
            // qualityComboBox
            // 
            this.qualityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.qualityComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.qualityComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qualityComboBox.FormattingEnabled = true;
            this.qualityComboBox.IntegralHeight = false;
            this.qualityComboBox.Location = new System.Drawing.Point(269, 8);
            this.qualityComboBox.MaxDropDownItems = 20;
            this.qualityComboBox.Name = "qualityComboBox";
            this.qualityComboBox.Size = new System.Drawing.Size(181, 23);
            this.qualityComboBox.TabIndex = 3;
            // 
            // fpsPanel
            // 
            this.fpsPanel.BackColor = System.Drawing.Color.Transparent;
            this.fpsPanel.Controls.Add(this.fpsComboBox);
            this.fpsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpsPanel.Location = new System.Drawing.Point(0, 225);
            this.fpsPanel.Name = "fpsPanel";
            this.fpsPanel.Size = new System.Drawing.Size(465, 40);
            this.fpsPanel.TabIndex = 12;
            this.fpsPanel.Tag = "FPS";
            // 
            // fpsComboBox
            // 
            this.fpsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fpsComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsComboBox.FormattingEnabled = true;
            this.fpsComboBox.IntegralHeight = false;
            this.fpsComboBox.Location = new System.Drawing.Point(269, 8);
            this.fpsComboBox.MaxDropDownItems = 20;
            this.fpsComboBox.Name = "fpsComboBox";
            this.fpsComboBox.Size = new System.Drawing.Size(181, 23);
            this.fpsComboBox.TabIndex = 2;
            // 
            // resolutionPanel
            // 
            this.resolutionPanel.BackColor = System.Drawing.Color.Transparent;
            this.resolutionPanel.Controls.Add(this.resolutionComboBox);
            this.resolutionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resolutionPanel.Location = new System.Drawing.Point(0, 185);
            this.resolutionPanel.Name = "resolutionPanel";
            this.resolutionPanel.Size = new System.Drawing.Size(465, 40);
            this.resolutionPanel.TabIndex = 10;
            this.resolutionPanel.Tag = "Resolution";
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resolutionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resolutionComboBox.FormattingEnabled = true;
            this.resolutionComboBox.IntegralHeight = false;
            this.resolutionComboBox.Location = new System.Drawing.Point(269, 8);
            this.resolutionComboBox.MaxDropDownItems = 20;
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.Size = new System.Drawing.Size(181, 23);
            this.resolutionComboBox.TabIndex = 2;
            // 
            // compressionPanel
            // 
            this.compressionPanel.BackColor = System.Drawing.Color.Transparent;
            this.compressionPanel.Controls.Add(this.compressionComboBox);
            this.compressionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.compressionPanel.Location = new System.Drawing.Point(0, 105);
            this.compressionPanel.Name = "compressionPanel";
            this.compressionPanel.Size = new System.Drawing.Size(465, 40);
            this.compressionPanel.TabIndex = 9;
            this.compressionPanel.Tag = "Compression";
            this.compressionPanel.Visible = false;
            // 
            // compressionComboBox
            // 
            this.compressionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.compressionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.compressionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.compressionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compressionComboBox.FormattingEnabled = true;
            this.compressionComboBox.IntegralHeight = false;
            this.compressionComboBox.Location = new System.Drawing.Point(269, 8);
            this.compressionComboBox.MaxDropDownItems = 20;
            this.compressionComboBox.Name = "compressionComboBox";
            this.compressionComboBox.Size = new System.Drawing.Size(181, 23);
            this.compressionComboBox.TabIndex = 3;
            // 
            // protocolPanel
            // 
            this.protocolPanel.BackColor = System.Drawing.Color.Transparent;
            this.protocolPanel.Controls.Add(this.protocolComboBox);
            this.protocolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.protocolPanel.Location = new System.Drawing.Point(0, 65);
            this.protocolPanel.Name = "protocolPanel";
            this.protocolPanel.Size = new System.Drawing.Size(465, 40);
            this.protocolPanel.TabIndex = 19;
            this.protocolPanel.Tag = "Protocol";
            // 
            // protocolComboBox
            // 
            this.protocolComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.protocolComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.protocolComboBox.FormattingEnabled = true;
            this.protocolComboBox.IntegralHeight = false;
            this.protocolComboBox.Location = new System.Drawing.Point(269, 8);
            this.protocolComboBox.MaxDropDownItems = 20;
            this.protocolComboBox.Name = "protocolComboBox";
            this.protocolComboBox.Size = new System.Drawing.Size(181, 23);
            this.protocolComboBox.TabIndex = 2;
            // 
            // channelIdPanel
            // 
            this.channelIdPanel.BackColor = System.Drawing.Color.Transparent;
            this.channelIdPanel.Controls.Add(this.channelIdComboBox);
            this.channelIdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.channelIdPanel.Location = new System.Drawing.Point(0, 25);
            this.channelIdPanel.Name = "channelIdPanel";
            this.channelIdPanel.Size = new System.Drawing.Size(465, 40);
            this.channelIdPanel.TabIndex = 20;
            this.channelIdPanel.Tag = "ChannelID";
            // 
            // channelIdComboBox
            // 
            this.channelIdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.channelIdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelIdComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelIdComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelIdComboBox.FormattingEnabled = true;
            this.channelIdComboBox.IntegralHeight = false;
            this.channelIdComboBox.Location = new System.Drawing.Point(269, 8);
            this.channelIdComboBox.MaxDropDownItems = 20;
            this.channelIdComboBox.Name = "channelIdComboBox";
            this.channelIdComboBox.Size = new System.Drawing.Size(181, 23);
            this.channelIdComboBox.TabIndex = 2;
            // 
            // uriPanel
            // 
            this.uriPanel.BackColor = System.Drawing.Color.Transparent;
            this.uriPanel.Controls.Add(this.uriTextBox);
            this.uriPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.uriPanel.Location = new System.Drawing.Point(0, 505);
            this.uriPanel.Name = "uriPanel";
            this.uriPanel.Size = new System.Drawing.Size(465, 40);
            this.uriPanel.TabIndex = 32;
            this.uriPanel.Tag = "URI";
            // 
            // uriTextBox
            // 
            this.uriTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uriTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uriTextBox.Location = new System.Drawing.Point(269, 8);
            this.uriTextBox.Name = "uriTextBox";
            this.uriTextBox.Size = new System.Drawing.Size(181, 21);
            this.uriTextBox.TabIndex = 2;
            // 
            // thresholdPanel
            // 
            this.thresholdPanel.BackColor = System.Drawing.Color.Transparent;
            this.thresholdPanel.Controls.Add(this.thresholdComboBox);
            this.thresholdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.thresholdPanel.Location = new System.Drawing.Point(0, 545);
            this.thresholdPanel.Name = "thresholdPanel";
            this.thresholdPanel.Size = new System.Drawing.Size(465, 40);
            this.thresholdPanel.TabIndex = 33;
            this.thresholdPanel.Tag = "MotionThreshold";
            this.thresholdPanel.Visible = false;
            // 
            // thresholdComboBox
            // 
            this.thresholdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thresholdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thresholdComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thresholdComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thresholdComboBox.FormattingEnabled = true;
            this.thresholdComboBox.IntegralHeight = false;
            this.thresholdComboBox.Location = new System.Drawing.Point(269, 8);
            this.thresholdComboBox.MaxDropDownItems = 20;
            this.thresholdComboBox.Name = "thresholdComboBox";
            this.thresholdComboBox.Size = new System.Drawing.Size(181, 23);
            this.thresholdComboBox.TabIndex = 3;
            // 
            // bitrateControlPanel
            // 
            this.bitrateControlPanel.BackColor = System.Drawing.Color.Transparent;
            this.bitrateControlPanel.Controls.Add(this.bitrateControlcomboBox);
            this.bitrateControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.bitrateControlPanel.Location = new System.Drawing.Point(0, 265);
            this.bitrateControlPanel.Name = "bitrateControlPanel";
            this.bitrateControlPanel.Size = new System.Drawing.Size(465, 40);
            this.bitrateControlPanel.TabIndex = 34;
            this.bitrateControlPanel.Tag = "BitrateControl";
            // 
            // bitrateControlcomboBox
            // 
            this.bitrateControlcomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bitrateControlcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitrateControlcomboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bitrateControlcomboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bitrateControlcomboBox.FormattingEnabled = true;
            this.bitrateControlcomboBox.IntegralHeight = false;
            this.bitrateControlcomboBox.Location = new System.Drawing.Point(269, 8);
            this.bitrateControlcomboBox.MaxDropDownItems = 20;
            this.bitrateControlcomboBox.Name = "bitrateControlcomboBox";
            this.bitrateControlcomboBox.Size = new System.Drawing.Size(181, 23);
            this.bitrateControlcomboBox.TabIndex = 3;
            // 
            // multicastNetwordAddressPanel
            // 
            this.multicastNetwordAddressPanel.BackColor = System.Drawing.Color.Transparent;
            this.multicastNetwordAddressPanel.Controls.Add(this.multicastNetworkAddressTextBox);
            this.multicastNetwordAddressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.multicastNetwordAddressPanel.Location = new System.Drawing.Point(0, 585);
            this.multicastNetwordAddressPanel.Name = "multicastNetwordAddressPanel";
            this.multicastNetwordAddressPanel.Size = new System.Drawing.Size(465, 40);
            this.multicastNetwordAddressPanel.TabIndex = 35;
            this.multicastNetwordAddressPanel.Tag = "MulticastNetworkAddress";
            // 
            // multicastNetworkAddressTextBox
            // 
            this.multicastNetworkAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.multicastNetworkAddressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multicastNetworkAddressTextBox.Location = new System.Drawing.Point(269, 8);
            this.multicastNetworkAddressTextBox.Name = "multicastNetworkAddressTextBox";
            this.multicastNetworkAddressTextBox.Size = new System.Drawing.Size(181, 21);
            this.multicastNetworkAddressTextBox.TabIndex = 2;
            // 
            // videoInPortPanel
            // 
            this.videoInPortPanel.BackColor = System.Drawing.Color.Transparent;
            this.videoInPortPanel.Controls.Add(this.videoInPortTextBox);
            this.videoInPortPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.videoInPortPanel.Location = new System.Drawing.Point(0, 625);
            this.videoInPortPanel.Name = "videoInPortPanel";
            this.videoInPortPanel.Size = new System.Drawing.Size(465, 40);
            this.videoInPortPanel.TabIndex = 36;
            this.videoInPortPanel.Tag = "VideoInPort";
            // 
            // videoInPortTextBox
            // 
            this.videoInPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.videoInPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoInPortTextBox.Location = new System.Drawing.Point(269, 8);
            this.videoInPortTextBox.Name = "videoInPortTextBox";
            this.videoInPortTextBox.ShortcutsEnabled = false;
            this.videoInPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.videoInPortTextBox.TabIndex = 2;
            // 
            // audioInPortPanel
            // 
            this.audioInPortPanel.BackColor = System.Drawing.Color.Transparent;
            this.audioInPortPanel.Controls.Add(this.audioInPortTextBox);
            this.audioInPortPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.audioInPortPanel.Location = new System.Drawing.Point(0, 665);
            this.audioInPortPanel.Name = "audioInPortPanel";
            this.audioInPortPanel.Size = new System.Drawing.Size(465, 40);
            this.audioInPortPanel.TabIndex = 37;
            this.audioInPortPanel.Tag = "AudioInPort";
            // 
            // audioInPortTextBox
            // 
            this.audioInPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioInPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioInPortTextBox.Location = new System.Drawing.Point(269, 8);
            this.audioInPortTextBox.Name = "audioInPortTextBox";
            this.audioInPortTextBox.ShortcutsEnabled = false;
            this.audioInPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.audioInPortTextBox.TabIndex = 2;
            // 
            // profileModePanel
            // 
            this.profileModePanel.BackColor = System.Drawing.Color.Transparent;
            this.profileModePanel.Controls.Add(this.profileModeComboBox);
            this.profileModePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.profileModePanel.Location = new System.Drawing.Point(0, 705);
            this.profileModePanel.Name = "profileModePanel";
            this.profileModePanel.Size = new System.Drawing.Size(465, 40);
            this.profileModePanel.TabIndex = 38;
            this.profileModePanel.Tag = "ProfileMode";
            this.profileModePanel.Visible = false;
            // 
            // profileModeComboBox
            // 
            this.profileModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.profileModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileModeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.profileModeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileModeComboBox.FormattingEnabled = true;
            this.profileModeComboBox.IntegralHeight = false;
            this.profileModeComboBox.Location = new System.Drawing.Point(269, 8);
            this.profileModeComboBox.MaxDropDownItems = 20;
            this.profileModeComboBox.Name = "profileModeComboBox";
            this.profileModeComboBox.Size = new System.Drawing.Size(181, 23);
            this.profileModeComboBox.TabIndex = 3;
            // 
            // dewarpModePanel
            // 
            this.dewarpModePanel.BackColor = System.Drawing.Color.Transparent;
            this.dewarpModePanel.Controls.Add(this.dewarpModeComboBox);
            this.dewarpModePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dewarpModePanel.Location = new System.Drawing.Point(0, 145);
            this.dewarpModePanel.Name = "dewarpModePanel";
            this.dewarpModePanel.Size = new System.Drawing.Size(465, 40);
            this.dewarpModePanel.TabIndex = 39;
            this.dewarpModePanel.Tag = "DewarpMode";
            this.dewarpModePanel.Visible = false;
            // 
            // dewarpModeComboBox
            // 
            this.dewarpModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dewarpModeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dewarpModeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpModeComboBox.FormattingEnabled = true;
            this.dewarpModeComboBox.IntegralHeight = false;
            this.dewarpModeComboBox.Location = new System.Drawing.Point(269, 8);
            this.dewarpModeComboBox.MaxDropDownItems = 20;
            this.dewarpModeComboBox.Name = "dewarpModeComboBox";
            this.dewarpModeComboBox.Size = new System.Drawing.Size(181, 23);
            this.dewarpModeComboBox.TabIndex = 3;
            // 
            // httpsPanel
            // 
            this.httpsPanel.BackColor = System.Drawing.Color.Transparent;
            this.httpsPanel.Controls.Add(this.httpsTextBox);
            this.httpsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.httpsPanel.Location = new System.Drawing.Point(0, 745);
            this.httpsPanel.Name = "httpsPanel";
            this.httpsPanel.Size = new System.Drawing.Size(465, 40);
            this.httpsPanel.TabIndex = 40;
            this.httpsPanel.Tag = "HTTPSPort";
            // 
            // httpsTextBox
            // 
            this.httpsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.httpsTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpsTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.httpsTextBox.Location = new System.Drawing.Point(269, 8);
            this.httpsTextBox.MaxLength = 5;
            this.httpsTextBox.Name = "httpsTextBox";
            this.httpsTextBox.ShortcutsEnabled = false;
            this.httpsTextBox.Size = new System.Drawing.Size(181, 21);
            this.httpsTextBox.TabIndex = 2;
            // 
            // VideoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.httpsPanel);
            this.Controls.Add(this.profileModePanel);
            this.Controls.Add(this.audioInPortPanel);
            this.Controls.Add(this.videoInPortPanel);
            this.Controls.Add(this.multicastNetwordAddressPanel);
            this.Controls.Add(this.thresholdPanel);
            this.Controls.Add(this.uriPanel);
            this.Controls.Add(this.rtspPanel);
            this.Controls.Add(this.streamPanel);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.bitratePanel);
            this.Controls.Add(this.qualityPanel);
            this.Controls.Add(this.bitrateControlPanel);
            this.Controls.Add(this.fpsPanel);
            this.Controls.Add(this.resolutionPanel);
            this.Controls.Add(this.dewarpModePanel);
            this.Controls.Add(this.compressionPanel);
            this.Controls.Add(this.protocolPanel);
            this.Controls.Add(this.channelIdPanel);
            this.Name = "VideoControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(465, 804);
            this.rtspPanel.ResumeLayout(false);
            this.rtspPanel.PerformLayout();
            this.streamPanel.ResumeLayout(false);
            this.streamPanel.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.bitratePanel.ResumeLayout(false);
            this.qualityPanel.ResumeLayout(false);
            this.fpsPanel.ResumeLayout(false);
            this.resolutionPanel.ResumeLayout(false);
            this.compressionPanel.ResumeLayout(false);
            this.protocolPanel.ResumeLayout(false);
            this.channelIdPanel.ResumeLayout(false);
            this.uriPanel.ResumeLayout(false);
            this.uriPanel.PerformLayout();
            this.thresholdPanel.ResumeLayout(false);
            this.bitrateControlPanel.ResumeLayout(false);
            this.multicastNetwordAddressPanel.ResumeLayout(false);
            this.multicastNetwordAddressPanel.PerformLayout();
            this.videoInPortPanel.ResumeLayout(false);
            this.videoInPortPanel.PerformLayout();
            this.audioInPortPanel.ResumeLayout(false);
            this.audioInPortPanel.PerformLayout();
            this.profileModePanel.ResumeLayout(false);
            this.dewarpModePanel.ResumeLayout(false);
            this.httpsPanel.ResumeLayout(false);
            this.httpsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel bitratePanel;
        private System.Windows.Forms.ComboBox bitrateComboBox;
        private PanelBase.DoubleBufferPanel fpsPanel;
        private System.Windows.Forms.ComboBox fpsComboBox;
        private PanelBase.DoubleBufferPanel qualityPanel;
        private System.Windows.Forms.ComboBox qualityComboBox;
        private PanelBase.DoubleBufferPanel resolutionPanel;
        private System.Windows.Forms.ComboBox resolutionComboBox;
        private PanelBase.DoubleBufferPanel compressionPanel;
        private System.Windows.Forms.ComboBox compressionComboBox;
        private PanelBase.DoubleBufferPanel controlPanel;
        private PanelBase.DoubleBufferPanel streamPanel;
        private PanelBase.DoubleBufferPanel rtspPanel;
        private PanelBase.DoubleBufferPanel protocolPanel;
        private System.Windows.Forms.ComboBox protocolComboBox;
        private PanelBase.DoubleBufferPanel channelIdPanel;
        private System.Windows.Forms.ComboBox channelIdComboBox;
        private PanelBase.DoubleBufferPanel uriPanel;
        private PanelBase.DoubleBufferPanel thresholdPanel;
        private System.Windows.Forms.ComboBox thresholdComboBox;
        private PanelBase.HotKeyTextBox controlPortTextBox;
        private PanelBase.HotKeyTextBox streamPortTextBox;
        private PanelBase.HotKeyTextBox rtspPortTextBox;
        private System.Windows.Forms.TextBox uriTextBox;
        private PanelBase.DoubleBufferPanel bitrateControlPanel;
        private System.Windows.Forms.ComboBox bitrateControlcomboBox;
        private PanelBase.DoubleBufferPanel multicastNetwordAddressPanel;
        private System.Windows.Forms.TextBox multicastNetworkAddressTextBox;
        private PanelBase.DoubleBufferPanel videoInPortPanel;
        private PanelBase.HotKeyTextBox videoInPortTextBox;
        private PanelBase.DoubleBufferPanel audioInPortPanel;
        private PanelBase.HotKeyTextBox audioInPortTextBox;
        private PanelBase.DoubleBufferPanel profileModePanel;
        private System.Windows.Forms.ComboBox profileModeComboBox;
        private PanelBase.DoubleBufferPanel dewarpModePanel;
        private System.Windows.Forms.ComboBox dewarpModeComboBox;
        private PanelBase.DoubleBufferPanel httpsPanel;
        private PanelBase.HotKeyTextBox httpsTextBox;
    }
}
