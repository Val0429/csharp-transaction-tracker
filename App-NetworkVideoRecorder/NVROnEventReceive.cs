using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Constant;
using DeviceConstant;
using Interface;
using ServerProfile;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
    {
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String command, StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

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
                if (cameraEvent.Type == EventType.UserDefine)
                    cameraEvent.Value = true;

                var handling = camera.EventHandling.GetEventHandleViaCameraEvent(cameraEvent.Type, cameraEvent.Id, cameraEvent.Value);

                if (handling == null || handling.Count == 0) continue;

                var condition = camera.EventHandling.GetEventCondition(cameraEvent.Type, cameraEvent.Id, cameraEvent.Value);

                if (!condition.Enable) return;

                //will set enable to false, and after interval, reset enable to true.
                condition.CoolDownTriggerInterval();

                handling.Sort((x, y) => (y.Type - x.Type));
                //Do handling here);
                foreach (var eventHandle in handling)
                {
                    switch (eventHandle.Type)
                    {
                        case HandleType.ExecCmd:
                            ExecCmd(eventHandle);
                            break;

                        case HandleType.Audio:
                            Audio(eventHandle, camera);
                            break;

                        case HandleType.Beep:
                            Beep(eventHandle);
                            break;

                        case HandleType.HotSpot:
                            HotSpot(eventHandle);
                            break;

                        case HandleType.GoToPreset:
                            GoToPreset(eventHandle);
                            break;

                        case HandleType.PopupPlayback:
                            PopupInstantPlayback(eventHandle);
                            break;

                        case HandleType.PopupLive:
                            PopupLiveStream(eventHandle);
                            break;
                    }
                }
            }
        }

        private void ExecCmd(EventHandle eventHandle)
        {
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
                catch (Exception) { }
            }
        }

        private void Audio(EventHandle eventHandle, ICamera camera)
        {
            var audioEventHandle = eventHandle as AudioEventHandle;
            //play audio
            if (audioEventHandle != null)
            {
                try
                {
                    var name = camera.ToString().Replace(" ", "") + new Random().NextDouble();

                    mciSendString("seek " + name + " to start", null, 0, IntPtr.Zero);
                    mciSendString("stop " + name, null, 0, IntPtr.Zero);

                    String sFileName = audioEventHandle.FileName;
                    string openFile = "open \"" + sFileName + "\" type mpegvideo alias " + name;

                    mciSendString(openFile, null, 0, IntPtr.Zero);

                    mciSendString("play " + name, null, 0, IntPtr.Zero);
                }
                catch (Exception) { }
            }
        }

        private void Beep(EventHandle eventHandle)
        {
            var beepEventHandle = eventHandle as BeepEventHandle;
            if (beepEventHandle != null)
            {
                Nvr.Utility.PlaySystemSound(beepEventHandle.Times,
                    Convert.ToUInt16(beepEventHandle.Duration * 1000),
                    Convert.ToUInt16(beepEventHandle.Interval * 1000));
            }
        }

        private void PopupInstantPlayback(EventHandle eventHandle)
        {
            //no permission
            if (!Pages.ContainsKey("Playback")) return;

            var popupPlaybackEventHandle = eventHandle as PopupPlaybackEventHandle;
            if (popupPlaybackEventHandle != null)
            {
                var permission = Nvr.User.Current.CheckPermission(popupPlaybackEventHandle.Device, Permission.Access);
                if (!permission) return;

                PopupInstantPlayback(popupPlaybackEventHandle.Device, DateTimes.ToUtc(Nvr.Server.DateTime, Nvr.Server.TimeZone));
            }
        }

        private void PopupLiveStream(EventHandle eventHandle)
        {
            var popupLiveEventHandle = eventHandle as PopupLiveEventHandle;
            if (popupLiveEventHandle != null)
            {
                var permission = Nvr.User.Current.CheckPermission(popupLiveEventHandle.Device, Permission.Access);
                if (!permission) return;

                PopupLiveStream(popupLiveEventHandle.Device);
            }
        }

        private void HotSpot(EventHandle eventHandle)
        {
            var hotspotEventHandle = eventHandle as HotSpotEventHandle;
            if (hotspotEventHandle != null)
            {
                var permission = Nvr.User.Current.CheckPermission(hotspotEventHandle.Device, Permission.Access);
                if (!permission) return;

                RaiseOnHotSpotEvent(new EventArgs<ICamera>((hotspotEventHandle.Device as ICamera)));
            }
        }

        private void GoToPreset(EventHandle eventHandle)
        {
            var gotoPresetEventHandle = eventHandle as GotoPresetEventHandle;
            if (gotoPresetEventHandle != null)
            {
                var permission = Nvr.User.Current.CheckPermission(gotoPresetEventHandle.Device, Permission.Access);
                if (!permission) return;

                if (gotoPresetEventHandle.Device is ICamera)
                    (gotoPresetEventHandle.Device as ICamera).PresetPointGo = gotoPresetEventHandle.PresetPoint;
            }
        }
    }
}
