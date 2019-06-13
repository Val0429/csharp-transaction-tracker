namespace POSException
{
    sealed partial class ExceptionSearch
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
            this.viewModelPanel = new PanelBase.DoubleBufferPanel();
            this.searchComboBox = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.exceptionAndKeywordLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.posComboBox = new System.Windows.Forms.ComboBox();
            this.posLabel = new System.Windows.Forms.Label();
            this.panelResult = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.viewModelPanel.SuspendLayout();
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
            this.titlePanel.Size = new System.Drawing.Size(295, 42);
            this.titlePanel.TabIndex = 3;
            // 
            // viewModelPanel
            // 
            this.viewModelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.viewModelPanel.Controls.Add(this.searchComboBox);
            this.viewModelPanel.Controls.Add(this.searchButton);
            this.viewModelPanel.Controls.Add(this.endTimePicker);
            this.viewModelPanel.Controls.Add(this.endDatePicker);
            this.viewModelPanel.Controls.Add(this.startTimePicker);
            this.viewModelPanel.Controls.Add(this.startDatePicker);
            this.viewModelPanel.Controls.Add(this.exceptionAndKeywordLabel);
            this.viewModelPanel.Controls.Add(this.endLabel);
            this.viewModelPanel.Controls.Add(this.startLabel);
            this.viewModelPanel.Controls.Add(this.posComboBox);
            this.viewModelPanel.Controls.Add(this.posLabel);
            this.viewModelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.viewModelPanel.Location = new System.Drawing.Point(0, 42);
            this.viewModelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.viewModelPanel.Name = "viewModelPanel";
            this.viewModelPanel.Size = new System.Drawing.Size(295, 169);
            this.viewModelPanel.TabIndex = 4;
            // 
            // searchComboBox
            // 
            this.searchComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchComboBox.FormattingEnabled = true;
            this.searchComboBox.Location = new System.Drawing.Point(8, 126);
            this.searchComboBox.Name = "searchComboBox";
            this.searchComboBox.Size = new System.Drawing.Size(193, 23);
            this.searchComboBox.TabIndex = 31;
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.BackColor = System.Drawing.Color.Transparent;
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.searchButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.Location = new System.Drawing.Point(205, 122);
            this.searchButton.Margin = new System.Windows.Forms.Padding(3, 12, 15, 3);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(80, 30);
            this.searchButton.TabIndex = 30;
            this.searchButton.TabStop = false;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SearchButtonMouseClick);
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "HH:mm:ss";
            this.endTimePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(208, 70);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.ShowUpDown = true;
            this.endTimePicker.Size = new System.Drawing.Size(77, 21);
            this.endTimePicker.TabIndex = 12;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "yyyy-MM-dd";
            this.endDatePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(100, 70);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(101, 21);
            this.endDatePicker.TabIndex = 11;
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm:ss";
            this.startTimePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new System.Drawing.Point(208, 36);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.ShowUpDown = true;
            this.startTimePicker.Size = new System.Drawing.Size(77, 21);
            this.startTimePicker.TabIndex = 9;
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "yyyy-MM-dd";
            this.startDatePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(100, 36);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(101, 21);
            this.startDatePicker.TabIndex = 8;
            // 
            // exceptionAndKeywordLabel
            // 
            this.exceptionAndKeywordLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionAndKeywordLabel.ForeColor = System.Drawing.Color.White;
            this.exceptionAndKeywordLabel.Location = new System.Drawing.Point(5, 100);
            this.exceptionAndKeywordLabel.Name = "exceptionAndKeywordLabel";
            this.exceptionAndKeywordLabel.Size = new System.Drawing.Size(283, 23);
            this.exceptionAndKeywordLabel.TabIndex = 3;
            this.exceptionAndKeywordLabel.Text = "Exception / Keyword";
            this.exceptionAndKeywordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // endLabel
            // 
            this.endLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endLabel.ForeColor = System.Drawing.Color.White;
            this.endLabel.Location = new System.Drawing.Point(5, 68);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(88, 23);
            this.endLabel.TabIndex = 3;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startLabel
            // 
            this.startLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.White;
            this.startLabel.Location = new System.Drawing.Point(5, 34);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(88, 23);
            this.startLabel.TabIndex = 2;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // posComboBox
            // 
            this.posComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.posComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posComboBox.FormattingEnabled = true;
            this.posComboBox.Location = new System.Drawing.Point(99, 4);
            this.posComboBox.Name = "posComboBox";
            this.posComboBox.Size = new System.Drawing.Size(186, 23);
            this.posComboBox.TabIndex = 1;
            // 
            // posLabel
            // 
            this.posLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posLabel.ForeColor = System.Drawing.Color.White;
            this.posLabel.Location = new System.Drawing.Point(5, 3);
            this.posLabel.Name = "posLabel";
            this.posLabel.Size = new System.Drawing.Size(88, 23);
            this.posLabel.TabIndex = 0;
            this.posLabel.Text = "POS";
            this.posLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelResult
            // 
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelResult.Location = new System.Drawing.Point(0, 211);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(295, 330);
            this.panelResult.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 541);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 236);
            this.panel1.TabIndex = 7;
            // 
            // ExceptionSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelResult);
            this.Controls.Add(this.viewModelPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "ExceptionSearch";
            this.Size = new System.Drawing.Size(295, 777);
            this.viewModelPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        public PanelBase.DoubleBufferPanel viewModelPanel;
        private System.Windows.Forms.Label posLabel;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.ComboBox posComboBox;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label exceptionAndKeywordLabel;
        private System.Windows.Forms.ComboBox searchComboBox;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Panel panel1;
    }
}
