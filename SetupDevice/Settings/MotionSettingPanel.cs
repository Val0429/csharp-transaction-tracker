using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public partial class MotionSettingPanel : UserControl
    {
        public IServer Server;
        private Image _snapshot = null;
        private ICamera _camera;
        public ICamera Camera
        {
            get { return _camera; }
            set {
                _camera = value;
                _snapshot = _camera.GetSnapshot();
            }
        }

        public void Initialize()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;

            BackgroundImage = Manager.BackgroundNoBorder;

            for (UInt16 i = 0; i <= 15; i++)
            {
                temporalComboBox.Items.Add(i);
                spatialComboBox.Items.Add(i);
                levelComboBox.Items.Add(i);
            }

            for (UInt16 i = 0; i <= 63; i++)
                speedComboBox.Items.Add(i);

            for (UInt16 i = 1; i <= 100; i++)
            {
                thresholdComboBox1.Items.Add(i);
                thresholdComboBox2.Items.Add(i);
                thresholdComboBox3.Items.Add(i);
            }

            checkBox1.CheckedChanged += CheckBox1CheckedChanged;
            checkBox2.CheckedChanged += CheckBox2CheckedChanged;
            checkBox3.CheckedChanged += CheckBox3CheckedChanged;

            thresholdComboBox1.SelectedIndexChanged += ThresholdComboBox1SelectedIndexChanged;
            thresholdComboBox2.SelectedIndexChanged += ThresholdComboBox2SelectedIndexChanged;
            thresholdComboBox3.SelectedIndexChanged += ThresholdComboBox3SelectedIndexChanged;

            temporalComboBox.SelectedIndexChanged += TemporalComboBoxSelectedIndexChanged;
            spatialComboBox.SelectedIndexChanged += SpatialComboBoxSelectedIndexChanged;
            levelComboBox.SelectedIndexChanged += LevelComboBoxSelectedIndexChanged;
            speedComboBox.SelectedIndexChanged += SpeedComboBoxvSelectedIndexChanged;
        }

        private void ThresholdComboBox1SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.MotionThreshold[1] = Convert.ToUInt16(thresholdComboBox1.SelectedItem);
            CameraModify();
        }

        private void ThresholdComboBox2SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.MotionThreshold[2] = Convert.ToUInt16(thresholdComboBox2.SelectedItem);
            CameraModify();
        }

        private void ThresholdComboBox3SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.MotionThreshold[3] = Convert.ToUInt16(thresholdComboBox3.SelectedItem);
            CameraModify();
        }

        private void TemporalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.Profile.CaptureCardConfig.TemporalSensitivity = Convert.ToUInt16(temporalComboBox.SelectedItem);
            CameraModify();
        }

        private void SpatialComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.Profile.CaptureCardConfig.SpatialSensitivity = Convert.ToUInt16(spatialComboBox.SelectedItem);
            CameraModify();
        }

        private void LevelComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.Profile.CaptureCardConfig.LevelSensitivity = Convert.ToUInt16(levelComboBox.SelectedItem);
            CameraModify();
        }

        private void SpeedComboBoxvSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            _camera.Profile.CaptureCardConfig.Speed = Convert.ToUInt16(speedComboBox.SelectedItem);
            CameraModify();
        }

        private void CheckBox1CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (checkBox1.Checked)
            {
                if (!_camera.MotionRectangles.ContainsKey(1))
                    _camera.MotionRectangles.Add(1, new Rectangle(0, 0, 80, 80));

                thresholdComboBox1.Enabled = true;
            }
            else
            {
                _camera.MotionRectangles.Remove(1);
                thresholdComboBox1.Enabled = false;
            }

            MotionRegionControl.DrawSettingByMotionRectangles();
            CameraModify();
        }

        private void CheckBox2CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (checkBox2.Checked)
            {
                if (!_camera.MotionRectangles.ContainsKey(2))
                    _camera.MotionRectangles.Add(2, new Rectangle(80, 80, 80, 80));

                thresholdComboBox2.Enabled = true;
            }
            else
            {
                _camera.MotionRectangles.Remove(2);
                thresholdComboBox2.Enabled = false;
            }

            MotionRegionControl.DrawSettingByMotionRectangles();
            CameraModify();
        }

        private void CheckBox3CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (checkBox3.Checked)
            {
                if (!_camera.MotionRectangles.ContainsKey(3))
                    _camera.MotionRectangles.Add(3, new Rectangle(160, 160, 80, 80));

                thresholdComboBox3.Enabled = true;
            }
            else
            {
                _camera.MotionRectangles.Remove(3);
                thresholdComboBox3.Enabled = false;
            }

            MotionRegionControl.DrawSettingByMotionRectangles();
            CameraModify();
        }

        private Boolean _isEdit = false;
        public void ParseDevice()
        {
            _isEdit = false;

            thresholdComboBox1.Enabled = checkBox1.Checked = (_camera.MotionRectangles.ContainsKey(1));
            thresholdComboBox2.Enabled = checkBox2.Checked = (_camera.MotionRectangles.ContainsKey(2));
            thresholdComboBox3.Enabled = checkBox3.Checked = (_camera.MotionRectangles.ContainsKey(3));

            thresholdComboBox1.SelectedItem = _camera.MotionThreshold[1];
            thresholdComboBox2.SelectedItem = _camera.MotionThreshold[2];
            thresholdComboBox3.SelectedItem = _camera.MotionThreshold[3];

            temporalComboBox.SelectedItem = _camera.Profile.CaptureCardConfig.TemporalSensitivity;
            spatialComboBox.SelectedItem = _camera.Profile.CaptureCardConfig.SpatialSensitivity;
            levelComboBox.SelectedItem = _camera.Profile.CaptureCardConfig.LevelSensitivity;
            speedComboBox.SelectedItem = _camera.Profile.CaptureCardConfig.Speed;

            MotionRegionControl.ArrangeSettingByDevice(_camera, _snapshot);
            MotionRegionControl.DrawSettingByMotionRectangles();

            _isEdit = true;
        }
        
        public void CameraModify()
        {
            if (_camera.ReadyState == ReadyState.Ready)
                _camera.ReadyState = ReadyState.Modify;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}
