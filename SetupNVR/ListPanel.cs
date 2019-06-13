using System.Linq;
using Constant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Manager = SetupBase.Manager;

namespace SetupNVR
{
    public partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<INVR>> OnNVREdit;
        public event EventHandler OnNVRAdd;
        public event EventHandler OnNVRSearch;

        public IApp App;
        public IServer Server;
        public IVAS VAS;
        public IFOS FOS;
        public ICMS CMS;
        public IPTS PTS;
        public INVRManager NVRManager;

        public Dictionary<String, String> Localization;
        private bool _autoSearchDisabled;
        private String[] _manufacture;


        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   {"SetupDevice_AllManufacturer", "Manufacturers"},
                                   {"SetupNVR_SearchNVR", "Search NVR"},
                                   {"SetupNVR_AddedNVR", "Added NVR"},
                                   {"SetupNVR_AddNewNVR", "Add new NVR..."},
                                   {"SetupNVR_NVRStatusNone", "None: No status."},
                                   {"SetupNVR_NVRStatusGood", "Good: No response status percentage between 0% to 35%."},
                                   {"SetupNVR_NVRStatusNormal", "Normal: No response status percentage between 36% to 70%."},
                                   {"SetupNVR_NVRStatusWarning", "Warning: No response status percentage between 71% to 99%."},
                                   {"SetupNVR_NVRStatusFailures", "Failures: Server failures. No response status percentage is 100%."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedNVRLabel.Text = Localization["SetupNVR_AddedNVR"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }


        public bool AutoSearchDisabled
        {
            get { return _autoSearchDisabled; }
            set
            {
                _autoSearchDisabled = value;

                var visible = !_autoSearchDisabled;
                labelSearch.Visible = visible;
                searchDoubleBufferPanel.Visible = visible;
            }
        }

        public virtual String SelectedManufacturer
        {
            get
            {
                if (manufactureComboBox.SelectedIndex == 0)
                    return "ALL";

                var brand = manufactureComboBox.SelectedItem.ToString();

                return brand;
            }
        }

        protected virtual string[] CreateManufatureList()
        {
            if (CMS != null)
            {
                return new[] { "iSAP", "Diviotec", "Hikvision", "VioStor", "DIGIEVER", "SAMSUNG", "Digifort" };//, "Digifort", "VIVOTEK", "ACTi", "Milestone", "Salient"
            }

            return new string[] { };
        }

        public void Initialize()
        {
            searchDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            searchDoubleBufferPanel.MouseClick += SearchDoubleBufferPanelMouseClick;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
            searchDoubleBufferPanel.Visible = false;

            var list = CreateManufatureList();
            Array.Sort(list);
            _manufacture = list;
            foreach (var brand in _manufacture)
            {
                manufactureComboBox.Items.Add(brand);
            }

            if (manufactureComboBox.Items.Count > 1)
            {
                manufactureComboBox.Items.Insert(0, Localization["SetupDevice_AllManufacturer"]);
                Manager.DropDownWidth(manufactureComboBox);
                manufactureComboBox.SelectedIndex = 0;
                manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
                manufactureComboBox.Visible = true;
            }
            else if (manufactureComboBox.Items.Count == 1)
            {
                manufactureComboBox.SelectedIndex = 0;
                manufactureComboBox.Visible = false;
            }

            if (FOS != null)
            {
                flagGrayLabel.Text = @"        " + Localization["SetupNVR_NVRStatusNone"];
                flagGreenLabel.Text = @"        " + Localization["SetupNVR_NVRStatusGood"];
                flagYellowLabel.Text = @"        " + Localization["SetupNVR_NVRStatusNormal"];
                flagRedLabel.Text = @"        " + Localization["SetupNVR_NVRStatusWarning"];
                flagCloseLabel.Text = @"        " + Localization["SetupNVR_NVRStatusFailures"];

                flagGrayLabel.Image = Resources.GetResources(Properties.Resources.flag_gray, Properties.Resources.IMGFlagGray);
                flagGreenLabel.Image = Resources.GetResources(Properties.Resources.flag_green, Properties.Resources.IMGFlagGreen);
                flagYellowLabel.Image = Resources.GetResources(Properties.Resources.flag_yellow, Properties.Resources.IMGFlagYellow);
                flagRedLabel.Image = Resources.GetResources(Properties.Resources.flag_red, Properties.Resources.IMGFlagRed);
                flagCloseLabel.Image = Resources.GetResources(Properties.Resources.flag_close, Properties.Resources.IMGFlagClose);
            }
        }

        private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (OnNVRSearch != null)
                OnNVRSearch(this, e);
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var panel = sender as DoubleBufferPanel;
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, searchDoubleBufferPanel);
            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            if (panel != null)
            {
                if (Localization.ContainsKey("SetupNVR_" + panel.Tag))
                    Manager.PaintText(g, Localization["SetupNVR_" + panel.Tag]);
            }

        }

        private readonly Queue<NVRPanel> _recycleNVR = new Queue<NVRPanel>();
        private Point _previousScrollPosition = new Point();
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;

            List<INVR> sortResult = null;
            if (CMS != null)
                sortResult = new List<INVR>(CMS.NVRManager.NVRs.Values);
            else if (FOS != null)
            {
                sortResult = new List<INVR>(FOS.NVR.NVRs.Values);
                flagGrayLabel.Visible = flagGreenLabel.Visible = flagYellowLabel.Visible = flagRedLabel.Visible = flagCloseLabel.Visible = (sortResult.Count > 0);
            }
            else if (PTS != null)
            {
                sortResult = new List<INVR>(PTS.NVR.NVRs.Values);
                //only allow 1 nv to be added
                //if (sortResult.Count > 0)
                //    addNewDoubleBufferPanel.Visible = false;
            }
            else if (VAS != null)
                sortResult = new List<INVR>(VAS.NVR.NVRs.Values);

            if (sortResult == null)
            {
                addedNVRLabel.Visible = false;
                return;
            }

            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedNVRLabel.Visible = false;
                return;
            }

            addedNVRLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (INVR nvr in sortResult)
            {
                NVRPanel nvrPanel = GetNVRPanel();

                nvrPanel.NVR = nvr;

                containerPanel.Controls.Add(nvrPanel);
            }

            NVRPanel nvrTitlePanel = GetNVRPanel();
            nvrTitlePanel.IsTitle = true;
            nvrTitlePanel.Cursor = Cursors.Default;
            nvrTitlePanel.EditVisible = false;
            //deviceTitleControl.OnSortChange += DeviceControlOnSortChange;
            nvrTitlePanel.OnSelectAll += NVRPanelOnSelectAll;
            nvrTitlePanel.OnSelectNone += NVRPanelOnSelectNone;
            containerPanel.Controls.Add(nvrTitlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        protected virtual NVRPanel CreateNvrPanel()
        {
            var nvrPanel = new NVRPanel
            {
                App = App,
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };
            return nvrPanel;
        }

        private NVRPanel GetNVRPanel()
        {
            if (_recycleNVR.Count > 0)
            {
                return _recycleNVR.Dequeue();
            }

            var nvrPanel = CreateNvrPanel();

            if (CMS != null)
                nvrPanel.Server = CMS;
            else if (FOS != null)
                nvrPanel.Server = FOS;
            else if (PTS != null)
                nvrPanel.Server = PTS;
            else if (VAS != null)
                nvrPanel.Server = VAS;

            nvrPanel.OnNVREditClick += NVRControlOnNVREditClick;
            nvrPanel.OnSelectChange += NVRControlOnSelectChange;

            return nvrPanel;
        }

        public Brush SelectedColor
        {
            set
            {
                foreach (NVRPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

        public void ShowCheckBox()
        {
            addNewDoubleBufferPanel.Visible = addedNVRLabel.Visible = false;

            foreach (NVRPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedNVRs()
        {
            var nvrs = new List<String>();

            foreach (var control in SelectedControls)
            {
                if (CMS != null)
                {
                    foreach (KeyValuePair<UInt16, IDevice> device in control.NVR.Device.Devices)
                    {
                        CMS.NVRManager.RemoveDeviceChannelTable(device.Value);
                    }
                    CMS.NVRManager.NVRs.Remove(control.NVR.Id);
                }
                else if (FOS != null)
                    FOS.NVR.NVRs.Remove(control.NVR.Id);
                else if (PTS != null)
                    PTS.NVR.NVRs.Remove(control.NVR.Id);
                else if (VAS != null)
                    VAS.NVR.NVRs.Remove(control.NVR.Id);

                nvrs.Add(control.NVR.Id.ToString());
                control.NVR.ReadyState = ReadyState.Delete;

                if (CMS != null)
                    CMS.NVRModify(control.NVR);
                else if (PTS != null)
                    PTS.NVRModify(control.NVR);
                else if (VAS != null)
                    VAS.NVRModify(control.NVR);
            }

            Server.WriteOperationLog("Remove NVR %1".Replace("%1", String.Join(",", nvrs.ToArray())));
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnNVRAdd != null)
                OnNVRAdd(this, e);
        }

        private void SearchDoubleBufferPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (OnNVRSearch != null)
                OnNVRSearch(this, e);
        }

        private void NVRControlOnNVREditClick(Object sender, EventArgs e)
        {
            if (((NVRPanel)sender).NVR != null)
            {
                if (OnNVREdit != null)
                    OnNVREdit(this, new EventArgs<INVR>(((NVRPanel)sender).NVR));
            }
        }

        private void NVRControlOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as NVRPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (NVRPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as NVRPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= NVRPanelOnSelectAll;
                title.OnSelectNone -= NVRPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += NVRPanelOnSelectAll;
                title.OnSelectNone += NVRPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (NVRPanel nvrPanel in containerPanel.Controls)
            {
                nvrPanel.SelectionVisible = false;

                nvrPanel.Checked = false;
                nvrPanel.NVR = null;
                nvrPanel.Cursor = Cursors.Hand;
                nvrPanel.EditVisible = true;

                if (nvrPanel.IsTitle)
                {
                    nvrPanel.OnSelectAll -= NVRPanelOnSelectAll;
                    nvrPanel.OnSelectNone -= NVRPanelOnSelectNone;
                    nvrPanel.IsTitle = false;
                }

                if (!_recycleNVR.Contains(nvrPanel))
                {
                    _recycleNVR.Enqueue(nvrPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void NVRPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (NVRPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void NVRPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (NVRPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public IEnumerable<NVRPanel> SelectedControls
        {
            get { return containerPanel.Controls.OfType<NVRPanel>().Where(c => c.Checked && c.NVR != null); }
        }
    }
}