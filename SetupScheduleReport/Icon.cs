
using Constant;

namespace SetupScheduleReport
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ScheduleReportIcon", "Schedule Report");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ScheduleReportIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            Button.Name = @"Schedule Report";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        }
    }
}
