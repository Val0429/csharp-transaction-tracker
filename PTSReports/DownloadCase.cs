using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace PTSReports
{
    public partial class DownloadCase : UserControl, IControl
    {
        public event EventHandler OnDownloadCase;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public DownloadCase()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DownloadCase_Button", "Export Case"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            SharedToolTips.SharedToolTip.SetToolTip(downloadButton, Localization["DownloadCase_Button"]);
            downloadButton.Image = Resources.GetResources(Properties.Resources.download, Properties.Resources.IMGDownload);

            downloadButton.MouseDown += ButtonMouseDown;
            downloadButton.MouseUp += ButtonMouseUp;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            downloadButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            downloadButton.BackgroundImage = Properties.Resources.button;
        }

        private void DownloadButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDownloadCase != null)
                OnDownloadCase(this, null);
        }
    }
}