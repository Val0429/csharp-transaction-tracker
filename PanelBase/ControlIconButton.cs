
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace PanelBase
{
    public sealed class ControlIconButton : Button
    {
        private static Image _iconBgActivate;
        public static Image IconBgActivate
        {
            get
            {
                if (_iconBgActivate == null)
                    _iconBgActivate = Resources.GetResources(Properties.Resources.icon_bg_activate, Properties.Resources.IMGIconBGActivate);

                return _iconBgActivate;
            }
        }

        public ControlIconButton()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ImageAlign = ContentAlignment.MiddleCenter;
            BackgroundImageLayout = ImageLayout.Center;
            Dock = DockStyle.Top;
            Margin = new Padding(0);
            Size = new Size(70, 74);
            Cursor = Cursors.Hand;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.CheckedBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
        }
    }
}
