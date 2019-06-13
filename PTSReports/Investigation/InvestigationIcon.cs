
using Constant;

namespace PTSReports.Investigation
{
    public class InvestigationIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportInvestigationIcon", "Investigation");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.investigationIcon_activate, Properties.Resources.IMGInvestigationActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.investigationIcon, Properties.Resources.IMGInvestigationIcon);

            Button.Text = TitleName = Localization["Control_ReportInvestigationIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportInvestigation";
        }
    }
}
