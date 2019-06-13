using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Interface;

namespace PanelBase
{
    public partial class BlockControlBase : UserControl, IControl, IBlockPanelUse, IServerUse, IMouseHandler
    {
        // Fields
        private IServer _server;
        private bool _isLoad;


        // Constructor
        public BlockControlBase()
        {
            InitializeComponent();
        }


        // Properties
        public virtual string TitleName { get; set; }

        [Browsable(false)]
        public IBlockPanel BlockPanel { get; set; }
        
        [Browsable(false)]
        public IServer Server
        {
            get { return _server; }
            set { _server = value; }
        }

        [Browsable(false)]
        public IUser CurrentUser { get { return _server.User.Current; } }


        // Methods
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            var controlPanel = Parent as IControlPanel;
            if (controlPanel != null && BlockPanel != null)
            {
                BlockPanel.SyncDisplayControlList.Add(controlPanel);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_isLoad) return;
            
            _isLoad = true;

            base.OnLoad(e);
        }

        void IControl.Activate()
        {
            var handler = ControlLoad;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void IControl.Deactivate()
        {
            var handler = ControlUnload;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        // Events
        public event EventHandler ControlLoad;

        public event EventHandler ControlUnload;

        public virtual void GlobalMouseHandler()
        {
            
        }
    }
}
