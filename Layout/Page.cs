using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Behavior;
using Constant;
using Interface;
using PanelBase;

namespace Layout
{
    public class Page : IPage
    {
        public String Name { get; set; }
        public String TitleName { get; set; }
        public IApp App { get; set; }
        public IServer Server { get; set; }
        public ILayoutManager Layout { get; protected set; }
        public IBehaviorManager Behavior { get; protected set; }

        public Button Icon { get; private set; }
        public Panel Content { get; private set; }
        public Panel Function { get; private set; }
        public String Version { get; private set; }


        protected const String PageName = @"app/{PAGE}";
        protected const String PluginsPageName = @"plug-ins/{PAGE}";
        private String _xmlFilePath;
        private XmlElement _pageNode;
        public XmlElement PageNode
        {
            get { return _pageNode; }
            set
            {
                _pageNode = value;

                String configFile = Xml.GetFirstElementValueByTagName(_pageNode, "Config");
                _xmlFilePath = PageName.Replace("{PAGE}", configFile);
                IsExists = (File.Exists(_xmlFilePath));

                //check if it's plug-ins page
                if (!IsExists)
                {
                    _xmlFilePath = PluginsPageName.Replace("{PAGE}", configFile);
                    IsExists = (File.Exists(_xmlFilePath));
                    IsPlugins = true;
                }
            }
        }

        public Dictionary<String, String> Localization;

        public Boolean IsExists { get; protected set; }
        public Boolean IsInitialize { get; private set; }
        public Boolean IsActivate { get; private set; }
        public Boolean IsCoolDown { get; private set; }
        public Boolean IsPlugins { get; private set; }

        protected Image NormalStateImage;
        protected Image ActivateStateImage;

        public Page()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Page_Live", "Live"},
								   {"Page_Playback", "Playback"},
								   {"Page_SmartSearch", "Smart Search"},
								   {"Page_Investigation", "Investigation"},
								   {"Page_Report", "Report"},
								   {"Page_eMap", "eMap"},
								   {"Page_Event", "Event"},
								   {"Page_Setup", "Setup"},
								   {"Page_Interrogation", "Interrogation"},
								   {"Page_InterrogationSearch", "Interrogation Search"},
								   
								   {"Page_SwitchPage", "Switch page to %1"}
							   };
            Localizations.Update(Localization);

            IsExists = false;
            IsCoolDown = true;
            IsPlugins = false;

            Version = "1.0";

            Icon = new Button
                       {
                           BackColor = Color.Transparent,
                           Cursor = Cursors.Hand,
                           FlatStyle = FlatStyle.Flat,
                       };

            Icon.FlatAppearance.BorderSize = 0;
            Icon.FlatAppearance.CheckedBackColor = Color.Transparent;
            Icon.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Icon.FlatAppearance.MouseOverBackColor = Color.Transparent;

            Content = new Panel
            {
                Dock = DockStyle.Fill,
            };

