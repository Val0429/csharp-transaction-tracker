using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace App_POSTransactionServer
{
    public partial class POSTransactionServer
    {
        private Boolean _nvrsReady;
        private Boolean _logining = true;
        public override Boolean Login()
        {
            InitializePanel();

            _pts.OnLoadFailure -= PTSOnLoadFailure;
            _pts.OnLoadFailure += PTSOnLoadFailure;
            _pts.Initialize();
            _pts.Login();
         //   _pts.OnLoadComplete -= PTSOnLoadComplete;
           // _pts.OnLoadComplete += PTSOnLoadComplete;

             while (_logining)
           // while (false)
            {
                if (_pts.ReadyState == ReadyState.Ready)
                {
                    _pts.OnLoadFailure -= PTSOnLoadFailure;
                    
                    //if (_pts.Server.ProductNo != "00003")
                    //{
                    //    TopMostMessageBox.Show(Localization["Application_ServerIsNotPTS"].Replace("%1", _pts.Credential.Domain), Localization["MessageBox_Error"],
                    //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return false;
                    //}

                    if (_pts.NVR.NVRs.Count == 0)
                    {
                        ConvertTempDeviceToNVRDevice();
                        InitialTimer();

                        return true;
                    }

                    _nvrsReady = false;

                    foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
                    {
                        if (obj.Value.ValidateCredential())
                        {
                            LoginNVR(obj.Value);
                            _pts.NVRModify(obj.Value);
                        }
                        else
                        {
                            obj.Value.Initialize();
                            if (obj.Value.Utility == null)
                            {
                                TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                                return false;
                            }
                            obj.Value.ReadyState = ReadyState.Unavailable;
                        }
                    }

                    if (!_pts.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New)))
                    {
                        ConvertTempDeviceToNVRDevice();
                        InitialTimer();
                       
                        return true;
                    }

                    break;
                }

                Application.RaiseIdle(null);
                Thread.Sleep(250);
            }

            String progress = "";
            while (_logining)
            {
                if (_nvrsReady)
                {
                    ConvertTempDeviceToNVRDevice();
                    InitialTimer();
                    return true;
                }

                foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
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
            if (_pts.POS.POSServer.Count == 0) return;
            foreach (IPOS pos in _pts.POS.POSServer)
            {
                var devices = new List<IDevice>(pos.Items);
                pos.Items.Clear();
                pos.View.Clear();
                foreach (IDevice device in devices)
                {
                    if (_pts.NVR.NVRs.ContainsKey(device.Server.Id))
                    {
                        if (_pts.NVR.NVRs[device.Server.Id].ReadyState == ReadyState.Ready && _pts.NVR.NVRs[device.Server.Id].Device.Devices.ContainsKey(device.Id))
                        {
                            pos.Items.Add(_pts.NVR.NVRs[device.Server.Id].Device.Devices[device.Id]);
                        }
                    }
                }

                if (pos.Items.Count > 0)
                    pos.Items.Sort((x, y) => (x.Id - y.Id));
                pos.View.AddRange(pos.Items);
            }
        }

        private void LoginNVR(INVR nvr)
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
                //why stop timer?
                //nvr.StopTimer();

                switch (nvr.Manufacture)
                {
                    case "Salient":
                        break;

                    default:
                        if ((nvr.Server.ProductNo != "00001" && nvr.Server.ProductNo != "00004" && nvr.Server.ProductNo != "00005") && nvr.ReadyState == ReadyState.Ready)
                            nvr.ReadyState = ReadyState.Unavailable;
                        break;
                }
            }

            if (_pts.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
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

            if (_pts.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
                return;

            _nvrsReady = true;
        }

        private void PTSOnLoadFailure(Object sender, EventArgs<String> e)
        {
            _logining = false;
            _pts.OnLoadFailure -= PTSOnLoadFailure;

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

            _pts.OnSaveFailure -= PTSOnSaveFailure;
            _pts.OnSaveFailure += PTSOnSaveFailure;
            _pts.OnSaveComplete -= PTSOnSaveComplete;
            _pts.OnSaveComplete += PTSOnSaveComplete;

            _pts.Save();
        }

        private void PTSOnSaveComplete(Object sender, EventArgs<String> e)
        {
            _pts.OnSaveFailure -= PTSOnSaveFailure;
            _pts.OnSaveComplete -= PTSOnSaveComplete;

            _watch.Stop();
            String msg = Localization["Application_SaveCompleted"];
            //+@" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (_pts.Server.ReadyStatus == ManagerReadyState.Unavailable)
            {
                TopMostMessageBox.Show(Localization["Application_CannotConnect"], Localization["MessageBox_Information"],
                                       MessageBoxButtons.OK, MessageBoxIcon.Stop);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_pts.Server.ReadyStatus == ManagerReadyState.MajorModify)
            {
                if (_pts.Server.IsPortChange)
                {
                    msg += Environment.NewLine +
                           Localization["Application_PortChange"].Replace("%1", _pts.Server.Port.ToString());
                }

                if (_pts.Server.IsSSLPortChange)
                {
                    msg += Environment.NewLine +
                           Localization["Application_SSLPortChange"].Replace("%1", _pts.Server.SSLPort.ToString());
                }

                TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_pts.User.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_UserChange"],
                                       Localization["MessageBox_Information"], MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            _nvrsReady = true;
            var resyncCount = 0;
            foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
            {
                if (obj.Value.ReadyState == ReadyState.ReSync)
                {
                    resyncCount++;
                    _nvrsReady = false;
                    //1231321321321321
                    LoginNVR(obj.Value);
                    _pts.NVRModify(obj.Value);
                }
            }

            while (!_nvrsReady)
            {
                Thread.Sleep(250);
            }

            ShowUnavailableNVRMessage();
            if (resyncCount > 0)
            {
                ConvertTempDeviceToNVRDevice();
                _pts.ListenNVREvent();
            }

            //if (IntPtr.Size == 4) //only 32bit support joystick
            {
                if (_pts.Configure.EnableJoystick && !_pts.Utility.JoystickEnabled)
                    _pts.Utility.InitializeJoystick();

                if (!_pts.Configure.EnableJoystick && _pts.Utility.JoystickEnabled)
                    _pts.Utility.StopJoystickTread();
            }

            TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowUnavailableNVRMessage()
        {
            var unavailableNVRs = new List<INVR>();

            foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
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

        private void PTSOnSaveFailure(Object sender, EventArgs<String> e)
        {
            _pts.OnSaveFailure -= PTSOnSaveFailure;
            _pts.OnSaveComplete -= PTSOnSaveComplete;

            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private String _nvrLoginProgress;
        public override String LoginProgress
        {
            get { return _nvrLoginProgress ?? _pts.LoginProgress; }
        }
        private void PTSOnLoadComplete(Object sender, EventArgs<String> e) {

            _nvrsReady = true;
            var resyncCount = 0;
            foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
            {
                if (obj.Value.ReadyState == ReadyState.New)
                {
                    resyncCount++;

                    _nvrsReady = false;
                    //1231321321321321
                    LoginNVR(obj.Value);
                    _pts.NVRModify(obj.Value);
                }
            }

            while (!_nvrsReady)
            {
                Thread.Sleep(250);
            }

            ShowUnavailableNVRMessage();
            if (resyncCount > 0)
            {
                ConvertTempDeviceToNVRDevice();
                _pts.ListenNVREvent();
            }

            //if (IntPtr.Size == 4) //only 32bit support joystick
            {
                if (_pts.Configure.EnableJoystick && !_pts.Utility.JoystickEnabled)
                    _pts.Utility.InitializeJoystick();

                if (!_pts.Configure.EnableJoystick && _pts.Utility.JoystickEnabled)
                    _pts.Utility.StopJoystickTread();
            }





            // while (_logining)
            while (false)
            {
                if (_pts.ReadyState == ReadyState.Ready)
                {
                    _pts.OnLoadFailure -= PTSOnLoadFailure;

                    //if (_pts.Server.ProductNo != "00003")
                    //{
                    //    TopMostMessageBox.Show(Localization["Application_ServerIsNotPTS"].Replace("%1", _pts.Credential.Domain), Localization["MessageBox_Error"],
                    //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return false;
                    //}

                    if (_pts.NVR.NVRs.Count == 0)
                    {
                        ConvertTempDeviceToNVRDevice();
                        InitialTimer();

                        return ;
                    }

                    _nvrsReady = false;

                    foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
                    {
                        if (obj.Value.ValidateCredential())
                        {
                            LoginNVR(obj.Value);
                        }
                        else
                        {
                            obj.Value.Initialize();
                            if (obj.Value.Utility == null)
                            {
                                TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                                return ;
                            }
                            obj.Value.ReadyState = ReadyState.Unavailable;
                        }
                    }

                    if (!_pts.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New)))
                    {
                        ConvertTempDeviceToNVRDevice();
                        InitialTimer();

                        return ;
                    }

                    break;
                }

                Application.RaiseIdle(null);
                Thread.Sleep(250);
            }

            String progress = "";
            while (_logining)
            {
                if (_nvrsReady)
                {
                    ConvertTempDeviceToNVRDevice();
                    InitialTimer();
                    return ;
                }

                foreach (KeyValuePair<UInt16, INVR> obj in _pts.NVR.NVRs)
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



        }

    }
}
