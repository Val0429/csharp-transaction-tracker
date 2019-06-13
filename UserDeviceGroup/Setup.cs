using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace UserDeviceGroup
{
	public partial class Setup : UserControl, IControl, IMinimize, IServerUse, IAppUse
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<Boolean>> OnPanelVisibleChange;

		public event EventHandler<EventArgs<String>> OnUserDefineDeviceGroupModify;
		public event EventHandler OnUserDefineDeviceGroupDelete;

		public IApp App { get; set; }

		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
			}
		}

		protected readonly PanelTitleBar PanelTitleBar = new PanelTitleBar();
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }
		public virtual Image Icon { get; set; }

		public virtual void Activate()
		{
			_listPanel.GenerateViewModel();
		}
		public virtual void Deactivate() { }

		public Boolean IsMinimize { get; private set; }
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

		public UInt16 MinimizeHeight
		{
			get
			{
				if (_visible)
					return (UInt16) titlePanel.Size.Height;
				else
					return 0;
			}
		}

		private ListPanel _listPanel;

		private Button _deleteButton;
		private Button _cancelButton;
		private Button _confirmButton; 

		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_DeviceGroup", "Group"},
								   {"SetupTitle_Delete", "Delete"},
								   {"SetupTitle_Cancel", "Cancel"},
								   {"SetupTitle_Confirm", "Confirm"},
							   };
			Localizations.Update(Localization);
		}

		public virtual void Initialize()
		{
			InitializeComponent();

			Dock = DockStyle.Fill;

			PanelTitleBar.Text = TitleName = Localization["Control_DeviceGroup"];
			PanelTitleBar.Panel = this;
			titlePanel.Controls.Add(PanelTitleBar);

			DisplayEditPanel(this, EventArgs.Empty);

			_deleteButton = new Button
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Text = Localization["SetupTitle_Delete"],
				ForeColor = Color.Black,
				Size = new Size(80, 23),
				BackColor = Color.Transparent,
			};
			_deleteButton.MouseClick += DeleteButtonMouseClick;
			PanelTitleBar.Controls.Add(_deleteButton);

			_cancelButton = new Button
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Text = Localization["SetupTitle_Cancel"],
				ForeColor = Color.Black,
				Size = new Size(80, 23),
				BackColor = Color.Transparent,
			};
			_cancelButton.MouseClick += CancelButtonMouseClick;
			_cancelButton.Visible = false;
			PanelTitleBar.Controls.Add(_cancelButton);
			

			_confirmButton = new Button
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Text = Localization["SetupTitle_Confirm"],
				ForeColor = Color.Black,
				Size = new Size(80, 23),
				BackColor = Color.Transparent,
			};
			_confirmButton.MouseClick += ConfirmButtonMouseClick;
			_confirmButton.Visible = false;
			PanelTitleBar.Controls.Add(_confirmButton);
			

			_listPanel = new ListPanel
			{
				//App = App,
				Server = Server,
			};
			_listPanel.Initialize();
			_listPanel.GenerateViewModel();
			_listPanel.SelectionVisible = false;
			//_listPanel.OnUserDefineDeviceGroupModify += _listPanel_OnUserDefineDeviceGroupModify;
			//_listPanel.OnUserDefineDeviceGroupDelete += _listPanel_OnUserDefineDeviceGroupDelete;

			viewModelPanel.Controls.Add(_listPanel);

			App.OnUserDefineDeviceGroupModify += AppOnUserDefineDeviceGroupModify;
		}

		private void ConfirmButtonMouseClick(Object sender, MouseEventArgs e)
		{
			_listPanel.RemoveSelectedGroups();
			_listPanel.GenerateViewModel();

			_deleteButton.Visible = true;
			_cancelButton.Visible = false;
			_confirmButton.Visible = false;

			Server.Device.Save();
			Server.User.Save();

			if (OnUserDefineDeviceGroupDelete != null)
				OnUserDefineDeviceGroupDelete(this, null);
		}

		private void CancelButtonMouseClick(Object sender, MouseEventArgs e)
		{
			_listPanel.SelectionVisible = false;
			_listPanel.GenerateViewModel();

			_deleteButton.Visible = true;
			_cancelButton.Visible = false;
			_confirmButton.Visible = false;
		}

		private void DeleteButtonMouseClick(Object sender, MouseEventArgs e)
		{
			_listPanel.SelectionVisible = true;
			_listPanel.ShowGroup();

			_deleteButton.Visible = false;
			_cancelButton.Visible = true;
			_confirmButton.Visible = true;
		}

		private void AppOnUserDefineDeviceGroupModify(Object sender, EventArgs e)
		{
			_listPanel.GenerateViewModel();
		}

		private Boolean _visible = true;

		public void DisplayEditPanel(Object sender, EventArgs e)
		{
			_visible = !_visible;

			Parent.Visible = Visible = _visible;

			if (OnPanelVisibleChange != null)
				OnPanelVisibleChange(this, new EventArgs<bool>(Visible));

			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}
	}
}
