using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Constant;
using DeviceCab;
using DeviceConstant;
using PanelBase;

namespace SetupDevice
{
	public sealed partial class ResolutionRegionControl : UserControl
	{
		public Dictionary<String, String> Localization;
		public EditPanel EditPanel;

		public UInt16 StreamId;

		private static readonly Image _gridBg = Resources.GetResources(Properties.Resources.grid, Properties.Resources.IMGGrid);
		private BackgroundWorker _getSnapshotBackgroundWorker;
		public ResolutionRegionControl()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"DevicePanel_ResolutionRegionConfig", "Resolution Region Config"},
								   {"DevicePanel_RefreshSnapshot", "Refresh snapshot"},
								   {"DevicePanel_SaveSettingToGetSnapshot", "Save setting to get snapshot"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Top;
			
			snapshotPanel.BackgroundImage = _gridBg;

			snapshotPanel.MouseDown += SnapshotPanelMouseDown;
			snapshotPanel.MouseMove += SnapshotPanelMouseMove;
			snapshotPanel.MouseUp += SnapshotPanelMouseUp;

			containerPanel.Paint += ContainerPanelPaint;
			Paint += ResolutionRegionControlPaint;
			snapshotPanel.Paint += SnapshotPanelPaint;

			snapshotPanel.SizeChanged += SnapshotPanelSizeChanged;

			_getSnapshotBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
			_getSnapshotBackgroundWorker.DoWork += GetSnapshot;
			_getSnapshotBackgroundWorker.RunWorkerCompleted += GetSnapshotCompleted;
		}

