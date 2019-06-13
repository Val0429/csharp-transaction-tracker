namespace PTSReports.ExceptionTrend
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
            this.periodDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.periodComboBox = new System.Windows.Forms.ComboBox();
            this.chartDoubleBufferPanel = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.exceptionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.periodLabel = new System.Windows.Forms.Label();
            this.resultrPanel = new PanelBase.DoubleBufferPanel();
            this.foundLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.periodDoubleBufferPanel.SuspendLayout();
            this.resultrPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // periodDoubleBufferPanel
            // 
            this.periodDoubleBufferPanel.AutoScroll = true;
            this.periodDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.periodDoubleBufferPanel.Controls.Add(this.periodComboBox);
            this.periodDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.periodDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.periodDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.periodDoubleBufferPanel.Name = "periodDoubleBufferPanel";
            this.periodDoubleBufferPanel.Size = new System.Drawing.Size(439, 40);
            this.periodDoubleBufferPanel.TabIndex = 29;
            this.periodDoubleBufferPanel.Tag = "POS";
            // 
            // periodComboBox
            // 
            this.periodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.periodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.periodComboBox.FormattingEnabled = true;
            this.periodComboBox.Location = new System.Drawing.Point(301, 9);
            this.periodComboBox.Name = "periodComboBox";
            this.periodComboBox.Size = new System.Drawing.Size(121, 20);
            this.periodComboBox.TabIndex = 0;
            // 
            // chartDoubleBufferPanel
            // 
            this.chartDoubleBufferPanel.AutoScroll = true;
            this.chartDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.chartDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chartDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDoubleBufferPanel.Location = new System.Drawing.Point(12, 113);
            this.chartDoubleBufferPanel.MinimumSize = new System.Drawing.Size(0, 190);
            this.chartDoubleBufferPanel.Name = "chartDoubleBufferPanel";
            this.chartDoubleBufferPanel.Size = new System.Drawing.Size(439, 217);
            this.chartDoubleBufferPanel.TabIndex = 30;
            this.chartDoubleBufferPanel.Tag = "Exception";
            // 
            // exceptionDoubleBufferPanel
            // 
            this.exceptionDoubleBufferPanel.AutoScroll = true;
            this.exceptionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exceptionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
            this.exceptionDoubleBufferPanel.Name = "exceptionDoubleBufferPanel";
            this.exceptionDoubleBufferPanel.Size = new System.Drawing.Size(439, 40);
            this.exceptionDoubleBufferPanel.TabIndex = 31;
            this.exceptionDoubleBufferPanel.Tag = "Exception";
            // 
            // periodLabel
            // 
            this.periodLabel.BackColor = System.Drawing.Color.Transparent;
            this.periodLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.periodLabel.Location = new System.Drawing.Point(12, 58);
            this.periodLabel.Name = "periodLabel";
            this.periodLabel.Size = new System.Drawing.Size(439, 15);
            this.periodLabel.TabIndex = 32;
            // 
            // resultrPanel
            // 
            this.resultrPanel.BackColor = System.Drawing.Color.Transparent;
            this.resultrPanel.Controls.Add(this.foundLabel);
            this.resultrPanel.Controls.Add(this.resultLabel);
            this.resultrPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultrPanel.Location = new System.Drawing.Point(12, 113);
            this.resultrPanel.Name = "resultrPanel";
            this.resultrPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.resultrPanel.Size = new System.Drawing.Size(439, 20);
            this.resultrPanel.TabIndex = 33;
            // 
            // foundLabel
            // 
            this.foundLabel.AutoSize = true;
            this.foundLabel.BackColor = System.Drawing.Color.Transparent;
            this.foundLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.foundLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(177)))), ((int)(((byte)(224)))));
            this.foundLabel.Location = new System.Drawing.Point(64, 0);
            this.foundLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new System.Drawing.Size(52, 20);
            this.foundLabel.TabIndex = 7;
            this.foundLabel.Text = "1 Found";
            this.foundLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.resultLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultLabel.Location = new System.Drawing.Point(8, 0);
            this.resultLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.resultLabel.Size = new System.Drawing.Size(56, 20);
            this.resultLabel.TabIndex = 6;
            this.resultLabel.Text = "Result :";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // CriteriaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.chartDoubleBufferPanel);
            this.Controls.Add(this.resultrPanel);
            this.Controls.Add(this.exceptionDoubleBufferPanel);
            this.Controls.Add(this.periodLabel);
            this.Controls.Add(this.periodDoubleBufferPanel);
            this.Name = "CriteriaPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(463, 348);
            this.periodDoubleBufferPanel.ResumeLayout(false);
            this.resultrPanel.ResumeLayout(false);
            this.resultrPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel periodDoubleBufferPanel;
        private System.Windows.Forms.ComboBox periodComboBox;
        private PanelBase.DoubleBufferFlowLayoutPanel chartDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel exceptionDoubleBufferPanel;
        private System.Windows.Forms.Label periodLabel;
        private PanelBase.DoubleBufferPanel resultrPanel;
        private System.Windows.Forms.Label foundLabel;
        private System.Windows.Forms.Label resultLabel;
    }
}
