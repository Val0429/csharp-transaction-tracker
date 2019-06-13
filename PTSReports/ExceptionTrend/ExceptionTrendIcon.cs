
using Constant;

namespace PTSReports.ExceptionTrend
{
    public class ExceptionTrendIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportExceptionTrendIcon", "Exception Trend");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.trendIcon_activate, Properties.Resources.IMGTrendIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.trendIcon, Properties.Resources.IMGTrendIcon);

            Button.Text = TitleName = Localization["Control_ReportExceptionTrendIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportExceptionTrend";
        }
    }
}
