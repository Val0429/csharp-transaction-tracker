using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        private Color _deactivateRangeColor = Color.FromArgb(106, 110, 116);
        private Color _activateRangeColor = Color.FromArgb(150, 105, 28); //Color.FromArgb(255, 196, 61)

        public void SearchStartDateTimeChange(Object sender, EventArgs<String> e)
        {
            if (!rangeLeftPanel.Visible) return;

            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "StartTime");
            if (value == "" || value == "0") return;
            DateTime startDate = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);

            //if (OnExportStartDateTimeChange != null)
            //{
            //    if (TrackerContainer.RangeStartDate != DateTime.MinValue)
            //        OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
            //            DateTimes.ToUtcString(TrackerContainer.RangeStartDate, Server.Server.TimeZone))));
            //    else
            //        OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
            //            DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            //}

            if (startDate >= TrackerContainer.RangeEndDate)
            {
                SetRangeEndDate(DateTime.MaxValue);
                if (OnExportEndDateTimeChange != null)
                    OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMaxDateTime, Server.Server.TimeZone))));
            }
            SetRangeStartDate(startDate);
        }

        public void ExportDateTimeChange(Object sender, EventArgs<String> e)
        {
            if (!rangeLeftPanel.Visible || !rangeRightPanel.Visible) return;

            var xmlDoc = Xml.LoadXml(e.Value);
            String start = Xml.GetFirstElementValueByTagName(xmlDoc, "StartTime");
            if (start == "" || start == "0") return;
            DateTime startDate = DateTimes.ToDateTime(Convert.ToUInt64(start), Server.Server.TimeZone);

            String end = Xml.GetFirstElementValueByTagName(xmlDoc, "EndTime");
            if (end == "" || end == "0") return;
            DateTime endDate = DateTimes.ToDateTime(Convert.ToUInt64(end), Server.Server.TimeZone);

            if (startDate != TrackerContainer.RangeStartDate || endDate != TrackerContainer.RangeEndDate)
            {
                ClearSelection();

                //need to set end first, to coculator start position current
                SetRangeEndDate(endDate);
                SetRangeStartDate(startDate);
                SetRangeEndDate(endDate);
            }
        }

        protected void SetRangeStartDate(DateTime startDate)
        {
            TrackerContainer.RangeStartDate = startDate;
            TrackerContainer.Invalidate();
            if (TrackerContainer.RangeStartDate > DateTime.MinValue)
            {
                Int32 left = TrackerContainer.TicksToX(TrackerContainer.RangeStartDate.Ticks);
                if (TrackerContainer.RangeEndDate != DateTime.MaxValue)
                {
                    //range's width, if "too~~ small", split start and end to both side
                    double width = TrackerContainer.TicksToWidth(TrackerContainer.RangeEndDate.Ticks - TrackerContainer.RangeStartDate.Ticks) / 2.0;
                    if (width < rangeLeftPanel.Width)
                    {
                        left = Convert.ToInt32(left + width - rangeLeftPanel.Width);
                    }
                }

                rangeLeftPanel.Location = new Point(
                    Math.Min(Math.Max(left, devicePanel.Width), trackPanel.Width - rangeRightPanel.Width - rangeLeftPanel.Width),
                    rangeLeftPanel.Location.Y);
                rangeLeftPanel.BackColor = _activateRangeColor;
            }
            else
            {
                rangeLeftPanel.Location = new Point(devicePanel.Width, rangeLeftPanel.Location.Y);
                rangeLeftPanel.BackColor = _deactivateRangeColor;
            }
        }

        public void SearchEndDateTimeChange(Object sender, EventArgs<String> e)
        {
            if (!rangeRightPanel.Visible) return;

            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "EndTime");
            if (value == "" || value == "0") return;
            DateTime endDate = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);

            //if (OnExportEndDateTimeChange != null)
            //{
            //    if (TrackerContainer.RangeEndDate != DateTime.MaxValue)
            //        OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
            //            DateTimes.ToUtcString(TrackerContainer.RangeEndDate, Server.Server.TimeZone))));
            //}

            if (endDate <= TrackerContainer.RangeStartDate)
            {
                SetRangeStartDate(DateTime.MinValue);
                if (OnExportStartDateTimeChange != null)
                    OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                    DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            }
            SetRangeEndDate(endDate);
        }

        protected void SetRangeEndDate(DateTime endDate)
        {
            TrackerContainer.RangeEndDate = endDate;
            TrackerContainer.Invalidate();
            if (TrackerContainer.RangeEndDate < DateTime.MaxValue)
            {
                Int32 right = TrackerContainer.TicksToX(TrackerContainer.RangeEndDate.Ticks);

                if (TrackerContainer.RangeStartDate != DateTime.MinValue)
                {
                    //range's width, if "too~~ small", split start and end to both side
                    double width = TrackerContainer.TicksToWidth(TrackerContainer.RangeEndDate.Ticks - TrackerContainer.RangeStartDate.Ticks) / 2.0;
                    if (width < rangeRightPanel.Width)
                    {
                        right = Convert.ToInt32(right - width + rangeRightPanel.Width);
                    }
                }

                rangeRightPanel.Location = new Point(
                    Math.Min(Math.Max(right - rangeRightPanel.Width, rangeRightPanel.Width + devicePanel.Width), trackPanel.Width - rangeRightPanel.Width),
                    rangeRightPanel.Location.Y);
                rangeRightPanel.BackColor = _activateRangeColor;
            }
            else
            {
                rangeRightPanel.Location = new Point(trackPanel.Width - rangeRightPanel.Width, rangeRightPanel.Location.Y);
                rangeRightPanel.BackColor = _deactivateRangeColor;
            }
        }

        protected void RangeLeftPanelMouseDown(Object sender, MouseEventArgs e)
        {
            trackPanel.Capture = true;
            trackPanel.MouseMove -= RangeLeftMouseMove;
            trackPanel.MouseMove += RangeLeftMouseMove;
            trackPanel.MouseUp -= RangeLeftMouseUp;
            trackPanel.MouseUp += RangeLeftMouseUp;
        }

        protected static String ExportStartChangeXml(String startTime)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartTime", startTime));

            return xmlDoc.InnerXml;
        }

        protected static String ExportEndChangeXml(String endTime)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndTime", endTime));

            return xmlDoc.InnerXml;
        }

        protected void RangeRightPanelMouseDown(Object sender, MouseEventArgs e)
        {
            trackPanel.Capture = true;
            trackPanel.MouseMove -= RangeRightMouseMove;
            trackPanel.MouseMove += RangeRightMouseMove;
            trackPanel.MouseUp -= RangeRightMouseUp;
            trackPanel.MouseUp += RangeRightMouseUp;
        }

        protected void RangeRightMouseMove(Object sender, MouseEventArgs e)
        {
            rangeRightPanel.Location = new Point(
                Math.Min(Math.Max(e.X - rangeRightPanel.Width / 2, rangeLeftPanel.Location.X + rangeLeftPanel.Width), trackPanel.Width - rangeRightPanel.Width),
                rangeRightPanel.Location.Y);

            TrackerContainer.ExportRangeEnd(rangeRightPanel.Location.X + rangeRightPanel.Width, trackPanel.Width);

            rangeRightPanel.BackColor = (TrackerContainer.RangeEndDate == DateTime.MaxValue)
                                            ? _deactivateRangeColor
                                            : _activateRangeColor;

            if (OnExportEndDateTimeChange != null)
            {
                if (TrackerContainer.RangeEndDate != DateTime.MaxValue)
                    OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                            DateTimes.ToUtcString(TrackerContainer.RangeEndDate, Server.Server.TimeZone))));
                else
                    OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMaxDateTime, Server.Server.TimeZone))));
            }
        }

        protected void RangeRightMouseUp(Object sender, MouseEventArgs e)
        {
            trackPanel.MouseMove -= RangeRightMouseMove;
            trackPanel.MouseUp -= RangeRightMouseUp;
        }

        protected void RangeLeftMouseMove(Object sender, MouseEventArgs e)
        {
            rangeLeftPanel.Location = new Point(
                Math.Max(Math.Min(e.X - rangeLeftPanel.Width / 2, rangeRightPanel.Location.X - rangeLeftPanel.Width), devicePanel.Width),
                rangeLeftPanel.Location.Y);

            TrackerContainer.ExportRangeStart(rangeLeftPanel.Location.X, devicePanel.Width);

            rangeLeftPanel.BackColor = (TrackerContainer.RangeStartDate == DateTime.MinValue)
                                            ? _deactivateRangeColor
                                            : _activateRangeColor;

            if (OnExportStartDateTimeChange != null)
            {
                if (TrackerContainer.RangeStartDate != DateTime.MinValue)
                    OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.RangeStartDate, Server.Server.TimeZone))));
                else
                    OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            }
        }

        protected void RangeLeftMouseUp(Object sender, MouseEventArgs e)
        {
            trackPanel.MouseMove -= RangeLeftMouseMove;
            trackPanel.MouseUp -= RangeLeftMouseUp;
        }

        protected void SetStartButtonMouseClick(Object sender, MouseEventArgs e)
        {
            SetRangeStartDate(TrackerContainer.DateTime);

            if (OnExportStartDateTimeChange != null)
            {
                OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                    DateTimes.ToUtcString(TrackerContainer.RangeStartDate, Server.Server.TimeZone))));
            }

            if (TrackerContainer.DateTime >= TrackerContainer.RangeEndDate)
            {
                SetRangeEndDate(DateTime.MaxValue);
                SetRangeStartDate(TrackerContainer.DateTime);
                if (OnExportEndDateTimeChange != null)
                    OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMaxDateTime, Server.Server.TimeZone))));
            }
        }

        protected void SetEndButtonMouseClick(Object sender, MouseEventArgs e)
        {
            SetRangeEndDate(TrackerContainer.DateTime);

            if (OnExportEndDateTimeChange != null)
            {
                if (TrackerContainer.RangeEndDate != DateTime.MaxValue)
                    OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.RangeEndDate, Server.Server.TimeZone))));
            }

            if (TrackerContainer.DateTime <= TrackerContainer.RangeStartDate)
            {
                SetRangeStartDate(DateTime.MinValue);
                SetRangeEndDate(TrackerContainer.DateTime);
                if (OnExportStartDateTimeChange != null)
                    OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            }
        }

        protected void ClearSelectionButtonMouseClick(Object sender, MouseEventArgs e)
        {
            ClearSelection();
        }

        private void ClearSelection()
        {
            SetRangeStartDate(DateTime.MinValue);
            SetRangeEndDate(DateTime.MaxValue);

            if (OnExportStartDateTimeChange != null)
            {
                OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                    DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            }

            if (OnExportEndDateTimeChange != null)
            {
                OnExportEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(
                        DateTimes.ToUtcString(TrackerContainer.VisibleMaxDateTime, Server.Server.TimeZone))));
            }
        }
    }
}
