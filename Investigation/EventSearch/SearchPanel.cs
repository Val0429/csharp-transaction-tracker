using Constant;
using Device;
using DeviceConstant;
using Interface;
using Investigation.SaveReport;
using Microsoft.Reporting.WinForms;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using ApplicationForms = PanelBase.ApplicationForms;
using Manager = SetupBase.Manager;

namespace Investigation.EventSearch
{
	public partial class SearchPanel : UserControl
	{
		public IApp App;
		public ICMS CMS;
		public INVR NVR;
		public CameraEventSearchCriteria SearchCriteria;
		public Dictionary<String, String> Localization;
		//                                    result       click event time start       end
		public event EventHandler<EventArgs<List<CameraEvents>, DateTime, UInt64, UInt64>> OnPlayback;

		private readonly PageSelector _pageSelector;
		public SearchPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"Investigation_SaveReport", "Save Report"},

								   {"Investigation_SearchResult", "Search Result:"},
								   {"Investigation_SearchNoResult", "No Result Found"},
								   {"Investigation_SearchResultFound", "%1 Result Found"},
								   {"Investigation_SearchResultsFound", "%1 Results Found"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.None;

			_pageSelector = new PageSelector();
			_pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
			pagePanel.Controls.Add(_pageSelector);

			resultLabel.Text = Localization["Investigation_SearchResult"];
			foundLabel.Text = "";

			SearchCriteria = new CameraEventSearchCriteria();
		}

		private readonly List<EventResultPanel> _recycleEventResult = new List<EventResultPanel>();

	    protected virtual EventResultPanel GetEventResultPanel()
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

	    protected Boolean _isSearching;

	    protected readonly Stopwatch _watch = new Stopwatch();
	    protected Int32 _page;
		private const UInt16 CountPerPage = 20;

	    protected readonly Queue<ICamera> _queueSearchEventCamera = new Queue<ICamera>();
	    private int _TotalSearchCamera = 0;

	    protected ICamera _searchingCamera;
		public virtual void SearchEvent()
		{
			if (_isSearching) return;
			
			_page = 1;

			ClearViewModel();

			//refresh start and end
			if (SearchCriteria.DateTimeSet != DateTimeSet.None)
			{
				var range = DateTimes.UpdateStartAndEndDateTime(NVR.Server.DateTime, NVR.Server.TimeZone, SearchCriteria.DateTimeSet);
				SearchCriteria.StartDateTime = range[0];
				SearchCriteria.EndDateTime = range[1];
			}

			_queueSearchEventCamera.Clear();
		    _TotalSearchCamera = 0;

			if(CMS != null)
			{
				foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
				{
					if(!CMS.NVRManager.NVRs.ContainsKey(nvrDevice.NVRId)) continue;
					var nvr = CMS.NVRManager.NVRs[nvrDevice.NVRId];
					var device = nvr.Device.FindDeviceById(nvrDevice.DeviceId);
					if (device == null) continue;

					var camera = device as ICamera;
					if (camera == null) continue;

				    _TotalSearchCamera++;
					_queueSearchEventCamera.Enqueue(camera);
				}
			}
			else
			{
				foreach (var deviceId in SearchCriteria.Device)
				{
					var device = NVR.Device.FindDeviceById(deviceId);
					if (device == null) continue;

					var camera = device as ICamera;
					if (camera == null) continue;

                    _TotalSearchCamera++;
					_queueSearchEventCamera.Enqueue(camera);
				}
			}

			//do search
			_watch.Reset();
			_searchResult.Clear();
            
			if (_queueSearchEventCamera.Count > 0)
			{
				//ApplicationForms.ShowLoadingIcon(NVR.Form);
			    ApplicationForms.ProgressBarValue = 0;
			    ApplicationForms.ShowProgressBar(NVR.Form);

				_isSearching = true;
				_watch.Start();

				CheckIfNeedSearchNextCamera();
			}
			else //nothing to search
			{
				//get result to display
				UpdateResult();
			}
		}

	    protected virtual Boolean CheckIfNeedSearchNextCamera()
		{
			if (_queueSearchEventCamera.Count == 0)
			{
				_searchingCamera = null;
				return false;
			}

            ApplicationForms.ProgressBarValue = (_TotalSearchCamera - _queueSearchEventCamera.Count) * 100 / _TotalSearchCamera;

			_searchingCamera = _queueSearchEventCamera.Dequeue();
			_searchingCamera.OnSmartSearchResult -= SmartSearchResult;
			_searchingCamera.OnSmartSearchResult += SmartSearchResult;
			_searchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
			_searchingCamera.OnSmartSearchComplete += SmartSearchComplete;

			EventSearchDelegate eventSearchDelegate = _searchingCamera.EventSearch;                                                                    //period callback  delegate
			eventSearchDelegate.BeginInvoke(SearchCriteria.StartDateTime, SearchCriteria.EndDateTime, SearchCriteria.Event, null, null, null);

			return true;
		}

