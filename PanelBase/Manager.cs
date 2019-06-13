using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;

namespace PanelBase
{
    public static partial class Manager
    {
        public static String SelectionChangedXml(String from, String item, String previous, String buttons)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "From", from));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Item", item));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Previous", previous));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Buttons", buttons));

            return xmlDoc.InnerXml;
        }

        public static Boolean ParseSelectionChange(String xml,  String titleName, out String item)
        {
            XmlDocument xmlDoc = Xml.LoadXml(xml);

            String from = Xml.GetFirstElementValueByTagName(xmlDoc, "From");

            if (from != titleName)
            {
                item = "";
                return false;
            }

            item = Xml.GetFirstElementValueByTagName(xmlDoc, "Item");

            return true;
        }

        //-------------------------------------------------------------------------------------------------------------

        public static void ReplaceControl(Control oldControl, Control newControl, Control container, Action action = null)
        {
            if (newControl == null) return;

            if (oldControl == newControl)
            {
                if (action != null)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception)
                    {
                    }
                }
                //if (ReplaceControlComplete != null)
                //    ReplaceControlComplete(newControl, null);
            }

            newControl.Dock = DockStyle.Fill;
            newControl.Size = container.Size;
            newControl.Location = new Point(0, 0);
            container.Controls.Add(newControl);
            newControl.BringToFront();

            container.Controls.Remove(oldControl);

            if (action != null)
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                }
            }
            //if (ReplaceControlComplete != null)
            //    ReplaceControlComplete(newControl, null);
        }

        //-------------------------------------------------------------------------------------------------------------

        private static readonly Queue<Button> StoredButton = new Queue<Button>();
        private static readonly List<Button> UsingButton = new List<Button>();
        public static Button RegistButton()
        {
            Button button;
            if (StoredButton.Count > 0)
            {
                button = StoredButton.Dequeue();
            }
            else
            {
                button = new Button
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand,
                    //FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Margin = new Padding(0, 3, 5, 0),
                    Padding = new Padding(5, 0, 5, 0),
                    TabStop = false,
                };
            }
            UsingButton.Add(button);

            return button;
        }

        public static void UnregistButton(Button button)
        {
            UsingButton.Remove(button);

            if (!StoredButton.Contains(button))
                StoredButton.Enqueue(button);
        }

        public static void DropDownWidth(ComboBox comboBox)
        {
            Int32 maxWidth = comboBox.Width;

            foreach (var obj in comboBox.Items)
            {
                maxWidth = Math.Max(TextRenderer.MeasureText(obj.ToString(), comboBox.Font).Width, maxWidth);
            }
            comboBox.DropDownWidth = maxWidth;
        }
    }
}
