namespace SetupGeneral
{
    sealed partial class VideoWindowTitleBar
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
            this.keepLabel = new System.Windows.Forms.Label();
            this.noteLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.fontSettingLabel = new System.Windows.Forms.Label();
            this.fontColorDialog = new System.Windows.Forms.ColorDialog();
            this.backgroundDialog = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.previewBar = new System.Windows.Forms.Label();
            this.backgroundColorPanel = new PanelBase.DoubleBufferPanel();
            this.backgroundColorButton = new System.Windows.Forms.Button();
            this.fontColorPanel = new PanelBase.DoubleBufferPanel();
            this.fontcolorButton = new System.Windows.Forms.Button();
            this.fontSizePanel = new PanelBase.DoubleBufferPanel();
            this.fontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.fontFamilyPanel = new PanelBase.DoubleBufferPanel();
            this.fontFamilyComboBox = new System.Windows.Forms.ComboBox();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.previewLabel = new System.Windows.Forms.Label();
            this.previewPanel.SuspendLayout();
            this.backgroundColorPanel.SuspendLayout();
            this.fontColorPanel.SuspendLayout();
            this.fontSizePanel.SuspendLayout();
            this.fontFamilyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // keepLabel
            // 
            this.keepLabel.BackColor = System.Drawing.Color.Transparent;
            this.keepLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.keepLabel.Location = new System.Drawing.Point(12, 18);
            this.keepLabel.Name = "keepLabel";
            this.keepLabel.Size = new System.Drawing.Size(576, 15);
            this.keepLabel.TabIndex = 25;
            // 
            // noteLabel
            // 
            this.noteLabel.BackColor = System.Drawing.Color.Transparent;
            this.noteLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.noteLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noteLabel.ForeColor = System.Drawing.Color.DimGray;
            this.noteLabel.Location = new System.Drawing.Point(12, 83);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.noteLabel.Size = new System.Drawing.Size(576, 25);
            this.noteLabel.TabIndex = 26;
            this.noteLabel.Text = "Drag to order display. ";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(12, 108);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(576, 25);
            this.infoLabel.TabIndex = 27;
            this.infoLabel.Text = "Informations will be displayed from left to right on title bar.";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fontSettingLabel
            // 
            this.fontSettingLabel.BackColor = System.Drawing.Color.Transparent;
            this.fontSettingLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontSettingLabel.Location = new System.Drawing.Point(12, 133);
            this.fontSettingLabel.Name = "fontSettingLabel";
            this.fontSettingLabel.Size = new System.Drawing.Size(576, 15);
            this.fontSettingLabel.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(12, 308);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(576, 15);
            this.label1.TabIndex = 35;
            // 
            // previewPanel
            // 
            this.previewPanel.Controls.Add(this.previewBar);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewPanel.Location = new System.Drawing.Point(12, 348);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(576, 30);
            this.previewPanel.TabIndex = 36;
            // 
            // previewBar
            // 
            this.previewBar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.previewBar.Location = new System.Drawing.Point(14, 9);
            this.previewBar.Name = "previewBar";
            this.previewBar.Size = new System.Drawing.Size(506, 16);
            this.previewBar.TabIndex = 1;
            this.previewBar.Text = "label2";
            // 
            // backgroundColorPanel
            // 
            this.backgroundColorPanel.BackColor = System.Drawing.Color.Transparent;
            this.backgroundColorPanel.Controls.Add(this.backgroundColorButton);
            this.backgroundColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.backgroundColorPanel.Location = new System.Drawing.Point(12, 268);
            this.backgroundColorPanel.Name = "backgroundColorPanel";
            this.backgroundColorPanel.Size = new System.Drawing.Size(576, 40);
            this.backgroundColorPanel.TabIndex = 34;
            this.backgroundColorPanel.Tag = "BackgroundColor";
            // 
            // backgroundColorButton
            // 
            this.backgroundColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundColorButton.BackColor = System.Drawing.Color.Transparent;
            this.backgroundColorButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundColorButton.Location = new System.Drawing.Point(411, 7);
            this.backgroundColorButton.Name = "backgroundColorButton";
            this.backgroundColorButton.Size = new System.Drawing.Size(150, 27);
            this.backgroundColorButton.TabIndex = 1;
            this.backgroundColorButton.Text = "Color";
            this.backgroundColorButton.UseVisualStyleBackColor = false;
            // 
            // fontColorPanel
            // 
            this.fontColorPanel.BackColor = System.Drawing.Color.Transparent;
            this.fontColorPanel.Controls.Add(this.fontcolorButton);
            this.fontColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontColorPanel.Location = new System.Drawing.Point(12, 228);
            this.fontColorPanel.Name = "fontColorPanel";
            this.fontColorPanel.Size = new System.Drawing.Size(576, 40);
            this.fontColorPanel.TabIndex = 33;
            this.fontColorPanel.Tag = "FontColor";
            // 
            // fontcolorButton
            // 
            this.fontcolorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontcolorButton.BackColor = System.Drawing.Color.Transparent;
            this.fontcolorButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontcolorButton.Location = new System.Drawing.Point(411, 7);
            this.fontcolorButton.Name = "fontcolorButton";
            this.fontcolorButton.Size = new System.Drawing.Size(150, 27);
            this.fontcolorButton.TabIndex = 0;
            this.fontcolorButton.Text = "Color";
            this.fontcolorButton.UseVisualStyleBackColor = false;
            // 
            // fontSizePanel
            // 
            this.fontSizePanel.BackColor = System.Drawing.Color.Transparent;
            this.fontSizePanel.Controls.Add(this.fontSizeComboBox);
            this.fontSizePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontSizePanel.Location = new System.Drawing.Point(12, 188);
            this.fontSizePanel.Name = "fontSizePanel";
            this.fontSizePanel.Size = new System.Drawing.Size(576, 40);
            this.fontSizePanel.TabIndex = 32;
            this.fontSizePanel.Tag = "FontSize";
            // 
            // fontSizeComboBox
            // 
            this.fontSizeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fontSizeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fontSizeComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontSizeComboBox.FormattingEnabled = true;
            this.fontSizeComboBox.IntegralHeight = false;
            this.fontSizeComboBox.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
            this.fontSizeComboBox.Location = new System.Drawing.Point(497, 8);
            this.fontSizeComboBox.MaxDropDownItems = 20;
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            this.fontSizeComboBox.Size = new System.Drawing.Size(64, 23);
            this.fontSizeComboBox.TabIndex = 1;
            this.fontSizeComboBox.Tag = "FontFamily";
            // 
            // fontFamilyPanel
            // 
            this.fontFamilyPanel.BackColor = System.Drawing.Color.Transparent;
            this.fontFamilyPanel.Controls.Add(this.fontFamilyComboBox);
            this.fontFamilyPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontFamilyPanel.Location = new System.Drawing.Point(12, 148);
            this.fontFamilyPanel.Name = "fontFamilyPanel";
            this.fontFamilyPanel.Size = new System.Drawing.Size(576, 40);
            this.fontFamilyPanel.TabIndex = 30;
            this.fontFamilyPanel.Tag = "FontFamily";
            // 
            // fontFamilyComboBox
            // 
            this.fontFamilyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontFamilyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fontFamilyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fontFamilyComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontFamilyComboBox.FormattingEnabled = true;
            this.fontFamilyComboBox.IntegralHeight = false;
            this.fontFamilyComboBox.Location = new System.Drawing.Point(411, 9);
            this.fontFamilyComboBox.MaxDropDownItems = 20;
            this.fontFamilyComboBox.Name = "fontFamilyComboBox";
            this.fontFamilyComboBox.Size = new System.Drawing.Size(150, 23);
            this.fontFamilyComboBox.TabIndex = 1;
            this.fontFamilyComboBox.Tag = "FontFamily";
            // 
            // containerPanel
            // 
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 33);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(576, 50);
            this.containerPanel.TabIndex = 2;
            // 
            // previewLabel
            // 
            this.previewLabel.BackColor = System.Drawing.Color.Transparent;
            this.previewLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewLabel.ForeColor = System.Drawing.Color.DimGray;
            this.previewLabel.Location = new System.Drawing.Point(12, 323);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.previewLabel.Size = new System.Drawing.Size(576, 25);
            this.previewLabel.TabIndex = 37;
            this.previewLabel.Text = "Preview";
            this.previewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VideoWindowTitleBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.previewPanel);
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.backgroundColorPanel);
            this.Controls.Add(this.fontColorPanel);
            this.Controls.Add(this.fontSizePanel);
            this.Controls.Add(this.fontFamilyPanel);
            this.Controls.Add(this.fontSettingLabel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.keepLabel);
            this.DoubleBuffered = true;
            this.Name = "VideoWindowTitleBar";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(600, 444);
            this.previewPanel.ResumeLayout(false);
            this.backgroundColorPanel.ResumeLayout(false);
            this.fontColorPanel.ResumeLayout(false);
            this.fontSizePanel.ResumeLayout(false);
            this.fontFamilyPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.Label keepLabel;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Label infoLabel;
        private PanelBase.DoubleBufferPanel fontFamilyPanel;
        private System.Windows.Forms.ComboBox fontFamilyComboBox;
        private System.Windows.Forms.Label fontSettingLabel;
        private PanelBase.DoubleBufferPanel fontSizePanel;
        private System.Windows.Forms.ComboBox fontSizeComboBox;
        private PanelBase.DoubleBufferPanel fontColorPanel;
        private PanelBase.DoubleBufferPanel backgroundColorPanel;
        private System.Windows.Forms.ColorDialog fontColorDialog;
        private System.Windows.Forms.ColorDialog backgroundDialog;
        private System.Windows.Forms.Button backgroundColorButton;
        private System.Windows.Forms.Button fontcolorButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Label previewBar;
        private System.Windows.Forms.Label previewLabel;

    }
}
