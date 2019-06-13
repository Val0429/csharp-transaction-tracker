using Interface;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Behavior
{
    public class BehaviorManager : IBehaviorManager
    {
        private ILayoutManager _layout;
        public ILayoutManager Layout { set { _layout = value; } }

        public IApp App { get; set; }

        public XmlDocument ConfigNode
        {
            set { LoadConfigNode((XmlElement)value.SelectSingleNode("Page")); }
        }


        public BehaviorManager()
        {
            
        }


        private void LoadConfigNode(XmlElement configNode)
        {
            if (configNode == null) return;

            var behaviors = configNode.GetElementsByTagName("Behavior");

            if (behaviors.Count > 0)
            {
                foreach (XmlElement behaviorNode in behaviors)
                {
                    var triggers = behaviorNode.GetElementsByTagName("Trigger");
                    var receivers = behaviorNode.GetElementsByTagName("Receiver");
                    foreach (XmlElement triggerNode in triggers)
                    {
                        String tgr = triggerNode.GetAttribute("name");

                        foreach (XmlElement receiverNode in receivers)
                        {
                            String rcv = receiverNode.GetAttribute("name");

                            AddEventHandler(FindObjectViaName(tgr), FindObjectViaName(rcv), triggerNode.InnerText, receiverNode.InnerText);
                        }
                    }
                }
            }
            XmlElement initialize = (XmlElement)configNode.SelectSingleNode("Initialize");
            if (initialize != null)
            {
                var controls = initialize.GetElementsByTagName("Control");
                foreach (XmlElement control in controls)
                {
                    String obj = control.GetAttribute("name");
                    InvokeMethod(FindObjectViaName(obj), control.InnerText);
                }
            }
        }

        private Object FindObjectViaName(String name)
        {
            // ==== Modify By Tulip
            // For Plugin Xml to turn On/Off Plugin module
            if (name == "App")
                return App;

            foreach (IBlockPanel block in _layout.BlockPanels)
            {
                foreach (IControlPanel control in block.ControlPanels)
                {
                    if (control == null || control.Control == null) continue;

                    if (control.Control.Name == name)
                        return control.Control;
                }
            }

            foreach (var control in _layout.Function)
            {
                if (control.Name == name)
                    return control;
            }

            var target = _layout.Menus.FirstOrDefault(menu => menu.Name == name);

            Trace.WriteLineIf(target == null, "Control " + name + " NOT FOUND");

            return target;
        }

        private static void InvokeMethod(Object obj, String methodName)
        {
            if (obj == null) return;

            MethodInfo methodInfo = obj.GetType().GetMethod(methodName, new Type[] { });

            if (methodInfo == null) return;

            try
            {
                methodInfo.Invoke(obj, null);
            }
            catch (Exception)
            {
                Trace.WriteLine("Invoke " + methodName + " error");
            }
        }

        private static void AddEventHandler(Object triggerObj, Object receiverObj, String eventName, String handlerName)
        {
            //http://msdn.microsoft.com/en-us/library/system.reflection.eventinfo.addeventhandler.aspx

            if (triggerObj == null || receiverObj == null)
            {
                return;
            }

            EventInfo eventinfo = triggerObj.GetType().GetEvent(eventName);

            if (eventinfo == null)
            {
                Trace.WriteLine("Event: " + eventName + " NOT FOUND");
                return;
            }

            try
            {
                var handler = Delegate.CreateDelegate(eventinfo.EventHandlerType, receiverObj, handlerName);
                eventinfo.AddEventHandler(triggerObj, handler);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("AddEventHandler " + handlerName + " error" + ex.Message);
            }
        }
    }
}
