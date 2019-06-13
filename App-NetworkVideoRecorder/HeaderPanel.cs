using System;
using System.Windows.Forms;
using App;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
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

            BandwidthMenu.DropDownItems.AddRange(new[] {
				_originalMenu,
				_bandwidth1M,
				_bandwidth512K,
				_bandwidth256K,
				_bandwidth56K
			});
            //----------------------------------------
            AppName = Localization["Application_NetworkVideoRecorder"];

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

            //log page AFTER first page is activate (like switch page from live -> playback)

            if (_logPageInfo)
                LogCurrentViewLayout();

            if (!_logPageInfo)
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
                    BandwidthMenu.Visible = Nvr.Server.CheckProductNoToSupport("bandwidthControl");
                    break;

                case "Playback":
                    FullscreenMenu.Visible = true;
                    BandwidthMenu.Visible = Nvr.Server.CheckProductNoToSupport("bandwidthControl");//playback DO support
                    break;

                case "Investigation":
                    FullscreenMenu.Visible = false;
                    BandwidthMenu.Visible = Nvr.Server.CheckProductNoToSupport("bandwidthControl");//playback DO support
                    break;

                default:
                    FullscreenMenu.Visible = false;
                    BandwidthMenu.Visible = false;
                    break;
            }

            if (Nvr.Configure.EnableBandwidthControl)
            {
                BandwidthMenu.Visible = false;
            }
            else
            {
                if (!Nvr.Configure.CustomStreamSetting.Enable && Nvr.Configure.CustomStreamSetting.Bitrate == Bitrate.NA)
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

            Nvr.Configure.CustomStreamSetting.Enable = false;

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

            Nvr.Configure.CustomStreamSetting.Enable = true;

            Nvr.Configure.CustomStreamSetting.Compression = Compression.H264;
            //_nvr.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
            //_nvr.Configure.CustomStreamSetting.Bitrate =  Bitrate.Bitrate1M;
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

            Nvr.Configure.CustomStreamSetting.Enable = true;

            Nvr.Configure.CustomStreamSetting.Compression = Compression.H264;
            //_nvr.Configure.CustomStreamSetting.Resolution =  Resolution.R640X480;
            //_nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate512K;
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

            Nvr.Configure.CustomStreamSetting.Enable = true;

            Nvr.Configure.CustomStreamSetting.Compression = Compression.H264;
            //_nvr.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
            //_nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
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

            Nvr.Configure.CustomStreamSetting.Enable = true;

            Nvr.Configure.CustomStreamSetting.Compression = Compression.H264;
            //_nvr.Configure.CustomStreamSetting.Resolution =  Resolution.R160X120;
            //_nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;
            //for ap title display
            _totalBitrate = Bitrate.Bitrate56K;

            ResetTitleBarText();

            CalculatorVideoStreamSetting();
        }
    }
}
