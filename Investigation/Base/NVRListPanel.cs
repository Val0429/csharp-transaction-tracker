using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace Investigation.Base
{
    public sealed partial class NVRListPanel : UserControl
    {
        public IApp App;
        public IServer Server;
        public ICMS CMS;
        public CameraEventSearchCriteria SearchCriteria;
        public INVRManager NVRManager;
        public event EventHandler<EventArgs<INVR>> OnNVRClick;

        public Dictionary<String, String> Localization;
        public NVRListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupNVR_AddedNVR", "Added NVR"}
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedNVRLabel.Text = Localization["SetupNVR_AddedNVR"];
            Name = "BandwidthControlSetting";
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            CMS = Server as ICMS;
        }

        private void UpfateAllNVREnable()
        {
            foreach (NVRPanel control in containerPanel.Controls)
            {
                if (control.NVR == null || control.IsTitle) continue;
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
                if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
                NVRPanel nvrPanel = GetNVRPanel();

                nvrPanel.NVR = nvr;
                nvrPanel.IsTitle = false;
                nvrPanel.SearchCriteria = SearchCriteria;
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

            //EnabledCheckBoxClick(this, null);
            UpfateAllNVREnable();
        }

        private NVRPanel GetNVRPanel()
        {
            if (_recycleNVR.Count > 0)
            {
                return _recycleNVR.Dequeue();
            }

            var nvrPanel = new NVRPanel
            {
                App = App
            };

            if (CMS != null)
                nvrPanel.Server = CMS;

            nvrPanel.OnNVRClick += NVRPanelOnNVRClick;

            return nvrPanel;
        }

        private void NVRPanelOnNVRClick(object sender, EventArgs e)
        {
            if (((NVRPanel)sender).NVR == null) return;

            if (OnNVRClick != null)
                OnNVRClick(this, new EventArgs<INVR>(((NVRPanel)sender).NVR));
        }

        private void ClearViewModel()
        {
            foreach (NVRPanel nvrPanel in containerPanel.Controls)
            {
                nvrPanel.NVR = null;

                if (nvrPanel.IsTitle)
                {
                    nvrPanel.IsTitle = false;
                    nvrPanel.NVR = null;
                    nvrPanel.SearchCriteria = null;
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
        public Dictionary<String, String> Localization;
        public event EventHandler OnNVRClick;
        public CameraEventSearchCriteria SearchCriteria;
        public Boolean IsTitle;
        public IApp App;
        public IServer Server;
        public INVR NVR;
        public Boolean Exist;
        public NVRPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"NVR_ID", "ID"},
                                   {"NVR_Name", "Name"},
                                   {"NVR_Domain", "Domain"},
                                   {"NVR_Port", "Port"},
                                   {"NVR_DeviceQuantity", "Devices Quantity"},
                                   {"NVR_Manufacture", "Manufacture"}
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            Paint += NVRPanelPaint;
            MouseClick += NVRPanelMouseClick;
        }

        private void NVRPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (IsTitle) return;
            if (OnNVRClick != null)
                OnNVRClick(this, e);
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

            if (Width <= 200) return;
            Manager.PaintTitleText(g, Localization["NVR_ID"]);

            g.DrawString(Localization["NVR_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

            if (Width <= 320) return;
            g.DrawString(Localization["NVR_Domain"], Manager.Font, Manager.TitleTextColor, 200, 13);

            if (Server is ICMS)
            {
                if (Width <= 400) return;
                g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 520) return;
                g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 430, 13);

                if (Width <= 620) return;
                g.DrawString(Localization["NVR_Manufacture"], Manager.Font, Manager.TitleTextColor, 550, 13);

            }
        }

        private void NVRPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
            
                return;
            }

            Manager.Paint(g, (Control)sender);
            Manager.PaintEdit(g, this);

            Brush fontBrush = Brushes.Black;
            if (NVR.FailoverSetting != null && NVR.FailoverSetting.ActiveProfile)
            {
                Manager.PaintSelected(g);
                fontBrush = Manager.SelectedTextColor;
            }

            Manager.PaintStatus(g, NVR.ReadyState);

            if (Width <= 200) return;

            Manager.PaintText(g, NVR.Id.ToString().PadLeft(2, '0'), fontBrush);

            g.DrawString(NVR.Name, Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 320) return;
            g.DrawString(String.Format("{0} {1}", NVR.Credential.Domain, Exist ? Localization["NVR_InUse"] : String.Empty), Manager.Font, fontBrush, _addressRectangleF);

            if (Server is ICMS)
            {
                if (Width <= 400) return;
                g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 330, 13);

                if (Width <= 520) return;
                g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 430, 13);

                if (Width <= 620) return;
                g.DrawString(NVR.Manufacture, Manager.Font, fontBrush, 555, 13);

                if (Width <= 720) return;
                var list = new List<String>();
                var deviceSelecton = new List<String>();
                foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
                {
                    if(NVR.Id != nvrDevice.NVRId) continue;
                    var device = NVR.Device.FindDeviceById(nvrDevice.DeviceId);
                    if (device != null)
                        list.Add(String.Format("{0}", device));
                }

                foreach (var device in list)
                {
                    if (deviceSelecton.Count >= 3)
                    {
                        deviceSelecton[2] += " ...";
                        break;
                    }

                    if (String.IsNullOrEmpty(device)) continue;

                    deviceSelecton.Add(device);
                }

                Manager.PaintTextRight(g, this, String.Join(", ", deviceSelecton.ToArray()));
               // g.DrawString(String.Join(", ", deviceSelecton.ToArray()), Manager.Font, fontBrush, 600, 13);
            }
        }

    }
}