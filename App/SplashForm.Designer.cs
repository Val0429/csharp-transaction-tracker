using System.Drawing;

namespace App
{
    sealed partial class SplashForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splashLeftPanel = new System.Windows.Forms.Panel();
            this.splashRightPanel = new System.Windows.Forms.Panel();
            this.splashCenterPanel = new System.Windows.Forms.Panel();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.splashCenterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splashLeftPanel
            // 
            this.splashLeftPanel.BackgroundImage = global::App.Properties.Resources.splashLeft;
            this.splashLeftPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splashLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.splashLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.splashLeftPanel.Name = "splashLeftPanel";
            this.splashLeftPanel.Size = new System.Drawing.Size(25, 328);
            this.splashLeftPanel.TabIndex = 5;
            // 
            // splashRightPanel
            // 
            this.splashRightPanel.BackgroundImage = global::App.Properties.Resources.splashRight;
            this.splashRightPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splashRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.splashRightPanel.Location = new System.Drawing.Point(275, 0);
            this.splashRightPanel.Name = "splashRightPanel";
            this.splashRightPanel.Size = new System.Drawing.Size(25, 328);
            this.splashRightPanel.TabIndex = 6;
            // 
            // splashCenterPanel
            // 
            this.splashCenterPanel.BackgroundImage = global::App.Properties.Resources.splashCenter;
            this.splashCenterPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splashCenterPanel.Controls.Add(this.logoPanel);
            this.splashCenterPanel.Controls.Add(this.infoLabel);
            this.splashCenterPanel.Controls.Add(this.versionLabel);
            this.splashCenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splashCenterPanel.Location = new System.Drawing.Point(25, 0);
            this.splashCenterPanel.Name = "splashCenterPanel";
            this.splashCenterPanel.Padding = new System.Windows.Forms.Padding(0, 25, 0, 25);
            this.splashCenterPanel.Size = new System.Drawing.Size(250, 328);
            this.splashCenterPanel.TabIndex = 7;
            // 
            // logoPanel
            // 
            this.logoPanel.BackColor = System.Drawing.Color.Transparent;
            this.logoPanel.BackgroundImage = global::App.Properties.Resources.loadingClock;
            this.logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPanel.Location = new System.Drawing.Point(0, 25);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(250, 228);
            this.logoPanel.TabIndex = 6;
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.infoLabel.Location = new System.Drawing.Point(0, 253);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(250, 25);
            this.infoLabel.TabIndex = 7;
            this.infoLabel.Text = "Loading XXX";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.versionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.versionLabel.Location = new System.Drawing.Point(0, 278);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(250, 25);
            this.versionLabel.TabIndex = 8;
            this.versionLabel.Text = "Ver. %1";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(300, 328);
            this.ControlBox = false;
            this.Controls.Add(this.splashCenterPanel);
            this.Controls.Add(this.splashRightPanel);
            this.Controls.Add(this.splashLeftPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(300, 328);
            this.Name = "SplashForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.splashCenterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel splashLeftPanel;
        private System.Windows.Forms.Panel splashRightPanel;
        private System.Windows.Forms.Panel splashCenterPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Panel logoPanel;


    }
}