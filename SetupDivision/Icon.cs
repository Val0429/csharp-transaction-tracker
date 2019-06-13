
using Constant;

namespace SetupDivision
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_RegionIcon", "Region");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Button.Text = TitleName = Localization["Control_RegionIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"Region";
        }
    }
}
