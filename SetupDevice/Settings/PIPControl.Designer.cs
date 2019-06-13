namespace SetupDevice
{
    sealed partial class PIPControl
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
            this.positionPanel = new PanelBase.DoubleBufferPanel();
            this.positionComboBox = new System.Windows.Forms.ComboBox();
            this.streamPanel = new PanelBase.DoubleBufferPanel();
            this.streamComboBox = new System.Windows.Forms.ComboBox();
            this.devicePanel = new PanelBase.DoubleBufferPanel();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.positionPanel.SuspendLayout();
            this.streamPanel.SuspendLayout();
            this.devicePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // positionPanel
            // 
            this.positionPanel.BackColor = System.Drawing.Color.Transparent;
            this.positionPanel.Controls.Add(this.positionComboBox);
            this.positionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.positionPanel.Location = new System.Drawing.Point(0, 105);
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(431, 40);
            this.positionPanel.TabIndex = 11;
            this.positionPanel.Tag = "Position";
            // 
            // positionComboBox
            // 
            this.positionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.positionComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positionComboBox.FormattingEnabled = true;
            this.positionComboBox.IntegralHeight = false;
            this.positionComboBox.Location = new System.Drawing.Point(235, 8);
            this.positionComboBox.MaxDropDownItems = 20;
            this.positionComboBox.Name = "positionComboBox";
            this.positionComboBox.Size = new System.Drawing.Size(181, 23);
            this.positionComboBox.TabIndex = 2;
            // 
            // streamPanel
            // 
            this.streamPanel.BackColor = System.Drawing.Color.Transparent;
            this.streamPanel.Controls.Add(this.streamComboBox);
            this.streamPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.streamPanel.Location = new System.Drawing.Point(0, 65);
            this.streamPanel.Name = "streamPanel";
            this.streamPanel.Size = new System.Drawing.Size(431, 40);
            this.streamPanel.TabIndex = 10;
            this.streamPanel.Tag = "StreamId";
            // 
            // streamComboBox
            // 
            this.streamComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.streamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.streamComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.streamComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamComboBox.FormattingEnabled = true;
            this.streamComboBox.IntegralHeight = false;
            this.streamComboBox.Location = new System.Drawing.Point(235, 8);
            this.streamComboBox.MaxDropDownItems = 20;
            this.streamComboBox.Name = "streamComboBox";
            this.streamComboBox.Size = new System.Drawing.Size(181, 23);
            this.streamComboBox.TabIndex = 2;
            // 
            // devicePanel
            // 
            this.devicePanel.BackColor = System.Drawing.Color.Transparent;
            this.devicePanel.Controls.Add(this.deviceComboBox);
            this.devicePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.devicePanel.Location = new System.Drawing.Point(0, 25);
            this.devicePanel.Name = "devicePanel";
            this.devicePanel.Size = new System.Drawing.Size(431, 40);
            this.devicePanel.TabIndex = 29;
            this.devicePanel.Tag = "Device";
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deviceComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.IntegralHeight = false;
            this.deviceComboBox.Location = new System.Drawing.Point(235, 8);
            this.deviceComboBox.MaxDropDownItems = 20;
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(181, 23);
            this.deviceComboBox.TabIndex = 2;
            // 
            // PIPControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.positionPanel);
            this.Controls.Add(this.streamPanel);
            this.Controls.Add(this.devicePanel);
            this.Name = "PIPControl";
            this.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.Size = new System.Drawing.Size(431, 588);
            this.positionPanel.ResumeLayout(false);
            this.streamPanel.ResumeLayout(false);
            this.devicePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel streamPanel;
        private System.Windows.Forms.ComboBox streamComboBox;
        private PanelBase.DoubleBufferPanel positionPanel;
        private System.Windows.Forms.ComboBox positionComboBox;
        private PanelBase.DoubleBufferPanel devicePanel;
        private System.Windows.Forms.ComboBox deviceComboBox;
    }
}
