using System;

namespace App_VideoAnalyticsServer
{
    public partial class VideoAnalyticsServer
    {
        private readonly System.Timers.Timer _tickDateTimeTimer = new System.Timers.Timer();

        private void InitialTimer()
        {
            _tickDateTimeTimer.Elapsed += TickDateTimeTimer;
            _tickDateTimeTimer.Interval = 1000;
            _tickDateTimeTimer.SynchronizingObject = Form; 
        }

        private void  TickDateTimeTimer(Object sender, EventArgs e)
        {
            _timePanel.Invalidate();
        }
    }
}
