namespace CMSViewer
{
	partial class CMSViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMSViewer));
            this.recordStatusPanel = new System.Windows.Forms.Panel();
            this._control = new AxiCMSViewerLib.AxiCMSCtrl();
            ((System.ComponentModel.ISupportInitialize)(this._control)).BeginInit();
            this.SuspendLayout();
            // 
            // recordStatusPanel
            // 
            this.recordStatusPanel.BackColor = System.Drawing.Color.Black;
            this.recordStatusPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.recordStatusPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.recordStatusPanel.Location = new System.Drawing.Point(0, 0);
            this.recordStatusPanel.Margin = new System.Windows.Forms.Padding(0);
            this.recordStatusPanel.Name = "recordStatusPanel";
            this.recordStatusPanel.Size = new System.Drawing.Size(15, 15);
            this.recordStatusPanel.TabIndex = 2;
            this.recordStatusPanel.Visible = false;
            // 
            // _control
            // 
            this._control.Enabled = true;
            this._control.Location = new System.Drawing.Point(0, 0);
            this._control.Name = "_control";
            this._control.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_control.OcxState")));
            this._control.Size = new System.Drawing.Size(300, 300);
            this._control.TabIndex = 3;
            this._control.OnMouseKeyDoubleClick += new AxiCMSViewerLib._IiCMSViewerEvents_OnMouseKeyDoubleClickEventHandler(this.ControlOnMouseKeyDoubleClick);
            this._control.OnMouseKeyDown += new AxiCMSViewerLib._IiCMSViewerEvents_OnMouseKeyDownEventHandler(this.ControlOnMouseKeyDown);
            this._control.OnConnect += new AxiCMSViewerLib._IiCMSViewerEvents_OnConnectEventHandler(this.ControlOnConnect);
            this._control.OnPlay += new System.EventHandler(this.ControlOnPlay);
            this._control.OnDisconnect += new System.EventHandler(this.ControlOnDisconnect);
            this._control.OnNetworkLoss += new System.EventHandler(this.ControlOnNetworkLoss);
            this._control.OnConnectionRecovery += new AxiCMSViewerLib._IiCMSViewerEvents_OnConnectionRecoveryEventHandler(this.ControlOnConnectionRecovery);
            this._control.OnTimeCode += new AxiCMSViewerLib._IiCMSViewerEvents_OnTimeCodeEventHandler(this.ControlOnTimeCode);
            this._control.OnCloseFullScreen += new System.EventHandler(this.ControlOnCloseFullScreen);
            this._control.OnUpdateBitrate += new AxiCMSViewerLib._IiCMSViewerEvents_OnUpdateBitrateEventHandler(this.ControlOnUpdateBitrate);
            // 
            // CMSViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.recordStatusPanel);
            this.Controls.Add(this._control);
            this.Name = "CMSViewer";
            this.Size = new System.Drawing.Size(300, 300);
            ((System.ComponentModel.ISupportInitialize)(this._control)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel recordStatusPanel;
        private AxiCMSViewerLib.AxiCMSCtrl _control;
	}
}
