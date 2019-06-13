using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using Interface;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public partial class Watermark : UserControl
    {
        public IServer Server;
        public IApp App;
        public Dictionary<String, String> Localization;
        public Watermark()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"VideoWindowTitleBarPanel_FontFamily", "Font Family"},
                                   {"VideoWindowTitleBarPanel_FontSize", "Font Size"},
                                   {"VideoWindowTitleBarPanel_FontColor", "Font Color"},
                                   {"VideoWindowTitleBarPanel_BackgroundColor", "Background Color"},
                                   {"VideoWindowTitleBarPanel_Preview", "Preview"},
                                   {"VideoWindowTitleBarPanel_DragNote", "Drag to order display."},
                                   {"VideoWindowTitleBarPanel_DisplayNote", "Informations will be displayed from left to right on title bar."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Watermark";
            BackgroundImage = Manager.BackgroundNoBorder;

            //noteLabel.Text = Localization["VideoWindowTitleBarPanel_DragNote"];
            //infoLabel.Text = Localization["VideoWindowTitleBarPanel_DisplayNote"];
            previewLabel.Text = Localization["VideoWindowTitleBarPanel_Preview"];
        }

        public void Initialize()
        {
            //fontFamilyPanel.Visible = fontSizePanel.Visible = false;
            backgroundColorPanel.Visible = false;
            fontFamilyPanel.Paint += InputPanelPaint;
            fontSizePanel.Paint += InputPanelPaint;
            fontColorPanel.Paint += InputPanelPaint;
            fontTextPanel.Paint += InputPanelPaint;
            backgroundColorPanel.Paint += InputPanelPaint;

            //containerPanel.MouseUp += InformationControlMouseUp;

            fontFamilyComboBox.SelectedIndexChanged += FontFamilyComboBoxSelectedIndexChanged;
            fontSizeComboBox.SelectedIndexChanged += FontSizeComboBoxSelectedIndexChanged;
            fontcolorButton.Click += FontColorButtonClick;
            //backgroundColorButton.Click += BackgroundColorButtonClick;
            foreach (FontFamily font in FontFamily.Families)
            {
                fontFamilyComboBox.Items.Add(font.Name);
            }
            /*
            previewLabel.Visible = previewPanel.Visible =
            fontColorPanel.Visible =
            backgroundColorPanel.Visible = !(Server is ICMS);
            */
        }
        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            if (control.Tag.ToString() == "FontColor")
            {
                Manager.PaintTop(g, (Control)sender);
            }
            else
            {
                Manager.Paint(g, (Control)sender);
            }

          //  if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("VideoWindowTitleBarPanel_" + control.Tag))
                Manager.PaintText(g, Localization["VideoWindowTitleBarPanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
        private void FontFamilyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Configure.WatermarkFontFamify = fontFamilyComboBox.SelectedItem.ToString();
            ChangePreviewBar();
        }
        private void FontSizeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Configure.WatermarkFontSize = Convert.ToUInt16(fontSizeComboBox.SelectedItem);
            ChangePreviewBar();
        }
        private void FontColorButtonClick(object sender, EventArgs e)
        {
            fontColorDialog.AllowFullOpen = true;
            fontColorDialog.AnyColor = true;
            fontColorDialog.SolidColorOnly = false;
            fontColorDialog.Color = Color.White;

            if (fontColorDialog.ShowDialog() == DialogResult.OK)
            {
                fontcolorButton.BackColor = fontColorDialog.Color;
                Server.Configure.WatermarkFontColor = "#" + ColorToHexString(fontColorDialog.Color);
                ChangePreviewBar();
            }
        }
        static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public static String ColorToHexString(Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new String(chars);
        }

        private void ChangePreviewBar()
        {
            //var format = new List<String> { "   " };
            //previewBar.Text = String.Join("   ", format.ToArray());
            previewBar.Text = Server.Configure.WatermarkText;
            previewBar.ForeColor = ColorTranslator.FromHtml(Server.Configure.WatermarkFontColor);
           // previewBar.BackColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarBackgroundColor);
            previewBar.Font = new Font(Server.Configure.WatermarkFontFamify, Convert.ToUInt16(Server.Configure.WatermarkFontSize), FontStyle.Bold);
            //previewBar.Height = (int) (Convert.ToUInt16(Server.Configure.VideoTitleBarFontSize)*1.8);

            fontcolorButton.Text = Server.Configure.WatermarkFontColor;
            //backgroundColorButton.Text = Server.Configure.WatermarkBackgroundColor;
        }
        public void GeneratorInformationList()
        {
            fontFamilyComboBox.SelectedIndexChanged -= FontFamilyComboBoxSelectedIndexChanged;
            fontFamilyComboBox.SelectedItem = Server.Configure.WatermarkFontFamify;
            fontFamilyComboBox.SelectedIndexChanged += FontFamilyComboBoxSelectedIndexChanged;

            fontSizeComboBox.SelectedIndexChanged -= FontSizeComboBoxSelectedIndexChanged;
            fontSizeComboBox.SelectedItem = Server.Configure.WatermarkFontSize.ToString();
            fontSizeComboBox.SelectedIndexChanged += FontSizeComboBoxSelectedIndexChanged;
            textBox.Text = Server.Configure.WatermarkText;
            fontcolorButton.BackColor = ColorTranslator.FromHtml(Server.Configure.WatermarkFontColor);
            //backgroundColorButton.BackColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarBackgroundColor);

           // containerPanel.Visible = false;
            //ClearViewModel();
            /*
            var seq = 1;
            foreach (VideoWindowTitleBarInformation information in Server.Configure.VideoWindowTitleBarInformations)
            {
                InformationPanel infoPanel = GetInformationPanel();
                infoPanel.Order = (ushort)seq;
                infoPanel.Tag = information;
                containerPanel.Controls.Add(infoPanel);
                infoPanel.BringToFront();
                infoPanel.Enabled = true;
                infoPanel.InUse = true;
                seq++;
            }

            foreach (VideoWindowTitleBarInformation information in _informations)
            {
                if (Server.Configure.VideoWindowTitleBarInformations.Contains(information)) continue;
                InformationPanel infoPanel = GetInformationPanel();
                infoPanel.Order = 0;
                infoPanel.Tag = information;
                containerPanel.Controls.Add(infoPanel);
                infoPanel.BringToFront();
                infoPanel.Enabled = true;
                infoPanel.InUse = false;
            }

            InformationPanel titlePanel = GetInformationPanel();
            titlePanel.Cursor = Cursors.Default;
            containerPanel.Controls.Add(titlePanel);
            containerPanel.Visible = true;
            containerPanel.SendToBack();
            */
            ChangePreviewBar();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            Server.Configure.WatermarkText = textBox.Text;
            ChangePreviewBar();
        }
        /*
        private void ClearViewModel()
        {
            foreach (InformationPanel informationPanel in containerPanel.Controls)
            {
                informationPanel.Order = 0;
                informationPanel.Tag = null;
                informationPanel.InUse = false;
                informationPanel.DiskInfo = null;
                informationPanel.Cursor = Cursors.Hand;
                if (!_recycleInformation.Contains(informationPanel))
                {
                    _recycleInformation.Enqueue(informationPanel);
                }
            }
            containerPanel.Controls.Clear();
        }*/
    }
}
