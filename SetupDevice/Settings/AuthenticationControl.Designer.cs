namespace SetupDevice
{
    sealed partial class AuthenticationControl
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
            this.encryptionPanel = new PanelBase.DoubleBufferPanel();
            this.encryptionComboBox = new System.Windows.Forms.ComboBox();
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountPanel = new PanelBase.DoubleBufferPanel();
            this.userNameTextBox = new PanelBase.HotKeyTextBox();
            this.occupancyPriorityPanel = new PanelBase.DoubleBufferPanel();
            this.occupancyPriorityComboBox = new System.Windows.Forms.ComboBox();
            this.encryptionPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.occupancyPriorityPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // encryptionPanel
            // 
            this.encryptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.encryptionPanel.Controls.Add(this.encryptionComboBox);
            this.encryptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.encryptionPanel.Location = new System.Drawing.Point(0, 105);
            this.encryptionPanel.Name = "encryptionPanel";
            this.encryptionPanel.Size = new System.Drawing.Size(408, 40);
            this.encryptionPanel.TabIndex = 9;
            this.encryptionPanel.Tag = "Encryption";
            // 
            // encryptionComboBox
            // 
            this.encryptionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.encryptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encryptionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.encryptionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encryptionComboBox.FormattingEnabled = true;
            this.encryptionComboBox.IntegralHeight = false;
            this.encryptionComboBox.Location = new System.Drawing.Point(212, 8);
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
            this.passwordPanel.Location = new System.Drawing.Point(0, 65);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(408, 40);
            this.passwordPanel.TabIndex = 8;
            this.passwordPanel.Tag = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(212, 8);
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
            this.accountPanel.Location = new System.Drawing.Point(0, 25);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(408, 40);
            this.accountPanel.TabIndex = 7;
            this.accountPanel.Tag = "Account";
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userNameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameTextBox.Location = new System.Drawing.Point(212, 8);
            this.userNameTextBox.MaxLength = 100;
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(181, 21);
            this.userNameTextBox.TabIndex = 2;
            // 
            // occupancyPriorityPanel
            // 
            this.occupancyPriorityPanel.BackColor = System.Drawing.Color.Transparent;
            this.occupancyPriorityPanel.Controls.Add(this.occupancyPriorityComboBox);
            this.occupancyPriorityPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.occupancyPriorityPanel.Location = new System.Drawing.Point(0, 145);
            this.occupancyPriorityPanel.Name = "occupancyPriorityPanel";
            this.occupancyPriorityPanel.Size = new System.Drawing.Size(408, 40);
            this.occupancyPriorityPanel.TabIndex = 10;
            this.occupancyPriorityPanel.Tag = "OccupancyPriority";
            // 
            // occupancyPriorityComboBox
            // 
            this.occupancyPriorityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.occupancyPriorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.occupancyPriorityComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.occupancyPriorityComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.occupancyPriorityComboBox.FormattingEnabled = true;
            this.occupancyPriorityComboBox.IntegralHeight = false;
            this.occupancyPriorityComboBox.Location = new System.Drawing.Point(212, 8);
            this.occupancyPriorityComboBox.MaxDropDownItems = 20;
            this.occupancyPriorityComboBox.Name = "occupancyPriorityComboBox";
            this.occupancyPriorityComboBox.Size = new System.Drawing.Size(181, 23);
            this.occupancyPriorityComboBox.TabIndex = 3;
            // 
            // AuthenticationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.occupancyPriorityPanel);
            this.Controls.Add(this.encryptionPanel);
            this.Controls.Add(this.passwordPanel);
            this.Controls.Add(this.accountPanel);
            this.Name = "AuthenticationControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(408, 185);
            this.encryptionPanel.ResumeLayout(false);
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.occupancyPriorityPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel encryptionPanel;
        private System.Windows.Forms.ComboBox encryptionComboBox;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private PanelBase.DoubleBufferPanel accountPanel;
        private PanelBase.DoubleBufferPanel occupancyPriorityPanel;
        private System.Windows.Forms.ComboBox occupancyPriorityComboBox;
        private PanelBase.HotKeyTextBox userNameTextBox;
    }
}
