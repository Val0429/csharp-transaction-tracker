using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;
using Interface;
using ApplicationForms = App.ApplicationForms;

namespace App_POSTransactionServer
{
    public partial class POSTransactionServer
    {
        private Panel _storagePanel;
        private Panel _cpuPanel;
        private Panel _totalBitratePanel;

        private Panel _userPanel;
        private Panel _timePanel;

        protected override void InitializeStatePanel()
        {
            StatePanel = ApplicationForms.StatusPanelUI2();

            Form.Controls.Add(StatePanel);

            _storagePanel = ApplicationForms.StoragePanel();
            _storagePanel.Padding = new Padding(10, 0, 0, 0);

            _timePanel = ApplicationForms.TimePanel();
            _cpuPanel = ApplicationForms.CPUUsagePanel();
            _totalBitratePanel = ApplicationForms.TotalBitratePanel();
            _userPanel = ApplicationForms.UserPanel();

            //right
            StatePanel.Controls.Add(_userPanel);
            StatePanel.Controls.Add(_timePanel);

            //left
            //StatePanel.Controls.Add(_totalBitratePanel);
            //StatePanel.Controls.Add(_cpuPanel);
            //StatePanel.Controls.Add(_storagePanel);

            //_storagePanel.Paint += StoragePanelPaint;
            //_cpuPanel.Paint += CPUPanelPaint;
            //_totalBitratePanel.Paint += TotalBitratePanelPaint;
            _userPanel.Paint += UserPanelPaint;
            _timePanel.Paint += TimePanelPaint;
        }

        private static readonly Image _storageBg = StorageImage.StorageBg();
        private static readonly Image _storageBar = StorageImage.StorageUsage();
        private static readonly Image _keep = StorageImage.StorageKeep();

        private const UInt32 Gb2Byte = 1073741824;

        private Boolean _firstCheckStorageWidth = true;
        private void StoragePanelPaint(Object sender, PaintEventArgs e)
        {
            if (_pts == null) return;
            if (_pts.Server.StorageInfo.Count == 0) return;

            Int64 totalSpace = 0;
            Int64 usedspace = 0;
            Int64 freespace = 0;
            Int64 keepSpace = 0;

            foreach (Storage storage in _pts.Server.Storage)
            {
                if (!_pts.Server.StorageInfo.ContainsKey(storage.Key)) continue;

                keepSpace += storage.KeepSpace;
                totalSpace += _pts.Server.StorageInfo[storage.Key].Total;
                usedspace += _pts.Server.StorageInfo[storage.Key].Used;
                freespace += _pts.Server.StorageInfo[storage.Key].Free;
            }

            if (totalSpace == 0) return;

            keepSpace *= 1073741824;
            Graphics g = e.Graphics;

            Int32 left = _storagePanel.Padding.Left + 20 + 5; //icon(20) space(5)
            g.DrawImage(_storageBg, left, 14, _storageBg.Width, _storageBg.Height);

            var userec = new Rectangle
            {
                X = left,
                Y = 14,
                Width = Convert.ToUInt16(Math.Round((usedspace * _storageBg.Width) / (totalSpace * 1.0))),
                Height = _storageBar.Height
            };
            var usesrc = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = userec.Width,
                Height = userec.Height
            };
            g.DrawImage(_storageBar, userec, usesrc, GraphicsUnit.Pixel);

            Int32 keepWidth = Convert.ToUInt16(Math.Round((keepSpace * _storageBg.Width) / (totalSpace * 1.0)));
            var keeprec = new Rectangle
            {
                X = left + _storageBg.Width - keepWidth,
                Y = 14,
                Width = keepWidth,
                Height = _keep.Height
            };
            var keepsrc = new Rectangle
            {
                X = _keep.Width - keepWidth,
                Y = 0,
                Width = keeprec.Width,
                Height = keeprec.Height
            };
            g.DrawImage(_keep, keeprec, keepsrc, GraphicsUnit.Pixel);
            //g.DrawImage(_keep, left + _storageBg.Width - keepWidth, 14, keepWidth, _keep.Height);

