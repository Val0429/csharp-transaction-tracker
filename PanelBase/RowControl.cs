using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PanelBase
{
    public partial class RowControl : Panel
    {
        private Brush _rightTextColorBrush = Brushes.Black;
        private bool _isSelected;
        private TitleStyle _headerStyle;
        private string _rightText;


        public RowControl()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            if (HeaderStyle == TitleStyle.Dark)
            {
                Manager.PaintTitleTopInput(g, this);
            }
            else if (HeaderStyle == TitleStyle.HighLight)
            {
                Manager.PaintHighLightTopInput(g, this);
            }
            else if (HeaderStyle == TitleStyle.Single)
            {
                Manager.PaintSingleInput(g, this);
            }
            else
            {
                Manager.Paint(g, this);
                if (IsSelected)
                {
                    Manager.PaintSelected(g);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (IsSelected)
                {
                    Manager.PaintText(g, Text, Manager.SelectedTextColor);
                }
                else
                {
                    Manager.PaintText(g, Text, Enabled ? Brushes.Black : Brushes.Gray);
                }
            }

            if (!string.IsNullOrEmpty(RightText))
            {
                Manager.PaintTextRight(g, this, RightText, RightTextColorBrush);
            }
        }

        public TitleStyle HeaderStyle
        {
            get { return _headerStyle; }
            set
            {
                if (_headerStyle != value)
                {
                    _headerStyle = value;

                    if (_headerStyle != TitleStyle.Normal)
                    {
                        Height = 40;
                    }
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                if (HeaderStyle == TitleStyle.Dark)
                {
                    return Color.WhiteSmoke;
                }
                return base.ForeColor;
            }
            set { base.ForeColor = value; }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public string RightText
        {
            get { return _rightText; }
            set
            {
                if (_rightText != value)
                {
                    _rightText = value;

                    Invalidate();
                }
            }
        }

        public Brush RightTextColorBrush
        {
            get { return _rightTextColorBrush; }
            set { _rightTextColorBrush = value; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }
    }

    public enum TitleStyle { Normal, HighLight, Dark, Single }
}
