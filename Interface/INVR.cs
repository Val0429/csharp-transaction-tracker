using System;
using System.Collections.Generic;
using Constant;

namespace Interface
{
	public interface INVR : IServer
	{
		event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;
		event EventHandler<EventArgs<List<ICamera>>> OnCameraStatusReceive;

		event EventHandler<EventArgs<IDevice>> OnDeviceModify;
		event EventHandler<EventArgs<IDeviceGroup>> OnGroupModify;
		event EventHandler<EventArgs<IDeviceLayout>> OnDeviceLayoutModify;
		event EventHandler<EventArgs<ISubLayout>> OnSubLayoutModify;
		
		event EventHandler OnDevicePackUpdateCompleted;

		Boolean IsListenEvent { get; set; }

		//Failover property
		FailoverSetting FailoverSetting { get; set; }
		UInt64 ModifiedDate { get; set; }

		// Tulip NVR support Map
		//Dictionary<String, MapAttribute> Maps { get; }
		//Bitmap GetMap(String filename);
		Dictionary<UInt16, IDevice> TempDevices { get; }
		void AddDevicePermission();
		void SilentLoad();
		void UndoReload();
		//void SilentSave();
		void StopTimer();

		void UtilityOnServerEventReceive(String msg);
		void UtilityOnUploadProgress(Int32 progress);
		void DeletePresetPointRelativeEventHandle(ICamera device, UInt16 pointId);

		void DeviceLayoutModify(IDeviceLayout deviceLayout);
		void SubLayoutModify(ISubLayout subLayout);
        //void CalculatorVideoStreamSetting();
	}
}
