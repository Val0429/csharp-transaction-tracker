using System;
using App;
using Constant;
using Interface;

namespace PTSReports.Exception
{
    public class ExceptionIcon : SetupBase.Icon, IAppUse
    {
        public IApp App { get; set; }
        public override void Initialize()
        {
            Localization.Add("Control_ReportExceptionIcon", "Exception Search");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.exceptionIcon_activate, Properties.Resources.IMGExceptionActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.exceptionIcon, Properties.Resources.IMGExceptionIcon);

            Button.Text = TitleName = Localization["Control_ReportExceptionIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportException";

            App.OnSwitchPage += SearchCriteriaChange;
        }

        public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
        {
            if (!String.Equals(e.Value1, "Report")) return;

            var exceptionsReportParameter = e.Value2 as ExceptionReportParameter;
            if (exceptionsReportParameter == null) return;

            if (!InUse)
                ActiveSetup();
        }
    }
}
