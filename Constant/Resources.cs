using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Constant
{
    public class Resources
    {
        //if external resources available, replace internal resources.
        public static Image GetResources(Image internalResources, String externalResourcesPath)
        {
            if (File.Exists(externalResourcesPath))
            {
                try
                {
                    return Image.FromFile(externalResourcesPath);
                }
                catch (Exception)
                {
                    return internalResources;
                }
            }

            return internalResources;
        }

        //if external resources(.ico) available, replace internal resources.
        public static Icon GetResourcesForIcon(Icon internalResources, String externalResourcesPath)
        {
            if (File.Exists(externalResourcesPath))
            {
                try
                {
                    return Icon.ExtractAssociatedIcon(externalResourcesPath);
                }
                catch (Exception)
                {
                    return internalResources;
                }
            }

            return internalResources;
        }
        
        //<Projects>
        //    <Project name="Revert">
        //        <Objects>
        //            <Object name="buttonRevert">
        //                <Size width="100" height="100" />
        //                <Location x="100" y="100" />
        //                <BackColor>#626262</BackColor>
        //            </Object>
        //        </Objects>
        //    </Project>
        //    <Project name="Save">
        //        <Objects>
        //            <Object name="buttonSave">
        //                <Dock>Left</Dock>
        //                <Size width="100" height="100" />
        //            </Object>
        //            <Object name="autoSizeLabel">
        //                <Location x="100" y="100" />
        //            </Object>
        //        </Objects>
        //    </Project>
        //</Projects>
        public static void ParseResources(Control target, String xmlFile, String projectName)
        {
            if(!File.Exists(xmlFile)) return;
            List<Control> controlList = ReadChildren(target);

            try
            {
                XmlDocument xmlDoc = Xml.LoadXmlFromFile(xmlFile);
                XmlNodeList projectList = xmlDoc.GetElementsByTagName("Project");

                foreach (XmlElement projectNode in projectList)
                {
                    if (!String.Equals(projectNode.GetAttribute("name"), projectName)) continue;

                    XmlNodeList objectList = projectNode.GetElementsByTagName("Object");

                    foreach (XmlElement objectNode in objectList)
                    {
                        String objectName = objectNode.GetAttribute("name");
                        Control control = controlList.Find(obj => (String.Equals(obj.Name, objectName)));
                        if (control == null) continue;

                        UpdateDock(control, Xml.GetFirstElementValueByTagName(objectNode, "Dock"));
                        UpdateSize(control, Xml.GetFirstElementByTagName(objectNode, "Size"));
                        UpdateLocation(control, Xml.GetFirstElementByTagName(objectNode, "Location"));
                        UpdateBackColor(control, Xml.GetFirstElementValueByTagName(objectNode, "BackColor"));
                    }
                }
            }
            catch(Exception)
            {
            }
        }

        private static List<Control> ReadChildren(Control item)
        {
            var result = new List<Control>();

            foreach (Control control in item.Controls)
            {
                if (String.IsNullOrEmpty(control.Name)) continue;
                result.Add(control);
                if (control.HasChildren)
                {
                    result.AddRange(ReadChildren(control));
                }
            }

            return result;
        }

        private static readonly CultureInfo _enus = new CultureInfo("en-US");
        private static void UpdateDock(Control control, String dockStr)
        {
            if (String.IsNullOrEmpty(dockStr)) return;

            dockStr = dockStr.ToLower(_enus);
            switch (dockStr)
            {
                case "fill":
                    control.Dock = DockStyle.Fill;
                    break;

                case "none":
                    control.Dock = DockStyle.None;
                    break;

                case "left":
                    control.Dock = DockStyle.Left;
                    break;

                case "right":
                    control.Dock = DockStyle.Right;
                    break;

                case "top":
                    control.Dock = DockStyle.Top;
                    break;

                case "bottom":
                    control.Dock = DockStyle.Bottom;
                    break;
            }
        }

        private static void UpdateSize(Control control, XmlElement sizeNode)
        {
            if (sizeNode == null) return;

            String width = sizeNode.GetAttribute("width");
            if (!String.IsNullOrEmpty(width))
                control.Width = Convert.ToInt32(width);

            String height = sizeNode.GetAttribute("height");
            if (!String.IsNullOrEmpty(height))
                control.Height = Convert.ToInt32(height);
        }

        private static void UpdateLocation(Control control, XmlElement locationNode)
        {
            if (locationNode == null) return;

            String x = locationNode.GetAttribute("x");
            Point point = new Point(control.Location.X, control.Location.Y);
            if (!String.IsNullOrEmpty(x))
                point.X = Convert.ToInt32(x);

            String y = locationNode.GetAttribute("y");
            if (!String.IsNullOrEmpty(y))
                point.Y = Convert.ToInt32(y);

            control.Location = point;
        }

        private static void UpdateBackColor(Control control, String backColorStr)
        {
            if (String.IsNullOrEmpty(backColorStr)) return;

            control.BackColor = ColorTranslator.FromHtml(backColorStr);
        }
    }
}
