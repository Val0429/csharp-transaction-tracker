using System;
using System.Windows.Forms;
using PanelBase;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        protected Boolean FindNextRecord;
        protected Boolean FindPreviousRecord;

        protected delegate Int64 NextRecordDelegate();

        private bool _previous_e;
        protected void NextRecordButtonMouseDown(Object sender, MouseEventArgs e)
        {
            if (FindNextRecord) return;

            FindNextRecord = true;
            FindPreviousRecord = false;
            Stop();

            _previous_e = e != null;
            NextRecordDelegate nextRecordDelegate = TrackerContainer.NextRecord;
            nextRecordDelegate.BeginInvoke(NextRecordCallback, nextRecordDelegate);
        }

        protected delegate void NextRecordCallbackDelegate(IAsyncResult result);

        protected void NextRecordCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new NextRecordCallbackDelegate(NextRecordCallback), result);
                }
                catch (Exception)
                {
                }
                return;
            }
            bool e = _previous_e;
            _previous_e = false;

            if (!FindNextRecord) return;
            FindNextRecord = false;

            Int64 nextrecordTicks = ((NextRecordDelegate)result.AsyncState).EndInvoke(result);

            //if (Rate < 0)
            //    Stop();
            Stop();
            //timeout
            //if (nextrecordTicks == DateTime.MaxValue.Ticks)
            //    return;

            //find nothing
            //new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen },
            if (nextrecordTicks == DateTime.MaxValue.Ticks)
            {
                if (e)
                    TopMostMessageBox.Show(Localization["TimeTrack_NoMoreRecord"], Localization["MessageBox_Information"],
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TrackerContainer.DateTime = new DateTime(nextrecordTicks);
                //if(Rate == 1)
            }
        }

        private delegate Int64 PreviousRecordDelegate();

        protected void PreviousRecordButtonMouseDown(Object sender, MouseEventArgs e)
        {
            if (FindPreviousRecord) return;

            FindNextRecord = false;
            FindPreviousRecord = true;
            Stop();

            _previous_e = e != null;
            PreviousRecordDelegate previousRecordDelegate = TrackerContainer.PreviousRecord;
            previousRecordDelegate.BeginInvoke(PreviousRecordCallback, previousRecordDelegate);
        }

        protected void GotoBeginButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (FindPreviousRecord) return;

            FindNextRecord = false;
            FindPreviousRecord = true;
            Stop();
            PreviousRecordDelegate previousRecordDelegate = TrackerContainer.BeginRecord;
            previousRecordDelegate.BeginInvoke(PreviousRecordCallback, previousRecordDelegate);
        }

        private delegate void PreviousRecordCallbackDelegate(IAsyncResult result);
        private void PreviousRecordCallback(IAsyncResult result)
        {
            bool e = _previous_e;
            _previous_e = false;
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new PreviousRecordCallbackDelegate(PreviousRecordCallback), result);
                }
                catch (Exception)
                {
                }
                return;
            }
            if (!FindPreviousRecord) return;
            FindPreviousRecord = false;

            Int64 previousRecordTicks = ((PreviousRecordDelegate)result.AsyncState).EndInvoke(result);

            //if (Rate > 0)
            //    Stop();
            Stop();
            //timeout
            //if (previousRecordTicks == DateTime.MinValue.Ticks)
            //    return;

            //find nothing
            if (previousRecordTicks == DateTime.MinValue.Ticks)
            {
                if (e)
                    TopMostMessageBox.Show(Localization["TimeTrack_NoMoreRecord"], Localization["MessageBox_Information"],
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TrackerContainer.DateTime = new DateTime(previousRecordTicks);
            }
        }
    }
}
