using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupJoystick
{
    public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        public String TitleName { get; set; }

        public Setup()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Joystick", "Joystick"},

								   {"SetupJoystick_EnableJoystick", "Enable Joystick"},
								   {"SetupJoystick_Enabled", "Enabled"},
                                   {"SetupJoystick_EnableAxisJoystick", "Select to enable Axis Joystick"},
                                   {"SetupJoystick_RestartAlarm", "Please login again to make Joystick changes to take effect."},
                                   {"SetupAxisJoystick_Enabled", "Enabled"},
							   };
            Localizations.Update(Localization);

            Name = "Joystick";
            TitleName = Localization["Control_Joystick"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Joystick"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        public void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            // Enable Joystick
            enableJoystickDoubleBufferPanel.Paint += InputPanelPaint;
            enableJoystickCheckBox.Text = Localization["SetupJoystick_Enabled"];
            enableJoystickCheckBox.Checked = Server.Configure.EnableJoystick;
            enableJoystickCheckBox.CheckedChanged += EnableJoystickCheckBoxCheckedChanged;

            // Enable Axis Joystick
            enableAxisJoystickDoubleBufferPanel.Paint += InputAxisPanelPaint;
            enableAxisJoystickCheckBox.Text = Localization["SetupAxisJoystick_Enabled"];
            enableAxisJoystickCheckBox.Checked = Server.Configure.EnableAxisJoystick;
            enableAxisJoystickCheckBox.CheckedChanged += EnableAxisJoystickCheckBoxCheckedChanged;

            Server.OnLoadComplete += ServerOnLoadComplete;
            Server.OnSaveComplete += ServerOnSaveComplete;

            // Remember Status
            _lastAxisJoystickEnableStatus = GetJoystickStatus();
        }

        private int _lastAxisJoystickEnableStatus;
        private void ServerOnSaveComplete(Object sender, EventArgs<string> e)
        {
            var currentStatus = GetJoystickStatus();
            if (currentStatus != _lastAxisJoystickEnableStatus)
            {
                TopMostMessageBox.Show(Localization["SetupJoystick_RestartAlarm"]);
            }

            _lastAxisJoystickEnableStatus = currentStatus;
        }

        private int GetJoystickStatus()
        {
            if (Server.Configure.EnableJoystick)
                return 1;
            else if (Server.Configure.EnableAxisJoystick)
                return 2;
            else
                return 0;
        }

        private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);
        private void ServerOnLoadComplete(object sender, EventArgs<string> e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new RefreshContentDelegate(ServerOnLoadComplete), sender, e);
                    return;
                }
            }
            catch (Exception)
            {
            }

            enableJoystickCheckBox.Checked = Server.Configure.EnableJoystick;
            enableJoystickDoubleBufferPanel.Invalidate();
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.PaintTop(g, control);

            if (Localization.ContainsKey("SetupJoystick_" + control.Tag))
                Manager.PaintText(g, Localization["SetupJoystick_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void InputAxisPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.PaintBottom(g, control);

            if (Localization.ContainsKey("SetupJoystick_" + control.Tag))
                Manager.PaintText(g, Localization["SetupJoystick_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void EnableJoystickCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            bool @checked = enableJoystickCheckBox.Checked;
            // Cannot enable both
            if (enableAxisJoystickCheckBox.Checked)
            {
                enableAxisJoystickCheckBox.Checked = false;
                Server.Configure.EnableAxisJoystick = false;
            }

            // Enable this
            Server.Configure.EnableJoystick = enableJoystickCheckBox.Checked = @checked;
        }

        private void EnableAxisJoystickCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            bool @checked = enableAxisJoystickCheckBox.Checked;
            // Cannot enable both
            if (enableJoystickCheckBox.Checked)
            {
                enableJoystickCheckBox.Checked = false;
                Server.Configure.EnableJoystick = false;
            }

            // Enable this
            Server.Configure.EnableAxisJoystick = enableAxisJoystickCheckBox.Checked = @checked;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            //else //dont hide self to keep at last selection panel on screen
            //    Minimize();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }
}
