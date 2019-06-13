using DeviceCab;

namespace SetupDevice
{
	public partial class EditPanel
	{
		private void UpdateBrickcomSubStreamConfig()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

			ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
			ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);

			_videoControl2.UpdateSettingToEditComponent(Camera.Model);
			_videoControl2.UpdateSettingContent();

			if (!Camera.Profile.StreamConfigs.ContainsKey(3))
			{
				_connectionControl.UpdateStreamConfigList();
				return;
			}

			ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
			ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);

			_videoControl3.UpdateSettingToEditComponent(Camera.Model);
			_videoControl3.UpdateSettingContent();
			_connectionControl.UpdateStreamConfigList();
		}
		
		private void BrickcomPrimaryStreamOnCompressChange()
		{
			_isHandlePrimaryStreamEvent = false;
			if (Camera.Model.Series == "DynaColor")
			{
				UpdateDynaColorSubStreamConfig(1);
			}
			else
			{
				UpdateBrickcomSubStreamConfig();
			}
			_isHandlePrimaryStreamEvent = true;
		}
	}
}
