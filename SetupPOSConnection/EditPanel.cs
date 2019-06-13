using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using SetupPOS;
using Manager = SetupBase.Manager;

namespace SetupPOSConnection
{
    public sealed partial class EditPanel : UserControl
    {
        public IPOSConnection POSConnection;
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public Boolean IsEditing;
        private POSConnectionPanel _posConnectionPanel;

        private Encryption[] _encryption = new[] { 
            Encryption.Basic, 
            Encryption.Plain, 
            Encryption.Digest,
        };
        public EditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"POSConnection_Name", "Name"},
                                   {"POSConnection_Manufacture", "Manufacture"},
                                   {"POSConnection_Model", "Model"},
                                   {"POSConnection_Protocol", "Protocol"},
                                   {"POSConnection_NetworkAddress", "Network Address"},
                                   {"POSConnection_QueueInfo", "Queue Info"},
                                   {"POSConnection_ConnectInfo", "Connect Info"},
                                   {"POSConnection_AcceptPort", "Accept Port"},
                                   {"POSConnection_ConnectPort", "Connect Port"},
                                   {"POSConnection_Security", "Security"},
                                   {"POSConnection_Account", "Account"},
                                   {"POSConnection_Password", "Password"},

                                   {"EditPOSConnectionPanel_AcceptPortCantEmpty", "Accept port can't be empty."},
                                   {"EditPOSConnectionPanel_AcceptPortOutOfRange", "Accept port value must be 1 - 65535."},
                                   {"EditPOSConnectionPanel_AcceptPortCantTheSame", "Accept port can't be the same. %1 is used by %2."},
                                   
                                   {"EditPOSConnectionPanel_ConnectPortCantEmpty", "Connect port can't be empty."},
                                   {"EditPOSConnectionPanel_ConnectPortOutOfRange", "Connect port value must be 1 - 65535."},
                                   {"EditPOSConnectionPanel_ConnectPortCantTheSame", "Connect port can't be the same. %1 is used by %2."},

                                   {"EditPOSConnectionPanel_Information", "Information"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            //nameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            acceptPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            connectPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            _posConnectionPanel = new POSConnectionPanel
            {
                PTS = PTS,
                EditVisible = false,
                Padding = new Padding(0, 0, 0, 0),
            };
            Controls.Add(_posConnectionPanel);

            _posConnectionPanel.OnPOSSelectionChange += POSConnectionPanelOnPOSSelectionChange;
            _posConnectionPanel.BringToFront();

            containerPanel.Paint += ContainerPanelPaint;

            manufacturePanel.Paint += PaintInput;
            modelPanel.Paint += PaintInput;
            protocolPanel.Paint += PaintInput;
            namePanel.Paint += PaintInput;
            networkAddressPanel.Paint += PaintInput;
            queueInfoPanel.Paint += PaintInput;
            connectInfoPanel.Paint += PaintInput;
            acceptPortPanel.Paint += PaintInput;
            connectPortPanel.Paint += PaintInput;
            accountPanel.Paint += PaintInput;
            passwordPanel.Paint += PaintInput;
            securityPanel.Paint += PaintInput;
            protocolTypePanel.Paint += PaintInput;

            foreach (String manufacture in POS_Exception.Manufactures)
            {
                manufactureComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
            }
            manufactureComboBox.SelectedIndex = 0;
            manufactureComboBox.Enabled = (POS_Exception.Manufactures.Length > 1);

            foreach (var protocol in Device.POSConnection.ProtocolList)
            {
                protocolComboBox.Items.Add(protocol);
            }
            protocolComboBox.SelectedIndex = 0;
            protocolComboBox.Enabled = (Device.POSConnection.ProtocolList.Count > 1);

            foreach (Encryption encryption in _encryption)
                encryptionComboBox.Items.Add(Encryptions.ToString(encryption));

            protocolTypeComboBox.Items.Add("UDP");
            protocolTypeComboBox.Items.Add("TCP");
            protocolTypeComboBox.Items.Add("Multicast");
            protocolTypeComboBox.SelectedIndex = 0;

            nameTextBox.TextChanged += NameTextBoxTextChanged;
            ipAddressTextBox.TextChanged += IPAddressTextBoxTextChanged;
            queueInfoTextBox.TextChanged += QueueInfoTextBoxTextChanged;
            connectInfoTextBox.TextChanged += ConnectInfoTextBoxTextChanged;
            userNameTextBox.TextChanged += UserNameTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;

            //acceptPortTextBox.TextChanged += AcceptPortTextBoxTextChanged;
            acceptPortTextBox.LostFocus += AcceptPortTextBoxLostFocus;
            //connectPortTextBox.TextChanged += ConnectPortTextBoxTextChanged;
            connectPortTextBox.LostFocus += ConnectPortTextBoxLostFocus;

            manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
            //modelComboBox.SelectedIndexChanged += ModelComboBoxSelectedIndexChanged;
            protocolComboBox.SelectedIndexChanged += ProtocolComboBoxSelectedIndexChanged;
            encryptionComboBox.SelectedIndexChanged += EncryptionComboBoxSelectedIndexChanged;
            protocolTypeComboBox.SelectedIndexChanged += ProtocolTypeComboBoxSelectedIndexChanged;
        }

