namespace DownloadCaseForm
{
    sealed partial class DownloadCaseQueue
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.closeButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.addNewDoubleBufferPanel = new PanelBase.DoubleBufferPanel();
            this.endLabel = new System.Windows.Forms.Label();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startLabel = new System.Windows.Forms.Label();
            this.enableScheduleCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteSelectedButton = new System.Windows.Forms.Button();
            this.addNewDoubleBufferPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.cancelButotn;
            this.closeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.closeButton.Location = new System.Drawing.Point(663, 290);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(150, 41);
            this.closeButton.TabIndex = 19;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.BackColor = System.Drawing.Color.Transparent;
            this.deleteButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.exportButton;
            this.deleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deleteButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteButton.FlatAppearance.BorderSize = 0;
            this.deleteButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.deleteButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.deleteButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.deleteButton.Location = new System.Drawing.Point(495, 290);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(150, 41);
            this.deleteButton.TabIndex = 20;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // containerPanel
            // 
            this.containerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.containerPanel.AutoScroll = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Location = new System.Drawing.Point(12, 62);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(799, 224);
            this.containerPanel.TabIndex = 21;
            // 
            // addNewDoubleBufferPanel
            // 
            this.addNewDoubleBufferPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addNewDoubleBufferPanel.AutoScroll = true;
            this.addNewDoubleBufferPanel.BackColor = System.Drawing.Color.Transparent;
            this.addNewDoubleBufferPanel.Controls.Add(this.endLabel);
            this.addNewDoubleBufferPanel.Controls.Add(this.endTimePicker);
            this.addNewDoubleBufferPanel.Controls.Add(this.startTimePicker);
            this.addNewDoubleBufferPanel.Controls.Add(this.startLabel);
            this.addNewDoubleBufferPanel.Controls.Add(this.enableScheduleCheckBox);
            this.addNewDoubleBufferPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNewDoubleBufferPanel.Location = new System.Drawing.Point(12, 12);
            this.addNewDoubleBufferPanel.Name = "addNewDoubleBufferPanel";
            this.addNewDoubleBufferPanel.Size = new System.Drawing.Size(799, 40);
            this.addNewDoubleBufferPanel.TabIndex = 27;
            this.addNewDoubleBufferPanel.Tag = "AddNewDevice";
            // 
            // endLabel
            // 
            this.endLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endLabel.BackColor = System.Drawing.Color.Transparent;
            this.endLabel.ForeColor = System.Drawing.Color.Black;
            this.endLabel.Location = new System.Drawing.Point(657, 7);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(59, 22);
            this.endLabel.TabIndex = 30;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endTimePicker
            // 
            this.endTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endTimePicker.CustomFormat = "HH:mm";
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(722, 8);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(60, 21);
            this.endTimePicker.TabIndex = 31;
            // 
            // startTimePicker
            // 
            this.startTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startTimePicker.CustomFormat = "HH:mm";
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(591, 8);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(60, 21);
            this.startTimePicker.TabIndex = 29;
            // 
            // startLabel
            // 
            this.startLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startLabel.BackColor = System.Drawing.Color.Transparent;
            this.startLabel.ForeColor = System.Drawing.Color.Black;
            this.startLabel.Location = new System.Drawing.Point(501, 7);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(76, 22);
            this.startLabel.TabIndex = 28;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // enableScheduleCheckBox
            // 
            this.enableScheduleCheckBox.AutoSize = true;
            this.enableScheduleCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.enableScheduleCheckBox.Location = new System.Drawing.Point(14, 11);
            this.enableScheduleCheckBox.Name = "enableScheduleCheckBox";
            this.enableScheduleCheckBox.Size = new System.Drawing.Size(176, 19);
            this.enableScheduleCheckBox.TabIndex = 27;
            this.enableScheduleCheckBox.Text = "Enable schedule download";
            this.enableScheduleCheckBox.UseVisualStyleBackColor = false;
            // 
            // deleteSelectedButton
            // 
            this.deleteSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteSelectedButton.BackColor = System.Drawing.Color.Transparent;
            this.deleteSelectedButton.BackgroundImage = global::DownloadCaseForm.Properties.Resources.exportButton;
            this.deleteSelectedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deleteSelectedButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteSelectedButton.FlatAppearance.BorderSize = 0;
            this.deleteSelectedButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.deleteSelectedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.deleteSelectedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.deleteSelectedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteSelectedButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteSelectedButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.deleteSelectedButton.Location = new System.Drawing.Point(449, 257);
            this.deleteSelectedButton.Name = "deleteSelectedButton";
            this.deleteSelectedButton.Size = new System.Drawing.Size(196, 41);
            this.deleteSelectedButton.TabIndex = 28;
            this.deleteSelectedButton.Text = "Delete selection item";
            this.deleteSelectedButton.UseVisualStyleBackColor = false;
            this.deleteSelectedButton.Visible = false;
            this.deleteSelectedButton.Click += new System.EventHandler(this.DeleteSelectedButtonClick);
            // 
            // DownloadCaseQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::DownloadCaseForm.Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(823, 334);
            this.ControlBox = false;
            this.Controls.Add(this.deleteSelectedButton);
            this.Controls.Add(this.addNewDoubleBufferPanel);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(460, 38);
            this.Name = "DownloadCaseQueue";
            this.Text = "Download Case Queue";
            this.addNewDoubleBufferPanel.ResumeLayout(false);
            this.addNewDoubleBufferPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button deleteButton;
        private PanelBase.DoubleBufferPanel containerPanel;
        private PanelBase.DoubleBufferPanel addNewDoubleBufferPanel;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.CheckBox enableScheduleCheckBox;
        private System.Windows.Forms.Button deleteSelectedButton;
    }
}