using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class TimeControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;
        public TimeControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"MessageBox_Confirm","Confirm"},
                {"SetupServer_TimeZone", "Time Zone"},
                {"SetupServer_Time", "Date & Time"},
                {"SetupServer_EnabledDaylight", "Enable Daylight Saving"}, 
                {"SetupServer_NTPServer", "NTP Server"}, 
                {"SetupServer_Enabled", "Enabled"},
                {"SetupServer_DateTimeNTPServerWarning", "If you manually set the time, NTP server will be disabled."},
                {"SetupServer_DateTimeChangeWarning", "When adjusting the time and date, if the new date is one day or more earlier than the original date, then the log between the original date and the new date will be erased."}  
            };
            Localizations.Update(Localization);

            InitializeComponent();

            timeZonePanel.Paint += PanelPaint;
            dateTimePanel.Paint += PanelPaint;
            daylightPanel.Paint += PanelPaint;
            NTPServerPanel.Paint += PanelPaint;

            checkBoxDaylight.Text = checkBoxNTPServer.Text = Localization["SetupServer_Enabled"];
            labelNote.Text = Localization["SetupServer_DateTimeNTPServerWarning"];

            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "DateTime";

            BackgroundImage = Manager.BackgroundNoBorder;

            this.datePicker.CustomFormat = DateTimeConverter.GetDatePattern();
            this.timePicker.CustomFormat = DateTimeConverter.GetTimePattern();
        }

        public void InitialTimeSetting()
        {
            foreach (Constant.TimeZone zone in Server.Server.TimeZones)
                zoneComboBox.Items.Add(zone.Name);

            zoneComboBox.SelectedIndexChanged += ZoneComboBoxSelectedIndexChanged;
            datePicker.ValueChanged += DateTimePickerValueChanged;
            timePicker.ValueChanged += DateTimePickerValueChanged;
            checkBoxDaylight.Click += CheckBoxDaylightClick;
            checkBoxNTPServer.Click += CheckBoxNTPServerClick;
            textBoxNTPServer.TextChanged += TextBoxNTPServerTextChanged;

            Server.Server.OnDateTimeUpdate += ServerOnDateTimeUpdate;

            timeZonePanel.Visible = Server.Server.CheckSetupEnabled("DataTimeTimeZone");
            dateTimePanel.Visible = Server.Server.CheckSetupEnabled("DataTimeDateTime");
            daylightPanel.Visible = Server.Server.CheckSetupEnabled("DataTimeDaylightSaving");
            NTPServerPanel.Visible = labelNote.Visible = Server.Server.CheckSetupEnabled("DataTimeNTPServer");
        }

        private void DateTimePickerValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            var zerohour = Server.Server.ChangedDateTime.AddSeconds(Server.Server.ChangedTimeZone * -1);

            var oldDateTimeUtc = new DateTime(zerohour.Year, zerohour.Month, zerohour.Day, 0, 0, 0);

            var newDateTimeUtc = new DateTime(datePicker.Value.Year, datePicker.Value.Month, datePicker.Value.Day,
                timePicker.Value.Hour, timePicker.Value.Minute, timePicker.Value.Second).AddSeconds(Server.Server.ChangedTimeZone * -1);
            
            if (oldDateTimeUtc > newDateTimeUtc)
            {
                var result = MessageBox.Show(Localization["SetupServer_DateTimeChangeWarning"], Localization["MessageBox_Confirm"],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ChangeDateTime();
                }
                else
                {
                    datePicker.Value = timePicker.Value = Server.Server.ChangedDateTime;
                }
                return;
            }

            ChangeDateTime();
        }

        private void ChangeDateTime()
        {
            Server.Server.ChangedDateTime = new DateTime(datePicker.Value.Year, datePicker.Value.Month, datePicker.Value.Day, timePicker.Value.Hour, timePicker.Value.Minute, timePicker.Value.Second);
        }

        private void TextBoxNTPServerTextChanged(Object sender, EventArgs e)
        {
            if (_loading) return;
            Server.Server.NTPServer = textBoxNTPServer.Text;
        }

        private void CheckBoxNTPServerClick(Object sender, EventArgs e)
        {
            datePicker.Enabled = timePicker.Enabled = !checkBoxNTPServer.Checked;
            Server.Server.EnableNTPServer = textBoxNTPServer.Enabled = checkBoxNTPServer.Checked;
        }

        private void CheckBoxDaylightClick(Object sender, EventArgs e)
        {
            Server.Server.ChangedEnableDaylight = checkBoxDaylight.Checked;
        }

        private void ZoneComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (_loading) return;

            Server.Server.ChangedLocation = zoneComboBox.SelectedItem.ToString();

            var timeZoneDiff = Server.Server.ChangedTimeZone;
            foreach (var zone in Server.Server.TimeZones)
            {
                if (String.Equals(zone.Name, Server.Server.ChangedLocation))
                {
                    Server.Server.ChangedTimeZone = zone.Value;
                    break;
                }
            }

            timeZoneDiff = Server.Server.ChangedTimeZone - timeZoneDiff;
            
            if (timeZoneDiff != 0)
            {
                Server.Server.ChangedDateTime = Server.Server.ChangedDateTime.AddSeconds(timeZoneDiff);
                _loading = true;
                datePicker.Value = timePicker.Value = Server.Server.ChangedDateTime;
                _loading = false;
            }
        }

        private Boolean _loading;

        public void ParseTimeConfig()
        {
            _loading = true;
            checkBoxDaylight.Checked = Server.Server.ChangedEnableDaylight;
            checkBoxNTPServer.Checked = Server.Server.EnableNTPServer;
            datePicker.Enabled = timePicker.Enabled = !Server.Server.EnableNTPServer;
            zoneComboBox.SelectedItem = Server.Server.ChangedLocation;
            textBoxNTPServer.Text = Server.Server.NTPServer;
            textBoxNTPServer.Enabled = Server.Server.EnableNTPServer;

            datePicker.Value = timePicker.Value = Server.Server.ChangedDateTime;

            _loading = false;
        }

        private delegate void ServerOnDateTimeUpdateDelegate(Object sender, EventArgs e);
        private void ServerOnDateTimeUpdate(Object sender, EventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new ServerOnDateTimeUpdateDelegate(ServerOnDateTimeUpdate), sender, e);
                    return;
                }

                ParseTimeConfig();
            }
            catch (Exception)
            {
            }
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
    }
}
