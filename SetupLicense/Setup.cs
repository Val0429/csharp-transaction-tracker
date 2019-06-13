using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupLicense
{
    public partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        protected void RaiseOnSelectionChange(string xml)
        {
            var handler = OnSelectionChange;
            if (handler != null)
            {
                handler(this, new EventArgs<string>(xml));
            }
        }

        public String TitleName { get; set; }
        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Properties.Resources.icon;
        private static readonly Image _iconActivate = Properties.Resources.icon_activate;

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        private OpenFileDialog _openKeyDialog;

        public Setup()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_License", "License"},

								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Information", "Information"},
								   
								   {"SetupLicense_OnlineRegistration", "Online Registration"},
								   {"SetupLicense_Filter", "License key file"},
								   {"SetupLicense_LicenseKey", "License Key"},
								   {"SetupLicense_LicenseQuantity", "License Quantity"},
								   {"SetupLicense_EthernetCard", "Ethernet Card"},
								   {"SetupLicense_CheckLicenseLength", "Registration key length is 25. Please check the registration key is correct."},
								   {"SetupLicense_CheckLicenseFile", "Can't open license key file. Please check the license key file is correct."},
								   {"SetupLicense_AmountConfirm", "License quantity is \"%1\" .\r\nConfirm to register?"},
								   {"SetupLicense_KeyConfirm", "Registration key is \"%1\" .\r\nConfirm to register?"},
							   };
            Localizations.Update(Localization);

            Name = "License";
            TitleName = Localization["Control_License"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_License"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            _openKeyDialog = new OpenFileDialog
            {
                Filter = Localization["SetupLicense_Filter"] + @" (.lic)|*.lic"
            };

            amountDoubleBufferPanel.Paint += AmountInputPanelPaint;

            ethernetCardPanel.Paint += EthernetCardPanelPaint;

            licenseKeyBufferPanel.Paint += LicenseInputPanelPaint;

            ethernetCardPanel.Visible = licenseKeyBufferPanel.Visible = false;

            ethernetComboBox.SelectedIndexChanged += EthernetComboBoxSelectedIndexChanged;

            key1TextBox.Text = key2TextBox.Text = key3TextBox.Text = key4TextBox.Text = key5TextBox.Text = "";

            key1TextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            key2TextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            key3TextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            key4TextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            key5TextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            key1TextBox.OnPaste += Key1TextBoxOnPaste;
            key5TextBox.KeyPress += Key5TextBoxKeyPress;
        }

        protected void EthernetComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            infoContainerPanel.Controls.Clear();
            if (Server.License.Adaptor.Count == 0) return;

            var adaptor = Server.License.Adaptor[ethernetComboBox.SelectedIndex];

            var infoPanel = new InfoControl { Adaptor = adaptor };
            infoPanel.ParseInfo();
            infoContainerPanel.Controls.Add(infoPanel);
        }

        protected virtual void Key5TextBoxKeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SubmitOnlineRegistration();
            }
        }

        private void Key1TextBoxOnPaste(Object sender, EventArgs<String> e)
        {
            String key = e.Value;

            key1TextBox.TextChanged -= Key1TextBoxTextChanged;
            key2TextBox.TextChanged -= Key2TextBoxTextChanged;
            key3TextBox.TextChanged -= Key3TextBoxTextChanged;
            key4TextBox.TextChanged -= Key4TextBoxTextChanged;

            key1TextBox.Text = key.Substring(0, 5);
            key2TextBox.Text = key.Substring(5, 5);
            key3TextBox.Text = key.Substring(10, 5);
            key4TextBox.Text = key.Substring(15, 5);
            key5TextBox.Text = key.Substring(20, 5);

            key1TextBox.TextChanged += Key1TextBoxTextChanged;
            key2TextBox.TextChanged += Key2TextBoxTextChanged;
            key3TextBox.TextChanged += Key3TextBoxTextChanged;
            key4TextBox.TextChanged += Key4TextBoxTextChanged;

            key5TextBox.Focus();
        }

        protected void Key1TextBoxTextChanged(Object sender, EventArgs e)
        {
            if (key1TextBox.Text.Length == 5)
            {
                key2TextBox.Focus();
                key2TextBox.SelectAll();
            }
        }

        protected void Key2TextBoxTextChanged(Object sender, EventArgs e)
        {
            if (key2TextBox.Text.Length == 5)
            {
                key3TextBox.Focus();
                key3TextBox.SelectAll();
            }
        }

        protected void Key3TextBoxTextChanged(Object sender, EventArgs e)
        {
            if (key3TextBox.Text.Length == 5)
            {
                key4TextBox.Focus();
                key4TextBox.SelectAll();
            }
        }

        protected void Key4TextBoxTextChanged(Object sender, EventArgs e)
        {
            if (key4TextBox.Text.Length == 5)
            {
                key5TextBox.Focus();
                key5TextBox.SelectAll();
            }
        }

        protected virtual void EthernetCardPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, ethernetCardPanel);
            Manager.PaintText(g, Localization["SetupLicense_EthernetCard"]);
        }

        protected virtual void AmountInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, amountDoubleBufferPanel);
            Manager.PaintText(g, Localization["SetupLicense_LicenseQuantity"]);
            Manager.PaintTextRight(g, amountDoubleBufferPanel, Server.License.Amount.ToString());
        }

        protected virtual void LicenseInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, licenseKeyBufferPanel);
            Manager.PaintText(g, Localization["SetupLicense_LicenseKey"]);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            ShowLicenseAmount();
        }

        protected delegate void ShowLicenseAmountDelegate();

        protected virtual void ShowLicenseAmount()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new ShowLicenseAmountDelegate(ShowLicenseAmount));
                }
                catch (Exception)
                {
                }
                return;
            }

            amountDoubleBufferPanel.Visible = true;
            ethernetCardPanel.Visible = licenseKeyBufferPanel.Visible = false;

            infoContainerPanel.Controls.Clear();

            var temp = new List<Adaptor>(Server.License.Adaptor);
            temp.Reverse();
            foreach (var adaptor in temp)
            {
                var infoPanel = new InfoControl { Adaptor = adaptor };
                infoPanel.ParseInfo();
                infoContainerPanel.Controls.Add(infoPanel);
            }
            var formalLicenseCount = 0;
            foreach (var adaptor in Server.License.Adaptor)
            {
                foreach (var licenseInfo in adaptor.LicenseInfo)
                {
                    if (licenseInfo.Trial) continue;

                    formalLicenseCount += licenseInfo.Quantity;
                }
            }
            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "",
                    (formalLicenseCount < Server.License.Maximum) ? "OnlineRegistration,OfflineRegistration" : "")));
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "OnlineRegistration":
                    OnlineRegistration();
                    break;

                case "OfflineRegistration":
                    OfflineRegistration();
                    break;

                case "Confirm":
                    SubmitOnlineRegistration();
                    break;

                default:
                    if (item == TitleName || item == "Back")
                    {
                        ShowLicenseAmount();
                    }
                    break;
            }
        }

        protected virtual void OnlineRegistration()
        {
            infoContainerPanel.Controls.Clear();

            ethernetCardPanel.Visible = licenseKeyBufferPanel.Visible = true;
            amountDoubleBufferPanel.Visible = false;
            //upload .key(Xml) file to server

            key1TextBox.TextChanged -= Key1TextBoxTextChanged;
            key2TextBox.TextChanged -= Key2TextBoxTextChanged;
            key3TextBox.TextChanged -= Key3TextBoxTextChanged;
            key4TextBox.TextChanged -= Key4TextBoxTextChanged;

            ethernetComboBox.SelectedIndexChanged -= EthernetComboBoxSelectedIndexChanged;
            ethernetComboBox.Items.Clear();

            foreach (var adaptor in Server.License.Adaptor)
                ethernetComboBox.Items.Add(adaptor.Description);

            if (ethernetComboBox.Items.Count > 0)
            {
                ethernetComboBox.Enabled = true;
                ethernetComboBox.SelectedIndexChanged += EthernetComboBoxSelectedIndexChanged;
                ethernetComboBox.SelectedIndex = 0;
            }
            else
                ethernetComboBox.Enabled = false;

            Manager.DropDownWidth(ethernetComboBox);

            key1TextBox.Text = key2TextBox.Text = key3TextBox.Text = key4TextBox.Text = key5TextBox.Text = "";

            key1TextBox.TextChanged += Key1TextBoxTextChanged;
            key2TextBox.TextChanged += Key2TextBoxTextChanged;
            key3TextBox.TextChanged += Key3TextBoxTextChanged;
            key4TextBox.TextChanged += Key4TextBoxTextChanged;

            key1TextBox.Focus();

            var text = TitleName + "  /  " + Localization["SetupLicense_OnlineRegistration"];

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
        }

        protected virtual void SubmitOnlineRegistration()
        {
            if (ethernetComboBox.SelectedIndex < 0 || Server.License.Adaptor.Count == 0) return;

            if (key1TextBox.Text.Length != 5 || key2TextBox.Text.Length != 5 || key3TextBox.Text.Length != 5
                || key4TextBox.Text.Length != 5 || key5TextBox.Text.Length != 5)
            {
                TopMostMessageBox.Show(Localization["SetupLicense_CheckLicenseLength"], Localization["MessageBox_Error"],
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String[] key = new[]
			{
				key1TextBox.Text.ToUpper(),
				key2TextBox.Text.ToUpper(),
				key3TextBox.Text.ToUpper(),
				key4TextBox.Text.ToUpper(),
				key5TextBox.Text.ToUpper(),
			};

            DialogResult result = MessageBox.Show(Localization["SetupLicense_KeyConfirm"].Replace("%1", String.Join("-", key)), Localization["MessageBox_Information"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result != DialogResult.Yes) return;
            //wait 0.2sec to close dialog, avoid capture dialog as background image
            Thread.Sleep(200);

            var adaptor = Server.License.Adaptor[ethernetComboBox.SelectedIndex];
            var mac = adaptor.Mac.Replace("-", ":");
            Server.License.OnSaveComplete -= OnRegistrationComplete;
            Server.License.OnSaveComplete += OnRegistrationComplete;
            Server.License.Save(String.Join("-", key) + ";" + mac);
        }

        protected virtual void OnRegistrationComplete(Object sender, EventArgs e)
        {
            ShowLicenseAmount();
        }

        private void OfflineRegistration()
        {
            if (_openKeyDialog.ShowDialog() != DialogResult.OK) return;

            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(_openKeyDialog.OpenFile());
                String amount = Xml.GetFirstElementValueByTagName(xmlDoc, "NumberOfChannel");
                if (amount != "")
                {
                    DialogResult result = TopMostMessageBox.Show(Localization["SetupLicense_AmountConfirm"].Replace("%1", Convert.ToUInt16(amount).ToString()), Localization["MessageBox_Information"],
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        SubmitOfflineRegistration(xmlDoc.InnerXml);
                    }
                }
                else
                {
                    TopMostMessageBox.Show(Localization["SetupLicense_CheckLicenseFile"], Localization["MessageBox_Error"],
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                TopMostMessageBox.Show(Localization["SetupLicense_CheckLicenseFile"], Localization["MessageBox_Error"],
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void SubmitOfflineRegistration(String key)
        {
            Server.License.OnSaveComplete -= OnRegistrationComplete;
            Server.License.OnSaveComplete += OnRegistrationComplete;

            Server.License.Save(key);
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowLicenseAmount();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }

    public class KeyTextBox : TextBox
    {
        public event EventHandler<EventArgs<String>> OnPaste;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x302: //WM_PASTE
                    IDataObject clipData = Clipboard.GetDataObject();
                    if (clipData != null && OnPaste != null)
                    {
                        String key = ((String)clipData.GetData(DataFormats.Text));
                        if (key != null)
                        {
                            key = key.Replace("\r\n", "").Replace("-", "");
                            if (key.Length == 25)
                            {
                                OnPaste(this, new EventArgs<String>(key));
                                return;
                            }
                        }
                    }
                    base.WndProc(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}