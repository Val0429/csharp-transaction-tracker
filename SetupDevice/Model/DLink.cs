using System;
using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void DLinkOnAspectRatioChange()
        {
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateResolution(Camera.Model);
            _videoControl1.UpdateResolutionContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateResolution(Camera.Model);
            _videoControl2.UpdateResolutionContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateResolution(Camera.Model);
            _videoControl3.UpdateResolutionContent();
        }

        private void DLinkPrimaryStreamOnCompressChange()
        {
            _isHandlePrimaryStreamEvent = false;
            if (Camera.Model.Type == "DynaColor")
            {
                UpdateDynaColorSubStreamConfig(1);
            }
            _isHandlePrimaryStreamEvent = true;
        }

        private void DLinkStreamConfigBitrate(UInt16 streamId)
        {
            if (!Camera.Profile.StreamConfigs.ContainsKey(streamId)) return;

            switch (streamId)
            {
                case 1:
                    ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
                    _videoControl1.UpdateBitrate(Camera.Model);
                    _videoControl1.UpdateBitrateContent();
                    break;

                case 2:
                    ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                    _videoControl2.UpdateBitrate(Camera.Model);
                    _videoControl2.UpdateBitrateContent();
                    break;

                case 3:
                    ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                    _videoControl3.UpdateBitrate(Camera.Model);
                    _videoControl3.UpdateBitrateContent();
                    break;

                case 4:
                    ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                    _videoControl4.UpdateBitrate(Camera.Model);
                    _videoControl4.UpdateBitrateContent();
                    break;
            }

        }
    }
}
