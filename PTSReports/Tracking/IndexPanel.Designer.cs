namespace PTSReports.Tracking
{
    sealed partial class IndexPanel
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
            this.employeeProductivityImprovementTrendPanel = new PanelBase.DoubleBufferPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.employeeProductivityLossRankingPanel = new PanelBase.DoubleBufferPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.highAlertTop2EmployeesExceptionMonitoringPanel = new PanelBase.DoubleBufferPanel();
            this.employeesProductivityLossTrackingLabel = new System.Windows.Forms.Label();
            this.performanceDeviationAgainstPredefinedThresholdsPanel = new PanelBase.DoubleBufferPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.frequentExceptionsIncurredByEmployeesPanel = new PanelBase.DoubleBufferPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.frequentExceptionCategoriesIncurredPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionTrackingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // employeeProductivityImprovementTrendPanel
            // 
            this.employeeProductivityImprovementTrendPanel.AutoScroll = true;
            this.employeeProductivityImprovementTrendPanel.BackColor = System.Drawing.Color.Transparent;
            this.employeeProductivityImprovementTrendPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.employeeProductivityImprovementTrendPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.employeeProductivityImprovementTrendPanel.Location = new System.Drawing.Point(12, 318);
            this.employeeProductivityImprovementTrendPanel.Name = "employeeProductivityImprovementTrendPanel";
            this.employeeProductivityImprovementTrendPanel.Size = new System.Drawing.Size(456, 40);
            this.employeeProductivityImprovementTrendPanel.TabIndex = 50;
            this.employeeProductivityImprovementTrendPanel.Tag = "EmployeeProductivityImprovementTrend";
            this.employeeProductivityImprovementTrendPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.C3ReportPanelClick);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(12, 303);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(456, 15);
            this.label4.TabIndex = 49;
            // 
            // employeeProductivityLossRankingPanel
            // 
            this.employeeProductivityLossRankingPanel.AutoScroll = true;
            this.employeeProductivityLossRankingPanel.BackColor = System.Drawing.Color.Transparent;
            this.employeeProductivityLossRankingPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.employeeProductivityLossRankingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.employeeProductivityLossRankingPanel.Location = new System.Drawing.Point(12, 263);
            this.employeeProductivityLossRankingPanel.Name = "employeeProductivityLossRankingPanel";
            this.employeeProductivityLossRankingPanel.Size = new System.Drawing.Size(456, 40);
            this.employeeProductivityLossRankingPanel.TabIndex = 48;
            this.employeeProductivityLossRankingPanel.Tag = "EmployeeProductivityLossRanking";
            this.employeeProductivityLossRankingPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.C2ReportPanelClick);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(12, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(456, 15);
            this.label3.TabIndex = 47;
            // 
            // highAlertTop2EmployeesExceptionMonitoringPanel
            // 
            this.highAlertTop2EmployeesExceptionMonitoringPanel.AutoScroll = true;
            this.highAlertTop2EmployeesExceptionMonitoringPanel.BackColor = System.Drawing.Color.Transparent;
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Location = new System.Drawing.Point(12, 208);
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Name = "highAlertTop2EmployeesExceptionMonitoringPanel";
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Size = new System.Drawing.Size(456, 40);
            this.highAlertTop2EmployeesExceptionMonitoringPanel.TabIndex = 46;
            this.highAlertTop2EmployeesExceptionMonitoringPanel.Tag = "HighAlertTop2EmployeesExceptionMonitoring";
            this.highAlertTop2EmployeesExceptionMonitoringPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.C1ReportPanelClick);
            // 
            // employeesProductivityLossTrackingLabel
            // 
            this.employeesProductivityLossTrackingLabel.BackColor = System.Drawing.Color.Transparent;
            this.employeesProductivityLossTrackingLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.employeesProductivityLossTrackingLabel.ForeColor = System.Drawing.Color.DimGray;
            this.employeesProductivityLossTrackingLabel.Location = new System.Drawing.Point(12, 183);
            this.employeesProductivityLossTrackingLabel.Name = "employeesProductivityLossTrackingLabel";
            this.employeesProductivityLossTrackingLabel.Size = new System.Drawing.Size(456, 25);
            this.employeesProductivityLossTrackingLabel.TabIndex = 45;
            this.employeesProductivityLossTrackingLabel.Text = "Employees Productivity Loss Tracking";
            this.employeesProductivityLossTrackingLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // performanceDeviationAgainstPredefinedThresholdsPanel
            // 
            this.performanceDeviationAgainstPredefinedThresholdsPanel.AutoScroll = true;
            this.performanceDeviationAgainstPredefinedThresholdsPanel.BackColor = System.Drawing.Color.Transparent;
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Location = new System.Drawing.Point(12, 143);
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Name = "performanceDeviationAgainstPredefinedThresholdsPanel";
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Size = new System.Drawing.Size(456, 40);
            this.performanceDeviationAgainstPredefinedThresholdsPanel.TabIndex = 44;
            this.performanceDeviationAgainstPredefinedThresholdsPanel.Tag = "PerformanceDeviationAgainstPredefinedThresholds";
            this.performanceDeviationAgainstPredefinedThresholdsPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B3ReportPanelClick);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(456, 15);
            this.label2.TabIndex = 43;
            // 
            // frequentExceptionsIncurredByEmployeesPanel
            // 
            this.frequentExceptionsIncurredByEmployeesPanel.AutoScroll = true;
            this.frequentExceptionsIncurredByEmployeesPanel.BackColor = System.Drawing.Color.Transparent;
            this.frequentExceptionsIncurredByEmployeesPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frequentExceptionsIncurredByEmployeesPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.frequentExceptionsIncurredByEmployeesPanel.Location = new System.Drawing.Point(12, 88);
            this.frequentExceptionsIncurredByEmployeesPanel.Name = "frequentExceptionsIncurredByEmployeesPanel";
            this.frequentExceptionsIncurredByEmployeesPanel.Size = new System.Drawing.Size(456, 40);
            this.frequentExceptionsIncurredByEmployeesPanel.TabIndex = 42;
            this.frequentExceptionsIncurredByEmployeesPanel.Tag = "FrequentExceptionsIncurredByEmployees";
            this.frequentExceptionsIncurredByEmployeesPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B2ReportPanelClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 15);
            this.label1.TabIndex = 41;
            // 
            // frequentExceptionCategoriesIncurredPanel
            // 
            this.frequentExceptionCategoriesIncurredPanel.AutoScroll = true;
            this.frequentExceptionCategoriesIncurredPanel.BackColor = System.Drawing.Color.Transparent;
            this.frequentExceptionCategoriesIncurredPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.frequentExceptionCategoriesIncurredPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.frequentExceptionCategoriesIncurredPanel.Location = new System.Drawing.Point(12, 33);
            this.frequentExceptionCategoriesIncurredPanel.Name = "frequentExceptionCategoriesIncurredPanel";
            this.frequentExceptionCategoriesIncurredPanel.Size = new System.Drawing.Size(456, 40);
            this.frequentExceptionCategoriesIncurredPanel.TabIndex = 40;
            this.frequentExceptionCategoriesIncurredPanel.Tag = "FrequentExceptionCategoriesIncurred";
            this.frequentExceptionCategoriesIncurredPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B1ReportPanelClick);
            // 
            // exceptionTrackingLabel
            // 
            this.exceptionTrackingLabel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionTrackingLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionTrackingLabel.ForeColor = System.Drawing.Color.DimGray;
            this.exceptionTrackingLabel.Location = new System.Drawing.Point(12, 18);
            this.exceptionTrackingLabel.Name = "exceptionTrackingLabel";
            this.exceptionTrackingLabel.Size = new System.Drawing.Size(456, 15);
            this.exceptionTrackingLabel.TabIndex = 39;
            this.exceptionTrackingLabel.Text = "Exception Tracking";
            this.exceptionTrackingLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IndexPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.employeeProductivityImprovementTrendPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.employeeProductivityLossRankingPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.highAlertTop2EmployeesExceptionMonitoringPanel);
            this.Controls.Add(this.employeesProductivityLossTrackingLabel);
            this.Controls.Add(this.performanceDeviationAgainstPredefinedThresholdsPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.frequentExceptionsIncurredByEmployeesPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.frequentExceptionCategoriesIncurredPanel);
            this.Controls.Add(this.exceptionTrackingLabel);
            this.Name = "IndexPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel employeeProductivityImprovementTrendPanel;
        private System.Windows.Forms.Label label4;
        private PanelBase.DoubleBufferPanel employeeProductivityLossRankingPanel;
        private System.Windows.Forms.Label label3;
        private PanelBase.DoubleBufferPanel highAlertTop2EmployeesExceptionMonitoringPanel;
        private System.Windows.Forms.Label employeesProductivityLossTrackingLabel;
        private PanelBase.DoubleBufferPanel performanceDeviationAgainstPredefinedThresholdsPanel;
        private System.Windows.Forms.Label label2;
        private PanelBase.DoubleBufferPanel frequentExceptionsIncurredByEmployeesPanel;
        private System.Windows.Forms.Label label1;
        private PanelBase.DoubleBufferPanel frequentExceptionCategoriesIncurredPanel;
        private System.Windows.Forms.Label exceptionTrackingLabel;

    }
}
