namespace POSException
{
    partial class SaveReportForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.commentTextBox = new PanelBase.HotKeyTextBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.reportPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // commentTextBox
            // 
            this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commentTextBox.ForeColor = System.Drawing.Color.Black;
            this.commentTextBox.Location = new System.Drawing.Point(12, 2);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.ShortcutsEnabled = false;
            this.commentTextBox.Size = new System.Drawing.Size(662, 30);
            this.commentTextBox.TabIndex = 14;
            this.commentTextBox.Text = "Write Comment Here.";
            this.commentTextBox.Enter += new System.EventHandler(this.CommentTextBoxEnter);
            this.commentTextBox.Leave += new System.EventHandler(this.CommentTextBoxLeave);
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.BackColor = System.Drawing.Color.Transparent;
            this.applyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.applyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.applyButton.FlatAppearance.BorderSize = 0;
            this.applyButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.applyButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.applyButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.applyButton.ForeColor = System.Drawing.Color.Black;
            this.applyButton.Location = new System.Drawing.Point(680, 2);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(102, 30);
            this.applyButton.TabIndex = 15;
            this.applyButton.Text = "Apply Comment";
            this.applyButton.UseVisualStyleBackColor = false;
            this.applyButton.Click += new System.EventHandler(this.ApplyButtonClick);
            // 
            // reportPanel
            // 
            this.reportPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportPanel.Location = new System.Drawing.Point(0, 36);
            this.reportPanel.Name = "reportPanel";
            this.reportPanel.Size = new System.Drawing.Size(790, 758);
            this.reportPanel.TabIndex = 16;
            // 
            // SaveReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(794, 795);
            this.Controls.Add(this.reportPanel);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.commentTextBox);
            this.Name = "SaveReportForm";
            this.Text = "SaveAs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Panel reportPanel;
        private PanelBase.HotKeyTextBox commentTextBox;

    }
}