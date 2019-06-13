using System;
using System.Drawing;
using System.Windows.Forms;

namespace Constant
{
    public static class Drag
    {
        public static Boolean IsDrop(Control control)
        {
            return IsDrop(control, Cursor.Position);
        }

        public static Boolean IsDrop(Control control, Point point)
        {
            Point controlRelatedCoords = control.PointToClient(point);

            return (controlRelatedCoords.X >= 0 && controlRelatedCoords.X <= control.Size.Width &&
                    controlRelatedCoords.Y >= 0 && controlRelatedCoords.Y <= control.Size.Height);
        }
    }
}