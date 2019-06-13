namespace EMap
{
    partial class Setup
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
            this.setupImageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setupImageButton
            // 
            this.setupImageButton.BackgroundImage = global::EMap.Properties.Resources.buttonBG;
            this.setupImageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.setupImageButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupImageButton.FlatAppearance.BorderSize = 0;
            this.setupImageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.setupImageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.setupImageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.setupImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setupImageButton.Location = new System.Drawing.Point(7, 5);
            this.setupImageButton.Name = "setupImageButton";
            this.setupImageButton.Size = new System.Drawing.Size(56, 54);
            this.setupImageButton.TabIndex = 0;
            this.setupImageButton.UseVisualStyleBackColor = true;
            this.setupImageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SaveImageButtonMouseClick);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.setupImageButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button setupImageButton;
    }
}
