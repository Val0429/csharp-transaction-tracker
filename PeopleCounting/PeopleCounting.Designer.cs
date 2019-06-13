using System.Windows;

namespace PeopleCounting
{
    sealed partial class PeopleCounting
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
            this.panelFunction = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panelDesc = new System.Windows.Forms.Panel();
            this.pictureBoxGreenArrow = new System.Windows.Forms.PictureBox();
            this.labelPeopleOut = new System.Windows.Forms.Label();
            this.pictureBoxRedArrow = new System.Windows.Forms.PictureBox();
            this.labelPeopleIn = new System.Windows.Forms.Label();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonDrawing = new System.Windows.Forms.Button();
            this.buttonRefreshImage = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonVerify = new System.Windows.Forms.Button();
            this.buttonCancelMakingFeature = new System.Windows.Forms.Button();
            this.buttonClearFeature = new System.Windows.Forms.Button();
            this.buttonMakeFeature = new System.Windows.Forms.Button();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.peopleCountingControl = new PeopleCountingControl();
            this.panelFunction.SuspendLayout();
            this.panelDesc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreenArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFunction
            // 
            this.panelFunction.BackColor = System.Drawing.Color.White;
            this.panelFunction.Controls.Add(this.buttonSave);
            this.panelFunction.Controls.Add(this.panelDesc);
            this.panelFunction.Controls.Add(this.buttonSwitch);
            this.panelFunction.Controls.Add(this.buttonRotate);
            this.panelFunction.Controls.Add(this.buttonRemove);
            this.panelFunction.Controls.Add(this.buttonClear);
            this.panelFunction.Controls.Add(this.buttonDrawing);
            this.panelFunction.Controls.Add(this.buttonRefreshImage);
            this.panelFunction.Controls.Add(this.buttonDelete);
            this.panelFunction.Controls.Add(this.buttonVerify);
            this.panelFunction.Controls.Add(this.buttonCancelMakingFeature);
            this.panelFunction.Controls.Add(this.buttonClearFeature);
            this.panelFunction.Controls.Add(this.buttonMakeFeature);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFunction.Location = new System.Drawing.Point(0, 0);
            this.panelFunction.Margin = new System.Windows.Forms.Padding(0);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelFunction.Size = new System.Drawing.Size(1031, 30);
            this.panelFunction.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(950, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(78, 20);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.SaveClick);
            // 
            // panelDesc
            // 
            this.panelDesc.AutoSize = true;
            this.panelDesc.Controls.Add(this.pictureBoxGreenArrow);
            this.panelDesc.Controls.Add(this.labelPeopleOut);
            this.panelDesc.Controls.Add(this.pictureBoxRedArrow);
            this.panelDesc.Controls.Add(this.labelPeopleIn);
            this.panelDesc.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDesc.Location = new System.Drawing.Point(955, 4);
            this.panelDesc.Name = "panelDesc";
            this.panelDesc.Size = new System.Drawing.Size(203, 22);
            this.panelDesc.TabIndex = 12;
            // 
            // pictureBoxGreenArrow
            // 
            this.pictureBoxGreenArrow.BackgroundImage = global::PeopleCounting.Properties.Resources.arrowGreen;
            this.pictureBoxGreenArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxGreenArrow.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxGreenArrow.Location = new System.Drawing.Point(168, 0);
            this.pictureBoxGreenArrow.Name = "pictureBoxGreenArrow";
            this.pictureBoxGreenArrow.Size = new System.Drawing.Size(35, 22);
            this.pictureBoxGreenArrow.TabIndex = 8;
            this.pictureBoxGreenArrow.TabStop = false;
            this.pictureBoxGreenArrow.Visible = false;
            // 
            // labelPeopleOut
            // 
            this.labelPeopleOut.AutoSize = true;
            this.labelPeopleOut.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPeopleOut.ForeColor = System.Drawing.Color.Black;
            this.labelPeopleOut.Location = new System.Drawing.Point(100, 0);
            this.labelPeopleOut.Name = "labelPeopleOut";
            this.labelPeopleOut.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.labelPeopleOut.Size = new System.Drawing.Size(68, 20);
            this.labelPeopleOut.TabIndex = 9;
            this.labelPeopleOut.Text = "People Out";
            this.labelPeopleOut.Visible = false;
            // 
            // pictureBoxRedArrow
            // 
            this.pictureBoxRedArrow.BackgroundImage = global::PeopleCounting.Properties.Resources.arrowRed;
            this.pictureBoxRedArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxRedArrow.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxRedArrow.Location = new System.Drawing.Point(59, 0);
            this.pictureBoxRedArrow.Name = "pictureBoxRedArrow";
            this.pictureBoxRedArrow.Size = new System.Drawing.Size(41, 22);
            this.pictureBoxRedArrow.TabIndex = 5;
            this.pictureBoxRedArrow.TabStop = false;
            this.pictureBoxRedArrow.Visible = false;
            // 
            // labelPeopleIn
            // 
            this.labelPeopleIn.AutoSize = true;
            this.labelPeopleIn.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPeopleIn.ForeColor = System.Drawing.Color.Black;
            this.labelPeopleIn.Location = new System.Drawing.Point(0, 0);
            this.labelPeopleIn.Name = "labelPeopleIn";
            this.labelPeopleIn.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.labelPeopleIn.Size = new System.Drawing.Size(59, 20);
            this.labelPeopleIn.TabIndex = 7;
            this.labelPeopleIn.Text = "People In";
            this.labelPeopleIn.Visible = false;
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.AutoSize = true;
            this.buttonSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSwitch.Enabled = false;
            this.buttonSwitch.Location = new System.Drawing.Point(804, 4);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(151, 22);
            this.buttonSwitch.TabIndex = 7;
            this.buttonSwitch.Text = "Switch People In/Out";
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Visible = false;
            this.buttonSwitch.Click += new System.EventHandler(this.SwitchClick);
            // 
            // buttonRotate
            // 
            this.buttonRotate.AutoSize = true;
            this.buttonRotate.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRotate.Enabled = false;
            this.buttonRotate.Location = new System.Drawing.Point(713, 4);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(91, 22);
            this.buttonRotate.TabIndex = 6;
            this.buttonRotate.Text = "Rolate Line";
            this.buttonRotate.UseVisualStyleBackColor = true;
            this.buttonRotate.Visible = false;
            this.buttonRotate.Click += new System.EventHandler(this.RotateClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.AutoSize = true;
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(637, 4);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(76, 22);
            this.buttonRemove.TabIndex = 5;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Visible = false;
            this.buttonRemove.Click += new System.EventHandler(this.RemoveClick);
            // 
            // buttonClear
            // 
            this.buttonClear.AutoSize = true;
            this.buttonClear.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new System.Drawing.Point(576, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(61, 22);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Visible = false;
            this.buttonClear.Click += new System.EventHandler(this.ClearClick);
            // 
            // buttonDrawing
            // 
            this.buttonDrawing.AutoSize = true;
            this.buttonDrawing.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDrawing.Enabled = false;
            this.buttonDrawing.Location = new System.Drawing.Point(500, 4);
            this.buttonDrawing.Name = "buttonDrawing";
            this.buttonDrawing.Size = new System.Drawing.Size(76, 22);
            this.buttonDrawing.TabIndex = 3;
            this.buttonDrawing.Text = "Drawing";
            this.buttonDrawing.UseVisualStyleBackColor = true;
            this.buttonDrawing.Visible = false;
            this.buttonDrawing.Click += new System.EventHandler(this.DrawingClick);
            // 
            // buttonRefreshImage
            // 
            this.buttonRefreshImage.AutoSize = true;
            this.buttonRefreshImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRefreshImage.Enabled = false;
            this.buttonRefreshImage.Location = new System.Drawing.Point(436, 4);
            this.buttonRefreshImage.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonRefreshImage.Name = "buttonRefreshImage";
            this.buttonRefreshImage.Size = new System.Drawing.Size(64, 22);
            this.buttonRefreshImage.TabIndex = 2;
            this.buttonRefreshImage.Text = "Refresh";
            this.buttonRefreshImage.UseVisualStyleBackColor = true;
            this.buttonRefreshImage.Visible = false;
            this.buttonRefreshImage.Click += new System.EventHandler(this.ButtonRefreshImageClick);
            // 
            // buttonDelete
            // 
            this.buttonDelete.AutoSize = true;
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(358, 4);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(78, 22);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Visible = false;
            this.buttonDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // buttonVerify
            // 
            this.buttonVerify.AutoSize = true;
            this.buttonVerify.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonVerify.Location = new System.Drawing.Point(277, 4);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(81, 22);
            this.buttonVerify.TabIndex = 13;
            this.buttonVerify.Text = "Start Verify";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Visible = false;
            this.buttonVerify.Click += new System.EventHandler(this.VerifyClick);
            // 
            // buttonCancelMakingFeature
            // 
            this.buttonCancelMakingFeature.AutoSize = true;
            this.buttonCancelMakingFeature.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCancelMakingFeature.Location = new System.Drawing.Point(186, 4);
            this.buttonCancelMakingFeature.Name = "buttonCancelMakingFeature";
            this.buttonCancelMakingFeature.Size = new System.Drawing.Size(91, 22);
            this.buttonCancelMakingFeature.TabIndex = 15;
            this.buttonCancelMakingFeature.Text = "Cancel";
            this.buttonCancelMakingFeature.UseVisualStyleBackColor = true;
            this.buttonCancelMakingFeature.Visible = false;
            this.buttonCancelMakingFeature.Click += new System.EventHandler(this.CancelMakingFeatureClick);
            // 
            // buttonClearFeature
            // 
            this.buttonClearFeature.AutoSize = true;
            this.buttonClearFeature.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonClearFeature.Location = new System.Drawing.Point(94, 4);
            this.buttonClearFeature.Name = "buttonClearFeature";
            this.buttonClearFeature.Size = new System.Drawing.Size(92, 22);
            this.buttonClearFeature.TabIndex = 16;
            this.buttonClearFeature.Text = "Clear Feature";
            this.buttonClearFeature.UseVisualStyleBackColor = true;
            this.buttonClearFeature.Click += new System.EventHandler(this.ClearFeatureClick);
            // 
            // buttonMakeFeature
            // 
            this.buttonMakeFeature.AutoSize = true;
            this.buttonMakeFeature.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMakeFeature.Location = new System.Drawing.Point(3, 4);
            this.buttonMakeFeature.Name = "buttonMakeFeature";
            this.buttonMakeFeature.Size = new System.Drawing.Size(91, 22);
            this.buttonMakeFeature.TabIndex = 14;
            this.buttonMakeFeature.Text = "Make Feature";
            this.buttonMakeFeature.UseVisualStyleBackColor = true;
            this.buttonMakeFeature.Click += new System.EventHandler(this.MakeFeatureClick);
            // 
            // elementHost
            // 
            this.elementHost.BackColor = System.Drawing.Color.Transparent;
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 30);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(1031, 570);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = this.peopleCountingControl;
            // 
            // PeopleCounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.elementHost);
            this.Controls.Add(this.panelFunction);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PeopleCounting";
            this.Size = new System.Drawing.Size(1031, 600);
            this.panelFunction.ResumeLayout(false);
            this.panelFunction.PerformLayout();
            this.panelDesc.ResumeLayout(false);
            this.panelDesc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreenArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private PeopleCountingControl peopleCountingControl;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonDrawing;
        private System.Windows.Forms.Button buttonRefreshImage;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Panel panelDesc;
        private System.Windows.Forms.Label labelPeopleOut;
        private System.Windows.Forms.PictureBox pictureBoxGreenArrow;
        private System.Windows.Forms.Label labelPeopleIn;
        private System.Windows.Forms.PictureBox pictureBoxRedArrow;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.Button buttonMakeFeature;
        private System.Windows.Forms.Button buttonCancelMakingFeature;
        private System.Windows.Forms.Button buttonClearFeature;
    }
}
