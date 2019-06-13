using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Interface
{
    public interface IControlPanel
    {
        event EventHandler OnMinimizeChange;

        IBlockPanel BlockPanel { set; }
        Boolean IsDragable { get; }
        Boolean IsAutoHeight { get; }
        Boolean IsMinimize { get; }

        Size Size { get; set; }
        Boolean Visible { get; set; }
        XmlElement ControlNode { set; }
        IControl Control { get; set; }
        Button Icon { get; }

        void Activate();
        void Deactivate();
    }
}
