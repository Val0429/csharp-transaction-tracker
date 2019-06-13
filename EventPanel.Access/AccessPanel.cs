using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace EventPanel.Access
{
	public partial class AccessPanel : EventPanel
	{
		public override event EventHandler<EventArgs<String>> OnUserDefineEvent;

		public AccessPanel()
		{
            Icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
            _reset = Resources.GetResources(Properties.Resources.reset, Properties.Resources.IMGReset);

            Localization = new Dictionary<String, String>
                               {
                                   {"EventPanel_ClearEvent", "Clear event log"},
                               };
            Localizations.Update(Localization);
		}

		public override void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (e.Value == null) return;

			foreach (ICameraEvent cameraEvent in e.Value)
			{
				if (cameraEvent.Type == EventType.UserDefine)
				{
					if (cameraEvent.Device == null) continue;

					var str = EventTriggerXml(cameraEvent.Device.Id.ToString(), cameraEvent.Status);

					if (_showUserDefiendEvnet)
						AddEvent(cameraEvent);

					if (OnUserDefineEvent != null)
						OnUserDefineEvent(this, new EventArgs<string>(str));
				}
				else
				{
					AddEvent(cameraEvent);
				}
			}
		}

	}
}
