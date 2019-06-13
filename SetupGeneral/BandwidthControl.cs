using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public sealed partial class BandwidthControl : UserControl
    {
        public IApp App;
        public IServer Server;
        public ICMS CMS;
        public INVRManager NVRManager;
        private Boolean _customStreamSettinfEnable;
        private Bitrate _customStreamSettingBitrate;
        public Dictionary<String, String> Localization;

        public BandwidthControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Enabled", "Enabled"},
                                   {"SetupNVR_AddedNVR", "Added NVR"},
                                   {"SetupGeneral_EnableBandwidthControl","Bandwidth Control"}
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedNVRLabel.Text = Localization["SetupNVR_AddedNVR"];
            Name = "BandwidthControlSetting";
            BackgroundImage = Manager.BackgroundNoBorder;

            enabledCheckBox.Text = Localization["SetupGeneral_Enabled"];
            enabledCheckBox.Click += EnabledCheckBoxClick;
        }

        public void Initialize()
        {
            bandwidthPanel.Paint += InputPanelPaint;
            CMS = Server as ICMS;
            if (Server is INVR)
                addedNVRLabel.Text = String.Empty;

            _customStreamSettinfEnable = Server.Configure.CustomStreamSetting.Enable;
            _customStreamSettingBitrate = Server.Configure.CustomStreamSetting.Bitrate;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("SetupGeneral_" + control.Tag))
                Manager.PaintText(g, Localization["SetupGeneral_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void EnabledCheckBoxClick(object sender, EventArgs e)
        {
            Server.Configure.EnableBandwidthControl = enabledCheckBox.Checked;

            foreach (NVRPanel control in containerPanel.Controls)
            {
                if (control.NVR == null || control.IsTitle) continue;

                control.InUse = Server.Configure.EnableBandwidthControl;
                control.NVR.ReadyState = ReadyState.Modify;

                if(!Server.Configure.EnableBandwidthControl)
                {
                    foreach (KeyValuePair<ushort, IDevice> device in control.NVR.Device.Devices)
                    {
                        var camera = device.Value as ICamera;
                        if(camera == null) continue;
                        camera.Profile.StreamId = 1;
                    }
                }
            }

            if(!Server.Configure.EnableBandwidthControl)
            {
                Server.Configure.CustomStreamSetting.Enable = false;
                Server.Configure.CustomStreamSetting.Bitrate = Bitrate.NA;
            }
        }

        private void UpfateAllNVREnable()
        {
            foreach (NVRPanel control in containerPanel.Controls)
            {
                if (control.NVR == null || control.IsTitle) continue;

                control.InUse = Server.Configure.EnableBandwidthControl;
            }
        }

        private readonly Queue<NVRPanel> _recycleNVR = new Queue<NVRPanel>();
        private Point _previousScrollPosition = new Point(); 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();

            List<INVR> sortResult = null;
            if (CMS != null)
                sortResult = new List<INVR>(CMS.NVRManager.NVRs.Values);
            else if(Server is INVR)
            {
                sortResult = new List<INVR> {(INVR) Server};
            }

            if (sortResult == null)
            {
                addedNVRLabel.Visible = false;
                enabledCheckBox.Checked = Server.Configure.EnableBandwidthControl;
                return;
            }

            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedNVRLabel.Visible = false;
                enabledCheckBox.Checked = Server.Configure.EnableBandwidthControl;
                return;
            }

            addedNVRLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (INVR nvr in sortResult)
            {
                if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
                NVRPanel nvrPanel = GetNVRPanel();

                nvrPanel.NVR = nvr;
                nvrPanel.InUse = false;
                nvrPanel.Bandwidth = nvr.Configure.BandwidthControlBitrate;
                nvrPanel.Steaming = nvr.Configure.BandwidthControlStream;
                nvrPanel.IsTitle = false;
                containerPanel.Controls.Add(nvrPanel);
            }

            NVRPanel nvrTitlePanel = GetNVRPanel();
            nvrTitlePanel.IsTitle = true;
            nvrTitlePanel.Cursor = Cursors.Default;
            containerPanel.Controls.Add(nvrTitlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
            containerPanel.AutoScrollPosition = _previousScrollPosition;

            enabledCheckBox.Checked = Server.Configure.EnableBandwidthControl;
            //EnabledCheckBoxClick(this, null);
            UpfateAllNVREnable();
           
            enabledCheckBox.BringToFront();
            enabledCheckBox.Focus();
        }

        private NVRPanel GetNVRPanel()
        {
            if (_recycleNVR.Count > 0)
            {
                return _recycleNVR.Dequeue();
            }

            var nvrPanel = new NVRPanel
            {
                App = App,
                Server = CMS ?? Server
            };

            return nvrPanel;
        }

        private void ClearViewModel()
        {
            foreach (NVRPanel nvrPanel in containerPanel.Controls)
            {
                nvrPanel.NVR = null;

                nvrPanel.InUse = false;

                if (nvrPanel.IsTitle)
                {
                    nvrPanel.IsTitle = false;
                }

                if (!_recycleNVR.Contains(nvrPanel))
                {
                    _recycleNVR.Enqueue(nvrPanel);
                }
            }
            containerPanel.Controls.Clear();
        }
    }

    public sealed class NVRPanel : Panel
    {
        private readonly ComboBox _bandwidthComboBox;
        private readonly ComboBox _streamingComboBox;
        public Dictionary<String, String> Localization;

        private Boolean _inUse;
        public Boolean InUse
        {
            get
            {
                return _inUse;
            }
            set
            {
                _streamingComboBox.Enabled = _bandwidthComboBox.Enabled = _inUse = value;
            }
        }

        public UInt16 Steaming
        {
            get { return (ushort) (_streamingComboBox.SelectedItem); }
            set
            {
                _streamingComboBox.SelectedIndexChanged -= StreamingComboBoxSelectedIndexChanged;
                _streamingComboBox.SelectedIndex = value -1;
                _streamingComboBox.SelectedIndexChanged += StreamingComboBoxSelectedIndexChanged;
            }
        }

        public Bitrate Bandwidth
        {
            get { return _bandwidthComboBox.SelectedItem is Bitrate ? (Bitrate)_bandwidthComboBox.SelectedItem : Bitrate.Bitrate1M; }
            set
            {
                _bandwidthComboBox.SelectedIndexChanged -= BandwidthComboBoxChanged;
                if(value == Bitrate.NA)
                {
                    _bandwidthComboBox.SelectedItem = Localization["NVR_OriginalStreaming"];
                }
                else
                {
                    _bandwidthComboBox.SelectedItem = value;
                }
                _bandwidthComboBox.SelectedIndexChanged += BandwidthComboBoxChanged;
            }
        }

        public Boolean IsTitle;
        public IApp App;
        public IServer Server;
        public INVR NVR;
        public Boolean Exist;
        public NVRPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},
                                   {"Common_Min", "Min"},

                                   {"NVR_ID", "ID"},
                                   {"NVR_Name", "Name"},
                                   {"NVR_Domain", "Domain"},
                                   {"NVR_Port", "Port"},
                                   {"NVR_DeviceQuantity", "Devices Quantity"},
                                   {"NVR_Manufacture", "Manufacture"},
                                   {"NVR_InUse", "(In use)"},
                                   {"NVR_Bandwidth", "Bandwidth"},
                                   {"NVR_Stream", "Stream"},
                                   {"NVR_OriginalStreaming", "Original Streaming"}
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _bandwidthComboBox = new ComboBox
            {
                Width = 145,
                Dock = DockStyle.None,
                Location = new Point(700, 10),
                Visible = true,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                MaxLength = 4,
                Enabled = true,
                ImeMode = ImeMode.Disable
            };
            _bandwidthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _bandwidthComboBox.Items.Add(Localization["NVR_OriginalStreaming"]);
            _bandwidthComboBox.Items.Add(Bitrate.Bitrate1M);
            _bandwidthComboBox.Items.Add(Bitrate.Bitrate512K);
            _bandwidthComboBox.Items.Add(Bitrate.Bitrate256K);
            _bandwidthComboBox.Items.Add(Bitrate.Bitrate56K);
            _bandwidthComboBox.SelectedIndex = 0;
            _bandwidthComboBox.SelectedIndexChanged += BandwidthComboBoxChanged;
            Controls.Add(_bandwidthComboBox);

            _streamingComboBox = new ComboBox
            {
                Width = 145,
                Dock = DockStyle.None,
                Location = new Point(900, 10),
                Visible = true,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                MaxLength = 4,
                Enabled = true,
                ImeMode = ImeMode.Disable
            };
            _streamingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _streamingComboBox.Items.Add(1);
            _streamingComboBox.Items.Add(2);
            _streamingComboBox.SelectedIndex = 0;
            _streamingComboBox.SelectedIndexChanged += StreamingComboBoxSelectedIndexChanged;
            Controls.Add(_streamingComboBox);

            Paint += NVRPanelPaint;
        }

        private void StreamingComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            NVR.ReadyState = ReadyState.Modify;
            NVR.Configure.BandwidthControlStream = Convert.ToUInt16(_streamingComboBox.SelectedItem.ToString());
            NVR.Configure.CustomStreamSetting.StreamId = NVR.Configure.BandwidthControlStream;
            Invalidate();
        }

        private void BandwidthComboBoxChanged(Object sender, EventArgs e)
        {
            NVR.ReadyState = ReadyState.Modify;
            NVR.Configure.BandwidthControlBitrate = _bandwidthComboBox.SelectedItem is Bitrate ? (Bitrate)_bandwidthComboBox.SelectedItem : Bitrate.NA;
            Invalidate();
        }

        private static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);
        private static RectangleF _addressRectangleF = new RectangleF(200, 13, 140, 17);

        public void Initial()
        {
            Paint -= NVRPanelPaint;
            Paint += NVRPanelPaint;
        }

        private void PaintTitle(Graphics g)
        {
            if(Server is NVR)
            {
                if (Width <= 200) return;
                g.DrawString(Localization["NVR_Bandwidth"], Manager.Font, Manager.TitleTextColor, 50, 13);

                if (Width <= 320) return;
                g.DrawString(Localization["NVR_Stream"], Manager.Font, Manager.TitleTextColor, 250, 13);
                return;
            }

            if (Width <= 200) return;
            Manager.PaintTitleText(g, Localization["NVR_ID"]);

            g.DrawString(Localization["NVR_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

            if (Width <= 320) return;
            g.DrawString(Localization["NVR_Domain"], Manager.Font, Manager.TitleTextColor, 200, 13);

            if (Width <= 400) return;
            g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 330, 13);

            if (Width <= 520) return;
            g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 430, 13);

            if (Width <= 620) return;
            g.DrawString(Localization["NVR_Manufacture"], Manager.Font, Manager.TitleTextColor, 550, 13);

            if (Width <= 720) return;
            g.DrawString(Localization["NVR_Bandwidth"], Manager.Font, Manager.TitleTextColor, 700, 13);

            if (Width <= 920) return;
            g.DrawString(Localization["NVR_Stream"], Manager.Font, Manager.TitleTextColor, 900, 13);
        }

        private void NVRPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                _bandwidthComboBox.Visible =
                _streamingComboBox.Visible = false;
                return;
            }
            _bandwidthComboBox.Visible =
            _streamingComboBox.Visible = true;
            Manager.Paint(g, (Control)sender);

            Brush fontBrush = Brushes.Black;
            if (NVR.FailoverSetting != null && NVR.FailoverSetting.ActiveProfile)
            {
                Manager.PaintSelected(g);
                fontBrush = Manager.SelectedTextColor;
            }

            Manager.PaintStatus(g, NVR.ReadyState);

            if (Width <= 200) return;

            if(Server is NVR)
            {
                _bandwidthComboBox.Location = new Point(50, 10);
                _streamingComboBox.Location = new Point(250, 10);
                return;
            }

            Manager.PaintText(g, NVR.Id.ToString().PadLeft(2, '0'), fontBrush);

            g.DrawString(NVR.Name, Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 320) return;
            g.DrawString(String.Format("{0} {1}", NVR.Credential.Domain, Exist ? Localization["NVR_InUse"] : String.Empty), Manager.Font, fontBrush, _addressRectangleF);

            if (Width <= 400) return;
            g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 330, 13);

            if (Width <= 520) return;
            g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 430, 13);

            if (Width <= 620) return;
            g.DrawString(NVR.Manufacture, Manager.Font, fontBrush, 555, 13);
        }

    }
}