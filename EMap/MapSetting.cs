using Constant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
//using ApplicationForms = App.ApplicationForms;
using Application = System.Windows.Forms.Application;
//using ApplicationApp = App;
using ApplicationForms = App.ApplicationForms;
using Color = System.Drawing.Color;

namespace EMap
{
	public partial class MapSetting : UserControl, IControl, IAppUse, IServerUse, IMinimize, IMouseHandler, IBlockPanelUse
	{
		public String TitleName { get; set; }

		public IApp App
		{
			get; set;
		}

		public ushort MinimizeHeight
		{
			get { return (UInt16)panelTitle.Size.Height; }
		}

		public Boolean IsMinimize { get; private set; }

		public Button Icon { get; private set; }

		public void GlobalMouseHandler()
		{
			//if (Drag.IsDrop(eventListPanel))
			//{
			//    if (!eventListPanel.AutoScroll)
			//    {
			//        eventListPanel.AutoScroll = true;
			//    }

			//    return;
			//}

			//if (eventListPanel.AutoScroll)
			//    HideScrollBar();
		}

		public IBlockPanel BlockPanel { get; set; }

		protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
		protected readonly PanelTitleBarUI2 PanelPropertyTitleBarUI2 = new PanelTitleBarUI2();
		//minimize event
		public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<Boolean>> OnCheckUserPermission;
		public event EventHandler<EventArgs<String>> OnChangeMap;
		public event EventHandler<EventArgs<String>> OnChangeMapCountFromMaps;
		public event EventHandler<EventArgs<Boolean>> OnChangeSetupMode;
		public event EventHandler<EventArgs<Boolean>> OnBoxHeightChange;
		public PropertiesSetting PropertiesSetting;

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
		//create new PanelTitleBar
		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();

