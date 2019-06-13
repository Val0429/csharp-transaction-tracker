using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class ImmerVisionLayoutEditPanel : UserControl
    {
		public event EventHandler<EventArgs<IDeviceLayout>> OnSubDeviceLayoutEdit;

        public IApp App;
        public IServer Server;
        private INVR _nvr;
        public IDeviceLayout DeviceLayout;
	    //public IDevice Camera;
        public Dictionary<String, String> Localization;

        private VideoMonitor.VideoMonitor _monitor;
        public ImmerVisionLayoutEditPanel()
        {
	        Localization = new Dictionary<String, String>
		        {
			        {"DeviceLayoutPanel_Layout", "Layout"},
			        {"DeviceLayoutPanel_PredefineLayout", "Predefine Layout"},
			        {"DeviceLayoutPanel_Device", "Device"},
			        {"DeviceLayoutPanel_Name", "Name"},
			        {"DeviceLayoutPanel_DeviceResolution", "Device Resolution"},
			        {"DeviceLayoutPanel_SetupSubLayout", "Setup Sub-Layout"},
			        {"DeviceLayoutPanel_LensSetting", "Lens Setting"},
			        {"DeviceLayoutPanel_MountingType", "Mounting Type"},

			        {"DeviceLayoutPanel_Ceiling", "Ceiling"},
			        {"DeviceLayoutPanel_Ground", "Ground"},
			        {"DeviceLayoutPanel_Wall", "Wall"},

			        {"DeviceLayoutPanel_PTZ", "PTZ"},
			        {"DeviceLayoutPanel_Quad", "Quad"},
			        {"DeviceLayoutPanel_Perimeter", "Perimeter"},
		        };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.Background;

            layoutLabel.Text = Localization["DeviceLayoutPanel_Layout"];

			nameDoubleBufferPanel.Paint += NamePanelPaint;

            layoutDoubleBufferPanel.Paint += LayoutPanelPaint;
			deviceDoubleBufferPanel.Paint += DeviceDoubleBufferPanelPaint;
			resolutionDoubleBufferPanel.Paint += ResolutionPanelPaint;
			mountingTypeDoubleBufferPanel.Paint +=MountingTypeDoubleBufferPanelPaint;
			lensSettingDoubleBufferPanel.Paint +=LensSettingDoubleBufferPanelPaint;

            subLayoutDoubleBufferPanel.Paint += SubLayoutPanelPaint;
        }

		private void NamePanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, nameDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_Name"]);
		}

		private void DeviceDoubleBufferPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, deviceDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_Device"]);
		}

		private void ResolutionPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, resolutionDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_DeviceResolution"]);
		}

		private void MountingTypeDoubleBufferPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, mountingTypeDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_MountingType"]);
		}

		private void LensSettingDoubleBufferPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, lensSettingDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_LensSetting"]);
		}

		private void LayoutPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, layoutDoubleBufferPanel);

			Manager.PaintText(g, Localization["DeviceLayoutPanel_PredefineLayout"]);
		}

		private void SubLayoutPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (DeviceLayout.Items.Count(device => device != null) == 0)
            {
                Manager.PaintSingleInput(g, subLayoutDoubleBufferPanel);
            }
            else
            {
                Manager.PaintHighLightInput(g, subLayoutDoubleBufferPanel);
                Manager.PaintEdit(g, subLayoutDoubleBufferPanel);
            }

            Manager.PaintText(g, Localization["DeviceLayoutPanel_SetupSubLayout"]);
        }

        public void Initialize()
        {
            _nvr = Server as INVR;
            _monitor = new VideoMonitor.VideoMonitor { App = App, Server = Server };
            _monitor.Initialize();
            _monitor.HidePageLabel();
            //_monitor.SetEditProperty();
            _monitor.OnContentChange += MonitorOnContentChange;

            containerPanel.Controls.Add(_monitor);

            nameTextBox.MaxLength = 50;
			nameTextBox.KeyPress += nameTextBox_KeyPress;
			nameTextBox.TextChanged += NameTextBoxTextChanged;

			deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;

            resolutionComboBox.SelectedIndexChanged += ResolutionComboBoxSelectedIndexChanged;
			mountingTypeComboBox.SelectedIndexChanged += MountingTypeComboBoxSelectedIndexChanged;
			lensSettingComboBox.SelectedIndexChanged += LensSettingComboBoxSelectedIndexChanged;

			subLayoutDoubleBufferPanel.MouseClick += SubLayoutDoubleBufferPanelMouseClick;
        }

		void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (Char)37) { e.Handled = true; return; } 	//	%
			else if (e.KeyChar == (Char)42) { e.Handled = true; return; }	//	*
			else if (e.KeyChar == (Char)44) { e.Handled = true; return; }	//	,
			else if (e.KeyChar == (Char)47) { e.Handled = true; return; }	//	/ 
			else if (e.KeyChar == (Char)58) { e.Handled = true; return; }	//	:
			else if (e.KeyChar == (Char)92) { e.Handled = true; return; }	//	\
			else if (e.KeyChar == (Char)62) { e.Handled = true; return; }	//	>
			else if (e.KeyChar == (Char)63) { e.Handled = true; return; }	//	?
			else if (e.KeyChar == (Char)34) { e.Handled = true; return; }	//	"
			else if (e.KeyChar == (Char)60) { e.Handled = true; return; }	//	<
			else if (e.KeyChar == (Char)124) { e.Handled = true; return; }	//	|

			e.Handled = false;
		}

		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEdit) return;
			if (DeviceLayout == null) return;

			DeviceLayout.Name = nameTextBox.Text;
			_nvr.DeviceLayoutModify(DeviceLayout);
		}

		private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEdit) return;
			if (DeviceLayout == null) return;

			DeviceLayout.Items.Clear();
			DeviceLayout.SubLayouts.Clear();

			DeviceLayout.Items.Add(deviceComboBox.SelectedItem as IDevice);

			DeviceLayout.Dewarps = new List<bool> {false, false, false, false};

			_nvr.DeviceLayoutModify(DeviceLayout);

			UpdateDewarpSelection();
			ShowMonitor();
		}

	    private void ShowMonitor()
	    {
			_isEdit = false;
			UpdateResolution();
			_isEdit = true;

			if ( DeviceLayout.SubLayouts.Count == 0)
				GenerateSubLayout();

		    ShowSnapshot();
		    //_nvr.DeviceLayoutModify(DeviceLayout);
		    subLayoutDoubleBufferPanel.Invalidate();

		    if (DeviceLayout.Items.Count(device => device != null) == 0)
			    subLayoutDoubleBufferPanel.Cursor = Cursors.Default;
		    else
			    subLayoutDoubleBufferPanel.Cursor = Cursors.Hand;

	    }

	    private void ResolutionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			var resolution = Resolutions.ToIndex(resolutionComboBox.SelectedItem.ToString());
			if (resolution == Resolution.NA) return;

			if ( (Resolutions.ToWidth(resolution) == DeviceLayout.WindowWidth) &&
				(Resolutions.ToHeight(resolution) == DeviceLayout.WindowHeight)) return;

			DeviceLayout.WindowWidth = Resolutions.ToWidth(resolution);
			DeviceLayout.WindowHeight = Resolutions.ToHeight(resolution);

			Server.Device.CheckSubLayoutRange(DeviceLayout);
			_nvr.DeviceLayoutModify(DeviceLayout);

			DeviceLayout.SubLayouts.Clear();
			GenerateSubLayout();

			if (!_isEdit) return;
			if (DeviceLayout == null) return;


		    ShowMonitor();
		}

		private void MountingTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_isEdit) return;
			if (DeviceLayout == null) return;

			var item = mountingTypeComboBox.SelectedItem as ComboxItem;
			if ( item == null) return;

			DeviceLayout.MountingType = MountingTypes.ToIndex(item.Value);

			nameTextBox.Text = String.Format("{0}-{1}", MountingTypes.ToString(DeviceLayout.MountingType), LensSettings.ToString(DeviceLayout.LensSetting));

			DeviceLayout.SubLayouts.Clear();

            DeviceLayout.SubLayouts.Remove(99);

            if (DeviceLayout.SubLayouts.Count == 0)
                GenerateSubLayout();

            DewarpCheckBoxCheckedChanged(this, EventArgs.Empty);
		}

		private void LensSettingComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_isEdit) return;
			if (DeviceLayout == null) return;

			var item = lensSettingComboBox.SelectedItem as ComboxItem;
			if ( item == null) return;

			DeviceLayout.LensSetting = LensSettings.ToIndex(item.Value);

			nameTextBox.Text = String.Format("{0}-{1}", MountingTypes.ToString(DeviceLayout.MountingType), LensSettings.ToString(DeviceLayout.LensSetting));

			switch (item.Value)
			{
				case "PTZ" :
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_1, Properties.Resources.IMGLayout1);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_1s, Properties.Resources.IMGLayout1WithSource);
					break;
				case "Quad" :
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_4, Properties.Resources.IMGLayout4);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_4s, Properties.Resources.IMGLayout4WithSource);
					break;
                case "Perimeter":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_2, Properties.Resources.IMGLayout2);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_2s, Properties.Resources.IMGLayout2WithSource);
					break;
			}

			DeviceLayout.SubLayouts.Clear();

			SplitslabelClick(this, EventArgs.Empty);
		}

		private void SplitslabelClick(object sender, EventArgs e)
		{
			var item = lensSettingComboBox.SelectedItem as ComboxItem;
			if (item == null) return;

			var layout = new List<WindowLayout>();
			switch (item.Value)
			{
				case "PTZ":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_1_use, Properties.Resources.IMGLayout1_use);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_1s, Properties.Resources.IMGLayout1WithSource);
					layout = WindowLayouts.LayoutGenerate(1);
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "*";
					break;
				case "Quad":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_4_use, Properties.Resources.IMGLayout4_use);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_4s, Properties.Resources.IMGLayout4WithSource);
					layout = WindowLayouts.LayoutGenerate(4);
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "**,**";
					break;
                case "Perimeter":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_2_use, Properties.Resources.IMGLayout2_use);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_2s, Properties.Resources.IMGLayout2WithSource);
					layout = WindowLayouts.LayoutGenerate("*,*");
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "*,*";
					break;
			}

			DeviceLayout.isIncludeDevice = false;

			DeviceLayout.SubLayouts.Remove(99);

            if (DeviceLayout.SubLayouts.Count == 0)
                GenerateSubLayout();

            DewarpCheckBoxCheckedChanged(this, EventArgs.Empty);

			//ShowMonitor();
		}

		private void SplitswithsourcelabelClick(object sender, EventArgs e)
		{
			var item = lensSettingComboBox.SelectedItem as ComboxItem;
			if (item == null) return;

			var layout = new List<WindowLayout>();

			switch (item.Value)
			{
				case "PTZ":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_1, Properties.Resources.IMGLayout1);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_1s_use, Properties.Resources.IMGLayout1WithSource_use);
					layout = WindowLayouts.LayoutGenerate("**");
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "**";

					break;
				case "Quad":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_4, Properties.Resources.IMGLayout4);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_4s_use, Properties.Resources.IMGLayout4WithSource_use);
					layout = WindowLayouts.LayoutGenerate("11**,11**");
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "11**,11**";
					break;
                case "Perimeter":
					splitslabel.Image = Resources.GetResources(Properties.Resources.layout_2, Properties.Resources.IMGLayout2);
					splitswithsourcelabel.Image = Resources.GetResources(Properties.Resources.layout_2s_use, Properties.Resources.IMGLayout2WithSource_use);
					layout = WindowLayouts.LayoutGenerate("1*,1*");
					_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(layout));

					DeviceLayout.DefineLayout = "1*,1*";
					break;
			}

			DeviceLayout.isIncludeDevice = true;

			var newSubLayout = new Dictionary<UInt16, ISubLayout>();

			var sl99 = new SubLayout
			{
				DeviceLayout = DeviceLayout,
				Id = 99,
				Name = "Sub-Layout 00",
				X = 0,
				Y = 0,
				Width = DeviceLayout.Width,
				Height = DeviceLayout.Height,
				Server = Server,
			};

			newSubLayout[sl99.Id] = sl99;

			foreach (var subLayout in DeviceLayout.SubLayouts)
			{
				if (subLayout.Key == 99) continue;
				newSubLayout[subLayout.Key] = subLayout.Value;
			}

			DeviceLayout.SubLayouts = newSubLayout;
			
			ShowMonitor();
		}

        private void SubLayoutDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            var count = DeviceLayout.Items.Count(device => device != null);
            //no device
            if (count == 0) return;

            if (OnSubDeviceLayoutEdit != null)
				OnSubDeviceLayoutEdit(this, new EventArgs<IDeviceLayout>(DeviceLayout));
        }

		private void GenerateSubLayout()
		{
			IDeviceLayout dl1 = new DeviceLayout
					{
						Id = DeviceLayout.Id,
						Server = Server,
						LayoutX = 1,
						LayoutY = 1,
						WindowWidth = DeviceLayout.Width,
						WindowHeight = DeviceLayout.Height,
						Items = new List<IDevice> { deviceComboBox.SelectedItem as IDevice, null },
						Dewarps = new List<bool> { dewarpCheckBox.Checked, false },
						MountingType = DeviceLayout.MountingType,
					};

			var item = lensSettingComboBox.SelectedItem as ComboxItem;
			if (item != null)
			{
				switch (item.Value)
				{
					case "PTZ":
						{
							if (DeviceLayout.isIncludeDevice)
							{
								var sl99 = new SubLayout
									{

										DeviceLayout = dl1,
										Id = 99,
										Name = "Sub-Layout 00",
										X = 0,
										Y = 0,
										Width = DeviceLayout.Width,
										Height = DeviceLayout.Height,
										Server = Server,
									};

								dl1.SubLayouts[sl99.Id] = sl99;
							}

							var sl1 = new SubLayout
								{
									DeviceLayout = dl1,
									Id = 1,
									Name = "Sub-Layout 1",
									X = 0,
									Y = 0,
									Width = DeviceLayout.Width,
									Height = DeviceLayout.Height,
									Server = Server,
								};

							dl1.SubLayouts[sl1.Id] = sl1;
						}
						break;
					case "Quad":
						{
							if (DeviceLayout.isIncludeDevice)
							{
								var sl99 = new SubLayout
									{

										DeviceLayout = dl1,
										Id = 99,
										Name = "Sub-Layout 00",
										X = 0,
										Y = 0,
										Width = DeviceLayout.Width,
										Height = DeviceLayout.Height,
										Server = Server,
									};

								dl1.SubLayouts[sl99.Id] = sl99;
							}
							var sl1 = new SubLayout
								{

									DeviceLayout = dl1,
									Id = 1,
									Name = "Sub-Layout 1",
									X = 0,
									Y = 0,
									Width = DeviceLayout.Width / 2,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl1.Id] = sl1;

							var sl2 = new SubLayout
								{

									DeviceLayout = dl1,
									Id = 2,
									Name = "Sub-Layout 2",
									X = DeviceLayout.Width / 2,
									Y = 0,
									Width = DeviceLayout.Width / 2,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl2.Id] = sl2;

							var sl3 = new SubLayout
								{

									DeviceLayout = dl1,
									Id = 3,
									Name = "Sub-Layout 3",
									X = 0,
									Y = DeviceLayout.Height / 2,
									Width = DeviceLayout.Width / 2,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl3.Id] = sl3;

							var sl4 = new SubLayout
								{

									DeviceLayout = dl1,
									Id = 4,
									Name = "Sub-Layout 4",
									X = DeviceLayout.Width / 2,
									Y = DeviceLayout.Height / 2,
									Width = DeviceLayout.Width / 2,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl4.Id] = sl4;
						}
						break;
                    case "Perimeter":
						{
							if (DeviceLayout.isIncludeDevice)
							{
								var sl99 = new SubLayout
									{

										DeviceLayout = dl1,
										Id = 99,
										Name = "Sub-Layout 00",
										X = 0,
										Y = 0,
										Width = DeviceLayout.Width,
										Height = DeviceLayout.Height,
										Server = Server,
									};

								dl1.SubLayouts[sl99.Id] = sl99;
							}

							var sl1 = new SubLayout
								{
									DeviceLayout = dl1,
									Id = 1,
									Name = "Sub-Layout 1",
									X = 0,
									Y = 0,
									Width = DeviceLayout.Width,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl1.Id] = sl1;

							var sl2 = new SubLayout
								{

									DeviceLayout = dl1,
									Id = 2,
									Name = "Sub-Layout 2",
									X = 0,
									Y = DeviceLayout.Height / 2,
									Width = DeviceLayout.Width,
									Height = DeviceLayout.Height / 2,
									Server = Server,
								};
							dl1.SubLayouts[sl2.Id] = sl2;
						}
						break;
				}
			}
			DeviceLayout.SubLayouts = dl1.SubLayouts;
		}

        private Boolean _isEditLayout;
        private void ShowSnapshot()
        {
            _isEditLayout = false;

            _monitor.ClearAll();
            var group = new DeviceGroup {Name = DeviceLayout.Name};

			group.Layout = WindowLayouts.LayoutGenerate(DeviceLayout.DefineLayout);

			_monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(group.Layout));

	        if ( (DeviceLayout.Items.Count >= 1) && (DeviceLayout.Items[0] != null) )
				_monitor.ShowGroup(DeviceLayout);

	        _isEditLayout = true;
        }

        private void MonitorOnContentChange(Object sender, EventArgs<Object> e)
        {
            if (!_isEditLayout) return;

			//var devices = e.Value as IDevice[];
			//if(devices == null) return;

			//DeviceLayout.Items.Clear();
			//DeviceLayout.Items.AddRange(devices);

			//while (DeviceLayout.Items.Count < 4)
			//    DeviceLayout.Items.Add(null);

            _isEdit = false;

            var count = DeviceLayout.Items.Count;
            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    if (DeviceLayout.Items[0] != null)
                        deviceComboBox.SelectedItem = DeviceLayout.Items[0];
                    else
                        deviceComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 1)
                {
                    if (DeviceLayout.Items[1] != null)
                        mountingTypeComboBox.SelectedItem = DeviceLayout.Items[1];
                    else
                        mountingTypeComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 2)
                {
                    if (DeviceLayout.Items[2] != null)
                        lensSettingComboBox.SelectedItem = DeviceLayout.Items[2];
                    else
                        lensSettingComboBox.SelectedIndex = -1;
                    continue;
                }
            }

            _nvr.DeviceLayoutModify(DeviceLayout);
            UpdateDewarpSelection();

            _isEdit = true;
        }

		private void DewarpCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (!_isEdit) return;

			mountingTypeComboBox.Enabled = dewarpCheckBox.Checked;
			DeviceLayout.Dewarps[0] = dewarpCheckBox.Checked;

			foreach (KeyValuePair<ushort, ISubLayout> keyValuePair in DeviceLayout.SubLayouts)
			{
			    keyValuePair.Value.Dewarp = dewarpCheckBox.Checked ? 1 : 0;
				keyValuePair.Value.DeviceLayout.Dewarps = new List<bool> { dewarpCheckBox.Checked };
			}

			ShowMonitor();
		}

	    private void UpdateDewarpSelection()
	    {
		   dewarpCheckBox.Visible = false;
			mountingTypeComboBox.Enabled = dewarpCheckBox.Checked = false;
 
		    ICamera camera;
		    var count = DeviceLayout.Items.Count;
		    for (var i = 0; i < count; i++)
		    {
			    if (i == 0)
			    {
				    if (DeviceLayout.Items[i] != null)
				    {
						if (DeviceLayout.Items[i] is ISubLayout) continue;

					    camera = (ICamera) DeviceLayout.Items[i];
					    if (!String.IsNullOrEmpty(camera.Profile.DewarpType))
					    {
						    dewarpCheckBox.Visible = true;
						    if (DeviceLayout.Dewarps.Count > i)
						    {
								mountingTypeComboBox.Enabled = dewarpCheckBox.Checked = DeviceLayout.Dewarps[i];
						    }
							    
					    }
				    }
			    }
		    }
	    }

	    private Boolean _isEdit;
        public void ParseDeviceLayout()
        {
            if (DeviceLayout == null) return;

            _isEdit = false;

            nameTextBox.Text = DeviceLayout.Name;
//            layoutComboBox.SelectedItem = DeviceLayout.LayoutX + "x" + DeviceLayout.LayoutY;

            deviceComboBox.Items.Clear();
            mountingTypeComboBox.Items.Clear();
            lensSettingComboBox.Items.Clear();

			var sortResult = new List<IDevice>(Server.Device.Devices.Values);
			sortResult.Sort((x, y) => (x.Id - y.Id));

			foreach (var camera in sortResult)
			{
				if (!(camera is ICamera)) continue;

				deviceComboBox.Items.Add(camera);

				if (DeviceLayout.Items.Count >= 1)
					if (camera == DeviceLayout.Items[0])
						deviceComboBox.SelectedItem = camera;
			}

	        var item = new ComboxItem(Localization["DeviceLayoutPanel_Ceiling"], "Ceiling");
			mountingTypeComboBox.Items.Add(item);
	        if (item.Value == MountingTypes.ToString(DeviceLayout.MountingType))
		        mountingTypeComboBox.SelectedItem = item;

	        item = new ComboxItem(Localization["DeviceLayoutPanel_Ground"], "Ground");
			mountingTypeComboBox.Items.Add(item);
			if (item.Value == MountingTypes.ToString(DeviceLayout.MountingType))
				mountingTypeComboBox.SelectedItem = item;

	        item = new ComboxItem(Localization["DeviceLayoutPanel_Wall"], "Wall");
			mountingTypeComboBox.Items.Add(item);
			if (item.Value == MountingTypes.ToString(DeviceLayout.MountingType))
				mountingTypeComboBox.SelectedItem = item;

	        item = new ComboxItem(Localization["DeviceLayoutPanel_PTZ"], "PTZ");
			lensSettingComboBox.Items.Add(item);
			if (item.Value == LensSettings.ToString(DeviceLayout.LensSetting))
				lensSettingComboBox.SelectedItem = item;

	        item = new ComboxItem(Localization["DeviceLayoutPanel_Quad"], "Quad");
			lensSettingComboBox.Items.Add(item);
			if (item.Value == LensSettings.ToString(DeviceLayout.LensSetting))
				lensSettingComboBox.SelectedItem = item;

            item = new ComboxItem(Localization["DeviceLayoutPanel_Perimeter"], "Perimeter");
			lensSettingComboBox.Items.Add(item);
			if (item.Value == LensSettings.ToString(DeviceLayout.LensSetting))
				lensSettingComboBox.SelectedItem = item;

			Manager.DropDownWidth(deviceComboBox);
			Manager.DropDownWidth(mountingTypeComboBox);
			Manager.DropDownWidth(lensSettingComboBox);

			deviceComboBox.Enabled = (deviceComboBox.Items.Count > 0);
			mountingTypeComboBox.Enabled = true;
			lensSettingComboBox.Enabled = true;

			if (DeviceLayout.Dewarps.Count >= 1)
			{
				mountingTypeComboBox.Enabled = dewarpCheckBox.Checked = DeviceLayout.Dewarps[0];
			}

	        if (DeviceLayout.isIncludeDevice)
		        SplitswithsourcelabelClick(this, EventArgs.Empty);
			else
				SplitslabelClick(this, EventArgs.Empty);

			_isEdit = false;
            //UpdateResolution();
			UpdateDewarpSelection();
            //ShowSnapshot();

            subLayoutDoubleBufferPanel.Invalidate();
            if (DeviceLayout.Items.Count(device => device != null) == 0)
                subLayoutDoubleBufferPanel.Cursor = Cursors.Default;
            else
                subLayoutDoubleBufferPanel.Cursor = Cursors.Hand;

            _isEdit = true;

            containerPanel.Focus();
        }

        private void UpdateResolution()
        {
            resolutionComboBox.Items.Clear();
            var resolutions = new List<Resolution> { Resolution.R640X480 };

            foreach (var device in DeviceLayout.Items)
            {
                var camera = device as ICamera;
				if (camera is ISubLayout) continue;

                if (camera != null && !resolutions.Contains(camera.StreamConfig.Resolution))
                {
                    resolutions.Add(camera.StreamConfig.Resolution);
                }
            }

            resolutions.Sort();
            foreach (var resolution in resolutions)
            {
                resolutionComboBox.Items.Add(Resolutions.ToString(resolution));
            }
            var deviceResolution = Resolutions.ToIndex(DeviceLayout.WindowWidth + "x" + DeviceLayout.WindowHeight);
            if (resolutions.Contains(deviceResolution))
                resolutionComboBox.SelectedItem = Resolutions.ToString(deviceResolution);
            else
            {
                DeviceLayout.WindowWidth = 640;
                DeviceLayout.WindowHeight = 480;
                resolutionComboBox.SelectedItem = Resolutions.ToString(Resolution.R640X480);
            }
        }
    } 
}
