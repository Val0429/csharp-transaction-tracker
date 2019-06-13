using System;

namespace DeviceConstant
{
	public class CustomStreamConfig
	{
		public Boolean Enable { get; set; }
		public UInt16 HighLayout { get; set; }
		public UInt16 MiddleLayout { get; set; }
		public CustomStreamSetting HighStream { get; set; }
		public CustomStreamSetting MiddleStream { get; set; }
		public CustomStreamSetting LowStream { get; set; }

		public CustomStreamConfig()
		{
			Enable = false;
			HighLayout = 1;
			MiddleLayout = 4;
			HighStream = new CustomStreamSetting
			              	{
			              		StreamId = 1
							};
			MiddleStream = new CustomStreamSetting
			               	{
			               		StreamId = 2
			               	};
			LowStream = new CustomStreamSetting
			            	{
			            		StreamId = 2
			            	};
		}
	}

	public class RemoteStreamConfig
	{
		public Boolean Enable { get; set; }
		public CustomStreamConfig LiveSetting { get; set; }
		public CustomStreamConfig PlaybackSetting { get; set; }

		public RemoteStreamConfig()
		{
			Enable = false;
			LiveSetting = new CustomStreamConfig();
			PlaybackSetting = new CustomStreamConfig();
		}
	}

	public class CustomStreamSetting
	{
		public Boolean Enable { get; set; }
		public UInt16 StreamId { get; set; }
		public Compression Compression { get; set; }
		public Resolution Resolution { get; set; }
		public Bitrate Bitrate { get; set; }
		public UInt16 Quality { get; set; }
		public UInt16 Framerate { get; set; }

		public CustomStreamSetting()
		{
			Enable = false;
			StreamId = 1;
			Compression = Compression.H264;
			Resolution = Resolution.R640X480;
			Bitrate = Bitrate.Bitrate512K;
			Quality = 60;
			Framerate = 0; //not define
		}
	}
}
