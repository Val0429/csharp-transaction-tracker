
using Constant;

namespace SetupExceptionReport
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ExceptionReportIcon", "Exception Report");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ExceptionReportIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            Button.Name = @"Exception Report";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        }
    }
}
