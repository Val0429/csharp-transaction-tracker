namespace SetupDevice
{
    sealed partial class ConnectionControl
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
            this.remoteRecoveryPanel = new PanelBase.DoubleBufferPanel();
            this.RemoteRecoveryCheckBox = new System.Windows.Forms.CheckBox();
            this.uriPanel = new PanelBase.DoubleBufferPanel();
            this.uriTextBox = new PanelBase.HotKeyTextBox();
            this.dewarpTypePanel = new PanelBase.DoubleBufferPanel();
            this.dewarpTypeComboBox = new System.Windows.Forms.ComboBox();
            this.powerFrequencyPanel = new PanelBase.DoubleBufferPanel();
            this.powerFrequencyComboBox = new System.Windows.Forms.ComboBox();
            this.sensorModePanel = new PanelBase.DoubleBufferPanel();
            this.sensorModeComboBox = new System.Windows.Forms.ComboBox();
            this.tvStandardPanel = new PanelBase.DoubleBufferPanel();
            this.tvStandardComboBox = new System.Windows.Forms.ComboBox();
            this.recordProfilePanel = new PanelBase.DoubleBufferPanel();
            this.recordStreamComboBox = new System.Windows.Forms.ComboBox();
            this.liveProfilePanel = new PanelBase.DoubleBufferPanel();
            this.streamComboBox = new System.Windows.Forms.ComboBox();
            this.modePanel = new PanelBase.DoubleBufferPanel();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.aspectRatioPanel = new PanelBase.DoubleBufferPanel();
            this.aspectRatioComboBox = new System.Windows.Forms.ComboBox();
            this.aspectRatioCorrectionPanel = new PanelBase.DoubleBufferPanel();
            this.aspectRatioCorrectionCheckBox = new System.Windows.Forms.CheckBox();
            this.audioPanel = new PanelBase.DoubleBufferPanel();
            this.audioPortTextBox = new PanelBase.HotKeyTextBox();
            this.httpPanel = new PanelBase.DoubleBufferPanel();
            this.httpPortTextBox = new PanelBase.HotKeyTextBox();
            this.networkAddressPanel = new PanelBase.DoubleBufferPanel();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.mountTypePanel = new PanelBase.DoubleBufferPanel();
            this.mountTypeComboBox = new System.Windows.Forms.ComboBox();
            this.remoteRecoveryPanel.SuspendLayout();
            this.uriPanel.SuspendLayout();
            this.dewarpTypePanel.SuspendLayout();
            this.powerFrequencyPanel.SuspendLayout();
            this.sensorModePanel.SuspendLayout();
            this.tvStandardPanel.SuspendLayout();
            this.recordProfilePanel.SuspendLayout();
            this.liveProfilePanel.SuspendLayout();
            this.modePanel.SuspendLayout();
            this.aspectRatioPanel.SuspendLayout();
            this.aspectRatioCorrectionPanel.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.httpPanel.SuspendLayout();
            this.networkAddressPanel.SuspendLayout();
            this.mountTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // remoteRecoveryPanel
            // 
            this.remoteRecoveryPanel.BackColor = System.Drawing.Color.Transparent;
            this.remoteRecoveryPanel.Controls.Add(this.RemoteRecoveryCheckBox);
            this.remoteRecoveryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.remoteRecoveryPanel.Location = new System.Drawing.Point(0, 585);
            this.remoteRecoveryPanel.Name = "remoteRecoveryPanel";
            this.remoteRecoveryPanel.Size = new System.Drawing.Size(431, 40);
            this.remoteRecoveryPanel.TabIndex = 33;
            this.remoteRecoveryPanel.Tag = "RemoteRecovery";
            // 
            // RemoteRecoveryCheckBox
            // 
            this.RemoteRecoveryCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoteRecoveryCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoteRecoveryCheckBox.Location = new System.Drawing.Point(238, 14);
            this.RemoteRecoveryCheckBox.Name = "RemoteRecoveryCheckBox";
            this.RemoteRecoveryCheckBox.Size = new System.Drawing.Size(178, 19);
            this.RemoteRecoveryCheckBox.TabIndex = 0;
            this.RemoteRecoveryCheckBox.Text = "Enable";
            this.RemoteRecoveryCheckBox.UseVisualStyleBackColor = true;
            // 
            // uriPanel
            // 
            this.uriPanel.BackColor = System.Drawing.Color.Transparent;
            this.uriPanel.Controls.Add(this.uriTextBox);
            this.uriPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.uriPanel.Location = new System.Drawing.Point(0, 545);
            this.uriPanel.Name = "uriPanel";
            this.uriPanel.Size = new System.Drawing.Size(431, 40);
            this.uriPanel.TabIndex = 31;
            this.uriPanel.Tag = "URI";
            this.uriPanel.Visible = false;
            // 
            // uriTextBox
            // 
            this.uriTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uriTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uriTextBox.Location = new System.Drawing.Point(235, 8);
            this.uriTextBox.Name = "uriTextBox";
            this.uriTextBox.ShortcutsEnabled = false;
            this.uriTextBox.Size = new System.Drawing.Size(181, 21);
            this.uriTextBox.TabIndex = 2;
            // 
            // dewarpTypePanel
            // 
            this.dewarpTypePanel.BackColor = System.Drawing.Color.Transparent;
            this.dewarpTypePanel.Controls.Add(this.dewarpTypeComboBox);
            this.dewarpTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dewarpTypePanel.Location = new System.Drawing.Point(0, 465);
            this.dewarpTypePanel.Name = "dewarpTypePanel";
            this.dewarpTypePanel.Size = new System.Drawing.Size(431, 40);
            this.dewarpTypePanel.TabIndex = 32;
            this.dewarpTypePanel.Tag = "DewarpType";
            // 
            // dewarpTypeComboBox
            // 
            this.dewarpTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dewarpTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dewarpTypeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpTypeComboBox.FormattingEnabled = true;
            this.dewarpTypeComboBox.IntegralHeight = false;
            this.dewarpTypeComboBox.Location = new System.Drawing.Point(235, 8);
            this.dewarpTypeComboBox.MaxDropDownItems = 20;
            this.dewarpTypeComboBox.Name = "dewarpTypeComboBox";
            this.dewarpTypeComboBox.Size = new System.Drawing.Size(181, 23);
            this.dewarpTypeComboBox.TabIndex = 2;
            // 
            // powerFrequencyPanel
            // 
            this.powerFrequencyPanel.BackColor = System.Drawing.Color.Transparent;
            this.powerFrequencyPanel.Controls.Add(this.powerFrequencyComboBox);
            this.powerFrequencyPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.powerFrequencyPanel.Location = new System.Drawing.Point(0, 425);
            this.powerFrequencyPanel.Name = "powerFrequencyPanel";
            this.powerFrequencyPanel.Size = new System.Drawing.Size(431, 40);
            this.powerFrequencyPanel.TabIndex = 17;
            this.powerFrequencyPanel.Tag = "PowerFrequency";
            // 
            // powerFrequencyComboBox
            // 
            this.powerFrequencyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.powerFrequencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerFrequencyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.powerFrequencyComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerFrequencyComboBox.FormattingEnabled = true;
            this.powerFrequencyComboBox.IntegralHeight = false;
            this.powerFrequencyComboBox.Location = new System.Drawing.Point(235, 8);
            this.powerFrequencyComboBox.MaxDropDownItems = 20;
            this.powerFrequencyComboBox.Name = "powerFrequencyComboBox";
            this.powerFrequencyComboBox.Size = new System.Drawing.Size(181, 23);
            this.powerFrequencyComboBox.TabIndex = 2;
            // 
            // sensorModePanel
            // 
            this.sensorModePanel.BackColor = System.Drawing.Color.Transparent;
            this.sensorModePanel.Controls.Add(this.sensorModeComboBox);
            this.sensorModePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.sensorModePanel.Location = new System.Drawing.Point(0, 385);
            this.sensorModePanel.Name = "sensorModePanel";
            this.sensorModePanel.Size = new System.Drawing.Size(431, 40);
            this.sensorModePanel.TabIndex = 16;
            this.sensorModePanel.Tag = "SensorMode";
            // 
            // sensorModeComboBox
            // 
            this.sensorModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sensorModeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sensorModeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensorModeComboBox.FormattingEnabled = true;
            this.sensorModeComboBox.IntegralHeight = false;
            this.sensorModeComboBox.Location = new System.Drawing.Point(235, 8);
            this.sensorModeComboBox.MaxDropDownItems = 20;
            this.sensorModeComboBox.Name = "sensorModeComboBox";
            this.sensorModeComboBox.Size = new System.Drawing.Size(181, 23);
            this.sensorModeComboBox.TabIndex = 2;
            // 
            // tvStandardPanel
            // 
            this.tvStandardPanel.BackColor = System.Drawing.Color.Transparent;
            this.tvStandardPanel.Controls.Add(this.tvStandardComboBox);
            this.tvStandardPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tvStandardPanel.Location = new System.Drawing.Point(0, 345);
            this.tvStandardPanel.Name = "tvStandardPanel";
            this.tvStandardPanel.Size = new System.Drawing.Size(431, 40);
            this.tvStandardPanel.TabIndex = 26;
            this.tvStandardPanel.Tag = "TVStandard";
            // 
            // tvStandardComboBox
            // 
            this.tvStandardComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tvStandardComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tvStandardComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tvStandardComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvStandardComboBox.FormattingEnabled = true;
            this.tvStandardComboBox.IntegralHeight = false;
            this.tvStandardComboBox.Location = new System.Drawing.Point(235, 8);
            this.tvStandardComboBox.MaxDropDownItems = 20;
            this.tvStandardComboBox.Name = "tvStandardComboBox";
            this.tvStandardComboBox.Size = new System.Drawing.Size(181, 23);
            this.tvStandardComboBox.TabIndex = 2;
            // 
            // recordProfilePanel
            // 
            this.recordProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.recordProfilePanel.Controls.Add(this.recordStreamComboBox);
            this.recordProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.recordProfilePanel.Location = new System.Drawing.Point(0, 305);
            this.recordProfilePanel.Name = "recordProfilePanel";
            this.recordProfilePanel.Size = new System.Drawing.Size(431, 40);
            this.recordProfilePanel.TabIndex = 30;
            this.recordProfilePanel.Tag = "RecordingStream";
            // 
            // recordStreamComboBox
            // 
            this.recordStreamComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordStreamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordStreamComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.recordStreamComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordStreamComboBox.FormattingEnabled = true;
            this.recordStreamComboBox.IntegralHeight = false;
            this.recordStreamComboBox.Location = new System.Drawing.Point(235, 8);
            this.recordStreamComboBox.MaxDropDownItems = 20;
            this.recordStreamComboBox.Name = "recordStreamComboBox";
            this.recordStreamComboBox.Size = new System.Drawing.Size(181, 23);
            this.recordStreamComboBox.TabIndex = 2;
            // 
            // liveProfilePanel
            // 
            this.liveProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.liveProfilePanel.Controls.Add(this.streamComboBox);
            this.liveProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.liveProfilePanel.Location = new System.Drawing.Point(0, 265);
            this.liveProfilePanel.Name = "liveProfilePanel";
            this.liveProfilePanel.Size = new System.Drawing.Size(431, 40);
            this.liveProfilePanel.TabIndex = 11;
            this.liveProfilePanel.Tag = "LiveStream";
            // 
            // streamComboBox
            // 
            this.streamComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.streamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.streamComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.streamComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamComboBox.FormattingEnabled = true;
            this.streamComboBox.IntegralHeight = false;
            this.streamComboBox.Location = new System.Drawing.Point(235, 8);
            this.streamComboBox.MaxDropDownItems = 20;
            this.streamComboBox.Name = "streamComboBox";
            this.streamComboBox.Size = new System.Drawing.Size(181, 23);
            this.streamComboBox.TabIndex = 2;
            // 
            // modePanel
            // 
            this.modePanel.BackColor = System.Drawing.Color.Transparent;
            this.modePanel.Controls.Add(this.modeComboBox);
            this.modePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.modePanel.Location = new System.Drawing.Point(0, 225);
            this.modePanel.Name = "modePanel";
            this.modePanel.Size = new System.Drawing.Size(431, 40);
            this.modePanel.TabIndex = 10;
            this.modePanel.Tag = "Mode";
            // 
            // modeComboBox
            // 
            this.modeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.modeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.IntegralHeight = false;
            this.modeComboBox.Location = new System.Drawing.Point(235, 8);
            this.modeComboBox.MaxDropDownItems = 20;
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(181, 23);
            this.modeComboBox.TabIndex = 2;
            // 
            // aspectRatioPanel
            // 
            this.aspectRatioPanel.BackColor = System.Drawing.Color.Transparent;
            this.aspectRatioPanel.Controls.Add(this.aspectRatioComboBox);
            this.aspectRatioPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.aspectRatioPanel.Location = new System.Drawing.Point(0, 185);
            this.aspectRatioPanel.Name = "aspectRatioPanel";
            this.aspectRatioPanel.Padding = new System.Windows.Forms.Padding(0, 8, 15, 0);
            this.aspectRatioPanel.Size = new System.Drawing.Size(431, 40);
            this.aspectRatioPanel.TabIndex = 29;
            this.aspectRatioPanel.Tag = "AspectRatio";
            // 
            // aspectRatioComboBox
            // 
            this.aspectRatioComboBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.aspectRatioComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aspectRatioComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.aspectRatioComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aspectRatioComboBox.FormattingEnabled = true;
            this.aspectRatioComboBox.IntegralHeight = false;
            this.aspectRatioComboBox.Location = new System.Drawing.Point(235, 8);
            this.aspectRatioComboBox.MaxDropDownItems = 20;
            this.aspectRatioComboBox.Name = "aspectRatioComboBox";
            this.aspectRatioComboBox.Size = new System.Drawing.Size(181, 23);
            this.aspectRatioComboBox.TabIndex = 2;
            // 
            // aspectRatioCorrectionPanel
            // 
            this.aspectRatioCorrectionPanel.BackColor = System.Drawing.Color.Transparent;
            this.aspectRatioCorrectionPanel.Controls.Add(this.aspectRatioCorrectionCheckBox);
            this.aspectRatioCorrectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.aspectRatioCorrectionPanel.Location = new System.Drawing.Point(0, 145);
            this.aspectRatioCorrectionPanel.Name = "aspectRatioCorrectionPanel";
            this.aspectRatioCorrectionPanel.Size = new System.Drawing.Size(431, 40);
            this.aspectRatioCorrectionPanel.TabIndex = 28;
            this.aspectRatioCorrectionPanel.Tag = "AspectRatioCorrection";
            // 
            // aspectRatioCorrectionCheckBox
            // 
            this.aspectRatioCorrectionCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.aspectRatioCorrectionCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aspectRatioCorrectionCheckBox.Location = new System.Drawing.Point(238, 14);
            this.aspectRatioCorrectionCheckBox.Name = "aspectRatioCorrectionCheckBox";
            this.aspectRatioCorrectionCheckBox.Size = new System.Drawing.Size(178, 19);
            this.aspectRatioCorrectionCheckBox.TabIndex = 0;
            this.aspectRatioCorrectionCheckBox.Text = "Enable";
            this.aspectRatioCorrectionCheckBox.UseVisualStyleBackColor = true;
            // 
            // audioPanel
            // 
            this.audioPanel.BackColor = System.Drawing.Color.Transparent;
            this.audioPanel.Controls.Add(this.audioPortTextBox);
            this.audioPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.audioPanel.Location = new System.Drawing.Point(0, 105);
            this.audioPanel.Name = "audioPanel";
            this.audioPanel.Size = new System.Drawing.Size(431, 40);
            this.audioPanel.TabIndex = 27;
            this.audioPanel.Tag = "AudioOutPort";
            // 
            // audioPortTextBox
            // 
            this.audioPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.audioPortTextBox.Location = new System.Drawing.Point(235, 8);
            this.audioPortTextBox.MaxLength = 5;
            this.audioPortTextBox.Name = "audioPortTextBox";
            this.audioPortTextBox.ShortcutsEnabled = false;
            this.audioPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.audioPortTextBox.TabIndex = 2;
            // 
            // httpPanel
            // 
            this.httpPanel.BackColor = System.Drawing.Color.Transparent;
            this.httpPanel.Controls.Add(this.httpPortTextBox);
            this.httpPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.httpPanel.Location = new System.Drawing.Point(0, 65);
            this.httpPanel.Name = "httpPanel";
            this.httpPanel.Size = new System.Drawing.Size(431, 40);
            this.httpPanel.TabIndex = 15;
            this.httpPanel.Tag = "HTTPPort";
            // 
            // httpPortTextBox
            // 
            this.httpPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.httpPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.httpPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.httpPortTextBox.Location = new System.Drawing.Point(235, 8);
            this.httpPortTextBox.MaxLength = 5;
            this.httpPortTextBox.Name = "httpPortTextBox";
            this.httpPortTextBox.ShortcutsEnabled = false;
            this.httpPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.httpPortTextBox.TabIndex = 2;
            // 
            // networkAddressPanel
            // 
            this.networkAddressPanel.BackColor = System.Drawing.Color.Transparent;
            this.networkAddressPanel.Controls.Add(this.ipAddressTextBox);
            this.networkAddressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.networkAddressPanel.Location = new System.Drawing.Point(0, 25);
            this.networkAddressPanel.Name = "networkAddressPanel";
            this.networkAddressPanel.Size = new System.Drawing.Size(431, 40);
            this.networkAddressPanel.TabIndex = 8;
            this.networkAddressPanel.Tag = "NetworkAddress";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressTextBox.Location = new System.Drawing.Point(235, 8);
            this.ipAddressTextBox.MaxLength = 80;
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(181, 21);
            this.ipAddressTextBox.TabIndex = 2;
            // 
            // mountTypePanel
            // 
            this.mountTypePanel.BackColor = System.Drawing.Color.Transparent;
            this.mountTypePanel.Controls.Add(this.mountTypeComboBox);
            this.mountTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mountTypePanel.Location = new System.Drawing.Point(0, 505);
            this.mountTypePanel.Name = "mountTypePanel";
            this.mountTypePanel.Size = new System.Drawing.Size(431, 40);
            this.mountTypePanel.TabIndex = 34;
            this.mountTypePanel.Tag = "DeviceMountType";
            // 
            // mountTypeComboBox
            // 
            this.mountTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mountTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mountTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mountTypeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mountTypeComboBox.FormattingEnabled = true;
            this.mountTypeComboBox.IntegralHeight = false;
            this.mountTypeComboBox.Location = new System.Drawing.Point(235, 8);
            this.mountTypeComboBox.MaxDropDownItems = 20;
            this.mountTypeComboBox.Name = "mountTypeComboBox";
            this.mountTypeComboBox.Size = new System.Drawing.Size(181, 23);
            this.mountTypeComboBox.TabIndex = 2;
            // 
            // ConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.remoteRecoveryPanel);
            this.Controls.Add(this.uriPanel);
            this.Controls.Add(this.mountTypePanel);
            this.Controls.Add(this.dewarpTypePanel);
            this.Controls.Add(this.powerFrequencyPanel);
            this.Controls.Add(this.sensorModePanel);
            this.Controls.Add(this.tvStandardPanel);
            this.Controls.Add(this.recordProfilePanel);
            this.Controls.Add(this.liveProfilePanel);
            this.Controls.Add(this.modePanel);
            this.Controls.Add(this.aspectRatioPanel);
            this.Controls.Add(this.aspectRatioCorrectionPanel);
            this.Controls.Add(this.audioPanel);
            this.Controls.Add(this.httpPanel);
            this.Controls.Add(this.networkAddressPanel);
            this.Name = "ConnectionControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(431, 641);
            this.remoteRecoveryPanel.ResumeLayout(false);
            this.uriPanel.ResumeLayout(false);
            this.uriPanel.PerformLayout();
            this.dewarpTypePanel.ResumeLayout(false);
            this.powerFrequencyPanel.ResumeLayout(false);
            this.sensorModePanel.ResumeLayout(false);
            this.tvStandardPanel.ResumeLayout(false);
            this.recordProfilePanel.ResumeLayout(false);
            this.liveProfilePanel.ResumeLayout(false);
            this.modePanel.ResumeLayout(false);
            this.aspectRatioPanel.ResumeLayout(false);
            this.aspectRatioCorrectionPanel.ResumeLayout(false);
            this.audioPanel.ResumeLayout(false);
            this.audioPanel.PerformLayout();
            this.httpPanel.ResumeLayout(false);
            this.httpPanel.PerformLayout();
            this.networkAddressPanel.ResumeLayout(false);
            this.networkAddressPanel.PerformLayout();
            this.mountTypePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel modePanel;
        private System.Windows.Forms.ComboBox modeComboBox;
        private PanelBase.DoubleBufferPanel networkAddressPanel;
        private PanelBase.DoubleBufferPanel liveProfilePanel;
        private System.Windows.Forms.ComboBox streamComboBox;
        private PanelBase.DoubleBufferPanel httpPanel;
        private PanelBase.DoubleBufferPanel powerFrequencyPanel;
        private System.Windows.Forms.ComboBox powerFrequencyComboBox;
        private PanelBase.DoubleBufferPanel sensorModePanel;
        private System.Windows.Forms.ComboBox sensorModeComboBox;
        private PanelBase.DoubleBufferPanel tvStandardPanel;
        private System.Windows.Forms.ComboBox tvStandardComboBox;
        private PanelBase.DoubleBufferPanel audioPanel;
        private PanelBase.DoubleBufferPanel aspectRatioCorrectionPanel;
        private System.Windows.Forms.CheckBox aspectRatioCorrectionCheckBox;
        private PanelBase.DoubleBufferPanel aspectRatioPanel;
        private System.Windows.Forms.ComboBox aspectRatioComboBox;
        private PanelBase.DoubleBufferPanel recordProfilePanel;
        private System.Windows.Forms.ComboBox recordStreamComboBox;
        private PanelBase.DoubleBufferPanel uriPanel;
        private PanelBase.DoubleBufferPanel dewarpTypePanel;
        private System.Windows.Forms.ComboBox dewarpTypeComboBox;
        private System.Windows.Forms.TextBox ipAddressTextBox;
        private PanelBase.HotKeyTextBox httpPortTextBox;
        private PanelBase.HotKeyTextBox audioPortTextBox;
        private PanelBase.HotKeyTextBox uriTextBox;
        private PanelBase.DoubleBufferPanel remoteRecoveryPanel;
        private System.Windows.Forms.CheckBox RemoteRecoveryCheckBox;
        private PanelBase.DoubleBufferPanel mountTypePanel;
        private System.Windows.Forms.ComboBox mountTypeComboBox;
    }
}
