
using Constant;
using Interface;

namespace SetupLog
{
	public class Icon : SetupBase.Icon, IAppUse
	{
		public IApp App { get; set; }

		public override void Initialize()
		{
			Localization.Add("Control_LogIcon", "Log");
			base.Initialize();

			Button.Text = TitleName = Localization["Control_LogIcon"];
			Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
			Button.Name = @"Log";

			ActivateIcon = Resources.GetResources(Properties.Resources.icon_activate,
												  Properties.Resources.IMGIconActivate);
			InactivateIcon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
		}
	}
}
