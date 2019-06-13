using PanelBase;

namespace SetupJoystick
{
    partial class Setup
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
            this.contentPanel = new System.Windows.Forms.Panel();
            this.enableAxisJoystickDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.enableAxisJoystickCheckBox = new System.Windows.Forms.CheckBox();
            this.enableJoystickDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.enableJoystickCheckBox = new System.Windows.Forms.CheckBox();
            this.contentPanel.SuspendLayout();
            this.enableAxisJoystickDoubleBufferPanel.SuspendLayout();
            this.enableJoystickDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Controls.Add(this.enableAxisJoystickDoubleBufferPanel);
            this.contentPanel.Controls.Add(this.enableJoystickDoubleBufferPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(3, 3);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.contentPanel.Size = new System.Drawing.Size(480, 384);
            this.contentPanel.TabIndex = 6;
            // 
            // enableAxisJoystickDoubleBufferPanel
            // 
            this.enableAxisJoystickDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.enableAxisJoystickDoubleBufferPanel.Controls.Add(this.enableAxisJoystickCheckBox);
            this.enableAxisJoystickDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enableAxisJoystickDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.enableAxisJoystickDoubleBufferPanel.Location = new System.Drawing.Point(12, 58);
            this.enableAxisJoystickDoubleBufferPanel.Name = "enableAxisJoystickDoubleBufferPanel";
            this.enableAxisJoystickDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.enableAxisJoystickDoubleBufferPanel.TabIndex = 24;
            this.enableAxisJoystickDoubleBufferPanel.Tag = "EnableAxisJoystick";
            // 
            // enableAxisJoystickCheckBox
            // 
            this.enableAxisJoystickCheckBox.AutoSize = true;
            this.enableAxisJoystickCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.enableAxisJoystickCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableAxisJoystickCheckBox.Location = new System.Drawing.Point(360, 0);
            this.enableAxisJoystickCheckBox.Name = "enableAxisJoystickCheckBox";
            this.enableAxisJoystickCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.enableAxisJoystickCheckBox.Size = new System.Drawing.Size(96, 40);
            this.enableAxisJoystickCheckBox.TabIndex = 1;
            this.enableAxisJoystickCheckBox.Text = "Enabled";
            this.enableAxisJoystickCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableJoystickDoubleBufferPanel
            // 
            this.enableJoystickDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.enableJoystickDoubleBufferPanel.Controls.Add(this.enableJoystickCheckBox);
            this.enableJoystickDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enableJoystickDoubleBufferPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.enableJoystickDoubleBufferPanel.Location = new System.Drawing.Point(12, 18);
            this.enableJoystickDoubleBufferPanel.Name = "enableJoystickDoubleBufferPanel";
            this.enableJoystickDoubleBufferPanel.Size = new System.Drawing.Size(456, 40);
            this.enableJoystickDoubleBufferPanel.TabIndex = 23;
            this.enableJoystickDoubleBufferPanel.Tag = "EnableJoystick";
            // 
            // enableJoystickCheckBox
            // 
            this.enableJoystickCheckBox.AutoSize = true;
            this.enableJoystickCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.enableJoystickCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableJoystickCheckBox.Location = new System.Drawing.Point(360, 0);
            this.enableJoystickCheckBox.Name = "enableJoystickCheckBox";
            this.enableJoystickCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.enableJoystickCheckBox.Size = new System.Drawing.Size(96, 40);
            this.enableJoystickCheckBox.TabIndex = 1;
            this.enableJoystickCheckBox.Text = "Enabled";
            this.enableJoystickCheckBox.UseVisualStyleBackColor = true;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.contentPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Setup";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(486, 390);
            this.contentPanel.ResumeLayout(false);
            this.enableAxisJoystickDoubleBufferPanel.ResumeLayout(false);
            this.enableAxisJoystickDoubleBufferPanel.PerformLayout();
            this.enableJoystickDoubleBufferPanel.ResumeLayout(false);
            this.enableJoystickDoubleBufferPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel contentPanel;
        private PanelBase.DoubleBufferPanel enableJoystickDoubleBufferPanel;
        private System.Windows.Forms.CheckBox enableJoystickCheckBox;
        private DoubleBufferPanel enableAxisJoystickDoubleBufferPanel;
        private System.Windows.Forms.CheckBox enableAxisJoystickCheckBox;


    }
}
