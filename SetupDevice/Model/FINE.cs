using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateFINEStreamConfigFrameRate()
        {
            if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateFramerate(Camera.Model);
            _videoControl1.UpdateFramerateContent();

            ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateBitrate(Camera.Model);
            _videoControl1.UpdateBitrateContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateFramerate(Camera.Model);
            _videoControl2.UpdateFramerateContent();

            ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateBitrate(Camera.Model);
            _videoControl2.UpdateBitrateContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3 );
            _videoControl3.UpdateResolution(Camera.Model);
            _videoControl3.UpdateResolutionContent();

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateFramerate(Camera.Model);
            _videoControl3.UpdateFramerateContent();

            ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateBitrate(Camera.Model);
            _videoControl3.UpdateBitrateContent();
        }
    }
}
