using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace SetupBase
{
	public partial class UndoUI2 : UserControl, IControl, IAppUse
	{
		public String TitleName { get; set; }
		public IApp App { get; set; }

		public Dictionary<String, String> Localization;


        // Constructor
		public UndoUI2()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Confirm", "Confirm"},

								   {"Undo_Button", "Undo"},
								   {"Undo_Warning", "Please confirm again to proceed Undo ALL."},
							   };
			Localizations.Update(Localization);
		}

		public virtual void Initialize()
		{
			InitializeComponent();
			Dock = DockStyle.Bottom;
			undoButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);

			undoButton.Text = Localization["Undo_Button"];
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		private void UndoButtonMouseClick(Object sender, MouseEventArgs e)
		{
			ApplicationForms.ShowProgressBar(App.Form);
			Application.RaiseIdle(null);

			var result = TopMostMessageBox.Show(Localization["Undo_Warning"], Localization["MessageBox_Confirm"],
								MessageBoxButtons.YesNo, MessageBoxIcon.Question);

		    if (result == DialogResult.Yes)
		    {
		        App.Undo();

                OnUndoClick();
		    }
			else
			{
				ApplicationForms.HideProgressBar();
			}
		}


        // Event
	    public event EventHandler UndoClick;

	    private void OnUndoClick()
	    {
	        var handler = UndoClick;
	        if (handler != null)
	        {
	            handler(this, EventArgs.Empty);
	        }
	    }
	}
}