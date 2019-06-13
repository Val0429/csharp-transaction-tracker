namespace SmartSearch.Route
{
	partial class RouteSearch
	{
		/// <summary> 
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 元件設計工具產生的程式碼

		/// <summary> 
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
		/// 修改這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.parameterPanel = new System.Windows.Forms.Panel();
			this.limitLabel = new System.Windows.Forms.Label();
			this.speedLimitCheckBox = new System.Windows.Forms.CheckBox();
			this.speedLimitText = new System.Windows.Forms.TextBox();
			this.kmLabel = new System.Windows.Forms.Label();
			this.speacingComboBox = new System.Windows.Forms.ComboBox();
			this.secondLabel = new System.Windows.Forms.Label();
			this.spacingLabel = new System.Windows.Forms.Label();
			this.conditionPanel.SuspendLayout();
			this.parameterPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// conditionPanel
			// 
			this.conditionPanel.Controls.Add(this.parameterPanel);
			this.conditionPanel.Location = new System.Drawing.Point(-345, 50);
			this.conditionPanel.Size = new System.Drawing.Size(335, 0);
			this.conditionPanel.Controls.SetChildIndex(this.startDatePicker, 0);
			this.conditionPanel.Controls.SetChildIndex(this.startTimePicker, 0);
			this.conditionPanel.Controls.SetChildIndex(this.endDatePicker, 0);
			this.conditionPanel.Controls.SetChildIndex(this.endTimePicker, 0);
			this.conditionPanel.Controls.SetChildIndex(this.searchButton, 0);
			this.conditionPanel.Controls.SetChildIndex(this.parameterPanel, 0);
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.FlatAppearance.BorderSize = 0;
			this.searchButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
			this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.searchButton.Location = new System.Drawing.Point(246, 265);
			this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
			// 
			// parameterPanel
			// 
			this.parameterPanel.BackColor = System.Drawing.Color.Transparent;
			this.parameterPanel.Controls.Add(this.limitLabel);
			this.parameterPanel.Controls.Add(this.speedLimitCheckBox);
			this.parameterPanel.Controls.Add(this.speedLimitText);
			this.parameterPanel.Controls.Add(this.kmLabel);
			this.parameterPanel.Controls.Add(this.speacingComboBox);
			this.parameterPanel.Controls.Add(this.secondLabel);
			this.parameterPanel.Controls.Add(this.spacingLabel);
			this.parameterPanel.Location = new System.Drawing.Point(6, 78);
			this.parameterPanel.Name = "parameterPanel";
			this.parameterPanel.Size = new System.Drawing.Size(325, 189);
			this.parameterPanel.TabIndex = 40;
			// 
			// limitLabel
			// 
			this.limitLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.limitLabel.ForeColor = System.Drawing.Color.Black;
			this.limitLabel.Location = new System.Drawing.Point(6, 42);
			this.limitLabel.Name = "limitLabel";
			this.limitLabel.Size = new System.Drawing.Size(73, 22);
			this.limitLabel.TabIndex = 38;
			this.limitLabel.Text = "Speed Limit";
			this.limitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// speedLimitCheckBox
			// 
			this.speedLimitCheckBox.AutoSize = true;
			this.speedLimitCheckBox.BackColor = System.Drawing.Color.Transparent;
			this.speedLimitCheckBox.Font = new System.Drawing.Font("Arial", 9F);
			this.speedLimitCheckBox.Location = new System.Drawing.Point(89, 46);
			this.speedLimitCheckBox.Name = "speedLimitCheckBox";
			this.speedLimitCheckBox.Size = new System.Drawing.Size(15, 14);
			this.speedLimitCheckBox.TabIndex = 37;
			this.speedLimitCheckBox.UseVisualStyleBackColor = false;
			this.speedLimitCheckBox.CheckedChanged += new System.EventHandler(this.SpeedLimitCheckBoxCheckedChanged);
			// 
			// speedLimitText
			// 
			this.speedLimitText.Enabled = false;
			this.speedLimitText.Font = new System.Drawing.Font("Arial", 9F);
			this.speedLimitText.Location = new System.Drawing.Point(109, 43);
			this.speedLimitText.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
			this.speedLimitText.Name = "speedLimitText";
			this.speedLimitText.Size = new System.Drawing.Size(65, 21);
			this.speedLimitText.TabIndex = 35;
			// 
			// kmLabel
			// 
			this.kmLabel.BackColor = System.Drawing.Color.Transparent;
			this.kmLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.kmLabel.Location = new System.Drawing.Point(188, 41);
			this.kmLabel.Name = "kmLabel";
			this.kmLabel.Size = new System.Drawing.Size(60, 25);
			this.kmLabel.TabIndex = 36;
			this.kmLabel.Text = "km";
			this.kmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// speacingComboBox
			// 
			this.speacingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.speacingComboBox.Font = new System.Drawing.Font("Arial", 9F);
			this.speacingComboBox.FormattingEnabled = true;
			this.speacingComboBox.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "60",
            "120",
            "300",
            "600",
            "1800",
            "3600"});
			this.speacingComboBox.Location = new System.Drawing.Point(64, 8);
			this.speacingComboBox.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
			this.speacingComboBox.Name = "speacingComboBox";
			this.speacingComboBox.Size = new System.Drawing.Size(65, 23);
			this.speacingComboBox.TabIndex = 34;
			// 
			// secondLabel
			// 
			this.secondLabel.BackColor = System.Drawing.Color.Transparent;
			this.secondLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.secondLabel.Location = new System.Drawing.Point(143, 7);
			this.secondLabel.Name = "secondLabel";
			this.secondLabel.Size = new System.Drawing.Size(60, 25);
			this.secondLabel.TabIndex = 33;
			this.secondLabel.Text = "seconds";
			this.secondLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// spacingLabel
			// 
			this.spacingLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.spacingLabel.ForeColor = System.Drawing.Color.Black;
			this.spacingLabel.Location = new System.Drawing.Point(6, 8);
			this.spacingLabel.Name = "spacingLabel";
			this.spacingLabel.Size = new System.Drawing.Size(52, 22);
			this.spacingLabel.TabIndex = 0;
			this.spacingLabel.Text = "Spacing";
			this.spacingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RouteSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Name = "RouteSearch";
			this.Size = new System.Drawing.Size(1525, 880);
			this.conditionPanel.ResumeLayout(false);
			this.conditionPanel.PerformLayout();
			this.parameterPanel.ResumeLayout(false);
			this.parameterPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel parameterPanel;
		private System.Windows.Forms.Label spacingLabel;
		private System.Windows.Forms.ComboBox speacingComboBox;
		private System.Windows.Forms.Label secondLabel;
		private System.Windows.Forms.Label limitLabel;
		private System.Windows.Forms.CheckBox speedLimitCheckBox;
		private System.Windows.Forms.TextBox speedLimitText;
		private System.Windows.Forms.Label kmLabel;
	}
}
