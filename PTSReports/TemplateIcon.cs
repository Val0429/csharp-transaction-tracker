
using Constant;

namespace PTSReports
{
    public class TemplateIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportTemplateIcon", "Template");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ReportTemplateIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.templateIcon, Properties.Resources.IMGTemplateIcon);
            Button.Name = @"ReportTemplate";

            ActivateIcon = Resources.GetResources(Properties.Resources.templateIcon_activate, Properties.Resources.IMGTemplateIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.templateIcon, Properties.Resources.IMGTemplateIcon);
        }
    }
}
