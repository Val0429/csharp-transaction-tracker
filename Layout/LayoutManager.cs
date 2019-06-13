using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace Layout
{
    public class LayoutManager : ILayoutManager
    {
        public IPage Page { get; set; }
        public Dictionary<String, String> Localization;

        public List<IBlockPanel> BlockPanels { get; private set; }
        public List<IControl> Function { get; private set; }
        public List<ToolStripMenuItem> Menus { get; private set; }
        public List<Panel> DragDropProxy { get; private set; }

        public DockStyle SidePanelDockStyle { get; private set; }
        public Int32 SidePanelWidth { get; private set; }
        public Int32 FunctionPanelHeight { get; private set; }

        public XmlDocument ConfigNode
        {
            set { LoadConfigNode(value); }
        }

        public LayoutManager()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Menu_VideoTitleBar", "Video Title Bar"},
                                   {"Menu_AutoDropFrame", "Auto Drop Frame"},
                                   {"Menu_DecodeI-frame", "Decode I-frame"},
                                   {"Menu_SaveImage", "Save Images"},
                                   {"Menu_Disconnect", "Disconnect"},
                                   {"Menu_DisconnectAll", "Disconnect All"},
                                   {"Menu_SetupMap", "Setup Map"},
                                   {"Menu_ExportCaseQueue", "Export Case Queue"},
                                   {"Menu_RemoteConnectionMode", "Remote Connection Mode"},
                               };
            Localizations.Update(Localization);

            BlockPanels = new List<IBlockPanel>();
            Function = new List<IControl>();
            Menus = new List<ToolStripMenuItem>();
            DragDropProxy = new List<Panel>();

            SidePanelDockStyle = DockStyle.Left; //default
            SidePanelWidth = 70; //default
            FunctionPanelHeight = 210;//default
        }

        private static readonly Dictionary<String, Assembly> ClassDictionary = new Dictionary<String, Assembly>();
        protected void LoadConfigNode(XmlDocument configNode)
        {
            CreateBlocks(configNode);

            #region Function Block
            var functionRootNode = Xml.GetFirstElementByTagName(configNode, "Functions");

            var widthStr = Xml.GetFirstElementValueByTagName(functionRootNode, "Width");
            var heightStr = Xml.GetFirstElementValueByTagName(functionRootNode, "Height");
            var dockStr = Xml.GetFirstElementValueByTagName(functionRootNode, "Dock");

            if (!String.IsNullOrEmpty(widthStr))
            {
                SidePanelWidth = Convert.ToInt32(widthStr);
            }

            if (!String.IsNullOrEmpty(heightStr))
            {
                FunctionPanelHeight = Convert.ToInt32(heightStr);
            }

            if (!String.IsNullOrEmpty(dockStr))
            {
                switch (dockStr)
                {
                    case "right":
                        SidePanelDockStyle = DockStyle.Right;
                        break;

                    default:
                        SidePanelDockStyle = DockStyle.Left;
                        break;
                }
            }

            var functions = functionRootNode.GetElementsByTagName("Function");
            if (functions.Count > 0)
            {
                foreach (XmlElement node in functions)
                {
                    String name = Xml.GetFirstElementValueByTagName(node, "Name");
                    //if (name == "Broadcast" && Page.Server is ICMS)
                    //    continue;

                    String asmName = Xml.GetFirstElementValueByTagName(node, "Assembly");
                    if (!File.Exists(asmName)) continue;

                    Assembly assemblyDll;

                    if (ClassDictionary.ContainsKey(asmName))
                    {
                        assemblyDll = ClassDictionary[asmName];
                    }
                    else
                    {
                        assemblyDll = Assembly.LoadFrom(asmName);
                        ClassDictionary.Add(asmName, assemblyDll);
                    }
                    if (assemblyDll == null) continue;

                    IControl control;

                    try
                    {
                        String className = Xml.GetFirstElementValueByTagName(node, "ClassName");
                        control = Activator.CreateInstance(assemblyDll.GetType(className)) as IControl;

                        Logger.Current.InfoFormat("Create {0}.", control);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Logger.Current.Error(ex);
                        continue;
                    }

                    if (control != null)
                    {
                        if (control is IAppUse)
                            ((IAppUse)control).App = Page.App;

                        if (control is IServerUse)
                            ((IServerUse)control).Server = Page.Server;

                        control.Initialize();
                        control.Name = name;

                        //String key = "Control_" + name.Replace(" ", "");
                        //control.TitleName = (Localization.ContainsKey(key))
                        //    ? Localization[key] : name;

                        if (control is IDrag)
                            DragDropProxy.Add(((IDrag)control).DragDropProxy);

                        Function.Add(control);
                    }
                }
            }
            #endregion

            var menus = configNode.GetElementsByTagName("Menu");
            if (menus.Count <= 0) return;

            foreach (XmlElement node in menus)
            {
                if (node.InnerText == "Setup Map" && !Page.Server.User.Current.Group.CheckPermission("Setup", Permission.Access))
                    continue;

                var menu = new ToolStripMenuItemUI2
                {
                    Name = node.InnerText
                };

                String key = "Menu_" + menu.Name.Replace(" ", "");
                menu.Text = (Localization.ContainsKey(key))
                                ? Localization[key] : menu.Name;

                Menus.Add(menu);
            }
        }

        protected virtual void CreateBlocks(XmlDocument configNode)
        {
            var blocks = configNode.GetElementsByTagName("Block");
            foreach (XmlElement node in blocks)
            {
                var blockPanel = CreateBlockPanel(node);

                if (blockPanel.Dock != DockStyle.Fill)
                {
                    BlockPanels.Add(blockPanel);
                }
                else
                {
                    BlockPanels.Insert(0, blockPanel);
                }
            }
        }

        protected virtual IBlockPanel CreateBlockPanel(XmlElement node)
        {
            var blockPanel = new BlockPanel
            {
                LayoutManager = this,
                BlockNode = node,
            };

            return blockPanel;
        }

        public void Activate()
        {
            foreach (IBlockPanel block in BlockPanels)
            {
                block.Activate();
            }
        }

        public void Deactivate()
        {
            foreach (IBlockPanel block in BlockPanels)
                block.Deactivate();
        }

        public void Refresh()
        {
            if (!Page.IsActivate) return;

            foreach (IBlockPanel block in BlockPanels)
                block.RefreshComponent();
        }
    }

    class Logger
    {
        private static ILogger _current;
        public static ILogger Current { get { return _current ?? (_current = LoggerManager.Instance.GetLogger()); } }

        private Logger() { }
    }
}