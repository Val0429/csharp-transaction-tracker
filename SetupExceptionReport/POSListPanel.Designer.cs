namespace SetupExceptionReport
{
    partial class POSListPanel
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
            this.pageSelectorPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 43);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 339);
            this.containerPanel.TabIndex = 1;
            // 
            // pageSelectorPanel
            // 
            this.pageSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.pageSelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageSelectorPanel.Location = new System.Drawing.Point(12, 18);
            this.pageSelectorPanel.Name = "pageSelectorPanel";
            this.pageSelectorPanel.Size = new System.Drawing.Size(456, 25);
            this.pageSelectorPanel.TabIndex = 2;
            // 
            // POSListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pageSelectorPanel);
            this.Name = "POSListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Panel pageSelectorPanel;
    }
}
