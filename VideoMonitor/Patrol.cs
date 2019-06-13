using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;

namespace VideoMonitor
{
	public partial class VideoMonitor
	{
		public static List<PopupVideoMonitor> UsingPatrolVideoMonitor = new List<PopupVideoMonitor>();
		private readonly System.Timers.Timer _patrolDeviceTimer = new System.Timers.Timer();
		internal static Boolean _isPatrol = false;
		private UInt16 _amountOfMonitor;
		
        public void EnableMultiScreenPatrol(Object sender, EventArgs<String> e)
		{
			if (_isPatrol) return;
			_isPatrol = true;

			var inputForm = new AmountOfVideoMonitorForm
			{
				MaximumAmount =(UInt16) (MaximumPopupVideoMonitor - GetVideoMonitorCount + 1),
			};

			var result = inputForm.ShowDialog();

			if (result != DialogResult.OK) return;

			_patrolDeviceTimer.Interval = Server.Configure.PatrolInterval * 1000;
			_patrolDeviceTimer.Enabled = false;
			_patrolDeviceTimer.Enabled = true;

			_amountOfMonitor = inputForm.SelectAmount;
			switch (e.Value)
			{
				//patrol device list
				case "DeviceList":
					//把所有會顯示出來的device group都先計算出來產生好 放在 list 裡，之後只要依序取出來顯示即可
					CreateDeviceListPatrolContent();
					break;

				//patrol device view
				case "DeviceView":
					CreateDeviceViewPatrolContent();
					break;
			}

			DeviceMultiScreenPatrol();
		}

		private List<IDeviceGroup> _patrolDeviceGroupList = new List<IDeviceGroup>();
		private IDeviceGroup _currentDeviceGroup;
		private void CreateDeviceListPatrolContent()
		{
			_currentDeviceGroup = null;
			_patrolDeviceGroupList.Clear();

			if (CMS != null)
			{
				foreach (KeyValuePair<UInt16, INVR> obj in CMS.NVRManager.NVRs)
				{
					INVR nvr = obj.Value;
					if (!nvr.IsPatrolInclude) continue;

					var list = nvr.Device.Devices.Values.OrderBy(d => d.Id).ToList();
					ConvertListToDeviceGroup(list);
				}
			}
			else
			{
				var list = NVR.Device.Devices.Values.OrderBy(d => d.Id).ToList();
				ConvertListToDeviceGroup(list);
			}
		}

		private void ConvertListToDeviceGroup(IEnumerable<IDevice> list)
		{ 
			IDeviceGroup deviceGroup = null;
			int count = 0;
			int layoutCount = WindowLayout.Count;
			foreach (IDevice device in list)
			{
				if (count == 0 || deviceGroup == null)
				{
					deviceGroup = new DeviceGroup { Layout = new List<WindowLayout>(WindowLayout) };
				}
				deviceGroup.View.Add(device);

				if (device != null && !deviceGroup.Items.Contains(device))
					deviceGroup.Items.Add(device);

				count++;

				if (count == layoutCount)
				{
					_patrolDeviceGroupList.Add(deviceGroup);
					count = 0;
					deviceGroup = null;
				}
			}

			if (deviceGroup != null)
			{
				_patrolDeviceGroupList.Add(deviceGroup);
			}
		}

		private void CreateDeviceViewPatrolContent()
		{
			_currentDeviceGroup = null;
			_patrolDeviceGroupList.Clear();

			if (CMS != null)
			{
                ConvertNVRToDeviceGroup(CMS);
			}
			else
			{
				ConvertNVRToDeviceGroup(NVR);
			}
		}

