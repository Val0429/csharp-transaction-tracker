using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using DeviceCab;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
	public partial class EditPanel : UserControl
	{
		public IServer Server;
		public ICamera Camera;
        public event EventHandler OnPtzPatrol;
		public event EventHandler OnMotionSetting;
		public event EventHandler OnPresetPointSetting;
		public event EventHandler OnPresetTourSetting;

		public Boolean IsEditing;

		protected BrandControl _brandControl;
		private AuthenticationControl _authenticationControl;
		private ConnectionControl _connectionControl;
		private CaptureCardControl _captureCardControl;
		private VideoControl _videoControl1;
		private VideoControl _videoControl2;
		private VideoControl _videoControl3;
		private VideoControl _videoControl4;
		private VideoControl _videoControl5;
		private VideoControl _videoControl6;
		private IOPortControl _ioPortControl;
        private LiveCheckControl _liveCheckControl;
		private PtzCommandControl _ptzCommandControl;
		private MultiStreamingControl _multiStreamingControl;
		private ResolutionRegionControl _resolutionRegionControl1;
		private ResolutionRegionControl _resolutionRegionControl2;
        private PIPControl _pipControl;

		public Dictionary<String, String> Localization;
		public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},
                                   {"SetupDevice_PTZPatrol", "Digital PTZ Patrol Setting"},
								   {"SetupDevice_MotionSetting", "Motion Area Setting"},
								   {"SetupDevice_PresetPointSetting", "Preset Point Setting"},
								   {"SetupDevice_PresetTourSetting", "Preset Tour Setting"},
								   {"SetupDevice_InvalidIP", "Could not link to web page with invalid network address."}
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.None;
		}

		public virtual void Initialize()
		{
            ptzPatrolBufferPanel.Paint += InputPanelPaint;
            ptzPatrolBufferPanel.MouseClick += PtzPatrolBufferPanelMouseClick;

			motionSettingDoubleBufferPanel.Paint += InputPanelPaint;
			motionSettingDoubleBufferPanel.MouseClick += MotionSettingDoubleBufferPanelMouseClick;

			presetPointDoubleBufferPanel.Paint += InputPanelPaint;
			presetPointDoubleBufferPanel.MouseClick += PresetPointDoubleBufferPanelMouseClick;

			presetTourDoubleBufferPanel.Paint += InputPanelPaint;
			presetTourDoubleBufferPanel.MouseClick += PresetTourDoubleBufferPanelMouseClick;
			IsEditing = false;

			if (_brandControl == null)
			{
				_brandControl = new BrandControl
				{
					EditPanel = this,
				};
				_brandControl.InitializeBrand();
				_brandControl.OnBrandChange += BrandControlOnBrandChange;
				_brandControl.OnModelChange += BrandControlOnModelChange;
			}

			_authenticationControl = new AuthenticationControl
			{
				EditPanel = this
			};
			
			_captureCardControl = new CaptureCardControl
			{
				EditPanel = this
			};

			_connectionControl = new ConnectionControl
			{
				EditPanel = this
			};

			_connectionControl.OnModeChange += ConnectionControlOnModeChange;
			_connectionControl.OnTvStandardChange += ConnectionControlOnTvStandardChange;
			_connectionControl.OnSensorModeChange += ConnectionControlOnSensorModeChange;
			_connectionControl.OnPowerFrequencyChange += ConnectionControlOnPowerFrequencyChange;
            _connectionControl.OnDeviceMountTypeChange += ConnectionControlOnDeviceMountTypeChange;
			_connectionControl.OnRecordStreamChange += ConnectionControlOnRecordStreamChange;
			_connectionControl.OnAspectRatioChange += ConnectionControlOnAspectRatioChange;
			_connectionControl.OnAspectRatioCorrectionChange += ConnectionControlOnAspectRatioCorrectionChange;

			_ioPortControl = new IOPortControl
			{
				EditPanel = this
			};

			_multiStreamingControl = new MultiStreamingControl()
			{
				EditPanel = this
			};

		    _pipControl = new PIPControl()
		    {
		        EditPanel = this
		    };

            _liveCheckControl = new LiveCheckControl()
            {
                EditPanel = this
            };

			_ptzCommandControl = new PtzCommandControl
			{
				EditPanel = this
			};
			_ptzCommandControl.Initialize();

			_videoControl1 = new VideoControl
			{
				EditPanel = this,
				StreamId = 1,
			};
			_videoControl1.OnProtocolChange += PrimaryStreamOnProtocolChange;
			_videoControl1.OnCompressChange += PrimaryStreamOnCompressChange;
			_videoControl1.OnResolutionChange += PrimaryStreamOnResolutionChange;
			_videoControl1.OnQualityChange += PrimaryStreamOnQualityChange;
			_videoControl1.OnFrameRateChange += PrimaryStreamOnFrameRateChange;
			_videoControl1.OnBitRateChange += PrimaryStreamOnBitRateChange;
			_videoControl1.OnControlPortChange += PrimaryStreamOnControlPortChange;
			_videoControl1.OnStreamPortChange += PrimaryStreamOnStreamPortChange;
			_videoControl1.OnRtspPortChange += PrimaryStreamOnRtspPortChange;
			_videoControl1.OnChannelIdChange += PrimaryStreamOnChannelIdChange;
			_videoControl1.OnMotionThresholdChange += PrimaryStreamOnMotionThresholdChange;

			_videoControl2 = new VideoControl
			{
				EditPanel = this,
				StreamId = 2,
			};
			_videoControl2.OnCompressChange += VideoControl2OnCompressChange;
			_videoControl2.OnResolutionChange += VideoControl2OnResolutionChange;
			_videoControl2.OnFrameRateChange += VideoControl2OnFrameRateChange;
			_videoControl2.OnChannelIdChange += VideoControl2OnChannelIdChange;

			_videoControl3 = new VideoControl
			{
				EditPanel = this,
				StreamId = 3,
			};
			_videoControl3.OnCompressChange += VideoControl3OnCompressChange;
			_videoControl3.OnResolutionChange += VideoControl3OnResolutionChange;
			_videoControl3.OnFrameRateChange += VideoControl3OnFrameRateChange;

			_videoControl4 = new VideoControl
			{
				EditPanel = this,
				StreamId = 4,
			};

			_videoControl5 = new VideoControl
			{
				EditPanel = this,
				StreamId = 5,
			};

			_videoControl6 = new VideoControl
			{
				EditPanel = this,
				StreamId = 6,
			};

			_resolutionRegionControl1 = new ResolutionRegionControl
			{
				EditPanel = this,
				StreamId = 1,
			};

			_resolutionRegionControl2 = new ResolutionRegionControl
			{
				EditPanel = this,
				StreamId = 2,
			};

            //try
            //{
            //    if (Server.Server.SupportPIP)
            //        containerPanel.Controls.Add(_pipControl);
            //}
            //catch (Exception)
            //{

            //}
            
			containerPanel.Controls.Add(_multiStreamingControl);
			containerPanel.Controls.Add(_ptzCommandControl);
            containerPanel.Controls.Add(_liveCheckControl);
			containerPanel.Controls.Add(_ioPortControl);
			containerPanel.Controls.Add(_videoControl6);
			containerPanel.Controls.Add(_videoControl5);
			containerPanel.Controls.Add(_videoControl4);
			containerPanel.Controls.Add(_videoControl3);
			containerPanel.Controls.Add(_resolutionRegionControl2);
			containerPanel.Controls.Add(_videoControl2);
			containerPanel.Controls.Add(_resolutionRegionControl1);
			containerPanel.Controls.Add(_videoControl1);
			containerPanel.Controls.Add(_captureCardControl);
			containerPanel.Controls.Add(_connectionControl);
			containerPanel.Controls.Add(_authenticationControl);
			containerPanel.Controls.Add(_brandControl);
		}

		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, control);
			Manager.PaintEdit(g, control);

			if (Localization.ContainsKey("SetupDevice_" + control.Tag))
				Manager.PaintText(g, Localization["SetupDevice_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		public virtual void ParseDevice()
		{
			IsEditing = false;
			
			Visible = false;

			DisplayStreamConfigs();

			_brandControl.ParseDevice();
			_connectionControl.ParseDevice();
			_authenticationControl.ParseDevice();
			_captureCardControl.ParseDevice();
			_ioPortControl.ParseDevice();
            _liveCheckControl.ParseDevice();
			_ptzCommandControl.ParseDevice();
			_multiStreamingControl.ParseDevice();

            //try
            //{
            //    if (Server.Server.SupportPIP)
            //        _pipControl.ParseDevice();
            //}
            //catch (Exception)
            //{

            //}
            
			_resolutionRegionControl1.ParseDevice();
			_resolutionRegionControl2.ParseDevice();
			PresetPointAndTourVisible();

			Visible = true;
			IsEditing = true;
			containerPanel.AutoScrollPosition = new Point(0, 0);
			containerPanel.AutoScroll = false;
			containerPanel.Focus();
			containerPanel.AutoScroll = true;
		}

		private Boolean _isHandlePrimaryStreamEvent;
		public void UpdateSettingContent(CameraModel model)
		{
			_isHandlePrimaryStreamEvent = false;

			_videoControl6.UpdateSettingToEditComponent(model);
			_videoControl5.UpdateSettingToEditComponent(model);
			_videoControl4.UpdateSettingToEditComponent(model);
			_videoControl3.UpdateSettingToEditComponent(model);
			_videoControl2.UpdateSettingToEditComponent(model);
			_videoControl1.UpdateSettingToEditComponent(model);

			_authenticationControl.UpdateSettingContent();
			_videoControl6.UpdateSettingContent();
			_videoControl5.UpdateSettingContent();
			_videoControl4.UpdateSettingContent();
			_videoControl3.UpdateSettingContent();
			_videoControl2.UpdateSettingContent();
			_videoControl1.UpdateSettingContent();

			_isHandlePrimaryStreamEvent = true;
		}

		public void UpdateSettingContentAndSetDefault(CameraModel model)
		{
			ProfileChecker.SetDefaultProtocol(Camera, model);
			ProfileChecker.SetDefaultAccountPassword(Camera, model);
			ProfileChecker.SetDefaultMode(Camera, model);
			ProfileChecker.SetDefaultAudioOutPort(Camera, model);
			ProfileChecker.SetDefaultTvStandard(Camera, model);
			ProfileChecker.SetDefaultSensorMode(Camera, model);
			ProfileChecker.SetDefaultPowerFrequency(Camera, model);
			ProfileChecker.SetDefaultAspectRatio(Camera, Camera.Model);
			ProfileChecker.SetDefaultIOPort(Camera, Camera.Model);

			switch (Camera.Mode)
			{
				case CameraMode.Single:
					Camera.Profile.StreamConfigs.Remove(2);
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = 1;
					Camera.Profile.RecordStreamId = 1;
					break;

				case CameraMode.Dual:
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					AddStreamConfigFromDefault(2);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
					break;

				case CameraMode.Triple:
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					AddStreamConfigFromDefault(2);
					switch (Camera.Model.Manufacture)
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
							ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							break;

						case "Brickcom":
                        case "VIGZUL":
						case "Surveon":
						case "Certis":
                        case "FINE":
							if (Camera.Model.Series == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							}
							break;

						case "DLink":
							if (Camera.Model.Type == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							}
							break;
					}
					AddStreamConfigFromDefault(3);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)3);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)3);
					break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					AddStreamConfigFromDefault(2);
					switch (Camera.Model.Manufacture)
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
							ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							break;

						case "Brickcom":
                        case "VIGZUL":
						case "Surveon":
						case "Certis":
                        case "FINE":
							if (Camera.Model.Series == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							}
							break;

						case "DLink":
							if (Camera.Model.Type == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
							}
							break;
					}
					AddStreamConfigFromDefault(3);
					switch (Camera.Model.Manufacture)
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
							ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
							ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
							ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
							break;

						case "Brickcom":
                        case "VIGZUL":
						case "Surveon":
						case "Certis":
                        case "FINE":
							if (Camera.Model.Series == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
							}
							break;

						case "DLink":
							if (Camera.Model.Type == "DynaColor")
							{
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
							}
							break;
					}
					AddStreamConfigFromDefault(4);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)4);
					switch (Camera.Model.Manufacture)
					{
						case "ACTi":
							Camera.Profile.RecordStreamId = Camera.Profile.StreamId;
							break;

						default:
							Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)4);
							break;
					}
					break;

                case CameraMode.Five:
                    Camera.Profile.StreamConfigs.Remove(6);

                    AddStreamConfigFromDefault(2);
                    switch (Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[2], 2);
                            break;
                    }
                    AddStreamConfigFromDefault(3);
                    switch (Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[3], 3);
                            break;
                    }
                    AddStreamConfigFromDefault(4);
                    switch (Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                            ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[4], 4);
                            break;
                    }


                    Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)5);
                    Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)5);
                    break;

				case CameraMode.SixVga:
					AddStreamConfigFromDefault(2);
					AddStreamConfigFromDefault(3);
					AddStreamConfigFromDefault(4);
					AddStreamConfigFromDefault(5);
					AddStreamConfigFromDefault(6);
					switch (Camera.Model.Manufacture)
					{
						case "ACTi":
							Camera.Profile.RecordStreamId = Camera.Profile.StreamId;
							break;
					}
					break;
			}


			UpdateSettingContent(model);
		}

		public void AddStreamConfigFromDefault(UInt16 index)
		{
			switch (Camera.Mode)
			{
				case CameraMode.SixVga:
					if (!Camera.Profile.StreamConfigs.ContainsKey(index))
					{
						Camera.Profile.StreamConfigs.Add(index, StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]));
					}
					else
					{
						var channel = Camera.Profile.StreamConfigs[index].Channel;
						Camera.Profile.StreamConfigs[index] = StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]);
						Camera.Profile.StreamConfigs[index].Channel = channel;
					}
					break;

				case CameraMode.FourVga:
					switch (Camera.Model.Manufacture)
					{
						case "ACTi":
							if (!Camera.Profile.StreamConfigs.ContainsKey(index))
							{
								Camera.Profile.StreamConfigs.Add(index, StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]));
							}
							else
							{
								var channel = Camera.Profile.StreamConfigs[index].Channel;
								Camera.Profile.StreamConfigs[index] = StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]);
								Camera.Profile.StreamConfigs[index].Channel = channel;
							}
							break;

						default:
							if (!Camera.Profile.StreamConfigs.ContainsKey(index))
							{
								Camera.Profile.StreamConfigs.Add(index, StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]));

								ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.SetDefaultPort(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
								ProfileChecker.SetDefaultBitrateControl(Camera.Model, Camera.Profile.StreamConfigs[index]);
								ProfileChecker.SetDefaultMulticastNetworkAddress(Camera, Camera.Model, Camera.Profile.StreamConfigs[index]);
							}
							break;
					}
					break;

				default:
					if (!Camera.Profile.StreamConfigs.ContainsKey(index))
					{
						Camera.Profile.StreamConfigs.Add(index, StreamConfigs.Clone(Camera.Profile.StreamConfigs[1]));

						ProfileChecker.SetDefaultProtocol(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.SetDefaultPort(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.SetDefaultCompression(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, Camera.Profile.StreamConfigs[index], index);
						ProfileChecker.SetDefaultBitrateControl(Camera.Model, Camera.Profile.StreamConfigs[index]);
						ProfileChecker.SetDefaultMulticastNetworkAddress(Camera, Camera.Model, Camera.Profile.StreamConfigs[index]);
					}
					break;
			}
			//else
			//    Camera.Profile.StreamConfigs[index] = StreamConfigs.Clone(Camera.StreamConfig);

			//if (Camera.Mode == CameraMode.SixVga)
			//    Camera.Profile.StreamConfigs[index].Channel = index;
		}

		public void OpenWebPage()
		{
			const String pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
			const String patternDomain = @"^(http|https|ftp)://([a-zA-Z0-9_-]*(?:\.[a-zA-Z0-9_-]*)+):?([0-9]+)?/?";
			//create our Regular Expression object
			var link = String.Format("http://{0}:{1}", Camera.Profile.NetworkAddress, Camera.Profile.HttpPort);

			Regex checkIP = new Regex(pattern);
			Regex checkDomain = new Regex(patternDomain);

			if ( String.IsNullOrEmpty(Camera.Profile.NetworkAddress) || (!checkIP.IsMatch(Camera.Profile.NetworkAddress, 0) && !checkDomain.IsMatch(link, 0)))
			{
				TopMostMessageBox.Show(Localization["SetupDevice_InvalidIP"],
												Localization["MessageBox_Error"], MessageBoxButtons.OK,
												MessageBoxIcon.Error);
				return;
			}

			Process.Start(link);
		}

		public void CameraIsModify()
		{
			Server.DeviceModify(Camera);
		}

		protected void BrandControlOnBrandChange(Object sender, EventArgs e)
		{
			String brand = _brandControl.SelectedBrand;

			DisplayNetworkConfigs();

			_connectionControl.UpdateMode();
			_connectionControl.UpdateTvStandard();
			_connectionControl.UpdateSensor();
			_connectionControl.UpdatePowerFrequency();
            _connectionControl.UpdateDeviceMountType();
			_connectionControl.UpdateAspectRatio();
			_connectionControl.UpdateAudioOutPort();
			_connectionControl.UpdateURI();
			_connectionControl.UpdateDewarpType();
			//_connectionControl.SensorModePanelVisible();
			//_connectionControl.PowerFrequencyPanelVisible();

			_videoControl1.UpdateFieldByBrand(brand);
			_videoControl2.UpdateFieldByBrand(brand);
			_videoControl3.UpdateFieldByBrand(brand);
			_videoControl4.UpdateFieldByBrand(brand);
			_videoControl5.UpdateFieldByBrand(brand);
			_videoControl6.UpdateFieldByBrand(brand);

			switch (Camera.Mode)
			{
				case CameraMode.Single:
					Camera.Profile.StreamConfigs.Remove(2);
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					Camera.Profile.StreamId = 1;
					Camera.Profile.RecordStreamId = 1;
					break;

				case CameraMode.Dual:
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
					break;

				case CameraMode.Triple:
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)3);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)3);
					break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);

					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)4);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)4);
					break;

                case CameraMode.Five:
                    Camera.Profile.StreamConfigs.Remove(6);

                    Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)5);
                    Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)5);
                    break;
			}

			_connectionControl.AudioPortPanelVisible();
			_connectionControl.URIPanelVisible();
		}

		public void BrandControlOnModelChange(Object sender, EventArgs e)
		{
			//DisplayNetworkConfigs();
			_connectionControl.UpdateMode();
			_connectionControl.AspectRatioPanelVisible();
            _connectionControl.RemoteRecoveryPanelVisible();
			_connectionControl.UpdateTvStandard();
			_connectionControl.UpdateSensor();
			_connectionControl.UpdatePowerFrequency();
            _connectionControl.UpdateDeviceMountType();
			_connectionControl.UpdateAspectRatio();
			_connectionControl.UpdateAudioOutPort();
			_connectionControl.UpdateURI();
			_connectionControl.UpdateDewarpType();

            _liveCheckControl.ParseDevice();
			_ptzCommandControl.ParseDevice();
			_multiStreamingControl.ParseDevice();
            //try
            //{
            //    if (Server.Server.SupportPIP)
            //        _pipControl.ParseDevice();
            //}
            //catch (Exception)
            //{
		    
            //}
           
			_resolutionRegionControl1.ParseDevice();
			_resolutionRegionControl2.ParseDevice();
			PresetPointAndTourVisible();

			var displayIO = (Camera.Model.IOPortSupport != null);

			_ioPortControl.Visible = displayIO;
			if (displayIO)
				_ioPortControl.ParseDevice();

			if (Camera.Profile.CaptureCardConfig != null)
				_captureCardControl.ParseDevice();

            switch (Camera.Model.Manufacture)
            {
                case "iSapSolution":
                    _authenticationControl.Visible = Camera.Model.Type == "SmartPatrolService";
                    break;
            }
		}

		private void ConnectionControlOnModeChange(Object sender, EventArgs e)
		{
			//need check compression
			ProfileChecker.SetDefaultChannelId(Camera, Camera.Model);

			foreach (var config in Camera.Profile.StreamConfigs)
			{
				ProfileChecker.SetDefaultCompression(Camera, Camera.Model, config.Value, config.Key);
				ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, config.Value, config.Key);
				ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, config.Value, config.Key);
				ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, config.Value, config.Key);
			}
			UpdateStreamConfigs();
			DisplayStreamConfigs();

			DisplayRegionControlConfigs();
		}

		private void ConnectionControlOnTvStandardChange(Object sender, EventArgs e)
		{
			foreach (var config in Camera.Profile.StreamConfigs)
			{
				ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, config.Value, config.Key);
				ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, config.Value, config.Key);
				ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, config.Value, config.Key);
			}
			UpdateStreamConfigs();
			DisplayStreamConfigs();
		}

		private void ConnectionControlOnSensorModeChange(Object sender, EventArgs e)
		{
			switch (Camera.Model.Manufacture)
			{
				//Sensor Mode no effect video setting
				case "Messoa":
					return;

				default:
					foreach (var config in Camera.Profile.StreamConfigs)
					{
						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, config.Value, config.Key);
						ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, config.Value, config.Key);
						ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, config.Value, config.Key);
					}
					UpdateStreamConfigs();
					DisplayStreamConfigs();
					break;
			}
		}

        private void ConnectionControlOnDeviceMountTypeChange(object sender, EventArgs e)
        {
            switch (Camera.Model.Manufacture)
            {
                case "Axis":
                case "VIVOTEK":
                    foreach (var config in Camera.Profile.StreamConfigs)
                    {
                        ProfileChecker.CheckAvailableSetDefaultDewarpMode(Camera, Camera.Model, config.Value, config.Key);
                        ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, config.Value, config.Key);
                        ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, config.Value, config.Key);
                        ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, config.Value, config.Key);
                    }
                    UpdateStreamConfigs();
                    DisplayStreamConfigs();
                    return;
            }
        }

		private void ConnectionControlOnPowerFrequencyChange(Object sender, EventArgs e)
		{
			switch (Camera.Model.Manufacture)
			{
				//Power Frequency no effect video setting
				case "Messoa":
					return;

				default:
					foreach (var config in Camera.Profile.StreamConfigs)
					{
						ProfileChecker.CheckAvailableSetDefaultResolution(Camera, Camera.Model, config.Value, config.Key);
						ProfileChecker.CheckAvailableSetDefaultBitrate(Camera, Camera.Model, config.Value, config.Key);
						ProfileChecker.CheckAvailableSetDefaultFramerate(Camera, Camera.Model, config.Value, config.Key);
					}
					UpdateStreamConfigs();
					DisplayStreamConfigs();
					break;
			}
		}

		private void ConnectionControlOnRecordStreamChange(Object sender, EventArgs e)
		{
			DisplayVideoControlRecordLabel();
		}

		private void ConnectionControlOnAspectRatioChange(Object sender, EventArgs e)
		{
			switch (Camera.Model.Manufacture)
			{
				case "DLink":
					DLinkOnAspectRatioChange();
					break;

                case "Panasonic":
                    PanasonicOnAspectRatioChange();
                    break;

				case "ACTi":
					ACTiOnAspectRatioChange();
					break;
			}
		}

		private void ConnectionControlOnAspectRatioCorrectionChange(Object sender, EventArgs e)
		{
			switch (Camera.Model.Manufacture)
			{
				case "Axis":
					AxisOnAspectRatioCorrectionChange();
					break;
			}
		}

		private void PrimaryStreamOnProtocolChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigProtocol();
					return;

                case "SAMSUNG":
                case "inskytec":
                    //UpdateSAMSUNGSubStreamConfig();
			        return;
			}
		}

		private void PrimaryStreamOnCompressChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					ACTiPrimaryStreamOnCompressChange();
					break;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					_isHandlePrimaryStreamEvent = false;

					UpdateDynaColorSubStreamConfig(1);

					_isHandlePrimaryStreamEvent = true;
					break;

                case "SAMSUNG":
                case "inskytec":
                    //SAMSUNGPrimaryStreamOnCompressChange();
                    break;

				case "Brickcom":
                case "VIGZUL":
					BrickcomPrimaryStreamOnCompressChange();
					break;

				case "Surveon":
				case "Certis":
                case "FINE":
					_isHandlePrimaryStreamEvent = false;
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(1);
					}
					_isHandlePrimaryStreamEvent = true;
					break;

				case "DLink":
					DLinkPrimaryStreamOnCompressChange();
					break;

				case "Messoa":
					MessoaPrimaryStreamOnCompressChange();
					return;

                case "Panasonic":
			        PanasonicOnCompressionChange();
                    return;
			}
		}

		private void PrimaryStreamOnQualityChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigQuality();
					return;
			}
		}

		private void PrimaryStreamOnFrameRateChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigFrameRate();
					return;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(1);
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(1);
			        return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(1);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(1);
					}
					return;
			}
		}

		private void PrimaryStreamOnBitRateChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigBitRate();
					return;
			}
		}

		private void PrimaryStreamOnControlPortChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigControlPort();
					return;
			}
		}

		private void PrimaryStreamOnStreamPortChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigStreamingPort();
					return;
			}
		}

		private void PrimaryStreamOnRtspPortChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigRtspPort();
					return;
			}
		}

		private void PrimaryStreamOnResolutionChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ACTi":
					UpdateACTiStreamConfigFrameRate();
					return;

				case "ZAVIO":
					UpdateZAVIOStreamConfigFrameRate();
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(1);
                    return;

                case "A-MTK":
                    UpdateAMTKStreamConfigFrameRate(1);
                    return;

                case "FINE":
                    if (Camera.Model.Series == "DynaColor")
                        UpdateDynaColorSubStreamConfig(1);
                    else
                        UpdateFINEStreamConfigFrameRate();
                    return;

				case "Messoa":
					UpdateMessoaSubStreamConfig();
					return;

				case "GeoVision":
					UpdateGeoVisionSubStreamConfig();
					return;

				case "Brickcom":
                case "VIGZUL":
					if (Camera.Model.Series == "DynaColor")
						UpdateDynaColorSubStreamConfig(1);
					else
						UpdateBrickcomSubStreamConfig();
					return;

				case "Surveon":
				case "Certis":
					if (Camera.Model.Series == "DynaColor")
						UpdateDynaColorSubStreamConfig(1);
					return;

				case "DLink":
                    if (Camera.Model.Type == "DynaColor")
                        UpdateDynaColorSubStreamConfig(1);
                    else
                        DLinkStreamConfigBitrate(1);
					return;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(1);
					return;

				case "ArecontVision":
					_resolutionRegionControl1.UpdateResolutionRegion();
					return;

			}
		}

		private void PrimaryStreamOnMotionThresholdChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ArecontVision":
					if (Camera.Profile.StreamConfigs.ContainsKey(2))
						Camera.Profile.StreamConfigs[2].MotionThreshold = Camera.Profile.StreamConfigs[1].MotionThreshold;
					return;
			}
		}

		private void PrimaryStreamOnChannelIdChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ArecontVision":
					_resolutionRegionControl1.ParseDevice();

					if (Camera.Profile.StreamConfigs.ContainsKey(2))
					{
						Camera.Profile.StreamConfigs[2].Channel = Camera.Profile.StreamConfigs[1].Channel;
						_resolutionRegionControl2.ParseDevice();
					}
					return;
			}
		}

		private void VideoControl2OnCompressChange(Object sender, EventArgs e)
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

			_connectionControl.UpdateStreamConfigList();

			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(2);
					return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					return;
			}
		}

		private void VideoControl2OnResolutionChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ZAVIO":
					UpdateZAVIOStreamConfigFrameRate();
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(2);
                    return;

                case "A-MTK":
                    UpdateAMTKStreamConfigFrameRate(2);
                    return;

                case "FINE":
                    if (Camera.Model.Series == "DynaColor")
                    {
                        UpdateDynaColorSubStreamConfig(2);
                    }
                    else
                        UpdateFINEStreamConfigFrameRate();
                    return;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(2);
					return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					else
					{
					    DLinkStreamConfigBitrate(2);
					}
					return;

				case "ArecontVision":
					_resolutionRegionControl2.UpdateResolutionRegion();
					return;
			}
		}

		private void VideoControl2OnFrameRateChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(2);
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(2);
                    return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(2);
					}
					return;
			}
		}

		private void VideoControl2OnChannelIdChange(Object sender, EventArgs e)
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(2)) return;

			switch (Camera.Model.Manufacture)
			{
				case "ArecontVision":
					_resolutionRegionControl2.ParseDevice();
					return;
			}
		}

		private void VideoControl3OnCompressChange(Object sender, EventArgs e)
		{
			if (!Camera.Profile.StreamConfigs.ContainsKey(3)) return;

			_connectionControl.UpdateStreamConfigList();

			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(3);
					return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					return;
			}
		}

		private void VideoControl3OnResolutionChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "ZAVIO":
					UpdateZAVIOStreamConfigFrameRate();
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(3);
                    return;

                case "A-MTK":
                    UpdateAMTKStreamConfigFrameRate(3);
                    return;

                case "FINE":
                    if (Camera.Model.Series == "DynaColor")
                    {
                        UpdateDynaColorSubStreamConfig(3);
                    }
                    else 
                        UpdateFINEStreamConfigFrameRate();
                    return;

				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(3);
					return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					else
					{
					    DLinkStreamConfigBitrate(3);
					}
					return;
			}
		}

		private void VideoControl3OnFrameRateChange(Object sender, EventArgs e)
		{
			if (!_isHandlePrimaryStreamEvent) return;

			switch (Camera.Model.Manufacture)
			{
				case "MegaSys":
                case "Avigilon":
                case "DivioTec":
				case "SIEMENS":
				//case "SAMSUNG":
				case "HIKVISION":
				case "PULSE":
                case "ZeroOne":
					UpdateDynaColorSubStreamConfig(3);
					return;

                case "Dahua":
                case "GoodWill":
                    UpdateDahuaStreamConfigFrameRate(3);
                    return;

				case "Brickcom":
                case "VIGZUL":
				case "Surveon":
				case "Certis":
                case "FINE":
					if (Camera.Model.Series == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					return;

				case "DLink":
					if (Camera.Model.Type == "DynaColor")
					{
						UpdateDynaColorSubStreamConfig(3);
					}
					return;
			}
		}

		private void UpdateStreamConfigs()
		{
			switch (Camera.Mode)
			{
				case CameraMode.Single:
					Camera.Profile.StreamConfigs.Remove(2);
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = 1;
					Camera.Profile.RecordStreamId = 1;
					_videoControl1.UpdateSettingToEditComponent(Camera.Model);
					break;

				case CameraMode.Dual:
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
					_videoControl1.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					break;

				case CameraMode.Triple:
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)3);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)3);
					_videoControl1.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl3.UpdateSettingToEditComponent(Camera.Model);
					break;

                case CameraMode.Quad:
                    Camera.Profile.StreamConfigs.Remove(3);
                    Camera.Profile.StreamConfigs.Remove(4);
                    Camera.Profile.StreamConfigs.Remove(5);
                    Camera.Profile.StreamConfigs.Remove(6);
                    Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
                    Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
                    _videoControl1.UpdateSettingToEditComponent(Camera.Model);
                    _videoControl2.UpdateSettingToEditComponent(Camera.Model);
                    break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)4);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)4);
					_videoControl1.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl3.UpdateSettingToEditComponent(Camera.Model);
					_videoControl4.UpdateSettingToEditComponent(Camera.Model);
					break;

                case CameraMode.Five:
                    Camera.Profile.StreamConfigs.Remove(6);
                    Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)5);
                    Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)5);
                    _videoControl1.UpdateSettingToEditComponent(Camera.Model);
                    _videoControl2.UpdateSettingToEditComponent(Camera.Model);
                    _videoControl3.UpdateSettingToEditComponent(Camera.Model);
                    _videoControl4.UpdateSettingToEditComponent(Camera.Model);
                    _videoControl5.UpdateSettingToEditComponent(Camera.Model);
                    break;

				case CameraMode.SixVga:
					_videoControl1.UpdateSettingToEditComponent(Camera.Model);
					_videoControl2.UpdateSettingToEditComponent(Camera.Model);
					_videoControl3.UpdateSettingToEditComponent(Camera.Model);
					_videoControl4.UpdateSettingToEditComponent(Camera.Model);
					_videoControl5.UpdateSettingToEditComponent(Camera.Model);
					_videoControl6.UpdateSettingToEditComponent(Camera.Model);
					break;
			}
		}

		private void DisplayStreamConfigs()
		{
			switch (Camera.Mode)
			{
				case CameraMode.Single:
					_videoControl2.Visible = _videoControl3.Visible = _videoControl4.Visible = _videoControl5.Visible = _videoControl6.Visible = false;
					_videoControl1.ParseDevice();
					Camera.Profile.StreamConfigs.Remove(2);
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = 1;
					Camera.Profile.RecordStreamId = 1;
					break;

				case CameraMode.Dual:
					_videoControl3.Visible = _videoControl4.Visible = _videoControl5.Visible = _videoControl6.Visible = false;
					_videoControl2.Visible = true;
					_videoControl1.ParseDevice();
					_videoControl2.ParseDevice();
					Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
					break;

				case CameraMode.Triple:
					_videoControl4.Visible = _videoControl5.Visible = _videoControl6.Visible = false;
					_videoControl3.Visible = _videoControl2.Visible = true;
					_videoControl1.ParseDevice();
					_videoControl2.ParseDevice();
					_videoControl3.ParseDevice();
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)3);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)3);
					break;

                case CameraMode.Quad:
                    _videoControl3.Visible = _videoControl4.Visible = _videoControl5.Visible = _videoControl6.Visible = false;
					_videoControl2.Visible = true;
					_videoControl1.ParseDevice();
					_videoControl2.ParseDevice();
                    Camera.Profile.StreamConfigs.Remove(3);
					Camera.Profile.StreamConfigs.Remove(4);
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)2);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)2);
                    break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					_videoControl2.Visible = _videoControl3.Visible = _videoControl4.Visible = true;
					_videoControl5.Visible = _videoControl6.Visible = false;
					_videoControl1.ParseDevice();
					_videoControl2.ParseDevice();
					_videoControl3.ParseDevice();
					_videoControl4.ParseDevice();
					Camera.Profile.StreamConfigs.Remove(5);
					Camera.Profile.StreamConfigs.Remove(6);
					Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)4);
					Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)4);
					break;

                case CameraMode.Five:
                    _videoControl2.Visible = _videoControl3.Visible = _videoControl4.Visible = _videoControl5.Visible = true;
                    _videoControl6.Visible = false;
                    _videoControl1.ParseDevice();
                    _videoControl2.ParseDevice();
                    _videoControl3.ParseDevice();
                    _videoControl4.ParseDevice();
                    _videoControl5.ParseDevice();
                    Camera.Profile.StreamConfigs.Remove(6);
                    Camera.Profile.StreamId = Math.Min(Camera.Profile.StreamId, (UInt16)5);
                    Camera.Profile.RecordStreamId = Math.Min(Camera.Profile.RecordStreamId, (UInt16)5);
                    break;

				case CameraMode.SixVga:
					_videoControl2.Visible = _videoControl3.Visible = _videoControl4.Visible = _videoControl5.Visible = _videoControl6.Visible = true;
					_videoControl1.ParseDevice();
					_videoControl2.ParseDevice();
					_videoControl3.ParseDevice();
					_videoControl4.ParseDevice();
					_videoControl5.ParseDevice();
					_videoControl6.ParseDevice();
					break;
			}

			DisplayVideoControlRecordLabel();
			_multiStreamingControl.UpdateStreamConfigList();
		}

		private void DisplayRegionControlConfigs()
		{
			switch (Camera.Model.Manufacture)
			{
				case "ArecontVision":
					break;

				default:
					_resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;
					return;
			}

			switch (Camera.Mode)
			{
				case CameraMode.Single:
					_resolutionRegionControl1.Visible = true;
					_resolutionRegionControl2.Visible = false;
					_resolutionRegionControl1.ParseDevice();
					break;

				case CameraMode.Dual:
					_resolutionRegionControl1.Visible = true;
					_resolutionRegionControl2.Visible = true;
					_resolutionRegionControl1.ParseDevice();
					_resolutionRegionControl2.ParseDevice();
					break;
			}
		}

		private void DisplayVideoControlRecordLabel()
		{
			switch (Camera.Mode)
			{
				case CameraMode.Single:
					_videoControl1.ShowRecordText = false;
					break;

				case CameraMode.Dual:
					_videoControl1.ShowRecordText = (Camera.Profile.RecordStreamId == 1);
					_videoControl2.ShowRecordText = (Camera.Profile.RecordStreamId == 2);
					break;

				case CameraMode.Triple:
					_videoControl1.ShowRecordText = (Camera.Profile.RecordStreamId == 1);
					_videoControl2.ShowRecordText = (Camera.Profile.RecordStreamId == 2);
					_videoControl3.ShowRecordText = (Camera.Profile.RecordStreamId == 3);
					break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					_videoControl1.ShowRecordText = (Camera.Profile.RecordStreamId == 1);
					_videoControl2.ShowRecordText = (Camera.Profile.RecordStreamId == 2);
					_videoControl3.ShowRecordText = (Camera.Profile.RecordStreamId == 3);
					_videoControl4.ShowRecordText = (Camera.Profile.RecordStreamId == 4);
					break;

                case CameraMode.Five:
                    _videoControl1.ShowRecordText = (Camera.Profile.RecordStreamId == 1);
                    _videoControl2.ShowRecordText = (Camera.Profile.RecordStreamId == 2);
                    _videoControl3.ShowRecordText = (Camera.Profile.RecordStreamId == 3);
                    _videoControl4.ShowRecordText = (Camera.Profile.RecordStreamId == 4);
                    _videoControl5.ShowRecordText = (Camera.Profile.RecordStreamId == 5);
                    break;

				case CameraMode.SixVga:
					_videoControl1.ShowRecordText = (Camera.Profile.RecordStreamId == 1);
					_videoControl2.ShowRecordText = (Camera.Profile.RecordStreamId == 2);
					_videoControl3.ShowRecordText = (Camera.Profile.RecordStreamId == 3);
					_videoControl4.ShowRecordText = (Camera.Profile.RecordStreamId == 4);
					_videoControl5.ShowRecordText = (Camera.Profile.RecordStreamId == 5);
					_videoControl6.ShowRecordText = (Camera.Profile.RecordStreamId == 6);
					break;
			}
		}

		private void DisplayNetworkConfigs()
		{
			switch (Camera.Model.Manufacture)
			{
				case "iSapSolution":
					_authenticationControl.Visible = _connectionControl.Visible =
					_videoControl1.Visible = _captureCardControl.Visible =
					motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible = _ioPortControl.Visible =
                    _liveCheckControl.Visible = _ptzCommandControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;

                    if (Camera.Model.Type == "SmartPatrolService")
                        _authenticationControl.Visible = true;
					break;

				case "ONVIF":
                case "Kedacom":
					_authenticationControl.Visible = _connectionControl.Visible = _videoControl1.Visible = true;

					_captureCardControl.Visible =
					 motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible = _ioPortControl.Visible =
                     _liveCheckControl.Visible = _ptzCommandControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;
					_connectionControl.NetwordAddressPanelVisible(true);
					break;

				case "Customization":
                    _authenticationControl.Visible = _connectionControl.Visible = _videoControl1.Visible = _liveCheckControl.Visible = _ptzCommandControl.Visible = true;

					_captureCardControl.Visible = motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible =
					_ioPortControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;
					_connectionControl.NetwordAddressPanelVisible(true);
					break;

				case "NEXCOM":
					_connectionControl.Visible = _videoControl1.Visible = true;
					_ioPortControl.Visible = _authenticationControl.Visible = false;

					_captureCardControl.Visible = motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible =
                        _liveCheckControl.Visible = _ptzCommandControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;
					_connectionControl.NetwordAddressPanelVisible(false);
					break;

				case "YUAN":
                    if (Camera.Profile.CaptureCardConfig == null)
						Camera.Profile.CaptureCardConfig = new CaptureCardConfig();

					_connectionControl.Visible = _videoControl1.Visible =
					_captureCardControl.Visible = motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible = true;

                    _ioPortControl.Visible = _authenticationControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = _liveCheckControl.Visible = _ptzCommandControl.Visible = false;

					_connectionControl.NetwordAddressPanelVisible(false);
					break;

                case "Stretch":
					if (Camera.Profile.CaptureCardConfig == null)
						Camera.Profile.CaptureCardConfig = new CaptureCardConfig();

					_connectionControl.Visible = _videoControl1.Visible =
					_captureCardControl.Visible  = true;

                    motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible =
                    _ioPortControl.Visible = _authenticationControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = _liveCheckControl.Visible = _ptzCommandControl.Visible = false;

					_connectionControl.NetwordAddressPanelVisible(false);
					break;

				case "ArecontVision":
					_authenticationControl.Visible = _connectionControl.Visible = _videoControl1.Visible = true;
						
					switch (Camera.Mode)
					{
						case CameraMode.Single:
							_resolutionRegionControl1.Visible = true;
							_resolutionRegionControl2.Visible = false;
							break;

						case CameraMode.Dual:
							_resolutionRegionControl1.Visible = true;
							_resolutionRegionControl2.Visible = true;
							break;
					}

					_ioPortControl.Visible = (Camera.Model.IOPortSupport != null);

					_captureCardControl.Visible = motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible =
                        _liveCheckControl.Visible = _ptzCommandControl.Visible = false;
					_connectionControl.NetwordAddressPanelVisible(true);
					break;

				default:
					_authenticationControl.Visible = _connectionControl.Visible = _videoControl1.Visible = true;
					
					_ioPortControl.Visible = (Camera.Model.IOPortSupport != null);

					_captureCardControl.Visible = motionSettingDoubleBufferPanel.Visible = motionSettingLabel.Visible =
                        _liveCheckControl.Visible = _ptzCommandControl.Visible = _resolutionRegionControl1.Visible = _resolutionRegionControl2.Visible = false;
					_connectionControl.NetwordAddressPanelVisible(true);
					break;
			}

            switch (Camera.Model.Manufacture)
            {
                case "Certis":
                    if (Camera.Model.Type == "Badge")
                        _videoControl1.Visible = false;
                    break;
            }
		    _multiStreamingControl.Visible = Camera.Profile.StreamConfigs.Count > 0;
			_connectionControl.SensorModePanelVisible();
			_connectionControl.PowerFrequencyPanelVisible();
            _connectionControl.MountTypePanelVisible();
		}

		private void PresetPointAndTourVisible()
		{
			var visible = (Camera.ReadyState != ReadyState.New && Camera.Model.IsSupportPTZ);
			//visible = false;
			presetPointDoubleBufferPanel.Visible = presetPointLabel.Visible =
			presetTourDoubleBufferPanel.Visible = presetTourLabel.Visible = visible;
		}

        private void PtzPatrolBufferPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (OnPtzPatrol != null)
                OnPtzPatrol(this, null);
        }

		private void MotionSettingDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnMotionSetting != null)
				OnMotionSetting(this, null);
		}

		private void PresetPointDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPresetPointSetting != null)
				OnPresetPointSetting(this, null);
		}

		private void PresetTourDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPresetTourSetting != null)
				OnPresetTourSetting(this, null);
		}
	}
}
