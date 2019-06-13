using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace App_FailoverSystem
{
    public partial class FailoverSystem
    {
        private Boolean _nvrsReady;
        private Boolean _logining = true;
        public override Boolean Login()
        {
            InitializePanel();

            _fos.OnLoadFailure -= FOSOnLoadFailure;
            _fos.OnLoadFailure += FOSOnLoadFailure;
            _fos.Initialize();
            _fos.Login();

            while (_logining)
            {
                if (_fos.ReadyState == ReadyState.Ready)
                {
                    _fos.OnLoadFailure -= FOSOnLoadFailure;

                    if (_fos.Server.ProductNo != "00009")
                    {
                        TopMostMessageBox.Show(Localization["Application_ServerIsNotFOS"].Replace("%1", _fos.Credential.Domain), Localization["MessageBox_Error"],
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    InitialTimer();
                    if (_fos.NVR.NVRs.Count == 0)
                        return true;

                    _nvrsReady = false;

                    foreach (KeyValuePair<UInt16, INVR> obj in _fos.NVR.NVRs)
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

                    if (!_fos.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New)))
                        return true;

                    break;
                }
                Application.RaiseIdle(null);
                Thread.Sleep(200);
            }

            String progress = "";
            while (_logining)
            {
                if (_nvrsReady)
                    return true;

                foreach (KeyValuePair<UInt16, INVR> obj in _fos.NVR.NVRs)
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

        private void ValidateCredential(INVR nvr)
        {
            nvr.OnLoadFailure -= NvrOnLoadFailure;
            nvr.OnLoadFailure += NvrOnLoadFailure;
            nvr.OnLoadComplete -= NvrOnLoadComplete;
            nvr.OnLoadComplete += NvrOnLoadComplete;

            nvr.Initialize();
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

                //failover dont support linux nvr server
                // && nvr.Server.ProductNo != "00005"
                if ((nvr.Server.ProductNo != "00001" && nvr.Server.ProductNo != "00004") && nvr.ReadyState == ReadyState.Ready)
                    nvr.ReadyState = ReadyState.Unavailable;
            }

            if (_fos.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
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

            if (_fos.NVR.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
                return;

            _nvrsReady = true;
        }

        private void FOSOnLoadFailure(Object sender, EventArgs<String> e)
        {
            _logining = false;
            _fos.OnLoadFailure -= FOSOnLoadFailure;

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

            _fos.OnSaveFailure -= NvrOnSaveFailure;
            _fos.OnSaveFailure += NvrOnSaveFailure;
            _fos.OnSaveComplete -= FOSOnSaveComplete;
            _fos.OnSaveComplete += FOSOnSaveComplete;

            _fos.Save();
        }

        private void FOSOnSaveComplete(Object sender, EventArgs<String> e)
        {
            _fos.OnSaveFailure -= NvrOnSaveFailure;
            _fos.OnSaveComplete -= FOSOnSaveComplete;

            _watch.Stop();

            //----------------------------------------------------------------------
            //save nvr's device list before logout(<- maybe)
            _nvrsReady = true;

            var nvrs = new Dictionary<UInt16, INVR>();
            foreach (KeyValuePair<UInt16, INVR> obj in _fos.NVR.NVRs)
            {
                if (obj.Value.ReadyState == ReadyState.ReSync)
                {
                    _nvrsReady = false;
                    nvrs.Add(obj.Key, obj.Value);
                    ValidateCredential(obj.Value);
                }
            }

            while (!_nvrsReady)
            {
                Thread.Sleep(250);
            }

            ShowUnavailableNVRMessage();
            if (nvrs.Count > 0)
            {
                foreach (var nvr in nvrs)
                    _fos.NVR.UpdateFailoverDeviceList(nvr.Key, nvr.Value);
            }

            //when devices is saved, last thing is save nvr list
            _fos.NVR.SaveNVRDocument();
            //----------------------------------------------------------------------
            String msg = Localization["Application_SaveCompleted"];
            //+@" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (_fos.Server.ReadyStatus == ManagerReadyState.Unavailable)
            {
                TopMostMessageBox.Show(Localization["Application_CannotConnect"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_fos.Server.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_PortChange"].Replace("%1", _fos.Server.Port.ToString()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (_fos.User.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_UserChange"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowUnavailableNVRMessage()
        {
            var unavailableNVRs = new List<INVR>();

            foreach (KeyValuePair<UInt16, INVR> obj in _fos.NVR.NVRs)
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
                    if (list.Count > 10)
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
            _fos.OnSaveFailure -= NvrOnSaveFailure;
            _fos.OnSaveComplete -= FOSOnSaveComplete;

            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


        private String _nvrLoginProgress;
        public override String LoginProgress
        {
            get { return _nvrLoginProgress ?? _fos.LoginProgress; }
        }
    }
}
