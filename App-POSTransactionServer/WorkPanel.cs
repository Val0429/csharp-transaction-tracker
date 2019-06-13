using App;

namespace App_POSTransactionServer
{
    public partial class POSTransactionServer
	{
		protected override void InitializeWorkPanel()
		{
			MainPanel = ApplicationForms.MainPanelUI2();

			WorkPanel = ApplicationForms.WorkPanelUI2();

			WorkPanel.MouseUp += WorkPanelMouseUp;
			WorkPanel.MouseMove += WorkPanelMouseMove;

			MainPanel.Controls.Add(WorkPanel);
			Form.Controls.Add(MainPanel);
		}
	}
}
