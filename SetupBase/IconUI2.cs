
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SetupBase
{
    public sealed class IconUI2 : Button
    {
        private Int32 IconPositionX;
        private Int32 IconPositionY;
        private Int32 IconActivatePositionX;
        private Int32 IconActivatePositionY;

        private Image _icon;
        public Image IconImage
        {
            get { return _icon; }
            set
            {
                _icon = value;

                IconPositionX = 10 + (50 - _icon.Width) / 2;
                IconPositionY = (Height - _icon.Height) / 2;
            }
        }
        private Image _iconActivate;
        public Image IconActivateImage
        {
            get { return _iconActivate; }
            set
            {
                _iconActivate = value;

                IconActivatePositionX = 10 + (50 - _iconActivate.Width) / 2;
                IconActivatePositionY = (Height - _iconActivate.Height) / 2;
            }
        }

        public string IconText;

        private Boolean _isActivate;
        public bool IsActivate
        {
            get { return _isActivate; }
            set
            {
                _isActivate = value;

                BackColor = (value) ? Color.FromArgb(70, 74, 81) : Color.FromArgb(55, 59, 66);
                Invalidate();
            }
        }

        public IconUI2()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(55, 59, 66);
            ImageAlign = ContentAlignment.MiddleLeft;
            BackgroundImageLayout = ImageLayout.Center;
            Dock = DockStyle.Top;
            Margin = new Padding(0);
            Size = new Size(200, 49);
            Cursor = Cursors.Hand;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.CheckedBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;

            Paint += IconUI2Paint;
        }

        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly SolidBrush _fontBrush = new SolidBrush(Color.FromArgb(141, 145, 154));
        private readonly SolidBrush _fontEnableBrush = new SolidBrush(Color.White);
        private void IconUI2Paint(Object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawLine(Pens.Black, 0, Height - 1, Width, Height - 1);

            if (_isActivate)
            {
                if (IconActivateImage != null)
                {
                    g.DrawImage(IconActivateImage, IconActivatePositionX, IconActivatePositionY, IconActivateImage.Width, IconActivateImage.Height);
                }

                if (!String.IsNullOrEmpty(IconText))
                {
                    g.DrawString(IconText, _font, _fontEnableBrush, 60, 17);
                }
            }
            else
            {
                if (IconImage != null)
                {
                    g.DrawImage(IconImage, IconPositionX, IconPositionY, IconImage.Width, IconImage.Height);
                }

                if (!String.IsNullOrEmpty(IconText))
                {
                    g.DrawString(IconText, _font, _fontBrush, 60, 17);
                }
            }
    }
    }
}
