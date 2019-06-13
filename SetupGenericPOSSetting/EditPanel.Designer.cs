namespace SetupGenericPOSSetting
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
            this.transactionListBox = new System.Windows.Forms.ListBox();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.exportButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.protocolPanel = new PanelBase.DoubleBufferPanel();
            this.protocolComboBox = new System.Windows.Forms.ComboBox();
            this.posIdDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.hotKeyTextBox1 = new PanelBase.HotKeyTextBox();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.acceptPortTextBox = new PanelBase.HotKeyTextBox();
            this.networkAddressPanel = new PanelBase.DoubleBufferPanel();
            this.ipAddressTextBox = new PanelBase.HotKeyTextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.containerPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.protocolPanel.SuspendLayout();
            this.posIdDoubleBufferPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.networkAddressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.transactionListBox);
            this.containerPanel.Controls.Add(this.buttonPanel);
            this.containerPanel.Controls.Add(this.protocolPanel);
            this.containerPanel.Controls.Add(this.posIdDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.portPanel);
            this.containerPanel.Controls.Add(this.networkAddressPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 18, 0, 15);
            this.containerPanel.Size = new System.Drawing.Size(456, 555);
            this.containerPanel.TabIndex = 20;
            // 
            // transactionListBox
            // 
            this.transactionListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(103)))), ((int)(((byte)(117)))));
            this.transactionListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.transactionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transactionListBox.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transactionListBox.ForeColor = System.Drawing.Color.White;
            this.transactionListBox.FormattingEnabled = true;
            this.transactionListBox.Location = new System.Drawing.Point(0, 217);
            this.transactionListBox.Margin = new System.Windows.Forms.Padding(0);
            this.transactionListBox.Name = "transactionListBox";
            this.transactionListBox.Size = new System.Drawing.Size(456, 323);
            this.transactionListBox.TabIndex = 33;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.exportButton);
            this.buttonPanel.Controls.Add(this.clearButton);
            this.buttonPanel.Controls.Add(this.pauseButton);
            this.buttonPanel.Controls.Add(this.stopButton);
            this.buttonPanel.Controls.Add(this.startButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.Location = new System.Drawing.Point(0, 178);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(456, 39);
            this.buttonPanel.TabIndex = 32;
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(256, 9);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 34;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButtonClick);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(175, 9);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 34;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(337, 9);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 34;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Visible = false;
            this.pauseButton.Click += new System.EventHandler(this.PauseButtonClick);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(94, 9);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 33;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(13, 9);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 32;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // protocolPanel
            // 
            this.protocolPanel.BackColor = System.Drawing.Color.Transparent;
            this.protocolPanel.Controls.Add(this.protocolComboBox);
            this.protocolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.protocolPanel.Location = new System.Drawing.Point(0, 138);
            this.protocolPanel.Name = "protocolPanel";
            this.protocolPanel.Size = new System.Drawing.Size(456, 40);
            this.protocolPanel.TabIndex = 25;
            this.protocolPanel.Tag = "Protocol";
            // 
            // protocolComboBox
            // 
            this.protocolComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolComboBox.FormattingEnabled = true;
            this.protocolComboBox.Location = new System.Drawing.Point(260, 8);
            this.protocolComboBox.Name = "protocolComboBox";
            this.protocolComboBox.Size = new System.Drawing.Size(181, 23);
            this.protocolComboBox.TabIndex = 3;
            // 
            // posIdDoubleBufferPanel
            // 
            this.posIdDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.posIdDoubleBufferPanel.Controls.Add(this.hotKeyTextBox1);
            this.posIdDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posIdDoubleBufferPanel.Location = new System.Drawing.Point(0, 98);
            this.posIdDoubleBufferPanel.Name = "posIdDoubleBufferPanel";
            this.posIdDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.posIdDoubleBufferPanel.TabIndex = 29;
            this.posIdDoubleBufferPanel.Tag = "POSId";
            this.posIdDoubleBufferPanel.Visible = false;
            // 
            // hotKeyTextBox1
            // 
            this.hotKeyTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hotKeyTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.hotKeyTextBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hotKeyTextBox1.Location = new System.Drawing.Point(260, 8);
            this.hotKeyTextBox1.MaxLength = 25;
            this.hotKeyTextBox1.Name = "hotKeyTextBox1";
            this.hotKeyTextBox1.ShortcutsEnabled = false;
            this.hotKeyTextBox1.Size = new System.Drawing.Size(181, 21);
            this.hotKeyTextBox1.TabIndex = 2;
            this.hotKeyTextBox1.TabStop = false;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.acceptPortTextBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 58);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(456, 40);
            this.portPanel.TabIndex = 28;
            this.portPanel.Tag = "Port";
            // 
            // acceptPortTextBox
            // 
            this.acceptPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.acceptPortTextBox.Location = new System.Drawing.Point(260, 8);
            this.acceptPortTextBox.MaxLength = 5;
            this.acceptPortTextBox.Name = "acceptPortTextBox";
            this.acceptPortTextBox.ShortcutsEnabled = false;
            this.acceptPortTextBox.Size = new System.Drawing.Size(181, 21);
            this.acceptPortTextBox.TabIndex = 2;
            // 
            // networkAddressPanel
            // 
            this.networkAddressPanel.BackColor = System.Drawing.Color.Transparent;
            this.networkAddressPanel.Controls.Add(this.ipAddressTextBox);
            this.networkAddressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.networkAddressPanel.Location = new System.Drawing.Point(0, 18);
            this.networkAddressPanel.Name = "networkAddressPanel";
            this.networkAddressPanel.Size = new System.Drawing.Size(456, 40);
            this.networkAddressPanel.TabIndex = 26;
            this.networkAddressPanel.Tag = "NetworkAddress";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddressTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ipAddressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ipAddressTextBox.Location = new System.Drawing.Point(260, 8);
            this.ipAddressTextBox.MaxLength = 25;
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.ShortcutsEnabled = false;
            this.ipAddressTextBox.Size = new System.Drawing.Size(181, 21);
            this.ipAddressTextBox.TabIndex = 2;
            this.ipAddressTextBox.TabStop = false;
            // 
            // EditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Name = "EditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 591);
            this.containerPanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.protocolPanel.ResumeLayout(false);
            this.posIdDoubleBufferPanel.ResumeLayout(false);
            this.posIdDoubleBufferPanel.PerformLayout();
            this.portPanel.ResumeLayout(false);
            this.portPanel.PerformLayout();
            this.networkAddressPanel.ResumeLayout(false);
            this.networkAddressPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel networkAddressPanel;
        private PanelBase.DoubleBufferPanel portPanel;
        private PanelBase.DoubleBufferPanel protocolPanel;
        private System.Windows.Forms.ComboBox protocolComboBox;
        private PanelBase.HotKeyTextBox ipAddressTextBox;
        private PanelBase.HotKeyTextBox acceptPortTextBox;
        private PanelBase.DoubleBufferPanel posIdDoubleBufferPanel;
        private PanelBase.HotKeyTextBox hotKeyTextBox1;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ListBox transactionListBox;
    }
}
