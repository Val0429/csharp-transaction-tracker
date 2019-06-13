namespace Interrogation
{
    partial class Information
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Information));
            this.recordTimer = new System.Windows.Forms.Timer(this.components);
            this.controlPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.actionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.numberDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.numberLabel = new System.Windows.Forms.Label();
            this.noofRecordTextBox = new System.Windows.Forms.TextBox();
            this.dateDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dateLabel = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.nameDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.toolPanel = new PanelBase.DoubleBufferPanel();
            this.informationLabel = new PanelBase.DoubleBufferLabel();
            this.controlPanel.SuspendLayout();
            this.actionDoubleBufferPanel.SuspendLayout();
            this.numberDoubleBufferPanel.SuspendLayout();
            this.dateDoubleBufferPanel.SuspendLayout();
            this.nameDoubleBufferPanel.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // recordTimer
            // 
            this.recordTimer.Interval = 1000;
            this.recordTimer.Tick += new System.EventHandler(this.RecordTimerTick);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.actionDoubleBufferPanel);
            this.controlPanel.Controls.Add(this.label1);
            this.controlPanel.Controls.Add(this.numberDoubleBufferPanel);
            this.controlPanel.Controls.Add(this.dateDoubleBufferPanel);
            this.controlPanel.Controls.Add(this.nameDoubleBufferPanel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel.Location = new System.Drawing.Point(0, 32);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.controlPanel.Size = new System.Drawing.Size(706, 192);
            this.controlPanel.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(682, 7);
            this.label1.TabIndex = 34;
            // 
            // actionDoubleBufferPanel
            // 
            this.actionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.actionDoubleBufferPanel.Controls.Add(this.stopButton);
            this.actionDoubleBufferPanel.Controls.Add(this.startButton);
            this.actionDoubleBufferPanel.Controls.Add(this.timeLabel);
            this.actionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.actionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionDoubleBufferPanel.Location = new System.Drawing.Point(12, 131);
            this.actionDoubleBufferPanel.Name = "actionDoubleBufferPanel";
            this.actionDoubleBufferPanel.Size = new System.Drawing.Size(682, 40);
            this.actionDoubleBufferPanel.TabIndex = 33;
            this.actionDoubleBufferPanel.Tag = "NoofRecord";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stopButton.BackgroundImage")));
            this.stopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.stopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopButton.Enabled = false;
            this.stopButton.FlatAppearance.BorderSize = 0;
            this.stopButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopButton.Location = new System.Drawing.Point(504, 0);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(150, 40);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // startButton
            // 
            this.startButton.BackgroundImage = global::Interrogation.Properties.Resources.startRecord;
            this.startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startButton.Enabled = false;
            this.startButton.FlatAppearance.BorderSize = 0;
            this.startButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startButton.Location = new System.Drawing.Point(25, 0);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(150, 40);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Save && Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // timeLabel
            // 
            this.timeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.ForeColor = System.Drawing.Color.White;
            this.timeLabel.Location = new System.Drawing.Point(0, 0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(682, 40);
            this.timeLabel.TabIndex = 4;
            this.timeLabel.Text = "Recording Time 00:00:00";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numberDoubleBufferPanel
            // 
            this.numberDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.numberDoubleBufferPanel.Controls.Add(this.numberLabel);
            this.numberDoubleBufferPanel.Controls.Add(this.noofRecordTextBox);
            //this.numberDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numberDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.numberDoubleBufferPanel.Location = new System.Drawing.Point(12, 84);
            this.numberDoubleBufferPanel.Name = "numberDoubleBufferPanel";
            this.numberDoubleBufferPanel.Size = new System.Drawing.Size(682, 40);
            this.numberDoubleBufferPanel.TabIndex = 32;
            this.numberDoubleBufferPanel.Tag = "NoOfRecord";
            // 
            // numberLabel
            // 
            this.numberLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.numberLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.numberLabel.Location = new System.Drawing.Point(0, 0);
            this.numberLabel.Name = "numberLabel";
            this.numberLabel.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.numberLabel.Size = new System.Drawing.Size(168, 40);
            this.numberLabel.TabIndex = 4;
            this.numberLabel.Text = "No of Record";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // noofRecordTextBox
            // 
            this.noofRecordTextBox.Enabled = false;
            this.noofRecordTextBox.Location = new System.Drawing.Point(174, 9);
            this.noofRecordTextBox.MaxLength = 6;
            this.noofRecordTextBox.Name = "noofRecordTextBox";
            this.noofRecordTextBox.Size = new System.Drawing.Size(54, 21);
            this.noofRecordTextBox.TabIndex = 2;
            // 
            // dateDoubleBufferPanel
            // 
            this.dateDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.dateDoubleBufferPanel.Controls.Add(this.dateLabel);
            this.dateDoubleBufferPanel.Controls.Add(this.datePicker);
            //this.dateDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateDoubleBufferPanel.Location = new System.Drawing.Point(12, 44);
            this.dateDoubleBufferPanel.Name = "dateDoubleBufferPanel";
            this.dateDoubleBufferPanel.Size = new System.Drawing.Size(682, 40);
            this.dateDoubleBufferPanel.TabIndex = 31;
            this.dateDoubleBufferPanel.Tag = "Date";
            // 
            // dateLabel
            // 
            this.dateLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.dateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.dateLabel.Location = new System.Drawing.Point(0, 0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.dateLabel.Size = new System.Drawing.Size(168, 40);
            this.dateLabel.TabIndex = 3;
            this.dateLabel.Text = "Date";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "yyyy-MM-dd";
            this.datePicker.Enabled = false;
            this.datePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(174, 9);
            this.datePicker.Margin = new System.Windows.Forms.Padding(0);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(103, 21);
            this.datePicker.TabIndex = 2;
            this.datePicker.TabStop = false;
            // 
            // nameDoubleBufferPanel
            // 
            this.nameDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.nameDoubleBufferPanel.Controls.Add(this.nameLabel);
            this.nameDoubleBufferPanel.Controls.Add(this.nameTextBox);
            //this.nameDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nameDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameDoubleBufferPanel.Location = new System.Drawing.Point(12, 4);
            this.nameDoubleBufferPanel.Name = "nameDoubleBufferPanel";
            this.nameDoubleBufferPanel.Size = new System.Drawing.Size(682, 40);
            this.nameDoubleBufferPanel.TabIndex = 30;
            this.nameDoubleBufferPanel.Tag = "Name";
            // 
            // nameLabel
            // 
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.nameLabel.Location = new System.Drawing.Point(0, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.nameLabel.Size = new System.Drawing.Size(168, 40);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Enabled = false;
            this.nameTextBox.Location = new System.Drawing.Point(174, 9);
            this.nameTextBox.MaxLength = 50;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(360, 21);
            this.nameTextBox.TabIndex = 0;
            // 
            // toolPanel
            // 
            this.toolPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolPanel.BackgroundImage")));
            this.toolPanel.Controls.Add(this.informationLabel);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.toolPanel.Size = new System.Drawing.Size(706, 32);
            this.toolPanel.TabIndex = 30;
            // 
            // informationLabel
            // 
            this.informationLabel.AutoSize = true;
            this.informationLabel.BackColor = System.Drawing.Color.Transparent;
            this.informationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.informationLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.informationLabel.Location = new System.Drawing.Point(6, 0);
            this.informationLabel.MinimumSize = new System.Drawing.Size(45, 32);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(45, 32);
            this.informationLabel.TabIndex = 9;
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.toolPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "Information";
            this.Size = new System.Drawing.Size(706, 224);
            this.controlPanel.ResumeLayout(false);
            this.actionDoubleBufferPanel.ResumeLayout(false);
            this.numberDoubleBufferPanel.ResumeLayout(false);
            this.numberDoubleBufferPanel.PerformLayout();
            this.dateDoubleBufferPanel.ResumeLayout(false);
            this.nameDoubleBufferPanel.ResumeLayout(false);
            this.nameDoubleBufferPanel.PerformLayout();
            this.toolPanel.ResumeLayout(false);
            this.toolPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer recordTimer;
        private PanelBase.DoubleBufferPanel toolPanel;
        protected PanelBase.DoubleBufferLabel informationLabel;
        private System.Windows.Forms.Panel controlPanel;
        private PanelBase.DoubleBufferPanel actionDoubleBufferPanel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label timeLabel;
        private PanelBase.DoubleBufferPanel numberDoubleBufferPanel;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.TextBox noofRecordTextBox;
        private PanelBase.DoubleBufferPanel dateDoubleBufferPanel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.DateTimePicker datePicker;
        private PanelBase.DoubleBufferPanel nameDoubleBufferPanel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;

    }
}
