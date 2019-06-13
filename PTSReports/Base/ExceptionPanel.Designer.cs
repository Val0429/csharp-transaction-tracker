namespace PTSReports.Base
{
    sealed partial class ExceptionPanel
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
            this.filterPanel = new PanelBase.DoubleBufferPanel();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.filterLabel = new System.Windows.Forms.Label();
            this.filterPanel.SuspendLayout();
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
            // filterPanel
            // 
            this.filterPanel.AutoScroll = true;
            this.filterPanel.BackColor = System.Drawing.Color.Transparent;
            this.filterPanel.Controls.Add(this.filterComboBox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(12, 18);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(456, 40);
            this.filterPanel.TabIndex = 2;
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
            // filterLabel
            // 
            this.filterLabel.BackColor = System.Drawing.Color.Transparent;
            this.filterLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterLabel.Location = new System.Drawing.Point(12, 58);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(456, 15);
            this.filterLabel.TabIndex = 10;
            // 
            // ExceptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.filterPanel);
            this.DoubleBuffered = true;
            this.Name = "ExceptionPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.filterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel filterPanel;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.ComboBox filterComboBox;
    }
}
