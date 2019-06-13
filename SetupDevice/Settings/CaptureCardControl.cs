using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;

namespace SetupDevice
{
    public sealed partial class CaptureCardControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        private CaptureCardConfig _captureCardConfig;

        public CaptureCardControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_CaptureCardConfig", "Capture Card Config"},
                                   {"DevicePanel_Brightness", "Brightness"},
                                   {"DevicePanel_Contrast", "Contrast"},
                                   {"DevicePanel_Hue", "Hue"},
                                   {"DevicePanel_Saturation", "Saturation"},
                                   {"DevicePanel_Sharpness", "Sharpness"},
                                   {"DevicePanel_Gamma", "Gamma"},
                                   {"DevicePanel_ColorEnable", "Color Enable"},
                                   {"DevicePanel_WhiteBalance", "White Balance"},
                                   {"DevicePanel_BacklightCompensation", "Backlight Compensation"},
                                   {"DevicePanel_Gain", "Gain"}
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += CaptureCardControlPaint;

            for (UInt16 i = 0; i <= 255; i++)
            {
                brightnessComboBox.Items.Add(i);
                contrastComboBox.Items.Add(i);
                hueComboBox.Items.Add(i);
                saturationComboBox.Items.Add(i);
                sharpnessComboBox.Items.Add(i);
                gammaComboBox.Items.Add(i);
                colorEnableComboBox.Items.Add(i);
                whiteBalanceComboBox.Items.Add(i);
                backlightCompensationComboBox.Items.Add(i);
                gainComboBox.Items.Add(i);
            }

            brightnessPanel.Paint += PaintInput;
            contrastPanel.Paint += PaintInput;
            huePanel.Paint += PaintInput;
            saturationPanel.Paint += PaintInput;
            gammaPanel.Paint += PaintInput;
            sharpnessPanel.Paint += PaintInput;
            colorEnablePanel.Paint += PaintInput;
            whiteBalancePanel.Paint += PaintInput;
            backlightCompensationPanel.Paint += PaintInput;
            gainPanel.Paint += PaintInput;

            brightnessComboBox.SelectedIndexChanged += BrightnessComboBoxSelectedIndexChanged;
            contrastComboBox.SelectedIndexChanged += ContrastComboBoxSelectedIndexChanged;
            hueComboBox.SelectedIndexChanged += HueComboBoxSelectedIndexChanged;
            saturationComboBox.SelectedIndexChanged += SaturationComboBoxSelectedIndexChanged;
            gammaComboBox.SelectedIndexChanged += GammaComboBoxSelectedIndexChanged;
            sharpnessComboBox.SelectedIndexChanged += SharpnessComboBoxSelectedIndexChanged;
            colorEnableComboBox.SelectedIndexChanged += ColorEnableComboBoxSelectedIndexChanged;
            whiteBalanceComboBox.SelectedIndexChanged += WhiteBalanceComboBox;
            backlightCompensationComboBox.SelectedIndexChanged += BacklightCompensationComboBoxSelectedIndexChanged;
            gainComboBox.SelectedIndexChanged += GainComboBoxSelectedIndexChanged;
        }

        public void ParseDevice()
        {
            _captureCardConfig = EditPanel.Camera.Profile.CaptureCardConfig;
            if (_captureCardConfig == null) return;

            brightnessComboBox.SelectedItem = _captureCardConfig.Brightness;
            contrastComboBox.SelectedItem = _captureCardConfig.Contrast;
            hueComboBox.SelectedItem = _captureCardConfig.Hue;
            saturationComboBox.SelectedItem = _captureCardConfig.Saturation;
            sharpnessComboBox.SelectedItem = _captureCardConfig.Sharpness;
            gammaComboBox.SelectedItem = _captureCardConfig.Gamma;
            colorEnableComboBox.SelectedItem = _captureCardConfig.ColorEnable;
            whiteBalanceComboBox.SelectedItem = _captureCardConfig.WhiteBalance;
            backlightCompensationComboBox.SelectedItem = _captureCardConfig.BacklightCompensation;
            gainComboBox.SelectedItem = _captureCardConfig.Gain;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Stretch":
                    colorEnablePanel.Visible =
                        whiteBalancePanel.Visible =
                        backlightCompensationPanel.Visible =
                        gainPanel.Visible = false;
                    
                    break;

                default:
                    colorEnablePanel.Visible =
                        whiteBalancePanel.Visible =
                        backlightCompensationPanel.Visible =
                        gainPanel.Visible = true;
                    break;
            }
        }

        private void BrightnessComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Brightness = Convert.ToUInt16(brightnessComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void ContrastComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Contrast = Convert.ToUInt16(contrastComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void HueComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Hue = Convert.ToUInt16(hueComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void SaturationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Saturation = Convert.ToUInt16(saturationComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void GammaComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Gamma = Convert.ToUInt16(gammaComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void SharpnessComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.Sharpness = Convert.ToUInt16(sharpnessComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void ColorEnableComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.ColorEnable = Convert.ToUInt16(colorEnableComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void WhiteBalanceComboBox(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.WhiteBalance = Convert.ToUInt16(whiteBalanceComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void BacklightCompensationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.BacklightCompensation = Convert.ToUInt16(backlightCompensationComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void GainComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing || _captureCardConfig == null) return;

            _captureCardConfig.BacklightCompensation = Convert.ToUInt16(backlightCompensationComboBox.SelectedItem);

            EditPanel.CameraIsModify();
        }

        private void CaptureCardControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_CaptureCardConfig"], SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            SetupBase.Manager.Paint(g, control);

            if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                SetupBase.Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
            else
                SetupBase.Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
