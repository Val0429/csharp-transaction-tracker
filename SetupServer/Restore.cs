using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class RestoreControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;
        private readonly List<String> _contents = new List<String>();
        public List<String> Contents
        {
            get
            {
                _contents.Clear();

                if (Server is ICMS)
                {
                    if (deviceCheckBox.Checked) _contents.Add("device");
                    if (userCheckBox.Checked) _contents.Add("user");
                    if (generalCheckBox.Checked) _contents.Add("general");
                    if (emapCheckBox.Checked) _contents.Add("emap");
                    if (storageCheckBox.Checked) _contents.Add("storage");
                    if (licenseCheckBox.Checked) _contents.Add("license");
                    if (nvrCheckBox.Checked) _contents.Add("nvr");
                    if (serverCheckBox.Checked) _contents.Add("server");
                }
                else
                {
                    if (deviceCheckBox.Checked) _contents.Add("device");
                    if (userCheckBox.Checked) _contents.Add("user");
                    if (generalCheckBox.Checked) _contents.Add("general");
                    if (storageCheckBox.Checked) _contents.Add("storage");
                    if (licenseCheckBox.Checked) _contents.Add("license");
                    if (serverCheckBox.Checked) _contents.Add("server");
                }

                return _contents;
            }
        }
        public Stream Stream;
        public String FileName { get; set; }
        public string FilePath { get { return _openSettingDialog.FileName; } }
        private readonly OpenFileDialog _openSettingDialog;


        public RestoreControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"SetupGeneral_Include", "Include"},

                {"Control_License", "License"},

                {"SetupServer_SettingFile", "Setting File"},
                {"SetupServer_NoSelectedFile", "No selected file"},
                {"SetupServer_Browse", "Browse"},
                {"SetupServer_DeviceSetting", "Device Setting"},
                {"SetupServer_UserSetting", "User Setting"},
                {"SetupServer_GeneralSetting", "General Setting"},
                {"SetupServer_EMapSetting", "EMap Setting"},
                {"SetupServer_LicenseSetting", "License Setting"},
                {"SetupServer_StorageSetting", "Storage Setting"},
                {"SetupServer_NVRSetting", "NVR Setting"},
                {"SetupServer_ServerSetting", "Server Setting"},
                {"SetupServer_CardSetting", "Card Setting"},
                {"SetupServer_DoorSetting", "Door Setting"},
                {"SetupServer_EventHandlerSetting", "Event Handler Setting"},
                {"SetupServer_LPRDeviceSetting", "LPR Device Setting"},
                {"SetupServer_LPRGroupSetting", "LPR Group Setting"},
                {"SetupServer_RestoreLicenseWarning", "Don’t restore this license to a different NVR Server. Improper license restoration will make the NVR Server registration fail."},
                {"SetupServer_RestoreLPRWarning", "When you restore LPR Device setting, please also check the device setting, NVR setting and LPR device setting; otherwise restore it will fail."},
            };
            Localizations.Update(Localization);

            InitializeComponent();

            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Restore";

            _openSettingDialog = new OpenFileDialog
            {
                Filter = Localization["SetupServer_SettingFile"] + @" (.setting)|*.setting"
            };

            filePanel.Paint += FilePanelPaint;
            devicePanel.Paint += PanelPaint;
            userPanel.Paint += PanelPaint;
            generalPanel.Paint += PanelPaint;
            emapPanel.Paint += PanelPaint;
            storagePanel.Paint += PanelPaint;
            nvrPanel.Paint += PanelPaint;
            serverPanel.Paint += PanelPaint;
            _licensePlateOwnerPanel.Paint += PanelPaint;

            licenseLabelPanel.Paint += LicenseLabelPanelPaint;
            licensePanel.Paint += LicensePanelPaint;

            _lprDevicePanel.Text = Localization["SetupServer_LPRDeviceSetting"];
            _doorPanel.Text = Localization["SetupServer_DoorSetting"];
            _eventHandlerControl.Text = Localization["SetupServer_EventHandlerSetting"];
            _cardPanel.Text = Localization["SetupServer_CardSetting"];

            warningLabel.Text = Localization["SetupServer_RestoreLicenseWarning"];
            _sisWarningLabel.Visible = false;
            _sisWarningLabel.Text = Localization["SetupServer_RestoreLPRWarning"];

            browserButton.Text = Localization["SetupServer_Browse"];
            deviceCheckBox.Text = userCheckBox.Text = generalCheckBox.Text = emapCheckBox.Text = licenseCheckBox.Text =
                storageCheckBox.Text = nvrCheckBox.Text = serverCheckBox.Text = Localization["SetupGeneral_Include"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }


        private void PanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);
            if (Width <= 100) return;

            if (Localization.ContainsKey("SetupServer_" + control.Tag))
                Manager.PaintText(g, Localization["SetupServer_" + control.Tag], (control.Enabled) ? Brushes.Black : Brushes.Gray);
            else
                Manager.PaintText(g, control.Tag.ToString(), (control.Enabled) ? Brushes.Black : Brushes.Gray);
        }

        private void LicenseLabelPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["Control_License"], Manager.Font, Brushes.DimGray, 8, 10);
        }

        private void LicensePanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, control);
            if (Width <= 100) return;

            Manager.PaintText(g, Localization["SetupServer_LicenseSetting"], (control.Enabled) ? Brushes.Black : Brushes.Gray);
        }

        public void Reset()
        {
            _contents.Clear();
            Stream = null;

            if (Server is ICMS)
            {
                nvrPanel.Visible = emapPanel.Visible = true;
            }
            else
            {
                storagePanel.Visible = true;
                nvrPanel.Visible = emapPanel.Visible = false;
            }

            deviceCheckBox.Checked = userCheckBox.Checked = generalCheckBox.Checked = emapCheckBox.Checked =
                _lprDeviceCheckBox.Checked = _licensePlateOwnerCheckBox.Checked = _cardCheckBox.Checked = _doorCheckBox.Checked =
                licenseCheckBox.Checked = storageCheckBox.Checked = nvrCheckBox.Checked = serverCheckBox.Checked = false;

            warningLabel.Visible = false;
            _sisWarningLabel.Visible = false;

            devicePanel.Enabled = userPanel.Enabled = generalPanel.Enabled = emapPanel.Enabled =
                _lprDevicePanel.Enabled = _licensePlateOwnerPanel.Enabled = _cardPanel.Enabled = _doorPanel.Enabled =
                licensePanel.Enabled = storagePanel.Enabled = nvrPanel.Enabled = serverPanel.Enabled = false;
        }

        private void FilePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, filePanel);
            if (Width <= 100) return;

            String title = Localization["SetupServer_SettingFile"];

            title = title + " : ";

            if (Stream == null)
            {
                title = title + Localization["SetupServer_NoSelectedFile"];
                Manager.PaintText(g, title, Brushes.Black);
            }
            else
            {
                title = title + FileName;

                Manager.PaintText(g, title, Manager.SelectedTextColor);
            }
        }

        private void BrowserButtonClick(Object sender, EventArgs e)
        {
            if (_openSettingDialog.ShowDialog() != DialogResult.OK) return;

            devicePanel.Enabled = userPanel.Enabled = generalPanel.Enabled = emapPanel.Enabled =
                _lprDevicePanel.Enabled = _licensePlateOwnerPanel.Enabled = _cardPanel.Enabled = _doorPanel.Enabled =
                licensePanel.Enabled = storagePanel.Enabled = nvrPanel.Enabled = serverPanel.Enabled = true;

            Stream = _openSettingDialog.OpenFile();
            var fileInfo = new FileInfo(_openSettingDialog.FileName);
            FileName = fileInfo.Name;

            filePanel.Invalidate();
        }

        private void LicenseCheckBoxMouseClick(Object sender, MouseEventArgs e)
        {
            warningLabel.Visible = licenseCheckBox.Checked;
        }

        public void SetLprView()
        {
            Localization["SetupServer_RestoreLicenseWarning"] = "Don’t restore this license to a different SIS Server. Improper license restoration will make the SIS Server registration fail.";
            Localizations.Update(Localization);

            warningLabel.Text = Localization["SetupServer_RestoreLicenseWarning"];

            emapPanel.Visible = false;
            _sisWarningLabel.Visible = true;
            _lprDevicePanel.Visible = true;
            _licensePlateOwnerPanel.Visible = true;

            _sisWarningLabel.Visible = false;
        }

        public void SetAcsView()
        {
            Localization["SetupServer_RestoreLicenseWarning"] = "Don’t restore this license to a different SIS Server. Improper license restoration will make the SIS Server registration fail.";
            Localization["SetupServer_RestoreDoorWarning"] = "When you restore Door setting, please also check the Card setting; otherwise restore it might fail.";
            Localizations.Update(Localization);

            warningLabel.Text = Localization["SetupServer_RestoreLicenseWarning"];
            _sisWarningLabel.Text = Localization["SetupServer_RestoreDoorWarning"];

            emapPanel.Visible = false;
            _sisWarningLabel.Visible = true;
            _cardPanel.Visible = true;
            _eventHandlerControl.Visible = true;
            _doorPanel.Visible = true;

            _sisWarningLabel.Visible = false;
        }

        public void SetMobileView()
        {
            emapPanel.Visible = false;
        }


        public bool IncludeLprDeviceSetting { get { return _lprDeviceCheckBox.Checked; } }

        public bool LicensePlateOwnerSetting { get { return _licensePlateOwnerCheckBox.Checked; } }

        public bool IncludeCardSetting { get { return _cardCheckBox.Checked; } }

        public bool IncludeDoorSetting { get { return _doorCheckBox.Checked; } }

        public bool IncludeEventHandlerSetting { get { return _eventHandlerCheckBox.Checked; } }

        private void LprDeviceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _sisWarningLabel.Visible = _lprDeviceCheckBox.Checked;
        }

        private void DoorCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            containerPanel.SuspendLayout();
            _sisWarningLabel.Visible = _doorCheckBox.Checked;

            if (!_doorCheckBox.Checked)
            {
                _eventHandlerCheckBox.Checked = false;
            }
            _eventHandlerControl.Visible = _doorCheckBox.Checked;
            containerPanel.ResumeLayout();
            containerPanel.PerformLayout();
        }

        private void RestoreControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _eventHandlerControl.Visible = _doorCheckBox.Checked;
            }
        }
    }
}
