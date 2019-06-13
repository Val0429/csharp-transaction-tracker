using System;
using System.Windows.Forms;

namespace ViewerSalient
{
	sealed partial class VideoPlayer
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
            this._control = new AxCVClientControlLib.AxCVVideo();
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
            this._control.OnLostConnection += new System.EventHandler(this.ControlOnLostConnection);
            this._control.OnFailedConnection += new AxCVClientControlLib._ICVVideoEvents_OnFailedConnectionEventHandler(this.ControlOnFailedConnection);
            this._control.OnConnected += new System.EventHandler(this.ControlOnConnected);
            this._control.OnMouseDown += new AxCVClientControlLib._ICVVideoEvents_OnMouseDownEventHandler(this.ControlOnMouseDown);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
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

		private AxCVClientControlLib.AxCVVideo _control;
	    public void InitFisheyeLibrary(bool dewarpEnable, short mountType)
	    {
	        
	    }
	}
}
