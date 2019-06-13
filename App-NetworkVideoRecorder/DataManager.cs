using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using ApplicationForms = App.ApplicationForms;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
    {
        private Boolean _logining = true;
        public override Boolean Login()
        {
            base.Login();

            InitializePanel();

            Nvr.OnLoadFailure -= NvrOnLoadFailure;
            Nvr.OnLoadFailure += NvrOnLoadFailure;
            Nvr.Initialize();
            if (Nvr.Utility == null)
            {
                TopMostMessageBox.Show(Localization["Application_UtilityInitError"], Localization["MessageBox_Error"],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelAutoLogin();
                Application.Exit();
                return false;
            }
            Nvr.Login();

            while (_logining)
            {
                if (Nvr.ReadyState == ReadyState.Ready)
                {
                    Nvr.OnLoadFailure -= NvrOnLoadFailure;

                    if (Nvr.Server.ProductNo != "00001" && Nvr.Server.ProductNo != "00004" && Nvr.Server.ProductNo != "00005" && Nvr.Server.ProductNo != "00121" && Nvr.Server.ProductNo != "00122") //121, 122 linux
                    {
                        TopMostMessageBox.Show(Localization["Application_ServerIsNotNVR"].Replace("%1", Nvr.Credential.Domain), Localization["MessageBox_Error"],
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }


                    Nvr.WriteOperationLog(String.Format("{0} Login from {1} at {2}",
                        Nvr.Credential.UserName, GetIPAddress() ,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));

                    InitialTimer();
                    return true;
                }
                Application.RaiseIdle(null);
                Thread.Sleep(200);
            }

            return false;
        }

        private String GetIPAddress()
        {
            String ipaddress = "";

            var ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            var addr = ipEntry.AddressList;

            foreach (IPAddress t in addr)
            {
                if (t.AddressFamily != AddressFamily.InterNetwork) continue;
                ipaddress = t.ToString();
                break;
            }
            return ipaddress;
        }

        private void NvrOnLoadFailure(Object sender, EventArgs<String> e)
        {
            _logining = false;
            Nvr.OnLoadFailure -= NvrOnLoadFailure;

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

            Nvr.OnSaveFailure -= NvrOnSaveFailure;
            Nvr.OnSaveFailure += NvrOnSaveFailure;
            Nvr.OnSaveComplete -= NvrOnSaveComplete;
            Nvr.OnSaveComplete += NvrOnSaveComplete;

            Nvr.Save();
        }

        public override void Undo()
        {
            Nvr.OnLoadComplete -= NvrOnLoadComplete;
            Nvr.OnLoadComplete += NvrOnLoadComplete;

            //undo have change to failure, need to handle.
            Nvr.OnLoadFailure -= NvrOnLoadComplete;
            Nvr.OnLoadFailure += NvrOnLoadComplete;

            Nvr.UndoReload();
        }

        private void NvrOnLoadComplete(Object sender, EventArgs<String> e)
        {
            Nvr.OnLoadComplete -= NvrOnLoadComplete;
            Nvr.OnLoadFailure -= NvrOnLoadComplete;

            //refresh listen device status
            Nvr.Utility.UpdateEventRecive();

            //Undo 
            PanelBase.ApplicationForms.HideProgressBar();
            //ApplicationForms.HideProgressBar();
        }

        private void NvrOnSaveComplete(Object sender, EventArgs<String> e)
        {
            Nvr.OnSaveFailure -= NvrOnSaveFailure;
            Nvr.OnSaveComplete -= NvrOnSaveComplete;

            _watch.Stop();
            String msg = Localization["Application_SaveCompleted"];
            //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (Nvr.Server.ReadyStatus == ManagerReadyState.Unavailable)
            {
                TopMostMessageBox.Show(Localization["Application_CannotConnect"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (Nvr.Server.ReadyStatus == ManagerReadyState.MajorModify)
            {
                if (Nvr.Server.IsPortChange && Nvr.Server.Port > 0)
                {
                    msg += Environment.NewLine + Environment.NewLine +
                           Localization["Application_PortChange"].Replace("%1", Nvr.Server.Port.ToString());
                }

                if (Nvr.Server.IsSSLPortChange && Nvr.Server.SSLPort > 0)
                {
                    msg += Environment.NewLine + Environment.NewLine +
                           Localization["Application_SSLPortChange"].Replace("%1", Nvr.Server.SSLPort.ToString());
                }

                TopMostMessageBox.Show(msg,
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            if (Nvr.User.ReadyStatus == ManagerReadyState.MajorModify)
            {
                TopMostMessageBox.Show(msg + Environment.NewLine + Localization["Application_UserChange"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                OpenAnotherProcessAfterLogout = true;
                Logout();
                return;
            }

            _storagePanel.Invalidate();

            Nvr.Utility.UpdateEventRecive();

            //if (IntPtr.Size == 4) //only 32bit support joystick
            {
                if (Nvr.Configure.EnableJoystick && !Nvr.Utility.JoystickEnabled)
                    Nvr.Utility.InitializeJoystick();

                if (!Nvr.Configure.EnableJoystick && Nvr.Utility.JoystickEnabled)
                    Nvr.Utility.StopJoystickTread();
            }

            TopMostMessageBox.Show(msg, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NvrOnSaveFailure(Object sender, EventArgs<String> e)
        {
            Nvr.OnSaveFailure -= NvrOnSaveFailure;
            Nvr.OnSaveComplete -= NvrOnSaveComplete;

            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);

            _storagePanel.Invalidate();

            Nvr.Utility.UpdateEventRecive();
        }

        public override String LoginProgress
        {
            get { return Nvr.LoginProgress; }
        }
    }
}
