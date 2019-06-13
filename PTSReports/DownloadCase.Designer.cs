namespace PTSReports
{
    partial class DownloadCase
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
            this.downloadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // downloadButton
            // 
            this.downloadButton.BackgroundImage = global::PTSReports.Properties.Resources.button;
            this.downloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.downloadButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadButton.FlatAppearance.BorderSize = 0;
            this.downloadButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.downloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadButton.Image = global::PTSReports.Properties.Resources.download;
            this.downloadButton.Location = new System.Drawing.Point(7, 5);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(56, 54);
            this.downloadButton.TabIndex = 0;
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DownloadButtonMouseClick);
            // 
            // DownloadCase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.downloadButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DownloadCase";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button downloadButton;
    }
}
