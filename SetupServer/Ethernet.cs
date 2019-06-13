using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
	public sealed partial class EthernetControl : UserControl
	{
		public IServer Server;
		public Dictionary<String, String> Localization;
		public Ethernet Ethernet
		{
			get
			{
				if (Server == null) return null;
				return (Server.Server.Ethernets.ContainsKey(Id)) ? Server.Server.Ethernets[Id] : null;
			}
		}
		private Boolean _loading;
		public UInt16 Id;
		private Ethernet _anotherEthernet;

		public EthernetControl()
		{
			Localization = new Dictionary<String, String>
			{
				{"MessageBox_Error", "Error"},
				{"MessageBox_Confirm", "Confirm"},
				{"MessageBox_Information", "Information"},

				{"SetupServer_Ethernet", "Ethernet"},
				{"SetupServer_EnableDynamicIP", "Dynamic IP Address (DHCP)"},
				{"SetupServer_IPAddress", "IP Address"},
				{"SetupServer_IPMask", "Mask"},
				{"SetupServer_Gateway", "Gateway"},
				{"SetupServer_HostName", "Host Name"},
				{"SetupServer_EnableDynamicDNS", "Obtain DNS server address automatically"},
				{"SetupServer_PrimaryDNS", "Primary DNS"},
				{"SetupServer_SecondDNS", "Second DNS"},
				{"SetupServer_EthernetOffline", "Network cable is unplugged."}, 
				{"SetupServer_EthernetOnline", "Network cable is plugged."}, 
				{"SetupServer_EthernetUpdateCableStatus", "Update Status"}, 
				{"SetupServer_Enabled", "Enabled"},
				 {"SetupServer_UpdateEthernet", "Update"},
				{"SetupServer_EthernetWarning", "If the value entered for this parameter is invalid, the value will not be saved and the parameter will be empty."}
			};
			Localizations.Update(Localization);

			InitializeComponent();

			DoubleBuffered = true;
			Dock = DockStyle.None;

			BackgroundImage = Manager.BackgroundNoBorder;

			DHCPPanel.Paint += InputPanelPaint;
			IPAddressPanel.Paint += InputPanelPaint;
			MaskPanel.Paint += InputPanelPaint;
			GatewayPanel.Paint += InputPanelPaint;

			enableDynamicDNSPanel.Paint += InputPanelPaint;
			primaryDNSPanel.Paint += InputPanelPaint;
			secondDNSPanel.Paint += InputPanelPaint;

			checkBoxDynamicIP.Text = Localization["SetupServer_Enabled"];
			checkBoxDynamicDNS.Text = Localization["SetupServer_Enabled"];
			warningLabel.Text = Localization["SetupServer_EthernetWarning"];

			checkBoxDynamicIP.Click += CheckBoxDynamicIPClick;
			ipAddressInput.FieldChangedEvent += IPAddressInputFieldChangedEvent;
			maskInput.OnValueChanged += MaskInputOnValueChanged;
			gatewayInput.FieldChangedEvent += GatewayInputFieldChangedEvent;

			checkBoxDynamicDNS.Click += CheckBoxDynamicDNSClick;
			primaryDNSInput.FieldChangedEvent += PrimaryDNSInputFieldChangedEvent;
			secondDNSInput.FieldChangedEvent += SecondDNSInputFieldChangedEvent;

			buttonUpdateCableStatus.Text = Localization["SetupServer_EthernetUpdateCableStatus"];

		}

		public void InitailEthernetSetting()
		{
			Name = String.Format("{0} {1}", Localization["SetupServer_Ethernet"], Id);
			Server.Server.OnEthernetUpdateCableStatus += ServerOnEthernetUpdateCableStatus;

			var anotherId = (ushort) (Id == 1 ? 2 : 1);
			_anotherEthernet = Server.Server.Ethernets.ContainsKey(anotherId) ? Server.Server.Ethernets[anotherId] : null;
		}

		public void ParseEthernetInfo()
		{
			if(Ethernet == null) return;
			_loading = true;
			checkBoxDynamicIP.Checked = Ethernet.DynamicIP;
			ipAddressInput.Text = Ethernet.IPAddress;
			maskInput.Text(Ethernet.Mask);
			gatewayInput.Text = Ethernet.Gateway;
			ClickDymanicIP(true);

			checkBoxDynamicDNS.Checked = Server.Server.DNS.DynamicDNS;
			primaryDNSInput.Text = Server.Server.DNS.PrimaryDNS;
			secondDNSInput.Text = Server.Server.DNS.SecondDNS;
			CheckBoxDynamicDNSClick(null, null);

			cableStatusPanel.Visible = !Ethernet.DeviceCarrier;
			labelCableStatus.ForeColor = Ethernet.DeviceCarrier ? Color.DarkGreen : Color.Red;
			labelCableStatus.Text = Ethernet.DeviceCarrier ? Localization["SetupServer_EthernetOnline"] : Localization["SetupServer_EthernetOffline"];

			_loading = false;
		}

		private void CheckBoxDynamicIPClick(Object sender, EventArgs e)
		{
			ClickDymanicIP(false);
		}

		private void ClickDymanicIP(Boolean isInitial)
		{
			Ethernet.DynamicIP = checkBoxDynamicIP.Checked;
			ipAddressInput.Enabled = maskInput.Enabled = gatewayInput.Enabled = !checkBoxDynamicIP.Checked;

			if (isInitial == false && !Ethernet.DynamicIP && Server.Server.DNS.DynamicDNS)
			{
				checkBoxDynamicDNS.Checked = false;
				EnabledDynamicDNS();
			}

			//If have 2 ethernets(because share one DNS setting), checkBoxDynamicDNS will be enabled with both DynamicIP are enabled.
			//ClickDymanicIP can't  checked when  ethernet1 or ethernet2 DynamicIP is unchecked.
			checkBoxDynamicDNS.Enabled = (Ethernet.DynamicIP && (_anotherEthernet == null || _anotherEthernet.DynamicIP));

			UpdateStatus();
		}

		private void CheckBoxDynamicDNSClick(Object sender, EventArgs e)
		{
			EnabledDynamicDNS();
			UpdateStatus();
		}

		private void EnabledDynamicDNS()
		{
			Server.Server.DNS.DynamicDNS = checkBoxDynamicDNS.Checked;
			primaryDNSInput.Enabled = secondDNSInput.Enabled = !checkBoxDynamicDNS.Checked;
		}

		private void IPAddressInputFieldChangedEvent(Object sender, IPAddressControlLib.FieldChangedEventArgs e)
		{
			if (_loading) return;
			Ethernet.IPAddress = ipAddressInput.Text;
			UpdateStatus();
		}

		private void MaskInputOnValueChanged(Object sender, EventArgs<string> e)
		{
			if (_loading) return;
			Ethernet.Mask = maskInput.Text();
			UpdateStatus();
		}

		private void GatewayInputFieldChangedEvent(Object sender, IPAddressControlLib.FieldChangedEventArgs e)
		{
			if (_loading) return;
			Ethernet.Gateway = gatewayInput.Text;
			UpdateStatus();
		}

		private void PrimaryDNSInputFieldChangedEvent(Object sender, IPAddressControlLib.FieldChangedEventArgs e)
		{
			if (_loading) return;
			Server.Server.DNS.PrimaryDNS = primaryDNSInput.Text;
			UpdateStatus();
		}

		private void SecondDNSInputFieldChangedEvent(Object sender, IPAddressControlLib.FieldChangedEventArgs e)
		{
			if (_loading) return;
			Server.Server.DNS.SecondDNS = secondDNSInput.Text;
			UpdateStatus();
		}

		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;
			Manager.Paint(g, control);
			if (Localization.ContainsKey("SetupServer_" + control.Tag))
				Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		private void UpdateStatus()
		{
			if (_loading) return;
			Ethernet.ReadyStatus = ManagerReadyState.MajorModify;
		}

		private void UpdateCableStatusClick(object sender, EventArgs e)
		{
			Server.Server.UpdateEthernetCableStatus(Id);
		}

		private void ServerOnEthernetUpdateCableStatus(object sender, EventArgs<UInt16> e)
		{
			if (e.Value != Id) return;
			labelCableStatus.ForeColor = Ethernet.DeviceCarrier ? Color.DarkGreen : Color.Red;
			labelCableStatus.Text = Ethernet.DeviceCarrier ? Localization["SetupServer_EthernetOnline"] : Localization["SetupServer_EthernetOffline"];
		}
	}
}
