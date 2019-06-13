﻿namespace UserDeviceGroup
{
    partial class Setup
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.titlePanel = new System.Windows.Forms.Panel();
            this.viewModelPanel = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(10, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(222, 30);
            this.titlePanel.TabIndex = 1;
            // 
            // viewModelPanel
            // 
            this.viewModelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.viewModelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewModelPanel.Location = new System.Drawing.Point(10, 30);
            this.viewModelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.viewModelPanel.Name = "viewModelPanel";
            this.viewModelPanel.Size = new System.Drawing.Size(222, 245);
            this.viewModelPanel.TabIndex = 2;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.viewModelPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "Setup";
            this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.Size = new System.Drawing.Size(242, 285);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel viewModelPanel;
    }
}