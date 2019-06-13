using DeviceCab;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void UpdateZAVIOStreamConfigFrameRate()
        {
            if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateFramerate(Camera.Model);
            _videoControl1.UpdateFramerateContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateFramerate(Camera.Model);
            _videoControl2.UpdateFramerateContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateFramerate(Camera.Model);
            _videoControl3.UpdateFramerateContent();
        }
    }
}
