namespace VideoMonitor
{
    partial class Snapshot
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
            this.snapshotButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // snapshotButton
            // 
            this.snapshotButton.BackgroundImage = global::VideoMonitor.Properties.Resources.button;
            this.snapshotButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.snapshotButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotButton.FlatAppearance.BorderSize = 0;
            this.snapshotButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.snapshotButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.snapshotButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.snapshotButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.snapshotButton.Image = global::VideoMonitor.Properties.Resources.snapshotButton;
            this.snapshotButton.Location = new System.Drawing.Point(7, 5);
            this.snapshotButton.Name = "snapshotButton";
            this.snapshotButton.Size = new System.Drawing.Size(56, 54);
            this.snapshotButton.TabIndex = 0;
            this.snapshotButton.UseVisualStyleBackColor = true;
            //this.snapshotButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SnapshotButtonMouseClick);
            // 
            // Snapshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.snapshotButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Snapshot";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button snapshotButton;
    }
}
