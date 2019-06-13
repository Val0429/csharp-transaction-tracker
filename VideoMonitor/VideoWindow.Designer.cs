namespace VideoMonitor
{
    partial class VideoWindow
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
            this.viewerDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.instantPlaybackDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // viewerDoubleBufferPanel
            // 
            this.viewerDoubleBufferPanel.BackColor = System.Drawing.Color.Black;
            this.viewerDoubleBufferPanel.BackgroundImage = global::VideoMonitor.Properties.Resources.viewerBG;
            this.viewerDoubleBufferPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.viewerDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewerDoubleBufferPanel.Location = new System.Drawing.Point(2, 2);
            this.viewerDoubleBufferPanel.Name = "viewerDoubleBufferPanel";
            this.viewerDoubleBufferPanel.Size = new System.Drawing.Size(316, 204);
            this.viewerDoubleBufferPanel.TabIndex = 0;
            // 
            // instantPlaybackDoubleBufferPanel
            // 
            this.instantPlaybackDoubleBufferPanel.BackColor = System.Drawing.Color.Black;
            this.instantPlaybackDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.instantPlaybackDoubleBufferPanel.Location = new System.Drawing.Point(2, 206);
            this.instantPlaybackDoubleBufferPanel.Name = "instantPlaybackDoubleBufferPanel";
            this.instantPlaybackDoubleBufferPanel.Size = new System.Drawing.Size(316, 46);
            this.instantPlaybackDoubleBufferPanel.TabIndex = 0;
            // 
            // VideoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(44)))), ((int)(((byte)(49)))));
            this.Controls.Add(this.viewerDoubleBufferPanel);
            this.Controls.Add(this.instantPlaybackDoubleBufferPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VideoWindow";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(320, 254);
            this.SizeChanged += new System.EventHandler(this.VideoWindow_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

    	public PanelBase.DoubleBufferPanel viewerDoubleBufferPanel;
        protected PanelBase.DoubleBufferPanel instantPlaybackDoubleBufferPanel;
    }
}
