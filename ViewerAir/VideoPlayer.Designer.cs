using System;
using System.Drawing;

namespace ViewerAir
{
	partial class VideoPlayer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(Boolean disposing)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPlayer));
			this._control = new AxNvrViewerLib.AxNvrCtrl();
			this.recordStatusPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this._control)).BeginInit();
			this.SuspendLayout();
			// 
			// _control
			// 
			this._control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this._control.Enabled = true;
			this._control.Location = new System.Drawing.Point(0, 0);
			this._control.Margin = new System.Windows.Forms.Padding(0);
			this._control.Name = "_control";
			this._control.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_control.OcxState")));
			this._control.Size = new System.Drawing.Size(192, 192);
			this._control.TabIndex = 0;
			this._control.TabStop = false;
			this._control.OnMouseKeyDoubleClick += new AxNvrViewerLib._INvrViewerEvents_OnMouseKeyDoubleClickEventHandler(this.ControlOnMouseKeyDoubleClick);
			this._control.OnMouseKeyDown += new AxNvrViewerLib._INvrViewerEvents_OnMouseKeyDownEventHandler(this.ControlOnMouseKeyDown);
			this._control.OnConnect += new AxNvrViewerLib._INvrViewerEvents_OnConnectEventHandler(this.ControlOnConnect);
			this._control.OnPlay += new System.EventHandler(this.ControlOnPlay);
			this._control.OnDisconnect += new System.EventHandler(this.ControlOnDisconnect);
			this._control.OnNetworkLoss += new System.EventHandler(this.ControlOnNetworkLoss);
			this._control.OnConnectionRecovery += new AxNvrViewerLib._INvrViewerEvents_OnConnectionRecoveryEventHandler(this.ControlOnConnectionRecovery);
			this._control.OnTimeCode += new AxNvrViewerLib._INvrViewerEvents_OnTimeCodeEventHandler(this.ControlOnTimeCode);
			this._control.OnCloseFullScreen += new System.EventHandler(this.ControlOnCloseFullScreen);
			this._control.OnUpdateBitrate += new AxNvrViewerLib._INvrViewerEvents_OnUpdateBitrateEventHandler(this.ControlOnUpdateBitrate);
			// 
			// recordStatusPanel
			// 
			this.recordStatusPanel.BackColor = System.Drawing.Color.Black;
			this.recordStatusPanel.BackgroundImage = global::ViewerAir.Properties.Resources.normal;
			this.recordStatusPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.recordStatusPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.recordStatusPanel.Location = new System.Drawing.Point(0, 0);
			this.recordStatusPanel.Margin = new System.Windows.Forms.Padding(0);
			this.recordStatusPanel.Name = "recordStatusPanel";
			this.recordStatusPanel.Size = new System.Drawing.Size(16, 16);
			this.recordStatusPanel.TabIndex = 1;
			this.recordStatusPanel.Visible = false;
			// 
			// VideoPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.Add(this.recordStatusPanel);
			this.Controls.Add(this._control);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "VideoPlayer";
			this.Size = new System.Drawing.Size(192, 192);
			((System.ComponentModel.ISupportInitialize)(this._control)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public AxNvrViewerLib.AxNvrCtrl _control;
		private System.Windows.Forms.Panel recordStatusPanel;
	}
}
