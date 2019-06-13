using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupGeneral
{
    public partial class SaveImagePath : UserControl
    {
        public IServer Server;
        public IApp App;
        public Dictionary<String, String> Localization;

        public SaveImagePath()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Custom", "Custom"},

                                   {"SetupGeneral_Desktop", "Desktop"},
                                   {"SetupGeneral_Document", "My Documents"},
                                   {"SetupGeneral_Picture", "My Pictures"},
                                   {"SetupGeneral_SelectFilePath", "Select path"},
                                   {"SetupGeneral_SaveFolderName", "Save image folder name is \"%1 Image (%2)\""},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "SaveImagePath";
            BackgroundImage = Manager.BackgroundNoBorder;
            browserButton.BackgroundImage = Resources.GetResources(Properties.Resources.SelectFolder, Properties.Resources.IMGSelectFolder);

            customPanel.Paint += SaveImagePathInputPanelPaint;
            SharedToolTips.SharedToolTip.SetToolTip(browserButton, Localization["SetupGeneral_SelectFilePath"]);
        }

        public void ParseSetting()
        {
            saveImagePathTextBox.TextChanged -= SaveImageTextBoxTextChanged;

            switch (Server.Configure.SaveImagePath)
            {
                case "Desktop":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Desktop"];
                    break;

                case "Document":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Document"];
                    break;

                case "Picture":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Picture"];
                    break;

                default:
                    saveImagePathTextBox.Text = Server.Configure.SaveImagePath;
                    break;
            }

            saveImagePathTextBox.TextChanged += SaveImageTextBoxTextChanged;
        }

        protected virtual string GetFoldType()
        {
            if (Server is ICMS)
            {
                return "CMS";
            }

            var pts = Server as IPTS;
            if (pts != null)
            {
                switch (pts.ReleaseBrand)
                {
                    case "Salient":
                        return "TransactionTracker";
                    default:
                        return "PTS";
                }
            }

            return "NVR";
        }

        public void Initialize()
        {
            String folderType = GetFoldType();

            folderNameLabel.Text = Localization["SetupGeneral_SaveFolderName"].Replace("%1", folderType)
                .Replace("%2", Server.Credential.Domain);

            String[] paths = new[] { "Picture", "Document", "Desktop" };

            foreach (String path in paths)
            {
                var pathPanel = new ImagePathPanel
                {
                    Server = Server,
                    Tag = path,
                };
                pathPanel.MouseClick += PathPanelMouseClick;

                containerPanel.Controls.Add(pathPanel);
            }
        }

        private void PathPanelMouseClick(Object sender, MouseEventArgs e)
        {
            Server.Configure.SaveImagePath = ((Control)sender).Tag.ToString();

            saveImagePathTextBox.TextChanged -= SaveImageTextBoxTextChanged;

            switch (Server.Configure.SaveImagePath)
            {
                case "Desktop":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Desktop"];
                    break;

                case "Document":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Document"];
                    break;

                case "Picture":
                    saveImagePathTextBox.Text = Localization["SetupGeneral_Picture"];
                    break;
            }

            saveImagePathTextBox.TextChanged += SaveImageTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void SaveImagePathInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (Width <= 100) return;
            var isCustom = true;
            switch (Server.Configure.SaveImagePath)
            {
                case "Desktop":
                case "Document":
                case "Picture":
                    isCustom = false;
                    break;
            }

            if (isCustom)
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"], Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Localization["SetupGeneral_Custom"]);
        }

        private void SaveImageTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.SaveImagePath = saveImagePathTextBox.Text;
            
            Invalidate();
        }

        private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private void BrowserButtonClick(Object sender, EventArgs e)
        {
            switch (Server.Configure.SaveImagePath)
            {
                case "Desktop":
                    saveImageFolderBrowserDialog.SelectedPath = _desktopPath;
                    break;

                case "Document":
                    saveImageFolderBrowserDialog.SelectedPath = _documentPath;
                    break;

                case "Picture":
                    saveImageFolderBrowserDialog.SelectedPath = _picturePath;
                    break;

                default:
                    saveImageFolderBrowserDialog.SelectedPath = Server.Configure.SaveImagePath;
                    break;

            }
            var resault = saveImageFolderBrowserDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                Server.Configure.SaveImagePath = saveImagePathTextBox.Text = saveImageFolderBrowserDialog.SelectedPath;
            }
        }
    }

    public sealed class ImagePathPanel : Panel
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public ImagePathPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Desktop", "Desktop"},
                                   {"SetupGeneral_Document", "My Documents"},
                                   {"SetupGeneral_Picture", "My Pictures"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += PathPanelPaint;
        }

        private void PathPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Parent.Controls[0] == this)
            {
                Manager.PaintBottom(g, this);
            }
            else
            {
                Manager.PaintMiddle(g, this);
            }

            if (Width <= 100) return;
            if (Tag.ToString() == Server.Configure.SaveImagePath)
            {
                Manager.PaintText(g, Localization["SetupGeneral_" + Tag], Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Localization["SetupGeneral_" + Tag]);
        }
    }
}
