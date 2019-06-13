using System;
using System.Collections.Generic;

namespace Interface
{
	public interface IRestoreClient
    {
        Boolean Enabled { get; set; }
		String PageName { get; set; }

		Boolean FullScreen { get; set; }
		Boolean HideToolbar { get; set; }
		Boolean HidePanel { get; set; }
		Int16 TotalBitrate { get; set; }

		Int16 ViewTour { get; set; }
		Int16 TourItem { get; set; }

		IDeviceGroup DeviceGroup { get; set; }
		String Layout { get; set; }
		List<IDevice> Items { get; set; }
		List<Int16> StreamProfileId { get; set; }

		UInt64 TimeCode { get; set; }
    }
}
