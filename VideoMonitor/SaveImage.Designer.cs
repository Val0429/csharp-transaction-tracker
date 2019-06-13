namespace VideoMonitor
{
    partial class SaveImage
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
            this.saveImageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveImageButton
            // 
            this.saveImageButton.BackgroundImage = global::VideoMonitor.Properties.Resources.button;
            this.saveImageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.saveImageButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveImageButton.FlatAppearance.BorderSize = 0;
            this.saveImageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.saveImageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.saveImageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.saveImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveImageButton.Image = global::VideoMonitor.Properties.Resources.save_imageButton;
            this.saveImageButton.Location = new System.Drawing.Point(7, 5);
            this.saveImageButton.Name = "saveImageButton";
            this.saveImageButton.Size = new System.Drawing.Size(56, 54);
            this.saveImageButton.TabIndex = 0;
            this.saveImageButton.UseVisualStyleBackColor = true;
            this.saveImageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SaveImageButtonMouseClick);
            // 
            // SaveImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.saveImageButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SaveImage";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveImageButton;
    }
}
