namespace SetupNVR
{
    partial class SearchPanel
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
            this.manufacturesLabel = new System.Windows.Forms.Label();
            this.manufacturesPanel = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 10);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.containerPanel.Size = new System.Drawing.Size(456, 372);
            this.containerPanel.TabIndex = 2;
            // 
            // manufacturesLabel
            // 
            this.manufacturesLabel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturesLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manufacturesLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.manufacturesLabel.Location = new System.Drawing.Point(12, 10);
            this.manufacturesLabel.Name = "manufacturesLabel";
            this.manufacturesLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.manufacturesLabel.Size = new System.Drawing.Size(456, 20);
            this.manufacturesLabel.TabIndex = 5;
            this.manufacturesLabel.Text = "Manufacturers";
            this.manufacturesLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // manufacturesPanel
            // 
            this.manufacturesPanel.AutoSize = true;
            this.manufacturesPanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturesPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturesPanel.ForeColor = System.Drawing.Color.Black;
            this.manufacturesPanel.Location = new System.Drawing.Point(12, 30);
            this.manufacturesPanel.MinimumSize = new System.Drawing.Size(400, 50);
            this.manufacturesPanel.Name = "manufacturesPanel";
            this.manufacturesPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.manufacturesPanel.Size = new System.Drawing.Size(456, 50);
            this.manufacturesPanel.TabIndex = 6;
            // 
            // SearchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.manufacturesPanel);
            this.Controls.Add(this.manufacturesLabel);
            this.Name = "SearchPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 10, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label manufacturesLabel;
        private PanelBase.DoubleBufferFlowLayoutPanel manufacturesPanel;

    }
}
