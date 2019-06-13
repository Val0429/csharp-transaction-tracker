namespace SetupServer
{
    sealed partial class PowerControl
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
            this.rebootPanel = new PanelBase.DoubleBufferPanel();
            this.rebootLabel = new System.Windows.Forms.Label();
            this.shutdownPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // rebootPanel
            // 
            this.rebootPanel.BackColor = System.Drawing.Color.Transparent;
            this.rebootPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rebootPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rebootPanel.Location = new System.Drawing.Point(12, 18);
            this.rebootPanel.Name = "rebootPanel";
            this.rebootPanel.Size = new System.Drawing.Size(456, 40);
            this.rebootPanel.TabIndex = 8;
            this.rebootPanel.Tag = "Reboot";
            this.rebootPanel.Click += new System.EventHandler(this.RebootPanelClick);
            // 
            // rebootLabel
            // 
            this.rebootLabel.BackColor = System.Drawing.Color.Transparent;
            this.rebootLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rebootLabel.Location = new System.Drawing.Point(12, 58);
            this.rebootLabel.Name = "rebootLabel";
            this.rebootLabel.Size = new System.Drawing.Size(456, 15);
            this.rebootLabel.TabIndex = 26;
            // 
            // shutdownPanel
            // 
            this.shutdownPanel.BackColor = System.Drawing.Color.Transparent;
            this.shutdownPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.shutdownPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shutdownPanel.Location = new System.Drawing.Point(12, 73);
            this.shutdownPanel.Name = "shutdownPanel";
            this.shutdownPanel.Size = new System.Drawing.Size(456, 40);
            this.shutdownPanel.TabIndex = 27;
            this.shutdownPanel.Tag = "Shutdown";
            this.shutdownPanel.Click += new System.EventHandler(this.ShutdownPanelClick);
            // 
            // PowerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.shutdownPanel);
            this.Controls.Add(this.rebootLabel);
            this.Controls.Add(this.rebootPanel);
            this.Name = "PowerControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel rebootPanel;
        private System.Windows.Forms.Label rebootLabel;
        private PanelBase.DoubleBufferPanel shutdownPanel;
    }
}
