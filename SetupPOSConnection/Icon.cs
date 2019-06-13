
using Constant;

namespace SetupPOSConnection
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_POSConnectionIcon", "POS Connection");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Button.Text = TitleName = Localization["Control_POSConnectionIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"POSConnection";
        }
    }
}
