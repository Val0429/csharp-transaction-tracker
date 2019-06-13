﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports
{
	public sealed partial class Summary : UserControl, IControl, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public IServer Server { get; set; }
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Summary()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportSummary", "Summary"},

								   {"Common_Back", "Back"},
							   };
			Localizations.Update(Localization);

			Name = "ReportSummary";
			TitleName = Localization["Control_ReportSummary"];

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
