namespace SmartSearch
{
    partial class SearchResult
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
            this.minimizePictureBox = new System.Windows.Forms.PictureBox();
            this.previousPageButton = new System.Windows.Forms.Button();
            this.pagePanel = new PanelBase.DoubleBufferPanel();
            this.nextPageButton = new System.Windows.Forms.Button();
            this.snapshotFlowLayoutPanel = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePictureBox)).BeginInit();
            this.resultPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(42)))), ((int)(((byte)(46)))));
            this.titlePanel.BackgroundImage = global::SmartSearch.Properties.Resources.searchResultTitleBG;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Controls.Add(this.minimizePictureBox);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.ForeColor = System.Drawing.Color.White;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(500, 38);
            this.titlePanel.TabIndex = 1;
            // 
            // minimizePictureBox
            // 
            this.minimizePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.minimizePictureBox.BackgroundImage = global::SmartSearch.Properties.Resources.mini;
            this.minimizePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minimizePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizePictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.minimizePictureBox.Location = new System.Drawing.Point(0, 0);
            this.minimizePictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePictureBox.Name = "minimizePictureBox";
            this.minimizePictureBox.Size = new System.Drawing.Size(16, 38);
            this.minimizePictureBox.TabIndex = 7;
            this.minimizePictureBox.TabStop = false;
            // 
            // previousPageButton
            // 
            this.previousPageButton.BackgroundImage = global::SmartSearch.Properties.Resources.pageSwitchBG;
            this.previousPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.previousPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousPageButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.previousPageButton.FlatAppearance.BorderSize = 0;
            this.previousPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.previousPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousPageButton.Image = global::SmartSearch.Properties.Resources.previousPage;
            this.previousPageButton.Location = new System.Drawing.Point(0, 0);
            this.previousPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.previousPageButton.Name = "previousPageButton";
            this.previousPageButton.Size = new System.Drawing.Size(28, 184);
            this.previousPageButton.TabIndex = 4;
            this.previousPageButton.TabStop = false;
            this.previousPageButton.UseVisualStyleBackColor = true;
            this.previousPageButton.Click += new System.EventHandler(this.PreviousPageButtonClick);
            // 
            // pagePanel
            // 
            this.pagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.pagePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagePanel.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagePanel.Location = new System.Drawing.Point(0, 222);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Size = new System.Drawing.Size(500, 28);
            this.pagePanel.TabIndex = 0;
            // 
            // nextPageButton
            // 
            this.nextPageButton.BackgroundImage = global::SmartSearch.Properties.Resources.pageSwitchBG;
            this.nextPageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.nextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextPageButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextPageButton.FlatAppearance.BorderSize = 0;
            this.nextPageButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.nextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextPageButton.Image = global::SmartSearch.Properties.Resources.nextPage;
            this.nextPageButton.Location = new System.Drawing.Point(472, 0);
            this.nextPageButton.Margin = new System.Windows.Forms.Padding(0);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(28, 184);
            this.nextPageButton.TabIndex = 3;
            this.nextPageButton.TabStop = false;
            this.nextPageButton.UseVisualStyleBackColor = true;
            this.nextPageButton.Click += new System.EventHandler(this.NextPageButtonClick);
            // 
            // snapshotFlowLayoutPanel
            // 
            this.snapshotFlowLayoutPanel.AutoScroll = true;
            this.snapshotFlowLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(59)))), ((int)(((byte)(66)))));
            this.snapshotFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotFlowLayoutPanel.Location = new System.Drawing.Point(28, 0);
            this.snapshotFlowLayoutPanel.Name = "snapshotFlowLayoutPanel";
            this.snapshotFlowLayoutPanel.Size = new System.Drawing.Size(444, 184);
            this.snapshotFlowLayoutPanel.TabIndex = 2;
            // 
            // resultPanel
            // 
            this.resultPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(59)))), ((int)(((byte)(66)))));
            this.resultPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.resultPanel.Controls.Add(this.snapshotFlowLayoutPanel);
            this.resultPanel.Controls.Add(this.previousPageButton);
            this.resultPanel.Controls.Add(this.nextPageButton);
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.ForeColor = System.Drawing.Color.White;
            this.resultPanel.Location = new System.Drawing.Point(0, 38);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(500, 184);
            this.resultPanel.TabIndex = 3;
            // 
            // SearchResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.pagePanel);
            this.Controls.Add(this.titlePanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchResult";
            this.Size = new System.Drawing.Size(500, 250);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimizePictureBox)).EndInit();
            this.resultPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        protected PanelBase.DoubleBufferFlowLayoutPanel snapshotFlowLayoutPanel;
        private System.Windows.Forms.Button nextPageButton;
        private System.Windows.Forms.Button previousPageButton;
        private PanelBase.DoubleBufferPanel pagePanel;
        private System.Windows.Forms.PictureBox minimizePictureBox;
        private System.Windows.Forms.Panel resultPanel;
    }
}
