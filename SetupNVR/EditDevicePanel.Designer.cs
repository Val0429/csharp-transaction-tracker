namespace SetupNVR
{
    sealed partial class EditDevicePanel
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
            this.modelPanel = new PanelBase.DoubleBufferPanel();
            this.manufacturePanel = new PanelBase.DoubleBufferPanel();
            this.manufactureTextBox = new System.Windows.Forms.TextBox();
            this.streamPanel = new PanelBase.DoubleBufferPanel();
            this.liveStreamTextBox = new PanelBase.HotKeyTextBox();
            this.domainPanel = new PanelBase.DoubleBufferPanel();
            this.domainTextBox = new PanelBase.HotKeyTextBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.idPanel = new PanelBase.DoubleBufferPanel();
            this.idTextBox = new PanelBase.HotKeyTextBox();
            this.modelTextBox = new System.Windows.Forms.TextBox();
            this.containerPanel.SuspendLayout();
            this.modelPanel.SuspendLayout();
            this.manufacturePanel.SuspendLayout();
            this.streamPanel.SuspendLayout();
            this.domainPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.idPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.modelPanel);
            this.containerPanel.Controls.Add(this.manufacturePanel);
            this.containerPanel.Controls.Add(this.streamPanel);
            this.containerPanel.Controls.Add(this.domainPanel);
            this.containerPanel.Controls.Add(this.namePanel);
            this.containerPanel.Controls.Add(this.idPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 18, 0, 0);
            this.containerPanel.Size = new System.Drawing.Size(456, 478);
            this.containerPanel.TabIndex = 20;
            // 
            // modelPanel
            // 
            this.modelPanel.BackColor = System.Drawing.Color.Transparent;
            this.modelPanel.Controls.Add(this.modelTextBox);
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelPanel.Location = new System.Drawing.Point(0, 218);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(456, 40);
            this.modelPanel.TabIndex = 4;
            this.modelPanel.Tag = "Model";
            // 
            // manufacturePanel
            // 
            this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturePanel.Controls.Add(this.manufactureTextBox);
            this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturePanel.Location = new System.Drawing.Point(0, 178);
            this.manufacturePanel.Name = "manufacturePanel";
            this.manufacturePanel.Size = new System.Drawing.Size(456, 40);
            this.manufacturePanel.TabIndex = 20;
            this.manufacturePanel.Tag = "Manufacturer";
            // 
            // manufactureTextBox
            // 
            this.manufactureTextBox.AcceptsTab = true;
            this.manufactureTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manufactureTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.manufactureTextBox.Enabled = false;
            this.manufactureTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manufactureTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.manufactureTextBox.Location = new System.Drawing.Point(260, 8);
            this.manufactureTextBox.MaxLength = 25;
            this.manufactureTextBox.Name = "manufactureTextBox";
            this.manufactureTextBox.Size = new System.Drawing.Size(181, 21);
            this.manufactureTextBox.TabIndex = 2;
            this.manufactureTextBox.TabStop = false;
            // 
            // streamPanel
            // 
            this.streamPanel.BackColor = System.Drawing.Color.Transparent;
            this.streamPanel.Controls.Add(this.liveStreamTextBox);
            this.streamPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.streamPanel.Location = new System.Drawing.Point(0, 138);
            this.streamPanel.Name = "streamPanel";
            this.streamPanel.Size = new System.Drawing.Size(456, 40);
            this.streamPanel.TabIndex = 3;
            this.streamPanel.Tag = "LiveStream";
            // 
            // liveStreamTextBox
            // 
            this.liveStreamTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.liveStreamTextBox.Enabled = false;
            this.liveStreamTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveStreamTextBox.Location = new System.Drawing.Point(260, 10);
            this.liveStreamTextBox.MaxLength = 80;
            this.liveStreamTextBox.Name = "liveStreamTextBox";
            this.liveStreamTextBox.ShortcutsEnabled = false;
            this.liveStreamTextBox.Size = new System.Drawing.Size(181, 21);
            this.liveStreamTextBox.TabIndex = 4;
            // 
            // domainPanel
            // 
            this.domainPanel.BackColor = System.Drawing.Color.Transparent;
            this.domainPanel.Controls.Add(this.domainTextBox);
            this.domainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.domainPanel.Location = new System.Drawing.Point(0, 98);
            this.domainPanel.Name = "domainPanel";
            this.domainPanel.Size = new System.Drawing.Size(456, 40);
            this.domainPanel.TabIndex = 2;
            this.domainPanel.Tag = "NetworkAddress";
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Enabled = false;
            this.domainTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainTextBox.Location = new System.Drawing.Point(260, 9);
            this.domainTextBox.MaxLength = 80;
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.ShortcutsEnabled = false;
            this.domainTextBox.Size = new System.Drawing.Size(181, 21);
            this.domainTextBox.TabIndex = 2;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 58);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(456, 40);
            this.namePanel.TabIndex = 3;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(260, 9);
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 2;
            // 
            // idPanel
            // 
            this.idPanel.BackColor = System.Drawing.Color.Transparent;
            this.idPanel.Controls.Add(this.idTextBox);
            this.idPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.idPanel.Location = new System.Drawing.Point(0, 18);
            this.idPanel.Name = "idPanel";
            this.idPanel.Size = new System.Drawing.Size(456, 40);
            this.idPanel.TabIndex = 1;
            this.idPanel.Tag = "ID";
            // 
            // idTextBox
            // 
            this.idTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.idTextBox.Enabled = false;
            this.idTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idTextBox.Location = new System.Drawing.Point(260, 10);
            this.idTextBox.MaxLength = 25;
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.ShortcutsEnabled = false;
            this.idTextBox.Size = new System.Drawing.Size(181, 21);
            this.idTextBox.TabIndex = 3;
            // 
            // modelTextBox
            // 
            this.modelTextBox.AcceptsTab = true;
            this.modelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modelTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.modelTextBox.Enabled = false;
            this.modelTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.modelTextBox.Location = new System.Drawing.Point(260, 10);
            this.modelTextBox.MaxLength = 25;
            this.modelTextBox.Name = "modelTextBox";
            this.modelTextBox.Size = new System.Drawing.Size(181, 21);
            this.modelTextBox.TabIndex = 3;
            this.modelTextBox.TabStop = false;
            // 
            // EditDevicePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditDevicePanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 514);
            this.containerPanel.ResumeLayout(false);
            this.modelPanel.ResumeLayout(false);
            this.modelPanel.PerformLayout();
            this.manufacturePanel.ResumeLayout(false);
            this.manufacturePanel.PerformLayout();
            this.streamPanel.ResumeLayout(false);
            this.streamPanel.PerformLayout();
            this.domainPanel.ResumeLayout(false);
            this.domainPanel.PerformLayout();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.idPanel.ResumeLayout(false);
            this.idPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel domainPanel;
        private PanelBase.DoubleBufferPanel idPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.HotKeyTextBox domainTextBox;
        private PanelBase.DoubleBufferPanel modelPanel;
        private PanelBase.DoubleBufferPanel manufacturePanel;
        private System.Windows.Forms.TextBox manufactureTextBox;
        private PanelBase.DoubleBufferPanel streamPanel;
        private PanelBase.HotKeyTextBox liveStreamTextBox;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.HotKeyTextBox idTextBox;
        private System.Windows.Forms.TextBox modelTextBox;
    }
}
