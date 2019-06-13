namespace SetupGeneral
{
	sealed partial class DefaultView
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
			this.SuspendLayout();
			// 
			// containerPanel
			// 
			this.containerPanel.AutoScroll = true;
			this.containerPanel.BackColor = System.Drawing.Color.Transparent;
			this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.containerPanel.Location = new System.Drawing.Point(12, 18);
			this.containerPanel.Name = "containerPanel";
			this.containerPanel.Size = new System.Drawing.Size(555, 494);
			this.containerPanel.TabIndex = 1;
			// 
			// DefaultView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = global::SetupGeneral.Properties.Resources.bg_noborder;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.containerPanel);
			this.DoubleBuffered = true;
			this.Name = "DefaultView";
			this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
			this.Size = new System.Drawing.Size(579, 530);
			this.ResumeLayout(false);

        }

        #endregion

		private PanelBase.DoubleBufferPanel containerPanel;



    }
}
