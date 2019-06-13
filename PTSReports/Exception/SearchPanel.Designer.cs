﻿using System.Windows.Forms;

namespace PTSReports.Exception
{
    sealed partial class SearchPanel
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
            this.pagePanel = new PanelBase.DoubleBufferPanel();
            this.chartPanel = new PanelBase.DoubleBufferPanel();
            this.chartDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.resultrPanel = new PanelBase.DoubleBufferPanel();
            this.foundLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.chartPanel.SuspendLayout();
            this.resultrPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 50);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.containerPanel.Size = new System.Drawing.Size(456, 317);
            this.containerPanel.TabIndex = 0;
            // 
            // pagePanel
            // 
            this.pagePanel.BackColor = System.Drawing.Color.Transparent;
            this.pagePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagePanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagePanel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pagePanel.Location = new System.Drawing.Point(12, 367);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Padding = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.pagePanel.Size = new System.Drawing.Size(456, 30);
            this.pagePanel.TabIndex = 7;
            // 
            // chartPanel
            // 
            this.chartPanel.BackColor = System.Drawing.Color.Transparent;
            this.chartPanel.Controls.Add(this.chartDoubleBufferPanel);
            this.chartPanel.Cursor = System.Windows.Forms.Cursors.PanSouth;
            this.chartPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartPanel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.chartPanel.Location = new System.Drawing.Point(12, 10);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.chartPanel.Size = new System.Drawing.Size(456, 40);
            this.chartPanel.TabIndex = 8;
            this.chartPanel.Tag = "Chart";
            // 
            // chartDoubleBufferPanel
            // 
            this.chartDoubleBufferPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartDoubleBufferPanel.AutoScroll = true;
            this.chartDoubleBufferPanel.BackColor = System.Drawing.Color.White;
            this.chartDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chartDoubleBufferPanel.Location = new System.Drawing.Point(0, 40);
            this.chartDoubleBufferPanel.MinimumSize = new System.Drawing.Size(0, 180);
            this.chartDoubleBufferPanel.Name = "chartDoubleBufferPanel";
            this.chartDoubleBufferPanel.Size = new System.Drawing.Size(456, 180);
            this.chartDoubleBufferPanel.TabIndex = 25;
            this.chartDoubleBufferPanel.Tag = "Exception";
            // 
            // resultrPanel
            // 
            this.resultrPanel.BackColor = System.Drawing.Color.Transparent;
            this.resultrPanel.Controls.Add(this.foundLabel);
            this.resultrPanel.Controls.Add(this.resultLabel);
            this.resultrPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultrPanel.Location = new System.Drawing.Point(12, 50);
            this.resultrPanel.Name = "resultrPanel";
            this.resultrPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.resultrPanel.Size = new System.Drawing.Size(456, 20);
            this.resultrPanel.TabIndex = 9;
            // 
            // foundLabel
            // 
            this.foundLabel.AutoSize = true;
            this.foundLabel.BackColor = System.Drawing.Color.Transparent;
            this.foundLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.foundLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(177)))), ((int)(((byte)(224)))));
            this.foundLabel.Location = new System.Drawing.Point(64, 0);
            this.foundLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new System.Drawing.Size(52, 20);
            this.foundLabel.TabIndex = 7;
            this.foundLabel.Text = "1 Found";
            this.foundLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.resultLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultLabel.Location = new System.Drawing.Point(8, 0);
            this.resultLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.resultLabel.Size = new System.Drawing.Size(56, 20);
            this.resultLabel.TabIndex = 6;
            this.resultLabel.Text = "Result :";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SearchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.pagePanel);
            this.Controls.Add(this.resultrPanel);
            this.Controls.Add(this.chartPanel);
            this.Name = "SearchPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 10, 12, 8);
            this.Size = new System.Drawing.Size(480, 400);
            this.chartPanel.ResumeLayout(false);
            this.resultrPanel.ResumeLayout(false);
            this.resultrPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel pagePanel;
        private PanelBase.DoubleBufferPanel chartPanel;
        private PanelBase.DoubleBufferPanel chartDoubleBufferPanel;
        private PanelBase.DoubleBufferPanel resultrPanel;
        private Label foundLabel;
        private Label resultLabel;

    }
}