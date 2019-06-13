namespace GPSTree
{
	partial class TreeView
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
			this.titlePanel = new System.Windows.Forms.Panel();
			this.viewModelPanel = new PanelBase.DoubleBufferPanel();
			this.SuspendLayout();
			// 
			// titlePanel
			// 
			this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
			this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.titlePanel.ForeColor = System.Drawing.Color.White;
			this.titlePanel.Location = new System.Drawing.Point(0, 0);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.Size = new System.Drawing.Size(150, 30);
			this.titlePanel.TabIndex = 2;
			// 
			// viewModelPanel
			// 
			this.viewModelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
			this.viewModelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewModelPanel.Location = new System.Drawing.Point(0, 30);
			this.viewModelPanel.Name = "viewModelPanel";
			this.viewModelPanel.Size = new System.Drawing.Size(150, 120);
			this.viewModelPanel.TabIndex = 3;
			// 
			// TreeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.viewModelPanel);
			this.Controls.Add(this.titlePanel);
			this.Name = "TreeView";
			this.ResumeLayout(false);

		}

		#endregion

		private PanelBase.DoubleBufferPanel viewModelPanel;
		private System.Windows.Forms.Panel titlePanel;
	}
}
