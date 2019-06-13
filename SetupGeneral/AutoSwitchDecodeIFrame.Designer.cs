namespace SetupGeneral
{
    sealed partial class AutoSwitchDecodeIFrame
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
            this.countPanel = new PanelBase.DoubleBufferPanel();
            this.countComboBox = new System.Windows.Forms.ComboBox();
            this.autoSwitchPanel = new PanelBase.DoubleBufferPanel();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.containerPanel.SuspendLayout();
            this.countPanel.SuspendLayout();
            this.autoSwitchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.countPanel);
            this.containerPanel.Controls.Add(this.autoSwitchPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 18);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 364);
            this.containerPanel.TabIndex = 1;
            // 
            // countPanel
            // 
            this.countPanel.BackColor = System.Drawing.Color.Transparent;
            this.countPanel.Controls.Add(this.countComboBox);
            this.countPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.countPanel.Location = new System.Drawing.Point(0, 40);
            this.countPanel.Name = "countPanel";
            this.countPanel.Size = new System.Drawing.Size(456, 40);
            this.countPanel.TabIndex = 29;
            this.countPanel.Tag = "SimultaneousViewingLiveStreamingCountMoreThan";
            // 
            // countComboBox
            // 
            this.countComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.countComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.countComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countComboBox.FormattingEnabled = true;
            this.countComboBox.IntegralHeight = false;
            this.countComboBox.Location = new System.Drawing.Point(377, 8);
            this.countComboBox.MaxDropDownItems = 20;
            this.countComboBox.Name = "countComboBox";
            this.countComboBox.Size = new System.Drawing.Size(64, 23);
            this.countComboBox.TabIndex = 1;
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
            // AutoSwitchLiveVideoStream
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.DoubleBuffered = true;
            this.Name = "AutoSwitchLiveVideoStream";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.containerPanel.ResumeLayout(false);
            this.countPanel.ResumeLayout(false);
            this.autoSwitchPanel.ResumeLayout(false);
            this.autoSwitchPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel autoSwitchPanel;
        private PanelBase.DoubleBufferPanel countPanel;
        private System.Windows.Forms.ComboBox countComboBox;
        private System.Windows.Forms.CheckBox enabledCheckBox;



    }
}
