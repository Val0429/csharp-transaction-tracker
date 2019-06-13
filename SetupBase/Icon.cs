using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace SetupBase
{
    public partial class Icon : UserControl, IControl, IBlockPanelUse
    {
        public event EventHandler<EventArgs<String>> OnIconClick;

        public String TitleName { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Icon()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            BackgroundImage = Resources.GetResources(Properties.Resources.tagBG, Properties.Resources.IMGTagBg);
            ActivateIcon = InactivateIcon = Button.Image = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

            Localization = new Dictionary<String, String>();
        }

        public virtual void Initialize()
        {
            Localizations.Update(Localization);

            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);
        }

        protected Image ActivateIcon;
        protected Image InactivateIcon;

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void ActiveSetup()
        {
            foreach (IControlPanel controlPanel in BlockPanel.SyncDisplayControlList)
            {
                if(!(controlPanel.Control is Icon)) continue;
                
                var icon = controlPanel.Control as Icon;
                icon.InUse = (icon == this);
            }

            if (OnIconClick != null)
                OnIconClick(this, new EventArgs<String>(""));
        }

        private Boolean _inUse;
        public Boolean InUse
        {
            set
            {
                _inUse = value;
                if (_inUse)
                {
                    Button.Image = ActivateIcon;

                    BackgroundImage = Resources.GetResources(Properties.Resources.tagBG, Properties.Resources.IMGTagBg);
                    ForeColor = Color.Black;
                }
                else
                {
                    Button.Image = InactivateIcon;

                    BackgroundImage = null;
                    ForeColor = Color.Gray;
                }
            }
            get { return _inUse; }
        }

        private void ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            ActiveSetup();
        }
    }
}
