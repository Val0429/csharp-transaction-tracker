namespace SetupServer
{
    sealed partial class TimeControl
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.NTPServerPanel = new PanelBase.DoubleBufferPanel();
            this.checkBoxNTPServer = new System.Windows.Forms.CheckBox();
            this.textBoxNTPServer = new PanelBase.HotKeyTextBox();
            this.daylightPanel = new PanelBase.DoubleBufferPanel();
            this.checkBoxDaylight = new System.Windows.Forms.CheckBox();
            this.dateTimePanel = new PanelBase.DoubleBufferPanel();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.timeZonePanel = new PanelBase.DoubleBufferPanel();
            this.zoneComboBox = new System.Windows.Forms.ComboBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.NTPServerPanel.SuspendLayout();
            this.daylightPanel.SuspendLayout();
            this.dateTimePanel.SuspendLayout();
            this.timeZonePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.NTPServerPanel);
            this.containerPanel.Controls.Add(this.daylightPanel);
            this.containerPanel.Controls.Add(this.dateTimePanel);
            this.containerPanel.Controls.Add(this.timeZonePanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.containerPanel.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.TabIndex = 21;
            // 
            // NTPServerPanel
            // 
            this.NTPServerPanel.BackColor = System.Drawing.Color.Transparent;
            this.NTPServerPanel.Controls.Add(this.checkBoxNTPServer);
            this.NTPServerPanel.Controls.Add(this.textBoxNTPServer);
            this.NTPServerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NTPServerPanel.Location = new System.Drawing.Point(12, 138);
            this.NTPServerPanel.Name = "NTPServerPanel";
            this.NTPServerPanel.Size = new System.Drawing.Size(456, 40);
            this.NTPServerPanel.TabIndex = 21;
            this.NTPServerPanel.Tag = "NTPServer";
            // 
            // checkBoxNTPServer
            // 
            this.checkBoxNTPServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxNTPServer.AutoSize = true;
            this.checkBoxNTPServer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNTPServer.Location = new System.Drawing.Point(367, 13);
            this.checkBoxNTPServer.Name = "checkBoxNTPServer";
            this.checkBoxNTPServer.Size = new System.Drawing.Size(72, 19);
            this.checkBoxNTPServer.TabIndex = 1;
            this.checkBoxNTPServer.Text = "Enabled";
            this.checkBoxNTPServer.UseVisualStyleBackColor = true;
            // 
            // textBoxNTPServer
            // 
            this.textBoxNTPServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNTPServer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNTPServer.Location = new System.Drawing.Point(199, 10);
            this.textBoxNTPServer.Name = "textBoxNTPServer";
            this.textBoxNTPServer.Size = new System.Drawing.Size(156, 21);
            this.textBoxNTPServer.TabIndex = 0;
            // 
            // daylightPanel
            // 
            this.daylightPanel.BackColor = System.Drawing.Color.Transparent;
            this.daylightPanel.Controls.Add(this.checkBoxDaylight);
            this.daylightPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.daylightPanel.Location = new System.Drawing.Point(12, 98);
            this.daylightPanel.Name = "daylightPanel";
            this.daylightPanel.Size = new System.Drawing.Size(456, 40);
            this.daylightPanel.TabIndex = 20;
            this.daylightPanel.Tag = "EnabledDaylight";
            // 
            // checkBoxDaylight
            // 
            this.checkBoxDaylight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDaylight.AutoSize = true;
            this.checkBoxDaylight.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDaylight.Location = new System.Drawing.Point(367, 13);
            this.checkBoxDaylight.Name = "checkBoxDaylight";
            this.checkBoxDaylight.Size = new System.Drawing.Size(72, 19);
            this.checkBoxDaylight.TabIndex = 0;
            this.checkBoxDaylight.Text = "Enabled";
            this.checkBoxDaylight.UseVisualStyleBackColor = true;
            // 
            // dateTimePanel
            // 
            this.dateTimePanel.BackColor = System.Drawing.Color.Transparent;
            this.dateTimePanel.Controls.Add(this.timePicker);
            this.dateTimePanel.Controls.Add(this.datePicker);
            this.dateTimePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePanel.Location = new System.Drawing.Point(12, 58);
            this.dateTimePanel.Name = "dateTimePanel";
            this.dateTimePanel.Size = new System.Drawing.Size(456, 40);
            this.dateTimePanel.TabIndex = 19;
            this.dateTimePanel.Tag = "Time";
            // 
            // timePicker
            // 
            this.timePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timePicker.CustomFormat = "HH:mm:ss";
            this.timePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePicker.Location = new System.Drawing.Point(372, 13);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(72, 21);
            this.timePicker.TabIndex = 1;
            // 
            // datePicker
            // 
            this.datePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.datePicker.CustomFormat = "yyyy-MM-dd";
            this.datePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(263, 13);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(103, 21);
            this.datePicker.TabIndex = 0;
            // 
            // timeZonePanel
            // 
            this.timeZonePanel.BackColor = System.Drawing.Color.Transparent;
            this.timeZonePanel.Controls.Add(this.zoneComboBox);
            this.timeZonePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.timeZonePanel.Location = new System.Drawing.Point(12, 18);
            this.timeZonePanel.Name = "timeZonePanel";
            this.timeZonePanel.Size = new System.Drawing.Size(456, 40);
            this.timeZonePanel.TabIndex = 18;
            this.timeZonePanel.Tag = "TimeZone";
            // 
            // zoneComboBox
            // 
            this.zoneComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoneComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoneComboBox.FormattingEnabled = true;
            this.zoneComboBox.Location = new System.Drawing.Point(24, 10);
            this.zoneComboBox.Name = "zoneComboBox";
            this.zoneComboBox.Size = new System.Drawing.Size(420, 23);
            this.zoneComboBox.TabIndex = 0;
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNote.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelNote.Location = new System.Drawing.Point(12, 185);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(314, 15);
            this.labelNote.TabIndex = 22;
            this.labelNote.Text = "If you manaully set the time, NTP server will be disabled.";
            // 
            // TimeControl
            // 
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.containerPanel);
            this.Name = "TimeControl";
            this.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.ResumeLayout(false);
            this.NTPServerPanel.ResumeLayout(false);
            this.NTPServerPanel.PerformLayout();
            this.daylightPanel.ResumeLayout(false);
            this.daylightPanel.PerformLayout();
            this.dateTimePanel.ResumeLayout(false);
            this.timeZonePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel NTPServerPanel;
        private PanelBase.DoubleBufferPanel daylightPanel;
        private PanelBase.DoubleBufferPanel dateTimePanel;
        private PanelBase.DoubleBufferPanel timeZonePanel;
        private System.Windows.Forms.CheckBox checkBoxNTPServer;
        private System.Windows.Forms.TextBox textBoxNTPServer;
        private System.Windows.Forms.CheckBox checkBoxDaylight;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.ComboBox zoneComboBox;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.Label labelNote;

        
    }
}
