using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Device;
using Interface;
using PanelBase;

namespace SmartSearch
{
    public partial class SearchResult : UserControl, IControl, IServerUse//, IMouseHandler
    {
        //public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler OnMinimizeChange;

        private readonly System.Timers.Timer _dotTimer = new System.Timers.Timer();
        private readonly Stopwatch _watch = new Stopwatch();

        public Boolean IsMinimize = true;
        public Dictionary<String, String> Localization;

        public IServer Server { get; set; }
        public String TitleName { get; set; }

        protected static readonly List<SearchResultPanel> RecycleSearchResultPanel = new List<SearchResultPanel>();

        private PictureBox _smallSize;
        private PictureBox _mediumSize;
        private PictureBox _largeSize;
        private static readonly Image _mini = Resources.GetResources(Properties.Resources.mini, Properties.Resources.IMGMini);
        private static readonly Image _mini2 = Resources.GetResources(Properties.Resources.mini2, Properties.Resources.IMGMini2);
        protected ICamera _camera;

        private Int32 _switchPageWidth;
        private Int32 _switchPageHeight;

        private PageSelector _pageSelector;

        public SearchResult()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_SearchResult", "Search Result"},

								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Information", "Information"},
								   {"SearchResult_Searching", "Searching"},
								   {"SearchResult_NoResult", "Search Result : No Result Found"},
								   {"SearchResult_ResultFound", "Search Result : %1 Result Found"},
								   {"SearchResult_ResultsFound", "Search Result : %1 Results Found"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            _switchPageWidth = nextPageButton.Width;
            _switchPageHeight = pagePanel.Height;

            nextPageButton.Width = previousPageButton.Width = 0;
            pagePanel.Height = 0;

            minimizePictureBox.Image = _mini;

            titlePanel.Paint += TitlePanelPaint;
            minimizePictureBox.MouseClick += MinimizePictureBoxClick;

            _pageSelector = new PageSelector
            {
                BackColor = Color.Transparent,
                NormalColor = Color.WhiteSmoke,
                SelectColor = Color.FromArgb(70, 170, 206),
                PageLabelHeight = _switchPageHeight,
            };

            _pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
            pagePanel.Controls.Add(_pageSelector);
            pagePanel.Paint += PagePanelPaint;
        }

        private String _titleText;
        private readonly Font _font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly Pen _borderPen = new Pen(Color.FromArgb(30, 32, 37));

        private void PagePanelPaint(Object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawLine(_borderPen, 0, 0, pagePanel.Width, 0);
        }

        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(_titleText, _font, Brushes.White, 18, 11);
        }

        public void Initialize()
        {
            _titleText = TitleName = Localization["Control_SearchResult"];
            titlePanel.Invalidate();

            _dotTimer.Elapsed += DotSearchLabel;
            _dotTimer.Interval = 500;
            _dotTimer.SynchronizingObject = Server.Form;

            _smallSize = new PictureBox
            {
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Size = new Size(25, 25),
                BackColor = Color.Transparent,
                BackgroundImage = _small,
                BackgroundImageLayout = ImageLayout.Center
            };
            _smallSize.MouseClick += ChangeToSmallSize;
            titlePanel.Controls.Add(_smallSize);
            SharedToolTips.SharedToolTip.SetToolTip(_smallSize, "QQVGA" + Environment.NewLine + "160x120");

            _mediumSize = new PictureBox
            {
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Size = new Size(25, 25),
                BackColor = Color.Transparent,
                BackgroundImage = _mediumactivate,
                BackgroundImageLayout = ImageLayout.Center
            };
            _mediumSize.MouseClick += ChangeToMediumSize;
            titlePanel.Controls.Add(_mediumSize);
            SharedToolTips.SharedToolTip.SetToolTip(_mediumSize, "HQVGA" + Environment.NewLine + "240x160");

            _largeSize = new PictureBox
            {
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Size = new Size(25, 25),
                BackColor = Color.Transparent,
                BackgroundImage = _large,
                BackgroundImageLayout = ImageLayout.Center
            };
            _largeSize.MouseClick += ChangeToLargeSize;
            titlePanel.Controls.Add(_largeSize);
            SharedToolTips.SharedToolTip.SetToolTip(_largeSize, "QVGA" + Environment.NewLine + "320x240");

            snapshotFlowLayoutPanel.MouseClick += SnapshotLlowLayoutPanelMouseClick;
            snapshotFlowLayoutPanel.SizeChanged += SnapshotLlowLayoutPanelSizeChanged;

            nextPageButton.Image = Resources.GetResources(Properties.Resources.nextPage, Properties.Resources.IMGNextPage);
            previousPageButton.Image = Resources.GetResources(Properties.Resources.previousPage, Properties.Resources.IMGPreviousPage);
        }

