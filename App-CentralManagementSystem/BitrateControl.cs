﻿using System;
using System.Collections.Generic;
using System.Linq;
using DeviceConstant;
using Interface;
using Constant;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem
	{
		private Int32 _viewingDeviceNumber = 0;

		public void ViewingDeviceNumberChange(Object sender, EventArgs<Int32> e)
		{
			//same device number, config is the same
			if (_viewingDeviceNumber == e.Value) return;

			_viewingDeviceNumber = e.Value;

			//not enable, do nothing
			if (!CMS.Configure.CustomStreamSetting.Enable) return;
            if (CMS.Configure.EnableBandwidthControl) return;

			CalculatorVideoStreamSetting();
		}

		private void CalculatorVideoStreamSetting()
		{
			var totalBitrate = Convert.ToInt32(Bitrates.ToString(_totalBitrate));
			var count = Math.Max(_viewingDeviceNumber, 1);

			var shareBitrate = totalBitrate / count;
			if (shareBitrate >= 1000)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate1M;
			}
			else if (shareBitrate >= 500)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate500K;
			}
			else if (shareBitrate >= 256)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
			}
			else if (shareBitrate >= 128)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R240X180;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate128K;
			}
			else if (shareBitrate >= 56)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R160X120;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;
			}
			else if (shareBitrate >= 20)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R160X80;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate20K;
			}
			else if (shareBitrate >= 10)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R120X80;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate10K;
			}
			else if (shareBitrate >= 5)
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R90X60;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate5K;
			}
			else
			{
                CMS.Configure.CustomStreamSetting.Resolution = Resolution.R60X40;
                CMS.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate2K;
			}

			//will cause VideoMonitor Reconnect
			RaiseOnCustomVideoStream();
		}

        private List<INVR> _viewNVRList = new List<INVR>();
        private List<IDevice> _viewDeviceList = new List<IDevice>();
        public void ViewingDeviceListChange(Object sender, EventArgs<List<IDevice>> e)
        {
            //same device number, config is the same
            //if (_viewDeviceList.Count == e.Value.Count) return;

            _viewingDeviceNumber = e.Value.Count;
            _viewDeviceList = e.Value;

            //not enable, do nothing
            if (!CMS.Configure.EnableBandwidthControl) return;

            _viewNVRList.Clear();
            foreach (IDevice device in e.Value)
            {
                if(device == null) continue;
                var nvr = device.Server as INVR;
                if(nvr == null) continue;
                if (_viewNVRList.Contains(nvr)) continue;
                if(nvr.Configure.BandwidthControlBitrate == Bitrate.NA) continue;
                _viewNVRList.Add(nvr);
            }

            foreach (INVR nvr in _viewNVRList)
            {
                CalculatorVideoStreamSetting(nvr);
            }

            //CalculatorVideoStreamSetting();
        }

        private void CalculatorVideoStreamSetting(INVR nvr)
        {
            var totalBitrate = Convert.ToInt32(Bitrates.ToString(nvr.Configure.BandwidthControlBitrate));
            var belongNVRDeviceCount = _viewDeviceList.Count(device => device != null && device.Server == nvr);

            var count = Math.Max(belongNVRDeviceCount, 1);
            //Console.WriteLine(@"==============Device count = "+ count);
            var shareBitrate = totalBitrate / count;
            if (shareBitrate >= 1000)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate1M;
            }
            else if (shareBitrate >= 500)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R640X480;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate500K;
            }
            else if (shareBitrate >= 256)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R320X240;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate256K;
            }
            else if (shareBitrate >= 128)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R240X180;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate128K;
            }
            else if (shareBitrate >= 56)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R160X120;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate56K;
            }
            else if (shareBitrate >= 20)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R160X80;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate20K;
            }
            else if (shareBitrate >= 10)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R120X80;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate10K;
            }
            else if (shareBitrate >= 5)
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R90X60;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate5K;
            }
            else
            {
                nvr.Configure.CustomStreamSetting.Resolution = Resolution.R60X40;
                nvr.Configure.CustomStreamSetting.Bitrate = Bitrate.Bitrate2K;
            }

            nvr.Configure.CustomStreamSetting.StreamId = nvr.Configure.BandwidthControlStream;
            //Console.WriteLine("count: " + count + " , share bitrate: " + shareBitrate);
            //will cause VideoMonitor Reconnect
            RaiseOnCustomVideoStream();
        }
	}
}
