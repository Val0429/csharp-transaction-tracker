namespace SetupDevice
{
    sealed partial class ResolutionRegionControl
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.snapshotPanel = new PanelBase.DoubleBufferPanel();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.snapshotPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 25);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(9);
            this.containerPanel.Size = new System.Drawing.Size(640, 480);
            this.containerPanel.TabIndex = 21;
            this.containerPanel.Tag = "";
            // 
            // snapshotPanel
            // 
            this.snapshotPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.snapshotPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotPanel.Location = new System.Drawing.Point(9, 9);
            this.snapshotPanel.Margin = new System.Windows.Forms.Padding(0);
            this.snapshotPanel.Name = "snapshotPanel";
            this.snapshotPanel.Size = new System.Drawing.Size(622, 462);
            this.snapshotPanel.TabIndex = 1;
            // 
            // ResolutionRegionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.containerPanel);
            this.Name = "ResolutionRegionControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(640, 505);
            this.containerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel snapshotPanel;

    }
}
