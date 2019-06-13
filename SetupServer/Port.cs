using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public partial class PortControl : UserControl
    {
        public IServer Server { get; set; }
        public Dictionary<String, String> Localization;
        private readonly List<String> _notAllowPorts = new List<String>();

        private readonly List<String> _modifyed = new List<string>();

        public PortControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Error", "Error"},

                                   {"SetupServer_Custom", "Custom"},
                                   {"SetupServer_PortWarning", "Invalid port. Please input again  without %1."},
                                   {"SetupServer_DoNotSetPortToBeEqual", "Do not set \"Server port\" and \"Database port\" to be equal"},
                                   {"SetupServer_DoNotSetSSLPortToBeEqual", "Do not set \"Server port\" and \"Server SSL port\" to be equal"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Port";

            BackgroundImage = Manager.BackgroundNoBorder;

            //warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual"];
            customPanel.Paint += CustomPanelPaint;

            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            portTextBox.TextChanged += PortTextBoxTextChanged;
            portTextBox.LostFocus += PortTextBoxLostFocus;
        }

        public Boolean IsEditing;
        public void ParseSetting()
        {
            IsEditing = false;

            portTextBox.Text = (Server.Server.Port > 0)
                                   ? Server.Server.Port.ToString()
                                   : "";

            IsEditing = true;
            CheckPortStatus();

            _modifyed.Clear();
        }

        private void CheckPortStatus()
        {
            warningLabel.Visible = false;
            if (Server is IPTS)
            {
                if (Server.Server.Database.Port == Server.Server.Port)
                {
                    warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual"];
                    warningLabel.Visible = true;
                }
                else if (Server.Server.Port == Server.Server.SSLPort)
                {
                    warningLabel.Text = Localization["SetupServer_DoNotSetSSLPortToBeEqual"];
                    warningLabel.Visible = true;
                }
            }
            else if (Server is INVR)
            {
                if (Server.Server.Port == Server.Server.SSLPort)
                {
                    warningLabel.Text = Localization["SetupServer_DoNotSetSSLPortToBeEqual"];
                    warningLabel.Visible = true;
                }
            }
        }

        private readonly UInt16[] _portArray = new UInt16[] { 80, 82, 7777, 8080, 8088 };
        public void Initialize()
        {
            foreach (UInt16 port in _portArray.Reverse())
            {
                if (Server.Server.NotAllowPorts.Contains(port)) continue;
                var portPanel = new PortPanel
                {
                    Server = Server,
                    Tag = port.ToString(),
                };
                
                portPanel.MouseClick += PortPanelMouseClick;

                containerPanel.Controls.Add(portPanel);
            }

            foreach (UInt16 port in Server.Server.NotAllowPorts)
            {
                _notAllowPorts.Add(port.ToString());
            }
        }

        private void PortPanelMouseClick(Object sender, MouseEventArgs e)
        {
            portTextBox.Text = ((Control)sender).Tag.ToString();

            Server.WriteOperationLog("Change server port to %1".Replace("%1", Server.Server.Port.ToString()));

            ((Control)sender).Focus();
            Invalidate();
        }

        private void CustomPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_portArray.Contains(Server.Server.Port))
            {
                Manager.PaintText(g, Localization["SetupServer_Custom"], Manager.SelectedTextColor);

                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["SetupServer_Custom"]);
            }
        }

        private void PortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (portTextBox.Text != "") ? Convert.ToUInt32(portTextBox.Text) : 0;
            port = Math.Min(Math.Max(port, 1), 65535);

            if ( port != Server.Server.Port) _modifyed.Add("PORT");

            Server.Server.Port = Convert.ToUInt16(port);

            portTextBox.BackColor = Server.Server.NotAllowPorts.Contains(Server.Server.Port) ? Color.LightPink : Color.White;

            CheckPortStatus();

            Invalidate();
        }

        private void PortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            if (_modifyed.Contains("PORT"))
                Server.WriteOperationLog("Change server port to %1".Replace("%1", Server.Server.Port.ToString()));

            if (!Server.Server.NotAllowPorts.Contains(Server.Server.Port)) return;

            portTextBox.LostFocus -= PortTextBoxLostFocus;
            portTextBox.BackColor = Color.LightPink;
            var result = MessageBox.Show(Localization["SetupServer_PortWarning"].Replace("%1", String.Join(",", _notAllowPorts.ToArray())),
                                         Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                
            if(result == DialogResult.OK)
            {
                portTextBox.Focus();
                portTextBox.LostFocus += PortTextBoxLostFocus;
            }
        }
    }
}
