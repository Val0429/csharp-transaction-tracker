namespace SetupDeviceGroup
{
    sealed partial class LayoutListPanel
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
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.addedLayoutLabel = new System.Windows.Forms.Label();
			this.addNewLabel = new System.Windows.Forms.Label();
			this.immervisionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.containerPanel.Location = new System.Drawing.Point(12, 138);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
			this.containerPanel.Size = new System.Drawing.Size(456, 244);
            this.containerPanel.TabIndex = 2;
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.addNewDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
			this.addNewDoubleBufferPanel.Tag = "AddNewDeviceLayout";
            // 
            // addedGroupLabel
            // 
            this.addedLayoutLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedLayoutLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedLayoutLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedLayoutLabel.ForeColor = System.Drawing.Color.DimGray;
			this.addedLayoutLabel.Location = new System.Drawing.Point(12, 113);
            this.addedLayoutLabel.Name = "addedLayoutLabel";
            this.addedLayoutLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedLayoutLabel.Size = new System.Drawing.Size(456, 25);
            this.addedLayoutLabel.TabIndex = 9;
            this.addedLayoutLabel.Text = "Added group";
            this.addedLayoutLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// addNewLabel
			// 
			this.addNewLabel.BackColor = System.Drawing.Color.Transparent;
			this.addNewLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.addNewLabel.Location = new System.Drawing.Point(12, 58);
			this.addNewLabel.Name = "addNewLabel";
			this.addNewLabel.Size = new System.Drawing.Size(456, 15);
			this.addNewLabel.TabIndex = 16;
			// 
			// immervisionDoubleBufferPanel
			// 
			this.immervisionDoubleBufferPanel.AutoScroll = true;
			this.immervisionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
			this.immervisionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.immervisionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.immervisionDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
			this.immervisionDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
			this.immervisionDoubleBufferPanel.Name = "immervisionDoubleBufferPanel";
			this.immervisionDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
			this.immervisionDoubleBufferPanel.TabIndex = 17;
			this.immervisionDoubleBufferPanel.Tag = "ImmervisionLayout";
			// 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedLayoutLabel);
			this.Controls.Add(this.immervisionDoubleBufferPanel);
			this.Controls.Add(this.addNewLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
			this.Name = "LayoutListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label addedLayoutLabel;
		private System.Windows.Forms.Label addNewLabel;
		private PanelBase.DoubleBufferPanel immervisionDoubleBufferPanel;

    }
}
