namespace SetupLicense
{
    sealed partial class InfoControl
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
            this.descriptionPanel = new PanelBase.DoubleBufferPanel();
            this.macPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // descriptionPanel
            // 
            this.descriptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.descriptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.descriptionPanel.Location = new System.Drawing.Point(0, 15);
            this.descriptionPanel.Name = "descriptionPanel";
            this.descriptionPanel.Size = new System.Drawing.Size(408, 40);
            this.descriptionPanel.TabIndex = 8;
            this.descriptionPanel.Tag = "Description";
            // 
            // macPanel
            // 
            this.macPanel.BackColor = System.Drawing.Color.Transparent;
            this.macPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.macPanel.Location = new System.Drawing.Point(0, 55);
            this.macPanel.Name = "macPanel";
            this.macPanel.Size = new System.Drawing.Size(408, 40);
            this.macPanel.TabIndex = 7;
            this.macPanel.Tag = "MAC";
            // 
            // InfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.macPanel);
            this.Controls.Add(this.descriptionPanel);
            this.Name = "InfoControl";
            this.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.Size = new System.Drawing.Size(408, 145);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel descriptionPanel;
        private PanelBase.DoubleBufferPanel macPanel;
    }
}
