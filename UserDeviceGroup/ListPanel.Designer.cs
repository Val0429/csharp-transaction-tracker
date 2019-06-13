namespace UserDeviceGroup
{
	sealed partial class ListPanel
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
            this.publicContainerPanel = new PanelBase.DoubleBufferPanel();
            this.publicGroupLabel = new System.Windows.Forms.Label();
            this.privateGroupLabel = new System.Windows.Forms.Label();
            this.privateContainerPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // publicContainerPanel
            // 
            this.publicContainerPanel.AutoScroll = true;
            this.publicContainerPanel.AutoSize = true;
            this.publicContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.publicContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.publicContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.publicContainerPanel.Location = new System.Drawing.Point(12, 28);
            this.publicContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.publicContainerPanel.MaximumSize = new System.Drawing.Size(0, 200);
            this.publicContainerPanel.MinimumSize = new System.Drawing.Size(0, 200);
            this.publicContainerPanel.Name = "publicContainerPanel";
            this.publicContainerPanel.Size = new System.Drawing.Size(456, 200);
            this.publicContainerPanel.TabIndex = 2;
            // 
            // publicGroupLabel
            // 
            this.publicGroupLabel.BackColor = System.Drawing.Color.Transparent;
            this.publicGroupLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.publicGroupLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publicGroupLabel.ForeColor = System.Drawing.Color.DimGray;
            this.publicGroupLabel.Location = new System.Drawing.Point(12, 3);
            this.publicGroupLabel.Name = "publicGroupLabel";
            this.publicGroupLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.publicGroupLabel.Size = new System.Drawing.Size(456, 25);
            this.publicGroupLabel.TabIndex = 9;
            this.publicGroupLabel.Text = "Public group";
            this.publicGroupLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // privateGroupLabel
            // 
            this.privateGroupLabel.BackColor = System.Drawing.Color.Transparent;
            this.privateGroupLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.privateGroupLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.privateGroupLabel.ForeColor = System.Drawing.Color.DimGray;
            this.privateGroupLabel.Location = new System.Drawing.Point(12, 228);
            this.privateGroupLabel.Name = "privateGroupLabel";
            this.privateGroupLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.privateGroupLabel.Size = new System.Drawing.Size(456, 25);
            this.privateGroupLabel.TabIndex = 11;
            this.privateGroupLabel.Text = "Private group";
            this.privateGroupLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // privateContainerPanel
            // 
            this.privateContainerPanel.AutoScroll = true;
            this.privateContainerPanel.AutoSize = true;
            this.privateContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.privateContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.privateContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.privateContainerPanel.Location = new System.Drawing.Point(12, 253);
            this.privateContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.privateContainerPanel.Name = "privateContainerPanel";
            this.privateContainerPanel.Size = new System.Drawing.Size(456, 253);
            this.privateContainerPanel.TabIndex = 12;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::UserDeviceGroup.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.privateContainerPanel);
            this.Controls.Add(this.privateGroupLabel);
            this.Controls.Add(this.publicContainerPanel);
            this.Controls.Add(this.publicGroupLabel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.Size = new System.Drawing.Size(480, 512);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private PanelBase.DoubleBufferPanel publicContainerPanel;
		private System.Windows.Forms.Label publicGroupLabel;
		private System.Windows.Forms.Label privateGroupLabel;
		private PanelBase.DoubleBufferPanel privateContainerPanel;

	}
}
