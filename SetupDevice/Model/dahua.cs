using System;
using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateDahuaStreamConfigFrameRate(UInt16 streamId)
        {
            //share with GoodWill
            if (Camera.Profile.StreamConfigs.ContainsKey(1) && streamId == 1)
            {
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);

                _videoControl1.UpdateSettingToEditComponent(Camera.Model);
                _videoControl1.UpdateSettingContent();
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(2) && streamId == 2)
            {
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                
                _videoControl2.UpdateSettingToEditComponent(Camera.Model);
                _videoControl2.UpdateSettingContent();
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(3) && streamId == 3)
            {
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
         
                _videoControl3.UpdateSettingToEditComponent(Camera.Model);
                _videoControl3.UpdateSettingContent();
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(4) && streamId == 4)
            {
                ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
         
                _videoControl4.UpdateSettingToEditComponent(Camera.Model);
                _videoControl4.UpdateSettingContent();
            }

        }
    }
}
