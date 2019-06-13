namespace SetupDeviceGroup
{
    sealed partial class LayoutEditPanel
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
            this.layoutLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.deviceDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.device4DoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dewarpCheckBox4 = new System.Windows.Forms.CheckBox();
            this.device4ComboBox = new System.Windows.Forms.ComboBox();
            this.device3DoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dewarpCheckBox3 = new System.Windows.Forms.CheckBox();
            this.device3ComboBox = new System.Windows.Forms.ComboBox();
            this.device2DoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dewarpCheckBox2 = new System.Windows.Forms.CheckBox();
            this.device2ComboBox = new System.Windows.Forms.ComboBox();
            this.device1DoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.dewarpCheckBox1 = new System.Windows.Forms.CheckBox();
            this.device1ComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.subLayoutDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.layoutEditDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.resolutionDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.resolutionComboBox = new System.Windows.Forms.ComboBox();
            this.layoutDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.layoutComboBox = new System.Windows.Forms.ComboBox();
            this.nameDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.deviceDoubleBufferPanel.SuspendLayout();
            this.device4DoubleBufferPanel.SuspendLayout();
            this.device3DoubleBufferPanel.SuspendLayout();
            this.device2DoubleBufferPanel.SuspendLayout();
            this.device1DoubleBufferPanel.SuspendLayout();
            this.layoutEditDoubleBufferPanel.SuspendLayout();
            this.resolutionDoubleBufferPanel.SuspendLayout();
            this.layoutDoubleBufferPanel.SuspendLayout();
            this.nameDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 138);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.label1.Size = new System.Drawing.Size(456, 15);
            this.label1.TabIndex = 12;
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // layoutLabel
            // 
            this.layoutLabel.BackColor = System.Drawing.Color.Transparent;
            this.layoutLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.layoutLabel.Location = new System.Drawing.Point(12, 368);
            this.layoutLabel.Name = "layoutLabel";
            this.layoutLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.layoutLabel.Size = new System.Drawing.Size(456, 25);
            this.layoutLabel.TabIndex = 14;
            this.layoutLabel.Text = "layout";
            this.layoutLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 393);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 107);
            this.containerPanel.TabIndex = 2;
            // 
            // deviceDoubleBufferPanel
            // 
            this.deviceDoubleBufferPanel.AutoScroll = true;
            this.deviceDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.deviceDoubleBufferPanel.Controls.Add(this.device4DoubleBufferPanel);
            this.deviceDoubleBufferPanel.Controls.Add(this.device3DoubleBufferPanel);
            this.deviceDoubleBufferPanel.Controls.Add(this.device2DoubleBufferPanel);
            this.deviceDoubleBufferPanel.Controls.Add(this.device1DoubleBufferPanel);
            this.deviceDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deviceDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.deviceDoubleBufferPanel.Location = new System.Drawing.Point(12, 153);
            this.deviceDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.deviceDoubleBufferPanel.Name = "deviceDoubleBufferPanel";
            this.deviceDoubleBufferPanel.Size = new System.Drawing.Size(456, 160);
            this.deviceDoubleBufferPanel.TabIndex = 13;
            this.deviceDoubleBufferPanel.Tag = "Device";
            // 
            // device4DoubleBufferPanel
            // 
            this.device4DoubleBufferPanel.AutoScroll = true;
            this.device4DoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.device4DoubleBufferPanel.Controls.Add(this.dewarpCheckBox4);
            this.device4DoubleBufferPanel.Controls.Add(this.device4ComboBox);
            this.device4DoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.device4DoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.device4DoubleBufferPanel.Location = new System.Drawing.Point(0, 120);
            this.device4DoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.device4DoubleBufferPanel.Name = "device4DoubleBufferPanel";
            this.device4DoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.device4DoubleBufferPanel.TabIndex = 9;
            this.device4DoubleBufferPanel.Tag = "Layout";
            // 
            // dewarpCheckBox4
            // 
            this.dewarpCheckBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpCheckBox4.AutoSize = true;
            this.dewarpCheckBox4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpCheckBox4.Location = new System.Drawing.Point(214, 10);
            this.dewarpCheckBox4.Name = "dewarpCheckBox4";
            this.dewarpCheckBox4.Size = new System.Drawing.Size(69, 19);
            this.dewarpCheckBox4.TabIndex = 2;
            this.dewarpCheckBox4.Text = "Dewarp";
            this.dewarpCheckBox4.UseVisualStyleBackColor = true;
            this.dewarpCheckBox4.CheckedChanged += new System.EventHandler(this.DewarpCheckBox4CheckedChanged);
            // 
            // device4ComboBox
            // 
            this.device4ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.device4ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device4ComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device4ComboBox.FormattingEnabled = true;
            this.device4ComboBox.Location = new System.Drawing.Point(300, 8);
            this.device4ComboBox.Name = "device4ComboBox";
            this.device4ComboBox.Size = new System.Drawing.Size(141, 23);
            this.device4ComboBox.TabIndex = 0;
            // 
            // device3DoubleBufferPanel
            // 
            this.device3DoubleBufferPanel.AutoScroll = true;
            this.device3DoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.device3DoubleBufferPanel.Controls.Add(this.dewarpCheckBox3);
            this.device3DoubleBufferPanel.Controls.Add(this.device3ComboBox);
            this.device3DoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.device3DoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.device3DoubleBufferPanel.Location = new System.Drawing.Point(0, 80);
            this.device3DoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.device3DoubleBufferPanel.Name = "device3DoubleBufferPanel";
            this.device3DoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.device3DoubleBufferPanel.TabIndex = 8;
            this.device3DoubleBufferPanel.Tag = "Layout";
            // 
            // dewarpCheckBox3
            // 
            this.dewarpCheckBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpCheckBox3.AutoSize = true;
            this.dewarpCheckBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpCheckBox3.Location = new System.Drawing.Point(214, 10);
            this.dewarpCheckBox3.Name = "dewarpCheckBox3";
            this.dewarpCheckBox3.Size = new System.Drawing.Size(69, 19);
            this.dewarpCheckBox3.TabIndex = 2;
            this.dewarpCheckBox3.Text = "Dewarp";
            this.dewarpCheckBox3.UseVisualStyleBackColor = true;
            this.dewarpCheckBox3.CheckedChanged += new System.EventHandler(this.DewarpCheckBox3CheckedChanged);
            // 
            // device3ComboBox
            // 
            this.device3ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.device3ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device3ComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device3ComboBox.FormattingEnabled = true;
            this.device3ComboBox.Location = new System.Drawing.Point(300, 8);
            this.device3ComboBox.Name = "device3ComboBox";
            this.device3ComboBox.Size = new System.Drawing.Size(141, 23);
            this.device3ComboBox.TabIndex = 0;
            // 
            // device2DoubleBufferPanel
            // 
            this.device2DoubleBufferPanel.AutoScroll = true;
            this.device2DoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.device2DoubleBufferPanel.Controls.Add(this.dewarpCheckBox2);
            this.device2DoubleBufferPanel.Controls.Add(this.device2ComboBox);
            this.device2DoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.device2DoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.device2DoubleBufferPanel.Location = new System.Drawing.Point(0, 40);
            this.device2DoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.device2DoubleBufferPanel.Name = "device2DoubleBufferPanel";
            this.device2DoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.device2DoubleBufferPanel.TabIndex = 7;
            this.device2DoubleBufferPanel.Tag = "Layout";
            // 
            // dewarpCheckBox2
            // 
            this.dewarpCheckBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpCheckBox2.AutoSize = true;
            this.dewarpCheckBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpCheckBox2.Location = new System.Drawing.Point(214, 10);
            this.dewarpCheckBox2.Name = "dewarpCheckBox2";
            this.dewarpCheckBox2.Size = new System.Drawing.Size(69, 19);
            this.dewarpCheckBox2.TabIndex = 2;
            this.dewarpCheckBox2.Text = "Dewarp";
            this.dewarpCheckBox2.UseVisualStyleBackColor = true;
            this.dewarpCheckBox2.CheckedChanged += new System.EventHandler(this.DewarpCheckBox2CheckedChanged);
            // 
            // device2ComboBox
            // 
            this.device2ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.device2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device2ComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device2ComboBox.FormattingEnabled = true;
            this.device2ComboBox.Location = new System.Drawing.Point(300, 8);
            this.device2ComboBox.Name = "device2ComboBox";
            this.device2ComboBox.Size = new System.Drawing.Size(141, 23);
            this.device2ComboBox.TabIndex = 0;
            // 
            // device1DoubleBufferPanel
            // 
            this.device1DoubleBufferPanel.AutoScroll = true;
            this.device1DoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.device1DoubleBufferPanel.Controls.Add(this.dewarpCheckBox1);
            this.device1DoubleBufferPanel.Controls.Add(this.device1ComboBox);
            this.device1DoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.device1DoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.device1DoubleBufferPanel.Location = new System.Drawing.Point(0, 0);
            this.device1DoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.device1DoubleBufferPanel.Name = "device1DoubleBufferPanel";
            this.device1DoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.device1DoubleBufferPanel.TabIndex = 6;
            this.device1DoubleBufferPanel.Tag = "Layout";
            // 
            // dewarpCheckBox1
            // 
            this.dewarpCheckBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dewarpCheckBox1.AutoSize = true;
            this.dewarpCheckBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dewarpCheckBox1.Location = new System.Drawing.Point(214, 10);
            this.dewarpCheckBox1.Name = "dewarpCheckBox1";
            this.dewarpCheckBox1.Size = new System.Drawing.Size(69, 19);
            this.dewarpCheckBox1.TabIndex = 1;
            this.dewarpCheckBox1.Text = "Dewarp";
            this.dewarpCheckBox1.UseVisualStyleBackColor = true;
            this.dewarpCheckBox1.CheckedChanged += new System.EventHandler(this.DewarpCheckBox1CheckedChanged);
            // 
            // device1ComboBox
            // 
            this.device1ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.device1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device1ComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device1ComboBox.FormattingEnabled = true;
            this.device1ComboBox.Location = new System.Drawing.Point(300, 8);
            this.device1ComboBox.Name = "device1ComboBox";
            this.device1ComboBox.Size = new System.Drawing.Size(141, 23);
            this.device1ComboBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(12, 313);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.label3.Size = new System.Drawing.Size(456, 15);
            this.label3.TabIndex = 15;
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // subLayoutDoubleBufferPanel
            // 
            this.subLayoutDoubleBufferPanel.AutoScroll = true;
            this.subLayoutDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.subLayoutDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.subLayoutDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.subLayoutDoubleBufferPanel.Location = new System.Drawing.Point(12, 328);
            this.subLayoutDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.subLayoutDoubleBufferPanel.Name = "subLayoutDoubleBufferPanel";
            this.subLayoutDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.subLayoutDoubleBufferPanel.TabIndex = 16;
            this.subLayoutDoubleBufferPanel.Tag = "SubLayout";
            // 
            // layoutEditDoubleBufferPanel
            // 
            this.layoutEditDoubleBufferPanel.AutoScroll = true;
            this.layoutEditDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.layoutEditDoubleBufferPanel.Controls.Add(this.resolutionDoubleBufferPanel);
            this.layoutEditDoubleBufferPanel.Controls.Add(this.layoutDoubleBufferPanel);
            this.layoutEditDoubleBufferPanel.Controls.Add(this.nameDoubleBufferPanel);
            this.layoutEditDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.layoutEditDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutEditDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.layoutEditDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutEditDoubleBufferPanel.Name = "layoutEditDoubleBufferPanel";
            this.layoutEditDoubleBufferPanel.Size = new System.Drawing.Size(456, 120);
            this.layoutEditDoubleBufferPanel.TabIndex = 17;
            this.layoutEditDoubleBufferPanel.Tag = "";
            // 
            // resolutionDoubleBufferPanel
            // 
            this.resolutionDoubleBufferPanel.AutoScroll = true;
            this.resolutionDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.resolutionDoubleBufferPanel.Controls.Add(this.resolutionComboBox);
            this.resolutionDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.resolutionDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resolutionDoubleBufferPanel.Location = new System.Drawing.Point(0, 80);
            this.resolutionDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.resolutionDoubleBufferPanel.Name = "resolutionDoubleBufferPanel";
            this.resolutionDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.resolutionDoubleBufferPanel.TabIndex = 11;
            this.resolutionDoubleBufferPanel.Tag = "DeviceResolution";
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resolutionComboBox.FormattingEnabled = true;
            this.resolutionComboBox.Location = new System.Drawing.Point(300, 8);
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.Size = new System.Drawing.Size(141, 23);
            this.resolutionComboBox.TabIndex = 0;
            // 
            // layoutDoubleBufferPanel
            // 
            this.layoutDoubleBufferPanel.AutoScroll = true;
            this.layoutDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.layoutDoubleBufferPanel.Controls.Add(this.layoutComboBox);
            this.layoutDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.layoutDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutDoubleBufferPanel.Location = new System.Drawing.Point(0, 40);
            this.layoutDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDoubleBufferPanel.Name = "layoutDoubleBufferPanel";
            this.layoutDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.layoutDoubleBufferPanel.TabIndex = 6;
            this.layoutDoubleBufferPanel.Tag = "Name";
            // 
            // layoutComboBox
            // 
            this.layoutComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.layoutComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutComboBox.FormattingEnabled = true;
            this.layoutComboBox.Location = new System.Drawing.Point(300, 8);
            this.layoutComboBox.Name = "layoutComboBox";
            this.layoutComboBox.Size = new System.Drawing.Size(141, 23);
            this.layoutComboBox.TabIndex = 1;
            // 
            // nameDoubleBufferPanel
            // 
            this.nameDoubleBufferPanel.AutoScroll = true;
            this.nameDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.nameDoubleBufferPanel.Controls.Add(this.nameTextBox);
            this.nameDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.nameDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameDoubleBufferPanel.Location = new System.Drawing.Point(0, 0);
            this.nameDoubleBufferPanel.Margin = new System.Windows.Forms.Padding(0);
            this.nameDoubleBufferPanel.Name = "nameDoubleBufferPanel";
            this.nameDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.nameDoubleBufferPanel.TabIndex = 7;
            this.nameDoubleBufferPanel.Tag = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(300, 8);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(141, 21);
            this.nameTextBox.TabIndex = 0;
            // 
            // LayoutEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.layoutLabel);
            this.Controls.Add(this.subLayoutDoubleBufferPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.deviceDoubleBufferPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.layoutEditDoubleBufferPanel);
            this.Name = "LayoutEditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 0);
            this.Size = new System.Drawing.Size(480, 500);
            this.deviceDoubleBufferPanel.ResumeLayout(false);
            this.device4DoubleBufferPanel.ResumeLayout(false);
            this.device4DoubleBufferPanel.PerformLayout();
            this.device3DoubleBufferPanel.ResumeLayout(false);
            this.device3DoubleBufferPanel.PerformLayout();
            this.device2DoubleBufferPanel.ResumeLayout(false);
            this.device2DoubleBufferPanel.PerformLayout();
            this.device1DoubleBufferPanel.ResumeLayout(false);
            this.device1DoubleBufferPanel.PerformLayout();
            this.layoutEditDoubleBufferPanel.ResumeLayout(false);
            this.resolutionDoubleBufferPanel.ResumeLayout(false);
            this.layoutDoubleBufferPanel.ResumeLayout(false);
            this.nameDoubleBufferPanel.ResumeLayout(false);
            this.nameDoubleBufferPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label layoutLabel;
        private PanelBase.DoubleBufferPanel deviceDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel device4DoubleBufferPanel;
        private System.Windows.Forms.ComboBox device4ComboBox;
        private PanelBase.DoubleBufferPanel device3DoubleBufferPanel;
        private System.Windows.Forms.ComboBox device3ComboBox;
        private PanelBase.DoubleBufferPanel device2DoubleBufferPanel;
        private System.Windows.Forms.ComboBox device2ComboBox;
        private PanelBase.DoubleBufferPanel device1DoubleBufferPanel;
        private System.Windows.Forms.ComboBox device1ComboBox;
        private System.Windows.Forms.Label label3;
        private PanelBase.DoubleBufferPanel subLayoutDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel layoutEditDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel layoutDoubleBufferPanel;
        private System.Windows.Forms.ComboBox layoutComboBox;
        private PanelBase.DoubleBufferPanel nameDoubleBufferPanel;
        private System.Windows.Forms.TextBox nameTextBox;
        private PanelBase.DoubleBufferPanel resolutionDoubleBufferPanel;
        private System.Windows.Forms.ComboBox resolutionComboBox;
        private System.Windows.Forms.CheckBox dewarpCheckBox4;
        private System.Windows.Forms.CheckBox dewarpCheckBox3;
        private System.Windows.Forms.CheckBox dewarpCheckBox2;
        private System.Windows.Forms.CheckBox dewarpCheckBox1;
    }
}
