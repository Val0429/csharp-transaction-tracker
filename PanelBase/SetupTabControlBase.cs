using System;
using System.Linq;
using System.Windows.Forms;
using Interface;

namespace PanelBase
{
    public partial class SetupTabControlBase : BlockTabControlBase
    {
        protected const string Separator = "    /    ";

        // Fields
        protected string TitleXml;


        // Constructor
        public SetupTabControlBase()
        {
            InitializeComponent();
        }


        // Handlers
        public void OnTitleChanged(object sender, EventArgs<String> e)
        {
            String command;
            // To check the callback is from this page or not
            /*
             <XML>
               <From></From>
               <Item></Item>
             </XML>
             */
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out command)) return;

            OnCommandExecuted(command);
        }

        protected virtual void OnCommandExecuted(string command)
        {
            if (command == "Back")
            {
                SetTitle(TitleName);
            }
        }


        // Methods
        protected override Button CreateIcon()
        {
            var icon = new SetupIcon() { Icon = TabIcon, ActiveIcon = ActiveTabIcon, IconText = TitleName };
            icon.Click += DockIconClick;

            return icon;
        }

        protected override void RaiseOnMinimizeChange(EventArgs e)
        {
            base.RaiseOnMinimizeChange(e);

            var icon = Icon as SetupIcon;
            if (icon != null)
            {
                icon.IsActive = !IsMinimize;
            }

            if (!IsMinimize)
            {
                if (string.IsNullOrEmpty(TitleXml))
                {
                    TitleXml = CreateTitleXml(TitleName);
                }

                RaiseOnSelectionChange(new EventArgs<string>(TitleXml));
            }
        }

        protected void SetTitle(string title, bool back = false, params string[] buttons)
        {
            TitleXml = CreateTitleXml(title, back, buttons);

            if (!IsMinimize)
            {
                RaiseOnSelectionChange(new EventArgs<string>(TitleXml));
            }
        }
        /*
         <XML>
            <From></From>
            <Item></Item>
            <Previous></Previsou>
            <Buttons></Buttons>
         </XML>
         */
        protected virtual string CreateTitleXml(string title, bool previous = false, params string[] buttons)
        {
            // Delete,Clone,CopySetting
            var button = buttons != null && buttons.Any() ? String.Join(",", buttons) : string.Empty;

            var xml = Manager.SelectionChangedXml(TitleName, title, previous ? "Back" : string.Empty, button);

            return xml;
        }


        // Event
        public event EventHandler<EventArgs<string>> SelectionChange;
        protected void RaiseOnSelectionChange(EventArgs<string> e)
        {
            var handler = SelectionChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
