namespace VideoMonitor
{
	partial class ViewerPanel
	{
		/// <summary> 
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 元件設計工具產生的程式碼

		/// <summary> 
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
		/// 修改這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.titlePanel = new System.Windows.Forms.Panel();
			this.videoWindow = new VideoMonitor.VideoWindow();
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
			this.titlePanel.Size = new System.Drawing.Size(205, 30);
			this.titlePanel.TabIndex = 1;
			// 
			// videoWindow
			// 
			this.videoWindow.Active = false;
			this.videoWindow.App = null;
			this.videoWindow.Device = null;
			this.videoWindow.DisplayTitleBar = false;
			this.videoWindow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.videoWindow.Location = new System.Drawing.Point(0, 30);
			this.videoWindow.Name = "videoWindow";
			this.videoWindow.PlayMode = Constant.PlayMode.Live;
			this.videoWindow.PtzMode = Constant.PtzMode.Disable;
			this.videoWindow.ShowGPSPath = false;
			this.videoWindow.Size = new System.Drawing.Size(205, 185);
			this.videoWindow.TabIndex = 2;
			this.videoWindow.TitleName = null;
			this.videoWindow.Track = null;
			this.videoWindow.Viewer = null;
			this.videoWindow.WindowLayout = null;
			// 
			// ViewerPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.videoWindow);
			this.Controls.Add(this.titlePanel);
			this.Name = "ViewerPanel";
			this.Size = new System.Drawing.Size(205, 215);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel titlePanel;
		private VideoPlayer viewer;
		private VideoWindow videoWindow;
	}
}
