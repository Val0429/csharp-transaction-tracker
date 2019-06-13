using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;
using PanelBase;

namespace Investigation.EventCalendar
{
    public sealed partial class CriteriaPanel : UserControl
    {
        public event EventHandler OnDeviceEdit;
        public event EventHandler OnDateTimeEdit;
        public event EventHandler OnEventEdit;
        public ICMS CMS;
        public INVR NVR;

        public Dictionary<String, String> Localization;

        public CameraEventSearchCriteria SearchCriteria;

        public CriteriaPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Investigation_Device", "Device"},
								   {"Investigation_DateTime", "Date / Time"},
								   {"Investigation_Event", "Event"},
								   {"Investigation_AllDevice", "All Device"},
								   
								   {"Common_ThisMonth", "This month"},
								   {"Common_LastMonth", "Last month"},
								   {"Common_TheMonthBeforeLast", "The month before last"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            deviceDoubleBufferPanel.Paint += DeviceDoubleBufferPanelPaint;
            deviceDoubleBufferPanel.MouseClick += DeviceDoubleBufferPanelMouseClick;

            dateTimeDoubleBufferPanel.Paint += DateTimeDoubleBufferPanelPaint;
            dateTimeDoubleBufferPanel.MouseClick += DateTimeDoubleBufferPanelMouseClick;

            eventDoubleBufferPanel.Paint += EventDoubleBufferPanelPaint;
            eventDoubleBufferPanel.MouseClick += EventDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Panel panel, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, panel);
            Manager.PaintEdit(g, panel);

            if (Localization.ContainsKey("Investigation_" + panel.Tag))
                Manager.PaintText(g, Localization["Investigation_" + panel.Tag]);
            else
                Manager.PaintText(g, panel.Tag.ToString());
        }

        private void DeviceDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(deviceDoubleBufferPanel, e);

            Graphics g = e.Graphics;

            var containesAll = true;
            if (CMS != null)
            {
                foreach (KeyValuePair<UInt16, INVR> nvr in CMS.NVRManager.NVRs)
                {
                    foreach (KeyValuePair<UInt16, IDevice> device in nvr.Value.Device.Devices)
                    {
                        foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
                        {
                            if (nvrDevice.NVRId != device.Value.Server.Id && nvrDevice.DeviceId != device.Key)
                            {
                                containesAll = false;
                                break;
                            }
                        }
                        if (!containesAll)
                            break;
                    }
                    if (!containesAll)
                        break;
                }
            }
            else
            {
                foreach (var device in NVR.Device.Devices)
                {
                    if (!SearchCriteria.Device.Contains(device.Key))
                    {
                        containesAll = false;
                        break;
                    }
                }
            }

            if (containesAll && NVR.Device.Devices.Count > 3)
            {
                Manager.PaintTextRight(g, deviceDoubleBufferPanel, Localization["Investigation_AllDevice"]);
            }
            else
            {
                var deviceSelecton = new List<String>();

                var list = new List<String>();

                if (CMS != null)
                {
                    foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
                    {
                        if (!CMS.NVRManager.NVRs.ContainsKey(nvrDevice.NVRId)) continue;
                        var nvr = CMS.NVRManager.NVRs[nvrDevice.NVRId];
                        var device = nvr.Device.FindDeviceById(nvrDevice.DeviceId);
                        if (device != null)
                            list.Add(String.Format("{0} {1}", nvr, device));
                    }
                }
                else
                {
                    foreach (var deviceId in SearchCriteria.Device)
                    {
                        var device = NVR.Device.FindDeviceById(deviceId);
                        if (device != null)
                            list.Add(device.ToString());
                    }
                }


                list.Sort();

                foreach (var device in list)
                {
                    if (deviceSelecton.Count >= 3)
                    {
                        deviceSelecton[2] += " ...";
                        break;
                    }

                    if (String.IsNullOrEmpty(device)) continue;

                    deviceSelecton.Add(device);
                }

                Manager.PaintTextRight(g, deviceDoubleBufferPanel, String.Join(", ", deviceSelecton.ToArray()));
            }
        }

        private void DateTimeDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(dateTimeDoubleBufferPanel, e);

            Graphics g = e.Graphics;

            switch (SearchCriteria.DateTimeSet)
            {
                case DateTimeSet.ThisMonth:
                    Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_ThisMonth"]);
                    break;

                case DateTimeSet.LastMonth:
                    Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_LastMonth"]);
                    break;

                case DateTimeSet.TheMonthBeforeLast:
                    Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, Localization["Common_TheMonthBeforeLast"]);
                    break;

                default:
                    var start = DateTimes.ToDateTime(SearchCriteria.StartDateTime, NVR.Server.TimeZone);
                    var end = DateTimes.ToDateTime(SearchCriteria.EndDateTime, NVR.Server.TimeZone);

                    if (start.Year == end.Year && start.Month == end.Month)
                        Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, start.ToYearMonthString());
                    else
                        Manager.PaintTextRight(g, dateTimeDoubleBufferPanel, start.ToYearMonthString() + @" ~ " + end.ToYearMonthString());
                    break;
            }
        }

        private void EventDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        {
            InputPanelPaint(eventDoubleBufferPanel, e);

            Graphics g = e.Graphics;

            var eventSelection = new List<String>();

            foreach (var eventType in SearchCriteria.Event)
            {
                if (eventSelection.Count >= 3)
                {
                    eventSelection[2] += " ...";
                    break;
                }

                eventSelection.Add(CameraEventSearchCriteria.EventTypeToLocalizationString(eventType));
            }

            Manager.PaintTextRight(g, eventDoubleBufferPanel, String.Join(", ", eventSelection.ToArray()));
        }

        private void DeviceDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceEdit != null)
                OnDeviceEdit(this, e);
        }

        private void DateTimeDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDateTimeEdit != null)
                OnDateTimeEdit(this, e);
        }

        private void EventDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnEventEdit != null)
                OnEventEdit(this, e);
        }

        public void ShowCriteria()
        {
        }
    }
}