            Function = new PanelBase.DoubleBufferPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            _coolDownTimer.Elapsed += PageCoolDown;
            _coolDownTimer.Interval = 500;
        }

        private readonly List<IFocus> _focusList = new List<IFocus>();
        public List<IFocus> FocusList
        {
            get { return _focusList; }
        }

        private readonly List<IKeyPress> _keyPressList = new List<IKeyPress>();
        public List<IKeyPress> KeyPressList
        {
            get { return _keyPressList; }
        }

        protected readonly List<IDrop> _dragList = new List<IDrop>();
        protected readonly List<IMouseHandler> _mouseHandler = new List<IMouseHandler>();
        public List<IMouseHandler> MouseHandler
        {
            get { return _mouseHandler; }
        }

        public virtual void LoadConfig()
        {
            //String configFile = Xml.GetFirstElementValueByTagName(PageNode, "Config");

            if (!File.Exists(_xmlFilePath)) return;

            IsExists = true;

            XmlDocument configNode = Xml.LoadXmlFromFile(_xmlFilePath);

            //check xml's version
            var pageNode = configNode["Page"];

            if (pageNode != null)
            {
                var version = pageNode.GetAttribute("ver");
                if (!String.IsNullOrEmpty(version))
                    Version = version;
            }

            Name = Xml.GetFirstElementValueByTagName(configNode, "PageName");
            TitleName = (Localization.ContainsKey("Page_" + Name.Replace(" ", ""))
                             ? Localization["Page_" + Name.Replace(" ", "")]
                             : Name);

            CreatePageIcon();

            //使用XML來定義Lauout結構，要使用的物件及擺放的位置
            if (Layout == null)
            {
                Layout = LoadLayoutManager(configNode);
            }

            //使用XML來定義物件之間的關連
            Behavior = new BehaviorManager
            {
                App = App,
                Layout = Layout,
                ConfigNode = configNode,
            };

            foreach (BlockPanel block in Layout.BlockPanels)
            {
                foreach (ControlPanel controlPanel in block.ControlPanels)
                {
                    if (controlPanel.Control == null) continue;

                    if (controlPanel.Control is IFocus)
                        _focusList.Add(((IFocus)controlPanel.Control));

                    if (controlPanel.Control is IDrop)
                        _dragList.Add(((IDrop)controlPanel.Control));

                    if (controlPanel.Control is IKeyPress)
                        _keyPressList.Add(((IKeyPress)controlPanel.Control));

                    if (controlPanel.Control is IMouseHandler)
                        _mouseHandler.Add(((IMouseHandler)controlPanel.Control));
                }
            }
        }

        private void CreatePageIcon()
        {
            if (Version == "1.0")
            {
                Icon.Dock = DockStyle.Top;
                Icon.Size = new Size(75, 75);
                Icon.BackgroundImageLayout = ImageLayout.Center;

                SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

                switch (Name)
                {
                    case "Live":
                        NormalStateImage = Resources.GetResources(Properties.Resources.live, Properties.Resources.IMGLive);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.live_activate, Properties.Resources.IMGLiveActivate);
                        break;

                    case "Playback":
                        NormalStateImage = Resources.GetResources(Properties.Resources.playback, Properties.Resources.IMGPlayback);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.playback_activate, Properties.Resources.IMGPlaybackActivate);
                        break;

                    case "Investigation":
                        NormalStateImage = Resources.GetResources(Properties.Resources.smartSearch, Properties.Resources.IMGSmartSearch);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.smartSearch_activate, Properties.Resources.IMGSmartSearchActivate);
                        break;

                    case "Route Search":
                        NormalStateImage = Resources.GetResources(Properties.Resources.routesearch, Properties.Resources.IMGRouteSearch);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.routesearch_activate, Properties.Resources.IMGRouteSearchActivate);
                        break;

                    case "Map Tracker":
                        NormalStateImage = Resources.GetResources(Properties.Resources.tracker, Properties.Resources.IMGTracker);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.tracker_activate, Properties.Resources.IMGTrackerActivate);
                        break;

                    case "Report":
                        NormalStateImage = Resources.GetResources(Properties.Resources.report, Properties.Resources.IMGReport);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.report_activate, Properties.Resources.IMGReportActivate);
                        break;

                    case "eMap":
                        NormalStateImage = Resources.GetResources(Properties.Resources.emap, Properties.Resources.IMGEmap);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.emap_activate, Properties.Resources.IMGEmapActivate);
                        break;

                    case "Event":
                        NormalStateImage = Resources.GetResources(Properties.Resources._event, Properties.Resources.IMGEvent);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.event_activate, Properties.Resources.IMGEventActivate);
                        break;

                    case "Setup":
                        NormalStateImage = Resources.GetResources(Properties.Resources.setup, Properties.Resources.IMGSetup);
                        ActivateStateImage = Resources.GetResources(Properties.Resources.setup_activate, Properties.Resources.IMGSetupActivate);
                        break;
                }

                Icon.Click += PageButtonOnClick;
                Icon.Image = NormalStateImage;

                return;
            }

            if (Version == "2.0")
            {
                Icon.Padding = new Padding(0);

                Icon.TextAlign = ContentAlignment.MiddleCenter;
                Icon.Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Icon.ForeColor = Color.WhiteSmoke;
                Icon.Dock = DockStyle.Left;
                Icon.Size = new Size(170, 39);
                Icon.BackgroundImageLayout = ImageLayout.None;
                Icon.Text = TitleName;

                var pages = Server.Server.PageList.Keys.ToList();
                pages.Remove("Setup"); //remove setup from page switch panel

                if (pages.Count > 0)
                {
                    //first page
                    if (pages.First() == Name)
                    {
                        NormalStateImage = Properties.Resources.pageLeft;
                        ActivateStateImage = Properties.Resources.pageLeftActivate;
                    }
                    else if (pages.Last() == Name) //last page
                    {
                        NormalStateImage = Properties.Resources.pageRight;
                        ActivateStateImage = Properties.Resources.pageRightActivate;
                    }
                    else //center
                    {
                        NormalStateImage = Properties.Resources.pageCenter;
                        ActivateStateImage = Properties.Resources.pageCenterActivate;
                    }

                    Icon.Click += PageButtonOnClick;
                    Icon.Image = NormalStateImage;
                }

            }
        }

        protected virtual LayoutManager LoadLayoutManager(XmlDocument configNode)
        {
            return new LayoutManager
            {
                Page = this,
                ConfigNode = configNode,
            };
        }

        private readonly System.Timers.Timer _coolDownTimer = new System.Timers.Timer();

        private void PageCoolDown(Object sender, EventArgs e)
        {
            _coolDownTimer.Enabled = false;

            IsCoolDown = true;
        }

        protected virtual void PageButtonOnClick(Object sender, EventArgs e)
        {
            App.UpdateClientSetting(RestoreClientColumn.PageName, Name, null);

            App.Activate(this);
        }

        public void Initialize()
        {
            if (Layout == null) return;

            IsInitialize = true;

            //foreach (BlockPanel block in Layout.BlockPanels.Reverse<IBlockPanel>())
            foreach (BlockPanel block in Layout.BlockPanels)
            {
                //why add such THING here?
                //if(block.Dock == DockStyle.Left)
                //{
                //    Content.Controls.Add(new Panel
                //    {
                //        Dock = DockStyle.Left,
                //        BackColor = Color.DimGray,
                //        Width = 1,
                //    });
                //}

                Content.Controls.Add(block);

                foreach (ControlPanel controlPanel in block.ControlPanels)
                {
                    if (!controlPanel.IsDragable) continue;

                    ((IDrag)controlPanel.Control).OnDragStart += App.DragStart;
                }
            }

            foreach (IControl control in Layout.Function)
            {
                Function.Controls.Add((Control)control);
            }

            foreach (Panel ddproxy in Layout.DragDropProxy)
            {
                Content.Controls.Add(ddproxy);
            }
        }

        public virtual void Activate()
        {
            if (Layout == null) return;

            Icon.Image = ActivateStateImage;

            if (!IsInitialize)
                return;

            IsActivate = true;

            Layout.Activate();
        }

        public virtual void Deactivate()
        {
            if (Layout == null) return;

            Icon.Image = NormalStateImage;

            if (!IsInitialize) return;

            IsActivate = false;

            //_watch.Reset();
            //_watch.Start();

            Layout.Deactivate();

            //_watch.Stop();
            //Console.WriteLine(Name + " Deactivate: " + _watch.Elapsed.TotalSeconds);

            IsCoolDown = false;
            _coolDownTimer.Enabled = true;
        }

        public void HidePanel()
        {
            if (Layout == null) return;

            foreach (BlockPanel blockPanel in Layout.BlockPanels)
            {
                if (!blockPanel.IsAutoWidth)
                    blockPanel.Hide();
            }
        }

        public void ShowPanel()
        {
            if (Layout == null) return;

            foreach (BlockPanel blockPanel in Layout.BlockPanels)
            {
                blockPanel.Show();
            }
        }

        public void CheckDragDataType(List<IDrop> container, Object dragObj)
        {
            container.AddRange(_dragList.Where(drop => drop.CheckDragDataType(dragObj)));
        }
    }
}
