using App;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem
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
