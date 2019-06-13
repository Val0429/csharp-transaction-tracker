﻿namespace SetupServer
{
    sealed partial class UpgradeControl
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
            this.browserButton = new System.Windows.Forms.Button();
            this.filePanel = new PanelBase.DoubleBufferPanel();
            this.filePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // browserButton
            // 
            this.browserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browserButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browserButton.Location = new System.Drawing.Point(367, 7);
            this.browserButton.Name = "browserButton";
            this.browserButton.Size = new System.Drawing.Size(75, 27);
            this.browserButton.TabIndex = 0;
            this.browserButton.Text = "Broswer";
            this.browserButton.UseVisualStyleBackColor = true;
            this.browserButton.Click += new System.EventHandler(this.BrowserButtonClick);
            // 
            // filePanel
            // 
            this.filePanel.BackColor = System.Drawing.Color.Transparent;
            this.filePanel.Controls.Add(this.browserButton);
            this.filePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filePanel.Location = new System.Drawing.Point(12, 18);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(456, 40);
            this.filePanel.TabIndex = 8;
            this.filePanel.Tag = "SettingFile";
            // 
            // UpgradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.filePanel);
            this.Name = "UpgradeControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.filePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button browserButton;
        private PanelBase.DoubleBufferPanel filePanel;
    }
}