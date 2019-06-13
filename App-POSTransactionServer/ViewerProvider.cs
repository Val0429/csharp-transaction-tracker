using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace App_POSTransactionServer
{
	public partial class POSTransactionServer
	{
		public override CustomStreamSetting CustomStreamSetting
		{
			get
			{
				return _pts.Configure.CustomStreamSetting;
			}
		}

		public override Int32 AudioOutChannelCount { get { return _pts.Utility.AudioOutChannelCount; } }

		private  readonly Queue<IViewer> _storedViewer = new Queue<IViewer>();
		private  readonly List<IViewer> _recyclingViewer = new List<IViewer>();
		private  readonly List<IViewer> _usingViewer = new List<IViewer>();

		public override void RegistViewer(UInt16 count)
		{
			for (UInt16 i = 0; i < count; i++)
			{
				try
				{
					IViewer viewer;
					switch (_pts.ReleaseBrand)
					{
						case "Salient":
							viewer = new ViewerSalient.VideoPlayer();
							break;

						default:
							viewer = new ViewerAir.VideoPlayer();
							break;
					}

					viewer.App = this;
					viewer.StretchToFit = true;
					viewer.Dewarp = false;
					viewer.DisplayTitleBar = false;
					viewer.AudioIn = false;

					viewer.SetVisible(false);
					viewer.OnFullScreen += ViewerOnFullScreen;

					_storedViewer.Enqueue(viewer);
				}
				catch(Exception)
				{
					TopMostMessageBox.Show(@"Viewer Initialize Error", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Application.Exit();
					return;
				}
			}
		}

		public override IViewer RegistViewer()
		{
			if (_storedViewer.Count == 0)
			{
				RegistViewer(1);
			}

			IViewer viewer = _storedViewer.Dequeue();

			_usingViewer.Add(viewer);

			//ShowStatus();

			return viewer;
		}
		
		private  IViewer _fullScreenViewer;
		protected void ViewerOnFullScreen(Object sender, EventArgs e)
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
			viewer.Camera = null;
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
		
		protected  void ViewerOnRecycle(Object sender, EventArgs<Int32> e)
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

		public  void ClearAllViewer()
		{
			try
			{
				_usingViewer.Clear();
				_recyclingViewer.Clear();
				_storedViewer.Clear();
			}
			catch(Exception)
			{
			}
		}
	}
}
