using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Interface
{
    public interface IBlockPanel
    {
        ILayoutManager LayoutManager { get; set; }

        Boolean IsDockable { get; }
        Boolean IsAutoWidth { get; }

        List<IControlPanel> ControlPanels { get; }
        XmlElement BlockNode { set; }

        Color BackColor { get; set; }
        DockStyle Dock { get; }

        List<IControlPanel> SyncDisplayControlList { get; }

        void ShowThisControlPanel(Control control);
        void HideThisControlPanel(Control control);

        Boolean IsFocusedControl(Control control);
        void Activate();
        void Deactivate();
        void RefreshComponent();
    }
}
