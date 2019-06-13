namespace SetupServer
{
    sealed partial class DatabaseControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.settingPanel = new PanelBase.DoubleBufferPanel();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.domainPanel = new PanelBase.DoubleBufferPanel();
            this.domainTextBox = new PanelBase.HotKeyTextBox();
            this.keepDaysPanel = new PanelBase.DoubleBufferPanel();
            this.keepMonthsTextBox = new System.Windows.Forms.TextBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.settingPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.domainPanel.SuspendLayout();
            this.keepDaysPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 15);
            this.label1.TabIndex = 15;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 153);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 40);
            this.containerPanel.TabIndex = 16;
            this.containerPanel.Visible = false;
            // 
            // settingPanel
            // 
            this.settingPanel.AutoScroll = true;
            this.settingPanel.AutoSize = true;
            this.settingPanel.BackColor = System.Drawing.Color.Transparent;
            this.settingPanel.Controls.Add(this.domainPanel);
            this.settingPanel.Controls.Add(this.keepDaysPanel);
            this.settingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingPanel.Location = new System.Drawing.Point(12, 18);
            this.settingPanel.Name = "settingPanel";
            this.settingPanel.Size = new System.Drawing.Size(456, 120);
            this.settingPanel.TabIndex = 19;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.portTextBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 80);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(456, 40);
            this.portPanel.TabIndex = 20;
            this.portPanel.Tag = "Port";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portTextBox.Location = new System.Drawing.Point(385, 9);
            this.portTextBox.MaxLength = 5;
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(51, 21);
            this.portTextBox.TabIndex = 4;
            // 
            // domainPanel
            // 
            this.domainPanel.BackColor = System.Drawing.Color.Transparent;
            this.domainPanel.Controls.Add(this.domainTextBox);
            this.domainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.domainPanel.Enabled = false;
            this.domainPanel.Location = new System.Drawing.Point(0, 40);
            this.domainPanel.Name = "domainPanel";
            this.domainPanel.Size = new System.Drawing.Size(456, 40);
            this.domainPanel.TabIndex = 19;
            this.domainPanel.Tag = "Domain";
            this.domainPanel.Visible = false;
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainTextBox.Location = new System.Drawing.Point(260, 10);
            this.domainTextBox.MaxLength = 80;
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.ShortcutsEnabled = false;
            this.domainTextBox.Size = new System.Drawing.Size(181, 21);
            this.domainTextBox.TabIndex = 3;
            // 
            // keepDaysPanel
            // 
            this.keepDaysPanel.BackColor = System.Drawing.Color.Transparent;
            this.keepDaysPanel.Controls.Add(this.keepMonthsTextBox);
            this.keepDaysPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.keepDaysPanel.Location = new System.Drawing.Point(0, 0);
            this.keepDaysPanel.Name = "keepDaysPanel";
            this.keepDaysPanel.Size = new System.Drawing.Size(456, 40);
            this.keepDaysPanel.TabIndex = 23;
            this.keepDaysPanel.Tag = "KeepDays";
            // 
            // keepMonthsTextBox
            // 
            this.keepMonthsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keepMonthsTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keepMonthsTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.keepMonthsTextBox.Location = new System.Drawing.Point(385, 9);
            this.keepMonthsTextBox.MaxLength = 2;
            this.keepMonthsTextBox.Name = "keepMonthsTextBox";
            this.keepMonthsTextBox.Size = new System.Drawing.Size(56, 21);
            this.keepMonthsTextBox.TabIndex = 0;
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.Transparent;
            this.warningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.warningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(12, 193);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(456, 20);
            this.warningLabel.TabIndex = 20;
            this.warningLabel.Text = "Do not set \"Server port\" and \"Database port\" to be equal";
            this.warningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.warningLabel.Visible = false;
            // 
            // DatabaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.portPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingPanel);
            this.DoubleBuffered = true;
            this.Name = "DatabaseControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 417);
            this.settingPanel.ResumeLayout(false);
            this.portPanel.ResumeLayout(false);
            this.portPanel.PerformLayout();
            this.domainPanel.ResumeLayout(false);
            this.domainPanel.PerformLayout();
            this.keepDaysPanel.ResumeLayout(false);
            this.keepDaysPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel settingPanel;
        private PanelBase.DoubleBufferPanel portPanel;
        private System.Windows.Forms.TextBox portTextBox;
        private PanelBase.DoubleBufferPanel domainPanel;
        private PanelBase.HotKeyTextBox domainTextBox;
        private PanelBase.DoubleBufferPanel keepDaysPanel;
        private System.Windows.Forms.TextBox keepMonthsTextBox;
        private System.Windows.Forms.Label warningLabel;


    }
}
