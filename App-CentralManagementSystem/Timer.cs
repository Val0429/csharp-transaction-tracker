using System;

namespace App_CentralManagementSystem
{
	public partial class CentralManagementSystem
	{
		private readonly System.Timers.Timer _tickDateTimeTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _checkAllNVRTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _tickCpuTimer = new System.Timers.Timer();
		private void InitialTimer()
		{
			_tickDateTimeTimer.Elapsed += TickDateTimeTimer;
			_tickDateTimeTimer.Interval = 1000;
			_tickDateTimeTimer.SynchronizingObject = Form;

            _checkAllNVRTimer.Elapsed += CheckAllNVRTimerElapsed;
            _checkAllNVRTimer.Interval = 30000;
            _checkAllNVRTimer.SynchronizingObject = Form;

            _tickCpuTimer.Elapsed += TickCPU;
            _tickCpuTimer.Interval = 5000;//5sec
            _tickCpuTimer.SynchronizingObject = Form; 
		}

        private void CheckAllNVRTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CMS.Utility.GetAllNVRStatus();
        }

		private void TickDateTimeTimer(Object sender, EventArgs e)
		{
			_timePanel.Invalidate();

            if (CMS.Configure.AutoLockApplicationTimer == 0) return; // Disabled
            if (IdleTimer == -1) return;  // Unlock in Progress
            if (IsLock) return; // Locked

            IdleTimer++;

            if (IdleTimer > CMS.Configure.AutoLockApplicationTimer)
            {
                _isLock = true;

                LockApplication();
                UpdateMenuVisible();
            }

            Console.WriteLine(IdleTimer);
		}

        private void TickCPU(Object sender, EventArgs e)
        {
            _cpuPanel.Invalidate();
        }
	}
}
