using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void AxisOnAspectRatioCorrectionChange()
        {
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateResolution(Camera.Model);
            _videoControl1.UpdateResolutionContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateResolution(Camera.Model);
            _videoControl2.UpdateResolutionContent();
        }
    }
}
