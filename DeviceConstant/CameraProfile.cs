using System;
using System.Collections.Generic;
using Constant;

namespace DeviceConstant
{
	public class CameraProfile
	{
		//Order by "Alphabetical"

		public AspectRatio AspectRatio = AspectRatio.NonSpecific;

		public Boolean AspectRatioCorrection;

        public Boolean RemoteRecovery;

		public UInt16 AudioOutPort;

		public Authentication Authentication = new Authentication();

		public CaptureCardConfig CaptureCardConfig;

		public UInt32 ConnectionTimeout = 15000;

		public UInt16 HttpPort = 80;

		//public String LanIp;

		//public String MulticastIp;

		public String NetworkAddress;

		public PowerFrequency PowerFrequency = PowerFrequency.NonSpecific;
        public DeviceMountType DeviceMountType = DeviceMountType.NonSpecific;

		public UInt32 ReceiveTimeout = 15000;

		public SensorMode SensorMode = SensorMode.NonSpecific;

		public UInt16 StreamId = 1;
		public UInt16 RecordStreamId = 1;
		public Dictionary<UInt16, StreamConfig> StreamConfigs = new Dictionary<UInt16, StreamConfig>();

		public TvStandard TvStandard = TvStandard.NonSpecific;

		public String URI;
		public PtzCommand PtzCommand = new PtzCommand();
		//public String WanIp;

		public String DewarpType = ""; // A0**V,  A8TRT,  A1UST

		public UInt16 HighProfile = 1;
		public UInt16 MediumProfile = 1;
		public UInt16 LowProfile = 1;

	    public String LiveCheckURI;
	    public UInt64 LiveCheckInterval;
	    public UInt64 LiveCheckRetryCount;
	}
}
