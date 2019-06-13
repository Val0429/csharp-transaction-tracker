using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class DatabaseControl : UserControl
    {
        private IPTS _pts;
        public IServer Server
        {
            set { _pts = value as IPTS; }
            get { return _pts; }
        }

        public Dictionary<String, String> Localization;

        //private readonly UInt16[] _keeyMonths = new ushort[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        public DatabaseControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupServer_Custom", "Custom"},
                                   {"SetupServer_DoNotSetPortToBeEqual", "Do not set \"Server port\" and \"Database port\" to be equal"},
                                   {"SetupServer_DoNotSetPortToBeEqual2", "Do not set \"Server SSL port\" and \"Database port\" to be equal"},

                                   {"Database_Domain", "Domain"},
                                   {"Database_Port", "Port"},
                                   {"Database_Account", "Account"},
                                   {"Database_Password", "Password"},
                                   {"Database_KeepMonths", "Months to keep transaction data"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Database";

            BackgroundImage = Manager.BackgroundNoBorder;

            //warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual"];

            keepDaysPanel.Paint += KeepDaysPanelPaint;
            //domainPanel.Paint += PanelPaint;
            portPanel.Paint += PortPanelPaint;
            //accountPanel.Paint += PanelPaint;
            //passwordPanel.Paint += PanelPaint;

            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            keepMonthsTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            //foreach (var keeyDay in _keeyMonths)
            //{
            //    keepMonthsTextBox.Items.Add(keeyDay.ToString());
            //}
            keepMonthsTextBox.TextChanged += KeepMonthsTextBoxTextChanged;

            domainTextBox.TextChanged += DomainTextBoxTextChanged;
            portTextBox.TextChanged += PortTextBoxTextChanged;
        }

        //private readonly UInt16[] _portArray = new UInt16[] { 7778, 8089 };//8081, 
        public void Initialize()
        {
            //foreach (UInt16 port in _portArray.Reverse())
            //{
            //    var portPanel = new DataBasePortPanel
            //    {
            //        Server = Server,
            //        Tag = port.ToString(),
            //    };

            //    portPanel.MouseClick += PortPanelMouseClick;

            //    containerPanel.Controls.Add(portPanel);
            //}
        }

        //private void PortPanelMouseClick(Object sender, MouseEventArgs e)
        //{
        //    portTextBox.Text = ((Control)sender).Tag.ToString();

        //    ((Control)sender).Focus();
        //    Invalidate();
        //}


        private void KeepDaysPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, keepDaysPanel);

            if (keepDaysPanel.Width <= 200) return;

            Manager.PaintText(g, Localization["Database_KeepMonths"]);
        }

        private void PortPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, portPanel);

            if (portPanel.Width <= 100) return;

            Manager.PaintText(g, Localization["Database_Port"]);

            //if (!_portArray.Contains(Server.Server.Database.Port))
            //{
            //    Manager.PaintText(g, Localization["Database_Port"], Manager.SelectedTextColor);

            //    Manager.PaintSelected(g);
            //}
            //else
            //{
            //    Manager.PaintText(g, Localization["Database_Port"]);
            //}
        }

        public Boolean IsEditing;
        public void ParseSetting()
        {
            if (_pts == null) return;
            IsEditing = false;

            domainTextBox.Text = _pts.Server.Database.Domain;
            portTextBox.Text = _pts.Server.Database.Port.ToString();
            keepMonthsTextBox.Text = _pts.Server.DatabaseKeepMonths.ToString();

            IsEditing = true;

            if (Server is IPTS)
            {
                if(Server.Server.Database.Port == Server.Server.Port)
                {
                    warningLabel.Visible = true;
                    warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual"];
                }
                else if (Server.Server.Database.Port == Server.Server.SSLPort)
                {
                    warningLabel.Visible = true;
                    warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual2"];
                }
                else
                {
                    warningLabel.Visible = false;
                }
            }
        }

        private void KeepMonthsTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (String.IsNullOrEmpty(keepMonthsTextBox.Text)) return;

            var month = Convert.ToInt32(keepMonthsTextBox.Text);
            _pts.Server.DatabaseKeepMonths = Convert.ToUInt16(Math.Min(Math.Max(month, 1), 24));
            _pts.Server.DatabaseKeepMonthsReadyStatus = ReadyState.Modify;
        }

        private void DomainTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _pts.Server.Database.Domain = domainTextBox.Text;
            Server.Server.DatabaseReadyStatus = ReadyState.Modify;
        }

        private void PortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (portTextBox.Text != "") ? Convert.ToUInt32(portTextBox.Text) : 0;

            Server.Server.Database.Port = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            Server.Server.DatabaseReadyStatus = ReadyState.Modify;

            if (Server.Server.Database.Port == Server.Server.Port)
            {
                warningLabel.Visible = true;
                warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual"];
            }
            else if (Server.Server.Database.Port == Server.Server.SSLPort)
            {
                warningLabel.Visible = true;
                warningLabel.Text = Localization["SetupServer_DoNotSetPortToBeEqual2"];
            }
            else
            {
                warningLabel.Visible = false;
            }

            Invalidate();
        }

        //private void PanelPaint(Object sender, PaintEventArgs e)
        //{
        //    var control = sender as Control;
        //    if(control == null) return;

        //    Graphics g = e.Graphics;

        //    Manager.Paint(g, control);
        //    if (Width <= 100) return;

        //    if (Localization.ContainsKey("Database_" + control.Tag))
        //        Manager.PaintText(g, Localization["Database_" + control.Tag]);
        //    else
        //        Manager.PaintText(g, control.Tag.ToString());
        //}
    }

    public sealed class DataBasePortPanel : Panel
    {
        public IServer Server;
        public DataBasePortPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += PortPanelPaint;
        }

        private void PortPanelPaint(Object sender, PaintEventArgs e)
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
            if (Tag.ToString() == Server.Server.Database.Port.ToString())
            {
                Manager.PaintText(g, Tag.ToString(), Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Tag.ToString());
        }
    }
}
