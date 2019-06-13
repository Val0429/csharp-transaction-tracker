namespace SetupGeneral
{
    partial class ExportVideoPath
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
            this.customPanel = new PanelBase.DoubleBufferPanel();
            this.browserButton = new System.Windows.Forms.Button();
            this.exportVideoPathTextBox = new PanelBase.HotKeyTextBox();
            this.folderNameLabel = new System.Windows.Forms.Label();
            this.exportVideoFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.customPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 58);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 40);
            this.containerPanel.TabIndex = 1;
            // 
            // customPanel
            // 
            this.customPanel.BackColor = System.Drawing.Color.Transparent;
            this.customPanel.Controls.Add(this.browserButton);
            this.customPanel.Controls.Add(this.exportVideoPathTextBox);
            this.customPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel.Location = new System.Drawing.Point(12, 18);
            this.customPanel.Name = "customPanel";
            this.customPanel.Size = new System.Drawing.Size(456, 40);
            this.customPanel.TabIndex = 7;
            this.customPanel.Tag = "Custom";
            // 
            // browserButton
            // 
            this.browserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browserButton.BackColor = System.Drawing.Color.Transparent;
            this.browserButton.BackgroundImage = global::SetupGeneral.Properties.Resources.SelectFolder;
            this.browserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.browserButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.browserButton.FlatAppearance.BorderSize = 0;
            this.browserButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.browserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browserButton.ForeColor = System.Drawing.Color.Black;
            this.browserButton.Location = new System.Drawing.Point(404, 7);
            this.browserButton.Name = "browserButton";
            this.browserButton.Size = new System.Drawing.Size(38, 26);
            this.browserButton.TabIndex = 10;
            this.browserButton.UseVisualStyleBackColor = false;
            this.browserButton.Click += new System.EventHandler(this.BrowserButtonClick);
            // 
            // exportVideoPathTextBox
            // 
            this.exportVideoPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportVideoPathTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportVideoPathTextBox.Location = new System.Drawing.Point(232, 9);
            this.exportVideoPathTextBox.Name = "exportVideoPathTextBox";
            this.exportVideoPathTextBox.ShortcutsEnabled = false;
            this.exportVideoPathTextBox.Size = new System.Drawing.Size(166, 21);
            this.exportVideoPathTextBox.TabIndex = 0;
            // 
            // folderNameLabel
            // 
            this.folderNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.folderNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.folderNameLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folderNameLabel.ForeColor = System.Drawing.Color.DimGray;
            this.folderNameLabel.Location = new System.Drawing.Point(12, 98);
            this.folderNameLabel.Name = "folderNameLabel";
            this.folderNameLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.folderNameLabel.Size = new System.Drawing.Size(456, 20);
            this.folderNameLabel.TabIndex = 8;
            this.folderNameLabel.Tag = "";
            this.folderNameLabel.Text = "ExportFolderName";
            this.folderNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExportVideoPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.folderNameLabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.customPanel);
            this.DoubleBuffered = true;
            this.Name = "ExportVideoPath";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.customPanel.ResumeLayout(false);
            this.customPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel customPanel;
        private System.Windows.Forms.Label folderNameLabel;
        private System.Windows.Forms.Button browserButton;
        private System.Windows.Forms.FolderBrowserDialog exportVideoFolderBrowserDialog;
        private PanelBase.HotKeyTextBox exportVideoPathTextBox;


    }
}
