using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PanelBase
{
    public sealed class DoubleBufferLabel : Label
    {
        public DoubleBufferLabel()
        {
            DoubleBuffered = true;
        }
    }

    [Designer(typeof(ParentControlDesigner))]
    public sealed class DoubleBufferFlowLayoutPanel : FlowLayoutPanel
    {
        public DoubleBufferFlowLayoutPanel()
        {
            DoubleBuffered = true;
        }
    }

    public sealed class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            DoubleBuffered = true;
        }
    }

    public sealed class DoubleBufferTableLayoutPanel : TableLayoutPanel
    {
        public DoubleBufferTableLayoutPanel()
        {
            DoubleBuffered = true;
        }
    }
}
