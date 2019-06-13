namespace SmartSearch.Badge
{
	partial class BadgeSearch
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
			this.staffComboBox = new System.Windows.Forms.ComboBox();
			this.AccessComboBox = new System.Windows.Forms.ComboBox();
			this.accessLabel = new System.Windows.Forms.Label();
			this.staffLabel = new System.Windows.Forms.Label();
			this.conditionPanel.SuspendLayout();
			this.maskDoubleBufferPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.Size = new System.Drawing.Size(0, 20);
			// 
			// conditionPanel
			// 
			this.conditionPanel.Location = new System.Drawing.Point(-345, 50);
			this.conditionPanel.Size = new System.Drawing.Size(335, 0);
			// 
			// searchButton
			// 
			this.searchButton.FlatAppearance.BorderSize = 0;
			this.searchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.searchButton.Location = new System.Drawing.Point(247, -41);
			// 
			// maskDoubleBufferPanel
			// 
			this.maskDoubleBufferPanel.Controls.Add(this.staffComboBox);
			this.maskDoubleBufferPanel.Controls.Add(this.AccessComboBox);
			this.maskDoubleBufferPanel.Controls.Add(this.accessLabel);
			this.maskDoubleBufferPanel.Controls.Add(this.staffLabel);
			this.maskDoubleBufferPanel.Size = new System.Drawing.Size(335, 830);
			this.maskDoubleBufferPanel.Visible = true;
			// 
			// staffComboBox
			// 
			this.staffComboBox.Font = new System.Drawing.Font("Arial", 9F);
			this.staffComboBox.FormattingEnabled = true;
			this.staffComboBox.Location = new System.Drawing.Point(63, 85);
			this.staffComboBox.Name = "staffComboBox";
			this.staffComboBox.Size = new System.Drawing.Size(121, 23);
			this.staffComboBox.TabIndex = 41;
			// 
			// AccessComboBox
			// 
			this.AccessComboBox.Font = new System.Drawing.Font("Arial", 9F);
			this.AccessComboBox.FormattingEnabled = true;
			this.AccessComboBox.Location = new System.Drawing.Point(63, 119);
			this.AccessComboBox.Name = "AccessComboBox";
			this.AccessComboBox.Size = new System.Drawing.Size(121, 23);
			this.AccessComboBox.TabIndex = 40;
			// 
			// accessLabel
			// 
			this.accessLabel.BackColor = System.Drawing.Color.Transparent;
			this.accessLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.accessLabel.ForeColor = System.Drawing.Color.Black;
			this.accessLabel.Location = new System.Drawing.Point(12, 119);
			this.accessLabel.Name = "accessLabel";
			this.accessLabel.Size = new System.Drawing.Size(49, 22);
			this.accessLabel.TabIndex = 39;
			this.accessLabel.Text = "Access";
			this.accessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// staffLabel
			// 
			this.staffLabel.BackColor = System.Drawing.Color.Transparent;
			this.staffLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.staffLabel.ForeColor = System.Drawing.Color.Black;
			this.staffLabel.Location = new System.Drawing.Point(12, 85);
			this.staffLabel.Name = "staffLabel";
			this.staffLabel.Size = new System.Drawing.Size(49, 22);
			this.staffLabel.TabIndex = 38;
			this.staffLabel.Text = "Staff";
			this.staffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BadgeSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Name = "BadgeSearch";
			this.Size = new System.Drawing.Size(0, 0);
			this.conditionPanel.ResumeLayout(false);
			this.conditionPanel.PerformLayout();
			this.maskDoubleBufferPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label staffLabel;
		private System.Windows.Forms.Label accessLabel;
		private System.Windows.Forms.ComboBox staffComboBox;
		private System.Windows.Forms.ComboBox AccessComboBox;
	}
}
