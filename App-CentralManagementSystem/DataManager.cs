using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PanelBase;
using ApplicationForms = App.ApplicationForms;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem
    {
        private Boolean _nvrsReady;
        private Boolean _logining = true;
        public override Boolean Login()
        {
            base.Login();

            InitializePanel();

            CMS.OnLoadFailure -= CmsOnLoadFailure;
            CMS.OnLoadFailure += CmsOnLoadFailure;
            CMS.Initialize();
            if (CMS.Utility == null)
            {
                TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelAutoLogin();
                Application.Exit();
                return false;
            }
            CMS.Login();

            while (_logining)
            {
                if (CMS.ReadyState == ReadyState.Ready)
                {
                    CMS.OnLoadFailure -= CmsOnLoadFailure;
                    
                    if (CMS.Server.ProductNo != "00003")
                    {
                        TopMostMessageBox.Show(Localization["Application_ServerIsNotCMS"].Replace("%1", CMS.Credential.Domain).Replace("%2", CMS.Credential.Port.ToString()), Localization["MessageBox_Error"],
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        CancelAutoLogin();
                        Application.Exit();
                        return false;
                    }

                    if (CMS.NVRManager.NVRs.Count == 0)
                    {
                        ConvertTempDeviceToNVRDevice();
                        InitialTimer();
                        return true;
                    }

                    _nvrsReady = false;

                    //foreach (KeyValuePair<UInt16, INVR> obj in CMS.NVRManager.NVRs)
                    //{
                    //    if (obj.Value.ValidateCredential())
                    //    {
                    //        LoginNVR(obj.Value);
                    //    }
                    //    else
                    //    {
                    //        obj.Value.Initialize();
                    //        if (obj.Value.Utility == null)
                    //        {
                    //            TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                    //                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            Application.Exit();
                    //            return false;
                    //        }
                    //        obj.Value.ReadyState = ReadyState.Unavailable;
                    //    }
                    //}
                    
                    _nvrsReady = true;
                    if (!CMS.NVRManager.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New)))
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

                foreach (KeyValuePair<UInt16, INVR> obj in CMS.NVRManager.NVRs)
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
            RemoveNoPermissionNVR();

            //Convert temp device to nvr's device
            if (CMS.Device.Groups.Count == 0) return;

            INVRManager nvrManager = CMS.NVRManager;
            var nvrs = CMS.NVRManager.NVRs;

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in CMS.Device.Groups)
            {
                IDeviceGroup deviceGroup = obj.Value;
                
                var devices = new List<IDevice>(deviceGroup.Items);
                deviceGroup.Items.Clear();
                
                foreach (IDevice device in devices.Where(d => nvrs.ContainsKey(d.Server.Id)))
                {
                    var nvrId = device.Server.Id;
                    var nvr = nvrManager.FindNVRById(nvrId);
                    if (nvr.ReadyState == ReadyState.Ready && nvr.Device.Devices.ContainsKey(device.Id))
                    {
                        deviceGroup.Items.Add(nvr.Device.FindDeviceById(device.Id));
                    }
                }

                if (deviceGroup.Items.Count > 0)
                    deviceGroup.Items.Sort((x, y) => (x.Id - y.Id));

                //----------------

                devices = new List<IDevice>(deviceGroup.View);
                deviceGroup.View.Clear();
                foreach (IDevice device in devices)
                {
                    if (device == null)
                    {
                        deviceGroup.View.Add(null);
                        continue;
                    }

                    if (nvrs.ContainsKey(device.Server.Id))
                    {
                        if (nvrs[device.Server.Id].ReadyState == ReadyState.Ready && nvrs[device.Server.Id].Device.Devices.ContainsKey(device.Id))
                            deviceGroup.View.Add(nvrs[device.Server.Id].Device.Devices[device.Id]);
                    }
                }
            }

            //-------------------

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in CMS.Device.Groups)
            {
                IDeviceGroup deviceGroup = obj.Value;
                while (deviceGroup.View.Count > 0 && deviceGroup.View[deviceGroup.View.Count - 1] == null)
                {
                    deviceGroup.View.RemoveAt(deviceGroup.View.Count - 1);
                }
            }

            // ================

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in CMS.User.Current.DeviceGroups)
            {
                IDeviceGroup deviceGroup = obj.Value;

                var devices = new List<IDevice>(deviceGroup.Items);
                deviceGroup.Items.Clear();

                foreach (IDevice device in devices.Where(d => nvrs.ContainsKey(d.Server.Id)))
                {
                    var nvrId = device.Server.Id;
                    var nvr = nvrManager.FindNVRById(nvrId);
                    if (nvr.ReadyState == ReadyState.Ready && nvr.Device.Devices.ContainsKey(device.Id))
                    {
                        deviceGroup.Items.Add(nvr.Device.FindDeviceById(device.Id));
                    }
                }

                if (deviceGroup.Items.Count > 0)
                    deviceGroup.Items.Sort((x, y) => (x.Id - y.Id));

                //----------------

                devices = new List<IDevice>(deviceGroup.View);
                deviceGroup.View.Clear();
                foreach (IDevice device in devices)
                {
                    if (device == null)
                    {
                        deviceGroup.View.Add(null);
                        continue;
                    }

                    if (nvrs.ContainsKey(device.Server.Id))
                    {
                        if (nvrs[device.Server.Id].ReadyState == ReadyState.Ready && nvrs[device.Server.Id].Device.Devices.ContainsKey(device.Id))
                            deviceGroup.View.Add(nvrs[device.Server.Id].Device.Devices[device.Id]);
                    }
                }
            }

            //-------------------

            foreach (KeyValuePair<UInt16, IDeviceGroup> obj in CMS.User.Current.DeviceGroups)
            {
                IDeviceGroup deviceGroup = obj.Value;
                while (deviceGroup.View.Count > 0 && deviceGroup.View[deviceGroup.View.Count - 1] == null)
                {
                    deviceGroup.View.RemoveAt(deviceGroup.View.Count - 1);
                }
            }
        }

        private void RemoveNoPermissionNVR()
        {
            if (CMS.User.Current.Group.IsFullAccessToDevices) return;

            INVR[] nvrs = (from obj in CMS.NVRManager.NVRs where !CMS.User.Current.CheckPermission(obj.Value, Permission.Access) select obj.Value).ToArray();

            foreach (INVR nvr in nvrs)
            {
                CMS.NVRManager.NVRs.Remove(nvr.Id);
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

                if ((nvr.Server.ProductNo != "00001" && nvr.Server.ProductNo != "00004" && nvr.Server.ProductNo != "00005") && nvr.ReadyState == ReadyState.Ready)
                    nvr.ReadyState = ReadyState.Unavailable;
            }

            if (CMS.NVRManager.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
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
                nvr.ReadyState = ReadyState.Unavailable;
            }

            if (CMS.NVRManager.NVRs.Any(obj => (obj.Value.ReadyState == ReadyState.New || obj.Value.ReadyState == ReadyState.ReSync)))
                return;

            _nvrsReady = true;
        }

        private void CmsOnLoadFailure(Object sender, EventArgs<String> e)
        {
            _logining = false;
            CMS.OnLoadFailure -= CmsOnLoadFailure;

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

            CMS.OnSaveFailure -= CmsOnSaveFailure;
            CMS.OnSaveFailure += CmsOnSaveFailure;
            CMS.OnSaveComplete -= CmsOnSaveComplete;
            CMS.OnSaveComplete += CmsOnSaveComplete;

            CMS.Save();
        }

        public override void Undo()
        {
            CMS.OnLoadComplete -= CmsOnLoadComplete;
            CMS.OnLoadComplete += CmsOnLoadComplete;

            //undo have change to failure, need to handle.
            CMS.OnLoadFailure -= CmsOnLoadComplete;
            CMS.OnLoadFailure += CmsOnLoadComplete;

            CMS.UndoReload();
        }

        private void CmsOnLoadComplete(Object sender, EventArgs<String> e)
        {
            CMS.OnLoadComplete -= CmsOnLoadComplete;
            CMS.OnLoadFailure -= CmsOnLoadComplete;

            //refresh listen device status
            CMS.Utility.UpdateEventRecive();

            PanelBase.ApplicationForms.HideProgressBar();
            //ApplicationForms.HideProgressBar();
        }

        private void CmsOnSaveComplete(Object sender, EventArgs<String> e)
        {
            CMS.OnSaveFailure -= CmsOnSaveFailure;
            CMS.OnSaveComplete -= CmsOnSaveComplete;

            _watch.Stop();
            String msg = Localization["Application_SaveCompleted"];
                //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (CMS.Server.ReadyStatus == ManagerReadyState.Unavailable)
            {
                TopMostMessageBox.Show(Localization["Application_CannotConnect"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Logout();

                OpenAnotherProcessAfterLogout = true;
                return;
            }

            if (CMS.Server.ReadyStatus == ManagerReadyState.MajorModify)
            {
                if (CMS.Server.IsPortChange)
                {
                    msg += Environment.NewLine +
                           Localization["Application_PortChange"].Replace("%1", CMS.Server.Port.ToString());
                }

                if (CMS.Server.IsSSLPortChange)
                {
                    msg += Environment.NewLine +
                           Localization["Application_SSLPortChange"].Replace("%1", CMS.Server.SSLPort.ToString());
                }

                TopMostMessageBox.Show(msg,
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logout();

                OpenAnotherProcessAfterLogout = true;
                return;
            }

            if (CMS.User.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_UserChange"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logout();

                OpenAnotherProcessAfterLogout = true;
                return;
            }

            if (CMS.NVRManager.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_NVRChange"],
                                       Localization["MessageBox_Information"], MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            _nvrsReady = true;
            var resyncCount = 0;
            //foreach (KeyValuePair<UInt16, INVR> obj in CMS.NVRManager.NVRs)
            //{
            //    if (obj.Value.ReadyState == ReadyState.ReSync)
            //    {
            //        resyncCount++;
            //        _nvrsReady = false;
            //        LoginNVR(obj.Value);
            //        CMS.NVRModify(obj.Value);
            //    }
            //}

            while (!_nvrsReady)
            {
                Thread.Sleep(250);
            }

            //ShowUnavailableNVRMessage();
            if (resyncCount > 0)
            {
                ConvertTempDeviceToNVRDevice();
                CMS.ListenNVREvent(null);
            }

            //if (IntPtr.Size == 4) //only 32bit support joystick
            {
                if (CMS.Configure.EnableJoystick && !CMS.Utility.JoystickEnabled)
                    CMS.Utility.InitializeJoystick();

                if (!CMS.Configure.EnableJoystick && CMS.Utility.JoystickEnabled)
                    CMS.Utility.StopJoystickTread();
            }

            TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowUnavailableNVRMessage()
        {
            var unavailableNVRs = CMS.NVRManager.NVRs.Values.Where(n => n.ReadyState != ReadyState.Ready)
                                                     .OrderBy(n => n.Id)
                                                     .ToList();

            if (unavailableNVRs.Count > 0)
            {
                var list = new List<String>();

                foreach (INVR nvr in unavailableNVRs)
                {
                    nvr.Device.Devices.Clear();
                    nvr.Device.Groups.Clear();
                    list.Add(nvr + " " + nvr.Credential.Domain);
                    if(list.Count >= 10)
                    {
                        list.Add("...");
                        break;
                    }
                }
                TopMostMessageBox.Show(Localization["Application_SomeNVRUnavailable"] + Environment.NewLine + String.Join(Environment.NewLine, list.ToArray()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CmsOnSaveFailure(Object sender, EventArgs<String> e)
        {
            CMS.OnSaveFailure -= CmsOnSaveFailure;
            CMS.OnSaveComplete -= CmsOnSaveComplete;

            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private String _nvrLoginProgress;
        public override String LoginProgress
        {
            get { return _nvrLoginProgress ?? CMS.LoginProgress; }
        }
    }
}
