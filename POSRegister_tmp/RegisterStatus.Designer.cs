namespace PosRegister
{
	partial class RegisterStatus
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
			this.RegisterPanel = new System.Windows.Forms.Panel();
			this.registerComboBox = new System.Windows.Forms.ComboBox();
			this.RegisterLabel = new System.Windows.Forms.Label();
			this.updatePanel = new System.Windows.Forms.Panel();
			this.lastUpdateLabel = new System.Windows.Forms.Label();
			this.updateLabel = new System.Windows.Forms.Label();
			this.TransactionPanel = new System.Windows.Forms.Panel();
			this.transactionTextBox = new System.Windows.Forms.TextBox();
			this.RegisterPanel.SuspendLayout();
			this.updatePanel.SuspendLayout();
			this.TransactionPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// titlePanel
			// 
			this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.titlePanel.ForeColor = System.Drawing.Color.White;
			this.titlePanel.Location = new System.Drawing.Point(0, 0);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.Size = new System.Drawing.Size(250, 30);
			this.titlePanel.TabIndex = 7;
			// 
			// RegisterPanel
			// 
			this.RegisterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(119)))), ((int)(((byte)(127)))));
			this.RegisterPanel.Controls.Add(this.registerComboBox);
			this.RegisterPanel.Controls.Add(this.RegisterLabel);
			this.RegisterPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.RegisterPanel.Location = new System.Drawing.Point(0, 30);
			this.RegisterPanel.Name = "RegisterPanel";
			this.RegisterPanel.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.RegisterPanel.Size = new System.Drawing.Size(250, 30);
			this.RegisterPanel.TabIndex = 8;
			// 
			// registerComboBox
			// 
			this.registerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.registerComboBox.FormattingEnabled = true;
			this.registerComboBox.Location = new System.Drawing.Point(65, 4);
			this.registerComboBox.Name = "registerComboBox";
			this.registerComboBox.Size = new System.Drawing.Size(179, 23);
			this.registerComboBox.TabIndex = 1;
			// 
			// RegisterLabel
			// 
			this.RegisterLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.RegisterLabel.ForeColor = System.Drawing.Color.White;
			this.RegisterLabel.Location = new System.Drawing.Point(7, 4);
			this.RegisterLabel.Name = "RegisterLabel";
			this.RegisterLabel.Size = new System.Drawing.Size(57, 23);
			this.RegisterLabel.TabIndex = 0;
			this.RegisterLabel.Text = "Register:";
			this.RegisterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// updatePanel
			// 
			this.updatePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(119)))), ((int)(((byte)(127)))));
			this.updatePanel.Controls.Add(this.lastUpdateLabel);
			this.updatePanel.Controls.Add(this.updateLabel);
			this.updatePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.updatePanel.Location = new System.Drawing.Point(0, 297);
			this.updatePanel.Name = "updatePanel";
			this.updatePanel.Size = new System.Drawing.Size(250, 30);
			this.updatePanel.TabIndex = 9;
			// 
			// lastUpdateLabel
			// 
			this.lastUpdateLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.lastUpdateLabel.ForeColor = System.Drawing.Color.White;
			this.lastUpdateLabel.Location = new System.Drawing.Point(79, 4);
			this.lastUpdateLabel.Name = "lastUpdateLabel";
			this.lastUpdateLabel.Size = new System.Drawing.Size(134, 23);
			this.lastUpdateLabel.TabIndex = 1;
			this.lastUpdateLabel.Text = "2112/03/15 12:12:12";
			this.lastUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// updateLabel
			// 
			this.updateLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.updateLabel.ForeColor = System.Drawing.Color.White;
			this.updateLabel.Location = new System.Drawing.Point(3, 4);
			this.updateLabel.Name = "updateLabel";
			this.updateLabel.Size = new System.Drawing.Size(80, 23);
			this.updateLabel.TabIndex = 0;
			this.updateLabel.Text = "Last Update:";
			this.updateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TransactionPanel
			// 
			this.TransactionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(95)))), ((int)(((byte)(103)))));
			this.TransactionPanel.Controls.Add(this.transactionTextBox);
			this.TransactionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TransactionPanel.Location = new System.Drawing.Point(0, 60);
			this.TransactionPanel.Name = "TransactionPanel";
			this.TransactionPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.TransactionPanel.Size = new System.Drawing.Size(250, 237);
			this.TransactionPanel.TabIndex = 10;
			// 
			// transactionTextBox
			// 
			this.transactionTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(95)))), ((int)(((byte)(103)))));
			this.transactionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.transactionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.transactionTextBox.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.transactionTextBox.ForeColor = System.Drawing.Color.White;
			this.transactionTextBox.Location = new System.Drawing.Point(5, 0);
			this.transactionTextBox.Multiline = true;
			this.transactionTextBox.Name = "transactionTextBox";
			this.transactionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.transactionTextBox.Size = new System.Drawing.Size(245, 237);
			this.transactionTextBox.TabIndex = 0;
			this.transactionTextBox.Text = "\r\n";
			// 
			// RegisterStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.TransactionPanel);
			this.Controls.Add(this.updatePanel);
			this.Controls.Add(this.RegisterPanel);
			this.Controls.Add(this.titlePanel);
			this.Font = new System.Drawing.Font("Arial", 9F);
			this.Name = "RegisterStatus";
			this.Size = new System.Drawing.Size(250, 327);
			this.RegisterPanel.ResumeLayout(false);
			this.updatePanel.ResumeLayout(false);
			this.TransactionPanel.ResumeLayout(false);
			this.TransactionPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel titlePanel;
		private System.Windows.Forms.Panel RegisterPanel;
		private System.Windows.Forms.ComboBox registerComboBox;
		private System.Windows.Forms.Label RegisterLabel;
		private System.Windows.Forms.Panel updatePanel;
		private System.Windows.Forms.Label lastUpdateLabel;
		private System.Windows.Forms.Label updateLabel;
		private System.Windows.Forms.Panel TransactionPanel;
		private System.Windows.Forms.TextBox transactionTextBox;
	}
}
