using Constant;
using Interface;
using PanelBase;
using SetupDevice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Manager = SetupBase.Manager;

namespace SetupNVR
{
    public partial class SearchPanel : UserControl
    {
        public event EventHandler OnSearchStart;
        public event EventHandler OnSearchComplete;
        public event EventHandler OnNVRModify;

        public IServer Server;
        public ICMS CMS;
        public Dictionary<String, String> Localization;
        public Queue<String> Manufacturers = new Queue<String>();
        private String _searchingManufacture;
        //private readonly Timer _dotTimer = new Timer();
        private String[] _manufacture;


        public SearchPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Data_NotSupport", "Not Support"},

								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},
                                   {"SetupDevice_AllManufacturer", "Manufacturers"},
                                   {"SetupNVR_MaximumAmount", "Reached maximum number of NVR limit \"%1\""},
								   {"SetupNVR_SearchNoResult", "No NVR Found"},
								   {"SetupNVR_SearchNVRFound", "%1 NVR Found"},
								   {"SetupNVR_SearchNVRsFound", "%1 NVRs Found"},
								   {"SetupNVR_NVRSelected", "%1 NVR Selected"},
								   {"SetupNVR_NVRsSelected", "%1 NVRs Selected"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            BackgroundImage = Manager.BackgroundNoBorder;
            DoubleBuffered = true;
            Dock = DockStyle.None;
        }

        private readonly List<CheckBox> _manufacturesCheckBox = new List<CheckBox>();

        protected virtual string[] CreateManufactureList()
        {
            if (CMS != null)
            {
                return new[] { "iSAP", "Diviotec", "Hikvision", "VioStor", "DIGIEVER", "SAMSUNG", "Digifort" };//, "Digifort", "VIVOTEK", "ACTi", "Milestone", "Salient"
            }
            return new string[] { };
        }

        public void Initialize()
        {
            manufacturesLabel.Text = Localization["SetupDevice_AllManufacturer"];

            var list = CreateManufactureList();
            Array.Sort(list);
            _manufacture = list;

            foreach (var manufacture in _manufacture)
            {
                var checkBox = new CheckBox
                {
                    Text = manufacture,
                    Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    MinimumSize = new Size(115, 0),
                    AutoSize = true,
                    Padding = new Padding(0, 4, 0, 0)
                };
                _manufacturesCheckBox.Add(checkBox);
                manufacturesPanel.Controls.Add(checkBox);
            }
        }

