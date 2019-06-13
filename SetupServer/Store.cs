using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed partial class StoreControl : UserControl
    {
        private IPTS _pts;
        public IServer Server
        {
            set { _pts = value as IPTS; }
            get { return _pts; }
        }

        public Dictionary<String, String> Localization;

        public StoreControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupServer_StoreId", "Id"},
                                   {"SetupServer_StoreName", "Name"},
                                   {"SetupServer_StoreAddress", "Address"},
                                   {"SetupServer_StorePhone", "Phone"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Store";

            BackgroundImage = Manager.BackgroundNoBorder;

            storeIdPanel.Paint += PanelPaint;
            storeNamePanel.Paint += PanelPaint;
            addressPanel.Paint += PanelPaint;
            phonePanel.Paint += PanelPaint;

            storeIdTextBox.TextChanged += StoreIdTextBoxTextChanged;
            storeNameTextBox.TextChanged += StoreNameTextBoxTextChanged;
            addressTextBox.TextChanged += AddressTextBoxTextChanged;
            phoneTextBox.TextChanged += PhoneTextBoxTextChanged;
        }

        public Boolean IsEditing;
        public void ParseSetting()
        {
            if (_pts == null) return;
            IsEditing = false;

            storeIdTextBox.Text = _pts.Configure.Store.Id;
            storeNameTextBox.Text = _pts.Configure.Store.Name;
            addressTextBox.Text = _pts.Configure.Store.Address;
            phoneTextBox.Text = _pts.Configure.Store.Phone;

            IsEditing = true;
        }

        private void StoreIdTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _pts.Configure.Store.Id = storeIdTextBox.Text;
        }

        private void StoreNameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _pts.Configure.Store.Name = storeNameTextBox.Text;
        }

        private void AddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _pts.Configure.Store.Address = addressTextBox.Text;
        }

        private void PhoneTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _pts.Configure.Store.Phone = phoneTextBox.Text;
        }

        private void PanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if(control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);
            if (Width <= 100) return;

            if(Localization.ContainsKey("SetupServer_" + control.Tag))
                Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
