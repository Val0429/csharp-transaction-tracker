using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace EventPanel.Popup
{
	public partial class EventPopup : EventPanel
	{
		public override event EventHandler<EventArgs<String>> OnEventTrigger;
		public override event EventHandler<EventArgs<String>> OnUserDefineEvent;

		public override void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (e.Value == null) return;

			// Tulip
			foreach (ICameraEvent cameraEvent in e.Value)
			{
				var str = OnEventTriggerXml
					.Replace("{DeviceID}", cameraEvent.Device.Id.ToString())
					.Replace("{Status}", cameraEvent.Status);

				if (cameraEvent.Type != EventType.UserDefine)
				{
					LogOnLogDoubleClick(this, new EventArgs<IDevice, DateTime>(cameraEvent.Device, cameraEvent.DateTime));

					if (OnEventTrigger != null)
						OnEventTrigger(this, new EventArgs<string>(str));
				}
				else
				{
					if (OnUserDefineEvent != null)
						OnUserDefineEvent(this, new EventArgs<string>(str));

					continue;
				}

				AddEvent(cameraEvent);
			}
		}
	}
}
