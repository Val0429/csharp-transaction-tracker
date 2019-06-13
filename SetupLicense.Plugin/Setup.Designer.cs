namespace SetupLicense.Plugin
{
	partial class Setup : SetupLicense.Setup
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
            this.contentPanel.SuspendLayout();
            this.licenseKeyBufferPanel.SuspendLayout();
            this.ethernetCardPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Size = new System.Drawing.Size(1296, 425);
            // 
            // amountDoubleBufferPanel
            // 
            this.amountDoubleBufferPanel.Size = new System.Drawing.Size(1272, 40);
            // 
            // licenseKeyBufferPanel
            // 
            this.licenseKeyBufferPanel.Size = new System.Drawing.Size(1272, 40);
            // 
            // key1TextBox
            // 
            this.key1TextBox.Location = new System.Drawing.Point(946, 9);
            // 
            // key2TextBox
            // 
            this.key2TextBox.Location = new System.Drawing.Point(1011, 9);
            // 
            // key5TextBox
            // 
            this.key5TextBox.Location = new System.Drawing.Point(1206, 9);
            // 
            // key4TextBox
            // 
            this.key4TextBox.Location = new System.Drawing.Point(1141, 9);
            // 
            // key3TextBox
            // 
            this.key3TextBox.Location = new System.Drawing.Point(1076, 9);
            // 
            // infoContainerPanel
            // 
            this.infoContainerPanel.Size = new System.Drawing.Size(1272, 40);
            // 
            // ethernetCardPanel
            // 
            this.ethernetCardPanel.Size = new System.Drawing.Size(1272, 40);
            // 
            // ethernetComboBox
            // 
            this.ethernetComboBox.Location = new System.Drawing.Point(946, 8);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "Setup";
            this.Size = new System.Drawing.Size(1302, 431);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.licenseKeyBufferPanel.ResumeLayout(false);
            this.licenseKeyBufferPanel.PerformLayout();
            this.ethernetCardPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
	}
}
