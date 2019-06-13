using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace VideoMonitor
{
    public partial class VideoMenu
    {
        protected Button _do1Button;
        protected Button _do2Button;
        protected Button _do3Button;
        protected Button _do4Button;
        protected Button _do5Button;
        protected Button _do6Button;
        protected Button _do7Button;
        protected Button _do8Button;

        private static readonly Image _digitaloutput1 = Resources.GetResources(Properties.Resources.digital_output1, Properties.Resources.IMGDigital_output1);
        private static readonly Image _digitaloutput1Activate = Resources.GetResources(Properties.Resources.digital_output1_activate, Properties.Resources.IMGDigital_output1_activate);
        private static readonly Image _digitaloutput2 = Resources.GetResources(Properties.Resources.digital_output2, Properties.Resources.IMGDigital_output2);
        private static readonly Image _digitaloutput2Activate = Resources.GetResources(Properties.Resources.digital_output2_activate, Properties.Resources.IMGDigital_output2_activate);
        private static readonly Image _digitaloutput3 = Resources.GetResources(Properties.Resources.digital_output3, Properties.Resources.IMGDigital_output3);
        private static readonly Image _digitaloutput3Activate = Resources.GetResources(Properties.Resources.digital_output3_activate, Properties.Resources.IMGDigital_output3_activate);
        private static readonly Image _digitaloutput4 = Resources.GetResources(Properties.Resources.digital_output4, Properties.Resources.IMGDigital_output4);
        private static readonly Image _digitaloutput4Activate = Resources.GetResources(Properties.Resources.digital_output4_activate, Properties.Resources.IMGDigital_output4_activate);
        private static readonly Image _digitaloutput5 = Resources.GetResources(Properties.Resources.digital_output5, Properties.Resources.IMGDigital_output5);
        private static readonly Image _digitaloutput5Activate = Resources.GetResources(Properties.Resources.digital_output5_activate, Properties.Resources.IMGDigital_output5_activate);
        private static readonly Image _digitaloutput6 = Resources.GetResources(Properties.Resources.digital_output6, Properties.Resources.IMGDigital_output6);
        private static readonly Image _digitaloutput6Activate = Resources.GetResources(Properties.Resources.digital_output6_activate, Properties.Resources.IMGDigital_output6_activate);
        private static readonly Image _digitaloutput7 = Resources.GetResources(Properties.Resources.digital_output7, Properties.Resources.IMGDigital_output7);
        private static readonly Image _digitaloutput7Activate = Resources.GetResources(Properties.Resources.digital_output7_activate, Properties.Resources.IMGDigital_output7_activate);
        private static readonly Image _digitaloutput8 = Resources.GetResources(Properties.Resources.digital_output8, Properties.Resources.IMGDigital_output8);
        private static readonly Image _digitaloutput8Activate = Resources.GetResources(Properties.Resources.digital_output8_activate, Properties.Resources.IMGDigital_output8_activate);

        private delegate void DigitalOutputDelegate();
        private delegate void DigitalOutputIdDelegate(UInt16 id);

        protected virtual void DoMouseClick(Button button, UInt16 id, Image activate, Image normal)
        {
            if (VideoWindow == null) return;
            if (VideoWindow.Camera.Model == null) return;

            if ((String.Equals(button.Tag.ToString(), "Inactivate")))
            {
                button.Tag = "Activate";
                button.BackgroundImage = activate;

                if (VideoWindow.Camera != null)
                {
                    VideoWindow.Camera.DigitalOutputStatus[id] = true;

                    Server.WriteOperationLog("Device %1 Digital Output %2 On"
                        .Replace("%1", VideoWindow.Camera.Id.ToString()).Replace("%2", id.ToString()));
                }
                else
                    return;
            }
            else
            {
                button.Tag = "Inactivate";
                button.BackgroundImage = normal;

                if (VideoWindow.Camera != null)
                {
                    VideoWindow.Camera.DigitalOutputStatus[id] = false;
                    Server.WriteOperationLog("Device %1 Digital Output %2 Off"
                        .Replace("%1", VideoWindow.Camera.Id.ToString()).Replace("%2", id.ToString()));
                }
                else
                    return;
            }

            if (VideoWindow.Camera.Model.Manufacture == "ACTi")
            {
                DigitalOutputDelegate digitalOutputDelegate = VideoWindow.Camera.DigitalOutput;
                digitalOutputDelegate.BeginInvoke(null, null);
            }
            else
            {
                DigitalOutputIdDelegate digitalOutputDelegate = VideoWindow.Camera.DigitalOutput;
                digitalOutputDelegate.BeginInvoke(id, null, null);
            }
        }

        protected void Do1MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do1Button, 1, _digitaloutput1Activate, _digitaloutput1);
        }

        protected void Do2MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do2Button, 2, _digitaloutput2Activate, _digitaloutput2);
        }

        protected void Do3MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do3Button, 3, _digitaloutput3Activate, _digitaloutput3);
        }

        protected void Do4MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do4Button, 4, _digitaloutput4Activate, _digitaloutput4);
        }

        protected void Do5MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do5Button, 5, _digitaloutput5Activate, _digitaloutput5);
        }

        protected void Do6MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do6Button, 6, _digitaloutput6Activate, _digitaloutput6);
        }

        protected void Do7MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do7Button, 7, _digitaloutput7Activate, _digitaloutput7);
        }

        protected void Do8MouseClick(Object sender, MouseEventArgs e)
        {
            DoMouseClick(_do8Button, 8, _digitaloutput8Activate, _digitaloutput8);
        }

        protected void UpdateDo1Button()
        {
            if (_do1Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do1Button);
                return;
            }

            var hasDO1 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(1) && VideoWindow.Camera.IOPort[1] == IOPort.Output)
                hasDO1 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 1)
                hasDO1 = true;

            if (hasDO1)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[1])
                {
                    _do1Button.BackgroundImage = _digitaloutput1Activate;
                    _do1Button.Tag = "Activate";
                }
                else
                {
                    _do1Button.BackgroundImage = _digitaloutput1;
                    _do1Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do1Button);
                Controls.Add(_do1Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do1Button);
                //_do1Button.BackgroundImage = _digitaloutput1;
                //_do1Button.Tag = "Inactivate";
            }
        }

        protected void UpdateDo2Button()
        {
            if (_do2Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do2Button);
                return;
            }

            var hasDO2 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(2) && VideoWindow.Camera.IOPort[2] == IOPort.Output)
                hasDO2 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 2)
                hasDO2 = true;

            if (hasDO2)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[2])
                {
                    _do2Button.BackgroundImage = _digitaloutput2Activate;
                    _do2Button.Tag = "Activate";
                }
                else
                {
                    _do2Button.BackgroundImage = _digitaloutput2;
                    _do2Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do2Button);
                Controls.Add(_do2Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do2Button);
            }
        }

        protected void UpdateDo3Button()
        {
            if (_do3Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do3Button);
                return;
            }

            var hasDO3 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(3) && VideoWindow.Camera.IOPort[3] == IOPort.Output)
                hasDO3 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 3)
                hasDO3 = true;

            if (hasDO3)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[3])
                {
                    _do3Button.BackgroundImage = _digitaloutput3Activate;
                    _do3Button.Tag = "Activate";
                }
                else
                {
                    _do3Button.BackgroundImage = _digitaloutput3;
                    _do3Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do3Button);
                Controls.Add(_do3Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do3Button);
            }
        }

        protected void UpdateDo4Button()
        {
            if (_do4Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do4Button);
                return;
            }

            var hasDO4 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(4) && VideoWindow.Camera.IOPort[4] == IOPort.Output)
                hasDO4 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 4)
                hasDO4 = true;

            if (hasDO4)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[4])
                {
                    _do4Button.BackgroundImage = _digitaloutput4Activate;
                    _do4Button.Tag = "Activate";
                }
                else
                {
                    _do4Button.BackgroundImage = _digitaloutput4;
                    _do4Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do4Button);
                Controls.Add(_do4Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do4Button);
            }
        }

        protected void UpdateDo5Button()
        {
            if (_do5Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do5Button);
                return;
            }

            var hasDO5 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(5) && VideoWindow.Camera.IOPort[5] == IOPort.Output)
                hasDO5 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 5)
                hasDO5 = true;

            if (hasDO5)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[5])
                {
                    _do5Button.BackgroundImage = _digitaloutput5Activate;
                    _do5Button.Tag = "Activate";
                }
                else
                {
                    _do5Button.BackgroundImage = _digitaloutput5;
                    _do5Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do5Button);
                Controls.Add(_do5Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do5Button);
            }
        }

        protected void UpdateDo6Button()
        {
            if (_do6Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do6Button);
                return;
            }

            var hasDO6 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(6) && VideoWindow.Camera.IOPort[6] == IOPort.Output)
                hasDO6 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 6)
                hasDO6 = true;

            if (hasDO6)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[6])
                {
                    _do6Button.BackgroundImage = _digitaloutput6Activate;
                    _do6Button.Tag = "Activate";
                }
                else
                {
                    _do6Button.BackgroundImage = _digitaloutput6;
                    _do6Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do6Button);
                Controls.Add(_do6Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do6Button);
            }
        }

        protected void UpdateDo7Button()
        {
            if (_do7Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do7Button);
                return;
            }

            var hasDO7 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(7) && VideoWindow.Camera.IOPort[7] == IOPort.Output)
                hasDO7 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 7)
                hasDO7 = true;

            if (hasDO7)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[7])
                {
                    _do7Button.BackgroundImage = _digitaloutput7Activate;
                    _do7Button.Tag = "Activate";
                }
                else
                {
                    _do7Button.BackgroundImage = _digitaloutput7;
                    _do7Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do7Button);
                Controls.Add(_do7Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do7Button);
            }
        }

        protected void UpdateDo8Button()
        {
            if (_do8Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_do8Button);
                return;
            }

            var hasDO8 = false;
            if (VideoWindow.Camera.IOPort.ContainsKey(8) && VideoWindow.Camera.IOPort[8] == IOPort.Output)
                hasDO8 = true;

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfDo >= 8)
                hasDO8 = true;

            if (hasDO8)
            {
                if (VideoWindow.Camera.DigitalOutputStatus[8])
                {
                    _do8Button.BackgroundImage = _digitaloutput8Activate;
                    _do8Button.Tag = "Activate";
                }
                else
                {
                    _do8Button.BackgroundImage = _digitaloutput8;
                    _do8Button.Tag = "Inactivate";
                }
                SetButtonPosition(_do8Button);
                Controls.Add(_do8Button);
                _count++;
            }
            else
            {
                Controls.Remove(_do8Button);
            }
        }

    }
}