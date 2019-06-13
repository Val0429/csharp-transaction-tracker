namespace PTSReports.AdvancedSearch
{
    sealed partial class ExceptionAmountPanel
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
            this.filterPanel = new PanelBase.DoubleBufferPanel();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.conditionalDoubleBufferPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 113);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 269);
            this.containerPanel.TabIndex = 1;
            // 
            // conditionalLabel
            // 
            this.conditionalLabel.BackColor = System.Drawing.Color.Transparent;
            this.conditionalLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionalLabel.Location = new System.Drawing.Point(12, 98);
            this.conditionalLabel.Name = "conditionalLabel";
            this.conditionalLabel.Size = new System.Drawing.Size(456, 15);
            this.conditionalLabel.TabIndex = 13;
            // 
            // conditionalDoubleBufferPanel
            // 
            this.conditionalDoubleBufferPanel.AutoScroll = true;
            this.conditionalDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.conditionalDoubleBufferPanel.Controls.Add(this.conditionalComboBox);
            this.conditionalDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.conditionalDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionalDoubleBufferPanel.Location = new System.Drawing.Point(12, 58);
            this.conditionalDoubleBufferPanel.Name = "conditionalDoubleBufferPanel";
            this.conditionalDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.conditionalDoubleBufferPanel.TabIndex = 12;
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
            // filterPanel
            // 
            this.filterPanel.AutoScroll = true;
            this.filterPanel.BackColor = System.Drawing.Color.Transparent;
            this.filterPanel.Controls.Add(this.filterComboBox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(12, 18);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(456, 40);
            this.filterPanel.TabIndex = 14;
            // 
            // filterComboBox
            // 
            this.filterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.Location = new System.Drawing.Point(315, 9);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(121, 23);
            this.filterComboBox.TabIndex = 0;
            // 
            // ExceptionAmountPanel
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
            this.Controls.Add(this.filterPanel);
            this.DoubleBuffered = true;
            this.Name = "ExceptionAmountPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.conditionalDoubleBufferPanel.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label conditionalLabel;
        private PanelBase.DoubleBufferPanel conditionalDoubleBufferPanel;
        private System.Windows.Forms.ComboBox conditionalComboBox;
        private PanelBase.DoubleBufferPanel filterPanel;
        private System.Windows.Forms.ComboBox filterComboBox;


    }
}
