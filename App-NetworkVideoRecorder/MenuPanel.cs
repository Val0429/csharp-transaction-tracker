using System;
using System.Drawing;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using Layout;
using PanelBase;

namespace App_NetworkVideoRecorder
{
	public partial class NetworkVideoRecorder
	{
		protected ToolStripMenuItem BandwidthMenu;
		private StripMenu _originalMenu;
		private StripMenu _bandwidth1M;
		private StripMenu _bandwidth512K;
		private StripMenu _bandwidth256K;
		private StripMenu _bandwidth56K;

		protected override void InitializeMenuPanel()
		{
			base.InitializeMenuPanel();

			BandwidthMenu = new ToolStripMenuItem
			{
				Alignment = ToolStripItemAlignment.Left,
				Text = Localization["Menu_Bandwidth"],
			};
			//----------------------------------------
			_originalMenu = new StripMenu
			{
				Text = Localization["Menu_OriginalStreaming"],
				IsSelected = true,
			};
			_originalMenu.Click += SetVideoStreamToOriginal;

			_bandwidth1M = new StripMenu {Text = Localization["Menu_1MVGA"]};
			_bandwidth1M.Click += SetVideoStreamTo1M;

			_bandwidth512K = new StripMenu {Text = Localization["Menu_512KVGA"]};
			_bandwidth512K.Click += SetVideoStreamTo512K;

			_bandwidth256K = new StripMenu {Text = Localization["Menu_256KCIF"]};
			_bandwidth256K.Click += SetVideoStreamTo256K;

			_bandwidth56K = new StripMenu {Text = Localization["Menu_56KQCIF"]};
			_bandwidth56K.Click += SetVideoStreamTo56K;

			BandwidthMenu.DropDownItems.AddRange(new[] {
				_originalMenu,
				_bandwidth1M,
				_bandwidth512K,
				_bandwidth256K,
				_bandwidth56K
			});
			//--------add BandwidthMenu after application
			MenuStrip.Items.Clear();
			MenuStrip.Items.AddRange(new[] {
				ApplicationMenu,
				BandwidthMenu,
				HideToolStripMenuItem,
				HidePanelStripMenuItem,
			});
			//----------------------------------------
			BandwidthMenu.Visible = false;

			AppName = Localization["Application_NetworkVideoRecorder"];

			SystemSecurity = new ToolStripMenuItem
			{
				Text = Localization["Menu_SystemSecurity"],
			};
			SystemSecurity.Click += SystemSecurityToolStripMenuItemClick;

			//Lock NVR
			LockNVR = new ToolStripMenuItem
			{
				Text = Localization["Menu_LockNVR"],
			};
			LockNVR.Click += LockNvrToolStripMenuItemClick;


			ApplicationMenu.DropDownItems.Clear();
			ApplicationMenu.DropDownItems.Add(SystemSecurity);
			ApplicationMenu.DropDownItems.Add(LockNVR);
			ApplicationMenu.DropDownItems.Add(SignOut);
		}

		

		public override void Activate(IPage page)
		{
			base.Activate(page);

			if (String.Equals(page.Name, "Live") || String.Equals(page.Name, "Playback"))
				BandwidthMenu.Visible = true;
			else
				BandwidthMenu.Visible = false;
		}

		public virtual void SetDefaultBandWidth(Int16 bandwidth)
		{
				switch (bandwidth)
				{
					case 0:
						_originalMenu.PerformClick();
						break;

					case 56:
						_bandwidth56K.PerformClick();
						break;

					case 256:
						_bandwidth256K.PerformClick();
						break;

					case 512:
						_bandwidth512K.PerformClick(); 
						break;

					case 1024:
						_bandwidth1M.PerformClick();
						break;
				}
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
				_isLock = value ;
				if (value)
				{
					if (!_isLockSystem)
						IsLockSystem = true;

					MainPanel.Enabled = false;
					SignOut.ShortcutKeys = Keys.None;
					LockNVR.Image = ActivateImage;

					foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
					{
						if (dropDownItem == LockNVR) continue;

						dropDownItem.Visible = false;
					}
					BandwidthMenu.Enabled = false;
					
				}
				else
				{
					if (!_isManualLock)
						IsLockSystem = false;

					MainPanel.Enabled = true;
					SignOut.ShortcutKeys = Keys.Control | Keys.W;
					LockNVR.Image = null;

					foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
					{
						if (dropDownItem == LockNVR) continue;

						dropDownItem.Visible = true;
					}
					BandwidthMenu.Enabled = true;
				}
			}
		}

