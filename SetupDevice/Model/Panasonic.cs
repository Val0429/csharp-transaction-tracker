using DeviceCab;
using DeviceConstant;

namespace SetupDevice
{
    public partial class EditPanel
    {
        private void PanasonicOnAspectRatioChange()
        {
            if (Camera.Model.Series == "SW4x8" || Camera.Model.Series == "SFx631L")
            {
                _connectionControl.UpdateMode();
            }
            
            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
            _videoControl1.UpdateConnectionProtocolContent();
            _videoControl1.UpdateResolution(Camera.Model);
            _videoControl1.UpdateResolutionContent();

            _videoControl1.UpdateSettingToEditComponent(Camera.Model);
            _videoControl1.UpdateSettingContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateConnectionProtocolContent();
            _videoControl2.UpdateResolution(Camera.Model);
            _videoControl2.UpdateResolutionContent();

            _videoControl2.UpdateSettingToEditComponent(Camera.Model);
            _videoControl2.UpdateSettingContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateConnectionProtocolContent();
            _videoControl3.UpdateResolution(Camera.Model);
            _videoControl3.UpdateResolutionContent();

            _videoControl3.UpdateSettingToEditComponent(Camera.Model);
            _videoControl3.UpdateSettingContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(4)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            _videoControl4.UpdateConnectionProtocolContent();
            _videoControl4.UpdateResolution(Camera.Model);
            _videoControl4.UpdateResolutionContent();

            _videoControl4.UpdateSettingToEditComponent(Camera.Model);
            _videoControl4.UpdateSettingContent();
        }

        private void PanasonicOnCompressionChange()
        {
            if (Camera.Model.Series != "SW3X6") return;

            if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
            _videoControl2.UpdateConnectionProtocolContent();
            _videoControl2.UpdateResolution(Camera.Model);
            _videoControl2.UpdateResolutionContent();

            _videoControl2.UpdateSettingToEditComponent(Camera.Model);
            _videoControl2.UpdateSettingContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
            _videoControl3.UpdateConnectionProtocolContent();
            _videoControl3.UpdateResolution(Camera.Model);
            _videoControl3.UpdateResolutionContent();

            _videoControl3.UpdateSettingToEditComponent(Camera.Model);
            _videoControl3.UpdateSettingContent();

            if (!Camera.Profile.StreamConfigs.ContainsKey(4)) return;

            ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
            _videoControl4.UpdateConnectionProtocolContent();
            _videoControl4.UpdateResolution(Camera.Model);
            _videoControl4.UpdateResolutionContent();

            _videoControl4.UpdateSettingToEditComponent(Camera.Model);
            _videoControl4.UpdateSettingContent();
        }
    }
}
