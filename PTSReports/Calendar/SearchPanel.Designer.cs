using System.Windows.Forms;
using PanelBase;

namespace PTSReports.Calendar
{
    sealed partial class SearchPanel
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
            this.containerPanel = new PanelBase.DoubleBufferPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.monthPanel = new PanelBase.DoubleBufferLabel();
            this.monthLabel = new PanelBase.DoubleBufferLabel();
            this.nextPictureBox = new System.Windows.Forms.PictureBox();
            this.previousPictureBox = new System.Windows.Forms.PictureBox();
            this.weekTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.exceptionFlowLayoutPanel = new PanelBase.DoubleBufferFlowLayoutPanel();
            this.resultrPanel = new PanelBase.DoubleBufferPanel();
            this.foundLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.monthPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nextPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousPictureBox)).BeginInit();
            this.resultrPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerPanel
            // 
            this.containerPanel.AutoScroll = true;
            this.containerPanel.AutoSize = true;
            this.containerPanel.BackColor = System.Drawing.Color.Transparent;
            this.containerPanel.Controls.Add(this.tableLayoutPanel);
            this.containerPanel.Controls.Add(this.monthPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(12, 70);
            this.containerPanel.MinimumSize = new System.Drawing.Size(0, 50);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(5);
            this.containerPanel.Size = new System.Drawing.Size(500, 322);
            this.containerPanel.TabIndex = 2;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel.Location = new System.Drawing.Point(5, 87);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 302F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(490, 230);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // monthPanel
            // 
            this.monthPanel.BackColor = System.Drawing.Color.Transparent;
            this.monthPanel.Controls.Add(this.monthLabel);
            this.monthPanel.Controls.Add(this.nextPictureBox);
            this.monthPanel.Controls.Add(this.previousPictureBox);
            this.monthPanel.Controls.Add(this.weekTableLayoutPanel);
            this.monthPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.monthPanel.Location = new System.Drawing.Point(5, 5);
            this.monthPanel.Margin = new System.Windows.Forms.Padding(0);
            this.monthPanel.Name = "monthPanel";
            this.monthPanel.Size = new System.Drawing.Size(490, 82);
            this.monthPanel.TabIndex = 8;
            // 
            // monthLabel
            // 
            this.monthLabel.BackColor = System.Drawing.Color.Transparent;
            this.monthLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.monthLabel.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.monthLabel.Location = new System.Drawing.Point(50, 0);
            this.monthLabel.Name = "monthLabel";
            this.monthLabel.Size = new System.Drawing.Size(390, 44);
            this.monthLabel.TabIndex = 7;
            this.monthLabel.Text = "YYYY MM";
            this.monthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nextPictureBox
            // 
            this.nextPictureBox.BackgroundImage = global::PTSReports.Properties.Resources.next;
            this.nextPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextPictureBox.Location = new System.Drawing.Point(440, 0);
            this.nextPictureBox.Name = "nextPictureBox";
            this.nextPictureBox.Size = new System.Drawing.Size(50, 44);
            this.nextPictureBox.TabIndex = 10;
            this.nextPictureBox.TabStop = false;
            this.nextPictureBox.Click += new System.EventHandler(this.NextPictureBoxClick);
            // 
            // previousPictureBox
            // 
            this.previousPictureBox.BackgroundImage = global::PTSReports.Properties.Resources.previous;
            this.previousPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.previousPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.previousPictureBox.Location = new System.Drawing.Point(0, 0);
            this.previousPictureBox.Name = "previousPictureBox";
            this.previousPictureBox.Size = new System.Drawing.Size(50, 44);
            this.previousPictureBox.TabIndex = 9;
            this.previousPictureBox.TabStop = false;
            this.previousPictureBox.Click += new System.EventHandler(this.PreviousPictureBoxClick);
            // 
            // weekTableLayoutPanel
            // 
            this.weekTableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.weekTableLayoutPanel.ColumnCount = 7;
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.weekTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.weekTableLayoutPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weekTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.weekTableLayoutPanel.Location = new System.Drawing.Point(0, 44);
            this.weekTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.weekTableLayoutPanel.Name = "weekTableLayoutPanel";
            this.weekTableLayoutPanel.RowCount = 1;
            this.weekTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.weekTableLayoutPanel.Size = new System.Drawing.Size(490, 38);
            this.weekTableLayoutPanel.TabIndex = 8;
            // 
            // exceptionFlowLayoutPanel
            // 
            this.exceptionFlowLayoutPanel.AutoSize = true;
            this.exceptionFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.exceptionFlowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.exceptionFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionFlowLayoutPanel.Location = new System.Drawing.Point(12, 30);
            this.exceptionFlowLayoutPanel.MinimumSize = new System.Drawing.Size(0, 40);
            this.exceptionFlowLayoutPanel.Name = "exceptionFlowLayoutPanel";
            this.exceptionFlowLayoutPanel.Size = new System.Drawing.Size(500, 40);
            this.exceptionFlowLayoutPanel.TabIndex = 7;
            // 
            // resultrPanel
            // 
            this.resultrPanel.BackColor = System.Drawing.Color.Transparent;
            this.resultrPanel.Controls.Add(this.foundLabel);
            this.resultrPanel.Controls.Add(this.resultLabel);
            this.resultrPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultrPanel.Location = new System.Drawing.Point(12, 10);
            this.resultrPanel.Name = "resultrPanel";
            this.resultrPanel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.resultrPanel.Size = new System.Drawing.Size(500, 20);
            this.resultrPanel.TabIndex = 9;
            // 
            // foundLabel
            // 
            this.foundLabel.AutoSize = true;
            this.foundLabel.BackColor = System.Drawing.Color.Transparent;
            this.foundLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.foundLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(177)))), ((int)(((byte)(224)))));
            this.foundLabel.Location = new System.Drawing.Point(64, 0);
            this.foundLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new System.Drawing.Size(52, 20);
            this.foundLabel.TabIndex = 7;
            this.foundLabel.Text = "1 Found";
            this.foundLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.BackColor = System.Drawing.Color.Transparent;
            this.resultLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.resultLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultLabel.Location = new System.Drawing.Point(8, 0);
            this.resultLabel.MinimumSize = new System.Drawing.Size(0, 20);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.resultLabel.Size = new System.Drawing.Size(56, 20);
            this.resultLabel.TabIndex = 6;
            this.resultLabel.Text = "Result :";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SearchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::PTSReports.Properties.Resources.bg_noborder;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.exceptionFlowLayoutPanel);
            this.Controls.Add(this.resultrPanel);
            this.Name = "SearchPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 10, 12, 8);
            this.Size = new System.Drawing.Size(524, 400);
            this.containerPanel.ResumeLayout(false);
            this.monthPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nextPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousPictureBox)).EndInit();
            this.resultrPanel.ResumeLayout(false);
            this.resultrPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelBase.DoubleBufferPanel containerPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private PanelBase.DoubleBufferLabel monthLabel;
        private PanelBase.DoubleBufferLabel monthPanel;
        private System.Windows.Forms.TableLayoutPanel weekTableLayoutPanel;
        private PictureBox nextPictureBox;
        private PictureBox previousPictureBox;
        private DoubleBufferFlowLayoutPanel exceptionFlowLayoutPanel;
        private DoubleBufferPanel resultrPanel;
        private Label foundLabel;
        private Label resultLabel;
    }
}
