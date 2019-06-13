namespace SetupPOSConnection
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.addedPOSConnectionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 83);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 299);
            this.containerPanel.TabIndex = 2;
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.addNewDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
            this.addNewDoubleBufferPanel.Tag = "AddNewPOSConnection";
            // 
            // addedPOSConnectionLabel
            // 
            this.addedPOSConnectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedPOSConnectionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedPOSConnectionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedPOSConnectionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedPOSConnectionLabel.Location = new System.Drawing.Point(12, 58);
            this.addedPOSConnectionLabel.Name = "addedPOSConnectionLabel";
            this.addedPOSConnectionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedPOSConnectionLabel.Size = new System.Drawing.Size(456, 25);
            this.addedPOSConnectionLabel.TabIndex = 9;
            this.addedPOSConnectionLabel.Text = "Added POS Connection";
            this.addedPOSConnectionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedPOSConnectionLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label addedPOSConnectionLabel;

    }
}
