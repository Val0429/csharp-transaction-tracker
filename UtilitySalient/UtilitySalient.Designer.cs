namespace UtilitySalient
{
	partial class UtilitySalient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UtilitySalient));
            this._control = new AxCVClientControlLib.AxCVVideo();
            ((System.ComponentModel.ISupportInitialize)(this._control)).BeginInit();
            this.SuspendLayout();
            // 
            // _control
            // 
            this._control.Dock = System.Windows.Forms.DockStyle.Fill;
            this._control.Enabled = true;
            this._control.Location = new System.Drawing.Point(0, 0);
            this._control.Name = "_control";
            this._control.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_control.OcxState")));
            this._control.Size = new System.Drawing.Size(150, 150);
            this._control.TabIndex = 0;
            // 
            // UtilitySalient
            // 
            this.Controls.Add(this._control);
            this.Name = "UtilitySalient";
            ((System.ComponentModel.ISupportInitialize)(this._control)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxCVClientControlLib.AxCVVideo _control;
    }
}
