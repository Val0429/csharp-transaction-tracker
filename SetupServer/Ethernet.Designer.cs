using IPAddressControlLib;

namespace SetupServer
{
    sealed partial class EthernetControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.warningLabel = new System.Windows.Forms.Label();
            this.DNSContainPanel = new PanelBase.DoubleBufferPanel();
            this.secondDNSPanel = new PanelBase.DoubleBufferPanel();
            this.secondDNSInput = new IPAddressControlLib.IPAddressControl();
            this.primaryDNSPanel = new PanelBase.DoubleBufferPanel();
            this.primaryDNSInput = new IPAddressControlLib.IPAddressControl();
            this.enableDynamicDNSPanel = new PanelBase.DoubleBufferPanel();
            this.checkBoxDynamicDNS = new System.Windows.Forms.CheckBox();
            this.IPcontainPanel = new PanelBase.DoubleBufferPanel();
            this.GatewayPanel = new PanelBase.DoubleBufferPanel();
            this.gatewayInput = new IPAddressControlLib.IPAddressControl();
            this.MaskPanel = new PanelBase.DoubleBufferPanel();
            this.maskInput = new SetupServer.MaskInputControl();
            this.IPAddressPanel = new PanelBase.DoubleBufferPanel();
            this.ipAddressInput = new IPAddressControlLib.IPAddressControl();
            this.DHCPPanel = new PanelBase.DoubleBufferPanel();
            this.checkBoxDynamicIP = new System.Windows.Forms.CheckBox();
            this.cableStatusPanel = new System.Windows.Forms.Panel();
            this.buttonUpdateCableStatus = new System.Windows.Forms.Button();
            this.labelCableStatus = new System.Windows.Forms.Label();
            this.DNSContainPanel.SuspendLayout();
            this.secondDNSPanel.SuspendLayout();
            this.primaryDNSPanel.SuspendLayout();
            this.enableDynamicDNSPanel.SuspendLayout();
            this.IPcontainPanel.SuspendLayout();
            this.GatewayPanel.SuspendLayout();
            this.MaskPanel.SuspendLayout();
            this.IPAddressPanel.SuspendLayout();
            this.DHCPPanel.SuspendLayout();
            this.cableStatusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // warningLabel
            // 
            this.warningLabel.BackColor = System.Drawing.Color.Transparent;
            this.warningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.warningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.warningLabel.Location = new System.Drawing.Point(12, 324);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.warningLabel.Size = new System.Drawing.Size(456, 29);
            this.warningLabel.TabIndex = 25;
            // 
            // DNSContainPanel
            // 
            this.DNSContainPanel.BackColor = System.Drawing.Color.Transparent;
            this.DNSContainPanel.Controls.Add(this.secondDNSPanel);
            this.DNSContainPanel.Controls.Add(this.primaryDNSPanel);
            this.DNSContainPanel.Controls.Add(this.enableDynamicDNSPanel);
            this.DNSContainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DNSContainPanel.Location = new System.Drawing.Point(12, 195);
            this.DNSContainPanel.Name = "DNSContainPanel";
            this.DNSContainPanel.Size = new System.Drawing.Size(456, 129);
            this.DNSContainPanel.TabIndex = 23;
            // 
            // secondDNSPanel
            // 
            this.secondDNSPanel.BackColor = System.Drawing.Color.Transparent;
            this.secondDNSPanel.Controls.Add(this.secondDNSInput);
            this.secondDNSPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.secondDNSPanel.Location = new System.Drawing.Point(0, 80);
            this.secondDNSPanel.Name = "secondDNSPanel";
            this.secondDNSPanel.Size = new System.Drawing.Size(456, 40);
            this.secondDNSPanel.TabIndex = 21;
            this.secondDNSPanel.Tag = "SecondDNS";
            // 
            // secondDNSInput
            // 
            this.secondDNSInput.AllowInternalTab = false;
            this.secondDNSInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.secondDNSInput.AutoHeight = true;
            this.secondDNSInput.BackColor = System.Drawing.SystemColors.Window;
            this.secondDNSInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.secondDNSInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondDNSInput.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondDNSInput.Location = new System.Drawing.Point(320, 10);
            this.secondDNSInput.MinimumSize = new System.Drawing.Size(99, 21);
            this.secondDNSInput.Name = "secondDNSInput";
            this.secondDNSInput.ReadOnly = false;
            this.secondDNSInput.Size = new System.Drawing.Size(120, 21);
            this.secondDNSInput.TabIndex = 3;
            this.secondDNSInput.Text = "...";
            // 
            // primaryDNSPanel
            // 
            this.primaryDNSPanel.BackColor = System.Drawing.Color.Transparent;
            this.primaryDNSPanel.Controls.Add(this.primaryDNSInput);
            this.primaryDNSPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.primaryDNSPanel.Location = new System.Drawing.Point(0, 40);
            this.primaryDNSPanel.Name = "primaryDNSPanel";
            this.primaryDNSPanel.Size = new System.Drawing.Size(456, 40);
            this.primaryDNSPanel.TabIndex = 19;
            this.primaryDNSPanel.Tag = "PrimaryDNS";
            // 
            // primaryDNSInput
            // 
            this.primaryDNSInput.AllowInternalTab = false;
            this.primaryDNSInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryDNSInput.AutoHeight = true;
            this.primaryDNSInput.BackColor = System.Drawing.SystemColors.Window;
            this.primaryDNSInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.primaryDNSInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryDNSInput.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primaryDNSInput.Location = new System.Drawing.Point(320, 9);
            this.primaryDNSInput.MinimumSize = new System.Drawing.Size(99, 21);
            this.primaryDNSInput.Name = "primaryDNSInput";
            this.primaryDNSInput.ReadOnly = false;
            this.primaryDNSInput.Size = new System.Drawing.Size(120, 21);
            this.primaryDNSInput.TabIndex = 2;
            this.primaryDNSInput.Text = "...";
            // 
            // enableDynamicDNSPanel
            // 
            this.enableDynamicDNSPanel.BackColor = System.Drawing.Color.Transparent;
            this.enableDynamicDNSPanel.Controls.Add(this.checkBoxDynamicDNS);
            this.enableDynamicDNSPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.enableDynamicDNSPanel.Location = new System.Drawing.Point(0, 0);
            this.enableDynamicDNSPanel.Name = "enableDynamicDNSPanel";
            this.enableDynamicDNSPanel.Size = new System.Drawing.Size(456, 40);
            this.enableDynamicDNSPanel.TabIndex = 18;
            this.enableDynamicDNSPanel.Tag = "EnableDynamicDNS";
            // 
            // checkBoxDynamicDNS
            // 
            this.checkBoxDynamicDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDynamicDNS.AutoSize = true;
            this.checkBoxDynamicDNS.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDynamicDNS.Location = new System.Drawing.Point(368, 12);
            this.checkBoxDynamicDNS.Name = "checkBoxDynamicDNS";
            this.checkBoxDynamicDNS.Size = new System.Drawing.Size(72, 19);
            this.checkBoxDynamicDNS.TabIndex = 1;
            this.checkBoxDynamicDNS.Text = "Enabled";
            this.checkBoxDynamicDNS.UseVisualStyleBackColor = true;
            // 
            // IPcontainPanel
            // 
            this.IPcontainPanel.BackColor = System.Drawing.Color.Transparent;
            this.IPcontainPanel.Controls.Add(this.GatewayPanel);
            this.IPcontainPanel.Controls.Add(this.MaskPanel);
            this.IPcontainPanel.Controls.Add(this.IPAddressPanel);
            this.IPcontainPanel.Controls.Add(this.DHCPPanel);
            this.IPcontainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.IPcontainPanel.Location = new System.Drawing.Point(12, 18);
            this.IPcontainPanel.Name = "IPcontainPanel";
            this.IPcontainPanel.Size = new System.Drawing.Size(456, 177);
            this.IPcontainPanel.TabIndex = 22;
            // 
            // GatewayPanel
            // 
            this.GatewayPanel.BackColor = System.Drawing.Color.Transparent;
            this.GatewayPanel.Controls.Add(this.gatewayInput);
            this.GatewayPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.GatewayPanel.Location = new System.Drawing.Point(0, 120);
            this.GatewayPanel.Name = "GatewayPanel";
            this.GatewayPanel.Size = new System.Drawing.Size(456, 40);
            this.GatewayPanel.TabIndex = 20;
            this.GatewayPanel.Tag = "Gateway";
            // 
            // gatewayInput
            // 
            this.gatewayInput.AllowInternalTab = false;
            this.gatewayInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gatewayInput.AutoHeight = true;
            this.gatewayInput.BackColor = System.Drawing.SystemColors.Window;
            this.gatewayInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gatewayInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.gatewayInput.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gatewayInput.Location = new System.Drawing.Point(320, 9);
            this.gatewayInput.MinimumSize = new System.Drawing.Size(99, 21);
            this.gatewayInput.Name = "gatewayInput";
            this.gatewayInput.ReadOnly = false;
            this.gatewayInput.Size = new System.Drawing.Size(120, 21);
            this.gatewayInput.TabIndex = 2;
            this.gatewayInput.Text = "...";
            // 
            // MaskPanel
            // 
            this.MaskPanel.BackColor = System.Drawing.Color.Transparent;
            this.MaskPanel.Controls.Add(this.maskInput);
            this.MaskPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MaskPanel.Location = new System.Drawing.Point(0, 80);
            this.MaskPanel.Name = "MaskPanel";
            this.MaskPanel.Size = new System.Drawing.Size(456, 40);
            this.MaskPanel.TabIndex = 21;
            this.MaskPanel.Tag = "IPMask";
            // 
            // maskInput
            // 
            this.maskInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maskInput.Location = new System.Drawing.Point(195, 8);
            this.maskInput.Margin = new System.Windows.Forms.Padding(48, 24, 48, 24);
            this.maskInput.Name = "maskInput";
            this.maskInput.Size = new System.Drawing.Size(250, 24);
            this.maskInput.TabIndex = 0;
            // 
            // IPAddressPanel
            // 
            this.IPAddressPanel.BackColor = System.Drawing.Color.Transparent;
            this.IPAddressPanel.Controls.Add(this.ipAddressInput);
            this.IPAddressPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.IPAddressPanel.Location = new System.Drawing.Point(0, 40);
            this.IPAddressPanel.Name = "IPAddressPanel";
            this.IPAddressPanel.Size = new System.Drawing.Size(456, 40);
            this.IPAddressPanel.TabIndex = 19;
            this.IPAddressPanel.Tag = "IPAddress";
            // 
            // ipAddressInput
            // 
            this.ipAddressInput.AllowInternalTab = false;
            this.ipAddressInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddressInput.AutoHeight = true;
            this.ipAddressInput.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressInput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressInput.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressInput.Location = new System.Drawing.Point(320, 9);
            this.ipAddressInput.MinimumSize = new System.Drawing.Size(99, 21);
            this.ipAddressInput.Name = "ipAddressInput";
            this.ipAddressInput.ReadOnly = false;
            this.ipAddressInput.Size = new System.Drawing.Size(120, 21);
            this.ipAddressInput.TabIndex = 1;
            this.ipAddressInput.Text = "...";
            // 
            // DHCPPanel
            // 
            this.DHCPPanel.BackColor = System.Drawing.Color.Transparent;
            this.DHCPPanel.Controls.Add(this.checkBoxDynamicIP);
            this.DHCPPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DHCPPanel.Location = new System.Drawing.Point(0, 0);
            this.DHCPPanel.Name = "DHCPPanel";
            this.DHCPPanel.Size = new System.Drawing.Size(456, 40);
            this.DHCPPanel.TabIndex = 18;
            this.DHCPPanel.Tag = "EnableDynamicIP";
            // 
            // checkBoxDynamicIP
            // 
            this.checkBoxDynamicIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDynamicIP.AutoSize = true;
            this.checkBoxDynamicIP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDynamicIP.Location = new System.Drawing.Point(368, 11);
            this.checkBoxDynamicIP.Name = "checkBoxDynamicIP";
            this.checkBoxDynamicIP.Size = new System.Drawing.Size(72, 19);
            this.checkBoxDynamicIP.TabIndex = 0;
            this.checkBoxDynamicIP.Text = "Enabled";
            this.checkBoxDynamicIP.UseVisualStyleBackColor = true;
            // 
            // cableStatusPanel
            // 
            this.cableStatusPanel.Controls.Add(this.buttonUpdateCableStatus);
            this.cableStatusPanel.Controls.Add(this.labelCableStatus);
            this.cableStatusPanel.Location = new System.Drawing.Point(12, 353);
            this.cableStatusPanel.Name = "cableStatusPanel";
            this.cableStatusPanel.Padding = new System.Windows.Forms.Padding(42, 0, 0, 0);
            this.cableStatusPanel.Size = new System.Drawing.Size(456, 34);
            this.cableStatusPanel.TabIndex = 26;
            // 
            // buttonUpdateCableStatus
            // 
            this.buttonUpdateCableStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonUpdateCableStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdateCableStatus.Location = new System.Drawing.Point(6, 6);
            this.buttonUpdateCableStatus.Name = "buttonUpdateCableStatus";
            this.buttonUpdateCableStatus.Size = new System.Drawing.Size(108, 23);
            this.buttonUpdateCableStatus.TabIndex = 1;
            this.buttonUpdateCableStatus.Text = "Update Status";
            this.buttonUpdateCableStatus.UseVisualStyleBackColor = true;
            this.buttonUpdateCableStatus.Click += new System.EventHandler(this.UpdateCableStatusClick);
            // 
            // labelCableStatus
            // 
            this.labelCableStatus.AutoSize = true;
            this.labelCableStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCableStatus.Location = new System.Drawing.Point(118, 10);
            this.labelCableStatus.Name = "labelCableStatus";
            this.labelCableStatus.Size = new System.Drawing.Size(149, 15);
            this.labelCableStatus.TabIndex = 0;
            this.labelCableStatus.Text = "Network cable is plugged.";
            // 
            // EthernetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.cableStatusPanel);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.DNSContainPanel);
            this.Controls.Add(this.IPcontainPanel);
            this.Name = "EthernetControl";
            this.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.Size = new System.Drawing.Size(480, 400);
            this.DNSContainPanel.ResumeLayout(false);
            this.secondDNSPanel.ResumeLayout(false);
            this.primaryDNSPanel.ResumeLayout(false);
            this.enableDynamicDNSPanel.ResumeLayout(false);
            this.enableDynamicDNSPanel.PerformLayout();
            this.IPcontainPanel.ResumeLayout(false);
            this.GatewayPanel.ResumeLayout(false);
            this.MaskPanel.ResumeLayout(false);
            this.IPAddressPanel.ResumeLayout(false);
            this.DHCPPanel.ResumeLayout(false);
            this.DHCPPanel.PerformLayout();
            this.cableStatusPanel.ResumeLayout(false);
            this.cableStatusPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelBase.DoubleBufferPanel IPcontainPanel;
        private PanelBase.DoubleBufferPanel GatewayPanel;
        private PanelBase.DoubleBufferPanel MaskPanel;
        private PanelBase.DoubleBufferPanel IPAddressPanel;
        private PanelBase.DoubleBufferPanel DHCPPanel;
        private PanelBase.DoubleBufferPanel DNSContainPanel;
        private PanelBase.DoubleBufferPanel secondDNSPanel;
        private PanelBase.DoubleBufferPanel primaryDNSPanel;
        private PanelBase.DoubleBufferPanel enableDynamicDNSPanel;
        private System.Windows.Forms.Label warningLabel;
        private IPAddressControlLib.IPAddressControl ipAddressInput;
        private IPAddressControl gatewayInput;
        private System.Windows.Forms.CheckBox checkBoxDynamicIP;
        private IPAddressControl secondDNSInput;
        private IPAddressControl primaryDNSInput;
        private System.Windows.Forms.CheckBox checkBoxDynamicDNS;
        private SetupServer.MaskInputControl maskInput;
        private System.Windows.Forms.Panel cableStatusPanel;
        private System.Windows.Forms.Button buttonUpdateCableStatus;
        private System.Windows.Forms.Label labelCableStatus;
    }
}
