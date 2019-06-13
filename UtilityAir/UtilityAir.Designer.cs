namespace UtilityAir
{
    partial class UtilityAir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UtilityAir));
            this._control = new AxNvrUtilityLib.AxNvrUtility();
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
            this._control.Size = new System.Drawing.Size(240, 160);
            this._control.TabIndex = 0;
            // 
            // UtilityAir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._control);
            this.Name = "UtilityAir";
            this.Size = new System.Drawing.Size(240, 160);
            ((System.ComponentModel.ISupportInitialize)(this._control)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxNvrUtilityLib.AxNvrUtility _control;
    }
}
