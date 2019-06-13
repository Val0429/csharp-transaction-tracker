using PanelBase;

namespace SetupServer
{
    sealed partial class RestoreControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.warningLabel = new System.Windows.Forms.Label();
            this.licensePanel = new PanelBase.DoubleBufferPanel();
            this.licenseCheckBox = new System.Windows.Forms.CheckBox();
            this.licenseLabelPanel = new PanelBase.DoubleBufferPanel();
            this._sisWarningLabel = new System.Windows.Forms.Label();
            this._eventHandlerControl = new PanelBase.RowControl();
            this._eventHandlerCheckBox = new System.Windows.Forms.CheckBox();
            this._cardPanel = new PanelBase.RowControl();
            this._cardCheckBox = new System.Windows.Forms.CheckBox();
            this._doorPanel = new PanelBase.RowControl();
            this._doorCheckBox = new System.Windows.Forms.CheckBox();
            this._lprDevicePanel = new PanelBase.RowControl();
            this._lprDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this._licensePlateOwnerPanel = new PanelBase.DoubleBufferPanel();
            this._licensePlateOwnerCheckBox = new System.Windows.Forms.CheckBox();
            this.serverPanel = new PanelBase.DoubleBufferPanel();
            this.serverCheckBox = new System.Windows.Forms.CheckBox();
            this.nvrPanel = new PanelBase.DoubleBufferPanel();
            this.nvrCheckBox = new System.Windows.Forms.CheckBox();
            this.storagePanel = new PanelBase.DoubleBufferPanel();
            this.storageCheckBox = new System.Windows.Forms.CheckBox();
            this.emapPanel = new PanelBase.DoubleBufferPanel();
            this.emapCheckBox = new System.Windows.Forms.CheckBox();
            this.generalPanel = new PanelBase.DoubleBufferPanel();
            this.generalCheckBox = new System.Windows.Forms.CheckBox();
            this.userPanel = new PanelBase.DoubleBufferPanel();
            this.userCheckBox = new System.Windows.Forms.CheckBox();
            this.devicePanel = new PanelBase.DoubleBufferPanel();
            this.deviceCheckBox = new System.Windows.Forms.CheckBox();
            this.filePanel = new PanelBase.DoubleBufferPanel();
            this.browserButton = new System.Windows.Forms.Button();
            this.containerPanel.SuspendLayout();
            this.licensePanel.SuspendLayout();
            this._eventHandlerControl.SuspendLayout();
            this._cardPanel.SuspendLayout();
            this._doorPanel.SuspendLayout();
            this._lprDevicePanel.SuspendLayout();
            this._licensePlateOwnerPanel.SuspendLayout();
            this.serverPanel.SuspendLayout();
            this.nvrPanel.SuspendLayout();
            this.storagePanel.SuspendLayout();
            this.emapPanel.SuspendLayout();
            this.generalPanel.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.devicePanel.SuspendLayout();
            this.filePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(780, 15);
            this.label1.TabIndex = 9;
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.warningLabel);
            this.containerPanel.Controls.Add(this.licensePanel);
            this.containerPanel.Controls.Add(this.licenseLabelPanel);
            this.containerPanel.Controls.Add(this._sisWarningLabel);
            this.containerPanel.Controls.Add(this._cardPanel);
            this.containerPanel.Controls.Add(this._eventHandlerControl);
            this.containerPanel.Controls.Add(this._doorPanel);
            this.containerPanel.Controls.Add(this._lprDevicePanel);
            this.containerPanel.Controls.Add(this._licensePlateOwnerPanel);
            this.containerPanel.Controls.Add(this.serverPanel);
            this.containerPanel.Controls.Add(this.nvrPanel);
            this.containerPanel.Controls.Add(this.storagePanel);
            this.containerPanel.Controls.Add(this.emapPanel);
            this.containerPanel.Controls.Add(this.generalPanel);
            this.containerPanel.Controls.Add(this.userPanel);
            this.containerPanel.Controls.Add(this.devicePanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 73);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(780, 536);
            this.containerPanel.TabIndex = 18;
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.Transparent;
            this.warningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.warningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(0, 574);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(780, 20);
            this.warningLabel.TabIndex = 28;
            this.warningLabel.Text = "Don’t restore this license to a different NVR Server. Improper license restoratio" +
    "n will make the NVR Server registration fail.";
            this.warningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.warningLabel.Visible = false;
            // 
            // licensePanel
            // 
            this.licensePanel.BackColor = System.Drawing.Color.Transparent;
            this.licensePanel.Controls.Add(this.licenseCheckBox);
            this.licensePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.licensePanel.Location = new System.Drawing.Point(0, 534);
            this.licensePanel.Name = "licensePanel";
            this.licensePanel.Size = new System.Drawing.Size(780, 40);
            this.licensePanel.TabIndex = 27;
            this.licensePanel.Tag = "LicenseSetting";
            // 
            // licenseCheckBox
            // 
            this.licenseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseCheckBox.AutoSize = true;
            this.licenseCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.licenseCheckBox.Location = new System.Drawing.Point(700, 11);
            this.licenseCheckBox.Name = "licenseCheckBox";
            this.licenseCheckBox.Size = new System.Drawing.Size(66, 19);
            this.licenseCheckBox.TabIndex = 2;
            this.licenseCheckBox.Text = "Include";
            this.licenseCheckBox.UseVisualStyleBackColor = true;
            this.licenseCheckBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LicenseCheckBoxMouseClick);
            // 
            // licenseLabelPanel
            // 
            this.licenseLabelPanel.BackColor = System.Drawing.Color.Transparent;
            this.licenseLabelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.licenseLabelPanel.Location = new System.Drawing.Point(0, 500);
            this.licenseLabelPanel.Name = "licenseLabelPanel";
            this.licenseLabelPanel.Size = new System.Drawing.Size(780, 34);
            this.licenseLabelPanel.TabIndex = 26;
            this.licenseLabelPanel.Tag = "";
            // 
            // _sisWarningLabel
            // 
            this._sisWarningLabel.BackColor = System.Drawing.Color.Transparent;
            this._sisWarningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._sisWarningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._sisWarningLabel.ForeColor = System.Drawing.Color.Red;
            this._sisWarningLabel.Location = new System.Drawing.Point(0, 480);
            this._sisWarningLabel.Name = "_sisWarningLabel";
            this._sisWarningLabel.Size = new System.Drawing.Size(780, 20);
            this._sisWarningLabel.TabIndex = 33;
            this._sisWarningLabel.Text = "When you restore LPR Device setting, please also check the device setting, NVR se" +
    "tting and LPR device setting; otherwise restore it will fail.";
            this._sisWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._sisWarningLabel.Visible = false;
            // 
            // _eventHandlerControl
            // 
            this._eventHandlerControl.BackColor = System.Drawing.Color.Transparent;
            this._eventHandlerControl.Controls.Add(this._eventHandlerCheckBox);
            this._eventHandlerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this._eventHandlerControl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._eventHandlerControl.HeaderStyle = PanelBase.TitleStyle.Normal;
            this._eventHandlerControl.IsSelected = false;
            this._eventHandlerControl.Location = new System.Drawing.Point(0, 400);
            this._eventHandlerControl.Name = "_eventHandlerControl";
            this._eventHandlerControl.RightText = null;
            this._eventHandlerControl.Size = new System.Drawing.Size(780, 40);
            this._eventHandlerControl.TabIndex = 34;
            this._eventHandlerControl.Tag = "";
            this._eventHandlerControl.Text = "Event Handler Setting";
            this._eventHandlerControl.Visible = false;
            // 
            // _eventHandlerCheckBox
            // 
            this._eventHandlerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._eventHandlerCheckBox.AutoSize = true;
            this._eventHandlerCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._eventHandlerCheckBox.Location = new System.Drawing.Point(700, 11);
            this._eventHandlerCheckBox.Name = "_eventHandlerCheckBox";
            this._eventHandlerCheckBox.Size = new System.Drawing.Size(66, 19);
            this._eventHandlerCheckBox.TabIndex = 2;
            this._eventHandlerCheckBox.Text = "Include";
            this._eventHandlerCheckBox.UseVisualStyleBackColor = true;
            // 
            // _cardPanel
            // 
            this._cardPanel.BackColor = System.Drawing.Color.Transparent;
            this._cardPanel.Controls.Add(this._cardCheckBox);
            this._cardPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._cardPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cardPanel.HeaderStyle = PanelBase.TitleStyle.Normal;
            this._cardPanel.IsSelected = false;
            this._cardPanel.Location = new System.Drawing.Point(0, 440);
            this._cardPanel.Name = "_cardPanel";
            this._cardPanel.RightText = null;
            this._cardPanel.Size = new System.Drawing.Size(780, 40);
            this._cardPanel.TabIndex = 31;
            this._cardPanel.Tag = "CardSetting";
            this._cardPanel.Text = "Card Setting";
            this._cardPanel.Visible = false;
            // 
            // _cardCheckBox
            // 
            this._cardCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cardCheckBox.AutoSize = true;
            this._cardCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cardCheckBox.Location = new System.Drawing.Point(700, 11);
            this._cardCheckBox.Name = "_cardCheckBox";
            this._cardCheckBox.Size = new System.Drawing.Size(66, 19);
            this._cardCheckBox.TabIndex = 2;
            this._cardCheckBox.Text = "Include";
            this._cardCheckBox.UseVisualStyleBackColor = true;
            // 
            // _doorPanel
            // 
            this._doorPanel.BackColor = System.Drawing.Color.Transparent;
            this._doorPanel.Controls.Add(this._doorCheckBox);
            this._doorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._doorPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._doorPanel.HeaderStyle = PanelBase.TitleStyle.Normal;
            this._doorPanel.IsSelected = false;
            this._doorPanel.Location = new System.Drawing.Point(0, 360);
            this._doorPanel.Name = "_doorPanel";
            this._doorPanel.RightText = null;
            this._doorPanel.Size = new System.Drawing.Size(780, 40);
            this._doorPanel.TabIndex = 32;
            this._doorPanel.Tag = "DoorSetting";
            this._doorPanel.Text = "Door Setting";
            this._doorPanel.Visible = false;
            // 
            // _doorCheckBox
            // 
            this._doorCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._doorCheckBox.AutoSize = true;
            this._doorCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._doorCheckBox.Location = new System.Drawing.Point(700, 11);
            this._doorCheckBox.Name = "_doorCheckBox";
            this._doorCheckBox.Size = new System.Drawing.Size(66, 19);
            this._doorCheckBox.TabIndex = 2;
            this._doorCheckBox.Text = "Include";
            this._doorCheckBox.UseVisualStyleBackColor = true;
            this._doorCheckBox.CheckedChanged += new System.EventHandler(this.DoorCheckBoxCheckedChanged);
            // 
            // _lprDevicePanel
            // 
            this._lprDevicePanel.BackColor = System.Drawing.Color.Transparent;
            this._lprDevicePanel.Controls.Add(this._lprDeviceCheckBox);
            this._lprDevicePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._lprDevicePanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lprDevicePanel.HeaderStyle = PanelBase.TitleStyle.Normal;
            this._lprDevicePanel.IsSelected = false;
            this._lprDevicePanel.Location = new System.Drawing.Point(0, 320);
            this._lprDevicePanel.Name = "_lprDevicePanel";
            this._lprDevicePanel.RightText = null;
            this._lprDevicePanel.Size = new System.Drawing.Size(780, 40);
            this._lprDevicePanel.TabIndex = 29;
            this._lprDevicePanel.Tag = "LPRDeviceSetting";
            this._lprDevicePanel.Text = "LPR Device Setting";
            this._lprDevicePanel.Visible = false;
            // 
            // _lprDeviceCheckBox
            // 
            this._lprDeviceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lprDeviceCheckBox.AutoSize = true;
            this._lprDeviceCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lprDeviceCheckBox.Location = new System.Drawing.Point(700, 11);
            this._lprDeviceCheckBox.Name = "_lprDeviceCheckBox";
            this._lprDeviceCheckBox.Size = new System.Drawing.Size(66, 19);
            this._lprDeviceCheckBox.TabIndex = 2;
            this._lprDeviceCheckBox.Text = "Include";
            this._lprDeviceCheckBox.UseVisualStyleBackColor = true;
            this._lprDeviceCheckBox.CheckedChanged += new System.EventHandler(this.LprDeviceCheckBox_CheckedChanged);
            // 
            // _licensePlateOwnerPanel
            // 
            this._licensePlateOwnerPanel.BackColor = System.Drawing.Color.Transparent;
            this._licensePlateOwnerPanel.Controls.Add(this._licensePlateOwnerCheckBox);
            this._licensePlateOwnerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._licensePlateOwnerPanel.Location = new System.Drawing.Point(0, 280);
            this._licensePlateOwnerPanel.Name = "_licensePlateOwnerPanel";
            this._licensePlateOwnerPanel.Size = new System.Drawing.Size(780, 40);
            this._licensePlateOwnerPanel.TabIndex = 30;
            this._licensePlateOwnerPanel.Tag = "LPRGroupSetting";
            this._licensePlateOwnerPanel.Visible = false;
            // 
            // _licensePlateOwnerCheckBox
            // 
            this._licensePlateOwnerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._licensePlateOwnerCheckBox.AutoSize = true;
            this._licensePlateOwnerCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._licensePlateOwnerCheckBox.Location = new System.Drawing.Point(700, 11);
            this._licensePlateOwnerCheckBox.Name = "_licensePlateOwnerCheckBox";
            this._licensePlateOwnerCheckBox.Size = new System.Drawing.Size(66, 19);
            this._licensePlateOwnerCheckBox.TabIndex = 2;
            this._licensePlateOwnerCheckBox.Text = "Include";
            this._licensePlateOwnerCheckBox.UseVisualStyleBackColor = true;
            // 
            // serverPanel
            // 
            this.serverPanel.BackColor = System.Drawing.Color.Transparent;
            this.serverPanel.Controls.Add(this.serverCheckBox);
            this.serverPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverPanel.Location = new System.Drawing.Point(0, 240);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(780, 40);
            this.serverPanel.TabIndex = 25;
            this.serverPanel.Tag = "ServerSetting";
            // 
            // serverCheckBox
            // 
            this.serverCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serverCheckBox.AutoSize = true;
            this.serverCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverCheckBox.Location = new System.Drawing.Point(700, 11);
            this.serverCheckBox.Name = "serverCheckBox";
            this.serverCheckBox.Size = new System.Drawing.Size(66, 19);
            this.serverCheckBox.TabIndex = 2;
            this.serverCheckBox.Text = "Include";
            this.serverCheckBox.UseVisualStyleBackColor = true;
            // 
            // nvrPanel
            // 
            this.nvrPanel.BackColor = System.Drawing.Color.Transparent;
            this.nvrPanel.Controls.Add(this.nvrCheckBox);
            this.nvrPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nvrPanel.Location = new System.Drawing.Point(0, 200);
            this.nvrPanel.Name = "nvrPanel";
            this.nvrPanel.Size = new System.Drawing.Size(780, 40);
            this.nvrPanel.TabIndex = 24;
            this.nvrPanel.Tag = "NVRSetting";
            // 
            // nvrCheckBox
            // 
            this.nvrCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nvrCheckBox.AutoSize = true;
            this.nvrCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nvrCheckBox.Location = new System.Drawing.Point(700, 11);
            this.nvrCheckBox.Name = "nvrCheckBox";
            this.nvrCheckBox.Size = new System.Drawing.Size(66, 19);
            this.nvrCheckBox.TabIndex = 2;
            this.nvrCheckBox.Text = "Include";
            this.nvrCheckBox.UseVisualStyleBackColor = true;
            // 
            // storagePanel
            // 
            this.storagePanel.BackColor = System.Drawing.Color.Transparent;
            this.storagePanel.Controls.Add(this.storageCheckBox);
            this.storagePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.storagePanel.Location = new System.Drawing.Point(0, 160);
            this.storagePanel.Name = "storagePanel";
            this.storagePanel.Size = new System.Drawing.Size(780, 40);
            this.storagePanel.TabIndex = 23;
            this.storagePanel.Tag = "StorageSetting";
            // 
            // storageCheckBox
            // 
            this.storageCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.storageCheckBox.AutoSize = true;
            this.storageCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storageCheckBox.Location = new System.Drawing.Point(700, 11);
            this.storageCheckBox.Name = "storageCheckBox";
            this.storageCheckBox.Size = new System.Drawing.Size(66, 19);
            this.storageCheckBox.TabIndex = 2;
            this.storageCheckBox.Text = "Include";
            this.storageCheckBox.UseVisualStyleBackColor = true;
            // 
            // emapPanel
            // 
            this.emapPanel.BackColor = System.Drawing.Color.Transparent;
            this.emapPanel.Controls.Add(this.emapCheckBox);
            this.emapPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.emapPanel.Location = new System.Drawing.Point(0, 120);
            this.emapPanel.Name = "emapPanel";
            this.emapPanel.Size = new System.Drawing.Size(780, 40);
            this.emapPanel.TabIndex = 21;
            this.emapPanel.Tag = "EMapSetting";
            // 
            // emapCheckBox
            // 
            this.emapCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emapCheckBox.AutoSize = true;
            this.emapCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emapCheckBox.Location = new System.Drawing.Point(700, 11);
            this.emapCheckBox.Name = "emapCheckBox";
            this.emapCheckBox.Size = new System.Drawing.Size(66, 19);
            this.emapCheckBox.TabIndex = 2;
            this.emapCheckBox.Text = "Include";
            this.emapCheckBox.UseVisualStyleBackColor = true;
            // 
            // generalPanel
            // 
            this.generalPanel.BackColor = System.Drawing.Color.Transparent;
            this.generalPanel.Controls.Add(this.generalCheckBox);
            this.generalPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.generalPanel.Location = new System.Drawing.Point(0, 80);
            this.generalPanel.Name = "generalPanel";
            this.generalPanel.Size = new System.Drawing.Size(780, 40);
            this.generalPanel.TabIndex = 20;
            this.generalPanel.Tag = "GeneralSetting";
            // 
            // generalCheckBox
            // 
            this.generalCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generalCheckBox.AutoSize = true;
            this.generalCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generalCheckBox.Location = new System.Drawing.Point(700, 11);
            this.generalCheckBox.Name = "generalCheckBox";
            this.generalCheckBox.Size = new System.Drawing.Size(66, 19);
            this.generalCheckBox.TabIndex = 2;
            this.generalCheckBox.Text = "Include";
            this.generalCheckBox.UseVisualStyleBackColor = true;
            // 
            // userPanel
            // 
            this.userPanel.BackColor = System.Drawing.Color.Transparent;
            this.userPanel.Controls.Add(this.userCheckBox);
            this.userPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userPanel.Location = new System.Drawing.Point(0, 40);
            this.userPanel.Name = "userPanel";
            this.userPanel.Size = new System.Drawing.Size(780, 40);
            this.userPanel.TabIndex = 19;
            this.userPanel.Tag = "UserSetting";
            // 
            // userCheckBox
            // 
            this.userCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userCheckBox.AutoSize = true;
            this.userCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userCheckBox.Location = new System.Drawing.Point(700, 11);
            this.userCheckBox.Name = "userCheckBox";
            this.userCheckBox.Size = new System.Drawing.Size(66, 19);
            this.userCheckBox.TabIndex = 2;
            this.userCheckBox.Text = "Include";
            this.userCheckBox.UseVisualStyleBackColor = true;
            // 
            // devicePanel
            // 
            this.devicePanel.BackColor = System.Drawing.Color.Transparent;
            this.devicePanel.Controls.Add(this.deviceCheckBox);
            this.devicePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.devicePanel.Location = new System.Drawing.Point(0, 0);
            this.devicePanel.Name = "devicePanel";
            this.devicePanel.Size = new System.Drawing.Size(780, 40);
            this.devicePanel.TabIndex = 18;
            this.devicePanel.Tag = "DeviceSetting";
            // 
            // deviceCheckBox
            // 
            this.deviceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceCheckBox.AutoSize = true;
            this.deviceCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceCheckBox.Location = new System.Drawing.Point(700, 11);
            this.deviceCheckBox.Name = "deviceCheckBox";
            this.deviceCheckBox.Size = new System.Drawing.Size(66, 19);
            this.deviceCheckBox.TabIndex = 2;
            this.deviceCheckBox.Text = "Include";
            this.deviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // filePanel
            // 
            this.filePanel.BackColor = System.Drawing.Color.Transparent;
            this.filePanel.Controls.Add(this.browserButton);
            this.filePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filePanel.Location = new System.Drawing.Point(12, 18);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(780, 40);
            this.filePanel.TabIndex = 7;
            this.filePanel.Tag = "SettingFile";
            // 
            // browserButton
            // 
            this.browserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browserButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browserButton.Location = new System.Drawing.Point(691, 7);
            this.browserButton.Name = "browserButton";
            this.browserButton.Size = new System.Drawing.Size(75, 27);
            this.browserButton.TabIndex = 0;
            this.browserButton.Text = "Broswer";
            this.browserButton.UseVisualStyleBackColor = true;
            this.browserButton.Click += new System.EventHandler(this.BrowserButtonClick);
            // 
            // RestoreControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filePanel);
            this.DoubleBuffered = true;
            this.Name = "RestoreControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(804, 627);
            this.VisibleChanged += new System.EventHandler(this.RestoreControl_VisibleChanged);
            this.containerPanel.ResumeLayout(false);
            this.licensePanel.ResumeLayout(false);
            this.licensePanel.PerformLayout();
            this._eventHandlerControl.ResumeLayout(false);
            this._eventHandlerControl.PerformLayout();
            this._cardPanel.ResumeLayout(false);
            this._cardPanel.PerformLayout();
            this._doorPanel.ResumeLayout(false);
            this._doorPanel.PerformLayout();
            this._lprDevicePanel.ResumeLayout(false);
            this._lprDevicePanel.PerformLayout();
            this._licensePlateOwnerPanel.ResumeLayout(false);
            this._licensePlateOwnerPanel.PerformLayout();
            this.serverPanel.ResumeLayout(false);
            this.serverPanel.PerformLayout();
            this.nvrPanel.ResumeLayout(false);
            this.nvrPanel.PerformLayout();
            this.storagePanel.ResumeLayout(false);
            this.storagePanel.PerformLayout();
            this.emapPanel.ResumeLayout(false);
            this.emapPanel.PerformLayout();
            this.generalPanel.ResumeLayout(false);
            this.generalPanel.PerformLayout();
            this.userPanel.ResumeLayout(false);
            this.userPanel.PerformLayout();
            this.devicePanel.ResumeLayout(false);
            this.devicePanel.PerformLayout();
            this.filePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel filePanel;
        private System.Windows.Forms.Button browserButton;
        private System.Windows.Forms.Label label1;
        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel serverPanel;
        private System.Windows.Forms.CheckBox serverCheckBox;
        private PanelBase.DoubleBufferPanel nvrPanel;
        private System.Windows.Forms.CheckBox nvrCheckBox;
        private PanelBase.DoubleBufferPanel storagePanel;
        private System.Windows.Forms.CheckBox storageCheckBox;
        private PanelBase.DoubleBufferPanel emapPanel;
        private System.Windows.Forms.CheckBox emapCheckBox;
        private PanelBase.DoubleBufferPanel generalPanel;
        private System.Windows.Forms.CheckBox generalCheckBox;
        private PanelBase.DoubleBufferPanel userPanel;
        private System.Windows.Forms.CheckBox userCheckBox;
        private PanelBase.DoubleBufferPanel devicePanel;
        private System.Windows.Forms.CheckBox deviceCheckBox;
        private PanelBase.DoubleBufferPanel licenseLabelPanel;
        private PanelBase.DoubleBufferPanel licensePanel;
        private System.Windows.Forms.CheckBox licenseCheckBox;
        private System.Windows.Forms.Label warningLabel;
        private PanelBase.RowControl _lprDevicePanel;
        private System.Windows.Forms.CheckBox _lprDeviceCheckBox;
        private System.Windows.Forms.Label _sisWarningLabel;
        private PanelBase.DoubleBufferPanel _licensePlateOwnerPanel;
        private System.Windows.Forms.CheckBox _licensePlateOwnerCheckBox;
        private PanelBase.RowControl _cardPanel;
        private System.Windows.Forms.CheckBox _cardCheckBox;
        private PanelBase.RowControl _doorPanel;
        private System.Windows.Forms.CheckBox _doorCheckBox;
        private RowControl _eventHandlerControl;
        private System.Windows.Forms.CheckBox _eventHandlerCheckBox;
    }
}
