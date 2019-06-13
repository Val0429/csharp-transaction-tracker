using System;

namespace TimeTrack
{
    sealed partial class MiniTimeTrackController
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.PlayButton = new PanelBase.DoubleBufferPanel();
            this.PauseButton = new PanelBase.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.Transparent;
            this.PlayButton.BackgroundImage = global::TimeTrack.Properties.Resources.Play;
            this.PlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PlayButton.Location = new System.Drawing.Point(43, 0);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(42, 31);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlayButtonMouseUp);
            // 
            // PauseButton
            // 
            this.PauseButton.BackColor = System.Drawing.Color.Transparent;
            this.PauseButton.BackgroundImage = global::TimeTrack.Properties.Resources.Pause;
            this.PauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PauseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PauseButton.Location = new System.Drawing.Point(1, 0);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(42, 31);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PauseButtonMouseUp);
            // 
            // MiniTimeTrackController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::TimeTrack.Properties.Resources.controllerBG;
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.PauseButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MiniTimeTrackController";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.Size = new System.Drawing.Size(88, 31);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel PauseButton;
        private PanelBase.DoubleBufferPanel PlayButton;
    }
}
