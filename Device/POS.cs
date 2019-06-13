using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using Interface;

namespace Device
{
	public class POS : IPOS
	{
		public String Manufacture { get; set; }
		public String Model { get; set; } //current not use
		public String Id { get; set; }

	    ushort IDeviceGroup.Id{ get; set; }

	    public String Name { get; set; }
		public IServer Server { get; set; }
		public UInt16 LicenseId { get; set; }
		public UInt16 Exception { get; set; } //id link to exception file
		public UInt16 Keyword { get; set; } //id link to exception file
		public List<IDevice> Items { get; set; }
		public List<IDevice> View { get; set; }
		public List<WindowLayout> Layout { get; set; }
		public Boolean IsExpand { get; set; }
        public Boolean IsCapture { get; set; }
		public ExceptionReports ExceptionReports { get; protected set; }
        public List<XmlElement> Regions { get; set; }
        public List<Int16> MountType { get; set; }
        public List<Boolean> DewarpEnable { get; set; }
		public ReadyState ReadyState { get; set; }

		public POS()
		{
			Manufacture = "UNKNOWN";
			//RegisterId = 1;
			Items = new List<IDevice>();
			View = new List<IDevice>();
			Layout = new List<WindowLayout>();
			ExceptionReports = new ExceptionReports();

			ReadyState = ReadyState.New;
		}

		public override String ToString()
		{
			//return Id.ToString().PadLeft(2, '0') + " " + Name;
			return Id + " " + Name;
		}
	}
}
