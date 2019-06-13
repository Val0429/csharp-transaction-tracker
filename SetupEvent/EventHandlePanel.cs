using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
	public class EventHandlePanel : Panel
	{
		public event EventHandler OnEventEditClick;
		protected virtual void FireOnEventEditClick(object sender, EventArgs e)
		{
			if (OnEventEditClick != null)
				OnEventEditClick(sender, e);
		}

		public event EventHandler OnHandleChange;

		public IServer Server;
		public ICamera Camera;
		private readonly Queue<HandlePanel> _recycleHandle = new Queue<HandlePanel>();
		private EventPanel _eventPanel;

		public EventCondition EventCondition
		{
			get
			{
				return _eventPanel.EventCondition;
			}
			set
			{
				_eventPanel.EventCondition = value;
			}
		}

		public List<EventHandle> EventHandles;

		public EventHandlePanel()
		{
			Dock = DockStyle.Top;
			Padding = new Padding(0, 0, 0, 15);
			DoubleBuffered = true;
			AutoSize = true;
			MinimumSize = new Size(0, 15);
		}

		public void Initialize()
		{
			_eventPanel = new EventPanel();

			_eventPanel.OnEventEditClick += EditButtonMouseClick;
			_eventPanel.OnIntervalEdit += EventPanelIntervalEdit;
		}

		public void ShowHandles()
		{
			_isEditing = false;
			if (EventHandles != null && EventHandles.Count > 0)
			{
				foreach (var eventHandle in EventHandles)
				{
					var handlePanel = GetHandlePanel();
					handlePanel.EventHandle = eventHandle;
					handlePanel.SettingEnable = true;
					Controls.Add(handlePanel);
					handlePanel.BringToFront();
				}

				var handleTitlePanel = GetHandlePanel();
				handleTitlePanel.IsTitle = true;
				handleTitlePanel.Cursor = Cursors.Default;
				handleTitlePanel.OnSelectAll += HandlePanelOnSelectAll;
				handleTitlePanel.OnSelectNone += HandlePanelOnSelectNone;
				Controls.Add(handleTitlePanel);

				_eventPanel.IntervalVisible = (Server is IFOS) ? false : true;
			}
			else
				_eventPanel.IntervalVisible = false;

			_eventPanel.EditVisible = true;
			Controls.Add(_eventPanel);
			_isEditing = true;
		}

		private HandlePanel GetHandlePanel()
		{
			HandlePanel handlePanel;
			if (_recycleHandle.Count > 0)
			{
				handlePanel = _recycleHandle.Dequeue();
			}
			else
			{
				handlePanel = new HandlePanel
				{
					Server = Server,
					SelectionVisible = false,
				};
				handlePanel.Initialize();

				handlePanel.OnSelectChange += HandlePanelOnSelectChange;
				handlePanel.OnHandleChange += HandlePanelOnHandleChange;
			}

			handlePanel.Camera = Camera;
			handlePanel.Cursor = Cursors.Default;
			handlePanel.SelectedColor = Manager.DeleteTextColor;
			return handlePanel;
		}

		public Boolean EditVisible
		{
			set
			{
				_eventPanel.EditVisible = value;
				if (!value)
					_eventPanel.IntervalVisible = false;
			}
		}

		private Boolean _isEditing;
		private void HandlePanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as HandlePanel;
			if (panel == null) return;

			if (panel.EventHandle == null) return;

			var selectAll = false;
			if (panel.Checked)
			{
				//Add Handle
				if (_isEditing)
				{
					EventHandles.Add(panel.EventHandle);
					panel.EventHandle.ReadyState = ReadyState.New;
					panel.SettingEnable = true;
				}

				selectAll = true;
				foreach (Control control in Controls)
				{
					if (control is EventPanel) continue;

					var handlePanel = control as HandlePanel;
					if (handlePanel == null || handlePanel.IsTitle) continue;
					if (!handlePanel.Checked && handlePanel.CheckBoxEnabled)
					{
						selectAll = false;
						break;
					}
				}
			}
			else
			{
				//Remove Handle
				if (_isEditing)
				{
					EventHandles.Remove(panel.EventHandle);
					panel.EventHandle.ReadyState = ReadyState.NotInUse;
					panel.SettingEnable = false;
				}
			}

			var title = ((Controls.Contains(_eventPanel))
				? ((Controls.Count > 2) ? Controls[Controls.Count - 2] : null)
				: Controls[Controls.Count - 1]) as HandlePanel;
			
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= HandlePanelOnSelectAll;
				title.OnSelectNone -= HandlePanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += HandlePanelOnSelectAll;
				title.OnSelectNone += HandlePanelOnSelectNone;
			}

			if (!_isEditing) return;

			if (Camera == null || Camera.EventHandling == null) return;

			Camera.EventHandling.ReadyState = ReadyState.Modify;

			if (OnHandleChange != null)
				OnHandleChange(this, null);
		}

		private void HandlePanelOnHandleChange(Object sender, EventArgs e)
		{
			if (Camera == null || Camera.EventHandling == null) return;

			Camera.EventHandling.ReadyState = ReadyState.Modify;

			if (OnHandleChange != null)
				OnHandleChange(this, null);
		}

		private void EventPanelIntervalEdit(Object sender, EventArgs e)
		{
			if (Camera == null || Camera.EventHandling == null) return;

			Camera.EventHandling.ReadyState = ReadyState.Modify;

			if (OnHandleChange != null)
				OnHandleChange(this, null);
		}

		protected virtual void EditButtonMouseClick(Object sender, EventArgs e)
		{
			if (EventCondition == null) return;

			if (OnEventEditClick != null)
				OnEventEditClick(this, e);
		}

		public void ClearViewModel()
		{
			_isEditing = false;
			_eventPanel.EventCondition = null;
			foreach (Control control in Controls)
			{
				if (control is HandlePanel)
				{
					HandlePanel handlePanel = control as HandlePanel;
					
					handlePanel.Camera = null;
					handlePanel.SelectionVisible = false;

					handlePanel.OnSelectChange -= HandlePanelOnSelectChange;
					handlePanel.Checked = false;
					handlePanel.EventHandle = null;
					handlePanel.OnSelectChange += HandlePanelOnSelectChange;

					if (handlePanel.IsTitle)
					{
						handlePanel.OnSelectAll -= HandlePanelOnSelectAll;
						handlePanel.OnSelectNone -= HandlePanelOnSelectNone;
						handlePanel.IsTitle = false;
					}

					if (!_recycleHandle.Contains(handlePanel))
						_recycleHandle.Enqueue(handlePanel);
				}
				else if (control is EventPanel)
				{
					((EventPanel)control).EventCondition = null;
				}
			}

			Controls.Clear();
			_isEditing = true;
		}

		public Boolean SelectionVisible
		{
			set
			{
				_isEditing = false;
				foreach (Control control in Controls)
				{
					if (control is EventPanel) continue;

					((HandlePanel)control).SelectionVisible = value;
				}
			}
		}

		public Boolean SettingEnable
		{
			set
			{
				foreach (Control control in Controls)
				{
					if (control is EventPanel) continue;

					((HandlePanel)control).SettingEnable = value;
				}
			}
		}

		protected HandlePanel GetNewAddHandlePanel(EventHandle handle)
		{
			HandlePanel handlePanel = GetHandlePanel();
			handlePanel.EventHandle = handle;
			handlePanel.SettingEnable = false;
			handlePanel.SelectionVisible = true;
			handlePanel.SelectedColor = Manager.SelectedTextColor;

			return handlePanel;
		}

		public void AddHandle()
		{
			EventCondition condition = _eventPanel.EventCondition;
			ClearViewModel();

			if (Camera != null)
			{
				ApplicationTriggerAction();

				Controls.Add(GetNewAddHandlePanel(new UploadFtpEventHandle
				{
					Device = Camera,
					FileName = Camera.ToString(),
					ReadyState = ReadyState.NotInUse,
				}));

				Controls.Add(GetNewAddHandlePanel(new SendMailEventHandle
				{
					Device = Camera,
					MailReceiver = Server.User.Current.Email,
					//Subject = Camera + " - " + condition.ToString(),
					//Body = Camera + " - " + condition.ToString() + " (SNVR " + App.Credential.Domain + ")",
					AttachFile = true,
					ReadyState = ReadyState.NotInUse,
				}));

				var hasDOPanel = false;
				UInt16 defaultDoId = 1;
				//Check if DO is Avaliable
				if (Camera.Model.IOPortSupport != null)
				{
					foreach (var ioPort in Camera.IOPort)
					{
						if (ioPort.Value == IOPort.Output)
						{
							defaultDoId = ioPort.Key;
							hasDOPanel = true;
							break;
						}
					}
				}
				else
				{
					if (Camera.Model.NumberOfDo > 0)
						hasDOPanel = true;
				}

			    var allDevices = new List<IDevice>();
                if(Server is ICMS)
                {
                    var cms = Server as ICMS;
                    foreach (KeyValuePair<ushort, INVR> nvr in cms.NVRManager.NVRs)
                    {
                        allDevices.AddRange(nvr.Value.Device.Devices.Values);
                    }
                }
                else
                {
                    allDevices.AddRange(Server.Device.Devices.Values);
                }

				if (hasDOPanel)
				{
					Controls.Add(GetNewAddHandlePanel(new TriggerDigitalOutEventHandle
					{
						Device = Camera,
						DigitalOutId = defaultDoId,
						ReadyState = ReadyState.NotInUse,
					}));
				}
				else
				{
					foreach (IDevice obj in allDevices)
					{
						var camera = obj as ICamera;
						if (camera == null) continue;
						if (camera == Camera) continue;

						if (camera.Model.IOPortSupport != null)
						{
							var dosupport = false;
							foreach (var ioPort in camera.IOPort)
							{
								if (ioPort.Value == IOPort.Output)
								{
									defaultDoId = ioPort.Key;
									dosupport = true;
									break;
								}
							}
							if (!dosupport)
								continue;
						}
						else
						{
							if (camera.Model.NumberOfDo == 0)
								continue;
						}

						Controls.Add(GetNewAddHandlePanel(new TriggerDigitalOutEventHandle
						{
							Device = obj,
							DigitalOutId = defaultDoId,
							ReadyState = ReadyState.NotInUse,
						}));
						break;
					}
				}

				//Check if Preset is Avaliable
                foreach (IDevice obj in allDevices)
				{
					var camera = obj as ICamera;
					if (camera == null) continue;
					if (camera.PresetPoints == null || camera.PresetPoints.Count == 0) continue;

					var pointIds = camera.PresetPoints.Keys.ToList();
					pointIds.Sort();
					Controls.Add(GetNewAddHandlePanel(new GotoPresetEventHandle
					{
						Device = camera,
						PresetPoint = pointIds.First(),
						ReadyState = ReadyState.NotInUse,
					}));
					break;
				}
			}
			else if(Server is IFOS)
			{
				Controls.Add(GetNewAddHandlePanel(new SendMailEventHandle
				{
					MailReceiver = Server.User.Current.Email,
					AttachFile = false,
					ReadyState = ReadyState.NotInUse,
				}));
			}

			var handleTitlePanel = GetHandlePanel();
			handleTitlePanel.IsTitle = true;
			handleTitlePanel.Cursor = Cursors.Default;
			handleTitlePanel.OnSelectAll += HandlePanelOnSelectAll;
			handleTitlePanel.OnSelectNone += HandlePanelOnSelectNone;
			Controls.Add(handleTitlePanel);

			_eventPanel.EventCondition = condition;
			_eventPanel.EditVisible = false;
			//Controls.Add(_eventPanel);

			SelectionVisible = true;
			_isEditing = true;
		}

		protected virtual void ApplicationTriggerAction()
		{
			//Controls.Add(GetNewAddHandlePanel(new GotoPresetEventHandle
			//{
			//    Device = Camera,
			//    ReadyState = ReadyState.NotInUse,
			//}));

			Controls.Add(GetNewAddHandlePanel(new HotSpotEventHandle
				{
					Device = Camera,
					ReadyState = ReadyState.NotInUse,
				}));

			Controls.Add(GetNewAddHandlePanel(new PopupPlaybackEventHandle
				{
					Device = Camera,
					ReadyState = ReadyState.NotInUse,
				}));

			Controls.Add(GetNewAddHandlePanel(new PopupLiveEventHandle
			{
				Device = Camera,
				ReadyState = ReadyState.NotInUse,
			}));

			Controls.Add(GetNewAddHandlePanel(new ExecEventHandle
				{
					ReadyState = ReadyState.NotInUse,
				}));

			Controls.Add(GetNewAddHandlePanel(new AudioEventHandle
				{
					ReadyState = ReadyState.NotInUse,
				}));

			Controls.Add(GetNewAddHandlePanel(new BeepEventHandle
				{
					ReadyState = ReadyState.NotInUse,
				}));
		}

		public void RemoveSelectedHandles()
		{
			var removeHandles = new List<EventHandle>();

			foreach (Control control in Controls)
			{
				if (control is EventPanel) continue;

				if (!((HandlePanel)control).Checked || ((HandlePanel)control).EventHandle == null) continue;

				removeHandles.Add(((HandlePanel)control).EventHandle);
			}

			if (removeHandles.Count == 0) return;

			foreach (EventHandle eventHandle in removeHandles)
			{
				EventHandles.Remove(eventHandle);
			}

			if (Camera == null || Camera.EventHandling == null) return;

			Camera.EventHandling.ReadyState = ReadyState.Modify;

			if (OnHandleChange != null)
				OnHandleChange(this, null);
		}

		private void HandlePanelOnSelectAll(Object sender, EventArgs e)
		{
			List<HandlePanel> controls = new List<HandlePanel>();
			foreach (Control control in Controls)
			{
				if (control is EventPanel) continue;
				controls.Add(((HandlePanel)control));
			}
			controls.Reverse();

			foreach (HandlePanel control in controls)
			{
				control.Checked = true;
			}
		}

		private void HandlePanelOnSelectNone(Object sender, EventArgs e)
		{
			foreach (Control control in Controls)
			{
				if (control is EventPanel) continue;

				((HandlePanel)control).Checked = false;
			}
		}
	}
}
