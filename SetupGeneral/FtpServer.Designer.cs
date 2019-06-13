namespace SetupGeneral
{
    sealed partial class FtpServer
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
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountPanel = new PanelBase.DoubleBufferPanel();
            this.accountTextBox = new PanelBase.HotKeyTextBox();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.portTextBox = new PanelBase.HotKeyTextBox();
            this.directoryPanel = new PanelBase.DoubleBufferPanel();
            this.directoryTextBox = new PanelBase.HotKeyTextBox();
            this.serverPanel = new PanelBase.DoubleBufferPanel();
            this.serverTextBox = new PanelBase.HotKeyTextBox();
            this.containerPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.directoryPanel.SuspendLayout();
            this.serverPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.passwordPanel);
            this.containerPanel.Controls.Add(this.accountPanel);
            this.containerPanel.Controls.Add(this.portPanel);
            this.containerPanel.Controls.Add(this.directoryPanel);
            this.containerPanel.Controls.Add(this.serverPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 364);
            this.containerPanel.TabIndex = 1;
            // 
            // passwordPanel
            // 
            this.passwordPanel.BackColor = System.Drawing.Color.Transparent;
            this.passwordPanel.Controls.Add(this.passwordTextBox);
            this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordPanel.Location = new System.Drawing.Point(0, 160);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(456, 40);
            this.passwordPanel.TabIndex = 5;
            this.passwordPanel.Tag = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(260, 8);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(181, 21);
            this.passwordTextBox.TabIndex = 5;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // accountPanel
            // 
            this.accountPanel.BackColor = System.Drawing.Color.Transparent;
            this.accountPanel.Controls.Add(this.accountTextBox);
            this.accountPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.accountPanel.Location = new System.Drawing.Point(0, 120);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(456, 40);
            this.accountPanel.TabIndex = 4;
            this.accountPanel.Tag = "Account";
            // 
            // accountTextBox
            // 
            this.accountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accountTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountTextBox.Location = new System.Drawing.Point(260, 8);
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.Size = new System.Drawing.Size(181, 21);
            this.accountTextBox.TabIndex = 4;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.portTextBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 80);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(456, 40);
            this.portPanel.TabIndex = 3;
            this.portPanel.Tag = "Port";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTextBox.Location = new System.Drawing.Point(260, 8);
            this.portTextBox.MaxLength = 5;
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(181, 21);
            this.portTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portTextBox.TabIndex = 3;
            // 
            // directoryPanel
            // 
            this.directoryPanel.BackColor = System.Drawing.Color.Transparent;
            this.directoryPanel.Controls.Add(this.directoryTextBox);
            this.directoryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.directoryPanel.Location = new System.Drawing.Point(0, 40);
            this.directoryPanel.Name = "directoryPanel";
            this.directoryPanel.Size = new System.Drawing.Size(456, 40);
            this.directoryPanel.TabIndex = 2;
            this.directoryPanel.Tag = "RemoteDirectory";
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directoryTextBox.Location = new System.Drawing.Point(260, 8);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(181, 21);
            this.directoryTextBox.TabIndex = 2;
            // 
            // serverPanel
            // 
            this.serverPanel.BackColor = System.Drawing.Color.Transparent;
            this.serverPanel.Controls.Add(this.serverTextBox);
            this.serverPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverPanel.Location = new System.Drawing.Point(0, 0);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(456, 40);
            this.serverPanel.TabIndex = 1;
            this.serverPanel.Tag = "Server";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverTextBox.Location = new System.Drawing.Point(260, 8);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(181, 21);
            this.serverTextBox.TabIndex = 1;
            // 
            // FtpServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.DoubleBuffered = true;
            this.Name = "FtpServer";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.ResumeLayout(false);
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.portPanel.ResumeLayout(false);
            this.portPanel.PerformLayout();
            this.directoryPanel.ResumeLayout(false);
            this.directoryPanel.PerformLayout();
            this.serverPanel.ResumeLayout(false);
            this.serverPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel directoryPanel;
        private System.Windows.Forms.TextBox directoryTextBox;
        private PanelBase.DoubleBufferPanel serverPanel;
        private System.Windows.Forms.TextBox serverTextBox;
        private PanelBase.DoubleBufferPanel portPanel;
        private System.Windows.Forms.TextBox portTextBox;
        private PanelBase.DoubleBufferPanel accountPanel;
        private System.Windows.Forms.TextBox accountTextBox;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;



    }
}
