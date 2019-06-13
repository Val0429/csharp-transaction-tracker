using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IDeviceGroup>> OnDeviceGroupEdit;
        public event EventHandler OnDeviceGroupAdd;

        public IServer Server;
        public Dictionary<String, String> Localization;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupDeviceGroup_AddNewGroup", "Add new group..."},
                                   {"SetupDeviceGroup_AddedGroup", "Added group"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
            addedGroupLabel.Text = Localization["SetupDeviceGroup_AddedGroup"];
        }

        public void Initialize()
        {
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupDeviceGroup_AddNewGroup"]);
        }

        private readonly Queue<GroupDevicePanel> _recycleGroup = new Queue<GroupDevicePanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();
            addedGroupLabel.Visible = addNewDoubleBufferPanel.Visible = true;

            if (Server.Device.Groups == null) return;

            var sortResult = new List<IDeviceGroup>(Server.Device.Groups.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedGroupLabel.Visible = false;
                return;
            }

            if (sortResult.Count == 1 && !(Server is ICMS))
            {
                addedGroupLabel.Visible = false;
                return;
            }

            addedGroupLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IDeviceGroup group in sortResult)
            {
                if (group == null) continue;
                if (group.Id == 0) continue;//dont show all device group

                GroupDevicePanel deviceGroupPanel = GetDeviceGroupPanel();

                deviceGroupPanel.Group = group;
                deviceGroupPanel.EditVisible = true;

                deviceGroupPanel.ShowDevices();
                containerPanel.Controls.Add(deviceGroupPanel);
            }
            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (GroupDevicePanel deviceGroupControl in containerPanel.Controls)
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
                EditVisible = true,
            };

            deviceGroupPanel.OnDeviceGroupEditClick += DeviceGroupControlOnDeviceGroupEditClick;

            return deviceGroupPanel;
        }

        public void RemoveSelectedGroups()
        {
            var groups = new List<String>();

            foreach (GroupDevicePanel deviceGroupControl in containerPanel.Controls)
            {
                if (!deviceGroupControl.Checked) continue;

                Server.GroupModify(deviceGroupControl.Group);
                Server.Device.Groups.Remove(deviceGroupControl.Group.Id);
                groups.Add(deviceGroupControl.Group.Id.ToString());
            }

            Server.WriteOperationLog("Remove Group %1".Replace("%1", String.Join(",", groups.ToArray())));
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceGroupAdd != null)
                OnDeviceGroupAdd(this, e);
        }

        private void DeviceGroupControlOnDeviceGroupEditClick(Object sender, EventArgs e)
        {
            if (OnDeviceGroupEdit != null)
                OnDeviceGroupEdit(this, new EventArgs<IDeviceGroup>(((GroupDevicePanel)sender).Group));
        }

        public void ShowGroup()
        {
            addedGroupLabel.Visible = addNewDoubleBufferPanel.Visible = false;

            foreach (GroupDevicePanel deviceGroupControl in containerPanel.Controls)
            {
                deviceGroupControl.ShowGroup();
                deviceGroupControl.EditVisible = false;
            }

        }

        private void ClearViewModel()
        {
            foreach (GroupDevicePanel deviceGroupControl in containerPanel.Controls)
            {
                deviceGroupControl.ClearViewModel();
                deviceGroupControl.Group = null;
                if (!_recycleGroup.Contains(deviceGroupControl))
                    _recycleGroup.Enqueue(deviceGroupControl);
            }

            containerPanel.Controls.Clear();
        }
    } 
}
