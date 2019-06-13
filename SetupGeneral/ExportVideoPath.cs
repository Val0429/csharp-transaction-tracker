using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupGeneral
{
    public partial class ExportVideoPath : UserControl
    {
        public IServer Server { get; set; }
        public IApp App { get; set; }
        public Dictionary<String, String> Localization;

        public ExportVideoPath()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Custom", "Custom"},

                                   {"SetupGeneral_Desktop", "Desktop"},
                                   {"SetupGeneral_Document", "My Documents"},
                                   {"SetupGeneral_Picture", "My Pictures"},
                                   {"SetupGeneral_SelectFilePath", "Select path"},
                                   {"SetupGeneral_ExportFolderName", "Export video folder name is \"%1 Video (%2)\""},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "ExportVideoPath";
            BackgroundImage = Manager.BackgroundNoBorder;
            browserButton.BackgroundImage = Resources.GetResources(Properties.Resources.SelectFolder, Properties.Resources.IMGSelectFolder);

            customPanel.Paint += ExportVideoPathInputPanelPaint;
            SharedToolTips.SharedToolTip.SetToolTip(browserButton, Localization["SetupGeneral_SelectFilePath"]);
        }

        public void ParseSetting()
        {
            exportVideoPathTextBox.TextChanged -= ExportVideoTextBoxTextChanged;

            switch (Server.Configure.ExportVideoPath)
            {
                case "Desktop":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Desktop"];
                    break;

                case "Document":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Document"];
                    break;

                case "Picture":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Picture"];
                    break;

                default:
                    exportVideoPathTextBox.Text = Server.Configure.ExportVideoPath;
                    break;
            }

            exportVideoPathTextBox.TextChanged += ExportVideoTextBoxTextChanged;
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
            var folderType = GetFoldType();

            folderNameLabel.Text = Localization["SetupGeneral_ExportFolderName"].Replace("%1", folderType)
                .Replace("%2", Server.Credential.Domain);

            String[] paths = new[] { "Picture", "Document", "Desktop" };

            foreach (String path in paths)
            {
                var pathPanel = new ExportPathPanel
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
            Server.Configure.ExportVideoPath = ((Control)sender).Tag.ToString();

            exportVideoPathTextBox.TextChanged -= ExportVideoTextBoxTextChanged;

            switch (Server.Configure.ExportVideoPath)
            {
                case "Desktop":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Desktop"];
                    break;

                case "Document":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Document"];
                    break;

                case "Picture":
                    exportVideoPathTextBox.Text = Localization["SetupGeneral_Picture"];
                    break;
            }

            exportVideoPathTextBox.TextChanged += ExportVideoTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void ExportVideoPathInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;
            var isCustom = true;
            switch (Server.Configure.ExportVideoPath)
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

        private void ExportVideoTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.ExportVideoPath = exportVideoPathTextBox.Text;

            Invalidate();
        }

        private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private void BrowserButtonClick(Object sender, EventArgs e)
        {
            switch (Server.Configure.ExportVideoPath)
            {
                case "Desktop":
                    exportVideoFolderBrowserDialog.SelectedPath = _desktopPath;
                    break;

                case "Document":
                    exportVideoFolderBrowserDialog.SelectedPath = _documentPath;
                    break;

                case "Picture":
                    exportVideoFolderBrowserDialog.SelectedPath = _picturePath;
                    break;

                default:
                    exportVideoFolderBrowserDialog.SelectedPath = Server.Configure.ExportVideoPath;
                    break;

            }
            var resault = exportVideoFolderBrowserDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                Server.Configure.ExportVideoPath = exportVideoPathTextBox.Text = exportVideoFolderBrowserDialog.SelectedPath;
            }
        }

        public sealed class ExportPathPanel : Panel
        {
            public IServer Server;
            public Dictionary<String, String> Localization;

            public ExportPathPanel()
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
                if (Tag.ToString() == Server.Configure.ExportVideoPath)
                {
                    Manager.PaintText(g, Localization["SetupGeneral_" + Tag], Manager.SelectedTextColor);
                    Manager.PaintSelected(g);
                }
                else
                    Manager.PaintText(g, Localization["SetupGeneral_" + Tag]);
            }
        }
    }
}
