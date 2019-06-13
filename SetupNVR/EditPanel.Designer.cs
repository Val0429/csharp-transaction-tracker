namespace SetupNVR
{
    partial class EditPanel
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
            this.deviceContainer = new PanelBase.DoubleBufferPanel();
            this.addedNVRLabel = new System.Windows.Forms.Label();
            this.serverStatusIntervalDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.serverStatusCheckIntervalComboBox = new System.Windows.Forms.ComboBox();
            this.blockSizePanel = new PanelBase.DoubleBufferPanel();
            this.blockSizeComboBox = new System.Windows.Forms.ComboBox();
            this.launchTimePanel = new PanelBase.DoubleBufferPanel();
            this.launchTimeComboBox = new System.Windows.Forms.ComboBox();
            this.sslConnectionPanel = new PanelBase.DoubleBufferPanel();
            this.sslCheckBox = new System.Windows.Forms.CheckBox();
            this.patrolPanel = new PanelBase.DoubleBufferPanel();
            this.patrolCheckBox = new System.Windows.Forms.CheckBox();
            this.eventPanel = new PanelBase.DoubleBufferPanel();
            this.listenEventCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountPanel = new PanelBase.DoubleBufferPanel();
            this.accountTextBox = new System.Windows.Forms.TextBox();
            this.serverPortPanel = new PanelBase.DoubleBufferPanel();
            this.serverPortComboBox = new System.Windows.Forms.ComboBox();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.portComboBox = new System.Windows.Forms.ComboBox();
            this.domainPanel = new PanelBase.DoubleBufferPanel();
            this.domainTextBox = new PanelBase.HotKeyTextBox();
            this.manufacturePanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.containerPanel.SuspendLayout();
            this.serverStatusIntervalDoubleBufferPanel.SuspendLayout();
            this.blockSizePanel.SuspendLayout();
            this.launchTimePanel.SuspendLayout();
            this.sslConnectionPanel.SuspendLayout();
            this.patrolPanel.SuspendLayout();
            this.eventPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.serverPortPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.domainPanel.SuspendLayout();
            this.manufacturePanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.deviceContainer);
            this.containerPanel.Controls.Add(this.addedNVRLabel);
            this.containerPanel.Controls.Add(this.serverStatusIntervalDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.blockSizePanel);
            this.containerPanel.Controls.Add(this.launchTimePanel);
            this.containerPanel.Controls.Add(this.sslConnectionPanel);
            this.containerPanel.Controls.Add(this.patrolPanel);
            this.containerPanel.Controls.Add(this.eventPanel);
            this.containerPanel.Controls.Add(this.passwordPanel);
            this.containerPanel.Controls.Add(this.accountPanel);
            this.containerPanel.Controls.Add(this.serverPortPanel);
            this.containerPanel.Controls.Add(this.portPanel);
            this.containerPanel.Controls.Add(this.domainPanel);
            this.containerPanel.Controls.Add(this.manufacturePanel);
            this.containerPanel.Controls.Add(this.namePanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 18, 0, 0);
            this.containerPanel.Size = new System.Drawing.Size(456, 478);
            this.containerPanel.TabIndex = 20;
            // 
            // deviceContainer
            // 
            this.deviceContainer.AutoScroll = true;
            this.deviceContainer.AutoSize = true;
            this.deviceContainer.BackColor = System.Drawing.Color.Transparent;
            this.deviceContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.deviceContainer.Location = new System.Drawing.Point(0, 563);
            this.deviceContainer.MinimumSize = new System.Drawing.Size(0, 30);
            this.deviceContainer.Name = "deviceContainer";
            this.deviceContainer.Size = new System.Drawing.Size(439, 30);
            this.deviceContainer.TabIndex = 26;
            // 
            // addedNVRLabel
            // 
            this.addedNVRLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedNVRLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedNVRLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedNVRLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedNVRLabel.Location = new System.Drawing.Point(0, 538);
            this.addedNVRLabel.Name = "addedNVRLabel";
            this.addedNVRLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedNVRLabel.Size = new System.Drawing.Size(439, 25);
            this.addedNVRLabel.TabIndex = 27;
            this.addedNVRLabel.Text = "Device";
            this.addedNVRLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serverStatusIntervalDoubleBufferPanel
            // 
            this.serverStatusIntervalDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.serverStatusIntervalDoubleBufferPanel.Controls.Add(this.serverStatusCheckIntervalComboBox);
            this.serverStatusIntervalDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverStatusIntervalDoubleBufferPanel.Location = new System.Drawing.Point(0, 498);
            this.serverStatusIntervalDoubleBufferPanel.Name = "serverStatusIntervalDoubleBufferPanel";
            this.serverStatusIntervalDoubleBufferPanel.Size = new System.Drawing.Size(439, 40);
            this.serverStatusIntervalDoubleBufferPanel.TabIndex = 29;
            this.serverStatusIntervalDoubleBufferPanel.Tag = "ServerStatusCheckInterval";
            this.serverStatusIntervalDoubleBufferPanel.Visible = false;
            // 
            // serverStatusCheckIntervalComboBox
            // 
            this.serverStatusCheckIntervalComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serverStatusCheckIntervalComboBox.FormattingEnabled = true;
            this.serverStatusCheckIntervalComboBox.Location = new System.Drawing.Point(227, 6);
            this.serverStatusCheckIntervalComboBox.MaxLength = 3;
            this.serverStatusCheckIntervalComboBox.Name = "serverStatusCheckIntervalComboBox";
            this.serverStatusCheckIntervalComboBox.Size = new System.Drawing.Size(181, 23);
            this.serverStatusCheckIntervalComboBox.TabIndex = 12;
            // 
            // blockSizePanel
            // 
            this.blockSizePanel.BackColor = System.Drawing.Color.Transparent;
            this.blockSizePanel.Controls.Add(this.blockSizeComboBox);
            this.blockSizePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.blockSizePanel.Location = new System.Drawing.Point(0, 458);
            this.blockSizePanel.Name = "blockSizePanel";
            this.blockSizePanel.Size = new System.Drawing.Size(439, 40);
            this.blockSizePanel.TabIndex = 11;
            this.blockSizePanel.Tag = "BlockSize";
            this.blockSizePanel.Visible = false;
            // 
            // blockSizeComboBox
            // 
            this.blockSizeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blockSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blockSizeComboBox.FormattingEnabled = true;
            this.blockSizeComboBox.Location = new System.Drawing.Point(227, 6);
            this.blockSizeComboBox.Name = "blockSizeComboBox";
            this.blockSizeComboBox.Size = new System.Drawing.Size(181, 23);
            this.blockSizeComboBox.TabIndex = 11;
            // 
            // launchTimePanel
            // 
            this.launchTimePanel.BackColor = System.Drawing.Color.Transparent;
            this.launchTimePanel.Controls.Add(this.launchTimeComboBox);
            this.launchTimePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.launchTimePanel.Location = new System.Drawing.Point(0, 418);
            this.launchTimePanel.Name = "launchTimePanel";
            this.launchTimePanel.Size = new System.Drawing.Size(439, 40);
            this.launchTimePanel.TabIndex = 10;
            this.launchTimePanel.Tag = "LaunchTime";
            this.launchTimePanel.Visible = false;
            // 
            // launchTimeComboBox
            // 
            this.launchTimeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.launchTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.launchTimeComboBox.FormattingEnabled = true;
            this.launchTimeComboBox.Location = new System.Drawing.Point(227, 6);
            this.launchTimeComboBox.Name = "launchTimeComboBox";
            this.launchTimeComboBox.Size = new System.Drawing.Size(181, 23);
            this.launchTimeComboBox.TabIndex = 10;
            // 
            // sslConnectionPanel
            // 
            this.sslConnectionPanel.BackColor = System.Drawing.Color.Transparent;
            this.sslConnectionPanel.Controls.Add(this.sslCheckBox);
            this.sslConnectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.sslConnectionPanel.Location = new System.Drawing.Point(0, 378);
            this.sslConnectionPanel.Name = "sslConnectionPanel";
            this.sslConnectionPanel.Size = new System.Drawing.Size(439, 40);
            this.sslConnectionPanel.TabIndex = 10;
            this.sslConnectionPanel.Tag = "SSLConnection";
            // 
            // sslCheckBox
            // 
            this.sslCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sslCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sslCheckBox.Location = new System.Drawing.Point(243, 11);
            this.sslCheckBox.Name = "sslCheckBox";
            this.sslCheckBox.Size = new System.Drawing.Size(181, 19);
            this.sslCheckBox.TabIndex = 9;
            this.sslCheckBox.Text = "Enable";
            this.sslCheckBox.UseVisualStyleBackColor = true;
            // 
            // patrolPanel
            // 
            this.patrolPanel.BackColor = System.Drawing.Color.Transparent;
            this.patrolPanel.Controls.Add(this.patrolCheckBox);
            this.patrolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.patrolPanel.Location = new System.Drawing.Point(0, 338);
            this.patrolPanel.Name = "patrolPanel";
            this.patrolPanel.Size = new System.Drawing.Size(439, 40);
            this.patrolPanel.TabIndex = 8;
            this.patrolPanel.Tag = "Patrol";
            // 
            // patrolCheckBox
            // 
            this.patrolCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.patrolCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patrolCheckBox.Location = new System.Drawing.Point(243, 11);
            this.patrolCheckBox.Name = "patrolCheckBox";
            this.patrolCheckBox.Size = new System.Drawing.Size(181, 19);
            this.patrolCheckBox.TabIndex = 8;
            this.patrolCheckBox.Text = "Include";
            this.patrolCheckBox.UseVisualStyleBackColor = true;
            // 
            // eventPanel
            // 
            this.eventPanel.BackColor = System.Drawing.Color.Transparent;
            this.eventPanel.Controls.Add(this.listenEventCheckBox);
            this.eventPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.eventPanel.Location = new System.Drawing.Point(0, 298);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Size = new System.Drawing.Size(439, 40);
            this.eventPanel.TabIndex = 7;
            this.eventPanel.Tag = "ListenEvent";
            // 
            // listenEventCheckBox
            // 
            this.listenEventCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listenEventCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listenEventCheckBox.Location = new System.Drawing.Point(243, 11);
            this.listenEventCheckBox.Name = "listenEventCheckBox";
            this.listenEventCheckBox.Size = new System.Drawing.Size(181, 19);
            this.listenEventCheckBox.TabIndex = 7;
            this.listenEventCheckBox.Text = "Listen event";
            this.listenEventCheckBox.UseVisualStyleBackColor = true;
            // 
            // passwordPanel
            // 
            this.passwordPanel.BackColor = System.Drawing.Color.Transparent;
            this.passwordPanel.Controls.Add(this.passwordTextBox);
            this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordPanel.Location = new System.Drawing.Point(0, 258);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(439, 40);
            this.passwordPanel.TabIndex = 6;
            this.passwordPanel.Tag = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(243, 8);
            this.passwordTextBox.MaxLength = 25;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(181, 21);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // accountPanel
            // 
            this.accountPanel.BackColor = System.Drawing.Color.Transparent;
            this.accountPanel.Controls.Add(this.accountTextBox);
            this.accountPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.accountPanel.Location = new System.Drawing.Point(0, 218);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(439, 40);
            this.accountPanel.TabIndex = 5;
            this.accountPanel.Tag = "Account";
            // 
            // accountTextBox
            // 
            this.accountTextBox.AcceptsTab = true;
            this.accountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accountTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.accountTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.accountTextBox.Location = new System.Drawing.Point(243, 8);
            this.accountTextBox.MaxLength = 25;
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.Size = new System.Drawing.Size(181, 21);
            this.accountTextBox.TabIndex = 5;
            // 
            // serverPortPanel
            // 
            this.serverPortPanel.BackColor = System.Drawing.Color.Transparent;
            this.serverPortPanel.Controls.Add(this.serverPortComboBox);
            this.serverPortPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverPortPanel.Location = new System.Drawing.Point(0, 178);
            this.serverPortPanel.Name = "serverPortPanel";
            this.serverPortPanel.Size = new System.Drawing.Size(439, 40);
            this.serverPortPanel.TabIndex = 28;
            this.serverPortPanel.Tag = "ServerPort";
            // 
            // serverPortComboBox
            // 
            this.serverPortComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serverPortComboBox.FormattingEnabled = true;
            this.serverPortComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.serverPortComboBox.Location = new System.Drawing.Point(243, 8);
            this.serverPortComboBox.MaxLength = 5;
            this.serverPortComboBox.Name = "serverPortComboBox";
            this.serverPortComboBox.Size = new System.Drawing.Size(181, 23);
            this.serverPortComboBox.TabIndex = 4;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.portComboBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 138);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(439, 40);
            this.portPanel.TabIndex = 4;
            this.portPanel.Tag = "Port";
            // 
            // portComboBox
            // 
            this.portComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portComboBox.FormattingEnabled = true;
            this.portComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portComboBox.Location = new System.Drawing.Point(243, 8);
            this.portComboBox.MaxLength = 5;
            this.portComboBox.Name = "portComboBox";
            this.portComboBox.Size = new System.Drawing.Size(181, 23);
            this.portComboBox.TabIndex = 4;
            // 
            // domainPanel
            // 
            this.domainPanel.BackColor = System.Drawing.Color.Transparent;
            this.domainPanel.Controls.Add(this.domainTextBox);
            this.domainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.domainPanel.Location = new System.Drawing.Point(0, 98);
            this.domainPanel.Name = "domainPanel";
            this.domainPanel.Size = new System.Drawing.Size(439, 40);
            this.domainPanel.TabIndex = 3;
            this.domainPanel.Tag = "Domain";
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainTextBox.Location = new System.Drawing.Point(243, 8);
            this.domainTextBox.MaxLength = 80;
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.ShortcutsEnabled = false;
            this.domainTextBox.Size = new System.Drawing.Size(181, 21);
            this.domainTextBox.TabIndex = 3;
            // 
            // manufacturePanel
            // 
            this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturePanel.Controls.Add(this.manufactureComboBox);
            this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturePanel.Location = new System.Drawing.Point(0, 58);
            this.manufacturePanel.Name = "manufacturePanel";
            this.manufacturePanel.Size = new System.Drawing.Size(439, 40);
            this.manufacturePanel.TabIndex = 2;
            this.manufacturePanel.Tag = "Manufacture";
            // 
            // manufactureComboBox
            // 
            this.manufactureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manufactureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufactureComboBox.FormattingEnabled = true;
            this.manufactureComboBox.Location = new System.Drawing.Point(243, 6);
            this.manufactureComboBox.Name = "manufactureComboBox";
            this.manufactureComboBox.Size = new System.Drawing.Size(181, 23);
            this.manufactureComboBox.TabIndex = 2;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 18);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(439, 40);
            this.namePanel.TabIndex = 1;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(243, 8);
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 1;
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 514);
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.serverStatusIntervalDoubleBufferPanel.ResumeLayout(false);
            this.blockSizePanel.ResumeLayout(false);
            this.launchTimePanel.ResumeLayout(false);
            this.sslConnectionPanel.ResumeLayout(false);
            this.patrolPanel.ResumeLayout(false);
            this.eventPanel.ResumeLayout(false);
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.serverPortPanel.ResumeLayout(false);
            this.portPanel.ResumeLayout(false);
            this.domainPanel.ResumeLayout(false);
            this.domainPanel.PerformLayout();
            this.manufacturePanel.ResumeLayout(false);
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel portPanel;
        private PanelBase.DoubleBufferPanel domainPanel;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private PanelBase.DoubleBufferPanel accountPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private System.Windows.Forms.ComboBox portComboBox;
        private PanelBase.DoubleBufferPanel eventPanel;
        private System.Windows.Forms.CheckBox listenEventCheckBox;
        private PanelBase.DoubleBufferPanel patrolPanel;
        private System.Windows.Forms.CheckBox patrolCheckBox;
        private PanelBase.DoubleBufferPanel blockSizePanel;
        private System.Windows.Forms.ComboBox blockSizeComboBox;
        private PanelBase.DoubleBufferPanel launchTimePanel;
        private System.Windows.Forms.ComboBox launchTimeComboBox;
        private PanelBase.DoubleBufferPanel manufacturePanel;
        private PanelBase.DoubleBufferPanel sslConnectionPanel;
        private System.Windows.Forms.CheckBox sslCheckBox;
        private PanelBase.HotKeyTextBox domainTextBox;
        private new System.Windows.Forms.TextBox accountTextBox;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.DoubleBufferPanel deviceContainer;
        private System.Windows.Forms.Label addedNVRLabel;
        private System.Windows.Forms.ComboBox manufactureComboBox;
        private PanelBase.DoubleBufferPanel serverPortPanel;
        private System.Windows.Forms.ComboBox serverPortComboBox;
        private PanelBase.DoubleBufferPanel serverStatusIntervalDoubleBufferPanel;
        private System.Windows.Forms.ComboBox serverStatusCheckIntervalComboBox;
    }
}
