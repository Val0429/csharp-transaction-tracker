using System.Windows.Forms;

namespace SetupGenericPOSSetting
{
    sealed partial class EditTransactionPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewTagDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.tagContainerPanel = new PanelBase.DoubleBufferPanel();
            this.tagLabel = new System.Windows.Forms.Label();
            this.addNewExceptionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionContainerPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionLabel = new System.Windows.Forms.Label();
            this.segmentContainerPanel = new PanelBase.DoubleBufferPanel();
            this.segmentLabel = new System.Windows.Forms.Label();
            this.separatorDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.separatorTextBox = new System.Windows.Forms.TextBox();
            this.namePanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new PanelBase.HotKeyTextBox();
            this.importPanel = new PanelBase.DoubleBufferPanel();
            this.importButton = new System.Windows.Forms.Button();
            this.informationLabel = new System.Windows.Forms.Label();
            this.transactionPanel = new PanelBase.DoubleBufferPanel();
            this.transactionTextBox = new System.Windows.Forms.TextBox();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.exportButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.protocolPanel = new PanelBase.DoubleBufferPanel();
            this.protocolComboBox = new System.Windows.Forms.ComboBox();
            this.portPanel = new PanelBase.DoubleBufferPanel();
            this.acceptPortTextBox = new PanelBase.HotKeyTextBox();
            this.networkAddressPanel = new PanelBase.DoubleBufferPanel();
            this.ipAddressTextBox = new PanelBase.HotKeyTextBox();
            this.importTransactionPanel = new PanelBase.DoubleBufferPanel();
            this.importTransactionButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.containerPanel.SuspendLayout();
            this.separatorDoubleBufferPanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.importPanel.SuspendLayout();
            this.transactionPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.protocolPanel.SuspendLayout();
            this.portPanel.SuspendLayout();
            this.networkAddressPanel.SuspendLayout();
            this.importTransactionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(268, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 511);
            this.panel1.TabIndex = 35;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.addNewTagDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.tagContainerPanel);
            this.containerPanel.Controls.Add(this.tagLabel);
            this.containerPanel.Controls.Add(this.addNewExceptionDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.exceptionContainerPanel);
            this.containerPanel.Controls.Add(this.exceptionLabel);
            this.containerPanel.Controls.Add(this.segmentContainerPanel);
            this.containerPanel.Controls.Add(this.segmentLabel);
            this.containerPanel.Controls.Add(this.separatorDoubleBufferPanel);
            this.containerPanel.Controls.Add(this.namePanel);
            this.containerPanel.Controls.Add(this.importPanel);
            this.containerPanel.Controls.Add(this.informationLabel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 8);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(256, 511);
            this.containerPanel.TabIndex = 20;
            // 
            // addNewTagDoubleBufferPanel
            // 
            this.addNewTagDoubleBufferPanel.AutoScroll = true;
            this.addNewTagDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewTagDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewTagDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewTagDoubleBufferPanel.Location = new System.Drawing.Point(0, 350);
            this.addNewTagDoubleBufferPanel.Name = "addNewTagDoubleBufferPanel";
            this.addNewTagDoubleBufferPanel.Size = new System.Drawing.Size(256, 40);
            this.addNewTagDoubleBufferPanel.TabIndex = 34;
            this.addNewTagDoubleBufferPanel.Tag = "AddNewTag";
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
            this.tagContainerPanel.Size = new System.Drawing.Size(256, 30);
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
            this.tagLabel.Size = new System.Drawing.Size(256, 25);
            this.tagLabel.TabIndex = 29;
            this.tagLabel.Text = "Tag";
            this.tagLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // addNewExceptionDoubleBufferPanel
            // 
            this.addNewExceptionDoubleBufferPanel.AutoScroll = true;
            this.addNewExceptionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewExceptionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewExceptionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewExceptionDoubleBufferPanel.Location = new System.Drawing.Point(0, 255);
            this.addNewExceptionDoubleBufferPanel.Name = "addNewExceptionDoubleBufferPanel";
            this.addNewExceptionDoubleBufferPanel.Size = new System.Drawing.Size(256, 40);
            this.addNewExceptionDoubleBufferPanel.TabIndex = 33;
            this.addNewExceptionDoubleBufferPanel.Tag = "AddNewException";
            // 
            // exceptionContainerPanel
            // 
            this.exceptionContainerPanel.AutoScroll = true;
            this.exceptionContainerPanel.AutoSize = true;
            this.exceptionContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionContainerPanel.Location = new System.Drawing.Point(0, 225);
            this.exceptionContainerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.exceptionContainerPanel.Name = "exceptionContainerPanel";
            this.exceptionContainerPanel.Size = new System.Drawing.Size(256, 30);
            this.exceptionContainerPanel.TabIndex = 26;
            // 
            // exceptionLabel
            // 
            this.exceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.exceptionLabel.Location = new System.Drawing.Point(0, 200);
            this.exceptionLabel.Name = "exceptionLabel";
            this.exceptionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.exceptionLabel.Size = new System.Drawing.Size(256, 25);
            this.exceptionLabel.TabIndex = 25;
            this.exceptionLabel.Text = "Exception";
            this.exceptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // segmentContainerPanel
            // 
            this.segmentContainerPanel.AutoScroll = true;
            this.segmentContainerPanel.AutoSize = true;
            this.segmentContainerPanel.BackColor = System.Drawing.Color.Transparent;
            this.segmentContainerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.segmentContainerPanel.Location = new System.Drawing.Point(0, 170);
            this.segmentContainerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.segmentContainerPanel.Name = "segmentContainerPanel";
            this.segmentContainerPanel.Size = new System.Drawing.Size(256, 30);
            this.segmentContainerPanel.TabIndex = 28;
            // 
            // segmentLabel
            // 
            this.segmentLabel.BackColor = System.Drawing.Color.Transparent;
            this.segmentLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.segmentLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.segmentLabel.ForeColor = System.Drawing.Color.DimGray;
            this.segmentLabel.Location = new System.Drawing.Point(0, 145);
            this.segmentLabel.Name = "segmentLabel";
            this.segmentLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.segmentLabel.Size = new System.Drawing.Size(256, 25);
            this.segmentLabel.TabIndex = 27;
            this.segmentLabel.Text = "Transaction";
            this.segmentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // separatorDoubleBufferPanel
            // 
            this.separatorDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.separatorDoubleBufferPanel.Controls.Add(this.separatorTextBox);
            this.separatorDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorDoubleBufferPanel.Location = new System.Drawing.Point(0, 105);
            this.separatorDoubleBufferPanel.Name = "separatorDoubleBufferPanel";
            this.separatorDoubleBufferPanel.Size = new System.Drawing.Size(256, 40);
            this.separatorDoubleBufferPanel.TabIndex = 32;
            this.separatorDoubleBufferPanel.Tag = "Name";
            // 
            // separatorTextBox
            // 
            this.separatorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.separatorTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.separatorTextBox.Location = new System.Drawing.Point(60, 8);
            this.separatorTextBox.MaxLength = 25;
            this.separatorTextBox.Name = "separatorTextBox";
            this.separatorTextBox.Size = new System.Drawing.Size(181, 21);
            this.separatorTextBox.TabIndex = 2;
            this.separatorTextBox.TabStop = false;
            // 
            // namePanel
            // 
            this.namePanel.BackColor = System.Drawing.Color.Transparent;
            this.namePanel.Controls.Add(this.nameTextBox);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 65);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(256, 40);
            this.namePanel.TabIndex = 20;
            this.namePanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(60, 8);
            this.nameTextBox.MaxLength = 25;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ShortcutsEnabled = false;
            this.nameTextBox.Size = new System.Drawing.Size(181, 21);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TabStop = false;
            // 
            // importPanel
            // 
            this.importPanel.BackColor = System.Drawing.Color.Transparent;
            this.importPanel.Controls.Add(this.importButton);
            this.importPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.importPanel.Location = new System.Drawing.Point(0, 25);
            this.importPanel.Name = "importPanel";
            this.importPanel.Size = new System.Drawing.Size(256, 40);
            this.importPanel.TabIndex = 24;
            this.importPanel.Tag = "Import";
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Location = new System.Drawing.Point(166, 9);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.AddExceptionByFileDoubleBufferPanelMouseClick);
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
            this.informationLabel.Size = new System.Drawing.Size(256, 25);
            this.informationLabel.TabIndex = 31;
            this.informationLabel.Text = "Information";
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // transactionPanel
            // 
            this.transactionPanel.AutoScroll = true;
            this.transactionPanel.BackColor = System.Drawing.Color.Transparent;
            this.transactionPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.transactionPanel.Controls.Add(this.transactionTextBox);
            this.transactionPanel.Controls.Add(this.buttonPanel);
            this.transactionPanel.Controls.Add(this.protocolPanel);
            this.transactionPanel.Controls.Add(this.portPanel);
            this.transactionPanel.Controls.Add(this.networkAddressPanel);
            this.transactionPanel.Controls.Add(this.importTransactionPanel);
            this.transactionPanel.Controls.Add(this.infoLabel);
            this.transactionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.transactionPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transactionPanel.Location = new System.Drawing.Point(278, 8);
            this.transactionPanel.Name = "transactionPanel";
            this.transactionPanel.Size = new System.Drawing.Size(528, 511);
            this.transactionPanel.TabIndex = 37;
            // 
            // transactionTextBox
            // 
            this.transactionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transactionTextBox.Font = new System.Drawing.Font("Arial", 9F);
            this.transactionTextBox.Location = new System.Drawing.Point(0, 224);
            this.transactionTextBox.Multiline = true;
            this.transactionTextBox.Name = "transactionTextBox";
            this.transactionTextBox.ReadOnly = true;
            this.transactionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.transactionTextBox.Size = new System.Drawing.Size(528, 287);
            this.transactionTextBox.TabIndex = 37;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.exportButton);
            this.buttonPanel.Controls.Add(this.clearButton);
            this.buttonPanel.Controls.Add(this.pauseButton);
            this.buttonPanel.Controls.Add(this.stopButton);
            this.buttonPanel.Controls.Add(this.startButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.Location = new System.Drawing.Point(0, 185);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(528, 39);
            this.buttonPanel.TabIndex = 41;
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
            this.protocolPanel.Location = new System.Drawing.Point(0, 145);
            this.protocolPanel.Name = "protocolPanel";
            this.protocolPanel.Size = new System.Drawing.Size(528, 40);
            this.protocolPanel.TabIndex = 38;
            this.protocolPanel.Tag = "Protocol";
            // 
            // protocolComboBox
            // 
            this.protocolComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolComboBox.FormattingEnabled = true;
            this.protocolComboBox.Location = new System.Drawing.Point(332, 8);
            this.protocolComboBox.Name = "protocolComboBox";
            this.protocolComboBox.Size = new System.Drawing.Size(181, 23);
            this.protocolComboBox.TabIndex = 3;
            // 
            // portPanel
            // 
            this.portPanel.BackColor = System.Drawing.Color.Transparent;
            this.portPanel.Controls.Add(this.acceptPortTextBox);
            this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portPanel.Location = new System.Drawing.Point(0, 105);
            this.portPanel.Name = "portPanel";
            this.portPanel.Size = new System.Drawing.Size(528, 40);
            this.portPanel.TabIndex = 40;
            this.portPanel.Tag = "Port";
            // 
            // acceptPortTextBox
            // 
            this.acceptPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptPortTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acceptPortTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.acceptPortTextBox.Location = new System.Drawing.Point(332, 8);
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
            this.networkAddressPanel.Location = new System.Drawing.Point(0, 65);
            this.networkAddressPanel.Name = "networkAddressPanel";
            this.networkAddressPanel.Size = new System.Drawing.Size(528, 40);
            this.networkAddressPanel.TabIndex = 39;
            this.networkAddressPanel.Tag = "NetworkAddress";
            this.networkAddressPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.networkAddressPanel_Paint);
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddressTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ipAddressTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ipAddressTextBox.Location = new System.Drawing.Point(332, 8);
            this.ipAddressTextBox.MaxLength = 25;
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.ShortcutsEnabled = false;
            this.ipAddressTextBox.Size = new System.Drawing.Size(181, 21);
            this.ipAddressTextBox.TabIndex = 2;
            this.ipAddressTextBox.TabStop = false;
            // 
            // importTransactionPanel
            // 
            this.importTransactionPanel.BackColor = System.Drawing.Color.Transparent;
            this.importTransactionPanel.Controls.Add(this.importTransactionButton);
            this.importTransactionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.importTransactionPanel.Location = new System.Drawing.Point(0, 25);
            this.importTransactionPanel.Name = "importTransactionPanel";
            this.importTransactionPanel.Size = new System.Drawing.Size(528, 40);
            this.importTransactionPanel.TabIndex = 24;
            this.importTransactionPanel.Tag = "ImportTransaction";
            // 
            // importTransactionButton
            // 
            this.importTransactionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importTransactionButton.Location = new System.Drawing.Point(438, 9);
            this.importTransactionButton.Name = "importTransactionButton";
            this.importTransactionButton.Size = new System.Drawing.Size(75, 23);
            this.importTransactionButton.TabIndex = 0;
            this.importTransactionButton.Text = "Import";
            this.importTransactionButton.UseVisualStyleBackColor = true;
            this.importTransactionButton.Click += new System.EventHandler(this.ImportButtonClick);
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(0, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.infoLabel.Size = new System.Drawing.Size(528, 25);
            this.infoLabel.TabIndex = 31;
            this.infoLabel.Text = "Transaction";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // EditTransactionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.transactionPanel);
            this.Name = "EditTransactionPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 8, 12, 18);
            this.Size = new System.Drawing.Size(818, 537);
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.separatorDoubleBufferPanel.ResumeLayout(false);
            this.separatorDoubleBufferPanel.PerformLayout();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.importPanel.ResumeLayout(false);
            this.transactionPanel.ResumeLayout(false);
            this.transactionPanel.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.protocolPanel.ResumeLayout(false);
            this.portPanel.ResumeLayout(false);
            this.portPanel.PerformLayout();
            this.networkAddressPanel.ResumeLayout(false);
            this.networkAddressPanel.PerformLayout();
            this.importTransactionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel namePanel;
        private PanelBase.DoubleBufferPanel importPanel;
        private System.Windows.Forms.Label exceptionLabel;
        private PanelBase.DoubleBufferPanel exceptionContainerPanel;
        private PanelBase.DoubleBufferPanel tagContainerPanel;
        private System.Windows.Forms.Label tagLabel;
        private PanelBase.DoubleBufferPanel segmentContainerPanel;
        private System.Windows.Forms.Label segmentLabel;
        private System.Windows.Forms.Label informationLabel;
        private PanelBase.HotKeyTextBox nameTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private PanelBase.DoubleBufferPanel separatorDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel addNewExceptionDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel addNewTagDoubleBufferPanel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private TextBox separatorTextBox;
        private PanelBase.DoubleBufferPanel transactionPanel;
        private TextBox transactionTextBox;
        private PanelBase.DoubleBufferPanel importTransactionPanel;
        private Button importTransactionButton;
        private Label infoLabel;
        private Panel buttonPanel;
        private Button exportButton;
        private Button clearButton;
        private Button pauseButton;
        private Button stopButton;
        private Button startButton;
        private PanelBase.DoubleBufferPanel protocolPanel;
        private ComboBox protocolComboBox;
        private PanelBase.DoubleBufferPanel portPanel;
        private PanelBase.HotKeyTextBox acceptPortTextBox;
        private PanelBase.DoubleBufferPanel networkAddressPanel;
        private PanelBase.HotKeyTextBox ipAddressTextBox;
        private SaveFileDialog saveFileDialog;
    }
}
