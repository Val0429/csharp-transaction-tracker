using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using App;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace App_POSTransactionServer
{
    public partial class POSTransactionServer
	{
		private ToolStripMenuItemUI2 _originalMenu;
		private ToolStripMenuItemUI2 _bandwidth1M;
		private ToolStripMenuItemUI2 _bandwidth512K;
		private ToolStripMenuItemUI2 _bandwidth256K;
		private ToolStripMenuItemUI2 _bandwidth56K;
		protected override void InitializeHeaderPanel()
		{
			base.InitializeHeaderPanelUI2();

			//----------------------------------------
			_originalMenu = new ToolStripMenuItemUI2
			{
				Text = Localization["Menu_OriginalStreaming"],
				IsSelected = true,
			};
			_originalMenu.Click += SetVideoStreamToOriginal;

			_bandwidth1M = new ToolStripMenuItemUI2 { Text = Localization["Menu_1M"] };
			_bandwidth1M.Click += SetVideoStreamTo1M;

			_bandwidth512K = new ToolStripMenuItemUI2 { Text = Localization["Menu_512K"] };
			_bandwidth512K.Click += SetVideoStreamTo512K;

			_bandwidth256K = new ToolStripMenuItemUI2 { Text = Localization["Menu_256K"] };
			_bandwidth256K.Click += SetVideoStreamTo256K;

			_bandwidth56K = new ToolStripMenuItemUI2 { Text = Localization["Menu_56K"] };
			_bandwidth56K.Click += SetVideoStreamTo56K;

            switch (_pts.ReleaseBrand)
            {
                case "Salient":
                    _bandwidth1M.Text = @"1080P";
                    _bandwidth512K.Text = @"720P"; //Localization["Menu_512KVGA"].Split(' ')[1];
                    _bandwidth256K.Text = @"4 CIF"; //Localization["Menu_256KCIF"].Split(' ')[1];
                    _bandwidth56K.Text = @"CIF"; //"Localization["Menu_128KQCIF"].Split(' ')[1];
                    BandwidthMenu.ToolTipText = Localization["Menu_Resolution"];

                    if (File.Exists(Application.StartupPath + "\\images\\headerBg.png"))
                    {
                        Image headerBg = Image.FromFile(Application.StartupPath + "\\images\\headerBg.png");
                        HeaderPanel.BackgroundImage = headerBg;
                    }
                    _bandwidth1M.DropDown.BackColor = Color.FromArgb(39, 41, 44);
                    _bandwidth1M.DropDownDirection = ToolStripDropDownDirection.Left;
                    _bandwidth1M.Tag = Resolution.R1920X1080;
                    _bandwidth1M.DropDownItems.AddRange(new[]
                                                            {
                                                                new ToolStripMenuItemUI2 { Text = Localization["Menu_Original"], Tag = 0},
                                                                new ToolStripMenuItemUI2 { Text = @"7 fps" , Tag = 7},
                                                                new ToolStripMenuItemUI2 { Text = @"10 fps", Tag = 10 }
                                                            });

                    foreach (ToolStripMenuItemUI2 item in _bandwidth1M.DropDownItems)
                    {
                        item.Click += FPSItemClick;
                    }

                    _bandwidth512K.DropDown.BackColor = Color.FromArgb(39, 41, 44);
                    _bandwidth512K.DropDownDirection = ToolStripDropDownDirection.Left;
                    _bandwidth512K.BackColor = Color.Black;
                    _bandwidth512K.Tag = Resolution.R1280X720;
                    _bandwidth512K.DropDownItems.AddRange(new[]
                                                            {
                                                                new ToolStripMenuItemUI2 { Text = Localization["Menu_Original"], Tag = 0},
                                                                new ToolStripMenuItemUI2 { Text = @"7 fps" , Tag = 7},
                                                                new ToolStripMenuItemUI2 { Text = @"10 fps", Tag = 10 }
                                                            });

                    foreach (ToolStripMenuItemUI2 item in _bandwidth512K.DropDownItems)
                    {
                        item.Click += FPSItemClick;
                    }

                    _bandwidth256K.DropDown.BackColor = Color.FromArgb(39, 41, 44);
                    _bandwidth256K.DropDownDirection = ToolStripDropDownDirection.Left;
                    _bandwidth256K.BackColor = Color.Black;
                    _bandwidth256K.Tag = Resolution.R720X480;
                    _bandwidth256K.DropDownItems.AddRange(new[]
                                                            {
                                                                new ToolStripMenuItemUI2 { Text = Localization["Menu_Original"], Tag = 0},
                                                                new ToolStripMenuItemUI2 { Text = @"7 fps" , Tag = 7},
                                                                new ToolStripMenuItemUI2 { Text = @"10 fps", Tag = 10 }
                                                            });

                    foreach (ToolStripMenuItemUI2 item in _bandwidth256K.DropDownItems)
                    {
                        item.Click += FPSItemClick;
                    }

                    _bandwidth56K.DropDown.BackColor = Color.FromArgb(39, 41, 44);
                    _bandwidth56K.DropDownDirection = ToolStripDropDownDirection.Left;
                    _bandwidth56K.BackColor = Color.Black;
                    _bandwidth56K.Tag = Resolution.R352X288;
                    _bandwidth56K.DropDownItems.AddRange(new[]
                                                            {
                                                                new ToolStripMenuItemUI2 { Text = Localization["Menu_Original"], Tag = 0},
                                                                new ToolStripMenuItemUI2 { Text = @"7 fps" , Tag = 7},
                                                                new ToolStripMenuItemUI2 { Text = @"10 fps", Tag = 10 }
                                                            });

                    foreach (ToolStripMenuItemUI2 item in _bandwidth56K.DropDownItems)
                    {
                        item.Click += FPSItemClick;
                    }

                    //if (File.Exists(Application.StartupPath + "\\images\\loadingClock.png"))
                    //{
                    //    Image loading = Image.FromFile(Application.StartupPath + "\\images\\loadingClock.png");
                    //    LoadingIcon.BackgroundImage = loading;
                    //}

                    BandwidthMenu.DropDownItems.AddRange(new[] {
				        _originalMenu,
				        _bandwidth1M,
				        _bandwidth512K,
				        _bandwidth256K,
				        _bandwidth56K
			        });

                    _originalMenu.Click -= SetVideoStreamToOriginal;
			        _bandwidth1M.Click -= SetVideoStreamTo1M;
			        _bandwidth512K.Click -= SetVideoStreamTo512K;
			        _bandwidth256K.Click -= SetVideoStreamTo256K;
			        _bandwidth56K.Click -= SetVideoStreamTo56K;
                    _originalMenu.Click += FPSItemClick;
                    break;

                default:
                    BandwidthMenu.DropDownItems.AddRange(new[] {
				        _originalMenu,
				        //_bandwidth1M,
				        _bandwidth512K,
				        _bandwidth256K,
				        _bandwidth56K
			        });
                    break;
            }

			//----------------------------------------
            AppName = Localization["Application_POSTransactionServer"];

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

        private void FPSItemClick(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItemUI2;
            if (item == null) return;

            _pts.Configure.CustomStreamSetting.Enable = true;

            foreach (ToolStripMenuItemUI2 resolutionItem in BandwidthMenu.DropDownItems)
            {
                if(resolutionItem == null) continue;
                resolutionItem.IsSelected = false;

                if (resolutionItem == item)//only original bandwidth will fire this event
                {
                    resolutionItem.IsSelected = true;
                    _pts.Configure.CustomStreamSetting.Enable = false;
                }

                foreach (ToolStripMenuItemUI2 fpsItem in resolutionItem.DropDownItems)
                {
                    if (fpsItem == null) continue;
                    fpsItem.IsSelected = false;
                    if(fpsItem == item)
                    {
                        fpsItem.IsSelected = resolutionItem.IsSelected = true;
                        _pts.Configure.CustomStreamSetting.Resolution = (Resolution) resolutionItem.Tag;
                        _pts.Configure.CustomStreamSetting.Framerate = Convert.ToUInt16(fpsItem.Tag);
                    }
                }
            }

            CalculatorVideoStreamSetting();
        }

        protected override void FullscreenMenuToolStripMenuItemClick(Object sender, EventArgs e)
        {
            FullScreen();
        }

		private Boolean _logPageInfo = false;
		public override void Activate(IPage page)
		{
			base.Activate(page);

			//log page AFTER first page is activate (like switch page from live -> playback)
            switch (_pts.ReleaseBrand)
            {
                case "Salient":
                    if (String.Equals(page.Name, "Live")  || String.Equals(page.Name, "Playback"))
                        BandwidthMenu.Visible = true;
                    else
                        BandwidthMenu.Visible = false;
                    break;

                default:
                    if (String.Equals(page.Name, "Live") || String.Equals(page.Name, "Playback"))
                        BandwidthMenu.Visible = true;
                    else
                        BandwidthMenu.Visible = false;
                    break;
            }

			if (_logPageInfo)
				LogCurrentViewLayout();

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
					BandwidthMenu.Visible = (_pts.Server.CheckProductNoToSupport("bandwidthControl") ||  _pts.ReleaseBrand == "Salient");//playback DO support
					break;

				case "Investigation":
					FullscreenMenu.Visible = false;
					BandwidthMenu.Visible = (_pts.Server.CheckProductNoToSupport("bandwidthControl")  ||  _pts.ReleaseBrand == "Salient");//playback DO support
					break;

				default:
					FullscreenMenu.Visible = false;
					BandwidthMenu.Visible = false;
					break;
			}

            if (_pts.Configure.EnableBandwidthControl)
            {
                BandwidthMenu.Visible = false;
            }
            else
            {
                if (!_pts.Configure.CustomStreamSetting.Enable && _pts.Configure.CustomStreamSetting.Bitrate == Bitrate.NA)
                {
                    SetVideoStreamToOriginal(null, null);
                }
            }

			RefreshMenuStripIconStyle();
		}

		public virtual void SetDefaultBandWidth(Int16 bandwidth)
		{
				switch (bandwidth)
				{
					case 56:
						SetVideoStreamTo56K(null, null);
						break;

					case 256:
						SetVideoStreamTo256K(null, null);
						break;

					case 512:
						SetVideoStreamTo512K(null, null);
						break;

					case 1024:
						SetVideoStreamTo1M(null, null);
						break;

					default:
						SetVideoStreamToOriginal(null, null);
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
                    _unlockAppForm.FormBorderStyle = FormBorderStyle.FixedDialog;
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

		private void SetVideoStreamToOriginal(Object sender, EventArgs e)
		{
			if (_originalMenu.IsSelected) return;
			
			UpdateClientSetting(RestoreClientColumn.TotalBitrate, "0", null);

			_originalMenu.IsSelected = true;
			foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _originalMenu) continue;

				menuItem.IsSelected = false;
			}

			_pts.Configure.CustomStreamSetting.Enable = false;

			ResetTitleBarText();

			RaiseOnCustomVideoStream();
		}

		private Bitrate _totalBitrate;
		private void SetVideoStreamTo1M(Object sender, EventArgs e)
		{
			if (_bandwidth1M.IsSelected) return;

			_bandwidth1M.IsSelected = true;
			foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth1M) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting(RestoreClientColumn.TotalBitrate, "1024", null);

			_pts.Configure.CustomStreamSetting.Enable = true;

			_pts.Configure.CustomStreamSetting.Compression = Compression.H264;
			//_pts.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
			//_pts.Configure.CustomStreamSetting.Bitrate =  Bitrate.Bitrate1M;
			//for ap title display
			_totalBitrate = Bitrate.Bitrate1M;

			ResetTitleBarText();

			CalculatorVideoStreamSetting();
		}

		private void SetVideoStreamTo512K(Object sender, EventArgs e)
		{
			if (_bandwidth512K.IsSelected) return;

			_bandwidth512K.IsSelected = true;
			foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth512K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting(RestoreClientColumn.TotalBitrate, "512", null);

			_pts.Configure.CustomStreamSetting.Enable = true;

			_pts.Configure.CustomStreamSetting.Compression =  Compression.H264;

            if(_pts.ReleaseBrand == "Salient")
            {
                _pts.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                //_pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate512K;
            }

			//for ap title display
			_totalBitrate = Bitrate.Bitrate512K;

			ResetTitleBarText();

			CalculatorVideoStreamSetting();
		}

		private void SetVideoStreamTo256K(Object sender, EventArgs e)
		{
			if (_bandwidth256K.IsSelected) return;

			_bandwidth256K.IsSelected = true;
			foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth256K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting(RestoreClientColumn.TotalBitrate, "256", null);

			_pts.Configure.CustomStreamSetting.Enable = true;

			_pts.Configure.CustomStreamSetting.Compression = Compression.H264;
            if (_pts.ReleaseBrand == "Salient")
            {
                _pts.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
                //_pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate512K;
            }
			//_pts.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
			//_pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
			//for ap title display
			_totalBitrate = Bitrate.Bitrate256K;

			ResetTitleBarText();

			CalculatorVideoStreamSetting();
		}

		private void SetVideoStreamTo56K(Object sender, EventArgs e)
		{
			if (_bandwidth56K.IsSelected) return;

			_bandwidth56K.IsSelected = true;
			foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
			{
				if (menuItem == _bandwidth56K) continue;

				menuItem.IsSelected = false;
			}

			UpdateClientSetting(RestoreClientColumn.TotalBitrate, "56", null);

			_pts.Configure.CustomStreamSetting.Enable = true;

			_pts.Configure.CustomStreamSetting.Compression =  Compression.H264;
            if (_pts.ReleaseBrand == "Salient")
            {
                _pts.Configure.CustomStreamSetting.Resolution = Resolution.R160X120;
                //_pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate512K;
            }
			//_pts.Configure.CustomStreamSetting.Resolution =  Resolution.R160X120;
			//_pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;
			//for ap title display
			_totalBitrate = Bitrate.Bitrate56K;

			ResetTitleBarText();

			CalculatorVideoStreamSetting();
		}
	}
}
