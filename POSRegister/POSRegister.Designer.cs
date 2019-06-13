namespace POSRegister
{
    partial class POSRegister
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.viewModelPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(218, 42);
            this.titlePanel.TabIndex = 3;
            // 
            // viewModelPanel
            // 
            this.viewModelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.viewModelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewModelPanel.Location = new System.Drawing.Point(0, 42);
            this.viewModelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.viewModelPanel.Name = "viewModelPanel";
            this.viewModelPanel.Size = new System.Drawing.Size(218, 263);
            this.viewModelPanel.TabIndex = 4;
            // 
            // POSRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.viewModelPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "POSRegister";
            this.Size = new System.Drawing.Size(218, 305);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel viewModelPanel;
    }
}
