using System.Windows;

namespace PeopleCounting
{
    sealed partial class PeopleCountingPanel
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
            this.panelFunction = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonMakeFeature = new System.Windows.Forms.Button();
            this.buttonDeleteFeature = new System.Windows.Forms.Button();
            this.buttonCancelMakingFeature = new System.Windows.Forms.Button();
            this.separatedLabel = new System.Windows.Forms.Label();
            this.buttonDrawing = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.buttonVerify = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.panelDesc = new System.Windows.Forms.Panel();
            this.pictureBoxPeopleOutArrow = new System.Windows.Forms.PictureBox();
            this.labelPeopleOut = new System.Windows.Forms.Label();
            this.pictureBoxPeopleInArrow = new System.Windows.Forms.PictureBox();
            this.labelPeopleIn = new System.Windows.Forms.Label();
            this.separatedLabel2 = new System.Windows.Forms.Label();
            this.buttonRefreshImage = new System.Windows.Forms.Button();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.peopleCountingControl = new PeopleCounting.PeopleCountingControl();
            this.panelFunction.SuspendLayout();
            this.panelDesc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeopleOutArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeopleInArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFunction
            // 
            this.panelFunction.BackColor = System.Drawing.Color.White;
            this.panelFunction.Controls.Add(this.buttonMakeFeature);
            this.panelFunction.Controls.Add(this.buttonDeleteFeature);
            this.panelFunction.Controls.Add(this.buttonCancelMakingFeature);
            this.panelFunction.Controls.Add(this.separatedLabel);
            this.panelFunction.Controls.Add(this.buttonDrawing);
            this.panelFunction.Controls.Add(this.buttonRemove);
            this.panelFunction.Controls.Add(this.buttonClear);
            this.panelFunction.Controls.Add(this.buttonRotate);
            this.panelFunction.Controls.Add(this.buttonVerify);
            this.panelFunction.Controls.Add(this.buttonSave);
            this.panelFunction.Controls.Add(this.buttonSwitch);
            this.panelFunction.Controls.Add(this.panelDesc);
            this.panelFunction.Controls.Add(this.separatedLabel2);
            this.panelFunction.Controls.Add(this.buttonRefreshImage);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFunction.Location = new System.Drawing.Point(0, 0);
            this.panelFunction.Margin = new System.Windows.Forms.Padding(0);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(1438, 30);
            this.panelFunction.TabIndex = 0;
            // 
            // buttonMakeFeature
            // 
            this.buttonMakeFeature.AutoSize = true;
            this.buttonMakeFeature.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMakeFeature.Location = new System.Drawing.Point(3, 3);
            this.buttonMakeFeature.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonMakeFeature.Name = "buttonMakeFeature";
            this.buttonMakeFeature.Size = new System.Drawing.Size(91, 25);
            this.buttonMakeFeature.TabIndex = 14;
            this.buttonMakeFeature.Text = "Make Feature";
            this.buttonMakeFeature.UseVisualStyleBackColor = true;
            this.buttonMakeFeature.Click += new System.EventHandler(this.MakeFeatureClick);
            // 
            // buttonDeleteFeature
            // 
            this.buttonDeleteFeature.AutoSize = true;
            this.buttonDeleteFeature.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDeleteFeature.Location = new System.Drawing.Point(100, 3);
            this.buttonDeleteFeature.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonDeleteFeature.Name = "buttonDeleteFeature";
            this.buttonDeleteFeature.Size = new System.Drawing.Size(105, 25);
            this.buttonDeleteFeature.TabIndex = 16;
            this.buttonDeleteFeature.Text = "Delete Features";
            this.buttonDeleteFeature.UseVisualStyleBackColor = true;
            this.buttonDeleteFeature.Visible = false;
            this.buttonDeleteFeature.Click += new System.EventHandler(this.ClearFeatureClick);
            // 
            // buttonCancelMakingFeature
            // 
            this.buttonCancelMakingFeature.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancelMakingFeature.Location = new System.Drawing.Point(211, 3);
            this.buttonCancelMakingFeature.Name = "buttonCancelMakingFeature";
            this.buttonCancelMakingFeature.Size = new System.Drawing.Size(78, 25);
            this.buttonCancelMakingFeature.TabIndex = 15;
            this.buttonCancelMakingFeature.Text = "Cancel";
            this.buttonCancelMakingFeature.UseVisualStyleBackColor = true;
            this.buttonCancelMakingFeature.Visible = false;
            this.buttonCancelMakingFeature.Click += new System.EventHandler(this.CancelMakingFeatureClick);
            // 
            // separatedLabel
            // 
            this.separatedLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.separatedLabel.Image = global::PeopleCounting.Properties.Resources.separator;
            this.separatedLabel.Location = new System.Drawing.Point(292, 0);
            this.separatedLabel.Margin = new System.Windows.Forms.Padding(0);
            this.separatedLabel.Name = "separatedLabel";
            this.separatedLabel.Size = new System.Drawing.Size(20, 31);
            this.separatedLabel.TabIndex = 17;
            this.separatedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDrawing
            // 
            this.buttonDrawing.AutoSize = true;
            this.buttonDrawing.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDrawing.Enabled = false;
            this.buttonDrawing.Location = new System.Drawing.Point(315, 3);
            this.buttonDrawing.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonDrawing.Name = "buttonDrawing";
            this.buttonDrawing.Size = new System.Drawing.Size(90, 25);
            this.buttonDrawing.TabIndex = 3;
            this.buttonDrawing.Text = "Draw Region";
            this.buttonDrawing.UseVisualStyleBackColor = true;
            this.buttonDrawing.Visible = false;
            this.buttonDrawing.Click += new System.EventHandler(this.DrawingClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.AutoSize = true;
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(411, 3);
            this.buttonRemove.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(90, 25);
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
            this.buttonClear.Location = new System.Drawing.Point(507, 3);
            this.buttonClear.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(90, 25);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Visible = false;
            this.buttonClear.Click += new System.EventHandler(this.ClearClick);
            // 
            // buttonRotate
            // 
            this.buttonRotate.AutoSize = true;
            this.buttonRotate.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRotate.Enabled = false;
            this.buttonRotate.Location = new System.Drawing.Point(603, 3);
            this.buttonRotate.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(91, 25);
            this.buttonRotate.TabIndex = 6;
            this.buttonRotate.Text = "Rolate Line";
            this.buttonRotate.UseVisualStyleBackColor = true;
            this.buttonRotate.Visible = false;
            this.buttonRotate.Click += new System.EventHandler(this.RotateClick);
            // 
            // buttonVerify
            // 
            this.buttonVerify.AutoSize = true;
            this.buttonVerify.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonVerify.Location = new System.Drawing.Point(700, 3);
            this.buttonVerify.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(90, 25);
            this.buttonVerify.TabIndex = 13;
            this.buttonVerify.Text = "Start Verify";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Visible = false;
            this.buttonVerify.Click += new System.EventHandler(this.VerifyClick);
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSave.Location = new System.Drawing.Point(796, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(78, 25);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.SaveClick);
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.AutoSize = true;
            this.buttonSwitch.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSwitch.Enabled = false;
            this.buttonSwitch.Location = new System.Drawing.Point(880, 3);
            this.buttonSwitch.MinimumSize = new System.Drawing.Size(90, 22);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(151, 25);
            this.buttonSwitch.TabIndex = 7;
            this.buttonSwitch.Text = "Switch People In/Out";
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Visible = false;
            this.buttonSwitch.Click += new System.EventHandler(this.SwitchClick);
            // 
            // panelDesc
            // 
            this.panelDesc.AutoSize = true;
            this.panelDesc.Controls.Add(this.pictureBoxPeopleOutArrow);
            this.panelDesc.Controls.Add(this.labelPeopleOut);
            this.panelDesc.Controls.Add(this.pictureBoxPeopleInArrow);
            this.panelDesc.Controls.Add(this.labelPeopleIn);
            this.panelDesc.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDesc.Location = new System.Drawing.Point(1037, 3);
            this.panelDesc.Name = "panelDesc";
            this.panelDesc.Size = new System.Drawing.Size(203, 25);
            this.panelDesc.TabIndex = 12;
            // 
            // pictureBoxGreenArrow
            // 
            this.pictureBoxPeopleOutArrow.BackgroundImage = global::PeopleCounting.Properties.Resources.arrowPeopleOut;
            this.pictureBoxPeopleOutArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxPeopleOutArrow.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPeopleOutArrow.Location = new System.Drawing.Point(168, 0);
            this.pictureBoxPeopleOutArrow.Name = "pictureBoxPeopleOutArrow";
            this.pictureBoxPeopleOutArrow.Size = new System.Drawing.Size(35, 25);
            this.pictureBoxPeopleOutArrow.TabIndex = 8;
            this.pictureBoxPeopleOutArrow.TabStop = false;
            this.pictureBoxPeopleOutArrow.Visible = false;
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
            this.pictureBoxPeopleInArrow.BackgroundImage = global::PeopleCounting.Properties.Resources.arrowPeopleIn;
            this.pictureBoxPeopleInArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxPeopleInArrow.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPeopleInArrow.Location = new System.Drawing.Point(59, 0);
            this.pictureBoxPeopleInArrow.Name = "pictureBoxPeopleInArrow";
            this.pictureBoxPeopleInArrow.Size = new System.Drawing.Size(41, 25);
            this.pictureBoxPeopleInArrow.TabIndex = 5;
            this.pictureBoxPeopleInArrow.TabStop = false;
            this.pictureBoxPeopleInArrow.Visible = false;
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
            // separatedLabel2
            // 
            this.separatedLabel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.separatedLabel2.Image = global::PeopleCounting.Properties.Resources.separator;
            this.separatedLabel2.Location = new System.Drawing.Point(1243, 0);
            this.separatedLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.separatedLabel2.Name = "separatedLabel2";
            this.separatedLabel2.Size = new System.Drawing.Size(20, 31);
            this.separatedLabel2.TabIndex = 18;
            this.separatedLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRefreshImage
            // 
            this.buttonRefreshImage.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRefreshImage.Location = new System.Drawing.Point(1266, 3);
            this.buttonRefreshImage.Name = "buttonRefreshImage";
            this.buttonRefreshImage.Size = new System.Drawing.Size(78, 25);
            this.buttonRefreshImage.TabIndex = 2;
            this.buttonRefreshImage.Text = "Refresh";
            this.buttonRefreshImage.UseVisualStyleBackColor = true;
            this.buttonRefreshImage.Visible = false;
            this.buttonRefreshImage.Click += new System.EventHandler(this.ButtonRefreshImageClick);
            // 
            // elementHost
            // 
            this.elementHost.BackColor = System.Drawing.Color.Transparent;
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 30);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(1438, 570);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = this.peopleCountingControl;
            // 
            // PeopleCountingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.elementHost);
            this.Controls.Add(this.panelFunction);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PeopleCountingPanel";
            this.Size = new System.Drawing.Size(1438, 600);
            this.panelFunction.ResumeLayout(false);
            this.panelFunction.PerformLayout();
            this.panelDesc.ResumeLayout(false);
            this.panelDesc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeopleOutArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPeopleInArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelFunction;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private PeopleCountingControl peopleCountingControl;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonDrawing;
        private System.Windows.Forms.Button buttonRefreshImage;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelDesc;
        private System.Windows.Forms.Label labelPeopleOut;
        private System.Windows.Forms.PictureBox pictureBoxPeopleOutArrow;
        private System.Windows.Forms.Label labelPeopleIn;
        private System.Windows.Forms.PictureBox pictureBoxPeopleInArrow;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.Button buttonMakeFeature;
        private System.Windows.Forms.Button buttonCancelMakingFeature;
        private System.Windows.Forms.Button buttonDeleteFeature;
        private System.Windows.Forms.Label separatedLabel;
        private System.Windows.Forms.Label separatedLabel2;
    }
}
