using System.Drawing;

namespace PresetPointPanel
{
	partial class PresetPointPanel
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.presetPointListPanel = new System.Windows.Forms.Panel();
            this.presetPointcomboBox = new System.Windows.Forms.ComboBox();
            this.presetPointLabel = new System.Windows.Forms.Label();
            this.presetTourlabel = new System.Windows.Forms.Label();
            this.presetTourComboBox = new System.Windows.Forms.ComboBox();
            this.presetPointListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(200, 30);
            this.titlePanel.TabIndex = 2;
            // 
            // presetPointListPanel
            // 
            this.presetPointListPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.presetPointListPanel.Controls.Add(this.presetPointcomboBox);
            this.presetPointListPanel.Controls.Add(this.presetPointLabel);
            this.presetPointListPanel.Controls.Add(this.presetTourlabel);
            this.presetPointListPanel.Controls.Add(this.presetTourComboBox);
            this.presetPointListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.presetPointListPanel.Location = new System.Drawing.Point(0, 30);
            this.presetPointListPanel.Name = "presetPointListPanel";
            this.presetPointListPanel.Size = new System.Drawing.Size(200, 45);
            this.presetPointListPanel.TabIndex = 3;
            // 
            // presetPointcomboBox
            // 
            this.presetPointcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetPointcomboBox.Enabled = false;
            this.presetPointcomboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.presetPointcomboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetPointcomboBox.FormattingEnabled = true;
            this.presetPointcomboBox.Location = new System.Drawing.Point(14, 11);
            this.presetPointcomboBox.Margin = new System.Windows.Forms.Padding(0);
            this.presetPointcomboBox.Name = "presetPointcomboBox";
            this.presetPointcomboBox.Size = new System.Drawing.Size(168, 23);
            this.presetPointcomboBox.TabIndex = 0;
            this.presetPointcomboBox.SelectionChangeCommitted += new System.EventHandler(this.PresetPointcomboBoxSelectionChangeCommitted);
            // 
            // presetPointLabel
            // 
            this.presetPointLabel.BackColor = System.Drawing.Color.Transparent;
            this.presetPointLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetPointLabel.ForeColor = System.Drawing.Color.White;
            this.presetPointLabel.Location = new System.Drawing.Point(14, 35);
            this.presetPointLabel.Margin = new System.Windows.Forms.Padding(0);
            this.presetPointLabel.Name = "presetPointLabel";
            this.presetPointLabel.Size = new System.Drawing.Size(97, 30);
            this.presetPointLabel.TabIndex = 2;
            this.presetPointLabel.Text = "Preset Point";
            this.presetPointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.presetPointLabel.Visible = false;
            // 
            // presetTourlabel
            // 
            this.presetTourlabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.presetTourlabel.BackColor = System.Drawing.Color.Transparent;
            this.presetTourlabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetTourlabel.ForeColor = System.Drawing.Color.White;
            this.presetTourlabel.Location = new System.Drawing.Point(14, 55);
            this.presetTourlabel.Name = "presetTourlabel";
            this.presetTourlabel.Size = new System.Drawing.Size(97, 30);
            this.presetTourlabel.TabIndex = 3;
            this.presetTourlabel.Text = "Preset Tour";
            this.presetTourlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.presetTourlabel.Visible = false;
            // 
            // presetTourComboBox
            // 
            this.presetTourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetTourComboBox.Enabled = false;
            this.presetTourComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.presetTourComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetTourComboBox.FormattingEnabled = true;
            this.presetTourComboBox.Location = new System.Drawing.Point(17, 83);
            this.presetTourComboBox.Name = "presetTourComboBox";
            this.presetTourComboBox.Size = new System.Drawing.Size(68, 23);
            this.presetTourComboBox.TabIndex = 1;
            this.presetTourComboBox.Visible = false;
            this.presetTourComboBox.SelectionChangeCommitted += new System.EventHandler(this.PresetTourComboBoxSelectionChangeCommitted);
            // 
            // PresetPointPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.presetPointListPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "PresetPointPanel";
            this.Size = new System.Drawing.Size(200, 75);
            this.presetPointListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Panel presetPointListPanel;
        private System.Windows.Forms.ComboBox presetPointcomboBox;
        private System.Windows.Forms.ComboBox presetTourComboBox;
        private System.Windows.Forms.Label presetPointLabel;
        private System.Windows.Forms.Label presetTourlabel;
    }
}
