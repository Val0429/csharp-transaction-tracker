namespace SetupException
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
            this.thresholdLabel = new System.Windows.Forms.Label();
            this.addNewLabel = new System.Windows.Forms.Label();
            this.addedPOSExceptionLabel = new System.Windows.Forms.Label();
            this.addExceptionByFileLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionColorDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.thresholdDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.addExceptionByFileDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // thresholdLabel
            // 
            this.thresholdLabel.BackColor = System.Drawing.Color.Transparent;
            this.thresholdLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.thresholdLabel.Location = new System.Drawing.Point(12, 168);
            this.thresholdLabel.Name = "thresholdLabel";
            this.thresholdLabel.Size = new System.Drawing.Size(456, 15);
            this.thresholdLabel.TabIndex = 21;
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
            // addedPOSExceptionLabel
            // 
            this.addedPOSExceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedPOSExceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedPOSExceptionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedPOSExceptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedPOSExceptionLabel.Location = new System.Drawing.Point(12, 223);
            this.addedPOSExceptionLabel.Name = "addedPOSExceptionLabel";
            this.addedPOSExceptionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedPOSExceptionLabel.Size = new System.Drawing.Size(456, 25);
            this.addedPOSExceptionLabel.TabIndex = 25;
            this.addedPOSExceptionLabel.Text = "Added Exception";
            this.addedPOSExceptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // addExceptionByFileLabel
            // 
            this.addExceptionByFileLabel.BackColor = System.Drawing.Color.Transparent;
            this.addExceptionByFileLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addExceptionByFileLabel.Location = new System.Drawing.Point(12, 113);
            this.addExceptionByFileLabel.Name = "addExceptionByFileLabel";
            this.addExceptionByFileLabel.Size = new System.Drawing.Size(456, 15);
            this.addExceptionByFileLabel.TabIndex = 27;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "xml file (*.xml)|*.xml";
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 248);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 134);
            this.containerPanel.TabIndex = 24;
            // 
            // exceptionColorDoubleBufferPanel
            // 
            this.exceptionColorDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionColorDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exceptionColorDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionColorDoubleBufferPanel.Location = new System.Drawing.Point(12, 183);
            this.exceptionColorDoubleBufferPanel.Name = "exceptionColorDoubleBufferPanel";
            this.exceptionColorDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.exceptionColorDoubleBufferPanel.TabIndex = 22;
            this.exceptionColorDoubleBufferPanel.Tag = "ExceptionColor";
            // 
            // thresholdDoubleBufferPanel
            // 
            this.thresholdDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.thresholdDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.thresholdDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.thresholdDoubleBufferPanel.Location = new System.Drawing.Point(12, 128);
            this.thresholdDoubleBufferPanel.Name = "thresholdDoubleBufferPanel";
            this.thresholdDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.thresholdDoubleBufferPanel.TabIndex = 20;
            this.thresholdDoubleBufferPanel.Tag = "Threadhold";
            // 
            // addExceptionByFileDoubleBufferPanel
            // 
            this.addExceptionByFileDoubleBufferPanel.AutoScroll = true;
            this.addExceptionByFileDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addExceptionByFileDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addExceptionByFileDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addExceptionByFileDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
            this.addExceptionByFileDoubleBufferPanel.Name = "addExceptionByFileDoubleBufferPanel";
            this.addExceptionByFileDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.addExceptionByFileDoubleBufferPanel.TabIndex = 26;
            this.addExceptionByFileDoubleBufferPanel.Tag = "AddNewExceptionByFile";
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
            this.addNewDoubleBufferPanel.Tag = "AddNewException";
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedPOSExceptionLabel);
            this.Controls.Add(this.exceptionColorDoubleBufferPanel);
            this.Controls.Add(this.thresholdLabel);
            this.Controls.Add(this.thresholdDoubleBufferPanel);
            this.Controls.Add(this.addExceptionByFileLabel);
            this.Controls.Add(this.addExceptionByFileDoubleBufferPanel);
            this.Controls.Add(this.addNewLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel exceptionColorDoubleBufferPanel;
        private System.Windows.Forms.Label thresholdLabel;
        private PanelBase.DoubleBufferPanel thresholdDoubleBufferPanel;
        private System.Windows.Forms.Label addNewLabel;
        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label addedPOSExceptionLabel;
        private System.Windows.Forms.Label addExceptionByFileLabel;
        private PanelBase.DoubleBufferPanel addExceptionByFileDoubleBufferPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;


    }
}
