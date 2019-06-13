namespace VideoMonitor
{
	sealed partial class AmountOfVideoMonitorForm
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
            this.amountLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.amountComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // amountLabel
            // 
            this.amountLabel.BackColor = System.Drawing.Color.Transparent;
            this.amountLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.amountLabel.Location = new System.Drawing.Point(12, 40);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(188, 29);
            this.amountLabel.TabIndex = 0;
            this.amountLabel.Text = "Amount of monitor";
            this.amountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.Transparent;
            this.okButton.BackgroundImage = global::VideoMonitor.Properties.Resources.okButton;
            this.okButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.okButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.okButton.FlatAppearance.BorderSize = 0;
            this.okButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.okButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.okButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.okButton.Location = new System.Drawing.Point(97, 102);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(150, 41);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // amountComboBox
            // 
            this.amountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.amountComboBox.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountComboBox.FormattingEnabled = true;
            this.amountComboBox.Location = new System.Drawing.Point(221, 43);
            this.amountComboBox.Name = "amountComboBox";
            this.amountComboBox.Size = new System.Drawing.Size(79, 25);
            this.amountComboBox.TabIndex = 4;
            // 
            // AmountOfVideoMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::VideoMonitor.Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(339, 170);
            this.ControlBox = false;
            this.Controls.Add(this.amountComboBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.amountLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AmountOfVideoMonitorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Confirm";
            this.Shown += new System.EventHandler(this.AmountOfVideoMonitorFormShown);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label amountLabel;
        public System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ComboBox amountComboBox;
	}
}