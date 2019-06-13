using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using Interface;

namespace TimeTrack
{
    partial class TimeTrack
    {

        private void UtilityOnAxisJogDialRotate(object sender, EventArgs<AxisJogDialEvent> e)
        {
            // Workaround for double Timetrack bug
            if (this.Size.Width < 0) return;
            AxisJogDialEvent @event = e.Value;

            Invoke((MethodInvoker)delegate()
            {
                TimeTrackControllerOnPlayRateChanged(this, new EventArgs<float>(@event.Direction / 2.0f));
            });
        }

        private void UtilityOnAxisJogDialShuttle(object sender, EventArgs<AxisJogDialEvent> e)
        {
            // Workaround for double Timetrack bug
            if (this.Size.Width < 0) return;
            AxisJogDialEvent @event = e.Value;

            // -8~-1 0~7
            Invoke((MethodInvoker)delegate()
            {
                var direction = @event.Direction >= 0 ? @event.Direction + 1 : @event.Direction;
                //if (direction > 0)
                //    TimeTrackController.FastPlaySpeed = direction;
                //else
                //    TimeTrackController.FastReverseSpeed = direction;

                TimeTrackControllerOnPlayRateChanged(this, new EventArgs<float>(@event.Direction >= 0 ? @event.Direction + 1 : @event.Direction));
            });
        }

        private void UtilityOnAxisJogDialButtonDown(object sender, EventArgs<AxisJogDialButton> e)
        {
            // Workaround for double Timetrack bug
            if (this.Size.Width < 0) return;
            AxisJogDialButton button = e.Value;

            Invoke((MethodInvoker)delegate()
            {
                switch (button)
                {
                    case AxisJogDialButton.PlayPause:
                        if (Rate == 0)
                            Play();
                        else
                            Stop();
                        break;
                    case AxisJogDialButton.Previous:
                        PreviousRecordButtonMouseDown(this, null);
                        break;
                    case AxisJogDialButton.Next:
                        NextRecordButtonMouseDown(this, null);
                        break;
                    case AxisJogDialButton.Flag:
                        TrackerContainer.AddBookmark();
                        break;
                }
            });
        }
    }
}
