using Constant;
using Interface;
//using PanelBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PanelBase;
using VideoMonitor;
//using ApplicationForms = App.ApplicationForms;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;
using Cursors = System.Windows.Input.Cursors;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using FontFamily = System.Windows.Media.FontFamily;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using Menu = VideoMonitor.VideoMenu;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Orientation = System.Windows.Controls.Orientation;
using Point = System.Windows.Point;
using ProgressBar = System.Windows.Controls.ProgressBar;
using Size = System.Windows.Size;
using ToolTip = System.Windows.Controls.ToolTip;

namespace EMap
{
    /// <summary>
    /// Interaction logic for PadControl.xaml
    /// </summary>
    public partial class PadControl 
    {
        public event EventHandler<EventArgs<String>> OnNVRClick;
        public event EventHandler<EventArgs<String>> OnCameraClick;
        public event EventHandler<EventArgs<String>> OnCameraEditClick;
        public event EventHandler<EventArgs<String>> OnViaClick;
        public event EventHandler<EventArgs<CameraDescritpion>> OnMoveDescription;
        public event EventHandler<EventArgs<CameraAttributes>> OnMoveCamera;
        public event EventHandler<EventArgs<ViaAttributes>> OnMoveVia;
        public event EventHandler<EventArgs<NVRAttributes>> OnMoveNVR;
        public event EventHandler<EventArgs<CameraAroundItem>> OnMoveCameraAroundItem;
        public event EventHandler<EventArgs<String,String>> OnDescriptionEditClick;
        public event EventHandler<EventArgs<Point>> OnMoveMap;
        public event EventHandler<EventArgs<Double,Point>> OnScaleMap;
        public event EventHandler<EventArgs<String,Double, Double>> OnMoveVideoWindow;
        public event EventHandler<EventArgs<MapHotZoneAttributes>> OnCompleteDrawingHotZone;
        public event EventHandler<EventArgs<String>> OnClickHotZone;
        public event EventHandler<EventArgs<String,Int32, System.Drawing.Point>> OnChangeHotZonePoint;
        public event EventHandler<EventArgs<ViaAttributes>> OnCompleteAddingVia;
        public event EventHandler<EventArgs<String>> OnClickAlarm;
        public event EventHandler<EventArgs<String,DateTime>> OnClickEventList;
        public event EventHandler<EventArgs<String>> OnClickRemoveAllEvent;
        public event EventHandler<EventArgs<String>> OnClickChangeMap;

        public IServer Server;
        public ICMS CMS;
        public String Mode;
        public Double Scale;
        public Canvas MainCanvas;
        private ScaleTransform _cvsScaleTransform;
        private Dictionary<String, String> _activateCameras;
        private String _currentMapId;
        private Canvas _canvas;
        public Point MainCanvasPoint;
        public Size ParentViewSize { get; set; }
        public Dictionary<String, BitmapSource> Maps;
        public Dictionary<String, String> Localization;
        private Boolean _startMoveVideoWindow;
        private WindowsFormsHost _eventListPanel;
        private System.Windows.Forms.ListBox _eventList;
        private DataTable _eventTable;
        private readonly System.Windows.Forms.ToolTip _toolTip = new System.Windows.Forms.ToolTip();
        private Dictionary<String, BitmapSource> _cameraIcons;
        private Dictionary<String, BitmapSource> _cameraActivateIcons;
        private Dictionary<String, BitmapSource> _cameraOverIcons;
        private Dictionary<String, BitmapSource> _cameraDisconnectIcons;
        private Dictionary<String, BitmapSource> _icons;
        private ToolTip _hotzonePointTooltip;

        public PadControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   {"EMap_MessageBoxReadImageFailed", "Read map failed. Please try again."},
                                   {"EMap_MessageBoxDrawingHotZoneFailed", "This is not a polygon. Do you want to quit this drawing?"},
                                   {"EMap_ButtonEventListReload", "Click here to reload events from the device."},
                                   {"EMap_ButtonEventListReset", "Click here to reset events to empty."},
                                   {"EMap_ButtonEventListClose", "Close events list."},
                                   {"EMap_PointCompleteHotZone", "Click to complete this polygon."},
                                   {"EMap_AlarmEventList", "Click to show event list."},
                               };

            Localizations.Update(Localization);

