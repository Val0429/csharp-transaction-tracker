using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Device;
using Interface;
using PanelBase;

namespace UserDeviceGroup
{
	public partial class Save : UserControl, IControl, IServerUse, IAppUse
	{
		public String TitleName { get; set; }

		public IApp App { get; set; }

		private ICMS _cms;
		private INVR _nvr;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set { 
				_server = value;

				if (value is INVR)
					_nvr = value as INVR;
				if (value is ICMS)
					_cms = value as ICMS;
			}
		}

		public Dictionary<String, String> Localization;
		private readonly ToolTip _toolTip = new ToolTip();
		
		public Save()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Save_Button", "Save Settings"},
								   {"MessageBox_Information", "Information"},
								   {"SetupDeviceGroup_NewGroup", "New Group"},
							   };
			Localizations.Update(Localization);
		}

		public virtual void Initialize()
		{
			InitializeComponent();
			Dock = DockStyle.Bottom;
			saveButton.Image = Resources.GetResources(Properties.Resources.save, Properties.Resources.IMGSave);

			_toolTip.SetToolTip(saveButton, Localization["Save_Button"]);

			DisplayEditPanel(this, EventArgs.Empty);
		}

		private Boolean _visible = true;
		public void DisplayEditPanel(Object sender, EventArgs e)
		{
			_visible = !_visible;

			Visible = _visible;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		private List<WindowLayout> _layout = WindowLayouts.LayoutGenerate(4);
		public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
		{
			if (e.Value == null) return;

			_layout = e.Value;
		}

		protected List<IDevice> UsingDevices = new List<IDevice>();
		public void ContentChange(Object sender, EventArgs<Object> e)
		{
			UsingDevices.Clear();
			if (e.Value is IDevice)
			{
				UsingDevices.Add(e.Value as IDevice);
			}
			else if (e.Value is IDevice[])
			{
				UsingDevices.AddRange(e.Value as IDevice[]);
			}
		}

		private void SaveButtonMouseClick(Object sender, MouseEventArgs e)
		{
			IDeviceGroup deviceGroup = new DeviceGroup();

			if (_nvr == null) return;

			//default is private view
			deviceGroup.Id = _nvr.User.Current.GetNewGroupId();
			deviceGroup.Name = Localization["SetupDeviceGroup_NewGroup"] + @" " + deviceGroup.Id;

			var inputForm = new GroupNameInputForm
			{
				DeviceGroup = deviceGroup,
				Server = _nvr
			};

			var result = inputForm.ShowDialog();

			if (result != DialogResult.OK) return;
			if (String.IsNullOrEmpty(deviceGroup.Name) || deviceGroup.Id == 0 ) return;

			deviceGroup.View.AddRange(UsingDevices.ToArray());
			foreach (var device in deviceGroup.View)
			{
				if (device == null) continue;
				if(deviceGroup.Items.Contains(device)) continue;

				deviceGroup.Items.Add(device);
			}
			deviceGroup.Items.Sort((x, y) => (x.Id - y.Id));

			deviceGroup.Layout.Clear();
			deviceGroup.Layout.AddRange(_layout);

			App.SaveUserDefineDeviceGroup(deviceGroup);
		}
	}
}