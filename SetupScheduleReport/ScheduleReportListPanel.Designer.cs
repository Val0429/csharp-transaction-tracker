namespace SetupScheduleReport
{
    sealed partial class ScheduleReportListPanel
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
            this.addedScheduleReportLabel = new System.Windows.Forms.Label();
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
            this.addNewDoubleBufferPanel.Tag = "AddNewGroup";
            // 
            // addedScheduleReportLabel
            // 
            this.addedScheduleReportLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedScheduleReportLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedScheduleReportLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedScheduleReportLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedScheduleReportLabel.Location = new System.Drawing.Point(12, 58);
            this.addedScheduleReportLabel.Name = "addedScheduleReportLabel";
            this.addedScheduleReportLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedScheduleReportLabel.Size = new System.Drawing.Size(456, 25);
            this.addedScheduleReportLabel.TabIndex = 9;
            this.addedScheduleReportLabel.Text = "Added schedule report";
            this.addedScheduleReportLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ScheduleReportListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SetupScheduleReport.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedScheduleReportLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Name = "ScheduleReportListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label addedScheduleReportLabel;

    }
}
