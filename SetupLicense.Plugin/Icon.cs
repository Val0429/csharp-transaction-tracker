
using Constant;

namespace SetupLicense.Plugin
{
    public class Icon : SetupLicense.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_Plug-inLicenseIcon", "Plug-in License");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Button.Text = TitleName = Localization["Control_Plug-inLicenseIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"Plug-in License";
        }
    }
}
