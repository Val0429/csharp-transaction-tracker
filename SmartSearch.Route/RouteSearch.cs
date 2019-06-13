using System;
using System.Drawing;
using System.Windows.Forms;
using Interface;

namespace SmartSearch.Route
{
	public partial class RouteSearch : SmartSearch
	{
		public event EventHandler<EventArgs<String>> OnConditionChange;
		private const String OnConditionChangeXml =
			"<XML><Channel>{CHANNEL}</Channel><StartTime>{STARTTIME}</StartTime><EndTime>{ENDTIME}</EndTime><Spaceing>{SPACEING}</Spaceing><Limit>{LIMIT}</Limit></XML>";

		public RouteSearch()
		{
			InitializeComponent();
		}

		public override void Initialize()
		{
			base.Initialize();
			speacingComboBox.Text = @"120";
		}

		public override void AppendDevice(IDevice device)
		{
			base.AppendDevice(device);

			if (device != null)
			{
				nameLabel.Text = "";
				if (VideoWindow.Device.GPSInfo.ServerNo != "")
				{
					nameLabel.Text = @"[" + VideoWindow.Device.GPSInfo.ServerNo + @" ]";
				}

				if (VideoWindow.Device.GPSInfo.VehicleNo != "")
				{
					nameLabel.Text += @"[" + VideoWindow.Device.GPSInfo.VehicleNo + @"] ";
				}

				nameLabel.Text += VideoWindow.Device;

				if (App.Type == Constant.App.CMS)
					nameLabel.Text += @" (" + device.Server.Name + @" " +
									 device.Server.Credential.Domain + @")";
			}
			else
				nameLabel.Text = "";
		}

		public override void DragStop(Point point, EventArgs<Object> e)
		{
			base.DragStop(point, e);

			if (VideoWindow.Device != null)
			{
				nameLabel.Text = "";
				if (VideoWindow.Device.GPSInfo.ServerNo != "")
				{
					nameLabel.Text = @"[" + VideoWindow.Device.GPSInfo.ServerNo + @" ]";
				}

				if (VideoWindow.Device.GPSInfo.VehicleNo != "")
				{
					nameLabel.Text += @"[" + VideoWindow.Device.GPSInfo.VehicleNo + @"] ";
				}

				nameLabel.Text += VideoWindow.Device;

				if (App.Type == Constant.App.CMS)
					nameLabel.Text += @" (" + VideoWindow.Device.Server.Name + @" " + VideoWindow.Device.Server.Credential.Domain + @")";
			}
		}

		private void SpeedLimitCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			var obj = sender as CheckBox;
			if (obj == null) return;

			if (obj.CheckState == CheckState.Checked)
			{
				speedLimitText.Text = @"70";
				speedLimitText.Enabled = true;
			}
			else
			{
				speedLimitText.Text = "";
				speedLimitText.Enabled = false;
			}
		}

		private void SearchButtonClick(object sender, EventArgs e)
		{
			if (VideoWindow.Device == null)
				return;

			var bt = new DateTime(1970, 1, 1, 0, 0, 0, 0);

			var start = Convert.ToDateTime(startDatePicker.Text + ' ' + startTimePicker.Text).ToUniversalTime();
			var end = Convert.ToDateTime(endDatePicker.Text + ' ' + endTimePicker.Text).ToUniversalTime();
			var startUTime = Convert.ToUInt64((start - bt).TotalMilliseconds);
			var endUTime = Convert.ToUInt64((end - bt).TotalMilliseconds);

			if (OnConditionChange != null)
			{
				int limit;
				int.TryParse(speedLimitText.Text, out limit);
				if (limit == 0) limit = 9999;

				var str = OnConditionChangeXml
					.Replace("{CHANNEL}", VideoWindow.Device.Id.ToString())
					.Replace("{STARTTIME}", startUTime.ToString())
					.Replace("{ENDTIME}", endUTime.ToString())
					.Replace("{SPACEING}", speacingComboBox.Text)
					.Replace("{LIMIT}", limit.ToString());

				OnConditionChange(this, new EventArgs<String>(str));

			}

			GoTo(startUTime);
		}
	}
}
