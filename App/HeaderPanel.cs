using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace App
{
	public partial class AppClient
	{
		protected virtual void InitializeHeaderPanel()
		{
			HeaderPanel = ApplicationForms.MenuPanel();

			var logo = ApplicationForms.Logo();

			var version = ApplicationForms.VersionLabel();
			version.Text = Localization["Application_Ver"].Replace("%1", Version);
			
			//--------------------------------------------------------------------------
			ApplicationMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Left,
				Text = Localization["Menu_Application"],
			};

			HidePanelStripMenuItem = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Left,
				Text = Localization["Menu_HidePanel"],
			};
			HidePanelStripMenuItem.Click += HidePanelStripMenuItemClick;
			
			//--------------------------------------------------------------------------

			MenuStrip = new MenuStrip
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(0, 0, 0, 0),
				BackColor = Color.Transparent,
			};

			MenuStrip.Items.AddRange(new [] {
				ApplicationMenu,
				HidePanelStripMenuItem,
			});
			
			//--------------------------------------------------------------------------

			SignOut = new ToolStripMenuItemUI2
			{
				Text = Localization["Menu_SignOut"],
				ShortcutKeys = Keys.Control | Keys.W,
			};
			SignOut.Click += SignOutToolStripMenuItemClick;

			ApplicationMenu.DropDownItems.Add(SignOut);

			HeaderPanel.Controls.Add(MenuStrip);
			HeaderPanel.Controls.Add(logo);
			HeaderPanel.Controls.Add(version);
			Form.Controls.Add(HeaderPanel);
		}

		protected virtual void InitializeHeaderPanelUI2()
		{
			HeaderPanel = ApplicationForms.MenuPanelUI2();

			LogoPictureBox = ApplicationForms.Logo();

			//var version = ApplicationForms.VersionLabel();
			//version.Text = Localization["Application_Ver"].Replace("%1", Version);
			
			//--------------------------------------------------------------------------
			ApplicationMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Right,
				Image = Properties.Resources.applicationMenuIcon,
				BackgroundImageLayout = ImageLayout.Center,
				Size = new Size(45, 39),
				ImageAlign = ContentAlignment.MiddleCenter,
				ToolTipText = Localization["Menu_Application"],
				AutoToolTip = false,
				AutoSize = false,
				DropDown = { BackColor = Color.FromArgb(39, 41, 44) },
			};
			 
			BandwidthMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Right,
				Image = Properties.Resources.bandwidthMenuIcon,
				BackgroundImageLayout = ImageLayout.Center,
				Size = new Size(45, 39),
				ImageAlign = ContentAlignment.MiddleCenter,
				ToolTipText = Localization["Menu_Bandwidth"],
				AutoToolTip = false,
				AutoSize = false,
				DropDown = { BackColor = Color.FromArgb(39, 41, 44) },
			};

			FullscreenMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Right,
				Image = Properties.Resources.fullscreenMenuIcon,
				BackgroundImageLayout = ImageLayout.Center,
				Size = new Size(45, 39),
				ImageAlign = ContentAlignment.MiddleCenter,
				ToolTipText = Localization["Menu_Fullscreen"],
				AutoToolTip = false,
				AutoSize = false,
				DropDown = { BackColor = Color.FromArgb(39, 41, 44) },
			};
			FullscreenMenu.Click += FullscreenMenuToolStripMenuItemClick;

			SetupMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Right,
				Image = Properties.Resources.setupMenuIcon,
				BackgroundImageLayout = ImageLayout.Center,
				Size = new Size(45, 39),
				ImageAlign = ContentAlignment.MiddleCenter,
				ToolTipText = Localization["Menu_Setup"],
				AutoToolTip = false,
				AutoSize = false,
				DropDown = { BackColor = Color.FromArgb(39, 41, 44) },
			};
			SetupMenu.Click += SetupMenuToolStripMenuItemClick;

			//--------------------------------------------------------------------------

			MenuStrip = new MenuStrip
			{
				Dock = DockStyle.None,
				BackColor = Color.Transparent,
				Anchor = AnchorStyles.Top | AnchorStyles.Right,
				ShowItemToolTips = true,
				AutoSize = true,
			};
			
			MenuStrip.Items.AddRange(new[] {
				SetupMenu,
				FullscreenMenu,
				BandwidthMenu,
				ApplicationMenu,
			});
			RefreshMenuStripIconStyle();
			//MenuStrip.MinimumSize = MenuStrip.MaximumSize = new Size(MenuStrip.Items.Count * ApplicationMenu.Width, ApplicationMenu.Height);
			//MenuStrip.Size = new Size(MenuStrip.Items.Count * (ApplicationMenu.Width + 1)+2, ApplicationMenu.Height);
			MenuStrip.Location = new Point(HeaderPanel.Width - MenuStrip.Width - 10, 9); //4 icon(45 * 4 = 180) + space(10)

			//--------------------------------------------------------------------------

			SignOut = new ToolStripMenuItemUI2
						  {
							  Text = Localization["Menu_SignOut"],
							  ShortcutKeys = Keys.Control | Keys.W
						  };
			SignOut.Click += SignOutToolStripMenuItemClick;

			ApplicationMenu.DropDownItems.Add(SignOut);

			About = new ToolStripMenuItemUI2
						{
							Text = Localization["Menu_About"]
						};
			About.Click += AboutToolStripMenuItemClick;

			ApplicationMenu.DropDownItems.Add(About);

			//--------------------------------------------------------------------------

			SwitchPagePanel = ApplicationForms.SwitchPagePanelUI2();

			//------------------------------------------------------------------
			HeaderPanel.Controls.Add(MenuStrip);
			HeaderPanel.Controls.Add(LogoPictureBox);
			HeaderPanel.Controls.Add(SwitchPagePanel);

			//MenuPanel.Controls.Add(version);
			Form.Controls.Add(HeaderPanel);

			HeaderPanel.SizeChanged += HeaderPanelSizeChanged;
		}

		protected virtual void FullscreenMenuToolStripMenuItemClick(Object sender, EventArgs e)
		{
			FullScreen();
		}

		protected virtual void RefreshMenuStripIconStyle()
		{
			var list = new List<ToolStripMenuItem>();

			foreach (ToolStripMenuItem toolStripMenuItem in MenuStrip.Items)
			{
				if (!toolStripMenuItem.Visible) continue;
				list.Add(toolStripMenuItem);
			}

			if(list.Count == 0) return;

			var firstIcon = list.First();
			list.Remove(firstIcon);
			
			//it could be setup
			firstIcon.BackgroundImage = _menuIconBg3;
			if (PageActivated != null && PageActivated.Name == "Setup")
				firstIcon.BackgroundImage = _menuIconBg3On;

			if(list.Count == 0) return;

			var lastIcon = list.Last();
			list.Remove(lastIcon);

			lastIcon.BackgroundImage = _menuIconBg;

			foreach (var toolStripMenuItem in list)
			{
				toolStripMenuItem.BackgroundImage = _menuIconBg2;
			}
		}

		private void HeaderPanelSizeChanged(Object sender, EventArgs e)
		{
			if (!SwitchPagePanel.Visible || SwitchPagePanel.Controls.Count == 0) return;

			var width = SwitchPagePanel.Controls.Count * 170;
			var leftWidth = (HeaderPanel.Width - LogoPictureBox.Width - MenuStrip.Width - width) / 2;
			SwitchPagePanel.Location = new Point(LogoPictureBox.Location.X + LogoPictureBox.Width + leftWidth, 10);
		}

		protected String AppName = "App";

		protected virtual void HidePanelStripMenuItemClick(Object sender, EventArgs e)
		{
			IsHidePanel = !IsHidePanel;

			UpdateClientSetting(RestoreClientColumn.HidePanel, IsHidePanel ? "true" : "false", null);
		}
	}
}
