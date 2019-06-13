using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupGeneral
{
    public sealed partial class ExportVideoMaxiFileSize : UserControl
    {
        public IServer Server;
        public IApp App;
        public Dictionary<String, String> Localization;

        public ExportVideoMaxiFileSize()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Custom", "Custom"},
                                   {"SetupGeneral_VideoExportMaximumSizeValue", "Maximum file size should between %1 MB to %2 MB (1.8GB)"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "ExportVideo";
            BackgroundImage = Manager.BackgroundNoBorder;
            infoLabel.Text = Localization["SetupGeneral_VideoExportMaximumSizeValue"].Replace("%1", MiniumInterval.ToString()).Replace("%2", MaximumInterval.ToString());
            
            customPanel.Paint += FileSizeInputPanelPaint;

            fileSizeTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
        }

        private const UInt16 MiniumInterval = 100;
        private const UInt16 MaximumInterval = 1843;
        public void ParseSetting()
        {
            fileSizeTextBox.TextChanged -= FileSizeTextBoxTextChanged;

            fileSizeTextBox.Text = Server.Configure.ExportVideoMaxiFileSize.ToString();
            
            fileSizeTextBox.TextChanged += FileSizeTextBoxTextChanged;
        }

        private readonly UInt16[] _sizeArray = new UInt16[] { 100, 200, 300, 500, 800, 1024, 1228, 1536, 1843};
        public void Initialize()
        {
            foreach (UInt16 size in _sizeArray.Reverse())
            {
                SizePanel sizePanel = new SizePanel
                {
                    Server = Server,
                    Tag = size.ToString(),
                };

                sizePanel.MouseClick += SizePanelMouseClick;

                containerPanel.Controls.Add(sizePanel);
            }
        }

        private void SizePanelMouseClick(Object sender, MouseEventArgs e)
        {
            Server.Configure.ExportVideoMaxiFileSize = Convert.ToUInt16(((Control)sender).Tag);

            fileSizeTextBox.TextChanged -= FileSizeTextBoxTextChanged;

            fileSizeTextBox.Text = Server.Configure.ExportVideoMaxiFileSize.ToString();

            fileSizeTextBox.TextChanged += FileSizeTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void FileSizeInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_sizeArray.Contains(Server.Configure.ExportVideoMaxiFileSize))
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"], Manager.SelectedTextColor);

                Manager.PaintTextRight(g, customPanel, "MB", Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"]);

                Manager.PaintTextRight(g, customPanel, "MB");
            }
        }

        private void FileSizeTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 size = (fileSizeTextBox.Text != "") ? Convert.ToUInt32(fileSizeTextBox.Text) : 0;

            Server.Configure.ExportVideoMaxiFileSize = Convert.ToUInt16(Math.Min(Math.Max(size, MiniumInterval), MaximumInterval));

            Invalidate();
        }
    }

    public sealed class SizePanel : Panel
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public SizePanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += PortPanelPaint;
        }

        private String _size;
        private void PortPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            if(_size == null)
            {
                UInt16 size = Convert.ToUInt16(Tag);
                if (size < 1024)
                {
                    _size = String.Format("{0} MB", size);
                }
                else
                {
                    switch (size)
                    {
                        case 1024:
                            _size = "1 GB";
                            break;

                        case 1228:
                            _size = "1.2 GB";
                            break;

                        case 1536:
                            _size = "1.5 GB";
                            break;

                        case 1843:
                            _size = "1.8 GB";
                            break;
                    }
                }
            }

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (Width <= 100) return;
            if (Tag.ToString() == Server.Configure.ExportVideoMaxiFileSize.ToString())
            {
                Manager.PaintText(g, _size, Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, _size);
        }
    }
}
