using System;
using System.Collections.Generic;
using System.Media;
using Constant;
using Interface;

namespace EventPanel.Action
{
	public partial class EventAction : EventPanel
	{
		public override event EventHandler<EventArgs<String>> OnEventTrigger;
		public override event EventHandler<EventArgs<String>> OnUserDefineEvent;

		//private Boolean _bHotSpot = false;
		private Boolean _bPopup = false;
		private Boolean _bBeep = false;
		private String _cmdFileName = "";

		private Boolean _showUserDefiendEvnet = false;

		//public void SetHotSpot()
		//{
		//	_bHotSpot = true;
		//}

		public void SetPopup()
		{
			_bPopup = true;
		}

		public void SetBeep()
		{
			_bBeep = true;
		}

		public void SetExecuteCommand()
		{
			_cmdFileName = "cmd";
		}

		public void ShowUserDefiendEvnet()
		{
			_showUserDefiendEvnet = true;
		}

		public override void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (e.Value == null) return;

			foreach (ICameraEvent cameraEvent in e.Value)
			{
				var str = EventTriggerXml(cameraEvent.Device.Id.ToString(), cameraEvent.Status);

				//if (_bPopup)
				//    LogOnLogDoubleClick(this, new EventArgs<IDevice, DateTime>(cameraEvent.Device, cameraEvent.DateTime));

				//if (_bBeep)
				//    SystemSounds.Beep.Play();


				//if (_cmdFileName != "")
				//{
				//    //Create process 
				//    var pProcess = new System.Diagnostics.Process
				//                    {
				//                        StartInfo =
				//                            {
				//                                FileName = _cmdFileName
				//                                , UseShellExecute = false
				//                                //, Arguments = strCommandParameters
				//                                //, RedirectStandardOutput = true
				//                                //, WorkingDirectory = strWorkingDirectory
				//                            }
				//                    };
				//    pProcess.Start();
				//    pProcess.WaitForExit(1);
				//}

				if (cameraEvent.Type != EventType.UserDefine)
				{
					AddEvent(cameraEvent);

					if (OnEventTrigger != null)
						OnEventTrigger(this, new EventArgs<string>(str));
				}
				else
				{
					if (_showUserDefiendEvnet)
						AddEvent(cameraEvent);

					if (OnUserDefineEvent != null)
						OnUserDefineEvent(this, new EventArgs<string>(str));
				}

				
			}
		}
	}
}
