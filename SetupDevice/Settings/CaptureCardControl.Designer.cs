namespace SetupDevice
{
    sealed partial class CaptureCardControl
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
            this.gainPanel = new PanelBase.DoubleBufferPanel();
            this.colorEnablePanel = new PanelBase.DoubleBufferPanel();
            this.colorEnableComboBox = new System.Windows.Forms.ComboBox();
            this.gammaPanel = new PanelBase.DoubleBufferPanel();
            this.gammaComboBox = new System.Windows.Forms.ComboBox();
            this.sharpnessPanel = new PanelBase.DoubleBufferPanel();
            this.sharpnessComboBox = new System.Windows.Forms.ComboBox();
            this.saturationPanel = new PanelBase.DoubleBufferPanel();
            this.saturationComboBox = new System.Windows.Forms.ComboBox();
            this.huePanel = new PanelBase.DoubleBufferPanel();
            this.hueComboBox = new System.Windows.Forms.ComboBox();
            this.contrastPanel = new PanelBase.DoubleBufferPanel();
            this.contrastComboBox = new System.Windows.Forms.ComboBox();
            this.brightnessPanel = new PanelBase.DoubleBufferPanel();
            this.brightnessComboBox = new System.Windows.Forms.ComboBox();
            this.backlightCompensationPanel = new PanelBase.DoubleBufferPanel();
            this.whiteBalancePanel = new PanelBase.DoubleBufferPanel();
            this.whiteBalanceComboBox = new System.Windows.Forms.ComboBox();
            this.backlightCompensationComboBox = new System.Windows.Forms.ComboBox();
            this.gainComboBox = new System.Windows.Forms.ComboBox();
            this.gainPanel.SuspendLayout();
            this.colorEnablePanel.SuspendLayout();
            this.gammaPanel.SuspendLayout();
            this.sharpnessPanel.SuspendLayout();
            this.saturationPanel.SuspendLayout();
            this.huePanel.SuspendLayout();
            this.contrastPanel.SuspendLayout();
            this.brightnessPanel.SuspendLayout();
            this.backlightCompensationPanel.SuspendLayout();
            this.whiteBalancePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gainPanel
            // 
            this.gainPanel.BackColor = System.Drawing.Color.Transparent;
            this.gainPanel.Controls.Add(this.gainComboBox);
            this.gainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.gainPanel.Location = new System.Drawing.Point(0, 385);
            this.gainPanel.Name = "gainPanel";
            this.gainPanel.Size = new System.Drawing.Size(465, 40);
            this.gainPanel.TabIndex = 18;
            this.gainPanel.Tag = "Gain";
            // 
            // colorEnablePanel
            // 
            this.colorEnablePanel.BackColor = System.Drawing.Color.Transparent;
            this.colorEnablePanel.Controls.Add(this.colorEnableComboBox);
            this.colorEnablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorEnablePanel.Location = new System.Drawing.Point(0, 265);
            this.colorEnablePanel.Name = "colorEnablePanel";
            this.colorEnablePanel.Size = new System.Drawing.Size(465, 40);
            this.colorEnablePanel.TabIndex = 13;
            this.colorEnablePanel.Tag = "ColorEnable";
            // 
            // colorEnableComboBox
            // 
            this.colorEnableComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colorEnableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorEnableComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.colorEnableComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorEnableComboBox.FormattingEnabled = true;
            this.colorEnableComboBox.IntegralHeight = false;
            this.colorEnableComboBox.Location = new System.Drawing.Point(269, 8);
            this.colorEnableComboBox.MaxDropDownItems = 20;
            this.colorEnableComboBox.Name = "colorEnableComboBox";
            this.colorEnableComboBox.Size = new System.Drawing.Size(181, 23);
            this.colorEnableComboBox.TabIndex = 3;
            // 
            // gammaPanel
            // 
            this.gammaPanel.BackColor = System.Drawing.Color.Transparent;
            this.gammaPanel.Controls.Add(this.gammaComboBox);
            this.gammaPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.gammaPanel.Location = new System.Drawing.Point(0, 225);
            this.gammaPanel.Name = "gammaPanel";
            this.gammaPanel.Size = new System.Drawing.Size(465, 40);
            this.gammaPanel.TabIndex = 11;
            this.gammaPanel.Tag = "Gamma";
            // 
            // gammaComboBox
            // 
            this.gammaComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gammaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gammaComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gammaComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gammaComboBox.FormattingEnabled = true;
            this.gammaComboBox.IntegralHeight = false;
            this.gammaComboBox.Location = new System.Drawing.Point(269, 8);
            this.gammaComboBox.MaxDropDownItems = 20;
            this.gammaComboBox.Name = "gammaComboBox";
            this.gammaComboBox.Size = new System.Drawing.Size(181, 23);
            this.gammaComboBox.TabIndex = 3;
            // 
            // sharpnessPanel
            // 
            this.sharpnessPanel.BackColor = System.Drawing.Color.Transparent;
            this.sharpnessPanel.Controls.Add(this.sharpnessComboBox);
            this.sharpnessPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.sharpnessPanel.Location = new System.Drawing.Point(0, 185);
            this.sharpnessPanel.Name = "sharpnessPanel";
            this.sharpnessPanel.Size = new System.Drawing.Size(465, 40);
            this.sharpnessPanel.TabIndex = 12;
            this.sharpnessPanel.Tag = "Sharpness";
            // 
            // sharpnessComboBox
            // 
            this.sharpnessComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sharpnessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sharpnessComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sharpnessComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sharpnessComboBox.FormattingEnabled = true;
            this.sharpnessComboBox.IntegralHeight = false;
            this.sharpnessComboBox.Location = new System.Drawing.Point(269, 8);
            this.sharpnessComboBox.MaxDropDownItems = 20;
            this.sharpnessComboBox.Name = "sharpnessComboBox";
            this.sharpnessComboBox.Size = new System.Drawing.Size(181, 23);
            this.sharpnessComboBox.TabIndex = 2;
            // 
            // saturationPanel
            // 
            this.saturationPanel.BackColor = System.Drawing.Color.Transparent;
            this.saturationPanel.Controls.Add(this.saturationComboBox);
            this.saturationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.saturationPanel.Location = new System.Drawing.Point(0, 145);
            this.saturationPanel.Name = "saturationPanel";
            this.saturationPanel.Size = new System.Drawing.Size(465, 40);
            this.saturationPanel.TabIndex = 10;
            this.saturationPanel.Tag = "Saturation";
            // 
            // saturationComboBox
            // 
            this.saturationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saturationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.saturationComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saturationComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saturationComboBox.FormattingEnabled = true;
            this.saturationComboBox.IntegralHeight = false;
            this.saturationComboBox.Location = new System.Drawing.Point(269, 8);
            this.saturationComboBox.MaxDropDownItems = 20;
            this.saturationComboBox.Name = "saturationComboBox";
            this.saturationComboBox.Size = new System.Drawing.Size(181, 23);
            this.saturationComboBox.TabIndex = 2;
            // 
            // huePanel
            // 
            this.huePanel.BackColor = System.Drawing.Color.Transparent;
            this.huePanel.Controls.Add(this.hueComboBox);
            this.huePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.huePanel.Location = new System.Drawing.Point(0, 105);
            this.huePanel.Name = "huePanel";
            this.huePanel.Size = new System.Drawing.Size(465, 40);
            this.huePanel.TabIndex = 9;
            this.huePanel.Tag = "Hue";
            // 
            // hueComboBox
            // 
            this.hueComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hueComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hueComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hueComboBox.FormattingEnabled = true;
            this.hueComboBox.IntegralHeight = false;
            this.hueComboBox.Location = new System.Drawing.Point(269, 8);
            this.hueComboBox.MaxDropDownItems = 20;
            this.hueComboBox.Name = "hueComboBox";
            this.hueComboBox.Size = new System.Drawing.Size(181, 23);
            this.hueComboBox.TabIndex = 3;
            // 
            // contrastPanel
            // 
            this.contrastPanel.BackColor = System.Drawing.Color.Transparent;
            this.contrastPanel.Controls.Add(this.contrastComboBox);
            this.contrastPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.contrastPanel.Location = new System.Drawing.Point(0, 65);
            this.contrastPanel.Name = "contrastPanel";
            this.contrastPanel.Size = new System.Drawing.Size(465, 40);
            this.contrastPanel.TabIndex = 19;
            this.contrastPanel.Tag = "Contrast";
            // 
            // contrastComboBox
            // 
            this.contrastComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.contrastComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contrastComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.contrastComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contrastComboBox.FormattingEnabled = true;
            this.contrastComboBox.IntegralHeight = false;
            this.contrastComboBox.Location = new System.Drawing.Point(269, 8);
            this.contrastComboBox.MaxDropDownItems = 20;
            this.contrastComboBox.Name = "contrastComboBox";
            this.contrastComboBox.Size = new System.Drawing.Size(181, 23);
            this.contrastComboBox.TabIndex = 2;
            // 
            // brightnessPanel
            // 
            this.brightnessPanel.BackColor = System.Drawing.Color.Transparent;
            this.brightnessPanel.Controls.Add(this.brightnessComboBox);
            this.brightnessPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.brightnessPanel.Location = new System.Drawing.Point(0, 25);
            this.brightnessPanel.Name = "brightnessPanel";
            this.brightnessPanel.Size = new System.Drawing.Size(465, 40);
            this.brightnessPanel.TabIndex = 20;
            this.brightnessPanel.Tag = "Brightness";
            // 
            // brightnessComboBox
            // 
            this.brightnessComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brightnessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brightnessComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.brightnessComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brightnessComboBox.FormattingEnabled = true;
            this.brightnessComboBox.IntegralHeight = false;
            this.brightnessComboBox.Location = new System.Drawing.Point(269, 8);
            this.brightnessComboBox.MaxDropDownItems = 20;
            this.brightnessComboBox.Name = "brightnessComboBox";
            this.brightnessComboBox.Size = new System.Drawing.Size(181, 23);
            this.brightnessComboBox.TabIndex = 2;
            // 
            // backlightCompensationPanel
            // 
            this.backlightCompensationPanel.BackColor = System.Drawing.Color.Transparent;
            this.backlightCompensationPanel.Controls.Add(this.backlightCompensationComboBox);
            this.backlightCompensationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.backlightCompensationPanel.Location = new System.Drawing.Point(0, 345);
            this.backlightCompensationPanel.Name = "backlightCompensationPanel";
            this.backlightCompensationPanel.Size = new System.Drawing.Size(465, 40);
            this.backlightCompensationPanel.TabIndex = 17;
            this.backlightCompensationPanel.Tag = "BacklightCompensation";
            // 
            // whiteBalancePanel
            // 
            this.whiteBalancePanel.BackColor = System.Drawing.Color.Transparent;
            this.whiteBalancePanel.Controls.Add(this.whiteBalanceComboBox);
            this.whiteBalancePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.whiteBalancePanel.Location = new System.Drawing.Point(0, 305);
            this.whiteBalancePanel.Name = "whiteBalancePanel";
            this.whiteBalancePanel.Size = new System.Drawing.Size(465, 40);
            this.whiteBalancePanel.TabIndex = 16;
            this.whiteBalancePanel.Tag = "WhiteBalance";
            // 
            // whiteBalanceComboBox
            // 
            this.whiteBalanceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.whiteBalanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whiteBalanceComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.whiteBalanceComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteBalanceComboBox.FormattingEnabled = true;
            this.whiteBalanceComboBox.IntegralHeight = false;
            this.whiteBalanceComboBox.Location = new System.Drawing.Point(269, 8);
            this.whiteBalanceComboBox.MaxDropDownItems = 20;
            this.whiteBalanceComboBox.Name = "whiteBalanceComboBox";
            this.whiteBalanceComboBox.Size = new System.Drawing.Size(181, 23);
            this.whiteBalanceComboBox.TabIndex = 4;
            // 
            // backlightCompensationComboBox
            // 
            this.backlightCompensationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backlightCompensationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backlightCompensationComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.backlightCompensationComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backlightCompensationComboBox.FormattingEnabled = true;
            this.backlightCompensationComboBox.IntegralHeight = false;
            this.backlightCompensationComboBox.Location = new System.Drawing.Point(269, 8);
            this.backlightCompensationComboBox.MaxDropDownItems = 20;
            this.backlightCompensationComboBox.Name = "backlightCompensationComboBox";
            this.backlightCompensationComboBox.Size = new System.Drawing.Size(181, 23);
            this.backlightCompensationComboBox.TabIndex = 5;
            // 
            // gainComboBox
            // 
            this.gainComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gainComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gainComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gainComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gainComboBox.FormattingEnabled = true;
            this.gainComboBox.IntegralHeight = false;
            this.gainComboBox.Location = new System.Drawing.Point(269, 8);
            this.gainComboBox.MaxDropDownItems = 20;
            this.gainComboBox.Name = "gainComboBox";
            this.gainComboBox.Size = new System.Drawing.Size(181, 23);
            this.gainComboBox.TabIndex = 6;
            // 
            // CaptureCardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gainPanel);
            this.Controls.Add(this.backlightCompensationPanel);
            this.Controls.Add(this.whiteBalancePanel);
            this.Controls.Add(this.colorEnablePanel);
            this.Controls.Add(this.gammaPanel);
            this.Controls.Add(this.sharpnessPanel);
            this.Controls.Add(this.saturationPanel);
            this.Controls.Add(this.huePanel);
            this.Controls.Add(this.contrastPanel);
            this.Controls.Add(this.brightnessPanel);
            this.Name = "CaptureCardControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(465, 425);
            this.gainPanel.ResumeLayout(false);
            this.colorEnablePanel.ResumeLayout(false);
            this.gammaPanel.ResumeLayout(false);
            this.sharpnessPanel.ResumeLayout(false);
            this.saturationPanel.ResumeLayout(false);
            this.huePanel.ResumeLayout(false);
            this.contrastPanel.ResumeLayout(false);
            this.brightnessPanel.ResumeLayout(false);
            this.backlightCompensationPanel.ResumeLayout(false);
            this.whiteBalancePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel colorEnablePanel;
        private System.Windows.Forms.ComboBox colorEnableComboBox;
        private PanelBase.DoubleBufferPanel sharpnessPanel;
        private System.Windows.Forms.ComboBox sharpnessComboBox;
        private PanelBase.DoubleBufferPanel gammaPanel;
        private System.Windows.Forms.ComboBox gammaComboBox;
        private PanelBase.DoubleBufferPanel saturationPanel;
        private System.Windows.Forms.ComboBox saturationComboBox;
        private PanelBase.DoubleBufferPanel huePanel;
        private System.Windows.Forms.ComboBox hueComboBox;
        private PanelBase.DoubleBufferPanel gainPanel;
        private PanelBase.DoubleBufferPanel contrastPanel;
        private System.Windows.Forms.ComboBox contrastComboBox;
        private PanelBase.DoubleBufferPanel brightnessPanel;
        private System.Windows.Forms.ComboBox brightnessComboBox;
        private PanelBase.DoubleBufferPanel backlightCompensationPanel;
        private PanelBase.DoubleBufferPanel whiteBalancePanel;
        private System.Windows.Forms.ComboBox whiteBalanceComboBox;
        private System.Windows.Forms.ComboBox gainComboBox;
        private System.Windows.Forms.ComboBox backlightCompensationComboBox;
    }
}