		private delegate void SmartSearchResultDelegate(Object sender, EventArgs<String> e);
		public void SmartSearchResult(Object sender, EventArgs<String> e)
		{
			if (!_isSearching) return;

			if (InvokeRequired)
			{
				Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
				return;
			}

			var xmlDoc = Xml.LoadXml(e.Value);

			var rootNode = Xml.GetFirstElementByTagName(xmlDoc, "SmartSearch");
			var deviceIdStr = rootNode.GetAttribute("Id");
			if (String.IsNullOrEmpty(deviceIdStr)) return;

			var deviceId = Convert.ToUInt16(deviceIdStr);
			IDevice device;
			if (CMS != null)
			{
				var nvrIdStr = rootNode.GetAttribute("nvrId");
				if (String.IsNullOrEmpty(nvrIdStr)) return;
				var nvrId = Convert.ToUInt16(nvrIdStr);
				if (!CMS.NVRManager.NVRs.ContainsKey(nvrId)) return;
				var nvr = CMS.NVRManager.NVRs[nvrId];
				device = nvr.Device.FindDeviceById(deviceId);
				if (device == null) return;
			}
			else
			{
				device = NVR.Device.FindDeviceById(deviceId);
				if (device == null) return;
			}

			var times = xmlDoc.GetElementsByTagName("Time");

			//Parse search result))
			foreach (XmlElement time in times)
			{
				var cameraEvent = new CameraEvents();
				_searchResult.Add(cameraEvent);

				var id = time.GetAttribute("id");
				cameraEvent.Device = device;
				cameraEvent.Type = (EventType)Enum.Parse(typeof(EventType), time.GetAttribute("type"), true);
				
				if(!String.IsNullOrEmpty(id))
					cameraEvent.Id = Convert.ToUInt16(id);

				cameraEvent.Value = (time.GetAttribute("value") == "true");
				cameraEvent.Timecode = Convert.ToUInt64(time.InnerText);
				cameraEvent.DateTime = DateTimes.ToDateTime(cameraEvent.Timecode, NVR.Server.TimeZone);
			    cameraEvent.Status = time.GetAttribute("status");
			}

			//_searchResult.Sort((x, y) => (String.Compare(x.Timecode.ToString(), y.Timecode.ToString())));
		}

