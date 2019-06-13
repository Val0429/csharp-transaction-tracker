using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Constant.Utility;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupLog
{
    public partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        public String TitleName { get; set; }
        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        protected readonly List<LogType> _showLogTypes = new List<LogType>();
        //private ScrollBar _logScrollBar;

        private readonly System.Timers.Timer _loadLogTimer = new System.Timers.Timer();
        private readonly System.Timers.Timer _hideLoadingTimer = new System.Timers.Timer();
        //private readonly System.Timers.Timer _dotTimer = new System.Timers.Timer();
        //private readonly System.Timers.Timer _scrollLogTimer = new System.Timers.Timer();
        //private readonly System.Timers.Timer _resetScrollTimer = new System.Timers.Timer();

        private readonly PageSelector _pageSelector;
        private readonly Stopwatch _watch = new Stopwatch();

        public Setup()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_Log", "Log"},

                                   {"MessageBox_Information", "Information"},
                                   {"Common_UsedSeconds", "(%1 seconds)"},

                                   {"SetupLog_Log", "Log"},
                                   {"SetupLog_System", "System"},
                                   {"SetupLog_User", "User"},
                                   {"SetupLog_SearchResult", "Search Result:"},
                                   {"SetupLog_NoLogFound", "No Log Found"},
                                   {"SetupLog_LogFound", "%1  Log Found"},
                                   {"SetupLog_LogsFound", " %1  Logs Found"},

                                   {"SetupLog_AllLog", "All log"},
                                   {"SetupLog_NoSystemLog", "No system log"},
                                   {"SetupLog_NoUserLog", "No user log"},

                                   {"SetupLog_ID", "ID"},
                                   {"SetupLog_Time", "Time"},
                                   {"SetupLog_Description", "Description"},

                                   {"SetupLog_LoadingLog", "Loading Log"},
                                   {"SetupLog_DisplaySystemLog", "Display system log"},
                                   {"SetupLog_DisplayUserLog", "Display user operation log"},
                                   {"SetupLog_SaveAsComplete", "Save \"%1\" Completed."},
                                   {"SetupLog_SaveAsFailure", "Save Failure."},
                                   {"SetupLog_ExcelFile", "Excel file"},
                                   {"SetupLog_HTMLFile", "HTML file"},
                                   {"SetupLog_TextFile", "Text file"},
                                   {"SetupLog_XmlFile", "XML file"},
                                    {"SetupLog_PDFFile", "PDF file"},
								   //Excel file(*.xls) |*.xls|HTML file(*.html) |*.html|Text file(*.txt) |*.txt|XML file(*.xml) |*.xml
							   };
            Localizations.Update(Localization);

            Name = "Log";
            TitleName = Localization["Control_Log"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            BackgroundImage = Manager.Background;
            contentPanel.BackgroundImage = Manager.BackgroundNoBorder;

            logToolPanel.BackgroundImage = Resources.GetResources(Properties.Resources.tool_bg, Properties.Resources.IMGToolBg);
            serverPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.service_bg, Properties.Resources.IMGServiceBg);

            _pageSelector = new PageSelector
            {
                PagesDisplay = 10,
                Dock = DockStyle.Fill,
                //MinimumSize = new Size(450, 25),
                //AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent
            };
            _pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
            resultrPanel.Controls.Add(_pageSelector);
            _pageSelector.BringToFront();

            resultLabel.Text = Localization["SetupLog_SearchResult"];

            filterSystemLabel.Text = Localization["SetupLog_System"];
            filterUserLabel.Text = Localization["SetupLog_User"];
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Log"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------

            this.datePicker.CustomFormat = DateTimeConverter.GetDatePattern();
        }

        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            foundLabel.Text = "";

            _showLogTypes.Add(LogType.Server);
            //_showLogTypes.Add(LogType.Action);

            SharedToolTips.SharedToolTip.SetToolTip(serverPictureBox, Localization["SetupLog_DisplaySystemLog"]);
            SharedToolTips.SharedToolTip.SetToolTip(actionPictureBox, Localization["SetupLog_DisplayUserLog"]);

            //_dotTimer.Elapsed += DotLogLabel;
            //_dotTimer.Interval = 500;
            //_dotTimer.SynchronizingObject = Server.Form;

            _loadLogTimer.Elapsed += LoadLog;
            _loadLogTimer.Interval = 1000;
            _loadLogTimer.SynchronizingObject = Server.Form;

            _hideLoadingTimer.Elapsed += HideLoading;
            _hideLoadingTimer.Interval = 500;
            _hideLoadingTimer.SynchronizingObject = Server.Form;

            //_scrollLogTimer.Elapsed += ScrollLogIndex;
            //_scrollLogTimer.Interval = 150;
            //_scrollLogTimer.SynchronizingObject = Server.Form;

            //_resetScrollTimer.Elapsed += ResetScroll;
            //_resetScrollTimer.Interval = 350;
            //_resetScrollTimer.SynchronizingObject = Server.Form;

            datePicker.CloseUp += DatePickerCloseUp;
            datePicker.ValueChanged += DatePickerValueChanged;
            datePicker.DropDown += DatePickerDropDown;

            //_logScrollBar = new VScrollBar
            //{
            //    Dock = DockStyle.Right,
            //    Minimum = 0,
            //    Maximum = 50,
            //    Value = 0,
            //    Enabled = false
            //};
            //_logScrollBar.Scroll += ScrollDoubleBufferPanelScroll;
            //scrollDoubleBufferPanel.Controls.Add(_logScrollBar);

            //scrollDoubleBufferPanel.Scroll += ScrollDoubleBufferPanelScroll;

            //logDoubleBufferPanel.MouseWheel += ScrollDoubleBufferPanelMouseWheel;
            //logDoubleBufferPanel.MouseDown += ScrollDoubleBufferPanelMouseDown;

            logPanel.Paint += LogInputPanelPaint;
            logFilterPanel.Paint += LogFilterPanelPaint;

            contentPanel.Controls.Remove(titlePanel);
            titlePanel.Paint += TitlePanelPaint;
            //logDoubleBufferPanel.Paint += LogDoubleBufferPanelPaint;

            //logDoubleBufferPanel.SizeChanged += LogDoubleBufferPanelSizeChanged;
        }

        private Int32 _page = 1;
        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            _page = _pageSelector.SelectPage;

            logDoubleBufferPanel.Visible = false;

            ClearLog();
            UpdateLog();

            logDoubleBufferPanel.Visible = true;
        }

        public void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "SaveAs":
                    try
                    {
                        SaveAs();
                    }
                    catch (Exception exception)
                    {
                        TopMostMessageBox.Show(Localization["SetupLog_SaveAsFailure"] + Environment.NewLine + exception.Message,
                            Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        //private void ScrollDoubleBufferPanelMouseWheel(Object sender, MouseEventArgs e)
        //{
        //    if(e.Delta > 0)
        //    {
        //        _logScrollBar.Value = Math.Max(_logScrollBar.Value - 1, _logScrollBar.Minimum);
        //        _logIndex = (UInt16)Math.Max(_logIndex - 10, 0);
        //        logDoubleBufferPanel.Invalidate();
        //    }
        //    else
        //    {
        //        _logScrollBar.Value = Math.Min(_logScrollBar.Value + 1, _logScrollBar.Maximum);
        //        var index = Math.Max(_logs.Count - (logDoubleBufferPanel.Height / 40), 0);
        //        _logIndex = (UInt16)Math.Min(_logIndex + 10, index);
        //        logDoubleBufferPanel.Invalidate();
        //    }
        //}

        //private void ScrollDoubleBufferPanelMouseDown(Object sender, MouseEventArgs e)
        //{
        //    logDoubleBufferPanel.Focus();
        //}

        //private void LogDoubleBufferPanelSizeChanged(Object sender, EventArgs e)
        //{
        //    if (_logs.Count == 0)
        //    {
        //        _logScrollBar.Enabled = false;
        //        return;
        //    }

        //    _logScrollBar.Enabled = (_logs.Count > logDoubleBufferPanel.Height / 40 || _logIndex != 0);
        //}

        //private Int16 _moveLogSpeed;
        //private void ScrollDoubleBufferPanelScroll(Object sender, ScrollEventArgs e)
        //{
        //    _resetScrollTimer.Enabled = false;
        //    //Console.WriteLine(e.Type);
        //    if (e.Type == ScrollEventType.EndScroll)
        //    {
        //        _moveLogSpeed = 0;
        //        //_scrollLogTimer.Enabled = false;
        //        _resetScrollTimer.Enabled = true;
        //        return;
        //    }

        //    if (e.Type == ScrollEventType.SmallIncrement)
        //    {
        //        _logIndex = (UInt16)Math.Min(_logIndex + 1, _logs.Count - (logDoubleBufferPanel.Height / 40));
        //        logDoubleBufferPanel.Invalidate();
        //        return;
        //    }

        //    if (e.Type == ScrollEventType.SmallDecrement)
        //    {
        //        _logIndex = (UInt16)Math.Max(_logIndex - 1, 0);
        //        logDoubleBufferPanel.Invalidate();
        //        return;
        //    }

        //    if (e.Type == ScrollEventType.LargeIncrement)
        //    {
        //        _logIndex = (UInt16)Math.Min(_logIndex + 50, _logs.Count - (logDoubleBufferPanel.Height / 40));
        //        logDoubleBufferPanel.Invalidate();
        //        return;
        //    }

        //    if (e.Type == ScrollEventType.LargeDecrement)
        //    {
        //        _logIndex = (UInt16)Math.Max(_logIndex - 50, 0);
        //        logDoubleBufferPanel.Invalidate();
        //        return;
        //    }

        //    //ScrollLogIndex();
        //    //if(e.Type == ScrollEventType.ThumbTrack)
        //    {
        //        if (e.NewValue == 0)
        //        {
        //            //_scrollLogTimer.Enabled = false;
        //            _logIndex = 0;
        //            logDoubleBufferPanel.Invalidate();
        //            return;
        //        }
        //        if (e.NewValue == 41)
        //        {
        //            //_scrollLogTimer.Enabled = false;
        //            Int32 idx = _logs.Count - (logDoubleBufferPanel.Height / 40);
        //            _logIndex = (idx >= 0)
        //                            ? (UInt16)idx : (UInt16)0;
        //            logDoubleBufferPanel.Invalidate();
        //            return;
        //        }

        //        //_scrollLogTimer.Enabled = true;
        //        if (_moveLogSpeed == 0)
        //            _moveLogSpeed = (Int16)(e.NewValue - e.OldValue);
        //        else if (e.NewValue - e.OldValue < 0)
        //        {
        //            if (_moveLogSpeed > 0)
        //                _moveLogSpeed = (Int16)(e.NewValue - e.OldValue);
        //            else
        //                _moveLogSpeed += (Int16)(e.NewValue - e.OldValue);
        //        }
        //        else
        //        {
        //            if (_moveLogSpeed < 0)
        //                _moveLogSpeed = (Int16)(e.NewValue - e.OldValue);
        //            else
        //                _moveLogSpeed += (Int16)(e.NewValue - e.OldValue);
        //        }
        //    }
        //}

        //private void ScrollLogIndex()
        //{
        //    if(_logs.Count == 0) return;

        //    UInt16 index = _logIndex;
        //    //up
        //    if (_moveLogSpeed < 0)
        //    {
        //        if (_logIndex + _moveLogSpeed > 0)
        //            _logIndex = Convert.ToUInt16(_logIndex + _moveLogSpeed);
        //        else
        //        {
        //            _logIndex = 0;
        //        }
        //    }
        //    else//down
        //    {
        //        if ((_logs.Count - (_logIndex + _moveLogSpeed)) >= (logDoubleBufferPanel.Height / 40))
        //            _logIndex = Convert.ToUInt16(_logIndex + _moveLogSpeed);
        //        else
        //        {
        //            Int32 idx = _logs.Count - (logDoubleBufferPanel.Height/40);
        //            _logIndex = (idx  >= 0)
        //                            ? (UInt16)idx : (UInt16)0;
        //        }
        //    }

        //    if (index == _logIndex) return;
        //    logDoubleBufferPanel.Invalidate();
        //}

        //private void ResetScroll(Object sender, EventArgs e)
        //{
        //    _resetScrollTimer.Enabled = false;
        //    if(_logScrollBar.Value == 0 && _logIndex != 0)
        //        _logScrollBar.Value = 1;
        //    else if (_logScrollBar.Value == 41 && (_logIndex + logDoubleBufferPanel.Height / 40) < _logs.Count)
        //        _logScrollBar.Value = 40;
        //}

        //private void ScrollLogIndex(Object sender, EventArgs e)
        //{
        //    ScrollLogIndex();
        //}

        private delegate List<Log> LoadLogDelegate(DateTime dateTime, LogType[] types);
        private void LoadLog(Object sender, EventArgs e)
        {
            _loadLogTimer.Enabled = false;
            LoadLog();
        }

        private void HideLoading(Object sender, EventArgs e)
        {
            _hideLoadingTimer.Enabled = false;

            ApplicationForms.HideLoadingIcon();
            logDoubleBufferPanel.Focus();
        }

        private LogPanel GetEventLog()
        {
            if (StoredLog.Count > 0)
            {
                return StoredLog.Dequeue();
            }

            return new LogPanel();
        }

        public readonly Queue<LogPanel> StoredLog = new Queue<LogPanel>();
        private readonly String[] _previousFilter = new String[4];
        private void LoadLog()
        {
            if (string.IsNullOrEmpty(Server.Credential.Domain)) return;

            if (!datePicker.Enabled) return;

            ClearLog();

            _previousFilter[0] = SystemFilter;
            _previousFilter[1] = SystemDescFilter;
            _previousFilter[2] = UserFilter;
            _previousFilter[3] = UserDescFilter;

            //filterSystemLogComboBox.Items.Clear();
            filterSystemDescComboBox.Items.Clear();
            //filterUserLogComboBox.Items.Clear();
            filterUserDescComboBox.Items.Clear();

            _watch.Reset();
            _watch.Start();
            //_dotTimer.Enabled = true;
            _logIndex = 0;
            //_logScrollBar.Value = 0;
            //_dot = "";
            _logs.Clear();
            //logDoubleBufferPanel.Invalidate();

            foundLabel.Text = Localization["SetupLog_LoadingLog"];

            containerPanel.Visible =
            serverPictureBox.Enabled = actionPictureBox.Enabled = datePicker.Enabled = false;

            logFilterPanel.Enabled = false;

            ApplicationForms.ShowLoadingIcon(Server.Form);
            Application.RaiseIdle(null);

            LoadLogDelegate loadLogDelegate = LoadLog;

            loadLogDelegate.BeginInvoke(datePicker.Value, _showLogTypes.ToArray(), LoadLogCallback, loadLogDelegate);

            logDoubleBufferPanel.Focus();
        }

        protected virtual List<Log> LoadLog(DateTime dateTime, LogType[] types)
        {
            return Server.Server.LoadLog(dateTime, types);
        }

        //private String _dot = "";
        //private delegate void DotLogLabelDelegate(Object sender, EventArgs e);
        //private void DotLogLabel(Object sender, EventArgs e)
        //{
        //    //if (InvokeRequired)
        //    //{
        //    //    Invoke(new DotLogLabelDelegate(DotLogLabel), sender, e);
        //    //    return;
        //    //}

        //    if (_dot.Length > 20)
        //        _dot = ".";

        //    _dot += ".";

        //    logLabel.Text = Localization["SetupLog_LoadingLog"] + _dot;
        //}

        private delegate void LoadLogCallbackDelegate(IAsyncResult result);

        private List<Log> _logs = new List<Log>();
        private List<Log> _logResults;
        private UInt16 _logIndex;
        private void LoadLogCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new LoadLogCallbackDelegate(LoadLogCallback), result);
                }
                catch (Exception)
                {
                }
                return;
            }

            _logResults = ((LoadLogDelegate)result.AsyncState).EndInvoke(result);

            _logs.AddRange(_logResults);

            _watch.Stop();

            _page = 1;
            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(_logs.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();

            Boolean fullDesc = (_showLogTypes.Contains(LogType.Server) && _showLogTypes.Count > 1);

            foreach (Log log in _logs)
                log.FullDescription = fullDesc;

            if (_logs.Count > 0)
            {
                if (String.Join("", _previousFilter).Length == 0)
                    UpdateLabelLogFound(_watch.Elapsed.TotalSeconds);

                //_logScrollBar.Enabled = (_logs.Count > logDoubleBufferPanel.Height / 40);

                var serverfilters = GetServerfilters();
                var userfilters = GetUserfilters();

                //keep last selection, filter result then display it.
                FilterLogWithCondition();

                filterSystemLogComboBox.Items.Clear();
                filterUserLogComboBox.Items.Clear();
                //----------------------------

                var serverDescfilters = GetServerDescfilters();
                var userDescfilters = GetUserDescfilters();

                serverfilters.Sort();
                serverDescfilters.Sort();

                userfilters.Sort();
                userDescfilters.Sort();

                filterSystemLogComboBox.SelectedIndexChanged -= FilterSystemLogComboBoxIndexChanged;
                filterSystemDescComboBox.SelectedIndexChanged -= FilterComboBoxIndexChanged;
                filterUserLogComboBox.SelectedIndexChanged -= FilterUserLogComboBoxIndexChanged;
                filterUserDescComboBox.SelectedIndexChanged -= FilterComboBoxIndexChanged;
                //----------------------------------------------------------
                if (serverfilters.Count > 0)
                {
                    filterSystemLogComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                    filterSystemLogComboBox.Enabled = true;
                    foreach (String filter in serverfilters)
                    {
                        filterSystemLogComboBox.Items.Add(filter);
                    }
                }
                else
                {
                    filterSystemLogComboBox.Items.Clear();
                    filterSystemLogComboBox.Items.Add(Localization["SetupLog_NoSystemLog"]);
                    filterSystemLogComboBox.Enabled = false;
                }
                //----------------------------------------------------------
                if (serverDescfilters.Count > 0)
                {
                    filterSystemDescComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                    filterSystemDescComboBox.Enabled = true;
                    foreach (String filter in serverDescfilters)
                    {
                        filterSystemDescComboBox.Items.Add(filter);
                    }
                }
                else
                {
                    filterSystemDescComboBox.Items.Add("");
                    filterSystemDescComboBox.Enabled = false;
                }
                //----------------------------------------------------------
                if (userfilters.Count > 0)
                {
                    filterUserLogComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                    filterUserLogComboBox.Enabled = true;
                    foreach (String filter in userfilters)
                    {
                        filterUserLogComboBox.Items.Add(filter);
                    }
                }
                else
                {
                    filterUserLogComboBox.Items.Clear();
                    filterUserLogComboBox.Items.Add(Localization["SetupLog_NoUserLog"]);
                    filterUserLogComboBox.Enabled = false;
                }
                //----------------------------------------------------------
                if (userDescfilters.Count > 0)
                {
                    filterUserDescComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                    filterUserDescComboBox.Enabled = true;
                    foreach (String filter in userDescfilters)
                    {
                        filterUserDescComboBox.Items.Add(filter);
                    }
                }
                else
                {
                    filterUserDescComboBox.Items.Add("");
                    filterUserDescComboBox.Enabled = false;
                }
                //----------------------------------------------------------
                SetPreviousFilter(serverfilters, 0, filterSystemLogComboBox);
                Manager.DropDownWidth(filterSystemLogComboBox);
                filterSystemLogComboBox.SelectedIndexChanged += FilterSystemLogComboBoxIndexChanged;

                //----------------------------------------------------------
                SetPreviousFilter(serverDescfilters, 1, filterSystemDescComboBox);
                Manager.DropDownWidth(filterSystemDescComboBox);
                filterSystemDescComboBox.SelectedIndexChanged += FilterComboBoxIndexChanged;

                //----------------------------------------------------------
                SetPreviousFilter(userfilters, 2, filterUserLogComboBox);
                Manager.DropDownWidth(filterUserLogComboBox);
                filterUserLogComboBox.SelectedIndexChanged += FilterUserLogComboBoxIndexChanged;

                //----------------------------------------------------------
                SetPreviousFilter(userDescfilters, 3, filterUserDescComboBox);
                Manager.DropDownWidth(filterUserDescComboBox);
                filterUserDescComboBox.SelectedIndexChanged += FilterComboBoxIndexChanged;

                if (String.Join("", _previousFilter).Length > 0)
                {
                    FilterLogWithCondition();

                    UpdateLabelLogFound(_watch.Elapsed.TotalSeconds);
                }

                if (_logs.Count > 0)
                {
                    containerPanel.Visible = true;
                    logFilterPanel.Enabled = true;
                }
                else
                {
                    containerPanel.Visible = false;
                    logFilterPanel.Enabled = false;
                }
            }
            else
            {
                filterSystemLogComboBox.Items.Clear();
                filterSystemLogComboBox.Items.Add(Localization["SetupLog_NoSystemLog"]);
                filterSystemLogComboBox.SelectedIndex = 0;
                filterSystemLogComboBox.Enabled = false;

                filterSystemDescComboBox.Items.Add("");
                filterSystemDescComboBox.Enabled = false;

                filterUserLogComboBox.Items.Clear();
                filterUserLogComboBox.Items.Add(Localization["SetupLog_NoUserLog"]);
                filterUserLogComboBox.SelectedIndex = 0;
                filterUserLogComboBox.Enabled = false;

                filterUserDescComboBox.Items.Add("");
                filterUserDescComboBox.Enabled = false;

                UpdateLabelLogFound(_watch.Elapsed.TotalSeconds);
                containerPanel.Visible = false;
                logFilterPanel.Enabled = false;
            }

            if (OnSelectionChange != null)
            {
                var xml = Manager.SelectionChangedXml(TitleName, TitleName, "", (_logs.Count > 0) ? "SaveAs" : "");
                OnSelectionChange(this, new EventArgs<String>(xml));
            }

            serverPictureBox.Enabled = actionPictureBox.Enabled = datePicker.Enabled = true;

            //at least show loading for 0.5 sec (else it just flash)
            if (_watch.Elapsed.TotalMilliseconds > 500)
            {
                ApplicationForms.HideLoadingIcon();
                logDoubleBufferPanel.Focus();
            }
            else
                _hideLoadingTimer.Enabled = true;
        }

        private DateTime _previousDateTime;
        private void DatePickerDropDown(Object sender, EventArgs e)
        {
            _previousDateTime = datePicker.Value;
            datePicker.ValueChanged -= DatePickerValueChanged;
        }

        private void DatePickerValueChanged(Object sender, EventArgs e)
        {
            WaitToGetLog();
        }

        private void DatePickerCloseUp(Object sender, EventArgs e)
        {
            if (_previousDateTime != datePicker.Value)
                WaitToGetLog();
            datePicker.ValueChanged -= DatePickerValueChanged;
            datePicker.ValueChanged += DatePickerValueChanged;
        }

        private void SetPreviousFilter(List<String> filters, UInt16 id, ComboBox comboBox)
        {
            Boolean hasfilter = false;
            if (_previousFilter[id] != "")
            {
                if (filters.Contains(_previousFilter[id]))
                {
                    hasfilter = true;
                    comboBox.SelectedIndex = comboBox.Items.IndexOf(_previousFilter[id]);
                }
            }

            if (!hasfilter)
            {
                if (comboBox.Items.Count == 2)
                {
                    comboBox.SelectedIndex = 1;
                    comboBox.Enabled = false;
                }
                else
                    comboBox.SelectedIndex = 0;
            }
            else
            {
                if (comboBox.Items.Count == 2)
                    comboBox.Enabled = false;
            }
        }

        protected void WaitToGetLog()
        {
            _loadLogTimer.Enabled = false;
            _loadLogTimer.Enabled = true;

            filterSystemLogComboBox.Enabled = filterSystemDescComboBox.Enabled = filterUserLogComboBox.Enabled = filterUserDescComboBox.Enabled = false;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(
                    Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            //auto search when activate this page
            if (_logs.Count == 0)
            {
                LoadLog();
            }

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(
                    Manager.SelectionChangedXml(TitleName, TitleName, "", (_logs.Count > 0) ? "SaveAs" : "")));
        }

        private void ServerPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (_showLogTypes.Contains(LogType.Server))
            {
                _showLogTypes.Remove(LogType.Server);
                serverPictureBox.BackgroundImage = null;
            }
            else
            {
                _showLogTypes.Add(LogType.Server);
                serverPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.service_bg, Properties.Resources.IMGServiceBg);
            }

            WaitToGetLog();
        }

        private void ActionPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (_showLogTypes.Contains(LogType.Action))
            {
                _showLogTypes.Remove(LogType.Action);
                _showLogTypes.Remove(LogType.Operation);
                actionPictureBox.BackgroundImage = null;
            }
            else
            {
                _showLogTypes.Add(LogType.Action);
                _showLogTypes.Add(LogType.Operation);
                actionPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.action_bg, Properties.Resources.IMGActionBg);
            }

            WaitToGetLog();
        }

        private void LogInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, logPanel);

            if (logPanel.Width <= 100) return;

            Manager.PaintText(g, Localization["SetupLog_Log"]);
        }

        private void LogFilterPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, logFilterPanel);
        }

        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.PaintTitleTopInput(g, titlePanel);

            if (Width <= 200) return;
            Manager.PaintText(g, Localization["SetupLog_ID"], Manager.TitleTextColor);

            g.DrawString(Localization["SetupLog_Time"], Manager.Font, Manager.TitleTextColor, 90, 13);

            if (_showLogTypes.Count > 0)
            {
                if (_showLogTypes.Contains(LogType.Server))
                {

                    if (Width <= 310) return;
                    g.DrawString(Localization["SetupLog_System"], Manager.Font, Manager.TitleTextColor, 180, 13);

                    if ((_showLogTypes.Contains(LogType.Action)) || (_showLogTypes.Contains(LogType.Operation)))
                    {
                        if (Width <= 420) return;
                        g.DrawString(Localization["SetupLog_User"], Manager.Font, Manager.TitleTextColor, 300, 13);

                        if (Width <= 550) return;
                        g.DrawString(Localization["SetupLog_Description"], Manager.Font, Manager.TitleTextColor, 440, 13);
                    }
                    else
                    {
                        if (Width <= 430) return;
                        g.DrawString(Localization["SetupLog_Description"], Manager.Font, Manager.TitleTextColor, 320, 13);
                    }
                }
                else
                {
                    if (Width <= 310) return;
                    g.DrawString(Localization["SetupLog_User"], Manager.Font, Manager.TitleTextColor, 180, 13);

                    if (Width <= 430) return;
                    g.DrawString(Localization["SetupLog_Description"], Manager.Font, Manager.TitleTextColor, 320, 13);
                }
            }
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            //else //dont hide self to keep at last selection panel on screen
            //    Minimize();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }
}
