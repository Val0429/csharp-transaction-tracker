
using Constant;

namespace PTSReports
{
    public class InvestigationIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportInvestigationIcon", "Investigation");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportInvestigationIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.investigationIcon, Properties.Resources.IMGInvestigationIcon);
            Button.Name = @"ReportInvestigation";

            ActivateIcon = Resources.GetResources(Properties.Resources.investigationIcon_activate, Properties.Resources.IMGInvestigationActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.investigationIcon, Properties.Resources.IMGInvestigationIcon);
        }
    }
}
