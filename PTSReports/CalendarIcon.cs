
using Constant;

namespace PTSReports
{
    public class CalendarIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportCalendarIcon", "Calendar");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportCalendarIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.calendarIcon, Properties.Resources.IMGCalendarIcon);
            Button.Name = @"ReportCalendar";

            ActivateIcon = Resources.GetResources(Properties.Resources.calendarIcon_activate, Properties.Resources.IMGCalendarIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.calendarIcon, Properties.Resources.IMGCalendarIcon);
        }
    }
}
