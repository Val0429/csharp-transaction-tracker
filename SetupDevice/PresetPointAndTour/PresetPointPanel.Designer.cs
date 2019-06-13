namespace SetupDevice
{
    partial class PresetPointPanel
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
            this.videoWindowPanel = new System.Windows.Forms.Panel();
            this.monitorLabel = new System.Windows.Forms.Label();
            this.pointPanel = new System.Windows.Forms.Panel();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.videoWindowPanel);
            this.containerPanel.Controls.Add(this.monitorLabel);
            this.containerPanel.Controls.Add(this.pointPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(476, 364);
            this.containerPanel.TabIndex = 20;
            // 
            // videoWindowPanel
            // 
            this.videoWindowPanel.BackColor = System.Drawing.Color.DimGray;
            this.videoWindowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoWindowPanel.Location = new System.Drawing.Point(0, 0);
            this.videoWindowPanel.Name = "videoWindowPanel";
            this.videoWindowPanel.Size = new System.Drawing.Size(476, 154);
            this.videoWindowPanel.TabIndex = 20;
            // 
            // monitorLabel
            // 
            this.monitorLabel.BackColor = System.Drawing.Color.Transparent;
            this.monitorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.monitorLabel.Location = new System.Drawing.Point(0, 154);
            this.monitorLabel.Name = "monitorLabel";
            this.monitorLabel.Size = new System.Drawing.Size(476, 10);
            this.monitorLabel.TabIndex = 23;
            // 
            // pointPanel
            // 
            this.pointPanel.AutoScroll = true;
            this.pointPanel.BackColor = System.Drawing.Color.Transparent;
            this.pointPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pointPanel.Location = new System.Drawing.Point(0, 164);
            this.pointPanel.Name = "pointPanel";
            this.pointPanel.Size = new System.Drawing.Size(476, 200);
            this.pointPanel.TabIndex = 22;
            // 
            // PresetPointPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "PresetPointPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(500, 400);
            this.containerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Panel videoWindowPanel;
        private System.Windows.Forms.Panel pointPanel;
        private System.Windows.Forms.Label monitorLabel;
    }
}
