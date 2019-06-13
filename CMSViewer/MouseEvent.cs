using System;

namespace CMSViewer
{
    public partial class CMSViewer
    {
        protected virtual void ControlOnMouseKeyDoubleClick(Object sender,  AxiCMSViewerLib._IiCMSViewerEvents_OnMouseKeyDoubleClickEvent e)
        {
            if (e.nBtn != 1) return;

            ViewFullScreen();
        }

        private void ControlOnMouseKeyDown(Object sender,  AxiCMSViewerLib._IiCMSViewerEvents_OnMouseKeyDownEvent e)
        {
            RaiseMouseKeyDown(e.nX, e.nY, e.nBtn);
        }
    }
}
