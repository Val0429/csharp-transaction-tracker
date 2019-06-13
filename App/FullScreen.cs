using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace App
{
	public partial class AppClient
	{
		private Padding  _previousWorkPanelPadding;
		private Padding  _previousMainPanelPadding;
		private Boolean _isFullScreen;
		public virtual Boolean IsFullScreen
		{
			get
			{
				return _isFullScreen;
			}
			set
			{
				if (_isFullScreen == value) return;
				_isFullScreen = value;

				if (value)
				{
					if (ToolPanel != null)
						ToolPanel.Visible = false;

					StatePanel.Visible = HeaderPanel.Visible = false;
					_previousMainPanelPadding = MainPanel.Padding;
					_previousWorkPanelPadding = WorkPanel.Padding;
					MainPanel.Padding = new Padding(0);
					WorkPanel.Padding = new Padding(0);
				}
				else
				{
					if (ToolPanel != null)
						ToolPanel.Visible = true;

					StatePanel.Visible = HeaderPanel.Visible = true;

					MainPanel.Padding = _previousMainPanelPadding;
					WorkPanel.Padding = _previousWorkPanelPadding;
					if (!IsLock)
					{
						ResetTitleBarText();
					}
				}
			}
		}

		public virtual void ResetTitleBarText()
		{
		}

		private Boolean _isHidePanel;
		public virtual Boolean IsHidePanel
		{
			get
			{
				return _isHidePanel;
			}
			set
			{
				if (_isHidePanel == value) return;
				_isHidePanel = value;
				if (HidePanelStripMenuItem != null)
					HidePanelStripMenuItem.Text = (IsHidePanel ? Localization["Menu_ShowPanel"] : Localization["Menu_HidePanel"]);

				if (value)
				{
					UpdateClientSetting(RestoreClientColumn.HidePanel, "true", null);
					PageActivated.HidePanel();
				}
				else
				{
					UpdateClientSetting(RestoreClientColumn.HidePanel, "false", null);
					PageActivated.ShowPanel();
				}

			}
		}

		public void FullScreen()
		{
			//if (PageActivated.Name != "Live") return;

			UpdateClientSetting(RestoreClientColumn.FullScreen, "true", null);

			IsHidePanel = true;
			IsFullScreen = true;

			((ApplicationForm)Form).PreviousWindowState = Form.WindowState;

			//switch to normal and switch to maximum to ensure window is REALL maximum
			if (Form.WindowState == FormWindowState.Maximized)
				Form.WindowState = FormWindowState.Normal;

			Form.ShowIcon = false;

			Form.FormBorderStyle = FormBorderStyle.None;
			Form.WindowState = FormWindowState.Maximized;

			Form.TopMost = true;

			foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
			{
				foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
				{
					if (controlPanel.Control is IFullScreen)
					{
						((IFullScreen)controlPanel.Control).FullScreen();
					}
				}
			}
		}

		public void ExitFullScreen()
		{
			//if (PageActivated.Name != "Live") return;

			UpdateClientSetting(RestoreClientColumn.FullScreen, "false", null);

			IsHidePanel = false;
			IsFullScreen = false;

			//restore WindowState BEFORE set FormBorderStyle = Sizable, else Form.Icon will appear as another form
			Form.WindowState = ((ApplicationForm)Form).PreviousWindowState;// FormWindowState.Normal;

			if (!IsLock)
			{
				Form.TopMost = false;
				Form.FormBorderStyle = FormBorderStyle.Sizable;
			}

			Form.ShowIcon = true;

            //avoid problem the path live(instant playback) --> playback -->exit full sreen --> live
		    foreach (KeyValuePair<string, IPage> page in Pages)
		    {
                foreach (var blockPanel in page.Value.Layout.BlockPanels)
                {
                    foreach (var controlPanel in blockPanel.ControlPanels)
                    {
                        if (controlPanel.Control is IFullScreen)
                        {
                            ((IFullScreen)controlPanel.Control).ExitFullScreen();
                        }
                    }
                }
		    }
		}
	}
}
