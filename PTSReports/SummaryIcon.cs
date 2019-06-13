
using Constant;

namespace PTSReports
{
    public class SummaryIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportSummaryIcon", "Summary");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportSummaryIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.summaryIcon, Properties.Resources.IMGSummaryIcon);
            Button.Name = @"ReportSummary";

            ActivateIcon = Resources.GetResources(Properties.Resources.summaryIcon_activate, Properties.Resources.IMGSummaryActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.summaryIcon, Properties.Resources.IMGSummaryIcon);
        }
    }
}
