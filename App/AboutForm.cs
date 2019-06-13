using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace App
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            if (File.Exists(Application.StartupPath + "\\images\\logo.png"))
            {
                Image newLogo = Image.FromFile(Application.StartupPath + "\\images\\logo.png");
                if (newLogo != null)
                {
                    logo.BackgroundImage = newLogo;
                }
            }

            MinimumSize = MaximumSize = Size;
        }

        public void UpdateInfo(String versionText)
        {
            versionLabel.Text = versionText;
        }

        public void UpdateDevicePackInfo(String versionText)
        {
            devicePackVersionLabel.Text = versionText;
        }
    }
}
