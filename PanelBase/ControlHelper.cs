using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PanelBase
{
    public static class ControlHelper
    {
        // UI
        public static void Insert(this System.Windows.Forms.Control.ControlCollection controls, System.Windows.Forms.Control control, int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("index", "The new position cannot less than 0.");
            if (control == null) throw new ArgumentNullException("control", "The control cannot be null.");

            controls.Add(control);
            controls.SetChildIndex(control, index);
        }

        public static void SetFontStyle(this System.Windows.Forms.Control control, FontStyle style)
        {
            var font = new Font(control.Font, style);
            control.Font = font;
        }

        public static void SetFontStyle(this System.Windows.Forms.Control control, FontStyle style, Color foreColor)
        {
            control.ForeColor = foreColor;
            control.SetFontStyle(style);
        }
    }
}
