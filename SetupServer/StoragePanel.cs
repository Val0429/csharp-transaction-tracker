using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public sealed class StoragePanel : Panel
    {
        public UInt16 Order { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IServer Server { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DiskInfo DiskInfo { get; set; }


        public event EventHandler OnStorageDrag;
        public event EventHandler OnKeepSpaceChange;
        public event EventHandler OnKeepSpaceLostFocus;
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
                if (_inUse != value)
                {
                    _keepSpaceTextBox.Enabled = _inUse = value;
                    Invalidate();
                }
            }
        }

        private readonly TextBox _keepSpaceTextBox;


        // Constructor
        public StoragePanel()
        {
            Localization = new Dictionary<String, String>
            {
                {"StoragePanel_Sequence", "Sequence"},
                {"StoragePanel_Drive", "Drive"},
                {"StoragePanel_Capacity", "Capacity"},
                {"StoragePanel_Available", "Available"},
                {"StoragePanel_Used", "Used"},
                {"StoragePanel_KeepSpace", "Keep Space"},
                {"StoragePanel_Usage", "Usage %"},
            };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            _keepSpaceTextBox = new HotKeyTextBox
            {
                Width = 35,
                Dock = DockStyle.None,
                Location = new Point(500, 10),
                Visible = false,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                MaxLength = 4,
                ShortcutsEnabled = false,
                Enabled = false,
                ImeMode = ImeMode.Disable
            };

            _keepSpaceTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _keepSpaceTextBox.TextChanged += KeepSpaceTextBoxTextChanged;
            _keepSpaceTextBox.LostFocus += KeepSpaceTextBoxLostFocus;
            Controls.Add(_keepSpaceTextBox);

            MouseDown += StoragePanelMouseDown;
            MouseUp += StoragePanelMouseUp;
            Paint += StoragePanelPaint;
        }


        public UInt16 KeepSpace
        {
            get
            {
                UInt32 space = (_keepSpaceTextBox.Text != "") ? Convert.ToUInt32(_keepSpaceTextBox.Text) : 0;

                return Convert.ToUInt16(Math.Min(Math.Max(space, 1), 65535));
            }
            set
            {
                _keepSpaceTextBox.Text = value.ToString();
            }
        }

        public Boolean DisplayEditor
        {
            set
            {
                _keepSpaceTextBox.Visible = value;
            }
        }

        private void KeepSpaceTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (OnKeepSpaceChange != null)
                OnKeepSpaceChange(this, null);
        }

        private void KeepSpaceTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (OnKeepSpaceLostFocus != null)
                OnKeepSpaceLostFocus(this, null);
        }

        private void StoragePanelMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= StoragePanelMouseMove;
        }

        private Int32 _positionX;
        private void StoragePanelMouseDown(Object sender, MouseEventArgs e)
        {
            if (!_inUse) return;

            _positionX = e.X;
            MouseMove -= StoragePanelMouseMove;
            MouseMove += StoragePanelMouseMove;
        }

        private void StoragePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _positionX) return;
            if (OnStorageDrag != null)
            {
                MouseMove -= StoragePanelMouseMove;
                OnStorageDrag(this, e);
            }
        }

        private static readonly Image _storageBg = StorageImage.StorageBg2();
        private static readonly Image _storageBar = StorageImage.StorageUsage();
        private static readonly Image _keep = StorageImage.StorageKeep();
        private static readonly Image _storageBarActivate = StorageImage.StorageActivate();
        public Brush SelectedColor = Manager.SelectedTextColor;

        private const UInt32 Gb2Byte = 1073741824;

        private void StoragePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Server == null) return;
            if (Server.Server.StorageInfo.Count == 0) return;

            if (Parent == null) return;

            if (Width <= 100) return;

            Graphics g = e.Graphics;

            if (Tag == null)
            {
                Manager.PaintTitleTopInput(g, this);

                if (Server is ICMS || Server.Server.Platform == Platform.Linux)
                {
                    if (Width <= 110) return;
                    g.DrawString(Localization["StoragePanel_Drive"], Manager.Font, Manager.TitleTextColor, 30, 13);

                    if (Width <= 210) return;
                    g.DrawString(Localization["StoragePanel_Capacity"], Manager.Font, Manager.TitleTextColor, 116, 13);

                    if (Width <= 310) return;
                    g.DrawString(Localization["StoragePanel_Available"], Manager.Font, Manager.TitleTextColor, 216, 13);

                    if (Width <= 410) return;
                    g.DrawString(Localization["StoragePanel_Used"], Manager.Font, Manager.TitleTextColor, 316, 13);

                    if (Width <= 510) return;
                    g.DrawString(Localization["StoragePanel_Usage"], Manager.Font, Manager.TitleTextColor, 400, 13);
                }
                else
                {
                    Manager.PaintTitleText(g, Localization["StoragePanel_Sequence"]);

                    if (Width <= 210) return;
                    g.DrawString(Localization["StoragePanel_Drive"], Manager.Font, Manager.TitleTextColor, 130, 13);

                    if (Width <= 310) return;
                    g.DrawString(Localization["StoragePanel_Capacity"], Manager.Font, Manager.TitleTextColor, 216, 13);

                    if (Width <= 410) return;
                    g.DrawString(Localization["StoragePanel_Available"], Manager.Font, Manager.TitleTextColor, 316, 13);

                    if (Width <= 510) return;
                    g.DrawString(Localization["StoragePanel_Used"], Manager.Font, Manager.TitleTextColor, 416, 13);

                    if (Width <= 590) return;
                    g.DrawString(Localization["StoragePanel_KeepSpace"], Manager.Font, Manager.TitleTextColor, 500, 13);

                    if (Width <= 690) return;
                    g.DrawString(Localization["StoragePanel_Usage"], Manager.Font, Manager.TitleTextColor, 600, 13);
                }

                return;
            }

            Manager.Paint(g, this);

            Brush brush = Brushes.Black;
            if (InUse)
            {
                brush = SelectedColor;
                Manager.PaintSelected(g);
            }

            if (!Enabled)
                brush = Brushes.Gray;

            var storageBGX = 600;
            var usageX = 720;
            if (Server is ICMS || Server.Server.Platform == Platform.Linux)
            {
                storageBGX = 400;
                usageX = 520;

                if (Width <= 110) return;
                g.DrawString(Tag.ToString(), Manager.Font, brush, 30, 13);

                if (Width <= 210) return;
                g.DrawString((DiskInfo.Total / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 116, 13);

                if (Width <= 310) return;
                g.DrawString((DiskInfo.Free / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 216, 13);

                if (Width <= 410) return;
                g.DrawString((DiskInfo.Used / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 316, 13);

                if (Width <= 510) return;
                _keepSpaceTextBox.Visible = false;
            }
            else
            {
                if (Order != 0)
                    Manager.PaintText(g, Order.ToString(), brush);

                if (Width <= 210) return;
                g.DrawString(Tag.ToString(), Manager.Font, brush, 130, 13);

                if (Width <= 310) return;
                g.DrawString((DiskInfo.Total / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 216, 13);

                if (Width <= 410) return;
                g.DrawString((DiskInfo.Free / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 316, 13);

                if (Width <= 510) return;
                g.DrawString((DiskInfo.Used / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 416, 13);

                if (Width <= 590) return;
                g.DrawString("GB", Manager.Font, brush, 535, 13);

                if (Width <= 690) return;
            }

            var width = 120;
            var usage = DiskInfo.Used * 1.0 / DiskInfo.Total;

            g.DrawImage(_storageBg, storageBGX, 16, width, 7);

            var userec = new Rectangle
            {
                X = storageBGX,
                Y = 16,
                Width = Convert.ToUInt16(Math.Round(usage * width)),
                Height = _storageBar.Height
            };
            var usesrc = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = userec.Width,
                Height = userec.Height
            };
            g.DrawImage(_storageBar, userec, usesrc, GraphicsUnit.Pixel);

            //------------

            foreach (var storage in Server.Server.ChangedStorage)
            {
                if (storage.Key != Tag.ToString()) continue;

                Int32 keepWidth = Convert.ToUInt16(storage.KeepSpace * (Gb2Byte * 1.0) * width / DiskInfo.Total);

                var keeprec = new Rectangle
                {
                    X = storageBGX + width - keepWidth,
                    Y = 16,
                    Width = keepWidth,
                    Height = _keep.Height
                };
                var keepsrc = new Rectangle
                {
                    X = _keep.Width - keepWidth,
                    Y = 0,
                    Width = keeprec.Width,
                    Height = keeprec.Height
                };
                g.DrawImage(_keep, keeprec, keepsrc, GraphicsUnit.Pixel);
                break;
            }

            g.DrawString((usage * 100).ToString("0") + "%", Manager.Font, brush, usageX, 13);
        }
    }
}