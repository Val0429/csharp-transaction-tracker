namespace SetupExceptionReport
{
    sealed partial class ExceptionReportEditPanel
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
            this.settingPanel = new PanelBase.DoubleBufferPanel();
            this.incrementSelectPanel = new PanelBase.DoubleBufferPanel();
            this.incrementComboBox = new System.Windows.Forms.ComboBox();
            this.thresholdPanel = new PanelBase.DoubleBufferPanel();
            this.thresholdComboBox = new System.Windows.Forms.ComboBox();
            this.formatPanel = new PanelBase.DoubleBufferPanel();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.exceptionPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionComboBox = new System.Windows.Forms.ComboBox();
            this.mailPanel = new PanelBase.DoubleBufferPanel();
            this.subjectPanel = new PanelBase.DoubleBufferPanel();
            this.subjectTextBox = new PanelBase.HotKeyTextBox();
            this.recipientPanel = new PanelBase.DoubleBufferPanel();
            this.receiverComboBox = new System.Windows.Forms.ComboBox();
            this.mailLabel = new System.Windows.Forms.Label();
            this.settingPanel.SuspendLayout();
            this.incrementSelectPanel.SuspendLayout();
            this.thresholdPanel.SuspendLayout();
            this.formatPanel.SuspendLayout();
            this.exceptionPanel.SuspendLayout();
            this.mailPanel.SuspendLayout();
            this.subjectPanel.SuspendLayout();
            this.recipientPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingPanel
            // 
            this.settingPanel.AutoSize = true;
            this.settingPanel.BackColor = System.Drawing.Color.Transparent;
            this.settingPanel.Controls.Add(this.incrementSelectPanel);
            this.settingPanel.Controls.Add(this.thresholdPanel);
            this.settingPanel.Controls.Add(this.formatPanel);
            this.settingPanel.Controls.Add(this.exceptionPanel);
            this.settingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingPanel.Location = new System.Drawing.Point(12, 18);
            this.settingPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.settingPanel.Name = "settingPanel";
            this.settingPanel.Size = new System.Drawing.Size(528, 160);
            this.settingPanel.TabIndex = 17;
            this.settingPanel.Tag = "";
            // 
            // incrementSelectPanel
            // 
            this.incrementSelectPanel.BackColor = System.Drawing.Color.Transparent;
            this.incrementSelectPanel.Controls.Add(this.incrementComboBox);
            this.incrementSelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.incrementSelectPanel.Location = new System.Drawing.Point(0, 120);
            this.incrementSelectPanel.Name = "incrementSelectPanel";
            this.incrementSelectPanel.Size = new System.Drawing.Size(528, 40);
            this.incrementSelectPanel.TabIndex = 17;
            this.incrementSelectPanel.Tag = "Increment";
            // 
            // incrementComboBox
            // 
            this.incrementComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.incrementComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.incrementComboBox.FormattingEnabled = true;
            this.incrementComboBox.Location = new System.Drawing.Point(407, 8);
            this.incrementComboBox.Name = "incrementComboBox";
            this.incrementComboBox.Size = new System.Drawing.Size(106, 23);
            this.incrementComboBox.TabIndex = 0;
            // 
            // thresholdPanel
            // 
            this.thresholdPanel.BackColor = System.Drawing.Color.Transparent;
            this.thresholdPanel.Controls.Add(this.thresholdComboBox);
            this.thresholdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.thresholdPanel.Location = new System.Drawing.Point(0, 80);
            this.thresholdPanel.Name = "thresholdPanel";
            this.thresholdPanel.Size = new System.Drawing.Size(528, 40);
            this.thresholdPanel.TabIndex = 18;
            this.thresholdPanel.Tag = "Threshold";
            // 
            // thresholdComboBox
            // 
            this.thresholdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thresholdComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thresholdComboBox.FormattingEnabled = true;
            this.thresholdComboBox.Location = new System.Drawing.Point(407, 8);
            this.thresholdComboBox.Name = "thresholdComboBox";
            this.thresholdComboBox.Size = new System.Drawing.Size(106, 23);
            this.thresholdComboBox.TabIndex = 0;
            // 
            // formatPanel
            // 
            this.formatPanel.BackColor = System.Drawing.Color.Transparent;
            this.formatPanel.Controls.Add(this.formatComboBox);
            this.formatPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.formatPanel.Location = new System.Drawing.Point(0, 40);
            this.formatPanel.Name = "formatPanel";
            this.formatPanel.Size = new System.Drawing.Size(528, 40);
            this.formatPanel.TabIndex = 19;
            this.formatPanel.Tag = "Format";
            // 
            // formatComboBox
            // 
            this.formatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Location = new System.Drawing.Point(407, 8);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(106, 23);
            this.formatComboBox.TabIndex = 0;
            // 
            // exceptionPanel
            // 
            this.exceptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionPanel.Controls.Add(this.exceptionComboBox);
            this.exceptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionPanel.Location = new System.Drawing.Point(0, 0);
            this.exceptionPanel.Name = "exceptionPanel";
            this.exceptionPanel.Size = new System.Drawing.Size(528, 40);
            this.exceptionPanel.TabIndex = 13;
            this.exceptionPanel.Tag = "Exception";
            // 
            // exceptionComboBox
            // 
            this.exceptionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exceptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exceptionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionComboBox.FormattingEnabled = true;
            this.exceptionComboBox.Location = new System.Drawing.Point(407, 8);
            this.exceptionComboBox.Name = "exceptionComboBox";
            this.exceptionComboBox.Size = new System.Drawing.Size(106, 23);
            this.exceptionComboBox.TabIndex = 0;
            // 
            // mailPanel
            // 
            this.mailPanel.AutoSize = true;
            this.mailPanel.BackColor = System.Drawing.Color.Transparent;
            this.mailPanel.Controls.Add(this.subjectPanel);
            this.mailPanel.Controls.Add(this.recipientPanel);
            this.mailPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mailPanel.Location = new System.Drawing.Point(12, 203);
            this.mailPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.mailPanel.Name = "mailPanel";
            this.mailPanel.Size = new System.Drawing.Size(528, 80);
            this.mailPanel.TabIndex = 18;
            this.mailPanel.Tag = "";
            // 
            // subjectPanel
            // 
            this.subjectPanel.BackColor = System.Drawing.Color.Transparent;
            this.subjectPanel.Controls.Add(this.subjectTextBox);
            this.subjectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.subjectPanel.Location = new System.Drawing.Point(0, 40);
            this.subjectPanel.Name = "subjectPanel";
            this.subjectPanel.Size = new System.Drawing.Size(528, 40);
            this.subjectPanel.TabIndex = 26;
            this.subjectPanel.Tag = "Subject";
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subjectTextBox.Location = new System.Drawing.Point(332, 8);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(181, 21);
            this.subjectTextBox.TabIndex = 2;
            // 
            // recipientPanel
            // 
            this.recipientPanel.BackColor = System.Drawing.Color.Transparent;
            this.recipientPanel.Controls.Add(this.receiverComboBox);
            this.recipientPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.recipientPanel.Location = new System.Drawing.Point(0, 0);
            this.recipientPanel.Name = "recipientPanel";
            this.recipientPanel.Size = new System.Drawing.Size(528, 40);
            this.recipientPanel.TabIndex = 25;
            this.recipientPanel.Tag = "Recipient";
            // 
            // receiverComboBox
            // 
            this.receiverComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.receiverComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.receiverComboBox.FormattingEnabled = true;
            this.receiverComboBox.Location = new System.Drawing.Point(332, 8);
            this.receiverComboBox.Name = "receiverComboBox";
            this.receiverComboBox.Size = new System.Drawing.Size(181, 23);
            this.receiverComboBox.TabIndex = 3;
            // 
            // mailLabel
            // 
            this.mailLabel.BackColor = System.Drawing.Color.Transparent;
            this.mailLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mailLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mailLabel.ForeColor = System.Drawing.Color.DimGray;
            this.mailLabel.Location = new System.Drawing.Point(12, 178);
            this.mailLabel.Name = "mailLabel";
            this.mailLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.mailLabel.Size = new System.Drawing.Size(528, 25);
            this.mailLabel.TabIndex = 19;
            this.mailLabel.Text = "Send mail";
            this.mailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ExceptionReportEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SetupExceptionReport.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.mailPanel);
            this.Controls.Add(this.mailLabel);
            this.Controls.Add(this.settingPanel);
            this.DoubleBuffered = true;
            this.Name = "ExceptionReportEditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(552, 526);
            this.settingPanel.ResumeLayout(false);
            this.incrementSelectPanel.ResumeLayout(false);
            this.thresholdPanel.ResumeLayout(false);
            this.formatPanel.ResumeLayout(false);
            this.exceptionPanel.ResumeLayout(false);
            this.mailPanel.ResumeLayout(false);
            this.subjectPanel.ResumeLayout(false);
            this.subjectPanel.PerformLayout();
            this.recipientPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel settingPanel;
        private PanelBase.DoubleBufferPanel thresholdPanel;
        private System.Windows.Forms.ComboBox thresholdComboBox;
        private PanelBase.DoubleBufferPanel incrementSelectPanel;
        private System.Windows.Forms.ComboBox incrementComboBox;
        private PanelBase.DoubleBufferPanel exceptionPanel;
        private System.Windows.Forms.ComboBox exceptionComboBox;
        private PanelBase.DoubleBufferPanel mailPanel;
        private PanelBase.DoubleBufferPanel subjectPanel;
        private PanelBase.HotKeyTextBox subjectTextBox;
        private PanelBase.DoubleBufferPanel recipientPanel;
        private System.Windows.Forms.Label mailLabel;
        private System.Windows.Forms.ComboBox receiverComboBox;
        private PanelBase.DoubleBufferPanel formatPanel;
        private System.Windows.Forms.ComboBox formatComboBox;


    }
}
