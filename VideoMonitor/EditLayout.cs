using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
	public partial class VideoMonitor
	{
		private Point _locationBeforeDrag;
		//private Size _sizeBeforeDrag;
		private Point _dragStartPoint;

		protected void VideoWindowOnWindowMouseDrag(Object sender, MouseEventArgs e)
		{
			if (ActivateVideoWindow == null) return;

			_dragStartPoint = new Point(e.X, e.Y);
			_locationBeforeDrag = new Point(ActivateVideoWindow.Location.X, ActivateVideoWindow.Location.Y);

			//_sizeBeforeDrag = new Size(ActivateVideoWindow.Width, ActivateVideoWindow.Height);
			//ActivateVideoWindow.Size = new Size(320, 240);

			windowsPanel.MouseUp += PanelMouseUp;
			windowsPanel.MouseMove += PanelMouseMove;
			ActivateVideoWindow.BringToFront();

			if (!windowsPanel.Capture)
				windowsPanel.Capture = true;
		}

		private void PanelMouseMove(Object sender, MouseEventArgs e)
		{
			ActivateVideoWindow.Location = new Point(e.X - _dragStartPoint.X, e.Y - _dragStartPoint.Y);
		}

		private void PanelMouseUp(Object sender, MouseEventArgs e)
		{
			windowsPanel.MouseUp -= PanelMouseUp;
			windowsPanel.MouseMove -= PanelMouseMove;

			ActivateVideoWindow.Location = _locationBeforeDrag;
			//ActivateVideoWindow.Size = _sizeBeforeDrag;

			Point point = Cursor.Position;
			var window = DropOnWindow(point);

			if (window != null && ActivateVideoWindow != window)
			{
				if (ActivateVideoWindow.Camera != null || window.Camera != null)
				{
					var index1 = VideoWindows.IndexOf(ActivateVideoWindow);
					var index2 = VideoWindows.IndexOf(window);

					while(FocusGroup.View.Count <= index1)
						FocusGroup.View.Add(null);

					FocusGroup.View[index1] = window.Camera;

					while (FocusGroup.View.Count <= index2)
						FocusGroup.View.Add(null);

					FocusGroup.View[index2] = ActivateVideoWindow.Camera;

					while (FocusGroup.View.Count > 0 && FocusGroup.View[FocusGroup.View.Count - 1] == null)
					{
						FocusGroup.View.RemoveAt(FocusGroup.View.Count - 1);
					}

					Devices.Clear();
					for (Int32 i = 0; i < FocusGroup.View.Count; i++)
					{
						Devices.Add(i, FocusGroup.View[i]);
						if (Devices.Count >= MaxConnection)
							break;
					}
					//-------------------------------

					var device = ActivateVideoWindow.Camera;
					ActivateVideoWindow.Camera = window.Camera;
					window.Camera = device;

					//-------------------------------
					ActivateVideoWindow.Active = false;
					ActivateVideoWindow = window;
					ActivateVideoWindow.Active = true;

					Server.GroupModify(FocusGroup);
					RaiseOnContentChange(ToArray());

					if (OnViewingDeviceNumberChange != null)
					{
						var deviceCount = (Devices == null) ? 0 : Devices.Count;
						var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

						foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
						{
							deviceCount += popupVideoMonitor.DeviceCount;
							layoutCount += popupVideoMonitor.LayoutCount;
						}

						var count = Math.Min(layoutCount, deviceCount);
						OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
					}

					if (OnViewingDeviceListChange != null)
					{
                        OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
					}
				}
			}
		}

		private IVideoWindow DropOnWindow(Point point)
		{
			foreach (IVideoWindow videoWindow in VideoWindows)
			{
				//Check visible window for drop
				if (!videoWindow.Visible) continue;

				if (Drag.IsDrop(videoWindow as Control, point))
				{
					return videoWindow;
				}
			}

			return null;
		}

		public void DeleteDeviceView(Object sender, EventArgs e)
		{
			if (FocusGroup == null) return;
			if (FocusGroup.Id == 0) return; //can't delete all device group

			if (Server.Device.Groups.Values.ToArray().Contains(FocusGroup) && !Server.User.Current.Group.IsFullAccessToDevices)
			{
				TopMostMessageBox.Show(Localization["VideoMonitor_NoPermissionToDelete"], Localization["MessageBox_Information"],
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			DialogResult result = TopMostMessageBox.Show(Localization["VideoMonitor_DeleteGroupConfirm"].Replace("%1", FocusGroup.ToString()), Localization["MessageBox_Confirm"],
												  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result != DialogResult.Yes) return;

			App.DeleteUserDefineDeviceGroup(FocusGroup);

			ClearAll();
		}
	}
}
