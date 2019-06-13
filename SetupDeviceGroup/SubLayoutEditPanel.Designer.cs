namespace SetupDeviceGroup
{
    sealed partial class SubLayoutEditPanel
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
            this.layoutLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.subDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // layoutLabel
            // 
            this.layoutLabel.BackColor = System.Drawing.Color.Transparent;
            this.layoutLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layoutLabel.Location = new System.Drawing.Point(12, 258);
            this.layoutLabel.Name = "layoutLabel";
            this.layoutLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.layoutLabel.Size = new System.Drawing.Size(456, 25);
            this.layoutLabel.TabIndex = 14;
            this.layoutLabel.Text = "layout";
            this.layoutLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 283);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 117);
            this.containerPanel.TabIndex = 2;
            // 
            // subDoubleBufferPanel
            // 
            this.subDoubleBufferPanel.AutoScroll = true;
            this.subDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.subDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.subDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.subDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.subDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.subDoubleBufferPanel.Name = "subDoubleBufferPanel";
            this.subDoubleBufferPanel.Size = new System.Drawing.Size(456, 240);
            this.subDoubleBufferPanel.TabIndex = 13;
            this.subDoubleBufferPanel.Tag = "";
            // 
            // SubLayoutEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.layoutLabel);
            this.Controls.Add(this.subDoubleBufferPanel);
            this.Name = "SubLayoutEditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 0);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label layoutLabel;
        private PanelBase.DoubleBufferPanel subDoubleBufferPanel;

    }
}
