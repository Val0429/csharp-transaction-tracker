using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupPOSConnection
{
    public sealed class ConnectionPanel : Panel
    {
        public event EventHandler OnConnectionEditClick;

        public Dictionary<String, String> Localization;
        
        private readonly  CheckBox _checkBox = new CheckBox();

        public IPOSConnection POSConnection;

        public ConnectionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"ConnectionPanel_NumPOS", "(%1 POS)"},
                                   {"POSConnection_Manufacture", "Manufacture"},
                                   {"POSConnection_AcceptPort", "Accept Port"},
                                   {"POSConnection_ConnectPort", "Connect Port"},
                                    {"POSConnection_Protocol", "Protocol"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Size = new Size(300, 40);

            BackColor = Color.Transparent;

            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += ConnectionPanelMouseClick;
            Paint += ConnectionPanelPaint;
        }

        public Brush SelectedColor = Manager.DeleteTextColor;

        private void ConnectionPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null || POSConnection == null) return;

            Graphics g = e.Graphics;

            Brush fontBrush = Brushes.Black;

            if (_editVisible || _checkBox.Visible)
            {
                if (_editVisible)
                {
                    Manager.Paint(g, (Control)sender, true);
                    Manager.PaintEdit(g, this);
                }
                else
                    Manager.Paint(g, (Control)sender);
            }
            else
            {
                Manager.PaintSingleInput(g, (Control)sender);
            }

            Manager.PaintStatus(g, POSConnection.ReadyState);

            if (Width <= 300) return;

            if (_checkBox.Visible && Checked)
                fontBrush = SelectedColor;

            var name = POSConnection.ToString();
            if (POSConnection.POS.Count != 0)
                name += " " + Localization["ConnectionPanel_NumPOS"].Replace("%1", POSConnection.POS.Count.ToString());

            name += ", " + Localization["POSConnection_Manufacture"] + " : " +
                    POS_Exception.ToDisplay(POSConnection.Manufacture);

            switch (POSConnection.Manufacture)
            {
                case "ActiveMQ":
                    name += ", " + Localization["POSConnection_ConnectPort"] + " : " + POSConnection.ConnectPort;
                    break;

                case "Oracle":
                    //add nothing
                    break;
                case "Generic":
                    name += ", " + Localization["POSConnection_AcceptPort"] + " : " + POSConnection.AcceptPort + ", " + Localization["POSConnection_Protocol"] + " : " + POSConnection.Protocol;
                    break;

                default:
                    name += ", " + Localization["POSConnection_AcceptPort"] + " : " + POSConnection.AcceptPort;
                    break;
            }

            var nameRectangleF = new RectangleF(44, 13, Width - 74, 15);
            g.DrawString(name, Manager.Font, fontBrush, nameRectangleF);
        }

        private void ConnectionPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (_checkBox.Visible)
            {
                _checkBox.Checked = !_checkBox.Checked;
                return;
            }

            if (OnConnectionEditClick != null)
                OnConnectionEditClick(this, e);
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();
        }

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            get { return _checkBox.Visible; }
            set
            {
                if (value)
                {
                    if (POSConnection == null)
                        _checkBox.Visible = false;
                    else
                        _checkBox.Visible = true;
                }
                else
                {
                    _checkBox.Visible = false;
                }
            }
        }

        private Boolean _editVisible;
        public Boolean EditVisible
        {
            set
            {
                _editVisible = value;
                Invalidate();
            }
        }
    }
}
