using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PanelBase
{
    public partial class SetupIcon : System.Windows.Forms.Button
    {
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly SolidBrush _fontBrush = new SolidBrush(Color.FromArgb(141, 145, 154));
        private readonly SolidBrush _fontEnableBrush = new SolidBrush(Color.White);

        private Int32 _iconPositionX;
        private Int32 _iconPositionY;


        public SetupIcon()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var g = pe.Graphics;

            g.DrawLine(Pens.Black, 0, Height - 1, Width, Height - 1);

            if (_isActive)
            {
                if (ActiveIcon != null)
                {
                    g.DrawImage(ActiveIcon, _iconPositionX, _iconPositionY, ActiveIcon.Width, ActiveIcon.Height);
                }

                if (!String.IsNullOrEmpty(IconText))
                {
                    g.DrawString(IconText, _font, _fontEnableBrush, 60, 17);
                }
            }
            else
            {
                if (Icon != null)
                {
                    g.DrawImage(Icon, _iconPositionX, _iconPositionY, Icon.Width, Icon.Height);
                }

                if (!String.IsNullOrEmpty(IconText))
                {
                    g.DrawString(IconText, _font, _fontBrush, 60, 17);
                }
            }
        }


        public string IconText { get; set; }

        private Image _icon;
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;

                if (_icon != null)
                {
                    _iconPositionX = 10 + (50 - _icon.Width) / 2;
                    _iconPositionY = (Height - _icon.Height) / 2;
                }
            }
        }
        private Image _activeIcon;
        public Image ActiveIcon
        {
            get { return _activeIcon; }
            set
            {
                _activeIcon = value;

                if (_activeIcon != null)
                {
                    _iconPositionX = 10 + (50 - _activeIcon.Width) / 2;
                    _iconPositionY = (Height - _activeIcon.Height) / 2;
                }
            }
        }

        private Boolean _isActive;
        [Browsable(false)]
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;

                _isActive = value;

                BackColor = (value) ? Color.FromArgb(70, 74, 81) : Color.FromArgb(55, 59, 66);

                Invalidate();
            }
        }

        public override Image BackgroundImage
        {
            get { return null; }
            set { Image = null; }
        }
    }
}
