using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceCab;
using DeviceConstant;
using PanelBase;

namespace SetupDevice
{
	public sealed partial class VideoControl : UserControl
	{
		public event EventHandler OnProtocolChange;
		public event EventHandler OnCompressChange;
		public event EventHandler OnResolutionChange;
		public event EventHandler OnFrameRateChange;
		public event EventHandler OnBitRateChange;
		public event EventHandler OnQualityChange;
		public event EventHandler OnControlPortChange;
		public event EventHandler OnStreamPortChange;
		public event EventHandler OnRtspPortChange;
		public event EventHandler OnChannelIdChange;
		public event EventHandler OnMotionThresholdChange;

		public Dictionary<String, String> Localization;
		public EditPanel EditPanel;

		public UInt16 StreamId;

		public VideoControl()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"DevicePanel_VideoStream", "Video Stream"},
								   {"DevicePanel_VideoStreamID", "Video Stream %1"},
								   {"DevicePanel_ChannelID", "Channel ID"},
								   {"DevicePanel_Protocol", "Protocol"},
								   {"DevicePanel_Compression", "Compression"},
								   {"DevicePanel_Resolution", "Resolution"},
								   {"DevicePanel_FPS", "FPS"},
								   {"DevicePanel_Quality", "Quality"},
								   {"DevicePanel_Bitrate", "Bitrate"},
								   {"DevicePanel_ControlPort", "Control Port"},
								   {"DevicePanel_StreamPort", "Stream Port"},
                                   {"DevicePanel_HTTPSPort", "HTTPS Port"},
								   {"DevicePanel_RTSPPort", "RTSP Port"},
								   {"DevicePanel_URI", "URI"},
								   {"DevicePanel_MotionThreshold", "Motion Threshold"},
								   {"DevicePanel_BitrateControl", "Bitrate Control"},
								   {"DevicePanel_MulticastNetworkAddress", "Multicast Network Address"},
								   {"DevicePanel_VideoInPort", "Video In Port"},
								   {"DevicePanel_AudioInPort", "Audio In Port"},
								   {"DevicePanel_ProfileMode", "Profile Mode"},
                                   {"DevicePanel_DewarpMode", "Dewarp Mode"},
								   {"VideoControl_ForRecording", "(For video recording)"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Top;

			for (UInt16 i = 10; i <= 100; i += 10)
				qualityComboBox.Items.Add(i);

			for (UInt16 i = 0; i <= 100; i += 10)
				thresholdComboBox.Items.Add(i);

			//bitrate control
			bitrateControlcomboBox.Items.Add(BitrateControls.ToString(BitrateControl.VBR));
			bitrateControlcomboBox.Items.Add(BitrateControls.ToString(BitrateControl.CBR));

			Paint += VideoControlPaint;

			channelIdPanel.Paint += PaintInput;
			protocolPanel.Paint += PaintInput;
			compressionPanel.Paint += PaintInput;
            dewarpModePanel.Paint += PaintInput;
			resolutionPanel.Paint += PaintInput;
			qualityPanel.Paint += PaintInput;
			fpsPanel.Paint += PaintInput;
			bitratePanel.Paint += PaintInput;
			controlPanel.Paint += PaintInput;
			streamPanel.Paint += PaintInput;
            httpsPanel.Paint += PaintInput;
			rtspPanel.Paint += PaintInput;
			uriPanel.Paint += PaintInput;
			thresholdPanel.Paint += PaintInput;
			bitrateControlPanel.Paint += PaintInput;

			multicastNetwordAddressPanel.Paint += PaintInput;
			videoInPortPanel.Paint += PaintInput;
			audioInPortPanel.Paint += PaintInput;

			profileModePanel.Paint += PaintInput;

			controlPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
			streamPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
			rtspPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
			videoInPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
			audioInPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

			channelIdComboBox.SelectedIndexChanged += ChannelIdComboBoxSelectedIndexChanged;
            dewarpModeComboBox.SelectedIndexChanged += DewarpModeComboBoxSelectedIndexChanged;
			protocolComboBox.SelectedIndexChanged += ProtocolComboBoxSelectedIndexChanged;
			compressionComboBox.SelectedIndexChanged += CompressionComboBoxSelectedIndexChanged;
			resolutionComboBox.SelectedIndexChanged += ResolutionComboBoxSelectedIndexChanged;
			qualityComboBox.SelectedIndexChanged += QualityComboBoxSelectedIndexChanged;
			fpsComboBox.SelectedIndexChanged += FpsComboBoxSelectedIndexChanged;
			bitrateComboBox.SelectedIndexChanged += BitrateComboBoxSelectedIndexChanged;
			controlPortTextBox.TextChanged += ControlPortTextBoxTextChanged;
			streamPortTextBox.TextChanged += StreamPortTextBoxTextChanged;
		    httpsTextBox.TextChanged += HttpsPortTextBoxTextChanged;
			rtspPortTextBox.TextChanged += RtspPortTextBoxTextChanged;
			uriTextBox.TextChanged += URITextBoxTextChanged;
			multicastNetworkAddressTextBox.TextChanged += MulticastNetworkAddressTextBoxTextChanged;
			videoInPortTextBox.TextChanged += VideoInPortTextBoxTextChanged;
			audioInPortTextBox.TextChanged += AudioInPortTextBoxTextChanged;
			thresholdComboBox.SelectedIndexChanged += ThresholdComboBoxSelectedIndexChanged;
			bitrateControlcomboBox.SelectedIndexChanged += BitrateControlcomboBoxSelectedIndexChanged;
			profileModeComboBox.SelectedIndexChanged += ProfileModeComboBoxSelectedIndexChanged;
		}

