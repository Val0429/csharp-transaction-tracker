namespace POSException
{
    sealed partial class TransactionDetail
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.transactionListBox = new System.Windows.Forms.ListBox();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(580, 37);
            this.titlePanel.TabIndex = 3;
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(68)))));
            this.containerPanel.Controls.Add(this.transactionListBox);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 37);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(580, 113);
            this.containerPanel.TabIndex = 4;
            // 
            // transactionListBox
            // 
            this.transactionListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(103)))), ((int)(((byte)(117)))));
            this.transactionListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.transactionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transactionListBox.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transactionListBox.ForeColor = System.Drawing.Color.White;
            this.transactionListBox.FormattingEnabled = true;
            this.transactionListBox.Location = new System.Drawing.Point(0, 0);
            this.transactionListBox.Margin = new System.Windows.Forms.Padding(0);
            this.transactionListBox.Name = "transactionListBox";
            this.transactionListBox.Size = new System.Drawing.Size(580, 113);
            this.transactionListBox.TabIndex = 2;
            // 
            // TransactionDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.titlePanel);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.Name = "TransactionDetail";
            this.Size = new System.Drawing.Size(580, 150);
            this.containerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.ListBox transactionListBox;
    }
}