		private void FrozenScreen()
		{
			KeyboardHook();
			HideToolbar();
			DisableTaskManager();

			_form.Text = "";
			_form.ControlBox = false;
			_form.WindowState = FormWindowState.Normal;
			_form.WindowState = FormWindowState.Maximized;
			_form.TopMost = true;
			_form.BringToFront();

			SystemSecurity.Image = ActivateImage;
		}

		protected Boolean _isLockSystem;
		protected Boolean _isManualLock;
		public override Boolean IsLockSystem
		{
			get { return _isLockSystem; }
			protected set
			{
				if (value)
				{
					DialogResult result = TopMostMessageBox.Show(Localization["Application_ConfirmSystemSceurity"], Localization["MessageBox_Confirm"],
						MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						FrozenScreen();

						_isLockSystem = true;
						MainPanel.Enabled = true;

					}
					else
						_isLockSystem = false;
				}
				else
				{
					if (_unlockAppForm == null)
					{
						_unlockAppForm = new UnlockAppForm
						{
							App = this,
							User = _nvr.User.Current,
							Icon = Form.Icon,
						};

						_unlockAppForm.OnCancel += UnlockAppFormOnCancel;
						_unlockAppForm.OnConfirm += UnlockAppFormOnConfirm;
					}

					// Add By Tulip for DVR Mode
					_form.TopMost = false;
					_unlockAppForm.TopMost = true;

					_unlockAppForm.ShowDialog();
				}
			}
		}

		protected virtual void UnlockAppFormOnConfirm(Object sender, EventArgs e)
		{
			ReleaseKeyboardHook();
			ShowToolbar();
			EnableTaskManager();

			foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
			{
				if (dropDownItem == LockNVR) continue;

				dropDownItem.Visible = true;
			}

			BandwidthMenu.Enabled = true;
			MainPanel.Enabled = _form.Enabled = true;

			_form.BringToFront();
			_form.Activate();
			_form.ControlBox = true;

			ResetTitleBarText();

			_form.FormBorderStyle = FormBorderStyle.Sizable;
			_form.WindowState = FormWindowState.Normal;
			_form.WindowState = FormWindowState.Maximized;
			SystemSecurity.Image = null;
			SignOut.ShortcutKeys = Keys.Control | Keys.W;
			_isLockSystem = false;
		}

		private void SystemSecurityToolStripMenuItemClick(Object sender, EventArgs e)
		{
			IsLockSystem = !_isLockSystem;
			_isManualLock = IsLockSystem;
		}

		private void LockNvrToolStripMenuItemClick(object sender, EventArgs e)
		{
			IsLock = !_isLock;
		}

		protected virtual void SetVideoStreamToOriginal(Object sender, EventArgs e)
		{
			if (_originalMenu.IsSelected) return;
			
			UpdateClientSetting("TotalBitrate", "0", null);

			_originalMenu.IsSelected = true;
			foreach (StripMenu menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _originalMenu) continue;

				menuItem.IsSelected = false;
			}

			_nvr.Configure.CustomLiveSetting.Enable = false;
			_nvr.Configure.CustomPlaybackSetting.Enable = false;

			ResetTitleBarText();

