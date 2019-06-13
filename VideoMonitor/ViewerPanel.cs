using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
	public partial class ViewerPanel : UserControl, IControl, IAppUse, IServerUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;

		protected readonly PanelTitleBar PanelTitleBar = new PanelTitleBar();

		public String TitleName
		{
			get
			{
				return PanelTitleBar.Text;
			}
			set
			{
				PanelTitleBar.Text = value;
			}
		}

		public virtual Image Icon { get; set; }

		public IApp App { get; set; }
		protected INVR NVR;
		protected ICMS CMS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is INVR)
					NVR = value as INVR;
				if (value is ICMS)
					CMS = value as ICMS;
			}
		}

		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }

		public ViewerPanel()
		{
			Icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
		}

		public void Initialize()
		{
			InitializeComponent();

			Dock = DockStyle.Fill;
			PanelTitleBar.Panel = this;
			titlePanel.Controls.Add(PanelTitleBar);
		}

		public virtual void Activate()
		{
		}

		public void Deactivate()
		{
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

		public void LoadDevice(Object sender, EventArgs<String> e)
		{
			//"<XML><Channel>1</Channel><Timestamp>0</Timestamp></XML>"
			var channel = Convert.ToUInt16(Xml.GetFirstElementsValueByTagName(Xml.LoadXml(e.Value), "Channel"));
			var device = NVR.Device.FindDeviceById(channel) as ICamera;

			videoWindow.Device = device;
			videoWindow.Play();
		}
	}
}
