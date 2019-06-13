namespace SetupServer
{
    sealed partial class ArchiveServerControl
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
            this.sslConnectionPanel = new PanelBase.DoubleBufferPanel();
            this.sslCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountPanel = new PanelBase.DoubleBufferPanel();
            this.accountTextBox = new System.Windows.Forms.TextBox();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.portComboBox = new System.Windows.Forms.ComboBox();
            this.domainPanel = new PanelBase.DoubleBufferPanel();
            this.domainTextBox = new PanelBase.HotKeyTextBox();
            this.containerPanel.SuspendLayout();
            this.sslConnectionPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.domainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.sslConnectionPanel);
            this.containerPanel.Controls.Add(this.passwordPanel);
            this.containerPanel.Controls.Add(this.accountPanel);
            this.containerPanel.Controls.Add(this.portPanel);
            this.containerPanel.Controls.Add(this.domainPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.containerPanel.Size = new System.Drawing.Size(456, 478);
            this.containerPanel.TabIndex = 20;
            // 
            // sslConnectionPanel
            // 
            this.sslConnectionPanel.BackColor = System.Drawing.Color.Transparent;
            this.sslConnectionPanel.Controls.Add(this.sslCheckBox);
            this.sslConnectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.sslConnectionPanel.Location = new System.Drawing.Point(0, 165);
            this.sslConnectionPanel.Name = "sslConnectionPanel";
            this.sslConnectionPanel.Size = new System.Drawing.Size(456, 40);
            this.sslConnectionPanel.TabIndex = 10;
            this.sslConnectionPanel.Tag = "SSLConnection";
            // 
            // sslCheckBox
            // 
            this.sslCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sslCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sslCheckBox.Location = new System.Drawing.Point(260, 11);
            this.sslCheckBox.Name = "sslCheckBox";
            this.sslCheckBox.Size = new System.Drawing.Size(181, 19);
            this.sslCheckBox.TabIndex = 9;
            this.sslCheckBox.Text = "Enable";
            this.sslCheckBox.UseVisualStyleBackColor = true;
            // 
            // passwordPanel
            // 
            this.passwordPanel.BackColor = System.Drawing.Color.Transparent;
            this.passwordPanel.Controls.Add(this.passwordTextBox);
            this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordPanel.Location = new System.Drawing.Point(0, 125);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(456, 40);
            this.passwordPanel.TabIndex = 6;
            this.passwordPanel.Tag = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(260, 8);
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
            this.accountPanel.Location = new System.Drawing.Point(0, 85);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(456, 40);
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
            this.accountTextBox.Location = new System.Drawing.Point(260, 8);
            this.accountTextBox.MaxLength = 25;
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.Size = new System.Drawing.Size(181, 21);
            this.accountTextBox.TabIndex = 5;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.portComboBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 45);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(456, 40);
            this.portPanel.TabIndex = 4;
            this.portPanel.Tag = "Port";
            // 
            // portComboBox
            // 
            this.portComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portComboBox.FormattingEnabled = true;
            this.portComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portComboBox.Location = new System.Drawing.Point(260, 8);
            this.portComboBox.Name = "portComboBox";
            this.portComboBox.Size = new System.Drawing.Size(181, 23);
            this.portComboBox.TabIndex = 4;
            // 
            // domainPanel
            // 
            this.domainPanel.BackColor = System.Drawing.Color.Transparent;
            this.domainPanel.Controls.Add(this.domainTextBox);
            this.domainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.domainPanel.Location = new System.Drawing.Point(0, 5);
            this.domainPanel.Name = "domainPanel";
            this.domainPanel.Size = new System.Drawing.Size(456, 40);
            this.domainPanel.TabIndex = 3;
            this.domainPanel.Tag = "Domain";
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainTextBox.Location = new System.Drawing.Point(260, 8);
            this.domainTextBox.MaxLength = 80;
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.ShortcutsEnabled = false;
            this.domainTextBox.Size = new System.Drawing.Size(181, 21);
            this.domainTextBox.TabIndex = 3;
            // 
            // ArchiveServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "ArchiveServerControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 514);
            this.containerPanel.ResumeLayout(false);
            this.sslConnectionPanel.ResumeLayout(false);
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.portPanel.ResumeLayout(false);
            this.domainPanel.ResumeLayout(false);
            this.domainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel portPanel;
        private PanelBase.DoubleBufferPanel domainPanel;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private PanelBase.DoubleBufferPanel accountPanel;
        private System.Windows.Forms.ComboBox portComboBox;
        private PanelBase.DoubleBufferPanel sslConnectionPanel;
        private System.Windows.Forms.CheckBox sslCheckBox;
        private PanelBase.HotKeyTextBox domainTextBox;
        private new System.Windows.Forms.TextBox accountTextBox;
    }
}
