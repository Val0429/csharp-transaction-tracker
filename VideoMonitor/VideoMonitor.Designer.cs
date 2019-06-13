using System;
using System.Drawing;

namespace VideoMonitor
{
	partial class VideoMonitor
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
		protected void InitializeComponent()
		{
			this.windowsPanel = new PanelBase.DoubleBufferPanel();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.toolPanel = new PanelBase.DoubleBufferPanel();
			this.nameLabel = new PanelBase.DoubleBufferLabel();
			this.pagerPanelButton = new PanelBase.DoubleBufferPanel();
			this.popupPanel = new PanelBase.DoubleBufferPanel();
			this.nextPageButton = new PanelBase.DoubleBufferPanel();
			this.pageLabel = new PanelBase.DoubleBufferLabel();
			this.pagerPanel = new PanelBase.DoubleBufferPanel();
			this.previousPageButton = new PanelBase.DoubleBufferPanel();
			this.toolPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// windowsPanel
			// 
			this.windowsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
			this.windowsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.windowsPanel.Location = new System.Drawing.Point(0, 32);
			this.windowsPanel.Name = "windowsPanel";
			this.windowsPanel.Size = new System.Drawing.Size(649, 388);
			this.windowsPanel.TabIndex = 0;
			// 
			// toolPanel
			// 
			this.toolPanel.BackgroundImage = global::VideoMonitor.Properties.Resources.toolBG;
			this.toolPanel.Controls.Add(this.nameLabel);
			this.toolPanel.Controls.Add(this.pagerPanelButton);
			this.toolPanel.Controls.Add(this.popupPanel);
			this.toolPanel.Controls.Add(this.nextPageButton);
			this.toolPanel.Controls.Add(this.pageLabel);
			this.toolPanel.Controls.Add(this.pagerPanel);
			this.toolPanel.Controls.Add(this.previousPageButton);
			this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolPanel.Location = new System.Drawing.Point(0, 0);
			this.toolPanel.Name = "toolPanel";
			this.toolPanel.Size = new System.Drawing.Size(649, 32);
			this.toolPanel.TabIndex = 7;
			// 
			// nameLabel
			// 
			this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
			this.nameLabel.Location = new System.Drawing.Point(159, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(265, 32);
			this.nameLabel.TabIndex = 10;
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.nameLabel.UseMnemonic = false;
			// 
			// pagerPanelButton
			// 
			this.pagerPanelButton.BackgroundImage = global::VideoMonitor.Properties.Resources.left;
			this.pagerPanelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pagerPanelButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pagerPanelButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.pagerPanelButton.Location = new System.Drawing.Point(424, 0);
			this.pagerPanelButton.Margin = new System.Windows.Forms.Padding(0);
			this.pagerPanelButton.Name = "pagerPanelButton";
			this.pagerPanelButton.Size = new System.Drawing.Size(25, 32);
			this.pagerPanelButton.TabIndex = 12;
			// 
			// popupPanel
			// 
			this.popupPanel.BackgroundImage = global::VideoMonitor.Properties.Resources.popup;
			this.popupPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.popupPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.popupPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.popupPanel.Location = new System.Drawing.Point(121, 0);
			this.popupPanel.Margin = new System.Windows.Forms.Padding(0);
			this.popupPanel.Name = "popupPanel";
			this.popupPanel.Size = new System.Drawing.Size(38, 32);
			this.popupPanel.TabIndex = 13;
			this.popupPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PopupPanelMouseClick);
			// 
			// nextPageButton
			// 
			this.nextPageButton.BackgroundImage = global::VideoMonitor.Properties.Resources.nextPage;
			this.nextPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.nextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.nextPageButton.Dock = System.Windows.Forms.DockStyle.Left;
			this.nextPageButton.Location = new System.Drawing.Point(83, 0);
			this.nextPageButton.Margin = new System.Windows.Forms.Padding(0);
			this.nextPageButton.Name = "nextPageButton";
			this.nextPageButton.Size = new System.Drawing.Size(38, 32);
			this.nextPageButton.TabIndex = 8;
			this.nextPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NextPageButtonMouseClick);
			// 
			// pageLabel
			// 
			this.pageLabel.AutoSize = true;
			this.pageLabel.BackColor = System.Drawing.Color.Transparent;
			this.pageLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.pageLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
			this.pageLabel.Location = new System.Drawing.Point(38, 0);
			this.pageLabel.MinimumSize = new System.Drawing.Size(45, 32);
			this.pageLabel.Name = "pageLabel";
			this.pageLabel.Size = new System.Drawing.Size(45, 32);
			this.pageLabel.TabIndex = 9;
			this.pageLabel.Text = "1/1";
			this.pageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pagerPanel
			// 
			this.pagerPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.pagerPanel.Location = new System.Drawing.Point(449, 0);
			this.pagerPanel.Name = "pagerPanel";
			this.pagerPanel.Size = new System.Drawing.Size(200, 32);
			this.pagerPanel.TabIndex = 11;
			// 
			// previousPageButton
			// 
			this.previousPageButton.BackgroundImage = global::VideoMonitor.Properties.Resources.previousPage;
			this.previousPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.previousPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.previousPageButton.Dock = System.Windows.Forms.DockStyle.Left;
			this.previousPageButton.Location = new System.Drawing.Point(0, 0);
			this.previousPageButton.Margin = new System.Windows.Forms.Padding(0);
			this.previousPageButton.Name = "previousPageButton";
			this.previousPageButton.Size = new System.Drawing.Size(38, 32);
			this.previousPageButton.TabIndex = 7;
			this.previousPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PreviousPageButtonMouseClick);
			// 
			// VideoMonitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.windowsPanel);
			this.Controls.Add(this.toolPanel);
			this.DoubleBuffered = true;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "VideoMonitor";
			this.Size = new System.Drawing.Size(649, 420);
			this.toolPanel.ResumeLayout(false);
			this.toolPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		protected PanelBase.DoubleBufferPanel windowsPanel;
		private System.Windows.Forms.ContextMenu contextMenu;
	    protected PanelBase.DoubleBufferPanel toolPanel;
		protected PanelBase.DoubleBufferPanel nextPageButton;
		protected PanelBase.DoubleBufferLabel pageLabel;
	    protected PanelBase.DoubleBufferPanel pagerPanel;
		protected PanelBase.DoubleBufferLabel nameLabel;
		protected PanelBase.DoubleBufferPanel previousPageButton;
		protected PanelBase.DoubleBufferPanel pagerPanelButton;
		protected PanelBase.DoubleBufferPanel popupPanel;
	}
}
