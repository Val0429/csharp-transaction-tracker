using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public partial class OverallControl : UserControl
    {
        public event EventHandler OnPortEdit;
        public event EventHandler OnSSLPortEdit;
        public event EventHandler OnDateTimeEdit;
        public event EventHandler OnStorageEdit;
        public event EventHandler OnRestore;
        public event EventHandler OnPowerEdit;
        public event EventHandler OnRAIDEdit;
        public event EventHandler OnUpgradeEdit;
        public event EventHandler OnStoreEdit;
        public event EventHandler OnDBEdit;
        public event EventHandler OnDevicePackEdit;
        public event EventHandler OnArchiveServerEdit;

        public event EventHandler<EventArgs<UInt16>> OnEthernetEdit;

        public Dictionary<String, String> Localization;

        public IApp App;
        public IServer Server;
        private ICMS _cms;
        private IFOS _fos;
        private IPTS _pts;
        public OverallControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                    {"SetupServer_Port", "Port"},
                                    {"SetupServer_SSLPort", "SSL Port"},
                                    {"SetupServer_Storage", "Storage"},
                                    {"SetupServer_Ethernet", "Ethernet"},
                                    {"SetupServer_RAID", "RAID"},
                                    {"SetupServer_DateTime", "Date Time"},
                                    {"SetupServer_Power", "Power"},
                                    {"SetupServer_Upgrade", "Upgrade"},
                                    {"SetupServer_Component", "Component"},
                                    {"SetupServer_Version", "Version"},
                                    {"SetupServer_Backup", "Backup"},
                                    {"SetupServer_Restore", "Restore"},
                                    {"SetupServer_Status", "Status"},
                                    {"SetupServer_Progress", "Progress"},
                                    {"SetupServer_Store", "Store"},
                                    {"SetupServer_DB", "Database"},
                                    {"SetupServer_DevicePackVersion", "Device Pack Version"},
                                    {"SetupServer_ArchiveServer", "Archive Server"},

                                    {"SetupServer_NoStorageDiskSelected", "The storage disk is not selected"},
                                    {"SetupServer_RAIDUnselected","RAID mode is not selected."},
                                    {"SetupServer_Standby", "Standby"},
                                    {"SetupServer_FailoverRecording", "Failover Recording"},
                                    {"SetupServer_Synchronize", "Synchronize"},
                                    {"SetupServer_MergeDatabase", "Merge Database"},
                                    {"SetupServer_Enabled", "Enabled"},
                                    {"SetupServer_UPNP", "UPNP"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        //private IViewer _viewer;
        public virtual void Initialize()
        {
            //if (Server.Utility != null)
            //{
            //    utilityPanel.Tag = Localization["SetupServer_Component"] + " : " + Server.Utility.ComponentName;
            //    utilityPanel.Paint += UtilityPanelPaint;
            //}

            //_viewer = App.RegistViewer();
            //if (_viewer != null)
            //{
            //    viewerPanel.Tag = Localization["SetupServer_Component"] + " : " + _viewer.ComponentName;
            //    viewerPanel.Paint += ViewerPanelPaint;
            //}

            //jane say dont display version panel
            Controls.Remove(viewerLabel);
            Controls.Remove(viewerPanel);
            Controls.Remove(utilityLabel);
            Controls.Remove(utilityPanel);

            ethernet1Panel.Tag = String.Format(Localization["SetupServer_Ethernet"] + " {0}", 1);
            ethernet2Panel.Tag = String.Format(Localization["SetupServer_Ethernet"] + " {0}", 2);

            storagePanel.Paint += StoragePanelPaint;
            backupPanel.Paint += InputPanelPaint;
            restorePanel.Paint += RestorePanelPaint;
            dateTimePanel.Paint += DateTimePanelPaint;
            ethernet1Panel.Paint += Ethernet1PanelPaint;
            ethernet2Panel.Paint += Ethernet2PanelPaint;
            RAIDPanel.Paint += RAIDPanelPaint;
            upgradePanel.Paint += UpgragePanelPaint;
            powerPanel.Paint += PowerPanelPaint;
            portPanel.Paint += PortPanelPaint;
            sslportPanel.Paint += SSLPortPanelPaint;
            statusPanel.Paint += StatusPanelPaint;
            progressPanel.Paint += InputPanelPaint;
            storePanel.Paint += StorePanelPaint;
            dbPanel.Paint += DBPanelPaint;
            devicePackPanel.Paint += DevicePackPanelPaint;
            archivePanel.Paint += ArchivePanelPaint;
            upnpDoubleBufferPanel.Paint += UPNPPanelPaint;

            upnpCheckBox.Text = Localization["SetupServer_Enabled"];
            upnpCheckBox.Checked = Server.Server.EnableUPNP;
            upnpCheckBox.Click += UNPNEnabledCheckBoxClick;

            if (Server is ICMS)
            {
                //storagePanel.Visible = storageLabel.Visible = false;
                archivePanel.Visible = archiveLabel.Visible = Server.Server.SupportAchiveServer;
                _cms = Server as ICMS;
            }
            else if (Server is IVAS)
            {
                storagePanel.Visible = storageLabel.Visible =
                backupPanel.Visible = backupLabel.Visible =
                restorePanel.Visible = restoreLabel.Visible = false;
                //utilityPanel.Visible = utilityLabel.Visible =
                //viewerPanel.Visible = viewerLabel.Visible = false;
            }
            else if (Server is IFOS)
            {
                _fos = Server as IFOS;
                sslportPanel.Visible = sslportLabel.Visible =
                backupPanel.Visible = backupLabel.Visible =
                restorePanel.Visible = restoreLabel.Visible = false;
                //utilityPanel.Visible = utilityLabel.Visible =
                //viewerPanel.Visible = viewerLabel.Visible = false;

                statusPanel.Visible = statusLabel.Visible = true;
            }
            else if (Server is IPTS)
            {
                _pts = Server as IPTS;
                storagePanel.Visible = storageLabel.Visible =
                backupPanel.Visible = backupLabel.Visible =
                upnpDoubleBufferPanel.Visible = upnpLabel.Visible =
                restorePanel.Visible = restoreLabel.Visible = false;
                //utilityPanel.Visible = utilityLabel.Visible =
                //viewerPanel.Visible = viewerLabel.Visible = false;

                storePanel.Visible = storeLabel.Visible = true;
                dbPanel.Visible = dbLabel.Visible = true;

                sslportPanel.Visible = sslportLabel.Visible = false;
            }
            else
            {
                if (Server.Server.Platform == Platform.Linux)
                {
                    storagePanel.Visible = storageLabel.Visible = Server.Server.CheckProductNoToSupport("storage");
                    dateTimePanel.Visible = dateTimeLabel.Visible = Server.Server.CheckSetupEnabled("DataTime");
                    if ((Server.Server.CheckSetupEnabled("Ethernet1") && Server.Server.Ethernets.ContainsKey(1)))
                    {
                        ethernet1Panel.Visible = ethernet1Label.Visible = Server.Server.Ethernets[1].DeviceExist;
                    }
                    else
                    {
                        ethernet1Panel.Visible = ethernet1Label.Visible = false;
                    }

                    if ((Server.Server.CheckSetupEnabled("Ethernet2") && Server.Server.Ethernets.ContainsKey(2)))
                    {
                        ethernet2Panel.Visible = ethernet2Label.Visible = Server.Server.Ethernets[2].DeviceExist;
                    }
                    else
                    {
                        ethernet2Panel.Visible = ethernet2Label.Visible = false;
                    }

                    portPanel.Visible = portLabel.Visible = Server.Server.CheckSetupEnabled("Port");
                    RAIDPanel.Visible = RAIDlabel.Visible = ((Server.Server.CheckSetupEnabled("RAID") || Server.Server.CheckSetupEnabled("KeepSpace")) && Server.Server.CheckProductNoToSupport("raid"));
                    if (!Server.Server.CheckSetupEnabled("RAID") && Server.Server.CheckSetupEnabled("KeepSpace"))
                        RAIDPanel.Tag = "Storage";

                    backupPanel.Visible = backupLabel.Visible = Server.Server.CheckSetupEnabled("Backup");
                    restorePanel.Visible = restoreLabel.Visible = Server.Server.CheckSetupEnabled("Restore");
                    if (Server.Server.CheckSetupEnabled("Upgrade"))
                    {
                        upgradePanel.Visible = upgradeLabel.Visible = Server.Server.CheckProductNoToSupport("upgrade");
                    }
                    else
                    {
                        upgradePanel.Visible = upgradeLabel.Visible = false;
                    }

                    powerPanel.Visible = powerLabel.Visible = Server.Server.CheckSetupEnabled("Power");
                }
                else //it's windows nvr
                {
                    viewerPanel.Visible = utilityPanel.Visible = false;
                    storagePanel.Visible = storageLabel.Visible = true;
                    devicePackPanel.Visible = devicePackLabel.Visible = true;
                }
            }

        }

        public void ParserSetting()
        {
            upnpCheckBox.Checked = Server.Server.EnableUPNP;
        }

        private void UNPNEnabledCheckBoxClick(object sender, EventArgs e)
        {
            Server.Server.EnableUPNP = upnpCheckBox.Checked;
        }

        public void DisplayProgress()
        {
            if (_fos != null && _fos.NVR.FailoverStatus == FailoverStatus.Synchronize)
            {
                progressValueLabel.Text = _fos.NVR.SynchronizeProgress + @"%";
                progressBar.Value = _fos.NVR.SynchronizeProgress;
                progressPanel.Visible = progressLabel.Visible = true;
            }
            else
            {
                progressValueLabel.Text = @"0%";
                progressBar.Value = 0;
                progressPanel.Visible = progressLabel.Visible = false;
            }
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            if(_pts != null)
            {
                Manager.PaintHighLightInput(g, control);
            }
            else
            {
                Manager.PaintSingleInput(g, control);
            }

            string controlText = Localization.ContainsKey("SetupServer_" + control.Tag)
                ? Localization["SetupServer_" + control.Tag] : control.Tag.ToString();

            Manager.PaintText(g, controlText);
        }

        //private void UtilityPanelPaint(Object sender, PaintEventArgs e)
        //{
        //    InputPanelPaint(sender, e);

        //    Graphics g = e.Graphics;

        //    if (Server.Utility != null)
        //        Manager.PaintTextRight(g, utilityPanel, Server.Utility.Version);
        //}

        //public void ViewerPanelPaint(Object sender, PaintEventArgs e)
        //{
        //    InputPanelPaint(sender, e);

        //    Graphics g = e.Graphics;

        //    Manager.PaintTextRight(g, viewerPanel, _viewer.Version);
        //}

        public void RestorePanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;

            Manager.PaintEdit(g, restorePanel);
        }

        private const UInt32 Gb2Byte = 1073741824;
        private void StoragePanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            if (Server == null) return;
            if (Server.Server.StorageInfo.Count == 0) return;

            Graphics g = e.Graphics;

            String[] storages = (from storage in Server.Server.ChangedStorage
                                 where Server.Server.StorageInfo.ContainsKey(storage.Key)
                                 select storage.Key + (Server.Server.StorageInfo[storage.Key].Total / Gb2Byte).ToString(" (0GB)")).ToArray();
            //String[] storages = Server.Server.Storage.Select(storage => storage.Key + (Server.Server.StorageInfo[storage.Key].Total / Gb2Byte).ToString(" (0GB)")).ToArray();
            if (storages.Length > 0)
                Manager.PaintTextRight(g, storagePanel, String.Join(", ", storages));
            else
                Manager.PaintTextRight(g, storagePanel, Localization["SetupServer_NoStorageDiskSelected"], Manager.DeleteTextColor);

            Manager.PaintEdit(g, storagePanel);
        }

        private void PortPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            var brush = (Server.Server.Port == Server.Server.SSLPort) ? Manager.DeleteTextColor : Brushes.Black;

            Graphics g = e.Graphics;
            if (Server.Server.Port > 0)
                Manager.PaintTextRight(g, portPanel, Server.Server.Port.ToString(), brush);
            Manager.PaintEdit(g, portPanel);
        }

        private void SSLPortPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            var brush =
                (Server.Server.Port == Server.Server.SSLPort)
                    ? Manager.DeleteTextColor
                    : Brushes.Black;

            Graphics g = e.Graphics;
            if (Server.Server.SSLPort > 0)
                Manager.PaintTextRight(g, sslportPanel, Server.Server.SSLPort.ToString(), brush);
            Manager.PaintEdit(g, sslportPanel);
        }

        private void StatusPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            if (_fos == null) return;

            Graphics g = e.Graphics;

            var brushes = Brushes.Black;
            var status = Localization["SetupServer_Standby"];
            switch (_fos.NVR.FailoverStatus)
            {
                case FailoverStatus.Recording:
                    status = Localization["SetupServer_FailoverRecording"];
                    break;

                case FailoverStatus.Synchronize:
                    status = Localization["SetupServer_Synchronize"];
                    break;

                case FailoverStatus.MergeDatabase:
                    status = Localization["SetupServer_MergeDatabase"];
                    break;
            }

            if (_fos.NVR.FailoverStatus != FailoverStatus.Ping)
            {
                brushes = Manager.SelectedTextColor;
                foreach (var obj in _fos.NVR.NVRs)
                {
                    if (obj.Value.FailoverSetting.ActiveProfile)
                    {
                        status += " (" + obj.Value + ", " + obj.Value.Credential.Domain + ")";
                        break;
                    }
                }
            }
            Manager.PaintTextRight(g, statusPanel, status, brushes);
        }

        private void DateTimePanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, portPanel, (Server.Server.EnableNTPServer) ? Server.Server.NTPServer : Server.Server.ChangedLocation);
            Manager.PaintEdit(g, portPanel);
        }

        private void Ethernet1PanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, portPanel, Server.Server.Ethernets[1].IPAddress);
            Manager.PaintEdit(g, portPanel);
        }

        private void Ethernet2PanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, portPanel, Server.Server.Ethernets[2].IPAddress);
            Manager.PaintEdit(g, portPanel);
        }

        private void RAIDPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, portPanel, ReadRAIDMode(Server.Server.RAID.Mode), Server.Server.RAID.Mode == RAIDMode.None ? Manager.DeleteTextColor : Brushes.Black);
            Manager.PaintEdit(g, portPanel);
        }

        private void UpgragePanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintEdit(g, portPanel);
        }

        private void PowerPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintEdit(g, portPanel);
        }

        private void StorePanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            if (_pts == null) return;
            var name = _pts.Configure.Store.ToString();
            if (name.Length > 25)
                name = name.Substring(0, 25) + "...";

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, storePanel, name);
            Manager.PaintEdit(g, storePanel);
        }

        private void DBPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            if (_pts == null) return;

            var brush =
                ((Server.Server.Database.Port == Server.Server.Port || Server.Server.Database.Port == Server.Server.SSLPort))
                    ? Manager.DeleteTextColor
                    : Brushes.Black;

            Graphics g = e.Graphics;
            if (!String.IsNullOrEmpty(_pts.Server.Database.Domain))
                Manager.PaintTextRight(g, dbPanel, _pts.Server.Database.Port.ToString(), brush);
            //Manager.PaintTextRight(g, dbPanel, _pts.Server.Database.Domain + @":" + _pts.Server.Database.Port);
            Manager.PaintEdit(g, dbPanel);
        }

        private void DevicePackPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(sender, e);

            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, devicePackPanel, Server.Server.DevicePackVersion);
            Manager.PaintEdit(g, devicePackPanel);
        }

        private void ArchivePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_cms == null) return;
            InputPanelPaint(sender, e);
            
            Graphics g = e.Graphics;
            Manager.PaintTextRight(g, archivePanel, _cms.NVRManager.ArchiveServer.Domain);
            Manager.PaintEdit(g, archivePanel);
        }

        protected void UPNPPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            if (Server is IPTS)
            {
                Manager.PaintHighLightInput(g, control);
            }
            else
            {
                Manager.PaintSingleInput(g, control);
            }

            if (Localization.ContainsKey("SetupServer_" + control.Tag))
                Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private String ReadRAIDMode(RAIDMode mode)
        {
            switch (mode)
            {
                case RAIDMode.RAID0:
                    return "RAID 0";
                case RAIDMode.RAID1:
                    return "RAID 1";
                case RAIDMode.RAID5:
                    return "RAID 5";
                case RAIDMode.RAID10:
                    return "RAID 10";
                default:
                    return Localization["SetupServer_RAIDUnselected"];
            }
        }
        //--------------------------------------------------------------------------------------------

        private void PortMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPortEdit != null)
                OnPortEdit(this, null);
        }

        private void StorageMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnStorageEdit != null)
                OnStorageEdit(this, null);
        }

        private void BackupPanelMouseClick(Object sender, MouseEventArgs e)
        {
            ApplicationForms.ShowLoadingIcon(Server.Form);
            Server.Server.Backup();
            ApplicationForms.HideLoadingIcon();
        }

        private void RestorePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnRestore != null)
                OnRestore(this, null);
        }

        private void DateTimePanelClick(object sender, EventArgs e)
        {
            if (OnDateTimeEdit != null)
                OnDateTimeEdit(this, null);
        }

        private void Ethernet1PanelClick(object sender, EventArgs e)
        {
            if (OnEthernetEdit != null)
                OnEthernetEdit(this, new EventArgs<UInt16>(1));
        }

        private void Ethernet2PanelClick(object sender, EventArgs e)
        {
            if (OnEthernetEdit != null)
                OnEthernetEdit(this, new EventArgs<UInt16>(2));
        }

        private void RAIDPanelClick(object sender, EventArgs e)
        {
            if (OnRAIDEdit != null)
                OnRAIDEdit(this, null);
        }

        private void UpgradePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnUpgradeEdit != null)
                OnUpgradeEdit(this, null);
        }

        private void PowerPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPowerEdit != null)
                OnPowerEdit(this, null);
        }

        private void StorePanelClick(Object sender, EventArgs e)
        {
            if (OnStoreEdit != null)
                OnStoreEdit(this, null);
        }

        private void DBPanelClick(Object sender, EventArgs e)
        {
            if (OnDBEdit != null)
                OnDBEdit(this, null);
        }

        private void SSLPortPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSSLPortEdit != null)
                OnSSLPortEdit(this, null);
        }

        private void DevicePackPanelClick(Object sender, EventArgs e)
        {
            if (OnDevicePackEdit != null)
                OnDevicePackEdit(this, null);
        }

        private void ArchivePanelClick(Object sender, EventArgs e)
        {
            if (OnArchiveServerEdit != null)
                OnArchiveServerEdit(this, null);
        }
    }
}
