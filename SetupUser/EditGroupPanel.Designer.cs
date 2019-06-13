namespace SetupUser
{
    partial class EditGroupPanel
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
            this.functionPanel = new PanelBase.DoubleBufferPanel();
            this.functionLabel = new System.Windows.Forms.Label();
            this.permissionPanel = new PanelBase.DoubleBufferPanel();
            this.permissionLabel = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.functionPanel);
            this.containerPanel.Controls.Add(this.functionLabel);
            this.containerPanel.Controls.Add(this.permissionPanel);
            this.containerPanel.Controls.Add(this.permissionLabel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 364);
            this.containerPanel.TabIndex = 2;
            // 
            // functionPanel
            // 
            this.functionPanel.AutoScroll = true;
            this.functionPanel.AutoSize = true;
            this.functionPanel.BackColor = System.Drawing.Color.Transparent;
            this.functionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionPanel.Location = new System.Drawing.Point(0, 95);
            this.functionPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.functionPanel.Name = "functionPanel";
            this.functionPanel.Size = new System.Drawing.Size(456, 269);
            this.functionPanel.TabIndex = 20;
            this.functionPanel.Tag = "";
            // 
            // functionLabel
            // 
            this.functionLabel.BackColor = System.Drawing.Color.Transparent;
            this.functionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.functionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.functionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.functionLabel.Location = new System.Drawing.Point(0, 70);
            this.functionLabel.Name = "functionLabel";
            this.functionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.functionLabel.Size = new System.Drawing.Size(456, 25);
            this.functionLabel.TabIndex = 21;
            this.functionLabel.Text = "Setup Function";
            this.functionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // permissionPanel
            // 
            this.permissionPanel.AutoScroll = true;
            this.permissionPanel.AutoSize = true;
            this.permissionPanel.BackColor = System.Drawing.Color.Transparent;
            this.permissionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.permissionPanel.Location = new System.Drawing.Point(0, 20);
            this.permissionPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.permissionPanel.Name = "permissionPanel";
            this.permissionPanel.Size = new System.Drawing.Size(456, 50);
            this.permissionPanel.TabIndex = 17;
            this.permissionPanel.Tag = "";
            // 
            // permissionLabel
            // 
            this.permissionLabel.BackColor = System.Drawing.Color.Transparent;
            this.permissionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.permissionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.permissionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.permissionLabel.Location = new System.Drawing.Point(0, 0);
            this.permissionLabel.Name = "permissionLabel";
            this.permissionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.permissionLabel.Size = new System.Drawing.Size(456, 20);
            this.permissionLabel.TabIndex = 19;
            this.permissionLabel.Text = "Permission";
            this.permissionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // EditGroupPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditGroupPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected PanelBase.DoubleBufferPanel containerPanel;
        protected PanelBase.DoubleBufferPanel permissionPanel;
        protected System.Windows.Forms.Label permissionLabel;
        protected PanelBase.DoubleBufferPanel functionPanel;
        protected System.Windows.Forms.Label functionLabel;

    }
}
