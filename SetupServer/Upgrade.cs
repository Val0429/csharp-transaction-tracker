using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class UpgradeControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;
        public Stream Stream;
        public String FileName;
        public readonly OpenFileDialog OpenSettingDialog;

        public UpgradeControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"SetupServer_Browse", "Browse"},
                {"SetupServer_SettingFile", "Setting File"},
                {"SetupServer_NoSelectedFile", "No selected file"}
            };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Upgrade";

            BackgroundImage = Manager.BackgroundNoBorder;

            OpenSettingDialog = new OpenFileDialog
            {
                Filter = Localization["SetupServer_SettingFile"] + @" (.img)|*.img"
            };

            filePanel.Paint += FilePanelPaint;
            browserButton.Text = Localization["SetupServer_Browse"];
        }

        private void FilePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, filePanel);
            if (Width <= 100) return;

            String title = Localization["SetupServer_SettingFile"];

            title = title + " : ";

            if (Stream == null)
            {
                title = title + Localization["SetupServer_NoSelectedFile"];
                Manager.PaintText(g, title, Brushes.Black);
            }
            else
            {
                title = title + FileName;

                Manager.PaintText(g, title, Manager.SelectedTextColor);
            }
        }

        private void BrowserButtonClick(object sender, EventArgs e)
        {
            if (OpenSettingDialog.ShowDialog() != DialogResult.OK) return;

            Stream = OpenSettingDialog.OpenFile();
            var fileInfo = new FileInfo(OpenSettingDialog.FileName);
            FileName = fileInfo.Name;

            filePanel.Invalidate();
        }
    }
}
