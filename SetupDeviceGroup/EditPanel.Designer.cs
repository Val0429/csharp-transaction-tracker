namespace SetupDeviceGroup
{
    sealed partial class EditPanel
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
            this.setLayoutDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 128);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 254);
            this.containerPanel.TabIndex = 2;
            // 
            // setLayoutDoubleBufferPanel
            // 
            this.setLayoutDoubleBufferPanel.AutoScroll = true;
            this.setLayoutDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.setLayoutDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setLayoutDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.setLayoutDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.setLayoutDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.setLayoutDoubleBufferPanel.Name = "setLayoutDoubleBufferPanel";
            this.setLayoutDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.setLayoutDoubleBufferPanel.TabIndex = 5;
            this.setLayoutDoubleBufferPanel.Tag = "SetGroupLayout";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.label1.Size = new System.Drawing.Size(456, 15);
            this.label1.TabIndex = 12;
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupPanel
            // 
            this.groupPanel.AutoScroll = true;
            this.groupPanel.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.groupPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel.Location = new System.Drawing.Point(12, 73);
            this.groupPanel.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(456, 55);
            this.groupPanel.TabIndex = 13;
            this.groupPanel.Tag = "";
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.groupPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.setLayoutDoubleBufferPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel setLayoutDoubleBufferPanel;
        private System.Windows.Forms.Label label1;
        private PanelBase.DoubleBufferPanel groupPanel;

    }
}
