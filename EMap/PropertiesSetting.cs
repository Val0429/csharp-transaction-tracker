using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace EMap
{
	public partial class PropertiesSetting : UserControl, IControl, IMinimize, IServerUse
	{
		//minimize event
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnChangeMapFromProperties;
		public event EventHandler<EventArgs<String>> OnChangeMapNameFromProperties;
		public event EventHandler<EventArgs<String>> OnModifyCameraFromProperties;
		public event EventHandler<EventArgs<String>> OnModifyAnyAboutMap;
		public event EventHandler<EventArgs<String>> OnRemoveAllActivateCameras;
		public event EventHandler<EventArgs<MapHotZoneAttributes>> OnDrawHotZone;
		public event EventHandler<EventArgs<ViaAttributes>> OnAddVia;

		//create new PanelTitleBar
		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();
		public Dictionary<String, String> Localization;
		public MapHandler MapHandler
		{
			set
			{
				_mapHandler = value;
			}
		}
		private MapHandler _mapHandler;
		private String _currentId;
		private DataTable _mapsTable;
		private Boolean _propertyLoadLock;
		private String _currentMapId;
		protected ICMS CMS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is ICMS)
					CMS = value as ICMS;
			}
		}
		private String _status;
		private static readonly Image _smallWindowIcon = Resources.GetResources(Properties.Resources.small, Properties.Resources.IMGSmallWindow);
		private static readonly Image _smallWindowIconActivate= Resources.GetResources(Properties.Resources.small_activate, Properties.Resources.IMGSmallWindowActivate);
		private static readonly Image _mediumWindowIcon = Resources.GetResources(Properties.Resources.medium, Properties.Resources.IMGMediumWindow);
		private static readonly Image _mediumWindowIconActivate = Resources.GetResources(Properties.Resources.medium_activate, Properties.Resources.IMGMediumWindowActivate);
		private static readonly Image _largeWindowIcon = Resources.GetResources(Properties.Resources.large, Properties.Resources.IMGLargeWindow);
		private static readonly Image _largeWindowIconActivate= Resources.GetResources(Properties.Resources.large_activate, Properties.Resources.IMGLargeWindowActivate);
		protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
		protected readonly PanelTitleBarUI2 PanelPropertyTitleBarUI2 = new PanelTitleBarUI2();

		//minimize setting
		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		public Button Icon { get; private set; }

		public String TitleName { get; set; }

		public PropertiesSetting()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Properties", "Properties"},

								   {"MessageBox_Confirm", "Confirm"},
								   {"MessageBox_Information", "Information"},
								   {"EMap_Name", "Name"},
								   {"EMap_File", "File"},
								   {"EMap_DefaultMap", "Default Map"},
								   {"EMap_MapTransferTooltipForPoint", "Quick to add map transfer."},
								   {"EMap_MapTransferTooltipForZone", "Add map transfer by using polygon."},
								   {"EMap_MapTransferUsing", "Using"},
								   {"EMap_ButtonAddTransfer", "Add Map Link"},
								   {"EMap_ButtonDrawingHotZone", "Draw Zone Link"},
								   {"EMap_Description", "Description"},
								   {"EMap_Angle", "Angle"},
								   {"EMap_AudioIn", "Audio in"},
								   {"EMap_AudioOut", "Audio out"},
								   {"EMap_Remove", "Remove"},
								   {"EMap_LinkToMap", "Link To Map"},
								   {"EMap_MessageBoxRemoveCameraConfirm", "Do you want to delete the camera  \"%1\" in map?"},
								   {"EMap_MessageBoxRemoveViaConfirm", "Do you want to delete the  transfer point  \"%1\" in map?"},
								   {"EMap_MessageBoxRemoveNVRConfirm", "Do you want to delete the  NVR \"%1\" in map?"},
								   {"EMap_MessageBoxRemoveTransferZoneConfirm", "Do you want to delete this  transfer region in map?"},
								   {"EMap_NewViaName", "Link to"},
								   {"EMap_SelectFilePath", "Select file path"},
								   {"EMap_MapNameEmpty", "The name of map can't be empty."},
								   {"EMap_MessageBoxUploadMapFail", "Upload failed. Please try again."},
								   {"EMap_MessageBoxUploadMapWrongGetExtension", "Upload failed. Wrong file format."},
								   {"EMap_RemoveCameraNVRViaToolTip", "Remove  %1"},
								   {"EMap_VideoWindowSize", "Video Window Size"},
								   {"EMap_VideoWindowSizeLarge", "Large"},
								   {"EMap_VideoWindowSizeMedium", "Medium"},
								   {"EMap_VideoWindowSizeSmall", "Small"},
								   {"EMap_HotZoneNewName", "New Transfer Region"},
								   {"EMap_HotZoneOpacity", "Opacity"},
								   {"EMap_HotZoneOpacityHigh", "High"},
								   {"EMap_HotZoneOpacityLow", "Low"},
								   {"EMap_HotZoneTooltipSelectColor", "Select Color"},
								   {"EMap_HotZoneDragToOpacity", "Drag to set opacity of the  polygon."},
								   {"EMap_CameraDragToAngle", "Drag to rotate angle of the camera. "},

								   {"EMap_SelectedMap", "Selected Map"},
								  {"EMap_SelectedCamera", "Selected Camera"},
								  {"EMap_SelectedNVR", "Selected NVR"},
								  {"EMap_SelectedMapLink", "Selected Map Link"},
								  {"EMap_SelectedHotZone", "Selected Zone Link"},
							   };
			Localizations.Update(Localization);

			//if (CMS == null) return;

			InitializeComponent();
			Dock = DockStyle.Fill;
			//---------------------------
			Icon = new ControlIconButton { Image = Properties.Resources.map };
			Icon.Click += DockIconClick;
			//---------------------------
		}

		public void Initialize()
		{
			PanelTitleBarUI2.Text = TitleName = Localization["Control_Properties"];
			panelTitle.Controls.Add(PanelTitleBarUI2);

			PanelPropertyTitleBarUI2.Text = Localization["EMap_SelectedMap"];
			panelMapTitle.Controls.Add(PanelPropertyTitleBarUI2);
			//_panelTitleBar.Text = TitleName = Localization["Control_Properties"];
			//_panelTitleBar.Panel = this;
			//panelTitle.Controls.Add(_panelTitleBar);
           
			labelName.Text = Localization["EMap_Name"];
			labelImage.Text = Localization["EMap_File"];
			labelIsDefault.Text = Localization["EMap_DefaultMap"];
			buttonAddTransfer.Text = Localization["EMap_ButtonAddTransfer"];
			buttonAddHotZone.Text = Localization["EMap_ButtonDrawingHotZone"];
			labelUsing.Text = Localization["EMap_MapTransferUsing"];
			labelMapLinkToMap.Text = Localization["EMap_LinkToMap"];

			labelCameraAngle.Text = Localization["EMap_Angle"];
			buttonRomoveCamera.Text = Localization["EMap_Remove"];
			labelShowVideoWindowSize.Text = Localization["EMap_VideoWindowSize"];

			labelNVRLinkToMap.Text = Localization["EMap_LinkToMap"];
			buttonRemoveNVR.Text = Localization["EMap_Remove"];

			labelViaDescription.Text = Localization["EMap_Description"];
			labelViaLinkTo.Text = Localization["EMap_LinkToMap"];
			buttonRomoveVia.Text = Localization["EMap_Remove"];

            labelHotZoneName.Text = Localization["EMap_Name"];
			labelHotZoneOpacity.Text = Localization["EMap_HotZoneOpacity"];
			labelHotZoneOpacityHigh.Text = Localization["EMap_HotZoneOpacityHigh"];
			labelHotZoneOpacityLow.Text = Localization["EMap_HotZoneOpacityLow"];
			labelHotZoneLinkToMap.Text = Localization["EMap_LinkToMap"];
			buttonRemoveHotZone.Text = Localization["EMap_Remove"];
			//language end

			_mapHandler = new MapHandler { CMS = CMS };
		   
			textBoxName.TextChanged += TextBoxNameTextChanged;
			textBoxName.LostFocus += TextBoxNameTextLostFocus;
			checkBoxIsDefault.Click += CheckBoxIsDefaultCheckedChanged;
			buttonEditImage.Click += ButtonEditImageClick;
			CameraRotate.ValueChanged += CameraRotateValueChanged;
			trackBarOpacity.ValueChanged += HotZoneOpacityValueChanged;
			comboBoxMaps.SelectedValueChanged += new EventHandler(ComboBoxMapsSelectedValueChanged);
			//checkBoxDefaultShowWindow.Click += new EventHandler(CheckBoxDefaultShowWindowClick);
            //textBoxViaName.TextChanged += TextBoxViaDescriptionLostFocus;
			textBoxViaName.TextChanged += TextBoxViaDescriptionLostFocus;
			comboBoxMapListVia.SelectedValueChanged += ComboBoxMapListViaSelectedValueChanged;
			comboBoxNVRMaps.SelectedIndexChanged += ComboBoxNVRMapsSelectedValueChanged;
			textBoxHotZoneName.TextChanged += TextBoxHotZoneNameTextChanged;
			comboBoxHotZoneMaps.SelectedIndexChanged += ComboBoxHotZoneMapsSelectedIndexChanged;

			SharedToolTips.SharedToolTip.SetToolTip(buttonEditImage, Localization["EMap_SelectFilePath"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonRomoveCamera, Localization["EMap_RemoveCameraNVRViaToolTip"].Replace("%1","Ctrl + D"));
			SharedToolTips.SharedToolTip.SetToolTip(buttonRemoveNVR, Localization["EMap_RemoveCameraNVRViaToolTip"].Replace("%1", "Ctrl + D"));
			SharedToolTips.SharedToolTip.SetToolTip(buttonRomoveVia, Localization["EMap_RemoveCameraNVRViaToolTip"].Replace("%1", "Ctrl + D"));
			SharedToolTips.SharedToolTip.SetToolTip(buttonRemoveHotZone, Localization["EMap_RemoveCameraNVRViaToolTip"].Replace("%1", "Ctrl + D"));
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeLarge, Localization["EMap_VideoWindowSizeLarge"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeMedium, Localization["EMap_VideoWindowSizeMedium"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonWindowSizeSmall, Localization["EMap_VideoWindowSizeSmall"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonAddTransfer, Localization["EMap_MapTransferTooltipForPoint"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonAddHotZone, Localization["EMap_MapTransferTooltipForZone"]);
			SharedToolTips.SharedToolTip.SetToolTip(buttonSelectColor, Localization["EMap_HotZoneTooltipSelectColor"]);
			SharedToolTips.SharedToolTip.SetToolTip(trackBarOpacity, Localization["EMap_HotZoneDragToOpacity"]);
			SharedToolTips.SharedToolTip.SetToolTip(CameraRotate, Localization["EMap_CameraDragToAngle"]);
			Visible = false;

			buttonEditImage.BackgroundImage = Resources.GetResources(Properties.Resources.SelectFolder, Properties.Resources.IMGSelectFolder);

			panelColorTable.Paint += PanelColorTablePaint;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void ReloadMap(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "mode");

			if (target == "refresh")
			{

				if(CMS.NVRManager.Maps != null)
				{
					var defaultMap = _mapHandler.FindDefaultMap();

					if(defaultMap != null)
					{
						LoadMapInfo(defaultMap.Id);
					}
					else
					{
						HideAllPanels();
					}
				}

			}

		}

		private static readonly CultureInfo _enus = new CultureInfo("en-US");
		void ButtonEditImageClick(object sender, EventArgs e)
		{
			using (var file = new OpenFileDialog())
			{
				file.Filter = @"Image Files(*.jpg *.png *.bmp *.gif)|*.jpg;*.png;*bmp;*.gif";
				file.ShowDialog();
				if (String.IsNullOrEmpty(file.FileName))
				{
					return;
				}

				if (file.Filter.IndexOf(Path.GetExtension(file.FileName).ToLower(_enus)) < 0)
				{

					TopMostMessageBox.Show(Localization["EMap_MessageBoxUploadMapWrongGetExtension"], Localization["MessageBox_Information"],
											   MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				var ext = Path.GetExtension(file.FileName);

				var finalFileName = String.Format("Map{0}{1}", _currentId, ext);

				var mapFile = new Bitmap(file.FileName);
				//Boolean restult = CMS.NVR.UploadMap(mapFile, finalFileName);

				//if(restult == false)
				//{
				//    TopMostMessageBox.Show(Localization["EMap_MessageBoxUploadMapFail"], Localization["MessageBox_Information"]);
				//    return;
				//}

				//var image = Image.FromFile(file.FileName);

				var map = CMS.NVRManager.FindMapById(_currentId);
				if(map != null)
				{
					map.Image = mapFile;
					map.OriginalFile = file.SafeFileName;
					map.SystemFile = finalFileName;
					map.Width = mapFile.Width;
					map.Height = mapFile.Height;
				}

				textBoxImage.Text = file.SafeFileName;

				OnSyncData();
				ChangeMap(_currentId,"ModifyImage");
			}
		}

		private void ChangeMap(String mapId,String mode)
		{
			if (OnChangeMapFromProperties != null)
				OnChangeMapFromProperties(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId><Mode>{1}</Mode></xml>", mapId,mode)));
		}

		private void CheckBoxIsDefaultCheckedChanged(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId))
			{
				return;
			}

			if (checkBoxIsDefault.Checked)
			{
				foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
				{
					map.Value.IsDefault = map.Key == _currentId ? true: false;
				}
			}
			else
			{
				var map = CMS.NVRManager.FindMapById(_currentId);
				if(map != null)
				{
					map.IsDefault = false;
				}
			}

			OnSyncData();
		}

	 
		private void TextBoxNameTextChanged(Object sender, EventArgs e)
		{
			if(String.IsNullOrEmpty(_currentId) || panelMap.Visible == false || _propertyLoadLock)
			{
				return;
			}

			var map = CMS.NVRManager.FindMapById(_currentId);

			if(map != null)
			{
				if (textBoxName.Text == map.Name)
				{
					return;
				}

				map.Name = textBoxName.Text;
				SharedToolTips.SharedToolTip.SetToolTip(textBoxName, textBoxName.Text);
			}

			OnSyncData();

			if (OnChangeMapNameFromProperties != null)
			{
				OnChangeMapNameFromProperties(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", _currentId)));
			}
		}

		private void TextBoxNameTextLostFocus(Object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId) || panelMap.Visible == false || _propertyLoadLock)
			{
				return;
			}

			var map = CMS.NVRManager.FindMapById(_currentId);

			if(map != null)
			{
				if (String.IsNullOrEmpty(textBoxName.Text))
				{
					TopMostMessageBox.Show(Localization["EMap_MapNameEmpty"], Localization["MessageBox_Information"],
											   MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxName.Text = map.Name;
					return;
				}
			}

		}

		private void OnSyncData()
		{
			 NotifyMapSettingForModifyMapData();
		}

		private void NotifyMapSettingForModifyMapData()
		{
			if (OnModifyAnyAboutMap != null)
			{
				OnModifyAnyAboutMap(this, new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId></xml>", _currentMapId)));
			}
		}

		public void ChangeSetupMode(Object sender, EventArgs<Boolean> e)
		{
			Visible = e.Value;
			if (Visible)
			{
				Server.Form.KeyDown += FormKeyDown;
				Server.Form.KeyUp += FormKeyUp;
				LoadMapInfo(_currentMapId);
				if(String.IsNullOrEmpty(_currentId) || _currentMapId == "Root")
				{
					panelMapTitle.Visible = panelMap.Visible = false;
				}
				else
				{
					panelMapTitle.Visible = panelMap.Visible = true;
				}
			}
			else
			{
				Server.Form.KeyDown -= FormKeyDown;
				Server.Form.KeyUp -= FormKeyUp;
				panelMapTitle.Visible = panelMap.Visible = Visible;
			}
		}

		private Int32 _currentKey;

		void FormKeyUp(object sender, KeyEventArgs e)
		{
			_currentKey = -1;
		}

		private void FormKeyDown(object sender, KeyEventArgs e)
		{
			if ((_currentKey == 17 && e.KeyValue == 68) || (_currentKey == 68 && e.KeyValue == 17))
			{
		 
				switch (_status)
				{
					case "Camera":
						ButtonRomoveCameraClick(this,new EventArgs());
						break;

					case "NVR":
						ButtonRemoveNVRClick(this,new EventArgs());
						break;

					case "Via":
						ButtonRomoveViaClick(this,new EventArgs());
						break;

					case "HotZone":
						ButtonRemoveHotZoneClick(this, new EventArgs());
						break;
				}
				_currentKey = -1;
				return;
			}
			_currentKey = e.KeyValue;
			
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else
				Minimize();
		}

		public void Minimize()
		{
			IsMinimize = true;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
			if (!_showProperty) return;

			IsMinimize = false;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}

		private Boolean _showProperty;
		public void ChangeBoxHeight(Object sender, EventArgs<Boolean> e)
		{
			_showProperty = e.Value;
			if(e.Value)
			{
				Maximize();
			}
			else
			{
				Minimize();
			}
		}

		public void SyncMapData(Object sender, EventArgs<String> e)
		{
			//CMS.NVR.MapDoc = Xml.LoadXml(e.Value);
		}

		public void ChangeMapById(Object sender, EventArgs<String> e)
		{
			_propertyLoadLock = true;
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");

			panelTitle.Visible = false;
			HideAllPanels();

			if(target =="-1" || target =="Root" || String.IsNullOrEmpty(target))
			{
				textBoxName.Text = String.Empty;
				textBoxImage.Text = String.Empty;
				checkBoxIsDefault.Checked = false;
				_currentId = null;
			    _currentMapId = target;
				buttonEditImage.Visible = false;
				panelMapTitle.Visible = panelMap.Visible = false;
				return;
			}

			panelMapTitle.Visible = panelMap.Visible = Visible;

			if (String.IsNullOrEmpty(target) != true)
			{
				var map = CMS.NVRManager.FindMapById(target);
				if(map != null)
				{
					LoadMapInfo(map.Id);
				}

			}
			_propertyLoadLock = false;
		}

		private void LoadMapInfo(String mapId)
		{
			if(String.IsNullOrEmpty(mapId) || !Visible)
			{
				return;
			}
			_propertyLoadLock = true;

			var map = CMS.NVRManager.FindMapById(mapId);

			if(map != null)
			{
				textBoxName.Text = map.Name;
				textBoxImage.Text = map.OriginalFile;
				checkBoxIsDefault.Checked = map.IsDefault;
				_currentId = map.Id;
				_currentMapId = map.Id;
				LoadMapsList(comboBoxMaps);

				var dataSource = comboBoxMaps.DataSource as DataTable;
				if (dataSource != null)
				{
					buttonAddHotZone.Enabled =  buttonAddTransfer.Enabled = comboBoxMaps.Enabled = dataSource.Rows.Count != 0;
				}

				buttonEditImage.Visible = true;
				RemoveActivateCameras();
				_status = "Map";
				if (panelMap.Visible != true)
				{
					panelTitle.Visible = false;
					HideAllPanels();
					panelMapTitle.Visible = panelMap.Visible = true;
				}
			}

			_propertyLoadLock = false;
		}

		private void ComboBoxMapsSelectedValueChanged(object sender, EventArgs e)
		{
			SharedToolTips.SharedToolTip.SetToolTip(comboBoxMaps, comboBoxMaps.Text);
		}

		public void ReadCameraData(Object sender, EventArgs<String, String> e)
		{
			_currentMapId = e.Value2;

			String target = e.Value1;

			var cam = _mapHandler.FindCameraById(target);

			if(cam != null)
			{
				var camSysId = cam.SystemId;
				var camNVRId = cam.NVRSystemId;

				var nvr = CMS.NVRManager.FindNVRById(Convert.ToUInt16(camNVRId));
				if (nvr == null) return;
				if (nvr.ReadyState == ReadyState.NotInUse) return;
				//if (nvr.ReadyState != ReadyState.Ready) return;

				var camName = nvr.Device.FindDeviceById(Convert.ToUInt16(camSysId));
				if (camName == null) return;

				_propertyLoadLock = true;
				
				if (panelCamera.Visible != true)
				{
					PanelTitleBarUI2.Text = Localization["EMap_SelectedCamera"];
					HideAllPanels();
					panelCamera.Visible = true;
					panelCamera.BringToFront();
				}
				
				if(!panelTitle.Visible)
				{
					panelTitle.Visible = true;
				}
					

				_currentId = cam.Id;
				
				labelCameraName.Text = String.Format("{0} ({1})", camName.Name, nvr.Name);   //cam.Attributes["Name"].Value;
				CameraRotate.Value = Convert.ToInt32(cam.Rotate);
				//checkBoxDefaultShowWindow.Checked = cam.IsDefaultOpenVideoWindow;
				if(cam.VideoWindowSize == 2)
				{
					VideoWindowSizeChange(buttonWindowSizeMedium,false);
				}
				else if (cam.VideoWindowSize == 3)
				{
					VideoWindowSizeChange(buttonWindowSizeLarge,false);
				}
				else
				{
					VideoWindowSizeChange(buttonWindowSizeSmall,false);
				}
				_status = "Camera";
				_propertyLoadLock = false;
			}

		}

		public void ReadNVRData(Object sender, EventArgs<String, String> e)
		{
			_currentMapId = e.Value2;

			_propertyLoadLock = true;

			String target = e.Value1;

			var nvr = _mapHandler.FindNVRById(target);

			if(nvr != null)
			{
				var nvrServerId = nvr.SystemId;
				var nvrServer = CMS.NVRManager.FindNVRById(Convert.ToUInt16(nvrServerId));
				if (nvrServer == null) return;
				if (nvrServer.ReadyState != ReadyState.Ready) return;

				var narName = nvrServer.Name;

				if(panelNVR.Visible != true)
				{
					PanelTitleBarUI2.Text = Localization["EMap_SelectedNVR"];
					HideAllPanels();
					panelNVR.Visible = true;
					panelNVR.BringToFront();
					//panelTitle.Visible = true;
				}

				if (!panelTitle.Visible)
				{
					panelTitle.Visible = true;
				}

				_currentId = nvr.Id;
				
				labelNVRName.Text = narName;
				LoadMapsList(comboBoxNVRMaps);

                if (String.IsNullOrEmpty(nvr.LinkToMap))
                {
                    foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                    {
                        if (_currentMapId != map.Key)
                        {
                            nvr.LinkToMap = map.Key;
                            break;
                        }
                    }
                }

				ChangeMapsListSelected(comboBoxNVRMaps, nvr.LinkToMap);

				RemoveActivateCameras();
				_status = "NVR";
			}
			_propertyLoadLock = false;
		}

		private void ComboBoxNVRMapsSelectedValueChanged(object sender, EventArgs e)
		{
			if (_propertyLoadLock)
			{
				return; 
			}

			comboBoxNVRMaps.BackColor = Color.White;

			var nvr = _mapHandler.FindNVRById(_currentId);
			if(nvr != null)
			{
				nvr.LinkToMap = comboBoxNVRMaps.SelectedValue.ToString();
			}

			SharedToolTips.SharedToolTip.SetToolTip(comboBoxNVRMaps, comboBoxNVRMaps.Text);

			OnSyncData();
		}

		private void ButtonRemoveNVRClick(object sender, EventArgs e)
		{
			DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemoveNVRConfirm"].Replace("%1", labelNVRName.Text), Localization["MessageBox_Confirm"],
												  MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (result != DialogResult.OK) return;
			if (String.IsNullOrEmpty(_currentId)) return;

			foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
				{
					if (nvr.Key == _currentId)
					{
						_mapHandler.RemoveNVRById(_currentId);
						if (OnModifyCameraFromProperties != null)
						{
							OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Remove</Mode></xml>", _currentId)));
						}
						_currentId = null;
						OnSyncData();
						LoadMapInfo(map.Key);
						panelTitle.Visible = false;
						HideAllPanels();
						panelMapTitle.Visible = panelMap.Visible = true;
						return;
					}
				}
			}
		}

		public void ReadHotZoneData(Object sender, EventArgs<String, String> e)
		{
			_currentMapId = e.Value2;

			_propertyLoadLock = true;
			
			String target = e.Value1;

			var hotzone = _mapHandler.FindHotZoneById(target);

			if (hotzone != null)
			{

				if (panelHotZone.Visible != true)
				{
					PanelTitleBarUI2.Text = Localization["EMap_SelectedHotZone"];
					HideAllPanels();
					panelHotZone.Visible = true;
					panelHotZone.BringToFront();
					//panelTitle.Visible = true;
				}

				if (!panelTitle.Visible)
				{
					panelTitle.Visible = true;
				}

				_currentId = hotzone.Id;

				trackBarOpacity.Value = (int)hotzone.Opacity == 0 ? 8 : (int)hotzone.Opacity;
				if ((int)hotzone.Opacity == 0)
				{
					hotzone.Opacity = 8;
				}
				LoadMapsList(comboBoxHotZoneMaps);

				ChangeMapsListSelected(comboBoxHotZoneMaps, hotzone.LinkToMap);

				textBoxHotZoneName.Text = hotzone.Name;

				_status = "HotZone";
				var pp = new List<Point>();

				foreach (var point in hotzone.Points)
				{
					pp.Add(new Point(point.X/10,point.Y/10));
				}
				_color = new SolidBrush(hotzone.Color);
				_point = pp.ToArray();
			   panelColorTable.Invalidate();
			}
			_propertyLoadLock = false;
		}

		private Point[] _point;
		private Brush _color = Brushes.Black;
		private void PanelColorTablePaint(Object sender, PaintEventArgs e)
		{
			if (_point == null || _point.Length == 0) return;
			var g = e.Graphics;
			g.FillPolygon(_color, _point);
		}

		private void TextBoxHotZoneNameTextChanged(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId) && panelHotZone.Visible == false)
			{
				return;
			}

			var zone = _mapHandler.FindHotZoneById(_currentId);
			if (zone != null)
			{
				if (textBoxHotZoneName.Text == zone.Name)
				{
					return;
				}

				zone.Name = textBoxHotZoneName.Text;

			}

			OnSyncData();

			if (OnModifyCameraFromProperties != null)
			{
				OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>HotZoneDescription</Mode></xml>", _currentId)));
			}
		}

		private void HotZoneOpacityValueChanged(object sender, EventArgs e)
		{
			if (_propertyLoadLock)
			{
				return;
			}

			var hotzone = _mapHandler.FindHotZoneById(_currentId);
			if (hotzone != null)
			{
				hotzone.Opacity = trackBarOpacity.Value;
				OnSyncData();
			}

			if (OnModifyCameraFromProperties != null)
			{
				OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Opacity</Mode><Opacity>{1}</Opacity></xml>", _currentId, trackBarOpacity.Value)));
			}
		}

		private void ButtonSelectColorClick(object sender, EventArgs e)
		{
			var result = colorDialog.ShowDialog();
			if(result == DialogResult.OK)
			{
				var hotzone = _mapHandler.FindHotZoneById(_currentId);

				if (hotzone != null)
				{
					var color = colorDialog.Color;
					Console.WriteLine(color.ToString());
					_color = new SolidBrush(color);
					OnSyncData();
					hotzone.Color = color;
					var pp = new List<Point>();
					foreach (var point in hotzone.Points)
					{
						pp.Add(new Point(point.X / 10, point.Y / 10));
					}
					_point = pp.ToArray();
					panelColorTable.Invalidate();

					if (OnModifyCameraFromProperties != null)
					{
						OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>HotZoneColor</Mode></xml>", _currentId)));
					}
				}
			}
		}

		private void ComboBoxHotZoneMapsSelectedIndexChanged(object sender, EventArgs e)
		{
			if (_propertyLoadLock)
			{
				return;
			}

			var hotzone = _mapHandler.FindHotZoneById(_currentId);
			if (hotzone != null)
			{
				hotzone.LinkToMap = comboBoxHotZoneMaps.SelectedValue.ToString();
			}

			SharedToolTips.SharedToolTip.SetToolTip(comboBoxHotZoneMaps, comboBoxHotZoneMaps.Text);

			OnSyncData();
		}

		private void ButtonRemoveHotZoneClick(object sender, EventArgs e)
		{
			DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemoveTransferZoneConfirm"], Localization["MessageBox_Confirm"],
												  MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (result != DialogResult.OK) return;
			if (String.IsNullOrEmpty(_currentId)) return;

			foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, MapHotZoneAttributes> hotZone in map.Value.HotZones)
				{
					if (hotZone.Key == _currentId)
					{
						_mapHandler.RemoveHotZoneById(_currentId);
						if (OnModifyCameraFromProperties != null)
						{
							OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Remove</Mode></xml>", _currentId)));
						}
						_currentId = null;
						OnSyncData();
						LoadMapInfo(map.Key);
						panelTitle.Visible = false;
						HideAllPanels();
						panelMapTitle.Visible = panelMap.Visible = true;
						return;
					}
				}
			}
		}

		public void ReadViaData(Object sender, EventArgs<String, String> e)
		{
			_currentMapId = e.Value2;

			_propertyLoadLock = true;

			String target = e.Value1;

			var via = _mapHandler.FindViaById(target);
			if(via != null)
			{
				if(panelVia.Visible != true)
				{
					PanelTitleBarUI2.Text = Localization["EMap_SelectedMapLink"];
					HideAllPanels();
					panelVia.Visible = true;
					panelVia.BringToFront();
					//panelTitle.Visible = true;
				}

				if (!panelTitle.Visible)
				{
					panelTitle.Visible = true;
				}

				_currentId = via.Id;
				
				textBoxViaName.Text = via.Name;
				LoadMapsList(comboBoxMapListVia);
				ChangeMapsListSelected(comboBoxMapListVia, via.LinkToMap);
				_status = "Via";
				RemoveActivateCameras();
			}
			_propertyLoadLock = false;
		}

		private void ComboBoxMapListViaSelectedValueChanged(object sender, EventArgs e)
		{
			if( _propertyLoadLock)
			{
				return;
			}
			
			comboBoxNVRMaps.BackColor = Color.White;

			var via = _mapHandler.FindViaById(_currentId);
			if(via != null)
			{
				via.LinkToMap = comboBoxMapListVia.SelectedValue.ToString();
			}

			SharedToolTips.SharedToolTip.SetToolTip(comboBoxMapListVia, comboBoxMapListVia.Text);

			OnSyncData();
		}

		private void TextBoxViaDescriptionLostFocus(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId) && panelVia.Visible == false)
			{
				return;
			}

			var via = _mapHandler.FindViaById(_currentId);
			if(via != null)
			{
				if (textBoxViaName.Text == via.Name)
				{
					return;
				}

				via.Name = textBoxViaName.Text;

			}

			OnSyncData();

			if (OnModifyCameraFromProperties != null)
			{
				OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>ViaDescription</Mode></xml>", _currentId)));
			}
		}

        //private void TextBoxViaNameTextChanged(object sender, EventArgs e)
        //{
        //    if (String.IsNullOrEmpty(_currentId) && panelVia.Visible == false)
        //    {
        //        return;
        //    }

        //    var via = _mapHandler.FindViaById(_currentId);
        //    if (via != null)
        //    {
        //        if (textBoxViaName.Text == via.Name)
        //        {
        //            return;
        //        }

        //        via.Name = textBoxViaName.Text;

        //    }

        //    OnSyncData();

        //    if (OnModifyCameraFromProperties != null)
        //    {
        //        OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>ViaDescription</Mode></xml>", _currentId)));
        //    }
        //}

		private void ButtonRomoveViaClick(object sender, EventArgs e)
		{
			DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemoveViaConfirm"].Replace("%1", textBoxViaName.Text), Localization["MessageBox_Confirm"],
												  MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (result != DialogResult.OK) return;

			if (String.IsNullOrEmpty(_currentId)) return;

			foreach (KeyValuePair<String,MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, ViaAttributes> via in map.Value.Vias)
				{
					if (via.Key == _currentId)
					{
						_mapHandler.RemoveViaById(_currentId);
						OnSyncData();

						if (OnModifyCameraFromProperties != null)
						{
							OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Remove</Mode></xml>", _currentId)));
						}
						_currentId = null;

						LoadMapInfo(map.Key);
						panelTitle.Visible = false;
						HideAllPanels();
						panelMapTitle.Visible = panelMap.Visible = true;
						return;
					}
				}
			}

		}

		private void CameraRotateValueChanged(object sender, EventArgs e)
		{
			if( _propertyLoadLock)
			{
				return;
			}

			var cam = _mapHandler.FindCameraById(_currentId);
			if(cam != null)
			{
				cam.Rotate = CameraRotate.Value;
				OnSyncData();
			}

			//_mapHandler.UpdateMapData(CMS.NVR.MapDoc, "Camera", _currentId, "Rotate", CameraRotate.Value.ToString());

			if (OnModifyCameraFromProperties != null)
			{
				OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Rotate</Mode><Rotate>{1}</Rotate></xml>", _currentId, CameraRotate.Value)));
			}
		}

		private String _cameraVideoWindowSize;

		private void VideoWindowSizeChange(Button target,Boolean isUpdate)
		{
			if (_cameraVideoWindowSize == "Small")
			{
				buttonWindowSizeSmall.BackgroundImage = _smallWindowIcon;
			}
			else if (_cameraVideoWindowSize == "Medium")
			{
				buttonWindowSizeMedium.BackgroundImage = _mediumWindowIcon;
			}
			else if (_cameraVideoWindowSize == "Large")
			{
				buttonWindowSizeLarge.BackgroundImage = _largeWindowIcon;
			}

			var size = 1;

			if (target.Name == "buttonWindowSizeSmall")
			{
				if (target.BackgroundImage == _smallWindowIconActivate) return;
				target.BackgroundImage = _smallWindowIconActivate;
				_cameraVideoWindowSize = "Small";
				size = 1;
			}
			else if (target.Name == "buttonWindowSizeMedium")
			{
				if (target.BackgroundImage == _mediumWindowIconActivate) return;
				target.BackgroundImage = _mediumWindowIconActivate;
				_cameraVideoWindowSize = "Medium";
				size = 2;
			}
			else if (target.Name == "buttonWindowSizeLarge")
			{
				if (target.BackgroundImage == _largeWindowIconActivate) return;
				target.BackgroundImage = _largeWindowIconActivate;
				_cameraVideoWindowSize = "Large";
				size = 3;
			}

			if(isUpdate)
			{
				var cam = _mapHandler.FindCameraById(_currentId);
				if (cam != null)
				{
					if (cam.VideoWindowSize != size)
					{
						cam.VideoWindowSize = size;
						OnSyncData();

						if (OnModifyCameraFromProperties != null)
						{
							OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>VideoWindowResize</Mode><Size>{1}</Size></xml>", _currentId, size)));
						}

					}
				}
			}
		}

		private void VideoWindowSizeChangeClick(object sender, EventArgs e)
		{
			var target = sender as Button;
			if (target != null)
			{
				VideoWindowSizeChange(target,true);
			}
		}

		private void HideAllPanels()
		{
			panelMapTitle.Visible =
			panelMap.Visible =
			panelTitle.Visible =
			panelCamera.Visible = 
			panelVia.Visible = 
			panelNVR.Visible =
			panelHotZone.Visible = false;
		}

		private void ButtonRomoveCameraClick(object sender, EventArgs e)
		{
			DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemoveCameraConfirm"].Replace("%1", labelCameraName.Text), Localization["MessageBox_Confirm"],
												  MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

			if (result != DialogResult.OK) return;
			if (String.IsNullOrEmpty(_currentId)) return;

			foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
				{
					if (camera.Key == _currentId)
					{
						_mapHandler.RemoveCameraById(_currentId);
						OnSyncData();
						if (OnModifyCameraFromProperties != null)
						{
							OnModifyCameraFromProperties(this, new EventArgs<String>(String.Format("<xml><Id>{0}</Id><Mode>Remove</Mode></xml>", _currentId)));
						}

						_currentId = null;

						LoadMapInfo(map.Key);
						panelTitle.Visible = false;
						HideAllPanels();
						panelMapTitle.Visible = panelMap.Visible = true;
						return;
					}
				}

			}
		}

		private void ButtonAddTransferClick(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId))
			{
				return;
			}

			if(comboBoxMaps.SelectedValue == null)
			{
				return;
			}

			var selectMap = CMS.NVRManager.FindMapById(comboBoxMaps.SelectedValue.ToString());
			if(selectMap != null)
			{
				var mapName = selectMap.Name;

				var newVia = new ViaAttributes
				{
					Name = String.Format("{0} {1}", Localization["EMap_NewViaName"], mapName),
					Type = "Via",
					X = 100,
					Y = 100,
					DescX = 100,
					DescY = 130,
					LinkToMap = comboBoxMaps.SelectedValue.ToString()
				};
				
				if(OnAddVia != null)
				{
					OnAddVia(this,new EventArgs<ViaAttributes>(newVia));
				}

				//_mapHandler.CreateViaReturnId(_currentId, newVia);

				//OnSyncData();

				//ChangeMap(_currentId, "Refresh");
			}
		}

		private void ButtonAddHotZoneClick(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_currentId))
			{
				return;
			}

			if (comboBoxMaps.SelectedValue == null)
			{
				return;
			}

			var zone = new MapHotZoneAttributes
						   {
							   LinkToMap = comboBoxMaps.SelectedValue.ToString(),
							   Points = new List<Point>(),
							   Opacity = 8,
							   Color = Color.LightSeaGreen,
							   Name = Localization["EMap_HotZoneNewName"],
							   Type = "HotZone"
						   };

			if (OnDrawHotZone != null)
			{
				OnDrawHotZone(this, new EventArgs<MapHotZoneAttributes>(zone));
			}
		}

        protected static Int32 SortByIdThenNVR(MapAttribute x, MapAttribute y)
        {
            if (x.Id != y.Id)
                return (Convert.ToInt16(x.Id) - Convert.ToInt16(y.Id));

            return (Convert.ToInt16(String.IsNullOrEmpty(x.ParentId) ? "0" : x.ParentId) - Convert.ToInt16(String.IsNullOrEmpty(y.ParentId) ? "0" : y.ParentId));
        }

		private void LoadMapsList(ComboBox list)
		{
			//Map list
			//var maps = CMS.NVR.MapDoc.GetElementsByTagName("Map");

			_mapsTable = new DataTable();
			_mapsTable.Columns.Add("MapName", typeof(String));
			_mapsTable.Columns.Add("MapId", typeof(String));

            var sortResult = new List<MapAttribute>(CMS.NVRManager.Maps.Values);
            sortResult.Sort(SortByIdThenNVR);

            foreach (MapAttribute map in sortResult)
			{
                //if (CMS.NVRManager.Maps.ContainsKey(map.ParentId)) continue;
                if (!String.IsNullOrEmpty(map.ParentId)) continue;
				if (_currentMapId != map.Id || list.Name == "comboBoxNVRMaps")
				{
					_mapsTable.Rows.Add(map.Name, map.Id);
				}
                AddMapToDataTableByParentId(map.Id);
			}

			list.DataSource = _mapsTable;
			list.DisplayMember = "MapName";
			list.ValueMember = "MapId";

			//mapList.DataBindings;
		}

        private void AddMapToDataTableByParentId(String parentId)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                if (map.Value.ParentId != parentId) continue;
                if (_currentMapId != map.Key)
                {
                    _mapsTable.Rows.Add(map.Value.Name, map.Key);
                }
                AddMapToDataTableByParentId(map.Key);
            }
        }

		private static void ChangeMapsListSelected(ComboBox list, String mapId)
		{
			var dataSource = list.DataSource as DataTable;

			if (dataSource != null)
				for (int i = 0; i < dataSource.Rows.Count; i++)
				{
					//開始檢查給定的值
					if ((String)dataSource.Rows[i][list.ValueMember] == mapId)
					{
						list.SelectedIndex = i;
						SharedToolTips.SharedToolTip.SetToolTip(list,list.Text);
						return;
					}
				}
			return;
		}

		private static void ChangeListNameById(ComboBox list, String mapId, String value)
		{
			var dataSource = list.DataSource as DataTable;

			if (dataSource != null)
				for (int i = 0; i < dataSource.Rows.Count; i++)
				{
					//開始檢查給定的值
					if ((String)dataSource.Rows[i][list.ValueMember] == mapId)
					{
						dataSource.Rows[i][list.DisplayMember] = value;
						SharedToolTips.SharedToolTip.SetToolTip(list,list.Text);
						break;
					}
				}
		}

		public void ChangeMapNameById(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");

			var map = CMS.NVRManager.FindMapById(target);
			if(map != null)
			{
				ChangeListNameById(comboBoxMaps, target, map.Name);
				ChangeListNameById(comboBoxMapListVia, target, map.Name);
			}

		}

		public void ChangeMapCount(Object sender, EventArgs<String> e)
		{
			_propertyLoadLock = true;
			LoadMapsList(comboBoxMaps);
			LoadMapsList(comboBoxMapListVia);
			_propertyLoadLock = false;
		}

		private void RemoveActivateCameras()
		{
			if(OnRemoveAllActivateCameras!=null)
			{
				OnRemoveAllActivateCameras(this, new EventArgs<String>(String.Format("<xml><Mode>Remove</Mode></xml>")));
			}
		}

	}
}
