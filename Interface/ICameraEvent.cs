using System;
using Constant;

namespace Interface
{
    public interface ICameraEvent
    {
        IDevice Device { get; set; }
        INVR NVR { get; set; }
        EventType Type { get; set; }
        DateTime DateTime { get; set; }
        UInt16 Id { get; set; }
        Boolean Value { get; set; }
		String Status { get; set; }
        String ToString();
    }
}