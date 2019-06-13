
using Constant;

namespace SetupGenericPOSSetting
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_GenericPOSSetting", "Generic POS Setting");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Button.Text = TitleName = Localization["Control_GenericPOSSetting"];
            Button.Image = InactivateIcon;
            Button.Name = @"GenericPOSSetting";
        }
    }
}
