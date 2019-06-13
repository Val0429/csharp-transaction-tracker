
using Constant;

namespace PTSReports.Deviation
{
    public class DeviationIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportDeviationIcon", "Deviation");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.deviationIcon_activate, Properties.Resources.IMGDeviationIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.deviationIcon, Properties.Resources.IMGDeviationIcon);

            Button.Text = TitleName = Localization["Control_ReportDeviationIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportDeviation";
        }
    }
}
