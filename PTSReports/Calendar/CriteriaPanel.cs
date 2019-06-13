using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Calendar
{
	public sealed partial class CriteriaPanel : UserControl
	{
		public event EventHandler OnPOSEdit;
		public event EventHandler OnDateTimeEdit;
		public event EventHandler OnExceptionEdit;
		
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_AllPOS", "All POS"},
								   
								   {"Common_ThisMonth", "This month"},
								   {"Common_LastMonth", "Last month"},
								   {"Common_TheMonthBeforeLast", "The month before last"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
		}

		public void Initialize()
		{
			posDoubleBufferPanel.Paint += POSDoubleBufferPanelPaint;
			posDoubleBufferPanel.MouseClick += POSDoubleBufferPanelMouseClick;

			dateTimeDoubleBufferPanel.Paint += DateTimeDoubleBufferPanelPaint;
			dateTimeDoubleBufferPanel.MouseClick += DateTimeDoubleBufferPanelMouseClick;

			exceptionDoubleBufferPanel.Paint += ExceptionDoubleBufferPanelPaint;
			exceptionDoubleBufferPanel.MouseClick += ExceptionDoubleBufferPanelMouseClick;
		}

		private void InputPanelPaint(Panel panel, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, panel);
			Manager.PaintEdit(g, panel);

			if (Localization.ContainsKey("PTSReports_" + panel.Tag))
				Manager.PaintText(g, Localization["PTSReports_" + panel.Tag]);
			else
				Manager.PaintText(g, panel.Tag.ToString());
		}

		private void POSDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			InputPanelPaint(posDoubleBufferPanel, e);

			Graphics g = e.Graphics;
			
			var containesAll = true;
			foreach (IPOS pos in PTS.POS.POSServer)
			{
				if (!SearchCriteria.POS.Contains(pos.Id))
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

				foreach (var posId in SearchCriteria.POS)
				{
					var pos = PTS.POS.FindPOSById(posId);
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

		private void DateTimeDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			InputPanelPaint(dateTimeDoubleBufferPanel, e);

			Graphics g = e.Graphics;

			switch (SearchCriteria.DateTimeSet)
			{
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
					Manager.PaintTextRight(g, dateTimeDoubleBufferPanel,
						DateTimes.ToDateTime(SearchCriteria.StartDateTime, PTS.Server.TimeZone).ToString("yyyy-MM"));
					break;
			}
		}

		private void ExceptionDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			InputPanelPaint(exceptionDoubleBufferPanel, e);

			Graphics g = e.Graphics;

			var exceptionSelecton = new List<String>();

			var list = new List<String>();

			foreach (var exception in SearchCriteria.Exceptions)
			{
				list.Add(exception);
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
		
		private void POSDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPOSEdit != null)
				OnPOSEdit(this, e);
		}

		private void DateTimeDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnDateTimeEdit != null)
				OnDateTimeEdit(this, e);
		}

		private void ExceptionDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionEdit != null)
				OnExceptionEdit(this, e);
		}
		
		public void ShowCriteria()
		{
		}
	} 
}