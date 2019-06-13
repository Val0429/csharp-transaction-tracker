namespace Investigation
{
    partial class SaveResult
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
            this.saveResultButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveResultButton
            // 
            this.saveResultButton.BackgroundImage = global::Investigation.Properties.Resources.button;
            this.saveResultButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.saveResultButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveResultButton.FlatAppearance.BorderSize = 0;
            this.saveResultButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.saveResultButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.saveResultButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.saveResultButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveResultButton.Image = global::Investigation.Properties.Resources.saveReport;
            this.saveResultButton.Location = new System.Drawing.Point(7, 5);
            this.saveResultButton.Name = "saveResultButton";
            this.saveResultButton.Size = new System.Drawing.Size(56, 54);
            this.saveResultButton.TabIndex = 0;
            this.saveResultButton.UseVisualStyleBackColor = true;
            this.saveResultButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SaveResultButtonMouseClick);
            // 
            // SaveResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.saveResultButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SaveResult";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveResultButton;
    }
}
