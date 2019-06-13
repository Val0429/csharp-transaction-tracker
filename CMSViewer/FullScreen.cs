using System;
using System.Globalization;
using Interface;

namespace CMSViewer
{
    public partial class CMSViewer
    {
        private UInt16 _previousFullScreenStreamId;
        private void ViewFullScreen()
        {
            if (_control.IsFullScreen() != 0) return;

            if (OnFullScreen != null)
                OnFullScreen(this, null);

            //layout dont have to switch high resolution after full screen

            var id = ((ProfileId == 0) ? ((Camera.Profile != null) ? Camera.Profile.StreamId : 1) : ProfileId);
            _previousFullScreenStreamId = 0;
            if (id != 1) //high video stream
            {
                _previousFullScreenStreamId = Convert.ToUInt16(id);
                ProfileId = 1;
                Reconnect();
            }

            BeforeFullScreen();

            _control.EnableFullScreen();
        }

        // For Plugin Use
        protected virtual void BeforeFullScreen()
        {
        }

        private void ControlOnCloseFullScreen(Object sender, EventArgs e)
        {
            AfterCloseFullScreen();

            if (_previousFullScreenStreamId != 0) //restore stream id
            {
                ProfileId = _previousFullScreenStreamId;
                _previousFullScreenStreamId = 0;
                Reconnect();
            }

            if (OnCloseFullScreen != null)
                OnCloseFullScreen(this, new EventArgs<String>(Timecode.ToString(CultureInfo.InvariantCulture)));
        }

        protected virtual void AfterCloseFullScreen()
        {
            // For Plugin Use
        }

        public void CloseFullScreen()
        {
            _control.CloseFullScreenWindow();
        }
    }
}
