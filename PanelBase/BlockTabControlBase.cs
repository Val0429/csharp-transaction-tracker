using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Interface;

namespace PanelBase
{
    public class BlockTabControlBase : BlockControlBase, IMinimize
    {
        // Field
        private bool _isMinimized;


        // Constructor
        public BlockTabControlBase()
        {

        }


        // Handler
        private void BlockTabControlBaseControlLoad(object sender, EventArgs e)
        {
            this.ControlLoad -= BlockTabControlBaseControlLoad;

            var controlPanel = Parent as IControlPanel;
            if (controlPanel != null && BlockPanel != null)
            {
                var index = BlockPanel.SyncDisplayControlList.IndexOf(controlPanel);
                if (index == 0)
                {
                    Maximize();
                }
            }
        }


        // Propreties
        ushort IMinimize.MinimizeHeight
        {
            get { return 0; }
        }

        [Browsable(false)]
        public Button Icon { get; private set; }

        /// <summary>
        /// The active tab icon
        /// </summary>
        [Browsable(true)]
        public Image ActiveTabIcon { get; set; }

        /// <summary>
        /// The inactive tab icon
        /// </summary>
        [Browsable(true)]
        public Image TabIcon { get; set; }
        [Browsable(false)]
        public bool IsMinimize
        {
            get { return _isMinimized; }
            private set
            {
                if (_isMinimized != value)
                {
                    if (value)
                    {
                        Icon.Image = TabIcon;
                        Icon.BackgroundImage = null;
                    }
                    else
                    {
                        Icon.Image = ActiveTabIcon;
                        Icon.BackgroundImage = ControlIconButton.IconBgActivate;
                    }

                    _isMinimized = value;

                    RaiseOnMinimizeChange(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public bool CanCollapsed { get; set; }


        // Methods
        protected void DockIconClick(Object sender, EventArgs e)
        {
            OnDockIconClick();
        }

        protected virtual void OnDockIconClick()
        {
            if (IsMinimize)
            {
                Maximize();
            }
            else
            {
                if (CanCollapsed)
                {
                    ((IMinimize)this).Minimize(); 
                }
            }
        }
        
        /// <summary>
        /// Create tab icon and set title name.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Tab
            Icon = CreateIcon();

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            this.ControlLoad += BlockTabControlBaseControlLoad;
        }

        protected virtual Button CreateIcon()
        {
            var icon = new ControlIconButton { Image = ActiveTabIcon, BackgroundImage = ControlIconButton.IconBgActivate };
            icon.Click += DockIconClick;

            return icon;
        }

        public void Maximize()
        {
            if (BlockPanel != null)
            {
                BlockPanel.ShowThisControlPanel(this);
            }

            IsMinimize = false;

            Refresh();
        }

        void IMinimize.Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
            {
                BlockPanel.HideThisControlPanel(this);
            }

            ((IControl)this).Deactivate();

            IsMinimize = true;
        }


        // Events
        public event EventHandler OnMinimizeChange;
        protected virtual void RaiseOnMinimizeChange(EventArgs e)
        {
            var handler = OnMinimizeChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
