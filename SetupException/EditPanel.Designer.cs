namespace SetupException
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.tagContainerPanel = new PanelBase.DoubleBufferPanel();
            this.tagLabel = new System.Windows.Forms.Label();
            this.segmentContainerPanel = new PanelBase.DoubleBufferPanel();
            this.segmentLabel = new System.Windows.Forms.Label();
            this.exceptionContainerPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionLabel = new System.Windows.Forms.Label();
            this.FilterPOSIdPanel = new PanelBase.DoubleBufferPanel();
            this.filterPODIdCheckBox = new System.Windows.Forms.CheckBox();
            this.transactionTypePanel = new PanelBase.DoubleBufferPanel();
            this.transactionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.manufacturePanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.informationLabel = new System.Windows.Forms.Label();
            this.separatorDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.separatorTextBox = new System.Windows.Forms.TextBox();
            this.containerPanel.SuspendLayout();
            this.FilterPOSIdPanel.SuspendLayout();
            this.transactionTypePanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.manufacturePanel.SuspendLayout();
            this.separatorDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.tagContainerPanel);
            this.containerPanel.Controls.Add(this.tagLabel);
            this.containerPanel.Controls.Add(this.segmentContainerPanel);
            this.containerPanel.Controls.Add(this.segmentLabel);
            this.containerPanel.Controls.Add(this.exceptionContainerPanel);
            this.containerPanel.Controls.Add(this.exceptionLabel);
            this.containerPanel.Controls.Add(this.separatorDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.FilterPOSIdPanel);
            this.containerPanel.Controls.Add(this.transactionTypePanel);
            this.containerPanel.Controls.Add(this.namePanel);
            this.containerPanel.Controls.Add(this.manufacturePanel);
            this.containerPanel.Controls.Add(this.informationLabel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 8);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 465);
            this.containerPanel.TabIndex = 20;
            // 
            // tagContainerPanel
            // 
            this.tagContainerPanel.AutoScroll = true;
            this.tagContainerPanel.AutoSize = true;
            this.tagContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.tagContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tagContainerPanel.Location = new System.Drawing.Point(0, 320);
            this.tagContainerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.tagContainerPanel.Name = "tagContainerPanel";
            this.tagContainerPanel.Size = new System.Drawing.Size(456, 30);
            this.tagContainerPanel.TabIndex = 30;
            // 
            // tagLabel
            // 
            this.tagLabel.BackColor = System.Drawing.Color.Transparent;
            this.tagLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tagLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagLabel.ForeColor = System.Drawing.Color.DimGray;
            this.tagLabel.Location = new System.Drawing.Point(0, 295);
            this.tagLabel.Name = "tagLabel";
            this.tagLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tagLabel.Size = new System.Drawing.Size(456, 25);
            this.tagLabel.TabIndex = 29;
            this.tagLabel.Text = "Tag";
            this.tagLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // segmentContainerPanel
            // 
            this.segmentContainerPanel.AutoScroll = true;
            this.segmentContainerPanel.AutoSize = true;
            this.segmentContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.segmentContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.segmentContainerPanel.Location = new System.Drawing.Point(0, 265);
            this.segmentContainerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.segmentContainerPanel.Name = "segmentContainerPanel";
            this.segmentContainerPanel.Size = new System.Drawing.Size(456, 30);
            this.segmentContainerPanel.TabIndex = 28;
            // 
            // segmentLabel
            // 
            this.segmentLabel.BackColor = System.Drawing.Color.Transparent;
            this.segmentLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.segmentLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.segmentLabel.ForeColor = System.Drawing.Color.DimGray;
            this.segmentLabel.Location = new System.Drawing.Point(0, 240);
            this.segmentLabel.Name = "segmentLabel";
            this.segmentLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.segmentLabel.Size = new System.Drawing.Size(456, 25);
            this.segmentLabel.TabIndex = 27;
            this.segmentLabel.Text = "Segment";
            this.segmentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // exceptionContainerPanel
            // 
            this.exceptionContainerPanel.AutoScroll = true;
            this.exceptionContainerPanel.AutoSize = true;
            this.exceptionContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionContainerPanel.Location = new System.Drawing.Point(0, 210);
            this.exceptionContainerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.exceptionContainerPanel.Name = "exceptionContainerPanel";
            this.exceptionContainerPanel.Size = new System.Drawing.Size(456, 30);
            this.exceptionContainerPanel.TabIndex = 26;
            // 
            // exceptionLabel
            // 
            this.exceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.exceptionLabel.Location = new System.Drawing.Point(0, 185);
            this.exceptionLabel.Name = "exceptionLabel";
            this.exceptionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.exceptionLabel.Size = new System.Drawing.Size(456, 25);
            this.exceptionLabel.TabIndex = 25;
            this.exceptionLabel.Text = "Exception";
            this.exceptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FilterPOSIdPanel
            // 
            this.FilterPOSIdPanel.BackColor = System.Drawing.Color.Transparent;
            this.FilterPOSIdPanel.Controls.Add(this.filterPODIdCheckBox);
            this.FilterPOSIdPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FilterPOSIdPanel.Location = new System.Drawing.Point(0, 145);
            this.FilterPOSIdPanel.Name = "FilterPOSIdPanel";
            this.FilterPOSIdPanel.Size = new System.Drawing.Size(456, 40);
            this.FilterPOSIdPanel.TabIndex = 32;
            this.FilterPOSIdPanel.Tag = "FilterPOSId";
            // 
            // filterPODIdCheckBox
            // 
            this.filterPODIdCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterPODIdCheckBox.AutoSize = true;
            this.filterPODIdCheckBox.Location = new System.Drawing.Point(347, 11);
            this.filterPODIdCheckBox.Name = "filterPODIdCheckBox";
            this.filterPODIdCheckBox.Size = new System.Drawing.Size(94, 19);
            this.filterPODIdCheckBox.TabIndex = 0;
            this.filterPODIdCheckBox.Text = "Filter POS Id";
            this.filterPODIdCheckBox.UseVisualStyleBackColor = true;
            // 
            // transactionTypePanel
            // 
            this.transactionTypePanel.BackColor = System.Drawing.Color.Transparent;
            this.transactionTypePanel.Controls.Add(this.transactionTypeComboBox);
            this.transactionTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.transactionTypePanel.Location = new System.Drawing.Point(0, 105);
            this.transactionTypePanel.Name = "transactionTypePanel";
            this.transactionTypePanel.Size = new System.Drawing.Size(456, 40);
            this.transactionTypePanel.TabIndex = 26;
            this.transactionTypePanel.Tag = "TransactionType";
            // 
            // transactionTypeComboBox
            // 
            this.transactionTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transactionTypeComboBox.FormattingEnabled = true;
            this.transactionTypeComboBox.Location = new System.Drawing.Point(260, 8);
            this.transactionTypeComboBox.Name = "transactionTypeComboBox";
            this.transactionTypeComboBox.Size = new System.Drawing.Size(181, 23);
            this.transactionTypeComboBox.TabIndex = 3;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 65);
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
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TabStop = false;
            // 
            // manufacturePanel
            // 
            this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturePanel.Controls.Add(this.manufactureComboBox);
            this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturePanel.Location = new System.Drawing.Point(0, 25);
            this.manufacturePanel.Name = "manufacturePanel";
            this.manufacturePanel.Size = new System.Drawing.Size(456, 40);
            this.manufacturePanel.TabIndex = 24;
            this.manufacturePanel.Tag = "Manufacture";
            // 
            // manufactureComboBox
            // 
            this.manufactureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manufactureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufactureComboBox.FormattingEnabled = true;
            this.manufactureComboBox.Location = new System.Drawing.Point(260, 8);
            this.manufactureComboBox.Name = "manufactureComboBox";
            this.manufactureComboBox.Size = new System.Drawing.Size(181, 23);
            this.manufactureComboBox.TabIndex = 3;
            // 
            // informationLabel
            // 
            this.informationLabel.BackColor = System.Drawing.Color.Transparent;
            this.informationLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.informationLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informationLabel.ForeColor = System.Drawing.Color.DimGray;
            this.informationLabel.Location = new System.Drawing.Point(0, 0);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.informationLabel.Size = new System.Drawing.Size(456, 25);
            this.informationLabel.TabIndex = 31;
            this.informationLabel.Text = "Information";
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // separatorDoubleBufferPanel
            // 
            this.separatorDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.separatorDoubleBufferPanel.Controls.Add(this.separatorTextBox);
            this.separatorDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorDoubleBufferPanel.Location = new System.Drawing.Point(0, 350);
            this.separatorDoubleBufferPanel.Name = "separatorDoubleBufferPanel";
            this.separatorDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.separatorDoubleBufferPanel.TabIndex = 33;
            this.separatorDoubleBufferPanel.Tag = "Name";
            // 
            // separatorTextBox
            // 
            this.separatorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.separatorTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.separatorTextBox.Location = new System.Drawing.Point(260, 8);
            this.separatorTextBox.MaxLength = 25;
            this.separatorTextBox.Name = "separatorTextBox";
            this.separatorTextBox.Size = new System.Drawing.Size(181, 21);
            this.separatorTextBox.TabIndex = 2;
            this.separatorTextBox.TabStop = false;
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 8, 12, 18);
            this.Size = new System.Drawing.Size(480, 491);
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.FilterPOSIdPanel.ResumeLayout(false);
            this.FilterPOSIdPanel.PerformLayout();
            this.transactionTypePanel.ResumeLayout(false);
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.manufacturePanel.ResumeLayout(false);
            this.separatorDoubleBufferPanel.ResumeLayout(false);
            this.separatorDoubleBufferPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.DoubleBufferPanel manufacturePanel;
        private System.Windows.Forms.ComboBox manufactureComboBox;
        private System.Windows.Forms.Label exceptionLabel;
        private PanelBase.DoubleBufferPanel exceptionContainerPanel;
        private PanelBase.DoubleBufferPanel tagContainerPanel;
        private System.Windows.Forms.Label tagLabel;
        private PanelBase.DoubleBufferPanel segmentContainerPanel;
        private System.Windows.Forms.Label segmentLabel;
        private System.Windows.Forms.Label informationLabel;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.DoubleBufferPanel transactionTypePanel;
        private System.Windows.Forms.ComboBox transactionTypeComboBox;
        private PanelBase.DoubleBufferPanel FilterPOSIdPanel;
        private System.Windows.Forms.CheckBox filterPODIdCheckBox;
        private PanelBase.DoubleBufferPanel separatorDoubleBufferPanel;
        private System.Windows.Forms.TextBox separatorTextBox;
    }
}
