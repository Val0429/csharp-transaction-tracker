namespace SetupDevice
{
    sealed partial class ListPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.addedDeviceLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.searchDeviceDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.manufactureComboBox = new System.Windows.Forms.ComboBox();
            this.searchDeviceDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 15);
            this.label1.TabIndex = 9;
            // 
            // addedDeviceLabel
            // 
            this.addedDeviceLabel.BackColor = System.Drawing.Color.Transparent;
            this.addedDeviceLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.addedDeviceLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addedDeviceLabel.ForeColor = System.Drawing.Color.DimGray;
            this.addedDeviceLabel.Location = new System.Drawing.Point(12, 113);
            this.addedDeviceLabel.Name = "addedDeviceLabel";
            this.addedDeviceLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.addedDeviceLabel.Size = new System.Drawing.Size(456, 25);
            this.addedDeviceLabel.TabIndex = 11;
            this.addedDeviceLabel.Text = "Added Devices";
            this.addedDeviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 138);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 244);
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
            this.addNewDoubleBufferPanel.Tag = "AddNewDevice";
            // 
            // searchDeviceDoubleBufferPanel
            // 
            this.searchDeviceDoubleBufferPanel.AutoScroll = true;
            this.searchDeviceDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.searchDeviceDoubleBufferPanel.Controls.Add(this.manufactureComboBox);
            this.searchDeviceDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchDeviceDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchDeviceDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.searchDeviceDoubleBufferPanel.Name = "searchDeviceDoubleBufferPanel";
            this.searchDeviceDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.searchDeviceDoubleBufferPanel.TabIndex = 10;
            this.searchDeviceDoubleBufferPanel.Tag = "SearchDevice";
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
            this.manufactureComboBox.TabIndex = 0;
            this.manufactureComboBox.TabStop = false;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.addedDeviceLabel);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchDeviceDoubleBufferPanel);
            this.Name = "ListPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.searchDeviceDoubleBufferPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label addedDeviceLabel;
        private PanelBase.DoubleBufferPanel searchDeviceDoubleBufferPanel;
        private System.Windows.Forms.ComboBox manufactureComboBox;


    }
}
