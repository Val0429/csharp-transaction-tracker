namespace SetupGeneral
{
    sealed partial class AutoSwitchLiveVideoStream
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
            this.lowProfilePanel = new PanelBase.DoubleBufferPanel();
            this.lowProfileComboBox = new System.Windows.Forms.ComboBox();
            this.mediumProfilePanel = new PanelBase.DoubleBufferPanel();
            this.toLabel = new System.Windows.Forms.Label();
            this.lowLabel = new System.Windows.Forms.Label();
            this.highLabel = new System.Windows.Forms.Label();
            this.highProfilePanel = new PanelBase.DoubleBufferPanel();
            this.highProfileComboBox = new System.Windows.Forms.ComboBox();
            this.autoSwitchPanel = new PanelBase.DoubleBufferPanel();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.lowProfilePanel.SuspendLayout();
            this.mediumProfilePanel.SuspendLayout();
            this.highProfilePanel.SuspendLayout();
            this.autoSwitchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.lowProfilePanel);
            this.containerPanel.Controls.Add(this.mediumProfilePanel);
            this.containerPanel.Controls.Add(this.highProfilePanel);
            this.containerPanel.Controls.Add(this.autoSwitchPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 162);
            this.containerPanel.TabIndex = 1;
            // 
            // lowProfilePanel
            // 
            this.lowProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.lowProfilePanel.Controls.Add(this.lowProfileComboBox);
            this.lowProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lowProfilePanel.Location = new System.Drawing.Point(0, 120);
            this.lowProfilePanel.Name = "lowProfilePanel";
            this.lowProfilePanel.Size = new System.Drawing.Size(456, 40);
            this.lowProfilePanel.TabIndex = 31;
            this.lowProfilePanel.Tag = "LowProfileStartMoreThen";
            // 
            // lowProfileComboBox
            // 
            this.lowProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lowProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lowProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowProfileComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowProfileComboBox.FormattingEnabled = true;
            this.lowProfileComboBox.IntegralHeight = false;
            this.lowProfileComboBox.Location = new System.Drawing.Point(377, 8);
            this.lowProfileComboBox.MaxDropDownItems = 20;
            this.lowProfileComboBox.Name = "lowProfileComboBox";
            this.lowProfileComboBox.Size = new System.Drawing.Size(64, 23);
            this.lowProfileComboBox.TabIndex = 1;
            // 
            // mediumProfilePanel
            // 
            this.mediumProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.mediumProfilePanel.Controls.Add(this.toLabel);
            this.mediumProfilePanel.Controls.Add(this.lowLabel);
            this.mediumProfilePanel.Controls.Add(this.highLabel);
            this.mediumProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediumProfilePanel.Location = new System.Drawing.Point(0, 80);
            this.mediumProfilePanel.Name = "mediumProfilePanel";
            this.mediumProfilePanel.Size = new System.Drawing.Size(456, 40);
            this.mediumProfilePanel.TabIndex = 30;
            this.mediumProfilePanel.Tag = "MediumProfileStartMoreThen";
            // 
            // toLabel
            // 
            this.toLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toLabel.AutoSize = true;
            this.toLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.toLabel.Location = new System.Drawing.Point(401, 15);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(14, 15);
            this.toLabel.TabIndex = 2;
            this.toLabel.Text = "~";
            // 
            // lowLabel
            // 
            this.lowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lowLabel.AutoSize = true;
            this.lowLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.lowLabel.Location = new System.Drawing.Point(420, 15);
            this.lowLabel.Name = "lowLabel";
            this.lowLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lowLabel.Size = new System.Drawing.Size(21, 15);
            this.lowLabel.TabIndex = 1;
            this.lowLabel.Text = "64";
            // 
            // highLabel
            // 
            this.highLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.highLabel.AutoSize = true;
            this.highLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.highLabel.Location = new System.Drawing.Point(377, 15);
            this.highLabel.Name = "highLabel";
            this.highLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.highLabel.Size = new System.Drawing.Size(21, 15);
            this.highLabel.TabIndex = 0;
            this.highLabel.Text = "64";
            // 
            // highProfilePanel
            // 
            this.highProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.highProfilePanel.Controls.Add(this.highProfileComboBox);
            this.highProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.highProfilePanel.Location = new System.Drawing.Point(0, 40);
            this.highProfilePanel.Name = "highProfilePanel";
            this.highProfilePanel.Size = new System.Drawing.Size(456, 40);
            this.highProfilePanel.TabIndex = 29;
            this.highProfilePanel.Tag = "HighProfileStartLessThen";
            // 
            // highProfileComboBox
            // 
            this.highProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.highProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.highProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highProfileComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highProfileComboBox.FormattingEnabled = true;
            this.highProfileComboBox.IntegralHeight = false;
            this.highProfileComboBox.Location = new System.Drawing.Point(377, 8);
            this.highProfileComboBox.MaxDropDownItems = 20;
            this.highProfileComboBox.Name = "highProfileComboBox";
            this.highProfileComboBox.Size = new System.Drawing.Size(64, 23);
            this.highProfileComboBox.TabIndex = 1;
            // 
            // autoSwitchPanel
            // 
            this.autoSwitchPanel.BackColor = System.Drawing.Color.Transparent;
            this.autoSwitchPanel.Controls.Add(this.enabledCheckBox);
            this.autoSwitchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.autoSwitchPanel.Location = new System.Drawing.Point(0, 0);
            this.autoSwitchPanel.Name = "autoSwitchPanel";
            this.autoSwitchPanel.Size = new System.Drawing.Size(456, 40);
            this.autoSwitchPanel.TabIndex = 25;
            this.autoSwitchPanel.Tag = "AutoSwitch";
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.enabledCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enabledCheckBox.Location = new System.Drawing.Point(360, 0);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.enabledCheckBox.Size = new System.Drawing.Size(96, 40);
            this.enabledCheckBox.TabIndex = 1;
            this.enabledCheckBox.Text = "Enabled";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(12, 180);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(456, 25);
            this.infoLabel.TabIndex = 33;
            this.infoLabel.Text = "High profile streaming count must be less then low profile streaming count.";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AutoSwitchLiveVideoStream
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.containerPanel);
            this.DoubleBuffered = true;
            this.Name = "AutoSwitchLiveVideoStream";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.ResumeLayout(false);
            this.lowProfilePanel.ResumeLayout(false);
            this.mediumProfilePanel.ResumeLayout(false);
            this.mediumProfilePanel.PerformLayout();
            this.highProfilePanel.ResumeLayout(false);
            this.autoSwitchPanel.ResumeLayout(false);
            this.autoSwitchPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel autoSwitchPanel;
        private PanelBase.DoubleBufferPanel highProfilePanel;
        private System.Windows.Forms.ComboBox highProfileComboBox;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private PanelBase.DoubleBufferPanel lowProfilePanel;
        private System.Windows.Forms.ComboBox lowProfileComboBox;
        private PanelBase.DoubleBufferPanel mediumProfilePanel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.Label lowLabel;
        private System.Windows.Forms.Label highLabel;
        private System.Windows.Forms.Label infoLabel;



    }
}
