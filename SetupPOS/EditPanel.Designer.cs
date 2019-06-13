namespace SetupPOS
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
            this.keywordPanel = new PanelBase.DoubleBufferPanel();
            this.keywordComboBox = new System.Windows.Forms.ComboBox();
            this.exceptionPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionComboBox = new System.Windows.Forms.ComboBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.modelPanel = new PanelBase.DoubleBufferPanel();
            this.modelComboBox = new System.Windows.Forms.ComboBox();
            this.manufacturePanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.registerIDPanel = new PanelBase.DoubleBufferPanel();
            this.registerIDTextBox = new PanelBase.HotKeyTextBox();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.posPanel.SuspendLayout();
            this.keywordPanel.SuspendLayout();
            this.exceptionPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.modelPanel.SuspendLayout();
            this.manufacturePanel.SuspendLayout();
            this.registerIDPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // posPanel
            // 
            this.posPanel.AutoSize = true;
            this.posPanel.BackColor = System.Drawing.Color.Transparent;
            this.posPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.posPanel.Controls.Add(this.keywordPanel);
            this.posPanel.Controls.Add(this.exceptionPanel);
            this.posPanel.Controls.Add(this.namePanel);
            this.posPanel.Controls.Add(this.modelPanel);
            this.posPanel.Controls.Add(this.manufacturePanel);
            this.posPanel.Controls.Add(this.registerIDPanel);
            this.posPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posPanel.Location = new System.Drawing.Point(12, 18);
            this.posPanel.Name = "posPanel";
            this.posPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.posPanel.Size = new System.Drawing.Size(456, 252);
            this.posPanel.TabIndex = 20;
            // 
            // keywordPanel
            // 
            this.keywordPanel.BackColor = System.Drawing.Color.Transparent;
            this.keywordPanel.Controls.Add(this.keywordComboBox);
            this.keywordPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.keywordPanel.Location = new System.Drawing.Point(0, 200);
            this.keywordPanel.Name = "keywordPanel";
            this.keywordPanel.Size = new System.Drawing.Size(456, 40);
            this.keywordPanel.TabIndex = 25;
            this.keywordPanel.Tag = "Keyword";
            this.keywordPanel.Visible = false;
            // 
            // keywordComboBox
            // 
            this.keywordComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keywordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keywordComboBox.FormattingEnabled = true;
            this.keywordComboBox.Location = new System.Drawing.Point(260, 8);
            this.keywordComboBox.Name = "keywordComboBox";
            this.keywordComboBox.Size = new System.Drawing.Size(181, 23);
            this.keywordComboBox.TabIndex = 3;
            // 
            // exceptionPanel
            // 
            this.exceptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionPanel.Controls.Add(this.exceptionComboBox);
            this.exceptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionPanel.Location = new System.Drawing.Point(0, 160);
            this.exceptionPanel.Name = "exceptionPanel";
            this.exceptionPanel.Size = new System.Drawing.Size(456, 40);
            this.exceptionPanel.TabIndex = 23;
            this.exceptionPanel.Tag = "Exception";
            // 
            // exceptionComboBox
            // 
            this.exceptionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exceptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exceptionComboBox.FormattingEnabled = true;
            this.exceptionComboBox.Location = new System.Drawing.Point(260, 8);
            this.exceptionComboBox.Name = "exceptionComboBox";
            this.exceptionComboBox.Size = new System.Drawing.Size(181, 23);
            this.exceptionComboBox.TabIndex = 3;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 120);
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
            // modelPanel
            // 
            this.modelPanel.BackColor = System.Drawing.Color.Transparent;
            this.modelPanel.Controls.Add(this.modelComboBox);
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelPanel.Location = new System.Drawing.Point(0, 80);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(456, 40);
            this.modelPanel.TabIndex = 2;
            this.modelPanel.Tag = "Model";
            this.modelPanel.Visible = false;
            // 
            // modelComboBox
            // 
            this.modelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelComboBox.FormattingEnabled = true;
            this.modelComboBox.Location = new System.Drawing.Point(260, 8);
            this.modelComboBox.Name = "modelComboBox";
            this.modelComboBox.Size = new System.Drawing.Size(181, 23);
            this.modelComboBox.TabIndex = 4;
            // 
            // manufacturePanel
            // 
            this.manufacturePanel.BackColor = System.Drawing.Color.Transparent;
            this.manufacturePanel.Controls.Add(this.manufactureComboBox);
            this.manufacturePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.manufacturePanel.Location = new System.Drawing.Point(0, 40);
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
            // registerIDPanel
            // 
            this.registerIDPanel.BackColor = System.Drawing.Color.Transparent;
            this.registerIDPanel.Controls.Add(this.registerIDTextBox);
            this.registerIDPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.registerIDPanel.Location = new System.Drawing.Point(0, 0);
            this.registerIDPanel.Name = "registerIDPanel";
            this.registerIDPanel.Size = new System.Drawing.Size(456, 40);
            this.registerIDPanel.TabIndex = 22;
            this.registerIDPanel.Tag = "RegisterId";
            // 
            // registerIDTextBox
            // 
            this.registerIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.registerIDTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.registerIDTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registerIDTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.registerIDTextBox.Location = new System.Drawing.Point(260, 8);
            this.registerIDTextBox.MaxLength = 999;
            this.registerIDTextBox.Name = "registerIDTextBox";
            this.registerIDTextBox.ShortcutsEnabled = false;
            this.registerIDTextBox.Size = new System.Drawing.Size(181, 21);
            this.registerIDTextBox.TabIndex = 3;
            this.registerIDTextBox.TabStop = false;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 270);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 203);
            this.containerPanel.TabIndex = 21;
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.posPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 491);
            this.posPanel.ResumeLayout(false);
            this.keywordPanel.ResumeLayout(false);
            this.exceptionPanel.ResumeLayout(false);
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.modelPanel.ResumeLayout(false);
            this.manufacturePanel.ResumeLayout(false);
            this.registerIDPanel.ResumeLayout(false);
            this.registerIDPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel posPanel;
        private PanelBase.DoubleBufferPanel modelPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.DoubleBufferPanel exceptionPanel;
        private System.Windows.Forms.ComboBox exceptionComboBox;
        private PanelBase.DoubleBufferPanel registerIDPanel;
        private PanelBase.DoubleBufferPanel manufacturePanel;
        private System.Windows.Forms.ComboBox manufactureComboBox;
        private System.Windows.Forms.ComboBox modelComboBox;
        private PanelBase.DoubleBufferPanel keywordPanel;
        private System.Windows.Forms.ComboBox keywordComboBox;
        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.HotKeyTextBox nameTextBox;
        private PanelBase.HotKeyTextBox registerIDTextBox;
    }
}