        private void SnapshotLlowLayoutPanelMouseClick(Object sender, MouseEventArgs e)
        {
            snapshotFlowLayoutPanel.Focus();
        }

        protected String _size = "HQVGA";

        private static readonly Image _small = Resources.GetResources(Properties.Resources.small, Properties.Resources.IMGSmall);
        private static readonly Image _smallactivate = Resources.GetResources(Properties.Resources.small_activate, Properties.Resources.IMGSmall_activate);
        private static readonly Image _medium = Resources.GetResources(Properties.Resources.medium, Properties.Resources.IMGMedium);
        private static readonly Image _mediumactivate = Resources.GetResources(Properties.Resources.medium_activate, Properties.Resources.IMGMedium_activate);
        private static readonly Image _large = Resources.GetResources(Properties.Resources.large, Properties.Resources.IMGLarge);
        private static readonly Image _largeactivate = Resources.GetResources(Properties.Resources.large_activate, Properties.Resources.IMGLarge_activate);

        private void ChangeToSmallSize(Object sender, MouseEventArgs e)
        {
            if (_size == "QQVGA") return;

            _size = "QQVGA";
            _smallSize.BackgroundImage = _smallactivate;
            _mediumSize.BackgroundImage = _medium;
            _largeSize.BackgroundImage = _large;

            snapshotFlowLayoutPanel.AutoScrollPosition = new Point(0, 0);

            snapshotFlowLayoutPanel.Visible = false;
            foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
            {
                panel.SmallSize();
            }
            snapshotFlowLayoutPanel.Visible = true;
        }

        private void ChangeToMediumSize(Object sender, MouseEventArgs e)
        {
            if (_size == "HQVGA") return;

            _size = "HQVGA";
            _smallSize.BackgroundImage = _small;
            _mediumSize.BackgroundImage = _mediumactivate;
            _largeSize.BackgroundImage = _large;

            snapshotFlowLayoutPanel.AutoScrollPosition = new Point(0, 0);

            snapshotFlowLayoutPanel.Visible = false;
            foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
            {
                panel.MediumSize();
            }
            snapshotFlowLayoutPanel.Visible = true;
        }

        private void ChangeToLargeSize(Object sender, MouseEventArgs e)
        {
            if (_size == "QVGA") return;

            _size = "QVGA";
            _smallSize.BackgroundImage = _small;
            _mediumSize.BackgroundImage = _medium;
            _largeSize.BackgroundImage = _largeactivate;

            snapshotFlowLayoutPanel.AutoScrollPosition = new Point(0, 0);

            snapshotFlowLayoutPanel.Visible = false;
            foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
            {
                panel.LargeSize();
            }
            snapshotFlowLayoutPanel.Visible = true;
        }

        private delegate void SmartSearchCompleteDelegate(Object sender, EventArgs e);
        public void SmartSearchComplete(Object sender, EventArgs e)
        {
            if (!_isSearch) return;

            if (InvokeRequired)
            {
                Invoke(new SmartSearchCompleteDelegate(SmartSearchComplete), sender, e);
                return;
            }
            _dotTimer.Enabled = false;
            _watch.Stop();

            if (_watch.ElapsedMilliseconds == 0)
            {
                _titleText = Localization["Control_SearchResult"];
                titlePanel.Invalidate();
                return;
            }

            _isSearch = false;

            if (_camera == null)
            {
                _titleText = Localization["Control_SearchResult"];

                pagePanel.Height = 0;
                titlePanel.Invalidate();
                return;
            }

            if (_found == 0)
            {
                _titleText = Localization["SearchResult_NoResult"];
                //+ @" " + WatchElapsedToString();

                foreach (SearchResultPanel searchResultPanel in snapshotFlowLayoutPanel.Controls)
                {
                    searchResultPanel.Reset();
                    RecycleSearchResultPanel.Add(searchResultPanel);
                }

                snapshotFlowLayoutPanel.Controls.Clear();
            }
            else
            {
                _titleText = ((_found == 1)
                    ? Localization["SearchResult_ResultFound"].Replace("%1", _found.ToString())
                                  : Localization["SearchResult_ResultsFound"].Replace("%1", _found.ToString()));

                //sort last time
                //_sortedList.Sort((x, y) => (String.Compare(x.Timecode.ToString(), y.Timecode.ToString())));
                _sortedList.Sort((x, y) => (Compare(x, y)));

                UpdatePageContent();

                SetCurrentPage();
            }
            titlePanel.Invalidate();
        }

