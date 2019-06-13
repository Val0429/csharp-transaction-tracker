using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public sealed partial class PowerControl : UserControl
    {
        public IServer Server;
        public IApp App;
        public Dictionary<String, String> Localization;
        public PowerControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"SetupServer_Reboot", "Reboot"},
                {"SetupServer_Shutdown", "Shutdown"},
                {"MessageBox_Confirm", "Confirm"},
                {"SetupServer_ConfirmReboot","Are you sure you want to reboot?"},
                {"SetupServer_ConfirmShutdown","Are you sure you want to shutdown?"}
            };
            Localizations.Update(Localization);
            InitializeComponent();

            rebootPanel.Paint += InputPanelPaint;
            shutdownPanel.Paint += InputPanelPaint;
  
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Power";

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;
            Manager.PaintSingleInput(g, control);

            if (Localization.ContainsKey("SetupServer_" + control.Tag))
                Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void RebootPanelClick(object sender, EventArgs e)
        {
            DialogResult msgResult = TopMostMessageBox.Show(Localization["SetupServer_ConfirmReboot"], Localization["MessageBox_Confirm"],
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (msgResult == DialogResult.Yes)
            {
                Server.Server.Reboot();
                App.Quit();
                Application.Exit();
            }
        }

        private void ShutdownPanelClick(object sender, EventArgs e)
        {
            DialogResult msgResult = TopMostMessageBox.Show(Localization["SetupServer_ConfirmShutdown"], Localization["MessageBox_Confirm"],
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (msgResult == DialogResult.Yes)
            {
                Server.Server.Shutdown();
                App.Quit();
                Application.Exit();
            }
        }
    }
}
