using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace PresetPointPanel
{
	public partial class PresetPointPanel : UserControl, IControl, IServerUse, IMinimize, IAppUse
	{
		public event EventHandler OnMinimizeChange;

		public IApp App { get; set; }
		public IServer Server { get; set; }

		protected ICamera _camera;

		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private Image _icon;

		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();

		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }
	   
		public PresetPointPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_PresetPoint", "Preset Point"},

								   {"PresetPointPanel_PresetPoint", "Preset Point"},
								   {"PresetPointPanel_PresetTour", "Preset Tour"},
								   {"PresetPointPanel_Stop", "Stop"},
								   {"PresetPointPanel_GotoPerset", "Goto preset %1"},
							   };
			Localizations.Update(Localization);

			_icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

			InitializeComponent();

			presetPointListPanel.BackColor = ColorTranslator.FromHtml("#626262");
			Dock = DockStyle.Fill;

			presetPointLabel.Text = Localization["PresetPointPanel_PresetPoint"];
			presetTourlabel.Text = Localization["PresetPointPanel_PresetTour"];

			//presetPointcomboBox.Location = new Point(presetPointcomboBox.Location.X, 18);
			//presetPointLabel.Location = new Point(presetPointLabel.Location.X, 16);
			//presetTourComboBox.Visible = presetTourlabel.Visible = false;
			//---------------------------
			Icon = new ControlIconButton { Image = _icon };
			Icon.Click += DockIconClick;
			//---------------------------
		}

		public virtual void Initialize()
		{
			_panelTitleBar.Text = TitleName = Localization["Control_PresetPoint"];

			_panelTitleBar.Panel = this;
			titlePanel.Controls.Add(_panelTitleBar);

			var toolTip = new ToolTip();
			toolTip.SetToolTip(Icon, TitleName);

			if (Server is INVR)
				((INVR)Server).OnDeviceModify += DeviceModify;
		}

		public void DeviceModify(Object sender, EventArgs<IDevice> e)
		{
			if (e.Value == null) return;

			if (_camera == e.Value)
				SelectionChange(null, PTZMode.Disable);
		}

		public void SelectionChange(Object sender, EventArgs<ICamera, PTZMode> e)
		{
			SelectionChange(e.Value1, e.Value2);
		}

		public void SelectionChange(ICamera camera, PTZMode ptzMode)
		{
			_camera = camera;

			presetPointcomboBox.Items.Clear();
//			presetTourComboBox.Items.Clear();
			presetPointcomboBox.Enabled = false;
//			presetTourComboBox.Enabled = false;

			if (_camera != null && _camera.CheckPermission(Permission.OpticalPTZ))
			{
				LoadPresetData();

				//if (ptzMode == PTZMode.Optical)
				//{
				//    Maximize();
				//}
				//else
				//    Minimize();
			}
			else
				Minimize();
		}

		public void PTZModeChange(Object sender, EventArgs<String> e)
		{
			XmlDocument xmlDoc = Xml.LoadXml(e.Value);

			String status = Xml.GetFirstElementValueByTagName(xmlDoc, "Status");

			var display = (_camera != null && _camera.Model != null && _camera.Model.IsSupportPTZ && _camera.CheckPermission(Permission.OpticalPTZ));

			if (display && status == "Activate" && _camera.PresetPoints.Count > 0)
			{
				Maximize();
				//LoadPresetData();
			}
			else
				Minimize();
		}

		private static String PatrolTourChangedXml(String type, String nvr, String id)
		{
			var xmlDoc = new XmlDocument();

			XmlElement xmlRoot = xmlDoc.CreateElement("XML");
			xmlDoc.AppendChild(xmlRoot);

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Type", type));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", nvr));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Id", id));

			return xmlDoc.InnerXml;
		}

		private void LoadPresetData()
		{
			presetPointcomboBox.Items.Clear();
			//presetTourComboBox.Items.Clear();

			if (_camera == null || _camera.Model == null || !_camera.Model.IsSupportPTZ || !_camera.CheckPermission(Permission.OpticalPTZ))
				return;

			var pointIds = _camera.PresetPoints.Keys.ToList();
			pointIds.Sort();
			foreach (var pointId in pointIds)
			{
				presetPointcomboBox.Items.Add(pointId.ToString().PadLeft(2, '0') + " " + _camera.PresetPoints[pointId].Name);
			}

			if (_camera.PresetPoints.ContainsKey(_camera.PresetPointGo))
				presetPointcomboBox.SelectedItem = _camera.PresetPoints[_camera.PresetPointGo].Name;

			//if (_camera.PresetTours.Count > 0)
			//    presetTourComboBox.Items.Add("Stop");

			//var tourIds = _camera.PresetTours.Keys.ToList();
			//tourIds.Sort();
			//foreach (var tourId in tourIds)
			//{
			//    presetTourComboBox.Items.Add(tourId.ToString().PadLeft(2, '0') + " " + _camera.PresetTours[tourId].Name);
			//}

			if (presetPointcomboBox.Items.Count > 0)
				presetPointcomboBox.Enabled = true;

			//if (presetTourComboBox.Items.Count > 0)
			//    presetTourComboBox.Enabled = true;

			//if(_camera.PresetTours.ContainsKey(_camera.PresetTourGo))
			//    presetTourComboBox.SelectedItem = _camera.PresetTours[_camera.PresetTourGo].Name;
		}

		//private static String PresetOperationXml(String command, String preset)
		//{
		//    var xmlDoc = new XmlDocument();

		//    XmlElement xmlRoot = xmlDoc.CreateElement("XML");
		//    xmlDoc.AppendChild(xmlRoot);

		//    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Command", command));
		//    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Preset", preset));

		//    return xmlDoc.InnerXml;
		//}

		private void PresetPointcomboBoxSelectionChangeCommitted(Object sender, EventArgs e)
		{
			if (_camera == null || presetPointcomboBox.SelectedItem == null) return;

			foreach (KeyValuePair<UInt16, PresetPoint> obj in _camera.PresetPoints)
			{
				if (String.Equals(obj.Value.ToString(), presetPointcomboBox.SelectedItem.ToString()))
				{
					Server.WriteOperationLog(Localization["PresetPointPanel_GotoPerset"].Replace("%1", obj.Key.ToString()));
					_camera.PresetPointGo = obj.Key;
				}
			}
		}

		private void PresetTourComboBoxSelectionChangeCommitted(Object sender, EventArgs e)
		{
			//if (_camera == null) return;

			////STOP
			//if (presetTourComboBox.SelectedIndex == 0)
			//{
			//    _camera.PresetTourGo = 0;
			//}
			//else
			//{
			//    foreach (KeyValuePair<UInt16, PresetTour> obj in _camera.PresetTours)
			//    {
			//        if (String.Equals(obj.Value.Name, presetTourComboBox.SelectedItem.ToString()))
			//        {
			//            _camera.PresetTourGo = obj.Key;
			//        }
			//    }
			//}

			//Server.Device.Save(PatrolTourChangedXml("PresetTour", _camera.Server.Id.ToString(), _camera.Id.ToString()));
		}

		public virtual void Activate()
		{
		}

		public virtual void Deactivate()
		{
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
			IsMinimize = false;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}
	}
}
