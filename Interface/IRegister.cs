using System;
using System.Collections.Generic;
using Constant;

namespace Interface
{
	public interface IRegister
	{
		Int32 Id { get; set; }
		String Store { get; set; }
		String Station { get; set; }
		String Name { get; set; }
		String IpAddress { get; set; }
		Int32 Layout { get; set; }
		List<Int32> Devices { get; set; }

		ReadyState ReadyState { get; set; }
	}
}
