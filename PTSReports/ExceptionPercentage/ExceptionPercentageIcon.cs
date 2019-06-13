
using Constant;

namespace PTSReports.ExceptionPercentage
{
    public class ExceptionPercentageIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportExceptionPercentageIcon", "Exception Percentage");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.exceptionPercentageIcon_activate, Properties.Resources.IMGExceptionPercentageIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.exceptionPercentageIcon, Properties.Resources.IMGExceptionPercentageIcon);

            Button.Text = TitleName = Localization["Control_ReportExceptionPercentageIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportExceptionPercentage";
        }
    }
}
