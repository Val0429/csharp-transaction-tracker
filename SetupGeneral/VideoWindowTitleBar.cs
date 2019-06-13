using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public sealed partial class VideoWindowTitleBar : UserControl
    {
        public IServer Server;
        public IApp App;
        private readonly Queue<InformationPanel> _recycleInformation = new Queue<InformationPanel>();

        private readonly List<VideoWindowTitleBarInformation> _informations = new List<VideoWindowTitleBarInformation>
                                                                                  {
                                                                                      //VideoWindowTitleBarInformation.ChannelNumber,
                                                                                      VideoWindowTitleBarInformation.CameraName,
                                                                                      VideoWindowTitleBarInformation.Compression,
                                                                                      VideoWindowTitleBarInformation.Resolution,
                                                                                      VideoWindowTitleBarInformation.Bitrate,
                                                                                      VideoWindowTitleBarInformation.FPS,
                                                                                      VideoWindowTitleBarInformation.DateTime
    };

        public Dictionary<String, String> Localization;
        public VideoWindowTitleBar()
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
            Name = "VideoWindowTitleBar";
            BackgroundImage = Manager.BackgroundNoBorder;

            noteLabel.Text = Localization["VideoWindowTitleBarPanel_DragNote"];
            infoLabel.Text = Localization["VideoWindowTitleBarPanel_DisplayNote"];
            previewLabel.Text = Localization["VideoWindowTitleBarPanel_Preview"];
        }

        public void Initialize()
        {
            fontFamilyPanel.Visible = fontSizePanel.Visible = false;

            fontFamilyPanel.Paint += InputPanelPaint;
            fontSizePanel.Paint += InputPanelPaint;
            fontColorPanel.Paint += InputPanelPaint;
            backgroundColorPanel.Paint += InputPanelPaint;

            containerPanel.MouseUp += InformationControlMouseUp;

            fontFamilyComboBox.SelectedIndexChanged += FontFamilyComboBoxSelectedIndexChanged;
            fontSizeComboBox.SelectedIndexChanged += FontSizeComboBoxSelectedIndexChanged;
            fontcolorButton.Click += FontColorButtonClick;
            backgroundColorButton.Click += BackgroundColorButtonClick;
            foreach (FontFamily font in FontFamily.Families)
            {
                fontFamilyComboBox.Items.Add(font.Name);
            }

            previewLabel.Visible = previewPanel.Visible =
            fontColorPanel.Visible =
            backgroundColorPanel.Visible = !(Server is ICMS);
        }

        private void FontFamilyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Configure.VideoTitleBarFontFamify = fontFamilyComboBox.SelectedItem.ToString();
            ChangePreviewBar();
        }

        private void FontSizeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Configure.VideoTitleBarFontSize = Convert.ToUInt16(fontSizeComboBox.SelectedItem);
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
                Server.Configure.VideoTitleBarFontColor = "#" + ColorToHexString(fontColorDialog.Color);
                ChangePreviewBar();
            }
        }

        private void BackgroundColorButtonClick(object sender, EventArgs e)
        {
            fontColorDialog.AllowFullOpen = true;
            fontColorDialog.AnyColor = true;
            fontColorDialog.SolidColorOnly = false;
            fontColorDialog.Color = Color.White;

            if (backgroundDialog.ShowDialog() == DialogResult.OK)
            {
                backgroundColorButton.BackColor = backgroundDialog.Color;
                Server.Configure.VideoTitleBarBackgroundColor = "#" + ColorToHexString(backgroundDialog.Color);
                ChangePreviewBar();
            }
        }

        private void ChangePreviewBar()
        {
            var format = new List<String> { "   " };

            foreach (VideoWindowTitleBarInformation information in Server.Configure.VideoWindowTitleBarInformations)
            {
                switch (information)
                {
                    case VideoWindowTitleBarInformation.CameraName:
                        format.Add("PTZ Camera");
                        break;

                    case VideoWindowTitleBarInformation.Compression:
                        format.Add("H.264");
                        break;

                    case VideoWindowTitleBarInformation.Resolution:
                        format.Add("640 x 480");
                        break;

                    case VideoWindowTitleBarInformation.Bitrate:
                        format.Add("512kbps");
                        break;

                    case VideoWindowTitleBarInformation.FPS:
                        format.Add("FPS:1");
                        break;

                    case VideoWindowTitleBarInformation.DateTime:
                        format.Add("2014-11-12 00:00:00");// %%ms
                        break;
                }
            }

            previewBar.Text = String.Join("   ", format.ToArray());
            previewBar.ForeColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarFontColor);
            previewBar.BackColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarBackgroundColor);
            previewBar.Font = new Font(Server.Configure.VideoTitleBarFontFamify, Convert.ToUInt16(Server.Configure.VideoTitleBarFontSize), FontStyle.Bold);
            //previewBar.Height = (int) (Convert.ToUInt16(Server.Configure.VideoTitleBarFontSize)*1.8);

            fontcolorButton.Text = Server.Configure.VideoTitleBarFontColor;
            backgroundColorButton.Text = Server.Configure.VideoTitleBarBackgroundColor;
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

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("VideoWindowTitleBarPanel_" + control.Tag))
                Manager.PaintText(g, Localization["VideoWindowTitleBarPanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        public void GeneratorInformationList()
        {
            fontFamilyComboBox.SelectedIndexChanged -= FontFamilyComboBoxSelectedIndexChanged;
            fontFamilyComboBox.SelectedItem = Server.Configure.VideoTitleBarFontFamify;
            fontFamilyComboBox.SelectedIndexChanged += FontFamilyComboBoxSelectedIndexChanged;

            fontSizeComboBox.SelectedIndexChanged -= FontSizeComboBoxSelectedIndexChanged;
            fontSizeComboBox.SelectedItem = Server.Configure.VideoTitleBarFontSize.ToString();
            fontSizeComboBox.SelectedIndexChanged += FontSizeComboBoxSelectedIndexChanged;

            fontcolorButton.BackColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarFontColor);
            backgroundColorButton.BackColor = ColorTranslator.FromHtml(Server.Configure.VideoTitleBarBackgroundColor);

            containerPanel.Visible = false;
            ClearViewModel();
            var seq = 1;
            foreach (VideoWindowTitleBarInformation information in Server.Configure.VideoWindowTitleBarInformations)
            {
                InformationPanel infoPanel = GetInformationPanel();
                infoPanel.Order = (ushort) seq;
                infoPanel.Tag = information;
                containerPanel.Controls.Add(infoPanel);
                infoPanel.BringToFront();
                infoPanel.Enabled = true;
                infoPanel.InUse = true;
                seq++;
            }

            foreach (VideoWindowTitleBarInformation information in _informations)
            {
                if(Server.Configure.VideoWindowTitleBarInformations.Contains(information)) continue;
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
            ChangePreviewBar();
        }

        private void InformationPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (!(sender is InformationPanel) || ((InformationPanel)sender).Tag == null) return;
            
            var informationPanel = sender as InformationPanel;

            var information = (VideoWindowTitleBarInformation)informationPanel.Tag;

            var inUse = false;
            foreach (VideoWindowTitleBarInformation info in Server.Configure.VideoWindowTitleBarInformations)
            {
                if (info == information)
                {
                    inUse = true;
                    break;
                }
            }

            informationPanel.InUse =inUse != true;

             Server.Configure.VideoWindowTitleBarInformations.Clear();

            var temp = new List<InformationPanel>();
            foreach (InformationPanel panel in containerPanel.Controls)
            {
                if (panel.InUse)
                {
                    temp.Add(panel);
                }
                else
                {
                    panel.Order = 0;
                }
            }
            temp.Reverse();

            UInt16 order = 1;
            foreach (InformationPanel panel in temp)
            {
                if (panel.Tag is VideoWindowTitleBarInformation)
                    Server.Configure.VideoWindowTitleBarInformations.Add((VideoWindowTitleBarInformation)panel.Tag);
                panel.Order = order;
                order++;
            }

            Invalidate();
        }

        private InformationPanel GetInformationPanel()
        {
            if (_recycleInformation.Count > 0)
            {
                return _recycleInformation.Dequeue();
            }

            var informationPanel = new InformationPanel
            {
                Server = Server,
            };
            informationPanel.MouseClick += InformationPanelMouseClick;
            informationPanel.OnInformationDrag += InformationPanelOnStorageDrag;

            return informationPanel;
        }

        private InformationPanel _dragInformationPanel;
        private void InformationPanelOnStorageDrag(Object sender, EventArgs e)
        {
            containerPanel.Cursor = Cursors.NoMoveVert;
            containerPanel.Capture = true;
            _dragInformationPanel = (InformationPanel) sender;
        }

        private void InformationControlMouseUp(Object sender, MouseEventArgs e)
        {
            containerPanel.Cursor = Cursors.Default;

            if(_dragInformationPanel == null) return;

            InformationPanel dragToInformationPanel = null;

            foreach (InformationPanel storagePanel in containerPanel.Controls)
            {
                if (storagePanel != _dragInformationPanel && storagePanel.InUse && storagePanel.Tag != null)
                {
                    if (Drag.IsDrop(storagePanel))
                    {
                        dragToInformationPanel = storagePanel;
                        break;
                    }
                }
            }

            if (dragToInformationPanel == null) return;

            var tag = _dragInformationPanel.Tag;
            _dragInformationPanel.Tag = dragToInformationPanel.Tag;
            dragToInformationPanel.Tag = tag;

            var info = _dragInformationPanel.DiskInfo;
            _dragInformationPanel.DiskInfo = dragToInformationPanel.DiskInfo;
            dragToInformationPanel.DiskInfo = info;

            Server.Configure.VideoWindowTitleBarInformations.Clear();

            var temp = new List<InformationPanel>();
            foreach (InformationPanel panel in containerPanel.Controls)
            {
                if (!(panel.Tag is VideoWindowTitleBarInformation)) continue;
                if (panel.InUse)
                {
                    temp.Add(panel);
                }
                else
                {
                    panel.Order = 0;
                }
            }
            temp.Reverse();

            UInt16 order = 1;
            foreach (InformationPanel informationPanel in temp)
            {
                informationPanel.Order = order;
                Server.Configure.VideoWindowTitleBarInformations.Add((VideoWindowTitleBarInformation) informationPanel.Tag);
                order++;
            }

            _dragInformationPanel = null;
            Invalidate();
            ChangePreviewBar();
        }
        
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
        }
    }

    public sealed class InformationPanel : Panel
    {
        public UInt16 Order;
        public IServer Server;
        public DiskInfo DiskInfo;
        public event EventHandler OnInformationDrag;
        public Dictionary<String, String> Localization;

        private Boolean _inUse;
        public Boolean InUse
        {
            get
            {
                return _inUse;
            }
            set
            {
                _inUse = value;
            }
        }

        public InformationPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"VideoWindowTitleBarPanel_Sequence", "Sequence"},
                                   {"VideoWindowTitleBarPanel_Information", "Information"},
                                   {"VideoWindowTitleBarPanel_ChannelNumber", "Channel number"},
                                   {"VideoWindowTitleBarPanel_CameraName", "Camera name"},
                                   {"VideoWindowTitleBarPanel_Compression", "Compression"},
                                   {"VideoWindowTitleBarPanel_Resolution", "Resolution"},
                                   {"VideoWindowTitleBarPanel_FPS", "FPS"},
                                   {"VideoWindowTitleBarPanel_Bitrate", "Bitrate"},
                                   {"VideoWindowTitleBarPanel_DateTime", "Date & Time"}
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            MouseDown += InformationPanelMouseDown;
            MouseUp += StoragePanelMouseUp;
            Paint += InformationPanelPaint;
        }

        private void StoragePanelMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= InformationPanelMouseMove;
        }

        private Int32 _positionX;
        private void InformationPanelMouseDown(Object sender, MouseEventArgs e)
        {
            if (!_inUse) return;

            _positionX = e.X;
            MouseMove -= InformationPanelMouseMove;
            MouseMove += InformationPanelMouseMove;
        }

        private void InformationPanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _positionX) return;
            if (OnInformationDrag != null)
            {
                MouseMove -= InformationPanelMouseMove;
                OnInformationDrag(this, e);
            }
        }

        public Brush SelectedColor = Manager.SelectedTextColor;
        private void InformationPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Server == null) return;
            
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Width <= 100) return;

            if(Tag == null)
            {
                Manager.PaintTitleTopInput(g, this);
                Manager.PaintText(g, Localization["VideoWindowTitleBarPanel_Sequence"], Manager.TitleTextColor);
                
                if (Width <= 210) return;
                g.DrawString(Localization["VideoWindowTitleBarPanel_Information"], Manager.Font, Manager.TitleTextColor, 130, 13);

                return;
            }

            Manager.Paint(g, this);
            Brush brush = Brushes.Black;

            if (_inUse)
            {
                brush = SelectedColor;
                Manager.PaintSelected(g);
            }
            if (!Enabled)
                brush = Brushes.Gray;

            if (Order != 0)
                Manager.PaintText(g, Order.ToString(), brush);

            if (Width <= 210) return;
            g.DrawString(Localization["VideoWindowTitleBarPanel_" + Tag], Manager.Font, brush, 130, 13);

        }
    }
}
