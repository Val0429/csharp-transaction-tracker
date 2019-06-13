using System;

namespace SetupSchedule.PreRecord30
{
	public partial class SchedulePanel : SetupSchedule.SchedulePanel
	{
		protected override void PreRecordTextBoxTextChanged(Object sender, EventArgs e)
		{
			UInt32 duration = (preRecordTextBox.Text != "") ? Convert.ToUInt32(preRecordTextBox.Text) * 1000 : 0;

			Camera.PreRecord = Convert.ToUInt32(Math.Min(Math.Max(duration, 0), 30000));

			CameraIsModify();
		}
	}
}
