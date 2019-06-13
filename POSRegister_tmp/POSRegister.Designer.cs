namespace POSRegister
{
    partial class POSRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSRegister));
            this.titlePanel = new System.Windows.Forms.Panel();
            this.viewModelPanel = new PanelBase.DoubleBufferPanel();
            this.searchPanel = new PanelBase.DoubleBufferPanel();
            this.keywordComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Label();
            this.searchPanel.SuspendLayout();
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
            this.titlePanel.Size = new System.Drawing.Size(218, 42);
            this.titlePanel.TabIndex = 3;
            // 
            // viewModelPanel
            // 
            this.viewModelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.viewModelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewModelPanel.Location = new System.Drawing.Point(0, 76);
            this.viewModelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.viewModelPanel.Name = "viewModelPanel";
            this.viewModelPanel.Size = new System.Drawing.Size(218, 229);
            this.viewModelPanel.TabIndex = 4;
            // 
            // searchPanel
            // 
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.searchPanel.Controls.Add(this.keywordComboBox);
            this.searchPanel.Controls.Add(this.cancelButton);
            this.searchPanel.Controls.Add(this.searchButton);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(0, 42);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(218, 34);
            this.searchPanel.TabIndex = 1;
            // 
            // keywordComboBox
            // 
            this.keywordComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keywordComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.keywordComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.keywordComboBox.FormattingEnabled = true;
            this.keywordComboBox.ItemHeight = 18;
            this.keywordComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.keywordComboBox.Location = new System.Drawing.Point(7, 5);
            this.keywordComboBox.Name = "keywordComboBox";
            this.keywordComboBox.Size = new System.Drawing.Size(157, 24);
            this.keywordComboBox.TabIndex = 0;
            this.keywordComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.keywordComboBox_DrawItem);
            this.keywordComboBox.SelectedIndexChanged += new System.EventHandler(this.keywordComboBox_SelectedIndexChanged);
            this.keywordComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keywordComboBox_KeyDown);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.Location = new System.Drawing.Point(192, 6);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(22, 22);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.Location = new System.Drawing.Point(168, 6);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(22, 22);
            this.searchButton.TabIndex = 4;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // POSRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.viewModelPanel);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "POSRegister";
            this.Size = new System.Drawing.Size(218, 305);
            this.searchPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel viewModelPanel;
        private PanelBase.DoubleBufferPanel searchPanel;
        private System.Windows.Forms.Label cancelButton;
        private System.Windows.Forms.Label searchButton;
        private System.Windows.Forms.ComboBox keywordComboBox;
    }
}
