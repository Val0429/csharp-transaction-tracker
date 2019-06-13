using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupNVR
{
    public class NVRPanel : Panel
    {
        public event EventHandler OnNVREditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        protected readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle { get; set; }
        public IApp App { get; set; }
        public IServer Server { get; set; }
        public INVR NVR { get; set; }
        public String DataType = "Information";
        public Boolean Exist;
        private static readonly Image FlagGray = Resources.GetResources(Properties.Resources.flag_gray, Properties.Resources.IMGFlagGray);
        private static readonly Image FlagGreen = Resources.GetResources(Properties.Resources.flag_green, Properties.Resources.IMGFlagGreen);
        private static readonly Image FlagYellow = Resources.GetResources(Properties.Resources.flag_yellow, Properties.Resources.IMGFlagYellow);
        private static readonly Image FlagRed = Resources.GetResources(Properties.Resources.flag_red, Properties.Resources.IMGFlagRed);
        private static readonly Image FlagClose = Resources.GetResources(Properties.Resources.flag_close, Properties.Resources.IMGFlagClose);
        //private static readonly Image Selected = Resources.GetResources(Properties.Resources.selected, Properties.Resources.IMGSelected);
        
        
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
                                   {"NVR_Event", "Event"},
                                   {"NVR_Patrol", "Patrol"},
                                   {"NVR_LaunchTime", "Failover Launch Time"},
                                   {"NVR_BlockSize", "Block Size"},
                                   {"NVR_Modified", "Modified"},
                                   {"NVR_Status", "Status"},
                                   {"NVR_InUse", "(In use)"},
                                   {"NVR_Manufacture", "Manufacture"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += NVRPanelMouseClick;
            Paint += NVRPanelPaint;
        }

        protected static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);
        protected static RectangleF _addressRectangleF = new RectangleF(200, 13, 140, 17);

        public void Initial()
        {
            Paint -= NVRPanelPaint;
            Paint += NVRPanelPaint;
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;
            if (DataType == "Information")
                Manager.PaintTitleText(g, Localization["NVR_ID"]);

            g.DrawString(Localization["NVR_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

            if (Width <= 320) return;
            g.DrawString(Localization["NVR_Domain"], Manager.Font, Manager.TitleTextColor, 200, 13);

            OnServerTitlePaint(g);
        }

        protected virtual void OnServerTitlePaint(Graphics g)
        {
            if (Server is ICMS)
            {
                if (Width <= 400) return;
                g.DrawString(Localization["NVR_Event"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 520) return;
                g.DrawString(Localization["NVR_Patrol"], Manager.Font, Manager.TitleTextColor, 430, 13);

                if (Width <= 620) return;
                g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 550, 13);

                if (Width <= 720) return;
                g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 650, 13);

                if (Width <= 890) return;
                g.DrawString(Localization["NVR_Modified"], Manager.Font, Manager.TitleTextColor, 790, 13);

                if (Width <= 990) return;
                g.DrawString(Localization["NVR_Manufacture"], Manager.Font, Manager.TitleTextColor, 990, 13);
            }
            else if (Server is IVAS)
            {
                if (Width <= 400) return;
                g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 500) return;
                g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 430, 13);

                if (Width <= 720) return;
                g.DrawString(Localization["NVR_Modified"], Manager.Font, Manager.TitleTextColor, 570, 13);
            }
            else if (Server is IFOS)
            {
                if (Width <= 400) return;
                g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 500) return;
                g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 430, 13);

                if (Width <= 670) return;
                g.DrawString(Localization["NVR_Status"], Manager.Font, Manager.TitleTextColor, 570, 13);

                if (Width <= 750) return;
                g.DrawString(Localization["NVR_LaunchTime"], Manager.Font, Manager.TitleTextColor, 660, 13);

                if (Width <= 910) return;
                g.DrawString(Localization["NVR_BlockSize"], Manager.Font, Manager.TitleTextColor, 805, 13);

                if (Width <= 1030) return;
                g.DrawString(Localization["NVR_Modified"], Manager.Font, Manager.TitleTextColor, 905, 13);
            }
            else if (Server is IPTS)
            {
                if (Width <= 400) return;
                g.DrawString(Localization["NVR_Port"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 500) return;
                g.DrawString(Localization["NVR_DeviceQuantity"], Manager.Font, Manager.TitleTextColor, 430, 13);

                if (Width <= 720) return;
                g.DrawString(Localization["NVR_Modified"], Manager.Font, Manager.TitleTextColor, 570, 13);
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

            if (_editVisible)
                Manager.PaintEdit(g, this);

            Brush fontBrush = Brushes.Black;
            if (NVR.FailoverSetting != null && NVR.FailoverSetting.ActiveProfile)
            {
                Manager.PaintSelected(g);
                fontBrush = Manager.SelectedTextColor;
            }

            Manager.PaintStatus(g, NVR.ReadyState);

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;

            if (DataType == "Information")
                Manager.PaintText(g, NVR.Id.ToString().PadLeft(2, '0'), fontBrush);

            g.DrawString(NVR.Name, Manager.Font, fontBrush, _nameRectangleF);

            if (Width <= 320) return;
            g.DrawString(String.Format("{0} {1}", NVR.Credential.Domain, Exist ? Localization["NVR_InUse"] : String.Empty), Manager.Font, fontBrush, _addressRectangleF);

            OnServerPaint(g);
        }

        protected virtual void OnServerPaint(Graphics g)
        {
            Brush fontBrush = (_checkBox.Visible && Checked) ? SelectedColor : Brushes.Black;

            if (Server is ICMS)
            {
                if (Width <= 400) return;
                if (NVR.IsListenEvent)
                    Manager.PaintSelected(g, 340, 13);

                if (Width <= 520) return;
                if (NVR.IsPatrolInclude)
                    Manager.PaintSelected(g, 440, 13);

                if (Width <= 620) return;
                g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 555, 13);

                if (Width <= 720) return;
                g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 675, 13);

                if (Width <= 890) return;
                if (NVR.ModifiedDate != 0)
                    g.DrawString(DateTimes.ToDateTimeString(NVR.ModifiedDate, Server.Server.TimeZone), Manager.Font, fontBrush, 790, 13);
                else
                    g.DrawString("N/A", Manager.Font, fontBrush, 790, 13);

                if (Width <= 990) return;
                g.DrawString(NVR.Manufacture, Manager.Font, fontBrush, 990, 13);

            }
            else if (Server is IVAS)
            {
                g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 335, 13);

                if (Width <= 500) return;
                g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 455, 13);

                if (Width <= 720) return;
                if (NVR.ModifiedDate != 0)
                    g.DrawString(DateTimes.ToDateTimeString(NVR.ModifiedDate, Server.Server.TimeZone), Manager.Font, fontBrush, 570, 13);
                else
                    g.DrawString("N/A", Manager.Font, fontBrush, 570, 13);
            }
            else if (Server is IFOS)
            {
                if (Width <= 400) return;
                g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 335, 13);

                if (Width <= 500) return;
                g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 455, 13);

                if (NVR.FailoverSetting == null) return;
                if (Width <= 670) return;
                if (NVR.FailoverSetting.FailPercent < 0)
                    g.DrawImage(FlagGray, 570, 12);
                else if (NVR.FailoverSetting.FailPercent >= 100)
                    g.DrawImage(FlagClose, 570, 12);
                else
                {
                    if (NVR.FailoverSetting.FailPercent <= 35)
                        g.DrawImage(FlagGreen, 570, 12);
                    else if (NVR.FailoverSetting.FailPercent <= 70)
                        g.DrawImage(FlagYellow, 570, 12);
                    else
                        g.DrawImage(FlagRed, 570, 12);

                    g.DrawString(NVR.FailoverSetting.FailPercent + "%", Manager.Font, fontBrush, 590, 13);
                }

                if (Width <= 750) return;
                g.DrawString(DurationToText(NVR.FailoverSetting.LaunchTime), Manager.Font, fontBrush, 670, 13);

                if (Width <= 910) return;
                g.DrawString(SizeToText(NVR.FailoverSetting.BlockSize), Manager.Font, fontBrush, 815, 13);

                if (Width <= 1030) return;
                if (NVR.ModifiedDate != 0)
                    g.DrawString(DateTimes.ToDateTimeString(NVR.ModifiedDate, Server.Server.TimeZone), Manager.Font, fontBrush, 905, 13);
                else
                    g.DrawString("N/A", Manager.Font, fontBrush, 905, 13);
            }
            else if (Server is IPTS)
            {
                if (Width <= 400) return;
                g.DrawString(NVR.Credential.Port.ToString(), Manager.Font, fontBrush, 335, 13);

                if (Width <= 500) return;
                g.DrawString(NVR.Device.Devices.Count.ToString(), Manager.Font, fontBrush, 455, 13);

                if (Width <= 720) return;
                if (NVR.ModifiedDate != 0)
                    g.DrawString(DateTimes.ToDateTimeString(NVR.ModifiedDate, Server.Server.TimeZone), Manager.Font, fontBrush, 570, 13);
                else
                    g.DrawString("N/A", Manager.Font, fontBrush, 570, 13);
            }
        }

        private void NVRPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle)
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
            }
            else
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
                if (OnNVREditClick != null)
                    OnNVREditClick(this, e);
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if (IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }

            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        public Brush SelectedColor = Manager.SelectedTextColor;

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            set { _checkBox.Visible = value; }
        }

        private Boolean _editVisible;
        public Boolean EditVisible
        {
            set
            {
                _editVisible = value;
                Invalidate();
            }
            get { return _editVisible; }
        }

        private String SizeToText(UInt32 block)
        {
            if (block < 1048576)
                return KBToStr(block);

            return MBToStr(block);
        }

        private String KBToStr(UInt32 block)
        {
            return (block / 1024) + @"K";
        }

        private String MBToStr(UInt32 block)
        {
            String str = "";
            if (block >= 1048576)
                str = (block / 1048576) + @"M";

            if (block % 1048576 != 0)
                str += " " + KBToStr(block % 1048576);

            return str;
        }

        private String DurationToText(UInt32 duration)
        {
            if (duration < 60000)
                return SecToStr(duration);

            return MinToStr(duration);
        }

        private String SecToStr(UInt32 sec)
        {
            return (sec / 1000) + Localization["Common_Sec"];
        }

        private String MinToStr(UInt32 min)
        {
            String str = "";
            if (min >= 60000)
                str = (min / 60000) + Localization["Common_Min"];

            if (min % 60000 != 0)
                str += " " + SecToStr(min % 60000);

            return str;
        }
    }
}
