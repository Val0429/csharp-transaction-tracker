namespace PTSReports.Exception
{
    sealed partial class CriteriaPanel
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
            this.components = new System.ComponentModel.Container();
            this.posDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.posLabel = new System.Windows.Forms.Label();
            this.dateTimeDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.exceptionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // posDoubleBufferPanel
            // 
            this.posDoubleBufferPanel.AutoScroll = true;
            this.posDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.posDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.posDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.posDoubleBufferPanel.Name = "posDoubleBufferPanel";
            this.posDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.posDoubleBufferPanel.TabIndex = 3;
            this.posDoubleBufferPanel.Tag = "POS";
            // 
            // posLabel
            // 
            this.posLabel.BackColor = System.Drawing.Color.Transparent;
            this.posLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posLabel.Location = new System.Drawing.Point(12, 58);
            this.posLabel.Name = "posLabel";
            this.posLabel.Size = new System.Drawing.Size(456, 15);
            this.posLabel.TabIndex = 9;
            // 
            // dateTimeDoubleBufferPanel
            // 
            this.dateTimeDoubleBufferPanel.AutoScroll = true;
            this.dateTimeDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.dateTimeDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateTimeDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimeDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
            this.dateTimeDoubleBufferPanel.Name = "dateTimeDoubleBufferPanel";
            this.dateTimeDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.dateTimeDoubleBufferPanel.TabIndex = 10;
            this.dateTimeDoubleBufferPanel.Tag = "DateTime";
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.dateTimeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimeLabel.Location = new System.Drawing.Point(12, 113);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(456, 15);
            this.dateTimeLabel.TabIndex = 11;
            // 
            // exceptionDoubleBufferPanel
            // 
            this.exceptionDoubleBufferPanel.AutoScroll = true;
            this.exceptionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exceptionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionDoubleBufferPanel.Location = new System.Drawing.Point(12, 128);
            this.exceptionDoubleBufferPanel.Name = "exceptionDoubleBufferPanel";
            this.exceptionDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.exceptionDoubleBufferPanel.TabIndex = 12;
            this.exceptionDoubleBufferPanel.Tag = "Exception";
            // 
            // exceptionLabel
            // 
            this.exceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionLabel.Location = new System.Drawing.Point(12, 168);
            this.exceptionLabel.Name = "exceptionLabel";
            this.exceptionLabel.Size = new System.Drawing.Size(456, 15);
            this.exceptionLabel.TabIndex = 13;
            // 
            // CriteriaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.exceptionLabel);
            this.Controls.Add(this.exceptionDoubleBufferPanel);
            this.Controls.Add(this.dateTimeLabel);
            this.Controls.Add(this.dateTimeDoubleBufferPanel);
            this.Controls.Add(this.posLabel);
            this.Controls.Add(this.posDoubleBufferPanel);
            this.Name = "CriteriaPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel posDoubleBufferPanel;
        private System.Windows.Forms.Label posLabel;
        private PanelBase.DoubleBufferPanel dateTimeDoubleBufferPanel;
        private System.Windows.Forms.Label dateTimeLabel;
        private PanelBase.DoubleBufferPanel exceptionDoubleBufferPanel;
        private System.Windows.Forms.Label exceptionLabel;
    }
}
