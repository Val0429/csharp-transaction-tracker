using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public sealed partial class StorageControl : UserControl
    {
        public IServer Server;
        public event EventHandler OnDeviceKeepDaysRecording;
        private readonly Queue<StoragePanel> _recycleStorage = new Queue<StoragePanel>();
        public Dictionary<String, String> Localization;
        public StorageControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupServer_DeviceKeepDaysRecording", "Device keep days recording"},
                                   {"StorageControl_LessThanRequire", "Disk size less than %1GB will not be used as recording storage"},
                                   {"StorageControl_KeepSpaceValue", "Set keep space between %1GB and  %2GB under max capacity for best performance"},
                                   {"StorageControl_KeepSpaceSuggest", "Suggested minimum keep space amount:"},
                                   {"StorageControl_KeepSpaceSuggest1", "For total bitrate under 10Mb/s = 15GB"},
                                   {"StorageControl_KeepSpaceSuggest2", "For total bitrate 10Mb/s to 50Mb/s = 30GB"},
                                   {"StorageControl_KeepSpaceSuggest3", "For total bitrate 50Mb/s to 100Mb/s = 40GB"},
                                   {"StorageControl_KeepSpaceSuggest4", "For total bitrate 100Mb/s to 150Mb/s = 50GB"},
                                   {"StorageControl_KeepSpaceSuggest5", "For total bitrate 150Mb/s to 200Mb/s = 60GB"},
                                   {"StorageControl_PlaybackLoadStorage", "Select drive for the temp folder of video playback."},
                                   {"SetupGeneral_Disabled", "Disabled"},
                                   {"SetupGeneral_Enabled", "Enabled"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Storage";

            containerPanel.MouseUp += StorageControlMouseUp;

            infoLabel.Text = Localization["StorageControl_LessThanRequire"].Replace("%1", MinimumSizeRequire.ToString());
            infoLabel2.Text =
                Localization["StorageControl_KeepSpaceValue"].Replace("%1", MinimizeKeepSpace.ToString()).Replace("%2", MiniimizeRecordSpace.ToString()) +
                Environment.NewLine + Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest"] +
                Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest1"] +
                Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest2"] +
                Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest3"] +
                Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest4"] +
                Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest5"];

            BackgroundImage = Manager.BackgroundNoBorder;

            var days = new List<object>();
            for (int i = 1; i <= 60; i++)
                days.Add((ushort)i);

            deviceKeepDaysRecordingPanel.Tag = "DeviceKeepDaysRecording";
            deviceKeepDaysRecordingPanel.Paint += InputPanelPaint;

            deviceKeepDaysRecordingPanel.MouseClick += DeviceKeepDaysRecordingPanelMouseClick;
        }

        private void DeviceKeepDaysRecordingPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (OnDeviceKeepDaysRecording != null)
                OnDeviceKeepDaysRecording(this, null);
        }

        private static UInt16 MinimumSizeRequire = 30;
        private static UInt16 DefaultKeepSpace = 30;
        private static UInt16 MinimizeKeepSpace = 15;
        private static UInt16 MiniimizeRecordSpace = 50;
        private Boolean _isEditing;
        public void GeneratorStorageList()
        {
            _isEditing = false;
            containerPanel.Visible = false;
            ClearViewModel();

            if (Server.Device == null)
            {
                deviceKeepDaysRecordingPanel.Visible = false;
            }
            else
            {
                deviceKeepDaysRecordingPanel.Visible = deviceKeepDaysLabel.Visible = Server.Device.Devices.Count > 0;
            }

            MinimumSizeRequire = Server.Server.CheckProductNoToSupportNumber("minimumSizeRequire");
            DefaultKeepSpace = Server.Server.CheckProductNoToSupportNumber("defaultKeepSpace");
            MinimizeKeepSpace = Server.Server.CheckProductNoToSupportNumber("minimizeKeepSpace");
            MiniimizeRecordSpace = Server.Server.CheckProductNoToSupportNumber("maximizeKeepSpace");

            infoLabel2.Visible = MinimizeKeepSpace < MiniimizeRecordSpace;

            Boolean hasLessRequireStorage = false;
            foreach (KeyValuePair<String, DiskInfo> obj in Server.Server.StorageInfo)
            {
                StoragePanel storagePanel = GetStoragePanel();
                storagePanel.Order = 0;
                storagePanel.Tag = obj.Key;
                storagePanel.DisplayEditor = true;
                if (Server is ICMS)
                {
                    storagePanel.KeepSpace = 0;
                }
                else
                {
                    storagePanel.KeepSpace = DefaultKeepSpace;
                }
                storagePanel.DiskInfo = obj.Value;
                containerPanel.Controls.Add(storagePanel);
                storagePanel.BringToFront();

                if (storagePanel.DiskInfo.Total > MinimumSizeRequire * 1.0 * Gb2Byte)//30GB
                {
                    storagePanel.Enabled = true;
                }
                else
                {
                    hasLessRequireStorage = true;
                    storagePanel.Enabled = false;
                }
            }

            if (Server is ICMS)
            {
                infoLabel.Text = Localization["StorageControl_PlaybackLoadStorage"];
                infoLabel.Visible = true;
                infoLabel2.Visible = false;
            }
            else
            {
                infoLabel.Visible = hasLessRequireStorage;
            }

            var temp = new List<Storage>(Server.Server.ChangedStorage);
            temp.Reverse();

            UInt16 order = Convert.ToUInt16(temp.Count);
            foreach (Storage storage in temp)
            {
                foreach (StoragePanel storagePanel in containerPanel.Controls)
                {
                    if (storage.Key != storagePanel.Tag.ToString()) continue;

                    storagePanel.Order = order;
                    storagePanel.KeepSpace = storage.KeepSpace;
                    storagePanel.InUse = true;
                    storagePanel.SendToBack();
                    order--;
                    break;
                }
            }

            StoragePanel titlePanel = GetStoragePanel();
            titlePanel.Cursor = Cursors.Default;
            containerPanel.Controls.Add(titlePanel);
            containerPanel.Visible = true;
            containerPanel.SendToBack();

            deviceKeepDaysLabel.SendToBack();
            deviceKeepDaysRecordingPanel.SendToBack();

            _isEditing = true;
        }

        public void GenerateSISStorageList()
        {
            GeneratorStorageList();

            deviceKeepDaysRecordingPanel.Visible = deviceKeepDaysLabel.Visible = false;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Server.Device.Devices.Count == 0) return;
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            if (control.Tag.ToString() == "DeviceKeepDaysRecording")
            {
                Manager.PaintHighLightInput(g, control);
                Manager.PaintEdit(g, control);
                Manager.PaintTextRight(g, deviceKeepDaysRecordingPanel, !Server.Server.KeepDaysEnabled ? Localization["SetupGeneral_Disabled"] : Localization["SetupGeneral_Enabled"]);
            }
            else
            {
                Manager.PaintSingleInput(g, control);
            }

            string tagText = Localization.ContainsKey("SetupServer_" + control.Tag)
                ? Localization["SetupServer_" + control.Tag]
                : control.Tag.ToString();

            Manager.PaintText(g, tagText);
        }

        private void StoragePanelMouseClickForSingleSelect(StoragePanel storagePanel)
        {
            if (storagePanel.InUse) return;

            var drive = storagePanel.Tag.ToString();

            //Remove all options
            for (Int32 i = 0; i < Server.Server.ChangedStorage.Count; i++)
            {
                Server.Server.ChangedStorage.RemoveAt(i);
                break;
            }

            foreach (StoragePanel panel in containerPanel.Controls)
            {
                if (panel.Tag == null) continue;
                if (panel.Tag.ToString() == drive)
                {
                    panel.InUse = true;
                    continue;
                }
                panel.InUse = false;
            }

            if (storagePanel.InUse)
            {
                Server.Server.ChangedStorage.Insert(0, new Storage
                {
                    Key = drive,
                    Name = drive,
                    KeepSpace = storagePanel.KeepSpace,
                });
            }

            Invalidate();
        }

        private void StoragePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (!(sender is StoragePanel) || ((StoragePanel)sender).Tag == null) return;

            var storagePanel = sender as StoragePanel;

            if (Server is ICMS)
            {
                StoragePanelMouseClickForSingleSelect(storagePanel);
                return;
            }

            if (Server is IFOS)
            {
                if (storagePanel.InUse && Server.Server.ChangedStorage.Count == 1)
                    return;
            }

            if (Server.Server.Platform == Platform.Linux)
            {
                StoragePanelMouseClickForSingleSelect(storagePanel);
                return;
            }

            var drive = storagePanel.Tag.ToString();

            var inUse = false;
            foreach (Storage storage in Server.Server.ChangedStorage)
            {
                if (storage.Key != drive) continue;

                inUse = true;
                break;
            }

            //Remove
            if (inUse)
            {
                if (Server.Server.ChangedStorage.Count > 0) //> 1
                {
                    storagePanel.InUse = false;
                    storagePanel.Order = 0;

                    for (Int32 i = 0; i < Server.Server.ChangedStorage.Count; i++)
                    {
                        if (Server.Server.ChangedStorage[i].Key != drive) continue;

                        Server.Server.ChangedStorage.RemoveAt(i);
                        break;
                    }
                }
                else
                {
                    return;
                }
            }
            else//Add
            {
                storagePanel.InUse = true;
            }

            var temp = new List<StoragePanel>();
            foreach (StoragePanel panel in containerPanel.Controls)
            {
                if (panel.InUse)
                    temp.Add(panel);
            }
            temp.Reverse();

            UInt16 order = 1;
            foreach (StoragePanel panel in temp)
            {
                panel.Order = order;
                order++;
            }

            if (storagePanel.InUse)
            {
                Server.Server.ChangedStorage.Insert(temp.IndexOf(storagePanel), new Storage
                {
                    Key = drive,
                    Name = drive,
                    KeepSpace = storagePanel.KeepSpace,
                });
                StoragePanelOnKeepSpaceChange(storagePanel, null);
            }

            Invalidate();
        }

        private StoragePanel GetStoragePanel()
        {
            if (_recycleStorage.Count > 0)
            {
                return _recycleStorage.Dequeue();
            }

            var storagePanel = new StoragePanel
            {
                Server = Server,
            };

            storagePanel.MouseClick += StoragePanelMouseClick;

            if (Server is ICMS == false)
            {
                storagePanel.OnKeepSpaceChange += StoragePanelOnKeepSpaceChange;
                storagePanel.OnKeepSpaceLostFocus += StoragePanelOnKeepSpaceLostFocus;
                storagePanel.OnStorageDrag += StoragePanelOnStorageDrag;
            }

            return storagePanel;
        }

        private StoragePanel _dragStoragePanel;

        private const UInt32 Gb2Byte = 1073741824;

        private void StoragePanelOnKeepSpaceLostFocus(Object sender, EventArgs e)
        {
            if (!_isEditing || !(sender is StoragePanel) || ((StoragePanel)sender).Tag == null) return;

            var storagePanel = sender as StoragePanel;

            String drive = storagePanel.Tag.ToString();

            foreach (Storage storage in Server.Server.ChangedStorage)
            {
                if (storage.Key != drive) continue;
                storagePanel.KeepSpace = storage.KeepSpace;

                storagePanel.Invalidate();

                break;
            }
        }

        private void StoragePanelOnKeepSpaceChange(Object sender, EventArgs e)
        {
            if (!_isEditing || !(sender is StoragePanel) || ((StoragePanel)sender).Tag == null) return;

            var storagePanel = sender as StoragePanel;

            String drive = storagePanel.Tag.ToString();

            foreach (Storage storage in Server.Server.ChangedStorage)
            {
                if (storage.Key != drive) continue;
                UInt16 maxfree = Convert.ToUInt16(storagePanel.DiskInfo.Total / Gb2Byte);
                //WHY!? -> to make sure recorder have space for recording, keep space can't == total free space
                if (maxfree > MiniimizeRecordSpace)
                    maxfree -= MiniimizeRecordSpace;

                storage.KeepSpace = Math.Max(Math.Min(Math.Max(storagePanel.KeepSpace, MinimizeKeepSpace), maxfree), MinimizeKeepSpace);
                storagePanel.Invalidate();

                break;
            }
        }

        private void StoragePanelOnStorageDrag(Object sender, EventArgs e)
        {
            containerPanel.Cursor = Cursors.NoMoveVert;
            containerPanel.Capture = true;
            _dragStoragePanel = (StoragePanel)sender;
        }

        private void StorageControlMouseUp(Object sender, MouseEventArgs e)
        {
            containerPanel.Cursor = Cursors.Default;

            if (_dragStoragePanel == null) return;

            StoragePanel dragToStoragePanel = null;

            foreach (StoragePanel storagePanel in containerPanel.Controls)
            {
                if (storagePanel != _dragStoragePanel && storagePanel.InUse && storagePanel.Tag != null)
                {
                    if (Drag.IsDrop(storagePanel))
                    {
                        dragToStoragePanel = storagePanel;
                        break;
                    }
                }
            }

            if (dragToStoragePanel == null) return;

            var tag = _dragStoragePanel.Tag;
            _dragStoragePanel.Tag = dragToStoragePanel.Tag;
            dragToStoragePanel.Tag = tag;

            var keepSpace = _dragStoragePanel.KeepSpace;
            _dragStoragePanel.KeepSpace = dragToStoragePanel.KeepSpace;
            dragToStoragePanel.KeepSpace = keepSpace;

            var info = _dragStoragePanel.DiskInfo;
            _dragStoragePanel.DiskInfo = dragToStoragePanel.DiskInfo;
            dragToStoragePanel.DiskInfo = info;

            Server.Server.ChangedStorage.Clear();

            var temp = new List<StoragePanel>();
            foreach (StoragePanel panel in containerPanel.Controls)
            {
                if (panel.InUse && panel.Tag != null)
                    temp.Add(panel);
            }
            temp.Reverse();

            UInt16 order = 1;
            foreach (StoragePanel storagePanel in temp)
            {
                storagePanel.Order = order;
                Server.Server.ChangedStorage.Add(new Storage
                {
                    Key = storagePanel.Tag.ToString(),
                    KeepSpace = storagePanel.KeepSpace,
                });
                order++;
            }

            _dragStoragePanel = null;
            Invalidate();
            //GeneratorStorageList();
        }

        private void ClearViewModel()
        {
            foreach (StoragePanel storagePanel in containerPanel.Controls)
            {
                storagePanel.Order = 0;
                storagePanel.Tag = null;
                storagePanel.InUse = false;
                storagePanel.DiskInfo = null;
                storagePanel.Cursor = Cursors.Hand;
                storagePanel.DisplayEditor = false;
                if (!_recycleStorage.Contains(storagePanel))
                {
                    _recycleStorage.Enqueue(storagePanel);
                }
            }
            containerPanel.Controls.Clear();
        }
    }
}
