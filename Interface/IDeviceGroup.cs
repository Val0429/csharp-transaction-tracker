using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Interface
{
	public interface IDeviceGroup
	{
		UInt16 Id { get; set; }
		String Name { get; set; }
		IServer Server { get; set; }
        /// <summary>
        /// The device entity
        /// </summary>
		List<IDevice> Items { get; set; }//define as Object, can set value as null, better then define as Int16
        List<XmlElement> Regions { get; set; }
        List<Int16> MountType { get; set; }
        List<Boolean> DewarpEnable { get; set; }
        /// <summary>
        /// The sequence of the view. Each item of View must be in IDeviceGroup.Items or null.
        /// </summary>
		List<IDevice> View { get; set; }
		List<WindowLayout> Layout { get; set; }
		Boolean IsExpand { get; set; }
		ReadyState ReadyState { get; set; }
	}
}
