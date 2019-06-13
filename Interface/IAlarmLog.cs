using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constant;

namespace Interface
{
    public interface IAlarmLog
    {
        String Id { get; set; }

        KeyValuePair<UInt16, String> Catalog { get; set; }
        KeyValuePair<UInt16, String> Category { get; set; }
        IServer Server { get; set; }
        IDevice Device { get; set; }

        DateTime AlarmTime { get; set; }
        String AlarmType { get; set; }
        AlarmStatus Status { get; set; }
        Int16 Severity { get; set; }
        String Description { get; set; }

        List<IAlarmAction> Actions { get; set; }
    }
}