        private void ProtocolTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Protocol = protocolTypeComboBox.SelectedItem.ToString();

            if (POSConnection.Protocol == "Multicast")
                networkAddressPanel.Visible = true;
            else
                networkAddressPanel.Visible = false;

            POSConnectionIsModify();
        }

        private void POSConnectionPanelOnPOSSelectionChange(Object sender, EventArgs e)
        {
            var panel = sender as POSPanel;
            if (panel == null || panel.POS == null) return;

            if (panel.Checked)
            {
                if (!POSConnection.POS.Contains(panel.POS))
                    POSConnection.POS.Add(panel.POS);
            }
            else
            {
                POSConnection.POS.Remove(panel.POS);
            }
            POSConnectionIsModify();
        }

        private void ContainerPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["EditPOSConnectionPanel_Information"], Manager.Font, Brushes.DimGray, 8, 0);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (Localization.ContainsKey("POSConnection_" + ((Control)sender).Tag))
                Manager.PaintText(g, Localization["POSConnection_" + ((Control)sender).Tag]);
            else
                Manager.PaintText(g, ((Control)sender).Tag.ToString());
        }

        public void ParsePOSConnection()
        {
            if (POSConnection == null) return;

            IsEditing = false;

            nameTextBox.Text = POSConnection.Name;
            ipAddressTextBox.Text = POSConnection.Authentication.Domain;
            queueInfoTextBox.Text = POSConnection.QueueInfo;
            connectInfoTextBox.Text = POSConnection.ConnectInfo;

            if (POSConnection.AcceptPort == 0)
                acceptPortTextBox.Text = "";
            else
                acceptPortTextBox.Text = POSConnection.AcceptPort.ToString();

            connectPortTextBox.Text = POSConnection.ConnectPort.ToString();
            userNameTextBox.Text = POSConnection.Authentication.UserName;
            passwordTextBox.Text = POSConnection.Authentication.Password;

            manufactureComboBox.SelectedItem = POS_Exception.ToDisplay(POSConnection.Manufacture);
            encryptionComboBox.SelectedItem = Encryptions.ToString(POSConnection.Authentication.Encryption);
            protocolComboBox.SelectedItem = POSConnection.Protocol;
            encryptionComboBox.SelectedItem = Encryptions.ToString(POSConnection.Authentication.Encryption);
            protocolTypeComboBox.SelectedItem = POSConnection.Protocol;

            _posConnectionPanel.ClearViewModel();
            _posConnectionPanel.POSConnection = POSConnection;
            _posConnectionPanel.ShowPOSWithSelection();


            UpdateDisplayPanel();

            switch (POSConnection.Manufacture)
            {
                case "ActiveMQ":
                case "Oracle":
                case "Oracle Demo":
                    connectPortTextBox.Focus();
                    break;

                default:
                    acceptPortTextBox.Focus();
                    break;
            }

            IsEditing = true;
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Name = nameTextBox.Text;
            POSConnectionIsModify();
        }

        private void IPAddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Authentication.Domain = ipAddressTextBox.Text;
            POSConnectionIsModify();
        }

        private void QueueInfoTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.QueueInfo = queueInfoTextBox.Text;
            POSConnectionIsModify();
        }

        private void ConnectInfoTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.ConnectInfo = connectInfoTextBox.Text;
            POSConnectionIsModify();
        }
        
        private void UserNameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Authentication.UserName = userNameTextBox.Text;
            POSConnectionIsModify();
        }

        //private void AcceptPortTextBoxTextChanged(Object sender, EventArgs e)
        //{
        //    if (!IsEditing) return;

        //    if (String.IsNullOrEmpty(acceptPortTextBox.Text)) return;

        //    UInt32 port = Convert.ToUInt32(acceptPortTextBox.Text);
        //    if (port > 0 && port <= 65535)
        //    {
        //        acceptPortTextBox.BackColor = Color.White;
        //        POSConnection.AcceptPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
        //        POSConnectionIsModify();
        //    }
        //}

        private void AcceptPortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (manufactureComboBox.Focused) return;

            bool pass = true;
            UInt32 port = 0;
            if (String.IsNullOrEmpty(acceptPortTextBox.Text))
            {
                TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                pass = false;
            }
            else
            {
                port = Convert.ToUInt32(acceptPortTextBox.Text);

                if (port < 1 || port > 65535)
                {
                    TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortOutOfRange"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pass = false;
                }
                else
                {
                    foreach (var obj in PTS.POS.Connections)
                    {
                        var connection = obj.Value;
                        if (connection == POSConnection) continue;
                        if (connection.AcceptPort != port) continue;

                        TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortCantTheSame"].
                            Replace("%1", connection.AcceptPort.ToString()).Replace("%2", connection.ToString()),
                            Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pass = false;
                        break;
                    }
                }
            }

            if (pass)
            {
                acceptPortTextBox.BackColor = Color.White;
                POSConnection.AcceptPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
                POSConnectionIsModify();
            }
            else
            {
                if (POSConnection.AcceptPort == 0)
                    acceptPortTextBox.Text = "";
                else
                    acceptPortTextBox.Text = POSConnection.AcceptPort.ToString();
                acceptPortTextBox.BackColor = Color.FromArgb(223, 173, 183);
                acceptPortTextBox.Focus();
            }
        }

        private void ConnectPortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (manufactureComboBox.Focused) return;

            bool pass = true;
            UInt32 port = 0;
            if (String.IsNullOrEmpty(connectPortTextBox.Text))
            {
                TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_ConnectPortCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                pass = false;
            }
            else
            {
                port = Convert.ToUInt32(connectPortTextBox.Text);

                if (port < 1 || port > 65535)
                {
                    TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_ConnectPortOutOfRange"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pass = false;
                }
                else
                {
                    foreach (var obj in PTS.POS.Connections)
                    {
                        var connection = obj.Value;
                        if (connection == POSConnection) continue;
                        if (connection.ConnectPort != port) continue;

                        TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_ConnectPortCantTheSame"].
                            Replace("%1", connection.ConnectPort.ToString()).Replace("%2", connection.ToString()),
                            Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pass = false;
                        break;
                    }
                }
            }

            if (pass)
            {
                connectPortTextBox.BackColor = Color.White;
                POSConnection.ConnectPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
                POSConnectionIsModify();
            }
            else
            {
                if (POSConnection.ConnectPort == 0)
                    connectPortTextBox.Text = "";
                else
                    connectPortTextBox.Text = POSConnection.ConnectPort.ToString();
                connectPortTextBox.BackColor = Color.FromArgb(223, 173, 183);
                connectPortTextBox.Focus();
            }
        }

        //private void ConnectPortTextBoxTextChanged(Object sender, EventArgs e)
        //{
        //    if (!IsEditing) return;

        //    if (String.IsNullOrEmpty(connectPortTextBox.Text)) return;

        //    UInt32 port = Convert.ToUInt32(connectPortTextBox.Text);
        //    POSConnection.ConnectPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
        //    POSConnectionIsModify();
        //}

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Authentication.Password = passwordTextBox.Text;
            POSConnectionIsModify();
        }

        private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (POSConnection == null) return;

            POSConnection.Manufacture = POS_Exception.ToIndex(manufactureComboBox.SelectedItem.ToString());
            POSConnection.SetDefaultAuthentication();

            switch (POSConnection.Manufacture)
            {
                case "Generic":
                    POSConnection.Protocol = protocolTypeComboBox.SelectedItem as String;
                    break;

                default:
                    POSConnection.Protocol = protocolComboBox.SelectedItem as String;
                    break;
            }

            var poss = new List<IPOS>(POSConnection.POS);
            foreach (var pos in poss)
            {
                if (pos.Manufacture == POSConnection.Manufacture) continue;

                POSConnection.POS.Remove(pos);
            }

            _posConnectionPanel.ClearViewModel();
            _posConnectionPanel.POSConnection = POSConnection;
            _posConnectionPanel.ShowPOSWithSelection();

            POSConnectionIsModify();

            UpdateDisplayPanel();
            ParsePOSConnection();
        }

        //private void ModelComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        //{
        //    if (!IsEditing) return;

        //    POSConnection.Model = modelComboBox.SelectedItem.ToString();
        //    POSConnectionIsModify();
        //}

        private void ProtocolComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Protocol = protocolComboBox.SelectedItem.ToString();

            POSConnectionIsModify();
        }

        private void EncryptionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSConnection.Authentication.Encryption = Encryptions.ToIndex(encryptionComboBox.SelectedItem.ToString()); ;
            POSConnectionIsModify();
        }

        private void UpdateDisplayPanel()
        {
            if (POSConnection == null) return;

            switch (POSConnection.Manufacture)
            {
                case "ActiveMQ":
                    passwordPanel.Visible = true;
                    accountPanel.Visible = true;
                    acceptPortPanel.Visible = false;
                    connectPortPanel.Visible = true;
                    networkAddressPanel.Visible = true;
                    queueInfoPanel.Visible = true;
                    connectInfoPanel.Visible = false;
                    protocolTypePanel.Visible = false;
                    break;

                case "Oracle":
                //case "Oracle Demo":
                    passwordPanel.Visible = true;
                    accountPanel.Visible = true;
                    acceptPortPanel.Visible = false;
                    connectPortPanel.Visible = false;
                    networkAddressPanel.Visible = false;
                    queueInfoPanel.Visible = true;
                    connectInfoPanel.Visible = true;
                    protocolTypePanel.Visible = false;
                    break;

                case "Generic":
                    passwordPanel.Visible = false;
                    accountPanel.Visible = false;
                    acceptPortPanel.Visible = true;
                    connectPortPanel.Visible = false;
                    networkAddressPanel.Visible = false;
                    queueInfoPanel.Visible = false;
                    connectInfoPanel.Visible = false;
                    protocolTypePanel.Visible = true;
                    break;

                default:
                    passwordPanel.Visible = false;
                    accountPanel.Visible = false;
                    acceptPortPanel.Visible = true;
                    connectPortPanel.Visible = false;
                    networkAddressPanel.Visible = false;
                    queueInfoPanel.Visible = false;
                    connectInfoPanel.Visible = false;
                    protocolTypePanel.Visible = false;
                    break;
            }
        }

        public void POSConnectionIsModify()
        {
            if (POSConnection.ReadyState == ReadyState.Ready)
                POSConnection.ReadyState = ReadyState.Modify;
        }
    }
}
