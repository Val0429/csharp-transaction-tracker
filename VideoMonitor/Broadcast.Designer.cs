namespace VideoMonitor
{
    partial class Broadcast
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
            this.broadcastButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // broadcastButton
            // 
            this.broadcastButton.BackgroundImage = global::VideoMonitor.Properties.Resources.button;
            this.broadcastButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.broadcastButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.broadcastButton.FlatAppearance.BorderSize = 0;
            this.broadcastButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.broadcastButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.broadcastButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.broadcastButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.broadcastButton.Image = global::VideoMonitor.Properties.Resources.broadcast;
            this.broadcastButton.Location = new System.Drawing.Point(7, 5);
            this.broadcastButton.Name = "broadcastButton";
            this.broadcastButton.Size = new System.Drawing.Size(56, 54);
            this.broadcastButton.TabIndex = 0;
            this.broadcastButton.UseVisualStyleBackColor = true;
            this.broadcastButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BroadcastButtonMouseClick);
            // 
            // Broadcast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.broadcastButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Broadcast";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button broadcastButton;
    }
}
