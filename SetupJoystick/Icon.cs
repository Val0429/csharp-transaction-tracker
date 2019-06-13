
using Constant;

namespace SetupJoystick
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_JoystickIcon", "Joystick");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_JoystickIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            Button.Name = @"Joystick";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        }
    }
}
