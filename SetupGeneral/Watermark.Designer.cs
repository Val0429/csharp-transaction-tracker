namespace SetupGeneral
{
    partial class Watermark
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.previewPanel = new System.Windows.Forms.Panel();
            this.previewBar = new System.Windows.Forms.Label();
            this.previewLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundColorPanel = new PanelBase.DoubleBufferPanel();
            this.backgroundColorButton = new System.Windows.Forms.Button();
            this.fontColorPanel = new PanelBase.DoubleBufferPanel();
            this.fontcolorButton = new System.Windows.Forms.Button();
            this.fontSizePanel = new PanelBase.DoubleBufferPanel();
            this.fontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.fontFamilyPanel = new PanelBase.DoubleBufferPanel();
            this.fontFamilyComboBox = new System.Windows.Forms.ComboBox();
            this.fontSettingLabel = new System.Windows.Forms.Label();
            this.fontColorDialog = new System.Windows.Forms.ColorDialog();
            this.fontTextPanel = new PanelBase.DoubleBufferPanel();
            this.textBox = new System.Windows.Forms.TextBox();
            this.previewPanel.SuspendLayout();
            this.backgroundColorPanel.SuspendLayout();
            this.fontColorPanel.SuspendLayout();
            this.fontSizePanel.SuspendLayout();
            this.fontFamilyPanel.SuspendLayout();
            this.fontTextPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewPanel
            // 
            this.previewPanel.Controls.Add(this.previewBar);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewPanel.Location = new System.Drawing.Point(0, 255);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(749, 132);
            this.previewPanel.TabIndex = 44;
            // 
            // previewBar
            // 
            this.previewBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewBar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.previewBar.Location = new System.Drawing.Point(0, 0);
            this.previewBar.Name = "previewBar";
            this.previewBar.Size = new System.Drawing.Size(749, 132);
            this.previewBar.TabIndex = 1;
            this.previewBar.Text = "label2";
            // 
            // previewLabel
            // 
            this.previewLabel.BackColor = System.Drawing.Color.Transparent;
            this.previewLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewLabel.ForeColor = System.Drawing.Color.DimGray;
            this.previewLabel.Location = new System.Drawing.Point(0, 230);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.previewLabel.Size = new System.Drawing.Size(749, 25);
            this.previewLabel.TabIndex = 45;
            this.previewLabel.Text = "Preview";
            this.previewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(749, 15);
            this.label1.TabIndex = 43;
            // 
            // backgroundColorPanel
            // 
            this.backgroundColorPanel.BackColor = System.Drawing.Color.Transparent;
            this.backgroundColorPanel.Controls.Add(this.backgroundColorButton);
            this.backgroundColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.backgroundColorPanel.Location = new System.Drawing.Point(0, 175);
            this.backgroundColorPanel.Name = "backgroundColorPanel";
            this.backgroundColorPanel.Size = new System.Drawing.Size(749, 40);
            this.backgroundColorPanel.TabIndex = 42;
            this.backgroundColorPanel.Tag = "BackgroundColor";
            // 
            // backgroundColorButton
            // 
            this.backgroundColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundColorButton.BackColor = System.Drawing.Color.Transparent;
            this.backgroundColorButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundColorButton.Location = new System.Drawing.Point(584, 7);
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
            this.fontColorPanel.Location = new System.Drawing.Point(0, 95);
            this.fontColorPanel.Name = "fontColorPanel";
            this.fontColorPanel.Size = new System.Drawing.Size(749, 40);
            this.fontColorPanel.TabIndex = 41;
            this.fontColorPanel.Tag = "FontColor";
            // 
            // fontcolorButton
            // 
            this.fontcolorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontcolorButton.BackColor = System.Drawing.Color.Transparent;
            this.fontcolorButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fontcolorButton.Location = new System.Drawing.Point(584, 7);
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
            this.fontSizePanel.Location = new System.Drawing.Point(0, 55);
            this.fontSizePanel.Name = "fontSizePanel";
            this.fontSizePanel.Size = new System.Drawing.Size(749, 40);
            this.fontSizePanel.TabIndex = 40;
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
            this.fontSizeComboBox.Location = new System.Drawing.Point(670, 8);
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
            this.fontFamilyPanel.Location = new System.Drawing.Point(0, 15);
            this.fontFamilyPanel.Name = "fontFamilyPanel";
            this.fontFamilyPanel.Size = new System.Drawing.Size(749, 40);
            this.fontFamilyPanel.TabIndex = 38;
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
            this.fontFamilyComboBox.Location = new System.Drawing.Point(584, 9);
            this.fontFamilyComboBox.MaxDropDownItems = 20;
            this.fontFamilyComboBox.Name = "fontFamilyComboBox";
            this.fontFamilyComboBox.Size = new System.Drawing.Size(150, 23);
            this.fontFamilyComboBox.TabIndex = 1;
            this.fontFamilyComboBox.Tag = "FontFamily";
            // 
            // fontSettingLabel
            // 
            this.fontSettingLabel.BackColor = System.Drawing.Color.Transparent;
            this.fontSettingLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontSettingLabel.Location = new System.Drawing.Point(0, 0);
            this.fontSettingLabel.Name = "fontSettingLabel";
            this.fontSettingLabel.Size = new System.Drawing.Size(749, 15);
            this.fontSettingLabel.TabIndex = 39;
            // 
            // fontTextPanel
            // 
            this.fontTextPanel.BackColor = System.Drawing.Color.Transparent;
            this.fontTextPanel.Controls.Add(this.textBox);
            this.fontTextPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontTextPanel.Location = new System.Drawing.Point(0, 135);
            this.fontTextPanel.Name = "fontTextPanel";
            this.fontTextPanel.Size = new System.Drawing.Size(749, 40);
            this.fontTextPanel.TabIndex = 46;
            this.fontTextPanel.Tag = "Text";
            // 
            // textBox
            // 
            this.textBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox.Location = new System.Drawing.Point(584, 6);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(150, 22);
            this.textBox.TabIndex = 0;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // Watermark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewPanel);
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.backgroundColorPanel);
            this.Controls.Add(this.fontTextPanel);
            this.Controls.Add(this.fontColorPanel);
            this.Controls.Add(this.fontSizePanel);
            this.Controls.Add(this.fontFamilyPanel);
            this.Controls.Add(this.fontSettingLabel);
            this.Name = "Watermark";
            this.Size = new System.Drawing.Size(749, 418);
            this.previewPanel.ResumeLayout(false);
            this.backgroundColorPanel.ResumeLayout(false);
            this.fontColorPanel.ResumeLayout(false);
            this.fontSizePanel.ResumeLayout(false);
            this.fontFamilyPanel.ResumeLayout(false);
            this.fontTextPanel.ResumeLayout(false);
            this.fontTextPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Label previewBar;
        private System.Windows.Forms.Label previewLabel;
        private System.Windows.Forms.Label label1;
        private PanelBase.DoubleBufferPanel backgroundColorPanel;
        private System.Windows.Forms.Button backgroundColorButton;
        private PanelBase.DoubleBufferPanel fontColorPanel;
        private System.Windows.Forms.Button fontcolorButton;
        private PanelBase.DoubleBufferPanel fontSizePanel;
        private System.Windows.Forms.ComboBox fontSizeComboBox;
        private PanelBase.DoubleBufferPanel fontFamilyPanel;
        private System.Windows.Forms.ComboBox fontFamilyComboBox;
        private System.Windows.Forms.Label fontSettingLabel;
        private System.Windows.Forms.ColorDialog fontColorDialog;
        private PanelBase.DoubleBufferPanel fontTextPanel;
        private System.Windows.Forms.TextBox textBox;

    }
}
