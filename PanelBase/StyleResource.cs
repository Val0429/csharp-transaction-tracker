using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PanelBase
{
    public static class StyleResource
    {
        public static readonly Color SalientSideBarColor = Color.FromArgb(96, 103, 117);

        public static readonly Brush FontBrush = Brushes.Black;

#if Salient
        public static readonly SolidBrush InputBrush = new SolidBrush(Color.FromArgb(96, 103, 117));

        public static readonly Brush InputFontBrush = Brushes.White;
#else
#if Salient_TT
        public static readonly SolidBrush InputBrush = new SolidBrush(Color.FromArgb(167, 173, 183));

        public static readonly Brush InputFontBrush = Brushes.Black;
#else
        public static readonly SolidBrush InputBrush = new SolidBrush(Color.FromArgb(234, 235, 239));

        public static readonly Brush InputFontBrush = Brushes.Black;
#endif
#endif
    }
}
