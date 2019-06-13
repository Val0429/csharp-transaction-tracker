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
    public sealed partial class EditDevicePanel : UserControl
    {
        public IServer Server;
        public ICamera Camera;
        public Dictionary<String, String> Localization;
        public Boolean IsEditing;

        private readonly List<String> _modifyed = new List<string>();

        public EditDevicePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_ID", "ID"},
                                   {"DevicePanel_Name", "Name"},
                                   {"DevicePanel_NetworkAddress", "Network Address"},
                                    {"DevicePanel_Model", "Model"},
                                   {"DevicePanel_LiveStream", "Live Stream"},
                                   {"DevicePanel_Manufacturer", "Manufacturer"},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            idPanel.Paint += PaintInput;
            domainPanel.Paint += PaintInput;
            streamPanel.Paint += PaintInput;
            namePanel.Paint += PaintInput;
            manufacturePanel.Paint += PaintInput;
            modelPanel.Paint += PaintInput;

            nameTextBox.TextChanged += NameTextBoxTextChanged;
           
            nameTextBox.LostFocus += NameTextBoxLostFocus;
        }

        private void NameTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("NAME")) return;

            _modifyed.Remove("NAME");
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (Localization.ContainsKey("DevicePanel_" + ((Control)sender).Tag))
                Manager.PaintText(g, Localization["DevicePanel_" + ((Control)sender).Tag]);
            else
                Manager.PaintText(g, ((Control)sender).Tag.ToString());
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            Camera.Name = nameTextBox.Text;
            Camera.ReadyState = ReadyState.Modify;
            Camera.Server.ReadyState = ReadyState.Modify;
        }

        public void Parse()
        {
            nameTextBox.TextChanged -= NameTextBoxTextChanged;
            nameTextBox.LostFocus -= NameTextBoxLostFocus;

            idTextBox.Text = Camera.Id.ToString();
            nameTextBox.Text = Camera.Name;
            domainTextBox.Text = Camera.Profile.NetworkAddress;

            liveStreamTextBox.Text = Camera.Profile.StreamId.ToString();
            manufactureTextBox.Text = Camera.Model.Manufacture;
            modelTextBox.Text = Camera.Model.Model;

            nameTextBox.TextChanged += NameTextBoxTextChanged;
            nameTextBox.LostFocus += NameTextBoxLostFocus;
        }
    }
}
