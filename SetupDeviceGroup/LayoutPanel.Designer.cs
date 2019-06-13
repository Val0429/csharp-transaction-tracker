namespace SetupDeviceGroup
{
    sealed partial class LayoutPanel
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
            this.pagerPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(2, 6);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(476, 339);
            this.containerPanel.TabIndex = 3;
            // 
            // pagerPanel
            // 
            this.pagerPanel.AutoScroll = true;
            this.pagerPanel.BackColor = System.Drawing.Color.Transparent;
            this.pagerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagerPanel.Location = new System.Drawing.Point(2, 345);
            this.pagerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.pagerPanel.Name = "pagerPanel";
            this.pagerPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pagerPanel.Size = new System.Drawing.Size(476, 40);
            this.pagerPanel.TabIndex = 4;
            // 
            // LayoutPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pagerPanel);
            this.Name = "LayoutPanel";
            this.Padding = new System.Windows.Forms.Padding(2, 6, 2, 15);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel pagerPanel;


    }
}
