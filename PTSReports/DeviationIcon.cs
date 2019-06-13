
using Constant;

namespace PTSReports
{
    public class DeviationIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportDeviationIcon", "Deviation");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportDeviationIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.deviationIcon, Properties.Resources.IMGDeviationIcon);
            Button.Name = @"ReportDeviation";

            ActivateIcon = Resources.GetResources(Properties.Resources.deviationIcon_activate, Properties.Resources.IMGDeviationIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.deviationIcon, Properties.Resources.IMGDeviationIcon);
        }
    }
}
