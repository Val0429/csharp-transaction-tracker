namespace SetupNVR
{
    partial class ListPanel
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
            this.addedNVRLabel = new System.Windows.Forms.Label();
            this.flagGrayLabel = new System.Windows.Forms.Label();
            this.flagGreenLabel = new System.Windows.Forms.Label();
            this.flagYellowLabel = new System.Windows.Forms.Label();
            this.flagRedLabel = new System.Windows.Forms.Label();
            this.flagCloseLabel = new System.Windows.Forms.Label();
            this.labelSearch = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.searchDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.searchDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // addedNVRLabel
            // 
            this.addedNVRLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedNVRLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedNVRLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedNVRLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedNVRLabel.Location = new System.Drawing.Point(12, 113);
            this.addedNVRLabel.Name = "addedNVRLabel";
            this.addedNVRLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedNVRLabel.Size = new System.Drawing.Size(456, 25);
            this.addedNVRLabel.TabIndex = 11;
            this.addedNVRLabel.Text = "Added NVR";
            this.addedNVRLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // flagGrayLabel
            // 
            this.flagGrayLabel.BackColor = System.Drawing.Color.Transparent;
            this.flagGrayLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flagGrayLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagGrayLabel.ForeColor = System.Drawing.Color.DimGray;
            this.flagGrayLabel.Image = global::SetupNVR.Properties.Resources.flag_gray;
            this.flagGrayLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagGrayLabel.Location = new System.Drawing.Point(12, 168);
            this.flagGrayLabel.Name = "flagGrayLabel";
            this.flagGrayLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flagGrayLabel.Size = new System.Drawing.Size(456, 25);
            this.flagGrayLabel.TabIndex = 12;
            this.flagGrayLabel.Text = "        None: No status.";
            this.flagGrayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagGrayLabel.Visible = false;
            // 
            // flagGreenLabel
            // 
            this.flagGreenLabel.BackColor = System.Drawing.Color.Transparent;
            this.flagGreenLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flagGreenLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagGreenLabel.ForeColor = System.Drawing.Color.DimGray;
            this.flagGreenLabel.Image = global::SetupNVR.Properties.Resources.flag_green;
            this.flagGreenLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagGreenLabel.Location = new System.Drawing.Point(12, 193);
            this.flagGreenLabel.Name = "flagGreenLabel";
            this.flagGreenLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flagGreenLabel.Size = new System.Drawing.Size(456, 25);
            this.flagGreenLabel.TabIndex = 13;
            this.flagGreenLabel.Text = "        Good: Check status no response percentage between 0% to 35%.";
            this.flagGreenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagGreenLabel.Visible = false;
            // 
            // flagYellowLabel
            // 
            this.flagYellowLabel.BackColor = System.Drawing.Color.Transparent;
            this.flagYellowLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flagYellowLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagYellowLabel.ForeColor = System.Drawing.Color.DimGray;
            this.flagYellowLabel.Image = global::SetupNVR.Properties.Resources.flag_yellow;
            this.flagYellowLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagYellowLabel.Location = new System.Drawing.Point(12, 218);
            this.flagYellowLabel.Name = "flagYellowLabel";
            this.flagYellowLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flagYellowLabel.Size = new System.Drawing.Size(456, 25);
            this.flagYellowLabel.TabIndex = 14;
            this.flagYellowLabel.Text = "        Normal: Check status no response percentage between 36% to 70%.";
            this.flagYellowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagYellowLabel.Visible = false;
            // 
            // flagRedLabel
            // 
            this.flagRedLabel.BackColor = System.Drawing.Color.Transparent;
            this.flagRedLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flagRedLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagRedLabel.ForeColor = System.Drawing.Color.DimGray;
            this.flagRedLabel.Image = global::SetupNVR.Properties.Resources.flag_red;
            this.flagRedLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagRedLabel.Location = new System.Drawing.Point(12, 243);
            this.flagRedLabel.Name = "flagRedLabel";
            this.flagRedLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flagRedLabel.Size = new System.Drawing.Size(456, 25);
            this.flagRedLabel.TabIndex = 15;
            this.flagRedLabel.Text = "        Warning: Check status no response percentage between 71% to 99%.";
            this.flagRedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagRedLabel.Visible = false;
            // 
            // flagCloseLabel
            // 
            this.flagCloseLabel.BackColor = System.Drawing.Color.Transparent;
            this.flagCloseLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flagCloseLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagCloseLabel.ForeColor = System.Drawing.Color.DimGray;
            this.flagCloseLabel.Image = global::SetupNVR.Properties.Resources.flag_close;
            this.flagCloseLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagCloseLabel.Location = new System.Drawing.Point(12, 268);
            this.flagCloseLabel.Name = "flagCloseLabel";
            this.flagCloseLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flagCloseLabel.Size = new System.Drawing.Size(456, 25);
            this.flagCloseLabel.TabIndex = 16;
            this.flagCloseLabel.Text = "        Failures: Server failures. Check status no response percentage 100%.";
            this.flagCloseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flagCloseLabel.Visible = false;
            // 
            // labelSearch
            // 
            this.labelSearch.BackColor = System.Drawing.Color.Transparent;
            this.labelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSearch.Location = new System.Drawing.Point(12, 58);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(456, 15);
            this.labelSearch.TabIndex = 18;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 138);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 30);
            this.containerPanel.TabIndex = 1;
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 73);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.addNewDoubleBufferPanel.TabIndex = 3;
            this.addNewDoubleBufferPanel.Tag = "AddNewNVR";
            // 
            // searchDoubleBufferPanel
            // 
            this.searchDoubleBufferPanel.AutoScroll = true;
            this.searchDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.searchDoubleBufferPanel.Controls.Add(this.manufactureComboBox);
            this.searchDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.searchDoubleBufferPanel.Name = "searchDoubleBufferPanel";
            this.searchDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.searchDoubleBufferPanel.TabIndex = 17;
            this.searchDoubleBufferPanel.Tag = "SearchNVR";
            // 
            // manufactureComboBox
            // 
            this.manufactureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manufactureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufactureComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manufactureComboBox.FormattingEnabled = true;
            this.manufactureComboBox.Location = new System.Drawing.Point(305, 8);
            this.manufactureComboBox.Name = "manufactureComboBox";
            this.manufactureComboBox.Size = new System.Drawing.Size(121, 23);
            this.manufactureComboBox.TabIndex = 1;
            this.manufactureComboBox.TabStop = false;
            this.manufactureComboBox.Visible = false;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.flagCloseLabel);
            this.Controls.Add(this.flagRedLabel);
            this.Controls.Add(this.flagYellowLabel);
            this.Controls.Add(this.flagGreenLabel);
            this.Controls.Add(this.flagGrayLabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedNVRLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.searchDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.searchDoubleBufferPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        public PanelBase.DoubleBufferPanel searchDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label addedNVRLabel;
        private System.Windows.Forms.Label flagGrayLabel;
        private System.Windows.Forms.Label flagGreenLabel;
        private System.Windows.Forms.Label flagYellowLabel;
        private System.Windows.Forms.Label flagRedLabel;
        private System.Windows.Forms.Label flagCloseLabel;
        protected System.Windows.Forms.Label labelSearch;
        protected System.Windows.Forms.ComboBox manufactureComboBox;
        


    }
}
