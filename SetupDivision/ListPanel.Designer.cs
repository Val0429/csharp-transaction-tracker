namespace SetupDivision
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
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.pageSelectorPanel = new System.Windows.Forms.Panel();
            this.addedItemLabel = new System.Windows.Forms.Label();
            this.pageSelectorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(550, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
            this.addNewDoubleBufferPanel.Tag = "AddNewPOS";
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 83);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(550, 480);
            this.containerPanel.TabIndex = 20;
            // 
            // pageSelectorPanel
            // 
            this.pageSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.pageSelectorPanel.Controls.Add(this.addedItemLabel);
            this.pageSelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageSelectorPanel.Location = new System.Drawing.Point(12, 58);
            this.pageSelectorPanel.Name = "pageSelectorPanel";
            this.pageSelectorPanel.Size = new System.Drawing.Size(550, 25);
            this.pageSelectorPanel.TabIndex = 21;
            // 
            // addedItemLabel
            // 
            this.addedItemLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedItemLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.addedItemLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedItemLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedItemLabel.Location = new System.Drawing.Point(0, 0);
            this.addedItemLabel.Name = "addedItemLabel";
            this.addedItemLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedItemLabel.Size = new System.Drawing.Size(107, 25);
            this.addedItemLabel.TabIndex = 19;
            this.addedItemLabel.Text = "Added Region";
            this.addedItemLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.Size = new System.Drawing.Size(574, 581);
            this.pageSelectorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Panel pageSelectorPanel;
        private System.Windows.Forms.Label addedItemLabel;
    }
}
