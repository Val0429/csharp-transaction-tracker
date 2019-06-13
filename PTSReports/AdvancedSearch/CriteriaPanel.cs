using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class CriteriaPanel : UserControl
	{
		public event EventHandler OnPOSEdit;
		public event EventHandler OnDateTimeEdit;
		public event EventHandler OnCashierIdEdit;
		public event EventHandler OnCashierEdit;
		public event EventHandler OnExceptionEdit;
		public event EventHandler OnExceptionAmountEdit;
		public event EventHandler OnTagEdit;
		public event EventHandler OnKeywordEdit;
        public event EventHandler OnTimeIntervalEdit;
        public event EventHandler OnCountingEdit;

		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public POS_Exception.AdvancedSearchCriteria SearchCriteria;

		public CriteriaPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_POS", "POS"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_ExceptionAmount", "Exception Amount"},
								   {"PTSReports_Tag", "Tag"},
								   {"PTSReports_Keyword", "Keyword"},
                                   {"PTSReports_TimeIntervel", "Time Interval"},
                                   {"PTSReports_CountingDiscrepancies", "Counting Discrepancies"},

								   {"PTSReports_AllPOS", "All POS"},

								   {"Common_Today", "Today"},
								   {"Common_Yesterday", "Yesterday"},
								   {"Common_DayBeforeYesterday", "The day before yesterday"},
								   {"Common_ThisWeek", "This week"},
								   {"Common_ThisMonth", "This month"},
								   {"Common_LastMonth", "Last month"},
								   {"Common_TheMonthBeforeLast", "The month before last"},

                                    {"Common_Sec", "Sec"},
                                    {"Common_Pcs", "Piece"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
		}

		public void Initialize()
		{
			dateTimeDoubleBufferPanel.Paint += DateTimeDoubleBufferPanelPaint;
			dateTimeDoubleBufferPanel.MouseClick += DateTimeDoubleBufferPanelMouseClick;

			posDoubleBufferPanel.Paint += POSDubleBufferPanelPaint;
			posDoubleBufferPanel.MouseClick += POSDubleBufferPanelMouseClick;

			cashierIdDoubleBufferPanel.Paint += CashierIdDoubleBufferPanelPaint;
			cashierIdDoubleBufferPanel.MouseClick += CashierIdDoubleBufferPanelMouseClick;

			cashierDoubleBufferPanel.Paint += CashierDoubleBufferPanelPaint;
			cashierDoubleBufferPanel.MouseClick += CashierDoubleBufferPanelMouseClick;

			exceptionDoubleBufferPanel.Paint += ExceptionDoubleBufferPanelPaint;
			exceptionDoubleBufferPanel.MouseClick += ExceptionDoubleBufferPanelMouseClick;

			exceptionAmountDoubleBufferPanel.Paint += ExceptionAmountDoubleBufferPanelPaint;
			exceptionAmountDoubleBufferPanel.MouseClick += ExceptionAmountDoubleBufferPanelMouseClick;

			tagDoubleBufferPanel.Paint += TagDoubleBufferPanelPaint;
			tagDoubleBufferPanel.MouseClick += TagDoubleBufferPanelMouseClick;

			keywordDoubleBufferPanel.Paint += KeywordDoubleBufferPanelPaint;
			keywordDoubleBufferPanel.MouseClick += KeywordDoubleBufferPanelMouseClick;

            timeIntervalDoubleBufferPanel.Paint += TimeIntervalDoubleBufferPanelPaint;
            timeIntervalDoubleBufferPanel.Click += TimeIntervalDoubleBufferPanelClick;

            countingDiscrepanciesDoubleBufferPanel.Paint += CountingDiscrepanciesDoubleBufferPanelPaint;
            countingDiscrepanciesDoubleBufferPanel.Click += CountingDiscrepanciesDoubleBufferPanelClick;
            
            // these functions for demo
		    countingDiscrepanciesDoubleBufferPanel.Visible =
		    timeIntervalDoubleBufferPanel.Visible = timeIntervalLabel.Visible = false;
		}

		private void DateTimeDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, dateTimeDoubleBufferPanel);
			Manager.PaintEdit(g, dateTimeDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_DateTime"]);
			
			switch (SearchCriteria.DateTimeSet)
			{
				case DateTimeSet.Today:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_Today"]);
					break;
					
				case DateTimeSet.Yesterday:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_Yesterday"]);
					break;

				case DateTimeSet.DayBeforeYesterday:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_DayBeforeYesterday"]);
					break;

				case DateTimeSet.ThisWeek:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_ThisWeek"]);
					break;

				case DateTimeSet.ThisMonth:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_ThisMonth"]);
					break;

				case DateTimeSet.LastMonth:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_LastMonth"]);
					break;

				case DateTimeSet.TheMonthBeforeLast:
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_TheMonthBeforeLast"]);
					break;

				default:
					// HH:mm:ss
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel,
						DateTimes.ToDateTime(SearchCriteria.StartDateTime, PTS.Server.TimeZone).ToString("MM-dd-yyyy") + @" ~ " +
						DateTimes.ToDateTime(SearchCriteria.EndDateTime, PTS.Server.TimeZone).ToString("MM-dd-yyyy"));
					break;
			}
		}

		private void POSDubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, posDoubleBufferPanel);

			Manager.PaintEdit(g, posDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_POS"]);

			var containesAll = true;
			var posCriterias = SearchCriteria.POSCriterias;
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
				Manager.PaintTextRight(g, posDoubleBufferPanel, Localization["PTSReports_AllPOS"]);
			}
			else
			{
				var posSelecton = new List<String>();

				var list = new List<String>();

				foreach (var posCriteria in SearchCriteria.POSCriterias)
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

				Manager.PaintTextRight(g, posDoubleBufferPanel, String.Join(", ", posSelecton.ToArray()));
			}
		}

		private void CashierIdDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, cashierIdDoubleBufferPanel);

			Manager.PaintEdit(g, cashierIdDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_CashierId"]);

			var cashierIdSelecton = new List<String>();

			var list = new List<String>();

			foreach (var cashierIdCriteria in SearchCriteria.CashierIdCriterias)
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

			Manager.PaintTextRight(g, cashierIdDoubleBufferPanel, String.Join(", ", cashierIdSelecton.ToArray()));
		}

		private void CashierDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, cashierDoubleBufferPanel);

			Manager.PaintEdit(g, cashierDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Cashier"]);

			var cashierSelecton = new List<String>();

			var list = new List<String>();

			foreach (var cashierCriteria in SearchCriteria.CashierCriterias)
			{
				list.Add(cashierCriteria.Cashier);
			}

			list.Sort();

			foreach (var cashierId in list)
			{
				if (cashierSelecton.Count >= 3)
				{
					cashierSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(cashierId)) continue;

				cashierSelecton.Add(cashierId);
			}

			Manager.PaintTextRight(g, cashierDoubleBufferPanel, String.Join(", ", cashierSelecton.ToArray()));
		}

		private void ExceptionDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, exceptionDoubleBufferPanel);

			Manager.PaintEdit(g, exceptionDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Exception"]);

			var exceptionSelecton = new List<String>();

			var list = new List<String>();

			foreach (var exceptionCriteria in SearchCriteria.ExceptionCriterias)
			{
				list.Add(exceptionCriteria.Exception);
			}

			list.Sort();

			foreach (var exception in list)
			{
				if (exceptionSelecton.Count >= 3)
				{
					exceptionSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(exception)) continue;

                exceptionSelecton.Add(POS_Exception.FindExceptionValueByKey(exception));
			}

			Manager.PaintTextRight(g, exceptionDoubleBufferPanel, String.Join(", ", exceptionSelecton.ToArray()));
		}

		private void ExceptionAmountDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, exceptionAmountDoubleBufferPanel);

			Manager.PaintEdit(g, exceptionAmountDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_ExceptionAmount"]);

			var list = new List<String>();

			var exceptionAmountSelecton = new List<String>();
			foreach (var exceptionAmountCriteria in SearchCriteria.ExceptionAmountCriterias)
			{
				list.Add(exceptionAmountCriteria.Exception);
			}

			list.Sort();

			foreach (var exception in list)
			{
				if (exceptionAmountSelecton.Count >= 3)
				{
					exceptionAmountSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(exception)) continue;

                exceptionAmountSelecton.Add(POS_Exception.FindExceptionValueByKey(exception));
			}

			Manager.PaintTextRight(g, exceptionAmountDoubleBufferPanel, String.Join(", ", exceptionAmountSelecton.ToArray()));
		}

		private void TagDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, tagDoubleBufferPanel);

			Manager.PaintEdit(g, tagDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Tag"]);

			var tagSelecton = new List<String>();

			var list = new List<String>();

			foreach (var tagCriteria in SearchCriteria.TagCriterias)
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

			Manager.PaintTextRight(g, tagDoubleBufferPanel, String.Join(", ", tagSelecton.ToArray()));
		}

		private void KeywordDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, keywordDoubleBufferPanel);

			Manager.PaintEdit(g, keywordDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Keyword"]);

			var keywordSelecton = new List<String>();

			var list = new List<String>();

			foreach (var keywordCriteria in SearchCriteria.KeywordCriterias)
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

			Manager.PaintTextRight(g, keywordDoubleBufferPanel, String.Join(", ", keywordSelecton.ToArray()));
		}

        private void TimeIntervalDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, timeIntervalDoubleBufferPanel);

            Manager.PaintEdit(g, timeIntervalDoubleBufferPanel);

            Manager.PaintText(g, Localization["PTSReports_TimeIntervel"]);

            Manager.PaintTextRight(g, timeIntervalDoubleBufferPanel, SearchCriteria.TimeIntervalCriteria.Enable ? SearchCriteria.TimeIntervalCriteria.Sec + "  "+ Localization["Common_Sec"] : String.Empty);
        }

        private void CountingDiscrepanciesDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, countingDiscrepanciesDoubleBufferPanel);

            Manager.PaintEdit(g, countingDiscrepanciesDoubleBufferPanel);

            Manager.PaintText(g, Localization["PTSReports_CountingDiscrepancies"]);

            Manager.PaintTextRight(g, countingDiscrepanciesDoubleBufferPanel, SearchCriteria.CountingCriteria.Enable ? SearchCriteria.CountingCriteria.Piece + "  " + Localization["Common_Pcs"] : String.Empty);
        }

		private void DateTimeDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnDateTimeEdit != null)
				OnDateTimeEdit(this, e);
		}

		private void POSDubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPOSEdit != null)
				OnPOSEdit(this, e);
		}

		private void CashierIdDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnCashierIdEdit != null)
				OnCashierIdEdit(this, e);
		}

		private void CashierDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnCashierEdit != null)
				OnCashierEdit(this, e);
		}

		private void ExceptionDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionEdit != null)
				OnExceptionEdit(this, e);
		}

		private void ExceptionAmountDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionAmountEdit != null)
				OnExceptionAmountEdit(this, e);
		}

		private void TagDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnTagEdit != null)
				OnTagEdit(this, e);
		}

		private void KeywordDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnKeywordEdit != null)
				OnKeywordEdit(this, e);
		}

        private void TimeIntervalDoubleBufferPanelClick(object sender, EventArgs e)
        {
            if (OnTimeIntervalEdit != null)
                OnTimeIntervalEdit(this, e);
        }

        private void CountingDiscrepanciesDoubleBufferPanelClick(object sender, EventArgs e)
        {
            if (OnCountingEdit != null)
                OnCountingEdit(this, e);
        }

		public void ShowCriteria()
		{
		}
	} 
}