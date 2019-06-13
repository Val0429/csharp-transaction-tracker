namespace PTSReports.Base
{
    sealed partial class StorePanel
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.pageSelectorPanel = new System.Windows.Forms.Panel();
            this.addedItemLabel = new System.Windows.Forms.Label();
            this.pageSelectorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 43);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 339);
            this.containerPanel.TabIndex = 1;
            // 
            // pageSelectorPanel
            // 
            this.pageSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.pageSelectorPanel.Controls.Add(this.addedItemLabel);
            this.pageSelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageSelectorPanel.Location = new System.Drawing.Point(12, 18);
            this.pageSelectorPanel.Name = "pageSelectorPanel";
            this.pageSelectorPanel.Size = new System.Drawing.Size(456, 25);
            this.pageSelectorPanel.TabIndex = 23;
            // 
            // addedItemLabel
            // 
            this.addedItemLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedItemLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.addedItemLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedItemLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedItemLabel.Location = new System.Drawing.Point(0, 0);
            this.addedItemLabel.Name = "addedItemLabel";
            this.addedItemLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedItemLabel.Size = new System.Drawing.Size(107, 25);
            this.addedItemLabel.TabIndex = 20;
            this.addedItemLabel.Text = "Store";
            this.addedItemLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // StorePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pageSelectorPanel);
            this.DoubleBuffered = true;
            this.Name = "StorePanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.pageSelectorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        public System.Windows.Forms.Panel pageSelectorPanel;
        private System.Windows.Forms.Label addedItemLabel;
    }
}