        private int Compare(CameraEvent x, CameraEvent y)
        {
            if (x.Timecode < y.Timecode) return -1;
            if (x.Timecode > y.Timecode) return 1;

            return 0;
        }

        private void UpdatePageContent()
        {
            PageCount = Convert.ToUInt16(Math.Ceiling(_found * 1.0 / NumPerPage));

            if (PageCount > 1)
            {
                pagePanel.Height = _switchPageHeight;

                PageIndex = 1;
                _pageSelector.Pages = PageCount;
                _pageSelector.SelectPage = 1;
                _pageSelector.ShowPages();
            }
            else
            {
                _pageSelector.ClearViewModel();

                pagePanel.Height = 0;
            }
        }

        protected readonly List<CameraEvent> _sortedList = new List<CameraEvent>();
        protected const UInt16 NumPerPage = 20;//25
        protected Int32 _found;

        protected delegate void SmartSearchResultDelegate(Object sender, EventArgs<String> e);
        public virtual void SmartSearchResult(Object sender, EventArgs<String> e)
        {
            if (!_isSearch) return;

            if (InvokeRequired)
            {
                Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
                return;
            }

            var times = Xml.LoadXml(e.Value).GetElementsByTagName("Time");

            _found += times.Count;

            snapshotFlowLayoutPanel.Visible = false;

            foreach (XmlElement time in times)
            {
                AppendSearchResultPanel(_camera, Convert.ToUInt64(time.InnerText), time.GetAttribute("type"), time.GetAttribute("desc"));
            }

            snapshotFlowLayoutPanel.Visible = true;
        }

        private void AppendSearchResultPanel(ICamera camera, UInt64 timecode, String type, String desc)
        {
            _sortedList.Add(new CameraEvent { Camera = camera, Timecode = timecode, Type = type, Description = desc });
        }

        public void ApplyPlaybackParameter(List<CameraEvents> cameraList)
        {
            ClearResult();

            foreach (var cameraEvents in cameraList)
            {
                var camera = cameraEvents.Device as ICamera;
                if (camera == null) continue;
                _camera = camera;
                _found++;
                AppendSearchResultPanel(camera, cameraEvents.Timecode, cameraEvents.Type.ToString(), cameraEvents.ToLocalizationString());
            }

            _watch.Reset();
            if (_camera == null)
            {
                _titleText = Localization["Control_SearchResult"];

                pagePanel.Height = 0;
                titlePanel.Invalidate();
                return;
            }

            if (_found == 0)
                _titleText = Localization["SearchResult_NoResult"];
            else
            {
                _titleText = ((_found == 1)
                    ? Localization["SearchResult_ResultFound"].Replace("%1", _found.ToString())
                                  : Localization["SearchResult_ResultsFound"].Replace("%1", _found.ToString()));

                UpdatePageContent();

                SetCurrentPage();
            }
            titlePanel.Invalidate();
        }

        private String _dot = "";
        private void DotSearchLabel(Object sender, EventArgs e)
        {
            if (_dot.Length > 20)
                _dot = ".";

            _dot += ".";

            if (!_dotTimer.Enabled) return;

            if (_found == 0)
                _titleText = Localization["SearchResult_Searching"] + _dot;
            else
            {
                _titleText = ((_found == 1)
                                  ? Localization["SearchResult_ResultFound"].Replace("%1", _found.ToString())
                                  : Localization["SearchResult_ResultsFound"].Replace("%1", _found.ToString()))
                             + ((_watch.IsRunning) ? _dot : "");
            }
            titlePanel.Invalidate();
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            _camera = null;
            if (e.Value is ICamera)
                _camera = e.Value as ICamera;

            _titleText = Localization["Control_SearchResult"];
            _dotTimer.Enabled = false;
            titlePanel.Invalidate();
            ClearResult();
        }

