namespace Interrogation
{
    partial class CriteriaPanel
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
			this.nameLabel = new System.Windows.Forms.Label();
			this.nameDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.noofrecordLabel = new System.Windows.Forms.Label();
			this.noofrecordDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
			this.noofRecordTextBox = new System.Windows.Forms.TextBox();
			this.nameDoubleBufferPanel.SuspendLayout();
			this.noofrecordDoubleBufferPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// deviceDoubleBufferPanel
			// 
			this.deviceDoubleBufferPanel.Size = new System.Drawing.Size(1415, 40);
			// 
			// posLabel
			// 
			this.posLabel.Size = new System.Drawing.Size(1415, 15);
			// 
			// dateTimeDoubleBufferPanel
			// 
			this.dateTimeDoubleBufferPanel.Size = new System.Drawing.Size(1415, 40);
			// 
			// dateTimeLabel
			// 
			this.dateTimeLabel.Size = new System.Drawing.Size(1415, 15);
			// 
			// eventDoubleBufferPanel
			// 
			this.eventDoubleBufferPanel.Size = new System.Drawing.Size(1415, 40);
			this.eventDoubleBufferPanel.Visible = false;
			// 
			// exceptionLabel
			// 
			this.exceptionLabel.Size = new System.Drawing.Size(1415, 15);
			this.exceptionLabel.Visible = false;
			// 
			// nameLabel
			// 
			this.nameLabel.BackColor = System.Drawing.Color.Transparent;
			this.nameLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.nameLabel.Location = new System.Drawing.Point(12, 223);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(1415, 15);
			this.nameLabel.TabIndex = 15;
			// 
			// nameDoubleBufferPanel
			// 
			this.nameDoubleBufferPanel.AutoScroll = true;
			this.nameDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
			this.nameDoubleBufferPanel.Controls.Add(this.nameTextBox);
			this.nameDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.nameDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.nameDoubleBufferPanel.Location = new System.Drawing.Point(12, 183);
			this.nameDoubleBufferPanel.Name = "nameDoubleBufferPanel";
			this.nameDoubleBufferPanel.Size = new System.Drawing.Size(1415, 40);
			this.nameDoubleBufferPanel.TabIndex = 14;
			this.nameDoubleBufferPanel.Tag = "Name";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nameTextBox.Location = new System.Drawing.Point(1031, 8);
			this.nameTextBox.MaxLength = 50;
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(360, 21);
			this.nameTextBox.TabIndex = 1;
			// 
			// noofrecordLabel
			// 
			this.noofrecordLabel.BackColor = System.Drawing.Color.Transparent;
			this.noofrecordLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.noofrecordLabel.Location = new System.Drawing.Point(12, 278);
			this.noofrecordLabel.Name = "noofrecordLabel";
			this.noofrecordLabel.Size = new System.Drawing.Size(1415, 15);
			this.noofrecordLabel.TabIndex = 17;
			// 
			// noofrecordDoubleBufferPanel
			// 
			this.noofrecordDoubleBufferPanel.AutoScroll = true;
			this.noofrecordDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
			this.noofrecordDoubleBufferPanel.Controls.Add(this.noofRecordTextBox);
			this.noofrecordDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.noofrecordDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.noofrecordDoubleBufferPanel.Location = new System.Drawing.Point(12, 238);
			this.noofrecordDoubleBufferPanel.Name = "noofrecordDoubleBufferPanel";
			this.noofrecordDoubleBufferPanel.Size = new System.Drawing.Size(1415, 40);
			this.noofrecordDoubleBufferPanel.TabIndex = 16;
			this.noofrecordDoubleBufferPanel.Tag = "NoOfRecord";
			// 
			// noofRecordTextBox
			// 
			this.noofRecordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.noofRecordTextBox.Location = new System.Drawing.Point(1337, 8);
			this.noofRecordTextBox.MaxLength = 6;
			this.noofRecordTextBox.Name = "noofRecordTextBox";
			this.noofRecordTextBox.Size = new System.Drawing.Size(54, 21);
			this.noofRecordTextBox.TabIndex = 3;
			// 
			// CriteriaPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.Controls.Add(this.noofrecordLabel);
			this.Controls.Add(this.noofrecordDoubleBufferPanel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.nameDoubleBufferPanel);
			this.Font = new System.Drawing.Font("Arial", 9F);
			this.Name = "CriteriaPanel";
			this.Size = new System.Drawing.Size(1439, 855);
			this.Controls.SetChildIndex(this.deviceDoubleBufferPanel, 0);
			this.Controls.SetChildIndex(this.posLabel, 0);
			this.Controls.SetChildIndex(this.dateTimeDoubleBufferPanel, 0);
			this.Controls.SetChildIndex(this.dateTimeLabel, 0);
			this.Controls.SetChildIndex(this.eventDoubleBufferPanel, 0);
			this.Controls.SetChildIndex(this.exceptionLabel, 0);
			this.Controls.SetChildIndex(this.nameDoubleBufferPanel, 0);
			this.Controls.SetChildIndex(this.nameLabel, 0);
			this.Controls.SetChildIndex(this.noofrecordDoubleBufferPanel, 0);
			this.Controls.SetChildIndex(this.noofrecordLabel, 0);
			this.nameDoubleBufferPanel.ResumeLayout(false);
			this.nameDoubleBufferPanel.PerformLayout();
			this.noofrecordDoubleBufferPanel.ResumeLayout(false);
			this.noofrecordDoubleBufferPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label nameLabel;
        public PanelBase.DoubleBufferPanel nameDoubleBufferPanel;
        public System.Windows.Forms.Label noofrecordLabel;
        public PanelBase.DoubleBufferPanel noofrecordDoubleBufferPanel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox noofRecordTextBox;
    }
}
