namespace PTSReports.AdvancedSearch
{
    sealed partial class CashierPanel
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
            this.conditionalLabel = new System.Windows.Forms.Label();
            this.conditionalDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.conditionalComboBox = new System.Windows.Forms.ComboBox();
            this.conditionalDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 73);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 309);
            this.containerPanel.TabIndex = 1;
            // 
            // conditionalLabel
            // 
            this.conditionalLabel.BackColor = System.Drawing.Color.Transparent;
            this.conditionalLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionalLabel.Location = new System.Drawing.Point(12, 58);
            this.conditionalLabel.Name = "conditionalLabel";
            this.conditionalLabel.Size = new System.Drawing.Size(456, 15);
            this.conditionalLabel.TabIndex = 17;
            // 
            // conditionalDoubleBufferPanel
            // 
            this.conditionalDoubleBufferPanel.AutoScroll = true;
            this.conditionalDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionalDoubleBufferPanel.Controls.Add(this.conditionalComboBox);
            this.conditionalDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.conditionalDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionalDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.conditionalDoubleBufferPanel.Name = "conditionalDoubleBufferPanel";
            this.conditionalDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.conditionalDoubleBufferPanel.TabIndex = 16;
            this.conditionalDoubleBufferPanel.Tag = "SearchConditionsAssociated";
            // 
            // conditionalComboBox
            // 
            this.conditionalComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.conditionalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionalComboBox.FormattingEnabled = true;
            this.conditionalComboBox.Location = new System.Drawing.Point(375, 9);
            this.conditionalComboBox.Name = "conditionalComboBox";
            this.conditionalComboBox.Size = new System.Drawing.Size(64, 20);
            this.conditionalComboBox.TabIndex = 0;
            // 
            // CashierPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.conditionalLabel);
            this.Controls.Add(this.conditionalDoubleBufferPanel);
            this.DoubleBuffered = true;
            this.Name = "CashierPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.conditionalDoubleBufferPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label conditionalLabel;
        private PanelBase.DoubleBufferPanel conditionalDoubleBufferPanel;
        private System.Windows.Forms.ComboBox conditionalComboBox;


    }
}
