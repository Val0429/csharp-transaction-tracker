using System;
using System.Collections.Generic;

namespace Constant
{
    public class FailoverSetting
    {
        public Boolean ActiveProfile = false;
        public Int16 FailPercent = -1; //0~100% 100% = failure, failover will takeover recording, -1 not data
        public UInt16 FailoverPort = 8801;
        public UInt32 LaunchTime = 60000; // 60sec  30~600
        public UInt32 PingTime = 5000; //5sec fixed
        public UInt32 BlockSize = 4194304; //4MB  4K~8M  1024 base
        //LaunchTime / PingTime = query times,
        //ex 60 / 5 = check nvr alive 12 times, if 12 times ping "ALL" failure, start failover
    }

    public enum FailoverStatus : ushort
    {
        Ping,
        Recording,
        Synchronize,
        MergeDatabase,
    }
}
