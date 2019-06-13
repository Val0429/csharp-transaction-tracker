namespace PrintImageForm
{
	sealed partial class PrintEvidence
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
			this.printImageLabel1 = new System.Windows.Forms.Label();
			this.transactionRichTextBox = new System.Windows.Forms.RichTextBox();
			this.commentTextBox = new PanelBase.HotKeyTextBox();
			this.doneButton = new System.Windows.Forms.Button();
			this.VideoFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.printImageLabel2 = new System.Windows.Forms.Label();
			this.printImageLabel3 = new System.Windows.Forms.Label();
			this.printImageLabel4 = new System.Windows.Forms.Label();
			this.titlePanel.SuspendLayout();
			this.VideoFlowPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// printButton
			// 
			this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.printButton.BackColor = System.Drawing.Color.Transparent;
			this.printButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.printButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.printButton.FlatAppearance.BorderSize = 0;
			this.printButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.printButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.printButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.printButton.ForeColor = System.Drawing.Color.Black;
			this.printButton.Location = new System.Drawing.Point(830, 634);
			this.printButton.Name = "printButton";
			this.printButton.Size = new System.Drawing.Size(70, 30);
			this.printButton.TabIndex = 30;
			this.printButton.Text = "Print";
			this.printButton.UseVisualStyleBackColor = false;
			this.printButton.Click += new System.EventHandler(this.PrintButtonClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.BackColor = System.Drawing.Color.Transparent;
			this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cancelButton.FlatAppearance.BorderSize = 0;
			this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.cancelButton.ForeColor = System.Drawing.Color.Black;
			this.cancelButton.Location = new System.Drawing.Point(929, 634);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(70, 30);
			this.cancelButton.TabIndex = 31;
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
			this.titlePanel.Controls.Add(this.nameLabel);
			this.titlePanel.Controls.Add(this.datetimeLabel);
			this.titlePanel.Location = new System.Drawing.Point(12, 12);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.Size = new System.Drawing.Size(987, 54);
			this.titlePanel.TabIndex = 23;
			// 
			// nameLabel
			// 
			this.nameLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nameLabel.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.ForeColor = System.Drawing.Color.Black;
			this.nameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.nameLabel.Location = new System.Drawing.Point(12, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(226, 53);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "Incident Report";
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// datetimeLabel
			// 
			this.datetimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.datetimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.datetimeLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.datetimeLabel.ForeColor = System.Drawing.Color.Black;
			this.datetimeLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.datetimeLabel.Location = new System.Drawing.Point(756, 29);
			this.datetimeLabel.Name = "datetimeLabel";
			this.datetimeLabel.Size = new System.Drawing.Size(226, 23);
			this.datetimeLabel.TabIndex = 1;
			this.datetimeLabel.Text = "2011-06-02 11:22:33";
			this.datetimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// printImageLabel1
			// 
			this.printImageLabel1.Location = new System.Drawing.Point(0, 0);
			this.printImageLabel1.Margin = new System.Windows.Forms.Padding(0);
			this.printImageLabel1.Name = "printImageLabel1";
			this.printImageLabel1.Size = new System.Drawing.Size(338, 245);
			this.printImageLabel1.TabIndex = 20;
			this.printImageLabel1.Text = "Loading...";
			this.printImageLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// transactionRichTextBox
			// 
			this.transactionRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.transactionRichTextBox.BackColor = System.Drawing.SystemColors.Info;
			this.transactionRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.transactionRichTextBox.Font = new System.Drawing.Font("Lucida Console", 9F);
			this.transactionRichTextBox.Location = new System.Drawing.Point(12, 73);
			this.transactionRichTextBox.Name = "transactionRichTextBox";
			this.transactionRichTextBox.Size = new System.Drawing.Size(305, 484);
			this.transactionRichTextBox.TabIndex = 29;
			this.transactionRichTextBox.Text = "";
			// 
			// commentTextBox
			// 
			this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.commentTextBox.ForeColor = System.Drawing.Color.White;
			this.commentTextBox.Location = new System.Drawing.Point(12, 563);
			this.commentTextBox.MaxLength = 1000;
			this.commentTextBox.Multiline = true;
			this.commentTextBox.Name = "commentTextBox";
			this.commentTextBox.Size = new System.Drawing.Size(987, 56);
			this.commentTextBox.TabIndex = 34;
			this.commentTextBox.Text = "Write Comment Here.";
			this.commentTextBox.Enter += new System.EventHandler(this.CommentTextBoxEnter);
			// 
			// doneButton
			// 
			this.doneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.doneButton.BackColor = System.Drawing.Color.Transparent;
			this.doneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.doneButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.doneButton.FlatAppearance.BorderSize = 0;
			this.doneButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.doneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.doneButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.doneButton.ForeColor = System.Drawing.Color.Black;
			this.doneButton.Location = new System.Drawing.Point(731, 634);
			this.doneButton.Name = "doneButton";
			this.doneButton.Size = new System.Drawing.Size(70, 30);
			this.doneButton.TabIndex = 35;
			this.doneButton.Text = "Done";
			this.doneButton.UseVisualStyleBackColor = false;
			this.doneButton.Click += new System.EventHandler(this.DoneButtonClick);
			// 
			// VideoFlowPanel
			// 
			this.VideoFlowPanel.Controls.Add(this.printImageLabel1);
			this.VideoFlowPanel.Controls.Add(this.printImageLabel2);
			this.VideoFlowPanel.Controls.Add(this.printImageLabel3);
			this.VideoFlowPanel.Controls.Add(this.printImageLabel4);
			this.VideoFlowPanel.Location = new System.Drawing.Point(323, 70);
			this.VideoFlowPanel.Margin = new System.Windows.Forms.Padding(0);
			this.VideoFlowPanel.Name = "VideoFlowPanel";
			this.VideoFlowPanel.Size = new System.Drawing.Size(676, 490);
			this.VideoFlowPanel.TabIndex = 36;
			// 
			// printImageLabel2
			// 
			this.printImageLabel2.Location = new System.Drawing.Point(338, 0);
			this.printImageLabel2.Margin = new System.Windows.Forms.Padding(0);
			this.printImageLabel2.Name = "printImageLabel2";
			this.printImageLabel2.Size = new System.Drawing.Size(338, 245);
			this.printImageLabel2.TabIndex = 21;
			this.printImageLabel2.Text = "Loading...";
			this.printImageLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// printImageLabel3
			// 
			this.printImageLabel3.Location = new System.Drawing.Point(0, 245);
			this.printImageLabel3.Margin = new System.Windows.Forms.Padding(0);
			this.printImageLabel3.Name = "printImageLabel3";
			this.printImageLabel3.Size = new System.Drawing.Size(338, 245);
			this.printImageLabel3.TabIndex = 22;
			this.printImageLabel3.Text = "Loading...";
			this.printImageLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// printImageLabel4
			// 
			this.printImageLabel4.Location = new System.Drawing.Point(338, 245);
			this.printImageLabel4.Margin = new System.Windows.Forms.Padding(0);
			this.printImageLabel4.Name = "printImageLabel4";
			this.printImageLabel4.Size = new System.Drawing.Size(338, 245);
			this.printImageLabel4.TabIndex = 23;
			this.printImageLabel4.Text = "Loading...";
			this.printImageLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PrintEvidence
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackgroundImage = global::PrintImageForm.Properties.Resources.controllerBG;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(1011, 676);
			this.ControlBox = false;
			this.Controls.Add(this.VideoFlowPanel);
			this.Controls.Add(this.commentTextBox);
			this.Controls.Add(this.printButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.doneButton);
			this.Controls.Add(this.transactionRichTextBox);
			this.Controls.Add(this.titlePanel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "PrintEvidence";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PrintEvidence";
			this.titlePanel.ResumeLayout(false);
			this.VideoFlowPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label printImageLabel1;
		private System.Windows.Forms.Label datetimeLabel;
		private PanelBase.DoubleBufferPanel titlePanel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.RichTextBox transactionRichTextBox;
		private System.Windows.Forms.Button printButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button doneButton;
		private System.Windows.Forms.FlowLayoutPanel VideoFlowPanel;
		private System.Windows.Forms.Label printImageLabel2;
		private System.Windows.Forms.Label printImageLabel3;
		private System.Windows.Forms.Label printImageLabel4;
		private PanelBase.HotKeyTextBox commentTextBox;
	}
}