		private TreeView _mapTreeView;
		private MapHandler _mapHandler;
		private Boolean _isFirstLoad;
		private ImageList _imageList;
		private List<String> _editMaps;
		private List<String> _newMaps;
		private List<String> _deleteMaps;
		private List<String> _alarmMaps;
		private Dictionary<String,Int32> _tempMapsImages;
		private String _currentMapId;
		public Dictionary<String, String> Localization;
		private Boolean _mapLoadLock;
		private String _mode;
		private static readonly Image _treeIconSpace  = Resources.GetResources(Properties.Resources.space, Properties.Resources.IMGSpace);
		private static readonly Image _treeIconNew  = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
		private static readonly Image _treeIconModify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
		private static readonly Image _treeIconAlarm= Resources.GetResources(Properties.Resources.treeAlarm, Properties.Resources.IMGTreeAlarm);
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.map, Properties.Resources.IMGMapIcon);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.map_activate, Properties.Resources.IMGMapIconActivate);
		//minimize setting

		//settings events
		public event EventHandler<EventArgs<String>> OnChangeMapFromProperties;
		public event EventHandler<EventArgs<String>> OnChangeMapNameFromProperties;
		public event EventHandler<EventArgs<String>> OnModifyCameraFromProperties;
		public event EventHandler<EventArgs<String>> OnModifyAnyAboutMap;
		public event EventHandler<EventArgs<String>> OnRemoveAllActivateCameras;
		public event EventHandler<EventArgs<MapHotZoneAttributes>> OnDrawHotZone;
		public event EventHandler<EventArgs<ViaAttributes>> OnAddVia;

		public MapSetting()
		{
			Localization = new Dictionary<String, String>
							   {
									{"Control_Maps", "Map"},

									{"EMap_TreeMap", "Map"},
									{"EMap_AllMaps", "All maps"},
									{"EMap_ButtonAddMap", "Add Map"},
									{"EMap_ButtonDeleteMap", "Delete Map"},
									{"EMap_CheckBoxSetupMode", "Setup the maps"},
									{"MessageBox_Confirm", "Confirm"},
									{"MessageBox_Information", "Information"},
									{"EMap_MessageBoxRemoveMapConfirm", "Do you want to delete the  map  \"%1\" ?"},
									{"EMap_MessageBoxUploadMapWrongGetExtension", "Upload failed. Wrong file format."},
									{"MessageBox_Error", "Error"},
									{"EMap_TryAgain", "Please try again."},
									{"EMap_NewMapName", "New Map"},
									{"Application_SaveCompleted", "Save completed"},
									{"EMap_SaveCompletedRemind", "Do you want to leave setup mode?"},
									{"EMap_MessageBoxRemindSaveData", "Do you want to save your setting?"},
									{"EMap_MessageBoxUploadMapFail", "Upload failed. Please try again."},
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
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			PanelTitleBarUI2.Text = TitleName = Localization["Control_Maps"];
			panelTitle.Controls.Add(PanelTitleBarUI2);
            //PanelTitleBarUI2.InitializeToolStripMenuItem();

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

			_mapHandler = new MapHandler { CMS = CMS };
			_newMaps = new List<String>();
			_editMaps = new List<String>();
			_deleteMaps = new List<String>();
			_alarmMaps = new List<String>();
			_tempMapsImages = new Dictionary<String, Int32>();

			buttonAddMap.Text = Localization["EMap_ButtonAddMap"];
			buttonDelete.Text = Localization["EMap_ButtonDeleteMap"];
			//language end

			_imageList = new ImageList();
			_imageList.Images.Add(_treeIconSpace);    // imageIndex = 0 , -1
			_imageList.Images.Add(_treeIconModify); // imageIndex = 1
			_imageList.Images.Add(_treeIconNew);      // imageIndex = 2
			_imageList.Images.Add(_treeIconAlarm);   // imageIndex = 3

			_imageList.ImageSize = new System.Drawing.Size(18, 18);
			_imageList.ColorDepth = ColorDepth.Depth32Bit;
			_imageList.TransparentColor = Color.Transparent;

			_mapTreeView = new TreeView
			{
				Dock = DockStyle.Fill,
				BackColor = Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68))))),
				ForeColor = Color.White,
				ImageList = _imageList,
				ImageIndex = 0,
				BorderStyle = BorderStyle.None,
				Cursor = Cursors.Hand,
				LabelEdit = false,
				Indent = 10,
				LineColor = Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68))))),
			};
			panelTree.Controls.Add(_mapTreeView);

			_isFirstLoad = true;
			_mapLoadLock = true;
			CreateMapTree();
			_mapLoadLock = false;
			_mode = "View";
			panelFunction.Visible = false;

			PropertiesSetting = new PropertiesSetting { panelSettings = { Dock = DockStyle.Fill }, Server = Server, MapHandler = _mapHandler };
			PropertiesSetting.Initialize();
			panelSetting.Controls.Add(PropertiesSetting.panelSettings);
			PropertiesSetting.panelSettings.Visible = true;
			PropertiesSetting.OnChangeMapFromProperties += PropertiesSettingOnChangeMapFromProperties;
			PropertiesSetting.OnChangeMapNameFromProperties += PropertiesSettingOnChangeMapNameFromProperties;
			PropertiesSetting.OnModifyCameraFromProperties += PropertiesSettingOnModifyCameraFromProperties;
			PropertiesSetting.OnModifyAnyAboutMap += PropertiesSettingOnModifyAnyAboutMap;
			PropertiesSetting.OnRemoveAllActivateCameras += PropertiesSettingOnRemoveAllActivateCameras;
			PropertiesSetting.OnDrawHotZone += PropertiesSettingOnDrawHotZone;
			PropertiesSetting.OnAddVia += PropertiesSettingOnAddVia;
		}

		//properties setting panel
		private void PropertiesSettingOnAddVia(object sender, EventArgs<ViaAttributes> e)
		{
			if (OnAddVia != null)
				OnAddVia(this, e);
		}

		private void PropertiesSettingOnDrawHotZone(object sender, EventArgs<MapHotZoneAttributes> e)
		{
			if (OnDrawHotZone != null)
				OnDrawHotZone(this, e);
		}

		private void PropertiesSettingOnRemoveAllActivateCameras(object sender, EventArgs<string> e)
		{
			if (OnRemoveAllActivateCameras != null)
				OnRemoveAllActivateCameras(this, e);
		}

		private void PropertiesSettingOnModifyAnyAboutMap(object sender, EventArgs<string> e)
		{
			if (OnModifyAnyAboutMap != null)
				OnModifyAnyAboutMap(this, e);
		}

		private void PropertiesSettingOnModifyCameraFromProperties(object sender, EventArgs<string> e)
		{
			if (OnModifyCameraFromProperties != null)
				OnModifyCameraFromProperties(this, e);
		}

		private void PropertiesSettingOnChangeMapNameFromProperties(object sender, EventArgs<string> e)
		{
			if (OnChangeMapNameFromProperties != null)
			{
				OnChangeMapNameFromProperties(this, e);
				ChangeMapNameById(this, e);
			}
		}

		private void PropertiesSettingOnChangeMapFromProperties(object sender, EventArgs<string> e)
		{
			if(OnChangeMapFromProperties != null)
				OnChangeMapFromProperties(this,e);
		}

		public void ReadCameraData(Object sender, EventArgs<String, String> e)
		{
			PropertiesSetting.ReadCameraData(sender, e);
		}

		public void ReadViaData(Object sender, EventArgs<String, String> e)
		{
			PropertiesSetting.ReadViaData(sender, e);
		}

		public void ReadNVRData(Object sender, EventArgs<String, String> e)
		{
			PropertiesSetting.ReadNVRData(sender, e);
		}

		public void ReadHotZoneData(Object sender, EventArgs<String, String> e)
		{
			PropertiesSetting.ReadHotZoneData(sender, e);
		}
		//properties setting panel

		public void ReloadMap(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "mode");

			if (target == "refresh")
			{
				_editMaps.Clear();
				_newMaps.Clear();
				_deleteMaps.Clear();
				_mapLoadLock = true;
				CreateMapTree();
				_mapLoadLock = false;
			}

			PropertiesSetting.ReloadMap(sender, e);
		}


		public void Activate()
		{
			if (_isFirstLoad)
			{
				_mapLoadLock = true;
				var node = _mapTreeView.SelectedNode;
				_mapTreeView.SelectedNode = node;

				_mapTreeView.AfterSelect -= MapTreeViewMouseDoubleClick;
				_mapTreeView.NodeMouseClick -= MapTreeViewNodeMouseClick;

				_mapTreeView.AfterSelect += MapTreeViewMouseDoubleClick;
				_mapTreeView.NodeMouseClick += MapTreeViewNodeMouseClick;

				//_mapTreeView.ItemDrag += MapTreeViewItemDrag;
				//_mapTreeView.DragOver += MapTreeViewDragOver;
				//_mapTreeView.DragEnter += MapTreeViewDragEnter;
				//_mapTreeView.DragDrop += MapTreeViewDragDrop;
				_mapLoadLock = false;

                if (OnCheckUserPermission != null)
                    OnCheckUserPermission(this, new EventArgs<Boolean>(Server.User.Current.Group.IsFullAccessToDevices));
			}
			else
			{
				CreateMapTree();
				if (_alarmMaps.Count > 0) 
					LogEventAlarm(this, new EventArgs<String>(""));
			}
		}

		public void Deactivate()
		{
		}

		public void MoveToMapByCamera(Object sender, EventArgs<IDevice> e)
		{
			if(e.Value == null) return;

			var device = e.Value;

			var maps = _mapHandler.FindCameraByDeviceIdNVRSystemIdReturnMap(device.Id, device.Server.Id);
			if (maps != null)
			{
                MapAttribute map = null;
                if (maps.ContainsKey(_currentMapId))
                {
                    map = maps[_currentMapId];
                }
                else
                {
                    foreach (KeyValuePair<String, MapAttribute> mapAttribute in maps)
                    {
                        map = mapAttribute.Value;
                        break;
                    }
                }

			    if (map != null)
			    {
			        var mapId = map.Id;
			        ChangeTreeViewSelected(mapId);
			    }
			}
		}

		public void MoveToMapByNVR(Object sender, EventArgs<INVR> e)
		{
			if (e.Value == null) return;
			if (e.Value is ICMS) return;

			var nvrServer = e.Value;

			//var nvr = _mapHandler.FindFirstNodeByAttribute(CMS.NVR.MapDoc, "/Maps/Map/NVRs/NVR", "SystemId", nvrServer.Id.ToString());

			foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<string, NVRAttributes> nvr in map.Value.NVRs)
				{
					if (nvr.Key == nvrServer.Id.ToString())
					{
						ChangeTreeViewSelected(map.Key);
					}
				}
			}
	   
		}

		public void SetupMap(Object sender, EventArgs e)
		{
			_mode = _mode == "View" ? "Setup" : "View";
			_mapTreeView.AllowDrop = _mode == "Setup";
		   // _mapTreeView.Cursor = _mode == "Setup" ? Cursors.NoMoveVert : Cursors.Hand;
			if(_mode == "View")
			{
				if(_editMaps.Count>0 || _newMaps.Count>0 || _deleteMaps.Count>0)
				{
					DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemindSaveData"], Localization["MessageBox_Information"],
											 MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

					if (result == DialogResult.OK)
					{
					   SaveMapData(this, new EventArgs());
					}
	   
				}

				LogEventAlarm(this,new EventArgs<string>(""));
			}
			else
			{
				foreach (KeyValuePair<String, TreeNode> n in nodeArr)
				{
					var node = n.Value;
					if (_tempMapsImages.ContainsKey(node.Name))
					{
						node.ImageIndex = _tempMapsImages[node.Name];
						node.SelectedImageIndex = _tempMapsImages[node.Name];
						_tempMapsImages.Remove(node.Name);
					}
				}
				_tempMapsImages.Clear();

				buttonDelete.Visible = _currentMapId != "Root";
			}

			if (OnBoxHeightChange != null && IsMinimize == false)
			{
				OnBoxHeightChange(this, new EventArgs<Boolean>(_mode == "Setup"));
			}

			panelFunction.Visible =  (_mode == "Setup");

			if (OnChangeSetupMode!=null )
			{
				var eventArgs = new EventArgs<Boolean>(_mode == "Setup");
				OnChangeSetupMode(this, eventArgs);
				ChangeMap(_currentMapId, "Refresh");
				PropertiesSetting.ChangeSetupMode(this, eventArgs);
			}
		}

		public void ChangeMapNameById(Object sender, EventArgs<String> e)
		{
			
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");

			var map = CMS.NVRManager.FindMapById(target);

			if(map!=null)
			{
				if (_mapTreeView != null)
				{
					var nNode = nodeArr[target];

					nNode.Text = map.Name;

					MakeEditMarkForMap(target);
				}
			}

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
			if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
			{
				BlockPanel.HideThisControlPanel(this);

				if (!App.StartupOption.Loading)
				{
					App.StartupOption.HidePanel = true;
					App.StartupOption.SaveSetting();
				}
			}

			Icon.Image = _icon;
			Icon.BackgroundImage = null;

			IsMinimize = true;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);

			//IsMinimize = true;
			//if (OnMinimizeChange != null)
			//    OnMinimizeChange(this, null);

			//IsMinimize = true;
			//if (OnMinimizeChange != null)
			//    OnMinimizeChange(this, null);

			//if(OnBoxHeightChange != null)
			//{
			//    OnBoxHeightChange(this,new EventArgs<Boolean>(false));
			//}
		}

		public void Maximize()
		{
			if (BlockPanel.LayoutManager.Page.Version == "2.0")
			{
				BlockPanel.ShowThisControlPanel(this);

				if (!App.StartupOption.Loading)
				{
					App.StartupOption.HidePanel = false;
					App.StartupOption.SaveSetting();
				}

				if (!String.IsNullOrEmpty(_currentMapId))
					ChangeMap(_currentMapId, "Refresh");
			}

			Icon.Image = _iconActivate;
			Icon.BackgroundImage = ControlIconButton.IconBgActivate;

			IsMinimize = false;

			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);

			//IsMinimize = false;
			//if (OnMinimizeChange != null)
			//    OnMinimizeChange(this, null);

			//if (OnBoxHeightChange != null)
			//{
			//    if(_mode == "Setup")
			//    {
			//        OnBoxHeightChange(this, new EventArgs<Boolean>(true));
			//    }
			//}
		}

		private Dictionary<String, TreeNode> nodeArr = new Dictionary<String, TreeNode>();
		private List<String> _noParentNode = new List<String>();
		private TreeNode cNode = new TreeNode();
		public void CreateMapTree()
		{
			if(!String.IsNullOrEmpty(_currentMapId))
			{
				if (nodeArr.ContainsKey(_currentMapId))
					cNode = nodeArr[_currentMapId];
				else
				{
					cNode = null;
				}
			}
			else
			{
				cNode = null;
			}

			_mapTreeView.Nodes.Clear();
			nodeArr.Clear();
			_noParentNode.Clear();
			if (cNode == null) 
				cNode = new TreeNode();
			var rootNode = new TreeNode(Localization["EMap_AllMaps"]) {Name = "Root"};
			_mapTreeView.Nodes.Add(rootNode);
			 cNode = rootNode;

		    var sortResult = new List<MapAttribute>(CMS.NVRManager.Maps.Values);
            sortResult.Sort(SortByIdThenNVR);
            foreach (MapAttribute map in sortResult)
		    {
                if (!String.IsNullOrEmpty(map.ParentId))
                {
                    if (nodeArr.ContainsKey(map.ParentId))
                    {
                        var parentNode = nodeArr[map.ParentId];
                        CreateMapTreeNode(map.Id, map, parentNode);
                    }
                    else
                    {
                        _noParentNode.Add(map.Id);
                    }
                }
                else
                {
                    CreateMapTreeNode(map.Id, map, rootNode);
                }
		    }

            //foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
            //{
            //    if(!String.IsNullOrEmpty(map.Value.ParentId))
            //    {
            //        if(nodeArr.ContainsKey(map.Value.ParentId))
            //        {
            //            var parentNode = nodeArr[map.Value.ParentId];
            //            CreateMapTreeNode(map.Key, map.Value, parentNode);
            //        }
            //        else
            //        {
            //            _noParentNode.Add(map.Key);
            //        }
            //    }
            //    else
            //    {
            //        CreateMapTreeNode(map.Key, map.Value, rootNode);
            //    }
            //}

			AppendNoParentNode();
			_mapTreeView.SelectedNode = cNode;
			_mapTreeView.ExpandAll();
		}

        protected static Int32 SortByIdThenNVR(MapAttribute x, MapAttribute y)
        {
            if (x.Id != y.Id)
                return (Convert.ToInt16(x.Id) - Convert.ToInt16(y.Id));

            return (Convert.ToInt16(String.IsNullOrEmpty(x.ParentId) ? "0" : x.ParentId) - Convert.ToInt16(String.IsNullOrEmpty(y.ParentId) ? "0" : y.ParentId));
        }

		private void AppendNoParentNode()
		{
			var removeMapId = new List<String>();
			foreach (String mapId in _noParentNode)
			{
				var map = CMS.NVRManager.FindMapById(mapId);
				if(map != null)
				{
					if(!nodeArr.ContainsKey(map.ParentId))
					{
						var parentNode = nodeArr[map.ParentId];
						CreateMapTreeNode(map.Id, map, parentNode);
					}
                    else
                    {
                        var parentNode = nodeArr[map.ParentId];
                        if (!nodeArr.ContainsKey(map.Id))
                            CreateMapTreeNode(map.Id, map, parentNode);
                        //continue;
                    }
				}
				removeMapId.Add(mapId);
			}

			foreach (string id in removeMapId)
			{
				_noParentNode.Remove(id);
			}

			if(_noParentNode.Count>0)
				AppendNoParentNode();
		}

		private void CreateMapTreeNode(String mapId, MapAttribute mapAttribute, TreeNode node)
		{
			var id = mapId;
			var stateNew = String.Empty;
			if (_newMaps.Count > 0)
			{
				stateNew = _newMaps.Find(a => a == id);
			}
			var stateEdit = String.Empty;
			if (_editMaps.Count > 0)
			{
				stateEdit = _editMaps.Find(a => a == id);
			}

			var newNode = new TreeNode(mapAttribute.Name)
			{
				Name = id
			};

			if (String.IsNullOrEmpty(stateNew) == false)
			{
				newNode.ImageIndex = 2;
				newNode.SelectedImageIndex = 2;
			}
			else
			{
				if (String.IsNullOrEmpty(stateEdit) == false)
				{
					newNode.ImageIndex = 1;
					newNode.SelectedImageIndex = 1;
				}
				else
				{
					newNode.ImageIndex = 0;
				}
			}


			nodeArr.Add(id, newNode);
			if (mapAttribute.IsDefault || String.IsNullOrEmpty(cNode.Name))
			{
				cNode = newNode;
			}

			if(_currentMapId == mapId)
				cNode = newNode;

			node.Nodes.Add(newNode);
		}

		void MapTreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			var target = e.Node.Name;

			if (_currentMapId == target)
				{
					ChangeMap(_currentMapId,"Refresh");
				}
		}

		private void MapTreeViewMouseDoubleClick(object sender, TreeViewEventArgs e)
		{
			if(_mapLoadLock)
			{
				//return;
			}
			var node = e.Node;
			if (node != null)
			{
				foreach (KeyValuePair<String, TreeNode> treeNode in nodeArr)
				{
					treeNode.Value.BackColor = Color.Empty;
				}

				foreach (TreeNode n in _mapTreeView.Nodes)
				{
					n.BackColor = Color.Empty;
				}

				//if (node.Name == @"Root")
				//{
				//    ChangeMap("-1", "Refresh");
				//    if(_mode == "Setup")
				//    {
				//        buttonDelete.Visible = false;
				//    }
				//}
				//else
				//{
				//    ChangeMap(node.Name, "Refresh");
				//    if (_mode == "Setup")
				//    {
				//        buttonDelete.Visible = true;
				//    }

				//    node.BackColor = Color.DodgerBlue;
				//}

				ChangeMap(node.Name, "Refresh");
				_currentMapId = node.Name;

				if (_mode == "Setup")
				{
					buttonDelete.Visible = node.Name != @"Root";
				}

				node.BackColor = Color.DodgerBlue;
			}

		}

		//private void MapTreeViewItemDrag(object sender, ItemDragEventArgs e)
		//{
		//    DoDragDrop(e.Item, DragDropEffects.Move);
		//}

		//private void MapTreeViewDragEnter(object sender, DragEventArgs e)
		//{
		//    e.Effect = DragDropEffects.Move;
		//}

		//private void MapTreeViewDragOver(object sender, DragEventArgs e)
		//{
		//    Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
		//    TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt);
		//    if (destinationNode == null) return;

		//    MapTreeViewDragNodeColorFix();

		//    destinationNode.BackColor = Color.DarkOrange;
		//}

		//private void MapTreeViewDragDrop(object sender, DragEventArgs e)
		//{
		//    TreeNode dragNode;

		//    if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
		//    {
		//        Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
		//        TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt);
		//        if (destinationNode == null) return;
		//        dragNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
		//        if (dragNode == destinationNode) return;
		//        if (destinationNode.TreeView == dragNode.TreeView)
		//        {
		//            //destinationNode.Nodes.Add((TreeNode)NewNode.Clone());
		//            if (pt.Y <= 9)
		//            {
		//                _mapTreeView.Nodes.Insert(0, (TreeNode)dragNode.Clone());
		//                _mapHandler.MoveMapIndex(dragNode.Name, 0);
		//            }
		//            else
		//            {
		//                var index = pt.Y >= destinationNode.Bounds.Top + (destinationNode.Bounds.Height / 4)
		//                                ? destinationNode.Index + 1
		//                                : destinationNode.Index;
		//                _mapTreeView.Nodes.Insert(index, (TreeNode)dragNode.Clone());
		//                _mapHandler.MoveMapIndex(dragNode.Name, (ushort)index);
		//            }

		//            destinationNode.Expand();
		//            //Remove Original Node
		//            dragNode.Remove();
		//            MapTreeViewDragNodeColorFix();

		//            if (OnChangeMapCountFromMaps != null)
		//            {
		//                OnChangeMapCountFromMaps(this, new EventArgs<String>(String.Format("<xml></xml>")));
		//            }
		//            _currentMapId = dragNode.Name;
		//            ChangeMap(_currentMapId, "Refresh");
		//            ChangeTreeViewSelected(_currentMapId);
		//        }
		//    }
		//}

		//private void MapTreeViewDragNodeColorFix()
		//{
		//    var nodes = _mapTreeView.Nodes;
		//    foreach (TreeNode n in nodes)
		//    {
		//        n.BackColor = Color.Empty;
		//        var ns = n.Nodes;
		//        foreach (TreeNode n1 in ns)
		//        {
		//            n1.BackColor = Color.Empty;
		//        }
		//    }
		//}

		private void ChangeMap(String mapId,String type)
		{
			var eventArgs = new EventArgs<String>(String.Format("<xml><MapId>{0}</MapId><Mode>{1}</Mode></xml>", mapId, type));
			if (OnChangeMap != null && _mapLoadLock  ==false && _isFirstLoad == false)
			{
				OnChangeMap(this, eventArgs);
				PropertiesSetting.ChangeMapById(this, eventArgs);
			}

			_isFirstLoad = false;
		}

		public void ChangeMapById(Object sender, EventArgs<String> e)
		{
			_mapLoadLock = true;
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");

			ChangeTreeViewSelected(target);

			PropertiesSetting.ChangeMapById(sender, e);
			_mapLoadLock = false;
		}

		public void ReceiveMapIsChanged(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			//<xml><MapId>1</MapId></xml>
			String target = Xml.GetFirstElementValueByTagName(xmlDoc, "MapId");
			MakeEditMarkForMap(target);
			//ReceiveMapIsChanged(sender, e);
		}

		public void MakeEditMarkForMap(String madId)
		{
			if (_mode == "View")
			{
				return;
			}

			var map = CMS.NVRManager.FindMapById(madId);

			if(map != null)
			{
				if (_mapTreeView != null)
				{
					var nNode = nodeArr[madId];
					if (nNode.SelectedImageIndex == -1 || nNode.SelectedImageIndex == 3)
					{
						nNode.SelectedImageIndex = 1;
						nNode.ImageIndex = 1;
					}

					if (_editMaps.FindAll(item => item == madId).Count == 0)
					{
						_editMaps.Add(madId);
					}

				}
			}

		}
		 
		public void SaveMapData(Object sender, EventArgs e)
		{
            ApplicationForms.ShowLoadingIcon(Server.Form);
            ApplicationForms.ShowProgressBar(Server.Form);
			Application.RaiseIdle(null);
			_mapHandler.SyncDeviceAndNVRName(CMS.NVRManager);
			CMS.NVRManager.SaveMap(_mapHandler.MakeMapSaveXmlDocument());
			_editMaps.Clear();
			_newMaps.Clear();
			_deleteMaps.Clear();

			if (_mapTreeView.SelectedNode != null)
			{
				var mapId = _mapTreeView.SelectedNode.Name;
				CreateMapTree();
				ChangeTreeViewSelected(mapId);
			}
	   
			if (_mode == "Setup")
			{
				DialogResult msgResult = TopMostMessageBox.Show(String.Format("{0}\n{1}", Localization["Application_SaveCompleted"], Localization["EMap_SaveCompletedRemind"]), Localization["MessageBox_Information"],
										   MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if (msgResult == DialogResult.Yes)
				{
					SetupMap(this, null);
				}
			}
			else
			{
				TopMostMessageBox.Show(String.Format("{0}", Localization["Application_SaveCompleted"]), Localization["MessageBox_Information"],
										   MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
            ApplicationForms.HideProgressBar();
            ApplicationForms.HideLoadingIcon();
		}

		private void ChangeTreeViewSelected(String mapId)
		{
			if (nodeArr.ContainsKey(mapId))
				_mapTreeView.SelectedNode = nodeArr[mapId];

			if (mapId == "Root")
				_mapTreeView.SelectedNode = _mapTreeView.Nodes[0];
		}

		private static readonly CultureInfo _enus = new CultureInfo("en-US");
		private void BtnAddMapClick(object sender, EventArgs e)
		{
			var file = new OpenFileDialog
			{
				Filter = @"Image Files(*.jpg *.png *.bmp *.gif)|*.jpg;*.png;*bmp;*.gif"
			};

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
				//_mapHandler.CheckFileNameCount = 1;
				// finalFileName = _mapHandler.CheckMapFileName(finalFileName);
		  
				//File.Copy(file.FileName, String.Format("{0}{1}", folderPath, finalFileName), false);

		   var image = Image.FromFile(file.FileName);
			

			//var maps = CMS.NVR.MapDoc.GetElementsByTagName("Map");

		   var mapId = _mapHandler.CreateMapReturnMapId(Localization["EMap_NewMapName"], file.SafeFileName, image.Width.ToString(), image.Height.ToString(), false, "10", _currentMapId == "Root" ? String.Empty : _currentMapId);

			var ext = Path.GetExtension(file.FileName);

			var finalFileName = String.Format("Map{0}{1}",mapId,ext);

			var map = new Bitmap(file.FileName);

			//Boolean result = CMS.NVR.UploadMap(map, finalFileName);

			//if (result == false)
			//{
			//    TopMostMessageBox.Show(Localization["EMap_MessageBoxUploadMapFail"], Localization["MessageBox_Information"]);
			//    _mapHandler.RemoveMapById(mapId);
			//    return;
			//}

			//_mapHandler.UpdateMapData(CMS.NVR.MapDoc, "Map", mapId, "SystemFile", finalFileName);
			var cMap = CMS.NVRManager.FindMapById(mapId);
			if (cMap == null) return;
			cMap.SystemFile = finalFileName;
			cMap.Image = map;
			//notify e-map's map list to change
			if (OnChangeMapCountFromMaps != null)
			{
				var eventArgs = new EventArgs<String>(String.Format("<xml></xml>"));
				OnChangeMapCountFromMaps(this, eventArgs);
				PropertiesSetting.ChangeMapCount(this, eventArgs);
			}
			_newMaps.Add(mapId);
			_mapLoadLock = true;
			CreateMapTree();
			ChangeTreeViewSelected(mapId);
			_mapLoadLock = false;
			ChangeMap(mapId, "ModifyImage");
	  
		}

		private void ButtonDeleteClick(object sender, EventArgs e)
		{
			var treeNode = _mapTreeView.SelectedNode;
			if (treeNode != null)
			{
				if (treeNode.Name != @"Root")
				{
					DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxRemoveMapConfirm"].Replace("%1", treeNode.Text), Localization["MessageBox_Confirm"],
											 MessageBoxButtons.OKCancel, MessageBoxIcon.Question);


					if (result == DialogResult.OK)
					{
						//_mapHandler.DeleteMapData(CMS.NVR.MapDoc, "Map", treeNode.Name);
						_deleteMaps.Add(treeNode.Text);
						var deleteVia = new List<String>();
						var deleteNVR = new List<String>();
                        var deletePolygon = new List<String>();

						foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
						{
							foreach (KeyValuePair<String, ViaAttributes> via in map.Value.Vias)
							{
								if (via.Value.LinkToMap == treeNode.Name)
								{
									//_mapHandler.RemoveViaById(via.Key);
									deleteVia.Add(via.Key);
								}
							}

							foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
							{
								if(nvr.Value.LinkToMap == treeNode.Name)
								{
									//_mapHandler.RemoveNVRById(nvr.Key);
									deleteNVR.Add(nvr.Key);
								}
							}

                            foreach (KeyValuePair<String, MapHotZoneAttributes> hotZone in map.Value.HotZones)
                            {
                                if (hotZone.Value.LinkToMap == treeNode.Name)
                                {
                                    //_mapHandler.RemoveNVRById(nvr.Key);
                                    deletePolygon.Add(hotZone.Key);
                                }
                            }
						}
						_mapHandler.RemoveMapById(treeNode.Name);

						foreach (string id in deleteVia)
						{
							_mapHandler.RemoveViaById(id);
						}

						foreach (string id in deleteNVR)
						{
							_mapHandler.RemoveNVRById(id);
						}

                        foreach (string id in deletePolygon)
                        {
                            _mapHandler.RemoveHotZoneById(id);
                        }

						var newMapId = "Root";

						var defaultMap = _mapHandler.FindDefaultMap();

						if(defaultMap!=null)
						{
							newMapId = defaultMap.Id;
						}

						//notify e-map's map list to change
						if (OnChangeMapCountFromMaps != null)
						{
							var eventArgs = new EventArgs<String>(String.Format("<xml></xml>"));
							OnChangeMapCountFromMaps(this, eventArgs);
							PropertiesSetting.ChangeMapCount(this, eventArgs);
						}

						_currentMapId = newMapId;
						
						ChangeMap(newMapId,"Refresh");
						_mapLoadLock = true;
						CreateMapTree();
						ChangeTreeViewSelected(newMapId);
						_mapLoadLock = false;
					}
				}
			}
		}

		public void LogEventAlarm(Object sender, EventArgs<String> e)
		{
			_alarmMaps.Clear();
			foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
			{
				foreach (KeyValuePair<string, CameraAttributes> camera in map.Value.Cameras)
				{
					if(camera.Value.IsEventAlarm)
					{
						INVR nvr = CMS.NVRManager.FindNVRById(camera.Value.NVRSystemId);
						if (nvr == null) continue;
						if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
						if (nvr.Device.FindDeviceById(Convert.ToUInt16(camera.Value.SystemId)) == null) continue;

						if (!_alarmMaps.Contains(map.Key))
						{
							_alarmMaps.Add(map.Key);
							break;
						}
					}
				}
			}

			if (_mode == "View")
			{
				foreach (KeyValuePair<String, TreeNode> n in nodeArr)
				{
					var node = n.Value;
					if (_alarmMaps.Contains(node.Name))
					{
						if (node.ImageIndex != 3)
						{
							if (!_tempMapsImages.ContainsKey(node.Name))
							{
								_tempMapsImages.Add(node.Name, node.ImageIndex);
							}
						}
						node.ImageIndex = 3;
						node.SelectedImageIndex = 3;
						ExpandParentNodes(node);
						//var map = CMS.NVR.FindMapById(node.Name);
						//if(map == null) continue;
						//if (!String.IsNullOrEmpty(map.ParentId))
						//    ExpandParentNodes(map.ParentId);
					}
					else
					{
						if (_tempMapsImages.ContainsKey(node.Name))
						{
							node.ImageIndex = _tempMapsImages[node.Name];
							node.SelectedImageIndex = _tempMapsImages[node.Name];
							_tempMapsImages.Remove(node.Name);
						}
					}
				}
			}
		}

		private void ExpandParentNodes(TreeNode node)
		{
			var parentNode = node.Parent;
			if (parentNode != null)
			{
				parentNode.Expand();

				if (parentNode.Parent != null)
					ExpandParentNodes(parentNode);
			}
		}

	}
}
