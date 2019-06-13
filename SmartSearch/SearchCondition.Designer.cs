namespace SmartSearch
{
    sealed partial class SearchCondition
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
            this.searchButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.conditionPanel = new PanelBase.DoubleBufferPanel();
            this.userDefineCheckBox = new System.Windows.Forms.CheckBox();
            this.temperingDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.audioDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.objectCountingOutCheckBox = new System.Windows.Forms.CheckBox();
            this.objectCountingInCheckBox = new System.Windows.Forms.CheckBox();
            this.loiteringDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.intrusionDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.crossLineCheckBox = new System.Windows.Forms.CheckBox();
            this.nvrComboBox = new System.Windows.Forms.ComboBox();
            this.nvrLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.periodTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.panicCheckBox = new System.Windows.Forms.CheckBox();
            this.videoRecoveryCheckBox = new System.Windows.Forms.CheckBox();
            this.videoLossCheckBox = new System.Windows.Forms.CheckBox();
            this.networkRecoveryCheckBox = new System.Windows.Forms.CheckBox();
            this.networkLossCheckBox = new System.Windows.Forms.CheckBox();
            this.manualRecordCheckBox = new System.Windows.Forms.CheckBox();
            this.doCheckBox = new System.Windows.Forms.CheckBox();
            this.setupIVSMotionPictureBox = new System.Windows.Forms.PictureBox();
            this.ivsLabel = new System.Windows.Forms.Label();
            this.ivsMotionCheckBox = new System.Windows.Forms.CheckBox();
            this.periodLabel = new System.Windows.Forms.Label();
            this.periodPanel = new PanelBase.DoubleBufferPanel();
            this.periodPointPictureBox = new System.Windows.Forms.PictureBox();
            this.minusPictureBox = new System.Windows.Forms.PictureBox();
            this.plusPictureBox = new System.Windows.Forms.PictureBox();
            this.eventLabel = new System.Windows.Forms.Label();
            this.diCheckBox = new System.Windows.Forms.CheckBox();
            this.motionCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonPanel.SuspendLayout();
            this.conditionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setupIVSMotionPictureBox)).BeginInit();
            this.periodPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.periodPointPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plusPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchButton.BackColor = System.Drawing.Color.Transparent;
            this.searchButton.BackgroundImage = global::SmartSearch.Properties.Resources.searchButton;
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.searchButton.Location = new System.Drawing.Point(35, 5);
            this.searchButton.Margin = new System.Windows.Forms.Padding(0);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(150, 41);
            this.searchButton.TabIndex = 29;
            this.searchButton.TabStop = false;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.BackColor = System.Drawing.Color.Transparent;
            this.stopButton.BackgroundImage = global::SmartSearch.Properties.Resources.stopButotn;
            this.stopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopButton.Enabled = false;
            this.stopButton.FlatAppearance.BorderSize = 0;
            this.stopButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopButton.Location = new System.Drawing.Point(204, 5);
            this.stopButton.Margin = new System.Windows.Forms.Padding(0);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(150, 41);
            this.stopButton.TabIndex = 30;
            this.stopButton.TabStop = false;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Visible = false;
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.SystemColors.Window;
            this.buttonPanel.BackgroundImage = global::SmartSearch.Properties.Resources.toolBG;
            this.buttonPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPanel.Controls.Add(this.searchButton);
            this.buttonPanel.Controls.Add(this.stopButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.ForeColor = System.Drawing.Color.White;
            this.buttonPanel.Location = new System.Drawing.Point(0, 648);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(220, 52);
            this.buttonPanel.TabIndex = 3;
            // 
            // conditionPanel
            // 
            this.conditionPanel.AutoScroll = true;
            this.conditionPanel.BackColor = System.Drawing.Color.DimGray;
            this.conditionPanel.BackgroundImage = global::SmartSearch.Properties.Resources.controllerBG;
            this.conditionPanel.Controls.Add(this.userDefineCheckBox);
            this.conditionPanel.Controls.Add(this.temperingDetectionCheckBox);
            this.conditionPanel.Controls.Add(this.audioDetectionCheckBox);
            this.conditionPanel.Controls.Add(this.objectCountingOutCheckBox);
            this.conditionPanel.Controls.Add(this.objectCountingInCheckBox);
            this.conditionPanel.Controls.Add(this.loiteringDetectionCheckBox);
            this.conditionPanel.Controls.Add(this.intrusionDetectionCheckBox);
            this.conditionPanel.Controls.Add(this.crossLineCheckBox);
            this.conditionPanel.Controls.Add(this.nvrComboBox);
            this.conditionPanel.Controls.Add(this.nvrLabel);
            this.conditionPanel.Controls.Add(this.panel4);
            this.conditionPanel.Controls.Add(this.panel3);
            this.conditionPanel.Controls.Add(this.panel2);
            this.conditionPanel.Controls.Add(this.panel1);
            this.conditionPanel.Controls.Add(this.deviceComboBox);
            this.conditionPanel.Controls.Add(this.deviceLabel);
            this.conditionPanel.Controls.Add(this.endLabel);
            this.conditionPanel.Controls.Add(this.startLabel);
            this.conditionPanel.Controls.Add(this.endTimePicker);
            this.conditionPanel.Controls.Add(this.endDatePicker);
            this.conditionPanel.Controls.Add(this.startTimePicker);
            this.conditionPanel.Controls.Add(this.startDatePicker);
            this.conditionPanel.Controls.Add(this.periodTimeCheckBox);
            this.conditionPanel.Controls.Add(this.panicCheckBox);
            this.conditionPanel.Controls.Add(this.videoRecoveryCheckBox);
            this.conditionPanel.Controls.Add(this.videoLossCheckBox);
            this.conditionPanel.Controls.Add(this.networkRecoveryCheckBox);
            this.conditionPanel.Controls.Add(this.networkLossCheckBox);
            this.conditionPanel.Controls.Add(this.manualRecordCheckBox);
            this.conditionPanel.Controls.Add(this.doCheckBox);
            this.conditionPanel.Controls.Add(this.setupIVSMotionPictureBox);
            this.conditionPanel.Controls.Add(this.ivsLabel);
            this.conditionPanel.Controls.Add(this.ivsMotionCheckBox);
            this.conditionPanel.Controls.Add(this.periodLabel);
            this.conditionPanel.Controls.Add(this.periodPanel);
            this.conditionPanel.Controls.Add(this.eventLabel);
            this.conditionPanel.Controls.Add(this.diCheckBox);
            this.conditionPanel.Controls.Add(this.motionCheckBox);
            this.conditionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionPanel.Location = new System.Drawing.Point(0, 0);
            this.conditionPanel.Name = "conditionPanel";
            this.conditionPanel.Size = new System.Drawing.Size(220, 648);
            this.conditionPanel.TabIndex = 1;
            // 
            // userDefineCheckBox
            // 
            this.userDefineCheckBox.AutoSize = true;
            this.userDefineCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.userDefineCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userDefineCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.userDefineCheckBox.Image = global::SmartSearch.Properties.Resources.userdefine;
            this.userDefineCheckBox.Location = new System.Drawing.Point(17, 779);
            this.userDefineCheckBox.Name = "userDefineCheckBox";
            this.userDefineCheckBox.Size = new System.Drawing.Size(122, 21);
            this.userDefineCheckBox.TabIndex = 55;
            this.userDefineCheckBox.Text = "User Define";
            this.userDefineCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.userDefineCheckBox.UseVisualStyleBackColor = false;
            // 
            // temperingDetectionCheckBox
            // 
            this.temperingDetectionCheckBox.AutoSize = true;
            this.temperingDetectionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.temperingDetectionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.temperingDetectionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.temperingDetectionCheckBox.Image = global::SmartSearch.Properties.Resources.objectCountingOut;
            this.temperingDetectionCheckBox.Location = new System.Drawing.Point(17, 752);
            this.temperingDetectionCheckBox.Name = "temperingDetectionCheckBox";
            this.temperingDetectionCheckBox.Size = new System.Drawing.Size(160, 21);
            this.temperingDetectionCheckBox.TabIndex = 54;
            this.temperingDetectionCheckBox.Text = "Tamper Detection";
            this.temperingDetectionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.temperingDetectionCheckBox.UseVisualStyleBackColor = false;
            // 
            // audioDetectionCheckBox
            // 
            this.audioDetectionCheckBox.AutoSize = true;
            this.audioDetectionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.audioDetectionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioDetectionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.audioDetectionCheckBox.Image = global::SmartSearch.Properties.Resources.objectCountingOut;
            this.audioDetectionCheckBox.Location = new System.Drawing.Point(17, 726);
            this.audioDetectionCheckBox.Name = "audioDetectionCheckBox";
            this.audioDetectionCheckBox.Size = new System.Drawing.Size(143, 21);
            this.audioDetectionCheckBox.TabIndex = 53;
            this.audioDetectionCheckBox.Text = "Autio Detection";
            this.audioDetectionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.audioDetectionCheckBox.UseVisualStyleBackColor = false;
            // 
            // objectCountingOutCheckBox
            // 
            this.objectCountingOutCheckBox.AutoSize = true;
            this.objectCountingOutCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.objectCountingOutCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectCountingOutCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.objectCountingOutCheckBox.Image = global::SmartSearch.Properties.Resources.objectCountingOut;
            this.objectCountingOutCheckBox.Location = new System.Drawing.Point(17, 699);
            this.objectCountingOutCheckBox.Name = "objectCountingOutCheckBox";
            this.objectCountingOutCheckBox.Size = new System.Drawing.Size(178, 21);
            this.objectCountingOutCheckBox.TabIndex = 52;
            this.objectCountingOutCheckBox.Text = "Object Counting Out";
            this.objectCountingOutCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.objectCountingOutCheckBox.UseVisualStyleBackColor = false;
            // 
            // objectCountingInCheckBox
            // 
            this.objectCountingInCheckBox.AutoSize = true;
            this.objectCountingInCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.objectCountingInCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectCountingInCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.objectCountingInCheckBox.Image = global::SmartSearch.Properties.Resources.objectCountingIn;
            this.objectCountingInCheckBox.Location = new System.Drawing.Point(17, 672);
            this.objectCountingInCheckBox.Name = "objectCountingInCheckBox";
            this.objectCountingInCheckBox.Size = new System.Drawing.Size(165, 21);
            this.objectCountingInCheckBox.TabIndex = 51;
            this.objectCountingInCheckBox.Text = "Object Counting In";
            this.objectCountingInCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.objectCountingInCheckBox.UseVisualStyleBackColor = false;
            // 
            // loiteringDetectionCheckBox
            // 
            this.loiteringDetectionCheckBox.AutoSize = true;
            this.loiteringDetectionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.loiteringDetectionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loiteringDetectionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.loiteringDetectionCheckBox.Image = global::SmartSearch.Properties.Resources.loiteringDetection;
            this.loiteringDetectionCheckBox.Location = new System.Drawing.Point(17, 645);
            this.loiteringDetectionCheckBox.Name = "loiteringDetectionCheckBox";
            this.loiteringDetectionCheckBox.Size = new System.Drawing.Size(166, 21);
            this.loiteringDetectionCheckBox.TabIndex = 50;
            this.loiteringDetectionCheckBox.Text = "Loitering Detection";
            this.loiteringDetectionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.loiteringDetectionCheckBox.UseVisualStyleBackColor = false;
            // 
            // intrusionDetectionCheckBox
            // 
            this.intrusionDetectionCheckBox.AutoSize = true;
            this.intrusionDetectionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.intrusionDetectionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.intrusionDetectionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.intrusionDetectionCheckBox.Image = global::SmartSearch.Properties.Resources.intrusionDetection;
            this.intrusionDetectionCheckBox.Location = new System.Drawing.Point(17, 618);
            this.intrusionDetectionCheckBox.Name = "intrusionDetectionCheckBox";
            this.intrusionDetectionCheckBox.Size = new System.Drawing.Size(166, 21);
            this.intrusionDetectionCheckBox.TabIndex = 49;
            this.intrusionDetectionCheckBox.Text = "Intrusion Detection";
            this.intrusionDetectionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.intrusionDetectionCheckBox.UseVisualStyleBackColor = false;
            // 
            // crossLineCheckBox
            // 
            this.crossLineCheckBox.AutoSize = true;
            this.crossLineCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.crossLineCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crossLineCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.crossLineCheckBox.Image = global::SmartSearch.Properties.Resources.crossline;
            this.crossLineCheckBox.Location = new System.Drawing.Point(19, 591);
            this.crossLineCheckBox.Name = "crossLineCheckBox";
            this.crossLineCheckBox.Size = new System.Drawing.Size(116, 21);
            this.crossLineCheckBox.TabIndex = 48;
            this.crossLineCheckBox.Text = "Cross Line";
            this.crossLineCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.crossLineCheckBox.UseVisualStyleBackColor = false;
            // 
            // nvrComboBox
            // 
            this.nvrComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nvrComboBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nvrComboBox.FormattingEnabled = true;
            this.nvrComboBox.Location = new System.Drawing.Point(19, 35);
            this.nvrComboBox.Name = "nvrComboBox";
            this.nvrComboBox.Size = new System.Drawing.Size(181, 24);
            this.nvrComboBox.TabIndex = 47;
            // 
            // nvrLabel
            // 
            this.nvrLabel.BackColor = System.Drawing.Color.Transparent;
            this.nvrLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nvrLabel.ForeColor = System.Drawing.Color.White;
            this.nvrLabel.Location = new System.Drawing.Point(22, 10);
            this.nvrLabel.Name = "nvrLabel";
            this.nvrLabel.Size = new System.Drawing.Size(171, 22);
            this.nvrLabel.TabIndex = 46;
            this.nvrLabel.Text = "NVR";
            this.nvrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(47)))));
            this.panel4.Location = new System.Drawing.Point(10, 339);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(172, 1);
            this.panel4.TabIndex = 45;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(47)))));
            this.panel3.Location = new System.Drawing.Point(10, 285);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(172, 1);
            this.panel3.TabIndex = 44;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(47)))));
            this.panel2.Location = new System.Drawing.Point(10, 227);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(172, 1);
            this.panel2.TabIndex = 43;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(47)))));
            this.panel1.Location = new System.Drawing.Point(10, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 1);
            this.panel1.TabIndex = 42;
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceComboBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.Location = new System.Drawing.Point(19, 89);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(181, 24);
            this.deviceComboBox.TabIndex = 41;
            // 
            // deviceLabel
            // 
            this.deviceLabel.BackColor = System.Drawing.Color.Transparent;
            this.deviceLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceLabel.ForeColor = System.Drawing.Color.White;
            this.deviceLabel.Location = new System.Drawing.Point(22, 65);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(171, 22);
            this.deviceLabel.TabIndex = 40;
            this.deviceLabel.Text = "Device";
            this.deviceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // endLabel
            // 
            this.endLabel.BackColor = System.Drawing.Color.Transparent;
            this.endLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endLabel.ForeColor = System.Drawing.Color.White;
            this.endLabel.Location = new System.Drawing.Point(22, 172);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(171, 22);
            this.endLabel.TabIndex = 6;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startLabel
            // 
            this.startLabel.BackColor = System.Drawing.Color.Transparent;
            this.startLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.White;
            this.startLabel.Location = new System.Drawing.Point(22, 121);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(171, 22);
            this.startLabel.TabIndex = 8;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "HH:mm:ss";
            this.endTimePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(125, 195);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(75, 23);
            this.endTimePicker.TabIndex = 10;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "yyyy-MM-dd";
            this.endDatePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(19, 195);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(101, 23);
            this.endDatePicker.TabIndex = 9;
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm:ss";
            this.startTimePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(125, 144);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(75, 23);
            this.startTimePicker.TabIndex = 7;
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "yyyy-MM-dd";
            this.startDatePicker.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(19, 144);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(101, 23);
            this.startDatePicker.TabIndex = 5;
            // 
            // periodTimeCheckBox
            // 
            this.periodTimeCheckBox.AutoSize = true;
            this.periodTimeCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.periodTimeCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.periodTimeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.periodTimeCheckBox.Location = new System.Drawing.Point(19, 258);
            this.periodTimeCheckBox.Name = "periodTimeCheckBox";
            this.periodTimeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.periodTimeCheckBox.TabIndex = 39;
            this.periodTimeCheckBox.Tag = "Period";
            this.periodTimeCheckBox.UseVisualStyleBackColor = false;
            // 
            // panicCheckBox
            // 
            this.panicCheckBox.AutoSize = true;
            this.panicCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.panicCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panicCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.panicCheckBox.Image = global::SmartSearch.Properties.Resources.panic;
            this.panicCheckBox.Location = new System.Drawing.Point(19, 441);
            this.panicCheckBox.Name = "panicCheckBox";
            this.panicCheckBox.Size = new System.Drawing.Size(82, 21);
            this.panicCheckBox.TabIndex = 36;
            this.panicCheckBox.Text = "Panic";
            this.panicCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.panicCheckBox.UseVisualStyleBackColor = false;
            // 
            // videoRecoveryCheckBox
            // 
            this.videoRecoveryCheckBox.AutoSize = true;
            this.videoRecoveryCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.videoRecoveryCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoRecoveryCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.videoRecoveryCheckBox.Image = global::SmartSearch.Properties.Resources.videorecovery;
            this.videoRecoveryCheckBox.Location = new System.Drawing.Point(19, 466);
            this.videoRecoveryCheckBox.Name = "videoRecoveryCheckBox";
            this.videoRecoveryCheckBox.Size = new System.Drawing.Size(147, 21);
            this.videoRecoveryCheckBox.TabIndex = 35;
            this.videoRecoveryCheckBox.Text = "Video Recovery";
            this.videoRecoveryCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.videoRecoveryCheckBox.UseVisualStyleBackColor = false;
            // 
            // videoLossCheckBox
            // 
            this.videoLossCheckBox.AutoSize = true;
            this.videoLossCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.videoLossCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoLossCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.videoLossCheckBox.Image = global::SmartSearch.Properties.Resources.videoloss;
            this.videoLossCheckBox.Location = new System.Drawing.Point(19, 491);
            this.videoLossCheckBox.Name = "videoLossCheckBox";
            this.videoLossCheckBox.Size = new System.Drawing.Size(113, 21);
            this.videoLossCheckBox.TabIndex = 34;
            this.videoLossCheckBox.Text = "Video Lost";
            this.videoLossCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.videoLossCheckBox.UseVisualStyleBackColor = false;
            // 
            // networkRecoveryCheckBox
            // 
            this.networkRecoveryCheckBox.AutoSize = true;
            this.networkRecoveryCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.networkRecoveryCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.networkRecoveryCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.networkRecoveryCheckBox.Image = global::SmartSearch.Properties.Resources.networkrecovery;
            this.networkRecoveryCheckBox.Location = new System.Drawing.Point(19, 541);
            this.networkRecoveryCheckBox.Name = "networkRecoveryCheckBox";
            this.networkRecoveryCheckBox.Size = new System.Drawing.Size(164, 21);
            this.networkRecoveryCheckBox.TabIndex = 33;
            this.networkRecoveryCheckBox.Text = "Network Recovery";
            this.networkRecoveryCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.networkRecoveryCheckBox.UseVisualStyleBackColor = false;
            // 
            // networkLossCheckBox
            // 
            this.networkLossCheckBox.AutoSize = true;
            this.networkLossCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.networkLossCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.networkLossCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.networkLossCheckBox.Image = global::SmartSearch.Properties.Resources.networkloss;
            this.networkLossCheckBox.Location = new System.Drawing.Point(19, 566);
            this.networkLossCheckBox.Name = "networkLossCheckBox";
            this.networkLossCheckBox.Size = new System.Drawing.Size(130, 21);
            this.networkLossCheckBox.TabIndex = 32;
            this.networkLossCheckBox.Text = "Network Lost";
            this.networkLossCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.networkLossCheckBox.UseVisualStyleBackColor = false;
            // 
            // manualRecordCheckBox
            // 
            this.manualRecordCheckBox.AutoSize = true;
            this.manualRecordCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.manualRecordCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manualRecordCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.manualRecordCheckBox.Image = global::SmartSearch.Properties.Resources.record;
            this.manualRecordCheckBox.Location = new System.Drawing.Point(19, 391);
            this.manualRecordCheckBox.Name = "manualRecordCheckBox";
            this.manualRecordCheckBox.Size = new System.Drawing.Size(143, 21);
            this.manualRecordCheckBox.TabIndex = 31;
            this.manualRecordCheckBox.Text = "Manual Record";
            this.manualRecordCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.manualRecordCheckBox.UseVisualStyleBackColor = false;
            // 
            // doCheckBox
            // 
            this.doCheckBox.AutoSize = true;
            this.doCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.doCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.doCheckBox.Image = global::SmartSearch.Properties.Resources._do;
            this.doCheckBox.Location = new System.Drawing.Point(19, 516);
            this.doCheckBox.Name = "doCheckBox";
            this.doCheckBox.Size = new System.Drawing.Size(133, 21);
            this.doCheckBox.TabIndex = 30;
            this.doCheckBox.Text = "Digital Output";
            this.doCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.doCheckBox.UseVisualStyleBackColor = false;
            // 
            // setupIVSMotionPictureBox
            // 
            this.setupIVSMotionPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.setupIVSMotionPictureBox.BackgroundImage = global::SmartSearch.Properties.Resources.ive_setup;
            this.setupIVSMotionPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.setupIVSMotionPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setupIVSMotionPictureBox.Location = new System.Drawing.Point(51, 291);
            this.setupIVSMotionPictureBox.Name = "setupIVSMotionPictureBox";
            this.setupIVSMotionPictureBox.Size = new System.Drawing.Size(33, 20);
            this.setupIVSMotionPictureBox.TabIndex = 27;
            this.setupIVSMotionPictureBox.TabStop = false;
            this.setupIVSMotionPictureBox.Visible = false;
            // 
            // ivsLabel
            // 
            this.ivsLabel.BackColor = System.Drawing.Color.Transparent;
            this.ivsLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ivsLabel.ForeColor = System.Drawing.Color.White;
            this.ivsLabel.Location = new System.Drawing.Point(22, 289);
            this.ivsLabel.Name = "ivsLabel";
            this.ivsLabel.Size = new System.Drawing.Size(171, 22);
            this.ivsLabel.TabIndex = 26;
            this.ivsLabel.Text = "IVS";
            this.ivsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ivsMotionCheckBox
            // 
            this.ivsMotionCheckBox.AutoSize = true;
            this.ivsMotionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.ivsMotionCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ivsMotionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ivsMotionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.ivsMotionCheckBox.Image = global::SmartSearch.Properties.Resources.ivs;
            this.ivsMotionCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ivsMotionCheckBox.Location = new System.Drawing.Point(19, 314);
            this.ivsMotionCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.ivsMotionCheckBox.Name = "ivsMotionCheckBox";
            this.ivsMotionCheckBox.Size = new System.Drawing.Size(153, 21);
            this.ivsMotionCheckBox.TabIndex = 25;
            this.ivsMotionCheckBox.Text = "Motion Detection";
            this.ivsMotionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ivsMotionCheckBox.UseVisualStyleBackColor = false;
            // 
            // periodLabel
            // 
            this.periodLabel.BackColor = System.Drawing.Color.Transparent;
            this.periodLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.periodLabel.ForeColor = System.Drawing.Color.White;
            this.periodLabel.Location = new System.Drawing.Point(22, 229);
            this.periodLabel.Name = "periodLabel";
            this.periodLabel.Size = new System.Drawing.Size(171, 22);
            this.periodLabel.TabIndex = 23;
            this.periodLabel.Text = "Period : 10Mins";
            this.periodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // periodPanel
            // 
            this.periodPanel.BackColor = System.Drawing.Color.Transparent;
            this.periodPanel.BackgroundImage = global::SmartSearch.Properties.Resources.scaleBar;
            this.periodPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.periodPanel.Controls.Add(this.periodPointPictureBox);
            this.periodPanel.Controls.Add(this.minusPictureBox);
            this.periodPanel.Controls.Add(this.plusPictureBox);
            this.periodPanel.Location = new System.Drawing.Point(36, 254);
            this.periodPanel.Name = "periodPanel";
            this.periodPanel.Size = new System.Drawing.Size(164, 23);
            this.periodPanel.TabIndex = 21;
            // 
            // periodPointPictureBox
            // 
            this.periodPointPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.periodPointPictureBox.BackgroundImage = global::SmartSearch.Properties.Resources.scalePoint;
            this.periodPointPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.periodPointPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.periodPointPictureBox.Location = new System.Drawing.Point(15, 0);
            this.periodPointPictureBox.Name = "periodPointPictureBox";
            this.periodPointPictureBox.Size = new System.Drawing.Size(11, 23);
            this.periodPointPictureBox.TabIndex = 5;
            this.periodPointPictureBox.TabStop = false;
            // 
            // minusPictureBox
            // 
            this.minusPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.minusPictureBox.BackgroundImage = global::SmartSearch.Properties.Resources.minus_scale;
            this.minusPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minusPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minusPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.minusPictureBox.Location = new System.Drawing.Point(0, 0);
            this.minusPictureBox.Name = "minusPictureBox";
            this.minusPictureBox.Size = new System.Drawing.Size(24, 23);
            this.minusPictureBox.TabIndex = 20;
            this.minusPictureBox.TabStop = false;
            // 
            // plusPictureBox
            // 
            this.plusPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.plusPictureBox.BackgroundImage = global::SmartSearch.Properties.Resources.plus_scale;
            this.plusPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plusPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plusPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.plusPictureBox.Location = new System.Drawing.Point(140, 0);
            this.plusPictureBox.Name = "plusPictureBox";
            this.plusPictureBox.Size = new System.Drawing.Size(24, 23);
            this.plusPictureBox.TabIndex = 19;
            this.plusPictureBox.TabStop = false;
            // 
            // eventLabel
            // 
            this.eventLabel.BackColor = System.Drawing.Color.Transparent;
            this.eventLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventLabel.ForeColor = System.Drawing.Color.White;
            this.eventLabel.Location = new System.Drawing.Point(22, 341);
            this.eventLabel.Name = "eventLabel";
            this.eventLabel.Size = new System.Drawing.Size(171, 22);
            this.eventLabel.TabIndex = 13;
            this.eventLabel.Tag = "Event";
            this.eventLabel.Text = "Event";
            this.eventLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // diCheckBox
            // 
            this.diCheckBox.AutoSize = true;
            this.diCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.diCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.diCheckBox.Image = global::SmartSearch.Properties.Resources.di;
            this.diCheckBox.Location = new System.Drawing.Point(19, 366);
            this.diCheckBox.Name = "diCheckBox";
            this.diCheckBox.Size = new System.Drawing.Size(120, 21);
            this.diCheckBox.TabIndex = 12;
            this.diCheckBox.Text = "Digital Input";
            this.diCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.diCheckBox.UseVisualStyleBackColor = false;
            // 
            // motionCheckBox
            // 
            this.motionCheckBox.AutoSize = true;
            this.motionCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.motionCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motionCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
            this.motionCheckBox.Image = global::SmartSearch.Properties.Resources.motion;
            this.motionCheckBox.Location = new System.Drawing.Point(19, 416);
            this.motionCheckBox.Name = "motionCheckBox";
            this.motionCheckBox.Size = new System.Drawing.Size(87, 21);
            this.motionCheckBox.TabIndex = 11;
            this.motionCheckBox.Text = "Motion";
            this.motionCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.motionCheckBox.UseVisualStyleBackColor = false;
            // 
            // SearchCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.conditionPanel);
            this.Controls.Add(this.buttonPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchCondition";
            this.Size = new System.Drawing.Size(220, 700);
            this.buttonPanel.ResumeLayout(false);
            this.conditionPanel.ResumeLayout(false);
            this.conditionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setupIVSMotionPictureBox)).EndInit();
            this.periodPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.periodPointPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plusPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public PanelBase.DoubleBufferPanel conditionPanel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.CheckBox periodTimeCheckBox;
        public System.Windows.Forms.CheckBox panicCheckBox;
        private System.Windows.Forms.CheckBox videoRecoveryCheckBox;
        private System.Windows.Forms.CheckBox videoLossCheckBox;
        private System.Windows.Forms.CheckBox networkRecoveryCheckBox;
        private System.Windows.Forms.CheckBox networkLossCheckBox;
        private System.Windows.Forms.CheckBox manualRecordCheckBox;
        private System.Windows.Forms.CheckBox doCheckBox;
        private System.Windows.Forms.PictureBox setupIVSMotionPictureBox;
        private System.Windows.Forms.Label ivsLabel;
        private System.Windows.Forms.CheckBox ivsMotionCheckBox;
        private System.Windows.Forms.Label periodLabel;
        private PanelBase.DoubleBufferPanel periodPanel;
        private System.Windows.Forms.PictureBox periodPointPictureBox;
        private System.Windows.Forms.PictureBox minusPictureBox;
        private System.Windows.Forms.PictureBox plusPictureBox;
        private System.Windows.Forms.Label eventLabel;
        private System.Windows.Forms.CheckBox diCheckBox;
        private System.Windows.Forms.CheckBox motionCheckBox;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox nvrComboBox;
        private System.Windows.Forms.Label nvrLabel;
        private System.Windows.Forms.CheckBox crossLineCheckBox;
        private System.Windows.Forms.CheckBox intrusionDetectionCheckBox;
        private System.Windows.Forms.CheckBox loiteringDetectionCheckBox;
        private System.Windows.Forms.CheckBox objectCountingInCheckBox;
        private System.Windows.Forms.CheckBox objectCountingOutCheckBox;
        private System.Windows.Forms.CheckBox audioDetectionCheckBox;
        private System.Windows.Forms.CheckBox temperingDetectionCheckBox;
        private System.Windows.Forms.CheckBox userDefineCheckBox;
    }
}
