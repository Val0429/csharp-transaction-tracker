namespace App_POSTransactionServer
{
	partial class UnlockAppForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.unlockLabel = new System.Windows.Forms.Label();
            this.unlockButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.password2TextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.accountTextBox = new PanelBase.HotKeyTextBox();
            this.accountLabel = new System.Windows.Forms.Label();
            this.logoutButton = new System.Windows.Forms.Button();
            this.lineLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // unlockLabel
            // 
            this.unlockLabel.BackColor = System.Drawing.Color.Transparent;
            this.unlockLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.unlockLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.unlockLabel.Location = new System.Drawing.Point(27, 26);
            this.unlockLabel.Name = "unlockLabel";
            this.unlockLabel.Size = new System.Drawing.Size(391, 41);
            this.unlockLabel.TabIndex = 0;
            this.unlockLabel.Text = "Enter the \"%1\" password to unlock application";
            // 
            // unlockButton
            // 
            this.unlockButton.BackColor = System.Drawing.Color.Transparent;
            this.unlockButton.BackgroundImage = Properties.Resources.loginButton;
            this.unlockButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.unlockButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.unlockButton.FlatAppearance.BorderSize = 0;
            this.unlockButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.unlockButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.unlockButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.unlockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unlockButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.unlockButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.unlockButton.Location = new System.Drawing.Point(67, 105);
            this.unlockButton.Name = "unlockButton";
            this.unlockButton.Size = new System.Drawing.Size(150, 41);
            this.unlockButton.TabIndex = 2;
            this.unlockButton.Text = "Unlock";
            this.unlockButton.UseVisualStyleBackColor = false;
            this.unlockButton.Click += new System.EventHandler(this.UnlockButtonClick);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Font = new System.Drawing.Font("Arial", 11F);
            this.passwordTextBox.Location = new System.Drawing.Point(190, 70);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(188, 24);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordTextBoxKeyPress);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImage = Properties.Resources.cancelButotn;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cancelButton.Location = new System.Drawing.Point(224, 105);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 41);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // password2TextBox
            // 
            this.password2TextBox.Font = new System.Drawing.Font("Arial", 11F);
            this.password2TextBox.Location = new System.Drawing.Point(190, 212);
            this.password2TextBox.Name = "password2TextBox";
            this.password2TextBox.Size = new System.Drawing.Size(188, 24);
            this.password2TextBox.TabIndex = 17;
            this.password2TextBox.UseSystemPasswordChar = true;
            this.password2TextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Password2TextBoxPreviewKeyDown);
            // 
            // passwordLabel
            // 
            this.passwordLabel.BackColor = System.Drawing.Color.Transparent;
            this.passwordLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.passwordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.passwordLabel.Location = new System.Drawing.Point(81, 212);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.passwordLabel.Size = new System.Drawing.Size(94, 22);
            this.passwordLabel.TabIndex = 21;
            this.passwordLabel.Text = "Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accountTextBox
            // 
            this.accountTextBox.Font = new System.Drawing.Font("Arial", 11F);
            this.accountTextBox.Location = new System.Drawing.Point(190, 177);
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.ShortcutsEnabled = false;
            this.accountTextBox.Size = new System.Drawing.Size(188, 24);
            this.accountTextBox.TabIndex = 16;
            this.accountTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AccountTextBoxPreviewKeyDown);
            // 
            // accountLabel
            // 
            this.accountLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountLabel.Font = new System.Drawing.Font("Arial", 11F);
            this.accountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.accountLabel.Location = new System.Drawing.Point(81, 177);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.accountLabel.Size = new System.Drawing.Size(94, 22);
            this.accountLabel.TabIndex = 20;
            this.accountLabel.Text = "Account";
            this.accountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logoutButton
            // 
            this.logoutButton.BackColor = System.Drawing.Color.Transparent;
		    this.logoutButton.BackgroundImage = Properties.Resources.cancelButotn;
            this.logoutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logoutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutButton.FlatAppearance.BorderSize = 0;
            this.logoutButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.logoutButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.logoutButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.logoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logoutButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.logoutButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.logoutButton.Location = new System.Drawing.Point(223, 253);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(150, 41);
            this.logoutButton.TabIndex = 18;
            this.logoutButton.Text = "Sign Out";
            this.logoutButton.UseVisualStyleBackColor = false;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButtonClick);
            // 
            // lineLabel
            // 
            this.lineLabel.BackColor = System.Drawing.Color.Black;
            this.lineLabel.Location = new System.Drawing.Point(0, 159);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(462, 1);
            this.lineLabel.TabIndex = 19;
            // 
            // UnlockAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		    this.BackgroundImage = Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(446, 152);
		    this.AutoSize = false;
            this.ControlBox = false;
            this.Controls.Add(this.password2TextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.accountTextBox);
            this.Controls.Add(this.accountLabel);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.lineLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.unlockButton);
            this.Controls.Add(this.unlockLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "UnlockAppForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unlock Application";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label unlockLabel;
		public System.Windows.Forms.TextBox passwordTextBox;
		public System.Windows.Forms.Button unlockButton;
		public System.Windows.Forms.Button cancelButton;
		public System.Windows.Forms.TextBox password2TextBox;
		public System.Windows.Forms.Label passwordLabel;
		public PanelBase.HotKeyTextBox accountTextBox;
		public System.Windows.Forms.Label accountLabel;
		public System.Windows.Forms.Button logoutButton;
		private System.Windows.Forms.Label lineLabel;
	}
}