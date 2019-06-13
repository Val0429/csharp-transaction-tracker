using System.Drawing;

namespace SetupServer
{
    sealed partial class RAID
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.RAIDlabel = new System.Windows.Forms.Label();
            this.labelKeepSpace = new System.Windows.Forms.Label();
            this.DisksLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDisks = new System.Windows.Forms.Label();
            this.keepSpacePanel = new PanelBase.DoubleBufferPanel();
            this.GBlabel = new System.Windows.Forms.Label();
            this.textBoxKeepSpace = new PanelBase.HotKeyTextBox();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.RAID10Panel = new PanelBase.DoubleBufferPanel();
            this.RAID5Panel = new PanelBase.DoubleBufferPanel();
            this.RAID1Panel = new PanelBase.DoubleBufferPanel();
            this.RAID0Panel = new PanelBase.DoubleBufferPanel();
            this.statusPanel = new PanelBase.DoubleBufferPanel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.keepSpacePanel.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusLabel.Location = new System.Drawing.Point(12, 133);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(456, 15);
            this.statusLabel.TabIndex = 20;
            // 
            // RAIDlabel
            // 
            this.RAIDlabel.BackColor = System.Drawing.Color.Transparent;
            this.RAIDlabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RAIDlabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RAIDlabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.RAIDlabel.Location = new System.Drawing.Point(12, 313);
            this.RAIDlabel.Name = "RAIDlabel";
            this.RAIDlabel.Size = new System.Drawing.Size(456, 29);
            this.RAIDlabel.TabIndex = 22;
            // 
            // labelKeepSpace
            // 
            this.labelKeepSpace.BackColor = System.Drawing.Color.Transparent;
            this.labelKeepSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelKeepSpace.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeepSpace.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelKeepSpace.Location = new System.Drawing.Point(12, 382);
            this.labelKeepSpace.Name = "labelKeepSpace";
            this.labelKeepSpace.Size = new System.Drawing.Size(456, 125);
            this.labelKeepSpace.TabIndex = 24;
            this.labelKeepSpace.Text = "Set keep space between %1GB and  %2GB under max capacity for best performance\r\n\r\n" +
    "Suggested minimum Keep Space amount:\r\n1.\r\n2.\r\n3.\r\n4.\r\n5.";
            this.labelKeepSpace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DisksLayoutPanel
            // 
            this.DisksLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisksLayoutPanel.Location = new System.Drawing.Point(12, 18);
            this.DisksLayoutPanel.MinimumSize = new System.Drawing.Size(100, 60);
            this.DisksLayoutPanel.Name = "DisksLayoutPanel";
            this.DisksLayoutPanel.Size = new System.Drawing.Size(456, 60);
            this.DisksLayoutPanel.TabIndex = 25;
            // 
            // labelDisks
            // 
            this.labelDisks.BackColor = System.Drawing.Color.Transparent;
            this.labelDisks.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDisks.Location = new System.Drawing.Point(12, 78);
            this.labelDisks.Name = "labelDisks";
            this.labelDisks.Size = new System.Drawing.Size(456, 15);
            this.labelDisks.TabIndex = 26;
            // 
            // keepSpacePanel
            // 
            this.keepSpacePanel.BackColor = System.Drawing.Color.Transparent;
            this.keepSpacePanel.Controls.Add(this.GBlabel);
            this.keepSpacePanel.Controls.Add(this.textBoxKeepSpace);
            this.keepSpacePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.keepSpacePanel.Location = new System.Drawing.Point(12, 342);
            this.keepSpacePanel.Name = "keepSpacePanel";
            this.keepSpacePanel.Size = new System.Drawing.Size(456, 40);
            this.keepSpacePanel.TabIndex = 23;
            this.keepSpacePanel.Tag = "Keep";
            // 
            // GBlabel
            // 
            this.GBlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GBlabel.AutoSize = true;
            this.GBlabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBlabel.Location = new System.Drawing.Point(411, 14);
            this.GBlabel.Name = "GBlabel";
            this.GBlabel.Size = new System.Drawing.Size(24, 15);
            this.GBlabel.TabIndex = 1;
            this.GBlabel.Text = "GB";
            // 
            // textBoxKeepSpace
            // 
            this.textBoxKeepSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeepSpace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.textBoxKeepSpace.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxKeepSpace.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxKeepSpace.Location = new System.Drawing.Point(350, 11);
            this.textBoxKeepSpace.MaxLength = 4;
            this.textBoxKeepSpace.Name = "textBoxKeepSpace";
            this.textBoxKeepSpace.Size = new System.Drawing.Size(55, 21);
            this.textBoxKeepSpace.TabIndex = 0;
            this.textBoxKeepSpace.Text = "0";
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.RAID10Panel);
            this.containerPanel.Controls.Add(this.RAID5Panel);
            this.containerPanel.Controls.Add(this.RAID1Panel);
            this.containerPanel.Controls.Add(this.RAID0Panel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 148);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 165);
            this.containerPanel.TabIndex = 21;
            // 
            // RAID10Panel
            // 
            this.RAID10Panel.BackColor = System.Drawing.Color.Transparent;
            this.RAID10Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RAID10Panel.Location = new System.Drawing.Point(0, 120);
            this.RAID10Panel.Name = "RAID10Panel";
            this.RAID10Panel.Size = new System.Drawing.Size(456, 40);
            this.RAID10Panel.TabIndex = 20;
            this.RAID10Panel.Tag = "RAID 10";
            // 
            // RAID5Panel
            // 
            this.RAID5Panel.BackColor = System.Drawing.Color.Transparent;
            this.RAID5Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RAID5Panel.Location = new System.Drawing.Point(0, 80);
            this.RAID5Panel.Name = "RAID5Panel";
            this.RAID5Panel.Size = new System.Drawing.Size(456, 40);
            this.RAID5Panel.TabIndex = 21;
            this.RAID5Panel.Tag = "RAID 5";
            // 
            // RAID1Panel
            // 
            this.RAID1Panel.BackColor = System.Drawing.Color.Transparent;
            this.RAID1Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RAID1Panel.Location = new System.Drawing.Point(0, 40);
            this.RAID1Panel.Name = "RAID1Panel";
            this.RAID1Panel.Size = new System.Drawing.Size(456, 40);
            this.RAID1Panel.TabIndex = 19;
            this.RAID1Panel.Tag = "RAID 1";
            // 
            // RAID0Panel
            // 
            this.RAID0Panel.BackColor = System.Drawing.Color.Transparent;
            this.RAID0Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RAID0Panel.Location = new System.Drawing.Point(0, 0);
            this.RAID0Panel.Name = "RAID0Panel";
            this.RAID0Panel.Size = new System.Drawing.Size(456, 40);
            this.RAID0Panel.TabIndex = 18;
            this.RAID0Panel.Tag = "RAID 0";
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.Transparent;
            this.statusPanel.Controls.Add(this.labelProgress);
            this.statusPanel.Controls.Add(this.progressBar);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusPanel.Location = new System.Drawing.Point(12, 93);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(456, 40);
            this.statusPanel.TabIndex = 19;
            this.statusPanel.Tag = "Status";
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.AutoSize = true;
            this.labelProgress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgress.Location = new System.Drawing.Point(335, 13);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(18, 15);
            this.labelProgress.TabIndex = 1;
            this.labelProgress.Text = "%";
            this.labelProgress.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.ForeColor = System.Drawing.SystemColors.Desktop;
            this.progressBar.Location = new System.Drawing.Point(142, 13);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(190, 14);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // RAID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.labelKeepSpace);
            this.Controls.Add(this.keepSpacePanel);
            this.Controls.Add(this.RAIDlabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.labelDisks);
            this.Controls.Add(this.DisksLayoutPanel);
            this.Name = "RAID";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 581);
            this.keepSpacePanel.ResumeLayout(false);
            this.keepSpacePanel.PerformLayout();
            this.containerPanel.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel RAID5Panel;
        private PanelBase.DoubleBufferPanel RAID10Panel;
        private PanelBase.DoubleBufferPanel RAID1Panel;
        private PanelBase.DoubleBufferPanel RAID0Panel;
        private System.Windows.Forms.Label statusLabel;
        private PanelBase.DoubleBufferPanel statusPanel;
        private System.Windows.Forms.Label RAIDlabel;
        private PanelBase.DoubleBufferPanel keepSpacePanel;
        private System.Windows.Forms.Label GBlabel;
        private System.Windows.Forms.Label labelKeepSpace;
        private System.Windows.Forms.FlowLayoutPanel DisksLayoutPanel;
        private System.Windows.Forms.Label labelDisks;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgress;
        private PanelBase.HotKeyTextBox textBoxKeepSpace;
    }
}
