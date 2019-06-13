
using Constant;

namespace PTSReports.Template
{
    public class TemplateIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportTemplateIcon", "Template");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.templateIcon_activate, Properties.Resources.IMGTemplateIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.templateIcon, Properties.Resources.IMGTemplateIcon);

            Button.Text = TitleName = Localization["Control_ReportTemplateIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportTemplate";
        }
    }
}
