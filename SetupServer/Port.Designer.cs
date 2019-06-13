namespace SetupServer
{
    partial class PortControl
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
            this.customPanel = new PanelBase.DoubleBufferPanel();
            this.portTextBox = new PanelBase.HotKeyTextBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.customPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 58);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 40);
            this.containerPanel.TabIndex = 1;
            // 
            // customPanel
            // 
            this.customPanel.BackColor = System.Drawing.Color.Transparent;
            this.customPanel.Controls.Add(this.portTextBox);
            this.customPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel.Location = new System.Drawing.Point(12, 18);
            this.customPanel.Name = "customPanel";
            this.customPanel.Size = new System.Drawing.Size(456, 40);
            this.customPanel.TabIndex = 7;
            this.customPanel.Tag = "Custom";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.portTextBox.Location = new System.Drawing.Point(385, 9);
            this.portTextBox.MaxLength = 5;
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.ShortcutsEnabled = false;
            this.portTextBox.Size = new System.Drawing.Size(51, 21);
            this.portTextBox.TabIndex = 0;
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.Transparent;
            this.warningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.warningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(12, 98);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(456, 20);
            this.warningLabel.TabIndex = 19;
            this.warningLabel.Text = "Do not set \"Server port\" and \"Database port\" to be equal";
            this.warningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.warningLabel.Visible = false;
            // 
            // PortControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.customPanel);
            this.DoubleBuffered = true;
            this.Name = "PortControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.customPanel.ResumeLayout(false);
            this.customPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel customPanel;
        private PanelBase.HotKeyTextBox portTextBox;
        private System.Windows.Forms.Label warningLabel;


    }
}