        protected virtual SearchResultPanel GetSearchResultPanel()
        {
            SearchResultPanel searchResultPanel = null;
            if (RecycleSearchResultPanel.Count > 0)
            {
                foreach (SearchResultPanel panel in RecycleSearchResultPanel)
                {
                    if (panel.IsLoadingImage) continue;

                    searchResultPanel = panel;
                    break;
                }

                if (searchResultPanel != null)
                {
                    RecycleSearchResultPanel.Remove(searchResultPanel);
                    return searchResultPanel;
                }
            }

            searchResultPanel = new SearchResultPanel
            {
                SearchResult = this,
                Server = Server,
            };
            searchResultPanel.OnSelectionChange += SearchResultPanelOnSelectionChange;

            return searchResultPanel;
        }

        private const UInt16 MaximumConnection = 2;
        private UInt16 _connection;
        public List<SearchResultPanel> QueueSearchResultPanel = new List<SearchResultPanel>();
        public void QueueLoadSnapshot(SearchResultPanel searchResultPanel)
        {
            if (_connection < MaximumConnection)
            {
                _connection++;

                if (QueueSearchResultPanel.Contains(searchResultPanel))
                {
                    try
                    {
                        QueueSearchResultPanel.Remove(searchResultPanel);
                    }
                    catch (Exception exception)//exception before, still dont know why
                    {
                        Console.WriteLine(exception);
                    }
                }

                LoadSnapshotDelegate loadSnapshotDelegate = searchResultPanel.LoadSnapshot;
                loadSnapshotDelegate.BeginInvoke(LoadSnapshotCallback, loadSnapshotDelegate);

                return;
            }

            if (!QueueSearchResultPanel.Contains(searchResultPanel))
                QueueSearchResultPanel.Add(searchResultPanel);
        }

        private delegate void LoadSnapshotDelegate();
        private void LoadSnapshotCallback(IAsyncResult result)
        {
            ((LoadSnapshotDelegate)result.AsyncState).EndInvoke(result);
            _connection--;

            Thread.Sleep(300);

            if (QueueSearchResultPanel.Count > 0)
                QueueLoadSnapshot(QueueSearchResultPanel[0]);
        }

        private static String TimecodeChangeXml(String timestamp)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Timestamp", timestamp));

