using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupDeviceGroup;

namespace UserDeviceGroup
{
	public sealed partial class ListPanel : UserControl
	{
		public IServer Server;
		public Dictionary<String, String> Localization;

		public ListPanel()
		{
			Localization = new Dictionary<String, String>
                               {
                                   {"SetupDeviceGroup_AddNewGroup", "Add new group..."},
                                   {"SetupDeviceGroup_PublicView", "Public views"},
								   {"SetupDeviceGroup_PrivateView", "Private views"},
                                   {"SetupDeviceGroup_RemoveGroups", "Remove Group %1"},
                               };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
			publicGroupLabel.Text = Localization["SetupDeviceGroup_PublicView"];
			privateGroupLabel.Text = Localization["SetupDeviceGroup_PrivateView"];
		}

		public void Initialize()
		{
		}

		private readonly Queue<GroupDevicePanel> _recycleGroup = new Queue<GroupDevicePanel>();

		private Point _publicScrollPosition;
		private Point _privateScrollPosition;

		public void GenerateViewModel()
		{
			_publicScrollPosition = publicContainerPanel.AutoScrollPosition;
			_publicScrollPosition.Y *= -1;

			_privateScrollPosition = privateContainerPanel.AutoScrollPosition;
			_privateScrollPosition.Y *= -1;

			ClearViewModel();

			var sortResult = new List<IDeviceGroup>();

			if ((Server.User.Current.Group.Name == "Administrator") || (Server.User.Current.Group.Name == "Superuser"))
			{
				publicGroupLabel.Visible = true;
				publicContainerPanel.Visible = true;

				sortResult = new List<IDeviceGroup>(Server.Device.Groups.Values);
				sortResult.Sort((x, y) => (y.Id - x.Id));

				publicContainerPanel.Visible = false;
				foreach (IDeviceGroup group in sortResult)
				{
					if (group == null) continue;
					if (group.Id == 0) continue; //dont show all device group

					GroupDevicePanel deviceGroupPanel = GetDeviceGroupPanel();

					deviceGroupPanel.Group = group;
					deviceGroupPanel.EditVisible = true;

					deviceGroupPanel.ShowDevices();
					publicContainerPanel.Controls.Add(deviceGroupPanel);
				}

				publicContainerPanel.Visible = true;
				publicContainerPanel.AutoScrollPosition = _publicScrollPosition;
			}
			else
			{
				publicGroupLabel.Visible = false;
				publicContainerPanel.Visible = false;
			}

			if (Server.User.Current.DeviceGroups.Count == 0) return;

			sortResult.Clear();
			sortResult = new List<IDeviceGroup>(Server.User.Current.DeviceGroups.Values);
			sortResult.Sort((x, y) => (x.Id - y.Id));

			foreach (IDeviceGroup group in sortResult)
			{
				if (group == null) continue;

				GroupDevicePanel deviceGroupPanel = GetDeviceGroupPanel();

				deviceGroupPanel.Group = group;
				deviceGroupPanel.EditVisible = true;

				deviceGroupPanel.ShowDevices();
				privateContainerPanel.Controls.Add(deviceGroupPanel);
				privateContainerPanel.Controls.SetChildIndex(deviceGroupPanel, 0);
			}

			privateContainerPanel.Visible = true;
			privateContainerPanel.AutoScrollPosition = _privateScrollPosition;

			publicContainerPanel.Focus();
		}

		public Boolean SelectionVisible
		{
			set
			{
				foreach (GroupDevicePanel deviceGroupControl in publicContainerPanel.Controls)
					deviceGroupControl.SelectionVisible = value;

				foreach (GroupDevicePanel deviceGroupControl in privateContainerPanel.Controls)
					deviceGroupControl.SelectionVisible = value;
			}
		}

		private GroupDevicePanel GetDeviceGroupPanel()
		{
			if (_recycleGroup.Count > 0)
			{
				return _recycleGroup.Dequeue();
			}

			var deviceGroupPanel = new GroupDevicePanel
			{
				Server = Server,
				EditVisible = false,
			};

			//deviceGroupPanel.OnDeviceGroupEditClick += DeviceGroupControlOnDeviceGroupEditClick;

			return deviceGroupPanel;
		}

		public void RemoveSelectedGroups()
		{
			var groups = new List<String>();

			foreach (GroupDevicePanel deviceGroupControl in publicContainerPanel.Controls)
			{
				if (!deviceGroupControl.Checked) continue;

				Server.GroupModify(deviceGroupControl.Group);
				Server.Device.Groups.Remove(deviceGroupControl.Group.Id);
				groups.Add(deviceGroupControl.Group.Id.ToString());
			}

			foreach (GroupDevicePanel deviceGroupControl in privateContainerPanel.Controls)
			{
				if (!deviceGroupControl.Checked) continue;

				Server.GroupModify(deviceGroupControl.Group);
				Server.User.Current.DeviceGroups.Remove(deviceGroupControl.Group.Id);
				groups.Add(deviceGroupControl.Group.Id.ToString());
			}

			Server.WriteOperationLog(Localization["SetupDeviceGroup_RemoveGroups"].Replace("%1", String.Join(",", groups.ToArray())));
		}

		//private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		//{
		//    if (OnDeviceGroupAdd != null)
		//        OnDeviceGroupAdd(this, e);
		//}

		//private void DeviceGroupControlOnDeviceGroupEditClick(Object sender, EventArgs e)
		//{
		//    if (OnDeviceGroupEdit != null)
		//        OnDeviceGroupEdit(this, new EventArgs<IDeviceGroup>(((GroupDevicePanel)sender).Group));
		//}

		public void ShowGroup()
		{
			//addedGroupLabel.Visible = addNewDoubleBufferPanel.Visible = false;

			foreach (GroupDevicePanel deviceGroupControl in publicContainerPanel.Controls)
			{
				deviceGroupControl.ShowGroup();
				deviceGroupControl.EditVisible = false;
			}

			foreach (GroupDevicePanel deviceGroupControl in privateContainerPanel.Controls)
			{
				deviceGroupControl.ShowGroup();
				deviceGroupControl.EditVisible = false;
			}
		}

		private void ClearViewModel()
		{
			foreach (GroupDevicePanel deviceGroupControl in publicContainerPanel.Controls)
			{
				deviceGroupControl.ClearViewModel();
				deviceGroupControl.Group = null;
				if (!_recycleGroup.Contains(deviceGroupControl))
					_recycleGroup.Enqueue(deviceGroupControl);
			}

			publicContainerPanel.Controls.Clear();

			foreach (GroupDevicePanel deviceGroupControl in privateContainerPanel.Controls)
			{
				deviceGroupControl.ClearViewModel();
				deviceGroupControl.Group = null;
				if (!_recycleGroup.Contains(deviceGroupControl))
					_recycleGroup.Enqueue(deviceGroupControl);
			}

			privateContainerPanel.Controls.Clear();
		}
	}
}
