namespace PTSReports.AdvancedSearch
{
    sealed partial class CountingPanel
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
            this.conditionalLabel = new System.Windows.Forms.Label();
            this.pieceTextBox = new System.Windows.Forms.TextBox();
            this.enablePanel = new PanelBase.DoubleBufferPanel();
            this.pieceLabel = new System.Windows.Forms.Label();
            this.enableCheckBox = new System.Windows.Forms.CheckBox();
            this.enablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // conditionalLabel
            // 
            this.conditionalLabel.BackColor = System.Drawing.Color.Transparent;
            this.conditionalLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionalLabel.Location = new System.Drawing.Point(12, 58);
            this.conditionalLabel.Name = "conditionalLabel";
            this.conditionalLabel.Size = new System.Drawing.Size(456, 15);
            this.conditionalLabel.TabIndex = 15;
            // 
            // pieceTextBox
            // 
            this.pieceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pieceTextBox.Font = new System.Drawing.Font("Arial", 9F);
            this.pieceTextBox.Location = new System.Drawing.Point(317, 11);
            this.pieceTextBox.Name = "pieceTextBox";
            this.pieceTextBox.Size = new System.Drawing.Size(72, 21);
            this.pieceTextBox.TabIndex = 0;
            // 
            // enablePanel
            // 
            this.enablePanel.AutoScroll = true;
            this.enablePanel.BackColor = System.Drawing.Color.Transparent;
            this.enablePanel.Controls.Add(this.pieceLabel);
            this.enablePanel.Controls.Add(this.pieceTextBox);
            this.enablePanel.Controls.Add(this.enableCheckBox);
            this.enablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.enablePanel.Location = new System.Drawing.Point(12, 18);
            this.enablePanel.Name = "enablePanel";
            this.enablePanel.Size = new System.Drawing.Size(456, 40);
            this.enablePanel.TabIndex = 16;
            // 
            // pieceLabel
            // 
            this.pieceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pieceLabel.AutoSize = true;
            this.pieceLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.pieceLabel.Location = new System.Drawing.Point(395, 13);
            this.pieceLabel.Name = "pieceLabel";
            this.pieceLabel.Size = new System.Drawing.Size(38, 15);
            this.pieceLabel.TabIndex = 1;
            this.pieceLabel.Text = "Piece";
            // 
            // enableCheckBox
            // 
            this.enableCheckBox.AutoSize = true;
            this.enableCheckBox.Font = new System.Drawing.Font("Arial", 9F);
            this.enableCheckBox.Location = new System.Drawing.Point(11, 13);
            this.enableCheckBox.Name = "enableCheckBox";
            this.enableCheckBox.Size = new System.Drawing.Size(15, 14);
            this.enableCheckBox.TabIndex = 0;
            this.enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // CountingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.conditionalLabel);
            this.Controls.Add(this.enablePanel);
            this.DoubleBuffered = true;
            this.Name = "CountingPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.enablePanel.ResumeLayout(false);
            this.enablePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label conditionalLabel;
        private PanelBase.DoubleBufferPanel enablePanel;
        private System.Windows.Forms.CheckBox enableCheckBox;
        private System.Windows.Forms.TextBox pieceTextBox;
        private System.Windows.Forms.Label pieceLabel;


    }
}
