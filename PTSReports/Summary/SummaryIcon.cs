
using System;
using App;
using Constant;
using Interface;

namespace PTSReports.Summary
{
    public class SummaryIcon : SetupBase.Icon, IAppUse
    {
        public IApp App { get; set; }
        public override void Initialize()
        {
            Localization.Add("Control_ReportSummaryIcon", "Summary");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.summaryIcon_activate, Properties.Resources.IMGSummaryActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.summaryIcon, Properties.Resources.IMGSummaryIcon);

            Button.Text = TitleName = Localization["Control_ReportSummaryIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportSummary";

            //App.OnSwitchPage += SearchCriteriaChange;
        }

        //public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
        //{
        //    if (!String.Equals(e.Value1, "Report")) return;

        //    var summaryReportParameter = e.Value2 as SummaryReportParameter;
        //    if (summaryReportParameter == null) return;

        //    if (!InUse)
        //        ActiveSetup();
        //}
    }
}
