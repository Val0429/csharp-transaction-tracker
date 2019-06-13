using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public sealed partial class LiveCheckControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;
        private UInt64 _miniInterval = 5;
        public LiveCheckControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"DevicePanel_LiveCheck", "Live Check"},
                {"DevicePanel_URI", "URI"},
                {"DevicePanel_Interval", "Interval (Sec)"},
                {"DevicePanel_RetryCount", "Retry Count"}
            };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += MultiStreamingControlPaint;
            uriPanel.Paint += PaintInput;
            intervalPanel.Paint += PaintInput;
            retryCountPanel.Paint += PaintInput;

            intervalTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            retryCountTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            uriTextBox.TextChanged += URITextBoxTextChanged;
            intervalTextBox.TextChanged += IntervalTextBoxTextChanged;
            retryCountTextBox.TextChanged += RetryCountTextBoxTextChanged;
        }

        private void RetryCountTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            UInt64 value = 0;

            if (!String.IsNullOrEmpty(retryCountTextBox.Text))
                value = Convert.ToUInt64(retryCountTextBox.Text);

            EditPanel.Camera.Profile.LiveCheckRetryCount = value;

            EditPanel.CameraIsModify();
        }

        private void IntervalTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            UInt64 value = _miniInterval;

            if (!String.IsNullOrEmpty(intervalTextBox.Text))
                value = Convert.ToUInt64(intervalTextBox.Text);

            EditPanel.Camera.Profile.LiveCheckInterval = Math.Max(value, _miniInterval);

            EditPanel.CameraIsModify();
        }

        private void URITextBoxTextChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.LiveCheckURI = uriTextBox.Text;

            EditPanel.CameraIsModify();
        }

        private void MultiStreamingControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_LiveCheck"], Manager.Font, Brushes.DimGray, 8, 10);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            var control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);

            if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        public void ParseDevice()
        {
            uriTextBox.Text = EditPanel.Camera.Profile.LiveCheckURI;
            intervalTextBox.Text = EditPanel.Camera.Profile.LiveCheckInterval.ToString();
            retryCountTextBox.Text = EditPanel.Camera.Profile.LiveCheckRetryCount.ToString();
        }

    }
}
