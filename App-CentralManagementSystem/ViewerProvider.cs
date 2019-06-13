using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace App_CentralManagementSystem
{
    public partial class CentralManagementSystem
    {
        private readonly List<IViewer> _recyclingViewer = new List<IViewer>();
        private readonly Queue<IViewer> _storedViewer = new Queue<IViewer>();
        private readonly List<IViewer> _usingViewer = new List<IViewer>();

        private IViewer _fullScreenViewer;


        public override CustomStreamSetting CustomStreamSetting
        {
            get { return CMS.Configure.CustomStreamSetting; }
        }

        public override int AudioOutChannelCount
        {
            get { return CMS.Utility.AudioOutChannelCount; }
        }


        public override void RegistViewer(ushort count)
        {
            for (ushort i = 0; i < count; i++)
            {
                try
                {
                    IViewer viewer = new CMSViewer.CMSViewer
                    {
                        App = this,
                        StretchToFit = true,
                        DisplayTitleBar = false,
                        AudioIn = false
                    };

                    viewer.SetVisible(false);
                    viewer.OnFullScreen += ViewerOnFullScreen;

                    _storedViewer.Enqueue(viewer);
                }
                catch (Exception)
                {
                    TopMostMessageBox.Show(@"Viewer Initialize Error", @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
        }

        public override IViewer RegistViewer()
        {
            IViewer viewer;
            if (_storedViewer.Count > 0)
            {
                viewer = _storedViewer.Dequeue();
            }
            else
            {
                viewer = new CMSViewer.CMSViewer
                {
                    App = this,
                    StretchToFit = true,
                    DisplayTitleBar = false,
                    AudioIn = false
                };

                viewer.SetVisible(false);
                viewer.OnFullScreen += ViewerOnFullScreen;
            }

            _usingViewer.Add(viewer);

            //ShowStatus();

            return viewer;
        }

        protected void ViewerOnFullScreen(object sender, EventArgs e)
        {
            if (_fullScreenViewer != null && _fullScreenViewer != sender)
                _fullScreenViewer.CloseFullScreen();

            if (sender is IViewer)
                _fullScreenViewer = (IViewer)sender;
        }

        public override void UnregistViewer(IViewer viewer)
        {
            _usingViewer.Remove(viewer);

            _recyclingViewer.Add(viewer);

            viewer.Parent = null;
            viewer.SetVisible(false);
            viewer.Active = false;
            viewer.StretchToFit = true;
            viewer.Dewarp = false;
            viewer.DisplayTitleBar = false;
            viewer.AudioIn = false;
            viewer.AutoDropFrame();
            viewer.EnableMotionDetection(false);
            viewer.Host = "";

            if (viewer.NetworkStatus != NetworkStatus.Idle)
            {
                viewer.OnDisconnect += ViewerOnRecycle;
                viewer.Stop();
                viewer.Camera = null;
            }
            else
            {
                viewer.Stop();
                if (!_storedViewer.Contains(viewer))
                    _storedViewer.Enqueue(viewer);
                _recyclingViewer.Remove(viewer);
            }

            //ShowStatus();
        }

        protected virtual void ViewerOnRecycle(object sender, EventArgs<int> e)
        {
            var viewer = sender as IViewer;
            if (viewer != null)
            {
                viewer.OnDisconnect -= ViewerOnRecycle;
                if (!_storedViewer.Contains(viewer))
                    _storedViewer.Enqueue(viewer);
                _recyclingViewer.Remove(viewer);
            }

            //ShowStatus();
        }
    }
}