		private void VideoControlPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			String text = (StreamId == 0)
							  ? Localization["DevicePanel_VideoStream"]
							  : Localization["DevicePanel_VideoStreamID"].Replace("%1", StreamId.ToString());

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Axis":
                    if ((EditPanel.Camera.Mode == CameraMode.Quad) && StreamId == 3)
                    {
                        text += " (QUAD) ";
                    }
                    break;
            }

			if (ShowRecordText)
				text += " " + Localization["VideoControl_ForRecording"];

            g.DrawString(text, SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
		}

		public void ParseDevice()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			Enabled = true;
			if (EditPanel.Camera.Model.Type == "CaptureCard")
			{
				protocolPanel.Visible = false;
			}
			else
			{
				switch (EditPanel.Camera.Model.Manufacture)
				{
					case "ACTi":
						if ((EditPanel.Camera.Mode == CameraMode.SixVga || EditPanel.Camera.Mode == CameraMode.FourVga) && StreamId != 1)
							Enabled = false;
						protocolPanel.Visible = true;
						break;

					case "Messoa":
						//case "DLink":
						protocolPanel.Visible = (StreamId == 1);
						break;

					case "EverFocus":
						rtspPanel.Visible = protocolPanel.Visible = (StreamId == 1);
						break;

                    case "Kedacom":
                        protocolPanel.Visible = false;
                        break;

					default:
						protocolPanel.Visible = true;
						break;
				}
			}

			StreamPanelVisible();
			UpdateChannelContent();
		    UpdateDewarpModeContent();
			UpdateConnectionProtocolContent();
			UpdateCompressionContent();
			UpdateResolutionContent();
			UpdateQualityContent();
			UpdateFramerateContent();
			UpdateBitrateContent();
			UpdateControlPortContent();
			UpdateStreamingPortContent();
		    UpdateHttpsPortContent();
			UpdateRtspPortContent();
			UpdateURIContent();
			UpdateMulticastNetworkAddressContent();
			UpdateVideoInPortContent();
			UpdateAudioInPortContent();
			UpdateThresholdContent();
			UpdateBitrateControlContent();
			URIPanelVisible();
			ThresholdPanelVisible();
			BitrateControlPanelVisible();
			QualityPanelVisible();
			UpdateProfileModeContent();
		}

		private void ChannelIdComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].Channel = Convert.ToUInt16(channelIdComboBox.SelectedItem);

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Axis":
                    if(EditPanel.Camera.Mode == CameraMode.Quad)
                    {
                        foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
                        {
                            if (config.Key == 1) continue;

                            config.Value.Channel = EditPanel.Camera.Profile.StreamConfigs[StreamId].Channel;
                        }
                    }
                    break;
                case "ArecontVision":
                case "BOSCH":
                case "Avigilon":
                    foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
                    {
                        if (config.Key == 1) continue;

                        config.Value.Channel = EditPanel.Camera.Profile.StreamConfigs[StreamId].Channel;
                    }
                    break;
            }
			EditPanel.CameraIsModify();

			if (OnChannelIdChange != null)
				OnChannelIdChange(null, null);
		}

        private void DewarpModeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

            EditPanel.Camera.Profile.StreamConfigs[StreamId].Dewarp = Dewarps.ToIndex(dewarpModeComboBox.SelectedItem.ToString());

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "VIVOTEK":
                case "Axis":
                    ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

                    UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);
                    UpdateBitrate(EditPanel.Camera.Model);

                    UpdateResolutionContent();
					UpdateFramerateContent();
					UpdateBitrateContent();
                    break;
            }

            EditPanel.CameraIsModify();
        }

		private void ProtocolComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
			{
				StreamPanelVisible();
				RtspPanelVisible();
				MulticastPanelVisible();
				return;
			}

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol = ConnectionProtocols.DisplayStringToIndex(protocolComboBox.SelectedItem.ToString());

			switch(EditPanel.Camera.Model.Manufacture)
			{
				case "Axis":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateCompressions(EditPanel.Camera.Model);
					UpdateCompressionContent();

					break;

				case "ArecontVision":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.SetDefaultBitrateControl(EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId]);
					UpdateCompressions(EditPanel.Camera.Model);
					UpdateCompressionContent();

					UpdateBitrateControlContent();
					BitrateControlPanelVisible();
					QualityPanelVisible();
					break;

				case "EverFocus":
					foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
					{
						if (config.Key == 1) continue;

						config.Value.ConnectionProtocol = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol;
					}
					break;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					if (EditPanel.Camera.Model.Series == "DynaColor") 
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateCompressions(EditPanel.Camera.Model);
					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);
					if (EditPanel.Camera.Model.Series == "DynaColor") 
						UpdateBitrate(EditPanel.Camera.Model);

					UpdateCompressionContent();
					UpdateResolutionContent();
					UpdateFramerateContent();
					if (EditPanel.Camera.Model.Series == "DynaColor") 
						UpdateBitrateContent();
					UpdateStreamingPortContent();
                    UpdateHttpsPortContent();
					break;

				case "DLink":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					if (EditPanel.Camera.Model.Type == "DynaColor")
					{
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					}

					UpdateCompressions(EditPanel.Camera.Model);
					if (EditPanel.Camera.Model.Type == "DynaColor")
					{
						UpdateResolution(EditPanel.Camera.Model);
						UpdateFramerate(EditPanel.Camera.Model);
						UpdateBitrate(EditPanel.Camera.Model);
					}

					UpdateCompressionContent();
					if (EditPanel.Camera.Model.Type == "DynaColor")
					{
						UpdateResolutionContent();
						UpdateFramerateContent();
						UpdateBitrateContent();
					}
					break;

                case "Panasonic":
                    ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateCompressions(EditPanel.Camera.Model);
					UpdateCompressionContent();

                    ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
                    UpdateFramerate(EditPanel.Camera.Model);
                    UpdateFramerateContent();
					break;

				case "ACTi":
					if (OnProtocolChange != null)
						OnProtocolChange(null, null);
					break;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateCompressions(EditPanel.Camera.Model);
					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);
					UpdateBitrate(EditPanel.Camera.Model);

					UpdateCompressionContent();
					UpdateBitrateContent();
					UpdateResolutionContent();
					UpdateFramerateContent();
					UpdateStreamingPortContent();
                    UpdateHttpsPortContent();

                    if (OnProtocolChange != null)
                        OnProtocolChange(null, null);
					break;

				case "VIVOTEK":
					ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateCompressions(EditPanel.Camera.Model);
					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);
					UpdateBitrate(EditPanel.Camera.Model);

					UpdateCompressionContent();
					UpdateBitrateContent();
					UpdateResolutionContent();
					UpdateFramerateContent();

					break;
			}

			StreamPanelVisible();
			RtspPanelVisible();
			MulticastPanelVisible();
			EditPanel.CameraIsModify();
		}

		private void CompressionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			if (!EditPanel.IsEditing)
			{
				UpdateFieldByCompression();
				return;
			}

			EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression = Compressions.DisplayStringToIndex(compressionComboBox.SelectedItem.ToString());

			//when change compression, also reload bitrate
			ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
			UpdateBitrate(EditPanel.Camera.Model);
			UpdateBitrateContent();

			EditPanel.CameraIsModify();

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "Axis":
					if (EditPanel.Camera.Model.Model == "Q6035")
					{
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						UpdateFramerate(EditPanel.Camera.Model);
						UpdateFramerateContent();
					}
					break;

				case "ArecontVision":
					ProfileChecker.SetDefaultBitrateControl(EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId]);
					UpdateBitrateControlContent();
					BitrateControlPanelVisible();
					QualityPanelVisible();
					break;

				case "Messoa":
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateFramerate(EditPanel.Camera.Model);
					UpdateBitrate(EditPanel.Camera.Model);

					UpdateFramerateContent();
					UpdateBitrateContent();
					break;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
				case "VIVOTEK":
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateBitrate(EditPanel.Camera.Model);
					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);

					UpdateBitrateContent();
					UpdateResolutionContent();
					UpdateFramerateContent();
					break;

                case "Dahua":
                case "GoodWill":
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
                    ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);
                    UpdateBitrate(EditPanel.Camera.Model);

					UpdateBitrateContent();
					UpdateResolutionContent();
					UpdateFramerateContent();
					break;

				case "ACTi":
                case "Panasonic":
					ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					
					UpdateResolution(EditPanel.Camera.Model);
					UpdateFramerate(EditPanel.Camera.Model);

					UpdateResolutionContent();
					UpdateFramerateContent();
					break;

				case "Brickcom":
                case "VIGZUL":
                case "Certis":
                case "FINE":
					if (EditPanel.Camera.Model.Series == "DynaColor")
					{
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateBitrate(EditPanel.Camera.Model);
						UpdateResolution(EditPanel.Camera.Model);
						UpdateFramerate(EditPanel.Camera.Model);

						UpdateBitrateContent();
						UpdateResolutionContent();
						UpdateFramerateContent();
					}
					else
					{
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateResolution(EditPanel.Camera.Model);
						UpdateFramerate(EditPanel.Camera.Model);

						UpdateResolutionContent();
						UpdateFramerateContent();
					}
					break;

				case "DLink":
					if (EditPanel.Camera.Model.Type == "DynaColor")
					{
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateBitrate(EditPanel.Camera.Model);
						UpdateResolution(EditPanel.Camera.Model);
						UpdateFramerate(EditPanel.Camera.Model);

						UpdateBitrateContent();
						UpdateResolutionContent();
						UpdateFramerateContent();
					}
					break;

				case "Surveon":
					if (EditPanel.Camera.Model.Series == "DynaColor")
					{
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateBitrate(EditPanel.Camera.Model);
						UpdateResolution(EditPanel.Camera.Model);
						UpdateFramerate(EditPanel.Camera.Model);

						UpdateBitrateContent();
						UpdateResolutionContent();
						UpdateFramerateContent();
					}
					else
					{
						ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateResolution(EditPanel.Camera.Model);
						UpdateResolutionContent();
					}
					break;
			}

			UpdateFieldByCompression();
			RtspPanelVisible();

			if (OnCompressChange != null)
				OnCompressChange(null, null);
		}

		private void ResolutionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].Resolution = Resolutions.ToIndex(resolutionComboBox.SelectedItem.ToString());

			EditPanel.CameraIsModify();

			EditPanel.Server.Device.CheckSubLayoutPositionAndResolution(EditPanel.Camera);

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "Messoa":
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateFramerate(EditPanel.Camera.Model);
					UpdateBitrate(EditPanel.Camera.Model);
					
					UpdateFramerateContent();
					UpdateBitrateContent();

					if (OnResolutionChange != null)
						OnResolutionChange(null, null);
					break;

				case "ZAVIO":
                case "Dahua":
                case "GoodWill":
                //case "FINE":
                case "MOBOTIX":
                case "A-MTK":
					if (OnResolutionChange != null)
						OnResolutionChange(null, null);
					break;

				case "GeoVision":
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateBitrate(EditPanel.Camera.Model);
					bitrateComboBox.SelectedItem = Bitrates.ToDisplayString(EditPanel.Camera.Profile.StreamConfigs[StreamId].Bitrate);

					if (OnResolutionChange != null)
						OnResolutionChange(null, null);
					break;

				case "MegaSys":
                case "Avigilon":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
				case "ArecontVision":
				case "Brickcom":
                case "VIGZUL":
                case "Certis":
                case "FINE":
				case "DLink":
                case "Panasonic":
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					UpdateFramerate(EditPanel.Camera.Model);
					UpdateFramerateContent();

					if (OnResolutionChange != null)
						OnResolutionChange(null, null);
					break;

				case "ACTi":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
                case "DivioTec":
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateFramerate(EditPanel.Camera.Model);
					UpdateFramerateContent();

					if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Mjpeg)
					{
						UpdateBitrate(EditPanel.Camera.Model);
						UpdateBitrateContent();
					}
					if (OnResolutionChange != null)
						OnResolutionChange(null, null);
					break;

				case "Surveon":
					if (EditPanel.Camera.Model.Series == "DynaColor")
					{
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						UpdateFramerate(EditPanel.Camera.Model);
						UpdateFramerateContent();

						if (OnResolutionChange != null)
							OnResolutionChange(null, null);
					}
					else
					{
						ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
						ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

						UpdateFramerate(EditPanel.Camera.Model);
						UpdateFramerateContent();

						if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Mjpeg)
						{
							UpdateBitrate(EditPanel.Camera.Model);
							UpdateBitrateContent();
						}
					}
					break;

				//case "DLink":
				//    ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
				//    UpdateFramerate(EditPanel.Camera.Model);
				//    UpdateFramerateContent();
				//    break;

				case "VIVOTEK":
					ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);
					ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[StreamId], StreamId);

					UpdateFramerate(EditPanel.Camera.Model);
					UpdateFramerateContent();

					UpdateBitrate(EditPanel.Camera.Model);
					UpdateBitrateContent();
					break;
			}
		}

		private void FpsComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].Framerate = Convert.ToUInt16(fpsComboBox.SelectedItem);

			EditPanel.CameraIsModify();
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
				case "Brickcom":
                case "VIGZUL":
				case "DLink":
                case "Panasonic":
				case "Surveon":
				case "Certis":
                case "FINE":
                case "Dahua":
                case "GoodWill":
					if (OnFrameRateChange != null)
						OnFrameRateChange(null, null);
					break;
			}
		}

		private void QualityComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].VideoQuality = Convert.ToUInt16(qualityComboBox.SelectedItem);

			EditPanel.CameraIsModify();
			
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
					if (OnQualityChange != null)
						OnQualityChange(null, null);
					break;
			}
		}

		private void BitrateComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].Bitrate = Bitrates.DisplayStringToIndex(bitrateComboBox.SelectedItem.ToString());

			EditPanel.CameraIsModify();
			
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
					if (OnBitRateChange != null)
						OnBitRateChange(null, null);
					break;
			}
		}

		private void ControlPortTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			UInt32 port = (controlPortTextBox.Text != "") ? Convert.ToUInt32(controlPortTextBox.Text) : 6001;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Control = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

			EditPanel.CameraIsModify();

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
					if (OnControlPortChange != null)
						OnControlPortChange(null, null);
					break;
			}
		}

        private void HttpsPortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

            UInt32 port = (httpsTextBox.Text != "") ? Convert.ToUInt32(httpsTextBox.Text) : 443;

            EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Https = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            EditPanel.CameraIsModify();
        }

		private void StreamPortTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			UInt32 port = (streamPortTextBox.Text != "") ? Convert.ToUInt32(streamPortTextBox.Text) : 6002;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Streaming = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

			EditPanel.CameraIsModify();

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
					if (OnStreamPortChange != null)
						OnStreamPortChange(null, null);
					break;
			}
		}

		private void RtspPortTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			UInt32 port = (rtspPortTextBox.Text != "") ? Convert.ToUInt32(rtspPortTextBox.Text) : 554;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Rtsp = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

			EditPanel.CameraIsModify();

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
				case "EverFocus":
					foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
					{
						if(config.Key == 1) continue;

						config.Value.ConnectionPort.Rtsp = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Rtsp;
					}
					break;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (EditPanel.Camera.Model.Series != "DynaColor") return;
				   
					foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
					{
						if (config.Key == 1) continue;

						config.Value.ConnectionPort.Rtsp = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Rtsp;
					}
					break;

                case "DLink":
                    if (EditPanel.Camera.Model.Type != "DynaColor") return;

                    foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
                    {
                        if (config.Key == 1) continue;

                        config.Value.ConnectionPort.Rtsp = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Rtsp;
                    }
                    break;

				case "ACTi":
					if (OnRtspPortChange != null)
						OnRtspPortChange(null, null);
					break;
			}
		}

		private void URITextBoxTextChanged(object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].URI =uriTextBox.Text;

			EditPanel.CameraIsModify();
		}

		private void MulticastNetworkAddressTextBoxTextChanged(object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].MulticastNetworkAddress = multicastNetworkAddressTextBox.Text;

			EditPanel.CameraIsModify();
		}

		private void VideoInPortTextBoxTextChanged(object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.VideoIn = (ushort) (String.IsNullOrEmpty(videoInPortTextBox.Text.Trim()) ? 0 : Convert.ToUInt16(videoInPortTextBox.Text));

			EditPanel.CameraIsModify();
		}

		private void AudioInPortTextBoxTextChanged(object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.AudioIn = (ushort)(String.IsNullOrEmpty(audioInPortTextBox.Text.Trim()) ? 0 : Convert.ToUInt16(audioInPortTextBox.Text));

			EditPanel.CameraIsModify();
		}

		private void ThresholdComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ArecontVision":
					break;

				default:
					return;
			}
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].MotionThreshold = Convert.ToUInt16(thresholdComboBox.SelectedItem);

			EditPanel.CameraIsModify();

			if (OnMotionThresholdChange != null)
				OnMotionThresholdChange(this, null);
		}

		private void BitrateControlcomboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ArecontVision":
					break;

				default:
					return;
			}

			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			EditPanel.Camera.Profile.StreamConfigs[StreamId].BitrateControl =
				BitrateControls.ToIndex(bitrateControlcomboBox.SelectedItem.ToString());

			QualityPanelVisible();
			EditPanel.CameraIsModify();
		}

		private void ProfileModeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing || !EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "BOSCH":
					foreach (KeyValuePair<UInt16, String> streamMode in ((BOSCHCameraModel)EditPanel.Camera.Model).StreamProfileMode)
					{
					   if(String.Equals(streamMode.Value, profileModeComboBox.SelectedItem.ToString()))
					   {
						   EditPanel.Camera.Profile.StreamConfigs[StreamId].ProfileMode = streamMode.Key;
						   break;
					   }
					}
						
					break;
			}

			EditPanel.CameraIsModify();
		}

		public void UpdateSettingContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			UpdateChannelContent();
		    UpdateDewarpModeContent();
			UpdateConnectionProtocolContent();
			UpdateCompressionContent();
			UpdateResolutionContent();
			UpdateQualityContent();
			UpdateFramerateContent();
			UpdateBitrateContent();
			UpdateControlPortContent();
			UpdateStreamingPortContent();
            UpdateHttpsPortContent();
			UpdateRtspPortContent();
			UpdateMulticastNetworkAddressContent();
			UpdateVideoInPortContent();
			UpdateAudioInPortContent();
			UpdateProfileModeContent();
		}

		public void UpdateChannelContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			channelIdComboBox.SelectedIndexChanged -= ChannelIdComboBoxSelectedIndexChanged;
			channelIdComboBox.SelectedItem = EditPanel.Camera.Profile.StreamConfigs[StreamId].Channel;
			channelIdComboBox.SelectedIndexChanged += ChannelIdComboBoxSelectedIndexChanged;
		}

        public void UpdateDewarpModeContent()
        {
            if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
                return;

            dewarpModeComboBox.SelectedIndexChanged -= DewarpModeComboBoxSelectedIndexChanged;
            dewarpModeComboBox.SelectedItem = Dewarps.ToString(EditPanel.Camera.Profile.StreamConfigs[StreamId].Dewarp);
            dewarpModeComboBox.SelectedIndexChanged += DewarpModeComboBoxSelectedIndexChanged;
        }

		public void UpdateConnectionProtocolContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			protocolComboBox.SelectedIndexChanged -= ProtocolComboBoxSelectedIndexChanged;
			protocolComboBox.SelectedItem = ConnectionProtocols.ToDisplayString(EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol);
			StreamPanelVisible();
			RtspPanelVisible();
			MulticastPanelVisible();
			protocolComboBox.SelectedIndexChanged += ProtocolComboBoxSelectedIndexChanged;
		}

		public void UpdateCompressionContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			compressionComboBox.SelectedIndexChanged -= CompressionComboBoxSelectedIndexChanged;
			compressionComboBox.SelectedItem = Compressions.ToDisplayString(EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression);
			UpdateFieldByCompression();
			compressionComboBox.SelectedIndexChanged += CompressionComboBoxSelectedIndexChanged;
		}

		public void UpdateResolutionContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			resolutionComboBox.SelectedIndexChanged -= ResolutionComboBoxSelectedIndexChanged;
			resolutionComboBox.SelectedItem = Resolutions.ToString(EditPanel.Camera.Profile.StreamConfigs[StreamId].Resolution);
			resolutionComboBox.SelectedIndexChanged += ResolutionComboBoxSelectedIndexChanged;
		}

		public void UpdateQualityContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			qualityComboBox.SelectedIndexChanged -= QualityComboBoxSelectedIndexChanged;
			qualityComboBox.SelectedItem = EditPanel.Camera.Profile.StreamConfigs[StreamId].VideoQuality;
			qualityComboBox.SelectedIndexChanged += QualityComboBoxSelectedIndexChanged;
		}

		public void UpdateFramerateContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;
			
			fpsComboBox.SelectedIndexChanged -= FpsComboBoxSelectedIndexChanged;
			fpsComboBox.SelectedItem = EditPanel.Camera.Profile.StreamConfigs[StreamId].Framerate;
			fpsComboBox.SelectedIndexChanged += FpsComboBoxSelectedIndexChanged;
		}

		public void UpdateBitrateContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			bitrateComboBox.SelectedIndexChanged -= BitrateComboBoxSelectedIndexChanged;
			bitrateComboBox.SelectedItem = Bitrates.ToDisplayString(EditPanel.Camera.Profile.StreamConfigs[StreamId].Bitrate);
			bitrateComboBox.SelectedIndexChanged += BitrateComboBoxSelectedIndexChanged;
		}
		
		public void UpdateControlPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			controlPortTextBox.TextChanged -= ControlPortTextBoxTextChanged;
			controlPortTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Control.ToString();
			controlPortTextBox.TextChanged += ControlPortTextBoxTextChanged;
		}

        public void UpdateHttpsPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

            httpsTextBox.TextChanged -= HttpsPortTextBoxTextChanged;
            httpsTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Https.ToString();
            httpsTextBox.TextChanged += HttpsPortTextBoxTextChanged;
		}

		public void UpdateStreamingPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;
			
			streamPortTextBox.TextChanged -= StreamPortTextBoxTextChanged;
			streamPortTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Streaming.ToString();
			streamPortTextBox.TextChanged += StreamPortTextBoxTextChanged;
		}
		
		public void UpdateRtspPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			rtspPortTextBox.TextChanged -= RtspPortTextBoxTextChanged;
			rtspPortTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.Rtsp.ToString();
			rtspPortTextBox.TextChanged += RtspPortTextBoxTextChanged;
		}

		public void UpdateURIContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			uriTextBox.TextChanged -= URITextBoxTextChanged;
			uriTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].URI;
			uriTextBox.TextChanged += URITextBoxTextChanged;
		}

		public void UpdateMulticastNetworkAddressContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			multicastNetworkAddressTextBox.TextChanged -= MulticastNetworkAddressTextBoxTextChanged;
			multicastNetworkAddressTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].MulticastNetworkAddress;
			multicastNetworkAddressTextBox.TextChanged += MulticastNetworkAddressTextBoxTextChanged;
		}

		public void UpdateVideoInPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			videoInPortTextBox.TextChanged -= VideoInPortTextBoxTextChanged;
			videoInPortTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.VideoIn.ToString();
			videoInPortTextBox.TextChanged += VideoInPortTextBoxTextChanged;
		}

		public void UpdateAudioInPortContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			audioInPortTextBox.TextChanged -= AudioInPortTextBoxTextChanged;
			audioInPortTextBox.Text = EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionPort.AudioIn.ToString();
			audioInPortTextBox.TextChanged += AudioInPortTextBoxTextChanged;
		}

		public void UpdateThresholdContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			thresholdComboBox.SelectedIndexChanged -= ThresholdComboBoxSelectedIndexChanged;
			thresholdComboBox.SelectedItem = EditPanel.Camera.Profile.StreamConfigs[StreamId].MotionThreshold;
			thresholdComboBox.SelectedIndexChanged += ThresholdComboBoxSelectedIndexChanged;
		}

		public void UpdateProfileModeContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			profileModeComboBox.SelectedIndexChanged -= ProfileModeComboBoxSelectedIndexChanged;
			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "BOSCH":

					if (((BOSCHCameraModel)EditPanel.Camera.Model).StreamProfileMode.ContainsKey(EditPanel.Camera.Profile.StreamConfigs[StreamId].ProfileMode))
					{
						profileModeComboBox.SelectedItem = ((BOSCHCameraModel)EditPanel.Camera.Model).StreamProfileMode[EditPanel.Camera.Profile.StreamConfigs[StreamId].ProfileMode];
					}
					else
					{
						if(profileModeComboBox.Items.Count > 0)
						{
							foreach (KeyValuePair<UInt16, String> streamMode in ((BOSCHCameraModel) EditPanel.Camera.Model).StreamProfileMode)
							{
								profileModeComboBox.SelectedItem = streamMode.Value;
								break;
							}
						}
					}   
					break;
			}
			profileModeComboBox.SelectedIndexChanged += ProfileModeComboBoxSelectedIndexChanged;
		}

		public void UpdateBitrateControlContent()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			bitrateControlcomboBox.SelectedIndexChanged -= BitrateControlcomboBoxSelectedIndexChanged;
			bitrateControlcomboBox.SelectedItem = BitrateControls.ToString(EditPanel.Camera.Profile.StreamConfigs[StreamId].BitrateControl);
			bitrateControlcomboBox.SelectedIndexChanged += BitrateControlcomboBoxSelectedIndexChanged;
		}

		public void UpdateSettingToEditComponent(CameraModel model)
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId))
				return;

			UpdateChannelId(model);
			UpdateProtocol(model);
		    UpdateDewarpMode(model);
			UpdateCompressions(model);
			UpdateResolution(model);
			UpdateFramerate(model);
			UpdateBitrate(model);
			UpdateProfileMode(model);
		}

		private void UpdateChannelId(CameraModel model)
		{
			channelIdComboBox.Items.Clear();

			switch(EditPanel.Camera.Mode)
			{
				case CameraMode.SixVga:
					for (UInt16 i = 1; i <= 6; i++)
						channelIdComboBox.Items.Add(i);
					channelIdComboBox.Enabled = false;
							channelIdPanel.Visible = true;
					break;

				case CameraMode.FourVga:
					switch (EditPanel.Camera.Model.Manufacture)
					{
						case "ACTi":
							for (UInt16 i = 1; i <= 4; i++)
								channelIdComboBox.Items.Add(i);
							channelIdComboBox.Enabled = false;
									channelIdPanel.Visible = true;
							break;

						default:
							for (UInt16 i = 1; i <= model.NumberOfChannel; i++)
								channelIdComboBox.Items.Add(i);

							channelIdComboBox.Enabled = (channelIdComboBox.Items.Count > 1);
							channelIdPanel.Visible = false;
							break;
					}
					break;

                case CameraMode.Quad:
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "Axis":
                            channelIdComboBox.Enabled =
                            channelIdPanel.Visible = false;
                            break;
                    }
                    break;

				default:
					for (UInt16 i = 1; i <= model.NumberOfChannel; i++)
						channelIdComboBox.Items.Add(i);

					channelIdComboBox.Enabled = (channelIdComboBox.Items.Count > 1);

					switch (EditPanel.Camera.Model.Manufacture)
					{
                        //case "Axis":
                        //case "ACTi":
                        //case "BOSCH":
                        //case "VIVOTEK":
                        //case "Brickcom":
                        //    channelIdPanel.Visible = model.NumberOfChannel > 1;
                        //    break;

						case "ArecontVision":
							channelIdPanel.Visible = (StreamId == 1 && model.NumberOfChannel > 1);
							break;

                        case "BOSCH":
                        case "Avigilon":
                            channelIdPanel.Visible = (StreamId == 1 && model.NumberOfChannel > 1);
                            channelIdPanel.Enabled = (EditPanel.Camera.ReadyState == ReadyState.JustAdd || EditPanel.Camera.ReadyState == ReadyState.New);
                            break;

						case "NEXCOM":
						case "YUAN":
                        case "Stretch":
							channelIdPanel.Visible = true;
							break;

						default:
                            channelIdPanel.Visible = model.NumberOfChannel > 1;
							//channelIdPanel.Visible = false;
							break;
					}

					break;
			}
		}

		private void UpdateProtocol(CameraModel model)
		{
			protocolComboBox.Items.Clear();
			var connectionProtocols = ProfileChecker.GetConnectionProtocol(EditPanel.Camera, model, StreamId);
			foreach (ConnectionProtocol connectionProtocol in connectionProtocols)
				protocolComboBox.Items.Add(ConnectionProtocols.ToDisplayString(connectionProtocol));

			protocolComboBox.Enabled = (protocolComboBox.Items.Count > 1);
		}

        private void UpdateDewarpMode(CameraModel model)
        {
            dewarpModeComboBox.Items.Clear();
            var dewarps = ProfileChecker.GetDewarpMode(EditPanel.Camera, model, StreamId);

            if(dewarps != null)
            {
                foreach (Dewarp dewarp in dewarps)
                    dewarpModeComboBox.Items.Add(Dewarps.ToString(dewarp));
            }

            dewarpModeComboBox.Enabled = (dewarpModeComboBox.Items.Count > 1);

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "VIVOTEK":
                     if (EditPanel.Camera.Model.Type == "fisheye" && EditPanel.Camera.Profile.SensorMode == SensorMode.Fisheye)
                    {
                        dewarpModePanel.Visible = (dewarpModeComboBox.Items.Count > 0);
                    }
                    else
                    {
                        dewarpModePanel.Visible = false;
                    }
                    break;

                case "Axis":
                    if (EditPanel.Camera.Model.Type == "fisheye")
                    {
                        dewarpModePanel.Visible = (dewarpModeComboBox.Items.Count > 0);
                    }
                    else
                    {
                        dewarpModePanel.Visible = false;
                    }
                    break;

                default:
                    dewarpModePanel.Visible = false;
                    break;
            }
        }

		private void UpdateCompressions(CameraModel model)
		{
			compressionComboBox.Items.Clear();
			IEnumerable<Compression> compressions = ProfileChecker.GetCompression(EditPanel.Camera, model, StreamId);

			if (compressions != null)
			{
				foreach (Compression compression in compressions)
					compressionComboBox.Items.Add(Compressions.ToDisplayString(compression));

				if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.Off && !compressionComboBox.Items.Contains(Compressions.ToDisplayString(Compression.Off)))
					compressionComboBox.Items.Add(Compressions.ToDisplayString(Compression.Off));
			}
			else
				compressionComboBox.Items.Add(Compressions.ToDisplayString(Compression.Off));

			compressionComboBox.Enabled = (compressionComboBox.Items.Count > 1);

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ONVIF":
                case "Kedacom":
				case "Customization":
					compressionPanel.Visible = false;
					break;

                case "Certis":
                    if(EditPanel.Camera.Model.Type == "Badge")
                    {
                        compressionPanel.Visible = false;
                    }
                    else
                    {
                        compressionPanel.Visible = true;
                    }
			        break;

				default:
					compressionPanel.Visible = true;
					break;
			}
		}

		public void UpdateResolution(CameraModel model)
		{
			resolutionComboBox.Items.Clear();
			Resolution[] resolutions = ProfileChecker.GetResolution(EditPanel.Camera, model, StreamId);

			if (resolutions != null)
			{
				foreach (Resolution resolution in resolutions)
					resolutionComboBox.Items.Add(Resolutions.ToString(resolution));
			}

			resolutionComboBox.Enabled = (resolutionComboBox.Items.Count > 1);

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "BOSCH":
					resolutionPanel.Visible = (resolutionComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId));
					break;

				default:
					resolutionPanel.Visible = (resolutionComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId) && EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Off);
					break;
			}
			
		}

		public void UpdateFramerate(CameraModel model)
		{
			fpsComboBox.Items.Clear();
			UInt16[] framerates = ProfileChecker.GetFramerate(EditPanel.Camera, model, StreamId);

			if (framerates != null)
			{
                Array.Sort(framerates);
				foreach (UInt16 framerate in framerates)
					fpsComboBox.Items.Add(framerate);
			}

			fpsComboBox.Enabled = (fpsComboBox.Items.Count > 1);
			fpsPanel.Visible = (fpsComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId) && EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Off);
		}

		public void UpdateBitrate(CameraModel model)
		{
			bitrateComboBox.Items.Clear();
			Bitrate[] bitrates = ProfileChecker.GetBitrate(EditPanel.Camera, model, StreamId);

			if (bitrates != null)
			{
				foreach (Bitrate bitrate in bitrates)
					bitrateComboBox.Items.Add(Bitrates.ToDisplayString(bitrate));
			}

			bitrateComboBox.Enabled = (bitrateComboBox.Items.Count > 1);
			bitratePanel.Visible = (bitrateComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)
				&& EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Off && EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Mjpeg);
		}

		public void UpdateProfileMode(CameraModel model)
		{
			profileModeComboBox.Items.Clear();

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "BOSCH":
					foreach (KeyValuePair<UInt16, String> streamMode in ((BOSCHCameraModel) EditPanel.Camera.Model).StreamProfileMode)
						profileModeComboBox.Items.Add(streamMode.Value);

                    
                    profileModePanel.Visible = (profileModeComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId) && (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Mjpeg || EditPanel.Camera.Model.Type == "VideoServer"));
					break;

                default:
			        profileModePanel.Visible = (profileModeComboBox.Items.Count > 0 && EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId) && EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression != Compression.Mjpeg);
			        break;
			}

            profileModeComboBox.Enabled = (profileModeComboBox.Items.Count > 1);
		}

		public void UpdateFieldByCompression()
		{
			MaximumSize = new Size(0, Size.Height);
			switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression)
			{
				case Compression.Mjpeg:
					qualityPanel.Visible = true;
					resolutionPanel.Visible = true;
					fpsPanel.Visible = (fpsComboBox.Items.Count > 0);
					bitratePanel.Visible = false;
					break;

				case Compression.H264:
				case Compression.Mpeg4:
				case Compression.Svc:
					resolutionPanel.Visible = true;
					bitratePanel.Visible = (bitrateComboBox.Items.Count > 0);
					fpsPanel.Visible = (fpsComboBox.Items.Count > 0);
					qualityPanel.Visible = false;
					break;

				case Compression.Off:
					bitratePanel.Visible = false;
					qualityPanel.Visible = false;
					resolutionPanel.Visible = false;
					fpsPanel.Visible = false;
					break;
			}
			MaximumSize = DefaultMaximumSize;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "BOSCH":
					resolutionPanel.Visible = false;
			        qualityPanel.Visible = (EditPanel.Camera.Model.Type != "VideoServer");
					break;

                case "SAMSUNG":
                    bitratePanel.Visible = true;
                    break;

                case "inskytec":
                case "Dahua":
                    bitratePanel.Visible = true;
                    qualityPanel.Visible = false;
                    break;

				case "PULSE":
					qualityPanel.Visible = false;
					break;

				case "ArecontVision":
					QualityPanelVisible();
					break;

                case "Certis":
                    if (EditPanel.Camera.Model.Type == "Badge")
                        resolutionPanel.Visible = bitratePanel.Visible = false;
                    break;
			}

			Int16 count = 0;
			foreach (Control control in Controls)
			{
				if (control.Visible) count++;
			}
			Size = new Size(0, count * 40 + 25);
			Invalidate();
		}

		public void UpdateFieldByBrand(String brand)
		{
			switch (brand)
			{
				case "ACTi":
					//channelIdPanel.Visible = (EditPanel.Camera.Model.NumberOfChannel > 1);//controlled by UpdateChanelId()
					controlPanel.Visible = true;
					StreamPanelVisible();
					break;

                case "A-MTK":
                    controlPanel.Visible = channelIdPanel.Visible = false;
                    StreamPanelVisible();
                    break;

				case "ArecontVision":
					//channelIdPanel.Visible = (StreamId == 1 && EditPanel.Camera.Model.NumberOfChannel > 1);//controlled by UpdateChanelId()
                    controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
					break;

				case "MegaSys":
                //case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					channelIdPanel.Visible = controlPanel.Visible = false;
					StreamPanelVisible();
					break;

				case "Surveon":
				case "Certis":
                case "FINE":
                    if (EditPanel.Camera.Model.Series == "DynaColor" || EditPanel.Camera.Model.Series == "A-MTK")
					{
						channelIdPanel.Visible = controlPanel.Visible = false;
						StreamPanelVisible();
					}
					else
                        channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;

					break;

                case "Brickcom":
                case "VIGZUL":
                    if (EditPanel.Camera.Model.Series == "DynaColor")
                    {
                        channelIdPanel.Visible = controlPanel.Visible = false;
                        StreamPanelVisible();
                    }
                    else
                        controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;//channelIdPanel.Visible = 

                    break;

                case "DLink":
                    if (EditPanel.Camera.Model.Type == "DynaColor")
                    {
                        channelIdPanel.Visible = controlPanel.Visible = false;
                        StreamPanelVisible();
                    }
                    else
                        channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;

                    break;

				//case "ETROVISION":
				//case "IPSurveillance":
				//case "XTS":
				//    channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = false;
				//    break;
                case "Panasonic":
                    channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
                    break;

				case "NEXCOM":
				case "YUAN":
                case "Stretch":
					//channelIdPanel.Visible = true;//controlled by UpdateChanelId()
                    controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
					break;

				case "Messoa":
                    channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
					protocolPanel.Visible = (StreamId == 1);
					break;

				case "Axis":
				case "VIVOTEK":
                case "BOSCH":
                case "Avigilon":
					//channelIdPanel.Visible = (EditPanel.Camera.Model.NumberOfChannel > 1);//controlled by UpdateChanelId()
                    controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
					break;

				default:
                    channelIdPanel.Visible = controlPanel.Visible = streamPanel.Visible = httpsPanel.Visible = false;
					break;
			}
			
			Invalidate();
		}

		private Boolean _showRecordText;
		public Boolean ShowRecordText
		{
			get { return _showRecordText; }
			set {
				_showRecordText = value;
				Invalidate();
			}
		}

		public void ThresholdPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ArecontVision":
					thresholdPanel.Visible = thresholdComboBox.Enabled = (StreamId == 1);
					break;

				default:
					thresholdPanel.Visible = thresholdComboBox.Enabled = false;
					break;
			}
		}

		public void BitrateControlPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ArecontVision":
					bitrateControlPanel.Visible = bitrateControlcomboBox.Enabled = EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.H264;
					break;

				default:
					bitrateControlPanel.Visible = bitrateControlcomboBox.Enabled = false;
					break;
			}
		}

		public void QualityPanelVisible()
		{
			//if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			//switch (EditPanel.Camera.Model.Manufacture)
			//{
			//    case "ArecontVision":
			//        qualityPanel.Visible =
			//                EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.H264 && EditPanel.Camera.Profile.StreamConfigs[StreamId].BitrateControl == BitrateControl.VBR;
			//        break;
			//}
		}

		public void URIPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "Customization":
					uriPanel.Visible = uriTextBox.Enabled = true;
					break;

				default:
					uriPanel.Visible = uriTextBox.Enabled = false;
					break;
			}
		}

		public void RtspPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
					switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
					{
						case ConnectionProtocol.RtspOverUdp:
						case ConnectionProtocol.RtspOverHttp:
						case ConnectionProtocol.RtspOverTcp:
							rtspPanel.Visible = true;
							break;

						default:
							rtspPanel.Visible = false;
							break;
					}
					break;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (EditPanel.Camera.Model.Series == "DynaColor")
					{
						switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
						{
							case ConnectionProtocol.RtspOverUdp:
							case ConnectionProtocol.RtspOverHttp:
							case ConnectionProtocol.RtspOverTcp:
								rtspPanel.Visible = true;
								break;

							default:
								rtspPanel.Visible = false;
								break;
						}
					}
					else
					{
						switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
						{
							case ConnectionProtocol.RtspOverUdp:
							case ConnectionProtocol.RtspOverTcp:
								rtspPanel.Visible = true;
								break;

							default:
								rtspPanel.Visible = false;
								break;
						}
					}
					break;

                case "DLink":
                    if (EditPanel.Camera.Model.Type == "DynaColor")
                    {
                        switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                        {
                            case ConnectionProtocol.RtspOverUdp:
                            case ConnectionProtocol.RtspOverHttp:
                            case ConnectionProtocol.RtspOverTcp:
                                rtspPanel.Visible = true;
                                break;

                            default:
                                rtspPanel.Visible = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                        {
                            case ConnectionProtocol.RtspOverUdp:
                            case ConnectionProtocol.RtspOverTcp:
                                rtspPanel.Visible = true;
                                break;

                            default:
                                rtspPanel.Visible = false;
                                break;
                        }
                    }
                    break;

                case "Panasonic":
                    switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                    {
                        case ConnectionProtocol.RtspOverUdp:
                        case ConnectionProtocol.RtspOverTcp:
                            rtspPanel.Visible = true;
                            break;

                        default:
                            rtspPanel.Visible = false;
                            break;
                    }
                    break;

				case "PULSE":
                case "ZeroOne":
					rtspPanel.Visible = true;
					break;

                case "Customization":
                    switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                    {
                        case ConnectionProtocol.RtspOverUdp:
                        case ConnectionProtocol.RtspOverTcp:
                            rtspPanel.Visible = true;
                            rtspPortTextBox.Text = (554).ToString();
                            break;
                        case ConnectionProtocol.RtspOverHttp:
                            rtspPanel.Visible = true;
                            rtspPortTextBox.Text = EditPanel.Camera.Profile.HttpPort.ToString();
                            break;
                        default:
                            rtspPanel.Visible = false;
                            break;
                    }
                    break;

				default:
					switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
					{
						case ConnectionProtocol.RtspOverUdp:
						case ConnectionProtocol.RtspOverTcp:
							rtspPanel.Visible = true;
							break;

						default:
							rtspPanel.Visible = false;
							break;
					}
					break;
			}

			if (StreamId != 1)
			{
				switch (EditPanel.Camera.Model.Manufacture)
				{
					case "EverFocus":
					case "MegaSys":
                    case "Avigilon":
                    case "DivioTec":
					case "HIKVISION":
					case "PULSE":
                    case "ZeroOne":
					case "GeoVision":
						rtspPanel.Visible = false;
						break;

					case "Brickcom":
                    case "VIGZUL":
					case "Surveon":
					case "Certis":
                    case "FINE":
						if (EditPanel.Camera.Model.Series == "DynaColor") rtspPanel.Visible = false;
						break;

                    case "DLink":
                        if (EditPanel.Camera.Model.Type == "DynaColor") rtspPanel.Visible = false;
                        break;
				}
			}

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ONVIF":
                case "Kedacom":
				case "Customization":
					break;

				case "Axis":
					if (EditPanel.Camera.Model.Type != "AudioBox")
					{
						if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.Off)
							rtspPanel.Visible = false;
					}
					break;

                case "Certis":
                    if (EditPanel.Camera.Model.Type == "Badge")
                    {
                            rtspPanel.Visible = true;
                    }
                    else
                    {
                        if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.Off)
                            rtspPanel.Visible = false;
                    }
			        break;

				default:
					if (EditPanel.Camera.Profile.StreamConfigs[StreamId].Compression == Compression.Off)
						rtspPanel.Visible = false;
					break;
			}

			Invalidate();
		}

		public void MulticastPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "DLink":
					switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
					{
						case ConnectionProtocol.Multicast:
							multicastNetwordAddressPanel.Visible =
							videoInPortPanel.Visible =
							audioInPortPanel.Visible = true;
							break;

						default:
							multicastNetwordAddressPanel.Visible =
							videoInPortPanel.Visible =
							audioInPortPanel.Visible = false;
							break;
					}
					break;

				default:
					multicastNetwordAddressPanel.Visible =
					videoInPortPanel.Visible =
					audioInPortPanel.Visible = false;
					break;
			}
		}

        public void HttpsPanelVisible()
        {
            if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;
            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "A-MTK":
                    switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                    {
                        case ConnectionProtocol.Https:
                            httpsPanel.Visible = true;
                            break;

                        default:
                            httpsPanel.Visible = false;
                            break;
                    }
                    break;

                case "Certis":
                  if (EditPanel.Camera.Model.Series == "A-MTK")
                    {
                        switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                        {
                            case ConnectionProtocol.Https:
                                httpsPanel.Visible = true;
                                break;

                            default:
                                httpsPanel.Visible = false;
                                break;
                        }
                    }
                    else
                      httpsPanel.Visible = false;
                    break;

                default:
                    httpsPanel.Visible = false;
                    break;
            }

            Invalidate();
        }

	    public void StreamPanelVisible()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			switch (EditPanel.Camera.Model.Manufacture)
			{
				case "ACTi":
					switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
					{
						case ConnectionProtocol.Tcp:
							streamPanel.Visible = true;
							break;

						default:
							streamPanel.Visible = false;
							break;
					}
					break;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
                case "SAMSUNG":
                case "inskytec":
				case "HIKVISION":
					switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
					{
						case ConnectionProtocol.Http:
							streamPanel.Visible = true;
							break;

						default:
							streamPanel.Visible = false;
							break;
					}
					break;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (EditPanel.Camera.Model.Series == "DynaColor")
					{
						switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
						{
							case ConnectionProtocol.Http:
								streamPanel.Visible = true;
								break;

							default:
								streamPanel.Visible = false;
								break;
						}
					}
					else
						streamPanel.Visible = false;
					break;

                case "DLink":
                    if (EditPanel.Camera.Model.Type == "DynaColor" && EditPanel.Camera.Model.Model != "DVS-310-1")
                    {
                        switch (EditPanel.Camera.Profile.StreamConfigs[StreamId].ConnectionProtocol)
                        {
                            case ConnectionProtocol.Http:
                                streamPanel.Visible = true;
                                break;

                            default:
                                streamPanel.Visible = false;
                                break;
                        }
                    }
                    else
                        streamPanel.Visible = false;
                    break;

				default:
					streamPanel.Visible = false;
					break;
			}
	        HttpsPanelVisible();

			Invalidate();
		}

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			Control control = (Control)sender;

			if (control == null || control.Parent == null) return;

			Graphics g = e.Graphics;

            SetupBase.Manager.Paint(g, control);

			if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                SetupBase.Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
			else
                SetupBase.Manager.PaintText(g, control.Tag.ToString());
		}
	}
}
