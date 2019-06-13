using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Constant.Utility;
using Interface;

namespace Layout
{
    public class ControlPanel : Panel, IControlPanel
    {
        public event EventHandler OnMinimizeChange;

        private IBlockPanel _blockPanel;
        public IBlockPanel BlockPanel
        {
            set
            {
                _blockPanel = value;
                if (value.LayoutManager.Page != null)
                {
                    _app = value.LayoutManager.Page.App;
                    _server = value.LayoutManager.Page.Server;
                }
            }
        }

        private IApp _app;
        private IServer _server;
        private Boolean _isAutoHeight = true;
        //public Dictionary<String, String> Localization;

        public XmlElement ControlNode
        {
            set { LoadConfig(value); }
        }

        public ControlPanel()
        {
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            Padding = new Padding(0);
            Margin = new Padding(0);
        }

        public sealed override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        public Boolean IsAutoHeight { get { return _isAutoHeight && !IsMinimize; } }
        public Boolean IsMinimize
        {
            get
            {
                return Control is IMinimize ? ((IMinimize)Control).IsMinimize : false;
            }
        }

        public Boolean IsDragable
        {
            get
            {
                return Control is IDrag ? ((IDrag)Control).DragDropProxy != null : false;
            }
        }

        private Int32 _fixedHeight;

        public IControl Control { get; set; }

        public Button Icon
        {
            get
            {
                var control = Control as IMinimize;
                if (control != null)
                {
                    return control.Icon;
                }

                return null;
            }
        }

        private static readonly Dictionary<String, Assembly> ClassDictionary = new Dictionary<String, Assembly>();
        private void LoadConfig(XmlElement controlNode)
        {
            var dockValue = Xml.GetFirstElementValueByTagName(controlNode, "Dock");
            if (!string.IsNullOrEmpty(dockValue))
            {
                var dock = dockValue.ToEnum(DockStyle.Top);
                Dock = dock;
            }

            var height = Xml.GetFirstElementValueByTagName(controlNode, "Height");
            if (height != "auto")
            {
                _isAutoHeight = false;
                _fixedHeight = Height = Convert.ToInt32(height);
            }

            var asmName = Xml.GetFirstElementValueByTagName(controlNode, "Assembly");
            if (asmName == "") return;
            if (!File.Exists(asmName)) return;

            var className = Xml.GetFirstElementValueByTagName(controlNode, "ClassName");
            Assembly asm;
            if (ClassDictionary.ContainsKey(asmName))
            {
                asm = ClassDictionary[asmName];
            }
            else
            {
                asm = Assembly.LoadFrom(asmName);
                ClassDictionary.Add(asmName, asm);
            }
            try
            {
                Control = Activator.CreateInstance(asm.GetType(className)) as IControl;
            }
            catch (Exception)
            {
                Control = null;
            }
            //Assembly asm = Assembly.LoadFrom(asmName);
            //Control = Activator.CreateInstance(asm.GetType(className)) as IControl;

            if (Control == null) return;

            var control = Control as Control;
            if (control != null)
            {
                Controls.Add(control);
            }

            if (Control is IAppUse)
                ((IAppUse)Control).App = _app;

            if (Control is IServerUse)
                ((IServerUse)Control).Server = _server;

            if (Control is IBlockPanelUse)
                ((IBlockPanelUse)Control).BlockPanel = _blockPanel;

            Control.Initialize();

            var name = Xml.GetFirstElementValueByTagName(controlNode, "Name");
            Control.Name = name;
            if (String.IsNullOrEmpty(Control.TitleName))
                Control.TitleName = Control.Name;

            if (Control is IDrag)
                _blockPanel.LayoutManager.DragDropProxy.Add(((IDrag)Control).DragDropProxy);

            //String key = "Control_" + name.Replace(" ", "");
            //Control.TitleName = (Localization.ContainsKey(key))
            //    ? Localization[key] : name;

            if (Control is IMinimize)
            {
                ((IMinimize)Control).OnMinimizeChange += ObjectOnMinimizeChange;
            }
            //else //look like doesn't matter
            //{
            //    VisibleChanged += ObjectOnVisibleChanged;
            //}
        }

        public void Activate()
        {
            if (Control != null)
                Control.Activate();
        }

        public void Deactivate()
        {
            if (Control != null)
                Control.Deactivate();
        }

        //private void ObjectOnVisibleChanged(Object sender, EventArgs e)
        //{
        //    if (Visible && OnMinimizeChange != null)
        //        OnMinimizeChange(this, null);
        //}

        private void ObjectOnMinimizeChange(Object sender, EventArgs e)
        {
            var minimize = Control as IMinimize;
            if (minimize == null) return;

            if (IsMinimize)
            {
                if (_blockPanel.IsDockable)
                {
                    Visible = false;
                }
                else
                {
                    Height = minimize.MinimizeHeight;
                }
            }
            else
            {
                if (_blockPanel.IsDockable)
                {
                    Visible = true;
                }
                else
                {
                    if (_fixedHeight != 0)
                        Height = _fixedHeight;
                }
            }

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }
}
