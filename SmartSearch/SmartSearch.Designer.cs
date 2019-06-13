namespace SmartSearch
{
	partial class SmartSearch
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
			this.videoWindowPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// videoWindowPanel
			// 
			this.videoWindowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
			this.videoWindowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.videoWindowPanel.Location = new System.Drawing.Point(0, 0);
			this.videoWindowPanel.Name = "videoWindowPanel";
			this.videoWindowPanel.Size = new System.Drawing.Size(626, 360);
			this.videoWindowPanel.TabIndex = 19;
			// 
			// SmartSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.videoWindowPanel);
			this.Name = "SmartSearch";
			this.Size = new System.Drawing.Size(626, 360);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel videoWindowPanel;
	}
}
