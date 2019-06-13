namespace SetupDevice
{
    sealed partial class LiveCheckControl
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
            this.retryCountPanel = new PanelBase.DoubleBufferPanel();
            this.intervalPanel = new PanelBase.DoubleBufferPanel();
            this.uriPanel = new PanelBase.DoubleBufferPanel();
            this.uriTextBox = new System.Windows.Forms.TextBox();
            this.intervalTextBox = new PanelBase.HotKeyTextBox();
            this.retryCountTextBox = new PanelBase.HotKeyTextBox();
            this.retryCountPanel.SuspendLayout();
            this.intervalPanel.SuspendLayout();
            this.uriPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // retryCountPanel
            // 
            this.retryCountPanel.BackColor = System.Drawing.Color.Transparent;
            this.retryCountPanel.Controls.Add(this.retryCountTextBox);
            this.retryCountPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.retryCountPanel.Location = new System.Drawing.Point(0, 105);
            this.retryCountPanel.Name = "retryCountPanel";
            this.retryCountPanel.Size = new System.Drawing.Size(431, 40);
            this.retryCountPanel.TabIndex = 11;
            this.retryCountPanel.Tag = "RetryCount";
            // 
            // intervalPanel
            // 
            this.intervalPanel.BackColor = System.Drawing.Color.Transparent;
            this.intervalPanel.Controls.Add(this.intervalTextBox);
            this.intervalPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.intervalPanel.Location = new System.Drawing.Point(0, 65);
            this.intervalPanel.Name = "intervalPanel";
            this.intervalPanel.Size = new System.Drawing.Size(431, 40);
            this.intervalPanel.TabIndex = 10;
            this.intervalPanel.Tag = "Interval";
            // 
            // uriPanel
            // 
            this.uriPanel.BackColor = System.Drawing.Color.Transparent;
            this.uriPanel.Controls.Add(this.uriTextBox);
            this.uriPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.uriPanel.Location = new System.Drawing.Point(0, 25);
            this.uriPanel.Name = "uriPanel";
            this.uriPanel.Size = new System.Drawing.Size(431, 40);
            this.uriPanel.TabIndex = 29;
            this.uriPanel.Tag = "URI";
            // 
            // uriTextBox
            // 
            this.uriTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uriTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uriTextBox.Location = new System.Drawing.Point(235, 9);
            this.uriTextBox.Name = "uriTextBox";
            this.uriTextBox.Size = new System.Drawing.Size(181, 21);
            this.uriTextBox.TabIndex = 3;
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.intervalTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.intervalTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.intervalTextBox.Location = new System.Drawing.Point(235, 10);
            this.intervalTextBox.MaxLength = 5;
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.ShortcutsEnabled = false;
            this.intervalTextBox.Size = new System.Drawing.Size(181, 21);
            this.intervalTextBox.TabIndex = 3;
            // 
            // retryCountTextBox
            // 
            this.retryCountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.retryCountTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retryCountTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.retryCountTextBox.Location = new System.Drawing.Point(235, 10);
            this.retryCountTextBox.MaxLength = 5;
            this.retryCountTextBox.Name = "retryCountTextBox";
            this.retryCountTextBox.ShortcutsEnabled = false;
            this.retryCountTextBox.Size = new System.Drawing.Size(181, 21);
            this.retryCountTextBox.TabIndex = 4;
            // 
            // LiveCheckControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.retryCountPanel);
            this.Controls.Add(this.intervalPanel);
            this.Controls.Add(this.uriPanel);
            this.Name = "LiveCheckControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(431, 588);
            this.retryCountPanel.ResumeLayout(false);
            this.retryCountPanel.PerformLayout();
            this.intervalPanel.ResumeLayout(false);
            this.intervalPanel.PerformLayout();
            this.uriPanel.ResumeLayout(false);
            this.uriPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel intervalPanel;
        private PanelBase.DoubleBufferPanel retryCountPanel;
        private PanelBase.DoubleBufferPanel uriPanel;
        private System.Windows.Forms.TextBox uriTextBox;
        private PanelBase.HotKeyTextBox retryCountTextBox;
        private PanelBase.HotKeyTextBox intervalTextBox;
    }
}
