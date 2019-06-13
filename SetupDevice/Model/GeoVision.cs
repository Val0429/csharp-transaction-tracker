using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateGeoVisionSubStreamConfig()
        {
            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);

            _videoControl2.UpdateResolution(Camera.Model);
            _videoControl2.UpdateBitrate(Camera.Model);

            _videoControl2.UpdateResolutionContent();
            _videoControl2.UpdateBitrateContent();
        }
    }
}
