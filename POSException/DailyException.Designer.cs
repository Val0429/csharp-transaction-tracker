namespace POSException
{
    sealed partial class DailyException
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
            this.exceptionPanel = new PanelBase.DoubleBufferPanel();
            this.posPanel = new PanelBase.DoubleBufferPanel();
            this.posComboBox = new System.Windows.Forms.ComboBox();
            this.containerPanel.SuspendLayout();
            this.posPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(200, 42);
            this.titlePanel.TabIndex = 3;
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.containerPanel.Controls.Add(this.exceptionPanel);
            this.containerPanel.Controls.Add(this.posPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 42);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(200, 263);
            this.containerPanel.TabIndex = 4;
            // 
            // exceptionPanel
            // 
            this.exceptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exceptionPanel.Location = new System.Drawing.Point(0, 30);
            this.exceptionPanel.Name = "exceptionPanel";
            this.exceptionPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.exceptionPanel.Size = new System.Drawing.Size(200, 233);
            this.exceptionPanel.TabIndex = 1;
            // 
            // posPanel
            // 
            this.posPanel.BackColor = System.Drawing.Color.Transparent;
            this.posPanel.Controls.Add(this.posComboBox);
            this.posPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posPanel.Location = new System.Drawing.Point(0, 0);
            this.posPanel.Name = "posPanel";
            this.posPanel.Size = new System.Drawing.Size(200, 30);
            this.posPanel.TabIndex = 0;
            // 
            // posComboBox
            // 
            this.posComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.posComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.posComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posComboBox.FormattingEnabled = true;
            this.posComboBox.Location = new System.Drawing.Point(8, 3);
            this.posComboBox.Name = "posComboBox";
            this.posComboBox.Size = new System.Drawing.Size(186, 23);
            this.posComboBox.TabIndex = 0;
            // 
            // DailyException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "DailyException";
            this.Size = new System.Drawing.Size(200, 305);
            this.containerPanel.ResumeLayout(false);
            this.posPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel posPanel;
        private System.Windows.Forms.ComboBox posComboBox;
        private PanelBase.DoubleBufferPanel exceptionPanel;
    }
}
