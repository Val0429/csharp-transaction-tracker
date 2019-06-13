using System;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
	public partial class VideoMonitor
	{
		public void PTZOperation(Object sender, EventArgs<String> e)
		{
			if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
			if (ActivateVideoWindow.Camera == null || ActivateVideoWindow.Camera.Model == null || !ActivateVideoWindow.Camera.Model.IsSupportPTZ) return;
			if (!ActivateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;

			//Console.WriteLine(e.Value);

			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			String command = Xml.GetFirstElementValueByTagName(xmlDoc, "Command");

			switch (command)
			{
				case "cmdZoom":
					String zoomMode = Xml.GetFirstElementValueByTagName(xmlDoc, "Mode");
					switch (zoomMode)
					{
						case "TELE":
						case "WIDE":
							zoomMode += "," + 3;
							break;
					}
					command += ("=" + zoomMode);
					break;

				case "cmdFocus":
					String focusMode = Xml.GetFirstElementValueByTagName(xmlDoc, "Mode");
					command += ("=" + focusMode);
					break;

                case "cmdPTZPresetGO":
                    String pointId = Xml.GetFirstElementValueByTagName(xmlDoc, "Id");
                    command += ("=" + pointId);
                    break;

                case "cmdPTZPresetTourStart":
                    break;

                case "cmdPTZPresetTourStop":
                    break;

				case "cmdMoveStop":
					break;

                case "cmdObjTrackStart":
                    break;

                case "cmdObjTrackStop":
                    break;

				case "enablePTZ":
					ActivateVideoWindow.Viewer.PtzMode = PTZMode.Optical;
					return;

				case "disablePTZ":
					ActivateVideoWindow.Viewer.PtzMode = PTZMode.Digital;
					return;

				default:
					UInt16 panSpeed = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlDoc, "PanSpeed"));
					UInt16 tiltSpeed = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlDoc, "TiltSpeed"));
					command += ("=" + panSpeed + "," + tiltSpeed);
					break;
			}

			ActivateVideoWindow.SendPTZCommand(command);
			Console.WriteLine(command);
		}

        public void PTZPatrolOperation(Object sender, EventArgs<WindowPTZRegionLayout> e)
        {
            if (ActivateVideoWindow == null) return;
            if (ActivateVideoWindow.Viewer == null) return;
            //ActivateVideoWindow.PtzMode = PTZMode.Optical;
            if(e.Value == null)
            {
                ActivateVideoWindow.Viewer.SetDigitalPtzRegion(String.Empty);
                return;
            }
            ActivateVideoWindow.Viewer.SetDigitalPtzRegion(e.Value.RegionXML.OuterXml);
        }

		//public void PresetOperation(Object sender, EventArgs<String> e)
		//{
		//    if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
		//    if (ActivateVideoWindow.Camera == null || !ActivateVideoWindow.Camera.Model.IsSupportPTZ) return;
		//    if (!ActivateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;

		//    XmlDocument xmlDoc = Xml.LoadXml(e.Value);
		//    String command = Xml.GetFirstElementValueByTagName(xmlDoc, "Command");

		//    if (command == "cmdPTZPresetGO")
		//    {
		//        UInt16 preset = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(xmlDoc, "Preset"));

		//        ActivateVideoWindow.Camera.PresetPointGo = preset;
		//    }
		//}
	}
}
