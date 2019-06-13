namespace SetupServer
{
    sealed partial class StoreControl
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
            this.storeIdPanel = new PanelBase.DoubleBufferPanel();
            this.storeIdTextBox = new PanelBase.HotKeyTextBox();
            this.storeNamePanel = new PanelBase.DoubleBufferPanel();
            this.storeNameTextBox = new PanelBase.HotKeyTextBox();
            this.addressPanel = new PanelBase.DoubleBufferPanel();
            this.addressTextBox = new PanelBase.HotKeyTextBox();
            this.phonePanel = new PanelBase.DoubleBufferPanel();
            this.phoneTextBox = new PanelBase.HotKeyTextBox();
            this.storeIdPanel.SuspendLayout();
            this.storeNamePanel.SuspendLayout();
            this.addressPanel.SuspendLayout();
            this.phonePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // storeIdPanel
            // 
            this.storeIdPanel.BackColor = System.Drawing.Color.Transparent;
            this.storeIdPanel.Controls.Add(this.storeIdTextBox);
            this.storeIdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.storeIdPanel.Location = new System.Drawing.Point(12, 18);
            this.storeIdPanel.Name = "storeIdPanel";
            this.storeIdPanel.Size = new System.Drawing.Size(456, 40);
            this.storeIdPanel.TabIndex = 7;
            this.storeIdPanel.Tag = "StoreId";
            // 
            // storeIdTextBox
            // 
            this.storeIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.storeIdTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeIdTextBox.Location = new System.Drawing.Point(260, 10);
            this.storeIdTextBox.MaxLength = 80;
            this.storeIdTextBox.Name = "storeIdTextBox";
            this.storeIdTextBox.Size = new System.Drawing.Size(181, 21);
            this.storeIdTextBox.TabIndex = 3;
            // 
            // storeNamePanel
            // 
            this.storeNamePanel.BackColor = System.Drawing.Color.Transparent;
            this.storeNamePanel.Controls.Add(this.storeNameTextBox);
            this.storeNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.storeNamePanel.Location = new System.Drawing.Point(12, 58);
            this.storeNamePanel.Name = "storeNamePanel";
            this.storeNamePanel.Size = new System.Drawing.Size(456, 40);
            this.storeNamePanel.TabIndex = 10;
            this.storeNamePanel.Tag = "StoreName";
            // 
            // storeNameTextBox
            // 
            this.storeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.storeNameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeNameTextBox.Location = new System.Drawing.Point(260, 10);
            this.storeNameTextBox.MaxLength = 80;
            this.storeNameTextBox.Name = "storeNameTextBox";
            this.storeNameTextBox.Size = new System.Drawing.Size(181, 21);
            this.storeNameTextBox.TabIndex = 3;
            // 
            // addressPanel
            // 
            this.addressPanel.BackColor = System.Drawing.Color.Transparent;
            this.addressPanel.Controls.Add(this.addressTextBox);
            this.addressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressPanel.Location = new System.Drawing.Point(12, 98);
            this.addressPanel.Name = "addressPanel";
            this.addressPanel.Size = new System.Drawing.Size(456, 40);
            this.addressPanel.TabIndex = 12;
            this.addressPanel.Tag = "StoreAddress";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addressTextBox.Location = new System.Drawing.Point(260, 10);
            this.addressTextBox.MaxLength = 80;
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(181, 21);
            this.addressTextBox.TabIndex = 3;
            // 
            // phonePanel
            // 
            this.phonePanel.BackColor = System.Drawing.Color.Transparent;
            this.phonePanel.Controls.Add(this.phoneTextBox);
            this.phonePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.phonePanel.Location = new System.Drawing.Point(12, 138);
            this.phonePanel.Name = "phonePanel";
            this.phonePanel.Size = new System.Drawing.Size(456, 40);
            this.phonePanel.TabIndex = 14;
            this.phonePanel.Tag = "StorePhone";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoneTextBox.Location = new System.Drawing.Point(260, 10);
            this.phoneTextBox.MaxLength = 80;
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(181, 21);
            this.phoneTextBox.TabIndex = 3;
            // 
            // StoreControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.phonePanel);
            this.Controls.Add(this.addressPanel);
            this.Controls.Add(this.storeNamePanel);
            this.Controls.Add(this.storeIdPanel);
            this.DoubleBuffered = true;
            this.Name = "StoreControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 417);
            this.storeIdPanel.ResumeLayout(false);
            this.storeIdPanel.PerformLayout();
            this.storeNamePanel.ResumeLayout(false);
            this.storeNamePanel.PerformLayout();
            this.addressPanel.ResumeLayout(false);
            this.addressPanel.PerformLayout();
            this.phonePanel.ResumeLayout(false);
            this.phonePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel storeIdPanel;
        private System.Windows.Forms.TextBox storeIdTextBox;
        private PanelBase.DoubleBufferPanel storeNamePanel;
        private System.Windows.Forms.TextBox storeNameTextBox;
        private PanelBase.DoubleBufferPanel addressPanel;
        private System.Windows.Forms.TextBox addressTextBox;
        private PanelBase.DoubleBufferPanel phonePanel;
        private System.Windows.Forms.TextBox phoneTextBox;


    }
}
