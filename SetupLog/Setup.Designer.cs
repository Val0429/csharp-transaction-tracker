namespace SetupLog
{
	partial class Setup
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
            this.contentPanel = new PanelBase.DoubleBufferPanel();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.logDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.titlePanel = new PanelBase.DoubleBufferPanel();
            this.scrollDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.resultrPanel = new PanelBase.DoubleBufferPanel();
            this.foundLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.logFilterPanel = new PanelBase.DoubleBufferPanel();
            this.filterUserDescComboBox = new System.Windows.Forms.ComboBox();
            this.filterUserLogComboBox = new System.Windows.Forms.ComboBox();
            this.filterUserLabel = new System.Windows.Forms.Label();
            this.filterSystemLabel = new System.Windows.Forms.Label();
            this.filterSystemDescComboBox = new System.Windows.Forms.ComboBox();
            this.filterSystemLogComboBox = new System.Windows.Forms.ComboBox();
            this.logPanel = new PanelBase.DoubleBufferPanel();
            this.logToolPanel = new System.Windows.Forms.Panel();
            this.actionPictureBox = new System.Windows.Forms.PictureBox();
            this.serverPictureBox = new System.Windows.Forms.PictureBox();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.contentPanel.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.resultrPanel.SuspendLayout();
            this.logFilterPanel.SuspendLayout();
            this.logPanel.SuspendLayout();
            this.logToolPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.contentPanel.Controls.Add(this.containerPanel);
            this.contentPanel.Controls.Add(this.resultrPanel);
            this.contentPanel.Controls.Add(this.logFilterPanel);
            this.contentPanel.Controls.Add(this.logPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(3, 3);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.contentPanel.Size = new System.Drawing.Size(647, 402);
            this.contentPanel.TabIndex = 7;
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.logDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.titlePanel);
            this.containerPanel.Controls.Add(this.scrollDoubleBufferPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 153);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(623, 231);
            this.containerPanel.TabIndex = 14;
            // 
            // logDoubleBufferPanel
            // 
            this.logDoubleBufferPanel.AutoScroll = true;
            this.logDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.logDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logDoubleBufferPanel.Location = new System.Drawing.Point(0, 40);
            this.logDoubleBufferPanel.Name = "logDoubleBufferPanel";
            this.logDoubleBufferPanel.Size = new System.Drawing.Size(605, 191);
            this.logDoubleBufferPanel.TabIndex = 15;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.Transparent;
            this.titlePanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(605, 40);
            this.titlePanel.TabIndex = 14;
            // 
            // scrollDoubleBufferPanel
            // 
            this.scrollDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.scrollDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollDoubleBufferPanel.Location = new System.Drawing.Point(605, 0);
            this.scrollDoubleBufferPanel.Name = "scrollDoubleBufferPanel";
            this.scrollDoubleBufferPanel.Size = new System.Drawing.Size(18, 231);
            this.scrollDoubleBufferPanel.TabIndex = 17;
            this.scrollDoubleBufferPanel.Visible = false;
            // 
            // resultrPanel
            // 
            this.resultrPanel.BackColor = System.Drawing.Color.Transparent;
            this.resultrPanel.Controls.Add(this.foundLabel);
            this.resultrPanel.Controls.Add(this.resultLabel);
            this.resultrPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultrPanel.Location = new System.Drawing.Point(12, 128);
            this.resultrPanel.Name = "resultrPanel";
            this.resultrPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.resultrPanel.Size = new System.Drawing.Size(623, 25);
            this.resultrPanel.TabIndex = 16;
            // 
            // foundLabel
            // 
            this.foundLabel.AutoSize = true;
            this.foundLabel.BackColor = System.Drawing.Color.Transparent;
            this.foundLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.foundLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(177)))), ((int)(((byte)(224)))));
            this.foundLabel.Location = new System.Drawing.Point(64, 0);
            this.foundLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new System.Drawing.Size(52, 20);
            this.foundLabel.TabIndex = 7;
            this.foundLabel.Text = "1 Found";
            this.foundLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.resultLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultLabel.Location = new System.Drawing.Point(8, 0);
            this.resultLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.resultLabel.Size = new System.Drawing.Size(56, 20);
            this.resultLabel.TabIndex = 6;
            this.resultLabel.Text = "Result :";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // logFilterPanel
            // 
            this.logFilterPanel.BackColor = System.Drawing.Color.Transparent;
            this.logFilterPanel.Controls.Add(this.filterUserDescComboBox);
            this.logFilterPanel.Controls.Add(this.filterUserLogComboBox);
            this.logFilterPanel.Controls.Add(this.filterUserLabel);
            this.logFilterPanel.Controls.Add(this.filterSystemLabel);
            this.logFilterPanel.Controls.Add(this.filterSystemDescComboBox);
            this.logFilterPanel.Controls.Add(this.filterSystemLogComboBox);
            this.logFilterPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.logFilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logFilterPanel.Location = new System.Drawing.Point(12, 58);
            this.logFilterPanel.Name = "logFilterPanel";
            this.logFilterPanel.Size = new System.Drawing.Size(623, 70);
            this.logFilterPanel.TabIndex = 15;
            // 
            // filterUserDescComboBox
            // 
            this.filterUserDescComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterUserDescComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterUserDescComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterUserDescComboBox.FormattingEnabled = true;
            this.filterUserDescComboBox.IntegralHeight = false;
            this.filterUserDescComboBox.Location = new System.Drawing.Point(254, 38);
            this.filterUserDescComboBox.MaxDropDownItems = 20;
            this.filterUserDescComboBox.Name = "filterUserDescComboBox";
            this.filterUserDescComboBox.Size = new System.Drawing.Size(350, 23);
            this.filterUserDescComboBox.TabIndex = 13;
            // 
            // filterUserLogComboBox
            // 
            this.filterUserLogComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterUserLogComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterUserLogComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterUserLogComboBox.FormattingEnabled = true;
            this.filterUserLogComboBox.IntegralHeight = false;
            this.filterUserLogComboBox.Location = new System.Drawing.Point(76, 38);
            this.filterUserLogComboBox.MaxDropDownItems = 20;
            this.filterUserLogComboBox.Name = "filterUserLogComboBox";
            this.filterUserLogComboBox.Size = new System.Drawing.Size(164, 23);
            this.filterUserLogComboBox.TabIndex = 12;
            // 
            // filterUserLabel
            // 
            this.filterUserLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterUserLabel.Location = new System.Drawing.Point(11, 39);
            this.filterUserLabel.Name = "filterUserLabel";
            this.filterUserLabel.Size = new System.Drawing.Size(60, 23);
            this.filterUserLabel.TabIndex = 11;
            this.filterUserLabel.Text = "User";
            this.filterUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filterSystemLabel
            // 
            this.filterSystemLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterSystemLabel.Location = new System.Drawing.Point(11, 9);
            this.filterSystemLabel.Name = "filterSystemLabel";
            this.filterSystemLabel.Size = new System.Drawing.Size(60, 23);
            this.filterSystemLabel.TabIndex = 10;
            this.filterSystemLabel.Text = "System";
            this.filterSystemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filterSystemDescComboBox
            // 
            this.filterSystemDescComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterSystemDescComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterSystemDescComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterSystemDescComboBox.FormattingEnabled = true;
            this.filterSystemDescComboBox.IntegralHeight = false;
            this.filterSystemDescComboBox.Location = new System.Drawing.Point(254, 9);
            this.filterSystemDescComboBox.MaxDropDownItems = 20;
            this.filterSystemDescComboBox.Name = "filterSystemDescComboBox";
            this.filterSystemDescComboBox.Size = new System.Drawing.Size(350, 23);
            this.filterSystemDescComboBox.TabIndex = 9;
            // 
            // filterSystemLogComboBox
            // 
            this.filterSystemLogComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterSystemLogComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterSystemLogComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterSystemLogComboBox.FormattingEnabled = true;
            this.filterSystemLogComboBox.IntegralHeight = false;
            this.filterSystemLogComboBox.Location = new System.Drawing.Point(76, 9);
            this.filterSystemLogComboBox.MaxDropDownItems = 20;
            this.filterSystemLogComboBox.Name = "filterSystemLogComboBox";
            this.filterSystemLogComboBox.Size = new System.Drawing.Size(164, 23);
            this.filterSystemLogComboBox.TabIndex = 8;
            // 
            // logPanel
            // 
            this.logPanel.BackColor = System.Drawing.Color.Transparent;
            this.logPanel.Controls.Add(this.logToolPanel);
            this.logPanel.Controls.Add(this.datePicker);
            this.logPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.logPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logPanel.Location = new System.Drawing.Point(12, 18);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(623, 40);
            this.logPanel.TabIndex = 7;
            this.logPanel.Tag = "Log";
            // 
            // logToolPanel
            // 
            this.logToolPanel.BackgroundImage = global::SetupLog.Properties.Resources.tool_bg;
            this.logToolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logToolPanel.Controls.Add(this.actionPictureBox);
            this.logToolPanel.Controls.Add(this.serverPictureBox);
            this.logToolPanel.Location = new System.Drawing.Point(199, 8);
            this.logToolPanel.Margin = new System.Windows.Forms.Padding(0);
            this.logToolPanel.Name = "logToolPanel";
            this.logToolPanel.Size = new System.Drawing.Size(45, 22);
            this.logToolPanel.TabIndex = 7;
            // 
            // actionPictureBox
            // 
            this.actionPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.actionPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.actionPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.actionPictureBox.Image = global::SetupLog.Properties.Resources.action_icon;
            this.actionPictureBox.Location = new System.Drawing.Point(23, 0);
            this.actionPictureBox.Name = "actionPictureBox";
            this.actionPictureBox.Size = new System.Drawing.Size(22, 22);
            this.actionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.actionPictureBox.TabIndex = 2;
            this.actionPictureBox.TabStop = false;
            this.actionPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ActionPictureBoxMouseClick);
            // 
            // serverPictureBox
            // 
            this.serverPictureBox.BackgroundImage = global::SetupLog.Properties.Resources.service_bg;
            this.serverPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.serverPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.serverPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.serverPictureBox.Image = global::SetupLog.Properties.Resources.service_icon;
            this.serverPictureBox.Location = new System.Drawing.Point(0, 0);
            this.serverPictureBox.Name = "serverPictureBox";
            this.serverPictureBox.Size = new System.Drawing.Size(23, 22);
            this.serverPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.serverPictureBox.TabIndex = 1;
            this.serverPictureBox.TabStop = false;
            this.serverPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ServerPictureBoxMouseClick);
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "yyyy-MM-dd";
            this.datePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(76, 9);
            this.datePicker.Margin = new System.Windows.Forms.Padding(0);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(103, 21);
            this.datePicker.TabIndex = 1;
            this.datePicker.TabStop = false;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.contentPanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(653, 408);
            this.contentPanel.ResumeLayout(false);
            this.containerPanel.ResumeLayout(false);
            this.resultrPanel.ResumeLayout(false);
            this.resultrPanel.PerformLayout();
            this.logFilterPanel.ResumeLayout(false);
            this.logPanel.ResumeLayout(false);
            this.logToolPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.actionPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverPictureBox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private PanelBase.DoubleBufferPanel contentPanel;
		private PanelBase.DoubleBufferPanel logPanel;
		private System.Windows.Forms.DateTimePicker datePicker;
		protected System.Windows.Forms.Panel logToolPanel;
		private System.Windows.Forms.PictureBox actionPictureBox;
		private System.Windows.Forms.PictureBox serverPictureBox;
		private PanelBase.DoubleBufferPanel containerPanel;
		private PanelBase.DoubleBufferPanel titlePanel;
		private PanelBase.DoubleBufferPanel logDoubleBufferPanel;
		private PanelBase.DoubleBufferPanel scrollDoubleBufferPanel;
		private System.Windows.Forms.ComboBox filterSystemLogComboBox;
		private System.Windows.Forms.ComboBox filterSystemDescComboBox;
		private PanelBase.DoubleBufferPanel logFilterPanel;
		private System.Windows.Forms.Label filterSystemLabel;
		private System.Windows.Forms.Label filterUserLabel;
		private System.Windows.Forms.ComboBox filterUserLogComboBox;
		private System.Windows.Forms.ComboBox filterUserDescComboBox;
		private PanelBase.DoubleBufferPanel resultrPanel;
		private System.Windows.Forms.Label foundLabel;
		private System.Windows.Forms.Label resultLabel;

	}
}
