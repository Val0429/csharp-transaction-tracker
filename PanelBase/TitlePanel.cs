using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PanelBase
{
    public sealed partial class TitlePanel : System.Windows.Forms.Panel
    {
        public TitlePanel()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            if (!string.IsNullOrEmpty(Text))
            {
                g.DrawString(Text, Manager.Font, Brushes.DimGray, 8, Padding.Top - 20);
            }
        }

        [Browsable(true)]
        public override string Text { get; set; }
    }
}
