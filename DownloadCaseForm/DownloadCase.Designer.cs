using System.Drawing;

namespace DownloadCaseForm
{
    sealed partial class DownloadCase
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
            this.downloadButton = new System.Windows.Forms.Button();
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
            this.commentLabel = new System.Windows.Forms.Label();
            this.addedToQueueButton = new System.Windows.Forms.Button();
            this.endoceComboBox = new System.Windows.Forms.ComboBox();
            this.mbLabel = new System.Windows.Forms.Label();
            this.MaxiNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.exportMaxiSizeLabel = new System.Windows.Forms.Label();
            this.scaleComboBox = new System.Windows.Forms.ComboBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.qualityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.qualityLabel = new System.Windows.Forms.Label();
            this.commentTextBox = new PanelBase.HotKeyTextBox();
            this.pathTextBox = new PanelBase.HotKeyTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.MaxiNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // exportProgressBar
            // 
            this.exportProgressBar.Location = new System.Drawing.Point(93, 415);
            this.exportProgressBar.Name = "exportProgressBar";
            this.exportProgressBar.Size = new System.Drawing.Size(278, 23);
            this.exportProgressBar.Step = 1;
            this.exportProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.exportProgressBar.TabIndex = 12;
            this.exportProgressBar.Value = 50;
            // 
            // completedLabel
            // 
            this.completedLabel.AutoSize = true;
            this.completedLabel.BackColor = System.Drawing.Color.Transparent;
            this.completedLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.completedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.completedLabel.Location = new System.Drawing.Point(94, 397);
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
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(156, 18);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(103, 21);
            this.startDatePicker.TabIndex = 1;
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm:ss";
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(262, 18);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(75, 21);
            this.startTimePicker.TabIndex = 2;
            // 
            // exportStatusLabel
            // 
            this.exportStatusLabel.AutoSize = true;
            this.exportStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.exportStatusLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.exportStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.exportStatusLabel.Location = new System.Drawing.Point(375, 436);
            this.exportStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.exportStatusLabel.Name = "exportStatusLabel";
            this.exportStatusLabel.Size = new System.Drawing.Size(37, 17);
            this.exportStatusLabel.TabIndex = 11;
            this.exportStatusLabel.Text = "50%";
            this.exportStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // downloadButton
            // 
            this.downloadButton.BackColor = System.Drawing.Color.Transparent;
            this.downloadButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.exportButton;
            this.downloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.downloadButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downloadButton.FlatAppearance.BorderSize = 0;
            this.downloadButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.downloadButton.Location = new System.Drawing.Point(166, 458);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(150, 41);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = false;
            this.downloadButton.Click += new System.EventHandler(this.DownloadButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.cancelButotn;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cancelButton.Location = new System.Drawing.Point(323, 458);
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
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(262, 52);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(75, 21);
            this.endTimePicker.TabIndex = 4;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "yyyy-MM-dd";
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(156, 52);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(103, 21);
            this.endDatePicker.TabIndex = 3;
            // 
            // browserButton
            // 
            this.browserButton.BackColor = System.Drawing.Color.Transparent;
            this.browserButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.SelectFolder;
            this.browserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.browserButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.browserButton.FlatAppearance.BorderSize = 0;
            this.browserButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browserButton.ForeColor = System.Drawing.Color.Black;
            this.browserButton.Location = new System.Drawing.Point(335, 86);
            this.browserButton.Name = "browserButton";
            this.browserButton.Size = new System.Drawing.Size(30, 26);
            this.browserButton.TabIndex = 9;
            this.browserButton.UseVisualStyleBackColor = false;
            this.browserButton.Click += new System.EventHandler(this.BrowserButtonMouseClick);
            // 
            // startLabel
            // 
            this.startLabel.BackColor = System.Drawing.Color.Transparent;
            this.startLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.startLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.startLabel.Location = new System.Drawing.Point(92, 18);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(64, 22);
            this.startLabel.TabIndex = 2;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // endLabel
            // 
            this.endLabel.BackColor = System.Drawing.Color.Transparent;
            this.endLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.endLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.endLabel.Location = new System.Drawing.Point(92, 52);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(64, 22);
            this.endLabel.TabIndex = 1;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pathLabel
            // 
            this.pathLabel.BackColor = System.Drawing.Color.Transparent;
            this.pathLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.pathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.pathLabel.Location = new System.Drawing.Point(92, 86);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(64, 22);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path";
            this.pathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // overlayCheckBox
            // 
            this.overlayCheckBox.AutoSize = true;
            this.overlayCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.overlayCheckBox.Checked = true;
            this.overlayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overlayCheckBox.Font = new System.Drawing.Font("Arial", 11F);
            this.overlayCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.overlayCheckBox.Location = new System.Drawing.Point(156, 249);
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
            this.audioInCheckBox.Font = new System.Drawing.Font("Arial", 11F);
            this.audioInCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioInCheckBox.Location = new System.Drawing.Point(156, 124);
            this.audioInCheckBox.Name = "audioInCheckBox";
            this.audioInCheckBox.Size = new System.Drawing.Size(78, 21);
            this.audioInCheckBox.TabIndex = 14;
            this.audioInCheckBox.Text = "Audio In";
            this.audioInCheckBox.UseVisualStyleBackColor = false;
            // 
            // audioLabel
            // 
            this.audioLabel.BackColor = System.Drawing.Color.Transparent;
            this.audioLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.audioLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioLabel.Location = new System.Drawing.Point(92, 120);
            this.audioLabel.Name = "audioLabel";
            this.audioLabel.Size = new System.Drawing.Size(60, 22);
            this.audioLabel.TabIndex = 15;
            this.audioLabel.Text = "Audio";
            this.audioLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // encodeLabel
            // 
            this.encodeLabel.BackColor = System.Drawing.Color.Transparent;
            this.encodeLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.encodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.encodeLabel.Location = new System.Drawing.Point(90, 150);
            this.encodeLabel.Name = "encodeLabel";
            this.encodeLabel.Size = new System.Drawing.Size(60, 22);
            this.encodeLabel.TabIndex = 16;
            this.encodeLabel.Text = "Encode";
            this.encodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // audioOutCheckBox
            // 
            this.audioOutCheckBox.AutoSize = true;
            this.audioOutCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.audioOutCheckBox.Checked = true;
            this.audioOutCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.audioOutCheckBox.Font = new System.Drawing.Font("Arial", 11F);
            this.audioOutCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.audioOutCheckBox.Location = new System.Drawing.Point(249, 124);
            this.audioOutCheckBox.Name = "audioOutCheckBox";
            this.audioOutCheckBox.Size = new System.Drawing.Size(91, 21);
            this.audioOutCheckBox.TabIndex = 17;
            this.audioOutCheckBox.Text = "Audio Out";
            this.audioOutCheckBox.UseVisualStyleBackColor = false;
            // 
            // doneButton
            // 
            this.doneButton.BackColor = System.Drawing.Color.Transparent;
            this.doneButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.exportButton;
            this.doneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.doneButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doneButton.FlatAppearance.BorderSize = 0;
            this.doneButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doneButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.doneButton.Location = new System.Drawing.Point(323, 494);
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
            this.orangeRadioButton.Font = new System.Drawing.Font("Arial", 11F);
            this.orangeRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.orangeRadioButton.Location = new System.Drawing.Point(156, 201);
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
            this.encodeAVIRadioButton.Font = new System.Drawing.Font("Arial", 11F);
            this.encodeAVIRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.encodeAVIRadioButton.Location = new System.Drawing.Point(156, 224);
            this.encodeAVIRadioButton.Name = "encodeAVIRadioButton";
            this.encodeAVIRadioButton.Size = new System.Drawing.Size(146, 21);
            this.encodeAVIRadioButton.TabIndex = 20;
            this.encodeAVIRadioButton.Text = "Convert to MJPEG";
            this.encodeAVIRadioButton.UseVisualStyleBackColor = false;
            this.encodeAVIRadioButton.CheckedChanged += new System.EventHandler(this.EncodeRadioButtonCheckedChanged);
            // 
            // commentLabel
            // 
            this.commentLabel.BackColor = System.Drawing.Color.Transparent;
            this.commentLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.commentLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.commentLabel.Location = new System.Drawing.Point(90, 324);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(86, 22);
            this.commentLabel.TabIndex = 22;
            this.commentLabel.Text = "Comment";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // addedToQueueButton
            // 
            this.addedToQueueButton.BackColor = System.Drawing.Color.Transparent;
            this.addedToQueueButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.exportButton;
            this.addedToQueueButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addedToQueueButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addedToQueueButton.FlatAppearance.BorderSize = 0;
            this.addedToQueueButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.addedToQueueButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.addedToQueueButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.addedToQueueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addedToQueueButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedToQueueButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.addedToQueueButton.Location = new System.Drawing.Point(9, 458);
            this.addedToQueueButton.Name = "addedToQueueButton";
            this.addedToQueueButton.Size = new System.Drawing.Size(150, 41);
            this.addedToQueueButton.TabIndex = 24;
            this.addedToQueueButton.Text = "Add to queue";
            this.addedToQueueButton.UseVisualStyleBackColor = false;
            this.addedToQueueButton.Click += new System.EventHandler(this.AddedToQueueButtonClick);
            // 
            // endoceComboBox
            // 
            this.endoceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endoceComboBox.FormattingEnabled = true;
            this.endoceComboBox.Location = new System.Drawing.Point(156, 151);
            this.endoceComboBox.Name = "endoceComboBox";
            this.endoceComboBox.Size = new System.Drawing.Size(77, 23);
            this.endoceComboBox.TabIndex = 31;
            // 
            // mbLabel
            // 
            this.mbLabel.BackColor = System.Drawing.Color.Transparent;
            this.mbLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.mbLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.mbLabel.Location = new System.Drawing.Point(364, 177);
            this.mbLabel.Name = "mbLabel";
            this.mbLabel.Size = new System.Drawing.Size(33, 22);
            this.mbLabel.TabIndex = 34;
            this.mbLabel.Text = "MB";
            this.mbLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxiNumericUpDown
            // 
            this.MaxiNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Location = new System.Drawing.Point(298, 179);
            this.MaxiNumericUpDown.Maximum = new decimal(new int[] {
            1843,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.MaxiNumericUpDown.Name = "MaxiNumericUpDown";
            this.MaxiNumericUpDown.Size = new System.Drawing.Size(60, 21);
            this.MaxiNumericUpDown.TabIndex = 33;
            this.MaxiNumericUpDown.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // exportMaxiSizeLabel
            // 
            this.exportMaxiSizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.exportMaxiSizeLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.exportMaxiSizeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.exportMaxiSizeLabel.Location = new System.Drawing.Point(153, 177);
            this.exportMaxiSizeLabel.Name = "exportMaxiSizeLabel";
            this.exportMaxiSizeLabel.Size = new System.Drawing.Size(139, 22);
            this.exportMaxiSizeLabel.TabIndex = 32;
            this.exportMaxiSizeLabel.Text = "Export maximum size";
            this.exportMaxiSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scaleComboBox
            // 
            this.scaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleComboBox.FormattingEnabled = true;
            this.scaleComboBox.Location = new System.Drawing.Point(280, 296);
            this.scaleComboBox.Name = "scaleComboBox";
            this.scaleComboBox.Size = new System.Drawing.Size(60, 23);
            this.scaleComboBox.TabIndex = 38;
            // 
            // scaleLabel
            // 
            this.scaleLabel.BackColor = System.Drawing.Color.Transparent;
            this.scaleLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.scaleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.scaleLabel.Location = new System.Drawing.Point(153, 295);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(52, 22);
            this.scaleLabel.TabIndex = 37;
            this.scaleLabel.Text = "Scale";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // qualityNumericUpDown
            // 
            this.qualityNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityNumericUpDown.Location = new System.Drawing.Point(280, 274);
            this.qualityNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.qualityNumericUpDown.Name = "qualityNumericUpDown";
            this.qualityNumericUpDown.Size = new System.Drawing.Size(60, 21);
            this.qualityNumericUpDown.TabIndex = 36;
            this.qualityNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // qualityLabel
            // 
            this.qualityLabel.BackColor = System.Drawing.Color.Transparent;
            this.qualityLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.qualityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.qualityLabel.Location = new System.Drawing.Point(153, 271);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(52, 22);
            this.qualityLabel.TabIndex = 35;
            this.qualityLabel.Text = "Quality";
            this.qualityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // commentTextBox
            // 
            this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commentTextBox.ForeColor = System.Drawing.Color.Black;
            this.commentTextBox.Location = new System.Drawing.Point(93, 349);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.ShortcutsEnabled = false;
            this.commentTextBox.Size = new System.Drawing.Size(324, 46);
            this.commentTextBox.TabIndex = 23;
            this.commentTextBox.Text = "Write Comment Here.";
            this.commentTextBox.Enter += new System.EventHandler(this.CommentTextBoxEnter);
            this.commentTextBox.Leave += new System.EventHandler(this.CommentTextBoxLeave);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(156, 87);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ShortcutsEnabled = false;
            this.pathTextBox.Size = new System.Drawing.Size(181, 21);
            this.pathTextBox.TabIndex = 10;
            this.pathTextBox.Text = "C:\\";
            // 
            // DownloadCase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::DownloadCaseForm.Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(483, 632);
            this.ControlBox = false;
            this.Controls.Add(this.scaleComboBox);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.qualityNumericUpDown);
            this.Controls.Add(this.qualityLabel);
            this.Controls.Add(this.mbLabel);
            this.Controls.Add(this.MaxiNumericUpDown);
            this.Controls.Add(this.exportMaxiSizeLabel);
            this.Controls.Add(this.endoceComboBox);
            this.Controls.Add(this.addedToQueueButton);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.commentLabel);
            this.Controls.Add(this.encodeAVIRadioButton);
            this.Controls.Add(this.orangeRadioButton);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.audioOutCheckBox);
            this.Controls.Add(this.encodeLabel);
            this.Controls.Add(this.audioLabel);
            this.Controls.Add(this.audioInCheckBox);
            this.Controls.Add(this.overlayCheckBox);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.endLabel);
            this.Controls.Add(this.startLabel);
            this.Controls.Add(this.browserButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.endTimePicker);
            this.Controls.Add(this.endDatePicker);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportStatusLabel);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.startDatePicker);
            this.Controls.Add(this.exportProgressBar);
            this.Controls.Add(this.completedLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DownloadCase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download Case";
            ((System.ComponentModel.ISupportInitialize)(this.MaxiNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar exportProgressBar;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.Label exportStatusLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button downloadButton;
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
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.RadioButton orangeRadioButton;
        private System.Windows.Forms.RadioButton encodeAVIRadioButton;
        private System.Windows.Forms.Label completedLabel;
        private System.Windows.Forms.Label commentLabel;
        private PanelBase.HotKeyTextBox pathTextBox;
        private PanelBase.HotKeyTextBox commentTextBox;
        private System.Windows.Forms.Button addedToQueueButton;
        private System.Windows.Forms.ComboBox endoceComboBox;
        private System.Windows.Forms.Label mbLabel;
        private System.Windows.Forms.NumericUpDown MaxiNumericUpDown;
        private System.Windows.Forms.Label exportMaxiSizeLabel;
        private System.Windows.Forms.ComboBox scaleComboBox;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.NumericUpDown qualityNumericUpDown;
        private System.Windows.Forms.Label qualityLabel;
    }
}