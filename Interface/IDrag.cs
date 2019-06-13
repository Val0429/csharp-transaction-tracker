using System;
using System.Windows.Forms;

namespace Interface
{
    public interface IDrag
    {
        event EventHandler<EventArgs<Object>> OnDragStart;

        Panel DragDropProxy { get; }
    }
}
