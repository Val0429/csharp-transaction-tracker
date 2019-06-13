using System;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        private UInt16 _previousFullScreenStreamId;
        public String FullScreenPTZRegion;
        private void ViewFullScreen()
        {
            if (_control.IsFullScreen() != 0) return;

            if (OnFullScreen != null)
                OnFullScreen(this, null);

            //layout dont have to switch high resolution after full screen
            if (_deviceLayout == null && _subLayout == null)
            {
                var highProfileStreamId = 1;
                var currentStreamId = ProfileId;

                if (currentStreamId == 0 && Camera.Profile != null)
                    currentStreamId = Camera.Profile.StreamId;

                if (Camera.Profile != null)
                    highProfileStreamId = Camera.Profile.HighProfile;

                _previousFullScreenStreamId = 0;
                //switch to high profile
                if (currentStreamId != highProfileStreamId)
                {
                    _previousFullScreenStreamId = currentStreamId;
                    ProfileId = 1;
                    FullScreenPTZRegion = GetDigitalPtzRegion();
                    Reconnect();
                }
                /*var id = ((ProfileId == 0) ? ((Camera.Profile != null) ? Camera.Profile.StreamId : 1) : ProfileId);
                _previousFullScreenStreamId = 0;
                if (id != 1) //high video stream
                {
                    _previousFullScreenStreamId = Convert.ToUInt16(id);
                    ProfileId = 1;
                    Reconnect();
                }*/
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
                FullScreenPTZRegion = GetDigitalPtzRegion();
                Reconnect();
            }

            if (OnCloseFullScreen != null)
                OnCloseFullScreen(this, new EventArgs<String>(_timecode.ToString()));
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
