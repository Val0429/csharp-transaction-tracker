using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace PTZPanel
{
    public partial class PTZPanel : UserControl, IControl, IServerUse, IMinimize, IAppUse, IBlockPanelUse
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnPTZOperation;
        public event EventHandler<EventArgs<WindowPTZRegionLayout>> OnPTZPatrolOperation;

        private const UInt16 DefaultSpeed = 3;
        private const UInt16 MaximumSpeed = 5;

        private Boolean _isDrag;
        protected UInt16 _panSpeed;
        protected UInt16 _tiltSpeed;
        protected String _direction;
        protected String _zoomMode;
        private UInt16 _lastPanSpeed;
        private UInt16 _lastTiltSpeed;
        private String _lastDirection;
        private readonly System.Timers.Timer _patrolRegionTimer = new System.Timers.Timer();
        public Dictionary<String, String> Localization;

        public IApp App { get; set; }
        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        private ICamera _camera;

        public String TitleName { get; set; }

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }
        public Boolean IsMinimize { get; private set; }

        private const UInt16 MoverRange = 50;
        private Point _initialMoverLocation;
        private UInt16 _moverWidthRadius;

        private static readonly Image _light = Resources.GetResources(Properties.Resources.controllerLight, Properties.Resources.IMGControllerLight);
        private static readonly Image _mover = Resources.GetResources(Properties.Resources.mover, Properties.Resources.IMGMover);
        private static readonly Image _moverdisable = Resources.GetResources(Properties.Resources.mover_disable, Properties.Resources.IMGMoverDisable);
        private static readonly Image _controllerBg = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);
        private static readonly Image _controllerBg2 = Resources.GetResources(Properties.Resources.controllerBG2, Properties.Resources.IMGControllerBG2);
        private static readonly Image _incress = Resources.GetResources(Properties.Resources.incress, Properties.Resources.IMGIncress);
        private static readonly Image _incressactivate = Resources.GetResources(Properties.Resources.incress_activate, Properties.Resources.IMGIncressActivate);
        private static readonly Image _reduce = Resources.GetResources(Properties.Resources.reduce, Properties.Resources.IMGReduce);
        private static readonly Image _reduceactivate = Resources.GetResources(Properties.Resources.reduce_activate, Properties.Resources.IMGReduceActivate);

        public PTZPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_PTZ", "PTZ"},

                                   {"PTZPanel_Zoom", "Zoom"},
                                   {"PTZPanel_Focus", "Focus"},

                                   {"PTZPanel_MoveStop", "Stop"},
                                   {"PTZPanel_MoveUp", "Up"},
                                   {"PTZPanel_MoveDown", "Down"},
                                   {"PTZPanel_MoveLeft", "Left"},
                                   {"PTZPanel_MoveRight", "Right"},
                                   {"PTZPanel_MoveUpRight", "Up-Right"},
                                   {"PTZPanel_MoveDownRight", "Down-Right"},
                                   {"PTZPanel_MoveUpLeft", "Up-Left"},
                                   {"PTZPanel_MoveDownLeft", "Down-Left"},

                                   {"PTZPanel_FocusNEAR", "Focus NEAR"},
                                   {"PTZPanel_FocusFAR", "Focus FAR"},
                                   {"PTZPanel_FocusSTOP", "Focus STOP"},

                                   {"PTZPanel_ZoomWIDE", "Zoom WIDE"},
                                   {"PTZPanel_ZoomTELE", "Zoom TELE"},
                                   {"PTZPanel_ZoomSTOP", "Zoom STOP"},
                                   {"PTZPanel_AutoTracking", "Auto Tracking"},
                                   {"PTZPanel_EnablePresetTour", "Enable preset tour"},

                                   {"PresetPointPanel_PresetPoint", "Preset Point"},
                                   {"PresetPointPanel_Stop", "Stop"},

                                   {"PTZPanel_DigitalPTZRegions", "Digital PTZ Regions"},
                                   {"PTZPanel_EnableDigitalPTZPatrol", "Enable digital PTZ patrol"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();

            ptzControlPanel.BackgroundImage = Resources.GetResources(Properties.Resources.ptzBG, Properties.Resources.IMGPtzBG);
            recudeFocusButton.Image = _reduce;
            incressFocusButton.Image = _incress;
            reduceZoomButton.Image = _reduce;
            incressZoomButton.Image = _incress;
            controllerPanel.BackgroundImage = _controllerBg2;
            moverPictureBox.Image = _mover;

            ptzControlPanel.BackColor = ColorTranslator.FromHtml("#626262");
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            zoomLabel.Text = Localization["PTZPanel_Zoom"];
            focusLabel.Text = Localization["PTZPanel_Focus"];
            autoTrackingCheckBox.Text = Localization["PTZPanel_AutoTracking"];
            presetPointLabel.Text = Localization["PresetPointPanel_PresetPoint"];
            enableTourCheckBox.Text = Localization["PTZPanel_EnablePresetTour"];
            patrolLabel.Text = Localization["PTZPanel_DigitalPTZRegions"];
            enablePTZPatrolCheckBox.Text = Localization["PTZPanel_EnableDigitalPTZPatrol"];
            //---------------------------
            Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
            Icon.Click += DockIconClick;
            //---------------------------

            presetPointcomboBox.SelectionChangeCommitted += PresetPointcomboBoxSelectionChangeCommitted;
            RegionsComboBox.SelectionChangeCommitted += RegionsComboBoxSelectionChangeCommitted;
            enableTourCheckBox.CheckedChanged += EnablePresetTourCheckBoxCheckedChanged;
            enablePTZPatrolCheckBox.CheckedChanged += EnablePTZPatrolCheckBoxCheckedChanged;
            autoTrackingCheckBox.Click += AutoTrackingCheckBoxCheckedChanged;

            reduceZoomButton.MouseUp += ReduceZoomButtonMouseUp;
            reduceZoomButton.MouseDown += ReduceZoomButtonMouseDown;
            incressZoomButton.MouseUp += IncressZoomButtonMouseUp;
            incressZoomButton.MouseDown += IncressZoomButtonMouseDown;
            recudeFocusButton.MouseUp += RecudeFocusButtonMouseUp;
            recudeFocusButton.MouseDown += RecudeFocusButtonMouseDown;
            incressFocusButton.MouseUp += IncressFocusButtonMouseUp;
            incressFocusButton.MouseDown += IncressFocusButtonMouseDown;

            controllerPanel.Paint += ControllerPanelPaint;

            _patrolRegionTimer.Interval = 5000;
            _patrolRegionTimer.Elapsed += PatrolRegionTimerElapsed;
        }


        private void AutoTrackingCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (_camera.Model.IsSupportPTZ && _camera.Model.AutoTrackingSupport)
            {
                if (OnPTZOperation != null)
                {
                    OnPTZOperation(this, new EventArgs<String>(PanTiltOperationXml(autoTrackingCheckBox.Checked ? "cmdObjTrackStart" : "cmdObjTrackStop", "", "")));
                }
            }
        }

        private Boolean _isEditing;

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            PanelTitleBarUI2.Text = TitleName = Localization["Control_PTZ"];
            titlePanel.Controls.Add(PanelTitleBarUI2);

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            //get value here is not property, should get when ptz panel is opened
            //_initialMoverLocation = moverPictureBox.Location;

            _moverWidthRadius = Convert.ToUInt16(moverPictureBox.Size.Width / 2);

            if (Server is INVR)
                ((INVR)Server).OnDeviceModify += DeviceModify;

            if (Server is ICMS)
            {
                autoTrackingCheckBox.Visible = false;
                //enableTourCheckBox.Visible = false;

                patrolLabel.Visible =
                enablePTZPatrolCheckBox.Visible =
                RegionsComboBox.Visible = false;
                toolPanel.Height = toolPanel.Height - 80;
            }

            if (Server is IPTS)
            {
                autoTrackingCheckBox.Visible =
                enableTourCheckBox.Visible =
                presetPointLabel.Visible = presetPointcomboBox.Visible =
                patrolLabel.Visible =
                enablePTZPatrolCheckBox.Visible =
                RegionsComboBox.Visible = false;
                toolPanel.Height = toolPanel.Height - 180;
            }

            moverPictureBox.Image = _moverdisable;
            controllerPanel.BackgroundImage = _controllerBg;
            controllerPanel.Enabled = false;
        }

        public void DisablePtzPreset()
        {
            presetPointLabel.Visible = false;
            presetPointcomboBox.Visible = false;
            enableTourCheckBox.Visible = false;

            toolPanel.Height = toolPanel.Controls.OfType<Control>().Where(c => c.Visible).Max(c => c.Location.Y + c.Height) + 5;
        }

        public void AddPresetPanel()
        {
            //Height = 280;
        }

        private float _angle;
        private void ControllerPanelPaint(Object sender, PaintEventArgs e)
        {
            if (!_isDrag) return;
            //Console.WriteLine(_angle);
            Graphics g = e.Graphics;
            //g.Clear(Color.Empty);
            g.TranslateTransform((float)controllerPanel.Width / 2, (float)controllerPanel.Height / 2);
            g.RotateTransform(_angle);
            g.TranslateTransform(-(float)controllerPanel.Width / 2, -(float)controllerPanel.Height / 2);
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(_light, _initialMoverLocation.X - 50, _initialMoverLocation.Y - 50);
        }

        public void DeviceModify(Object sender, EventArgs<IDevice> e)
        {
            if (e.Value == null) return;

            if (_camera == e.Value)
                SelectionChange(null, PTZMode.Disable);
        }

        public virtual void Activate()
        {
            if (_initialMoverLocation.X == 0 && _initialMoverLocation.Y == 0)
                _initialMoverLocation = moverPictureBox.Location;
        }

        public virtual void Deactivate()
        {
            //PatrolStop(this, new EventArgs<object>(null));
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

            if (_camera != null && _camera.Model != null)
            {
                if (_camera.Model.IsSupportPTZ)
                {
                    if (OnPTZOperation != null)
                        OnPTZOperation(this, new EventArgs<String>(PanTiltOperationXml("disablePTZ", "", "")));
                }
            }

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
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
            }

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            if (_camera != null && _camera.Model != null)
            {
                if (_camera.Model.IsSupportPTZ)
                {
                    if (OnPTZOperation != null)
                        OnPTZOperation(this, new EventArgs<String>(PanTiltOperationXml("enablePTZ", "", "")));
                }

                if (Server is ICMS)
                {
                    if (!_camera.IsLoadPresetPoint)
                    {
                        ApplicationForms.ShowLoadingIcon(Server.Form);
                        var cms = Server as ICMS;
                        cms.NVRManager.LoadNVRDevicePresetPoint((INVR)_camera.Server);
                        ApplicationForms.HideLoadingIcon();
                    }
                }
            }

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);

        }

        public void PTZModeChange(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);

            String status = Xml.GetFirstElementValueByTagName(xmlDoc, "Status");

            var display = (_camera != null && _camera.Model != null && _camera.Model.IsSupportPTZ && _camera.CheckPermission(Permission.OpticalPTZ));

            if (display && status == "Activate")
                Maximize();
            //dont minimize when ptz is off
            //else
            //Minimize();
        }

        public void SelectionChange(Object sender, EventArgs<ICamera, PTZMode> e)
        {
            SelectionChange(e.Value1, e.Value2);
        }

        public void SelectionChange(ICamera camera, PTZMode ptzmode)
        {
            _isEditing = false;

            _camera = camera;
            _patrolRegionTimer.Enabled = false;

            presetPointcomboBox.Items.Clear();
            presetPointcomboBox.Enabled = false;

            enableTourCheckBox.Checked = false;
            enableTourCheckBox.Enabled = false;

            autoTrackingCheckBox.Checked = false;

            var ptEnable = false;
            var zEnable = false;
            var fEnable = false;

            if (_camera != null && _camera.Model != null && _camera.CheckPermission(Permission.OpticalPTZ))
            {
                autoTrackingCheckBox.Enabled = camera.Model.AutoTrackingSupport;

                if (Server is ICMS)
                {
                    if (IsMinimize == false)
                    {
                        if (!_camera.IsLoadPresetPoint)
                        {
                            ApplicationForms.ShowLoadingIcon(Server.Form);
                            var cms = Server as ICMS;
                            cms.NVRManager.LoadNVRDevicePresetPoint((INVR)_camera.Server);
                            ApplicationForms.HideLoadingIcon();
                        }
                    }

                    enableTourCheckBox.Enabled = _camera.PresetPoints.Count > 0;
                    if (_camera.PresetTourGo == 1)
                        enableTourCheckBox.Checked = true;
                }
                else
                {
                    if (_camera.PresetTours.Count > 0)
                    {
                        enableTourCheckBox.Enabled = true;
                        if (_camera.PresetTourGo == _camera.PresetTours.Values.First().Id)
                            enableTourCheckBox.Checked = true;
                    }
                }


                ptEnable = (_camera.Model.PanSupport || _camera.Model.TiltSupport);
                zEnable = _camera.Model.ZoomSupport;
                fEnable = _camera.Model.FocusSupport;
            }

            if (ptEnable || zEnable || fEnable)
            {
                if (ptEnable)
                {
                    controllerPanel.Enabled = true;
                    moverPictureBox.Image = _mover;
                    controllerPanel.BackgroundImage = _controllerBg2;
                }
                else
                {
                    controllerPanel.Enabled = false;
                    moverPictureBox.Image = _moverdisable;
                    controllerPanel.BackgroundImage = _controllerBg;
                }

                reduceZoomButton.Enabled = incressZoomButton.Enabled = zEnable;
                recudeFocusButton.Enabled = incressFocusButton.Enabled = fEnable;
                if (Server is ICMS)
                {
                    focusPanel.Visible = fEnable;
                }

                LoadPresetData();

                if (ptzmode == PTZMode.Optical)
                    Maximize();
                //else //dont auto hide
                //    Minimize();
            }
            else
            {
                moverPictureBox.Image = _moverdisable;
                controllerPanel.BackgroundImage = _controllerBg;

                autoTrackingCheckBox.Enabled =
                controllerPanel.Enabled =
                reduceZoomButton.Enabled = incressZoomButton.Enabled =
                recudeFocusButton.Enabled = incressFocusButton.Enabled =
                presetPointcomboBox.Enabled = false;
                LoadDigitalPTZRegions();
                //dont auto hide
                //Minimize();
            }

            _isEditing = true;
        }

        private Dictionary<UInt16, WindowPTZRegionLayout> _patrolList = new Dictionary<UInt16, WindowPTZRegionLayout>();
        private void LoadDigitalPTZRegions()
        {
            RegionsComboBox.Items.Clear();
            _patrolList.Clear();

            if (_camera == null || _camera.Model == null || !_camera.CheckPermission(Permission.OpticalPTZ))
            {
                RegionsComboBox.Enabled = enablePTZPatrolCheckBox.Enabled = false;
                return;
            }

            var points = _camera.PatrolPoints.Values.Where(x => x != null).OrderBy(g => g.Id);
            RegionsComboBox.Items.Add(String.Empty);
            var count = 1;
            foreach (var point in points)
            {
                RegionsComboBox.Items.Add(point.Id.ToString().PadLeft(2, '0') + " " + point.Name);
                _patrolList.Add((ushort)count, point);
                count++;
            }

            RegionsComboBox.SelectedIndex = 0;


            RegionsComboBox.Enabled = enablePTZPatrolCheckBox.Enabled = RegionsComboBox.Items.Count > 1;
        }

        private void LoadPresetData()
        {
            presetPointcomboBox.Items.Clear();

            if (_camera == null || _camera.Model == null || !_camera.CheckPermission(Permission.OpticalPTZ))
                return;

            if (!_camera.Model.IsSupportPTZ)
            {
                LoadDigitalPTZRegions();
                return;
            }

            LoadDigitalPTZRegions();

            var points = _camera.PresetPoints.Values.OrderBy(g => g.Id);

            foreach (var point in points)
            {
                presetPointcomboBox.Items.Add(point.Id.ToString().PadLeft(2, '0') + " " + point.Name);
            }

            if (_camera.PresetPoints.ContainsKey(_camera.PresetPointGo))
                presetPointcomboBox.SelectedItem = _camera.PresetPoints[_camera.PresetPointGo].Name;

            if (presetPointcomboBox.Items.Count > 0)
                presetPointcomboBox.Enabled = true;
        }


        private void RegionsComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_camera == null || RegionsComboBox.SelectedItem == null) return;

            if (String.IsNullOrEmpty(RegionsComboBox.SelectedItem.ToString()))
            {
                if (OnPTZPatrolOperation != null)
                    OnPTZPatrolOperation(this, new EventArgs<WindowPTZRegionLayout>(null));
                return;
            }

            foreach (KeyValuePair<ushort, WindowPTZRegionLayout> obj in _camera.PatrolPoints)
            {
                if (obj.Value == null) continue;
                if (String.Equals(obj.Value.ToString(), RegionsComboBox.SelectedItem.ToString()))
                {
                    if (OnPTZPatrolOperation != null)
                        OnPTZPatrolOperation(this, new EventArgs<WindowPTZRegionLayout>(obj.Value));
                }
            }
        }

        private void PresetPointcomboBoxSelectionChangeCommitted(Object sender, EventArgs e)
        {
            if (_camera == null || presetPointcomboBox.SelectedItem == null) return;

            foreach (KeyValuePair<UInt16, PresetPoint> obj in _camera.PresetPoints)
            {
                if (String.Equals(obj.Value.ToString(), presetPointcomboBox.SelectedItem.ToString()))
                {
                    Server.WriteOperationLog("Goto preset %1".Replace("%1", obj.Key.ToString()));
                    if (Server is ICMS)
                    {
                        PresetGoOperation(obj.Key.ToString());
                    }
                    else
                    {
                        _camera.PresetPointGo = obj.Key;
                    }
                }
            }
        }

        private UInt16 _currentRegionId = 1;
        private void EnablePTZPatrolCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (!_isEditing) return;
            if (_camera == null) return;

            if (enablePTZPatrolCheckBox.Checked)
            {
                _currentRegionId = 1;
                _patrolRegionTimer.Interval = _camera.PatrolInterval * 1000;
                _patrolRegionTimer.Enabled = true;
                RegionsComboBox.Enabled = false;
                PatrolRegionTimerElapsed(this, null);
            }
            else
            {
                _currentRegionId = 1;
                _patrolRegionTimer.Enabled = false;
                RegionsComboBox.Enabled = true;
            }
        }

        public void PatrolStop(Object sender, EventArgs<Object> e)
        {
            if (e.Value == null)
            {
                _currentRegionId = 1;
                _patrolRegionTimer.Enabled = false;
                RegionsComboBox.Enabled = true;
                enablePTZPatrolCheckBox.Checked = false;
            }
        }

        private void PatrolRegionTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_patrolList.ContainsKey(_currentRegionId))
            {
                _currentRegionId = 1;
            }

            if (!_patrolList.ContainsKey(_currentRegionId)) return;

            var region = _patrolList[_currentRegionId];

            if (region == null) return;
            if (OnPTZPatrolOperation != null)
            {
                OnPTZPatrolOperation(this, new EventArgs<WindowPTZRegionLayout>(region));
            }
            _currentRegionId++;
        }

        private void EnablePresetTourCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;
            if (_camera == null) return;

            if (enableTourCheckBox.Checked && _camera.PresetPoints.Count > 0)
                _camera.PresetTourGo = _camera.PresetPoints.Values.First().Id;// 1
            else
                _camera.PresetTourGo = 0;

            _camera.PresetTours.IsModify = true;
            Server.Device.SavePresetTour(_camera);

            if (Server is ICMS)
            {
                if (OnPTZOperation != null)
                {
                    OnPTZOperation(this, new EventArgs<String>(EnableTourOperationXml(enableTourCheckBox.Checked ? "cmdPTZPresetTourStart" : "cmdPTZPresetTourStop")));
                }
            }
        }

        private static String EnableTourOperationXml(String command)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Command", command));

            return xmlDoc.InnerXml;
        }

        private static String PanTiltOperationXml(String command, String panSpeed, String tiltSpeed)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Command", command));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "PanSpeed", panSpeed));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TiltSpeed", tiltSpeed));

            return xmlDoc.InnerXml;
        }

        protected void PanTiltOperation()
        {
            if (_lastPanSpeed == _panSpeed && _lastTiltSpeed == _tiltSpeed && _lastDirection == _direction) return;

            if (OnPTZOperation != null)
            {
                var log = _direction.Substring(3);
                if (log != _lastLog)
                {
                    Server.WriteOperationLog("PTZ Operation %1".Replace("%1", Localization["PTZPanel_" + _direction.Substring(3)]));
                    _lastLog = log;
                }

                OnPTZOperation(this, new EventArgs<String>(PanTiltOperationXml(_direction, _panSpeed.ToString(), _tiltSpeed.ToString())));
            }

            _lastPanSpeed = _panSpeed;
            _lastTiltSpeed = _tiltSpeed;
            _lastDirection = _direction;
        }

        private static String ZoomFocusOperationXml(String command, String mode)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Command", command));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mode", mode));

            return xmlDoc.InnerXml;
        }

        private String _lastLog = "";
        protected void ZoomFocusOperation()
        {
            if (OnPTZOperation != null)
            {
                var log = _direction.Substring(3) + _zoomMode;
                if (log != _lastLog)
                {
                    Server.WriteOperationLog("PTZ Operation %1".Replace("%1", Localization["PTZPanel_" + _direction.Substring(3) + _zoomMode]));
                    _lastLog = log;
                }

                OnPTZOperation(this, new EventArgs<String>(ZoomFocusOperationXml(_direction, _zoomMode)));
            }

        }

        private static String PresetGoOperationXml(String command, String id)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Command", command));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Id", id));

            return xmlDoc.InnerXml;
        }

        protected void PresetGoOperation(String id)
        {
            if (OnPTZOperation != null)
            {
                var log = id;
                if (log != _lastLog)
                {
                    Server.WriteOperationLog("PTZ Preset Go %1".Replace("%1", id));
                    _lastLog = log;
                }

                OnPTZOperation(this, new EventArgs<String>(PresetGoOperationXml("cmdPTZPresetGO", id)));
            }

        }

        private void UpPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            _direction = "cmdMoveUp";
            _panSpeed = 0;
            _angle = 270;
            _isDrag = true;
            _tiltSpeed = DefaultSpeed;
            PanTiltOperation();
            moverPictureBox.Location = new Point(moverPictureBox.Location.X, moverPictureBox.Location.Y - MoverRange);
            MoverPictureBoxMouseDown(this, e);
            moverPictureBox.Capture = true;
        }

        private void RightPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            _direction = "cmdMoveRight";
            _panSpeed = DefaultSpeed;
            _tiltSpeed = 0;
            _angle = 0;
            _isDrag = true;
            PanTiltOperation();
            moverPictureBox.Location = new Point(moverPictureBox.Location.X + MoverRange, moverPictureBox.Location.Y);
            MoverPictureBoxMouseDown(this, e);
            moverPictureBox.Capture = true;
        }

        private void DownPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            _direction = "cmdMoveDown";
            _panSpeed = 0;
            _tiltSpeed = 90;
            _isDrag = true;
            _tiltSpeed = DefaultSpeed;
            PanTiltOperation();
            moverPictureBox.Location = new Point(moverPictureBox.Location.X, moverPictureBox.Location.Y + MoverRange);
            MoverPictureBoxMouseDown(this, e);
            moverPictureBox.Capture = true;
        }

        private void LeftPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            _direction = "cmdMoveLeft";
            _panSpeed = DefaultSpeed;
            _tiltSpeed = 0;
            _tiltSpeed = 180;
            _isDrag = true;
            PanTiltOperation();
            moverPictureBox.Location = new Point(moverPictureBox.Location.X - MoverRange, moverPictureBox.Location.Y);
            MoverPictureBoxMouseDown(this, e);
            moverPictureBox.Capture = true;
        }

        private void PtzPictureBoxMouseUp(Object sender, MouseEventArgs e)
        {
            _direction = "cmdMoveStop";
            _panSpeed = 0;
            _tiltSpeed = 0;
            _isDrag = false;
            PanTiltOperation();
            moverPictureBox.Location = _initialMoverLocation;
        }

        private void MoverPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            _isDrag = true;
            moverPictureBox.MouseMove -= MoverPictureBoxMouseMove;
            moverPictureBox.MouseMove += MoverPictureBoxMouseMove;
        }

        private void MoverPictureBoxMouseUp(Object sender, MouseEventArgs e)
        {
            moverPictureBox.MouseMove -= MoverPictureBoxMouseMove;
            moverPictureBox.Location = _initialMoverLocation;
            moverPictureBox.Capture = false;
            _isDrag = false;

            _direction = "cmdMoveStop";
            _panSpeed = 0;
            _tiltSpeed = 0;
            PanTiltOperation();
            controllerPanel.Invalidate();
        }

        private void MoverPictureBoxMouseMove(Object sender, MouseEventArgs e)
        {
            Point point = moverPictureBox.Location;
            Angle(_initialMoverLocation.X, _initialMoverLocation.Y, point.X, point.Y);

            Int32 rangeX = point.X + e.X - _moverWidthRadius - _initialMoverLocation.X;
            Int32 rangeY = point.Y + e.Y - _moverWidthRadius - _initialMoverLocation.Y;

            Double distance = Math.Round(Math.Sqrt(Math.Pow(rangeX, 2) + Math.Pow(rangeY, 2)), 0);

            if (distance > MoverRange)
            {
                Double percent = MoverRange / distance;

                point.X = _initialMoverLocation.X + Convert.ToInt32(Math.Round(rangeX * percent));
                point.Y = _initialMoverLocation.Y + Convert.ToInt32(Math.Round(rangeY * percent));

                distance = MoverRange;
            }
            else
            {
                point.X += (e.X - _moverWidthRadius);
                point.Y += (e.Y - _moverWidthRadius);
            }

            rangeX = point.X - _initialMoverLocation.X;
            rangeY = point.Y - _initialMoverLocation.Y;

            moverPictureBox.Location = point;

            //Console.WriteLine(rangeX + " " +rangeY);

            _panSpeed = _tiltSpeed = Convert.ToUInt16(Math.Round(distance * MaximumSpeed / MoverRange));

            if (Math.Abs(rangeX) > Math.Abs(rangeY))
                _tiltSpeed = Convert.ToUInt16(Math.Round(_panSpeed * Math.Abs(rangeY / (double)rangeX)));
            else if (Math.Abs(rangeX) < Math.Abs(rangeY))
                _panSpeed = Convert.ToUInt16(Math.Round(_tiltSpeed * Math.Abs(rangeX / (double)rangeY)));

            //Console.WriteLine(_panSpeed + " " + _tiltSpeed);

            if (rangeX == 0 && rangeY == 0) return;

            //Up, Down
            if (_panSpeed == 0) //rangeX == 0)
            {
                _direction = (rangeY < 0) ? "cmdMoveUp" : "cmdMoveDown";
            }
            //Left, Right
            else if (_tiltSpeed == 0) //rangeY == 0)
            {
                _direction = (rangeX < 0) ? "cmdMoveLeft" : "cmdMoveRight";
            }
            //Up-Right
            else if (rangeX > 0 && rangeY < 0)
            {
                _direction = "cmdMoveUpRight";
            }
            //Down-Right
            else if (rangeX > 0 && rangeY > 0)
            {
                _direction = "cmdMoveDownRight";
            }
            //Up-Left
            else if (rangeX < 0 && rangeY < 0)
            {
                _direction = "cmdMoveUpLeft";
            }
            //Down-Left
            else
            {
                _direction = "cmdMoveDownLeft";
            }
            //Console.WriteLine(_direction);

            PanTiltOperation();
            controllerPanel.Invalidate();
        }

        public void Angle(double px1, double py1, double px2, double py2)
        {
            _angle = (float)(Math.Atan2(py2 - py1, px2 - px1) * 180.0 / Math.PI);
        }

        private void IncressFocusButtonMouseDown(Object sender, MouseEventArgs e)
        {
            incressFocusButton.Image = _incressactivate;
            _direction = "cmdFocus";
            _zoomMode = "NEAR";
            ZoomFocusOperation();
        }

        private void IncressFocusButtonMouseUp(Object sender, MouseEventArgs e)
        {
            incressFocusButton.Image = _incress;
            _direction = "cmdFocus";
            _zoomMode = "STOP";
            ZoomFocusOperation();
        }

        private void RecudeFocusButtonMouseDown(Object sender, MouseEventArgs e)
        {
            recudeFocusButton.Image = _reduceactivate;
            _direction = "cmdFocus";
            _zoomMode = "FAR";
            ZoomFocusOperation();
        }

        private void RecudeFocusButtonMouseUp(Object sender, MouseEventArgs e)
        {
            recudeFocusButton.Image = _reduce;
            _direction = "cmdFocus";
            _zoomMode = "STOP";
            ZoomFocusOperation();
        }

        private void IncressZoomButtonMouseDown(Object sender, MouseEventArgs e)
        {
            incressZoomButton.Image = _incressactivate;
            _direction = "cmdZoom";
            _zoomMode = "TELE";
            ZoomFocusOperation();
        }

        private void IncressZoomButtonMouseUp(Object sender, MouseEventArgs e)
        {
            incressZoomButton.Image = _incress;
            _direction = "cmdZoom";
            _zoomMode = "STOP";
            ZoomFocusOperation();
        }

        private void ReduceZoomButtonMouseDown(Object sender, MouseEventArgs e)
        {
            reduceZoomButton.Image = _reduceactivate;
            _direction = "cmdZoom";
            _zoomMode = "WIDE";
            ZoomFocusOperation();
        }

        private void ReduceZoomButtonMouseUp(Object sender, MouseEventArgs e)
        {
            reduceZoomButton.Image = _reduce;
            _direction = "cmdZoom";
            _zoomMode = "STOP";
            ZoomFocusOperation();
        }
    }
}
