namespace SetupSchedule
{
    sealed partial class CopySchedulePanel
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.copyFromPanel = new PanelBase.DoubleBufferPanel();
            this.eventHandlingCheckBox = new System.Windows.Forms.CheckBox();
            this.recordCheckBox = new System.Windows.Forms.CheckBox();
            this.copyFromComboBox = new System.Windows.Forms.ComboBox();
            this.nvrComboBox = new System.Windows.Forms.ComboBox();
            this.copyFromPanel.SuspendLayout();
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
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 73);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(456, 309);
            this.containerPanel.TabIndex = 1;
            // 
            // copyFromPanel
            // 
            this.copyFromPanel.BackColor = System.Drawing.Color.Transparent;
            this.copyFromPanel.Controls.Add(this.nvrComboBox);
            this.copyFromPanel.Controls.Add(this.eventHandlingCheckBox);
            this.copyFromPanel.Controls.Add(this.recordCheckBox);
            this.copyFromPanel.Controls.Add(this.copyFromComboBox);
            this.copyFromPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.copyFromPanel.Location = new System.Drawing.Point(12, 18);
            this.copyFromPanel.Name = "copyFromPanel";
            this.copyFromPanel.Size = new System.Drawing.Size(456, 40);
            this.copyFromPanel.TabIndex = 8;
            this.copyFromPanel.Tag = "Copy From";
            // 
            // eventHandlingCheckBox
            // 
            this.eventHandlingCheckBox.AutoSize = true;
            this.eventHandlingCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventHandlingCheckBox.Location = new System.Drawing.Point(368, 13);
            this.eventHandlingCheckBox.Name = "eventHandlingCheckBox";
            this.eventHandlingCheckBox.Size = new System.Drawing.Size(109, 19);
            this.eventHandlingCheckBox.TabIndex = 5;
            this.eventHandlingCheckBox.Text = "Event Handling";
            this.eventHandlingCheckBox.UseVisualStyleBackColor = true;
            // 
            // recordCheckBox
            // 
            this.recordCheckBox.AutoSize = true;
            this.recordCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordCheckBox.Location = new System.Drawing.Point(246, 13);
            this.recordCheckBox.Name = "recordCheckBox";
            this.recordCheckBox.Size = new System.Drawing.Size(117, 19);
            this.recordCheckBox.TabIndex = 4;
            this.recordCheckBox.Text = "Video Recording";
            this.recordCheckBox.UseVisualStyleBackColor = true;
            // 
            // copyFromComboBox
            // 
            this.copyFromComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyFromComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.copyFromComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyFromComboBox.FormattingEnabled = true;
            this.copyFromComboBox.IntegralHeight = false;
            this.copyFromComboBox.Location = new System.Drawing.Point(117, 9);
            this.copyFromComboBox.Name = "copyFromComboBox";
            this.copyFromComboBox.Size = new System.Drawing.Size(120, 23);
            this.copyFromComboBox.TabIndex = 3;
            // 
            // nvrComboBox
            // 
            this.nvrComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nvrComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nvrComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nvrComboBox.FormattingEnabled = true;
            this.nvrComboBox.IntegralHeight = false;
            this.nvrComboBox.Location = new System.Drawing.Point(14, 9);
            this.nvrComboBox.Name = "nvrComboBox";
            this.nvrComboBox.Size = new System.Drawing.Size(120, 23);
            this.nvrComboBox.TabIndex = 6;
            // 
            // CopySchedulePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.copyFromPanel);
            this.Name = "CopySchedulePanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.copyFromPanel.ResumeLayout(false);
            this.copyFromPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel copyFromPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox copyFromComboBox;
        private System.Windows.Forms.CheckBox recordCheckBox;
        private System.Windows.Forms.CheckBox eventHandlingCheckBox;
        private System.Windows.Forms.ComboBox nvrComboBox;


    }
}
