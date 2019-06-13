using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;

namespace SetupBase
{
    public partial class SaveUI2 : UserControl, IControl, IAppUse
    {
        private readonly ILogger _logger = LoggerManager.Instance.GetLogger();

        public String TitleName { get; set; }
        public IApp App { get; set; }

        public Dictionary<String, String> Localization;


        // Constructor
        public SaveUI2()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Save_Button", "Save"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;
            saveButton.BackgroundImage = Resources.GetResources(Properties.Resources.saveButton, Properties.Resources.IMGSaveButton);

            saveButton.Text = Localization["Save_Button"];
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SaveButtonMouseClick(Object sender, MouseEventArgs e)
        {
            try
            {
                OnSaveClick();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            App.Save();

            OnSaveCompleted();
        }


        // Event
        public event EventHandler SaveClick;

        private void OnSaveClick()
        {
            var handler = SaveClick;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler SaveCompleted;

        private void OnSaveCompleted()
        {
            var handler = SaveCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}