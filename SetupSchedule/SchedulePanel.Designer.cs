namespace SetupSchedule
{
	partial class SchedulePanel
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
			this.tableLayoutPanel = new PanelBase.DoubleBufferPanel();
			this.eventSchedulePanel = new PanelBase.DoubleBufferPanel();
			this.quickSetEventComboBox = new System.Windows.Forms.ComboBox();
			this.eventToolPanel = new System.Windows.Forms.Panel();
			this.eraserEventPictureBox = new System.Windows.Forms.PictureBox();
			this.eventHandlingPictureBox = new System.Windows.Forms.PictureBox();
			this.eventLabel = new System.Windows.Forms.Label();
			this.durationPanel = new PanelBase.DoubleBufferPanel();
			this.postDurationPanel = new PanelBase.DoubleBufferPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.postRecordTextBox = new PanelBase.HotKeyTextBox();
			this.postLabel = new System.Windows.Forms.Label();
			this.preDurationPanel = new PanelBase.DoubleBufferPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.preRecordTextBox = new PanelBase.HotKeyTextBox();
			this.previousLabel = new System.Windows.Forms.Label();
			this.durationLabel = new System.Windows.Forms.Label();
			this.recordSchedulePanel = new PanelBase.DoubleBufferPanel();
			this.recordLabel = new System.Windows.Forms.Label();
			this.filledEventCheckBox = new System.Windows.Forms.CheckBox();
			this.quickSetRecordComboBox = new System.Windows.Forms.ComboBox();
			this.recordToolPanel = new System.Windows.Forms.Panel();
			this.eraserPictureBox = new System.Windows.Forms.PictureBox();
			this.eventPictureBox = new System.Windows.Forms.PictureBox();
			this.recordPictureBox = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel.SuspendLayout();
			this.eventSchedulePanel.SuspendLayout();
			this.eventToolPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.eraserEventPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eventHandlingPictureBox)).BeginInit();
			this.durationPanel.SuspendLayout();
			this.postDurationPanel.SuspendLayout();
			this.preDurationPanel.SuspendLayout();
			this.recordSchedulePanel.SuspendLayout();
			this.recordLabel.SuspendLayout();
			this.recordToolPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.eraserPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eventPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.recordPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel.Controls.Add(this.eventSchedulePanel);
			this.tableLayoutPanel.Controls.Add(this.durationPanel);
			this.tableLayoutPanel.Controls.Add(this.recordSchedulePanel);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(12, 18);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.Size = new System.Drawing.Size(671, 624);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// eventSchedulePanel
			// 
			this.eventSchedulePanel.BackColor = System.Drawing.Color.Transparent;
			this.eventSchedulePanel.Controls.Add(this.quickSetEventComboBox);
			this.eventSchedulePanel.Controls.Add(this.eventToolPanel);
			this.eventSchedulePanel.Controls.Add(this.eventLabel);
			this.eventSchedulePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.eventSchedulePanel.Location = new System.Drawing.Point(0, 338);
			this.eventSchedulePanel.Margin = new System.Windows.Forms.Padding(0);
			this.eventSchedulePanel.Name = "eventSchedulePanel";
			this.eventSchedulePanel.Size = new System.Drawing.Size(671, 268);
			this.eventSchedulePanel.TabIndex = 1;
			// 
			// quickSetEventComboBox
			// 
			this.quickSetEventComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.quickSetEventComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.quickSetEventComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.quickSetEventComboBox.FormattingEnabled = true;
			this.quickSetEventComboBox.IntegralHeight = false;
			this.quickSetEventComboBox.Location = new System.Drawing.Point(210, 1);
			this.quickSetEventComboBox.MaxDropDownItems = 20;
			this.quickSetEventComboBox.Name = "quickSetEventComboBox";
			this.quickSetEventComboBox.Size = new System.Drawing.Size(215, 23);
			this.quickSetEventComboBox.TabIndex = 7;
			// 
			// eventToolPanel
			// 
			this.eventToolPanel.BackgroundImage = global::SetupSchedule.Properties.Resources.tool_bg2;
			this.eventToolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.eventToolPanel.Controls.Add(this.eraserEventPictureBox);
			this.eventToolPanel.Controls.Add(this.eventHandlingPictureBox);
			this.eventToolPanel.Location = new System.Drawing.Point(150, 2);
			this.eventToolPanel.Margin = new System.Windows.Forms.Padding(0);
			this.eventToolPanel.Name = "eventToolPanel";
			this.eventToolPanel.Size = new System.Drawing.Size(45, 22);
			this.eventToolPanel.TabIndex = 6;
			// 
			// eraserEventPictureBox
			// 
			this.eraserEventPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.eraserEventPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.eraserEventPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.eraserEventPictureBox.Image = global::SetupSchedule.Properties.Resources.eraser_icon;
			this.eraserEventPictureBox.Location = new System.Drawing.Point(23, 0);
			this.eraserEventPictureBox.Name = "eraserEventPictureBox";
			this.eraserEventPictureBox.Size = new System.Drawing.Size(22, 22);
			this.eraserEventPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.eraserEventPictureBox.TabIndex = 2;
			this.eraserEventPictureBox.TabStop = false;
			this.eraserEventPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EraserEventPictureBoxMouseClick);
			// 
			// eventHandlingPictureBox
			// 
			this.eventHandlingPictureBox.BackgroundImage = global::SetupSchedule.Properties.Resources.record_bg;
			this.eventHandlingPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.eventHandlingPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.eventHandlingPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.eventHandlingPictureBox.Image = global::SetupSchedule.Properties.Resources.event_icon;
			this.eventHandlingPictureBox.Location = new System.Drawing.Point(0, 0);
			this.eventHandlingPictureBox.Name = "eventHandlingPictureBox";
			this.eventHandlingPictureBox.Size = new System.Drawing.Size(23, 22);
			this.eventHandlingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.eventHandlingPictureBox.TabIndex = 1;
			this.eventHandlingPictureBox.TabStop = false;
			this.eventHandlingPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EventHandlingPictureBoxMouseClick);
			// 
			// eventLabel
			// 
			this.eventLabel.BackColor = System.Drawing.Color.Transparent;
			this.eventLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.eventLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.eventLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.eventLabel.Location = new System.Drawing.Point(0, 0);
			this.eventLabel.Name = "eventLabel";
			this.eventLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
			this.eventLabel.Size = new System.Drawing.Size(671, 28);
			this.eventLabel.TabIndex = 5;
			this.eventLabel.Text = "Event Handling";
			this.eventLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// durationPanel
			// 
			this.durationPanel.BackColor = System.Drawing.Color.Transparent;
			this.durationPanel.Controls.Add(this.postDurationPanel);
			this.durationPanel.Controls.Add(this.preDurationPanel);
			this.durationPanel.Controls.Add(this.durationLabel);
			this.durationPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.durationPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.durationPanel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.durationPanel.Location = new System.Drawing.Point(0, 268);
			this.durationPanel.Name = "durationPanel";
			this.durationPanel.Size = new System.Drawing.Size(671, 70);
			this.durationPanel.TabIndex = 10;
			// 
			// postDurationPanel
			// 
			this.postDurationPanel.BackColor = System.Drawing.Color.Transparent;
			this.postDurationPanel.Controls.Add(this.label2);
			this.postDurationPanel.Controls.Add(this.postRecordTextBox);
			this.postDurationPanel.Controls.Add(this.postLabel);
			this.postDurationPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.postDurationPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.postDurationPanel.ForeColor = System.Drawing.Color.DimGray;
			this.postDurationPanel.Location = new System.Drawing.Point(192, 30);
			this.postDurationPanel.Name = "postDurationPanel";
			this.postDurationPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.postDurationPanel.Size = new System.Drawing.Size(479, 30);
			this.postDurationPanel.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.postDurationPanel.TabIndex = 12;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label2.Location = new System.Drawing.Point(170, 5);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
			this.label2.Size = new System.Drawing.Size(303, 18);
			this.label2.TabIndex = 13;
			this.label2.Text = "(Duration should between %1 - %2 secs)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// postRecordTextBox
			// 
			this.postRecordTextBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.postRecordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.postRecordTextBox.Location = new System.Drawing.Point(125, 5);
			this.postRecordTextBox.MaxLength = 5;
			this.postRecordTextBox.Name = "postRecordTextBox";
			this.postRecordTextBox.Size = new System.Drawing.Size(45, 21);
			this.postRecordTextBox.TabIndex = 12;
			// 
			// postLabel
			// 
			this.postLabel.AutoSize = true;
			this.postLabel.BackColor = System.Drawing.Color.Transparent;
			this.postLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.postLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.postLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.postLabel.Location = new System.Drawing.Point(0, 5);
			this.postLabel.Name = "postLabel";
			this.postLabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.postLabel.Size = new System.Drawing.Size(125, 18);
			this.postLabel.TabIndex = 11;
			this.postLabel.Text = "Post-event Recording";
			this.postLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// preDurationPanel
			// 
			this.preDurationPanel.BackColor = System.Drawing.Color.Transparent;
			this.preDurationPanel.Controls.Add(this.label1);
			this.preDurationPanel.Controls.Add(this.preRecordTextBox);
			this.preDurationPanel.Controls.Add(this.previousLabel);
			this.preDurationPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.preDurationPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.preDurationPanel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.preDurationPanel.Location = new System.Drawing.Point(192, 0);
			this.preDurationPanel.Name = "preDurationPanel";
			this.preDurationPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.preDurationPanel.Size = new System.Drawing.Size(479, 30);
			this.preDurationPanel.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label1.Location = new System.Drawing.Point(164, 5);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
			this.label1.Size = new System.Drawing.Size(303, 18);
			this.label1.TabIndex = 12;
			this.label1.Text = "(Duration should between %1 - %2 secs)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// preRecordTextBox
			// 
			this.preRecordTextBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.preRecordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.preRecordTextBox.Location = new System.Drawing.Point(119, 5);
			this.preRecordTextBox.MaxLength = 5;
			this.preRecordTextBox.Name = "preRecordTextBox";
			this.preRecordTextBox.Size = new System.Drawing.Size(45, 21);
			this.preRecordTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.preRecordTextBox.TabIndex = 9;
			// 
			// previousLabel
			// 
			this.previousLabel.AutoSize = true;
			this.previousLabel.BackColor = System.Drawing.Color.Transparent;
			this.previousLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.previousLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.previousLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.previousLabel.Location = new System.Drawing.Point(0, 5);
			this.previousLabel.Name = "previousLabel";
			this.previousLabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.previousLabel.Size = new System.Drawing.Size(119, 18);
			this.previousLabel.TabIndex = 10;
			this.previousLabel.Text = "Pre-event Recording";
			this.previousLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// durationLabel
			// 
			this.durationLabel.AutoSize = true;
			this.durationLabel.BackColor = System.Drawing.Color.Transparent;
			this.durationLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.durationLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.durationLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.durationLabel.Location = new System.Drawing.Point(0, 0);
			this.durationLabel.MinimumSize = new System.Drawing.Size(0, 60);
			this.durationLabel.Name = "durationLabel";
			this.durationLabel.Padding = new System.Windows.Forms.Padding(8, 3, 8, 0);
			this.durationLabel.Size = new System.Drawing.Size(192, 60);
			this.durationLabel.TabIndex = 10;
			this.durationLabel.Text = "Event Recording Duration(Sec)";
			this.durationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// recordSchedulePanel
			// 
			this.recordSchedulePanel.BackColor = System.Drawing.Color.Transparent;
			this.recordSchedulePanel.Controls.Add(this.recordLabel);
			this.recordSchedulePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.recordSchedulePanel.Location = new System.Drawing.Point(0, 0);
			this.recordSchedulePanel.Margin = new System.Windows.Forms.Padding(0);
			this.recordSchedulePanel.Name = "recordSchedulePanel";
			this.recordSchedulePanel.Size = new System.Drawing.Size(671, 268);
			this.recordSchedulePanel.TabIndex = 0;
			// 
			// recordLabel
			// 
			this.recordLabel.BackColor = System.Drawing.Color.Transparent;
			this.recordLabel.Controls.Add(this.filledEventCheckBox);
			this.recordLabel.Controls.Add(this.quickSetRecordComboBox);
			this.recordLabel.Controls.Add(this.recordToolPanel);
			this.recordLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.recordLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.recordLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.recordLabel.Location = new System.Drawing.Point(0, 0);
			this.recordLabel.Name = "recordLabel";
			this.recordLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
			this.recordLabel.Size = new System.Drawing.Size(671, 28);
			this.recordLabel.TabIndex = 4;
			this.recordLabel.Text = "Video Recording";
			this.recordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// filledEventCheckBox
			// 
			this.filledEventCheckBox.AutoSize = true;
			this.filledEventCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.filledEventCheckBox.Location = new System.Drawing.Point(435, 5);
			this.filledEventCheckBox.Name = "filledEventCheckBox";
			this.filledEventCheckBox.Size = new System.Drawing.Size(207, 19);
			this.filledEventCheckBox.TabIndex = 7;
			this.filledEventCheckBox.Text = "Fill the blank with event recording";
			this.filledEventCheckBox.UseVisualStyleBackColor = true;
			// 
			// quickSetRecordComboBox
			// 
			this.quickSetRecordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.quickSetRecordComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.quickSetRecordComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.quickSetRecordComboBox.FormattingEnabled = true;
			this.quickSetRecordComboBox.IntegralHeight = false;
			this.quickSetRecordComboBox.Location = new System.Drawing.Point(210, 1);
			this.quickSetRecordComboBox.MaxDropDownItems = 20;
			this.quickSetRecordComboBox.Name = "quickSetRecordComboBox";
			this.quickSetRecordComboBox.Size = new System.Drawing.Size(215, 23);
			this.quickSetRecordComboBox.TabIndex = 6;
			// 
			// recordToolPanel
			// 
			this.recordToolPanel.BackgroundImage = global::SetupSchedule.Properties.Resources.tool_bg;
			this.recordToolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.recordToolPanel.Controls.Add(this.eraserPictureBox);
			this.recordToolPanel.Controls.Add(this.eventPictureBox);
			this.recordToolPanel.Controls.Add(this.recordPictureBox);
			this.recordToolPanel.Location = new System.Drawing.Point(130, 2);
			this.recordToolPanel.Margin = new System.Windows.Forms.Padding(0);
			this.recordToolPanel.Name = "recordToolPanel";
			this.recordToolPanel.Size = new System.Drawing.Size(69, 22);
			this.recordToolPanel.TabIndex = 5;
			// 
			// eraserPictureBox
			// 
			this.eraserPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.eraserPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.eraserPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.eraserPictureBox.Image = global::SetupSchedule.Properties.Resources.eraser_icon;
			this.eraserPictureBox.Location = new System.Drawing.Point(47, 0);
			this.eraserPictureBox.Name = "eraserPictureBox";
			this.eraserPictureBox.Size = new System.Drawing.Size(22, 22);
			this.eraserPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.eraserPictureBox.TabIndex = 2;
			this.eraserPictureBox.TabStop = false;
			this.eraserPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EraserPictureBoxMouseClick);
			// 
			// eventPictureBox
			// 
			this.eventPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.eventPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.eventPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.eventPictureBox.Image = global::SetupSchedule.Properties.Resources.alarm_icon;
			this.eventPictureBox.Location = new System.Drawing.Point(23, 0);
			this.eventPictureBox.Name = "eventPictureBox";
			this.eventPictureBox.Size = new System.Drawing.Size(24, 22);
			this.eventPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.eventPictureBox.TabIndex = 1;
			this.eventPictureBox.TabStop = false;
			this.eventPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EventPictureBoxMouseClick);
			// 
			// recordPictureBox
			// 
			this.recordPictureBox.BackgroundImage = global::SetupSchedule.Properties.Resources.record_bg;
			this.recordPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.recordPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.recordPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.recordPictureBox.Image = global::SetupSchedule.Properties.Resources.record_icon;
			this.recordPictureBox.Location = new System.Drawing.Point(0, 0);
			this.recordPictureBox.Name = "recordPictureBox";
			this.recordPictureBox.Size = new System.Drawing.Size(23, 22);
			this.recordPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.recordPictureBox.TabIndex = 0;
			this.recordPictureBox.TabStop = false;
			this.recordPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RecordPictureBoxMouseClick);
			// 
			// SchedulePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.tableLayoutPanel);
			this.DoubleBuffered = true;
			this.Name = "SchedulePanel";
			this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
			this.Size = new System.Drawing.Size(695, 660);
			this.tableLayoutPanel.ResumeLayout(false);
			this.eventSchedulePanel.ResumeLayout(false);
			this.eventToolPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.eraserEventPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eventHandlingPictureBox)).EndInit();
			this.durationPanel.ResumeLayout(false);
			this.durationPanel.PerformLayout();
			this.postDurationPanel.ResumeLayout(false);
			this.postDurationPanel.PerformLayout();
			this.preDurationPanel.ResumeLayout(false);
			this.preDurationPanel.PerformLayout();
			this.recordSchedulePanel.ResumeLayout(false);
			this.recordLabel.ResumeLayout(false);
			this.recordLabel.PerformLayout();
			this.recordToolPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.eraserPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eventPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.recordPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private PanelBase.DoubleBufferPanel tableLayoutPanel;
		private PanelBase.DoubleBufferPanel recordSchedulePanel;
		private PanelBase.DoubleBufferPanel eventSchedulePanel;
		private System.Windows.Forms.Label eventLabel;
		private System.Windows.Forms.Panel eventToolPanel;
		private System.Windows.Forms.PictureBox eraserEventPictureBox;
		private System.Windows.Forms.PictureBox eventHandlingPictureBox;
		private System.Windows.Forms.ComboBox quickSetEventComboBox;
		private System.Windows.Forms.Label recordLabel;
		private System.Windows.Forms.CheckBox filledEventCheckBox;
		private System.Windows.Forms.ComboBox quickSetRecordComboBox;
		private System.Windows.Forms.Panel recordToolPanel;
		private System.Windows.Forms.PictureBox eraserPictureBox;
		private System.Windows.Forms.PictureBox eventPictureBox;
		private System.Windows.Forms.PictureBox recordPictureBox;
		private PanelBase.DoubleBufferPanel durationPanel;
		private System.Windows.Forms.Label durationLabel;
		private PanelBase.DoubleBufferPanel postDurationPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox postRecordTextBox;
		private System.Windows.Forms.Label postLabel;
		private PanelBase.DoubleBufferPanel preDurationPanel;
		public System.Windows.Forms.Label label1;
		protected System.Windows.Forms.TextBox preRecordTextBox;
		private System.Windows.Forms.Label previousLabel;

	}
}
