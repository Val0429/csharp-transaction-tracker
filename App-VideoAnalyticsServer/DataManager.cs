using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace App_VideoAnalyticsServer
{
    public partial class VideoAnalyticsServer
    {
        private Boolean _nvrsReady;
        private Boolean _logining = true;
        public override Boolean Login()
        {
            InitializePanel();

            _vas.OnLoadFailure -= VasOnLoadFailure;
            _vas.OnLoadFailure += VasOnLoadFailure;
            _vas.Initialize();
            _vas.Login();

            while (_logining)
            {
                if (_vas.ReadyState == ReadyState.Ready)
                {
                    _vas.OnLoadFailure -= VasOnLoadFailure;

                    if (_vas.Server.ProductNo != "00007")
                    {
                        TopMostMessageBox.Show(Localization["Application_ServerIsNotVAS"].Replace("%1", _vas.Credential.Domain), Localization["MessageBox_Error"],
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    InitialTimer();
                    if (_vas.NVR.NVRs.Count == 0)
                    {
                        ConvertTempDeviceToNVRDevice();
                        return true;
                    }

                    _nvrsReady = false;

                    foreach (KeyValuePair<UInt16, INVR> obj in _vas.NVR.NVRs)
                    {
                        if (obj.Value.ValidateCredential())
                        {
                            ValidateCredential(obj.Value);
                        }
                        else
                        {
                            obj.Value.Initialize();
                            obj.Value.ReadyState = ReadyState.Unavailable;
                        }
                    }

                    if (!_vas.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New)))
                    {
                        ConvertTempDeviceToNVRDevice();
                        return true;
                    }

                    break;
                }
                Application.RaiseIdle(null);
                Thread.Sleep(200);
            }

            String progress = "";
            while (_logining)
            {
                if (_nvrsReady)
                {
                    ConvertTempDeviceToNVRDevice();
                    return true;
                }

                foreach (KeyValuePair<UInt16, INVR> obj in _vas.NVR.NVRs)
                {
                    if (obj.Value.LoginProgress != null && obj.Value.LoginProgress != progress && obj.Value.ReadyState != ReadyState.Ready)
                    {
                        progress = obj.Value.LoginProgress;
                        _nvrLoginProgress = progress + " (" + obj.Value.Name + ")";
                        break;
                    }
                }

                Application.RaiseIdle(null);
                Thread.Sleep(250);
            }
            return false;
        }

        private void ConvertTempDeviceToNVRDevice()
        {
            //Convert temp device to nvr's device
            if (_vas.Device.Devices.Count == 0) return;
            if (_vas.NVR.NVRs.Count == 0) return;

            var devices = new List<IDevice>(_vas.Device.Devices.Values);
            _vas.Device.Devices.Clear();
            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in _vas.Device.Groups)
            {
                obj.Value.Items.Clear();
            }
            foreach (IDevice device in devices)
            {
                var camera = device as ICamera;
                if (camera == null) continue;

                if (_vas.NVR.NVRs.ContainsKey(device.Server.Id))
                {
                    if (_vas.NVR.NVRs[device.Server.Id].ReadyState == ReadyState.Ready && _vas.NVR.NVRs[device.Server.Id].Device.Devices.ContainsKey(device.Id))
                    {
                        var copyFrom = _vas.NVR.NVRs[device.Server.Id].Device.Devices[device.Id] as ICamera;
                        if (copyFrom == null) continue;

                        camera.Id = _vas.Device.GetNewDeviceId();
                        camera.Server = copyFrom.Server;
                        camera.Profile = copyFrom.Profile;
                        camera.Name = copyFrom.Name;
                        camera.Model = copyFrom.Model;

                        foreach (KeyValuePair<UInt16, IDeviceGroup> obj in _vas.Device.Groups)
                        {
                            obj.Value.Items.Add(camera);
                        }
                        _vas.Device.Devices.Add(camera.Id, camera);
                    }
                }
            }
            if (_vas.Device.Devices.Count > 0)
            {
                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in _vas.Device.Groups)
                {
                    obj.Value.Items.Sort((x, y) => (x.Id - y.Id));
                }
            }
        }

        private void ValidateCredential(INVR nvr)
        {
            nvr.OnLoadFailure -= NvrOnLoadFailure;
            nvr.OnLoadFailure += NvrOnLoadFailure;
            nvr.OnLoadComplete -= NvrOnLoadComplete;
            nvr.OnLoadComplete += NvrOnLoadComplete;

            nvr.Initialize();
            nvr.License.CheckLicenseExpire = false;
            nvr.Login();
        }

        private void NvrOnLoadComplete(Object sender, EventArgs<String> e)
        {
            var nvr = sender as INVR;
            if (nvr != null)
            {
                nvr.OnLoadFailure -= NvrOnLoadFailure;
                nvr.OnLoadComplete -= NvrOnLoadComplete;
                nvr.StopTimer();

                if ((nvr.Server.ProductNo != "00001" && nvr.Server.ProductNo != "00004" && nvr.Server.ProductNo != "00005") && nvr.ReadyState == ReadyState.Ready)
                    nvr.ReadyState = ReadyState.Unavailable;
            }

            if (_vas.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
                return;

            _nvrsReady = true;
        }

        private void NvrOnLoadFailure(Object sender, EventArgs<String> e)
        {
            var nvr = sender as INVR;
            if (nvr != null)
            {
                nvr.OnLoadFailure -= NvrOnLoadFailure;
                nvr.OnLoadComplete -= NvrOnLoadComplete;
            }

            if (_vas.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
                return;

            _nvrsReady = true;
        }

        private void VasOnLoadFailure(Object sender, EventArgs<String> e)
        {
            _logining = false;
            _vas.OnLoadFailure -= VasOnLoadFailure;

            CancelAutoLogin();

            TopMostMessageBox.Show(Localization["Application_ConnectionTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"]
                , MessageBoxButtons.OK, MessageBoxIcon.Stop);

            try
            {
                Application.Exit();
            }
            catch (Exception)
            {
            }
        }

        public override void Save()
        {
            _watch.Reset();
            _watch.Start();

            _vas.OnSaveFailure -= NvrOnSaveFailure;
            _vas.OnSaveFailure += NvrOnSaveFailure;
            _vas.OnSaveComplete -= VasOnSaveComplete;
            _vas.OnSaveComplete += VasOnSaveComplete;

            _vas.Save();
        }

        private void VasOnSaveComplete(Object sender, EventArgs<String> e)
        {
            _vas.OnSaveFailure -= NvrOnSaveFailure;
            _vas.OnSaveComplete -= VasOnSaveComplete;

            _watch.Stop();
            String msg = Localization["Application_SaveCompleted"];
                //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (_vas.Server.ReadyStatus == ManagerReadyState.Unavailable)
            {
                TopMostMessageBox.Show(Localization["Application_CannotConnect"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_vas.Server.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_PortChange"].Replace("%1", _vas.Server.Port.ToString()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_vas.User.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_UserChange"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            _nvrsReady = true;
            var resyncCount = 0;

            foreach (KeyValuePair<UInt16, INVR> obj in _vas.NVR.NVRs)
            {
                if (obj.Value.ReadyState == ReadyState.ReSync)
                {
                    resyncCount++;
                    _nvrsReady = false;
                    ValidateCredential(obj.Value);
                    _vas.NVRModify(obj.Value);
                }
            }

            while (!_nvrsReady)
            {
                Thread.Sleep(250);
            }

            ShowUnavailableNVRMessage();

            TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowUnavailableNVRMessage()
        {
            var unavailableNVRs = new List<INVR>();

            foreach (KeyValuePair<UInt16, INVR> obj in _vas.NVR.NVRs)
            {
                if (obj.Value.ReadyState != ReadyState.Ready)
                    unavailableNVRs.Add(obj.Value);
            }

            if (unavailableNVRs.Count > 0)
            {
                var list = new List<String>();

                unavailableNVRs.Sort((x, y) => (x.Id - y.Id));
                foreach (INVR nvr in unavailableNVRs)
                {
                    nvr.Device.Devices.Clear();
                    nvr.Device.Groups.Clear();
                    list.Add(nvr + " " + nvr.Credential.Domain);
                    if (list.Count >= 10)
                    {
                        list.Add("...");
                        break;
                    }
                }
                TopMostMessageBox.Show(Localization["Application_SomeNVRUnavailable"] + Environment.NewLine + String.Join(Environment.NewLine, list.ToArray()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NvrOnSaveFailure(Object sender, EventArgs<String> e)
        {
            _vas.OnSaveFailure -= NvrOnSaveFailure;
            _vas.OnSaveComplete -= VasOnSaveComplete;

            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


        private String _nvrLoginProgress;
        public override String LoginProgress
        {
            get { return _nvrLoginProgress ?? _vas.LoginProgress; }
        }
    }
}
