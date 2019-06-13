
using Constant;
using Interface;

namespace SetupServer
{
	public class Icon : SetupBase.Icon, IAppUse
	{
		public IApp App { get; set; }

		public override void Initialize()
		{
			Localization.Add("Control_ServerIcon", "Server");
			base.Initialize();

			Button.Text = TitleName = Localization["Control_ServerIcon"];
			Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
			Button.Name = @"Server";

			ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
			InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
		}
	}
}
