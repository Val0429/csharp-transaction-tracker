
using Constant;

namespace PTSReports.Calendar
{
    public class CalendarIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportCalendarIcon", "Calendar");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.calendarIcon_activate, Properties.Resources.IMGCalendarIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.calendarIcon, Properties.Resources.IMGCalendarIcon);

            Button.Text = TitleName = Localization["Control_ReportCalendarIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportCalendar";
        }
    }
}
