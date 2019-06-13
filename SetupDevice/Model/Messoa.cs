using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateMessoaSubStreamConfig()
        {
            if (Camera.Profile.StreamConfigs.ContainsKey(2))
            {
                ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            }

            _videoControl2.UpdateSettingToEditComponent(Camera.Model);
            _videoControl2.UpdateSettingContent();

            if (Camera.Profile.StreamConfigs.ContainsKey(3))
            {
                ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            }

            _videoControl3.UpdateSettingToEditComponent(Camera.Model);
            _videoControl3.UpdateSettingContent();
        }

        private void MessoaPrimaryStreamOnCompressChange()
        {
            _isHandlePrimaryStreamEvent = false;

            if (Camera.Profile.StreamConfigs.ContainsKey(1))
            {
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            }
            _videoControl1.UpdateSettingToEditComponent(Camera.Model);
            _videoControl1.UpdateSettingContent();

            UpdateMessoaSubStreamConfig();

            _isHandlePrimaryStreamEvent = true;
        }
    }
}
