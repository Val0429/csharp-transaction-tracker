using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace PosRegister
{
	public partial class RegisterStatus : UserControl, IControl, IServerUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();

		public String TitleName
		{
			get
			{
				return _panelTitleBar.Text;
			}
			set
			{
				_panelTitleBar.Text = value;
			}
		}
		public Image Icon
		{
			get
			{
				return Properties.Resources.icon;
			}
		}

		public void Activate() { }

		public void Deactivate() { }

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

		public void Initialize()
		{
			InitializeComponent();
			Dock = DockStyle.Fill;

			_panelTitleBar.Panel = this;
			titlePanel.Controls.Add(_panelTitleBar);

			NVR.OnEventReceive -= EventReceive;
			NVR.OnEventReceive += EventReceive;
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

		public virtual void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (e.Value == null) return;

			foreach (ICameraEvent cameraEvent in e.Value)
			{
				if (cameraEvent.Device == null) continue;

				if (cameraEvent.Type == EventType.UserDefine)
				{
					transactionTextBox.Text += cameraEvent.Status ;
					transactionTextBox.Text += Environment.NewLine;

					transactionTextBox.SelectionStart = transactionTextBox.Text.Length;
					transactionTextBox.ScrollToCaret();
					transactionTextBox.Refresh();
 
					lastUpdateLabel.Text = cameraEvent.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
				}
			}
		}
	}
}
