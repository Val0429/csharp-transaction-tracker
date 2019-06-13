namespace SetupDevice
{
	partial class BrandControl
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
			this.namePanel = new PanelBase.DoubleBufferPanel();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.modelPanel = new PanelBase.DoubleBufferPanel();
			this.modelComboBox = new System.Windows.Forms.ComboBox();
			this.manufacturePanel = new PanelBase.DoubleBufferPanel();
			this.brandComboBox = new System.Windows.Forms.ComboBox();
			this.openWebButton = new System.Windows.Forms.Button();
			this.namePanel.SuspendLayout();
			this.modelPanel.SuspendLayout();
			this.manufacturePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// namePanel
			// 
			this.namePanel.BackColor = System.Drawing.Color.Transparent;
			this.namePanel.Controls.Add(this.nameTextBox);
			this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.namePanel.Location = new System.Drawing.Point(0, 105);
			this.namePanel.Name = "namePanel";
			this.namePanel.Size = new System.Drawing.Size(414, 40);
			this.namePanel.TabIndex = 9;
			this.namePanel.Tag = "Name";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameTextBox.Location = new System.Drawing.Point(218, 8);
			this.nameTextBox.MaxLength = 50;
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(181, 21);
			this.nameTextBox.TabIndex = 2;
			// 
			// modelPanel
			// 
			this.modelPanel.BackColor = System.Drawing.Color.Transparent;
			this.modelPanel.Controls.Add(this.modelComboBox);
			this.modelPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.modelPanel.Location = new System.Drawing.Point(0, 65);
			this.modelPanel.Name = "modelPanel";
			this.modelPanel.Size = new System.Drawing.Size(414, 40);
			this.modelPanel.TabIndex = 8;
			this.modelPanel.Tag = "Model";
			// 
			// modelComboBox
			// 
			this.modelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.modelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.modelComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.modelComboBox.FormattingEnabled = true;
			this.modelComboBox.IntegralHeight = false;
			this.modelComboBox.Location = new System.Drawing.Point(218, 8);
			this.modelComboBox.MaxDropDownItems = 20;
			this.modelComboBox.Name = "modelComboBox";
			this.modelComboBox.Size = new System.Drawing.Size(181, 23);
			this.modelComboBox.Sorted = true;
			this.modelComboBox.TabIndex = 2;
			// 
			// manufacturePanel
			// 
			this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
			this.manufacturePanel.Controls.Add(this.brandComboBox);
			this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.manufacturePanel.Location = new System.Drawing.Point(0, 25);
			this.manufacturePanel.Name = "manufacturePanel";
			this.manufacturePanel.Size = new System.Drawing.Size(414, 40);
			this.manufacturePanel.TabIndex = 7;
			this.manufacturePanel.Tag = "Manufacturer";
			// 
			// brandComboBox
			// 
			this.brandComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.brandComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.brandComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.brandComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.brandComboBox.FormattingEnabled = true;
			this.brandComboBox.IntegralHeight = false;
			this.brandComboBox.Location = new System.Drawing.Point(218, 8);
			this.brandComboBox.Name = "brandComboBox";
			this.brandComboBox.Size = new System.Drawing.Size(181, 23);
			this.brandComboBox.Sorted = true;
			this.brandComboBox.TabIndex = 2;
			// 
			// openWebButton
			// 
			this.openWebButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.openWebButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.openWebButton.Location = new System.Drawing.Point(218, 0);
			this.openWebButton.Name = "openWebButton";
			this.openWebButton.Size = new System.Drawing.Size(181, 24);
			this.openWebButton.TabIndex = 11;
			this.openWebButton.Text = "Open web browser";
			this.openWebButton.UseVisualStyleBackColor = true;
			this.openWebButton.Visible = false;
			this.openWebButton.Click += new System.EventHandler(this.OpenWebButtonClick);
			// 
			// BrandControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.openWebButton);
			this.Controls.Add(this.namePanel);
			this.Controls.Add(this.modelPanel);
			this.Controls.Add(this.manufacturePanel);
			this.Name = "BrandControl";
			this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
			this.Size = new System.Drawing.Size(414, 145);
			this.namePanel.ResumeLayout(false);
			this.namePanel.PerformLayout();
			this.modelPanel.ResumeLayout(false);
			this.manufacturePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public PanelBase.DoubleBufferPanel namePanel;
		public PanelBase.DoubleBufferPanel modelPanel;
		public System.Windows.Forms.ComboBox modelComboBox;
		public PanelBase.DoubleBufferPanel manufacturePanel;
		public System.Windows.Forms.ComboBox brandComboBox;
		protected System.Windows.Forms.Button openWebButton;
		private System.Windows.Forms.TextBox nameTextBox;
	}
}
