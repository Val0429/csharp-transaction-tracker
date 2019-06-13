namespace SetupUser
{
    partial class EditUserPanel
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
            this.permissionPanel = new PanelBase.DoubleBufferPanel();
            this.permissionLabel = new System.Windows.Forms.Label();
            this.informationPanel = new PanelBase.DoubleBufferPanel();
            this.groupPanel = new PanelBase.DoubleBufferPanel();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.emailPanel = new PanelBase.DoubleBufferPanel();
            this.emailTextBox = new PanelBase.HotKeyTextBox();
            this.confirmPasswordPanel = new PanelBase.DoubleBufferPanel();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.passwordPanel = new PanelBase.DoubleBufferPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.informationLabel = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.informationPanel.SuspendLayout();
            this.groupPanel.SuspendLayout();
            this.emailPanel.SuspendLayout();
            this.confirmPasswordPanel.SuspendLayout();
            this.passwordPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.permissionPanel);
            this.containerPanel.Controls.Add(this.permissionLabel);
            this.containerPanel.Controls.Add(this.informationPanel);
            this.containerPanel.Controls.Add(this.informationLabel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 420);
            this.containerPanel.TabIndex = 20;
            // 
            // permissionPanel
            // 
            this.permissionPanel.AutoScroll = true;
            this.permissionPanel.AutoSize = true;
            this.permissionPanel.BackColor = System.Drawing.Color.Transparent;
            this.permissionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionPanel.Location = new System.Drawing.Point(0, 245);
            this.permissionPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.permissionPanel.Name = "permissionPanel";
            this.permissionPanel.Size = new System.Drawing.Size(456, 175);
            this.permissionPanel.TabIndex = 23;
            this.permissionPanel.Tag = "";
            // 
            // permissionLabel
            // 
            this.permissionLabel.BackColor = System.Drawing.Color.Transparent;
            this.permissionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.permissionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.permissionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.permissionLabel.Location = new System.Drawing.Point(0, 220);
            this.permissionLabel.Name = "permissionLabel";
            this.permissionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.permissionLabel.Size = new System.Drawing.Size(456, 25);
            this.permissionLabel.TabIndex = 22;
            this.permissionLabel.Text = "DevicePermission";
            this.permissionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // informationPanel
            // 
            this.informationPanel.AutoScroll = true;
            this.informationPanel.AutoSize = true;
            this.informationPanel.BackColor = System.Drawing.Color.Transparent;
            this.informationPanel.Controls.Add(this.groupPanel);
            this.informationPanel.Controls.Add(this.emailPanel);
            this.informationPanel.Controls.Add(this.confirmPasswordPanel);
            this.informationPanel.Controls.Add(this.passwordPanel);
            this.informationPanel.Controls.Add(this.namePanel);
            this.informationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.informationPanel.Location = new System.Drawing.Point(0, 20);
            this.informationPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.informationPanel.Name = "informationPanel";
            this.informationPanel.Size = new System.Drawing.Size(456, 200);
            this.informationPanel.TabIndex = 21;
            this.informationPanel.Tag = "";
            // 
            // groupPanel
            // 
            this.groupPanel.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel.Controls.Add(this.groupComboBox);
            this.groupPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel.Location = new System.Drawing.Point(0, 160);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(456, 40);
            this.groupPanel.TabIndex = 24;
            this.groupPanel.Tag = "Group";
            // 
            // groupComboBox
            // 
            this.groupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.IntegralHeight = false;
            this.groupComboBox.Location = new System.Drawing.Point(260, 8);
            this.groupComboBox.MaxDropDownItems = 20;
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(181, 23);
            this.groupComboBox.TabIndex = 2;
            // 
            // emailPanel
            // 
            this.emailPanel.BackColor = System.Drawing.Color.Transparent;
            this.emailPanel.Controls.Add(this.emailTextBox);
            this.emailPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.emailPanel.Location = new System.Drawing.Point(0, 120);
            this.emailPanel.Name = "emailPanel";
            this.emailPanel.Size = new System.Drawing.Size(456, 40);
            this.emailPanel.TabIndex = 23;
            this.emailPanel.Tag = "Email";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emailTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTextBox.Location = new System.Drawing.Point(260, 8);
            this.emailTextBox.MaxLength = 40;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(181, 21);
            this.emailTextBox.TabIndex = 2;
            // 
            // confirmPasswordPanel
            // 
            this.confirmPasswordPanel.BackColor = System.Drawing.Color.Transparent;
            this.confirmPasswordPanel.Controls.Add(this.confirmPasswordTextBox);
            this.confirmPasswordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.confirmPasswordPanel.Location = new System.Drawing.Point(0, 80);
            this.confirmPasswordPanel.Name = "confirmPasswordPanel";
            this.confirmPasswordPanel.Size = new System.Drawing.Size(456, 40);
            this.confirmPasswordPanel.TabIndex = 22;
            this.confirmPasswordPanel.Tag = "ConfirmPassword";
            // 
            // confirmPasswordTextBox
            // 
            this.confirmPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmPasswordTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPasswordTextBox.Location = new System.Drawing.Point(260, 8);
            this.confirmPasswordTextBox.MaxLength = 25;
            this.confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            this.confirmPasswordTextBox.PasswordChar = '*';
            this.confirmPasswordTextBox.Size = new System.Drawing.Size(181, 21);
            this.confirmPasswordTextBox.TabIndex = 2;
            this.confirmPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordPanel
            // 
            this.passwordPanel.BackColor = System.Drawing.Color.Transparent;
            this.passwordPanel.Controls.Add(this.passwordTextBox);
            this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordPanel.Location = new System.Drawing.Point(0, 40);
            this.passwordPanel.Name = "passwordPanel";
            this.passwordPanel.Size = new System.Drawing.Size(456, 40);
            this.passwordPanel.TabIndex = 21;
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
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 0);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(456, 40);
            this.namePanel.TabIndex = 20;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(260, 8);
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nameTextBox.TabIndex = 2;
            // 
            // informationLabel
            // 
            this.informationLabel.BackColor = System.Drawing.Color.Transparent;
            this.informationLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.informationLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informationLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.informationLabel.Location = new System.Drawing.Point(0, 0);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.informationLabel.Size = new System.Drawing.Size(456, 20);
            this.informationLabel.TabIndex = 20;
            this.informationLabel.Text = "Information";
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // EditUserPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditUserPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 456);
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.informationPanel.ResumeLayout(false);
            this.groupPanel.ResumeLayout(false);
            this.emailPanel.ResumeLayout(false);
            this.emailPanel.PerformLayout();
            this.confirmPasswordPanel.ResumeLayout(false);
            this.confirmPasswordPanel.PerformLayout();
            this.passwordPanel.ResumeLayout(false);
            this.passwordPanel.PerformLayout();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label informationLabel;
        private PanelBase.DoubleBufferPanel informationPanel;
        private PanelBase.DoubleBufferPanel groupPanel;
        private System.Windows.Forms.ComboBox groupComboBox;
        private PanelBase.DoubleBufferPanel emailPanel;
        private System.Windows.Forms.TextBox emailTextBox;
        private PanelBase.DoubleBufferPanel confirmPasswordPanel;
        private System.Windows.Forms.TextBox confirmPasswordTextBox;
        private PanelBase.DoubleBufferPanel passwordPanel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private PanelBase.DoubleBufferPanel namePanel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label permissionLabel;
        private PanelBase.DoubleBufferPanel permissionPanel;
    }
}
