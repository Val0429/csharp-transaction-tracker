using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using App;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using VideoMonitor;
using FontStyle = System.Drawing.FontStyle;
using Menu = VideoMonitor.VideoMenu;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;


namespace EMap
{
	public partial class MapControl : UserControl, IControl, IAppUse, IServerUse, IDrop, IMouseHandler
	{
		//minimize event
		public event EventHandler<EventArgs<String>> OnChangeMapFromEMap;
		public event EventHandler<EventArgs<String, String>> OnModifyCamera;
		public event EventHandler<EventArgs<String, String>> OnModifyVia;
		public event EventHandler<EventArgs<String, String>> OnModifyNVR;
		public event EventHandler<EventArgs<String>> OnModifyAnyAboutMap;
		public event EventHandler<EventArgs<String>> OnLogEventAlarmToMaps;
		public event EventHandler<EventArgs> OnStartToSetup;
		public event EventHandler<EventArgs<String, String>> OnClickDrawHotZone;

		public IApp App { get; set; }
		protected INVR NVR;
		protected ICMS CMS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
            {
                if (_server != null)
                {
                    _server.Utility.OnClickButton -= UtilityOnClickButton;
                    _server.Utility.OnMoveAxis -= UtilityOnMoveAxis;
                }

                _server = value;
                if (_server != null && _server.Configure.EnableJoystick)
                {
                    _server.Utility.OnClickButton += UtilityOnClickButton;
                    _server.Utility.OnMoveAxis += UtilityOnMoveAxis;
                }

				if (value is INVR)
					NVR = value as INVR;
				if (value is ICMS)
					CMS = value as ICMS;
			}
		}
		private MapAttribute _currentMap;
		public String CurrentMapId;
		private DataTable _mapsTable;
		public ICamera Camera;
		public DateTime DateTime = DateTime.MinValue;
		private Dictionary<ICamera, VideoWindow> _previewScreen;
		//private Dictionary<Panel, VideoWindow> _previewScreen;
		private MapHandler _mapHandler;
		private String _mode;
		public Dictionary<String, String> Localization;
		private Boolean _mapLoadLock;
		private Boolean _isVideoOnMap;
		public Dictionary<Int32, Size> VideoWindowSizeList;
		private Int32 _cameraVideoWindowSize;
		private static readonly Image _smallWindowIcon= Resources.GetResources(Properties.Resources.small, Properties.Resources.IMGSmallWindow);
		private static readonly Image _smallWindowIconActivate = Resources.GetResources(Properties.Resources.small_activate, Properties.Resources.IMGSmallWindowActivate);
		private static readonly Image _mediumWindowIcon = Resources.GetResources(Properties.Resources.medium, Properties.Resources.IMGMediumWindow);
		private static readonly Image _mediumWindowIconActivate= Resources.GetResources(Properties.Resources.medium_activate, Properties.Resources.IMGMediumWindowActivate);
		private static readonly Image _largeWindowIcon = Resources.GetResources(Properties.Resources.large, Properties.Resources.IMGLargeWindow);
		private static readonly Image _largeWindowIconActivate = Resources.GetResources(Properties.Resources.large_activate, Properties.Resources.IMGLargeWindowActivate);
		private static readonly Image _defaultWindowIcon = Resources.GetResources(Properties.Resources.ScreenDefault, Properties.Resources.IMGDefaultWindow);
		private static readonly Image _defaultWindowIconActivate= Resources.GetResources(Properties.Resources.ScreenDefault_activate, Properties.Resources.IMGDefaultWindowActivate);
		private static readonly Image _buttonUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
		private static readonly Image _buttonDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
		private static readonly Image _buttonRight = Resources.GetResources(Properties.Resources.arrow_right, Properties.Resources.IMGArrowRight);
		private static readonly Image _buttonLeft = Resources.GetResources(Properties.Resources.arrow_left, Properties.Resources.IMGArrowLeft);
		private static readonly Image _buttonBack = Resources.GetResources(Properties.Resources.original, Properties.Resources.IMGOriginalPoint);
		private static readonly Image _buttonUpActivate = Resources.GetResources(Properties.Resources.arrow_up_activate, Properties.Resources.IMGArrowUp);
		private static readonly Image _buttonDownActivate = Resources.GetResources(Properties.Resources.arrow_down_activate, Properties.Resources.IMGArrowUpActivate);
		private static readonly Image _buttonRightActivate = Resources.GetResources(Properties.Resources.arrow_right_activate, Properties.Resources.IMGArrowRightActivate);
		private static readonly Image _buttonLeftActivate = Resources.GetResources(Properties.Resources.arrow_left_activate, Properties.Resources.IMGArrowLeftActivate);
		private static readonly Image _buttonBackActivate = Resources.GetResources(Properties.Resources.original_activate, Properties.Resources.IMGOriginalPointActivate);
		private static readonly Image _buttonZoonIn = Resources.GetResources(Properties.Resources.zoom_in, Properties.Resources.IMGZoomIn);
		private static readonly Image _buttonZoonOut = Resources.GetResources(Properties.Resources.zoom_out, Properties.Resources.IMGZoomOut);
		private static readonly Image _buttonZoonDefault = Resources.GetResources(Properties.Resources.zoom1_1, Properties.Resources.IMGZoom1_1);
		private static readonly Image _buttonZoonInActivate = Resources.GetResources(Properties.Resources.zoom_in_activate, Properties.Resources.IMGZoomInActivate);
		private static readonly Image _buttonZoonOutActivate = Resources.GetResources(Properties.Resources.zoom_out_activate, Properties.Resources.IMGZoomOutActivate);
		private static readonly Image _buttonZoonDefaultActivate = Resources.GetResources(Properties.Resources.zoom1_1_activate, Properties.Resources.IMGZoom1_1Activate);
	    private static UInt16 MaxLog = 100;
		public void Initialize()
		{
			//language start
			Localization = new Dictionary<String, String>
							   {
								   {"Control_E-Map", "eMap"},

								   {"EMap_AllMaps", "All maps"},
								   {"MessageBox_Information", "Information"},
								   {"EMap_MessageMapExist", "This device existed in the map."},
								   {"EMap_MapButtonZoomIn", "Zoom In"},
								   {"EMap_MapButtonZoomOut", "Zoom Out"},
								   {"EMap_MapButtonZoomDefault", "No Zoom"},
								   {"EMap_MapButtonZoomDrag", "Drag To Zoom"},
								   {"EMap_MapButtonMoveUp", "Pan  Up"},
								   {"EMap_MapButtonMoveDown", "Pan  Down"},
								   {"EMap_MapButtonMoveRight", "Pan  Right"},
								   {"EMap_MapButtonMoveLeft", "Pan  Left"},
								   {"EMap_MapButtonMoveDefault", "Go Home"},
								   {"EMap_MessageBoxStartSetupMode", "Do you want to edit eMap?"},
								   {"EMap_MessageBoxRemindAddMap", "Please add new map."},
								   {"EMap_MessageLabelRemindAddMap", "Please add new map."},
								   {"EMap_MessageBoxReadImageFailed", "Read map failed. Please try again."},
								   {"EMap_MessageBoxDrawingHotZoneFailed", "This is not a polygon. Do you want to quit this drawing?"},
								   {"EMap_VideoWindowSizeLarge", "Large"},
								   {"EMap_VideoWindowSizeMedium", "Medium"},
								   {"EMap_VideoWindowSizeSmall", "Small"},
								   {"EMap_VideoWindowSizeDefault", "Set all windows to original size."},
								   {"EMap_MessageEventWithNoCameraInMap", "The camera \"%1\" is not in any maps."},
								   {"EMap_ButtonEventListReload", "Click here to reload events from the device."},
								   {"EMap_ButtonEventListReset", "Click here to reset events to empty."},
								   {"EMap_ButtonEventListClose", "Close events list."},
							   };

			Localizations.Update(Localization);

			_mapLoadLock = true;
			if (CMS == null) return;

			InitializeComponent();

			TitleName = Localization["Control_E-Map"];

			//padCom.Localization = Localization;
			Dock = DockStyle.Fill;
			padCom.Server = Server;
			padCom.CMS = CMS;
			padCom.Mode = "View";
			padCom.Scale = 10;
			padCom.Height = Int32.Parse(Properties.Resources.PadHeight);
			padCom.Width = Int32.Parse(Properties.Resources.PadWidth);
			padCom.OnCameraClick += padCom_OnCameraClick;
			padCom.OnViaClick += padCom_OnViaClick;
			padCom.OnClickChangeMap += padCom_OnClickChangeMap;
			padCom.OnNVRClick+=padCom_OnNVRClick;
			padCom.OnCameraEditClick += padCom_OnCameraRightClick;
			padCom.OnDescriptionEditClick += padCom_OnDescriptionEditClick;
			padCom.OnMoveDescription += padCom_OnMoveDescription;
			padCom.OnMoveCamera += padCom_OnMoveCamera;
			padCom.OnMoveVideoWindow += padCom_OnMoveVideoWindow;
			padCom.OnMoveVia += padCom_OnMoveVia;
			padCom.OnMoveNVR += padCom_OnMoveNVR;
			padCom.OnMoveCameraAroundItem += padCom_OnMoveCameraAroundItem;
			padCom.OnMoveMap += PadComOnMoveMap;
			padCom.OnScaleMap += PadComOnScaleMap;
			padCom.OnCompleteDrawingHotZone += padCom_OnCompleteDrawingHotZone;
			padCom.OnClickHotZone += padCom_OnClickHotZone;
			padCom.OnChangeHotZonePoint += padCom_OnChangeHotZonePoint;
			padCom.OnCompleteAddingVia += padCom_OnCompleteAddingVia;
			padCom.OnClickAlarm +=padCom_OnClickAlarm;
			padCom.OnClickEventList +=padCom_OnClickEventList;
			padCom.OnClickRemoveAllEvent += padCom_OnClickRemoveAllEvent;
			
			mapList.SelectedIndexChanged += MapListSelectedIndexChanged;
			buttonUp.Click +=ButtonMapMoveClick;
			buttonDown.Click += ButtonMapMoveClick;
			buttonLeft.Click += ButtonMapMoveClick;
			buttonRight.Click += ButtonMapMoveClick;
			SizeChanged +=MapControlSizeChanged;

			buttonZoomIn.MouseDown += ButtonZoomInMouseDown;
			buttonZoomIn.MouseUp += ButtonZoomInMouseUp;
			buttonZoomOut.MouseDown += ButtonZoomOutMouseDown;
			buttonZoomOut.MouseUp += ButtonZoomOutMouseUp;
			buttonZoomDefault.MouseDown += ButtonZoomDefaultMouseDown;
			buttonZoomDefault.MouseUp += ButtonZoomDefaultMouseUp;

			buttonUp.MouseDown += ButtonUpMouseDown;
			buttonUp.MouseUp +=ButtonUpMouseUp;
			buttonDown.MouseDown += ButtonDownMouseDown;
			buttonDown.MouseUp += ButtonDownMouseUp;
			buttonRight.MouseDown += ButtonRightMouseDown;
			buttonRight.MouseUp += ButtonRightMouseUp;
			buttonLeft.MouseDown += ButtonLeftMouseDown;
			buttonLeft.MouseUp += ButtonLeftMouseUp;
			buttonBack.MouseDown += ButtonBackMouseDown;
			buttonBack.MouseUp += ButtonBackMouseUp; 

			VideoWindowSizeList = new Dictionary<int, Size>
									  {
										  {
											  1,
											  new Size(
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeSmall.Split(',')[0]),
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeSmall.Split(',')[1]))
											  },
										  {
											  2,
											  new Size(
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeMedium.Split(',')[0]),
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeMedium.Split(',')[1]))
											  },
										  {
											  3,
											  new Size(
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeLarge.Split(',')[0]),
											  Convert.ToInt32(Properties.Resources.VideoWindowSizeLarge.Split(',')[1]))
											  }
									  };

			SharedToolTips.SharedToolTip.SetToolTip(buttonZoomIn, Localization["EMap_MapButtonZoomIn"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonZoomOut, Localization["EMap_MapButtonZoomOut"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonZoomDefault, Localization["EMap_MapButtonZoomDefault"]);
			SharedToolTips.SharedToolTip.SetToolTip(trackBarForZoom, Localization["EMap_MapButtonZoomDrag"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonUp, Localization["EMap_MapButtonMoveUp"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonDown, Localization["EMap_MapButtonMoveDown"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonRight, Localization["EMap_MapButtonMoveRight"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonLeft, Localization["EMap_MapButtonMoveLeft"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonBack, Localization["EMap_MapButtonMoveDefault"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeLarge, Localization["EMap_VideoWindowSizeLarge"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeMedium, Localization["EMap_VideoWindowSizeMedium"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeSmall, Localization["EMap_VideoWindowSizeSmall"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonDefaultVideoSize, Localization["EMap_VideoWindowSizeDefault"]);
			labelAddMap.Text = Localization["EMap_MessageLabelRemindAddMap"];
			_mapHandler = new MapHandler {CMS = CMS};
			MapControlLoad();
			_previewScreen = new Dictionary<ICamera, VideoWindow>();
			_cameraVideoWindowSize = -1;
			
			_mode = "View";
			panelScreen.AutoScroll = true;

			_toolMenu = new Menu
			{
				HasPlaybackPage = App.Pages.ContainsKey("Playback"),
				Server = CMS,
                App = App,
				UserDefineLocation = true,
			};
			_toolMenu.GenerateLiveToolMenu();
			_toolMenu.OnButtonClick += ToolManuButtonClick;
			padCom.ToolMenu = _toolMenu;
			_mapLoadLock = false;
			_isVideoOnMap = true;

			panelVideoWindowSize.Visible = _isVideoOnMap;

			NVR.OnEventReceive -= EventReceive;
			NVR.OnEventReceive += EventReceive;

			WindowSizeChangeClick(buttonDefaultVideoSize, new EventArgs());

			buttonZoomIn.BackgroundImage = _buttonZoonIn;
			buttonZoomOut.BackgroundImage = _buttonZoonOut;
			buttonZoomDefault.BackgroundImage = _buttonZoonDefault;
			buttonDown.BackgroundImage = _buttonDown;
			buttonLeft.BackgroundImage = _buttonLeft;
			buttonRight.BackgroundImage = _buttonRight;
			buttonUp.BackgroundImage = _buttonUp;
			buttonBack.BackgroundImage = _buttonBack;

			_mini = Resources.GetResources(Properties.Resources.mini, Properties.Resources.IMGMini);
			_mini2 = Resources.GetResources(Properties.Resources.mini2, Properties.Resources.IMGMini2);
			titlePanel.BackgroundImage = Resources.GetResources(Properties.Resources.banner, Properties.Resources.BGBanner);

			CMS.OnCameraStatusReceive -= CMSOnCameraEventReceive;
			CMS.OnCameraStatusReceive += CMSOnCameraEventReceive;

			CMS.OnNVRStatusReceive -= CMSOnNVRStatusReceive;
			CMS.OnNVRStatusReceive += CMSOnNVRStatusReceive;

		}

		private void ButtonZoomDefaultMouseDown(object sender, MouseEventArgs e)
		{
			buttonZoomDefault.BackgroundImage = _buttonZoonDefaultActivate;
		}

		private void ButtonZoomDefaultMouseUp(object sender, MouseEventArgs e)
		{
			buttonZoomDefault.BackgroundImage = _buttonZoonDefault;
		}

		private void ButtonZoomInMouseDown(object sender, MouseEventArgs e)
		{
            if (CurrentMapId == "Root") return;
			buttonZoomIn.BackgroundImage = _buttonZoonInActivate;
		}

		private void ButtonZoomInMouseUp(object sender, MouseEventArgs e)
		{
			buttonZoomIn.BackgroundImage = _buttonZoonIn;
		}

		private void ButtonZoomOutMouseDown(object sender, MouseEventArgs e)
		{
            if (CurrentMapId == "Root") return;
			buttonZoomOut.BackgroundImage = _buttonZoonOutActivate;
		}

		private void ButtonZoomOutMouseUp(object sender, MouseEventArgs e)
		{
			buttonZoomOut.BackgroundImage = _buttonZoonOut;
		}

		private void ButtonBackMouseDown(object sender, MouseEventArgs e)
		{
			buttonBack.BackgroundImage = _buttonBackActivate;
		}

		private void ButtonBackMouseUp(object sender, MouseEventArgs e)
		{
			buttonBack.BackgroundImage = _buttonBack;
		}

		private void ButtonLeftMouseDown(object sender, MouseEventArgs e)
		{
			buttonLeft.BackgroundImage = _buttonLeftActivate;
		}

		private void ButtonLeftMouseUp(object sender, MouseEventArgs e)
		{
			buttonLeft.BackgroundImage = _buttonLeft;
		}

		private void ButtonRightMouseDown(object sender, MouseEventArgs e)
		{
			buttonRight.BackgroundImage = _buttonRightActivate;
		}

		private void ButtonRightMouseUp(object sender, MouseEventArgs e)
		{
			buttonRight.BackgroundImage = _buttonRight;
		}

		private void ButtonDownMouseUp(object sender, MouseEventArgs e)
		{
			buttonDown.BackgroundImage = _buttonDown;
		}

		private void ButtonDownMouseDown(object sender, MouseEventArgs e)
		{
			buttonDown.BackgroundImage = _buttonDownActivate;
		}

		private void ButtonUpMouseDown(object sender, MouseEventArgs e)
		{
			buttonUp.BackgroundImage = _buttonUpActivate;
		}

		private void ButtonUpMouseUp(object sender, MouseEventArgs e)
		{
			buttonUp.BackgroundImage = _buttonUp;
		}

		public virtual void CMSOnCameraEventReceive(Object sender, EventArgs<List<ICamera>> e)
		{
			if (_mode == "Setup") return;

			foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
				{
					var objNVR = CMS.NVRManager.FindNVRById(Convert.ToUInt16(camera.Value.NVRSystemId));
					if (objNVR != null)
					{
						if(objNVR.Device.Devices.ContainsKey(Convert.ToUInt16(camera.Value.SystemId)))
						{
							var device = objNVR.Device.Devices[Convert.ToUInt16(camera.Value.SystemId)]  as ICamera;
							if(device == null) continue;
							camera.Value.CameraStatus = device.Status.ToString();
						}
					}   
				}
			}

            if (_activateVideoWindow != null)
            {
                if (_activateVideoWindow.ToolMenu != null)
                    _activateVideoWindow.ToolMenu.CheckStatus();
            }

			padCom.UpdateDeviceStatus();
		}

		private void CMSOnNVRStatusReceive(object sender, EventArgs<INVR> e)
		{
			if (_mode == "Setup") return;

			foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
				{
					var objNVR = CMS.NVRManager.FindNVRById(Convert.ToUInt16(nvr.Value.SystemId));
					if (objNVR != null)
						nvr.Value.Status = objNVR.NVRStatus;
				}
			}

            if (_activateVideoWindow != null)
            {
                if (_activateVideoWindow.ToolMenu != null)
                    _activateVideoWindow.ToolMenu.CheckStatus();
            }

			padCom.UpdateNVRStatus();
		}

		private Int32 _originalFunctionPanelHeight;
		private Int32 _originalScreenPanelHeight;
		private  Image _mini;
		private  Image _mini2;
		//minimize action
		private void Minimize()
		{
			IsMinimize = true;
			minimizePictureBox.BackgroundImage = _mini;

			panelControl.Visible = panelFunction.Visible = false;
			_originalFunctionPanelHeight = panelFunction.Height;
			_originalScreenPanelHeight = panelScreen.Height;
			panelControl.Height =panelFunction.Height = 0;
		   panelScreen.Height = Height - 30;
		 
		}

		private void Maximize()
		{
			IsMinimize = false;
			minimizePictureBox.BackgroundImage = _mini2;

			panelControl.Visible = panelFunction.Visible = true;
			panelControl.Height = Convert.ToInt32(Properties.Resources.PadHeight);
			panelFunction.Height = _originalFunctionPanelHeight;
			panelScreen.Height = _originalScreenPanelHeight;
		}

		public Boolean IsMinimize { get; private set; }
		private void MinimizePictureBoxMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else
				Minimize();
		}

		private readonly Font _font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private void TitlePanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.DrawString(TitleName, _font, Brushes.White, 18, 8);
		}

		private void MapControlSizeChanged(object sender, EventArgs e)
		{
			if (IsMinimize && !_isVideoOnMap)
			{
				panelScreen.Height = ClientSize.Height-titlePanel.Height;
			}
			//padCom.ParentViewSize = new System.Windows.Size(panelControl.Width, panelControl.Height);
		}
 
		public Boolean CheckDragDataType(Object dragObj)
		{
			return (dragObj is INVR || dragObj is IDeviceGroup || dragObj is IDevice);
		}

		public void DragStop(Point point, EventArgs<Object> e)
		{
			if (!Drag.IsDrop(this, point)) return;
			if( _mode == "View")
			{
				if (!Server.User.Current.Group.CheckPermission("Setup", Permission.Access))
					return;

				DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxStartSetupMode"], Localization["MessageBox_Information"],
											 MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

				if (result == DialogResult.OK)
				{
					if (OnStartToSetup != null)
					{
						OnStartToSetup(this, new EventArgs<String>(""));
					}
				}
				else
				{
					return;
				}
			}

			if (String.IsNullOrEmpty(CurrentMapId) || CurrentMapId == "-1" || CurrentMapId == "Root")
			{
				TopMostMessageBox.Show(Localization["EMap_MessageBoxRemindAddMap"], Localization["MessageBox_Information"],
											 MessageBoxButtons.OK, MessageBoxIcon.Question);
				return;
			}

			point = PointToClient(point);

			Object dragObj = e.Value;

			var newPoint = TransferPointToPadPoint(point.X, point.Y);

			Double x = newPoint.X;
			Double y = newPoint.Y;

			//if (!padCom.HitTestNodeOnMap(x, y)) return;

			if (dragObj  is IDevice)
			{
				var objDevice = dragObj as IDevice;
				var nvrSystemId = objDevice.Server.Id;
				INVR nvr = CMS.NVRManager.FindNVRById(nvrSystemId);
				if (nvr.ReadyState == ReadyState.NotInUse) return;
				//if (nvr.ReadyState != ReadyState.Ready) return;
				if (nvr.Device.Devices.ContainsValue((IDevice)dragObj))
				{
					var map = CMS.NVRManager.FindMapById(CurrentMapId);
					if (map != null)
					{
						foreach (KeyValuePair<string, CameraAttributes> cam in map.Cameras)
						{
							if (cam.Value.SystemId == objDevice.Id && cam.Value.NVRSystemId == nvrSystemId)
							{
								TopMostMessageBox.Show(Localization["EMap_MessageMapExist"], Localization["MessageBox_Information"],
													   MessageBoxButtons.OK, MessageBoxIcon.Information);
								return;
							}
						}
					}

				   CreateCameraInMap(((IDevice)dragObj), new Point((int) x, (int) y));
				}
			}
		   else if (dragObj is INVR)
		   {
			   if (CMS.NVRManager.NVRs.ContainsValue((INVR)dragObj))
			   {
				   if (!CMS.NVRManager.Maps.ContainsKey(CurrentMapId)) return;
				   var nvrs = CMS.NVRManager.Maps[CurrentMapId].NVRs;
				   foreach (KeyValuePair<string, NVRAttributes> nvr in nvrs)
				   {
					   if (nvr.Value.SystemId.ToString() == ((INVR) dragObj).Id.ToString())
					   {
						   TopMostMessageBox.Show(Localization["EMap_MessageMapExist"], Localization["MessageBox_Information"],
												   MessageBoxButtons.OK, MessageBoxIcon.Information);
						   return;
					   }
				   }
			
				   CreateNVRInMap((INVR)dragObj, new Point((int) x, (int) y));
			   }
		   }

		}

		private Point TransferPointToPadPoint(Double x,Double y)
		{
			var currentScale = Convert.ToDouble(trackBarForZoom.Value) / 10;

			Double newX = x - Convert.ToInt32((panelControl.Width - padCom.Width) / 2);
			Double newY = y - Convert.ToInt32((panelControl.Height - padCom.Height) / 2 + titlePanel.Height + panelFunction.Height);

			newX -= padCom.MainCanvasPoint.X;
			newY -= padCom.MainCanvasPoint.Y;

			if (currentScale != 1)
			{
				newX += (padCom.CurrentMapSize.Width * currentScale - padCom.CurrentMapSize.Width) / 2;
				newY += (padCom.CurrentMapSize.Height * currentScale - padCom.CurrentMapSize.Height) / 2;
				newX /= currentScale;
				newY /= currentScale;
			}

			return new Point((int) newX,(int) newY);
		}

		public void DragMove(MouseEventArgs e)
		{
		}

		private void NotifyMapSettingForModifyMapData()
		{
			if(OnModifyAnyAboutMap!=null)
			{
				OnModifyAnyAboutMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", CurrentMapId)));
			}
		}

		public void CreateCameraInMap(IDevice device, Point position)
		{
			var nvrSystemId = device.Server.Id;
			INVR nvr = CMS.NVRManager.FindNVRById(nvrSystemId);
			if (nvr.ReadyState == ReadyState.NotInUse) return;
			//if (nvr.ReadyState != ReadyState.Ready) return;
			var cam = device as ICamera;
			if (cam == null) return;
			var camera = new CameraAttributes
							 {
								 SystemId = device.Id,
								 NVRSystemId = device.Server.Id,
								 Name = String.Format("{0} ({1})", device, nvr.Name), //device.ToString(),
								 Type = "Camera",
								 X = Convert.ToDouble(position.X),
								 Y = Convert.ToDouble(position.Y),
								 Rotate = 0,
								 DescX = Convert.ToDouble(position.X),
								 DescY = Convert.ToDouble(position.Y + 40),
								 IsSpeakerEnabled = false,
								 SpeakerX = Convert.ToDouble(position.X -30),
								 SpeakerY = Convert.ToDouble(position.Y + 20),
								 IsAudioEnabled = false,
								 AudioX = Convert.ToDouble(position.X -50),
								 AudioY = Convert.ToDouble(position.Y + 20),
								 IsDefaultOpenVideoWindow = false,
								 IsVideoWindowOpen = false,
								 VideoWindowSize = 2,
								 VideoWindowX = Convert.ToDouble(position.X),
								 VideoWindowY = Convert.ToDouble(position.Y+20),
								 IsEventAlarm = false,
								 EventRecords = new List<CameraEventRecord>(),
								 CameraStatus = cam.Status.ToString()
							 };

			var id = _mapHandler.CreateCameraReturnId( CurrentMapId, camera);

			OnSyncData();

			FindMapAndGoToMapById(CurrentMapId);

			if (id != null && OnModifyCamera != null)
			{
				OnModifyCamera(this, new EventArgs<String, String>(id, CurrentMapId));
				padCom.ActivateCameraImage(id,true);
			}
		}

		public void CreateNVRInMap(INVR nvr, Point position)
		{
			var nvrAttributes = new NVRAttributes
						  {
							  SystemId = nvr.Id,
							  Name = nvr.Name,
							  Type = "NVR",
							  X = Convert.ToDouble(position.X),
							  Y = Convert.ToDouble(position.Y),
							  DescX = Convert.ToDouble(position.X),
							  DescY = Convert.ToDouble(position.Y + 60),
							  LinkToMap = String.Empty,
							  Status = nvr.NVRStatus
			};


			var id = _mapHandler.CreateNVRReturnId(CurrentMapId, nvrAttributes);

			OnSyncData();

			FindMapAndGoToMapById(CurrentMapId);

			if (id != null && OnModifyNVR != null)
			{
				OnModifyNVR(this, new EventArgs<String, String>(id, CurrentMapId));
			}

		}

		public void ReloadMap(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "mode");
			
			if(target == "refresh")
			{
				padCom.Maps.Clear();
				MapControlLoad();

				var defaultMap = _mapHandler.FindDefaultMap();

				CurrentMapId = defaultMap == null ? "Root" : defaultMap.Id;
				if (OnChangeMapFromEMap != null)
					OnChangeMapFromEMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", CurrentMapId)));

				panelControl.Visible = true;
				FindMapAndGoToMapById(CurrentMapId);
                trackBarForZoom.Enabled = buttonZoomIn.Enabled = buttonZoomOut.Enabled = CurrentMapId != "Root";
				//if (defaultMap != null)
				//{
				//    CurrentMapId = defaultMap.Id;
				//    ChangeMapsListSelected(CurrentMapId);
				//    if (OnChangeMapFromEMap != null)
				//        OnChangeMapFromEMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", CurrentMapId)));

				//    panelControl.Visible = true;
				//    FindMapAndGoToMapById(CurrentMapId);
				//}
				//else
				//{
				//    CurrentMapId = "-1";
				//    panelControl.Visible = false;
				//    padCom.ClearPadWindow();
				//    CurrentMapId = String.Empty;
				//}
			}
		}

		public void ChangeSetupMode(Object sender, EventArgs<Boolean> e)
		{
			padCom.Mode = _mode = e.Value ? "Setup":"View";

			RemoveAllCamerasActivate();
			//padCom.RemoveAllActivateCameraImage();
			padCom.ChangeAllItemsCursor();
			padCom.RemoveHopZonePoints();
			padCom.RemoveEventListPanel();

			if(_isVideoOnMap)
			{
				padCom.RemoveAllVideoWindow();
				if (!String.IsNullOrEmpty(CurrentMapId))
				{
					var maps = CMS.NVRManager.FindMapById(CurrentMapId);
					if (maps != null)
					{
						foreach (KeyValuePair<String, CameraAttributes> cam in maps.Cameras)
						{
							cam.Value.IsVideoWindowOpen = false;
							if (_mode == "View" && cam.Value.IsEventAlarm)
							{
								padCom.CreateEventAlarmByCamera(cam.Value);
							}
							else
							{
								padCom.RemoveEventAlarmByCameraId(cam.Key);
							}
						}
					}
				}

				panelVideoWindowSize.Visible = _mode == "Setup" ? false : true;
				FindMapAndGoToMapById(CurrentMapId);
			}

            if (_mode == "View")
                CMSOnNVRStatusReceive(this, null);
		}

		private void MapListSelectedIndexChanged(object sender, EventArgs e)
		{
			if(_mapLoadLock) return;
			SharedToolTips.SharedToolTip.SetToolTip(mapList, mapList.SelectedText);
			ChangeMapsListSelected(mapList.SelectedValue.ToString());
			FindMapAndGoToMapById(mapList.SelectedValue.ToString());
			OnChangeMapEvent(mapList.SelectedValue.ToString());
		}

		public void ChangeMapNameById(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");

			var map = CMS.NVRManager.FindMapById(target);
			if(map != null)
			{
				_mapLoadLock = true;
				ChangeListNameById(target, map.Name);
				_mapLoadLock = false;
			}

		}

		private void FindMapAndGoToMapById(string mapId)
		{
			var map = CMS.NVRManager.FindMapById(mapId);
			if(_mode == "View")
				panelVideoWindowSize.Visible = (map != null);
			if(map != null)
			{
				padCom.Scale = map.Scale;
                CurrentMapId = mapId;
                //if(CurrentMapId != "Root")
                //{
                //    var x = map.X;
                //    var y = map.Y;
                //    if (x == 0)
                //        x = (padCom.Width - map.Width) / 2;
                //    if (y == 0)
                //        y = (padCom.Height - map.Height) / 2;

                //    padCom.CreateAndShowMainWindow(x, y, map.Scale, map.ScaleCenterX, map.ScaleCenterY);
                //}
                //else
                //{
                //    padCom.CreateAndShowMainWindow(map.X, map.Y, map.Scale, map.ScaleCenterX, map.ScaleCenterY);
                //}

                padCom.CreateAndShowMainWindow(map.X, map.Y, map.Scale, map.ScaleCenterX, map.ScaleCenterY);

				_currentMap = map;
				padCom.CreateMapByObject(map);

				_currentClickCamera = String.Empty;
                _activateVideoWindow = null;

				foreach (KeyValuePair<string, CameraAttributes> camera in map.Cameras)
				{
					INVR nvr = CMS.NVRManager.FindNVRById(camera.Value.NVRSystemId);
					if (nvr == null) continue;
					if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
					if(nvr.Device.FindDeviceById(Convert.ToUInt16(camera.Value.SystemId)) == null) continue;
					foreach (KeyValuePair<ICamera, VideoWindow> vw in _previewScreen)
					{
						if (camera.Value.IsVideoWindowOpen && vw.Value.Camera.Id == camera.Value.SystemId && vw.Value.Camera.Server.Id == camera.Value.NVRSystemId)
						{
							//var cam = map.Cameras[camId];
							//vw.Key.Name = mapId;
							vw.Value.Name = camera.Key;
							var recreateSize = VideoWindowSizeList[camera.Value.VideoWindowSize];
							if (_cameraVideoWindowSize != 0 && _mode == "View")
							{
								recreateSize = VideoWindowSizeList[_cameraVideoWindowSize];
							}
							vw.Value.Size = recreateSize;
                            padCom.CreateVideoWindow(vw.Value, camera.Value.VideoWindowX, camera.Value.VideoWindowY, camera.Value,vw.Value.Active);
							if (vw.Value.Active)
							{
								_currentClickCamera = camera.Key;
                                _activateVideoWindow = vw.Value;

								//vw.Value.Active = true;
								padCom.ActivateCameraImage(camera.Key,true);
							}
						}
					}
					if(camera.Value.IsEventAlarm)
					{
						//padCom.CreateEventAlarmByCamera(camera.Value);
					}
				}

				foreach (KeyValuePair<ICamera, VideoWindow> vw in _previewScreen)
				{
					//vw.Value.Active = false;ffff
					var mapItem = _mapHandler.FindMapByCameraId(vw.Value.Name);
					if(mapItem != null)
					{
						if (mapItem.Id == CurrentMapId)
						{
							vw.Value.Activate();
                            vw.Value.Visible = true;
						}
						else
						{
							vw.Value.Deactivate();
                            vw.Value.Visible = false;
						}
					}
					
				}

				trackBarForZoom.Value = map.Scale;
			}
			else
			{
                CurrentMapId = mapId;
                padCom.Scale = 10;
				trackBarForZoom.Value = 10;
				padCom.CreateAndShowMainWindow(0, 0, 10, 0, 0);
				padCom.CreateRootMap();
			}
            trackBarForZoom.Enabled = buttonZoomIn.Enabled = buttonZoomOut.Enabled = CurrentMapId != "Root";
		}

		public void ChangeMapById(Object sender, EventArgs<String> e)
		{
			_mapLoadLock = true;
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");
			String mode = Xml.GetFirstElementValueByTagName(xmlDoc, "Mode");

			if (target == "-1" || String.IsNullOrEmpty(target))
			{
				panelControl.Visible = false;
				padCom.ClearPadWindow();
				CurrentMapId = String.Empty;
				return;
			}
			
			panelControl.Visible = true;
			
			if (String.IsNullOrEmpty(target) != true)
			{
				if (mapList.SelectedValue != null)
					//if(target != mapList.SelectedValue.ToString())
					//{
					if (mode == "ModifyImage")
					{
						var map = CMS.NVRManager.FindMapById(target);
						if(map != null)
						{
							//padCom.RemoveMapDictionaryByFileName(map.SystemFile);

							if(map.Image != null)
							{
								BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(map.Image.GetHbitmap(),
				IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
								if (padCom.Maps.ContainsKey(map.Id))
								{
									padCom.Maps[map.Id] = bitmapSource;
								}
								else
								{
									padCom.Maps.Add(map.Id,bitmapSource);
								}
							}
						}
					}
					else if (mode == "Refresh")
					{
		   
						var maplist = new List<String>(padCom.Maps.Keys);
						foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
						{
							maplist.Remove(map.Key);
						}
						foreach (string key in maplist)
						{
							padCom.RemoveMapDictionaryById(key);
						}
					}
						//ApplicationForms.ShowLoadingIcon(CMS.Form);
						FindMapAndGoToMapById(target);
						//ApplicationForms.HideLoadingIcon(CMS.Form);
				   // }
				CurrentMapId = target;
				//padCom.MainCanvasPoint = new System.Windows.Point(0,0);
				ChangeMapsListSelected(target);

                trackBarForZoom.Enabled = buttonZoomIn.Enabled = buttonZoomOut.Enabled = CurrentMapId != "Root";
			}
			_mapLoadLock = false;
			
		}

		public void StartAddVia(Object sender, EventArgs<ViaAttributes> e)
		{
			padCom.StartAddViaOnMap(e.Value);
		}


		private void padCom_OnCompleteAddingVia(object sender, EventArgs<ViaAttributes> e)
		{
			var id = _mapHandler.CreateViaReturnId(CurrentMapId, e.Value);

			if (OnModifyVia != null)
			{
				OnModifyVia(this, new EventArgs<String, String>(id, CurrentMapId));
			}

			FindMapAndGoToMapById(CurrentMapId);

			OnSyncData();
		}

		public void StartDrawHotZone(Object sender, EventArgs<MapHotZoneAttributes> e)
		{
			padCom.StartDrawHotZoneOnMap(e.Value);
		}

		private void padCom_OnCompleteDrawingHotZone(object sender, EventArgs<MapHotZoneAttributes> e)
		{
			var id = _mapHandler.CreateHotZoneReturnId(CurrentMapId, e.Value);

			padCom.GiveIdToCurrentHotZoneAfterComplete(id);

			if (OnClickDrawHotZone != null)
			{
				OnClickDrawHotZone(this, new EventArgs<String, String>(id, CurrentMapId));
			}

			OnSyncData();
		}

		private void padCom_OnClickHotZone(object sender, EventArgs<String> e)
		{
			var id = e.Value;
			if(!String.IsNullOrEmpty(id))
			{
				if(_mode == "View")
				{
					var hotzone = _mapHandler.FindHotZoneById(id);
					if (hotzone != null)
					{
						var target = hotzone.LinkToMap;
						if (String.IsNullOrEmpty(target) != true)
						{
							OnChangeMapEvent(target);
							FindMapAndGoToMapById(target);
							ChangeMapsListSelected(target);
						}
					}
				}
				else
				{
					padCom.RemoveAllActivateCameraImage();
					if(OnClickDrawHotZone != null)
					{
						OnClickDrawHotZone(this,new EventArgs<String, String>(id,CurrentMapId));
					}
				}
			}
		}

		private void padCom_OnChangeHotZonePoint(object sender, EventArgs<String,Int32, Point> e)
		{
			var id = e.Value1;
			if(!String.IsNullOrEmpty(id))
			{
				var hotzone = _mapHandler.FindHotZoneById(id);
				if (hotzone != null)
				{
					hotzone.Points[e.Value2] = e.Value3;

					if (OnClickDrawHotZone != null)
					{
						OnClickDrawHotZone(this, new EventArgs<String, String>(id, CurrentMapId));
					}
				}
			}
			OnSyncData();
		}

		public void ReadCameraDataUpdateById(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "Id");
			String mode = Xml.GetFirstElementValueByTagName(xmlDoc, "Mode");

			if (target == "-1")
			{
				padCom.ClearPadWindow();
				return;
			}

			if (String.IsNullOrEmpty(target) != true)
			{
			    NotifyMapSettingForModifyMapData();
				if (mode == "ViaDescription")
				{
					var via = _mapHandler.FindViaById(target);
					if (via != null)
					{
						var desc = via.Name;
						padCom.ModifyNameById(target, via.Type, desc);
					}
					return;
				}

				if (mode == "HotZoneDescription")
				{
					var zone = _mapHandler.FindHotZoneById(target);
					if (zone != null)
					{
						var desc = zone.Name;
						padCom.ModifyNameById(target, zone.Type, desc);
					}
					return;
				}

				if (mode == "NVRDescription")
				{
					var nvr = _mapHandler.FindNVRById(target);
					if(nvr != null)
					{
						var desc = nvr.Name;
						padCom.ModifyNameById(target, nvr.Type, desc);
					}
					return;
				}

				if (mode == "Remove")
				{
					FindMapAndGoToMapById(CurrentMapId);
					return;
				}

				if (mode == "Opacity")
				{
					String opacity = Xml.GetFirstElementValueByTagName(xmlDoc, "Opacity");
					padCom.UpdateHotZoneOpacity(target, Int32.Parse(opacity));
					return;
				}

				if (mode == "HotZoneColor")
				{
					var hotzone = _mapHandler.FindHotZoneById(target);
					if (hotzone != null)
					{
						padCom.UpdateHotZoneColor(target, hotzone.Color);
					}
					
					return;
				}

				var cam = _mapHandler.FindCameraById(target);
				if(cam != null)
				{
					if (mode == "Description")
					{
						var desc = cam.Name;
						padCom.ModifyNameById(target, "Camera", desc);
					}
					else if (mode == "Audio" || mode == "Speaker" || mode == "RemoveCamera")
					{
						FindMapAndGoToMapById(CurrentMapId);
					}
					else if (mode == "Rotate")
					{
						String rotate = Xml.GetFirstElementValueByTagName(xmlDoc, "Rotate");
						padCom.ModifyCameraRotate(target, Int32.Parse(rotate));
					}
					else if (mode == "VideoWindowResize")
					{
						var size = Int32.Parse(Xml.GetFirstElementValueByTagName(xmlDoc, "Size"));
						foreach (KeyValuePair<ICamera, VideoWindow> vw in _previewScreen)
						{
							if(vw.Value.Name == target)
							{
								padCom.RemoveVideoWindow(vw.Value);
								//vw.Key.Width = VideoWindowSizeList[size].Width;
								//vw.Key.Height = VideoWindowSizeList[size].Height;
								vw.Value.Width = VideoWindowSizeList[size].Width;
								vw.Value.Height = VideoWindowSizeList[size].Height;

								padCom.CreateVideoWindow(vw.Value, cam.VideoWindowX, cam.VideoWindowY, cam,true);
								//padCom.UpdateVideoWindow(vw.Key);
							}
						}
						
					}
				}
			}
			
		}

		private void OnChangeMapEvent(String mapId)
		{
			if(OnChangeMapFromEMap!=null && _mapLoadLock==false)
			{
				OnChangeMapFromEMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", mapId)));
			}
		}

		void padCom_OnMoveVia(object sender, EventArgs<ViaAttributes> e)
		{
			ViaAttributes node = e.Value;

			var via = _mapHandler.FindViaById(node.Id);
			if(via != null)
			{
				via.X = node.X;
				via.Y = node.Y;
				via.DescX = node.DescX;
				via.DescY = node.DescY;
			}
			padCom.RemoveAllActivateCameraImage();
			OnSyncData();
		}

		void padCom_OnMoveNVR(object sender, EventArgs<NVRAttributes> e)
		{
			NVRAttributes node = e.Value;

			var nvr = _mapHandler.FindNVRById(node.Id);
			if (nvr != null)
			{
				nvr.X = node.X;
				nvr.Y = node.Y;
				nvr.DescX = node.DescX;
				nvr.DescY = node.DescY;
			}
			padCom.RemoveAllActivateCameraImage();
			OnSyncData();
		}

		private void padCom_OnMoveCamera(object sender, EventArgs<CameraAttributes> e)
		{
			CameraAttributes node = e.Value;

			var cam = _mapHandler.FindCameraById(node.Id);
			if(cam != null)
			{
				cam.X = node.X;
				cam.Y = node.Y;
				cam.DescX = node.DescX;
				cam.DescY = node.DescY;
				cam.VideoWindowX = node.VideoWindowX;
				cam.VideoWindowY = node.VideoWindowY;
				if (node.IsSpeakerEnabled)
				{
					cam.SpeakerX = node.SpeakerX;
					cam.SpeakerY = node.SpeakerY;
				}

				if (node.IsAudioEnabled)
				{
					cam.AudioX = node.AudioX;
					cam.AudioY = node.AudioY;
				}
			}
			OnSyncData();
		}

		private void padCom_OnMoveVideoWindow(object sender, EventArgs<String,Double, Double> e)
		{
			var cam = _mapHandler.FindCameraById(e.Value1);
			if (cam != null)
			{
				cam.VideoWindowX = e.Value2;
				cam.VideoWindowY = e.Value3;
				OnSyncData();
			}
		}

		private void padCom_OnMoveDescription(object sender, EventArgs<CameraDescritpion> e)
		{
			CameraDescritpion node = e.Value;
			if (node.Type !="Camera")
			{
				switch (node.Type)
				{
					case "NVR":
						var nvr = _mapHandler.FindNVRById(node.Id);
						if (nvr != null)
						{
							nvr.DescX = node.DescX;
							nvr.DescY = node.DescY;
						}
						break;

					case "Via":
						var via = _mapHandler.FindViaById(node.Id);
						if (via != null)
						{
							via.DescX = node.DescX;
							via.DescY = node.DescY;
						}
						break;

					case "HotZone":
						var zone = _mapHandler.FindHotZoneById(node.Id);
						if(zone != null)
						{
							zone.DescX = node.DescX;
							zone.DescY = node.DescY;
						}
						break;
				}

				padCom.RemoveAllActivateCameraImage();
			}
			else
			{
				var cam = _mapHandler.FindCameraById(node.Id);
				if (cam != null)
				{
					cam.DescX = node.DescX;
					cam.DescY = node.DescY;
				}
				padCom.ActivateCameraImage(node.Id,true);
			}
			OnSyncData();
		}

		void padCom_OnMoveCameraAroundItem(object sender, EventArgs<CameraAroundItem> e)
		{
			CameraAroundItem node = e.Value;

			var cam = _mapHandler.FindCameraById(node.CameraId);
			if(cam != null)
			{
				if (node.Type == "Speaker")
				{
					cam.SpeakerX = node.X;
					cam.SpeakerY = node.Y;
				}
				else
				{
					cam.AudioX = node.X;
					cam.AudioY = node.Y;
				}
			}
		
			padCom.ActivateCameraImage(node.CameraId, true);
			OnSyncData();
		}

		private void OnSyncData()
		{
			 NotifyMapSettingForModifyMapData();
		}

		private void padCom_OnClickChangeMap(object sender, EventArgs<string> e)
		{
			var mapId = e.Value;
			OnChangeMapEvent(mapId);
			FindMapAndGoToMapById(mapId);
			ChangeMapsListSelected(mapId);
		}

		private void padCom_OnNVRClick(object sender, EventArgs<String> e)
		{
			var id = e.Value;

			var nvr = _mapHandler.FindNVRById(id);
			if(nvr != null)
			{
				var target = nvr.LinkToMap;
				if (String.IsNullOrEmpty(target) != true)
				{
					OnChangeMapEvent(target);
					FindMapAndGoToMapById(target);
					ChangeMapsListSelected(target);
				}
			}
		}

		private void padCom_OnViaClick(Object sender, EventArgs<String> e)
		{
			var id = e.Value;

			var via = _mapHandler.FindViaById(id);
			if(via != null)
			{
				var target = via.LinkToMap;
				if (String.IsNullOrEmpty(target) != true)
				{
					OnChangeMapEvent(target);
					FindMapAndGoToMapById(target);
					ChangeMapsListSelected(target);
				}
			}
		}

		public void SyncMapData(Object sender, EventArgs<String> e)
		{
			//CMS.NVR.MapDoc = Xml.LoadXml(e.Value);
		}

		void padCom_OnDescriptionEditClick(object sender, EventArgs<string, string> e)
		{
			var id = e.Value1;
			var type = e.Value2;

			switch (type)
			{
				case "Via":
					if (OnModifyVia != null)
					{
						OnModifyVia(this, new EventArgs<String, String>(id, CurrentMapId));
					}
					break;
				case "NVR":
					if (OnModifyNVR != null)
					{
						OnModifyNVR(this, new EventArgs<String, String>(id, CurrentMapId));
					}
					break;
				case "HotZone":
					if (OnClickDrawHotZone != null)
					{
						OnClickDrawHotZone(this, new EventArgs<String, String>(id,CurrentMapId));
					}
					break;
			}

			padCom.RemoveAllActivateCameraImage();
		}

		void padCom_OnCameraRightClick(object sender, EventArgs<String> e)
		{
			var camId = e.Value;

			if (camId != null && OnModifyCamera != null)
			{
				OnModifyCamera(this, new EventArgs<String, String>(camId, CurrentMapId));
				padCom.ActivateCameraImage(camId, true);
			}
		}

		private void padCom_OnCameraClick(Object sender, EventArgs<String> e)
		{
			var  camId  = e.Value;
		  
			var cam = _mapHandler.FindCameraById(camId);
			if(cam != null)
			{
				if (cam.IsVideoWindowOpen && _currentClickCamera == camId)
				{
					foreach (KeyValuePair<ICamera, VideoWindow> valuePair in _previewScreen)
					{
						if (valuePair.Value.Camera.Id == cam.SystemId && valuePair.Value.Camera.Server.Id == cam.NVRSystemId)
						{
							DisconnectVideoWindow(valuePair.Value);
							return;
						}
					}
				}
				else
				{
					CreateVideoWindowByCameraId(camId);
				}

				_currentClickCamera = cam.Id;
			}
		   
		}

		private String _currentClickCamera;
		
		private void CreateVideoWindowByCameraId(String camId)
		{
			CreateVideoWindowByCameraId(camId, DateTime.MinValue);
		}

		private void CreateVideoWindowByCameraId(String camId, DateTime dateTime)
		{
			
			var cam = _mapHandler.FindCameraById(camId);
			if (cam != null)
			{
				padCom.ActivateCameraImage(camId, true);
				var camSysId = Convert.ToUInt16(cam.SystemId);
				var camNVRSysId = Convert.ToUInt16(cam.NVRSystemId);
				INVR nvr = CMS.NVRManager.FindNVRById(camNVRSysId);
				if (nvr == null) return;
				if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) return;
				//if (nvr.ReadyState != ReadyState.Ready) return;
				if (!nvr.Device.Devices.ContainsKey(camSysId)) return;
				var camera = nvr.Device.Devices[camSysId] as ICamera;
				if (camera == null) return;

				var mapId = CurrentMapId;

				var createSize = VideoWindowSizeList[cam.VideoWindowSize];

				if (_cameraVideoWindowSize != 0 && _mode == "View")
				{
					createSize = VideoWindowSizeList[_cameraVideoWindowSize];
				}

				var checkRepeat = false;
				foreach (KeyValuePair<ICamera, VideoWindow> valuePair in _previewScreen)
				{
					if (valuePair.Value.Camera.Id == camSysId && valuePair.Value.Camera.Server.Id == camNVRSysId)
					{
						checkRepeat = true;
                        valuePair.Value.Viewer.Visible = true;
                        valuePair.Value.Visible = true;
						valuePair.Value.Active = true;
					   // _toolMenu.PanelPoint = valuePair.Key.Location;
						valuePair.Value.ToolMenu.Visible = true;
                        _activateVideoWindow = valuePair.Value;

						valuePair.Value.Focus();
						if (dateTime != DateTime.MinValue)
						{
							if(valuePair.Value.PlayMode == PlayMode.Playback1X)
							{
								valuePair.Value.StopInstantPlayback();
							}

							valuePair.Value.StartInstantPlayback(dateTime);
						}

						if (_isVideoOnMap)
						{
							var map = _mapHandler.FindMapByCameraId(valuePair.Value.Name);
							if (map != null)
							{
								if (mapId != map.Id)
								{
									valuePair.Value.Name = cam.Id;
									valuePair.Value.Activate();
									//valuePair.Key.Size = createSize;
									valuePair.Value.Size = createSize;
									padCom.CreateVideoWindow(valuePair.Value, cam.VideoWindowX, cam.VideoWindowY, cam, true);
									cam.IsVideoWindowOpen = true;
								}
								else
								{
									//VideoVindowClickAction(valuePair.Value);
									var result = padCom.ClickVideoWindowOnMap(valuePair.Value, cam);
									if (!result)
									{
										//DisconnectVideoWindow(valuePair.Value);
										//return;
									}
									//padCom.RemoveToolMenu();
								}
							}
							
						}
					}
					else
					{
						valuePair.Value.Active = false;
						//*valuePair.Value.ToolMenu.Visible = false;
					}
				}

				if (checkRepeat)
				{
					return;
				}
				var videoWindow = VideoWindowProvider.RegistVideoWindow();

				videoWindow.Server = Server;
				videoWindow.App = App;
                videoWindow.Initialize();
				videoWindow.DisplayTitleBar = true;
				//videoWindow.Dock = DockStyle.None;
				videoWindow.Dock = DockStyle.Fill;
				videoWindow.Location = new Point(0, 0);
				videoWindow.DisplayTitleBar = true;
                videoWindow.Viewer.Size = _isVideoOnMap ? createSize : new Size(330, 330);
				videoWindow.Size = _isVideoOnMap ? createSize : new Size(330, 330);
				videoWindow.Camera = camera;
				videoWindow.OnSelectionChange += VideoWindowClick;
				videoWindow.OnTitleBarClick += VideoWindowOnTitleBarClick;
                videoWindow.Viewer.Visible = true;
                videoWindow.Visible = true;
				videoWindow.Name = camId;
				if (dateTime != DateTime.MinValue)
				{
					videoWindow.Stretch = Server.Configure.StretchPlaybackVideo;
					videoWindow.StartInstantPlayback(dateTime);
				}
				else
				{
					videoWindow.Stretch = Server.Configure.StretchLiveVideo;
					videoWindow.Play();
				}
				videoWindow.ToolMenu = _toolMenu;
				videoWindow.Active = true;
				_toolMenu.PanelPoint = new Point(0, 0);
				_toolMenu.BringToFront();
				videoWindow.Focus();
				//videoWindow.GlobalMouseHandler();
				//wPanel.Controls.Add(videoWindow);
				_toolMenu.Dock = DockStyle.Top;
				//*wPanel.Controls.Add(_toolMenu);
                _activateVideoWindow = videoWindow;

				_previewScreen.Add(camera, videoWindow);
				if (_isVideoOnMap)
				{
					padCom.CreateVideoWindow(videoWindow, cam.VideoWindowX, cam.VideoWindowY, cam, true);
					cam.IsVideoWindowOpen = true;
				}
				else
				{
					if (panelScreen.Visible == false)
					{
						panelScreen.Visible = true;
					}
				}

				if (cam.IsEventAlarm)
				{
					padCom.CreateEventListByCamera(cam);
				}

				return;
			}
		}

		private void VideoWindowClick(Object sender, EventArgs e)
		{
			if (sender is VideoWindow)
			{
				foreach (KeyValuePair<ICamera, VideoWindow> obj in _previewScreen)
				{
					if (obj.Value == sender)
					{

						//*obj.Key.Controls.Add(_toolMenu);
						_toolMenu.BringToFront();
						_toolMenu.PanelPoint = new Point(0, 0);
                        obj.Value.Viewer.Visible = true;
						obj.Value.Active = true;
						obj.Value.ToolMenu.Visible = true;

						var map = _mapHandler.FindMapByCameraId(obj.Value.Name);
						if(map != null)
						{
							if (map.Id != CurrentMapId)
							{
								OnChangeMapEvent(map.Id);
								FindMapAndGoToMapById(map.Id);
								ChangeMapsListSelected(map.Id);
							}
						}
						
						padCom.ActivateCameraImage(obj.Value.Name, true);
						if (_isVideoOnMap)
						{
							var camId = obj.Value.Name;
							var cam = _mapHandler.FindCameraById(camId);
							if (cam != null)
							{
								var result = padCom.ClickVideoWindowOnMap(obj.Value, cam);
								if (result)
								{
									obj.Value.PtzMode = PTZMode.Disable;
									obj.Value.PtzMode = PTZMode.Digital;
									_toolMenu.Visible = true;
								}
								_currentClickCamera = cam.Id;
							    _activateVideoWindow = obj.Value;

                                if (cam.IsEventAlarm && obj.Value.PtzMode == PTZMode.Digital)
								{
									padCom.CreateEventListByCamera(cam);
								}
							}

							if (_mode == "Setup" && camId != null && OnModifyCamera != null)
							{
								OnModifyCamera(this, new EventArgs<String, String>(camId, CurrentMapId));
								padCom.ActivateCameraImage(camId, true);
							}
						}
					}
					else
					{
						obj.Value.Active = false;
						//obj.Value.ToolMenu.Visible = false;
					}
				}
				_toolMenu.Visible = true;
			}
		}

		private void VideoWindowOnTitleBarClick(object sender, EventArgs e)
		{
			var target = sender as VideoWindow;
			if (target != null)
			{
				var point = PointToClient(Cursor.Position);
				var newPoint = TransferPointToPadPoint(point.X, point.Y);
				padCom.DragVideoWindow(target.Name, newPoint.X, newPoint.Y);
			}
		}

		public void RemoveAllActivateCameras(Object sender, EventArgs<String> e)
		{
			if(_mode=="Setup")
			{
				RemoveAllCamerasActivate();
				padCom.RemoveAllActivateCameraImage();
			}
			
		}

		private void RemoveAllCamerasActivate()
		{
			foreach (KeyValuePair<ICamera, VideoWindow> obj in _previewScreen)
			{
				if(obj.Value.Active)
				{
					obj.Value.Active = false;
					padCom.ActivateCameraImage(obj.Value.Name,false);
					padCom.RemoveEventAlarmByCameraId(obj.Value.Name);
				}
			}
			padCom.RemoveToolMenu();
            if(_mode == "View")
			    _previewScreen.Clear();
			panelScreen.Controls.Clear();
			panelScreen.Visible = false;
		}

		private Menu _toolMenu;

		public void GlobalMouseHandler()
		{
			//if (_previewScreen == null)
			//{
			//    return;
			//}
			//foreach (KeyValuePair<Panel, VideoWindow> obj in _previewScreen)
			//{
			//    //*if (obj.Value.Active)
			//    //*obj.Value.GlobalMouseHandler();
			//    //if (obj.Value.Active)
			//    //obj.Value.GlobalMouseHandler();
			//}
		}

		protected void ToolManuButtonClick(Object sender, EventArgs<String> e)
		{
			VideoWindow activateVideoWindow = null;
			foreach (KeyValuePair<ICamera, VideoWindow> obj in _previewScreen)
			{
				if (obj.Value.Active)
				{
					activateVideoWindow = obj.Value;
					break;
				}
			}

			if (activateVideoWindow == null) return;

			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			String button = Xml.GetFirstElementValueByTagName(xmlDoc, "Button");
			//String status = Xml.GetFirstElementsValueByTagName(xmlDoc, "Status");

			switch (button)
			{
				case "Disconnect":
					DisconnectVideoWindow(activateVideoWindow);
					break;

				case "Playback":
					if (activateVideoWindow.Camera != null && activateVideoWindow.Viewer != null && activateVideoWindow.Track != null)
					{
						App.SwitchPage("Playback", new PlaybackParameter
													   {
															Device = activateVideoWindow.Camera,
															Timecode = DateTimes.ToUtc(activateVideoWindow.Track.DateTime, Server.Server.TimeZone),
															TimeUnit = activateVideoWindow.Track.UnitTime
													   });
					}
					break;
			}
		}

		private void DisconnectVideoWindow(VideoWindow activateVideoWindow)
		{
			if(_mode == "View")
			{
				padCom.ActivateCameraImage(activateVideoWindow.Name, false);
			}
			
			activateVideoWindow.ToolMenu.Visible = false;
			activateVideoWindow.ToolMenu.VideoWindow = null;

			activateVideoWindow.Active = false;
			if (_isVideoOnMap)
			{
				padCom.RemoveVideoWindow(activateVideoWindow);
				//padCom.RemoveEventListPanelByCameraId(activateVideoWindow.Name);
			}
			RemoveVideoWindow(activateVideoWindow.Camera);
		}

		private void RemoveVideoWindow(ICamera camera)
		{
			var vw = _previewScreen[camera]; 
			
			var camId = vw.Name;
			var cam = _mapHandler.FindCameraById(camId);
			var checkUse = false;
			if(cam != null)
			{
				if (cam.IsEventAlarm)
				{
					RemoveMapsCameraAlarmByCameraId(cam.Id);
					NotifyEventAlarmChanged();
				}
				
				cam.IsVideoWindowOpen = false;
				foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
				{
					foreach (KeyValuePair<string, CameraAttributes> obj in map.Value.Cameras)
					{
						if (obj.Key != camId)
						{
							if (obj.Value.IsVideoWindowOpen && cam.SystemId == obj.Value.SystemId && cam.NVRSystemId == obj.Value.NVRSystemId)
							{
								//device.Name = "-1";
								vw.Name = obj.Key;
								checkUse = true;
								break;
							}
						}
					}
				}
			}

			if (!checkUse)
			{
				vw.OnSelectionChange -= VideoWindowClick;
				vw.OnTitleBarClick -= VideoWindowOnTitleBarClick;
				vw.Deactivate();
				VideoWindowProvider.UnregisterVideoWindow(vw);
				//vw.Dispose();
				_previewScreen.Remove(camera);
			}
			
			if (_previewScreen.Count==0)
			{
				panelScreen.Visible = false;
			}
		}

		public void DisconnectAll(Object sender, EventArgs e)
		{
			foreach (KeyValuePair<ICamera, VideoWindow> obj in _previewScreen)
			{
				padCom.RemoveVideoWindow(obj.Value);
				obj.Value.Active = false;
				padCom.ActivateCameraImage(obj.Value.Name, false);
				//padCom.RemoveEventAlarmByCameraId(obj.Value.Name);
				obj.Value.Deactivate();
			}
		   
			padCom.RemoveToolMenu();
			_previewScreen.Clear();
			panelScreen.Controls.Clear();
			panelScreen.Visible = false;
			_currentClickCamera = String.Empty;
            _activateVideoWindow = null;
		}

		public void Activate()
		{
			_mapLoadLock = true;
			_mapHandler.SyncDeviceAndNVRName(CMS.NVRManager);

			padCom.RemoveAllActivateCameraImage();

			var defaultMap = _mapHandler.FindDefaultMap();
			if(defaultMap != null)
			{
				if (defaultMap.IsDefault)
				{
					if (CurrentMapId != defaultMap.Id)
					{
						ChangeMapsListSelected(defaultMap.Id);
						if (OnChangeMapFromEMap != null)
							OnChangeMapFromEMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", defaultMap.Id)));
					}
					CurrentMapId = defaultMap.Id;
				}   
			}
			else
			{
				CurrentMapId = "Root";
			}

			if (!String.IsNullOrEmpty(CurrentMapId)) FindMapAndGoToMapById(CurrentMapId);

			//foreach (KeyValuePair<ICamera, VideoWindow> videoWindow in _previewScreen)
			//{
			//    videoWindow.Value.Activate();
			//}
			_mapLoadLock = false;
			//panelFunction.Visible  = panelScaleFunction.Visible = mapList.Visible = panelVideoWindowSize.Visible = true;
		}

		public void Deactivate()
		{
			foreach (KeyValuePair<ICamera, VideoWindow> videoWindow in _previewScreen)
			{
				videoWindow.Value.Deactivate();
			}
			//panelFunction.Visible = panelScaleFunction.Visible = mapList.Visible = panelVideoWindowSize.Visible = false;
		}

		public String TitleName { get; set; }

		private void MapControlLoad()
		{           
			try
			{
				if(CMS == null) return;

				//Find default map and draw 
				if (CMS.NVRManager.Maps != null)
				{
					_mapLoadLock = true;
				  
					var defaultMap = _mapHandler.FindDefaultMap();

					LoadMapsList();

					CurrentMapId = defaultMap == null ? "Root" : defaultMap.Id;

					ChangeMapsListSelected(CurrentMapId);

					_mapHandler.SyncDeviceAndNVRName(CMS.NVRManager);
                    trackBarForZoom.Enabled = buttonZoomIn.Enabled = buttonZoomOut.Enabled = CurrentMapId != "Root";
					//if (defaultMap!=null)
					//{
					//    ChangeMapsListSelected(defaultMap.Id);
					//    //if(mapList.SelectedValue == )
					//    //{
							
					//    //}
					//    CurrentMapId = defaultMap.Id;

					//    _mapHandler.SyncDeviceAndNVRName(CMS.NVR);
					//}
					//else
					//{
					//    CurrentMapId = "-1";
					//}
					_mapLoadLock = false;
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		private Dictionary<String, UInt16> _backupMapLevel = new Dictionary<String, UInt16>();

		private void LoadMapsList()
		{
			//Map list
			//var maps = CMS.NVR.MapDoc.GetElementsByTagName("Map");
			_backupMapLevel.Clear();
			_mapsTable = new DataTable();
			_mapsTable.Columns.Add("MapName", typeof(String));
			_mapsTable.Columns.Add("MapId", typeof(String));

			_mapsTable.Rows.Add(Localization["EMap_AllMaps"], "Root");

            var sortResult = new List<MapAttribute>(CMS.NVRManager.Maps.Values);
            sortResult.Sort(SortByIdThenNVR);
            foreach (MapAttribute map in sortResult)
            {
                if (!String.IsNullOrEmpty(map.ParentId)) continue;
                _mapsTable.Rows.Add(map.Name, map.Id);
                _backupMapLevel.Add(map.Id, 0);
                ParseMapForList(map.Id, 0);
            }
            //foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
            //{
            //    if(!String.IsNullOrEmpty(map.Value.ParentId)) continue;
            //    _mapsTable.Rows.Add(map.Value.Name, map.Value.Id);
            //    _backupMapLevel.Add(map.Key, 0);
            //    ParseMapForList(map.Key, 0);
            //}

			mapList.DataSource = _mapsTable;
			panelScaleFunction.Visible = mapList.Visible = _mapsTable.Rows.Count > 0;
			labelAddMap.Visible = _mapsTable.Rows.Count == 0;
			mapList.DisplayMember = "MapName";
			mapList.ValueMember = "MapId";

			//mapList.DataBindings;
		}

        protected static Int32 SortByIdThenNVR(MapAttribute x, MapAttribute y)
        {
            if (x.Id != y.Id)
                return (Convert.ToInt16(x.Id) - Convert.ToInt16(y.Id));

            return (Convert.ToInt16(String.IsNullOrEmpty(x.ParentId) ? "0" : x.ParentId) - Convert.ToInt16(String.IsNullOrEmpty(y.ParentId) ? "0" : y.ParentId));
        }

		private void ParseMapForList(String parentId, UInt16 level)
		{
			var line = "";
			for (int i = 0; i <= level; i++)
                line = line + "-"; //line = "    " + line;

            level++;
			foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
			{
				if(map.Value.ParentId != parentId) continue;
				_mapsTable.Rows.Add(String.Format("{0} {1}", line, map.Value.Name), map.Value.Id);
				_backupMapLevel.Add(map.Key, level);
				ParseMapForList(map.Value.Id, level);
			}
		}

		private void ChangeMapsListSelected(String mapId)
		{
			var dataSource = mapList.DataSource as DataTable;

			if (dataSource != null)
				for (int i = 0; i < dataSource.Rows.Count; i++)
				{
					//開始檢查給定的值
					if ((string)dataSource.Rows[i][mapList.ValueMember] == mapId)
					{
						mapList.SelectedIndex = i;
						SharedToolTips.SharedToolTip.SetToolTip(mapList, mapList.Text);
						break;
					}
				}
		}

		private void ChangeListNameById(String mapId, String value)
		{
			var dataSource = mapList.DataSource as DataTable;

			if (dataSource != null)
				for (int i = 0; i < dataSource.Rows.Count; i++)
				{
					//開始檢查給定的值
					if ((string)dataSource.Rows[i][mapList.ValueMember] == mapId)
					{
						var level = 0;
						if (_backupMapLevel.ContainsKey(mapId))
							level = _backupMapLevel[mapId];
						var line = "";
						for (var j = 0; j <= level-1; j++)
                            line = line + "-"; //line = "    " + line;
						dataSource.Rows[i][mapList.DisplayMember] = String.Format("{0}{1}", line, value) ;
						
						SharedToolTips.SharedToolTip.SetToolTip(mapList, value);
						break;
					}
				}
		}

		public void ChangeMapCount(Object sender, EventArgs<String> e)
		{
			_mapLoadLock = true;
			LoadMapsList();
			_mapLoadLock = false;
		}

		private void TrackBarForZoomScroll(Object sender, EventArgs e)
		{
			if (_mapLoadLock) return;
            if (CurrentMapId == "Root") return;
			padCom.Scale = trackBarForZoom.Value;
			var map = CMS.NVRManager.FindMapById(CurrentMapId);
			padCom.ZoomCanvas(map);
		}

		private void PadComOnMoveMap(object sender, EventArgs<System.Windows.Point> e)
		{
			if (String.IsNullOrEmpty(CurrentMapId) == false && CurrentMapId != "-1" )
			{
				var map = CMS.NVRManager.FindMapById(CurrentMapId);
				if (map != null)
				{
					map.X = e.Value.X;
					map.Y = e.Value.Y;
				}

				OnSyncData();
			}
		}

		private void PadComOnScaleMap(object sender, EventArgs<double, System.Windows.Point> e)
		
		{
			if (String.IsNullOrEmpty(CurrentMapId) == false && CurrentMapId != "-1" && _mapLoadLock==false)
			{
				var map = CMS.NVRManager.FindMapById(CurrentMapId);
				if(map != null)
				{
					map.Scale = Convert.ToInt32(e.Value1);
					map.ScaleCenterX = Convert.ToDouble(e.Value2.X);
					map.ScaleCenterY = Convert.ToDouble(e.Value2.Y);
					OnSyncData();
				}
			}
		}

		private void ZoomOutClick(object sender, EventArgs e)
		{
            if (CurrentMapId == "Root") return;
			if (trackBarForZoom.Value == trackBarForZoom.Minimum) return;
			trackBarForZoom.Value--;
		}

		private void ZoomInClick(object sender, EventArgs e)
		{
            if (CurrentMapId == "Root") return;
			if (trackBarForZoom.Value == trackBarForZoom.Maximum) return;
			trackBarForZoom.Value++;
		}

		private void ZoomDefaultClick(object sender, EventArgs e)
		{
			trackBarForZoom.Value = 10;
		}

		private void ButtonMapMoveClick(object sender, EventArgs e)
		{
			var currentScale = Convert.ToDouble(trackBarForZoom.Value)/10;
			var  movePx =100 * currentScale;
			var target = sender as Button;
			if (target != null)
			{
				padCom.MoveMainCanvasByDistant(target.Name.Replace("button",""), movePx);
			}
		}

		private void MapBackClick(Object sender, EventArgs e)
		{
			padCom.MoveMainCanvasToOriginal();
		}

		private void WindowSizeChangeClick(Object sender, EventArgs e)
		{
			var target = sender as Button;
			if (target == null) return;

			if (_cameraVideoWindowSize == 1)
			{
				if (target == buttonWindowSizeSmall)
				{
					return;
				}
				if (buttonWindowSizeSmall != null) buttonWindowSizeSmall.BackgroundImage = _smallWindowIcon;
			}
			else if (_cameraVideoWindowSize == 2)
			{
				if (target.Name == "buttonWindowSizeMedium")
				{
					return;
				}
				buttonWindowSizeMedium.BackgroundImage = _mediumWindowIcon;
			}
			else if (_cameraVideoWindowSize == 3)
			{
				if (target.Name == "buttonWindowSizeLarge")
				{
					return;
				}
				buttonWindowSizeLarge.BackgroundImage = _largeWindowIcon;
			}
			else if (_cameraVideoWindowSize == 0)
			{
				if (target.Name == "buttonDefaultVideoSize")
				{
					return;
				}
				buttonDefaultVideoSize.BackgroundImage = _defaultWindowIcon;
			}

			var size = 0;

			if (target.Name == "buttonWindowSizeSmall" && _cameraVideoWindowSize != 1)
			{
				//if (target.BackgroundImage == Properties.Resources.small_activate) return;
				target.BackgroundImage = _smallWindowIconActivate;
				   
				size = 1;
			}
			else if (target.Name == "buttonWindowSizeMedium" && _cameraVideoWindowSize != 2)
			{
				//if (target.BackgroundImage == Properties.Resources.medium_activate) return;
				target.BackgroundImage = _mediumWindowIconActivate;
				   
				size = 2;
			}
			else if (target.Name == "buttonWindowSizeLarge" && _cameraVideoWindowSize != 3)
			{
				//if (target.BackgroundImage == Properties.Resources.large_activate) return;
				target.BackgroundImage = _largeWindowIconActivate;
				   
				size = 3;
			}
			else if (target.Name == "buttonDefaultVideoSize" && _cameraVideoWindowSize != 0)
			{
				//if (_cameraVideoWindowSize == 0) return;
				target.BackgroundImage = _defaultWindowIconActivate;
					
				size = 0;
			}
			
			//padCom.RemoveToolMenu();

			foreach (KeyValuePair<ICamera, VideoWindow> vw in _previewScreen)
			{
				var cam = _mapHandler.FindCameraById(vw.Value.Name);
				if (cam == null) continue;
				if (!cam.IsVideoWindowOpen) continue;
				var newSize = new Size();
				if (size == 0)
				{
				   // if (cam.VideoWindowSize != _cameraVideoWindowSize)
					//{
						//padCom.RemoveVideoWindow(vw.Key);
					newSize = new Size(VideoWindowSizeList[cam.VideoWindowSize].Width,
										   VideoWindowSizeList[cam.VideoWindowSize].Height);
					//}
				}
				else
				{
					//padCom.RemoveVideoWindow(vw.Key);
					newSize = new Size(VideoWindowSizeList[size].Width,
										   VideoWindowSizeList[size].Height);

				}

				if (newSize.Width != vw.Value.Width || newSize.Height != vw.Value.Height)
				{
					vw.Value.Width = newSize.Width;
					vw.Value.Height = newSize.Height;
					var map = _mapHandler.FindMapByCameraId(vw.Value.Name);
					if (map != null)
					{
						if (map.Id == CurrentMapId)
						{
							padCom.UpdateVideoWindow(vw.Value);
						}
					}
				}
			}

			_cameraVideoWindowSize = size;
		}

		private void padCom_OnClickAlarm(object sender, EventArgs<String> e)
		{
			var cam = _mapHandler.FindCameraById(e.Value);
			if(cam != null)
			{
				padCom.CreateEventListByCamera(cam);
			}
		}

		private void padCom_OnClickEventList(object sender, EventArgs<String,DateTime> e)
		{
			var camId = e.Value1;
			var cam = _mapHandler.FindCameraById(camId);
			if(cam != null)
			{
				CreateVideoWindowByCameraId(camId, e.Value2);
				cam.IsVideoWindowOpen = true;
				_currentClickCamera = camId;
				//cam.IsEventAlarm = true;
                var cams = _mapHandler.FindCameraByDeviceIdNVRSystemId(Convert.ToUInt16(cam.SystemId), cam.NVRSystemId);
                foreach (CameraAttributes camera in cams)
                {
                    foreach (CameraEventRecord record in camera.EventRecords)
                    {
                        if (record.EventTime == e.Value2)
                        {
                            record.IsRead = true;
                        }
                    }
                    camera.EventRecords.RemoveAll(a => a.IsRead);

                    if (camera.EventRecords.Count == 0)
                    {
                        RemoveMapsCameraAlarmByCameraId(camera.Id);
                        NotifyEventAlarmChanged();
                    }
                }
			}
		}

		private void padCom_OnClickRemoveAllEvent(object sender, EventArgs<string> e)
		{
			var camId = e.Value;
			var cam = _mapHandler.FindCameraById(camId);
			if(cam != null)
			{
                var cams = _mapHandler.FindCameraByDeviceIdNVRSystemId(Convert.ToUInt16(cam.SystemId), cam.NVRSystemId);
                foreach ( CameraAttributes camera in cams)
                {
                    camera.EventRecords.Clear();
                }
			    RemoveMapsCameraAlarmByCameraId(cam.Id);
				padCom.RemoveEventListPanel();
				NotifyEventAlarmChanged();
			}
		}

		private void RemoveMapsCameraAlarmByCameraId(String camId)
		{
			var cam = _mapHandler.FindCameraById(camId);
			if(cam != null)
			{
				if (cam.EventRecords.Count == 0)
				{
					padCom.RemoveEventAlarmByCameraId(camId);
					cam.IsEventAlarm = false;

					var alarmCams = _mapHandler.FindCameraByDeviceIdNVRSystemId(cam.SystemId, cam.NVRSystemId);

					foreach (CameraAttributes alarmCam in alarmCams)
					{
						if (alarmCam.IsEventAlarm)
						{
							alarmCam.IsEventAlarm = false;
						}
					}
				}
			}
		}

		public void ClickEvent(Object sender, EventArgs<IDevice, DateTime> e)
		{
			var device = e.Value1;

			var maps = _mapHandler.FindCameraByDeviceIdNVRSystemIdReturnMap(device.Id, device.Server.Id);
			if (maps != null)
			{
			    MapAttribute map = null;
                if (_currentMap != null && maps.ContainsKey(_currentMap.Id))
                {
                    map = maps[_currentMap.Id];
                }
                else
                {
                    foreach (KeyValuePair<String, MapAttribute> mapAttribute in maps)
                    {
                        _currentMap = map = mapAttribute.Value;
                        break;
                    }
                }

                if(map != null)
                {
                    var cam = _mapHandler.FindCameraByDeviceIdNVRSystemId(device.Id, device.Server.Id);
                    if (cam.Count > 0)
                    {
                        CameraAttributes camera = null;
                        foreach (CameraAttributes cameraAttributese in cam)
                        {
                            if(_currentMap.Cameras.ContainsKey(cameraAttributese.Id))
                            {
                                camera = cameraAttributese;
                                break;
                            }
                        }
                        
                        if(camera != null)
                        {
                            INVR nvr = CMS.NVRManager.FindNVRById(camera.NVRSystemId);
                            if (nvr == null) return;

                            if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) return;

                            if (nvr.Device.FindDeviceById(Convert.ToUInt16(camera.SystemId)) == null) return;

                            var mapId = map.Id;
                            mapList.SelectedValue = mapId;

                            CreateVideoWindowByCameraId(camera.Id, e.Value2);
                            camera.IsVideoWindowOpen = true;
                            _currentClickCamera = camera.Id;
                            padCom.CreateEventAlarmByCamera(camera);
                            camera.IsEventAlarm = true;
                        }
                        
                    }
                }
				else
                {
                    CameraIsNotInTheAnyMaps(e);
                }
			}
			else
			{
				CameraIsNotInTheAnyMaps(e);
			}
		}

		private void CameraIsNotInTheAnyMaps(EventArgs<IDevice, DateTime> e)
		{
			var device = e.Value1;

			var warnString = Localization["EMap_MessageEventWithNoCameraInMap"].Replace("%1", String.Format("{0} {1}", device, device.Server));

			DialogResult result = TopMostMessageBox.Show(warnString, Localization["MessageBox_Information"],
										 MessageBoxButtons.OK, MessageBoxIcon.Question);

			if (result == DialogResult.OK)
			{
				App.PopupInstantPlayback(e.Value1, DateTimes.ToUtc(e.Value2, Server.Server.TimeZone));
			}
		}

        public void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (e.Value == null) return;

			var check = 0;

            //VideoWindow activateVideoWindow = null;
            //foreach (KeyValuePair<ICamera, VideoWindow> obj in _previewScreen)
            //{
            //    if (obj.Value.Active)
            //    {
            //        activateVideoWindow = obj.Value;
            //        break;
            //    }
            //}

            if (_activateVideoWindow != null)
            {
                if (_activateVideoWindow.ToolMenu != null)
                    _activateVideoWindow.ToolMenu.CheckStatus();
            }

			foreach (ICameraEvent cameraEvent in e.Value)
			{
				if (cameraEvent.Device == null) continue;

				var cams = _mapHandler.FindCameraByDeviceIdNVRSystemId(cameraEvent.Device.Id, cameraEvent.NVR.Id);

				if (cams.Count>0)
					foreach ( CameraAttributes camera in cams)
					{
						INVR nvr = CMS.NVRManager.FindNVRById(camera.NVRSystemId);
						if (nvr == null) continue;
						if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;

						if (nvr.Device.FindDeviceById(Convert.ToUInt16(camera.SystemId)) == null) continue;

						if (!camera.IsEventAlarm)
						{
							var map = _mapHandler.FindMapByCameraId(camera.Id);
							if (_mode == "View" && map != null && !camera.IsEventAlarm)
							{
								if(map.Id == CurrentMapId)
									padCom.CreateEventAlarmByCamera(camera);

								if (CurrentMapId == "Root")
									padCom.CreateMapEventAlarmById(map.Id);
									
							}
							camera.IsEventAlarm = true;
							check++;
						}
						camera.EventRecords.Add(new CameraEventRecord{EventTime = cameraEvent.DateTime,IsRead = false});
                        while (camera.EventRecords.Count > MaxLog)
                        {
                            camera.EventRecords.Remove(camera.EventRecords[0]);
                        }
					}

			}
			if(check > 0)
			{
				NotifyEventAlarmChanged();
			}
			
		}

		private void NotifyEventAlarmChanged()
		{
			if (OnLogEventAlarmToMaps != null)
			{
				OnLogEventAlarmToMaps(this, new EventArgs<String>(""));
			}
		}

	}
}
