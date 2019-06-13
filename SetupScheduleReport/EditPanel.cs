using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PTSReports.Base;
using SetupBase;

namespace SetupScheduleReport
{
	public sealed partial class EditPanel : UserControl
	{
		public IPTS PTS;
		public Dictionary<String, String> Localization;
		public ScheduleReport Report;
		private readonly Dictionary<UInt16, CheckBox> _dailyCheckBoxDic = new Dictionary<UInt16, CheckBox>();
		private readonly Dictionary<UInt16, RadioButton> _dailyRadioButtonDic = new Dictionary<UInt16, RadioButton>();
        //private POSPanel _posSelectionPanel;
        private StorePanel _posSelectionPanel;
        private ExceptionPanel _exceptionSelectionPanel;
		private readonly POS_Exception.SearchCriteria _searchCriteria = new POS_Exception.SearchCriteria();
		public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Manufacture", "Manufacture"},
								   {"PTSReports_AllException", "All Exception"},

								   {"Common_Mon", "Mon"},
								   {"Common_Tue", "Tue"},
								   {"Common_Wed", "Wed"},
								   {"Common_Thu", "Thu"},
								   {"Common_Fri", "Fri"},
								   {"Common_Sat", "Sat"},
								   {"Common_Sun", "Sun"},

								   {"ScheduleReportPanel_Type", "Type"},
								   {"ScheduleReportPanel_SendTime", "Send time"},
								   {"ScheduleReportPanel_SendDay", "Send day"},
								   {"ScheduleReportPanel_Format", "Format"},
								   {"ScheduleReportPanel_POS", "POS"},
								   {"ScheduleReportPanel_Exception", "Exception"},
								   {"ScheduleReportPanel_SendMail", "Send mail"},
								   {"ScheduleReportPanel_Recipient", "Recipient"},
								   {"ScheduleReportPanel_Subject", "Subject"},

								   {"ScheduleReportPanel_Daily", "Daily Report"},
								   {"ScheduleReportPanel_Weekly", "Weekly Report"},
								   {"ScheduleReportPanel_Monthly", "Monthly Report"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "ScheduleReport";
			BackgroundImage = Manager.BackgroundNoBorder;

			typePanel.Paint += InputPanelPaint;
			sendTimePanel.Paint += InputPanelPaint;
			weekdaySelectPanel.Paint += InputPanelPaint;
			weekdayRadioSelectPanel.Paint += InputPanelPaint;
			monthDaySelectPanel.Paint += InputPanelPaint;
			formatPanel.Paint += InputPanelPaint;
			recipientPanel.Paint += InputPanelPaint;
			subjectPanel.Paint += InputPanelPaint;
			filterPanel.Paint += FilterPanelPaint;

			filterComboBox.Items.Add(Localization["PTSReports_AllException"]);
			foreach (var manufacture in POS_Exception.Manufactures)
			{
				filterComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
			}
			filterComboBox.SelectedIndex = 0;
			filterComboBox.SelectedIndexChanged += FilterComboBoxSelectedIndexChanged;

			_dailyCheckBoxDic.Add(0, sunCheckBox);
			_dailyCheckBoxDic.Add(1, monCheckBox);
			_dailyCheckBoxDic.Add(2, tueCheckBox);
			_dailyCheckBoxDic.Add(3, wedCheckBox);
			_dailyCheckBoxDic.Add(4, thuCheckBox);
			_dailyCheckBoxDic.Add(5, friCheckBox);
			_dailyCheckBoxDic.Add(6, satCheckBox);

			_dailyRadioButtonDic.Add(0, sunRadioButton);
			_dailyRadioButtonDic.Add(1, monRadioButton);
			_dailyRadioButtonDic.Add(2, tueRadioButton);
			_dailyRadioButtonDic.Add(3, wedRadioButton);
			_dailyRadioButtonDic.Add(4, thuRadioButton);
			_dailyRadioButtonDic.Add(5, friRadioButton);
			_dailyRadioButtonDic.Add(6, satRadioButton);

			sunCheckBox.Text = sunRadioButton.Text = Localization["Common_Sun"];
			monCheckBox.Text = monRadioButton.Text = Localization["Common_Mon"];
			tueCheckBox.Text = tueRadioButton.Text = Localization["Common_Tue"];
			wedCheckBox.Text = wedRadioButton.Text = Localization["Common_Wed"];
			thuCheckBox.Text = thuRadioButton.Text = Localization["Common_Thu"];
			friCheckBox.Text = friRadioButton.Text = Localization["Common_Fri"];
			satCheckBox.Text = satRadioButton.Text = Localization["Common_Sat"];
			
			//posLabel.Text = Localization["ScheduleReportPanel_POS"];
			exceptionLabel.Text = Localization["ScheduleReportPanel_Exception"];
			mailLabel.Text = Localization["ScheduleReportPanel_SendMail"];
		}

