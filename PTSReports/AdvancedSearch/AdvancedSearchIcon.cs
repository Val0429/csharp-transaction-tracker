using System;
using App;
using Constant;
using Interface;

namespace PTSReports.AdvancedSearch
{
    public class AdvancedSearchIcon : SetupBase.Icon, IAppUse
    {
        public IApp App { get; set; }
        public override void Initialize()
        {
            Localization.Add("Control_AdvancedSearchIcon", "Advanced Search");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.advancedSearchIcon_activate, Properties.Resources.IMGAdvancedSearchActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.advancedSearchIcon, Properties.Resources.IMGAdvancedSearchIcon);

            Button.Text = TitleName = Localization["Control_AdvancedSearchIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"AdvancedSearch";

            //App.OnSwitchPage += SearchCriteriaChange;
        }

        //public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
        //{
        //    if (!String.Equals(e.Value1, "Report")) return;

        //    var exceptionsReportParameter = e.Value2 as ExceptionReportParameter;
        //    if (exceptionsReportParameter == null) return;

        //    if (!InUse)
        //        ActiveSetup();
        //}
    }
}
