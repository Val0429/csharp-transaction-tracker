using System;
using System.Collections.Generic;
using DeviceConstant;
using Interface;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
    {
        private Int32 _viewingDeviceNumber = 0;

        public void ViewingDeviceNumberChange(Object sender, EventArgs<Int32> e)
        {
            if (Nvr.Configure.EnableBandwidthControl)
            {
                _viewingDeviceNumber = e.Value;
                ViewingDeviceListChange();
                return;
            }
            else
            {
                if (!Nvr.Configure.CustomStreamSetting.Enable && Nvr.Configure.CustomStreamSetting.Bitrate == Bitrate.NA)
                    SetVideoStreamToOriginal(this, null);
            }

            //same device number, config is the same
            if (_viewingDeviceNumber == e.Value) return;

            _viewingDeviceNumber = e.Value;

            //not enable, do nothing
            if (!Nvr.Configure.CustomStreamSetting.Enable) return;

            CalculatorVideoStreamSetting();
        }

        private void CalculatorVideoStreamSetting()
        {
            var totalBitrate = Convert.ToInt32(Bitrates.ToString(_totalBitrate));
            var count = Math.Max(_viewingDeviceNumber, 1);

            var shareBitrate = totalBitrate / count;
            if (shareBitrate >= 1000)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate1M;
            }
            else if (shareBitrate >= 500)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate500K;
            }
            else if (shareBitrate >= 256)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
            }
            else if (shareBitrate >= 128)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R240X180;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate128K;
            }
            else if (shareBitrate >= 56)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R160X120;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;
            }
            else if (shareBitrate >= 20)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R160X80;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate20K;
            }
            else if (shareBitrate >= 10)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R120X80;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate10K;
            }
            else if (shareBitrate >= 5)
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R90X60;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate5K;
            }
            else
            {
                Nvr.Configure.CustomStreamSetting.Resolution = Resolution.R60X40;
                Nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate2K;
            }

            //will cause VideoMonitor Reconnect
            RaiseOnCustomVideoStream();
        }

        private void ViewingDeviceListChange()
        {
            //not enable, do nothing
            if (!Nvr.Configure.EnableBandwidthControl) return;
            if (Nvr.Configure.BandwidthControlBitrate == Bitrate.NA) return;

            _totalBitrate = Nvr.Configure.BandwidthControlBitrate;
            Nvr.Configure.CustomStreamSetting.Enable = true;
            Nvr.Configure.CustomStreamSetting.StreamId = Nvr.Configure.BandwidthControlStream;
            CalculatorVideoStreamSetting();
        }

        public void ViewingDeviceListChange(Object sender, EventArgs<List<IDevice>> e)
        {
            return;
        }
    }
}