		public void Initialize()
		{
            //_posSelectionPanel = new POSPanel
            _posSelectionPanel = new StorePanel
            {
				PTS = PTS,
				SearchCriteria = _searchCriteria,
			};
			_posSelectionPanel.OnSelectChange += PosSelectionPanelOnSelectChange;
			_posSelectionPanel.ContainerPanel.AutoSize = true;
            posPanel.Controls.Add(_posSelectionPanel.ContainerPanel);
            posPanel.Controls.Add(_posSelectionPanel.pageSelectorPanel);

            _exceptionSelectionPanel = new ExceptionPanel
			{
				PTS = PTS,
				SearchCriteria = _searchCriteria,
			};
			_exceptionSelectionPanel.OnSelectChange += ExceptionSelectionPanelOnSelectChange;
			_exceptionSelectionPanel.ContainerPanel.AutoSize = true;
			exceptionPanel.Controls.Add(_exceptionSelectionPanel.ContainerPanel);

			periodComboBox.Items.Add(Localization["ScheduleReportPanel_Daily"]);
			periodComboBox.Items.Add(Localization["ScheduleReportPanel_Weekly"]);
			periodComboBox.Items.Add(Localization["ScheduleReportPanel_Monthly"]);

			periodComboBox.SelectedIndexChanged += PeriodComboBoxSelectedIndexChanged;

			for (var i = 1; i <= 31; i++)
			{
				dayComboBox.Items.Add(i.ToString());
			}

			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.PDF));
			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.Word));
			formatComboBox.Items.Add(ReportFormats.ToString(ReportFormat.Excel));

			dayComboBox.SelectedIndexChanged += DayComboBoxSelectedIndexChanged;
			timePicker.ValueChanged += TimePickerValueChanged;
			formatComboBox.SelectedIndexChanged += FormatComboBoxSelectedIndexChanged;

			sunCheckBox.CheckedChanged += SunCheckBoxCheckedChanged;
			monCheckBox.CheckedChanged += MonCheckBoxCheckedChanged;
			tueCheckBox.CheckedChanged += TueCheckBoxCheckedChanged;
			wedCheckBox.CheckedChanged += WedCheckBoxCheckedChanged;
			thuCheckBox.CheckedChanged += ThuCheckBoxCheckedChanged;
			friCheckBox.CheckedChanged += FriCheckBoxCheckedChanged;
			satCheckBox.CheckedChanged += SatCheckBoxCheckedChanged;

			sunRadioButton.CheckedChanged += SunRadioButtonCheckedChanged;
			monRadioButton.CheckedChanged += MonRadioButtonCheckedChanged;
			tueRadioButton.CheckedChanged += TueRadioButtonCheckedChanged;
			wedRadioButton.CheckedChanged += WedRadioButtonCheckedChanged;
			thuRadioButton.CheckedChanged += ThuRadioButtonCheckedChanged;
			friRadioButton.CheckedChanged += FriRadioButtonCheckedChanged;
			satRadioButton.CheckedChanged += SatRadioButtonCheckedChanged;

			receiverComboBox.TextChanged += ReceiverTextBoxTextChanged;
			subjectTextBox.TextChanged += SubjectTextBoxTextChanged;
		}

		private void FilterComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			FilterException();
		}

		private void FilterException()
		{
			if (filterComboBox.SelectedIndex == 0)
				_exceptionSelectionPanel.FilterExceptionByManufacture("");
			else
				_exceptionSelectionPanel.FilterExceptionByManufacture(filterComboBox.SelectedItem.ToString());
		}


		private void PosSelectionPanelOnSelectChange(Object sender, EventArgs e)
		{
			Report.ReportForm.POS.Clear();
			Report.ReportForm.POS.AddRange(_searchCriteria.POS);

            Report.ReportForm.Store.Clear();
            Report.ReportForm.Store.AddRange(_searchCriteria.Store);


            ScheduleReportModify();
		}

		private void ExceptionSelectionPanelOnSelectChange(Object sender, EventArgs e)
		{
			Report.ReportForm.Exceptions.Clear();
			Report.ReportForm.Exceptions.AddRange(_searchCriteria.Exceptions);
			Report.ReportForm.Exceptions.Sort((x, y) => (y.CompareTo(x)));
			ScheduleReportModify();
		}
		
		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, control);

			if (Localization.ContainsKey("ScheduleReportPanel_" + control.Tag))
				Manager.PaintText(g, Localization["ScheduleReportPanel_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		private void FilterPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, filterPanel);

			if (filterPanel.Width <= 100) return;

			Manager.PaintText(g, Localization["PTSReports_Manufacture"]);
		}

		public void ParseReport()
		{
			ParsePeriodAndDaySetting();

			_isEditing = false;


			_searchCriteria.POS.Clear();
            _searchCriteria.Store.Clear();
            _searchCriteria.Exceptions.Clear();

			_searchCriteria.POS.AddRange(Report.ReportForm.POS);
            _searchCriteria.Store.AddRange(Report.ReportForm.Store);
            _searchCriteria.Exceptions.AddRange(Report.ReportForm.Exceptions);

			_posSelectionPanel.ParseSetting();
			//posPanel.Height =  _posSelectionPanel.ContainerPanel.Controls.Count * 40;

			_exceptionSelectionPanel.ParseSetting();
			FilterException();
			//exceptionPanel.Height = _exceptionSelectionPanel.ContainerPanel.Controls.Count * 40;

			timePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(Report.Time);

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

		private void ParsePeriodAndDaySetting()
		{
			_isEditing = false;

			switch (Report.Period)
			{
				case ReportPeriod.Daily:
					periodComboBox.SelectedIndex = 0;
					weekdaySelectPanel.Visible = true;
					weekdayRadioSelectPanel.Visible = false;
					monthDaySelectPanel.Visible = false;

					foreach (var obj in _dailyCheckBoxDic)
					{
						obj.Value.Checked = false;
					}

					foreach (var day in Report.Days)
					{
						if (_dailyCheckBoxDic.ContainsKey(day))
							_dailyCheckBoxDic[day].Checked = true;
					}
					break;

				case ReportPeriod.Weekly:
					periodComboBox.SelectedIndex = 1;
					weekdayRadioSelectPanel.Visible = true;
					weekdaySelectPanel.Visible = false;
					monthDaySelectPanel.Visible = false;

					foreach (var obj in _dailyRadioButtonDic)
					{
						obj.Value.Checked = false;
					}
					while (Report.Days.Count > 1)
					{
						Report.Days.Remove(Report.Days[Report.Days.Count - 1]);
					}

					foreach (var day in Report.Days)
					{
						if (_dailyRadioButtonDic.ContainsKey(day))
							_dailyRadioButtonDic[day].Checked = true;
					}
					break;

				case ReportPeriod.Monthly:
					periodComboBox.SelectedIndex = 2;
					monthDaySelectPanel.Visible = true;
					weekdaySelectPanel.Visible = false;
					weekdayRadioSelectPanel.Visible = false;

					dayComboBox.SelectedIndex = Report.Days.First() - 1;
					break;
			}

			_isEditing = true;
		}

		private Boolean _isEditing;

		private void PeriodComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.Days.Clear();
			switch (periodComboBox.SelectedIndex)
			{
				case 0:
					Report.Period = ReportPeriod.Daily;
					Report.Days.AddRange(new List<UInt16> { 0, 1, 2, 3, 4, 5, 6 }); //full week sending daily report
					break;

				case 1:
					Report.Period = ReportPeriod.Weekly;
					Report.Days.Add(0);//sunday send week report
					break;

				case 2:
					Report.Period = ReportPeriod.Monthly;
					Report.Days.Add(1);//1st day send month report
					break;
			}
			
			ParsePeriodAndDaySetting();

			ScheduleReportModify();
		}

		private void TimePickerValueChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.Time = timePicker.Value.Hour * 60 * 60 + timePicker.Value.Minute * 60 + timePicker.Value.Second;

			ScheduleReportModify();
		}

		private void DayComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.Days.Clear();

			Report.Days.Add(Convert.ToUInt16(dayComboBox.SelectedItem.ToString()));

			ScheduleReportModify();
		}

		private void FormatComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.Format = ReportFormats.ToIndex(formatComboBox.SelectedItem.ToString());

			ScheduleReportModify();
		}

		private void SunCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (sunCheckBox.Checked)
				Report.Days.Add(0);
			else
				Report.Days.Remove(0);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void MonCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (monCheckBox.Checked)
				Report.Days.Add(1);
			else
				Report.Days.Remove(1);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void TueCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (tueCheckBox.Checked)
				Report.Days.Add(2);
			else
				Report.Days.Remove(2);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void WedCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (wedCheckBox.Checked)
				Report.Days.Add(3);
			else
				Report.Days.Remove(3);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void ThuCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (thuCheckBox.Checked)
				Report.Days.Add(4);
			else
				Report.Days.Remove(4);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void FriCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (friCheckBox.Checked)
				Report.Days.Add(5);
			else
				Report.Days.Remove(5);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void SatCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			if (satCheckBox.Checked)
				Report.Days.Add(6);
			else
				Report.Days.Remove(6);

			Report.Days.Sort();
			ScheduleReportModify();
		}

		private void SunRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (sunRadioButton.Checked)
				Report.Days.Add(0);
			ScheduleReportModify();
		}

		private void MonRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (monRadioButton.Checked)
				Report.Days.Add(1);
			ScheduleReportModify();
		}

		private void TueRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (tueRadioButton.Checked)
				Report.Days.Add(2);
			ScheduleReportModify();
		}

		private void WedRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (wedRadioButton.Checked)
				Report.Days.Add(3);
			ScheduleReportModify();
		}

		private void ThuRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (thuRadioButton.Checked)
				Report.Days.Add(4);
			ScheduleReportModify();
		}

		private void FriRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (friRadioButton.Checked)
				Report.Days.Add(5);
			ScheduleReportModify();
		}

		private void SatRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			Report.Days.Clear();

			if (satRadioButton.Checked)
				Report.Days.Add(6);
			ScheduleReportModify();
		}

		private void ReceiverTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.MailReceiver = receiverComboBox.Text;
			ScheduleReportModify();
		}

		private void SubjectTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			Report.ReportForm.Subject = subjectTextBox.Text;
			ScheduleReportModify();
		}

		private void ScheduleReportModify()
		{
			PTS.POS.ScheduleReports.ReadyState = ReadyState.Modify;

			if (Report.ReadyState == ReadyState.Ready)
				Report.ReadyState = ReadyState.Modify;
		}
	}
}