        private readonly Queue<Label> _recycleLabels = new Queue<Label>();
        private Label GetResultLabel()
        {
            if (_recycleLabels.Count > 0)
            {
                return _recycleLabels.Dequeue();
            }

            return new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Padding = new Padding(8, 10, 0, 2),
                Size = new Size(456, 30),
                TextAlign = ContentAlignment.BottomLeft,
                ForeColor = SystemColors.ControlDarkDark
            };
        }

        private readonly Queue<DeviceResultPanel> _recyclePanels = new Queue<DeviceResultPanel>();
        private DeviceResultPanel GetResultPanel()
        {
            if (_recyclePanels.Count > 0)
            {
                return _recyclePanels.Dequeue();
            }

            return new DeviceResultPanel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                MinimumSize = new Size(0, 15),
                Size = new Size(456, 15)
            };
        }

        private readonly Queue<NVRPanel> _recycleDevice = new Queue<NVRPanel>();
        private NVRPanel GetDevicePanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = CreateNvrPanel();
            devicePanel.OnSelectChange += NVRPanelOnSelectChange;

            return devicePanel;
        }

        protected virtual NVRPanel CreateNvrPanel()
        {
            var devicePanel = new NVRPanel
            {
                SelectionVisible = true,
                Server = Server,
                EditVisible = false,
                DataType = "Search"
            };
            return devicePanel;
        }

        public void ApplyManufactures(String manufacturer)
        {
            foreach (var checkBox in _manufacturesCheckBox)
            {
                checkBox.Checked = (String.Equals(checkBox.Text, manufacturer)); 
            }
        }

        private Boolean _isSearching;
        private readonly Queue<Label> _noResultLabels = new Queue<Label>();
        private delegate List<INVR> SearchNVRDelegate(String manufacturer);
        private readonly Stopwatch _watch = new Stopwatch();
        public void SearchNVR()
        {
            if (_isSearching) return;

            _isEditing = false;

            Manufacturers.Clear();

            foreach (var checkBox in _manufacturesCheckBox)
            {
                if (checkBox.Checked)
                {
                    Manufacturers.Enqueue(checkBox.Text);
                }
            }

            if (Manufacturers.Count == 0) return;

            ClearViewModel();

            ApplicationForms.ShowLoadingIcon(Server.Form);

            _noResultLabels.Clear();
            _isSearching = true;
            _watch.Reset();
            _watch.Start();
            _searchingManufacture = Manufacturers.Dequeue();
            SearchNVRDelegate searchDeviceDelegate = CMS.NVRManager.SearchNVR;
            searchDeviceDelegate.BeginInvoke(_searchingManufacture, SearchNVRCallback, searchDeviceDelegate);

            if (OnSearchStart != null)
                OnSearchStart(this, null);
        }

        private delegate void SearchDeviceCallbackDelegate(IAsyncResult result);
        private void SearchNVRCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new SearchDeviceCallbackDelegate(SearchNVRCallback), result);
                }
                catch (Exception)
                {
                    _watch.Stop();
                    SearchCompleted();
                }
                return;
            }

            var searchResult = ((SearchNVRDelegate)result.AsyncState).EndInvoke(result);

            _watch.Stop();

            var searchPanel = GetResultPanel();
            searchPanel.ResultLabel = GetResultLabel();

            containerPanel.Controls.Add(searchPanel.ResultLabel);
            searchPanel.ResultLabel.BringToFront();
            searchPanel.ResultLabel.Text = String.Empty;
            searchPanel.ResultLabel.Tag = String.Empty;
            if (searchResult.Count == 0)
            {
                searchPanel.ResultLabel.Text = Localization["SetupNVR_SearchNoResult"];
                //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
                _noResultLabels.Enqueue(searchPanel.ResultLabel);
            }
            else
            {
                searchPanel.ResultLabel.Tag += ((searchResult.Count == 1)
                            ? Localization["SetupNVR_SearchNVRFound"]
                            : Localization["SetupNVR_SearchNVRsFound"]).Replace("%1", searchResult.Count.ToString());

                //searchPanel.ResultLabel.Tag += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

                searchPanel.ResultLabel.Text = searchPanel.ResultLabel.Tag.ToString();

                searchResult.Sort((x, y) => (y.Id - x.Id));

                containerPanel.Controls.Add(searchPanel);
                searchPanel.BringToFront();

                foreach (INVR nvr in searchResult)
                {
                    NVRPanel nvrPanel = GetDevicePanel();
                    nvrPanel.Initial();
                    nvrPanel.NVR = nvr;

                    var exist = false;
                    foreach (KeyValuePair<ushort, INVR> existNVR in CMS.NVRManager.NVRs)
                    {
                        if (existNVR.Value.ReadyState != ReadyState.Ready) continue;
                        if (existNVR.Value.Credential.Domain == nvr.Credential.Domain)
                        {
                            exist = true;
                            break;
                        }
                    }
                    nvrPanel.EditVisible = false;
                    nvrPanel.Exist = exist;
                    searchPanel.Controls.Add(nvrPanel);
                    //else if device is IPos or something else
                }

                var deviceTitlePanel = GetDevicePanel();
                deviceTitlePanel.Initial();
                deviceTitlePanel.IsTitle = true;
                deviceTitlePanel.EditVisible = false;
                deviceTitlePanel.Cursor = Cursors.Default;
                deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
                deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
                searchPanel.Controls.Add(deviceTitlePanel);
            }

            if (Manufacturers.Count > 0)
            {
                _watch.Reset();
                _watch.Start();
                _searchingManufacture = Manufacturers.Dequeue();
                SearchNVRDelegate searchDeviceDelegate = CMS.NVRManager.SearchNVR;
                searchDeviceDelegate.BeginInvoke(_searchingManufacture, SearchNVRCallback, searchDeviceDelegate);
                return;
            }

            //group no-result labels
            foreach (var noResultLabel in _noResultLabels)
            {
                noResultLabel.BringToFront();
            }
            _noResultLabels.Clear();
            SearchCompleted();
        }
        private void SearchCompleted()
        {
            _isEditing = true;
            _isSearching = false;
            if (OnSearchComplete != null)
                OnSearchComplete(this, null);

            ApplicationForms.HideLoadingIcon();
            containerPanel.AutoScroll = false;
            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            var titleControl = (sender as Control);
            if (titleControl == null) return;

            //containerPanel.AutoScroll = false;
            var position = containerPanel.AutoScrollPosition;
            position.Y *= -1;
            containerPanel.Enabled = false;
            var controls = new List<NVRPanel>();

            foreach (NVRPanel control in titleControl.Parent.Controls)
            {
                controls.Add(control);
            }
            controls.Reverse();

            foreach (NVRPanel control in controls)
            {

                if (control.NVR != null)
                {
                    control.NVR.ReadyState = ReadyState.New;
                    CMS.NVRModify(control.NVR);
                }
                control.Checked = true;
                if (!control.Checked)
                    break;
            }
            containerPanel.AutoScrollPosition = position;
            containerPanel.Enabled = true;
            //containerPanel.AutoScroll = true;

        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            var titleControl = (sender as Control);
            if (titleControl == null) return;

            //containerPanel.AutoScroll = false;
            var position = containerPanel.AutoScrollPosition;
            position.Y *= -1;
            containerPanel.Enabled = false;
            foreach (NVRPanel control in titleControl.Parent.Controls)
            {
                control.Checked = false;
                if (control.NVR != null)
                {
                    control.NVR.ReadyState = ReadyState.NotInUse;
                    CMS.NVRModify(control.NVR);
                }
            }
            containerPanel.AutoScrollPosition = position;
            containerPanel.Enabled = true;
            //containerPanel.AutoScroll = true;
        }

        private Boolean _isEditing;
        private void NVRPanelOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;
            if (!(sender is NVRPanel)) return;

            var panel = sender as NVRPanel;
            if (panel.NVR == null) return;

            var resultPanel = panel.Parent as DeviceResultPanel;
            if (resultPanel == null) return;

            UInt16 count = 0;
            foreach (NVRPanel control in resultPanel.Controls)
            {
                if (control.IsTitle || control.NVR == null) continue;
                control.NVR.ReadyState = control.Checked ? ReadyState.New : ReadyState.JustAdd;
                if (control.Checked)
                    count++;
            }

            if (count == 0)
                resultPanel.ResultLabel.Text = resultPanel.ResultLabel.Tag.ToString();
            else
                resultPanel.ResultLabel.Text = resultPanel.ResultLabel.Tag + @", " + ((count == 1)
                                                        ? Localization["SetupNVR_NVRSelected"]
                                                        : Localization["SetupNVR_NVRsSelected"]).Replace("%1", count.ToString());

            //Add Device
            var selectAll = false;
            if (panel.Checked)
            {
                panel.NVR.Id = CMS.NVRManager.GetNewNVRId();

                if (panel.NVR.Id == 0)
                {
                    panel.Checked = false;
                    panel.NVR.ReadyState = ReadyState.NotInUse;
                    TopMostMessageBox.Show(Localization["SetupNVR_MaximumAmount"].Replace("%1", CMS.NVRManager.MaximunNVRAmount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    AddNVR(panel.NVR);
                    selectAll = true;
                    foreach (NVRPanel control in resultPanel.Controls)
                    {
                        if (control.IsTitle) continue;
                        if (!control.Checked && control.Enabled)
                        {
                            selectAll = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                RemoveNVR(panel.NVR as INVR);
            }

            var title = resultPanel.Controls[resultPanel.Controls.Count - 1] as NVRPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }
        }

        private void AddNVR(INVR nvr)
        {
            if (nvr == null) return;

            nvr.ReadyState = ReadyState.New;
            CMS.NVRManager.NVRs.Add(nvr.Id, nvr);
            CMS.NVRModify(nvr);

            if (OnNVRModify != null)
                OnNVRModify(this, null);
        }

        private void RemoveNVR(INVR nvr)
        {
            if (nvr == null) return;

            nvr.ReadyState = ReadyState.NotInUse;

            UInt16 key = 0;

            foreach (KeyValuePair<UInt16, INVR> obj in CMS.NVRManager.NVRs)
            {
                //if (!(obj.Value is ICamera)) continue;

                if (obj.Value != nvr) continue;

                key = obj.Key;
                CMS.NVRManager.NVRs.Remove(obj.Key);
                CMS.NVRModify(obj.Value);
                break;
            }

            Boolean changeOrder = true;

            while (changeOrder && key > 0)
            {
                changeOrder = false;
                IDevice changeDevice = null;
                UInt16 changeKey = key;
                foreach (KeyValuePair<UInt16, IDevice> obj in Server.Device.Devices)
                {
                    if (obj.Value.ReadyState != ReadyState.New) continue;
                    if (obj.Key > changeKey)
                    {
                        changeDevice = obj.Value;
                        break;
                    }
                }

                if (changeDevice == null) continue;

                Server.Device.Devices.Add(changeKey, changeDevice);

                Server.Device.Devices.Remove(changeDevice.Id);

                key = changeDevice.Id;
                changeDevice.Id = changeKey;
                changeOrder = true;
            }

            if (OnNVRModify != null)
                OnNVRModify(this, null);
        }

        public void ClearViewModel()
        {
            _isEditing = false;
            if (containerPanel.Controls.Count == 0) return;

            foreach (Control control in containerPanel.Controls)
            {
                if (control is Label)
                {
                    var label = control as Label;
                    if (!_recycleLabels.Contains(label))
                        _recycleLabels.Enqueue(label);
                    continue;
                }

                if (control is DeviceResultPanel)
                {
                    var panel = control as DeviceResultPanel;

                    foreach (NVRPanel devicePanel in panel.Controls)
                    {
                        devicePanel.OnSelectChange -= NVRPanelOnSelectChange;
                        devicePanel.SelectionVisible = true;
                        devicePanel.EditVisible = false;
                        devicePanel.Checked = false;
                        devicePanel.NVR = null;
                        devicePanel.OnSelectChange += NVRPanelOnSelectChange;

                        if (devicePanel.IsTitle)
                        {
                            devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
                            devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
                            devicePanel.IsTitle = false;
                        }

                        if (!_recycleDevice.Contains(devicePanel))
                            _recycleDevice.Enqueue(devicePanel);
                    }
                    panel.Controls.Clear();

                    if (!_recyclePanels.Contains(panel))
                        _recyclePanels.Enqueue(panel);
                }
            }

            containerPanel.Controls.Clear();
            //foreach (NVRPanel nvrPanel in containerPanel.Controls)
            //{
            //    nvrPanel.SelectionVisible = false;

            //    nvrPanel.Checked = false;
            //    nvrPanel.NVR = null;
            //    nvrPanel.Cursor = Cursors.Hand;
            //    nvrPanel.EditVisible = true;

            //    if (nvrPanel.IsTitle)
            //    {
            //        nvrPanel.OnSelectAll -= DevicePanelOnSelectAll;
            //        nvrPanel.OnSelectNone -= DevicePanelOnSelectNone;
            //        nvrPanel.IsTitle = false;
            //    }

            //    if (!_recycleDevice.Contains(nvrPanel))
            //    {
            //        _recycleDevice.Enqueue(nvrPanel);
            //    }
            //}
            //containerPanel.Controls.Clear();
        }
    }
}
