using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace TimeTrack
{
    public partial class EventButton : Panel
    {
        public EventButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public Image ActiveImage { get; set; }

        public Image Image { get; set; }

        public EventType EventType { get; set; }
    }
}
