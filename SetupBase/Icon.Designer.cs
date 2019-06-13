namespace SetupBase
{
    partial class Icon
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
            this.Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button
            // 
            this.Button.BackColor = System.Drawing.Color.Transparent;
            this.Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Button.FlatAppearance.BorderSize = 0;
            this.Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button.Image = global::SetupBase.Properties.Resources.icon;
            this.Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button.Location = new System.Drawing.Point(0, 0);
            this.Button.Margin = new System.Windows.Forms.Padding(0);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(200, 40);
            this.Button.TabIndex = 0;
            this.Button.TabStop = false;
            this.Button.Text = "Icon";
            this.Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button.UseVisualStyleBackColor = false;
            this.Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonMouseClick);
            // 
            // Icon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::SetupBase.Properties.Resources.tagBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.Button);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Icon";
            this.Size = new System.Drawing.Size(200, 40);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button Button;

    }
}
