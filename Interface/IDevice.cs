using System;
using Constant;
using DeviceConstant;
using System.Drawing;

namespace Interface
{
	public interface IDevice
	{
		IServer Server { get; set; }
		UInt16 Id { get; set; }
		String Name { get; set; }
		ReadyState ReadyState { get; set; }
		DeviceType DeviceType { get; set; }

		Boolean CheckPermission(Permission permission);
		IDevice CloneDevice();

        String LastPicture { get; set; }
	}
}
