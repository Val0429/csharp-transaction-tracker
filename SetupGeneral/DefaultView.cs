using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupGeneral
{
    public sealed partial class DefaultView : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        //private UInt16 _minCount = 1;
        //private UInt16 _maxCount = 64;

		public DefaultView()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_DefaultView", "Default View"},
								   {"Control_General", "General"},
								   {"SetupGeneral_LatestView", "Latest View"},
								   {"SetupGeneral_BandWidth", "Band Width"},
								   {"SetupGeneral_FullScreen", "Full Screen"},
								   {"SetupGeneral_HidePanel", "Hide Panel"},
								   {"SetupGeneral_HideToolbar", "Hide Toolbar"},
								   {"Common_Hide", "Hide"},
								   {"Page_Live", "Live"},
                                   {"SetupGeneral_Enabled", "Enabled"},
                                   {"SetupGeneral_ViewTour", "View Tour"},
                                   {"Control_DeviceGroup", "Group"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
			Name = "DefaultView";
            Dock = DockStyle.None;
            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);

			
        }

        public void Initialize()
        {
			
        }

		void GroupPanelMouseClick(object sender, MouseEventArgs e)
		{
			Server.Configure.DefaultView = (IDeviceGroup)((Control)sender).Tag;

			((Control)sender).Focus();
			Invalidate();
		}

        public void ParseSetting()
        {
			containerPanel.Controls.Clear();

			foreach (var deviceGroup in Server.Device.Groups)
			{
				var groupPanel = new GroupPanel
				{
					Server = Server,
					Tag = deviceGroup.Value
				};
				groupPanel.MouseClick += GroupPanelMouseClick;
				containerPanel.Controls.Add(groupPanel);
				containerPanel.Controls.SetChildIndex(groupPanel, 0);
			}
        }
    }

	public class GroupPanel : Panel
	{
		public IServer Server;
		public Dictionary<String, String> Localization;

		public GroupPanel()
		{
			DoubleBuffered = true;
			Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
			BackColor = Color.Transparent;
			Dock = DockStyle.Top;
			Height = 40;
			Cursor = Cursors.Hand;

			Paint += GroupPanelPaint;
		}

		private void GroupPanelPaint(object sender, PaintEventArgs e)
		{
			if (Parent == null) return;
			if (Tag == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);
			if (Width <= 100) return;

			if (Tag.ToString() == Server.Configure.DefaultView.ToString())
			{
				Manager.PaintText(g, Tag.ToString(), Brushes.RoyalBlue);
				Manager.PaintSelected(g);
			}
			else
				Manager.PaintText(g, Tag.ToString());
		}
	}
}
