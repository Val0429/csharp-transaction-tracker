using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class SSLPortControl : UserControl
    {
        public IServer Server ;
        public Dictionary<String, String> Localization;
        private List<String> _notAllowPorts;

        private List<String> _modifyed = new List<string>();

        public SSLPortControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Error", "Error"},

                                   {"SetupServer_Custom", "Custom"},
                                   {"SetupServer_PortWarning", "Invalid port. Please input again  without %1."},
                                   {"SetupServer_DoNotSetPortToBeEqual2", "Do not set \"Server SSL port\" and \"Database port\" to be equal"},
                                   {"SetupServer_DoNotSetSSLPortToBeEqual", "Do not set \"Server port\" and \"Server SSL port\" to be equal"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "SSLPort";

            BackgroundImage = Manager.BackgroundNoBorder;

            //warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual2"];
            customPanel.Paint += CustomPanelPaint;

            sslPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            sslPortTextBox.TextChanged += SSLPortTextBoxTextChanged;
            sslPortTextBox.LostFocus += SSLPortTextBoxLostFocus;

            _notAllowPorts = new List<String>();
        }

        public Boolean IsEditing;
        public void ParseSetting()
        {
            IsEditing = false;

            sslPortTextBox.Text = (Server.Server.SSLPort > 0)
                                   ? Server.Server.SSLPort.ToString()
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
                if (Server.Server.Database.Port == Server.Server.SSLPort)
                {
                    warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual2"];
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

        private readonly UInt16[] _portArray = new UInt16[] { 443, 777, 8081, 8089 };
        public void Initialize()
        {
            foreach (UInt16 port in _portArray.Reverse())
            {
                if (Server.Server.NotAllowPorts.Contains(port)) continue;
                var sslPortPanel = new SSLPortPanel
                {
                    Server = Server,
                    Tag = port.ToString(),
                };

                sslPortPanel.MouseClick += SSLPortPanelMouseClick;

                containerPanel.Controls.Add(sslPortPanel);
            }

            foreach (UInt16 port in Server.Server.NotAllowPorts)
            {
                _notAllowPorts.Add(port.ToString());
            }
        }

        private void SSLPortPanelMouseClick(Object sender, MouseEventArgs e)
        {
            sslPortTextBox.Text = ((Control)sender).Tag.ToString();

            Server.WriteOperationLog("Change server SSL port to %1".Replace("%1", Server.Server.SSLPort.ToString()));

            ((Control)sender).Focus();
            Invalidate();
        }

        private void CustomPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_portArray.Contains(Server.Server.SSLPort))
            {
                Manager.PaintText(g, Localization["SetupServer_Custom"], Manager.SelectedTextColor);

                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["SetupServer_Custom"]);
            }
        }

        private void SSLPortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (sslPortTextBox.Text != "") ? Convert.ToUInt32(sslPortTextBox.Text) : 0;
            port = Math.Min(Math.Max(port, 1), 65535);

            if ( port != Server.Server.SSLPort) _modifyed.Add("PORT");

            Server.Server.SSLPort = Convert.ToUInt16(port);

            sslPortTextBox.BackColor = Server.Server.NotAllowPorts.Contains(Server.Server.SSLPort) ? Color.LightPink : Color.White;

            CheckPortStatus();

            Invalidate();
        }

        private void SSLPortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            if (_modifyed.Contains("PORT"))
                Server.WriteOperationLog("Change server SSL port to %1".Replace("%1", Server.Server.SSLPort.ToString()));

            if (!Server.Server.NotAllowPorts.Contains(Server.Server.SSLPort)) return;

            sslPortTextBox.LostFocus -= SSLPortTextBoxLostFocus;
            sslPortTextBox.BackColor = Color.LightPink;
            var result = MessageBox.Show(Localization["SetupServer_PortWarning"].Replace("%1", String.Join(",", _notAllowPorts.ToArray())),
                                         Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                
            if(result == DialogResult.OK)
            {
                sslPortTextBox.Focus();
                sslPortTextBox.LostFocus += SSLPortTextBoxLostFocus;
            }
        }
    }

    public sealed class SSLPortPanel : Panel
    {
        public IServer Server;
        public SSLPortPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += SSLPortPanelPaint;
        }

        private void SSLPortPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Parent.Controls[0] == this)
            {
                Manager.PaintBottom(g, this);
            }
            else
            {
                Manager.PaintMiddle(g, this);
            }

            if (Width <= 100) return;
            if (Tag.ToString() == Server.Server.SSLPort.ToString())
            {
                Manager.PaintText(g, Tag.ToString(), Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Tag.ToString());
        }
    }
}
