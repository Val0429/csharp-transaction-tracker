namespace ExportVideoForm
{
    partial class ExportVideo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.exportProgressBar = new System.Windows.Forms.ProgressBar();
            this.completedLabel = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.exportStatusLabel = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.exportFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.browserButton = new System.Windows.Forms.Button();
            this.startLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.overlayCheckBox = new System.Windows.Forms.CheckBox();
            this.audioInCheckBox = new System.Windows.Forms.CheckBox();
            this.audioLabel = new System.Windows.Forms.Label();
            this.encodeLabel = new System.Windows.Forms.Label();
            this.audioOutCheckBox = new System.Windows.Forms.CheckBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.orangeRadioButton = new System.Windows.Forms.RadioButton();
            this.encodeAVIRadioButton = new System.Windows.Forms.RadioButton();
            this.exportMaxiSizeLabel = new System.Windows.Forms.Label();
            this.MaxiNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.qualityLabel = new System.Windows.Forms.Label();
            this.qualityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.scaleComboBox = new System.Windows.Forms.ComboBox();
            this.endoceComboBox = new System.Windows.Forms.ComboBox();
            this.mbLabel = new System.Windows.Forms.Label();
            this.dewarpLabel = new System.Windows.Forms.Label();
            this.dewarpComboBox = new System.Windows.Forms.ComboBox();
            this.pathTextBox = new PanelBase.HotKeyTextBox();
            this.fisheyeOnlyLabel = new System.Windows.Forms.Label();
            this._audioPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MaxiNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityNumericUpDown)).BeginInit();
            this._audioPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportProgressBar
            // 
            this.exportProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportProgressBar.Location = new System.Drawing.Point(66, 412);
            this.exportProgressBar.Name = "exportProgressBar";
            this.exportProgressBar.Size = new System.Drawing.Size(335, 23);
            this.exportProgressBar.Step = 1;
            this.exportProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.exportProgressBar.TabIndex = 12;
            this.exportProgressBar.Value = 50;
            // 
            // completedLabel
            // 
            this.completedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.completedLabel.AutoSize = true;
            this.completedLabel.BackColor = System.Drawing.Color.Transparent;
            this.completedLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.completedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.completedLabel.Location = new System.Drawing.Point(64, 439);
            this.completedLabel.Margin = new System.Windows.Forms.Padding(0);
            this.completedLabel.Name = "completedLabel";
            this.completedLabel.Size = new System.Drawing.Size(122, 17);
            this.completedLabel.TabIndex = 21;
            this.completedLabel.Text = "Elapsed time: %1";
            this.completedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "yyyy-MM-dd";
            this.startDatePicker.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(147, 19);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(111, 24);
            this.startDatePicker.TabIndex = 1;
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm:ss";
            this.startTimePicker.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(268, 19);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(83, 24);
            this.startTimePicker.TabIndex = 2;
            // 
            // exportStatusLabel
            // 
            this.exportStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportStatusLabel.AutoSize = true;
            this.exportStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.exportStatusLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.exportStatusLabel.Location = new System.Drawing.Point(405, 415);
            this.exportStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.exportStatusLabel.Name = "exportStatusLabel";
            this.exportStatusLabel.Size = new System.Drawing.Size(37, 17);
            this.exportStatusLabel.TabIndex = 11;
            this.exportStatusLabel.Text = "50%";
            this.exportStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportButton.BackColor = System.Drawing.Color.Transparent;
            this.exportButton.BackgroundImage = global::ExportVideoForm.Properties.Resources.exportButton;
            this.exportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.exportButton.Location = new System.Drawing.Point(72, 467);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(150, 41);
            this.exportButton.TabIndex = 5;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.ExportButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImage = global::ExportVideoForm.Properties.Resources.cancelButotn;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cancelButton.Location = new System.Drawing.Point(240, 467);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 41);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "HH:mm:ss";
            this.endTimePicker.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(268, 54);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(83, 24);
            this.endTimePicker.TabIndex = 4;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "yyyy-MM-dd";
            this.endDatePicker.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(147, 54);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(111, 24);
            this.endDatePicker.TabIndex = 3;
            // 
            // browserButton
            // 
            this.browserButton.BackColor = System.Drawing.Color.Transparent;
            this.browserButton.BackgroundImage = global::ExportVideoForm.Properties.Resources.SelectFolder;
            this.browserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.browserButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.browserButton.FlatAppearance.BorderSize = 0;
            this.browserButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browserButton.ForeColor = System.Drawing.Color.Black;
            this.browserButton.Location = new System.Drawing.Point(354, 89);
            this.browserButton.Name = "browserButton";
            this.browserButton.Size = new System.Drawing.Size(49, 26);
            this.browserButton.TabIndex = 9;
            this.browserButton.UseVisualStyleBackColor = false;
            this.browserButton.Click += new System.EventHandler(this.BrowserButtonMouseClick);
            // 
            // startLabel
            // 
            this.startLabel.BackColor = System.Drawing.Color.Transparent;
            this.startLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.startLabel.Location = new System.Drawing.Point(16, 19);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(111, 22);
            this.startLabel.TabIndex = 2;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endLabel
            // 
            this.endLabel.BackColor = System.Drawing.Color.Transparent;
            this.endLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.endLabel.Location = new System.Drawing.Point(16, 54);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(111, 22);
            this.endLabel.TabIndex = 1;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pathLabel
            // 
            this.pathLabel.BackColor = System.Drawing.Color.Transparent;
            this.pathLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.pathLabel.Location = new System.Drawing.Point(16, 89);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(111, 22);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path";
            this.pathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // overlayCheckBox
            // 
            this.overlayCheckBox.AutoSize = true;
            this.overlayCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.overlayCheckBox.Checked = true;
            this.overlayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overlayCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overlayCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.overlayCheckBox.Location = new System.Drawing.Point(147, 113);
            this.overlayCheckBox.Name = "overlayCheckBox";
            this.overlayCheckBox.Size = new System.Drawing.Size(178, 21);
            this.overlayCheckBox.TabIndex = 13;
            this.overlayCheckBox.Text = "Overlay OSD On Video";
            this.overlayCheckBox.UseVisualStyleBackColor = false;
            this.overlayCheckBox.Visible = false;
            // 
            // audioInCheckBox
            // 
            this.audioInCheckBox.AutoSize = true;
            this.audioInCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.audioInCheckBox.Checked = true;
            this.audioInCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.audioInCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioInCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioInCheckBox.Location = new System.Drawing.Point(147, 2);
            this.audioInCheckBox.Name = "audioInCheckBox";
            this.audioInCheckBox.Size = new System.Drawing.Size(78, 21);
            this.audioInCheckBox.TabIndex = 14;
            this.audioInCheckBox.Text = "Audio In";
            this.audioInCheckBox.UseVisualStyleBackColor = false;
            // 
            // audioLabel
            // 
            this.audioLabel.BackColor = System.Drawing.Color.Transparent;
            this.audioLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioLabel.Location = new System.Drawing.Point(16, 0);
            this.audioLabel.Name = "audioLabel";
            this.audioLabel.Size = new System.Drawing.Size(112, 22);
            this.audioLabel.TabIndex = 15;
            this.audioLabel.Text = "Audio";
            this.audioLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // encodeLabel
            // 
            this.encodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.encodeLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.encodeLabel.Location = new System.Drawing.Point(16, 0);
            this.encodeLabel.Name = "encodeLabel";
            this.encodeLabel.Size = new System.Drawing.Size(112, 22);
            this.encodeLabel.TabIndex = 16;
            this.encodeLabel.Text = "Encode";
            this.encodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // audioOutCheckBox
            // 
            this.audioOutCheckBox.AutoSize = true;
            this.audioOutCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.audioOutCheckBox.Checked = true;
            this.audioOutCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.audioOutCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioOutCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioOutCheckBox.Location = new System.Drawing.Point(251, 2);
            this.audioOutCheckBox.Name = "audioOutCheckBox";
            this.audioOutCheckBox.Size = new System.Drawing.Size(91, 21);
            this.audioOutCheckBox.TabIndex = 17;
            this.audioOutCheckBox.Text = "Audio Out";
            this.audioOutCheckBox.UseVisualStyleBackColor = false;
            // 
            // doneButton
            // 
            this.doneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.doneButton.BackColor = System.Drawing.Color.Transparent;
            this.doneButton.BackgroundImage = global::ExportVideoForm.Properties.Resources.exportButton;
            this.doneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.doneButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doneButton.FlatAppearance.BorderSize = 0;
            this.doneButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doneButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.doneButton.Location = new System.Drawing.Point(72, 514);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(150, 41);
            this.doneButton.TabIndex = 18;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = false;
            this.doneButton.Click += new System.EventHandler(this.DoneButtonClick);
            // 
            // orangeRadioButton
            // 
            this.orangeRadioButton.AutoSize = true;
            this.orangeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.orangeRadioButton.Checked = true;
            this.orangeRadioButton.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orangeRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.orangeRadioButton.Location = new System.Drawing.Point(147, 61);
            this.orangeRadioButton.Name = "orangeRadioButton";
            this.orangeRadioButton.Size = new System.Drawing.Size(156, 21);
            this.orangeRadioButton.TabIndex = 19;
            this.orangeRadioButton.TabStop = true;
            this.orangeRadioButton.Text = "Export original video";
            this.orangeRadioButton.UseVisualStyleBackColor = false;
            this.orangeRadioButton.CheckedChanged += new System.EventHandler(this.OrangeRadioButtonCheckedChanged);
            // 
            // encodeAVIRadioButton
            // 
            this.encodeAVIRadioButton.AutoSize = true;
            this.encodeAVIRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.encodeAVIRadioButton.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encodeAVIRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.encodeAVIRadioButton.Location = new System.Drawing.Point(147, 87);
            this.encodeAVIRadioButton.Name = "encodeAVIRadioButton";
            this.encodeAVIRadioButton.Size = new System.Drawing.Size(146, 21);
            this.encodeAVIRadioButton.TabIndex = 20;
            this.encodeAVIRadioButton.Text = "Convert to MJPEG";
            this.encodeAVIRadioButton.UseVisualStyleBackColor = false;
            this.encodeAVIRadioButton.CheckedChanged += new System.EventHandler(this.EncodeRadioButtonCheckedChanged);
            // 
            // exportMaxiSizeLabel
            // 
            this.exportMaxiSizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.exportMaxiSizeLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportMaxiSizeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.exportMaxiSizeLabel.Location = new System.Drawing.Point(144, 37);
            this.exportMaxiSizeLabel.Name = "exportMaxiSizeLabel";
            this.exportMaxiSizeLabel.Size = new System.Drawing.Size(139, 22);
            this.exportMaxiSizeLabel.TabIndex = 24;
            this.exportMaxiSizeLabel.Text = "Export maximum size";
            this.exportMaxiSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxiNumericUpDown
            // 
            this.MaxiNumericUpDown.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxiNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Location = new System.Drawing.Point(289, 36);
            this.MaxiNumericUpDown.Maximum = new decimal(new int[] {
            1843,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Name = "MaxiNumericUpDown";
            this.MaxiNumericUpDown.Size = new System.Drawing.Size(60, 24);
            this.MaxiNumericUpDown.TabIndex = 25;
            this.MaxiNumericUpDown.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // qualityLabel
            // 
            this.qualityLabel.BackColor = System.Drawing.Color.Transparent;
            this.qualityLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qualityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.qualityLabel.Location = new System.Drawing.Point(144, 135);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(90, 22);
            this.qualityLabel.TabIndex = 26;
            this.qualityLabel.Text = "Quality";
            this.qualityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // qualityNumericUpDown
            // 
            this.qualityNumericUpDown.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qualityNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityNumericUpDown.Location = new System.Drawing.Point(250, 136);
            this.qualityNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityNumericUpDown.Name = "qualityNumericUpDown";
            this.qualityNumericUpDown.Size = new System.Drawing.Size(60, 24);
            this.qualityNumericUpDown.TabIndex = 27;
            this.qualityNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // scaleLabel
            // 
            this.scaleLabel.BackColor = System.Drawing.Color.Transparent;
            this.scaleLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.scaleLabel.Location = new System.Drawing.Point(144, 164);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(90, 22);
            this.scaleLabel.TabIndex = 28;
            this.scaleLabel.Text = "Scale";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scaleComboBox
            // 
            this.scaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleComboBox.FormattingEnabled = true;
            this.scaleComboBox.Location = new System.Drawing.Point(249, 165);
            this.scaleComboBox.Name = "scaleComboBox";
            this.scaleComboBox.Size = new System.Drawing.Size(60, 25);
            this.scaleComboBox.TabIndex = 29;
            // 
            // endoceComboBox
            // 
            this.endoceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endoceComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endoceComboBox.FormattingEnabled = true;
            this.endoceComboBox.Location = new System.Drawing.Point(145, 0);
            this.endoceComboBox.Name = "endoceComboBox";
            this.endoceComboBox.Size = new System.Drawing.Size(113, 25);
            this.endoceComboBox.TabIndex = 30;
            // 
            // mbLabel
            // 
            this.mbLabel.BackColor = System.Drawing.Color.Transparent;
            this.mbLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.mbLabel.Location = new System.Drawing.Point(355, 37);
            this.mbLabel.Name = "mbLabel";
            this.mbLabel.Size = new System.Drawing.Size(33, 22);
            this.mbLabel.TabIndex = 31;
            this.mbLabel.Text = "MB";
            this.mbLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dewarpLabel
            // 
            this.dewarpLabel.BackColor = System.Drawing.Color.Transparent;
            this.dewarpLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.dewarpLabel.Location = new System.Drawing.Point(144, 196);
            this.dewarpLabel.Name = "dewarpLabel";
            this.dewarpLabel.Size = new System.Drawing.Size(90, 22);
            this.dewarpLabel.TabIndex = 32;
            this.dewarpLabel.Text = "Dewarp";
            this.dewarpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dewarpComboBox
            // 
            this.dewarpComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dewarpComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpComboBox.FormattingEnabled = true;
            this.dewarpComboBox.Location = new System.Drawing.Point(249, 196);
            this.dewarpComboBox.Name = "dewarpComboBox";
            this.dewarpComboBox.Size = new System.Drawing.Size(102, 25);
            this.dewarpComboBox.TabIndex = 33;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathTextBox.Location = new System.Drawing.Point(147, 90);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ShortcutsEnabled = false;
            this.pathTextBox.Size = new System.Drawing.Size(204, 24);
            this.pathTextBox.TabIndex = 10;
            this.pathTextBox.Text = "C:\\";
            // 
            // fisheyeOnlyLabel
            // 
            this.fisheyeOnlyLabel.BackColor = System.Drawing.Color.Transparent;
            this.fisheyeOnlyLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fisheyeOnlyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.fisheyeOnlyLabel.Location = new System.Drawing.Point(357, 197);
            this.fisheyeOnlyLabel.Name = "fisheyeOnlyLabel";
            this.fisheyeOnlyLabel.Size = new System.Drawing.Size(114, 22);
            this.fisheyeOnlyLabel.TabIndex = 31;
            this.fisheyeOnlyLabel.Text = "(For Fisheye)";
            this.fisheyeOnlyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _audioPanel
            // 
            this._audioPanel.BackColor = System.Drawing.Color.Transparent;
            this._audioPanel.Controls.Add(this.audioOutCheckBox);
            this._audioPanel.Controls.Add(this.audioInCheckBox);
            this._audioPanel.Controls.Add(this.audioLabel);
            this._audioPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._audioPanel.Location = new System.Drawing.Point(0, 126);
            this._audioPanel.Name = "_audioPanel";
            this._audioPanel.Size = new System.Drawing.Size(483, 33);
            this._audioPanel.TabIndex = 34;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.fisheyeOnlyLabel);
            this.panel2.Controls.Add(this.dewarpComboBox);
            this.panel2.Controls.Add(this.dewarpLabel);
            this.panel2.Controls.Add(this.scaleComboBox);
            this.panel2.Controls.Add(this.scaleLabel);
            this.panel2.Controls.Add(this.qualityNumericUpDown);
            this.panel2.Controls.Add(this.qualityLabel);
            this.panel2.Controls.Add(this.overlayCheckBox);
            this.panel2.Controls.Add(this.encodeAVIRadioButton);
            this.panel2.Controls.Add(this.orangeRadioButton);
            this.panel2.Controls.Add(this.exportMaxiSizeLabel);
            this.panel2.Controls.Add(this.MaxiNumericUpDown);
            this.panel2.Controls.Add(this.mbLabel);
            this.panel2.Controls.Add(this.endoceComboBox);
            this.panel2.Controls.Add(this.encodeLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 159);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(483, 231);
            this.panel2.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.browserButton);
            this.panel3.Controls.Add(this.pathTextBox);
            this.panel3.Controls.Add(this.pathLabel);
            this.panel3.Controls.Add(this.endTimePicker);
            this.panel3.Controls.Add(this.endDatePicker);
            this.panel3.Controls.Add(this.endLabel);
            this.panel3.Controls.Add(this.startTimePicker);
            this.panel3.Controls.Add(this.startDatePicker);
            this.panel3.Controls.Add(this.startLabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(483, 126);
            this.panel3.TabIndex = 36;
            // 
            // ExportVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::ExportVideoForm.Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(483, 523);
            this.ControlBox = false;
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportStatusLabel);
            this.Controls.Add(this.exportProgressBar);
            this.Controls.Add(this.completedLabel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this._audioPanel);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ExportVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Video";
            ((System.ComponentModel.ISupportInitialize)(this.MaxiNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityNumericUpDown)).EndInit();
            this._audioPanel.ResumeLayout(false);
            this._audioPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.FolderBrowserDialog exportFolderBrowserDialog;
        private System.Windows.Forms.Button browserButton;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.CheckBox overlayCheckBox;
        private System.Windows.Forms.CheckBox audioInCheckBox;
        private System.Windows.Forms.Label audioLabel;
        private System.Windows.Forms.Label encodeLabel;
        private System.Windows.Forms.CheckBox audioOutCheckBox;
        private System.Windows.Forms.RadioButton orangeRadioButton;
        private System.Windows.Forms.RadioButton encodeAVIRadioButton;
        private PanelBase.HotKeyTextBox pathTextBox;
        public System.Windows.Forms.ProgressBar exportProgressBar;
        public System.Windows.Forms.Label exportStatusLabel;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.Button exportButton;
        public System.Windows.Forms.Button doneButton;
        public System.Windows.Forms.Label completedLabel;
        private System.Windows.Forms.Label exportMaxiSizeLabel;
        private System.Windows.Forms.NumericUpDown MaxiNumericUpDown;
        private System.Windows.Forms.Label qualityLabel;
        private System.Windows.Forms.NumericUpDown qualityNumericUpDown;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.ComboBox scaleComboBox;
        private System.Windows.Forms.ComboBox endoceComboBox;
        private System.Windows.Forms.Label mbLabel;
        private System.Windows.Forms.Label dewarpLabel;
        private System.Windows.Forms.ComboBox dewarpComboBox;
        private System.Windows.Forms.Label fisheyeOnlyLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        protected System.Windows.Forms.Panel _audioPanel;
    }
}