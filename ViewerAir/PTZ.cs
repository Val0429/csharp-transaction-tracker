using System;
using Constant;
using DeviceConstant;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        protected PTZMode _ptzMode = PTZMode.None;
        public PTZMode PtzMode
        {
            get
            {
                if (_control == null) return PTZMode.None;

                switch (_control.GetPTZMode())
                {
                    case 1:
                        return PTZMode.Digital;

                    case 2:
                        return PTZMode.Optical;

                    default:
                        return _ptzMode;
                }
            }
            set
            {
                //I try to avoid same job, but look like will miss judgment, don't care if ptz status is the same, just change it.
                //if (value == PtzMode) return;
                var mode = _control.GetPTZMode();
                if ((value == PTZMode.Digital && mode == 1) || (value == PTZMode.Optical && mode == 2))
                    return;

                //use another way 
                switch (value)
                {
                    case PTZMode.Digital:
                        if (_control.GetPTZMode() == 1)
                            return;
                        break;

                    case PTZMode.Optical:
                        if (_control.GetPTZMode() == 2)
                            return;
                        break;
                }

                if (Camera == null) return;
                
                var status = NetworkStatus;
                if (_control != null && (status == NetworkStatus.Connected || status == NetworkStatus.Streaming || status == NetworkStatus.Idle))
                {
                    switch (value)
                    {
                        case PTZMode.Digital:
                            _control.DisableMousePTZ();
                            _control.EnableMouseDigitalPTZ();
                            break;

                        case PTZMode.Optical:
                            if (Camera.Model != null && Camera.Model.IsSupportPTZ)
                            {
                                _control.DisableMouseDigitalPTZ();
                                _control.EnableMousePTZ();
                            }
                            break;

                        default:
                            _control.DisableMousePTZ();
                            _control.DisableMouseDigitalPTZ();
                            break;
                    }
                }
                _ptzMode = value;
            }
        }

        private String _previousCmd;
        public void SendPTZCommand(String cmd)
        {
            //Console.WriteLine(cmd);
            if (String.Equals(_previousCmd, cmd)) return;

            //Console.WriteLine(cmd);
            try
            {
                _control.SendPTZCommand(cmd);
            }
            catch (Exception ex)
            {
                //Trace.WriteLine(ex.Message);
            }

            _previousCmd = cmd;
        }
    }
}
