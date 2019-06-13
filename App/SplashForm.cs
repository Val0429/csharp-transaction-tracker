using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;

namespace App
{
    public sealed partial class SplashForm : Form
    {
        public Dictionary<String, String> Localization;

        public SplashForm()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SplashForm_Ver", "Version %1"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            TopMost = false;
            versionLabel.Text = "";
            infoLabel.Text = "";

            var loadingImage = Resources.GetResources(Properties.Resources.loadingClock, Properties.Resources.IMGLoadingClock);
            logoPanel.BackgroundImage = Resources.GetResources(Properties.Resources.loadingClock, Properties.Resources.IMGLoadingClock);

            splashLeftPanel.BackgroundImage = Resources.GetResources(Properties.Resources.splashLeft,
                                                                     Properties.Resources.IMGSplashLeft);
            splashRightPanel.BackgroundImage = Resources.GetResources(Properties.Resources.splashRight,
                                                                     Properties.Resources.IMGSplashRight);
            splashCenterPanel.BackgroundImage = Resources.GetResources(Properties.Resources.splashCenter,
                                                                     Properties.Resources.IMGSplashCenter);

            String path = Environment.CurrentDirectory;// Application.StartupPath;
            if (!File.Exists(path + "\\images\\splash.png")) return;

            Image splashLogo = Image.FromFile(path + "\\images\\splash.png");

            logoPanel.BackgroundImage = splashLogo;

            //adjust width
            var newSize = new Size(Width, Height);
            if (splashLogo.Width != loadingImage.Width)
            {
                newSize.Width += (splashLogo.Width - loadingImage.Width);
            }
            //adjust height
            if (splashLogo.Height != loadingImage.Height)
            {
                newSize.Height += (splashLogo.Height - loadingImage.Height);
            }

            MaximumSize = newSize;
            Size = MaximumSize;
        }

        public delegate void SetInfoDelegate(String info);
        public void SetInfo(String info)
        {
            if (infoLabel.InvokeRequired)
            {
                infoLabel.Invoke(new SetInfoDelegate(SetInfo), info);
                return;
            }
            infoLabel.Text = info;
            //infoLabel.Invalidate();
            //infoLabel.Update();
            infoLabel.Refresh();
        }

        public String Version
        {
            set
            {
                versionLabel.Text = Localization["SplashForm_Ver"].Replace("%1", value);
            }
        }
    }
}
