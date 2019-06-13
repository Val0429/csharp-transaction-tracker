using System;
using DeviceConstant;

namespace ViewerAir
{
    public partial class VideoPlayer
    {

        public void SetupMotionStart()
        {
            PtzMode = PTZMode.Disable;
            //_control.EnableMotionDetection(1);
            _control.SetupMotionStart();
        }

        public String GetMotionRegion()
        {
            String str = _control.SetupMotionEnd();

            _control.SetupMotionStart();

            return str;
        }

        public void SetupMotionEnd()
        {
            //_control.SetupMotionEnd();
            _control.EnableMotionDetection(0);
        }

        public void EnableMotionDetection(Boolean enable)
        {
            if (enable)
            {
                _ptzMode = PTZMode.None;
                _control.EnableMotionDetection(1);
            }
            else
            {
                _control.EnableMotionDetection(0);
            }
        }

    }
}
