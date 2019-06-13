using System;
using Interface;
using SetupBase;

namespace SetupSchedule.PreRecord30
{
	public partial class Setup : SetupSchedule.Setup
	{
		public override event EventHandler<EventArgs<String>> OnSelectionChange;
		private new SchedulePanel _schedulePanel;

		public override void Initialize()
		{
			base.Initialize();
			_schedulePanel = new SchedulePanel
			{
				Server = Server,
			};
			_schedulePanel.Initialize();
		}

		protected override void EditDevice(IDevice device)
		{
			if (!(device is ICamera)) return;

			_focusControl = _schedulePanel;
			_schedulePanel.Camera = (ICamera)device;

			Manager.MoveToSettingComplete -= ManagerMoveToEditComplete;
			Manager.MoveToSettingComplete += ManagerMoveToEditComplete;
			Manager.MoveToSetting(_listPanel, _schedulePanel, contentPanel);
		}

		protected override void ManagerMoveToEditComplete(Object sender, EventArgs e)
		{
			Manager.MoveToSettingComplete -= ManagerMoveToEditComplete;

			_schedulePanel.ParseDevice();
            
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(
					Manager.SelectionChangedXml(TitleName, _schedulePanel.Camera.ToString(), Localization["Common_Back"], "")));
		}
	}
}
