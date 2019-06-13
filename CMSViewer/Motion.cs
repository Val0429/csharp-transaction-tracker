using System;
using DeviceConstant;
using Interface;

namespace CMSViewer
{
    public partial class CMSViewer
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

        public void SetSubLayoutRegion(ISubLayout subLayout)
        {
        }

        public void UpdateSubLayoutRegion(ISubLayout subLayout)
        {
        }

    }
}
