namespace VideoMonitor
{
    partial class ManualRecord
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
            this.manualRecordButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // manualRecordButton
            // 
            this.manualRecordButton.BackgroundImage = global::VideoMonitor.Properties.Resources.button;
            this.manualRecordButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.manualRecordButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manualRecordButton.FlatAppearance.BorderSize = 0;
            this.manualRecordButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.manualRecordButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.manualRecordButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.manualRecordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualRecordButton.Image = global::VideoMonitor.Properties.Resources.recordButton;
            this.manualRecordButton.Location = new System.Drawing.Point(7, 5);
            this.manualRecordButton.Name = "manualRecordButton";
            this.manualRecordButton.Size = new System.Drawing.Size(56, 54);
            this.manualRecordButton.TabIndex = 0;
            this.manualRecordButton.UseVisualStyleBackColor = true;
            this.manualRecordButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ManualRecordButtonMouseClick);
            // 
            // ManualRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.manualRecordButton);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ManualRecord";
            this.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Size = new System.Drawing.Size(70, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button manualRecordButton;
    }
}
