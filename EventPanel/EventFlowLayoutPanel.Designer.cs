using System.Drawing;

namespace EventPanel
{
    partial class EventFlowLayoutPanel
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
            this.eventListPanel = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // eventListPanel
            // 
            this.eventListPanel.AutoScroll = true;
            this.eventListPanel.BackColor = System.Drawing.Color.Black;
            this.eventListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventListPanel.Location = new System.Drawing.Point(0, 30);
            this.eventListPanel.Name = "eventListPanel";
            this.eventListPanel.Size = new System.Drawing.Size(200, 120);
            this.eventListPanel.TabIndex = 2;
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(200, 30);
            this.titlePanel.TabIndex = 1;
            // 
            // EventFlowLayoutPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.eventListPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "EventFlowLayoutPanel";
            this.Size = new System.Drawing.Size(200, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private PanelBase.DoubleBufferFlowLayoutPanel eventListPanel;
    }
}
