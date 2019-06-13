using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports
{
	public sealed partial class Template : UserControl, IControl, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public IServer Server { get; set; }
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Template()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportTemplate", "Template"},

								   {"Common_Back", "Back"},
							   };
			Localizations.Update(Localization);

			Name = "ReportTemplate";
			TitleName = Localization["Control_ReportTemplate"];

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
			BlockPanel.ShowThisControlPanel(this);

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}
	}
}