            return xmlDoc.InnerXml;
        }

        protected void SearchResultPanelOnSelectionChange(Object sender, EventArgs e)
        {
            if (OnTimecodeChange != null && sender is SearchResultPanel)
            {
                var panel = ((SearchResultPanel)sender);

                var timecode = panel.Timecode;
                if (panel.Camera.Server.Server.TimeZone != Server.Server.TimeZone)
                {
                    Int64 time = Convert.ToInt64(timecode);
                    time -= (Server.Server.TimeZone * 1000);
                    time += (panel.Camera.Server.Server.TimeZone * 1000);
                    timecode = Convert.ToUInt64(time);
                }

                OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(timecode.ToString(CultureInfo.InvariantCulture))));
            }
        }

        private Boolean _isSearch;
        public void SearchStart(Object sender, EventArgs e)
        {
            _isSearch = true;
            _found = 0;
            _watch.Reset();
            _watch.Start();
            _dot = "";
            _dotTimer.Enabled = true;
            _titleText = Localization["SearchResult_Searching"];
            titlePanel.Invalidate();
            ClearResult();
        }

        public void ClearResult()
        {
            foreach (SearchResultPanel searchResultPanel in snapshotFlowLayoutPanel.Controls)
            {
                searchResultPanel.Reset();
                RecycleSearchResultPanel.Add(searchResultPanel);
            }

            QueueSearchResultPanel.Clear();
            snapshotFlowLayoutPanel.Controls.Clear();
            _sortedList.Clear();

            _pageSelector.ClearViewModel();

            PageIndex = 1;
            _found = 0;
            _connection = 0;

            //nextPageButton.Width = previousPageButton.Width = 0;
            pagePanel.Height = 0;
        }

        private void SnapshotLlowLayoutPanelSizeChanged(Object sender, EventArgs e)
        {
            foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
            {
                panel.CheckVisible();
            }
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
            _dotTimer.Enabled = false;

            if (!_isSearch) return;
            _isSearch = false;

            _titleText = ((_found == 1)
                                       ? Localization["SearchResult_ResultFound"].Replace("%1", _found.ToString())
                              : Localization["SearchResult_ResultsFound"].Replace("%1", _found.ToString()));

            UpdatePageContent();
            titlePanel.Invalidate();
        }

        public Int32 PageCount = 1;
        public Int32 PageIndex = 1;
        private void PreviousPageButtonClick(Object sender, EventArgs e)
        {
            PageIndex--;

            if (PageIndex < 1)
                PageIndex = PageCount;

            _pageSelector.SelectPage = PageIndex;
            _pageSelector.ShowPages();

            SetCurrentPage();
        }

        private void NextPageButtonClick(Object sender, EventArgs e)
        {
            PageIndex++;

            if (PageIndex > PageCount)
                PageIndex = 1;

            _pageSelector.SelectPage = PageIndex;
            _pageSelector.ShowPages();

            SetCurrentPage();
        }

        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            PageIndex = e.Value;
            SetCurrentPage();
        }

        protected void SetCurrentPage()
        {
            if (PageIndex > PageCount)
                PageIndex = 1;
            else if (PageIndex < 1)
                PageIndex = PageCount;

            if (PageCount > 1)
            {
                pagePanel.Height = _switchPageHeight;
            }
            else
            {
                pagePanel.Height = 0;
            }

            snapshotFlowLayoutPanel.AutoScrollPosition = new Point(0, 0);

            snapshotFlowLayoutPanel.Visible = false;

            foreach (SearchResultPanel searchResultPanel in snapshotFlowLayoutPanel.Controls)
            {
                searchResultPanel.Reset();
                RecycleSearchResultPanel.Add(searchResultPanel);
            }
            snapshotFlowLayoutPanel.Controls.Clear();

            for (var i = (PageIndex - 1) * NumPerPage; i < (PageIndex) * NumPerPage; i++)
            {
                if (i >= _sortedList.Count) break;

                snapshotFlowLayoutPanel.Controls.Add(CreateSearchResultPanelByCameraEvent(_sortedList[i]));
            }

            switch (_size)
            {
                case "QQVGA":
                    foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
                        panel.SmallSize();
                    break;

                case "HQVGA":
                    foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
                        panel.MediumSize();
                    break;

                case "QVGA":
                    foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
                        panel.LargeSize();
                    break;
            }

            snapshotFlowLayoutPanel.Visible = true;
        }

        public SearchResultPanel CreateSearchResultPanelByCameraEvent(CameraEvent cameraEvent)
        {
            SearchResultPanel searchResultPanel = GetSearchResultPanel();

            searchResultPanel.Camera = cameraEvent.Camera;
            searchResultPanel.Timecode = cameraEvent.Timecode;

            searchResultPanel.Type = cameraEvent.Type;

            SharedToolTips.SharedToolTip.SetToolTip(searchResultPanel, cameraEvent.Description);

            if (snapshotFlowLayoutPanel.Controls.Count < NumPerPage)
            {
                switch (_size)
                {
                    case "QQVGA":
                        searchResultPanel.SmallSize();
                        break;

                    case "HQVGA":
                        searchResultPanel.MediumSize();
                        break;

                    case "QVGA":
                        searchResultPanel.LargeSize();
                        break;
                }
            }

            return searchResultPanel;
        }

        public void Minimize()
        {
            IsMinimize = true;
            minimizePictureBox.Image = _mini;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            IsMinimize = false;
            minimizePictureBox.Image = _mini2;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        private void MinimizePictureBoxClick(Object sender, MouseEventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
        }
    }

    public class CameraEvent
    {
        public ICamera Camera;
        public UInt64 Timecode;
        public String Type;
        public String Description;
    }
}
