using System;
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
        //protected ToolStripMenuItemUI2 BandwidthMenu;
        //private ToolStripMenuItemUI2 _originalMenu;
        //private ToolStripMenuItemUI2 _bandwidth1M;
        //private ToolStripMenuItemUI2 _bandwidth512K;
        //private ToolStripMenuItemUI2 _bandwidth256K;
        //private ToolStripMenuItemUI2 _bandwidth56K;
		
        //protected override void InitializeHeaderPanel()
        //{
        //    base.InitializeHeaderPanel();

        //    BandwidthMenu = new ToolStripMenuItemUI2
        //    {
        //        Alignment = ToolStripItemAlignment.Left,
        //    };
        //    //----------------------------------------
        //    _originalMenu = new ToolStripMenuItemUI2
        //    {
        //        Text = Localization["Menu_OriginalStreaming"],
        //        IsSelected = true,
        //    };
        //    _originalMenu.Click += SetVideoStreamToOriginal;

        //    _bandwidth1M = new ToolStripMenuItemUI2();
        //    _bandwidth1M.Click += SetVideoStreamTo1M;

        //    _bandwidth512K = new ToolStripMenuItemUI2();
        //    _bandwidth512K.Click += SetVideoStreamTo512K;

        //    _bandwidth256K = new ToolStripMenuItemUI2();
        //    _bandwidth256K.Click += SetVideoStreamTo256K;

        //    _bandwidth56K = new ToolStripMenuItemUI2();
        //    _bandwidth56K.Click += SetVideoStreamTo56K;

        //    BandwidthMenu.Text = Localization["Menu_Bandwidth"];
        //    _bandwidth1M.Text = Localization["Menu_1M"];
        //    _bandwidth512K.Text = Localization["Menu_512K"];
        //    _bandwidth256K.Text = Localization["Menu_256K"];
        //    _bandwidth56K.Text = Localization["Menu_56K"];

        //    BandwidthMenu.DropDownItems.AddRange(new[] {
        //        _originalMenu,
        //        _bandwidth1M,
        //        _bandwidth512K,
        //        _bandwidth256K,
        //        _bandwidth56K
        //    });
        //    //--------add BandwidthMenu after application
        //    MenuStrip.Items.Clear();
        //    MenuStrip.Items.AddRange(new[] {
        //        ApplicationMenu,
        //        BandwidthMenu,
        //        HidePanelStripMenuItem,
        //    });
        //    //----------------------------------------
        //    BandwidthMenu.Visible = false;
			
        //    AppName = Localization["Application_POSTransactionServer"];

        //    ApplicationMenu.DropDownItems.Clear();
        //    ApplicationMenu.DropDownItems.Add(SignOut);
        //}

        //public override void Activate(IPage page)
        //{
        //    base.Activate(page);

        //    switch (_pts.ReleaseBrand)
        //    {
        //        case "Salient":
        //            if (String.Equals(page.Name, "Live"))
        //                BandwidthMenu.Visible = true;
        //            else
        //                BandwidthMenu.Visible = false;
        //            break;

        //        default:
        //            if (String.Equals(page.Name, "Live") || String.Equals(page.Name, "Playback"))
        //                BandwidthMenu.Visible = true;
        //            else
        //                BandwidthMenu.Visible = false;
        //            break;
        //    }
        //}

        //private UnlockAppForm _unlockAppForm;
        //private Boolean _isLock;
        //public override Boolean IsLock
        //{
        //    get
        //    {
        //        return _isLock;
        //    }
        //    protected set
        //    {
        //        if(value)
        //        {
        //            DialogResult result = TopMostMessageBox.Show(Localization["Application_ConfirmLockApp"], Localization["MessageBox_Confirm"],
        //                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            if (result != DialogResult.Yes) return;

        //            _isLock = true;

        //            LockApplication();
        //        }
        //        else
        //        {
        //            //ask user's account password to unlock
        //            _unlockAppForm.TopMost = true;
        //            _unlockAppForm.ShowDialog();
        //        }
        //    }
        //}

        //private void UnlockAppFormOnConfirm(Object sender, EventArgs e)
        //{
        //    UnlockApplication();

        //    ResetTitleBarText();
        //    UpdateMenuVisible();

        //    _isLock = false;
        //}

        //private void UpdateMenuVisible()
        //{
        //    switch (PageActivated.Name)
        //    {
        //        case "Live":
        //            FullscreenMenu.Visible = true;
        //            BandwidthMenu.Visible = true;
        //            break;

        //        case "Playback":
        //            FullscreenMenu.Visible = true;
        //            BandwidthMenu.Visible = true;
        //            break;

        //        default:
        //            FullscreenMenu.Visible = false;
        //            BandwidthMenu.Visible = false;
        //            break;
        //    }
        //    RefreshMenuStripIconStyle();
        //}

        //protected virtual void SetVideoStreamToOriginal(Object sender, EventArgs e)
        //{
        //    if (_originalMenu.IsSelected) return;

        //    _originalMenu.IsSelected = true;
        //    foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
        //    {
        //        if(menuItem == _originalMenu) continue;

        //        menuItem.IsSelected = false;
        //    }

        //    _pts.Configure.CustomStreamSetting.Enable = false; 

        //    RaiseOnCustomVideoStream();
        //}

        //protected virtual void SetVideoStreamTo1M(Object sender, EventArgs e)
        //{
        //    if (_bandwidth1M.IsSelected) return;

        //    _bandwidth1M.IsSelected = true;
        //    foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
        //    {
        //        if (menuItem == _bandwidth1M) continue;

        //        menuItem.IsSelected = false;
        //    }

        //    _pts.Configure.CustomStreamSetting.Enable = true;

        //    _pts.Configure.CustomStreamSetting.Compression = Compression.H264;
        //    _pts.Configure.CustomStreamSetting.Resolution =  Resolution.R640X480;
        //    _pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate1M;
        //    _pts.Configure.CustomStreamSetting.Framerate =  0;

        //    RaiseOnCustomVideoStream();
        //}

        //protected virtual void SetVideoStreamTo512K(Object sender, EventArgs e)
        //{
        //    if (_bandwidth512K.IsSelected) return;

        //    _bandwidth512K.IsSelected = true;
        //    foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
        //    {
        //        if (menuItem == _bandwidth512K) continue;

        //        menuItem.IsSelected = false;
        //    }

        //    _pts.Configure.CustomStreamSetting.Enable = true;

        //    _pts.Configure.CustomStreamSetting.Compression = Compression.H264;
        //    _pts.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
        //    _pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate512K;
        //    _pts.Configure.CustomStreamSetting.Framerate = 0;

        //    RaiseOnCustomVideoStream();
        //}

        //protected virtual void SetVideoStreamTo256K(Object sender, EventArgs e)
        //{
        //    if (_bandwidth256K.IsSelected) return;

        //    _bandwidth256K.IsSelected = true;
        //    foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
        //    {
        //        if (menuItem == _bandwidth256K) continue;

        //        menuItem.IsSelected = false;
        //    }

        //    _pts.Configure.CustomStreamSetting.Enable = true;

        //    _pts.Configure.CustomStreamSetting.Compression = Compression.H264;
        //    _pts.Configure.CustomStreamSetting.Resolution =Resolution.R320X240;
        //    _pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
        //    _pts.Configure.CustomStreamSetting.Framerate = 0;

        //    RaiseOnCustomVideoStream();
        //}

        //protected virtual void SetVideoStreamTo56K(Object sender, EventArgs e)
        //{
        //    if (_bandwidth56K.IsSelected) return;

        //    _bandwidth56K.IsSelected = true;
        //    foreach (ToolStripMenuItemUI2 menuItem in BandwidthMenu.DropDownItems)
        //    {
        //        if (menuItem == _bandwidth56K) continue;

        //        menuItem.IsSelected = false;
        //    }

        //    _pts.Configure.CustomStreamSetting.Enable = true;

        //    _pts.Configure.CustomStreamSetting.Compression = Compression.H264;
        //    _pts.Configure.CustomStreamSetting.Resolution =Resolution.R160X120;
        //    _pts.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;

        //    _pts.Configure.CustomStreamSetting.Framerate = 0;

        //    RaiseOnCustomVideoStream();
        //}
	}
}
