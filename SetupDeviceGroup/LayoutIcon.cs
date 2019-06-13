
using Constant;

namespace SetupDeviceGroup
{
    public class LayoutIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ImageStitchingIcon", "Image Stitching");
            base.Initialize();

            Button.Text = TitleName = Localization["Control_ImageStitchingIcon"];
            Button.Image = Resources.GetResources(Properties.Resources.icon_layout, Properties.Resources.IMGIconLayout);
            Button.Name = @"Device Layout";

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_layout_activate, Properties.Resources.IMGIconLayoutActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon_layout, Properties.Resources.IMGIconLayout);
        }
    }
}
