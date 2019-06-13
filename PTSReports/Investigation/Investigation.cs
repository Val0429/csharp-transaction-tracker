using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Investigation
{
	public sealed partial class Investigation : UserControl, IControl, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public IServer Server { get; set; }
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Investigation()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportInvestigation", "Investigation"},
							   };
			Localizations.Update(Localization);

			Name = "ReportInvestigation";
			TitleName = Localization["Control_ReportInvestigation"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}
	}
}
