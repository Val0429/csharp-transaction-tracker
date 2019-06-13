using System;
using Constant;

namespace Interface
{
    public interface IAlarmAction
    {
        String Id { get; set; }

        Int16 No { get; set; }

        String Actioner { get; set; }
        DateTime ActionTime { get; set; }
        AlarmStatus Status { get; set; }
        String Description { get; set; }
    }
}
