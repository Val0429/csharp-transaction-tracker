namespace SetupServer
{
    partial class DeviceKeepDaysRecording
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.applyAllPanel = new PanelBase.DoubleBufferPanel();
            this.setButton = new System.Windows.Forms.Button();
            this.defaultDaysComboBox = new System.Windows.Forms.ComboBox();
            this.enableKeepDaysPanel = new PanelBase.DoubleBufferPanel();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.applyAllPanel.SuspendLayout();
            this.enableKeepDaysPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.BackColor = System.Drawing.Color.Transparent;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.infoLabel.Location = new System.Drawing.Point(12, 98);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 5);
            this.infoLabel.Size = new System.Drawing.Size(456, 25);
            this.infoLabel.TabIndex = 32;
            this.infoLabel.Text = "Patrol interval should between 5 secs to 1 hour";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 123);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 259);
            this.containerPanel.TabIndex = 1;
            // 
            // applyAllPanel
            // 
            this.applyAllPanel.BackColor = System.Drawing.Color.Transparent;
            this.applyAllPanel.Controls.Add(this.setButton);
            this.applyAllPanel.Controls.Add(this.defaultDaysComboBox);
            this.applyAllPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.applyAllPanel.Location = new System.Drawing.Point(12, 58);
            this.applyAllPanel.Name = "applyAllPanel";
            this.applyAllPanel.Size = new System.Drawing.Size(456, 40);
            this.applyAllPanel.TabIndex = 31;
            this.applyAllPanel.Tag = "KeepDaysOfNewlyAddedDevices";
            // 
            // setButton
            // 
            this.setButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setButton.BackColor = System.Drawing.Color.Transparent;
            this.setButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setButton.Location = new System.Drawing.Point(281, 7);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(150, 27);
            this.setButton.TabIndex = 2;
            this.setButton.Text = "Apply to all devices";
            this.setButton.UseVisualStyleBackColor = false;
            // 
            // defaultDaysComboBox
            // 
            this.defaultDaysComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultDaysComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.defaultDaysComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultDaysComboBox.FormattingEnabled = true;
            this.defaultDaysComboBox.IntegralHeight = false;
            this.defaultDaysComboBox.Location = new System.Drawing.Point(210, 9);
            this.defaultDaysComboBox.MaxDropDownItems = 20;
            this.defaultDaysComboBox.Name = "defaultDaysComboBox";
            this.defaultDaysComboBox.Size = new System.Drawing.Size(64, 23);
            this.defaultDaysComboBox.TabIndex = 1;
            // 
            // enableKeepDaysPanel
            // 
            this.enableKeepDaysPanel.BackColor = System.Drawing.Color.Transparent;
            this.enableKeepDaysPanel.Controls.Add(this.enabledCheckBox);
            this.enableKeepDaysPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.enableKeepDaysPanel.Location = new System.Drawing.Point(12, 18);
            this.enableKeepDaysPanel.Name = "enableKeepDaysPanel";
            this.enableKeepDaysPanel.Size = new System.Drawing.Size(456, 40);
            this.enableKeepDaysPanel.TabIndex = 30;
            this.enableKeepDaysPanel.Tag = "EnabledKeepDays";
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
            // DeviceKeepDaysRecording
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.applyAllPanel);
            this.Controls.Add(this.enableKeepDaysPanel);
            this.Name = "DeviceKeepDaysRecording";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.applyAllPanel.ResumeLayout(false);
            this.enableKeepDaysPanel.ResumeLayout(false);
            this.enableKeepDaysPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel applyAllPanel;
        private System.Windows.Forms.ComboBox defaultDaysComboBox;
        private PanelBase.DoubleBufferPanel enableKeepDaysPanel;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button setButton;


    }
}
