namespace SetupDevice
{
    partial class PtzPatrolPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.intervalPanel = new PanelBase.DoubleBufferPanel();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.videoWindowPanel = new System.Windows.Forms.Panel();
            this.monitorLabel = new System.Windows.Forms.Label();
            this.pointPanel = new System.Windows.Forms.Panel();
            this.secLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.intervalPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.videoWindowPanel);
            this.containerPanel.Controls.Add(this.label1);
            this.containerPanel.Controls.Add(this.intervalPanel);
            this.containerPanel.Controls.Add(this.infoLabel);
            this.containerPanel.Controls.Add(this.monitorLabel);
            this.containerPanel.Controls.Add(this.pointPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(476, 364);
            this.containerPanel.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(476, 10);
            this.label1.TabIndex = 25;
            // 
            // intervalPanel
            // 
            this.intervalPanel.BackColor = System.Drawing.Color.Transparent;
            this.intervalPanel.Controls.Add(this.secLabel);
            this.intervalPanel.Controls.Add(this.intervalTextBox);
            this.intervalPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.intervalPanel.Location = new System.Drawing.Point(0, 114);
            this.intervalPanel.Name = "intervalPanel";
            this.intervalPanel.Size = new System.Drawing.Size(476, 40);
            this.intervalPanel.TabIndex = 24;
            this.intervalPanel.Tag = "PatrolInterval";
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.intervalTextBox.Location = new System.Drawing.Point(313, 10);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(100, 21);
            this.intervalTextBox.TabIndex = 0;
            // 
            // videoWindowPanel
            // 
            this.videoWindowPanel.BackColor = System.Drawing.Color.DimGray;
            this.videoWindowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoWindowPanel.Location = new System.Drawing.Point(0, 0);
            this.videoWindowPanel.Name = "videoWindowPanel";
            this.videoWindowPanel.Size = new System.Drawing.Size(476, 104);
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
            // secLabel
            // 
            this.secLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.secLabel.AutoSize = true;
            this.secLabel.Location = new System.Drawing.Point(421, 13);
            this.secLabel.Name = "secLabel";
            this.secLabel.Size = new System.Drawing.Size(35, 15);
            this.secLabel.TabIndex = 1;
            this.secLabel.Text = "Secs";
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(0, 79);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(476, 25);
            this.infoLabel.TabIndex = 26;
            this.infoLabel.Text = "Patrol interval should between 5 secs to 1 hour";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PtzPatrolPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "PtzPatrolPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(500, 400);
            this.containerPanel.ResumeLayout(false);
            this.intervalPanel.ResumeLayout(false);
            this.intervalPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Panel videoWindowPanel;
        private System.Windows.Forms.Panel pointPanel;
        private System.Windows.Forms.Label monitorLabel;
        private PanelBase.DoubleBufferPanel intervalPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox intervalTextBox;
        private System.Windows.Forms.Label secLabel;
        private System.Windows.Forms.Label infoLabel;
    }
}
