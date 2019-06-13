
using Constant;

namespace SetupUser
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_UserIcon", "User");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_UserIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            Button.Name = @"User";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        }
    }
}
