using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interface;

namespace SetupServer
{
    public sealed partial class MaskInputControl : UserControl
    {
        public event EventHandler<EventArgs<String>> OnValueChanged;
        private List<ComboBox> _comboBoxes;
        private readonly List<UInt16> _firstMaskList = new List<UInt16> { 255, 254, 252, 248, 240, 224, 192 };
        private readonly List<UInt16> _maskList = new List<UInt16> { 255, 254, 252, 248, 240, 224, 192, 128, 0 };
        private Boolean _loading = false;
        public MaskInputControl()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Dock = DockStyle.None;
           

            foreach (UInt16 number in _firstMaskList)
            {
                cmbMask1.Items.Add(number);
            }

            foreach (UInt16 number in _maskList)
            {
                cmbMask2.Items.Add(number);
                cmbMask3.Items.Add(number);
                if(number < 254) cmbMask4.Items.Add(number);
            }

            cmbMask1.SelectedValueChanged += Mask1SelectedValueChanged;
            cmbMask2.SelectedValueChanged += Mask2SelectedValueChanged;
            cmbMask3.SelectedValueChanged += Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged += Mask4SelectedValueChanged;

            _comboBoxes = new List<ComboBox>
            {
                cmbMask1, cmbMask2, cmbMask3, cmbMask4
            };

            EnabledChanged += MaskInputControlEnabledChanged;
        }

        private void Mask1SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            cmbMask2.SelectedValueChanged -= Mask2SelectedValueChanged;
            cmbMask3.SelectedValueChanged -= Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            if(Convert.ToUInt16(cmbMask1.SelectedItem) < 255)
            {
                cmbMask2.SelectedItem = Convert.ToUInt16(0);
                cmbMask2.Enabled = false;
            }
            else
            {
                cmbMask2.Enabled = true;
            }
            Mask2SelectedValueChanged(null, null);
            cmbMask2.SelectedValueChanged -= Mask2SelectedValueChanged;
            cmbMask3.SelectedValueChanged -= Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            cmbMask2.SelectedValueChanged += Mask2SelectedValueChanged;
            cmbMask3.SelectedValueChanged += Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged += Mask4SelectedValueChanged;
        }

        private void Mask2SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            cmbMask3.SelectedValueChanged -= Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            if (Convert.ToUInt16(cmbMask2.SelectedItem) < 255)
            {
                cmbMask3.SelectedItem = Convert.ToUInt16(0);
                cmbMask3.Enabled = false;
            }
            else
            {
                cmbMask3.Enabled = true;
            }
            Mask3SelectedValueChanged(null, null);
            cmbMask3.SelectedValueChanged -= Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            cmbMask3.SelectedValueChanged += Mask3SelectedValueChanged;
            cmbMask4.SelectedValueChanged += Mask4SelectedValueChanged;
        }

        private void Mask3SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            if (Convert.ToUInt16(cmbMask3.SelectedItem) < 255)
            {
                cmbMask4.SelectedItem = Convert.ToUInt16(0);
                cmbMask4.Enabled = false;
            }
            else
            {
                cmbMask4.Enabled = true;
            }
            Mask4SelectedValueChanged(null, null);
            cmbMask4.SelectedValueChanged -= Mask4SelectedValueChanged;
            cmbMask4.SelectedValueChanged += Mask4SelectedValueChanged;
        }

        private void Mask4SelectedValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
           if(OnValueChanged != null)
               OnValueChanged(this,new EventArgs<String>(String.Format("{0}.{1}.{2}.{3}", cmbMask1.SelectedItem, cmbMask2.SelectedItem,cmbMask3.SelectedItem,cmbMask4.SelectedItem)));
        }

        public new String Text()
        {
            return String.Format("{0}.{1}.{2}.{3}", cmbMask1.SelectedItem, cmbMask2.SelectedItem, cmbMask3.SelectedItem,
                                 cmbMask4.SelectedItem);
        }
        public new void Text(String value)
        {

            //172.2.2.2
            _loading = true;
            if(value.IndexOf(".") < 0) return;
            var values = value.Split('.');
            for (var i = 0; i < values.Length; i++)
            {
                ushort result;
                if (UInt16.TryParse(values[i], out result))
                {
                    _comboBoxes[i].SelectedItem = Convert.ToUInt16(values[i]);
                }
            }
            _loading = false;
        }

        private void MaskInputControlEnabledChanged(object sender, EventArgs e)
        {
            if (Enabled)
            {
                cmbMask1.Enabled = true;
                Mask1SelectedValueChanged(null, null);
                return;
            }
            EnabledChanged -= MaskInputControlEnabledChanged;
            Enabled = true;
            cmbMask1.Enabled = false;
            cmbMask2.Enabled = false;
            cmbMask3.Enabled = false;
            cmbMask4.Enabled = false;
            Enabled = false;
            EnabledChanged -= MaskInputControlEnabledChanged;
            EnabledChanged += MaskInputControlEnabledChanged;
        }
    }
   
}
