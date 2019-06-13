using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace Layout
{
    public class BlockPanel : Panel, IBlockPanel
    {
        protected IPage Page { get; set; }
        private ILayoutManager _layoutManager;
        public ILayoutManager LayoutManager
        {
            get { return _layoutManager; }
            set
            {
                _layoutManager = value;
                Page = value.Page;
            }
        }

        public XmlElement BlockNode
        {
            set { LoadConfig(value); }
        }

        public Boolean IsDockable { get; private set; }
        public Boolean IsAutoWidth { get; protected set; }
        public Boolean IsDragable { get { return false; } }

        protected readonly DoubleBufferPanel ControlPanel = new DoubleBufferPanel();
        protected Panel DockPanel;

        public List<IControlPanel> SyncDisplayControlList { get; private set; }


        // Constructor
        public BlockPanel()
        {
            IsAutoWidth = false;
            DoubleBuffered = true;
            IsDockable = false;
            BackColor = Color.FromArgb(29, 32, 37);
            Margin = new Padding(0);
            Padding = new Padding(1, 0, 0, 0);

            ControlPanel.Dock = DockStyle.Fill;
            Controls.Add(ControlPanel);

            SyncDisplayControlList = new List<IControlPanel>();
            ControlPanels = new List<IControlPanel>();
            SizeChanged += BlockPanelSizeChanged;
        }

        public List<IControlPanel> ControlPanels { get; private set; }

        //private readonly Stopwatch _watch = new Stopwatch();
        public void Activate()
        {
            foreach (IControlPanel control in ControlPanels)
            {
                //_watch.Reset();
                //_watch.Start();

                control.Activate();

                //_watch.Stop();
                //Console.WriteLine(control.Control.TitleName + " Activate: " + _watch.Elapsed.TotalSeconds);
            }
        }

        public void Deactivate()
        {
            foreach (IControlPanel control in ControlPanels)
                control.Deactivate();
        }

        private const UInt16 MinimumHeight = 30;
        public void RefreshComponent()
        {
            if (_layoutManager == null || _layoutManager.Page == null || !_layoutManager.Page.IsActivate) return;

            Int32 autoHeightCount = ControlPanels.Count(control => control.IsAutoHeight && control.Visible);

            if (autoHeightCount == 0) return;

            Int32 totalFixedHeight = ControlPanels.Where(control => !control.IsAutoHeight && control.Visible)
                .Cast<ControlPanel>().Sum(control => control.Height);

            if (DockPanel != null)
                totalFixedHeight += DockPanel.Height;

            if (Height < totalFixedHeight) return;

            UInt16 autoHeight = Math.Max(Convert.ToUInt16((Height - totalFixedHeight) / autoHeightCount), MinimumHeight);

            if (autoHeight <= 0) return;

            foreach (var control in ControlPanels.OfType<ControlPanel>())
            {
                if (control.IsAutoHeight && control.Visible)
                    control.Height = autoHeight;
            }
        }

        protected Int32 WidthFromConfig;
        protected virtual void LoadConfig(XmlElement blockNode)
        {
            String width = Xml.GetFirstElementValueByTagName(blockNode, "Width");
            String dock = Xml.GetFirstElementValueByTagName(blockNode, "Dock");

            if (width != "auto")
            {
                WidthFromConfig = Width = Convert.ToInt32(width);
                switch (dock)
                {
                    case "left":
                        Dock = DockStyle.Left;
                        break;

                    case "right":
                        Dock = DockStyle.Right;
                        break;

                    default:
                        Dock = DockStyle.Fill;
                        break;
                }
            }
            else
            {
                IsAutoWidth = true;
                Dock = DockStyle.Fill;
            }

            var controls = blockNode.GetElementsByTagName("Control");
            //Int32 count = 0;
            foreach (XmlElement node in controls)
            {
                if (LayoutManager.Page.Name == "Setup")
                {
                    String controlName = Xml.GetFirstElementValueByTagName(node, "Name");

                    Boolean hasPermission = GetHasPermission(controlName);

                    if (!hasPermission) continue;
                }

                var controlPanel = new ControlPanel
                {
                    Width = Width,
                    BlockPanel = this,
                    ControlNode = node
                };

                if (controlPanel.Control != null)
                    ControlPanels.Add(controlPanel);
            }

            if (Page.Version == "1.0")
            {
                XmlNode controlDockNode = blockNode.SelectSingleNode("ControlDock");

                if (controlDockNode != null)
                {
                    IsDockable = true;
                    DockPanel = new Panel
                    {
                        Dock = DockStyle.Bottom,
                        Height = Convert.ToInt32(Xml.GetFirstElementValueByTagName(controlDockNode, "Height")),
                    };
                    Controls.Add(DockPanel);
                }
            }
            else
            {
                IsDockable = (Xml.GetFirstElementValueByTagName(blockNode, "Dockable") == "true");
            }

            foreach (IControlPanel controlPanel in ControlPanels)
            {
                var control = controlPanel as Control;
                if (control == null) continue;

                if (DockPanel != null && controlPanel.Icon != null)
                    DockPanel.Controls.Add(controlPanel.Icon);

                if (IsDockable && Page.Version == "2.0")
                {
                    //simply set active control to dock fill, no need caculator each control's height
                    if (controlPanel.Control is IMinimize && controlPanel.Control is Control)
                    {
                        (controlPanel.Control as Control).Dock = DockStyle.Fill;
                    }
                }
                controlPanel.OnMinimizeChange += BlockPanelSizeChanged;

                ControlPanel.Controls.Add(control);
                (control).BringToFront();
            }
        }

        protected virtual bool GetHasPermission(String controlName)
        {
            Boolean hasPermission = false;
            switch (controlName)
            {
                case "Title":
                case "IconTitle":
                    hasPermission = true;
                    break;

                case "Server":
                case "Server Icon":
                    hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Server));
                    break;

                case "NVR":
                case "NVR Icon":
                    hasPermission = (Page.Server is ICMS || Page.Server is IVAS || Page.Server is IFOS || Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.NVR));
                    break;

                case "Device":
                case "Device Icon":
                    hasPermission = (Page.Server is INVR && !(Page.Server is ICMS));
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Device));
                    break;

                case "Device Layout":
                case "Device Layout Icon":
                    if (Page.App.isSupportImageStitching)
                    {
                        hasPermission = (Page.Server is INVR && !(Page.Server is ICMS));
                        hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.ImageStitching));
                    }
                    else
                        hasPermission = false;
                    break;

                //case "Device Group":
                //case "Device Group Icon":
                //    hasPermission = (Page.Server is INVR);
                //    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.DeviceGroup));
                //    break;

                case "Exception":
                case "Exception Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Exception));
                    break;

                case "POS":
                case "POS Icon":
                case "Generic POS Setting":
                case "Generic POS Setting Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.POS));
                    break;

                case "POS Connection":
                case "POS Connection Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.POSConnection));
                    break;

                case "Division":
                case "Division Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Division));
                    hasPermission = true;
                    break;

                case "Region":
                case "Region Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Region));
                    hasPermission = true;
                    break;

                case "Store":
                case "Store Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Store));
                    hasPermission = true;
                    break;
                    
                case "PeopleCounting":
                case "PeopleCounting Icon":
                    hasPermission = (Page.Server is IVAS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.PeopleCounting));
                    break;

                case "Event":
                case "Event Icon":
                    hasPermission = ((Page.Server is INVR || Page.Server is IFOS));//&& !(Page.Server is ICMS)
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Event));
                    break;

                case "Schedule":
                case "Schedule Icon":
                    hasPermission = ((Page.Server is INVR || Page.Server is IVAS));
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Schedule));
                    break;

                case "Exception Report":
                case "Exception Report Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.ExceptionReport));
                    break;

                case "Schedule Report":
                case "Schedule Report Icon":
                    hasPermission = (Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.ScheduleReport));
                    break;

                case "General":
                case "General Icon":
                    hasPermission = (Page.Server is INVR || Page.Server is IFOS || Page.Server is IPTS);
                    hasPermission = (hasPermission && Page.Server.User.Current.Group.CheckPermission("Setup", Permission.General));
                    break;

                case "User":
                case "User Icon":
                    hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.User));
                    break;

                case "Joystick":
                case "Joystick Icon":
                    //if (IntPtr.Size == 4 && Page.Server is INVR) //only 32bit support joystick
                    hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Joystick));
                    break;

                case "License":
                case "License Icon":
                    hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.License));
                    break;

                case "Log":
                case "Log Icon":
                    hasPermission = (Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Log));
                    break;
            }
            return hasPermission;
        }

        private IControlPanel _focusedIControlPanel;
        private Boolean _ignoreCheckBlockVisible;
        public void ShowThisControlPanel(Control control)
        {
            IControlPanel panel = ControlPanels.FirstOrDefault(controlPanel => controlPanel.Control == control);
            if (panel == null) return;

            Width = WidthFromConfig;

            _focusedIControlPanel = panel;
            //ControlPanel.Visible = false;

            //----------------------------------------------
            _ignoreCheckBlockVisible = true;
            foreach (IControlPanel controlPanel in SyncDisplayControlList)
            {
                if (controlPanel.Control == control) continue;
                var minimize = controlPanel.Control as IMinimize;

                if (minimize != null && !minimize.IsMinimize)
                {
                    minimize.Minimize();
                }
            }
            _ignoreCheckBlockVisible = false;
            //----------------------------------------------
            //check not auto hide control and caculator it's height (like setupbase title)

            var size = ControlPanel.Size;
            if (ControlPanels.Count != SyncDisplayControlList.Count)
            {
                foreach (var controlPanel in ControlPanels)
                {
                    if (SyncDisplayControlList.Contains(controlPanel)) continue;

                    size.Height -= ((Control)controlPanel).Height;
                }
            }

            panel.Visible = true;
            control.Size = panel.Size = size;

            //ControlPanel.Visible = true;

            panel.Activate();
        }

        public void HideThisControlPanel(Control control)
        {
            IControlPanel panel = ControlPanels.FirstOrDefault(controlPanel => controlPanel.Control == control);
            if (panel == null) return;

            foreach (IControlPanel controlPanel in SyncDisplayControlList)
            {
                if (controlPanel.Control == control)
                {
                    controlPanel.Visible = false;
                    //dont deactive control, if control need deactive, should call it self after hide panel
                    //controlPanel.Deactivate();
                    continue;
                }
            }

            if (_ignoreCheckBlockVisible) return;

            if (!Page.IsActivate) return;

            if (_focusedIControlPanel == panel && IsDockable && Page.Version == "2.0")
            {
                _focusedIControlPanel = null;
                Width = 0;
            }
        }

        public Boolean IsFocusedControl(Control control)
        {
            IControlPanel panel = ControlPanels.FirstOrDefault(controlPanel => controlPanel.Control == control);
            if (panel == null) return false;

            return (_focusedIControlPanel == panel);
        }

        protected void BlockPanelSizeChanged(Object sender, EventArgs e)
        {
            RefreshComponent();
        }
    }
}
