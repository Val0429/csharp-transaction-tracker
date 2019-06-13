using System;
using System.Windows.Forms;

namespace SetupNVR
{
    partial class SetupForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.saveButton = new System.Windows.Forms.Button();
            this.savePanel = new System.Windows.Forms.Panel();
            this.savePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.AutoSize = true;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.saveButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.Location = new System.Drawing.Point(641, 0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 27);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save Setting";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // savePanel
            // 
            this.savePanel.BackColor = System.Drawing.Color.DimGray;
            this.savePanel.BackgroundImage = global::SetupNVR.Properties.Resources.banner;
            this.savePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.savePanel.Controls.Add(this.saveButton);
            this.savePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.savePanel.Location = new System.Drawing.Point(0, 477);
            this.savePanel.Name = "savePanel";
            this.savePanel.Size = new System.Drawing.Size(726, 27);
            this.savePanel.TabIndex = 1;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(726, 504);
            this.Controls.Add(this.savePanel);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.savePanel.ResumeLayout(false);
            this.savePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button saveButton;
        private Panel savePanel;




    }
}

