namespace SetupSchedule
{
    sealed partial class NVRListPanel
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
            this.addedNVRLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // addedNVRLabel
            // 
            this.addedNVRLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedNVRLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedNVRLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedNVRLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedNVRLabel.Location = new System.Drawing.Point(12, 18);
            this.addedNVRLabel.Name = "addedNVRLabel";
            this.addedNVRLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedNVRLabel.Size = new System.Drawing.Size(456, 25);
            this.addedNVRLabel.TabIndex = 11;
            this.addedNVRLabel.Text = "Added NVR";
            this.addedNVRLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 43);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 30);
            this.containerPanel.TabIndex = 1;
            // 
            // NVRListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedNVRLabel);
            this.Name = "NVRListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label addedNVRLabel;
    }
}
