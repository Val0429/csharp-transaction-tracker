namespace TimeTrack
{
    partial class PrintImage
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
            this.printButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // printButton
            // 
            this.printButton.BackgroundImage = global::TimeTrack.Properties.Resources.button;
            this.printButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.printButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printButton.FlatAppearance.BorderSize = 0;
            this.printButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.printButton.Image = global::TimeTrack.Properties.Resources.print_image;
            this.printButton.Location = new System.Drawing.Point(7, 5);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(56, 54);
            this.printButton.TabIndex = 0;
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PrintButtonMouseClick);
            // 
            // PrintImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.printButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PrintImage";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button printButton;
    }
}
