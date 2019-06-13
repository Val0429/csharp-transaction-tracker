using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PTSReportsGenerator;

namespace POSException
{
	public partial class SaveReportForm : Form
	{
		public Dictionary<String, String> Localization;
		public SaveReportForm()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"SaveReport_WriteComment", "Enter Comment Here."},
								   {"SaveReport_ApplyComment", "Apply Comment"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();

			commentTextBox.Text = Localization["SaveReport_WriteComment"];
			applyButton.Text = Localization["SaveReport_ApplyComment"];

			commentTextBox.TextChanged += CommentTextBoxTextChanged;
			commentTextBox.Enabled = false;
			Shown += SaveReportFormShown;
		}

		private void SaveReportFormShown(Object sender, EventArgs e)
		{
			Shown -= SaveReportFormShown;
			commentTextBox.Enabled = true;
			applyButton.Focus();
		}

		private PTSReportViewer _reportViewer;
		public void AddReport(PTSReportViewer report)
		{
			_reportViewer = report;
			reportPanel.Controls.Add(report);
		}

		private String _comments = "";
		private void CommentTextBoxEnter(Object sender, EventArgs e)
		{
			if (_comments == "")
				commentTextBox.Text = "";
		}

		private void CommentTextBoxLeave(Object sender, EventArgs e)
		{
			if (_comments == "")
			{
				commentTextBox.TextChanged -= CommentTextBoxTextChanged;

				commentTextBox.Text = Localization["SaveReport_WriteComment"];

				commentTextBox.TextChanged += CommentTextBoxTextChanged;
			}
		}

		private void CommentTextBoxTextChanged(Object sender, EventArgs e)
		{
			_comments = commentTextBox.Text;
		}

		private void ApplyButtonClick(Object sender, EventArgs e)
		{
			if (_reportViewer == null) return;

			if(_reportViewer.UpdateParameter("DescriptionContent", _comments))
				_reportViewer.RefreshReport();
		}
	}
}
