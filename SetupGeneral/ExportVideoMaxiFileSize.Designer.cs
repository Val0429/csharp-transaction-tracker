namespace SetupGeneral
{
    sealed partial class ExportVideoMaxiFileSize
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
            this.customPanel = new PanelBase.DoubleBufferPanel();
            this.fileSizeTextBox = new PanelBase.HotKeyTextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.customPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 83);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 299);
            this.containerPanel.TabIndex = 1;
            // 
            // customPanel
            // 
            this.customPanel.BackColor = System.Drawing.Color.Transparent;
            this.customPanel.Controls.Add(this.fileSizeTextBox);
            this.customPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel.Location = new System.Drawing.Point(12, 18);
            this.customPanel.Name = "customPanel";
            this.customPanel.Size = new System.Drawing.Size(456, 40);
            this.customPanel.TabIndex = 7;
            this.customPanel.Tag = "Custom";
            // 
            // fileSizeTextBox
            // 
            this.fileSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSizeTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSizeTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.fileSizeTextBox.Location = new System.Drawing.Point(355, 9);
            this.fileSizeTextBox.MaxLength = 4;
            this.fileSizeTextBox.Name = "fileSizeTextBox";
            this.fileSizeTextBox.Size = new System.Drawing.Size(43, 21);
            this.fileSizeTextBox.TabIndex = 0;
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(12, 58);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(456, 25);
            this.infoLabel.TabIndex = 8;
            this.infoLabel.Text = "Maximum file size should between %1 MB to %2 MB (1.8GB)";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExportVideoMaxiFileSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.customPanel);
            this.DoubleBuffered = true;
            this.Name = "ExportVideoMaxiFileSize";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.customPanel.ResumeLayout(false);
            this.customPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel customPanel;
        private System.Windows.Forms.Label infoLabel;
        private PanelBase.HotKeyTextBox fileSizeTextBox;


    }
}
