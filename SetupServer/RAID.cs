using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
	public sealed partial class RAID : UserControl
	{
		public IServer Server;
		public Dictionary<String, String> Localization;
		private const UInt16 MinimizeKeepSpace = 15;
		private const UInt16 MaximizeKeepSpace = 50;
		private DiskInfo _disk;
		private readonly System.Timers.Timer _formatProcessTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _processTimer = new System.Timers.Timer(); // for resync, recovery, check
		public event EventHandler<EventArgs<String>> OnUpdateButtons;
		public RAID()
		{
			Localization = new Dictionary<String, String>
			{
				{"MessageBox_Error", "Error"},
				{"MessageBox_Confirm", "Confirm"},
				{"MessageBox_Information", "Information"},

				{"SetupServer_Status", "Status"},
				{"SetupServer_StatusStabdBy", "Stand by"}, 
				{"SetupServer_StatusCheck", "Check"},
				{"SetupServer_StatusFormat", "Format"},
				{"SetupServer_StatusActive", "Active"},
				{"SetupServer_StatusInactive", "Inactive"},
				{"SetupServer_StatusRecovery", "Recovery"},
				{"SetupServer_StatusResync", "Resync"},
				{"SetupServer_StatusDegraded", "Degraded"},
				{"SetupServer_RAIDCompleted", "RAID completed"},
				{"SetupServer_ConfirmFormatEmptyError", "Please select a RAID mode."},
				{"SetupServer_ConfirmFormatError", "You can\'t  format with %1  right now, please select another mode."},
				{"SetupServer_ConfirmFormatStatusError", "You can\'t  format with status \"%1\" right now."},
				{"SetupServer_ConfirmFormat", "Are you sure you want to format with %1?"},
				{"SetupServer_Description", "Description of %1:"},
				{"SetupServer_DescRAID0", "Improved performance and additional storage but no redundancy or fault tolerance."},
				{"SetupServer_DescRAID1", "Maximum protection for critical data. Identical data is written to multiple drives simultaneously."},
				{"SetupServer_DescRAID5", "Data protection against a drive failure, strong performance and efficient storage use."},
				{"SetupServer_DescRAID10", "Higher performance than RAID 1 and stronger data protection than RAID 0."},

				{"SetupServer_RAID_DiskStatus_GOOD", "Disk Status is good,  S.M.A.R.T  passed."},
				{"SetupServer_RAID_DiskStatus_SMART_NOT_SUPPORT", "S.M.A.R.T notsupported."},
				{"SetupServer_RAID_DiskStatus_SMART_PARSE_ERROR", "S.M.A.R.T failures."},
				{"SetupServer_RAID_DiskStatus_BAD_SECTOR", "Disk has bad track, S.M.A.R.T detect %1 bad track(s)."},
				{"SetupServer_RAID_DiskStatus_ATTRIBUTE_NOW", "Disk status abnormal, S.M.A.R.T detects abnormal activity."},
				{"SetupServer_RAID_DiskStatus_ATTRIBUTE_IN_THE_PAST", "Disk status abnormal, S.M.A.R.T detects abnormal activity."},
				{"SetupServer_RAID_DiskStatus_BAD_STATUS", "Disk condition danger, S.M.A.R.T detects disk failure."},

				{"StorageControl_KeepSpaceValue", "Keep space should between %1GB to disk capacity less %2GB"},
				{"StorageControl_KeepSpaceSuggest", "Suggested minimum keep space amount:"},
				{"StorageControl_KeepSpaceSuggest1", "For total bitrate under 10Mb/s = 15GB"},
				{"StorageControl_KeepSpaceSuggest2", "For total bitrate 10Mb/s to 50Mb/s = 30GB"},
				{"StorageControl_KeepSpaceSuggest3", "For total bitrate 50Mb/s to 100Mb/s = 40GB"},
				{"StorageControl_KeepSpaceSuggest4", "For total bitrate 100Mb/s to 150Mb/s = 50GB"},
				{"StorageControl_KeepSpaceSuggest5", "For total bitrate 150Mb/s to 200Mb/s = 60GB"},

				{"StoragePanel_KeepSpace", "Keep Space"},
			};
			Localizations.Update(Localization);

			InitializeComponent();

			DoubleBuffered = true;
			Dock = DockStyle.None;
			BackgroundImage = Manager.BackgroundNoBorder;

			statusPanel.Paint += StatusPanelPaint;

			keepSpacePanel.Tag = Localization["StoragePanel_KeepSpace"];
			keepSpacePanel.Paint += InputPanelPaint;
			labelKeepSpace.Text =
				Localization["StorageControl_KeepSpaceValue"].Replace("%1", MinimizeKeepSpace.ToString()).Replace("%2", MaximizeKeepSpace.ToString()) +
				Environment.NewLine + Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest"] +
				Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest1"] +
				Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest2"] +
				Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest3"] +
				Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest4"] +
				Environment.NewLine + Localization["StorageControl_KeepSpaceSuggest5"];

			textBoxKeepSpace.KeyPress += KeyAccept.AcceptNumberOnly;
			textBoxKeepSpace.TextChanged += TextBoxKeepSpaceChange;

			RAID0Panel.Tag = RAIDMode.RAID0;
			RAID1Panel.Tag = RAIDMode.RAID1;
			RAID5Panel.Tag = RAIDMode.RAID5;
			RAID10Panel.Tag = RAIDMode.RAID10;

			RAID0Panel.Paint += RAIDPanelPaint;
			RAID1Panel.Paint += RAIDPanelPaint;
			RAID10Panel.Paint += RAIDPanelPaint;
			RAID5Panel.Paint += RAIDPanelPaint;

			RAID0Panel.Click += RAIDPanelClick;
			RAID1Panel.Click += RAIDPanelClick;
			RAID5Panel.Click += RAIDPanelClick;
			RAID10Panel.Click += RAIDPanelClick;

			_formatProcessTimer.Elapsed += FormatProcessTimerTick;
			_formatProcessTimer.Interval = 10000;

			_processTimer.Elapsed += FormatProcessTimerTick;
			_processTimer.Interval = 60000;
		}

		public void InitialRAIDSetting()
		{
			_formatProcessTimer.SynchronizingObject = Server.Form;
			_processTimer.SynchronizingObject = Server.Form;

			if (!Server.Server.CheckSetupEnabled("RAID"))
				Name = "Storage";
			Server.Server.OnRAIDProcessUpdate += ServerOnRAIDProcessUpdate;
			DisksLayoutPanel.Visible = labelDisks.Visible = statusPanel.Visible = statusLabel.Visible = containerPanel.Visible = RAIDlabel.Visible = Server.Server.CheckSetupEnabled("RAID");
		}

		public void ParseRAIDInfo()
		{
			_formatRAIDStatus = Server.Server.RAID.Status;

			foreach (KeyValuePair<String, DiskInfo> diskInfo in Server.Server.StorageInfo)
			{
				if (diskInfo.Key == "\\")
				{
					_disk = diskInfo.Value;
					break;
				}
			}

			foreach (Storage storage in Server.Server.ChangedStorage)
			{
				if (storage.Key == "\\") textBoxKeepSpace.Text = storage.KeepSpace.ToString();
			}

			ShowRAIDDescription(Server.Server.RAID.Mode);
			UpdateRAIDStatus();
		}

		private void UpdateRAIDStatus()
		{
			DisksLayoutPanel.Controls.Clear();

			foreach (KeyValuePair<String, RAIDDisk> diskStatus in Server.Server.RAID.DiskStatus)
			{
				var desc = diskStatus.Value.Description;

				if (Localization.ContainsKey("SetupServer_RAID_DiskStatus_" + desc))
				{
					desc = Localization["SetupServer_RAID_DiskStatus_" + desc];
				}

				var panel = new StorageDiskLabel
				{
					Id = diskStatus.Key,
					Status = diskStatus.Value.Status,
					ToolTip = desc.Replace("%1", diskStatus.Value.DescValue)
				};

				DisksLayoutPanel.Controls.Add(panel);
			}

			switch (Server.Server.RAID.Disks)
			{
				case 1:
					RAID0Panel.Enabled = true;
					RAID1Panel.Enabled = RAID5Panel.Enabled = RAID10Panel.Enabled = false;
					break;

				case 2:
					RAID0Panel.Enabled = RAID1Panel.Enabled = true;
					RAID5Panel.Enabled = RAID10Panel.Enabled = false;
					break;

				case 3:
				case 5:
					RAID0Panel.Enabled = RAID5Panel.Enabled = true;
					RAID1Panel.Enabled = RAID10Panel.Enabled = false;
					break;

				case 4:
				case 6:
					RAID0Panel.Enabled = RAID5Panel.Enabled = RAID10Panel.Enabled = true;
					RAID1Panel.Enabled = false;
					break;
				default:
					RAID0Panel.Enabled = RAID1Panel.Enabled = RAID5Panel.Enabled = RAID10Panel.Enabled = false;
					break;
			}

			RAID0Panel.Cursor = RAID0Panel.Enabled ? Cursors.Hand : Cursors.Default;
			RAID1Panel.Cursor = RAID1Panel.Enabled ? Cursors.Hand : Cursors.Default;
			RAID5Panel.Cursor = RAID5Panel.Enabled ? Cursors.Hand : Cursors.Default;
			RAID10Panel.Cursor = RAID10Panel.Enabled ? Cursors.Hand : Cursors.Default;

			if (Server.Server.RAID.Status == RAIDStatus.Inactive || Server.Server.RAID.Status == RAIDStatus.Format || Server.Server.RAID.Status == RAIDStatus.Standby || Server.Server.RAID.Status == RAIDStatus.Recovery)
			{
				textBoxKeepSpace.Enabled = false;
			}
			else
			{
				textBoxKeepSpace.Enabled = true;
			}

			if (Server.Server.RAID.Status == RAIDStatus.Active || Server.Server.RAID.Status == RAIDStatus.Inactive || Server.Server.RAID.Status == RAIDStatus.Degrade)
			{
				labelProgress.Visible = progressBar.Visible = false;
				_formatProcessTimer.Enabled = false;
				_processTimer.Enabled = false;
			}

			if (!_processTimer.Enabled && (Server.Server.RAID.Status == RAIDStatus.Check || Server.Server.RAID.Status == RAIDStatus.Recovery || Server.Server.RAID.Status == RAIDStatus.Resync))
			{
				_processTimer.Enabled = true;
				progressBar.Value = Server.Server.RAID.Process;
				labelProgress.Text = String.Format("{0}%", Server.Server.RAID.Process);
				labelProgress.Visible = progressBar.Visible = true;
				Server.Server.LoadRAID();
			}

			if (!_formatProcessTimer.Enabled && (Server.Server.RAID.Status == RAIDStatus.Standby || Server.Server.RAID.Status == RAIDStatus.Format))
			{
				_formatProcessTimer.Enabled = true;
				progressBar.Value = (Server.Server.RAID.Status == RAIDStatus.Standby) ? 0 : Server.Server.RAID.Process;
				labelProgress.Text = String.Format("{0}%", Server.Server.RAID.Status == RAIDStatus.Standby ? 0 : Server.Server.RAID.Process);
				labelProgress.Visible = progressBar.Visible = true;

				Server.Server.LoadRAID();
			}

			UpdateFormatButton();
		}

		private void UpdateFormatButton()
		{
			var buttons = String.Empty;

			if (Server.Server.CheckSetupEnabled("RAID") && Server.Server.RAID.Status != RAIDStatus.Standby && Server.Server.RAID.Status != RAIDStatus.Format)
			{
				buttons = "Format";
			}

			if (OnUpdateButtons != null)
				OnUpdateButtons(this, new EventArgs<String>(buttons));
		}

		private void TextBoxKeepSpaceChange(Object sender, EventArgs e)
		{
			var result = true;
			var value = String.IsNullOrEmpty(textBoxKeepSpace.Text) ? MinimizeKeepSpace : Convert.ToUInt16(textBoxKeepSpace.Text);
			if (value > (Math.Floor(_disk.Total * 1.0 / 1073741824) - MaximizeKeepSpace))
			{
				var temp = Convert.ToUInt16(Math.Max(Math.Floor(_disk.Total * 1.0 / 1073741824) - MaximizeKeepSpace, MinimizeKeepSpace));
				if (temp != value)
				{
					value = temp;
					result = false;
				}
			}

			foreach (Storage storage in Server.Server.ChangedStorage)
			{
				if (storage.Key == "\\") storage.KeepSpace = value;
			}

			textBoxKeepSpace.BackColor = result ? Color.White : Color.LightPink;
		}

		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;
			Manager.PaintSingleInput(g, (Control)sender);

			if (Localization.ContainsKey("SetupServer_" + control.Tag))
				Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		private static void PanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, control);
		}

		private void RAIDPanelPaint(Object sender, PaintEventArgs e)
		{
			PanelPaint(sender, e);

			if (Width <= 100) return;

			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			var type = (RAIDMode)Enum.Parse(typeof(RAIDMode), control.Tag.ToString(), true);

			Manager.PaintText(g, ReadRAIDMode(type), (control.Enabled) ? Brushes.Black : Brushes.Gray);

			if (type == Server.Server.RAID.Mode)
			{
				Manager.PaintSelected(g);
			}
		}

		private void StatusPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Width <= 100) return;
			Graphics g = e.Graphics;
			Manager.PaintSingleInput(g, (Control)sender);
			Manager.PaintTextRight(g, statusPanel, ReadStatus(Server.Server.RAID.Status), ReadStatusColor(Server.Server.RAID.Status));
		}

		private void RAIDPanelClick(Object sender, EventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			if (!control.Enabled) return;

			Server.Server.RAID.Mode = (RAIDMode)control.Tag;
			ShowRAIDDescription(Server.Server.RAID.Mode);

			control.Focus();
			Invalidate();
		}

		private void ShowRAIDDescription(RAIDMode mode)
		{
			switch (mode)
			{
				case RAIDMode.RAID0:
					RAIDlabel.Text = String.Format("{0} {1}", Localization["SetupServer_Description"].Replace("%1", "RAID 0"), Localization["SetupServer_DescRAID0"]);
					break;
				case RAIDMode.RAID1:
					RAIDlabel.Text = String.Format("{0} {1}", Localization["SetupServer_Description"].Replace("%1", "RAID 1"), Localization["SetupServer_DescRAID1"]);
					break;
				case RAIDMode.RAID5:
					RAIDlabel.Text = String.Format("{0} {1}", Localization["SetupServer_Description"].Replace("%1", "RAID 5"), Localization["SetupServer_DescRAID5"]);
					break;
				case RAIDMode.RAID10:
					RAIDlabel.Text = String.Format("{0} {1}", Localization["SetupServer_Description"].Replace("%1", "RAID 10"), Localization["SetupServer_DescRAID10"]);
					break;
				default:
					RAIDlabel.Text = String.Empty;
					break;
			}
		}

		private String ReadStatus(RAIDStatus status)
		{
			switch (status)
			{
				case RAIDStatus.Standby:
					return Localization["SetupServer_StatusStabdBy"];
				case RAIDStatus.Check:
					return Localization["SetupServer_StatusCheck"];
				case RAIDStatus.Format:
					return Localization["SetupServer_StatusFormat"];
				case RAIDStatus.Active:
					return Localization["SetupServer_StatusActive"];
				case RAIDStatus.Inactive:
					return Localization["SetupServer_StatusInactive"];
				case RAIDStatus.Recovery:
					return Localization["SetupServer_StatusRecovery"];
				case RAIDStatus.Resync:
					return Localization["SetupServer_StatusResync"];
				case RAIDStatus.Degrade:
					return Localization["SetupServer_StatusDegraded"];
			}

			return String.Empty;
		}

		private Brush ReadStatusColor(RAIDStatus status)
		{
			switch (status)
			{
				case RAIDStatus.Standby:
				case RAIDStatus.Check:
				case RAIDStatus.Format:
				case RAIDStatus.Recovery:
				case RAIDStatus.Resync:
					return Manager.SelectedTextColor;

				case RAIDStatus.Inactive:
				case RAIDStatus.Degrade:
					return Manager.DeleteTextColor;
			}
			return Brushes.Black;
		}

		private void FormatProcessTimerTick(Object sender, EventArgs e)
		{
			Server.Server.LoadRAID();
		}

		private RAIDStatus _formatRAIDStatus;

		private void ServerOnRAIDProcessUpdate(Object sender, EventArgs e)
		{
			if (Server.Server.RAID.Status != RAIDStatus.Standby)
			{
				progressBar.Value = Server.Server.RAID.Process;
				labelProgress.Text = String.Format("{0}%", Server.Server.RAID.Process);
			};

			if (Server.Server.RAID.Status == RAIDStatus.Active || Server.Server.RAID.Status == RAIDStatus.Inactive || Server.Server.RAID.Status == RAIDStatus.Degrade)
			{

				if (_formatRAIDStatus == RAIDStatus.Format)
				{
					_formatRAIDStatus = Server.Server.RAID.Status;
					TopMostMessageBox.Show(Localization["SetupServer_RAIDCompleted"],
											Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
					ParseRAIDInfo();
				}

				_formatRAIDStatus = Server.Server.RAID.Status;
				labelProgress.Visible = progressBar.Visible = false;
				_formatProcessTimer.Enabled = false;
				_processTimer.Enabled = false;
			}
			else
			{
				if (Server.Server.RAID.Status != RAIDStatus.Format && _formatRAIDStatus == RAIDStatus.Format)
				{
					_formatProcessTimer.Enabled = false;
					_formatRAIDStatus = Server.Server.RAID.Status;
					TopMostMessageBox.Show(Localization["SetupServer_RAIDCompleted"],
											Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

					_processTimer.Enabled = true;
					ParseRAIDInfo();
				}
			}

			_formatRAIDStatus = Server.Server.RAID.Status;
			statusPanel.Focus();
			Invalidate();
		}

		public void CheckFormat()
		{
			Server.Server.LoadRAID();

			UpdateFormatButton();
			UpdateRAIDStatus();

			if (Server.Server.CheckSetupEnabled("RAID") && (Server.Server.RAID.Status == RAIDStatus.Standby || Server.Server.RAID.Status == RAIDStatus.Format))
			{
				TopMostMessageBox.Show(Localization["SetupServer_ConfirmFormatStatusError"].Replace("%1", ReadStatus(Server.Server.RAID.Status)),
										Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			if (Server.Server.RAID.Mode == RAIDMode.None)
			{
				TopMostMessageBox.Show(Localization["SetupServer_ConfirmFormatEmptyError"],
										Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			var check = DoubleCheckFormat();
			if (!check)
			{
				TopMostMessageBox.Show(Localization["SetupServer_ConfirmFormatError"].Replace("%1", ReadRAIDMode(Server.Server.RAID.Mode)),
									   Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			var result = TopMostMessageBox.Show(Localization["SetupServer_ConfirmFormat"].Replace("%1", ReadRAIDMode(Server.Server.RAID.Mode)), Localization["MessageBox_Confirm"],
								   MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result != DialogResult.Yes) return;

			Server.Server.RAID.Status = _formatRAIDStatus = RAIDStatus.Standby;
			labelProgress.Visible = progressBar.Visible = true;
			progressBar.Value = 0;
			labelProgress.Text = String.Format("{0}%", 0);

			Server.Server.RAIDFormat();

			textBoxKeepSpace.Enabled = false;
			textBoxKeepSpace.Text = MinimizeKeepSpace.ToString();
			TextBoxKeepSpaceChange(null, null);
			Server.Server.SaveStorage();

			_formatProcessTimer.Enabled = true;
			_processTimer.Enabled = false;
			UpdateFormatButton();
			statusPanel.Focus();
			Invalidate();
		}

		private Boolean DoubleCheckFormat()
		{
			if ((RAIDMode)RAID0Panel.Tag == Server.Server.RAID.Mode && !RAID0Panel.Enabled) return false;
			if ((RAIDMode)RAID1Panel.Tag == Server.Server.RAID.Mode && !RAID1Panel.Enabled) return false;
			if ((RAIDMode)RAID5Panel.Tag == Server.Server.RAID.Mode && !RAID5Panel.Enabled) return false;
			if ((RAIDMode)RAID10Panel.Tag == Server.Server.RAID.Mode && !RAID10Panel.Enabled) return false;

			return true;
		}

		private String ReadRAIDMode(RAIDMode mode)
		{
			switch (mode)
			{
				case RAIDMode.RAID0:
					return "RAID 0";
				case RAIDMode.RAID1:
					return "RAID 1";
				case RAIDMode.RAID5:
					return "RAID 5";
				case RAIDMode.RAID10:
					return "RAID 10";
				default:
					return String.Empty;
			}
		}
	}

	public sealed class StorageDiskLabel : Label
	{
		private static readonly Image _iconDiskError = Resources.GetResources(Properties.Resources.disk_err, Properties.Resources.IMGDiskError);
		private static readonly Image _iconDiskLost = Resources.GetResources(Properties.Resources.disk_lost, Properties.Resources.IMGDiskLost);
		private static readonly Image _iconDiskRebuild = Resources.GetResources(Properties.Resources.disk_rebuild, Properties.Resources.IMGDiskRebuild);
		private static readonly Image _iconDiskRun = Resources.GetResources(Properties.Resources.disk_run, Properties.Resources.IMGDiskRun);
		private static readonly Image _iconDiskStop = Resources.GetResources(Properties.Resources.disk_stop, Properties.Resources.IMGDiskStop);
		private static readonly Image _iconDiskWarning = Resources.GetResources(Properties.Resources.disk_warning, Properties.Resources.IMGDiskWarning);

		public static Dictionary<String, String> Localization;
		public StorageDiskLabel()
		{
			if (Localization == null)
			{
				Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"SetupServer_DiskUse", "Use"},
									{"SetupServer_DiskUnuse", "Unused"},
									{"SetupServer_DiskError", "Error"},
									{"SetupServer_DiskLost", "Lost"},
									{"SetupServer_DiskRebuild", "Rebuild"},
									{"SetupServer_DiskWarning", "Warning"}
							   };
				Localizations.Update(Localization);
			}

			Size = new Size(120, 60);
			Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			BorderStyle = BorderStyle.FixedSingle;
			Image = _iconDiskStop;
			ImageAlign = ContentAlignment.TopCenter;
			TextAlign = ContentAlignment.BottomCenter;
			Padding = new Padding(0, 0, 0, 5);
			Text = @"DISK";
			BackColor = Color.DarkGray;
			Cursor = Cursors.Hand;
			Click += StorageDiskLabelClick;
		}

		private void StorageDiskLabelClick(object sender, EventArgs e)
		{
			var icon = MessageBoxIcon.Information;
			switch (_status)
			{
				case RAIDDiskStatus.Warning:
					icon = MessageBoxIcon.Warning;
					break;
				case RAIDDiskStatus.Lost:
				case RAIDDiskStatus.Error:
					icon = MessageBoxIcon.Error;
					break;
			}

			var result = TopMostMessageBox.Show(String.Format("{0}\n{1}", Text, ToolTip), Localization["MessageBox_Information"],
				MessageBoxButtons.OK, icon);

			if(result == DialogResult.OK) ApplicationForms.HideLoadingIcon();
		}

		public String Id = "";

		private String _tooltip;
		public String ToolTip
		{
			get { return _tooltip; }
			set {
				_tooltip = value;
				SharedToolTips.SharedToolTip.SetToolTip(this, _tooltip);
			}
		}

		private RAIDDiskStatus _status;
		public RAIDDiskStatus Status
		{
			get { return _status; }
			set
			{
				_status = value;
				var statusString = String.Empty;
				switch (_status)
				{
					case RAIDDiskStatus.Error:
						Image = _iconDiskError;
						statusString = Localization["SetupServer_DiskError"];
						break;
					case RAIDDiskStatus.Lost:
						Image = _iconDiskLost;
						statusString = Localization["SetupServer_DiskLost"];
						break;
					case RAIDDiskStatus.Unused:
						Image = _iconDiskStop;
						statusString = Localization["SetupServer_DiskUnuse"];
						break;
					case RAIDDiskStatus.Ok:
						Image = _iconDiskRun;
						statusString = Localization["SetupServer_DiskUse"];
						break;
					case RAIDDiskStatus.Rebuild:
						Image = _iconDiskRebuild;
						statusString = Localization["SetupServer_DiskRebuild"];
						break;
					case RAIDDiskStatus.Warning:
						Image = _iconDiskWarning;
						statusString = Localization["SetupServer_DiskWarning"];
						break;
				}
				Text = String.Format("{0} : {1}", Id, statusString);
			}
		}
	}
}
