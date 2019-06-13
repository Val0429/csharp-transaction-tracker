using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupScheduleReport
{
	public sealed class ScheduleReportPanel : Panel
	{
		public event EventHandler OnReportEditClick;
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;

		public ScheduleReport Report;
		public Boolean IsTitle;
		public IPTS PTS;

		public ScheduleReportPanel()
		{
			Localization = new Dictionary<String, String>
							   {
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
								   {"ScheduleReportPanel_Recipient", "Recipient"},
								   
								   {"ScheduleReportPanel_Daily", "Daily Report"},
								   {"ScheduleReportPanel_Weekly", "Weekly Report"},
								   {"ScheduleReportPanel_Monthly", "Monthly Report"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Height = 40;

			_checkBox = new CheckBox
			{
				Location = new Point(10, 8),
				Dock = DockStyle.None,
				Width = 25,
			};

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += ScheduleReportPanelMouseClick;
			Paint += ScheduleReportPanelPaint;
		}

		private void ScheduleReportPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsTitle)
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
					return;
				}
			}
			else
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
					return;
				}
				if (OnReportEditClick != null)
					OnReportEditClick(this, e);
			}
		}

		private void CheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			Invalidate();

			if (IsTitle)
			{
				if (Checked && OnSelectAll != null)
					OnSelectAll(this, null);
				else if (!Checked && OnSelectNone != null)
					OnSelectNone(this, null);

				return;
			}

			_checkBox.Focus();
			if (OnSelectChange != null)
				OnSelectChange(this, null);
		}

		//public Brush SelectedColor = Manager.SelectedTextColor;

		private void ScheduleReportPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			if (IsTitle)
			{
				Manager.PaintTitleTopInput(g, this);
				PaintTitle(g);
				return;
			}

			Manager.Paint(g, this);

			if (_editVisible)
				Manager.PaintEdit(g, this);

			if (Report == null) return;

			Manager.PaintStatus(g, Report.ReadyState);

			if (Width < 200) return;

			Brush fontBrush = Brushes.Black;
			if (_checkBox.Visible && Checked)
			{
				fontBrush = Manager.DeleteTextColor;
			}

			switch (Report.Period)
			{
				case ReportPeriod.Daily:
					Manager.PaintText(g, Localization["ScheduleReportPanel_Daily"], fontBrush);
					break;

				case ReportPeriod.Weekly:
					Manager.PaintText(g, Localization["ScheduleReportPanel_Weekly"], fontBrush);
					break;

				case ReportPeriod.Monthly:
					Manager.PaintText(g, Localization["ScheduleReportPanel_Monthly"], fontBrush);
					break;
			}

			if (Width < 300) return;
			g.DrawString(ReportFormats.ToString(Report.ReportForm.Format), Manager.Font, fontBrush, 200, 13);


			if (Width < 400) return;
			var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(Report.Time);
			g.DrawString(date.ToString("HH:mm"), Manager.Font, fontBrush, 300, 13);

			if(Width < 500) return;

			var days = new List<String>();
			switch (Report.Period)
			{
				case ReportPeriod.Daily:
					foreach (var day in Report.Days)
					{
						if (days.Count >= 1)
						{
							days[0] += " ...";
							break;
						}

						switch (day)
						{
							case 0:
								days.Add(Localization["Common_Sun"]);
								break;

							case 1:
								days.Add(Localization["Common_Mon"]);
								break;

							case 2:
								days.Add(Localization["Common_Tue"]);
								break;

							case 3:
								days.Add(Localization["Common_Wed"]);
								break;

							case 4:
								days.Add(Localization["Common_Thu"]);
								break;

							case 5:
								days.Add(Localization["Common_Fri"]);
								break;

							case 6:
								days.Add(Localization["Common_Sat"]);
								break;

						}
					}
					g.DrawString(String.Join(",", days.ToArray()), Manager.Font, fontBrush, 400, 13);
					break;

				case ReportPeriod.Weekly:
					foreach (var day in Report.Days)
					{
						if (days.Count >= 1)
						{
							days[0] += " ...";
							break;
						}

						switch (day)
						{
							case 0:
								days.Add(Localization["Common_Sun"]);
								break;

							case 1:
								days.Add(Localization["Common_Mon"]);
								break;

							case 2:
								days.Add(Localization["Common_Tue"]);
								break;

							case 3:
								days.Add(Localization["Common_Wed"]);
								break;

							case 4:
								days.Add(Localization["Common_Thu"]);
								break;

							case 5:
								days.Add(Localization["Common_Fri"]);
								break;

							case 6:
								days.Add(Localization["Common_Sat"]);
								break;

						}
					}
					g.DrawString(String.Join(",", days.ToArray()), Manager.Font, fontBrush, 400, 13);
					break;

				case ReportPeriod.Monthly:
					g.DrawString(Report.Days.First().ToString(), Manager.Font, fontBrush, 400, 13);
					break;
			}

			var posSelecton = new List<String>();
			foreach (var posId in Report.ReportForm.POS)
			{
				if (posSelecton.Count >= 1)
				{
					posSelecton[0] += " ...";
					break;
				}

				var pos = PTS.POS.FindPOSById(posId);
				if (pos != null)
					posSelecton.Add(pos.ToString());
			}

			if (Width < 650) return;
			g.DrawString(String.Join(",", posSelecton.ToArray()), Manager.Font, fontBrush, 500, 13);

			var list = new List<String> (Report.ReportForm.Exceptions.ToArray());
			list.Sort();
			var exceptionSelecton = new List<String>();
			foreach (var exception in list)
			{
				if (exceptionSelecton.Count >= 1)
				{
					exceptionSelecton[0] += " ...";
					break;
				}

                exceptionSelecton.Add(POS_Exception.FindExceptionValueByKey(exception));
			}

			if (Width < 800) return;
			g.DrawString(String.Join(",", exceptionSelecton.ToArray()), Manager.Font, fontBrush, 650, 13);

			if (Width < 900) return;
			g.DrawString(Report.ReportForm.MailReceiver, Manager.Font, fontBrush, 800, 13);
		}

		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["ScheduleReportPanel_Type"]);

			if (Width < 300) return;
			g.DrawString(Localization["ScheduleReportPanel_Format"], Manager.Font, Manager.TitleTextColor, 200, 13);

			if (Width < 400) return;
			g.DrawString(Localization["ScheduleReportPanel_SendTime"], Manager.Font, Manager.TitleTextColor, 300, 13);

			if (Width < 500) return;
			g.DrawString(Localization["ScheduleReportPanel_SendDay"], Manager.Font, Manager.TitleTextColor, 400, 13);

			if (Width < 650) return;
			g.DrawString(Localization["ScheduleReportPanel_POS"], Manager.Font, Manager.TitleTextColor, 500, 13);

			if (Width < 800) return;
			g.DrawString(Localization["ScheduleReportPanel_Exception"], Manager.Font, Manager.TitleTextColor, 650, 13);

			if (Width < 900) return;
			g.DrawString(Localization["ScheduleReportPanel_Recipient"], Manager.Font, Manager.TitleTextColor, 800, 13);
		}

		public Boolean SelectionVisible
		{
			set { _checkBox.Visible = value; }
		}

		public Boolean Checked
		{
			set { _checkBox.Checked = value; }
			get { return _checkBox.Checked; }
		}

		private Boolean _editVisible;
		public Boolean EditVisible
		{
			set
			{
				_editVisible = value;
				Invalidate();
			}
		}
	}
}
