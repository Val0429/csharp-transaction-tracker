namespace SetupLicense
{
	partial class Setup
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
			this.contentPanel = new PanelBase.DoubleBufferPanel();
			this.infoContainerPanel = new PanelBase.DoubleBufferPanel();
			this.ethernetCardPanel = new PanelBase.DoubleBufferPanel();
			this.ethernetComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.licenseKeyBufferPanel = new PanelBase.DoubleBufferPanel();
			this.key5TextBox = new SetupLicense.KeyTextBox();
			this.key4TextBox = new SetupLicense.KeyTextBox();
			this.key3TextBox = new SetupLicense.KeyTextBox();
			this.key2TextBox = new SetupLicense.KeyTextBox();
			this.key1TextBox = new SetupLicense.KeyTextBox();
			this.amountDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
			this.contentPanel.SuspendLayout();
			this.ethernetCardPanel.SuspendLayout();
			this.licenseKeyBufferPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// contentPanel
			// 
			this.contentPanel.AutoScroll = true;
			this.contentPanel.BackColor = System.Drawing.Color.Transparent;
			this.contentPanel.Controls.Add(this.infoContainerPanel);
			this.contentPanel.Controls.Add(this.ethernetCardPanel);
			this.contentPanel.Controls.Add(this.label1);
			this.contentPanel.Controls.Add(this.licenseKeyBufferPanel);
			this.contentPanel.Controls.Add(this.amountDoubleBufferPanel);
			this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.contentPanel.Location = new System.Drawing.Point(3, 3);
			this.contentPanel.Name = "contentPanel";
			this.contentPanel.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
			this.contentPanel.Size = new System.Drawing.Size(480, 384);
			this.contentPanel.TabIndex = 6;
			// 
			// infoContainerPanel
			// 
			this.infoContainerPanel.AutoSize = true;
			this.infoContainerPanel.BackColor = System.Drawing.Color.Transparent;
			this.infoContainerPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.infoContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.infoContainerPanel.Location = new System.Drawing.Point(12, 153);
			this.infoContainerPanel.MinimumSize = new System.Drawing.Size(0, 40);
			this.infoContainerPanel.Name = "infoContainerPanel";
			this.infoContainerPanel.Size = new System.Drawing.Size(456, 40);
			this.infoContainerPanel.TabIndex = 11;
			this.infoContainerPanel.Tag = "";
			// 
			// ethernetCardPanel
			// 
			this.ethernetCardPanel.BackColor = System.Drawing.Color.Transparent;
			this.ethernetCardPanel.Controls.Add(this.ethernetComboBox);
			this.ethernetCardPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.ethernetCardPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ethernetCardPanel.Location = new System.Drawing.Point(12, 113);
			this.ethernetCardPanel.Name = "ethernetCardPanel";
			this.ethernetCardPanel.Size = new System.Drawing.Size(456, 40);
			this.ethernetCardPanel.TabIndex = 12;
			this.ethernetCardPanel.Tag = "EthernetCard";
			// 
			// ethernetComboBox
			// 
			this.ethernetComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ethernetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ethernetComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ethernetComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ethernetComboBox.FormattingEnabled = true;
			this.ethernetComboBox.IntegralHeight = false;
			this.ethernetComboBox.Location = new System.Drawing.Point(130, 8);
			this.ethernetComboBox.MaxDropDownItems = 20;
			this.ethernetComboBox.Name = "ethernetComboBox";
			this.ethernetComboBox.Size = new System.Drawing.Size(313, 23);
			this.ethernetComboBox.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(12, 98);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(456, 15);
			this.label1.TabIndex = 13;
			// 
			// licenseKeyBufferPanel
			// 
			this.licenseKeyBufferPanel.BackColor = System.Drawing.Color.Transparent;
			this.licenseKeyBufferPanel.Controls.Add(this.key5TextBox);
			this.licenseKeyBufferPanel.Controls.Add(this.key4TextBox);
			this.licenseKeyBufferPanel.Controls.Add(this.key3TextBox);
			this.licenseKeyBufferPanel.Controls.Add(this.key2TextBox);
			this.licenseKeyBufferPanel.Controls.Add(this.key1TextBox);
			this.licenseKeyBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.licenseKeyBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.licenseKeyBufferPanel.Location = new System.Drawing.Point(12, 58);
			this.licenseKeyBufferPanel.Name = "licenseKeyBufferPanel";
			this.licenseKeyBufferPanel.Size = new System.Drawing.Size(456, 40);
			this.licenseKeyBufferPanel.TabIndex = 10;
			this.licenseKeyBufferPanel.Tag = "License Key";
			// 
			// key5TextBox
			// 
			this.key5TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.key5TextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.key5TextBox.Location = new System.Drawing.Point(390, 9);
			this.key5TextBox.MaxLength = 5;
			this.key5TextBox.Name = "key5TextBox";
			this.key5TextBox.Size = new System.Drawing.Size(50, 23);
			this.key5TextBox.TabIndex = 7;
			this.key5TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.key5TextBox.Text = "WWWWW";
			this.key5TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// key4TextBox
			// 
			this.key4TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.key4TextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.key4TextBox.Location = new System.Drawing.Point(325, 9);
			this.key4TextBox.MaxLength = 5;
			this.key4TextBox.Name = "key4TextBox";
			this.key4TextBox.Size = new System.Drawing.Size(50, 23);
			this.key4TextBox.TabIndex = 6;
			this.key4TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.key4TextBox.Text = "WWWWW";
			this.key4TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// key3TextBox
			// 
			this.key3TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.key3TextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.key3TextBox.Location = new System.Drawing.Point(260, 9);
			this.key3TextBox.MaxLength = 5;
			this.key3TextBox.Name = "key3TextBox";
			this.key3TextBox.Size = new System.Drawing.Size(50, 23);
			this.key3TextBox.TabIndex = 5;
			this.key3TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.key3TextBox.Text = "WWWWW";
			this.key3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// key2TextBox
			// 
			this.key2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.key2TextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.key2TextBox.Location = new System.Drawing.Point(195, 9);
			this.key2TextBox.MaxLength = 5;
			this.key2TextBox.Name = "key2TextBox";
			this.key2TextBox.Size = new System.Drawing.Size(50, 23);
			this.key2TextBox.TabIndex = 4;
			this.key2TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.key2TextBox.Text = "WWWWW";
			this.key2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// key1TextBox
			// 
			this.key1TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.key1TextBox.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.key1TextBox.Location = new System.Drawing.Point(130, 9);
			this.key1TextBox.MaxLength = 5;
			this.key1TextBox.Name = "key1TextBox";
			this.key1TextBox.Size = new System.Drawing.Size(50, 23);
			this.key1TextBox.TabIndex = 3;
			this.key1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.key1TextBox.Text = "WWWWW";
			this.key1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// amountDoubleBufferPanel
			// 
			this.amountDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
			this.amountDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.amountDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.amountDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
			this.amountDoubleBufferPanel.Name = "amountDoubleBufferPanel";
			this.amountDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
			this.amountDoubleBufferPanel.TabIndex = 7;
			this.amountDoubleBufferPanel.Tag = "License Quantity";
			// 
			// Setup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.contentPanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "Setup";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(486, 390);
			this.contentPanel.ResumeLayout(false);
			this.contentPanel.PerformLayout();
			this.ethernetCardPanel.ResumeLayout(false);
			this.licenseKeyBufferPanel.ResumeLayout(false);
			this.licenseKeyBufferPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public PanelBase.DoubleBufferPanel contentPanel;
		public PanelBase.DoubleBufferPanel amountDoubleBufferPanel;
		public PanelBase.DoubleBufferPanel licenseKeyBufferPanel;
		public KeyTextBox key1TextBox;
		public KeyTextBox key2TextBox;
		public KeyTextBox key5TextBox;
		public KeyTextBox key4TextBox;
		public KeyTextBox key3TextBox;
		public PanelBase.DoubleBufferPanel infoContainerPanel;
		public PanelBase.DoubleBufferPanel ethernetCardPanel;
		public System.Windows.Forms.ComboBox ethernetComboBox;
		private System.Windows.Forms.Label label1;
	}
}
