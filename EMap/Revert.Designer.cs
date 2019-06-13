namespace EMap
{
    partial class Revert
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
            this.buttonRevert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRevert
            // 
            this.buttonRevert.BackgroundImage = global::EMap.Properties.Resources.buttonBG;
            this.buttonRevert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonRevert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRevert.FlatAppearance.BorderSize = 0;
            this.buttonRevert.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonRevert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonRevert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonRevert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRevert.Image = global::EMap.Properties.Resources.refresh;
            this.buttonRevert.Location = new System.Drawing.Point(7, 5);
            this.buttonRevert.Name = "buttonRevert";
            this.buttonRevert.Size = new System.Drawing.Size(56, 54);
            this.buttonRevert.TabIndex = 2;
            this.buttonRevert.UseVisualStyleBackColor = true;
            this.buttonRevert.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonRefreshClick);
            // 
            // Revert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.buttonRevert);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Location = new System.Drawing.Point(8, 8);
            this.Name = "Revert";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRevert;

    }
}