		private Size _regionSize = new Size(100, 100);
		private Point _regionLocation = new Point(0, 0);
		private readonly Pen _borderPen = new Pen(Color.FromArgb(51, 153, 255), 2) { DashStyle = DashStyle.Dash };
		private readonly SolidBrush _fillBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255));

		private void SnapshotPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			g.FillRectangle(_fillBrush, _regionLocation.X, _regionLocation.Y, _regionSize.Width, _regionSize.Height);
			g.DrawRectangle(_borderPen, _regionLocation.X + 1, _regionLocation.Y + 1, _regionSize.Width - 2, _regionSize.Height - 2);
		}

		private Point _panelMouseDownLocation;
		private void SnapshotPanelMouseMove(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var x = e.X + _panelMouseDownLocation.X;
			var y = e.Y + _panelMouseDownLocation.Y;
			_regionLocation.X = Math.Min(Math.Max(0, x), snapshotPanel.Width - _regionSize.Width);
			_regionLocation.Y = Math.Min(Math.Max(0, y), snapshotPanel.Height - _regionSize.Height);

			snapshotPanel.Invalidate();
		}

		private void SnapshotPanelMouseDown(Object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_panelMouseDownLocation = new Point(_regionLocation.X - e.X, _regionLocation.Y - e.Y);
		}

		private void SnapshotPanelMouseUp(Object sender, MouseEventArgs e)
		{
			if (EditPanel.Camera == null) return;
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;
			if (_maximumResolution == Resolution.NA) return;
			if(snapshotPanel.Width == 0 || snapshotPanel.Height == 0) return;

			var width = Resolutions.ToWidth(_maximumResolution);
			var height = Resolutions.ToHeight(_maximumResolution);
			
			var streamConfig = EditPanel.Camera.Profile.StreamConfigs[StreamId];
			streamConfig.RegionStartPointX = Convert.ToInt32(((width * 1.0) / (snapshotPanel.Width * 1.0)) * (_regionLocation.X * 1.0));
			streamConfig.RegionStartPointY = Convert.ToInt32(((height * 1.0) / (snapshotPanel.Height * 1.0)) * (_regionLocation.Y * 1.0));
			
			EditPanel.CameraIsModify();
		}

		private Boolean _refreshSnapshot;
		private Resolution _maximumResolution = Resolution.NA;
		public void ParseDevice()
		{
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			var streamConfig = EditPanel.Camera.Profile.StreamConfigs[StreamId];
			if(!(EditPanel.Camera.Model is ArecontVisionCameraModel))
			{
				_maximumResolution = Resolution.NA;
				return;
			}
			var model = (ArecontVisionCameraModel) EditPanel.Camera.Model;

			_maximumResolution = model.GetMaximumResilotion();
			if (_maximumResolution == Resolution.NA) return;

			var width = Resolutions.ToWidth(_maximumResolution);
			var height = Resolutions.ToHeight(_maximumResolution);

			snapshotPanel.BackgroundImage = _gridBg;
			if (EditPanel.Camera.ReadyState == ReadyState.Ready || EditPanel.Camera.ReadyState == ReadyState.Modify)
			{
				if (_getSnapshotBackgroundWorker.IsBusy)
				{
					//stop complete event
					_getSnapshotBackgroundWorker.RunWorkerCompleted -= GetSnapshotCompleted;
					_getSnapshotBackgroundWorker.CancelAsync();
					_getSnapshotBackgroundWorker.Dispose();
					_getSnapshotBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
					_getSnapshotBackgroundWorker.DoWork += GetSnapshot;
					_getSnapshotBackgroundWorker.RunWorkerCompleted += GetSnapshotCompleted;
				}
				_refreshSnapshot = true;
				_getSnapshotBackgroundWorker.RunWorkerAsync(
					new GetSnapshotArgument
						{
							Size = new Size(width, height),
							ChannelId = streamConfig.Channel
						});
			}
			
			UpdateResolutionRegion();
			Invalidate();
		}

		private Image _snapshot;
		public void GetSnapshot(Object sender, DoWorkEventArgs e)
		{
			var argument = e.Argument as GetSnapshotArgument;
			if (argument == null)
			{
				return;
			}

			_snapshot = EditPanel.Camera.GetFullResolutionSnapshot(argument.Size, argument.ChannelId);
		}

		private void GetSnapshotCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			snapshotPanel.BackgroundImage = _snapshot ?? _gridBg;
			_refreshSnapshot = false;
			Invalidate();
		}

		public void UpdateResolutionRegion()
		{
			if (_maximumResolution == Resolution.NA) return;
			if (snapshotPanel.Width == 0 || snapshotPanel.Height == 0) return;
			if (!EditPanel.Camera.Profile.StreamConfigs.ContainsKey(StreamId)) return;

			_regionSize.Width = Math.Min(_regionSize.Width, snapshotPanel.Width);
			_regionSize.Height = Math.Min(_regionSize.Height, snapshotPanel.Height);

			var width = Resolutions.ToWidth(_maximumResolution);
			var height = Resolutions.ToHeight(_maximumResolution);

			var streamConfig = EditPanel.Camera.Profile.StreamConfigs[StreamId];
			var resolution = streamConfig.Resolution;
			var sizeWidth = Resolutions.ToWidth(resolution);
			var sizeHeight = Resolutions.ToHeight(resolution);

			var widthPercent = (width * 1.0) / (snapshotPanel.Width * 1.0);
			var heightPercent = (height * 1.0) / (snapshotPanel.Height * 1.0);

			_regionSize = new Size
			{
				Width = Convert.ToInt32((sizeWidth * 1.0) / widthPercent),
				Height = Convert.ToInt32((sizeHeight * 1.0) / heightPercent),
			};
			if (resolution == _maximumResolution)
			{
				_regionLocation = new Point(0, 0);
				streamConfig.RegionStartPointX = 0;
				streamConfig.RegionStartPointY = 0;
			}
			else
			{
				Int32 x = Convert.ToInt32((streamConfig.RegionStartPointX * 1.0) / widthPercent);
				Int32 y = Convert.ToInt32((streamConfig.RegionStartPointY * 1.0) / heightPercent);
				//make sure range stay in snapshot

				x = Math.Min(Math.Max(0, x), snapshotPanel.Width - _regionSize.Width);
				y = Math.Min(Math.Max(0, y), snapshotPanel.Height - _regionSize.Height);

				_regionLocation.X = x;
				streamConfig.RegionStartPointX = Convert.ToInt32(widthPercent * (_regionLocation.X * 1.0));

				_regionLocation.Y = y;
				streamConfig.RegionStartPointY = Convert.ToInt32(heightPercent * (_regionLocation.Y * 1.0));
			}

			snapshotPanel.Invalidate();
		}

		private void SnapshotPanelSizeChanged(Object sender, EventArgs e)
		{
			UpdateResolutionRegion();
		}

		private void ResolutionRegionControlPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			var title = Localization["DevicePanel_ResolutionRegionConfig"] + " " + StreamId;
			
			if (EditPanel.Camera.ReadyState == ReadyState.New)
				title += " (" + Localization["DevicePanel_SaveSettingToGetSnapshot"] + ")";

			if (_refreshSnapshot)
				title += " (" + Localization["DevicePanel_RefreshSnapshot"] + ")";

            g.DrawString(title, SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
		}

		private void ContainerPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            SetupBase.Manager.PaintSingleInput(g, containerPanel);
		}
	}

	public class GetSnapshotArgument
	{
		public Size Size;
		public UInt16 ChannelId;
	}
}
