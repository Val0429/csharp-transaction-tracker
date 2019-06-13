using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace Pager
{
	sealed class LayoutButton : UserControl
	{
		public event MouseEventHandler OnLayoutMouseDown;
		public Dictionary<String, String> Localization;
		public List<WindowLayout> WindowLayout;

		private static Image _layout_1 = Resources.GetResources(Properties.Resources.layout_1, Properties.Resources.IMGLayout1);
		private static Image _layout_1_use = Resources.GetResources(Properties.Resources.layout_1_use, Properties.Resources.IMGLayout1Use);
		private static Image _layout_2 = Resources.GetResources(Properties.Resources.layout_2, Properties.Resources.IMGLayout2);
		private static Image _layout_2_use = Resources.GetResources(Properties.Resources.layout_2_use, Properties.Resources.IMGLayout2Use);
		private static Image _layout_2a = Resources.GetResources(Properties.Resources.layout_2a, Properties.Resources.IMGLayout2a);
		private static Image _layout_2a_use = Resources.GetResources(Properties.Resources.layout_2a_use, Properties.Resources.IMGLayout2aUse);
		private static Image _layout_3 = Resources.GetResources(Properties.Resources.layout_3, Properties.Resources.IMGLayout3);
		private static Image _layout_3_use = Resources.GetResources(Properties.Resources.layout_3_use, Properties.Resources.IMGLayout3Use);
		private static Image _layout_3a = Resources.GetResources(Properties.Resources.layout_3a, Properties.Resources.IMGLayout3a);
		private static Image _layout_3a_use = Resources.GetResources(Properties.Resources.layout_3a_use, Properties.Resources.IMGLayout3aUse);
		private static Image _layout_4 = Resources.GetResources(Properties.Resources.layout_4, Properties.Resources.IMGLayout4);
		private static Image _layout_4_use = Resources.GetResources(Properties.Resources.layout_4_use, Properties.Resources.IMGLayout4Use);
		private static Image _layout_5 = Resources.GetResources(Properties.Resources.layout_5, Properties.Resources.IMGLayout5);
		private static Image _layout_5_use = Resources.GetResources(Properties.Resources.layout_5_use, Properties.Resources.IMGLayout5Use);
		private static Image _layout_6 = Resources.GetResources(Properties.Resources.layout_6, Properties.Resources.IMGLayout6);
		private static Image _layout_6_use = Resources.GetResources(Properties.Resources.layout_6_use, Properties.Resources.IMGLayout6Use);
		private static Image _layout_6a = Resources.GetResources(Properties.Resources.layout_6a, Properties.Resources.IMGLayout6a);
		private static Image _layout_6a_use = Resources.GetResources(Properties.Resources.layout_6a_use, Properties.Resources.IMGLayout6aUse);
		private static Image _layout_8 = Resources.GetResources(Properties.Resources.layout_8, Properties.Resources.IMGLayout8);
		private static Image _layout_8_use = Resources.GetResources(Properties.Resources.layout_8_use, Properties.Resources.IMGLayout8Use);
		private static Image _layout_8a = Resources.GetResources(Properties.Resources.layout_8a, Properties.Resources.IMGLayout8a);
		private static Image _layout_8a_use = Resources.GetResources(Properties.Resources.layout_8a_use, Properties.Resources.IMGLayout8aUse);
		private static Image _layout_9 = Resources.GetResources(Properties.Resources.layout_9, Properties.Resources.IMGLayout9);
		private static Image _layout_9_use = Resources.GetResources(Properties.Resources.layout_9_use, Properties.Resources.IMGLayout9Use);
		private static Image _layout_9a = Resources.GetResources(Properties.Resources.layout_9a, Properties.Resources.IMGLayout9a);
		private static Image _layout_9a_use = Resources.GetResources(Properties.Resources.layout_9a_use, Properties.Resources.IMGLayout9aUse);
		private static Image _layout_9b = Resources.GetResources(Properties.Resources.layout_9b, Properties.Resources.IMGLayout9b);
		private static Image _layout_9b_use = Resources.GetResources(Properties.Resources.layout_9b_use, Properties.Resources.IMGLayout9bUse);
		private static Image _layout_10 = Resources.GetResources(Properties.Resources.layout_10, Properties.Resources.IMGLayout10);
		private static Image _layout_10_use = Resources.GetResources(Properties.Resources.layout_10_use, Properties.Resources.IMGLayout10Use);
		private static Image _layout_16 = Resources.GetResources(Properties.Resources.layout_16, Properties.Resources.IMGLayout16);
		private static Image _layout_16_use = Resources.GetResources(Properties.Resources.layout_16_use, Properties.Resources.IMGLayout16Use);
		private static Image _layout_25 = Resources.GetResources(Properties.Resources.layout_25, Properties.Resources.IMGLayout25);
		private static Image _layout_25_use = Resources.GetResources(Properties.Resources.layout_25_use, Properties.Resources.IMGLayout25Use);
		private static Image _layout_36 = Resources.GetResources(Properties.Resources.layout_36, Properties.Resources.IMGLayout36);
		private static Image _layout_36_use = Resources.GetResources(Properties.Resources.layout_36_use, Properties.Resources.IMGLayout36Use);
		private static Image _layout_49 = Resources.GetResources(Properties.Resources.layout_49, Properties.Resources.IMGLayout49);
		private static Image _layout_49_use = Resources.GetResources(Properties.Resources.layout_49_use, Properties.Resources.IMGLayout49Use);
		private static Image _layout_64 = Resources.GetResources(Properties.Resources.layout_64, Properties.Resources.IMGLayout64);
		private static Image _layout_64_use = Resources.GetResources(Properties.Resources.layout_64_use, Properties.Resources.IMGLayout64Use);

		private Image _normalIcon;
		private Image _useIcon;

		public String LayoutFormat
		{
			set
			{
				WindowLayout = WindowLayouts.LayoutGenerate(value);
				switch (value)
				{
					case "*":
						if(Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 1.ToString()));
						_normalIcon = _layout_1;
						_useIcon = _layout_1_use;
						break;

					case "**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 2.ToString()));
						_normalIcon = _layout_2;
						_useIcon = _layout_2_use;
						break;

					case "*,*":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 2.ToString()));
						_normalIcon = _layout_2a;
						_useIcon = _layout_2a_use;
						break;

					case "11,**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 3.ToString()));
						_normalIcon = _layout_3;
						_useIcon = _layout_3_use;
						break;

					case "1*,1*":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 3.ToString()));
						_normalIcon = _layout_3a;
						_useIcon = _layout_3a_use;
						break;

					case "**,**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 4.ToString()));
						_normalIcon = _layout_4;
						_useIcon = _layout_4_use;
						break;

					case "11**,11**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 5.ToString()));
						 _normalIcon = _layout_5;
						_useIcon = _layout_5_use;
						break;
					

					case "1*,1*,2*,2*":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 6.ToString()));
						_normalIcon = _layout_6;
						_useIcon = _layout_6_use;
						break;

					case "11*,11*,***":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 6.ToString()));
						_normalIcon = _layout_6a;
						_useIcon = _layout_6a_use;
						break;

					case "111*,111*,111*,****":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 8.ToString()));
						_normalIcon = _layout_8;
						_useIcon = _layout_8_use;
						break;

					case "****,****":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 8.ToString()));
						_normalIcon = _layout_8a;
						_useIcon = _layout_8a_use;
						break;

					case "***,***,***":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 9.ToString()));
						_normalIcon = _layout_9;
						_useIcon = _layout_9_use;
						break;

					case "11**,11**,11**,11**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 9.ToString()));
						_normalIcon = _layout_9a;
						_useIcon = _layout_9a_use;
						break;

					case "1111,1111,****,****":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 9.ToString()));
						_normalIcon = _layout_9b;
						_useIcon = _layout_9b_use;
						break;

					case "11**,11**,22**,22**":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 10.ToString()));
						_normalIcon = _layout_10;
						_useIcon = _layout_10_use;
						break;

					case "****,****,****,****":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 16.ToString()));
						_normalIcon = _layout_16;
						_useIcon = _layout_16_use;
						break;

					case "*****,*****,*****,*****,*****":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 25.ToString()));
						_normalIcon = _layout_25;
						_useIcon = _layout_25_use;
						break;

					case "******,******,******,******,******,******":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 36.ToString()));
						_normalIcon = _layout_36;
						_useIcon = _layout_36_use;
						break;

					case "*******,*******,*******,*******,*******,*******,*******":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 49.ToString()));
						_normalIcon = _layout_49;
						_useIcon = _layout_49_use;
						break;

					case "********,********,********,********,********,********,********,********":
						if (Localization != null)
							SharedToolTips.SharedToolTip.SetToolTip(this, Localization["Pager_Layout"].Replace("%1", 64.ToString()));
						_normalIcon = _layout_64;
						_useIcon = _layout_64_use;
						break;
				}

				BackgroundImage = _normalIcon;
			}
		}

		private Boolean _active;
		public Boolean Active
		{
			get { return _active; }
			set
			{
				_active = value;
				BackgroundImage = (value) ? _useIcon : _normalIcon;
			}
		}
		public LayoutButton()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Pager_Layout", "Layout %1"},
							   };
			Localizations.Update(Localization);

			Margin = new Padding(0);
			Size = new Size(25, 25);
			DoubleBuffered = true;
			BackgroundImageLayout = ImageLayout.Stretch;
			Cursor = Cursors.Hand;

			BackColor = Color.Transparent;

			MouseClick += LayoutControlMouseClick;
		}

		private void LayoutControlMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnLayoutMouseDown != null)
				OnLayoutMouseDown(this, e);
		}

		public void LargeSize()
		{
			Margin = new Padding(12, 0, 12, 0);
			//Size = new Size(50, 50);
			//BackgroundImageLayout = ImageLayout.Center;
		}
	}
}
