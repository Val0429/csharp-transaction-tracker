using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Constant;
using Device;

namespace ServerProfile.Plugin
{
	public partial class DeviceManager : ServerProfile.DeviceManager
	{
		protected override void BeepHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
		{
			//<Beep>
			//    <Times>2</Times>
			//    <Duration>2</Duration>
			//    <Interval>1</Interval>
			//</Beep>

			var handle = new BeepEventHandle
			{
				Times = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Times")),
				Duration = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Duration")),
				ReadyState = ReadyState.Ready,
			};
			eventHandle.Add(handle);
		}

		protected override void AudioHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
		{
			//<Audio>
			//    <FileName>C:\\1.wav</FileName>
			//</Audio>

			var handle = new AudioEventHandle
			{
				FileName = Xml.GetFirstElementValueByTagName(node, "FileName"),
				ReadyState = ReadyState.Ready,
			};
			eventHandle.Add(handle);
		}

		protected override void ExecHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
		{
			//<ExecCmd>
			//    <FileName>C:\\1.wav</FileName>
			//</ExecCmd>

			var handle = new ExecEventHandle
			{
				FileName = Xml.GetFirstElementValueByTagName(node, "FileName"),
				ReadyState = ReadyState.Ready,
			};
			eventHandle.Add(handle);
		}

		protected override void PopupPlaybackHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
		{
			//<Popup>
			//    <Device>1</Device>
			//</Popup>

			var handle = new PopupPlaybackEventHandle
			{
				Device = new BasicDevice{ Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Device")) },
				ReadyState = ReadyState.Ready,
			};
			eventHandle.Add(handle);
		}

		protected override void HotSpotHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
		{
			//<HotSpot>
			//    <Device>1</Device>
			//</HotSpot>

			var handle = new HotSpotEventHandle
			{
				Device = new BasicDevice { Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Device")) },
				ReadyState = ReadyState.Ready,
			};
			eventHandle.Add(handle);
		}
	}
}
