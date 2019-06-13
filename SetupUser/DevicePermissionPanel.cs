using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupUser
{
    public sealed class DevicePermissionPanel : Panel
    {
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;
        private readonly CheckBox _checkBox;
        private readonly CheckBox _opticalPTZCheckBox;
        private readonly CheckBox _audioInCheckBox;
        private readonly CheckBox _audioOutCheckBox;
        private readonly CheckBox _manualRecordCheckBox;
        private readonly CheckBox _exportVideoCheckBox;
        private readonly CheckBox _printImageCheckBox;

        public IApp App;
        public Boolean IsTitle;
        private Boolean _isEdit;
        private IDevice _device;
        public IDevice Device
        {
            get { return _device; }
            set
            {
                _device = value;
                CheckPermission();
            }
        }

        public IUser User;

        public DevicePermissionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePermissionPanel_ID", "ID"},
                                   {"DevicePermissionPanel_Name", "Name"},
                                   {"DevicePermissionPanel_PTZ", "PTZ"},
                                   {"DevicePermissionPanel_AudioIn", "Audio In"},
                                   {"DevicePermissionPanel_AudioOut", "Audio Out"},
                                   {"DevicePermissionPanel_ManualRecord", "Manual Record"},
                                   {"DevicePermissionPanel_ExportVideo", "Export Video"},
                                   {"DevicePermissionPanel_PrintImage", "Print Image"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _checkBox = new CheckBox
            {
                Padding = new Padding(10, 0, 0, 0),
                Dock = DockStyle.Left,
                Width = 25,
            };
            Controls.Add(_checkBox);

            _opticalPTZCheckBox = GetCheckBox("PTZ", 200);
            Controls.Add(_opticalPTZCheckBox);

            _audioInCheckBox = GetCheckBox("AudioIn", 320);
            Controls.Add(_audioInCheckBox);

            _audioOutCheckBox = GetCheckBox("AudioOut", 440);
            Controls.Add(_audioOutCheckBox);

            _manualRecordCheckBox = GetCheckBox("Record", 560);
            Controls.Add(_manualRecordCheckBox);

            _exportVideoCheckBox = GetCheckBox("Export", 680);
            Controls.Add(_exportVideoCheckBox);

            _printImageCheckBox = GetCheckBox("Print", 800);
            Controls.Add(_printImageCheckBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;
            _opticalPTZCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;
            _audioInCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;
            _audioOutCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;
            _manualRecordCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;
            _exportVideoCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;
            _printImageCheckBox.CheckedChanged += PermissionCheckBoxCheckedChanged;

            MouseClick += DevicePanelMouseClick;
            Paint += DevicePanelPaint;
        }

        private void CheckBoxImage()
        {
            _opticalPTZCheckBox.Image = (_opticalPTZCheckBox.Checked) ? _ptz : _ptzUnselect;
            _audioInCheckBox.Image = (_audioInCheckBox.Checked) ? _audioIn : _audioInUnselect;
            _audioOutCheckBox.Image = (_audioOutCheckBox.Checked) ? _audioOut : _audioOutUnselect;
            _manualRecordCheckBox.Image = (_manualRecordCheckBox.Checked) ? _record : _recordUnselect;
            _exportVideoCheckBox.Image = (_exportVideoCheckBox.Checked) ? _export : _exportUnselect;
            _printImageCheckBox.Image = (_printImageCheckBox.Checked) ? _print : _printUnselect;
        }

        private static readonly Image _ptz = Resources.GetResources(Properties.Resources.opticalptz, Properties.Resources.IMGOpticalptz);
        private static readonly Image _audioIn = Resources.GetResources(Properties.Resources.audioin, Properties.Resources.IMGAudioin);
        private static readonly Image _audioOut = Resources.GetResources(Properties.Resources.audioout, Properties.Resources.IMGAudioout);
        private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
        private static readonly Image _export = Resources.GetResources(Properties.Resources.export_video, Properties.Resources.IMGExport_video);
        private static readonly Image _print = Resources.GetResources(Properties.Resources.print_image, Properties.Resources.IMGPrint_image);

        private static readonly Image _ptzUnselect = Resources.GetResources(Properties.Resources.opticalptz_unselect, Properties.Resources.IMGOpticalptz_unselect);
        private static readonly Image _audioInUnselect = Resources.GetResources(Properties.Resources.audioin_unselect, Properties.Resources.IMGAudioin_unselect);
        private static readonly Image _audioOutUnselect = Resources.GetResources(Properties.Resources.audioout_unselect, Properties.Resources.IMGAudioout_unselect);
        private static readonly Image _recordUnselect = Resources.GetResources(Properties.Resources.record_unselect, Properties.Resources.IMGRecord_unselect);
        private static readonly Image _exportUnselect = Resources.GetResources(Properties.Resources.export_video_unselect, Properties.Resources.IMGExport_video_unselect);
        private static readonly Image _printUnselect = Resources.GetResources(Properties.Resources.print_image_unselect, Properties.Resources.IMGPrint_image_unselect);

        private static CheckBox GetCheckBox(String tag, Int32 x)
        {
            return new CheckBox
            {
                Padding = new Padding(10, 0, 35, 0), 
                Dock = DockStyle.None, 
                Size= new Size(100, 40),
                Tag = tag,
                Location = new Point(x, 0),
            };
        }

        private void PermissionCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if(!_isEdit) return;

            CheckBox checkBox = sender as CheckBox;
            if(checkBox == null) return;
            if(!User.Permissions.ContainsKey(_device)) return;

            Boolean check = checkBox.Checked;
            switch (checkBox.Tag.ToString())
            {
                case "PTZ":
                    if(!check)
                        User.Permissions[_device].Remove(Permission.OpticalPTZ);
                    else if (!User.Permissions[_device].Contains(Permission.OpticalPTZ))
                        User.Permissions[_device].Add(Permission.OpticalPTZ);
                    _opticalPTZCheckBox.Image = (check) ? _ptz : _ptzUnselect;
                    break;

                case "AudioIn":
                    if (!check)
                        User.Permissions[_device].Remove(Permission.AudioIn);
                    else if (!User.Permissions[_device].Contains(Permission.AudioIn))
                        User.Permissions[_device].Add(Permission.AudioIn);
                    _audioInCheckBox.Image = (check) ? _audioIn : _audioInUnselect;
                    break;

                case "AudioOut":
                    if (!check)
                        User.Permissions[_device].Remove(Permission.AudioOut);
                    else if (!User.Permissions[_device].Contains(Permission.AudioOut))
                        User.Permissions[_device].Add(Permission.AudioOut);
                    _audioOutCheckBox.Image = (check) ? _audioOut : _audioOutUnselect;
                    break;

                case "Record":
                    if (!check)
                        User.Permissions[_device].Remove(Permission.ManualRecord);
                    else if (!User.Permissions[_device].Contains(Permission.ManualRecord))
                        User.Permissions[_device].Add(Permission.ManualRecord);
                    _manualRecordCheckBox.Image = (check) ? _record : _recordUnselect;
                    break;

                case "Export":
                    if (!check)
                        User.Permissions[_device].Remove(Permission.ExportVideo);
                    else if (!User.Permissions[_device].Contains(Permission.ExportVideo))
                        User.Permissions[_device].Add(Permission.ExportVideo);
                    _exportVideoCheckBox.Image = (check) ? _export : _exportUnselect;
                    break;

                case "Print":
                    if (!check)
                        User.Permissions[_device].Remove(Permission.PrintImage);
                    else if (!User.Permissions[_device].Contains(Permission.PrintImage))
                        User.Permissions[_device].Add(Permission.PrintImage);
                    _printImageCheckBox.Image = (check) ? _print : _printUnselect;
                    break;
            }

            UserIsModify();
        }

        private static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);

        private static RectangleF _P2F = new RectangleF(200, 13, 115, 17);
        private static RectangleF _P3F = new RectangleF(320, 13, 115, 17);
        private static RectangleF _P4F = new RectangleF(440, 13, 115, 17);
        private static RectangleF _P5F = new RectangleF(560, 13, 115, 17);
        private static RectangleF _P6F = new RectangleF(680, 13, 115, 17);
        private static RectangleF _P7F = new RectangleF(800, 13, 115, 17);
        private void DevicePanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, control);
                if (Width <= 200) return;
                Manager.PaintTitleText(g, Localization["DevicePermissionPanel_ID"]);

                g.DrawString(Localization["DevicePermissionPanel_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);

                g.DrawString(Localization["DevicePermissionPanel_PTZ"], Manager.Font, Manager.TitleTextColor, _P2F);

                g.DrawString(Localization["DevicePermissionPanel_AudioIn"], Manager.Font, Manager.TitleTextColor, _P3F);

                g.DrawString(Localization["DevicePermissionPanel_AudioOut"], Manager.Font, Manager.TitleTextColor, _P4F);

                g.DrawString(Localization["DevicePermissionPanel_ManualRecord"], Manager.Font, Manager.TitleTextColor, _P5F);

                if (User.Group.CheckPermission("Playback", Permission.Access))
                {
                    g.DrawString(Localization["DevicePermissionPanel_ExportVideo"], Manager.Font, Manager.TitleTextColor, _P6F);

                    g.DrawString(Localization["DevicePermissionPanel_PrintImage"], Manager.Font, Manager.TitleTextColor, _P7F);
                }

                return;
            }

            Manager.Paint(g, control);
            if(_device == null) return;

            Manager.PaintStatus(g, _device.ReadyState);

            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;
            Manager.PaintText(g, _device.Id.ToString());
            g.DrawString(_device.Name, Manager.Font, fontBrush, _nameRectangleF);
        }

        private void DevicePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (_checkBox.Visible)
            {
                _checkBox.Checked = !_checkBox.Checked;
                return;
            }
        }
    
        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if(IsTitle)
            {
                if (_checkBox.Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!_checkBox.Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }

            if (!_isEdit || _device == null) return;

            //Add Device Permission
            if (_checkBox.Checked)
            {
                User.AddFullDevicePermission(_device);
            }
            else//Remove Device Permission
            {
                PermissionSelectionVisible = false;
                User.Permissions.Remove(_device);
            }

            CheckPermission();
            UserIsModify();

            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        //private IPage _playbackPage;
        private void CheckPermission()
        {
            if (_device == null) return;
            if (User == null) return;
            _isEdit = false;

            _checkBox.Checked = PermissionSelectionVisible = User.CheckPermission(_device, Permission.Access);
            if (_checkBox.Checked)
            {
                _opticalPTZCheckBox.Checked = User.CheckPermission(_device, Permission.OpticalPTZ);
                _audioInCheckBox.Checked = User.CheckPermission(_device, Permission.AudioIn);
                _audioOutCheckBox.Checked = User.CheckPermission(_device, Permission.AudioOut);
                _manualRecordCheckBox.Checked = User.CheckPermission(_device, Permission.ManualRecord);
                _exportVideoCheckBox.Checked = User.CheckPermission(_device, Permission.ExportVideo);
                _printImageCheckBox.Checked = User.CheckPermission(_device, Permission.PrintImage);
            }
            else
            {
                _opticalPTZCheckBox.Checked = _audioInCheckBox.Checked = _audioOutCheckBox.Checked =
                                                                         _manualRecordCheckBox.Checked = _exportVideoCheckBox.Checked = _printImageCheckBox.Checked = false;
            }

            //if (_playbackPage == null)
            //{
            //    if(App.Pages.ContainsKey("Playback"))
            //        _playbackPage = App.Pages["Playback"];
            //}

            _exportVideoCheckBox.Visible = _printImageCheckBox.Visible =
                                           (_permissionSelectionVisible && User.Group.CheckPermission("Playback", Permission.Access));

            CheckBoxImage();

            _isEdit = true;
            Invalidate();
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
            set{ _checkBox.Visible = value; }
        }

        private Boolean _permissionSelectionVisible;
        public Boolean PermissionSelectionVisible
        {
            set
            {
                _permissionSelectionVisible = value;
                _opticalPTZCheckBox.Visible = _audioInCheckBox.Visible = _audioOutCheckBox.Visible =
                _manualRecordCheckBox.Visible = _exportVideoCheckBox.Visible = _printImageCheckBox.Visible = _permissionSelectionVisible;
            }
        }

        public void UserIsModify()
        {
            if (User.ReadyState == ReadyState.Ready)
                User.ReadyState = ReadyState.Modify;
        }
    }
}