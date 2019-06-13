using System;
using System.Drawing;
using System.Windows.Forms;
using Interface;
using ApplicationForms = App.ApplicationForms;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem
    {
        private readonly Panel _tempPanel = new Panel();
        private Panel _cpuPanel;
        private Panel _totalBitratePanel;

        private Panel _userPanel;
        private Panel _timePanel;

        protected override void InitializeStatePanel()
        {
            StatePanel = ApplicationForms.StatusPanelUI2();

            Form.Controls.Add(StatePanel);
            
            _timePanel = ApplicationForms.TimePanel();
            _cpuPanel = ApplicationForms.CPUUsagePanel();
            _totalBitratePanel = ApplicationForms.TotalBitratePanel();
            _userPanel = ApplicationForms.UserPanel();

            //right
            StatePanel.Controls.Add(_userPanel);
            StatePanel.Controls.Add(_timePanel);

            //left
            StatePanel.Controls.Add(_totalBitratePanel);
            StatePanel.Controls.Add(_cpuPanel);
            _tempPanel.Width = 10;
            _tempPanel.Dock = DockStyle.Left;
            _tempPanel.BackColor = Color.Transparent;
            StatePanel.Controls.Add(_tempPanel);

            _cpuPanel.Paint += CPUPanelPaint;
            _totalBitratePanel.Paint += TotalBitratePanelPaint;
            _userPanel.Paint += UserPanelPaint;
            _timePanel.Paint += TimePanelPaint;
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
            if (_bitrateUsage == "N/A" || String.IsNullOrEmpty(_bitrateUsage))
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
            g.DrawString(Localization["Application_CPUUsage"].Replace("%1", CMS.Server.CPUUsage.ToString()),
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

        private void TimePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var timezone = "(GMT ";
            if (CMS.Server.TimeZone > 0)
                timezone += "+ ";
            else if (CMS.Server.TimeZone < 0)
                timezone += "- ";

            //only hours, ex +8  -7
            if ((CMS.Server.TimeZone % 3600) == 0)
            {
                timezone += Convert.ToUInt16(Math.Abs(CMS.Server.TimeZone / 3600));
            }
            else //containes minis, ex +3.5
            {
                timezone += Math.Round(Math.Abs(CMS.Server.TimeZone / 3600.0), 1, MidpointRounding.AwayFromZero);
            }
            //var text = CMS.Server.Location + @" " + CMS.Server.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var text = timezone + ") " + CMS.Server.DateTime.ToString("yyyy/MM/dd HH:mm:ss");

            g.DrawString(text, ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);
        }

        private Boolean _firstCheckUserWidth = true;
        private void UserPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(CMS.User.Current.Credential.UserName, ApplicationForms.StatusFont, ApplicationForms.StatusFontColor, 25, 9);

            //check used width, adjust panel width, but just once
            if (!_firstCheckUserWidth) return;
            _firstCheckUserWidth = false;

            var text = CMS.User.Current.Credential.UserName;
            SizeF fSize = g.MeasureString(text, ApplicationForms.StatusFont);
            _userPanel.Width = 25 + Convert.ToInt32(fSize.Width) + 15; //15 for safe range;
        }
    }
}
