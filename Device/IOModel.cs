using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace Device
{
	public class IOModel : IIOModel
	{
		public ReadyState ReadyState { get; set; }

		public UInt16 Id { get; set; }
		public String Name { get; set; }
		public String Manufacture { get; set; }
		public ServerCredential Credential { get; set; }

		public UInt16 DICount { get; set; }
		public UInt16 DOCount { get; set; }

		public override String ToString()
		{
			return Id.ToString().PadLeft(2, '0') + " " + Name;
		}

		private Dictionary<String, IIOEventHandle> _handles = new Dictionary<String, IIOEventHandle>();
		public Dictionary<String, IIOEventHandle> Handles
		{
			get { return _handles; }
			set { _handles = value; }
		}
	}

	public class IOEventHandle : IIOEventHandle
	{
		public String Name { get; set; }
		public INVR NVR { get; set; }
		public UInt16 Camera { get; set; }
	}
}
