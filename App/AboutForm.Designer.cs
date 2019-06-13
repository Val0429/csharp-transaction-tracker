namespace App
{
    partial class AboutForm
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.logo = new System.Windows.Forms.Panel();
            this.devicePackVersionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.versionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.versionLabel.Location = new System.Drawing.Point(0, 244);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(405, 25);
            this.versionLabel.TabIndex = 9;
            this.versionLabel.Text = "Ver. %1";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.BackgroundImage = global::App.Properties.Resources.setupMenuIcon;
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logo.Location = new System.Drawing.Point(42, 27);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(319, 191);
            this.logo.TabIndex = 10;
            // 
            // devicePackVersionLabel
            // 
            this.devicePackVersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.devicePackVersionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.devicePackVersionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.devicePackVersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.devicePackVersionLabel.Location = new System.Drawing.Point(0, 269);
            this.devicePackVersionLabel.Name = "devicePackVersionLabel";
            this.devicePackVersionLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.devicePackVersionLabel.Size = new System.Drawing.Size(405, 35);
            this.devicePackVersionLabel.TabIndex = 11;
            this.devicePackVersionLabel.Text = "Ver. %1";
            this.devicePackVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::App.Properties.Resources.toolBg2;
            this.ClientSize = new System.Drawing.Size(405, 304);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.devicePackVersionLabel);
            this.Controls.Add(this.logo);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel logo;
        private System.Windows.Forms.Label devicePackVersionLabel;

    }
}