		private void ConvertNVRToDeviceGroup(IServer nvr)
		{
			var deviceGroups = new List<IDeviceGroup>();
			//public
			var groups = nvr.Device.Groups.Values.OrderBy(group => group.Id);

			foreach (var group in groups)
			{
				//dont show all device group
				if (group.Id == 0) continue;

				if (group.Items.Count > 0)
					deviceGroups.Add(group);
			}

			groups = Server.User.Current.DeviceGroups.Values.OrderBy(group => group.Id);

			foreach (var group in groups)
			{
				if (group.Items.Count > 0)
					deviceGroups.Add(group);
			}

			IDeviceGroup deviceGroup = null;
			foreach(IDeviceGroup group in deviceGroups)
			{
				int count = 0;
				List<WindowLayout> layout;
				if (group.Layout != null && group.Layout.Count > 0 && group.Layout.Count <= MaxConnection)
				{
					layout = group.Layout;
				}
				else
				{
					layout = WindowLayouts.LayoutGenerate((UInt16)Math.Min(group.Items.Count, MaxConnection));
				}

				int layoutCount = layout.Count;

				foreach (IDevice device in group.View)
				{
					if (count == 0 || deviceGroup == null)
					{
						deviceGroup = new DeviceGroup { Layout = new List<WindowLayout>(layout), Name = group .Name};
					}
					deviceGroup.View.Add(device);

					if (device != null && !deviceGroup.Items.Contains(device))
						deviceGroup.Items.Add(device);

					count++;

					if (count == layoutCount)
					{
						_patrolDeviceGroupList.Add(deviceGroup);
						count = 0;
						deviceGroup = null;
					}
				}

				if (deviceGroup != null)
				{
					_patrolDeviceGroupList.Add(deviceGroup);
				}
				//---
			}
		}

		private void DeviceMultiScreenPatrol(Object sender, EventArgs e)
		{
			DeviceMultiScreenPatrol();
		}

		private void DeviceMultiScreenPatrol()
		{
			//沒有東西可以patrol
			if (_patrolDeviceGroupList.Count == 0) return;

			//是否啟用patrol
			if (!_patrolDeviceTimer.Enabled) return;

			var newVideoMonitors = new List<PopupVideoMonitor>();
			var usedVideoMonitors = new List<PopupVideoMonitor>();

			//將每一個螢幕所要顯示的內容組合成device group然後發送過去顯示
			for (var i = 1; i <= _amountOfMonitor; i++)
			{
				IDeviceGroup deviceGroup = null;
				if (_currentDeviceGroup == null)
					deviceGroup = _patrolDeviceGroupList[0];
				else
				{
					//抓下一個group 
					var index = _patrolDeviceGroupList.IndexOf(_currentDeviceGroup) + 1;
					if (_patrolDeviceGroupList.Count <= index)
						index = 0;

					deviceGroup = _patrolDeviceGroupList[index];
				}
				_currentDeviceGroup = deviceGroup;
				//---------------------------------------------------------------
				//顯示在本體
				if (i == 1)
				{
					//---停止觸發 content Change 否則device tree會觸發停止patrol
					_stopTriggerOnContentChangeEvent = true;

					ClearAll();
					ShowGroup(deviceGroup);
					//--恢復觸發 content change
					_stopTriggerOnContentChangeEvent = false;
				}
				else//顯示在彈出視窗
				{
					//取出正在使用中的
					if (UsingPatrolVideoMonitor.Count >= i - 1)
					{
						PopupVideoMonitor videoMonitor = UsingPatrolVideoMonitor[i - 2];

						videoMonitor.Reset();
						videoMonitor.DeviceGroup = deviceGroup;

						usedVideoMonitors.Add(videoMonitor);
					}
					else//彈出新的來使用(第一次)
					{
						PopupVideoMonitor videoMonitor = PopupVideoMonitor(deviceGroup);
						videoMonitor.DisableCloseButton();

						if (!UsingPatrolVideoMonitor.Contains(videoMonitor))
							UsingPatrolVideoMonitor.Add(videoMonitor);

						newVideoMonitors.Add(videoMonitor);
					}
				}
			}

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

			foreach (PopupVideoMonitor popupVideoMonitor in newVideoMonitors)
			{
				popupVideoMonitor.Show();
				popupVideoMonitor.BringToFront();
			}

			foreach (PopupVideoMonitor popupVideoMonitor in usedVideoMonitors)
			{
				popupVideoMonitor.Initialize();
				popupVideoMonitor.Play();
			}

            AutoChangeProfileMode();
		}

		public void DisableMultiScreenPatrol(Object sender, EventArgs<String> e)
		{
			if (!_isPatrol) return;
			_isPatrol = false;

			StopMultiScreenPatrol();
		}

		private void StopMultiScreenPatrol()
		{
			if (!_patrolDeviceTimer.Enabled) return;

			_patrolDeviceTimer.Enabled = false;

			ClosePatrolVideoMonitor();
			//清掉正在看的畫面
			ClearAll();
		}

		private void ClosePatrolVideoMonitor()
		{
			foreach(PopupVideoMonitor videoMonitor  in UsingPatrolVideoMonitor)
			{
				videoMonitor.Close();
			}

			UsingPatrolVideoMonitor.Clear();
		}
	}
}
