using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;
using POSException;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGenericPOSSetting
{
    public sealed partial class EditPanel : UserControl
    {
        public IPOSConnection GenericPosSetting;
        public POS_Exception POSException;
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public Boolean IsEditing;
        public Boolean IsRunning;
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
                                   {"POSConnection_ExportCompleted", "Export transactions successfully."},
                                   {"POSConnection_Name", "Name"},
                                   {"POSConnection_Protocol", "Protocol"},
                                   {"POSConnection_NetworkAddress", "Network Address"},
                                   {"POSConnection_Port", "Port"},
                                   {"POSConnection_POSId", "POS Id"},

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

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private static String FilePath = String.Format(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("SetupGenericPOSSetting.dll", "")) + "\\" + "collection";
        public void Initialize()
        {
           
            containerPanel.Paint += ContainerPanelPaint;

            protocolPanel.Paint += PaintInput;
            networkAddressPanel.Paint += PaintInput;
            portPanel.Paint += PaintInput;
            posIdDoubleBufferPanel.Paint += PaintInput;

            protocolComboBox.Items.Add("UDP");
            protocolComboBox.Items.Add("TCP");
            protocolComboBox.SelectedIndex = 0;

            ipAddressTextBox.TextChanged += IPAddressTextBoxTextChanged;
            acceptPortTextBox.LostFocus += AcceptPortTextBoxLostFocus;
            protocolComboBox.SelectedIndexChanged += ProtocolComboBoxSelectedIndexChanged;

            PTS.POS.OnSaveComplete += POSOnSaveComplete;

            if (PTS != null)
            {
                PTS.OnPOSEventReceive -= POSEventReceive;
                PTS.OnPOSEventReceive += POSEventReceive;
            }

            ClearFolder();
        }

        private void ClearFolder()
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(FilePath))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(FilePath);
                }
                else
                {
                    foreach (FileInfo file in new DirectoryInfo(FilePath).GetFiles())
                    {
                        file.Delete();
                    }
                    Directory.Delete(FilePath);
                    Directory.CreateDirectory(FilePath);
                }
            }
            catch (Exception)
            {
            }
        }

        private Boolean IsPauseReceiveEvent;
        private void POSEventReceive(Object sender, EventArgs<POS_Exception.TransactionItem> e)
        {
            if (IsPauseReceiveEvent) return;
            if (!IsRunning) return;

            AddEvent(e.Value);
        }

        private UInt64 _currentLine = 0;
        private const UInt16 MaximumAmount = 5000;
        private delegate void AddEventDelegate(POS_Exception.TransactionItem transactionItem);
        private void AddEvent(POS_Exception.TransactionItem transactionItem)
        {
            //if (_selectedPOSId != "0" && _selectedPOSId != transactionItem.POS) return;
            if (transactionItem.POS != "PTSDemo") return;
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new AddEventDelegate(AddEvent), transactionItem);
                }
                catch (Exception)
                {
                }
                return;
            }
            if (transactionListBox.Items.Count > MaximumAmount)
            {
                //transactionListBox.Items.RemoveAt(0);
                //recycle
                var item = transactionListBox.Items[0] as TransactionItemControl2;
                if (item != null)
                {
                    item.TransactionItem = transactionItem;
                    item.HeightVerified = item.NeedHeight = false;
                    item.POS = PTS.POS.FindPOSById(transactionItem.POS);
                    transactionListBox.Items.RemoveAt(0);
                    transactionListBox.Items.Add(item);
                    WriteLine(item.ToFileString());
                }
            }
            else
            {
                var item = new TransactionItemControl2
                               {
                                   POS = PTS.POS.FindPOSById(transactionItem.POS),
                                   TransactionItem = transactionItem,
                                   DisplayPOS = true,
                               };
                transactionListBox.Items.Add(item);
                WriteLine(item.ToFileString());
                //transactionListBox.Items.Add(transactionItemDetail.DateTime.ToString("  yyyy-MM-dd  HH:mm:ss       ") + transactionItemDetail.Content);
            }

            //scroll down
            //_ignore = true;
            transactionListBox.SelectedIndex = transactionListBox.Items.Count - 1;
            //select none
            transactionListBox.SelectedIndex = -1;
            //_ignore = false;
        }

        private void WriteLine(String line)
        {
            try
            {
                _currentLine++;

                StreamWriter streamWriter = File.AppendText(String.Format("{0}/Collection{1}.txt", FilePath, (_currentLine/MaximumAmount) + 1));
                streamWriter.Write(line + "</br>");
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
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
            if (GenericPosSetting == null) return;

            IsEditing = false;

            ipAddressTextBox.Text = GenericPosSetting.Authentication.Domain;

            if (GenericPosSetting.AcceptPort == 0)
                acceptPortTextBox.Text = "";
            else
                acceptPortTextBox.Text = GenericPosSetting.AcceptPort.ToString();

            protocolComboBox.SelectedItem = GenericPosSetting.Protocol;

            UpdateDisplayPanel();

            IsEditing = true;
        }

        private void IPAddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            GenericPosSetting.Authentication.Domain = ipAddressTextBox.Text;
            POSConnectionIsModify();
        }

        private void AcceptPortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
           
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
                        if (connection == GenericPosSetting) continue;
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
                GenericPosSetting.AcceptPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
                POSConnectionIsModify();
            }
            else
            {
                if (GenericPosSetting.AcceptPort == 0)
                    acceptPortTextBox.Text = "";
                else
                    acceptPortTextBox.Text = GenericPosSetting.AcceptPort.ToString();
                acceptPortTextBox.BackColor = Color.FromArgb(223, 173, 183);
                acceptPortTextBox.Focus();
            }
        }

        private void ProtocolComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            GenericPosSetting.Protocol = protocolComboBox.SelectedItem.ToString();

            POSConnectionIsModify();
        }

        private void UpdateDisplayPanel()
        {
            if (GenericPosSetting == null) return;

        }

        public void POSConnectionIsModify()
        {
            if (GenericPosSetting.ReadyState == ReadyState.Ready)
                GenericPosSetting.ReadyState = ReadyState.Modify;
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            //if (IsRunning) return;
            IsRunning = true;
            if (!PTS.POS.Connections.ContainsKey(GenericPosSetting.Id))
            {
                GenericPosSetting.ReadyState = ReadyState.Modify;
                PTS.POS.Connections.Add(GenericPosSetting.Id, GenericPosSetting);
            }

            if (!PTS.POS.Exceptions.ContainsKey(POSException.Id))
            {
                POSException.ReadyState = ReadyState.Modify;
                PTS.POS.Exceptions.Add(POSException.Id, POSException);
            }
            PTS.POS.Save("Generic");
            ApplicationForms.ShowLoadingIcon(PTS.Form);
        }


        private void POSOnSaveComplete(object sender, EventArgs e)
        {
            ApplicationForms.HideLoadingIcon();
        }

        private void PauseButtonClick(object sender, EventArgs e)
        {
            IsPauseReceiveEvent = !IsPauseReceiveEvent;

            if (IsPauseReceiveEvent)
            {
                stopButton.Text = @"Play";
                //_pauseEvent.BackgroundImage = _pauseeventactivate;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_ResumeEvent"]);
            }
            else
            {
                stopButton.Text = @"Stop";
                //_pauseEvent.BackgroundImage = _pauseevent;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_PauseEvent"]);
            }
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            _currentLine = 0;
            Stop();
            transactionListBox.Items.Clear();
            ClearFolder();
        }

        private void ExportButtonClick(object sender, EventArgs e)
        {
            DialogResult resault = folderBrowserDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                 var path = folderBrowserDialog.SelectedPath;
                 var folderPath = String.Format(@"{0}\Transaction from {1} port {2} ({3})", path, GenericPosSetting.Authentication.Domain, GenericPosSetting.AcceptPort, DateTime.Now.ToString("MM-dd-yyyy  HH-mm-ss", CultureInfo.InvariantCulture));
                if (!Directory.Exists(folderPath))
                 {
                     Directory.CreateDirectory(folderPath);
                 }

                foreach (FileInfo file in new DirectoryInfo(FilePath).GetFiles())
                {
                    File.Copy(Path.Combine(FilePath, file.Name), Path.Combine(folderPath, file.Name), true);
                }

                TopMostMessageBox.Show(Localization["POSConnection_ExportCompleted"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            if (!IsRunning) return;
            IsRunning = false;
            PTS.POS.Exceptions.Remove(POSException.Id);
            PTS.POS.Connections.Remove(GenericPosSetting.Id);
            PTS.POS.Save("Generic");
            ApplicationForms.ShowLoadingIcon(PTS.Form);
        }

        public void Stop()
        {
            StopButtonClick(this, null);
        }
    }
}
