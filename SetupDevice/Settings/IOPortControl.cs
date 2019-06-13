using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;

namespace SetupDevice
{
    public sealed partial class IOPortControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        private Dictionary<UInt16, IOPort> _ioPort;

        public IOPortControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_IOPortConfig", "I/O Port Config"},
                                   {"DevicePanel_IOPort", "I/O Port %1"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += IOPortControlPaint;

            port1Panel.Paint += PaintInput;
            port2Panel.Paint += PaintInput;
            port3Panel.Paint += PaintInput;
            port4Panel.Paint += PaintInput;
            port5Panel.Paint += PaintInput;
            port6Panel.Paint += PaintInput;
            port7Panel.Paint += PaintInput;
            port8Panel.Paint += PaintInput;

            port1ComboBox.SelectedIndexChanged += Port1ComboBoxSelectedIndexChanged;
            port2ComboBox.SelectedIndexChanged += Port2ComboBoxSelectedIndexChanged;
            port3ComboBox.SelectedIndexChanged += Port3ComboBoxSelectedIndexChanged;
            port4ComboBox.SelectedIndexChanged += Port4ComboBoxSelectedIndexChanged;
            port5ComboBox.SelectedIndexChanged += Port5ComboBoxSelectedIndexChanged;
            port6ComboBox.SelectedIndexChanged += Port6ComboBoxSelectedIndexChanged;
            port7ComboBox.SelectedIndexChanged += Port7ComboBoxSelectedIndexChanged;
            port8ComboBox.SelectedIndexChanged += Port8ComboBoxSelectedIndexChanged;
        }

        public void ParseDevice()
        {
            if (EditPanel.Camera.Model.IOPortSupport == null) return;

            port1ComboBox.Items.Clear();
            port2ComboBox.Items.Clear();
            port3ComboBox.Items.Clear();
            port4ComboBox.Items.Clear();
            port5ComboBox.Items.Clear();
            port6ComboBox.Items.Clear();
            port7ComboBox.Items.Clear();
            port8ComboBox.Items.Clear();

            foreach (IOPort port in EditPanel.Camera.Model.IOPortSupport)
            {
                var str = IOPorts.ToString(port);
                port1ComboBox.Items.Add(str);
                port2ComboBox.Items.Add(str);
                port3ComboBox.Items.Add(str);
                port4ComboBox.Items.Add(str);
                port5ComboBox.Items.Add(str);
                port6ComboBox.Items.Add(str);
                port7ComboBox.Items.Add(str);
                port8ComboBox.Items.Add(str);
            }

            _ioPort = EditPanel.Camera.IOPort;
            if (_ioPort == null) return;

            //------------------------------------------------
            if (_ioPort.ContainsKey(1))
            {
                port1ComboBox.SelectedItem = IOPorts.ToString(_ioPort[1]);
                port1Panel.Visible = true;
            }
            else
                port1Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(2))
            {
                port2ComboBox.SelectedItem = IOPorts.ToString(_ioPort[2]);
                port2Panel.Visible = true;
            }
            else
                port2Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(3))
            {
                port3ComboBox.SelectedItem = IOPorts.ToString(_ioPort[3]);
                port3Panel.Visible = true;
            }
            else
                port3Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(4))
            {
                port4ComboBox.SelectedItem = IOPorts.ToString(_ioPort[4]);
                port4Panel.Visible = true;
            }
            else
                port4Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(5))
            {
                port5ComboBox.SelectedItem = IOPorts.ToString(_ioPort[5]);
                port5Panel.Visible = true;
            }
            else
                port5Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(6))
            {
                port6ComboBox.SelectedItem = IOPorts.ToString(_ioPort[6]);
                port6Panel.Visible = true;
            }
            else
                port6Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(7))
            {
                port7ComboBox.SelectedItem = IOPorts.ToString(_ioPort[7]);
                port7Panel.Visible = true;
            }
            else
                port7Panel.Visible = false;
            //------------------------------------------------
            if (_ioPort.ContainsKey(8))
            {
                port8ComboBox.SelectedItem = IOPorts.ToString(_ioPort[8]);
                port8Panel.Visible = true;
            }
            else
                port8Panel.Visible = false;

            if (port1Panel.Visible) port1Panel.Invalidate();
            if (port2Panel.Visible) port2Panel.Invalidate();
            if (port3Panel.Visible) port3Panel.Invalidate();
            if (port4Panel.Visible) port4Panel.Invalidate();
            if (port5Panel.Visible) port5Panel.Invalidate();
            if (port6Panel.Visible) port6Panel.Invalidate();
            if (port7Panel.Visible) port7Panel.Invalidate();
            if (port8Panel.Visible) port8Panel.Invalidate();
            //------------------------------------------------
            port1ComboBox.Enabled = port2ComboBox.Enabled = port3ComboBox.Enabled = port4ComboBox.Enabled =
                port5ComboBox.Enabled = port6ComboBox.Enabled = port7ComboBox.Enabled = port8ComboBox.Enabled = 
                EditPanel.Camera.Model.IOPortConfigurable;
        }

        private void Port1ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(1)) return;

            _ioPort[1] = IOPorts.ToIndex(port1ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port2ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(2)) return;

            _ioPort[2] = IOPorts.ToIndex(port2ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port3ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(3)) return;

            _ioPort[3] = IOPorts.ToIndex(port3ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port4ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(4)) return;

            _ioPort[4] = IOPorts.ToIndex(port4ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port5ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(5)) return;

            _ioPort[5] = IOPorts.ToIndex(port5ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port6ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(6)) return;

            _ioPort[6] = IOPorts.ToIndex(port6ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port7ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(7)) return;

            _ioPort[7] = IOPorts.ToIndex(port7ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void Port8ComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _ioPort == null || !_ioPort.ContainsKey(8)) return;

            _ioPort[8] = IOPorts.ToIndex(port8ComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void IOPortControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_IOPortConfig"], SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            SetupBase.Manager.Paint(g, control);

            SetupBase.Manager.PaintText(g, Localization["DevicePanel_IOPort"].Replace("%1", control.Tag.ToString()));
        }
    }
}
