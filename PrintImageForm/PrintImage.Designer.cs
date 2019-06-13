namespace PrintImageForm
{
    partial class PrintImage
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
            this.printButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titlePanel = new PanelBase.DoubleBufferPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.datetimeLabel = new System.Windows.Forms.Label();
            this.toolPanel = new PanelBase.DoubleBufferPanel();
            this.nextPageButton = new System.Windows.Forms.Button();
            this.pageLabel = new System.Windows.Forms.Label();
            this.previousPageButton = new System.Windows.Forms.Button();
            this.includeCheckBox = new System.Windows.Forms.CheckBox();
            this.printImageLabel = new System.Windows.Forms.Label();
            this.overlayCheckBox = new System.Windows.Forms.CheckBox();
            this.informationCheckBox = new System.Windows.Forms.CheckBox();
            this.commentTextBox = new PanelBase.HotKeyTextBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.toolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.BackColor = System.Drawing.Color.Transparent;
            this.printButton.BackgroundImage = global::PrintImageForm.Properties.Resources.printButton;
            this.printButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.printButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.printButton.FlatAppearance.BorderSize = 0;
            this.printButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.printButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.printButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.printButton.Location = new System.Drawing.Point(97, 526);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(150, 41);
            this.printButton.TabIndex = 7;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = false;
            this.printButton.Click += new System.EventHandler(this.PrintButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImage = global::PrintImageForm.Properties.Resources.cancelButotn;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cancelButton.Location = new System.Drawing.Point(289, 526);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 41);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // titlePanel
            // 
            this.titlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titlePanel.BackColor = System.Drawing.Color.Transparent;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(536, 54);
            this.titlePanel.TabIndex = 9;
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nameLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.nameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nameLabel.Location = new System.Drawing.Point(121, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(263, 32);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Camera Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nameLabel.UseMnemonic = false;
            // 
            // datetimeLabel
            // 
            this.datetimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.datetimeLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.datetimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.datetimeLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datetimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(189)))), ((int)(((byte)(197)))));
            this.datetimeLabel.Location = new System.Drawing.Point(384, 0);
            this.datetimeLabel.Name = "datetimeLabel";
            this.datetimeLabel.Size = new System.Drawing.Size(150, 32);
            this.datetimeLabel.TabIndex = 1;
            this.datetimeLabel.Text = "2011-06-02 11:22:33";
            this.datetimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolPanel
            // 
            this.toolPanel.BackgroundImage = global::PrintImageForm.Properties.Resources.toolBG;
            this.toolPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolPanel.Controls.Add(this.nameLabel);
            this.toolPanel.Controls.Add(this.nextPageButton);
            this.toolPanel.Controls.Add(this.datetimeLabel);
            this.toolPanel.Controls.Add(this.pageLabel);
            this.toolPanel.Controls.Add(this.previousPageButton);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(534, 32);
            this.toolPanel.TabIndex = 10;
            // 
            // nextPageButton
            // 
            this.nextPageButton.BackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.BackgroundImage = global::PrintImageForm.Properties.Resources.nextPage;
            this.nextPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextPageButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.nextPageButton.FlatAppearance.BorderSize = 0;
            this.nextPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextPageButton.Location = new System.Drawing.Point(83, 0);
            this.nextPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(38, 32);
            this.nextPageButton.TabIndex = 3;
            this.nextPageButton.TabStop = false;
            this.nextPageButton.UseVisualStyleBackColor = false;
            this.nextPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NextPageButtonMouseClick);
            // 
            // pageLabel
            // 
            this.pageLabel.BackColor = System.Drawing.Color.Transparent;
            this.pageLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.pageLabel.Location = new System.Drawing.Point(38, 0);
            this.pageLabel.Name = "pageLabel";
            this.pageLabel.Size = new System.Drawing.Size(45, 32);
            this.pageLabel.TabIndex = 4;
            this.pageLabel.Text = "1/1";
            this.pageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previousPageButton
            // 
            this.previousPageButton.BackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.BackgroundImage = global::PrintImageForm.Properties.Resources.previousPage;
            this.previousPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.previousPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousPageButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.previousPageButton.FlatAppearance.BorderSize = 0;
            this.previousPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousPageButton.Location = new System.Drawing.Point(0, 0);
            this.previousPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousPageButton.Name = "previousPageButton";
            this.previousPageButton.Size = new System.Drawing.Size(38, 32);
            this.previousPageButton.TabIndex = 2;
            this.previousPageButton.TabStop = false;
            this.previousPageButton.UseVisualStyleBackColor = false;
            this.previousPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PreviousPageButtonMouseClick);
            // 
            // includeCheckBox
            // 
            this.includeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.includeCheckBox.AutoSize = true;
            this.includeCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.includeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.includeCheckBox.Location = new System.Drawing.Point(20, 369);
            this.includeCheckBox.Name = "includeCheckBox";
            this.includeCheckBox.Size = new System.Drawing.Size(66, 19);
            this.includeCheckBox.TabIndex = 5;
            this.includeCheckBox.Text = "Include";
            this.includeCheckBox.UseVisualStyleBackColor = false;
            this.includeCheckBox.Click += new System.EventHandler(this.IncludeCheckBoxClick);
            // 
            // printImageLabel
            // 
            this.printImageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printImageLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(57)))), ((int)(((byte)(65)))));
            this.printImageLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.printImageLabel.Location = new System.Drawing.Point(20, 52);
            this.printImageLabel.Name = "printImageLabel";
            this.printImageLabel.Size = new System.Drawing.Size(494, 300);
            this.printImageLabel.TabIndex = 0;
            this.printImageLabel.Text = "Loading...";
            this.printImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overlayCheckBox
            // 
            this.overlayCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.overlayCheckBox.AutoSize = true;
            this.overlayCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.overlayCheckBox.Checked = true;
            this.overlayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overlayCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.overlayCheckBox.Location = new System.Drawing.Point(20, 493);
            this.overlayCheckBox.Name = "overlayCheckBox";
            this.overlayCheckBox.Size = new System.Drawing.Size(188, 19);
            this.overlayCheckBox.TabIndex = 11;
            this.overlayCheckBox.Text = "Overlay Information On Image";
            this.overlayCheckBox.UseVisualStyleBackColor = false;
            // 
            // informationCheckBox
            // 
            this.informationCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.informationCheckBox.AutoSize = true;
            this.informationCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.informationCheckBox.Checked = true;
            this.informationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.informationCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(199)))));
            this.informationCheckBox.Location = new System.Drawing.Point(20, 468);
            this.informationCheckBox.Name = "informationCheckBox";
            this.informationCheckBox.Size = new System.Drawing.Size(116, 19);
            this.informationCheckBox.TabIndex = 12;
            this.informationCheckBox.Text = "Print Information";
            this.informationCheckBox.UseVisualStyleBackColor = false;
            // 
            // commentTextBox
            // 
            this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commentTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(56)))));
            this.commentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commentTextBox.ForeColor = System.Drawing.Color.White;
            this.commentTextBox.Location = new System.Drawing.Point(20, 394);
            this.commentTextBox.MaxLength = 1000;
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.ShortcutsEnabled = false;
            this.commentTextBox.Size = new System.Drawing.Size(494, 65);
            this.commentTextBox.TabIndex = 13;
            this.commentTextBox.Text = "Write Comment Here.";
            this.commentTextBox.Enter += new System.EventHandler(this.CommentTextBoxEnter);
            // 
            // doneButton
            // 
            this.doneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.doneButton.BackColor = System.Drawing.Color.Transparent;
            this.doneButton.BackgroundImage = global::PrintImageForm.Properties.Resources.printButton;
            this.doneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.doneButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doneButton.FlatAppearance.BorderSize = 0;
            this.doneButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doneButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.doneButton.Location = new System.Drawing.Point(289, 471);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(150, 41);
            this.doneButton.TabIndex = 19;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = false;
            this.doneButton.Click += new System.EventHandler(this.DoneButtonClick);
            // 
            // PrintImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::PrintImageForm.Properties.Resources.controllerBG;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(534, 582);
            this.ControlBox = false;
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.includeCheckBox);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.informationCheckBox);
            this.Controls.Add(this.overlayCheckBox);
            this.Controls.Add(this.printImageLabel);
            this.Controls.Add(this.toolPanel);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.cancelButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Image";
            this.toolPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button cancelButton;
        private PanelBase.DoubleBufferPanel titlePanel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label datetimeLabel;
        private PanelBase.DoubleBufferPanel toolPanel;
        private System.Windows.Forms.Button previousPageButton;
        private System.Windows.Forms.Button nextPageButton;
        private System.Windows.Forms.Label pageLabel;
        private System.Windows.Forms.Label printImageLabel;
        private System.Windows.Forms.CheckBox includeCheckBox;
        protected System.Windows.Forms.CheckBox overlayCheckBox;
        protected System.Windows.Forms.CheckBox informationCheckBox;
        private System.Windows.Forms.Button doneButton;
        private PanelBase.HotKeyTextBox commentTextBox;
    }
}