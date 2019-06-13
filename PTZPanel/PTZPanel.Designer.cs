using System.Drawing;

namespace PTZPanel
{
	partial class PTZPanel
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.ptzControlPanel = new System.Windows.Forms.Panel();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.enablePTZPatrolCheckBox = new System.Windows.Forms.CheckBox();
            this.RegionsComboBox = new System.Windows.Forms.ComboBox();
            this.patrolLabel = new System.Windows.Forms.Label();
            this.autoTrackingCheckBox = new System.Windows.Forms.CheckBox();
            this.enableTourCheckBox = new System.Windows.Forms.CheckBox();
            this.presetPointcomboBox = new System.Windows.Forms.ComboBox();
            this.presetPointLabel = new System.Windows.Forms.Label();
            this.focusPanel = new System.Windows.Forms.Panel();
            this.incressFocusButton = new System.Windows.Forms.Button();
            this.recudeFocusButton = new System.Windows.Forms.Button();
            this.focusLabel = new System.Windows.Forms.Label();
            this.zoomPanel = new System.Windows.Forms.Panel();
            this.incressZoomButton = new System.Windows.Forms.Button();
            this.reduceZoomButton = new System.Windows.Forms.Button();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.controllerPanel = new System.Windows.Forms.Panel();
            this.moverPictureBox = new System.Windows.Forms.PictureBox();
            this.leftPictureBox = new System.Windows.Forms.PictureBox();
            this.rightPictureBox = new System.Windows.Forms.PictureBox();
            this.downPictureBox = new System.Windows.Forms.PictureBox();
            this.upPictureBox = new System.Windows.Forms.PictureBox();
            this.ptzControlPanel.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.focusPanel.SuspendLayout();
            this.zoomPanel.SuspendLayout();
            this.controllerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moverPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(220, 42);
            this.titlePanel.TabIndex = 2;
            // 
            // ptzControlPanel
            // 
            this.ptzControlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.ptzControlPanel.BackgroundImage = global::PTZPanel.Properties.Resources.ptzBG;
            this.ptzControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptzControlPanel.Controls.Add(this.toolPanel);
            this.ptzControlPanel.Controls.Add(this.controllerPanel);
            this.ptzControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptzControlPanel.Location = new System.Drawing.Point(0, 42);
            this.ptzControlPanel.Name = "ptzControlPanel";
            this.ptzControlPanel.Size = new System.Drawing.Size(220, 435);
            this.ptzControlPanel.TabIndex = 3;
            // 
            // toolPanel
            // 
            this.toolPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolPanel.BackColor = System.Drawing.Color.Transparent;
            this.toolPanel.BackgroundImage = global::PTZPanel.Properties.Resources.toolBG;
            this.toolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolPanel.Controls.Add(this.enablePTZPatrolCheckBox);
            this.toolPanel.Controls.Add(this.RegionsComboBox);
            this.toolPanel.Controls.Add(this.patrolLabel);
            this.toolPanel.Controls.Add(this.autoTrackingCheckBox);
            this.toolPanel.Controls.Add(this.enableTourCheckBox);
            this.toolPanel.Controls.Add(this.presetPointcomboBox);
            this.toolPanel.Controls.Add(this.presetPointLabel);
            this.toolPanel.Controls.Add(this.focusPanel);
            this.toolPanel.Controls.Add(this.zoomPanel);
            this.toolPanel.Location = new System.Drawing.Point(0, 201);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(220, 242);
            this.toolPanel.TabIndex = 6;
            // 
            // enablePTZPatrolCheckBox
            // 
            this.enablePTZPatrolCheckBox.AutoSize = true;
            this.enablePTZPatrolCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enablePTZPatrolCheckBox.ForeColor = System.Drawing.Color.White;
            this.enablePTZPatrolCheckBox.Location = new System.Drawing.Point(30, 206);
            this.enablePTZPatrolCheckBox.Name = "enablePTZPatrolCheckBox";
            this.enablePTZPatrolCheckBox.Size = new System.Drawing.Size(160, 19);
            this.enablePTZPatrolCheckBox.TabIndex = 14;
            this.enablePTZPatrolCheckBox.Text = "Enable digital PTZ patrol";
            this.enablePTZPatrolCheckBox.UseVisualStyleBackColor = true;
            // 
            // RegionsComboBox
            // 
            this.RegionsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RegionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegionsComboBox.Enabled = false;
            this.RegionsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.RegionsComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegionsComboBox.FormattingEnabled = true;
            this.RegionsComboBox.Location = new System.Drawing.Point(29, 178);
            this.RegionsComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.RegionsComboBox.Name = "RegionsComboBox";
            this.RegionsComboBox.Size = new System.Drawing.Size(162, 23);
            this.RegionsComboBox.TabIndex = 13;
            // 
            // patrolLabel
            // 
            this.patrolLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patrolLabel.BackColor = System.Drawing.Color.Transparent;
            this.patrolLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patrolLabel.ForeColor = System.Drawing.Color.White;
            this.patrolLabel.Location = new System.Drawing.Point(15, 153);
            this.patrolLabel.Margin = new System.Windows.Forms.Padding(0);
            this.patrolLabel.Name = "patrolLabel";
            this.patrolLabel.Size = new System.Drawing.Size(190, 25);
            this.patrolLabel.TabIndex = 12;
            this.patrolLabel.Text = "Digital PTZ Regions";
            this.patrolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoTrackingCheckBox
            // 
            this.autoTrackingCheckBox.AutoSize = true;
            this.autoTrackingCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoTrackingCheckBox.ForeColor = System.Drawing.Color.White;
            this.autoTrackingCheckBox.Location = new System.Drawing.Point(47, 52);
            this.autoTrackingCheckBox.Name = "autoTrackingCheckBox";
            this.autoTrackingCheckBox.Size = new System.Drawing.Size(100, 19);
            this.autoTrackingCheckBox.TabIndex = 11;
            this.autoTrackingCheckBox.Text = "Auto Tracking";
            this.autoTrackingCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableTourCheckBox
            // 
            this.enableTourCheckBox.AutoSize = true;
            this.enableTourCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableTourCheckBox.ForeColor = System.Drawing.Color.White;
            this.enableTourCheckBox.Location = new System.Drawing.Point(30, 131);
            this.enableTourCheckBox.Name = "enableTourCheckBox";
            this.enableTourCheckBox.Size = new System.Drawing.Size(127, 19);
            this.enableTourCheckBox.TabIndex = 10;
            this.enableTourCheckBox.Text = "Enable preset tour";
            this.enableTourCheckBox.UseVisualStyleBackColor = true;
            // 
            // presetPointcomboBox
            // 
            this.presetPointcomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presetPointcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetPointcomboBox.Enabled = false;
            this.presetPointcomboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.presetPointcomboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetPointcomboBox.FormattingEnabled = true;
            this.presetPointcomboBox.Location = new System.Drawing.Point(29, 103);
            this.presetPointcomboBox.Margin = new System.Windows.Forms.Padding(0);
            this.presetPointcomboBox.Name = "presetPointcomboBox";
            this.presetPointcomboBox.Size = new System.Drawing.Size(162, 23);
            this.presetPointcomboBox.TabIndex = 9;
            // 
            // presetPointLabel
            // 
            this.presetPointLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presetPointLabel.BackColor = System.Drawing.Color.Transparent;
            this.presetPointLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetPointLabel.ForeColor = System.Drawing.Color.White;
            this.presetPointLabel.Location = new System.Drawing.Point(15, 78);
            this.presetPointLabel.Margin = new System.Windows.Forms.Padding(0);
            this.presetPointLabel.Name = "presetPointLabel";
            this.presetPointLabel.Size = new System.Drawing.Size(190, 25);
            this.presetPointLabel.TabIndex = 8;
            this.presetPointLabel.Text = "Preset Point";
            this.presetPointLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // focusPanel
            // 
            this.focusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusPanel.BackColor = System.Drawing.Color.Transparent;
            this.focusPanel.Controls.Add(this.incressFocusButton);
            this.focusPanel.Controls.Add(this.recudeFocusButton);
            this.focusPanel.Controls.Add(this.focusLabel);
            this.focusPanel.Location = new System.Drawing.Point(15, 24);
            this.focusPanel.Name = "focusPanel";
            this.focusPanel.Size = new System.Drawing.Size(190, 23);
            this.focusPanel.TabIndex = 7;
            // 
            // incressFocusButton
            // 
            this.incressFocusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.incressFocusButton.FlatAppearance.BorderSize = 0;
            this.incressFocusButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.incressFocusButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.incressFocusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.incressFocusButton.Image = global::PTZPanel.Properties.Resources.incress;
            this.incressFocusButton.Location = new System.Drawing.Point(120, 0);
            this.incressFocusButton.Margin = new System.Windows.Forms.Padding(0);
            this.incressFocusButton.Name = "incressFocusButton";
            this.incressFocusButton.Size = new System.Drawing.Size(30, 30);
            this.incressFocusButton.TabIndex = 5;
            this.incressFocusButton.UseVisualStyleBackColor = true;
            // 
            // recudeFocusButton
            // 
            this.recudeFocusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recudeFocusButton.FlatAppearance.BorderSize = 0;
            this.recudeFocusButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.recudeFocusButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.recudeFocusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.recudeFocusButton.Image = global::PTZPanel.Properties.Resources.reduce;
            this.recudeFocusButton.Location = new System.Drawing.Point(83, 0);
            this.recudeFocusButton.Margin = new System.Windows.Forms.Padding(0);
            this.recudeFocusButton.Name = "recudeFocusButton";
            this.recudeFocusButton.Size = new System.Drawing.Size(30, 30);
            this.recudeFocusButton.TabIndex = 6;
            this.recudeFocusButton.UseVisualStyleBackColor = true;
            // 
            // focusLabel
            // 
            this.focusLabel.BackColor = System.Drawing.Color.Transparent;
            this.focusLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.focusLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.focusLabel.ForeColor = System.Drawing.Color.White;
            this.focusLabel.Location = new System.Drawing.Point(0, 0);
            this.focusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.focusLabel.Name = "focusLabel";
            this.focusLabel.Size = new System.Drawing.Size(70, 23);
            this.focusLabel.TabIndex = 1;
            this.focusLabel.Text = "Focus";
            this.focusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // zoomPanel
            // 
            this.zoomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomPanel.BackColor = System.Drawing.Color.Transparent;
            this.zoomPanel.Controls.Add(this.incressZoomButton);
            this.zoomPanel.Controls.Add(this.reduceZoomButton);
            this.zoomPanel.Controls.Add(this.zoomLabel);
            this.zoomPanel.Location = new System.Drawing.Point(15, 1);
            this.zoomPanel.Name = "zoomPanel";
            this.zoomPanel.Size = new System.Drawing.Size(190, 27);
            this.zoomPanel.TabIndex = 6;
            // 
            // incressZoomButton
            // 
            this.incressZoomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.incressZoomButton.FlatAppearance.BorderSize = 0;
            this.incressZoomButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.incressZoomButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.incressZoomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.incressZoomButton.Image = global::PTZPanel.Properties.Resources.incress;
            this.incressZoomButton.Location = new System.Drawing.Point(120, 0);
            this.incressZoomButton.Margin = new System.Windows.Forms.Padding(0);
            this.incressZoomButton.Name = "incressZoomButton";
            this.incressZoomButton.Size = new System.Drawing.Size(30, 30);
            this.incressZoomButton.TabIndex = 6;
            this.incressZoomButton.UseVisualStyleBackColor = true;
            // 
            // reduceZoomButton
            // 
            this.reduceZoomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reduceZoomButton.FlatAppearance.BorderSize = 0;
            this.reduceZoomButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.reduceZoomButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.reduceZoomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reduceZoomButton.Image = global::PTZPanel.Properties.Resources.reduce;
            this.reduceZoomButton.Location = new System.Drawing.Point(83, 0);
            this.reduceZoomButton.Margin = new System.Windows.Forms.Padding(0);
            this.reduceZoomButton.Name = "reduceZoomButton";
            this.reduceZoomButton.Size = new System.Drawing.Size(30, 30);
            this.reduceZoomButton.TabIndex = 7;
            this.reduceZoomButton.UseVisualStyleBackColor = true;
            // 
            // zoomLabel
            // 
            this.zoomLabel.BackColor = System.Drawing.Color.Transparent;
            this.zoomLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.zoomLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomLabel.ForeColor = System.Drawing.Color.White;
            this.zoomLabel.Location = new System.Drawing.Point(0, 0);
            this.zoomLabel.Margin = new System.Windows.Forms.Padding(0);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(70, 27);
            this.zoomLabel.TabIndex = 1;
            this.zoomLabel.Text = "Zoom";
            this.zoomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // controllerPanel
            // 
            this.controllerPanel.BackColor = System.Drawing.Color.Transparent;
            this.controllerPanel.BackgroundImage = global::PTZPanel.Properties.Resources.controllerBG2;
            this.controllerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.controllerPanel.Controls.Add(this.moverPictureBox);
            this.controllerPanel.Controls.Add(this.leftPictureBox);
            this.controllerPanel.Controls.Add(this.rightPictureBox);
            this.controllerPanel.Controls.Add(this.downPictureBox);
            this.controllerPanel.Controls.Add(this.upPictureBox);
            this.controllerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controllerPanel.Location = new System.Drawing.Point(0, 0);
            this.controllerPanel.Name = "controllerPanel";
            this.controllerPanel.Size = new System.Drawing.Size(220, 200);
            this.controllerPanel.TabIndex = 0;
            // 
            // moverPictureBox
            // 
            this.moverPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.moverPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.moverPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moverPictureBox.Image = global::PTZPanel.Properties.Resources.mover;
            this.moverPictureBox.Location = new System.Drawing.Point(73, 64);
            this.moverPictureBox.Name = "moverPictureBox";
            this.moverPictureBox.Size = new System.Drawing.Size(75, 75);
            this.moverPictureBox.TabIndex = 0;
            this.moverPictureBox.TabStop = false;
            this.moverPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoverPictureBoxMouseDown);
            this.moverPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoverPictureBoxMouseUp);
            // 
            // leftPictureBox
            // 
            this.leftPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.leftPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.leftPictureBox.Location = new System.Drawing.Point(34, 83);
            this.leftPictureBox.Name = "leftPictureBox";
            this.leftPictureBox.Size = new System.Drawing.Size(32, 38);
            this.leftPictureBox.TabIndex = 4;
            this.leftPictureBox.TabStop = false;
            this.leftPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LeftPictureBoxMouseDown);
            this.leftPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzPictureBoxMouseUp);
            // 
            // rightPictureBox
            // 
            this.rightPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rightPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rightPictureBox.Location = new System.Drawing.Point(154, 83);
            this.rightPictureBox.Name = "rightPictureBox";
            this.rightPictureBox.Size = new System.Drawing.Size(32, 38);
            this.rightPictureBox.TabIndex = 3;
            this.rightPictureBox.TabStop = false;
            this.rightPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RightPictureBoxMouseDown);
            this.rightPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzPictureBoxMouseUp);
            // 
            // downPictureBox
            // 
            this.downPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.downPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downPictureBox.Location = new System.Drawing.Point(91, 146);
            this.downPictureBox.Name = "downPictureBox";
            this.downPictureBox.Size = new System.Drawing.Size(38, 32);
            this.downPictureBox.TabIndex = 2;
            this.downPictureBox.TabStop = false;
            this.downPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DownPictureBoxMouseDown);
            this.downPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzPictureBoxMouseUp);
            // 
            // upPictureBox
            // 
            this.upPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.upPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.upPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.upPictureBox.Location = new System.Drawing.Point(91, 26);
            this.upPictureBox.Name = "upPictureBox";
            this.upPictureBox.Size = new System.Drawing.Size(38, 32);
            this.upPictureBox.TabIndex = 1;
            this.upPictureBox.TabStop = false;
            this.upPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UpPictureBoxMouseDown);
            this.upPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzPictureBoxMouseUp);
            // 
            // PTZPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.ptzControlPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "PTZPanel";
            this.Size = new System.Drawing.Size(220, 477);
            this.ptzControlPanel.ResumeLayout(false);
            this.toolPanel.ResumeLayout(false);
            this.toolPanel.PerformLayout();
            this.focusPanel.ResumeLayout(false);
            this.zoomPanel.ResumeLayout(false);
            this.controllerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.moverPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upPictureBox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel titlePanel;
		private System.Windows.Forms.Panel ptzControlPanel;
		private System.Windows.Forms.Panel controllerPanel;
		private System.Windows.Forms.PictureBox moverPictureBox;
		private System.Windows.Forms.PictureBox leftPictureBox;
		private System.Windows.Forms.PictureBox rightPictureBox;
		private System.Windows.Forms.PictureBox downPictureBox;
        private System.Windows.Forms.PictureBox upPictureBox;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.ComboBox presetPointcomboBox;
        private System.Windows.Forms.Label presetPointLabel;
        private System.Windows.Forms.Panel focusPanel;
        private System.Windows.Forms.Button incressFocusButton;
        private System.Windows.Forms.Button recudeFocusButton;
        private System.Windows.Forms.Label focusLabel;
        private System.Windows.Forms.Panel zoomPanel;
        private System.Windows.Forms.Button incressZoomButton;
        private System.Windows.Forms.Button reduceZoomButton;
        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.CheckBox enableTourCheckBox;
        private System.Windows.Forms.CheckBox autoTrackingCheckBox;
        private System.Windows.Forms.CheckBox enablePTZPatrolCheckBox;
        private System.Windows.Forms.ComboBox RegionsComboBox;
        private System.Windows.Forms.Label patrolLabel;
	}
}
