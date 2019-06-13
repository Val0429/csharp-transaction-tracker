using System;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        protected virtual void ControlOnMouseKeyDoubleClick(Object sender, AxNvrViewerLib._INvrViewerEvents_OnMouseKeyDoubleClickEvent e)
        {
            if (e.nBtn != 1) return;

            ViewFullScreen();
        }

        private void ControlOnMouseKeyDown(Object sender, AxNvrViewerLib._INvrViewerEvents_OnMouseKeyDownEvent e)
        {
            RaiseMouseKeyDown(e.nX, e.nY, e.nBtn);
        }
    }
}
