namespace TimeTrack
{
    partial class ExportVideo
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
            this.exportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.BackgroundImage = global::TimeTrack.Properties.Resources.button;
            this.exportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.exportButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Image = global::TimeTrack.Properties.Resources.export_video;
            this.exportButton.Location = new System.Drawing.Point(7, 5);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(56, 54);
            this.exportButton.TabIndex = 0;
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExportButtonMouseClick);
            // 
            // ExportVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.exportButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ExportVideo";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button exportButton;
    }
}
