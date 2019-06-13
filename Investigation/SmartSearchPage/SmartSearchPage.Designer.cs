
namespace Investigation.SmartSearchPage
{
    partial class SmartSearchPage
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
            this.contentPanel = new System.Windows.Forms.Panel();
            this.searchResultPanel = new System.Windows.Forms.Panel();
            this.timeTrackPanel = new System.Windows.Forms.Panel();
            this.smartSearchPanel = new System.Windows.Forms.Panel();
            this.conditionPanel = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Controls.Add(this.searchResultPanel);
            this.contentPanel.Controls.Add(this.timeTrackPanel);
            this.contentPanel.Controls.Add(this.smartSearchPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(221, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(519, 589);
            this.contentPanel.TabIndex = 6;
            // 
            // searchResultPanel
            // 
            this.searchResultPanel.BackColor = System.Drawing.Color.Transparent;
            this.searchResultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResultPanel.Location = new System.Drawing.Point(0, 400);
            this.searchResultPanel.Name = "searchResultPanel";
            this.searchResultPanel.Size = new System.Drawing.Size(519, 42);
            this.searchResultPanel.TabIndex = 10;
            // 
            // timeTrackPanel
            // 
            this.timeTrackPanel.BackColor = System.Drawing.Color.Transparent;
            this.timeTrackPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timeTrackPanel.Location = new System.Drawing.Point(0, 442);
            this.timeTrackPanel.Name = "timeTrackPanel";
            this.timeTrackPanel.Size = new System.Drawing.Size(519, 147);
            this.timeTrackPanel.TabIndex = 9;
            // 
            // smartSearchPanel
            // 
            this.smartSearchPanel.BackColor = System.Drawing.Color.Transparent;
            this.smartSearchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.smartSearchPanel.Location = new System.Drawing.Point(0, 0);
            this.smartSearchPanel.Name = "smartSearchPanel";
            this.smartSearchPanel.Size = new System.Drawing.Size(519, 400);
            this.smartSearchPanel.TabIndex = 8;
            // 
            // conditionPanel
            // 
            this.conditionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(32)))), ((int)(((byte)(37)))));
            this.conditionPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.conditionPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.conditionPanel.Location = new System.Drawing.Point(0, 0);
            this.conditionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.conditionPanel.Name = "conditionPanel";
            this.conditionPanel.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.conditionPanel.Size = new System.Drawing.Size(221, 589);
            this.conditionPanel.TabIndex = 7;
            // 
            // SmartSearchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.conditionPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SmartSearchPage";
            this.Size = new System.Drawing.Size(740, 589);
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Panel searchResultPanel;
        private System.Windows.Forms.Panel timeTrackPanel;
        private System.Windows.Forms.Panel smartSearchPanel;
        private System.Windows.Forms.Panel conditionPanel;


    }
}
