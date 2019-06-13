using System;
using Constant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public void AppendLiveDevice(Object sender, EventArgs<String, Object> e)
        {
            if (!String.Equals(e.Value1, "Live"))
                return;
            var LiveParameter = e.Value2 as LiveParameter;
            if (LiveParameter != null && LiveParameter.Nvr != null)
            {
                DropNVR(LiveParameter.Nvr);
            }
            else if (LiveParameter != null && LiveParameter.Device != null)
            {
                AppendDevice(LiveParameter.Device);
            }
        }

        public void AppendPlaybackDevice(Object sender, EventArgs<String, Object> e)
        {
            if (!String.Equals(e.Value1, "Playback"))
                return;
            
            var playbackParameter = e.Value2 as PlaybackParameter;
            if (playbackParameter != null)
                ApplyPlaybackParameter(playbackParameter);

            var transactionListParameter = e.Value2 as TransactionListParameter;
            if (transactionListParameter != null)
                ApplyTransactionListParameter(transactionListParameter);
        }

        //(IDevice device, UInt64 timecode, TimeUnit timeunit)
        private void ApplyPlaybackParameter(PlaybackParameter parameter)
        {
            
            if (parameter == null) return;

            if (parameter.Device != null)
            {
                ClearAll();
                SetLayout(WindowLayouts.LayoutGenerate(1));

                if (OnTimecodeChange != null && parameter.Timecode != 0)
                    OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(parameter.Timecode.ToString())));

                AppendDevice(parameter.Device);
            }
            else if (parameter.Nvr != null)
            {
                ClearAll();
                SetLayout(WindowLayouts.LayoutGenerate(1));

                if (OnTimecodeChange != null && parameter.Timecode != 0)
                    OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(parameter.Timecode.ToString())));

                DropNVR(parameter.Nvr);
            }
        }

        private void ApplyTransactionListParameter(TransactionListParameter parameter)
        {
            if (parameter == null) return;

            var pos = PTS.POS.FindPOSById(parameter.Transaction.POSId);
            if (pos != null)
            {
                ShowGroup(pos);
            }
        }
    }
}
