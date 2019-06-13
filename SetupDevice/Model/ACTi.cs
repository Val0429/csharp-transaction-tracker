using DeviceCab;
using DeviceConstant;

namespace SetupDevice
{
	public partial class EditPanel
	{
		private void UpdateACTiSubStreamConfig()
		{
			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Compression = primyConfig.Compression;
						streamConfig.Value.Framerate = primyConfig.Framerate;
					}

					_videoControl2.UpdateCompressionContent();
					_videoControl3.UpdateCompressionContent();
					_videoControl4.UpdateCompressionContent();
					_videoControl5.UpdateCompressionContent();
					_videoControl6.UpdateCompressionContent();

					_videoControl2.UpdateFramerate(Camera.Model);
					_videoControl3.UpdateFramerate(Camera.Model);
					_videoControl4.UpdateFramerate(Camera.Model);
					_videoControl5.UpdateFramerate(Camera.Model);
					_videoControl6.UpdateFramerate(Camera.Model);

					_videoControl2.UpdateFramerateContent();
					_videoControl3.UpdateFramerateContent();
					_videoControl4.UpdateFramerateContent();
					_videoControl5.UpdateFramerateContent();
					_videoControl6.UpdateFramerateContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Compression = primyConfigInFourVga.Compression;
						streamConfig.Value.Framerate = primyConfigInFourVga.Framerate;
					}

					_videoControl2.UpdateCompressionContent();
					_videoControl3.UpdateCompressionContent();
					_videoControl4.UpdateCompressionContent();
				  
					_videoControl2.UpdateFramerate(Camera.Model);
					_videoControl3.UpdateFramerate(Camera.Model);
					_videoControl4.UpdateFramerate(Camera.Model);
				   
					_videoControl2.UpdateFramerateContent();
					_videoControl3.UpdateFramerateContent();
					_videoControl4.UpdateFramerateContent();
					break;

				default:
					if (Camera.Profile.StreamConfigs.ContainsKey(2))
					{
						ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
						ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					}

					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigQuality()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.VideoQuality = primyConfig.VideoQuality;
					}

					_videoControl2.UpdateQualityContent();
					_videoControl3.UpdateQualityContent();
					_videoControl4.UpdateQualityContent();
					_videoControl5.UpdateQualityContent();
					_videoControl6.UpdateQualityContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.VideoQuality = primyConfigInFourVga.VideoQuality;
					}

					_videoControl2.UpdateQualityContent();
					_videoControl3.UpdateQualityContent();
					_videoControl4.UpdateQualityContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigProtocol()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionProtocol = primyConfig.ConnectionProtocol;
					}

					_videoControl2.UpdateConnectionProtocolContent();
					_videoControl3.UpdateConnectionProtocolContent();
					_videoControl4.UpdateConnectionProtocolContent();
					_videoControl5.UpdateConnectionProtocolContent();
					_videoControl6.UpdateConnectionProtocolContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionProtocol = primyConfigInFourVga.ConnectionProtocol;
					}

					_videoControl2.UpdateConnectionProtocolContent();
					_videoControl3.UpdateConnectionProtocolContent();
					_videoControl4.UpdateConnectionProtocolContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigFrameRate()
		{
			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Framerate = primyConfig.Framerate;
					}

					_videoControl2.UpdateFramerateContent();
					_videoControl3.UpdateFramerateContent();
					_videoControl4.UpdateFramerateContent();
					_videoControl5.UpdateFramerateContent();
					_videoControl6.UpdateFramerateContent();
					break;

				case CameraMode.FourVga:
					if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Framerate = primyConfigInFourVga.Framerate;
					}

					_videoControl2.UpdateFramerateContent();
					_videoControl3.UpdateFramerateContent();
					_videoControl4.UpdateFramerateContent();
					break;

				default:
					if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;
					ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					//no match resolution in this condition, change sub stream compression
					if (Camera.Profile.StreamConfigs[2].Resolution == Resolution.NA)
					{
						ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);

						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					}
					ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigBitRate()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Bitrate = primyConfig.Bitrate;
					}

					_videoControl2.UpdateBitrateContent();
					_videoControl3.UpdateBitrateContent();
					_videoControl4.UpdateBitrateContent();
					_videoControl5.UpdateBitrateContent();
					_videoControl6.UpdateBitrateContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.Bitrate = primyConfigInFourVga.Bitrate;
					}

					_videoControl2.UpdateBitrateContent();
					_videoControl3.UpdateBitrateContent();
					_videoControl4.UpdateBitrateContent();
					break;

			}
		}

		private void UpdateACTiStreamConfigControlPort()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Control = primyConfig.ConnectionPort.Control;
					}

					_videoControl2.UpdateControlPortContent();
					_videoControl3.UpdateControlPortContent();
					_videoControl4.UpdateControlPortContent();
					_videoControl5.UpdateControlPortContent();
					_videoControl6.UpdateControlPortContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Control = primyConfigInFourVga.ConnectionPort.Control;
					}

					_videoControl2.UpdateControlPortContent();
					_videoControl3.UpdateControlPortContent();
					_videoControl4.UpdateControlPortContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigStreamingPort()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Streaming = primyConfig.ConnectionPort.Streaming;
					}

					_videoControl2.UpdateStreamingPortContent();
					_videoControl3.UpdateStreamingPortContent();
					_videoControl4.UpdateStreamingPortContent();
					_videoControl5.UpdateStreamingPortContent();
					_videoControl6.UpdateStreamingPortContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Streaming = primyConfigInFourVga.ConnectionPort.Streaming;
					}

					_videoControl2.UpdateStreamingPortContent();
					_videoControl3.UpdateStreamingPortContent();
					_videoControl4.UpdateStreamingPortContent();
					break;
			}
		}

		private void UpdateACTiStreamConfigRtspPort()
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(1)) return;

			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					var primyConfig = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Rtsp = primyConfig.ConnectionPort.Rtsp;
					}

					_videoControl2.UpdateRtspPortContent();
					_videoControl3.UpdateRtspPortContent();
					_videoControl4.UpdateRtspPortContent();
					_videoControl5.UpdateRtspPortContent();
					_videoControl6.UpdateRtspPortContent();
					break;

				case CameraMode.FourVga:
					var primyConfigInFourVga = Camera.Profile.StreamConfigs[1];
					foreach (var streamConfig in Camera.Profile.StreamConfigs)
					{
						if (streamConfig.Key == 1) continue;
						streamConfig.Value.ConnectionPort.Rtsp = primyConfigInFourVga.ConnectionPort.Rtsp;
					}

					_videoControl2.UpdateRtspPortContent();
					_videoControl3.UpdateRtspPortContent();
					_videoControl4.UpdateRtspPortContent();
					break;
			}
		}

		private void ACTiOnAspectRatioChange()
		{
			ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[1], 1);
			_videoControl1.UpdateResolution(Camera.Model);
			_videoControl1.UpdateResolutionContent();

			if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

			ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
			_videoControl2.UpdateResolution(Camera.Model);
			_videoControl2.UpdateResolutionContent();
		}
		
		private void ACTiPrimaryStreamOnCompressChange()
		{
			_isHandlePrimaryStreamEvent = false;

			UpdateACTiSubStreamConfig();

			_isHandlePrimaryStreamEvent = true;
		}
	}
}
