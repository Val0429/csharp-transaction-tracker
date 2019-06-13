using System;
using System.Windows.Forms;
using App;
using Interface;
using PanelBase;

namespace App_FailoverSystem
{
    public partial class FailoverSystem
	{
		protected override void InitializeHeaderPanel()
		{
			base.InitializeHeaderPanelUI2();

			//----------------------------------------
            AppName = Localization["Application_FailoverSystem"];

			//Lock NVR
			LockApp = new ToolStripMenuItemUI2
						  {
							  Text = Localization["Menu_LockApplication"]
						  };
			LockApp.Click += LockAppToolStripMenuItemClick;

			ApplicationMenu.DropDownItems.Clear();
			ApplicationMenu.DropDownItems.Add(LockApp);
			ApplicationMenu.DropDownItems.Add(SignOut);
			ApplicationMenu.DropDownItems.Add(About);
		}

		private Boolean _logPageInfo = false;
		public override void Activate(IPage page)
		{
			base.Activate(page);

            BandwidthMenu.Visible = false;

			if(!_logPageInfo)
			 _logPageInfo = true;

			ToolPanel.Dock = PageActivated.Layout.SidePanelDockStyle;
			ToolPanel.Width = PageActivated.Layout.SidePanelWidth;
			PageFunctionPanel.Height = PageActivated.Layout.FunctionPanelHeight;
			
			UpdateMenuVisible();
		}

		private void UpdateMenuVisible()
		{
			if (_isLock)
			{
				FullscreenMenu.Visible = false;
				BandwidthMenu.Visible = false;
				RefreshMenuStripIconStyle();
				return;

			}
            
			switch (PageActivated.Name)
			{
				case "Live":
					FullscreenMenu.Visible = true;
                    BandwidthMenu.Visible = true;
					break;

				case "Playback":
					FullscreenMenu.Visible = true;
					BandwidthMenu.Visible = _fos.Server.CheckProductNoToSupport("bandwidthControl");//playback DO support
					break;

				case "Investigation":
					FullscreenMenu.Visible = false;
					BandwidthMenu.Visible = _fos.Server.CheckProductNoToSupport("bandwidthControl");//playback DO support
					break;

				default:
					FullscreenMenu.Visible = false;
					BandwidthMenu.Visible = false;
					break;
			}

            if (_fos.Configure.EnableBandwidthControl)
            {
                BandwidthMenu.Visible = false;
            }

			RefreshMenuStripIconStyle();
		}

		protected UnlockAppForm _unlockAppForm;
		protected Boolean _isLock;
		public override Boolean IsLock
		{
			get
			{
				return _isLock;
			}
			protected set
			{
				if (value)
				{
					DialogResult result = TopMostMessageBox.Show(Localization["Application_ConfirmLockApp"],
																	Localization["MessageBox_Confirm"],
																	MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result != DialogResult.Yes) 
						return;

					_isLock = true;

					LockApplication();
					UpdateMenuVisible();
				}
				else
				{
					//ask user's account password to unlock
					_unlockAppForm.TopMost = true;
					_unlockAppForm.ShowDialog();
				}
			}
		}

		protected virtual void UnlockAppFormOnConfirm(Object sender, EventArgs e)
		{
			UnlockApplication();

			_isLock = false;

			ResetTitleBarText();
			UpdateMenuVisible();
		}

		private void LockAppToolStripMenuItemClick(Object sender, EventArgs e)
		{
			IsLock = !_isLock;
		}
	}
}
