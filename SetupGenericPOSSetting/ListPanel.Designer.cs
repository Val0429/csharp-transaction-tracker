namespace SetupGenericPOSSetting
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
            this.thresholdLabel = new System.Windows.Forms.Label();
            this.transactionSettingDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.addNewLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addedPOSExceptionLabel = new System.Windows.Forms.Label();
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
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
            this.addNewDoubleBufferPanel.Tag = "AddNewSetting";
            // 
            // thresholdLabel
            // 
            this.thresholdLabel.BackColor = System.Drawing.Color.Transparent;
            this.thresholdLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.thresholdLabel.Location = new System.Drawing.Point(12, 113);
            this.thresholdLabel.Name = "thresholdLabel";
            this.thresholdLabel.Size = new System.Drawing.Size(456, 15);
            this.thresholdLabel.TabIndex = 21;
            // 
            // transactionSettingDoubleBufferPanel
            // 
            this.transactionSettingDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.transactionSettingDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transactionSettingDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.transactionSettingDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
            this.transactionSettingDoubleBufferPanel.Name = "transactionSettingDoubleBufferPanel";
            this.transactionSettingDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.transactionSettingDoubleBufferPanel.TabIndex = 20;
            this.transactionSettingDoubleBufferPanel.Tag = "TransactionSetting";
            // 
            // addNewLabel
            // 
            this.addNewLabel.BackColor = System.Drawing.Color.Transparent;
            this.addNewLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewLabel.Location = new System.Drawing.Point(12, 58);
            this.addNewLabel.Name = "addNewLabel";
            this.addNewLabel.Size = new System.Drawing.Size(456, 15);
            this.addNewLabel.TabIndex = 19;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 153);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 229);
            this.containerPanel.TabIndex = 24;
            // 
            // addedPOSExceptionLabel
            // 
            this.addedPOSExceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedPOSExceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedPOSExceptionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedPOSExceptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedPOSExceptionLabel.Location = new System.Drawing.Point(12, 128);
            this.addedPOSExceptionLabel.Name = "addedPOSExceptionLabel";
            this.addedPOSExceptionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedPOSExceptionLabel.Size = new System.Drawing.Size(456, 25);
            this.addedPOSExceptionLabel.TabIndex = 25;
            this.addedPOSExceptionLabel.Text = "Added Exception";
            this.addedPOSExceptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedPOSExceptionLabel);
            this.Controls.Add(this.thresholdLabel);
            this.Controls.Add(this.transactionSettingDoubleBufferPanel);
            this.Controls.Add(this.addNewLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label thresholdLabel;
        private PanelBase.DoubleBufferPanel transactionSettingDoubleBufferPanel;
        private System.Windows.Forms.Label addNewLabel;
        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label addedPOSExceptionLabel;


    }
}
