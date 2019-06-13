using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Template
{
	public sealed class TemplatePanel : Panel
	{
		public event EventHandler OnTemplateEditClick;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;

		public POS_Exception.TemplateConfig Config;
		public IPTS PTS;

		public TemplatePanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_POS", "POS"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_ExceptionAmount", "Exception Amount"},
								   {"PTSReports_Tag", "Tag"},
								   {"PTSReports_Keyword", "Keyword"},

								   {"PTSReports_AllPOS", "All POS"},

								   {"Common_Today", "Today"},
								   {"Common_Yesterday", "Yesterday"},
								   {"Common_DayBeforeYesterday", "The day before yesterday"},
								   {"Common_ThisWeek", "This week"},
								   {"Common_ThisMonth", "This month"},
								   {"Common_LastMonth", "Last month"},
								   {"Common_TheMonthBeforeLast", "The month before last"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Height = 280;//40

			_checkBox = new CheckBox
			{
				Location = new Point(10, 128),
				Dock = DockStyle.None,
				Width = 25,
			};

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += TemplatePanelMouseClick;
			Paint += TemplatePanelPaint;
		}

		private void TemplatePanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (_checkBox.Visible)
			{
				_checkBox.Checked = !_checkBox.Checked;
				return;
			}
			if (OnTemplateEditClick != null)
				OnTemplateEditClick(this, e);
		}

		private void CheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			Invalidate();

			_checkBox.Focus();
			if (OnSelectChange != null)
				OnSelectChange(this, null);
		}

		private void TemplatePanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, this);
			if (_editVisible)
				Manager.PaintEdit(g, this, 135);

			if (Config == null) return;

			//tooooooo complicate
			//Manager.PaintStatus(g, Config.ReadyState, 134);

			if (Width < 200) return;

			Brush fontBrush = Brushes.Black;
			if (_checkBox.Visible && Checked)
			{
				fontBrush = Manager.DeleteTextColor;
			}
			PaintDateTime(g, fontBrush);
			PaintPOS(g, fontBrush);
			PaintCashierId(g, fontBrush);
			PaintCashier(g, fontBrush);
			PaintExceptionAmount(g, fontBrush);
			PaintTag(g, fontBrush);
			PaintKeyword(g, fontBrush);
		}

		private void PaintDateTime(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_DateTime"], fontBrush);

			switch (Config.DateTimeSet)
			{
				case DateTimeSet.Today:
					Manager.PaintTextRight(g, this, Localization["Common_Today"], fontBrush, 13);
					break;

				case DateTimeSet.Yesterday:
					Manager.PaintTextRight(g, this, Localization["Common_Yesterday"], fontBrush, 13);
					break;

				case DateTimeSet.DayBeforeYesterday:
					Manager.PaintTextRight(g, this, Localization["Common_DayBeforeYesterday"], fontBrush, 13);
					break;

				case DateTimeSet.ThisWeek:
					Manager.PaintTextRight(g, this, Localization["Common_ThisWeek"], fontBrush, 13);
					break;

				case DateTimeSet.ThisMonth:
					Manager.PaintTextRight(g, this, Localization["Common_ThisMonth"], fontBrush, 13);
					break;

				case DateTimeSet.LastMonth:
					Manager.PaintTextRight(g, this, Localization["Common_LastMonth"], fontBrush, 13);
					break;

				case DateTimeSet.TheMonthBeforeLast:
					Manager.PaintTextRight(g, this, Localization["Common_TheMonthBeforeLast"], fontBrush, 13);
					break;

				default:
					// HH:mm:ss
					Manager.PaintTextRight(g, this,
						DateTimes.ToDateTime(Config.StartDateTime, PTS.Server.TimeZone).ToString("MM-dd-yyyy") + @" ~ " +
						DateTimes.ToDateTime(Config.EndDateTime, PTS.Server.TimeZone).ToString("MM-dd-yyyy"),
						fontBrush, 13);
					break;
			}
		}

		private void PaintPOS(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_POS"], fontBrush, 44, 53);

			var containesAll = true;
			var posCriterias = Config.POSCriterias;
			foreach (IPOS pos in PTS.POS.POSServer)
			{
				var hasCriteria = false;
				foreach (var posCriteria in posCriterias)
				{
					if (posCriteria.POSId == pos.Id)
					{
						hasCriteria = true;
						break;
					}
				}
				if (!hasCriteria)
				{
					containesAll = false;
					break;
				}
			}

			if (containesAll && PTS.POS.POSServer.Count > 3)
			{
				Manager.PaintTextRight(g, this, Localization["PTSReports_AllPOS"], fontBrush, 53);
			}
			else
			{
				var posSelecton = new List<String>();

				var list = new List<String>();

				foreach (var posCriteria in Config.POSCriterias)
				{
					var pos = PTS.POS.FindPOSById(posCriteria.POSId);
					if (pos != null)
						list.Add(pos.ToString());
				}

				list.Sort();

				foreach (var pos in list)
				{
					if (posSelecton.Count >= 3)
					{
						posSelecton[2] += " ...";
						break;
					}

					if (String.IsNullOrEmpty(pos)) continue;

					posSelecton.Add(pos);
				}

				Manager.PaintTextRight(g, this, String.Join(", ", posSelecton.ToArray()), fontBrush, 53);
			}
		}

		private void PaintCashierId(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_CashierId"], fontBrush, 44, 93);

			var cashierIdSelecton = new List<String>();

			var list = new List<String>();

			foreach (var cashierIdCriteria in Config.CashierIdCriterias)
			{
				list.Add(cashierIdCriteria.CashierId);
			}

			list.Sort();

			foreach (var cashierId in list)
			{
				if (cashierIdSelecton.Count >= 3)
				{
					cashierIdSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(cashierId)) continue;

				cashierIdSelecton.Add(cashierId);
			}

			Manager.PaintTextRight(g, this, String.Join(", ", cashierIdSelecton.ToArray()), fontBrush, 93);
		}

		private void PaintCashier(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_Cashier"], fontBrush, 44, 133);

			var cashierSelecton = new List<String>();

			var list = new List<String>();

			foreach (var cashierCriteria in Config.CashierCriterias)
			{
				list.Add(cashierCriteria.Cashier);
			}

			list.Sort();

			foreach (var cashier in list)
			{
				if (cashierSelecton.Count >= 3)
				{
					cashierSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(cashier)) continue;

				cashierSelecton.Add(cashier);
			}

			Manager.PaintTextRight(g, this, String.Join(", ", cashierSelecton.ToArray()), fontBrush, 133);
		}

		private void PaintExceptionAmount(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_ExceptionAmount"], fontBrush, 44, 173);

			var exceptionAmountSelecton = new List<String>();

			var list = new List<String>();

			foreach (var exceptionAmountCriteria in Config.ExceptionAmountCriterias)
			{
				list.Add(exceptionAmountCriteria.Exception);
			}

			list.Sort();

			foreach (var exceptionAmount in list)
			{
				if (exceptionAmountSelecton.Count >= 3)
				{
					exceptionAmountSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(exceptionAmount)) continue;

                exceptionAmountSelecton.Add(POS_Exception.FindExceptionValueByKey(exceptionAmount));
			}

			Manager.PaintTextRight(g, this, String.Join(", ", exceptionAmountSelecton.ToArray()), fontBrush, 173);
		}

		private void PaintTag(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_Tag"], fontBrush, 44, 213);

			var tagSelecton = new List<String>();

			var list = new List<String>();

			foreach (var tagCriteria in Config.TagCriterias)
			{
				list.Add(tagCriteria.TagName);
			}

			list.Sort();

			foreach (var tag in list)
			{
				if (tagSelecton.Count >= 3)
				{
					tagSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(tag)) continue;

				tagSelecton.Add(tag);
			}

			Manager.PaintTextRight(g, this, String.Join(", ", tagSelecton.ToArray()), fontBrush, 213);
		}

		private void PaintKeyword(Graphics g, Brush fontBrush)
		{
			Manager.PaintText(g, Localization["PTSReports_Keyword"], fontBrush, 44, 253);

			var keywordSelecton = new List<String>();

			var list = new List<String>();

			foreach (var keywordCriteria in Config.KeywordCriterias)
			{
				list.Add(keywordCriteria.Keyword);
			}

			list.Sort();

			foreach (var keyword in list)
			{
				if (keywordSelecton.Count >= 3)
				{
					keywordSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(keyword)) continue;

				keywordSelecton.Add(keyword);
			}

			Manager.PaintTextRight(g, this, String.Join(", ", keywordSelecton.ToArray()), fontBrush, 253);
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
