namespace SetupBase
{
    partial class Undo
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
            this.undoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.undoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.undoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoButton.FlatAppearance.BorderSize = 0;
            this.undoButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.undoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.undoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.undoButton.Image = global::SetupBase.Properties.Resources.save;
            this.undoButton.Location = new System.Drawing.Point(8, 6);
            this.undoButton.Name = "saveButton";
            this.undoButton.Size = new System.Drawing.Size(44, 44);
            this.undoButton.TabIndex = 0;
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UndoButtonMouseClick);
            // 
            // Save
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.undoButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Name = "Save";
            this.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Size = new System.Drawing.Size(60, 56);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button undoButton;
    }
}