	    protected delegate void SmartSearchCompleteDelegate(Object sender, EventArgs e);
		public virtual void SmartSearchComplete(Object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new SmartSearchCompleteDelegate(SmartSearchComplete), sender, e);
				return;
			}

		    ApplicationForms.ProgressBarValue = 99;

			if (_searchingCamera != null)
			{
				_searchingCamera.OnSmartSearchResult -= SmartSearchResult;
				_searchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
			}

			if (CheckIfNeedSearchNextCamera())
				return;

			_watch.Stop();

			//sort last time
			//_searchResult.Sort((x, y) => (String.Compare(x.Timecode.ToString(), y.Timecode.ToString())));
			_searchResult.Sort((x, y) => (Compare(x, y)));

			//get result to display
			UpdateResult();
		}

	    protected virtual int Compare(CameraEvents x, CameraEvents y)
		{
			if (x.Timecode < y.Timecode) return -1;
			if (x.Timecode > y.Timecode) return 1;

			return 0;
		}

	    protected delegate void EventSearchDelegate(UInt64 startTime, UInt64 endTime, List<EventType> events, List<UInt64> period);

	    protected readonly List<CameraEvents> _searchResult = new List<CameraEvents>();

	    protected void UpdateResult()
		{
			if (_searchResult.Count == 0)
			{
				foundLabel.Text = Localization["Investigation_SearchNoResult"];
					 //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
			}
			else
			{
				_pageSelector.Pages = Convert.ToUInt16(Math.Ceiling(_searchResult.Count / (CountPerPage * 1.0)));
				_pageSelector.SelectPage = _page;
				_pageSelector.ShowPages();

				foundLabel.Text = ((_searchResult.Count == 1)
							? Localization["Investigation_SearchResultFound"]
							: Localization["Investigation_SearchResultsFound"]).Replace("%1", _searchResult.Count.ToString());

				//foundLabel.Text += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

				ChangeDisplayPage();
			}

			_isSearching = false;

			//ApplicationForms.HideLoadingIcon();
            ApplicationForms.HideProgressBar();

			containerPanel.AutoScroll = false;
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
			containerPanel.AutoScroll = true;
		}

		private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
		{
			_page = _pageSelector.SelectPage;

			ClearViewModel();
			ChangeDisplayPage();
		}

		private void ChangeDisplayPage()
		{
			var count = 1;

			var index = (_page - 1)*CountPerPage;
			var result = (index + CountPerPage <= _searchResult.Count)
							  ? _searchResult.GetRange(index, CountPerPage)
							  : _searchResult.GetRange(index, _searchResult.Count - index);

			foreach (var cameraEvent in result)
			{
				var resultPanel = GetEventResultPanel();
				resultPanel.Id = Convert.ToInt32(count + ((_page - 1) * CountPerPage));
				resultPanel.CameraEvent = cameraEvent;

				containerPanel.Controls.Add(resultPanel);
				resultPanel.BringToFront();
				count++;
			}

			var resultTitlePanel = GetEventResultPanel();
			resultTitlePanel.IsTitle = true;
			resultTitlePanel.Cursor = Cursors.Default;
			containerPanel.Controls.Add(resultTitlePanel);
		}

	    protected virtual void EventResultPanelOnPlayback(Object sender, EventArgs e)
		{
			var panel = sender as EventResultPanel;
			if (panel == null || panel.CameraEvent == null) return;

			var list = new List<CameraEvents>();
			foreach (var cameraEvent in _searchResult)
			{
				if(cameraEvent.Device != panel.CameraEvent.Device) continue;
				if(cameraEvent.Type != panel.CameraEvent.Type) continue;

				list.Add(cameraEvent);
			}

			if(OnPlayback != null)
				OnPlayback(this, new EventArgs<List<CameraEvents>, DateTime, UInt64, UInt64>(
					list,
					panel.CameraEvent.DateTime,
					SearchCriteria.StartDateTime,
					SearchCriteria.EndDateTime));
		}

		public void ClearResult()
		{
			_pageSelector.ClearViewModel();
			foundLabel.Text = "";

			ClearViewModel();
		}

		public void ClearViewModel()
		{
			if (containerPanel.Controls.Count == 0) return;

			foreach (EventResultPanel control in containerPanel.Controls)
			{
				control.Reset();

				if (!_recycleEventResult.Contains(control))
					_recycleEventResult.Add(control);
			}

			containerPanel.Controls.Clear();
		}

		//---------------------------------------------------------------------------------------------------------------------

		private const UInt16 MaximumConnection = 3;
		private UInt16 _connection;
		public List<EventResultPanel> QueueSearchResultPanel = new List<EventResultPanel>();
		public void QueueLoadSnapshot(EventResultPanel eventResultPanel)
		{
			if (eventResultPanel.CameraEvent.Device == null) return;

			if (_connection < MaximumConnection)
			{
				if (QueueSearchResultPanel.Contains(eventResultPanel))
					QueueSearchResultPanel.Remove(eventResultPanel);

				_connection++;
				LoadSnapshotDelegate loadSnapshotDelegate = eventResultPanel.LoadSnapshot;
				loadSnapshotDelegate.BeginInvoke(LoadSnapshotCallback, loadSnapshotDelegate);

				return;
			}

			if (!QueueSearchResultPanel.Contains(eventResultPanel))
				QueueSearchResultPanel.Add(eventResultPanel);
		}

		private delegate void LoadSnapshotDelegate();
		private void LoadSnapshotCallback(IAsyncResult result)
		{
			((LoadSnapshotDelegate)result.AsyncState).EndInvoke(result);
			_connection--;

			if (QueueSearchResultPanel.Count > 0)
				QueueLoadSnapshot(QueueSearchResultPanel[0]);
		}
		
		//---------------------------------------------------------------------------------------------------------------------
		public void SaveReport()
		{
			if (_searchResult == null) return;

			ApplicationForms.ShowLoadingIcon(NVR.Form);
			Application.RaiseIdle(null);

			ShowReportForm(_searchResult);
			ApplicationForms.HideLoadingIcon();
		}

	    protected virtual void ShowReportForm(List<CameraEvents> resultList)
		{
			var saveReportForm = new SaveReportForm
			{
				Icon = NVR.Form.Icon,
				Text = Localization["Investigation_SaveReport"],
				Size = new Size(640, 700)
			};

			ReportViewer reportViewer = null;
			if(CMS != null)
			{
				reportViewer = new CMSInvestigationReportViewer
				{
					TimeZone = NVR.Server.TimeZone,
					ShowZoomControl = false
				};
				((CMSInvestigationReportViewer)reportViewer).ParseEventList(resultList);
			}else
			{
				reportViewer = new InvestigationReportViewer
				{
					TimeZone = NVR.Server.TimeZone,
					ShowZoomControl = false
				};
				((InvestigationReportViewer)reportViewer).ParseEventList(resultList);
			}

			saveReportForm.Controls.Add(reportViewer);
			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}
	}
}
