
using Constant;

namespace SetupDeviceGroup
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_DeviceGroupIcon", "Group");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_DeviceGroupIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            Button.Name = @"Group";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        }
    }
}
