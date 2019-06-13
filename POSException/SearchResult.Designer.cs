namespace POSException
{
    sealed partial class SearchResult
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.resultListBox = new System.Windows.Forms.ListBox();
            this.pagePanel = new PanelBase.DoubleBufferPanel();
            this.pageLabel = new PanelBase.DoubleBufferLabel();
            this.nextPageButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.previousPageButton = new System.Windows.Forms.Button();
            this.firstButton = new System.Windows.Forms.Button();
            this.containerPanel.SuspendLayout();
            this.pagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(200, 42);
            this.titlePanel.TabIndex = 3;
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.containerPanel.Controls.Add(this.resultListBox);
            this.containerPanel.Controls.Add(this.pagePanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 42);
            this.containerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Size = new System.Drawing.Size(200, 288);
            this.containerPanel.TabIndex = 4;
            // 
            // resultListBox
            // 
            this.resultListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(103)))), ((int)(((byte)(117)))));
            this.resultListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultListBox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultListBox.ForeColor = System.Drawing.Color.White;
            this.resultListBox.FormattingEnabled = true;
            this.resultListBox.Location = new System.Drawing.Point(0, 0);
            this.resultListBox.Name = "resultListBox";
            this.resultListBox.Size = new System.Drawing.Size(200, 263);
            this.resultListBox.TabIndex = 1;
            // 
            // pagePanel
            // 
            this.pagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(61)))), ((int)(((byte)(68)))));
            this.pagePanel.Controls.Add(this.pageLabel);
            this.pagePanel.Controls.Add(this.nextPageButton);
            this.pagePanel.Controls.Add(this.lastButton);
            this.pagePanel.Controls.Add(this.previousPageButton);
            this.pagePanel.Controls.Add(this.firstButton);
            this.pagePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagePanel.Location = new System.Drawing.Point(0, 263);
            this.pagePanel.MinimumSize = new System.Drawing.Size(0, 25);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Size = new System.Drawing.Size(200, 25);
            this.pagePanel.TabIndex = 2;
            // 
            // pageLabel
            // 
            this.pageLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pageLabel.BackColor = System.Drawing.Color.Transparent;
            this.pageLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageLabel.ForeColor = System.Drawing.Color.DimGray;
            this.pageLabel.Location = new System.Drawing.Point(50, 2);
            this.pageLabel.Name = "pageLabel";
            this.pageLabel.Size = new System.Drawing.Size(100, 20);
            this.pageLabel.TabIndex = 4;
            this.pageLabel.Text = "1/1";
            this.pageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nextPageButton
            // 
            this.nextPageButton.BackgroundImage = global::POSException.Properties.Resources.nextPage;
            this.nextPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextPageButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextPageButton.FlatAppearance.BorderSize = 0;
            this.nextPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextPageButton.Location = new System.Drawing.Point(140, 0);
            this.nextPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(30, 25);
            this.nextPageButton.TabIndex = 3;
            this.nextPageButton.TabStop = false;
            this.nextPageButton.UseVisualStyleBackColor = true;
            this.nextPageButton.Visible = false;
            this.nextPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NextPageButtonMouseClick);
            // 
            // lastButton
            // 
            this.lastButton.BackgroundImage = global::POSException.Properties.Resources.lastPage;
            this.lastButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lastButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lastButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.lastButton.FlatAppearance.BorderSize = 0;
            this.lastButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.lastButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lastButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lastButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lastButton.Location = new System.Drawing.Point(170, 0);
            this.lastButton.Margin = new System.Windows.Forms.Padding(0);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(30, 25);
            this.lastButton.TabIndex = 6;
            this.lastButton.TabStop = false;
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Visible = false;
            this.lastButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LastButtonMouseClick);
            // 
            // previousPageButton
            // 
            this.previousPageButton.BackgroundImage = global::POSException.Properties.Resources.previousPage;
            this.previousPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.previousPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousPageButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.previousPageButton.FlatAppearance.BorderSize = 0;
            this.previousPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousPageButton.Location = new System.Drawing.Point(30, 0);
            this.previousPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousPageButton.Name = "previousPageButton";
            this.previousPageButton.Size = new System.Drawing.Size(30, 25);
            this.previousPageButton.TabIndex = 2;
            this.previousPageButton.TabStop = false;
            this.previousPageButton.UseVisualStyleBackColor = true;
            this.previousPageButton.Visible = false;
            this.previousPageButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PreviousPageButtonMouseClick);
            // 
            // firstButton
            // 
            this.firstButton.BackgroundImage = global::POSException.Properties.Resources.firstPage;
            this.firstButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.firstButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.firstButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.firstButton.FlatAppearance.BorderSize = 0;
            this.firstButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.firstButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.firstButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.firstButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.firstButton.Location = new System.Drawing.Point(0, 0);
            this.firstButton.Margin = new System.Windows.Forms.Padding(0);
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new System.Drawing.Size(30, 25);
            this.firstButton.TabIndex = 5;
            this.firstButton.TabStop = false;
            this.firstButton.UseVisualStyleBackColor = true;
            this.firstButton.Visible = false;
            this.firstButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FirstButtonMouseClick);
            // 
            // SearchResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "SearchResult";
            this.Size = new System.Drawing.Size(200, 330);
            this.containerPanel.ResumeLayout(false);
            this.pagePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.ListBox resultListBox;
        private  PanelBase.DoubleBufferPanel pagePanel;
        protected System.Windows.Forms.Button previousPageButton;
        protected System.Windows.Forms.Button nextPageButton;
        protected PanelBase.DoubleBufferLabel pageLabel;
        protected System.Windows.Forms.Button firstButton;
        protected System.Windows.Forms.Button lastButton;
    }
}
