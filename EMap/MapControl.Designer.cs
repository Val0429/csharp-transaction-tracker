using System.Drawing;
using System.Windows.Forms;

namespace EMap
{
    partial class MapControl
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
            this.titlePanel = new PanelBase.DoubleBufferPanel();
            this.minimizePictureBox = new System.Windows.Forms.PictureBox();
            this.panelFunction = new System.Windows.Forms.Panel();
            this.panelVideoWindowSize = new System.Windows.Forms.Panel();
            this.buttonDefaultVideoSize = new System.Windows.Forms.Button();
            this.buttonWindowSizeSmall = new System.Windows.Forms.Button();
            this.buttonWindowSizeMedium = new System.Windows.Forms.Button();
            this.buttonWindowSizeLarge = new System.Windows.Forms.Button();
            this.labelAddMap = new System.Windows.Forms.Label();
            this.panelScaleFunction = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonZoomDefault = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.trackBarForZoom = new System.Windows.Forms.TrackBar();
            this.mapList = new System.Windows.Forms.ComboBox();
            this.panelControl = new PanelBase.DoubleBufferPanel();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.padCom = new EMap.PadControl();
            this.panelScreen = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePictureBox)).BeginInit();
            this.panelFunction.SuspendLayout();
            this.panelVideoWindowSize.SuspendLayout();
            this.panelScaleFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarForZoom)).BeginInit();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImage = global::EMap.Properties.Resources.banner;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Controls.Add(this.minimizePictureBox);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(800, 30);
            this.titlePanel.TabIndex = 5;
            this.titlePanel.Visible = false;
            this.titlePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TitlePanelPaint);
            // 
            // minimizePictureBox
            // 
            this.minimizePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.minimizePictureBox.BackgroundImage = global::EMap.Properties.Resources.mini2;
            this.minimizePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minimizePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizePictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.minimizePictureBox.Location = new System.Drawing.Point(0, 0);
            this.minimizePictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePictureBox.Name = "minimizePictureBox";
            this.minimizePictureBox.Size = new System.Drawing.Size(16, 30);
            this.minimizePictureBox.TabIndex = 7;
            this.minimizePictureBox.TabStop = false;
            this.minimizePictureBox.Visible = false;
            this.minimizePictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MinimizePictureBoxMouseClick);
            // 
            // panelFunction
            // 
            this.panelFunction.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelFunction.BackgroundImage = global::EMap.Properties.Resources.toolBG;
            this.panelFunction.Controls.Add(this.panelVideoWindowSize);
            this.panelFunction.Controls.Add(this.labelAddMap);
            this.panelFunction.Controls.Add(this.panelScaleFunction);
            this.panelFunction.Controls.Add(this.mapList);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFunction.Location = new System.Drawing.Point(0, 30);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(800, 42);
            this.panelFunction.TabIndex = 6;
            // 
            // panelVideoWindowSize
            // 
            this.panelVideoWindowSize.BackgroundImage = global::EMap.Properties.Resources.toolBG;
            this.panelVideoWindowSize.Controls.Add(this.buttonDefaultVideoSize);
            this.panelVideoWindowSize.Controls.Add(this.buttonWindowSizeSmall);
            this.panelVideoWindowSize.Controls.Add(this.buttonWindowSizeMedium);
            this.panelVideoWindowSize.Controls.Add(this.buttonWindowSizeLarge);
            this.panelVideoWindowSize.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelVideoWindowSize.Location = new System.Drawing.Point(622, 0);
            this.panelVideoWindowSize.Name = "panelVideoWindowSize";
            this.panelVideoWindowSize.Size = new System.Drawing.Size(178, 42);
            this.panelVideoWindowSize.TabIndex = 8;
            // 
            // buttonDefaultVideoSize
            // 
            this.buttonDefaultVideoSize.BackColor = System.Drawing.Color.Transparent;
            this.buttonDefaultVideoSize.BackgroundImage = global::EMap.Properties.Resources.ScreenDefault;
            this.buttonDefaultVideoSize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonDefaultVideoSize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonDefaultVideoSize.FlatAppearance.BorderSize = 0;
            this.buttonDefaultVideoSize.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonDefaultVideoSize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDefaultVideoSize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonDefaultVideoSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefaultVideoSize.ForeColor = System.Drawing.Color.Black;
            this.buttonDefaultVideoSize.Location = new System.Drawing.Point(7, 11);
            this.buttonDefaultVideoSize.Name = "buttonDefaultVideoSize";
            this.buttonDefaultVideoSize.Size = new System.Drawing.Size(39, 24);
            this.buttonDefaultVideoSize.TabIndex = 17;
            this.buttonDefaultVideoSize.UseVisualStyleBackColor = false;
            this.buttonDefaultVideoSize.Click += new System.EventHandler(this.WindowSizeChangeClick);
            // 
            // buttonWindowSizeSmall
            // 
            this.buttonWindowSizeSmall.BackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeSmall.BackgroundImage = global::EMap.Properties.Resources.small;
            this.buttonWindowSizeSmall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonWindowSizeSmall.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonWindowSizeSmall.FlatAppearance.BorderSize = 0;
            this.buttonWindowSizeSmall.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeSmall.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeSmall.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWindowSizeSmall.ForeColor = System.Drawing.Color.Black;
            this.buttonWindowSizeSmall.Location = new System.Drawing.Point(51, 11);
            this.buttonWindowSizeSmall.Name = "buttonWindowSizeSmall";
            this.buttonWindowSizeSmall.Size = new System.Drawing.Size(30, 26);
            this.buttonWindowSizeSmall.TabIndex = 16;
            this.buttonWindowSizeSmall.UseVisualStyleBackColor = false;
            this.buttonWindowSizeSmall.Click += new System.EventHandler(this.WindowSizeChangeClick);
            // 
            // buttonWindowSizeMedium
            // 
            this.buttonWindowSizeMedium.BackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeMedium.BackgroundImage = global::EMap.Properties.Resources.medium;
            this.buttonWindowSizeMedium.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonWindowSizeMedium.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonWindowSizeMedium.FlatAppearance.BorderSize = 0;
            this.buttonWindowSizeMedium.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeMedium.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeMedium.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeMedium.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWindowSizeMedium.ForeColor = System.Drawing.Color.Black;
            this.buttonWindowSizeMedium.Location = new System.Drawing.Point(88, 10);
            this.buttonWindowSizeMedium.Name = "buttonWindowSizeMedium";
            this.buttonWindowSizeMedium.Size = new System.Drawing.Size(30, 26);
            this.buttonWindowSizeMedium.TabIndex = 15;
            this.buttonWindowSizeMedium.UseVisualStyleBackColor = false;
            this.buttonWindowSizeMedium.Click += new System.EventHandler(this.WindowSizeChangeClick);
            // 
            // buttonWindowSizeLarge
            // 
            this.buttonWindowSizeLarge.BackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeLarge.BackgroundImage = global::EMap.Properties.Resources.large;
            this.buttonWindowSizeLarge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonWindowSizeLarge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonWindowSizeLarge.FlatAppearance.BorderSize = 0;
            this.buttonWindowSizeLarge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeLarge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeLarge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonWindowSizeLarge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWindowSizeLarge.ForeColor = System.Drawing.Color.Black;
            this.buttonWindowSizeLarge.Location = new System.Drawing.Point(128, 10);
            this.buttonWindowSizeLarge.Name = "buttonWindowSizeLarge";
            this.buttonWindowSizeLarge.Size = new System.Drawing.Size(30, 26);
            this.buttonWindowSizeLarge.TabIndex = 13;
            this.buttonWindowSizeLarge.UseVisualStyleBackColor = false;
            this.buttonWindowSizeLarge.Click += new System.EventHandler(this.WindowSizeChangeClick);
            // 
            // labelAddMap
            // 
            this.labelAddMap.AutoSize = true;
            this.labelAddMap.BackColor = System.Drawing.Color.Transparent;
            this.labelAddMap.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddMap.ForeColor = System.Drawing.SystemColors.Control;
            this.labelAddMap.Location = new System.Drawing.Point(30, 15);
            this.labelAddMap.Name = "labelAddMap";
            this.labelAddMap.Size = new System.Drawing.Size(128, 15);
            this.labelAddMap.TabIndex = 7;
            this.labelAddMap.Text = "Please add new map.";
            // 
            // panelScaleFunction
            // 
            this.panelScaleFunction.BackgroundImage = global::EMap.Properties.Resources.toolBG;
            this.panelScaleFunction.Controls.Add(this.buttonBack);
            this.panelScaleFunction.Controls.Add(this.buttonRight);
            this.panelScaleFunction.Controls.Add(this.buttonLeft);
            this.panelScaleFunction.Controls.Add(this.buttonDown);
            this.panelScaleFunction.Controls.Add(this.buttonUp);
            this.panelScaleFunction.Controls.Add(this.buttonZoomDefault);
            this.panelScaleFunction.Controls.Add(this.buttonZoomIn);
            this.panelScaleFunction.Controls.Add(this.buttonZoomOut);
            this.panelScaleFunction.Controls.Add(this.trackBarForZoom);
            this.panelScaleFunction.Location = new System.Drawing.Point(181, 0);
            this.panelScaleFunction.Name = "panelScaleFunction";
            this.panelScaleFunction.Size = new System.Drawing.Size(451, 42);
            this.panelScaleFunction.TabIndex = 6;
            this.panelScaleFunction.Visible = false;
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.Transparent;
            this.buttonBack.BackgroundImage = global::EMap.Properties.Resources.original;
            this.buttonBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBack.FlatAppearance.BorderSize = 0;
            this.buttonBack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Location = new System.Drawing.Point(417, 9);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(27, 28);
            this.buttonBack.TabIndex = 12;
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.MapBackClick);
            // 
            // buttonRight
            // 
            this.buttonRight.BackColor = System.Drawing.Color.Transparent;
            this.buttonRight.BackgroundImage = global::EMap.Properties.Resources.arrow_right;
            this.buttonRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRight.FlatAppearance.BorderSize = 0;
            this.buttonRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.Location = new System.Drawing.Point(320, 10);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(26, 25);
            this.buttonRight.TabIndex = 11;
            this.buttonRight.UseVisualStyleBackColor = false;
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackColor = System.Drawing.Color.Transparent;
            this.buttonLeft.BackgroundImage = global::EMap.Properties.Resources.arrow_left;
            this.buttonLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLeft.FlatAppearance.BorderSize = 0;
            this.buttonLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.Location = new System.Drawing.Point(287, 10);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(26, 25);
            this.buttonLeft.TabIndex = 10;
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.Click += new System.EventHandler(this.ButtonMapMoveClick);
            // 
            // buttonDown
            // 
            this.buttonDown.BackColor = System.Drawing.Color.Transparent;
            this.buttonDown.BackgroundImage = global::EMap.Properties.Resources.arrow_down;
            this.buttonDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonDown.FlatAppearance.BorderSize = 0;
            this.buttonDown.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDown.Location = new System.Drawing.Point(384, 10);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(26, 25);
            this.buttonDown.TabIndex = 9;
            this.buttonDown.UseVisualStyleBackColor = false;
            // 
            // buttonUp
            // 
            this.buttonUp.BackColor = System.Drawing.Color.Transparent;
            this.buttonUp.BackgroundImage = global::EMap.Properties.Resources.arrow_up;
            this.buttonUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonUp.FlatAppearance.BorderSize = 0;
            this.buttonUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Location = new System.Drawing.Point(352, 10);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(26, 25);
            this.buttonUp.TabIndex = 8;
            this.buttonUp.UseVisualStyleBackColor = false;
            // 
            // buttonZoomDefault
            // 
            this.buttonZoomDefault.BackColor = System.Drawing.Color.Transparent;
            this.buttonZoomDefault.BackgroundImage = global::EMap.Properties.Resources.zoom1_1;
            this.buttonZoomDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZoomDefault.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonZoomDefault.FlatAppearance.BorderSize = 0;
            this.buttonZoomDefault.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomDefault.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomDefault.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomDefault.Location = new System.Drawing.Point(237, 10);
            this.buttonZoomDefault.Name = "buttonZoomDefault";
            this.buttonZoomDefault.Size = new System.Drawing.Size(24, 24);
            this.buttonZoomDefault.TabIndex = 7;
            this.buttonZoomDefault.UseVisualStyleBackColor = false;
            this.buttonZoomDefault.Click += new System.EventHandler(this.ZoomDefaultClick);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.buttonZoomIn.BackgroundImage = global::EMap.Properties.Resources.zoom_in;
            this.buttonZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZoomIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonZoomIn.FlatAppearance.BorderSize = 0;
            this.buttonZoomIn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomIn.Location = new System.Drawing.Point(211, 10);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(24, 24);
            this.buttonZoomIn.TabIndex = 6;
            this.buttonZoomIn.UseVisualStyleBackColor = false;
            this.buttonZoomIn.Click += new System.EventHandler(this.ZoomInClick);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.buttonZoomOut.BackgroundImage = global::EMap.Properties.Resources.zoom_out;
            this.buttonZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZoomOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonZoomOut.FlatAppearance.BorderSize = 0;
            this.buttonZoomOut.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomOut.Location = new System.Drawing.Point(13, 10);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(24, 24);
            this.buttonZoomOut.TabIndex = 5;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.ZoomOutClick);
            // 
            // trackBarForZoom
            // 
            this.trackBarForZoom.AutoSize = false;
            this.trackBarForZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(61)))), ((int)(((byte)(68)))));
            this.trackBarForZoom.LargeChange = 1;
            this.trackBarForZoom.Location = new System.Drawing.Point(33, 7);
            this.trackBarForZoom.Maximum = 30;
            this.trackBarForZoom.Minimum = 2;
            this.trackBarForZoom.Name = "trackBarForZoom";
            this.trackBarForZoom.Size = new System.Drawing.Size(183, 25);
            this.trackBarForZoom.TabIndex = 4;
            this.trackBarForZoom.Value = 5;
            this.trackBarForZoom.ValueChanged += new System.EventHandler(this.TrackBarForZoomScroll);
            // 
            // mapList
            // 
            this.mapList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mapList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapList.FormattingEnabled = true;
            this.mapList.Location = new System.Drawing.Point(8, 11);
            this.mapList.Name = "mapList";
            this.mapList.Size = new System.Drawing.Size(172, 23);
            this.mapList.TabIndex = 5;
            this.mapList.Visible = false;
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.Black;
            this.panelControl.Controls.Add(this.elementHost1);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 72);
            this.panelControl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(800, 196);
            this.panelControl.TabIndex = 0;
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.Black;
            this.elementHost1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(800, 196);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Child = this.padCom;
            // 
            // panelScreen
            // 
            this.panelScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelScreen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelScreen.Location = new System.Drawing.Point(0, 268);
            this.panelScreen.Margin = new System.Windows.Forms.Padding(0);
            this.panelScreen.Name = "panelScreen";
            this.panelScreen.Size = new System.Drawing.Size(800, 332);
            this.panelScreen.TabIndex = 8;
            this.panelScreen.Visible = false;
            // 
            // MapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelScreen);
            this.Controls.Add(this.panelFunction);
            this.Controls.Add(this.titlePanel);
            this.Name = "MapControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimizePictureBox)).EndInit();
            this.panelFunction.ResumeLayout(false);
            this.panelFunction.PerformLayout();
            this.panelVideoWindowSize.ResumeLayout(false);
            this.panelScaleFunction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarForZoom)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel titlePanel;
        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.TrackBar trackBarForZoom;
        private System.Windows.Forms.ComboBox mapList;
        private PanelBase.DoubleBufferPanel panelControl;
        private PanelBase.DoubleBufferFlowLayoutPanel panelScreen;
        private System.Windows.Forms.PictureBox minimizePictureBox;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private PadControl padCom;
        private System.Windows.Forms.Panel panelScaleFunction;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonZoomDefault;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelAddMap;
        private System.Windows.Forms.Panel panelVideoWindowSize;
        private System.Windows.Forms.Button buttonWindowSizeLarge;
        private System.Windows.Forms.Button buttonWindowSizeSmall;
        private System.Windows.Forms.Button buttonWindowSizeMedium;
        private System.Windows.Forms.Button buttonDefaultVideoSize;
    }
}
