namespace SetupServer
{
    sealed partial class StorageControl
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.infoLabel2 = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.deviceKeepDaysRecordingPanel = new PanelBase.DoubleBufferPanel();
            this.deviceKeepDaysLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(12, 248);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(576, 25);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Disk size less than %1GB will not be used as recording storage";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // infoLabel2
            // 
            this.infoLabel2.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel2.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel2.Location = new System.Drawing.Point(12, 123);
            this.infoLabel2.Name = "infoLabel2";
            this.infoLabel2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel2.Size = new System.Drawing.Size(576, 125);
            this.infoLabel2.TabIndex = 2;
            this.infoLabel2.Text = "Set keep space between %1GB and  %2GB under max capacity for best performance\r\n\r\n" +
    "Suggested minimum Keep Space amount:\r\n1.\r\n2.\r\n3.\r\n4.\r\n5.";
            this.infoLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 73);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(576, 50);
            this.containerPanel.TabIndex = 2;
            // 
            // deviceKeepDaysRecordingPanel
            // 
            this.deviceKeepDaysRecordingPanel.AutoScroll = true;
            this.deviceKeepDaysRecordingPanel.BackColor = System.Drawing.Color.Transparent;
            this.deviceKeepDaysRecordingPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deviceKeepDaysRecordingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.deviceKeepDaysRecordingPanel.Location = new System.Drawing.Point(12, 18);
            this.deviceKeepDaysRecordingPanel.Name = "deviceKeepDaysRecordingPanel";
            this.deviceKeepDaysRecordingPanel.Size = new System.Drawing.Size(576, 40);
            this.deviceKeepDaysRecordingPanel.TabIndex = 26;
            this.deviceKeepDaysRecordingPanel.Tag = "AddNewDevice";
            // 
            // deviceKeepDaysLabel
            // 
            this.deviceKeepDaysLabel.BackColor = System.Drawing.Color.Transparent;
            this.deviceKeepDaysLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.deviceKeepDaysLabel.Location = new System.Drawing.Point(12, 58);
            this.deviceKeepDaysLabel.Name = "deviceKeepDaysLabel";
            this.deviceKeepDaysLabel.Size = new System.Drawing.Size(576, 15);
            this.deviceKeepDaysLabel.TabIndex = 27;
            // 
            // StorageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.infoLabel2);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.deviceKeepDaysLabel);
            this.Controls.Add(this.deviceKeepDaysRecordingPanel);
            this.DoubleBuffered = true;
            this.Name = "StorageControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(600, 444);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Label infoLabel2;
        private PanelBase.DoubleBufferPanel deviceKeepDaysRecordingPanel;
        private System.Windows.Forms.Label deviceKeepDaysLabel;
    }
}
