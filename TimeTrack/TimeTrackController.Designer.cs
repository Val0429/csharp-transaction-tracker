using System;

namespace TimeTrack
{
    partial class TimeTrackController
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
            this.FastReverseSpeedSelectorButton = new System.Windows.Forms.PictureBox();
            this.FastReverseButton = new System.Windows.Forms.PictureBox();
            this.FastPlayButton = new System.Windows.Forms.PictureBox();
            this.PlayButton = new System.Windows.Forms.PictureBox();
            this.FastPlaySpeedSelectorButton = new System.Windows.Forms.PictureBox();
            this.FastPlayList = new System.Windows.Forms.Panel();
            this.FastReverseList = new System.Windows.Forms.Panel();
            this.PauseButton = new System.Windows.Forms.PictureBox();
            this.fastSpeedLabel = new System.Windows.Forms.Label();
            this.reverseSpeedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FastReverseSpeedSelectorButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastReverseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastPlayButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastPlaySpeedSelectorButton)).BeginInit();
            this.FastPlayList.SuspendLayout();
            this.FastReverseList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PauseButton)).BeginInit();
            this.SuspendLayout();
            // 
            // FastReverseSpeedSelectorButton
            // 
            this.FastReverseSpeedSelectorButton.BackColor = System.Drawing.Color.Transparent;
            this.FastReverseSpeedSelectorButton.BackgroundImage = global::TimeTrack.Properties.Resources.Black;
            this.FastReverseSpeedSelectorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastReverseSpeedSelectorButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FastReverseSpeedSelectorButton.Location = new System.Drawing.Point(52, 0);
            this.FastReverseSpeedSelectorButton.Name = "FastReverseSpeedSelectorButton";
            this.FastReverseSpeedSelectorButton.Size = new System.Drawing.Size(26, 37);
            this.FastReverseSpeedSelectorButton.TabIndex = 7;
            this.FastReverseSpeedSelectorButton.TabStop = false;
            // 
            // FastReverseButton
            // 
            this.FastReverseButton.BackColor = System.Drawing.Color.Transparent;
            this.FastReverseButton.BackgroundImage = global::TimeTrack.Properties.Resources.FastReverse;
            this.FastReverseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastReverseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FastReverseButton.Location = new System.Drawing.Point(48, 0);
            this.FastReverseButton.Margin = new System.Windows.Forms.Padding(0);
            this.FastReverseButton.Name = "FastReverseButton";
            this.FastReverseButton.Size = new System.Drawing.Size(42, 37);
            this.FastReverseButton.TabIndex = 4;
            this.FastReverseButton.TabStop = false;
            this.FastReverseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FastReverseButtonMouseDown);
            this.FastReverseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastReverseButtonMouseUp);
            // 
            // FastPlayButton
            // 
            this.FastPlayButton.BackColor = System.Drawing.Color.Transparent;
            this.FastPlayButton.BackgroundImage = global::TimeTrack.Properties.Resources.FastPlay;
            this.FastPlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastPlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FastPlayButton.Location = new System.Drawing.Point(164, 0);
            this.FastPlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.FastPlayButton.Name = "FastPlayButton";
            this.FastPlayButton.Size = new System.Drawing.Size(42, 37);
            this.FastPlayButton.TabIndex = 2;
            this.FastPlayButton.TabStop = false;
            this.FastPlayButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FastPlayButtonMouseDown);
            this.FastPlayButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastPlayButtonMouseUp);
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.Transparent;
            this.PlayButton.BackgroundImage = global::TimeTrack.Properties.Resources.Play;
            this.PlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PlayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayButton.Location = new System.Drawing.Point(106, 0);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(42, 37);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.TabStop = false;
            this.PlayButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlayButtonMouseUp);
            // 
            // FastPlaySpeedSelectorButton
            // 
            this.FastPlaySpeedSelectorButton.BackColor = System.Drawing.Color.Transparent;
            this.FastPlaySpeedSelectorButton.BackgroundImage = global::TimeTrack.Properties.Resources.Black;
            this.FastPlaySpeedSelectorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastPlaySpeedSelectorButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FastPlaySpeedSelectorButton.Location = new System.Drawing.Point(26, 0);
            this.FastPlaySpeedSelectorButton.Name = "FastPlaySpeedSelectorButton";
            this.FastPlaySpeedSelectorButton.Size = new System.Drawing.Size(26, 37);
            this.FastPlaySpeedSelectorButton.TabIndex = 9;
            this.FastPlaySpeedSelectorButton.TabStop = false;
            // 
            // FastPlayList
            // 
            this.FastPlayList.BackgroundImage = global::TimeTrack.Properties.Resources.FastPlayList;
            this.FastPlayList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastPlayList.Controls.Add(this.FastPlaySpeedSelectorButton);
            this.FastPlayList.Location = new System.Drawing.Point(150, 0);
            this.FastPlayList.Margin = new System.Windows.Forms.Padding(0);
            this.FastPlayList.Name = "FastPlayList";
            this.FastPlayList.Size = new System.Drawing.Size(105, 37);
            this.FastPlayList.TabIndex = 10;
            this.FastPlayList.Visible = false;
            this.FastPlayList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FastPlayListMouseMove);
            this.FastPlayList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastPlayListMouseUp);
            // 
            // FastReverseList
            // 
            this.FastReverseList.BackgroundImage = global::TimeTrack.Properties.Resources.FastReverseList;
            this.FastReverseList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FastReverseList.Controls.Add(this.FastReverseSpeedSelectorButton);
            this.FastReverseList.Location = new System.Drawing.Point(0, 0);
            this.FastReverseList.Margin = new System.Windows.Forms.Padding(0);
            this.FastReverseList.Name = "FastReverseList";
            this.FastReverseList.Size = new System.Drawing.Size(105, 37);
            this.FastReverseList.TabIndex = 11;
            this.FastReverseList.Visible = false;
            this.FastReverseList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FastReverseListMouseMove);
            this.FastReverseList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastReverseListMouseUp);
            // 
            // PauseButton
            // 
            this.PauseButton.BackColor = System.Drawing.Color.Transparent;
            this.PauseButton.BackgroundImage = global::TimeTrack.Properties.Resources.Pause;
            this.PauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PauseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PauseButton.Location = new System.Drawing.Point(106, 0);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(42, 37);
            this.PauseButton.TabIndex = 12;
            this.PauseButton.TabStop = false;
            this.PauseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PauseButtonMouseUp);
            // 
            // fastSpeedLabel
            // 
            this.fastSpeedLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastSpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(198)))), ((int)(((byte)(203)))));
            this.fastSpeedLabel.Location = new System.Drawing.Point(208, 0);
            this.fastSpeedLabel.Name = "fastSpeedLabel";
            this.fastSpeedLabel.Size = new System.Drawing.Size(32, 37);
            this.fastSpeedLabel.TabIndex = 13;
            this.fastSpeedLabel.Text = "8x";
            this.fastSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // reverseSpeedLabel
            // 
            this.reverseSpeedLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reverseSpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(198)))), ((int)(((byte)(203)))));
            this.reverseSpeedLabel.Location = new System.Drawing.Point(11, 0);
            this.reverseSpeedLabel.Name = "reverseSpeedLabel";
            this.reverseSpeedLabel.Size = new System.Drawing.Size(35, 37);
            this.reverseSpeedLabel.TabIndex = 14;
            this.reverseSpeedLabel.Text = "8x";
            this.reverseSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TimeTrackController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.FastReverseList);
            this.Controls.Add(this.FastPlayList);
            this.Controls.Add(this.FastReverseButton);
            this.Controls.Add(this.FastPlayButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.fastSpeedLabel);
            this.Controls.Add(this.reverseSpeedLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TimeTrackController";
            this.Size = new System.Drawing.Size(256, 37);
            ((System.ComponentModel.ISupportInitialize)(this.FastReverseSpeedSelectorButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastReverseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastPlayButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FastPlaySpeedSelectorButton)).EndInit();
            this.FastPlayList.ResumeLayout(false);
            this.FastReverseList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PauseButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.PictureBox FastPlayButton;
        private System.Windows.Forms.PictureBox FastReverseButton;
    	protected System.Windows.Forms.PictureBox FastReverseSpeedSelectorButton;
    	protected System.Windows.Forms.PictureBox FastPlaySpeedSelectorButton;
		protected System.Windows.Forms.Panel FastPlayList;
		protected System.Windows.Forms.Panel FastReverseList;
        public System.Windows.Forms.Label fastSpeedLabel;
		public System.Windows.Forms.Label reverseSpeedLabel;
		public System.Windows.Forms.PictureBox PlayButton;
		public System.Windows.Forms.PictureBox PauseButton;
    }
}
