using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Constant;
using Interface;
using ServerProfile;

namespace App_NetworkVideoRecorder
{
	public partial class NetworkVideoRecorder
	{
		//[DllImport("winmm.dll")]
		//static extern Int32 mciSendString(String command, StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

		public override event EventHandler<EventArgs<ICamera>> OnHotSpotEvent;

		private void NVROnEventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			foreach (var cameraEvent in e.Value)
			{
				if (cameraEvent.Device == null) continue;
				if (!(cameraEvent.Device is ICamera)) continue;

				var camera = cameraEvent.Device as ICamera;

				//check if event trigger time is in eventhandling schedule, else do nothing
				if (!camera.EventSchedule.CheckSchedule(DateTimes.ToScheduleTime(cameraEvent.DateTime))) continue;

				//get handling list that match cameraEvent, list could be null or empty.
				var handling = camera.EventHandling.GetEventHandleViaCameraEvent(cameraEvent.Type, cameraEvent.Id, cameraEvent.Value);

				if (handling == null || handling.Count == 0) continue;

				//Do handling here
				foreach (var eventHandle in handling)
				{
					switch (eventHandle.Type)
					{
						case HandleType.ExecCmd:
							var execEventHanele = eventHandle as ExecEventHandle;
							if (execEventHanele != null)
							{
								try
								{
									//Create process 
									var pProcess = new Process
													{
														StartInfo =
															{
																FileName = execEventHanele.FileName,
																UseShellExecute = false
																//, Arguments = strCommandParameters
																//, RedirectStandardOutput = true
																//, WorkingDirectory = strWorkingDirectory
															}
													};
									pProcess.Start();
									pProcess.WaitForExit(1);
								}
								catch( Exception ex) { }
							}
							break;

						case HandleType.Audio:
							var audioEventHandle = eventHandle as AudioEventHandle;
							//play audio
							if (audioEventHandle != null)
							{
								//try
								//{
								//    mciSendString("seek MediaFile to start", null, 0, IntPtr.Zero);
								//    mciSendString("stop MediaFile", null, 0, IntPtr.Zero);

								//    String sFileName = audioEventHandle.FileName;
								//    string openFile = "open \"" + sFileName + "\" type mpegvideo alias MediaFile";

								//    mciSendString(openFile, null, 0, IntPtr.Zero);

								//    mciSendString("play MediaFile", null, 0, IntPtr.Zero);
								//}
								//catch ( Exception ex) { }
							}
							break;

						case HandleType.Beep:
							var beepEventHandle = eventHandle as BeepEventHandle;
							if (beepEventHandle != null)
							{
								_nvr.Utility.PlaySystemSound(beepEventHandle.Times,
									Convert.ToUInt16(beepEventHandle.Duration * 1000),
									Convert.ToUInt16(beepEventHandle.Interval * 1000));
							}
							break;

						case HandleType.PopupPlayback:
							var popupPlaybackEventHandle = eventHandle as PopupPlaybackEventHandle;
							if (popupPlaybackEventHandle != null)
							{
								PopupInstantPlayback(popupPlaybackEventHandle.Device, DateTimes.ToUtc(_nvr.Server.DateTime, _nvr.Server.TimeZone));
							}
							break;
						case HandleType.HotSpot:
							var hotspotEventHandle = eventHandle as HotSpotEventHandle;
							if (hotspotEventHandle != null)
							{
								OnHotSpotEvent(this, new EventArgs<ICamera>( (hotspotEventHandle.Device as ICamera) ) );
							}
							break;
					}
				}
			}
		}
	}
}
