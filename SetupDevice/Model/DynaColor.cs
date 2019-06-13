using System;
using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateDynaColorSubStreamConfig(UInt16 streamId)
        {
            if (Camera.Profile.StreamConfigs.ContainsKey(2) && streamId < 2)
            {
                ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);

                _videoControl2.UpdateSettingToEditComponent(Camera.Model);
                //_videoControl2.UpdateFramerate(Camera.Model);
                _videoControl2.UpdateSettingContent();
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(3) && streamId < 3)
            {
                ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);

                _videoControl3.UpdateSettingToEditComponent(Camera.Model);
                //_videoControl3.UpdateFramerate(Camera.Model);
                _videoControl3.UpdateSettingContent();
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(4))
            {
                ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);

                _videoControl4.UpdateSettingToEditComponent(Camera.Model);
                //_videoControl4.UpdateFramerate(Camera.Model);
                _videoControl4.UpdateSettingContent();
            }
        }
    }
}
