using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using Interface;

namespace App_NetworkVideoRecorder
{
	public partial class NetworkVideoRecorder
	{
		private void RestoreLastStartupOption()
		{
			StartupOption.Doamin = Nvr.Credential.Domain;
			StartupOption.Port = Nvr.Credential.Port.ToString();
			StartupOption.Account = Nvr.Credential.UserName;
			StartupOption.Password = Nvr.Credential.Password;
			StartupOption.SSLEnable = Nvr.Credential.SSLEnable;

			StartupOption.SaveSetting();

			if (StartupOption.Enabled == false)
			{
				Activate(Pages.First().Value);
				return;
			}

			//---- Playback Timecode
			if (StartupOption.PageName == "Playback")
			{
				PlaybackTimeCode = StartupOption.TimeCode;
			}

			//---- Page
			if (!String.IsNullOrEmpty(StartupOption.PageName) && Pages.ContainsKey(StartupOption.PageName))
				Activate(Pages[StartupOption.PageName]);
			else
				Activate(Pages.First().Value);

            if(StartupOption.VideoTitleBar)
            {
                
            }

			if (StartupOption.FullScreen)
			{
				Form.Shown += FormShownFullScreen;
				//FullScreen();
			}
			else
			{
				//---- HidePanel
				if (StartupOption.HidePanel)
				{
					foreach (IBlockPanel blockPanel in PageActivated.Layout.BlockPanels)
					{
						if (blockPanel.IsAutoWidth) continue;

						foreach (IControlPanel controlPanel in blockPanel.ControlPanels)
						{
							var panel = (IMinimize) (controlPanel.Control);
							panel.Maximize();
							panel.Minimize();
						}
					}
				}
			}
			//---- TotalBitrate
			if (StartupOption.TotalBitrate != -1)
			{
				SetDefaultBandWidth(StartupOption.TotalBitrate);
			}
		}

		private void FormShownFullScreen(Object sender, EventArgs e)
		{
			Form.Shown -= FormShownFullScreen;
			FullScreen();
		}

		public override void UpdateClientSetting(RestoreClientColumn keyColumn, String value1, List<Int16> value2)
		{
			switch (keyColumn)
			{
				case RestoreClientColumn.Enabled:
					StartupOption.Enabled = (value1 == "true");
					break;

				case RestoreClientColumn.PageName:
					StartupOption.PageName = value1;
					break;

				case RestoreClientColumn.FullScreen:
					StartupOption.FullScreen = (value1 == "true");
					break;

				case RestoreClientColumn.HidePanel:
					StartupOption.HidePanel = (value1 == "true");
					break;

				case RestoreClientColumn.TotalBitrate:
					StartupOption.TotalBitrate = Convert.ToInt16(value1);
					break;

				case RestoreClientColumn.ViewTour:
					StartupOption.GroupPatrol = (value1 == "true");
					break;

				case RestoreClientColumn.TourItem:
					StartupOption.TourItem = Convert.ToUInt16(value1);
					break;

				case RestoreClientColumn.DeviceGroup:
					StartupOption.DeviceGroup = value1;
					break;

				case RestoreClientColumn.Layout:
					StartupOption.Layout = value1;
					break;

				case RestoreClientColumn.Items:
					StartupOption.Items = value1;
					break;

				case RestoreClientColumn.StreamProfileId:
					StartupOption.StreamProfileId = value2;
					break;

				case RestoreClientColumn.TimeCode:
					StartupOption.TimeCode = Convert.ToUInt64(value1);
					break;

				case RestoreClientColumn.PlaySpeed:
					StartupOption.PlaySpeed = Convert.ToSingle(value1);
					break;

				case RestoreClientColumn.PlayDirection:
					StartupOption.PlayDirection = Convert.ToInt16(value1);
					break;
			}

			StartupOption.SaveSetting();
		}

	}
}
