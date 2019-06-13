
using Constant;

namespace SetupException
{
    public class Icon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ExceptionIcon", "Exception");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Button.Text = TitleName = Localization["Control_ExceptionIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"Exception";
        }
    }
}
