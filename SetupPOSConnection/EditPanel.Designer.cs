namespace SetupPOSConnection
{
    sealed partial class EditPanel
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
            this.securityPanel = new PanelBase.DoubleBufferPanel();
            this.encryptionComboBox = new System.Windows.Forms.ComboBox();
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountPanel = new PanelBase.DoubleBufferPanel();
            this.userNameTextBox = new PanelBase.HotKeyTextBox();
            this.connectPortPanel = new PanelBase.DoubleBufferPanel();
            this.connectPortTextBox = new PanelBase.HotKeyTextBox();
            this.acceptPortPanel = new PanelBase.DoubleBufferPanel();
            this.acceptPortTextBox = new PanelBase.HotKeyTextBox();
            this.connectInfoPanel = new PanelBase.DoubleBufferPanel();
            this.connectInfoTextBox = new PanelBase.HotKeyTextBox();
            this.queueInfoPanel = new PanelBase.DoubleBufferPanel();
            this.queueInfoTextBox = new PanelBase.HotKeyTextBox();
            this.networkAddressPanel = new PanelBase.DoubleBufferPanel();
            this.ipAddressTextBox = new PanelBase.HotKeyTextBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.protocolPanel = new PanelBase.DoubleBufferPanel();
            this.protocolComboBox = new System.Windows.Forms.ComboBox();
            this.modelPanel = new PanelBase.DoubleBufferPanel();
            this.modelComboBox = new System.Windows.Forms.ComboBox();
            this.manufacturePanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.protocolTypeComboBox = new System.Windows.Forms.ComboBox();
            this.protocolTypePanel = new PanelBase.DoubleBufferPanel();
            this.containerPanel.SuspendLayout();
            this.securityPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.connectPortPanel.SuspendLayout();
            this.acceptPortPanel.SuspendLayout();
            this.connectInfoPanel.SuspendLayout();
            this.queueInfoPanel.SuspendLayout();
            this.networkAddressPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.protocolPanel.SuspendLayout();
            this.modelPanel.SuspendLayout();
            this.manufacturePanel.SuspendLayout();
            this.protocolTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.protocolTypePanel);
            this.containerPanel.Controls.Add(this.securityPanel);
            this.containerPanel.Controls.Add(this.passwordPanel);
            this.containerPanel.Controls.Add(this.accountPanel);
            this.containerPanel.Controls.Add(this.connectPortPanel);
            this.containerPanel.Controls.Add(this.acceptPortPanel);
            this.containerPanel.Controls.Add(this.connectInfoPanel);
            this.containerPanel.Controls.Add(this.queueInfoPanel);
            this.containerPanel.Controls.Add(this.networkAddressPanel);
            this.containerPanel.Controls.Add(this.namePanel);
            this.containerPanel.Controls.Add(this.protocolPanel);
            this.containerPanel.Controls.Add(this.modelPanel);
            this.containerPanel.Controls.Add(this.manufacturePanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 18, 0, 15);
            this.containerPanel.Size = new System.Drawing.Size(456, 553);
            this.containerPanel.TabIndex = 20;
            // 
            // securityPanel
            // 
            this.securityPanel.BackColor = System.Drawing.Color.Transparent;
            this.securityPanel.Controls.Add(this.encryptionComboBox);
            this.securityPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.securityPanel.Location = new System.Drawing.Point(0, 458);
            this.securityPanel.Name = "securityPanel";
            this.securityPanel.Size = new System.Drawing.Size(456, 40);
            this.securityPanel.TabIndex = 32;
            this.securityPanel.Tag = "Security";
            this.securityPanel.Visible = false;
            // 
            // encryptionComboBox
            // 
            this.encryptionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.encryptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encryptionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.encryptionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encryptionComboBox.FormattingEnabled = true;
            this.encryptionComboBox.IntegralHeight = false;
            this.encryptionComboBox.Location = new System.Drawing.Point(260, 8);
            this.encryptionComboBox.MaxDropDownItems = 20;
            this.encryptionComboBox.Name = "encryptionComboBox";
            this.encryptionComboBox.Size = new System.Drawing.Size(181, 23);
            this.encryptionComboBox.TabIndex = 3;
            // 
            // passwordPanel
            // 
            this.passwordPanel.BackColor = System.Drawing.Color.Transparent;
            this.passwordPanel.Controls.Add(this.passwordTextBox);
            this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordPanel.Location = new System.Drawing.Point(0, 418);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(456, 40);
            this.passwordPanel.TabIndex = 31;
            this.passwordPanel.Tag = "Password";
            this.passwordPanel.Visible = false;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.passwordTextBox.Location = new System.Drawing.Point(260, 8);
            this.passwordTextBox.MaxLength = 100;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(181, 21);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // accountPanel
            // 
            this.accountPanel.BackColor = System.Drawing.Color.Transparent;
            this.accountPanel.Controls.Add(this.userNameTextBox);
            this.accountPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.accountPanel.Location = new System.Drawing.Point(0, 378);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(456, 40);
            this.accountPanel.TabIndex = 30;
            this.accountPanel.Tag = "Account";
            this.accountPanel.Visible = false;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userNameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.userNameTextBox.Location = new System.Drawing.Point(260, 8);
            this.userNameTextBox.MaxLength = 100;
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.ShortcutsEnabled = false;
            this.userNameTextBox.Size = new System.Drawing.Size(181, 21);
            this.userNameTextBox.TabIndex = 2;
            // 
            // connectPortPanel
            // 
            this.connectPortPanel.BackColor = System.Drawing.Color.Transparent;
            this.connectPortPanel.Controls.Add(this.connectPortTextBox);
            this.connectPortPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectPortPanel.Location = new System.Drawing.Point(0, 338);
            this.connectPortPanel.Name = "connectPortPanel";
            this.connectPortPanel.Size = new System.Drawing.Size(456, 40);
            this.connectPortPanel.TabIndex = 29;
            this.connectPortPanel.Tag = "ConnectPort";
            this.connectPortPanel.Visible = false;
            // 
            // connectPortTextBox
            // 
            this.connectPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.connectPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.connectPortTextBox.Location = new System.Drawing.Point(260, 8);
            this.connectPortTextBox.MaxLength = 5;
            this.connectPortTextBox.Name = "connectPortTextBox";
            this.connectPortTextBox.ShortcutsEnabled = false;
            this.connectPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.connectPortTextBox.TabIndex = 2;
            // 
            // acceptPortPanel
            // 
            this.acceptPortPanel.BackColor = System.Drawing.Color.Transparent;
            this.acceptPortPanel.Controls.Add(this.acceptPortTextBox);
            this.acceptPortPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.acceptPortPanel.Location = new System.Drawing.Point(0, 298);
            this.acceptPortPanel.Name = "acceptPortPanel";
            this.acceptPortPanel.Size = new System.Drawing.Size(456, 40);
            this.acceptPortPanel.TabIndex = 28;
            this.acceptPortPanel.Tag = "AcceptPort";
            // 
            // acceptPortTextBox
            // 
            this.acceptPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.acceptPortTextBox.Location = new System.Drawing.Point(260, 8);
            this.acceptPortTextBox.MaxLength = 5;
            this.acceptPortTextBox.Name = "acceptPortTextBox";
            this.acceptPortTextBox.ShortcutsEnabled = false;
            this.acceptPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.acceptPortTextBox.TabIndex = 2;
            // 
            // connectInfoPanel
            // 
            this.connectInfoPanel.BackColor = System.Drawing.Color.Transparent;
            this.connectInfoPanel.Controls.Add(this.connectInfoTextBox);
            this.connectInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectInfoPanel.Location = new System.Drawing.Point(0, 258);
            this.connectInfoPanel.Name = "connectInfoPanel";
            this.connectInfoPanel.Size = new System.Drawing.Size(456, 40);
            this.connectInfoPanel.TabIndex = 33;
            this.connectInfoPanel.Tag = "ConnectInfo";
            this.connectInfoPanel.Visible = false;
            // 
            // connectInfoTextBox
            // 
            this.connectInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.connectInfoTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.connectInfoTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectInfoTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.connectInfoTextBox.Location = new System.Drawing.Point(260, 8);
            this.connectInfoTextBox.Name = "connectInfoTextBox";
            this.connectInfoTextBox.ShortcutsEnabled = false;
            this.connectInfoTextBox.Size = new System.Drawing.Size(181, 21);
            this.connectInfoTextBox.TabIndex = 2;
            this.connectInfoTextBox.TabStop = false;
            // 
            // queueInfoPanel
            // 
            this.queueInfoPanel.BackColor = System.Drawing.Color.Transparent;
            this.queueInfoPanel.Controls.Add(this.queueInfoTextBox);
            this.queueInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.queueInfoPanel.Location = new System.Drawing.Point(0, 218);
            this.queueInfoPanel.Name = "queueInfoPanel";
            this.queueInfoPanel.Size = new System.Drawing.Size(456, 40);
            this.queueInfoPanel.TabIndex = 33;
            this.queueInfoPanel.Tag = "QueueInfo";
            this.queueInfoPanel.Visible = false;
            // 
            // queueInfoTextBox
            // 
            this.queueInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueInfoTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.queueInfoTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queueInfoTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.queueInfoTextBox.Location = new System.Drawing.Point(260, 8);
            this.queueInfoTextBox.Name = "queueInfoTextBox";
            this.queueInfoTextBox.ShortcutsEnabled = false;
            this.queueInfoTextBox.Size = new System.Drawing.Size(181, 21);
            this.queueInfoTextBox.TabIndex = 2;
            this.queueInfoTextBox.TabStop = false;
            // 
            // networkAddressPanel
            // 
            this.networkAddressPanel.BackColor = System.Drawing.Color.Transparent;
            this.networkAddressPanel.Controls.Add(this.ipAddressTextBox);
            this.networkAddressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.networkAddressPanel.Location = new System.Drawing.Point(0, 178);
            this.networkAddressPanel.Name = "networkAddressPanel";
            this.networkAddressPanel.Size = new System.Drawing.Size(456, 40);
            this.networkAddressPanel.TabIndex = 26;
            this.networkAddressPanel.Tag = "NetworkAddress";
            this.networkAddressPanel.Visible = false;
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddressTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ipAddressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ipAddressTextBox.Location = new System.Drawing.Point(260, 8);
            this.ipAddressTextBox.MaxLength = 25;
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.ShortcutsEnabled = false;
            this.ipAddressTextBox.Size = new System.Drawing.Size(181, 21);
            this.ipAddressTextBox.TabIndex = 2;
            this.ipAddressTextBox.TabStop = false;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 138);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(456, 40);
            this.namePanel.TabIndex = 20;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(260, 8);
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TabStop = false;
            // 
            // protocolPanel
            // 
            this.protocolPanel.BackColor = System.Drawing.Color.Transparent;
            this.protocolPanel.Controls.Add(this.protocolComboBox);
            this.protocolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.protocolPanel.Location = new System.Drawing.Point(0, 98);
            this.protocolPanel.Name = "protocolPanel";
            this.protocolPanel.Size = new System.Drawing.Size(456, 40);
            this.protocolPanel.TabIndex = 25;
            this.protocolPanel.Tag = "Protocol";
            this.protocolPanel.Visible = false;
            // 
            // protocolComboBox
            // 
            this.protocolComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolComboBox.FormattingEnabled = true;
            this.protocolComboBox.Location = new System.Drawing.Point(260, 8);
            this.protocolComboBox.Name = "protocolComboBox";
            this.protocolComboBox.Size = new System.Drawing.Size(181, 23);
            this.protocolComboBox.TabIndex = 3;
            // 
            // modelPanel
            // 
            this.modelPanel.BackColor = System.Drawing.Color.Transparent;
            this.modelPanel.Controls.Add(this.modelComboBox);
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelPanel.Location = new System.Drawing.Point(0, 58);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(456, 40);
            this.modelPanel.TabIndex = 2;
            this.modelPanel.Tag = "Model";
            this.modelPanel.Visible = false;
            // 
            // modelComboBox
            // 
            this.modelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelComboBox.FormattingEnabled = true;
            this.modelComboBox.Location = new System.Drawing.Point(260, 8);
            this.modelComboBox.Name = "modelComboBox";
            this.modelComboBox.Size = new System.Drawing.Size(181, 23);
            this.modelComboBox.TabIndex = 4;
            // 
            // manufacturePanel
            // 
            this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturePanel.Controls.Add(this.manufactureComboBox);
            this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturePanel.Location = new System.Drawing.Point(0, 18);
            this.manufacturePanel.Name = "manufacturePanel";
            this.manufacturePanel.Size = new System.Drawing.Size(456, 40);
            this.manufacturePanel.TabIndex = 24;
            this.manufacturePanel.Tag = "Manufacture";
            // 
            // manufactureComboBox
            // 
            this.manufactureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manufactureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufactureComboBox.FormattingEnabled = true;
            this.manufactureComboBox.Location = new System.Drawing.Point(260, 8);
            this.manufactureComboBox.Name = "manufactureComboBox";
            this.manufactureComboBox.Size = new System.Drawing.Size(181, 23);
            this.manufactureComboBox.TabIndex = 3;
            // 
            // protocolTypeComboBox
            // 
            this.protocolTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolTypeComboBox.FormattingEnabled = true;
            this.protocolTypeComboBox.Location = new System.Drawing.Point(260, 8);
            this.protocolTypeComboBox.Name = "protocolTypeComboBox";
            this.protocolTypeComboBox.Size = new System.Drawing.Size(181, 23);
            this.protocolTypeComboBox.TabIndex = 3;
            // 
            // protocolTypePanel
            // 
            this.protocolTypePanel.BackColor = System.Drawing.Color.Transparent;
            this.protocolTypePanel.Controls.Add(this.protocolTypeComboBox);
            this.protocolTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.protocolTypePanel.Location = new System.Drawing.Point(0, 498);
            this.protocolTypePanel.Name = "protocolTypePanel";
            this.protocolTypePanel.Size = new System.Drawing.Size(456, 40);
            this.protocolTypePanel.TabIndex = 34;
            this.protocolTypePanel.Tag = "Protocol";
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 591);
            this.containerPanel.ResumeLayout(false);
            this.securityPanel.ResumeLayout(false);
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.connectPortPanel.ResumeLayout(false);
            this.connectPortPanel.PerformLayout();
            this.acceptPortPanel.ResumeLayout(false);
            this.acceptPortPanel.PerformLayout();
            this.connectInfoPanel.ResumeLayout(false);
            this.connectInfoPanel.PerformLayout();
            this.queueInfoPanel.ResumeLayout(false);
            this.queueInfoPanel.PerformLayout();
            this.networkAddressPanel.ResumeLayout(false);
            this.networkAddressPanel.PerformLayout();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.protocolPanel.ResumeLayout(false);
            this.modelPanel.ResumeLayout(false);
            this.manufacturePanel.ResumeLayout(false);
            this.protocolTypePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel modelPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.DoubleBufferPanel manufacturePanel;
        private System.Windows.Forms.ComboBox manufactureComboBox;
        private System.Windows.Forms.ComboBox modelComboBox;
        private PanelBase.DoubleBufferPanel networkAddressPanel;
        private PanelBase.DoubleBufferPanel connectPortPanel;
        private PanelBase.DoubleBufferPanel acceptPortPanel;
        private PanelBase.DoubleBufferPanel securityPanel;
        private System.Windows.Forms.ComboBox encryptionComboBox;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private PanelBase.DoubleBufferPanel accountPanel;
        private PanelBase.DoubleBufferPanel protocolPanel;
        private System.Windows.Forms.ComboBox protocolComboBox;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.HotKeyTextBox ipAddressTextBox;
        private PanelBase.HotKeyTextBox connectPortTextBox;
        private PanelBase.HotKeyTextBox acceptPortTextBox;
        private PanelBase.HotKeyTextBox userNameTextBox;
        private PanelBase.DoubleBufferPanel queueInfoPanel;
        private PanelBase.HotKeyTextBox queueInfoTextBox;
        private PanelBase.DoubleBufferPanel connectInfoPanel;
        private PanelBase.HotKeyTextBox connectInfoTextBox;
        private PanelBase.DoubleBufferPanel protocolTypePanel;
        private System.Windows.Forms.ComboBox protocolTypeComboBox;
    }
}
