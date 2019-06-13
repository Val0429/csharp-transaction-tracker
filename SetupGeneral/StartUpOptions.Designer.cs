namespace SetupGeneral
{
	sealed partial class StartupOptions
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
            this.groupPanel = new PanelBase.DoubleBufferPanel();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.patrolDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.patrolCheckBox = new System.Windows.Forms.CheckBox();
            this.totalBitratePanel = new PanelBase.DoubleBufferPanel();
            this.totalBitrateComboBox = new System.Windows.Forms.ComboBox();
            this.hidePanelPanel = new PanelBase.DoubleBufferPanel();
            this.hidePanelCheckBox = new System.Windows.Forms.CheckBox();
            this.fullscreenPanel = new PanelBase.DoubleBufferPanel();
            this.fullScreenCheckBox = new System.Windows.Forms.CheckBox();
            this.restoreClientPanel = new PanelBase.DoubleBufferPanel();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.titleBarPanel = new PanelBase.DoubleBufferPanel();
            this.videoTitleBarCheckBox = new System.Windows.Forms.CheckBox();
            this.containerPanel.SuspendLayout();
            this.groupPanel.SuspendLayout();
            this.patrolDoubleBufferPanel.SuspendLayout();
            this.totalBitratePanel.SuspendLayout();
            this.hidePanelPanel.SuspendLayout();
            this.fullscreenPanel.SuspendLayout();
            this.restoreClientPanel.SuspendLayout();
            this.titleBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.groupPanel);
            this.containerPanel.Controls.Add(this.patrolDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.totalBitratePanel);
            this.containerPanel.Controls.Add(this.hidePanelPanel);
            this.containerPanel.Controls.Add(this.fullscreenPanel);
            this.containerPanel.Controls.Add(this.titleBarPanel);
            this.containerPanel.Controls.Add(this.restoreClientPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(555, 494);
            this.containerPanel.TabIndex = 1;
            // 
            // groupPanel
            // 
            this.groupPanel.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel.Controls.Add(this.groupComboBox);
            this.groupPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel.Location = new System.Drawing.Point(0, 200);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(555, 40);
            this.groupPanel.TabIndex = 33;
            this.groupPanel.Tag = "GroupView";
            // 
            // groupComboBox
            // 
            this.groupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.IntegralHeight = false;
            this.groupComboBox.Location = new System.Drawing.Point(390, 8);
            this.groupComboBox.MaxDropDownItems = 20;
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(150, 23);
            this.groupComboBox.TabIndex = 1;
            this.groupComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupComboBoxSelectedIndexChanged);
            // 
            // patrolDoubleBufferPanel
            // 
            this.patrolDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.patrolDoubleBufferPanel.Controls.Add(this.patrolCheckBox);
            this.patrolDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.patrolDoubleBufferPanel.Location = new System.Drawing.Point(0, 160);
            this.patrolDoubleBufferPanel.Name = "patrolDoubleBufferPanel";
            this.patrolDoubleBufferPanel.Size = new System.Drawing.Size(555, 40);
            this.patrolDoubleBufferPanel.TabIndex = 32;
            this.patrolDoubleBufferPanel.Tag = "ViewTour";
            // 
            // patrolCheckBox
            // 
            this.patrolCheckBox.AutoSize = true;
            this.patrolCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.patrolCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patrolCheckBox.Location = new System.Drawing.Point(459, 0);
            this.patrolCheckBox.Name = "patrolCheckBox";
            this.patrolCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.patrolCheckBox.Size = new System.Drawing.Size(96, 40);
            this.patrolCheckBox.TabIndex = 1;
            this.patrolCheckBox.Text = "Enabled";
            this.patrolCheckBox.UseVisualStyleBackColor = true;
            this.patrolCheckBox.Click += new System.EventHandler(this.PatrolCheckBoxClick);
            // 
            // totalBitratePanel
            // 
            this.totalBitratePanel.BackColor = System.Drawing.Color.Transparent;
            this.totalBitratePanel.Controls.Add(this.totalBitrateComboBox);
            this.totalBitratePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.totalBitratePanel.Location = new System.Drawing.Point(0, 120);
            this.totalBitratePanel.Name = "totalBitratePanel";
            this.totalBitratePanel.Size = new System.Drawing.Size(555, 40);
            this.totalBitratePanel.TabIndex = 37;
            this.totalBitratePanel.Tag = "TotalBitrate";
            // 
            // totalBitrateComboBox
            // 
            this.totalBitrateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalBitrateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.totalBitrateComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.totalBitrateComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalBitrateComboBox.FormattingEnabled = true;
            this.totalBitrateComboBox.IntegralHeight = false;
            this.totalBitrateComboBox.Location = new System.Drawing.Point(390, 8);
            this.totalBitrateComboBox.MaxDropDownItems = 20;
            this.totalBitrateComboBox.Name = "totalBitrateComboBox";
            this.totalBitrateComboBox.Size = new System.Drawing.Size(150, 23);
            this.totalBitrateComboBox.TabIndex = 2;
            this.totalBitrateComboBox.SelectedIndexChanged += new System.EventHandler(this.TotalBitrateComboBoxSelectedIndexChanged);
            // 
            // hidePanelPanel
            // 
            this.hidePanelPanel.BackColor = System.Drawing.Color.Transparent;
            this.hidePanelPanel.Controls.Add(this.hidePanelCheckBox);
            this.hidePanelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hidePanelPanel.Location = new System.Drawing.Point(0, 80);
            this.hidePanelPanel.Name = "hidePanelPanel";
            this.hidePanelPanel.Size = new System.Drawing.Size(555, 40);
            this.hidePanelPanel.TabIndex = 36;
            this.hidePanelPanel.Tag = "HidePanel";
            // 
            // hidePanelCheckBox
            // 
            this.hidePanelCheckBox.AutoSize = true;
            this.hidePanelCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.hidePanelCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hidePanelCheckBox.Location = new System.Drawing.Point(479, 0);
            this.hidePanelCheckBox.Name = "hidePanelCheckBox";
            this.hidePanelCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.hidePanelCheckBox.Size = new System.Drawing.Size(76, 40);
            this.hidePanelCheckBox.TabIndex = 2;
            this.hidePanelCheckBox.Text = "Hide";
            this.hidePanelCheckBox.UseVisualStyleBackColor = true;
            this.hidePanelCheckBox.Click += new System.EventHandler(this.HidePanelCheckBoxClick);
            // 
            // fullscreenPanel
            // 
            this.fullscreenPanel.BackColor = System.Drawing.Color.Transparent;
            this.fullscreenPanel.Controls.Add(this.fullScreenCheckBox);
            this.fullscreenPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fullscreenPanel.Location = new System.Drawing.Point(0, 40);
            this.fullscreenPanel.Name = "fullscreenPanel";
            this.fullscreenPanel.Size = new System.Drawing.Size(555, 40);
            this.fullscreenPanel.TabIndex = 34;
            this.fullscreenPanel.Tag = "FullScreen";
            // 
            // fullScreenCheckBox
            // 
            this.fullScreenCheckBox.AutoSize = true;
            this.fullScreenCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.fullScreenCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fullScreenCheckBox.Location = new System.Drawing.Point(459, 0);
            this.fullScreenCheckBox.Name = "fullScreenCheckBox";
            this.fullScreenCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.fullScreenCheckBox.Size = new System.Drawing.Size(96, 40);
            this.fullScreenCheckBox.TabIndex = 2;
            this.fullScreenCheckBox.Text = "Enabled";
            this.fullScreenCheckBox.UseVisualStyleBackColor = true;
            this.fullScreenCheckBox.Click += new System.EventHandler(this.FullScreenCheckBoxClick);
            // 
            // restoreClientPanel
            // 
            this.restoreClientPanel.BackColor = System.Drawing.Color.Transparent;
            this.restoreClientPanel.Controls.Add(this.enabledCheckBox);
            this.restoreClientPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.restoreClientPanel.Location = new System.Drawing.Point(0, 0);
            this.restoreClientPanel.Name = "restoreClientPanel";
            this.restoreClientPanel.Size = new System.Drawing.Size(555, 40);
            this.restoreClientPanel.TabIndex = 26;
            this.restoreClientPanel.Tag = "StartupOptions";
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.enabledCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enabledCheckBox.Location = new System.Drawing.Point(459, 0);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.enabledCheckBox.Size = new System.Drawing.Size(96, 40);
            this.enabledCheckBox.TabIndex = 1;
            this.enabledCheckBox.Text = "Enabled";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.Click += new System.EventHandler(this.EnabledCheckBoxClick);
            // 
            // titleBarPanel
            // 
            this.titleBarPanel.BackColor = System.Drawing.Color.Transparent;
            this.titleBarPanel.Controls.Add(this.videoTitleBarCheckBox);
            this.titleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarPanel.Location = new System.Drawing.Point(0, 240);
            this.titleBarPanel.Name = "titleBarPanel";
            this.titleBarPanel.Size = new System.Drawing.Size(555, 40);
            this.titleBarPanel.TabIndex = 38;
            this.titleBarPanel.Tag = "VideoTitleBar";
            // 
            // videoTitleBarCheckBox
            // 
            this.videoTitleBarCheckBox.AutoSize = true;
            this.videoTitleBarCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.videoTitleBarCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoTitleBarCheckBox.Location = new System.Drawing.Point(459, 0);
            this.videoTitleBarCheckBox.Name = "videoTitleBarCheckBox";
            this.videoTitleBarCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.videoTitleBarCheckBox.Size = new System.Drawing.Size(96, 40);
            this.videoTitleBarCheckBox.TabIndex = 2;
            this.videoTitleBarCheckBox.Text = "Enabled";
            this.videoTitleBarCheckBox.UseVisualStyleBackColor = true;
            this.videoTitleBarCheckBox.Click += new System.EventHandler(this.VideoTitleBarCheckBoxClick);
            // 
            // StartupOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.DoubleBuffered = true;
            this.Name = "StartupOptions";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(579, 530);
            this.containerPanel.ResumeLayout(false);
            this.groupPanel.ResumeLayout(false);
            this.patrolDoubleBufferPanel.ResumeLayout(false);
            this.patrolDoubleBufferPanel.PerformLayout();
            this.totalBitratePanel.ResumeLayout(false);
            this.hidePanelPanel.ResumeLayout(false);
            this.hidePanelPanel.PerformLayout();
            this.fullscreenPanel.ResumeLayout(false);
            this.fullscreenPanel.PerformLayout();
            this.restoreClientPanel.ResumeLayout(false);
            this.restoreClientPanel.PerformLayout();
            this.titleBarPanel.ResumeLayout(false);
            this.titleBarPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private PanelBase.DoubleBufferPanel containerPanel;
		private PanelBase.DoubleBufferPanel groupPanel;
		private System.Windows.Forms.ComboBox groupComboBox;
		private PanelBase.DoubleBufferPanel patrolDoubleBufferPanel;
		private System.Windows.Forms.CheckBox patrolCheckBox;
		private PanelBase.DoubleBufferPanel totalBitratePanel;
		private System.Windows.Forms.ComboBox totalBitrateComboBox;
		private PanelBase.DoubleBufferPanel hidePanelPanel;
		private System.Windows.Forms.CheckBox hidePanelCheckBox;
		private PanelBase.DoubleBufferPanel fullscreenPanel;
		private System.Windows.Forms.CheckBox fullScreenCheckBox;
		private PanelBase.DoubleBufferPanel restoreClientPanel;
		private System.Windows.Forms.CheckBox enabledCheckBox;
        private PanelBase.DoubleBufferPanel titleBarPanel;
        private System.Windows.Forms.CheckBox videoTitleBarCheckBox;



	}
}
