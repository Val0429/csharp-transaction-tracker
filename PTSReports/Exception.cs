using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports
{
	public sealed partial class Exception : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public IApp App { get; set; }
		public IServer Server { get; set; }
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public List<POS> POS;
		public List<POSException.Exception> Exceptions;
		public UInt64 StartDateTime;
		public UInt64 EndDateTime;

		public Exception()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportException", "Exception"},

								   {"Common_Back", "Back"},
							   };
			Localizations.Update(Localization);

			Name = "ReportException";
			TitleName = Localization["Control_ReportException"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			App.OnSwitchPage += SearchCriteriaChange;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
		{
			if (!String.Equals(e.Value1, "Report")) return;

			var exceptionsReportParameter = e.Value2 as ExceptionsReportParameter;
			if (exceptionsReportParameter == null) return;

			POS = new List<POS>(exceptionsReportParameter.POS.ToArray());
			Exceptions = new List<POSException.Exception>(exceptionsReportParameter.Exceptions.ToArray());
			StartDateTime = exceptionsReportParameter.StartDateTime;
			EndDateTime = exceptionsReportParameter.EndDateTime;
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			BlockPanel.ShowThisControlPanel(this);

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}
	}
}
