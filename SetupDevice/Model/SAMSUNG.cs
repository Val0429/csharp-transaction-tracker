using DeviceCab;
using DeviceConstant;

namespace SetupDevice
{
	public partial class EditPanel
	{
		private void UpdateSAMSUNGSubStreamConfig()
		{
            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;
			switch (Camera.Mode)
			{
				case CameraMode.Dual:
					if (Camera.Profile.StreamConfigs.ContainsKey(2))
					{
                        ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					}

					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingContent();

			        break;
			}
		}

        private void SAMSUNGPrimaryStreamOnCompressChange()
        {
            _isHandlePrimaryStreamEvent = false;
            UpdateSAMSUNGSubStreamConfig();
            _isHandlePrimaryStreamEvent = true;
        }
	}
}
