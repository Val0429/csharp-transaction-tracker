using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Interface
{
    public interface ILayoutManager
    {
        IPage Page { get; set; }

        XmlDocument ConfigNode { set; }
        List<IBlockPanel> BlockPanels { get; }
        List<IControl> Function { get; }
        List<ToolStripMenuItem> Menus { get; }
        List<Panel> DragDropProxy { get; }

        DockStyle SidePanelDockStyle  { get; }
        Int32 SidePanelWidth { get; } 
        Int32 FunctionPanelHeight { get; } 

        void Activate();
        void Deactivate();
        void Refresh();
    }
}
