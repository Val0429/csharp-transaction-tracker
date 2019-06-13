using System;
using System.Drawing;
using System.Windows.Forms;

namespace Interface
{
    public interface IDrop
    {
        Boolean CheckDragDataType(Object dragObj);

        void DragStop(Point point, EventArgs<Object> xml);
        void DragMove(MouseEventArgs e);
    }
}
