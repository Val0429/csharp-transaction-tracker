using System;
using System.Drawing;
using System.Windows.Forms;
using PanelBase;

namespace App
{
    public partial class AppClient
    {
        protected virtual void InitializeMenuPanel()
        {
            MenuPanel = ApplicationForms.MenuPanel();

            var logo = ApplicationForms.Logo();

            var version = ApplicationForms.VersionLabel();
            version.Text = Localization["Application_Ver"].Replace("%1", Version);
            
            //--------------------------------------------------------------------------
            ApplicationMenu = new ToolStripMenuItem
            {
                Alignment = ToolStripItemAlignment.Left,
                Text = Localization["Menu_Application"],
            };

            HideToolStripMenuItem = new ToolStripMenuItem
            {
                Alignment = ToolStripItemAlignment.Left,
                Text = Localization["Menu_HideToolbar"],
                //ShortcutKeys = Keys.Escape
            };

            HideToolStripMenuItem.Click += HideToolStripMenuItemClick;

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
                HideToolStripMenuItem,
                HidePanelStripMenuItem,
            });
            
            //--------------------------------------------------------------------------

            SignOut = new ToolStripMenuItem
            {
                Text = Localization["Menu_SignOut"],
                ShortcutKeys = Keys.Control | Keys.W,
            };
            SignOut.Click += LogoutToolStripMenuItemClick;

            ApplicationMenu.DropDownItems.Add(SignOut);

            MenuPanel.Controls.Add(MenuStrip);
            MenuPanel.Controls.Add(logo);
            MenuPanel.Controls.Add(version);
            Form.Controls.Add(MenuPanel);
        }

        protected String AppName = "App";
        protected virtual void HideToolStripMenuItemClick(Object sender, EventArgs e)
        {
            IsFullScreen = !IsFullScreen;

			UpdateClientSetting("HideToolbar", IsFullScreen ? "true" : "false", null);
        }

        protected virtual void HidePanelStripMenuItemClick(Object sender, EventArgs e)
        {
            IsHidePanel = !IsHidePanel;

			UpdateClientSetting("HidePanel", IsHidePanel ? "true" : "false", null);
        }
    }
}
