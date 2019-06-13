using System.Drawing;

namespace EventPanel
{
    partial class PriorityEventPanel
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
            this.eventListPanel = new System.Windows.Forms.Panel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // eventListPanel
            // 
            this.eventListPanel.AutoScroll = true;
            this.eventListPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.eventListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventListPanel.Location = new System.Drawing.Point(0, 42);
            this.eventListPanel.Name = "eventListPanel";
            this.eventListPanel.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.eventListPanel.Size = new System.Drawing.Size(220, 108);
            this.eventListPanel.TabIndex = 2;
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(220, 42);
            this.titlePanel.TabIndex = 1;
            // 
            // EventPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.eventListPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "EventPanel";
            this.Size = new System.Drawing.Size(220, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Panel eventListPanel;
    }
}
