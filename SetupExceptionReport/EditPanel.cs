using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupExceptionReport
{
	public sealed partial class EditPanel : UserControl
	{
		public IPTS PTS;
		public IPOS POS;
		public Dictionary<String, String> Localization;
		public ExceptionReport Report;
		public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"ExceptionReportPanel_Format", "Format"},
								   {"ExceptionReportPanel_Exception", "Exception"},
								   {"ExceptionReportPanel_Threshold", "Threshold"},
								   {"ExceptionReportPanel_Increment", "Increment"},

								   {"ExceptionReportPanel_SendMail", "Send mail"},
								   {"ExceptionReportPanel_Recipient", "Recipient"},
								   {"ExceptionReportPanel_Subject", "Subject"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "ExceptionReport";
			BackgroundImage = Manager.BackgroundNoBorder;

			thresholdComboBox.KeyPress += KeyAccept.AcceptNumberOnly;
			incrementComboBox.KeyPress += KeyAccept.AcceptNumberOnly;

			exceptionPanel.Paint += InputPanelPaint;
			incrementSelectPanel.Paint += InputPanelPaint;
			thresholdPanel.Paint += InputPanelPaint;
			formatPanel.Paint += InputPanelPaint;
			recipientPanel.Paint += InputPanelPaint;
			subjectPanel.Paint += InputPanelPaint;

			mailLabel.Text = Localization["ExceptionReportPanel_SendMail"];
		}

		public void Initialize()
		{
			//exceptionComboBox.Items.Add();

			exceptionComboBox.SelectedIndexChanged += ExceptionComboBoxSelectedIndexChanged;

			for (var i = 5; i <= 100; i+=5)
			{
				incrementComboBox.Items.Add(i.ToString());
			}
			incrementComboBox.TextChanged += IncrementComboBoxTextChanged;

			for (var i = 5; i <= 100; i += 5)
			{
				thresholdComboBox.Items.Add(i.ToString());
			}
			thresholdComboBox.TextChanged += ThresholdComboBoxTextChanged;

			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.PDF));
			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.Word));
			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.Excel));
			formatComboBox.SelectedIndexChanged += FormatComboBoxSelectedIndexChanged;

			receiverComboBox.TextChanged += ReceiverTextBoxTextChanged;
			subjectTextBox.TextChanged += SubjectTextBoxTextChanged;
		}
		
		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, control);

			if (Localization.ContainsKey("ExceptionReportPanel_" + control.Tag))
				Manager.PaintText(g, Localization["ExceptionReportPanel_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		public void ParseReport()
		{
			_isEditing = false;

			exceptionComboBox.Items.Clear();
			var exceptions = (PTS.POS.Exceptions.ContainsKey(POS.Exception))
								 ? PTS.POS.Exceptions[POS.Exception]
								 : ((PTS.POS.Exceptions.Values.Count > 0)
									? PTS.POS.Exceptions.Values.First()
									:null);

			if (exceptions != null && exceptions.Exceptions.Count > 0)
			{
				var str = exceptions.Exceptions.ToList();
				str.Sort((x, y) => (String.Compare(x.Value, y.Value)));

                ComboxItem exceptionItem = null;
				foreach (var exception in str)
				{
				    var item = new ComboxItem(POS_Exception.FindExceptionValueByKey(exception.Value), exception.Key);
                    exceptionComboBox.Items.Add(item);
                    if (exception.Key == Report.Exception)
                        exceptionItem = item;
				}

                if(exceptionItem == null)
                {
                    exceptionComboBox.SelectedIndex = 0;
                }
                else
                {
                    exceptionComboBox.SelectedItem = exceptionItem;
                }
				
			}
			Manager.DropDownWidth(exceptionComboBox);
			thresholdComboBox.Text = Report.Threshold.ToString();
			incrementComboBox.Text = Report.Increment.ToString();
			
			switch (Report.ReportForm.Format)
			{
				case ReportFormat.PDF:
					formatComboBox.SelectedIndex = 0;
					break;

				case ReportFormat.Word:
					formatComboBox.SelectedIndex = 1;
					break;

				case ReportFormat.Excel:
					formatComboBox.SelectedIndex = 2;
					break;
			}

			receiverComboBox.Items.Clear();
			foreach (var obj in PTS.User.Users)
			{
				if (String.IsNullOrEmpty(obj.Value.Email.Trim())) continue;

				receiverComboBox.Items.Add(obj.Value.Email);
			}

			receiverComboBox.Text = Report.ReportForm.MailReceiver;
			subjectTextBox.Text = Report.ReportForm.Subject;
			
			Select();
			AutoScrollPosition = new Point(0, 0);

			_isEditing = true;
		}

		private Boolean _isEditing;

		private void ExceptionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

		    var item = (exceptionComboBox.SelectedItem) as ComboxItem;
            if (item == null) return;

            Report.Exception = item.Value;
			Report.ReportForm.Exceptions.Clear();
			Report.ReportForm.Exceptions.Add(Report.Exception);

			ExceptionReportModify();
		}

		private void ThresholdComboBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (String.IsNullOrEmpty(thresholdComboBox.Text)) return;

			Report.Threshold = Convert.ToUInt16(Math.Min(Math.Max(Convert.ToInt32(thresholdComboBox.Text), 1), 9999));

			ExceptionReportModify();
		}

		private void IncrementComboBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (String.IsNullOrEmpty(incrementComboBox.Text)) return;

			Report.Increment = Convert.ToUInt16(Math.Min(Math.Max(Convert.ToInt32(incrementComboBox.Text), 1), 9999));

			ExceptionReportModify();
		}

		private void FormatComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.Format = ReportFormats.ToIndex(formatComboBox.SelectedItem.ToString());

			ExceptionReportModify();
		}

		private void ReceiverTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.MailReceiver = receiverComboBox.Text;
			ExceptionReportModify();
		}

		private void SubjectTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.Subject = subjectTextBox.Text;
			ExceptionReportModify();
		}

		private void ExceptionReportModify()
		{
			if (Report.ReadyState == ReadyState.Ready)
				Report.ReadyState = ReadyState.Modify;

			if (POS.ExceptionReports.ReadyState == ReadyState.Ready)
				POS.ExceptionReports.ReadyState = ReadyState.Modify;

			if (POS.ReadyState == ReadyState.Ready)
				POS.ReadyState = ReadyState.Modify;
		}
	}
}
