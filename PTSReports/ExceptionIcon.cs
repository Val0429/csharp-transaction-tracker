
using System;
using Constant;
using Interface;

namespace PTSReports
{
    public class ExceptionIcon : SetupBase.Icon, IAppUse
    {
        public IApp App { get; set; }
        public override void Initialize()
        {
            Localization.Add("Control_ReportExceptionIcon", "Exception");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportExceptionIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.exceptionIcon, Properties.Resources.IMGExceptionIcon);
            Button.Name = @"ReportException";

            ActivateIcon = Resources.GetResources(Properties.Resources.exceptionIcon_activate, Properties.Resources.IMGExceptionActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.exceptionIcon, Properties.Resources.IMGExceptionIcon);

            App.OnSwitchPage += SearchCriteriaChange;
        }

        public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
        {
            if (!InUse)
                ActiveSetup();
        }
    }
}
