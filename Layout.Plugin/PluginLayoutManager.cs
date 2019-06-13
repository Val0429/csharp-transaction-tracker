using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;

namespace Layout.Plugin
{
	public class PluginLayoutManager : LayoutManager
	{
		public override XmlDocument ConfigNode
		{
			set { LoadConfigNode(value); }
		}

		protected override void LoadConfigNode(XmlDocument configNode)
		{
			var blocks = configNode.GetElementsByTagName("Block");
			if (blocks.Count > 0)
			{
				foreach (XmlElement node in blocks)
				{
					PluginBlockPanel blockPanel = new PluginBlockPanel
					{
						LayoutManager = this,
						BlockNode = node,
					};

					if (blockPanel.Dock != DockStyle.Fill)
					{
						BlockPanels.Add(blockPanel);
					}
					else
						BlockPanels.Insert(0, blockPanel);
				}
			}

			var functions = configNode.GetElementsByTagName("Function");
			if (functions.Count > 0)
			{
				foreach (XmlElement node in functions)
				{
					String name = Xml.GetFirstElementsValueByTagName(node, "Name");
					String asmName = Xml.GetFirstElementsValueByTagName(node, "Assembly");
					String className = Xml.GetFirstElementsValueByTagName(node, "ClassName");

					if (name == "Broadcast" && _page.Server is ICMS)
						continue;

					var asm = Assembly.LoadFrom(asmName);
					IControl control = asm.CreateInstance(className) as IControl;
					if (control != null)
					{
						if (control is IAppUse)
							((IAppUse)control).App = _page.App;

						if (control is IServerUse)
							((IServerUse)control).Server = _page.Server;

						control.Initialize();
						control.Name = name;

						//String key = "Control_" + name.Replace(" ", "");
						//control.TitleName = (Localization.ContainsKey(key))
						//    ? Localization[key] : name;

						if (control is IDrag)
							DragDropProxy.Add(((IDrag)control).DragDropProxy);

						Function.Add(control);
					}
				}
			}

			var menus = configNode.GetElementsByTagName("Menu");
			if (menus.Count > 0)
			{
				foreach (XmlElement node in menus)
				{
					if (node.InnerText == "Setup Map" && !_page.Server.User.Current.Group.CheckPermission("Setup", Permission.Access))
						continue;

					StripMenu menu = new StripMenu
					{
						Name = node.InnerText,
					};

					String key = "Menu_" + menu.Name.Replace(" ", "");
					menu.Text = (Localization.ContainsKey(key))
						? Localization[key] : menu.Name;

					Menus.Add(menu);
				}
			}
		}
	}
}
