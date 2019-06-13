using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;

namespace Layout.Plugin
{
	public class PluginBlockPanel : BlockPanel
	{
		protected override void LoadConfig(XmlElement blockNode)
		{
			String width = Xml.GetFirstElementsValueByTagName(blockNode, "Width");
			String dock = Xml.GetFirstElementsValueByTagName(blockNode, "Dock");
			String color = Xml.GetFirstElementsValueByTagName(blockNode, "BackgroundColor");

			if (width != "auto")
			{
				Width = Convert.ToInt32(width);
				switch (dock)
				{
					case "left":
						Dock = DockStyle.Left;
						break;

					case "right":
						Dock = DockStyle.Right;
						break;

					default:
						Dock = DockStyle.Fill;
						break;
				}
			}
			else
			{
				IsAutoWidth = true;
				Dock = DockStyle.Fill;
			}

			_isAutoScroll = Convert.ToBoolean(Xml.GetFirstElementsValueByTagName(blockNode, "AutoScroll"));
			_controlPanel.AutoScroll = _isAutoScroll;

			BackColor = ColorTranslator.FromHtml(color);

			var controls = blockNode.GetElementsByTagName("Control");
			//Int32 count = 0;
			foreach (XmlElement node in controls)
			{
				if (LayoutManager.Page.Name == "Setup")
				{
					String controlName = Xml.GetFirstElementsValueByTagName(node, "Name");

					Boolean hasPermission = false;
					switch (controlName)
					{
						case "Title":
						case "IconTitle":
							hasPermission = true;
							break;

						case "Server":
						case "Server Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.Server));
							break;

						case "NVR":
						case "NVR Icon":
							hasPermission = (_page.Server is ICMS || _page.Server is IVAS);
							//hasPermission = (_page.Server is INVR);
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.NVR));
							break;

						case "Device":
						case "Device Icon":
							hasPermission = (_page.Server is INVR && !(_page.Server is ICMS));
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.Device));
							break;

						case "Device Group":
						case "Device Group Icon":
							hasPermission = (_page.Server is INVR); //ICMS is INVR
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.DeviceGroup));
							break;

						case "Register":
						case "Register Icon":
							hasPermission = (_page.Server is ICMS || _page.Server is INVR);
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.Register));
							break;

						case "Exception":
						case "Exception Icon":
							hasPermission = (_page.Server is ICMS || _page.Server is INVR);
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.Exception));
							break;

						case "PeopleCounting":
						case "PeopleCounting Icon":
							hasPermission = (_page.Server is IVAS);
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.PeopleCounting));
							break;

						case "Event":
						case "Event Icon":
							hasPermission = (_page.Server is INVR && !(_page.Server is ICMS));
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.Event));
							break;

						case "Schedule":
						case "Schedule Icon":
							hasPermission = (_page.Server is INVR && !(_page.Server is ICMS));
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.Schedule));
							break;

						case "General":
						case "General Icon":
							hasPermission = (_page.Server is INVR); //ICMS is INVR
							hasPermission = (hasPermission && _page.Server.User.Current.Group.CheckPermission("Setup", Permission.General));
							break;

						case "User":
						case "User Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.User));
							break;

						case "Joystick":
						case "Joystick Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.Joystick));
							break;

						case "License":
						case "License Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.License));
							break;

						case "Plug-in License":
						case "Plug-in License Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.PluginLicense));
							break;

						case "Log":
						case "Log Icon":
							hasPermission = (_page.Server.User.Current.Group.CheckPermission("Setup", Permission.Log));
							break;
					}
					if (!hasPermission) continue;
				}

				ControlPanels.Insert(0, new ControlPanel
				{
					BlockPanel = this,
					ControlNode = node,
				});
			}

			Controls.Add(_controlPanel);

			_controlPanel.Dock = DockStyle.Fill;

			XmlNode controlDockNode = blockNode.SelectSingleNode("ControlDock");

			if (controlDockNode != null)
			{
				_dockPanel = new Panel
				{
					Dock = DockStyle.Bottom,
					Height = Convert.ToInt32(Xml.GetFirstElementsValueByTagName(controlDockNode, "Height")),
					BackColor = ColorTranslator.FromHtml(Xml.GetFirstElementsValueByTagName(controlDockNode, "BackgroundColor")),
				};
				Controls.Add(_dockPanel);
			}

			foreach (IControlPanel control in ControlPanels)
			{
				if (_dockPanel != null && control.Icon != null)
					_dockPanel.Controls.Add(control.Icon);

				control.OnMinimizeChange += BlockPanelSizeChanged;

				_controlPanel.Controls.Add((Control)control);
			}
		}
	}
}