            var usage = usedspace * 1.0 / totalSpace;

            var text = Localization["Application_StorageUsage"].Replace("%1", (usage * 100).ToString("0"))
                .Replace("%2", Math.Floor(usedspace * 1.0 / Gb2Byte).ToString("N0")).Replace("%3", Math.Floor(freespace * 1.0 / Gb2Byte).ToString("N0"));

            g.DrawString(text, ApplicationForms.StatusFont, Brushes.LightGray, _storageBg.Width + left + 5, 9);

            //check used width, adjust panel width, but just once
            if (_firstCheckStorageWidth)
            {
                _firstCheckStorageWidth = false;

                SizeF fSize = g.MeasureString(text, ApplicationForms.StatusFont);
                _storagePanel.Width = _storageBg.Width + left + 5 + Convert.ToInt32(fSize.Width) + 15; //15 for safe range;
            }
        }

        //when bitrate is N/A , dont display bitrate panel
        private String _bitrateUsage = "N/A";
        private Boolean _showBitrateUsage = true;
        public void BitrateUsageChange(Object sender, EventArgs<String> e)
        {
            //_showBitrateUsage = (String.IsNullOrEmpty(e.Value) != true);
            _bitrateUsage = e.Value;
            _totalBitratePanel.Invalidate();
        }

        private void TotalBitratePanelPaint(Object sender, PaintEventArgs e)
        {
            //no bitrate, dont display
            if (_bitrateUsage == "N/A")
            {
                //icon
                _totalBitratePanel.Controls[0].Visible = false;
                return;
            }
            _totalBitratePanel.Controls[0].Visible = true;
            Graphics g = e.Graphics;
            g.DrawString(_showBitrateUsage ? Localization["Application_TotalBitrate"].Replace("%1", _bitrateUsage) : String.Empty,
                ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);
        }

        private Boolean _firstCheckCPUWidth = true;
        private void CPUPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Localization["Application_CPUUsage"].Replace("%1", _pts.Server.CPUUsage.ToString()),
                ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);

            //check used width, adjust panel width, but just once
            if (_firstCheckCPUWidth)
            {
                _firstCheckCPUWidth = false;

                var text = Localization["Application_CPUUsage"].Replace("%1", "100");
                SizeF fSize = g.MeasureString(text, ApplicationForms.StatusFont);
                _cpuPanel.Width = 25 + Convert.ToInt32(fSize.Width) + 15; //15 for safe range;
            }
        }

        private Boolean _firstCheckTimeWidth = true;
        private void TimePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var timezone = "(GMT ";
            if (_pts.Server.TimeZone > 0)
                timezone += "+ ";
            else if (_pts.Server.TimeZone < 0)
                timezone += "- ";

            //only hours, ex +8  -7
            if ((_pts.Server.TimeZone % 3600) == 0)
            {
                timezone += Convert.ToUInt16(Math.Abs(_pts.Server.TimeZone / 3600));
            }
            else //containes minis, ex +3.5
            {
                timezone += Math.Round(Math.Abs(_pts.Server.TimeZone / 3600.0), 1, MidpointRounding.AwayFromZero);
            }
            //var text = _pts.Server.Location + @" " + _pts.Server.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var text = timezone + ") " + _pts.Server.DateTime.ToString("yyyy/MM/dd HH:mm:ss");

            g.DrawString(text, ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);
        }

        private Boolean _firstCheckUserWidth = true;
        private void UserPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(_pts.User.Current.Credential.UserName, ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);

            //check used width, adjust panel width, but just once
            if (!_firstCheckUserWidth) return;
            _firstCheckUserWidth = false;

            var text = _pts.User.Current.Credential.UserName;
            SizeF fSize = g.MeasureString(text, ApplicationForms.StatusFont);
            _userPanel.Width = 25 + Convert.ToInt32(fSize.Width) + 15; //15 for safe range;
        }
    }
}
