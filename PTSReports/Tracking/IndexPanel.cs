using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Tracking
{
	public sealed partial class IndexPanel : UserControl
	{
		public event EventHandler<EventArgs<String>> OnReportSelect;
		
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public POS_Exception.SearchCriteria SearchCriteria;

		public IndexPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_ExceptionTracking", "Exception Tracking"},
								   {"PTSReports_EmployeesProductivityLossTracking", "Employees Productivity Loss Tracking"},

								   {"PTSReports_FrequentExceptionCategoriesIncurred", "Frequent exception categories"},
								   {"PTSReports_FrequentExceptionsIncurredByEmployees", "Frequent exception categories by employees"},
								   {"PTSReports_PerformanceDeviationAgainstPredefinedThresholds", "Performance deviation against predefined thresholds"},
								   {"PTSReports_HighAlertTop2EmployeesExceptionMonitoring", "Ranking of incidence with counts and employees"},
								   {"PTSReports_EmployeeProductivityLossRanking", "Ranking of employees with the highest incidence"},
								   {"PTSReports_EmployeeProductivityImprovementTrend", "Incidence Trend of employees"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;


			exceptionTrackingLabel.Text = Localization["PTSReports_ExceptionTracking"];
			employeesProductivityLossTrackingLabel.Text = Localization["PTSReports_EmployeesProductivityLossTracking"];

			frequentExceptionCategoriesIncurredPanel.Paint += PaintInput;
			frequentExceptionsIncurredByEmployeesPanel.Paint += PaintInput;
			performanceDeviationAgainstPredefinedThresholdsPanel.Paint += PaintInput;
			highAlertTop2EmployeesExceptionMonitoringPanel.Paint += PaintInput;
			employeeProductivityLossRankingPanel.Paint += PaintInput;
			employeeProductivityImprovementTrendPanel.Paint += PaintInput;
		}

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			var control = (Control)sender;

			if (control == null || control.Parent == null) return;

			var g = e.Graphics;
			if (PTS != null && PTS.License.Amount > 0)
			{
				Manager.PaintHighLightInput(g, control);
				Manager.PaintEdit(g, control);
			}
			else
			{
				Manager.PaintSingleInput(g, control);
			}

			if (Localization.ContainsKey("PTSReports_" + control.Tag))
				Manager.PaintText(g, Localization["PTSReports_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		public void Activate()
		{
			if (PTS == null) return;

			var cursor = (PTS.License.Amount > 0)
							 ? Cursors.Hand
							 : Cursors.Default;
			
			frequentExceptionCategoriesIncurredPanel.Cursor =
				frequentExceptionsIncurredByEmployeesPanel.Cursor =
				performanceDeviationAgainstPredefinedThresholdsPanel.Cursor =
				highAlertTop2EmployeesExceptionMonitoringPanel.Cursor =
				employeeProductivityLossRankingPanel.Cursor =
				employeeProductivityImprovementTrendPanel.Cursor = cursor;
		}

		public void Initialize()
		{
		}

		private void B1ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if(OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("B1"));
		}

		private void B2ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if (OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("B2"));
		}

		private void B3ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if (OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("B3"));
		}

		private void C1ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if (OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("C1"));
		}

		private void C2ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if (OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("C2"));
		}

		private void C3ReportPanelClick(Object sender, MouseEventArgs e)
		{
			if (PTS == null || PTS.License.Amount == 0) return;

			if (OnReportSelect != null)
				OnReportSelect(this, new EventArgs<String>("C3"));
		}
	} 
}