namespace SetupStore
{
    sealed partial class EditPanel
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
            this.posPanel = new PanelBase.DoubleBufferPanel();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.itemIDPanel = new PanelBase.DoubleBufferPanel();
            this.itemIDTextBox = new PanelBase.HotKeyTextBox();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.pageSelectorPanel = new System.Windows.Forms.Panel();
            this.posPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.itemIDPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // posPanel
            // 
            this.posPanel.AutoSize = true;
            this.posPanel.BackColor = System.Drawing.Color.Transparent;
            this.posPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.posPanel.Controls.Add(this.namePanel);
            this.posPanel.Controls.Add(this.itemIDPanel);
            this.posPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posPanel.Location = new System.Drawing.Point(12, 18);
            this.posPanel.Name = "posPanel";
            this.posPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.posPanel.Size = new System.Drawing.Size(456, 92);
            this.posPanel.TabIndex = 20;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 40);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(456, 40);
            this.namePanel.TabIndex = 20;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(260, 8);
            this.nameTextBox.MaxLength = 18;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TabStop = false;
            // 
            // itemIDPanel
            // 
            this.itemIDPanel.BackColor = System.Drawing.Color.Transparent;
            this.itemIDPanel.Controls.Add(this.itemIDTextBox);
            this.itemIDPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemIDPanel.Location = new System.Drawing.Point(0, 0);
            this.itemIDPanel.Name = "itemIDPanel";
            this.itemIDPanel.Size = new System.Drawing.Size(456, 40);
            this.itemIDPanel.TabIndex = 22;
            this.itemIDPanel.Tag = "itemId";
            // 
            // itemIDTextBox
            // 
            this.itemIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemIDTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.itemIDTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemIDTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.itemIDTextBox.Location = new System.Drawing.Point(260, 8);
            this.itemIDTextBox.MaxLength = 999;
            this.itemIDTextBox.Name = "itemIDTextBox";
            this.itemIDTextBox.ShortcutsEnabled = false;
            this.itemIDTextBox.Size = new System.Drawing.Size(181, 21);
            this.itemIDTextBox.TabIndex = 3;
            this.itemIDTextBox.TabStop = false;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 135);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 338);
            this.containerPanel.TabIndex = 21;
            // 
            // pageSelectorPanel
            // 
            this.pageSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.pageSelectorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageSelectorPanel.Location = new System.Drawing.Point(12, 110);
            this.pageSelectorPanel.Name = "pageSelectorPanel";
            this.pageSelectorPanel.Size = new System.Drawing.Size(456, 25);
            this.pageSelectorPanel.TabIndex = 22;
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pageSelectorPanel);
            this.Controls.Add(this.posPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 491);
            this.posPanel.ResumeLayout(false);
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.itemIDPanel.ResumeLayout(false);
            this.itemIDPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel posPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.DoubleBufferPanel itemIDPanel;
        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.HotKeyTextBox itemIDTextBox;
        private System.Windows.Forms.Panel pageSelectorPanel;
    }
}
