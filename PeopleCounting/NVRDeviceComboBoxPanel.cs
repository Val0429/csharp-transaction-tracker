using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PeopleCounting
{
    public sealed class NVRDeviceComboBoxPanel : Panel
    {
        public event EventHandler OnDeviceEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly  CheckBox _checkBox = new CheckBox();
        private readonly ComboBox _nvrComboBox = new ComboBox();
        private readonly ComboBox _deviceComboBox = new ComboBox();
        private readonly TextBox _dispatcherTextBox = new PanelBase.HotKeyTextBox();
        private readonly ComboBox _thresholdComboBox = new ComboBox();
        private readonly ComboBox _numberComboBox = new ComboBox();
        private readonly ComboBox _persionComboBox = new ComboBox();
        private readonly TextBox _directTextBox = new PanelBase.HotKeyTextBox();
        private readonly TextBox _frameTextBox = new PanelBase.HotKeyTextBox();
        private readonly TextBox _retryTextBox = new PanelBase.HotKeyTextBox();
        private readonly TextBox _intervalTextBox = new PanelBase.HotKeyTextBox();

        private IVAS _vas;
        public IVAS VAS
        {
            get { return _vas; }
            set
            {
                _vas = value;
                _nvrComboBox.SelectedIndexChanged -= NVRComboBoxSelectedIndexChanged;
                _nvrComboBox.Items.Clear();

                _deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
                _deviceComboBox.Items.Clear();

                _dispatcherTextBox.TextChanged -= DispatcherTextBoxTextChanged;
                _dispatcherTextBox.Text = @"http://";

                if (_vas == null) return;
                foreach (KeyValuePair<UInt16, INVR> obj in _vas.NVR.NVRs)
                {
                    if (obj.Value.Device != null && obj.Value.Device.Devices.Count > 0)
                        _nvrComboBox.Items.Add(obj.Value);
                }

                _nvrComboBox.SelectedIndexChanged -= NVRComboBoxSelectedIndexChanged;
                _nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;
            }
        }

        public Boolean IsTitle;
        private ICamera _camera;
        public ICamera Camera
        {
            get { return _camera; }
            set 
            {
                _camera = value;
                if(_camera != null)
                {
                    _deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
                    _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
                    _nvrComboBox.SelectedItem = _camera.Server;

                    _dispatcherTextBox.TextChanged -= DispatcherTextBoxTextChanged;
                    _dispatcherTextBox.Text = (_camera.Dispatcher.Domain == "") ?  @"http://" : _camera.Dispatcher.Domain;
                    _dispatcherTextBox.TextChanged += DispatcherTextBoxTextChanged;

                    _thresholdComboBox.SelectedIndexChanged -= ThresholdComboBoxSelectedIndexChanged;
                    _thresholdComboBox.SelectedItem = _camera.PeopleCountingSetting.FeatureThreshold;
                    _thresholdComboBox.SelectedIndexChanged += ThresholdComboBoxSelectedIndexChanged;

                    _numberComboBox.SelectedIndexChanged -= NumberComboBoxChanged;
                    _numberComboBox.SelectedItem = _camera.PeopleCountingSetting.FeatureNumberThreshold;
                    _numberComboBox.SelectedIndexChanged += NumberComboBoxChanged;

                    _persionComboBox.SelectedIndexChanged -= PersionComboBoxSelectedIndexChanged;
                    _persionComboBox.SelectedItem = _camera.PeopleCountingSetting.PersonNumber;
                    _persionComboBox.SelectedIndexChanged += PersionComboBoxSelectedIndexChanged;

                    _directTextBox.TextChanged -= DirectTextBoxTextChanged;
                    _directTextBox.Text = _camera.PeopleCountingSetting.DirectNumber.ToString();
                    _directTextBox.TextChanged += DirectTextBoxTextChanged;

                    _frameTextBox.TextChanged -= FrameTextBoxTextChanged;
                    _frameTextBox.Text = _camera.PeopleCountingSetting.FrameIndex.ToString();
                    _frameTextBox.TextChanged += FrameTextBoxTextChanged;

                    _retryTextBox.TextChanged -= RetryTextBoxTextChanged;
                    _retryTextBox.Text = _camera.PeopleCountingSetting.Retry.ToString();
                    _retryTextBox.TextChanged += RetryTextBoxTextChanged;

                    _intervalTextBox.TextChanged -= IntervalTextBoxTextChanged;
                    _intervalTextBox.Text = _camera.PeopleCountingSetting.Interval.ToString();
                    _intervalTextBox.TextChanged += IntervalTextBoxTextChanged;
                }
            }
        }

        public NVRDeviceComboBoxPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_NVR", "NVR"},
                                   {"Control_Device", "Device"},
                                   {"Control_URI", "URI"},
                                   {"VAS_ID", "ID"},
                                   {"VAS_Threshold", "Threshold"},
                                   {"VAS_NumberThreshold", "Number"},
                                   {"VAS_Person", "Person"},
                                   {"VAS_Direct", "Direct"},
                                   {"VAS_Frame", "Frame"},
                                   {"VAS_Retry", "Retry"},
                                   {"VAS_Interval", "Interval"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;
            
            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            _nvrComboBox.Sorted = true;
            _nvrComboBox.Width = 110;
            _nvrComboBox.Location = new Point(70, 7);
            _nvrComboBox.Dock = DockStyle.None;
            _nvrComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _nvrComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Manager.DropDownWidth(_nvrComboBox);

            _deviceComboBox.Sorted = true;
            _deviceComboBox.Width = 110;
            _deviceComboBox.Location = new Point(195, 7);
            _deviceComboBox.Dock = DockStyle.None;
            _deviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _deviceComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Manager.DropDownWidth(_deviceComboBox);

            _dispatcherTextBox.Width = 150;
            _dispatcherTextBox.Location = new Point(320, 8);
            _dispatcherTextBox.Dock = DockStyle.None;
            _dispatcherTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _thresholdComboBox.Width = 60;
            _thresholdComboBox.Location = new Point(485, 7);
            _thresholdComboBox.Dock = DockStyle.None;
            _thresholdComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _thresholdComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _numberComboBox.Width = 60;
            _numberComboBox.Location = new Point(560, 7);
            _numberComboBox.Dock = DockStyle.None;
            _numberComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _numberComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _persionComboBox.Width = 60;
            _persionComboBox.Location = new Point(635, 7);
            _persionComboBox.Dock = DockStyle.None;
            _persionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _persionComboBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            for (UInt16 i = 0; i <= 100; i++)
            {
                _thresholdComboBox.Items.Add(i);
                _numberComboBox.Items.Add(i);
                _persionComboBox.Items.Add(i);
            }

            _directTextBox.Width = 60;
            _directTextBox.Location = new Point(710, 7);
            _directTextBox.Dock = DockStyle.None;
            _directTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _directTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _directTextBox.ImeMode = ImeMode.Disable;

            _frameTextBox.Width = 60;
            _frameTextBox.Location = new Point(785, 7);
            _frameTextBox.Dock = DockStyle.None;
            _frameTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _frameTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _frameTextBox.ImeMode = ImeMode.Disable;

            _retryTextBox.Width = 60;
            _retryTextBox.MaxLength = 5;
            _retryTextBox.Location = new Point(860, 7);
            _retryTextBox.Dock = DockStyle.None;
            _retryTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _retryTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _retryTextBox.ImeMode = ImeMode.Disable;

            _intervalTextBox.Width = 60;
            _intervalTextBox.MaxLength = 2;
            _intervalTextBox.Location = new Point(935, 7);
            _intervalTextBox.Dock = DockStyle.None;
            _intervalTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _intervalTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _intervalTextBox.ImeMode = ImeMode.Disable;

            Controls.Add(_checkBox);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceComboBox);
            Controls.Add(_dispatcherTextBox);

            Controls.Add(_thresholdComboBox);
            Controls.Add(_numberComboBox);
            Controls.Add(_persionComboBox);
            Controls.Add(_directTextBox);
            Controls.Add(_frameTextBox);
            Controls.Add(_retryTextBox);
            Controls.Add(_intervalTextBox);
            
            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += DevicePanelMouseClick;
            Paint += DevicePanelPaint;
        }

        private void NVRComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            var server = _nvrComboBox.SelectedItem as IServer;
            
            if (server != null && server.Device != null)
            {
                _deviceComboBox.Items.Clear();
                _deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;

                foreach (KeyValuePair<UInt16, IDevice> obj in server.Device.Devices)
                {
                    _deviceComboBox.Items.Add(obj.Value);
                }

                IDevice copyFrom = (from obj in server.Device.Devices
                            where (obj.Value is ICamera)
                            where ((ICamera)obj.Value).Profile == _camera.Profile
                            select obj.Value).FirstOrDefault();

                if (copyFrom != null)
                {
                    _deviceComboBox.SelectedItem = copyFrom;
                    _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
                }
                else
                {
                    _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
                    if (_deviceComboBox.Items.Count > 0)
                        _deviceComboBox.SelectedIndex = 0;
                }
            }
        }

        public void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_vas.Device.Devices.ContainsValue(_camera))
            {
                _camera = null;
                return;
            }
            
            var copyFrom = _deviceComboBox.SelectedItem as ICamera;
            if (copyFrom == null) return;

            _camera.Server = copyFrom.Server;
            _camera.Profile = copyFrom.Profile;
            _camera.Name = copyFrom.Name;
            _camera.Model = copyFrom.Model;
            _camera.Rectangles.Clear();
            _camera.Dispatcher = new NetworkCredential();
            //_device.Rectangles = copyFrom.Rectangles;
            //_device.Dispatcher = copyFrom.Dispatcher;
        }

        private void ThresholdComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            _camera.PeopleCountingSetting.FeatureThreshold = Convert.ToUInt16(_thresholdComboBox.SelectedItem);
        }

        private void NumberComboBoxChanged(Object sender, EventArgs e)
        {
            _camera.PeopleCountingSetting.FeatureNumberThreshold = Convert.ToUInt16(_numberComboBox.SelectedItem);
        }

        private void PersionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            _camera.PeopleCountingSetting.PersonNumber = Convert.ToUInt16(_persionComboBox.SelectedItem);
        }

        private void DirectTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_directTextBox.Text))
                _camera.PeopleCountingSetting.DirectNumber = Convert.ToUInt16(_directTextBox.Text);
        }

        private void FrameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_frameTextBox.Text))
                _camera.PeopleCountingSetting.FrameIndex = Convert.ToUInt16(_frameTextBox.Text);
        }

        private void DispatcherTextBoxTextChanged(Object sender, EventArgs e)
        {
            _camera.Dispatcher.Domain = _dispatcherTextBox.Text;
        }

        private void RetryTextBoxTextChanged(Object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(_retryTextBox.Text))
                _camera.PeopleCountingSetting.Retry = 5; //default
            else
            {
                var retry = Convert.ToInt32(_retryTextBox.Text);
                _camera.PeopleCountingSetting.Retry = Convert.ToUInt16(Math.Min(Math.Max(retry, 0), 65535));
            }
        }

        private void IntervalTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_intervalTextBox.Text))
                _camera.PeopleCountingSetting.Interval = 30; //default
            else
            {
                var interval = Convert.ToInt32(_intervalTextBox.Text);
                _camera.PeopleCountingSetting.Interval = Convert.ToUInt16(Math.Min(Math.Max(interval, 1), 60));
            }
        }

        private void DevicePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            
            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            if(IsTitle)
            {
                if (Width <= 100) return;
                Manager.PaintText(g, Localization["VAS_ID"]);

                if (Width <= 200) return;
                g.DrawString(Localization["Control_NVR"], Manager.Font, Brushes.Black, 75, 13);

                if (Width <= 310) return;
                g.DrawString(Localization["Control_Device"], Manager.Font, Brushes.Black, 200, 13);
                
                if (Width <= 470) return;
                g.DrawString(Localization["Control_URI"], Manager.Font, Brushes.Black, 325, 13);
                
                if (Width <= 550) return;
                g.DrawString(Localization["VAS_Threshold"], Manager.Font, Brushes.Black, 490, 13);
                
                if (Width <= 625) return;
                g.DrawString(Localization["VAS_NumberThreshold"], Manager.Font, Brushes.Black, 565, 13);

                if (Width <= 700) return;
                g.DrawString(Localization["VAS_Person"], Manager.Font, Brushes.Black, 640, 13);

                if (Width <= 775) return;
                g.DrawString(Localization["VAS_Direct"], Manager.Font, Brushes.Black, 715, 13);

                if (Width <= 850) return;
                g.DrawString(Localization["VAS_Frame"], Manager.Font, Brushes.Black, 790, 13);

                if (Width <= 925) return;
                g.DrawString(Localization["VAS_Retry"], Manager.Font, Brushes.Black, 865, 13);

                if (Width <= 1000) return;
                g.DrawString(Localization["VAS_Interval"], Manager.Font, Brushes.Black, 940, 13);
                return;
            }

            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }
            Manager.PaintText(g, Camera.Id.ToString().PadLeft(2, '0'), fontBrush);
        }

        public void ShowSetting()
        {
            _nvrComboBox.Visible = _deviceComboBox.Visible = _dispatcherTextBox.Visible =
                _thresholdComboBox.Visible = _numberComboBox.Visible = _persionComboBox.Visible =
                _directTextBox.Visible = _frameTextBox.Visible = _retryTextBox.Visible = _intervalTextBox.Visible = true;
        }

        public void HideSetting()
        {
            _nvrComboBox.Visible = _deviceComboBox.Visible = _dispatcherTextBox.Visible =
                _thresholdComboBox.Visible = _numberComboBox.Visible = _persionComboBox.Visible =
                _directTextBox.Visible = _frameTextBox.Visible = _retryTextBox.Visible = _intervalTextBox.Visible = false;
        }

        private void DevicePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle)
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
            }
            else
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
                if (OnDeviceEditClick != null)
                    OnDeviceEditClick(this, e);
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if(IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }
            
            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        public Brush SelectedColor = Manager.SelectedTextColor;

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            set
            {
                _checkBox.Visible = value;
                _nvrComboBox.Enabled = _deviceComboBox.Enabled = _dispatcherTextBox.Enabled =
                _thresholdComboBox.Enabled = _numberComboBox.Enabled = _persionComboBox.Enabled =
                _directTextBox.Enabled = _frameTextBox.Enabled = _retryTextBox.Visible = _intervalTextBox.Visible = !value;
            }
        }

        private Boolean _editVisible;
        public Boolean EditVisible { 
            set
            {
                _editVisible = value;
                Invalidate();
            }
        }
    }
}
