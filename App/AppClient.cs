using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace App
{
    public partial class AppClient : IApp
    {
        public event EventHandler OnLogout;
        public event EventHandler<EventArgs<String, Object>> OnSwitchPage;
        public event EventHandler OnCustomVideoStream;
        public static Boolean IsAdministrator;
        protected void RaiseOnCustomVideoStream()
        {
            if (OnCustomVideoStream != null)
            {
                OnCustomVideoStream(this, EventArgs.Empty);
            }
        }

        public Boolean OpenAnotherProcessAfterLogout { get; set; }

        public event EventHandler<EventArgs<ICamera>> OnHotSpotEvent;
        protected void RaiseOnHotSpotEvent(EventArgs<ICamera> e)
        {
            var handler = OnHotSpotEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public virtual event EventHandler<EventArgs<Boolean>> OnLockApplication;

        public virtual event EventHandler<EventArgs<Int32, Int32, Int32>> OnJoystickMove;
        public virtual event EventHandler<EventArgs<String, String, Int32>> OnJoystickOperation;

        public virtual event EventHandler<EventArgs<String>> OnOpcTreeReceived;
        public virtual String KeyPad { get; set; }

        public virtual event EventHandler<EventArgs<Boolean>> OnAutoLoadVideoOn;
        public virtual event EventHandler<EventArgs<Boolean>> OnTitleBarFormatOn;
        public virtual event EventHandler<EventArgs<Boolean>> OnProfileSelectionOn;
        public virtual event EventHandler<EventArgs<Boolean>> OnTranscodeStreamOn;

        public virtual event EventHandler OnAppStarted;

        protected void RaiseOnAppStarted(EventArgs e)
        {
            if (OnAppStarted != null)
            {
                OnAppStarted(this, e);
            }
        }

        public event EventHandler SaveCompleted;

        protected void OnSaveCompleted(EventArgs e)
        {
            var handler = SaveCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler OnUserDefineDeviceGroupModify;

        protected void RaiseOnUserDefineDeviceGroupModify(EventArgs e)
        {
            var handler = OnUserDefineDeviceGroupModify;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<EventArgs<Boolean>> OnSupportImageStitching;
        protected void RaiseOnSupportImageStitching(EventArgs<Boolean> e)
        {
            if (OnSupportImageStitching != null)
            {
                OnSupportImageStitching(this, e);
            }
        }

        public Boolean isSupportImageStitching { get; set; }
        public virtual void ImageStitching()
        {
            isSupportImageStitching = true;
        }

        public Dictionary<String, String> Localization { get; set; }
        protected AppClientPropertiesBase AppProperties;

        public String Version { get; protected set; }
        public String DevicePackVersion { get; protected set; }

        public virtual ulong PlaybackTimeCode { get; set; }

        public virtual Boolean IsLock { get; protected set; }

        public Boolean IsInitialize { get; protected set; }

        public virtual Form Form { get; set; }
        public virtual ServerCredential Credential { get; set; }
        public virtual String Language { get; set; }

        public Dictionary<String, IPage> Pages { get; protected set; }
        public IPage PageActivated { get; set; }

        public virtual String LoginProgress { get; protected set; }

        public virtual Boolean SwitchDVRMode(String mode, bool status)
        {
            return true;
        }
        public virtual Int64 IdleTimer { get; set; }

        public virtual void RemoveProperties() { }

        public virtual StartupOptions StartupOption { get; set; }

        public Panel ToolPanel { get; set; }
        public Panel SwitchPagePanel { get; set; }
        public Panel PageFunctionPanel { get; set; }
        public Panel PageDockIconPanel { get; set; }
        public Panel MiniFunctionPanel { get; set; }
        public Panel HeaderPanel { get; set; }
        public Panel MainPanel { get; set; }
        public Panel WorkPanel { get; set; }
        public Panel StatePanel { get; set; }
        protected PictureBox LogoPictureBox;

        protected MenuStrip MenuStrip;

        protected ToolStripMenuItem ApplicationMenu;
        protected ToolStripMenuItem BandwidthMenu;
        protected ToolStripMenuItem SetupMenu;
        protected ToolStripMenuItem FullscreenMenu;

        protected ToolStripMenuItemUI2 LockApp;
        protected ToolStripMenuItemUI2 SignOut;
        protected ToolStripMenuItemUI2 About;
        protected ToolStripMenuItem HidePanelStripMenuItem;

        private Image _menuIconBg;
        private Image _menuIconBg2;
        private Image _menuIconBg3;
        private Image _menuIconBg3On;

        private Boolean _isFunctionPanelExpand = true;
        protected virtual void InitializePanel()
        {
            OpenAnotherProcessAfterLogout = false;

            _menuIconBg = Resources.GetResources(Properties.Resources.menuIconBG, Properties.Resources.IMGMenuIconBG);
            _menuIconBg2 = Resources.GetResources(Properties.Resources.menuIconBG2, Properties.Resources.IMGMenuIconBG2);
            _menuIconBg3 = Resources.GetResources(Properties.Resources.menuIconBG3, Properties.Resources.IMGMenuIconBG3);
            _menuIconBg3On = Resources.GetResources(Properties.Resources.menuIconBG3On, Properties.Resources.IMGMenuIconBG3On);

            InitializeWorkPanel();
            InitializeHeaderPanel();
            InitializeToolPanel();
            InitializeStatePanel();

            IsInitialize = true;
        }

        protected virtual void InitializeWorkPanel()
        {
            MainPanel = ApplicationForms.MainPanel();

            WorkPanel = ApplicationForms.WorkPanel();

            WorkPanel.MouseUp += WorkPanelMouseUp;
            WorkPanel.MouseMove += WorkPanelMouseMove;

            MainPanel.Controls.Add(WorkPanel);
            Form.Controls.Add(MainPanel);
        }

        //---------------------------------------------------------------------------------------------
        public virtual Boolean Login()
        {
            var file = Path.Combine(StartupOptions.SettingFilePath(), StartupOptions.SettingFile);
            if (File.Exists(file))
                StartupOption = new StartupOptions();

            return true;
        }

        public virtual void Logout()
        {
            if (StartupOption != null)
            {
                StartupOption.LogoutProcess = true;
                StartupOption.ClearSetting();
            }

            CancelAutoLogin();

            Quit();

            if (OnLogout != null)
                OnLogout(this, null);
        }

        public virtual void Quit()
        {
        }

        public virtual void Save()
        {
        }

        public virtual void Undo()
        {
        }

        public virtual void Activate()
        {
        }

        public virtual void Activate(IPage page)
        {
            Form.Shown += FormShown;

            ActivatePage(page);

            if (PageActivated.Layout != null)
                PageActivated.Layout.Refresh();
        }

        protected virtual void ActivateDockableControl()
        {
            if (PageActivated.Version != "2.0") return;

            PageDockIconPanel.Controls.Clear();
            foreach (var blockPanel in PageActivated.Layout.BlockPanels)
            {
                if (!blockPanel.IsDockable) continue;
                foreach (var controlPanel in blockPanel.ControlPanels)
                {
                    if (controlPanel.Icon == null) continue;

                    PageDockIconPanel.Controls.Add(controlPanel.Icon);
                    controlPanel.Icon.BringToFront();
                }
            }
        }

        private void ActivatePage(IPage page)
        {
            if (!Pages.ContainsValue(page)) return;
            if (PageActivated == page) return;
            if (!page.IsCoolDown) return;

            IPage previousPage = null;
            if (PageActivated != null)
                previousPage = PageActivated;

            PageActivated = page;

            if (!PageActivated.IsInitialize)
                PageActivated.Initialize();

            if (IsHidePanel)
                PageActivated.HidePanel();
            else
                PageActivated.ShowPanel();

            //--------------- add page content ------------------------------
            WorkPanel.Controls.Add(PageActivated.Content);
            PageActivated.Content.BringToFront();

            if (PageActivated.Function.Controls.Count > 0)
            {
                PageFunctionPanel.Controls.Add(PageActivated.Function);
                PageActivated.Function.BringToFront();
            }

            ActivateDockableControl();

            if (PageActivated.Layout != null)
            {
                foreach (ToolStripMenuItem menu in PageActivated.Layout.Menus)
                {
                    if (menu == null) continue;
                    ApplicationMenu.DropDownItems.Add(menu);
                }
            }

            //----------------add fixed menu item ------------------------------
            if (LockApp != null)
                ApplicationMenu.DropDownItems.Add(LockApp);

            if (SignOut != null)
                ApplicationMenu.DropDownItems.Add(SignOut);

            if (About != null)
                ApplicationMenu.DropDownItems.Add(About);

            //--------------- remove previous page content ------------------------------
            if (previousPage != null)
            {
                previousPage.Deactivate();
                WorkPanel.Controls.Remove(previousPage.Content);
                PageFunctionPanel.Controls.Remove(previousPage.Function);

                foreach (ToolStripMenuItem menu in previousPage.Layout.Menus)
                    ApplicationMenu.DropDownItems.Remove(menu);
            }

            //--------------IF NVR IS LOCK, ONL SHOW LOCK NVR MENU
            if (IsLock)
            {
                foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
                {
                    if (dropDownItem == LockApp) continue;

                    dropDownItem.Visible = false;
                }
            }

            //--------------- activate it! -----------------------------------
            PageActivated.Activate();
        }

        public virtual void Deactivate()
        {
            if (PageActivated != null)
                PageActivated.Deactivate();

            PageActivated = null;
        }

        public virtual void PopupInstantPlayback(IDevice device, UInt64 timecode)
        {
        }

        public virtual void PopupInstantPlayback(IDevice device, ulong timecode, object info)
        {

        }

        public virtual void PopupLiveStream(IDevice device)
        {
        }

        public virtual void ExportVideo(IDevice[] usingDevices, DateTime start, DateTime end)
        {
        }

        public virtual void ExportVideoWithInfo(IDevice[] usingDevices, DateTime start, DateTime end, String xmlInfo)
        {
        }

        public virtual void DownloadCase(IDevice[] usingDevices, DateTime start, DateTime end, XmlDocument xmlDoc)
        {
        }

        public virtual void PrintImage(List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime)
        {
        }

        public virtual void SaveUserDefineDeviceGroup(IDeviceGroup group)
        {
        }

        public virtual void DeleteUserDefineDeviceGroup(IDeviceGroup group)
        {
        }
        //---------------------------------------------------------------------------------------------
        public virtual void RegistViewer(UInt16 count)
        {
        }

        public virtual IViewer RegistViewer()
        {
            return null;
        }

        public virtual void UnregistViewer(IViewer viewer)
        {
        }

        public Int32 PlaybackCount { get; set; }
        public virtual CustomStreamSetting CustomStreamSetting
        {
            get { return null; }
        }

        public virtual Int32 AudioOutChannelCount { get { return 0; } }
        //---------------------------------------------------------------------------------------------

        protected virtual void FormShown(Object sender, EventArgs e)
        {
            Form.Shown -= FormShown;

            RefreshMenuStripIconStyle();
            if (PageActivated != null && PageActivated.Layout != null)
                PageActivated.Layout.Refresh();
        }


        public void CancelAutoLogin()
        {
            try
            {
                AppProperties.DefaultAutoSignIn = false;
                AppProperties.SaveProperties();
            }
            catch (Exception)
            {
            }
        }

        public void CheckDisplayLocation(Form window)
        {
            try
            {
                Boolean hasScreen = false;
                String name = AppProperties.DefaultScreenName;
                if ((name != ""))
                {

                    hasScreen = (Screen.AllScreens.Any(screen => screen.GetHashCode() == Convert.ToInt32(name)));
                }

                if (AppProperties.DefaultWindowState == "Maximized")
                {
                    if (hasScreen)
                        window.Location = new Point(AppProperties.DefaultWindowLocationX, AppProperties.DefaultWindowLocationY);
                    else
                        window.StartPosition = FormStartPosition.CenterScreen;


                    window.WindowState = FormWindowState.Maximized;
                    return;
                }

                if (AppProperties.DefaultWindowWidth == 0 || AppProperties.DefaultWindowHeight == 0)
                {
                    window.StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    window.Height = AppProperties.DefaultWindowHeight;
                    window.Width = AppProperties.DefaultWindowWidth;
                }

                if (AppProperties.DefaultWindowLocationX == 0 || AppProperties.DefaultWindowLocationY == 0 || !hasScreen)
                {
                    window.StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    window.Location = new Point(AppProperties.DefaultWindowLocationX, AppProperties.DefaultWindowLocationY);
                }
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
        }

        public void SetDisplayLocation(Form window)
        {
            AppProperties.DefaultWindowWidth = window.Width;
            AppProperties.DefaultWindowHeight = window.Height;
            AppProperties.DefaultWindowLocationX = window.Location.X;
            AppProperties.DefaultWindowLocationY = window.Location.Y;
            AppProperties.DefaultWindowState = window.WindowState.ToString();
            AppProperties.DefaultScreenName = Screen.FromControl(window).GetHashCode().ToString();

            AppProperties.SaveProperties();
        }

        public void SwitchPage(String page, Object parameter)
        {
            if (IsLock) return;

            if (!Pages.ContainsKey(page)) return;

            if (IsFullScreen)
            {
                //exit current page's controls fullscreen status
                foreach (var blockPanel in PageActivated.Layout.BlockPanels)
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

            Activate(Pages[page]);

            if (IsFullScreen)
            {
                //activate current page's controls fullscreen status
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

            if (OnSwitchPage != null)
                OnSwitchPage(this, new EventArgs<String, Object>(page, parameter));
        }

        protected virtual void SignOutToolStripMenuItemClick(Object sender, EventArgs e)
        {
            DialogResult result = TopMostMessageBox.Show(Localization["Application_ConfirmSignOut"], Localization["MessageBox_Confirm"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AppProperties.RemoveProperties(Credential);

                Logout();
            }
        }

        protected virtual void AboutToolStripMenuItemClick(Object sender, EventArgs e)
        {
            var aboutForm = new AboutForm
            {
                Icon = Form.Icon,
                Text = Localization["Menu_About"]
            };

            aboutForm.UpdateInfo(Localization["Application_Ver"].Replace("%1", Version));
            aboutForm.UpdateDevicePackInfo(DevicePackVersion != null
                                               ? Localization["Application_DevicePackVer"].Replace("%1", DevicePackVersion)
                                               : String.Empty);

            aboutForm.Show();
        }

        protected virtual void LockApplication()
        {

            ((ApplicationForm)Form).PreviousWindowState = Form.WindowState;
            ((ApplicationForm)Form).PreviousWindowSize = Form.Size;

            KeyboardHook();
            HideToolbar();
            DisableTaskManager();

            Form.Text = "";
            Form.ControlBox = false;
            Form.FormBorderStyle = FormBorderStyle.None;
            Form.WindowState = FormWindowState.Normal;
            Form.WindowState = FormWindowState.Maximized;
            Form.TopMost = true;
            Form.BringToFront();

            SwitchPagePanel.Enabled = false;
            MainPanel.Enabled = false;
            SignOut.ShortcutKeys = Keys.None;
            LockApp.IsSelected = true;

            foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
            {
                if (dropDownItem == LockApp) continue;

                dropDownItem.Visible = false;
            }

            if (OnLockApplication != null)
                OnLockApplication(this, new EventArgs<Boolean>(true));
        }

        protected virtual void UnlockApplication()
        {
            ReleaseKeyboardHook();
            ShowToolbar();
            EnableTaskManager();

            foreach (ToolStripMenuItem dropDownItem in ApplicationMenu.DropDownItems)
            {
                dropDownItem.Visible = true;
            }

            Form.ControlBox = true;
            Form.FormBorderStyle = FormBorderStyle.Sizable;
            Form.WindowState = ((ApplicationForm)Form).PreviousWindowState;
            Form.Size = ((ApplicationForm)Form).PreviousWindowSize;
            SignOut.ShortcutKeys = Keys.Control | Keys.W;
            Form.TopMost = false;
            Form.BringToFront();
            Form.Activate();

            SwitchPagePanel.Enabled = true;
            MainPanel.Enabled = true;
            LockApp.IsSelected = false;

            RefreshMenuStripIconStyle();

            if (IdleTimer > 0)
                IdleTimer = 0;
            if (OnLockApplication != null)
                OnLockApplication(this, new EventArgs<Boolean>(false));
        }

        protected virtual void SetupMenuToolStripMenuItemClick(Object sender, EventArgs e)
        {
            UpdateClientSetting(RestoreClientColumn.PageName, "Setup", null);

            SwitchPage("Setup", null);
        }

        protected virtual void UnlockAppFormOnCancel(Object sender, EventArgs e)
        {
            // Add By Tulip for DVR Mode
            Form.TopMost = true;

            Form.Enabled = true;
            Form.BringToFront();
            Form.Activate();
        }
        //---------------------------------------------------------------------------------------------
        protected Boolean IsDragStart;
        protected readonly List<IDrop> DragDropControls = new List<IDrop>();
        protected Object DragObj;
        public void DragStart(Object sender, EventArgs<Object> e)
        {
            if (IsDragStart) return;

            DragDropControls.Clear();

            PageActivated.CheckDragDataType(DragDropControls, e.Value);

            if (DragDropControls.Count <= 0) return;

            DragObj = e.Value;
            IsDragStart = true;
            if (sender is IDrag && ((IDrag)sender).DragDropProxy != null)
            {
                ((IDrag)sender).DragDropProxy.Visible = true;
                ((IDrag)sender).DragDropProxy.BringToFront();
            }
            if (!WorkPanel.Capture)
                WorkPanel.Capture = true;
        }

        protected void WorkPanelMouseUp(Object sender, MouseEventArgs e)
        {
            if (!IsDragStart) return;

            Point point = Cursor.Position;
            foreach (IDrop iDrop in DragDropControls)
                iDrop.DragStop(point, new EventArgs<Object>(DragObj));
            IsDragStart = false;
            DragDropControls.Clear();

            if (IdleTimer > 0)
                IdleTimer = 0;
        }

        protected void WorkPanelMouseMove(Object sender, MouseEventArgs e)
        {
            //this is useless! (deray) GlobatMouseMoveHandler will catch mouse event
            //if (IdleTimer > 0)
            //    IdleTimer = 0;

            if (!IsDragStart) return;

            foreach (IDrop control in DragDropControls)
                control.DragMove(e);
        }

        private Point _cursorPoint;
        public virtual void GlobatMouseMoveHandler()
        {
            var point = Cursor.Position;

            if (point == _cursorPoint)
            {
                return;
            }
            else
                _cursorPoint = point;

            if (IdleTimer > 0)
                IdleTimer = 0;

            if (PageActivated == null) return;

            foreach (IMouseHandler control in PageActivated.MouseHandler)
                control.GlobalMouseHandler();
        }

        public void WindowFocusGet()
        {
            if (PageActivated == null) return;
            foreach (IFocus focus in PageActivated.FocusList)
                focus.WindowFocusGet();
        }

        public void WindowFocusLost()
        {
            if (PageActivated == null) return;
            foreach (IFocus focus in PageActivated.FocusList)
                focus.WindowFocusLost();
        }

        public virtual void KeyPress(Keys keyData)
        {
            if (PageActivated == null) return;
            foreach (IKeyPress focus in PageActivated.KeyPressList)
                focus.KeyboardPress(keyData);

            if (IsLock && IsFullScreen)
                IsLock = false;
        }

        protected static readonly Image CollapseImage = Resources.GetResources(Properties.Resources.collapse, Properties.Resources.IMGCollapse);
        protected static readonly Image ExpandImage = Resources.GetResources(Properties.Resources.expand, Properties.Resources.IMGExpand);

        public virtual void UpdateClientSetting(RestoreClientColumn keyColumn, String value1, List<Int16> value2)
        {

        }
    }
}
