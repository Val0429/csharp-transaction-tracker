namespace UserDeviceGroup
{
	sealed partial class GroupNameInputForm
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
			this.saveButton = new System.Windows.Forms.Button();
			this.groupnameTextBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.publishCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// unlockLabel
			// 
			this.unlockLabel.BackColor = System.Drawing.Color.Transparent;
			this.unlockLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.unlockLabel.Location = new System.Drawing.Point(27, 26);
			this.unlockLabel.Name = "unlockLabel";
			this.unlockLabel.Size = new System.Drawing.Size(316, 41);
			this.unlockLabel.TabIndex = 0;
			this.unlockLabel.Text = "Please enter the Device Group name";
			// 
			// saveButton
			// 
			this.saveButton.BackColor = System.Drawing.Color.Transparent;
			this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.saveButton.FlatAppearance.BorderSize = 0;
			this.saveButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.saveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.saveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.saveButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.saveButton.ForeColor = System.Drawing.Color.Black;
			this.saveButton.Location = new System.Drawing.Point(197, 100);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(84, 30);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = false;
			this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			// 
			// groupnameTextBox
			// 
			this.groupnameTextBox.Location = new System.Drawing.Point(30, 104);
			this.groupnameTextBox.Name = "groupnameTextBox";
			this.groupnameTextBox.Size = new System.Drawing.Size(152, 22);
			this.groupnameTextBox.TabIndex = 1;
			this.groupnameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GroupnameTextBoxKeyPress);
			// 
			// cancelButton
			// 
			this.cancelButton.BackColor = System.Drawing.Color.Transparent;
			this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cancelButton.FlatAppearance.BorderSize = 0;
			this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(288, 100);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(84, 30);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
			// 
			// publishCheckBox
			// 
			this.publishCheckBox.AutoSize = true;
			this.publishCheckBox.BackColor = System.Drawing.Color.Transparent;
			this.publishCheckBox.Font = new System.Drawing.Font("Arial", 9F);
			this.publishCheckBox.Location = new System.Drawing.Point(30, 71);
			this.publishCheckBox.Name = "publishCheckBox";
			this.publishCheckBox.Size = new System.Drawing.Size(126, 19);
			this.publishCheckBox.TabIndex = 4;
			this.publishCheckBox.Text = "Publish this group";
			this.publishCheckBox.UseVisualStyleBackColor = false;
			this.publishCheckBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PublishCheckBoxMouseClick);
			// 
			// GroupNameInputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackgroundImage = global::UserDeviceGroup.Properties.Resources.controllerBG;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(395, 165);
			this.ControlBox = false;
			this.Controls.Add(this.publishCheckBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.groupnameTextBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.unlockLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupNameInputForm";
			this.Text = "User Define Group";
			this.Shown += new System.EventHandler(this.GroupNameInputFormShown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label unlockLabel;
		public System.Windows.Forms.TextBox groupnameTextBox;
		public System.Windows.Forms.Button saveButton;
		public System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox publishCheckBox;
	}
}