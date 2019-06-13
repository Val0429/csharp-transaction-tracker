namespace SetupDevice
{
    sealed partial class MultiStreamingControl
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
            this.lowProfilePanel = new PanelBase.DoubleBufferPanel();
            this.lowProfileComboBox = new System.Windows.Forms.ComboBox();
            this.mediumProfilePanel = new PanelBase.DoubleBufferPanel();
            this.mediumProfileComboBox = new System.Windows.Forms.ComboBox();
            this.highProfilePanel = new PanelBase.DoubleBufferPanel();
            this.highProfileComboBox = new System.Windows.Forms.ComboBox();
            this.lowProfilePanel.SuspendLayout();
            this.mediumProfilePanel.SuspendLayout();
            this.highProfilePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lowProfilePanel
            // 
            this.lowProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.lowProfilePanel.Controls.Add(this.lowProfileComboBox);
            this.lowProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lowProfilePanel.Location = new System.Drawing.Point(0, 105);
            this.lowProfilePanel.Name = "lowProfilePanel";
            this.lowProfilePanel.Size = new System.Drawing.Size(431, 40);
            this.lowProfilePanel.TabIndex = 11;
            this.lowProfilePanel.Tag = "LowProfile";
            // 
            // lowProfileComboBox
            // 
            this.lowProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lowProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lowProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowProfileComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowProfileComboBox.FormattingEnabled = true;
            this.lowProfileComboBox.IntegralHeight = false;
            this.lowProfileComboBox.Location = new System.Drawing.Point(235, 8);
            this.lowProfileComboBox.MaxDropDownItems = 20;
            this.lowProfileComboBox.Name = "lowProfileComboBox";
            this.lowProfileComboBox.Size = new System.Drawing.Size(181, 23);
            this.lowProfileComboBox.TabIndex = 2;
            // 
            // mediumProfilePanel
            // 
            this.mediumProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.mediumProfilePanel.Controls.Add(this.mediumProfileComboBox);
            this.mediumProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediumProfilePanel.Location = new System.Drawing.Point(0, 65);
            this.mediumProfilePanel.Name = "mediumProfilePanel";
            this.mediumProfilePanel.Size = new System.Drawing.Size(431, 40);
            this.mediumProfilePanel.TabIndex = 10;
            this.mediumProfilePanel.Tag = "MediumProfile";
            // 
            // mediumProfileComboBox
            // 
            this.mediumProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mediumProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mediumProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mediumProfileComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mediumProfileComboBox.FormattingEnabled = true;
            this.mediumProfileComboBox.IntegralHeight = false;
            this.mediumProfileComboBox.Location = new System.Drawing.Point(235, 8);
            this.mediumProfileComboBox.MaxDropDownItems = 20;
            this.mediumProfileComboBox.Name = "mediumProfileComboBox";
            this.mediumProfileComboBox.Size = new System.Drawing.Size(181, 23);
            this.mediumProfileComboBox.TabIndex = 2;
            // 
            // highProfilePanel
            // 
            this.highProfilePanel.BackColor = System.Drawing.Color.Transparent;
            this.highProfilePanel.Controls.Add(this.highProfileComboBox);
            this.highProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.highProfilePanel.Location = new System.Drawing.Point(0, 25);
            this.highProfilePanel.Name = "highProfilePanel";
            this.highProfilePanel.Size = new System.Drawing.Size(431, 40);
            this.highProfilePanel.TabIndex = 29;
            this.highProfilePanel.Tag = "HighProfile";
            // 
            // highProfileComboBox
            // 
            this.highProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.highProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.highProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highProfileComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highProfileComboBox.FormattingEnabled = true;
            this.highProfileComboBox.IntegralHeight = false;
            this.highProfileComboBox.Location = new System.Drawing.Point(235, 8);
            this.highProfileComboBox.MaxDropDownItems = 20;
            this.highProfileComboBox.Name = "highProfileComboBox";
            this.highProfileComboBox.Size = new System.Drawing.Size(181, 23);
            this.highProfileComboBox.TabIndex = 2;
            // 
            // MultiStreamingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lowProfilePanel);
            this.Controls.Add(this.mediumProfilePanel);
            this.Controls.Add(this.highProfilePanel);
            this.Name = "MultiStreamingControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(431, 588);
            this.lowProfilePanel.ResumeLayout(false);
            this.mediumProfilePanel.ResumeLayout(false);
            this.highProfilePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel mediumProfilePanel;
        private System.Windows.Forms.ComboBox mediumProfileComboBox;
        private PanelBase.DoubleBufferPanel lowProfilePanel;
        private System.Windows.Forms.ComboBox lowProfileComboBox;
        private PanelBase.DoubleBufferPanel highProfilePanel;
        private System.Windows.Forms.ComboBox highProfileComboBox;
    }
}
