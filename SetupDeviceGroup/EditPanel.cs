using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class EditPanel : UserControl
    {
        public IServer Server;
        public IDeviceGroup Group;
        public Dictionary<String, String> Localization;

        public event EventHandler OnDeviceSelectionChange;
        public event EventHandler<EventArgs<IDeviceGroup>> OnGroupLayoutEdit;
        
        private GroupDevicePanel _deviceGroupPanel;

        private List<String> _modifyed = new List<string>();

        public EditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupDeviceGroup_SetGroupLayout", "Setting group layout"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            _deviceGroupPanel = new GroupDevicePanel
            {
                Server = Server,
                EditVisible = false,
                SelectionVisible = false,
                CustomGroupPanelContainer = groupPanel,
            };

            _deviceGroupPanel.OnDeviceSelectionChange += DeviceGroupPanelOnDeviceSelectionChange;

            containerPanel.Controls.Add(_deviceGroupPanel);
            setLayoutDoubleBufferPanel.Paint += InputPanelPaint;
            setLayoutDoubleBufferPanel.MouseClick += SetLayoutDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, setLayoutDoubleBufferPanel);
            Manager.PaintEdit(g, setLayoutDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupDeviceGroup_SetGroupLayout"]);
        }

        private void SetLayoutDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnGroupLayoutEdit != null)
                OnGroupLayoutEdit(this, new EventArgs<IDeviceGroup>(Group));
        }

        private void DeviceGroupPanelOnDeviceSelectionChange(Object sender, EventArgs e)
        {
            List<IDevice> devices = _deviceGroupPanel.DeviceSelection;

            Boolean isChange = false;
            foreach (IDevice device in devices)
            {
                if (Group.Items.Contains(device)) continue;

                isChange = true;
                break;
            }

            if(!isChange)
            {
                if (Group.Items.Any(device => !devices.Contains(device)))
                {
                    isChange = true;
                }
            }

            if (!isChange) return;
            Group.Items.Clear();
            Group.Items.AddRange(devices);

            Group.View.Clear();
            Group.View.AddRange(devices);

            Group.Layout = null;

            Server.GroupModify(Group);

            if (OnDeviceSelectionChange != null)
                OnDeviceSelectionChange(this, null);
        }

        public void ParseGroup()
        {
            ClearViewModel();

            if (Group == null) return;

            _deviceGroupPanel.Group = Group;
            _deviceGroupPanel.EditVisible = false;

            _deviceGroupPanel.ShowDevicesWithSelection();

            containerPanel.Focus();
        }

        private void ClearViewModel()
        {
            _deviceGroupPanel.ClearViewModel();
            _deviceGroupPanel.Group = null;
        }
    } 
}
