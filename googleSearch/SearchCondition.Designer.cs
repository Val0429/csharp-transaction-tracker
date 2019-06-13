namespace GoogleSearch
{
    partial class SearchCondition
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
            this.videoWindowPanel = new System.Windows.Forms.Panel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.conditionPanel = new PanelBase.DoubleBufferPanel();
            this.speedLimitCheckBox = new System.Windows.Forms.CheckBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.speacingLabel = new System.Windows.Forms.Label();
            this.speacingComboBox = new System.Windows.Forms.ComboBox();
            this.secondLabel = new System.Windows.Forms.Label();
            this.speedLimitText = new System.Windows.Forms.TextBox();
            this.kmLabel = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.conditionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoWindowPanel
            // 
            this.videoWindowPanel.BackColor = System.Drawing.Color.DimGray;
            this.videoWindowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoWindowPanel.Location = new System.Drawing.Point(10, 40);
            this.videoWindowPanel.Name = "videoWindowPanel";
            this.videoWindowPanel.Size = new System.Drawing.Size(623, 285);
            this.videoWindowPanel.TabIndex = 1;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(10, 10);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(888, 30);
            this.titlePanel.TabIndex = 19;
            // 
            // conditionPanel
            // 
            this.conditionPanel.BackgroundImage = global::GoogleSearch.Properties.Resources.controllerBG;
            this.conditionPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.conditionPanel.Controls.Add(this.speedLimitCheckBox);
            this.conditionPanel.Controls.Add(this.dateLabel);
            this.conditionPanel.Controls.Add(this.datePicker);
            this.conditionPanel.Controls.Add(this.startTimeLabel);
            this.conditionPanel.Controls.Add(this.startTimePicker);
            this.conditionPanel.Controls.Add(this.endTimeLabel);
            this.conditionPanel.Controls.Add(this.endTimePicker);
            this.conditionPanel.Controls.Add(this.speacingLabel);
            this.conditionPanel.Controls.Add(this.speacingComboBox);
            this.conditionPanel.Controls.Add(this.secondLabel);
            this.conditionPanel.Controls.Add(this.speedLimitText);
            this.conditionPanel.Controls.Add(this.kmLabel);
            this.conditionPanel.Controls.Add(this.searchButton);
            this.conditionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.conditionPanel.Location = new System.Drawing.Point(633, 40);
            this.conditionPanel.Name = "conditionPanel";
            this.conditionPanel.Size = new System.Drawing.Size(265, 285);
            this.conditionPanel.TabIndex = 0;
            // 
            // speedLimitCheckBox
            // 
            this.speedLimitCheckBox.AutoSize = true;
            this.speedLimitCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.speedLimitCheckBox.Location = new System.Drawing.Point(10, 150);
            this.speedLimitCheckBox.Name = "speedLimitCheckBox";
            this.speedLimitCheckBox.Size = new System.Drawing.Size(92, 19);
            this.speedLimitCheckBox.TabIndex = 33;
            this.speedLimitCheckBox.Text = "Speed Limit";
            this.speedLimitCheckBox.UseVisualStyleBackColor = false;
            this.speedLimitCheckBox.CheckedChanged += new System.EventHandler(this.SpeedLimitCheckBoxCheckedChanged);
            // 
            // dateLabel
            // 
            this.dateLabel.BackColor = System.Drawing.Color.Transparent;
            this.dateLabel.Location = new System.Drawing.Point(10, 15);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(74, 25);
            this.dateLabel.TabIndex = 22;
            this.dateLabel.Text = "Date";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "yyyy-MM-dd";
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(110, 17);
            this.datePicker.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(103, 21);
            this.datePicker.TabIndex = 19;
            this.datePicker.ValueChanged += new System.EventHandler(this.DatePickerValueChanged);
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.startTimeLabel.Location = new System.Drawing.Point(10, 48);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(74, 25);
            this.startTimeLabel.TabIndex = 20;
            this.startTimeLabel.Text = "Start Time";
            this.startTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm:ss";
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(110, 50);
            this.startTimePicker.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(80, 21);
            this.startTimePicker.TabIndex = 21;
            this.startTimePicker.ValueChanged += new System.EventHandler(this.StartTimePickerValueChanged);
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.endTimeLabel.Location = new System.Drawing.Point(10, 81);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(74, 25);
            this.endTimeLabel.TabIndex = 30;
            this.endTimeLabel.Text = "End Time";
            this.endTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "HH:mm:ss";
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(110, 83);
            this.endTimePicker.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(80, 21);
            this.endTimePicker.TabIndex = 24;
            this.endTimePicker.ValueChanged += new System.EventHandler(this.EndTimePickerValueChanged);
            // 
            // speacingLabel
            // 
            this.speacingLabel.BackColor = System.Drawing.Color.Transparent;
            this.speacingLabel.Location = new System.Drawing.Point(10, 114);
            this.speacingLabel.Name = "speacingLabel";
            this.speacingLabel.Size = new System.Drawing.Size(74, 25);
            this.speacingLabel.TabIndex = 31;
            this.speacingLabel.Text = "Spacing";
            this.speacingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // speacingComboBox
            // 
            this.speacingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.speacingComboBox.FormattingEnabled = true;
            this.speacingComboBox.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "60",
            "120",
            "300",
            "600",
            "1800",
            "3600"});
            this.speacingComboBox.Location = new System.Drawing.Point(110, 115);
            this.speacingComboBox.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.speacingComboBox.Name = "speacingComboBox";
            this.speacingComboBox.Size = new System.Drawing.Size(65, 23);
            this.speacingComboBox.TabIndex = 32;
            // 
            // secondLabel
            // 
            this.secondLabel.BackColor = System.Drawing.Color.Transparent;
            this.secondLabel.Location = new System.Drawing.Point(189, 114);
            this.secondLabel.Name = "secondLabel";
            this.secondLabel.Size = new System.Drawing.Size(60, 25);
            this.secondLabel.TabIndex = 31;
            this.secondLabel.Text = "seconds";
            this.secondLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // speedLimitText
            // 
            this.speedLimitText.Enabled = false;
            this.speedLimitText.Location = new System.Drawing.Point(110, 149);
            this.speedLimitText.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.speedLimitText.Name = "speedLimitText";
            this.speedLimitText.Size = new System.Drawing.Size(65, 21);
            this.speedLimitText.TabIndex = 29;
            this.speedLimitText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpeedLimitTextKeyPress);
            // 
            // kmLabel
            // 
            this.kmLabel.BackColor = System.Drawing.Color.Transparent;
            this.kmLabel.Location = new System.Drawing.Point(189, 147);
            this.kmLabel.Name = "kmLabel";
            this.kmLabel.Size = new System.Drawing.Size(60, 25);
            this.kmLabel.TabIndex = 31;
            this.kmLabel.Text = "km";
            this.kmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.BackColor = System.Drawing.Color.Transparent;
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchButton.Enabled = false;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.searchButton.Location = new System.Drawing.Point(177, 235);
            this.searchButton.Margin = new System.Windows.Forms.Padding(3, 12, 15, 3);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(70, 30);
            this.searchButton.TabIndex = 28;
            this.searchButton.TabStop = false;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // SearchCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.videoWindowPanel);
            this.Controls.Add(this.conditionPanel);
            this.Controls.Add(this.titlePanel);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SearchCondition";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.Size = new System.Drawing.Size(908, 325);
            this.Load += new System.EventHandler(this.SearchConditionLoad);
            this.conditionPanel.ResumeLayout(false);
            this.conditionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel conditionPanel;
        private System.Windows.Forms.Panel videoWindowPanel;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.Label speacingLabel;
        private System.Windows.Forms.Label secondLabel;
        private System.Windows.Forms.Label kmLabel;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.ComboBox speacingComboBox;
        private System.Windows.Forms.TextBox speedLimitText;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.CheckBox speedLimitCheckBox;
    }
}
