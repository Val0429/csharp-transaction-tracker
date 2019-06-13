using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PanelBase
{
    public partial class ToggleButton : Button
    {
        // Fields
        private readonly Color _inactiveForeColor;
        private readonly Color _inactiveBackColor;
        private Image _inactiveBackgroundImage;
        private Image _activeBackgroundImage;
        private string _toolTipText;
        private bool _active;


        // Constructor
        public ToggleButton()
        {
            ActiveForeColor = ForeColor;

            InitializeComponent();

            FlatAppearance.CheckedBackColor = Color.Transparent;

            _inactiveForeColor = ForeColor;
            _inactiveBackColor = BackColor;
        }

        protected override void OnClick(EventArgs e)
        {
            Active = !Active;

            base.OnClick(e);
        }
        

        // Properties
        public sealed override Color ForeColor { get; set; }

        public sealed override Color BackColor { get; set; }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;

                    Image img;
                    Color foreColor;
                    Color backColor;
                    if (_active)
                    {
                        img = ActiveBackgroundImage;
                        foreColor = ActiveForeColor;
                        backColor = ActiveBackColor;
                    }
                    else
                    {
                        img = InactiveBackgroundImage;
                        foreColor = _inactiveForeColor;
                        backColor = _inactiveBackColor;
                    }

                    BackgroundImage = img;
                    ForeColor = foreColor;
                    BackColor = backColor;

                    OnStateChanged(new StateChangedEventArgs(_active));
                }
            }
        }

        public Image ActiveBackgroundImage
        {
            get { return _activeBackgroundImage; }
            set
            {
                if (_activeBackgroundImage != value)
                {
                    _activeBackgroundImage = value;

                    if (Active)
                    {
                        BackgroundImage = _activeBackgroundImage;
                    }
                }
            }
        }

        public Image InactiveBackgroundImage
        {
            get { return _inactiveBackgroundImage; }
            set
            {
                if (_inactiveBackgroundImage != value)
                {
                    _inactiveBackgroundImage = value;

                    if (!Active)
                    {
                        BackgroundImage = _inactiveBackgroundImage;
                    }
                }
            }
        }

        public Color ActiveForeColor { get; set; }

        public Color ActiveBackColor { get; set; }

        public string ToolTipText
        {
            get { return _toolTipText; }
            set
            {
                if (_toolTipText != value)
                {
                    _toolTipText = value;

                    SharedToolTips.SharedToolTip.SetToolTip(this, _toolTipText);
                }
            }
        }


        /// <summary>
        /// To raise StateChanged if Active state is changed.
        /// This event is fired before Click event when user clicks.
        /// </summary>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        protected void OnStateChanged(StateChangedEventArgs e)
        {
            var handler = StateChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class StateChangedEventArgs : EventArgs
    {
        internal StateChangedEventArgs(bool active)
        {
            Active = active;
        }

        public bool Active { get; private set; }
    }
}
