using System;
using Constant;
using Interface;

namespace Layout.Plugin
{
	public class BlockPanel : Layout.BlockPanel
	{
		protected override bool GetHasPermission(string controlName)
		{
			Boolean hasPermission = false;

			hasPermission = base.GetHasPermission(controlName);

			if (!hasPermission)
			{
				switch (controlName)
				{
					case "Plug-in License":
					case "Plug-in License Icon":
						hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.PluginLicense));
						break;

					case "IO Model":
					case "IO Model Icon":
						//hasPermission = (Page.Server is ICMS);
						hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.IOModel));
						break;

                    //case "IO Handle":
                    //case "IO Handle Icon":
                    //    hasPermission = (Page.Server is ICMS);
                    //    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.IOHandle));
                    //    break;
				}
			}

			return hasPermission;
		}
	}
}
