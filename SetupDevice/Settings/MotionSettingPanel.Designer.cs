using System.Windows.Forms;

namespace SetupDevice
{
    partial class MotionSettingPanel
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
            this.containerPanel = new System.Windows.Forms.Panel();
            this.brightnessPanel = new PanelBase.DoubleBufferPanel();
            this.thresholdComboBox1 = new System.Windows.Forms.ComboBox();
            this.thresholdComboBox3 = new System.Windows.Forms.ComboBox();
            this.thresholdComboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.temporalComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.spatialComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.levelComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.speedComboBox = new System.Windows.Forms.ComboBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.MotionRegionControl = new SetupDevice.MotionRegionControl();
            this.containerPanel.SuspendLayout();
            this.brightnessPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.containerPanel.Controls.Add(this.elementHost1);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerPanel.Location = new System.Drawing.Point(12, 88);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(1022, 283);
            this.containerPanel.TabIndex = 20;
            // 
            // brightnessPanel
            // 
            this.brightnessPanel.BackColor = System.Drawing.Color.Transparent;
            this.brightnessPanel.Controls.Add(this.speedComboBox);
            this.brightnessPanel.Controls.Add(this.label6);
            this.brightnessPanel.Controls.Add(this.levelComboBox);
            this.brightnessPanel.Controls.Add(this.label5);
            this.brightnessPanel.Controls.Add(this.spatialComboBox);
            this.brightnessPanel.Controls.Add(this.label4);
            this.brightnessPanel.Controls.Add(this.label1);
            this.brightnessPanel.Controls.Add(this.label3);
            this.brightnessPanel.Controls.Add(this.thresholdComboBox1);
            this.brightnessPanel.Controls.Add(this.thresholdComboBox3);
            this.brightnessPanel.Controls.Add(this.thresholdComboBox2);
            this.brightnessPanel.Controls.Add(this.checkBox3);
            this.brightnessPanel.Controls.Add(this.checkBox2);
            this.brightnessPanel.Controls.Add(this.checkBox1);
            this.brightnessPanel.Controls.Add(this.temporalComboBox);
            this.brightnessPanel.Controls.Add(this.label2);
            this.brightnessPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.brightnessPanel.Location = new System.Drawing.Point(12, 18);
            this.brightnessPanel.Name = "brightnessPanel";
            this.brightnessPanel.Size = new System.Drawing.Size(1022, 70);
            this.brightnessPanel.TabIndex = 21;
            this.brightnessPanel.Tag = "Brightness";
            // 
            // thresholdComboBox1
            // 
            this.thresholdComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thresholdComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thresholdComboBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thresholdComboBox1.FormattingEnabled = true;
            this.thresholdComboBox1.IntegralHeight = false;
            this.thresholdComboBox1.Location = new System.Drawing.Point(99, 40);
            this.thresholdComboBox1.MaxDropDownItems = 20;
            this.thresholdComboBox1.Name = "thresholdComboBox1";
            this.thresholdComboBox1.Size = new System.Drawing.Size(67, 23);
            this.thresholdComboBox1.TabIndex = 2;
            // 
            // thresholdComboBox3
            // 
            this.thresholdComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thresholdComboBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thresholdComboBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thresholdComboBox3.FormattingEnabled = true;
            this.thresholdComboBox3.IntegralHeight = false;
            this.thresholdComboBox3.Location = new System.Drawing.Point(286, 40);
            this.thresholdComboBox3.MaxDropDownItems = 20;
            this.thresholdComboBox3.Name = "thresholdComboBox3";
            this.thresholdComboBox3.Size = new System.Drawing.Size(67, 23);
            this.thresholdComboBox3.TabIndex = 2;
            // 
            // thresholdComboBox2
            // 
            this.thresholdComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thresholdComboBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thresholdComboBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thresholdComboBox2.FormattingEnabled = true;
            this.thresholdComboBox2.IntegralHeight = false;
            this.thresholdComboBox2.Location = new System.Drawing.Point(189, 40);
            this.thresholdComboBox2.MaxDropDownItems = 20;
            this.thresholdComboBox2.Name = "thresholdComboBox2";
            this.thresholdComboBox2.Size = new System.Drawing.Size(67, 23);
            this.thresholdComboBox2.TabIndex = 2;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.Location = new System.Drawing.Point(286, 11);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(72, 19);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "Motion 3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(189, 11);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 19);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Motion 2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(99, 11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 19);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Motion 1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // temporalComboBox
            // 
            this.temporalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.temporalComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.temporalComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.temporalComboBox.FormattingEnabled = true;
            this.temporalComboBox.IntegralHeight = false;
            this.temporalComboBox.Location = new System.Drawing.Point(565, 7);
            this.temporalComboBox.MaxDropDownItems = 20;
            this.temporalComboBox.Name = "temporalComboBox";
            this.temporalComboBox.Size = new System.Drawing.Size(67, 23);
            this.temporalComboBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 35);
            this.label2.TabIndex = 6;
            this.label2.Text = "Enable";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(422, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 35);
            this.label3.TabIndex = 7;
            this.label3.Text = "Temporal Sensitivity";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 35);
            this.label1.TabIndex = 8;
            this.label1.Text = "Threshold";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(662, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 35);
            this.label4.TabIndex = 9;
            this.label4.Text = "Spatial Sensitivity";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // spatialComboBox
            // 
            this.spatialComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spatialComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.spatialComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spatialComboBox.FormattingEnabled = true;
            this.spatialComboBox.IntegralHeight = false;
            this.spatialComboBox.Location = new System.Drawing.Point(805, 7);
            this.spatialComboBox.MaxDropDownItems = 20;
            this.spatialComboBox.Name = "spatialComboBox";
            this.spatialComboBox.Size = new System.Drawing.Size(67, 23);
            this.spatialComboBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(422, 35);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 35);
            this.label5.TabIndex = 11;
            this.label5.Text = "Level Sensitivity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // levelComboBox
            // 
            this.levelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.levelComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.IntegralHeight = false;
            this.levelComboBox.Location = new System.Drawing.Point(565, 40);
            this.levelComboBox.MaxDropDownItems = 20;
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(67, 23);
            this.levelComboBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(662, 35);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 35);
            this.label6.TabIndex = 13;
            this.label6.Text = "Speed";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // speedComboBox
            // 
            this.speedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.speedComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.speedComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speedComboBox.FormattingEnabled = true;
            this.speedComboBox.IntegralHeight = false;
            this.speedComboBox.Location = new System.Drawing.Point(805, 40);
            this.speedComboBox.MaxDropDownItems = 20;
            this.speedComboBox.Name = "speedComboBox";
            this.speedComboBox.Size = new System.Drawing.Size(67, 23);
            this.speedComboBox.TabIndex = 14;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1022, 283);
            this.elementHost1.TabIndex = 1;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.MotionRegionControl;
            // 
            // MotionSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.brightnessPanel);
            this.Name = "MotionSettingPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(1046, 389);
            this.containerPanel.ResumeLayout(false);
            this.brightnessPanel.ResumeLayout(false);
            this.brightnessPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel containerPanel;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private MotionRegionControl MotionRegionControl;
        private PanelBase.DoubleBufferPanel brightnessPanel;
        private System.Windows.Forms.ComboBox temporalComboBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox thresholdComboBox1;
        private System.Windows.Forms.ComboBox thresholdComboBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private ComboBox thresholdComboBox3;
        private Label label2;
        private Label label1;
        private Label label3;
        private ComboBox speedComboBox;
        private Label label6;
        private ComboBox levelComboBox;
        private Label label5;
        private ComboBox spatialComboBox;
        private Label label4;
    }
}
