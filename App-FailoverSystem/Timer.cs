using System;

namespace App_FailoverSystem
{
    public partial class FailoverSystem
    {
        private readonly System.Timers.Timer _tickDateTimeTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _refreshFailoverStatusTimer = new System.Timers.Timer();

        private void InitialTimer()
        {
            _tickDateTimeTimer.Elapsed += TickDateTimeTimer;
            _tickDateTimeTimer.Interval = 1000;
            _tickDateTimeTimer.SynchronizingObject = Form; 

            var setting = new Constant.FailoverSetting();
            _refreshFailoverStatusTimer.Elapsed += RefreshFailoverStatusTimerTick;
            _refreshFailoverStatusTimer.Interval = Math.Max(Convert.ToInt32(setting.PingTime), 5000);//5sec
            _refreshFailoverStatusTimer.SynchronizingObject = Form; 
        }

        private void  TickDateTimeTimer(Object sender, EventArgs e)
        {
            _timePanel.Invalidate();
        }

        private void RefreshNVR()
        {
            _fos.NVR.UpdateNVRStatus();
        }

        private void RefreshFailoverStatusTimerTick(Object sender, EventArgs e)
        {
            RefreshNVR();
        }
    }
}
