namespace SetupBase
{
    partial class UndoUI2
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
            // undoButton
            // 
            this.undoButton.BackgroundImage = global::SetupBase.Properties.Resources.cancelButotn;
            this.undoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.undoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.undoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoButton.FlatAppearance.BorderSize = 0;
            this.undoButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.undoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.undoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.undoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.undoButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undoButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.undoButton.Location = new System.Drawing.Point(25, 7);
            this.undoButton.Margin = new System.Windows.Forms.Padding(0);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(150, 41);
            this.undoButton.TabIndex = 0;
            this.undoButton.Text = "Undo";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UndoButtonMouseClick);
            // 
            // UndoUI2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.undoButton);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Name = "UndoUI2";
            this.Padding = new System.Windows.Forms.Padding(25, 7, 25, 7);
            this.Size = new System.Drawing.Size(200, 55);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button undoButton;
    }
}