            _cameraIcons = new Dictionary<string, BitmapSource>();
            _cameraIcons.Add("0", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._0, Properties.Resources.IMGCamera0)));
            _cameraIcons.Add("1", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._1, Properties.Resources.IMGCamera1)));
            _cameraIcons.Add("2", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._2, Properties.Resources.IMGCamera2)));
            _cameraIcons.Add("3", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._3, Properties.Resources.IMGCamera3)));
            _cameraIcons.Add("4", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._4, Properties.Resources.IMGCamera4)));
            _cameraIcons.Add("5", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._5, Properties.Resources.IMGCamera5)));
            _cameraIcons.Add("6", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._6, Properties.Resources.IMGCamera6)));
            _cameraIcons.Add("7", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._7, Properties.Resources.IMGCamera7)));

            _cameraOverIcons = new Dictionary<string, BitmapSource>();
            _cameraOverIcons.Add("0", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._0_over, Properties.Resources.IMGCameraOver0)));
            _cameraOverIcons.Add("1", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._1_over, Properties.Resources.IMGCameraOver1)));
            _cameraOverIcons.Add("2", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._2_over, Properties.Resources.IMGCameraOver2)));
            _cameraOverIcons.Add("3", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._3_over, Properties.Resources.IMGCameraOver3)));
            _cameraOverIcons.Add("4", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._4_over, Properties.Resources.IMGCameraOver4)));
            _cameraOverIcons.Add("5", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._5_over, Properties.Resources.IMGCameraOver5)));
            _cameraOverIcons.Add("6", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._6_over, Properties.Resources.IMGCameraOver6)));
            _cameraOverIcons.Add("7", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._7_over, Properties.Resources.IMGCameraOver7)));

            _cameraActivateIcons = new Dictionary<string, BitmapSource>();
            _cameraActivateIcons.Add("0", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._0_activate, Properties.Resources.IMGCameraActivate0)));
            _cameraActivateIcons.Add("1", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._1_activate, Properties.Resources.IMGCameraActivate1)));
            _cameraActivateIcons.Add("2", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._2_activate, Properties.Resources.IMGCameraActivate2)));
            _cameraActivateIcons.Add("3", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._3_activate, Properties.Resources.IMGCameraActivate3)));
            _cameraActivateIcons.Add("4", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._4_activate, Properties.Resources.IMGCameraActivate4)));
            _cameraActivateIcons.Add("5", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._5_activate, Properties.Resources.IMGCameraActivate5)));
            _cameraActivateIcons.Add("6", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._6_activate, Properties.Resources.IMGCameraActivate6)));
            _cameraActivateIcons.Add("7", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._7_activate, Properties.Resources.IMGCameraActivate7)));

            _cameraDisconnectIcons = new Dictionary<string, BitmapSource>();
            _cameraDisconnectIcons.Add("0", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._0_disconnect, Properties.Resources.IMGCameraDisconnect0)));
            _cameraDisconnectIcons.Add("1", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._1_disconnect, Properties.Resources.IMGCameraDisconnect1)));
            _cameraDisconnectIcons.Add("2", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._2_disconnect, Properties.Resources.IMGCameraDisconnect2)));
            _cameraDisconnectIcons.Add("3", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._3_disconnect, Properties.Resources.IMGCameraDisconnect3)));
            _cameraDisconnectIcons.Add("4", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._4_disconnect, Properties.Resources.IMGCameraDisconnect4)));
            _cameraDisconnectIcons.Add("5", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._5_disconnect, Properties.Resources.IMGCameraDisconnect5)));
            _cameraDisconnectIcons.Add("6", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._6_disconnect, Properties.Resources.IMGCameraDisconnect6)));
            _cameraDisconnectIcons.Add("7", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources._7_disconnect, Properties.Resources.IMGCameraDisconnect7)));

            _icons = new Dictionary<string, BitmapSource>();
            _icons.Add("Speaker", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.speaker, Properties.Resources.IMGSpeaker)));
            _icons.Add("Audio", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.audio, Properties.Resources.IMGAudio)));
            _icons.Add("NVR", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.NVR, Properties.Resources.IMGNVR)));
            _icons.Add("NVRActivate", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.NVR_activate, Properties.Resources.IMGNVRActivate)));
            _icons.Add("NVRDisconnect", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.NVR_disconnect, Properties.Resources.IMGNVRDisconnect)));
            _icons.Add("Via", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.icon_transfer, Properties.Resources.IMGVia)));
            _icons.Add("ViaActivate", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.icon_transfer_activate, Properties.Resources.IMGViaActivate)));
            _icons.Add("Alarm", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.alarm, Properties.Resources.IMGAlarm)));
            _icons.Add("HotZoneMainPoint", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.HotZone, Properties.Resources.IMGHotZone)));
            _icons.Add("HotZoneMainActivatePoint", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.HotZoneActivate, Properties.Resources.IMGHotZoneActivate)));
            _icons.Add("HotZoneRedPoint", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.HotZoneRedPoint, Properties.Resources.IMGHotZoneRedPoint)));
            _icons.Add("HotZoneGrayPoint", Converter.ImageToBitmapSource(Constant.Resources.GetResources(Properties.Resources.HotZoneGreyPoint, Properties.Resources.IMGHotZoneGreyPoint)));

            _hotzonePointTooltip = new ToolTip
                                       {
                                           Content = Localization["EMap_PointCompleteHotZone"]
                                       };

            _canvas = new Canvas
            {
                Width = Width,
                Height = Height,
                Background = Brushes.Black,
                AllowDrop = true,
                
            };

            MainCanvas = new Canvas
            {
                Width = Width,
                Height = Height,
                Background = Brushes.Black,
                AllowDrop = true,
            };

            MainCanvas.Drop += DropNodeOnCanvas;
            MainCanvas.DragOver += MainCanvasDragOver;
            MainCanvas.MouseLeftButtonDown += MainCanvasMouseLeftButtonDown;

            Background = Brushes.Black;
            
            Maps = new Dictionary<string, BitmapSource>();
            _canvas.Drop += PadControlDrop;
            _canvas.DragOver += CanvasDragOver;
            
            var mainWindow = this;
            mainWindow.Content = null;
            mainWindow.Content = _canvas;

            InitializeComponent();
            ClearPadWindow();
            AllowDrop = true;
            
            _activateCameras = new Dictionary<String, String>();
            _startMoveVideoWindow = false;

            _toolMenuPanel = new WindowsFormsHost
            {
                Name = "ToolMenu",
                Width = 30,
                //Background = Brushes.Transparent,
                //OpacityMask = Brushes.Transparent,
            };
            _toolMenuPanel.SizeChanged += ToolMenuPanelSizeChanged;

            //Hot zone drawing line
            CurrentLine = new Line
            {
                Stroke = Brushes.OrangeRed,
                StrokeThickness = 3
            };

            //Create event list panel
            _eventListPanel = new WindowsFormsHost
                                  {
                                      Background = Brushes.DimGray,
                                      Width = 125,
                                      Height = 120
                                  };

            var eventListPanel = new System.Windows.Forms.Panel
                                     {
                                         Dock = DockStyle.Fill,
                                         BorderStyle = BorderStyle.None
                                     };

            var innerTopPanel = new System.Windows.Forms.Panel
                                    {
                                        Height = 20,
                                        Dock = DockStyle.Top,
                                        BorderStyle = BorderStyle.None
                                    };

            var btnClose = new System.Windows.Forms.Button
                               {
                                   Width = 20,
                                   Height = 20,
                                   Dock = DockStyle.Right,
                                   BackColor = Color.DarkRed,
                                   BackgroundImage = Constant.Resources.GetResources(Properties.Resources.windowClose, Properties.Resources.IMGCloseWindow),
                                   BackgroundImageLayout = ImageLayout.Stretch
                               };
            btnClose.Click += EventListCloseClick;
            _toolTip.SetToolTip(btnClose, Localization["EMap_ButtonEventListClose"]);
            innerTopPanel.Controls.Add(btnClose);

            var btnRemove = new System.Windows.Forms.Button
            {
                Width = 20,
                Height = 20,
                Dock = DockStyle.Left,
                BackColor = Color.DimGray,
                BackgroundImage = Constant.Resources.GetResources(Properties.Resources.reset, Properties.Resources.IMGResetEvent),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            btnRemove.Click += EventListRemoveEventClick;
            _toolTip.SetToolTip(btnRemove, Localization["EMap_ButtonEventListReset"]);
            innerTopPanel.Controls.Add(btnRemove);

            var btnReload = new System.Windows.Forms.Button
            {
                Width = 20,
                Height = 20,
                Dock = DockStyle.Left,
                BackColor = Color.DimGray,
                BackgroundImage = Constant.Resources.GetResources(Properties.Resources.reload, Properties.Resources.IMGReload),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            btnReload.Click += EventListReloadEventClick;
            _toolTip.SetToolTip(btnReload, Localization["EMap_ButtonEventListReload"]);
            innerTopPanel.Controls.Add(btnReload);

            eventListPanel.Controls.Add(innerTopPanel);
            
            _eventList = new System.Windows.Forms.ListBox
                             {
                                 BackColor = Color.Black,
                                 Dock = DockStyle.Fill,
                                 Height = 120,
                                 ForeColor = Color.White,
                                 Font = new Font("Arial", 8),
                                 ItemHeight = 10,
                                 BorderStyle = BorderStyle.None
                             };
            _eventList.DisplayMember = "Text";
            _eventList.ValueMember = "DateTime";
            _eventTable = new DataTable();
            _eventTable.Columns.Add("Text", typeof(String));
            _eventTable.Columns.Add("DateTime", typeof(DateTime));
            _eventList.DataSource = _eventTable;
            _eventList.Click += EventListClick;
            
            eventListPanel.Controls.Add(_eventList);
            _eventList.BringToFront();
            _eventListPanel.Child = eventListPanel;
        }
        
        private void ToolMenuPanelSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            if (ToolMenu != null)
                _toolMenuPanel.Child = ToolMenu;
        }

        public Point ReadClientPoint(Point p)
        {
            Mouse.GetPosition(MainCanvas);
            return PointToScreen(Mouse.GetPosition(MainCanvas));
        }

        public void ClearPadWindow()
        {
            _canvas.Children.Clear();
            _currentMapId = String.Empty;
            CurrentMainCanvasPoint = new Point(0,0);
            CurrentNodePoint = new Point(0,0);
        }

        public void CreateAndShowMainWindow(Double x,Double y,Double scale,Double centerX,Double centerY)
        {
            MainCanvas.Children.Clear();
            _currentNVRId = String.Empty;
            _currentViaId = String.Empty;

            var trueScale = scale / 10;
            _cvsScaleTransform = new ScaleTransform(trueScale, trueScale, centerX, centerY);

            MainCanvas.RenderTransform = _cvsScaleTransform;
            CurrentMainCanvasPoint = new Point(x, y);
            MainCanvasPoint = new Point(x, y);
            Canvas.SetTop(MainCanvas,y);
            Canvas.SetLeft(MainCanvas, x);
            
            _canvas.Children.Clear();
            _canvas.Children.Add(MainCanvas);
            _currentCameraClick = String.Empty;

            if (IsDrawingHotZone)
            {
                QuitDrawingHotZoneOnMap();
            }

            QuitAddingViaOnMap();
        }

        public void CreateMapByObject(MapAttribute mapAttribute )
        {
            MainCanvas.Children.Add(CreateImageNode("Map", false, null, mapAttribute.Width, mapAttribute.Height, 0, 0, 0, mapAttribute,null));
            _currentMapId = mapAttribute.Id;

            String activateCamId;
            _activateCameras.TryGetValue(_currentMapId, out activateCamId);

            //hotzone is lowest layer for avoid cover cameras
            foreach (var hotzone in mapAttribute.HotZones)
            {
                var myPolygon = CreateBasicPolygonByHotZoneAttribute(hotzone.Value);
                myPolygon.Cursor = Mode == "View" ? Cursors.Hand : Cursors.Arrow;
                var myPointCollection = new PointCollection();
                foreach (System.Drawing.Point point in hotzone.Value.Points)
                {
                    myPointCollection.Add(new Point(point.X, point.Y));
                }

                myPolygon.Points = myPointCollection;
                myPolygon.ToolTip = new ToolTip { Content = hotzone.Value.Name };
                myPolygon.MouseLeftButtonDown += PolygonMouseLeftButtonDown;
                myPolygon.DataContext = hotzone.Value;
                MainCanvas.Children.Add(myPolygon);
                CreateNameLabel(hotzone.Value.Id, "HotZone", hotzone.Value.Name, hotzone.Value.DescX, hotzone.Value.DescY);
            }

            foreach (var camera in mapAttribute.Cameras)
            {
                INVR nvr = CMS.NVRManager.FindNVRById(camera.Value.NVRSystemId);
                if (nvr == null) continue;
                if (nvr.ReadyState == ReadyState.NotInUse) continue;
                if (nvr.Device.FindDeviceById(Convert.ToUInt16(camera.Value.SystemId)) == null) continue;

                var imgNo = camera.Value.Rotate / 45;

                if (imgNo>7)
                {
                    imgNo = 0;
                }

                var isActivate = false;

                if (String.IsNullOrEmpty(activateCamId) == false)
                {
                    if (activateCamId == camera.Value.Id)
                    {
                        isActivate = true;
                    }
                }

                var icon = isActivate ? _cameraActivateIcons[imgNo.ToString()] : _cameraIcons[imgNo.ToString()];
                if (camera.Value.CameraStatus.ToUpper() == "NOSINGAL")
                {
                    icon = _cameraDisconnectIcons[imgNo.ToString()];
                }
               
                MainCanvas.Children.Add(CreateImageNode("Camera", true, icon, 50, 50, camera.Value.X, camera.Value.Y, camera.Value.Rotate, camera.Value,camera.Value.Name));
                CreateNameLabel(camera.Value.Id, "Camera", camera.Value.Name, camera.Value.DescX, camera.Value.DescY);

                if (camera.Value.IsSpeakerEnabled)
                {
                    var speaker = new CameraAroundItem
                                      {
                                          CameraId = camera.Value.Id,
                                          Type = "Speaker",
                                          X = camera.Value.SpeakerX,
                                          Y = camera.Value.SpeakerY
                                      };
                    MainCanvas.Children.Add(CreateImageNode("Speaker", true, _icons["Speaker"], 25, 25, camera.Value.SpeakerX, camera.Value.SpeakerY, 0, speaker,null));
                }

                if (camera.Value.IsAudioEnabled)
                {
                    var audio = new CameraAroundItem
                    {
                        CameraId = camera.Value.Id,
                        Type = "Audio",
                        X = camera.Value.AudioX,
                        Y = camera.Value.AudioY
                    };
                    MainCanvas.Children.Add(CreateImageNode("Audio", true, _icons["Audio"], 25, 25, camera.Value.AudioX, camera.Value.AudioY, 0, audio,null));
                }

                if(camera.Value.IsEventAlarm)
                {
                    CreateEventAlarmByCamera(camera.Value);
                }

            }

            foreach (var nvrItem in mapAttribute.NVRs)
            {
                INVR nvr = CMS.NVRManager.FindNVRById(nvrItem.Value.SystemId);
                if (nvr == null) continue;
                if (nvr.ReadyState == ReadyState.NotInUse) continue;

                var icon = _icons["NVR"];
                if (nvrItem.Value.Status == NVRStatus.NoSignal || nvrItem.Value.Status== NVRStatus.WrongAccountPassowrd)
                {
                    icon = _icons["NVRDisconnect"];
                }

                MainCanvas.Children.Add(CreateImageNode("NVR", true, icon, 50, 50, nvrItem.Value.X, nvrItem.Value.Y, 0, nvrItem.Value, nvrItem.Value.Name));
                CreateNameLabel(nvrItem.Value.Id, "NVR", nvrItem.Value.Name, nvrItem.Value.DescX, nvrItem.Value.DescY);
            }

            foreach (var via in mapAttribute.Vias)
            {
                MainCanvas.Children.Add(CreateImageNode("Via", true, _icons["Via"], 50, 25, via.Value.X, via.Value.Y, 0, via.Value,via.Value.Name));
                CreateNameLabel(via.Value.Id, "Via", via.Value.Name, via.Value.DescX, via.Value.DescY);
            }
        }

        private Dictionary<String, MapAttribute> _tempMapList = new Dictionary<String, MapAttribute>();
        public void CreateRootMap()
        {
            _tempMapList.Clear();

            var trueScale = 1;
            _cvsScaleTransform = new ScaleTransform(trueScale, trueScale, 0, 0);

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                if (!String.IsNullOrEmpty(map.Value.ParentId)) continue;
                _tempMapList.Add(map.Value.Id, map.Value);
                ParseMapForList(map.Key);
            }

            var count = 0.0;
            var x = 0;
            var y = 0;
            var width = 200;
            var height = 150;
            foreach (KeyValuePair<String, MapAttribute> map in _tempMapList)
            {
                count++;
                var mapIcon = CreateImageNode("Map", false, null, width, height, x, y, 0, map.Value, map.Value.Name);
                mapIcon.Cursor = Cursors.Hand;
                mapIcon.DataContext = map.Key;
                mapIcon.MouseDown += MapIconMouseDown;
                MainCanvas.Children.Add(mapIcon);
                CreateNameLabel(map.Key, "Map", map.Value.Name, x+(width/2), y+height-8);
                if(count % 3 > 0)
                {
                    x += width;
                }
                else
                {
                    x = 0;
                }

                y = Convert.ToInt16(Math.Floor(count / 3)) * height;
            }

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (var camera in map.Value.Cameras)
                {
                    if (!camera.Value.IsEventAlarm) continue;
                    CreateMapEventAlarmById(map.Key);
                    break;
                }
            }
        }

        private void ParseMapForList(String parentId)
        {
            foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
            {
                if (map.Value.ParentId != parentId) continue;
                _tempMapList.Add(map.Value.Id, map.Value);
                ParseMapForList(map.Value.Id);
            }
        }

        private void MapIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as Image;
            if (target == null) return;
            
            var dataContext = target.DataContext as CameraDescritpion;
            var mapId = dataContext == null ? target.DataContext.ToString() : dataContext.Id; 

            OnClickChangeMap(this, new EventArgs<String>(mapId));
        }

        private void MapEventPolygonMouseDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as Polyline;
            if (target == null) return;

            var mapId = target.DataContext.ToString();

            OnClickChangeMap(this, new EventArgs<String>(mapId));
        }

        public void CreateMapEventAlarmById(String mapId)
        {
            if (Mode == "Setup") return;
            var check = FindMapEventAlarmByCameraId(mapId);
            if (check != null) return;

            var mapIcon = FindMapIconById(mapId);
            if (mapIcon == null) return;
            var myPolygon = new Polyline
            {
                Stroke = Brushes.Brown,
                Fill = Brushes.Red,
                Opacity = 0.3,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                DataContext = mapId,
                Cursor = Cursors.Hand
            };

            var myPointCollection = new PointCollection
                                        {
                                            new Point(Canvas.GetLeft(mapIcon), Canvas.GetTop(mapIcon)),
                                            new Point(Canvas.GetLeft(mapIcon) + mapIcon.Width, Canvas.GetTop(mapIcon)),
                                            new Point(Canvas.GetLeft(mapIcon) + mapIcon.Width,
                                                      Canvas.GetTop(mapIcon) + mapIcon.Height),
                                            new Point(Canvas.GetLeft(mapIcon), Canvas.GetTop(mapIcon) + mapIcon.Height),
                                            new Point(Canvas.GetLeft(mapIcon), Canvas.GetTop(mapIcon))
                                        };

            myPolygon.Points = myPointCollection;
            myPolygon.MouseDown += MapEventPolygonMouseDown;
            MainCanvas.Children.Add(myPolygon);

            var mapLabel = FindMapLabelById(mapId);
            if (mapLabel == null) return;
            var mapIconData = mapLabel.DataContext as CameraDescritpion;
            if (mapIconData == null) return;
            var alarmIcon = CreateImageNode("MapAlarm", true, _icons["Alarm"], 25, 25, mapIconData.DescX - 50,
                                            mapIconData.DescY - 20, 0, mapId, "");
            alarmIcon.MouseDown += MapIconMouseDown;
            MainCanvas.Children.Add(alarmIcon);
        }

        public TextBlock FindMapLabelById(String mapId)
        {
            return (from node in MainCanvas.Children.OfType<TextBlock>()
                    let source = node.DataContext as CameraDescritpion
                    where source != null
                    where source.Id == mapId
                    select node).FirstOrDefault();
        }

        public Image FindMapIconById(String mapId)
        {
            return (from node in MainCanvas.Children.OfType<Image>()
                    where node.Name == "Map"
                    let source = node.DataContext as String
                    where source == mapId
                    select node).FirstOrDefault();
        }

        public Image FindMapEventAlarmByCameraId(String mapId)
        {
            return (from node in MainCanvas.Children.OfType<Image>()
                    where node.Name == "MapAlarm"
                    let source = node.DataContext as String
                    where source == mapId
                    select node).FirstOrDefault();
        }

        public void CreateEventAlarmByCamera(CameraAttributes camera)
        {
            if(Mode == "Setup") return;
            var check = FindEventAlarmByCameraId(camera.Id);
            if(check != null) return;
            var alarm = new CameraAroundItem
            {
                CameraId = camera.Id,
                Type = "Alarm",
                X = camera.X-20,
                Y = camera.Y
            };
            MainCanvas.Children.Add(CreateImageNode("Alarm", true, _icons["Alarm"], 25, 25, camera.X - 20, camera.Y, 0, alarm, Localization["EMap_AlarmEventList"]));
        }

        public Image FindEventAlarmByCameraId(String camId)
        {
            return (from node in MainCanvas.Children.OfType<Image>()
                    where node.Name == "Alarm"
                    let source = node.DataContext as CameraAroundItem
                    where source != null
                    where source.CameraId == camId
                    select node).FirstOrDefault();
        }

        public void RemoveEventAlarmByCameraId(String camId)
        {
            var alarm = FindEventAlarmByCameraId(camId);
            if (alarm != null)
            {
                MainCanvas.Children.Remove(alarm);
            }
        }

        public void CreateEventListByCamera(CameraAttributes camera)
        {
            if (Mode == "Setup") return;
            //return;
            if(_canvas.Children.Contains(_eventListPanel))
            {
                //if (_eventListPanel.Name == String.Format("C{0}", camera.Id)) {return;}
                _canvas.Children.Remove(_eventListPanel);
            }

           _eventTable.Clear();
            _eventList.Name = camera.Id;

            for (var i = camera.EventRecords.Count-1; i >= 0; i--)
            {
                var record = camera.EventRecords[i];
                
                var dt = String.Format("{0:yyyy/MM/dd HH:mm:ss}", record.EventTime);
                _eventTable.Rows.Add(dt, record.EventTime);
            }
            var newPoint = CaculateVideoWindowPositionToCanvas(camera.VideoWindowX, camera.VideoWindowY);
            Canvas.SetTop(_eventListPanel, newPoint.Y);
            Canvas.SetLeft(_eventListPanel, newPoint.X - _eventListPanel.Width);
            _eventListPanel.Name = String.Format("C{0}", camera.Id);
            _eventListPanel.DataContext = camera;
            _canvas.Children.Add(_eventListPanel);
            EventListClick(null, null);
        }

        private void EventListClick(object sender, EventArgs e)
        {
            //if sender == null --> manually call
            if(_eventListPanel != null && _eventList != null)
            {
                var camId = _eventList.Name;
                if (OnClickEventList != null)
                {
                    var dataSource = _eventList.DataSource as DataTable;
                    if(dataSource != null)
                    {
                        //var dt = DateTime.ParseExact(_eventTable.Rows[_eventList.SelectedIndex]..ToString().Replace("*",""), "yyyy/MM/dd HH:mm:ss", null);
                        var dt = _eventList.SelectedValue;
                        if (_eventTable.Rows.Count > 0)
                            if (_eventTable.Rows[_eventList.SelectedIndex]["Text"].ToString().IndexOf("*") < 0)
                            {
                                _eventTable.Rows[_eventList.SelectedIndex]["Text"] = String.Format("*{0}", _eventTable.Rows[_eventList.SelectedIndex]["Text"]);
                            }

                        if (OnClickEventList != null)// && sender != null
                        {
                            if (dt == null)
                            {
                                RemoveEventListPanel();
                            }
                            else
                            {
                                OnClickEventList(this, new EventArgs<String, DateTime>(camId, (DateTime)dt));
                            }
                        }
                    }
                }
            }
        }

        private void EventListCloseClick(object sender, EventArgs e)
        {
            RemoveEventListPanel();
        }

        private void EventListReloadEventClick(object sender, EventArgs e)
        {
            if (OnClickAlarm != null)
            {
                var cam = _eventListPanel.DataContext as CameraAttributes;
                if (cam != null)
                {
                    OnClickAlarm(this, new EventArgs<String>(cam.Id));
                }
            }
        }

        private void EventListRemoveEventClick(object sender, EventArgs e)
        {
            if (OnClickRemoveAllEvent != null)
            {
                var cam = _eventListPanel.DataContext as CameraAttributes;
                if (cam != null)
                {
                    OnClickRemoveAllEvent(this, new EventArgs<String>(cam.Id));
                }
            }
        }

        public void RemoveEventListPanel()
        {
            //return;
            if(_canvas.Children.Contains(_eventListPanel))
            {
                _canvas.Children.Remove(_eventListPanel);
            }
        }

        public void RemoveEventListPanelByCameraId(String camId)
        {
            //return;
            if (_canvas.Children.Contains(_eventListPanel))
            {
                if (_eventListPanel.Name == String.Format("C{0}",camId))
                {
                    _canvas.Children.Remove(_eventListPanel);
                }
            }
        }

        public ViaAttributes ViaPoint = null;
        public void StartAddViaOnMap(ViaAttributes via)
        {
            if(IsDrawingHotZone)
            {
                QuitDrawingHotZoneOnMap();
            }
            MainCanvas.Cursor = Cursors.Cross;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown -= CompleteAddingVia;
            MainCanvas.MouseLeftButtonDown += CompleteAddingVia;
            ViaPoint = via;
        }

        private void CompleteAddingVia(object sender, MouseButtonEventArgs e)
        {
            if(ViaPoint == null) return;

            var newPoint = e.GetPosition(MainCanvas);
            ViaPoint.X = newPoint.X;
            ViaPoint.Y = newPoint.Y;
            ViaPoint.DescX = newPoint.X;
            ViaPoint.DescY = newPoint.Y + 30;
            if (OnCompleteAddingVia != null)
            {
                OnCompleteAddingVia(this,new EventArgs<ViaAttributes>(ViaPoint));
            }

            QuitAddingViaOnMap();
        }

        public void QuitAddingViaOnMap()
        {
            ViaPoint = null;
            MainCanvas.Cursor = Cursors.Arrow;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown += MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown -= CompleteAddingVia;
        }

        public Boolean IsDrawingHotZone = false;
        public Boolean IsLockDrawingHotZone = false;
        public MapHotZoneAttributes HotZone;
        public Polygon CurrentHotZonePolygon;
        public Polyline CurrentHotZonePolylineForCreate;
        public Line CurrentLine = new Line();

        public void StartDrawHotZoneOnMap(MapHotZoneAttributes hotzone)
        {
            QuitAddingViaOnMap();
            if (IsDrawingHotZone)
            {
                QuitDrawingHotZoneOnMap();
            }
            MainCanvas.Cursor = Cursors.Cross;
            IsDrawingHotZone = true;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDownForHotZone;
            MainCanvas.MouseLeftButtonDown += MainCanvasMouseLeftButtonDownForHotZone;
            HotZone = hotzone;

            //var myPolygon = CreateBasicPolygonByHotZoneAttribute(hotzone);

            var myPolygon = new Polyline()
            {
                Stroke = Brushes.OrangeRed,
                Opacity = hotzone.Opacity == 0 ? 10 : hotzone.Opacity / 10,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                DataContext = hotzone
            };

            var myPointCollection = new PointCollection();
            myPolygon.Points = myPointCollection;
            MainCanvas.Children.Add(myPolygon);
            CurrentHotZonePolylineForCreate = myPolygon;
            IsLockDrawingHotZone = false;
        }

        private  Polygon CreateBasicPolygonByHotZoneAttribute(MapHotZoneAttributes hotzone)
        {
            var myPolygon = new Polygon
            {
                Stroke = Brushes.DarkGray,
                Fill = ConvertDrawingColorToMediaColor(hotzone.Color),
                Opacity = hotzone.Opacity ==0 ? 10 :  hotzone.Opacity/10,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                DataContext = hotzone
            };

            myPolygon.MouseEnter += PolygonMouseEnter;
            myPolygon.MouseLeave += PolygonMouseLeave;

            return myPolygon;
        }

        private static System.Windows.Media.Brush _tmpColor;
        private Double _tmpOpacity;

        private  void PolygonMouseLeave(object sender, MouseEventArgs e)
        {
            if (Mode == "Setup") return;
            var polygon = sender as Polygon;
            if (polygon != null)
            {
                MakePolygonToUnselected(polygon);
            }
        }

        private void MakePolygonToUnselected(Polygon polygon)
        {
            if (polygon != null)
            {
                //polygon.Fill = _tmpColor;
                polygon.Opacity = _tmpOpacity;
                polygon.StrokeThickness = 2;
                polygon.Stroke = Brushes.DarkGray;
                _tmpColor = null;
            }
        }

        private  void PolygonMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mode == "Setup") return;
            var polygon = sender as Polygon;
            if (polygon != null && _tmpColor == null)
            {
                MakePolygonToSelected(polygon);
            }
        }

        private void MakePolygonToSelected(Polygon polygon)
        {
            if (polygon != null && _tmpColor == null)
            {
                _tmpColor = polygon.Fill;
                _tmpOpacity = polygon.Opacity;
                //polygon.Fill = Brushes.DarkOrange;
                polygon.Opacity = 0.8;
                polygon.StrokeThickness = 4;
                polygon.Stroke = Brushes.OrangeRed;
            }
        }
        
        private void MakePolygonSelectedByLabel(TextBlock label, Boolean isSelected)
        {
            if (label != null)
            {
                var hotzone = label.DataContext as CameraDescritpion;
                if (hotzone != null)
                {
                    var zone = FindPolygonInMapById(hotzone.Id);
                    if (zone != null)
                    {
                        if(isSelected)
                        {
                            MakePolygonToSelected(zone);
                        }
                        else
                        {
                            MakePolygonToUnselected(zone);
                        }
                    }
                }
            }
        }

        private void HotZoneLabelMouseLeave(object sender, MouseEventArgs e)
        {
            if (Mode == "Setup") return;
            var label = sender as TextBlock;
            if(label != null)
            {
                MakePolygonSelectedByLabel(label,false);
            }
        }

        private void HotZoneLabelMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mode == "Setup") return;
            var label = sender as TextBlock;
            if (label != null)
            {
                MakePolygonSelectedByLabel(label, true);
            }
        }

        private void MainCanvasMouseLeftButtonDownForHotZone(object sender, MouseButtonEventArgs e)
        {
            if (IsDrawingHotZone == false) return;
            if (IsLockDrawingHotZone)
            {
                IsLockDrawingHotZone = false;
                return;
            }
            var newPoint = e.GetPosition(MainCanvas);

            if (CurrentLine != null)
            {
                CurrentLine.X1 = newPoint.X;
                CurrentLine.Y1 = newPoint.Y;
                CurrentLine.X2 = newPoint.X;
                CurrentLine.Y2 = newPoint.Y;
            }

            if (HotZone.Points.Count==0)
            {
                MainCanvas.Children.Add(CurrentLine);
                MainCanvas.MouseMove += MainCanvasMouseMoveForLine;

                var node = CreateImageNode("HotZone", false, _icons["HotZoneMainPoint"], 24, 24, newPoint.X,
                                           newPoint.Y, 0, null,null);
                node.MouseLeftButtonDown += CompleteHotZoneDrawing;
                node.MouseEnter += HotZoneMouseEnter;
                node.MouseLeave += HotZoneMouseLeave;
                node.ToolTip = _hotzonePointTooltip;
               
                MainCanvas.Children.Add(node);
            }
            else
            {
                MainCanvas.Children.Add(CreateImageNode("HotZone", false, _icons["HotZoneGrayPoint"], 16, 16, newPoint.X, newPoint.Y, 0, null,null));
                
            }

            HotZone.Points.Add(new System.Drawing.Point((int)newPoint.X, (int)newPoint.Y));

            CurrentHotZonePolylineForCreate.Points.Add(new Point(newPoint.X, newPoint.Y));

            IsLockDrawingHotZone = false;
        }

        private void HotZoneMouseLeave(object sender, MouseEventArgs e)
        {
            var node = sender as Image;
            if (node != null)
            {
                ChangeHotZoneHomePoint(node, false);
            }
        }

        private void HotZoneMouseEnter(object sender, MouseEventArgs e)
        {
            var node = sender as Image;
            if(node != null)
            {
                ChangeHotZoneHomePoint(node, true);
            }
        }

        private void ChangeHotZoneHomePoint(Image node, Boolean isActivate)
        {
            var tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = isActivate ? _icons["HotZoneMainActivatePoint"] : _icons["HotZoneMainPoint"];
            var transform = new RotateTransform(0);
            tb.Transform = transform;
            tb.EndInit();
            node.Source = tb;
        }

        private void CompleteHotZoneDrawing(object sender, MouseButtonEventArgs e)
        {
            if(IsDrawingHotZone == false) return;

            if (HotZone.Points.Count < 3)
            {
                DialogResult result = TopMostMessageBox.Show(Localization["EMap_MessageBoxDrawingHotZoneFailed"], Localization["MessageBox_Information"],
                                             MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    QuitDrawingHotZoneOnMap();
                    return;
                }
                else
                {
                    IsLockDrawingHotZone = true;
                    return;
                }
            }

            MainCanvas.Cursor = Cursors.Arrow;
            IsDrawingHotZone = false;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown += MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDownForHotZone;
            RemoveHotZoneLine();

            CurrentHotZonePolygon = CreateBasicPolygonByHotZoneAttribute(HotZone);
            CurrentHotZonePolygon.Points = CurrentHotZonePolylineForCreate.Points;

            var totalX = 0.0;
            var totalY = 0.0;
            foreach (Point point in CurrentHotZonePolygon.Points)
            {
                totalX += point.X;
                totalY += point.Y;
            }

            HotZone.DescX = totalX/CurrentHotZonePolygon.Points.Count;
            HotZone.DescY = totalY / CurrentHotZonePolygon.Points.Count;
            MainCanvas.Children.Add(CurrentHotZonePolygon);
            if (MainCanvas.Children.Contains(CurrentHotZonePolylineForCreate))
            {
                MainCanvas.Children.Remove(CurrentHotZonePolylineForCreate);
            }
            CurrentHotZonePolylineForCreate = null;

            if (CurrentHotZonePolygon != null) CurrentHotZonePolygon.MouseLeftButtonDown += PolygonMouseLeftButtonDown;
            if(OnCompleteDrawingHotZone != null)
            {
                OnCompleteDrawingHotZone(this,new EventArgs<MapHotZoneAttributes>(HotZone));
                DrawHotZonePoints();
            }
        }

        public void QuitDrawingHotZoneOnMap()
        {
            if (CurrentHotZonePolylineForCreate != null)
            {
                if (MainCanvas.Children.Contains(CurrentHotZonePolylineForCreate))
                {
                    MainCanvas.Children.Remove(CurrentHotZonePolylineForCreate);
                }
                CurrentHotZonePolylineForCreate = null;
            }
            RemoveHotZoneLine();
            RemoveHopZonePoints();
            
            MainCanvas.Children.Remove(CurrentHotZonePolygon);
            MainCanvas.Cursor = Cursors.Arrow;
            IsDrawingHotZone = false;
            IsLockDrawingHotZone = false;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown += MainCanvasMouseLeftButtonDown;
            MainCanvas.MouseLeftButtonDown -= MainCanvasMouseLeftButtonDownForHotZone;
            CurrentHotZonePolygon = null;
        }

        public void GiveIdToCurrentHotZoneAfterComplete(String id)
        {
            if(CurrentHotZonePolygon != null)
            {
                var hotzone = CurrentHotZonePolygon.DataContext as MapHotZoneAttributes;
                if(hotzone != null)
                {
                    hotzone.Id = id;

                    CreateNameLabel(id, "HotZone", hotzone.Name, hotzone.DescX,hotzone.DescY);
                }
            }
        }

        private void MainCanvasMouseMoveForLine(object sender, MouseEventArgs e)
        {
            var cPoint = e.GetPosition(MainCanvas);
            CurrentLine.X2 = cPoint.X;
            CurrentLine.Y2 = cPoint.Y;
        }

        private void RemoveHotZoneLine()
        {
            if (CurrentLine != null)
            {
                if (MainCanvas.Children.Contains(CurrentLine))
                {
                    MainCanvas.Children.Remove(CurrentLine);
                }
            }
        }

        private void PolygonMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDrawingHotZone) return;

            var zone = sender as Polygon;

            if (zone != null)
            {
                if (OnClickHotZone != null)
                {
                    var hotzone = zone.DataContext as MapHotZoneAttributes;
                    if (hotzone != null)
                    {
                        OnClickHotZone(this, new EventArgs<String>(hotzone.Id));
                    }
                }

                if (Mode == "View")
                {
                    return;
                }

                var polygon = sender as Polygon;
                if (polygon != null)
                {
                    CurrentHotZonePolygon = polygon;
                    DrawHotZonePoints();
                }

                //CurrentNodePoint = e.GetPosition(MainCanvas);
                var data = new DataObject(typeof(Polygon), zone);
                DragDrop.DoDragDrop(zone, data, DragDropEffects.None);

            }
        }

        public void UpdateHotZoneOpacity(String id, Double opacity)
        {
            if(CurrentHotZonePolygon != null)
            {
                var hotzone = CurrentHotZonePolygon.DataContext as MapHotZoneAttributes;
                if(hotzone != null)
                {
                    if(hotzone.Id == id)
                    {
                        CurrentHotZonePolygon.Opacity = opacity / 10;
                    }
                }
            }
        }

        public void UpdateHotZoneColor(String id, Color color)
        {
            if (CurrentHotZonePolygon != null)
            {
                var hotzone = CurrentHotZonePolygon.DataContext as MapHotZoneAttributes;
                if (hotzone != null)
                {
                    if (hotzone.Id == id)
                    {
                        CurrentHotZonePolygon.Fill = ConvertDrawingColorToMediaColor(color);
                    }
                }
            }
        }

        private static System.Windows.Media.Brush ConvertDrawingColorToMediaColor(Color color)
        {
            var tempColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            
            return  new SolidColorBrush(tempColor);
        }

        private void DrawHotZonePoints()
        {
            RemoveHopZonePoints();
            var count = 0;
            foreach (Point point in CurrentHotZonePolygon.Points)
            {
                var newPoint = CreateImageNode("HotZone", true, _icons["HotZoneRedPoint"], 16, 16, point.X, point.Y, 0,
                                            count, null);
                count++;
                MainCanvas.Children.Add(newPoint);
            }
            RemoveNVRAndViaActivate();
        }

        public void RemoveHopZonePoints()
        {
            var list = new List<Image>();

            foreach (var child in MainCanvas.Children)
            {
                var node = child as Image;
                if (node != null)
                {
                    if (node.Name == "HotZone")
                    {
                        list.Add(node);
                    }
                }
            }

            foreach (Image image in list)
            {
                MainCanvas.Children.Remove(image);
            }
            RemoveHotZoneLine();
        }

        public Image CreateNode(String name, Boolean isDrag, BitmapSource image, Int32 width, Int32 height, Int32 x, Int32 y)
        {
            return CreateImageNode(name, isDrag, image, width, height, x, y, 0, null,null);
        }

        public void RemoveMapDictionaryById(String id)
        {
            Maps.Remove(id);
        }

        private VideoWindow _currentVideoWindow;
        private StackPanel _currentVideoWindowStackPanel;
        private WindowsFormsHost _toolMenuPanel;
        public Menu ToolMenu;

        public void CreateVideoWindow(VideoWindow videoWindow,Double x,Double y,CameraAttributes camera,Boolean isAppendToolMenu)
        {
            foreach (var child in _canvas.Children)
            {
                var checkPanel = child as StackPanel;
                if (checkPanel == null) continue;

                var checkWin = checkPanel.Children[0] as WindowsFormsHost;
                if (checkWin == null) continue;

                var checkValue = checkWin.DataContext as VideoWindow;
                if (checkValue == null) continue;

                if (checkValue == videoWindow) return;
            }
            var winformHost = new WindowsFormsHost
                                  {
                                      Child = videoWindow,
                                      Width = videoWindow.Width,
                                      Height = videoWindow.Height,
                                      DataContext = camera,
                                      Background = Brushes.Transparent,
                                      OpacityMask = Brushes.Transparent,
                                  };
            var panel = new StackPanel 
            {
                Width = winformHost.Width,
                Height = winformHost.Height
            };
            panel.Children.Add(winformHost);

            var newPoint = CaculateVideoWindowPositionToCanvas(x, y);

            var dic = new Dictionary<String, Point> { { "Original", new Point(x, y) }, { "Current", newPoint } };
            panel.DataContext = dic;
            panel.Name = String.Format("C{0}", camera.Id);

            Canvas.SetTop(panel, newPoint.Y);
            Canvas.SetLeft(panel, newPoint.X);

            _canvas.Children.Add(panel);

            if (!isAppendToolMenu) return;

            _currentVideoWindowStackPanel = panel;
            _currentVideoWindow = videoWindow;
            AppendToolMenu(panel);
        }

        public void AppendToolMenu(StackPanel panel)
        {
            if (panel == null) return;
            RemoveToolMenu();
            _toolMenuPanel.Child = ToolMenu;
            Canvas.SetTop(_toolMenuPanel, Canvas.GetTop(panel));
            Canvas.SetLeft(_toolMenuPanel, Canvas.GetLeft(panel) + panel.Width);
            //if (ToolMenu.Height + Canvas.GetTop(panel) > Height)
            //{
            //    Canvas.SetTop(_toolMenuPanel, Canvas.GetTop(panel) - (ToolMenu.Height - panel.Height));
            //}
            //else
            //{
            //    Canvas.SetTop(_toolMenuPanel, Canvas.GetTop(panel));
            //}
            _canvas.Children.Add(_toolMenuPanel);
        }

        public void RemoveToolMenu()
        {
            if(_canvas.Children.Contains(_toolMenuPanel))
                _canvas.Children.Remove(_toolMenuPanel);
        }

        private Point CaculateVideoWindowPositionToCanvas(Double x,Double y)
        {
            var currentScale = Scale / 10;

            x += MainCanvasPoint.X;
            y += MainCanvasPoint.Y;

            var centerX = (CurrentMapSize.Width / 2) + MainCanvasPoint.X;
            var centerY = (CurrentMapSize.Height / 2) + MainCanvasPoint.Y;

            var diffX = (x - centerX);
            var diffY = (y - centerY);

            diffX *= currentScale - 1;
            diffY *= currentScale - 1;

            x += diffX;
            y += diffY;

            return new Point(x,y);
        }

        public void RemoveVideoWindow(VideoWindow videoWindow)
        {
            var children = _canvas.Children;
            foreach (UIElement element in children)
            {
                var ele = element as StackPanel;
                if (ele != null)
                {
                    var win = ele.Children[0] as WindowsFormsHost;
                    if (win != null)
                    {
                        var wwPanel = win.Child as VideoWindow;
                        if (videoWindow == wwPanel)
                        {
                            _canvas.Children.Remove(element);
                            _currentVideoWindowStackPanel = null;
                            if (_currentVideoWindow == videoWindow) _currentVideoWindow = null;
                            RemoveToolMenu();
                            if (videoWindow != null) RemoveEventListPanelByCameraId(ele.Name.Replace("C", String.Empty));
                            return;
                        }
                    }
                }
            }
        }

        public void UpdateVideoWindow(VideoWindow videoWindow)
        {
            var children = _canvas.Children;
            foreach (UIElement element in children)
            {
                var ele = element as StackPanel;
                if (ele == null) continue;

                var win = ele.Children[0] as WindowsFormsHost;
                if (win == null) continue;

                var vVideoWindow = win.Child as VideoWindow;
                if (videoWindow != null && videoWindow == vVideoWindow)
                {
                    ele.Width = videoWindow.Width;
                    ele.Height = videoWindow.Height;
                    win.Width = videoWindow.Width;
                    win.Height = videoWindow.Height;
                    win.Child = videoWindow;
                    if (_currentVideoWindow == videoWindow)
                    {
                        AppendToolMenu(ele);
                        // _currentVideoWindowPanel = null;
                    }
                    return;
                }
            }
        }

        public void RemoveAllVideoWindow()
        {
            var children = _canvas.Children;
            var list = children.OfType<StackPanel>().ToList();

            foreach (StackPanel element in list)
            {
                _canvas.Children.Remove(element);
            }
        }

        public Boolean ClickVideoWindowOnMap(VideoWindow videoWindow,CameraAttributes camera)
        {
            if(_currentVideoWindow == videoWindow)
            {
                if (!_canvas.Children.Contains(_toolMenuPanel) && _currentVideoWindowStackPanel != null)
                {
                    AppendToolMenu(_currentVideoWindowStackPanel);
                }
                else if (_canvas.Children.Contains(_toolMenuPanel))
                {
                    AppendToolMenu(_currentVideoWindowStackPanel);
                }
                return false;
            }

            var children = _canvas.Children;
            
            //_currentVideoWindow = videoWindow;

            StackPanel clickTarget;

            foreach (UIElement element in children)
            {
                var ele = element as StackPanel;
                if (ele != null)
                {

                    var win = ele.Children[0] as WindowsFormsHost;
                    if (win != null)
                    {
                        var vVideoWindow = win.Child as VideoWindow;

                        if (vVideoWindow== videoWindow)
                        {
                            clickTarget = ele;
                            var p = clickTarget.DataContext as Dictionary<String, Point>;
                            _canvas.Children.Remove(ele);
                            if(p != null)
                            {
                                //CreateVideoWindow(wPanel, p["Original"].X,p["Original"].Y, camera,true);
                                _canvas.Children.Add(clickTarget);
                                _currentVideoWindow = videoWindow;
                                _currentVideoWindowStackPanel = ele;
                                AppendToolMenu(_currentVideoWindowStackPanel);
                            }
                            break;
                        }

                    }
                }
            }

            //ClickVideoWindowOnMap(videoWindow, camera);
            return true;
        }

        private ProgressBar _progbar;

        public void MakeMaskforLoading()
        {
           
            _progbar = new ProgressBar
                          {
                              IsIndeterminate = false,
                              Orientation = Orientation.Horizontal,
                              Width = 150,
                              Height = 15
                          };
            var duration = new Duration(TimeSpan.FromSeconds(10));
            var doubleanimation = new DoubleAnimation(100.0, duration);
            _progbar.BeginAnimation(RangeBase.ValueProperty, doubleanimation);
            System.Windows.Controls.Panel.SetZIndex(_progbar, 999);
            Canvas.SetTop(_progbar,Height/2);
            Canvas.SetLeft(_progbar,Width/2-_progbar.Width/2);
            _canvas.Children.Add(_progbar);

        }

        public void RemoveMaskForLoading()
        {
            _canvas.Children.Remove(_progbar);
        }

        private BitmapSource MakeBitmapSourceByHttp(String id,String fileName, ICMS cms)
        {
            if (Maps.ContainsKey(id))
            {
                return Maps[id];
            }
            App.ApplicationForms.ShowLoadingIcon(Server.Form);

            Bitmap map = cms.NVRManager.GetMap(fileName);
            if(map == null)
            {
                TopMostMessageBox.Show(Localization["EMap_MessageBoxReadImageFailed"], Localization["MessageBox_Information"]);
                App.ApplicationForms.HideLoadingIcon();
                return null;
            }
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(map.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            Maps.Add(id, bitmapSource);

            App.ApplicationForms.HideLoadingIcon();
            return bitmapSource;
        }

        //private static Bitmap ConvertBitmap(BitmapSource src)
        //{
        //    int width = src.PixelWidth;
        //    int height = src.PixelHeight;
        //    int stride = width * ((src.Format.BitsPerPixel + 7) / 8);

        //    var bits = new byte[height * stride];

        //    var strm = new MemoryStream();
        //    BitmapEncoder encoder = new BmpBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(src));
        //    encoder.Save(strm);
        //    return new System.Drawing.Bitmap(strm);
        //}

        private static readonly CultureInfo _enus = new CultureInfo("en-US");
        private static BitmapSource MakeBitmapSourceByFileType(String path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                Stream imageStreamSource = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read); //new Uri(@path
                switch (file.Extension.ToUpper(_enus))
                {
                    case ".PNG":
                        var decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapsource = decoder.Frames[0];
                       // imageStreamSource.Dispose();
                        return bitmapsource;

                    case ".JPG":
                        var decoder2 = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapsource2 = decoder2.Frames[0];
                        //imageStreamSource.Dispose();
                        return bitmapsource2;

                    case ".BMP":
                        var decoder3 = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapsource3 = decoder3.Frames[0];
                        //imageStreamSource.Dispose();
                        return bitmapsource3;

                    case ".GIF":
                        var decoder4 = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapsource4 = decoder4.Frames[0];
                       // imageStreamSource.Dispose();
                        return bitmapsource4;
                }

            }
            else
            {
                return null;
            }
            return null;
        }


        public Size CurrentMapSize;

        public Image CreateImageNode(String type, bool isDrag, BitmapSource image, Int32 width, Int32 height, Double x, Double y, Int32 rotate, Object item, String tooltipString)
        {
            var myImage = new Image();
            try
            {
                // Create the TransformedBitmap to use as the Image source.
                var tb = new TransformedBitmap();
                tb.BeginInit();

                if (type == "Map")
                {
                    var mapItem = item as MapAttribute;
                    if (mapItem != null)
                    {
                        tb.Source = MakeBitmapSourceByHttp(mapItem.Id, mapItem.SystemFile, CMS);
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else
                {
                    //tb.Source = MakeBitmapSourceByFileType(path);
                    tb.Source = image;
                }

                //tb.Source = type == "Map" ? MakeBitmapSourceByHttp(path, CMS) : MakeBitmapSourceByFileType(path);
                
                // Set image rotation.
                var transform = new RotateTransform(0) { CenterX = x + (myImage.Width) / 2 };
                tb.Transform = transform;
                tb.EndInit();

                //set image source
                myImage.Source = tb;
                tb.Source = null;

                myImage.DataContext = item;
                myImage.Name = type;
                if(String.IsNullOrEmpty(tooltipString) == false)
                {
                    myImage.ToolTip = new ToolTip {Content =tooltipString};
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
      
            if (isDrag)
            {
                var newSize = CalculateImageSize(width, height);
                myImage.Width = newSize.Width/(Scale/10) ;
                myImage.Height = newSize.Height/(Scale/10); 
                myImage.MouseLeftButtonDown += DragImage;
                //myImage.MouseRightButtonDown += ItemMouseRightButtonDown;
                myImage.MouseDown += NodeMouseDown;
                Canvas.SetTop(myImage,( y-myImage.Height/2) );
                Canvas.SetLeft(myImage, (x - myImage.Width / 2) );
                if(type!="Speaker" && type !="Audio")
                {
                    myImage.Cursor = Mode == "View" ? Cursors.Hand : Cursors.ScrollAll;
                }
            }
            else
            {
                var newSize = CalculateImageSize(width, height);
                MainCanvas.Width = myImage.Width = newSize.Width;
                MainCanvas.Height = myImage.Height = newSize.Height;

                CurrentMapSize = newSize;
                if(type == "Map")
                {
                    Canvas.SetTop(myImage, y);
                    Canvas.SetLeft(myImage, x);
                }
                else
                {
                    Canvas.SetTop(myImage, (y - myImage.Height / 2));
                    Canvas.SetLeft(myImage, (x - myImage.Width / 2));
                }
                
            }
            return myImage;
        }

        private void CreateNameLabel(String id,String type, String name, Double x,Double y )
        {
       
            var node = new TextBlock
                           {
                               Text = name,
                               TextWrapping = TextWrapping.WrapWithOverflow
                           };
         
            var desc = new CameraDescritpion
                           {
                               Id = id,
                               Type = type,
                               DescX = x,
                               DescY = y
                           };

            node.Foreground = Brushes.White;

            switch (type)
            {
                case "Camera":
                    node.Background = Brushes.SteelBlue;
                    break;
                case "Via":
                    node.Background = Brushes.DarkGreen;
                    break;
                case "NVR":
                    node.Background = Brushes.DimGray;
                    break;
                case "HotZone":
                    node.Foreground = Brushes.Black;
                    node.MouseEnter += HotZoneLabelMouseEnter;
                    node.MouseLeave += HotZoneLabelMouseLeave;
                    break;
                case "Map":
                    node.Background = Brushes.SlateGray;
                    break;
            }

            node.FontFamily = new FontFamily("Arial");
            node.FontSize = Convert.ToInt32(10 / (Scale / 10));
            node.FontStretch = FontStretches.UltraExpanded;
            node.FontStyle = FontStyles.Normal;
            node.FontWeight = FontWeights.Regular;
            //node.LineHeight = Double.NaN;
            node.Padding = new Thickness(2, 2, 2, 2);
            node.TextAlignment = TextAlignment.Center;
      
            node.TextWrapping = TextWrapping.Wrap;
            node.Typography.NumeralStyle = FontNumeralStyle.Normal;
            node.Typography.SlashedZero = true;
            node.DataContext = desc;
            node.MouseLeftButtonDown += DragNameLabel;
            node.MouseDown += NameNodeMouseDown;
            node.Width = 120/(Scale/10) ;
      
            node.MinHeight = 16/(Scale/10);
            node.Cursor = Mode == "View" ? Cursors.Hand : Cursors.ScrollAll;
            Canvas.SetTop(node, (desc.DescY - node.MinHeight / 2));
            Canvas.SetLeft(node, (desc.DescX - node.Width / 2));
            MainCanvas.Children.Add(node);
        }

        public void ChangeAllItemsCursor()
        {
            var cursor = Cursors.ScrollAll;
            if (Mode == "View")
            {
                cursor = Cursors.Hand;
            }

            if (MainCanvas == null)
            {
                return;
            }

             var nodes = MainCanvas.Children;
             foreach (UIElement elem in nodes)
             {
                 var type = elem.GetType().Name;
                 if (type == "Image")
                 {
                     var elementImg = elem as Image;
                     if (elementImg != null)
                     {
                         if (elementImg.Name == "Camera" || elementImg.Name == "Via" || elementImg.Name == "NVR" || elementImg.Name == "Alarm")
                         {
                             elementImg.Cursor = cursor;
                         }
                     }
                 }
                 else if (type == "TextBlock")
                 {
                     var elementText = elem as TextBlock;
                     if (elementText != null)
                     {
                         elementText.Cursor = cursor;
                     }
                 }
                 else if (type == "Polygon")
                 {

                     var elementPolygon = elem as Polygon;
                     if (elementPolygon != null)
                     {
                         elementPolygon.Cursor = Mode == "Setup" ? Cursors.Arrow : Cursors.Hand;
                     }
                 }
             }
        }

        private static Size CalculateImageSize(Double width, Double height)
        {
            var returnSize = new Size
                                 {
                                     Width = width,
                                     Height = height
                                 };

            if (width <= Int32.Parse(Properties.Resources.PadWidth) && height <= Int32.Parse(Properties.Resources.PadHeight))
            {
                returnSize.Width = width;
                returnSize.Height = height;
            }
            else
            {
                if ( width > Int32.Parse(Properties.Resources.PadWidth))
                {
                    returnSize.Width = Int32.Parse(Properties.Resources.PadWidth);
                    returnSize.Height = height*(Double.Parse(Properties.Resources.PadWidth)/ Double.Parse(width.ToString()) );

                    if(returnSize.Height > Double.Parse(Properties.Resources.PadHeight))
                    {
                        returnSize = CalculateImageSize(returnSize.Width, returnSize.Height);
                    }
                }
                else
                {
                    if (height > Int32.Parse(Properties.Resources.PadHeight))
                    {
                        returnSize.Height = Int32.Parse(Properties.Resources.PadHeight);
                        returnSize.Width = width * (Double.Parse(Properties.Resources.PadHeight) / Double.Parse(height.ToString()));

                        if (returnSize.Width > Double.Parse(Properties.Resources.PadWidth))
                        {
                            returnSize = CalculateImageSize(returnSize.Width, returnSize.Height);
                        }
                    }
                }
        }
            return returnSize;
        }

        private Point CurrentNodePoint { get; set; }
        private Point CurrentMainCanvasPoint { get; set; }

        private void DragNameLabel(object sender, MouseButtonEventArgs e)
        {
            var node = e.Source as TextBlock;

            if (node != null)
            {
                CurrentNodePoint = e.GetPosition(MainCanvas);

                var data = new DataObject(typeof(TextBlock), node);

                DragDrop.DoDragDrop(node, data, Mode == "Setup" ? DragDropEffects.Move : DragDropEffects.None);
            }
        }

        private void MainCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cvs = sender as Canvas;

            if (cvs != null)
            {
                CurrentMainCanvasPoint = e.GetPosition(_canvas);

                var data = new DataObject(typeof(Canvas), cvs);

                HitTestResult results = VisualTreeHelper.HitTest(_canvas, CurrentMainCanvasPoint);
                if (results!=null)
                {
                    var nodeType = results.VisualHit.GetType();
                    if (nodeType.Name == "Image")
                    {
                        var node = results.VisualHit as Image;
                        if (node != null)
                        {
                            if (node.Name == "Map")
                            {
                                _canvas.Cursor = Cursors.Hand;
                                DragDrop.DoDragDrop(cvs, data, DragDropEffects.All);
                                
                                return;
                            }
                        }
                    }
                }

                DragDrop.DoDragDrop(cvs, data, DragDropEffects.None);
            }
        }

        public Boolean HitTestNodeOnMap(Double x,Double y)
        {
            var point = new Point(x, y);
            HitTestResult results = VisualTreeHelper.HitTest(_canvas, point);
            if (results != null)
            {
                var nodeType = results.VisualHit.GetType();
                if (nodeType.Name == "Image")
                {
                    var node = results.VisualHit as Image;
                    if (node != null)
                    {
                        if (node.Name == "Map")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
  
        public void PadMove(String director,Int32 speed)
        {
            //MainCanvas
        }

        private void PadControlDrop(object sender, DragEventArgs e)
        {
            var node = e.Data.GetData(typeof(Canvas)) as Canvas;
            if (node == null)
            {
                return;
            }

            MoveMainCanvasWindow(true, e);
        }

        private void CanvasDragOver(object sender, DragEventArgs e)
        {
            var node = e.Data.GetData(typeof(Canvas)) as Canvas;
            if (node == null)
            {
                return;
            }
            MoveMainCanvasWindow(false, e);
        }

        private void MainCanvasDragOver(object sender, DragEventArgs e)
        {
            if (Mode == "View")
            {
                return;
            }

            var node = e.Data.GetData(typeof(Image)) as Image;
            var nodeText = e.Data.GetData(typeof(TextBlock)) as TextBlock;
            if (node == null && nodeText == null)
            {
                return;
            }
            else
            {
                if (node != null)
                {
                    if (node.Name == "Speaker" || node.Name == "Audio" || node.Name == "Alarm")
                    {
                        return;
                    }

                    var type = node.DataContext.GetType();

                    if (type.Name == "CameraAttributes")
                    {
                        Canvas.SetTop(node, e.GetPosition(MainCanvas).Y - node.Height / 2);
                        Canvas.SetLeft(node, e.GetPosition(MainCanvas).X - node.Width / 2);

                        var source = node.DataContext as CameraAttributes;

                        if (source != null)
                        {
                            var diffX = e.GetPosition(MainCanvas).X - source.X;
                            var diffY = e.GetPosition(MainCanvas).Y - source.Y;

                            source.X = e.GetPosition(MainCanvas).X;
                            source.Y = e.GetPosition(MainCanvas).Y;

                            source.VideoWindowX += diffX;
                            source.VideoWindowY += diffY;

                            //reset description , speaker and audio posision
                            var nodes = MainCanvas.Children;
                            foreach (UIElement elem in nodes)
                            {
                                var elemType = elem.GetType();
                                if (elemType.Name == "TextBlock")
                                {
                                    var element = elem as TextBlock;
                                    if (element != null)
                                    {
                                        var lblSource = element.DataContext as CameraDescritpion;
                                        if (lblSource != null)
                                        {
                                            if (lblSource.Id == source.Id && lblSource.Type == "Camera")
                                            {
                                                var newY = lblSource.DescY + diffY;
                                                var newX = lblSource.DescX + diffX;

                                                Canvas.SetTop(element, newY - (element.MinHeight / 2));
                                                Canvas.SetLeft(element, newX - (element.Width / 2));
                                                lblSource.DescY = newY;
                                                lblSource.DescX = newX;

                                                source.DescX = newX;
                                                source.DescY = newY;
                                            }
                                        }
                                    }
                                }
                                else if (elemType.Name == "Image")
                                {
                                    var elementImg = elem as Image;
                                    if (elementImg != null)
                                    {
                                        var item = elementImg.DataContext as CameraAroundItem;
                                        if (item != null)
                                        {
                                            if (item.CameraId == source.Id)
                                            {
                                                var newY = item.Y + diffY;
                                                var newX = item.X + diffX;
                                                Canvas.SetTop(elementImg, newY - (elementImg.Height / 2));
                                                Canvas.SetLeft(elementImg, newX - (elementImg.Width / 2));
                                                item.Y = newY;
                                                item.X = newX;

                                                if (item.Type == "Speaker")
                                                {
                                                    source.SpeakerX = newX;
                                                    source.SpeakerY = newY;
                                                }
                                                else // ==audio
                                                {
                                                    source.AudioX = newX;
                                                    source.AudioY = newY;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (elemType.Name == "StackPanel")
                                {
                                    var elementPanel = elem as StackPanel;
                                    if (elementPanel != null)
                                    {
                                        if (elementPanel.Name.Replace("C", "") == source.Id)
                                        {
                                            var p = elementPanel.DataContext as Dictionary<String, Point>;
                                            if (p != null)
                                            {

                                                var newX = p["Original"].X + diffX;
                                                var newY = p["Original"].Y + diffY;

                                                var newCurrentX = p["Current"].X + diffX;
                                                var newCurrentY = p["Current"].Y + diffY;

                                                var dic = new Dictionary<String, Point>
                                                              {
                                                                  {"Original", new Point(newX, newY)},
                                                                  {"Current", new Point(newCurrentX, newCurrentY)}
                                                              };
                                                elementPanel.DataContext = dic;

                                                source.VideoWindowX = newX;
                                                source.VideoWindowY = newY;

                                                Canvas.SetTop(elementPanel, newCurrentY);
                                                Canvas.SetLeft(elementPanel, newCurrentX);
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (var child in _canvas.Children)
                            {
                                var elementPanel = child as StackPanel;
                                if (elementPanel != null)
                                {
                                    if (elementPanel.Name.Replace("C", "") == source.Id)
                                    {
                                        var p = elementPanel.DataContext as Dictionary<String, Point>;
                                        if (p != null)
                                        {
                                            var newX = p["Original"].X + diffX;
                                            var newY = p["Original"].Y + diffY;

                                            var newPoint = CaculateVideoWindowPositionToCanvas(newX, newY);

                                            var newCurrentX = newPoint.X;
                                            var newCurrentY = newPoint.Y;

                                            var dic = new Dictionary<String, Point>
                                                          {
                                                              {"Original", new Point(newX, newY)},
                                                              {"Current", new Point(newCurrentX, newCurrentY)}
                                                          };
                                            elementPanel.DataContext = dic;

                                            source.VideoWindowX = newX;
                                            source.VideoWindowY = newY;

                                            Canvas.SetTop(elementPanel, newCurrentY);
                                            Canvas.SetLeft(elementPanel, newCurrentX);

                                            if (_currentVideoWindowStackPanel == elementPanel && _canvas.Children.Contains(_toolMenuPanel))
                                            {
                                                Canvas.SetTop(_toolMenuPanel, newCurrentY);
                                                Canvas.SetLeft(_toolMenuPanel, newCurrentX + elementPanel.Width);
                                            }
                                        }
                                    }
                                }
                            }

                            if (OnMoveCamera != null)
                            {
                                OnMoveCamera(this, new EventArgs<CameraAttributes>(source));
                                return;
                            }
                        }
                    }
                    else
                    {
                        
                        DropNodeOnCanvas(null, e);
                        CurrentMainCanvasPoint = e.GetPosition(_canvas);
                    }

                }
                else
                {
                    DropNodeOnCanvas(null, e);
                    CurrentMainCanvasPoint = e.GetPosition(_canvas);
                }
            }

            return;
        }

        private void MoveVideoWindow(Boolean isDrop, Double diffX, Double diffY)
        {
            var count = 0;

            foreach (var child in _canvas.Children)
            {
                var panel = child as StackPanel;
                if (panel != null)
                {
                    var p = panel.DataContext as Dictionary<String, Point>;
                    if (p != null)
                    {
                        var panelX = p["Current"].X;
                        var panelY = p["Current"].Y;

                        var newX = panelX + diffX;
                        var newY = panelY + diffY;
               
                        Canvas.SetTop(panel,newY );
                        Canvas.SetLeft(panel, newX);

                        if (panel.Name == _eventListPanel.Name)
                        {
                            if (_canvas.Children.Contains(_eventListPanel))
                            {
                                Canvas.SetTop(_eventListPanel, newY);
                                Canvas.SetLeft(_eventListPanel, newX - _eventListPanel.Width);
                            }
                        }

                        count++;
                        if (isDrop)
                        {
                            panel.Visibility = Visibility.Visible;
                            var dic = new Dictionary<String, Point>
                                          {
                                              {"Original", p["Original"]},
                                              {"Current", new Point(newX, newY)}
                                          };
                            panel.DataContext = dic;

                            _startMoveVideoWindow = false;
                        }
                    }
                }
            }

            if (_canvas.Children.Contains(_toolMenuPanel) && count > 0)
            {
                RemoveToolMenu();
            }

            if (isDrop && count>0)
            {
                AppendToolMenu(_currentVideoWindowStackPanel);
            }
        }

        private void ScaleVideoWindow()
        {
            foreach (var child in _canvas.Children)
            {
                var panel = child as StackPanel;
                if (panel != null)
                {
                    var p = panel.DataContext as Dictionary<String, Point>;
                    if (p != null)
                    {
                        var x = p["Original"].X;
                        var y = p["Original"].Y;

                        var newPoint = CaculateVideoWindowPositionToCanvas(x, y);

                        var dic = new Dictionary<String, Point> { { "Original", p["Original"] }, { "Current", newPoint } };
                        panel.DataContext = dic;

                        Canvas.SetTop(panel, newPoint.Y);
                        Canvas.SetLeft(panel, newPoint.X);

                        if (panel.Name == _eventListPanel.Name)
                        {
                            if (_canvas.Children.Contains(_eventListPanel))
                            {
                                Canvas.SetTop(_eventListPanel, newPoint.Y);
                                Canvas.SetLeft(_eventListPanel, newPoint.X - _eventListPanel.Width);
                            }
                        }

                    }
                }
            }

            if (_currentVideoWindowStackPanel!= null && _canvas.Children.Contains(_toolMenuPanel))
            {
                var p = _currentVideoWindowStackPanel.DataContext as Dictionary<String, Point>;
                if (p != null)
                {
                    Canvas.SetTop(_toolMenuPanel, p["Current"].Y);
                    Canvas.SetLeft(_toolMenuPanel, p["Current"].X + _currentVideoWindowStackPanel.Width);
                }
            }

        }

        private void ShowHideAllVideoWindow(String type)
        {
            foreach (var child in _canvas.Children)
            {
                var panel = child as StackPanel;
                if (panel != null)
                {
                    if(type == "Show")
                    {
                        if (panel.Visibility == Visibility.Hidden)
                        {
                            panel.Visibility = Visibility.Visible;
                        }
                    }
                    else if (type == "Hide")
                    {
                        if (panel.Visibility == Visibility.Visible)
                        {
                            panel.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }

            if(_canvas.Children.Contains(_toolMenuPanel))
            {
                //_toolMenuPanel.Visibility = type == "Show"? Visibility.Visible : Visibility.Hidden;
                RemoveToolMenu();
            }

        }

        private void MoveMainCanvasWindow(Boolean isDrop, DragEventArgs e)
        {
            //var isNodeMove = false;

            //if (Math.Abs(e.GetPosition(_canvas).X - CurrentMainCanvasPoint.X) > 2 && Math.Abs(e.GetPosition(_canvas).Y - CurrentMainCanvasPoint.Y) > 2)
            //{
                //isNodeMove = true;
            //}
 
            var cvs = e.Data.GetData(typeof(Canvas)) as Canvas;

            if (cvs != null)
            {
                if (_startMoveVideoWindow != true)
                {
                    //ShowHideAllVideoWindow("Hide");
                }

                var diffX = (e.GetPosition(_canvas).X - CurrentMainCanvasPoint.X);
                var diffY = (e.GetPosition(_canvas).Y - CurrentMainCanvasPoint.Y);

                var newY = MainCanvasPoint.Y + diffY;
                var newX = MainCanvasPoint.X + diffX;

                Canvas.SetTop(cvs, newY);
                Canvas.SetLeft(cvs, newX);

                MoveVideoWindow(isDrop, diffX, diffY);
                _startMoveVideoWindow = true;
                if (isDrop)
                {
                    //MoveVideoWindow(true, diffX, diffY);
                    MainCanvasPoint.Y = newY;
                    MainCanvasPoint.X = newX;
                    
                    _canvas.Cursor = Cursors.Arrow;
                    if (OnMoveMap!=null)
                    {
                        OnMoveMap(this,new EventArgs<Point>(new Point(MainCanvasPoint.X,MainCanvasPoint.Y)));
                    }
                }
            }
        }

        private Storyboard _moveStoryboard;
        private Boolean _lockMapAnimation;

        public void MoveMainCanvasToOriginal()
        {
            if (_lockMapAnimation)
            {
                return;
            }

            _lockMapAnimation = true;
            CurrentMainCanvasPoint = MainCanvasPoint;
            MapMoveAnimation("Back", MainCanvasPoint, new Point(0, 0));
            MainCanvasPoint.X = MainCanvasPoint.Y = 0;
            ShowHideAllVideoWindow("Hide");
            if (OnMoveMap != null)
            {
                OnMoveMap(this, new EventArgs<Point>(new Point(MainCanvasPoint.X, MainCanvasPoint.Y)));
            }
        }

        public void MoveMainCanvasByDistant(String type, Double distance)
        {
            if (_lockMapAnimation)
            {
                return;
            }

            _lockMapAnimation = true;
            CurrentMainCanvasPoint = MainCanvasPoint;
            var newX = MainCanvasPoint.X;
            var newY = MainCanvasPoint.Y;

            switch (type)
            {
                case "Up":
                    newY += distance;
                    
                    break;

                case "Down":
                    newY -= distance;
                    break;

                case "Left":
                    newX += distance;
                    break;

                case "Right":
                    newX -= distance;
                    break;

            }
            
            MapMoveAnimation(type, MainCanvasPoint,new Point(newX,newY));
            MainCanvasPoint.Y = newY;
            MainCanvasPoint.X = newX;
            ShowHideAllVideoWindow("Hide");
        }

        private void MapMoveAnimation(String type, Point oldPoint,Point newPoint)
        {
            _moveStoryboard = new Storyboard();
            _moveStoryboard.Completed += MapMoveComplete;
            _moveStoryboard.FillBehavior = FillBehavior.Stop;

            if (type == "Up" || type == "Down" || type=="Back")
            {
                var moveTopAnimation = new DoubleAnimation(oldPoint.Y, newPoint.Y, TimeSpan.FromSeconds(0.3), FillBehavior.Stop);
                Storyboard.SetTargetProperty(moveTopAnimation, new PropertyPath("(Canvas.Top)"));
                _moveStoryboard.Children.Add(moveTopAnimation);

            }

            if (type == "Left" || type == "Right" || type == "Back")
            {
                var moveLeftAnimation = new DoubleAnimation(oldPoint.X, newPoint.X, TimeSpan.FromSeconds(0.3), FillBehavior.Stop);
                Storyboard.SetTargetProperty(moveLeftAnimation, new PropertyPath("(Canvas.Left)"));
                _moveStoryboard.Children.Add(moveLeftAnimation);

            }

            _moveStoryboard.Begin(MainCanvas);
        }

        private void MapMoveComplete(object sender, EventArgs e)
        {
            _moveStoryboard.BeginAnimation(Canvas.TopProperty, null, HandoffBehavior.Compose);
            _moveStoryboard.BeginAnimation(Canvas.LeftProperty, null, HandoffBehavior.Compose);

            var diffX = MainCanvasPoint.X - CurrentMainCanvasPoint.X ;
            var diffY = MainCanvasPoint.Y - CurrentMainCanvasPoint.Y ;
            MoveVideoWindow(true, diffX, diffY);

            Canvas.SetTop(MainCanvas, MainCanvasPoint.Y);
            Canvas.SetLeft(MainCanvas, MainCanvasPoint.X);

            if (OnMoveMap != null)
            {
                OnMoveMap(this, new EventArgs<Point>(new Point(MainCanvasPoint.X, MainCanvasPoint.Y)));
            }
            _moveStoryboard = null;
            _lockMapAnimation = false;
        }

        public Point CurrentVideoWindowPoint;
        public StackPanel CurrentMoveStackPanel;

        public void DragVideoWindow(String id,Double x,Double y)
        {
            var newPoint = CaculateVideoWindowPositionToCanvas(x, y);
            CurrentVideoWindowPoint = new Point(newPoint.X, newPoint.Y);
            foreach (var child in _canvas.Children)
            {
                var elementPanel = child as StackPanel;
                if (elementPanel != null)
                {
                    if (elementPanel.Name.Replace("C", "") == id)
                    {
                        CurrentMoveStackPanel = elementPanel;
                    }
                }
            }
            if (_toolMenuPanel != null)
            {
                _toolMenuPanel.Visibility = Visibility.Hidden;
            }
            if(_canvas.Children.Contains(_eventListPanel))
            {
                _eventListPanel.Visibility = Visibility.Hidden;
            }

            _canvas.CaptureMouse();
            _canvas.MouseMove += CanvasMouseMoveForVideoWindow;
            _canvas.MouseUp += CanvasMouseUpForVideoWindow;
        }

        private void CanvasMouseUpForVideoWindow(object sender, MouseButtonEventArgs e)
        {
            if (CurrentMoveStackPanel != null)
            {
                var p = CurrentMoveStackPanel.DataContext as Dictionary<String, Point>;
                if (p != null)
                {
                    if (OnMoveVideoWindow != null)
                    {
                        var camId = CurrentMoveStackPanel.Name.Replace("C", "");
                        OnMoveVideoWindow(this, new EventArgs<String,Double, Double>(camId,p["Original"].X, p["Original"].Y));
                        if (_toolMenuPanel != null)
                        {
                            _toolMenuPanel.Visibility = Visibility.Visible;
                        }
                        if (_canvas.Children.Contains(_toolMenuPanel) && CurrentMoveStackPanel == _currentVideoWindowStackPanel)
                        {
                            AppendToolMenu(CurrentMoveStackPanel);
                        }
                        if (_canvas.Children.Contains(_eventListPanel))
                        {
                            _eventListPanel.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            CurrentMoveStackPanel = null;
            _canvas.ReleaseMouseCapture();
            _canvas.MouseMove -= CanvasMouseMoveForVideoWindow;
            _canvas.MouseUp -= CanvasMouseUpForVideoWindow;
        }

        private void CanvasMouseMoveForVideoWindow(object sender, MouseEventArgs e)
        {
            if(CurrentMoveStackPanel != null)
            {
                var currentPoint = e.GetPosition(_canvas);
                var diffX = currentPoint.X - CurrentVideoWindowPoint.X ;
                var diffY = currentPoint.Y - CurrentVideoWindowPoint.Y ;
                var p = CurrentMoveStackPanel.DataContext as Dictionary<String, Point>;
                if (p != null)
                {

                    var currentScale = Scale/10;

                    var newX = p["Original"].X + (diffX/ currentScale);
                    var newY = p["Original"].Y + (diffY/currentScale);

                    var newCurrentX = p["Current"].X + diffX;
                    var newCurrentY = p["Current"].Y + diffY;

                    var passX = true;
                    var passY = true;

                    //if (newX <= -100 || newX >= (MainCanvas.Width * currentScale))
                    //{
                    //    passX = false;
                    //    newX = p["Original"].X;
                    //    newCurrentX = p["Current"].X;
                    //}

                    //if (newY <= -100 || newY >= (MainCanvas.Height * currentScale) - 20 + 20)
                    //{
                    //    passY = false;
                    //    newY = p["Original"].Y;
                    //    newCurrentY = p["Current"].Y;
                    //}

                    //if (passX == false && passY == false)
                    //{
                    //    return;
                    //}

                    var dic = new Dictionary<String, Point>
                                  {
                                      {"Original", new Point(newX, newY)},
                                      {"Current", new Point(newCurrentX, newCurrentY)}
                                  };
                    CurrentMoveStackPanel.DataContext = dic;
                    
                    Canvas.SetTop(CurrentMoveStackPanel, newCurrentY);
                    Canvas.SetLeft(CurrentMoveStackPanel, newCurrentX);

                    if (CurrentMoveStackPanel.Name == _eventListPanel.Name)
                    {
                        if (_canvas.Children.Contains(_eventListPanel))
                        {
                            Canvas.SetTop(_eventListPanel, newCurrentY);
                            Canvas.SetLeft(_eventListPanel, newCurrentX - _eventListPanel.Width);
                        }
                    }

                }
                if (_toolMenuPanel != null)
                {
                    _toolMenuPanel.Visibility = Visibility.Hidden;
                }
                CurrentVideoWindowPoint = currentPoint;
            }
        }

        private String _currentCameraClick;

        private void DragImage(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            
            if (image != null)
            {
                CurrentNodePoint = e.GetPosition(MainCanvas);

                var data = new DataObject(typeof(Image), image);

                if (image.Name != "Audio" && image.Name != "Speaker" && image.Name != "Alarm" && Mode == "Setup")
                {
                    if (IsDrawingHotZone) return;
                    DragDrop.DoDragDrop(image, data, DragDropEffects.Move);
                }
                else
                {
                    DragDrop.DoDragDrop(image, data, DragDropEffects.None);
                }
            }
        }

        private void ItemMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image)
            {
                var cam = ((Image)sender).DataContext as CameraAttributes;

                if (cam != null)
                {
                    CameraEditTrigger(cam.Id);
                    if (OnCameraClick != null)
                    {
                        OnCameraClick(this, new EventArgs<String>(cam.Id));
                    }
                    return;
                }
            }

        }

        private void NodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mode == "Setup")
            {
                return;
            }
            var node = sender as Image;
            if (node == null) return;
            var type = node.DataContext.GetType();
            switch (type.Name)
            {
                case "CameraAttributes":
                    var cam = node.DataContext as CameraAttributes;
                    if (cam != null)
                    {
                        if (OnCameraClick != null)
                        {
                            OnCameraClick(this, new EventArgs<String>(cam.Id));
                            return;
                        }
                    }
                    break;

                case "NVRAttributes":
                    var nvr = node.DataContext as NVRAttributes;
                    if (OnNVRClick != null && nvr != null)
                    {
                        OnNVRClick(this, new EventArgs<String>(nvr.Id));
                    }
                    break;

                case "ViaAttributes":
                    var via = node.DataContext as ViaAttributes;
                    if (OnViaClick != null && via != null)
                    {
                        OnViaClick(this, new EventArgs<String>(via.Id));
                        return;
                    }
                    break;
                case "CameraAroundItem": //audio speaker
                    var item = node.DataContext as CameraAroundItem;
                    if(item != null)
                    {
                        if(item.Type == "Alarm" && OnClickAlarm != null)
                        {
                            OnClickAlarm(this,new EventArgs<String>(item.CameraId));
                        }
                    }

                    break;
               
            }
        }

        private void NameNodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mode == "Setup")
            {
                return;
            }
            var descriptionNode = sender as TextBlock;
            if (descriptionNode != null)
            {
                var desc = descriptionNode.DataContext as CameraDescritpion;
                if (desc != null)
                {
                    switch (desc.Type)
                    {
                        case "Camera":
                            OnCameraClick(this, new EventArgs<String>(desc.Id));
                            break;

                        case "Via":
                            OnViaClick(this, new EventArgs<String>(desc.Id));
                            break;

                        case "NVR":
                            OnNVRClick(this, new EventArgs<String>(desc.Id));
                            break;

                        case "HotZone":
                            OnClickHotZone(this, new EventArgs<String>(desc.Id));
                            break;

                        case "Map":
                            OnClickChangeMap(this, new EventArgs<String>(desc.Id));
                            break;
                    }
                }
            }
        }

        public void DropNodeOnCanvas(object sender, DragEventArgs e)
        {
            if (IsDrawingHotZone) return;
            var isNodeMove = false;

            if (Math.Abs(e.GetPosition(MainCanvas).X - CurrentNodePoint.X) > 2 && Math.Abs(e.GetPosition(MainCanvas).Y - CurrentNodePoint.Y) > 2 && Mode == "Setup")
            {
                isNodeMove = true;
            }

            //Drop TextBlock (description)
            var descriptionNode = e.Data.GetData(typeof(TextBlock)) as TextBlock;
            if (descriptionNode!=null)
            {
                if (isNodeMove)
                {
                    if (OnMoveDescription!=null)
                    {
                        Canvas.SetTop(descriptionNode, e.GetPosition(MainCanvas).Y - descriptionNode.MinHeight / 2);
                        Canvas.SetLeft(descriptionNode, e.GetPosition(MainCanvas).X - descriptionNode.Width / 2);

                        var desc = descriptionNode.DataContext as CameraDescritpion;

                        if (desc != null)
                        {
                            var diffX = e.GetPosition(MainCanvas).X - desc.DescX;
                            var diffY = e.GetPosition(MainCanvas).Y - desc.DescY;

                            desc.DescX = e.GetPosition(MainCanvas).X;
                            desc.DescY = e.GetPosition(MainCanvas).Y;

                            OnMoveDescription(this, new EventArgs<CameraDescritpion>(desc));
                           
                            if(desc.Type == "Camera")
                            {
                                var nodes = MainCanvas.Children;
                                foreach (UIElement elem in nodes)
                                {
                                    
                                    var elementImg = elem as Image;
                                    if (elementImg != null)
                                    {
                                        if(elementImg.Name == "Alarm") continue;
                                        var item = elementImg.DataContext as CameraAroundItem;
                                        if (item != null)
                                        {
                                            if (item.CameraId == desc.Id)
                                            {
                                                var newY = item.Y + diffY;
                                                var newX = item.X + diffX;
                                                Canvas.SetTop(elementImg, newY - (elementImg.Height / 2));
                                                Canvas.SetLeft(elementImg, newX - (elementImg.Width / 2));
                                                item.Y = newY;
                                                item.X = newX;

                                                if (OnMoveCameraAroundItem != null)
                                                {
                                                    OnMoveCameraAroundItem(this, new EventArgs<CameraAroundItem>(item));
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    var desc = descriptionNode.DataContext as CameraDescritpion;
                    if (desc != null)
                    {
                        if(desc.Type == "Camera")
                        {
                            if(Mode=="Setup")
                            {
                                CameraEditTrigger(desc.Id);
                            }
                            else
                            {
                                OnCameraClick(this, new EventArgs<String>(desc.Id));
                            }
                        }
                        else
                        {
                            if (Mode == "Setup")
                            {
                                DescriptionEditTrigger(desc.Id, desc.Type);
                            }
                            else
                            {
                                switch (desc.Type)
                                {
                                    case "Via":
                                        OnViaClick(this, new EventArgs<String>(desc.Id));
                                        break;

                                    case "NVR":
                                        OnNVRClick(this, new EventArgs<String>(desc.Id));
                                        break;

                                    case "HotZone":
                                        OnClickHotZone(this, new EventArgs<String>(desc.Id));
                                        break;
                                }
                            }
                            
                        }
                    }
                }
                return;
            }

            var node = e.Data.GetData(typeof (Image)) as Image;

            if (node != null)
            {
                if (isNodeMove)
                {
                    if (node.Name == "Speaker" || node.Name == "Audio" || node.Name == "Alarm")
                    {
                        return;
                    }

                    if (node.Name != "HotZone")
                    {
                        Canvas.SetTop(node, e.GetPosition(MainCanvas).Y - node.Height / 2);
                        Canvas.SetLeft(node, e.GetPosition(MainCanvas).X - node.Width / 2);
                    }

                    var type = node.DataContext.GetType();

                    switch (type.Name)
                    {
                        case "CameraAttributes":
                            var source = node.DataContext as CameraAttributes;

                            if (source != null)
                            {
                                var diffX = e.GetPosition(MainCanvas).X - source.X;
                                var diffY = e.GetPosition(MainCanvas).Y - source.Y;

                                source.X = e.GetPosition(MainCanvas).X;
                                source.Y = e.GetPosition(MainCanvas).Y;

                                source.VideoWindowX += diffX;
                                source.VideoWindowY += diffY;

                                //reset description , speaker and audio posision
                                var nodes = MainCanvas.Children;
                                foreach (UIElement elem in nodes)
                                {
                                    var elemType = elem.GetType();
                                    if (elemType.Name == "TextBlock")
                                    {
                                        var element = elem as TextBlock;
                                        if (element != null)
                                        {
                                            var lblSource = element.DataContext as CameraDescritpion;
                                            if (lblSource != null)
                                            {
                                                if (lblSource.Id == source.Id && lblSource.Type == "Camera")
                                                {
                                                    var newY = lblSource.DescY + diffY;
                                                    var newX = lblSource.DescX + diffX;

                                                    Canvas.SetTop(element, newY - (element.MinHeight / 2));
                                                    Canvas.SetLeft(element, newX - (element.Width / 2));
                                                    lblSource.DescY = newY;
                                                    lblSource.DescX = newX;

                                                    source.DescX = newX;
                                                    source.DescY = newY;
                                                }
                                            }
                                        }
                                    }
                                    else if (elemType.Name == "Image")
                                    {
                                        var elementImg = elem as Image;
                                        if (elementImg != null)
                                        {
                                            var item = elementImg.DataContext as CameraAroundItem;
                                            if (item != null)
                                            {
                                                if (item.CameraId == source.Id)
                                                {
                                                    var newY = item.Y + diffY;
                                                    var newX = item.X + diffX;
                                                    Canvas.SetTop(elementImg, newY - (elementImg.Height / 2));
                                                    Canvas.SetLeft(elementImg, newX - (elementImg.Width / 2));
                                                    item.Y = newY;
                                                    item.X = newX;

                                                    if (item.Type == "Speaker")
                                                    {
                                                        source.SpeakerX = newX;
                                                        source.SpeakerY = newY;
                                                    }
                                                    else // ==audio
                                                    {
                                                        source.AudioX = newX;
                                                        source.AudioY = newY;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (elemType.Name == "StackPanel")
                                    {
                                        var elementPanel = elem as StackPanel;
                                        if (elementPanel != null)
                                        {
                                            if(elementPanel.Name.Replace("C","") == source.Id)
                                            {
                                                var p = elementPanel.DataContext as Dictionary<String, Point>;
                                                if(p != null)
                                                {

                                                    var newX = p["Original"].X + diffX;
                                                    var newY = p["Original"].Y + diffY;

                                                    var newCurrentX = p["Current"].X + diffX;
                                                    var newCurrentY = p["Current"].Y + diffY;

                                                    var dic = new Dictionary<String, Point>
                                                                  {
                                                                      {"Original", new Point(newX, newY)},
                                                                      {"Current", new Point(newCurrentX, newCurrentY)}
                                                                  };
                                                    elementPanel.DataContext = dic;

                                                    source.VideoWindowX = newX;
                                                    source.VideoWindowY = newY;

                                                    Canvas.SetTop(elementPanel, newCurrentY);
                                                    Canvas.SetLeft(elementPanel, newCurrentX);
                                                }
                                            }
                                        }
                                    }
                                    
                                    //camera's description
                                    //var element = elem as TextBlock;
                                    //if (element != null)
                                    //{
                                    //    var lblSource = element.DataContext as CameraDescritpion;
                                    //    if (lblSource != null)
                                    //    {
                                    //        if (lblSource.Id == source.Id && lblSource.Type == "Camera")
                                    //        {
                                    //            var newY = lblSource.DescY + diffY;
                                    //            var newX = lblSource.DescX + diffX;

                                    //            Canvas.SetTop(element, newY - (element.MinHeight / 2));
                                    //            Canvas.SetLeft(element, newX - (element.Width / 2));
                                    //            lblSource.DescY = newY;
                                    //            lblSource.DescX = newX;

                                    //            source.DescX = newX;
                                    //            source.DescY = newY;

                                    //            //Console.WriteLine(newX + "-" + newY);
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    var elementImg = elem as Image;
                                    //    if (elementImg != null)
                                    //    {
                                    //        var item = elementImg.DataContext as CameraAroundItem;
                                    //        if (item != null)
                                    //        {
                                    //            if (item.CameraId == source.Id)
                                    //            {
                                    //                var newY = item.Y + diffY;
                                    //                var newX = item.X + diffX;
                                    //                Canvas.SetTop(elementImg, newY - (elementImg.Height / 2));
                                    //                Canvas.SetLeft(elementImg, newX - (elementImg.Width / 2));
                                    //                item.Y = newY;
                                    //                item.X = newX;

                                    //                if (item.Type == "Speaker")
                                    //                {
                                    //                    source.SpeakerX = newX;
                                    //                    source.SpeakerY = newY;
                                    //                }
                                    //                else // ==audio
                                    //                {
                                    //                    source.AudioX = newX;
                                    //                    source.AudioY = newY;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }

                                foreach (var child in _canvas.Children)
                                {
                                    var elementPanel = child as StackPanel;
                                    if (elementPanel != null)
                                    {
                                        if (elementPanel.Name.Replace("C", "") == source.Id)
                                        {
                                            var p = elementPanel.DataContext as Dictionary<String, Point>;
                                            if (p != null)
                                            {
                                                var newX = p["Original"].X + diffX;
                                                var newY = p["Original"].Y + diffY;

                                                var newPoint = CaculateVideoWindowPositionToCanvas(newX, newY);

                                                var newCurrentX = newPoint.X;
                                                var newCurrentY = newPoint.Y;

                                                var dic = new Dictionary<String, Point>
                                                              {
                                                                  {"Original", new Point(newX, newY)},
                                                                  {"Current", new Point(newCurrentX, newCurrentY)}
                                                              };
                                                elementPanel.DataContext = dic;

                                                source.VideoWindowX = newX;
                                                source.VideoWindowY = newY;

                                                Canvas.SetTop(elementPanel, newCurrentY);
                                                Canvas.SetLeft(elementPanel, newCurrentX);

                                                if (_currentVideoWindowStackPanel == elementPanel && _canvas.Children.Contains(_toolMenuPanel))
                                                {
                                                    Canvas.SetTop(_toolMenuPanel, newCurrentY);
                                                    Canvas.SetLeft(_toolMenuPanel, newCurrentX + elementPanel.Width);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (OnMoveCamera != null)
                                {
                                    OnMoveCamera(this, new EventArgs<CameraAttributes>(source));
                                    return;
                                }
                            }
                            break;
                        //case "CameraAroundItem":
                        //    //Speaker & Audio
                        //    var sourceAround = node.DataContext as CameraAroundItem;
                        //    if (sourceAround != null)
                        //    {
                        //        sourceAround.X = e.GetPosition(MainCanvas).X;
                        //        sourceAround.Y = e.GetPosition(MainCanvas).Y;

                        //        if (OnMoveCameraAroundItem != null)
                        //        {
                        //            OnMoveCameraAroundItem(this, new EventArgs<CameraAroundItem>(sourceAround));
                        //        }

                        //    }
                        //    break;
                        case "ViaAttributes":

                            var sourceVia = node.DataContext as ViaAttributes;

                            if (sourceVia != null)
                            {
                                var diffX = e.GetPosition(MainCanvas).X - sourceVia.X;
                                var diffY = e.GetPosition(MainCanvas).Y - sourceVia.Y;

                                sourceVia.X = e.GetPosition(MainCanvas).X;
                                sourceVia.Y = e.GetPosition(MainCanvas).Y;

                                //reset description , speaker and audio posision
                                var nodes = MainCanvas.Children;
                                foreach (UIElement elem in nodes)
                                {
                                    var element = elem as TextBlock;
                                    if (element != null)
                                    {
                                        var lblSource = element.DataContext as CameraDescritpion;
                                        if (lblSource != null)
                                        {
                                            if (lblSource.Id == sourceVia.Id && lblSource.Type == "Via")
                                            {
                                                var newY = lblSource.DescY + diffY;
                                                var newX = lblSource.DescX + diffX;
                                                Canvas.SetTop(element, newY - (element.MinHeight / 2));
                                                Canvas.SetLeft(element, newX - (element.Width / 2));
                                                lblSource.DescY = newY;
                                                lblSource.DescX = newX;

                                                sourceVia.DescX = newX;
                                                sourceVia.DescY = newY;
                                            }
                                        }
                                    }

                                }

                                if (OnMoveVia != null)
                                {
                                    OnMoveVia(this, new EventArgs<ViaAttributes>(sourceVia));
                                    return;
                                }
                            }
                            
                            break;

                        case "NVRAttributes":

                            var sourceNVR = node.DataContext as NVRAttributes;

                            if (sourceNVR != null)
                            {
                                var diffX = e.GetPosition(MainCanvas).X - sourceNVR.X;
                                var diffY = e.GetPosition(MainCanvas).Y - sourceNVR.Y;

                                sourceNVR.X = e.GetPosition(MainCanvas).X;
                                sourceNVR.Y = e.GetPosition(MainCanvas).Y;

                                //reset description , speaker and audio posision
                                var nodes = MainCanvas.Children;
                                foreach (UIElement elem in nodes)
                                {
                                    var element = elem as TextBlock;
                                    if (element != null)
                                    {
                                        var lblSource = element.DataContext as CameraDescritpion;
                                        if (lblSource != null)
                                        {
                                            if (lblSource.Id == sourceNVR.Id && lblSource.Type == "NVR")
                                            {
                                                var newY = lblSource.DescY + diffY;
                                                var newX = lblSource.DescX + diffX;
                                                Canvas.SetTop(element, newY - (element.MinHeight / 2));
                                                Canvas.SetLeft(element, newX - (element.Width / 2));
                                                lblSource.DescY = newY;
                                                lblSource.DescX = newX;

                                                sourceNVR.DescX = newX;
                                                sourceNVR.DescY = newY;
                                            }
                                        }
                                    }

                                }

                                if (OnMoveNVR != null)
                                {
                                    OnMoveNVR(this, new EventArgs<NVRAttributes>(sourceNVR));
                                    return;
                                }
                            }

                            break;

                        case "Int32":
                            if(node.Name == "HotZone")
                            {
                                //var sourcePoint = node.DataContext is Point ? (Point)node.DataContext : new Point();
                                var index = node.DataContext is Int32 ? (Int32)node.DataContext : -1;
                                if (index == -1)
                                {
                                    return;
                                }
                               // var index = CurrentHotZonePolygon.Points.IndexOf(sourcePoint);

                                var newP = new Point(e.GetPosition(MainCanvas).X, e.GetPosition(MainCanvas).Y);

                                CurrentHotZonePolygon.Points[index] = newP;
                                Canvas.SetTop(node, e.GetPosition(MainCanvas).Y - node.Height / 2);
                                Canvas.SetLeft(node, e.GetPosition(MainCanvas).X - node.Width / 2);

                                if (OnChangeHotZonePoint != null && sender != null)
                                {
                                    var hotzone = CurrentHotZonePolygon.DataContext as MapHotZoneAttributes;
                                    if (hotzone != null)
                                    {
                                        var newPoint = new System.Drawing.Point((int)e.GetPosition(MainCanvas).X, (int)e.GetPosition(MainCanvas).Y);
                                        hotzone.Points[index] = newPoint;
                                        OnChangeHotZonePoint(this, new EventArgs<String, Int32, System.Drawing.Point>(hotzone.Id, index, newPoint));
                                    }
                                }
                            }
                            break;
                    }

                }
                else
                {

                    var type = node.DataContext.GetType();
                    switch (type.Name)
                    {
                        case "CameraAttributes":
                            var cam = node.DataContext as CameraAttributes;
                            if (cam != null)
                            {
                                if (Mode == "View")
                                {
                                    if (OnCameraClick != null)
                                    {
                                        OnCameraClick(this, new EventArgs<String>(cam.Id));
                                    }
                                }
                                else if (Mode == "Setup")
                                {
                                    CameraEditTrigger(cam.Id);
                                    if (OnCameraClick != null && _currentCameraClick == cam.Id)
                                    {
                                        OnCameraClick(this, new EventArgs<String>(cam.Id)); 
                                    }
                                    _currentCameraClick = cam.Id;
                                }
                            }
                            
                            break;

                        case "NVRAttributes":
                            var nvr = node.DataContext as NVRAttributes;
                            if (OnNVRClick != null && nvr != null)
                            {
                                if (Mode == "View")
                                {
                                    OnNVRClick(this, new EventArgs<String>(nvr.Id));
                                }
                                else if (Mode == "Setup")
                                {
                                    DescriptionEditTrigger(nvr.Id, "NVR");
                                }
                            }
                            break;

                        case "ViaAttributes":
                            var via = node.DataContext as ViaAttributes;
                            if (OnViaClick != null && via != null)
                            {

                                if (Mode == "View")
                                {
                                    OnViaClick(this, new EventArgs<String>(via.Id));
                                }
                                else if (Mode == "Setup")
                                {
                                    DescriptionEditTrigger(via.Id, "Via");
                                }
                                return;
                            }
                            break;
                        case "CameraAroundItem": //audio speaker
                            var audioSpeaker = node.DataContext as CameraAroundItem;
                            if (audioSpeaker != null)
                            {
                                CameraEditTrigger(audioSpeaker.CameraId);
                                return;
                            }
                            break;
                    }
                }
            }
        }

        private void CameraEditTrigger(String camId)
        {
            RemoveHopZonePoints();
            if (OnCameraEditClick != null)
            {
                OnCameraEditClick(this, new EventArgs<String>(camId));
                return;
            }
        }

        private Polygon FindPolygonInMapById(String id)
        {
            if(String.IsNullOrEmpty(id)) return null;

            foreach (var child in MainCanvas.Children)
            {
                var zone = child as Polygon;
                if(zone != null)
                {
                    var hotzone = zone.DataContext as MapHotZoneAttributes;
                    if (hotzone != null)
                    {
                        if (hotzone.Id == id)
                        {
                            return zone;
                        }
                    }
                }
            }

            return null;
        }

        private void DescriptionEditTrigger(String id,String type)
        {
            RemoveHopZonePoints();
            RemoveNVRAndViaActivate();
            if(type == "HotZone")
            {
                var zone = FindPolygonInMapById(id);
                CurrentHotZonePolygon = zone;
                DrawHotZonePoints();
            }
            else
            {
                switch (type)
                {
                    case "NVR":
                        ActivateNVRImage(id, true);
                        break;

                    case "Via":
                        ActivateViaImage(id, true);
                        break;
                }
            }

            if (OnDescriptionEditClick != null)
            {
                if (type != null) OnDescriptionEditClick(this, new EventArgs<String,String>(id,type));
                return;
            }

        }

        public void ModifyCameraRotate(String id, Int32 rotate)
        {
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as CameraAttributes;
                    if (lblSource != null)
                    {
                        if (lblSource.Id == id)
                        {
                            lblSource.Rotate = rotate;

                            var imgNo = rotate / 45;

                            if (imgNo > 7)
                            {
                                imgNo = 0;
                            }

                            //var icon = String.Format("{0}/{1}.png", Properties.Resources.CameraPath, imgNo);    //Properties.Resources.IconCamera;

                            var isActivate = false;
                            String activateCamId;
                            _activateCameras.TryGetValue(_currentMapId, out activateCamId);
                            if(String.IsNullOrEmpty(activateCamId)==false)
                            {
                                if (activateCamId == id)
                                {
                                    isActivate = true;
                                }
                            }

                            var icon = isActivate ? _cameraActivateIcons[imgNo.ToString()] : _cameraIcons[imgNo.ToString()];
                            if (lblSource.CameraStatus.ToUpper() == "NOSINGAL" || lblSource.CameraStatus.ToUpper() == "WRONGACCOUNTPASSWORD")
                            {
                                icon = _cameraDisconnectIcons[imgNo.ToString()];
                            }

                            var tb = new TransformedBitmap();
                            tb.BeginInit();
                            tb.Source = icon;
                            var transform = new RotateTransform(0);
                            tb.Transform = transform;
                            tb.EndInit();

                            element.Source = tb;

                            return;
                        }
                    }
                }
            }
        }

        public void RemoveAllActivateCameraImage()
        {
            _currentCameraClick = String.Empty;
            foreach (KeyValuePair<string, string> camera in _activateCameras)
            {
                ActivateCameraImage(camera.Value, false);
            }
        }

        public void ActivateCameraImage(String id,Boolean on)
        {
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as CameraAttributes;
                    if (lblSource != null)
                    {
                        if (lblSource.Id == id)
                        {
                            var rotate = lblSource.Rotate;

                            //remove this map activate acmera
                            var imgNo = rotate / 45;

                            if (imgNo > 7)
                            {
                                imgNo = 0;
                            }

                            if(on)
                            {
                                RemoveNVRAndViaActivate();
                                String activateCamId;
                                _activateCameras.TryGetValue(_currentMapId, out activateCamId);
                                if (String.IsNullOrEmpty(activateCamId) == false)
                                {
                                    ActivateCameraImage(activateCamId, false);
                                }
                                _activateCameras = new Dictionary<string, string> {{_currentMapId, id}};
                            }
                            else
                            {
                                _activateCameras = new Dictionary<string, string>();
                            }

                            //var icon = on ? String.Format("{0}/{1}_activate.png", Properties.Resources.CameraPath, imgNo) : String.Format("{0}/{1}.png", Properties.Resources.CameraPath, imgNo);    //Properties.Resources.IconCamera;
                            var icon = on ? _cameraActivateIcons[imgNo.ToString()] : _cameraIcons[imgNo.ToString()];    //Properties.Resources.IconCamera;

                            if(!on)
                            {
                                if (lblSource.CameraStatus.ToUpper() == "NOSINGAL" || lblSource.CameraStatus.ToUpper() == "WRONGACCOUNTPASSWORD")
                                {
                                    icon = _cameraDisconnectIcons[imgNo.ToString()];
                                }
                            }

                            var tb = new TransformedBitmap();
                            tb.BeginInit();
                            tb.Source = icon;
                            var transform = new RotateTransform(0);
                            tb.Transform = transform;
                            tb.EndInit();

                            element.Source = tb;

                            return;
                        }
                    }
                }
            }
        }

        private String _currentNVRId;
        private String _currentViaId;
        private void ActivateNVRImage(String id, Boolean on)
        {
            if (id == _currentNVRId) return;
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as NVRAttributes;
                    if (lblSource != null)
                    {
                        if (lblSource.Id == id)
                        {
                            var icon = on ? _icons["NVRActivate"] : _icons["NVR"];    //Properties.Resources.IconCamera;
                            var tb = new TransformedBitmap();
                            tb.BeginInit();
                            tb.Source = icon;
                            var transform = new RotateTransform(0);
                            tb.Transform = transform;
                            tb.EndInit();

                            element.Source = tb;
                            if(on) _currentNVRId = id;
                            return;
                        }
                    }
                }
            }
        }

        private void ActivateViaImage(String id, Boolean on)
        {
            if (id == _currentViaId) return;
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as ViaAttributes;
                    if (lblSource != null)
                    {
                        if (lblSource.Id == id)
                        {
                            var icon = on ? _icons["ViaActivate"] : _icons["Via"];    //Properties.Resources.IconCamera;
                            var tb = new TransformedBitmap();
                            tb.BeginInit();
                            tb.Source = icon;
                            var transform = new RotateTransform(0);
                            tb.Transform = transform;
                            tb.EndInit();

                            element.Source = tb;
                            if(on) _currentViaId = id;
                            return;
                        }
                    }
                }
            }
        }

        private void RemoveNVRAndViaActivate()
        {
            if(!String.IsNullOrEmpty(_currentNVRId))
            {
                var id = _currentNVRId;
                _currentNVRId = String.Empty;
                ActivateNVRImage(id, false);
            }

            if (!String.IsNullOrEmpty(_currentViaId))
            {
                var id = _currentViaId;
                _currentViaId = String.Empty;
                ActivateViaImage(id, false);
            }
        }

        public void UpdateNVRStatus()
        {
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as NVRAttributes;
                    if (lblSource != null)
                    {
                        var status = lblSource.Status == NVRStatus.Health || lblSource.Status == NVRStatus.Bad;

                        var icon = status ? _icons["NVR"] : _icons["NVRDisconnect"];    //Properties.Resources.IconCamera;
                        var tb = new TransformedBitmap();
                        tb.BeginInit();
                        tb.Source = icon;
                        var transform = new RotateTransform(0);
                        tb.Transform = transform;
                        tb.EndInit();

                        element.Source = tb;
                    }
                }
            }
        }

        public void UpdateDeviceStatus()
        {
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as Image;
                if (element != null)
                {
                    var lblSource = element.DataContext as CameraAttributes;
                    if (lblSource != null)
                    {
                        var status = lblSource.CameraStatus.ToUpper() != "NOSIGNAL";
                        var rotate = lblSource.Rotate;

                        //remove this map activate acmera
                        var imgNo = rotate / 45;

                        if (imgNo > 7)
                        {
                            imgNo = 0;
                        }

                        var isActivate = false;

                        if (status)
                        {
                            String activateCamId;
                            _activateCameras.TryGetValue(_currentMapId, out activateCamId);
                            isActivate = activateCamId == lblSource.Id;
                        }

                        //var icon = on ? String.Format("{0}/{1}_activate.png", Properties.Resources.CameraPath, imgNo) : String.Format("{0}/{1}.png", Properties.Resources.CameraPath, imgNo);    //Properties.Resources.IconCamera;
                        var icon = isActivate ? _cameraActivateIcons[imgNo.ToString()] : _cameraIcons[imgNo.ToString()];    //Properties.Resources.IconCamera;

                        if (!status)
                        {
                            icon = _cameraDisconnectIcons[imgNo.ToString()];
                        }

                        var tb = new TransformedBitmap();
                        tb.BeginInit();
                        tb.Source = icon;
                        var transform = new RotateTransform(0);
                        tb.Transform = transform;
                        tb.EndInit();

                        element.Source = tb;

                        return;
                    }
                }
            }
        }

        public void ModifyNameById(String id,String type,String name)
        {
            var nodes = MainCanvas.Children;
            foreach (UIElement elem in nodes)
            {
                var element = elem as TextBlock;
                if (element != null)
                {
                    var lblSource = element.DataContext as CameraDescritpion;
                    if (lblSource != null)
                    {
                        if (lblSource.Id == id && lblSource.Type == type)
                        {
                            element.Text = name;
                        }
                    }
                }
            }
        }

        public void ZoomCanvas(MapAttribute map)
        {
            if (IsDrawingHotZone)
            {
                QuitDrawingHotZoneOnMap();
            }
            _cvsScaleTransform.CenterX = CurrentMapSize.Width / 2;
            _cvsScaleTransform.CenterY = CurrentMapSize.Height / 2;
            var currentScale =Convert.ToDouble(Scale/10);
            _cvsScaleTransform.ScaleX = currentScale;
            _cvsScaleTransform.ScaleY = currentScale;

            if (OnScaleMap != null)
            {
                OnScaleMap(this, new EventArgs<Double, Point>(Scale, new Point(_cvsScaleTransform.CenterX, _cvsScaleTransform.CenterY)));
            }
            MainCanvas.Children.Clear();
            if(map == null)
            {
                CreateRootMap();
            }
            else
            {
                CreateMapByObject(map);
            }
            
            ScaleVideoWindow();
        }
    }
}