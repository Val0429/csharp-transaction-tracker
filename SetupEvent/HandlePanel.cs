using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
	public class HandlePanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;
		public event EventHandler OnHandleChange;

		public IServer Server;
		public ICamera Camera;
		public Boolean IsTitle;
		private EventHandle _eventHandle;
		public EventHandle EventHandle
		{
			get { return _eventHandle; }
			set
			{
				if (_focusSettingPanel != null)
					_focusSettingPanel.Visible = false;

				_eventHandle = value;
				_focusSettingPanel = null;

				if(_eventHandle == null)
				{
					_checkBox.Enabled = true;
					return;
				}

				_checkBox.Enabled = _eventHandle.Enable;

				switch(_eventHandle.Type)
				{
					case HandleType.Beep:
						_focusSettingPanel = _beepSettingPanel;
						_beepSettingPanel.EventHandle = (BeepEventHandle)_eventHandle;
						break;
					case HandleType.Audio:
						_focusSettingPanel = _audioSettingPanel;
						_audioSettingPanel.EventHandle = (AudioEventHandle)_eventHandle;
						break;

					case HandleType.ExecCmd:
						_focusSettingPanel = _execSettingPanel;
						_execSettingPanel.EventHandle = (ExecEventHandle)_eventHandle;
						break;

					case HandleType.HotSpot:
						_focusSettingPanel = _hotspotSettingPanel;
						_hotspotSettingPanel.EventHandle = (HotSpotEventHandle)_eventHandle;
						break;

					case HandleType.GoToPreset:
						_focusSettingPanel = _gotoPresetSettingPanel;
						_gotoPresetSettingPanel.EventHandle = (GotoPresetEventHandle)_eventHandle;
						break;

					case HandleType.PopupPlayback:
						_focusSettingPanel = _popupPlaybackSettingPanel;
						_popupPlaybackSettingPanel.EventHandle = (PopupPlaybackEventHandle)_eventHandle;
						break;

					case HandleType.PopupLive:
						_focusSettingPanel = _popupLiveSettingPanel;
						_popupLiveSettingPanel.EventHandle = (PopupLiveEventHandle)_eventHandle;
						break;

					case HandleType.TriggerDigitalOut:
						_focusSettingPanel = _triggerDigitalOutSettingPanel;
						_triggerDigitalOutSettingPanel.EventHandle = (TriggerDigitalOutEventHandle)_eventHandle;
						break;

					case HandleType.SendMail:
						if (Camera != null)
						{
							_focusSettingPanel = _sendMailSettingPanel;
							_sendMailSettingPanel.EventHandle = (SendMailEventHandle)_eventHandle;
						}
						else
						{
							_focusSettingPanel = _sendMailNoAttachSettingPanel;
							_sendMailNoAttachSettingPanel.EventHandle = (SendMailEventHandle)_eventHandle;
						}

						break;

					case HandleType.UploadFtp:
						_focusSettingPanel = _uploadFtpSettingPanel;
						_uploadFtpSettingPanel.EventHandle = (UploadFtpEventHandle)_eventHandle;
						break;
				}

				if (_focusSettingPanel != null)
					_focusSettingPanel.Visible = true;
			}
		}

		private readonly CheckBox _checkBox;
		private Panel _focusSettingPanel;
		private BeepSettingPanel _beepSettingPanel;
		private AudioSettingPanel _audioSettingPanel;
		private ExecSettingPanel _execSettingPanel;
		private GotoPresetSettingPanel _gotoPresetSettingPanel;
		private PopupPlaybackSettingPanel _popupPlaybackSettingPanel;
		private PopupLiveSettingPanel _popupLiveSettingPanel;
		private HotSpotSettingPanel _hotspotSettingPanel;
		private TriggerDigitalOutSettingPanel _triggerDigitalOutSettingPanel;
		private SendMailSettingPanel _sendMailSettingPanel;
		private SendMailNoAttachSettingPanel _sendMailNoAttachSettingPanel;
		private UploadFtpSettingPanel _uploadFtpSettingPanel;
		public Dictionary<String, String> Localization;

		public HandlePanel()
		{
			Localization = new Dictionary<String, String>
			{
					 {"HandlePanel_EventHandler", "Event Handler"},
					 {"HandlePanel_Setting", "Seting"},
					 {"HandlePanel_Beep", "Beep"},
					 {"HandlePanel_Audio", "Audio"},
					 {"HandlePanel_Command", "Command"},
					 {"HandlePanel_Hotspot", "Hotspot"},
					 {"HandlePanel_GotoPreset", "Goto Preset"},
					 {"HandlePanel_PopupPlayback", "Popup Playback"},
					 {"HandlePanel_PopupLive", "Popup Live"},
					 {"HandlePanel_TriggerDO", "Trigger DO"},
					 {"HandlePanel_SendMail", "Send Mail"},
					 {"HandlePanel_UploadFTP", "Upload FTP"},
			};
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Size = new Size(400, 40);
			BackColor = Color.Transparent;

			_checkBox = new CheckBox
			{
				Padding = new Padding(10, 0, 0, 0),
				Dock = DockStyle.Left,
				Width = 25
			};

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += HandlePanelMouseClick;
			Paint += HandlePanelPaint;
		}

		public void Initialize()
		{
			_beepSettingPanel = new BeepSettingPanel
			{
				HandlePanel = this,
			};

			_audioSettingPanel = new AudioSettingPanel
			{
				HandlePanel = this,
			};

			_execSettingPanel = new ExecSettingPanel
			{
				HandlePanel = this,
			};

			_hotspotSettingPanel = new HotSpotSettingPanel
			{
				HandlePanel = this,
			};

			_gotoPresetSettingPanel = new GotoPresetSettingPanel
			{
				HandlePanel = this,
			};

			_popupPlaybackSettingPanel = new PopupPlaybackSettingPanel
			{
				HandlePanel = this,
			};

			_popupLiveSettingPanel = new PopupLiveSettingPanel
			{
				HandlePanel = this,
			};
			
			_triggerDigitalOutSettingPanel = new TriggerDigitalOutSettingPanel
			{
				HandlePanel = this,
			};

			_sendMailSettingPanel = new SendMailSettingPanel
			{
				HandlePanel = this,
			};

			_sendMailNoAttachSettingPanel = new SendMailNoAttachSettingPanel
			{
				HandlePanel = this,
			};

			_uploadFtpSettingPanel = new UploadFtpSettingPanel
			{
				HandlePanel = this,
			};

			_beepSettingPanel.Visible = false;
			_audioSettingPanel.Visible = false;
			_execSettingPanel.Visible = false;
			_hotspotSettingPanel.Visible = false;
			_gotoPresetSettingPanel.Visible = false;
			_popupPlaybackSettingPanel.Visible = false;
			_popupLiveSettingPanel.Visible = false;
			_triggerDigitalOutSettingPanel.Visible = false;
			_sendMailSettingPanel.Visible = false;
			_sendMailNoAttachSettingPanel.Visible = false;
			_uploadFtpSettingPanel.Visible = false;

			Controls.Add(_beepSettingPanel);
			Controls.Add(_audioSettingPanel);
			Controls.Add(_execSettingPanel);
			Controls.Add(_gotoPresetSettingPanel);
			Controls.Add(_popupPlaybackSettingPanel);
			Controls.Add(_popupLiveSettingPanel);
			Controls.Add(_hotspotSettingPanel);
			Controls.Add(_triggerDigitalOutSettingPanel);
			Controls.Add(_sendMailSettingPanel);
			Controls.Add(_sendMailNoAttachSettingPanel);
			Controls.Add(_uploadFtpSettingPanel);
		}

		public void HandleChange()
		{
			if (_eventHandle.ReadyState == ReadyState.Ready)
			{
				_eventHandle.ReadyState = ReadyState.Modify;
				Invalidate();
			}

			if (Camera == null || Camera.EventHandling == null) return;

			Camera.EventHandling.ReadyState = ReadyState.Modify;

			if (OnHandleChange != null)
				OnHandleChange(this, null);
		}

		private static RectangleF _nameRectangleF = new RectangleF(44, 13, 156, 17);

		private void HandlePanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, this);

			if(IsTitle)
			{
				if (Width <= 200) return;
				Manager.PaintText(g, Localization["HandlePanel_EventHandler"]);

				if (Width <= 350) return;
				g.DrawString(Localization["HandlePanel_Setting"], Manager.Font, Brushes.Black, 200, 13);
				return;
			}

			Manager.PaintStatus(g, EventHandle.ReadyState);

			Brush fontBrush = Brushes.Black;
			if (_checkBox.Visible && Checked)
			{
				fontBrush = SelectedColor;
			}

			if (Width <= 200) return;
			g.DrawString(HandleToLocalizationString(EventHandle.Type), Manager.Font, (_eventHandle.Enable) ? fontBrush : Brushes.Gray, _nameRectangleF);
		}

		public String HandleToLocalizationString(HandleType type)
		{
			switch (type)
			{
				case HandleType.Beep:
					return Localization["HandlePanel_Beep"];

				case HandleType.Audio:
					return Localization["HandlePanel_Audio"];

				case HandleType.ExecCmd :
					return Localization["HandlePanel_Command"];

				case HandleType.HotSpot:
					return Localization["HandlePanel_Hotspot"];

				case HandleType.GoToPreset:
					return Localization["HandlePanel_GotoPreset"];

				case HandleType.PopupPlayback:
					return Localization["HandlePanel_PopupPlayback"];

				case HandleType.PopupLive:
					return Localization["HandlePanel_PopupLive"];

				case HandleType.TriggerDigitalOut:
					return Localization["HandlePanel_TriggerDO"];

				case HandleType.SendMail:
					return Localization["HandlePanel_SendMail"];

				case HandleType.UploadFtp:
					return Localization["HandlePanel_UploadFTP"];

				default:
					return type.ToString();
			}
		}

		private void HandlePanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (_checkBox.Visible && _checkBox.Enabled)
			{
				_checkBox.Checked = !_checkBox.Checked;
			}
		}

		private void CheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			Invalidate();

			if(IsTitle)
			{
				if (Checked && OnSelectAll != null)
					OnSelectAll(this, null);
				else if (!Checked && OnSelectNone != null)
					OnSelectNone(this, null);

				return;
			}

			if (OnSelectChange != null)
				OnSelectChange(this, null);
		}

		public Brush SelectedColor = Manager.DeleteTextColor;

		public Boolean Checked
		{
			get
			{
				return _checkBox.Checked;
			}
			set
			{
				if(_checkBox.Enabled)
					_checkBox.Checked = (value && _checkBox.Visible);
			}
		}

		public Boolean SelectionVisible
		{
			get { return _checkBox.Visible; }
			set
			{
				_checkBox.Visible = value;
			}
		}

		public Boolean SettingEnable
		{
			set
			{
				if (_focusSettingPanel != null)
					_focusSettingPanel.Enabled = value;
			}
		}

		public Boolean CheckBoxEnabled
		{
			get { return _checkBox.Enabled; }
		}
	}
}
