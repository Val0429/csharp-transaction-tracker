
using Constant;

namespace PTSReports.Tracking
{
    public class TrackingIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportTrackingIcon", "Report");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.trackingIcon_activate, Properties.Resources.IMGTrackingIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.trackingIcon, Properties.Resources.IMGTrackingIcon);

            Button.Text = TitleName = Localization["Control_ReportTrackingIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportTracking";
        }
    }
}
