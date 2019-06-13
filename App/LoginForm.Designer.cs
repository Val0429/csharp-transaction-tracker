using System;

namespace App
{
    partial class LoginForm
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
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hostComboBox = new System.Windows.Forms.ComboBox();
            this.hostLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.localizationComboBox = new System.Windows.Forms.ComboBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.sslCheckBox = new System.Windows.Forms.CheckBox();
            this.signMeInAutomaticallyCheckBox = new System.Windows.Forms.CheckBox();
            this.forgetMePanel = new System.Windows.Forms.Panel();
            this.titlePanel = new PanelBase.DoubleBufferPanel();
            this.signInLabel = new System.Windows.Forms.Label();
            this.accountTextBox = new PanelBase.HotKeyTextBox();
            this.portTextBox = new PanelBase.HotKeyTextBox();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(160, 177);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(235, 24);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.passwordLabel.BackColor = System.Drawing.Color.Transparent;
            this.passwordLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.passwordLabel.Location = new System.Drawing.Point(12, 177);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(130, 22);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accountLabel
            // 
            this.accountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.accountLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.accountLabel.Location = new System.Drawing.Point(12, 132);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(130, 22);
            this.accountLabel.TabIndex = 4;
            this.accountLabel.Text = "Account";
            this.accountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.label2.Location = new System.Drawing.Point(336, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = ":";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hostComboBox
            // 
            this.hostComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hostComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hostComboBox.FormattingEnabled = true;
            this.hostComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.hostComboBox.Location = new System.Drawing.Point(160, 86);
            this.hostComboBox.Name = "hostComboBox";
            this.hostComboBox.Size = new System.Drawing.Size(170, 25);
            this.hostComboBox.TabIndex = 1;
            this.hostComboBox.SelectionChangeCommitted += new System.EventHandler(this.HostComboBoxSelectionChangeCommitted);
            // 
            // hostLabel
            // 
            this.hostLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hostLabel.BackColor = System.Drawing.Color.Transparent;
            this.hostLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hostLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.hostLabel.Location = new System.Drawing.Point(12, 86);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(130, 22);
            this.hostLabel.TabIndex = 3;
            this.hostLabel.Text = "Host";
            this.hostLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // loginButton
            // 
            this.loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loginButton.BackColor = System.Drawing.Color.Transparent;
            this.loginButton.BackgroundImage = global::App.Properties.Resources.loginButton;
            this.loginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.loginButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.loginButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.loginButton.Location = new System.Drawing.Point(79, 359);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(150, 41);
            this.loginButton.TabIndex = 9;
            this.loginButton.Text = "Sign In";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LoginButtonMouseClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImage = global::App.Properties.Resources.cancelButotn;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cancelButton.Location = new System.Drawing.Point(235, 359);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 41);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonMouseClick);
            // 
            // localizationComboBox
            // 
            this.localizationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.localizationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localizationComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.localizationComboBox.FormattingEnabled = true;
            this.localizationComboBox.Location = new System.Drawing.Point(160, 222);
            this.localizationComboBox.Name = "localizationComboBox";
            this.localizationComboBox.Size = new System.Drawing.Size(235, 25);
            this.localizationComboBox.TabIndex = 5;
            // 
            // languageLabel
            // 
            this.languageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.languageLabel.BackColor = System.Drawing.Color.Transparent;
            this.languageLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.languageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.languageLabel.Location = new System.Drawing.Point(12, 222);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(130, 22);
            this.languageLabel.TabIndex = 12;
            this.languageLabel.Text = "Language";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.logoPictureBox.Location = new System.Drawing.Point(8, 3);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(478, 60);
            this.logoPictureBox.TabIndex = 13;
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.Visible = false;
            this.logoPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragPanelMouseDown);
            // 
            // sslCheckBox
            // 
            this.sslCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sslCheckBox.AutoSize = true;
            this.sslCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.sslCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sslCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.sslCheckBox.Location = new System.Drawing.Point(160, 321);
            this.sslCheckBox.Name = "sslCheckBox";
            this.sslCheckBox.Size = new System.Drawing.Size(178, 21);
            this.sslCheckBox.TabIndex = 14;
            this.sslCheckBox.Text = "Enable SSL connection";
            this.sslCheckBox.UseVisualStyleBackColor = false;
            // 
            // signMeInAutomaticallyCheckBox
            // 
            this.signMeInAutomaticallyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.signMeInAutomaticallyCheckBox.AutoSize = true;
            this.signMeInAutomaticallyCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.signMeInAutomaticallyCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signMeInAutomaticallyCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.signMeInAutomaticallyCheckBox.Location = new System.Drawing.Point(160, 290);
            this.signMeInAutomaticallyCheckBox.Name = "signMeInAutomaticallyCheckBox";
            this.signMeInAutomaticallyCheckBox.Size = new System.Drawing.Size(185, 21);
            this.signMeInAutomaticallyCheckBox.TabIndex = 15;
            this.signMeInAutomaticallyCheckBox.Text = "Sign me in automatically";
            this.signMeInAutomaticallyCheckBox.UseVisualStyleBackColor = false;
            this.signMeInAutomaticallyCheckBox.CheckedChanged += new System.EventHandler(this.SignMeInAutomaticallyCheckBoxCheckedChanged);
            // 
            // forgetMePanel
            // 
            this.forgetMePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forgetMePanel.BackColor = System.Drawing.Color.Transparent;
            this.forgetMePanel.BackgroundImage = global::App.Properties.Resources.forgetMe;
            this.forgetMePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.forgetMePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgetMePanel.Location = new System.Drawing.Point(402, 132);
            this.forgetMePanel.Name = "forgetMePanel";
            this.forgetMePanel.Size = new System.Drawing.Size(24, 24);
            this.forgetMePanel.TabIndex = 16;
            this.forgetMePanel.Visible = false;
            this.forgetMePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ForgetMePanelMouseClick);
            // 
            // titlePanel
            // 
            this.titlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titlePanel.BackColor = System.Drawing.Color.Transparent;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.titlePanel.Controls.Add(this.signInLabel);
            this.titlePanel.Location = new System.Drawing.Point(10, 7);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(476, 49);
            this.titlePanel.TabIndex = 8;
            // 
            // signInLabel
            // 
            this.signInLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.signInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signInLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.signInLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signInLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.signInLabel.Location = new System.Drawing.Point(0, 0);
            this.signInLabel.Margin = new System.Windows.Forms.Padding(0);
            this.signInLabel.Name = "signInLabel";
            this.signInLabel.Padding = new System.Windows.Forms.Padding(20, 0, 5, 0);
            this.signInLabel.Size = new System.Drawing.Size(476, 49);
            this.signInLabel.TabIndex = 0;
            this.signInLabel.Text = "Sign In";
            this.signInLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.signInLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragPanelMouseDown);
            // 
            // accountTextBox
            // 
            this.accountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.accountTextBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.accountTextBox.Location = new System.Drawing.Point(160, 132);
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.ShortcutsEnabled = false;
            this.accountTextBox.Size = new System.Drawing.Size(235, 24);
            this.accountTextBox.TabIndex = 3;
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portTextBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portTextBox.Location = new System.Drawing.Point(350, 87);
            this.portTextBox.MaxLength = 5;
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.ShortcutsEnabled = false;
            this.portTextBox.Size = new System.Drawing.Size(45, 24);
            this.portTextBox.TabIndex = 2;
            this.portTextBox.WordWrap = false;
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.rememberCheckBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rememberCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.rememberCheckBox.Location = new System.Drawing.Point(160, 259);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(126, 21);
            this.rememberCheckBox.TabIndex = 17;
            this.rememberCheckBox.Text = "Remember me";
            this.rememberCheckBox.UseVisualStyleBackColor = false;
            this.rememberCheckBox.CheckedChanged += new EventHandler(RememberCheckBoxCheckedChanged);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::App.Properties.Resources.loginPanel;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(494, 418);
            this.ControlBox = false;
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.forgetMePanel);
            this.Controls.Add(this.signMeInAutomaticallyCheckBox);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.sslCheckBox);
            this.Controls.Add(this.languageLabel);
            this.Controls.Add(this.localizationComboBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.accountTextBox);
            this.Controls.Add(this.accountLabel);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hostLabel);
            this.Controls.Add(this.hostComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.loginButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(494, 418);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(494, 418);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected PanelBase.DoubleBufferPanel titlePanel;
        protected System.Windows.Forms.Label signInLabel;
        protected System.Windows.Forms.PictureBox logoPictureBox;
        public System.Windows.Forms.Button loginButton;
        public System.Windows.Forms.Label label2;
        public PanelBase.HotKeyTextBox portTextBox;
        public System.Windows.Forms.Label hostLabel;
        public PanelBase.HotKeyTextBox accountTextBox;
        public System.Windows.Forms.Label accountLabel;
        public System.Windows.Forms.Label passwordLabel;
        public System.Windows.Forms.TextBox passwordTextBox;
        public System.Windows.Forms.ComboBox hostComboBox;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.ComboBox localizationComboBox;
        public System.Windows.Forms.Label languageLabel;
        public System.Windows.Forms.CheckBox sslCheckBox;
        public System.Windows.Forms.CheckBox signMeInAutomaticallyCheckBox;
        public System.Windows.Forms.Panel forgetMePanel;
        public System.Windows.Forms.CheckBox rememberCheckBox;
    }
}
