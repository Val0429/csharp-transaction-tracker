using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ViewerAir;

namespace App_NetworkVideoRecorder
{
    public partial class NetworkVideoRecorder
    {
        /// <summary>
        /// Available viewer
        /// </summary>
        protected readonly Queue<IViewer> _storedViewer = new Queue<IViewer>();
        /// <summary>
        /// Occupid invisible viewer
        /// </summary>
        protected readonly List<IViewer> _recyclingViewer = new List<IViewer>();
        /// <summary>
        /// Occupid viewer
        /// </summary>
        protected readonly List<IViewer> _usingViewer = new List<IViewer>();


        public override CustomStreamSetting CustomStreamSetting
        {
            get
            {
                return Nvr.Configure.CustomStreamSetting;
            }
        }

        public override Int32 AudioOutChannelCount { get { return Nvr.Utility.AudioOutChannelCount; } }

        public override void RegistViewer(UInt16 count)
        {
            for (UInt16 i = 0; i < count; i++)
            {
                try
                {
                    IViewer viewer = new VideoPlayer
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
                    TopMostMessageBox.Show(@"Viewer Initialize Error", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
        }

        [Conditional("DEBUG")]
        protected void ShowStatus()
        {
            if (Debugger.IsAttached)
                Console.WriteLine(_storedViewer.Count + "\t\t" + _recyclingViewer.Count + "\t\t" + _usingViewer.Count);
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
                viewer = new VideoPlayer
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

            ShowStatus();

            return viewer;
        }

        private IViewer _fullScreenViewer;
        protected void ViewerOnFullScreen(Object sender, EventArgs e)
        {
            if (_fullScreenViewer != null && _fullScreenViewer != sender)
            {
                _fullScreenViewer.CloseFullScreen();
            }

            var viewer = sender as IViewer;
            if (viewer != null)
            {
                _fullScreenViewer = viewer;
            }
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

            if (viewer.NetworkStatus == NetworkStatus.Idle)
            {
                viewer.Stop();
                viewer.Camera = null;

                _recyclingViewer.Remove(viewer);
                
                if (!_storedViewer.Contains(viewer))
                {
                    _storedViewer.Enqueue(viewer);
                }
            }
            else
            {
                viewer.OnDisconnect += ViewerOnRecycle;
                viewer.Stop();
                viewer.Camera = null;
            }

            ShowStatus();
        }

        protected virtual void ViewerOnRecycle(Object sender, EventArgs<Int32> e)
        {
            var viewer = sender as IViewer;
            if (viewer != null)
            {
                viewer.OnDisconnect -= ViewerOnRecycle;
                
                _recyclingViewer.Remove(viewer);

                if (!_storedViewer.Contains(viewer))
                {
                    _storedViewer.Enqueue(viewer);
                }
            }

            ShowStatus();
        }
    }
}