			if (OnCustomVideoStream != null)
				OnCustomVideoStream(this, null);
		}

		private Int32 _liveViewingDeviceNumber = 0;
		private void ChangeLiveCustomStream(Int32 deviceNumber)
		{
			_liveViewingDeviceNumber = deviceNumber;

			//not enable, do nothing
			if (!_nvr.Configure.CustomLiveSetting.Enable) return;

			//same device number, config is the same
			if (_liveViewingDeviceNumber == deviceNumber) return;
			CalculatorLiveVideoStreamSetting();

			//no need this if, videoMonitor will check if it's activate
			//not in live page, only change setting but dont re-connect
			//if (PageActivated.Name != "Live") {}
		}

		private void CalculatorLiveVideoStreamSetting()
		{
			var totalBitrate = Convert.ToInt32(Bitrates.ToString(_nvr.Configure.TotalLiveSetting.Bitrate));
			var count = Math.Max(_liveViewingDeviceNumber, 1);

			var shareBitrate = totalBitrate/count;
			if (shareBitrate >= 1000)
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R640X480;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate1M;
			}
			else if (shareBitrate >= 500)
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R640X480;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate500K;
			}
			else if (shareBitrate >= 250)
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R320X240;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate256K;
			}
			else if (shareBitrate >= 128)
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R240X180;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate128K;
			}
			else if (shareBitrate >= 56)
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R160X120;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate56K;
			}
			else
			{
				_nvr.Configure.CustomLiveSetting.Resolution = Resolution.R160X80;
				_nvr.Configure.CustomLiveSetting.Bitrate = Bitrate.Bitrate20K;
			}

			//will cause VideoMonitor Reconnect
			if (OnCustomVideoStream != null)
				OnCustomVideoStream(this, null);
		}

		protected virtual void SetVideoStreamTo1M(Object sender, EventArgs e)
		{
			if (_bandwidth1M.IsSelected) return;

			_bandwidth1M.IsSelected = true;
			foreach (StripMenu menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth1M) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting("TotalBitrate", "1024", null);

			_nvr.Configure.CustomLiveSetting.Enable =
			_nvr.Configure.CustomPlaybackSetting.Enable = true;

			_nvr.Configure.CustomLiveSetting.Compression =
			_nvr.Configure.CustomPlaybackSetting.Compression = Compression.H264;

			_nvr.Configure.TotalLiveSetting.Resolution =
			_nvr.Configure.TotalPlaybackSetting.Resolution = Resolution.R640X480;

			_nvr.Configure.TotalLiveSetting.Bitrate =
			_nvr.Configure.TotalPlaybackSetting.Bitrate = Bitrate.Bitrate1M;

			ResetTitleBarText();

			CalculatorLiveVideoStreamSetting();
		}

		protected virtual void SetVideoStreamTo512K(Object sender, EventArgs e)
		{
			if (_bandwidth512K.IsSelected) return;

			_bandwidth512K.IsSelected = true;
			foreach (StripMenu menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth512K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting("TotalBitrate", "512", null);

			_nvr.Configure.CustomLiveSetting.Enable =
			_nvr.Configure.CustomPlaybackSetting.Enable = true;

			_nvr.Configure.CustomLiveSetting.Compression =
			_nvr.Configure.CustomPlaybackSetting.Compression = Compression.H264;

			_nvr.Configure.TotalLiveSetting.Resolution =
			_nvr.Configure.TotalPlaybackSetting.Resolution = Resolution.R640X480;

			_nvr.Configure.TotalLiveSetting.Bitrate =
			_nvr.Configure.TotalPlaybackSetting.Bitrate = Bitrate.Bitrate512K;

			ResetTitleBarText();

			CalculatorLiveVideoStreamSetting();
		}

		protected virtual void SetVideoStreamTo256K(Object sender, EventArgs e)
		{
			if (_bandwidth256K.IsSelected) return;

			_bandwidth256K.IsSelected = true;
			foreach (StripMenu menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth256K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting("TotalBitrate", "256", null);

			_nvr.Configure.CustomLiveSetting.Enable =
			_nvr.Configure.CustomPlaybackSetting.Enable = true;

			_nvr.Configure.CustomLiveSetting.Compression =
			_nvr.Configure.CustomPlaybackSetting.Compression = Compression.H264;

			_nvr.Configure.TotalLiveSetting.Resolution =
			_nvr.Configure.TotalPlaybackSetting.Resolution = Resolution.R320X240;

			_nvr.Configure.TotalLiveSetting.Bitrate =
			_nvr.Configure.TotalPlaybackSetting.Bitrate = Bitrate.Bitrate256K;

			ResetTitleBarText();

			CalculatorLiveVideoStreamSetting();
		}

		protected virtual void SetVideoStreamTo56K(Object sender, EventArgs e)
		{
			if (_bandwidth56K.IsSelected) return;

			_bandwidth56K.IsSelected = true;
			foreach (StripMenu menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth56K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting("TotalBitrate", "56", null);

			_nvr.Configure.CustomLiveSetting.Enable =
			_nvr.Configure.CustomPlaybackSetting.Enable = true;

			_nvr.Configure.CustomLiveSetting.Compression =
			_nvr.Configure.CustomPlaybackSetting.Compression = Compression.H264;

			_nvr.Configure.TotalLiveSetting.Resolution =
			_nvr.Configure.TotalPlaybackSetting.Resolution = Resolution.R160X120;

			_nvr.Configure.TotalLiveSetting.Bitrate =
			_nvr.Configure.TotalPlaybackSetting.Bitrate = Bitrate.Bitrate56K;

			ResetTitleBarText();

			CalculatorLiveVideoStreamSetting();
		}
	}
}
