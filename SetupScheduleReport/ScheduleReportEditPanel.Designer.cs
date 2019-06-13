namespace SetupScheduleReport
{
    sealed partial class ScheduleReportEditPanel
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
            this.posLabel = new System.Windows.Forms.Label();
            this.posPanel = new PanelBase.DoubleBufferPanel();
            this.exceptionLabel = new System.Windows.Forms.Label();
            this.exceptionPanel = new PanelBase.DoubleBufferPanel();
            this.settingPanel = new PanelBase.DoubleBufferPanel();
            this.monthDaySelectPanel = new PanelBase.DoubleBufferPanel();
            this.dayComboBox = new System.Windows.Forms.ComboBox();
            this.weekdayRadioSelectPanel = new PanelBase.DoubleBufferPanel();
            this.sunRadioButton = new System.Windows.Forms.RadioButton();
            this.monRadioButton = new System.Windows.Forms.RadioButton();
            this.tueRadioButton = new System.Windows.Forms.RadioButton();
            this.wedRadioButton = new System.Windows.Forms.RadioButton();
            this.thuRadioButton = new System.Windows.Forms.RadioButton();
            this.friRadioButton = new System.Windows.Forms.RadioButton();
            this.satRadioButton = new System.Windows.Forms.RadioButton();
            this.weekdaySelectPanel = new PanelBase.DoubleBufferPanel();
            this.sunCheckBox = new System.Windows.Forms.CheckBox();
            this.monCheckBox = new System.Windows.Forms.CheckBox();
            this.tueCheckBox = new System.Windows.Forms.CheckBox();
            this.wedCheckBox = new System.Windows.Forms.CheckBox();
            this.thuCheckBox = new System.Windows.Forms.CheckBox();
            this.friCheckBox = new System.Windows.Forms.CheckBox();
            this.satCheckBox = new System.Windows.Forms.CheckBox();
            this.sendTimePanel = new PanelBase.DoubleBufferPanel();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.formatPanel = new PanelBase.DoubleBufferPanel();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.typePanel = new PanelBase.DoubleBufferPanel();
            this.periodComboBox = new System.Windows.Forms.ComboBox();
            this.mailPanel = new PanelBase.DoubleBufferPanel();
            this.subjectPanel = new PanelBase.DoubleBufferPanel();
            this.subjectTextBox = new PanelBase.HotKeyTextBox();
            this.recipientPanel = new PanelBase.DoubleBufferPanel();
            this.receiverComboBox = new System.Windows.Forms.ComboBox();
            this.mailLabel = new System.Windows.Forms.Label();
            this.settingPanel.SuspendLayout();
            this.monthDaySelectPanel.SuspendLayout();
            this.weekdayRadioSelectPanel.SuspendLayout();
            this.weekdaySelectPanel.SuspendLayout();
            this.sendTimePanel.SuspendLayout();
            this.formatPanel.SuspendLayout();
            this.typePanel.SuspendLayout();
            this.mailPanel.SuspendLayout();
            this.subjectPanel.SuspendLayout();
            this.recipientPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // posLabel
            // 
            this.posLabel.BackColor = System.Drawing.Color.Transparent;
            this.posLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posLabel.ForeColor = System.Drawing.Color.DimGray;
            this.posLabel.Location = new System.Drawing.Point(12, 283);
            this.posLabel.Name = "posLabel";
            this.posLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.posLabel.Size = new System.Drawing.Size(528, 25);
            this.posLabel.TabIndex = 13;
            this.posLabel.Text = "POS";
            this.posLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // posPanel
            // 
            this.posPanel.AutoSize = true;
            this.posPanel.BackColor = System.Drawing.Color.Transparent;
            this.posPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.posPanel.Location = new System.Drawing.Point(12, 308);
            this.posPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.posPanel.Name = "posPanel";
            this.posPanel.Size = new System.Drawing.Size(528, 40);
            this.posPanel.TabIndex = 14;
            this.posPanel.Tag = "";
            // 
            // exceptionLabel
            // 
            this.exceptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.exceptionLabel.Location = new System.Drawing.Point(12, 348);
            this.exceptionLabel.Name = "exceptionLabel";
            this.exceptionLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.exceptionLabel.Size = new System.Drawing.Size(528, 25);
            this.exceptionLabel.TabIndex = 15;
            this.exceptionLabel.Text = "Exception";
            this.exceptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // exceptionPanel
            // 
            this.exceptionPanel.AutoSize = true;
            this.exceptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionPanel.Location = new System.Drawing.Point(12, 373);
            this.exceptionPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.exceptionPanel.Name = "exceptionPanel";
            this.exceptionPanel.Size = new System.Drawing.Size(528, 40);
            this.exceptionPanel.TabIndex = 16;
            this.exceptionPanel.Tag = "";
            // 
            // settingPanel
            // 
            this.settingPanel.BackColor = System.Drawing.Color.Transparent;
            this.settingPanel.Controls.Add(this.monthDaySelectPanel);
            this.settingPanel.Controls.Add(this.weekdayRadioSelectPanel);
            this.settingPanel.Controls.Add(this.weekdaySelectPanel);
            this.settingPanel.Controls.Add(this.sendTimePanel);
            this.settingPanel.Controls.Add(this.formatPanel);
            this.settingPanel.Controls.Add(this.typePanel);
            this.settingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingPanel.Location = new System.Drawing.Point(12, 18);
            this.settingPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.settingPanel.Name = "settingPanel";
            this.settingPanel.Size = new System.Drawing.Size(528, 160);
            this.settingPanel.TabIndex = 17;
            this.settingPanel.Tag = "";
            // 
            // monthDaySelectPanel
            // 
            this.monthDaySelectPanel.BackColor = System.Drawing.Color.Transparent;
            this.monthDaySelectPanel.Controls.Add(this.dayComboBox);
            this.monthDaySelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.monthDaySelectPanel.Location = new System.Drawing.Point(0, 200);
            this.monthDaySelectPanel.Name = "monthDaySelectPanel";
            this.monthDaySelectPanel.Size = new System.Drawing.Size(528, 40);
            this.monthDaySelectPanel.TabIndex = 17;
            this.monthDaySelectPanel.Tag = "SendDay";
            // 
            // dayComboBox
            // 
            this.dayComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dayComboBox.FormattingEnabled = true;
            this.dayComboBox.Location = new System.Drawing.Point(407, 8);
            this.dayComboBox.Name = "dayComboBox";
            this.dayComboBox.Size = new System.Drawing.Size(106, 23);
            this.dayComboBox.TabIndex = 0;
            // 
            // weekdayRadioSelectPanel
            // 
            this.weekdayRadioSelectPanel.BackColor = System.Drawing.Color.Transparent;
            this.weekdayRadioSelectPanel.Controls.Add(this.sunRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.monRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.tueRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.wedRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.thuRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.friRadioButton);
            this.weekdayRadioSelectPanel.Controls.Add(this.satRadioButton);
            this.weekdayRadioSelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.weekdayRadioSelectPanel.Location = new System.Drawing.Point(0, 160);
            this.weekdayRadioSelectPanel.Name = "weekdayRadioSelectPanel";
            this.weekdayRadioSelectPanel.Size = new System.Drawing.Size(528, 40);
            this.weekdayRadioSelectPanel.TabIndex = 16;
            this.weekdayRadioSelectPanel.Tag = "SendDay";
            // 
            // sunRadioButton
            // 
            this.sunRadioButton.AutoSize = true;
            this.sunRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.sunRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sunRadioButton.Location = new System.Drawing.Point(199, 0);
            this.sunRadioButton.Name = "sunRadioButton";
            this.sunRadioButton.Size = new System.Drawing.Size(47, 40);
            this.sunRadioButton.TabIndex = 6;
            this.sunRadioButton.Text = "Sun";
            this.sunRadioButton.UseVisualStyleBackColor = true;
            // 
            // monRadioButton
            // 
            this.monRadioButton.AutoSize = true;
            this.monRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.monRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monRadioButton.Location = new System.Drawing.Point(246, 0);
            this.monRadioButton.Name = "monRadioButton";
            this.monRadioButton.Size = new System.Drawing.Size(48, 40);
            this.monRadioButton.TabIndex = 5;
            this.monRadioButton.Text = "Mon";
            this.monRadioButton.UseVisualStyleBackColor = true;
            // 
            // tueRadioButton
            // 
            this.tueRadioButton.AutoSize = true;
            this.tueRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.tueRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tueRadioButton.Location = new System.Drawing.Point(294, 0);
            this.tueRadioButton.Name = "tueRadioButton";
            this.tueRadioButton.Size = new System.Drawing.Size(46, 40);
            this.tueRadioButton.TabIndex = 4;
            this.tueRadioButton.Text = "Tue";
            this.tueRadioButton.UseVisualStyleBackColor = true;
            // 
            // wedRadioButton
            // 
            this.wedRadioButton.AutoSize = true;
            this.wedRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.wedRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wedRadioButton.Location = new System.Drawing.Point(340, 0);
            this.wedRadioButton.Name = "wedRadioButton";
            this.wedRadioButton.Size = new System.Drawing.Size(50, 40);
            this.wedRadioButton.TabIndex = 2;
            this.wedRadioButton.Text = "Wed";
            this.wedRadioButton.UseVisualStyleBackColor = true;
            // 
            // thuRadioButton
            // 
            this.thuRadioButton.AutoSize = true;
            this.thuRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.thuRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thuRadioButton.Location = new System.Drawing.Point(390, 0);
            this.thuRadioButton.Name = "thuRadioButton";
            this.thuRadioButton.Size = new System.Drawing.Size(46, 40);
            this.thuRadioButton.TabIndex = 1;
            this.thuRadioButton.Text = "Thu";
            this.thuRadioButton.UseVisualStyleBackColor = true;
            // 
            // friRadioButton
            // 
            this.friRadioButton.AutoSize = true;
            this.friRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.friRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friRadioButton.Location = new System.Drawing.Point(436, 0);
            this.friRadioButton.Name = "friRadioButton";
            this.friRadioButton.Size = new System.Drawing.Size(39, 40);
            this.friRadioButton.TabIndex = 0;
            this.friRadioButton.Text = "Fri";
            this.friRadioButton.UseVisualStyleBackColor = true;
            // 
            // satRadioButton
            // 
            this.satRadioButton.AutoSize = true;
            this.satRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.satRadioButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.satRadioButton.Location = new System.Drawing.Point(475, 0);
            this.satRadioButton.Name = "satRadioButton";
            this.satRadioButton.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.satRadioButton.Size = new System.Drawing.Size(53, 40);
            this.satRadioButton.TabIndex = 3;
            this.satRadioButton.Text = "Sat";
            this.satRadioButton.UseVisualStyleBackColor = true;
            // 
            // weekdaySelectPanel
            // 
            this.weekdaySelectPanel.BackColor = System.Drawing.Color.Transparent;
            this.weekdaySelectPanel.Controls.Add(this.sunCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.monCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.tueCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.wedCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.thuCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.friCheckBox);
            this.weekdaySelectPanel.Controls.Add(this.satCheckBox);
            this.weekdaySelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.weekdaySelectPanel.Location = new System.Drawing.Point(0, 120);
            this.weekdaySelectPanel.Name = "weekdaySelectPanel";
            this.weekdaySelectPanel.Size = new System.Drawing.Size(528, 40);
            this.weekdaySelectPanel.TabIndex = 15;
            this.weekdaySelectPanel.Tag = "SendDay";
            // 
            // sunCheckBox
            // 
            this.sunCheckBox.AutoSize = true;
            this.sunCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.sunCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sunCheckBox.Location = new System.Drawing.Point(192, 0);
            this.sunCheckBox.Name = "sunCheckBox";
            this.sunCheckBox.Size = new System.Drawing.Size(48, 40);
            this.sunCheckBox.TabIndex = 6;
            this.sunCheckBox.Text = "Sun";
            this.sunCheckBox.UseVisualStyleBackColor = true;
            // 
            // monCheckBox
            // 
            this.monCheckBox.AutoSize = true;
            this.monCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.monCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monCheckBox.Location = new System.Drawing.Point(240, 0);
            this.monCheckBox.Name = "monCheckBox";
            this.monCheckBox.Size = new System.Drawing.Size(49, 40);
            this.monCheckBox.TabIndex = 5;
            this.monCheckBox.Text = "Mon";
            this.monCheckBox.UseVisualStyleBackColor = true;
            // 
            // tueCheckBox
            // 
            this.tueCheckBox.AutoSize = true;
            this.tueCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.tueCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tueCheckBox.Location = new System.Drawing.Point(289, 0);
            this.tueCheckBox.Name = "tueCheckBox";
            this.tueCheckBox.Size = new System.Drawing.Size(47, 40);
            this.tueCheckBox.TabIndex = 4;
            this.tueCheckBox.Text = "Tue";
            this.tueCheckBox.UseVisualStyleBackColor = true;
            // 
            // wedCheckBox
            // 
            this.wedCheckBox.AutoSize = true;
            this.wedCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.wedCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wedCheckBox.Location = new System.Drawing.Point(336, 0);
            this.wedCheckBox.Name = "wedCheckBox";
            this.wedCheckBox.Size = new System.Drawing.Size(51, 40);
            this.wedCheckBox.TabIndex = 2;
            this.wedCheckBox.Text = "Wed";
            this.wedCheckBox.UseVisualStyleBackColor = true;
            // 
            // thuCheckBox
            // 
            this.thuCheckBox.AutoSize = true;
            this.thuCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.thuCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thuCheckBox.Location = new System.Drawing.Point(387, 0);
            this.thuCheckBox.Name = "thuCheckBox";
            this.thuCheckBox.Size = new System.Drawing.Size(47, 40);
            this.thuCheckBox.TabIndex = 1;
            this.thuCheckBox.Text = "Thu";
            this.thuCheckBox.UseVisualStyleBackColor = true;
            // 
            // friCheckBox
            // 
            this.friCheckBox.AutoSize = true;
            this.friCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.friCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friCheckBox.Location = new System.Drawing.Point(434, 0);
            this.friCheckBox.Name = "friCheckBox";
            this.friCheckBox.Size = new System.Drawing.Size(40, 40);
            this.friCheckBox.TabIndex = 0;
            this.friCheckBox.Text = "Fri";
            this.friCheckBox.UseVisualStyleBackColor = true;
            // 
            // satCheckBox
            // 
            this.satCheckBox.AutoSize = true;
            this.satCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.satCheckBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.satCheckBox.Location = new System.Drawing.Point(474, 0);
            this.satCheckBox.Name = "satCheckBox";
            this.satCheckBox.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.satCheckBox.Size = new System.Drawing.Size(54, 40);
            this.satCheckBox.TabIndex = 3;
            this.satCheckBox.Text = "Sat";
            this.satCheckBox.UseVisualStyleBackColor = true;
            // 
            // sendTimePanel
            // 
            this.sendTimePanel.BackColor = System.Drawing.Color.Transparent;
            this.sendTimePanel.Controls.Add(this.timePicker);
            this.sendTimePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.sendTimePanel.Location = new System.Drawing.Point(0, 80);
            this.sendTimePanel.Name = "sendTimePanel";
            this.sendTimePanel.Size = new System.Drawing.Size(528, 40);
            this.sendTimePanel.TabIndex = 14;
            this.sendTimePanel.Tag = "SendTime";
            // 
            // timePicker
            // 
            this.timePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timePicker.CustomFormat = "HH:mm";
            this.timePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePicker.Location = new System.Drawing.Point(450, 10);
            this.timePicker.Margin = new System.Windows.Forms.Padding(0);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(63, 21);
            this.timePicker.TabIndex = 14;
            this.timePicker.TabStop = false;
            // 
            // formatPanel
            // 
            this.formatPanel.BackColor = System.Drawing.Color.Transparent;
            this.formatPanel.Controls.Add(this.formatComboBox);
            this.formatPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.formatPanel.Location = new System.Drawing.Point(0, 40);
            this.formatPanel.Name = "formatPanel";
            this.formatPanel.Size = new System.Drawing.Size(528, 40);
            this.formatPanel.TabIndex = 18;
            this.formatPanel.Tag = "Format";
            // 
            // formatComboBox
            // 
            this.formatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Location = new System.Drawing.Point(407, 8);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(106, 23);
            this.formatComboBox.TabIndex = 0;
            // 
            // typePanel
            // 
            this.typePanel.BackColor = System.Drawing.Color.Transparent;
            this.typePanel.Controls.Add(this.periodComboBox);
            this.typePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.typePanel.Location = new System.Drawing.Point(0, 0);
            this.typePanel.Name = "typePanel";
            this.typePanel.Size = new System.Drawing.Size(528, 40);
            this.typePanel.TabIndex = 13;
            this.typePanel.Tag = "Type";
            // 
            // periodComboBox
            // 
            this.periodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.periodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.periodComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.periodComboBox.FormattingEnabled = true;
            this.periodComboBox.Location = new System.Drawing.Point(407, 8);
            this.periodComboBox.Name = "periodComboBox";
            this.periodComboBox.Size = new System.Drawing.Size(106, 23);
            this.periodComboBox.TabIndex = 0;
            // 
            // mailPanel
            // 
            this.mailPanel.AutoSize = true;
            this.mailPanel.BackColor = System.Drawing.Color.Transparent;
            this.mailPanel.Controls.Add(this.subjectPanel);
            this.mailPanel.Controls.Add(this.recipientPanel);
            this.mailPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mailPanel.Location = new System.Drawing.Point(12, 203);
            this.mailPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.mailPanel.Name = "mailPanel";
            this.mailPanel.Size = new System.Drawing.Size(528, 80);
            this.mailPanel.TabIndex = 18;
            this.mailPanel.Tag = "";
            // 
            // subjectPanel
            // 
            this.subjectPanel.BackColor = System.Drawing.Color.Transparent;
            this.subjectPanel.Controls.Add(this.subjectTextBox);
            this.subjectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.subjectPanel.Location = new System.Drawing.Point(0, 40);
            this.subjectPanel.Name = "subjectPanel";
            this.subjectPanel.Size = new System.Drawing.Size(528, 40);
            this.subjectPanel.TabIndex = 26;
            this.subjectPanel.Tag = "Subject";
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subjectTextBox.Location = new System.Drawing.Point(332, 8);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(181, 21);
            this.subjectTextBox.TabIndex = 2;
            // 
            // recipientPanel
            // 
            this.recipientPanel.BackColor = System.Drawing.Color.Transparent;
            this.recipientPanel.Controls.Add(this.receiverComboBox);
            this.recipientPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.recipientPanel.Location = new System.Drawing.Point(0, 0);
            this.recipientPanel.Name = "recipientPanel";
            this.recipientPanel.Size = new System.Drawing.Size(528, 40);
            this.recipientPanel.TabIndex = 25;
            this.recipientPanel.Tag = "Recipient";
            // 
            // receiverComboBox
            // 
            this.receiverComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.receiverComboBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.receiverComboBox.FormattingEnabled = true;
            this.receiverComboBox.Location = new System.Drawing.Point(332, 8);
            this.receiverComboBox.Name = "receiverComboBox";
            this.receiverComboBox.Size = new System.Drawing.Size(181, 23);
            this.receiverComboBox.TabIndex = 3;
            // 
            // mailLabel
            // 
            this.mailLabel.BackColor = System.Drawing.Color.Transparent;
            this.mailLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mailLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mailLabel.ForeColor = System.Drawing.Color.DimGray;
            this.mailLabel.Location = new System.Drawing.Point(12, 178);
            this.mailLabel.Name = "mailLabel";
            this.mailLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.mailLabel.Size = new System.Drawing.Size(528, 25);
            this.mailLabel.TabIndex = 19;
            this.mailLabel.Text = "Send mail";
            this.mailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ScheduleReportEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SetupScheduleReport.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.exceptionPanel);
            this.Controls.Add(this.exceptionLabel);
            this.Controls.Add(this.posPanel);
            this.Controls.Add(this.posLabel);
            this.Controls.Add(this.mailPanel);
            this.Controls.Add(this.mailLabel);
            this.Controls.Add(this.settingPanel);
            this.DoubleBuffered = true;
            this.Name = "ScheduleReportEditPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(552, 526);
            this.settingPanel.ResumeLayout(false);
            this.monthDaySelectPanel.ResumeLayout(false);
            this.weekdayRadioSelectPanel.ResumeLayout(false);
            this.weekdayRadioSelectPanel.PerformLayout();
            this.weekdaySelectPanel.ResumeLayout(false);
            this.weekdaySelectPanel.PerformLayout();
            this.sendTimePanel.ResumeLayout(false);
            this.formatPanel.ResumeLayout(false);
            this.typePanel.ResumeLayout(false);
            this.mailPanel.ResumeLayout(false);
            this.subjectPanel.ResumeLayout(false);
            this.subjectPanel.PerformLayout();
            this.recipientPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label posLabel;
        private PanelBase.DoubleBufferPanel posPanel;
        private System.Windows.Forms.Label exceptionLabel;
        private PanelBase.DoubleBufferPanel exceptionPanel;
        private PanelBase.DoubleBufferPanel settingPanel;
        private PanelBase.DoubleBufferPanel formatPanel;
        private System.Windows.Forms.ComboBox formatComboBox;
        private PanelBase.DoubleBufferPanel monthDaySelectPanel;
        private System.Windows.Forms.ComboBox dayComboBox;
        private PanelBase.DoubleBufferPanel weekdayRadioSelectPanel;
        private System.Windows.Forms.RadioButton sunRadioButton;
        private System.Windows.Forms.RadioButton monRadioButton;
        private System.Windows.Forms.RadioButton tueRadioButton;
        private System.Windows.Forms.RadioButton wedRadioButton;
        private System.Windows.Forms.RadioButton thuRadioButton;
        private System.Windows.Forms.RadioButton friRadioButton;
        private System.Windows.Forms.RadioButton satRadioButton;
        private PanelBase.DoubleBufferPanel weekdaySelectPanel;
        private System.Windows.Forms.CheckBox sunCheckBox;
        private System.Windows.Forms.CheckBox monCheckBox;
        private System.Windows.Forms.CheckBox tueCheckBox;
        private System.Windows.Forms.CheckBox wedCheckBox;
        private System.Windows.Forms.CheckBox thuCheckBox;
        private System.Windows.Forms.CheckBox friCheckBox;
        private System.Windows.Forms.CheckBox satCheckBox;
        private PanelBase.DoubleBufferPanel sendTimePanel;
        protected System.Windows.Forms.DateTimePicker timePicker;
        private PanelBase.DoubleBufferPanel typePanel;
        private System.Windows.Forms.ComboBox periodComboBox;
        private PanelBase.DoubleBufferPanel mailPanel;
        private PanelBase.DoubleBufferPanel subjectPanel;
        private PanelBase.HotKeyTextBox subjectTextBox;
        private PanelBase.DoubleBufferPanel recipientPanel;
        private System.Windows.Forms.Label mailLabel;
        private System.Windows.Forms.ComboBox receiverComboBox;


    }
}
