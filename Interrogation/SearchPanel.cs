using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using App;
using Constant;
using Device;
using Interface;
using Investigation.SaveReport;
using Microsoft.Reporting.WinForms;

namespace Interrogation
{
    public partial class SearchPanel : Investigation.EventSearch.SearchPanel
    {
        public String SearchName { get; set; }
        public String NumberofRecord { get; set; }

        public SearchPanel()
        {
            InitializeComponent();
        }

        protected new delegate void SmartSearchCompleteDelegate(Object sender, EventArgs e);
        public override void SmartSearchComplete(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new SmartSearchCompleteDelegate(SmartSearchComplete), sender, e);
                return;
            }

            if (_searchingCamera != null)
            {
                _searchingCamera.OnSmartSearchResult -= SmartSearchResult;
                _searchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
            }

            if (CheckIfNeedSearchNextCamera())
                return;

            _watch.Stop();

            var tmpResult = new List<CameraEvents>(_searchResult);
            _searchResult.Clear();

            foreach (var cameraEvent in tmpResult)
            {
                // cameraEvent.Status = "<Data><Action>STOP</Action><Name>12345678901234567890123456789012345678901234567890</Name><DateTime>1416823639710</DateTime><NoOfRecord>123456</NoOfRecord></Data>"
                if (!cameraEvent.Status.StartsWith("<Data>")) continue;
                
                var xmlDoc = Xml.LoadXml(cameraEvent.Status);

                var name = Xml.GetFirstElementValueByTagName(xmlDoc, "Name");
                var record = Xml.GetFirstElementValueByTagName(xmlDoc, "NoOfRecord");

                var bMatch = true;

				if (SearchName != "")
				{
					if (name.IndexOf(SearchName) < 0)
						bMatch = false;	
				}

				if (NumberofRecord != "")
				{
					if (record.IndexOf(NumberofRecord) < 0)
						bMatch = false;
				}

                if (bMatch)
                {
                    cameraEvent.Timecode += 1000;
                    cameraEvent.DateTime = cameraEvent.DateTime.AddSeconds(1);
                    _searchResult.Add(cameraEvent);
                }
            }


            //sort last time
            //_searchResult.Sort((x, y) => (String.Compare(x.Timecode.ToString(), y.Timecode.ToString())));
            _searchResult.Sort((x, y) => (Compare(x, y)));

            //get result to display
            UpdateResult();
        }

        protected override int Compare(CameraEvents x, CameraEvents y)
        {
            var xEvent = Xml.LoadXml(x.Status);
            var yEvent = Xml.LoadXml(y.Status);

            var xRecord = Xml.GetFirstElementValueByTagName(xEvent, "NoOfRecord");
            var yRecord = Xml.GetFirstElementValueByTagName(yEvent, "NoOfRecord");

            var ret = String.Compare(xRecord, yRecord);

            if (ret != 0)
                return ret;
            else 
            {
                var xName = Xml.GetFirstElementValueByTagName(xEvent, "Name");
                var yName = Xml.GetFirstElementValueByTagName(yEvent, "Name");

                ret = String.Compare(xName, yName);

                if (ret != 0)
                    return ret;
                else
                {
                    if (x.Timecode < y.Timecode) return -1;
                    if (x.Timecode > y.Timecode) return 1;
                }
            }

            return 0;
        }

        private readonly List<EventResultPanel> _recycleEventResult = new List<EventResultPanel>();

        protected override Investigation.EventSearch.EventResultPanel GetEventResultPanel()
        {
            EventResultPanel eventResultPanel = null;
            if (_recycleEventResult.Count > 0)
            {
                foreach (EventResultPanel resultPanel in _recycleEventResult)
                {
                    if (resultPanel.IsLoadingImage) continue;

                    eventResultPanel = resultPanel;
                    break;
                }

                if (eventResultPanel != null)
                {
                    _recycleEventResult.Remove(eventResultPanel);
                    return eventResultPanel;
                }
            }

            eventResultPanel = new EventResultPanel
            {
                App = App,
                NVR = NVR,
                SearchPanel = this,
            };
            eventResultPanel.OnPlayback += EventResultPanelOnPlayback;

            return eventResultPanel;
        }

        protected override void ShowReportForm(List<CameraEvents> resultList)
		{
			var saveReportForm = new SaveReportForm
			{
				Icon = NVR.Form.Icon,
				Text = Localization["Investigation_SaveReport"],
				Size = new Size(750, 700)
			};

			ReportViewer reportViewer = null;
			if(CMS != null)
			{
				reportViewer = new ReportViewer
				{
					TimeZone = NVR.Server.TimeZone,
					ShowZoomControl = false
				};
				reportViewer.ParseEventList(resultList);
			}else
			{
				reportViewer = new ReportViewer
				{
					TimeZone = NVR.Server.TimeZone,
					ShowZoomControl = false
				};
				reportViewer.ParseEventList(resultList);
			}

			saveReportForm.Controls.Add(reportViewer);
			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}
    }
}
