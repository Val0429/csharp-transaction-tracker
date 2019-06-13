using System;
using System.Drawing;
using System.Xml;
using Constant;

namespace Layout.Plugin
{
	public class Page : Layout.Page
	{
		private readonly Image _sdPlayback;
		private readonly Image _sdPlaybackactivate;

		//public override void LoadConfig()
		//{
		//    base.LoadConfig();

		//    switch (Name)
		//    {
		//        case "SD Playback":
		//            Icon.Image = _sdPlayback;
		//            break;
		//    }
		//}

		protected override Layout.LayoutManager LoadLayoutManager(XmlDocument configNode)
		{
			return new LayoutManager
						{
							Page = this,
							ConfigNode = configNode,
						};
		}

		protected override void AppButtonOnClick(Object sender, EventArgs e)
		{
			Server.WriteOperationLog(Localization["Page_SwitchPage"].Replace("%1", TitleName));

			base.AppButtonOnClick(sender, e);
		}

		//public override void Activate()
		//{
		//    switch (Name)
		//    {
		//        case "SD Playback":
		//            Icon.Image = _sdPlaybackactivate;
		//            break;
		//    }

		//    base.Activate();
		//}

		//public override void Deactivate()
		//{
		//    switch (Name)
		//    {
		//        case "SD Playback":
		//            Icon.Image = _sdPlayback;
		//            break;
		//    }

		//    base.Deactivate();
		//}
	}
}
