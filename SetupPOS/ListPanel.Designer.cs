namespace SetupPOS
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
            this.addedPOSLabel = new System.Windows.Forms.Label();
            this.pageSelectorPanel = new System.Windows.Forms.Panel();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.pageSelectorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // addedPOSLabel
            // 
            this.addedPOSLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedPOSLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.addedPOSLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedPOSLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedPOSLabel.Location = new System.Drawing.Point(0, 0);
            this.addedPOSLabel.Name = "addedPOSLabel";
            this.addedPOSLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedPOSLabel.Size = new System.Drawing.Size(84, 25);
            this.addedPOSLabel.TabIndex = 19;
            this.addedPOSLabel.Text = "Added POS";
            this.addedPOSLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pageSelectorPanel
            // 
            this.pageSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.pageSelectorPanel.Controls.Add(this.addedPOSLabel);
            this.pageSelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageSelectorPanel.Location = new System.Drawing.Point(12, 58);
            this.pageSelectorPanel.Name = "pageSelectorPanel";
            this.pageSelectorPanel.Size = new System.Drawing.Size(929, 25);
            this.pageSelectorPanel.TabIndex = 0;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 83);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(929, 495);
            this.containerPanel.TabIndex = 20;
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(929, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
            this.addNewDoubleBufferPanel.Tag = "AddNewPOS";
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pageSelectorPanel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(953, 596);
            this.pageSelectorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label addedPOSLabel;
        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Panel pageSelectorPanel;
    }
}
